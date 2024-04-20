using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.IO.Packaging;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Utility;
using MS.Win32;

namespace System.Windows.Controls
{
	// Token: 0x0200080B RID: 2059
	public sealed class WebBrowser : ActiveXHost
	{
		// Token: 0x06007877 RID: 30839 RVA: 0x00300683 File Offset: 0x002FF683
		static WebBrowser()
		{
			WebBrowser.TurnOnFeatureControlKeys();
			ControlsTraceLogger.AddControl(TelemetryControls.WebBrowser);
		}

		// Token: 0x06007878 RID: 30840 RVA: 0x00300698 File Offset: 0x002FF698
		public WebBrowser() : base(new Guid("8856f961-340a-11d0-a96b-00c04fd705a2"), true)
		{
			this._hostingAdaptor = new WebBrowser.WebOCHostingAdaptor(this);
		}

		// Token: 0x06007879 RID: 30841 RVA: 0x003006B7 File Offset: 0x002FF6B7
		public void Navigate(Uri source)
		{
			this.Navigate(source, null, null, null);
		}

		// Token: 0x0600787A RID: 30842 RVA: 0x003006C3 File Offset: 0x002FF6C3
		public void Navigate(string source)
		{
			this.Navigate(source, null, null, null);
		}

		// Token: 0x0600787B RID: 30843 RVA: 0x003006D0 File Offset: 0x002FF6D0
		public void Navigate(Uri source, string targetFrameName, byte[] postData, string additionalHeaders)
		{
			object obj = targetFrameName;
			object obj2 = postData;
			object obj3 = additionalHeaders;
			this.DoNavigate(source, ref obj, ref obj2, ref obj3, false);
		}

		// Token: 0x0600787C RID: 30844 RVA: 0x003006F4 File Offset: 0x002FF6F4
		public void Navigate(string source, string targetFrameName, byte[] postData, string additionalHeaders)
		{
			object obj = targetFrameName;
			object obj2 = postData;
			object obj3 = additionalHeaders;
			Uri source2 = new Uri(source);
			this.DoNavigate(source2, ref obj, ref obj2, ref obj3, true);
		}

		// Token: 0x0600787D RID: 30845 RVA: 0x0030071D File Offset: 0x002FF71D
		public void NavigateToStream(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.DocumentStream = stream;
			this.Source = null;
		}

		// Token: 0x0600787E RID: 30846 RVA: 0x0030073C File Offset: 0x002FF73C
		public void NavigateToString(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException("text");
			}
			MemoryStream memoryStream = new MemoryStream(text.Length);
			StreamWriter streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(text);
			streamWriter.Flush();
			memoryStream.Position = 0L;
			this.NavigateToStream(memoryStream);
		}

		// Token: 0x0600787F RID: 30847 RVA: 0x00300789 File Offset: 0x002FF789
		public void GoBack()
		{
			base.VerifyAccess();
			this.AxIWebBrowser2.GoBack();
		}

		// Token: 0x06007880 RID: 30848 RVA: 0x0030079C File Offset: 0x002FF79C
		public void GoForward()
		{
			base.VerifyAccess();
			this.AxIWebBrowser2.GoForward();
		}

		// Token: 0x06007881 RID: 30849 RVA: 0x003007AF File Offset: 0x002FF7AF
		public void Refresh()
		{
			base.VerifyAccess();
			this.AxIWebBrowser2.Refresh();
		}

		// Token: 0x06007882 RID: 30850 RVA: 0x003007C4 File Offset: 0x002FF7C4
		public void Refresh(bool noCache)
		{
			base.VerifyAccess();
			object obj = noCache ? 3 : 0;
			this.AxIWebBrowser2.Refresh2(ref obj);
		}

		// Token: 0x06007883 RID: 30851 RVA: 0x003007F1 File Offset: 0x002FF7F1
		public object InvokeScript(string scriptName)
		{
			return this.InvokeScript(scriptName, null);
		}

