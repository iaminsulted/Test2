using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace MS.Internal.Progressivity
{
	// Token: 0x02000153 RID: 339
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e7b92912-c7ca-4629-8f39-0f537cfab57e")]
	[ComImport]
	internal interface IByteRangeDownloaderService
	{
		// Token: 0x06000B3B RID: 2875
		void InitializeByteRangeDownloader([MarshalAs(UnmanagedType.LPWStr)] string url, [MarshalAs(UnmanagedType.LPWStr)] string tempFile, SafeWaitHandle eventHandle);

		// Token: 0x06000B3C RID: 2876
		void RequestDownloadByteRanges([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] byteRanges, int size);

		// Token: 0x06000B3D RID: 2877
		void GetDownloadedByteRanges([MarshalAs(UnmanagedType.LPArray)] out int[] byteRanges, [MarshalAs(UnmanagedType.I4)] out int size);

		// Token: 0x06000B3E RID: 2878
		void ReleaseByteRangeDownloader();
	}
}
