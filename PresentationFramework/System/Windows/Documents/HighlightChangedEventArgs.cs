using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x02000621 RID: 1569
	internal abstract class HighlightChangedEventArgs
	{
		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06004D64 RID: 19812
		internal abstract IList Ranges { get; }

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06004D65 RID: 19813
		internal abstract Type OwnerType { get; }
	}
}
