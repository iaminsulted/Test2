using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001B1 RID: 433
	internal sealed class BamlPresentationOptionsAttributeNode : BamlTreeNode
	{
		// Token: 0x06000E1C RID: 3612 RVA: 0x00137381 File Offset: 0x00136381
		internal BamlPresentationOptionsAttributeNode(string name, string value) : base(BamlNodeType.PresentationOptionsAttribute)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00137399 File Offset: 0x00136399
		internal override void Serialize(BamlWriter writer)
		{
			writer.WritePresentationOptionsAttribute(this._name, this._value);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x001373AD File Offset: 0x001363AD
		internal override BamlTreeNode Copy()
		{
			return new BamlPresentationOptionsAttributeNode(this._name, this._value);
		}

		// Token: 0x04000A2F RID: 2607
		private string _name;

		// Token: 0x04000A30 RID: 2608
		private string _value;
	}
}
