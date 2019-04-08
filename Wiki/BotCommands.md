# Admin Commands
## `/init`
This command is used to init a telegram group in order the bot know to what game and what team it belongs. The syntax is:

_Usage_

    /init gameId=<gameId> teamId=<teamId>

After the init, the bot will store the gameId and teamId in persistant storage

## `/reset`
This command will erase the data stored of the group. An `/init` should be done prior any other commands

## `/begin`
This command is used to start the team on the game. It logs the departure time.
It will additionnaly reset the hidden and action nodes.

## `/end`
This command is used to terminate the game for the team. It logs the arrival time.

## `/broadcast`
This command send a message to all teams of a game or an individual team.

_Usage_

    /broadcast [gameId=<gameId>] [teamId=<teamId>] <message>

_Restrictions_
The telegram group should have been initialized.

## `/broadcastLocation`
This command will send to groups a location.

_Usage_

    /broadcastLocation [gameId=<gameId>] [teamId=<teamId>] Lat=<latitude> Lng=<longitude>

_Restrictions_
The telegram group should have been initialized.

## `/give`
This command will give points to the team which belongs to the telegram group

_Usage_

    /give points=<points>

_Note_ You can also give negative points
 
_Restrictions_
The telegram group should have been initialized.

## `/state`
This command display the state of the chatroom

_Usage_

    /state

_Result_
> Game Status: Initialized  
> Game: (Id:1, Name: Test, StartDate: 05/09/2018 07:59:54)  
> Team: (Id: 1, Name: Team 1)  
> CurrentLocation: (Lat:65,3, Lng: 3,9)  

## `/rename`
This command ask the bot to rename the chat according to the name of the team and game

_Usage_

    /rename

_Result_

The chat is renamed.

_Restrictions_

The chat should have been initialized.

## `/leave`
This command ask to the bot to leave the group.

_Usage_

    /leave

## `/refresh`
Refresh the Hidden and Action node of the team

_Usage_
	/refresh

_Result_

The hidden and action node of the team had been updated. _WARNING_ If the team had already visit a node, 
the bot will allow a new visit.

_Restrictions_

The chat should have been initialized.

# Users commands
## `/help`
Display a short help message to the group

_Usage_

`/help`


## `/displayNode`
Display the position of the next node and ask player to reach it

_Usage_

`/displayNode`

_Restrictions_
* The group should had been initialized
* The game should be in `started` mode for the team

## `/displayHints`
Display hints for hidden node and what each node give to the user's team

_Usage_
/hints

_Restrictions_
* The group should had been initialized
* The game should be in `started` mode for the team

## `/resetNext`
Reset the current node to the closest according to the current position of the team which use it.

_Usage_
/resetNext

_Restrictions_
* The group should had been initialized
* The game should be in `started` mode for the team
* The current position of the team should be set

# Shadow commands
## `/redeem`
This command is not to be used directly in a group, but send through deep link to the bot after scanning a QRcode.

_Usage_

    /redeem gameId=<gameId> pass=<passcode>

_Restrictions_
* This command should be used through deep linking. 
* A team cannot redeem a same code multiple times. 
* The player that redeem passcode should have registered it's telegram username in the team. 
* The game should be active and the same day of the redeem.

## `/newUser`
this command is issued internally by the bot when a player is added to the group. It will add the user in the team set in the chatroom by `/init` command.

When creating a chatroom, add the non-players user (such as admin), then the bot, init the chat with `/init` command then add players. The team in back-end will be updated.

_Restrictions_
* The chatroom should had been initialized before adding new player


