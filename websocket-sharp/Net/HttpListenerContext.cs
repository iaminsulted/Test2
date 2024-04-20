using System;
using System.Security.Principal;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Net
{
	// Token: 0x02000021 RID: 33
	public sealed class HttpListenerContext
	{
		// Token: 0x06000271 RID: 625 RVA: 0x0000FFF6 File Offset: 0x0000E1F6
		internal HttpListenerContext(HttpConnection connection)
		{
			this._connection = connection;
			this._errorStatus = 400;
			this._request = new HttpListenerRequest(this);
			this._response = new HttpListenerResponse(this);
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0001002C File Offset: 0x0000E22C
		internal HttpConnection Connection
		{
			get
			{
				return this._connection;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00010044 File Offset: 0x0000E244
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0001005C File Offset: 0x0000E25C
		internal string ErrorMessage
		{
			get
			{
				return this._error;
			}
			set
			{
				this._error = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00010068 File Offset: 0x0000E268
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00010080 File Offset: 0x0000E280
		internal int ErrorStatus
		{
			get
			{
				return this._errorStatus;
			}
			set
			{
				this._errorStatus = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0001008C File Offset: 0x0000E28C
		internal bool HasError
		{
			get
			{
				return this._error != null;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000278 RID: 632 RVA: 0x000100A8 File Offset: 0x0000E2A8
		// (set) Token: 0x06000279 RID: 633 RVA: 0x000100C0 File Offset: 0x0000E2C0
		internal HttpListener Listener
		{
			get
			{
				return this._listener;
			}
			set
			{
				this._listener = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600027A RID: 634 RVA: 0x000100CC File Offset: 0x0000E2CC
		public HttpListenerRequest Request
		{
			get
			{
				return this._request;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600027B RID: 635 RVA: 0x000100E4 File Offset: 0x0000E2E4
		public HttpListenerResponse Response
		{
			get
			{
				return this._response;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600027C RID: 636 RVA: 0x000100FC File Offset: 0x0000E2FC
		public IPrincipal User
		{
			get
			{
				return this._user;
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00010114 File Offset: 0x0000E314
		internal bool Authenticate()
		{
			AuthenticationSchemes authenticationSchemes = this._listener.SelectAuthenticationScheme(this._request);
			bool flag = authenticationSchemes == AuthenticationSchemes.Anonymous;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = authenticationSchemes == AuthenticationSchemes.None;
				if (flag2)
				{
					this._response.Close(HttpStatusCode.Forbidden);
					result = false;
				}
				else
				{
					string realm = this._listener.GetRealm();
					IPrincipal principal = HttpUtility.CreateUser(this._request.Headers["Authorization"], authenticationSchemes, realm, this._request.HttpMethod, this._listener.GetUserCredentialsFinder());
					bool flag3 = principal == null || !principal.Identity.IsAuthenticated;
					if (flag3)
					{
						this._response.CloseWithAuthChallenge(new AuthenticationChallenge(authenticationSchemes, realm).ToString());
						result = false;
					}
					else
					{
						this._user = principal;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000101F4 File Offset: 0x0000E3F4
		internal bool Register()
		{
			return this._listener.RegisterContext(this);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00010212 File Offset: 0x0000E412
		internal void Unregister()
		{
			this._listener.UnregisterContext(this);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00010224 File Offset: 0x0000E424
		public HttpListenerWebSocketContext AcceptWebSocket(string protocol)
		{
			bool flag = this._websocketContext != null;
			if (flag)
			{
				throw new InvalidOperationException("The accepting is already in progress.");
			}
			bool flag2 = protocol != null;
			if (flag2)
			{
				bool flag3 = protocol.Length == 0;
				if (flag3)
				{
					throw new ArgumentException("An empty string.", "protocol");
				}
				bool flag4 = !protocol.IsToken();
				if (flag4)
				{
					throw new ArgumentException("Contains an invalid character.", "protocol");
				}
			}
			this._websocketContext = new HttpListenerWebSocketContext(this, protocol);
			return this._websocketContext;
		}

		// Token: 0x040000F1 RID: 241
		private HttpConnection _connection;

		// Token: 0x040000F2 RID: 242
		private string _error;

		// Token: 0x040000F3 RID: 243
		private int _errorStatus;

		// Token: 0x040000F4 RID: 244
		private HttpListener _listener;

		// Token: 0x040000F5 RID: 245
		private HttpListenerRequest _request;

		// Token: 0x040000F6 RID: 246
		private HttpListenerResponse _response;

		// Token: 0x040000F7 RID: 247
		private IPrincipal _user;

		// Token: 0x040000F8 RID: 248
		private HttpListenerWebSocketContext _websocketContext;
	}
}
