using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using System.Collections;

namespace LuaConnector.ORM.Sql
{
	public class RowDataCollestion : IEnumerable<string>, ICollection<string>
	{
		public int Count
		{
			get { return _columns.Count; }
		}

		public bool IsReadOnly { get; } = false;

		private List<string> _columns;

		public RowDataCollestion()
		{
			_columns = new List<string>();
		}

		public void Add(string item)
		{
			_columns.Add(item);
		}

		public void Add(object item)
		{
			_columns.Add(item.ToString());
		}

		public string ToSql()
		{
			StringBuilder result = new StringBuilder();

			foreach(var item in _columns)
				result.AppendFormat("\"{0}\",", item);

			return result.Remove(result.Length - 1, 1).ToString();
		}

		public IEnumerator<string> GetEnumerator()
		{
			for (int i = 0; i < _columns.Count; i++)
				yield return _columns[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region Unused Interfaces

		public bool Remove(string item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(string item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
