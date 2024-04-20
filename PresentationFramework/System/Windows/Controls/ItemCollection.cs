using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using MS.Internal.Controls;
using MS.Internal.Data;
using MS.Internal.KnownBoxes;
using MS.Internal.Utility;

namespace System.Windows.Controls
{
	// Token: 0x0200079B RID: 1947
	[Localizability(LocalizationCategory.Ignore)]
	public sealed class ItemCollection : CollectionView, IList, ICollection, IEnumerable, IEditableCollectionViewAddNewItem, IEditableCollectionView, ICollectionViewLiveShaping, IItemProperties, IWeakEventListener
	{
		// Token: 0x06006CAE RID: 27822 RVA: 0x002C9D52 File Offset: 0x002C8D52
		internal ItemCollection(DependencyObject modelParent) : base(EmptyEnumerable.Instance, false)
		{
			this._modelParent = new WeakReference(modelParent);
		}

		// Token: 0x06006CAF RID: 27823 RVA: 0x002C9D74 File Offset: 0x002C8D74
		internal ItemCollection(FrameworkElement modelParent, int capacity) : base(EmptyEnumerable.Instance, false)
		{
			this._defaultCapacity = capacity;
			this._modelParent = new WeakReference(modelParent);
		}

		// Token: 0x06006CB0 RID: 27824 RVA: 0x002C9D9D File Offset: 0x002C8D9D
		public override bool MoveCurrentToFirst()
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToFirst();
		}

