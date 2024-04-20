using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A5 RID: 677
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("973510DB-7D7F-452B-8975-74A85828D354")]
	[ComImport]
	internal interface IFileDialogEvents
	{
		// Token: 0x0600195B RID: 6491
		[PreserveSig]
		HRESULT OnFileOk(IFileDialog pfd);

		// Token: 0x0600195C RID: 6492
		[PreserveSig]
		HRESULT OnFolderChanging(IFileDialog pfd, IShellItem psiFolder);

		// Token: 0x0600195D RID: 6493
		[PreserveSig]
		HRESULT OnFolderChange(IFileDialog pfd);

		// Token: 0x0600195E RID: 6494
		[PreserveSig]
		HRESULT OnSelectionChange(IFileDialog pfd);

		// Token: 0x0600195F RID: 6495
		[PreserveSig]
		HRESULT OnShareViolation(IFileDialog pfd, IShellItem psi, out FDESVR pResponse);

		// Token: 0x06001960 RID: 6496
		[PreserveSig]
		HRESULT OnTypeChange(IFileDialog pfd);

		// Token: 0x06001961 RID: 6497
		[PreserveSig]
		HRESULT OnOverwrite(IFileDialog pfd, IShellItem psi, out FDEOR pResponse);
	}
}
