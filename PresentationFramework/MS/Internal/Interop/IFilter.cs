using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Interop
{
	// Token: 0x02000178 RID: 376
	[Guid("89BCB740-6119-101A-BCB7-00DD010655AF")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IFilter
	{
		// Token: 0x06000C4C RID: 3148
		IFILTER_FLAGS Init([In] IFILTER_INIT grfFlags, [In] uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] FULLPROPSPEC[] aAttributes);

		// Token: 0x06000C4D RID: 3149
		STAT_CHUNK GetChunk();

		// Token: 0x06000C4E RID: 3150
		void GetText([In] [Out] ref uint pcwcBuffer, [In] IntPtr pBuffer);

		// Token: 0x06000C4F RID: 3151
		IntPtr GetValue();

		// Token: 0x06000C50 RID: 3152
		IntPtr BindRegion([In] FILTERREGION origPos, [In] ref Guid riid);
	}
}
