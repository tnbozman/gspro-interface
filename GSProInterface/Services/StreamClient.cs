using GSProInterface.Models;
using GSProInterface.Models.Reponse;
using GSProInterface.Models.Request;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSProInterface.Services
{
    public class StreamClient: IStreamClient
    {
        // ManualResetEvent instances signal completion.
        private Socket client = null;
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        private Queue<string> response = new Queue<string>();
        private readonly ILogger<IStreamClient> _logger;

        public StreamClient(ILogger<IStreamClient> logger)
        {
            _logger = logger;
        }

        public void Connect(string address, int port)
        {
            // Connect to a remote device.
            // Establish the remote endpoint for the socket.  
            // The name of the
            // remote device is "host.contoso.com". 

            IPAddress ipAddress = IPAddress.Parse(address);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //IPEndPoint remoteEP = new IPEndPoint(SessionSingleton.IpAddress, SessionSingleton.Port);
            // Create a TCP/IP socket.  
            //Socket client = new Socket(SessionSingleton.IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();
            _logger.LogDebug($"Connection to {address}:{port} successful");
        }

        private void ConnectCallback(IAsyncResult connectionResult)
        {
            try
            {
                // Retrieve the socket from the state object.  
                client = (Socket)connectionResult.AsyncState;

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

        public void Disconnect()
        {
            // Release the socket.  
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public void Send(ShotDataDto data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
            sendDone.WaitOne();
            _logger.LogDebug($"Send complete.");
        }

        private void SendCallback(IAsyncResult sendResult)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)sendResult.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(sendResult);
                _logger.LogDebug($"Sent { bytesSent} bytes to server.");

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        public ResponseDto Receive()
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
                receiveDone.WaitOne();
                // need to clean up
                var result = JsonConvert.DeserializeObject<ResponseDto>(response.Dequeue());
                if (result == null)
                {
                    throw new Exception("Response Empty");
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        private void ReceiveCallback(IAsyncResult receiveResult)
        {
            try
            {
                _logger.LogDebug("Entered ReceiveCallback");
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)receiveResult.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(receiveResult);

                _logger.LogDebug("bytes read > 0");
                if(bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    // Get the rest of the data.  
                    _logger.LogDebug(state.sb.ToString());
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response.Enqueue(state.sb.ToString());
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }
    }
}
