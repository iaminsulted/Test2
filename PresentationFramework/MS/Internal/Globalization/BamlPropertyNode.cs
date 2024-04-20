using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A7 RID: 423
	internal sealed class BamlPropertyNode : BamlStartComplexPropertyNode
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x00137064 File Offset: 0x00136064
		internal BamlPropertyNode(string assemblyName, string ownerTypeFullName, string propertyName, string value, BamlAttributeUsage usage) : base(assemblyName, ownerTypeFullName, propertyName)
		{
			this._value = value;
			this._attributeUsage = usage;
			this._nodeType = BamlNodeType.Property;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00137088 File Offset: 0x00136088
		internal override void Serialize(BamlWriter writer)
		{
			if (!LocComments.IsLocCommentsProperty(this._ownerTypeFullName, this._propertyName) && !LocComments.IsLocLocalizabilityProperty(this._ownerTypeFullName, this._propertyName))
			{
				writer.WriteProperty(this._assemblyName, this._ownerTypeFullName, this._propertyName, this._value, this._attributeUsage);
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x001370DF File Offset: 0x001360DF
		internal override BamlTreeNode Copy()
		{
			return new BamlPropertyNode(this._assemblyName, this._ownerTypeFullName, this._propertyName, this._value, this._attributeUsage);
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x00137104 File Offset: 0x00136104
		// (set) Token: 0x06000DFA RID: 3578 RVA: 0x0013710C File Offset: 0x0013610C
		internal string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00137115 File Offset: 0x00136115
		// (set) Token: 0x06000DFC RID: 3580 RVA: 0x0013711D File Offset: 0x0013611D
		internal int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x04000A1A RID: 2586
		private string _value;

		// Token: 0x04000A1B RID: 2587
		private BamlAttributeUsage _attributeUsage;

		// Token: 0x04000A1C RID: 2588
		private int _index;
	}
}
