using System;
using System.Globalization;

namespace System.Windows.Controls
{
	// Token: 0x020007B8 RID: 1976
	public struct PageRange
	{
		// Token: 0x06007035 RID: 28725 RVA: 0x002D702F File Offset: 0x002D602F
		public PageRange(int page)
		{
			this._pageFrom = page;
			this._pageTo = page;
		}

		// Token: 0x06007036 RID: 28726 RVA: 0x002D703F File Offset: 0x002D603F
		public PageRange(int pageFrom, int pageTo)
		{
			this._pageFrom = pageFrom;
			this._pageTo = pageTo;
		}

		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x06007037 RID: 28727 RVA: 0x002D704F File Offset: 0x002D604F
		// (set) Token: 0x06007038 RID: 28728 RVA: 0x002D7057 File Offset: 0x002D6057
		public int PageFrom
		{
			get
			{
				return this._pageFrom;
			}
			set
			{
				this._pageFrom = value;
			}
		}

		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x06007039 RID: 28729 RVA: 0x002D7060 File Offset: 0x002D6060
		// (set) Token: 0x0600703A RID: 28730 RVA: 0x002D7068 File Offset: 0x002D6068
		public int PageTo
		{
			get
			{
				return this._pageTo;
			}
			set
			{
				this._pageTo = value;
			}
		}

		// Token: 0x0600703B RID: 28731 RVA: 0x002D7074 File Offset: 0x002D6074
		public override string ToString()
		{
			string result;
			if (this._pageTo != this._pageFrom)
			{
				result = string.Format(CultureInfo.InvariantCulture, SR.Get("PrintDialogPageRange"), this._pageFrom, this._pageTo);
			}
			else
			{
				result = this._pageFrom.ToString(CultureInfo.InvariantCulture);
			}
			return result;
		}

		// Token: 0x0600703C RID: 28732 RVA: 0x002D70CE File Offset: 0x002D60CE
		public override bool Equals(object obj)
		{
			return obj != null && !(obj.GetType() != typeof(PageRange)) && this.Equals((PageRange)obj);
		}

		// Token: 0x0600703D RID: 28733 RVA: 0x002D70F8 File Offset: 0x002D60F8
		public bool Equals(PageRange pageRange)
		{
			return pageRange.PageFrom == this.PageFrom && pageRange.PageTo == this.PageTo;
		}

		// Token: 0x0600703E RID: 28734 RVA: 0x002D711A File Offset: 0x002D611A
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600703F RID: 28735 RVA: 0x002D712C File Offset: 0x002D612C
		public static bool operator ==(PageRange pr1, PageRange pr2)
		{
			return pr1.Equals(pr2);
		}

		// Token: 0x06007040 RID: 28736 RVA: 0x002D7136 File Offset: 0x002D6136
		public static bool operator !=(PageRange pr1, PageRange pr2)
		{
			return !pr1.Equals(pr2);
		}

		// Token: 0x040036DD RID: 14045
		private int _pageFrom;

		// Token: 0x040036DE RID: 14046
		private int _pageTo;
	}
}
