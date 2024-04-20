using System;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020001F8 RID: 504
	internal class DesiredSizeChangedEventArgs : EventArgs
	{
		// Token: 0x06001284 RID: 4740 RVA: 0x0014AC94 File Offset: 0x00149C94
		internal DesiredSizeChangedEventArgs(UIElement child)
		{
			this._child = child;
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001285 RID: 4741 RVA: 0x0014ACA3 File Offset: 0x00149CA3
		internal UIElement Child
		{
			get
			{
				return this._child;
			}
		}

		// Token: 0x04000B30 RID: 2864
		private readonly UIElement _child;
	}
}
