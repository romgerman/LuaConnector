using System;

using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;

using LuaConnector.ORM;

namespace LuaConnector
{
	public class LuaConnector : Script
    {
		public static Script Instance;
		public static string ServerDirectory => AppDomain.CurrentDomain.BaseDirectory;

		ScriptLoader _loader;

		public LuaConnector()
		{
			API.onResourceStart += OnResourceStart;
			API.onResourceStop += OnResourceStop;

			API.onChatCommand += OnChatCommand;
			API.onChatMessage += OnChatMessage;

			API.onClientEventTrigger += OnClientEventTrigger;

			API.onPlayerBeginConnect += OnPlayerBeginConnect;
			API.onPlayerConnected += OnPlayerConnected;
			API.onPlayerDisconnected += OnPlayerDisconnected;
			API.onPlayerFinishedDownload += OnPlayerFinishedDownload;

			API.onUpdate += OnUpdate;

			API.onEntityDataChange += OnEntityDataChange;
			API.onEntityEnterColShape += OnEntityEnterColShape;
			API.onEntityExitColShape += OnEntityExitColShape;

			API.onMapChange += OnMapChange;

			API.onPickupRespawn += OnPickupRespawn;

			API.onPlayerArmorChange += OnPlayerArmorChange;
			API.onPlayerDeath += OnPlayerDeath;
			API.onPlayerDetonateStickies += OnPlayerDetonateStickies;
			API.onPlayerEnterVehicle += OnPlayerEnterVehicle;
			API.onPlayerExitVehicle += OnPlayerExitVehicle;
			API.onPlayerHealthChange += OnPlayerHealthChange;
			API.onPlayerModelChange += OnPlayerModelChange;
			API.onPlayerPickup += OnPlayerPickup;
			API.onPlayerRespawn += OnPlayerRespawn;
			API.onPlayerWeaponAmmoChange += OnPlayerWeaponAmmoChange;
			API.onPlayerWeaponSwitch += OnPlayerWeaponSwitch;

			API.onVehicleDeath += OnVehicleDeath;
			API.onVehicleDoorBreak += OnVehicleDoorBreak;
			API.onVehicleHealthChange += OnVehicleHealthChange;
			API.onVehicleSirenToggle += OnVehicleSirenToggle;
			API.onVehicleTrailerChange += OnVehicleTrailerChange;
			API.onVehicleTyreBurst += OnVehicleTyreBurst;
			API.onVehicleWindowSmash += OnVehicleWindowSmash;

			Instance = this;
		}

		public static void Print(LogCat cat, string text)
		{
			API.shared.consoleOutput(cat, text);
		}

		#region events

		private void OnVehicleWindowSmash(NetHandle entity, int index)
		{
			_loader.CallAll("Server", "OnVehicleWindowSmash", entity, index);
		}

		private void OnVehicleTyreBurst(NetHandle entity, int index)
		{
			_loader.CallAll("Server", "OnVehicleTyreBurst", entity, index);
		}

		private void OnVehicleTrailerChange(NetHandle tower, NetHandle trailer)
		{
			_loader.CallAll("Server", "OnVehicleTrailerChange", tower, trailer);
		}

		private void OnVehicleSirenToggle(NetHandle entity, bool oldValue)
		{
			_loader.CallAll("Server", "OnVehicleSirenToggle", entity, oldValue);
		}

		private void OnVehicleHealthChange(NetHandle entity, float oldValue)
		{
			_loader.CallAll("Server", "OnVehicleHealthChange", entity, oldValue);
		}

		private void OnVehicleDoorBreak(NetHandle entity, int index)
		{
			_loader.CallAll("Server", "OnVehicleDoorBreak", entity, index);
		}

		private void OnVehicleDeath(NetHandle entity)
		{
			_loader.CallAll("Server", "OnVehicleDeath", entity);
		}

		private void OnPlayerWeaponSwitch(Client player, WeaponHash oldValue)
		{
			_loader.CallAll("Server", "OnPlayerWeaponSwitch", player, oldValue);
		}

		private void OnPlayerWeaponAmmoChange(Client player, WeaponHash weapon, int oldValue)
		{
			_loader.CallAll("Server", "OnPlayerWeaponAmmoChange", player, weapon, oldValue);
		}

		private void OnPlayerRespawn(Client player)
		{
			_loader.CallAll("Server", "OnPlayerRespawn", player);
		}

		private void OnPlayerPickup(Client pickupee, NetHandle pickupHandle)
		{
			_loader.CallAll("Server", "OnPlayerPickup", pickupee, pickupHandle);
		}

		private void OnPlayerModelChange(Client player, int oldValue)
		{
			_loader.CallAll("Server", "OnPlayerModelChange", player, oldValue);
		}

		private void OnPlayerHealthChange(Client player, int oldValue)
		{
			_loader.CallAll("Server", "OnPlayerHealthChange", player, oldValue);
		}

		private void OnPlayerFinishedDownload(Client player)
		{
			LuaModules.ClientsideMenuModule.RegisterMenuHandlersForClient(player);
			LuaModules.ClientsideMenuModule.RegisterAllMenusForClient(player);

			_loader.CallAll("Server", "OnPlayerFinishedDownload", player);
		}

		private void OnPlayerExitVehicle(Client player, NetHandle vehicle)
		{
			_loader.CallAll("Server", "OnPlayerExitVehicle", player, vehicle);
		}

		private void OnPlayerEnterVehicle(Client player, NetHandle vehicle)
		{
			_loader.CallAll("Server", "OnPlayerEnterVehicle", player, vehicle);
		}

