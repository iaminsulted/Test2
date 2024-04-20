using System;

namespace System.Windows.Navigation
{
	// Token: 0x020005BA RID: 1466
	public class NavigationProgressEventArgs : EventArgs
	{
		// Token: 0x060046B8 RID: 18104 RVA: 0x00226B5C File Offset: 0x00225B5C
		internal NavigationProgressEventArgs(Uri uri, long bytesRead, long maxBytes, object Navigator)
		{
			this._uri = uri;
			this._bytesRead = bytesRead;
			this._maxBytes = maxBytes;
			this._navigator = Navigator;
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x060046B9 RID: 18105 RVA: 0x00226B81 File Offset: 0x00225B81
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x060046BA RID: 18106 RVA: 0x00226B89 File Offset: 0x00225B89
		public long BytesRead
		{
			get
			{
				return this._bytesRead;
			}
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x060046BB RID: 18107 RVA: 0x00226B91 File Offset: 0x00225B91
		public long MaxBytes
		{
			get
			{
				return this._maxBytes;
			}
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x060046BC RID: 18108 RVA: 0x00226B99 File Offset: 0x00225B99
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		// Token: 0x04002598 RID: 9624
		private Uri _uri;

		// Token: 0x04002599 RID: 9625
		private long _bytesRead;

		// Token: 0x0400259A RID: 9626
		private long _maxBytes;

		// Token: 0x0400259B RID: 9627
		private object _navigator;
	}
}
