using System;
using System.Net;

namespace System.Windows.Navigation
{
	// Token: 0x020005B8 RID: 1464
	public class NavigationFailedEventArgs : EventArgs
	{
		// Token: 0x060046AF RID: 18095 RVA: 0x00226AE6 File Offset: 0x00225AE6
		internal NavigationFailedEventArgs(Uri uri, object extraData, object navigator, WebRequest request, WebResponse response, Exception e)
		{
			this._uri = uri;
			this._extraData = extraData;
			this._navigator = navigator;
			this._request = request;
			this._response = response;
			this._exception = e;
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x060046B0 RID: 18096 RVA: 0x00226B1B File Offset: 0x00225B1B
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x060046B1 RID: 18097 RVA: 0x00226B23 File Offset: 0x00225B23
		public object ExtraData
		{
			get
			{
				return this._extraData;
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x00226B2B File Offset: 0x00225B2B
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x060046B3 RID: 18099 RVA: 0x00226B33 File Offset: 0x00225B33
		public WebRequest WebRequest
		{
			get
			{
				return this._request;
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x060046B4 RID: 18100 RVA: 0x00226B3B File Offset: 0x00225B3B
		public WebResponse WebResponse
		{
			get
			{
				return this._response;
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x060046B5 RID: 18101 RVA: 0x00226B43 File Offset: 0x00225B43
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x060046B6 RID: 18102 RVA: 0x00226B4B File Offset: 0x00225B4B
		// (set) Token: 0x060046B7 RID: 18103 RVA: 0x00226B53 File Offset: 0x00225B53
		public bool Handled
		{
			get
			{
				return this._handled;
			}
			set
			{
				this._handled = value;
			}
		}

		// Token: 0x0400258C RID: 9612
		private Uri _uri;

		// Token: 0x0400258D RID: 9613
		private object _extraData;

		// Token: 0x0400258E RID: 9614
		private object _navigator;

		// Token: 0x0400258F RID: 9615
		private WebRequest _request;

		// Token: 0x04002590 RID: 9616
		private WebResponse _response;

		// Token: 0x04002591 RID: 9617
		private Exception _exception;

		// Token: 0x04002592 RID: 9618
		private bool _handled;
	}
}
