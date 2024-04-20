using System;
using System.ComponentModel;

namespace System.Windows
{
	// Token: 0x02000397 RID: 919
	public class SessionEndingCancelEventArgs : CancelEventArgs
	{
		// Token: 0x0600253A RID: 9530 RVA: 0x00185D7F File Offset: 0x00184D7F
		internal SessionEndingCancelEventArgs(ReasonSessionEnding reasonSessionEnding)
		{
			this._reasonSessionEnding = reasonSessionEnding;
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x0600253B RID: 9531 RVA: 0x00185D8E File Offset: 0x00184D8E
		public ReasonSessionEnding ReasonSessionEnding
		{
			get
			{
				return this._reasonSessionEnding;
			}
		}

		// Token: 0x0400117C RID: 4476
		private ReasonSessionEnding _reasonSessionEnding;
	}
}
