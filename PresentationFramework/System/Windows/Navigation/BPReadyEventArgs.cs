using System;
using System.ComponentModel;

namespace System.Windows.Navigation
{
	// Token: 0x020005C6 RID: 1478
	internal class BPReadyEventArgs : CancelEventArgs
	{
		// Token: 0x0600476B RID: 18283 RVA: 0x00229E9B File Offset: 0x00228E9B
		internal BPReadyEventArgs(object content, Uri uri)
		{
			this._content = content;
			this._uri = uri;
		}

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x0600476C RID: 18284 RVA: 0x00229EB1 File Offset: 0x00228EB1
		internal object Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x0600476D RID: 18285 RVA: 0x00229EB9 File Offset: 0x00228EB9
		internal Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x040025CB RID: 9675
		private object _content;

		// Token: 0x040025CC RID: 9676
		private Uri _uri;
	}
}
