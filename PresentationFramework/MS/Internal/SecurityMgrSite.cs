using System;
using System.Windows;
using MS.Win32;

namespace MS.Internal
{
	// Token: 0x020000EC RID: 236
	internal class SecurityMgrSite : NativeMethods.IInternetSecurityMgrSite
	{
		// Token: 0x0600043A RID: 1082 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal SecurityMgrSite()
		{
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000FF5F0 File Offset: 0x000FE5F0
		public void GetWindow(ref IntPtr phwnd)
		{
			phwnd = IntPtr.Zero;
			if (Application.Current != null)
			{
				Window mainWindow = Application.Current.MainWindow;
				if (mainWindow != null)
				{
					phwnd = mainWindow.CriticalHandle;
				}
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void EnableModeless(bool fEnable)
		{
		}
	}
}
