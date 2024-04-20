using System;

namespace System.Windows.Markup
{
	// Token: 0x020004E7 RID: 1255
	internal class XamlPropertyComplexStartNode : XamlPropertyBaseNode
	{
		// Token: 0x06003F9E RID: 16286 RVA: 0x00212354 File Offset: 0x00211354
		internal XamlPropertyComplexStartNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.PropertyComplexStart, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x00212374 File Offset: 0x00211374
		internal XamlPropertyComplexStartNode(XamlNodeType token, int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(token, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}
	}
}
