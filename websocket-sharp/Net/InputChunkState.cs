using System;

namespace WebSocketSharp.Net
{
	// Token: 0x02000036 RID: 54
	internal enum InputChunkState
	{
		// Token: 0x0400018F RID: 399
		None,
		// Token: 0x04000190 RID: 400
		Data,
		// Token: 0x04000191 RID: 401
		DataEnded,
		// Token: 0x04000192 RID: 402
		Trailer,
		// Token: 0x04000193 RID: 403
		End
	}
}
