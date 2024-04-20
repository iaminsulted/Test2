using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Annotations.Component;
using MS.Internal.Documents;
using MS.Internal.IO.Packaging;

namespace System.Windows.Documents
{
	// Token: 0x020005F0 RID: 1520
	[ContentProperty("Pages")]
	public class FixedDocument : FrameworkContentElement, IDocumentPaginatorSource, IAddChildInternal, IAddChild, IServiceProvider, IFixedNavigate, IUriContext
	{
		// Token: 0x06004A26 RID: 18982 RVA: 0x00231BA8 File Offset: 0x00230BA8
		static FixedDocument()
		{
			ContentElement.FocusableProperty.OverrideMetadata(typeof(FixedDocument), new FrameworkPropertyMetadata(true));
			NavigationService.NavigationServiceProperty.OverrideMetadata(typeof(FixedDocument), new FrameworkPropertyMetadata(new PropertyChangedCallback(FixedHyperLink.OnNavigationServiceChanged)));
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x00231C54 File Offset: 0x00230C54
		public FixedDocument()
		{
			this._Init();
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x00231C80 File Offset: 0x00230C80
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextContainer))
			{
				return this.FixedContainer;
			}
			if (serviceType == typeof(RubberbandSelector))
			{
				if (this._rubberbandSelector == null)
				{
					this._rubberbandSelector = new RubberbandSelector();
				}
				return this._rubberbandSelector;
			}
			return null;
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x00231CE8 File Offset: 0x00230CE8
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PageContent pageContent = value as PageContent;
			if (pageContent == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(PageContent)
				}), "value");
			}
			if (pageContent.IsInitialized)
			{
				this._pages.Add(pageContent);
				return;
			}
			if (this._partialPage == null)
			{
				this._partialPage = pageContent;
				this._partialPage.ChangeLogicalParent(this);
				this._partialPage.Initialized += this.OnPageLoaded;
				return;
			}
			throw new InvalidOperationException(SR.Get("PrevoiusPartialPageContentOutstanding"));
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06004A2B RID: 18987 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x06004A2C RID: 18988 RVA: 0x0022A4F5 File Offset: 0x002294F5
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

		// Token: 0x06004A2D RID: 18989 RVA: 0x00231D96 File Offset: 0x00230D96
		void IFixedNavigate.NavigateAsync(string elementID)
		{
			if (this.IsPageCountValid)
			{
				FixedHyperLink.NavigateToElement(this, elementID);
				return;
			}
			this._navigateAfterPagination = true;
			this._navigateFragment = elementID;
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x00231DB8 File Offset: 0x00230DB8
		UIElement IFixedNavigate.FindElementByID(string elementID, out FixedPage rootFixedPage)
		{
			UIElement uielement = null;
			rootFixedPage = null;
			if (char.IsDigit(elementID[0]))
			{
				int num = Convert.ToInt32(elementID, CultureInfo.InvariantCulture);
				num--;
				uielement = this.GetFixedPage(num);
				rootFixedPage = this.GetFixedPage(num);
			}
			else
			{
				PageContentCollection pages = this.Pages;
				int i = 0;
				int count = pages.Count;
				while (i < count)
				{
					PageContent pageContent = pages[i];
					if (pageContent.PageStream != null)
					{
						FixedPage fixedPage = this.GetFixedPage(i);
						if (fixedPage != null)
						{
							uielement = ((IFixedNavigate)fixedPage).FindElementByID(elementID, out rootFixedPage);
							if (uielement != null)
							{
								break;
							}
						}
					}
					else if (pageContent.ContainsID(elementID))
					{
						FixedPage fixedPage = this.GetFixedPage(i);
						if (fixedPage != null)
						{
							uielement = ((IFixedNavigate)fixedPage).FindElementByID(elementID, out rootFixedPage);
							if (uielement == null)
							{
								uielement = fixedPage;
								break;
							}
							break;
						}
					}
					i++;
				}
			}
			return uielement;
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06004A2F RID: 18991 RVA: 0x00231E73 File Offset: 0x00230E73
		// (set) Token: 0x06004A30 RID: 18992 RVA: 0x00231E85 File Offset: 0x00230E85
		internal NavigationService NavigationService
		{
			get
			{
				return (NavigationService)base.GetValue(NavigationService.NavigationServiceProperty);
			}
			set
			{
				base.SetValue(NavigationService.NavigationServiceProperty, value);
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06004A31 RID: 18993 RVA: 0x00231E93 File Offset: 0x00230E93
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return this.Pages.GetEnumerator();
			}
		}

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06004A32 RID: 18994 RVA: 0x00231EA0 File Offset: 0x00230EA0
		public DocumentPaginator DocumentPaginator
		{
			get
			{
				return this._paginator;
			}
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x00231EA8 File Offset: 0x00230EA8
		internal DocumentPage GetPage(int pageNumber)
		{
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (pageNumber >= this.Pages.Count)
			{
				return DocumentPage.Missing;
			}
			FixedPage fixedPage = this.SyncGetPage(pageNumber, false);
			if (fixedPage == null)
			{
				return DocumentPage.Missing;
			}
			Size size = this.ComputePageSize(fixedPage);
			DocumentPage result = new FixedDocumentPage(this, fixedPage, size, pageNumber);
			fixedPage.Measure(size);
			fixedPage.Arrange(new Rect(default(Point), size));
			return result;
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x00231F24 File Offset: 0x00230F24
		internal void GetPageAsync(int pageNumber, object userState)
		{
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (userState == null)
			{
				throw new ArgumentNullException("userState");
			}
			if (pageNumber < this.Pages.Count)
			{
				FixedDocument.GetPageAsyncRequest getPageAsyncRequest = new FixedDocument.GetPageAsyncRequest(this.Pages[pageNumber], pageNumber, userState);
				this._asyncOps[userState] = getPageAsyncRequest;
				DispatcherOperationCallback method = new DispatcherOperationCallback(this.GetPageAsyncDelegate);
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, method, getPageAsyncRequest);
				return;
			}
			this._NotifyGetPageAsyncCompleted(DocumentPage.Missing, pageNumber, null, false, userState);
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x00231FB4 File Offset: 0x00230FB4
		internal int GetPageNumber(ContentPosition contentPosition)
		{
			if (contentPosition == null)
			{
				throw new ArgumentNullException("contentPosition");
			}
			FixedTextPointer fixedTextPointer = contentPosition as FixedTextPointer;
			if (fixedTextPointer == null)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"));
			}
			return fixedTextPointer.FixedTextContainer.GetPageNumber(fixedTextPointer);
		}

		// Token: 0x06004A36 RID: 18998 RVA: 0x00231FF8 File Offset: 0x00230FF8
		internal void CancelAsync(object userState)
		{
			if (userState == null)
			{
				throw new ArgumentNullException("userState");
			}
			FixedDocument.GetPageAsyncRequest getPageAsyncRequest;
			if (this._asyncOps.TryGetValue(userState, out getPageAsyncRequest) && getPageAsyncRequest != null)
			{
				getPageAsyncRequest.Cancelled = true;
				getPageAsyncRequest.PageContent.GetPageRootAsyncCancel();
			}
		}

		// Token: 0x06004A37 RID: 18999 RVA: 0x00232038 File Offset: 0x00231038
		internal ContentPosition GetObjectPosition(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			DependencyObject dependencyObject = o as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException(SR.Get("FixedDocumentExpectsDependencyObject"));
			}
			FixedPage fixedPage = null;
			int num = -1;
			if (dependencyObject != this)
			{
				DependencyObject dependencyObject2 = dependencyObject;
				while (dependencyObject2 != null)
				{
					fixedPage = (dependencyObject2 as FixedPage);
					if (fixedPage != null)
					{
						num = this.GetIndexOfPage(fixedPage);
						if (num >= 0)
						{
							break;
						}
						dependencyObject2 = fixedPage.Parent;
					}
					else
					{
						dependencyObject2 = LogicalTreeHelper.GetParent(dependencyObject2);
					}
				}
			}
			else if (this.Pages.Count > 0)
			{
				num = 0;
			}
			FixedTextPointer fixedTextPointer = null;
			if (num >= 0)
			{
				FlowPosition flowPosition = null;
				Path path = dependencyObject as Path;
				if (dependencyObject is Glyphs || dependencyObject is Image || (path != null && path.Fill is ImageBrush))
				{
					FixedPosition fixedPosition = new FixedPosition(fixedPage.CreateFixedNode(num, (UIElement)dependencyObject), 0);
					flowPosition = this.FixedContainer.FixedTextBuilder.CreateFlowPosition(fixedPosition);
				}
				if (flowPosition == null)
				{
					flowPosition = this.FixedContainer.FixedTextBuilder.GetPageStartFlowPosition(num);
				}
				fixedTextPointer = new FixedTextPointer(true, LogicalDirection.Forward, flowPosition);
			}
			if (fixedTextPointer == null)
			{
				return ContentPosition.Missing;
			}
			return fixedTextPointer;
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x00232144 File Offset: 0x00231144
		internal ContentPosition GetPagePosition(DocumentPage page)
		{
			FixedDocumentPage fixedDocumentPage = page as FixedDocumentPage;
			if (fixedDocumentPage == null)
			{
				return ContentPosition.Missing;
			}
			return fixedDocumentPage.ContentPosition;
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06004A39 RID: 19001 RVA: 0x00232167 File Offset: 0x00231167
		internal bool IsPageCountValid
		{
			get
			{
				return base.IsInitialized;
			}
		}

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x06004A3A RID: 19002 RVA: 0x0023216F File Offset: 0x0023116F
		internal int PageCount
		{
			get
			{
				return this.Pages.Count;
			}
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x06004A3B RID: 19003 RVA: 0x0023217C File Offset: 0x0023117C
		// (set) Token: 0x06004A3C RID: 19004 RVA: 0x0023218F File Offset: 0x0023118F
		internal Size PageSize
		{
			get
			{
				return new Size(this._pageWidth, this._pageHeight);
			}
			set
			{
				this._pageWidth = value.Width;
				this._pageHeight = value.Height;
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x06004A3D RID: 19005 RVA: 0x002321AB File Offset: 0x002311AB
		internal bool HasExplicitStructure
		{
			get
			{
				return this._hasExplicitStructure;
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x06004A3E RID: 19006 RVA: 0x002321B3 File Offset: 0x002311B3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public PageContentCollection Pages
		{
			get
			{
				return this._pages;
			}
		}

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06004A3F RID: 19007 RVA: 0x002321BB File Offset: 0x002311BB
		// (set) Token: 0x06004A40 RID: 19008 RVA: 0x002321C8 File Offset: 0x002311C8
		public object PrintTicket
		{
			get
			{
				return base.GetValue(FixedDocument.PrintTicketProperty);
			}
			set
			{
				base.SetValue(FixedDocument.PrintTicketProperty, value);
			}
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x0022ECE4 File Offset: 0x0022DCE4
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentAutomationPeer(this);
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x002321D8 File Offset: 0x002311D8
		internal int GetIndexOfPage(FixedPage p)
		{
			PageContentCollection pages = this.Pages;
			int i = 0;
			int count = pages.Count;
			while (i < count)
			{
				if (pages[i].IsOwnerOf(p))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x00232211 File Offset: 0x00231211
		internal bool IsValidPageIndex(int index)
		{
			return index >= 0 && index < this.Pages.Count;
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x00232227 File Offset: 0x00231227
		internal FixedPage SyncGetPageWithCheck(int index)
		{
			if (this.IsValidPageIndex(index))
			{
				return this.SyncGetPage(index, false);
			}
			return null;
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x0023223C File Offset: 0x0023123C
		internal FixedPage SyncGetPage(int index, bool forceReload)
		{
			PageContentCollection pages = this.Pages;
			FixedPage pageRoot;
			try
			{
				pageRoot = pages[index].GetPageRoot(forceReload);
			}
			catch (Exception ex)
			{
				if (ex is InvalidOperationException || ex is ApplicationException)
				{
					throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, SR.Get("ExceptionInGetPage"), index), ex);
				}
				throw;
			}
			return pageRoot;
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x002322A8 File Offset: 0x002312A8
		internal void OnPageContentAppended(int index)
		{
			this.FixedContainer.FixedTextBuilder.AddVirtualPage();
			this._paginator.NotifyPaginationProgress(new PaginationProgressEventArgs(index, 1));
			if (base.IsInitialized)
			{
				this._paginator.NotifyPagesChanged(new PagesChangedEventArgs(index, 1));
			}
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x002322E6 File Offset: 0x002312E6
		internal void EnsurePageSize(FixedPage fp)
		{
			if (DoubleUtil.IsNaN(fp.Width))
			{
				fp.Width = this._pageWidth;
			}
			if (DoubleUtil.IsNaN(fp.Height))
			{
				fp.Height = this._pageHeight;
			}
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x0023231C File Offset: 0x0023131C
		internal bool GetPageSize(ref Size pageSize, int pageNumber)
		{
			if (pageNumber < this.Pages.Count)
			{
				FixedPage fp = null;
				if (!this._pendingPages.Contains(this.Pages[pageNumber]))
				{
					fp = this.SyncGetPage(pageNumber, false);
				}
				pageSize = this.ComputePageSize(fp);
				return true;
			}
			return false;
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x0023236B File Offset: 0x0023136B
		internal Size ComputePageSize(FixedPage fp)
		{
			if (fp == null)
			{
				return new Size(this._pageWidth, this._pageHeight);
			}
			this.EnsurePageSize(fp);
			return new Size(fp.Width, fp.Height);
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06004A4A RID: 19018 RVA: 0x0023239A File Offset: 0x0023139A
		internal FixedTextContainer FixedContainer
		{
			get
			{
				if (this._fixedTextContainer == null)
				{
					this._fixedTextContainer = new FixedTextContainer(this);
					this._fixedTextContainer.Highlights.Changed += this.OnHighlightChanged;
				}
				return this._fixedTextContainer;
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06004A4B RID: 19019 RVA: 0x002323D2 File Offset: 0x002313D2
		internal Dictionary<FixedPage, ArrayList> Highlights
		{
			get
			{
				return this._highlights;
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06004A4C RID: 19020 RVA: 0x002323DA File Offset: 0x002313DA
		// (set) Token: 0x06004A4D RID: 19021 RVA: 0x002323E2 File Offset: 0x002313E2
		internal DocumentReference DocumentReference
		{
			get
			{
				return this._documentReference;
			}
			set
			{
				this._documentReference = value;
			}
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x002323EC File Offset: 0x002313EC
		private void _Init()
		{
			this._paginator = new FixedDocumentPaginator(this);
			this._pages = new PageContentCollection(this);
			this._highlights = new Dictionary<FixedPage, ArrayList>();
			this._asyncOps = new Dictionary<object, FixedDocument.GetPageAsyncRequest>();
			this._pendingPages = new List<PageContent>();
			this._hasExplicitStructure = false;
			base.Initialized += this.OnInitialized;
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x0023244C File Offset: 0x0023144C
		private void OnInitialized(object sender, EventArgs e)
		{
			if (this._navigateAfterPagination)
			{
				FixedHyperLink.NavigateToElement(this, this._navigateFragment);
				this._navigateAfterPagination = false;
			}
			this.ValidateDocStructure();
			if (this.PageCount > 0)
			{
				DocumentPage page = this.GetPage(0);
				if (page != null)
				{
					FixedPage fixedPage = page.Visual as FixedPage;
					if (fixedPage != null)
					{
						base.Language = fixedPage.Language;
					}
				}
			}
			this._paginator.NotifyPaginationCompleted(e);
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x002324B8 File Offset: 0x002314B8
		internal void ValidateDocStructure()
		{
			Uri baseUri = BaseUriHelper.GetBaseUri(this);
			if (baseUri.Scheme.Equals(PackUriHelper.UriSchemePack, StringComparison.OrdinalIgnoreCase) && !baseUri.Host.Equals(BaseUriHelper.PackAppBaseUri.Host) && !baseUri.Host.Equals(BaseUriHelper.SiteOfOriginBaseUri.Host))
			{
				Uri structureUriFromRelationship = FixedDocument.GetStructureUriFromRelationship(baseUri, "http://schemas.microsoft.com/xps/2005/06/documentstructure");
				if (structureUriFromRelationship != null)
				{
					ContentType contentType;
					FixedDocument.ValidateAndLoadPartFromAbsoluteUri(structureUriFromRelationship, true, "DocumentStructure", out contentType);
					if (!FixedDocument._documentStructureContentType.AreTypeAndSubTypeEqual(contentType))
					{
						throw new FileFormatException(SR.Get("InvalidDSContentType"));
					}
					this._hasExplicitStructure = true;
				}
			}
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x00232558 File Offset: 0x00231558
		internal static StoryFragments GetStoryFragments(FixedPage fixedPage)
		{
			object obj = null;
			Uri baseUri = BaseUriHelper.GetBaseUri(fixedPage);
			if (baseUri.Scheme.Equals(PackUriHelper.UriSchemePack, StringComparison.OrdinalIgnoreCase) && !baseUri.Host.Equals(BaseUriHelper.PackAppBaseUri.Host) && !baseUri.Host.Equals(BaseUriHelper.SiteOfOriginBaseUri.Host))
			{
				Uri structureUriFromRelationship = FixedDocument.GetStructureUriFromRelationship(baseUri, "http://schemas.microsoft.com/xps/2005/06/storyfragments");
				if (structureUriFromRelationship != null)
				{
					ContentType contentType;
					obj = FixedDocument.ValidateAndLoadPartFromAbsoluteUri(structureUriFromRelationship, false, null, out contentType);
					if (!FixedDocument._storyFragmentsContentType.AreTypeAndSubTypeEqual(contentType))
					{
						throw new FileFormatException(SR.Get("InvalidSFContentType"));
					}
					if (!(obj is StoryFragments))
					{
						throw new FileFormatException(SR.Get("InvalidStoryFragmentsMarkup"));
					}
				}
			}
			return obj as StoryFragments;
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x00232610 File Offset: 0x00231610
		private static object ValidateAndLoadPartFromAbsoluteUri(Uri AbsoluteUriDoc, bool validateOnly, string rootElement, out ContentType mimeType)
		{
			mimeType = null;
			object result = null;
			try
			{
				Stream stream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(AbsoluteUriDoc, out mimeType);
				ParserContext parserContext = new ParserContext();
				parserContext.BaseUri = AbsoluteUriDoc;
				XpsValidatingLoader xpsValidatingLoader = new XpsValidatingLoader();
				if (validateOnly)
				{
					xpsValidatingLoader.Validate(stream, null, parserContext, mimeType, rootElement);
				}
				else
				{
					result = xpsValidatingLoader.Load(stream, null, parserContext, mimeType);
				}
			}
			catch (Exception ex)
			{
				if (!(ex is WebException) && !(ex is InvalidOperationException))
				{
					throw;
				}
			}
			return result;
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x00232688 File Offset: 0x00231688
		private static Uri GetStructureUriFromRelationship(Uri contentUri, string relationshipName)
		{
			Uri result = null;
			if (contentUri != null && relationshipName != null)
			{
				Uri partUri = PackUriHelper.GetPartUri(contentUri);
				if (partUri != null)
				{
					Uri packageUri = PackUriHelper.GetPackageUri(contentUri);
					Package package = PreloadedPackages.GetPackage(packageUri);
					if (package == null)
					{
						package = PackageStore.GetPackage(packageUri);
					}
					if (package != null)
					{
						PackageRelationshipCollection relationshipsByType = package.GetPart(partUri).GetRelationshipsByType(relationshipName);
						Uri uri = null;
						foreach (PackageRelationship packageRelationship in relationshipsByType)
						{
							uri = PackUriHelper.ResolvePartUri(partUri, packageRelationship.TargetUri);
						}
						if (uri != null)
						{
							result = PackUriHelper.Create(packageUri, uri);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x00232740 File Offset: 0x00231740
		private void OnPageLoaded(object sender, EventArgs e)
		{
			if ((PageContent)sender == this._partialPage)
			{
				this._partialPage.Initialized -= this.OnPageLoaded;
				this._pages.Add(this._partialPage);
				this._partialPage = null;
			}
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x00232780 File Offset: 0x00231780
		internal FixedPage GetFixedPage(int pageNumber)
		{
			FixedPage result = null;
			FixedDocumentPage fixedDocumentPage = this.GetPage(pageNumber) as FixedDocumentPage;
			if (fixedDocumentPage != null && fixedDocumentPage != DocumentPage.Missing)
			{
				result = fixedDocumentPage.FixedPage;
			}
			return result;
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x002327B0 File Offset: 0x002317B0
		private void OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			ITextContainer fixedContainer = this.FixedContainer;
			Highlights highlights = null;
			FixedDocumentSequence fixedDocumentSequence = base.Parent as FixedDocumentSequence;
			if (fixedDocumentSequence != null)
			{
				highlights = fixedDocumentSequence.TextContainer.Highlights;
			}
			else
			{
				highlights = this.FixedContainer.Highlights;
			}
			List<FixedPage> list = new List<FixedPage>();
			foreach (FixedPage item in this._highlights.Keys)
			{
				list.Add(item);
			}
			this._highlights.Clear();
			StaticTextPointer staticTextPointer = fixedContainer.CreateStaticPointerAtOffset(0);
			for (;;)
			{
				if (!highlights.IsContentHighlighted(staticTextPointer, LogicalDirection.Forward))
				{
					staticTextPointer = highlights.GetNextHighlightChangePosition(staticTextPointer, LogicalDirection.Forward);
					if (staticTextPointer.IsNull)
					{
						break;
					}
				}
				object highlightValue = highlights.GetHighlightValue(staticTextPointer, LogicalDirection.Forward, typeof(TextSelection));
				StaticTextPointer textPosition = staticTextPointer;
				FixedHighlightType fixedHighlightType = FixedHighlightType.None;
				Brush foregroundBrush = null;
				Brush backgroundBrush = null;
				if (highlightValue != DependencyProperty.UnsetValue)
				{
					do
					{
						staticTextPointer = highlights.GetNextHighlightChangePosition(staticTextPointer, LogicalDirection.Forward);
					}
					while (highlights.GetHighlightValue(staticTextPointer, LogicalDirection.Forward, typeof(TextSelection)) != DependencyProperty.UnsetValue);
					fixedHighlightType = FixedHighlightType.TextSelection;
					foregroundBrush = null;
					backgroundBrush = null;
				}
				else
				{
					AnnotationHighlightLayer.HighlightSegment highlightSegment = highlights.GetHighlightValue(textPosition, LogicalDirection.Forward, typeof(HighlightComponent)) as AnnotationHighlightLayer.HighlightSegment;
					if (highlightSegment != null)
					{
						staticTextPointer = highlights.GetNextHighlightChangePosition(staticTextPointer, LogicalDirection.Forward);
						fixedHighlightType = FixedHighlightType.AnnotationHighlight;
						backgroundBrush = highlightSegment.Fill;
					}
				}
				if (fixedHighlightType != FixedHighlightType.None)
				{
					this.FixedContainer.GetMultiHighlights((FixedTextPointer)textPosition.CreateDynamicTextPointer(LogicalDirection.Forward), (FixedTextPointer)staticTextPointer.CreateDynamicTextPointer(LogicalDirection.Forward), this._highlights, fixedHighlightType, foregroundBrush, backgroundBrush);
				}
			}
			ArrayList arrayList = new ArrayList();
			IList ranges = args.Ranges;
			for (int i = 0; i < ranges.Count; i++)
			{
				TextSegment textSegment = (TextSegment)ranges[i];
				int pageNumber = this.FixedContainer.GetPageNumber(textSegment.Start);
				int pageNumber2 = this.FixedContainer.GetPageNumber(textSegment.End);
				for (int j = pageNumber; j <= pageNumber2; j++)
				{
					if (arrayList.IndexOf(j) < 0)
					{
						arrayList.Add(j);
					}
				}
			}
			ICollection<FixedPage> keys = this._highlights.Keys;
			foreach (FixedPage fixedPage in list)
			{
				if (!keys.Contains(fixedPage))
				{
					int indexOfPage = this.GetIndexOfPage(fixedPage);
					if (indexOfPage >= 0 && indexOfPage < this.PageCount && arrayList.IndexOf(indexOfPage) < 0)
					{
						arrayList.Add(indexOfPage);
					}
				}
			}
			arrayList.Sort();
			foreach (object obj in arrayList)
			{
				int index = (int)obj;
				HighlightVisual highlightVisual = HighlightVisual.GetHighlightVisual(this.SyncGetPage(index, false));
				if (highlightVisual != null)
				{
					highlightVisual.InvalidateHighlights();
				}
			}
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x00232AB8 File Offset: 0x00231AB8
		private object GetPageAsyncDelegate(object arg)
		{
			FixedDocument.GetPageAsyncRequest getPageAsyncRequest = (FixedDocument.GetPageAsyncRequest)arg;
			PageContent pageContent = getPageAsyncRequest.PageContent;
			if (!this._pendingPages.Contains(pageContent))
			{
				this._pendingPages.Add(pageContent);
				pageContent.GetPageRootCompleted += this.OnGetPageRootCompleted;
				pageContent.GetPageRootAsync(false);
				if (getPageAsyncRequest.Cancelled)
				{
					pageContent.GetPageRootAsyncCancel();
				}
			}
			return null;
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x00232B18 File Offset: 0x00231B18
		private void OnGetPageRootCompleted(object sender, GetPageRootCompletedEventArgs args)
		{
			PageContent pageContent = (PageContent)sender;
			pageContent.GetPageRootCompleted -= this.OnGetPageRootCompleted;
			this._pendingPages.Remove(pageContent);
			ArrayList arrayList = new ArrayList();
			IEnumerator<KeyValuePair<object, FixedDocument.GetPageAsyncRequest>> enumerator = this._asyncOps.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<object, FixedDocument.GetPageAsyncRequest> keyValuePair = enumerator.Current;
					FixedDocument.GetPageAsyncRequest value = keyValuePair.Value;
					if (value.PageContent == pageContent)
					{
						ArrayList arrayList2 = arrayList;
						keyValuePair = enumerator.Current;
						arrayList2.Add(keyValuePair.Key);
						DocumentPage page = DocumentPage.Missing;
						if (!value.Cancelled && !args.Cancelled && args.Error == null)
						{
							FixedPage result = args.Result;
							Size fixedSize = this.ComputePageSize(result);
							page = new FixedDocumentPage(this, result, fixedSize, this.Pages.IndexOf(pageContent));
						}
						this._NotifyGetPageAsyncCompleted(page, value.PageNumber, args.Error, value.Cancelled, value.UserState);
					}
				}
			}
			finally
			{
				foreach (object key in arrayList)
				{
					this._asyncOps.Remove(key);
				}
			}
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x00232C68 File Offset: 0x00231C68
		private void _NotifyGetPageAsyncCompleted(DocumentPage page, int pageNumber, Exception error, bool cancelled, object userState)
		{
			this._paginator.NotifyGetPageCompleted(new GetPageCompletedEventArgs(page, pageNumber, error, cancelled, userState));
		}

		// Token: 0x040026DF RID: 9951
		public static readonly DependencyProperty PrintTicketProperty = DependencyProperty.RegisterAttached("PrintTicket", typeof(object), typeof(FixedDocument), new FrameworkPropertyMetadata(null));

		// Token: 0x040026E0 RID: 9952
		private IDictionary<object, FixedDocument.GetPageAsyncRequest> _asyncOps;

		// Token: 0x040026E1 RID: 9953
		private IList<PageContent> _pendingPages;

		// Token: 0x040026E2 RID: 9954
		private PageContentCollection _pages;

		// Token: 0x040026E3 RID: 9955
		private PageContent _partialPage;

		// Token: 0x040026E4 RID: 9956
		private Dictionary<FixedPage, ArrayList> _highlights;

		// Token: 0x040026E5 RID: 9957
		private double _pageWidth = 816.0;

		// Token: 0x040026E6 RID: 9958
		private double _pageHeight = 1056.0;

		// Token: 0x040026E7 RID: 9959
		private FixedTextContainer _fixedTextContainer;

		// Token: 0x040026E8 RID: 9960
		private RubberbandSelector _rubberbandSelector;

		// Token: 0x040026E9 RID: 9961
		private bool _navigateAfterPagination;

		// Token: 0x040026EA RID: 9962
		private string _navigateFragment;

		// Token: 0x040026EB RID: 9963
		private FixedDocumentPaginator _paginator;

		// Token: 0x040026EC RID: 9964
		private DocumentReference _documentReference;

		// Token: 0x040026ED RID: 9965
		private bool _hasExplicitStructure;

		// Token: 0x040026EE RID: 9966
		private const string _structureRelationshipName = "http://schemas.microsoft.com/xps/2005/06/documentstructure";

		// Token: 0x040026EF RID: 9967
		private const string _storyFragmentsRelationshipName = "http://schemas.microsoft.com/xps/2005/06/storyfragments";

		// Token: 0x040026F0 RID: 9968
		private static readonly ContentType _storyFragmentsContentType = new ContentType("application/vnd.ms-package.xps-storyfragments+xml");

		// Token: 0x040026F1 RID: 9969
		private static readonly ContentType _documentStructureContentType = new ContentType("application/vnd.ms-package.xps-documentstructure+xml");

		// Token: 0x040026F2 RID: 9970
		private static DependencyObjectType UIElementType = DependencyObjectType.FromSystemTypeInternal(typeof(UIElement));

		// Token: 0x02000B32 RID: 2866
		private class GetPageAsyncRequest
		{
			// Token: 0x06008C9F RID: 35999 RVA: 0x0033D388 File Offset: 0x0033C388
			internal GetPageAsyncRequest(PageContent pageContent, int pageNumber, object userState)
			{
				this.PageContent = pageContent;
				this.PageNumber = pageNumber;
				this.UserState = userState;
				this.Cancelled = false;
			}

			// Token: 0x040047FE RID: 18430
			internal PageContent PageContent;

			// Token: 0x040047FF RID: 18431
			internal int PageNumber;

			// Token: 0x04004800 RID: 18432
			internal object UserState;

			// Token: 0x04004801 RID: 18433
			internal bool Cancelled;
		}
	}
}
