using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MS.Internal.Data
{
	// Token: 0x0200023F RID: 575
	internal class RBTree<T> : RBNode<T>, IList<T>, ICollection<T>, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x0600161B RID: 5659 RVA: 0x00159425 File Offset: 0x00158425
		public RBTree() : base(false)
		{
			base.Size = 64;
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool HasData
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x0600161D RID: 5661 RVA: 0x00159436 File Offset: 0x00158436
		// (set) Token: 0x0600161E RID: 5662 RVA: 0x0015943E File Offset: 0x0015843E
		public Comparison<T> Comparison
		{
			get
			{
				return this._comparison;
			}
			set
			{
				this._comparison = value;
			}
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00159447 File Offset: 0x00158447
		public RBFinger<T> BoundedSearch(T x, int low, int high)
		{
			return base.BoundedSearch(x, low, high, this.Comparison);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00159458 File Offset: 0x00158458
		public void Insert(T x)
		{
			RBFinger<T> finger = base.Find(x, this.Comparison);
			this.Insert(finger, x, true);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0015947C File Offset: 0x0015847C
		private void Insert(RBFinger<T> finger, T x, bool checkSort = false)
		{
			RBNode<T> rbnode = finger.Node;
			if (rbnode == this)
			{
				rbnode = this.InsertNode(0);
				rbnode.InsertAt(0, x, null, null);
			}
			else if (rbnode.Size < 64)
			{
				rbnode.InsertAt(finger.Offset, x, null, null);
			}
			else
			{
				RBNode<T> rbnode2 = rbnode.GetSuccessor();
				RBNode<T> succsucc = null;
				if (rbnode2.Size >= 64)
				{
					if (rbnode2 != this)
					{
						succsucc = rbnode2;
					}
					rbnode2 = this.InsertNode(finger.Index + rbnode.Size - finger.Offset);
				}
				rbnode.InsertAt(finger.Offset, x, rbnode2, succsucc);
			}
			base.LeftChild.IsRed = false;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00159518 File Offset: 0x00158518
		public void Sort()
		{
			try
			{
				this.QuickSort();
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_IComparerFailed"), innerException);
			}
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00159550 File Offset: 0x00158550
		public void QuickSort()
		{
			if (this.Count > 1)
			{
				RBFinger<T> low = base.FindIndex(0, false);
				RBFinger<T> high = base.FindIndex(this.Count, false);
				this.QuickSort3(low, high);
				this.InsertionSortImpl();
			}
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0015958B File Offset: 0x0015858B
		public void InsertionSort()
		{
			if (this.Count > 1)
			{
				this.InsertionSortImpl();
			}
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0015959C File Offset: 0x0015859C
		private void QuickSort3(RBFinger<T> low, RBFinger<T> high)
		{
			while (high - low > 15)
			{
				RBFinger<T> rbfinger = low;
				RBFinger<T> rbfinger2 = low + 1;
				RBFinger<T> rbfinger3 = high - 1;
				RBFinger<T> rbfinger4 = high;
				RBFinger<T> rbfinger5 = base.FindIndex((low.Index + high.Index) / 2, true);
				int num = this.Comparison(low.Item, rbfinger5.Item);
				if (num < 0)
				{
					num = this.Comparison(rbfinger5.Item, rbfinger3.Item);
					if (num >= 0)
					{
						if (num == 0)
						{
							rbfinger4 = rbfinger3;
						}
						else
						{
							num = this.Comparison(low.Item, rbfinger3.Item);
							if (num < 0)
							{
								this.Exchange(rbfinger5, rbfinger3);
							}
							else if (num == 0)
							{
								this.Exchange(rbfinger5, rbfinger3);
								rbfinger = rbfinger2;
							}
							else
							{
								this.Exchange(low, rbfinger5);
								this.Exchange(low, rbfinger3);
							}
						}
					}
				}
				else if (num == 0)
				{
					num = this.Comparison(low.Item, rbfinger3.Item);
					if (num < 0)
					{
						rbfinger = rbfinger2;
					}
					else if (num == 0)
					{
						rbfinger = rbfinger2;
						rbfinger4 = rbfinger3;
					}
					else
					{
						this.Exchange(low, rbfinger3);
						rbfinger4 = rbfinger3;
					}
				}
				else
				{
					num = this.Comparison(low.Item, rbfinger3.Item);
					if (num < 0)
					{
						this.Exchange(low, rbfinger5);
					}
					else if (num == 0)
					{
						this.Exchange(low, rbfinger5);
						rbfinger4 = rbfinger3;
					}
					else
					{
						num = this.Comparison(rbfinger5.Item, rbfinger3.Item);
						if (num < 0)
						{
							this.Exchange(low, rbfinger5);
							this.Exchange(rbfinger5, rbfinger3);
						}
						else if (num == 0)
						{
							this.Exchange(low, rbfinger3);
							rbfinger = rbfinger2;
						}
						else
						{
							this.Exchange(low, rbfinger3);
						}
					}
				}
				T item = rbfinger5.Item;
				RBFinger<T> rbfinger6 = rbfinger2;
				RBFinger<T> rbfinger7 = rbfinger3;
				for (;;)
				{
					if (rbfinger6 < rbfinger7)
					{
						num = this.Comparison(rbfinger6.Item, item);
						if (num < 0)
						{
							this.Trade(rbfinger, rbfinger2, rbfinger6);
							rbfinger += rbfinger6 - rbfinger2;
							rbfinger6 = (rbfinger2 = ++rbfinger6);
							continue;
						}
						if (num == 0)
						{
							rbfinger6 = ++rbfinger6;
							continue;
						}
					}
					while (rbfinger6 < rbfinger7)
					{
						RBFinger<T> rbfinger8 = rbfinger7 - 1;
						num = this.Comparison(rbfinger8.Item, item);
						if (num < 0)
						{
							break;
						}
						if (num == 0)
						{
							rbfinger7 = --rbfinger7;
						}
						else
						{
							this.Trade(rbfinger7, rbfinger3, rbfinger4);
							rbfinger4 -= rbfinger3 - rbfinger7;
							rbfinger7 = (rbfinger3 = --rbfinger7);
						}
					}
					num = rbfinger7 - rbfinger6;
					if (num == 0)
					{
						goto Block_18;
					}
					if (num != 1)
					{
						if (num == 2)
						{
							goto Block_20;
						}
						this.Exchange(rbfinger6, rbfinger7 - 1);
						this.Trade(rbfinger, rbfinger2, rbfinger6);
						rbfinger += rbfinger6 - rbfinger2;
						rbfinger6 = (rbfinger2 = ++rbfinger6);
						this.Trade(rbfinger7, rbfinger3, rbfinger4);
						rbfinger4 -= rbfinger3 - rbfinger7;
						rbfinger7 = (rbfinger3 = --rbfinger7);
					}
				}
				IL_38B:
				if (rbfinger2 - low < high - rbfinger3)
				{
					this.QuickSort3(low, rbfinger2);
					low = rbfinger3;
					continue;
				}
				this.QuickSort3(rbfinger3, high);
				high = rbfinger2;
				continue;
				Block_18:
				this.Trade(low, rbfinger, rbfinger2);
				rbfinger2 += low - rbfinger;
				this.Trade(rbfinger3, rbfinger4, high);
				rbfinger3 += high - rbfinger4;
				goto IL_38B;
				Block_20:
				this.Trade(low, rbfinger, rbfinger2);
				rbfinger2 += low - rbfinger + 1;
				this.Exchange(rbfinger2 - 1, rbfinger6 + 1);
				if (rbfinger2 > rbfinger6)
				{
					rbfinger6 = ++rbfinger6;
				}
				this.Trade(rbfinger3, rbfinger4, high);
				rbfinger3 += high - rbfinger4 - 1;
				this.Exchange(rbfinger6, rbfinger3);
				goto IL_38B;
			}
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0015996C File Offset: 0x0015896C
		private void Trade(RBFinger<T> left, RBFinger<T> mid, RBFinger<T> right)
		{
			int num = Math.Min(mid - left, right - mid);
			for (int i = 0; i < num; i++)
			{
				right = --right;
				this.Exchange(left, right);
				left = ++left;
			}
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x001599B4 File Offset: 0x001589B4
		private void Exchange(RBFinger<T> f1, RBFinger<T> f2)
		{
			T item = f1.Item;
			f1.SetItem(f2.Item);
			f2.SetItem(item);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x001599E0 File Offset: 0x001589E0
		private void InsertionSortImpl()
		{
			RBFinger<T> finger = base.FindIndex(1, true);
			while (finger.Node != this)
			{
				RBFinger<T> newFinger = base.LocateItem(finger, this.Comparison);
				base.ReInsert(ref finger, newFinger);
				finger = ++finger;
			}
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00159A20 File Offset: 0x00158A20
		internal RBNode<T> InsertNode(int index)
		{
			RBNode<T> result;
			base.LeftChild = base.InsertNode(this, this, base.LeftChild, index, out result);
			return result;
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00159A45 File Offset: 0x00158A45
		internal void RemoveNode(int index)
		{
			base.LeftChild = base.DeleteNode(this, base.LeftChild, index);
			if (base.LeftChild != null)
			{
				base.LeftChild.IsRed = false;
			}
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00159A6F File Offset: 0x00158A6F
		internal virtual RBNode<T> NewNode()
		{
			return new RBNode<T>();
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00159A78 File Offset: 0x00158A78
		internal void ForEach(Action<T> action)
		{
			foreach (T obj in this)
			{
				action(obj);
			}
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00159AC0 File Offset: 0x00158AC0
		internal void ForEachUntil(Func<T, bool> action)
		{
			foreach (T arg in this)
			{
				if (action(arg))
				{
					break;
				}
			}
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x00159B0C File Offset: 0x00158B0C
		internal int IndexOf(T item, Func<T, T, bool> AreEqual)
		{
			if (this.Comparison != null)
			{
				RBFinger<T> finger = base.Find(item, this.Comparison);
				while (finger.Found && !AreEqual(finger.Item, item))
				{
					finger = ++finger;
					finger.Found = (finger.IsValid && this.Comparison(finger.Item, item) == 0);
				}
				if (!finger.Found)
				{
					return -1;
				}
				return finger.Index;
			}
			else
			{
				int result = 0;
				this.ForEachUntil(delegate(T x)
				{
					if (AreEqual(x, item))
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
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x00159BE7 File Offset: 0x00158BE7
		public virtual int IndexOf(T item)
		{
			return this.IndexOf(item, (T x, T y) => ItemsControl.EqualsEx(x, y));
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x00159C10 File Offset: 0x00158C10
		public void Insert(int index, T item)
		{
			this.VerifyIndex(index, 1);
			RBFinger<T> finger = base.FindIndex(index, false);
			this.Insert(finger, item, false);
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00159C38 File Offset: 0x00158C38
		public void RemoveAt(int index)
		{
			this.VerifyIndex(index, 0);
			this.SaveTree();
			int leftSize = base.LeftSize;
			RBFinger<T> rbfinger = base.FindIndex(index, true);
			base.RemoveAt(ref rbfinger);
			if (base.LeftChild != null)
			{
				base.LeftChild.IsRed = false;
			}
			this.Verify(leftSize - 1, true);
		}

		// Token: 0x1700044E RID: 1102
		public T this[int index]
		{
			get
			{
				this.VerifyIndex(index, 0);
				RBFinger<T> rbfinger = base.FindIndex(index, true);
				return rbfinger.Node.GetItemAt(rbfinger.Offset);
			}
			set
			{
				this.VerifyIndex(index, 0);
				RBFinger<T> rbfinger = base.FindIndex(index, true);
				rbfinger.Node.SetItemAt(rbfinger.Offset, value);
			}
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00159CF4 File Offset: 0x00158CF4
		public void Add(T item)
		{
			this.SaveTree();
			int leftSize = base.LeftSize;
			RBNode<T> rbnode = base.LeftChild;
			if (rbnode == null)
			{
				rbnode = this.InsertNode(0);
				rbnode.InsertAt(0, item, null, null);
			}
			else
			{
				while (rbnode.RightChild != null)
				{
					rbnode = rbnode.RightChild;
				}
				if (rbnode.Size < 64)
				{
					rbnode.InsertAt(rbnode.Size, item, null, null);
				}
				else
				{
					rbnode = this.InsertNode(base.LeftSize);
					rbnode.InsertAt(0, item, null, null);
				}
			}
			base.LeftChild.IsRed = false;
			this.Verify(leftSize + 1, false);
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00159D83 File Offset: 0x00158D83
		public void Clear()
		{
			base.LeftChild = null;
			base.LeftSize = 0;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00159D94 File Offset: 0x00158D94
		public bool Contains(T item)
		{
			return base.Find(item, this.Comparison).Found;
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x00159DB8 File Offset: 0x00158DB8
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (arrayIndex + this.Count > array.Length)
			{
				throw new ArgumentException(SR.Get("Argument_InvalidOffLen"));
			}
			foreach (T t in this)
			{
				array[arrayIndex] = t;
				arrayIndex++;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001638 RID: 5688 RVA: 0x00159E40 File Offset: 0x00158E40
		public int Count
		{
			get
			{
				return base.LeftSize;
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001639 RID: 5689 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00159E48 File Offset: 0x00158E48
		public bool Remove(T item)
		{
			RBFinger<T> rbfinger = base.Find(item, this.Comparison);
			if (rbfinger.Found)
			{
				base.RemoveAt(ref rbfinger);
			}
			if (base.LeftChild != null)
			{
				base.LeftChild.IsRed = false;
			}
			return rbfinger.Found;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x00159E8F File Offset: 0x00158E8F
		public IEnumerator<T> GetEnumerator()
		{
			RBFinger<T> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				yield return finger.Node.GetItemAt(finger.Offset);
				RBFinger<T> rbfinger = ++finger;
				finger = rbfinger;
			}
			yield break;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x00159E9E File Offset: 0x00158E9E
		IEnumerator IEnumerable.GetEnumerator()
		{
			RBFinger<T> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				yield return finger.Node.GetItemAt(finger.Offset);
				RBFinger<T> rbfinger = ++finger;
				finger = rbfinger;
			}
			yield break;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00159EAD File Offset: 0x00158EAD
		private void VerifyIndex(int index, int delta = 0)
		{
			if (index < 0 || index >= this.Count + delta)
			{
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private void Verify(int expectedSize, bool checkSort = true)
		{
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private void SaveTree()
		{
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void LoadTree(string s)
		{
		}

		// Token: 0x04000C52 RID: 3154
		private const int QuickSortThreshold = 15;

		// Token: 0x04000C53 RID: 3155
		private Comparison<T> _comparison;
	}
}
