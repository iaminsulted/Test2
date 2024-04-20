using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020005E6 RID: 1510
	[ContentProperty("References")]
	public class FixedDocumentSequence : FrameworkContentElement, IDocumentPaginatorSource, IAddChildInternal, IAddChild, IServiceProvider, IFixedNavigate, IUriContext
	{
		// Token: 0x060048E3 RID: 18659 RVA: 0x0022E6B4 File Offset: 0x0022D6B4
		static FixedDocumentSequence()
		{
			ContentElement.FocusableProperty.OverrideMetadata(typeof(FixedDocumentSequence), new FrameworkPropertyMetadata(true));
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x0022E709 File Offset: 0x0022D709
		public FixedDocumentSequence()
		{
			this._Init();
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x0022E718 File Offset: 0x0022D718
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextContainer))
			{
				return this.TextContainer;
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

		// Token: 0x060048E6 RID: 18662 RVA: 0x0022E780 File Offset: 0x0022D780
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			DocumentReference documentReference = value as DocumentReference;
			if (documentReference == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(DocumentReference)
				}), "value");
			}
			if (documentReference.IsInitialized)
			{
				this._references.Add(documentReference);
				return;
			}
			if (this._partialRef == null)
			{
				this._partialRef = documentReference;
				this._partialRef.Initialized += this._OnDocumentReferenceInitialized;
				return;
			}
			throw new InvalidOperationException(SR.Get("PrevoiusUninitializedDocumentReferenceOutstanding"));
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x0022E821 File Offset: 0x0022D821
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

		// Token: 0x060048E9 RID: 18665 RVA: 0x0022E844 File Offset: 0x0022D844
		UIElement IFixedNavigate.FindElementByID(string elementID, out FixedPage rootFixedPage)
		{
			UIElement uielement = null;
			rootFixedPage = null;
			if (char.IsDigit(elementID[0]))
			{
				int pageNumber = Convert.ToInt32(elementID, CultureInfo.InvariantCulture) - 1;
				DynamicDocumentPaginator paginator;
				int pageNumber2;
				if (this.TranslatePageNumber(pageNumber, out paginator, out pageNumber2))
				{
					FixedDocument fixedDocument = paginator.Source as FixedDocument;
					if (fixedDocument != null)
					{
						uielement = fixedDocument.GetFixedPage(pageNumber2);
					}
				}
			}
			else
			{
				foreach (DocumentReference docRef in this.References)
				{
					DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
					FixedDocument fixedDocument = paginator.Source as FixedDocument;
					if (fixedDocument != null)
					{
						uielement = ((IFixedNavigate)fixedDocument).FindElementByID(elementID, out rootFixedPage);
						if (uielement != null)
						{
							break;
						}
					}
				}
			}
			return uielement;
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x060048EA RID: 18666 RVA: 0x0022E900 File Offset: 0x0022D900
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				DocumentReference[] array = new DocumentReference[this._references.Count];
				this._references.CopyTo(array, 0);
				return array.GetEnumerator();
			}
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x060048EB RID: 18667 RVA: 0x0022E931 File Offset: 0x0022D931
		public DocumentPaginator DocumentPaginator
		{
			get
			{
				return this._paginator;
			}
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x0022E93C File Offset: 0x0022D93C
		internal DocumentPage GetPage(int pageNumber)
		{
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			DynamicDocumentPaginator dynamicDocumentPaginator;
			int pageNumber2;
			if (this.TranslatePageNumber(pageNumber, out dynamicDocumentPaginator, out pageNumber2))
			{
				DocumentPage page = dynamicDocumentPaginator.GetPage(pageNumber2);
				return new FixedDocumentSequenceDocumentPage(this, dynamicDocumentPaginator, page);
			}
			return DocumentPage.Missing;
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x0022E988 File Offset: 0x0022D988
		internal DocumentPage GetPage(FixedDocument document, int fixedDocPageNumber)
		{
			if (fixedDocPageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("fixedDocPageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			DocumentPage page = document.GetPage(fixedDocPageNumber);
			return new FixedDocumentSequenceDocumentPage(this, document.DocumentPaginator as DynamicDocumentPaginator, page);
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x0022E9D8 File Offset: 0x0022D9D8
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
			FixedDocumentSequence.GetPageAsyncRequest getPageAsyncRequest = new FixedDocumentSequence.GetPageAsyncRequest(new FixedDocumentSequence.RequestedPage(pageNumber), userState);
			this._asyncOps[userState] = getPageAsyncRequest;
			DispatcherOperationCallback method = new DispatcherOperationCallback(this._GetPageAsyncDelegate);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Background, method, getPageAsyncRequest);
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x0022EA44 File Offset: 0x0022DA44
		internal int GetPageNumber(ContentPosition contentPosition)
		{
			if (contentPosition == null)
			{
				throw new ArgumentNullException("contentPosition");
			}
			DynamicDocumentPaginator dynamicDocumentPaginator = null;
			ContentPosition contentPosition2 = null;
			if (contentPosition is DocumentSequenceTextPointer)
			{
				DocumentSequenceTextPointer documentSequenceTextPointer = (DocumentSequenceTextPointer)contentPosition;
				dynamicDocumentPaginator = this.GetPaginator(documentSequenceTextPointer.ChildBlock.DocRef);
				contentPosition2 = (documentSequenceTextPointer.ChildPointer as ContentPosition);
			}
			if (contentPosition2 == null)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"));
			}
			int pageNumber = dynamicDocumentPaginator.GetPageNumber(contentPosition2);
			int result;
			this._SynthesizeGlobalPageNumber(dynamicDocumentPaginator, pageNumber, out result);
			return result;
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x0022EABC File Offset: 0x0022DABC
		internal void CancelAsync(object userState)
		{
			if (userState == null)
			{
				throw new ArgumentNullException("userState");
			}
			if (this._asyncOps.ContainsKey(userState))
			{
				FixedDocumentSequence.GetPageAsyncRequest getPageAsyncRequest = this._asyncOps[userState];
				if (getPageAsyncRequest != null)
				{
					getPageAsyncRequest.Cancelled = true;
					if (getPageAsyncRequest.Page.ChildPaginator != null)
					{
						getPageAsyncRequest.Page.ChildPaginator.CancelAsync(getPageAsyncRequest);
					}
				}
			}
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x0022EB1C File Offset: 0x0022DB1C
		internal ContentPosition GetObjectPosition(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			foreach (DocumentReference docRef in this.References)
			{
				DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
				if (paginator != null)
				{
					ContentPosition objectPosition = paginator.GetObjectPosition(o);
					if (objectPosition != ContentPosition.Missing && objectPosition is ITextPointer)
					{
						return new DocumentSequenceTextPointer(new ChildDocumentBlock(this.TextContainer, docRef), (ITextPointer)objectPosition);
					}
				}
			}
			return ContentPosition.Missing;
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x0022EBB8 File Offset: 0x0022DBB8
		internal ContentPosition GetPagePosition(DocumentPage page)
		{
			FixedDocumentSequenceDocumentPage fixedDocumentSequenceDocumentPage = page as FixedDocumentSequenceDocumentPage;
			if (fixedDocumentSequenceDocumentPage == null)
			{
				return ContentPosition.Missing;
			}
			return fixedDocumentSequenceDocumentPage.ContentPosition;
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x060048F3 RID: 18675 RVA: 0x0022EBDC File Offset: 0x0022DBDC
		internal bool IsPageCountValid
		{
			get
			{
				bool result = true;
				if (base.IsInitialized)
				{
					using (IEnumerator<DocumentReference> enumerator = this.References.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DocumentReference docRef = enumerator.Current;
							DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
							if (paginator == null || !paginator.IsPageCountValid)
							{
								result = false;
								break;
							}
						}
						return result;
					}
				}
				result = false;
				return result;
			}
		}

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x060048F4 RID: 18676 RVA: 0x0022EC48 File Offset: 0x0022DC48
		internal int PageCount
		{
			get
			{
				int num = 0;
				foreach (DocumentReference docRef in this.References)
				{
					DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
					if (paginator != null)
					{
						num += paginator.PageCount;
						if (!paginator.IsPageCountValid)
						{
							break;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x060048F5 RID: 18677 RVA: 0x0022ECB0 File Offset: 0x0022DCB0
		// (set) Token: 0x060048F6 RID: 18678 RVA: 0x0022ECB8 File Offset: 0x0022DCB8
		internal Size PageSize
		{
			get
			{
				return this._pageSize;
			}
			set
			{
				this._pageSize = value;
			}
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x060048F7 RID: 18679 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x060048F8 RID: 18680 RVA: 0x0022A4F5 File Offset: 0x002294F5
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

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x060048F9 RID: 18681 RVA: 0x0022ECC1 File Offset: 0x0022DCC1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[CLSCompliant(false)]
		public DocumentReferenceCollection References
		{
			get
			{
				return this._references;
			}
		}

		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x060048FA RID: 18682 RVA: 0x0022ECC9 File Offset: 0x0022DCC9
		// (set) Token: 0x060048FB RID: 18683 RVA: 0x0022ECD6 File Offset: 0x0022DCD6
		public object PrintTicket
		{
			get
			{
				return base.GetValue(FixedDocumentSequence.PrintTicketProperty);
			}
			set
			{
				base.SetValue(FixedDocumentSequence.PrintTicketProperty, value);
			}
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x0022ECE4 File Offset: 0x0022DCE4
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentAutomationPeer(this);
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x0022ECEC File Offset: 0x0022DCEC
		internal DynamicDocumentPaginator GetPaginator(DocumentReference docRef)
		{
			DynamicDocumentPaginator dynamicDocumentPaginator = null;
			IDocumentPaginatorSource documentPaginatorSource = docRef.CurrentlyLoadedDoc;
			if (documentPaginatorSource != null)
			{
				dynamicDocumentPaginator = (documentPaginatorSource.DocumentPaginator as DynamicDocumentPaginator);
			}
			else
			{
				documentPaginatorSource = docRef.GetDocument(false);
				if (documentPaginatorSource != null)
				{
					dynamicDocumentPaginator = (documentPaginatorSource.DocumentPaginator as DynamicDocumentPaginator);
					dynamicDocumentPaginator.PaginationCompleted += this._OnChildPaginationCompleted;
					dynamicDocumentPaginator.PaginationProgress += this._OnChildPaginationProgress;
					dynamicDocumentPaginator.PagesChanged += this._OnChildPagesChanged;
				}
			}
			return dynamicDocumentPaginator;
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x0022ED64 File Offset: 0x0022DD64
		internal bool TranslatePageNumber(int pageNumber, out DynamicDocumentPaginator childPaginator, out int childPageNumber)
		{
			childPaginator = null;
			childPageNumber = 0;
			foreach (DocumentReference docRef in this.References)
			{
				childPaginator = this.GetPaginator(docRef);
				if (childPaginator != null)
				{
					childPageNumber = pageNumber;
					if (childPaginator.PageCount > childPageNumber)
					{
						return true;
					}
					if (!childPaginator.IsPageCountValid)
					{
						break;
					}
					pageNumber -= childPaginator.PageCount;
				}
			}
			return false;
		}

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x060048FF RID: 18687 RVA: 0x0022EDE8 File Offset: 0x0022DDE8
		internal DocumentSequenceTextContainer TextContainer
		{
			get
			{
				if (this._textContainer == null)
				{
					this._textContainer = new DocumentSequenceTextContainer(this);
				}
				return this._textContainer;
			}
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x0022EE04 File Offset: 0x0022DE04
		private void _Init()
		{
			this._paginator = new FixedDocumentSequencePaginator(this);
			this._references = new DocumentReferenceCollection();
			this._references.CollectionChanged += this._OnCollectionChanged;
			this._asyncOps = new Dictionary<object, FixedDocumentSequence.GetPageAsyncRequest>();
			this._pendingPages = new List<FixedDocumentSequence.RequestedPage>();
			this._pageSize = new Size(816.0, 1056.0);
			base.Initialized += this.OnInitialized;
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x0022EE84 File Offset: 0x0022DE84
		private void OnInitialized(object sender, EventArgs e)
		{
			bool flag = true;
			foreach (DocumentReference docRef in this.References)
			{
				DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
				if (paginator == null || !paginator.IsPageCountValid)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this._paginator.NotifyPaginationCompleted(EventArgs.Empty);
			}
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
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x0022EF2C File Offset: 0x0022DF2C
		private void _OnDocumentReferenceInitialized(object sender, EventArgs e)
		{
			DocumentReference documentReference = (DocumentReference)sender;
			if (documentReference == this._partialRef)
			{
				this._partialRef.Initialized -= this._OnDocumentReferenceInitialized;
				this._partialRef = null;
				this._references.Add(documentReference);
			}
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x0022EF74 File Offset: 0x0022DF74
		private void _OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.Action != NotifyCollectionChangedAction.Add)
			{
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			if (args.NewItems.Count != 1)
			{
				throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
			}
			object obj = args.NewItems[0];
			base.AddLogicalChild(obj);
			int pageCount = this.PageCount;
			DynamicDocumentPaginator paginator = this.GetPaginator((DocumentReference)obj);
			if (paginator == null)
			{
				throw new ApplicationException(SR.Get("DocumentReferenceHasInvalidDocument"));
			}
			int pageCount2 = paginator.PageCount;
			int start = pageCount - pageCount2;
			if (pageCount2 > 0)
			{
				this._paginator.NotifyPaginationProgress(new PaginationProgressEventArgs(start, pageCount2));
				this._paginator.NotifyPagesChanged(new PagesChangedEventArgs(start, pageCount2));
				return;
			}
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x0022F03C File Offset: 0x0022E03C
		private bool _SynthesizeGlobalPageNumber(DynamicDocumentPaginator childPaginator, int childPageNumber, out int pageNumber)
		{
			pageNumber = 0;
			foreach (DocumentReference docRef in this.References)
			{
				DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
				if (paginator != null)
				{
					if (paginator == childPaginator)
					{
						pageNumber += childPageNumber;
						return true;
					}
					pageNumber += paginator.PageCount;
				}
			}
			return false;
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x0022F0AC File Offset: 0x0022E0AC
		private void _OnChildPaginationCompleted(object sender, EventArgs args)
		{
			if (this.IsPageCountValid)
			{
				this._paginator.NotifyPaginationCompleted(EventArgs.Empty);
				if (this._navigateAfterPagination)
				{
					FixedHyperLink.NavigateToElement(this, this._navigateFragment);
					this._navigateAfterPagination = false;
				}
			}
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x0022F0E4 File Offset: 0x0022E0E4
		private void _OnChildPaginationProgress(object sender, PaginationProgressEventArgs args)
		{
			int start;
			if (this._SynthesizeGlobalPageNumber((DynamicDocumentPaginator)sender, args.Start, out start))
			{
				this._paginator.NotifyPaginationProgress(new PaginationProgressEventArgs(start, args.Count));
			}
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x0022F120 File Offset: 0x0022E120
		private void _OnChildPagesChanged(object sender, PagesChangedEventArgs args)
		{
			int start;
			if (this._SynthesizeGlobalPageNumber((DynamicDocumentPaginator)sender, args.Start, out start))
			{
				this._paginator.NotifyPagesChanged(new PagesChangedEventArgs(start, args.Count));
				return;
			}
			this._paginator.NotifyPagesChanged(new PagesChangedEventArgs(this.PageCount, int.MaxValue));
		}

		// Token: 0x06004908 RID: 18696 RVA: 0x0022F178 File Offset: 0x0022E178
		private object _GetPageAsyncDelegate(object arg)
		{
			FixedDocumentSequence.GetPageAsyncRequest getPageAsyncRequest = (FixedDocumentSequence.GetPageAsyncRequest)arg;
			int pageNumber = getPageAsyncRequest.Page.PageNumber;
			if (getPageAsyncRequest.Cancelled || !this.TranslatePageNumber(pageNumber, out getPageAsyncRequest.Page.ChildPaginator, out getPageAsyncRequest.Page.ChildPageNumber) || getPageAsyncRequest.Cancelled)
			{
				this._NotifyGetPageAsyncCompleted(DocumentPage.Missing, pageNumber, null, true, getPageAsyncRequest.UserState);
				this._asyncOps.Remove(getPageAsyncRequest.UserState);
				return null;
			}
			if (!this._pendingPages.Contains(getPageAsyncRequest.Page))
			{
				this._pendingPages.Add(getPageAsyncRequest.Page);
				getPageAsyncRequest.Page.ChildPaginator.GetPageCompleted += this._OnGetPageCompleted;
				getPageAsyncRequest.Page.ChildPaginator.GetPageAsync(getPageAsyncRequest.Page.ChildPageNumber, getPageAsyncRequest);
			}
			return null;
		}

		// Token: 0x06004909 RID: 18697 RVA: 0x0022F24C File Offset: 0x0022E24C
		private void _OnGetPageCompleted(object sender, GetPageCompletedEventArgs args)
		{
			FixedDocumentSequence.GetPageAsyncRequest getPageAsyncRequest = (FixedDocumentSequence.GetPageAsyncRequest)args.UserState;
			this._pendingPages.Remove(getPageAsyncRequest.Page);
			DocumentPage page = DocumentPage.Missing;
			int pageNumber = getPageAsyncRequest.Page.PageNumber;
			if (!args.Cancelled && args.Error == null && args.DocumentPage != DocumentPage.Missing)
			{
				page = new FixedDocumentSequenceDocumentPage(this, (DynamicDocumentPaginator)sender, args.DocumentPage);
				this._SynthesizeGlobalPageNumber((DynamicDocumentPaginator)sender, args.PageNumber, out pageNumber);
			}
			if (!args.Cancelled)
			{
				ArrayList arrayList = new ArrayList();
				IEnumerator<KeyValuePair<object, FixedDocumentSequence.GetPageAsyncRequest>> enumerator = this._asyncOps.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<object, FixedDocumentSequence.GetPageAsyncRequest> keyValuePair = enumerator.Current;
						FixedDocumentSequence.GetPageAsyncRequest value = keyValuePair.Value;
						if (getPageAsyncRequest.Page.Equals(value.Page))
						{
							ArrayList arrayList2 = arrayList;
							keyValuePair = enumerator.Current;
							arrayList2.Add(keyValuePair.Key);
							this._NotifyGetPageAsyncCompleted(page, pageNumber, args.Error, value.Cancelled, value.UserState);
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
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x0022F3B0 File Offset: 0x0022E3B0
		private void _NotifyGetPageAsyncCompleted(DocumentPage page, int pageNumber, Exception error, bool cancelled, object userState)
		{
			this._paginator.NotifyGetPageCompleted(new GetPageCompletedEventArgs(page, pageNumber, error, cancelled, userState));
		}

		// Token: 0x04002651 RID: 9809
		public static readonly DependencyProperty PrintTicketProperty = DependencyProperty.RegisterAttached("PrintTicket", typeof(object), typeof(FixedDocumentSequence), new FrameworkPropertyMetadata(null));

		// Token: 0x04002652 RID: 9810
		private DocumentReferenceCollection _references;

		// Token: 0x04002653 RID: 9811
		private DocumentReference _partialRef;

		// Token: 0x04002654 RID: 9812
		private FixedDocumentSequencePaginator _paginator;

		// Token: 0x04002655 RID: 9813
		private IDictionary<object, FixedDocumentSequence.GetPageAsyncRequest> _asyncOps;

		// Token: 0x04002656 RID: 9814
		private IList<FixedDocumentSequence.RequestedPage> _pendingPages;

		// Token: 0x04002657 RID: 9815
		private Size _pageSize;

		// Token: 0x04002658 RID: 9816
		private bool _navigateAfterPagination;

		// Token: 0x04002659 RID: 9817
		private string _navigateFragment;

		// Token: 0x0400265A RID: 9818
		private DocumentSequenceTextContainer _textContainer;

		// Token: 0x0400265B RID: 9819
		private RubberbandSelector _rubberbandSelector;

		// Token: 0x02000B2E RID: 2862
		private struct RequestedPage
		{
			// Token: 0x06008C8E RID: 35982 RVA: 0x0033D13E File Offset: 0x0033C13E
			internal RequestedPage(int pageNumber)
			{
				this.PageNumber = pageNumber;
				this.ChildPageNumber = 0;
				this.ChildPaginator = null;
			}

			// Token: 0x06008C8F RID: 35983 RVA: 0x0033D155 File Offset: 0x0033C155
			public override int GetHashCode()
			{
				return this.PageNumber;
			}

			// Token: 0x06008C90 RID: 35984 RVA: 0x0033D15D File Offset: 0x0033C15D
			public override bool Equals(object obj)
			{
				return obj is FixedDocumentSequence.RequestedPage && this.Equals((FixedDocumentSequence.RequestedPage)obj);
			}

			// Token: 0x06008C91 RID: 35985 RVA: 0x0033D175 File Offset: 0x0033C175
			public bool Equals(FixedDocumentSequence.RequestedPage obj)
			{
				return this.PageNumber == obj.PageNumber;
			}

			// Token: 0x06008C92 RID: 35986 RVA: 0x0033D185 File Offset: 0x0033C185
			public static bool operator ==(FixedDocumentSequence.RequestedPage obj1, FixedDocumentSequence.RequestedPage obj2)
			{
				return obj1.Equals(obj2);
			}

			// Token: 0x06008C93 RID: 35987 RVA: 0x0033D18F File Offset: 0x0033C18F
			public static bool operator !=(FixedDocumentSequence.RequestedPage obj1, FixedDocumentSequence.RequestedPage obj2)
			{
				return !obj1.Equals(obj2);
			}

			// Token: 0x040047F7 RID: 18423
			internal DynamicDocumentPaginator ChildPaginator;

			// Token: 0x040047F8 RID: 18424
			internal int ChildPageNumber;

			// Token: 0x040047F9 RID: 18425
			internal int PageNumber;
		}

		// Token: 0x02000B2F RID: 2863
		private class GetPageAsyncRequest
		{
			// Token: 0x06008C94 RID: 35988 RVA: 0x0033D19C File Offset: 0x0033C19C
			internal GetPageAsyncRequest(FixedDocumentSequence.RequestedPage page, object userState)
			{
				this.Page = page;
				this.UserState = userState;
				this.Cancelled = false;
			}

			// Token: 0x040047FA RID: 18426
			internal FixedDocumentSequence.RequestedPage Page;

			// Token: 0x040047FB RID: 18427
			internal object UserState;

			// Token: 0x040047FC RID: 18428
			internal bool Cancelled;
		}
	}
}
