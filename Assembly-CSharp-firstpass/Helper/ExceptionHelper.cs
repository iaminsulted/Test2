using System;
using System.Runtime.InteropServices;

namespace Helper
{
	// Token: 0x02000032 RID: 50
	public static class ExceptionHelper
	{
		// Token: 0x060001E1 RID: 481 RVA: 0x00016624 File Offset: 0x00014824
		public static void CheckLastError()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == -2147483638 || lastWin32Error == -2147467259)
			{
				return;
			}
			if (lastWin32Error < 0)
			{
				Exception exceptionForHR = Marshal.GetExceptionForHR(lastWin32Error);
				string message = string.Format("This API has returned an exception from an HRESULT: 0x{0:X}", lastWin32Error);
				if (lastWin32Error <= -2147467261)
				{
					if (lastWin32Error == -2147467263)
					{
						throw new NotImplementedException(message, exceptionForHR);
					}
					if (lastWin32Error == -2147467261)
					{
						throw new ArgumentNullException(message, exceptionForHR);
					}
				}
				else
				{
					if (lastWin32Error == -2147024882)
					{
						throw new OutOfMemoryException(message, exceptionForHR);
					}
					if (lastWin32Error == -2147024809)
					{
						throw new ArgumentException(message, exceptionForHR);
					}
				}
				throw new InvalidOperationException(message, exceptionForHR);
			}
		}

		// Token: 0x040001EA RID: 490
		private const int E_NOTIMPL = -2147467263;

		// Token: 0x040001EB RID: 491
		private const int E_OUTOFMEMORY = -2147024882;

		// Token: 0x040001EC RID: 492
		private const int E_INVALIDARG = -2147024809;

		// Token: 0x040001ED RID: 493
		private const int E_POINTER = -2147467261;

		// Token: 0x040001EE RID: 494
		private const int E_PENDING = -2147483638;

		// Token: 0x040001EF RID: 495
		private const int E_FAIL = -2147467259;
	}
}
