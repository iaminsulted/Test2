using System;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Globalization
{
	// Token: 0x02000307 RID: 775
	[Guid("EA79A752-F7C2-4265-B1BD-C4DEC4E4F080")]
	[WindowsRuntimeType]
	internal interface ILanguage
	{
		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001CF8 RID: 7416
		string DisplayName { get; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001CF9 RID: 7417
		string LanguageTag { get; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001CFA RID: 7418
		string NativeName { get; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001CFB RID: 7419
		string Script { get; }
	}
}
