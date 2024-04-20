using System;

namespace System.Windows.Markup
{
	// Token: 0x020004FB RID: 1275
	internal class XamlPropertyIDictionaryStartNode : XamlPropertyComplexStartNode
	{
		// Token: 0x06003FEC RID: 16364 RVA: 0x002128A8 File Offset: 0x002118A8
		internal XamlPropertyIDictionaryStartNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.PropertyIDictionaryStart, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}
	}
}
