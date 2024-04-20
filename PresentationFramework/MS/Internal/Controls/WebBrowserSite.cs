using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000263 RID: 611
	internal class WebBrowserSite : ActiveXSite, UnsafeNativeMethods.IDocHostUIHandler, UnsafeNativeMethods.IOleControlSite
	{
		// Token: 0x060017B4 RID: 6068 RVA: 0x0015ECCD File Offset: 0x0015DCCD
		internal WebBrowserSite(WebBrowser host) : base(host)
		{
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		int UnsafeNativeMethods.IDocHostUIHandler.ShowContextMenu(int dwID, NativeMethods.POINT pt, object pcmdtReserved, object pdispReserved)
		{
			return 1;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0015ECD6 File Offset: 0x0015DCD6
		int UnsafeNativeMethods.IDocHostUIHandler.GetHostInfo(NativeMethods.DOCHOSTUIINFO info)
		{
			WebBrowser webBrowser = (WebBrowser)base.Host;
			info.dwDoubleClick = 0;
			info.dwFlags = 94846994;
			return 0;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IDocHostUIHandler.EnableModeless(bool fEnable)
		{
			return -2147467263;
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IDocHostUIHandler.ShowUI(int dwID, UnsafeNativeMethods.IOleInPlaceActiveObject activeObject, NativeMethods.IOleCommandTarget commandTarget, UnsafeNativeMethods.IOleInPlaceFrame frame, UnsafeNativeMethods.IOleInPlaceUIWindow doc)
		{
			return -2147467263;
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IDocHostUIHandler.HideUI()
		{
			return -2147467263;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IDocHostUIHandler.UpdateUI()
		{
			return -2147467263;
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IDocHostUIHandler.OnDocWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IDocHostUIHandler.OnFrameWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IDocHostUIHandler.ResizeBorder(NativeMethods.COMRECT rect, UnsafeNativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow)
		{
			return -2147467263;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0015C372 File Offset: 0x0015B372
		int UnsafeNativeMethods.IDocHostUIHandler.GetOptionKeyPath(string[] pbstrKey, int dw)
		{
			return -2147467263;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0015ECF7 File Offset: 0x0015DCF7
		int UnsafeNativeMethods.IDocHostUIHandler.GetDropTarget(UnsafeNativeMethods.IOleDropTarget pDropTarget, out UnsafeNativeMethods.IOleDropTarget ppDropTarget)
		{
			ppDropTarget = null;
			return -2147467263;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0015ED04 File Offset: 0x0015DD04
		int UnsafeNativeMethods.IDocHostUIHandler.GetExternal(out object ppDispatch)
		{
			WebBrowser webBrowser = (WebBrowser)base.Host;
			ppDispatch = webBrowser.HostingAdaptor.ObjectForScripting;
			return 0;
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		int UnsafeNativeMethods.IDocHostUIHandler.TranslateAccelerator(ref MSG msg, ref Guid group, int nCmdID)
		{
			return 1;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0015C624 File Offset: 0x0015B624
		int UnsafeNativeMethods.IDocHostUIHandler.TranslateUrl(int dwTranslate, string strUrlIn, out string pstrUrlOut)
		{
			pstrUrlOut = null;
			return -2147467263;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0015ECF7 File Offset: 0x0015DCF7
		int UnsafeNativeMethods.IDocHostUIHandler.FilterDataObject(IDataObject pDO, out IDataObject ppDORet)
		{
			ppDORet = null;
			return -2147467263;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0015ED2C File Offset: 0x0015DD2C
		int UnsafeNativeMethods.IOleControlSite.TranslateAccelerator(ref MSG msg, int grfModifiers)
		{
			if (msg.message == 256 && (int)msg.wParam == 9)
			{
				FocusNavigationDirection focusNavigationDirection = ((grfModifiers & 1) != 0) ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next;
				base.Host.Dispatcher.Invoke(DispatcherPriority.Send, new SendOrPostCallback(this.MoveFocusCallback), focusNavigationDirection);
				return 0;
			}
			return 1;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0015ED87 File Offset: 0x0015DD87
		private void MoveFocusCallback(object direction)
		{
			base.Host.MoveFocus(new TraversalRequest((FocusNavigationDirection)direction));
		}
	}
}
