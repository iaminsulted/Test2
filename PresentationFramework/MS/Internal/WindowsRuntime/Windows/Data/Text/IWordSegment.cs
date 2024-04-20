using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x02000313 RID: 787
	[WindowsRuntimeType]
	[Guid("D2D4BA6D-987C-4CC0-B6BD-D49A11B38F9A")]
	internal interface IWordSegment
	{
		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001D41 RID: 7489
		IReadOnlyList<AlternateWordForm> AlternateForms { get; }

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001D42 RID: 7490
		TextSegment SourceTextSegment { get; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001D43 RID: 7491
		string Text { get; }
	}
}
