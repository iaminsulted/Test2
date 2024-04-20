using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace MS.Internal.Data
{
	// Token: 0x0200022E RID: 558
	internal class LiveShapingList : IList, ICollection, IEnumerable
	{
		// Token: 0x06001521 RID: 5409 RVA: 0x00154210 File Offset: 0x00153210
		internal LiveShapingList(ICollectionViewLiveShaping view, LiveShapingFlags flags, IComparer comparer)
		{
			this._view = view;
			this._comparer = comparer;
			this._isCustomSorting = !(comparer is SortFieldComparer);
			this._dpFromPath = new LiveShapingList.DPFromPath();
			this._root = new LiveShapingTree(this);
			if (comparer != null)
			{
				this._root.Comparison = new Comparison<LiveShapingItem>(this.CompareLiveShapingItems);
			}
			this._sortDirtyItems = new List<LiveShapingItem>();
			this._filterDirtyItems = new List<LiveShapingItem>();
			this._groupDirtyItems = new List<LiveShapingItem>();
			this.SetLiveShapingProperties(flags);
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x0015429C File Offset: 0x0015329C
		internal ICollectionViewLiveShaping View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x001542A4 File Offset: 0x001532A4
		internal Dictionary<string, DependencyProperty> ObservedProperties
		{
			get
			{
				return this._dpFromPath;
			}
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x001542AC File Offset: 0x001532AC
		internal void SetLiveShapingProperties(LiveShapingFlags flags)
		{
			this._dpFromPath.BeginReset();
			SortDescriptionCollection sortDescriptions = ((ICollectionView)this.View).SortDescriptions;
			int count = sortDescriptions.Count;
			this._compInfos = new LivePropertyInfo[count];
			for (int i = 0; i < count; i++)
			{
				string path = this.NormalizePath(sortDescriptions[i].PropertyName);
				this._compInfos[i] = new LivePropertyInfo(path, this._dpFromPath.GetDP(path));
			}
			if (this.TestLiveShapingFlag(flags, LiveShapingFlags.Sorting))
			{
				Collection<string> liveSortingProperties = this.View.LiveSortingProperties;
				if (liveSortingProperties.Count == 0)
				{
					this._sortInfos = this._compInfos;
				}
				else
				{
					count = liveSortingProperties.Count;
					this._sortInfos = new LivePropertyInfo[count];
					for (int i = 0; i < count; i++)
					{
						string path = this.NormalizePath(liveSortingProperties[i]);
						this._sortInfos[i] = new LivePropertyInfo(path, this._dpFromPath.GetDP(path));
					}
				}
			}
			else
			{
				this._sortInfos = Array.Empty<LivePropertyInfo>();
			}
			if (this.TestLiveShapingFlag(flags, LiveShapingFlags.Filtering))
			{
				Collection<string> liveFilteringProperties = this.View.LiveFilteringProperties;
				count = liveFilteringProperties.Count;
				this._filterInfos = new LivePropertyInfo[count];
				for (int i = 0; i < count; i++)
				{
					string path = this.NormalizePath(liveFilteringProperties[i]);
					this._filterInfos[i] = new LivePropertyInfo(path, this._dpFromPath.GetDP(path));
				}
				this._filterRoot = new LiveShapingTree(this);
			}
			else
			{
				this._filterInfos = Array.Empty<LivePropertyInfo>();
				this._filterRoot = null;
			}
			if (this.TestLiveShapingFlag(flags, LiveShapingFlags.Grouping))
			{
				Collection<string> collection = this.View.LiveGroupingProperties;
				if (collection.Count == 0)
				{
					collection = new Collection<string>();
					ICollectionView collectionView = this.View as ICollectionView;
					ObservableCollection<GroupDescription> observableCollection = (collectionView != null) ? collectionView.GroupDescriptions : null;
					if (observableCollection != null)
					{
						foreach (GroupDescription groupDescription in observableCollection)
						{
							PropertyGroupDescription propertyGroupDescription = groupDescription as PropertyGroupDescription;
							if (propertyGroupDescription != null)
							{
								collection.Add(propertyGroupDescription.PropertyName);
							}
						}
					}
				}
				count = collection.Count;
				this._groupInfos = new LivePropertyInfo[count];
				for (int i = 0; i < count; i++)
				{
					string path = this.NormalizePath(collection[i]);
					this._groupInfos[i] = new LivePropertyInfo(path, this._dpFromPath.GetDP(path));
				}
			}
			else
			{
				this._groupInfos = Array.Empty<LivePropertyInfo>();
			}
			this._dpFromPath.EndReset();
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x00154538 File Offset: 0x00153538
		private bool TestLiveShapingFlag(LiveShapingFlags flags, LiveShapingFlags flag)
		{
			return (flags & flag) > (LiveShapingFlags)0;
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x00154540 File Offset: 0x00153540
		internal int Search(int index, int count, object value)
		{
			LiveShapingItem liveShapingItem = new LiveShapingItem(value, this, true, null, true);
			RBFinger<LiveShapingItem> rbfinger = this._root.BoundedSearch(liveShapingItem, index, index + count);
			this.ClearItem(liveShapingItem);
			if (!rbfinger.Found)
			{
				return ~rbfinger.Index;
			}
			return rbfinger.Index;
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x00154589 File Offset: 0x00153589
		internal void Sort()
		{
			this._root.Sort();
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x00154598 File Offset: 0x00153598
		internal int CompareLiveShapingItems(LiveShapingItem x, LiveShapingItem y)
		{
			if (x == y || ItemsControl.EqualsEx(x.Item, y.Item))
			{
				return 0;
			}
			int num = 0;
			if (!this._isCustomSorting)
			{
				SortFieldComparer sortFieldComparer = this._comparer as SortFieldComparer;
				SortDescriptionCollection sortDescriptions = ((ICollectionView)this.View).SortDescriptions;
				int num2 = this._compInfos.Length;
				for (int i = 0; i < num2; i++)
				{
					object value = x.GetValue(this._compInfos[i].Path, this._compInfos[i].Property);
					object value2 = y.GetValue(this._compInfos[i].Path, this._compInfos[i].Property);
					num = sortFieldComparer.BaseComparer.Compare(value, value2);
					if (sortDescriptions[i].Direction == ListSortDirection.Descending)
					{
						num = -num;
					}
					if (num != 0)
					{
						break;
					}
				}
			}
			else
			{
				num = this._comparer.Compare(x.Item, y.Item);
			}
			return num;
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x001546A4 File Offset: 0x001536A4
		internal void Move(int oldIndex, int newIndex)
		{
			this._root.Move(oldIndex, newIndex);
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x001546B3 File Offset: 0x001536B3
		internal void RestoreLiveSortingByInsertionSort(Action<NotifyCollectionChangedEventArgs, int, int> RaiseMoveEvent)
		{
			this._isRestoringLiveSorting = true;
			this._root.RestoreLiveSortingByInsertionSort(RaiseMoveEvent);
			this._isRestoringLiveSorting = false;
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x001546D0 File Offset: 0x001536D0
		internal void AddFilteredItem(object item)
		{
			LiveShapingItem item2 = new LiveShapingItem(item, this, true, null, false)
			{
				FailsFilter = true
			};
			this._filterRoot.Insert(this._filterRoot.Count, item2);
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x00154706 File Offset: 0x00153706
		internal void AddFilteredItem(LiveShapingItem lsi)
		{
			this.InitializeItem(lsi, lsi.Item, true, false);
			lsi.FailsFilter = true;
			this._filterRoot.Insert(this._filterRoot.Count, lsi);
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00154738 File Offset: 0x00153738
		internal void SetStartingIndexForFilteredItem(object item, int value)
		{
			foreach (LiveShapingItem liveShapingItem in this._filterDirtyItems)
			{
				if (ItemsControl.EqualsEx(item, liveShapingItem.Item))
				{
					liveShapingItem.StartingIndex = value;
					break;
				}
			}
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0015479C File Offset: 0x0015379C
		internal void RemoveFilteredItem(LiveShapingItem lsi)
		{
			this._filterRoot.RemoveAt(this._filterRoot.IndexOf(lsi));
			this.ClearItem(lsi);
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x001547BC File Offset: 0x001537BC
		internal void RemoveFilteredItem(object item)
		{
			LiveShapingItem liveShapingItem = this._filterRoot.FindItem(item);
			if (liveShapingItem != null)
			{
				this.RemoveFilteredItem(liveShapingItem);
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x001547E0 File Offset: 0x001537E0
		internal void ReplaceFilteredItem(object oldItem, object newItem)
		{
			LiveShapingItem liveShapingItem = this._filterRoot.FindItem(oldItem);
			if (liveShapingItem != null)
			{
				this.ClearItem(liveShapingItem);
				this.InitializeItem(liveShapingItem, newItem, true, false);
			}
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0015480E File Offset: 0x0015380E
		internal int IndexOf(LiveShapingItem lsi)
		{
			return this._root.IndexOf(lsi);
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0015481C File Offset: 0x0015381C
		internal void InitializeItem(LiveShapingItem lsi, object item, bool filtered, bool oneTime)
		{
			lsi.Item = item;
			if (!filtered)
			{
				foreach (LivePropertyInfo livePropertyInfo in this._sortInfos)
				{
					lsi.Block = this._root.PlaceholderBlock;
					lsi.SetBinding(livePropertyInfo.Path, livePropertyInfo.Property, oneTime, true);
				}
				foreach (LivePropertyInfo livePropertyInfo2 in this._groupInfos)
				{
					lsi.SetBinding(livePropertyInfo2.Path, livePropertyInfo2.Property, oneTime, false);
				}
			}
			foreach (LivePropertyInfo livePropertyInfo3 in this._filterInfos)
			{
				lsi.SetBinding(livePropertyInfo3.Path, livePropertyInfo3.Property, oneTime, false);
			}
			lsi.ForwardChanges = !oneTime;
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x001548EC File Offset: 0x001538EC
		internal void ClearItem(LiveShapingItem lsi)
		{
			lsi.ForwardChanges = false;
			foreach (DependencyProperty dp in this.ObservedProperties.Values)
			{
				BindingOperations.ClearBinding(lsi, dp);
			}
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0015494C File Offset: 0x0015394C
		private string NormalizePath(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				return path;
			}
			return string.Empty;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x00154960 File Offset: 0x00153960
		internal void OnItemPropertyChanged(LiveShapingItem lsi, DependencyProperty dp)
		{
			if (this.ContainsDP(this._sortInfos, dp) && !lsi.FailsFilter && !lsi.IsSortPendingClean)
			{
				lsi.IsSortDirty = true;
				lsi.IsSortPendingClean = true;
				this._sortDirtyItems.Add(lsi);
				this.OnLiveShapingDirty();
			}
			if (this.ContainsDP(this._filterInfos, dp) && !lsi.IsFilterDirty)
			{
				lsi.IsFilterDirty = true;
				this._filterDirtyItems.Add(lsi);
				this.OnLiveShapingDirty();
			}
			if (this.ContainsDP(this._groupInfos, dp) && !lsi.FailsFilter && !lsi.IsGroupDirty)
			{
				lsi.IsGroupDirty = true;
				this._groupDirtyItems.Add(lsi);
				this.OnLiveShapingDirty();
			}
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x00154A14 File Offset: 0x00153A14
		internal void OnItemPropertyChangedCrossThread(LiveShapingItem lsi, DependencyProperty dp)
		{
			if (this._isCustomSorting && this.ContainsDP(this._sortInfos, dp) && !lsi.FailsFilter)
			{
				lsi.IsSortDirty = true;
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001537 RID: 5431 RVA: 0x00154A3C File Offset: 0x00153A3C
		// (remove) Token: 0x06001538 RID: 5432 RVA: 0x00154A74 File Offset: 0x00153A74
		internal event EventHandler LiveShapingDirty;

		// Token: 0x06001539 RID: 5433 RVA: 0x00154AA9 File Offset: 0x00153AA9
		private void OnLiveShapingDirty()
		{
			if (this.LiveShapingDirty != null)
			{
				this.LiveShapingDirty(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x00154AC4 File Offset: 0x00153AC4
		private bool ContainsDP(LivePropertyInfo[] infos, DependencyProperty dp)
		{
			for (int i = 0; i < infos.Length; i++)
			{
				if (infos[i].Property == dp || (dp == null && string.IsNullOrEmpty(infos[i].Path)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x00154B07 File Offset: 0x00153B07
		internal void FindPosition(LiveShapingItem lsi, out int oldIndex, out int newIndex)
		{
			this._root.FindPosition(lsi, out oldIndex, out newIndex);
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x00154B17 File Offset: 0x00153B17
		internal List<LiveShapingItem> SortDirtyItems
		{
			get
			{
				return this._sortDirtyItems;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x00154B1F File Offset: 0x00153B1F
		internal List<LiveShapingItem> FilterDirtyItems
		{
			get
			{
				return this._filterDirtyItems;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x00154B27 File Offset: 0x00153B27
		internal List<LiveShapingItem> GroupDirtyItems
		{
			get
			{
				return this._groupDirtyItems;
			}
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00154B2F File Offset: 0x00153B2F
		internal LiveShapingItem ItemAt(int index)
		{
			return this._root[index];
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x00154B3D File Offset: 0x00153B3D
		internal bool IsRestoringLiveSorting
		{
			get
			{
				return this._isRestoringLiveSorting;
			}
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x00154B45 File Offset: 0x00153B45
		public int Add(object value)
		{
			this.Insert(this.Count, value);
			return this.Count;
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00154B5A File Offset: 0x00153B5A
		public void Clear()
		{
			this.ForEach(delegate(LiveShapingItem x)
			{
				this.ClearItem(x);
			});
			this._root = new LiveShapingTree(this);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00154B7A File Offset: 0x00153B7A
		public bool Contains(object value)
		{
			return this.IndexOf(value) >= 0;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00154B8C File Offset: 0x00153B8C
		public int IndexOf(object value)
		{
			int result = 0;
			this.ForEachUntil(delegate(LiveShapingItem x)
			{
				if (ItemsControl.EqualsEx(value, x.Item))
				{
					return true;
				}
				int result;
				result++;
				result = result;
				return false;
			});
			if (result >= this.Count)
			{
				return -1;
			}
			return result;
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00154BD5 File Offset: 0x00153BD5
		public void Insert(int index, object value)
		{
			this._root.Insert(index, new LiveShapingItem(value, this, false, null, false));
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001547 RID: 5447 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x00154BF0 File Offset: 0x00153BF0
		public void Remove(object value)
		{
			int num = this.IndexOf(value);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00154C10 File Offset: 0x00153C10
		public void RemoveAt(int index)
		{
			LiveShapingItem liveShapingItem = this._root[index];
			this._root.RemoveAt(index);
			this.ClearItem(liveShapingItem);
			liveShapingItem.IsDeleted = true;
		}

		// Token: 0x17000422 RID: 1058
		public object this[int index]
		{
			get
			{
				return this._root[index].Item;
			}
			set
			{
				this._root.ReplaceAt(index, value);
			}
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x001056E1 File Offset: 0x001046E1
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x00154C66 File Offset: 0x00153C66
		public int Count
		{
			get
			{
				return this._root.Count;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x00109403 File Offset: 0x00108403
		public object SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00154C73 File Offset: 0x00153C73
		public IEnumerator GetEnumerator()
		{
			return new LiveShapingList.ItemEnumerator(this._root.GetEnumerator());
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00154C85 File Offset: 0x00153C85
		private void ForEach(Action<LiveShapingItem> action)
		{
			this._root.ForEach(action);
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00154C93 File Offset: 0x00153C93
		private void ForEachUntil(Func<LiveShapingItem, bool> action)
		{
			this._root.ForEachUntil(action);
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal bool VerifyLiveSorting(LiveShapingItem lsi)
		{
			return true;
		}

		// Token: 0x04000BFD RID: 3069
		private ICollectionViewLiveShaping _view;

		// Token: 0x04000BFE RID: 3070
		private LiveShapingList.DPFromPath _dpFromPath;

		// Token: 0x04000BFF RID: 3071
		private LivePropertyInfo[] _compInfos;

		// Token: 0x04000C00 RID: 3072
		private LivePropertyInfo[] _sortInfos;

		// Token: 0x04000C01 RID: 3073
		private LivePropertyInfo[] _filterInfos;

		// Token: 0x04000C02 RID: 3074
		private LivePropertyInfo[] _groupInfos;

		// Token: 0x04000C03 RID: 3075
		private IComparer _comparer;

		// Token: 0x04000C04 RID: 3076
		private LiveShapingTree _root;

		// Token: 0x04000C05 RID: 3077
		private LiveShapingTree _filterRoot;

		// Token: 0x04000C06 RID: 3078
		private List<LiveShapingItem> _sortDirtyItems;

		// Token: 0x04000C07 RID: 3079
		private List<LiveShapingItem> _filterDirtyItems;

		// Token: 0x04000C08 RID: 3080
		private List<LiveShapingItem> _groupDirtyItems;

		// Token: 0x04000C09 RID: 3081
		private bool _isRestoringLiveSorting;

		// Token: 0x04000C0A RID: 3082
		private bool _isCustomSorting;

		// Token: 0x04000C0B RID: 3083
		private static List<DependencyProperty> s_dpList = new List<DependencyProperty>();

		// Token: 0x04000C0C RID: 3084
		private static object s_Sync = new object();

		// Token: 0x020009F4 RID: 2548
		private class DPFromPath : Dictionary<string, DependencyProperty>
		{
			// Token: 0x0600845C RID: 33884 RVA: 0x00325911 File Offset: 0x00324911
			public void BeginReset()
			{
				this._unusedKeys = new List<string>(base.Keys);
				this._dpIndex = 0;
			}

			// Token: 0x0600845D RID: 33885 RVA: 0x0032592C File Offset: 0x0032492C
			public void EndReset()
			{
				foreach (string key in this._unusedKeys)
				{
					base.Remove(key);
				}
				this._unusedKeys = null;
			}

			// Token: 0x0600845E RID: 33886 RVA: 0x00325988 File Offset: 0x00324988
			public DependencyProperty GetDP(string path)
			{
				DependencyProperty dependencyProperty;
				if (base.TryGetValue(path, out dependencyProperty))
				{
					this._unusedKeys.Remove(path);
					return dependencyProperty;
				}
				ICollection<DependencyProperty> values = base.Values;
				while (this._dpIndex < LiveShapingList.s_dpList.Count)
				{
					dependencyProperty = LiveShapingList.s_dpList[this._dpIndex];
					if (!values.Contains(dependencyProperty))
					{
						base[path] = dependencyProperty;
						return dependencyProperty;
					}
					this._dpIndex++;
				}
				object s_Sync = LiveShapingList.s_Sync;
				lock (s_Sync)
				{
					dependencyProperty = DependencyProperty.RegisterAttached(string.Format(TypeConverterHelper.InvariantEnglishUS, "LiveSortingTargetProperty{0}", LiveShapingList.s_dpList.Count), typeof(object), typeof(LiveShapingList));
					LiveShapingList.s_dpList.Add(dependencyProperty);
				}
				base[path] = dependencyProperty;
				return dependencyProperty;
			}

			// Token: 0x04004029 RID: 16425
			private List<string> _unusedKeys;

			// Token: 0x0400402A RID: 16426
			private int _dpIndex;
		}

		// Token: 0x020009F5 RID: 2549
		private class ItemEnumerator : IEnumerator
		{
			// Token: 0x06008460 RID: 33888 RVA: 0x00325A7C File Offset: 0x00324A7C
			public ItemEnumerator(IEnumerator<LiveShapingItem> ie)
			{
				this._ie = ie;
			}

			// Token: 0x06008461 RID: 33889 RVA: 0x00325A8B File Offset: 0x00324A8B
			void IEnumerator.Reset()
			{
				this._ie.Reset();
			}

			// Token: 0x06008462 RID: 33890 RVA: 0x00325A98 File Offset: 0x00324A98
			bool IEnumerator.MoveNext()
			{
				return this._ie.MoveNext();
			}

			// Token: 0x17001DBC RID: 7612
			// (get) Token: 0x06008463 RID: 33891 RVA: 0x00325AA5 File Offset: 0x00324AA5
			object IEnumerator.Current
			{
				get
				{
					return this._ie.Current.Item;
				}
			}

			// Token: 0x0400402B RID: 16427
			private IEnumerator<LiveShapingItem> _ie;
		}
	}
}
