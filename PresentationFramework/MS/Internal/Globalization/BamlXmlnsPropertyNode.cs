using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A4 RID: 420
	internal sealed class BamlXmlnsPropertyNode : BamlTreeNode
	{
		// Token: 0x06000DE4 RID: 3556 RVA: 0x00136F70 File Offset: 0x00135F70
		internal BamlXmlnsPropertyNode(string prefix, string xmlns) : base(BamlNodeType.XmlnsProperty)
		{
			this._prefix = prefix;
			this._xmlns = xmlns;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00136F87 File Offset: 0x00135F87
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteXmlnsProperty(this._prefix, this._xmlns);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00136F9B File Offset: 0x00135F9B
		internal override BamlTreeNode Copy()
		{
			return new BamlXmlnsPropertyNode(this._prefix, this._xmlns);
		}

		// Token: 0x04000A12 RID: 2578
		private string _prefix;

		// Token: 0x04000A13 RID: 2579
		private string _xmlns;
	}
}
