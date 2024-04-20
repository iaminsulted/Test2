using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020005E7 RID: 1511
	internal sealed class FixedDocumentSequenceDocumentPage : DocumentPage, IServiceProvider
	{
		// Token: 0x0600490B RID: 18699 RVA: 0x0022F3CC File Offset: 0x0022E3CC
		internal FixedDocumentSequenceDocumentPage(FixedDocumentSequence documentSequence, DynamicDocumentPaginator documentPaginator, DocumentPage documentPage) : base((documentPage is FixedDocumentPage) ? ((FixedDocumentPage)documentPage).FixedPage : documentPage.Visual, documentPage.Size, documentPage.BleedBox, documentPage.ContentBox)
		{
			this._fixedDocumentSequence = documentSequence;
			this._documentPaginator = documentPaginator;
			this._documentPage = documentPage;
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x0022F424 File Offset: 0x0022E424
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				if (this._textView == null)
				{
					this._textView = new DocumentSequenceTextView(this);
				}
				return this._textView;
			}
			return null;
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x0600490D RID: 18701 RVA: 0x0022F474 File Offset: 0x0022E474
		public override Visual Visual
		{
			get
			{
				if (!this._layedOut)
				{
					this._layedOut = true;
					UIElement uielement;
					if ((uielement = (base.Visual as UIElement)) != null)
					{
						uielement.Measure(base.Size);
						uielement.Arrange(new Rect(base.Size));
					}
				}
				return base.Visual;
			}
		}

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x0600490E RID: 18702 RVA: 0x0022F4C4 File Offset: 0x0022E4C4
		internal ContentPosition ContentPosition
		{
			get
			{
				ITextPointer textPointer = this._documentPaginator.GetPagePosition(this._documentPage) as ITextPointer;
				if (textPointer != null)
				{
					return new DocumentSequenceTextPointer(new ChildDocumentBlock(this._fixedDocumentSequence.TextContainer, this.ChildDocumentReference), textPointer);
				}
				return null;
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x0600490F RID: 18703 RVA: 0x0022F50C File Offset: 0x0022E50C
		internal DocumentReference ChildDocumentReference
		{
			get
			{
				foreach (DocumentReference documentReference in this._fixedDocumentSequence.References)
				{
					if (documentReference.CurrentlyLoadedDoc == this._documentPaginator.Source)
					{
						return documentReference;
					}
				}
				return null;
			}
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06004910 RID: 18704 RVA: 0x0022F574 File Offset: 0x0022E574
		internal DocumentPage ChildDocumentPage
		{
			get
			{
				return this._documentPage;
			}
		}

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06004911 RID: 18705 RVA: 0x0022F57C File Offset: 0x0022E57C
		internal FixedDocumentSequence FixedDocumentSequence
		{
			get
			{
				return this._fixedDocumentSequence;
			}
		}

		// Token: 0x0400265C RID: 9820
		private readonly FixedDocumentSequence _fixedDocumentSequence;

		// Token: 0x0400265D RID: 9821
		private readonly DynamicDocumentPaginator _documentPaginator;

		// Token: 0x0400265E RID: 9822
		private readonly DocumentPage _documentPage;

		// Token: 0x0400265F RID: 9823
		private bool _layedOut;

		// Token: 0x04002660 RID: 9824
		private DocumentSequenceTextView _textView;
	}
}
