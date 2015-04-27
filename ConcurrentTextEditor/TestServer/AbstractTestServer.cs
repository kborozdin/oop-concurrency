using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace TestServer
{
	public abstract class AbstractTestServer
	{
		protected Func<string, Func<HttpListenerRequest, string>, AbstractServer> createServer;

		public abstract void Initialize();

		[TestMethod]
		public void TestServiceHeavily()
		{
			var lines = new string[(int)1e5];
			for (int i = 0; i < lines.Length; i++)
				lines[i] = new string('a', 10);

			var service = new TextEditorService(createServer, lines);

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://localhost/");
				var content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("word", "b"),
					new KeyValuePair<string, string>("replace", "")
				});
				for (int i = 0; i < (int)2e5; i++)
					client.PostAsync("/", content);
			}
		}
	}
}
