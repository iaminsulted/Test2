using System;

namespace System.Windows.Markup
{
	// Token: 0x020004F0 RID: 1264
	internal class XamlElementEndNode : XamlNode
	{
		// Token: 0x06003FD4 RID: 16340 RVA: 0x0021271C File Offset: 0x0021171C
		internal XamlElementEndNode(int lineNumber, int linePosition, int depth) : this(XamlNodeType.ElementEnd, lineNumber, linePosition, depth)
		{
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x002123A0 File Offset: 0x002113A0
		internal XamlElementEndNode(XamlNodeType tokenType, int lineNumber, int linePosition, int depth) : base(tokenType, lineNumber, linePosition, depth)
		{
		}
	}
}
