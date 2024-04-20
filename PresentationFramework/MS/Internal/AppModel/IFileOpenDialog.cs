using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A8 RID: 680
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
	[ComImport]
	internal interface IFileOpenDialog : IFileDialog, IModalWindow
	{
		// Token: 0x0600197B RID: 6523
		[PreserveSig]
		HRESULT Show(IntPtr parent);

		// Token: 0x0600197C RID: 6524
		void SetFileTypes(uint cFileTypes, [In] COMDLG_FILTERSPEC[] rgFilterSpec);

		// Token: 0x0600197D RID: 6525
		void SetFileTypeIndex(uint iFileType);

		// Token: 0x0600197E RID: 6526
		uint GetFileTypeIndex();

		// Token: 0x0600197F RID: 6527
		uint Advise(IFileDialogEvents pfde);

		// Token: 0x06001980 RID: 6528
		void Unadvise(uint dwCookie);

		// Token: 0x06001981 RID: 6529
		void SetOptions(FOS fos);

		// Token: 0x06001982 RID: 6530
		FOS GetOptions();

		// Token: 0x06001983 RID: 6531
		void SetDefaultFolder(IShellItem psi);

		// Token: 0x06001984 RID: 6532
		void SetFolder(IShellItem psi);

		// Token: 0x06001985 RID: 6533
		IShellItem GetFolder();

		// Token: 0x06001986 RID: 6534
		IShellItem GetCurrentSelection();

		// Token: 0x06001987 RID: 6535
		void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x06001988 RID: 6536
		[return: MarshalAs(UnmanagedType.LPWStr)]
		void GetFileName();

		// Token: 0x06001989 RID: 6537
		void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		// Token: 0x0600198A RID: 6538
		void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);

		// Token: 0x0600198B RID: 6539
		void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		// Token: 0x0600198C RID: 6540
		IShellItem GetResult();

		// Token: 0x0600198D RID: 6541
		void AddPlace(IShellItem psi, FDAP fdcp);

		// Token: 0x0600198E RID: 6542
		void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		// Token: 0x0600198F RID: 6543
		void Close([MarshalAs(UnmanagedType.Error)] int hr);

		// Token: 0x06001990 RID: 6544
		void SetClientGuid([In] ref Guid guid);

		// Token: 0x06001991 RID: 6545
		void ClearClientData();

		// Token: 0x06001992 RID: 6546
		void SetFilter([MarshalAs(UnmanagedType.Interface)] object pFilter);

		// Token: 0x06001993 RID: 6547
		IShellItemArray GetResults();

		// Token: 0x06001994 RID: 6548
		IShellItemArray GetSelectedItems();
	}
}
