using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Json = Newtonsoft.Json;

using Server = GrandTheftMultiplayer.Server;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector.LuaModules
{
	internal class MenuItem
	{
		[Json.JsonProperty("id")]
		public string Id;

		[Json.JsonProperty("key")]
		public string Key;
		[Json.JsonProperty("value")]
		public string Value;

		[Json.JsonIgnore]
		public Lua.Closure Click;
	}
	
	internal class MenuDefinition
	{
		[Json.JsonProperty("title")]
		public string Title;
		[Json.JsonProperty("subtitle")]
		public string Subtitle;
		[Json.JsonProperty("button")]
		public string Button;

		[Json.JsonIgnore]
		public Lua.Closure Click;

		[Json.JsonProperty("items")]
		public List<MenuItem> Items;

		[Json.JsonProperty("x")]
		public int X;
		[Json.JsonProperty("y")]
		public int Y;

		public string ToJson()
		{
			return Json.Linq.JObject.FromObject(this).ToString();
		}
	}

	internal class MenuHandler
	{
		public bool IsOpen;
	}

	internal class MenuManager
	{
		public string Id { get; private set; }
		public Dictionary<string, MenuDefinition> Menus { get { return menus; } }

		public static Utils.NameGenerator NameGen { get; } = new Utils.NameGenerator();

		private Dictionary<string, MenuDefinition> menus;
		private Dictionary<Server.Elements.Client, Dictionary<string, MenuHandler>> clients;

		public MenuManager()
		{
			Id = Guid.NewGuid().ToString().Replace("-", "");
			menus = new Dictionary<string, MenuDefinition>();
			clients = new Dictionary<Server.Elements.Client, Dictionary<string, MenuHandler>>();
		}

		public bool AddMenu(string name, MenuDefinition def)
		{
			if (menus.ContainsKey(name))
				return false;

			menus.Add(name, def);

			return true;
		}

		public MenuDefinition GetMenu(string name)
		{
			if (menus.ContainsKey(name))
				return menus[name];

			return null;
		}

		public void RemoveMenu(string name)
		{
			menus.Remove(name);
		}

		public static MenuDefinition GetDefFromTable(Lua.Table table)
		{
			if (table.Pairs.Count() == 0)
				return null;

			var def = new MenuDefinition();

			def.Title = table.Get("title").CastToString() ?? "";
			def.Subtitle = table.Get("subtitle").CastToString() ?? "";
			def.Button = table.Get("button").CastToString();
			def.Click = table.Get("click").Function;

			var x = table.Get("x").CastToNumber();
			var y = table.Get("y").CastToNumber();

			def.X = x.HasValue ? (int)x.Value : 0;
			def.Y = y.HasValue ? (int)y.Value : 0;

			if (!table.Get("items").IsNil() && table.Get("items").Type == Lua.DataType.Table)
			{
				def.Items = new List<MenuItem>();

				var items = table.Get("items").Table;

				foreach(var i in items.Pairs)
				{
					def.Items.Add(new MenuItem()
					{
						Id = NameGen.GenerateNewName(),
						Key = i.Value.Table.Get("key").CastToString() ?? "",
						Value = i.Value.Table.Get("value").CastToString() ?? "",
						Click = i.Value.Table.Get("click").Function
					});
				}
			}

			return def;
		}

		public void RegisterClient(Server.Elements.Client client, string menu)
		{
			if (!clients.ContainsKey(client))
				clients.Add(client, new Dictionary<string, MenuHandler>());

			if (!clients[client].ContainsKey(menu))
				clients[client].Add(menu, new MenuHandler());
		}

		public void UnregisterClientMenu(Server.Elements.Client client, string menu)
		{
			if (!clients.ContainsKey(client))
				return;

			clients[client].Remove(menu);
		}

		public void UnregisterClient(Server.Elements.Client client)
		{
			clients.Remove(client);
		}

		public void RemoveAllMenus()
		{
			menus.Clear();
		}
	}

	/*
		Every menu has it's own guid so no events collision
	*/

	[Lua.MoonSharpModule(Namespace = "cmenu")]
	internal class ClientsideMenuModule
	{
		private static Dictionary<Lua.Script, MenuManager> managers = new Dictionary<Lua.Script, MenuManager>();
		
		/*
			Menu definition:
			{
				title: string,
				subtitle: string,
				button: string,
				click: function,
				x: number,
				y: number,
				items: [
					{
						id: string,
						key: string,
						value: any,
						click: function
					}
				]
			}
		*/
		
		/// <summary>
		/// Registers a new menu (sends it to the client with triggerClientEvent)
		/// <para>cmenu.register(name, menuDefinition)</para>
		/// </summary>
		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue register(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "register", Lua.DataType.String);
				var menuDef = args.AsType(1, "register", Lua.DataType.Table);

				if (!managers.ContainsKey(context.GetScript()))
					managers.Add(context.GetScript(), new MenuManager());

				if (!managers[context.GetScript()].AddMenu(name.String, MenuManager.GetDefFromTable(menuDef.Table)))
					throw new Lua.ScriptRuntimeException("There is already a menu with that name");

				RegisterMenuForAllClients(managers[context.GetScript()], name.String);

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue unregister(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "unregister", Lua.DataType.String);

				UnregisterMenuForAllClients(managers[context.GetScript()], name.String);
				managers[context.GetScript()].RemoveMenu(name.String);

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue open(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var name = args.AsType(0, "open", Lua.DataType.String);
				var client = args.AsUserData<Server.Elements.Client>(1, "open");

				ShowMenuForClient(client, managers[context.GetScript()], name.String);

				return Lua.DynValue.Nil;
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
				var name = args.AsType(0, "close", Lua.DataType.String);
				var client = args.AsUserData<Server.Elements.Client>(1, "close");

				HideMenuForClient(client, managers[context.GetScript()], name.String);

				return Lua.DynValue.Nil;
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		public static void ProcessMenuEvents(string eventName)
		{
			if (eventName.IndexOf('/') <= 0)
				return;

			var args = eventName.Split('/');

			var manager = managers.Where((x) => x.Value.Id == args[0]).ToArray();

			if (manager.Length > 0)
			{
				if (args[1] == "click")
				{
					var menu = manager[0].Value.GetMenu(args[2]);

					var item = menu.Items.Where((x) => x.Id == args[3]).ToArray();

					if (item.Length > 0)
					{
						item[0].Click.Call();
					}
				}
			}
		}

		public static void RegisterMenuHandlersForClient(Server.Elements.Client client)
		{
			foreach(var kvp in managers)
				LuaConnector.Instance.API.triggerClientEvent(client, "LUA_RegisterMenuHandler", kvp.Value.Id);
		}

		internal static void RegisterMenuForClient(Server.Elements.Client client, MenuManager manager, string name)
		{
			LuaConnector.Instance.API.triggerClientEvent(client, manager.Id.Urlify("register", name), manager.GetMenu(name).ToJson());
			manager.RegisterClient(client, name);
		}

		internal static void RegisterMenuForAllClients(MenuManager manager, string name)
		{
			var players = LuaConnector.Instance.API.getAllPlayers();

			for (int i = 0; i < players.Count; i++)
			{
				RegisterMenuForClient(players[i], manager, name);
			}
		}

		public static void RegisterAllMenusForClient(Server.Elements.Client client)
		{
			foreach (var manager in managers)
			{
				foreach (var kvp in manager.Value.Menus)
				{
					RegisterMenuForClient(client, manager.Value, kvp.Key);
				}
			}
		}

		internal static void UnregisterMenuForClient(Server.Elements.Client client, MenuManager manager, string name)
		{
			LuaConnector.Instance.API.triggerClientEvent(client, manager.Id.Urlify("unregister", name));
			manager.UnregisterClientMenu(client, name);
		}

		internal static void UnregisterMenuForAllClients(MenuManager manager, string name)
		{
			var players = LuaConnector.Instance.API.getAllPlayers();

			for (int i = 0; i < players.Count; i++)
			{
				UnregisterMenuForClient(players[i], manager, name);
			}
		}

		public static void UnregisterClient(Server.Elements.Client client)
		{
			foreach(var kvp in managers)
				kvp.Value.UnregisterClient(client);
		}

		public static void RemoveAllMenus()
		{
			foreach(var manager in managers)
			{
				foreach (var kvp in manager.Value.Menus)
				{
					UnregisterMenuForAllClients(manager.Value, kvp.Key);
				}

				manager.Value.RemoveAllMenus();
			}
		}

		internal static void ShowMenuForClient(Server.Elements.Client client, MenuManager manager, string name)
		{
			LuaConnector.Instance.API.triggerClientEvent(client, manager.Id.Urlify("open", name));
		}

		internal static void HideMenuForClient(Server.Elements.Client client, MenuManager manager, string name)
		{
			LuaConnector.Instance.API.triggerClientEvent(client, manager.Id.Urlify("close", name));
		}
	}
}
