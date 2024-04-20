using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B0 RID: 432
	[StructLayout(LayoutKind.Sequential)]
	internal class CCallbackBaseVTable
	{
		// Token: 0x04000A3A RID: 2618
		private const CallingConvention cc = CallingConvention.StdCall;

		// Token: 0x04000A3B RID: 2619
		[NonSerialized]
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CCallbackBaseVTable.RunCRDel m_RunCallResult;

		// Token: 0x04000A3C RID: 2620
		[NonSerialized]
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CCallbackBaseVTable.RunCBDel m_RunCallback;

		// Token: 0x04000A3D RID: 2621
		[NonSerialized]
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CCallbackBaseVTable.GetCallbackSizeBytesDel m_GetCallbackSizeBytes;

		// Token: 0x020002BA RID: 698
		// (Invoke) Token: 0x06001291 RID: 4753
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void RunCBDel(IntPtr pvParam);

		// Token: 0x020002BB RID: 699
		// (Invoke) Token: 0x06001295 RID: 4757
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void RunCRDel(IntPtr pvParam, [MarshalAs(UnmanagedType.I1)] bool bIOFailure, ulong hSteamAPICall);

		// Token: 0x020002BC RID: 700
		// (Invoke) Token: 0x06001299 RID: 4761
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate int GetCallbackSizeBytesDel();
	}
}
