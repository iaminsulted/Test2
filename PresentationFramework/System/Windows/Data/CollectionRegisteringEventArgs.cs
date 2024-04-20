using System;
using System.Collections;

namespace System.Windows.Data
{
	// Token: 0x02000453 RID: 1107
	public class CollectionRegisteringEventArgs : EventArgs
	{
		// Token: 0x060037DC RID: 14300 RVA: 0x001E71DF File Offset: 0x001E61DF
		internal CollectionRegisteringEventArgs(IEnumerable collection, object parent = null)
		{
			this._collection = collection;
			this._parent = parent;
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x060037DD RID: 14301 RVA: 0x001E71F5 File Offset: 0x001E61F5
		public IEnumerable Collection
		{
			get
			{
				return this._collection;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x060037DE RID: 14302 RVA: 0x001E71FD File Offset: 0x001E61FD
		public object Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x04001CFE RID: 7422
		private IEnumerable _collection;

		// Token: 0x04001CFF RID: 7423
		private object _parent;
	}
}
