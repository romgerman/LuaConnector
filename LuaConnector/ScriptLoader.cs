using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using Server = GrandTheftMultiplayer.Server;
using Shared = GrandTheftMultiplayer.Shared;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector
{
	class ScriptLoader
	{
		struct FSChange
		{
			public WatcherChangeTypes Type;
			public Timer Timer;
		}

		FileSystemWatcher _fsWatcher;
		string _scriptDirectory;

		ConcurrentDictionary<string, LuaScript> _scripts = new ConcurrentDictionary<string, LuaScript>();
		Dictionary<string, FSChange> _fsChanges = new Dictionary<string, FSChange>();

		public ScriptLoader(string folder)
		{
			_scriptDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder);
			_fsWatcher = new FileSystemWatcher(_scriptDirectory, "*.lua");

			_fsWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.FileName;
			_fsWatcher.Created += WatcherFileChanged;
			_fsWatcher.Changed += WatcherFileChanged;
			_fsWatcher.Deleted += WatcherFileChanged;
			
			Lua.Script.DefaultOptions.DebugPrint = (str) => LuaConnector.Print(Server.Constant.LogCat.Info, str);
			Lua.Script.DefaultOptions.ScriptLoader = new Lua.Loaders.FileSystemScriptLoader();
			Lua.Script.DefaultOptions.CheckThreadAccess = true;
			((Lua.Loaders.ScriptLoaderBase)(Lua.Script.DefaultOptions.ScriptLoader)).ModulePaths = new string[] { folder + "/?", folder + "/?.lua" };

			// Register all enums

			Lua.UserData.RegisterType<Shared.WeaponHash>();
			Lua.UserData.RegisterType<Shared.VehicleHash>();
			Lua.UserData.RegisterType<Shared.PickupHash>();
			Lua.UserData.RegisterType<Shared.EntityType>();
			Lua.UserData.RegisterType<Shared.VehicleDataFlags>();
			Lua.UserData.RegisterType<Server.Constant.ExplosionType>();
			Lua.UserData.RegisterType<Server.Constant.Hash>();
			Lua.UserData.RegisterType<Server.Constant.PedHash>();
			Lua.UserData.RegisterType<Server.Constant.WeaponComponent>();
			Lua.UserData.RegisterType<Server.Constant.WeaponTint>();
			//Lua.UserData.RegisterType<Shared.ConnectionChannel>();
			//Lua.UserData.RegisterType<Shared.EntityFlag>();
			//Lua.UserData.RegisterType<Shared.FileType>();
			//Lua.UserData.RegisterType<Shared.Lights>();
			//Lua.UserData.RegisterType<Shared.NonStandardVehicleMod>();
			//Lua.UserData.RegisterType<Shared.PacketType>();
			//Lua.UserData.RegisterType<Shared.PedDataFlags>();
			//Lua.UserData.RegisterType<Shared.ScriptVersion>();
			//Lua.UserData.RegisterType<Shared.ServerEventType>();
			//Lua.UserData.RegisterType<Shared.SyncEventType>();
			Lua.UserData.RegisterType<Server.ResourceType>();

			// Register classes that don't need to be wrapped

			Lua.UserData.RegisterType<Shared.NetHandle>();
			Lua.UserData.RegisterType<Server.API.CancelEventArgs>();
			Lua.UserData.RegisterType<Server.Constant.Color>();
			Lua.UserData.RegisterType<Server.Elements.Entity>();
			Lua.UserData.RegisterType<Server.Elements.Client>();
			Lua.UserData.RegisterType<Server.Elements.Vehicle>();
			Lua.UserData.RegisterType<Server.Elements.Blip>();
			Lua.UserData.RegisterType<Server.Elements.Marker>();
			Lua.UserData.RegisterType<Server.Elements.Object>();
			Lua.UserData.RegisterType<Server.Elements.ParticleEffect>();
			Lua.UserData.RegisterType<Server.Elements.Ped>();
			Lua.UserData.RegisterType<Server.Elements.Pickup>();
			Lua.UserData.RegisterType<Server.Elements.TextLabel>();
			Lua.UserData.RegisterType<Server.Managers.ColShape>();
			Lua.UserData.RegisterType<Server.Managers.Rectangle2DColShape>();
			Lua.UserData.RegisterType<Server.Managers.Rectangle3DColShape>();
			Lua.UserData.RegisterType<Server.Managers.CylinderColShape>();
			Lua.UserData.RegisterType<Server.Managers.SphereColShape>();
			Lua.UserData.RegisterType<Server.Managers.CommandInfo>();

			Lua.UserData.RegisterType<TimeSpan>();

			// Register wrapped classes
			
			Lua.UserData.RegisterProxyType<Vector3Proxy, Shared.Math.Vector3>(r => new Vector3Proxy(r), Lua.InteropAccessMode.Default, "Vector3");
			//Lua.UserData.RegisterProxyType<ClientProxy, Server.Elements.Client>(r => new ClientProxy(r), Lua.InteropAccessMode.Default, "Client");
		}

		private void WatcherFileChanged(object sender, FileSystemEventArgs e)
		{
			FSChange change;
			if (_fsChanges.TryGetValue(e.FullPath, out change))
			{
				if (e.ChangeType != WatcherChangeTypes.Deleted)
				{
					change.Type = e.ChangeType;
					_fsChanges.Remove(e.FullPath);
					_fsChanges.Add(e.FullPath, change);
				}

				return;
			}

			var timer = new Timer(500);
			timer.Elapsed += (s, a) => ResolveFSChange(e.FullPath);
			timer.AutoReset = false;

			_fsChanges.Add(e.FullPath, new FSChange()
			{
				Type = e.ChangeType,
				Timer = timer
			});

			timer.Start();
		}

		private void ResolveFSChange(string path)
		{
			var change = _fsChanges[path];
			
			Unload(path);

			if (change.Type != WatcherChangeTypes.Deleted)
				LoadScript(path);

			change.Timer.Close();
			_fsChanges.Remove(path);
		}

		public void LoadAll()
		{
			if (!Directory.Exists(_scriptDirectory))
			{
				Directory.CreateDirectory(_scriptDirectory);
			}
			else
			{
				string[] scripts = Directory.GetFiles(_scriptDirectory);

				foreach (var scriptPath in scripts)
				{
					LoadScript(scriptPath);
				}
			}

			_fsWatcher.EnableRaisingEvents = true;
		}

		public void UnloadAll()
		{
			CallAll("Script", "OnStop");
			LuaModules.TimerModule.DestroyAllTimers();
			LuaModules.ClientsideMenuModule.RemoveAllMenus();
			_fsWatcher.EnableRaisingEvents = false;

			lock(_scripts)
				_scripts.Clear();
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Unload(string path)
		{
			if (!_scripts.ContainsKey(path))
				return;

			LuaModules.TimerModule.DestroyAllTimerForScript(_scripts[path].Instance);
			LuaModules.CommandsModule.RemoveAllCommandsInScript(_scripts[path].Instance);

			_scripts[path].CallFunction("Script", "OnStop");

			LuaScript scr;
			if (!_scripts.TryRemove(path, out scr))
				Console.WriteLine($"Cannot unload script {scr.FileName}");
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void LoadScript(string filename)
		{
			var path = Path.GetFileName(filename);

			try
			{
				var script = new LuaScript(filename, this);

				if (!_scripts.TryAdd(filename, script))
					throw new Lua.ScriptRuntimeException($"Cannot add script {script.FileName}");

				LuaConnector.Instance.API.consoleOutput($"{path} was loaded successfully");

				script.CallFunction("Script", "OnStart");
				script.SetScriptHasLoaded();
			}
			catch(Lua.InterpreterException e)
			{
				LuaConnector.Print(Server.Constant.LogCat.Error, $"Cannot load {path}\n{e.DecoratedMessage}");
			}
		}
		
		public void CallAll(string @namespace, string function, params object[] args)
		{
			foreach (var script in _scripts)
				if (script.Value.HasLoaded)
					script.Value.CallFunction(@namespace, function, args);
		}
	}
}
