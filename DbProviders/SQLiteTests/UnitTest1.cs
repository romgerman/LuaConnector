using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data;
using SQLite;
using LuaConnector.ORM.Sql;

namespace SQLiteTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void SimpleConnect()
		{
			SQLiteProvider provider = new SQLiteProvider();
			provider.Connect("Data Source=C:\\Code\\Trash\\mydb.db;Version=3;");
			provider.Disconnect();
		}

		[TestMethod]
		public void CreateTable()
		{
			SQLiteProvider provider = new SQLiteProvider();
			provider.Connect("Data Source=C:\\Code\\Trash\\mydb.db;Version=3;");
			provider.Table("players").CreateIfNotExists(new SqlColumnDefinition[]
			{
				new SqlColumnDefinition("id", SqlDbType.Int, true, true) { Autoinc = true },
				new SqlColumnDefinition("name", SqlDbType.Text),
				new SqlColumnDefinition("money", SqlDbType.Money)
			});
			provider.Disconnect();
		}

		[TestMethod]
		public void InsertTest()
		{
			SQLiteProvider provider = new SQLiteProvider();
			provider.Connect("Data Source=C:\\Code\\Trash\\mydb.db;Version=3;");
			provider.Table("players").CreateIfNotExists(new SqlColumnDefinition[]
			{
				new SqlColumnDefinition("id", SqlDbType.Int, true, true) { Autoinc = true },
				new SqlColumnDefinition("name", SqlDbType.Text),
				new SqlColumnDefinition("money", SqlDbType.Money)
			});
			provider.Table("players").Insert(new string[] { "name", "money" }, new RowDataCollestion()
			{
				"George", "500"
			});
			provider.Disconnect();
		}

		[TestMethod]
		public void InsertManyTest()
		{
			SQLiteProvider provider = new SQLiteProvider();
			provider.Connect("Data Source=C:\\Code\\Trash\\mydb.db;Version=3;");
			provider.Table("players").CreateIfNotExists(new SqlColumnDefinition[]
			{
				new SqlColumnDefinition("id", SqlDbType.Int, true, true) { Autoinc = true },
				new SqlColumnDefinition("name", SqlDbType.Text),
				new SqlColumnDefinition("money", SqlDbType.Money)
			});
			provider.Table("players").InsertMany(new string[] { "name", "money" }, new InsertCollection()
			{
				{ "Petty" , 1523200  },
				{ "Mark" , -10  },
			});
			provider.Disconnect();
		}

		[TestMethod]
		public void GetFirstRowAsArray()
		{
			SQLiteProvider provider = new SQLiteProvider();
			provider.Connect("Data Source=C:\\Code\\Trash\\mydb.db;Version=3;");
			provider.Table("players").CreateIfNotExists(new SqlColumnDefinition[]
			{
				new SqlColumnDefinition("id", SqlDbType.Int, true, true) { Autoinc = true },
				new SqlColumnDefinition("name", SqlDbType.Text),
				new SqlColumnDefinition("money", SqlDbType.Money)
			});

			foreach(var item in provider.Table("players").First().AsArray())
			{
				Trace.WriteLine(item + " = " + item.GetType());
			}
			
			provider.Disconnect();
		}

		[TestMethod]
		public void GetFirstRowAsDictionary()
		{
			SQLiteProvider provider = new SQLiteProvider();
			provider.Connect("Data Source=C:\\Code\\Trash\\mydb.db;Version=3;");
			provider.Table("players").CreateIfNotExists(new SqlColumnDefinition[]
			{
				new SqlColumnDefinition("id", SqlDbType.Int, true, true) { Autoinc = true },
				new SqlColumnDefinition("name", SqlDbType.Text),
				new SqlColumnDefinition("money", SqlDbType.Money)
			});

			foreach (var item in provider.Table("players").First().AsTable())
			{
				Trace.WriteLine(item.Key + " = " + item.Value + " = " + item.Value.GetType());
			}

			provider.Disconnect();
		}

		[TestMethod]
		public void GetLastInsertedRow()
		{
			SQLiteProvider provider = new SQLiteProvider();
			provider.Connect("Data Source=C:\\Code\\Trash\\mydb.db;Version=3;");
			provider.Table("players").CreateIfNotExists(new SqlColumnDefinition[]
			{
				new SqlColumnDefinition("id", SqlDbType.Int, true, true) { Autoinc = true },
				new SqlColumnDefinition("name", SqlDbType.Text),
				new SqlColumnDefinition("money", SqlDbType.Money)
			});

			foreach (var item in provider.Table("players").LastInserted().AsTable())
			{
				Trace.WriteLine(item.Key + " = " + item.Value + " = " + item.Value.GetType());
			}

			provider.Disconnect();
		}
	}
}
