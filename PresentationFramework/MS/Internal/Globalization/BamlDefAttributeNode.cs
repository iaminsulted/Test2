using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001AC RID: 428
	internal sealed class BamlDefAttributeNode : BamlTreeNode
	{
		// Token: 0x06000E0D RID: 3597 RVA: 0x0013726D File Offset: 0x0013626D
		internal BamlDefAttributeNode(string name, string value) : base(BamlNodeType.DefAttribute)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00137285 File Offset: 0x00136285
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteDefAttribute(this._name, this._value);
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00137299 File Offset: 0x00136299
		internal override BamlTreeNode Copy()
		{
			return new BamlDefAttributeNode(this._name, this._value);
		}

		// Token: 0x04000A27 RID: 2599
		private string _name;

		// Token: 0x04000A28 RID: 2600
		private string _value;
	}
}
