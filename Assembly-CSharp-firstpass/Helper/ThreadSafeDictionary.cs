using System;
using System.Collections.Generic;

namespace Helper
{
	// Token: 0x02000037 RID: 55
	public class ThreadSafeDictionary<TKey, TValue>
	{
		// Token: 0x1700000B RID: 11
		public TValue this[TKey key]
		{
			get
			{
				Dictionary<TKey, TValue> impl = this._impl;
				TValue result;
				lock (impl)
				{
					result = this._impl[key];
				}
				return result;
			}
			set
			{
				Dictionary<TKey, TValue> impl = this._impl;
				lock (impl)
				{
					this._impl[key] = value;
				}
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00016B54 File Offset: 0x00014D54
		public void Add(TKey key, TValue value)
		{
			Dictionary<TKey, TValue> impl = this._impl;
			lock (impl)
			{
				this._impl.Add(key, value);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00016B9C File Offset: 0x00014D9C
		public bool TryGetValue(TKey key, out TValue value)
		{
			Dictionary<TKey, TValue> impl = this._impl;
			bool result;
			lock (impl)
			{
				result = this._impl.TryGetValue(key, out value);
			}
			return result;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00016BE8 File Offset: 0x00014DE8
		public bool Remove(TKey key)
		{
			Dictionary<TKey, TValue> impl = this._impl;
			bool result;
			lock (impl)
			{
				result = this._impl.Remove(key);
			}
			return result;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00016C30 File Offset: 0x00014E30
		public void Clear()
		{
			Dictionary<TKey, TValue> impl = this._impl;
			lock (impl)
			{
				this._impl.Clear();
			}
		}

		// Token: 0x040001F3 RID: 499
		protected readonly Dictionary<TKey, TValue> _impl = new Dictionary<TKey, TValue>();
	}
}