		private void OnPlayerDetonateStickies(Client player)
		{
			_loader.CallAll("Server", "OnPlayerDetonateStickies", player);
		}

		private void OnPlayerDeath(Client player, NetHandle entityKiller, int weapon)
		{
			_loader.CallAll("Server", "OnPlayerDeath", player, entityKiller, weapon);
		}

		private void OnPlayerBeginConnect(Client player, CancelEventArgs cancelConnection)
		{
			_loader.CallAll("Server", "OnPlayerBeginConnect", player, cancelConnection);
		}

		private void OnPlayerArmorChange(Client player, int oldValue)
		{
			_loader.CallAll("Server", "OnPlayerArmorChange", player, oldValue);
		}

		private void OnPickupRespawn(NetHandle entity)
		{
			_loader.CallAll("Server", "OnPickupRespawn", entity);
		}

		private void OnMapChange(string mapName, GrandTheftMultiplayer.Server.XmlGroup map)
		{
			//throw new NotImplementedException(); // TODO: implement this
		}

		private void OnEntityExitColShape(ColShape colshape, NetHandle entity)
		{
			_loader.CallAll("Server", "OnEntityExitColShape", colshape, entity);
		}

		private void OnEntityEnterColShape(ColShape colshape, NetHandle entity)
		{
			_loader.CallAll("Server", "OnEntityEnterColShape", colshape, entity);
		}

		private void OnEntityDataChange(NetHandle entity, string key, object oldValue)
		{
			_loader.CallAll("Server", "OnEntityDataChange", entity, key, oldValue);
		}

		private void OnUpdate()
		{
			if (!LuaModules.TimerModule.IsHighResolutionSet)
				LuaModules.TimerModule.ProcessTimers();

			_loader.CallAll("Server", "OnUpdate");
		}

		private void OnPlayerConnected(Client player)
		{
			_loader.CallAll("Server", "OnPlayerConnected", player);
		}

		private void OnPlayerDisconnected(Client player, string reason)
		{
			LuaModules.ClientsideMenuModule.UnregisterClient(player);
			_loader.CallAll("Server", "OnPlayerDisconnected", player, reason);
		}		

		private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
		{
			LuaModules.ClientsideMenuModule.ProcessMenuEvents(eventName);

			_loader.CallAll("Server", "OnClientEvent", sender, eventName, arguments);
		}

		private void OnChatMessage(Client sender, string message, CancelEventArgs cancel)
		{
			_loader.CallAll("Server", "OnChatMessage", sender, message, cancel);
		}

		private void OnChatCommand(Client sender, string command, CancelEventArgs cancel)
		{
			if (LuaModules.CommandsModule.Process(command, sender))
				cancel.Cancel = true;

			_loader.CallAll("Server", "OnChatCommand", sender, command, cancel);
		}

		#endregion

		private void OnResourceStart()
		{
			new ApiTable(this);

#if !DISABLE_DB

			Print(LogCat.Info, "Loading database providers...");
			LuaModules.DatabaseModule.RegisterDatabaseProviders();

#endif

			string scriptsPath = API.getSetting<string>("folder");

			Print(LogCat.Info, "Loading all Lua scripts...");

			_loader = new ScriptLoader(scriptsPath ?? "LuaScripts");
			_loader.LoadAll();
		}
		
		private void OnResourceStop()
		{
			_loader.UnloadAll();

			// Remove all events

			API.onResourceStart -= OnResourceStart;
			API.onResourceStop -= OnResourceStop;

			API.onChatCommand -= OnChatCommand;
			API.onChatMessage -= OnChatMessage;

			API.onClientEventTrigger -= OnClientEventTrigger;

			API.onPlayerBeginConnect -= OnPlayerBeginConnect;
			API.onPlayerConnected -= OnPlayerConnected;
			API.onPlayerDisconnected -= OnPlayerDisconnected;
			API.onPlayerFinishedDownload -= OnPlayerFinishedDownload;

			API.onUpdate -= OnUpdate;

			API.onEntityDataChange -= OnEntityDataChange;
			API.onEntityEnterColShape -= OnEntityEnterColShape;
			API.onEntityExitColShape -= OnEntityExitColShape;

			API.onMapChange -= OnMapChange;

			API.onPickupRespawn -= OnPickupRespawn;

			API.onPlayerArmorChange -= OnPlayerArmorChange;
			API.onPlayerDeath -= OnPlayerDeath;
			API.onPlayerDetonateStickies -= OnPlayerDetonateStickies;
			API.onPlayerEnterVehicle -= OnPlayerEnterVehicle;
			API.onPlayerExitVehicle -= OnPlayerExitVehicle;
			API.onPlayerHealthChange -= OnPlayerHealthChange;
			API.onPlayerModelChange -= OnPlayerModelChange;
			API.onPlayerPickup -= OnPlayerPickup;
			API.onPlayerRespawn -= OnPlayerRespawn;
			API.onPlayerWeaponAmmoChange -= OnPlayerWeaponAmmoChange;
			API.onPlayerWeaponSwitch -= OnPlayerWeaponSwitch;

			API.onVehicleDeath -= OnVehicleDeath;
			API.onVehicleDoorBreak -= OnVehicleDoorBreak;
			API.onVehicleHealthChange -= OnVehicleHealthChange;
			API.onVehicleSirenToggle -= OnVehicleSirenToggle;
			API.onVehicleTrailerChange -= OnVehicleTrailerChange;
			API.onVehicleTyreBurst -= OnVehicleTyreBurst;
			API.onVehicleWindowSmash -= OnVehicleWindowSmash;
		}
	}
}
