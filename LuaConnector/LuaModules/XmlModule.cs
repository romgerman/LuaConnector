using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Lua = MoonSharp.Interpreter;

namespace LuaConnector.LuaModules
{
	[Lua.MoonSharpModule(Namespace = "xml")]
	internal class XmlModule
	{
		private class QueryAttribute
		{
			public string Name;
			public string Value;
			public string Left;
		}

		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue parse(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var str = args.AsType(0, "parse", Lua.DataType.String);

				var doc = new XmlDocument();
				doc.LoadXml(str.String);

				var result = RecursiveReading(doc.DocumentElement, context.GetScript());

				return Lua.DynValue.NewTable(result);
			}
			catch(Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		private static Lua.Table RecursiveReading(XmlNode node, Lua.Script script)
		{
			var table = new Lua.Table(script);
			table["name"] = Lua.DynValue.NewString(node.Name);

			if (node.Attributes.Count != 0)
			{
				table["attributes"] = new Lua.Table(script);

				for (int i = 0; i < node.Attributes.Count; i++)
				{
					var attr = node.Attributes.Item(i);
					table["attributes", attr.Name] = Lua.DynValue.NewString(attr.Value);
				}
			}

			if (node.HasChildNodes)
			{
				if (node.ChildNodes.Count == 1 && node.ChildNodes.Item(0).NodeType == XmlNodeType.Text)
				{
					table["child"] = node.ChildNodes.Item(0).Value;
				}
				else
				{
					var nodes = new Lua.Table(script);

					for (int i = 0; i < node.ChildNodes.Count; i++)
					{
						var child = node.ChildNodes.Item(i);

						if (child.NodeType == XmlNodeType.Text)
						{
							nodes.Append(Lua.DynValue.NewString(child.Value));
							continue;
						}

						nodes.Append(Lua.DynValue.NewTable(RecursiveReading(child, script)));
					}

					table["child"] = nodes;
				}
			}

			return table;
		}

		// [+] xml.get(document, "settings/graphics/resolution") -- get value of "resolution"
		// [+] xml.get(document, "settings/graphics/resolution/!") -- get raw value of "resolution"
		// [+] xml.get(document, "inventory/items/5")			 -- get fifth "item"
		// [+] xml.get(document, "inventory/items/[3]")			 -- get first 3 elements
		// [+] xml.get(document, "inventory[]") -- get all attributes of "inventory"
		// [+] xml.get(document, "inventory[max_items]") -- get "max_items" attribute of "inventory"
		// xml.get(document, "inventory/items/[type]") -- get all elements with attribute "type"
		// xml.get(document, "inventory/items/[description=Blood of Hagal]") -- get all elements with "description" attribute equals specified text
		/*
			{
				name: "settings",
				child: {
					1: {
						name: "graphics",
						child: {
							1: {
								name: "resolution",
								child: "1280x760"
							},
							2: {
								name: "shadows",
								child: 
							}
						}
					},
					2: {
						name: "sound"
					}
				}
			}
		*/
		[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue get(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{
			try
			{
				var table = args.AsType(0, "get", Lua.DataType.Table);
				var query = args.AsType(1, "get", Lua.DataType.String);

				var queryArray = query.String.Split(new [] { '/' }, StringSplitOptions.RemoveEmptyEntries);
				var result = table.Table;

				if (queryArray.Length == 0)
					return Lua.DynValue.Nil;

				if (queryArray.Length == 1 && result.Get("name").String == queryArray[0])
					return Lua.DynValue.NewTable(result);

				var len = queryArray.Length;

				for(int i = 1; i < len; i++)
				{
					// Last element from query is number. Get element by specified index
					if (IsDigitsOnly(queryArray[i]))
					{
						if (result.Get("child").Type == Lua.DataType.Table)
						{
							var index = int.Parse(queryArray[i]);

							if (i == (len - 1))
								return result.Get("child").Table.Get(index);

							result = result.Get("child").Table.Get(index).Table;
							continue;
						}

						return Lua.DynValue.Nil;
					}

					var attr = QueryHasAttributeDefinition(queryArray[i]);

					if (attr != null)
					{
						if (!string.IsNullOrWhiteSpace(attr.Name) &&
							attr.Value == null &&
							IsDigitsOnly(attr.Name)) // When it's something/[341]
						{
							if (result.Get("child").Type == Lua.DataType.Table)
							{
								var amount = int.Parse(attr.Name);
								var count = result.Get("child").Table.Pairs.Count();
								amount = amount > count ? count : amount;

								var temp = new Lua.Table(context.GetScript(), result.Get("child").Table.Values.Take(amount).ToArray());

								if (i == (len - 1))
									return Lua.DynValue.NewTable(temp);
								
								var name = result.Get("name");
								result = new Lua.Table(context.GetScript());
								result["name"]  = name;
								result["child"] = temp;

								continue;
							}

							return Lua.DynValue.Nil;
						}
						else if (string.IsNullOrWhiteSpace(attr.Name) &&
								 string.IsNullOrWhiteSpace(attr.Value) &&
								 attr.Left != null) // When it's something[]
						{
							var nextElement = FindNextElement(attr.Left, result);

							if (nextElement == null)
								return Lua.DynValue.Nil;

							return nextElement.Get("attributes");
						}
						else if(!string.IsNullOrWhiteSpace(attr.Name) &&
								attr.Left != null)
						{
							var nextElement = FindNextElement(attr.Left, result);

							if (nextElement == null || nextElement.Get("attributes").IsNil())
								return Lua.DynValue.Nil;

							return nextElement.Get("attributes").Table.Get(attr.Name);
						}
					}

					if (queryArray[i] == "!")
						return Lua.DynValue.NewTable(result);

					var next = FindNextElement(queryArray[i], result);

					if (next == null)
						return Lua.DynValue.Nil;

					result = next;
				}

				if (result.Get("child").IsNil())
					return Lua.DynValue.Nil;

				if (result.Get("child").Type != Lua.DataType.Table)
					return result.Get("child");

				return Lua.DynValue.NewTable(result);
			}
			catch (Lua.SyntaxErrorException ex)
			{
				throw new Lua.ScriptRuntimeException(ex);
			}
		}

		private static Lua.Table FindNextElement(string name, Lua.Table current)
		{
			foreach (var p in current.Get("child").Table.Pairs)
				if (p.Value.Table.Get("name").String == name)
					return p.Value.Table;

			return null;
		}

		private static bool IsDigitsOnly(string str)
		{
			foreach (var c in str)
				if (!char.IsDigit(c))
					return false;

			return true;
		}

		private static QueryAttribute QueryHasAttributeDefinition(string element)
		{
			var start = element.IndexOf('[');
			var end = element.IndexOf(']');

			if (start < 0 || end < 0)
				return null;

			var attr = new QueryAttribute();
			var eq = element.IndexOf('=');

			if (start != 0)
				attr.Left = element.Substring(0, start);

			if (eq < 0)
			{
				if ((end - start) > 1)
					attr.Name = element.Substring(start + 1, end - start - 1);
			}
			else
			{
				attr.Name = element.Substring(start + 1, eq - start - 1);
				attr.Value = element.Substring(eq + 1, end - eq - 1);
			}

			return attr;
		}

		/*[Lua.MoonSharpModuleMethod]
		public static Lua.DynValue serialize(Lua.ScriptExecutionContext context, Lua.CallbackArguments args)
		{

		}*/
	}
}
