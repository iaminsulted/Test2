using System;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200013D RID: 317
	internal sealed class RowVisual : ContainerVisual
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x0011EE00 File Offset: 0x0011DE00
		internal RowVisual(TableRow row)
		{
			this._row = row;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0011EE0F File Offset: 0x0011DE0F
		internal TableRow Row
		{
			get
			{
				return this._row;
			}
		}

		// Token: 0x040007EB RID: 2027
		private readonly TableRow _row;
	}
}
