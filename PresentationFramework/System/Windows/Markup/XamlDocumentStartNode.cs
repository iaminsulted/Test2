using System;

namespace System.Windows.Markup
{
	// Token: 0x020004E3 RID: 1251
	internal class XamlDocumentStartNode : XamlNode
	{
		// Token: 0x06003F91 RID: 16273 RVA: 0x00212238 File Offset: 0x00211238
		internal XamlDocumentStartNode(int lineNumber, int linePosition, int depth) : base(XamlNodeType.DocumentStart, lineNumber, linePosition, depth)
		{
		}
	}
}
