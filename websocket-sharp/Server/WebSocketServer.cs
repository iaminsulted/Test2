using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server
{
	// Token: 0x02000045 RID: 69
	public class WebSocketServer
	{
		// Token: 0x0600047E RID: 1150 RVA: 0x00018E9C File Offset: 0x0001709C
		public WebSocketServer()
		{
			IPAddress any = IPAddress.Any;
			this.init(any.ToString(), any, 80, false);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00018EC8 File Offset: 0x000170C8
		public WebSocketServer(int port) : this(port, port == 443)
		{
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00018EDC File Offset: 0x000170DC
		public WebSocketServer(string url)
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
			bool flag3 = !WebSocketServer.tryCreateUri(url, out uri, out message);
			if (flag3)
			{
				throw new ArgumentException(message, "url");
			}
			string dnsSafeHost = uri.DnsSafeHost;
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
			this.init(dnsSafeHost, ipaddress, uri.Port, uri.Scheme == "wss");
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00018FB0 File Offset: 0x000171B0
		public WebSocketServer(int port, bool secure)
		{
			bool flag = !port.IsPortNumber();
			if (flag)
			{
				string message = "Less than 1 or greater than 65535.";
				throw new ArgumentOutOfRangeException("port", message);
			}
			IPAddress any = IPAddress.Any;
			this.init(any.ToString(), any, port, secure);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00018FFB File Offset: 0x000171FB
		public WebSocketServer(IPAddress address, int port) : this(address, port, port == 443)
		{
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00019010 File Offset: 0x00017210
		public WebSocketServer(IPAddress address, int port, bool secure)
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
			this.init(address.ToString(), address, port, secure);
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00019088 File Offset: 0x00017288
		public IPAddress Address
		{
			get
			{
				return this._address;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x000190A0 File Offset: 0x000172A0
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x000190B8 File Offset: 0x000172B8
		public bool AllowForwardedRequest
		{
			get
			{
				return this._allowForwardedRequest;
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
							this._allowForwardedRequest = value;
						}
					}
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00019138 File Offset: 0x00017338
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x00019150 File Offset: 0x00017350
		public WebSocketSharp.Net.AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				return this._authSchemes;
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
							this._authSchemes = value;
						}
					}
				}
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x000191D0 File Offset: 0x000173D0
		public bool IsListening
		{
			get
			{
				return this._state == ServerState.Start;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x000191F0 File Offset: 0x000173F0
		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x00019208 File Offset: 0x00017408
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x00019225 File Offset: 0x00017425
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

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00019238 File Offset: 0x00017438
		public Logger Log
		{
			get
			{
				return this._log;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00019250 File Offset: 0x00017450
		public int Port
		{
			get
			{
				return this._port;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00019268 File Offset: 0x00017468
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x00019280 File Offset: 0x00017480
		public string Realm
		{
			get
			{
				return this._realm;
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
							this._realm = value;
						}
					}
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00019300 File Offset: 0x00017500
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x00019318 File Offset: 0x00017518
		public bool ReuseAddress
		{
			get
			{
				return this._reuseAddress;
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
							this._reuseAddress = value;
						}
					}
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x00019398 File Offset: 0x00017598
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
				return this.getSslConfiguration();
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x000193CC File Offset: 0x000175CC
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x000193E4 File Offset: 0x000175E4
		public Func<IIdentity, WebSocketSharp.Net.NetworkCredential> UserCredentialsFinder
		{
			get
			{
				return this._userCredFinder;
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
							this._userCredFinder = value;
						}
					}
				}
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00019464 File Offset: 0x00017664
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x00019481 File Offset: 0x00017681
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00019494 File Offset: 0x00017694
		public WebSocketServiceManager WebSocketServices
		{
			get
			{
				return this._services;
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000194AC File Offset: 0x000176AC
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
					this._listener.Stop();
				}
				finally
				{
					this._services.Stop(1006, string.Empty);
				}
			}
			catch
			{
			}
			this._state = ServerState.Stop;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00019558 File Offset: 0x00017758
		private bool authenticateClient(TcpListenerWebSocketContext context)
		{
			bool flag = this._authSchemes == WebSocketSharp.Net.AuthenticationSchemes.Anonymous;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this._authSchemes == WebSocketSharp.Net.AuthenticationSchemes.None;
				result = (!flag2 && context.Authenticate(this._authSchemes, this._realmInUse, this._userCredFinder));
			}
			return result;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000195A8 File Offset: 0x000177A8
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

		// Token: 0x0600049C RID: 1180 RVA: 0x000195F4 File Offset: 0x000177F4
		private bool checkHostNameForRequest(string name)
		{
			return !this._dnsStyle || Uri.CheckHostName(name) != UriHostNameType.Dns || name == this._hostname;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00019628 File Offset: 0x00017828
		private static bool checkSslConfiguration(ServerSslConfiguration configuration, out string message)
		{
			message = null;
			bool flag = configuration.ServerCertificate == null;
			bool result;
			if (flag)
			{
				message = "There is no server certificate for secure connection.";
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00019658 File Offset: 0x00017858
		private string getRealm()
		{
			string realm = this._realm;
			return (realm != null && realm.Length > 0) ? realm : WebSocketServer._defaultRealm;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00019688 File Offset: 0x00017888
		private ServerSslConfiguration getSslConfiguration()
		{
			bool flag = this._sslConfig == null;
			if (flag)
			{
				this._sslConfig = new ServerSslConfiguration();
			}
			return this._sslConfig;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000196B8 File Offset: 0x000178B8
		private void init(string hostname, IPAddress address, int port, bool secure)
		{
			this._hostname = hostname;
			this._address = address;
			this._port = port;
			this._secure = secure;
			this._authSchemes = WebSocketSharp.Net.AuthenticationSchemes.Anonymous;
			this._dnsStyle = (Uri.CheckHostName(hostname) == UriHostNameType.Dns);
			this._listener = new TcpListener(address, port);
			this._log = new Logger();
			this._services = new WebSocketServiceManager(this._log);
			this._sync = new object();
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00019734 File Offset: 0x00017934
		private void processRequest(TcpListenerWebSocketContext context)
		{
			bool flag = !this.authenticateClient(context);
			if (flag)
			{
				context.Close(WebSocketSharp.Net.HttpStatusCode.Forbidden);
			}
			else
			{
				Uri requestUri = context.RequestUri;
				bool flag2 = requestUri == null;
				if (flag2)
				{
					context.Close(WebSocketSharp.Net.HttpStatusCode.BadRequest);
				}
				else
				{
					bool flag3 = !this._allowForwardedRequest;
					if (flag3)
					{
						bool flag4 = requestUri.Port != this._port;
						if (flag4)
						{
							context.Close(WebSocketSharp.Net.HttpStatusCode.BadRequest);
							return;
						}
						bool flag5 = !this.checkHostNameForRequest(requestUri.DnsSafeHost);
						if (flag5)
						{
							context.Close(WebSocketSharp.Net.HttpStatusCode.NotFound);
							return;
						}
					}
					string text = requestUri.AbsolutePath;
					bool flag6 = text.IndexOfAny(new char[]
					{
						'%',
						'+'
					}) > -1;
					if (flag6)
					{
						text = HttpUtility.UrlDecode(text, Encoding.UTF8);
					}
					WebSocketServiceHost webSocketServiceHost;
					bool flag7 = !this._services.InternalTryGetServiceHost(text, out webSocketServiceHost);
					if (flag7)
					{
						context.Close(WebSocketSharp.Net.HttpStatusCode.NotImplemented);
					}
					else
					{
						webSocketServiceHost.StartSession(context);
					}
				}
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00019848 File Offset: 0x00017A48
		private void receiveRequest()
		{
			for (;;)
			{
				TcpClient cl = null;
				try
				{
					cl = this._listener.AcceptTcpClient();
					ThreadPool.QueueUserWorkItem(delegate(object state)
					{
						try
						{
							TcpListenerWebSocketContext context = new TcpListenerWebSocketContext(cl, null, this._secure, this._sslConfigInUse, this._log);
							this.processRequest(context);
						}
						catch (Exception ex3)
						{
							this._log.Error(ex3.Message);
							this._log.Debug(ex3.ToString());
							cl.Close();
						}
					});
				}
				catch (SocketException ex)
				{
					bool flag = this._state == ServerState.ShuttingDown;
					if (flag)
					{
						this._log.Info("The underlying listener is stopped.");
						break;
					}
					this._log.Fatal(ex.Message);
					this._log.Debug(ex.ToString());
					break;
				}
				catch (Exception ex2)
				{
					this._log.Fatal(ex2.Message);
					this._log.Debug(ex2.ToString());
					bool flag2 = cl != null;
					if (flag2)
					{
						cl.Close();
					}
					break;
				}
			}
			bool flag3 = this._state != ServerState.ShuttingDown;
			if (flag3)
			{
				this.abort();
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00019968 File Offset: 0x00017B68
		private void start(ServerSslConfiguration sslConfig)
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
								this._sslConfigInUse = sslConfig;
								this._realmInUse = this.getRealm();
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

		// Token: 0x060004A4 RID: 1188 RVA: 0x00019A90 File Offset: 0x00017C90
		private void startReceiving()
		{
			bool reuseAddress = this._reuseAddress;
			if (reuseAddress)
			{
				this._listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			}
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

		// Token: 0x060004A5 RID: 1189 RVA: 0x00019B24 File Offset: 0x00017D24
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
								this.stopReceiving(5000);
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
									this._services.Stop(code, reason);
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

		// Token: 0x060004A6 RID: 1190 RVA: 0x00019CA8 File Offset: 0x00017EA8
		private void stopReceiving(int millisecondsTimeout)
		{
			try
			{
				this._listener.Stop();
			}
			catch (Exception innerException)
			{
				string message = "The underlying listener has failed to stop.";
				throw new InvalidOperationException(message, innerException);
			}
			this._receiveThread.Join(millisecondsTimeout);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00019CF4 File Offset: 0x00017EF4
		private static bool tryCreateUri(string uriString, out Uri result, out string message)
		{
			bool flag = !uriString.TryCreateWebSocketUri(out result, out message);
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				bool flag2 = result.PathAndQuery != "/";
				if (flag2)
				{
					result = null;
					message = "It includes either or both path and query components.";
					result2 = false;
				}
				else
				{
					result2 = true;
				}
			}
			return result2;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00019D40 File Offset: 0x00017F40
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

		// Token: 0x060004A9 RID: 1193 RVA: 0x00019DF5 File Offset: 0x00017FF5
		public void AddWebSocketService<TBehaviorWithNew>(string path) where TBehaviorWithNew : WebSocketBehavior, new()
		{
			this._services.AddService<TBehaviorWithNew>(path, null);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00019E06 File Offset: 0x00018006
		public void AddWebSocketService<TBehaviorWithNew>(string path, Action<TBehaviorWithNew> initializer) where TBehaviorWithNew : WebSocketBehavior, new()
		{
			this._services.AddService<TBehaviorWithNew>(path, initializer);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00019E18 File Offset: 0x00018018
		public bool RemoveWebSocketService(string path)
		{
			return this._services.RemoveService(path);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00019E38 File Offset: 0x00018038
		public void Start()
		{
			ServerSslConfiguration serverSslConfiguration = null;
			bool secure = this._secure;
			if (secure)
			{
				serverSslConfiguration = new ServerSslConfiguration(this.getSslConfiguration());
				string message;
				bool flag = !WebSocketServer.checkSslConfiguration(serverSslConfiguration, out message);
				if (flag)
				{
					throw new InvalidOperationException(message);
				}
			}
			this.start(serverSslConfiguration);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00019E7E File Offset: 0x0001807E
		public void Stop()
		{
			this.stop(1001, string.Empty);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00019E94 File Offset: 0x00018094
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

		// Token: 0x060004AF RID: 1199 RVA: 0x00019F68 File Offset: 0x00018168
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

		// Token: 0x04000216 RID: 534
		private IPAddress _address;

		// Token: 0x04000217 RID: 535
		private bool _allowForwardedRequest;

		// Token: 0x04000218 RID: 536
		private WebSocketSharp.Net.AuthenticationSchemes _authSchemes;

		// Token: 0x04000219 RID: 537
		private static readonly string _defaultRealm = "SECRET AREA";

		// Token: 0x0400021A RID: 538
		private bool _dnsStyle;

		// Token: 0x0400021B RID: 539
		private string _hostname;

		// Token: 0x0400021C RID: 540
		private TcpListener _listener;

		// Token: 0x0400021D RID: 541
		private Logger _log;

		// Token: 0x0400021E RID: 542
		private int _port;

		// Token: 0x0400021F RID: 543
		private string _realm;

		// Token: 0x04000220 RID: 544
		private string _realmInUse;

		// Token: 0x04000221 RID: 545
		private Thread _receiveThread;

		// Token: 0x04000222 RID: 546
		private bool _reuseAddress;

		// Token: 0x04000223 RID: 547
		private bool _secure;

		// Token: 0x04000224 RID: 548
		private WebSocketServiceManager _services;

		// Token: 0x04000225 RID: 549
		private ServerSslConfiguration _sslConfig;

		// Token: 0x04000226 RID: 550
		private ServerSslConfiguration _sslConfigInUse;

		// Token: 0x04000227 RID: 551
		private volatile ServerState _state;

		// Token: 0x04000228 RID: 552
		private object _sync;

		// Token: 0x04000229 RID: 553
		private Func<IIdentity, WebSocketSharp.Net.NetworkCredential> _userCredFinder;
	}
}
