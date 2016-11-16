using System;
using System.Linq;
using System.Numerics;

namespace CodeJam.Dogfooding.PerfTests
{
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
				if (new Vector<Byte>(a, i) != new Vector<byte>(b, i))
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
	}
}