using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Utility;
using MS.Utility;

namespace System.Windows.Documents
{
	// Token: 0x02000649 RID: 1609
	[ContentProperty("Child")]
	public sealed class PageContent : FrameworkElement, IAddChildInternal, IAddChild, IUriContext
	{
		// Token: 0x06004FBF RID: 20415 RVA: 0x002446CC File Offset: 0x002436CC
		public PageContent()
		{
			this._Init();
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x002446DC File Offset: 0x002436DC
		public FixedPage GetPageRoot(bool forceReload)
		{
			if (this._asyncOp != null)
			{
				this._asyncOp.Wait();
			}
			FixedPage fixedPage = null;
			if (!forceReload)
			{
				fixedPage = this._GetLoadedPage();
			}
			if (fixedPage == null)
			{
				fixedPage = this._LoadPage();
			}
			return fixedPage;
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x00244714 File Offset: 0x00243714
		public void GetPageRootAsync(bool forceReload)
		{
			if (this._asyncOp != null)
			{
				return;
			}
			FixedPage fixedPage = null;
			if (!forceReload)
			{
				fixedPage = this._GetLoadedPage();
			}
			if (fixedPage != null)
			{
				this._NotifyPageCompleted(fixedPage, null, false, null);
				return;
			}
			Dispatcher dispatcher = base.Dispatcher;
			Uri uri = this._ResolveUri();
			if (uri != null || this._child != null)
			{
				this._asyncOp = new PageContentAsyncResult(new AsyncCallback(this._RequestPageCallback), null, dispatcher, uri, uri, this._child);
				this._asyncOp.DispatcherOperation = dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this._asyncOp.Dispatch), null);
			}
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x002447A9 File Offset: 0x002437A9
		public void GetPageRootAsyncCancel()
		{
			if (this._asyncOp != null)
			{
				this._asyncOp.Cancel();
				this._asyncOp = null;
			}
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x002447C8 File Offset: 0x002437C8
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			FixedPage fixedPage = value as FixedPage;
			if (fixedPage == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(FixedPage)
				}), "value");
			}
			if (this._child != null)
			{
				throw new InvalidOperationException(SR.Get("CanOnlyHaveOneChild", new object[]
				{
					typeof(PageContent),
					value
				}));
			}
			this._pageRef = null;
			this._child = fixedPage;
			LogicalTreeHelper.AddLogicalChild(this, this._child);
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x00244867 File Offset: 0x00243867
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((PageContent)d)._pageRef = null;
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06004FC6 RID: 20422 RVA: 0x00244875 File Offset: 0x00243875
		// (set) Token: 0x06004FC7 RID: 20423 RVA: 0x00244887 File Offset: 0x00243887
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(PageContent.SourceProperty);
			}
			set
			{
				base.SetValue(PageContent.SourceProperty, value);
			}
		}

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06004FC8 RID: 20424 RVA: 0x00244895 File Offset: 0x00243895
		public LinkTargetCollection LinkTargets
		{
			get
			{
				if (this._linkTargets == null)
				{
					this._linkTargets = new LinkTargetCollection();
				}
				return this._linkTargets;
			}
		}

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06004FC9 RID: 20425 RVA: 0x002448B0 File Offset: 0x002438B0
		// (set) Token: 0x06004FCA RID: 20426 RVA: 0x002448B8 File Offset: 0x002438B8
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public FixedPage Child
		{
			get
			{
				return this._child;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._child != null)
				{
					throw new InvalidOperationException(SR.Get("CanOnlyHaveOneChild", new object[]
					{
						typeof(PageContent),
						value
					}));
				}
				this._pageRef = null;
				this._child = value;
				LogicalTreeHelper.AddLogicalChild(this, this._child);
			}
		}

		// Token: 0x06004FCB RID: 20427 RVA: 0x0024491C File Offset: 0x0024391C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeChild(XamlDesignerSerializationManager manager)
		{
			bool result = false;
			if (manager != null)
			{
				result = (manager.XmlWriter == null);
			}
			return result;
		}

		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06004FCC RID: 20428 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x06004FCD RID: 20429 RVA: 0x0022A4F5 File Offset: 0x002294F5
		Uri IUriContext.BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06004FCE RID: 20430 RVA: 0x0024493C File Offset: 0x0024393C
		// (remove) Token: 0x06004FCF RID: 20431 RVA: 0x00244974 File Offset: 0x00243974
		public event GetPageRootCompletedEventHandler GetPageRootCompleted;

		// Token: 0x06004FD0 RID: 20432 RVA: 0x002449A9 File Offset: 0x002439A9
		internal bool IsOwnerOf(FixedPage pageVisual)
		{
			if (pageVisual == null)
			{
				throw new ArgumentNullException("pageVisual");
			}
			return this._child == pageVisual || (this._pageRef != null && (FixedPage)this._pageRef.Target == pageVisual);
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x002449E4 File Offset: 0x002439E4
		internal Stream GetPageStream()
		{
			Uri uri = this._ResolveUri();
			Stream stream = null;
			if (uri != null)
			{
				stream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(uri);
				if (stream == null)
				{
					throw new ApplicationException(SR.Get("PageContentNotFound"));
				}
			}
			return stream;
		}

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06004FD2 RID: 20434 RVA: 0x002448B0 File Offset: 0x002438B0
		internal FixedPage PageStream
		{
			get
			{
				return this._child;
			}
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x00244A20 File Offset: 0x00243A20
		internal bool ContainsID(string elementID)
		{
			bool result = false;
			foreach (object obj in this.LinkTargets)
			{
				LinkTarget linkTarget = (LinkTarget)obj;
				if (elementID.Equals(linkTarget.Name))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06004FD4 RID: 20436 RVA: 0x00244A88 File Offset: 0x00243A88
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				FixedPage fixedPage = this._child;
				if (fixedPage == null)
				{
					fixedPage = this._GetLoadedPage();
				}
				FixedPage[] array;
				if (fixedPage == null)
				{
					array = Array.Empty<FixedPage>();
				}
				else
				{
					array = new FixedPage[]
					{
						fixedPage
					};
				}
				return array.GetEnumerator();
			}
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x00244AC2 File Offset: 0x00243AC2
		private void _Init()
		{
			base.InheritanceBehavior = InheritanceBehavior.SkipToAppNow;
			this._pendingStreams = new HybridDictionary();
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x00244AD8 File Offset: 0x00243AD8
		private void _NotifyPageCompleted(FixedPage result, Exception error, bool cancelled, object userToken)
		{
			if (this.GetPageRootCompleted != null)
			{
				GetPageRootCompletedEventArgs e = new GetPageRootCompletedEventArgs(result, error, cancelled, userToken);
				this.GetPageRootCompleted(this, e);
			}
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x00244B08 File Offset: 0x00243B08
		private Uri _ResolveUri()
		{
			Uri uri = this.Source;
			if (uri != null)
			{
				uri = BindUriHelper.GetUriToNavigate(this, ((IUriContext)this).BaseUri, uri);
			}
			return uri;
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x00244B34 File Offset: 0x00243B34
		private void _RequestPageCallback(IAsyncResult ar)
		{
			PageContentAsyncResult pageContentAsyncResult = (PageContentAsyncResult)ar;
			if (pageContentAsyncResult == this._asyncOp && pageContentAsyncResult.Result != null)
			{
				LogicalTreeHelper.AddLogicalChild(this, pageContentAsyncResult.Result);
				this._pageRef = new WeakReference(pageContentAsyncResult.Result);
			}
			this._asyncOp = null;
			this._NotifyPageCompleted(pageContentAsyncResult.Result, pageContentAsyncResult.Exception, pageContentAsyncResult.IsCancelled, pageContentAsyncResult.AsyncState);
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x00244B9C File Offset: 0x00243B9C
		private FixedPage _LoadPage()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXGetPageBegin);
			FixedPage fixedPage = null;
			try
			{
				if (this._child != null)
				{
					fixedPage = this._child;
				}
				else
				{
					Uri uri = this._ResolveUri();
					if (uri != null)
					{
						Stream stream;
						PageContent._LoadPageImpl(((IUriContext)this).BaseUri, uri, out fixedPage, out stream);
						if (fixedPage == null || fixedPage.IsInitialized)
						{
							stream.Close();
						}
						else
						{
							this._pendingStreams.Add(fixedPage, stream);
							fixedPage.Initialized += this._OnPaserFinished;
						}
					}
				}
				if (fixedPage != null)
				{
					LogicalTreeHelper.AddLogicalChild(this, fixedPage);
					this._pageRef = new WeakReference(fixedPage);
				}
				else
				{
					this._pageRef = null;
				}
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXGetPageEnd);
			}
			return fixedPage;
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x00244C5C File Offset: 0x00243C5C
		private FixedPage _GetLoadedPage()
		{
			FixedPage result = null;
			if (this._pageRef != null)
			{
				result = (FixedPage)this._pageRef.Target;
			}
			return result;
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x00244C85 File Offset: 0x00243C85
		private void _OnPaserFinished(object sender, EventArgs args)
		{
			if (this._pendingStreams.Contains(sender))
			{
				((Stream)this._pendingStreams[sender]).Close();
				this._pendingStreams.Remove(sender);
			}
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x00244CB8 File Offset: 0x00243CB8
		internal static void _LoadPageImpl(Uri baseUri, Uri uriToLoad, out FixedPage fixedPage, out Stream pageStream)
		{
			ContentType contentType;
			pageStream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(uriToLoad, out contentType);
			if (pageStream == null)
			{
				throw new ApplicationException(SR.Get("PageContentNotFound"));
			}
			ParserContext parserContext = new ParserContext();
			parserContext.BaseUri = uriToLoad;
			object obj;
			if (BindUriHelper.IsXamlMimeType(contentType))
			{
				obj = new XpsValidatingLoader().Load(pageStream, baseUri, parserContext, contentType);
			}
			else
			{
				if (!MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
				{
					throw new ApplicationException(SR.Get("PageContentUnsupportedMimeType"));
				}
				obj = XamlReader.LoadBaml(pageStream, parserContext, null, true);
			}
			if (obj != null && !(obj is FixedPage))
			{
				throw new ApplicationException(SR.Get("PageContentUnsupportedPageType", new object[]
				{
					obj.GetType()
				}));
			}
			fixedPage = (FixedPage)obj;
		}

		// Token: 0x04002862 RID: 10338
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(PageContent), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PageContent.OnSourceChanged)));

		// Token: 0x04002864 RID: 10340
		private WeakReference _pageRef;

		// Token: 0x04002865 RID: 10341
		private FixedPage _child;

		// Token: 0x04002866 RID: 10342
		private PageContentAsyncResult _asyncOp;

		// Token: 0x04002867 RID: 10343
		private HybridDictionary _pendingStreams;

		// Token: 0x04002868 RID: 10344
		private LinkTargetCollection _linkTargets;
	}
}
