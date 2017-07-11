using System;
using System.IO;

using LuaConnector.ORM;
using LuaConnector.ORM.Providers;
using LuaConnector.ORM.Attributes;

using System.Data.Common;
using System.Data.SQLite;

namespace SQLite
{
    public class SQLiteProvider : IProvider
    {
		public string Name { get; } = "sqlite";

		private SQLiteConnection _connection;

		public SQLiteProvider() { }
		
		public void Connect(string connectionString)
		{
			_connection = new SQLiteConnection(connectionString);

			var filename = FindFilenameInConnectionString(connectionString);

			if (filename != null && !File.Exists(filename))
				SQLiteConnection.CreateFile(filename);

			_connection.Open();
		}
		
		public void Disconnect()
		{
			_connection.Close();
		}
		
		public int ExecuteNonQuery(string query)
		{
			SQLiteCommand command = new SQLiteCommand(query, _connection);

			return command.ExecuteNonQuery();
		}
		
		public ITable Table(string name)
		{
			return new SQLiteTable(name, _connection);
		}		

		private string FindFilenameInConnectionString(string str)
		{
			string[] parameters = str.Split(';');

			foreach (var p in parameters)
				if (p.IndexOf("Data Source") > -1)
					return p.Substring(p.IndexOf('=') + 1);

			return null;
		}
	}
}
