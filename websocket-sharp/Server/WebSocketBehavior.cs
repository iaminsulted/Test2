using System;
using System.Collections.Specialized;
using System.IO;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server
{
	// Token: 0x0200004D RID: 77
	public abstract class WebSocketBehavior : IWebSocketSession
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x0001E278 File Offset: 0x0001C478
		protected WebSocketBehavior()
		{
			this._startTime = DateTime.MaxValue;
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001E290 File Offset: 0x0001C490
		protected NameValueCollection Headers
		{
			get
			{
				return (this._context != null) ? this._context.Headers : null;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0001E2B8 File Offset: 0x0001C4B8
		[Obsolete("This property will be removed.")]
		protected Logger Log
		{
			get
			{
				return (this._websocket != null) ? this._websocket.Log : null;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001E2E0 File Offset: 0x0001C4E0
		protected NameValueCollection QueryString
		{
			get
			{
				return (this._context != null) ? this._context.QueryString : null;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0001E308 File Offset: 0x0001C508
		protected WebSocketSessionManager Sessions
		{
			get
			{
				return this._sessions;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0001E320 File Offset: 0x0001C520
		public WebSocketState ConnectionState
		{
			get
			{
				return (this._websocket != null) ? this._websocket.ReadyState : WebSocketState.Connecting;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0001E348 File Offset: 0x0001C548
		public WebSocketContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001E360 File Offset: 0x0001C560
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0001E378 File Offset: 0x0001C578
		public Func<CookieCollection, CookieCollection, bool> CookiesValidator
		{
			get
			{
				return this._cookiesValidator;
			}
			set
			{
				this._cookiesValidator = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001E384 File Offset: 0x0001C584
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x0001E3B4 File Offset: 0x0001C5B4
		public bool EmitOnPing
		{
			get
			{
				return (this._websocket != null) ? this._websocket.EmitOnPing : this._emitOnPing;
			}
			set
			{
				bool flag = this._websocket != null;
				if (flag)
				{
					this._websocket.EmitOnPing = value;
				}
				else
				{
					this._emitOnPing = value;
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001E3E8 File Offset: 0x0001C5E8
		public string ID
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0001E400 File Offset: 0x0001C600
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x0001E418 File Offset: 0x0001C618
		public bool IgnoreExtensions
		{
			get
			{
				return this._ignoreExtensions;
			}
			set
			{
				this._ignoreExtensions = value;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0001E424 File Offset: 0x0001C624
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x0001E43C File Offset: 0x0001C63C
		public Func<string, bool> OriginValidator
		{
			get
			{
				return this._originValidator;
			}
			set
			{
				this._originValidator = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0001E448 File Offset: 0x0001C648
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x0001E480 File Offset: 0x0001C680
		public string Protocol
		{
			get
			{
				return (this._websocket != null) ? this._websocket.Protocol : (this._protocol ?? string.Empty);
			}
			set
			{
				bool flag = this.ConnectionState > WebSocketState.Connecting;
				if (flag)
				{
					string message = "The session has already started.";
					throw new InvalidOperationException(message);
				}
				bool flag2 = value == null || value.Length == 0;
				if (flag2)
				{
					this._protocol = null;
				}
				else
				{
					bool flag3 = !value.IsToken();
					if (flag3)
					{
						throw new ArgumentException("Not a token.", "value");
					}
					this._protocol = value;
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0001E4EC File Offset: 0x0001C6EC
		public DateTime StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001E504 File Offset: 0x0001C704
		private string checkHandshakeRequest(WebSocketContext context)
		{
			bool flag = this._originValidator != null;
			if (flag)
			{
				bool flag2 = !this._originValidator(context.Origin);
				if (flag2)
				{
					return "It includes no Origin header or an invalid one.";
				}
			}
			bool flag3 = this._cookiesValidator != null;
			if (flag3)
			{
				CookieCollection cookieCollection = context.CookieCollection;
				CookieCollection cookieCollection2 = context.WebSocket.CookieCollection;
				bool flag4 = !this._cookiesValidator(cookieCollection, cookieCollection2);
				if (flag4)
				{
					return "It includes no cookie or an invalid one.";
				}
			}
			return null;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001E58C File Offset: 0x0001C78C
		private void onClose(object sender, CloseEventArgs e)
		{
			bool flag = this._id == null;
			if (!flag)
			{
				this._sessions.Remove(this._id);
				this.OnClose(e);
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001E5C3 File Offset: 0x0001C7C3
		private void onError(object sender, ErrorEventArgs e)
		{
			this.OnError(e);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001E5CE File Offset: 0x0001C7CE
		private void onMessage(object sender, MessageEventArgs e)
		{
			this.OnMessage(e);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001E5DC File Offset: 0x0001C7DC
		private void onOpen(object sender, EventArgs e)
		{
			this._id = this._sessions.Add(this);
			bool flag = this._id == null;
			if (flag)
			{
				this._websocket.Close(CloseStatusCode.Away);
			}
			else
			{
				this._startTime = DateTime.Now;
				this.OnOpen();
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001E630 File Offset: 0x0001C830
		internal void Start(WebSocketContext context, WebSocketSessionManager sessions)
		{
			bool flag = this._websocket != null;
			if (flag)
			{
				this._websocket.Log.Error("A session instance cannot be reused.");
				context.WebSocket.Close(HttpStatusCode.ServiceUnavailable);
			}
			else
			{
				this._context = context;
				this._sessions = sessions;
				this._websocket = context.WebSocket;
				this._websocket.CustomHandshakeRequestChecker = new Func<WebSocketContext, string>(this.checkHandshakeRequest);
				this._websocket.EmitOnPing = this._emitOnPing;
				this._websocket.IgnoreExtensions = this._ignoreExtensions;
				this._websocket.Protocol = this._protocol;
				TimeSpan waitTime = sessions.WaitTime;
				bool flag2 = waitTime != this._websocket.WaitTime;
				if (flag2)
				{
					this._websocket.WaitTime = waitTime;
				}
				this._websocket.OnOpen += this.onOpen;
				this._websocket.OnMessage += this.onMessage;
				this._websocket.OnError += this.onError;
				this._websocket.OnClose += this.onClose;
				this._websocket.InternalAccept();
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001E778 File Offset: 0x0001C978
		protected void Close()
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The session has not started yet.";
				throw new InvalidOperationException(message);
			}
			this._websocket.Close();
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001E7B0 File Offset: 0x0001C9B0
		protected void Close(ushort code, string reason)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The session has not started yet.";
				throw new InvalidOperationException(message);
			}
			this._websocket.Close(code, reason);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001E7E8 File Offset: 0x0001C9E8
		protected void Close(CloseStatusCode code, string reason)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The session has not started yet.";
				throw new InvalidOperationException(message);
			}
			this._websocket.Close(code, reason);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001E820 File Offset: 0x0001CA20
		protected void CloseAsync()
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The session has not started yet.";
				throw new InvalidOperationException(message);
			}
			this._websocket.CloseAsync();
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001E858 File Offset: 0x0001CA58
		protected void CloseAsync(ushort code, string reason)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The session has not started yet.";
				throw new InvalidOperationException(message);
			}
			this._websocket.CloseAsync(code, reason);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001E890 File Offset: 0x0001CA90
		protected void CloseAsync(CloseStatusCode code, string reason)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The session has not started yet.";
				throw new InvalidOperationException(message);
			}
			this._websocket.CloseAsync(code, reason);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001E8C8 File Offset: 0x0001CAC8
		[Obsolete("This method will be removed.")]
		protected void Error(string message, Exception exception)
		{
			bool flag = message == null;
			if (flag)
			{
				throw new ArgumentNullException("message");
			}
			bool flag2 = message.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("An empty string.", "message");
			}
			this.OnError(new ErrorEventArgs(message, exception));
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00014C7C File Offset: 0x00012E7C
		protected virtual void OnClose(CloseEventArgs e)
		{
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00014C7C File Offset: 0x00012E7C
		protected virtual void OnError(ErrorEventArgs e)
		{
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00014C7C File Offset: 0x00012E7C
		protected virtual void OnMessage(MessageEventArgs e)
		{
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00014C7C File Offset: 0x00012E7C
		protected virtual void OnOpen()
		{
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001E914 File Offset: 0x0001CB14
		protected void Send(byte[] data)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			this._websocket.Send(data);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001E94C File Offset: 0x0001CB4C
		protected void Send(FileInfo fileInfo)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			this._websocket.Send(fileInfo);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001E984 File Offset: 0x0001CB84
		protected void Send(string data)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			this._websocket.Send(data);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001E9BC File Offset: 0x0001CBBC
		protected void Send(Stream stream, int length)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			this._websocket.Send(stream, length);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001E9F4 File Offset: 0x0001CBF4
		protected void SendAsync(byte[] data, Action<bool> completed)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			this._websocket.SendAsync(data, completed);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001EA2C File Offset: 0x0001CC2C
		protected void SendAsync(FileInfo fileInfo, Action<bool> completed)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			this._websocket.SendAsync(fileInfo, completed);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001EA64 File Offset: 0x0001CC64
		protected void SendAsync(string data, Action<bool> completed)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			this._websocket.SendAsync(data, completed);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001EA9C File Offset: 0x0001CC9C
		protected void SendAsync(Stream stream, int length, Action<bool> completed)
		{
			bool flag = this._websocket == null;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			this._websocket.SendAsync(stream, length, completed);
		}

		// Token: 0x04000256 RID: 598
		private WebSocketContext _context;

		// Token: 0x04000257 RID: 599
		private Func<CookieCollection, CookieCollection, bool> _cookiesValidator;

		// Token: 0x04000258 RID: 600
		private bool _emitOnPing;

		// Token: 0x04000259 RID: 601
		private string _id;

		// Token: 0x0400025A RID: 602
		private bool _ignoreExtensions;

		// Token: 0x0400025B RID: 603
		private Func<string, bool> _originValidator;

		// Token: 0x0400025C RID: 604
		private string _protocol;

		// Token: 0x0400025D RID: 605
		private WebSocketSessionManager _sessions;

		// Token: 0x0400025E RID: 606
		private DateTime _startTime;

		// Token: 0x0400025F RID: 607
		private WebSocket _websocket;
	}
}
