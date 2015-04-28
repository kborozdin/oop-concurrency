using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using ConcurrentTextEditor;

namespace Server
{
	public class TextEditorService
	{
		private readonly SafeStorage storage;
		private readonly AbstractServer server;
		private readonly Action waiter;
		private readonly Action<string> logger;

		public TextEditorService(Func<string, Func<HttpListenerRequest, string>, AbstractServer> createServer,
			Action waiter, Action<string> logger, string[] lines)
		{
			storage = new SafeStorage(lines);
			server = createServer("http://localhost:80/", GetResponse);
			this.waiter = waiter;
			this.logger = logger;
		}

		public void Run()
		{
			server.Start();

			logger("Serving...");
			logger("Press any key to exit");
			waiter();

			server.Stop();
		}

		private string GetResponse(HttpListenerRequest request)
		{
			var form = File.ReadAllText("form.html");

			string data;
			using (var reader = new StreamReader(request.InputStream))
				data = reader.ReadToEnd();
			var post = HttpUtility.ParseQueryString(data);

			string word = post["word"];
			string replace = post["replace"];

			if (word == null || replace == null)
				form = string.Format(form, "(no result)");
			else
			{
				var result = storage.ReplaceFirst(word, replace);

				if (result == null)
					form = string.Format(form, "(null)");
				else
					form = string.Format(form, result.ToString());
			}

			return form;
		}
	}
}
