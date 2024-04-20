using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Globalization
{
	// Token: 0x02000309 RID: 777
	[WindowsRuntimeType]
	[Guid("7D7DAF45-368D-4364-852B-DEC927037B85")]
	internal interface ILanguageExtensionSubtags
	{
		// Token: 0x06001CFD RID: 7421
		IReadOnlyList<string> GetExtensionSubtags(string singleton);
	}
}
