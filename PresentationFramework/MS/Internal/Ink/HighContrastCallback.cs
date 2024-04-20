using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace MS.Internal.Ink
{
	// Token: 0x02000186 RID: 390
	internal abstract class HighContrastCallback
	{
		// Token: 0x06000CE2 RID: 3298
		internal abstract void TurnHighContrastOn(Color highContrastColor);

		// Token: 0x06000CE3 RID: 3299
		internal abstract void TurnHighContrastOff();

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000CE4 RID: 3300
		internal abstract Dispatcher Dispatcher { get; }
	}
}
