using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server
{
	// Token: 0x02000046 RID: 70
	public class HttpServer
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x0001A018 File Offset: 0x00018218
		public HttpServer()
		{
			this.init("*", IPAddress.Any, 80, false);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001A036 File Offset: 0x00018236
		public HttpServer(int port) : this(port, port == 443)
		{
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001A04C File Offset: 0x0001824C
		public HttpServer(string url)
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
			Uri uri;
			string message;
			bool flag3 = !HttpServer.tryCreateUri(url, out uri, out message);
			if (flag3)
			{
				throw new ArgumentException(message, "url");
			}
			string dnsSafeHost = uri.GetDnsSafeHost(true);
			IPAddress ipaddress = dnsSafeHost.ToIPAddress();
			bool flag4 = ipaddress == null;
			if (flag4)
			{
				message = "The host part could not be converted to an IP address.";
				throw new ArgumentException(message, "url");
			}
			bool flag5 = !ipaddress.IsLocal();
			if (flag5)
			{
				message = "The IP address of the host is not a local IP address.";
				throw new ArgumentException(message, "url");
			}
			this.init(dnsSafeHost, ipaddress, uri.Port, uri.Scheme == "https");
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001A124 File Offset: 0x00018324
		public HttpServer(int port, bool secure)
		{
			bool flag = !port.IsPortNumber();
			if (flag)
			{
				string message = "Less than 1 or greater than 65535.";
				throw new ArgumentOutOfRangeException("port", message);
			}
			this.init("*", IPAddress.Any, port, secure);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001A16C File Offset: 0x0001836C
		public HttpServer(IPAddress address, int port) : this(address, port, port == 443)
		{
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001A180 File Offset: 0x00018380
		public HttpServer(IPAddress address, int port, bool secure)
		{
			bool flag = address == null;
			if (flag)
			{
				throw new ArgumentNullException("address");
			}
			bool flag2 = !address.IsLocal();
			if (flag2)
			{
				throw new ArgumentException("Not a local IP address.", "address");
			}
			bool flag3 = !port.IsPortNumber();
			if (flag3)
			{
				string message = "Less than 1 or greater than 65535.";
				throw new ArgumentOutOfRangeException("port", message);
			}
			this.init(address.ToString(true), address, port, secure);
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x0001A1F8 File Offset: 0x000183F8
		public IPAddress Address
		{
			get
			{
				return this._address;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0001A210 File Offset: 0x00018410
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x0001A230 File Offset: 0x00018430
		public WebSocketSharp.Net.AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				return this._listener.AuthenticationSchemes;
			}
			set
			{
				string message;
				bool flag = !this.canSet(out message);
				if (flag)
				{
					this._log.Warn(message);
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag2 = !this.canSet(out message);
						if (flag2)
						{
							this._log.Warn(message);
						}
						else
						{
							this._listener.AuthenticationSchemes = value;
						}
					}
				}
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0001A2B4 File Offset: 0x000184B4
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x0001A2CC File Offset: 0x000184CC
		public string DocumentRootPath
		{
			get
			{
				return this._docRootPath;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					throw new ArgumentNullException("value");
				}
				bool flag2 = value.Length == 0;
				if (flag2)
				{
					throw new ArgumentException("An empty string.", "value");
				}
				value = value.TrimSlashOrBackslashFromEnd();
				string text = null;
				try
				{
					text = Path.GetFullPath(value);
				}
				catch (Exception innerException)
				{
					throw new ArgumentException("An invalid path string.", "value", innerException);
				}
				bool flag3 = value == "/";
				if (flag3)
				{
					throw new ArgumentException("An absolute root.", "value");
				}
				bool flag4 = value == "\\";
				if (flag4)
				{
					throw new ArgumentException("An absolute root.", "value");
				}
				bool flag5 = value.Length == 2 && value[1] == ':';
				if (flag5)
				{
					throw new ArgumentException("An absolute root.", "value");
				}
				bool flag6 = text == "/";
				if (flag6)
				{
					throw new ArgumentException("An absolute root.", "value");
				}
				text = text.TrimSlashOrBackslashFromEnd();
				bool flag7 = text.Length == 2 && text[1] == ':';
				if (flag7)
				{
					throw new ArgumentException("An absolute root.", "value");
				}
				string message;
				bool flag8 = !this.canSet(out message);
				if (flag8)
				{
					this._log.Warn(message);
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag9 = !this.canSet(out message);
						if (flag9)
						{
							this._log.Warn(message);
						}
						else
						{
							this._docRootPath = value;
						}
					}
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0001A47C File Offset: 0x0001867C
		public bool IsListening
		{
			get
			{
				return this._state == ServerState.Start;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001A49C File Offset: 0x0001869C
		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001A4B4 File Offset: 0x000186B4
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x0001A4D1 File Offset: 0x000186D1
		public bool KeepClean
		{
			get
			{
				return this._services.KeepClean;
			}
			set
			{
				this._services.KeepClean = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001A4E4 File Offset: 0x000186E4
		public Logger Log
		{
			get
			{
				return this._log;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x0001A4FC File Offset: 0x000186FC
		public int Port
		{
			get
			{
				return this._port;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0001A514 File Offset: 0x00018714
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0001A534 File Offset: 0x00018734
		public string Realm
		{
			get
			{
				return this._listener.Realm;
			}
			set
			{
				string message;
				bool flag = !this.canSet(out message);
				if (flag)
				{
					this._log.Warn(message);
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag2 = !this.canSet(out message);
						if (flag2)
						{
							this._log.Warn(message);
						}
						else
						{
							this._listener.Realm = value;
						}
					}
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0001A5B8 File Offset: 0x000187B8
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x0001A5D8 File Offset: 0x000187D8
		public bool ReuseAddress
		{
			get
			{
				return this._listener.ReuseAddress;
			}
			set
			{
				string message;
				bool flag = !this.canSet(out message);
				if (flag)
				{
					this._log.Warn(message);
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag2 = !this.canSet(out message);
						if (flag2)
						{
							this._log.Warn(message);
						}
						else
						{
							this._listener.ReuseAddress = value;
						}
					}
				}
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0001A65C File Offset: 0x0001885C
		public ServerSslConfiguration SslConfiguration
		{
			get
			{
				bool flag = !this._secure;
				if (flag)
				{
					string message = "This instance does not provide secure connections.";
					throw new InvalidOperationException(message);
				}
				return this._listener.SslConfiguration;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0001A694 File Offset: 0x00018894
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x0001A6B4 File Offset: 0x000188B4
		public Func<IIdentity, WebSocketSharp.Net.NetworkCredential> UserCredentialsFinder
		{
			get
			{
				return this._listener.UserCredentialsFinder;
			}
			set
			{
				string message;
				bool flag = !this.canSet(out message);
				if (flag)
				{
					this._log.Warn(message);
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag2 = !this.canSet(out message);
						if (flag2)
						{
							this._log.Warn(message);
						}
						else
						{
							this._listener.UserCredentialsFinder = value;
						}
					}
				}
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0001A738 File Offset: 0x00018938
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x0001A755 File Offset: 0x00018955
		public TimeSpan WaitTime
		{
			get
			{
				return this._services.WaitTime;
			}
			set
			{
				this._services.WaitTime = value;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0001A768 File Offset: 0x00018968
		public WebSocketServiceManager WebSocketServices
		{
			get
			{
				return this._services;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060004CB RID: 1227 RVA: 0x0001A780 File Offset: 0x00018980
		// (remove) Token: 0x060004CC RID: 1228 RVA: 0x0001A7B8 File Offset: 0x000189B8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<HttpRequestEventArgs> OnConnect;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060004CD RID: 1229 RVA: 0x0001A7F0 File Offset: 0x000189F0
		// (remove) Token: 0x060004CE RID: 1230 RVA: 0x0001A828 File Offset: 0x00018A28
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<HttpRequestEventArgs> OnDelete;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060004CF RID: 1231 RVA: 0x0001A860 File Offset: 0x00018A60
		// (remove) Token: 0x060004D0 RID: 1232 RVA: 0x0001A898 File Offset: 0x00018A98
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<HttpRequestEventArgs> OnGet;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060004D1 RID: 1233 RVA: 0x0001A8D0 File Offset: 0x00018AD0
		// (remove) Token: 0x060004D2 RID: 1234 RVA: 0x0001A908 File Offset: 0x00018B08
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<HttpRequestEventArgs> OnHead;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060004D3 RID: 1235 RVA: 0x0001A940 File Offset: 0x00018B40
		// (remove) Token: 0x060004D4 RID: 1236 RVA: 0x0001A978 File Offset: 0x00018B78
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<HttpRequestEventArgs> OnOptions;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060004D5 RID: 1237 RVA: 0x0001A9B0 File Offset: 0x00018BB0
		// (remove) Token: 0x060004D6 RID: 1238 RVA: 0x0001A9E8 File Offset: 0x00018BE8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<HttpRequestEventArgs> OnPost;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060004D7 RID: 1239 RVA: 0x0001AA20 File Offset: 0x00018C20
		// (remove) Token: 0x060004D8 RID: 1240 RVA: 0x0001AA58 File Offset: 0x00018C58
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<HttpRequestEventArgs> OnPut;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060004D9 RID: 1241 RVA: 0x0001AA90 File Offset: 0x00018C90
		// (remove) Token: 0x060004DA RID: 1242 RVA: 0x0001AAC8 File Offset: 0x00018CC8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<HttpRequestEventArgs> OnTrace;

		// Token: 0x060004DB RID: 1243 RVA: 0x0001AB00 File Offset: 0x00018D00
		private void abort()
		{
			object sync = this._sync;
			lock (sync)
			{
				bool flag = this._state != ServerState.Start;
				if (flag)
				{
					return;
				}
				this._state = ServerState.ShuttingDown;
			}
			try
			{
				try
				{
					this._services.Stop(1006, string.Empty);
				}
				finally
				{
					this._listener.Abort();
				}
			}
			catch
			{
			}
			this._state = ServerState.Stop;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001ABAC File Offset: 0x00018DAC
		private bool canSet(out string message)
		{
			message = null;
			bool flag = this._state == ServerState.Start;
			bool result;
			if (flag)
			{
				message = "The server has already started.";
				result = false;
			}
			else
			{
				bool flag2 = this._state == ServerState.ShuttingDown;
				if (flag2)
				{
					message = "The server is shutting down.";
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001ABF8 File Offset: 0x00018DF8
		private bool checkCertificate(out string message)
		{
			message = null;
			bool flag = this._listener.SslConfiguration.ServerCertificate != null;
			string certificateFolderPath = this._listener.CertificateFolderPath;
			bool flag2 = EndPointListener.CertificateExists(this._port, certificateFolderPath);
			bool flag3 = !flag && !flag2;
			bool result;
			if (flag3)
			{
				message = "There is no server certificate for secure connection.";
				result = false;
			}
			else
			{
				bool flag4 = flag && flag2;
				if (flag4)
				{
					this._log.Warn("The server certificate associated with the port is used.");
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001AC70 File Offset: 0x00018E70
		private string createFilePath(string childPath)
		{
			childPath = childPath.TrimStart(new char[]
			{
				'/',
				'\\'
			});
			return new StringBuilder(this._docRootPath, 32).AppendFormat("/{0}", childPath).ToString().Replace('\\', '/');
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001ACC0 File Offset: 0x00018EC0
		private static WebSocketSharp.Net.HttpListener createListener(string hostname, int port, bool secure)
		{
			WebSocketSharp.Net.HttpListener httpListener = new WebSocketSharp.Net.HttpListener();
			string arg = secure ? "https" : "http";
			string uriPrefix = string.Format("{0}://{1}:{2}/", arg, hostname, port);
			httpListener.Prefixes.Add(uriPrefix);
			return httpListener;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001AD0C File Offset: 0x00018F0C
		private void init(string hostname, IPAddress address, int port, bool secure)
		{
			this._hostname = hostname;
			this._address = address;
			this._port = port;
			this._secure = secure;
			this._docRootPath = "./Public";
			this._listener = HttpServer.createListener(this._hostname, this._port, this._secure);
			this._log = this._listener.Log;
			this._services = new WebSocketServiceManager(this._log);
			this._sync = new object();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001AD8C File Offset: 0x00018F8C
		private void processRequest(WebSocketSharp.Net.HttpListenerContext context)
		{
			string httpMethod = context.Request.HttpMethod;
			EventHandler<HttpRequestEventArgs> eventHandler = (httpMethod == "GET") ? this.OnGet : ((httpMethod == "HEAD") ? this.OnHead : ((httpMethod == "POST") ? this.OnPost : ((httpMethod == "PUT") ? this.OnPut : ((httpMethod == "DELETE") ? this.OnDelete : ((httpMethod == "CONNECT") ? this.OnConnect : ((httpMethod == "OPTIONS") ? this.OnOptions : ((httpMethod == "TRACE") ? this.OnTrace : null)))))));
			bool flag = eventHandler != null;
			if (flag)
			{
				eventHandler(this, new HttpRequestEventArgs(context, this._docRootPath));
			}
			else
			{
				context.Response.StatusCode = 501;
			}
			context.Response.Close();
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001AE94 File Offset: 0x00019094
		private void processRequest(HttpListenerWebSocketContext context)
		{
			Uri requestUri = context.RequestUri;
			bool flag = requestUri == null;
			if (flag)
			{
				context.Close(WebSocketSharp.Net.HttpStatusCode.BadRequest);
			}
			else
			{
				string text = requestUri.AbsolutePath;
				bool flag2 = text.IndexOfAny(new char[]
				{
					'%',
					'+'
				}) > -1;
				if (flag2)
				{
					text = HttpUtility.UrlDecode(text, Encoding.UTF8);
				}
				WebSocketServiceHost webSocketServiceHost;
				bool flag3 = !this._services.InternalTryGetServiceHost(text, out webSocketServiceHost);
				if (flag3)
				{
					context.Close(WebSocketSharp.Net.HttpStatusCode.NotImplemented);
				}
				else
				{
					webSocketServiceHost.StartSession(context);
				}
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001AF24 File Offset: 0x00019124
		private void receiveRequest()
		{
			for (;;)
			{
				WebSocketSharp.Net.HttpListenerContext ctx = null;
				try
				{
					ctx = this._listener.GetContext();
					ThreadPool.QueueUserWorkItem(delegate(object state)
					{
						try
						{
							bool flag3 = ctx.Request.IsUpgradeRequest("websocket");
							if (flag3)
							{
								this.processRequest(ctx.AcceptWebSocket(null));
							}
							else
							{
								this.processRequest(ctx);
							}
						}
						catch (Exception ex2)
						{
							this._log.Fatal(ex2.Message);
							this._log.Debug(ex2.ToString());
							ctx.Connection.Close(true);
						}
					});
				}
				catch (WebSocketSharp.Net.HttpListenerException)
				{
					this._log.Info("The underlying listener is stopped.");
					break;
				}
				catch (InvalidOperationException)
				{
					this._log.Info("The underlying listener is stopped.");
					break;
				}
				catch (Exception ex)
				{
					this._log.Fatal(ex.Message);
					this._log.Debug(ex.ToString());
					bool flag = ctx != null;
					if (flag)
					{
						ctx.Connection.Close(true);
					}
					break;
				}
			}
			bool flag2 = this._state != ServerState.ShuttingDown;
			if (flag2)
			{
				this.abort();
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001B030 File Offset: 0x00019230
		private void start()
		{
			bool flag = this._state == ServerState.Start;
			if (flag)
			{
				this._log.Info("The server has already started.");
			}
			else
			{
				bool flag2 = this._state == ServerState.ShuttingDown;
				if (flag2)
				{
					this._log.Warn("The server is shutting down.");
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag3 = this._state == ServerState.Start;
						if (flag3)
						{
							this._log.Info("The server has already started.");
						}
						else
						{
							bool flag4 = this._state == ServerState.ShuttingDown;
							if (flag4)
							{
								this._log.Warn("The server is shutting down.");
							}
							else
							{
								this._services.Start();
								try
								{
									this.startReceiving();
								}
								catch
								{
									this._services.Stop(1011, string.Empty);
									throw;
								}
								this._state = ServerState.Start;
							}
						}
					}
				}
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001B144 File Offset: 0x00019344
		private void startReceiving()
		{
			try
			{
				this._listener.Start();
			}
			catch (Exception innerException)
			{
				string message = "The underlying listener has failed to start.";
				throw new InvalidOperationException(message, innerException);
			}
			this._receiveThread = new Thread(new ThreadStart(this.receiveRequest));
			this._receiveThread.IsBackground = true;
			this._receiveThread.Start();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001B1B4 File Offset: 0x000193B4
		private void stop(ushort code, string reason)
		{
			bool flag = this._state == ServerState.Ready;
			if (flag)
			{
				this._log.Info("The server is not started.");
			}
			else
			{
				bool flag2 = this._state == ServerState.ShuttingDown;
				if (flag2)
				{
					this._log.Info("The server is shutting down.");
				}
				else
				{
					bool flag3 = this._state == ServerState.Stop;
					if (flag3)
					{
						this._log.Info("The server has already stopped.");
					}
					else
					{
						object sync = this._sync;
						lock (sync)
						{
							bool flag4 = this._state == ServerState.ShuttingDown;
							if (flag4)
							{
								this._log.Info("The server is shutting down.");
								return;
							}
							bool flag5 = this._state == ServerState.Stop;
							if (flag5)
							{
								this._log.Info("The server has already stopped.");
								return;
							}
							this._state = ServerState.ShuttingDown;
						}
						try
						{
							bool flag6 = false;
							try
							{
								this._services.Stop(code, reason);
							}
							catch
							{
								flag6 = true;
								throw;
							}
							finally
							{
								try
								{
									this.stopReceiving(5000);
								}
								catch
								{
									bool flag7 = !flag6;
									if (flag7)
									{
										throw;
									}
								}
							}
						}
						finally
						{
							this._state = ServerState.Stop;
						}
					}
				}
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001B338 File Offset: 0x00019538
		private void stopReceiving(int millisecondsTimeout)
		{
			this._listener.Stop();
			this._receiveThread.Join(millisecondsTimeout);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001B354 File Offset: 0x00019554
		private static bool tryCreateUri(string uriString, out Uri result, out string message)
		{
			result = null;
			message = null;
			Uri uri = uriString.ToUri();
			bool flag = uri == null;
			bool result2;
			if (flag)
			{
				message = "An invalid URI string.";
				result2 = false;
			}
			else
			{
				bool flag2 = !uri.IsAbsoluteUri;
				if (flag2)
				{
					message = "A relative URI.";
					result2 = false;
				}
				else
				{
					string scheme = uri.Scheme;
					bool flag3 = !(scheme == "http") && !(scheme == "https");
					if (flag3)
					{
						message = "The scheme part is not 'http' or 'https'.";
						result2 = false;
					}
					else
					{
						bool flag4 = uri.PathAndQuery != "/";
						if (flag4)
						{
							message = "It includes either or both path and query components.";
							result2 = false;
						}
						else
						{
							bool flag5 = uri.Fragment.Length > 0;
							if (flag5)
							{
								message = "It includes the fragment component.";
								result2 = false;
							}
							else
							{
								bool flag6 = uri.Port == 0;
								if (flag6)
								{
									message = "The port part is zero.";
									result2 = false;
								}
								else
								{
									result = uri;
									result2 = true;
								}
							}
						}
					}
				}
			}
			return result2;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001B444 File Offset: 0x00019644
		[Obsolete("This method will be removed. Use added one instead.")]
		public void AddWebSocketService<TBehavior>(string path, Func<TBehavior> creator) where TBehavior : WebSocketBehavior
		{
			bool flag = path == null;
			if (flag)
			{
				throw new ArgumentNullException("path");
			}
			bool flag2 = creator == null;
			if (flag2)
			{
				throw new ArgumentNullException("creator");
			}
			bool flag3 = path.Length == 0;
			if (flag3)
			{
				throw new ArgumentException("An empty string.", "path");
			}
			bool flag4 = path[0] != '/';
			if (flag4)
			{
				throw new ArgumentException("Not an absolute path.", "path");
			}
			bool flag5 = path.IndexOfAny(new char[]
			{
				'?',
				'#'
			}) > -1;
			if (flag5)
			{
				string message = "It includes either or both query and fragment components.";
				throw new ArgumentException(message, "path");
			}
			this._services.Add<TBehavior>(path, creator);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001B4F9 File Offset: 0x000196F9
		public void AddWebSocketService<TBehaviorWithNew>(string path) where TBehaviorWithNew : WebSocketBehavior, new()
		{
			this._services.AddService<TBehaviorWithNew>(path, null);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001B50A File Offset: 0x0001970A
		public void AddWebSocketService<TBehaviorWithNew>(string path, Action<TBehaviorWithNew> initializer) where TBehaviorWithNew : WebSocketBehavior, new()
		{
			this._services.AddService<TBehaviorWithNew>(path, initializer);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001B51C File Offset: 0x0001971C
		[Obsolete("This method will be removed.")]
		public byte[] GetFile(string path)
		{
			bool flag = path == null;
			if (flag)
			{
				throw new ArgumentNullException("path");
			}
			bool flag2 = path.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("An empty string.", "path");
			}
			bool flag3 = path.IndexOf("..") > -1;
			if (flag3)
			{
				throw new ArgumentException("It contains '..'.", "path");
			}
			path = this.createFilePath(path);
			return File.Exists(path) ? File.ReadAllBytes(path) : null;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001B59C File Offset: 0x0001979C
		public bool RemoveWebSocketService(string path)
		{
			return this._services.RemoveService(path);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001B5BC File Offset: 0x000197BC
		public void Start()
		{
			bool secure = this._secure;
			if (secure)
			{
				string message;
				bool flag = !this.checkCertificate(out message);
				if (flag)
				{
					throw new InvalidOperationException(message);
				}
			}
			this.start();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001B5F3 File Offset: 0x000197F3
		public void Stop()
		{
			this.stop(1001, string.Empty);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001B608 File Offset: 0x00019808
		[Obsolete("This method will be removed.")]
		public void Stop(ushort code, string reason)
		{
			bool flag = !code.IsCloseStatusCode();
			if (flag)
			{
				string message = "Less than 1000 or greater than 4999.";
				throw new ArgumentOutOfRangeException("code", message);
			}
			bool flag2 = code == 1010;
			if (flag2)
			{
				string message2 = "1010 cannot be used.";
				throw new ArgumentException(message2, "code");
			}
			bool flag3 = !reason.IsNullOrEmpty();
			if (flag3)
			{
				bool flag4 = code == 1005;
				if (flag4)
				{
					string message3 = "1005 cannot be used.";
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
			}
			this.stop(code, reason);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001B6DC File Offset: 0x000198DC
		[Obsolete("This method will be removed.")]
		public void Stop(CloseStatusCode code, string reason)
		{
			bool flag = code == CloseStatusCode.MandatoryExtension;
			if (flag)
			{
				string message = "MandatoryExtension cannot be used.";
				throw new ArgumentException(message, "code");
			}
			bool flag2 = !reason.IsNullOrEmpty();
			if (flag2)
			{
				bool flag3 = code == CloseStatusCode.NoStatus;
				if (flag3)
				{
					string message2 = "NoStatus cannot be used.";
					throw new ArgumentException(message2, "code");
				}
				byte[] array;
				bool flag4 = !reason.TryGetUTF8EncodedBytes(out array);
				if (flag4)
				{
					string message3 = "It could not be UTF-8-encoded.";
					throw new ArgumentException(message3, "reason");
				}
				bool flag5 = array.Length > 123;
				if (flag5)
				{
					string message4 = "Its size is greater than 123 bytes.";
					throw new ArgumentOutOfRangeException("reason", message4);
				}
			}
			this.stop((ushort)code, reason);
		}

		// Token: 0x0400022A RID: 554
		private IPAddress _address;

		// Token: 0x0400022B RID: 555
		private string _docRootPath;

		// Token: 0x0400022C RID: 556
		private string _hostname;

		// Token: 0x0400022D RID: 557
		private WebSocketSharp.Net.HttpListener _listener;

		// Token: 0x0400022E RID: 558
		private Logger _log;

		// Token: 0x0400022F RID: 559
		private int _port;

		// Token: 0x04000230 RID: 560
		private Thread _receiveThread;

		// Token: 0x04000231 RID: 561
		private bool _secure;

		// Token: 0x04000232 RID: 562
		private WebSocketServiceManager _services;

		// Token: 0x04000233 RID: 563
		private volatile ServerState _state;

		// Token: 0x04000234 RID: 564
		private object _sync;
	}
}
