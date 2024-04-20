using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;

namespace WebSocketSharp.Net
{
	// Token: 0x02000020 RID: 32
	public sealed class HttpListener : IDisposable
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0000F44C File Offset: 0x0000D64C
		public HttpListener()
		{
			this._authSchemes = AuthenticationSchemes.Anonymous;
			this._connections = new Dictionary<HttpConnection, HttpConnection>();
			this._connectionsSync = ((ICollection)this._connections).SyncRoot;
			this._ctxQueue = new List<HttpListenerContext>();
			this._ctxQueueSync = ((ICollection)this._ctxQueue).SyncRoot;
			this._ctxRegistry = new Dictionary<HttpListenerContext, HttpListenerContext>();
			this._ctxRegistrySync = ((ICollection)this._ctxRegistry).SyncRoot;
			this._logger = new Logger();
			this._prefixes = new HttpListenerPrefixCollection(this);
			this._waitQueue = new List<HttpListenerAsyncResult>();
			this._waitQueueSync = ((ICollection)this._waitQueue).SyncRoot;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000F4F4 File Offset: 0x0000D6F4
		internal bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000F50C File Offset: 0x0000D70C
		// (set) Token: 0x06000244 RID: 580 RVA: 0x0000F524 File Offset: 0x0000D724
		internal bool ReuseAddress
		{
			get
			{
				return this._reuseAddress;
			}
			set
			{
				this._reuseAddress = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000F530 File Offset: 0x0000D730
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000F54F File Offset: 0x0000D74F
		public AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				this.CheckDisposed();
				return this._authSchemes;
			}
			set
			{
				this.CheckDisposed();
				this._authSchemes = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000F560 File Offset: 0x0000D760
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000F57F File Offset: 0x0000D77F
		public Func<HttpListenerRequest, AuthenticationSchemes> AuthenticationSchemeSelector
		{
			get
			{
				this.CheckDisposed();
				return this._authSchemeSelector;
			}
			set
			{
				this.CheckDisposed();
				this._authSchemeSelector = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000F590 File Offset: 0x0000D790
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000F5AF File Offset: 0x0000D7AF
		public string CertificateFolderPath
		{
			get
			{
				this.CheckDisposed();
				return this._certFolderPath;
			}
			set
			{
				this.CheckDisposed();
				this._certFolderPath = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
		// (set) Token: 0x0600024C RID: 588 RVA: 0x0000F5DF File Offset: 0x0000D7DF
		public bool IgnoreWriteExceptions
		{
			get
			{
				this.CheckDisposed();
				return this._ignoreWriteExceptions;
			}
			set
			{
				this.CheckDisposed();
				this._ignoreWriteExceptions = value;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000F5F0 File Offset: 0x0000D7F0
		public bool IsListening
		{
			get
			{
				return this._listening;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000F60C File Offset: 0x0000D80C
		public static bool IsSupported
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000F620 File Offset: 0x0000D820
		public Logger Log
		{
			get
			{
				return this._logger;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000F638 File Offset: 0x0000D838
		public HttpListenerPrefixCollection Prefixes
		{
			get
			{
				this.CheckDisposed();
				return this._prefixes;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000F658 File Offset: 0x0000D858
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000F677 File Offset: 0x0000D877
		public string Realm
		{
			get
			{
				this.CheckDisposed();
				return this._realm;
			}
			set
			{
				this.CheckDisposed();
				this._realm = value;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000F688 File Offset: 0x0000D888
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000F6B9 File Offset: 0x0000D8B9
		public ServerSslConfiguration SslConfiguration
		{
			get
			{
				this.CheckDisposed();
				ServerSslConfiguration result;
				if ((result = this._sslConfig) == null)
				{
					result = (this._sslConfig = new ServerSslConfiguration());
				}
				return result;
			}
			set
			{
				this.CheckDisposed();
				this._sslConfig = value;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		// (set) Token: 0x06000256 RID: 598 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public bool UnsafeConnectionNtlmAuthentication
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000F6D4 File Offset: 0x0000D8D4
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000F6F3 File Offset: 0x0000D8F3
		public Func<IIdentity, NetworkCredential> UserCredentialsFinder
		{
			get
			{
				this.CheckDisposed();
				return this._userCredFinder;
			}
			set
			{
				this.CheckDisposed();
				this._userCredFinder = value;
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000F704 File Offset: 0x0000D904
		private void cleanupConnections()
		{
			HttpConnection[] array = null;
			object connectionsSync = this._connectionsSync;
			lock (connectionsSync)
			{
				bool flag = this._connections.Count == 0;
				if (flag)
				{
					return;
				}
				Dictionary<HttpConnection, HttpConnection>.KeyCollection keys = this._connections.Keys;
				array = new HttpConnection[keys.Count];
				keys.CopyTo(array, 0);
				this._connections.Clear();
			}
			for (int i = array.Length - 1; i >= 0; i--)
			{
				array[i].Close(true);
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000F7A8 File Offset: 0x0000D9A8
		private void cleanupContextQueue(bool sendServiceUnavailable)
		{
			HttpListenerContext[] array = null;
			object ctxQueueSync = this._ctxQueueSync;
			lock (ctxQueueSync)
			{
				bool flag = this._ctxQueue.Count == 0;
				if (flag)
				{
					return;
				}
				array = this._ctxQueue.ToArray();
				this._ctxQueue.Clear();
			}
			bool flag2 = !sendServiceUnavailable;
			if (!flag2)
			{
				foreach (HttpListenerContext httpListenerContext in array)
				{
					HttpListenerResponse response = httpListenerContext.Response;
					response.StatusCode = 503;
					response.Close();
				}
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000F858 File Offset: 0x0000DA58
		private void cleanupContextRegistry()
		{
			HttpListenerContext[] array = null;
			object ctxRegistrySync = this._ctxRegistrySync;
			lock (ctxRegistrySync)
			{
				bool flag = this._ctxRegistry.Count == 0;
				if (flag)
				{
					return;
				}
				Dictionary<HttpListenerContext, HttpListenerContext>.KeyCollection keys = this._ctxRegistry.Keys;
				array = new HttpListenerContext[keys.Count];
				keys.CopyTo(array, 0);
				this._ctxRegistry.Clear();
			}
			for (int i = array.Length - 1; i >= 0; i--)
			{
				array[i].Connection.Close(true);
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000F900 File Offset: 0x0000DB00
		private void cleanupWaitQueue(Exception exception)
		{
			HttpListenerAsyncResult[] array = null;
			object waitQueueSync = this._waitQueueSync;
			lock (waitQueueSync)
			{
				bool flag = this._waitQueue.Count == 0;
				if (flag)
				{
					return;
				}
				array = this._waitQueue.ToArray();
				this._waitQueue.Clear();
			}
			foreach (HttpListenerAsyncResult httpListenerAsyncResult in array)
			{
				httpListenerAsyncResult.Complete(exception);
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000F98C File Offset: 0x0000DB8C
		private void close(bool force)
		{
			bool listening = this._listening;
			if (listening)
			{
				this._listening = false;
				EndPointManager.RemoveListener(this);
			}
			object ctxRegistrySync = this._ctxRegistrySync;
			lock (ctxRegistrySync)
			{
				this.cleanupContextQueue(!force);
			}
			this.cleanupContextRegistry();
			this.cleanupConnections();
			this.cleanupWaitQueue(new ObjectDisposedException(base.GetType().ToString()));
			this._disposed = true;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000FA18 File Offset: 0x0000DC18
		private HttpListenerAsyncResult getAsyncResultFromQueue()
		{
			bool flag = this._waitQueue.Count == 0;
			HttpListenerAsyncResult result;
			if (flag)
			{
				result = null;
			}
			else
			{
				HttpListenerAsyncResult httpListenerAsyncResult = this._waitQueue[0];
				this._waitQueue.RemoveAt(0);
				result = httpListenerAsyncResult;
			}
			return result;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000FA5C File Offset: 0x0000DC5C
		private HttpListenerContext getContextFromQueue()
		{
			bool flag = this._ctxQueue.Count == 0;
			HttpListenerContext result;
			if (flag)
			{
				result = null;
			}
			else
			{
				HttpListenerContext httpListenerContext = this._ctxQueue[0];
				this._ctxQueue.RemoveAt(0);
				result = httpListenerContext;
			}
			return result;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000FAA0 File Offset: 0x0000DCA0
		internal bool AddConnection(HttpConnection connection)
		{
			bool flag = !this._listening;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object connectionsSync = this._connectionsSync;
				lock (connectionsSync)
				{
					bool flag2 = !this._listening;
					if (flag2)
					{
						result = false;
					}
					else
					{
						this._connections[connection] = connection;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000FB10 File Offset: 0x0000DD10
		internal HttpListenerAsyncResult BeginGetContext(HttpListenerAsyncResult asyncResult)
		{
			object ctxRegistrySync = this._ctxRegistrySync;
			lock (ctxRegistrySync)
			{
				bool flag = !this._listening;
				if (flag)
				{
					throw new HttpListenerException(995);
				}
				HttpListenerContext contextFromQueue = this.getContextFromQueue();
				bool flag2 = contextFromQueue == null;
				if (flag2)
				{
					this._waitQueue.Add(asyncResult);
				}
				else
				{
					asyncResult.Complete(contextFromQueue, true);
				}
			}
			return asyncResult;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000FB90 File Offset: 0x0000DD90
		internal void CheckDisposed()
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000FBBC File Offset: 0x0000DDBC
		internal string GetRealm()
		{
			string realm = this._realm;
			return (realm != null && realm.Length > 0) ? realm : HttpListener._defaultRealm;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000FBEC File Offset: 0x0000DDEC
		internal Func<IIdentity, NetworkCredential> GetUserCredentialsFinder()
		{
			return this._userCredFinder;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000FC04 File Offset: 0x0000DE04
		internal bool RegisterContext(HttpListenerContext context)
		{
			bool flag = !this._listening;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object ctxRegistrySync = this._ctxRegistrySync;
				lock (ctxRegistrySync)
				{
					bool flag2 = !this._listening;
					if (flag2)
					{
						result = false;
					}
					else
					{
						this._ctxRegistry[context] = context;
						HttpListenerAsyncResult asyncResultFromQueue = this.getAsyncResultFromQueue();
						bool flag3 = asyncResultFromQueue == null;
						if (flag3)
						{
							this._ctxQueue.Add(context);
						}
						else
						{
							asyncResultFromQueue.Complete(context);
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000FC9C File Offset: 0x0000DE9C
		internal void RemoveConnection(HttpConnection connection)
		{
			object connectionsSync = this._connectionsSync;
			lock (connectionsSync)
			{
				this._connections.Remove(connection);
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000FCE0 File Offset: 0x0000DEE0
		internal AuthenticationSchemes SelectAuthenticationScheme(HttpListenerRequest request)
		{
			Func<HttpListenerRequest, AuthenticationSchemes> authSchemeSelector = this._authSchemeSelector;
			bool flag = authSchemeSelector == null;
			AuthenticationSchemes result;
			if (flag)
			{
				result = this._authSchemes;
			}
			else
			{
				try
				{
					result = authSchemeSelector(request);
				}
				catch
				{
					result = AuthenticationSchemes.None;
				}
			}
			return result;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000FD2C File Offset: 0x0000DF2C
		internal void UnregisterContext(HttpListenerContext context)
		{
			object ctxRegistrySync = this._ctxRegistrySync;
			lock (ctxRegistrySync)
			{
				this._ctxRegistry.Remove(context);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000FD70 File Offset: 0x0000DF70
		public void Abort()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.close(true);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000FD94 File Offset: 0x0000DF94
		public IAsyncResult BeginGetContext(AsyncCallback callback, object state)
		{
			this.CheckDisposed();
			bool flag = this._prefixes.Count == 0;
			if (flag)
			{
				throw new InvalidOperationException("The listener has no URI prefix on which listens.");
			}
			bool flag2 = !this._listening;
			if (flag2)
			{
				throw new InvalidOperationException("The listener hasn't been started.");
			}
			return this.BeginGetContext(new HttpListenerAsyncResult(callback, state));
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000FDF4 File Offset: 0x0000DFF4
		public void Close()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.close(false);
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000FE18 File Offset: 0x0000E018
		public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
		{
			this.CheckDisposed();
			bool flag = asyncResult == null;
			if (flag)
			{
				throw new ArgumentNullException("asyncResult");
			}
			HttpListenerAsyncResult httpListenerAsyncResult = asyncResult as HttpListenerAsyncResult;
			bool flag2 = httpListenerAsyncResult == null;
			if (flag2)
			{
				throw new ArgumentException("A wrong IAsyncResult.", "asyncResult");
			}
			bool endCalled = httpListenerAsyncResult.EndCalled;
			if (endCalled)
			{
				throw new InvalidOperationException("This IAsyncResult cannot be reused.");
			}
			httpListenerAsyncResult.EndCalled = true;
			bool flag3 = !httpListenerAsyncResult.IsCompleted;
			if (flag3)
			{
				httpListenerAsyncResult.AsyncWaitHandle.WaitOne();
			}
			return httpListenerAsyncResult.GetContext();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000FEA4 File Offset: 0x0000E0A4
		public HttpListenerContext GetContext()
		{
			this.CheckDisposed();
			bool flag = this._prefixes.Count == 0;
			if (flag)
			{
				throw new InvalidOperationException("The listener has no URI prefix on which listens.");
			}
			bool flag2 = !this._listening;
			if (flag2)
			{
				throw new InvalidOperationException("The listener hasn't been started.");
			}
			HttpListenerAsyncResult httpListenerAsyncResult = this.BeginGetContext(new HttpListenerAsyncResult(null, null));
			httpListenerAsyncResult.InGet = true;
			return this.EndGetContext(httpListenerAsyncResult);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000FF14 File Offset: 0x0000E114
		public void Start()
		{
			this.CheckDisposed();
			bool listening = this._listening;
			if (!listening)
			{
				EndPointManager.AddListener(this);
				this._listening = true;
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000FF48 File Offset: 0x0000E148
		public void Stop()
		{
			this.CheckDisposed();
			bool flag = !this._listening;
			if (!flag)
			{
				this._listening = false;
				EndPointManager.RemoveListener(this);
				object ctxRegistrySync = this._ctxRegistrySync;
				lock (ctxRegistrySync)
				{
					this.cleanupContextQueue(true);
				}
				this.cleanupContextRegistry();
				this.cleanupConnections();
				this.cleanupWaitQueue(new HttpListenerException(995, "The listener is stopped."));
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000FFD4 File Offset: 0x0000E1D4
		void IDisposable.Dispose()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.close(true);
			}
		}

		// Token: 0x040000DC RID: 220
		private AuthenticationSchemes _authSchemes;

		// Token: 0x040000DD RID: 221
		private Func<HttpListenerRequest, AuthenticationSchemes> _authSchemeSelector;

		// Token: 0x040000DE RID: 222
		private string _certFolderPath;

		// Token: 0x040000DF RID: 223
		private Dictionary<HttpConnection, HttpConnection> _connections;

		// Token: 0x040000E0 RID: 224
		private object _connectionsSync;

		// Token: 0x040000E1 RID: 225
		private List<HttpListenerContext> _ctxQueue;

		// Token: 0x040000E2 RID: 226
		private object _ctxQueueSync;

		// Token: 0x040000E3 RID: 227
		private Dictionary<HttpListenerContext, HttpListenerContext> _ctxRegistry;

		// Token: 0x040000E4 RID: 228
		private object _ctxRegistrySync;

		// Token: 0x040000E5 RID: 229
		private static readonly string _defaultRealm = "SECRET AREA";

		// Token: 0x040000E6 RID: 230
		private bool _disposed;

		// Token: 0x040000E7 RID: 231
		private bool _ignoreWriteExceptions;

		// Token: 0x040000E8 RID: 232
		private volatile bool _listening;

		// Token: 0x040000E9 RID: 233
		private Logger _logger;

		// Token: 0x040000EA RID: 234
		private HttpListenerPrefixCollection _prefixes;

		// Token: 0x040000EB RID: 235
		private string _realm;

		// Token: 0x040000EC RID: 236
		private bool _reuseAddress;

		// Token: 0x040000ED RID: 237
		private ServerSslConfiguration _sslConfig;

		// Token: 0x040000EE RID: 238
		private Func<IIdentity, NetworkCredential> _userCredFinder;

		// Token: 0x040000EF RID: 239
		private List<HttpListenerAsyncResult> _waitQueue;

		// Token: 0x040000F0 RID: 240
		private object _waitQueueSync;
	}
}
