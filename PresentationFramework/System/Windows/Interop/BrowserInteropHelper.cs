using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace System.Windows.Interop
{
	// Token: 0x02000420 RID: 1056
	public static class BrowserInteropHelper
	{
		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x060032E2 RID: 13026 RVA: 0x00109403 File Offset: 0x00108403
		public static object ClientSite
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x060032E3 RID: 13027 RVA: 0x00109403 File Offset: 0x00108403
		[Dynamic]
		public static dynamic HostScript
		{
			[return: Dynamic]
			get
			{
				return null;
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x060032E4 RID: 13028 RVA: 0x00105F35 File Offset: 0x00104F35
		public static bool IsBrowserHosted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x060032E5 RID: 13029 RVA: 0x001D3242 File Offset: 0x001D2242
		// (set) Token: 0x060032E6 RID: 13030 RVA: 0x001D324E File Offset: 0x001D224E
		internal static HostingFlags HostingFlags
		{
			get
			{
				return BrowserInteropHelper._hostingFlags.Value;
			}
			set
			{
				BrowserInteropHelper._hostingFlags.Value = value;
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x001D325B File Offset: 0x001D225B
		public static Uri Source
		{
			get
			{
				return SiteOfOriginContainer.BrowserSource;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x060032E8 RID: 13032 RVA: 0x001D3264 File Offset: 0x001D2264
		internal static bool IsViewer
		{
			get
			{
				Application application = Application.Current;
				return application != null && application.MimeType == MimeType.Markup;
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x001D3285 File Offset: 0x001D2285
		internal static bool IsHostedInIEorWebOC
		{
			get
			{
				return (BrowserInteropHelper.HostingFlags & HostingFlags.hfHostedInIEorWebOC) > (HostingFlags)0;
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x060032EA RID: 13034 RVA: 0x001D3291 File Offset: 0x001D2291
		// (set) Token: 0x060032EB RID: 13035 RVA: 0x001D32A6 File Offset: 0x001D22A6
		internal static bool IsInitialViewerNavigation
		{
			get
			{
				return BrowserInteropHelper.IsViewer && BrowserInteropHelper._isInitialViewerNavigation.Value;
			}
			set
			{
				BrowserInteropHelper._isInitialViewerNavigation.Value = value;
			}
		} = true;

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x060032EC RID: 13036 RVA: 0x001D32B3 File Offset: 0x001D22B3
		internal static UnsafeNativeMethods.IServiceProvider HostHtmlDocumentServiceProvider
		{
			get
			{
				Invariant.Assert(!BrowserInteropHelper._initializedHostScript.Value || BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value != null);
				return BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value;
			}
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x001D32E0 File Offset: 0x001D22E0
		private static void InitializeHostHtmlDocumentServiceProvider(DynamicScriptObject scriptObject)
		{
			if (BrowserInteropHelper.IsHostedInIEorWebOC && scriptObject.ScriptObject is UnsafeNativeMethods.IHTMLWindow4 && BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value == null)
			{
				object obj;
				Invariant.Assert(scriptObject.TryFindMemberAndInvokeNonWrapped("document", 2, true, null, out obj));
				BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value = (UnsafeNativeMethods.IServiceProvider)obj;
				BrowserInteropHelper._initializedHostScript.Value = true;
			}
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x001D3340 File Offset: 0x001D2340
		private static void HostFilterInput(ref MSG msg, ref bool handled)
		{
			WindowMessage message = (WindowMessage)msg.message;
			if ((message == WindowMessage.WM_INPUT || (message >= WindowMessage.WM_KEYFIRST && message <= WindowMessage.WM_IME_COMPOSITION) || (message >= WindowMessage.WM_MOUSEMOVE && message <= WindowMessage.WM_MOUSEHWHEEL)) && BrowserInteropHelper.ForwardTranslateAccelerator(ref msg, false) == 0)
			{
				handled = true;
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x001D3388 File Offset: 0x001D2388
		internal static IntPtr PostFilterInput(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (!handled && msg >= 256 && msg <= 271)
			{
				MSG msg2 = new MSG(hwnd, msg, wParam, lParam, SafeNativeMethods.GetMessageTime(), 0, 0);
				if (BrowserInteropHelper.ForwardTranslateAccelerator(ref msg2, true) == 0)
				{
					handled = true;
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x001D33CF File Offset: 0x001D23CF
		internal static void InitializeHostFilterInput()
		{
			ComponentDispatcher.ThreadFilterMessage += BrowserInteropHelper.HostFilterInput;
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x001D33E4 File Offset: 0x001D23E4
		private static void EnsureScriptInteropAllowed()
		{
			if (BrowserInteropHelper._isScriptInteropDisabled.Value == null)
			{
				BrowserInteropHelper._isScriptInteropDisabled.Value = new bool?(SafeSecurityHelper.IsFeatureDisabled(SafeSecurityHelper.KeyToRead.ScriptInteropDisable));
			}
		}

		// Token: 0x060032F2 RID: 13042
		[DllImport("PresentationHost_cor3.dll")]
		private static extern int ForwardTranslateAccelerator(ref MSG pMsg, bool appUnhandled);

		// Token: 0x04001C0C RID: 7180
		private static SecurityCriticalDataForSet<HostingFlags> _hostingFlags;

		// Token: 0x04001C0D RID: 7181
		private static SecurityCriticalDataForSet<bool> _isInitialViewerNavigation;

		// Token: 0x04001C0E RID: 7182
		private static SecurityCriticalDataForSet<bool?> _isScriptInteropDisabled;

		// Token: 0x04001C0F RID: 7183
		private static SecurityCriticalDataForSet<UnsafeNativeMethods.IServiceProvider> _hostHtmlDocumentServiceProvider;

		// Token: 0x04001C10 RID: 7184
		private static SecurityCriticalDataForSet<bool> _initializedHostScript;
	}
}
