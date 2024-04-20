using System;
using System.Collections;

namespace System.Windows
{
	// Token: 0x020003E7 RID: 999
	public sealed class WindowCollection : ICollection, IEnumerable
	{
		// Token: 0x06002AF2 RID: 10994 RVA: 0x001A0AB0 File Offset: 0x0019FAB0
		public WindowCollection()
		{
			this._list = new ArrayList(1);
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x001A0AC4 File Offset: 0x0019FAC4
		internal WindowCollection(int count)
		{
			this._list = new ArrayList(count);
		}

		// Token: 0x170009FD RID: 2557
		public Window this[int index]
		{
			get
			{
				return this._list[index] as Window;
			}
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x001A0AEB File Offset: 0x0019FAEB
		public IEnumerator GetEnumerator()
		{
			return this._list.GetEnumerator();
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x001A0AF8 File Offset: 0x0019FAF8
		void ICollection.CopyTo(Array array, int index)
		{
			this._list.CopyTo(array, index);
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x001A0AF8 File Offset: 0x0019FAF8
		public void CopyTo(Window[] array, int index)
		{
			this._list.CopyTo(array, index);
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x001A0B07 File Offset: 0x0019FB07
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06002AF9 RID: 11001 RVA: 0x001A0B14 File Offset: 0x0019FB14
		public bool IsSynchronized
		{
			get
			{
				return this._list.IsSynchronized;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06002AFA RID: 11002 RVA: 0x001A0B21 File Offset: 0x0019FB21
		public object SyncRoot
		{
			get
			{
				return this._list.SyncRoot;
			}
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x001A0B30 File Offset: 0x0019FB30
		internal WindowCollection Clone()
		{
			object syncRoot = this._list.SyncRoot;
			WindowCollection windowCollection;
			lock (syncRoot)
			{
				windowCollection = new WindowCollection(this._list.Count);
				for (int i = 0; i < this._list.Count; i++)
				{
					windowCollection._list.Add(this._list[i]);
				}
			}
			return windowCollection;
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x001A0BB0 File Offset: 0x0019FBB0
		internal void Remove(Window win)
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Remove(win);
			}
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x001A0BFC File Offset: 0x0019FBFC
		internal void RemoveAt(int index)
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Remove(index);
			}
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x001A0C4C File Offset: 0x0019FC4C
		internal int Add(Window win)
		{
			object syncRoot = this._list.SyncRoot;
			int result;
			lock (syncRoot)
			{
				result = this._list.Add(win);
			}
			return result;
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x001A0C9C File Offset: 0x0019FC9C
		internal bool HasItem(Window win)
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				for (int i = 0; i < this._list.Count; i++)
				{
					if (this._list[i] == win)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04001578 RID: 5496
		private ArrayList _list;
	}
}
