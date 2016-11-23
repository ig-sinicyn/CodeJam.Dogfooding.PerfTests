using System;
using System.IO;
using System.Linq;
using System.Net.Configuration;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using CodeJam.PerfTests;
using CodeJam.PerfTests.Configs;
using CodeJam.PerfTests.Configs.Factories;

using NUnit.Framework;

namespace CodeJam.Dogfooding.PerfTests
{
	class M : ICompetitionModifier
	{
		#region Implementation of ICompetitionModifier
		public void Modify(ManualCompetitionConfig competitionConfig) =>
			competitionConfig.ApplyToJobs(
				new Job()
				{
					Run =
					{
						WarmupCount = 200,
						TargetCount = 500,
						InvocationCount = 256
					}
				}, true);
		#endregion
	}

	//[CompetitionAnnotateSources]
	[CompetitionModifier(typeof(M))]
	public class ByteArrayEqualityPerfTest
	{
		private byte[] _arrayA;
		private byte[] _arrayB;

		[Test]
		public void RunByteArrayEqualityPerfTest() => Competition.Run(this);

		[Setup]
		public void Setup()
		{
			var rndSeed = new Random().Next();

			Console.WriteLine($"Rnd seed: {rndSeed}.");
			var rnd = new Random(rndSeed);
			// Constant rnd seed to get repeatable results
			_arrayA = Enumerable.Range(0, 128).Select(i => (byte)rnd.Next()).ToArray();
			_arrayB = _arrayA.ToArray();
		}

		[CompetitionBaseline]
		public bool EqualsForLoop() => ByteArrayEquality.EqualsForLoop(_arrayA, _arrayB);

		[CompetitionBenchmark(15.48, 16.96)]
		public bool EqualsLinq() => ByteArrayEquality.EqualsLinq(_arrayA, _arrayB);

		[CompetitionBenchmark(0.08, 0.11)]
		public bool EqualsCodeJam() => ByteArrayEquality.EqualsCodeJam(_arrayA, _arrayB);

		[CompetitionBenchmark(0.14, 0.21)]
		public bool EqualsVectors() => ByteArrayEquality.EqualsVectors(_arrayA, _arrayB);

		[CompetitionBenchmark(0.19, 0.24)]
		public bool EqualsUnsafe() => ByteArrayEquality.EqualsUnsafe(_arrayA, _arrayB);

		[CompetitionBenchmark(0.16, 0.19)]
		public bool EqualsInterop() => ByteArrayEquality.EqualsInterop(_arrayA, _arrayB);
	}
}