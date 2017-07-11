using System;
using System.Collections;
using System.Collections.Generic;

namespace LuaConnector.ORM.Sql
{
	public class InsertCollection : IEnumerable<RowDataCollestion>, ICollection<RowDataCollestion>
	{
		public RowDataCollestion First
		{
			get { return _insertions[0]; }
		}

		public int Count
		{
			get	{ return _insertions.Count;	}
		}

		public bool IsReadOnly { get; } = false;

		private List<RowDataCollestion> _insertions;
		
		public InsertCollection()
		{
			_insertions = new List<RowDataCollestion>();
		}

		public void Add(RowDataCollestion values)
		{
			_insertions.Add(values);
		}

		public void Add(params object[] values)
		{
			var data = new RowDataCollestion();

			foreach (var i in values)
				data.Add(i);

			_insertions.Add(data);
		}

		public IEnumerator<RowDataCollestion> GetEnumerator()
		{
			for (int i = 0; i < _insertions.Count; i++)
				yield return _insertions[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region Unused Interfaces

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(RowDataCollestion item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(RowDataCollestion[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(RowDataCollestion item)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
