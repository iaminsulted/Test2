using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using MS.Win32;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020005A3 RID: 1443
	public class WindowAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600461E RID: 17950 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public WindowAutomationPeer(Window owner) : base(owner)
		{
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x002256AE File Offset: 0x002246AE
		protected override string GetClassNameCore()
		{
			return "Window";
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x002256B8 File Offset: 0x002246B8
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (text == string.Empty)
			{
				Window window = (Window)base.Owner;
				if (!window.IsSourceWindowNull)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					UnsafeNativeMethods.GetWindowText(new HandleRef(null, window.CriticalHandle), stringBuilder, stringBuilder.Capacity);
					text = stringBuilder.ToString();
					if (text == null)
					{
						text = string.Empty;
					}
				}
			}
			return text;
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x0013CE2F File Offset: 0x0013BE2F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Window;
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x00225724 File Offset: 0x00224724
		protected override Rect GetBoundingRectangleCore()
		{
			Window window = (Window)base.Owner;
			Rect result = new Rect(0.0, 0.0, 0.0, 0.0);
			if (!window.IsSourceWindowNull)
			{
				NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, 0, 0);
				IntPtr criticalHandle = window.CriticalHandle;
				if (criticalHandle != IntPtr.Zero)
				{
					try
					{
						SafeNativeMethods.GetWindowRect(new HandleRef(null, criticalHandle), ref rect);
					}
					catch (Win32Exception)
					{
					}
				}
				result = new Rect((double)rect.left, (double)rect.top, (double)(rect.right - rect.left), (double)(rect.bottom - rect.top));
			}
			return result;
		}
	}
}
