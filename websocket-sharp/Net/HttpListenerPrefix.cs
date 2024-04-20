using System;

namespace WebSocketSharp.Net
{
	// Token: 0x0200003C RID: 60
	internal sealed class HttpListenerPrefix
	{
		// Token: 0x06000401 RID: 1025 RVA: 0x00017D49 File Offset: 0x00015F49
		internal HttpListenerPrefix(string uriPrefix)
		{
			this._original = uriPrefix;
			this.parse(uriPrefix);
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00017D64 File Offset: 0x00015F64
		public string Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00017D7C File Offset: 0x00015F7C
		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00017D94 File Offset: 0x00015F94
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00017DAC File Offset: 0x00015FAC
		public HttpListener Listener
		{
			get
			{
				return this._listener;
			}
			set
			{
				this._listener = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00017DB8 File Offset: 0x00015FB8
		public string Original
		{
			get
			{
				return this._original;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00017DD0 File Offset: 0x00015FD0
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00017DE8 File Offset: 0x00015FE8
		public string Port
		{
			get
			{
				return this._port;
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00017E00 File Offset: 0x00016000
		private void parse(string uriPrefix)
		{
			bool flag = uriPrefix.StartsWith("https");
			if (flag)
			{
				this._secure = true;
			}
			int length = uriPrefix.Length;
			int num = uriPrefix.IndexOf(':') + 3;
			int num2 = uriPrefix.IndexOf('/', num + 1, length - num - 1);
			int num3 = uriPrefix.LastIndexOf(':', num2 - 1, num2 - num - 1);
			bool flag2 = uriPrefix[num2 - 1] != ']' && num3 > num;
			if (flag2)
			{
				this._host = uriPrefix.Substring(num, num3 - num);
				this._port = uriPrefix.Substring(num3 + 1, num2 - num3 - 1);
			}
			else
			{
				this._host = uriPrefix.Substring(num, num2 - num);
				this._port = (this._secure ? "443" : "80");
			}
			this._path = uriPrefix.Substring(num2);
			this._prefix = string.Format("http{0}://{1}:{2}{3}", new object[]
			{
				this._secure ? "s" : "",
				this._host,
				this._port,
				this._path
			});
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00017F20 File Offset: 0x00016120
		public static void CheckPrefix(string uriPrefix)
		{
			bool flag = uriPrefix == null;
			if (flag)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			int length = uriPrefix.Length;
			bool flag2 = length == 0;
			if (flag2)
			{
				throw new ArgumentException("An empty string.", "uriPrefix");
			}
			bool flag3 = !uriPrefix.StartsWith("http://") && !uriPrefix.StartsWith("https://");
			if (flag3)
			{
				throw new ArgumentException("The scheme isn't 'http' or 'https'.", "uriPrefix");
			}
			int num = uriPrefix.IndexOf(':') + 3;
			bool flag4 = num >= length;
			if (flag4)
			{
				throw new ArgumentException("No host is specified.", "uriPrefix");
			}
			bool flag5 = uriPrefix[num] == ':';
			if (flag5)
			{
				throw new ArgumentException("No host is specified.", "uriPrefix");
			}
			int num2 = uriPrefix.IndexOf('/', num, length - num);
			bool flag6 = num2 == num;
			if (flag6)
			{
				throw new ArgumentException("No host is specified.", "uriPrefix");
			}
			bool flag7 = num2 == -1 || uriPrefix[length - 1] != '/';
			if (flag7)
			{
				throw new ArgumentException("Ends without '/'.", "uriPrefix");
			}
			bool flag8 = uriPrefix[num2 - 1] == ':';
			if (flag8)
			{
				throw new ArgumentException("No port is specified.", "uriPrefix");
			}
			bool flag9 = num2 == length - 2;
			if (flag9)
			{
				throw new ArgumentException("No path is specified.", "uriPrefix");
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00018074 File Offset: 0x00016274
		public override bool Equals(object obj)
		{
			HttpListenerPrefix httpListenerPrefix = obj as HttpListenerPrefix;
			return httpListenerPrefix != null && httpListenerPrefix._prefix == this._prefix;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000180A4 File Offset: 0x000162A4
		public override int GetHashCode()
		{
			return this._prefix.GetHashCode();
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000180C4 File Offset: 0x000162C4
		public override string ToString()
		{
			return this._prefix;
		}

		// Token: 0x0400019C RID: 412
		private string _host;

		// Token: 0x0400019D RID: 413
		private HttpListener _listener;

		// Token: 0x0400019E RID: 414
		private string _original;

		// Token: 0x0400019F RID: 415
		private string _path;

		// Token: 0x040001A0 RID: 416
		private string _port;

		// Token: 0x040001A1 RID: 417
		private string _prefix;

		// Token: 0x040001A2 RID: 418
		private bool _secure;
	}
}
