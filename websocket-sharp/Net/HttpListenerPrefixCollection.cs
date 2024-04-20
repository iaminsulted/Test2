using System;
using System.Collections;
using System.Collections.Generic;

namespace WebSocketSharp.Net
{
	// Token: 0x02000023 RID: 35
	public class HttpListenerPrefixCollection : ICollection<string>, IEnumerable<string>, IEnumerable
	{
		// Token: 0x06000286 RID: 646 RVA: 0x000102EC File Offset: 0x0000E4EC
		internal HttpListenerPrefixCollection(HttpListener listener)
		{
			this._listener = listener;
			this._prefixes = new List<string>();
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00010308 File Offset: 0x0000E508
		public int Count
		{
			get
			{
				return this._prefixes.Count;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00010328 File Offset: 0x0000E528
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0001033C File Offset: 0x0000E53C
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00010350 File Offset: 0x0000E550
		public void Add(string uriPrefix)
		{
			this._listener.CheckDisposed();
			HttpListenerPrefix.CheckPrefix(uriPrefix);
			bool flag = this._prefixes.Contains(uriPrefix);
			if (!flag)
			{
				this._prefixes.Add(uriPrefix);
				bool isListening = this._listener.IsListening;
				if (isListening)
				{
					EndPointManager.AddPrefix(uriPrefix, this._listener);
				}
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000103AC File Offset: 0x0000E5AC
		public void Clear()
		{
			this._listener.CheckDisposed();
			this._prefixes.Clear();
			bool isListening = this._listener.IsListening;
			if (isListening)
			{
				EndPointManager.RemoveListener(this._listener);
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000103F0 File Offset: 0x0000E5F0
		public bool Contains(string uriPrefix)
		{
			this._listener.CheckDisposed();
			bool flag = uriPrefix == null;
			if (flag)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			return this._prefixes.Contains(uriPrefix);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001042D File Offset: 0x0000E62D
		public void CopyTo(Array array, int offset)
		{
			this._listener.CheckDisposed();
			((ICollection)this._prefixes).CopyTo(array, offset);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0001044A File Offset: 0x0000E64A
		public void CopyTo(string[] array, int offset)
		{
			this._listener.CheckDisposed();
			this._prefixes.CopyTo(array, offset);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00010468 File Offset: 0x0000E668
		public IEnumerator<string> GetEnumerator()
		{
			return this._prefixes.GetEnumerator();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0001048C File Offset: 0x0000E68C
		public bool Remove(string uriPrefix)
		{
			this._listener.CheckDisposed();
			bool flag = uriPrefix == null;
			if (flag)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			bool flag2 = this._prefixes.Remove(uriPrefix);
			bool flag3 = flag2 && this._listener.IsListening;
			if (flag3)
			{
				EndPointManager.RemovePrefix(uriPrefix, this._listener);
			}
			return flag2;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000104F0 File Offset: 0x0000E6F0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._prefixes.GetEnumerator();
		}

		// Token: 0x040000F9 RID: 249
		private HttpListener _listener;

		// Token: 0x040000FA RID: 250
		private List<string> _prefixes;
	}
}
