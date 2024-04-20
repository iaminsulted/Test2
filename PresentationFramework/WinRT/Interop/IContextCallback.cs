using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020000BF RID: 191
	[Guid("000001da-0000-0000-C000-000000000046")]
	internal interface IContextCallback
	{
		// Token: 0x06000321 RID: 801
		unsafe void ContextCallback(PFNCONTEXTCALL pfnCallback, ComCallData* pParam, Guid riid, int iMethod);
	}
}
