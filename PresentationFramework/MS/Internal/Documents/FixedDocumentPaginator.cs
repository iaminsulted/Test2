using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001C3 RID: 451
	internal class FixedDocumentPaginator : DynamicDocumentPaginator, IServiceProvider
	{
		// Token: 0x06000F4F RID: 3919 RVA: 0x0013D5C1 File Offset: 0x0013C5C1
		internal FixedDocumentPaginator(FixedDocument document)
		{
			this._document = document;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0013D5D0 File Offset: 0x0013C5D0
		public override DocumentPage GetPage(int pageNumber)
		{
			return this._document.GetPage(pageNumber);
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0013D5DE File Offset: 0x0013C5DE
		public override void GetPageAsync(int pageNumber, object userState)
		{
			this._document.GetPageAsync(pageNumber, userState);
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0013D5ED File Offset: 0x0013C5ED
		public override void CancelAsync(object userState)
		{
			this._document.CancelAsync(userState);
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0013D5FB File Offset: 0x0013C5FB
		public override int GetPageNumber(ContentPosition contentPosition)
		{
			return this._document.GetPageNumber(contentPosition);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0013D609 File Offset: 0x0013C609
		public override ContentPosition GetPagePosition(DocumentPage page)
		{
			return this._document.GetPagePosition(page);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0013D617 File Offset: 0x0013C617
		public override ContentPosition GetObjectPosition(object o)
		{
			return this._document.GetObjectPosition(o);
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0013D625 File Offset: 0x0013C625
		public override bool IsPageCountValid
		{
			get
			{
				return this._document.IsPageCountValid;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0013D632 File Offset: 0x0013C632
		public override int PageCount
		{
			get
			{
				return this._document.PageCount;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x0013D63F File Offset: 0x0013C63F
		// (set) Token: 0x06000F59 RID: 3929 RVA: 0x0013D64C File Offset: 0x0013C64C
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

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x0013D65A File Offset: 0x0013C65A
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._document;
			}
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0013D662 File Offset: 0x0013C662
		internal void NotifyGetPageCompleted(GetPageCompletedEventArgs e)
		{
			this.OnGetPageCompleted(e);
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0013D66B File Offset: 0x0013C66B
		internal void NotifyPaginationCompleted(EventArgs e)
		{
			this.OnPaginationCompleted(e);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0013D674 File Offset: 0x0013C674
		internal void NotifyPaginationProgress(PaginationProgressEventArgs e)
		{
			this.OnPaginationProgress(e);
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0013D67D File Offset: 0x0013C67D
		internal void NotifyPagesChanged(PagesChangedEventArgs e)
		{
			this.OnPagesChanged(e);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0013D686 File Offset: 0x0013C686
		object IServiceProvider.GetService(Type serviceType)
		{
			return ((IServiceProvider)this._document).GetService(serviceType);
		}

		// Token: 0x04000A9B RID: 2715
		private readonly FixedDocument _document;
	}
}
