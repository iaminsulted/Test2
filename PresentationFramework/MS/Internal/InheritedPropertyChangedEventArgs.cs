using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020000F6 RID: 246
	internal class InheritedPropertyChangedEventArgs : EventArgs
	{
		// Token: 0x060005B8 RID: 1464 RVA: 0x00104386 File Offset: 0x00103386
		internal InheritedPropertyChangedEventArgs(ref InheritablePropertyChangeInfo info)
		{
			this._info = info;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0010439A File Offset: 0x0010339A
		internal InheritablePropertyChangeInfo Info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x040006D9 RID: 1753
		private InheritablePropertyChangeInfo _info;
	}
}
