using System;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace MS.Internal.Data
{
	// Token: 0x0200022F RID: 559
	internal class LiveShapingTree : RBTree<LiveShapingItem>
	{
		// Token: 0x06001556 RID: 5462 RVA: 0x00154CC0 File Offset: 0x00153CC0
		internal LiveShapingTree(LiveShapingList list)
		{
			this._list = list;
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001557 RID: 5463 RVA: 0x00154CCF File Offset: 0x00153CCF
		internal LiveShapingList List
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x00154CD7 File Offset: 0x00153CD7
		internal LiveShapingBlock PlaceholderBlock
		{
			get
			{
				if (this._placeholderBlock == null)
				{
					this._placeholderBlock = new LiveShapingBlock(false);
					this._placeholderBlock.Parent = this;
				}
				return this._placeholderBlock;
			}
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00154CFF File Offset: 0x00153CFF
		internal override RBNode<LiveShapingItem> NewNode()
		{
			return new LiveShapingBlock();
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x00154D08 File Offset: 0x00153D08
		internal void Move(int oldIndex, int newIndex)
		{
			LiveShapingItem item = base[oldIndex];
			base.RemoveAt(oldIndex);
			base.Insert(newIndex, item);
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x00154D2C File Offset: 0x00153D2C
		internal void RestoreLiveSortingByInsertionSort(Action<NotifyCollectionChangedEventArgs, int, int> RaiseMoveEvent)
		{
			RBFinger<LiveShapingItem> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				LiveShapingItem item = finger.Item;
				item.IsSortDirty = false;
				item.IsSortPendingClean = false;
				RBFinger<LiveShapingItem> newFinger = base.LocateItem(finger, base.Comparison);
				int index = finger.Index;
				int index2 = newFinger.Index;
				if (index != index2)
				{
					base.ReInsert(ref finger, newFinger);
					RaiseMoveEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item.Item, index, index2), index, index2);
				}
				finger = ++finger;
			}
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x00154DB0 File Offset: 0x00153DB0
		internal void FindPosition(LiveShapingItem lsi, out int oldIndex, out int newIndex)
		{
			RBFinger<LiveShapingItem> rbfinger;
			RBFinger<LiveShapingItem> rbfinger2;
			lsi.FindPosition(out rbfinger, out rbfinger2, base.Comparison);
			oldIndex = rbfinger.Index;
			newIndex = rbfinger2.Index;
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x00154DE0 File Offset: 0x00153DE0
		internal void ReplaceAt(int index, object item)
		{
			RBFinger<LiveShapingItem> rbfinger = base.FindIndex(index, true);
			rbfinger.Item.Clear();
			rbfinger.Node.SetItemAt(rbfinger.Offset, new LiveShapingItem(item, this.List, false, null, false));
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00154E28 File Offset: 0x00153E28
		internal LiveShapingItem FindItem(object item)
		{
			RBFinger<LiveShapingItem> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				if (ItemsControl.EqualsEx(finger.Item.Item, item))
				{
					return finger.Item;
				}
				finger = ++finger;
			}
			return null;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00154E70 File Offset: 0x00153E70
		public override int IndexOf(LiveShapingItem lsi)
		{
			RBFinger<LiveShapingItem> finger = lsi.GetFinger();
			if (!finger.Found)
			{
				return -1;
			}
			return finger.Index;
		}

		// Token: 0x04000C0D RID: 3085
		private LiveShapingList _list;

		// Token: 0x04000C0E RID: 3086
		private LiveShapingBlock _placeholderBlock;
	}
}
