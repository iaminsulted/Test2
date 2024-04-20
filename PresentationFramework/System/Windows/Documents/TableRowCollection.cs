using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000695 RID: 1685
	public sealed class TableRowCollection : IList<TableRow>, ICollection<TableRow>, IEnumerable<TableRow>, IEnumerable, IList, ICollection
	{
		// Token: 0x06005436 RID: 21558 RVA: 0x0025D665 File Offset: 0x0025C665
		internal TableRowCollection(TableRowGroup owner)
		{
			this._rowCollectionInternal = new TableTextElementCollectionInternal<TableRowGroup, TableRow>(owner);
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x0025D679 File Offset: 0x0025C679
		public void CopyTo(Array array, int index)
		{
			this._rowCollectionInternal.CopyTo(array, index);
		}

		// Token: 0x06005438 RID: 21560 RVA: 0x0025D688 File Offset: 0x0025C688
		public void CopyTo(TableRow[] array, int index)
		{
			this._rowCollectionInternal.CopyTo(array, index);
		}

		// Token: 0x06005439 RID: 21561 RVA: 0x0025D697 File Offset: 0x0025C697
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._rowCollectionInternal.GetEnumerator();
		}

		// Token: 0x0600543A RID: 21562 RVA: 0x0025D6A4 File Offset: 0x0025C6A4
		IEnumerator<TableRow> IEnumerable<TableRow>.GetEnumerator()
		{
			return ((IEnumerable<TableRow>)this._rowCollectionInternal).GetEnumerator();
		}

		// Token: 0x0600543B RID: 21563 RVA: 0x0025D6B1 File Offset: 0x0025C6B1
		public void Add(TableRow item)
		{
			this._rowCollectionInternal.Add(item);
		}

		// Token: 0x0600543C RID: 21564 RVA: 0x0025D6BF File Offset: 0x0025C6BF
		public void Clear()
		{
			this._rowCollectionInternal.Clear();
		}

		// Token: 0x0600543D RID: 21565 RVA: 0x0025D6CC File Offset: 0x0025C6CC
		public bool Contains(TableRow item)
		{
			return this._rowCollectionInternal.Contains(item);
		}

		// Token: 0x0600543E RID: 21566 RVA: 0x0025D6DA File Offset: 0x0025C6DA
		public int IndexOf(TableRow item)
		{
			return this._rowCollectionInternal.IndexOf(item);
		}

		// Token: 0x0600543F RID: 21567 RVA: 0x0025D6E8 File Offset: 0x0025C6E8
		public void Insert(int index, TableRow item)
		{
			this._rowCollectionInternal.Insert(index, item);
		}

		// Token: 0x06005440 RID: 21568 RVA: 0x0025D6F7 File Offset: 0x0025C6F7
		public bool Remove(TableRow item)
		{
			return this._rowCollectionInternal.Remove(item);
		}

		// Token: 0x06005441 RID: 21569 RVA: 0x0025D705 File Offset: 0x0025C705
		public void RemoveAt(int index)
		{
			this._rowCollectionInternal.RemoveAt(index);
		}

		// Token: 0x06005442 RID: 21570 RVA: 0x0025D713 File Offset: 0x0025C713
		public void RemoveRange(int index, int count)
		{
			this._rowCollectionInternal.RemoveRange(index, count);
		}

		// Token: 0x06005443 RID: 21571 RVA: 0x0025D722 File Offset: 0x0025C722
		public void TrimToSize()
		{
			this._rowCollectionInternal.TrimToSize();
		}

		// Token: 0x06005444 RID: 21572 RVA: 0x0025D72F File Offset: 0x0025C72F
		int IList.Add(object value)
		{
			return ((IList)this._rowCollectionInternal).Add(value);
		}

		// Token: 0x06005445 RID: 21573 RVA: 0x0025D73D File Offset: 0x0025C73D
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06005446 RID: 21574 RVA: 0x0025D745 File Offset: 0x0025C745
		bool IList.Contains(object value)
		{
			return ((IList)this._rowCollectionInternal).Contains(value);
		}

		// Token: 0x06005447 RID: 21575 RVA: 0x0025D753 File Offset: 0x0025C753
		int IList.IndexOf(object value)
		{
			return ((IList)this._rowCollectionInternal).IndexOf(value);
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x0025D761 File Offset: 0x0025C761
		void IList.Insert(int index, object value)
		{
			((IList)this._rowCollectionInternal).Insert(index, value);
		}

		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x06005449 RID: 21577 RVA: 0x0025D770 File Offset: 0x0025C770
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._rowCollectionInternal).IsFixedSize;
			}
		}

		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x0600544A RID: 21578 RVA: 0x0025D77D File Offset: 0x0025C77D
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._rowCollectionInternal).IsReadOnly;
			}
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x0025D78A File Offset: 0x0025C78A
		void IList.Remove(object value)
		{
			((IList)this._rowCollectionInternal).Remove(value);
		}

		// Token: 0x0600544C RID: 21580 RVA: 0x0025D798 File Offset: 0x0025C798
		void IList.RemoveAt(int index)
		{
			((IList)this._rowCollectionInternal).RemoveAt(index);
		}

		// Token: 0x170013E1 RID: 5089
		object IList.this[int index]
		{
			get
			{
				return ((IList)this._rowCollectionInternal)[index];
			}
			set
			{
				((IList)this._rowCollectionInternal)[index] = value;
			}
		}

		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x0600544F RID: 21583 RVA: 0x0025D7C3 File Offset: 0x0025C7C3
		public int Count
		{
			get
			{
				return this._rowCollectionInternal.Count;
			}
		}

		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x06005450 RID: 21584 RVA: 0x0025D77D File Offset: 0x0025C77D
		public bool IsReadOnly
		{
			get
			{
				return ((IList)this._rowCollectionInternal).IsReadOnly;
			}
		}

		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x06005451 RID: 21585 RVA: 0x0025D7D0 File Offset: 0x0025C7D0
		public bool IsSynchronized
		{
			get
			{
				return ((ICollection)this._rowCollectionInternal).IsSynchronized;
			}
		}

		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x06005452 RID: 21586 RVA: 0x0025D7DD File Offset: 0x0025C7DD
		public object SyncRoot
		{
			get
			{
				return ((ICollection)this._rowCollectionInternal).SyncRoot;
			}
		}

		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x06005453 RID: 21587 RVA: 0x0025D7EA File Offset: 0x0025C7EA
		// (set) Token: 0x06005454 RID: 21588 RVA: 0x0025D7F7 File Offset: 0x0025C7F7
		public int Capacity
		{
			get
			{
				return this._rowCollectionInternal.Capacity;
			}
			set
			{
				this._rowCollectionInternal.Capacity = value;
			}
		}

		// Token: 0x170013E7 RID: 5095
		public TableRow this[int index]
		{
			get
			{
				return this._rowCollectionInternal[index];
			}
			set
			{
				this._rowCollectionInternal[index] = value;
			}
		}

		// Token: 0x06005457 RID: 21591 RVA: 0x0025D822 File Offset: 0x0025C822
		internal void InternalAdd(TableRow item)
		{
			this._rowCollectionInternal.InternalAdd(item);
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x0025D830 File Offset: 0x0025C830
		internal void InternalRemove(TableRow item)
		{
			this._rowCollectionInternal.InternalRemove(item);
		}

		// Token: 0x06005459 RID: 21593 RVA: 0x0025D83E File Offset: 0x0025C83E
		private void EnsureCapacity(int min)
		{
			this._rowCollectionInternal.EnsureCapacity(min);
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x0025D84C File Offset: 0x0025C84C
		private void PrivateConnectChild(int index, TableRow item)
		{
			this._rowCollectionInternal.PrivateConnectChild(index, item);
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x0025D85B File Offset: 0x0025C85B
		private void PrivateDisconnectChild(TableRow item)
		{
			this._rowCollectionInternal.PrivateDisconnectChild(item);
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x0025D869 File Offset: 0x0025C869
		private bool BelongsToOwner(TableRow item)
		{
			return this._rowCollectionInternal.BelongsToOwner(item);
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x0025D877 File Offset: 0x0025C877
		private int FindInsertionIndex(TableRow item)
		{
			return this._rowCollectionInternal.FindInsertionIndex(item);
		}

		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x0600545E RID: 21598 RVA: 0x0025D885 File Offset: 0x0025C885
		// (set) Token: 0x0600545F RID: 21599 RVA: 0x0025D892 File Offset: 0x0025C892
		private int PrivateCapacity
		{
			get
			{
				return this._rowCollectionInternal.PrivateCapacity;
			}
			set
			{
				this._rowCollectionInternal.PrivateCapacity = value;
			}
		}

		// Token: 0x04002EFD RID: 12029
		private TableTextElementCollectionInternal<TableRowGroup, TableRow> _rowCollectionInternal;
	}
}
