using System;
using System.Windows.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000114 RID: 276
	internal struct DirtyTextRange
	{
		// Token: 0x0600071D RID: 1821 RVA: 0x0010C4C7 File Offset: 0x0010B4C7
		internal DirtyTextRange(int startIndex, int positionsAdded, int positionsRemoved, bool fromHighlightLayer = false)
		{
			this.StartIndex = startIndex;
			this.PositionsAdded = positionsAdded;
			this.PositionsRemoved = positionsRemoved;
			this.FromHighlightLayer = fromHighlightLayer;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0010C4E8 File Offset: 0x0010B4E8
		internal DirtyTextRange(TextContainerChangeEventArgs change)
		{
			this.StartIndex = change.ITextPosition.Offset;
			this.PositionsAdded = 0;
			this.PositionsRemoved = 0;
			this.FromHighlightLayer = false;
			switch (change.TextChange)
			{
			case TextChangeType.ContentAdded:
				this.PositionsAdded = change.Count;
				return;
			case TextChangeType.ContentRemoved:
				this.PositionsRemoved = change.Count;
				return;
			case TextChangeType.PropertyModified:
				this.PositionsAdded = change.Count;
				this.PositionsRemoved = change.Count;
				return;
			default:
				return;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0010C567 File Offset: 0x0010B567
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x0010C56F File Offset: 0x0010B56F
		internal int StartIndex { readonly get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0010C578 File Offset: 0x0010B578
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0010C580 File Offset: 0x0010B580
		internal int PositionsAdded { readonly get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0010C589 File Offset: 0x0010B589
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x0010C591 File Offset: 0x0010B591
		internal int PositionsRemoved { readonly get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0010C59A File Offset: 0x0010B59A
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0010C5A2 File Offset: 0x0010B5A2
		internal bool FromHighlightLayer { readonly get; set; }
	}
}
