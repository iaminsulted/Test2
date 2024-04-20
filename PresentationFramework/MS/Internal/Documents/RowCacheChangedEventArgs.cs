using System;
using System.Collections.Generic;

namespace MS.Internal.Documents
{
	// Token: 0x020001EC RID: 492
	internal class RowCacheChangedEventArgs : EventArgs
	{
		// Token: 0x0600116B RID: 4459 RVA: 0x00144080 File Offset: 0x00143080
		public RowCacheChangedEventArgs(List<RowCacheChange> changes)
		{
			this._changes = changes;
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x0014408F File Offset: 0x0014308F
		public List<RowCacheChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		// Token: 0x04000B0F RID: 2831
		private readonly List<RowCacheChange> _changes;
	}
}
