using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x0200064D RID: 1613
	public sealed class PageContentCollection : IEnumerable<PageContent>, IEnumerable
	{
		// Token: 0x06004FF1 RID: 20465 RVA: 0x00244F7B File Offset: 0x00243F7B
		internal PageContentCollection(FixedDocument logicalParent)
		{
			this._logicalParent = logicalParent;
			this._internalList = new List<PageContent>();
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x00244F98 File Offset: 0x00243F98
		public int Add(PageContent newPageContent)
		{
			if (newPageContent == null)
			{
				throw new ArgumentNullException("newPageContent");
			}
			this._logicalParent.AddLogicalChild(newPageContent);
			this.InternalList.Add(newPageContent);
			int num = this.InternalList.Count - 1;
			this._logicalParent.OnPageContentAppended(num);
			return num;
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x00244FE6 File Offset: 0x00243FE6
		public IEnumerator<PageContent> GetEnumerator()
		{
			return this.InternalList.GetEnumerator();
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x00244FF3 File Offset: 0x00243FF3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PageContent>)this).GetEnumerator();
		}

		// Token: 0x1700128F RID: 4751
		public PageContent this[int pageIndex]
		{
			get
			{
				return this.InternalList[pageIndex];
			}
		}

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06004FF6 RID: 20470 RVA: 0x00245009 File Offset: 0x00244009
		public int Count
		{
			get
			{
				return this.InternalList.Count;
			}
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x00245016 File Offset: 0x00244016
		internal int IndexOf(PageContent pc)
		{
			return this.InternalList.IndexOf(pc);
		}

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06004FF8 RID: 20472 RVA: 0x00245024 File Offset: 0x00244024
		private IList<PageContent> InternalList
		{
			get
			{
				return this._internalList;
			}
		}

		// Token: 0x04002877 RID: 10359
		private FixedDocument _logicalParent;

		// Token: 0x04002878 RID: 10360
		private List<PageContent> _internalList;
	}
}
