using System;
using System.Collections;

namespace MS.Internal
{
	// Token: 0x02000104 RID: 260
	internal sealed class WeakHashtable : Hashtable, IWeakHashtable
	{
		// Token: 0x06000639 RID: 1593 RVA: 0x001060E4 File Offset: 0x001050E4
		internal WeakHashtable() : base(WeakHashtable._comparer)
		{
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x001060F1 File Offset: 0x001050F1
		public override void Clear()
		{
			base.Clear();
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x001060F9 File Offset: 0x001050F9
		public override void Remove(object key)
		{
			base.Remove(key);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00106102 File Offset: 0x00105102
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this[new WeakHashtable.EqualityWeakReference(key)] = value;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00106118 File Offset: 0x00105118
		public object UnwrapKey(object key)
		{
			WeakHashtable.EqualityWeakReference equalityWeakReference = key as WeakHashtable.EqualityWeakReference;
			if (equalityWeakReference == null)
			{
				return null;
			}
			return equalityWeakReference.Target;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00106138 File Offset: 0x00105138
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
			float num = (float)(totalMemory - this._lastGlobalMem) / (float)this._lastGlobalMem;
			float num2 = (float)(count - this._lastHashCount) / (float)this._lastHashCount;
			if (num < 0f && num2 >= 0f)
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Keys)
				{
					WeakHashtable.EqualityWeakReference equalityWeakReference = obj as WeakHashtable.EqualityWeakReference;
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

		// Token: 0x0600063F RID: 1599 RVA: 0x00106278 File Offset: 0x00105278
		public static IWeakHashtable FromKeyType(Type tKey)
		{
			if (tKey == typeof(object) || tKey.IsValueType)
			{
				return new WeakObjectHashtable();
			}
			return new WeakHashtable();
		}

		// Token: 0x040006F6 RID: 1782
		private static IEqualityComparer _comparer = new WeakHashtable.WeakKeyComparer();

		// Token: 0x040006F7 RID: 1783
		private long _lastGlobalMem;

		// Token: 0x040006F8 RID: 1784
		private int _lastHashCount;

		// Token: 0x020008B8 RID: 2232
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x06008105 RID: 33029 RVA: 0x003228F0 File Offset: 0x003218F0
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
				WeakHashtable.EqualityWeakReference equalityWeakReference = x as WeakHashtable.EqualityWeakReference;
				WeakHashtable.EqualityWeakReference equalityWeakReference2 = y as WeakHashtable.EqualityWeakReference;
				if (equalityWeakReference != null && equalityWeakReference2 != null && !equalityWeakReference2.IsAlive && !equalityWeakReference.IsAlive)
				{
					return true;
				}
				if (equalityWeakReference != null)
				{
					x = equalityWeakReference.Target;
				}
				if (equalityWeakReference2 != null)
				{
					y = equalityWeakReference2.Target;
				}
				return x == y;
			}

			// Token: 0x06008106 RID: 33030 RVA: 0x00322958 File Offset: 0x00321958
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x020008B9 RID: 2233
		internal sealed class EqualityWeakReference
		{
			// Token: 0x06008108 RID: 33032 RVA: 0x00322960 File Offset: 0x00321960
			internal EqualityWeakReference(object o)
			{
				this._weakRef = new WeakReference(o);
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x17001D8C RID: 7564
			// (get) Token: 0x06008109 RID: 33033 RVA: 0x00322980 File Offset: 0x00321980
			public bool IsAlive
			{
				get
				{
					return this._weakRef.IsAlive;
				}
			}

			// Token: 0x17001D8D RID: 7565
			// (get) Token: 0x0600810A RID: 33034 RVA: 0x0032298D File Offset: 0x0032198D
			public object Target
			{
				get
				{
					return this._weakRef.Target;
				}
			}

			// Token: 0x0600810B RID: 33035 RVA: 0x0032299A File Offset: 0x0032199A
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || o == this.Target);
			}

			// Token: 0x0600810C RID: 33036 RVA: 0x003229C1 File Offset: 0x003219C1
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x04003C20 RID: 15392
			private int _hashCode;

			// Token: 0x04003C21 RID: 15393
			private WeakReference _weakRef;
		}
	}
}
