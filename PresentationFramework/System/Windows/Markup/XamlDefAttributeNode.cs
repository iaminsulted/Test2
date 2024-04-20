using System;

namespace System.Windows.Markup
{
	// Token: 0x02000501 RID: 1281
	internal class XamlDefAttributeNode : XamlAttributeNode
	{
		// Token: 0x06003FF3 RID: 16371 RVA: 0x0021291B File Offset: 0x0021191B
		internal XamlDefAttributeNode(int lineNumber, int linePosition, int depth, string name, string value) : base(XamlNodeType.DefAttribute, lineNumber, linePosition, depth, value)
		{
			this._attributeUsage = BamlAttributeUsage.Default;
			this._name = name;
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x00212939 File Offset: 0x00211939
		internal XamlDefAttributeNode(int lineNumber, int linePosition, int depth, string name, string value, BamlAttributeUsage bamlAttributeUsage) : base(XamlNodeType.DefAttribute, lineNumber, linePosition, depth, value)
		{
			this._attributeUsage = bamlAttributeUsage;
			this._name = name;
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06003FF5 RID: 16373 RVA: 0x00212958 File Offset: 0x00211958
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06003FF6 RID: 16374 RVA: 0x00212960 File Offset: 0x00211960
		internal BamlAttributeUsage AttributeUsage
		{
			get
			{
				return this._attributeUsage;
			}
		}

		// Token: 0x040023E8 RID: 9192
		private BamlAttributeUsage _attributeUsage;

		// Token: 0x040023E9 RID: 9193
		private string _name;
	}
}
