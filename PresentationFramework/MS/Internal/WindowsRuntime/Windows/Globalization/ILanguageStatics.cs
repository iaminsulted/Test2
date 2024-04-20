using System;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Globalization
{
	// Token: 0x0200030B RID: 779
	[WindowsRuntimeType]
	[Guid("B23CD557-0865-46D4-89B8-D59BE8990F0D")]
	internal interface ILanguageStatics
	{
		// Token: 0x06001CFF RID: 7423
		bool IsWellFormed(string languageTag);

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001D00 RID: 7424
		string CurrentInputMethodLanguageTag { get; }
	}
}
