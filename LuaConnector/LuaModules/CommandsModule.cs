using System;
using System.Collections.Generic;
using System.Linq;

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

		public static bool Process(string command, Server.Client client)
		{
			var args = command.SplitCmd();

			foreach(var script in _commands)
			{
				foreach (var cmd in script.Value)
				{
					if (args[0].Equals(cmd.Key))
					{
						if (cmd.Value.ArgsAsArray)
						{
							if (args.Length > 1)
								cmd.Value.Callback.Call(client, args.Skip(1).ToArray());
							else
								cmd.Value.Callback.Call(client);
						}
						else
						{
							if (args.Length > 1)
								cmd.Value.Callback.Call(new object[] { client }.Concat(args.Skip(1)).ToArray());
							else
								cmd.Value.Callback.Call(client);
						}

						return true;
					}
				}
			}

			return false;
		}

		public static void RemoveAllCommandsInScript(Lua.Script script)
		{
			lock(_commands)
				_commands.Remove(script);
		}
	}
}
