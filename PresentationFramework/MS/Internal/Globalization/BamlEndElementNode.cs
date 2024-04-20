using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A3 RID: 419
	internal sealed class BamlEndElementNode : BamlTreeNode
	{
		// Token: 0x06000DE1 RID: 3553 RVA: 0x00136F58 File Offset: 0x00135F58
		internal BamlEndElementNode() : base(BamlNodeType.EndElement)
		{
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00136F61 File Offset: 0x00135F61
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndElement();
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00136F69 File Offset: 0x00135F69
		internal override BamlTreeNode Copy()
		{
			return new BamlEndElementNode();
		}
	}
}
