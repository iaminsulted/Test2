using System;
using System.Collections;

namespace MS.Internal
{
	// Token: 0x02000105 RID: 261
	internal sealed class WeakObjectHashtable : Hashtable, IWeakHashtable
	{
		// Token: 0x06000641 RID: 1601 RVA: 0x001062AB File Offset: 0x001052AB
		internal WeakObjectHashtable() : base(WeakObjectHashtable._comparer)
		{
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x001062B8 File Offset: 0x001052B8
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this.WrapKey(ref key);
			this[key] = value;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x001062D0 File Offset: 0x001052D0
		private void WrapKey(ref object key)
		{
			if (key != null && !key.GetType().IsValueType)
			{
				key = new WeakObjectHashtable.EqualityWeakReference(key);
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x001062F0 File Offset: 0x001052F0
		public object UnwrapKey(object key)
		{
			WeakObjectHashtable.EqualityWeakReference equalityWeakReference = key as WeakObjectHashtable.EqualityWeakReference;
			if (equalityWeakReference == null)
			{
				return key;
			}
			return equalityWeakReference.Target;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00106310 File Offset: 0x00105310
		private void ScavengeKeys()
		{
			int count = this.Count;
			if (count == 0)
			{
				return;
			}
			if (this._lastHashCount == 0)
			{
				this._lastHashCount = count;
				return;
			}
			long totalMemory = GC.GetTotalMemory(false);
			if (this._lastGlobalMem == 0L)
			{
				this._lastGlobalMem = totalMemory;
				return;
			}
			long num = totalMemory - this._lastGlobalMem;
			long num2 = (long)(count - this._lastHashCount);
			if (num < 0L && num2 >= 0L)
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Keys)
				{
					WeakObjectHashtable.EqualityWeakReference equalityWeakReference = obj as WeakObjectHashtable.EqualityWeakReference;
					if (equalityWeakReference != null && !equalityWeakReference.IsAlive)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(equalityWeakReference);
					}
				}
				if (arrayList != null)
				{
					foreach (object key in arrayList)
					{
						this.Remove(key);
					}
				}
			}
			this._lastGlobalMem = totalMemory;
			this._lastHashCount = count;
		}

		// Token: 0x040006F9 RID: 1785
		private static IEqualityComparer _comparer = new WeakObjectHashtable.WeakKeyComparer();

		// Token: 0x040006FA RID: 1786
		private long _lastGlobalMem;

		// Token: 0x040006FB RID: 1787
		private int _lastHashCount;

		// Token: 0x020008BA RID: 2234
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x0600810D RID: 33037 RVA: 0x003229CC File Offset: 0x003219CC
			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					return y == null;
				}
				if (y == null || x.GetHashCode() != y.GetHashCode())
				{
					return false;
				}
				if (x == y)
				{
					return true;
				}
				WeakObjectHashtable.EqualityWeakReference equalityWeakReference;
				if ((equalityWeakReference = (x as WeakObjectHashtable.EqualityWeakReference)) != null)
				{
					x = equalityWeakReference.Target;
					if (x == null)
					{
						return false;
					}
				}
				WeakObjectHashtable.EqualityWeakReference equalityWeakReference2;
				if ((equalityWeakReference2 = (y as WeakObjectHashtable.EqualityWeakReference)) != null)
				{
					y = equalityWeakReference2.Target;
					if (y == null)
					{
						return false;
					}
				}
				return object.Equals(x, y);
			}

			// Token: 0x0600810E RID: 33038 RVA: 0x00322958 File Offset: 0x00321958
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x020008BB RID: 2235
		internal sealed class EqualityWeakReference
		{
			// Token: 0x06008110 RID: 33040 RVA: 0x00322A2F File Offset: 0x00321A2F
			internal EqualityWeakReference(object o)
			{
				this._weakRef = new WeakReference(o);
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x17001D8E RID: 7566
			// (get) Token: 0x06008111 RID: 33041 RVA: 0x00322A4F File Offset: 0x00321A4F
			public bool IsAlive
			{
				get
				{
					return this._weakRef.IsAlive;
				}
			}

			// Token: 0x17001D8F RID: 7567
			// (get) Token: 0x06008112 RID: 33042 RVA: 0x00322A5C File Offset: 0x00321A5C
			public object Target
			{
				get
				{
					return this._weakRef.Target;
				}
			}

			// Token: 0x06008113 RID: 33043 RVA: 0x00322A69 File Offset: 0x00321A69
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || o == this.Target);
			}

			// Token: 0x06008114 RID: 33044 RVA: 0x00322A90 File Offset: 0x00321A90
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x04003C22 RID: 15394
			private int _hashCode;

			// Token: 0x04003C23 RID: 15395
			private WeakReference _weakRef;
		}
	}
}
