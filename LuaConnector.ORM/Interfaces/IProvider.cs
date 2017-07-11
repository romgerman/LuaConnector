using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaConnector.ORM.Providers
{
	public interface IProvider
	{
		string Name { get; }

		void Connect(string connectionString);
		void Disconnect();

		int ExecuteNonQuery(string query);

		ITable Table(string name);
	}
}
