using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020001D5 RID: 469
	internal class PageCache
	{
		// Token: 0x060010AA RID: 4266 RVA: 0x00141198 File Offset: 0x00140198
		public PageCache()
		{
			this._cache = new List<PageCacheEntry>(this._defaultCacheSize);
			this._pageDestroyedWatcher = new PageDestroyedWatcher();
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x001413D1 File Offset: 0x001403D1
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x001411EC File Offset: 0x001401EC
		public DynamicDocumentPaginator Content
		{
			get
			{
				return this._documentPaginator;
			}
			set
			{
				if (this._documentPaginator != value)
				{
					this._dynamicPageSizes = false;
					this._defaultPageSize = this._initialDefaultPageSize;
					this._isDefaultSizeKnown = false;
					this._isPaginationCompleted = false;
					if (this._documentPaginator != null)
					{
						this._documentPaginator.PagesChanged -= this.OnPagesChanged;
						this._documentPaginator.GetPageCompleted -= this.OnGetPageCompleted;
						this._documentPaginator.PaginationProgress -= this.OnPaginationProgress;
						this._documentPaginator.PaginationCompleted -= this.OnPaginationCompleted;
						this._documentPaginator.IsBackgroundPaginationEnabled = this._originalIsBackgroundPaginationEnabled;
					}
					this._documentPaginator = value;
					this.ClearCache();
					if (this._documentPaginator != null)
					{
						this._documentPaginator.PagesChanged += this.OnPagesChanged;
						this._documentPaginator.GetPageCompleted += this.OnGetPageCompleted;
						this._documentPaginator.PaginationProgress += this.OnPaginationProgress;
						this._documentPaginator.PaginationCompleted += this.OnPaginationCompleted;
						this._documentPaginator.PageSize = this._defaultPageSize;
						this._originalIsBackgroundPaginationEnabled = this._documentPaginator.IsBackgroundPaginationEnabled;
						this._documentPaginator.IsBackgroundPaginationEnabled = true;
						if (this._documentPaginator.Source is DependencyObject)
						{
							if ((FlowDirection)((DependencyObject)this._documentPaginator.Source).GetValue(FrameworkElement.FlowDirectionProperty) == FlowDirection.LeftToRight)
							{
								this._isContentRightToLeft = false;
							}
							else
							{
								this._isContentRightToLeft = true;
							}
						}
					}
					if (this._documentPaginator != null)
					{
						if (this._documentPaginator.PageCount > 0)
						{
							this.OnPaginationProgress(this._documentPaginator, new PaginationProgressEventArgs(0, this._documentPaginator.PageCount));
						}
						if (this._documentPaginator.IsPageCountValid)
						{
							this.OnPaginationCompleted(this._documentPaginator, EventArgs.Empty);
						}
					}
				}
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x001413D9 File Offset: 0x001403D9
		public int PageCount
		{
			get
			{
				return this._cache.Count;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x001413E6 File Offset: 0x001403E6
		public bool DynamicPageSizes
		{
			get
			{
				return this._dynamicPageSizes;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x001413EE File Offset: 0x001403EE
		public bool IsContentRightToLeft
		{
			get
			{
				return this._isContentRightToLeft;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x001413F6 File Offset: 0x001403F6
		public bool IsPaginationCompleted
		{
			get
			{
				return this._isPaginationCompleted;
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060010B1 RID: 4273 RVA: 0x00141400 File Offset: 0x00140400
		// (remove) Token: 0x060010B2 RID: 4274 RVA: 0x00141438 File Offset: 0x00140438
		public event PaginationProgressEventHandler PaginationProgress;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060010B3 RID: 4275 RVA: 0x00141470 File Offset: 0x00140470
		// (remove) Token: 0x060010B4 RID: 4276 RVA: 0x001414A8 File Offset: 0x001404A8
		public event EventHandler PaginationCompleted;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060010B5 RID: 4277 RVA: 0x001414E0 File Offset: 0x001404E0
		// (remove) Token: 0x060010B6 RID: 4278 RVA: 0x00141518 File Offset: 0x00140518
		public event PagesChangedEventHandler PagesChanged;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060010B7 RID: 4279 RVA: 0x00141550 File Offset: 0x00140550
		// (remove) Token: 0x060010B8 RID: 4280 RVA: 0x00141588 File Offset: 0x00140588
		public event GetPageCompletedEventHandler GetPageCompleted;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060010B9 RID: 4281 RVA: 0x001415C0 File Offset: 0x001405C0
		// (remove) Token: 0x060010BA RID: 4282 RVA: 0x001415F8 File Offset: 0x001405F8
		public event PageCacheChangedEventHandler PageCacheChanged;

		// Token: 0x060010BB RID: 4283 RVA: 0x00141630 File Offset: 0x00140630
		public Size GetPageSize(int pageNumber)
		{
			if (pageNumber >= 0 && pageNumber < this._cache.Count)
			{
				Size pageSize = this._cache[pageNumber].PageSize;
				Invariant.Assert(pageSize != Size.Empty, "PageCache entry's PageSize is Empty.");
				return pageSize;
			}
			return new Size(0.0, 0.0);
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0014168D File Offset: 0x0014068D
		public bool IsPageDirty(int pageNumber)
		{
			return pageNumber < 0 || pageNumber >= this._cache.Count || this._cache[pageNumber].Dirty;
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x001416B4 File Offset: 0x001406B4
		private void OnPaginationProgress(object sender, PaginationProgressEventArgs args)
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.PaginationProgressDelegate), args);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x001416D0 File Offset: 0x001406D0
		private object PaginationProgressDelegate(object parameter)
		{
			PaginationProgressEventArgs paginationProgressEventArgs = parameter as PaginationProgressEventArgs;
			if (paginationProgressEventArgs == null)
			{
				throw new InvalidOperationException("parameter");
			}
			this.ValidatePaginationArgs(paginationProgressEventArgs.Start, paginationProgressEventArgs.Count);
			if (this._isPaginationCompleted)
			{
				if (paginationProgressEventArgs.Start == 0)
				{
					this._isDefaultSizeKnown = false;
					this._dynamicPageSizes = false;
				}
				this._isPaginationCompleted = false;
			}
			if (paginationProgressEventArgs.Start + paginationProgressEventArgs.Count < 0)
			{
				throw new ArgumentOutOfRangeException("args");
			}
			List<PageCacheChange> list = new List<PageCacheChange>(2);
			if (paginationProgressEventArgs.Count > 0)
			{
				if (paginationProgressEventArgs.Start >= this._cache.Count)
				{
					PageCacheChange pageCacheChange = this.AddRange(paginationProgressEventArgs.Start, paginationProgressEventArgs.Count);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
				else if (paginationProgressEventArgs.Start + paginationProgressEventArgs.Count < this._cache.Count)
				{
					PageCacheChange pageCacheChange = this.DirtyRange(paginationProgressEventArgs.Start, paginationProgressEventArgs.Count);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
				else
				{
					PageCacheChange pageCacheChange = this.DirtyRange(paginationProgressEventArgs.Start, this._cache.Count - paginationProgressEventArgs.Start);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
					pageCacheChange = this.AddRange(this._cache.Count, paginationProgressEventArgs.Count - (this._cache.Count - paginationProgressEventArgs.Start) + 1);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
			}
			int num = (this._documentPaginator != null) ? this._documentPaginator.PageCount : 0;
			if (num < this._cache.Count)
			{
				PageCacheChange pageCacheChange = new PageCacheChange(num, this._cache.Count - num, PageCacheChangeType.Remove);
				list.Add(pageCacheChange);
				this._cache.RemoveRange(num, this._cache.Count - num);
			}
			this.FirePageCacheChangedEvent(list);
			if (this.PaginationProgress != null)
			{
				this.PaginationProgress(this, paginationProgressEventArgs);
			}
			return null;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0014189F File Offset: 0x0014089F
		private void OnPaginationCompleted(object sender, EventArgs args)
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.PaginationCompletedDelegate), args);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x001418BC File Offset: 0x001408BC
		private object PaginationCompletedDelegate(object parameter)
		{
			EventArgs eventArgs = parameter as EventArgs;
			if (eventArgs == null)
			{
				throw new ArgumentOutOfRangeException("parameter");
			}
			this._isPaginationCompleted = true;
			if (this.PaginationCompleted != null)
			{
				this.PaginationCompleted(this, eventArgs);
			}
			return null;
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x001418FB File Offset: 0x001408FB
		private void OnPagesChanged(object sender, PagesChangedEventArgs args)
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.PagesChangedDelegate), args);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00141918 File Offset: 0x00140918
		private object PagesChangedDelegate(object parameter)
		{
			PagesChangedEventArgs pagesChangedEventArgs = parameter as PagesChangedEventArgs;
			if (pagesChangedEventArgs == null)
			{
				throw new ArgumentOutOfRangeException("parameter");
			}
			this.ValidatePaginationArgs(pagesChangedEventArgs.Start, pagesChangedEventArgs.Count);
			int num = pagesChangedEventArgs.Count;
			if (pagesChangedEventArgs.Start + pagesChangedEventArgs.Count >= this._cache.Count || pagesChangedEventArgs.Start + pagesChangedEventArgs.Count < 0)
			{
				num = this._cache.Count - pagesChangedEventArgs.Start;
			}
			List<PageCacheChange> list = new List<PageCacheChange>(1);
			if (num > 0)
			{
				PageCacheChange pageCacheChange = this.DirtyRange(pagesChangedEventArgs.Start, num);
				if (pageCacheChange != null)
				{
					list.Add(pageCacheChange);
				}
				this.FirePageCacheChangedEvent(list);
			}
			if (this.PagesChanged != null)
			{
				this.PagesChanged(this, pagesChangedEventArgs);
			}
			return null;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x001419D0 File Offset: 0x001409D0
		private void OnGetPageCompleted(object sender, GetPageCompletedEventArgs args)
		{
			if (!args.Cancelled && args.Error == null && args.DocumentPage != DocumentPage.Missing)
			{
				this._pageDestroyedWatcher.AddPage(args.DocumentPage);
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.GetPageCompletedDelegate), args);
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00141A28 File Offset: 0x00140A28
		private object GetPageCompletedDelegate(object parameter)
		{
			GetPageCompletedEventArgs getPageCompletedEventArgs = parameter as GetPageCompletedEventArgs;
			if (getPageCompletedEventArgs == null)
			{
				throw new ArgumentOutOfRangeException("parameter");
			}
			bool flag = this._pageDestroyedWatcher.IsDestroyed(getPageCompletedEventArgs.DocumentPage);
			this._pageDestroyedWatcher.RemovePage(getPageCompletedEventArgs.DocumentPage);
			if (flag)
			{
				return null;
			}
			if (!getPageCompletedEventArgs.Cancelled && getPageCompletedEventArgs.Error == null && getPageCompletedEventArgs.DocumentPage != DocumentPage.Missing)
			{
				if (getPageCompletedEventArgs.DocumentPage.Size == Size.Empty)
				{
					throw new ArgumentOutOfRangeException("args");
				}
				PageCacheEntry pageCacheEntry;
				pageCacheEntry.PageSize = getPageCompletedEventArgs.DocumentPage.Size;
				pageCacheEntry.Dirty = false;
				List<PageCacheChange> list = new List<PageCacheChange>(2);
				if (getPageCompletedEventArgs.PageNumber > this._cache.Count - 1)
				{
					PageCacheChange pageCacheChange = this.AddRange(getPageCompletedEventArgs.PageNumber, 1);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
					pageCacheChange = this.UpdateEntry(getPageCompletedEventArgs.PageNumber, pageCacheEntry);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
				else
				{
					PageCacheChange pageCacheChange = this.UpdateEntry(getPageCompletedEventArgs.PageNumber, pageCacheEntry);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
				if (this._isDefaultSizeKnown && pageCacheEntry.PageSize != this._lastPageSize)
				{
					this._dynamicPageSizes = true;
				}
				this._lastPageSize = pageCacheEntry.PageSize;
				if (!this._isDefaultSizeKnown)
				{
					this._defaultPageSize = pageCacheEntry.PageSize;
					this._isDefaultSizeKnown = true;
					this.SetDefaultPageSize(true);
				}
				this.FirePageCacheChangedEvent(list);
			}
			if (this.GetPageCompleted != null)
			{
				this.GetPageCompleted(this, getPageCompletedEventArgs);
			}
			return null;
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00141BA5 File Offset: 0x00140BA5
		private void ValidatePaginationArgs(int start, int count)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00141BC8 File Offset: 0x00140BC8
		private void SetDefaultPageSize(bool dirtyOnly)
		{
			List<PageCacheChange> list = new List<PageCacheChange>(this.PageCount);
			Invariant.Assert(this._defaultPageSize != Size.Empty, "Default Page Size is Empty.");
			for (int i = 0; i < this._cache.Count; i++)
			{
				if (this._cache[i].Dirty || !dirtyOnly)
				{
					PageCacheEntry newEntry;
					newEntry.PageSize = this._defaultPageSize;
					newEntry.Dirty = true;
					PageCacheChange pageCacheChange = this.UpdateEntry(i, newEntry);
					if (pageCacheChange != null)
					{
						list.Add(pageCacheChange);
					}
				}
			}
			this.FirePageCacheChangedEvent(list);
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00141C58 File Offset: 0x00140C58
		private void FirePageCacheChangedEvent(List<PageCacheChange> changes)
		{
			if (this.PageCacheChanged != null && changes != null && changes.Count > 0)
			{
				PageCacheChangedEventArgs e = new PageCacheChangedEventArgs(changes);
				this.PageCacheChanged(this, e);
			}
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00141C90 File Offset: 0x00140C90
		private PageCacheChange AddRange(int start, int count)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (count < 1)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			Invariant.Assert(this._defaultPageSize != Size.Empty, "Default Page Size is Empty.");
			if (start >= this._cache.Count)
			{
				count += start - this._cache.Count;
				start = this._cache.Count;
			}
			for (int i = start; i < start + count; i++)
			{
				PageCacheEntry item;
				item.PageSize = this._defaultPageSize;
				item.Dirty = true;
				this._cache.Add(item);
			}
			return new PageCacheChange(start, count, PageCacheChangeType.Add);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00141D38 File Offset: 0x00140D38
		private PageCacheChange UpdateEntry(int index, PageCacheEntry newEntry)
		{
			if (index >= this._cache.Count || index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			Invariant.Assert(newEntry.PageSize != Size.Empty, "Updated entry newEntry has Empty PageSize.");
			if (newEntry.PageSize != this._cache[index].PageSize || newEntry.Dirty != this._cache[index].Dirty)
			{
				this._cache[index] = newEntry;
				return new PageCacheChange(index, 1, PageCacheChangeType.Update);
			}
			return null;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00141DCC File Offset: 0x00140DCC
		private PageCacheChange DirtyRange(int start, int count)
		{
			if (start >= this._cache.Count)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (start + count > this._cache.Count || count < 1)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			Invariant.Assert(this._defaultPageSize != Size.Empty, "Default Page Size is Empty.");
			for (int i = start; i < start + count; i++)
			{
				PageCacheEntry value;
				value.Dirty = true;
				value.PageSize = this._defaultPageSize;
				this._cache[i] = value;
			}
			return new PageCacheChange(start, count, PageCacheChangeType.Update);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00141E64 File Offset: 0x00140E64
		private void ClearCache()
		{
			if (this._cache.Count > 0)
			{
				List<PageCacheChange> list = new List<PageCacheChange>(1);
				PageCacheChange item = new PageCacheChange(0, this._cache.Count, PageCacheChangeType.Remove);
				list.Add(item);
				this._cache.Clear();
				this.FirePageCacheChangedEvent(list);
			}
		}

		// Token: 0x04000AC9 RID: 2761
		private List<PageCacheEntry> _cache;

		// Token: 0x04000ACA RID: 2762
		private PageDestroyedWatcher _pageDestroyedWatcher;

		// Token: 0x04000ACB RID: 2763
		private DynamicDocumentPaginator _documentPaginator;

		// Token: 0x04000ACC RID: 2764
		private bool _originalIsBackgroundPaginationEnabled;

		// Token: 0x04000ACD RID: 2765
		private bool _dynamicPageSizes;

		// Token: 0x04000ACE RID: 2766
		private bool _isContentRightToLeft;

		// Token: 0x04000ACF RID: 2767
		private bool _isPaginationCompleted;

		// Token: 0x04000AD0 RID: 2768
		private bool _isDefaultSizeKnown;

		// Token: 0x04000AD1 RID: 2769
		private Size _defaultPageSize;

		// Token: 0x04000AD2 RID: 2770
		private Size _lastPageSize;

		// Token: 0x04000AD3 RID: 2771
		private readonly Size _initialDefaultPageSize = new Size(816.0, 1056.0);

		// Token: 0x04000AD4 RID: 2772
		private readonly int _defaultCacheSize = 64;
	}
}
