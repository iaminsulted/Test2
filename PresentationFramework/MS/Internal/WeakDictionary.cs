using System;
using System.Collections;
using System.Collections.Generic;

namespace MS.Internal
{
	// Token: 0x02000102 RID: 258
	internal class WeakDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where TKey : class
	{
		// Token: 0x0600061D RID: 1565 RVA: 0x00105CFE File Offset: 0x00104CFE
		public void Add(TKey key, TValue value)
		{
			this._hashTable.SetWeak(key, value);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00105D17 File Offset: 0x00104D17
		public bool ContainsKey(TKey key)
		{
			return this._hashTable.ContainsKey(key);
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00105D2A File Offset: 0x00104D2A
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new WeakDictionary<TKey, TValue>.KeyCollection<TKey, TValue>(this);
				}
				return this._keys;
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00105D46 File Offset: 0x00104D46
		public bool Remove(TKey key)
		{
			if (this._hashTable.ContainsKey(key))
			{
				this._hashTable.Remove(key);
				return true;
			}
			return false;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00105D6F File Offset: 0x00104D6F
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._hashTable.ContainsKey(key))
			{
				value = (TValue)((object)this._hashTable[key]);
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x00105DAA File Offset: 0x00104DAA
		public ICollection<TValue> Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new WeakDictionary<TKey, TValue>.ValueCollection<TKey, TValue>(this);
				}
				return this._values;
			}
		}

