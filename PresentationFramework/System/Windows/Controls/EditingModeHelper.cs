using System;

namespace System.Windows.Controls
{
	// Token: 0x02000810 RID: 2064
	internal static class EditingModeHelper
	{
		// Token: 0x060078CA RID: 30922 RVA: 0x003017E9 File Offset: 0x003007E9
		internal static bool IsDefined(InkCanvasEditingMode InkCanvasEditingMode)
		{
			return InkCanvasEditingMode >= InkCanvasEditingMode.None && InkCanvasEditingMode <= InkCanvasEditingMode.EraseByStroke;
		}
	}
}
