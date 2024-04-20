using System;
using System.ComponentModel;

namespace MS.Internal.Data
{
	// Token: 0x0200023E RID: 574
	internal class RBNode<T> : INotifyPropertyChanged
	{
		// Token: 0x060015EC RID: 5612 RVA: 0x001581CB File Offset: 0x001571CB
		public RBNode()
		{
			this._data = new T[64];
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		protected RBNode(bool b)
		{
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060015EE RID: 5614 RVA: 0x001581E0 File Offset: 0x001571E0
		// (set) Token: 0x060015EF RID: 5615 RVA: 0x001581E8 File Offset: 0x001571E8
		public RBNode<T> LeftChild { get; set; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060015F0 RID: 5616 RVA: 0x001581F1 File Offset: 0x001571F1
		// (set) Token: 0x060015F1 RID: 5617 RVA: 0x001581F9 File Offset: 0x001571F9
		public RBNode<T> RightChild { get; set; }

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x00158202 File Offset: 0x00157202
		// (set) Token: 0x060015F3 RID: 5619 RVA: 0x0015820A File Offset: 0x0015720A
		public RBNode<T> Parent { get; set; }

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060015F4 RID: 5620 RVA: 0x00158213 File Offset: 0x00157213
		// (set) Token: 0x060015F5 RID: 5621 RVA: 0x0015821B File Offset: 0x0015721B
		public bool IsRed { get; set; }

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public virtual bool HasData
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00158224 File Offset: 0x00157224
		// (set) Token: 0x060015F8 RID: 5624 RVA: 0x0015822C File Offset: 0x0015722C
		public int Size
		{
			get
			{
				return this._size;
			}
			set
			{
				this._size = value;
				this.OnPropertyChanged("Size");
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060015F9 RID: 5625 RVA: 0x00158240 File Offset: 0x00157240
		// (set) Token: 0x060015FA RID: 5626 RVA: 0x00158248 File Offset: 0x00157248
		public int LeftSize
		{
			get
			{
				return this._leftSize;
			}
			set
			{
				this._leftSize = value;
				this.OnPropertyChanged("LeftSize");
			}
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0015825C File Offset: 0x0015725C
		public T GetItemAt(int offset)
		{
			return this._data[offset];
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0015826A File Offset: 0x0015726A
		public virtual T SetItemAt(int offset, T x)
		{
			this._data[offset] = x;
			return x;
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0015827A File Offset: 0x0015727A
		public int OffsetOf(T x)
		{
			return Array.IndexOf<T>(this._data, x);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00158288 File Offset: 0x00157288
		internal RBNode<T> GetSuccessor()
		{
			RBNode<T> rbnode2;
			if (this.RightChild == null)
			{
				RBNode<T> rbnode = this;
				rbnode2 = rbnode.Parent;
				while (rbnode2.RightChild == rbnode)
				{
					rbnode = rbnode2;
					rbnode2 = rbnode.Parent;
				}
				return rbnode2;
			}
			rbnode2 = this.RightChild;
			for (RBNode<T> rbnode = rbnode2.LeftChild; rbnode != null; rbnode = rbnode2.LeftChild)
			{
				rbnode2 = rbnode;
			}
			return rbnode2;
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x001582DC File Offset: 0x001572DC
		internal RBNode<T> GetPredecessor()
		{
			RBNode<T> rbnode2;
			if (this.LeftChild == null)
			{
				RBNode<T> rbnode = this;
				rbnode2 = rbnode.Parent;
				while (rbnode2 != null && rbnode2.LeftChild == rbnode)
				{
					rbnode = rbnode2;
					rbnode2 = rbnode.Parent;
				}
				return rbnode2;
			}
			rbnode2 = this.LeftChild;
			for (RBNode<T> rbnode = rbnode2.RightChild; rbnode != null; rbnode = rbnode2.RightChild)
			{
				rbnode2 = rbnode;
			}
			return rbnode2;
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00158330 File Offset: 0x00157330
		protected RBFinger<T> FindIndex(int index, bool exists = true)
		{
			int num = exists ? 1 : 0;
			RBFinger<T> result;
			if (index + num <= this.LeftSize)
			{
				if (this.LeftChild == null)
				{
					result = new RBFinger<T>
					{
						Node = this,
						Offset = 0,
						Index = 0,
						Found = false
					};
				}
				else
				{
					result = this.LeftChild.FindIndex(index, exists);
				}
			}
			else if (index < this.LeftSize + this.Size)
			{
				result = new RBFinger<T>
				{
					Node = this,
					Offset = index - this.LeftSize,
					Index = index,
					Found = true
				};
			}
			else if (this.RightChild == null)
			{
				result = new RBFinger<T>
				{
					Node = this,
					Offset = this.Size,
					Index = this.LeftSize + this.Size,
					Found = false
				};
			}
			else
			{
				result = this.RightChild.FindIndex(index - this.LeftSize - this.Size, exists);
				result.Index += this.LeftSize + this.Size;
			}
			return result;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0015845C File Offset: 0x0015745C
		protected RBFinger<T> Find(T x, Comparison<T> comparison)
		{
			int num = (this._data != null) ? comparison(x, this.GetItemAt(0)) : -1;
			RBFinger<T> result;
			int compHigh;
			if (num <= 0)
			{
				if (this.LeftChild == null)
				{
					result = new RBFinger<T>
					{
						Node = this,
						Offset = 0,
						Index = 0,
						Found = (num == 0)
					};
				}
				else
				{
					result = this.LeftChild.Find(x, comparison);
					if (num == 0 && !result.Found)
					{
						result = new RBFinger<T>
						{
							Node = this,
							Offset = 0,
							Index = this.LeftSize,
							Found = true
						};
					}
				}
			}
			else if ((compHigh = comparison(x, this.GetItemAt(this.Size - 1))) <= 0)
			{
				bool found;
				int num2 = this.BinarySearch(x, 1, this.Size - 1, comparison, compHigh, out found);
				result = new RBFinger<T>
				{
					Node = this,
					Offset = num2,
					Index = this.LeftSize + num2,
					Found = found
				};
			}
			else if (this.RightChild == null)
			{
				result = new RBFinger<T>
				{
					Node = this,
					Offset = this.Size,
					Index = this.LeftSize + this.Size
				};
			}
			else
			{
				result = this.RightChild.Find(x, comparison);
				result.Index += this.LeftSize + this.Size;
			}
			return result;
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x001585E8 File Offset: 0x001575E8
		protected RBFinger<T> BoundedSearch(T x, int low, int high, Comparison<T> comparison)
		{
			RBNode<T> rbnode = this.LeftChild;
			RBNode<T> rbnode2 = this.RightChild;
			int num = 0;
			int num2 = this.Size;
			int num3;
			if (high <= this.LeftSize)
			{
				num3 = -1;
			}
			else
			{
				if (low >= this.LeftSize)
				{
					rbnode = null;
					num = low - this.LeftSize;
				}
				num3 = ((num < this.Size) ? comparison(x, this.GetItemAt(num)) : 1);
			}
			RBFinger<T> result;
			if (num3 <= 0)
			{
				if (rbnode == null)
				{
					result = new RBFinger<T>
					{
						Node = this,
						Offset = num,
						Index = num,
						Found = (num3 == 0)
					};
				}
				else
				{
					result = rbnode.BoundedSearch(x, low, high, comparison);
					if (num3 == 0 && !result.Found)
					{
						result = new RBFinger<T>
						{
							Node = this,
							Offset = 0,
							Index = this.LeftSize,
							Found = true
						};
					}
				}
				return result;
			}
			int num4;
			if (this.LeftSize + this.Size <= low)
			{
				num4 = 1;
			}
			else
			{
				if (this.LeftSize + this.Size >= high)
				{
					rbnode2 = null;
					num2 = high - this.LeftSize;
				}
				num4 = comparison(x, this.GetItemAt(num2 - 1));
			}
			if (num4 > 0)
			{
				if (rbnode2 == null)
				{
					result = new RBFinger<T>
					{
						Node = this,
						Offset = num2,
						Index = this.LeftSize + num2,
						Found = false
					};
				}
				else
				{
					int num5 = this.LeftSize + this.Size;
					result = rbnode2.BoundedSearch(x, low - num5, high - num5, comparison);
					result.Index += num5;
				}
				return result;
			}
			bool found;
			int num6 = this.BinarySearch(x, num + 1, num2 - 1, comparison, num4, out found);
			result = new RBFinger<T>
			{
				Node = this,
				Offset = num6,
				Index = this.LeftSize + num6,
				Found = found
			};
			return result;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x001587D4 File Offset: 0x001577D4
		private int BinarySearch(T x, int low, int high, Comparison<T> comparison, int compHigh, out bool found)
		{
			while (high - low > 3)
			{
				int num = (high + low) / 2;
				int num2 = comparison(x, this.GetItemAt(num));
				if (num2 <= 0)
				{
					compHigh = num2;
					high = num;
				}
				else
				{
					low = num + 1;
				}
			}
			int num3 = 0;
			while (low < high)
			{
				num3 = comparison(x, this.GetItemAt(low));
				if (num3 <= 0)
				{
					break;
				}
				low++;
			}
			if (low == high)
			{
				num3 = compHigh;
			}
			found = (num3 == 0);
			return low;
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x00158840 File Offset: 0x00157840
		protected RBFinger<T> LocateItem(RBFinger<T> finger, Comparison<T> comparison)
		{
			RBNode<T> rbnode = finger.Node;
			int num = finger.Index - finger.Offset;
			T itemAt = rbnode.GetItemAt(finger.Offset);
			for (int i = finger.Offset - 1; i >= 0; i--)
			{
				if (comparison(itemAt, rbnode.GetItemAt(i)) >= 0)
				{
					return new RBFinger<T>
					{
						Node = rbnode,
						Offset = i + 1,
						Index = num + i + 1
					};
				}
			}
			RBNode<T> rbnode2 = rbnode;
			for (RBNode<T> parent = rbnode2.Parent; parent != null; parent = rbnode2.Parent)
			{
				while (parent != null && rbnode2 == parent.LeftChild)
				{
					rbnode2 = parent;
					parent = rbnode2.Parent;
				}
				if (parent == null || comparison(itemAt, parent.GetItemAt(parent.Size - 1)) >= 0)
				{
					break;
				}
				num = num - rbnode.LeftSize - parent.Size;
				if (comparison(itemAt, parent.GetItemAt(0)) >= 0)
				{
					bool flag;
					int num2 = parent.BinarySearch(itemAt, 1, parent.Size - 1, comparison, -1, out flag);
					return new RBFinger<T>
					{
						Node = parent,
						Offset = num2,
						Index = num + num2
					};
				}
				rbnode2 = (rbnode = parent);
			}
			if (rbnode.LeftChild != null)
			{
				RBFinger<T> result = rbnode.LeftChild.Find(itemAt, comparison);
				if (result.Offset == result.Node.Size)
				{
					result = new RBFinger<T>
					{
						Node = result.Node.GetSuccessor(),
						Offset = 0,
						Index = result.Index
					};
				}
				return result;
			}
			return new RBFinger<T>
			{
				Node = rbnode,
				Offset = 0,
				Index = num
			};
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00158A0E File Offset: 0x00157A0E
		protected virtual void Copy(RBNode<T> sourceNode, int sourceOffset, RBNode<T> destNode, int destOffset, int count)
		{
			Array.Copy(sourceNode._data, sourceOffset, destNode._data, destOffset, count);
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00158A28 File Offset: 0x00157A28
		protected void ReInsert(ref RBFinger<T> oldFinger, RBFinger<T> newFinger)
		{
			RBNode<T> node = oldFinger.Node;
			RBNode<T> node2 = newFinger.Node;
			int offset = oldFinger.Offset;
			int offset2 = newFinger.Offset;
			T itemAt = node.GetItemAt(oldFinger.Offset);
			if (node == node2)
			{
				int num = offset - offset2;
				if (num != 0)
				{
					this.Copy(node, offset2, node, offset2 + 1, num);
					node.SetItemAt(offset2, itemAt);
					return;
				}
			}
			else
			{
				if (node2.Size < 64)
				{
					node2.InsertAt(offset2, itemAt, null, null);
					node.RemoveAt(ref oldFinger);
					return;
				}
				RBNode<T> rbnode = node2.GetSuccessor();
				if (rbnode == node)
				{
					T itemAt2 = node2.GetItemAt(63);
					this.Copy(node2, offset2, node2, offset2 + 1, 64 - offset2 - 1);
					node2.SetItemAt(offset2, itemAt);
					this.Copy(node, 0, node, 1, offset);
					node.SetItemAt(0, itemAt2);
					return;
				}
				if (rbnode.Size < 64)
				{
					node2.InsertAt(offset2, itemAt, rbnode, null);
				}
				else
				{
					RBNode<T> succsucc = rbnode;
					rbnode = this.InsertNodeAfter(node2);
					node2.InsertAt(offset2, itemAt, rbnode, succsucc);
				}
				node.RemoveAt(ref oldFinger);
			}
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x00158B2C File Offset: 0x00157B2C
		protected void RemoveAt(ref RBFinger<T> finger)
		{
			RBNode<T> node = finger.Node;
			int offset = finger.Offset;
			this.Copy(node, offset + 1, node, offset, node.Size - offset - 1);
			node.ChangeSize(-1);
			node.SetItemAt(node.Size, default(T));
			if (node.Size == 0)
			{
				finger.Node = node.GetSuccessor();
				finger.Offset = 0;
				int index;
				node.GetRootAndIndex(node, out index).RemoveNode(index);
			}
			finger.Offset--;
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x00158BB4 File Offset: 0x00157BB4
		protected RBNode<T> InsertNodeAfter(RBNode<T> node)
		{
			int num;
			return this.GetRootAndIndex(node, out num).InsertNode(num + node.Size);
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x00158BD8 File Offset: 0x00157BD8
		protected RBTree<T> GetRoot(RBNode<T> node)
		{
			for (RBNode<T> parent = node.Parent; parent != null; parent = node.Parent)
			{
				node = parent;
			}
			return (RBTree<T>)node;
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00158C04 File Offset: 0x00157C04
		protected RBTree<T> GetRootAndIndex(RBNode<T> node, out int index)
		{
			index = node.LeftSize;
			for (RBNode<T> parent = node.Parent; parent != null; parent = node.Parent)
			{
				if (node == parent.RightChild)
				{
					index += parent.LeftSize + parent.Size;
				}
				node = parent;
			}
			return (RBTree<T>)node;
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00158C50 File Offset: 0x00157C50
		internal void InsertAt(int offset, T x, RBNode<T> successor = null, RBNode<T> succsucc = null)
		{
			if (this.Size < 64)
			{
				this.Copy(this, offset, this, offset + 1, this.Size - offset);
				this.SetItemAt(offset, x);
				this.ChangeSize(1);
				return;
			}
			if (successor.Size != 0)
			{
				int num = (this.Size + successor.Size + 1) / 2;
				if (offset < num)
				{
					this.Copy(successor, 0, successor, 64 - num + 1, successor.Size);
					this.Copy(this, num - 1, successor, 0, 64 - num + 1);
					this.Copy(this, offset, this, offset + 1, num - 1 - offset);
					this.SetItemAt(offset, x);
				}
				else
				{
					this.Copy(successor, 0, successor, 64 - num, successor.Size);
					this.Copy(this, num, successor, 0, 64 - num);
					this.Copy(successor, offset - num, successor, offset - num + 1, successor.Size + 64 - offset);
					successor.SetItemAt(offset - num, x);
				}
				this.ChangeSize(num - 64);
				successor.ChangeSize(64 - num + 1);
				return;
			}
			if (succsucc != null)
			{
				int num2 = 21;
				this.Copy(successor, 0, successor, num2, successor.Size);
				this.Copy(this, 64 - num2, successor, 0, num2);
				this.Copy(succsucc, 0, successor, num2 + successor.Size, num2);
				this.Copy(succsucc, num2, succsucc, 0, 64 - num2);
				if (offset <= 64 - num2)
				{
					this.Copy(this, offset, this, offset + 1, 64 - num2 - offset);
					this.SetItemAt(offset, x);
					this.ChangeSize(1 - num2);
					successor.ChangeSize(num2 + num2);
				}
				else
				{
					this.Copy(successor, offset - (64 - num2), successor, offset - (64 - num2) + 1, successor.Size + num2 + num2 - (offset - (64 - num2)));
					successor.SetItemAt(offset - (64 - num2), x);
					this.ChangeSize(-num2);
					successor.ChangeSize(num2 + num2 + 1);
				}
				succsucc.ChangeSize(-num2);
				return;
			}
			if (offset < 64)
			{
				successor.InsertAt(0, this.GetItemAt(63), null, null);
				this.Copy(this, offset, this, offset + 1, 64 - offset - 1);
				this.SetItemAt(offset, x);
				return;
			}
			successor.InsertAt(0, x, null, null);
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x00158E5C File Offset: 0x00157E5C
		protected RBNode<T> InsertNode(RBTree<T> root, RBNode<T> parent, RBNode<T> node, int index, out RBNode<T> newNode)
		{
			if (node == null)
			{
				newNode = root.NewNode();
				newNode.Parent = parent;
				newNode.IsRed = true;
				return newNode;
			}
			if (index <= node.LeftSize)
			{
				node.LeftChild = this.InsertNode(root, node, node.LeftChild, index, out newNode);
			}
			else
			{
				node.RightChild = this.InsertNode(root, node, node.RightChild, index - node.LeftSize - node.Size, out newNode);
			}
			node = this.Fixup(node);
			return node;
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00158EE0 File Offset: 0x00157EE0
		protected void ChangeSize(int delta)
		{
			if (delta == 0)
			{
				return;
			}
			for (int i = this.Size + delta; i < this.Size; i++)
			{
				this._data[i] = default(T);
			}
			this.Size += delta;
			RBNode<T> rbnode = this;
			for (RBNode<T> parent = rbnode.Parent; parent != null; parent = rbnode.Parent)
			{
				if (parent.LeftChild == rbnode)
				{
					parent.LeftSize += delta;
				}
				rbnode = parent;
			}
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00158F5C File Offset: 0x00157F5C
		private RBNode<T> Substitute(RBNode<T> node, RBNode<T> sub, RBNode<T> parent)
		{
			sub.LeftChild = node.LeftChild;
			sub.RightChild = node.RightChild;
			sub.LeftSize = node.LeftSize;
			sub.Parent = node.Parent;
			sub.IsRed = node.IsRed;
			if (sub.LeftChild != null)
			{
				sub.LeftChild.Parent = sub;
			}
			if (sub.RightChild != null)
			{
				sub.RightChild.Parent = sub;
			}
			return sub;
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x00158FD0 File Offset: 0x00157FD0
		protected RBNode<T> DeleteNode(RBNode<T> parent, RBNode<T> node, int index)
		{
			if (index < node.LeftSize || (index == node.LeftSize && node.Size > 0))
			{
				if (!this.IsNodeRed(node.LeftChild) && !this.IsNodeRed(node.LeftChild.LeftChild))
				{
					node = this.MoveRedLeft(node);
				}
				node.LeftChild = this.DeleteNode(node, node.LeftChild, index);
			}
			else
			{
				bool flag = index == node.LeftSize;
				if (this.IsNodeRed(node.LeftChild))
				{
					node = node.RotateRight();
					flag = false;
				}
				if (flag && node.RightChild == null)
				{
					return null;
				}
				if (!this.IsNodeRed(node.RightChild) && !this.IsNodeRed(node.RightChild.LeftChild))
				{
					RBNode<T> rbnode = node;
					node = this.MoveRedRight(node);
					flag = (flag && rbnode == node);
				}
				if (flag)
				{
					RBNode<T> sub;
					node.RightChild = this.DeleteLeftmost(node.RightChild, out sub);
					node = this.Substitute(node, sub, parent);
				}
				else
				{
					node.RightChild = this.DeleteNode(node, node.RightChild, index - node.LeftSize - node.Size);
				}
			}
			return this.Fixup(node);
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x001590F0 File Offset: 0x001580F0
		private RBNode<T> DeleteLeftmost(RBNode<T> node, out RBNode<T> leftmost)
		{
			if (node.LeftChild == null)
			{
				leftmost = node;
				return null;
			}
			if (!this.IsNodeRed(node.LeftChild) && !this.IsNodeRed(node.LeftChild.LeftChild))
			{
				node = this.MoveRedLeft(node);
			}
			node.LeftChild = this.DeleteLeftmost(node.LeftChild, out leftmost);
			node.LeftSize -= leftmost.Size;
			return this.Fixup(node);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x00159162 File Offset: 0x00158162
		private bool IsNodeRed(RBNode<T> node)
		{
			return node != null && node.IsRed;
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00159170 File Offset: 0x00158170
		private RBNode<T> RotateLeft()
		{
			RBNode<T> rightChild = this.RightChild;
			rightChild.LeftSize += this.LeftSize + this.Size;
			rightChild.IsRed = this.IsRed;
			rightChild.Parent = this.Parent;
			this.RightChild = rightChild.LeftChild;
			if (this.RightChild != null)
			{
				this.RightChild.Parent = this;
			}
			rightChild.LeftChild = this;
			this.IsRed = true;
			this.Parent = rightChild;
			return rightChild;
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x001591EC File Offset: 0x001581EC
		private RBNode<T> RotateRight()
		{
			RBNode<T> leftChild = this.LeftChild;
			this.LeftSize -= leftChild.LeftSize + leftChild.Size;
			leftChild.IsRed = this.IsRed;
			leftChild.Parent = this.Parent;
			this.LeftChild = leftChild.RightChild;
			if (this.LeftChild != null)
			{
				this.LeftChild.Parent = this;
			}
			leftChild.RightChild = this;
			this.IsRed = true;
			this.Parent = leftChild;
			return leftChild;
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00159268 File Offset: 0x00158268
		private void ColorFlip()
		{
			this.IsRed = !this.IsRed;
			this.LeftChild.IsRed = !this.LeftChild.IsRed;
			this.RightChild.IsRed = !this.RightChild.IsRed;
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x001592B8 File Offset: 0x001582B8
		private RBNode<T> Fixup(RBNode<T> node)
		{
			if (!this.IsNodeRed(node.LeftChild) && this.IsNodeRed(node.RightChild))
			{
				node = node.RotateLeft();
			}
			if (this.IsNodeRed(node.LeftChild) && this.IsNodeRed(node.LeftChild.LeftChild))
			{
				node = node.RotateRight();
			}
			if (this.IsNodeRed(node.LeftChild) && this.IsNodeRed(node.RightChild))
			{
				node.ColorFlip();
			}
			return node;
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00159335 File Offset: 0x00158335
		private RBNode<T> MoveRedRight(RBNode<T> node)
		{
			node.ColorFlip();
			if (this.IsNodeRed(node.LeftChild.LeftChild))
			{
				node = node.RotateRight();
				node.ColorFlip();
			}
			return node;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x0015935F File Offset: 0x0015835F
		private RBNode<T> MoveRedLeft(RBNode<T> node)
		{
			node.ColorFlip();
			if (this.IsNodeRed(node.RightChild.LeftChild))
			{
				node.RightChild = node.RightChild.RotateRight();
				node = node.RotateLeft();
				node.ColorFlip();
			}
			return node;
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06001618 RID: 5656 RVA: 0x0015939C File Offset: 0x0015839C
		// (remove) Token: 0x06001619 RID: 5657 RVA: 0x001593D4 File Offset: 0x001583D4
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600161A RID: 5658 RVA: 0x00159409 File Offset: 0x00158409
		protected void OnPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x04000C48 RID: 3144
		protected const int MaxSize = 64;

		// Token: 0x04000C49 RID: 3145
		protected const int BinarySearchThreshold = 3;

		// Token: 0x04000C4E RID: 3150
		private int _size;

		// Token: 0x04000C4F RID: 3151
		private int _leftSize;

		// Token: 0x04000C51 RID: 3153
		private T[] _data;
	}
}
