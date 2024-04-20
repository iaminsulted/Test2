using System;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000151 RID: 337
	internal sealed class UpdateRecord
	{
		// Token: 0x06000AED RID: 2797 RVA: 0x0012BD90 File Offset: 0x0012AD90
		internal UpdateRecord()
		{
			this.Dtr = new DirtyTextRange(0, 0, 0, false);
			this.FirstPara = (this.SyncPara = null);
			this.ChangeType = PTS.FSKCHANGE.fskchNone;
			this.Next = null;
			this.InProcessing = false;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0012BDD8 File Offset: 0x0012ADD8
		internal void MergeWithNext()
		{
			int num = this.Next.Dtr.StartIndex - this.Dtr.StartIndex;
			this.Dtr.PositionsAdded = this.Dtr.PositionsAdded + (num + this.Next.Dtr.PositionsAdded);
			this.Dtr.PositionsRemoved = this.Dtr.PositionsRemoved + (num + this.Next.Dtr.PositionsRemoved);
			this.SyncPara = this.Next.SyncPara;
			this.Next = this.Next.Next;
		}

		// Token: 0x0400082B RID: 2091
		internal DirtyTextRange Dtr;

		// Token: 0x0400082C RID: 2092
		internal BaseParagraph FirstPara;

		// Token: 0x0400082D RID: 2093
		internal BaseParagraph SyncPara;

		// Token: 0x0400082E RID: 2094
		internal PTS.FSKCHANGE ChangeType;

		// Token: 0x0400082F RID: 2095
		internal UpdateRecord Next;

		// Token: 0x04000830 RID: 2096
		internal bool InProcessing;
	}
}
