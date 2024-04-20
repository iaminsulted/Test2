using System;

namespace System.Windows.Documents
{
	// Token: 0x02000623 RID: 1571
	internal abstract class HighlightLayer
	{
		// Token: 0x06004D6B RID: 19819
		internal abstract object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction);

		// Token: 0x06004D6C RID: 19820
		internal abstract bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction);

		// Token: 0x06004D6D RID: 19821
		internal abstract StaticTextPointer GetNextChangePosition(StaticTextPointer textPosition, LogicalDirection direction);

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06004D6E RID: 19822
		internal abstract Type OwnerType { get; }

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06004D6F RID: 19823
		// (remove) Token: 0x06004D70 RID: 19824
		internal abstract event HighlightChangedEventHandler Changed;
	}
}
