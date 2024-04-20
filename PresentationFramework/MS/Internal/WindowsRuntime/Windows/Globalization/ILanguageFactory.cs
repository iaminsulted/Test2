using System;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Globalization
{
	// Token: 0x0200030A RID: 778
	[Guid("9B0252AC-0C27-44F8-B792-9793FB66C63E")]
	[WindowsRuntimeType]
	internal interface ILanguageFactory
	{
		// Token: 0x06001CFE RID: 7422
		Language CreateLanguage(string languageTag);
	}
}
