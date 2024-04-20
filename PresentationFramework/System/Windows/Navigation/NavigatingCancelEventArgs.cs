using System;
using System.ComponentModel;
using System.Net;

namespace System.Windows.Navigation
{
	// Token: 0x020005B6 RID: 1462
	public class NavigatingCancelEventArgs : CancelEventArgs
	{
		// Token: 0x0600469D RID: 18077 RVA: 0x002269E0 File Offset: 0x002259E0
		internal NavigatingCancelEventArgs(Uri uri, object content, CustomContentState customContentState, object extraData, NavigationMode navigationMode, WebRequest request, object Navigator, bool isNavInitiator)
		{
			this._uri = uri;
			this._content = content;
			this._targetContentState = customContentState;
			this._navigationMode = navigationMode;
			this._extraData = extraData;
			this._webRequest = request;
			this._isNavInitiator = isNavInitiator;
			this._navigator = Navigator;
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x0600469E RID: 18078 RVA: 0x00226A30 File Offset: 0x00225A30
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x0600469F RID: 18079 RVA: 0x00226A38 File Offset: 0x00225A38
		public object Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x060046A0 RID: 18080 RVA: 0x00226A40 File Offset: 0x00225A40
		public CustomContentState TargetContentState
		{
			get
			{
				return this._targetContentState;
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x060046A2 RID: 18082 RVA: 0x00226A51 File Offset: 0x00225A51
		// (set) Token: 0x060046A1 RID: 18081 RVA: 0x00226A48 File Offset: 0x00225A48
		public CustomContentState ContentStateToSave
		{
			get
			{
				return this._contentStateToSave;
			}
			set
			{
				this._contentStateToSave = value;
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x060046A3 RID: 18083 RVA: 0x00226A59 File Offset: 0x00225A59
		public object ExtraData
		{
			get
			{
				return this._extraData;
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x060046A4 RID: 18084 RVA: 0x00226A61 File Offset: 0x00225A61
		public NavigationMode NavigationMode
		{
			get
			{
				return this._navigationMode;
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x060046A5 RID: 18085 RVA: 0x00226A69 File Offset: 0x00225A69
		public WebRequest WebRequest
		{
			get
			{
				return this._webRequest;
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x060046A6 RID: 18086 RVA: 0x00226A71 File Offset: 0x00225A71
		public bool IsNavigationInitiator
		{
			get
			{
				return this._isNavInitiator;
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x060046A7 RID: 18087 RVA: 0x00226A79 File Offset: 0x00225A79
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		// Token: 0x0400257D RID: 9597
		private Uri _uri;

		// Token: 0x0400257E RID: 9598
		private object _content;

		// Token: 0x0400257F RID: 9599
		private CustomContentState _targetContentState;

		// Token: 0x04002580 RID: 9600
		private CustomContentState _contentStateToSave;

		// Token: 0x04002581 RID: 9601
		private object _extraData;

		// Token: 0x04002582 RID: 9602
		private NavigationMode _navigationMode;

		// Token: 0x04002583 RID: 9603
		private WebRequest _webRequest;

		// Token: 0x04002584 RID: 9604
		private bool _isNavInitiator;

		// Token: 0x04002585 RID: 9605
		private object _navigator;
	}
}
