using System;

namespace LuaConnector
{
	interface IScript
	{
		void CallFunction(string @namespace, string function, params object[] args);
	}
}
