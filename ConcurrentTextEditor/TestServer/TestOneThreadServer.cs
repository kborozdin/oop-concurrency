using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace TestServer
{
	[TestClass]
	public class TestOneThreadServer : AbstractTestServer
	{
		[TestInitialize]
		override public void Initialize()
		{
			createServer = (p, r) => new OneThreadServer(p, r);
		}
	}
}
