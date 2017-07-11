using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LuaConnector.ORM
{
	public class ProviderLoader
	{
		public List<ProviderAssembly> Providers { get; private set; }

		private string _folder;

		public ProviderLoader(string folder)
		{
			this._folder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), folder);
			this.Providers = new List<ProviderAssembly>();
		}

		public void LoadAllProviders()
		{
			if (!Directory.Exists(_folder))
				return;

			string[] folders = Directory.GetDirectories(_folder);

			if (folders.Length == 0)
				return;

			// Search in folders so main folder it wont look like dump
			foreach(var fl in folders)
			{
				string[] files = Directory.GetFiles(fl, "*.dll");

				if (files.Length == 0)
					continue;

				foreach (var f in files)
				{
					if (SkipDefaultLibs(f))
						continue;

					Assembly ass = Assembly.LoadFrom(f);
					Type type = IsProviderAssembly(ass);


					Console.WriteLine(f);

					if (type != null)
					{
						var provider = new ProviderAssembly(ass, type);
						provider.FindTypesToRegister();
						provider.GetProviderShortname();

						Console.WriteLine("HAS SHIT");

						Providers.Add(provider);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Check if assembly has type that implements IProvider interface
		/// </summary>
		private	Type IsProviderAssembly(Assembly ass)
		{
			var tps = ass.GetTypes();

			foreach(var t in tps)
			{
				Console.WriteLine(t);
				if (t.IsAssignableFrom(typeof(Providers.IProvider)))
				{
					Console.WriteLine("YES"); // Why not? :C
					return t;
				}
					
			}

			return null;
		}

		/// <summary>
		/// Skips Microsoft assemblies and LuaConnector if there is one
		/// </summary>
		private bool SkipDefaultLibs(string path)
		{
			var name = Path.GetFileNameWithoutExtension(path);

			return name.StartsWith("System.") || name.StartsWith("LuaConnector.");
		}

	}
}
