using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

using NUnit.Framework;

namespace CodeJam.Dogfooding.PerfTests
{
	public static class ByteArrayEqualityTest
	{
		private static readonly byte[] _bytes = Encoding.Default.GetBytes("Hello, world!");
		private static readonly byte[] _sameBytes = Encoding.Default.GetBytes("Hello, world!");
		private static readonly byte[] _otherBytes = Encoding.Default.GetBytes("Emm?");

		private static void TestCore(Func<byte[], byte[], bool> comparer)
		{
			var algName = comparer.Method.Name;

			Assert.True(comparer(_bytes,_bytes), algName + nameof(_bytes));
			Assert.True(comparer(_sameBytes, _sameBytes), algName + nameof(_sameBytes));
			Assert.True(comparer(_otherBytes, _otherBytes), algName + nameof(_otherBytes));
			Assert.True(comparer(_bytes, _sameBytes), algName + nameof(_bytes) + nameof(_sameBytes));
			Assert.False(comparer(_bytes, _otherBytes), algName + nameof(_bytes) + nameof(_otherBytes));
		}

		[Test]
		public static void TestEqualsForLoop() => TestCore(ByteArrayEquality.EqualsForLoop);
		[Test]
		public static void TestEqualsLinq() => TestCore(ByteArrayEquality.EqualsLinq);
		[Test]
		public static void TestEqualsCodeJam() => TestCore(ByteArrayEquality.EqualsCodeJam);
		[Test]
		public static void TestEqualsVectors() => TestCore(ByteArrayEquality.EqualsVectors);
		[Test]
		public static void TestEqualsUnsafe() => TestCore(ByteArrayEquality.EqualsUnsafe);
		[Test]
		public static void TestEqualsInterop() => TestCore(ByteArrayEquality.EqualsInterop);
	}

	public static class ByteArrayEquality
	{
		public static bool EqualsForLoop(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;

			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
					return false;
			}
			return true;
		}

		public static bool EqualsLinq(byte[] a, byte[] b)
		{
			return a.SequenceEqual(b);
		}

		public static bool EqualsCodeJam(byte[] a, byte[] b)
		{
			return CodeJam.Collections.ArrayExtensions.EqualsTo(a, b);
		}
		
		public static bool EqualsVectors(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;

			int i;
			var max = a.Length - a.Length % Vector<byte>.Count;
			for (i = 0; i < max; i += Vector<byte>.Count)
			{
				if (new Vector<byte>(a, i) != new Vector<byte>(b, i))
					return false;
			}
			if (i < a.Length)
			{
				for (; i < a.Length; i++)
				{
					if (a[i] != b[i])
						return false;
				}
			}
			return true;
		}

		// Copyright (c) 2008-2013 Hafthor Stefansson
		// Distributed under the MIT/X11 software license
		// Ref: http://www.opensource.org/licenses/mit-license.php.
		// THANKSTO: http://stackoverflow.com/a/8808245/318263
		public static unsafe bool EqualsUnsafe(byte[] a1, byte[] a2)
		{
			if (a1 == null || a2 == null || a1.Length != a2.Length)
				return false;
			fixed (byte* p1 = a1, p2 = a2)
			{
				byte* x1 = p1, x2 = p2;
				int l = a1.Length;
				for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
					if (*((long*)x1) != *((long*)x2))
						return false;
				if ((l & 4) != 0)
				{ if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
				if ((l & 2) != 0)
				{ if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
				if ((l & 1) != 0)
					if (*((byte*)x1) != *((byte*)x2))
						return false;
				return true;
			}
		}

		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern int memcmp(byte[] b1, byte[] b2, long count);

		// THANKSTO: http://stackoverflow.com/a/1445405/318263
		// P/Invoke... booo...
		public static bool EqualsInterop(byte[] b1, byte[] b2)
		{
			// Validate buffers are the same length.
			// This also ensures that the count does not exceed the length of either buffer.  
			return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
		}
	}
}