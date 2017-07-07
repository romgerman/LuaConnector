using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaConnector.LuaModules.ORM;
using LuaConnector.LuaModules.ORM.Providers;

namespace SQLite
{
    public class SQLiteProvider : IProvider
    {
		public string Name { get; } = "sqlite";

		public SQLiteProvider()
		{
			
		}

		public ITable GetTableByName(string name)
		{
			throw new NotImplementedException();
		}
	}
}
