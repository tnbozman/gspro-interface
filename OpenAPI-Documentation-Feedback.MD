# GSPro Open Connect v1 Documentation
The following describes how to connect to the GSPro Connect interface with the “Open Connect” protocol, implemented using a tcp server.

## Summary
The overall process breaks down very simply; GSPro Connect opens a socket and listens for an incoming client. Once a connection is established with a client constant 2-way communication continues:

#### To GSPro Connect from Launch Monitor client:
- Shot Data
- Ready signals (currently not implemented)
- Heart beat (retired)

#### From GSPro to Client
- General response from incoming messages (success/failure)
- Player information

## Socket information

The socket connection is open and does not require authentication. 

Note: It is assumed that the launch monitor software is being ran on the same PC as GSPro Connect. If this is not the case, firewall and port forwarding may be required

- Port: 0921
- IP Address: 127.0.0.1
 
## Launch Monitor to GSPro Connect JSON
JSON is used as the format to communicate between GSPro Connect (server) and the Launch Monitor Client.

The JSON interface used for the Launch Monitor Client to communicate with GSPro Connect is shown below.
Within the interface it is demontrated that there are three important sections; root properties, BallData object and ShotDataOptions object.
The root properties are used to describe general properities (device id, units, shot number, api version), the BallData object represents the ball data used to describe a golf shot and
the ShotDataOptions describe the GSPro Connect what data is being sent.

```
{
"DeviceID": "GSPro LM 1.1",  			//required - unqiue per launch monitor / project type
"Units": "Yards",						//optional - default yards
"ShotNumber": 13,						//required - auto increment from LM
"APIversion": "1",						//required - "1" is current version
"BallData": {		
	"Speed": 147.5,					//required
	"SpinAxis": -13.2,					//required
	"TotalSpin": 3250.0,				//required
	"BackSpin": 2500.0,				//only required if total spin is not sent
	"SideSpin": -800.0,				//only required if total spin is not sent
	"HLA": 2.3,						//required
	"VLA": 14.3,						//required
	"CarryDistance": 256.5			//optional
},
"ClubData": {							
	"Speed": 0.0,
	"AngleOfAttack": 0.0,
	"FaceToTarget": 0.0,
	"Lie": 0.0,
	"Loft": 0.0,
	"Path": 0.0,
	"SpeedAtImpact": 0.0,
	"VerticalFaceImpact": 0.0,
	"HorizontalFaceImpact": 0.0,
	"ClosureRate": 0.0
},
"ShotDataOptions": {
	"ContainsBallData": true,				//required
	"ContainsClubData": false,			//required
	"LaunchMonitorIsReady": true, 		//optional
	"LaunchMonitorBallDetected": true, 	//optional
	"IsHeartBeat": false 					//optional (retired)
}
	}
```
## GSPro Connect to Launch Monitor JSON
The response from GSPro Connect is currently very simple. 
The two common responses you will receive are:
- 200: response confirming that we received and processed your JSON and 
- 201: Player information for handedness and club, The latter is typically used for Launch Monitors that need to switch between full strike clubs and putting.

Example:
```
{
	"Code": 201,
	"Message": "GSPro Player Information",
	"Player": {
		"Handed": "RH",
		"Club": "DR"
	}
}
```

Additional documentation will be added to describe more response codes into the future. 

Current Reponse Codes:
- 200: Shot received successfully
- 201: Player information
- 501/5XX: Failure occurred