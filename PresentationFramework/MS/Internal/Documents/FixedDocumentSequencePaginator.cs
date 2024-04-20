using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001C4 RID: 452
	internal class FixedDocumentSequencePaginator : DynamicDocumentPaginator, IServiceProvider
	{
		// Token: 0x06000F60 RID: 3936 RVA: 0x0013D694 File Offset: 0x0013C694
		internal FixedDocumentSequencePaginator(FixedDocumentSequence document)
		{
			this._document = document;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0013D6A3 File Offset: 0x0013C6A3
		public override DocumentPage GetPage(int pageNumber)
		{
			return this._document.GetPage(pageNumber);
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0013D6B1 File Offset: 0x0013C6B1
		public override void GetPageAsync(int pageNumber, object userState)
		{
			this._document.GetPageAsync(pageNumber, userState);
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0013D6C0 File Offset: 0x0013C6C0
		public override void CancelAsync(object userState)
		{
			this._document.CancelAsync(userState);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0013D6CE File Offset: 0x0013C6CE
		public override int GetPageNumber(ContentPosition contentPosition)
		{
			return this._document.GetPageNumber(contentPosition);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0013D6DC File Offset: 0x0013C6DC
		public override ContentPosition GetPagePosition(DocumentPage page)
		{
			return this._document.GetPagePosition(page);
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0013D6EA File Offset: 0x0013C6EA
		public override ContentPosition GetObjectPosition(object o)
		{
			return this._document.GetObjectPosition(o);
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0013D6F8 File Offset: 0x0013C6F8
		public override bool IsPageCountValid
		{
			get
			{
				return this._document.IsPageCountValid;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0013D705 File Offset: 0x0013C705
		public override int PageCount
		{
			get
			{
				return this._document.PageCount;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0013D712 File Offset: 0x0013C712
		// (set) Token: 0x06000F6A RID: 3946 RVA: 0x0013D71F File Offset: 0x0013C71F
		public override Size PageSize
		{
			get
			{
				return this._document.PageSize;
			}
			set
			{
				this._document.PageSize = value;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x0013D72D File Offset: 0x0013C72D
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._document;
			}
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0013D662 File Offset: 0x0013C662
		internal void NotifyGetPageCompleted(GetPageCompletedEventArgs e)
		{
			this.OnGetPageCompleted(e);
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0013D66B File Offset: 0x0013C66B
		internal void NotifyPaginationCompleted(EventArgs e)
		{
			this.OnPaginationCompleted(e);
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0013D674 File Offset: 0x0013C674
		internal void NotifyPaginationProgress(PaginationProgressEventArgs e)
		{
			this.OnPaginationProgress(e);
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0013D67D File Offset: 0x0013C67D
		internal void NotifyPagesChanged(PagesChangedEventArgs e)
		{
			this.OnPagesChanged(e);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0013D735 File Offset: 0x0013C735
		object IServiceProvider.GetService(Type serviceType)
		{
			return ((IServiceProvider)this._document).GetService(serviceType);
		}

		// Token: 0x04000A9C RID: 2716
		private readonly FixedDocumentSequence _document;
	}
}
