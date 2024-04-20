using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000697 RID: 1687
	public sealed class TableRowGroupCollection : IList<TableRowGroup>, ICollection<TableRowGroup>, IEnumerable<TableRowGroup>, IEnumerable, IList, ICollection
	{
		// Token: 0x06005478 RID: 21624 RVA: 0x0025DAC2 File Offset: 0x0025CAC2
		internal TableRowGroupCollection(Table owner)
		{
			this._rowGroupCollectionInternal = new TableTextElementCollectionInternal<Table, TableRowGroup>(owner);
		}

		// Token: 0x06005479 RID: 21625 RVA: 0x0025DAD6 File Offset: 0x0025CAD6
		public void CopyTo(Array array, int index)
		{
			this._rowGroupCollectionInternal.CopyTo(array, index);
		}

		// Token: 0x0600547A RID: 21626 RVA: 0x0025DAE5 File Offset: 0x0025CAE5
		public void CopyTo(TableRowGroup[] array, int index)
		{
			this._rowGroupCollectionInternal.CopyTo(array, index);
		}

		// Token: 0x0600547B RID: 21627 RVA: 0x0025DAF4 File Offset: 0x0025CAF4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._rowGroupCollectionInternal.GetEnumerator();
		}

		// Token: 0x0600547C RID: 21628 RVA: 0x0025DB01 File Offset: 0x0025CB01
		IEnumerator<TableRowGroup> IEnumerable<TableRowGroup>.GetEnumerator()
		{
			return ((IEnumerable<TableRowGroup>)this._rowGroupCollectionInternal).GetEnumerator();
		}

		// Token: 0x0600547D RID: 21629 RVA: 0x0025DB0E File Offset: 0x0025CB0E
		public void Add(TableRowGroup item)
		{
			this._rowGroupCollectionInternal.Add(item);
		}

		// Token: 0x0600547E RID: 21630 RVA: 0x0025DB1C File Offset: 0x0025CB1C
		public void Clear()
		{
			this._rowGroupCollectionInternal.Clear();
		}

		// Token: 0x0600547F RID: 21631 RVA: 0x0025DB29 File Offset: 0x0025CB29
		public bool Contains(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.Contains(item);
		}

		// Token: 0x06005480 RID: 21632 RVA: 0x0025DB37 File Offset: 0x0025CB37
		public int IndexOf(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.IndexOf(item);
		}

		// Token: 0x06005481 RID: 21633 RVA: 0x0025DB45 File Offset: 0x0025CB45
		public void Insert(int index, TableRowGroup item)
		{
			this._rowGroupCollectionInternal.Insert(index, item);
		}

		// Token: 0x06005482 RID: 21634 RVA: 0x0025DB54 File Offset: 0x0025CB54
		public bool Remove(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.Remove(item);
		}

		// Token: 0x06005483 RID: 21635 RVA: 0x0025DB62 File Offset: 0x0025CB62
		public void RemoveAt(int index)
		{
			this._rowGroupCollectionInternal.RemoveAt(index);
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x0025DB70 File Offset: 0x0025CB70
		public void RemoveRange(int index, int count)
		{
			this._rowGroupCollectionInternal.RemoveRange(index, count);
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x0025DB7F File Offset: 0x0025CB7F
		public void TrimToSize()
		{
			this._rowGroupCollectionInternal.TrimToSize();
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x0025DB8C File Offset: 0x0025CB8C
		int IList.Add(object value)
		{
			return ((IList)this._rowGroupCollectionInternal).Add(value);
		}

		// Token: 0x06005487 RID: 21639 RVA: 0x0025DB9A File Offset: 0x0025CB9A
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06005488 RID: 21640 RVA: 0x0025DBA2 File Offset: 0x0025CBA2
		bool IList.Contains(object value)
		{
			return ((IList)this._rowGroupCollectionInternal).Contains(value);
		}

		// Token: 0x06005489 RID: 21641 RVA: 0x0025DBB0 File Offset: 0x0025CBB0
		int IList.IndexOf(object value)
		{
			return ((IList)this._rowGroupCollectionInternal).IndexOf(value);
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x0025DBBE File Offset: 0x0025CBBE
		void IList.Insert(int index, object value)
		{
			((IList)this._rowGroupCollectionInternal).Insert(index, value);
		}

		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x0600548B RID: 21643 RVA: 0x0025DBCD File Offset: 0x0025CBCD
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._rowGroupCollectionInternal).IsFixedSize;
			}
		}

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x0600548C RID: 21644 RVA: 0x0025DBDA File Offset: 0x0025CBDA
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._rowGroupCollectionInternal).IsReadOnly;
			}
		}

		// Token: 0x0600548D RID: 21645 RVA: 0x0025DBE7 File Offset: 0x0025CBE7
		void IList.Remove(object value)
		{
			((IList)this._rowGroupCollectionInternal).Remove(value);
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x0025DBF5 File Offset: 0x0025CBF5
		void IList.RemoveAt(int index)
		{
			((IList)this._rowGroupCollectionInternal).RemoveAt(index);
		}

		// Token: 0x170013F2 RID: 5106
		object IList.this[int index]
		{
			get
			{
				return ((IList)this._rowGroupCollectionInternal)[index];
			}
			set
			{
				((IList)this._rowGroupCollectionInternal)[index] = value;
			}
		}

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x06005491 RID: 21649 RVA: 0x0025DC20 File Offset: 0x0025CC20
		public int Count
		{
			get
			{
				return this._rowGroupCollectionInternal.Count;
			}
		}

		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x06005492 RID: 21650 RVA: 0x0025DC2D File Offset: 0x0025CC2D
		public bool IsReadOnly
		{
			get
			{
				return this._rowGroupCollectionInternal.IsReadOnly;
			}
		}

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x06005493 RID: 21651 RVA: 0x0025DC3A File Offset: 0x0025CC3A
		public bool IsSynchronized
		{
			get
			{
				return this._rowGroupCollectionInternal.IsSynchronized;
			}
		}

		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x06005494 RID: 21652 RVA: 0x0025DC47 File Offset: 0x0025CC47
		public object SyncRoot
		{
			get
			{
				return this._rowGroupCollectionInternal.SyncRoot;
			}
		}

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x06005495 RID: 21653 RVA: 0x0025DC54 File Offset: 0x0025CC54
		// (set) Token: 0x06005496 RID: 21654 RVA: 0x0025DC61 File Offset: 0x0025CC61
		public int Capacity
		{
			get
			{
				return this._rowGroupCollectionInternal.Capacity;
			}
			set
			{
				this._rowGroupCollectionInternal.Capacity = value;
			}
		}

		// Token: 0x170013F8 RID: 5112
		public TableRowGroup this[int index]
		{
			get
			{
				return this._rowGroupCollectionInternal[index];
			}
			set
			{
				this._rowGroupCollectionInternal[index] = value;
			}
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x0025DC8C File Offset: 0x0025CC8C
		internal void InternalAdd(TableRowGroup item)
		{
			this._rowGroupCollectionInternal.InternalAdd(item);
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x0025DC9A File Offset: 0x0025CC9A
		internal void InternalRemove(TableRowGroup item)
		{
			this._rowGroupCollectionInternal.InternalRemove(item);
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x0025DCA8 File Offset: 0x0025CCA8
		private void EnsureCapacity(int min)
		{
			this._rowGroupCollectionInternal.EnsureCapacity(min);
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x0025DCB6 File Offset: 0x0025CCB6
		private void PrivateConnectChild(int index, TableRowGroup item)
		{
			this._rowGroupCollectionInternal.PrivateConnectChild(index, item);
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x0025DCC5 File Offset: 0x0025CCC5
		private void PrivateDisconnectChild(TableRowGroup item)
		{
			this._rowGroupCollectionInternal.PrivateDisconnectChild(item);
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x0025DCD3 File Offset: 0x0025CCD3
		private bool BelongsToOwner(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.BelongsToOwner(item);
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x0025DCE1 File Offset: 0x0025CCE1
		private int FindInsertionIndex(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.FindInsertionIndex(item);
		}

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x060054A0 RID: 21664 RVA: 0x0025DCEF File Offset: 0x0025CCEF
		// (set) Token: 0x060054A1 RID: 21665 RVA: 0x0025DCFC File Offset: 0x0025CCFC
		private int PrivateCapacity
		{
			get
			{
				return this._rowGroupCollectionInternal.PrivateCapacity;
			}
			set
			{
				this._rowGroupCollectionInternal.PrivateCapacity = value;
			}
		}

		// Token: 0x04002F02 RID: 12034
		private TableTextElementCollectionInternal<Table, TableRowGroup> _rowGroupCollectionInternal;
	}
}
