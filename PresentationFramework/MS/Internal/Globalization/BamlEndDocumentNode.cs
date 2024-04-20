using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A0 RID: 416
	internal sealed class BamlEndDocumentNode : BamlTreeNode
	{
		// Token: 0x06000DCC RID: 3532 RVA: 0x00136D87 File Offset: 0x00135D87
		internal BamlEndDocumentNode() : base(BamlNodeType.EndDocument)
		{
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00136D90 File Offset: 0x00135D90
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndDocument();
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00136D98 File Offset: 0x00135D98
		internal override BamlTreeNode Copy()
		{
			return new BamlEndDocumentNode();
		}
	}
}
