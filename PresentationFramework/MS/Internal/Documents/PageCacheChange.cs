using System;

namespace MS.Internal.Documents
{
	// Token: 0x020001DA RID: 474
	internal class PageCacheChange
	{
		// Token: 0x060010D7 RID: 4311 RVA: 0x00141FC5 File Offset: 0x00140FC5
		public PageCacheChange(int start, int count, PageCacheChangeType type)
		{
			this._start = start;
			this._count = count;
			this._type = type;
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x00141FE2 File Offset: 0x00140FE2
		public int Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x00141FEA File Offset: 0x00140FEA
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x00141FF2 File Offset: 0x00140FF2
		public PageCacheChangeType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x04000AD9 RID: 2777
		private readonly int _start;

		// Token: 0x04000ADA RID: 2778
		private readonly int _count;

		// Token: 0x04000ADB RID: 2779
		private readonly PageCacheChangeType _type;
	}
}
