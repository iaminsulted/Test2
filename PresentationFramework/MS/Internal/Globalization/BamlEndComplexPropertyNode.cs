using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A6 RID: 422
	internal sealed class BamlEndComplexPropertyNode : BamlTreeNode
	{
		// Token: 0x06000DF3 RID: 3571 RVA: 0x0013704B File Offset: 0x0013604B
		internal BamlEndComplexPropertyNode() : base(BamlNodeType.EndComplexProperty)
		{
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00137055 File Offset: 0x00136055
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndComplexProperty();
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0013705D File Offset: 0x0013605D
		internal override BamlTreeNode Copy()
		{
			return new BamlEndComplexPropertyNode();
		}
	}
}
