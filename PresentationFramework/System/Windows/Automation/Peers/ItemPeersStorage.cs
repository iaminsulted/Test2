using System;
using System.Collections.Generic;
using MS.Internal;
using MS.Internal.Automation;
using MS.Internal.Hashing.PresentationFramework;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000574 RID: 1396
	internal class ItemPeersStorage<T> where T : class
	{
		// Token: 0x060044CA RID: 17610 RVA: 0x0022247B File Offset: 0x0022147B
		public void Clear()
		{
			this._usesHashCode = false;
			this._count = 0;
			if (this._hashtable != null)
			{
				this._hashtable.Clear();
			}
			if (this._list != null)
			{
				this._list.Clear();
			}
		}

		// Token: 0x17000F73 RID: 3955
		public T this[object item]
		{
			get
			{
				if (this._count == 0 || item == null)
				{
					return default(T);
				}
				if (this._usesHashCode)
				{
					if (this._hashtable == null || !this._hashtable.ContainsKey(item))
					{
						return default(T);
					}
					return this._hashtable[item];
				}
				else
				{
					if (this._list == null)
					{
						return default(T);
					}
					for (int i = 0; i < this._list.Count; i++)
					{
						KeyValuePair<object, T> keyValuePair = this._list[i];
						if (object.Equals(item, keyValuePair.Key))
						{
							return keyValuePair.Value;
						}
					}
					return default(T);
				}
			}
			set
			{
				if (item == null)
				{
					return;
				}
				if (this._count == 0)
				{
					this._usesHashCode = (item != null && HashHelper.HasReliableHashCode(item));
				}
				if (this._usesHashCode)
				{
					if (this._hashtable == null)
					{
						this._hashtable = new WeakDictionary<object, T>();
					}
					if (!this._hashtable.ContainsKey(item) && value != null)
					{
						this._hashtable[item] = value;
					}
				}
				else
				{
					if (this._list == null)
					{
						this._list = new List<KeyValuePair<object, T>>();
					}
					if (value != null)
					{
						this._list.Add(new KeyValuePair<object, T>(item, value));
					}
				}
				this._count++;
			}
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x00222608 File Offset: 0x00221608
		public void Remove(object item)
		{
			if (this._usesHashCode)
			{
				if (item != null && this._hashtable.ContainsKey(item))
				{
					this._hashtable.Remove(item);
					if (!this._hashtable.ContainsKey(item))
					{
						this._count--;
						return;
					}
				}
			}
			else if (this._list != null)
			{
				int num = 0;
				while (num < this._list.Count && !object.Equals(item, this._list[num].Key))
				{
					num++;
				}
				if (num < this._list.Count)
				{
					this._list.RemoveAt(num);
					this._count--;
				}
			}
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x002226C4 File Offset: 0x002216C4
		public void PurgeWeakRefCollection()
		{
			if (!typeof(T).IsAssignableFrom(typeof(WeakReference)))
			{
				return;
			}
			List<object> list = new List<object>();
			if (this._usesHashCode)
			{
				if (this._hashtable == null)
				{
					return;
				}
				using (IEnumerator<KeyValuePair<object, T>> enumerator = this._hashtable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<object, T> keyValuePair = enumerator.Current;
						WeakReference weakReference = keyValuePair.Value as WeakReference;
						if (weakReference == null)
						{
							list.Add(keyValuePair.Key);
						}
						else
						{
							ElementProxy elementProxy = weakReference.Target as ElementProxy;
							if (elementProxy == null)
							{
								list.Add(keyValuePair.Key);
							}
							else if (!(elementProxy.Peer is ItemAutomationPeer))
							{
								list.Add(keyValuePair.Key);
							}
						}
					}
					goto IL_15D;
				}
			}
			if (this._list == null)
			{
				return;
			}
			foreach (KeyValuePair<object, T> keyValuePair2 in this._list)
			{
				WeakReference weakReference2 = keyValuePair2.Value as WeakReference;
				if (weakReference2 == null)
				{
					list.Add(keyValuePair2.Key);
				}
				else
				{
					ElementProxy elementProxy2 = weakReference2.Target as ElementProxy;
					if (elementProxy2 == null)
					{
						list.Add(keyValuePair2.Key);
					}
					else if (!(elementProxy2.Peer is ItemAutomationPeer))
					{
						list.Add(keyValuePair2.Key);
					}
				}
			}
			IL_15D:
			foreach (object item in list)
			{
				this.Remove(item);
			}
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x060044CF RID: 17615 RVA: 0x0022288C File Offset: 0x0022188C
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x04002537 RID: 9527
		private WeakDictionary<object, T> _hashtable;

		// Token: 0x04002538 RID: 9528
		private List<KeyValuePair<object, T>> _list;

		// Token: 0x04002539 RID: 9529
		private int _count;

		// Token: 0x0400253A RID: 9530
		private bool _usesHashCode;
	}
}
