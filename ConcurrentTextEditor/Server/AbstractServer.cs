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

		public AbstractServer(string prefix, Func<HttpListenerRequest, string> getResponse)
		{
			this.getResponse = getResponse;
			listener = new HttpListener();
			listener.Prefixes.Add(prefix);
		}

		protected abstract void AcceptClient(HttpListenerContext context);

		private async void Serve()
		{
			while (listener.IsListening)
			{
				try
				{
					var context = await listener.GetContextAsync();
					AcceptClient(context);
				}
				catch (HttpListenerException)
				{
				}
			}
		}

		public void Start()
		{
			listener.Start();
			Serve();
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
			listener.Stop();
		}
	}
}
