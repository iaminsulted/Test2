using System;

namespace MS.Internal.Documents
{
	// Token: 0x020001EB RID: 491
	internal class RowLayoutCompletedEventArgs : EventArgs
	{
		// Token: 0x06001169 RID: 4457 RVA: 0x00144069 File Offset: 0x00143069
		public RowLayoutCompletedEventArgs(int pivotRowIndex)
		{
			this._pivotRowIndex = pivotRowIndex;
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x00144078 File Offset: 0x00143078
		public int PivotRowIndex
		{
			get
			{
				return this._pivotRowIndex;
			}
		}

		// Token: 0x04000B0E RID: 2830
		private readonly int _pivotRowIndex;
	}
}
