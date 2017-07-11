using System;
using System.Collections.Generic;
using System.Reflection;

namespace LuaConnector.ORM
{
	public class ProviderAssembly
	{
		public string Name { get; private set; }
		public HashSet<Type> TypesToRegister { get; private set; }
		public Type ProviderClass { get; private set; }

		internal Assembly Asm;

		public ProviderAssembly(Assembly ass, Type type)
		{
			this.Asm = ass;
			this.ProviderClass = type;
			this.TypesToRegister = new HashSet<Type>();
		}

		public void FindTypesToRegister()
		{
			var types = Asm.GetTypes();

			foreach(var t in types)
			{
				var attr = t.GetCustomAttribute(typeof(Attributes.PublicDefinitionAttribute));

				if (attr == null)
					continue;

				TypesToRegister.Add(t);
			}
		}

		public void GetProviderShortname()
		{
			this.Name = (string)ProviderClass.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance).GetConstantValue();
			Console.WriteLine(Name);
		}
	}
}
