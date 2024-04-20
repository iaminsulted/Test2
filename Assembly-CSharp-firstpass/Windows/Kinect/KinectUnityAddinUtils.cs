using System;
using System.Runtime.InteropServices;

namespace Windows.Kinect
{
	// Token: 0x02000079 RID: 121
	public sealed class KinectUnityAddinUtils
	{
		// Token: 0x060005D0 RID: 1488
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void KinectUnityAddin_FreeMemory(IntPtr pToDealloc);

		// Token: 0x060005D1 RID: 1489 RVA: 0x00021A19 File Offset: 0x0001FC19
		public static void FreeMemory(IntPtr pToDealloc)
		{
			KinectUnityAddinUtils.KinectUnityAddin_FreeMemory(pToDealloc);
		}
	}
}
