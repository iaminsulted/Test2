using System;

namespace Steamworks
{
	// Token: 0x020001B2 RID: 434
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
	internal class CallbackIdentityAttribute : Attribute
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x0002A07F File Offset: 0x0002827F
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x0002A087 File Offset: 0x00028287
		public int Identity { get; set; }

		// Token: 0x06000D33 RID: 3379 RVA: 0x0002A090 File Offset: 0x00028290
		public CallbackIdentityAttribute(int callbackNum)
		{
			this.Identity = callbackNum;
		}
	}
}
