using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaConnector.ORM;
using LuaConnector.ORM.Sql;
using LuaConnector.ORM.Providers;
using LuaConnector.ORM.Attributes;

using System.Data;
using System.Data.SQLite;

namespace SQLite
{
	[PublicDefinition]
	public class SQLiteTable : ITable
	{
		private SQLiteConnection _connection;
		private string _name;

		internal SQLiteTable(string name, SQLiteConnection connection)
		{
			this._connection = connection;
			this._name = name;
		}
		
		public ITable Create(SqlColumnDefinition[] columns)
		{
			InternalCreate(columns, false);
			return this;
		}
		
		public ITable CreateIfNotExists(SqlColumnDefinition[] columns)
		{
			InternalCreate(columns, true);
			return this;
		}
		
		public void Insert(string[] columns, RowDataCollestion values)
		{
			StringBuilder result = new StringBuilder();

			result.AppendFormat("INSERT INTO {0} ", _name);

			if (columns != null)
			{
				result.Append("(");

				foreach (var col in columns)
				{
					result.AppendFormat("{0},", col);
				}

				result = result.Remove(result.Length - 1, 1);

				result.Append(") ");
			}

			result.Append("VALUES (");
			result.Append(values.ToSql());
			result.Append(");");

			using (SQLiteCommand cmd = new SQLiteCommand(result.ToString(), _connection))
				cmd.ExecuteNonQuery();
		}
		
		public void InsertMany(string[] columns, InsertCollection values)
		{
			StringBuilder result = new StringBuilder();

			result.Append("BEGIN TRANSACTION;");

			foreach(var rowData in values)
			{
				result.AppendLine();
				result.AppendFormat("INSERT INTO {0} ", _name);

				if (columns != null)
				{
					result.Append("(");

					foreach (var col in columns)
						result.AppendFormat("{0},", col);

					result = result.Remove(result.Length - 1, 1);

					result.Append(") ");
				}

				result.Append("VALUES (");
				result.Append(rowData.ToSql());
				result.Append(");");
			}

			result.AppendLine();
			result.Append("COMMIT;");

			using (SQLiteCommand cmd = new SQLiteCommand(result.ToString(), _connection))
				cmd.ExecuteNonQuery();
		}

		public void Select(string[] columns)
		{
			throw new NotImplementedException();
		}
		
		public IQuery First()
		{
			return new SQLiteQuery($"SELECT * FROM {_name} LIMIT 1", ReturnType.Single, _connection);
		}
		
		public IQuery LastInserted()
		{
			return new SQLiteQuery($"SELECT * FROM {_name} WHERE rowid = (SELECT MAX(rowid) FROM {_name});", ReturnType.Single, _connection);
		}

		public IQuery Where(string condition)
		{
			throw new NotImplementedException();
		}
		
		public int Drop()
		{
			SQLiteCommand cmd = new SQLiteCommand($"DROP TABLE {_name}", _connection);
			return cmd.ExecuteNonQuery();
		}


		private void InternalCreate(SqlColumnDefinition[] columns, bool ifNotExists)
		{
			StringBuilder result = new StringBuilder(columns.Length);

			result.Append("CREATE TABLE ");
			if (ifNotExists)
				result.Append("IF NOT EXISTS ");
			result.AppendFormat("{0}(", _name);

			foreach (var col in columns)
			{
				result.Append(col.Name);
				result.Append(' ');
				result.Append(col.TypeToSQLiteString());
				result.Append(' ');
				if (col.PrimaryKey)
					result.Append("PRIMARY KEY ");
				if (col.Autoinc)
					result.Append("AUTOINCREMENT ");
				if (col.NotNull)
					result.Append("NOT NULL ");
				if (col.Unique)
					result.Append("UNIQUE ");
				if (col.Default != null)
					result.AppendFormat("DEFAULT {0}", col.Default);
				if (col.Check != null)
					result.AppendFormat("CHECK({0})", col.Check);
				result.Append(',');
			}

			result = result.Remove(result.Length - 1, 1).Append(");");

			using (SQLiteCommand cmd = new SQLiteCommand(result.ToString(), _connection))
				cmd.ExecuteNonQuery();
		}
	}
}
