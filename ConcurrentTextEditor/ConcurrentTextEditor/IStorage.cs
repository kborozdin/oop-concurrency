using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentTextEditor
{
	public interface IStorage
	{
		Tuple<int, string> ReplaceFirst(string word, string replace);
	}
}
