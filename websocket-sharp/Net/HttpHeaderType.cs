using System;

namespace WebSocketSharp.Net
{
	// Token: 0x0200002D RID: 45
	[Flags]
	internal enum HttpHeaderType
	{
		// Token: 0x04000170 RID: 368
		Unspecified = 0,
		// Token: 0x04000171 RID: 369
		Request = 1,
		// Token: 0x04000172 RID: 370
		Response = 2,
		// Token: 0x04000173 RID: 371
		Restricted = 4,
		// Token: 0x04000174 RID: 372
		MultiValue = 8,
		// Token: 0x04000175 RID: 373
		MultiValueInRequest = 16,
		// Token: 0x04000176 RID: 374
		MultiValueInResponse = 32
	}
}
