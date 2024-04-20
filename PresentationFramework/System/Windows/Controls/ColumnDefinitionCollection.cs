using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Controls
{
	// Token: 0x0200081D RID: 2077
	public sealed class ColumnDefinitionCollection : IList<ColumnDefinition>, ICollection<ColumnDefinition>, IEnumerable<ColumnDefinition>, IEnumerable, IList, ICollection
	{
		// Token: 0x060078FC RID: 30972 RVA: 0x00301A55 File Offset: 0x00300A55
		internal ColumnDefinitionCollection(Grid owner)
		{
			this._owner = owner;
			this.PrivateOnModified();
		}

		// Token: 0x060078FD RID: 30973 RVA: 0x00301A6C File Offset: 0x00300A6C
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.Get("GridCollection_DestArrayInvalidRank"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("GridCollection_DestArrayInvalidLowerBound", new object[]
				{
					"index"
				}));
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException(SR.Get("GridCollection_DestArrayInvalidLength", new object[]
				{
					"array"
				}));
			}
			if (this._size > 0)
			{
				Array.Copy(this._items, 0, array, index, this._size);
			}
		}

		// Token: 0x060078FE RID: 30974 RVA: 0x00301B10 File Offset: 0x00300B10
		public void CopyTo(ColumnDefinition[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("GridCollection_DestArrayInvalidLowerBound", new object[]
				{
					"index"
				}));
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException(SR.Get("GridCollection_DestArrayInvalidLength", new object[]
				{
					"array"
				}));
			}
			if (this._size > 0)
			{
				Array.Copy(this._items, 0, array, index, this._size);
			}
		}

		// Token: 0x060078FF RID: 30975 RVA: 0x00301B95 File Offset: 0x00300B95
		int IList.Add(object value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value as ColumnDefinition);
			return this._size - 1;
		}

		// Token: 0x06007900 RID: 30976 RVA: 0x00301BBE File Offset: 0x00300BBE
		public void Add(ColumnDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value);
		}

		// Token: 0x06007901 RID: 30977 RVA: 0x00301BDC File Offset: 0x00300BDC
		public void Clear()
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateOnModified();
			for (int i = 0; i < this._size; i++)
			{
				this.PrivateDisconnectChild(this._items[i]);
				this._items[i] = null;
			}
			this._size = 0;
		}

		// Token: 0x06007902 RID: 30978 RVA: 0x00301C24 File Offset: 0x00300C24
		bool IList.Contains(object value)
		{
			ColumnDefinition columnDefinition = value as ColumnDefinition;
			return columnDefinition != null && columnDefinition.Parent == this._owner;
		}

		// Token: 0x06007903 RID: 30979 RVA: 0x00301C4C File Offset: 0x00300C4C
		public bool Contains(ColumnDefinition value)
		{
			return value != null && value.Parent == this._owner;
		}

		// Token: 0x06007904 RID: 30980 RVA: 0x00301C62 File Offset: 0x00300C62
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as ColumnDefinition);
		}

		// Token: 0x06007905 RID: 30981 RVA: 0x00301C70 File Offset: 0x00300C70
		public int IndexOf(ColumnDefinition value)
		{
			if (value == null || value.Parent != this._owner)
			{
				return -1;
			}
			return value.Index;
		}

		// Token: 0x06007906 RID: 30982 RVA: 0x00301C8B File Offset: 0x00300C8B
		void IList.Insert(int index, object value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value as ColumnDefinition);
		}

		// Token: 0x06007907 RID: 30983 RVA: 0x00301CC4 File Offset: 0x00300CC4
		public void Insert(int index, ColumnDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value);
		}

		// Token: 0x06007908 RID: 30984 RVA: 0x00301CF8 File Offset: 0x00300CF8
		void IList.Remove(object value)
		{
			this.PrivateVerifyWriteAccess();
			if (this.PrivateValidateValueForRemoval(value))
			{
				this.PrivateRemove(value as ColumnDefinition);
			}
		}

		// Token: 0x06007909 RID: 30985 RVA: 0x00301D15 File Offset: 0x00300D15
		public bool Remove(ColumnDefinition value)
		{
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value);
			}
			return flag;
		}

		// Token: 0x0600790A RID: 30986 RVA: 0x00301D28 File Offset: 0x00300D28
		public void RemoveAt(int index)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateRemove(this._items[index]);
		}

		// Token: 0x0600790B RID: 30987 RVA: 0x00301D5C File Offset: 0x00300D5C
		public void RemoveRange(int index, int count)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionCountNeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(SR.Get("TableCollectionRangeOutOfRange"));
			}
			this.PrivateOnModified();
			if (count > 0)
			{
				for (int i = index + count - 1; i >= index; i--)
				{
					this.PrivateDisconnectChild(this._items[i]);
				}
				this._size -= count;
				for (int j = index; j < this._size; j++)
				{
					this._items[j] = this._items[j + count];
					this._items[j].Index = j;
					this._items[j + count] = null;
				}
			}
		}

		// Token: 0x0600790C RID: 30988 RVA: 0x00301E2D File Offset: 0x00300E2D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ColumnDefinitionCollection.Enumerator(this);
		}

		// Token: 0x0600790D RID: 30989 RVA: 0x00301E2D File Offset: 0x00300E2D
		IEnumerator<ColumnDefinition> IEnumerable<ColumnDefinition>.GetEnumerator()
		{
			return new ColumnDefinitionCollection.Enumerator(this);
		}

		// Token: 0x17001C02 RID: 7170
		// (get) Token: 0x0600790E RID: 30990 RVA: 0x00301E3A File Offset: 0x00300E3A
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001C03 RID: 7171
		// (get) Token: 0x0600790F RID: 30991 RVA: 0x00301E42 File Offset: 0x00300E42
		bool IList.IsFixedSize
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		// Token: 0x17001C04 RID: 7172
		// (get) Token: 0x06007910 RID: 30992 RVA: 0x00301E42 File Offset: 0x00300E42
		public bool IsReadOnly
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		// Token: 0x17001C05 RID: 7173
		// (get) Token: 0x06007911 RID: 30993 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C06 RID: 7174
		// (get) Token: 0x06007912 RID: 30994 RVA: 0x000F93D3 File Offset: 0x000F83D3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001C07 RID: 7175
		object IList.this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return this._items[index];
			}
			set
			{
				this.PrivateVerifyWriteAccess();
				this.PrivateValidateValueForAddition(value);
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				this.PrivateDisconnectChild(this._items[index]);
				this.PrivateConnectChild(index, value as ColumnDefinition);
			}
		}

		// Token: 0x17001C08 RID: 7176
		public ColumnDefinition this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return (ColumnDefinition)this._items[index];
			}
			set
			{
				this.PrivateVerifyWriteAccess();
				this.PrivateValidateValueForAddition(value);
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				this.PrivateDisconnectChild(this._items[index]);
				this.PrivateConnectChild(index, value);
			}
		}

		// Token: 0x06007917 RID: 30999 RVA: 0x00301F55 File Offset: 0x00300F55
		internal void InternalTrimToSize()
		{
			this.PrivateSetCapacity(this._size);
		}

		// Token: 0x17001C09 RID: 7177
		// (get) Token: 0x06007918 RID: 31000 RVA: 0x00301E3A File Offset: 0x00300E3A
		internal int InternalCount
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001C0A RID: 7178
		// (get) Token: 0x06007919 RID: 31001 RVA: 0x00301F63 File Offset: 0x00300F63
		internal DefinitionBase[] InternalItems
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x0600791A RID: 31002 RVA: 0x00301F6B File Offset: 0x00300F6B
		private void PrivateVerifyWriteAccess()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("GridCollection_CannotModifyReadOnly", new object[]
				{
					"ColumnDefinitionCollection"
				}));
			}
		}

		// Token: 0x0600791B RID: 31003 RVA: 0x00301F94 File Offset: 0x00300F94
		private void PrivateValidateValueForAddition(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ColumnDefinition columnDefinition = value as ColumnDefinition;
			if (columnDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"ColumnDefinitionCollection",
					"ColumnDefinition"
				}));
			}
			if (columnDefinition.Parent != null)
			{
				throw new ArgumentException(SR.Get("GridCollection_InOtherCollection", new object[]
				{
					"value",
					"ColumnDefinitionCollection"
				}));
			}
		}

		// Token: 0x0600791C RID: 31004 RVA: 0x00302010 File Offset: 0x00301010
		private bool PrivateValidateValueForRemoval(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ColumnDefinition columnDefinition = value as ColumnDefinition;
			if (columnDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"ColumnDefinitionCollection",
					"ColumnDefinition"
				}));
			}
			return columnDefinition.Parent == this._owner;
		}

		// Token: 0x0600791D RID: 31005 RVA: 0x00302069 File Offset: 0x00301069
		private void PrivateConnectChild(int index, DefinitionBase value)
		{
			this._items[index] = value;
			value.Index = index;
			this._owner.AddLogicalChild(value);
			value.OnEnterParentTree();
		}

		// Token: 0x0600791E RID: 31006 RVA: 0x0030208D File Offset: 0x0030108D
		private void PrivateDisconnectChild(DefinitionBase value)
		{
			value.OnExitParentTree();
			this._items[value.Index] = null;
			value.Index = -1;
			this._owner.RemoveLogicalChild(value);
		}

		// Token: 0x0600791F RID: 31007 RVA: 0x003020B8 File Offset: 0x003010B8
		private void PrivateInsert(int index, DefinitionBase value)
		{
			this.PrivateOnModified();
			if (this._items == null)
			{
				this.PrivateSetCapacity(4);
			}
			else if (this._size == this._items.Length)
			{
				this.PrivateSetCapacity(Math.Max(this._items.Length * 2, 4));
			}
			for (int i = this._size - 1; i >= index; i--)
			{
				this._items[i + 1] = this._items[i];
				this._items[i].Index = i + 1;
			}
			this._items[index] = null;
			this._size++;
			this.PrivateConnectChild(index, value);
		}

		// Token: 0x06007920 RID: 31008 RVA: 0x00302158 File Offset: 0x00301158
		private void PrivateRemove(DefinitionBase value)
		{
			this.PrivateOnModified();
			int index = value.Index;
			this.PrivateDisconnectChild(value);
			this._size--;
			for (int i = index; i < this._size; i++)
			{
				this._items[i] = this._items[i + 1];
				this._items[i].Index = i;
			}
			this._items[this._size] = null;
		}

		// Token: 0x06007921 RID: 31009 RVA: 0x003021C4 File Offset: 0x003011C4
		private void PrivateOnModified()
		{
			this._version++;
			this._owner.ColumnDefinitionCollectionDirty = true;
			this._owner.Invalidate();
		}

		// Token: 0x06007922 RID: 31010 RVA: 0x003021EC File Offset: 0x003011EC
		private void PrivateSetCapacity(int value)
		{
			if (value <= 0)
			{
				this._items = null;
				return;
			}
			if (this._items == null || value != this._items.Length)
			{
				ColumnDefinition[] array = new ColumnDefinition[value];
				if (this._size > 0)
				{
					Array.Copy(this._items, 0, array, 0, this._size);
				}
				DefinitionBase[] items = array;
				this._items = items;
			}
		}

		// Token: 0x04003992 RID: 14738
		private readonly Grid _owner;

		// Token: 0x04003993 RID: 14739
		private DefinitionBase[] _items;

		// Token: 0x04003994 RID: 14740
		private int _size;

		// Token: 0x04003995 RID: 14741
		private int _version;

		// Token: 0x04003996 RID: 14742
		private const int c_defaultCapacity = 4;

		// Token: 0x02000C42 RID: 3138
		internal struct Enumerator : IEnumerator<ColumnDefinition>, IEnumerator, IDisposable
		{
			// Token: 0x0600915E RID: 37214 RVA: 0x003490C7 File Offset: 0x003480C7
			internal Enumerator(ColumnDefinitionCollection collection)
			{
				this._collection = collection;
				this._index = -1;
				this._version = ((this._collection != null) ? this._collection._version : -1);
				this._currentElement = collection;
			}

			// Token: 0x0600915F RID: 37215 RVA: 0x003490FC File Offset: 0x003480FC
			public bool MoveNext()
			{
				if (this._collection == null)
				{
					return false;
				}
				this.PrivateValidate();
				if (this._index < this._collection._size - 1)
				{
					this._index++;
					this._currentElement = this._collection[this._index];
					return true;
				}
				this._currentElement = this._collection;
				this._index = this._collection._size;
				return false;
			}

			// Token: 0x17001FD4 RID: 8148
			// (get) Token: 0x06009160 RID: 37216 RVA: 0x00349173 File Offset: 0x00348173
			object IEnumerator.Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x17001FD5 RID: 8149
			// (get) Token: 0x06009161 RID: 37217 RVA: 0x003491B4 File Offset: 0x003481B4
			public ColumnDefinition Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return (ColumnDefinition)this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x06009162 RID: 37218 RVA: 0x00349203 File Offset: 0x00348203
			public void Reset()
			{
				if (this._collection == null)
				{
					return;
				}
				this.PrivateValidate();
				this._currentElement = this._collection;
				this._index = -1;
			}

			// Token: 0x06009163 RID: 37219 RVA: 0x00349227 File Offset: 0x00348227
			public void Dispose()
			{
				this._currentElement = null;
			}

			// Token: 0x06009164 RID: 37220 RVA: 0x00349230 File Offset: 0x00348230
			private void PrivateValidate()
			{
				if (this._currentElement == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorCollectionDisposed"));
				}
				if (this._version != this._collection._version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
			}

			// Token: 0x04004C0D RID: 19469
			private ColumnDefinitionCollection _collection;

			// Token: 0x04004C0E RID: 19470
			private int _index;

			// Token: 0x04004C0F RID: 19471
			private int _version;

			// Token: 0x04004C10 RID: 19472
			private object _currentElement;
		}
	}
}
