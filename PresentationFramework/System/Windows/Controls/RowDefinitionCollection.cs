using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Controls
{
	// Token: 0x0200081F RID: 2079
	public sealed class RowDefinitionCollection : IList<RowDefinition>, ICollection<RowDefinition>, IEnumerable<RowDefinition>, IEnumerable, IList, ICollection
	{
		// Token: 0x0600792D RID: 31021 RVA: 0x003023ED File Offset: 0x003013ED
		internal RowDefinitionCollection(Grid owner)
		{
			this._owner = owner;
			this.PrivateOnModified();
		}

		// Token: 0x0600792E RID: 31022 RVA: 0x00302404 File Offset: 0x00301404
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

		// Token: 0x0600792F RID: 31023 RVA: 0x003024A8 File Offset: 0x003014A8
		public void CopyTo(RowDefinition[] array, int index)
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

		// Token: 0x06007930 RID: 31024 RVA: 0x0030252D File Offset: 0x0030152D
		int IList.Add(object value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value as RowDefinition);
			return this._size - 1;
		}

		// Token: 0x06007931 RID: 31025 RVA: 0x00302556 File Offset: 0x00301556
		public void Add(RowDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value);
		}

		// Token: 0x06007932 RID: 31026 RVA: 0x00302574 File Offset: 0x00301574
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

		// Token: 0x06007933 RID: 31027 RVA: 0x003025BC File Offset: 0x003015BC
		bool IList.Contains(object value)
		{
			RowDefinition rowDefinition = value as RowDefinition;
			return rowDefinition != null && rowDefinition.Parent == this._owner;
		}

		// Token: 0x06007934 RID: 31028 RVA: 0x003025E4 File Offset: 0x003015E4
		public bool Contains(RowDefinition value)
		{
			return value != null && value.Parent == this._owner;
		}

		// Token: 0x06007935 RID: 31029 RVA: 0x003025FA File Offset: 0x003015FA
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as RowDefinition);
		}

		// Token: 0x06007936 RID: 31030 RVA: 0x00302608 File Offset: 0x00301608
		public int IndexOf(RowDefinition value)
		{
			if (value == null || value.Parent != this._owner)
			{
				return -1;
			}
			return value.Index;
		}

		// Token: 0x06007937 RID: 31031 RVA: 0x00302623 File Offset: 0x00301623
		void IList.Insert(int index, object value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value as RowDefinition);
		}

		// Token: 0x06007938 RID: 31032 RVA: 0x0030265C File Offset: 0x0030165C
		public void Insert(int index, RowDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value);
		}

		// Token: 0x06007939 RID: 31033 RVA: 0x00302690 File Offset: 0x00301690
		void IList.Remove(object value)
		{
			this.PrivateVerifyWriteAccess();
			if (this.PrivateValidateValueForRemoval(value))
			{
				this.PrivateRemove(value as RowDefinition);
			}
		}

		// Token: 0x0600793A RID: 31034 RVA: 0x003026AD File Offset: 0x003016AD
		public bool Remove(RowDefinition value)
		{
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value);
			}
			return flag;
		}

		// Token: 0x0600793B RID: 31035 RVA: 0x003026C0 File Offset: 0x003016C0
		public void RemoveAt(int index)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateRemove(this._items[index]);
		}

		// Token: 0x0600793C RID: 31036 RVA: 0x003026F4 File Offset: 0x003016F4
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

		// Token: 0x0600793D RID: 31037 RVA: 0x003027C5 File Offset: 0x003017C5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new RowDefinitionCollection.Enumerator(this);
		}

		// Token: 0x0600793E RID: 31038 RVA: 0x003027C5 File Offset: 0x003017C5
		IEnumerator<RowDefinition> IEnumerable<RowDefinition>.GetEnumerator()
		{
			return new RowDefinitionCollection.Enumerator(this);
		}

		// Token: 0x17001C10 RID: 7184
		// (get) Token: 0x0600793F RID: 31039 RVA: 0x003027D2 File Offset: 0x003017D2
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001C11 RID: 7185
		// (get) Token: 0x06007940 RID: 31040 RVA: 0x003027DA File Offset: 0x003017DA
		bool IList.IsFixedSize
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		// Token: 0x17001C12 RID: 7186
		// (get) Token: 0x06007941 RID: 31041 RVA: 0x003027DA File Offset: 0x003017DA
		public bool IsReadOnly
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		// Token: 0x17001C13 RID: 7187
		// (get) Token: 0x06007942 RID: 31042 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C14 RID: 7188
		// (get) Token: 0x06007943 RID: 31043 RVA: 0x000F93D3 File Offset: 0x000F83D3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001C15 RID: 7189
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
				this.PrivateConnectChild(index, value as RowDefinition);
			}
		}

		// Token: 0x17001C16 RID: 7190
		public RowDefinition this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return (RowDefinition)this._items[index];
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

		// Token: 0x06007948 RID: 31048 RVA: 0x003028ED File Offset: 0x003018ED
		internal void InternalTrimToSize()
		{
			this.PrivateSetCapacity(this._size);
		}

		// Token: 0x17001C17 RID: 7191
		// (get) Token: 0x06007949 RID: 31049 RVA: 0x003027D2 File Offset: 0x003017D2
		internal int InternalCount
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001C18 RID: 7192
		// (get) Token: 0x0600794A RID: 31050 RVA: 0x003028FB File Offset: 0x003018FB
		internal DefinitionBase[] InternalItems
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x0600794B RID: 31051 RVA: 0x00302903 File Offset: 0x00301903
		private void PrivateVerifyWriteAccess()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("GridCollection_CannotModifyReadOnly", new object[]
				{
					"RowDefinitionCollection"
				}));
			}
		}

		// Token: 0x0600794C RID: 31052 RVA: 0x0030292C File Offset: 0x0030192C
		private void PrivateValidateValueForAddition(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			RowDefinition rowDefinition = value as RowDefinition;
			if (rowDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"RowDefinitionCollection",
					"RowDefinition"
				}));
			}
			if (rowDefinition.Parent != null)
			{
				throw new ArgumentException(SR.Get("GridCollection_InOtherCollection", new object[]
				{
					"value",
					"RowDefinitionCollection"
				}));
			}
		}

		// Token: 0x0600794D RID: 31053 RVA: 0x003029A8 File Offset: 0x003019A8
		private bool PrivateValidateValueForRemoval(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			RowDefinition rowDefinition = value as RowDefinition;
			if (rowDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"RowDefinitionCollection",
					"RowDefinition"
				}));
			}
			return rowDefinition.Parent == this._owner;
		}

		// Token: 0x0600794E RID: 31054 RVA: 0x00302A01 File Offset: 0x00301A01
		private void PrivateConnectChild(int index, DefinitionBase value)
		{
			this._items[index] = value;
			value.Index = index;
			this._owner.AddLogicalChild(value);
			value.OnEnterParentTree();
		}

		// Token: 0x0600794F RID: 31055 RVA: 0x00302A25 File Offset: 0x00301A25
		private void PrivateDisconnectChild(DefinitionBase value)
		{
			value.OnExitParentTree();
			this._items[value.Index] = null;
			value.Index = -1;
			this._owner.RemoveLogicalChild(value);
		}

		// Token: 0x06007950 RID: 31056 RVA: 0x00302A50 File Offset: 0x00301A50
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

		// Token: 0x06007951 RID: 31057 RVA: 0x00302AF0 File Offset: 0x00301AF0
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

		// Token: 0x06007952 RID: 31058 RVA: 0x00302B5C File Offset: 0x00301B5C
		private void PrivateOnModified()
		{
			this._version++;
			this._owner.RowDefinitionCollectionDirty = true;
			this._owner.Invalidate();
		}

		// Token: 0x06007953 RID: 31059 RVA: 0x00302B84 File Offset: 0x00301B84
		private void PrivateSetCapacity(int value)
		{
			if (value <= 0)
			{
				this._items = null;
				return;
			}
			if (this._items == null || value != this._items.Length)
			{
				RowDefinition[] array = new RowDefinition[value];
				if (this._size > 0)
				{
					Array.Copy(this._items, 0, array, 0, this._size);
				}
				DefinitionBase[] items = array;
				this._items = items;
			}
		}

		// Token: 0x0400399A RID: 14746
		private readonly Grid _owner;

		// Token: 0x0400399B RID: 14747
		private DefinitionBase[] _items;

		// Token: 0x0400399C RID: 14748
		private int _size;

		// Token: 0x0400399D RID: 14749
		private int _version;

		// Token: 0x0400399E RID: 14750
		private const int c_defaultCapacity = 4;

		// Token: 0x02000C43 RID: 3139
		internal struct Enumerator : IEnumerator<RowDefinition>, IEnumerator, IDisposable
		{
			// Token: 0x06009165 RID: 37221 RVA: 0x0034926D File Offset: 0x0034826D
			internal Enumerator(RowDefinitionCollection collection)
			{
				this._collection = collection;
				this._index = -1;
				this._version = ((this._collection != null) ? this._collection._version : -1);
				this._currentElement = collection;
			}

			// Token: 0x06009166 RID: 37222 RVA: 0x003492A0 File Offset: 0x003482A0
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

			// Token: 0x17001FD6 RID: 8150
			// (get) Token: 0x06009167 RID: 37223 RVA: 0x00349317 File Offset: 0x00348317
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

			// Token: 0x17001FD7 RID: 8151
			// (get) Token: 0x06009168 RID: 37224 RVA: 0x00349358 File Offset: 0x00348358
			public RowDefinition Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return (RowDefinition)this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x06009169 RID: 37225 RVA: 0x003493A7 File Offset: 0x003483A7
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

			// Token: 0x0600916A RID: 37226 RVA: 0x003493CB File Offset: 0x003483CB
			public void Dispose()
			{
				this._currentElement = null;
			}

			// Token: 0x0600916B RID: 37227 RVA: 0x003493D4 File Offset: 0x003483D4
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

			// Token: 0x04004C11 RID: 19473
			private RowDefinitionCollection _collection;

			// Token: 0x04004C12 RID: 19474
			private int _index;

			// Token: 0x04004C13 RID: 19475
			private int _version;

			// Token: 0x04004C14 RID: 19476
			private object _currentElement;
		}
	}
}
