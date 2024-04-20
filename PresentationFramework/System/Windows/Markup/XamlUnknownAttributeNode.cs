using System;

namespace System.Windows.Markup
{
	// Token: 0x020004EC RID: 1260
	internal class XamlUnknownAttributeNode : XamlAttributeNode
	{
		// Token: 0x06003FC4 RID: 16324 RVA: 0x002125EC File Offset: 0x002115EC
		internal XamlUnknownAttributeNode(int lineNumber, int linePosition, int depth, string xmlNamespace, string name, string value, BamlAttributeUsage attributeUsage) : base(XamlNodeType.UnknownAttribute, lineNumber, linePosition, depth, value)
		{
			this._xmlNamespace = xmlNamespace;
			this._name = name;
			this._attributeUsage = attributeUsage;
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06003FC5 RID: 16325 RVA: 0x00212613 File Offset: 0x00211613
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06003FC6 RID: 16326 RVA: 0x0021261B File Offset: 0x0021161B
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x040023CE RID: 9166
		private string _xmlNamespace;

		// Token: 0x040023CF RID: 9167
		private string _name;

		// Token: 0x040023D0 RID: 9168
		private BamlAttributeUsage _attributeUsage;
	}
}
