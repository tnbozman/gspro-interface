using GSProInterface.Models;
using GSProInterface.Models.enums;
using GSProInterface.Models.Messages;
using GSProInterface.Models.Reponse;
using GSProInterface.Models.Request;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// This implementation is derived from the following article https://www.codeproject.com/Articles/884583/Advanced-TCP-Socket-Programming-with-NET

namespace GSProInterface.Services
{
    public class StreamClientAdvanced : IStreamClient
    {
        private Thread receivingThread;
        private Thread sendingThread;

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        private readonly ILogger<IStreamClient> _logger;
        #region Properties
        public string Address { get; private set; }
        /// <summary>
        /// The Port that is used to connect to the remote server.
        /// </summary>
        public int Port { get; private set; }

        public Status Status { get; private set; }
        /// <summary>
        /// List containing all messages that is waiting to be delivered to the remote client/server
        /// </summary>
        public BlockingCollection<ShotMessage> SendMessageQueue { get; private set; }
        public Queue<ResponseMessage> ReceiveMessageQueue { get; private set; }
        #endregion

        #region Events
        /// <summary>
        /// Raises when the client was disconnected;
        /// </summary>
        public event Action<StreamClientAdvanced> ClientConnected;
        /// <summary>
        /// Raises when the client was disconnected;
        /// </summary>
        public event Action<StreamClientAdvanced> ClientDisconnected;
        /// <summary>
        /// Raises when a new response was received by the remote session client.
        /// </summary>
        public event Action<StreamClientAdvanced, ResponseDto> ShotReceived;
        /// <summary>
        /// Raises when a new response was received by the remote session client.
        /// </summary>
        public event Action<StreamClientAdvanced, ResponseDto> PlayerInformationReceived;
        /// <summary>
        /// Raises when a new response was received by the remote session client.
        /// </summary>
        public event Action<StreamClientAdvanced, string> ErrorDetected;

        #endregion

        #region Constructors
        public StreamClientAdvanced(ILogger<IStreamClient> logger)
        {
            _logger = logger;
            SendMessageQueue = new BlockingCollection<ShotMessage>();
            ReceiveMessageQueue = new Queue<ResponseMessage>();
            Status = Status.Disconnected;
        }
        #endregion

        #region Methods
        /// <summary>
        /// The TcpClient that is encapsulated by this client instance.
        /// </summary>
        public Socket client { get; set; }
        public void Connect(String address, int port)
        {
            Address = address;
            Port = port;
            IPAddress ipAddress = IPAddress.Parse(address);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Blocking = true;
            //IPEndPoint remoteEP = new IPEndPoint(SessionSingleton.IpAddress, SessionSingleton.Port);
            // Create a TCP/IP socket.  
            //Socket client = new Socket(SessionSingleton.IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();
            Status = Status.Connected;
            _logger.LogDebug($"Connection to {address}:{port} successful");

            receivingThread = new Thread(ReceivingMethod);
            receivingThread.IsBackground = true;
            
            sendingThread = new Thread(SendingMethod);
            sendingThread.IsBackground = true;
        }

        public void Start()
        {
            _logger.LogDebug($"Starting receiving thread.");
            receivingThread.Start();
            _logger.LogDebug($"Starting sending thread.");
            sendingThread.Start();
        }

        /// <summary>
        /// Disconnect from the remote server.
        /// </summary>
        public void Disconnect()
        {
            ReceiveMessageQueue.Clear();
            while(SendMessageQueue.Count > 0)
            {
                ShotMessage dummy;
                SendMessageQueue.TryTake(out dummy, TimeSpan.FromMilliseconds(10));
            }
            
            Status = Status.Disconnected;
            // Release the socket.  
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            OnClientDisconnected();
            _logger.LogDebug($"Connection disconnected.");
        }

        public void Send(ShotDataDto message)
        {
            _logger.LogDebug($"Sending message.");
            SendMessageQueue.Add(new ShotMessage(message));
        }

        public ResponseDto Receive()
        {
            if(ReceiveMessageQueue.Count > 0)
            {
                _logger.LogDebug($"Receiving message.");
                return ReceiveMessageQueue.Dequeue().Payload;
            }
            _logger.LogDebug($"No message found to return within receive message.");
            return null;
        }

        #endregion

