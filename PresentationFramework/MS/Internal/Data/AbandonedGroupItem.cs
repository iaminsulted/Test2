using System;

namespace MS.Internal.Data
{
	// Token: 0x0200020D RID: 525
	internal class AbandonedGroupItem
	{
		// Token: 0x0600138B RID: 5003 RVA: 0x0014E43A File Offset: 0x0014D43A
		public AbandonedGroupItem(LiveShapingItem lsi, CollectionViewGroupInternal group)
		{
			this._lsi = lsi;
			this._group = group;
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x0014E450 File Offset: 0x0014D450
		public LiveShapingItem Item
		{
			get
			{
				return this._lsi;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x0014E458 File Offset: 0x0014D458
		public CollectionViewGroupInternal Group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x04000B89 RID: 2953
		private LiveShapingItem _lsi;

		// Token: 0x04000B8A RID: 2954
		private CollectionViewGroupInternal _group;
	}
}