		// Token: 0x06007884 RID: 30852 RVA: 0x003007FC File Offset: 0x002FF7FC
		public object InvokeScript(string scriptName, params object[] args)
		{
			base.VerifyAccess();
			if (string.IsNullOrEmpty(scriptName))
			{
				throw new ArgumentNullException("scriptName");
			}
			UnsafeNativeMethods.IDispatchEx dispatchEx = null;
			UnsafeNativeMethods.IHTMLDocument2 nativeHTMLDocument = this.NativeHTMLDocument;
			if (nativeHTMLDocument != null)
			{
				dispatchEx = (nativeHTMLDocument.GetScript() as UnsafeNativeMethods.IDispatchEx);
			}
			object result = null;
			if (dispatchEx != null)
			{
				NativeMethods.DISPPARAMS dispparams = new NativeMethods.DISPPARAMS();
				dispparams.rgvarg = IntPtr.Zero;
				try
				{
					Guid empty = Guid.Empty;
					string[] rgszNames = new string[]
					{
						scriptName
					};
					int[] array = new int[]
					{
						-1
					};
					dispatchEx.GetIDsOfNames(ref empty, rgszNames, 1, Thread.CurrentThread.CurrentCulture.LCID, array).ThrowIfFailed();
					if (args != null)
					{
						Array.Reverse<object>(args);
					}
					dispparams.rgvarg = ((args == null) ? IntPtr.Zero : UnsafeNativeMethods.ArrayToVARIANTHelper.ArrayToVARIANTVector(args));
					dispparams.cArgs = (uint)((args == null) ? 0 : args.Length);
					dispparams.rgdispidNamedArgs = IntPtr.Zero;
					dispparams.cNamedArgs = 0U;
					dispatchEx.InvokeEx(array[0], Thread.CurrentThread.CurrentCulture.LCID, 1, dispparams, out result, new NativeMethods.EXCEPINFO(), null).ThrowIfFailed();
					return result;
				}
				finally
				{
					if (dispparams.rgvarg != IntPtr.Zero)
					{
						UnsafeNativeMethods.ArrayToVARIANTHelper.FreeVARIANTVector(dispparams.rgvarg, args.Length);
					}
				}
			}
			throw new InvalidOperationException(SR.Get("CannotInvokeScript"));
		}

		// Token: 0x17001BE3 RID: 7139
		// (get) Token: 0x06007886 RID: 30854 RVA: 0x00300958 File Offset: 0x002FF958
		// (set) Token: 0x06007885 RID: 30853 RVA: 0x00300948 File Offset: 0x002FF948
		public Uri Source
		{
			get
			{
				base.VerifyAccess();
				string text = this.AxIWebBrowser2.LocationURL;
				if (this.NavigatingToAboutBlank)
				{
					text = null;
				}
				if (!string.IsNullOrEmpty(text))
				{
					return new Uri(text);
				}
				return null;
			}
			set
			{
				base.VerifyAccess();
				this.Navigate(value);
			}
		}

		// Token: 0x17001BE4 RID: 7140
		// (get) Token: 0x06007887 RID: 30855 RVA: 0x00300991 File Offset: 0x002FF991
		public bool CanGoBack
		{
			get
			{
				base.VerifyAccess();
				return !base.IsDisposed && this._canGoBack;
			}
		}

		// Token: 0x17001BE5 RID: 7141
		// (get) Token: 0x06007888 RID: 30856 RVA: 0x003009A9 File Offset: 0x002FF9A9
		public bool CanGoForward
		{
			get
			{
				base.VerifyAccess();
				return !base.IsDisposed && this._canGoForward;
			}
		}

		// Token: 0x17001BE6 RID: 7142
		// (get) Token: 0x06007889 RID: 30857 RVA: 0x003009C1 File Offset: 0x002FF9C1
		// (set) Token: 0x0600788A RID: 30858 RVA: 0x003009CF File Offset: 0x002FF9CF
		public object ObjectForScripting
		{
			get
			{
				base.VerifyAccess();
				return this._objectForScripting;
			}
			set
			{
				base.VerifyAccess();
				if (value != null && !MarshalLocal.IsTypeVisibleFromCom(value.GetType()))
				{
					throw new ArgumentException(SR.Get("NeedToBeComVisible"));
				}
				this._objectForScripting = value;
				this._hostingAdaptor.ObjectForScripting = value;
			}
		}

		// Token: 0x17001BE7 RID: 7143
		// (get) Token: 0x0600788B RID: 30859 RVA: 0x00300A0A File Offset: 0x002FFA0A
		public object Document
		{
			get
			{
				base.VerifyAccess();
				return this.AxIWebBrowser2.Document;
			}
		}

