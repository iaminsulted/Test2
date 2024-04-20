using System;
using System.Collections;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001D6 RID: 470
	internal class PageDestroyedWatcher
	{
		// Token: 0x060010CC RID: 4300 RVA: 0x00141EB2 File Offset: 0x00140EB2
		public PageDestroyedWatcher()
		{
			this._table = new Hashtable(16);
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00141EC8 File Offset: 0x00140EC8
		public void AddPage(DocumentPage page)
		{
			if (!this._table.Contains(page))
			{
				this._table.Add(page, false);
				page.PageDestroyed += this.OnPageDestroyed;
				return;
			}
			this._table[page] = false;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00141F1A File Offset: 0x00140F1A
		public void RemovePage(DocumentPage page)
		{
			if (this._table.Contains(page))
			{
				this._table.Remove(page);
				page.PageDestroyed -= this.OnPageDestroyed;
			}
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00141F48 File Offset: 0x00140F48
		public bool IsDestroyed(DocumentPage page)
		{
			return !this._table.Contains(page) || (bool)this._table[page];
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00141F6C File Offset: 0x00140F6C
		private void OnPageDestroyed(object sender, EventArgs e)
		{
			DocumentPage documentPage = sender as DocumentPage;
			Invariant.Assert(documentPage != null, "Invalid type in PageDestroyedWatcher");
			if (this._table.Contains(documentPage))
			{
				this._table[documentPage] = true;
			}
		}

		// Token: 0x04000AD5 RID: 2773
		private Hashtable _table;
	}
}
