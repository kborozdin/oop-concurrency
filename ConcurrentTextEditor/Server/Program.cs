using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	class Program
	{
		static void Main(string[] args)
		{
			var service = new TextEditorService((p, r) => new TaskBasedServer(p, r),
				new[] { "abc", "def" });
			service.Run();
		}
	}
}
