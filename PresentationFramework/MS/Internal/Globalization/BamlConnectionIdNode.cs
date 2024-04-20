using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A1 RID: 417
	internal sealed class BamlConnectionIdNode : BamlTreeNode
	{
		// Token: 0x06000DCF RID: 3535 RVA: 0x00136D9F File Offset: 0x00135D9F
		internal BamlConnectionIdNode(int connectionId) : base(BamlNodeType.ConnectionId)
		{
			this._connectionId = connectionId;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00136DAF File Offset: 0x00135DAF
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteConnectionId(this._connectionId);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00136DBD File Offset: 0x00135DBD
		internal override BamlTreeNode Copy()
		{
			return new BamlConnectionIdNode(this._connectionId);
		}

		// Token: 0x04000A08 RID: 2568
		private int _connectionId;
	}
}
