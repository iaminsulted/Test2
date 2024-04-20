using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001B8 RID: 440
	internal abstract class ContentElementCollection<TParent, TItem> : IList<TItem>, ICollection<TItem>, IEnumerable<TItem>, IEnumerable, IList, ICollection where TParent : TextElement, IAcceptInsertion where TItem : FrameworkContentElement, IIndexedChild<TParent>
	{
		// Token: 0x06000E4A RID: 3658 RVA: 0x00138D66 File Offset: 0x00137D66
		internal ContentElementCollection(TParent owner)
		{
			this._owner = owner;
			this.Items = new TItem[this.DefaultCapacity];
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x00138D88 File Offset: 0x00137D88
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.Get("TableCollectionRankMultiDimNotSupported"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.Get("TableCollectionOutOfRangeNeedNonNegNum"));
			}
			if (array.Length - index < this.Size)
			{
				throw new ArgumentException(SR.Get("TableCollectionInvalidOffLen"));
			}
			Array.Copy(this.Items, 0, array, index, this.Size);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00138E0C File Offset: 0x00137E0C
		public void CopyTo(TItem[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.Get("TableCollectionOutOfRangeNeedNonNegNum"));
			}
			if (array.Length - index < this.Size)
			{
				throw new ArgumentException(SR.Get("TableCollectionInvalidOffLen"));
			}
			Array.Copy(this.Items, 0, array, index, this.Size);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00138E71 File Offset: 0x00137E71
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00138E79 File Offset: 0x00137E79
		internal IEnumerator GetEnumerator()
		{
			return new ContentElementCollection<TParent, TItem>.ContentElementCollectionEnumeratorSimple(this);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00138E79 File Offset: 0x00137E79
		IEnumerator<TItem> IEnumerable<!1>.GetEnumerator()
		{
			return new ContentElementCollection<TParent, TItem>.ContentElementCollectionEnumeratorSimple(this);
		}

		// Token: 0x06000E50 RID: 3664
		public abstract void Add(TItem item);

		// Token: 0x06000E51 RID: 3665
		public abstract void Clear();

		// Token: 0x06000E52 RID: 3666 RVA: 0x00138E81 File Offset: 0x00137E81
		public bool Contains(TItem item)
		{
			return this.BelongsToOwner(item);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00138E8F File Offset: 0x00137E8F
		public int IndexOf(TItem item)
		{
			if (this.BelongsToOwner(item))
			{
				return item.Index;
			}
			return -1;
		}

		// Token: 0x06000E54 RID: 3668
		public abstract void Insert(int index, TItem item);

		// Token: 0x06000E55 RID: 3669
		public abstract bool Remove(TItem item);

		// Token: 0x06000E56 RID: 3670
		public abstract void RemoveAt(int index);

		// Token: 0x06000E57 RID: 3671
		public abstract void RemoveRange(int index, int count);

		// Token: 0x06000E58 RID: 3672 RVA: 0x00138EA7 File Offset: 0x00137EA7
		public void TrimToSize()
		{
			this.PrivateCapacity = this.Size;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00138EB8 File Offset: 0x00137EB8
		int IList.Add(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TItem titem = value as TItem;
			this.Add(titem);
			return ((IList)this).IndexOf(titem);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00138EF2 File Offset: 0x00137EF2
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00138EFC File Offset: 0x00137EFC
		bool IList.Contains(object value)
		{
			TItem titem = value as TItem;
			return titem != null && this.Contains(titem);
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00138F28 File Offset: 0x00137F28
		int IList.IndexOf(object value)
		{
			TItem titem = value as TItem;
			if (titem == null)
			{
				return -1;
			}
			return this.IndexOf(titem);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00138F54 File Offset: 0x00137F54
		void IList.Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TItem titem = value as TItem;
			if (titem == null)
			{
				throw new ArgumentException(SR.Get("TableCollectionElementTypeExpected", new object[]
				{
					typeof(TItem).Name
				}), "value");
			}
			this.Insert(index, titem);
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00138FB8 File Offset: 0x00137FB8
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00138FC0 File Offset: 0x00137FC0
		void IList.Remove(object value)
		{
			TItem titem = value as TItem;
			if (titem == null)
			{
				return;
			}
			this.Remove(titem);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00138FEA File Offset: 0x00137FEA
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x17000263 RID: 611
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				TItem titem = value as TItem;
				if (titem == null)
				{
					throw new ArgumentException(SR.Get("TableCollectionElementTypeExpected", new object[]
					{
						typeof(TItem).Name
					}), "value");
				}
				this[index] = titem;
			}
		}

		// Token: 0x17000264 RID: 612
		public abstract TItem this[int index]
		{
			get;
			set;
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x00139068 File Offset: 0x00138068
		public int Count
		{
			get
			{
				return this.Size;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x000F93D3 File Offset: 0x000F83D3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x00139070 File Offset: 0x00138070
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x00139078 File Offset: 0x00138078
		public int Capacity
		{
			get
			{
				return this.PrivateCapacity;
			}
			set
			{
				this.PrivateCapacity = value;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x00139081 File Offset: 0x00138081
		public TParent Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x00139089 File Offset: 0x00138089
		// (set) Token: 0x06000E6E RID: 3694 RVA: 0x00139091 File Offset: 0x00138091
		private protected TItem[] Items
		{
			protected get
			{
				return this._items;
			}
			private set
			{
				this._items = value;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x0013909A File Offset: 0x0013809A
		// (set) Token: 0x06000E70 RID: 3696 RVA: 0x001390A2 File Offset: 0x001380A2
		protected int Size
		{
			get
			{
				return this._size;
			}
			set
			{
				this._size = value;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x001390AB File Offset: 0x001380AB
		// (set) Token: 0x06000E72 RID: 3698 RVA: 0x001390B3 File Offset: 0x001380B3
		protected int Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x001390BC File Offset: 0x001380BC
		protected int DefaultCapacity
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x001390BF File Offset: 0x001380BF
		internal void EnsureCapacity(int min)
		{
			if (this.PrivateCapacity < min)
			{
				this.PrivateCapacity = Math.Max(min, this.PrivateCapacity * 2);
			}
		}

		// Token: 0x06000E75 RID: 3701
		internal abstract void PrivateConnectChild(int index, TItem item);

		// Token: 0x06000E76 RID: 3702
		internal abstract void PrivateDisconnectChild(TItem item);

		// Token: 0x06000E77 RID: 3703 RVA: 0x001390E0 File Offset: 0x001380E0
		internal void PrivateRemove(TItem item)
		{
			int index = item.Index;
			this.PrivateDisconnectChild(item);
			int size = this.Size - 1;
			this.Size = size;
			for (int i = index; i < this.Size; i++)
			{
				this.Items[i] = this.Items[i + 1];
				this.Items[i].Index = i;
			}
			this.Items[this.Size] = default(TItem);
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0013916C File Offset: 0x0013816C
		internal bool BelongsToOwner(TItem item)
		{
			if (item == null)
			{
				return false;
			}
			DependencyObject parent = item.Parent;
			if (parent is ContentElementCollection<TParent, TItem>.DummyProxy)
			{
				parent = LogicalTreeHelper.GetParent(parent);
			}
			return parent == this.Owner;
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x001391AC File Offset: 0x001381AC
		// (set) Token: 0x06000E7A RID: 3706 RVA: 0x001391B8 File Offset: 0x001381B8
		internal int PrivateCapacity
		{
			get
			{
				return this.Items.Length;
			}
			set
			{
				if (value != this.Items.Length)
				{
					if (value < this.Size)
					{
						throw new ArgumentOutOfRangeException(SR.Get("TableCollectionNotEnoughCapacity"));
					}
					if (value > 0)
					{
						TItem[] array = new TItem[value];
						if (this.Size > 0)
						{
							Array.Copy(this.Items, 0, array, 0, this.Size);
						}
						this.Items = array;
						return;
					}
					this.Items = new TItem[this.DefaultCapacity];
				}
			}
		}

		// Token: 0x04000A4E RID: 2638
		private readonly TParent _owner;

		// Token: 0x04000A4F RID: 2639
		private TItem[] _items;

		// Token: 0x04000A50 RID: 2640
		private int _size;

		// Token: 0x04000A51 RID: 2641
		private int _version;

		// Token: 0x04000A52 RID: 2642
		protected const int c_defaultCapacity = 8;

		// Token: 0x020009CF RID: 2511
		protected class ContentElementCollectionEnumeratorSimple : IEnumerator<TItem>, IEnumerator, IDisposable
		{
			// Token: 0x060083F5 RID: 33781 RVA: 0x00324C20 File Offset: 0x00323C20
			internal ContentElementCollectionEnumeratorSimple(ContentElementCollection<TParent, TItem> collection)
			{
				this._collection = collection;
				this._index = -1;
				this.Version = this._collection.Version;
				this._currentElement = collection;
			}

			// Token: 0x060083F6 RID: 33782 RVA: 0x00324C50 File Offset: 0x00323C50
			public bool MoveNext()
			{
				if (this.Version != this._collection.Version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				if (this._index < this._collection.Size - 1)
				{
					this._index++;
					this._currentElement = this._collection[this._index];
					return true;
				}
				this._currentElement = this._collection;
				this._index = this._collection.Size;
				return false;
			}

			// Token: 0x17001DAB RID: 7595
			// (get) Token: 0x060083F7 RID: 33783 RVA: 0x00324CE0 File Offset: 0x00323CE0
			public TItem Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return (TItem)((object)this._currentElement);
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x17001DAC RID: 7596
			// (get) Token: 0x060083F8 RID: 33784 RVA: 0x00324D2F File Offset: 0x00323D2F
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060083F9 RID: 33785 RVA: 0x00324D3C File Offset: 0x00323D3C
			public void Reset()
			{
				if (this.Version != this._collection.Version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				this._currentElement = this._collection;
				this._index = -1;
			}

			// Token: 0x060083FA RID: 33786 RVA: 0x00219F89 File Offset: 0x00218F89
			public void Dispose()
			{
				GC.SuppressFinalize(this);
			}

			// Token: 0x04003FB8 RID: 16312
			private ContentElementCollection<TParent, TItem> _collection;

			// Token: 0x04003FB9 RID: 16313
			private int _index;

			// Token: 0x04003FBA RID: 16314
			protected int Version;

			// Token: 0x04003FBB RID: 16315
			private object _currentElement;
		}

		// Token: 0x020009D0 RID: 2512
		protected class DummyProxy : DependencyObject
		{
		}
	}
}
