using System;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000127 RID: 295
	internal sealed class LineVisual : DrawingVisual
	{
		// Token: 0x0600080F RID: 2063 RVA: 0x0011367E File Offset: 0x0011267E
		internal DrawingContext Open()
		{
			return base.RenderOpen();
		}

		// Token: 0x040007A2 RID: 1954
		internal double WidthIncludingTrailingWhitespace;
	}
}
