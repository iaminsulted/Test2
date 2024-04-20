using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x02000291 RID: 657
	[Guid("1f681651-1024-4798-af36-119bbe5e5665")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface INativeProgressPage
	{
		// Token: 0x060018E1 RID: 6369
		[PreserveSig]
		HRESULT Show();

		// Token: 0x060018E2 RID: 6370
		[PreserveSig]
		HRESULT Hide();

		// Token: 0x060018E3 RID: 6371
		[PreserveSig]
		HRESULT ShowProgressMessage(string message);

		// Token: 0x060018E4 RID: 6372
		[PreserveSig]
		HRESULT SetApplicationName(string appName);

		// Token: 0x060018E5 RID: 6373
		[PreserveSig]
		HRESULT SetPublisherName(string publisherName);

		// Token: 0x060018E6 RID: 6374
		[PreserveSig]
		HRESULT OnDownloadProgress(ulong bytesDownloaded, ulong bytesTotal);
	}
}
