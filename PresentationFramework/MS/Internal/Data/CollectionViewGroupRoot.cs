using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200020C RID: 524
	internal class CollectionViewGroupRoot : CollectionViewGroupInternal, INotifyCollectionChanged
	{
		// Token: 0x06001361 RID: 4961 RVA: 0x0014D74C File Offset: 0x0014C74C
		internal CollectionViewGroupRoot(CollectionView view) : base("Root", null, false)
		{
			this._view = view;
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06001362 RID: 4962 RVA: 0x0014D770 File Offset: 0x0014C770
		// (remove) Token: 0x06001363 RID: 4963 RVA: 0x0014D7A8 File Offset: 0x0014C7A8
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x06001364 RID: 4964 RVA: 0x0014D7DD File Offset: 0x0014C7DD
		public void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x0014D802 File Offset: 0x0014C802
		public virtual ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._groupBy;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x0014D80A File Offset: 0x0014C80A
		// (set) Token: 0x06001367 RID: 4967 RVA: 0x0014D812 File Offset: 0x0014C812
		public virtual GroupDescriptionSelectorCallback GroupBySelector
		{
			get
			{
				return this._groupBySelector;
			}
			set
			{
				this._groupBySelector = value;
			}
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0014D81B File Offset: 0x0014C81B
		protected override void OnGroupByChanged()
		{
			if (this.GroupDescriptionChanged != null)
			{
				this.GroupDescriptionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06001369 RID: 4969 RVA: 0x0014D838 File Offset: 0x0014C838
		// (remove) Token: 0x0600136A RID: 4970 RVA: 0x0014D870 File Offset: 0x0014C870
		internal event EventHandler GroupDescriptionChanged;

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x0014D8A5 File Offset: 0x0014C8A5
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x0014D8AD File Offset: 0x0014C8AD
		internal IComparer ActiveComparer
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

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x0014D8B6 File Offset: 0x0014C8B6
		internal CultureInfo Culture
		{
			get
			{
				return this._view.Culture;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x0014D8C3 File Offset: 0x0014C8C3
		// (set) Token: 0x0600136F RID: 4975 RVA: 0x0014D8CB File Offset: 0x0014C8CB
		internal bool IsDataInGroupOrder
		{
			get
			{
				return this._isDataInGroupOrder;
			}
			set
			{
				this._isDataInGroupOrder = value;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x0014D8D4 File Offset: 0x0014C8D4
		internal CollectionView View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0014D8DC File Offset: 0x0014C8DC
		internal void Initialize()
		{
			if (CollectionViewGroupRoot._topLevelGroupDescription == null)
			{
				CollectionViewGroupRoot._topLevelGroupDescription = new CollectionViewGroupRoot.TopLevelGroupDescription();
			}
			this.InitializeGroup(this, CollectionViewGroupRoot._topLevelGroupDescription, 0);
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0014D8FC File Offset: 0x0014C8FC
		internal void AddToSubgroups(object item, LiveShapingItem lsi, bool loading)
		{
			this.AddToSubgroups(item, lsi, this, 0, loading);
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0014D909 File Offset: 0x0014C909
		internal bool RemoveFromSubgroups(object item)
		{
			return this.RemoveFromSubgroups(item, this, 0);
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0014D914 File Offset: 0x0014C914
		internal void RemoveItemFromSubgroupsByExhaustiveSearch(object item)
		{
			this.RemoveItemFromSubgroupsByExhaustiveSearch(this, item);
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0014D920 File Offset: 0x0014C920
		internal void InsertSpecialItem(int index, object item, bool loading)
		{
			base.ChangeCounts(item, 1);
			base.ProtectedItems.Insert(index, item);
			if (!loading)
			{
				int index2 = base.LeafIndexFromItem(item, index);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index2));
			}
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0014D95C File Offset: 0x0014C95C
		internal void RemoveSpecialItem(int index, object item, bool loading)
		{
			int index2 = -1;
			if (!loading)
			{
				index2 = base.LeafIndexFromItem(item, index);
			}
			base.ChangeCounts(item, -1);
			base.ProtectedItems.RemoveAt(index);
			if (!loading)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index2));
			}
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0014D99C File Offset: 0x0014C99C
		internal void MoveWithinSubgroups(object item, LiveShapingItem lsi, IList list, int oldIndex, int newIndex)
		{
			if (lsi == null)
			{
				this.MoveWithinSubgroups(item, this, 0, list, oldIndex, newIndex);
				return;
			}
			CollectionViewGroupInternal parentGroup = lsi.ParentGroup;
			if (parentGroup != null)
			{
				this.MoveWithinSubgroup(item, parentGroup, list, oldIndex, newIndex);
				return;
			}
			foreach (CollectionViewGroupInternal group in lsi.ParentGroups)
			{
				this.MoveWithinSubgroup(item, group, list, oldIndex, newIndex);
			}
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0014DA20 File Offset: 0x0014CA20
		protected override int FindIndex(object item, object seed, IComparer comparer, int low, int high)
		{
			IEditableCollectionView editableCollectionView = this._view as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				if (editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
				{
					low++;
					if (editableCollectionView.IsAddingNew)
					{
						low++;
					}
				}
				else
				{
					if (editableCollectionView.IsAddingNew)
					{
						high--;
					}
					if (editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
					{
						high--;
					}
				}
			}
			return base.FindIndex(item, seed, comparer, low, high);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0014DA88 File Offset: 0x0014CA88
		internal void RestoreGrouping(LiveShapingItem lsi, List<AbandonedGroupItem> deleteList)
		{
			CollectionViewGroupRoot.GroupTreeNode groupTreeNode = this.BuildGroupTree(lsi);
			groupTreeNode.ContainsItem = true;
			this.RestoreGrouping(lsi, groupTreeNode, 0, deleteList);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0014DAB0 File Offset: 0x0014CAB0
		private void RestoreGrouping(LiveShapingItem lsi, CollectionViewGroupRoot.GroupTreeNode node, int level, List<AbandonedGroupItem> deleteList)
		{
			if (node.ContainsItem)
			{
				object obj = this.GetGroupName(lsi.Item, node.Group.GroupBy, level);
				if (obj == CollectionViewGroupRoot.UseAsItemDirectly)
				{
					goto IL_12E;
				}
				ICollection collection = obj as ICollection;
				ArrayList arrayList = (collection == null) ? null : new ArrayList(collection);
				for (CollectionViewGroupRoot.GroupTreeNode groupTreeNode = node.FirstChild; groupTreeNode != null; groupTreeNode = groupTreeNode.Sibling)
				{
					if (arrayList == null)
					{
						if (object.Equals(obj, groupTreeNode.Group.Name))
						{
							groupTreeNode.ContainsItem = true;
							obj = DependencyProperty.UnsetValue;
							break;
						}
					}
					else if (arrayList.Contains(groupTreeNode.Group.Name))
					{
						groupTreeNode.ContainsItem = true;
						arrayList.Remove(groupTreeNode.Group.Name);
					}
				}
				if (arrayList == null)
				{
					if (obj != DependencyProperty.UnsetValue)
					{
						this.AddToSubgroup(lsi.Item, lsi, node.Group, level, obj, false);
						goto IL_12E;
					}
					goto IL_12E;
				}
				else
				{
					using (IEnumerator enumerator = arrayList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object name = enumerator.Current;
							this.AddToSubgroup(lsi.Item, lsi, node.Group, level, name, false);
						}
						goto IL_12E;
					}
				}
			}
			if (node.ContainsItemDirectly)
			{
				deleteList.Add(new AbandonedGroupItem(lsi, node.Group));
			}
			IL_12E:
			for (CollectionViewGroupRoot.GroupTreeNode groupTreeNode2 = node.FirstChild; groupTreeNode2 != null; groupTreeNode2 = groupTreeNode2.Sibling)
			{
				this.RestoreGrouping(lsi, groupTreeNode2, level + 1, deleteList);
			}
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0014DC20 File Offset: 0x0014CC20
		private CollectionViewGroupRoot.GroupTreeNode BuildGroupTree(LiveShapingItem lsi)
		{
			CollectionViewGroupInternal collectionViewGroupInternal = lsi.ParentGroup;
			if (collectionViewGroupInternal != null)
			{
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode = new CollectionViewGroupRoot.GroupTreeNode
				{
					Group = collectionViewGroupInternal,
					ContainsItemDirectly = true
				};
				for (;;)
				{
					collectionViewGroupInternal = collectionViewGroupInternal.Parent;
					if (collectionViewGroupInternal == null)
					{
						break;
					}
					groupTreeNode = new CollectionViewGroupRoot.GroupTreeNode
					{
						Group = collectionViewGroupInternal,
						FirstChild = groupTreeNode
					};
				}
				return groupTreeNode;
			}
			List<CollectionViewGroupInternal> parentGroups = lsi.ParentGroups;
			List<CollectionViewGroupRoot.GroupTreeNode> list = new List<CollectionViewGroupRoot.GroupTreeNode>(parentGroups.Count + 1);
			CollectionViewGroupRoot.GroupTreeNode result = null;
			foreach (CollectionViewGroupInternal group in parentGroups)
			{
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode = new CollectionViewGroupRoot.GroupTreeNode
				{
					Group = group,
					ContainsItemDirectly = true
				};
				list.Add(groupTreeNode);
			}
			for (int i = 0; i < list.Count; i++)
			{
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode = list[i];
				collectionViewGroupInternal = groupTreeNode.Group.Parent;
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode2 = null;
				if (collectionViewGroupInternal == null)
				{
					result = groupTreeNode;
				}
				else
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (list[j].Group == collectionViewGroupInternal)
						{
							groupTreeNode2 = list[j];
							break;
						}
					}
					if (groupTreeNode2 == null)
					{
						groupTreeNode2 = new CollectionViewGroupRoot.GroupTreeNode
						{
							Group = collectionViewGroupInternal,
							FirstChild = groupTreeNode
						};
						list.Add(groupTreeNode2);
					}
					else
					{
						groupTreeNode.Sibling = groupTreeNode2.FirstChild;
						groupTreeNode2.FirstChild = groupTreeNode;
					}
				}
			}
			return result;
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0014DD80 File Offset: 0x0014CD80
		internal void DeleteAbandonedGroupItems(List<AbandonedGroupItem> deleteList)
		{
			foreach (AbandonedGroupItem abandonedGroupItem in deleteList)
			{
				this.RemoveFromGroupDirectly(abandonedGroupItem.Group, abandonedGroupItem.Item.Item);
				abandonedGroupItem.Item.RemoveParentGroup(abandonedGroupItem.Group);
			}
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0014DDF0 File Offset: 0x0014CDF0
		private void InitializeGroup(CollectionViewGroupInternal group, GroupDescription parentDescription, int level)
		{
			GroupDescription groupDescription = this.GetGroupDescription(group, parentDescription, level);
			group.GroupBy = groupDescription;
			ObservableCollection<object> observableCollection = (groupDescription != null) ? groupDescription.GroupNames : null;
			if (observableCollection != null)
			{
				int i = 0;
				int count = observableCollection.Count;
				while (i < count)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = new CollectionViewGroupInternal(observableCollection[i], group, true);
					this.InitializeGroup(collectionViewGroupInternal, groupDescription, level + 1);
					group.Add(collectionViewGroupInternal);
					i++;
				}
			}
			group.LastIndex = 0;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0014DE5C File Offset: 0x0014CE5C
		private GroupDescription GetGroupDescription(CollectionViewGroup group, GroupDescription parentDescription, int level)
		{
			GroupDescription groupDescription = null;
			if (group == this)
			{
				group = null;
			}
			if (groupDescription == null && this.GroupBySelector != null)
			{
				groupDescription = this.GroupBySelector(group, level);
			}
			if (groupDescription == null && level < this.GroupDescriptions.Count)
			{
				groupDescription = this.GroupDescriptions[level];
			}
			return groupDescription;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0014DEAC File Offset: 0x0014CEAC
		private void AddToSubgroups(object item, LiveShapingItem lsi, CollectionViewGroupInternal group, int level, bool loading)
		{
			object groupName = this.GetGroupName(item, group.GroupBy, level);
			if (groupName == CollectionViewGroupRoot.UseAsItemDirectly)
			{
				if (lsi != null)
				{
					lsi.AddParentGroup(group);
				}
				if (loading)
				{
					group.Add(item);
					return;
				}
				int index = group.Insert(item, item, this.ActiveComparer);
				int index2 = group.LeafIndexFromItem(item, index);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index2));
				return;
			}
			else
			{
				ICollection collection;
				if ((collection = (groupName as ICollection)) == null)
				{
					this.AddToSubgroup(item, lsi, group, level, groupName, loading);
					return;
				}
				foreach (object name in collection)
				{
					this.AddToSubgroup(item, lsi, group, level, name, loading);
				}
				return;
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0014DF7C File Offset: 0x0014CF7C
		private void AddToSubgroup(object item, LiveShapingItem lsi, CollectionViewGroupInternal group, int level, object name, bool loading)
		{
			int i = (loading && this.IsDataInGroupOrder) ? group.LastIndex : 0;
			object groupNameKey = this.GetGroupNameKey(name, group);
			CollectionViewGroupInternal collectionViewGroupInternal;
			if ((collectionViewGroupInternal = group.GetSubgroupFromMap(groupNameKey)) != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
			{
				group.LastIndex = ((group.Items[i] == collectionViewGroupInternal) ? i : 0);
				this.AddToSubgroups(item, lsi, collectionViewGroupInternal, level + 1, loading);
				return;
			}
			int count = group.Items.Count;
			while (i < count)
			{
				collectionViewGroupInternal = (group.Items[i] as CollectionViewGroupInternal);
				if (collectionViewGroupInternal != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
				{
					group.LastIndex = i;
					group.AddSubgroupToMap(groupNameKey, collectionViewGroupInternal);
					this.AddToSubgroups(item, lsi, collectionViewGroupInternal, level + 1, loading);
					return;
				}
				i++;
			}
			collectionViewGroupInternal = new CollectionViewGroupInternal(name, group, false);
			this.InitializeGroup(collectionViewGroupInternal, group.GroupBy, level + 1);
			if (loading)
			{
				group.Add(collectionViewGroupInternal);
				group.LastIndex = i;
			}
			else
			{
				group.Insert(collectionViewGroupInternal, item, this.ActiveComparer);
			}
			group.AddSubgroupToMap(groupNameKey, collectionViewGroupInternal);
			this.AddToSubgroups(item, lsi, collectionViewGroupInternal, level + 1, loading);
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0014E0A8 File Offset: 0x0014D0A8
		private void MoveWithinSubgroups(object item, CollectionViewGroupInternal group, int level, IList list, int oldIndex, int newIndex)
		{
			object groupName = this.GetGroupName(item, group.GroupBy, level);
			if (groupName == CollectionViewGroupRoot.UseAsItemDirectly)
			{
				this.MoveWithinSubgroup(item, group, list, oldIndex, newIndex);
				return;
			}
			ICollection collection;
			if ((collection = (groupName as ICollection)) == null)
			{
				this.MoveWithinSubgroup(item, group, level, groupName, list, oldIndex, newIndex);
				return;
			}
			foreach (object name in collection)
			{
				this.MoveWithinSubgroup(item, group, level, name, list, oldIndex, newIndex);
			}
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0014E144 File Offset: 0x0014D144
		private void MoveWithinSubgroup(object item, CollectionViewGroupInternal group, int level, object name, IList list, int oldIndex, int newIndex)
		{
			object groupNameKey = this.GetGroupNameKey(name, group);
			CollectionViewGroupInternal collectionViewGroupInternal;
			if ((collectionViewGroupInternal = group.GetSubgroupFromMap(groupNameKey)) != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
			{
				this.MoveWithinSubgroups(item, collectionViewGroupInternal, level + 1, list, oldIndex, newIndex);
				return;
			}
			int i = 0;
			int count = group.Items.Count;
			while (i < count)
			{
				collectionViewGroupInternal = (group.Items[i] as CollectionViewGroupInternal);
				if (collectionViewGroupInternal != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
				{
					group.AddSubgroupToMap(groupNameKey, collectionViewGroupInternal);
					this.MoveWithinSubgroups(item, collectionViewGroupInternal, level + 1, list, oldIndex, newIndex);
					return;
				}
				i++;
			}
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0014E1E9 File Offset: 0x0014D1E9
		private void MoveWithinSubgroup(object item, CollectionViewGroupInternal group, IList list, int oldIndex, int newIndex)
		{
			if (group.Move(item, list, ref oldIndex, ref newIndex))
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
			}
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0014E20C File Offset: 0x0014D20C
		private object GetGroupNameKey(object name, CollectionViewGroupInternal group)
		{
			object result = name;
			PropertyGroupDescription propertyGroupDescription = group.GroupBy as PropertyGroupDescription;
			if (propertyGroupDescription != null)
			{
				string text = name as string;
				if (text != null)
				{
					if (propertyGroupDescription.StringComparison == StringComparison.OrdinalIgnoreCase || propertyGroupDescription.StringComparison == StringComparison.InvariantCultureIgnoreCase)
					{
						text = text.ToUpperInvariant();
					}
					else if (propertyGroupDescription.StringComparison == StringComparison.CurrentCultureIgnoreCase)
					{
						text = text.ToUpper(CultureInfo.CurrentCulture);
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0014E268 File Offset: 0x0014D268
		private bool RemoveFromSubgroups(object item, CollectionViewGroupInternal group, int level)
		{
			bool result = false;
			object groupName = this.GetGroupName(item, group.GroupBy, level);
			ICollection collection;
			if (groupName == CollectionViewGroupRoot.UseAsItemDirectly)
			{
				result = this.RemoveFromGroupDirectly(group, item);
			}
			else if ((collection = (groupName as ICollection)) == null)
			{
				if (this.RemoveFromSubgroup(item, group, level, groupName))
				{
					result = true;
				}
			}
			else
			{
				foreach (object name in collection)
				{
					if (this.RemoveFromSubgroup(item, group, level, name))
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0014E304 File Offset: 0x0014D304
		private bool RemoveFromSubgroup(object item, CollectionViewGroupInternal group, int level, object name)
		{
			object groupNameKey = this.GetGroupNameKey(name, group);
			CollectionViewGroupInternal collectionViewGroupInternal;
			if ((collectionViewGroupInternal = group.GetSubgroupFromMap(groupNameKey)) != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
			{
				return this.RemoveFromSubgroups(item, collectionViewGroupInternal, level + 1);
			}
			int i = 0;
			int count = group.Items.Count;
			while (i < count)
			{
				collectionViewGroupInternal = (group.Items[i] as CollectionViewGroupInternal);
				if (collectionViewGroupInternal != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
				{
					return this.RemoveFromSubgroups(item, collectionViewGroupInternal, level + 1);
				}
				i++;
			}
			return true;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0014E398 File Offset: 0x0014D398
		private bool RemoveFromGroupDirectly(CollectionViewGroupInternal group, object item)
		{
			int num = group.Remove(item, true);
			if (num >= 0)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, num));
				return false;
			}
			return true;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0014E3C4 File Offset: 0x0014D3C4
		private void RemoveItemFromSubgroupsByExhaustiveSearch(CollectionViewGroupInternal group, object item)
		{
			if (this.RemoveFromGroupDirectly(group, item))
			{
				for (int i = group.Items.Count - 1; i >= 0; i--)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = group.Items[i] as CollectionViewGroupInternal;
					if (collectionViewGroupInternal != null)
					{
						this.RemoveItemFromSubgroupsByExhaustiveSearch(collectionViewGroupInternal, item);
					}
				}
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0014E410 File Offset: 0x0014D410
		private object GetGroupName(object item, GroupDescription groupDescription, int level)
		{
			if (groupDescription != null)
			{
				return groupDescription.GroupNameFromItem(item, level, this.Culture);
			}
			return CollectionViewGroupRoot.UseAsItemDirectly;
		}

		// Token: 0x04000B82 RID: 2946
		private CollectionView _view;

		// Token: 0x04000B83 RID: 2947
		private IComparer _comparer;

		// Token: 0x04000B84 RID: 2948
		private bool _isDataInGroupOrder;

		// Token: 0x04000B85 RID: 2949
		private ObservableCollection<GroupDescription> _groupBy = new ObservableCollection<GroupDescription>();

		// Token: 0x04000B86 RID: 2950
		private GroupDescriptionSelectorCallback _groupBySelector;

		// Token: 0x04000B87 RID: 2951
		private static GroupDescription _topLevelGroupDescription;

		// Token: 0x04000B88 RID: 2952
		private static readonly object UseAsItemDirectly = new NamedObject("UseAsItemDirectly");

		// Token: 0x020009E9 RID: 2537
		private class GroupTreeNode
		{
			// Token: 0x17001DB4 RID: 7604
			// (get) Token: 0x06008434 RID: 33844 RVA: 0x00325460 File Offset: 0x00324460
			// (set) Token: 0x06008435 RID: 33845 RVA: 0x00325468 File Offset: 0x00324468
			public CollectionViewGroupRoot.GroupTreeNode FirstChild { get; set; }

			// Token: 0x17001DB5 RID: 7605
			// (get) Token: 0x06008436 RID: 33846 RVA: 0x00325471 File Offset: 0x00324471
			// (set) Token: 0x06008437 RID: 33847 RVA: 0x00325479 File Offset: 0x00324479
			public CollectionViewGroupRoot.GroupTreeNode Sibling { get; set; }

			// Token: 0x17001DB6 RID: 7606
			// (get) Token: 0x06008438 RID: 33848 RVA: 0x00325482 File Offset: 0x00324482
			// (set) Token: 0x06008439 RID: 33849 RVA: 0x0032548A File Offset: 0x0032448A
			public CollectionViewGroupInternal Group { get; set; }

			// Token: 0x17001DB7 RID: 7607
			// (get) Token: 0x0600843A RID: 33850 RVA: 0x00325493 File Offset: 0x00324493
			// (set) Token: 0x0600843B RID: 33851 RVA: 0x0032549B File Offset: 0x0032449B
			public bool ContainsItem { get; set; }

			// Token: 0x17001DB8 RID: 7608
			// (get) Token: 0x0600843C RID: 33852 RVA: 0x003254A4 File Offset: 0x003244A4
			// (set) Token: 0x0600843D RID: 33853 RVA: 0x003254AC File Offset: 0x003244AC
			public bool ContainsItemDirectly { get; set; }
		}

		// Token: 0x020009EA RID: 2538
		private class TopLevelGroupDescription : GroupDescription
		{
			// Token: 0x06008440 RID: 33856 RVA: 0x0012F160 File Offset: 0x0012E160
			public override object GroupNameFromItem(object item, int level, CultureInfo culture)
			{
				throw new NotSupportedException();
			}
		}
	}
}
