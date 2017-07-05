using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Server = GrandTheftMultiplayer.Server;
using Shared = GrandTheftMultiplayer.Shared;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector
{
	class LuaScript : IScript
	{
		public Lua.Script Instance { get { return _context; } }
		public bool HasLoaded { get { return _hasLoaded; } }

		public string FilePath => _filename;
		public string FileName => Path.GetFileName(_filename);

		private Lua.Script _context;
		private ScriptLoader _loader;

		private string _filename;
		private bool _hasLoaded;

		public LuaScript(string filename, ScriptLoader loader)
		{
			_filename = filename;
			_loader = loader;
			_context  = new Lua.Script(Lua.CoreModules.Basic |
										Lua.CoreModules.Json |
										Lua.CoreModules.Math |
										Lua.CoreModules.Metatables |
										Lua.CoreModules.OS_Time |
										Lua.CoreModules.String |
										Lua.CoreModules.Table |
										Lua.CoreModules.TableIterators |
										Lua.CoreModules.ErrorHandling |
										Lua.CoreModules.GlobalConsts |
										Lua.CoreModules.Bit32 |
										Lua.CoreModules.LoadMethods |
										Lua.CoreModules.Coroutine |
										Lua.CoreModules.IO);

			Lua.ModuleRegister.RegisterModuleType<LuaModules.XmlModule>(_context.Globals);
			Lua.ModuleRegister.RegisterModuleType<LuaModules.HttpModule>(_context.Globals);
			Lua.ModuleRegister.RegisterModuleType<LuaModules.TimerModule>(_context.Globals);
			Lua.ModuleRegister.RegisterModuleType<LuaModules.CommandsModule>(_context.Globals);
			Lua.ModuleRegister.RegisterModuleType<LuaModules.ClientsideMenuModule>(_context.Globals);

			// Register namespaces
			
			_context.Globals["API"] = ApiTable.Table;
			_context.Globals["Script"] = Lua.DynValue.NewTable(_context);
			_context.Globals["Script", "Title"] = Lua.DynValue.NewString(FileName);
			_context.Globals["Server"] = Lua.DynValue.NewTable(_context);
			_context.Globals["Server", "TickCount"] = (Func<double>)Server_TickCount;

			// Register helper functions

			_context.Globals["Vector3"] = (Func<double, double, double, Shared.Math.Vector3>)Vector3Proxy.New;
			
			_context.Globals["Enum"] = Lua.DynValue.NewTable(_context);
			_context.Globals["Enum", "castTo"] = (Func<Lua.ScriptExecutionContext, Lua.CallbackArguments, Lua.DynValue>)Enum_castTo;
			_context.Globals["Enum", "value"]  = (Func<Lua.ScriptExecutionContext, Lua.CallbackArguments, Lua.DynValue>)Enum_value;
			_context.Globals["Enum", "values"] = (Func<Lua.ScriptExecutionContext, Lua.CallbackArguments, Lua.DynValue>)Enum_values;

			_context.Globals["Enum", "Hash"] = Lua.UserData.CreateStatic<Server.Constant.Hash>();
			_context.Globals["Enum", "PedHash"] = Lua.UserData.CreateStatic<Server.Constant.PedHash>();
			_context.Globals["Enum", "WeaponHash"] = Lua.UserData.CreateStatic<Shared.WeaponHash>();
			_context.Globals["Enum", "VehicleHash"]  = Lua.UserData.CreateStatic<Shared.VehicleHash>();
			_context.Globals["Enum", "PickupHash"]  = Lua.UserData.CreateStatic<Shared.PickupHash>();
			_context.Globals["Enum", "EntityType"]  = Lua.UserData.CreateStatic<Shared.EntityType>();
			_context.Globals["Enum", "VehicleDataFlags"] = Lua.UserData.CreateStatic<Shared.VehicleDataFlags>();
			_context.Globals["Enum", "ExplosionType"]   = Lua.UserData.CreateStatic<Server.Constant.ExplosionType>();
			_context.Globals["Enum", "WeaponComponent"] = Lua.UserData.CreateStatic<Server.Constant.WeaponComponent>();
			_context.Globals["Enum", "WeaponTint"] = Lua.UserData.CreateStatic<Server.Constant.WeaponTint>();
		}

		/// <summary>
		/// Loads lua scripts
		/// </summary>
		/// <returns>False if the script is a module</returns>
		public bool Load()
		{
			if (_context.DoFile(_filename).IsNil() && HasApiHooks())
				return true;

			return false;
		}

		#region Helpers
		
		private double Server_TickCount()
		{
			return Server.API.API.shared.TickCount;
		}

		private Lua.DynValue Enum_castTo(Lua.ScriptExecutionContext cx, Lua.CallbackArguments args)
		{
			if (args.Count < 2)
				throw new ArgumentException("You should provide two arguments: first is an enum type, second is an enum value (number or string)");

			if (args[0].Type != Lua.DataType.UserData)
				throw new ArgumentException("First argument should be in type of enum");

			var enumType = args[0].UserData.Descriptor.Type;

			if (!enumType.IsEnum)
				throw new ArgumentException("First argument should be in type of enum");

			var number = args[1];

			if (number.Type != Lua.DataType.Number && number.Type != Lua.DataType.String)
				throw new ArgumentException("Second argument should be in type of number or string");

			var str = string.Empty;

			if(number.Type == Lua.DataType.Number)
				str = Convert.ChangeType(number.Number, enumType.GetEnumUnderlyingType()).ToString();
			else
				str = number.String;

			try
			{
				var enumObj = Enum.Parse(enumType, str);

				return Lua.DynValue.FromObject(cx.GetScript(), enumObj);
			}
			catch
			{
				return Lua.DynValue.Nil;
			}
		}
		
		private Lua.DynValue Enum_value(Lua.ScriptExecutionContext cx, Lua.CallbackArguments args)
		{
			if (args.Count == 0)
				throw new ArgumentException("You should provide at least one argument in type of enum");

			Lua.DynValue[] result = new Lua.DynValue[args.Count];

			for(int i = 0; i < args.Count; i++)
			{
				var arg = args[i];

				if (arg.Type != Lua.DataType.UserData || arg.UserData.Object == null)
					throw new ArgumentException($"Argument #{i+1} should be in type of enum");

				var rawValue = arg.UserData.Object;
				var enumType = rawValue.GetType();
				var value = Convert.ChangeType(rawValue, enumType.GetEnumUnderlyingType());

				result[i] = Lua.DynValue.NewNumber(Convert.ToDouble(value));
			}

			return Lua.DynValue.NewTuple(result);
		}

		private Lua.DynValue Enum_values(Lua.ScriptExecutionContext cx, Lua.CallbackArguments args)
		{
			var en = args.AsType(0, "values", Lua.DataType.UserData).UserData;
			var raw = Enum.GetValues(en.Descriptor.Type);
			var result = new List<Lua.DynValue>(raw.Length);

			foreach(var i in raw)
				result.Add(Lua.DynValue.FromObject(cx.GetScript(), i));

			return Lua.DynValue.NewTable(cx.GetScript(), result.ToArray());
		}

		#endregion

		SemaphoreSlim semaphore = new SemaphoreSlim(1);
		
		public void CallFunction(string @namespace, string function, params object[] args)
		{
			try
			{
				semaphore.Wait();

				var ns = _context.Globals[@namespace] as Lua.Table;

				if (ns == null)
					return;

				var fn = ns[function] as Lua.Closure;

				if (fn == null)
					return;

				_context.Call(fn, args);
			}
			catch (Lua.ScriptRuntimeException e)
			{
				LuaConnector.Print(Server.Constant.LogCat.Error, $"An exception was raised when calling \"{function}\"");
				LuaConnector.Print(Server.Constant.LogCat.Error, $"Script {FileName} was unloaded");
				LuaConnector.Print(Server.Constant.LogCat.Error, e.DecoratedMessage);

				if (_context.SourceCodeCount > 0 && e.CallStack.Count > 0)
				{
					foreach(var item in e.CallStack)
					{
						LuaConnector.Print(Server.Constant.LogCat.Error, "in " + _context.GetSourceCode(_context.SourceCodeCount - 1).GetCodeSnippet(item.Location));
					}
				}

				_loader.Unload(_filename);
			}
			finally
			{
				semaphore.Release();
			}
		}

		public bool HasApiHooks()
		{
			if (_context.GetTable("Script").Keys.Count() > 0 ||
				_context.GetTable("Server").Keys.Count() > 0)
				return true;

			return false;
		}

		public void SetScriptHasLoaded()
		{
			_hasLoaded = true;
		}

	}
}
