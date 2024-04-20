using System;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000A3 RID: 163
	internal static class Context
	{
		// Token: 0x0600025C RID: 604
		[DllImport("api-ms-win-core-com-l1-1-0.dll")]
		private static extern int CoGetObjectContext(ref Guid riid, out IntPtr ppv);

		// Token: 0x0600025D RID: 605 RVA: 0x000FA018 File Offset: 0x000F9018
		public static IntPtr GetContextCallback()
		{
			Guid guid = typeof(IContextCallback).GUID;
			IntPtr result;
			Marshal.ThrowExceptionForHR(Context.CoGetObjectContext(ref guid, out result));
			return result;
		}
	}
}