		// Token: 0x1400014E RID: 334
		// (add) Token: 0x0600788C RID: 30860 RVA: 0x00300A20 File Offset: 0x002FFA20
		// (remove) Token: 0x0600788D RID: 30861 RVA: 0x00300A58 File Offset: 0x002FFA58
		public event NavigatingCancelEventHandler Navigating;

		// Token: 0x1400014F RID: 335
		// (add) Token: 0x0600788E RID: 30862 RVA: 0x00300A90 File Offset: 0x002FFA90
		// (remove) Token: 0x0600788F RID: 30863 RVA: 0x00300AC8 File Offset: 0x002FFAC8
		public event NavigatedEventHandler Navigated;

		// Token: 0x14000150 RID: 336
		// (add) Token: 0x06007890 RID: 30864 RVA: 0x00300B00 File Offset: 0x002FFB00
		// (remove) Token: 0x06007891 RID: 30865 RVA: 0x00300B38 File Offset: 0x002FFB38
		public event LoadCompletedEventHandler LoadCompleted;

		// Token: 0x06007892 RID: 30866 RVA: 0x00300B6D File Offset: 0x002FFB6D
		internal void OnNavigating(NavigatingCancelEventArgs e)
		{
			base.VerifyAccess();
			if (this.Navigating != null)
			{
				this.Navigating(this, e);
			}
		}

