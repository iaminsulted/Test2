using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x02000330 RID: 816
	internal static class SecurityHelper
	{
		// Token: 0x06001EA0 RID: 7840 RVA: 0x0016FEED File Offset: 0x0016EEED
		internal static Exception GetExceptionForHR(int hr)
		{
			return Marshal.GetExceptionForHR(hr, new IntPtr(-1));
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0016FEFB File Offset: 0x0016EEFB
		internal static void ShowMessageBoxHelper(Window parent, string text, string title, MessageBoxButton buttons, MessageBoxImage image)
		{
			if (parent != null)
			{
				MessageBox.Show(parent, text, title, buttons, image);
				return;
			}
			MessageBox.Show(text, title, buttons, image);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0016FF18 File Offset: 0x0016EF18
		internal static void ShowMessageBoxHelper(IntPtr parentHwnd, string text, string title, MessageBoxButton buttons, MessageBoxImage image)
		{
			MessageBox.ShowCore(parentHwnd, text, title, buttons, image, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0016FF28 File Offset: 0x0016EF28
		internal static bool AreStringTypesEqual(string m1, string m2)
		{
			return string.Compare(m1, m2, StringComparison.OrdinalIgnoreCase) == 0;
		}
	}
}
