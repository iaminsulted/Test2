using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Printing;
using System.Windows.Annotations;
using System.Windows.Automation.Peers;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps;
using MS.Internal;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.Commands;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000831 RID: 2097
	[ContentProperty("Document")]
	public abstract class DocumentViewerBase : Control, IAddChild, IServiceProvider
	{
		// Token: 0x06007B08 RID: 31496 RVA: 0x0030AD9C File Offset: 0x00309D9C
		static DocumentViewerBase()
		{
			DocumentViewerBase.CreateCommandBindings();
			EventManager.RegisterClassHandler(typeof(DocumentViewerBase), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(DocumentViewerBase.HandleRequestBringIntoView));
			TextBoxBase.AutoWordSelectionProperty.OverrideMetadata(typeof(DocumentViewerBase), new FrameworkPropertyMetadata(true));
		}

		// Token: 0x06007B09 RID: 31497 RVA: 0x0030AF47 File Offset: 0x00309F47
		protected DocumentViewerBase()
		{
			this._pageViews = new ReadOnlyCollection<DocumentPageView>(new List<DocumentPageView>());
			this.SetFlags(true, DocumentViewerBase.Flags.IsSelectionEnabled);
		}

		// Token: 0x06007B0A RID: 31498 RVA: 0x0030AF68 File Offset: 0x00309F68
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UpdatePageViews();
		}

		// Token: 0x06007B0B RID: 31499 RVA: 0x0013F776 File Offset: 0x0013E776
		public void PreviousPage()
		{
			this.OnPreviousPageCommand();
		}

		// Token: 0x06007B0C RID: 31500 RVA: 0x0013F77E File Offset: 0x0013E77E
		public void NextPage()
		{
			this.OnNextPageCommand();
		}

		// Token: 0x06007B0D RID: 31501 RVA: 0x0013F786 File Offset: 0x0013E786
		public void FirstPage()
		{
			this.OnFirstPageCommand();
		}

		// Token: 0x06007B0E RID: 31502 RVA: 0x0013F78E File Offset: 0x0013E78E
		public void LastPage()
		{
			this.OnLastPageCommand();
		}

		// Token: 0x06007B0F RID: 31503 RVA: 0x0013F7D0 File Offset: 0x0013E7D0
		public void GoToPage(int pageNumber)
		{
			this.OnGoToPageCommand(pageNumber);
		}

		// Token: 0x06007B10 RID: 31504 RVA: 0x0013F796 File Offset: 0x0013E796
		public void Print()
		{
			this.OnPrintCommand();
		}

		// Token: 0x06007B11 RID: 31505 RVA: 0x0013F79E File Offset: 0x0013E79E
		public void CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		// Token: 0x06007B12 RID: 31506 RVA: 0x0030AF76 File Offset: 0x00309F76
		public virtual bool CanGoToPage(int pageNumber)
		{
			return (pageNumber > 0 && pageNumber <= this.PageCount) || (this._document != null && pageNumber - 1 == this.PageCount && !this._document.DocumentPaginator.IsPageCountValid);
		}

		// Token: 0x17001C74 RID: 7284
		// (get) Token: 0x06007B13 RID: 31507 RVA: 0x0030AFAF File Offset: 0x00309FAF
		// (set) Token: 0x06007B14 RID: 31508 RVA: 0x0030AFB7 File Offset: 0x00309FB7
		public IDocumentPaginatorSource Document
		{
			get
			{
				return this._document;
			}
			set
			{
				base.SetValue(DocumentViewerBase.DocumentProperty, value);
			}
		}

		// Token: 0x17001C75 RID: 7285
		// (get) Token: 0x06007B15 RID: 31509 RVA: 0x0030AFC5 File Offset: 0x00309FC5
		public int PageCount
		{
			get
			{
				return (int)base.GetValue(DocumentViewerBase.PageCountProperty);
			}
		}

		// Token: 0x17001C76 RID: 7286
		// (get) Token: 0x06007B16 RID: 31510 RVA: 0x0030AFD7 File Offset: 0x00309FD7
		public virtual int MasterPageNumber
		{
			get
			{
				return (int)base.GetValue(DocumentViewerBase.MasterPageNumberProperty);
			}
		}

		// Token: 0x17001C77 RID: 7287
		// (get) Token: 0x06007B17 RID: 31511 RVA: 0x0030AFE9 File Offset: 0x00309FE9
		public virtual bool CanGoToPreviousPage
		{
			get
			{
				return (bool)base.GetValue(DocumentViewerBase.CanGoToPreviousPageProperty);
			}
		}

		// Token: 0x17001C78 RID: 7288
		// (get) Token: 0x06007B18 RID: 31512 RVA: 0x0030AFFB File Offset: 0x00309FFB
		public virtual bool CanGoToNextPage
		{
			get
			{
				return (bool)base.GetValue(DocumentViewerBase.CanGoToNextPageProperty);
			}
		}

		// Token: 0x17001C79 RID: 7289
		// (get) Token: 0x06007B19 RID: 31513 RVA: 0x0030B00D File Offset: 0x0030A00D
		[CLSCompliant(false)]
		public ReadOnlyCollection<DocumentPageView> PageViews
		{
			get
			{
				return this._pageViews;
			}
		}

		// Token: 0x06007B1A RID: 31514 RVA: 0x0030B015 File Offset: 0x0030A015
		public static bool GetIsMasterPage(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(DocumentViewerBase.IsMasterPageProperty);
		}

		// Token: 0x06007B1B RID: 31515 RVA: 0x0030B035 File Offset: 0x0030A035
		public static void SetIsMasterPage(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(DocumentViewerBase.IsMasterPageProperty, value);
		}

		// Token: 0x14000154 RID: 340
		// (add) Token: 0x06007B1C RID: 31516 RVA: 0x0030B054 File Offset: 0x0030A054
		// (remove) Token: 0x06007B1D RID: 31517 RVA: 0x0030B08C File Offset: 0x0030A08C
		public event EventHandler PageViewsChanged;

		// Token: 0x06007B1E RID: 31518 RVA: 0x0030B0C1 File Offset: 0x0030A0C1
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentViewerBaseAutomationPeer(this);
		}

		// Token: 0x06007B1F RID: 31519 RVA: 0x0030B0C9 File Offset: 0x0030A0C9
		protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
		{
			FlowDocument flowDocument = this._document as FlowDocument;
			if (flowDocument == null)
			{
				return;
			}
			flowDocument.SetDpi(newDpiScaleInfo);
		}

		// Token: 0x06007B20 RID: 31520 RVA: 0x0030B0E1 File Offset: 0x0030A0E1
		protected void InvalidatePageViews()
		{
			this.UpdatePageViews();
			base.InvalidateMeasure();
		}

		// Token: 0x06007B21 RID: 31521 RVA: 0x0030B0F0 File Offset: 0x0030A0F0
		protected DocumentPageView GetMasterPageView()
		{
			DocumentPageView documentPageView = null;
			for (int i = 0; i < this._pageViews.Count; i++)
			{
				if (DocumentViewerBase.GetIsMasterPage(this._pageViews[i]))
				{
					documentPageView = this._pageViews[i];
					break;
				}
			}
			if (documentPageView == null)
			{
				documentPageView = ((this._pageViews.Count > 0) ? this._pageViews[0] : null);
			}
			return documentPageView;
		}

		// Token: 0x06007B22 RID: 31522 RVA: 0x0030B15C File Offset: 0x0030A15C
		protected virtual ReadOnlyCollection<DocumentPageView> GetPageViewsCollection(out bool changed)
		{
			List<DocumentPageView> list = new List<DocumentPageView>(1);
			this.FindDocumentPageViews(this, list);
			AdornerDecorator adornerDecorator = this.FindAdornerDecorator(this);
			this.TextEditorRenderScope = ((adornerDecorator != null) ? (adornerDecorator.Child as FrameworkElement) : null);
			for (int i = 0; i < this._pageViews.Count; i++)
			{
				this._pageViews[i].DocumentPaginator = null;
			}
			changed = true;
			return new ReadOnlyCollection<DocumentPageView>(list);
		}

		// Token: 0x06007B23 RID: 31523 RVA: 0x0030B1C8 File Offset: 0x0030A1C8
		protected virtual void OnPageViewsChanged()
		{
			if (this.PageViewsChanged != null)
			{
				this.PageViewsChanged(this, EventArgs.Empty);
			}
			this.OnMasterPageNumberChanged();
		}

		// Token: 0x06007B24 RID: 31524 RVA: 0x0030B1E9 File Offset: 0x0030A1E9
		protected virtual void OnMasterPageNumberChanged()
		{
			this.UpdateReadOnlyProperties(true, true);
		}

		// Token: 0x06007B25 RID: 31525 RVA: 0x0030B1F3 File Offset: 0x0030A1F3
		protected virtual void OnBringIntoView(DependencyObject element, Rect rect, int pageNumber)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.OnGoToPageCommand(pageNumber);
		}

		// Token: 0x06007B26 RID: 31526 RVA: 0x0030B20A File Offset: 0x0030A20A
		protected virtual void OnPreviousPageCommand()
		{
			if (this.CanGoToPreviousPage)
			{
				this.ShiftPagesByOffset(-1);
			}
		}

		// Token: 0x06007B27 RID: 31527 RVA: 0x0030B21B File Offset: 0x0030A21B
		protected virtual void OnNextPageCommand()
		{
			if (this.CanGoToNextPage)
			{
				this.ShiftPagesByOffset(1);
			}
		}

		// Token: 0x06007B28 RID: 31528 RVA: 0x0030B22C File Offset: 0x0030A22C
		protected virtual void OnFirstPageCommand()
		{
			this.ShiftPagesByOffset(1 - this.MasterPageNumber);
		}

		// Token: 0x06007B29 RID: 31529 RVA: 0x0030B23C File Offset: 0x0030A23C
		protected virtual void OnLastPageCommand()
		{
			this.ShiftPagesByOffset(this.PageCount - this.MasterPageNumber);
		}

		// Token: 0x06007B2A RID: 31530 RVA: 0x0030B251 File Offset: 0x0030A251
		protected virtual void OnGoToPageCommand(int pageNumber)
		{
			if (this.CanGoToPage(pageNumber))
			{
				this.ShiftPagesByOffset(pageNumber - this.MasterPageNumber);
			}
		}

		// Token: 0x06007B2B RID: 31531 RVA: 0x0030B26C File Offset: 0x0030A26C
		protected virtual void OnPrintCommand()
		{
			PrintDocumentImageableArea printDocumentImageableArea = null;
			if (this._documentWriter != null)
			{
				return;
			}
			if (this._document != null)
			{
				XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref printDocumentImageableArea);
				if (xpsDocumentWriter != null && printDocumentImageableArea != null)
				{
					this._documentWriter = xpsDocumentWriter;
					this._documentWriter.WritingCompleted += this.HandlePrintCompleted;
					this._documentWriter.WritingCancelled += this.HandlePrintCancelled;
					CommandManager.InvalidateRequerySuggested();
					if (this._document is FixedDocumentSequence)
					{
						xpsDocumentWriter.WriteAsync(this._document as FixedDocumentSequence);
						return;
					}
					if (this._document is FixedDocument)
					{
						xpsDocumentWriter.WriteAsync(this._document as FixedDocument);
						return;
					}
					xpsDocumentWriter.WriteAsync(this._document.DocumentPaginator);
				}
			}
		}

		// Token: 0x06007B2C RID: 31532 RVA: 0x0030B32C File Offset: 0x0030A32C
		protected virtual void OnCancelPrintCommand()
		{
			if (this._documentWriter != null)
			{
				this._documentWriter.CancelAsync();
			}
		}

		// Token: 0x06007B2D RID: 31533 RVA: 0x0030B344 File Offset: 0x0030A344
		protected virtual void OnDocumentChanged()
		{
			for (int i = 0; i < this._pageViews.Count; i++)
			{
				this._pageViews[i].DocumentPaginator = ((this._document != null) ? this._document.DocumentPaginator : null);
			}
			this.UpdateReadOnlyProperties(true, true);
			this.AttachTextEditor();
		}

		// Token: 0x17001C7A RID: 7290
		// (get) Token: 0x06007B2E RID: 31534 RVA: 0x0030B39C File Offset: 0x0030A39C
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (base.HasLogicalChildren && this._document != null)
				{
					return new SingleChildEnumerator(this._document);
				}
				return EmptyEnumerator.Instance;
			}
		}

		// Token: 0x06007B2F RID: 31535 RVA: 0x0030B3BF File Offset: 0x0030A3BF
		internal bool IsMasterPageView(DocumentPageView pageView)
		{
			Invariant.Assert(pageView != null);
			return pageView == this.GetMasterPageView();
		}

		// Token: 0x06007B30 RID: 31536 RVA: 0x0030B3D4 File Offset: 0x0030A3D4
		internal ITextRange Find(FindToolBar findToolBar)
		{
			ITextView masterPageTextView = null;
			DocumentPageView masterPageView = this.GetMasterPageView();
			if (masterPageView != null && masterPageView != null)
			{
				masterPageTextView = (((IServiceProvider)masterPageView).GetService(typeof(ITextView)) as ITextView);
			}
			return DocumentViewerHelper.Find(findToolBar, this._textEditor, this._textView, masterPageTextView);
		}

		// Token: 0x17001C7B RID: 7291
		// (get) Token: 0x06007B31 RID: 31537 RVA: 0x0030B419 File Offset: 0x0030A419
		// (set) Token: 0x06007B32 RID: 31538 RVA: 0x0030B423 File Offset: 0x0030A423
		internal bool IsSelectionEnabled
		{
			get
			{
				return this.CheckFlags(DocumentViewerBase.Flags.IsSelectionEnabled);
			}
			set
			{
				this.SetFlags(value, DocumentViewerBase.Flags.IsSelectionEnabled);
				this.AttachTextEditor();
			}
		}

		// Token: 0x17001C7C RID: 7292
		// (get) Token: 0x06007B33 RID: 31539 RVA: 0x0030B434 File Offset: 0x0030A434
		internal TextEditor TextEditor
		{
			get
			{
				return this._textEditor;
			}
		}

		// Token: 0x17001C7D RID: 7293
		// (get) Token: 0x06007B34 RID: 31540 RVA: 0x0030B43C File Offset: 0x0030A43C
		// (set) Token: 0x06007B35 RID: 31541 RVA: 0x0030B444 File Offset: 0x0030A444
		internal FrameworkElement TextEditorRenderScope
		{
			get
			{
				return this._textEditorRenderScope;
			}
			set
			{
				this._textEditorRenderScope = value;
				this.AttachTextEditor();
			}
		}

		// Token: 0x06007B36 RID: 31542 RVA: 0x0030B454 File Offset: 0x0030A454
		private ITextPointer GetMasterPageTextPointer(bool startOfPage)
		{
			ITextPointer textPointer = null;
			DocumentPageView masterPageView = this.GetMasterPageView();
			if (masterPageView != null && masterPageView != null)
			{
				ITextView textView = ((IServiceProvider)masterPageView).GetService(typeof(ITextView)) as ITextView;
				if (textView != null && textView.IsValid)
				{
					foreach (TextSegment textSegment in textView.TextSegments)
					{
						if (!textSegment.IsNull)
						{
							if (textPointer == null)
							{
								textPointer = (startOfPage ? textSegment.Start : textSegment.End);
							}
							else if (startOfPage)
							{
								if (textSegment.Start.CompareTo(textPointer) < 0)
								{
									textPointer = textSegment.Start;
								}
							}
							else if (textSegment.End.CompareTo(textPointer) > 0)
							{
								textPointer = textSegment.End;
							}
						}
					}
				}
			}
			return textPointer;
		}

		// Token: 0x06007B37 RID: 31543 RVA: 0x0030B534 File Offset: 0x0030A534
		private void UpdatePageViews()
		{
			bool flag;
			ReadOnlyCollection<DocumentPageView> pageViewsCollection = this.GetPageViewsCollection(out flag);
			if (flag)
			{
				this.VerifyDocumentPageViews(pageViewsCollection);
				this._pageViews = pageViewsCollection;
				for (int i = 0; i < this._pageViews.Count; i++)
				{
					this._pageViews[i].DocumentPaginator = ((this._document != null) ? this._document.DocumentPaginator : null);
				}
				if (this._textView != null)
				{
					this._textView.OnPagesUpdated();
				}
				this.OnPageViewsChanged();
			}
		}

		// Token: 0x06007B38 RID: 31544 RVA: 0x0030B5B4 File Offset: 0x0030A5B4
		private void VerifyDocumentPageViews(ReadOnlyCollection<DocumentPageView> pageViews)
		{
			bool flag = false;
			if (pageViews == null)
			{
				throw new ArgumentException(SR.Get("DocumentViewerPageViewsCollectionEmpty"));
			}
			for (int i = 0; i < pageViews.Count; i++)
			{
				if (DocumentViewerBase.GetIsMasterPage(pageViews[i]))
				{
					if (flag)
					{
						throw new ArgumentException(SR.Get("DocumentViewerOneMasterPage"));
					}
					flag = true;
				}
			}
		}

		// Token: 0x06007B39 RID: 31545 RVA: 0x0030B60C File Offset: 0x0030A60C
		private void FindDocumentPageViews(Visual root, List<DocumentPageView> pageViews)
		{
			Invariant.Assert(root != null);
			Invariant.Assert(pageViews != null);
			int internalVisualChildrenCount = root.InternalVisualChildrenCount;
			for (int i = 0; i < internalVisualChildrenCount; i++)
			{
				Visual visual = root.InternalGetVisualChild(i);
				FrameworkElement frameworkElement = visual as FrameworkElement;
				if (frameworkElement != null)
				{
					if (frameworkElement.TemplatedParent != null)
					{
						if (frameworkElement is DocumentPageView)
						{
							pageViews.Add(frameworkElement as DocumentPageView);
						}
						else
						{
							this.FindDocumentPageViews(frameworkElement, pageViews);
						}
					}
				}
				else
				{
					this.FindDocumentPageViews(visual, pageViews);
				}
			}
		}

		// Token: 0x06007B3A RID: 31546 RVA: 0x0030B680 File Offset: 0x0030A680
		private AdornerDecorator FindAdornerDecorator(Visual root)
		{
			Invariant.Assert(root != null);
			AdornerDecorator adornerDecorator = null;
			int internalVisualChildrenCount = root.InternalVisualChildrenCount;
			for (int i = 0; i < internalVisualChildrenCount; i++)
			{
				Visual visual = root.InternalGetVisualChild(i);
				FrameworkElement frameworkElement = visual as FrameworkElement;
				if (frameworkElement != null)
				{
					if (frameworkElement.TemplatedParent != null)
					{
						if (frameworkElement is AdornerDecorator)
						{
							adornerDecorator = (AdornerDecorator)frameworkElement;
						}
						else if (!(frameworkElement is DocumentPageView))
						{
							adornerDecorator = this.FindAdornerDecorator(frameworkElement);
						}
					}
				}
				else
				{
					adornerDecorator = this.FindAdornerDecorator(visual);
				}
				if (adornerDecorator != null)
				{
					break;
				}
			}
			return adornerDecorator;
		}

		// Token: 0x06007B3B RID: 31547 RVA: 0x0030B6F8 File Offset: 0x0030A6F8
		private void AttachTextEditor()
		{
			AnnotationService service = AnnotationService.GetService(this);
			if (this._textEditor != null)
			{
				this._textEditor.OnDetach();
				this._textEditor = null;
				if (this._textView.TextContainer.TextView == this._textView)
				{
					this._textView.TextContainer.TextView = null;
				}
				this._textView = null;
			}
			if (service != null)
			{
				service.Disable();
			}
			ITextContainer textContainer = this.TextContainer;
			if (textContainer != null && this.TextEditorRenderScope != null && textContainer.TextSelection == null)
			{
				this._textView = new MultiPageTextView(this, this.TextEditorRenderScope, textContainer);
				this._textEditor = new TextEditor(textContainer, this, false);
				this._textEditor.IsReadOnly = !DocumentViewerBase.IsEditingEnabled;
				this._textEditor.TextView = this._textView;
				textContainer.TextView = this._textView;
			}
			if (service != null)
			{
				service.Enable(service.Store);
			}
		}

		// Token: 0x06007B3C RID: 31548 RVA: 0x0030B7D9 File Offset: 0x0030A7D9
		private void HandlePrintCompleted(object sender, WritingCompletedEventArgs e)
		{
			this.CleanUpPrintOperation();
		}

		// Token: 0x06007B3D RID: 31549 RVA: 0x0030B7D9 File Offset: 0x0030A7D9
		private void HandlePrintCancelled(object sender, WritingCancelledEventArgs e)
		{
			this.CleanUpPrintOperation();
		}

		// Token: 0x06007B3E RID: 31550 RVA: 0x0030B7E1 File Offset: 0x0030A7E1
		private void HandlePaginationCompleted(object sender, EventArgs e)
		{
			this.UpdateReadOnlyProperties(true, false);
		}

		// Token: 0x06007B3F RID: 31551 RVA: 0x0030B7E1 File Offset: 0x0030A7E1
		private void HandlePaginationProgress(object sender, EventArgs e)
		{
			this.UpdateReadOnlyProperties(true, false);
		}

		// Token: 0x06007B40 RID: 31552 RVA: 0x0030B7EC File Offset: 0x0030A7EC
		private void HandleGetPageNumberCompleted(object sender, GetPageNumberCompletedEventArgs e)
		{
			this.UpdateReadOnlyProperties(true, false);
			if (this._document != null && sender == this._document.DocumentPaginator && e != null && !e.Cancelled && e.Error == null)
			{
				DocumentViewerBase.BringIntoViewState bringIntoViewState = e.UserState as DocumentViewerBase.BringIntoViewState;
				if (bringIntoViewState != null && bringIntoViewState.Source == this)
				{
					this.OnBringIntoView(bringIntoViewState.TargetObject, bringIntoViewState.TargetRect, e.PageNumber + 1);
				}
			}
		}

		// Token: 0x06007B41 RID: 31553 RVA: 0x0030B85C File Offset: 0x0030A85C
		private void HandleRequestBringIntoView(RequestBringIntoViewEventArgs args)
		{
			Rect rect = Rect.Empty;
			if (args != null && args.TargetObject != null && this._document is DependencyObject)
			{
				DependencyObject dependencyObject = this._document as DependencyObject;
				if (args.TargetObject == this._document)
				{
					this.OnGoToPageCommand(1);
					args.Handled = true;
				}
				else
				{
					DependencyObject dependencyObject2 = args.TargetObject;
					while (dependencyObject2 != null && dependencyObject2 != dependencyObject)
					{
						FrameworkElement frameworkElement = dependencyObject2 as FrameworkElement;
						if (frameworkElement != null && frameworkElement.TemplatedParent != null)
						{
							dependencyObject2 = frameworkElement.TemplatedParent;
						}
						else
						{
							dependencyObject2 = LogicalTreeHelper.GetParent(dependencyObject2);
						}
					}
					if (dependencyObject2 != null)
					{
						if (args.TargetObject is UIElement)
						{
							UIElement uielement = (UIElement)args.TargetObject;
							if (VisualTreeHelper.IsAncestorOf(this, uielement))
							{
								rect = args.TargetRect;
								if (rect.IsEmpty)
								{
									rect = new Rect(uielement.RenderSize);
								}
								rect = uielement.TransformToAncestor(this).TransformBounds(rect);
								rect.IntersectsWith(new Rect(base.RenderSize));
							}
						}
						if (rect.IsEmpty)
						{
							DynamicDocumentPaginator dynamicDocumentPaginator = this._document.DocumentPaginator as DynamicDocumentPaginator;
							if (dynamicDocumentPaginator != null)
							{
								ContentPosition objectPosition = dynamicDocumentPaginator.GetObjectPosition(args.TargetObject);
								if (objectPosition != null && objectPosition != ContentPosition.Missing)
								{
									DocumentViewerBase.BringIntoViewState userState = new DocumentViewerBase.BringIntoViewState(this, objectPosition, args.TargetObject, args.TargetRect);
									dynamicDocumentPaginator.GetPageNumberAsync(objectPosition, userState);
								}
							}
						}
						args.Handled = true;
					}
				}
				if (args.Handled)
				{
					if (rect.IsEmpty)
					{
						base.BringIntoView();
						return;
					}
					base.BringIntoView(rect);
				}
			}
		}

		// Token: 0x06007B42 RID: 31554 RVA: 0x0030B9E0 File Offset: 0x0030A9E0
		private void UpdateReadOnlyProperties(bool pageCountChanged, bool masterPageChanged)
		{
			if (pageCountChanged)
			{
				base.SetValue(DocumentViewerBase.PageCountPropertyKey, (this._document != null) ? this._document.DocumentPaginator.PageCount : 0);
			}
			bool flag = false;
			if (masterPageChanged)
			{
				int num = 0;
				if (this._document != null && this._pageViews.Count > 0)
				{
					DocumentPageView masterPageView = this.GetMasterPageView();
					if (masterPageView != null)
					{
						num = masterPageView.PageNumber + 1;
					}
				}
				base.SetValue(DocumentViewerBase.MasterPageNumberPropertyKey, num);
				base.SetValue(DocumentViewerBase.CanGoToPreviousPagePropertyKey, this.MasterPageNumber > 1);
				flag = true;
			}
			if (pageCountChanged || masterPageChanged)
			{
				bool value = false;
				if (this._document != null)
				{
					value = (this.MasterPageNumber < this._document.DocumentPaginator.PageCount || !this._document.DocumentPaginator.IsPageCountValid);
				}
				base.SetValue(DocumentViewerBase.CanGoToNextPagePropertyKey, value);
				flag = true;
			}
			if (flag)
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x06007B43 RID: 31555 RVA: 0x0030BAC8 File Offset: 0x0030AAC8
		private void ShiftPagesByOffset(int offset)
		{
			if (offset != 0)
			{
				for (int i = 0; i < this._pageViews.Count; i++)
				{
					this._pageViews[i].PageNumber += offset;
				}
				this.OnMasterPageNumberChanged();
			}
		}

		// Token: 0x06007B44 RID: 31556 RVA: 0x0030BB0D File Offset: 0x0030AB0D
		private void SetFlags(bool value, DocumentViewerBase.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06007B45 RID: 31557 RVA: 0x0030BB2B File Offset: 0x0030AB2B
		private bool CheckFlags(DocumentViewerBase.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x06007B46 RID: 31558 RVA: 0x0030BB38 File Offset: 0x0030AB38
		private void DocumentChanged(IDocumentPaginatorSource oldDocument, IDocumentPaginatorSource newDocument)
		{
			this._document = newDocument;
			if (oldDocument != null)
			{
				if (this.CheckFlags(DocumentViewerBase.Flags.DocumentAsLogicalChild))
				{
					base.RemoveLogicalChild(oldDocument);
				}
				DynamicDocumentPaginator dynamicDocumentPaginator = oldDocument.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null)
				{
					dynamicDocumentPaginator.PaginationProgress -= new PaginationProgressEventHandler(this.HandlePaginationProgress);
					dynamicDocumentPaginator.PaginationCompleted -= this.HandlePaginationCompleted;
					dynamicDocumentPaginator.GetPageNumberCompleted -= this.HandleGetPageNumberCompleted;
				}
				DependencyObject dependencyObject = oldDocument as DependencyObject;
				if (dependencyObject != null)
				{
					dependencyObject.ClearValue(PathNode.HiddenParentProperty);
				}
			}
			DependencyObject dependencyObject2 = this._document as DependencyObject;
			if (dependencyObject2 != null && LogicalTreeHelper.GetParent(dependencyObject2) != null && dependencyObject2 is ContentElement)
			{
				ContentOperations.SetParent((ContentElement)dependencyObject2, this);
				this.SetFlags(false, DocumentViewerBase.Flags.DocumentAsLogicalChild);
			}
			else
			{
				this.SetFlags(true, DocumentViewerBase.Flags.DocumentAsLogicalChild);
			}
			if (this._document != null)
			{
				if (this.CheckFlags(DocumentViewerBase.Flags.DocumentAsLogicalChild))
				{
					base.AddLogicalChild(this._document);
				}
				DynamicDocumentPaginator dynamicDocumentPaginator = this._document.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null)
				{
					dynamicDocumentPaginator.PaginationProgress += new PaginationProgressEventHandler(this.HandlePaginationProgress);
					dynamicDocumentPaginator.PaginationCompleted += this.HandlePaginationCompleted;
					dynamicDocumentPaginator.GetPageNumberCompleted += this.HandleGetPageNumberCompleted;
				}
				DependencyObject dependencyObject3 = this._document as DependencyObject;
				FlowDocument flowDocument;
				if (this._document is FixedDocument || this._document is FixedDocumentSequence)
				{
					base.ClearValue(AnnotationService.DataIdProperty);
					AnnotationService.SetSubTreeProcessorId(this, FixedPageProcessor.Id);
					dependencyObject3.SetValue(PathNode.HiddenParentProperty, this);
					AnnotationService service = AnnotationService.GetService(this);
					if (service != null)
					{
						service.LocatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextRange));
						service.LocatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextAnchor));
					}
				}
				else if ((flowDocument = (this._document as FlowDocument)) != null)
				{
					flowDocument.SetDpi(base.GetDpi());
					flowDocument.SetValue(PathNode.HiddenParentProperty, this);
					AnnotationService service2 = AnnotationService.GetService(this);
					if (service2 != null)
					{
						service2.LocatorManager.RegisterSelectionProcessor(new TextSelectionProcessor(), typeof(TextRange));
						service2.LocatorManager.RegisterSelectionProcessor(new TextSelectionProcessor(), typeof(TextAnchor));
						service2.LocatorManager.RegisterSelectionProcessor(new TextViewSelectionProcessor(), typeof(DocumentViewerBase));
					}
					AnnotationService.SetDataId(this, "FlowDocument");
				}
				else
				{
					base.ClearValue(AnnotationService.SubTreeProcessorIdProperty);
					base.ClearValue(AnnotationService.DataIdProperty);
				}
			}
			DocumentViewerBaseAutomationPeer documentViewerBaseAutomationPeer = UIElementAutomationPeer.FromElement(this) as DocumentViewerBaseAutomationPeer;
			if (documentViewerBaseAutomationPeer != null)
			{
				documentViewerBaseAutomationPeer.InvalidatePeer();
			}
			this.OnDocumentChanged();
		}

		// Token: 0x06007B47 RID: 31559 RVA: 0x0030BDC0 File Offset: 0x0030ADC0
		private void CleanUpPrintOperation()
		{
			if (this._documentWriter != null)
			{
				this._documentWriter.WritingCompleted -= this.HandlePrintCompleted;
				this._documentWriter.WritingCancelled -= this.HandlePrintCancelled;
				this._documentWriter = null;
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x06007B48 RID: 31560 RVA: 0x0030BE10 File Offset: 0x0030AE10
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(DocumentViewerBase.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(DocumentViewerBase.CanExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.PreviousPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.NextPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.FirstPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.LastPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.GoToPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), ApplicationCommands.Print, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.P, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), ApplicationCommands.CancelPrint, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			TextEditor.RegisterCommandHandlers(typeof(DocumentViewerBase), true, !DocumentViewerBase.IsEditingEnabled, true);
		}

		// Token: 0x06007B49 RID: 31561 RVA: 0x0030BEF4 File Offset: 0x0030AEF4
		private static void CanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			DocumentViewerBase documentViewerBase = target as DocumentViewerBase;
			Invariant.Assert(documentViewerBase != null, "Target of CanExecuteRoutedEventHandler must be DocumentViewerBase.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == ApplicationCommands.Print)
			{
				args.CanExecute = (documentViewerBase.Document != null && documentViewerBase._documentWriter == null);
				args.Handled = true;
				return;
			}
			if (args.Command == ApplicationCommands.CancelPrint)
			{
				args.CanExecute = (documentViewerBase._documentWriter != null);
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x06007B4A RID: 31562 RVA: 0x0030BF78 File Offset: 0x0030AF78
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			DocumentViewerBase documentViewerBase = target as DocumentViewerBase;
			Invariant.Assert(documentViewerBase != null, "Target of ExecuteEvent must be DocumentViewerBase.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == NavigationCommands.PreviousPage)
			{
				documentViewerBase.OnPreviousPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.NextPage)
			{
				documentViewerBase.OnNextPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.FirstPage)
			{
				documentViewerBase.OnFirstPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.LastPage)
			{
				documentViewerBase.OnLastPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.GoToPage)
			{
				if (args.Parameter != null)
				{
					int num = -1;
					try
					{
						num = Convert.ToInt32(args.Parameter, CultureInfo.CurrentCulture);
					}
					catch (InvalidCastException)
					{
					}
					catch (OverflowException)
					{
					}
					catch (FormatException)
					{
					}
					if (num >= 0)
					{
						documentViewerBase.OnGoToPageCommand(num);
						return;
					}
				}
			}
			else
			{
				if (args.Command == ApplicationCommands.Print)
				{
					documentViewerBase.OnPrintCommand();
					return;
				}
				if (args.Command == ApplicationCommands.CancelPrint)
				{
					documentViewerBase.OnCancelPrintCommand();
					return;
				}
				Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
			}
		}

		// Token: 0x06007B4B RID: 31563 RVA: 0x0030C094 File Offset: 0x0030B094
		private static void HandleRequestBringIntoView(object sender, RequestBringIntoViewEventArgs args)
		{
			if (sender != null && sender is DocumentViewerBase)
			{
				((DocumentViewerBase)sender).HandleRequestBringIntoView(args);
			}
		}

		// Token: 0x06007B4C RID: 31564 RVA: 0x0030C0AD File Offset: 0x0030B0AD
		private static void DocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is DocumentViewerBase);
			((DocumentViewerBase)d).DocumentChanged((IDocumentPaginatorSource)e.OldValue, (IDocumentPaginatorSource)e.NewValue);
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x17001C7E RID: 7294
		// (get) Token: 0x06007B4D RID: 31565 RVA: 0x0030C0EC File Offset: 0x0030B0EC
		private ITextContainer TextContainer
		{
			get
			{
				ITextContainer result = null;
				if (this._document != null && this._document is IServiceProvider && this.CheckFlags(DocumentViewerBase.Flags.IsSelectionEnabled))
				{
					result = (((IServiceProvider)this._document).GetService(typeof(ITextContainer)) as ITextContainer);
				}
				return result;
			}
		}

		// Token: 0x06007B4E RID: 31566 RVA: 0x0030C13C File Offset: 0x0030B13C
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.Document != null)
			{
				throw new InvalidOperationException(SR.Get("DocumentViewerCanHaveOnlyOneChild"));
			}
			IDocumentPaginatorSource documentPaginatorSource = value as IDocumentPaginatorSource;
			if (documentPaginatorSource == null)
			{
				throw new ArgumentException(SR.Get("DocumentViewerChildMustImplementIDocumentPaginatorSource"), "value");
			}
			this.Document = documentPaginatorSource;
		}

		// Token: 0x06007B4F RID: 31567 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06007B50 RID: 31568 RVA: 0x0030C198 File Offset: 0x0030B198
		object IServiceProvider.GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				result = this._textView;
			}
			else if (serviceType == typeof(TextContainer) || serviceType == typeof(ITextContainer))
			{
				result = this.TextContainer;
			}
			return result;
		}

		// Token: 0x04003A2E RID: 14894
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(IDocumentPaginatorSource), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DocumentViewerBase.DocumentChanged)));

		// Token: 0x04003A2F RID: 14895
		protected static readonly DependencyPropertyKey PageCountPropertyKey = DependencyProperty.RegisterReadOnly("PageCount", typeof(int), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(0));

		// Token: 0x04003A30 RID: 14896
		public static readonly DependencyProperty PageCountProperty = DocumentViewerBase.PageCountPropertyKey.DependencyProperty;

		// Token: 0x04003A31 RID: 14897
		protected static readonly DependencyPropertyKey MasterPageNumberPropertyKey = DependencyProperty.RegisterReadOnly("MasterPageNumber", typeof(int), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(0));

		// Token: 0x04003A32 RID: 14898
		public static readonly DependencyProperty MasterPageNumberProperty = DocumentViewerBase.MasterPageNumberPropertyKey.DependencyProperty;

		// Token: 0x04003A33 RID: 14899
		protected static readonly DependencyPropertyKey CanGoToPreviousPagePropertyKey = DependencyProperty.RegisterReadOnly("CanGoToPreviousPage", typeof(bool), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003A34 RID: 14900
		public static readonly DependencyProperty CanGoToPreviousPageProperty = DocumentViewerBase.CanGoToPreviousPagePropertyKey.DependencyProperty;

		// Token: 0x04003A35 RID: 14901
		protected static readonly DependencyPropertyKey CanGoToNextPagePropertyKey = DependencyProperty.RegisterReadOnly("CanGoToNextPage", typeof(bool), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003A36 RID: 14902
		public static readonly DependencyProperty CanGoToNextPageProperty = DocumentViewerBase.CanGoToNextPagePropertyKey.DependencyProperty;

		// Token: 0x04003A37 RID: 14903
		public static readonly DependencyProperty IsMasterPageProperty = DependencyProperty.RegisterAttached("IsMasterPage", typeof(bool), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003A39 RID: 14905
		private ReadOnlyCollection<DocumentPageView> _pageViews;

		// Token: 0x04003A3A RID: 14906
		private FrameworkElement _textEditorRenderScope;

		// Token: 0x04003A3B RID: 14907
		private MultiPageTextView _textView;

		// Token: 0x04003A3C RID: 14908
		private TextEditor _textEditor;

		// Token: 0x04003A3D RID: 14909
		private IDocumentPaginatorSource _document;

		// Token: 0x04003A3E RID: 14910
		private DocumentViewerBase.Flags _flags;

		// Token: 0x04003A3F RID: 14911
		private XpsDocumentWriter _documentWriter;

		// Token: 0x04003A40 RID: 14912
		private static bool IsEditingEnabled = false;

		// Token: 0x02000C47 RID: 3143
		[Flags]
		private enum Flags
		{
			// Token: 0x04004C25 RID: 19493
			IsSelectionEnabled = 32,
			// Token: 0x04004C26 RID: 19494
			DocumentAsLogicalChild = 64
		}

		// Token: 0x02000C48 RID: 3144
		private class BringIntoViewState
		{
			// Token: 0x06009181 RID: 37249 RVA: 0x00349723 File Offset: 0x00348723
			internal BringIntoViewState(DocumentViewerBase source, ContentPosition contentPosition, DependencyObject targetObject, Rect targetRect)
			{
				this.Source = source;
				this.ContentPosition = contentPosition;
				this.TargetObject = targetObject;
				this.TargetRect = targetRect;
			}

			// Token: 0x04004C27 RID: 19495
			internal DocumentViewerBase Source;

			// Token: 0x04004C28 RID: 19496
			internal ContentPosition ContentPosition;

			// Token: 0x04004C29 RID: 19497
			internal DependencyObject TargetObject;

			// Token: 0x04004C2A RID: 19498
			internal Rect TargetRect;
		}
	}
}
