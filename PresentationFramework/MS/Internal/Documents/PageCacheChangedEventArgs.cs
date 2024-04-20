using System;
using System.Collections.Generic;

namespace MS.Internal.Documents
{
	// Token: 0x020001D9 RID: 473
	internal class PageCacheChangedEventArgs : EventArgs
	{
		// Token: 0x060010D5 RID: 4309 RVA: 0x00141FAE File Offset: 0x00140FAE
		public PageCacheChangedEventArgs(List<PageCacheChange> changes)
		{
			this._changes = changes;
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x00141FBD File Offset: 0x00140FBD
		public List<PageCacheChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		// Token: 0x04000AD8 RID: 2776
		private readonly List<PageCacheChange> _changes;
	}
}
