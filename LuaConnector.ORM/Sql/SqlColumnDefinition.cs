using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace LuaConnector.ORM.Sql
{
	public class SqlColumnDefinition
	{
		public SqlDbType Type  { get; set; }
		public string Name     { get; set; }

		public bool PrimaryKey { get; set; }
		public bool NotNull    { get; set; }
		public bool Unique     { get; set; }
		public bool Autoinc    { get; set; }
		public string Default  { get; set; }
		public string Check    { get; set; }

		public SqlColumnDefinition(string name, SqlDbType type, bool notNull = true, bool primaryKey = false)
		{
			this.Name = name;
			this.Type = type;
			this.NotNull = notNull;
			this.PrimaryKey = primaryKey;
		}

		public string TypeToSQLiteString()
		{
			return Utils.TypeToSQLiteString(Type);
		}
	}
}
