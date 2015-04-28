using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConcurrentTextEditor;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TestConcurrentTextEditor
{
	[TestClass]
	public class TestSafeStorage
	{
		private Random random = new Random(1234);

		[TestMethod]
		public void TestSimpleActions()
		{
			var lines = new[] { "abc", "aba" };
			var storage = new SafeStorage(lines);

			Assert.AreEqual(new Tuple<int, string>(0, "az"), storage.ReplaceFirst("bc", "z"));
			Assert.AreEqual(new Tuple<int, string>(0, "zz"), storage.ReplaceFirst("a", "z"));
			Assert.AreEqual(new Tuple<int, string>(1, "ba"), storage.ReplaceFirst("a", ""));
			Assert.AreEqual(new Tuple<int, string>(0, ""), storage.ReplaceFirst("zz", ""));
		}

		[TestMethod]
		public void TestOneThreadRun()
		{
			var lines = new string[(int)1e5];
			for (int i = 0; i < lines.Length; i++)
				lines[i] = random.GenerateRandomWord(10, 3);
			var storage = new SafeStorage(lines);

			for (int i = 0; i < (int)2e6; i++)
				storage.ReplaceFirst(random.GenerateRandomWord(3, 3), random.GenerateRandomWord(3, 3));
		}

		[TestMethod]
		public void TestMultiThreadRun()
		{
			var lines = new string[(int)2e5];
			for (int i = 0; i < lines.Length; i++)
				lines[i] = random.GenerateRandomWord(10, 3);
			var storage = new SafeStorage(lines);

			var tasks = new List<Task>();
			for (int i = 0; i < (int)2e6; i++)
			{
				var task = new Task(() => storage.ReplaceFirst(
					random.GenerateRandomWord(3, 3), random.GenerateRandomWord(3, 3)));
				task.Start();
				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray());
		}
	}
}
