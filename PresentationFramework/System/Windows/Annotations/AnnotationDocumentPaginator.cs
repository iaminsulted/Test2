using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Annotations.Storage;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.Annotations.Component;
using MS.Internal.Documents;

namespace System.Windows.Annotations
{
	// Token: 0x0200086A RID: 2154
	public sealed class AnnotationDocumentPaginator : DocumentPaginator
	{
		// Token: 0x06007EFC RID: 32508 RVA: 0x0031BCB3 File Offset: 0x0031ACB3
		public AnnotationDocumentPaginator(DocumentPaginator originalPaginator, Stream annotationStore) : this(originalPaginator, new XmlStreamStore(annotationStore), FlowDirection.LeftToRight)
		{
		}

		// Token: 0x06007EFD RID: 32509 RVA: 0x0031BCC3 File Offset: 0x0031ACC3
		public AnnotationDocumentPaginator(DocumentPaginator originalPaginator, Stream annotationStore, FlowDirection flowDirection) : this(originalPaginator, new XmlStreamStore(annotationStore), flowDirection)
		{
		}

		// Token: 0x06007EFE RID: 32510 RVA: 0x0031BCD3 File Offset: 0x0031ACD3
		public AnnotationDocumentPaginator(DocumentPaginator originalPaginator, AnnotationStore annotationStore) : this(originalPaginator, annotationStore, FlowDirection.LeftToRight)
		{
		}

		// Token: 0x06007EFF RID: 32511 RVA: 0x0031BCE0 File Offset: 0x0031ACE0
		public AnnotationDocumentPaginator(DocumentPaginator originalPaginator, AnnotationStore annotationStore, FlowDirection flowDirection)
		{
			this._isFixedContent = (originalPaginator is FixedDocumentPaginator || originalPaginator is FixedDocumentSequencePaginator);
			if (!this._isFixedContent && !(originalPaginator is FlowDocumentPaginator))
			{
				throw new ArgumentException(SR.Get("OnlyFlowAndFixedSupported"));
			}
			this._originalPaginator = originalPaginator;
			this._annotationStore = annotationStore;
			this._locatorManager = new LocatorManager(this._annotationStore);
			this._flowDirection = flowDirection;
			this._originalPaginator.GetPageCompleted += this.HandleGetPageCompleted;
			this._originalPaginator.ComputePageCountCompleted += this.HandleComputePageCountCompleted;
			this._originalPaginator.PagesChanged += this.HandlePagesChanged;
		}

		// Token: 0x17001D56 RID: 7510
		// (get) Token: 0x06007F00 RID: 32512 RVA: 0x0031BD98 File Offset: 0x0031AD98
		public override bool IsPageCountValid
		{
			get
			{
				return this._originalPaginator.IsPageCountValid;
			}
		}

		// Token: 0x17001D57 RID: 7511
		// (get) Token: 0x06007F01 RID: 32513 RVA: 0x0031BDA5 File Offset: 0x0031ADA5
		public override int PageCount
		{
			get
			{
				return this._originalPaginator.PageCount;
			}
		}

		// Token: 0x17001D58 RID: 7512
		// (get) Token: 0x06007F02 RID: 32514 RVA: 0x0031BDB2 File Offset: 0x0031ADB2
		// (set) Token: 0x06007F03 RID: 32515 RVA: 0x0031BDBF File Offset: 0x0031ADBF
		public override Size PageSize
		{
			get
			{
				return this._originalPaginator.PageSize;
			}
			set
			{
				this._originalPaginator.PageSize = value;
			}
		}

