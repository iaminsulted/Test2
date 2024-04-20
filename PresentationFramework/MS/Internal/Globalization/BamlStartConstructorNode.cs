using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001AE RID: 430
	internal sealed class BamlStartConstructorNode : BamlTreeNode
	{
		// Token: 0x06000E13 RID: 3603 RVA: 0x001372FE File Offset: 0x001362FE
		internal BamlStartConstructorNode() : base(BamlNodeType.StartConstructor)
		{
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00137308 File Offset: 0x00136308
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartConstructor();
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00137310 File Offset: 0x00136310
		internal override BamlTreeNode Copy()
		{
			return new BamlStartConstructorNode();
		}
	}
}
