using System;

namespace System.Windows.Markup
{
	// Token: 0x02000504 RID: 1284
	internal class XamlKeyElementStartNode : XamlElementStartNode
	{
		// Token: 0x06003FFC RID: 16380 RVA: 0x002129B8 File Offset: 0x002119B8
		internal XamlKeyElementStartNode(int lineNumber, int linePosition, int depth, string assemblyName, string typeFullName, Type elementType, Type serializerType) : base(XamlNodeType.KeyElementStart, lineNumber, linePosition, depth, assemblyName, typeFullName, elementType, serializerType, false, false, false)
		{
		}
	}
}
