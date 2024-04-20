using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000450 RID: 1104
	public sealed class BindingListCollectionView : CollectionView, IComparer, IEditableCollectionView, ICollectionViewLiveShaping, IItemProperties
	{
		// Token: 0x0600372F RID: 14127 RVA: 0x001E4510 File Offset: 0x001E3510
		public BindingListCollectionView(IBindingList list) : base(list)
		{
			this.InternalList = list;
			this._blv = (list as IBindingListView);
			this._isDataView = SystemDataHelper.IsDataView(list);
			this.SubscribeToChanges();
			this._group = new CollectionViewGroupRoot(this);
			this._group.GroupDescriptionChanged += this.OnGroupDescriptionChanged;
			((INotifyCollectionChanged)this._group).CollectionChanged += this.OnGroupChanged;
			((INotifyCollectionChanged)this._group.GroupDescriptions).CollectionChanged += this.OnGroupByChanged;
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x001E45B6 File Offset: 0x001E35B6
		public override bool PassesFilter(object item)
		{
			return !this.IsCustomFilterSet || this.Contains(item);
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x001E45C9 File Offset: 0x001E35C9
		public override bool Contains(object item)
		{
			base.VerifyRefreshNotDeferred();
			if (item != CollectionView.NewItemPlaceholder)
			{
				return this.CollectionProxy.Contains(item);
			}
			return this.NewItemPlaceholderPosition > NewItemPlaceholderPosition.None;
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x001E45EF File Offset: 0x001E35EF
		public override bool MoveCurrentToPosition(int position)
		{
			base.VerifyRefreshNotDeferred();
			if (position < -1 || position > this.InternalCount)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this._MoveTo(position);
			return this.IsCurrentInView;
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x001E461C File Offset: 0x001E361C
		int IComparer.Compare(object o1, object o2)
		{
			int num = this.InternalIndexOf(o1);
			int num2 = this.InternalIndexOf(o2);
			return num - num2;
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x001E463A File Offset: 0x001E363A
		public override int IndexOf(object item)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalIndexOf(item);
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x001E4649 File Offset: 0x001E3649
		public override object GetItemAt(int index)
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalItemAt(index);
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x001E4658 File Offset: 0x001E3658
		protected override IEnumerator GetEnumerator()
		{
			base.VerifyRefreshNotDeferred();
			return this.InternalGetEnumerator();
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x001E4666 File Offset: 0x001E3666
		public override void DetachFromSourceCollection()
		{
			if (this.InternalList != null && this.InternalList.SupportsChangeNotification)
			{
				this.InternalList.ListChanged -= this.OnListChanged;
			}
			this.InternalList = null;
			base.DetachFromSourceCollection();
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06003738 RID: 14136 RVA: 0x001E46A4 File Offset: 0x001E36A4
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				if (this.InternalList.SupportsSorting)
				{
					if (this._sort == null)
					{
						bool allowMultipleDescriptions = this._blv != null && this._blv.SupportsAdvancedSorting;
						this._sort = new BindingListCollectionView.BindingListSortDescriptionCollection(allowMultipleDescriptions);
						((INotifyCollectionChanged)this._sort).CollectionChanged += this.SortDescriptionsChanged;
					}
					return this._sort;
				}
				return SortDescriptionCollection.Empty;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06003739 RID: 14137 RVA: 0x001E470C File Offset: 0x001E370C
		public override bool CanSort
		{
			get
			{
				return this.InternalList.SupportsSorting;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x0600373A RID: 14138 RVA: 0x001E4719 File Offset: 0x001E3719
		// (set) Token: 0x0600373B RID: 14139 RVA: 0x001E4721 File Offset: 0x001E3721
		private IComparer ActiveComparer
		{
			get
			{
				return this._comparer;
			}
			set
			{
				this._comparer = value;
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x0600373C RID: 14140 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanFilter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x001E472A File Offset: 0x001E372A
		// (set) Token: 0x0600373E RID: 14142 RVA: 0x001E4734 File Offset: 0x001E3734
		public string CustomFilter
		{
			get
			{
				return this._customFilter;
			}
			set
			{
				if (!this.CanCustomFilter)
				{
					throw new NotSupportedException(SR.Get("BindingListCannotCustomFilter"));
				}
				if (this.IsAddingNew || this.IsEditingItem)
				{
					throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
					{
						"CustomFilter"
					}));
				}
				if (base.AllowsCrossThreadChanges)
				{
					base.VerifyAccess();
				}
				this._customFilter = value;
				base.RefreshOrDefer();
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x0600373F RID: 14143 RVA: 0x001E47A2 File Offset: 0x001E37A2
		public bool CanCustomFilter
		{
			get
			{
				return this._blv != null && this._blv.SupportsFiltering;
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06003740 RID: 14144 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanGroup
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06003741 RID: 14145 RVA: 0x001E47B9 File Offset: 0x001E37B9
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._group.GroupDescriptions;
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06003742 RID: 14146 RVA: 0x001E47C6 File Offset: 0x001E37C6
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				if (!this._isGrouping)
				{
					return null;
				}
				return this._group.Items;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06003743 RID: 14147 RVA: 0x001E47DD File Offset: 0x001E37DD
		// (set) Token: 0x06003744 RID: 14148 RVA: 0x001E47EC File Offset: 0x001E37EC
		[DefaultValue(null)]
		public GroupDescriptionSelectorCallback GroupBySelector
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
						"GroupBySelector"
					}));
				}
				this._group.GroupBySelector = value;
				base.RefreshOrDefer();
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06003745 RID: 14149 RVA: 0x001E4847 File Offset: 0x001E3847
		public override int Count
		{
			get
			{
				base.VerifyRefreshNotDeferred();
				return this.InternalCount;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06003746 RID: 14150 RVA: 0x001E4855 File Offset: 0x001E3855
		public override bool IsEmpty
		{
			get
			{
				return this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.None && this.CollectionProxy.Count == 0;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06003747 RID: 14151 RVA: 0x001E486F File Offset: 0x001E386F
		// (set) Token: 0x06003748 RID: 14152 RVA: 0x001E487C File Offset: 0x001E387C
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

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06003749 RID: 14153 RVA: 0x001E488A File Offset: 0x001E388A
		// (set) Token: 0x0600374A RID: 14154 RVA: 0x001E4894 File Offset: 0x001E3894
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
					if (!this._isGrouping)
					{
						base.OnCollectionChanged(null, notifyCollectionChangedEventArgs);
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

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x0600374B RID: 14155 RVA: 0x001E4AB9 File Offset: 0x001E3AB9
		public bool CanAddNew
		{
			get
			{
				return !this.IsEditingItem && this.InternalList.AllowNew;
			}
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x001E4AD0 File Offset: 0x001E3AD0
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
			object newItem = null;
			BindingOperations.AccessCollection(this.InternalList, delegate
			{
				this.ProcessPendingChanges();
				this._newItemIndex = -2;
				newItem = this.InternalList.AddNew();
			}, true);
			this.MoveCurrentTo(newItem);
			ISupportInitialize supportInitialize = newItem as ISupportInitialize;
			if (supportInitialize != null)
			{
				supportInitialize.BeginInit();
			}
			if (!this.IsDataView)
			{
				IEditableObject editableObject = newItem as IEditableObject;
				if (editableObject != null)
				{
					editableObject.BeginEdit();
				}
			}
			return newItem;
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x001E4B90 File Offset: 0x001E3B90
		private void BeginAddNew(object newItem, int index)
		{
			this.SetNewItem(newItem);
			this._newItemIndex = index;
			int index2 = index;
			if (!this._isGrouping)
			{
				switch (this.NewItemPlaceholderPosition)
				{
				case NewItemPlaceholderPosition.AtBeginning:
					this._newItemIndex--;
					index2 = 1;
					break;
				case NewItemPlaceholderPosition.AtEnd:
					index2 = this.InternalCount - 2;
					break;
				}
			}
			this.ProcessCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, index2));
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x001E4BFC File Offset: 0x001E3BFC
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
			ICancelAddNew ican = this.InternalList as ICancelAddNew;
			IEditableObject ieo;
			BindingOperations.AccessCollection(this.InternalList, delegate
			{
				this.ProcessPendingChanges();
				if (ican != null)
				{
					ican.EndNew(this._newItemIndex);
					return;
				}
				if ((ieo = (this._newItem as IEditableObject)) != null)
				{
					ieo.EndEdit();
				}
			}, true);
			if (this._newItem != CollectionView.NoNewItem)
			{
				int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
				NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = this.ProcessCommitNew(this._newItemIndex, this._newItemIndex + num);
				if (notifyCollectionChangedEventArgs != null)
				{
					base.OnCollectionChanged(this.InternalList, notifyCollectionChangedEventArgs);
				}
			}
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x001E4CC4 File Offset: 0x001E3CC4
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
			ICancelAddNew ican = this.InternalList as ICancelAddNew;
			IEditableObject ieo;
			BindingOperations.AccessCollection(this.InternalList, delegate
			{
				this.ProcessPendingChanges();
				if (ican != null)
				{
					ican.CancelNew(this._newItemIndex);
					return;
				}
				if ((ieo = (this._newItem as IEditableObject)) != null)
				{
					ieo.CancelEdit();
				}
			}, true);
			object newItem = this._newItem;
			object noNewItem = CollectionView.NoNewItem;
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x001E4D58 File Offset: 0x001E3D58
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

		// Token: 0x06003751 RID: 14161 RVA: 0x001E4DA4 File Offset: 0x001E3DA4
		private NotifyCollectionChangedEventArgs ProcessCommitNew(int fromIndex, int toIndex)
		{
			if (this._isGrouping)
			{
				this.CommitNewForGrouping();
				return null;
			}
			switch (this.NewItemPlaceholderPosition)
			{
			case NewItemPlaceholderPosition.AtBeginning:
				fromIndex = 1;
				break;
			case NewItemPlaceholderPosition.AtEnd:
				fromIndex = this.InternalCount - 2;
				break;
			}
			object changedItem = this.EndAddNew(false);
			NotifyCollectionChangedEventArgs result = null;
			if (fromIndex != toIndex)
			{
				result = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, changedItem, toIndex, fromIndex);
			}
			return result;
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x001E4E04 File Offset: 0x001E3E04
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
			object item = this.EndAddNew(false);
			this._group.RemoveSpecialItem(index, item, false);
			this.AddItemToGroups(item);
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06003753 RID: 14163 RVA: 0x001E4E73 File Offset: 0x001E3E73
		public bool IsAddingNew
		{
			get
			{
				return this._newItem != CollectionView.NoNewItem;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06003754 RID: 14164 RVA: 0x001E4E85 File Offset: 0x001E3E85
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

		// Token: 0x06003755 RID: 14165 RVA: 0x001E4E97 File Offset: 0x001E3E97
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

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06003756 RID: 14166 RVA: 0x001E4ECF File Offset: 0x001E3ECF
		public bool CanRemove
		{
			get
			{
				return !this.IsEditingItem && !this.IsAddingNew && this.InternalList.AllowRemove;
			}
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x001E4EF0 File Offset: 0x001E3EF0
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

		// Token: 0x06003758 RID: 14168 RVA: 0x001E4F40 File Offset: 0x001E3F40
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

		// Token: 0x06003759 RID: 14169 RVA: 0x001E4F98 File Offset: 0x001E3F98
		private void RemoveImpl(object item, int index)
		{
			if (item == CollectionView.NewItemPlaceholder)
			{
				throw new InvalidOperationException(SR.Get("RemovingPlaceholder"));
			}
			BindingOperations.AccessCollection(this.InternalList, delegate
			{
				this.ProcessPendingChanges();
				if (index >= this.InternalList.Count || !ItemsControl.EqualsEx(item, this.GetItemAt(index)))
				{
					index = this.InternalList.IndexOf(item);
					if (index < 0)
					{
						return;
					}
				}
				if (this._isGrouping)
				{
					index = this.InternalList.IndexOf(item);
				}
				else
				{
					int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
					index -= num;
				}
				try
				{
					this._isRemoving = true;
					this.InternalList.RemoveAt(index);
				}
				finally
				{
					this._isRemoving = false;
					this.DoDeferredActions();
				}
			}, true);
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x001E4FF8 File Offset: 0x001E3FF8
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

		// Token: 0x0600375B RID: 14171 RVA: 0x001E5064 File Offset: 0x001E4064
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
			IEditableObject ieo = this._editItem as IEditableObject;
			object editItem = this._editItem;
			this.SetEditItem(null);
			if (ieo != null)
			{
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					this.ProcessPendingChanges();
					ieo.EndEdit();
				}, true);
			}
			if (this._isGrouping)
			{
				this.RemoveItemFromGroups(editItem);
				this.AddItemToGroups(editItem);
				return;
			}
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x001E5114 File Offset: 0x001E4114
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

		// Token: 0x0600375D RID: 14173 RVA: 0x001E518C File Offset: 0x001E418C
		private void ImplicitlyCancelEdit()
		{
			IEditableObject editableObject = this._editItem as IEditableObject;
			this.SetEditItem(null);
			if (editableObject != null)
			{
				editableObject.CancelEdit();
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x0600375E RID: 14174 RVA: 0x001E51B5 File Offset: 0x001E41B5
		public bool CanCancelEdit
		{
			get
			{
				return this._editItem is IEditableObject;
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x001E51C5 File Offset: 0x001E41C5
		public bool IsEditingItem
		{
			get
			{
				return this._editItem != null;
			}
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06003760 RID: 14176 RVA: 0x001E51D0 File Offset: 0x001E41D0
		public object CurrentEditItem
		{
			get
			{
				return this._editItem;
			}
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x001E51D8 File Offset: 0x001E41D8
		private void SetEditItem(object item)
		{
			if (!ItemsControl.EqualsEx(item, this._editItem))
			{
				this._editItem = item;
				this.OnPropertyChanged("CurrentEditItem");
				this.OnPropertyChanged("IsEditingItem");
				this.OnPropertyChanged("CanCancelEdit");
				this.OnPropertyChanged("CanAddNew");
				this.OnPropertyChanged("CanRemove");
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool CanChangeLiveSorting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06003763 RID: 14179 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool CanChangeLiveFiltering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06003764 RID: 14180 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public bool CanChangeLiveGrouping
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06003765 RID: 14181 RVA: 0x001E5234 File Offset: 0x001E4234
		// (set) Token: 0x06003766 RID: 14182 RVA: 0x001E5259 File Offset: 0x001E4259
		public bool? IsLiveSorting
		{
			get
			{
				if (!this.IsDataView)
				{
					return null;
				}
				return new bool?(true);
			}
			set
			{
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveSorting",
					"CanChangeLiveSorting"
				}));
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x001E5234 File Offset: 0x001E4234
		// (set) Token: 0x06003768 RID: 14184 RVA: 0x001E5280 File Offset: 0x001E4280
		public bool? IsLiveFiltering
		{
			get
			{
				if (!this.IsDataView)
				{
					return null;
				}
				return new bool?(true);
			}
			set
			{
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveFiltering",
					"CanChangeLiveFiltering"
				}));
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x001E52A7 File Offset: 0x001E42A7
		// (set) Token: 0x0600376A RID: 14186 RVA: 0x001E52B0 File Offset: 0x001E42B0
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
					base.RefreshOrDefer();
					this.OnPropertyChanged("IsLiveGrouping");
				}
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x001E5315 File Offset: 0x001E4315
		public ObservableCollection<string> LiveSortingProperties
		{
			get
			{
				if (this._liveSortingProperties == null)
				{
					this._liveSortingProperties = new ObservableCollection<string>();
				}
				return this._liveSortingProperties;
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600376C RID: 14188 RVA: 0x001E5330 File Offset: 0x001E4330
		public ObservableCollection<string> LiveFilteringProperties
		{
			get
			{
				if (this._liveFilteringProperties == null)
				{
					this._liveFilteringProperties = new ObservableCollection<string>();
				}
				return this._liveFilteringProperties;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600376D RID: 14189 RVA: 0x001E534B File Offset: 0x001E434B
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

		// Token: 0x0600376E RID: 14190 RVA: 0x001E5380 File Offset: 0x001E4380
		private void OnLivePropertyListChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			bool? isLiveGrouping = this.IsLiveGrouping;
			bool flag = true;
			if (isLiveGrouping.GetValueOrDefault() == flag & isLiveGrouping != null)
			{
				base.RefreshOrDefer();
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x0600376F RID: 14191 RVA: 0x001E53B0 File Offset: 0x001E43B0
		public ReadOnlyCollection<ItemPropertyInfo> ItemProperties
		{
			get
			{
				return base.GetItemProperties();
			}
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x001E53B8 File Offset: 0x001E43B8
		protected override void RefreshOverride()
		{
			object currentItem = this.CurrentItem;
			int num = this.IsEmpty ? 0 : this.CurrentPosition;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			base.OnCurrentChanging();
			this._ignoreInnerRefresh = true;
			if (this.IsCustomFilterSet || this._isFiltered)
			{
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					if (this.IsCustomFilterSet)
					{
						this._isFiltered = true;
						this._blv.Filter = this._customFilter;
						return;
					}
					if (this._isFiltered)
					{
						this._isFiltered = false;
						this._blv.RemoveFilter();
					}
				}, true);
			}
			if (this._sort != null && this._sort.Count > 0 && this.CollectionProxy != null && this.CollectionProxy.Count > 0)
			{
				ListSortDescriptionCollection sorts = this.ConvertSortDescriptionCollection(this._sort);
				if (sorts.Count > 0)
				{
					this._isSorted = true;
					BindingOperations.AccessCollection(this.InternalList, delegate
					{
						if (this._blv == null)
						{
							this.InternalList.ApplySort(sorts[0].PropertyDescriptor, sorts[0].SortDirection);
							return;
						}
						this._blv.ApplySort(sorts);
					}, true);
				}
				this.ActiveComparer = new SortFieldComparer(this._sort, this.Culture);
			}
			else if (this._isSorted)
			{
				this._isSorted = false;
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					this.InternalList.RemoveSort();
				}, true);
				this.ActiveComparer = null;
			}
			this.InitializeGrouping();
			this.PrepareCachedList();
			this.PrepareGroups();
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
			this._ignoreInnerRefresh = false;
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

		// Token: 0x06003771 RID: 14193 RVA: 0x001E55DB File Offset: 0x001E45DB
		protected override void OnAllowsCrossThreadChangesChanged()
		{
			this.PrepareCachedList();
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x001E55E3 File Offset: 0x001E45E3
		private void PrepareCachedList()
		{
			if (base.AllowsCrossThreadChanges)
			{
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					this.RebuildLists();
				}, false);
				return;
			}
			this.RebuildListsCore();
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x001E560C File Offset: 0x001E460C
		private void RebuildLists()
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				base.ClearPendingChanges();
				this.RebuildListsCore();
			}
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x001E5654 File Offset: 0x001E4654
		private void RebuildListsCore()
		{
			this._cachedList = new ArrayList(this.InternalList);
			LiveShapingList liveShapingList = this._shadowList as LiveShapingList;
			if (liveShapingList != null)
			{
				liveShapingList.LiveShapingDirty -= this.OnLiveShapingDirty;
			}
			if (this._isGrouping)
			{
				bool? isLiveGrouping = this.IsLiveGrouping;
				bool flag = true;
				if (isLiveGrouping.GetValueOrDefault() == flag & isLiveGrouping != null)
				{
					liveShapingList = (this._shadowList = new LiveShapingList(this, this.GetLiveShapingFlags(), this.ActiveComparer));
					foreach (object value in this.InternalList)
					{
						liveShapingList.Add(value);
					}
					liveShapingList.LiveShapingDirty += this.OnLiveShapingDirty;
					return;
				}
			}
			if (base.AllowsCrossThreadChanges)
			{
				this._shadowList = new ArrayList(this.InternalList);
				return;
			}
			this._shadowList = null;
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Obsolete("Replaced by OnAllowsCrossThreadChangesChanged")]
		protected override void OnBeginChangeLogging(NotifyCollectionChangedEventArgs args)
		{
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x001E5758 File Offset: 0x001E4758
		protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			bool flag = false;
			this.ValidateCollectionChangedEventArgs(args);
			int currentPosition = this.CurrentPosition;
			int currentPosition2 = this.CurrentPosition;
			object currentItem = this.CurrentItem;
			bool isCurrentAfterLast = this.IsCurrentAfterLast;
			bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			bool flag2 = false;
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (this._newItemIndex == -2)
				{
					this.BeginAddNew(args.NewItems[0], args.NewStartingIndex);
					return;
				}
				if (this._isGrouping)
				{
					this.AddItemToGroups(args.NewItems[0]);
				}
				else
				{
					this.AdjustCurrencyForAdd(args.NewStartingIndex);
					flag = true;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (this._isGrouping)
				{
					this.RemoveItemFromGroups(args.OldItems[0]);
				}
				else
				{
					flag2 = this.AdjustCurrencyForRemove(args.OldStartingIndex);
					flag = true;
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (this._isGrouping)
				{
					this.RemoveItemFromGroups(args.OldItems[0]);
					this.AddItemToGroups(args.NewItems[0]);
				}
				else
				{
					flag2 = this.AdjustCurrencyForReplace(args.NewStartingIndex);
					flag = true;
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (!this._isGrouping)
				{
					this.AdjustCurrencyForMove(args.OldStartingIndex, args.NewStartingIndex);
					flag = true;
				}
				else
				{
					this._group.MoveWithinSubgroups(args.OldItems[0], null, this.InternalList, args.OldStartingIndex, args.NewStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				if (this._isGrouping)
				{
					base.RefreshOrDefer();
				}
				else
				{
					flag = true;
				}
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			if (base.AllowsCrossThreadChanges)
			{
				this.AdjustShadowCopy(args);
			}
			bool flag3 = this.IsCurrentAfterLast != isCurrentAfterLast;
			bool flag4 = this.IsCurrentBeforeFirst != isCurrentBeforeFirst;
			bool flag5 = this.CurrentPosition != currentPosition2;
			bool flag6 = this.CurrentItem != currentItem;
			isCurrentAfterLast = this.IsCurrentAfterLast;
			isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
			currentPosition2 = this.CurrentPosition;
			currentItem = this.CurrentItem;
			if (flag)
			{
				this.OnCollectionChanged(args);
				if (this.IsCurrentAfterLast != isCurrentAfterLast)
				{
					flag3 = false;
					isCurrentAfterLast = this.IsCurrentAfterLast;
				}
				if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
				{
					flag4 = false;
					isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
				}
				if (this.CurrentPosition != currentPosition2)
				{
					flag5 = false;
					currentPosition2 = this.CurrentPosition;
				}
				if (this.CurrentItem != currentItem)
				{
					flag6 = false;
					currentItem = this.CurrentItem;
				}
			}
			if (flag2)
			{
				this.MoveCurrencyOffDeletedElement(currentPosition);
				flag3 = (flag3 || this.IsCurrentAfterLast != isCurrentAfterLast);
				flag4 = (flag4 || this.IsCurrentBeforeFirst != isCurrentBeforeFirst);
				flag5 = (flag5 || this.CurrentPosition != currentPosition2);
				flag6 = (flag6 || this.CurrentItem != currentItem);
			}
			if (flag3)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (flag4)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (flag5)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (flag6)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06003777 RID: 14199 RVA: 0x001E5A6D File Offset: 0x001E4A6D
		private int InternalCount
		{
			get
			{
				if (this._isGrouping)
				{
					return this._group.ItemCount;
				}
				return ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.None) ? 0 : 1) + this.CollectionProxy.Count;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06003778 RID: 14200 RVA: 0x001E5A9B File Offset: 0x001E4A9B
		private bool IsDataView
		{
			get
			{
				return this._isDataView;
			}
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x001E5AA4 File Offset: 0x001E4AA4
		private int InternalIndexOf(object item)
		{
			if (this._isGrouping)
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
				case NewItemPlaceholderPosition.AtBeginning:
					return 1;
				case NewItemPlaceholderPosition.AtEnd:
					return this.InternalCount - 2;
				}
			}
			int num = this.CollectionProxy.IndexOf(item);
			if (num >= this.CollectionProxy.Count)
			{
				num = -1;
			}
			if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning && num >= 0)
			{
				num += (this.IsAddingNew ? 2 : 1);
			}
			return num;
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x001E5B6C File Offset: 0x001E4B6C
		private object InternalItemAt(int index)
		{
			if (this._isGrouping)
			{
				return this._group.LeafAt(index);
			}
			switch (this.NewItemPlaceholderPosition)
			{
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
					if (index <= this._newItemIndex + 1)
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
				if (this.IsAddingNew && index == this.InternalCount - 2)
				{
					return this._newItem;
				}
				break;
			}
			return this.CollectionProxy[index];
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x001E5C12 File Offset: 0x001E4C12
		private bool InternalContains(object item)
		{
			if (item == CollectionView.NewItemPlaceholder)
			{
				return this.NewItemPlaceholderPosition > NewItemPlaceholderPosition.None;
			}
			if (this._isGrouping)
			{
				return this._group.LeafIndexOf(item) >= 0;
			}
			return this.CollectionProxy.Contains(item);
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x001E5C4D File Offset: 0x001E4C4D
		private IEnumerator InternalGetEnumerator()
		{
			if (!this._isGrouping)
			{
				return new CollectionView.PlaceholderAwareEnumerator(this, this.CollectionProxy.GetEnumerator(), this.NewItemPlaceholderPosition, this._newItem);
			}
			return this._group.GetLeafEnumerator();
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x001E5C80 File Offset: 0x001E4C80
		private void AdjustShadowCopy(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this._shadowList.Insert(e.NewStartingIndex, e.NewItems[0]);
				return;
			case NotifyCollectionChangedAction.Remove:
				this._shadowList.RemoveAt(e.OldStartingIndex);
				return;
			case NotifyCollectionChangedAction.Replace:
				this._shadowList[e.OldStartingIndex] = e.NewItems[0];
				return;
			case NotifyCollectionChangedAction.Move:
				this._shadowList.Move(e.OldStartingIndex, e.NewStartingIndex);
				return;
			default:
				return;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x0600377E RID: 14206 RVA: 0x001E5D10 File Offset: 0x001E4D10
		private bool IsCurrentInView
		{
			get
			{
				return 0 <= this.CurrentPosition && this.CurrentPosition < this.InternalCount;
			}
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x001E5D2C File Offset: 0x001E4D2C
		private void _MoveTo(int proposed)
		{
			if (proposed == this.CurrentPosition || this.IsEmpty)
			{
				return;
			}
			object obj = (0 <= proposed && proposed < this.InternalCount) ? this.GetItemAt(proposed) : null;
			if (obj == CollectionView.NewItemPlaceholder)
			{
				return;
			}
			if (base.OKToChangeCurrent())
			{
				bool isCurrentAfterLast = this.IsCurrentAfterLast;
				bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
				base.SetCurrent(obj, proposed);
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

		// Token: 0x06003780 RID: 14208 RVA: 0x001E5DCE File Offset: 0x001E4DCE
		private void SubscribeToChanges()
		{
			if (this.InternalList.SupportsChangeNotification)
			{
				BindingOperations.AccessCollection(this.InternalList, delegate
				{
					this.InternalList.ListChanged += this.OnListChanged;
					this.RebuildLists();
				}, false);
			}
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x001E5DF8 File Offset: 0x001E4DF8
		private void OnListChanged(object sender, ListChangedEventArgs args)
		{
			if (this._ignoreInnerRefresh && args.ListChangedType == ListChangedType.Reset)
			{
				return;
			}
			NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = null;
			int num = this._isGrouping ? 0 : ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0);
			int num2 = args.NewIndex;
			switch (args.ListChangedType)
			{
			case ListChangedType.Reset:
			case ListChangedType.PropertyDescriptorAdded:
			case ListChangedType.PropertyDescriptorDeleted:
			case ListChangedType.PropertyDescriptorChanged:
				break;
			case ListChangedType.ItemAdded:
				if (this.InternalList.Count == this._cachedList.Count)
				{
					if (this.IsAddingNew && num2 == this._newItemIndex)
					{
						notifyCollectionChangedEventArgs = this.ProcessCommitNew(num2 + num, num2 + num);
						goto IL_38D;
					}
					goto IL_38D;
				}
				else
				{
					object obj = this.InternalList[num2];
					notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, obj, num2 + num);
					this._cachedList.Insert(num2, obj);
					if (this.InternalList.Count != this._cachedList.Count)
					{
						throw new InvalidOperationException(SR.Get("InconsistentBindingList", new object[]
						{
							this.InternalList,
							args.ListChangedType
						}));
					}
					if (num2 <= this._newItemIndex)
					{
						this._newItemIndex++;
						goto IL_38D;
					}
					goto IL_38D;
				}
				break;
			case ListChangedType.ItemDeleted:
			{
				object obj = this._cachedList[num2];
				this._cachedList.RemoveAt(num2);
				if (this.InternalList.Count != this._cachedList.Count)
				{
					throw new InvalidOperationException(SR.Get("InconsistentBindingList", new object[]
					{
						this.InternalList,
						args.ListChangedType
					}));
				}
				if (num2 < this._newItemIndex)
				{
					this._newItemIndex--;
				}
				if (obj == this.CurrentEditItem)
				{
					this.ImplicitlyCancelEdit();
				}
				if (obj == this.CurrentAddItem)
				{
					this.EndAddNew(true);
					NewItemPlaceholderPosition newItemPlaceholderPosition = this.NewItemPlaceholderPosition;
					if (newItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)
					{
						if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
						{
							num2 = this.InternalCount - 1;
						}
					}
					else
					{
						num2 = 0;
					}
				}
				notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, obj, num2 + num);
				goto IL_38D;
			}
			case ListChangedType.ItemMoved:
			{
				object obj;
				if (this.IsAddingNew && args.OldIndex == this._newItemIndex)
				{
					obj = this._newItem;
					notifyCollectionChangedEventArgs = this.ProcessCommitNew(args.OldIndex, num2 + num);
				}
				else
				{
					obj = this.InternalList[num2];
					notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, obj, num2 + num, args.OldIndex + num);
					if (args.OldIndex < this._newItemIndex && this._newItemIndex < args.NewIndex)
					{
						this._newItemIndex--;
					}
					else if (args.NewIndex <= this._newItemIndex && this._newItemIndex < args.OldIndex)
					{
						this._newItemIndex++;
					}
				}
				this._cachedList.RemoveAt(args.OldIndex);
				this._cachedList.Insert(args.NewIndex, obj);
				if (this.InternalList.Count != this._cachedList.Count)
				{
					throw new InvalidOperationException(SR.Get("InconsistentBindingList", new object[]
					{
						this.InternalList,
						args.ListChangedType
					}));
				}
				goto IL_38D;
			}
			case ListChangedType.ItemChanged:
				if (this._itemsRaisePropertyChanged == null)
				{
					object obj = this.InternalList[args.NewIndex];
					this._itemsRaisePropertyChanged = new bool?(obj is INotifyPropertyChanged);
				}
				if (this._itemsRaisePropertyChanged.Value)
				{
					goto IL_38D;
				}
				break;
			default:
				goto IL_38D;
			}
			if (this.IsEditingItem)
			{
				this.ImplicitlyCancelEdit();
			}
			if (this.IsAddingNew)
			{
				this._newItemIndex = this.InternalList.IndexOf(this._newItem);
				if (this._newItemIndex < 0)
				{
					this.EndAddNew(true);
				}
			}
			base.RefreshOrDefer();
			IL_38D:
			if (notifyCollectionChangedEventArgs != null)
			{
				base.OnCollectionChanged(sender, notifyCollectionChangedEventArgs);
			}
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x001E61A0 File Offset: 0x001E51A0
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

		// Token: 0x06003783 RID: 14211 RVA: 0x001E61F6 File Offset: 0x001E51F6
		private bool AdjustCurrencyForRemove(int index)
		{
			bool result = index == this.CurrentPosition;
			if (index < this.CurrentPosition)
			{
				base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
			}
			return result;
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x001E6220 File Offset: 0x001E5220
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

		// Token: 0x06003785 RID: 14213 RVA: 0x001E6292 File Offset: 0x001E5292
		private bool AdjustCurrencyForReplace(int index)
		{
			bool flag = index == this.CurrentPosition;
			if (flag)
			{
				base.SetCurrent(this.GetItemAt(index), index);
			}
			return flag;
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x001E62B0 File Offset: 0x001E52B0
		private void MoveCurrencyOffDeletedElement(int oldCurrentPosition)
		{
			int num = this.InternalCount - 1;
			int num2 = (oldCurrentPosition < num) ? oldCurrentPosition : num;
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

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06003787 RID: 14215 RVA: 0x001E62F7 File Offset: 0x001E52F7
		private IList CollectionProxy
		{
			get
			{
				if (this._shadowList != null)
				{
					return this._shadowList;
				}
				return this.InternalList;
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06003788 RID: 14216 RVA: 0x001E630E File Offset: 0x001E530E
		// (set) Token: 0x06003789 RID: 14217 RVA: 0x001E6316 File Offset: 0x001E5316
		private IBindingList InternalList
		{
			get
			{
				return this._internalList;
			}
			set
			{
				this._internalList = value;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x0600378A RID: 14218 RVA: 0x001E631F File Offset: 0x001E531F
		private bool IsCustomFilterSet
		{
			get
			{
				return this._blv != null && !string.IsNullOrEmpty(this._customFilter);
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x0600378B RID: 14219 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		private bool CanGroupNamesChange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x001E6339 File Offset: 0x001E5339
		private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.IsAddingNew || this.IsEditingItem)
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Sorting"
				}));
			}
			base.RefreshOrDefer();
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x001E6370 File Offset: 0x001E5370
		private ListSortDescriptionCollection ConvertSortDescriptionCollection(SortDescriptionCollection sorts)
		{
			ITypedList typedList;
			PropertyDescriptorCollection propertyDescriptorCollection;
			Type itemType;
			if ((typedList = (this.InternalList as ITypedList)) != null)
			{
				propertyDescriptorCollection = typedList.GetItemProperties(null);
			}
			else if ((itemType = base.GetItemType(true)) != null)
			{
				propertyDescriptorCollection = TypeDescriptor.GetProperties(itemType);
			}
			else
			{
				propertyDescriptorCollection = null;
			}
			if (propertyDescriptorCollection == null || propertyDescriptorCollection.Count == 0)
			{
				throw new ArgumentException(SR.Get("CannotDetermineSortByPropertiesForCollection"));
			}
			ListSortDescription[] array = new ListSortDescription[sorts.Count];
			for (int i = 0; i < sorts.Count; i++)
			{
				PropertyDescriptor propertyDescriptor = propertyDescriptorCollection.Find(sorts[i].PropertyName, true);
				if (propertyDescriptor == null)
				{
					string listName = typedList.GetListName(null);
					throw new ArgumentException(SR.Get("PropertyToSortByNotFoundOnType", new object[]
					{
						listName,
						sorts[i].PropertyName
					}));
				}
				ListSortDescription listSortDescription = new ListSortDescription(propertyDescriptor, sorts[i].Direction);
				array[i] = listSortDescription;
			}
			return new ListSortDescriptionCollection(array);
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x001E646D File Offset: 0x001E546D
		private void InitializeGrouping()
		{
			this._group.Clear();
			this._group.Initialize();
			this._isGrouping = (this._group.GroupBy != null);
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x001E649C File Offset: 0x001E549C
		private void PrepareGroups()
		{
			if (!this._isGrouping)
			{
				return;
			}
			IList collectionProxy = this.CollectionProxy;
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
					listComparer.ResetList(collectionProxy);
				}
				else
				{
					this._group.ActiveComparer = new CollectionViewGroupInternal.IListComparer(collectionProxy);
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
			bool flag = true;
			bool flag2 = isLiveGrouping.GetValueOrDefault() == flag & isLiveGrouping != null;
			LiveShapingList liveShapingList = collectionProxy as LiveShapingList;
			int i = 0;
			int count = collectionProxy.Count;
			while (i < count)
			{
				object obj = collectionProxy[i];
				LiveShapingItem lsi = flag2 ? liveShapingList.ItemAt(i) : null;
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

		// Token: 0x06003790 RID: 14224 RVA: 0x001E6615 File Offset: 0x001E5615
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

		// Token: 0x06003791 RID: 14225 RVA: 0x001E664A File Offset: 0x001E564A
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

		// Token: 0x06003792 RID: 14226 RVA: 0x001E664A File Offset: 0x001E564A
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

		// Token: 0x06003793 RID: 14227 RVA: 0x001E6680 File Offset: 0x001E5680
		private void AddItemToGroups(object item)
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
			this._group.AddToSubgroups(item, null, false);
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x001E66FE File Offset: 0x001E56FE
		private void RemoveItemFromGroups(object item)
		{
			if (this.CanGroupNamesChange || this._group.RemoveFromSubgroups(item))
			{
				this._group.RemoveItemFromSubgroupsByExhaustiveSearch(item);
			}
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x001E6724 File Offset: 0x001E5724
		private LiveShapingFlags GetLiveShapingFlags()
		{
			LiveShapingFlags liveShapingFlags = (LiveShapingFlags)0;
			bool? isLiveGrouping = this.IsLiveGrouping;
			bool flag = true;
			if (isLiveGrouping.GetValueOrDefault() == flag & isLiveGrouping != null)
			{
				liveShapingFlags |= LiveShapingFlags.Grouping;
			}
			return liveShapingFlags;
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x001E6758 File Offset: 0x001E5758
		internal void RestoreLiveShaping()
		{
			LiveShapingList liveShapingList = this.CollectionProxy as LiveShapingList;
			if (liveShapingList == null)
			{
				return;
			}
			if (this._isGrouping)
			{
				List<AbandonedGroupItem> deleteList = new List<AbandonedGroupItem>();
				foreach (LiveShapingItem liveShapingItem in liveShapingList.GroupDirtyItems)
				{
					if (!liveShapingItem.IsDeleted)
					{
						this._group.RestoreGrouping(liveShapingItem, deleteList);
						liveShapingItem.IsGroupDirty = false;
					}
				}
				this._group.DeleteAbandonedGroupItems(deleteList);
			}
			liveShapingList.GroupDirtyItems.Clear();
			this.IsLiveShapingDirty = false;
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x001E67FC File Offset: 0x001E57FC
		// (set) Token: 0x06003798 RID: 14232 RVA: 0x001E6804 File Offset: 0x001E5804
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

		// Token: 0x06003799 RID: 14233 RVA: 0x001E6833 File Offset: 0x001E5833
		private void OnLiveShapingDirty(object sender, EventArgs e)
		{
			this.IsLiveShapingDirty = true;
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x00150920 File Offset: 0x0014F920
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

		// Token: 0x0600379B RID: 14235 RVA: 0x00150A1C File Offset: 0x0014FA1C
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x001E683C File Offset: 0x001E583C
		private void DeferAction(Action action)
		{
			if (this._deferredActions == null)
			{
				this._deferredActions = new List<Action>();
			}
			this._deferredActions.Add(action);
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x001E6860 File Offset: 0x001E5860
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

		// Token: 0x04001CDD RID: 7389
		private IBindingList _internalList;

		// Token: 0x04001CDE RID: 7390
		private CollectionViewGroupRoot _group;

		// Token: 0x04001CDF RID: 7391
		private bool _isGrouping;

		// Token: 0x04001CE0 RID: 7392
		private IBindingListView _blv;

		// Token: 0x04001CE1 RID: 7393
		private BindingListCollectionView.BindingListSortDescriptionCollection _sort;

		// Token: 0x04001CE2 RID: 7394
		private IList _shadowList;

		// Token: 0x04001CE3 RID: 7395
		private bool _isSorted;

		// Token: 0x04001CE4 RID: 7396
		private IComparer _comparer;

		// Token: 0x04001CE5 RID: 7397
		private string _customFilter;

		// Token: 0x04001CE6 RID: 7398
		private bool _isFiltered;

		// Token: 0x04001CE7 RID: 7399
		private bool _ignoreInnerRefresh;

		// Token: 0x04001CE8 RID: 7400
		private bool? _itemsRaisePropertyChanged;

		// Token: 0x04001CE9 RID: 7401
		private bool _isDataView;

		// Token: 0x04001CEA RID: 7402
		private object _newItem = CollectionView.NoNewItem;

		// Token: 0x04001CEB RID: 7403
		private object _editItem;

		// Token: 0x04001CEC RID: 7404
		private int _newItemIndex;

		// Token: 0x04001CED RID: 7405
		private NewItemPlaceholderPosition _newItemPlaceholderPosition;

		// Token: 0x04001CEE RID: 7406
		private List<Action> _deferredActions;

		// Token: 0x04001CEF RID: 7407
		private bool _isRemoving;

		// Token: 0x04001CF0 RID: 7408
		private bool? _isLiveGrouping = new bool?(false);

		// Token: 0x04001CF1 RID: 7409
		private bool _isLiveShapingDirty;

		// Token: 0x04001CF2 RID: 7410
		private ObservableCollection<string> _liveSortingProperties;

		// Token: 0x04001CF3 RID: 7411
		private ObservableCollection<string> _liveFilteringProperties;

		// Token: 0x04001CF4 RID: 7412
		private ObservableCollection<string> _liveGroupingProperties;

		// Token: 0x04001CF5 RID: 7413
		private IList _cachedList;

		// Token: 0x02000AD6 RID: 2774
		private class BindingListSortDescriptionCollection : SortDescriptionCollection
		{
			// Token: 0x06008B27 RID: 35623 RVA: 0x00339415 File Offset: 0x00338415
			internal BindingListSortDescriptionCollection(bool allowMultipleDescriptions)
			{
				this._allowMultipleDescriptions = allowMultipleDescriptions;
			}

			// Token: 0x06008B28 RID: 35624 RVA: 0x00339424 File Offset: 0x00338424
			protected override void InsertItem(int index, SortDescription item)
			{
				if (!this._allowMultipleDescriptions && base.Count > 0)
				{
					throw new InvalidOperationException(SR.Get("BindingListCanOnlySortByOneProperty"));
				}
				base.InsertItem(index, item);
			}

			// Token: 0x040046F2 RID: 18162
			private bool _allowMultipleDescriptions;
		}
	}
}
