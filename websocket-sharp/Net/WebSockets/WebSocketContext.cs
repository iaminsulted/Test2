using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Security.Principal;

namespace WebSocketSharp.Net.WebSockets
{
	// Token: 0x02000044 RID: 68
	public abstract class WebSocketContext
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600046C RID: 1132
		public abstract CookieCollection CookieCollection { get; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600046D RID: 1133
		public abstract NameValueCollection Headers { get; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600046E RID: 1134
		public abstract string Host { get; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600046F RID: 1135
		public abstract bool IsAuthenticated { get; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000470 RID: 1136
		public abstract bool IsLocal { get; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000471 RID: 1137
		public abstract bool IsSecureConnection { get; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000472 RID: 1138
		public abstract bool IsWebSocketRequest { get; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000473 RID: 1139
		public abstract string Origin { get; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000474 RID: 1140
		public abstract NameValueCollection QueryString { get; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000475 RID: 1141
		public abstract Uri RequestUri { get; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000476 RID: 1142
		public abstract string SecWebSocketKey { get; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000477 RID: 1143
		public abstract IEnumerable<string> SecWebSocketProtocols { get; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000478 RID: 1144
		public abstract string SecWebSocketVersion { get; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000479 RID: 1145
		public abstract IPEndPoint ServerEndPoint { get; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600047A RID: 1146
		public abstract IPrincipal User { get; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600047B RID: 1147
		public abstract IPEndPoint UserEndPoint { get; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600047C RID: 1148
		public abstract WebSocket WebSocket { get; }
	}
}
