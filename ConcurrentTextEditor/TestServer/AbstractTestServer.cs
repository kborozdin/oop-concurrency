using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using ConcurrentTextEditor;
using System.Threading;

namespace TestServer
{
	public abstract class AbstractTestServer
	{
		private readonly Random random = new Random(1234);
		protected Func<string, Func<HttpListenerRequest, string>, AbstractServer> createServer;

		public abstract void Initialize();

		[TestMethod]
		public void TestServiceHeavily()
		{
			var lines = new string[(int)1e3];
			for (int i = 0; i < lines.Length; i++)
				lines[i] = random.GenerateRandomWord((int)1e4, 3);

			var stopEvent = new AutoResetEvent(false);
			new Thread(() =>
			{
				var service = new TextEditorService(createServer, () => stopEvent.WaitOne(), s => { }, lines);
				service.Run();
			}).Start();
			Thread.Sleep(500);

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://localhost/");

				var tasks = new List<Task>();
				for (int i = 0; i < 200; i++)
				{
					var content = new FormUrlEncodedContent(new[]
					{
						new KeyValuePair<string, string>("word", "d"),
						new KeyValuePair<string, string>("replace", "")
					});
					tasks.Add(client.PostAsync("/", content));
				}

				Task.WaitAll(tasks.ToArray());
			}

			stopEvent.Set();
			Thread.Sleep(500);
		}
	}
}
