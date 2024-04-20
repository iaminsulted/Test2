using System;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000140 RID: 320
	internal sealed class ParagraphBreakRun : TextEndOfParagraph
	{
		// Token: 0x060009AF RID: 2479 RVA: 0x0011EF7F File Offset: 0x0011DF7F
		internal ParagraphBreakRun(int length, PTS.FSFLRES breakReason) : base(length)
		{
			this.BreakReason = breakReason;
		}

		// Token: 0x040007F1 RID: 2033
		internal readonly PTS.FSFLRES BreakReason;
	}
}
