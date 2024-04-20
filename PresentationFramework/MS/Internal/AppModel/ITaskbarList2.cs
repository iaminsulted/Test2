using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x020002B0 RID: 688
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("602D4995-B13A-429b-A66E-1935E44F4317")]
	[ComImport]
	internal interface ITaskbarList2 : ITaskbarList
	{
		// Token: 0x060019C9 RID: 6601
		void HrInit();

		// Token: 0x060019CA RID: 6602
		void AddTab(IntPtr hwnd);

		// Token: 0x060019CB RID: 6603
		void DeleteTab(IntPtr hwnd);

		// Token: 0x060019CC RID: 6604
		void ActivateTab(IntPtr hwnd);

		// Token: 0x060019CD RID: 6605
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x060019CE RID: 6606
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
	}
}
