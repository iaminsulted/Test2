using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C4 RID: 452
	// (Invoke) Token: 0x06000DA6 RID: 3494
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void SteamAPI_CheckCallbackRegistered_t(int iCallbackNum);
}
