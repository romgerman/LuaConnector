using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaConnector.ORM
{
	public interface IQuery
	{
		void Insert();
		void Select();
		void Update();
		void Delete();
		void Where();

		Dictionary<string, object> AsTable();
		object[] AsArray();
	}
}
