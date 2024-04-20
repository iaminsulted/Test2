using System;
using System.Net;
using System.Windows.Threading;

namespace System.Windows.Navigation
{
	// Token: 0x020005C5 RID: 1477
	internal class RequestState
	{
		// Token: 0x06004766 RID: 18278 RVA: 0x00229E56 File Offset: 0x00228E56
		internal RequestState(WebRequest request, Uri source, object navState, Dispatcher callbackDispatcher)
		{
			this._request = request;
			this._source = source;
			this._navState = navState;
			this._callbackDispatcher = callbackDispatcher;
		}

		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06004767 RID: 18279 RVA: 0x00229E7B File Offset: 0x00228E7B
		internal WebRequest Request
		{
			get
			{
				return this._request;
			}
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06004768 RID: 18280 RVA: 0x00229E83 File Offset: 0x00228E83
		internal Uri Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06004769 RID: 18281 RVA: 0x00229E8B File Offset: 0x00228E8B
		internal object NavState
		{
			get
			{
				return this._navState;
			}
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x0600476A RID: 18282 RVA: 0x00229E93 File Offset: 0x00228E93
		internal Dispatcher CallbackDispatcher
		{
			get
			{
				return this._callbackDispatcher;
			}
		}

		// Token: 0x040025C7 RID: 9671
		private WebRequest _request;

		// Token: 0x040025C8 RID: 9672
		private Uri _source;

		// Token: 0x040025C9 RID: 9673
		private object _navState;

		// Token: 0x040025CA RID: 9674
		private Dispatcher _callbackDispatcher;
	}
}
