using System;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x020004F8 RID: 1272
	internal class XamlClrEventNode : XamlAttributeNode
	{
		// Token: 0x06003FE9 RID: 16361 RVA: 0x00212857 File Offset: 0x00211857
		internal XamlClrEventNode(int lineNumber, int linePosition, int depth, string eventName, MemberInfo eventMember, string value) : base(XamlNodeType.ClrEvent, lineNumber, linePosition, depth, value)
		{
		}
	}
}
