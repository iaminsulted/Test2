using System;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Globalization
{
	// Token: 0x0200030C RID: 780
	[WindowsRuntimeType]
	[Guid("30199F6E-914B-4B2A-9D6E-E3B0E27DBE4F")]
	internal interface ILanguageStatics2
	{
		// Token: 0x06001D01 RID: 7425
		bool TrySetInputMethodLanguageTag(string languageTag);
	}
}
