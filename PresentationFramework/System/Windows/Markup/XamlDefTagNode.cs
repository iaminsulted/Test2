using System;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x02000500 RID: 1280
	internal class XamlDefTagNode : XamlAttributeNode
	{
		// Token: 0x06003FF2 RID: 16370 RVA: 0x0021290C File Offset: 0x0021190C
		internal XamlDefTagNode(int lineNumber, int linePosition, int depth, bool isEmptyElement, XmlReader xmlReader, string defTagName) : base(XamlNodeType.DefTag, lineNumber, linePosition, depth, defTagName)
		{
		}
	}
}
