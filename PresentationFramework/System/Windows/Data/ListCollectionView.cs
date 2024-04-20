using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000460 RID: 1120
	public class ListCollectionView : CollectionView, IComparer, IEditableCollectionViewAddNewItem, IEditableCollectionView, ICollectionViewLiveShaping, IItemProperties
	{
		// Token: 0x060038E1 RID: 14561 RVA: 0x001EA064 File Offset: 0x001E9064
		public ListCollectionView(IList list) : base(list)
		{
			if (base.AllowsCrossThreadChanges)
			{
				BindingOperations.AccessCollection(list, delegate
				{
					base.ClearPendingChanges();
					this.ShadowCollection = new ArrayList((ICollection)this.SourceCollection);
					this._internalList = this.ShadowCollection;
				}, false);
			}
			else
			{
				this._internalList = list;
			}
			if (this.InternalList.Count == 0)
			{
				base.SetCurrent(null, -1, 0);
			}
			else
			{
				base.SetCurrent(this.InternalList[0], 0, 1);
			}
			this._group = new CollectionViewGroupRoot(this);
			this._group.GroupDescriptionChanged += this.OnGroupDescriptionChanged;
			((INotifyCollectionChanged)this._group).CollectionChanged += this.OnGroupChanged;
			((INotifyCollectionChanged)this._group.GroupDescriptions).CollectionChanged += this.OnGroupByChanged;
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x001EA150 File Offset: 0x001E9150
		protected override void RefreshOverride()
		{
			if (base.AllowsCrossThreadChanges)
			{
				BindingOperations.AccessCollection(this.SourceCollection, delegate
				{
					object syncRoot = base.SyncRoot;
					lock (syncRoot)
					{
						base.ClearPendingChanges();
						this.ShadowCollection = new ArrayList((ICollection)this.SourceCollection);
					}
				}, false);
			}
			object currentItem = this.CurrentItem;
			int num = this.IsEmpty ? -1 : this.CurrentPosition;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			base.OnCurrentChanging();
			this.PrepareLocalArray();
			if (isCurrentBeforeFirst || this.IsEmpty)
			{
				base.SetCurrent(null, -1);
			}
			else if (isCurrentAfterLast)
			{
				base.SetCurrent(null, this.InternalCount);
			}
			else
			{
				int num2 = this.InternalIndexOf(currentItem);
				if (num2 < 0)
				{
					num2 = ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0);
					object newItem;
					if (num2 < this.InternalCount && (newItem = this.InternalItemAt(num2)) != CollectionView.NewItemPlaceholder)
					{
						base.SetCurrent(newItem, num2);
					}
					else
					{
						base.SetCurrent(null, -1);
					}
				}
				else
				{
					base.SetCurrent(currentItem, num2);
				}
			}
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
			if (num != this.CurrentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (currentItem != this.CurrentItem)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x001EA288 File Offset: 0x001E9288
		public override bool Contains(object item)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalContains(item);
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x001EA298 File Offset: 0x001E9298
		public override bool MoveCurrentToPosition(int position)
		{
			base.VerifyRefreshNotDeferred();
			if (position < -1 || position > this.InternalCount)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			if (position != this.CurrentPosition || !base.IsCurrentInSync)
			{
				object obj = (0 <= position && position < this.InternalCount) ? this.InternalItemAt(position) : null;
				if (obj != CollectionView.NewItemPlaceholder && base.OKToChangeCurrent())
				{
					bool isCurrentAfterLast = this.IsCurrentAfterLast;
					bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
					base.SetCurrent(obj, position);
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
			}
			return this.IsCurrentInView;
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x060038E5 RID: 14565 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanGroup
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x060038E6 RID: 14566 RVA: 0x001EA35F File Offset: 0x001E935F
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._group.GroupDescriptions;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x060038E7 RID: 14567 RVA: 0x001EA36C File Offset: 0x001E936C
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				if (!this.IsGrouping)
				{
					return null;
				}
				return this._group.Items;
			}
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x001EA383 File Offset: 0x001E9383
		public override bool PassesFilter(object item)
		{
			return this.ActiveFilter == null || this.ActiveFilter(item);
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x001EA39B File Offset: 0x001E939B
		public override int IndexOf(object item)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalIndexOf(item);
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x001EA3AA File Offset: 0x001E93AA
		public override object GetItemAt(int index)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalItemAt(index);
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x001EA3B9 File Offset: 0x001E93B9
		int IComparer.Compare(object o1, object o2)
		{
			return this.Compare(o1, o2);
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x001EA3C4 File Offset: 0x001E93C4
		protected virtual int Compare(object o1, object o2)
		{
			if (this.IsGrouping)
			{
				int num = this.InternalIndexOf(o1);
				int num2 = this.InternalIndexOf(o2);
				return num - num2;
			}
			if (this.ActiveComparer != null)
			{
				return this.ActiveComparer.Compare(o1, o2);
			}
			int num3 = this.InternalList.IndexOf(o1);
			int num4 = this.InternalList.IndexOf(o2);
			return num3 - num4;
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x001EA41C File Offset: 0x001E941C
		protected override IEnumerator GetEnumerator()
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalGetEnumerator();
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x060038EE RID: 14574 RVA: 0x001EA42A File Offset: 0x001E942A
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

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x060038EF RID: 14575 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanSort
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x060038F0 RID: 14576 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanFilter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x060038F1 RID: 14577 RVA: 0x001EA445 File Offset: 0x001E9445
		// (set) Token: 0x060038F2 RID: 14578 RVA: 0x001EA450 File Offset: 0x001E9450
		public override Predicate<object> Filter
		{
			get
			{
				return base.Filter;
			}
			set
			{
				if (base.AllowsCrossThreadChanges)
				{
					base.VerifyAccess();
				}
				if (this.IsAddingNew || this.IsEditingItem)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
					{
						"Filter"
					}));
				}
				base.Filter = value;
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x060038F3 RID: 14579 RVA: 0x001EA4A0 File Offset: 0x001E94A0
		// (set) Token: 0x060038F4 RID: 14580 RVA: 0x001EA4A8 File Offset: 0x001E94A8
		public IComparer CustomSort
		{
			get
			{
				return this._customSort;
			}
			set
			{
				if (base.AllowsCrossThreadChanges)
				{
					base.VerifyAccess();
				}
				if (this.IsAddingNew || this.IsEditingItem)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
					{
						"CustomSort"
					}));
				}
				this._customSort = value;
				this.SetSortDescriptions(null);
				base.RefreshOrDefer();
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x060038F5 RID: 14581 RVA: 0x001EA505 File Offset: 0x001E9505
		// (set) Token: 0x060038F6 RID: 14582 RVA: 0x001EA514 File Offset: 0x001E9514
		[DefaultValue(null)]
		public virtual GroupDescriptionSelectorCallback GroupBySelector
		{
			get
			{
				return this._group.GroupBySelector;
			}
			set
			{
				if (!this.CanGroup)
				{
					throw new NotSupportedException();
				}
				if (this.IsAddingNew || this.IsEditingItem)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
					{
						"Grouping"
					}));
				}
				this._group.GroupBySelector = value;
				base.RefreshOrDefer();
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x060038F7 RID: 14583 RVA: 0x001EA56F File Offset: 0x001E956F
		public override int Count
		{
			get
			{
				base.VerifyRefreshNotDeferred();
				return this.InternalCount;
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x060038F8 RID: 14584 RVA: 0x001EA57D File Offset: 0x001E957D
		public override bool IsEmpty
		{
			get
			{
				return this.InternalCount == 0;
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x001EA588 File Offset: 0x001E9588
		// (set) Token: 0x060038FA RID: 14586 RVA: 0x001EA595 File Offset: 0x001E9595
		public bool IsDataInGroupOrder
		{
			get
			{
				return this._group.IsDataInGroupOrder;
			}
			set
			{
				this._group.IsDataInGroupOrder = value;
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x060038FB RID: 14587 RVA: 0x001EA5A3 File Offset: 0x001E95A3
		// (set) Token: 0x060038FC RID: 14588 RVA: 0x001EA5AC File Offset: 0x001E95AC
		public NewItemPlaceholderPosition NewItemPlaceholderPosition
		{
			get
			{
				return this._newItemPlaceholderPosition;
			}
			set
			{
				base.VerifyRefreshNotDeferred();
				if (value != this._newItemPlaceholderPosition && this.IsAddingNew)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
					{
						"NewItemPlaceholderPosition",
						"AddNew"
					}));
				}
				if (value != this._newItemPlaceholderPosition && this._isRemoving)
				{
					this.DeferAction(delegate
					{
						this.NewItemPlaceholderPosition = value;
					});
					return;
				}
				NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = null;
				int num = -1;
				int num2 = -1;
				switch (value)
				{
				case NewItemPlaceholderPosition.None:
					switch (this._newItemPlaceholderPosition)
					{
					case NewItemPlaceholderPosition.AtBeginning:
						num = 0;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, CollectionView.NewItemPlaceholder, num);
						break;
					case NewItemPlaceholderPosition.AtEnd:
						num = this.InternalCount - 1;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, CollectionView.NewItemPlaceholder, num);
						break;
					}
					break;
				case NewItemPlaceholderPosition.AtBeginning:
					switch (this._newItemPlaceholderPosition)
					{
					case NewItemPlaceholderPosition.None:
						num2 = 0;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, CollectionView.NewItemPlaceholder, num2);
						break;
					case NewItemPlaceholderPosition.AtEnd:
						num = this.InternalCount - 1;
						num2 = 0;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, CollectionView.NewItemPlaceholder, num2, num);
						break;
					}
					break;
				case NewItemPlaceholderPosition.AtEnd:
					switch (this._newItemPlaceholderPosition)
					{
					case NewItemPlaceholderPosition.None:
						num2 = this.InternalCount;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, CollectionView.NewItemPlaceholder, num2);
						break;
					case NewItemPlaceholderPosition.AtBeginning:
						num = 0;
						num2 = this.InternalCount - 1;
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, CollectionView.NewItemPlaceholder, num2, num);
						break;
					}
					break;
				}
				if (notifyCollectionChangedEventArgs != null)
				{
					this._newItemPlaceholderPosition = value;
					if (!this.IsGrouping)
					{
						this.ProcessCollectionChangedWithAdjustedIndex(notifyCollectionChangedEventArgs, num, num2);
					}
					else
					{
						if (num >= 0)
						{
							int index = (num == 0) ? 0 : (this._group.Items.Count - 1);
							this._group.RemoveSpecialItem(index, CollectionView.NewItemPlaceholder, false);
						}
						if (num2 >= 0)
						{
							int index2 = (num2 == 0) ? 0 : this._group.Items.Count;
							this._group.InsertSpecialItem(index2, CollectionView.NewItemPlaceholder, false);
						}
					}
					this.OnPropertyChanged("NewItemPlaceholderPosition");
				}
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x060038FD RID: 14589 RVA: 0x001EA7D2 File Offset: 0x001E97D2
		public bool CanAddNew
		{
			get
			{
				return !this.IsEditingItem && !this.SourceList.IsFixedSize && this.CanConstructItem;
			}
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x060038FE RID: 14590 RVA: 0x001EA7F1 File Offset: 0x001E97F1
		public bool CanAddNewItem
		{
			get
			{
				return !this.IsEditingItem && !this.SourceList.IsFixedSize;
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x060038FF RID: 14591 RVA: 0x001EA80B File Offset: 0x001E980B
		private bool CanConstructItem
		{
			get
			{
				if (!this._isItemConstructorValid)
				{
					this.EnsureItemConstructor();
				}
				return this._itemConstructor != null;
			}
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x001EA828 File Offset: 0x001E9828
		private void EnsureItemConstructor()
		{
			if (!this._isItemConstructorValid)
			{
				Type itemType = base.GetItemType(true);
				if (itemType != null)
				{
					this._itemConstructor = itemType.GetConstructor(Type.EmptyTypes);
					this._isItemConstructorValid = true;
				}
			}
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x001EA868 File Offset: 0x001E9868
		public object AddNew()
		{
			base.VerifyRefreshNotDeferred();
			if (this.IsEditingItem)
			{
				this.CommitEdit();
			}
			this.CommitNew();
			if (!this.CanAddNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
				{
					"AddNew"
				}));
			}
			return this.AddNewCommon(this._itemConstructor.Invoke(null));
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x001EA8C8 File Offset: 0x001E98C8
		public object AddNewItem(object newItem)
		{
			base.VerifyRefreshNotDeferred();
			if (this.IsEditingItem)
			{
				this.CommitEdit();
			}
			this.CommitNew();
			if (!this.CanAddNewItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
				{
					"AddNewItem"
				}));
			}
			return this.AddNewCommon(newItem);
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x001EA91C File Offset: 0x001E991C
		private object AddNewCommon(object newItem)
		{
			BindingOperations.AccessCollection(this.SourceList, delegate
			{
				this.ProcessPendingChanges();
				this._newItemIndex = -2;
				int index = this.SourceList.Add(newItem);
				if (!(this.SourceList is INotifyCollectionChanged))
				{
					if (!ItemsControl.EqualsEx(newItem, this.SourceList[index]))
					{
						index = this.SourceList.IndexOf(newItem);
					}
					this.BeginAddNew(newItem, index);
				}
			}, true);
			this.MoveCurrentTo(newItem);
			ISupportInitialize supportInitialize = newItem as ISupportInitialize;
			if (supportInitialize != null)
			{
				supportInitialize.BeginInit();
			}
			IEditableObject editableObject = newItem as IEditableObject;
			if (editableObject != null)
			{
				editableObject.BeginEdit();
			}
			return newItem;
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x001EA994 File Offset: 0x001E9994
		private void BeginAddNew(object newItem, int index)
		{
			this.SetNewItem(newItem);
			this._newItemIndex = index;
			int num = -1;
			switch (this.NewItemPlaceholderPosition)
			{
			case NewItemPlaceholderPosition.None:
				num = (this.UsesLocalArray ? (this.InternalCount - 1) : this._newItemIndex);
				break;
			case NewItemPlaceholderPosition.AtBeginning:
				num = 1;
				break;
			case NewItemPlaceholderPosition.AtEnd:
				num = this.InternalCount - 2;
				break;
			}
			this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, num), -1, num);
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x001EAA04 File Offset: 0x001E9A04
		public void CommitNew()
		{
			if (this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
				{
					"CommitNew",
					"EditItem"
				}));
			}
			base.VerifyRefreshNotDeferred();
			if (this._newItem == CollectionView.NoNewItem)
			{
				return;
			}
			if (this.IsGrouping)
			{
				this.CommitNewForGrouping();
				return;
			}
			int num = 0;
			switch (this.NewItemPlaceholderPosition)
			{
			case NewItemPlaceholderPosition.None:
				num = (this.UsesLocalArray ? (this.InternalCount - 1) : this._newItemIndex);
				break;
			case NewItemPlaceholderPosition.AtBeginning:
				num = 1;
				break;
			case NewItemPlaceholderPosition.AtEnd:
				num = this.InternalCount - 2;
				break;
			}
			object obj = this.EndAddNew(false);
			int num2 = this.AdjustBefore(NotifyCollectionChangedAction.Add, obj, this._newItemIndex);
			if (num2 < 0)
			{
				this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, obj, num), num, -1);
				return;
			}
			if (num == num2)
			{
				if (this.UsesLocalArray)
				{
					if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
					{
						num2--;
					}
					this.InternalList.Insert(num2, obj);
					return;
				}
			}
			else
			{
				this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, obj, num2, num), num, num2);
			}
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x001EAB0C File Offset: 0x001E9B0C
		private void CommitNewForGrouping()
		{
			int index;
			switch (this.NewItemPlaceholderPosition)
			{
			default:
				index = this._group.Items.Count - 1;
				break;
			case NewItemPlaceholderPosition.AtBeginning:
				index = 1;
				break;
			case NewItemPlaceholderPosition.AtEnd:
				index = this._group.Items.Count - 2;
				break;
			}
			int newItemIndex = this._newItemIndex;
			object obj = this.EndAddNew(false);
			this._group.RemoveSpecialItem(index, obj, false);
			this.ProcessCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, obj, newItemIndex));
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x001EAB8C File Offset: 0x001E9B8C
		public void CancelNew()
		{
			if (this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
				{
					"CancelNew",
					"EditItem"
				}));
			}
			base.VerifyRefreshNotDeferred();
			if (this._newItem == CollectionView.NoNewItem)
			{
				return;
			}
			BindingOperations.AccessCollection(this.SourceList, delegate
			{
				base.ProcessPendingChanges();
				this.SourceList.RemoveAt(this._newItemIndex);
				if (this._newItem != CollectionView.NoNewItem)
				{
					int num = this.AdjustBefore(NotifyCollectionChangedAction.Remove, this._newItem, this._newItemIndex);
					object changedItem = this.EndAddNew(true);
					this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItem, num), num, -1);
				}
			}, true);
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x001EABF4 File Offset: 0x001E9BF4
		private object EndAddNew(bool cancel)
		{
			object newItem = this._newItem;
			this.SetNewItem(CollectionView.NoNewItem);
			IEditableObject editableObject = newItem as IEditableObject;
			if (editableObject != null)
			{
				if (cancel)
				{
					editableObject.CancelEdit();
				}
				else
				{
					editableObject.EndEdit();
				}
			}
			ISupportInitialize supportInitialize = newItem as ISupportInitialize;
			if (supportInitialize != null)
			{
				supportInitialize.EndInit();
			}
			return newItem;
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06003909 RID: 14601 RVA: 0x001EAC3D File Offset: 0x001E9C3D
		public bool IsAddingNew
		{
			get
			{
				return this._newItem != CollectionView.NoNewItem;
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x0600390A RID: 14602 RVA: 0x001EAC4F File Offset: 0x001E9C4F
		public object CurrentAddItem
		{
			get
			{
				if (!this.IsAddingNew)
				{
					return null;
				}
				return this._newItem;
			}
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x001EAC61 File Offset: 0x001E9C61
		private void SetNewItem(object item)
		{
			if (!ItemsControl.EqualsEx(item, this._newItem))
			{
				this._newItem = item;
				this.OnPropertyChanged("CurrentAddItem");
				this.OnPropertyChanged("IsAddingNew");
				this.OnPropertyChanged("CanRemove");
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x0600390C RID: 14604 RVA: 0x001EAC99 File Offset: 0x001E9C99
		public bool CanRemove
		{
			get
			{
				return !this.IsEditingItem && !this.IsAddingNew && !this.SourceList.IsFixedSize;
			}
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x001EACBC File Offset: 0x001E9CBC
		public void RemoveAt(int index)
		{
			if (this.IsEditingItem || this.IsAddingNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"RemoveAt"
				}));
			}
			base.VerifyRefreshNotDeferred();
			this.RemoveImpl(this.GetItemAt(index), index);
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x001EAD0C File Offset: 0x001E9D0C
		public void Remove(object item)
		{
			if (this.IsEditingItem || this.IsAddingNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Remove"
				}));
			}
			base.VerifyRefreshNotDeferred();
			int num = this.InternalIndexOf(item);
			if (num >= 0)
			{
				this.RemoveImpl(item, num);
			}
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x001EAD64 File Offset: 0x001E9D64
		private void RemoveImpl(object item, int index)
		{
			if (item == CollectionView.NewItemPlaceholder)
			{
				throw new InvalidOperationException(SR.Get("RemovingPlaceholder"));
			}
			BindingOperations.AccessCollection(this.SourceList, delegate
			{
				this.ProcessPendingChanges();
				if (index >= this.InternalCount || !ItemsControl.EqualsEx(item, this.GetItemAt(index)))
				{
					index = this.InternalIndexOf(item);
					if (index < 0)
					{
						return;
					}
				}
				int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
				int index2 = index - num;
				bool flag = !(this.SourceList is INotifyCollectionChanged);
				try
				{
					this._isRemoving = true;
					if (this.UsesLocalArray || this.IsGrouping)
					{
						if (flag)
						{
							index2 = this.SourceList.IndexOf(item);
							this.SourceList.RemoveAt(index2);
						}
						else
						{
							this.SourceList.Remove(item);
						}
					}
					else
					{
						this.SourceList.RemoveAt(index2);
					}
					if (flag)
					{
						this.ProcessCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index2));
					}
				}
				finally
				{
					this._isRemoving = false;
					this.DoDeferredActions();
				}
			}, true);
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x001EADC4 File Offset: 0x001E9DC4
		public void EditItem(object item)
		{
			base.VerifyRefreshNotDeferred();
			if (item == CollectionView.NewItemPlaceholder)
			{
				throw new ArgumentException(SR.Get("CannotEditPlaceholder"), "item");
			}
			if (this.IsAddingNew)
			{
				if (ItemsControl.EqualsEx(item, this._newItem))
				{
					return;
				}
				this.CommitNew();
			}
			this.CommitEdit();
			this.SetEditItem(item);
			IEditableObject editableObject = item as IEditableObject;
			if (editableObject != null)
			{
				editableObject.BeginEdit();
			}
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x001EAE30 File Offset: 0x001E9E30
		public void CommitEdit()
		{
			if (this.IsAddingNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
				{
					"CommitEdit",
					"AddNew"
				}));
			}
			base.VerifyRefreshNotDeferred();
			if (this._editItem == null)
			{
				return;
			}
			object editItem = this._editItem;
			IEditableObject editableObject = this._editItem as IEditableObject;
			this.SetEditItem(null);
			if (editableObject != null)
			{
				editableObject.EndEdit();
			}
			int num = this.InternalIndexOf(editItem);
			bool flag = num >= 0;
			bool flag2 = flag ? this.PassesFilter(editItem) : (this.SourceList.Contains(editItem) && this.PassesFilter(editItem));
			if (this.IsGrouping)
			{
				if (flag)
				{
					this.RemoveItemFromGroups(editItem);
				}
				if (flag2)
				{
					LiveShapingList liveShapingList = this.InternalList as LiveShapingList;
					LiveShapingItem lsi = (liveShapingList == null) ? null : liveShapingList.ItemAt(liveShapingList.IndexOf(editItem));
					this.AddItemToGroups(editItem, lsi);
				}
				return;
			}
			if (this.UsesLocalArray)
			{
				IList internalList = this.InternalList;
				int num2 = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
				int num3 = -1;
				if (flag)
				{
					if (!flag2)
					{
						this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, editItem, num), num, -1);
						return;
					}
					if (this.ActiveComparer != null)
					{
						int num4 = num - num2;
						if (num4 > 0 && this.ActiveComparer.Compare(internalList[num4 - 1], editItem) > 0)
						{
							num3 = internalList.Search(0, num4, editItem, this.ActiveComparer);
							if (num3 < 0)
							{
								num3 = ~num3;
							}
						}
						else if (num4 < internalList.Count - 1 && this.ActiveComparer.Compare(editItem, internalList[num4 + 1]) > 0)
						{
							num3 = internalList.Search(num4 + 1, internalList.Count - num4 - 1, editItem, this.ActiveComparer);
							if (num3 < 0)
							{
								num3 = ~num3;
							}
							num3--;
						}
						if (num3 >= 0)
						{
							this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, editItem, num3 + num2, num), num, num3 + num2);
							return;
						}
					}
				}
				else if (flag2)
				{
					num3 = this.AdjustBefore(NotifyCollectionChangedAction.Add, editItem, this.SourceList.IndexOf(editItem));
					this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, editItem, num3 + num2), -1, num3 + num2);
				}
			}
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x001EB050 File Offset: 0x001EA050
		public void CancelEdit()
		{
			if (this.IsAddingNew)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[]
				{
					"CancelEdit",
					"AddNew"
				}));
			}
			base.VerifyRefreshNotDeferred();
			if (this._editItem == null)
			{
				return;
			}
			IEditableObject editableObject = this._editItem as IEditableObject;
			this.SetEditItem(null);
			if (editableObject != null)
			{
				editableObject.CancelEdit();
				return;
			}
			throw new InvalidOperationException(SR.Get("CancelEditNotSupported"));
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x001EB0C8 File Offset: 0x001EA0C8
		private void ImplicitlyCancelEdit()
		{
			IEditableObject editableObject = this._editItem as IEditableObject;
			this.SetEditItem(null);
			if (editableObject != null)
			{
				editableObject.CancelEdit();
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06003914 RID: 14612 RVA: 0x001EB0F1 File Offset: 0x001EA0F1
		public bool CanCancelEdit
		{
			get
			{
				return this._editItem is IEditableObject;
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06003915 RID: 14613 RVA: 0x001EB101 File Offset: 0x001EA101
		public bool IsEditingItem
		{
			get
			{
				return this._editItem != null;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06003916 RID: 14614 RVA: 0x001EB10C File Offset: 0x001EA10C
		public object CurrentEditItem
		{
			get
			{
				return this._editItem;
			}
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x001EB114 File Offset: 0x001EA114
		private void SetEditItem(object item)
		{
			if (!ItemsControl.EqualsEx(item, this._editItem))
			{
				this._editItem = item;
				this.OnPropertyChanged("CurrentEditItem");
				this.OnPropertyChanged("IsEditingItem");
				this.OnPropertyChanged("CanCancelEdit");
				this.OnPropertyChanged("CanAddNew");
				this.OnPropertyChanged("CanAddNewItem");
				this.OnPropertyChanged("CanRemove");
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06003918 RID: 14616 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public bool CanChangeLiveSorting
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06003919 RID: 14617 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public bool CanChangeLiveFiltering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x0600391A RID: 14618 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public bool CanChangeLiveGrouping
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x0600391B RID: 14619 RVA: 0x001EB178 File Offset: 0x001EA178
		// (set) Token: 0x0600391C RID: 14620 RVA: 0x001EB180 File Offset: 0x001EA180
		public bool? IsLiveSorting
		{
			get
			{
				return this._isLiveSorting;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				bool? flag = value;
				bool? isLiveSorting = this._isLiveSorting;
				if (!(flag.GetValueOrDefault() == isLiveSorting.GetValueOrDefault() & flag != null == (isLiveSorting != null)))
				{
					this._isLiveSorting = value;
					this.RebuildLocalArray();
					this.OnPropertyChanged("IsLiveSorting");
				}
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x001EB1E5 File Offset: 0x001EA1E5
		// (set) Token: 0x0600391E RID: 14622 RVA: 0x001EB1F0 File Offset: 0x001EA1F0
		public bool? IsLiveFiltering
		{
			get
			{
				return this._isLiveFiltering;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				bool? flag = value;
				bool? isLiveFiltering = this._isLiveFiltering;
				if (!(flag.GetValueOrDefault() == isLiveFiltering.GetValueOrDefault() & flag != null == (isLiveFiltering != null)))
				{
					this._isLiveFiltering = value;
					this.RebuildLocalArray();
					this.OnPropertyChanged("IsLiveFiltering");
				}
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x0600391F RID: 14623 RVA: 0x001EB255 File Offset: 0x001EA255
		// (set) Token: 0x06003920 RID: 14624 RVA: 0x001EB260 File Offset: 0x001EA260
		public bool? IsLiveGrouping
		{
			get
			{
				return this._isLiveGrouping;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				bool? flag = value;
				bool? isLiveGrouping = this._isLiveGrouping;
				if (!(flag.GetValueOrDefault() == isLiveGrouping.GetValueOrDefault() & flag != null == (isLiveGrouping != null)))
				{
					this._isLiveGrouping = value;
					this.RebuildLocalArray();
					this.OnPropertyChanged("IsLiveGrouping");
				}
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06003921 RID: 14625 RVA: 0x001EB2C8 File Offset: 0x001EA2C8
		private bool IsLiveShaping
		{
			get
			{
				bool? flag = this.IsLiveSorting;
				bool flag2 = true;
				if (!(flag.GetValueOrDefault() == flag2 & flag != null))
				{
					flag = this.IsLiveFiltering;
					flag2 = true;
					if (!(flag.GetValueOrDefault() == flag2 & flag != null))
					{
						flag = this.IsLiveGrouping;
						flag2 = true;
						return flag.GetValueOrDefault() == flag2 & flag != null;
					}
				}
				return true;
			}
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06003922 RID: 14626 RVA: 0x001EB32C File Offset: 0x001EA32C
		public ObservableCollection<string> LiveSortingProperties
		{
			get
			{
				if (this._liveSortingProperties == null)
				{
					this._liveSortingProperties = new ObservableCollection<string>();
					this._liveSortingProperties.CollectionChanged += this.OnLivePropertyListChanged;
				}
				return this._liveSortingProperties;
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06003923 RID: 14627 RVA: 0x001EB35E File Offset: 0x001EA35E
		public ObservableCollection<string> LiveFilteringProperties
		{
			get
			{
				if (this._liveFilteringProperties == null)
				{
					this._liveFilteringProperties = new ObservableCollection<string>();
					this._liveFilteringProperties.CollectionChanged += this.OnLivePropertyListChanged;
				}
				return this._liveFilteringProperties;
			}
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06003924 RID: 14628 RVA: 0x001EB390 File Offset: 0x001EA390
		public ObservableCollection<string> LiveGroupingProperties
		{
			get
			{
				if (this._liveGroupingProperties == null)
				{
					this._liveGroupingProperties = new ObservableCollection<string>();
					this._liveGroupingProperties.CollectionChanged += this.OnLivePropertyListChanged;
				}
				return this._liveGroupingProperties;
			}
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x001EB3C2 File Offset: 0x001EA3C2
		private void OnLivePropertyListChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.IsLiveShaping)
			{
				this.RebuildLocalArray();
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06003926 RID: 14630 RVA: 0x001E53B0 File Offset: 0x001E43B0
		public ReadOnlyCollection<ItemPropertyInfo> ItemProperties
		{
			get
			{
				return base.GetItemProperties();
			}
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x001EB3D2 File Offset: 0x001EA3D2
		protected override void OnAllowsCrossThreadChangesChanged()
		{
			if (base.AllowsCrossThreadChanges)
			{
				BindingOperations.AccessCollection(this.SourceCollection, delegate
				{
					object syncRoot = base.SyncRoot;
					lock (syncRoot)
					{
						base.ClearPendingChanges();
						this.ShadowCollection = new ArrayList((ICollection)this.SourceCollection);
						if (!this.UsesLocalArray)
						{
							this._internalList = this.ShadowCollection;
						}
					}
				}, false);
				return;
			}
			this.ShadowCollection = null;
			if (!this.UsesLocalArray)
			{
				this._internalList = this.SourceList;
			}
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Obsolete("Replaced by OnAllowsCrossThreadChangesChanged")]
		protected override void OnBeginChangeLogging(NotifyCollectionChangedEventArgs args)
		{
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x001EB410 File Offset: 0x001EA410
		protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			this.ValidateCollectionChangedEventArgs(args);
			if (!this._isItemConstructorValid)
			{
				switch (args.Action)
				{
				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Reset:
					this.OnPropertyChanged("CanAddNew");
					break;
				}
			}
			int num = -1;
			int num2 = -1;
			if (base.AllowsCrossThreadChanges && args.Action != NotifyCollectionChangedAction.Reset)
			{
				if ((args.Action != NotifyCollectionChangedAction.Remove && args.NewStartingIndex < 0) || (args.Action != NotifyCollectionChangedAction.Add && args.OldStartingIndex < 0))
				{
					return;
				}
				this.AdjustShadowCopy(args);
			}
			if (args.Action == NotifyCollectionChangedAction.Reset)
			{
				if (this.IsEditingItem)
				{
					this.ImplicitlyCancelEdit();
				}
				if (this.IsAddingNew)
				{
					this._newItemIndex = this.SourceList.IndexOf(this._newItem);
					if (this._newItemIndex < 0)
					{
						this.EndAddNew(true);
					}
				}
				base.RefreshOrDefer();
				return;
			}
			if (args.Action == NotifyCollectionChangedAction.Add && this._newItemIndex == -2)
			{
				this.BeginAddNew(args.NewItems[0], args.NewStartingIndex);
				return;
			}
			if (args.Action != NotifyCollectionChangedAction.Remove)
			{
				num2 = this.AdjustBefore(NotifyCollectionChangedAction.Add, args.NewItems[0], args.NewStartingIndex);
			}
			if (args.Action != NotifyCollectionChangedAction.Add)
			{
				num = this.AdjustBefore(NotifyCollectionChangedAction.Remove, args.OldItems[0], args.OldStartingIndex);
				if (this.UsesLocalArray && num >= 0 && num < num2)
				{
					num2--;
				}
			}
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (this.IsAddingNew && args.NewStartingIndex <= this._newItemIndex)
				{
					this._newItemIndex++;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
			{
				if (this.IsAddingNew && args.OldStartingIndex < this._newItemIndex)
				{
					this._newItemIndex--;
				}
				object obj = args.OldItems[0];
				if (obj == this.CurrentEditItem)
				{
					this.ImplicitlyCancelEdit();
				}
				else if (obj == this.CurrentAddItem)
				{
					this.EndAddNew(true);
				}
				break;
			}
			case NotifyCollectionChangedAction.Move:
				if (this.IsAddingNew)
				{
					if (args.OldStartingIndex == this._newItemIndex)
					{
						this._newItemIndex = args.NewStartingIndex;
					}
					else if (args.OldStartingIndex < this._newItemIndex && this._newItemIndex <= args.NewStartingIndex)
					{
						this._newItemIndex--;
					}
					else if (args.NewStartingIndex <= this._newItemIndex && this._newItemIndex < args.OldStartingIndex)
					{
						this._newItemIndex++;
					}
				}
				if (this.ActiveComparer != null && num == num2)
				{
					return;
				}
				break;
			}
			this.ProcessCollectionChangedWithAdjustedIndex(args, num, num2);
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x001EB6BC File Offset: 0x001EA6BC
		private void ProcessCollectionChangedWithAdjustedIndex(NotifyCollectionChangedEventArgs args, int adjustedOldIndex, int adjustedNewIndex)
		{
			NotifyCollectionChangedAction notifyCollectionChangedAction = args.Action;
			if (adjustedOldIndex == adjustedNewIndex && adjustedOldIndex >= 0)
			{
				notifyCollectionChangedAction = NotifyCollectionChangedAction.Replace;
			}
			else if (adjustedOldIndex == -1)
			{
				if (adjustedNewIndex < 0 && args.Action != NotifyCollectionChangedAction.Add)
				{
					notifyCollectionChangedAction = NotifyCollectionChangedAction.Remove;
				}
			}
			else if (adjustedOldIndex < -1)
			{
				if (adjustedNewIndex >= 0)
				{
					notifyCollectionChangedAction = NotifyCollectionChangedAction.Add;
				}
				else if (notifyCollectionChangedAction == NotifyCollectionChangedAction.Move)
				{
					return;
				}
			}
			else if (adjustedNewIndex < 0)
			{
				notifyCollectionChangedAction = NotifyCollectionChangedAction.Remove;
			}
			else
			{
				notifyCollectionChangedAction = NotifyCollectionChangedAction.Move;
			}
			int num = this.IsGrouping ? 0 : ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? (this.IsAddingNew ? 2 : 1) : 0);
			int currentPosition = this.CurrentPosition;
			int currentPosition2 = this.CurrentPosition;
			object currentItem = this.CurrentItem;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			object obj = (args.OldItems != null && args.OldItems.Count > 0) ? args.OldItems[0] : null;
			object obj2 = (args.NewItems != null && args.NewItems.Count > 0) ? args.NewItems[0] : null;
			LiveShapingList liveShapingList = this.InternalList as LiveShapingList;
			NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = null;
			switch (notifyCollectionChangedAction)
			{
			case NotifyCollectionChangedAction.Add:
			{
				if (adjustedNewIndex == -2)
				{
					if (liveShapingList != null)
					{
						bool? isLiveFiltering = this.IsLiveFiltering;
						bool flag = true;
						if (isLiveFiltering.GetValueOrDefault() == flag & isLiveFiltering != null)
						{
							liveShapingList.AddFilteredItem(obj2);
						}
					}
					return;
				}
				bool flag2 = obj2 == CollectionView.NewItemPlaceholder || (this.IsAddingNew && ItemsControl.EqualsEx(this._newItem, obj2));
				if (this.UsesLocalArray && !flag2)
				{
					this.InternalList.Insert(adjustedNewIndex - num, obj2);
				}
				if (!this.IsGrouping)
				{
					this.AdjustCurrencyForAdd(adjustedNewIndex);
					args = new NotifyCollectionChangedEventArgs(notifyCollectionChangedAction, obj2, adjustedNewIndex);
				}
				else
				{
					LiveShapingItem lsi = (liveShapingList == null || flag2) ? null : liveShapingList.ItemAt(adjustedNewIndex - num);
					this.AddItemToGroups(obj2, lsi);
				}
				break;
			}
			case NotifyCollectionChangedAction.Remove:
				if (adjustedOldIndex == -2)
				{
					if (liveShapingList != null)
					{
						bool? isLiveFiltering = this.IsLiveFiltering;
						bool flag = true;
						if (isLiveFiltering.GetValueOrDefault() == flag & isLiveFiltering != null)
						{
							liveShapingList.RemoveFilteredItem(obj);
						}
					}
					return;
				}
				if (this.UsesLocalArray)
				{
					int num2 = adjustedOldIndex - num;
					if (num2 < this.InternalList.Count && ItemsControl.EqualsEx(this.ItemFrom(this.InternalList[num2]), obj))
					{
						this.InternalList.RemoveAt(num2);
					}
				}
				if (!this.IsGrouping)
				{
					this.AdjustCurrencyForRemove(adjustedOldIndex);
					args = new NotifyCollectionChangedEventArgs(notifyCollectionChangedAction, args.OldItems[0], adjustedOldIndex);
				}
				else
				{
					this.RemoveItemFromGroups(obj);
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (adjustedOldIndex == -2)
				{
					if (liveShapingList != null)
					{
						bool? isLiveFiltering = this.IsLiveFiltering;
						bool flag = true;
						if (isLiveFiltering.GetValueOrDefault() == flag & isLiveFiltering != null)
						{
							liveShapingList.ReplaceFilteredItem(obj, obj2);
						}
					}
					return;
				}
				if (this.UsesLocalArray)
				{
					this.InternalList[adjustedOldIndex - num] = obj2;
				}
				if (!this.IsGrouping)
				{
					this.AdjustCurrencyForReplace(adjustedOldIndex);
					args = new NotifyCollectionChangedEventArgs(notifyCollectionChangedAction, args.NewItems[0], args.OldItems[0], adjustedOldIndex);
				}
				else
				{
					LiveShapingItem lsi = (liveShapingList == null) ? null : liveShapingList.ItemAt(adjustedNewIndex - num);
					this.RemoveItemFromGroups(obj);
					this.AddItemToGroups(obj2, lsi);
				}
				break;
			case NotifyCollectionChangedAction.Move:
			{
				bool flag3 = ItemsControl.EqualsEx(obj, obj2);
				if (this.UsesLocalArray && (liveShapingList == null || !liveShapingList.IsRestoringLiveSorting))
				{
					int num3 = adjustedOldIndex - num;
					int num4 = adjustedNewIndex - num;
					if (num3 < this.InternalList.Count && ItemsControl.EqualsEx(this.InternalList[num3], obj))
					{
						if (CollectionView.NewItemPlaceholder != obj2)
						{
							this.InternalList.Move(num3, num4);
							if (!flag3)
							{
								this.InternalList[num4] = obj2;
							}
						}
						else
						{
							this.InternalList.RemoveAt(num3);
						}
					}
					else if (CollectionView.NewItemPlaceholder != obj2)
					{
						this.InternalList.Insert(num4, obj2);
					}
				}
				if (!this.IsGrouping)
				{
					this.AdjustCurrencyForMove(adjustedOldIndex, adjustedNewIndex);
					if (flag3)
					{
						args = new NotifyCollectionChangedEventArgs(notifyCollectionChangedAction, args.OldItems[0], adjustedNewIndex, adjustedOldIndex);
					}
					else
					{
						notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, args.NewItems, adjustedNewIndex);
						args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, args.OldItems, adjustedOldIndex);
					}
				}
				else
				{
					LiveShapingItem lsi = (liveShapingList == null) ? null : liveShapingList.ItemAt(adjustedNewIndex);
					if (flag3)
					{
						this.MoveItemWithinGroups(obj, lsi, adjustedOldIndex, adjustedNewIndex);
					}
					else
					{
						this.RemoveItemFromGroups(obj);
						this.AddItemToGroups(obj2, lsi);
					}
				}
				break;
			}
			default:
				Invariant.Assert(false, SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					notifyCollectionChangedAction
				}));
				break;
			}
			bool flag4 = this.IsCurrentAfterLast != isCurrentAfterLast;
			bool flag5 = this.IsCurrentBeforeFirst != isCurrentBeforeFirst;
			bool flag6 = this.CurrentPosition != currentPosition2;
			bool flag7 = this.CurrentItem != currentItem;
			isCurrentAfterLast = this.IsCurrentAfterLast;
			isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			currentPosition2 = this.CurrentPosition;
			currentItem = this.CurrentItem;
			if (!this.IsGrouping)
			{
				this.OnCollectionChanged(args);
				if (notifyCollectionChangedEventArgs != null)
				{
					this.OnCollectionChanged(notifyCollectionChangedEventArgs);
				}
				if (this.IsCurrentAfterLast != isCurrentAfterLast)
				{
					flag4 = false;
					isCurrentAfterLast = this.IsCurrentAfterLast;
				}
				if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
				{
					flag5 = false;
					isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
				}
				if (this.CurrentPosition != currentPosition2)
				{
					flag6 = false;
					currentPosition2 = this.CurrentPosition;
				}
				if (this.CurrentItem != currentItem)
				{
					flag7 = false;
					currentItem = this.CurrentItem;
				}
			}
			if (this._currentElementWasRemoved)
			{
				this.MoveCurrencyOffDeletedElement(currentPosition);
				flag4 = (flag4 || this.IsCurrentAfterLast != isCurrentAfterLast);
				flag5 = (flag5 || this.IsCurrentBeforeFirst != isCurrentBeforeFirst);
				flag6 = (flag6 || this.CurrentPosition != currentPosition2);
				flag7 = (flag7 || this.CurrentItem != currentItem);
			}
			if (flag4)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (flag5)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (flag6)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (flag7)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x001EBC98 File Offset: 0x001EAC98
		protected int InternalIndexOf(object item)
		{
			if (this.IsGrouping)
			{
				return this._group.LeafIndexOf(item);
			}
			if (item == CollectionView.NewItemPlaceholder)
			{
				switch (this.NewItemPlaceholderPosition)
				{
				case NewItemPlaceholderPosition.None:
					return -1;
				case NewItemPlaceholderPosition.AtBeginning:
					return 0;
				case NewItemPlaceholderPosition.AtEnd:
					return this.InternalCount - 1;
				}
			}
			else if (this.IsAddingNew && ItemsControl.EqualsEx(item, this._newItem))
			{
				switch (this.NewItemPlaceholderPosition)
				{
				case NewItemPlaceholderPosition.None:
					if (this.UsesLocalArray)
					{
						return this.InternalCount - 1;
					}
					break;
				case NewItemPlaceholderPosition.AtBeginning:
					return 1;
				case NewItemPlaceholderPosition.AtEnd:
					return this.InternalCount - 2;
				}
			}
			int num = this.InternalList.IndexOf(item);
			if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning && num >= 0)
			{
				num += (this.IsAddingNew ? 2 : 1);
			}
			return num;
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x001EBD64 File Offset: 0x001EAD64
		protected object InternalItemAt(int index)
		{
			if (this.IsGrouping)
			{
				return this._group.LeafAt(index);
			}
			switch (this.NewItemPlaceholderPosition)
			{
			case NewItemPlaceholderPosition.None:
				if (this.IsAddingNew && this.UsesLocalArray && index == this.InternalCount - 1)
				{
					return this._newItem;
				}
				break;
			case NewItemPlaceholderPosition.AtBeginning:
				if (index == 0)
				{
					return CollectionView.NewItemPlaceholder;
				}
				index--;
				if (this.IsAddingNew)
				{
					if (index == 0)
					{
						return this._newItem;
					}
					if (this.UsesLocalArray || index <= this._newItemIndex)
					{
						index--;
					}
				}
				break;
			case NewItemPlaceholderPosition.AtEnd:
				if (index == this.InternalCount - 1)
				{
					return CollectionView.NewItemPlaceholder;
				}
				if (this.IsAddingNew)
				{
					if (index == this.InternalCount - 2)
					{
						return this._newItem;
					}
					if (!this.UsesLocalArray && index >= this._newItemIndex)
					{
						index++;
					}
				}
				break;
			}
			return this.InternalList[index];
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x001EBE54 File Offset: 0x001EAE54
		protected bool InternalContains(object item)
		{
			if (item == CollectionView.NewItemPlaceholder)
			{
				return this.NewItemPlaceholderPosition > NewItemPlaceholderPosition.None;
			}
			if (this.IsGrouping)
			{
				return this._group.LeafIndexOf(item) >= 0;
			}
			return this.InternalList.Contains(item);
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x001EBE8F File Offset: 0x001EAE8F
		protected IEnumerator InternalGetEnumerator()
		{
			if (!this.IsGrouping)
			{
				return new CollectionView.PlaceholderAwareEnumerator(this, this.InternalList.GetEnumerator(), this.NewItemPlaceholderPosition, this._newItem);
			}
			return this._group.GetLeafEnumerator();
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x0600392F RID: 14639 RVA: 0x001EBEC4 File Offset: 0x001EAEC4
		protected bool UsesLocalArray
		{
			get
			{
				if (this.ActiveComparer != null || this.ActiveFilter != null)
				{
					return true;
				}
				if (this.IsGrouping)
				{
					bool? isLiveGrouping = this.IsLiveGrouping;
					bool flag = true;
					return isLiveGrouping.GetValueOrDefault() == flag & isLiveGrouping != null;
				}
				return false;
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06003930 RID: 14640 RVA: 0x001EBF08 File Offset: 0x001EAF08
		protected IList InternalList
		{
			get
			{
				return this._internalList;
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x001EBF10 File Offset: 0x001EAF10
		// (set) Token: 0x06003932 RID: 14642 RVA: 0x001EBF18 File Offset: 0x001EAF18
		protected IComparer ActiveComparer
		{
			get
			{
				return this._activeComparer;
			}
			set
			{
				this._activeComparer = value;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06003933 RID: 14643 RVA: 0x001EBF21 File Offset: 0x001EAF21
		// (set) Token: 0x06003934 RID: 14644 RVA: 0x001EBF29 File Offset: 0x001EAF29
		protected Predicate<object> ActiveFilter
		{
			get
			{
				return this._activeFilter;
			}
			set
			{
				this._activeFilter = value;
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06003935 RID: 14645 RVA: 0x001EBF32 File Offset: 0x001EAF32
		protected bool IsGrouping
		{
			get
			{
				return this._isGrouping;
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06003936 RID: 14646 RVA: 0x001EBF3C File Offset: 0x001EAF3C
		protected int InternalCount
		{
			get
			{
				if (this.IsGrouping)
				{
					return this._group.ItemCount;
				}
				int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.None) ? 0 : 1;
				if (this.UsesLocalArray && this.IsAddingNew)
				{
					num++;
				}
				return num + this.InternalList.Count;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x001EBF8B File Offset: 0x001EAF8B
		// (set) Token: 0x06003938 RID: 14648 RVA: 0x001EBF93 File Offset: 0x001EAF93
		internal ArrayList ShadowCollection
		{
			get
			{
				return this._shadowCollection;
			}
			set
			{
				this._shadowCollection = value;
			}
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x001EBF9C File Offset: 0x001EAF9C
		internal void AdjustShadowCopy(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (e.NewStartingIndex > -1)
				{
					this.ShadowCollection.Insert(e.NewStartingIndex, e.NewItems[0]);
					return;
				}
				this.ShadowCollection.Add(e.NewItems[0]);
				return;
			case NotifyCollectionChangedAction.Remove:
				if (e.OldStartingIndex > -1)
				{
					this.ShadowCollection.RemoveAt(e.OldStartingIndex);
					return;
				}
				this.ShadowCollection.Remove(e.OldItems[0]);
				return;
			case NotifyCollectionChangedAction.Replace:
			{
				if (e.OldStartingIndex > -1)
				{
					this.ShadowCollection[e.OldStartingIndex] = e.NewItems[0];
					return;
				}
				int num = this.ShadowCollection.IndexOf(e.OldItems[0]);
				this.ShadowCollection[num] = e.NewItems[0];
				return;
			}
			case NotifyCollectionChangedAction.Move:
			{
				int num = e.OldStartingIndex;
				if (num < 0)
				{
					num = this.ShadowCollection.IndexOf(e.NewItems[0]);
				}
				this.ShadowCollection.RemoveAt(num);
				this.ShadowCollection.Insert(e.NewStartingIndex, e.NewItems[0]);
				return;
			}
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x0600393A RID: 14650 RVA: 0x001EC106 File Offset: 0x001EB106
		internal bool HasSortDescriptions
		{
			get
			{
				return this._sort != null && this._sort.Count > 0;
			}
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x001EC120 File Offset: 0x001EB120
		internal static IComparer PrepareComparer(IComparer customSort, SortDescriptionCollection sort, Func<CollectionView> lazyGetCollectionView)
		{
			if (customSort != null)
			{
				return customSort;
			}
			if (sort != null && sort.Count > 0)
			{
				CollectionView collectionView = lazyGetCollectionView();
				if (collectionView.SourceCollection != null)
				{
					IComparer comparer = SystemXmlHelper.PrepareXmlComparer(collectionView.SourceCollection, sort, collectionView.Culture);
					if (comparer != null)
					{
						return comparer;
					}
				}
				return new SortFieldComparer(sort, collectionView.Culture);
			}
			return null;
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x0600393C RID: 14652 RVA: 0x001EC173 File Offset: 0x001EB173
		private bool IsCurrentInView
		{
			get
			{
				return 0 <= this.CurrentPosition && this.CurrentPosition < this.InternalCount;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x0600393D RID: 14653 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		private bool CanGroupNamesChange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x0600393E RID: 14654 RVA: 0x001EC18E File Offset: 0x001EB18E
		private IList SourceList
		{
			get
			{
				return this.SourceCollection as IList;
			}
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x00150920 File Offset: 0x0014F920
		private void ValidateCollectionChangedEventArgs(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (e.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (e.NewItems.Count != 1 || e.OldItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (e.NewItems.Count != 1)
				{
					throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
				}
				if (e.NewStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("CannotMoveToUnknownPosition"));
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x001EC19C File Offset: 0x001EB19C
		private void PrepareLocalArray()
		{
			this.PrepareShaping();
			LiveShapingList liveShapingList = this._internalList as LiveShapingList;
			if (liveShapingList != null)
			{
				liveShapingList.LiveShapingDirty -= this.OnLiveShapingDirty;
				liveShapingList.Clear();
			}
			IList list;
			if (!base.AllowsCrossThreadChanges)
			{
				list = (this.SourceCollection as IList);
			}
			else
			{
				IList list2 = this.ShadowCollection;
				list = list2;
			}
			IList list3 = list;
			if (!this.UsesLocalArray)
			{
				this._internalList = list3;
			}
			else
			{
				int count = list3.Count;
				IList list4;
				if (!this.IsLiveShaping)
				{
					IList list2 = new ArrayList(count);
					list4 = list2;
				}
				else
				{
					IList list2 = new LiveShapingList(this, this.GetLiveShapingFlags(), this.ActiveComparer);
					list4 = list2;
				}
				IList list5 = list4;
				liveShapingList = (list5 as LiveShapingList);
				for (int i = 0; i < count; i++)
				{
					if (!this.IsAddingNew || i != this._newItemIndex)
					{
						object obj = list3[i];
						if (this.ActiveFilter == null || this.ActiveFilter(obj))
						{
							list5.Add(obj);
						}
						else
						{
							bool? isLiveFiltering = this.IsLiveFiltering;
							bool flag = true;
							if (isLiveFiltering.GetValueOrDefault() == flag & isLiveFiltering != null)
							{
								liveShapingList.AddFilteredItem(obj);
							}
						}
					}
				}
				if (this.ActiveComparer != null)
				{
					list5.Sort(this.ActiveComparer);
				}
				if (liveShapingList != null)
				{
					liveShapingList.LiveShapingDirty += this.OnLiveShapingDirty;
				}
				this._internalList = list5;
			}
			this.PrepareGroups();
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x001EC2EE File Offset: 0x001EB2EE
		private void OnLiveShapingDirty(object sender, EventArgs e)
		{
			this.IsLiveShapingDirty = true;
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x001EC2F7 File Offset: 0x001EB2F7
		private void RebuildLocalArray()
		{
			if (base.IsRefreshDeferred)
			{
				base.RefreshOrDefer();
				return;
			}
			this.PrepareLocalArray();
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x001EC310 File Offset: 0x001EB310
		private void MoveCurrencyOffDeletedElement(int oldCurrentPosition)
		{
			int num = this.InternalCount - 1;
			int num2 = (oldCurrentPosition < num) ? oldCurrentPosition : num;
			this._currentElementWasRemoved = false;
			base.OnCurrentChanging();
			if (num2 < 0)
			{
				base.SetCurrent(null, num2);
			}
			else
			{
				base.SetCurrent(this.InternalItemAt(num2), num2);
			}
			this.OnCurrentChanged();
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x001EC360 File Offset: 0x001EB360
		private int AdjustBefore(NotifyCollectionChangedAction action, object item, int index)
		{
			if (action == NotifyCollectionChangedAction.Reset)
			{
				return -1;
			}
			if (item == CollectionView.NewItemPlaceholder)
			{
				if (this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)
				{
					return this.InternalCount - 1;
				}
				return 0;
			}
			else if (this.IsAddingNew && this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.None && ItemsControl.EqualsEx(item, this._newItem))
			{
				if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
				{
					return 1;
				}
				if (!this.UsesLocalArray)
				{
					return index;
				}
				return this.InternalCount - 2;
			}
			else
			{
				int num = this.IsGrouping ? 0 : ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? (this.IsAddingNew ? 2 : 1) : 0);
				IEnumerable enumerable;
				if (!base.AllowsCrossThreadChanges)
				{
					enumerable = this.SourceCollection;
				}
				else
				{
					IEnumerable shadowCollection = this.ShadowCollection;
					enumerable = shadowCollection;
				}
				IList list = enumerable as IList;
				if (index < -1 || index > list.Count)
				{
					throw new InvalidOperationException(SR.Get("CollectionChangeIndexOutOfRange", new object[]
					{
						index,
						list.Count
					}));
				}
				if (action == NotifyCollectionChangedAction.Add)
				{
					if (index >= 0)
					{
						if (!ItemsControl.EqualsEx(item, list[index]))
						{
							throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[]
							{
								index
							}));
						}
					}
					else
					{
						index = list.IndexOf(item);
						if (index < 0)
						{
							throw new InvalidOperationException(SR.Get("AddedItemNotInCollection"));
						}
					}
				}
				if (!this.UsesLocalArray)
				{
					if (this.IsAddingNew && this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.None && index > this._newItemIndex)
					{
						index--;
					}
					if (index >= 0)
					{
						return index + num;
					}
					return index;
				}
				else
				{
					if (action == NotifyCollectionChangedAction.Add)
					{
						if (!this.PassesFilter(item))
						{
							return -2;
						}
						if (!this.UsesLocalArray)
						{
							index = -1;
						}
						else if (this.ActiveComparer != null)
						{
							index = this.InternalList.Search(item, this.ActiveComparer);
							if (index < 0)
							{
								index = ~index;
							}
						}
						else
						{
							index = this.MatchingSearch(item, index, list, this.InternalList);
						}
					}
					else if (action == NotifyCollectionChangedAction.Remove)
					{
						if (!this.IsAddingNew || item != this._newItem)
						{
							index = this.InternalList.IndexOf(item);
							if (index < 0)
							{
								return -2;
							}
						}
						else
						{
							switch (this.NewItemPlaceholderPosition)
							{
							case NewItemPlaceholderPosition.None:
								return this.InternalCount - 1;
							case NewItemPlaceholderPosition.AtBeginning:
								return 1;
							case NewItemPlaceholderPosition.AtEnd:
								return this.InternalCount - 2;
							}
						}
					}
					else
					{
						index = -1;
					}
					if (index >= 0)
					{
						return index + num;
					}
					return index;
				}
			}
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x001EC588 File Offset: 0x001EB588
		private int MatchingSearch(object item, int index, IList ilFull, IList ilPartial)
		{
			int num = 0;
			int num2 = 0;
			while (num < index && num2 < this.InternalList.Count)
			{
				if (ItemsControl.EqualsEx(ilFull[num], ilPartial[num2]))
				{
					num++;
					num2++;
				}
				else if (ItemsControl.EqualsEx(item, ilPartial[num2]))
				{
					num2++;
				}
				else
				{
					num++;
				}
			}
			return num2;
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x001EC5E8 File Offset: 0x001EB5E8
		private void AdjustCurrencyForAdd(int index)
		{
			if (this.InternalCount == 1)
			{
				base.SetCurrent(null, -1);
				return;
			}
			if (index <= this.CurrentPosition)
			{
				int num = this.CurrentPosition + 1;
				if (num < this.InternalCount)
				{
					base.SetCurrent(this.GetItemAt(num), num);
					return;
				}
				base.SetCurrent(null, this.InternalCount);
			}
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x001EC63E File Offset: 0x001EB63E
		private void AdjustCurrencyForRemove(int index)
		{
			if (index < this.CurrentPosition)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
				return;
			}
			if (index == this.CurrentPosition)
			{
				this._currentElementWasRemoved = true;
			}
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x001E6220 File Offset: 0x001E5220
		private void AdjustCurrencyForMove(int oldIndex, int newIndex)
		{
			if (oldIndex == this.CurrentPosition)
			{
				base.SetCurrent(this.GetItemAt(newIndex), newIndex);
				return;
			}
			if (oldIndex < this.CurrentPosition && this.CurrentPosition <= newIndex)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
				return;
			}
			if (newIndex <= this.CurrentPosition && this.CurrentPosition < oldIndex)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition + 1);
			}
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x001EC66E File Offset: 0x001EB66E
		private void AdjustCurrencyForReplace(int index)
		{
			if (index == this.CurrentPosition)
			{
				this._currentElementWasRemoved = true;
			}
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x001EC680 File Offset: 0x001EB680
		private void PrepareShaping()
		{
			this.ActiveComparer = ListCollectionView.PrepareComparer(this._customSort, this._sort, () => this);
			this.ActiveFilter = this.Filter;
			this._group.Clear();
			this._group.Initialize();
			this._isGrouping = (this._group.GroupBy != null);
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x001EC6E8 File Offset: 0x001EB6E8
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

		// Token: 0x0600394C RID: 14668 RVA: 0x001EC754 File Offset: 0x001EB754
		private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.IsAddingNew || this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Sorting"
				}));
			}
			if (this._sort.Count > 0)
			{
				this._customSort = null;
			}
			base.RefreshOrDefer();
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x001EC7AC File Offset: 0x001EB7AC
		private void PrepareGroups()
		{
			if (!this._isGrouping)
			{
				return;
			}
			IComparer activeComparer = this.ActiveComparer;
			if (activeComparer != null)
			{
				this._group.ActiveComparer = activeComparer;
			}
			else
			{
				CollectionViewGroupInternal.IListComparer listComparer = this._group.ActiveComparer as CollectionViewGroupInternal.IListComparer;
				if (listComparer != null)
				{
					listComparer.ResetList(this.InternalList);
				}
				else
				{
					this._group.ActiveComparer = new CollectionViewGroupInternal.IListComparer(this.InternalList);
				}
			}
			if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				this._group.InsertSpecialItem(0, CollectionView.NewItemPlaceholder, true);
				if (this.IsAddingNew)
				{
					this._group.InsertSpecialItem(1, this._newItem, true);
				}
			}
			bool? isLiveGrouping = this.IsLiveGrouping;
			isLiveGrouping.GetValueOrDefault();
			bool flag = isLiveGrouping != null;
			LiveShapingList liveShapingList = this.InternalList as LiveShapingList;
			int i = 0;
			int count = this.InternalList.Count;
			while (i < count)
			{
				object obj = this.InternalList[i];
				LiveShapingItem lsi = (liveShapingList != null) ? liveShapingList.ItemAt(i) : null;
				if (!this.IsAddingNew || !ItemsControl.EqualsEx(this._newItem, obj))
				{
					this._group.AddToSubgroups(obj, lsi, true);
				}
				i++;
			}
			if (this.IsAddingNew && this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)
			{
				this._group.InsertSpecialItem(this._group.Items.Count, this._newItem, true);
			}
			if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				this._group.InsertSpecialItem(this._group.Items.Count, CollectionView.NewItemPlaceholder, true);
			}
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x001EC92F File Offset: 0x001EB92F
		private void OnGroupChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				this.AdjustCurrencyForAdd(e.NewStartingIndex);
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				this.AdjustCurrencyForRemove(e.OldStartingIndex);
			}
			this.OnCollectionChanged(e);
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x001EC963 File Offset: 0x001EB963
		private void OnGroupByChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.IsAddingNew || this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Grouping"
				}));
			}
			base.RefreshOrDefer();
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x001EC963 File Offset: 0x001EB963
		private void OnGroupDescriptionChanged(object sender, EventArgs e)
		{
			if (this.IsAddingNew || this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Grouping"
				}));
			}
			base.RefreshOrDefer();
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x001EC99C File Offset: 0x001EB99C
		private void AddItemToGroups(object item, LiveShapingItem lsi)
		{
			if (this.IsAddingNew && item == this._newItem)
			{
				int index;
				switch (this.NewItemPlaceholderPosition)
				{
				default:
					index = this._group.Items.Count;
					break;
				case NewItemPlaceholderPosition.AtBeginning:
					index = 1;
					break;
				case NewItemPlaceholderPosition.AtEnd:
					index = this._group.Items.Count - 1;
					break;
				}
				this._group.InsertSpecialItem(index, item, false);
				return;
			}
			this._group.AddToSubgroups(item, lsi, false);
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x001ECA1A File Offset: 0x001EBA1A
		private void RemoveItemFromGroups(object item)
		{
			if (this.CanGroupNamesChange || this._group.RemoveFromSubgroups(item))
			{
				this._group.RemoveItemFromSubgroupsByExhaustiveSearch(item);
			}
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x001ECA3E File Offset: 0x001EBA3E
		private void MoveItemWithinGroups(object item, LiveShapingItem lsi, int oldIndex, int newIndex)
		{
			this._group.MoveWithinSubgroups(item, lsi, this.InternalList, oldIndex, newIndex);
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x001ECA58 File Offset: 0x001EBA58
		private LiveShapingFlags GetLiveShapingFlags()
		{
			LiveShapingFlags liveShapingFlags = (LiveShapingFlags)0;
			bool? flag = this.IsLiveSorting;
			bool flag2 = true;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				liveShapingFlags |= LiveShapingFlags.Sorting;
			}
			flag = this.IsLiveFiltering;
			flag2 = true;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				liveShapingFlags |= LiveShapingFlags.Filtering;
			}
			flag = this.IsLiveGrouping;
			flag2 = true;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				liveShapingFlags |= LiveShapingFlags.Grouping;
			}
			return liveShapingFlags;
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x001ECACC File Offset: 0x001EBACC
		internal void RestoreLiveShaping()
		{
			LiveShapingList liveShapingList = this.InternalList as LiveShapingList;
			if (liveShapingList == null)
			{
				return;
			}
			if (this.ActiveComparer != null)
			{
				if ((double)liveShapingList.SortDirtyItems.Count / (double)(liveShapingList.Count + 1) < 0.8)
				{
					using (List<LiveShapingItem>.Enumerator enumerator = liveShapingList.SortDirtyItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							LiveShapingItem liveShapingItem = enumerator.Current;
							if (liveShapingItem.IsSortDirty && !liveShapingItem.IsDeleted && liveShapingItem.ForwardChanges)
							{
								liveShapingItem.IsSortDirty = false;
								liveShapingItem.IsSortPendingClean = false;
								int num;
								int num2;
								liveShapingList.FindPosition(liveShapingItem, out num, out num2);
								if (num != num2)
								{
									if (num < num2)
									{
										num2--;
									}
									this.ProcessLiveShapingCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, liveShapingItem.Item, num, num2), num, num2);
								}
							}
						}
						goto IL_DB;
					}
				}
				liveShapingList.RestoreLiveSortingByInsertionSort(new Action<NotifyCollectionChangedEventArgs, int, int>(this.ProcessLiveShapingCollectionChange));
			}
			IL_DB:
			liveShapingList.SortDirtyItems.Clear();
			if (this.ActiveFilter != null)
			{
				foreach (LiveShapingItem liveShapingItem2 in liveShapingList.FilterDirtyItems)
				{
					if (liveShapingItem2.IsFilterDirty && liveShapingItem2.ForwardChanges)
					{
						object item = liveShapingItem2.Item;
						bool failsFilter = liveShapingItem2.FailsFilter;
						bool flag = !this.PassesFilter(item);
						if (failsFilter != flag)
						{
							if (flag)
							{
								int num3 = liveShapingList.IndexOf(liveShapingItem2);
								this.ProcessLiveShapingCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, num3), num3, -1);
								liveShapingList.AddFilteredItem(liveShapingItem2);
							}
							else
							{
								liveShapingList.RemoveFilteredItem(liveShapingItem2);
								int num3;
								if (this.ActiveComparer != null)
								{
									num3 = liveShapingList.Search(0, liveShapingList.Count, item);
									if (num3 < 0)
									{
										num3 = ~num3;
									}
								}
								else
								{
									IEnumerable enumerable;
									if (!base.AllowsCrossThreadChanges)
									{
										enumerable = this.SourceCollection;
									}
									else
									{
										IEnumerable shadowCollection = this.ShadowCollection;
										enumerable = shadowCollection;
									}
									IList list = enumerable as IList;
									num3 = liveShapingItem2.GetAndClearStartingIndex();
									while (num3 < list.Count && !ItemsControl.EqualsEx(item, list[num3]))
									{
										num3++;
									}
									liveShapingList.SetStartingIndexForFilteredItem(item, num3 + 1);
									num3 = this.MatchingSearch(item, num3, list, liveShapingList);
								}
								this.ProcessLiveShapingCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, num3), -1, num3);
							}
						}
						liveShapingItem2.IsFilterDirty = false;
					}
				}
			}
			liveShapingList.FilterDirtyItems.Clear();
			if (this.IsGrouping)
			{
				List<AbandonedGroupItem> deleteList = new List<AbandonedGroupItem>();
				foreach (LiveShapingItem liveShapingItem3 in liveShapingList.GroupDirtyItems)
				{
					if (liveShapingItem3.IsGroupDirty && !liveShapingItem3.IsDeleted && liveShapingItem3.ForwardChanges)
					{
						this._group.RestoreGrouping(liveShapingItem3, deleteList);
						liveShapingItem3.IsGroupDirty = false;
					}
				}
				this._group.DeleteAbandonedGroupItems(deleteList);
			}
			liveShapingList.GroupDirtyItems.Clear();
			this.IsLiveShapingDirty = false;
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x001ECE04 File Offset: 0x001EBE04
		private void ProcessLiveShapingCollectionChange(NotifyCollectionChangedEventArgs args, int oldIndex, int newIndex)
		{
			if (!this.IsGrouping && this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				if (oldIndex >= 0)
				{
					oldIndex++;
				}
				if (newIndex >= 0)
				{
					newIndex++;
				}
			}
			this.ProcessCollectionChangedWithAdjustedIndex(args, oldIndex, newIndex);
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06003957 RID: 14679 RVA: 0x001ECE32 File Offset: 0x001EBE32
		// (set) Token: 0x06003958 RID: 14680 RVA: 0x001ECE3A File Offset: 0x001EBE3A
		internal bool IsLiveShapingDirty
		{
			get
			{
				return this._isLiveShapingDirty;
			}
			set
			{
				if (value == this._isLiveShapingDirty)
				{
					return;
				}
				this._isLiveShapingDirty = value;
				if (value)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(this.RestoreLiveShaping));
				}
			}
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x001ECE6C File Offset: 0x001EBE6C
		private object ItemFrom(object o)
		{
			LiveShapingItem liveShapingItem = o as LiveShapingItem;
			if (liveShapingItem != null)
			{
				return liveShapingItem.Item;
			}
			return o;
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x00150A1C File Offset: 0x0014FA1C
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x001ECE8B File Offset: 0x001EBE8B
		private void DeferAction(Action action)
		{
			if (this._deferredActions == null)
			{
				this._deferredActions = new List<Action>();
			}
			this._deferredActions.Add(action);
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x001ECEAC File Offset: 0x001EBEAC
		private void DoDeferredActions()
		{
			if (this._deferredActions != null)
			{
				List<Action> deferredActions = this._deferredActions;
				this._deferredActions = null;
				foreach (Action action in deferredActions)
				{
					action();
				}
			}
		}

		// Token: 0x04001D52 RID: 7506
		private const double LiveSortingDensityThreshold = 0.8;

		// Token: 0x04001D53 RID: 7507
		private IList _internalList;

		// Token: 0x04001D54 RID: 7508
		private CollectionViewGroupRoot _group;

		// Token: 0x04001D55 RID: 7509
		private bool _isGrouping;

		// Token: 0x04001D56 RID: 7510
		private IComparer _activeComparer;

		// Token: 0x04001D57 RID: 7511
		private Predicate<object> _activeFilter;

		// Token: 0x04001D58 RID: 7512
		private SortDescriptionCollection _sort;

		// Token: 0x04001D59 RID: 7513
		private IComparer _customSort;

		// Token: 0x04001D5A RID: 7514
		private ArrayList _shadowCollection;

		// Token: 0x04001D5B RID: 7515
		private bool _currentElementWasRemoved;

		// Token: 0x04001D5C RID: 7516
		private object _newItem = CollectionView.NoNewItem;

		// Token: 0x04001D5D RID: 7517
		private object _editItem;

		// Token: 0x04001D5E RID: 7518
		private int _newItemIndex;

		// Token: 0x04001D5F RID: 7519
		private NewItemPlaceholderPosition _newItemPlaceholderPosition;

		// Token: 0x04001D60 RID: 7520
		private bool _isItemConstructorValid;

		// Token: 0x04001D61 RID: 7521
		private ConstructorInfo _itemConstructor;

		// Token: 0x04001D62 RID: 7522
		private List<Action> _deferredActions;

		// Token: 0x04001D63 RID: 7523
		private ObservableCollection<string> _liveSortingProperties;

		// Token: 0x04001D64 RID: 7524
		private ObservableCollection<string> _liveFilteringProperties;

		// Token: 0x04001D65 RID: 7525
		private ObservableCollection<string> _liveGroupingProperties;

		// Token: 0x04001D66 RID: 7526
		private bool? _isLiveSorting = new bool?(false);

		// Token: 0x04001D67 RID: 7527
		private bool? _isLiveFiltering = new bool?(false);

		// Token: 0x04001D68 RID: 7528
		private bool? _isLiveGrouping = new bool?(false);

		// Token: 0x04001D69 RID: 7529
		private bool _isLiveShapingDirty;

		// Token: 0x04001D6A RID: 7530
		private bool _isRemoving;

		// Token: 0x04001D6B RID: 7531
		private const int _unknownIndex = -1;
	}
}
