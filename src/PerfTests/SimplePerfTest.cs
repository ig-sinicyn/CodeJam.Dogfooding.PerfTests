using System;
using System.Threading;

using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess;

using CodeJam.PerfTests;
using CodeJam.PerfTests.Configs;
using CodeJam.PerfTests.Configs.Factories;

using NUnit.Framework;

namespace CodeJam.Dogfooding.PerfTests
{
	public class UseOldToolchain : ICompetitionModifier
	{
		public void Modify(ManualCompetitionConfig competitionConfig) =>
			competitionConfig.ApplyModifier(
				new Job("OldToolchain")
				{
					Infrastructure = { Toolchain = InProcessToolchainOld.Instance }
				});
	}

	public class SimplePerfTestBurst : SimplePerfTestBurstBase
	{
		[Test]
		public void Run() => Competition.Run(this);
	}

	[CompetitionModifier(typeof(UseOldToolchain))]
	public class SimplePerfTestOldBurst : SimplePerfTestBurstBase
	{
		[Test]
		public void Run() => Competition.Run(this);
	}

	public class SimplePerfTest : SimplePerfTestBase
	{
		[Test]
		public void Run() => Competition.Run(this);
	}

	[CompetitionModifier(typeof(UseOldToolchain))]
	public class SimplePerfTestOld : SimplePerfTestBase
	{
		[Test]
		public void Run() => Competition.Run(this);
	}

	#region Base
	[CompetitionFeatures(ContinuousIntegrationMode = true)]
	public class SimplePerfTestBase
	{
		private const int Count = 200;

		[CompetitionBaseline]
		public void Baseline() => Thread.SpinWait(Count);

		[CompetitionBenchmark(2.88, 3.12)]
		public void SlowerX3() => Thread.SpinWait(3 * Count);

		[CompetitionBenchmark(2.88, 3.12)]
		public void SlowerX32() => Thread.SpinWait(3 * Count);

		[CompetitionBenchmark(4.80, 5.20)]
		public void SlowerX5() => Thread.SpinWait(5 * Count);

		[CompetitionBenchmark(4.80, 5.20)]
		public void SlowerX52() => Thread.SpinWait(5 * Count);

		[CompetitionBenchmark(6.72, 7.28)]
		public void SlowerX7() => Thread.SpinWait(7 * Count);

		[CompetitionBenchmark(6.72, 7.28)]
		public void SlowerX72() => Thread.SpinWait(7 * Count);
	}

	[CompetitionBurstMode]
	[CompetitionFeatures(ContinuousIntegrationMode = true)]
	public class SimplePerfTestBurstBase
	{
		private const int Count = 10000;

		[CompetitionBaseline]
		public void Baseline() => Thread.SpinWait(Count);

		[CompetitionBenchmark(2.88, 3.12)]
		public void SlowerX3() => Thread.SpinWait(3 * Count);

		[CompetitionBenchmark(2.88, 3.12)]
		public void SlowerX32() => Thread.SpinWait(3 * Count);

		[CompetitionBenchmark(4.80, 5.20)]
		public void SlowerX5() => Thread.SpinWait(5 * Count);

		[CompetitionBenchmark(4.80, 5.20)]
		public void SlowerX52() => Thread.SpinWait(5 * Count);

		[CompetitionBenchmark(6.72, 7.28)]
		public void SlowerX7() => Thread.SpinWait(7 * Count);

		[CompetitionBenchmark(6.72, 7.28)]
		public void SlowerX72() => Thread.SpinWait(7 * Count);
	} 
	#endregion
}