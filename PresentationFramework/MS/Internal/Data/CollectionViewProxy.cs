using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200020E RID: 526
	internal class CollectionViewProxy : CollectionView, IEditableCollectionViewAddNewItem, IEditableCollectionView, ICollectionViewLiveShaping, IItemProperties
	{
		// Token: 0x0600138E RID: 5006 RVA: 0x0014E460 File Offset: 0x0014D460
		internal CollectionViewProxy(ICollectionView view) : base(view.SourceCollection, false)
		{
			this._view = view;
			view.CollectionChanged += this._OnViewChanged;
			view.CurrentChanging += this._OnCurrentChanging;
			view.CurrentChanged += this._OnCurrentChanged;
			INotifyPropertyChanged notifyPropertyChanged = view as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged += this._OnPropertyChanged;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x0014E4D3 File Offset: 0x0014D4D3
		// (set) Token: 0x06001390 RID: 5008 RVA: 0x0014E4E0 File Offset: 0x0014D4E0
		public override CultureInfo Culture
		{
			get
			{
				return this.ProxiedView.Culture;
			}
			set
			{
				this.ProxiedView.Culture = value;
			}
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0014E4EE File Offset: 0x0014D4EE
		public override bool Contains(object item)
		{
			return this.ProxiedView.Contains(item);
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x0014E4FC File Offset: 0x0014D4FC
		public override IEnumerable SourceCollection
		{
			get
			{
				return base.SourceCollection;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x0014E504 File Offset: 0x0014D504
		// (set) Token: 0x06001394 RID: 5012 RVA: 0x0014E511 File Offset: 0x0014D511
		public override Predicate<object> Filter
		{
			get
			{
				return this.ProxiedView.Filter;
			}
			set
			{
				this.ProxiedView.Filter = value;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x0014E51F File Offset: 0x0014D51F
		public override bool CanFilter
		{
			get
			{
				return this.ProxiedView.CanFilter;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x0014E52C File Offset: 0x0014D52C
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this.ProxiedView.SortDescriptions;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001397 RID: 5015 RVA: 0x0014E539 File Offset: 0x0014D539
		public override bool CanSort
		{
			get
			{
				return this.ProxiedView.CanSort;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x0014E546 File Offset: 0x0014D546
		public override bool CanGroup
		{
			get
			{
				return this.ProxiedView.CanGroup;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x0014E553 File Offset: 0x0014D553
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this.ProxiedView.GroupDescriptions;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x0014E560 File Offset: 0x0014D560
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				return this.ProxiedView.Groups;
			}
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0014E570 File Offset: 0x0014D570
		public override void Refresh()
		{
			IndexedEnumerable indexedEnumerable = Interlocked.Exchange<IndexedEnumerable>(ref this._indexer, null);
			if (indexedEnumerable != null)
			{
				indexedEnumerable.Invalidate();
			}
			this.ProxiedView.Refresh();
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0014E59E File Offset: 0x0014D59E
		public override IDisposable DeferRefresh()
		{
			return this.ProxiedView.DeferRefresh();
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x0600139D RID: 5021 RVA: 0x0014E5AB File Offset: 0x0014D5AB
		public override object CurrentItem
		{
			get
			{
				return this.ProxiedView.CurrentItem;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x0014E5B8 File Offset: 0x0014D5B8
		public override int CurrentPosition
		{
			get
			{
				return this.ProxiedView.CurrentPosition;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x0014E5C5 File Offset: 0x0014D5C5
		public override bool IsCurrentAfterLast
		{
			get
			{
				return this.ProxiedView.IsCurrentAfterLast;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x0014E5D2 File Offset: 0x0014D5D2
		public override bool IsCurrentBeforeFirst
		{
			get
			{
				return this.ProxiedView.IsCurrentBeforeFirst;
			}
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0014E5DF File Offset: 0x0014D5DF
		public override bool MoveCurrentToFirst()
		{
			return this.ProxiedView.MoveCurrentToFirst();
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0014E5EC File Offset: 0x0014D5EC
		public override bool MoveCurrentToPrevious()
		{
			return this.ProxiedView.MoveCurrentToPrevious();
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0014E5F9 File Offset: 0x0014D5F9
		public override bool MoveCurrentToNext()
		{
			return this.ProxiedView.MoveCurrentToNext();
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0014E606 File Offset: 0x0014D606
		public override bool MoveCurrentToLast()
		{
			return this.ProxiedView.MoveCurrentToLast();
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0014E613 File Offset: 0x0014D613
		public override bool MoveCurrentTo(object item)
		{
			return this.ProxiedView.MoveCurrentTo(item);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0014E621 File Offset: 0x0014D621
		public override bool MoveCurrentToPosition(int position)
		{
			return this.ProxiedView.MoveCurrentToPosition(position);
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060013A7 RID: 5031 RVA: 0x0014E62F File Offset: 0x0014D62F
		// (remove) Token: 0x060013A8 RID: 5032 RVA: 0x0014E638 File Offset: 0x0014D638
		public override event CurrentChangingEventHandler CurrentChanging
		{
			add
			{
				this.PrivateCurrentChanging += value;
			}
			remove
			{
				this.PrivateCurrentChanging -= value;
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060013A9 RID: 5033 RVA: 0x0014E641 File Offset: 0x0014D641
		// (remove) Token: 0x060013AA RID: 5034 RVA: 0x0014E64A File Offset: 0x0014D64A
		public override event EventHandler CurrentChanged
		{
			add
			{
				this.PrivateCurrentChanged += value;
			}
			remove
			{
				this.PrivateCurrentChanged -= value;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x0014E653 File Offset: 0x0014D653
		public override int Count
		{
			get
			{
				return this.EnumerableWrapper.Count;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x0014E660 File Offset: 0x0014D660
		public override bool IsEmpty
		{
			get
			{
				return this.ProxiedView.IsEmpty;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x0014E66D File Offset: 0x0014D66D
		public ICollectionView ProxiedView
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0014E675 File Offset: 0x0014D675
		public override int IndexOf(object item)
		{
			return this.EnumerableWrapper.IndexOf(item);
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0014E683 File Offset: 0x0014D683
		public override bool PassesFilter(object item)
		{
			return !this.ProxiedView.CanFilter || this.ProxiedView.Filter == null || item == CollectionView.NewItemPlaceholder || item == ((IEditableCollectionView)this).CurrentAddItem || this.ProxiedView.Filter(item);
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0014E6C3 File Offset: 0x0014D6C3
		public override object GetItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.EnumerableWrapper[index];
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x0014E6E0 File Offset: 0x0014D6E0
		public override void DetachFromSourceCollection()
		{
			if (this._view != null)
			{
				this._view.CollectionChanged -= this._OnViewChanged;
				this._view.CurrentChanging -= this._OnCurrentChanging;
				this._view.CurrentChanged -= this._OnCurrentChanged;
				INotifyPropertyChanged notifyPropertyChanged = this._view as INotifyPropertyChanged;
				if (notifyPropertyChanged != null)
				{
					notifyPropertyChanged.PropertyChanged -= this._OnPropertyChanged;
				}
				this._view = null;
			}
			base.DetachFromSourceCollection();
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x0014E768 File Offset: 0x0014D768
		// (set) Token: 0x060013B3 RID: 5043 RVA: 0x0014E78C File Offset: 0x0014D78C
		NewItemPlaceholderPosition IEditableCollectionView.NewItemPlaceholderPosition
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.NewItemPlaceholderPosition;
				}
				return NewItemPlaceholderPosition.None;
			}
			set
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x0014E7D0 File Offset: 0x0014D7D0
		bool IEditableCollectionView.CanAddNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanAddNew;
			}
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0014E7F4 File Offset: 0x0014D7F4
		object IEditableCollectionView.AddNew()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				return editableCollectionView.AddNew();
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNew"
			}));
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0014E834 File Offset: 0x0014D834
		void IEditableCollectionView.CommitNew()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x060013B7 RID: 5047 RVA: 0x0014E874 File Offset: 0x0014D874
		void IEditableCollectionView.CancelNew()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0014E8B4 File Offset: 0x0014D8B4
		bool IEditableCollectionView.IsAddingNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsAddingNew;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x0014E8D8 File Offset: 0x0014D8D8
		object IEditableCollectionView.CurrentAddItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentAddItem;
				}
				return null;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x0014E8FC File Offset: 0x0014D8FC
		bool IEditableCollectionView.CanRemove
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanRemove;
			}
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0014E920 File Offset: 0x0014D920
		void IEditableCollectionView.RemoveAt(int index)
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x060013BC RID: 5052 RVA: 0x0014E964 File Offset: 0x0014D964
		void IEditableCollectionView.Remove(object item)
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x060013BD RID: 5053 RVA: 0x0014E9A8 File Offset: 0x0014D9A8
		void IEditableCollectionView.EditItem(object item)
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x060013BE RID: 5054 RVA: 0x0014E9EC File Offset: 0x0014D9EC
		void IEditableCollectionView.CommitEdit()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x060013BF RID: 5055 RVA: 0x0014EA2C File Offset: 0x0014DA2C
		void IEditableCollectionView.CancelEdit()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0014EA6C File Offset: 0x0014DA6C
		bool IEditableCollectionView.CanCancelEdit
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanCancelEdit;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x0014EA90 File Offset: 0x0014DA90
		bool IEditableCollectionView.IsEditingItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsEditingItem;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x0014EAB4 File Offset: 0x0014DAB4
		object IEditableCollectionView.CurrentEditItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentEditItem;
				}
				return null;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x0014EAD8 File Offset: 0x0014DAD8
		bool IEditableCollectionViewAddNewItem.CanAddNewItem
		{
			get
			{
				IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this.ProxiedView as IEditableCollectionViewAddNewItem;
				return editableCollectionViewAddNewItem != null && editableCollectionViewAddNewItem.CanAddNewItem;
			}
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0014EAFC File Offset: 0x0014DAFC
		object IEditableCollectionViewAddNewItem.AddNewItem(object newItem)
		{
			IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this.ProxiedView as IEditableCollectionViewAddNewItem;
			if (editableCollectionViewAddNewItem != null)
			{
				return editableCollectionViewAddNewItem.AddNewItem(newItem);
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNewItem"
			}));
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x0014EB40 File Offset: 0x0014DB40
		bool ICollectionViewLiveShaping.CanChangeLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveSorting;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x0014EB64 File Offset: 0x0014DB64
		bool ICollectionViewLiveShaping.CanChangeLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveFiltering;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x0014EB88 File Offset: 0x0014DB88
		bool ICollectionViewLiveShaping.CanChangeLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveGrouping;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x0014EBAC File Offset: 0x0014DBAC
		// (set) Token: 0x060013C9 RID: 5065 RVA: 0x0014EBD8 File Offset: 0x0014DBD8
		bool? ICollectionViewLiveShaping.IsLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveSorting;
			}
			set
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					collectionViewLiveShaping.IsLiveSorting = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveSorting",
					"CanChangeLiveSorting"
				}));
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x0014EC24 File Offset: 0x0014DC24
		// (set) Token: 0x060013CB RID: 5067 RVA: 0x0014EC50 File Offset: 0x0014DC50
		bool? ICollectionViewLiveShaping.IsLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveFiltering;
			}
			set
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					collectionViewLiveShaping.IsLiveFiltering = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveFiltering",
					"CanChangeLiveFiltering"
				}));
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x0014EC9C File Offset: 0x0014DC9C
		// (set) Token: 0x060013CD RID: 5069 RVA: 0x0014ECC8 File Offset: 0x0014DCC8
		bool? ICollectionViewLiveShaping.IsLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveGrouping;
			}
			set
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					collectionViewLiveShaping.IsLiveGrouping = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveGrouping",
					"CanChangeLiveGrouping"
				}));
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x0014ED14 File Offset: 0x0014DD14
		ObservableCollection<string> ICollectionViewLiveShaping.LiveSortingProperties
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					return collectionViewLiveShaping.LiveSortingProperties;
				}
				if (this._liveSortingProperties == null)
				{
					this._liveSortingProperties = new ObservableCollection<string>();
				}
				return this._liveSortingProperties;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x0014ED50 File Offset: 0x0014DD50
		ObservableCollection<string> ICollectionViewLiveShaping.LiveFilteringProperties
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					return collectionViewLiveShaping.LiveFilteringProperties;
				}
				if (this._liveFilteringProperties == null)
				{
					this._liveFilteringProperties = new ObservableCollection<string>();
				}
				return this._liveFilteringProperties;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x0014ED8C File Offset: 0x0014DD8C
		ObservableCollection<string> ICollectionViewLiveShaping.LiveGroupingProperties
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					return collectionViewLiveShaping.LiveGroupingProperties;
				}
				if (this._liveGroupingProperties == null)
				{
					this._liveGroupingProperties = new ObservableCollection<string>();
				}
				return this._liveGroupingProperties;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x0014EDC8 File Offset: 0x0014DDC8
		ReadOnlyCollection<ItemPropertyInfo> IItemProperties.ItemProperties
		{
			get
			{
				IItemProperties itemProperties = this.ProxiedView as IItemProperties;
				if (itemProperties != null)
				{
					return itemProperties.ItemProperties;
				}
				return null;
			}
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0014EDEC File Offset: 0x0014DDEC
		protected override IEnumerator GetEnumerator()
		{
			return this.ProxiedView.GetEnumerator();
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0014EDFC File Offset: 0x0014DDFC
		internal override void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			if (this._view != null)
			{
				format(level + 1, this._view, new bool?(true), sources);
				object sourceCollection = this._view.SourceCollection;
				if (sourceCollection != null)
				{
					format(level + 2, sourceCollection, null, sources);
				}
			}
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0014EE59 File Offset: 0x0014DE59
		private void _OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			this.OnPropertyChanged(args);
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0014EE62 File Offset: 0x0014DE62
		private void _OnViewChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			this.OnCollectionChanged(args);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0014EE6B File Offset: 0x0014DE6B
		private void _OnCurrentChanging(object sender, CurrentChangingEventArgs args)
		{
			if (this.PrivateCurrentChanging != null)
			{
				this.PrivateCurrentChanging(this, args);
			}
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0014EE82 File Offset: 0x0014DE82
		private void _OnCurrentChanged(object sender, EventArgs args)
		{
			if (this.PrivateCurrentChanged != null)
			{
				this.PrivateCurrentChanged(this, args);
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x0014EE9C File Offset: 0x0014DE9C
		private IndexedEnumerable EnumerableWrapper
		{
			get
			{
				if (this._indexer == null)
				{
					IndexedEnumerable value = new IndexedEnumerable(this.ProxiedView, new Predicate<object>(this.PassesFilter));
					Interlocked.CompareExchange<IndexedEnumerable>(ref this._indexer, value, null);
				}
				return this._indexer;
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060013D9 RID: 5081 RVA: 0x0014EEE0 File Offset: 0x0014DEE0
		// (remove) Token: 0x060013DA RID: 5082 RVA: 0x0014EF18 File Offset: 0x0014DF18
		private event CurrentChangingEventHandler PrivateCurrentChanging;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060013DB RID: 5083 RVA: 0x0014EF50 File Offset: 0x0014DF50
		// (remove) Token: 0x060013DC RID: 5084 RVA: 0x0014EF88 File Offset: 0x0014DF88
		private event EventHandler PrivateCurrentChanged;

		// Token: 0x04000B8B RID: 2955
		private ICollectionView _view;

		// Token: 0x04000B8C RID: 2956
		private IndexedEnumerable _indexer;

		// Token: 0x04000B8F RID: 2959
		private ObservableCollection<string> _liveSortingProperties;

		// Token: 0x04000B90 RID: 2960
		private ObservableCollection<string> _liveFilteringProperties;

		// Token: 0x04000B91 RID: 2961
		private ObservableCollection<string> _liveGroupingProperties;
	}
}
