using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x02000025 RID: 37
	public sealed class HttpListenerResponse : IDisposable
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0001117A File Offset: 0x0000F37A
		internal HttpListenerResponse(HttpListenerContext context)
		{
			this._context = context;
			this._keepAlive = true;
			this._statusCode = 200;
			this._statusDescription = "OK";
			this._version = HttpVersion.Version11;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002BC RID: 700 RVA: 0x000111B4 File Offset: 0x0000F3B4
		// (set) Token: 0x060002BD RID: 701 RVA: 0x000111CC File Offset: 0x0000F3CC
		internal bool CloseConnection
		{
			get
			{
				return this._closeConnection;
			}
			set
			{
				this._closeConnection = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002BE RID: 702 RVA: 0x000111D8 File Offset: 0x0000F3D8
		// (set) Token: 0x060002BF RID: 703 RVA: 0x000111F0 File Offset: 0x0000F3F0
		internal bool HeadersSent
		{
			get
			{
				return this._headersSent;
			}
			set
			{
				this._headersSent = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x000111FC File Offset: 0x0000F3FC
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x00011214 File Offset: 0x0000F414
		public Encoding ContentEncoding
		{
			get
			{
				return this._contentEncoding;
			}
			set
			{
				this.checkDisposed();
				this._contentEncoding = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x00011228 File Offset: 0x0000F428
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x00011240 File Offset: 0x0000F440
		public long ContentLength64
		{
			get
			{
				return this._contentLength;
			}
			set
			{
				this.checkDisposedOrHeadersSent();
				bool flag = value < 0L;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("Less than zero.", "value");
				}
				this._contentLength = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00011278 File Offset: 0x0000F478
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x00011290 File Offset: 0x0000F490
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
			set
			{
				this.checkDisposed();
				bool flag = value != null && value.Length == 0;
				if (flag)
				{
					throw new ArgumentException("An empty string.", "value");
				}
				this._contentType = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x000112D0 File Offset: 0x0000F4D0
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x000112FA File Offset: 0x0000F4FA
		public CookieCollection Cookies
		{
			get
			{
				CookieCollection result;
				if ((result = this._cookies) == null)
				{
					result = (this._cookies = new CookieCollection());
				}
				return result;
			}
			set
			{
				this._cookies = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x00011304 File Offset: 0x0000F504
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x00011330 File Offset: 0x0000F530
		public WebHeaderCollection Headers
		{
			get
			{
				WebHeaderCollection result;
				if ((result = this._headers) == null)
				{
					result = (this._headers = new WebHeaderCollection(HttpHeaderType.Response, false));
				}
				return result;
			}
			set
			{
				bool flag = value != null && value.State != HttpHeaderType.Response;
				if (flag)
				{
					throw new InvalidOperationException("The specified headers aren't valid for a response.");
				}
				this._headers = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00011368 File Offset: 0x0000F568
		// (set) Token: 0x060002CB RID: 715 RVA: 0x00011380 File Offset: 0x0000F580
		public bool KeepAlive
		{
			get
			{
				return this._keepAlive;
			}
			set
			{
				this.checkDisposedOrHeadersSent();
				this._keepAlive = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00011394 File Offset: 0x0000F594
		public Stream OutputStream
		{
			get
			{
				this.checkDisposed();
				ResponseStream result;
				if ((result = this._outputStream) == null)
				{
					result = (this._outputStream = this._context.Connection.GetResponseStream());
				}
				return result;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002CD RID: 717 RVA: 0x000113D0 File Offset: 0x0000F5D0
		// (set) Token: 0x060002CE RID: 718 RVA: 0x000113E8 File Offset: 0x0000F5E8
		public Version ProtocolVersion
		{
			get
			{
				return this._version;
			}
			set
			{
				this.checkDisposedOrHeadersSent();
				bool flag = value == null;
				if (flag)
				{
					throw new ArgumentNullException("value");
				}
				bool flag2 = value.Major != 1 || (value.Minor != 0 && value.Minor != 1);
				if (flag2)
				{
					throw new ArgumentException("Not 1.0 or 1.1.", "value");
				}
				this._version = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00011454 File Offset: 0x0000F654
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0001146C File Offset: 0x0000F66C
		public string RedirectLocation
		{
			get
			{
				return this._location;
			}
			set
			{
				this.checkDisposed();
				bool flag = value == null;
				if (flag)
				{
					this._location = null;
				}
				else
				{
					Uri uri = null;
					bool flag2 = !value.MaybeUri() || !Uri.TryCreate(value, UriKind.Absolute, out uri);
					if (flag2)
					{
						throw new ArgumentException("Not an absolute URL.", "value");
					}
					this._location = value;
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x000114C8 File Offset: 0x0000F6C8
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x000114E0 File Offset: 0x0000F6E0
		public bool SendChunked
		{
			get
			{
				return this._sendChunked;
			}
			set
			{
				this.checkDisposedOrHeadersSent();
				this._sendChunked = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x000114F4 File Offset: 0x0000F6F4
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x0001150C File Offset: 0x0000F70C
		public int StatusCode
		{
			get
			{
				return this._statusCode;
			}
			set
			{
				this.checkDisposedOrHeadersSent();
				bool flag = value < 100 || value > 999;
				if (flag)
				{
					throw new ProtocolViolationException("A value isn't between 100 and 999 inclusive.");
				}
				this._statusCode = value;
				this._statusDescription = value.GetStatusDescription();
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00011554 File Offset: 0x0000F754
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0001156C File Offset: 0x0000F76C
		public string StatusDescription
		{
			get
			{
				return this._statusDescription;
			}
			set
			{
				this.checkDisposedOrHeadersSent();
				bool flag = value == null || value.Length == 0;
				if (flag)
				{
					this._statusDescription = this._statusCode.GetStatusDescription();
				}
				else
				{
					bool flag2 = !value.IsText() || value.IndexOfAny(new char[]
					{
						'\r',
						'\n'
					}) > -1;
					if (flag2)
					{
						throw new ArgumentException("Contains invalid characters.", "value");
					}
					this._statusDescription = value;
				}
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000115E8 File Offset: 0x0000F7E8
		private bool canAddOrUpdate(Cookie cookie)
		{
			bool flag = this._cookies == null || this._cookies.Count == 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				List<Cookie> list = this.findCookie(cookie).ToList<Cookie>();
				bool flag2 = list.Count == 0;
				if (flag2)
				{
					result = true;
				}
				else
				{
					int version = cookie.Version;
					foreach (Cookie cookie2 in list)
					{
						bool flag3 = cookie2.Version == version;
						if (flag3)
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00011698 File Offset: 0x0000F898
		private void checkDisposed()
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x000116C4 File Offset: 0x0000F8C4
		private void checkDisposedOrHeadersSent()
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			bool headersSent = this._headersSent;
			if (headersSent)
			{
				throw new InvalidOperationException("Cannot be changed after the headers are sent.");
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00011702 File Offset: 0x0000F902
		private void close(bool force)
		{
			this._disposed = true;
			this._context.Connection.Close(force);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001171E File Offset: 0x0000F91E
		private IEnumerable<Cookie> findCookie(Cookie cookie)
		{
			string name = cookie.Name;
			string domain = cookie.Domain;
			string path = cookie.Path;
			bool flag = this._cookies != null;
			if (flag)
			{
				foreach (object obj in this._cookies)
				{
					Cookie c = (Cookie)obj;
					bool flag2 = c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase) && c.Path.Equals(path, StringComparison.Ordinal);
					if (flag2)
					{
						yield return c;
					}
					c = null;
				}
				IEnumerator enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00011738 File Offset: 0x0000F938
		internal WebHeaderCollection WriteHeadersTo(MemoryStream destination)
		{
			WebHeaderCollection webHeaderCollection = new WebHeaderCollection(HttpHeaderType.Response, true);
			bool flag = this._headers != null;
			if (flag)
			{
				webHeaderCollection.Add(this._headers);
			}
			bool flag2 = this._contentType != null;
			if (flag2)
			{
				string value = (this._contentType.IndexOf("charset=", StringComparison.Ordinal) == -1 && this._contentEncoding != null) ? string.Format("{0}; charset={1}", this._contentType, this._contentEncoding.WebName) : this._contentType;
				webHeaderCollection.InternalSet("Content-Type", value, true);
			}
			bool flag3 = webHeaderCollection["Server"] == null;
			if (flag3)
			{
				webHeaderCollection.InternalSet("Server", "websocket-sharp/1.0", true);
			}
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			bool flag4 = webHeaderCollection["Date"] == null;
			if (flag4)
			{
				webHeaderCollection.InternalSet("Date", DateTime.UtcNow.ToString("r", invariantCulture), true);
			}
			bool flag5 = !this._sendChunked;
			if (flag5)
			{
				webHeaderCollection.InternalSet("Content-Length", this._contentLength.ToString(invariantCulture), true);
			}
			else
			{
				webHeaderCollection.InternalSet("Transfer-Encoding", "chunked", true);
			}
			bool flag6 = !this._context.Request.KeepAlive || !this._keepAlive || this._statusCode == 400 || this._statusCode == 408 || this._statusCode == 411 || this._statusCode == 413 || this._statusCode == 414 || this._statusCode == 500 || this._statusCode == 503;
			int reuses = this._context.Connection.Reuses;
			bool flag7 = flag6 || reuses >= 100;
			if (flag7)
			{
				webHeaderCollection.InternalSet("Connection", "close", true);
			}
			else
			{
				webHeaderCollection.InternalSet("Keep-Alive", string.Format("timeout=15,max={0}", 100 - reuses), true);
				bool flag8 = this._context.Request.ProtocolVersion < HttpVersion.Version11;
				if (flag8)
				{
					webHeaderCollection.InternalSet("Connection", "keep-alive", true);
				}
			}
			bool flag9 = this._location != null;
			if (flag9)
			{
				webHeaderCollection.InternalSet("Location", this._location, true);
			}
			bool flag10 = this._cookies != null;
			if (flag10)
			{
				foreach (object obj in this._cookies)
				{
					Cookie cookie = (Cookie)obj;
					webHeaderCollection.InternalSet("Set-Cookie", cookie.ToResponseString(), true);
				}
			}
			Encoding encoding = this._contentEncoding ?? Encoding.Default;
			StreamWriter streamWriter = new StreamWriter(destination, encoding, 256);
			streamWriter.Write("HTTP/{0} {1} {2}\r\n", this._version, this._statusCode, this._statusDescription);
			streamWriter.Write(webHeaderCollection.ToStringMultiValue(true));
			streamWriter.Flush();
			destination.Position = (long)encoding.GetPreamble().Length;
			return webHeaderCollection;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00011A78 File Offset: 0x0000FC78
		public void Abort()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.close(true);
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00011A9A File Offset: 0x0000FC9A
		public void AddHeader(string name, string value)
		{
			this.Headers.Set(name, value);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00011AAB File Offset: 0x0000FCAB
		public void AppendCookie(Cookie cookie)
		{
			this.Cookies.Add(cookie);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00011ABB File Offset: 0x0000FCBB
		public void AppendHeader(string name, string value)
		{
			this.Headers.Add(name, value);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00011ACC File Offset: 0x0000FCCC
		public void Close()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.close(false);
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00011AF0 File Offset: 0x0000FCF0
		public void Close(byte[] responseEntity, bool willBlock)
		{
			this.checkDisposed();
			bool flag = responseEntity == null;
			if (flag)
			{
				throw new ArgumentNullException("responseEntity");
			}
			int count = responseEntity.Length;
			Stream output = this.OutputStream;
			if (willBlock)
			{
				output.Write(responseEntity, 0, count);
				this.close(false);
			}
			else
			{
				output.BeginWrite(responseEntity, 0, count, delegate(IAsyncResult ar)
				{
					output.EndWrite(ar);
					this.close(false);
				}, null);
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00011B70 File Offset: 0x0000FD70
		public void CopyFrom(HttpListenerResponse templateResponse)
		{
			bool flag = templateResponse == null;
			if (flag)
			{
				throw new ArgumentNullException("templateResponse");
			}
			bool flag2 = templateResponse._headers != null;
			if (flag2)
			{
				bool flag3 = this._headers != null;
				if (flag3)
				{
					this._headers.Clear();
				}
				this.Headers.Add(templateResponse._headers);
			}
			else
			{
				bool flag4 = this._headers != null;
				if (flag4)
				{
					this._headers = null;
				}
			}
			this._contentLength = templateResponse._contentLength;
			this._statusCode = templateResponse._statusCode;
			this._statusDescription = templateResponse._statusDescription;
			this._keepAlive = templateResponse._keepAlive;
			this._version = templateResponse._version;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00011C20 File Offset: 0x0000FE20
		public void Redirect(string url)
		{
			this.checkDisposedOrHeadersSent();
			bool flag = url == null;
			if (flag)
			{
				throw new ArgumentNullException("url");
			}
			Uri uri = null;
			bool flag2 = !url.MaybeUri() || !Uri.TryCreate(url, UriKind.Absolute, out uri);
			if (flag2)
			{
				throw new ArgumentException("Not an absolute URL.", "url");
			}
			this._location = url;
			this._statusCode = 302;
			this._statusDescription = "Found";
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00011C94 File Offset: 0x0000FE94
		public void SetCookie(Cookie cookie)
		{
			bool flag = cookie == null;
			if (flag)
			{
				throw new ArgumentNullException("cookie");
			}
			bool flag2 = !this.canAddOrUpdate(cookie);
			if (flag2)
			{
				throw new ArgumentException("Cannot be replaced.", "cookie");
			}
			this.Cookies.Add(cookie);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00011CE0 File Offset: 0x0000FEE0
		void IDisposable.Dispose()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.close(true);
			}
		}

		// Token: 0x0400010F RID: 271
		private bool _closeConnection;

		// Token: 0x04000110 RID: 272
		private Encoding _contentEncoding;

		// Token: 0x04000111 RID: 273
		private long _contentLength;

		// Token: 0x04000112 RID: 274
		private string _contentType;

		// Token: 0x04000113 RID: 275
		private HttpListenerContext _context;

		// Token: 0x04000114 RID: 276
		private CookieCollection _cookies;

		// Token: 0x04000115 RID: 277
		private bool _disposed;

		// Token: 0x04000116 RID: 278
		private WebHeaderCollection _headers;

		// Token: 0x04000117 RID: 279
		private bool _headersSent;

		// Token: 0x04000118 RID: 280
		private bool _keepAlive;

		// Token: 0x04000119 RID: 281
		private string _location;

		// Token: 0x0400011A RID: 282
		private ResponseStream _outputStream;

		// Token: 0x0400011B RID: 283
		private bool _sendChunked;

		// Token: 0x0400011C RID: 284
		private int _statusCode;

		// Token: 0x0400011D RID: 285
		private string _statusDescription;

		// Token: 0x0400011E RID: 286
		private Version _version;
	}
}
