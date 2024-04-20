using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MS.Internal.Data;

namespace MS.Internal.Controls
{
	// Token: 0x0200025A RID: 602
	internal sealed class InnerItemCollectionView : CollectionView, IList, ICollection, IEnumerable
	{
		// Token: 0x0600173C RID: 5948 RVA: 0x0015DC20 File Offset: 0x0015CC20
		public InnerItemCollectionView(int capacity, ItemCollection itemCollection) : base(EmptyEnumerable.Instance, false)
		{
			this._rawList = (this._viewList = new ArrayList(capacity));
			this._itemCollection = itemCollection;
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x0015DC55 File Offset: 0x0015CC55
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				if (this._sort == null)
				{
					this.SetSortDescriptions(new SortDescriptionCollection());
				}
				return this._sort;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanSort
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x0015DC70 File Offset: 0x0015CC70
		public override bool Contains(object item)
		{
			return this._viewList.Contains(item);
		}

		// Token: 0x1700047B RID: 1147
		public object this[int index]
		{
			get
			{
				return this.GetItemAt(index);
			}
			set
			{
				bool flag = this.AssertPristineModelChild(value) != null;
				int currentPosition = this.CurrentPosition;
				object obj = this._viewList[index];
				this._viewList[index] = value;
				int num = -1;
				if (this.IsCachedMode)
				{
					num = this._rawList.IndexOf(obj);
					this._rawList[num] = value;
				}
				bool flag2 = true;
				if (flag)
				{
					flag2 = false;
					try
					{
						this.SetModelParent(value);
						flag2 = true;
					}
					finally
					{
						if (!flag2)
						{
							this._viewList[index] = obj;
							if (num > 0)
							{
								this._rawList[num] = obj;
							}
						}
						else
						{
							this.ClearModelParent(obj);
						}
					}
				}
				if (!flag2)
				{
					return;
				}
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, obj, index));
				this.SetIsModified();
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x0015DD48 File Offset: 0x0015CD48
		public int Add(object item)
		{
			bool flag = this.AssertPristineModelChild(item) != null;
			int num = this._viewList.Add(item);
			int num2 = -1;
			if (this.IsCachedMode)
			{
				num2 = this._rawList.Add(item);
			}
			bool flag2 = true;
			if (flag)
			{
				flag2 = false;
				try
				{
					this.SetModelParent(item);
					flag2 = true;
				}
				finally
				{
					if (!flag2)
					{
						this._viewList.RemoveAt(num);
						if (num2 >= 0)
						{
							this._rawList.RemoveAt(num2);
						}
						this.ClearModelParent(item);
						num = -1;
					}
				}
			}
			if (!flag2)
			{
				return -1;
			}
			this.AdjustCurrencyForAdd(num);
			this.SetIsModified();
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, num));
			return num;
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x0015DDEC File Offset: 0x0015CDEC
		public void Clear()
		{
			try
			{
				for (int i = this._rawList.Count - 1; i >= 0; i--)
				{
					this.ClearModelParent(this._rawList[i]);
				}
			}
			finally
			{
				this._rawList.Clear();
				base.RefreshOrDefer();
			}
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x0015DE48 File Offset: 0x0015CE48
		public void Insert(int index, object item)
		{
			bool flag = this.AssertPristineModelChild(item) != null;
			this._viewList.Insert(index, item);
			int num = -1;
			if (this.IsCachedMode)
			{
				num = this._rawList.Add(item);
			}
			bool flag2 = true;
			if (flag)
			{
				flag2 = false;
				try
				{
					this.SetModelParent(item);
					flag2 = true;
				}
				finally
				{
					if (!flag2)
					{
						this._viewList.RemoveAt(index);
						if (num >= 0)
						{
							this._rawList.RemoveAt(num);
						}
						this.ClearModelParent(item);
					}
				}
			}
			if (!flag2)
			{
				return;
			}
			this.AdjustCurrencyForAdd(index);
			this.SetIsModified();
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x0015DEE8 File Offset: 0x0015CEE8
		public void Remove(object item)
		{
			int index = this._viewList.IndexOf(item);
			int indexR = -1;
			if (this.IsCachedMode)
			{
				indexR = this._rawList.IndexOf(item);
			}
			this._RemoveAt(index, indexR, item);
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x0015DF24 File Offset: 0x0015CF24
		public void RemoveAt(int index)
		{
			if (0 <= index && index < this.ViewCount)
			{
				object obj = this[index];
				int indexR = -1;
				if (this.IsCachedMode)
				{
					indexR = this._rawList.IndexOf(obj);
				}
				this._RemoveAt(index, indexR, obj);
				return;
			}
			throw new ArgumentOutOfRangeException("index", SR.Get("ItemCollectionRemoveArgumentOutOfRange"));
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x0015DF7B File Offset: 0x0015CF7B
		object ICollection.SyncRoot
		{
			get
			{
				return this._rawList.SyncRoot;
			}
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x0015DF88 File Offset: 0x0015CF88
		void ICollection.CopyTo(Array array, int index)
		{
			this._viewList.CopyTo(array, index);
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x000F93D3 File Offset: 0x000F83D3
		public override IEnumerable SourceCollection
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x0015DF97 File Offset: 0x0015CF97
		public override int Count
		{
			get
			{
				return this.ViewCount;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x0015DF9F File Offset: 0x0015CF9F
		public override bool IsEmpty
		{
			get
			{
				return this.ViewCount == 0;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x0015DFAA File Offset: 0x0015CFAA
		public override bool NeedsRefresh
		{
			get
			{
				return base.NeedsRefresh || this._isModified;
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x0015DFBC File Offset: 0x0015CFBC
		public override int IndexOf(object item)
		{
			return this._viewList.IndexOf(item);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0015DFCA File Offset: 0x0015CFCA
		public override object GetItemAt(int index)
		{
			return this._viewList[index];
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0015DFD8 File Offset: 0x0015CFD8
		public override bool MoveCurrentTo(object item)
		{
			if (ItemsControl.EqualsEx(this.CurrentItem, item) && (item != null || this.IsCurrentInView))
			{
				return this.IsCurrentInView;
			}
			return this.MoveCurrentToPosition(this.IndexOf(item));
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x0015E008 File Offset: 0x0015D008
		public override bool MoveCurrentToPosition(int position)
		{
			if (position < -1 || position > this.ViewCount)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			if (position != this.CurrentPosition && base.OKToChangeCurrent())
			{
				bool isCurrentAfterLast = this.IsCurrentAfterLast;
				bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
				this._MoveCurrentToPosition(position);
				this.OnCurrentChanged();
				if (this.IsCurrentAfterLast != isCurrentAfterLast)
				{
					this.OnPropertyChanged("IsCurrentAfterLast");
				}
				if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
				{
					this.OnPropertyChanged("IsCurrentBeforeFirst");
				}
				this.OnPropertyChanged("CurrentPosition");
				this.OnPropertyChanged("CurrentItem");
			}
			return this.IsCurrentInView;
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0015E0A0 File Offset: 0x0015D0A0
		protected override void RefreshOverride()
		{
			bool isEmpty = this.IsEmpty;
			object currentItem = this.CurrentItem;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			int currentPosition = this.CurrentPosition;
			base.OnCurrentChanging();
			if (this.SortDescriptions.Count > 0 || this.Filter != null)
			{
				if (this.Filter == null)
				{
					this._viewList = new ArrayList(this._rawList);
				}
				else
				{
					this._viewList = new ArrayList();
					for (int i = 0; i < this._rawList.Count; i++)
					{
						if (this.Filter(this._rawList[i]))
						{
							this._viewList.Add(this._rawList[i]);
						}
					}
				}
				if (this._sort != null && this._sort.Count > 0 && this.ViewCount > 0)
				{
					SortFieldComparer.SortHelper(this._viewList, new SortFieldComparer(this._sort, this.Culture));
				}
			}
			else
			{
				this._viewList = this._rawList;
			}
			if (this.IsEmpty || isCurrentBeforeFirst)
			{
				this._MoveCurrentToPosition(-1);
			}
			else if (isCurrentAfterLast)
			{
				this._MoveCurrentToPosition(this.ViewCount);
			}
			else if (currentItem != null)
			{
				int num = this._viewList.IndexOf(currentItem);
				if (num < 0)
				{
					num = 0;
				}
				this._MoveCurrentToPosition(num);
			}
			this.ClearIsModified();
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			this.OnCurrentChanged();
			if (this.IsCurrentAfterLast != isCurrentAfterLast)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (currentPosition != this.CurrentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (currentItem != this.CurrentItem)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0015E258 File Offset: 0x0015D258
		protected override IEnumerator GetEnumerator()
		{
			return this._viewList.GetEnumerator();
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x0015E265 File Offset: 0x0015D265
		internal ItemCollection ItemCollection
		{
			get
			{
				return this._itemCollection;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001757 RID: 5975 RVA: 0x0015E26D File Offset: 0x0015D26D
		internal IEnumerator LogicalChildren
		{
			get
			{
				return this._rawList.GetEnumerator();
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x0015E27A File Offset: 0x0015D27A
		internal int RawCount
		{
			get
			{
				return this._rawList.Count;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001759 RID: 5977 RVA: 0x0015E287 File Offset: 0x0015D287
		private int ViewCount
		{
			get
			{
				return this._viewList.Count;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x0015E294 File Offset: 0x0015D294
		private bool IsCachedMode
		{
			get
			{
				return this._viewList != this._rawList;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600175B RID: 5979 RVA: 0x0015E2A7 File Offset: 0x0015D2A7
		private FrameworkElement ModelParentFE
		{
			get
			{
				return this.ItemCollection.ModelParentFE;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600175C RID: 5980 RVA: 0x0015E2B4 File Offset: 0x0015D2B4
		private bool IsCurrentInView
		{
			get
			{
				return 0 <= this.CurrentPosition && this.CurrentPosition < this.ViewCount;
			}
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x0015E2CF File Offset: 0x0015D2CF
		private void SetIsModified()
		{
			if (this.IsCachedMode)
			{
				this._isModified = true;
			}
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0015E2E0 File Offset: 0x0015D2E0
		private void ClearIsModified()
		{
			this._isModified = false;
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0015E2EC File Offset: 0x0015D2EC
		private void _RemoveAt(int index, int indexR, object item)
		{
			if (index >= 0)
			{
				this._viewList.RemoveAt(index);
			}
			if (indexR >= 0)
			{
				this._rawList.RemoveAt(indexR);
			}
			try
			{
				this.ClearModelParent(item);
			}
			finally
			{
				if (index >= 0)
				{
					this.AdjustCurrencyForRemove(index);
					this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
					if (this._currentElementWasRemoved)
					{
						this.MoveCurrencyOffDeletedElement();
					}
				}
			}
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0015E35C File Offset: 0x0015D35C
		private DependencyObject AssertPristineModelChild(object item)
		{
			DependencyObject dependencyObject = item as DependencyObject;
			if (dependencyObject == null)
			{
				return null;
			}
			if (LogicalTreeHelper.GetParent(dependencyObject) != null)
			{
				throw new InvalidOperationException(SR.Get("ReparentModelChildIllegal"));
			}
			return dependencyObject;
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x0015E38E File Offset: 0x0015D38E
		private void SetModelParent(object item)
		{
			if (this.ModelParentFE != null && item is DependencyObject)
			{
				LogicalTreeHelper.AddLogicalChild(this.ModelParentFE, null, item);
			}
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x0015E3AD File Offset: 0x0015D3AD
		private void ClearModelParent(object item)
		{
			if (this.ModelParentFE != null && item is DependencyObject)
			{
				LogicalTreeHelper.RemoveLogicalChild(this.ModelParentFE, null, item);
			}
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x0015E3CC File Offset: 0x0015D3CC
		private void SetSortDescriptions(SortDescriptionCollection descriptions)
		{
			if (this._sort != null)
			{
				((INotifyCollectionChanged)this._sort).CollectionChanged -= this.SortDescriptionsChanged;
			}
			this._sort = descriptions;
			if (this._sort != null)
			{
				Invariant.Assert(this._sort.Count == 0, "must be empty SortDescription collection");
				((INotifyCollectionChanged)this._sort).CollectionChanged += this.SortDescriptionsChanged;
			}
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0015E436 File Offset: 0x0015D436
		private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.RefreshOrDefer();
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x0015E43E File Offset: 0x0015D43E
		private void _MoveCurrentToPosition(int position)
		{
			if (position < 0)
			{
				base.SetCurrent(null, -1);
				return;
			}
			if (position >= this.ViewCount)
			{
				base.SetCurrent(null, this.ViewCount);
				return;
			}
			base.SetCurrent(this._viewList[position], position);
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x0015E478 File Offset: 0x0015D478
		private void AdjustCurrencyForAdd(int index)
		{
			if (index < 0)
			{
				return;
			}
			if (this.ViewCount == 1)
			{
				base.SetCurrent(null, -1);
				return;
			}
			if (index <= this.CurrentPosition)
			{
				int num = this.CurrentPosition + 1;
				if (num < this.ViewCount)
				{
					base.SetCurrent(this._viewList[num], num);
					return;
				}
				base.SetCurrent(null, this.ViewCount);
			}
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x0015E4D8 File Offset: 0x0015D4D8
		private void AdjustCurrencyForRemove(int index)
		{
			if (index < 0)
			{
				return;
			}
			if (index < this.CurrentPosition)
			{
				int num = this.CurrentPosition - 1;
				base.SetCurrent(this._viewList[num], num);
				return;
			}
			if (index == this.CurrentPosition)
			{
				this._currentElementWasRemoved = true;
			}
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0015E520 File Offset: 0x0015D520
		private void MoveCurrencyOffDeletedElement()
		{
			int num = this.ViewCount - 1;
			int position = (this.CurrentPosition < num) ? this.CurrentPosition : num;
			this._currentElementWasRemoved = false;
			base.OnCurrentChanging();
			this._MoveCurrentToPosition(position);
			this.OnCurrentChanged();
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00150A1C File Offset: 0x0014FA1C
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x04000CA1 RID: 3233
		private SortDescriptionCollection _sort;

		// Token: 0x04000CA2 RID: 3234
		private ArrayList _viewList;

		// Token: 0x04000CA3 RID: 3235
		private ArrayList _rawList;

		// Token: 0x04000CA4 RID: 3236
		private ItemCollection _itemCollection;

		// Token: 0x04000CA5 RID: 3237
		private bool _isModified;

		// Token: 0x04000CA6 RID: 3238
		private bool _currentElementWasRemoved;
	}
}
