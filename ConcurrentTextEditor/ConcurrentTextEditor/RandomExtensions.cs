using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentTextEditor
{
	public static class RandomExtensions
	{
		public static string GenerateRandomWord(this Random random, int length, int alphabetSize)
		{
			StringBuilder word = new StringBuilder();
			for (int i = 0; i < length; i++)
				word.Append((char)('a' + random.Next(alphabetSize)));
			return word.ToString();
		}
	}
}
