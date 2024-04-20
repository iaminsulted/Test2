using System;

namespace System.Windows.Documents
{
	// Token: 0x020005FF RID: 1535
	internal struct FixedPosition
	{
		// Token: 0x06004AFC RID: 19196 RVA: 0x00235544 File Offset: 0x00234544
		internal FixedPosition(FixedNode fixedNode, int offset)
		{
			this._fixedNode = fixedNode;
			this._offset = offset;
		}

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06004AFD RID: 19197 RVA: 0x00235554 File Offset: 0x00234554
		internal int Page
		{
			get
			{
				return this._fixedNode.Page;
			}
		}

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x06004AFE RID: 19198 RVA: 0x0023556F File Offset: 0x0023456F
		internal FixedNode Node
		{
			get
			{
				return this._fixedNode;
			}
		}

		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x06004AFF RID: 19199 RVA: 0x00235577 File Offset: 0x00234577
		internal int Offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x0400274B RID: 10059
		private readonly FixedNode _fixedNode;

		// Token: 0x0400274C RID: 10060
		private readonly int _offset;
	}
}
