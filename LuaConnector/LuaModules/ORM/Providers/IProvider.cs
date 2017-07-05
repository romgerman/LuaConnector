using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaConnector.LuaModules.ORM.Providers
{
	public interface IProvider
	{
		string Name { get; }

		ITable GetTableByName(string name);
	}
}
