using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020000B8 RID: 184
	[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
	internal interface IErrorInfo
	{
		// Token: 0x0600030D RID: 781
		Guid GetGuid();

		// Token: 0x0600030E RID: 782
		string GetSource();

		// Token: 0x0600030F RID: 783
		string GetDescription();

		// Token: 0x06000310 RID: 784
		string GetHelpFile();

		// Token: 0x06000311 RID: 785
		string GetHelpFileContent();
	}
}
