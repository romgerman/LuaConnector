using System;
using System.Collections.Generic;
using System.Data;

using LuaConnector.ORM.Sql;

namespace LuaConnector.ORM
{
	public interface ITable
	{
		ITable Create(SqlColumnDefinition[] columns);
		ITable CreateIfNotExists(SqlColumnDefinition[] columns);
		void Insert(string[] columns, RowDataCollestion values);
		void InsertMany(string[] columns, InsertCollection values);
		void Select(string[] columns);
		IQuery Where(string condition);
		IQuery First();
		IQuery LastInserted();
		int Drop();
	}
}
