using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	public class OneThreadServer : AbstractServer
	{
		public OneThreadServer(string prefix, Func<HttpListenerRequest, string> getResponse)
			: base(prefix, getResponse)
		{
		}

		override protected void AcceptClient(HttpListenerContext context)
		{
			ProcessClient(context);
		}
	}
}
