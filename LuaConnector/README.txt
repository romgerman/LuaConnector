
INSTALL:
	1. Place resource in "resources" folder of server
	2. Run server. LuaConnector will create "LuaScripts" folder in root server folder (You can specify folder in meta.xml)
	3. Make or place your scripts inside "LuaScripts" folder

TROUBLESHOOT:
	(?) I run server and have placed my scripts in LuaScripts folder but nothing happening
		- Restart server. It should write in command line that scripts was loaded

DOCUMENTATION: http://gtmp-luaconnector.readthedocs.io/en/latest/
FORUM THREAD: https://gt-mp.net/forum/thread/122-alpha-luaconnector-%E2%80%94-write-your-server-scripts-in-lua/

Changelog:

	[07.07.2017]
	- Timer module fix
	- Fixed threading error that throws when hot-reload script with "OnUpdate"
	- Fixed exception if you pass an Entity object instead of NetHandle
	- Other fixes
	
	[04.07.2017]
	- Fixed createVehicle optional argument
	- Added optional argument for "cmd" module
	- Fixed some of the threading issues

	[01.07.2017]
	- Fixed "require"
	
	[29.06.2017]
	- Added Enum.values function
	- Better error handling (?)
	- Fixed Enum.castTo now returns nil on fail
	- Clientside menu module (cmenu)
	- Docs improved

	[11.06.2017]:
	- Added getEntityFromHandle and Enum table functions to autocompletion packages
	- Added timers module and it's documentation
	- Fixed sendChatMessageToAll and sendChatMessageToPlayer again
	- Fixed Vector3 constructor now can be without args
	- Other fixes

	[11.06.2017]:
	- Fixed sendChatMessageToPlayer, sendChatMessageToAll
	- Fixed timer module crash when stopping server
	- Fixed hot-reload infinite script loading (?)
	- Added commands module
