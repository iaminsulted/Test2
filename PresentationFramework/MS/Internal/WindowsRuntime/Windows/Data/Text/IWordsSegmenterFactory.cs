using System;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x02000315 RID: 789
	[WindowsRuntimeType]
	[Guid("E6977274-FC35-455C-8BFB-6D7F4653CA97")]
	internal interface IWordsSegmenterFactory
	{
		// Token: 0x06001D48 RID: 7496
		WordsSegmenter CreateWithLanguage(string language);
	}
}
