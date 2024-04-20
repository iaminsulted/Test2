using System;

namespace System.Windows.Markup
{
	// Token: 0x020004E4 RID: 1252
	internal class XamlDocumentEndNode : XamlNode
	{
		// Token: 0x06003F92 RID: 16274 RVA: 0x00212244 File Offset: 0x00211244
		internal XamlDocumentEndNode(int lineNumber, int linePosition, int depth) : base(XamlNodeType.DocumentEnd, lineNumber, linePosition, depth)
		{
		}
	}
}
