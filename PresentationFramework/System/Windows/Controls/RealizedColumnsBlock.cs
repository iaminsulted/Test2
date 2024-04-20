using System;

namespace System.Windows.Controls
{
	// Token: 0x020007C7 RID: 1991
	internal struct RealizedColumnsBlock
	{
		// Token: 0x06007201 RID: 29185 RVA: 0x002DC992 File Offset: 0x002DB992
		public RealizedColumnsBlock(int startIndex, int endIndex, int startIndexOffset)
		{
			this = default(RealizedColumnsBlock);
			this.StartIndex = startIndex;
			this.EndIndex = endIndex;
			this.StartIndexOffset = startIndexOffset;
		}

		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x06007202 RID: 29186 RVA: 0x002DC9B0 File Offset: 0x002DB9B0
		// (set) Token: 0x06007203 RID: 29187 RVA: 0x002DC9B8 File Offset: 0x002DB9B8
		public int StartIndex { readonly get; private set; }

		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x06007204 RID: 29188 RVA: 0x002DC9C1 File Offset: 0x002DB9C1
		// (set) Token: 0x06007205 RID: 29189 RVA: 0x002DC9C9 File Offset: 0x002DB9C9
		public int EndIndex { readonly get; private set; }

		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x06007206 RID: 29190 RVA: 0x002DC9D2 File Offset: 0x002DB9D2
		// (set) Token: 0x06007207 RID: 29191 RVA: 0x002DC9DA File Offset: 0x002DB9DA
		public int StartIndexOffset { readonly get; private set; }
	}
}
