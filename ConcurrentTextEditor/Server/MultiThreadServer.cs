using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
	public class MultiThreadServer : AbstractServer
	{
		public MultiThreadServer(string prefix, Func<HttpListenerRequest, string> getResponse)
			: base(prefix, getResponse)
		{
		}

		override protected void Serve()
		{
			while (true)
			{
				var context = listener.GetContext();
				new Thread(() => ProcessClient(context)).Start();
			}
		}
	}
}
