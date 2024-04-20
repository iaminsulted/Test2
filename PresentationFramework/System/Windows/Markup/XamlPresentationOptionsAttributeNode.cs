using System;

namespace System.Windows.Markup
{
	// Token: 0x02000503 RID: 1283
	internal class XamlPresentationOptionsAttributeNode : XamlAttributeNode
	{
		// Token: 0x06003FFA RID: 16378 RVA: 0x00212997 File Offset: 0x00211997
		internal XamlPresentationOptionsAttributeNode(int lineNumber, int linePosition, int depth, string name, string value) : base(XamlNodeType.PresentationOptionsAttribute, lineNumber, linePosition, depth, value)
		{
			this._name = name;
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06003FFB RID: 16379 RVA: 0x002129AE File Offset: 0x002119AE
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x040023EC RID: 9196
		private string _name;
	}
}
