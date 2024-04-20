using System;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Globalization
{
	// Token: 0x02000308 RID: 776
	[Guid("6A47E5B5-D94D-4886-A404-A5A5B9D5B494")]
	[WindowsRuntimeType]
	internal interface ILanguage2
	{
		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001CFC RID: 7420
		LanguageLayoutDirection LayoutDirection { get; }
	}
}
