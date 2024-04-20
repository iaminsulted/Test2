using System;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020001E8 RID: 488
	internal class RowInfo
	{
		// Token: 0x06001158 RID: 4440 RVA: 0x00143F8B File Offset: 0x00142F8B
		public RowInfo()
		{
			this._rowSize = new Size(0.0, 0.0);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00143FB0 File Offset: 0x00142FB0
		public void AddPage(Size pageSize)
		{
			this._pageCount++;
			this._rowSize.Width = this._rowSize.Width + pageSize.Width;
			this._rowSize.Height = Math.Max(pageSize.Height, this._rowSize.Height);
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00144006 File Offset: 0x00143006
		public void ClearPages()
		{
			this._pageCount = 0;
			this._rowSize.Width = 0.0;
			this._rowSize.Height = 0.0;
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x00144037 File Offset: 0x00143037
		public Size RowSize
		{
			get
			{
				return this._rowSize;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x0014403F File Offset: 0x0014303F
		// (set) Token: 0x0600115D RID: 4445 RVA: 0x00144047 File Offset: 0x00143047
		public double VerticalOffset
		{
			get
			{
				return this._verticalOffset;
			}
			set
			{
				this._verticalOffset = value;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00144050 File Offset: 0x00143050
		// (set) Token: 0x0600115F RID: 4447 RVA: 0x00144058 File Offset: 0x00143058
		public int FirstPage
		{
			get
			{
				return this._firstPage;
			}
			set
			{
				this._firstPage = value;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x00144061 File Offset: 0x00143061
		public int PageCount
		{
			get
			{
				return this._pageCount;
			}
		}

		// Token: 0x04000B0A RID: 2826
		private Size _rowSize;

		// Token: 0x04000B0B RID: 2827
		private double _verticalOffset;

		// Token: 0x04000B0C RID: 2828
		private int _firstPage;

		// Token: 0x04000B0D RID: 2829
		private int _pageCount;
	}
}
