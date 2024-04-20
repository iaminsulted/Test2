using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000126 RID: 294
	internal sealed class LineBreakRecord : UnmanagedHandle
	{
		// Token: 0x0600080B RID: 2059 RVA: 0x00113633 File Offset: 0x00112633
		internal LineBreakRecord(PtsContext ptsContext, TextLineBreak textLineBreak) : base(ptsContext)
		{
			this._textLineBreak = textLineBreak;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00113643 File Offset: 0x00112643
		public override void Dispose()
		{
			if (this._textLineBreak != null)
			{
				this._textLineBreak.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0011365E File Offset: 0x0011265E
		internal LineBreakRecord Clone()
		{
			return new LineBreakRecord(base.PtsContext, this._textLineBreak.Clone());
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00113676 File Offset: 0x00112676
		internal TextLineBreak TextLineBreak
		{
			get
			{
				return this._textLineBreak;
			}
		}

		// Token: 0x040007A1 RID: 1953
		private TextLineBreak _textLineBreak;
	}
}
