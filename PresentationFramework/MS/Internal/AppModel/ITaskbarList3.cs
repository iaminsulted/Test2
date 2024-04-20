using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x020002B1 RID: 689
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	[ComImport]
	internal interface ITaskbarList3 : ITaskbarList2, ITaskbarList
	{
		// Token: 0x060019CF RID: 6607
		void HrInit();

		// Token: 0x060019D0 RID: 6608
		void AddTab(IntPtr hwnd);

		// Token: 0x060019D1 RID: 6609
		void DeleteTab(IntPtr hwnd);

		// Token: 0x060019D2 RID: 6610
		void ActivateTab(IntPtr hwnd);

		// Token: 0x060019D3 RID: 6611
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x060019D4 RID: 6612
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

		// Token: 0x060019D5 RID: 6613
		[PreserveSig]
		HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

		// Token: 0x060019D6 RID: 6614
		[PreserveSig]
		HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

		// Token: 0x060019D7 RID: 6615
		[PreserveSig]
		HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

		// Token: 0x060019D8 RID: 6616
		[PreserveSig]
		HRESULT UnregisterTab(IntPtr hwndTab);

		// Token: 0x060019D9 RID: 6617
		[PreserveSig]
		HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

		// Token: 0x060019DA RID: 6618
		[PreserveSig]
		HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

		// Token: 0x060019DB RID: 6619
		[PreserveSig]
		HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x060019DC RID: 6620
		[PreserveSig]
		HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x060019DD RID: 6621
		[PreserveSig]
		HRESULT ThumbBarSetImageList(IntPtr hwnd, [MarshalAs(UnmanagedType.IUnknown)] object himl);

		// Token: 0x060019DE RID: 6622
		[PreserveSig]
		HRESULT SetOverlayIcon(IntPtr hwnd, NativeMethods.IconHandle hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

		// Token: 0x060019DF RID: 6623
		[PreserveSig]
		HRESULT SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

		// Token: 0x060019E0 RID: 6624
		[PreserveSig]
		HRESULT SetThumbnailClip(IntPtr hwnd, NativeMethods.RefRECT prcClip);
	}
}
