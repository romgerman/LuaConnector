using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector
{
	static class Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NetHandle ToNetHandle(this Lua.DynValue val)
		{
			return val.ToObject<NetHandle>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Client ToClient(this Lua.DynValue val)
		{
			return val.ToObject<Client>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3(this Lua.DynValue val)
		{
			return val.ToObject<Vector3>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Lua.Table GetTable(this Lua.Script script, Lua.DynValue key)
		{
			return script.Globals.Get(key).Table;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Lua.Table GetTable(this Lua.Script script, string key)
		{
			return script.Globals.Get(key).Table;
		}

		public static string[] SplitCmd(this string str)
		{
			str = str.Substring(1);

			List<string> result = new List<string>();
			int index = 0;
			bool quote = false;
			string current = "";

			while (index < str.Length)
			{
				if (str[index] == ' ' && !quote)
				{
					result.Add(current);
					current = "";
				}
				else if (str[index] == '"')
				{
					if (quote)
					{
						result.Add(current);
						current = "";
					}

					quote = quote ? false : true;
				}
				else
				{
					current += str[index];
				}

				index++;
			}

			if (current != string.Empty)
				result.Add(current);

			return result.ToArray();
		}

		/// <summary>
		/// Makes "something/something/something"
		/// </summary>
		public static string Urlify(this string str, params string[] arr)
		{
			var result = str + "/";

			for(int i = 0; i < arr.Length; i++)
			{
				result += arr[i];

				if (i != arr.Length - 1)
					result += "/";
			}

			return result;
		}
	}
}
