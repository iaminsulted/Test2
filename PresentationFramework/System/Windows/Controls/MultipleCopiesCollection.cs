using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x020007B1 RID: 1969
	internal class MultipleCopiesCollection : IList, ICollection, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		// Token: 0x06006FC8 RID: 28616 RVA: 0x002D633E File Offset: 0x002D533E
		internal MultipleCopiesCollection(object item, int count)
		{
			this.CopiedItem = item;
			this._count = count;
		}

		// Token: 0x06006FC9 RID: 28617 RVA: 0x002D6354 File Offset: 0x002D5354
		internal void MirrorCollectionChange(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.Insert(e.NewStartingIndex);
				return;
			case NotifyCollectionChangedAction.Remove:
				this.RemoveAt(e.OldStartingIndex);
				return;
			case NotifyCollectionChangedAction.Replace:
				this.OnReplace(this.CopiedItem, this.CopiedItem, e.NewStartingIndex);
				return;
			case NotifyCollectionChangedAction.Move:
				this.Move(e.OldStartingIndex, e.NewStartingIndex);
				return;
			case NotifyCollectionChangedAction.Reset:
				this.Reset();
				return;
			default:
				return;
			}
		}

		// Token: 0x06006FCA RID: 28618 RVA: 0x002D63D0 File Offset: 0x002D53D0
		internal void SyncToCount(int newCount)
		{
			int repeatCount = this.RepeatCount;
			if (newCount != repeatCount)
			{
				if (newCount > repeatCount)
				{
					this.InsertRange(repeatCount, newCount - repeatCount);
					return;
				}
				int num = repeatCount - newCount;
				this.RemoveRange(repeatCount - num, num);
			}
		}

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x06006FCB RID: 28619 RVA: 0x002D6405 File Offset: 0x002D5405
		// (set) Token: 0x06006FCC RID: 28620 RVA: 0x002D6410 File Offset: 0x002D5410
		internal object CopiedItem
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value == CollectionView.NewItemPlaceholder)
				{
					value = DataGrid.NewItemPlaceholder;
				}
				if (this._item != value)
				{
					object item = this._item;
					this._item = value;
					this.OnPropertyChanged("Item[]");
					for (int i = 0; i < this._count; i++)
					{
						this.OnReplace(item, this._item, i);
					}
				}
			}
		}

		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x06006FCD RID: 28621 RVA: 0x002D646D File Offset: 0x002D546D
		// (set) Token: 0x06006FCE RID: 28622 RVA: 0x002D6475 File Offset: 0x002D5475
		private int RepeatCount
		{
			get
			{
				return this._count;
			}
			set
			{
				if (this._count != value)
				{
					this._count = value;
					this.OnPropertyChanged("Count");
					this.OnPropertyChanged("Item[]");
				}
			}
		}

		// Token: 0x06006FCF RID: 28623 RVA: 0x002D64A0 File Offset: 0x002D54A0
		private void Insert(int index)
		{
			int repeatCount = this.RepeatCount;
			this.RepeatCount = repeatCount + 1;
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, this.CopiedItem, index);
		}

		// Token: 0x06006FD0 RID: 28624 RVA: 0x002D64CC File Offset: 0x002D54CC
		private void InsertRange(int index, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Insert(index);
				index++;
			}
		}

		// Token: 0x06006FD1 RID: 28625 RVA: 0x002D64F1 File Offset: 0x002D54F1
		private void Move(int oldIndex, int newIndex)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, this.CopiedItem, newIndex, oldIndex));
		}

		// Token: 0x06006FD2 RID: 28626 RVA: 0x002D6508 File Offset: 0x002D5508
		private void RemoveAt(int index)
		{
			int repeatCount = this.RepeatCount;
			this.RepeatCount = repeatCount - 1;
			this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, this.CopiedItem, index);
		}

		// Token: 0x06006FD3 RID: 28627 RVA: 0x002D6534 File Offset: 0x002D5534
		private void RemoveRange(int index, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.RemoveAt(index);
			}
		}

		// Token: 0x06006FD4 RID: 28628 RVA: 0x002D6554 File Offset: 0x002D5554
		private void OnReplace(object oldItem, object newItem, int index)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
		}

		// Token: 0x06006FD5 RID: 28629 RVA: 0x002D6565 File Offset: 0x002D5565
		private void Reset()
		{
			this.RepeatCount = 0;
			this.OnCollectionReset();
		}

		// Token: 0x06006FD6 RID: 28630 RVA: 0x002D6574 File Offset: 0x002D5574
		public int Add(object value)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x06006FD7 RID: 28631 RVA: 0x002D6574 File Offset: 0x002D5574
		public void Clear()
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x002D6585 File Offset: 0x002D5585
		public bool Contains(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this._item == value;
		}

		// Token: 0x06006FD9 RID: 28633 RVA: 0x002D659E File Offset: 0x002D559E
		public int IndexOf(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this._item != value)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06006FDA RID: 28634 RVA: 0x002D6574 File Offset: 0x002D5574
		public void Insert(int index, object value)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x06006FDB RID: 28635 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x06006FDC RID: 28636 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006FDD RID: 28637 RVA: 0x002D6574 File Offset: 0x002D5574
		public void Remove(object value)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x06006FDE RID: 28638 RVA: 0x002D6574 File Offset: 0x002D5574
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x170019D0 RID: 6608
		public object this[int index]
		{
			get
			{
				if (index >= 0 && index < this.RepeatCount)
				{
					return this._item;
				}
				throw new ArgumentOutOfRangeException("index");
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06006FE1 RID: 28641 RVA: 0x0012F160 File Offset: 0x0012E160
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x06006FE2 RID: 28642 RVA: 0x002D65DA File Offset: 0x002D55DA
		public int Count
		{
			get
			{
				return this.RepeatCount;
			}
		}

		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x06006FE3 RID: 28643 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170019D3 RID: 6611
		// (get) Token: 0x06006FE4 RID: 28644 RVA: 0x000F93D3 File Offset: 0x000F83D3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06006FE5 RID: 28645 RVA: 0x002D65E2 File Offset: 0x002D55E2
		public IEnumerator GetEnumerator()
		{
			return new MultipleCopiesCollection.MultipleCopiesCollectionEnumerator(this);
		}

		// Token: 0x14000140 RID: 320
		// (add) Token: 0x06006FE6 RID: 28646 RVA: 0x002D65EC File Offset: 0x002D55EC
		// (remove) Token: 0x06006FE7 RID: 28647 RVA: 0x002D6624 File Offset: 0x002D5624
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x06006FE8 RID: 28648 RVA: 0x002D6659 File Offset: 0x002D5659
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
		}

		// Token: 0x06006FE9 RID: 28649 RVA: 0x002D6669 File Offset: 0x002D5669
		private void OnCollectionReset()
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06006FEA RID: 28650 RVA: 0x002D6677 File Offset: 0x002D5677
		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, e);
			}
		}

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x06006FEB RID: 28651 RVA: 0x002D6690 File Offset: 0x002D5690
		// (remove) Token: 0x06006FEC RID: 28652 RVA: 0x002D66C8 File Offset: 0x002D56C8
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06006FED RID: 28653 RVA: 0x002D66FD File Offset: 0x002D56FD
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06006FEE RID: 28654 RVA: 0x002D670B File Offset: 0x002D570B
		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		// Token: 0x040036B6 RID: 14006
		private object _item;

		// Token: 0x040036B7 RID: 14007
		private int _count;

		// Token: 0x040036B8 RID: 14008
		private const string CountName = "Count";

		// Token: 0x040036B9 RID: 14009
		private const string IndexerName = "Item[]";

		// Token: 0x02000C10 RID: 3088
		private class MultipleCopiesCollectionEnumerator : IEnumerator
		{
			// Token: 0x0600905A RID: 36954 RVA: 0x00346718 File Offset: 0x00345718
			public MultipleCopiesCollectionEnumerator(MultipleCopiesCollection collection)
			{
				this._collection = collection;
				this._item = this._collection.CopiedItem;
				this._count = this._collection.RepeatCount;
				this._current = -1;
			}

			// Token: 0x17001F89 RID: 8073
			// (get) Token: 0x0600905B RID: 36955 RVA: 0x00346750 File Offset: 0x00345750
			object IEnumerator.Current
			{
				get
				{
					if (this._current < 0)
					{
						return null;
					}
					if (this._current < this._count)
					{
						return this._item;
					}
					throw new InvalidOperationException();
				}
			}

			// Token: 0x0600905C RID: 36956 RVA: 0x00346778 File Offset: 0x00345778
			bool IEnumerator.MoveNext()
			{
				if (!this.IsCollectionUnchanged)
				{
					throw new InvalidOperationException();
				}
				int num = this._current + 1;
				if (num < this._count)
				{
					this._current = num;
					return true;
				}
				return false;
			}

			// Token: 0x0600905D RID: 36957 RVA: 0x003467AF File Offset: 0x003457AF
			void IEnumerator.Reset()
			{
				if (this.IsCollectionUnchanged)
				{
					this._current = -1;
					return;
				}
				throw new InvalidOperationException();
			}

			// Token: 0x17001F8A RID: 8074
			// (get) Token: 0x0600905E RID: 36958 RVA: 0x003467C6 File Offset: 0x003457C6
			private bool IsCollectionUnchanged
			{
				get
				{
					return this._collection.RepeatCount == this._count && this._collection.CopiedItem == this._item;
				}
			}

			// Token: 0x04004AD4 RID: 19156
			private object _item;

			// Token: 0x04004AD5 RID: 19157
			private int _count;

			// Token: 0x04004AD6 RID: 19158
			private int _current;

			// Token: 0x04004AD7 RID: 19159
			private MultipleCopiesCollection _collection;
		}
	}
}
