using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Controls;
using System.Windows.Navigation;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Internal.IO.Packaging;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000262 RID: 610
	[ClassInterface(ClassInterfaceType.None)]
	internal class WebBrowserEvent : InternalDispatchObject<UnsafeNativeMethods.DWebBrowserEvents2>, UnsafeNativeMethods.DWebBrowserEvents2
	{
		// Token: 0x06001790 RID: 6032 RVA: 0x0015E9C7 File Offset: 0x0015D9C7
		public WebBrowserEvent(WebBrowser parent)
		{
			this._parent = parent;
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x0015E9D8 File Offset: 0x0015D9D8
		public void BeforeNavigate2(object pDisp, ref object url, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
		{
			bool flag = false;
			bool flag2 = false;
			try
			{
				if (targetFrameName == null)
				{
					targetFrameName = "";
				}
				if (headers == null)
				{
					headers = "";
				}
				string text = (string)url;
				Uri uri = string.IsNullOrEmpty(text) ? null : new Uri(text);
				UnsafeNativeMethods.IWebBrowser2 webBrowser = (UnsafeNativeMethods.IWebBrowser2)pDisp;
				if (this._parent.AxIWebBrowser2 == webBrowser)
				{
					if (this._parent.NavigatingToAboutBlank && string.Compare(text, "about:blank", StringComparison.OrdinalIgnoreCase) != 0)
					{
						this._parent.NavigatingToAboutBlank = false;
					}
					if (this._parent.NavigatingToAboutBlank)
					{
						uri = null;
					}
					NavigatingCancelEventArgs navigatingCancelEventArgs = new NavigatingCancelEventArgs(uri, null, null, null, NavigationMode.New, null, null, true);
					Guid lastNavigation = this._parent.LastNavigation;
					this._parent.OnNavigating(navigatingCancelEventArgs);
					if (this._parent.LastNavigation != lastNavigation)
					{
						flag = true;
					}
					flag2 = navigatingCancelEventArgs.Cancel;
				}
			}
			catch
			{
				flag2 = true;
			}
			finally
			{
				if (flag2 && !flag)
				{
					this._parent.CleanInternalState();
				}
				if (flag2 || flag)
				{
					cancel = true;
				}
			}
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x0015EAF0 File Offset: 0x0015DAF0
		private static bool IsAllowedScriptScheme(Uri uri)
		{
			return uri != null && (uri.Scheme == "javascript" || uri.Scheme == "vbscript");
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0015EB24 File Offset: 0x0015DB24
		public void NavigateComplete2(object pDisp, ref object url)
		{
			UnsafeNativeMethods.IWebBrowser2 webBrowser = (UnsafeNativeMethods.IWebBrowser2)pDisp;
			if (this._parent.AxIWebBrowser2 == webBrowser)
			{
				if (this._parent.DocumentStream != null)
				{
					Invariant.Assert(this._parent.NavigatingToAboutBlank && string.Compare((string)url, "about:blank", StringComparison.OrdinalIgnoreCase) == 0);
					try
					{
						UnsafeNativeMethods.IHTMLDocument nativeHTMLDocument = this._parent.NativeHTMLDocument;
						if (nativeHTMLDocument != null)
						{
							UnsafeNativeMethods.IPersistStreamInit persistStreamInit = nativeHTMLDocument as UnsafeNativeMethods.IPersistStreamInit;
							IStream pstm = new ManagedIStream(this._parent.DocumentStream);
							persistStreamInit.Load(pstm);
						}
						return;
					}
					finally
					{
						this._parent.DocumentStream = null;
					}
				}
				string text = (string)url;
				if (this._parent.NavigatingToAboutBlank)
				{
					Invariant.Assert(string.Compare(text, "about:blank", StringComparison.OrdinalIgnoreCase) == 0);
					text = null;
				}
				NavigationEventArgs e = new NavigationEventArgs(string.IsNullOrEmpty(text) ? null : new Uri(text), null, null, null, null, true);
				this._parent.OnNavigated(e);
			}
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0015EC20 File Offset: 0x0015DC20
		public void DocumentComplete(object pDisp, ref object url)
		{
			UnsafeNativeMethods.IWebBrowser2 webBrowser = (UnsafeNativeMethods.IWebBrowser2)pDisp;
			if (this._parent.AxIWebBrowser2 == webBrowser)
			{
				string text = (string)url;
				if (this._parent.NavigatingToAboutBlank)
				{
					Invariant.Assert(string.Compare(text, "about:blank", StringComparison.OrdinalIgnoreCase) == 0);
					text = null;
				}
				NavigationEventArgs e = new NavigationEventArgs(string.IsNullOrEmpty(text) ? null : new Uri(text), null, null, null, null, true);
				this._parent.OnLoadCompleted(e);
			}
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0015EC95 File Offset: 0x0015DC95
		public void CommandStateChange(long command, bool enable)
		{
			if (command == 2L)
			{
				this._parent._canGoBack = enable;
				return;
			}
			if (command == 1L)
			{
				this._parent._canGoForward = enable;
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void TitleChange(string text)
		{
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void SetSecureLockIcon(int secureLockIcon)
		{
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void NewWindow2(ref object ppDisp, ref bool cancel)
		{
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void ProgressChange(int progress, int progressMax)
		{
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0015ECBA File Offset: 0x0015DCBA
		public void StatusTextChange(string text)
		{
			this._parent.RaiseEvent(new RequestSetStatusBarEventArgs(text));
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void DownloadBegin()
		{
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void FileDownload(ref bool activeDocument, ref bool cancel)
		{
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void PrivacyImpactedStateChange(bool bImpacted)
		{
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
		{
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void PrintTemplateTeardown(object pDisp)
		{
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void PrintTemplateInstantiation(object pDisp)
		{
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
		{
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void ClientToHostWindow(ref long cX, ref long cY)
		{
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void WindowClosing(bool isChildWindow, ref bool cancel)
		{
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void WindowSetHeight(int height)
		{
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void WindowSetWidth(int width)
		{
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void WindowSetTop(int top)
		{
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void WindowSetLeft(int left)
		{
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void WindowSetResizable(bool resizable)
		{
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnTheaterMode(bool theaterMode)
		{
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnFullScreen(bool fullScreen)
		{
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnStatusBar(bool statusBar)
		{
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnMenuBar(bool menuBar)
		{
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnToolBar(bool toolBar)
		{
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnVisible(bool visible)
		{
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnQuit()
		{
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void PropertyChange(string szProperty)
		{
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void DownloadComplete()
		{
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void SetPhishingFilterStatus(uint phishingFilterStatus)
		{
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void WindowStateChanged(uint dwFlags, uint dwValidFlagsMask)
		{
		}

		// Token: 0x04000CB1 RID: 3249
		private WebBrowser _parent;
	}
}
