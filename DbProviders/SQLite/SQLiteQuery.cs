using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaConnector.ORM;
using LuaConnector.ORM.Attributes;

using System.Data;
using System.Data.SQLite;

namespace SQLite
{
	internal enum ReturnType { NoReturn, Single, Many }

	[PublicDefinition]
	public class SQLiteQuery : IQuery
	{
		private SQLiteConnection _connection;
		private ReturnType _type;
		private string _query;

		internal SQLiteQuery(string query, ReturnType type, SQLiteConnection connection)
		{
			this._connection = connection;
			this._query = query;
			this._type = type;
		}

		public void Delete()
		{
			throw new NotImplementedException();
		}

		public void Insert()
		{
			throw new NotImplementedException();
		}

		public void Select()
		{
			throw new NotImplementedException();
		}

		public void Update()
		{
			throw new NotImplementedException();
		}

		public void Where()
		{
			throw new NotImplementedException();
		}

		public object[] AsArray()
		{
			switch (_type)
			{
				case ReturnType.NoReturn:
					return null;
				case ReturnType.Single:
					return AsArraySingle();
				case ReturnType.Many:
					return null;
			}

			return null;
		}

		public Dictionary<string, object> AsTable()
		{
			switch (_type)
			{
				case ReturnType.NoReturn:
					return null;
				case ReturnType.Single:
					return AsTableSingle();
				case ReturnType.Many:
					return null;
			}

			return null;
		}

		private Dictionary<string, object> AsTableSingle()
		{
			var result = new Dictionary<string, object>();

			using (var cmd = new SQLiteCommand(_query, _connection))
			{
				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.HasRows)
						return result;

					reader.Read();

					var columns = reader.GetValues();

					for (int i = 0; i < columns.Count; i++)
						result.Add(reader.GetName(i), reader.GetValue(i));

					return result;
				}
			}
		}

		private object[] AsArraySingle()
		{
			using (var cmd = new SQLiteCommand(_query, _connection))
			{
				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.HasRows)
						return new object[0];

					reader.Read();

					var columns = reader.GetValues();
					var result = new object[columns.Count];

					for(int i = 0; i < columns.Count; i++)				
						result[i] = reader.GetValue(i);

					return result;
				}
			}
		}

	}
}
