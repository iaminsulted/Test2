using System;

namespace System.Windows.Documents
{
	// Token: 0x020006B2 RID: 1714
	[Flags]
	internal enum FindFlags
	{
		// Token: 0x04002FBA RID: 12218
		None = 0,
		// Token: 0x04002FBB RID: 12219
		MatchCase = 1,
		// Token: 0x04002FBC RID: 12220
		FindInReverse = 2,
		// Token: 0x04002FBD RID: 12221
		FindWholeWordsOnly = 4,
		// Token: 0x04002FBE RID: 12222
		MatchDiacritics = 8,
		// Token: 0x04002FBF RID: 12223
		MatchKashida = 16,
		// Token: 0x04002FC0 RID: 12224
		MatchAlefHamza = 32
	}
}
