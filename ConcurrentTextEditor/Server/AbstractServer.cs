using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
	public abstract class AbstractServer
	{
		protected readonly HttpListener listener;
		protected readonly Func<HttpListenerRequest, string> getResponse;
		protected Thread serveThread;

		public AbstractServer(string prefix, Func<HttpListenerRequest, string> getResponse)
		{
			this.getResponse = getResponse;
			listener = new HttpListener();
			listener.Prefixes.Add(prefix);
		}

		protected abstract void Serve();

		public void Start()
		{
			listener.Start();
			serveThread = new Thread(Serve);
			serveThread.Start();
		}

		protected void ProcessClient(HttpListenerContext context)
		{
			var request = context.Request;

			var response = context.Response;
			string responseString = getResponse(request);

			var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
			response.ContentLength64 = buffer.Length;

			System.IO.Stream output = response.OutputStream;
			output.Write(buffer, 0, buffer.Length);
			output.Close();
		}

		public void Stop()
		{
			serveThread.Abort();
			listener.Stop();
		}
	}
}
