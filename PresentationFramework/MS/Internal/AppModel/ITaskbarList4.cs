using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x020002B2 RID: 690
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	[ComImport]
	internal interface ITaskbarList4 : ITaskbarList3, ITaskbarList2, ITaskbarList
	{
		// Token: 0x060019E1 RID: 6625
		void HrInit();

		// Token: 0x060019E2 RID: 6626
		void AddTab(IntPtr hwnd);

		// Token: 0x060019E3 RID: 6627
		void DeleteTab(IntPtr hwnd);

		// Token: 0x060019E4 RID: 6628
		void ActivateTab(IntPtr hwnd);

		// Token: 0x060019E5 RID: 6629
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x060019E6 RID: 6630
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

		// Token: 0x060019E7 RID: 6631
		[PreserveSig]
		HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

		// Token: 0x060019E8 RID: 6632
		[PreserveSig]
		HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

		// Token: 0x060019E9 RID: 6633
		[PreserveSig]
		HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

		// Token: 0x060019EA RID: 6634
		[PreserveSig]
		HRESULT UnregisterTab(IntPtr hwndTab);

		// Token: 0x060019EB RID: 6635
		[PreserveSig]
		HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

		// Token: 0x060019EC RID: 6636
		[PreserveSig]
		HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

		// Token: 0x060019ED RID: 6637
		[PreserveSig]
		HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x060019EE RID: 6638
		[PreserveSig]
		HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x060019EF RID: 6639
		[PreserveSig]
		HRESULT ThumbBarSetImageList(IntPtr hwnd, [MarshalAs(UnmanagedType.IUnknown)] object himl);

		// Token: 0x060019F0 RID: 6640
		[PreserveSig]
		HRESULT SetOverlayIcon(IntPtr hwnd, NativeMethods.IconHandle hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

		// Token: 0x060019F1 RID: 6641
		[PreserveSig]
		HRESULT SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

		// Token: 0x060019F2 RID: 6642
		[PreserveSig]
		HRESULT SetThumbnailClip(IntPtr hwnd, NativeMethods.RefRECT prcClip);

		// Token: 0x060019F3 RID: 6643
		[PreserveSig]
		HRESULT SetTabProperties(IntPtr hwndTab, STPF stpFlags);
	}
}
