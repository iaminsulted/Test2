using System;
using System.Collections.Generic;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x0200031B RID: 795
	// (Invoke) Token: 0x06001D72 RID: 7538
	[WindowsRuntimeType]
	internal delegate void WordSegmentsTokenizingHandler(IEnumerable<WordSegment> precedingWords, IEnumerable<WordSegment> words);
}
