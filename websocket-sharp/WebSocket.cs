using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp
{
	// Token: 0x02000007 RID: 7
	public class WebSocket : IDisposable
	{
		// Token: 0x06000087 RID: 135 RVA: 0x0000472C File Offset: 0x0000292C
		internal WebSocket(HttpListenerWebSocketContext context, string protocol)
		{
			this._context = context;
			this._protocol = protocol;
			this._closeContext = new Action(context.Close);
			this._logger = context.Log;
			this._message = new Action<MessageEventArgs>(this.messages);
			this._secure = context.IsSecureConnection;
			this._stream = context.Stream;
			this._waitTime = TimeSpan.FromSeconds(1.0);
			this.init();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000047B4 File Offset: 0x000029B4
		internal WebSocket(TcpListenerWebSocketContext context, string protocol)
		{
			this._context = context;
			this._protocol = protocol;
			this._closeContext = new Action(context.Close);
			this._logger = context.Log;
			this._message = new Action<MessageEventArgs>(this.messages);
			this._secure = context.IsSecureConnection;
			this._stream = context.Stream;
			this._waitTime = TimeSpan.FromSeconds(1.0);
			this.init();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000483C File Offset: 0x00002A3C
		public WebSocket(string url, params string[] protocols)
		{
			bool flag = url == null;
			if (flag)
			{
				throw new ArgumentNullException("url");
			}
			bool flag2 = url.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("An empty string.", "url");
			}
			string message;
			bool flag3 = !url.TryCreateWebSocketUri(out this._uri, out message);
			if (flag3)
			{
				throw new ArgumentException(message, "url");
			}
			bool flag4 = protocols != null && protocols.Length != 0;
			if (flag4)
			{
				bool flag5 = !WebSocket.checkProtocols(protocols, out message);
				if (flag5)
				{
					throw new ArgumentException(message, "protocols");
				}
				this._protocols = protocols;
			}
			this._base64Key = WebSocket.CreateBase64Key();
			this._client = true;
			this._logger = new Logger();
			this._message = new Action<MessageEventArgs>(this.messagec);
			this._secure = (this._uri.Scheme == "wss");
			this._waitTime = TimeSpan.FromSeconds(5.0);
			this.init();
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004940 File Offset: 0x00002B40
		internal CookieCollection CookieCollection
		{
			get
			{
				return this._cookies;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00004958 File Offset: 0x00002B58
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00004970 File Offset: 0x00002B70
		internal Func<WebSocketContext, string> CustomHandshakeRequestChecker
		{
			get
			{
				return this._handshakeRequestChecker;
			}
			set
			{
				this._handshakeRequestChecker = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000497C File Offset: 0x00002B7C
		internal bool HasMessage
		{
			get
			{
				object forMessageEventQueue = this._forMessageEventQueue;
				bool result;
				lock (forMessageEventQueue)
				{
					result = (this._messageEventQueue.Count > 0);
				}
				return result;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000049C4 File Offset: 0x00002BC4
		// (set) Token: 0x0600008F RID: 143 RVA: 0x000049DC File Offset: 0x00002BDC
		internal bool IgnoreExtensions
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000049E8 File Offset: 0x00002BE8
		internal bool IsConnected
		{
			get
			{
				return this._readyState == WebSocketState.Open || this._readyState == WebSocketState.Closing;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004A14 File Offset: 0x00002C14
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00004A2C File Offset: 0x00002C2C
		public CompressionMethod Compression
		{
			get
			{
				return this._compression;
			}
			set
			{
				string message = null;
				bool flag = !this._client;
				if (flag)
				{
					message = "This instance is not a client.";
					throw new InvalidOperationException(message);
				}
				bool flag2 = !this.canSet(out message);
				if (flag2)
				{
					this._logger.Warn(message);
				}
				else
				{
					object forState = this._forState;
					lock (forState)
					{
						bool flag3 = !this.canSet(out message);
						if (flag3)
						{
							this._logger.Warn(message);
						}
						else
						{
							this._compression = value;
						}
					}
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004ACC File Offset: 0x00002CCC
		public IEnumerable<Cookie> Cookies
		{
			get
			{
				object obj = this._cookies.SyncRoot;
				lock (obj)
				{
					foreach (object obj2 in this._cookies)
					{
						Cookie cookie = (Cookie)obj2;
						yield return cookie;
						cookie = null;
					}
					IEnumerator enumerator = null;
				}
				obj = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004AEC File Offset: 0x00002CEC
		public NetworkCredential Credentials
		{
			get
			{
				return this._credentials;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004B04 File Offset: 0x00002D04
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004B1C File Offset: 0x00002D1C
		public bool EmitOnPing
		{
			get
			{
				return this._emitOnPing;
			}
			set
			{
				this._emitOnPing = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004B28 File Offset: 0x00002D28
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00004B40 File Offset: 0x00002D40
		public bool EnableRedirection
		{
			get
			{
				return this._enableRedirection;
			}
			set
			{
				string message = null;
				bool flag = !this._client;
				if (flag)
				{
					message = "This instance is not a client.";
					throw new InvalidOperationException(message);
				}
				bool flag2 = !this.canSet(out message);
				if (flag2)
				{
					this._logger.Warn(message);
				}
				else
				{
					object forState = this._forState;
					lock (forState)
					{
						bool flag3 = !this.canSet(out message);
						if (flag3)
						{
							this._logger.Warn(message);
						}
						else
						{
							this._enableRedirection = value;
						}
					}
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004BE0 File Offset: 0x00002DE0
		public string Extensions
		{
			get
			{
				return this._extensions ?? string.Empty;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004C04 File Offset: 0x00002E04
		public bool IsAlive
		{
			get
			{
				return this.ping(WebSocket.EmptyBytes);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004C24 File Offset: 0x00002E24
		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004C3C File Offset: 0x00002E3C
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00004C56 File Offset: 0x00002E56
		public Logger Log
		{
			get
			{
				return this._logger;
			}
			internal set
			{
				this._logger = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004C64 File Offset: 0x00002E64
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00004C7C File Offset: 0x00002E7C
		public string Origin
		{
			get
			{
				return this._origin;
			}
			set
			{
				string message = null;
				bool flag = !this._client;
				if (flag)
				{
					message = "This instance is not a client.";
					throw new InvalidOperationException(message);
				}
				bool flag2 = !value.IsNullOrEmpty();
				if (flag2)
				{
					Uri uri;
					bool flag3 = !Uri.TryCreate(value, UriKind.Absolute, out uri);
					if (flag3)
					{
						message = "Not an absolute URI string.";
						throw new ArgumentException(message, "value");
					}
					bool flag4 = uri.Segments.Length > 1;
					if (flag4)
					{
						message = "It includes the path segments.";
						throw new ArgumentException(message, "value");
					}
				}
				bool flag5 = !this.canSet(out message);
				if (flag5)
				{
					this._logger.Warn(message);
				}
				else
				{
					object forState = this._forState;
					lock (forState)
					{
						bool flag6 = !this.canSet(out message);
						if (flag6)
						{
							this._logger.Warn(message);
						}
						else
						{
							this._origin = ((!value.IsNullOrEmpty()) ? value.TrimEnd(new char[]
							{
								'/'
							}) : value);
						}
					}
				}
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004D94 File Offset: 0x00002F94
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00004DB5 File Offset: 0x00002FB5
		public string Protocol
		{
			get
			{
				return this._protocol ?? string.Empty;
			}
			internal set
			{
				this._protocol = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004DC0 File Offset: 0x00002FC0
		public WebSocketState ReadyState
		{
			get
			{
				return this._readyState;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004DDC File Offset: 0x00002FDC
		public ClientSslConfiguration SslConfiguration
		{
			get
			{
				bool flag = !this._client;
				if (flag)
				{
					string message = "This instance is not a client.";
					throw new InvalidOperationException(message);
				}
				bool flag2 = !this._secure;
				if (flag2)
				{
					string message2 = "This instance does not use a secure connection.";
					throw new InvalidOperationException(message2);
				}
				return this.getSslConfiguration();
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004E2C File Offset: 0x0000302C
		public Uri Url
		{
			get
			{
				return this._client ? this._uri : this._context.RequestUri;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004E5C File Offset: 0x0000305C
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00004E74 File Offset: 0x00003074
		public TimeSpan WaitTime
		{
			get
			{
				return this._waitTime;
			}
			set
			{
				bool flag = value <= TimeSpan.Zero;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("value", "Zero or less.");
				}
				string message;
				bool flag2 = !this.canSet(out message);
				if (flag2)
				{
					this._logger.Warn(message);
				}
				else
				{
					object forState = this._forState;
					lock (forState)
					{
						bool flag3 = !this.canSet(out message);
						if (flag3)
						{
							this._logger.Warn(message);
						}
						else
						{
							this._waitTime = value;
						}
					}
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000A7 RID: 167 RVA: 0x00004F18 File Offset: 0x00003118
		// (remove) Token: 0x060000A8 RID: 168 RVA: 0x00004F50 File Offset: 0x00003150
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<CloseEventArgs> OnClose;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000A9 RID: 169 RVA: 0x00004F88 File Offset: 0x00003188
		// (remove) Token: 0x060000AA RID: 170 RVA: 0x00004FC0 File Offset: 0x000031C0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ErrorEventArgs> OnError;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000AB RID: 171 RVA: 0x00004FF8 File Offset: 0x000031F8
		// (remove) Token: 0x060000AC RID: 172 RVA: 0x00005030 File Offset: 0x00003230
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<MessageEventArgs> OnMessage;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000AD RID: 173 RVA: 0x00005068 File Offset: 0x00003268
		// (remove) Token: 0x060000AE RID: 174 RVA: 0x000050A0 File Offset: 0x000032A0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler OnOpen;

		// Token: 0x060000AF RID: 175 RVA: 0x000050D8 File Offset: 0x000032D8
		private bool accept()
		{
			bool flag = this._readyState == WebSocketState.Open;
			bool result;
			if (flag)
			{
				string message = "The handshake request has already been accepted.";
				this._logger.Warn(message);
				result = false;
			}
			else
			{
				object forState = this._forState;
				lock (forState)
				{
					bool flag2 = this._readyState == WebSocketState.Open;
					if (flag2)
					{
						string message2 = "The handshake request has already been accepted.";
						this._logger.Warn(message2);
						result = false;
					}
					else
					{
						bool flag3 = this._readyState == WebSocketState.Closing;
						if (flag3)
						{
							string message3 = "The close process has set in.";
							this._logger.Error(message3);
							message3 = "An interruption has occurred while attempting to accept.";
							this.error(message3, null);
							result = false;
						}
						else
						{
							bool flag4 = this._readyState == WebSocketState.Closed;
							if (flag4)
							{
								string message4 = "The connection has been closed.";
								this._logger.Error(message4);
								message4 = "An interruption has occurred while attempting to accept.";
								this.error(message4, null);
								result = false;
							}
							else
							{
								try
								{
									bool flag5 = !this.acceptHandshake();
									if (flag5)
									{
										return false;
									}
								}
								catch (Exception ex)
								{
									this._logger.Fatal(ex.Message);
									this._logger.Debug(ex.ToString());
									string message5 = "An exception has occurred while attempting to accept.";
									this.fatal(message5, ex);
									return false;
								}
								this._readyState = WebSocketState.Open;
								result = true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005278 File Offset: 0x00003478
		private bool acceptHandshake()
		{
			this._logger.Debug(string.Format("A handshake request from {0}:\n{1}", this._context.UserEndPoint, this._context));
			string message;
			bool flag = !this.checkHandshakeRequest(this._context, out message);
			bool result;
			if (flag)
			{
				this._logger.Error(message);
				this.refuseHandshake(CloseStatusCode.ProtocolError, "A handshake error has occurred while attempting to accept.");
				result = false;
			}
			else
			{
				bool flag2 = !this.customCheckHandshakeRequest(this._context, out message);
				if (flag2)
				{
					this._logger.Error(message);
					this.refuseHandshake(CloseStatusCode.PolicyViolation, "A handshake error has occurred while attempting to accept.");
					result = false;
				}
				else
				{
					this._base64Key = this._context.Headers["Sec-WebSocket-Key"];
					bool flag3 = this._protocol != null;
					if (flag3)
					{
						IEnumerable<string> secWebSocketProtocols = this._context.SecWebSocketProtocols;
						this.processSecWebSocketProtocolClientHeader(secWebSocketProtocols);
					}
					bool flag4 = !this._ignoreExtensions;
					if (flag4)
					{
						string value = this._context.Headers["Sec-WebSocket-Extensions"];
						this.processSecWebSocketExtensionsClientHeader(value);
					}
					result = this.sendHttpResponse(this.createHandshakeResponse());
				}
			}
			return result;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000053AC File Offset: 0x000035AC
		private bool canSet(out string message)
		{
			message = null;
			bool flag = this._readyState == WebSocketState.Open;
			bool result;
			if (flag)
			{
				message = "The connection has already been established.";
				result = false;
			}
			else
			{
				bool flag2 = this._readyState == WebSocketState.Closing;
				if (flag2)
				{
					message = "The connection is closing.";
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000053F8 File Offset: 0x000035F8
		private bool checkHandshakeRequest(WebSocketContext context, out string message)
		{
			message = null;
			bool flag = !context.IsWebSocketRequest;
			bool result;
			if (flag)
			{
				message = "Not a handshake request.";
				result = false;
			}
			else
			{
				bool flag2 = context.RequestUri == null;
				if (flag2)
				{
					message = "It specifies an invalid Request-URI.";
					result = false;
				}
				else
				{
					NameValueCollection headers = context.Headers;
					string text = headers["Sec-WebSocket-Key"];
					bool flag3 = text == null;
					if (flag3)
					{
						message = "It includes no Sec-WebSocket-Key header.";
						result = false;
					}
					else
					{
						bool flag4 = text.Length == 0;
						if (flag4)
						{
							message = "It includes an invalid Sec-WebSocket-Key header.";
							result = false;
						}
						else
						{
							string text2 = headers["Sec-WebSocket-Version"];
							bool flag5 = text2 == null;
							if (flag5)
							{
								message = "It includes no Sec-WebSocket-Version header.";
								result = false;
							}
							else
							{
								bool flag6 = text2 != "13";
								if (flag6)
								{
									message = "It includes an invalid Sec-WebSocket-Version header.";
									result = false;
								}
								else
								{
									string text3 = headers["Sec-WebSocket-Protocol"];
									bool flag7 = text3 != null && text3.Length == 0;
									if (flag7)
									{
										message = "It includes an invalid Sec-WebSocket-Protocol header.";
										result = false;
									}
									else
									{
										bool flag8 = !this._ignoreExtensions;
										if (flag8)
										{
											string text4 = headers["Sec-WebSocket-Extensions"];
											bool flag9 = text4 != null && text4.Length == 0;
											if (flag9)
											{
												message = "It includes an invalid Sec-WebSocket-Extensions header.";
												return false;
											}
										}
										result = true;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005554 File Offset: 0x00003754
		private bool checkHandshakeResponse(HttpResponse response, out string message)
		{
			message = null;
			bool isRedirect = response.IsRedirect;
			bool result;
			if (isRedirect)
			{
				message = "Indicates the redirection.";
				result = false;
			}
			else
			{
				bool isUnauthorized = response.IsUnauthorized;
				if (isUnauthorized)
				{
					message = "Requires the authentication.";
					result = false;
				}
				else
				{
					bool flag = !response.IsWebSocketResponse;
					if (flag)
					{
						message = "Not a WebSocket handshake response.";
						result = false;
					}
					else
					{
						NameValueCollection headers = response.Headers;
						bool flag2 = !this.validateSecWebSocketAcceptHeader(headers["Sec-WebSocket-Accept"]);
						if (flag2)
						{
							message = "Includes no Sec-WebSocket-Accept header, or it has an invalid value.";
							result = false;
						}
						else
						{
							bool flag3 = !this.validateSecWebSocketProtocolServerHeader(headers["Sec-WebSocket-Protocol"]);
							if (flag3)
							{
								message = "Includes no Sec-WebSocket-Protocol header, or it has an invalid value.";
								result = false;
							}
							else
							{
								bool flag4 = !this.validateSecWebSocketExtensionsServerHeader(headers["Sec-WebSocket-Extensions"]);
								if (flag4)
								{
									message = "Includes an invalid Sec-WebSocket-Extensions header.";
									result = false;
								}
								else
								{
									bool flag5 = !this.validateSecWebSocketVersionServerHeader(headers["Sec-WebSocket-Version"]);
									if (flag5)
									{
										message = "Includes an invalid Sec-WebSocket-Version header.";
										result = false;
									}
									else
									{
										result = true;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000565C File Offset: 0x0000385C
		private static bool checkProtocols(string[] protocols, out string message)
		{
			message = null;
			Func<string, bool> condition = (string protocol) => protocol.IsNullOrEmpty() || !protocol.IsToken();
			bool flag = protocols.Contains(condition);
			bool result;
			if (flag)
			{
				message = "It contains a value that is not a token.";
				result = false;
			}
			else
			{
				bool flag2 = protocols.ContainsTwice();
				if (flag2)
				{
					message = "It contains a value twice.";
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000056C0 File Offset: 0x000038C0
		private bool checkReceivedFrame(WebSocketFrame frame, out string message)
		{
			message = null;
			bool isMasked = frame.IsMasked;
			bool flag = this._client && isMasked;
			bool result;
			if (flag)
			{
				message = "A frame from the server is masked.";
				result = false;
			}
			else
			{
				bool flag2 = !this._client && !isMasked;
				if (flag2)
				{
					message = "A frame from a client is not masked.";
					result = false;
				}
				else
				{
					bool flag3 = this._inContinuation && frame.IsData;
					if (flag3)
					{
						message = "A data frame has been received while receiving continuation frames.";
						result = false;
					}
					else
					{
						bool flag4 = frame.IsCompressed && this._compression == CompressionMethod.None;
						if (flag4)
						{
							message = "A compressed frame has been received without any agreement for it.";
							result = false;
						}
						else
						{
							bool flag5 = frame.Rsv2 == Rsv.On;
							if (flag5)
							{
								message = "The RSV2 of a frame is non-zero without any negotiation for it.";
								result = false;
							}
							else
							{
								bool flag6 = frame.Rsv3 == Rsv.On;
								if (flag6)
								{
									message = "The RSV3 of a frame is non-zero without any negotiation for it.";
									result = false;
								}
								else
								{
									result = true;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000579C File Offset: 0x0000399C
		private void close(ushort code, string reason)
		{
			bool flag = this._readyState == WebSocketState.Closing;
			if (flag)
			{
				this._logger.Info("The closing is already in progress.");
			}
			else
			{
				bool flag2 = this._readyState == WebSocketState.Closed;
				if (flag2)
				{
					this._logger.Info("The connection has already been closed.");
				}
				else
				{
					bool flag3 = code == 1005;
					if (flag3)
					{
						this.close(PayloadData.Empty, true, true, false);
					}
					else
					{
						bool flag4 = !code.IsReserved();
						this.close(new PayloadData(code, reason), flag4, flag4, false);
					}
				}
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005830 File Offset: 0x00003A30
		private void close(PayloadData payloadData, bool send, bool receive, bool received)
		{
			object forState = this._forState;
			lock (forState)
			{
				bool flag = this._readyState == WebSocketState.Closing;
				if (flag)
				{
					this._logger.Info("The closing is already in progress.");
					return;
				}
				bool flag2 = this._readyState == WebSocketState.Closed;
				if (flag2)
				{
					this._logger.Info("The connection has already been closed.");
					return;
				}
				send = (send && this._readyState == WebSocketState.Open);
				receive = (send && receive);
				this._readyState = WebSocketState.Closing;
			}
			this._logger.Trace("Begin closing the connection.");
			bool wasClean = this.closeHandshake(payloadData, send, receive, received);
			this.releaseResources();
			this._logger.Trace("End closing the connection.");
			this._readyState = WebSocketState.Closed;
			CloseEventArgs closeEventArgs = new CloseEventArgs(payloadData);
			closeEventArgs.WasClean = wasClean;
			try
			{
				this.OnClose.Emit(this, closeEventArgs);
			}
			catch (Exception ex)
			{
				this._logger.Error(ex.ToString());
				this.error("An error has occurred during the OnClose event.", ex);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005974 File Offset: 0x00003B74
		private void closeAsync(ushort code, string reason)
		{
			bool flag = this._readyState == WebSocketState.Closing;
			if (flag)
			{
				this._logger.Info("The closing is already in progress.");
			}
			else
			{
				bool flag2 = this._readyState == WebSocketState.Closed;
				if (flag2)
				{
					this._logger.Info("The connection has already been closed.");
				}
				else
				{
					bool flag3 = code == 1005;
					if (flag3)
					{
						this.closeAsync(PayloadData.Empty, true, true, false);
					}
					else
					{
						bool flag4 = !code.IsReserved();
						this.closeAsync(new PayloadData(code, reason), flag4, flag4, false);
					}
				}
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005A08 File Offset: 0x00003C08
		private void closeAsync(PayloadData payloadData, bool send, bool receive, bool received)
		{
			Action<PayloadData, bool, bool, bool> closer = new Action<PayloadData, bool, bool, bool>(this.close);
			closer.BeginInvoke(payloadData, send, receive, received, delegate(IAsyncResult ar)
			{
				closer.EndInvoke(ar);
			}, null);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005A4C File Offset: 0x00003C4C
		private bool closeHandshake(byte[] frameAsBytes, bool receive, bool received)
		{
			bool flag = frameAsBytes != null && this.sendBytes(frameAsBytes);
			bool flag2 = !received && flag && receive && this._receivingExited != null;
			bool flag3 = flag2;
			if (flag3)
			{
				received = this._receivingExited.WaitOne(this._waitTime);
			}
			bool flag4 = flag && received;
			this._logger.Debug(string.Format("Was clean?: {0}\n  sent: {1}\n  received: {2}", flag4, flag, received));
			return flag4;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005ACC File Offset: 0x00003CCC
		private bool closeHandshake(PayloadData payloadData, bool send, bool receive, bool received)
		{
			bool flag = false;
			if (send)
			{
				WebSocketFrame webSocketFrame = WebSocketFrame.CreateCloseFrame(payloadData, this._client);
				flag = this.sendBytes(webSocketFrame.ToArray());
				bool client = this._client;
				if (client)
				{
					webSocketFrame.Unmask();
				}
			}
			bool flag2 = !received && flag && receive && this._receivingExited != null;
			bool flag3 = flag2;
			if (flag3)
			{
				received = this._receivingExited.WaitOne(this._waitTime);
			}
			bool flag4 = flag && received;
			this._logger.Debug(string.Format("Was clean?: {0}\n  sent: {1}\n  received: {2}", flag4, flag, received));
			return flag4;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005B7C File Offset: 0x00003D7C
		private bool connect()
		{
			bool flag = this._readyState == WebSocketState.Open;
			bool result;
			if (flag)
			{
				string message = "The connection has already been established.";
				this._logger.Warn(message);
				result = false;
			}
			else
			{
				object forState = this._forState;
				lock (forState)
				{
					bool flag2 = this._readyState == WebSocketState.Open;
					if (flag2)
					{
						string message2 = "The connection has already been established.";
						this._logger.Warn(message2);
						result = false;
					}
					else
					{
						bool flag3 = this._readyState == WebSocketState.Closing;
						if (flag3)
						{
							string message3 = "The close process has set in.";
							this._logger.Error(message3);
							message3 = "An interruption has occurred while attempting to connect.";
							this.error(message3, null);
							result = false;
						}
						else
						{
							bool flag4 = this._retryCountForConnect > WebSocket._maxRetryCountForConnect;
							if (flag4)
							{
								string message4 = "An opportunity for reconnecting has been lost.";
								this._logger.Error(message4);
								message4 = "An interruption has occurred while attempting to connect.";
								this.error(message4, null);
								result = false;
							}
							else
							{
								this._readyState = WebSocketState.Connecting;
								try
								{
									this.doHandshake();
								}
								catch (Exception ex)
								{
									this._retryCountForConnect++;
									this._logger.Fatal(ex.Message);
									this._logger.Debug(ex.ToString());
									string message5 = "An exception has occurred while attempting to connect.";
									this.fatal(message5, ex);
									return false;
								}
								this._retryCountForConnect = 1;
								this._readyState = WebSocketState.Open;
								result = true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005D30 File Offset: 0x00003F30
		private string createExtensions()
		{
			StringBuilder stringBuilder = new StringBuilder(80);
			bool flag = this._compression > CompressionMethod.None;
			if (flag)
			{
				string arg = this._compression.ToExtensionString(new string[]
				{
					"server_no_context_takeover",
					"client_no_context_takeover"
				});
				stringBuilder.AppendFormat("{0}, ", arg);
			}
			int length = stringBuilder.Length;
			bool flag2 = length > 2;
			string result;
			if (flag2)
			{
				stringBuilder.Length = length - 2;
				result = stringBuilder.ToString();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005DB4 File Offset: 0x00003FB4
		private HttpResponse createHandshakeFailureResponse(HttpStatusCode code)
		{
			HttpResponse httpResponse = HttpResponse.CreateCloseResponse(code);
			httpResponse.Headers["Sec-WebSocket-Version"] = "13";
			return httpResponse;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005DE4 File Offset: 0x00003FE4
		private HttpRequest createHandshakeRequest()
		{
			HttpRequest httpRequest = HttpRequest.CreateWebSocketRequest(this._uri);
			NameValueCollection headers = httpRequest.Headers;
			bool flag = !this._origin.IsNullOrEmpty();
			if (flag)
			{
				headers["Origin"] = this._origin;
			}
			headers["Sec-WebSocket-Key"] = this._base64Key;
			this._protocolsRequested = (this._protocols != null);
			bool protocolsRequested = this._protocolsRequested;
			if (protocolsRequested)
			{
				headers["Sec-WebSocket-Protocol"] = this._protocols.ToString(", ");
			}
			this._extensionsRequested = (this._compression > CompressionMethod.None);
			bool extensionsRequested = this._extensionsRequested;
			if (extensionsRequested)
			{
				headers["Sec-WebSocket-Extensions"] = this.createExtensions();
			}
			headers["Sec-WebSocket-Version"] = "13";
			AuthenticationResponse authenticationResponse = null;
			bool flag2 = this._authChallenge != null && this._credentials != null;
			if (flag2)
			{
				authenticationResponse = new AuthenticationResponse(this._authChallenge, this._credentials, this._nonceCount);
				this._nonceCount = authenticationResponse.NonceCount;
			}
			else
			{
				bool preAuth = this._preAuth;
				if (preAuth)
				{
					authenticationResponse = new AuthenticationResponse(this._credentials);
				}
			}
			bool flag3 = authenticationResponse != null;
			if (flag3)
			{
				headers["Authorization"] = authenticationResponse.ToString();
			}
			bool flag4 = this._cookies.Count > 0;
			if (flag4)
			{
				httpRequest.SetCookies(this._cookies);
			}
			return httpRequest;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005F54 File Offset: 0x00004154
		private HttpResponse createHandshakeResponse()
		{
			HttpResponse httpResponse = HttpResponse.CreateWebSocketResponse();
			NameValueCollection headers = httpResponse.Headers;
			headers["Sec-WebSocket-Accept"] = WebSocket.CreateResponseKey(this._base64Key);
			bool flag = this._protocol != null;
			if (flag)
			{
				headers["Sec-WebSocket-Protocol"] = this._protocol;
			}
			bool flag2 = this._extensions != null;
			if (flag2)
			{
				headers["Sec-WebSocket-Extensions"] = this._extensions;
			}
			bool flag3 = this._cookies.Count > 0;
			if (flag3)
			{
				httpResponse.SetCookies(this._cookies);
			}
			return httpResponse;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005FEC File Offset: 0x000041EC
		private bool customCheckHandshakeRequest(WebSocketContext context, out string message)
		{
			message = null;
			bool flag = this._handshakeRequestChecker == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				message = this._handshakeRequestChecker(context);
				result = (message == null);
			}
			return result;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006028 File Offset: 0x00004228
		private MessageEventArgs dequeueFromMessageEventQueue()
		{
			object forMessageEventQueue = this._forMessageEventQueue;
			MessageEventArgs result;
			lock (forMessageEventQueue)
			{
				result = ((this._messageEventQueue.Count > 0) ? this._messageEventQueue.Dequeue() : null);
			}
			return result;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000607C File Offset: 0x0000427C
		private void doHandshake()
		{
			this.setClientStream();
			HttpResponse httpResponse = this.sendHandshakeRequest();
			string message;
			bool flag = !this.checkHandshakeResponse(httpResponse, out message);
			if (flag)
			{
				throw new WebSocketException(CloseStatusCode.ProtocolError, message);
			}
			bool protocolsRequested = this._protocolsRequested;
			if (protocolsRequested)
			{
				this._protocol = httpResponse.Headers["Sec-WebSocket-Protocol"];
			}
			bool extensionsRequested = this._extensionsRequested;
			if (extensionsRequested)
			{
				this.processSecWebSocketExtensionsServerHeader(httpResponse.Headers["Sec-WebSocket-Extensions"]);
			}
			this.processCookies(httpResponse.Cookies);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006104 File Offset: 0x00004304
		private void enqueueToMessageEventQueue(MessageEventArgs e)
		{
			object forMessageEventQueue = this._forMessageEventQueue;
			lock (forMessageEventQueue)
			{
				this._messageEventQueue.Enqueue(e);
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006148 File Offset: 0x00004348
		private void error(string message, Exception exception)
		{
			try
			{
				this.OnError.Emit(this, new ErrorEventArgs(message, exception));
			}
			catch (Exception ex)
			{
				this._logger.Error(ex.Message);
				this._logger.Debug(ex.ToString());
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000061AC File Offset: 0x000043AC
		private void fatal(string message, Exception exception)
		{
			CloseStatusCode code = (exception is WebSocketException) ? ((WebSocketException)exception).Code : CloseStatusCode.Abnormal;
			this.fatal(message, (ushort)code);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000061E0 File Offset: 0x000043E0
		private void fatal(string message, ushort code)
		{
			PayloadData payloadData = new PayloadData(code, message);
			this.close(payloadData, !code.IsReserved(), false, false);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006209 File Offset: 0x00004409
		private void fatal(string message, CloseStatusCode code)
		{
			this.fatal(message, (ushort)code);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006218 File Offset: 0x00004418
		private ClientSslConfiguration getSslConfiguration()
		{
			bool flag = this._sslConfig == null;
			if (flag)
			{
				this._sslConfig = new ClientSslConfiguration(this._uri.DnsSafeHost);
			}
			return this._sslConfig;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006254 File Offset: 0x00004454
		private void init()
		{
			this._compression = CompressionMethod.None;
			this._cookies = new CookieCollection();
			this._forPing = new object();
			this._forSend = new object();
			this._forState = new object();
			this._messageEventQueue = new Queue<MessageEventArgs>();
			this._forMessageEventQueue = ((ICollection)this._messageEventQueue).SyncRoot;
			this._readyState = WebSocketState.Connecting;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000062BC File Offset: 0x000044BC
		private void message()
		{
			MessageEventArgs obj = null;
			object forMessageEventQueue = this._forMessageEventQueue;
			lock (forMessageEventQueue)
			{
				bool flag = this._inMessage || this._messageEventQueue.Count == 0 || this._readyState != WebSocketState.Open;
				if (flag)
				{
					return;
				}
				this._inMessage = true;
				obj = this._messageEventQueue.Dequeue();
			}
			this._message(obj);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006348 File Offset: 0x00004548
		private void messagec(MessageEventArgs e)
		{
			for (;;)
			{
				try
				{
					this.OnMessage.Emit(this, e);
				}
				catch (Exception ex)
				{
					this._logger.Error(ex.ToString());
					this.error("An error has occurred during an OnMessage event.", ex);
				}
				object forMessageEventQueue = this._forMessageEventQueue;
				lock (forMessageEventQueue)
				{
					bool flag = this._messageEventQueue.Count == 0 || this._readyState != WebSocketState.Open;
					if (flag)
					{
						this._inMessage = false;
						break;
					}
					e = this._messageEventQueue.Dequeue();
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006408 File Offset: 0x00004608
		private void messages(MessageEventArgs e)
		{
			try
			{
				this.OnMessage.Emit(this, e);
			}
			catch (Exception ex)
			{
				this._logger.Error(ex.ToString());
				this.error("An error has occurred during an OnMessage event.", ex);
			}
			object forMessageEventQueue = this._forMessageEventQueue;
			lock (forMessageEventQueue)
			{
				bool flag = this._messageEventQueue.Count == 0 || this._readyState != WebSocketState.Open;
				if (flag)
				{
					this._inMessage = false;
					return;
				}
				e = this._messageEventQueue.Dequeue();
			}
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				this.messages(e);
			});
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000064F0 File Offset: 0x000046F0
		private void open()
		{
			this._inMessage = true;
			this.startReceiving();
			try
			{
				this.OnOpen.Emit(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				this._logger.Error(ex.ToString());
				this.error("An error has occurred during the OnOpen event.", ex);
			}
			MessageEventArgs obj = null;
			object forMessageEventQueue = this._forMessageEventQueue;
			lock (forMessageEventQueue)
			{
				bool flag = this._messageEventQueue.Count == 0 || this._readyState != WebSocketState.Open;
				if (flag)
				{
					this._inMessage = false;
					return;
				}
				obj = this._messageEventQueue.Dequeue();
			}
			this._message.BeginInvoke(obj, delegate(IAsyncResult ar)
			{
				this._message.EndInvoke(ar);
			}, null);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000065D8 File Offset: 0x000047D8
		private bool ping(byte[] data)
		{
			bool flag = this._readyState != WebSocketState.Open;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ManualResetEvent pongReceived = this._pongReceived;
				bool flag2 = pongReceived == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					object forPing = this._forPing;
					lock (forPing)
					{
						try
						{
							pongReceived.Reset();
							bool flag3 = !this.send(Fin.Final, Opcode.Ping, data, false);
							if (flag3)
							{
								result = false;
							}
							else
							{
								result = pongReceived.WaitOne(this._waitTime);
							}
						}
						catch (ObjectDisposedException)
						{
							result = false;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000667C File Offset: 0x0000487C
		private bool processCloseFrame(WebSocketFrame frame)
		{
			PayloadData payloadData = frame.PayloadData;
			this.close(payloadData, !payloadData.HasReservedCode, false, true);
			return false;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000066AC File Offset: 0x000048AC
		private void processCookies(CookieCollection cookies)
		{
			bool flag = cookies.Count == 0;
			if (!flag)
			{
				this._cookies.SetOrRemove(cookies);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000066D8 File Offset: 0x000048D8
		private bool processDataFrame(WebSocketFrame frame)
		{
			this.enqueueToMessageEventQueue(frame.IsCompressed ? new MessageEventArgs(frame.Opcode, frame.PayloadData.ApplicationData.Decompress(this._compression)) : new MessageEventArgs(frame));
			return true;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006724 File Offset: 0x00004924
		private bool processFragmentFrame(WebSocketFrame frame)
		{
			bool flag = !this._inContinuation;
			if (flag)
			{
				bool isContinuation = frame.IsContinuation;
				if (isContinuation)
				{
					return true;
				}
				this._fragmentsOpcode = frame.Opcode;
				this._fragmentsCompressed = frame.IsCompressed;
				this._fragmentsBuffer = new MemoryStream();
				this._inContinuation = true;
			}
			this._fragmentsBuffer.WriteBytes(frame.PayloadData.ApplicationData, 1024);
			bool isFinal = frame.IsFinal;
			if (isFinal)
			{
				using (this._fragmentsBuffer)
				{
					byte[] rawData = this._fragmentsCompressed ? this._fragmentsBuffer.DecompressToArray(this._compression) : this._fragmentsBuffer.ToArray();
					this.enqueueToMessageEventQueue(new MessageEventArgs(this._fragmentsOpcode, rawData));
				}
				this._fragmentsBuffer = null;
				this._inContinuation = false;
			}
			return true;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000681C File Offset: 0x00004A1C
		private bool processPingFrame(WebSocketFrame frame)
		{
			this._logger.Trace("A ping was received.");
			WebSocketFrame webSocketFrame = WebSocketFrame.CreatePongFrame(frame.PayloadData, this._client);
			object forState = this._forState;
			lock (forState)
			{
				bool flag = this._readyState != WebSocketState.Open;
				if (flag)
				{
					this._logger.Error("The connection is closing.");
					return true;
				}
				bool flag2 = !this.sendBytes(webSocketFrame.ToArray());
				if (flag2)
				{
					return false;
				}
			}
			this._logger.Trace("A pong to this ping has been sent.");
			bool emitOnPing = this._emitOnPing;
			if (emitOnPing)
			{
				bool client = this._client;
				if (client)
				{
					webSocketFrame.Unmask();
				}
				this.enqueueToMessageEventQueue(new MessageEventArgs(frame));
			}
			return true;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006904 File Offset: 0x00004B04
		private bool processPongFrame(WebSocketFrame frame)
		{
			this._logger.Trace("A pong was received.");
			try
			{
				this._pongReceived.Set();
			}
			catch (NullReferenceException ex)
			{
				this._logger.Error(ex.Message);
				this._logger.Debug(ex.ToString());
				return false;
			}
			catch (ObjectDisposedException ex2)
			{
				this._logger.Error(ex2.Message);
				this._logger.Debug(ex2.ToString());
				return false;
			}
			this._logger.Trace("It has been signaled.");
			return true;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000069C8 File Offset: 0x00004BC8
		private bool processReceivedFrame(WebSocketFrame frame)
		{
			string message;
			bool flag = !this.checkReceivedFrame(frame, out message);
			if (flag)
			{
				throw new WebSocketException(CloseStatusCode.ProtocolError, message);
			}
			frame.Unmask();
			return frame.IsFragment ? this.processFragmentFrame(frame) : (frame.IsData ? this.processDataFrame(frame) : (frame.IsPing ? this.processPingFrame(frame) : (frame.IsPong ? this.processPongFrame(frame) : (frame.IsClose ? this.processCloseFrame(frame) : this.processUnsupportedFrame(frame)))));
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006A5C File Offset: 0x00004C5C
		private void processSecWebSocketExtensionsClientHeader(string value)
		{
			bool flag = value == null;
			if (!flag)
			{
				StringBuilder stringBuilder = new StringBuilder(80);
				bool flag2 = false;
				foreach (string text in value.SplitHeaderValue(new char[]
				{
					','
				}))
				{
					string text2 = text.Trim();
					bool flag3 = text2.Length == 0;
					if (!flag3)
					{
						bool flag4 = !flag2;
						if (flag4)
						{
							bool flag5 = text2.IsCompressionExtension(CompressionMethod.Deflate);
							if (flag5)
							{
								this._compression = CompressionMethod.Deflate;
								stringBuilder.AppendFormat("{0}, ", this._compression.ToExtensionString(new string[]
								{
									"client_no_context_takeover",
									"server_no_context_takeover"
								}));
								flag2 = true;
							}
						}
					}
				}
				int length = stringBuilder.Length;
				bool flag6 = length <= 2;
				if (!flag6)
				{
					stringBuilder.Length = length - 2;
					this._extensions = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006B6C File Offset: 0x00004D6C
		private void processSecWebSocketExtensionsServerHeader(string value)
		{
			bool flag = value == null;
			if (flag)
			{
				this._compression = CompressionMethod.None;
			}
			else
			{
				this._extensions = value;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006B94 File Offset: 0x00004D94
		private void processSecWebSocketProtocolClientHeader(IEnumerable<string> values)
		{
			bool flag = values.Contains((string val) => val == this._protocol);
			if (!flag)
			{
				this._protocol = null;
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006BC4 File Offset: 0x00004DC4
		private bool processUnsupportedFrame(WebSocketFrame frame)
		{
			this._logger.Fatal("An unsupported frame:" + frame.PrintToString(false));
			this.fatal("There is no way to handle it.", CloseStatusCode.PolicyViolation);
			return false;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006C08 File Offset: 0x00004E08
		private void refuseHandshake(CloseStatusCode code, string reason)
		{
			this._readyState = WebSocketState.Closing;
			HttpResponse response = this.createHandshakeFailureResponse(HttpStatusCode.BadRequest);
			this.sendHttpResponse(response);
			this.releaseServerResources();
			this._readyState = WebSocketState.Closed;
			CloseEventArgs e = new CloseEventArgs(code, reason);
			try
			{
				this.OnClose.Emit(this, e);
			}
			catch (Exception ex)
			{
				this._logger.Error(ex.Message);
				this._logger.Debug(ex.ToString());
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006C9C File Offset: 0x00004E9C
		private void releaseClientResources()
		{
			bool flag = this._stream != null;
			if (flag)
			{
				this._stream.Dispose();
				this._stream = null;
			}
			bool flag2 = this._tcpClient != null;
			if (flag2)
			{
				this._tcpClient.Close();
				this._tcpClient = null;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006CF0 File Offset: 0x00004EF0
		private void releaseCommonResources()
		{
			bool flag = this._fragmentsBuffer != null;
			if (flag)
			{
				this._fragmentsBuffer.Dispose();
				this._fragmentsBuffer = null;
				this._inContinuation = false;
			}
			bool flag2 = this._pongReceived != null;
			if (flag2)
			{
				this._pongReceived.Close();
				this._pongReceived = null;
			}
			bool flag3 = this._receivingExited != null;
			if (flag3)
			{
				this._receivingExited.Close();
				this._receivingExited = null;
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006D6C File Offset: 0x00004F6C
		private void releaseResources()
		{
			bool client = this._client;
			if (client)
			{
				this.releaseClientResources();
			}
			else
			{
				this.releaseServerResources();
			}
			this.releaseCommonResources();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006D9C File Offset: 0x00004F9C
		private void releaseServerResources()
		{
			bool flag = this._closeContext == null;
			if (!flag)
			{
				this._closeContext();
				this._closeContext = null;
				this._stream = null;
				this._context = null;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006DDC File Offset: 0x00004FDC
		private bool send(Opcode opcode, Stream stream)
		{
			object forSend = this._forSend;
			bool result;
			lock (forSend)
			{
				Stream stream2 = stream;
				bool flag = false;
				bool flag2 = false;
				try
				{
					bool flag3 = this._compression > CompressionMethod.None;
					if (flag3)
					{
						stream = stream.Compress(this._compression);
						flag = true;
					}
					flag2 = this.send(opcode, stream, flag);
					bool flag4 = !flag2;
					if (flag4)
					{
						this.error("A send has been interrupted.", null);
					}
				}
				catch (Exception ex)
				{
					this._logger.Error(ex.ToString());
					this.error("An error has occurred during a send.", ex);
				}
				finally
				{
					bool flag5 = flag;
					if (flag5)
					{
						stream.Dispose();
					}
					stream2.Dispose();
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006EC0 File Offset: 0x000050C0
		private bool send(Opcode opcode, Stream stream, bool compressed)
		{
			long length = stream.Length;
			bool flag = length == 0L;
			bool result;
			if (flag)
			{
				result = this.send(Fin.Final, opcode, WebSocket.EmptyBytes, false);
			}
			else
			{
				long num = length / (long)WebSocket.FragmentLength;
				int num2 = (int)(length % (long)WebSocket.FragmentLength);
				bool flag2 = num == 0L;
				if (flag2)
				{
					byte[] array = new byte[num2];
					result = (stream.Read(array, 0, num2) == num2 && this.send(Fin.Final, opcode, array, compressed));
				}
				else
				{
					bool flag3 = num == 1L && num2 == 0;
					if (flag3)
					{
						byte[] array = new byte[WebSocket.FragmentLength];
						result = (stream.Read(array, 0, WebSocket.FragmentLength) == WebSocket.FragmentLength && this.send(Fin.Final, opcode, array, compressed));
					}
					else
					{
						byte[] array = new byte[WebSocket.FragmentLength];
						bool flag4 = stream.Read(array, 0, WebSocket.FragmentLength) == WebSocket.FragmentLength && this.send(Fin.More, opcode, array, compressed);
						bool flag5 = !flag4;
						if (flag5)
						{
							result = false;
						}
						else
						{
							long num3 = (num2 == 0) ? (num - 2L) : (num - 1L);
							for (long num4 = 0L; num4 < num3; num4 += 1L)
							{
								flag4 = (stream.Read(array, 0, WebSocket.FragmentLength) == WebSocket.FragmentLength && this.send(Fin.More, Opcode.Cont, array, false));
								bool flag6 = !flag4;
								if (flag6)
								{
									return false;
								}
							}
							bool flag7 = num2 == 0;
							if (flag7)
							{
								num2 = WebSocket.FragmentLength;
							}
							else
							{
								array = new byte[num2];
							}
							result = (stream.Read(array, 0, num2) == num2 && this.send(Fin.Final, Opcode.Cont, array, false));
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000705C File Offset: 0x0000525C
		private bool send(Fin fin, Opcode opcode, byte[] data, bool compressed)
		{
			object forState = this._forState;
			bool result;
			lock (forState)
			{
				bool flag = this._readyState != WebSocketState.Open;
				if (flag)
				{
					this._logger.Error("The connection is closing.");
					result = false;
				}
				else
				{
					WebSocketFrame webSocketFrame = new WebSocketFrame(fin, opcode, data, compressed, this._client);
					result = this.sendBytes(webSocketFrame.ToArray());
				}
			}
			return result;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000070DC File Offset: 0x000052DC
		private void sendAsync(Opcode opcode, Stream stream, Action<bool> completed)
		{
			Func<Opcode, Stream, bool> sender = new Func<Opcode, Stream, bool>(this.send);
			sender.BeginInvoke(opcode, stream, delegate(IAsyncResult ar)
			{
				try
				{
					bool obj = sender.EndInvoke(ar);
					bool flag = completed != null;
					if (flag)
					{
						completed(obj);
					}
				}
				catch (Exception ex)
				{
					this._logger.Error(ex.ToString());
					this.error("An error has occurred during the callback for an async send.", ex);
				}
			}, null);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000712C File Offset: 0x0000532C
		private bool sendBytes(byte[] bytes)
		{
			try
			{
				this._stream.Write(bytes, 0, bytes.Length);
			}
			catch (Exception ex)
			{
				this._logger.Error(ex.Message);
				this._logger.Debug(ex.ToString());
				return false;
			}
			return true;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007194 File Offset: 0x00005394
		private HttpResponse sendHandshakeRequest()
		{
			HttpRequest httpRequest = this.createHandshakeRequest();
			HttpResponse httpResponse = this.sendHttpRequest(httpRequest, 90000);
			bool isUnauthorized = httpResponse.IsUnauthorized;
			if (isUnauthorized)
			{
				string text = httpResponse.Headers["WWW-Authenticate"];
				this._logger.Warn(string.Format("Received an authentication requirement for '{0}'.", text));
				bool flag = text.IsNullOrEmpty();
				if (flag)
				{
					this._logger.Error("No authentication challenge is specified.");
					return httpResponse;
				}
				this._authChallenge = AuthenticationChallenge.Parse(text);
				bool flag2 = this._authChallenge == null;
				if (flag2)
				{
					this._logger.Error("An invalid authentication challenge is specified.");
					return httpResponse;
				}
				bool flag3 = this._credentials != null && (!this._preAuth || this._authChallenge.Scheme == AuthenticationSchemes.Digest);
				if (flag3)
				{
					bool hasConnectionClose = httpResponse.HasConnectionClose;
					if (hasConnectionClose)
					{
						this.releaseClientResources();
						this.setClientStream();
					}
					AuthenticationResponse authenticationResponse = new AuthenticationResponse(this._authChallenge, this._credentials, this._nonceCount);
					this._nonceCount = authenticationResponse.NonceCount;
					httpRequest.Headers["Authorization"] = authenticationResponse.ToString();
					httpResponse = this.sendHttpRequest(httpRequest, 15000);
				}
			}
			bool isRedirect = httpResponse.IsRedirect;
			if (isRedirect)
			{
				string text2 = httpResponse.Headers["Location"];
				this._logger.Warn(string.Format("Received a redirection to '{0}'.", text2));
				bool enableRedirection = this._enableRedirection;
				if (enableRedirection)
				{
					bool flag4 = text2.IsNullOrEmpty();
					if (flag4)
					{
						this._logger.Error("No url to redirect is located.");
						return httpResponse;
					}
					Uri uri;
					string str;
					bool flag5 = !text2.TryCreateWebSocketUri(out uri, out str);
					if (flag5)
					{
						this._logger.Error("An invalid url to redirect is located: " + str);
						return httpResponse;
					}
					this.releaseClientResources();
					this._uri = uri;
					this._secure = (uri.Scheme == "wss");
					this.setClientStream();
					return this.sendHandshakeRequest();
				}
			}
			return httpResponse;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000073C8 File Offset: 0x000055C8
		private HttpResponse sendHttpRequest(HttpRequest request, int millisecondsTimeout)
		{
			this._logger.Debug("A request to the server:\n" + request.ToString());
			HttpResponse response = request.GetResponse(this._stream, millisecondsTimeout);
			this._logger.Debug("A response to this request:\n" + response.ToString());
			return response;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00007428 File Offset: 0x00005628
		private bool sendHttpResponse(HttpResponse response)
		{
			this._logger.Debug(string.Format("A response to {0}:\n{1}", this._context.UserEndPoint, response));
			return this.sendBytes(response.ToByteArray());
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000746C File Offset: 0x0000566C
		private void sendProxyConnectRequest()
		{
			HttpRequest httpRequest = HttpRequest.CreateConnectRequest(this._uri);
			HttpResponse httpResponse = this.sendHttpRequest(httpRequest, 90000);
			bool isProxyAuthenticationRequired = httpResponse.IsProxyAuthenticationRequired;
			if (isProxyAuthenticationRequired)
			{
				string text = httpResponse.Headers["Proxy-Authenticate"];
				this._logger.Warn(string.Format("Received a proxy authentication requirement for '{0}'.", text));
				bool flag = text.IsNullOrEmpty();
				if (flag)
				{
					throw new WebSocketException("No proxy authentication challenge is specified.");
				}
				AuthenticationChallenge authenticationChallenge = AuthenticationChallenge.Parse(text);
				bool flag2 = authenticationChallenge == null;
				if (flag2)
				{
					throw new WebSocketException("An invalid proxy authentication challenge is specified.");
				}
				bool flag3 = this._proxyCredentials != null;
				if (flag3)
				{
					bool hasConnectionClose = httpResponse.HasConnectionClose;
					if (hasConnectionClose)
					{
						this.releaseClientResources();
						this._tcpClient = new TcpClient(this._proxyUri.DnsSafeHost, this._proxyUri.Port);
						this._stream = this._tcpClient.GetStream();
					}
					AuthenticationResponse authenticationResponse = new AuthenticationResponse(authenticationChallenge, this._proxyCredentials, 0U);
					httpRequest.Headers["Proxy-Authorization"] = authenticationResponse.ToString();
					httpResponse = this.sendHttpRequest(httpRequest, 15000);
				}
				bool isProxyAuthenticationRequired2 = httpResponse.IsProxyAuthenticationRequired;
				if (isProxyAuthenticationRequired2)
				{
					throw new WebSocketException("A proxy authentication is required.");
				}
			}
			bool flag4 = httpResponse.StatusCode[0] != '2';
			if (flag4)
			{
				throw new WebSocketException("The proxy has failed a connection to the requested host and port.");
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000075CC File Offset: 0x000057CC
		private void setClientStream()
		{
			bool flag = this._proxyUri != null;
			if (flag)
			{
				this._tcpClient = new TcpClient(this._proxyUri.DnsSafeHost, this._proxyUri.Port);
				this._stream = this._tcpClient.GetStream();
				this.sendProxyConnectRequest();
			}
			else
			{
				this._tcpClient = new TcpClient(this._uri.DnsSafeHost, this._uri.Port);
				this._stream = this._tcpClient.GetStream();
			}
			bool secure = this._secure;
			if (secure)
			{
				ClientSslConfiguration sslConfiguration = this.getSslConfiguration();
				string targetHost = sslConfiguration.TargetHost;
				bool flag2 = targetHost != this._uri.DnsSafeHost;
				if (flag2)
				{
					throw new WebSocketException(CloseStatusCode.TlsHandshakeFailure, "An invalid host name is specified.");
				}
				try
				{
					SslStream sslStream = new SslStream(this._stream, false, sslConfiguration.ServerCertificateValidationCallback, sslConfiguration.ClientCertificateSelectionCallback);
					sslStream.AuthenticateAsClient(targetHost, sslConfiguration.ClientCertificates, sslConfiguration.EnabledSslProtocols, sslConfiguration.CheckCertificateRevocation);
					this._stream = sslStream;
				}
				catch (Exception innerException)
				{
					throw new WebSocketException(CloseStatusCode.TlsHandshakeFailure, innerException);
				}
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007704 File Offset: 0x00005904
		private void startReceiving()
		{
			bool flag = this._messageEventQueue.Count > 0;
			if (flag)
			{
				this._messageEventQueue.Clear();
			}
			this._pongReceived = new ManualResetEvent(false);
			this._receivingExited = new ManualResetEvent(false);
			Action receive = null;
			Action<WebSocketFrame> <>9__1;
			Action<Exception> <>9__2;
			receive = delegate()
			{
				Stream stream = this._stream;
				bool unmask = false;
				Action<WebSocketFrame> completed;
				if ((completed = <>9__1) == null)
				{
					completed = (<>9__1 = delegate(WebSocketFrame frame)
					{
						bool flag2 = !this.processReceivedFrame(frame) || this._readyState == WebSocketState.Closed;
						if (flag2)
						{
							ManualResetEvent receivingExited = this._receivingExited;
							bool flag3 = receivingExited != null;
							if (flag3)
							{
								receivingExited.Set();
							}
						}
						else
						{
							receive();
							bool flag4 = this._inMessage || !this.HasMessage || this._readyState != WebSocketState.Open;
							if (!flag4)
							{
								this.message();
							}
						}
					});
				}
				Action<Exception> error;
				if ((error = <>9__2) == null)
				{
					error = (<>9__2 = delegate(Exception ex)
					{
						this._logger.Fatal(ex.ToString());
						this.fatal("An exception has occurred while receiving.", ex);
					});
				}
				WebSocketFrame.ReadFrameAsync(stream, unmask, completed, error);
			};
			receive();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000777C File Offset: 0x0000597C
		private bool validateSecWebSocketAcceptHeader(string value)
		{
			return value != null && value == WebSocket.CreateResponseKey(this._base64Key);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000077A8 File Offset: 0x000059A8
		private bool validateSecWebSocketExtensionsServerHeader(string value)
		{
			bool flag = value == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = value.Length == 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !this._extensionsRequested;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = this._compression > CompressionMethod.None;
						foreach (string text in value.SplitHeaderValue(new char[]
						{
							','
						}))
						{
							string text2 = text.Trim();
							bool flag5 = flag4 && text2.IsCompressionExtension(this._compression);
							if (!flag5)
							{
								return false;
							}
							bool flag6 = !text2.Contains("server_no_context_takeover");
							if (flag6)
							{
								this._logger.Error("The server hasn't sent back 'server_no_context_takeover'.");
								return false;
							}
							bool flag7 = !text2.Contains("client_no_context_takeover");
							if (flag7)
							{
								this._logger.Warn("The server hasn't sent back 'client_no_context_takeover'.");
							}
							string method = this._compression.ToExtensionString(new string[0]);
							bool flag8 = text2.SplitHeaderValue(new char[]
							{
								';'
							}).Contains(delegate(string t)
							{
								t = t.Trim();
								return t != method && t != "server_no_context_takeover" && t != "client_no_context_takeover";
							});
							bool flag9 = flag8;
							if (flag9)
							{
								return false;
							}
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000792C File Offset: 0x00005B2C
		private bool validateSecWebSocketProtocolServerHeader(string value)
		{
			bool flag = value == null;
			bool result;
			if (flag)
			{
				result = !this._protocolsRequested;
			}
			else
			{
				bool flag2 = value.Length == 0;
				result = (!flag2 && this._protocolsRequested && this._protocols.Contains((string p) => p == value));
			}
			return result;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000799C File Offset: 0x00005B9C
		private bool validateSecWebSocketVersionServerHeader(string value)
		{
			return value == null || value == "13";
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000079BF File Offset: 0x00005BBF
		internal void Close(HttpResponse response)
		{
			this._readyState = WebSocketState.Closing;
			this.sendHttpResponse(response);
			this.releaseServerResources();
			this._readyState = WebSocketState.Closed;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000079E3 File Offset: 0x00005BE3
		internal void Close(HttpStatusCode code)
		{
			this.Close(this.createHandshakeFailureResponse(code));
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000079F4 File Offset: 0x00005BF4
		internal void Close(PayloadData payloadData, byte[] frameAsBytes)
		{
			object forState = this._forState;
			lock (forState)
			{
				bool flag = this._readyState == WebSocketState.Closing;
				if (flag)
				{
					this._logger.Info("The closing is already in progress.");
					return;
				}
				bool flag2 = this._readyState == WebSocketState.Closed;
				if (flag2)
				{
					this._logger.Info("The connection has already been closed.");
					return;
				}
				this._readyState = WebSocketState.Closing;
			}
			this._logger.Trace("Begin closing the connection.");
			bool flag3 = frameAsBytes != null && this.sendBytes(frameAsBytes);
			bool flag4 = flag3 && this._receivingExited != null && this._receivingExited.WaitOne(this._waitTime);
			bool flag5 = flag3 && flag4;
			this._logger.Debug(string.Format("Was clean?: {0}\n  sent: {1}\n  received: {2}", flag5, flag3, flag4));
			this.releaseServerResources();
			this.releaseCommonResources();
			this._logger.Trace("End closing the connection.");
			this._readyState = WebSocketState.Closed;
			CloseEventArgs closeEventArgs = new CloseEventArgs(payloadData);
			closeEventArgs.WasClean = flag5;
			try
			{
				this.OnClose.Emit(this, closeEventArgs);
			}
			catch (Exception ex)
			{
				this._logger.Error(ex.ToString());
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007B70 File Offset: 0x00005D70
		internal static string CreateBase64Key()
		{
			byte[] array = new byte[16];
			WebSocket.RandomNumber.GetBytes(array);
			return Convert.ToBase64String(array);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00007B9C File Offset: 0x00005D9C
		internal static string CreateResponseKey(string base64Key)
		{
			StringBuilder stringBuilder = new StringBuilder(base64Key, 64);
			stringBuilder.Append("258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
			SHA1 sha = new SHA1CryptoServiceProvider();
			byte[] inArray = sha.ComputeHash(stringBuilder.ToString().UTF8Encode());
			return Convert.ToBase64String(inArray);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007BE4 File Offset: 0x00005DE4
		internal void InternalAccept()
		{
			try
			{
				bool flag = !this.acceptHandshake();
				if (flag)
				{
					return;
				}
			}
			catch (Exception ex)
			{
				this._logger.Fatal(ex.Message);
				this._logger.Debug(ex.ToString());
				string message = "An exception has occurred while attempting to accept.";
				this.fatal(message, ex);
				return;
			}
			this._readyState = WebSocketState.Open;
			this.open();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007C60 File Offset: 0x00005E60
		internal bool Ping(byte[] frameAsBytes, TimeSpan timeout)
		{
			bool flag = this._readyState != WebSocketState.Open;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ManualResetEvent pongReceived = this._pongReceived;
				bool flag2 = pongReceived == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					object forPing = this._forPing;
					lock (forPing)
					{
						try
						{
							pongReceived.Reset();
							object forState = this._forState;
							lock (forState)
							{
								bool flag3 = this._readyState != WebSocketState.Open;
								if (flag3)
								{
									return false;
								}
								bool flag4 = !this.sendBytes(frameAsBytes);
								if (flag4)
								{
									return false;
								}
							}
							result = pongReceived.WaitOne(timeout);
						}
						catch (ObjectDisposedException)
						{
							result = false;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007D40 File Offset: 0x00005F40
		internal void Send(Opcode opcode, byte[] data, Dictionary<CompressionMethod, byte[]> cache)
		{
			object forSend = this._forSend;
			lock (forSend)
			{
				object forState = this._forState;
				lock (forState)
				{
					bool flag = this._readyState != WebSocketState.Open;
					if (flag)
					{
						this._logger.Error("The connection is closing.");
					}
					else
					{
						byte[] array;
						bool flag2 = !cache.TryGetValue(this._compression, out array);
						if (flag2)
						{
							array = new WebSocketFrame(Fin.Final, opcode, data.Compress(this._compression), this._compression > CompressionMethod.None, false).ToArray();
							cache.Add(this._compression, array);
						}
						this.sendBytes(array);
					}
				}
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007E18 File Offset: 0x00006018
		internal void Send(Opcode opcode, Stream stream, Dictionary<CompressionMethod, Stream> cache)
		{
			object forSend = this._forSend;
			lock (forSend)
			{
				Stream stream2;
				bool flag = !cache.TryGetValue(this._compression, out stream2);
				if (flag)
				{
					stream2 = stream.Compress(this._compression);
					cache.Add(this._compression, stream2);
				}
				else
				{
					stream2.Position = 0L;
				}
				this.send(opcode, stream2, this._compression > CompressionMethod.None);
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007EA4 File Offset: 0x000060A4
		public void Accept()
		{
			bool client = this._client;
			if (client)
			{
				string message = "This instance is a client.";
				throw new InvalidOperationException(message);
			}
			bool flag = this._readyState == WebSocketState.Closing;
			if (flag)
			{
				string message2 = "The close process is in progress.";
				throw new InvalidOperationException(message2);
			}
			bool flag2 = this._readyState == WebSocketState.Closed;
			if (flag2)
			{
				string message3 = "The connection has already been closed.";
				throw new InvalidOperationException(message3);
			}
			bool flag3 = this.accept();
			if (flag3)
			{
				this.open();
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00007F1C File Offset: 0x0000611C
		public void AcceptAsync()
		{
			bool client = this._client;
			if (client)
			{
				string message = "This instance is a client.";
				throw new InvalidOperationException(message);
			}
			bool flag = this._readyState == WebSocketState.Closing;
			if (flag)
			{
				string message2 = "The close process is in progress.";
				throw new InvalidOperationException(message2);
			}
			bool flag2 = this._readyState == WebSocketState.Closed;
			if (flag2)
			{
				string message3 = "The connection has already been closed.";
				throw new InvalidOperationException(message3);
			}
			Func<bool> acceptor = new Func<bool>(this.accept);
			acceptor.BeginInvoke(delegate(IAsyncResult ar)
			{
				bool flag3 = acceptor.EndInvoke(ar);
				if (flag3)
				{
					this.open();
				}
			}, null);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007FBA File Offset: 0x000061BA
		public void Close()
		{
			this.close(1005, string.Empty);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007FD0 File Offset: 0x000061D0
		public void Close(ushort code)
		{
			bool flag = !code.IsCloseStatusCode();
			if (flag)
			{
				string message = "Less than 1000 or greater than 4999.";
				throw new ArgumentOutOfRangeException("code", message);
			}
			bool flag2 = this._client && code == 1011;
			if (flag2)
			{
				string message2 = "1011 cannot be used.";
				throw new ArgumentException(message2, "code");
			}
			bool flag3 = !this._client && code == 1010;
			if (flag3)
			{
				string message3 = "1010 cannot be used.";
				throw new ArgumentException(message3, "code");
			}
			this.close(code, string.Empty);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00008064 File Offset: 0x00006264
		public void Close(CloseStatusCode code)
		{
			bool flag = this._client && code == CloseStatusCode.ServerError;
			if (flag)
			{
				string message = "ServerError cannot be used.";
				throw new ArgumentException(message, "code");
			}
			bool flag2 = !this._client && code == CloseStatusCode.MandatoryExtension;
			if (flag2)
			{
				string message2 = "MandatoryExtension cannot be used.";
				throw new ArgumentException(message2, "code");
			}
			this.close((ushort)code, string.Empty);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000080D4 File Offset: 0x000062D4
		public void Close(ushort code, string reason)
		{
			bool flag = !code.IsCloseStatusCode();
			if (flag)
			{
				string message = "Less than 1000 or greater than 4999.";
				throw new ArgumentOutOfRangeException("code", message);
			}
			bool flag2 = this._client && code == 1011;
			if (flag2)
			{
				string message2 = "1011 cannot be used.";
				throw new ArgumentException(message2, "code");
			}
			bool flag3 = !this._client && code == 1010;
			if (flag3)
			{
				string message3 = "1010 cannot be used.";
				throw new ArgumentException(message3, "code");
			}
			bool flag4 = reason.IsNullOrEmpty();
			if (flag4)
			{
				this.close(code, string.Empty);
			}
			else
			{
				bool flag5 = code == 1005;
				if (flag5)
				{
					string message4 = "1005 cannot be used.";
					throw new ArgumentException(message4, "code");
				}
				byte[] array;
				bool flag6 = !reason.TryGetUTF8EncodedBytes(out array);
				if (flag6)
				{
					string message5 = "It could not be UTF-8-encoded.";
					throw new ArgumentException(message5, "reason");
				}
				bool flag7 = array.Length > 123;
				if (flag7)
				{
					string message6 = "Its size is greater than 123 bytes.";
					throw new ArgumentOutOfRangeException("reason", message6);
				}
				this.close(code, reason);
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000081EC File Offset: 0x000063EC
		public void Close(CloseStatusCode code, string reason)
		{
			bool flag = this._client && code == CloseStatusCode.ServerError;
			if (flag)
			{
				string message = "ServerError cannot be used.";
				throw new ArgumentException(message, "code");
			}
			bool flag2 = !this._client && code == CloseStatusCode.MandatoryExtension;
			if (flag2)
			{
				string message2 = "MandatoryExtension cannot be used.";
				throw new ArgumentException(message2, "code");
			}
			bool flag3 = reason.IsNullOrEmpty();
			if (flag3)
			{
				this.close((ushort)code, string.Empty);
			}
			else
			{
				bool flag4 = code == CloseStatusCode.NoStatus;
				if (flag4)
				{
					string message3 = "NoStatus cannot be used.";
					throw new ArgumentException(message3, "code");
				}
				byte[] array;
				bool flag5 = !reason.TryGetUTF8EncodedBytes(out array);
				if (flag5)
				{
					string message4 = "It could not be UTF-8-encoded.";
					throw new ArgumentException(message4, "reason");
				}
				bool flag6 = array.Length > 123;
				if (flag6)
				{
					string message5 = "Its size is greater than 123 bytes.";
					throw new ArgumentOutOfRangeException("reason", message5);
				}
				this.close((ushort)code, reason);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000082E0 File Offset: 0x000064E0
		public void CloseAsync()
		{
			this.closeAsync(1005, string.Empty);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000082F4 File Offset: 0x000064F4
		public void CloseAsync(ushort code)
		{
			bool flag = !code.IsCloseStatusCode();
			if (flag)
			{
				string message = "Less than 1000 or greater than 4999.";
				throw new ArgumentOutOfRangeException("code", message);
			}
			bool flag2 = this._client && code == 1011;
			if (flag2)
			{
				string message2 = "1011 cannot be used.";
				throw new ArgumentException(message2, "code");
			}
			bool flag3 = !this._client && code == 1010;
			if (flag3)
			{
				string message3 = "1010 cannot be used.";
				throw new ArgumentException(message3, "code");
			}
			this.closeAsync(code, string.Empty);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00008388 File Offset: 0x00006588
		public void CloseAsync(CloseStatusCode code)
		{
			bool flag = this._client && code == CloseStatusCode.ServerError;
			if (flag)
			{
				string message = "ServerError cannot be used.";
				throw new ArgumentException(message, "code");
			}
			bool flag2 = !this._client && code == CloseStatusCode.MandatoryExtension;
			if (flag2)
			{
				string message2 = "MandatoryExtension cannot be used.";
				throw new ArgumentException(message2, "code");
			}
			this.closeAsync((ushort)code, string.Empty);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000083F8 File Offset: 0x000065F8
		public void CloseAsync(ushort code, string reason)
		{
			bool flag = !code.IsCloseStatusCode();
			if (flag)
			{
				string message = "Less than 1000 or greater than 4999.";
				throw new ArgumentOutOfRangeException("code", message);
			}
			bool flag2 = this._client && code == 1011;
			if (flag2)
			{
				string message2 = "1011 cannot be used.";
				throw new ArgumentException(message2, "code");
			}
			bool flag3 = !this._client && code == 1010;
			if (flag3)
			{
				string message3 = "1010 cannot be used.";
				throw new ArgumentException(message3, "code");
			}
			bool flag4 = reason.IsNullOrEmpty();
			if (flag4)
			{
				this.closeAsync(code, string.Empty);
			}
			else
			{
				bool flag5 = code == 1005;
				if (flag5)
				{
					string message4 = "1005 cannot be used.";
					throw new ArgumentException(message4, "code");
				}
				byte[] array;
				bool flag6 = !reason.TryGetUTF8EncodedBytes(out array);
				if (flag6)
				{
					string message5 = "It could not be UTF-8-encoded.";
					throw new ArgumentException(message5, "reason");
				}
				bool flag7 = array.Length > 123;
				if (flag7)
				{
					string message6 = "Its size is greater than 123 bytes.";
					throw new ArgumentOutOfRangeException("reason", message6);
				}
				this.closeAsync(code, reason);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00008510 File Offset: 0x00006710
		public void CloseAsync(CloseStatusCode code, string reason)
		{
			bool flag = this._client && code == CloseStatusCode.ServerError;
			if (flag)
			{
				string message = "ServerError cannot be used.";
				throw new ArgumentException(message, "code");
			}
			bool flag2 = !this._client && code == CloseStatusCode.MandatoryExtension;
			if (flag2)
			{
				string message2 = "MandatoryExtension cannot be used.";
				throw new ArgumentException(message2, "code");
			}
			bool flag3 = reason.IsNullOrEmpty();
			if (flag3)
			{
				this.closeAsync((ushort)code, string.Empty);
			}
			else
			{
				bool flag4 = code == CloseStatusCode.NoStatus;
				if (flag4)
				{
					string message3 = "NoStatus cannot be used.";
					throw new ArgumentException(message3, "code");
				}
				byte[] array;
				bool flag5 = !reason.TryGetUTF8EncodedBytes(out array);
				if (flag5)
				{
					string message4 = "It could not be UTF-8-encoded.";
					throw new ArgumentException(message4, "reason");
				}
				bool flag6 = array.Length > 123;
				if (flag6)
				{
					string message5 = "Its size is greater than 123 bytes.";
					throw new ArgumentOutOfRangeException("reason", message5);
				}
				this.closeAsync((ushort)code, reason);
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00008604 File Offset: 0x00006804
		public void Connect()
		{
			bool flag = !this._client;
			if (flag)
			{
				string message = "This instance is not a client.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = this._readyState == WebSocketState.Closing;
			if (flag2)
			{
				string message2 = "The close process is in progress.";
				throw new InvalidOperationException(message2);
			}
			bool flag3 = this._retryCountForConnect > WebSocket._maxRetryCountForConnect;
			if (flag3)
			{
				string message3 = "A series of reconnecting has failed.";
				throw new InvalidOperationException(message3);
			}
			bool flag4 = this.connect();
			if (flag4)
			{
				this.open();
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00008680 File Offset: 0x00006880
		public void ConnectAsync()
		{
			bool flag = !this._client;
			if (flag)
			{
				string message = "This instance is not a client.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = this._readyState == WebSocketState.Closing;
			if (flag2)
			{
				string message2 = "The close process is in progress.";
				throw new InvalidOperationException(message2);
			}
			bool flag3 = this._retryCountForConnect > WebSocket._maxRetryCountForConnect;
			if (flag3)
			{
				string message3 = "A series of reconnecting has failed.";
				throw new InvalidOperationException(message3);
			}
			Func<bool> connector = new Func<bool>(this.connect);
			connector.BeginInvoke(delegate(IAsyncResult ar)
			{
				bool flag4 = connector.EndInvoke(ar);
				if (flag4)
				{
					this.open();
				}
			}, null);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008724 File Offset: 0x00006924
		public bool Ping()
		{
			return this.ping(WebSocket.EmptyBytes);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00008744 File Offset: 0x00006944
		public bool Ping(string message)
		{
			bool flag = message.IsNullOrEmpty();
			bool result;
			if (flag)
			{
				result = this.ping(WebSocket.EmptyBytes);
			}
			else
			{
				byte[] array;
				bool flag2 = !message.TryGetUTF8EncodedBytes(out array);
				if (flag2)
				{
					string message2 = "It could not be UTF-8-encoded.";
					throw new ArgumentException(message2, "message");
				}
				bool flag3 = array.Length > 125;
				if (flag3)
				{
					string message3 = "Its size is greater than 125 bytes.";
					throw new ArgumentOutOfRangeException("message", message3);
				}
				result = this.ping(array);
			}
			return result;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000087BC File Offset: 0x000069BC
		public void Send(byte[] data)
		{
			bool flag = this._readyState != WebSocketState.Open;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = data == null;
			if (flag2)
			{
				throw new ArgumentNullException("data");
			}
			this.send(Opcode.Binary, new MemoryStream(data));
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000880C File Offset: 0x00006A0C
		public void Send(FileInfo fileInfo)
		{
			bool flag = this._readyState != WebSocketState.Open;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = fileInfo == null;
			if (flag2)
			{
				throw new ArgumentNullException("fileInfo");
			}
			bool flag3 = !fileInfo.Exists;
			if (flag3)
			{
				string message2 = "The file does not exist.";
				throw new ArgumentException(message2, "fileInfo");
			}
			FileStream stream;
			bool flag4 = !fileInfo.TryOpenRead(out stream);
			if (flag4)
			{
				string message3 = "The file could not be opened.";
				throw new ArgumentException(message3, "fileInfo");
			}
			this.send(Opcode.Binary, stream);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000088A0 File Offset: 0x00006AA0
		public void Send(string data)
		{
			bool flag = this._readyState != WebSocketState.Open;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = data == null;
			if (flag2)
			{
				throw new ArgumentNullException("data");
			}
			byte[] buffer;
			bool flag3 = !data.TryGetUTF8EncodedBytes(out buffer);
			if (flag3)
			{
				string message2 = "It could not be UTF-8-encoded.";
				throw new ArgumentException(message2, "data");
			}
			this.send(Opcode.Text, new MemoryStream(buffer));
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008918 File Offset: 0x00006B18
		public void Send(Stream stream, int length)
		{
			bool flag = this._readyState != WebSocketState.Open;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = stream == null;
			if (flag2)
			{
				throw new ArgumentNullException("stream");
			}
			bool flag3 = !stream.CanRead;
			if (flag3)
			{
				string message2 = "It cannot be read.";
				throw new ArgumentException(message2, "stream");
			}
			bool flag4 = length < 1;
			if (flag4)
			{
				string message3 = "Less than 1.";
				throw new ArgumentException(message3, "length");
			}
			byte[] array = stream.ReadBytes(length);
			int num = array.Length;
			bool flag5 = num == 0;
			if (flag5)
			{
				string message4 = "No data could be read from it.";
				throw new ArgumentException(message4, "stream");
			}
			bool flag6 = num < length;
			if (flag6)
			{
				this._logger.Warn(string.Format("Only {0} byte(s) of data could be read from the stream.", num));
			}
			this.send(Opcode.Binary, new MemoryStream(array));
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008A04 File Offset: 0x00006C04
		public void SendAsync(byte[] data, Action<bool> completed)
		{
			bool flag = this._readyState != WebSocketState.Open;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = data == null;
			if (flag2)
			{
				throw new ArgumentNullException("data");
			}
			this.sendAsync(Opcode.Binary, new MemoryStream(data), completed);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008A54 File Offset: 0x00006C54
		public void SendAsync(FileInfo fileInfo, Action<bool> completed)
		{
			bool flag = this._readyState != WebSocketState.Open;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = fileInfo == null;
			if (flag2)
			{
				throw new ArgumentNullException("fileInfo");
			}
			bool flag3 = !fileInfo.Exists;
			if (flag3)
			{
				string message2 = "The file does not exist.";
				throw new ArgumentException(message2, "fileInfo");
			}
			FileStream stream;
			bool flag4 = !fileInfo.TryOpenRead(out stream);
			if (flag4)
			{
				string message3 = "The file could not be opened.";
				throw new ArgumentException(message3, "fileInfo");
			}
			this.sendAsync(Opcode.Binary, stream, completed);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008AEC File Offset: 0x00006CEC
		public void SendAsync(string data, Action<bool> completed)
		{
			bool flag = this._readyState != WebSocketState.Open;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = data == null;
			if (flag2)
			{
				throw new ArgumentNullException("data");
			}
			byte[] buffer;
			bool flag3 = !data.TryGetUTF8EncodedBytes(out buffer);
			if (flag3)
			{
				string message2 = "It could not be UTF-8-encoded.";
				throw new ArgumentException(message2, "data");
			}
			this.sendAsync(Opcode.Text, new MemoryStream(buffer), completed);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008B64 File Offset: 0x00006D64
		public void SendAsync(Stream stream, int length, Action<bool> completed)
		{
			bool flag = this._readyState != WebSocketState.Open;
			if (flag)
			{
				string message = "The current state of the connection is not Open.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = stream == null;
			if (flag2)
			{
				throw new ArgumentNullException("stream");
			}
			bool flag3 = !stream.CanRead;
			if (flag3)
			{
				string message2 = "It cannot be read.";
				throw new ArgumentException(message2, "stream");
			}
			bool flag4 = length < 1;
			if (flag4)
			{
				string message3 = "Less than 1.";
				throw new ArgumentException(message3, "length");
			}
			byte[] array = stream.ReadBytes(length);
			int num = array.Length;
			bool flag5 = num == 0;
			if (flag5)
			{
				string message4 = "No data could be read from it.";
				throw new ArgumentException(message4, "stream");
			}
			bool flag6 = num < length;
			if (flag6)
			{
				this._logger.Warn(string.Format("Only {0} byte(s) of data could be read from the stream.", num));
			}
			this.sendAsync(Opcode.Binary, new MemoryStream(array), completed);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00008C50 File Offset: 0x00006E50
		public void SetCookie(Cookie cookie)
		{
			string message = null;
			bool flag = !this._client;
			if (flag)
			{
				message = "This instance is not a client.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = cookie == null;
			if (flag2)
			{
				throw new ArgumentNullException("cookie");
			}
			bool flag3 = !this.canSet(out message);
			if (flag3)
			{
				this._logger.Warn(message);
			}
			else
			{
				object forState = this._forState;
				lock (forState)
				{
					bool flag4 = !this.canSet(out message);
					if (flag4)
					{
						this._logger.Warn(message);
					}
					else
					{
						object syncRoot = this._cookies.SyncRoot;
						lock (syncRoot)
						{
							this._cookies.SetOrRemove(cookie);
						}
					}
				}
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00008D38 File Offset: 0x00006F38
		public void SetCredentials(string username, string password, bool preAuth)
		{
			string message = null;
			bool flag = !this._client;
			if (flag)
			{
				message = "This instance is not a client.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = !username.IsNullOrEmpty();
			if (flag2)
			{
				bool flag3 = username.Contains(new char[]
				{
					':'
				}) || !username.IsText();
				if (flag3)
				{
					message = "It contains an invalid character.";
					throw new ArgumentException(message, "username");
				}
			}
			bool flag4 = !password.IsNullOrEmpty();
			if (flag4)
			{
				bool flag5 = !password.IsText();
				if (flag5)
				{
					message = "It contains an invalid character.";
					throw new ArgumentException(message, "password");
				}
			}
			bool flag6 = !this.canSet(out message);
			if (flag6)
			{
				this._logger.Warn(message);
			}
			else
			{
				object forState = this._forState;
				lock (forState)
				{
					bool flag7 = !this.canSet(out message);
					if (flag7)
					{
						this._logger.Warn(message);
					}
					else
					{
						bool flag8 = username.IsNullOrEmpty();
						if (flag8)
						{
							this._credentials = null;
							this._preAuth = false;
						}
						else
						{
							this._credentials = new NetworkCredential(username, password, this._uri.PathAndQuery, new string[0]);
							this._preAuth = preAuth;
						}
					}
				}
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00008E94 File Offset: 0x00007094
		public void SetProxy(string url, string username, string password)
		{
			string message = null;
			bool flag = !this._client;
			if (flag)
			{
				message = "This instance is not a client.";
				throw new InvalidOperationException(message);
			}
			Uri uri = null;
			bool flag2 = !url.IsNullOrEmpty();
			if (flag2)
			{
				bool flag3 = !Uri.TryCreate(url, UriKind.Absolute, out uri);
				if (flag3)
				{
					message = "Not an absolute URI string.";
					throw new ArgumentException(message, "url");
				}
				bool flag4 = uri.Scheme != "http";
				if (flag4)
				{
					message = "The scheme part is not http.";
					throw new ArgumentException(message, "url");
				}
				bool flag5 = uri.Segments.Length > 1;
				if (flag5)
				{
					message = "It includes the path segments.";
					throw new ArgumentException(message, "url");
				}
			}
			bool flag6 = !username.IsNullOrEmpty();
			if (flag6)
			{
				bool flag7 = username.Contains(new char[]
				{
					':'
				}) || !username.IsText();
				if (flag7)
				{
					message = "It contains an invalid character.";
					throw new ArgumentException(message, "username");
				}
			}
			bool flag8 = !password.IsNullOrEmpty();
			if (flag8)
			{
				bool flag9 = !password.IsText();
				if (flag9)
				{
					message = "It contains an invalid character.";
					throw new ArgumentException(message, "password");
				}
			}
			bool flag10 = !this.canSet(out message);
			if (flag10)
			{
				this._logger.Warn(message);
			}
			else
			{
				object forState = this._forState;
				lock (forState)
				{
					bool flag11 = !this.canSet(out message);
					if (flag11)
					{
						this._logger.Warn(message);
					}
					else
					{
						bool flag12 = url.IsNullOrEmpty();
						if (flag12)
						{
							this._proxyUri = null;
							this._proxyCredentials = null;
						}
						else
						{
							this._proxyUri = uri;
							this._proxyCredentials = ((!username.IsNullOrEmpty()) ? new NetworkCredential(username, password, string.Format("{0}:{1}", this._uri.DnsSafeHost, this._uri.Port), new string[0]) : null);
						}
					}
				}
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000909C File Offset: 0x0000729C
		void IDisposable.Dispose()
		{
			this.close(1001, string.Empty);
		}

		// Token: 0x0400000F RID: 15
		private AuthenticationChallenge _authChallenge;

		// Token: 0x04000010 RID: 16
		private string _base64Key;

		// Token: 0x04000011 RID: 17
		private bool _client;

		// Token: 0x04000012 RID: 18
		private Action _closeContext;

		// Token: 0x04000013 RID: 19
		private CompressionMethod _compression;

		// Token: 0x04000014 RID: 20
		private WebSocketContext _context;

		// Token: 0x04000015 RID: 21
		private CookieCollection _cookies;

		// Token: 0x04000016 RID: 22
		private NetworkCredential _credentials;

		// Token: 0x04000017 RID: 23
		private bool _emitOnPing;

		// Token: 0x04000018 RID: 24
		private bool _enableRedirection;

		// Token: 0x04000019 RID: 25
		private string _extensions;

		// Token: 0x0400001A RID: 26
		private bool _extensionsRequested;

		// Token: 0x0400001B RID: 27
		private object _forMessageEventQueue;

		// Token: 0x0400001C RID: 28
		private object _forPing;

		// Token: 0x0400001D RID: 29
		private object _forSend;

		// Token: 0x0400001E RID: 30
		private object _forState;

		// Token: 0x0400001F RID: 31
		private MemoryStream _fragmentsBuffer;

		// Token: 0x04000020 RID: 32
		private bool _fragmentsCompressed;

		// Token: 0x04000021 RID: 33
		private Opcode _fragmentsOpcode;

		// Token: 0x04000022 RID: 34
		private const string _guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

		// Token: 0x04000023 RID: 35
		private Func<WebSocketContext, string> _handshakeRequestChecker;

		// Token: 0x04000024 RID: 36
		private bool _ignoreExtensions;

		// Token: 0x04000025 RID: 37
		private bool _inContinuation;

		// Token: 0x04000026 RID: 38
		private volatile bool _inMessage;

		// Token: 0x04000027 RID: 39
		private volatile Logger _logger;

		// Token: 0x04000028 RID: 40
		private static readonly int _maxRetryCountForConnect = 10;

		// Token: 0x04000029 RID: 41
		private Action<MessageEventArgs> _message;

		// Token: 0x0400002A RID: 42
		private Queue<MessageEventArgs> _messageEventQueue;

		// Token: 0x0400002B RID: 43
		private uint _nonceCount;

		// Token: 0x0400002C RID: 44
		private string _origin;

		// Token: 0x0400002D RID: 45
		private ManualResetEvent _pongReceived;

		// Token: 0x0400002E RID: 46
		private bool _preAuth;

		// Token: 0x0400002F RID: 47
		private string _protocol;

		// Token: 0x04000030 RID: 48
		private string[] _protocols;

		// Token: 0x04000031 RID: 49
		private bool _protocolsRequested;

		// Token: 0x04000032 RID: 50
		private NetworkCredential _proxyCredentials;

		// Token: 0x04000033 RID: 51
		private Uri _proxyUri;

		// Token: 0x04000034 RID: 52
		private volatile WebSocketState _readyState;

		// Token: 0x04000035 RID: 53
		private ManualResetEvent _receivingExited;

		// Token: 0x04000036 RID: 54
		private int _retryCountForConnect;

		// Token: 0x04000037 RID: 55
		private bool _secure;

		// Token: 0x04000038 RID: 56
		private ClientSslConfiguration _sslConfig;

		// Token: 0x04000039 RID: 57
		private Stream _stream;

		// Token: 0x0400003A RID: 58
		private TcpClient _tcpClient;

		// Token: 0x0400003B RID: 59
		private Uri _uri;

		// Token: 0x0400003C RID: 60
		private const string _version = "13";

		// Token: 0x0400003D RID: 61
		private TimeSpan _waitTime;

		// Token: 0x0400003E RID: 62
		internal static readonly byte[] EmptyBytes = new byte[0];

		// Token: 0x0400003F RID: 63
		internal static readonly int FragmentLength = 1016;

		// Token: 0x04000040 RID: 64
		internal static readonly RandomNumberGenerator RandomNumber = new RNGCryptoServiceProvider();
	}
}
