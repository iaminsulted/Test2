using System;
using System.Collections;
using System.Collections.Generic;

namespace MS.Internal
{
	// Token: 0x02000103 RID: 259
	internal class WeakHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : class
	{
		// Token: 0x0600062F RID: 1583 RVA: 0x00105F86 File Offset: 0x00104F86
		public void Add(T item)
		{
			if (!this._hashTable.ContainsKey(item))
			{
				this._hashTable.SetWeak(item, null);
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00105FAD File Offset: 0x00104FAD
		public void Clear()
		{
			this._hashTable.Clear();
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00105FBA File Offset: 0x00104FBA
		public bool Contains(T item)
		{
			return this._hashTable.ContainsKey(item);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00105FD0 File Offset: 0x00104FD0
		public void CopyTo(T[] array, int arrayIndex)
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
			foreach (T t in this)
			{
				num++;
			}
			if (num + arrayIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			foreach (T t2 in this)
			{
				array[arrayIndex++] = t2;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00106084 File Offset: 0x00105084
		public int Count
		{
			get
			{
				return this._hashTable.Count;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00106091 File Offset: 0x00105091
		public bool Remove(T item)
		{
			if (this._hashTable.ContainsKey(item))
			{
				this._hashTable.Remove(item);
				return true;
			}
			return false;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x001060BA File Offset: 0x001050BA
		public IEnumerator<T> GetEnumerator()
		{
			foreach (object obj in this._hashTable.Keys)
			{
				WeakHashtable.EqualityWeakReference equalityWeakReference = obj as WeakHashtable.EqualityWeakReference;
				if (equalityWeakReference != null)
				{
					T t = equalityWeakReference.Target as T;
					if (t != null)
					{
						yield return t;
					}
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x001060C9 File Offset: 0x001050C9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040006F5 RID: 1781
		private WeakHashtable _hashTable = new WeakHashtable();
	}
}
