using System;

namespace System.Windows.Controls
{
	// Token: 0x0200075E RID: 1886
	[Flags]
	internal enum DataGridNotificationTarget
	{
		// Token: 0x040033D3 RID: 13267
		None = 0,
		// Token: 0x040033D4 RID: 13268
		Cells = 1,
		// Token: 0x040033D5 RID: 13269
		CellsPresenter = 2,
		// Token: 0x040033D6 RID: 13270
		Columns = 4,
		// Token: 0x040033D7 RID: 13271
		ColumnCollection = 8,
		// Token: 0x040033D8 RID: 13272
		ColumnHeaders = 16,
		// Token: 0x040033D9 RID: 13273
		ColumnHeadersPresenter = 32,
		// Token: 0x040033DA RID: 13274
		DataGrid = 64,
		// Token: 0x040033DB RID: 13275
		DetailsPresenter = 128,
		// Token: 0x040033DC RID: 13276
		RefreshCellContent = 256,
		// Token: 0x040033DD RID: 13277
		RowHeaders = 512,
		// Token: 0x040033DE RID: 13278
		Rows = 1024,
		// Token: 0x040033DF RID: 13279
		All = 4095
	}
}
