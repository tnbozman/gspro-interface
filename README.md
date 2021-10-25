# Release
- 0.0.1 - [Initial Release](https://github.com/tnbozman/gspro-interface/releases/tag/0.01) 

# GSPro Client Interface

This project provides a C# implementation of the GSPro Connect Client.
This has been implemented using a TCP Client Stream using dotnet core 5.

## Client Interface
GSProStreamInterface has been implemented to provided to allow Launch Monitor interfaces to be developed and use this 
interface to communicate with GSPro Connect.

The GSProStreamInterface has been implemented in a manner to abstract away the complexity of the GSPro Connect socket 
connection and provide a simple communications interface that is capable of interfacing with launch monitor interfaces and 
to be developed into a winform UI application.

The interface is provided below:

```
namespace GSProInterface.Services
{
    public interface IGSProInterface
    {
        /// <summary>
        /// Status of the underlying socket connection
        /// </summary>
        Status Status { get; }

        /// <summary>
        ///  signals the client has successfully established a socket connection to GSPro Connect
        /// </summary>
        event Action<IGSProInterface> ClientConnected;
        /// <summary>
        /// signals that the socket to GSPro Connect has been disconnected
        /// </summary>
        event Action<IGSProInterface> ClientDisconnected;
        /// <summary>
        /// signals that a response to a shot has been received from GSPro Connect
        /// NOTE: This event is not thread safe see details below
        /// </summary>
        event Action<IGSProInterface, ResponseDto> ShotReceived;
        /// <summary>
        /// signals that a response to play information change in GSPro has been received from GSPro Connect
        /// NOTE: This event is not thread safe see details below
        /// </summary>
        event Action<IGSProInterface, ResponseDto> PlayerInformationReceived;
        /// <summary>
        /// An error has occured
        /// NOTE: This event is not thread safe see details below
        /// </summary>
        event Action<IGSProInterface, string> ErrorDetected;

        /// <summary>
        /// Attempt to make a connection to GSPro Connect using the provided ip address and port
        /// Successful connection will raise the ClientConnected event
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        void StartClient(string address, int port);
        /// <summary>
        /// Successful disconnection will raise the ClientDisconnected event
        /// </summary>
        void StopClient();
        /// <summary>
        /// Send a launch monitor status update to GSPro Connect
        /// </summary>
        /// <param name="launchMonitorIsReady"></param>
        void SendLaunchMonitorStatus(bool LaunchMonitorIsReady);
        /// <summary>
        /// Send a shot to GSPro Connect that contains ball data and retrieve the shot's GSPro response
        /// </summary>
        /// <param name="ballData"></param>
        /// <returns></returns>
        ResponseDto SendBallData(BallDataDto ballData);
        /// <summary>
        /// Send a shot to GSPro Connect that contains club data and retrieve the shot's GSPro response
        /// </summary>
        /// <param name="clubData"></param>
        /// <returns></returns>
        ResponseDto SendClubData(ClubDataDto shotData);
        /// <summary>
        /// Send a shot to GSPro Connect that contains ball and club data and retrieve the shot's GSPro response
        /// </summary>
        /// <param name="ballData"></param>
        /// <param name="clubData"></param>
        /// <returns></returns>
        ResponseDto SendBallAndClubData(BallDataDto ballData, ClubDataDto shotData);
    }
}
```

## Thread Events

The follow events are not thread safe and need to be marshalled to the main thread:
- PlayerInformationReceived (receive thread)
- ShotReceived (recieve thread)
- ErrorDetected (could be any thread - send, receive, main)

An example can be found below for the PlayerInformationReceived event:

```
        delegate void SetPlayerInformationCallback(ResponseDto response);

        private void SetPlayerInformation(ResponseDto response)
        {
            if (this.log.InvokeRequired)
            {
                SetPlayerInformationCallback d = new SetPlayerInformationCallback(SetPlayerInformation);
                this.Invoke(d, new object[] { response });
            }
            else
            {
                if (response != null && response.Player != null)
                {
                    if (!string.IsNullOrEmpty(response.Player.Club))
                        this.club_selection.Text = response.Player.Club;
                    if (!string.IsNullOrEmpty(response.Player.Handed))
                        this.player_handed.Text = response.Player.Handed;

                }
            }
        }

        private void OnPlayerInformationChange(IGSProInterface intf, ResponseDto response){
            SetPlayerInformation(response);
        }
```


# Issues Raised With GSPro
- Launch Monitor Status
- Passing and Id in the requests which is returned in the responses so that response can be matched to requests
- Socket State Management (GSPro currently does support disconnect/reconnect)
- Shot Reponse message error in format
- Launch Monitor Status 
