using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaConnector.LuaModules.ORM
{
	public interface ITable
	{
		void Insert();
		void Update();
		void Delete();
		void Get();
		void First();
		void Last();
		void All();
	}
}
