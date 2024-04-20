using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Hashing.PresentationFramework;

namespace System.Windows.Data
{
	// Token: 0x02000455 RID: 1109
	public class CollectionView : DispatcherObject, ICollectionView, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		// Token: 0x060037E3 RID: 14307 RVA: 0x001E7205 File Offset: 0x001E6205
		public CollectionView(IEnumerable collection) : this(collection, 0)
		{
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x001E7210 File Offset: 0x001E6210
		internal CollectionView(IEnumerable collection, int moveToFirst)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (base.GetType() == typeof(CollectionView) && TraceData.IsEnabled)
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.CollectionViewIsUnsupported, null);
			}
			this._engine = DataBindEngine.CurrentDataBindEngine;
			if (!this._engine.IsShutDown)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.AllowsCrossThreadChanges, this._engine.ViewManager.GetSynchronizationInfo(collection).IsSynchronized);
			}
			else
			{
				moveToFirst = -1;
			}
			this._sourceCollection = collection;
			INotifyCollectionChanged notifyCollectionChanged = collection as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				IBindingList bindingList;
				if (!(this is BindingListCollectionView) || ((bindingList = (collection as IBindingList)) != null && !bindingList.SupportsChangeNotification))
				{
					notifyCollectionChanged.CollectionChanged += this.OnCollectionChanged;
				}
				this.SetFlag(CollectionView.CollectionViewFlags.IsDynamic, true);
			}
			object currentItem = null;
			int currentPosition = -1;
			if (moveToFirst >= 0)
			{
				BindingOperations.AccessCollection(collection, delegate
				{
					IEnumerator enumerator = collection.GetEnumerator();
					if (enumerator.MoveNext())
					{
						currentItem = enumerator.Current;
						currentPosition = 0;
					}
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}, false);
			}
			this._currentItem = currentItem;
			this._currentPosition = currentPosition;
			this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, this._currentPosition < 0);
			this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, this._currentPosition < 0);
			this.SetFlag(CollectionView.CollectionViewFlags.CachedIsEmpty, this._currentPosition < 0);
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x001E73B6 File Offset: 0x001E63B6
		internal CollectionView(IEnumerable collection, bool shouldProcessCollectionChanged) : this(collection)
		{
			this.SetFlag(CollectionView.CollectionViewFlags.ShouldProcessCollectionChanged, shouldProcessCollectionChanged);
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x060037E6 RID: 14310 RVA: 0x001E73C7 File Offset: 0x001E63C7
		// (set) Token: 0x060037E7 RID: 14311 RVA: 0x001E73CF File Offset: 0x001E63CF
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public virtual CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._culture != value)
				{
					this._culture = value;
					this.OnPropertyChanged("Culture");
				}
			}
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x001E73FA File Offset: 0x001E63FA
		public virtual bool Contains(object item)
		{
			this.VerifyRefreshNotDeferred();
			return this.IndexOf(item) >= 0;
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x060037E9 RID: 14313 RVA: 0x001E740F File Offset: 0x001E640F
		public virtual IEnumerable SourceCollection
		{
			get
			{
				return this._sourceCollection;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x060037EA RID: 14314 RVA: 0x001E7417 File Offset: 0x001E6417
		// (set) Token: 0x060037EB RID: 14315 RVA: 0x001E741F File Offset: 0x001E641F
		public virtual Predicate<object> Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				if (!this.CanFilter)
				{
					throw new NotSupportedException();
				}
				this._filter = value;
				this.RefreshOrDefer();
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x060037EC RID: 14316 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public virtual bool CanFilter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x060037ED RID: 14317 RVA: 0x001E743C File Offset: 0x001E643C
		public virtual SortDescriptionCollection SortDescriptions
		{
			get
			{
				return SortDescriptionCollection.Empty;
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x060037EE RID: 14318 RVA: 0x00105F35 File Offset: 0x00104F35
		public virtual bool CanSort
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x060037EF RID: 14319 RVA: 0x00105F35 File Offset: 0x00104F35
		public virtual bool CanGroup
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x060037F0 RID: 14320 RVA: 0x00109403 File Offset: 0x00108403
		public virtual ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x060037F1 RID: 14321 RVA: 0x00109403 File Offset: 0x00108403
		public virtual ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x001E7444 File Offset: 0x001E6444
		public virtual void Refresh()
		{
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && (editableCollectionView.IsAddingNew || editableCollectionView.IsEditingItem))
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"Refresh"
				}));
			}
			this.RefreshInternal();
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x001E748F File Offset: 0x001E648F
		internal void RefreshInternal()
		{
			if (this.AllowsCrossThreadChanges)
			{
				base.VerifyAccess();
			}
			this.RefreshOverride();
			this.SetFlag(CollectionView.CollectionViewFlags.NeedsRefresh, false);
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x001E74B4 File Offset: 0x001E64B4
		public virtual IDisposable DeferRefresh()
		{
			if (this.AllowsCrossThreadChanges)
			{
				base.VerifyAccess();
			}
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && (editableCollectionView.IsAddingNew || editableCollectionView.IsEditingItem))
			{
				throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[]
				{
					"DeferRefresh"
				}));
			}
			this._deferLevel++;
			return new CollectionView.DeferHelper(this);
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x001E751B File Offset: 0x001E651B
		public virtual object CurrentItem
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this._currentItem;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x060037F6 RID: 14326 RVA: 0x001E7529 File Offset: 0x001E6529
		public virtual int CurrentPosition
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this._currentPosition;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x060037F7 RID: 14327 RVA: 0x001E7537 File Offset: 0x001E6537
		public virtual bool IsCurrentAfterLast
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast);
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x060037F8 RID: 14328 RVA: 0x001E7547 File Offset: 0x001E6547
		public virtual bool IsCurrentBeforeFirst
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst);
			}
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x001E7558 File Offset: 0x001E6558
		public virtual bool MoveCurrentToFirst()
		{
			this.VerifyRefreshNotDeferred();
			int position = 0;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				position = 1;
			}
			return this.MoveCurrentToPosition(position);
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x001E758C File Offset: 0x001E658C
		public virtual bool MoveCurrentToLast()
		{
			this.VerifyRefreshNotDeferred();
			int num = this.Count - 1;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				num--;
			}
			return this.MoveCurrentToPosition(num);
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x001E75C8 File Offset: 0x001E65C8
		public virtual bool MoveCurrentToNext()
		{
			this.VerifyRefreshNotDeferred();
			int num = this.CurrentPosition + 1;
			int count = this.Count;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && num == 0 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				num = 1;
			}
			if (editableCollectionView != null && num == count - 1 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				num = count;
			}
			return num <= count && this.MoveCurrentToPosition(num);
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x001E7624 File Offset: 0x001E6624
		public virtual bool MoveCurrentToPrevious()
		{
			this.VerifyRefreshNotDeferred();
			int num = this.CurrentPosition - 1;
			int count = this.Count;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && num == count - 1 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				num = count - 2;
			}
			if (editableCollectionView != null && num == 0 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
			{
				num = -1;
			}
			return num >= -1 && this.MoveCurrentToPosition(num);
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x001E7684 File Offset: 0x001E6684
		public virtual bool MoveCurrentTo(object item)
		{
			this.VerifyRefreshNotDeferred();
			if ((ItemsControl.EqualsEx(this.CurrentItem, item) || ItemsControl.EqualsEx(CollectionView.NewItemPlaceholder, item)) && (item != null || this.IsCurrentInView))
			{
				return this.IsCurrentInView;
			}
			int position = -1;
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if ((editableCollectionView != null && editableCollectionView.IsAddingNew && ItemsControl.EqualsEx(item, editableCollectionView.CurrentAddItem)) || this.PassesFilter(item))
			{
				position = this.IndexOf(item);
			}
			return this.MoveCurrentToPosition(position);
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x001E7704 File Offset: 0x001E6704
		public virtual bool MoveCurrentToPosition(int position)
		{
			this.VerifyRefreshNotDeferred();
			if (position < -1 || position > this.Count)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			IEditableCollectionView editableCollectionView = this as IEditableCollectionView;
			if (editableCollectionView != null && ((position == 0 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) || (position == this.Count - 1 && editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)))
			{
				return this.IsCurrentInView;
			}
			if ((position != this.CurrentPosition || !this.IsCurrentInSync) && this.OKToChangeCurrent())
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

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x060037FF RID: 14335 RVA: 0x001E77D8 File Offset: 0x001E67D8
		// (remove) Token: 0x06003800 RID: 14336 RVA: 0x001E7810 File Offset: 0x001E6810
		public virtual event CurrentChangingEventHandler CurrentChanging;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06003801 RID: 14337 RVA: 0x001E7848 File Offset: 0x001E6848
		// (remove) Token: 0x06003802 RID: 14338 RVA: 0x001E7880 File Offset: 0x001E6880
		public virtual event EventHandler CurrentChanged;

		// Token: 0x06003803 RID: 14339 RVA: 0x001E78B5 File Offset: 0x001E68B5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x001E78BD File Offset: 0x001E68BD
		public virtual bool PassesFilter(object item)
		{
			return !this.CanFilter || this.Filter == null || this.Filter(item);
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x001E78DD File Offset: 0x001E68DD
		public virtual int IndexOf(object item)
		{
			this.VerifyRefreshNotDeferred();
			return this.EnumerableWrapper.IndexOf(item);
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x001E78F1 File Offset: 0x001E68F1
		public virtual object GetItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.EnumerableWrapper[index];
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x001E7910 File Offset: 0x001E6910
		public virtual void DetachFromSourceCollection()
		{
			INotifyCollectionChanged notifyCollectionChanged = this._sourceCollection as INotifyCollectionChanged;
			IBindingList bindingList;
			if (notifyCollectionChanged != null && (!(this is BindingListCollectionView) || ((bindingList = (this._sourceCollection as IBindingList)) != null && !bindingList.SupportsChangeNotification)))
			{
				notifyCollectionChanged.CollectionChanged -= this.OnCollectionChanged;
			}
			this._sourceCollection = null;
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06003808 RID: 14344 RVA: 0x001E7964 File Offset: 0x001E6964
		public virtual int Count
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return this.EnumerableWrapper.Count;
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06003809 RID: 14345 RVA: 0x001E7977 File Offset: 0x001E6977
		public virtual bool IsEmpty
		{
			get
			{
				return this.EnumerableWrapper.IsEmpty;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x0600380A RID: 14346 RVA: 0x001E7984 File Offset: 0x001E6984
		public virtual IComparer Comparer
		{
			get
			{
				return this as IComparer;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x0600380B RID: 14347 RVA: 0x001E798C File Offset: 0x001E698C
		public virtual bool NeedsRefresh
		{
			get
			{
				return this.CheckFlag(CollectionView.CollectionViewFlags.NeedsRefresh);
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600380C RID: 14348 RVA: 0x001E7999 File Offset: 0x001E6999
		public virtual bool IsInUse
		{
			get
			{
				return this.CollectionChanged != null || this.PropertyChanged != null || this.CurrentChanged != null || this.CurrentChanging != null;
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600380D RID: 14349 RVA: 0x001E79BE File Offset: 0x001E69BE
		public static object NewItemPlaceholder
		{
			get
			{
				return CollectionView._newItemPlaceholder;
			}
		}

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x0600380E RID: 14350 RVA: 0x001E79C8 File Offset: 0x001E69C8
		// (remove) Token: 0x0600380F RID: 14351 RVA: 0x001E7A00 File Offset: 0x001E6A00
		protected virtual event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06003810 RID: 14352 RVA: 0x001E7A35 File Offset: 0x001E6A35
		// (remove) Token: 0x06003811 RID: 14353 RVA: 0x001E7A3E File Offset: 0x001E6A3E
		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			add
			{
				this.CollectionChanged += value;
			}
			remove
			{
				this.CollectionChanged -= value;
			}
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06003812 RID: 14354 RVA: 0x001E7A47 File Offset: 0x001E6A47
		// (remove) Token: 0x06003813 RID: 14355 RVA: 0x001E7A50 File Offset: 0x001E6A50
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this.PropertyChanged += value;
			}
			remove
			{
				this.PropertyChanged -= value;
			}
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x001E7A59 File Offset: 0x001E6A59
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06003815 RID: 14357 RVA: 0x001E7A70 File Offset: 0x001E6A70
		// (remove) Token: 0x06003816 RID: 14358 RVA: 0x001E7AA8 File Offset: 0x001E6AA8
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06003817 RID: 14359 RVA: 0x001E7AE0 File Offset: 0x001E6AE0
		protected virtual void RefreshOverride()
		{
			if (this.SortDescriptions.Count > 0)
			{
				throw new InvalidOperationException(SR.Get("ImplementOtherMembersWithSort", new object[]
				{
					"Refresh()"
				}));
			}
			object currentItem = this._currentItem;
			bool flag = this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast);
			bool flag2 = this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst);
			int currentPosition = this._currentPosition;
			this.OnCurrentChanging();
			this.InvalidateEnumerableWrapper();
			if (this.IsEmpty || flag2)
			{
				this._MoveCurrentToPosition(-1);
			}
			else if (flag)
			{
				this._MoveCurrentToPosition(this.Count);
			}
			else if (currentItem != null)
			{
				int num = this.EnumerableWrapper.IndexOf(currentItem);
				if (num < 0)
				{
					num = 0;
				}
				this._MoveCurrentToPosition(num);
			}
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			this.OnCurrentChanged();
			if (this.IsCurrentAfterLast != flag)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst != flag2)
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

		// Token: 0x06003818 RID: 14360 RVA: 0x001E7BE5 File Offset: 0x001E6BE5
		protected virtual IEnumerator GetEnumerator()
		{
			this.VerifyRefreshNotDeferred();
			if (this.SortDescriptions.Count > 0)
			{
				throw new InvalidOperationException(SR.Get("ImplementOtherMembersWithSort", new object[]
				{
					"GetEnumerator()"
				}));
			}
			return this.EnumerableWrapper.GetEnumerator();
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x001E7C24 File Offset: 0x001E6C24
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			this._timestamp++;
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
			if (args.Action != NotifyCollectionChangedAction.Replace && args.Action != NotifyCollectionChangedAction.Move)
			{
				this.OnPropertyChanged("Count");
			}
			bool isEmpty = this.IsEmpty;
			if (isEmpty != this.CheckFlag(CollectionView.CollectionViewFlags.CachedIsEmpty))
			{
				this.SetFlag(CollectionView.CollectionViewFlags.CachedIsEmpty, isEmpty);
				this.OnPropertyChanged("IsEmpty");
			}
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x001E7CAC File Offset: 0x001E6CAC
		protected void SetCurrent(object newItem, int newPosition)
		{
			int count = (newItem != null) ? 0 : (this.IsEmpty ? 0 : this.Count);
			this.SetCurrent(newItem, newPosition, count);
		}

		// Token: 0x0600381B RID: 14363 RVA: 0x001E7CDC File Offset: 0x001E6CDC
		protected void SetCurrent(object newItem, int newPosition, int count)
		{
			if (newItem != null)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, false);
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, false);
			}
			else if (count == 0)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, true);
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, true);
				newPosition = -1;
			}
			else
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, newPosition < 0);
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, newPosition >= count);
			}
			this._currentItem = newItem;
			this._currentPosition = newPosition;
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x001E7D40 File Offset: 0x001E6D40
		protected bool OKToChangeCurrent()
		{
			CurrentChangingEventArgs currentChangingEventArgs = new CurrentChangingEventArgs();
			this.OnCurrentChanging(currentChangingEventArgs);
			return !currentChangingEventArgs.Cancel;
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x001E7D63 File Offset: 0x001E6D63
		protected void OnCurrentChanging()
		{
			this._currentPosition = -1;
			this.OnCurrentChanging(CollectionView.uncancelableCurrentChangingEventArgs);
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x001E7D78 File Offset: 0x001E6D78
		protected virtual void OnCurrentChanging(CurrentChangingEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (this._currentChangedMonitor.Busy)
			{
				if (args.IsCancelable)
				{
					args.Cancel = true;
				}
				return;
			}
			if (this.CurrentChanging != null)
			{
				this.CurrentChanging(this, args);
			}
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x001E7DC8 File Offset: 0x001E6DC8
		protected virtual void OnCurrentChanged()
		{
			if (this.CurrentChanged != null && this._currentChangedMonitor.Enter())
			{
				using (this._currentChangedMonitor)
				{
					this.CurrentChanged(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x001E7E20 File Offset: 0x001E6E20
		protected virtual void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			this.ValidateCollectionChangedEventArgs(args);
			object currentItem = this._currentItem;
			bool flag = this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast);
			bool flag2 = this.CheckFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst);
			int currentPosition = this._currentPosition;
			bool flag3 = false;
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (this.PassesFilter(args.NewItems[0]))
				{
					flag3 = true;
					this.AdjustCurrencyForAdd(args.NewStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (this.PassesFilter(args.OldItems[0]))
				{
					flag3 = true;
					this.AdjustCurrencyForRemove(args.OldStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (this.PassesFilter(args.OldItems[0]) || this.PassesFilter(args.NewItems[0]))
				{
					flag3 = true;
					this.AdjustCurrencyForReplace(args.OldStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (this.PassesFilter(args.NewItems[0]))
				{
					flag3 = true;
					this.AdjustCurrencyForMove(args.OldStartingIndex, args.NewStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				this.RefreshOrDefer();
				return;
			}
			if (flag3)
			{
				this.OnCollectionChanged(args);
			}
			if (this._currentElementWasRemovedOrReplaced)
			{
				this.MoveCurrencyOffDeletedElement();
				this._currentElementWasRemovedOrReplaced = false;
			}
			if (this.IsCurrentAfterLast != flag)
			{
				this.OnPropertyChanged("IsCurrentAfterLast");
			}
			if (this.IsCurrentBeforeFirst != flag2)
			{
				this.OnPropertyChanged("IsCurrentBeforeFirst");
			}
			if (this._currentPosition != currentPosition)
			{
				this.OnPropertyChanged("CurrentPosition");
			}
			if (this._currentItem != currentItem)
			{
				this.OnPropertyChanged("CurrentItem");
			}
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x001E7FA9 File Offset: 0x001E6FA9
		protected void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (this.CheckFlag(CollectionView.CollectionViewFlags.ShouldProcessCollectionChanged))
			{
				if (!this.AllowsCrossThreadChanges)
				{
					if (!base.CheckAccess())
					{
						throw new NotSupportedException(SR.Get("MultiThreadedCollectionChangeNotSupported"));
					}
					this.ProcessCollectionChanged(args);
					return;
				}
				else
				{
					this.PostChange(args);
				}
			}
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnAllowsCrossThreadChangesChanged()
		{
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x001E7FE4 File Offset: 0x001E6FE4
		protected void ClearPendingChanges()
		{
			object syncRoot = this._changeLog.SyncRoot;
			lock (syncRoot)
			{
				this._changeLog.Clear();
				this._tempChangeLog.Clear();
			}
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x001E803C File Offset: 0x001E703C
		protected void ProcessPendingChanges()
		{
			object syncRoot = this._changeLog.SyncRoot;
			lock (syncRoot)
			{
				this.ProcessChangeLog(this._changeLog, true);
				this._changeLog.Clear();
			}
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Obsolete("Replaced by OnAllowsCrossThreadChangesChanged")]
		protected virtual void OnBeginChangeLogging(NotifyCollectionChangedEventArgs args)
		{
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x001E8094 File Offset: 0x001E7094
		[Obsolete("Replaced by ClearPendingChanges")]
		protected void ClearChangeLog()
		{
			this.ClearPendingChanges();
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x001E809C File Offset: 0x001E709C
		protected void RefreshOrDefer()
		{
			if (this.IsRefreshDeferred)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.NeedsRefresh, true);
				return;
			}
			this.RefreshInternal();
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06003828 RID: 14376 RVA: 0x001E80B9 File Offset: 0x001E70B9
		protected bool IsDynamic
		{
			get
			{
				return this.CheckFlag(CollectionView.CollectionViewFlags.IsDynamic);
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06003829 RID: 14377 RVA: 0x001E80C3 File Offset: 0x001E70C3
		protected bool AllowsCrossThreadChanges
		{
			get
			{
				return this.CheckFlag(CollectionView.CollectionViewFlags.AllowsCrossThreadChanges);
			}
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x001E80D0 File Offset: 0x001E70D0
		internal void SetAllowsCrossThreadChanges(bool value)
		{
			if (this.CheckFlag(CollectionView.CollectionViewFlags.AllowsCrossThreadChanges) == value)
			{
				return;
			}
			this.SetFlag(CollectionView.CollectionViewFlags.AllowsCrossThreadChanges, value);
			this.OnAllowsCrossThreadChangesChanged();
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x0600382B RID: 14379 RVA: 0x001E80F3 File Offset: 0x001E70F3
		protected bool UpdatedOutsideDispatcher
		{
			get
			{
				return this.AllowsCrossThreadChanges;
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600382C RID: 14380 RVA: 0x001E80FB File Offset: 0x001E70FB
		protected bool IsRefreshDeferred
		{
			get
			{
				return this._deferLevel > 0;
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x0600382D RID: 14381 RVA: 0x001E8106 File Offset: 0x001E7106
		protected bool IsCurrentInSync
		{
			get
			{
				if (this.IsCurrentInView)
				{
					return this.GetItemAt(this.CurrentPosition) == this.CurrentItem;
				}
				return this.CurrentItem == null;
			}
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x001E8130 File Offset: 0x001E7130
		internal void SetViewManagerData(object value)
		{
			if (this._vmData == null)
			{
				this._vmData = value;
				return;
			}
			object[] array;
			if ((array = (this._vmData as object[])) == null)
			{
				this._vmData = new object[]
				{
					this._vmData,
					value
				};
				return;
			}
			object[] array2 = new object[array.Length + 1];
			array.CopyTo(array2, 0);
			array2[array.Length] = value;
			this._vmData = array2;
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x001E8196 File Offset: 0x001E7196
		internal virtual bool HasReliableHashCodes()
		{
			return this.IsEmpty || HashHelper.HasReliableHashCode(this.GetItemAt(0));
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x001E81AE File Offset: 0x001E71AE
		internal void VerifyRefreshNotDeferred()
		{
			if (this.AllowsCrossThreadChanges)
			{
				base.VerifyAccess();
			}
			if (this.IsRefreshDeferred)
			{
				throw new InvalidOperationException(SR.Get("NoCheckOrChangeWhenDeferred"));
			}
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x001E81D8 File Offset: 0x001E71D8
		internal void InvalidateEnumerableWrapper()
		{
			IndexedEnumerable indexedEnumerable = Interlocked.Exchange<IndexedEnumerable>(ref this._enumerableWrapper, null);
			if (indexedEnumerable != null)
			{
				indexedEnumerable.Invalidate();
			}
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x001E81FC File Offset: 0x001E71FC
		internal ReadOnlyCollection<ItemPropertyInfo> GetItemProperties()
		{
			IEnumerable sourceCollection = this.SourceCollection;
			if (sourceCollection == null)
			{
				return null;
			}
			IEnumerable enumerable = null;
			ITypedList typedList = sourceCollection as ITypedList;
			Type itemType;
			object representativeItem;
			if (typedList != null)
			{
				enumerable = typedList.GetItemProperties(null);
			}
			else if ((itemType = this.GetItemType(false)) != null)
			{
				enumerable = TypeDescriptor.GetProperties(itemType);
			}
			else if ((representativeItem = this.GetRepresentativeItem()) != null)
			{
				ICustomTypeProvider customTypeProvider = representativeItem as ICustomTypeProvider;
				if (customTypeProvider == null)
				{
					enumerable = TypeDescriptor.GetProperties(representativeItem);
				}
				else
				{
					enumerable = customTypeProvider.GetCustomType().GetProperties();
				}
			}
			if (enumerable == null)
			{
				return null;
			}
			List<ItemPropertyInfo> list = new List<ItemPropertyInfo>();
			foreach (object obj in enumerable)
			{
				PropertyDescriptor propertyDescriptor;
				PropertyInfo propertyInfo;
				if ((propertyDescriptor = (obj as PropertyDescriptor)) != null)
				{
					list.Add(new ItemPropertyInfo(propertyDescriptor.Name, propertyDescriptor.PropertyType, propertyDescriptor));
				}
				else if ((propertyInfo = (obj as PropertyInfo)) != null)
				{
					list.Add(new ItemPropertyInfo(propertyInfo.Name, propertyInfo.PropertyType, propertyInfo));
				}
			}
			return new ReadOnlyCollection<ItemPropertyInfo>(list);
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x001E8324 File Offset: 0x001E7324
		internal Type GetItemType(bool useRepresentativeItem)
		{
			foreach (Type type in this.SourceCollection.GetType().GetInterfaces())
			{
				if (type.Name == CollectionView.IEnumerableT)
				{
					Type[] genericArguments = type.GetGenericArguments();
					if (genericArguments.Length == 1)
					{
						Type type2 = genericArguments[0];
						if (typeof(ICustomTypeProvider).IsAssignableFrom(type2))
						{
							break;
						}
						if (!(type2 == typeof(object)))
						{
							return type2;
						}
					}
				}
			}
			if (useRepresentativeItem)
			{
				return ReflectionHelper.GetReflectionType(this.GetRepresentativeItem());
			}
			return null;
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x001E83B4 File Offset: 0x001E73B4
		internal object GetRepresentativeItem()
		{
			if (this.IsEmpty)
			{
				return null;
			}
			object result = null;
			IEnumerator enumerator = this.GetEnumerator();
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (obj != null && obj != CollectionView.NewItemPlaceholder)
				{
					result = obj;
					break;
				}
			}
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			return result;
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x001E8408 File Offset: 0x001E7408
		internal virtual void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, null, sources);
			if (this._sourceCollection != null)
			{
				format(level + 1, this._sourceCollection, null, sources);
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x001E8448 File Offset: 0x001E7448
		internal object SyncRoot
		{
			get
			{
				return this._syncObject;
			}
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06003837 RID: 14391 RVA: 0x001E8450 File Offset: 0x001E7450
		internal int Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06003838 RID: 14392 RVA: 0x001E8458 File Offset: 0x001E7458
		private bool IsCurrentInView
		{
			get
			{
				this.VerifyRefreshNotDeferred();
				return 0 <= this.CurrentPosition && this.CurrentPosition < this.Count;
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06003839 RID: 14393 RVA: 0x001E847C File Offset: 0x001E747C
		private IndexedEnumerable EnumerableWrapper
		{
			get
			{
				if (this._enumerableWrapper == null)
				{
					IndexedEnumerable value = new IndexedEnumerable(this.SourceCollection, new Predicate<object>(this.PassesFilter));
					Interlocked.CompareExchange<IndexedEnumerable>(ref this._enumerableWrapper, value, null);
				}
				return this._enumerableWrapper;
			}
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x001E84C0 File Offset: 0x001E74C0
		private void _MoveCurrentToPosition(int position)
		{
			if (position < 0)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst, true);
				this.SetCurrent(null, -1);
				return;
			}
			if (position >= this.Count)
			{
				this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentAfterLast, true);
				this.SetCurrent(null, this.Count);
				return;
			}
			this.SetFlag(CollectionView.CollectionViewFlags.IsCurrentBeforeFirst | CollectionView.CollectionViewFlags.IsCurrentAfterLast, false);
			this.SetCurrent(this.EnumerableWrapper[position], position);
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x001E8520 File Offset: 0x001E7520
		private void MoveCurrencyOffDeletedElement()
		{
			int num = this.Count - 1;
			int position = (this._currentPosition < num) ? this._currentPosition : num;
			this.OnCurrentChanging();
			this._MoveCurrentToPosition(position);
			this.OnCurrentChanged();
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x001E855C File Offset: 0x001E755C
		private void EndDefer()
		{
			this._deferLevel--;
			if (this._deferLevel == 0 && this.CheckFlag(CollectionView.CollectionViewFlags.NeedsRefresh))
			{
				this.Refresh();
			}
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x001E8588 File Offset: 0x001E7588
		private void DeferProcessing(ICollection changeLog)
		{
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				object syncRoot2 = this._changeLog.SyncRoot;
				lock (syncRoot2)
				{
					if (this._changeLog == null)
					{
						this._changeLog = new ArrayList(changeLog);
					}
					else
					{
						this._changeLog.InsertRange(0, changeLog);
					}
					if (this._databindOperation != null)
					{
						this._engine.ChangeCost(this._databindOperation, changeLog.Count);
					}
					else
					{
						this._databindOperation = this._engine.Marshal(new DispatcherOperationCallback(this.ProcessInvoke), null, changeLog.Count);
					}
				}
			}
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x001E8658 File Offset: 0x001E7658
		private ICollection ProcessChangeLog(ArrayList changeLog, bool processAll = false)
		{
			int num = 0;
			bool flag = false;
			long ticks = DateTime.Now.Ticks;
			int count = changeLog.Count;
			while (num < changeLog.Count && !flag)
			{
				NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = changeLog[num] as NotifyCollectionChangedEventArgs;
				if (notifyCollectionChangedEventArgs != null)
				{
					this.ProcessCollectionChanged(notifyCollectionChangedEventArgs);
				}
				if (!processAll)
				{
					flag = (DateTime.Now.Ticks - ticks > 50000L);
				}
				num++;
			}
			if (flag && num < changeLog.Count)
			{
				changeLog.RemoveRange(0, num);
				return changeLog;
			}
			return null;
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x001E86DC File Offset: 0x001E76DC
		private bool CheckFlag(CollectionView.CollectionViewFlags flags)
		{
			return (this._flags & flags) > (CollectionView.CollectionViewFlags)0;
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x001E86E9 File Offset: 0x001E76E9
		private void SetFlag(CollectionView.CollectionViewFlags flags, bool value)
		{
			if (value)
			{
				this._flags |= flags;
				return;
			}
			this._flags &= ~flags;
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x001E870C File Offset: 0x001E770C
		private void PostChange(NotifyCollectionChangedEventArgs args)
		{
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				object syncRoot2 = this._changeLog.SyncRoot;
				lock (syncRoot2)
				{
					if (args.Action == NotifyCollectionChangedAction.Reset)
					{
						this._changeLog.Clear();
					}
					if (this._changeLog.Count == 0 && base.CheckAccess())
					{
						this.ProcessCollectionChanged(args);
					}
					else
					{
						this._changeLog.Add(args);
						if (this._databindOperation == null)
						{
							this._databindOperation = this._engine.Marshal(new DispatcherOperationCallback(this.ProcessInvoke), null, this._changeLog.Count);
						}
					}
				}
			}
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x001E87E4 File Offset: 0x001E77E4
		private object ProcessInvoke(object arg)
		{
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				object syncRoot2 = this._changeLog.SyncRoot;
				lock (syncRoot2)
				{
					this._databindOperation = null;
					this._tempChangeLog = this._changeLog;
					this._changeLog = new ArrayList();
				}
			}
			ICollection collection = this.ProcessChangeLog(this._tempChangeLog, false);
			if (collection != null && collection.Count > 0)
			{
				this.DeferProcessing(collection);
			}
			this._tempChangeLog = CollectionView.EmptyArrayList;
			return null;
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x001E8898 File Offset: 0x001E7898
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
				if (e.OldStartingIndex < 0)
				{
					throw new InvalidOperationException(SR.Get("RemovedItemNotFound"));
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

		// Token: 0x06003844 RID: 14404 RVA: 0x001E89B0 File Offset: 0x001E79B0
		private void AdjustCurrencyForAdd(int index)
		{
			if (this.Count == 1)
			{
				this._currentPosition = -1;
				return;
			}
			if (index <= this._currentPosition)
			{
				this._currentPosition++;
				if (this._currentPosition < this.Count)
				{
					this._currentItem = this.EnumerableWrapper[this._currentPosition];
				}
			}
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x001E8A0A File Offset: 0x001E7A0A
		private void AdjustCurrencyForRemove(int index)
		{
			if (index < this._currentPosition)
			{
				this._currentPosition--;
				return;
			}
			if (index == this._currentPosition)
			{
				this._currentElementWasRemovedOrReplaced = true;
			}
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x001E8A34 File Offset: 0x001E7A34
		private void AdjustCurrencyForMove(int oldIndex, int newIndex)
		{
			if ((oldIndex < this.CurrentPosition && newIndex < this.CurrentPosition) || (oldIndex > this.CurrentPosition && newIndex > this.CurrentPosition))
			{
				return;
			}
			if (oldIndex <= this.CurrentPosition)
			{
				this.AdjustCurrencyForRemove(oldIndex);
				return;
			}
			if (newIndex <= this.CurrentPosition)
			{
				this.AdjustCurrencyForAdd(newIndex);
			}
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x001E8A87 File Offset: 0x001E7A87
		private void AdjustCurrencyForReplace(int index)
		{
			if (index == this._currentPosition)
			{
				this._currentElementWasRemovedOrReplaced = true;
			}
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x00150A1C File Offset: 0x0014FA1C
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x04001D04 RID: 7428
		private ArrayList _changeLog = new ArrayList();

		// Token: 0x04001D05 RID: 7429
		private ArrayList _tempChangeLog = CollectionView.EmptyArrayList;

		// Token: 0x04001D06 RID: 7430
		private DataBindOperation _databindOperation;

		// Token: 0x04001D07 RID: 7431
		private object _vmData;

		// Token: 0x04001D08 RID: 7432
		private IEnumerable _sourceCollection;

		// Token: 0x04001D09 RID: 7433
		private CultureInfo _culture;

		// Token: 0x04001D0A RID: 7434
		private CollectionView.SimpleMonitor _currentChangedMonitor = new CollectionView.SimpleMonitor();

		// Token: 0x04001D0B RID: 7435
		private int _deferLevel;

		// Token: 0x04001D0C RID: 7436
		private IndexedEnumerable _enumerableWrapper;

		// Token: 0x04001D0D RID: 7437
		private Predicate<object> _filter;

		// Token: 0x04001D0E RID: 7438
		private object _currentItem;

		// Token: 0x04001D0F RID: 7439
		private int _currentPosition;

		// Token: 0x04001D10 RID: 7440
		private CollectionView.CollectionViewFlags _flags = CollectionView.CollectionViewFlags.ShouldProcessCollectionChanged | CollectionView.CollectionViewFlags.NeedsRefresh;

		// Token: 0x04001D11 RID: 7441
		private bool _currentElementWasRemovedOrReplaced;

		// Token: 0x04001D12 RID: 7442
		private static object _newItemPlaceholder = new NamedObject("NewItemPlaceholder");

		// Token: 0x04001D13 RID: 7443
		private object _syncObject = new object();

		// Token: 0x04001D14 RID: 7444
		private DataBindEngine _engine;

		// Token: 0x04001D15 RID: 7445
		private int _timestamp;

		// Token: 0x04001D16 RID: 7446
		private static readonly ArrayList EmptyArrayList = new ArrayList();

		// Token: 0x04001D17 RID: 7447
		private static readonly string IEnumerableT = typeof(IEnumerable<>).Name;

		// Token: 0x04001D18 RID: 7448
		internal static readonly object NoNewItem = new NamedObject("NoNewItem");

		// Token: 0x04001D19 RID: 7449
		private static readonly CurrentChangingEventArgs uncancelableCurrentChangingEventArgs = new CurrentChangingEventArgs(false);

		// Token: 0x04001D1A RID: 7450
		internal const string CountPropertyName = "Count";

		// Token: 0x04001D1B RID: 7451
		internal const string IsEmptyPropertyName = "IsEmpty";

		// Token: 0x04001D1C RID: 7452
		internal const string CulturePropertyName = "Culture";

		// Token: 0x04001D1D RID: 7453
		internal const string CurrentPositionPropertyName = "CurrentPosition";

		// Token: 0x04001D1E RID: 7454
		internal const string CurrentItemPropertyName = "CurrentItem";

		// Token: 0x04001D1F RID: 7455
		internal const string IsCurrentBeforeFirstPropertyName = "IsCurrentBeforeFirst";

		// Token: 0x04001D20 RID: 7456
		internal const string IsCurrentAfterLastPropertyName = "IsCurrentAfterLast";

		// Token: 0x02000ADF RID: 2783
		internal class PlaceholderAwareEnumerator : IEnumerator
		{
			// Token: 0x06008B3B RID: 35643 RVA: 0x00339717 File Offset: 0x00338717
			public PlaceholderAwareEnumerator(CollectionView collectionView, IEnumerator baseEnumerator, NewItemPlaceholderPosition placeholderPosition, object newItem)
			{
				this._collectionView = collectionView;
				this._timestamp = collectionView.Timestamp;
				this._baseEnumerator = baseEnumerator;
				this._placeholderPosition = placeholderPosition;
				this._newItem = newItem;
			}

			// Token: 0x06008B3C RID: 35644 RVA: 0x00339748 File Offset: 0x00338748
			public bool MoveNext()
			{
				if (this._timestamp != this._collectionView.Timestamp)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				switch (this._position)
				{
				case CollectionView.PlaceholderAwareEnumerator.Position.BeforePlaceholder:
					if (this._placeholderPosition == NewItemPlaceholderPosition.AtBeginning)
					{
						this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder;
					}
					else if (!this._baseEnumerator.MoveNext() || (this._newItem != CollectionView.NoNewItem && this._baseEnumerator.Current == this._newItem && !this._baseEnumerator.MoveNext()))
					{
						if (this._newItem != CollectionView.NoNewItem)
						{
							this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnNewItem;
						}
						else
						{
							if (this._placeholderPosition == NewItemPlaceholderPosition.None)
							{
								return false;
							}
							this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder;
						}
					}
					return true;
				case CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder:
					if (this._newItem != CollectionView.NoNewItem && this._placeholderPosition == NewItemPlaceholderPosition.AtBeginning)
					{
						this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnNewItem;
						return true;
					}
					break;
				case CollectionView.PlaceholderAwareEnumerator.Position.OnNewItem:
					if (this._placeholderPosition == NewItemPlaceholderPosition.AtEnd)
					{
						this._position = CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder;
						return true;
					}
					break;
				}
				this._position = CollectionView.PlaceholderAwareEnumerator.Position.AfterPlaceholder;
				return this._baseEnumerator.MoveNext() && (this._newItem == CollectionView.NoNewItem || this._baseEnumerator.Current != this._newItem || this._baseEnumerator.MoveNext());
			}

			// Token: 0x17001E7E RID: 7806
			// (get) Token: 0x06008B3D RID: 35645 RVA: 0x0033987F File Offset: 0x0033887F
			public object Current
			{
				get
				{
					if (this._position == CollectionView.PlaceholderAwareEnumerator.Position.OnPlaceholder)
					{
						return CollectionView.NewItemPlaceholder;
					}
					if (this._position != CollectionView.PlaceholderAwareEnumerator.Position.OnNewItem)
					{
						return this._baseEnumerator.Current;
					}
					return this._newItem;
				}
			}

			// Token: 0x06008B3E RID: 35646 RVA: 0x003398AB File Offset: 0x003388AB
			public void Reset()
			{
				this._position = CollectionView.PlaceholderAwareEnumerator.Position.BeforePlaceholder;
				this._baseEnumerator.Reset();
			}

			// Token: 0x04004705 RID: 18181
			private CollectionView _collectionView;

			// Token: 0x04004706 RID: 18182
			private IEnumerator _baseEnumerator;

			// Token: 0x04004707 RID: 18183
			private NewItemPlaceholderPosition _placeholderPosition;

			// Token: 0x04004708 RID: 18184
			private CollectionView.PlaceholderAwareEnumerator.Position _position;

			// Token: 0x04004709 RID: 18185
			private object _newItem;

			// Token: 0x0400470A RID: 18186
			private int _timestamp;

			// Token: 0x02000C85 RID: 3205
			private enum Position
			{
				// Token: 0x04004F94 RID: 20372
				BeforePlaceholder,
				// Token: 0x04004F95 RID: 20373
				OnPlaceholder,
				// Token: 0x04004F96 RID: 20374
				OnNewItem,
				// Token: 0x04004F97 RID: 20375
				AfterPlaceholder
			}
		}

		// Token: 0x02000AE0 RID: 2784
		private class DeferHelper : IDisposable
		{
			// Token: 0x06008B3F RID: 35647 RVA: 0x003398BF File Offset: 0x003388BF
			public DeferHelper(CollectionView collectionView)
			{
				this._collectionView = collectionView;
			}

			// Token: 0x06008B40 RID: 35648 RVA: 0x003398CE File Offset: 0x003388CE
			public void Dispose()
			{
				if (this._collectionView != null)
				{
					this._collectionView.EndDefer();
					this._collectionView = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x0400470B RID: 18187
			private CollectionView _collectionView;
		}

		// Token: 0x02000AE1 RID: 2785
		private class SimpleMonitor : IDisposable
		{
			// Token: 0x06008B41 RID: 35649 RVA: 0x003398F0 File Offset: 0x003388F0
			public bool Enter()
			{
				if (this._entered)
				{
					return false;
				}
				this._entered = true;
				return true;
			}

			// Token: 0x06008B42 RID: 35650 RVA: 0x00339904 File Offset: 0x00338904
			public void Dispose()
			{
				this._entered = false;
				GC.SuppressFinalize(this);
			}

			// Token: 0x17001E7F RID: 7807
			// (get) Token: 0x06008B43 RID: 35651 RVA: 0x00339913 File Offset: 0x00338913
			public bool Busy
			{
				get
				{
					return this._entered;
				}
			}

			// Token: 0x0400470C RID: 18188
			private bool _entered;
		}

		// Token: 0x02000AE2 RID: 2786
		[Flags]
		private enum CollectionViewFlags
		{
			// Token: 0x0400470E RID: 18190
			UpdatedOutsideDispatcher = 2,
			// Token: 0x0400470F RID: 18191
			ShouldProcessCollectionChanged = 4,
			// Token: 0x04004710 RID: 18192
			IsCurrentBeforeFirst = 8,
			// Token: 0x04004711 RID: 18193
			IsCurrentAfterLast = 16,
			// Token: 0x04004712 RID: 18194
			IsDynamic = 32,
			// Token: 0x04004713 RID: 18195
			IsDataInGroupOrder = 64,
			// Token: 0x04004714 RID: 18196
			NeedsRefresh = 128,
			// Token: 0x04004715 RID: 18197
			AllowsCrossThreadChanges = 256,
			// Token: 0x04004716 RID: 18198
			CachedIsEmpty = 512
		}
	}
}
