using System;

namespace System.Windows.Markup
{
	// Token: 0x020004CD RID: 1229
	internal abstract class ParserHooks
	{
		// Token: 0x06003F03 RID: 16131 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual ParserAction LoadNode(XamlNode tokenNode)
		{
			return ParserAction.Normal;
		}
	}
}
