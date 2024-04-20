using System;

namespace System.Windows.Documents
{
	// Token: 0x020006E2 RID: 1762
	internal enum XamlTokenType
	{
		// Token: 0x04003116 RID: 12566
		XTokInvalid,
		// Token: 0x04003117 RID: 12567
		XTokEOF,
		// Token: 0x04003118 RID: 12568
		XTokCharacters,
		// Token: 0x04003119 RID: 12569
		XTokEntity,
		// Token: 0x0400311A RID: 12570
		XTokStartElement,
		// Token: 0x0400311B RID: 12571
		XTokEndElement,
		// Token: 0x0400311C RID: 12572
		XTokCData,
		// Token: 0x0400311D RID: 12573
		XTokPI,
		// Token: 0x0400311E RID: 12574
		XTokComment,
		// Token: 0x0400311F RID: 12575
		XTokWS
	}
}
