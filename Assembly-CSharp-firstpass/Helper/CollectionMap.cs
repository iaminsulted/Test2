using System;
using System.Collections.Generic;

namespace Helper
{
	// Token: 0x02000030 RID: 48
	internal class CollectionMap<TKey, TValue> : ThreadSafeDictionary<TKey, TValue> where TValue : new()
	{
		// Token: 0x060001D7 RID: 471 RVA: 0x00016420 File Offset: 0x00014620
		public bool TryAddDefault(TKey key)
		{
			Dictionary<TKey, TValue> impl = this._impl;
			bool result;
			lock (impl)
			{
				if (!this._impl.ContainsKey(key))
				{
					this._impl.Add(key, Activator.CreateInstance<TValue>());
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
	}
}
