using System;
using System.Net;

namespace System.Windows.Navigation
{
	// Token: 0x020005B7 RID: 1463
	public class NavigationEventArgs : EventArgs
	{
		// Token: 0x060046A8 RID: 18088 RVA: 0x00226A81 File Offset: 0x00225A81
		internal NavigationEventArgs(Uri uri, object content, object extraData, WebResponse response, object Navigator, bool isNavigationInitiator)
		{
			this._uri = uri;
			this._content = content;
			this._extraData = extraData;
			this._webResponse = response;
			this._isNavigationInitiator = isNavigationInitiator;
			this._navigator = Navigator;
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x060046A9 RID: 18089 RVA: 0x00226AB6 File Offset: 0x00225AB6
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x060046AA RID: 18090 RVA: 0x00226ABE File Offset: 0x00225ABE
		public object Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x060046AB RID: 18091 RVA: 0x00226AC6 File Offset: 0x00225AC6
		public bool IsNavigationInitiator
		{
			get
			{
				return this._isNavigationInitiator;
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x060046AC RID: 18092 RVA: 0x00226ACE File Offset: 0x00225ACE
		public object ExtraData
		{
			get
			{
				return this._extraData;
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x060046AD RID: 18093 RVA: 0x00226AD6 File Offset: 0x00225AD6
		public WebResponse WebResponse
		{
			get
			{
				return this._webResponse;
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x060046AE RID: 18094 RVA: 0x00226ADE File Offset: 0x00225ADE
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		// Token: 0x04002586 RID: 9606
		private Uri _uri;

		// Token: 0x04002587 RID: 9607
		private object _content;

		// Token: 0x04002588 RID: 9608
		private object _extraData;

		// Token: 0x04002589 RID: 9609
		private WebResponse _webResponse;

		// Token: 0x0400258A RID: 9610
		private bool _isNavigationInitiator;

		// Token: 0x0400258B RID: 9611
		private object _navigator;
	}
}
