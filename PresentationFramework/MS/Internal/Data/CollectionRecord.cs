using System;

namespace MS.Internal.Data
{
	// Token: 0x02000248 RID: 584
	internal class CollectionRecord
	{
		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x0015ACD0 File Offset: 0x00159CD0
		// (set) Token: 0x06001681 RID: 5761 RVA: 0x0015ACE2 File Offset: 0x00159CE2
		public ViewTable ViewTable
		{
			get
			{
				return (ViewTable)this._wrViewTable.Target;
			}
			set
			{
				this._wrViewTable = new WeakReference(value);
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x0015ACF0 File Offset: 0x00159CF0
		public bool IsAlive
		{
			get
			{
				return this.SynchronizationInfo.IsAlive || this._wrViewTable.IsAlive;
			}
		}

		// Token: 0x04000C62 RID: 3170
		public SynchronizationInfo SynchronizationInfo;

		// Token: 0x04000C63 RID: 3171
		private WeakReference _wrViewTable = ViewManager.NullWeakRef;
	}
}
