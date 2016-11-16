using System;
using System.Linq;
using CodeJam.PerfTests;

namespace CodeJam.Dogfooding.PerfTests
{
	public class ByteArrayComparisonTest
	{
		private static readonly byte[] _arrayA;
		private static readonly byte[] _arrayB;

		private const int RepeatCount = 1000;

		static ByteArrayComparisonTest()
		{
			var rnd = new Random(0); // Constant rnd seed to get repeatable results
			_arrayA = Enumerable.Range(0, 19).Select(i => (byte)rnd.Next()).ToArray();
			_arrayB = _arrayA.ToArray();
		}

		[CompetitionBaseline]
		public bool EqualsForLoop()
		{
			var a = _arrayA;
			var b = _arrayB;
			bool result = false;
			//for (int i = 0; i < RepeatCount; i++)
				result = ByteArrayEquality.EqualsForLoop(a, b);

			return result;
		}

		[CompetitionBenchmark(9.65, 197.30)]
		public bool EqualsLinq()
		{
			var a = _arrayA;
			var b = _arrayB;
			bool result = false;
			//for (int i = 0; i < RepeatCount; i++)
				result = ByteArrayEquality.EqualsLinq(a, b);

			return result;
		}

		[CompetitionBenchmark(0.35, 1.25)]
		public bool EqualsCodeJam()
		{
			var a = _arrayA;
			var b = _arrayB;
			bool result = false;
			//for (int i = 0; i < RepeatCount; i++)
				result = ByteArrayEquality.EqualsCodeJam(a, b);

			return result;
		}

		[CompetitionBenchmark(0.27, 1.15)]
		public bool EqualsVectors()
		{
			var a = _arrayA;
			var b = _arrayB;
			bool result = false;
			//for (int i = 0; i < RepeatCount; i++)
			result = ByteArrayEquality.EqualsVectors(a, b);

			return result;
		}

		[CompetitionBenchmark(0.26, 0.71)]
		public bool EqualsUnsafe()
		{
			var a = _arrayA;
			var b = _arrayB;
			bool result = false;
			//for (int i = 0; i < RepeatCount; i++)
			result = ByteArrayEquality.EqualsUnsafe(a, b);

			return result;
		}
	}
}