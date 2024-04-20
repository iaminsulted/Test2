using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A7 RID: 679
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
	[ComImport]
	internal interface IFileDialog : IModalWindow
	{
		// Token: 0x06001963 RID: 6499
		[PreserveSig]
		HRESULT Show(IntPtr parent);

		// Token: 0x06001964 RID: 6500
		void SetFileTypes(uint cFileTypes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [In] COMDLG_FILTERSPEC[] rgFilterSpec);

		// Token: 0x06001965 RID: 6501
		void SetFileTypeIndex(uint iFileType);

		// Token: 0x06001966 RID: 6502
		uint GetFileTypeIndex();

		// Token: 0x06001967 RID: 6503
		uint Advise(IFileDialogEvents pfde);

		// Token: 0x06001968 RID: 6504
		void Unadvise(uint dwCookie);

		// Token: 0x06001969 RID: 6505
		void SetOptions(FOS fos);

		// Token: 0x0600196A RID: 6506
		FOS GetOptions();

		// Token: 0x0600196B RID: 6507
		void SetDefaultFolder(IShellItem psi);

		// Token: 0x0600196C RID: 6508
		void SetFolder(IShellItem psi);

		// Token: 0x0600196D RID: 6509
		IShellItem GetFolder();

		// Token: 0x0600196E RID: 6510
		IShellItem GetCurrentSelection();

		// Token: 0x0600196F RID: 6511
		void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x06001970 RID: 6512
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetFileName();

		// Token: 0x06001971 RID: 6513
		void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		// Token: 0x06001972 RID: 6514
		void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);

		// Token: 0x06001973 RID: 6515
		void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		// Token: 0x06001974 RID: 6516
		IShellItem GetResult();

		// Token: 0x06001975 RID: 6517
		void AddPlace(IShellItem psi, FDAP alignment);

		// Token: 0x06001976 RID: 6518
		void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		// Token: 0x06001977 RID: 6519
		void Close([MarshalAs(UnmanagedType.Error)] int hr);

		// Token: 0x06001978 RID: 6520
		void SetClientGuid([In] ref Guid guid);

		// Token: 0x06001979 RID: 6521
		void ClearClientData();

		// Token: 0x0600197A RID: 6522
		void SetFilter([MarshalAs(UnmanagedType.Interface)] object pFilter);
	}
}
