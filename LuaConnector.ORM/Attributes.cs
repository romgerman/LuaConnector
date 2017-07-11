using System;

namespace LuaConnector.ORM.Attributes
{
	/// <summary>
	/// Indicated if LuaConnector should register a class/method in Lua so user will be able to use it
	/// <para>For example you have "Create" method in your provider class and it returns a table class. Then table class and "Create" method should be marked with this attribute</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class PublicDefinitionAttribute : Attribute
	{
		public PublicDefinitionAttribute() { }
	}
}
