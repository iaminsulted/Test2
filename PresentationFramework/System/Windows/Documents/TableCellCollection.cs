using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000691 RID: 1681
	public sealed class TableCellCollection : IList<TableCell>, ICollection<TableCell>, IEnumerable<TableCell>, IEnumerable, IList, ICollection
	{
		// Token: 0x060053B7 RID: 21431 RVA: 0x0025CD42 File Offset: 0x0025BD42
		internal TableCellCollection(TableRow owner)
		{
			this._cellCollectionInternal = new TableTextElementCollectionInternal<TableRow, TableCell>(owner);
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x0025CD56 File Offset: 0x0025BD56
		public void CopyTo(Array array, int index)
		{
			this._cellCollectionInternal.CopyTo(array, index);
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x0025CD65 File Offset: 0x0025BD65
		public void CopyTo(TableCell[] array, int index)
		{
			this._cellCollectionInternal.CopyTo(array, index);
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x0025CD74 File Offset: 0x0025BD74
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._cellCollectionInternal.GetEnumerator();
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x0025CD81 File Offset: 0x0025BD81
		IEnumerator<TableCell> IEnumerable<TableCell>.GetEnumerator()
		{
			return ((IEnumerable<TableCell>)this._cellCollectionInternal).GetEnumerator();
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x0025CD8E File Offset: 0x0025BD8E
		public void Add(TableCell item)
		{
			this._cellCollectionInternal.Add(item);
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x0025CD9C File Offset: 0x0025BD9C
		public void Clear()
		{
			this._cellCollectionInternal.Clear();
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x0025CDA9 File Offset: 0x0025BDA9
		public bool Contains(TableCell item)
		{
			return this._cellCollectionInternal.Contains(item);
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x0025CDB7 File Offset: 0x0025BDB7
		public int IndexOf(TableCell item)
		{
			return this._cellCollectionInternal.IndexOf(item);
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x0025CDC5 File Offset: 0x0025BDC5
		public void Insert(int index, TableCell item)
		{
			this._cellCollectionInternal.Insert(index, item);
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x0025CDD4 File Offset: 0x0025BDD4
		public bool Remove(TableCell item)
		{
			return this._cellCollectionInternal.Remove(item);
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x0025CDE2 File Offset: 0x0025BDE2
		public void RemoveAt(int index)
		{
			this._cellCollectionInternal.RemoveAt(index);
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x0025CDF0 File Offset: 0x0025BDF0
		public void RemoveRange(int index, int count)
		{
			this._cellCollectionInternal.RemoveRange(index, count);
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x0025CDFF File Offset: 0x0025BDFF
		public void TrimToSize()
		{
			this._cellCollectionInternal.TrimToSize();
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x0025CE0C File Offset: 0x0025BE0C
		int IList.Add(object value)
		{
			return ((IList)this._cellCollectionInternal).Add(value);
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x0025CE1A File Offset: 0x0025BE1A
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x0025CE22 File Offset: 0x0025BE22
		bool IList.Contains(object value)
		{
			return ((IList)this._cellCollectionInternal).Contains(value);
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x0025CE30 File Offset: 0x0025BE30
		int IList.IndexOf(object value)
		{
			return ((IList)this._cellCollectionInternal).IndexOf(value);
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x0025CE3E File Offset: 0x0025BE3E
		void IList.Insert(int index, object value)
		{
			((IList)this._cellCollectionInternal).Insert(index, value);
		}

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x060053CA RID: 21450 RVA: 0x0025CE4D File Offset: 0x0025BE4D
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._cellCollectionInternal).IsFixedSize;
			}
		}

		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x060053CB RID: 21451 RVA: 0x0025CE5A File Offset: 0x0025BE5A
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._cellCollectionInternal).IsReadOnly;
			}
		}

		// Token: 0x060053CC RID: 21452 RVA: 0x0025CE67 File Offset: 0x0025BE67
		void IList.Remove(object value)
		{
			((IList)this._cellCollectionInternal).Remove(value);
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x0025CE75 File Offset: 0x0025BE75
		void IList.RemoveAt(int index)
		{
			((IList)this._cellCollectionInternal).RemoveAt(index);
		}

		// Token: 0x170013BB RID: 5051
		object IList.this[int index]
		{
			get
			{
				return ((IList)this._cellCollectionInternal)[index];
			}
			set
			{
				((IList)this._cellCollectionInternal)[index] = value;
			}
		}

		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x060053D0 RID: 21456 RVA: 0x0025CEA0 File Offset: 0x0025BEA0
		public int Count
		{
			get
			{
				return this._cellCollectionInternal.Count;
			}
		}

		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x060053D1 RID: 21457 RVA: 0x0025CE5A File Offset: 0x0025BE5A
		public bool IsReadOnly
		{
			get
			{
				return ((IList)this._cellCollectionInternal).IsReadOnly;
			}
		}

		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x060053D2 RID: 21458 RVA: 0x0025CEAD File Offset: 0x0025BEAD
		public bool IsSynchronized
		{
			get
			{
				return ((ICollection)this._cellCollectionInternal).IsSynchronized;
			}
		}

		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x060053D3 RID: 21459 RVA: 0x0025CEBA File Offset: 0x0025BEBA
		public object SyncRoot
		{
			get
			{
				return ((ICollection)this._cellCollectionInternal).SyncRoot;
			}
		}

		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x060053D4 RID: 21460 RVA: 0x0025CEC7 File Offset: 0x0025BEC7
		// (set) Token: 0x060053D5 RID: 21461 RVA: 0x0025CED4 File Offset: 0x0025BED4
		public int Capacity
		{
			get
			{
				return this._cellCollectionInternal.Capacity;
			}
			set
			{
				this._cellCollectionInternal.Capacity = value;
			}
		}

		// Token: 0x170013C1 RID: 5057
		public TableCell this[int index]
		{
			get
			{
				return this._cellCollectionInternal[index];
			}
			set
			{
				this._cellCollectionInternal[index] = value;
			}
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x0025CEFF File Offset: 0x0025BEFF
		internal void InternalAdd(TableCell item)
		{
			this._cellCollectionInternal.InternalAdd(item);
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x0025CF0D File Offset: 0x0025BF0D
		internal void InternalRemove(TableCell item)
		{
			this._cellCollectionInternal.InternalRemove(item);
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x0025CF1B File Offset: 0x0025BF1B
		private void EnsureCapacity(int min)
		{
			this._cellCollectionInternal.EnsureCapacity(min);
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x0025CF29 File Offset: 0x0025BF29
		private void PrivateConnectChild(int index, TableCell item)
		{
			this._cellCollectionInternal.PrivateConnectChild(index, item);
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x0025CF38 File Offset: 0x0025BF38
		private void PrivateDisconnectChild(TableCell item)
		{
			this._cellCollectionInternal.PrivateDisconnectChild(item);
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x0025CF46 File Offset: 0x0025BF46
		private bool BelongsToOwner(TableCell item)
		{
			return this._cellCollectionInternal.BelongsToOwner(item);
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x0025CF54 File Offset: 0x0025BF54
		private int FindInsertionIndex(TableCell item)
		{
			return this._cellCollectionInternal.FindInsertionIndex(item);
		}

		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x060053DF RID: 21471 RVA: 0x0025CF62 File Offset: 0x0025BF62
		// (set) Token: 0x060053E0 RID: 21472 RVA: 0x0025CF6F File Offset: 0x0025BF6F
		private int PrivateCapacity
		{
			get
			{
				return this._cellCollectionInternal.PrivateCapacity;
			}
			set
			{
				this._cellCollectionInternal.PrivateCapacity = value;
			}
		}

		// Token: 0x04002EF1 RID: 12017
		private TableTextElementCollectionInternal<TableRow, TableCell> _cellCollectionInternal;
	}
}
