using System;
using System.IO;
using System.Windows.Markup;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.Utility;

namespace System.Windows.Documents
{
	// Token: 0x020005E4 RID: 1508
	[UsableDuringInitialization(false)]
	public sealed class DocumentReference : FrameworkElement, IUriContext
	{
		// Token: 0x060048CB RID: 18635 RVA: 0x0022E38E File Offset: 0x0022D38E
		public DocumentReference()
		{
			this._Init();
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x0022E39C File Offset: 0x0022D39C
		public FixedDocument GetDocument(bool forceReload)
		{
			base.VerifyAccess();
			FixedDocument fixedDocument = null;
			if (this._doc != null)
			{
				fixedDocument = this._doc;
			}
			else
			{
				if (!forceReload)
				{
					fixedDocument = this.CurrentlyLoadedDoc;
				}
				if (fixedDocument == null)
				{
					FixedDocument fixedDocument2 = this._LoadDocument();
					if (fixedDocument2 != null)
					{
						this._docIdentity = fixedDocument2;
						fixedDocument = fixedDocument2;
					}
				}
			}
			if (fixedDocument != null)
			{
				LogicalTreeHelper.AddLogicalChild(base.Parent, fixedDocument);
			}
			return fixedDocument;
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x0022E3F2 File Offset: 0x0022D3F2
		public void SetDocument(FixedDocument doc)
		{
			base.VerifyAccess();
			this._docIdentity = null;
			this._doc = doc;
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x0022E408 File Offset: 0x0022D408
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentReference documentReference = (DocumentReference)d;
			if (!object.Equals(e.OldValue, e.NewValue))
			{
				Uri uri = (Uri)e.OldValue;
				Uri uri2 = (Uri)e.NewValue;
				documentReference._doc = null;
			}
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x060048CF RID: 18639 RVA: 0x0022E452 File Offset: 0x0022D452
		// (set) Token: 0x060048D0 RID: 18640 RVA: 0x0022E464 File Offset: 0x0022D464
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(DocumentReference.SourceProperty);
			}
			set
			{
				base.SetValue(DocumentReference.SourceProperty, value);
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x060048D2 RID: 18642 RVA: 0x0022A4F5 File Offset: 0x002294F5
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

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x060048D3 RID: 18643 RVA: 0x0022E472 File Offset: 0x0022D472
		internal FixedDocument CurrentlyLoadedDoc
		{
			get
			{
				return this._docIdentity;
			}
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x0022E47A File Offset: 0x0022D47A
		private void _Init()
		{
			base.InheritanceBehavior = InheritanceBehavior.SkipToAppNow;
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x0022E484 File Offset: 0x0022D484
		private Uri _ResolveUri()
		{
			Uri uri = this.Source;
			if (uri != null)
			{
				uri = BindUriHelper.GetUriToNavigate(this, ((IUriContext)this).BaseUri, uri);
			}
			return uri;
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x0022E4B0 File Offset: 0x0022D4B0
		private FixedDocument _LoadDocument()
		{
			FixedDocument fixedDocument = null;
			Uri uri = this._ResolveUri();
			if (uri != null)
			{
				ContentType contentType = null;
				Stream stream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(uri, out contentType);
				if (stream == null)
				{
					throw new ApplicationException(SR.Get("DocumentReferenceNotFound"));
				}
				ParserContext parserContext = new ParserContext();
				parserContext.BaseUri = uri;
				if (BindUriHelper.IsXamlMimeType(contentType))
				{
					fixedDocument = (new XpsValidatingLoader().Load(stream, ((IUriContext)this).BaseUri, parserContext, contentType) as FixedDocument);
				}
				else
				{
					if (!MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
					{
						throw new ApplicationException(SR.Get("DocumentReferenceUnsupportedMimeType"));
					}
					fixedDocument = (XamlReader.LoadBaml(stream, parserContext, null, true) as FixedDocument);
				}
				fixedDocument.DocumentReference = this;
			}
			return fixedDocument;
		}

		// Token: 0x0400264C RID: 9804
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(DocumentReference), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DocumentReference.OnSourceChanged)));

		// Token: 0x0400264D RID: 9805
		private FixedDocument _doc;

		// Token: 0x0400264E RID: 9806
		private FixedDocument _docIdentity;
	}
}