		// Token: 0x17001D59 RID: 7513
		// (get) Token: 0x06007F04 RID: 32516 RVA: 0x0031BDCD File Offset: 0x0031ADCD
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._originalPaginator.Source;
			}
		}

		// Token: 0x06007F05 RID: 32517 RVA: 0x0031BDDC File Offset: 0x0031ADDC
		public override DocumentPage GetPage(int pageNumber)
		{
			DocumentPage documentPage = this._originalPaginator.GetPage(pageNumber);
			if (documentPage != DocumentPage.Missing)
			{
				documentPage = this.ComposePageWithAnnotationVisuals(pageNumber, documentPage);
			}
			return documentPage;
		}

		// Token: 0x06007F06 RID: 32518 RVA: 0x0031BE08 File Offset: 0x0031AE08
		public override void GetPageAsync(int pageNumber, object userState)
		{
			this._originalPaginator.GetPageAsync(pageNumber, userState);
		}

		// Token: 0x06007F07 RID: 32519 RVA: 0x0031BE17 File Offset: 0x0031AE17
		public override void ComputePageCount()
		{
			this._originalPaginator.ComputePageCount();
		}

		// Token: 0x06007F08 RID: 32520 RVA: 0x0031BE24 File Offset: 0x0031AE24
		public override void ComputePageCountAsync(object userState)
		{
			this._originalPaginator.ComputePageCountAsync(userState);
		}

		// Token: 0x06007F09 RID: 32521 RVA: 0x0031BE32 File Offset: 0x0031AE32
		public override void CancelAsync(object userState)
		{
			this._originalPaginator.CancelAsync(userState);
		}

		// Token: 0x06007F0A RID: 32522 RVA: 0x0031BE40 File Offset: 0x0031AE40
		private void HandleGetPageCompleted(object sender, GetPageCompletedEventArgs e)
		{
			if (!e.Cancelled && e.Error == null && e.DocumentPage != DocumentPage.Missing)
			{
				e = new GetPageCompletedEventArgs(this.ComposePageWithAnnotationVisuals(e.PageNumber, e.DocumentPage), e.PageNumber, e.Error, e.Cancelled, e.UserState);
			}
			this.OnGetPageCompleted(e);
		}

		// Token: 0x06007F0B RID: 32523 RVA: 0x0031BEA2 File Offset: 0x0031AEA2
		private void HandleComputePageCountCompleted(object sender, AsyncCompletedEventArgs e)
		{
			this.OnComputePageCountCompleted(e);
		}

		// Token: 0x06007F0C RID: 32524 RVA: 0x0031BEAB File Offset: 0x0031AEAB
		private void HandlePagesChanged(object sender, PagesChangedEventArgs e)
		{
			this.OnPagesChanged(e);
		}

		// Token: 0x06007F0D RID: 32525 RVA: 0x0031BEB4 File Offset: 0x0031AEB4
		private DocumentPage ComposePageWithAnnotationVisuals(int pageNumber, DocumentPage page)
		{
			Size size = page.Size;
			AdornerDecorator adornerDecorator = new AdornerDecorator();
			adornerDecorator.FlowDirection = this._flowDirection;
			DocumentPageView documentPageView = new DocumentPageView();
			documentPageView.UseAsynchronousGetPage = false;
			documentPageView.DocumentPaginator = this._originalPaginator;
			documentPageView.PageNumber = pageNumber;
			adornerDecorator.Child = documentPageView;
			adornerDecorator.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			adornerDecorator.Arrange(new Rect(adornerDecorator.DesiredSize));
			adornerDecorator.UpdateLayout();
			AnnotationComponentManager annotationComponentManager = new AnnotationComponentManager(null);
			if (this._isFixedContent)
			{
				AnnotationService.SetSubTreeProcessorId(adornerDecorator, FixedPageProcessor.Id);
				this._locatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextRange));
			}
			else
			{
				AnnotationService.SetDataId(adornerDecorator, "FlowDocument");
				this._locatorManager.RegisterSelectionProcessor(new TextViewSelectionProcessor(), typeof(DocumentPageView));
				TextSelectionProcessor textSelectionProcessor = new TextSelectionProcessor();
				textSelectionProcessor.SetTargetDocumentPageView(documentPageView);
				this._locatorManager.RegisterSelectionProcessor(textSelectionProcessor, typeof(TextRange));
			}
			foreach (IAttachedAnnotation attachedAnnotation in this.ProcessAnnotations(documentPageView))
			{
				if (attachedAnnotation.AttachmentLevel != AttachmentLevel.Unresolved && attachedAnnotation.AttachmentLevel != AttachmentLevel.Incomplete)
				{
					annotationComponentManager.AddAttachedAnnotation(attachedAnnotation, false);
				}
			}
			adornerDecorator.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			adornerDecorator.Arrange(new Rect(adornerDecorator.DesiredSize));
			adornerDecorator.UpdateLayout();
			return new AnnotationDocumentPaginator.AnnotatedDocumentPage(page, adornerDecorator, size, new Rect(size), new Rect(size));
		}

		// Token: 0x06007F0E RID: 32526 RVA: 0x0031C060 File Offset: 0x0031B060
		private IList<IAttachedAnnotation> ProcessAnnotations(DocumentPageView dpv)
		{
			if (dpv == null)
			{
				throw new ArgumentNullException("dpv");
			}
			IList<IAttachedAnnotation> list = new List<IAttachedAnnotation>();
			IList<ContentLocatorBase> list2 = this._locatorManager.GenerateLocators(dpv);
			if (list2.Count > 0)
			{
				ContentLocator[] array = new ContentLocator[list2.Count];
				ICollection<ContentLocatorBase> collection = list2;
				ContentLocatorBase[] array2 = array;
				collection.CopyTo(array2, 0);
				IList<Annotation> annotations = this._annotationStore.GetAnnotations(array[0]);
				foreach (ContentLocatorBase contentLocatorBase in list2)
				{
					ContentLocator contentLocator = (ContentLocator)contentLocatorBase;
					if (contentLocator.Parts[contentLocator.Parts.Count - 1].NameValuePairs.ContainsKey("IncludeOverlaps"))
					{
						contentLocator.Parts.RemoveAt(contentLocator.Parts.Count - 1);
					}
				}
				foreach (Annotation annotation in annotations)
				{
					foreach (AnnotationResource annotationResource in annotation.Anchors)
					{
						foreach (ContentLocatorBase locator in annotationResource.ContentLocators)
						{
							AttachmentLevel attachmentLevel;
							object attachedAnchor = this._locatorManager.FindAttachedAnchor(dpv, array, locator, out attachmentLevel);
							if (attachmentLevel != AttachmentLevel.Unresolved)
							{
								Invariant.Assert(VisualTreeHelper.GetChildrenCount(dpv) == 1, "DocumentPageView has no visual children.");
								DependencyObject child = VisualTreeHelper.GetChild(dpv, 0);
								list.Add(new AttachedAnnotation(this._locatorManager, annotation, annotationResource, attachedAnchor, attachmentLevel, child as DocumentPageHost));
								break;
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x04003B65 RID: 15205
		private AnnotationStore _annotationStore;

		// Token: 0x04003B66 RID: 15206
		private DocumentPaginator _originalPaginator;

		// Token: 0x04003B67 RID: 15207
		private LocatorManager _locatorManager;

		// Token: 0x04003B68 RID: 15208
		private bool _isFixedContent;

		// Token: 0x04003B69 RID: 15209
		private FlowDirection _flowDirection;

		// Token: 0x02000C5A RID: 3162
		private class AnnotatedDocumentPage : DocumentPage, IContentHost
		{
			// Token: 0x060091D7 RID: 37335 RVA: 0x0034AE72 File Offset: 0x00349E72
			public AnnotatedDocumentPage(DocumentPage basePage, Visual visual, Size pageSize, Rect bleedBox, Rect contentBox) : base(visual, pageSize, bleedBox, contentBox)
			{
				this._basePage = (basePage as IContentHost);
			}

			// Token: 0x17001FEB RID: 8171
			// (get) Token: 0x060091D8 RID: 37336 RVA: 0x0034AE8C File Offset: 0x00349E8C
			public IEnumerator<IInputElement> HostedElements
			{
				get
				{
					if (this._basePage != null)
					{
						return this._basePage.HostedElements;
					}
					return new HostedElements(new ReadOnlyCollection<TextSegment>(new List<TextSegment>(0)));
				}
			}

			// Token: 0x060091D9 RID: 37337 RVA: 0x0034AEB2 File Offset: 0x00349EB2
			public ReadOnlyCollection<Rect> GetRectangles(ContentElement child)
			{
				if (this._basePage != null)
				{
					return this._basePage.GetRectangles(child);
				}
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}

			// Token: 0x060091DA RID: 37338 RVA: 0x0034AED4 File Offset: 0x00349ED4
			public IInputElement InputHitTest(Point point)
			{
				if (this._basePage != null)
				{
					return this._basePage.InputHitTest(point);
				}
				return null;
			}

			// Token: 0x060091DB RID: 37339 RVA: 0x0034AEEC File Offset: 0x00349EEC
			public void OnChildDesiredSizeChanged(UIElement child)
			{
				if (this._basePage != null)
				{
					this._basePage.OnChildDesiredSizeChanged(child);
				}
			}

			// Token: 0x04004C6E RID: 19566
			private IContentHost _basePage;
		}
	}
}
