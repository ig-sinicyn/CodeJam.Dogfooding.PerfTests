using System;
using System.Reflection;
using CodeJam.PerfTests.Running.Console;

namespace CodeJam.Dogfooding
{
	public static class Program
	{
		static void Main(string[] args)
		{
			Console.WindowWidth = 150;
			Console.WindowHeight = 50;
			ConsoleCompetition.Run(Assembly.GetExecutingAssembly());
			//BenchmarkRunner.Run<ByteArrayComparisonTest>();
		}
	}
}
