using System;

namespace System.Windows.Markup
{
	// Token: 0x020004F3 RID: 1267
	internal class XamlUnknownTagStartNode : XamlAttributeNode
	{
		// Token: 0x06003FDA RID: 16346 RVA: 0x00212762 File Offset: 0x00211762
		internal XamlUnknownTagStartNode(int lineNumber, int linePosition, int depth, string xmlNamespace, string value) : base(XamlNodeType.UnknownTagStart, lineNumber, linePosition, depth, value)
		{
			this._xmlNamespace = xmlNamespace;
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06003FDB RID: 16347 RVA: 0x00212779 File Offset: 0x00211779
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x040023DB RID: 9179
		private string _xmlNamespace;
	}
}
