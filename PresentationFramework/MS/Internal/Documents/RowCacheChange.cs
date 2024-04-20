using System;

namespace MS.Internal.Documents
{
	// Token: 0x020001ED RID: 493
	internal class RowCacheChange
	{
		// Token: 0x0600116D RID: 4461 RVA: 0x00144097 File Offset: 0x00143097
		public RowCacheChange(int start, int count)
		{
			this._start = start;
			this._count = count;
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x001440AD File Offset: 0x001430AD
		public int Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x0600116F RID: 4463 RVA: 0x001440B5 File Offset: 0x001430B5
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x04000B10 RID: 2832
		private readonly int _start;

		// Token: 0x04000B11 RID: 2833
		private readonly int _count;
	}
}
