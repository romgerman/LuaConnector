using System;

namespace LuaConnector.Utilities
{
	class NameGenerator
	{
		char[] allowedChars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
										   'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
										   '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

		int index = 1;

		public string GenerateNewName()
		{
			var len = allowedChars.Length;
			var result = string.Empty;

			var n = index;

			while (n > 0)
			{
				var letter = n % len;
				if (letter == 0)
					letter = len;

				result = allowedChars[letter - 1] + result;
				n = ((n - 1) / len);
			}

			index++;

			return result;
		}
	}
}
