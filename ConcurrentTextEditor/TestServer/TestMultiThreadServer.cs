using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace TestServer
{
	[TestClass]
	public class TestMultiThreadServer : AbstractTestServer
	{
		[TestInitialize]
		override public void Initialize()
		{
			createServer = (p, r) => new MultiThreadServer(p, r);
		}
	}
}
