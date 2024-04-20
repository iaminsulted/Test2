using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020000BB RID: 187
	[Guid("82BA7092-4C88-427D-A7BC-16DD93FEB67E")]
	internal interface IRestrictedErrorInfo
	{
		// Token: 0x06000314 RID: 788
		void GetErrorDetails(out string description, out int error, out string restrictedDescription, out string capabilitySid);

		// Token: 0x06000315 RID: 789
		string GetReference();
	}
}
