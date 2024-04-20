using System;

namespace System.Windows.Documents
{
	// Token: 0x0200063C RID: 1596
	internal interface IXamlErrorHandler
	{
		// Token: 0x06004F0D RID: 20237
		void Error(string message, XamlToRtfError xamlToRtfError);

		// Token: 0x06004F0E RID: 20238
		void FatalError(string message, XamlToRtfError xamlToRtfError);

		// Token: 0x06004F0F RID: 20239
		void IgnorableWarning(string message, XamlToRtfError xamlToRtfError);
	}
}
