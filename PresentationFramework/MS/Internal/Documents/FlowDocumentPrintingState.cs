using System;
using System.Windows;
using System.Windows.Xps;

namespace MS.Internal.Documents
{
	// Token: 0x020001C2 RID: 450
	internal class FlowDocumentPrintingState
	{
		// Token: 0x04000A96 RID: 2710
		internal XpsDocumentWriter XpsDocumentWriter;

		// Token: 0x04000A97 RID: 2711
		internal Size PageSize;

		// Token: 0x04000A98 RID: 2712
		internal Thickness PagePadding;

		// Token: 0x04000A99 RID: 2713
		internal double ColumnWidth;

		// Token: 0x04000A9A RID: 2714
		internal bool IsSelectionEnabled;
	}
}
