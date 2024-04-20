using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x02000314 RID: 788
	[WindowsRuntimeType]
	[Guid("86B4D4D1-B2FE-4E34-A81D-66640300454F")]
	internal interface IWordsSegmenter
	{
		// Token: 0x06001D44 RID: 7492
		WordSegment GetTokenAt(string text, uint startIndex);

		// Token: 0x06001D45 RID: 7493
		IReadOnlyList<WordSegment> GetTokens(string text);

		// Token: 0x06001D46 RID: 7494
		void Tokenize(string text, uint startIndex, WordSegmentsTokenizingHandler handler);

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001D47 RID: 7495
		string ResolvedLanguage { get; }
	}
}
