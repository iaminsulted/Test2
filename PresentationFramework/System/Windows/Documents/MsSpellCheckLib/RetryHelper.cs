using System;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x020006FE RID: 1790
	internal static class RetryHelper
	{
		// Token: 0x06005DC3 RID: 24003 RVA: 0x0028DBFC File Offset: 0x0028CBFC
		internal static bool TryCallAction(Action action, RetryHelper.RetryActionPreamble preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (action != null)
					{
						action();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble(out action);
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x06005DC4 RID: 24004 RVA: 0x0028DC78 File Offset: 0x0028CC78
		internal static bool TryCallAction(Action action, RetryHelper.RetryPreamble preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (action != null)
					{
						action();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble();
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x06005DC5 RID: 24005 RVA: 0x0028DCF0 File Offset: 0x0028CCF0
		internal static bool TryExecuteFunction<TResult>(Func<TResult> func, out TResult result, RetryHelper.RetryFunctionPreamble<TResult> preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			result = default(TResult);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (func != null)
					{
						result = func();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble(out func);
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x06005DC6 RID: 24006 RVA: 0x0028DD78 File Offset: 0x0028CD78
		internal static bool TryExecuteFunction<TResult>(Func<TResult> func, out TResult result, RetryHelper.RetryPreamble preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			result = default(TResult);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (func != null)
					{
						result = func();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble();
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x06005DC7 RID: 24007 RVA: 0x0028DE00 File Offset: 0x0028CE00
		private static bool MatchException(Exception exception, List<Type> exceptions)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			if (exceptions == null)
			{
				throw new ArgumentNullException("exceptions");
			}
			Type exceptionType = exception.GetType();
			return exceptions.Find((Type e) => e.IsAssignableFrom(exceptionType)) != null;
		}

		// Token: 0x06005DC8 RID: 24008 RVA: 0x0028DE53 File Offset: 0x0028CE53
		private static void ValidateExceptionTypeList(List<Type> exceptions)
		{
			if (exceptions == null)
			{
				throw new ArgumentNullException("exceptions");
			}
			Invariant.Assert(exceptions.TrueForAll((Type t) => typeof(Exception).IsAssignableFrom(t)));
		}

		// Token: 0x02000BB5 RID: 2997
		// (Invoke) Token: 0x06008F23 RID: 36643
		internal delegate bool RetryPreamble();

		// Token: 0x02000BB6 RID: 2998
		// (Invoke) Token: 0x06008F27 RID: 36647
		internal delegate bool RetryActionPreamble(out Action action);

		// Token: 0x02000BB7 RID: 2999
		// (Invoke) Token: 0x06008F2B RID: 36651
		internal delegate bool RetryFunctionPreamble<TResult>(out Func<TResult> func);
	}
}
