using System;
using System.Windows.Interop;
using System.Windows.Threading;

namespace MS.Internal.AppModel
{
	// Token: 0x02000293 RID: 659
	internal class NativeProgressPageProxy : IProgressPage2, IProgressPage
	{
		// Token: 0x060018E8 RID: 6376 RVA: 0x001621DB File Offset: 0x001611DB
		internal NativeProgressPageProxy(INativeProgressPage npp)
		{
			this._npp = npp;
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x001621EA File Offset: 0x001611EA
		public void ShowProgressMessage(string message)
		{
			this._npp.ShowProgressMessage(message);
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x001056E1 File Offset: 0x001046E1
		// (set) Token: 0x060018EA RID: 6378 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public Uri DeploymentPath
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060018ED RID: 6381 RVA: 0x001056E1 File Offset: 0x001046E1
		// (set) Token: 0x060018EC RID: 6380 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public DispatcherOperationCallback StopCallback
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x060018EE RID: 6382 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public DispatcherOperationCallback RefreshCallback
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x001056E1 File Offset: 0x001046E1
		// (set) Token: 0x060018F0 RID: 6384 RVA: 0x001621F9 File Offset: 0x001611F9
		public string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				this._npp.SetApplicationName(value);
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x001056E1 File Offset: 0x001046E1
		// (set) Token: 0x060018F2 RID: 6386 RVA: 0x00162208 File Offset: 0x00161208
		public string PublisherName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				this._npp.SetPublisherName(value);
			}
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00162217 File Offset: 0x00161217
		public void UpdateProgress(long bytesDownloaded, long bytesTotal)
		{
			this._npp.OnDownloadProgress((ulong)bytesDownloaded, (ulong)bytesTotal);
		}

		// Token: 0x04000D71 RID: 3441
		private INativeProgressPage _npp;
	}
}
