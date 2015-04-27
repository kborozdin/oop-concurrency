using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConcurrentTextEditor;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TestConcurrentTextEditor
{
	[TestClass]
	public class TestUnsafeStorage
	{
		[TestMethod]
		[ExpectedException(typeof(AggregateException))]
		public void TestUnsafeMustFail()
		{
			var lines = new string[10000];
			for (int i = 0; i < lines.Length; i++)
				lines[i] = "a";
			var storage = new UnsafeStorage(lines);

			var tasks = new List<Task>();
			for (int i = 0; i < lines.Length; i++)
			{
				var task = new Task(() => storage.ReplaceFirst("a", ""));
				task.Start();
				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray());
		}
	}
}
