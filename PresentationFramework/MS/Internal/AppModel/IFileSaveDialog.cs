using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A9 RID: 681
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("84bccd23-5fde-4cdb-aea4-af64b83d78ab")]
	[ComImport]
	internal interface IFileSaveDialog : IFileDialog, IModalWindow
	{
		// Token: 0x06001995 RID: 6549
		[PreserveSig]
		HRESULT Show(IntPtr parent);

		// Token: 0x06001996 RID: 6550
		void SetFileTypes(uint cFileTypes, [In] COMDLG_FILTERSPEC[] rgFilterSpec);

		// Token: 0x06001997 RID: 6551
		void SetFileTypeIndex(uint iFileType);

		// Token: 0x06001998 RID: 6552
		uint GetFileTypeIndex();

		// Token: 0x06001999 RID: 6553
		uint Advise(IFileDialogEvents pfde);

		// Token: 0x0600199A RID: 6554
		void Unadvise(uint dwCookie);

		// Token: 0x0600199B RID: 6555
		void SetOptions(FOS fos);

		// Token: 0x0600199C RID: 6556
		FOS GetOptions();

		// Token: 0x0600199D RID: 6557
		void SetDefaultFolder(IShellItem psi);

		// Token: 0x0600199E RID: 6558
		void SetFolder(IShellItem psi);

		// Token: 0x0600199F RID: 6559
		IShellItem GetFolder();

		// Token: 0x060019A0 RID: 6560
		IShellItem GetCurrentSelection();

		// Token: 0x060019A1 RID: 6561
		void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x060019A2 RID: 6562
		[return: MarshalAs(UnmanagedType.LPWStr)]
		void GetFileName();

		// Token: 0x060019A3 RID: 6563
		void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		// Token: 0x060019A4 RID: 6564
		void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);

		// Token: 0x060019A5 RID: 6565
		void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		// Token: 0x060019A6 RID: 6566
		IShellItem GetResult();

		// Token: 0x060019A7 RID: 6567
		void AddPlace(IShellItem psi, FDAP fdcp);

		// Token: 0x060019A8 RID: 6568
		void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		// Token: 0x060019A9 RID: 6569
		void Close([MarshalAs(UnmanagedType.Error)] int hr);

		// Token: 0x060019AA RID: 6570
		void SetClientGuid([In] ref Guid guid);

		// Token: 0x060019AB RID: 6571
		void ClearClientData();

		// Token: 0x060019AC RID: 6572
		void SetFilter([MarshalAs(UnmanagedType.Interface)] object pFilter);

		// Token: 0x060019AD RID: 6573
		void SetSaveAsItem(IShellItem psi);

		// Token: 0x060019AE RID: 6574
		void SetProperties([MarshalAs(UnmanagedType.Interface)] [In] object pStore);

		// Token: 0x060019AF RID: 6575
		void SetCollectedProperties([MarshalAs(UnmanagedType.Interface)] [In] object pList, [In] int fAppendDefault);

		// Token: 0x060019B0 RID: 6576
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetProperties();

		// Token: 0x060019B1 RID: 6577
		void ApplyProperties(IShellItem psi, [MarshalAs(UnmanagedType.Interface)] object pStore, [In] ref IntPtr hwnd, [MarshalAs(UnmanagedType.Interface)] object pSink);
	}
}
