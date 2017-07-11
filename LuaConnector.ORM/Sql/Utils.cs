using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace LuaConnector.ORM.Sql
{
	internal static class Utils
	{
		public static string TypeToSQLiteString(SqlDbType type)
		{
			switch (type)
			{
				case SqlDbType.BigInt:
					return "INTEGER";
				case SqlDbType.Bit:
					return "INTEGER";
				case SqlDbType.Int:
					return "INTEGER";
				case SqlDbType.Money:
					return "INTEGER";
				case SqlDbType.SmallMoney:
					return "INTEGER";
				case SqlDbType.SmallInt:
					return "INTEGER";
				case SqlDbType.TinyInt:
					return "INTEGER";
				case SqlDbType.Char:
					return "TEXT";
				case SqlDbType.NChar:
					return "TEXT";
				case SqlDbType.NText:
					return "TEXT";
				case SqlDbType.NVarChar:
					return "TEXT";
				case SqlDbType.Xml:
					return "TEXT";
				case SqlDbType.Text:
					return "TEXT";
				case SqlDbType.VarChar:
					return "TEXT";
				case SqlDbType.Float:
					return "REAL";
				case SqlDbType.Real:
					return "REAL";
				case SqlDbType.SmallDateTime:
					return "NUMERIC";
				case SqlDbType.DateTime:
					return "NUMERIC";
				case SqlDbType.Decimal:
					return "NUMERIC";
				case SqlDbType.Timestamp:
					return "NUMERIC";
				case SqlDbType.Date:
					return "NUMERIC";
				case SqlDbType.Time:
					return "NUMERIC";
				case SqlDbType.DateTime2:
					return "NUMERIC";
				case SqlDbType.DateTimeOffset:
					return "NUMERIC";
				case SqlDbType.Image:
					return "BLOB";
				case SqlDbType.Binary:
					return "BLOB";
				case SqlDbType.UniqueIdentifier:
					return "BLOB";
				case SqlDbType.VarBinary:
					return "BLOB";
				case SqlDbType.Variant:
					return "BLOB";
				case SqlDbType.Udt:
					return "BLOB";
				case SqlDbType.Structured:
					return "BLOB";
				default:
					return "BLOB";
			}
		}
	}
}
