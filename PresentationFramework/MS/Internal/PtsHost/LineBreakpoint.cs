using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000130 RID: 304
	internal sealed class LineBreakpoint : UnmanagedHandle
	{
		// Token: 0x06000857 RID: 2135 RVA: 0x00114A74 File Offset: 0x00113A74
		internal LineBreakpoint(OptimalBreakSession optimalBreakSession, TextBreakpoint textBreakpoint) : base(optimalBreakSession.PtsContext)
		{
			this._textBreakpoint = textBreakpoint;
			this._optimalBreakSession = optimalBreakSession;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00114A90 File Offset: 0x00113A90
		public override void Dispose()
		{
			if (this._textBreakpoint != null)
			{
				this._textBreakpoint.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00114AAB File Offset: 0x00113AAB
		internal OptimalBreakSession OptimalBreakSession
		{
			get
			{
				return this._optimalBreakSession;
			}
		}

		// Token: 0x040007B7 RID: 1975
		private TextBreakpoint _textBreakpoint;

		// Token: 0x040007B8 RID: 1976
		private OptimalBreakSession _optimalBreakSession;
	}
}