		// Token: 0x06006CB1 RID: 27825 RVA: 0x002C9DBA File Offset: 0x002C8DBA
		public override bool MoveCurrentToNext()
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToNext();
		}

		// Token: 0x06006CB2 RID: 27826 RVA: 0x002C9DD7 File Offset: 0x002C8DD7
		public override bool MoveCurrentToPrevious()
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToPrevious();
		}

		// Token: 0x06006CB3 RID: 27827 RVA: 0x002C9DF4 File Offset: 0x002C8DF4
		public override bool MoveCurrentToLast()
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToLast();
		}

		// Token: 0x06006CB4 RID: 27828 RVA: 0x002C9E11 File Offset: 0x002C8E11
		public override bool MoveCurrentTo(object item)
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentTo(item);
		}

		// Token: 0x06006CB5 RID: 27829 RVA: 0x002C9E2F File Offset: 0x002C8E2F
		public override bool MoveCurrentToPosition(int position)
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToPosition(position);
		}

		// Token: 0x06006CB6 RID: 27830 RVA: 0x002C9E4D File Offset: 0x002C8E4D
		protected override IEnumerator GetEnumerator()
		{
			if (!this.EnsureCollectionView())
			{
				return EmptyEnumerator.Instance;
			}
			return ((IEnumerable)this._collectionView).GetEnumerator();
		}

		// Token: 0x06006CB7 RID: 27831 RVA: 0x002C9E68 File Offset: 0x002C8E68
		public int Add(object newItem)
		{
			this.CheckIsUsingInnerView();
			int result = this._internalView.Add(newItem);
			this.ModelParent.SetValue(ItemsControl.HasItemsPropertyKey, BooleanBoxes.TrueBox);
			return result;
		}

		// Token: 0x06006CB8 RID: 27832 RVA: 0x002C9E94 File Offset: 0x002C8E94
		public void Clear()
		{
			this.VerifyRefreshNotDeferred();
			if (this.IsUsingItemsSource)
			{
				throw new InvalidOperationException(SR.Get("ItemsSourceInUse"));
			}
			if (this._internalView != null)
			{
				this._internalView.Clear();
			}
			this.ModelParent.ClearValue(ItemsControl.HasItemsPropertyKey);
		}

		// Token: 0x06006CB9 RID: 27833 RVA: 0x002C9EE2 File Offset: 0x002C8EE2
		public override bool Contains(object containItem)
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.Contains(containItem);
		}

		// Token: 0x06006CBA RID: 27834 RVA: 0x002C9F00 File Offset: 0x002C8F00
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank > 1)
			{
				throw new ArgumentException(SR.Get("BadTargetArray"), "array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (!this.EnsureCollectionView())
			{
				return;
			}
			this.VerifyRefreshNotDeferred();
			IndexedEnumerable.CopyTo(this._collectionView, array, index);
		}

		// Token: 0x06006CBB RID: 27835 RVA: 0x002C9F64 File Offset: 0x002C8F64
		public override int IndexOf(object item)
		{
			if (!this.EnsureCollectionView())
			{
				return -1;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.IndexOf(item);
		}

		// Token: 0x06006CBC RID: 27836 RVA: 0x002C9F84 File Offset: 0x002C8F84
		public override object GetItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.VerifyRefreshNotDeferred();
			if (!this.EnsureCollectionView())
			{
				throw new InvalidOperationException(SR.Get("ItemCollectionHasNoCollection"));
			}
			if (this._collectionView == this._internalView && index >= this._internalView.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._collectionView.GetItemAt(index);
		}

		// Token: 0x06006CBD RID: 27837 RVA: 0x002C9FF1 File Offset: 0x002C8FF1
		public void Insert(int insertIndex, object insertItem)
		{
			this.CheckIsUsingInnerView();
			this._internalView.Insert(insertIndex, insertItem);
			this.ModelParent.SetValue(ItemsControl.HasItemsPropertyKey, BooleanBoxes.TrueBox);
		}

		// Token: 0x06006CBE RID: 27838 RVA: 0x002CA01B File Offset: 0x002C901B
		public void Remove(object removeItem)
		{
			this.CheckIsUsingInnerView();
			this._internalView.Remove(removeItem);
			if (this.IsEmpty)
			{
				this.ModelParent.ClearValue(ItemsControl.HasItemsPropertyKey);
			}
		}

		// Token: 0x06006CBF RID: 27839 RVA: 0x002CA047 File Offset: 0x002C9047
		public void RemoveAt(int removeIndex)
		{
			this.CheckIsUsingInnerView();
			this._internalView.RemoveAt(removeIndex);
			if (this.IsEmpty)
			{
				this.ModelParent.ClearValue(ItemsControl.HasItemsPropertyKey);
			}
		}

		// Token: 0x06006CC0 RID: 27840 RVA: 0x002CA073 File Offset: 0x002C9073
		public override bool PassesFilter(object item)
		{
			return !this.EnsureCollectionView() || this._collectionView.PassesFilter(item);
		}

		// Token: 0x06006CC1 RID: 27841 RVA: 0x002CA08B File Offset: 0x002C908B
		protected override void RefreshOverride()
		{
			if (this._collectionView != null)
			{
				if (this._collectionView.NeedsRefresh)
				{
					this._collectionView.Refresh();
					return;
				}
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x17001912 RID: 6418
		// (get) Token: 0x06006CC2 RID: 27842 RVA: 0x002CA0BA File Offset: 0x002C90BA
		public override int Count
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return 0;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.Count;
			}
		}

		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x06006CC3 RID: 27843 RVA: 0x002CA0D7 File Offset: 0x002C90D7
		public override bool IsEmpty
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return true;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.IsEmpty;
			}
		}

		// Token: 0x17001914 RID: 6420
		public object this[int index]
		{
			get
			{
				return this.GetItemAt(index);
			}
			set
			{
				this.CheckIsUsingInnerView();
				if (index < 0 || index >= this._internalView.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				this._internalView[index] = value;
			}
		}

		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x06006CC6 RID: 27846 RVA: 0x002CA126 File Offset: 0x002C9126
		public override IEnumerable SourceCollection
		{
			get
			{
				if (this.IsUsingItemsSource)
				{
					return this.ItemsSource;
				}
				this.EnsureInternalView();
				return this;
			}
		}

		// Token: 0x17001916 RID: 6422
		// (get) Token: 0x06006CC7 RID: 27847 RVA: 0x002CA13E File Offset: 0x002C913E
		public override bool NeedsRefresh
		{
			get
			{
				return this.EnsureCollectionView() && this._collectionView.NeedsRefresh;
			}
		}

		// Token: 0x17001917 RID: 6423
		// (get) Token: 0x06006CC8 RID: 27848 RVA: 0x002CA158 File Offset: 0x002C9158
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				if (this.MySortDescriptions == null)
				{
					this.MySortDescriptions = new SortDescriptionCollection();
					if (this._collectionView != null)
					{
						this.CloneList(this.MySortDescriptions, this._collectionView.SortDescriptions);
					}
					((INotifyCollectionChanged)this.MySortDescriptions).CollectionChanged += this.SortDescriptionsChanged;
				}
				return this.MySortDescriptions;
			}
		}

		// Token: 0x17001918 RID: 6424
		// (get) Token: 0x06006CC9 RID: 27849 RVA: 0x002CA1B4 File Offset: 0x002C91B4
		public override bool CanSort
		{
			get
			{
				return !this.EnsureCollectionView() || this._collectionView.CanSort;
			}
		}

		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x06006CCA RID: 27850 RVA: 0x002CA1CB File Offset: 0x002C91CB
		// (set) Token: 0x06006CCB RID: 27851 RVA: 0x002CA1E7 File Offset: 0x002C91E7
		public override Predicate<object> Filter
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return this.MyFilter;
				}
				return this._collectionView.Filter;
			}
			set
			{
				this.MyFilter = value;
				if (this._collectionView != null)
				{
					this._collectionView.Filter = value;
				}
			}
		}

		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x06006CCC RID: 27852 RVA: 0x002CA204 File Offset: 0x002C9204
		public override bool CanFilter
		{
			get
			{
				return !this.EnsureCollectionView() || this._collectionView.CanFilter;
			}
		}

		// Token: 0x1700191B RID: 6427
		// (get) Token: 0x06006CCD RID: 27853 RVA: 0x002CA21B File Offset: 0x002C921B
		public override bool CanGroup
		{
			get
			{
				return this.EnsureCollectionView() && this._collectionView.CanGroup;
			}
		}

		// Token: 0x1700191C RID: 6428
		// (get) Token: 0x06006CCE RID: 27854 RVA: 0x002CA234 File Offset: 0x002C9234
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				if (this.MyGroupDescriptions == null)
				{
					this.MyGroupDescriptions = new ObservableCollection<GroupDescription>();
					if (this._collectionView != null)
					{
						this.CloneList(this.MyGroupDescriptions, this._collectionView.GroupDescriptions);
					}
					((INotifyCollectionChanged)this.MyGroupDescriptions).CollectionChanged += this.GroupDescriptionsChanged;
				}
				return this.MyGroupDescriptions;
			}
		}

		// Token: 0x1700191D RID: 6429
		// (get) Token: 0x06006CCF RID: 27855 RVA: 0x002CA290 File Offset: 0x002C9290
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return null;
				}
				return this._collectionView.Groups;
			}
		}

		// Token: 0x06006CD0 RID: 27856 RVA: 0x002CA2A7 File Offset: 0x002C92A7
		public override IDisposable DeferRefresh()
		{
			if (this._deferLevel == 0 && this._collectionView != null)
			{
				this._deferInnerRefresh = this._collectionView.DeferRefresh();
			}
			this._deferLevel++;
			return new ItemCollection.DeferHelper(this);
		}

		// Token: 0x1700191E RID: 6430
		// (get) Token: 0x06006CD1 RID: 27857 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700191F RID: 6431
		// (get) Token: 0x06006CD2 RID: 27858 RVA: 0x002CA2DE File Offset: 0x002C92DE
		object ICollection.SyncRoot
		{
			get
			{
				if (this.IsUsingItemsSource)
				{
					throw new NotSupportedException(SR.Get("ItemCollectionShouldUseInnerSyncRoot"));
				}
				return this._internalView.SyncRoot;
			}
		}

		// Token: 0x17001920 RID: 6432
		// (get) Token: 0x06006CD3 RID: 27859 RVA: 0x002CA303 File Offset: 0x002C9303
		bool IList.IsFixedSize
		{
			get
			{
				return this.IsUsingItemsSource;
			}
		}

		// Token: 0x17001921 RID: 6433
		// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x002CA303 File Offset: 0x002C9303
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsUsingItemsSource;
			}
		}

		// Token: 0x17001922 RID: 6434
		// (get) Token: 0x06006CD5 RID: 27861 RVA: 0x002CA30B File Offset: 0x002C930B
		public override int CurrentPosition
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return -1;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.CurrentPosition;
			}
		}

		// Token: 0x17001923 RID: 6435
		// (get) Token: 0x06006CD6 RID: 27862 RVA: 0x002CA328 File Offset: 0x002C9328
		public override object CurrentItem
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return null;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.CurrentItem;
			}
		}

		// Token: 0x17001924 RID: 6436
		// (get) Token: 0x06006CD7 RID: 27863 RVA: 0x002CA345 File Offset: 0x002C9345
		public override bool IsCurrentAfterLast
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return false;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.IsCurrentAfterLast;
			}
		}

		// Token: 0x17001925 RID: 6437
		// (get) Token: 0x06006CD8 RID: 27864 RVA: 0x002CA362 File Offset: 0x002C9362
		public override bool IsCurrentBeforeFirst
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return false;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.IsCurrentBeforeFirst;
			}
		}

		// Token: 0x17001926 RID: 6438
		// (get) Token: 0x06006CD9 RID: 27865 RVA: 0x002CA380 File Offset: 0x002C9380
		// (set) Token: 0x06006CDA RID: 27866 RVA: 0x002CA3A4 File Offset: 0x002C93A4
		NewItemPlaceholderPosition IEditableCollectionView.NewItemPlaceholderPosition
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.NewItemPlaceholderPosition;
				}
				return NewItemPlaceholderPosition.None;
			}
			set
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					editableCollectionView.NewItemPlaceholderPosition = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
				{
					"NewItemPlaceholderPosition"
				}));
			}
		}

		// Token: 0x17001927 RID: 6439
		// (get) Token: 0x06006CDB RID: 27867 RVA: 0x002CA3E8 File Offset: 0x002C93E8
		bool IEditableCollectionView.CanAddNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanAddNew;
			}
		}

		// Token: 0x06006CDC RID: 27868 RVA: 0x002CA40C File Offset: 0x002C940C
		object IEditableCollectionView.AddNew()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				return editableCollectionView.AddNew();
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNew"
			}));
		}

		// Token: 0x06006CDD RID: 27869 RVA: 0x002CA44C File Offset: 0x002C944C
		void IEditableCollectionView.CommitNew()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CommitNew();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CommitNew"
			}));
		}

		// Token: 0x06006CDE RID: 27870 RVA: 0x002CA48C File Offset: 0x002C948C
		void IEditableCollectionView.CancelNew()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CancelNew();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CancelNew"
			}));
		}

		// Token: 0x17001928 RID: 6440
		// (get) Token: 0x06006CDF RID: 27871 RVA: 0x002CA4CC File Offset: 0x002C94CC
		bool IEditableCollectionView.IsAddingNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsAddingNew;
			}
		}

		// Token: 0x17001929 RID: 6441
		// (get) Token: 0x06006CE0 RID: 27872 RVA: 0x002CA4F0 File Offset: 0x002C94F0
		object IEditableCollectionView.CurrentAddItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentAddItem;
				}
				return null;
			}
		}

		// Token: 0x1700192A RID: 6442
		// (get) Token: 0x06006CE1 RID: 27873 RVA: 0x002CA514 File Offset: 0x002C9514
		bool IEditableCollectionView.CanRemove
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanRemove;
			}
		}

		// Token: 0x06006CE2 RID: 27874 RVA: 0x002CA538 File Offset: 0x002C9538
		void IEditableCollectionView.RemoveAt(int index)
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.RemoveAt(index);
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"RemoveAt"
			}));
		}

		// Token: 0x06006CE3 RID: 27875 RVA: 0x002CA57C File Offset: 0x002C957C
		void IEditableCollectionView.Remove(object item)
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.Remove(item);
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"Remove"
			}));
		}

		// Token: 0x06006CE4 RID: 27876 RVA: 0x002CA5C0 File Offset: 0x002C95C0
		void IEditableCollectionView.EditItem(object item)
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.EditItem(item);
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"EditItem"
			}));
		}

		// Token: 0x06006CE5 RID: 27877 RVA: 0x002CA604 File Offset: 0x002C9604
		void IEditableCollectionView.CommitEdit()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CommitEdit();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CommitEdit"
			}));
		}

		// Token: 0x06006CE6 RID: 27878 RVA: 0x002CA644 File Offset: 0x002C9644
		void IEditableCollectionView.CancelEdit()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CancelEdit();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CancelEdit"
			}));
		}

		// Token: 0x1700192B RID: 6443
		// (get) Token: 0x06006CE7 RID: 27879 RVA: 0x002CA684 File Offset: 0x002C9684
		bool IEditableCollectionView.CanCancelEdit
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanCancelEdit;
			}
		}

		// Token: 0x1700192C RID: 6444
		// (get) Token: 0x06006CE8 RID: 27880 RVA: 0x002CA6A8 File Offset: 0x002C96A8
		bool IEditableCollectionView.IsEditingItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsEditingItem;
			}
		}

		// Token: 0x1700192D RID: 6445
		// (get) Token: 0x06006CE9 RID: 27881 RVA: 0x002CA6CC File Offset: 0x002C96CC
		object IEditableCollectionView.CurrentEditItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentEditItem;
				}
				return null;
			}
		}

		// Token: 0x1700192E RID: 6446
		// (get) Token: 0x06006CEA RID: 27882 RVA: 0x002CA6F0 File Offset: 0x002C96F0
		bool IEditableCollectionViewAddNewItem.CanAddNewItem
		{
			get
			{
				IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this._collectionView as IEditableCollectionViewAddNewItem;
				return editableCollectionViewAddNewItem != null && editableCollectionViewAddNewItem.CanAddNewItem;
			}
		}

		// Token: 0x06006CEB RID: 27883 RVA: 0x002CA714 File Offset: 0x002C9714
		object IEditableCollectionViewAddNewItem.AddNewItem(object newItem)
		{
			IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this._collectionView as IEditableCollectionViewAddNewItem;
			if (editableCollectionViewAddNewItem != null)
			{
				return editableCollectionViewAddNewItem.AddNewItem(newItem);
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNewItem"
			}));
		}

		// Token: 0x1700192F RID: 6447
		// (get) Token: 0x06006CEC RID: 27884 RVA: 0x002CA758 File Offset: 0x002C9758
		public bool CanChangeLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveSorting;
			}
		}

		// Token: 0x17001930 RID: 6448
		// (get) Token: 0x06006CED RID: 27885 RVA: 0x002CA77C File Offset: 0x002C977C
		public bool CanChangeLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveFiltering;
			}
		}

		// Token: 0x17001931 RID: 6449
		// (get) Token: 0x06006CEE RID: 27886 RVA: 0x002CA7A0 File Offset: 0x002C97A0
		public bool CanChangeLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveGrouping;
			}
		}

		// Token: 0x17001932 RID: 6450
		// (get) Token: 0x06006CEF RID: 27887 RVA: 0x002CA7C4 File Offset: 0x002C97C4
		// (set) Token: 0x06006CF0 RID: 27888 RVA: 0x002CA7F0 File Offset: 0x002C97F0
		public bool? IsLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveSorting;
			}
			set
			{
				this.MyIsLiveSorting = value;
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveSorting)
				{
					collectionViewLiveShaping.IsLiveSorting = value;
				}
			}
		}

		// Token: 0x17001933 RID: 6451
		// (get) Token: 0x06006CF1 RID: 27889 RVA: 0x002CA824 File Offset: 0x002C9824
		// (set) Token: 0x06006CF2 RID: 27890 RVA: 0x002CA850 File Offset: 0x002C9850
		public bool? IsLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveFiltering;
			}
			set
			{
				this.MyIsLiveFiltering = value;
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveFiltering)
				{
					collectionViewLiveShaping.IsLiveFiltering = value;
				}
			}
		}

		// Token: 0x17001934 RID: 6452
		// (get) Token: 0x06006CF3 RID: 27891 RVA: 0x002CA884 File Offset: 0x002C9884
		// (set) Token: 0x06006CF4 RID: 27892 RVA: 0x002CA8B0 File Offset: 0x002C98B0
		public bool? IsLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveGrouping;
			}
			set
			{
				this.MyIsLiveGrouping = value;
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveGrouping)
				{
					collectionViewLiveShaping.IsLiveGrouping = value;
				}
			}
		}

		// Token: 0x17001935 RID: 6453
		// (get) Token: 0x06006CF5 RID: 27893 RVA: 0x002CA8E4 File Offset: 0x002C98E4
		public ObservableCollection<string> LiveSortingProperties
		{
			get
			{
				if (this.MyLiveSortingProperties == null)
				{
					this.MyLiveSortingProperties = new ObservableCollection<string>();
					ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
					if (collectionViewLiveShaping != null)
					{
						this.CloneList(this.MyLiveSortingProperties, collectionViewLiveShaping.LiveSortingProperties);
					}
					((INotifyCollectionChanged)this.MyLiveSortingProperties).CollectionChanged += this.LiveSortingChanged;
				}
				return this.MyLiveSortingProperties;
			}
		}

		// Token: 0x17001936 RID: 6454
		// (get) Token: 0x06006CF6 RID: 27894 RVA: 0x002CA944 File Offset: 0x002C9944
		public ObservableCollection<string> LiveFilteringProperties
		{
			get
			{
				if (this.MyLiveFilteringProperties == null)
				{
					this.MyLiveFilteringProperties = new ObservableCollection<string>();
					ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
					if (collectionViewLiveShaping != null)
					{
						this.CloneList(this.MyLiveFilteringProperties, collectionViewLiveShaping.LiveFilteringProperties);
					}
					((INotifyCollectionChanged)this.MyLiveFilteringProperties).CollectionChanged += this.LiveFilteringChanged;
				}
				return this.MyLiveFilteringProperties;
			}
		}

		// Token: 0x17001937 RID: 6455
		// (get) Token: 0x06006CF7 RID: 27895 RVA: 0x002CA9A4 File Offset: 0x002C99A4
		public ObservableCollection<string> LiveGroupingProperties
		{
			get
			{
				if (this.MyLiveGroupingProperties == null)
				{
					this.MyLiveGroupingProperties = new ObservableCollection<string>();
					ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
					if (collectionViewLiveShaping != null)
					{
						this.CloneList(this.MyLiveGroupingProperties, collectionViewLiveShaping.LiveGroupingProperties);
					}
					((INotifyCollectionChanged)this.MyLiveGroupingProperties).CollectionChanged += this.LiveGroupingChanged;
				}
				return this.MyLiveGroupingProperties;
			}
		}

		// Token: 0x17001938 RID: 6456
		// (get) Token: 0x06006CF8 RID: 27896 RVA: 0x002CAA04 File Offset: 0x002C9A04
		ReadOnlyCollection<ItemPropertyInfo> IItemProperties.ItemProperties
		{
			get
			{
				IItemProperties itemProperties = this._collectionView as IItemProperties;
				if (itemProperties != null)
				{
					return itemProperties.ItemProperties;
				}
				return null;
			}
		}

		// Token: 0x17001939 RID: 6457
		// (get) Token: 0x06006CF9 RID: 27897 RVA: 0x002CAA28 File Offset: 0x002C9A28
		internal DependencyObject ModelParent
		{
			get
			{
				return (DependencyObject)this._modelParent.Target;
			}
		}

		// Token: 0x1700193A RID: 6458
		// (get) Token: 0x06006CFA RID: 27898 RVA: 0x002CAA3A File Offset: 0x002C9A3A
		internal FrameworkElement ModelParentFE
		{
			get
			{
				return this.ModelParent as FrameworkElement;
			}
		}

		// Token: 0x06006CFB RID: 27899 RVA: 0x002CAA48 File Offset: 0x002C9A48
		internal void SetItemsSource(IEnumerable value, Func<object, object> GetSourceItem = null)
		{
			if (!this.IsUsingItemsSource && this._internalView != null && this._internalView.RawCount > 0)
			{
				throw new InvalidOperationException(SR.Get("CannotUseItemsSource"));
			}
			this._itemsSource = value;
			this._isUsingItemsSource = true;
			this.SetCollectionView(CollectionViewSource.GetDefaultCollectionView(this._itemsSource, this.ModelParent, GetSourceItem));
		}

		// Token: 0x06006CFC RID: 27900 RVA: 0x002CAAA9 File Offset: 0x002C9AA9
		internal void ClearItemsSource()
		{
			if (this.IsUsingItemsSource)
			{
				this._itemsSource = null;
				this._isUsingItemsSource = false;
				this.SetCollectionView(this._internalView);
			}
		}

		// Token: 0x1700193B RID: 6459
		// (get) Token: 0x06006CFD RID: 27901 RVA: 0x002CAACD File Offset: 0x002C9ACD
		internal IEnumerable ItemsSource
		{
			get
			{
				return this._itemsSource;
			}
		}

		// Token: 0x1700193C RID: 6460
		// (get) Token: 0x06006CFE RID: 27902 RVA: 0x002CAAD5 File Offset: 0x002C9AD5
		internal bool IsUsingItemsSource
		{
			get
			{
				return this._isUsingItemsSource;
			}
		}

		// Token: 0x1700193D RID: 6461
		// (get) Token: 0x06006CFF RID: 27903 RVA: 0x002CAADD File Offset: 0x002C9ADD
		internal CollectionView CollectionView
		{
			get
			{
				return this._collectionView;
			}
		}

		// Token: 0x06006D00 RID: 27904 RVA: 0x002CAAE5 File Offset: 0x002C9AE5
		internal void BeginInit()
		{
			this._isInitializing = true;
			if (this._collectionView != null)
			{
				this.UnhookCollectionView(this._collectionView);
			}
		}

		// Token: 0x06006D01 RID: 27905 RVA: 0x002CAB02 File Offset: 0x002C9B02
		internal void EndInit()
		{
			this.EnsureCollectionView();
			this._isInitializing = false;
			if (this._collectionView != null)
			{
				this.HookCollectionView(this._collectionView);
				this.Refresh();
			}
		}

		// Token: 0x1700193E RID: 6462
		// (get) Token: 0x06006D02 RID: 27906 RVA: 0x002CAB2C File Offset: 0x002C9B2C
		internal IEnumerator LogicalChildren
		{
			get
			{
				this.EnsureInternalView();
				return this._internalView.LogicalChildren;
			}
		}

		// Token: 0x06006D03 RID: 27907 RVA: 0x002CAB3F File Offset: 0x002C9B3F
		internal override void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			if (this._collectionView != null)
			{
				this._collectionView.GetCollectionChangedSources(level + 1, format, sources);
			}
		}

		// Token: 0x1700193F RID: 6463
		// (get) Token: 0x06006D04 RID: 27908 RVA: 0x002CAB68 File Offset: 0x002C9B68
		private new bool IsRefreshDeferred
		{
			get
			{
				return this._deferLevel > 0;
			}
		}

		// Token: 0x06006D05 RID: 27909 RVA: 0x002CAB74 File Offset: 0x002C9B74
		private bool EnsureCollectionView()
		{
			if (this._collectionView == null && !this.IsUsingItemsSource && this._internalView != null)
			{
				if (this._internalView.IsEmpty)
				{
					bool isInitializing = this._isInitializing;
					this._isInitializing = true;
					this.SetCollectionView(this._internalView);
					this._isInitializing = isInitializing;
				}
				else
				{
					this.SetCollectionView(this._internalView);
				}
				if (!this._isInitializing)
				{
					this.HookCollectionView(this._collectionView);
				}
			}
			return this._collectionView != null;
		}

		// Token: 0x06006D06 RID: 27910 RVA: 0x002CABF2 File Offset: 0x002C9BF2
		private void EnsureInternalView()
		{
			if (this._internalView == null)
			{
				this._internalView = new InnerItemCollectionView(this._defaultCapacity, this);
			}
		}

		// Token: 0x06006D07 RID: 27911 RVA: 0x002CAC10 File Offset: 0x002C9C10
		private void SetCollectionView(CollectionView view)
		{
			if (this._collectionView == view)
			{
				return;
			}
			if (this._collectionView != null)
			{
				if (!this._isInitializing)
				{
					this.UnhookCollectionView(this._collectionView);
				}
				if (this.IsRefreshDeferred)
				{
					this._deferInnerRefresh.Dispose();
					this._deferInnerRefresh = null;
				}
			}
			bool flag = false;
			this._collectionView = view;
			base.InvalidateEnumerableWrapper();
			if (this._collectionView != null)
			{
				this._deferInnerRefresh = this._collectionView.DeferRefresh();
				this.ApplySortFilterAndGroup();
				if (!this._isInitializing)
				{
					this.HookCollectionView(this._collectionView);
				}
				if (!this.IsRefreshDeferred)
				{
					flag = !this._collectionView.NeedsRefresh;
					this._deferInnerRefresh.Dispose();
					this._deferInnerRefresh = null;
				}
			}
			else if (!this.IsRefreshDeferred)
			{
				flag = true;
			}
			if (flag)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
			this.OnPropertyChanged(new PropertyChangedEventArgs("IsLiveSorting"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("IsLiveFiltering"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("IsLiveGrouping"));
		}

		// Token: 0x06006D08 RID: 27912 RVA: 0x002CAD14 File Offset: 0x002C9D14
		private void ApplySortFilterAndGroup()
		{
			if (!this.IsShapingActive)
			{
				return;
			}
			if (this._collectionView.CanSort)
			{
				SortDescriptionCollection master = this.IsSortingSet ? this.MySortDescriptions : this._collectionView.SortDescriptions;
				SortDescriptionCollection clone = this.IsSortingSet ? this._collectionView.SortDescriptions : this.MySortDescriptions;
				using (this.SortDescriptionsMonitor.Enter())
				{
					this.CloneList(clone, master);
				}
			}
			if (this._collectionView.CanFilter && this.MyFilter != null)
			{
				this._collectionView.Filter = this.MyFilter;
			}
			if (this._collectionView.CanGroup)
			{
				ObservableCollection<GroupDescription> master2 = this.IsGroupingSet ? this.MyGroupDescriptions : this._collectionView.GroupDescriptions;
				ObservableCollection<GroupDescription> clone2 = this.IsGroupingSet ? this._collectionView.GroupDescriptions : this.MyGroupDescriptions;
				using (this.GroupDescriptionsMonitor.Enter())
				{
					this.CloneList(clone2, master2);
				}
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				if (this.MyIsLiveSorting != null && collectionViewLiveShaping.CanChangeLiveSorting)
				{
					collectionViewLiveShaping.IsLiveSorting = this.MyIsLiveSorting;
				}
				if (this.MyIsLiveFiltering != null && collectionViewLiveShaping.CanChangeLiveFiltering)
				{
					collectionViewLiveShaping.IsLiveFiltering = this.MyIsLiveFiltering;
				}
				if (this.MyIsLiveGrouping != null && collectionViewLiveShaping.CanChangeLiveGrouping)
				{
					collectionViewLiveShaping.IsLiveGrouping = this.MyIsLiveGrouping;
				}
			}
		}

		// Token: 0x06006D09 RID: 27913 RVA: 0x002CAEB8 File Offset: 0x002C9EB8
		private void HookCollectionView(CollectionView view)
		{
			CollectionChangedEventManager.AddHandler(view, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnViewCollectionChanged));
			CurrentChangingEventManager.AddHandler(view, new EventHandler<CurrentChangingEventArgs>(this.OnCurrentChanging));
			CurrentChangedEventManager.AddHandler(view, new EventHandler<EventArgs>(this.OnCurrentChanged));
			PropertyChangedEventManager.AddHandler(view, new EventHandler<PropertyChangedEventArgs>(this.OnViewPropertyChanged), string.Empty);
			SortDescriptionCollection sortDescriptions = view.SortDescriptions;
			if (sortDescriptions != null && sortDescriptions != SortDescriptionCollection.Empty)
			{
				CollectionChangedEventManager.AddHandler(sortDescriptions, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerSortDescriptionsChanged));
			}
			ObservableCollection<GroupDescription> groupDescriptions = view.GroupDescriptions;
			if (groupDescriptions != null)
			{
				CollectionChangedEventManager.AddHandler(groupDescriptions, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerGroupDescriptionsChanged));
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = view as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				ObservableCollection<string> liveSortingProperties = collectionViewLiveShaping.LiveSortingProperties;
				if (liveSortingProperties != null)
				{
					CollectionChangedEventManager.AddHandler(liveSortingProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveSortingChanged));
				}
				ObservableCollection<string> liveFilteringProperties = collectionViewLiveShaping.LiveFilteringProperties;
				if (liveFilteringProperties != null)
				{
					CollectionChangedEventManager.AddHandler(liveFilteringProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveFilteringChanged));
				}
				ObservableCollection<string> liveGroupingProperties = collectionViewLiveShaping.LiveGroupingProperties;
				if (liveGroupingProperties != null)
				{
					CollectionChangedEventManager.AddHandler(liveGroupingProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveGroupingChanged));
				}
			}
		}

		// Token: 0x06006D0A RID: 27914 RVA: 0x002CAFB8 File Offset: 0x002C9FB8
		private void UnhookCollectionView(CollectionView view)
		{
			CollectionChangedEventManager.RemoveHandler(view, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnViewCollectionChanged));
			CurrentChangingEventManager.RemoveHandler(view, new EventHandler<CurrentChangingEventArgs>(this.OnCurrentChanging));
			CurrentChangedEventManager.RemoveHandler(view, new EventHandler<EventArgs>(this.OnCurrentChanged));
			PropertyChangedEventManager.RemoveHandler(view, new EventHandler<PropertyChangedEventArgs>(this.OnViewPropertyChanged), string.Empty);
			SortDescriptionCollection sortDescriptions = view.SortDescriptions;
			if (sortDescriptions != null && sortDescriptions != SortDescriptionCollection.Empty)
			{
				CollectionChangedEventManager.RemoveHandler(sortDescriptions, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerSortDescriptionsChanged));
			}
			ObservableCollection<GroupDescription> groupDescriptions = view.GroupDescriptions;
			if (groupDescriptions != null)
			{
				CollectionChangedEventManager.RemoveHandler(groupDescriptions, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerGroupDescriptionsChanged));
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = view as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				ObservableCollection<string> liveSortingProperties = collectionViewLiveShaping.LiveSortingProperties;
				if (liveSortingProperties != null)
				{
					CollectionChangedEventManager.RemoveHandler(liveSortingProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveSortingChanged));
				}
				ObservableCollection<string> liveFilteringProperties = collectionViewLiveShaping.LiveFilteringProperties;
				if (liveFilteringProperties != null)
				{
					CollectionChangedEventManager.RemoveHandler(liveFilteringProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveFilteringChanged));
				}
				ObservableCollection<string> liveGroupingProperties = collectionViewLiveShaping.LiveGroupingProperties;
				if (liveGroupingProperties != null)
				{
					CollectionChangedEventManager.RemoveHandler(liveGroupingProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveGroupingChanged));
				}
			}
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				if (editableCollectionView.IsAddingNew)
				{
					editableCollectionView.CancelNew();
				}
				if (editableCollectionView.IsEditingItem)
				{
					if (editableCollectionView.CanCancelEdit)
					{
						editableCollectionView.CancelEdit();
						return;
					}
					editableCollectionView.CommitEdit();
				}
			}
		}

		// Token: 0x06006D0B RID: 27915 RVA: 0x002CB0F3 File Offset: 0x002CA0F3
		private void OnViewCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.InvalidateEnumerableWrapper();
			this.OnCollectionChanged(e);
		}

		// Token: 0x06006D0C RID: 27916 RVA: 0x002CB102 File Offset: 0x002CA102
		private void OnCurrentChanged(object sender, EventArgs e)
		{
			this.OnCurrentChanged();
		}

		// Token: 0x06006D0D RID: 27917 RVA: 0x002CB10A File Offset: 0x002CA10A
		private void OnCurrentChanging(object sender, CurrentChangingEventArgs e)
		{
			this.OnCurrentChanging(e);
		}

		// Token: 0x06006D0E RID: 27918 RVA: 0x0014EE59 File Offset: 0x0014DE59
		private void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged(e);
		}

		// Token: 0x06006D0F RID: 27919 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x06006D10 RID: 27920 RVA: 0x002CB113 File Offset: 0x002CA113
		private void CheckIsUsingInnerView()
		{
			if (this.IsUsingItemsSource)
			{
				throw new InvalidOperationException(SR.Get("ItemsSourceInUse"));
			}
			this.EnsureInternalView();
			this.EnsureCollectionView();
			this.VerifyRefreshNotDeferred();
		}

		// Token: 0x06006D11 RID: 27921 RVA: 0x002CB140 File Offset: 0x002CA140
		private void EndDefer()
		{
			this._deferLevel--;
			if (this._deferLevel == 0)
			{
				if (this._deferInnerRefresh != null)
				{
					IDisposable deferInnerRefresh = this._deferInnerRefresh;
					this._deferInnerRefresh = null;
					deferInnerRefresh.Dispose();
					return;
				}
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x06006D12 RID: 27922 RVA: 0x002CB17F File Offset: 0x002CA17F
		private new void VerifyRefreshNotDeferred()
		{
			if (this.IsRefreshDeferred)
			{
				throw new InvalidOperationException(SR.Get("NoCheckOrChangeWhenDeferred"));
			}
		}

		// Token: 0x06006D13 RID: 27923 RVA: 0x002CB19C File Offset: 0x002CA19C
		private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.SortDescriptionsMonitor.Busy)
			{
				return;
			}
			if (this._collectionView != null && this._collectionView.CanSort)
			{
				using (this.SortDescriptionsMonitor.Enter())
				{
					this.SynchronizeCollections<SortDescription>(e, this.MySortDescriptions, this._collectionView.SortDescriptions);
				}
			}
			this.IsSortingSet = true;
		}

		// Token: 0x06006D14 RID: 27924 RVA: 0x002CB214 File Offset: 0x002CA214
		private void OnInnerSortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.SortDescriptionsMonitor.Busy)
			{
				return;
			}
			using (this.SortDescriptionsMonitor.Enter())
			{
				this.SynchronizeCollections<SortDescription>(e, this._collectionView.SortDescriptions, this.MySortDescriptions);
			}
			this.IsSortingSet = false;
		}

		// Token: 0x06006D15 RID: 27925 RVA: 0x002CB280 File Offset: 0x002CA280
		private void GroupDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.GroupDescriptionsMonitor.Busy)
			{
				return;
			}
			if (this._collectionView != null && this._collectionView.CanGroup)
			{
				using (this.GroupDescriptionsMonitor.Enter())
				{
					this.SynchronizeCollections<GroupDescription>(e, this.MyGroupDescriptions, this._collectionView.GroupDescriptions);
				}
			}
			this.IsGroupingSet = true;
		}

		// Token: 0x06006D16 RID: 27926 RVA: 0x002CB2F8 File Offset: 0x002CA2F8
		private void OnInnerGroupDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.GroupDescriptionsMonitor.Busy)
			{
				return;
			}
			using (this.GroupDescriptionsMonitor.Enter())
			{
				this.SynchronizeCollections<GroupDescription>(e, this._collectionView.GroupDescriptions, this.MyGroupDescriptions);
			}
			this.IsGroupingSet = false;
		}

		// Token: 0x06006D17 RID: 27927 RVA: 0x002CB364 File Offset: 0x002CA364
		private void LiveSortingChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.LiveSortingMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveSortingMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, this.MyLiveSortingProperties, collectionViewLiveShaping.LiveSortingProperties);
				}
			}
			this.IsLiveSortingSet = true;
		}

		// Token: 0x06006D18 RID: 27928 RVA: 0x002CB3D0 File Offset: 0x002CA3D0
		private void OnInnerLiveSortingChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.LiveSortingMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveSortingMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, collectionViewLiveShaping.LiveSortingProperties, this.MyLiveSortingProperties);
				}
			}
			this.IsLiveSortingSet = false;
		}

		// Token: 0x06006D19 RID: 27929 RVA: 0x002CB444 File Offset: 0x002CA444
		private void LiveFilteringChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.LiveFilteringMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveFilteringMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, this.MyLiveFilteringProperties, collectionViewLiveShaping.LiveFilteringProperties);
				}
			}
			this.IsLiveFilteringSet = true;
		}

		// Token: 0x06006D1A RID: 27930 RVA: 0x002CB4B0 File Offset: 0x002CA4B0
		private void OnInnerLiveFilteringChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.LiveFilteringMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveFilteringMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, collectionViewLiveShaping.LiveFilteringProperties, this.MyLiveFilteringProperties);
				}
			}
			this.IsLiveFilteringSet = false;
		}

		// Token: 0x06006D1B RID: 27931 RVA: 0x002CB524 File Offset: 0x002CA524
		private void LiveGroupingChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.LiveGroupingMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveGroupingMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, this.MyLiveGroupingProperties, collectionViewLiveShaping.LiveGroupingProperties);
				}
			}
			this.IsLiveGroupingSet = true;
		}

		// Token: 0x06006D1C RID: 27932 RVA: 0x002CB590 File Offset: 0x002CA590
		private void OnInnerLiveGroupingChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.LiveGroupingMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveGroupingMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, collectionViewLiveShaping.LiveGroupingProperties, this.MyLiveGroupingProperties);
				}
			}
			this.IsLiveGroupingSet = false;
		}

		// Token: 0x06006D1D RID: 27933 RVA: 0x002CB604 File Offset: 0x002CA604
		private void SynchronizeCollections<T>(NotifyCollectionChangedEventArgs e, Collection<T> origin, Collection<T> clone)
		{
			if (clone == null)
			{
				return;
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (clone.Count + e.NewItems.Count == origin.Count)
				{
					for (int i = 0; i < e.NewItems.Count; i++)
					{
						clone.Insert(e.NewStartingIndex + i, (T)((object)e.NewItems[i]));
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (clone.Count - e.OldItems.Count == origin.Count)
				{
					for (int j = 0; j < e.OldItems.Count; j++)
					{
						clone.RemoveAt(e.OldStartingIndex);
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (clone.Count == origin.Count)
				{
					for (int k = 0; k < e.OldItems.Count; k++)
					{
						clone[e.OldStartingIndex + k] = (T)((object)e.NewItems[k]);
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (clone.Count == origin.Count)
				{
					if (e.NewItems.Count == 1)
					{
						clone.RemoveAt(e.OldStartingIndex);
						clone.Insert(e.NewStartingIndex, (T)((object)e.NewItems[0]));
						return;
					}
					for (int l = 0; l < e.OldItems.Count; l++)
					{
						clone.RemoveAt(e.OldStartingIndex);
					}
					for (int m = 0; m < e.NewItems.Count; m++)
					{
						clone.Insert(e.NewStartingIndex + m, (T)((object)e.NewItems[m]));
					}
					return;
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
			this.CloneList(clone, origin);
		}

		// Token: 0x06006D1E RID: 27934 RVA: 0x002CB7EC File Offset: 0x002CA7EC
		private void CloneList(IList clone, IList master)
		{
			if (clone == null || master == null)
			{
				return;
			}
			if (clone.Count > 0)
			{
				clone.Clear();
			}
			int i = 0;
			int count = master.Count;
			while (i < count)
			{
				clone.Add(master[i]);
				i++;
			}
		}

		// Token: 0x17001940 RID: 6464
		// (get) Token: 0x06006D1F RID: 27935 RVA: 0x002CB830 File Offset: 0x002CA830
		private bool IsShapingActive
		{
			get
			{
				return this._shapingStorage != null;
			}
		}

		// Token: 0x06006D20 RID: 27936 RVA: 0x002CB83B File Offset: 0x002CA83B
		private void EnsureShapingStorage()
		{
			if (!this.IsShapingActive)
			{
				this._shapingStorage = new ItemCollection.ShapingStorage();
			}
		}

		// Token: 0x17001941 RID: 6465
		// (get) Token: 0x06006D21 RID: 27937 RVA: 0x002CB850 File Offset: 0x002CA850
		// (set) Token: 0x06006D22 RID: 27938 RVA: 0x002CB867 File Offset: 0x002CA867
		private SortDescriptionCollection MySortDescriptions
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._sort;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._sort = value;
			}
		}

		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x06006D23 RID: 27939 RVA: 0x002CB87B File Offset: 0x002CA87B
		// (set) Token: 0x06006D24 RID: 27940 RVA: 0x002CB892 File Offset: 0x002CA892
		private bool IsSortingSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isSortingSet;
			}
			set
			{
				this._shapingStorage._isSortingSet = value;
			}
		}

		// Token: 0x17001943 RID: 6467
		// (get) Token: 0x06006D25 RID: 27941 RVA: 0x002CB8A0 File Offset: 0x002CA8A0
		private MonitorWrapper SortDescriptionsMonitor
		{
			get
			{
				if (this._shapingStorage._sortDescriptionsMonitor == null)
				{
					this._shapingStorage._sortDescriptionsMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._sortDescriptionsMonitor;
			}
		}

		// Token: 0x17001944 RID: 6468
		// (get) Token: 0x06006D26 RID: 27942 RVA: 0x002CB8CA File Offset: 0x002CA8CA
		// (set) Token: 0x06006D27 RID: 27943 RVA: 0x002CB8E1 File Offset: 0x002CA8E1
		private Predicate<object> MyFilter
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._filter;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._filter = value;
			}
		}

		// Token: 0x17001945 RID: 6469
		// (get) Token: 0x06006D28 RID: 27944 RVA: 0x002CB8F5 File Offset: 0x002CA8F5
		// (set) Token: 0x06006D29 RID: 27945 RVA: 0x002CB90C File Offset: 0x002CA90C
		private ObservableCollection<GroupDescription> MyGroupDescriptions
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._groupBy;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._groupBy = value;
			}
		}

		// Token: 0x17001946 RID: 6470
		// (get) Token: 0x06006D2A RID: 27946 RVA: 0x002CB920 File Offset: 0x002CA920
		// (set) Token: 0x06006D2B RID: 27947 RVA: 0x002CB937 File Offset: 0x002CA937
		private bool IsGroupingSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isGroupingSet;
			}
			set
			{
				if (this.IsShapingActive)
				{
					this._shapingStorage._isGroupingSet = value;
				}
			}
		}

		// Token: 0x17001947 RID: 6471
		// (get) Token: 0x06006D2C RID: 27948 RVA: 0x002CB94D File Offset: 0x002CA94D
		private MonitorWrapper GroupDescriptionsMonitor
		{
			get
			{
				if (this._shapingStorage._groupDescriptionsMonitor == null)
				{
					this._shapingStorage._groupDescriptionsMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._groupDescriptionsMonitor;
			}
		}

		// Token: 0x17001948 RID: 6472
		// (get) Token: 0x06006D2D RID: 27949 RVA: 0x002CB978 File Offset: 0x002CA978
		// (set) Token: 0x06006D2E RID: 27950 RVA: 0x002CB9A2 File Offset: 0x002CA9A2
		private bool? MyIsLiveSorting
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._isLiveSorting;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._isLiveSorting = value;
			}
		}

		// Token: 0x17001949 RID: 6473
		// (get) Token: 0x06006D2F RID: 27951 RVA: 0x002CB9B6 File Offset: 0x002CA9B6
		// (set) Token: 0x06006D30 RID: 27952 RVA: 0x002CB9CD File Offset: 0x002CA9CD
		private ObservableCollection<string> MyLiveSortingProperties
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._liveSortingProperties;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._liveSortingProperties = value;
			}
		}

		// Token: 0x1700194A RID: 6474
		// (get) Token: 0x06006D31 RID: 27953 RVA: 0x002CB9E1 File Offset: 0x002CA9E1
		// (set) Token: 0x06006D32 RID: 27954 RVA: 0x002CB9F8 File Offset: 0x002CA9F8
		private bool IsLiveSortingSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isLiveSortingSet;
			}
			set
			{
				this._shapingStorage._isLiveSortingSet = value;
			}
		}

		// Token: 0x1700194B RID: 6475
		// (get) Token: 0x06006D33 RID: 27955 RVA: 0x002CBA06 File Offset: 0x002CAA06
		private MonitorWrapper LiveSortingMonitor
		{
			get
			{
				if (this._shapingStorage._liveSortingMonitor == null)
				{
					this._shapingStorage._liveSortingMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._liveSortingMonitor;
			}
		}

		// Token: 0x1700194C RID: 6476
		// (get) Token: 0x06006D34 RID: 27956 RVA: 0x002CBA30 File Offset: 0x002CAA30
		// (set) Token: 0x06006D35 RID: 27957 RVA: 0x002CBA5A File Offset: 0x002CAA5A
		private bool? MyIsLiveFiltering
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._isLiveFiltering;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._isLiveFiltering = value;
			}
		}

		// Token: 0x1700194D RID: 6477
		// (get) Token: 0x06006D36 RID: 27958 RVA: 0x002CBA6E File Offset: 0x002CAA6E
		// (set) Token: 0x06006D37 RID: 27959 RVA: 0x002CBA85 File Offset: 0x002CAA85
		private ObservableCollection<string> MyLiveFilteringProperties
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._liveFilteringProperties;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._liveFilteringProperties = value;
			}
		}

		// Token: 0x1700194E RID: 6478
		// (get) Token: 0x06006D38 RID: 27960 RVA: 0x002CBA99 File Offset: 0x002CAA99
		// (set) Token: 0x06006D39 RID: 27961 RVA: 0x002CBAB0 File Offset: 0x002CAAB0
		private bool IsLiveFilteringSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isLiveFilteringSet;
			}
			set
			{
				this._shapingStorage._isLiveFilteringSet = value;
			}
		}

		// Token: 0x1700194F RID: 6479
		// (get) Token: 0x06006D3A RID: 27962 RVA: 0x002CBABE File Offset: 0x002CAABE
		private MonitorWrapper LiveFilteringMonitor
		{
			get
			{
				if (this._shapingStorage._liveFilteringMonitor == null)
				{
					this._shapingStorage._liveFilteringMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._liveFilteringMonitor;
			}
		}

		// Token: 0x17001950 RID: 6480
		// (get) Token: 0x06006D3B RID: 27963 RVA: 0x002CBAE8 File Offset: 0x002CAAE8
		// (set) Token: 0x06006D3C RID: 27964 RVA: 0x002CBB12 File Offset: 0x002CAB12
		private bool? MyIsLiveGrouping
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._isLiveGrouping;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._isLiveGrouping = value;
			}
		}

		// Token: 0x17001951 RID: 6481
		// (get) Token: 0x06006D3D RID: 27965 RVA: 0x002CBB26 File Offset: 0x002CAB26
		// (set) Token: 0x06006D3E RID: 27966 RVA: 0x002CBB3D File Offset: 0x002CAB3D
		private ObservableCollection<string> MyLiveGroupingProperties
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._liveGroupingProperties;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._liveGroupingProperties = value;
			}
		}

		// Token: 0x17001952 RID: 6482
		// (get) Token: 0x06006D3F RID: 27967 RVA: 0x002CBB51 File Offset: 0x002CAB51
		// (set) Token: 0x06006D40 RID: 27968 RVA: 0x002CBB68 File Offset: 0x002CAB68
		private bool IsLiveGroupingSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isLiveGroupingSet;
			}
			set
			{
				this._shapingStorage._isLiveGroupingSet = value;
			}
		}

		// Token: 0x17001953 RID: 6483
		// (get) Token: 0x06006D41 RID: 27969 RVA: 0x002CBB76 File Offset: 0x002CAB76
		private MonitorWrapper LiveGroupingMonitor
		{
			get
			{
				if (this._shapingStorage._liveGroupingMonitor == null)
				{
					this._shapingStorage._liveGroupingMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._liveGroupingMonitor;
			}
		}

		// Token: 0x04003611 RID: 13841
		private InnerItemCollectionView _internalView;

		// Token: 0x04003612 RID: 13842
		private IEnumerable _itemsSource;

		// Token: 0x04003613 RID: 13843
		private CollectionView _collectionView;

		// Token: 0x04003614 RID: 13844
		private int _defaultCapacity = 16;

		// Token: 0x04003615 RID: 13845
		private bool _isUsingItemsSource;

		// Token: 0x04003616 RID: 13846
		private bool _isInitializing;

		// Token: 0x04003617 RID: 13847
		private int _deferLevel;

		// Token: 0x04003618 RID: 13848
		private IDisposable _deferInnerRefresh;

		// Token: 0x04003619 RID: 13849
		private ItemCollection.ShapingStorage _shapingStorage;

		// Token: 0x0400361A RID: 13850
		private WeakReference _modelParent;

		// Token: 0x02000BFA RID: 3066
		private class ShapingStorage
		{
			// Token: 0x04004A96 RID: 19094
			public bool _isSortingSet;

			// Token: 0x04004A97 RID: 19095
			public bool _isGroupingSet;

			// Token: 0x04004A98 RID: 19096
			public bool _isLiveSortingSet;

			// Token: 0x04004A99 RID: 19097
			public bool _isLiveFilteringSet;

			// Token: 0x04004A9A RID: 19098
			public bool _isLiveGroupingSet;

			// Token: 0x04004A9B RID: 19099
			public SortDescriptionCollection _sort;

			// Token: 0x04004A9C RID: 19100
			public Predicate<object> _filter;

			// Token: 0x04004A9D RID: 19101
			public ObservableCollection<GroupDescription> _groupBy;

			// Token: 0x04004A9E RID: 19102
			public bool? _isLiveSorting;

			// Token: 0x04004A9F RID: 19103
			public bool? _isLiveFiltering;

			// Token: 0x04004AA0 RID: 19104
			public bool? _isLiveGrouping;

			// Token: 0x04004AA1 RID: 19105
			public ObservableCollection<string> _liveSortingProperties;

			// Token: 0x04004AA2 RID: 19106
			public ObservableCollection<string> _liveFilteringProperties;

			// Token: 0x04004AA3 RID: 19107
			public ObservableCollection<string> _liveGroupingProperties;

			// Token: 0x04004AA4 RID: 19108
			public MonitorWrapper _sortDescriptionsMonitor;

			// Token: 0x04004AA5 RID: 19109
			public MonitorWrapper _groupDescriptionsMonitor;

			// Token: 0x04004AA6 RID: 19110
			public MonitorWrapper _liveSortingMonitor;

			// Token: 0x04004AA7 RID: 19111
			public MonitorWrapper _liveFilteringMonitor;

			// Token: 0x04004AA8 RID: 19112
			public MonitorWrapper _liveGroupingMonitor;
		}

		// Token: 0x02000BFB RID: 3067
		private class DeferHelper : IDisposable
		{
			// Token: 0x06008FFB RID: 36859 RVA: 0x0034591A File Offset: 0x0034491A
			public DeferHelper(ItemCollection itemCollection)
			{
				this._itemCollection = itemCollection;
			}

			// Token: 0x06008FFC RID: 36860 RVA: 0x00345929 File Offset: 0x00344929
			public void Dispose()
			{
				if (this._itemCollection != null)
				{
					this._itemCollection.EndDefer();
					this._itemCollection = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004AA9 RID: 19113
			private ItemCollection _itemCollection;
		}
	}
}
