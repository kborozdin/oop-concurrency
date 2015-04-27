using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentTextEditor
{
	public static class StringExtensions
	{
		public static int FastIndexOf(this string hay, string needle)
		{
			if (needle == "")
				return 0;

			string text = needle + "\0" + hay;
			var z = new int[text.Length];
			int left = 0, right = -1;

			for (int i = 1; i < text.Length; i++)
			{
				if (i <= right)
					z[i] = Math.Min(right - i + 1, z[i - left]);
				while (i + z[i] < text.Length && text[z[i]] == text[i + z[i]])
					z[i]++;
				if (z[i] == needle.Length)
					return i - needle.Length - 1;
				if (i + z[i] - 1 > right)
				{
					left = i;
					right = i + z[i] - 1;
				}
			}

			return -1;
		}
	}
}
