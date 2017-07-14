using System;

using Server = GrandTheftMultiplayer.Server;
using Shared = GrandTheftMultiplayer.Shared;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector.Utilities
{
	internal static class Utils
	{
		public static Shared.NetHandle TryGetHandleFromEntity(Lua.DynValue entity)
		{
			if (entity.UserData.Object.GetType().IsSubclassOf(typeof(Server.Elements.Entity)))
				return ((Server.Elements.Entity)entity.ToObject()).handle;
			if (entity.UserData.Object.GetType().Equals(typeof(Server.Elements.Client)))
				return entity.ToClient().handle;

			return entity.ToNetHandle();
		}
	}
}
