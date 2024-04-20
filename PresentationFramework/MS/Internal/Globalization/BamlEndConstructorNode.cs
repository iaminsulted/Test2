using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001AF RID: 431
	internal sealed class BamlEndConstructorNode : BamlTreeNode
	{
		// Token: 0x06000E16 RID: 3606 RVA: 0x00137317 File Offset: 0x00136317
		internal BamlEndConstructorNode() : base(BamlNodeType.EndConstructor)
		{
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00137321 File Offset: 0x00136321
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndConstructor();
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00137329 File Offset: 0x00136329
		internal override BamlTreeNode Copy()
		{
			return new BamlEndConstructorNode();
		}
	}
}