		// Token: 0x170000EA RID: 234
		public TValue this[TKey key]
		{
			get
			{
				if (!this._hashTable.ContainsKey(key))
				{
					throw new KeyNotFoundException();
				}
				return (TValue)((object)this._hashTable[key]);
			}
			set
			{
				this._hashTable.SetWeak(key, value);
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00105DF7 File Offset: 0x00104DF7
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00105E0D File Offset: 0x00104E0D
		public void Clear()
		{
			this._hashTable.Clear();
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00105E1C File Offset: 0x00104E1C
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this._hashTable.ContainsKey(item.Key) && object.Equals(this._hashTable[item.Key], item.Value);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00105E74 File Offset: 0x00104E74
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = 0;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
			{
				num++;
			}
			if (num + arrayIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair2 in this)
			{
				array[arrayIndex++] = keyValuePair2;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x00105F28 File Offset: 0x00104F28
		public int Count
		{
			get
			{
				return this._hashTable.Count;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00105F38 File Offset: 0x00104F38
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00105F52 File Offset: 0x00104F52
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (object key in this._hashTable.Keys)
			{
				TKey tkey = this._hashTable.UnwrapKey(key) as TKey;
				if (tkey != null)
				{
					yield return new KeyValuePair<TKey, TValue>(tkey, (TValue)((object)this._hashTable[key]));
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00105F61 File Offset: 0x00104F61
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040006F2 RID: 1778
		private IWeakHashtable _hashTable = WeakHashtable.FromKeyType(typeof(TKey));

		// Token: 0x040006F3 RID: 1779
		private WeakDictionary<TKey, TValue>.KeyCollection<TKey, TValue> _keys;

		// Token: 0x040006F4 RID: 1780
		private WeakDictionary<TKey, TValue>.ValueCollection<TKey, TValue> _values;

		// Token: 0x020008B4 RID: 2228
		private class KeyCollection<KeyType, ValueType> : ICollection<KeyType>, IEnumerable<KeyType>, IEnumerable where KeyType : class
		{
			// Token: 0x060080DF RID: 32991 RVA: 0x00322593 File Offset: 0x00321593
			public KeyCollection(WeakDictionary<KeyType, ValueType> dict)
			{
				this.Dict = dict;
			}

			// Token: 0x17001D82 RID: 7554
			// (get) Token: 0x060080E0 RID: 32992 RVA: 0x003225A2 File Offset: 0x003215A2
			// (set) Token: 0x060080E1 RID: 32993 RVA: 0x003225AA File Offset: 0x003215AA
			public WeakDictionary<KeyType, ValueType> Dict { get; private set; }

			// Token: 0x060080E2 RID: 32994 RVA: 0x001056E1 File Offset: 0x001046E1
			public void Add(KeyType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060080E3 RID: 32995 RVA: 0x001056E1 File Offset: 0x001046E1
			public void Clear()
			{
				throw new NotImplementedException();
			}

			// Token: 0x060080E4 RID: 32996 RVA: 0x003225B3 File Offset: 0x003215B3
			public bool Contains(KeyType item)
			{
				return this.Dict.ContainsKey(item);
			}

			// Token: 0x060080E5 RID: 32997 RVA: 0x001056E1 File Offset: 0x001046E1
			public void CopyTo(KeyType[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			// Token: 0x17001D83 RID: 7555
			// (get) Token: 0x060080E6 RID: 32998 RVA: 0x003225C1 File Offset: 0x003215C1
			public int Count
			{
				get
				{
					return this.Dict.Count;
				}
			}

			// Token: 0x17001D84 RID: 7556
			// (get) Token: 0x060080E7 RID: 32999 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060080E8 RID: 33000 RVA: 0x001056E1 File Offset: 0x001046E1
			public bool Remove(KeyType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060080E9 RID: 33001 RVA: 0x003225CE File Offset: 0x003215CE
			public IEnumerator<KeyType> GetEnumerator()
			{
				IWeakHashtable hashTable = this.Dict._hashTable;
				foreach (object key in hashTable.Keys)
				{
					KeyType keyType = hashTable.UnwrapKey(key) as KeyType;
					if (keyType != null)
					{
						yield return keyType;
					}
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x060080EA RID: 33002 RVA: 0x003225DD File Offset: 0x003215DD
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}

		// Token: 0x020008B5 RID: 2229
		private class ValueCollection<KeyType, ValueType> : ICollection<ValueType>, IEnumerable<ValueType>, IEnumerable where KeyType : class
		{
			// Token: 0x060080EB RID: 33003 RVA: 0x003225E5 File Offset: 0x003215E5
			public ValueCollection(WeakDictionary<KeyType, ValueType> dict)
			{
				this.Dict = dict;
			}

			// Token: 0x17001D85 RID: 7557
			// (get) Token: 0x060080EC RID: 33004 RVA: 0x003225F4 File Offset: 0x003215F4
			// (set) Token: 0x060080ED RID: 33005 RVA: 0x003225FC File Offset: 0x003215FC
			public WeakDictionary<KeyType, ValueType> Dict { get; private set; }

			// Token: 0x060080EE RID: 33006 RVA: 0x001056E1 File Offset: 0x001046E1
			public void Add(ValueType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060080EF RID: 33007 RVA: 0x001056E1 File Offset: 0x001046E1
			public void Clear()
			{
				throw new NotImplementedException();
			}

			// Token: 0x060080F0 RID: 33008 RVA: 0x001056E1 File Offset: 0x001046E1
			public bool Contains(ValueType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060080F1 RID: 33009 RVA: 0x001056E1 File Offset: 0x001046E1
			public void CopyTo(ValueType[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			// Token: 0x17001D86 RID: 7558
			// (get) Token: 0x060080F2 RID: 33010 RVA: 0x00322605 File Offset: 0x00321605
			public int Count
			{
				get
				{
					return this.Dict.Count;
				}
			}

			// Token: 0x17001D87 RID: 7559
			// (get) Token: 0x060080F3 RID: 33011 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060080F4 RID: 33012 RVA: 0x001056E1 File Offset: 0x001046E1
			public bool Remove(ValueType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060080F5 RID: 33013 RVA: 0x00322612 File Offset: 0x00321612
			public IEnumerator<ValueType> GetEnumerator()
			{
				IWeakHashtable hashTable = this.Dict._hashTable;
				foreach (object key in hashTable.Keys)
				{
					if (hashTable.UnwrapKey(key) is KeyType)
					{
						yield return (ValueType)((object)hashTable[key]);
					}
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x060080F6 RID: 33014 RVA: 0x00322621 File Offset: 0x00321621
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}
