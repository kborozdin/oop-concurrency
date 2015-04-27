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
		Random random = new Random();

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

		private string GenerateRandomWord(int length, int alphabetSize)
		{
			StringBuilder word = new StringBuilder();
			for (int i = 0; i < length; i++)
				word.Append((char)('a' + random.Next(alphabetSize)));
			return word.ToString();
		}

		[TestMethod]
		public void TestOneThreadRun()
		{
			var lines = new string[(int)1e5];
			for (int i = 0; i < lines.Length; i++)
				lines[i] = GenerateRandomWord(10, 3);
			var storage = new SafeStorage(lines);

			for (int i = 0; i < (int)1e6; i++)
				storage.ReplaceFirst(GenerateRandomWord(3, 3), GenerateRandomWord(3, 3));
		}

		[TestMethod]
		public void TestMultiThreadRun()
		{
			var lines = new string[(int)1e5];
			for (int i = 0; i < lines.Length; i++)
				lines[i] = GenerateRandomWord(10, 3);
			var storage = new SafeStorage(lines);

			var tasks = new List<Task>();
			for (int i = 0; i < (int)1e6; i++)
			{
				var task = new Task(() => storage.ReplaceFirst(GenerateRandomWord(3, 3), GenerateRandomWord(3, 3)));
				task.Start();
				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray());
		}
	}
}
