using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200022B RID: 555
	internal class LiveShapingItem : DependencyObject
	{
		// Token: 0x060014F8 RID: 5368 RVA: 0x00153E30 File Offset: 0x00152E30
		internal LiveShapingItem(object item, LiveShapingList list, bool filtered = false, LiveShapingBlock block = null, bool oneTime = false)
		{
			this._block = block;
			list.InitializeItem(this, item, filtered, oneTime);
			this.ForwardChanges = !oneTime;
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060014F9 RID: 5369 RVA: 0x00153E56 File Offset: 0x00152E56
		// (set) Token: 0x060014FA RID: 5370 RVA: 0x00153E5E File Offset: 0x00152E5E
		internal object Item
		{
			get
			{
				return this._item;
			}
			set
			{
				this._item = value;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060014FB RID: 5371 RVA: 0x00153E67 File Offset: 0x00152E67
		// (set) Token: 0x060014FC RID: 5372 RVA: 0x00153E6F File Offset: 0x00152E6F
		internal LiveShapingBlock Block
		{
			get
			{
				return this._block;
			}
			set
			{
				this._block = value;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060014FD RID: 5373 RVA: 0x00153E78 File Offset: 0x00152E78
		private LiveShapingList List
		{
			get
			{
				return this.Block.List;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060014FE RID: 5374 RVA: 0x00153E85 File Offset: 0x00152E85
		// (set) Token: 0x060014FF RID: 5375 RVA: 0x00153E8E File Offset: 0x00152E8E
		internal bool IsSortDirty
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsSortDirty);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsSortDirty, value);
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x00153E98 File Offset: 0x00152E98
		// (set) Token: 0x06001501 RID: 5377 RVA: 0x00153EA1 File Offset: 0x00152EA1
		internal bool IsSortPendingClean
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsSortPendingClean);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsSortPendingClean, value);
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x00153EAB File Offset: 0x00152EAB
		// (set) Token: 0x06001503 RID: 5379 RVA: 0x00153EB4 File Offset: 0x00152EB4
		internal bool IsFilterDirty
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsFilterDirty);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsFilterDirty, value);
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x00153EBE File Offset: 0x00152EBE
		// (set) Token: 0x06001505 RID: 5381 RVA: 0x00153EC7 File Offset: 0x00152EC7
		internal bool IsGroupDirty
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsGroupDirty);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsGroupDirty, value);
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x00153ED1 File Offset: 0x00152ED1
		// (set) Token: 0x06001507 RID: 5383 RVA: 0x00153EDB File Offset: 0x00152EDB
		internal bool FailsFilter
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.FailsFilter);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.FailsFilter, value);
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x00153EE6 File Offset: 0x00152EE6
		// (set) Token: 0x06001509 RID: 5385 RVA: 0x00153EF0 File Offset: 0x00152EF0
		internal bool ForwardChanges
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.ForwardChanges);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.ForwardChanges, value);
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x00153EFB File Offset: 0x00152EFB
		// (set) Token: 0x0600150B RID: 5387 RVA: 0x00153F05 File Offset: 0x00152F05
		internal bool IsDeleted
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsDeleted);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsDeleted, value);
			}
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00153F10 File Offset: 0x00152F10
		internal void FindPosition(out RBFinger<LiveShapingItem> oldFinger, out RBFinger<LiveShapingItem> newFinger, Comparison<LiveShapingItem> comparison)
		{
			this.Block.FindPosition(this, out oldFinger, out newFinger, comparison);
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00153F21 File Offset: 0x00152F21
		internal RBFinger<LiveShapingItem> GetFinger()
		{
			return this.Block.GetFinger(this);
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x00153F2F File Offset: 0x00152F2F
		// (set) Token: 0x0600150F RID: 5391 RVA: 0x00153F41 File Offset: 0x00152F41
		internal int StartingIndex
		{
			get
			{
				return (int)base.GetValue(LiveShapingItem.StartingIndexProperty);
			}
			set
			{
				base.SetValue(LiveShapingItem.StartingIndexProperty, value);
			}
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00153F54 File Offset: 0x00152F54
		internal int GetAndClearStartingIndex()
		{
			int startingIndex = this.StartingIndex;
			base.ClearValue(LiveShapingItem.StartingIndexProperty);
			return startingIndex;
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x00153F68 File Offset: 0x00152F68
		internal void SetBinding(string path, DependencyProperty dp, bool oneTime = false, bool enableXT = false)
		{
			if (enableXT && oneTime)
			{
				enableXT = false;
			}
			if (!base.LookupEntry(dp.GlobalIndex).Found)
			{
				if (!string.IsNullOrEmpty(path))
				{
					Binding binding;
					if (SystemXmlHelper.IsXmlNode(this._item))
					{
						binding = new Binding();
						binding.XPath = path;
					}
					else
					{
						binding = new Binding(path);
					}
					binding.Source = this._item;
					if (oneTime)
					{
						binding.Mode = BindingMode.OneTime;
					}
					BindingExpressionBase bindingExpressionBase = binding.CreateBindingExpression(this, dp);
					if (enableXT)
					{
						bindingExpressionBase.TargetWantsCrossThreadNotifications = true;
					}
					base.SetValue(dp, bindingExpressionBase);
					return;
				}
				if (!oneTime)
				{
					INotifyPropertyChanged notifyPropertyChanged = this.Item as INotifyPropertyChanged;
					if (notifyPropertyChanged != null)
					{
						PropertyChangedEventManager.AddHandler(notifyPropertyChanged, new EventHandler<PropertyChangedEventArgs>(this.OnPropertyChanged), string.Empty);
					}
				}
			}
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0015401E File Offset: 0x0015301E
		internal object GetValue(string path, DependencyProperty dp)
		{
			if (!string.IsNullOrEmpty(path))
			{
				this.SetBinding(path, dp, false, false);
				return base.GetValue(dp);
			}
			return this.Item;
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00154040 File Offset: 0x00153040
		internal void Clear()
		{
			this.List.ClearItem(this);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0015404E File Offset: 0x0015304E
		internal void OnCrossThreadPropertyChange(DependencyProperty dp)
		{
			this.List.OnItemPropertyChangedCrossThread(this, dp);
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00154060 File Offset: 0x00153060
		internal void AddParentGroup(CollectionViewGroupInternal group)
		{
			object value = base.GetValue(LiveShapingItem.ParentGroupsProperty);
			if (value == null)
			{
				base.SetValue(LiveShapingItem.ParentGroupsProperty, group);
				return;
			}
			List<CollectionViewGroupInternal> list;
			if ((list = (value as List<CollectionViewGroupInternal>)) == null)
			{
				list = new List<CollectionViewGroupInternal>(2);
				list.Add(value as CollectionViewGroupInternal);
				list.Add(group);
				base.SetValue(LiveShapingItem.ParentGroupsProperty, list);
				return;
			}
			list.Add(group);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x001540C4 File Offset: 0x001530C4
		internal void RemoveParentGroup(CollectionViewGroupInternal group)
		{
			object value = base.GetValue(LiveShapingItem.ParentGroupsProperty);
			List<CollectionViewGroupInternal> list = value as List<CollectionViewGroupInternal>;
			if (list == null)
			{
				if (value == group)
				{
					base.ClearValue(LiveShapingItem.ParentGroupsProperty);
					return;
				}
			}
			else
			{
				list.Remove(group);
				if (list.Count == 1)
				{
					base.SetValue(LiveShapingItem.ParentGroupsProperty, list[0]);
				}
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001517 RID: 5399 RVA: 0x0015411A File Offset: 0x0015311A
		internal List<CollectionViewGroupInternal> ParentGroups
		{
			get
			{
				return base.GetValue(LiveShapingItem.ParentGroupsProperty) as List<CollectionViewGroupInternal>;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x0015412C File Offset: 0x0015312C
		internal CollectionViewGroupInternal ParentGroup
		{
			get
			{
				return base.GetValue(LiveShapingItem.ParentGroupsProperty) as CollectionViewGroupInternal;
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0015413E File Offset: 0x0015313E
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.ForwardChanges)
			{
				this.List.OnItemPropertyChanged(this, e.Property);
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0015415B File Offset: 0x0015315B
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.List.OnItemPropertyChanged(this, null);
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0015416A File Offset: 0x0015316A
		private bool TestFlag(LiveShapingItem.PrivateFlags flag)
		{
			return (this._flags & flag) > (LiveShapingItem.PrivateFlags)0;
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00154177 File Offset: 0x00153177
		private void ChangeFlag(LiveShapingItem.PrivateFlags flag, bool value)
		{
			if (value)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x04000BF1 RID: 3057
		private static readonly DependencyProperty StartingIndexProperty = DependencyProperty.Register("StartingIndex", typeof(int), typeof(LiveShapingItem));

		// Token: 0x04000BF2 RID: 3058
		private static readonly DependencyProperty ParentGroupsProperty = DependencyProperty.Register("ParentGroups", typeof(object), typeof(LiveShapingItem));

		// Token: 0x04000BF3 RID: 3059
		private LiveShapingBlock _block;

		// Token: 0x04000BF4 RID: 3060
		private object _item;

		// Token: 0x04000BF5 RID: 3061
		private LiveShapingItem.PrivateFlags _flags;

		// Token: 0x020009F3 RID: 2547
		[Flags]
		private enum PrivateFlags
		{
			// Token: 0x04004022 RID: 16418
			IsSortDirty = 1,
			// Token: 0x04004023 RID: 16419
			IsSortPendingClean = 2,
			// Token: 0x04004024 RID: 16420
			IsFilterDirty = 4,
			// Token: 0x04004025 RID: 16421
			IsGroupDirty = 8,
			// Token: 0x04004026 RID: 16422
			FailsFilter = 16,
			// Token: 0x04004027 RID: 16423
			ForwardChanges = 32,
			// Token: 0x04004028 RID: 16424
			IsDeleted = 64
		}
	}
}
