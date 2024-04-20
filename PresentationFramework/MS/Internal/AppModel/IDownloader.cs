using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200027A RID: 634
	internal interface IDownloader
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001834 RID: 6196
		NavigationService Downloader { get; }

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06001835 RID: 6197
		// (remove) Token: 0x06001836 RID: 6198
		event EventHandler ContentRendered;
	}
}
