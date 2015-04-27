using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentTextEditor
{
	public class UnsafeStorage : IStorage
	{
		private string[] lines;

		public UnsafeStorage(string[] lines)
		{
			this.lines = lines;
		}

		public Tuple<int, string> ReplaceFirst(string word,	string replace)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				int position = lines[i].FastIndexOf(word);
				if (position == -1)
					continue;

				lines[i] = lines[i].Substring(0, position) + replace + lines[i].Substring(position + word.Length);
				return Tuple.Create<int, string>(i, lines[i]);
			}

			return null;
		}
	}
}
