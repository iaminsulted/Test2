using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Markup.Localizer
{
	// Token: 0x0200053F RID: 1343
	public sealed class BamlLocalizationDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x0600426C RID: 17004 RVA: 0x0021B691 File Offset: 0x0021A691
		public BamlLocalizationDictionary()
		{
			this._dictionary = new Dictionary<BamlLocalizableResourceKey, BamlLocalizableResource>();
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x0600426D RID: 17005 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x0600426E RID: 17006 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x0600426F RID: 17007 RVA: 0x0021B6A4 File Offset: 0x0021A6A4
		public BamlLocalizableResourceKey RootElementKey
		{
			get
			{
				return this._rootElementKey;
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06004270 RID: 17008 RVA: 0x0021B6AC File Offset: 0x0021A6AC
		public ICollection Keys
		{
			get
			{
				return ((IDictionary)this._dictionary).Keys;
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06004271 RID: 17009 RVA: 0x0021B6BE File Offset: 0x0021A6BE
		public ICollection Values
		{
			get
			{
				return ((IDictionary)this._dictionary).Values;
			}
		}

		// Token: 0x17000F00 RID: 3840
		public BamlLocalizableResource this[BamlLocalizableResourceKey key]
		{
			get
			{
				this.CheckNonNullParam(key, "key");
				return this._dictionary[key];
			}
			set
			{
				this.CheckNonNullParam(key, "key");
				this._dictionary[key] = value;
			}
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x0021B705 File Offset: 0x0021A705
		public void Add(BamlLocalizableResourceKey key, BamlLocalizableResource value)
		{
			this.CheckNonNullParam(key, "key");
			this._dictionary.Add(key, value);
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x0021B720 File Offset: 0x0021A720
		public void Clear()
		{
			this._dictionary.Clear();
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x0021B72D File Offset: 0x0021A72D
		public void Remove(BamlLocalizableResourceKey key)
		{
			this._dictionary.Remove(key);
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x0021B73C File Offset: 0x0021A73C
		public bool Contains(BamlLocalizableResourceKey key)
		{
			this.CheckNonNullParam(key, "key");
			return this._dictionary.ContainsKey(key);
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x0021B756 File Offset: 0x0021A756
		public BamlLocalizationDictionaryEnumerator GetEnumerator()
		{
			return new BamlLocalizationDictionaryEnumerator(((IDictionary)this._dictionary).GetEnumerator());
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06004279 RID: 17017 RVA: 0x0021B76D File Offset: 0x0021A76D
		public int Count
		{
			get
			{
				return this._dictionary.Count;
			}
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x0021B77C File Offset: 0x0021A77C
		public void CopyTo(DictionaryEntry[] array, int arrayIndex)
		{
			this.CheckNonNullParam(array, "array");
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", SR.Get("ParameterCannotBeNegative"));
			}
			if (arrayIndex >= array.Length)
			{
				throw new ArgumentException(SR.Get("Collection_CopyTo_IndexGreaterThanOrEqualToArrayLength", new object[]
				{
					"arrayIndex",
					"array"
				}), "arrayIndex");
			}
			if (this.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException(SR.Get("Collection_CopyTo_NumberOfElementsExceedsArrayLength", new object[]
				{
					"arrayIndex",
					"array"
				}));
			}
			foreach (KeyValuePair<BamlLocalizableResourceKey, BamlLocalizableResource> keyValuePair in this._dictionary)
			{
				DictionaryEntry dictionaryEntry = new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
				array[arrayIndex++] = dictionaryEntry;
			}
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x0021B870 File Offset: 0x0021A870
		bool IDictionary.Contains(object key)
		{
			this.CheckNonNullParam(key, "key");
			return ((IDictionary)this._dictionary).Contains(key);
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x0021B88F File Offset: 0x0021A88F
		void IDictionary.Add(object key, object value)
		{
			this.CheckNonNullParam(key, "key");
			((IDictionary)this._dictionary).Add(key, value);
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x0021B8AF File Offset: 0x0021A8AF
		void IDictionary.Remove(object key)
		{
			this.CheckNonNullParam(key, "key");
			((IDictionary)this._dictionary).Remove(key);
		}

		// Token: 0x17000F02 RID: 3842
		object IDictionary.this[object key]
		{
			get
			{
				this.CheckNonNullParam(key, "key");
				return ((IDictionary)this._dictionary)[key];
			}
			set
			{
				this.CheckNonNullParam(key, "key");
				((IDictionary)this._dictionary)[key] = value;
			}
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x0021B90D File Offset: 0x0021A90D
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x0021B915 File Offset: 0x0021A915
		void ICollection.CopyTo(Array array, int index)
		{
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException(SR.Get("Collection_CopyTo_ArrayCannotBeMultidimensional"), "array");
			}
			this.CopyTo(array as DictionaryEntry[], index);
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06004282 RID: 17026 RVA: 0x0021B945 File Offset: 0x0021A945
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06004283 RID: 17027 RVA: 0x0021B94D File Offset: 0x0021A94D
		object ICollection.SyncRoot
		{
			get
			{
				return ((IDictionary)this._dictionary).SyncRoot;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06004284 RID: 17028 RVA: 0x0021B95F File Offset: 0x0021A95F
		bool ICollection.IsSynchronized
		{
			get
			{
				return ((IDictionary)this._dictionary).IsSynchronized;
			}
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x0021B90D File Offset: 0x0021A90D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x0021B974 File Offset: 0x0021A974
		internal BamlLocalizationDictionary Copy()
		{
			BamlLocalizationDictionary bamlLocalizationDictionary = new BamlLocalizationDictionary();
			foreach (KeyValuePair<BamlLocalizableResourceKey, BamlLocalizableResource> keyValuePair in this._dictionary)
			{
				BamlLocalizableResource value = (keyValuePair.Value == null) ? null : new BamlLocalizableResource(keyValuePair.Value);
				bamlLocalizationDictionary.Add(keyValuePair.Key, value);
			}
			bamlLocalizationDictionary._rootElementKey = this._rootElementKey;
			return bamlLocalizationDictionary;
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x0021B9F4 File Offset: 0x0021A9F4
		internal void SetRootElementKey(BamlLocalizableResourceKey key)
		{
			this._rootElementKey = key;
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x0021B9FD File Offset: 0x0021A9FD
		private void CheckNonNullParam(object param, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		// Token: 0x040024FE RID: 9470
		private IDictionary<BamlLocalizableResourceKey, BamlLocalizableResource> _dictionary;

		// Token: 0x040024FF RID: 9471
		private BamlLocalizableResourceKey _rootElementKey;
	}
}
