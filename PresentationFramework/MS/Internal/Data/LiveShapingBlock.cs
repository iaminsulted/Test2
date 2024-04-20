﻿using System;
using System.Collections.Generic;

namespace MS.Internal.Data
{
	// Token: 0x0200022A RID: 554
	internal class LiveShapingBlock : RBNode<LiveShapingItem>
	{
		// Token: 0x060014EA RID: 5354 RVA: 0x00153700 File Offset: 0x00152700
		internal LiveShapingBlock()
		{
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00153708 File Offset: 0x00152708
		internal LiveShapingBlock(bool b) : base(b)
		{
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x00153711 File Offset: 0x00152711
		private LiveShapingBlock ParentBlock
		{
			get
			{
				return base.Parent as LiveShapingBlock;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0015371E File Offset: 0x0015271E
		private LiveShapingBlock LeftChildBlock
		{
			get
			{
				return (LiveShapingBlock)base.LeftChild;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x0015372B File Offset: 0x0015272B
		private LiveShapingBlock RightChildBlock
		{
			get
			{
				return (LiveShapingBlock)base.RightChild;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x00153738 File Offset: 0x00152738
		internal LiveShapingList List
		{
			get
			{
				return ((LiveShapingTree)base.GetRoot(this)).List;
			}
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0015374B File Offset: 0x0015274B
		public override LiveShapingItem SetItemAt(int offset, LiveShapingItem lsi)
		{
			base.SetItemAt(offset, lsi);
			if (lsi != null)
			{
				lsi.Block = this;
			}
			return lsi;
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x00153764 File Offset: 0x00152764
		protected override void Copy(RBNode<LiveShapingItem> sourceNode, int sourceOffset, RBNode<LiveShapingItem> destNode, int destOffset, int count)
		{
			base.Copy(sourceNode, sourceOffset, destNode, destOffset, count);
			if (sourceNode != destNode)
			{
				LiveShapingBlock block = (LiveShapingBlock)destNode;
				int i = 0;
				while (i < count)
				{
					destNode.GetItemAt(destOffset).Block = block;
					i++;
					destOffset++;
				}
			}
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x001537AC File Offset: 0x001527AC
		internal RBFinger<LiveShapingItem> GetFinger(LiveShapingItem lsi)
		{
			int num = base.OffsetOf(lsi);
			int num2;
			base.GetRootAndIndex(this, out num2);
			return new RBFinger<LiveShapingItem>
			{
				Node = this,
				Offset = num,
				Index = num2 + num,
				Found = true
			};
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x001537F8 File Offset: 0x001527F8
		internal void FindPosition(LiveShapingItem item, out RBFinger<LiveShapingItem> oldFinger, out RBFinger<LiveShapingItem> newFinger, Comparison<LiveShapingItem> comparison)
		{
			int size = base.Size;
			int num = -1;
			int num2 = -1;
			int i;
			for (i = 0; i < size; i++)
			{
				LiveShapingItem itemAt = base.GetItemAt(i);
				if (item == itemAt)
				{
					break;
				}
				if (!itemAt.IsSortDirty)
				{
					num2 = i;
					if (num < 0)
					{
						num = i;
					}
				}
			}
			int j;
			for (j = i + 1; j < size; j++)
			{
				LiveShapingItem itemAt = base.GetItemAt(j);
				if (!itemAt.IsSortDirty)
				{
					break;
				}
			}
			int num3 = j;
			for (int k = size - 1; k > num3; k--)
			{
				LiveShapingItem itemAt = base.GetItemAt(k);
				if (!itemAt.IsSortDirty)
				{
					num3 = k;
				}
			}
			int num4;
			base.GetRootAndIndex(this, out num4);
			oldFinger = new RBFinger<LiveShapingItem>
			{
				Node = this,
				Offset = i,
				Index = num4 + i,
				Found = true
			};
			LiveShapingItem liveShapingItem = (num2 >= 0) ? base.GetItemAt(num2) : null;
			LiveShapingItem liveShapingItem2 = (j < size) ? base.GetItemAt(j) : null;
			int num5;
			int num6;
			if (liveShapingItem != null && (num5 = comparison(item, liveShapingItem)) < 0)
			{
				if (num != num2)
				{
					num5 = comparison(item, base.GetItemAt(num));
				}
				if (num5 >= 0)
				{
					newFinger = this.LocalSearch(item, num + 1, num2, comparison);
					return;
				}
				newFinger = this.SearchLeft(item, num, comparison);
				return;
			}
			else if (liveShapingItem2 != null && (num6 = comparison(item, liveShapingItem2)) > 0)
			{
				if (num3 != j)
				{
					num6 = comparison(item, base.GetItemAt(num3));
				}
				if (num6 <= 0)
				{
					newFinger = this.LocalSearch(item, j + 1, num3, comparison);
					return;
				}
				newFinger = this.SearchRight(item, num3 + 1, comparison);
				return;
			}
			else if (liveShapingItem != null)
			{
				if (liveShapingItem2 != null)
				{
					newFinger = oldFinger;
					return;
				}
				newFinger = this.SearchRight(item, i, comparison);
				return;
			}
			else
			{
				if (liveShapingItem2 != null)
				{
					newFinger = this.SearchLeft(item, i, comparison);
					return;
				}
				newFinger = this.SearchLeft(item, i, comparison);
				if (newFinger.Node == this)
				{
					newFinger = this.SearchRight(item, i, comparison);
				}
				return;
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00153A04 File Offset: 0x00152A04
		private RBFinger<LiveShapingItem> LocalSearch(LiveShapingItem item, int left, int right, Comparison<LiveShapingItem> comparison)
		{
			int num2;
			while (right - left > 3)
			{
				int num = (left + right) / 2;
				num2 = num;
				while (num2 >= left && base.GetItemAt(num2).IsSortDirty)
				{
					num2--;
				}
				if (num2 < left || comparison(base.GetItemAt(num2), item) <= 0)
				{
					left = num + 1;
				}
				else
				{
					right = num2;
				}
			}
			num2 = left;
			while (num2 < right && (base.GetItemAt(num2).IsSortDirty || comparison(item, base.GetItemAt(num2)) > 0))
			{
				num2++;
			}
			int num3;
			base.GetRootAndIndex(this, out num3);
			return new RBFinger<LiveShapingItem>
			{
				Node = this,
				Offset = num2,
				Index = num3 + num2
			};
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00153AB4 File Offset: 0x00152AB4
		private RBFinger<LiveShapingItem> SearchLeft(LiveShapingItem item, int offset, Comparison<LiveShapingItem> comparison)
		{
			LiveShapingBlock node = this;
			List<LiveShapingBlock> list = new List<LiveShapingBlock>();
			list.Add(this.LeftChildBlock);
			LiveShapingBlock liveShapingBlock = this;
			for (LiveShapingBlock parentBlock = liveShapingBlock.ParentBlock; parentBlock != null; parentBlock = liveShapingBlock.ParentBlock)
			{
				if (parentBlock.RightChildBlock == liveShapingBlock)
				{
					int num;
					int num2;
					int num3;
					parentBlock.GetFirstAndLastCleanItems(out num, out num2, out num3);
					if (num >= num3)
					{
						list.Add(parentBlock.LeftChildBlock);
					}
					else
					{
						if (comparison(item, parentBlock.GetItemAt(num2)) > 0)
						{
							break;
						}
						if (comparison(item, parentBlock.GetItemAt(num)) >= 0)
						{
							return parentBlock.LocalSearch(item, num + 1, num2, comparison);
						}
						list.Clear();
						list.Add(parentBlock.LeftChildBlock);
						node = parentBlock;
						offset = num;
					}
				}
				liveShapingBlock = parentBlock;
			}
			Stack<LiveShapingBlock> stack = new Stack<LiveShapingBlock>(list);
			while (stack.Count > 0)
			{
				liveShapingBlock = stack.Pop();
				if (liveShapingBlock != null)
				{
					int num;
					int num2;
					int num3;
					liveShapingBlock.GetFirstAndLastCleanItems(out num, out num2, out num3);
					if (num >= num3)
					{
						stack.Push(liveShapingBlock.LeftChildBlock);
						stack.Push(liveShapingBlock.RightChildBlock);
					}
					else if (comparison(item, liveShapingBlock.GetItemAt(num2)) > 0)
					{
						stack.Clear();
						stack.Push(liveShapingBlock.RightChildBlock);
					}
					else
					{
						if (comparison(item, liveShapingBlock.GetItemAt(num)) >= 0)
						{
							return liveShapingBlock.LocalSearch(item, num + 1, num2, comparison);
						}
						node = liveShapingBlock;
						offset = num;
						stack.Push(liveShapingBlock.LeftChildBlock);
					}
				}
			}
			int num4;
			base.GetRootAndIndex(node, out num4);
			return new RBFinger<LiveShapingItem>
			{
				Node = node,
				Offset = offset,
				Index = num4 + offset
			};
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00153C44 File Offset: 0x00152C44
		private RBFinger<LiveShapingItem> SearchRight(LiveShapingItem item, int offset, Comparison<LiveShapingItem> comparison)
		{
			LiveShapingBlock node = this;
			List<LiveShapingBlock> list = new List<LiveShapingBlock>();
			list.Add(this.RightChildBlock);
			LiveShapingBlock liveShapingBlock = this;
			for (LiveShapingBlock parentBlock = liveShapingBlock.ParentBlock; parentBlock != null; parentBlock = liveShapingBlock.ParentBlock)
			{
				if (parentBlock.LeftChildBlock == liveShapingBlock)
				{
					int num;
					int num2;
					int num3;
					parentBlock.GetFirstAndLastCleanItems(out num, out num2, out num3);
					if (num >= num3)
					{
						list.Add(parentBlock.RightChildBlock);
					}
					else
					{
						if (comparison(item, parentBlock.GetItemAt(num)) < 0)
						{
							break;
						}
						if (comparison(item, parentBlock.GetItemAt(num2)) <= 0)
						{
							return parentBlock.LocalSearch(item, num + 1, num2, comparison);
						}
						list.Clear();
						list.Add(parentBlock.RightChildBlock);
						node = parentBlock;
						offset = num2 + 1;
					}
				}
				liveShapingBlock = parentBlock;
			}
			Stack<LiveShapingBlock> stack = new Stack<LiveShapingBlock>(list);
			while (stack.Count > 0)
			{
				liveShapingBlock = stack.Pop();
				if (liveShapingBlock != null)
				{
					int num;
					int num2;
					int num3;
					liveShapingBlock.GetFirstAndLastCleanItems(out num, out num2, out num3);
					if (num >= num3)
					{
						stack.Push(liveShapingBlock.RightChildBlock);
						stack.Push(liveShapingBlock.LeftChildBlock);
					}
					else if (comparison(item, liveShapingBlock.GetItemAt(num)) < 0)
					{
						stack.Clear();
						stack.Push(liveShapingBlock.LeftChildBlock);
					}
					else
					{
						if (comparison(item, liveShapingBlock.GetItemAt(num2)) <= 0)
						{
							return liveShapingBlock.LocalSearch(item, num + 1, num2, comparison);
						}
						node = liveShapingBlock;
						offset = num2 + 1;
						stack.Push(liveShapingBlock.RightChildBlock);
					}
				}
			}
			int num4;
			base.GetRootAndIndex(node, out num4);
			return new RBFinger<LiveShapingItem>
			{
				Node = node,
				Offset = offset,
				Index = num4 + offset
			};
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00153DD8 File Offset: 0x00152DD8
		private void GetFirstAndLastCleanItems(out int first, out int last, out int size)
		{
			size = base.Size;
			first = 0;
			while (first < size && base.GetItemAt(first).IsSortDirty)
			{
				first++;
			}
			last = size - 1;
			while (last > first && base.GetItemAt(last).IsSortDirty)
			{
				last--;
			}
		}
	}
}
