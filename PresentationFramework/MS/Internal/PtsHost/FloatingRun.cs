using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200013F RID: 319
	internal sealed class FloatingRun : TextHidden
	{
		// Token: 0x060009AD RID: 2477 RVA: 0x0011EF67 File Offset: 0x0011DF67
		internal FloatingRun(int length, bool figure) : base(length)
		{
			this._figure = figure;
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0011EF77 File Offset: 0x0011DF77
		internal bool Figure
		{
			get
			{
				return this._figure;
			}
		}

		// Token: 0x040007F0 RID: 2032
		private readonly bool _figure;
	}
}
