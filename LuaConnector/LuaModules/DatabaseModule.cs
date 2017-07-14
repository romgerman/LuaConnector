using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

#if !DISABLE_DB

using LuaConnector.ORM;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector.LuaModules
{
	[Lua.MoonSharpModule(Namespace = "orm")]
	internal class DatabaseModule
	{
		private static ProviderLoader _loader;

		public static void RegisterDatabaseProviders()
		{
			_loader = new ProviderLoader("Providers");
			_loader.LoadAllProviders();
			Console.WriteLine(_loader.Providers.Count);
			RegisterProvidersTypes(_loader);
		}

		private static void RegisterProvidersTypes(ProviderLoader loader)
		{
			foreach(var p in loader.Providers)
			{
				foreach (var t in p.TypesToRegister)
				{
					Lua.UserData.RegisterType(t);
				}
			}
		}
	}
}

#endif
