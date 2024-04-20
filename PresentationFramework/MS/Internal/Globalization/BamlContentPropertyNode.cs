using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001B0 RID: 432
	internal sealed class BamlContentPropertyNode : BamlTreeNode
	{
		// Token: 0x06000E19 RID: 3609 RVA: 0x00137330 File Offset: 0x00136330
		internal BamlContentPropertyNode(string assemblyName, string typeFullName, string propertyName) : base(BamlNodeType.ContentProperty)
		{
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._propertyName = propertyName;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0013734E File Offset: 0x0013634E
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteContentProperty(this._assemblyName, this._typeFullName, this._propertyName);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00137368 File Offset: 0x00136368
		internal override BamlTreeNode Copy()
		{
			return new BamlContentPropertyNode(this._assemblyName, this._typeFullName, this._propertyName);
		}

		// Token: 0x04000A2C RID: 2604
		private string _assemblyName;

		// Token: 0x04000A2D RID: 2605
		private string _typeFullName;

		// Token: 0x04000A2E RID: 2606
		private string _propertyName;
	}
}
