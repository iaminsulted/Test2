using System;
using System.Windows.Threading;

namespace System.Windows.Interop
{
	// Token: 0x02000424 RID: 1060
	public interface IProgressPage
	{
		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x0600334A RID: 13130
		// (set) Token: 0x0600334B RID: 13131
		Uri DeploymentPath { get; set; }

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x0600334C RID: 13132
		// (set) Token: 0x0600334D RID: 13133
		DispatcherOperationCallback StopCallback { get; set; }

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x0600334E RID: 13134
		// (set) Token: 0x0600334F RID: 13135
		DispatcherOperationCallback RefreshCallback { get; set; }

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06003350 RID: 13136
		// (set) Token: 0x06003351 RID: 13137
		string ApplicationName { get; set; }

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06003352 RID: 13138
		// (set) Token: 0x06003353 RID: 13139
		string PublisherName { get; set; }

		// Token: 0x06003354 RID: 13140
		void UpdateProgress(long bytesDownloaded, long bytesTotal);
	}
}
