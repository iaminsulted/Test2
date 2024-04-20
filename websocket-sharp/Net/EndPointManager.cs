using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace WebSocketSharp.Net
{
	// Token: 0x0200001E RID: 30
	internal sealed class EndPointManager
	{
		// Token: 0x0600021C RID: 540 RVA: 0x00009A4B File Offset: 0x00007C4B
		private EndPointManager()
		{
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000E1A8 File Offset: 0x0000C3A8
		private static void addPrefix(string uriPrefix, HttpListener listener)
		{
			HttpListenerPrefix httpListenerPrefix = new HttpListenerPrefix(uriPrefix);
			IPAddress ipaddress = EndPointManager.convertToIPAddress(httpListenerPrefix.Host);
			bool flag = ipaddress == null;
			if (flag)
			{
				throw new HttpListenerException(87, "Includes an invalid host.");
			}
			bool flag2 = !ipaddress.IsLocal();
			if (flag2)
			{
				throw new HttpListenerException(87, "Includes an invalid host.");
			}
			int num;
			bool flag3 = !int.TryParse(httpListenerPrefix.Port, out num);
			if (flag3)
			{
				throw new HttpListenerException(87, "Includes an invalid port.");
			}
			bool flag4 = !num.IsPortNumber();
			if (flag4)
			{
				throw new HttpListenerException(87, "Includes an invalid port.");
			}
			string path = httpListenerPrefix.Path;
			bool flag5 = path.IndexOf('%') != -1;
			if (flag5)
			{
				throw new HttpListenerException(87, "Includes an invalid path.");
			}
			bool flag6 = path.IndexOf("//", StringComparison.Ordinal) != -1;
			if (flag6)
			{
				throw new HttpListenerException(87, "Includes an invalid path.");
			}
			IPEndPoint ipendPoint = new IPEndPoint(ipaddress, num);
			EndPointListener endPointListener;
			bool flag7 = EndPointManager._endpoints.TryGetValue(ipendPoint, out endPointListener);
			if (flag7)
			{
				bool flag8 = endPointListener.IsSecure ^ httpListenerPrefix.IsSecure;
				if (flag8)
				{
					throw new HttpListenerException(87, "Includes an invalid scheme.");
				}
			}
			else
			{
				endPointListener = new EndPointListener(ipendPoint, httpListenerPrefix.IsSecure, listener.CertificateFolderPath, listener.SslConfiguration, listener.ReuseAddress);
				EndPointManager._endpoints.Add(ipendPoint, endPointListener);
			}
			endPointListener.AddPrefix(httpListenerPrefix, listener);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000E308 File Offset: 0x0000C508
		private static IPAddress convertToIPAddress(string hostname)
		{
			bool flag = hostname == "*";
			IPAddress result;
			if (flag)
			{
				result = IPAddress.Any;
			}
			else
			{
				bool flag2 = hostname == "+";
				if (flag2)
				{
					result = IPAddress.Any;
				}
				else
				{
					result = hostname.ToIPAddress();
				}
			}
			return result;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000E350 File Offset: 0x0000C550
		private static void removePrefix(string uriPrefix, HttpListener listener)
		{
			HttpListenerPrefix httpListenerPrefix = new HttpListenerPrefix(uriPrefix);
			IPAddress ipaddress = EndPointManager.convertToIPAddress(httpListenerPrefix.Host);
			bool flag = ipaddress == null;
			if (!flag)
			{
				bool flag2 = !ipaddress.IsLocal();
				if (!flag2)
				{
					int num;
					bool flag3 = !int.TryParse(httpListenerPrefix.Port, out num);
					if (!flag3)
					{
						bool flag4 = !num.IsPortNumber();
						if (!flag4)
						{
							string path = httpListenerPrefix.Path;
							bool flag5 = path.IndexOf('%') != -1;
							if (!flag5)
							{
								bool flag6 = path.IndexOf("//", StringComparison.Ordinal) != -1;
								if (!flag6)
								{
									IPEndPoint key = new IPEndPoint(ipaddress, num);
									EndPointListener endPointListener;
									bool flag7 = !EndPointManager._endpoints.TryGetValue(key, out endPointListener);
									if (!flag7)
									{
										bool flag8 = endPointListener.IsSecure ^ httpListenerPrefix.IsSecure;
										if (!flag8)
										{
											endPointListener.RemovePrefix(httpListenerPrefix, listener);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000E43C File Offset: 0x0000C63C
		internal static bool RemoveEndPoint(IPEndPoint endpoint)
		{
			object syncRoot = ((ICollection)EndPointManager._endpoints).SyncRoot;
			bool result;
			lock (syncRoot)
			{
				EndPointListener endPointListener;
				bool flag = !EndPointManager._endpoints.TryGetValue(endpoint, out endPointListener);
				if (flag)
				{
					result = false;
				}
				else
				{
					EndPointManager._endpoints.Remove(endpoint);
					endPointListener.Close();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000E4A8 File Offset: 0x0000C6A8
		public static void AddListener(HttpListener listener)
		{
			List<string> list = new List<string>();
			object syncRoot = ((ICollection)EndPointManager._endpoints).SyncRoot;
			lock (syncRoot)
			{
				try
				{
					foreach (string text in listener.Prefixes)
					{
						EndPointManager.addPrefix(text, listener);
						list.Add(text);
					}
				}
				catch
				{
					foreach (string uriPrefix in list)
					{
						EndPointManager.removePrefix(uriPrefix, listener);
					}
					throw;
				}
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000E58C File Offset: 0x0000C78C
		public static void AddPrefix(string uriPrefix, HttpListener listener)
		{
			object syncRoot = ((ICollection)EndPointManager._endpoints).SyncRoot;
			lock (syncRoot)
			{
				EndPointManager.addPrefix(uriPrefix, listener);
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000E5D0 File Offset: 0x0000C7D0
		public static void RemoveListener(HttpListener listener)
		{
			object syncRoot = ((ICollection)EndPointManager._endpoints).SyncRoot;
			lock (syncRoot)
			{
				foreach (string uriPrefix in listener.Prefixes)
				{
					EndPointManager.removePrefix(uriPrefix, listener);
				}
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000E64C File Offset: 0x0000C84C
		public static void RemovePrefix(string uriPrefix, HttpListener listener)
		{
			object syncRoot = ((ICollection)EndPointManager._endpoints).SyncRoot;
			lock (syncRoot)
			{
				EndPointManager.removePrefix(uriPrefix, listener);
			}
		}

		// Token: 0x040000C4 RID: 196
		private static readonly Dictionary<IPEndPoint, EndPointListener> _endpoints = new Dictionary<IPEndPoint, EndPointListener>();
	}
}
