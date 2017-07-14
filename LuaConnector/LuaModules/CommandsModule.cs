using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Server = GrandTheftMultiplayer.Server.Elements;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector.LuaModules
{
	internal class LuaCmd
	{
		public bool ArgsAsArray;
		public Lua.Closure Callback;
	}

	[Lua.MoonSharpModule(Namespace = "cmd")]
	internal class CommandsModule
	{
		private static Dictionary<Lua.Script, Dictionary<string, LuaCmd>> _commands = new Dictionary<Lua.Script, Dictionary<string, LuaCmd>>();

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue add(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "add", Lua.DataType.String).String;
				var callback = args.AsType(1, "add", Lua.DataType.Function).Function;
				var asArray = args.AsType(2, "add", Lua.DataType.Boolean, true);

				lock(_commands)
				{
					if (!_commands.ContainsKey(context.GetScript()))
						_commands.Add(context.GetScript(), new Dictionary<string, LuaCmd>());

					if (_commands[context.GetScript()].ContainsKey(name))
						throw new Lua.ScriptRuntimeException("There is already a command with that name");

					_commands[context.GetScript()].Add(name, new LuaCmd { Callback = callback, ArgsAsArray = asArray.IsNil() ? false : asArray.Boolean });
				}

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue remove(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "remove", Lua.DataType.String).String;

				lock(_commands)
					_commands[context.GetScript()].Remove(name);

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		static SemaphoreSlim semaphore = new SemaphoreSlim(1);

		public static bool Process(string command, Server.Client client)
		{
			if (command.Length == 1)
				return true;

			var args = command.SplitCmd();

			try
			{
				semaphore.Wait();

				foreach (var script in _commands)
				{
					foreach (var cmd in script.Value)
					{
						if (!args[0].Equals(cmd.Key))
							continue;

						if (cmd.Value.ArgsAsArray)
						{
							if (args.Length > 1)
								return IsCancelling(cmd.Value.Callback.Call(client, args.Skip(1).ToArray()));
							else
								return IsCancelling(cmd.Value.Callback.Call(client));
						}
						else
						{
							if (args.Length > 1)
								return IsCancelling(cmd.Value.Callback.Call(new object[] { client }.Concat(args.Skip(1)).ToArray()));
							else
								return IsCancelling(cmd.Value.Callback.Call(client));
						}
					}
				}
			}
			finally
			{
				semaphore.Release();
			}			

			return false;
		}

		private static bool IsCancelling(Lua.DynValue ret)
		{
			return ret.IsNil() ? true : ret.Boolean;
		}

		public static void RemoveAllCommandsInScript(Lua.Script script)
		{
			lock(_commands)
				_commands.Remove(script);
		}
	}
}
