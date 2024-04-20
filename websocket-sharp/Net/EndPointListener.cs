using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace WebSocketSharp.Net
{
	// Token: 0x0200001D RID: 29
	internal sealed class EndPointListener
	{
		// Token: 0x06000208 RID: 520 RVA: 0x0000D64C File Offset: 0x0000B84C
		internal EndPointListener(IPEndPoint endpoint, bool secure, string certificateFolderPath, ServerSslConfiguration sslConfig, bool reuseAddress)
		{
			if (secure)
			{
				X509Certificate2 certificate = EndPointListener.getCertificate(endpoint.Port, certificateFolderPath, sslConfig.ServerCertificate);
				bool flag = certificate == null;
				if (flag)
				{
					throw new ArgumentException("No server certificate could be found.");
				}
				this._secure = true;
				this._sslConfig = new ServerSslConfiguration(sslConfig);
				this._sslConfig.ServerCertificate = certificate;
			}
			this._endpoint = endpoint;
			this._prefixes = new Dictionary<HttpListenerPrefix, HttpListener>();
			this._unregistered = new Dictionary<HttpConnection, HttpConnection>();
			this._unregisteredSync = ((ICollection)this._unregistered).SyncRoot;
			this._socket = new Socket(endpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (reuseAddress)
			{
				this._socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			}
			this._socket.Bind(endpoint);
			this._socket.Listen(500);
			this._socket.BeginAccept(new AsyncCallback(EndPointListener.onAccept), this);
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000D748 File Offset: 0x0000B948
		public IPAddress Address
		{
			get
			{
				return this._endpoint.Address;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000D768 File Offset: 0x0000B968
		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000D780 File Offset: 0x0000B980
		public int Port
		{
			get
			{
				return this._endpoint.Port;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000D7A0 File Offset: 0x0000B9A0
		public ServerSslConfiguration SslConfiguration
		{
			get
			{
				return this._sslConfig;
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000D7B8 File Offset: 0x0000B9B8
		private static void addSpecial(List<HttpListenerPrefix> prefixes, HttpListenerPrefix prefix)
		{
			string path = prefix.Path;
			foreach (HttpListenerPrefix httpListenerPrefix in prefixes)
			{
				bool flag = httpListenerPrefix.Path == path;
				if (flag)
				{
					throw new HttpListenerException(87, "The prefix is already in use.");
				}
			}
			prefixes.Add(prefix);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000D830 File Offset: 0x0000BA30
		private static RSACryptoServiceProvider createRSAFromFile(string filename)
		{
			byte[] array = null;
			using (FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
			}
			RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
			rsacryptoServiceProvider.ImportCspBlob(array);
			return rsacryptoServiceProvider;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000D898 File Offset: 0x0000BA98
		private static X509Certificate2 getCertificate(int port, string folderPath, X509Certificate2 defaultCertificate)
		{
			bool flag = folderPath == null || folderPath.Length == 0;
			if (flag)
			{
				folderPath = EndPointListener._defaultCertFolderPath;
			}
			try
			{
				string text = Path.Combine(folderPath, string.Format("{0}.cer", port));
				string text2 = Path.Combine(folderPath, string.Format("{0}.key", port));
				bool flag2 = File.Exists(text) && File.Exists(text2);
				if (flag2)
				{
					return new X509Certificate2(text)
					{
						PrivateKey = EndPointListener.createRSAFromFile(text2)
					};
				}
			}
			catch
			{
			}
			return defaultCertificate;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000D944 File Offset: 0x0000BB44
		private void leaveIfNoPrefix()
		{
			bool flag = this._prefixes.Count > 0;
			if (!flag)
			{
				List<HttpListenerPrefix> list = this._unhandled;
				bool flag2 = list != null && list.Count > 0;
				if (!flag2)
				{
					list = this._all;
					bool flag3 = list != null && list.Count > 0;
					if (!flag3)
					{
						EndPointManager.RemoveEndPoint(this._endpoint);
					}
				}
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000D9AC File Offset: 0x0000BBAC
		private static void onAccept(IAsyncResult asyncResult)
		{
			EndPointListener endPointListener = (EndPointListener)asyncResult.AsyncState;
			Socket socket = null;
			try
			{
				socket = endPointListener._socket.EndAccept(asyncResult);
			}
			catch (SocketException)
			{
			}
			catch (ObjectDisposedException)
			{
				return;
			}
			try
			{
				endPointListener._socket.BeginAccept(new AsyncCallback(EndPointListener.onAccept), endPointListener);
			}
			catch
			{
				bool flag = socket != null;
				if (flag)
				{
					socket.Close();
				}
				return;
			}
			bool flag2 = socket == null;
			if (!flag2)
			{
				EndPointListener.processAccepted(socket, endPointListener);
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000DA50 File Offset: 0x0000BC50
		private static void processAccepted(Socket socket, EndPointListener listener)
		{
			HttpConnection httpConnection = null;
			try
			{
				httpConnection = new HttpConnection(socket, listener);
				object unregisteredSync = listener._unregisteredSync;
				lock (unregisteredSync)
				{
					listener._unregistered[httpConnection] = httpConnection;
				}
				httpConnection.BeginReadRequest();
			}
			catch
			{
				bool flag = httpConnection != null;
				if (flag)
				{
					httpConnection.Close(true);
				}
				else
				{
					socket.Close();
				}
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000DAD4 File Offset: 0x0000BCD4
		private static bool removeSpecial(List<HttpListenerPrefix> prefixes, HttpListenerPrefix prefix)
		{
			string path = prefix.Path;
			int count = prefixes.Count;
			for (int i = 0; i < count; i++)
			{
				bool flag = prefixes[i].Path == path;
				if (flag)
				{
					prefixes.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000DB30 File Offset: 0x0000BD30
		private static HttpListener searchHttpListenerFromSpecial(string path, List<HttpListenerPrefix> prefixes)
		{
			bool flag = prefixes == null;
			HttpListener result;
			if (flag)
			{
				result = null;
			}
			else
			{
				HttpListener httpListener = null;
				int num = -1;
				foreach (HttpListenerPrefix httpListenerPrefix in prefixes)
				{
					string path2 = httpListenerPrefix.Path;
					int length = path2.Length;
					bool flag2 = length < num;
					if (!flag2)
					{
						bool flag3 = path.StartsWith(path2);
						if (flag3)
						{
							num = length;
							httpListener = httpListenerPrefix.Listener;
						}
					}
				}
				result = httpListener;
			}
			return result;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		internal static bool CertificateExists(int port, string folderPath)
		{
			bool flag = folderPath == null || folderPath.Length == 0;
			if (flag)
			{
				folderPath = EndPointListener._defaultCertFolderPath;
			}
			string path = Path.Combine(folderPath, string.Format("{0}.cer", port));
			string path2 = Path.Combine(folderPath, string.Format("{0}.key", port));
			return File.Exists(path) && File.Exists(path2);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000DC3C File Offset: 0x0000BE3C
		internal void RemoveConnection(HttpConnection connection)
		{
			object unregisteredSync = this._unregisteredSync;
			lock (unregisteredSync)
			{
				this._unregistered.Remove(connection);
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000DC80 File Offset: 0x0000BE80
		internal bool TrySearchHttpListener(Uri uri, out HttpListener listener)
		{
			listener = null;
			bool flag = uri == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				string host = uri.Host;
				bool flag2 = Uri.CheckHostName(host) == UriHostNameType.Dns;
				string b = uri.Port.ToString();
				string text = HttpUtility.UrlDecode(uri.AbsolutePath);
				string text2 = (text[text.Length - 1] != '/') ? (text + "/") : text;
				bool flag3 = host != null && host.Length > 0;
				if (flag3)
				{
					int num = -1;
					foreach (HttpListenerPrefix httpListenerPrefix in this._prefixes.Keys)
					{
						bool flag4 = flag2;
						if (flag4)
						{
							string host2 = httpListenerPrefix.Host;
							bool flag5 = Uri.CheckHostName(host2) == UriHostNameType.Dns && host2 != host;
							if (flag5)
							{
								continue;
							}
						}
						bool flag6 = httpListenerPrefix.Port != b;
						if (!flag6)
						{
							string path = httpListenerPrefix.Path;
							int length = path.Length;
							bool flag7 = length < num;
							if (!flag7)
							{
								bool flag8 = text.StartsWith(path) || text2.StartsWith(path);
								if (flag8)
								{
									num = length;
									listener = this._prefixes[httpListenerPrefix];
								}
							}
						}
					}
					bool flag9 = num != -1;
					if (flag9)
					{
						return true;
					}
				}
				List<HttpListenerPrefix> prefixes = this._unhandled;
				listener = EndPointListener.searchHttpListenerFromSpecial(text, prefixes);
				bool flag10 = listener == null && text2 != text;
				if (flag10)
				{
					listener = EndPointListener.searchHttpListenerFromSpecial(text2, prefixes);
				}
				bool flag11 = listener != null;
				if (flag11)
				{
					result = true;
				}
				else
				{
					prefixes = this._all;
					listener = EndPointListener.searchHttpListenerFromSpecial(text, prefixes);
					bool flag12 = listener == null && text2 != text;
					if (flag12)
					{
						listener = EndPointListener.searchHttpListenerFromSpecial(text2, prefixes);
					}
					result = (listener != null);
				}
			}
			return result;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000DE8C File Offset: 0x0000C08C
		public void AddPrefix(HttpListenerPrefix prefix, HttpListener listener)
		{
			bool flag = prefix.Host == "*";
			if (flag)
			{
				List<HttpListenerPrefix> list;
				List<HttpListenerPrefix> list2;
				do
				{
					list = this._unhandled;
					list2 = ((list != null) ? new List<HttpListenerPrefix>(list) : new List<HttpListenerPrefix>());
					prefix.Listener = listener;
					EndPointListener.addSpecial(list2, prefix);
				}
				while (Interlocked.CompareExchange<List<HttpListenerPrefix>>(ref this._unhandled, list2, list) != list);
			}
			else
			{
				bool flag2 = prefix.Host == "+";
				if (flag2)
				{
					List<HttpListenerPrefix> list;
					List<HttpListenerPrefix> list2;
					do
					{
						list = this._all;
						list2 = ((list != null) ? new List<HttpListenerPrefix>(list) : new List<HttpListenerPrefix>());
						prefix.Listener = listener;
						EndPointListener.addSpecial(list2, prefix);
					}
					while (Interlocked.CompareExchange<List<HttpListenerPrefix>>(ref this._all, list2, list) != list);
				}
				else
				{
					Dictionary<HttpListenerPrefix, HttpListener> prefixes;
					for (;;)
					{
						prefixes = this._prefixes;
						bool flag3 = prefixes.ContainsKey(prefix);
						if (flag3)
						{
							break;
						}
						Dictionary<HttpListenerPrefix, HttpListener> dictionary = new Dictionary<HttpListenerPrefix, HttpListener>(prefixes);
						dictionary[prefix] = listener;
						if (Interlocked.CompareExchange<Dictionary<HttpListenerPrefix, HttpListener>>(ref this._prefixes, dictionary, prefixes) == prefixes)
						{
							return;
						}
					}
					bool flag4 = prefixes[prefix] != listener;
					if (flag4)
					{
						throw new HttpListenerException(87, string.Format("There's another listener for {0}.", prefix));
					}
				}
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
		public void Close()
		{
			this._socket.Close();
			HttpConnection[] array = null;
			object unregisteredSync = this._unregisteredSync;
			lock (unregisteredSync)
			{
				bool flag = this._unregistered.Count == 0;
				if (flag)
				{
					return;
				}
				Dictionary<HttpConnection, HttpConnection>.KeyCollection keys = this._unregistered.Keys;
				array = new HttpConnection[keys.Count];
				keys.CopyTo(array, 0);
				this._unregistered.Clear();
			}
			for (int i = array.Length - 1; i >= 0; i--)
			{
				array[i].Close(true);
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000E070 File Offset: 0x0000C270
		public void RemovePrefix(HttpListenerPrefix prefix, HttpListener listener)
		{
			bool flag = prefix.Host == "*";
			if (flag)
			{
				List<HttpListenerPrefix> list;
				List<HttpListenerPrefix> list2;
				do
				{
					list = this._unhandled;
					bool flag2 = list == null;
					if (flag2)
					{
						break;
					}
					list2 = new List<HttpListenerPrefix>(list);
					bool flag3 = !EndPointListener.removeSpecial(list2, prefix);
					if (flag3)
					{
						break;
					}
				}
				while (Interlocked.CompareExchange<List<HttpListenerPrefix>>(ref this._unhandled, list2, list) != list);
				this.leaveIfNoPrefix();
			}
			else
			{
				bool flag4 = prefix.Host == "+";
				if (flag4)
				{
					List<HttpListenerPrefix> list;
					List<HttpListenerPrefix> list2;
					do
					{
						list = this._all;
						bool flag5 = list == null;
						if (flag5)
						{
							break;
						}
						list2 = new List<HttpListenerPrefix>(list);
						bool flag6 = !EndPointListener.removeSpecial(list2, prefix);
						if (flag6)
						{
							break;
						}
					}
					while (Interlocked.CompareExchange<List<HttpListenerPrefix>>(ref this._all, list2, list) != list);
					this.leaveIfNoPrefix();
				}
				else
				{
					Dictionary<HttpListenerPrefix, HttpListener> prefixes;
					Dictionary<HttpListenerPrefix, HttpListener> dictionary;
					do
					{
						prefixes = this._prefixes;
						bool flag7 = !prefixes.ContainsKey(prefix);
						if (flag7)
						{
							break;
						}
						dictionary = new Dictionary<HttpListenerPrefix, HttpListener>(prefixes);
						dictionary.Remove(prefix);
					}
					while (Interlocked.CompareExchange<Dictionary<HttpListenerPrefix, HttpListener>>(ref this._prefixes, dictionary, prefixes) != prefixes);
					this.leaveIfNoPrefix();
				}
			}
		}

		// Token: 0x040000BA RID: 186
		private List<HttpListenerPrefix> _all;

		// Token: 0x040000BB RID: 187
		private static readonly string _defaultCertFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		// Token: 0x040000BC RID: 188
		private IPEndPoint _endpoint;

		// Token: 0x040000BD RID: 189
		private Dictionary<HttpListenerPrefix, HttpListener> _prefixes;

		// Token: 0x040000BE RID: 190
		private bool _secure;

		// Token: 0x040000BF RID: 191
		private Socket _socket;

		// Token: 0x040000C0 RID: 192
		private ServerSslConfiguration _sslConfig;

		// Token: 0x040000C1 RID: 193
		private List<HttpListenerPrefix> _unhandled;

		// Token: 0x040000C2 RID: 194
		private Dictionary<HttpConnection, HttpConnection> _unregistered;

		// Token: 0x040000C3 RID: 195
		private object _unregisteredSync;
	}
}
