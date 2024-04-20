using System;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x02000264 RID: 612
	[Flags]
	internal enum XmlToken
	{
		// Token: 0x04000CB3 RID: 3251
		MetaData = 1,
		// Token: 0x04000CB4 RID: 3252
		Left = 4,
		// Token: 0x04000CB5 RID: 3253
		Top = 8,
		// Token: 0x04000CB6 RID: 3254
		XOffset = 16,
		// Token: 0x04000CB7 RID: 3255
		YOffset = 32,
		// Token: 0x04000CB8 RID: 3256
		Width = 128,
		// Token: 0x04000CB9 RID: 3257
		Height = 256,
		// Token: 0x04000CBA RID: 3258
		IsExpanded = 512,
		// Token: 0x04000CBB RID: 3259
		Author = 1024,
		// Token: 0x04000CBC RID: 3260
		Text = 8192,
		// Token: 0x04000CBD RID: 3261
		Ink = 32768,
		// Token: 0x04000CBE RID: 3262
		ZOrder = 131072
	}
}
