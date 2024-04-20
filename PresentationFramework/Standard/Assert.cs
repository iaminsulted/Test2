using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Standard
{
	// Token: 0x02000006 RID: 6
	internal static class Assert
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000F6B1D File Offset: 0x000F5B1D
		private static void _Break()
		{
			Debugger.Break();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000F6B24 File Offset: 0x000F5B24
		[Conditional("DEBUG")]
		public static void Evaluate(Assert.EvaluateFunction argument)
		{
			argument();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Obsolete("Use Assert.AreEqual instead of Assert.Equals", false)]
		[Conditional("DEBUG")]
		public static void Equals<T>(T expected, T actual)
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000F6B30 File Offset: 0x000F5B30
		[Conditional("DEBUG")]
		public static void AreEqual<T>(T expected, T actual)
		{
			if (expected == null)
			{
				if (actual != null && !actual.Equals(expected))
				{
					Assert._Break();
					return;
				}
			}
			else if (!expected.Equals(actual))
			{
				Assert._Break();
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000F6B84 File Offset: 0x000F5B84
		[Conditional("DEBUG")]
		public static void AreNotEqual<T>(T notExpected, T actual)
		{
			if (notExpected == null)
			{
				if (actual == null || actual.Equals(notExpected))
				{
					Assert._Break();
					return;
				}
			}
			else if (notExpected.Equals(actual))
			{
				Assert._Break();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000F6BD6 File Offset: 0x000F5BD6
		[Conditional("DEBUG")]
		public static void Implies(bool condition, bool result)
		{
			if (condition && !result)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000F6BE3 File Offset: 0x000F5BE3
		[Conditional("DEBUG")]
		public static void Implies(bool condition, Assert.ImplicationFunction result)
		{
			if (condition && !result())
			{
				Assert._Break();
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		public static void IsNeitherNullNorEmpty(string value)
		{
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000F6BF5 File Offset: 0x000F5BF5
		[Conditional("DEBUG")]
		public static void IsNeitherNullNorWhitespace(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Assert._Break();
			}
			if (value.Trim().Length == 0)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000F6C16 File Offset: 0x000F5C16
		[Conditional("DEBUG")]
		public static void IsNotNull<T>(T value) where T : class
		{
			if (value == null)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000F6C28 File Offset: 0x000F5C28
		[Conditional("DEBUG")]
		public static void IsDefault<T>(T value) where T : struct
		{
			value.Equals(default(T));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000F6C28 File Offset: 0x000F5C28
		[Conditional("DEBUG")]
		public static void IsNotDefault<T>(T value) where T : struct
		{
			value.Equals(default(T));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000F6C51 File Offset: 0x000F5C51
		[Conditional("DEBUG")]
		public static void IsFalse(bool condition)
		{
			if (condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000F6C51 File Offset: 0x000F5C51
		[Conditional("DEBUG")]
		public static void IsFalse(bool condition, string message)
		{
			if (condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000F6C5B File Offset: 0x000F5C5B
		[Conditional("DEBUG")]
		public static void IsTrue(bool condition)
		{
			if (!condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000F6C5B File Offset: 0x000F5C5B
		[Conditional("DEBUG")]
		public static void IsTrue(bool condition, string message)
		{
			if (!condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000F6C65 File Offset: 0x000F5C65
		[Conditional("DEBUG")]
		public static void Fail()
		{
			Assert._Break();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000F6C65 File Offset: 0x000F5C65
		[Conditional("DEBUG")]
		public static void Fail(string message)
		{
			Assert._Break();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000F6C6C File Offset: 0x000F5C6C
		[Conditional("DEBUG")]
		public static void IsNull<T>(T item) where T : class
		{
			if (item != null)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000F6C7B File Offset: 0x000F5C7B
		[Conditional("DEBUG")]
		public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive)
		{
			if (value < lowerBoundInclusive || value > upperBoundInclusive)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000F6C8A File Offset: 0x000F5C8A
		[Conditional("DEBUG")]
		public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive)
		{
			if (value < lowerBoundInclusive || value >= upperBoundExclusive)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000F6C99 File Offset: 0x000F5C99
		[Conditional("DEBUG")]
		public static void IsApartmentState(ApartmentState expectedState)
		{
			if (Thread.CurrentThread.GetApartmentState() != expectedState)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000F6CAD File Offset: 0x000F5CAD
		[Conditional("DEBUG")]
		public static void NullableIsNotNull<T>(T? value) where T : struct
		{
			if (value == null)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000F6CBD File Offset: 0x000F5CBD
		[Conditional("DEBUG")]
		public static void NullableIsNull<T>(T? value) where T : struct
		{
			if (value != null)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000F6CCD File Offset: 0x000F5CCD
		[Conditional("DEBUG")]
		public static void IsNotOnMainThread()
		{
			if (Application.Current.Dispatcher.CheckAccess())
			{
				Assert._Break();
			}
		}

		// Token: 0x0200087A RID: 2170
		// (Invoke) Token: 0x06007FFE RID: 32766
		public delegate void EvaluateFunction();

		// Token: 0x0200087B RID: 2171
		// (Invoke) Token: 0x06008002 RID: 32770
		public delegate bool ImplicationFunction();
	}
}