		// Token: 0x06007893 RID: 30867 RVA: 0x00300B8A File Offset: 0x002FFB8A
		internal void OnNavigated(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.Navigated != null)
			{
				this.Navigated(this, e);
			}
		}

		// Token: 0x06007894 RID: 30868 RVA: 0x00300BA7 File Offset: 0x002FFBA7
		internal void OnLoadCompleted(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.LoadCompleted != null)
			{
				this.LoadCompleted(this, e);
			}
		}

		// Token: 0x06007895 RID: 30869 RVA: 0x00300BC4 File Offset: 0x002FFBC4
		internal override object CreateActiveXObject(Guid clsid)
		{
			return this._hostingAdaptor.CreateWebOC();
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x00300BD1 File Offset: 0x002FFBD1
		internal override void AttachInterfaces(object nativeActiveXObject)
		{
			this._axIWebBrowser2 = (UnsafeNativeMethods.IWebBrowser2)nativeActiveXObject;
		}

		// Token: 0x06007897 RID: 30871 RVA: 0x00300BDF File Offset: 0x002FFBDF
		internal override void DetachInterfaces()
		{
			this._axIWebBrowser2 = null;
		}

		// Token: 0x06007898 RID: 30872 RVA: 0x00300BE8 File Offset: 0x002FFBE8
		internal override void CreateSink()
		{
			this._cookie = new ConnectionPointCookie(this._axIWebBrowser2, this._hostingAdaptor.CreateEventSink(), typeof(UnsafeNativeMethods.DWebBrowserEvents2));
		}

		// Token: 0x06007899 RID: 30873 RVA: 0x00300C10 File Offset: 0x002FFC10
		internal override void DetachSink()
		{
			if (this._cookie != null)
			{
				this._cookie.Disconnect();
				this._cookie = null;
			}
		}

		// Token: 0x0600789A RID: 30874 RVA: 0x00300C2C File Offset: 0x002FFC2C
		internal override ActiveXSite CreateActiveXSite()
		{
			return new WebBrowserSite(this);
		}

		// Token: 0x0600789B RID: 30875 RVA: 0x00300C34 File Offset: 0x002FFC34
		internal override DrawingGroup GetDrawing()
		{
			return base.GetDrawing();
		}

		// Token: 0x0600789C RID: 30876 RVA: 0x00300C3C File Offset: 0x002FFC3C
		internal void CleanInternalState()
		{
			this.NavigatingToAboutBlank = false;
			this.DocumentStream = null;
		}

		// Token: 0x17001BE8 RID: 7144
		// (get) Token: 0x0600789D RID: 30877 RVA: 0x00300C4C File Offset: 0x002FFC4C
		internal UnsafeNativeMethods.IHTMLDocument2 NativeHTMLDocument
		{
			get
			{
				return this.AxIWebBrowser2.Document as UnsafeNativeMethods.IHTMLDocument2;
			}
		}

		// Token: 0x17001BE9 RID: 7145
		// (get) Token: 0x0600789E RID: 30878 RVA: 0x00300C60 File Offset: 0x002FFC60
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal UnsafeNativeMethods.IWebBrowser2 AxIWebBrowser2
		{
			get
			{
				if (this._axIWebBrowser2 == null)
				{
					if (base.IsDisposed)
					{
						throw new ObjectDisposedException(base.GetType().Name);
					}
					base.TransitionUpTo(ActiveXHelper.ActiveXState.Running);
				}
				if (this._axIWebBrowser2 == null)
				{
					throw new InvalidOperationException(SR.Get("WebBrowserNoCastToIWebBrowser2"));
				}
				return this._axIWebBrowser2;
			}
		}

		// Token: 0x17001BEA RID: 7146
		// (get) Token: 0x0600789F RID: 30879 RVA: 0x00300CB5 File Offset: 0x002FFCB5
		internal WebBrowser.WebOCHostingAdaptor HostingAdaptor
		{
			get
			{
				return this._hostingAdaptor;
			}
		}

		// Token: 0x17001BEB RID: 7147
		// (get) Token: 0x060078A0 RID: 30880 RVA: 0x00300CBD File Offset: 0x002FFCBD
		// (set) Token: 0x060078A1 RID: 30881 RVA: 0x00300CC5 File Offset: 0x002FFCC5
		internal Stream DocumentStream
		{
			get
			{
				return this._documentStream;
			}
			set
			{
				this._documentStream = value;
			}
		}

		// Token: 0x17001BEC RID: 7148
		// (get) Token: 0x060078A2 RID: 30882 RVA: 0x00300CCE File Offset: 0x002FFCCE
		// (set) Token: 0x060078A3 RID: 30883 RVA: 0x00300CDB File Offset: 0x002FFCDB
		internal bool NavigatingToAboutBlank
		{
			get
			{
				return this._navigatingToAboutBlank.Value;
			}
			set
			{
				this._navigatingToAboutBlank.Value = value;
			}
		}

		// Token: 0x17001BED RID: 7149
		// (get) Token: 0x060078A4 RID: 30884 RVA: 0x00300CE9 File Offset: 0x002FFCE9
		// (set) Token: 0x060078A5 RID: 30885 RVA: 0x00300CF6 File Offset: 0x002FFCF6
		internal Guid LastNavigation
		{
			get
			{
				return this._lastNavigation.Value;
			}
			set
			{
				this._lastNavigation.Value = value;
			}
		}

		// Token: 0x060078A6 RID: 30886 RVA: 0x00300D04 File Offset: 0x002FFD04
		private void LoadedHandler(object sender, RoutedEventArgs args)
		{
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this);
			if (presentationSource != null && presentationSource.RootVisual is PopupRoot)
			{
				throw new InvalidOperationException(SR.Get("CannotBeInsidePopup"));
			}
		}

		// Token: 0x060078A7 RID: 30887 RVA: 0x00300D38 File Offset: 0x002FFD38
		private static void TurnOnFeatureControlKeys()
		{
			Version version = Environment.OSVersion.Version;
			if (version.Major == 5 && version.Minor == 2 && version.MajorRevision == 0)
			{
				return;
			}
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(0, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(1, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(2, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(3, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(4, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(5, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(6, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(7, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(8, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(9, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(10, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(11, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(12, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(13, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(14, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(15, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(16, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(17, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(18, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(20, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(22, 2, true);
			UnsafeNativeMethods.CoInternetSetFeatureEnabled(25, 2, true);
		}

		// Token: 0x060078A8 RID: 30888 RVA: 0x00300E40 File Offset: 0x002FFE40
		private void DoNavigate(Uri source, ref object targetFrameName, ref object postData, ref object headers, bool ignoreEscaping = false)
		{
			base.VerifyAccess();
			NativeMethods.IOleCommandTarget oleCommandTarget = (NativeMethods.IOleCommandTarget)this.AxIWebBrowser2;
			object obj = false;
			oleCommandTarget.Exec(null, 23, 0, new object[]
			{
				obj
			}, 0);
			this.LastNavigation = Guid.NewGuid();
			if (source == null)
			{
				this.NavigatingToAboutBlank = true;
				source = new Uri("about:blank");
			}
			else
			{
				this.CleanInternalState();
			}
			if (!source.IsAbsoluteUri)
			{
				throw new ArgumentException(SR.Get("AbsoluteUriOnly"), "source");
			}
			if (PackUriHelper.IsPackUri(source))
			{
				source = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(source);
			}
			object obj2 = null;
			object obj3 = ignoreEscaping ? source.AbsoluteUri : BindUriHelper.UriToString(source);
			try
			{
				this.AxIWebBrowser2.Navigate2(ref obj3, ref obj2, ref targetFrameName, ref postData, ref headers);
			}
			catch (COMException ex)
			{
				this.CleanInternalState();
				if (ex.ErrorCode != -2147023673)
				{
					throw;
				}
			}
		}

		// Token: 0x060078A9 RID: 30889 RVA: 0x00300F28 File Offset: 0x002FFF28
		private void SyncUIActiveState()
		{
			if (base.ActiveXState != ActiveXHelper.ActiveXState.UIActive && this.HasFocusWithinCore())
			{
				Invariant.Assert(base.ActiveXState == ActiveXHelper.ActiveXState.InPlaceActive);
				base.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
			}
		}

		// Token: 0x060078AA RID: 30890 RVA: 0x00300F50 File Offset: 0x002FFF50
		protected override bool TranslateAcceleratorCore(ref MSG msg, ModifierKeys modifiers)
		{
			this.SyncUIActiveState();
			Invariant.Assert(base.ActiveXState >= ActiveXHelper.ActiveXState.UIActive, "Should be at least UIActive when we are processing accelerator keys");
			return base.ActiveXInPlaceActiveObject.TranslateAccelerator(ref msg) == 0;
		}

		// Token: 0x060078AB RID: 30891 RVA: 0x00300F7D File Offset: 0x002FFF7D
		protected override bool TabIntoCore(TraversalRequest request)
		{
			Invariant.Assert(base.ActiveXState >= ActiveXHelper.ActiveXState.InPlaceActive, "Should be at least InPlaceActive when tabbed into");
			bool flag = base.DoVerb(-4);
			if (flag)
			{
				base.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
			}
			return flag;
		}

		// Token: 0x04003969 RID: 14697
		internal bool _canGoBack;

		// Token: 0x0400396A RID: 14698
		internal bool _canGoForward;

		// Token: 0x0400396B RID: 14699
		internal const string AboutBlankUriString = "about:blank";

		// Token: 0x0400396C RID: 14700
		private UnsafeNativeMethods.IWebBrowser2 _axIWebBrowser2;

		// Token: 0x0400396D RID: 14701
		private WebBrowser.WebOCHostingAdaptor _hostingAdaptor;

		// Token: 0x0400396E RID: 14702
		private ConnectionPointCookie _cookie;

		// Token: 0x0400396F RID: 14703
		private object _objectForScripting;

		// Token: 0x04003970 RID: 14704
		private Stream _documentStream;

		// Token: 0x04003971 RID: 14705
		private SecurityCriticalDataForSet<bool> _navigatingToAboutBlank;

		// Token: 0x04003972 RID: 14706
		private SecurityCriticalDataForSet<Guid> _lastNavigation;

		// Token: 0x02000C3F RID: 3135
		internal class WebOCHostingAdaptor
		{
			// Token: 0x0600914B RID: 37195 RVA: 0x00348EB8 File Offset: 0x00347EB8
			internal WebOCHostingAdaptor(WebBrowser webBrowser)
			{
				this._webBrowser = webBrowser;
			}

			// Token: 0x17001FCF RID: 8143
			// (get) Token: 0x0600914C RID: 37196 RVA: 0x00348EC7 File Offset: 0x00347EC7
			// (set) Token: 0x0600914D RID: 37197 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			internal virtual object ObjectForScripting
			{
				get
				{
					return this._webBrowser.ObjectForScripting;
				}
				set
				{
				}
			}

			// Token: 0x0600914E RID: 37198 RVA: 0x00348ED4 File Offset: 0x00347ED4
			internal virtual object CreateWebOC()
			{
				return Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("8856f961-340a-11d0-a96b-00c04fd705a2")));
			}

			// Token: 0x0600914F RID: 37199 RVA: 0x00348EEA File Offset: 0x00347EEA
			internal virtual object CreateEventSink()
			{
				return new WebBrowserEvent(this._webBrowser);
			}

			// Token: 0x04004C03 RID: 19459
			protected WebBrowser _webBrowser;
		}
	}
}
