using System;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000141 RID: 321
	internal sealed class LineBreakRun : TextEndOfLine
	{
		// Token: 0x060009B0 RID: 2480 RVA: 0x0011EF8F File Offset: 0x0011DF8F
		internal LineBreakRun(int length, PTS.FSFLRES breakReason) : base(length)
		{
			this.BreakReason = breakReason;
		}

		// Token: 0x040007F2 RID: 2034
		internal readonly PTS.FSFLRES BreakReason;
	}
}
