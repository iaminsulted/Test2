using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020000EA RID: 234
	internal abstract class GlyphRunInfo
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600042B RID: 1067
		internal abstract Point StartPosition { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600042C RID: 1068
		internal abstract Point EndPosition { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600042D RID: 1069
		internal abstract double WidthEmFontSize { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600042E RID: 1070
		internal abstract double HeightEmFontSize { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600042F RID: 1071
		internal abstract bool GlyphsHaveSidewaysOrientation { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000430 RID: 1072
		internal abstract int BidiLevel { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000431 RID: 1073
		internal abstract uint LanguageID { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000432 RID: 1074
		internal abstract string UnicodeString { get; }
	}
}
