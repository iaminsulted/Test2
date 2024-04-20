using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Principal;

namespace WebSocketSharp.Net.WebSockets
{
	// Token: 0x02000042 RID: 66
	public class HttpListenerWebSocketContext : WebSocketContext
	{
		// Token: 0x0600043B RID: 1083 RVA: 0x0001869C File Offset: 0x0001689C
		internal HttpListenerWebSocketContext(HttpListenerContext context, string protocol)
		{
			this._context = context;
			this._websocket = new WebSocket(this, protocol);
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x000186BC File Offset: 0x000168BC
		internal Logger Log
		{
			get
			{
				return this._context.Listener.Log;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x000186E0 File Offset: 0x000168E0
		internal Stream Stream
		{
			get
			{
				return this._context.Connection.Stream;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00018704 File Offset: 0x00016904
		public override CookieCollection CookieCollection
		{
			get
			{
				return this._context.Request.Cookies;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00018728 File Offset: 0x00016928
		public override NameValueCollection Headers
		{
			get
			{
				return this._context.Request.Headers;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0001874C File Offset: 0x0001694C
		public override string Host
		{
			get
			{
				return this._context.Request.UserHostName;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00018770 File Offset: 0x00016970
		public override bool IsAuthenticated
		{
			get
			{
				return this._context.Request.IsAuthenticated;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00018794 File Offset: 0x00016994
		public override bool IsLocal
		{
			get
			{
				return this._context.Request.IsLocal;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000187B8 File Offset: 0x000169B8
		public override bool IsSecureConnection
		{
			get
			{
				return this._context.Request.IsSecureConnection;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x000187DC File Offset: 0x000169DC
		public override bool IsWebSocketRequest
		{
			get
			{
				return this._context.Request.IsWebSocketRequest;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00018800 File Offset: 0x00016A00
		public override string Origin
		{
			get
			{
				return this._context.Request.Headers["Origin"];
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0001882C File Offset: 0x00016A2C
		public override NameValueCollection QueryString
		{
			get
			{
				return this._context.Request.QueryString;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00018850 File Offset: 0x00016A50
		public override Uri RequestUri
		{
			get
			{
				return this._context.Request.Url;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00018874 File Offset: 0x00016A74
		public override string SecWebSocketKey
		{
			get
			{
				return this._context.Request.Headers["Sec-WebSocket-Key"];
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x000188A0 File Offset: 0x00016AA0
		public override IEnumerable<string> SecWebSocketProtocols
		{
			get
			{
				string val = this._context.Request.Headers["Sec-WebSocket-Protocol"];
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

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x000188C0 File Offset: 0x00016AC0
		public override string SecWebSocketVersion
		{
			get
			{
				return this._context.Request.Headers["Sec-WebSocket-Version"];
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x000188EC File Offset: 0x00016AEC
		public override IPEndPoint ServerEndPoint
		{
			get
			{
				return this._context.Request.LocalEndPoint;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00018910 File Offset: 0x00016B10
		public override IPrincipal User
		{
			get
			{
				return this._context.User;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00018930 File Offset: 0x00016B30
		public override IPEndPoint UserEndPoint
		{
			get
			{
				return this._context.Request.RemoteEndPoint;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00018954 File Offset: 0x00016B54
		public override WebSocket WebSocket
		{
			get
			{
				return this._websocket;
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001896C File Offset: 0x00016B6C
		internal void Close()
		{
			this._context.Connection.Close(true);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00018981 File Offset: 0x00016B81
		internal void Close(HttpStatusCode code)
		{
			this._context.Response.Close(code);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00018998 File Offset: 0x00016B98
		public override string ToString()
		{
			return this._context.Request.ToString();
		}

		// Token: 0x04000209 RID: 521
		private HttpListenerContext _context;

		// Token: 0x0400020A RID: 522
		private WebSocket _websocket;
	}
}
