using System;

namespace System.Windows.Markup
{
	// Token: 0x020004E8 RID: 1256
	internal class XamlPropertyComplexEndNode : XamlNode
	{
		// Token: 0x06003FA0 RID: 16288 RVA: 0x00212394 File Offset: 0x00211394
		internal XamlPropertyComplexEndNode(int lineNumber, int linePosition, int depth) : base(XamlNodeType.PropertyComplexEnd, lineNumber, linePosition, depth)
		{
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x002123A0 File Offset: 0x002113A0
		internal XamlPropertyComplexEndNode(XamlNodeType token, int lineNumber, int linePosition, int depth) : base(token, lineNumber, linePosition, depth)
		{
		}
	}
}
