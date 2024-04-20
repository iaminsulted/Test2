using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000693 RID: 1683
	public sealed class TableColumnCollection : IList<TableColumn>, ICollection<TableColumn>, IEnumerable<TableColumn>, IEnumerable, IList, ICollection
	{
		// Token: 0x060053F5 RID: 21493 RVA: 0x0025D147 File Offset: 0x0025C147
		internal TableColumnCollection(Table owner)
		{
			this._columnCollection = new TableColumnCollectionInternal(owner);
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x0025D15B File Offset: 0x0025C15B
		public void CopyTo(Array array, int index)
		{
			this._columnCollection.CopyTo(array, index);
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x0025D16A File Offset: 0x0025C16A
		public void CopyTo(TableColumn[] array, int index)
		{
			this._columnCollection.CopyTo(array, index);
		}

		// Token: 0x060053F8 RID: 21496 RVA: 0x0025D179 File Offset: 0x0025C179
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._columnCollection.GetEnumerator();
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x0025D186 File Offset: 0x0025C186
		IEnumerator<TableColumn> IEnumerable<TableColumn>.GetEnumerator()
		{
			return ((IEnumerable<TableColumn>)this._columnCollection).GetEnumerator();
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x0025D193 File Offset: 0x0025C193
		public void Add(TableColumn item)
		{
			this._columnCollection.Add(item);
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x0025D1A1 File Offset: 0x0025C1A1
		public void Clear()
		{
			this._columnCollection.Clear();
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x0025D1AE File Offset: 0x0025C1AE
		public bool Contains(TableColumn item)
		{
			return this._columnCollection.Contains(item);
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x0025D1BC File Offset: 0x0025C1BC
		public int IndexOf(TableColumn item)
		{
			return this._columnCollection.IndexOf(item);
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x0025D1CA File Offset: 0x0025C1CA
		public void Insert(int index, TableColumn item)
		{
			this._columnCollection.Insert(index, item);
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x0025D1D9 File Offset: 0x0025C1D9
		public bool Remove(TableColumn item)
		{
			return this._columnCollection.Remove(item);
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x0025D1E7 File Offset: 0x0025C1E7
		public void RemoveAt(int index)
		{
			this._columnCollection.RemoveAt(index);
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x0025D1F5 File Offset: 0x0025C1F5
		public void RemoveRange(int index, int count)
		{
			this._columnCollection.RemoveRange(index, count);
		}

		// Token: 0x06005402 RID: 21506 RVA: 0x0025D204 File Offset: 0x0025C204
		public void TrimToSize()
		{
			this._columnCollection.TrimToSize();
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x0025D214 File Offset: 0x0025C214
		int IList.Add(object value)
		{
			if (!(value is TableColumn))
			{
				throw new ArgumentException(SR.Get("TableCollectionElementTypeExpected", new object[]
				{
					typeof(TableColumn).Name
				}), "value");
			}
			return ((IList)this._columnCollection).Add(value);
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x0025D262 File Offset: 0x0025C262
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x0025D26A File Offset: 0x0025C26A
		bool IList.Contains(object value)
		{
			return ((IList)this._columnCollection).Contains(value);
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x0025D278 File Offset: 0x0025C278
		int IList.IndexOf(object value)
		{
			return ((IList)this._columnCollection).IndexOf(value);
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x0025D286 File Offset: 0x0025C286
		void IList.Insert(int index, object value)
		{
			((IList)this._columnCollection).Insert(index, value);
		}

		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06005408 RID: 21512 RVA: 0x0025D295 File Offset: 0x0025C295
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._columnCollection).IsFixedSize;
			}
		}

		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x06005409 RID: 21513 RVA: 0x0025D2A2 File Offset: 0x0025C2A2
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._columnCollection).IsReadOnly;
			}
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x0025D2AF File Offset: 0x0025C2AF
		void IList.Remove(object value)
		{
			((IList)this._columnCollection).Remove(value);
		}

		// Token: 0x0600540B RID: 21515 RVA: 0x0025D2BD File Offset: 0x0025C2BD
		void IList.RemoveAt(int index)
		{
			((IList)this._columnCollection).RemoveAt(index);
		}

		// Token: 0x170013CB RID: 5067
		object IList.this[int index]
		{
			get
			{
				return ((IList)this._columnCollection)[index];
			}
			set
			{
				((IList)this._columnCollection)[index] = value;
			}
		}

		// Token: 0x170013CC RID: 5068
		// (get) Token: 0x0600540E RID: 21518 RVA: 0x0025D2E8 File Offset: 0x0025C2E8
		public int Count
		{
			get
			{
				return this._columnCollection.Count;
			}
		}

		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x0600540F RID: 21519 RVA: 0x0025D2F5 File Offset: 0x0025C2F5
		public bool IsReadOnly
		{
			get
			{
				return this._columnCollection.IsReadOnly;
			}
		}

		// Token: 0x170013CE RID: 5070
		// (get) Token: 0x06005410 RID: 21520 RVA: 0x0025D302 File Offset: 0x0025C302
		public bool IsSynchronized
		{
			get
			{
				return this._columnCollection.IsSynchronized;
			}
		}

		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x06005411 RID: 21521 RVA: 0x0025D30F File Offset: 0x0025C30F
		public object SyncRoot
		{
			get
			{
				return this._columnCollection.SyncRoot;
			}
		}

		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x06005412 RID: 21522 RVA: 0x0025D31C File Offset: 0x0025C31C
		// (set) Token: 0x06005413 RID: 21523 RVA: 0x0025D329 File Offset: 0x0025C329
		public int Capacity
		{
			get
			{
				return this._columnCollection.PrivateCapacity;
			}
			set
			{
				this._columnCollection.PrivateCapacity = value;
			}
		}

		// Token: 0x170013D1 RID: 5073
		public TableColumn this[int index]
		{
			get
			{
				return this._columnCollection[index];
			}
			set
			{
				this._columnCollection[index] = value;
			}
		}

		// Token: 0x04002EF5 RID: 12021
		private TableColumnCollectionInternal _columnCollection;
	}
}
