using System;
using System.IO;
using System.Security.Principal;
using System.Text;
using WebSocketSharp.Net;

namespace WebSocketSharp.Server
{
	// Token: 0x02000048 RID: 72
	public class HttpRequestEventArgs : EventArgs
	{
		// Token: 0x06000500 RID: 1280 RVA: 0x0001B8AB File Offset: 0x00019AAB
		internal HttpRequestEventArgs(HttpListenerContext context, string documentRootPath)
		{
			this._context = context;
			this._docRootPath = documentRootPath;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001B8C4 File Offset: 0x00019AC4
		public HttpListenerRequest Request
		{
			get
			{
				return this._context.Request;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0001B8E4 File Offset: 0x00019AE4
		public HttpListenerResponse Response
		{
			get
			{
				return this._context.Response;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x0001B904 File Offset: 0x00019B04
		public IPrincipal User
		{
			get
			{
				return this._context.User;
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001B924 File Offset: 0x00019B24
		private string createFilePath(string childPath)
		{
			childPath = childPath.TrimStart(new char[]
			{
				'/',
				'\\'
			});
			return new StringBuilder(this._docRootPath, 32).AppendFormat("/{0}", childPath).ToString().Replace('\\', '/');
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001B974 File Offset: 0x00019B74
		private static bool tryReadFile(string path, out byte[] contents)
		{
			contents = null;
			bool flag = !File.Exists(path);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				try
				{
					contents = File.ReadAllBytes(path);
				}
				catch
				{
					return false;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001B9C0 File Offset: 0x00019BC0
		public byte[] ReadFile(string path)
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
			byte[] result;
			HttpRequestEventArgs.tryReadFile(this.createFilePath(path), out result);
			return result;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001BA38 File Offset: 0x00019C38
		public bool TryReadFile(string path, out byte[] contents)
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
			return HttpRequestEventArgs.tryReadFile(this.createFilePath(path), out contents);
		}

		// Token: 0x04000240 RID: 576
		private HttpListenerContext _context;

		// Token: 0x04000241 RID: 577
		private string _docRootPath;
	}
}
