using System;

namespace WinRT
{
	// Token: 0x020000A5 RID: 165
	internal static class ExceptionExtensions
	{
		// Token: 0x06000269 RID: 617 RVA: 0x000FA594 File Offset: 0x000F9594
		public static void SetHResult(this Exception ex, int value)
		{
			ex.GetType().GetProperty("HResult").SetValue(ex, value);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000FA5B4 File Offset: 0x000F95B4
		internal static Exception GetExceptionForHR(this Exception innerException, int hresult, string messageResource)
		{
			Exception ex;
			if (innerException != null)
			{
				ex = new Exception(innerException.Message ?? messageResource, innerException);
			}
			else
			{
				ex = new Exception(messageResource);
			}
			ex.SetHResult(hresult);
			return ex;
		}
	}
}
