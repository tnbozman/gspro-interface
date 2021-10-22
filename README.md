# GSPro Open Connect v1 Documentation
The following describes how to connect to the GSPro Connect interface with the “Open Connect” protocol.

## Summary
The overall process breaks down very simple; GSPro Connect opens a socket and listens for an incoming client. Once connected is established a constant 2-way communication continues:

### To GSPro Connect from Launch Monitor client:
- Shot Data
- Ready signals
- Heart beat
### From GSPro to Client
- General response from incoming messages (success/failure)
- Player information

## Socket information

The socket connection is open and does not require authentication. Note: It is assumed that the launch monitor software is being ran on the same PC as GSPro connect. If this is not the case, firewall and port forwarding may be required

- Port: 0921
- IP Address: 127.0.0.1
 
## Launch Monitor to GSPro Connect JSON
The JSON required to send a shot is straight forward and readable. The two more important sections are the root properties and BallData object. The other area of important is the shotData properties to describe what data is being sent in.

```
{
"DeviceID": "GSPro LM 1.1",  			//required - unqiue per launch monitor / prooject type
"Units": "Yards",						//default yards
"ShotNumber": 13,						//required - auto increment from LM
"APIversion": "1",						//required - "1" is current version
"BallData": {		
	"Speed": 147.5,						//required
	"SpinAxis": -13.2,					//required
	"TotalSpin": 3250.0,				//required
	"BackSpin": 2500.0,					//only required if total spin is not sent
	"SideSpin": -800.0,					//only required if total spin is not sent
	"HLA": 2.3,							//required
	"VLA": 14.3,						//required
	"CarryDistance": 256.5				//optional
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
	"ContainsBallData": true,			//required
	"ContainsClubData": false,			//required
	"LaunchMonitorIsReady": true, 		//not required
	"LaunchMonitorBallDetected": true, 	//not required
	"IsHeartBeat": false 				//not required
}
	}
```
## GSPro Connect to Launch Monitor JSON
The response from GSPro Connect is currently very simple. The two common responses you will receive is 1) 200 response confirming that we received and processed your JSON and 2) Player information for handedness and club, The latter is typically used for Launch monitors that need to switch between full strike clubs and putting.

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
Additional documentation will be added to describe additional response codes. Currently:

- 200: Shot received successfully
- 201: Player information
- 501/5XX: Failure occurred