using System;
using System.Collections.Generic;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020001E7 RID: 487
	internal class RowCache
	{
		// Token: 0x06001132 RID: 4402 RVA: 0x00142C94 File Offset: 0x00141C94
		public RowCache()
		{
			this._rowCache = new List<RowInfo>(this._defaultRowCacheSize);
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x00142D81 File Offset: 0x00141D81
		// (set) Token: 0x06001133 RID: 4403 RVA: 0x00142CE8 File Offset: 0x00141CE8
		public PageCache PageCache
		{
			get
			{
				return this._pageCache;
			}
			set
			{
				this._rowCache.Clear();
				this._isLayoutCompleted = false;
				this._isLayoutRequested = false;
				if (this._pageCache != null)
				{
					this._pageCache.PageCacheChanged -= this.OnPageCacheChanged;
					this._pageCache.PaginationCompleted -= this.OnPaginationCompleted;
				}
				this._pageCache = value;
				if (this._pageCache != null)
				{
					this._pageCache.PageCacheChanged += this.OnPageCacheChanged;
					this._pageCache.PaginationCompleted += this.OnPaginationCompleted;
				}
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x00142D89 File Offset: 0x00141D89
		public int RowCount
		{
			get
			{
				return this._rowCache.Count;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00142DC5 File Offset: 0x00141DC5
		// (set) Token: 0x06001136 RID: 4406 RVA: 0x00142D96 File Offset: 0x00141D96
		public double VerticalPageSpacing
		{
			get
			{
				return this._verticalPageSpacing;
			}
			set
			{
				if (value < 0.0)
				{
					value = 0.0;
				}
				if (value != this._verticalPageSpacing)
				{
					this._verticalPageSpacing = value;
					this.RecalcLayoutForScaleOrSpacing();
				}
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x00142DFC File Offset: 0x00141DFC
		// (set) Token: 0x06001138 RID: 4408 RVA: 0x00142DCD File Offset: 0x00141DCD
		public double HorizontalPageSpacing
		{
			get
			{
				return this._horizontalPageSpacing;
			}
			set
			{
				if (value < 0.0)
				{
					value = 0.0;
				}
				if (value != this._horizontalPageSpacing)
				{
					this._horizontalPageSpacing = value;
					this.RecalcLayoutForScaleOrSpacing();
				}
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x00142E1C File Offset: 0x00141E1C
		// (set) Token: 0x0600113A RID: 4410 RVA: 0x00142E04 File Offset: 0x00141E04
		public double Scale
		{
			get
			{
				return this._scale;
			}
			set
			{
				if (this._scale != value)
				{
					this._scale = value;
					this.RecalcLayoutForScaleOrSpacing();
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x00142E24 File Offset: 0x00141E24
		public double ExtentHeight
		{
			get
			{
				return this._extentHeight;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x00142E2C File Offset: 0x00141E2C
		public double ExtentWidth
		{
			get
			{
				return this._extentWidth;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x00142E34 File Offset: 0x00141E34
		public bool HasValidLayout
		{
			get
			{
				return this._hasValidLayout;
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x0600113F RID: 4415 RVA: 0x00142E3C File Offset: 0x00141E3C
		// (remove) Token: 0x06001140 RID: 4416 RVA: 0x00142E74 File Offset: 0x00141E74
		public event RowCacheChangedEventHandler RowCacheChanged;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001141 RID: 4417 RVA: 0x00142EAC File Offset: 0x00141EAC
		// (remove) Token: 0x06001142 RID: 4418 RVA: 0x00142EE4 File Offset: 0x00141EE4
		public event RowLayoutCompletedEventHandler RowLayoutCompleted;

		// Token: 0x06001143 RID: 4419 RVA: 0x00142F19 File Offset: 0x00141F19
		public RowInfo GetRow(int index)
		{
			if (index < 0 || index > this._rowCache.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._rowCache[index];
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00142F44 File Offset: 0x00141F44
		public RowInfo GetRowForPageNumber(int pageNumber)
		{
			if (pageNumber < 0 || pageNumber > this.LastPageInCache)
			{
				throw new ArgumentOutOfRangeException("pageNumber");
			}
			return this._rowCache[this.GetRowIndexForPageNumber(pageNumber)];
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00142F70 File Offset: 0x00141F70
		public int GetRowIndexForPageNumber(int pageNumber)
		{
			if (pageNumber < 0 || pageNumber > this.LastPageInCache)
			{
				throw new ArgumentOutOfRangeException("pageNumber");
			}
			for (int i = 0; i < this._rowCache.Count; i++)
			{
				RowInfo rowInfo = this._rowCache[i];
				if (pageNumber >= rowInfo.FirstPage && pageNumber < rowInfo.FirstPage + rowInfo.PageCount)
				{
					return i;
				}
			}
			throw new InvalidOperationException(SR.Get("RowCachePageNotFound"));
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00142FE4 File Offset: 0x00141FE4
		public int GetRowIndexForVerticalOffset(double offset)
		{
			if (offset < 0.0 || offset > this.ExtentHeight)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (this._rowCache.Count == 0)
			{
				return 0;
			}
			double num = Math.Round(offset, this._findOffsetPrecision);
			int i = 0;
			while (i < this._rowCache.Count)
			{
				double num2 = Math.Round(this._rowCache[i].VerticalOffset, this._findOffsetPrecision);
				double num3 = Math.Round(this._rowCache[i].RowSize.Height, this._findOffsetPrecision);
				bool flag = false;
				if (DoubleUtil.AreClose(num2, num2 + num3))
				{
					flag = true;
				}
				if (flag && DoubleUtil.AreClose(num, num2))
				{
					return i;
				}
				if (num >= num2 && num < num2 + num3)
				{
					if (this.WithinVisibleDelta(num2 + num3, num) || i == this._rowCache.Count - 1)
					{
						return i;
					}
					return i + 1;
				}
				else
				{
					i++;
				}
			}
			DoubleUtil.AreClose(offset, this.ExtentHeight);
			return this._rowCache.Count - 1;
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x001430F4 File Offset: 0x001420F4
		public void GetVisibleRowIndices(double startOffset, double endOffset, out int startRowIndex, out int rowCount)
		{
			startRowIndex = 0;
			rowCount = 0;
			if (endOffset < startOffset)
			{
				throw new ArgumentOutOfRangeException("endOffset");
			}
			if (startOffset < 0.0 || startOffset > this.ExtentHeight)
			{
				return;
			}
			if (this._rowCache.Count == 0)
			{
				return;
			}
			startRowIndex = this.GetRowIndexForVerticalOffset(startOffset);
			rowCount = 1;
			startOffset = Math.Round(startOffset, this._findOffsetPrecision);
			endOffset = Math.Round(endOffset, this._findOffsetPrecision);
			for (int i = startRowIndex + 1; i < this._rowCache.Count; i++)
			{
				double num = Math.Round(this._rowCache[i].VerticalOffset, this._findOffsetPrecision);
				if (num >= endOffset || !this.WithinVisibleDelta(endOffset, num))
				{
					break;
				}
				rowCount++;
			}
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x001431B0 File Offset: 0x001421B0
		public void RecalcLayoutForScaleOrSpacing()
		{
			if (this.PageCache == null)
			{
				throw new InvalidOperationException(SR.Get("RowCacheRecalcWithNoPageCache"));
			}
			this._extentWidth = 0.0;
			this._extentHeight = 0.0;
			double num = 0.0;
			for (int i = 0; i < this._rowCache.Count; i++)
			{
				RowInfo rowInfo = this._rowCache[i];
				int pageCount = rowInfo.PageCount;
				rowInfo.ClearPages();
				rowInfo.VerticalOffset = num;
				for (int j = rowInfo.FirstPage; j < rowInfo.FirstPage + pageCount; j++)
				{
					Size scaledPageSize = this.GetScaledPageSize(j);
					rowInfo.AddPage(scaledPageSize);
				}
				this._extentWidth = Math.Max(rowInfo.RowSize.Width, this._extentWidth);
				num += rowInfo.RowSize.Height;
				this._extentHeight += rowInfo.RowSize.Height;
				this._rowCache[i] = rowInfo;
			}
			RowCacheChangedEventArgs e = new RowCacheChangedEventArgs(new List<RowCacheChange>(1)
			{
				new RowCacheChange(0, this._rowCache.Count)
			});
			this.RowCacheChanged(this, e);
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x001432F8 File Offset: 0x001422F8
		public void RecalcRows(int pivotPage, int columns)
		{
			if (this.PageCache == null)
			{
				throw new InvalidOperationException(SR.Get("RowCacheRecalcWithNoPageCache"));
			}
			if (pivotPage < 0 || pivotPage > this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("pivotPage");
			}
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			this._layoutColumns = columns;
			this._layoutPivotPage = pivotPage;
			this._hasValidLayout = false;
			if (this.PageCache.PageCount < this._layoutColumns)
			{
				if (!this.PageCache.IsPaginationCompleted || this.PageCache.PageCount == 0)
				{
					this._isLayoutRequested = true;
					this._isLayoutCompleted = false;
					return;
				}
				this._layoutColumns = Math.Min(this._layoutColumns, this.PageCache.PageCount);
				this._layoutColumns = Math.Max(1, this._layoutColumns);
				this._layoutPivotPage = 0;
			}
			this._extentHeight = 0.0;
			this._extentWidth = 0.0;
			if (this.PageCache.DynamicPageSizes)
			{
				this._pivotRowIndex = this.RecalcRowsForDynamicPageSizes(this._layoutPivotPage, this._layoutColumns);
			}
			else
			{
				this._pivotRowIndex = this.RecalcRowsForFixedPageSizes(this._layoutPivotPage, this._layoutColumns);
			}
			this._isLayoutCompleted = true;
			this._isLayoutRequested = false;
			this._hasValidLayout = true;
			RowLayoutCompletedEventArgs e = new RowLayoutCompletedEventArgs(this._pivotRowIndex);
			this.RowLayoutCompleted(this, e);
			RowCacheChangedEventArgs e2 = new RowCacheChangedEventArgs(new List<RowCacheChange>(1)
			{
				new RowCacheChange(0, this._rowCache.Count)
			});
			this.RowCacheChanged(this, e2);
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x0014348C File Offset: 0x0014248C
		private int LastPageInCache
		{
			get
			{
				if (this._rowCache.Count == 0)
				{
					return -1;
				}
				RowInfo rowInfo = this._rowCache[this._rowCache.Count - 1];
				return rowInfo.FirstPage + rowInfo.PageCount - 1;
			}
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x001434D0 File Offset: 0x001424D0
		private bool WithinVisibleDelta(double offset1, double offset2)
		{
			return offset1 - offset2 > this._visibleDelta;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x001434E0 File Offset: 0x001424E0
		private int RecalcRowsForDynamicPageSizes(int pivotPage, int columns)
		{
			if (pivotPage < 0 || pivotPage >= this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("pivotPage");
			}
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			if (pivotPage + columns > this.PageCache.PageCount)
			{
				pivotPage = Math.Max(0, this.PageCache.PageCount - columns);
			}
			this._rowCache.Clear();
			RowInfo rowInfo = this.CreateFixedRow(pivotPage, columns);
			double width = rowInfo.RowSize.Width;
			List<RowInfo> list = new List<RowInfo>(pivotPage / columns);
			int i = pivotPage;
			while (i > 0)
			{
				RowInfo rowInfo2 = this.CreateDynamicRow(i - 1, width, false);
				i = rowInfo2.FirstPage;
				list.Add(rowInfo2);
			}
			for (int j = list.Count - 1; j >= 0; j--)
			{
				this.AddRow(list[j]);
			}
			int count = this._rowCache.Count;
			this.AddRow(rowInfo);
			i = pivotPage + columns;
			while (i < this.PageCache.PageCount)
			{
				RowInfo rowInfo3 = this.CreateDynamicRow(i, width, true);
				i += rowInfo3.PageCount;
				this.AddRow(rowInfo3);
			}
			return count;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00143608 File Offset: 0x00142608
		private RowInfo CreateDynamicRow(int startPage, double rowWidth, bool createForward)
		{
			if (startPage >= this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("startPage");
			}
			RowInfo rowInfo = new RowInfo();
			Size scaledPageSize = this.GetScaledPageSize(startPage);
			rowInfo.AddPage(scaledPageSize);
			do
			{
				if (createForward)
				{
					scaledPageSize = this.GetScaledPageSize(startPage + rowInfo.PageCount);
					if (startPage + rowInfo.PageCount >= this.PageCache.PageCount)
					{
						break;
					}
					if (rowInfo.RowSize.Width + scaledPageSize.Width > rowWidth)
					{
						break;
					}
				}
				else
				{
					scaledPageSize = this.GetScaledPageSize(startPage - rowInfo.PageCount);
					if (startPage - rowInfo.PageCount < 0 || rowInfo.RowSize.Width + scaledPageSize.Width > rowWidth)
					{
						break;
					}
				}
				rowInfo.AddPage(scaledPageSize);
			}
			while (rowInfo.PageCount != DocumentViewerConstants.MaximumMaxPagesAcross);
			if (!createForward)
			{
				rowInfo.FirstPage = startPage - (rowInfo.PageCount - 1);
			}
			else
			{
				rowInfo.FirstPage = startPage;
			}
			return rowInfo;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x001436EC File Offset: 0x001426EC
		private int RecalcRowsForFixedPageSizes(int startPage, int columns)
		{
			if (startPage < 0 || startPage > this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("startPage");
			}
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			this._rowCache.Clear();
			for (int i = 0; i < this.PageCache.PageCount; i += columns)
			{
				RowInfo newRow = this.CreateFixedRow(i, columns);
				this.AddRow(newRow);
			}
			return this.GetRowIndexForPageNumber(startPage);
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00143760 File Offset: 0x00142760
		private RowInfo CreateFixedRow(int startPage, int columns)
		{
			if (startPage >= this.PageCache.PageCount)
			{
				throw new ArgumentOutOfRangeException("startPage");
			}
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			RowInfo rowInfo = new RowInfo();
			rowInfo.FirstPage = startPage;
			int num = startPage;
			while (num < startPage + columns && num <= this.PageCache.PageCount - 1)
			{
				Size scaledPageSize = this.GetScaledPageSize(num);
				rowInfo.AddPage(scaledPageSize);
				num++;
			}
			return rowInfo;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x001437D0 File Offset: 0x001427D0
		private RowCacheChange AddPageRange(int startPage, int count)
		{
			if (!this._isLayoutCompleted)
			{
				throw new InvalidOperationException(SR.Get("RowCacheCannotModifyNonExistentLayout"));
			}
			int i = startPage;
			int num = startPage + count;
			int num2 = 0;
			if (startPage > this.LastPageInCache + 1)
			{
				i = this.LastPageInCache + 1;
			}
			RowInfo rowInfo = this._rowCache[this._rowCache.Count - 1];
			Size scaledPageSize = this.GetScaledPageSize(i);
			RowInfo row = this.GetRow(this._pivotRowIndex);
			bool flag = false;
			while (i < num && rowInfo.RowSize.Width + scaledPageSize.Width <= row.RowSize.Width)
			{
				rowInfo.AddPage(scaledPageSize);
				i++;
				scaledPageSize = this.GetScaledPageSize(i);
				flag = true;
			}
			int num3;
			if (flag)
			{
				num3 = this._rowCache.Count - 1;
				this.UpdateRow(num3, rowInfo);
			}
			else
			{
				num3 = this._rowCache.Count;
			}
			while (i < num)
			{
				RowInfo rowInfo2 = new RowInfo();
				rowInfo2.FirstPage = i;
				do
				{
					scaledPageSize = this.GetScaledPageSize(i);
					rowInfo2.AddPage(scaledPageSize);
					i++;
				}
				while (rowInfo2.RowSize.Width + scaledPageSize.Width <= row.RowSize.Width && i < num);
				this.AddRow(rowInfo2);
				num2++;
			}
			return new RowCacheChange(num3, num2);
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00143928 File Offset: 0x00142928
		private void AddRow(RowInfo newRow)
		{
			if (this._rowCache.Count == 0)
			{
				newRow.VerticalOffset = 0.0;
				this._extentWidth = newRow.RowSize.Width;
			}
			else
			{
				RowInfo rowInfo = this._rowCache[this._rowCache.Count - 1];
				newRow.VerticalOffset = rowInfo.VerticalOffset + rowInfo.RowSize.Height;
				this._extentWidth = Math.Max(newRow.RowSize.Width, this._extentWidth);
			}
			this._extentHeight += newRow.RowSize.Height;
			this._rowCache.Add(newRow);
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x001439E4 File Offset: 0x001429E4
		private RowCacheChange UpdatePageRange(int startPage, int count)
		{
			if (!this._isLayoutCompleted)
			{
				throw new InvalidOperationException(SR.Get("RowCacheCannotModifyNonExistentLayout"));
			}
			int rowIndexForPageNumber = this.GetRowIndexForPageNumber(startPage);
			int num = rowIndexForPageNumber;
			int num2 = startPage;
			while (num2 < startPage + count && num < this._rowCache.Count)
			{
				RowInfo rowInfo = this._rowCache[num];
				RowInfo rowInfo2 = new RowInfo();
				rowInfo2.VerticalOffset = rowInfo.VerticalOffset;
				rowInfo2.FirstPage = rowInfo.FirstPage;
				for (int i = rowInfo.FirstPage; i < rowInfo.FirstPage + rowInfo.PageCount; i++)
				{
					Size scaledPageSize = this.GetScaledPageSize(i);
					rowInfo2.AddPage(scaledPageSize);
				}
				this.UpdateRow(num, rowInfo2);
				num2 = rowInfo2.FirstPage + rowInfo2.PageCount;
				num++;
			}
			return new RowCacheChange(rowIndexForPageNumber, num - rowIndexForPageNumber);
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00143AB8 File Offset: 0x00142AB8
		private void UpdateRow(int index, RowInfo newRow)
		{
			if (!this._isLayoutCompleted)
			{
				throw new InvalidOperationException(SR.Get("RowCacheCannotModifyNonExistentLayout"));
			}
			if (index > this._rowCache.Count)
			{
				return;
			}
			RowInfo rowInfo = this._rowCache[index];
			this._rowCache[index] = newRow;
			if (rowInfo.RowSize.Height != newRow.RowSize.Height)
			{
				double num = newRow.RowSize.Height - rowInfo.RowSize.Height;
				for (int i = index + 1; i < this._rowCache.Count; i++)
				{
					RowInfo rowInfo2 = this._rowCache[i];
					rowInfo2.VerticalOffset += num;
					this._rowCache[i] = rowInfo2;
				}
				this._extentHeight += num;
			}
			if (newRow.RowSize.Width > this._extentWidth)
			{
				this._extentWidth = newRow.RowSize.Width;
				return;
			}
			if (rowInfo.RowSize.Width != newRow.RowSize.Width)
			{
				this._extentWidth = 0.0;
				for (int j = 0; j < this._rowCache.Count; j++)
				{
					RowInfo rowInfo3 = this._rowCache[j];
					this._extentWidth = Math.Max(rowInfo3.RowSize.Width, this._extentWidth);
				}
			}
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00143C34 File Offset: 0x00142C34
		private RowCacheChange TrimPageRange(int startPage)
		{
			int num = this.GetRowIndexForPageNumber(startPage);
			RowInfo row = this.GetRow(num);
			if (row.FirstPage < startPage)
			{
				RowInfo rowInfo = new RowInfo();
				rowInfo.VerticalOffset = row.VerticalOffset;
				rowInfo.FirstPage = row.FirstPage;
				for (int i = row.FirstPage; i < startPage; i++)
				{
					Size scaledPageSize = this.GetScaledPageSize(i);
					rowInfo.AddPage(scaledPageSize);
				}
				this.UpdateRow(num, rowInfo);
				num++;
			}
			int count = this._rowCache.Count - num;
			if (num < this._rowCache.Count)
			{
				this._rowCache.RemoveRange(num, count);
			}
			this._extentHeight = row.VerticalOffset;
			return new RowCacheChange(num, count);
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00143CE8 File Offset: 0x00142CE8
		private Size GetScaledPageSize(int pageNumber)
		{
			Size pageSize = this.PageCache.GetPageSize(pageNumber);
			if (pageSize.IsEmpty)
			{
				pageSize = new Size(0.0, 0.0);
			}
			pageSize.Width *= this.Scale;
			pageSize.Height *= this.Scale;
			pageSize.Width += this.HorizontalPageSpacing;
			pageSize.Height += this.VerticalPageSpacing;
			return pageSize;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00143D78 File Offset: 0x00142D78
		private void OnPageCacheChanged(object sender, PageCacheChangedEventArgs args)
		{
			if (this._isLayoutCompleted)
			{
				List<RowCacheChange> list = new List<RowCacheChange>(args.Changes.Count);
				for (int i = 0; i < args.Changes.Count; i++)
				{
					PageCacheChange pageCacheChange = args.Changes[i];
					switch (pageCacheChange.Type)
					{
					case PageCacheChangeType.Add:
					case PageCacheChangeType.Update:
						if (pageCacheChange.Start > this.LastPageInCache)
						{
							RowCacheChange rowCacheChange = this.AddPageRange(pageCacheChange.Start, pageCacheChange.Count);
							if (rowCacheChange != null)
							{
								list.Add(rowCacheChange);
							}
						}
						else if (pageCacheChange.Start + pageCacheChange.Count - 1 <= this.LastPageInCache)
						{
							RowCacheChange rowCacheChange2 = this.UpdatePageRange(pageCacheChange.Start, pageCacheChange.Count);
							if (rowCacheChange2 != null)
							{
								list.Add(rowCacheChange2);
							}
						}
						else
						{
							RowCacheChange rowCacheChange3 = this.UpdatePageRange(pageCacheChange.Start, this.LastPageInCache - pageCacheChange.Start);
							if (rowCacheChange3 != null)
							{
								list.Add(rowCacheChange3);
							}
							rowCacheChange3 = this.AddPageRange(this.LastPageInCache + 1, pageCacheChange.Count - (this.LastPageInCache - pageCacheChange.Start));
							if (rowCacheChange3 != null)
							{
								list.Add(rowCacheChange3);
							}
						}
						break;
					case PageCacheChangeType.Remove:
						if (this.PageCache.PageCount - 1 < this.LastPageInCache)
						{
							RowCacheChange rowCacheChange4 = this.TrimPageRange(this.PageCache.PageCount);
							if (rowCacheChange4 != null)
							{
								list.Add(rowCacheChange4);
							}
						}
						if (this._rowCache.Count <= 1 && (this._rowCache.Count == 0 || this._rowCache[0].PageCount < this._layoutColumns))
						{
							this.RecalcRows(0, this._layoutColumns);
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("args");
					}
				}
				RowCacheChangedEventArgs e = new RowCacheChangedEventArgs(list);
				this.RowCacheChanged(this, e);
				return;
			}
			if (this._isLayoutRequested)
			{
				this.RecalcRows(this._layoutPivotPage, this._layoutColumns);
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00143F6F File Offset: 0x00142F6F
		private void OnPaginationCompleted(object sender, EventArgs args)
		{
			if (this._isLayoutRequested)
			{
				this.RecalcRows(this._layoutPivotPage, this._layoutColumns);
			}
		}

		// Token: 0x04000AFA RID: 2810
		private List<RowInfo> _rowCache;

		// Token: 0x04000AFB RID: 2811
		private int _layoutPivotPage;

		// Token: 0x04000AFC RID: 2812
		private int _layoutColumns;

		// Token: 0x04000AFD RID: 2813
		private int _pivotRowIndex;

		// Token: 0x04000AFE RID: 2814
		private PageCache _pageCache;

		// Token: 0x04000AFF RID: 2815
		private bool _isLayoutRequested;

		// Token: 0x04000B00 RID: 2816
		private bool _isLayoutCompleted;

		// Token: 0x04000B01 RID: 2817
		private double _verticalPageSpacing;

		// Token: 0x04000B02 RID: 2818
		private double _horizontalPageSpacing;

		// Token: 0x04000B03 RID: 2819
		private double _scale = 1.0;

		// Token: 0x04000B04 RID: 2820
		private double _extentHeight;

		// Token: 0x04000B05 RID: 2821
		private double _extentWidth;

		// Token: 0x04000B06 RID: 2822
		private bool _hasValidLayout;

		// Token: 0x04000B07 RID: 2823
		private readonly int _defaultRowCacheSize = 32;

		// Token: 0x04000B08 RID: 2824
		private readonly int _findOffsetPrecision = 2;

		// Token: 0x04000B09 RID: 2825
		private readonly double _visibleDelta = 0.5;
	}
}
