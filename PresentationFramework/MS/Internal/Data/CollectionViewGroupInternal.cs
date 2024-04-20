using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x0200020B RID: 523
	internal class CollectionViewGroupInternal : CollectionViewGroup
	{
		// Token: 0x06001342 RID: 4930 RVA: 0x0014CDEC File Offset: 0x0014BDEC
		internal CollectionViewGroupInternal(object name, CollectionViewGroupInternal parent, bool isExplicit = false) : base(name)
		{
			this._parentGroup = parent;
			this._isExplicit = isExplicit;
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001343 RID: 4931 RVA: 0x0014CE0A File Offset: 0x0014BE0A
		public override bool IsBottomLevel
		{
			get
			{
				return this._groupBy == null;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x0014CE15 File Offset: 0x0014BE15
		// (set) Token: 0x06001345 RID: 4933 RVA: 0x0014CE20 File Offset: 0x0014BE20
		internal GroupDescription GroupBy
		{
			get
			{
				return this._groupBy;
			}
			set
			{
				bool isBottomLevel = this.IsBottomLevel;
				if (this._groupBy != null)
				{
					PropertyChangedEventManager.RemoveHandler(this._groupBy, new EventHandler<PropertyChangedEventArgs>(this.OnGroupByChanged), string.Empty);
				}
				this._groupBy = value;
				if (this._groupBy != null)
				{
					PropertyChangedEventManager.AddHandler(this._groupBy, new EventHandler<PropertyChangedEventArgs>(this.OnGroupByChanged), string.Empty);
				}
				this._groupComparer = ((this._groupBy == null) ? null : ListCollectionView.PrepareComparer(this._groupBy.CustomSort, this._groupBy.SortDescriptionsInternal, delegate
				{
					for (CollectionViewGroupInternal collectionViewGroupInternal = this; collectionViewGroupInternal != null; collectionViewGroupInternal = collectionViewGroupInternal.Parent)
					{
						CollectionViewGroupRoot collectionViewGroupRoot = collectionViewGroupInternal as CollectionViewGroupRoot;
						if (collectionViewGroupRoot != null)
						{
							return collectionViewGroupRoot.View;
						}
					}
					return null;
				}));
				if (isBottomLevel != this.IsBottomLevel)
				{
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsBottomLevel"));
				}
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0014CED2 File Offset: 0x0014BED2
		// (set) Token: 0x06001347 RID: 4935 RVA: 0x0014CEDA File Offset: 0x0014BEDA
		internal int FullCount
		{
			get
			{
				return this._fullCount;
			}
			set
			{
				this._fullCount = value;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x0014CEE3 File Offset: 0x0014BEE3
		// (set) Token: 0x06001349 RID: 4937 RVA: 0x0014CEEB File Offset: 0x0014BEEB
		internal int LastIndex
		{
			get
			{
				return this._lastIndex;
			}
			set
			{
				this._lastIndex = value;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x0014CEF4 File Offset: 0x0014BEF4
		internal object SeedItem
		{
			get
			{
				if (base.ItemCount > 0 && (this.GroupBy == null || this.GroupBy.GroupNames.Count == 0))
				{
					int i = 0;
					int count = base.Items.Count;
					while (i < count)
					{
						CollectionViewGroupInternal collectionViewGroupInternal = base.Items[i] as CollectionViewGroupInternal;
						if (collectionViewGroupInternal == null)
						{
							return base.Items[i];
						}
						if (collectionViewGroupInternal.ItemCount > 0)
						{
							return collectionViewGroupInternal.SeedItem;
						}
						i++;
					}
					return DependencyProperty.UnsetValue;
				}
				return DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0014CF79 File Offset: 0x0014BF79
		internal void Add(object item)
		{
			if (this._groupComparer == null)
			{
				this.ChangeCounts(item, 1);
				base.ProtectedItems.Add(item);
				return;
			}
			this.Insert(item, null, null);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0014CFA4 File Offset: 0x0014BFA4
		internal int Remove(object item, bool returnLeafIndex)
		{
			int result = -1;
			int num = base.ProtectedItems.IndexOf(item);
			if (num >= 0)
			{
				if (returnLeafIndex)
				{
					result = this.LeafIndexFromItem(null, num);
				}
				CollectionViewGroupInternal collectionViewGroupInternal = item as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					collectionViewGroupInternal.Clear();
					this.RemoveSubgroupFromMap(collectionViewGroupInternal);
				}
				this.ChangeCounts(item, -1);
				if (base.ProtectedItems.Count > 0)
				{
					base.ProtectedItems.RemoveAt(num);
				}
			}
			return result;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0014D00C File Offset: 0x0014C00C
		internal void Clear()
		{
			this.FullCount = 1;
			base.ProtectedItemCount = 0;
			if (this._groupBy != null)
			{
				PropertyChangedEventManager.RemoveHandler(this._groupBy, new EventHandler<PropertyChangedEventArgs>(this.OnGroupByChanged), string.Empty);
				this._groupBy = null;
				int i = 0;
				int count = base.ProtectedItems.Count;
				while (i < count)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = base.ProtectedItems[i] as CollectionViewGroupInternal;
					if (collectionViewGroupInternal != null)
					{
						collectionViewGroupInternal.Clear();
					}
					i++;
				}
			}
			base.ProtectedItems.Clear();
			if (this._nameToGroupMap != null)
			{
				this._nameToGroupMap.Clear();
			}
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0014D0A4 File Offset: 0x0014C0A4
		internal int LeafIndexOf(object item)
		{
			int num = 0;
			int i = 0;
			int count = base.Items.Count;
			while (i < count)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = base.Items[i] as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					int num2 = collectionViewGroupInternal.LeafIndexOf(item);
					if (num2 >= 0)
					{
						return num + num2;
					}
					num += collectionViewGroupInternal.ItemCount;
				}
				else
				{
					if (ItemsControl.EqualsEx(item, base.Items[i]))
					{
						return num;
					}
					num++;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0014D11C File Offset: 0x0014C11C
		internal int LeafIndexFromItem(object item, int index)
		{
			int num = 0;
			CollectionViewGroupInternal collectionViewGroupInternal = this;
			while (collectionViewGroupInternal != null)
			{
				int num2 = 0;
				int count = collectionViewGroupInternal.Items.Count;
				while (num2 < count && (index >= 0 || !ItemsControl.EqualsEx(item, collectionViewGroupInternal.Items[num2])) && index != num2)
				{
					CollectionViewGroupInternal collectionViewGroupInternal2 = collectionViewGroupInternal.Items[num2] as CollectionViewGroupInternal;
					num += ((collectionViewGroupInternal2 == null) ? 1 : collectionViewGroupInternal2.ItemCount);
					num2++;
				}
				item = collectionViewGroupInternal;
				collectionViewGroupInternal = collectionViewGroupInternal.Parent;
				index = -1;
			}
			return num;
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0014D198 File Offset: 0x0014C198
		internal object LeafAt(int index)
		{
			int i = 0;
			int count = base.Items.Count;
			while (i < count)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = base.Items[i] as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					if (index < collectionViewGroupInternal.ItemCount)
					{
						return collectionViewGroupInternal.LeafAt(index);
					}
					index -= collectionViewGroupInternal.ItemCount;
				}
				else
				{
					if (index == 0)
					{
						return base.Items[i];
					}
					index--;
				}
				i++;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0014D20E File Offset: 0x0014C20E
		internal IEnumerator GetLeafEnumerator()
		{
			return new CollectionViewGroupInternal.LeafEnumerator(this);
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0014D218 File Offset: 0x0014C218
		internal int Insert(object item, object seed, IComparer comparer)
		{
			int low = 0;
			if (this._groupComparer == null && this.GroupBy != null)
			{
				low = this.GroupBy.GroupNames.Count;
			}
			int num = this.FindIndex(item, seed, comparer, low, base.ProtectedItems.Count);
			this.ChangeCounts(item, 1);
			base.ProtectedItems.Insert(num, item);
			return num;
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0014D274 File Offset: 0x0014C274
		protected virtual int FindIndex(object item, object seed, IComparer comparer, int low, int high)
		{
			int i;
			if (this._groupComparer == null)
			{
				if (comparer != null)
				{
					CollectionViewGroupInternal.IListComparer listComparer = comparer as CollectionViewGroupInternal.IListComparer;
					if (listComparer != null)
					{
						listComparer.Reset();
					}
					for (i = low; i < high; i++)
					{
						CollectionViewGroupInternal collectionViewGroupInternal = base.ProtectedItems[i] as CollectionViewGroupInternal;
						object obj = (collectionViewGroupInternal != null) ? collectionViewGroupInternal.SeedItem : base.ProtectedItems[i];
						if (obj != DependencyProperty.UnsetValue && comparer.Compare(seed, obj) < 0)
						{
							break;
						}
					}
				}
				else
				{
					i = high;
				}
			}
			else
			{
				i = low;
				while (i < high && this._groupComparer.Compare(item, base.ProtectedItems[i]) >= 0)
				{
					i++;
				}
			}
			return i;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0014D318 File Offset: 0x0014C318
		internal bool Move(object item, IList list, ref int oldIndex, ref int newIndex)
		{
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			int count = base.ProtectedItems.Count;
			int num4 = 0;
			for (;;)
			{
				if (num4 == oldIndex)
				{
					num = num3;
					if (num2 >= 0)
					{
						goto IL_6F;
					}
					num3++;
				}
				if (num4 == newIndex)
				{
					num2 = num3;
					if (num >= 0)
					{
						break;
					}
					num4++;
					oldIndex++;
				}
				if (num3 < count && ItemsControl.EqualsEx(base.ProtectedItems[num3], list[num4]))
				{
					num3++;
				}
				num4++;
			}
			num2--;
			IL_6F:
			if (num == num2)
			{
				return false;
			}
			int num5 = 0;
			int num6;
			int num7;
			int num8;
			if (num < num2)
			{
				num6 = num + 1;
				num7 = num2 + 1;
				num8 = this.LeafIndexFromItem(null, num);
			}
			else
			{
				num6 = num2;
				num7 = num;
				num8 = this.LeafIndexFromItem(null, num2);
			}
			for (int i = num6; i < num7; i++)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = base.Items[i] as CollectionViewGroupInternal;
				num5 += ((collectionViewGroupInternal == null) ? 1 : collectionViewGroupInternal.ItemCount);
			}
			if (num < num2)
			{
				oldIndex = num8;
				newIndex = oldIndex + num5;
			}
			else
			{
				newIndex = num8;
				oldIndex = newIndex + num5;
			}
			base.ProtectedItems.Move(num, num2);
			return true;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0014D42D File Offset: 0x0014C42D
		protected virtual void OnGroupByChanged()
		{
			if (this.Parent != null)
			{
				this.Parent.OnGroupByChanged();
			}
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0014D442 File Offset: 0x0014C442
		internal void AddSubgroupToMap(object nameKey, CollectionViewGroupInternal subgroup)
		{
			if (nameKey == null)
			{
				nameKey = CollectionViewGroupInternal._nullGroupNameKey;
			}
			if (this._nameToGroupMap == null)
			{
				this._nameToGroupMap = new Hashtable();
			}
			this._nameToGroupMap[nameKey] = new WeakReference(subgroup);
			this.ScheduleMapCleanup();
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0014D47C File Offset: 0x0014C47C
		private void RemoveSubgroupFromMap(CollectionViewGroupInternal subgroup)
		{
			if (this._nameToGroupMap == null)
			{
				return;
			}
			object obj = null;
			foreach (object obj2 in this._nameToGroupMap.Keys)
			{
				WeakReference weakReference = this._nameToGroupMap[obj2] as WeakReference;
				if (weakReference != null && weakReference.Target == subgroup)
				{
					obj = obj2;
					break;
				}
			}
			if (obj != null)
			{
				this._nameToGroupMap.Remove(obj);
			}
			this.ScheduleMapCleanup();
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0014D514 File Offset: 0x0014C514
		internal CollectionViewGroupInternal GetSubgroupFromMap(object nameKey)
		{
			if (this._nameToGroupMap != null)
			{
				if (nameKey == null)
				{
					nameKey = CollectionViewGroupInternal._nullGroupNameKey;
				}
				WeakReference weakReference = this._nameToGroupMap[nameKey] as WeakReference;
				if (weakReference != null)
				{
					return weakReference.Target as CollectionViewGroupInternal;
				}
			}
			return null;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0014D555 File Offset: 0x0014C555
		private void ScheduleMapCleanup()
		{
			if (!this._mapCleanupScheduled)
			{
				this._mapCleanupScheduled = true;
				Dispatcher.CurrentDispatcher.BeginInvoke(new Action(delegate()
				{
					this._mapCleanupScheduled = false;
					if (this._nameToGroupMap != null)
					{
						ArrayList arrayList = new ArrayList();
						foreach (object obj in this._nameToGroupMap.Keys)
						{
							WeakReference weakReference = this._nameToGroupMap[obj] as WeakReference;
							if (weakReference == null || !weakReference.IsAlive)
							{
								arrayList.Add(obj);
							}
						}
						foreach (object key in arrayList)
						{
							this._nameToGroupMap.Remove(key);
						}
					}
				}), DispatcherPriority.ContextIdle, Array.Empty<object>());
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x0014D583 File Offset: 0x0014C583
		internal CollectionViewGroupInternal Parent
		{
			get
			{
				return this._parentGroup;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x0014D58B File Offset: 0x0014C58B
		private bool IsExplicit
		{
			get
			{
				return this._isExplicit;
			}
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0014D594 File Offset: 0x0014C594
		protected void ChangeCounts(object item, int delta)
		{
			bool flag = !(item is CollectionViewGroup);
			using (CollectionViewGroupInternal.EmptyGroupRemover emptyGroupRemover = CollectionViewGroupInternal.EmptyGroupRemover.Create(flag && delta < 0))
			{
				for (CollectionViewGroupInternal collectionViewGroupInternal = this; collectionViewGroupInternal != null; collectionViewGroupInternal = collectionViewGroupInternal._parentGroup)
				{
					collectionViewGroupInternal.FullCount += delta;
					if (flag)
					{
						collectionViewGroupInternal.ProtectedItemCount += delta;
						if (collectionViewGroupInternal.ProtectedItemCount == 0)
						{
							emptyGroupRemover.RemoveEmptyGroup(collectionViewGroupInternal);
						}
					}
				}
			}
			this._version++;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0014D624 File Offset: 0x0014C624
		private void OnGroupByChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnGroupByChanged();
		}

		// Token: 0x04000B76 RID: 2934
		private GroupDescription _groupBy;

		// Token: 0x04000B77 RID: 2935
		private CollectionViewGroupInternal _parentGroup;

		// Token: 0x04000B78 RID: 2936
		private IComparer _groupComparer;

		// Token: 0x04000B79 RID: 2937
		private int _fullCount = 1;

		// Token: 0x04000B7A RID: 2938
		private int _lastIndex;

		// Token: 0x04000B7B RID: 2939
		private int _version;

		// Token: 0x04000B7C RID: 2940
		private Hashtable _nameToGroupMap;

		// Token: 0x04000B7D RID: 2941
		private bool _mapCleanupScheduled;

		// Token: 0x04000B7E RID: 2942
		private bool _isExplicit;

		// Token: 0x04000B7F RID: 2943
		private static NamedObject _nullGroupNameKey = new NamedObject("NullGroupNameKey");

		// Token: 0x020009E6 RID: 2534
		internal class IListComparer : IComparer
		{
			// Token: 0x06008427 RID: 33831 RVA: 0x003251F3 File Offset: 0x003241F3
			internal IListComparer(IList list)
			{
				this.ResetList(list);
			}

			// Token: 0x06008428 RID: 33832 RVA: 0x00325202 File Offset: 0x00324202
			internal void Reset()
			{
				this._index = 0;
			}

			// Token: 0x06008429 RID: 33833 RVA: 0x0032520B File Offset: 0x0032420B
			internal void ResetList(IList list)
			{
				this._list = list;
				this._index = 0;
			}

			// Token: 0x0600842A RID: 33834 RVA: 0x0032521C File Offset: 0x0032421C
			public int Compare(object x, object y)
			{
				if (ItemsControl.EqualsEx(x, y))
				{
					return 0;
				}
				int num = (this._list != null) ? this._list.Count : 0;
				while (this._index < num)
				{
					object o = this._list[this._index];
					if (ItemsControl.EqualsEx(x, o))
					{
						return -1;
					}
					if (ItemsControl.EqualsEx(y, o))
					{
						return 1;
					}
					this._index++;
				}
				return 1;
			}

			// Token: 0x04003FFB RID: 16379
			private int _index;

			// Token: 0x04003FFC RID: 16380
			private IList _list;
		}

		// Token: 0x020009E7 RID: 2535
		private class LeafEnumerator : IEnumerator
		{
			// Token: 0x0600842B RID: 33835 RVA: 0x0032528D File Offset: 0x0032428D
			public LeafEnumerator(CollectionViewGroupInternal group)
			{
				this._group = group;
				this.DoReset();
			}

			// Token: 0x0600842C RID: 33836 RVA: 0x003252A2 File Offset: 0x003242A2
			void IEnumerator.Reset()
			{
				this.DoReset();
			}

			// Token: 0x0600842D RID: 33837 RVA: 0x003252AA File Offset: 0x003242AA
			private void DoReset()
			{
				this._version = this._group._version;
				this._index = -1;
				this._subEnum = null;
			}

			// Token: 0x0600842E RID: 33838 RVA: 0x003252CC File Offset: 0x003242CC
			bool IEnumerator.MoveNext()
			{
				if (this._group._version != this._version)
				{
					throw new InvalidOperationException();
				}
				while (this._subEnum == null || !this._subEnum.MoveNext())
				{
					this._index++;
					if (this._index >= this._group.Items.Count)
					{
						return false;
					}
					CollectionViewGroupInternal collectionViewGroupInternal = this._group.Items[this._index] as CollectionViewGroupInternal;
					if (collectionViewGroupInternal == null)
					{
						this._current = this._group.Items[this._index];
						this._subEnum = null;
						return true;
					}
					this._subEnum = collectionViewGroupInternal.GetLeafEnumerator();
				}
				this._current = this._subEnum.Current;
				return true;
			}

			// Token: 0x17001DB3 RID: 7603
			// (get) Token: 0x0600842F RID: 33839 RVA: 0x00325394 File Offset: 0x00324394
			object IEnumerator.Current
			{
				get
				{
					if (this._index < 0 || this._index >= this._group.Items.Count)
					{
						throw new InvalidOperationException();
					}
					return this._current;
				}
			}

			// Token: 0x04003FFD RID: 16381
			private CollectionViewGroupInternal _group;

			// Token: 0x04003FFE RID: 16382
			private int _version;

			// Token: 0x04003FFF RID: 16383
			private int _index;

			// Token: 0x04004000 RID: 16384
			private IEnumerator _subEnum;

			// Token: 0x04004001 RID: 16385
			private object _current;
		}

		// Token: 0x020009E8 RID: 2536
		private class EmptyGroupRemover : IDisposable
		{
			// Token: 0x06008430 RID: 33840 RVA: 0x003253C3 File Offset: 0x003243C3
			public static CollectionViewGroupInternal.EmptyGroupRemover Create(bool isNeeded)
			{
				if (!isNeeded)
				{
					return null;
				}
				return new CollectionViewGroupInternal.EmptyGroupRemover();
			}

			// Token: 0x06008431 RID: 33841 RVA: 0x003253CF File Offset: 0x003243CF
			public void RemoveEmptyGroup(CollectionViewGroupInternal group)
			{
				if (this._toRemove == null)
				{
					this._toRemove = new List<CollectionViewGroupInternal>();
				}
				this._toRemove.Add(group);
			}

			// Token: 0x06008432 RID: 33842 RVA: 0x003253F0 File Offset: 0x003243F0
			public void Dispose()
			{
				if (this._toRemove != null)
				{
					foreach (CollectionViewGroupInternal collectionViewGroupInternal in this._toRemove)
					{
						CollectionViewGroupInternal parent = collectionViewGroupInternal.Parent;
						if (parent != null && !collectionViewGroupInternal.IsExplicit)
						{
							parent.Remove(collectionViewGroupInternal, false);
						}
					}
				}
			}

			// Token: 0x04004002 RID: 16386
			private List<CollectionViewGroupInternal> _toRemove;
		}
	}
}
