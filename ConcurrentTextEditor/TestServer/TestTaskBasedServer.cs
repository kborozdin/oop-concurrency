using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace TestServer
{
	[TestClass]
	public class TestTaskBasedServer : AbstractTestServer
	{
		[TestInitialize]
		override public void Initialize()
		{
			createServer = (p, r) => new TaskBasedServer(p, r);
		}
	}
}
