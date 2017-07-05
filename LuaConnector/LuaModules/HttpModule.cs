using System;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector.LuaModules
{
	internal class HttpModuleOptions
	{
		public HttpMethod RequestMethod;
		public string Url;
		public Dictionary<string, IEnumerable<string>> Headers;
		public Lua.DynValue PostBody;
	}

	internal class LuaHttpResponse
	{
		public string content;
		public int status;
		public Dictionary<string, IEnumerable<string>> headers;
	}
	
	internal class LuaHttpRequest
	{
		[Lua.MoonSharpHidden]
		private HttpClient client;
		private HttpRequestMessage request;
		private Lua.Closure callback;

		internal LuaHttpRequest(HttpModuleOptions options, Lua.Closure callback, HttpClient client)
		{
			this.callback = callback;
			this.client  = client;
			this.request = new HttpRequestMessage(options.RequestMethod, new Uri(options.Url));
			
			if (options.PostBody != null && (options.RequestMethod == HttpMethod.Post || options.RequestMethod == HttpMethod.Put))
			{
				if (options.PostBody.Type == Lua.DataType.String)
				{
					request.Content = new StringContent(options.PostBody.String);
				}
			}

			if (options.Headers != null)
			{
				foreach (var kvp in options.Headers)
				{
					if (HttpModule.IsEntityHeader(kvp.Key))
					{
						if (request.Content == null)
							request.Content = new StringContent("");

						request.Content.Headers.Add(kvp.Key, kvp.Value);
					}
					else
					{
						request.Headers.Add(kvp.Key, kvp.Value);
					}
				}
			}
		}

		public async void send()
		{
			HttpResponseMessage respose = await client.SendAsync(this.request, HttpCompletionOption.ResponseContentRead);

			var luaResponse = new LuaHttpResponse();
			luaResponse.content = await respose.Content.ReadAsStringAsync();
			luaResponse.status = (int)respose.StatusCode;
			luaResponse.headers = new Dictionary<string, IEnumerable<string>>();

			foreach(var head in respose.Headers)
				luaResponse.headers.Add(head.Key, head.Value);

			this.callback.Call(this, luaResponse);
		}
	}

	[Lua.MoonSharpModule(Namespace = "http")]
	internal class HttpModule
	{
		internal static HttpClient _client;

		public static void MoonSharpInit(Lua.Table globalTable, Lua.Table namespaceTable)
		{
			_client = new HttpClient();

			Lua.UserData.RegisterType<LuaModules.LuaHttpRequest>(Lua.InteropAccessMode.Default, "httpRequest");
			Lua.UserData.RegisterType<LuaModules.LuaHttpResponse>(Lua.InteropAccessMode.Default, "httpResponse");
		}

		/*
			This function is async
			http.request(method, url, callback)
			http.request(options, callback)
			options = method, url, headers, content
		*/

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue request(Lua.ScriptExecutionContext ctx, Lua.CallbackArguments args)
		{
			try
			{
				var options = new HttpModuleOptions();

				if (args[0].Type == Lua.DataType.Table)
				{
					var table = args[0].Table;

					options.RequestMethod = GetMethodFromString(table.Get("method").String);
					options.Url = table.Get("url").String;

					if (!table.Get("headers").IsNil())
					{
						options.Headers = new Dictionary<string, IEnumerable<string>>();

						foreach(var pair in table.Get("headers").Table.Pairs)
						{
							if (pair.Value.Type == Lua.DataType.Table)
							{
								options.Headers.Add(pair.Key.String, pair.Value.Table.Values.Select(v => v.String));
							}
							else
							{
								options.Headers.Add(pair.Key.String, new string[] { pair.Value.String });
							}
						}
					}

					if (!table.Get("content").IsNil())
					{
						options.PostBody = table.Get("content");
					}
				}
				else
				{
					var method = GetMethodFromString(args[0].String);
					var url = args[1].String;

					options.RequestMethod = method;
					options.Url = url;
				}		

				var callback = args.GetArray().Last().Function;
				var request = new LuaHttpRequest(options, callback, _client);

				return Lua.DynValue.FromObject(ctx.GetScript(), request);
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		private static HttpMethod GetMethodFromString(string name)
		{
			HttpMethod result = HttpMethod.Get;

			if (name.Equals("get", StringComparison.CurrentCultureIgnoreCase))
			{
				result = HttpMethod.Get;
			}
			else if (name.Equals("head", StringComparison.CurrentCultureIgnoreCase))
			{
				result = HttpMethod.Head;
			}
			else if (name.Equals("post", StringComparison.CurrentCultureIgnoreCase))
			{
				result = HttpMethod.Post;
			}
			else if (name.Equals("put", StringComparison.CurrentCultureIgnoreCase))
			{
				result = HttpMethod.Put;
			}
			else if (name.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
			{
				result = HttpMethod.Delete;
			}
			else if (name.Equals("options", StringComparison.CurrentCultureIgnoreCase))
			{
				result = HttpMethod.Options;
			}
			else
			{
				throw new HttpRequestException("Invalid HTTP method");
			}

			return result;
		}

		public static bool IsEntityHeader(string name)
		{
			switch (name)
			{
				case "Content-Disposition":	return true;
				case "Content-Encoding": return true;
				case "Content-Language": return true;
				case "Content-Length": return true;
				case "Content-Location": return true;
				case "Content-MD5": return true;
				case "Content-Range": return true;
				case "Content-Type": return true;
				case "Content-Version": return true;
				case "Derived-From": return true;
				case "Expires": return true;
				case "Last-Modified": return true;
				case "Link": return true;
				case "Title": return true;
				default: return false;
			}
		}
	}
}
