using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentTextEditor
{
	public class SafeStorage : IStorage
	{
		private string[] lines;
		private object[] mutex;

		public SafeStorage(string[] lines)
		{
			this.lines = lines;
			mutex = new object[lines.Length];
			for (int i = 0; i < lines.Length; i++)
				mutex[i] = new object();
		}

		public Tuple<int, string> ReplaceFirst(string word, string replace)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				lock (mutex[i])
				{
					int position = lines[i].FastIndexOf(word);
					if (position == -1)
						continue;

					lines[i] = lines[i].Substring(0, position) + replace + lines[i].Substring(position + word.Length);
					return Tuple.Create<int, string>(i, lines[i]);
				}
			}

			return null;
		}
	}
}
