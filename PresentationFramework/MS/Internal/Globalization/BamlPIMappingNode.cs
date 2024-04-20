using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001AD RID: 429
	internal sealed class BamlPIMappingNode : BamlTreeNode
	{
		// Token: 0x06000E10 RID: 3600 RVA: 0x001372AC File Offset: 0x001362AC
		internal BamlPIMappingNode(string xmlNamespace, string clrNamespace, string assemblyName) : base(BamlNodeType.PIMapping)
		{
			this._xmlNamespace = xmlNamespace;
			this._clrNamespace = clrNamespace;
			this._assemblyName = assemblyName;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x001372CB File Offset: 0x001362CB
		internal override void Serialize(BamlWriter writer)
		{
			writer.WritePIMapping(this._xmlNamespace, this._clrNamespace, this._assemblyName);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x001372E5 File Offset: 0x001362E5
		internal override BamlTreeNode Copy()
		{
			return new BamlPIMappingNode(this._xmlNamespace, this._clrNamespace, this._assemblyName);
		}

		// Token: 0x04000A29 RID: 2601
		private string _xmlNamespace;

		// Token: 0x04000A2A RID: 2602
		private string _clrNamespace;

		// Token: 0x04000A2B RID: 2603
		private string _assemblyName;
	}
}