        #region Threads Methods
        private void SendingMethod(object obj)
        {
            _logger.LogDebug($"Sending thread started.");
            int reattempts = 1;
            ShotMessage msg = null;
            while (Status != Status.Disconnected)
            {
                
                // if the msg is null make a blocking take, if msg is not null due to re-attempt reuse the msg
                if(msg == null)
                    msg = SendMessageQueue.Take();

                _logger.LogDebug($"Sending message within sending thread.");
                var data = JsonConvert.SerializeObject(msg.Payload);

                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                try
                {
                    // Begin sending the data to the remote device.  
                    client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client); sendDone.WaitOne();
                    sendDone.WaitOne();
                    reattempts = 1;
                    // set msg to null to cause the next read to be blocking
                    msg = null;
                    _logger.LogDebug($"Message sent.");
                }
                catch(Exception ex)
                {
                    var errMsg = $"Send Attempt {reattempts} failed.";
                    _logger.LogError(errMsg);
                    OnErrorDetected(errMsg);
                    OnErrorDetected(ex.Message);
                    if (reattempts == 3)
                    {
                        _logger.LogError($"Sending failed reattempt limit, disconnecting.");
                        Disconnect();
                    }
                    reattempts++;
                }
            }
            Thread.Sleep(100);
            
        }
        // https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket.receive?view=net-5.0
        private void ReceivingMethod(object obj)
        {
            try
            {
                _logger.LogDebug($"Receiving thread started.");
                while (Status != Status.Disconnected)
                {
                    // Create the state object.  
                    StateObject state = new StateObject();
                    state.workSocket = client;

                    // Begin receiving the data from the remote device.  
                    int bytesRead = client.Receive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None);

                    _logger.LogDebug("bytes read > 0");
                    if (bytesRead > 0)
                    {
                        // There might be more data, so store the data received so far.  
                        var response = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                        // Get the rest of the data.  
                        _logger.LogDebug(response);
                        var payload = JsonConvert.DeserializeObject<ResponseDto>(response);
                        MessageBase msg;
                        if (payload == null)
                        {
                            _logger.LogError("Received message payload was not valid and parsed to null.");
                            msg = new ErrorMessage(new ResponseException("Response failed to deserialise to a Response."));
                        }
                        else
                        {
                            msg = new ResponseMessage(payload);
                        }
                        OnMessageReceived(msg);
                        // Signal that all bytes have been received.  
                        receiveDone.Set();
                    }

                    receiveDone.WaitOne();
                    _logger.LogDebug($"Message Received.");
                    Thread.Sleep(100);
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Callbacks
        private void ConnectCallback(IAsyncResult connectionResult)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)connectionResult.AsyncState;

                // Complete the connection.  
                client.EndConnect(connectionResult);

                _logger.LogDebug("Connection to {0} achieved.", client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to connect to GSPro Connect: Make sure GSPro Connect has been started and is waiting for connection");
                _logger.LogError(e.ToString());
            }
        }
        
        private void SendCallback(IAsyncResult sendResult)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)sendResult.AsyncState;
                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(sendResult);
                _logger.LogDebug("Sent {0} bytes to server.", bytesSent);
                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        #endregion

        #region Raise Events

        protected virtual void OnMessageReceived(MessageBase msg)
        {
            if (msg.HasError)
            {
                _logger.LogDebug($"Received message has an error.");
                _logger.LogDebug(msg.Exception.Message);
                OnErrorDetected(msg.Exception.Message);
                return;
            }

            if (msg is ResponseMessage)
            {
                var response = (msg as ResponseMessage);
                _logger.LogDebug($"Received message queued.");
                ReceiveMessageQueue.Enqueue(response);
                OnResponseReceived(response.Payload);
            }
        }

        protected virtual void OnClientConnected()
        {
            _logger.LogDebug($"ClientConnected triggered.");
            if (ClientConnected != null) ClientConnected(this);
        }

        protected virtual void OnClientDisconnected()
        {
            _logger.LogDebug($"ClientDisconnected triggered.");
            if (ClientDisconnected != null) ClientDisconnected(this);
        }

        protected virtual void OnResponseReceived(ResponseDto response)
        {
            if(response.Code == (int)ResponseCodes.SHOT_SUCCESS)
            {
                _logger.LogDebug($"ShotReceived triggered.");
                if (ShotReceived != null) ShotReceived(this, response);
            }else if (response.Code == (int)ResponseCodes.PLAYER_INFO)
            {
                _logger.LogDebug($"PlayerInformationReceived triggered.");
                if (PlayerInformationReceived != null) PlayerInformationReceived(this, response);
            }
            else
            {
                _logger.LogDebug($"ErrorDetected triggered from OnResponseReceived.");
                if (ErrorDetected != null) ErrorDetected(this, $"GSPro Error Code Received Code: {response.Code} - Message = {response.Message}");
            }
            
        }

        protected virtual void OnErrorDetected(string errorMessage)
        {
            _logger.LogDebug($"ErrorDetected triggered.");
            if (ErrorDetected != null) ErrorDetected(this, errorMessage);
        }
        #endregion
    }
}
