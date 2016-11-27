﻿using System;

using BenchmarkDotNet.Characteristics;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.Results;

// ReSharper disable once CheckNamespace

namespace BenchmarkDotNet.Toolchains.InProcess
{
	/// <summary>
	/// Implementation of <see cref="IBuilder"/> for in-process benchmarks.
	/// </summary>
	public class InProcessBuilderOld : IBuilder
	{
		/// <summary>Builds the benchmark.</summary>
		/// <param name="generateResult">Generation result.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="benchmark">The benchmark.</param>
		/// <param name="resolver">The resolver.</param>
		/// <returns>Build result.</returns>
		public BuildResult Build(GenerateResult generateResult, ILogger logger, Benchmark benchmark, IResolver resolver) =>
			BuildResult.Success(generateResult);
	}
}