using System;

namespace System.Windows.Markup
{
	// Token: 0x020004F4 RID: 1268
	internal class XamlUnknownTagEndNode : XamlNode
	{
		// Token: 0x06003FDC RID: 16348 RVA: 0x00212781 File Offset: 0x00211781
		internal XamlUnknownTagEndNode(int lineNumber, int linePosition, int depth, string localName, string xmlNamespace) : base(XamlNodeType.UnknownTagEnd, lineNumber, linePosition, depth)
		{
			this._localName = localName;
			this._xmlNamespace = xmlNamespace;
		}

		// Token: 0x040023DC RID: 9180
		private string _localName;

		// Token: 0x040023DD RID: 9181
		private string _xmlNamespace;
	}
}
