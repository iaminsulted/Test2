using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;

namespace WebSocketSharp.Net.WebSockets
{
	// Token: 0x02000043 RID: 67
	internal class TcpListenerWebSocketContext : WebSocketContext
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x000189BC File Offset: 0x00016BBC
		internal TcpListenerWebSocketContext(TcpClient tcpClient, string protocol, bool secure, ServerSslConfiguration sslConfig, Logger log)
		{
			this._tcpClient = tcpClient;
			this._secure = secure;
			this._log = log;
			NetworkStream stream = tcpClient.GetStream();
			if (secure)
			{
				SslStream sslStream = new SslStream(stream, false, sslConfig.ClientCertificateValidationCallback);
				sslStream.AuthenticateAsServer(sslConfig.ServerCertificate, sslConfig.ClientCertificateRequired, sslConfig.EnabledSslProtocols, sslConfig.CheckCertificateRevocation);
				this._stream = sslStream;
			}
			else
			{
				this._stream = stream;
			}
			Socket client = tcpClient.Client;
			this._serverEndPoint = client.LocalEndPoint;
			this._userEndPoint = client.RemoteEndPoint;
			this._request = HttpRequest.Read(this._stream, 90000);
			this._websocket = new WebSocket(this, protocol);
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00018A7C File Offset: 0x00016C7C
		internal Logger Log
		{
			get
			{
				return this._log;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00018A94 File Offset: 0x00016C94
		internal Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00018AAC File Offset: 0x00016CAC
		public override CookieCollection CookieCollection
		{
			get
			{
				return this._request.Cookies;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00018ACC File Offset: 0x00016CCC
		public override NameValueCollection Headers
		{
			get
			{
				return this._request.Headers;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00018AEC File Offset: 0x00016CEC
		public override string Host
		{
			get
			{
				return this._request.Headers["Host"];
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00018B14 File Offset: 0x00016D14
		public override bool IsAuthenticated
		{
			get
			{
				return this._user != null;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00018B30 File Offset: 0x00016D30
		public override bool IsLocal
		{
			get
			{
				return this.UserEndPoint.Address.IsLocal();
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00018B54 File Offset: 0x00016D54
		public override bool IsSecureConnection
		{
			get
			{
				return this._secure;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00018B6C File Offset: 0x00016D6C
		public override bool IsWebSocketRequest
		{
			get
			{
				return this._request.IsWebSocketRequest;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00018B8C File Offset: 0x00016D8C
		public override string Origin
		{
			get
			{
				return this._request.Headers["Origin"];
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00018BB4 File Offset: 0x00016DB4
		public override NameValueCollection QueryString
		{
			get
			{
				bool flag = this._queryString == null;
				if (flag)
				{
					Uri requestUri = this.RequestUri;
					this._queryString = QueryStringCollection.Parse((requestUri != null) ? requestUri.Query : null, Encoding.UTF8);
				}
				return this._queryString;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x00018C04 File Offset: 0x00016E04
		public override Uri RequestUri
		{
			get
			{
				bool flag = this._requestUri == null;
				if (flag)
				{
					this._requestUri = HttpUtility.CreateRequestUrl(this._request.RequestUri, this._request.Headers["Host"], this._request.IsWebSocketRequest, this._secure);
				}
				return this._requestUri;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00018C6C File Offset: 0x00016E6C
		public override string SecWebSocketKey
		{
			get
			{
				return this._request.Headers["Sec-WebSocket-Key"];
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00018C94 File Offset: 0x00016E94
		public override IEnumerable<string> SecWebSocketProtocols
		{
			get
			{
				string val = this._request.Headers["Sec-WebSocket-Protocol"];
				bool flag = val == null || val.Length == 0;
				if (flag)
				{
					yield break;
				}
				foreach (string elm in val.Split(new char[]
				{
					','
				}))
				{
					string protocol = elm.Trim();
					bool flag2 = protocol.Length == 0;
					if (!flag2)
					{
						yield return protocol;
						protocol = null;
						elm = null;
					}
				}
				string[] array = null;
				yield break;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00018CB4 File Offset: 0x00016EB4
		public override string SecWebSocketVersion
		{
			get
			{
				return this._request.Headers["Sec-WebSocket-Version"];
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00018CDC File Offset: 0x00016EDC
		public override IPEndPoint ServerEndPoint
		{
			get
			{
				return (IPEndPoint)this._serverEndPoint;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00018CFC File Offset: 0x00016EFC
		public override IPrincipal User
		{
			get
			{
				return this._user;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00018D14 File Offset: 0x00016F14
		public override IPEndPoint UserEndPoint
		{
			get
			{
				return (IPEndPoint)this._userEndPoint;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00018D34 File Offset: 0x00016F34
		public override WebSocket WebSocket
		{
			get
			{
				return this._websocket;
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00018D4C File Offset: 0x00016F4C
		private HttpRequest sendAuthenticationChallenge(string challenge)
		{
			HttpResponse httpResponse = HttpResponse.CreateUnauthorizedResponse(challenge);
			byte[] array = httpResponse.ToByteArray();
			this._stream.Write(array, 0, array.Length);
			return HttpRequest.Read(this._stream, 15000);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00018D90 File Offset: 0x00016F90
		internal bool Authenticate(AuthenticationSchemes scheme, string realm, Func<IIdentity, NetworkCredential> credentialsFinder)
		{
			string chal = new AuthenticationChallenge(scheme, realm).ToString();
			int retry = -1;
			Func<bool> auth = null;
			auth = delegate()
			{
				int retry = retry;
				retry++;
				bool flag = retry > 99;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					IPrincipal principal = HttpUtility.CreateUser(this._request.Headers["Authorization"], scheme, realm, this._request.HttpMethod, credentialsFinder);
					bool flag2 = principal != null && principal.Identity.IsAuthenticated;
					if (flag2)
					{
						this._user = principal;
						result = true;
					}
					else
					{
						this._request = this.sendAuthenticationChallenge(chal);
						result = auth();
					}
				}
				return result;
			};
			return auth();
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00018E0B File Offset: 0x0001700B
		internal void Close()
		{
			this._stream.Close();
			this._tcpClient.Close();
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00018E28 File Offset: 0x00017028
		internal void Close(HttpStatusCode code)
		{
			HttpResponse httpResponse = HttpResponse.CreateCloseResponse(code);
			byte[] array = httpResponse.ToByteArray();
			this._stream.Write(array, 0, array.Length);
			this._stream.Close();
			this._tcpClient.Close();
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00018E70 File Offset: 0x00017070
		public override string ToString()
		{
			return this._request.ToString();
		}

		// Token: 0x0400020B RID: 523
		private Logger _log;

		// Token: 0x0400020C RID: 524
		private NameValueCollection _queryString;

		// Token: 0x0400020D RID: 525
		private HttpRequest _request;

		// Token: 0x0400020E RID: 526
		private Uri _requestUri;

		// Token: 0x0400020F RID: 527
		private bool _secure;

		// Token: 0x04000210 RID: 528
		private EndPoint _serverEndPoint;

		// Token: 0x04000211 RID: 529
		private Stream _stream;

		// Token: 0x04000212 RID: 530
		private TcpClient _tcpClient;

		// Token: 0x04000213 RID: 531
		private IPrincipal _user;

		// Token: 0x04000214 RID: 532
		private EndPoint _userEndPoint;

		// Token: 0x04000215 RID: 533
		private WebSocket _websocket;
	}
}
