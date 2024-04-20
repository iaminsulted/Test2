using System;

namespace System.Windows.Markup
{
	// Token: 0x020004F9 RID: 1273
	internal class XamlPropertyArrayStartNode : XamlPropertyComplexStartNode
	{
		// Token: 0x06003FEA RID: 16362 RVA: 0x00212868 File Offset: 0x00211868
		internal XamlPropertyArrayStartNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.PropertyArrayStart, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}
	}
}
