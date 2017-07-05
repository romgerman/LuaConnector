using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector.LuaModules
{
	// MAYBE: own IO module

	public enum LuaFileOpenMode
	{
		Read = 1,
		Write = 2,
		Append = 4,
		Update = 8,
		Binary = 16
	}

	public class LuaFileHandle
	{
		[Lua.MoonSharpHidden]
		private FileStream _file;
		[Lua.MoonSharpHidden]
		private LuaFileOpenMode _mode;

		public void close()
		{

		}

		public void flush()
		{

		}

		public void lines()
		{

		}

		public void read()
		{

		}

		public void seek()
		{

		}

		public void write()
		{

		}

		public void setvbuf()
		{

		}

		public static bool CheckMode(LuaFileOpenMode target, LuaFileOpenMode mode)
		{
			return (target & mode) == mode;
		}
	}

	[Lua.MoonSharpModule(Namespace = "io")]
	public class IOModule
	{
		public static string DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory;

		public static void MoonSharpInit(Lua.Table globalTable, Lua.Table namespaceTable)
		{

		}

		private static LuaFileOpenMode ParseMode(string str)
		{
			var mode = LuaFileOpenMode.Read;

			if (str.IndexOf('r') >= 0)
			{
				mode |= LuaFileOpenMode.Read;
			}
			else if (str.IndexOf('w') >= 0)
			{
				mode |= LuaFileOpenMode.Write;
			}
			else if (str.IndexOf('a') >= 0)
			{
				mode |= LuaFileOpenMode.Append;
			}
			else if (str.IndexOf('b') >= 0)
			{
				mode |= LuaFileOpenMode.Binary;
			}
			else if (str.IndexOf('+') >= 0)
			{
				mode |= LuaFileOpenMode.Update;
			}
			else
			{
				throw new InvalidOperationException("Unknown file access mode");
			}

			return mode;
		}

		private static bool DoesPathContainsPath(string source, string target)
		{
			var sourceFolders = source.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
			var targetFolders = target.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			if (targetFolders.Length < sourceFolders.Length)
				return false;

			for (int i = 0; i < sourceFolders.Length; i++)
			{
				if (!targetFolders[i].Equals(sourceFolders[i], StringComparison.CurrentCultureIgnoreCase))
				{
					return false;
				}
			}

			return true;
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue open(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var mode = LuaFileOpenMode.Read;

				if (args.Count > 1)
				{
					mode = ParseMode(args[1].String);
				}

				var path = args[0].String;

				if (LuaFileHandle.CheckMode(mode, LuaFileOpenMode.Append) |
					LuaFileHandle.CheckMode(mode, LuaFileOpenMode.Write) |
					LuaFileHandle.CheckMode(mode, LuaFileOpenMode.Update))
				{
					if (!DoesPathContainsPath(DefaultDirectory, path) && Path.IsPathRooted(path))
					{
						throw new FileLoadException("You can write files outside server folder");
					}
				}

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue close(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				//File.Open

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue flush(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				//File.Open

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue input(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				//File.Open

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue lines(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				//File.Open

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue output(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				//File.Open

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue read(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				//File.Open

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue tmpfile(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				//File.Open

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue type(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				//File.Open

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue write(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				//File.Open

				return null;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}
	}
}
