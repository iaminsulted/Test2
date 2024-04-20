using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x020002AF RID: 687
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("56FDF342-FD6D-11d0-958A-006097C9A090")]
	[ComImport]
	internal interface ITaskbarList
	{
		// Token: 0x060019C4 RID: 6596
		void HrInit();

		// Token: 0x060019C5 RID: 6597
		void AddTab(IntPtr hwnd);

		// Token: 0x060019C6 RID: 6598
		void DeleteTab(IntPtr hwnd);

		// Token: 0x060019C7 RID: 6599
		void ActivateTab(IntPtr hwnd);

		// Token: 0x060019C8 RID: 6600
		void SetActiveAlt(IntPtr hwnd);
	}
}
