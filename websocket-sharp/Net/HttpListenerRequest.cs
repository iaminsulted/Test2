using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x02000024 RID: 36
	public sealed class HttpListenerRequest
	{
		// Token: 0x06000293 RID: 659 RVA: 0x00010529 File Offset: 0x0000E729
		internal HttpListenerRequest(HttpListenerContext context)
		{
			this._context = context;
			this._connection = context.Connection;
			this._contentLength = -1L;
			this._headers = new WebHeaderCollection();
			this._requestTraceIdentifier = Guid.NewGuid();
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00010564 File Offset: 0x0000E764
		public string[] AcceptTypes
		{
			get
			{
				string text = this._headers["Accept"];
				bool flag = text == null;
				string[] result;
				if (flag)
				{
					result = null;
				}
				else
				{
					bool flag2 = this._acceptTypes == null;
					if (flag2)
					{
						this._acceptTypes = text.SplitHeaderValue(new char[]
						{
							','
						}).Trim().ToList<string>().ToArray();
					}
					result = this._acceptTypes;
				}
				return result;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public int ClientCertificateError
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000296 RID: 662 RVA: 0x000105D0 File Offset: 0x0000E7D0
		public Encoding ContentEncoding
		{
			get
			{
				bool flag = this._contentEncoding == null;
				if (flag)
				{
					this._contentEncoding = (this.getContentEncoding() ?? Encoding.UTF8);
				}
				return this._contentEncoding;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0001060C File Offset: 0x0000E80C
		public long ContentLength64
		{
			get
			{
				return this._contentLength;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00010624 File Offset: 0x0000E824
		public string ContentType
		{
			get
			{
				return this._headers["Content-Type"];
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00010648 File Offset: 0x0000E848
		public CookieCollection Cookies
		{
			get
			{
				bool flag = this._cookies == null;
				if (flag)
				{
					this._cookies = this._headers.GetCookies(false);
				}
				return this._cookies;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00010680 File Offset: 0x0000E880
		public bool HasEntityBody
		{
			get
			{
				return this._contentLength > 0L || this._chunked;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600029B RID: 667 RVA: 0x000106A8 File Offset: 0x0000E8A8
		public NameValueCollection Headers
		{
			get
			{
				return this._headers;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600029C RID: 668 RVA: 0x000106C0 File Offset: 0x0000E8C0
		public string HttpMethod
		{
			get
			{
				return this._httpMethod;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600029D RID: 669 RVA: 0x000106D8 File Offset: 0x0000E8D8
		public Stream InputStream
		{
			get
			{
				bool flag = this._inputStream == null;
				if (flag)
				{
					this._inputStream = (this.getInputStream() ?? Stream.Null);
				}
				return this._inputStream;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00010714 File Offset: 0x0000E914
		public bool IsAuthenticated
		{
			get
			{
				return this._context.User != null;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00010734 File Offset: 0x0000E934
		public bool IsLocal
		{
			get
			{
				return this._connection.IsLocal;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00010754 File Offset: 0x0000E954
		public bool IsSecureConnection
		{
			get
			{
				return this._connection.IsSecure;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x00010774 File Offset: 0x0000E974
		public bool IsWebSocketRequest
		{
			get
			{
				return this._httpMethod == "GET" && this._protocolVersion > HttpVersion.Version10 && this._headers.Upgrades("websocket");
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x000107C0 File Offset: 0x0000E9C0
		public bool KeepAlive
		{
			get
			{
				return this._headers.KeepsAlive(this._protocolVersion);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x000107E4 File Offset: 0x0000E9E4
		public IPEndPoint LocalEndPoint
		{
			get
			{
				return this._connection.LocalEndPoint;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00010804 File Offset: 0x0000EA04
		public Version ProtocolVersion
		{
			get
			{
				return this._protocolVersion;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0001081C File Offset: 0x0000EA1C
		public NameValueCollection QueryString
		{
			get
			{
				bool flag = this._queryString == null;
				if (flag)
				{
					Uri url = this.Url;
					this._queryString = QueryStringCollection.Parse((url != null) ? url.Query : null, Encoding.UTF8);
				}
				return this._queryString;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0001086C File Offset: 0x0000EA6C
		public string RawUrl
		{
			get
			{
				return this._rawUrl;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00010884 File Offset: 0x0000EA84
		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return this._connection.RemoteEndPoint;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x000108A4 File Offset: 0x0000EAA4
		public Guid RequestTraceIdentifier
		{
			get
			{
				return this._requestTraceIdentifier;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x000108BC File Offset: 0x0000EABC
		public Uri Url
		{
			get
			{
				bool flag = !this._urlSet;
				if (flag)
				{
					this._url = HttpUtility.CreateRequestUrl(this._rawUrl, this._userHostName ?? this.UserHostAddress, this.IsWebSocketRequest, this.IsSecureConnection);
					this._urlSet = true;
				}
				return this._url;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00010918 File Offset: 0x0000EB18
		public Uri UrlReferrer
		{
			get
			{
				string text = this._headers["Referer"];
				bool flag = text == null;
				Uri result;
				if (flag)
				{
					result = null;
				}
				else
				{
					bool flag2 = this._urlReferrer == null;
					if (flag2)
					{
						this._urlReferrer = text.ToUri();
					}
					result = this._urlReferrer;
				}
				return result;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0001096C File Offset: 0x0000EB6C
		public string UserAgent
		{
			get
			{
				return this._headers["User-Agent"];
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00010990 File Offset: 0x0000EB90
		public string UserHostAddress
		{
			get
			{
				return this._connection.LocalEndPoint.ToString();
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002AD RID: 685 RVA: 0x000109B4 File Offset: 0x0000EBB4
		public string UserHostName
		{
			get
			{
				return this._userHostName;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002AE RID: 686 RVA: 0x000109CC File Offset: 0x0000EBCC
		public string[] UserLanguages
		{
			get
			{
				string text = this._headers["Accept-Language"];
				bool flag = text == null;
				string[] result;
				if (flag)
				{
					result = null;
				}
				else
				{
					bool flag2 = this._userLanguages == null;
					if (flag2)
					{
						this._userLanguages = text.Split(new char[]
						{
							','
						}).Trim().ToList<string>().ToArray();
					}
					result = this._userLanguages;
				}
				return result;
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00010A34 File Offset: 0x0000EC34
		private void finishInitialization10()
		{
			string text = this._headers["Transfer-Encoding"];
			bool flag = text != null;
			if (flag)
			{
				this._context.ErrorMessage = "Invalid Transfer-Encoding header";
			}
			else
			{
				bool flag2 = this._httpMethod == "POST";
				if (flag2)
				{
					bool flag3 = this._contentLength == -1L;
					if (flag3)
					{
						this._context.ErrorMessage = "Content-Length header required";
					}
					else
					{
						bool flag4 = this._contentLength == 0L;
						if (flag4)
						{
							this._context.ErrorMessage = "Invalid Content-Length header";
						}
					}
				}
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00010ACC File Offset: 0x0000ECCC
		private Encoding getContentEncoding()
		{
			string text = this._headers["Content-Type"];
			bool flag = text == null;
			Encoding result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Encoding encoding;
				HttpUtility.TryGetEncoding(text, out encoding);
				result = encoding;
			}
			return result;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00010B08 File Offset: 0x0000ED08
		private RequestStream getInputStream()
		{
			return (this._contentLength > 0L || this._chunked) ? this._connection.GetRequestStream(this._contentLength, this._chunked) : null;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00010B48 File Offset: 0x0000ED48
		internal void AddHeader(string headerField)
		{
			char c = headerField[0];
			bool flag = c == ' ' || c == '\t';
			if (flag)
			{
				this._context.ErrorMessage = "Invalid header field";
			}
			else
			{
				int num = headerField.IndexOf(':');
				bool flag2 = num < 1;
				if (flag2)
				{
					this._context.ErrorMessage = "Invalid header field";
				}
				else
				{
					string text = headerField.Substring(0, num).Trim();
					bool flag3 = text.Length == 0 || !text.IsToken();
					if (flag3)
					{
						this._context.ErrorMessage = "Invalid header name";
					}
					else
					{
						string text2 = (num < headerField.Length - 1) ? headerField.Substring(num + 1).Trim() : string.Empty;
						this._headers.InternalSet(text, text2, false);
						string a = text.ToLower(CultureInfo.InvariantCulture);
						bool flag4 = a == "host";
						if (flag4)
						{
							bool flag5 = this._userHostName != null;
							if (flag5)
							{
								this._context.ErrorMessage = "Invalid Host header";
							}
							else
							{
								bool flag6 = text2.Length == 0;
								if (flag6)
								{
									this._context.ErrorMessage = "Invalid Host header";
								}
								else
								{
									this._userHostName = text2;
								}
							}
						}
						else
						{
							bool flag7 = a == "content-length";
							if (flag7)
							{
								bool flag8 = this._contentLength > -1L;
								if (flag8)
								{
									this._context.ErrorMessage = "Invalid Content-Length header";
								}
								else
								{
									long num2;
									bool flag9 = !long.TryParse(text2, out num2);
									if (flag9)
									{
										this._context.ErrorMessage = "Invalid Content-Length header";
									}
									else
									{
										bool flag10 = num2 < 0L;
										if (flag10)
										{
											this._context.ErrorMessage = "Invalid Content-Length header";
										}
										else
										{
											this._contentLength = num2;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00010D20 File Offset: 0x0000EF20
		internal void FinishInitialization()
		{
			bool flag = this._protocolVersion == HttpVersion.Version10;
			if (flag)
			{
				this.finishInitialization10();
			}
			else
			{
				bool flag2 = this._userHostName == null;
				if (flag2)
				{
					this._context.ErrorMessage = "Host header required";
				}
				else
				{
					string text = this._headers["Transfer-Encoding"];
					bool flag3 = text != null;
					if (flag3)
					{
						StringComparison comparisonType = StringComparison.OrdinalIgnoreCase;
						bool flag4 = !text.Equals("chunked", comparisonType);
						if (flag4)
						{
							this._context.ErrorMessage = string.Empty;
							this._context.ErrorStatus = 501;
							return;
						}
						this._chunked = true;
					}
					bool flag5 = this._httpMethod == "POST" || this._httpMethod == "PUT";
					if (flag5)
					{
						bool flag6 = this._contentLength <= 0L && !this._chunked;
						if (flag6)
						{
							this._context.ErrorMessage = string.Empty;
							this._context.ErrorStatus = 411;
							return;
						}
					}
					string text2 = this._headers["Expect"];
					bool flag7 = text2 != null;
					if (flag7)
					{
						StringComparison comparisonType2 = StringComparison.OrdinalIgnoreCase;
						bool flag8 = !text2.Equals("100-continue", comparisonType2);
						if (flag8)
						{
							this._context.ErrorMessage = "Invalid Expect header";
						}
						else
						{
							ResponseStream responseStream = this._connection.GetResponseStream();
							responseStream.InternalWrite(HttpListenerRequest._100continue, 0, HttpListenerRequest._100continue.Length);
						}
					}
				}
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00010EB4 File Offset: 0x0000F0B4
		internal bool FlushInput()
		{
			Stream inputStream = this.InputStream;
			bool flag = inputStream == Stream.Null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				int num = 2048;
				bool flag2 = this._contentLength > 0L && this._contentLength < (long)num;
				if (flag2)
				{
					num = (int)this._contentLength;
				}
				byte[] buffer = new byte[num];
				for (;;)
				{
					try
					{
						IAsyncResult asyncResult = inputStream.BeginRead(buffer, 0, num, null, null);
						bool flag3 = !asyncResult.IsCompleted;
						if (flag3)
						{
							int millisecondsTimeout = 100;
							bool flag4 = !asyncResult.AsyncWaitHandle.WaitOne(millisecondsTimeout);
							if (flag4)
							{
								result = false;
								break;
							}
						}
						bool flag5 = inputStream.EndRead(asyncResult) <= 0;
						if (flag5)
						{
							result = true;
							break;
						}
					}
					catch
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00010F90 File Offset: 0x0000F190
		internal bool IsUpgradeRequest(string protocol)
		{
			return this._headers.Upgrades(protocol);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		internal void SetRequestLine(string requestLine)
		{
			string[] array = requestLine.Split(new char[]
			{
				' '
			}, 3);
			bool flag = array.Length < 3;
			if (flag)
			{
				this._context.ErrorMessage = "Invalid request line (parts)";
			}
			else
			{
				string text = array[0];
				bool flag2 = text.Length == 0;
				if (flag2)
				{
					this._context.ErrorMessage = "Invalid request line (method)";
				}
				else
				{
					string text2 = array[1];
					bool flag3 = text2.Length == 0;
					if (flag3)
					{
						this._context.ErrorMessage = "Invalid request line (target)";
					}
					else
					{
						string text3 = array[2];
						bool flag4 = text3.Length != 8;
						if (flag4)
						{
							this._context.ErrorMessage = "Invalid request line (version)";
						}
						else
						{
							bool flag5 = text3.IndexOf("HTTP/") != 0;
							if (flag5)
							{
								this._context.ErrorMessage = "Invalid request line (version)";
							}
							else
							{
								Version version;
								bool flag6 = !text3.Substring(5).TryCreateVersion(out version);
								if (flag6)
								{
									this._context.ErrorMessage = "Invalid request line (version)";
								}
								else
								{
									bool flag7 = version.Major < 1;
									if (flag7)
									{
										this._context.ErrorMessage = "Invalid request line (version)";
									}
									else
									{
										bool flag8 = !text.IsHttpMethod(version);
										if (flag8)
										{
											this._context.ErrorMessage = "Invalid request line (method)";
										}
										else
										{
											this._httpMethod = text;
											this._rawUrl = text2;
											this._protocolVersion = version;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		public X509Certificate2 GetClientCertificate()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0001112C File Offset: 0x0000F32C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("{0} {1} HTTP/{2}\r\n", this._httpMethod, this._rawUrl, this._protocolVersion).Append(this._headers.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x040000FB RID: 251
		private static readonly byte[] _100continue = Encoding.ASCII.GetBytes("HTTP/1.1 100 Continue\r\n\r\n");

		// Token: 0x040000FC RID: 252
		private string[] _acceptTypes;

		// Token: 0x040000FD RID: 253
		private bool _chunked;

		// Token: 0x040000FE RID: 254
		private HttpConnection _connection;

		// Token: 0x040000FF RID: 255
		private Encoding _contentEncoding;

		// Token: 0x04000100 RID: 256
		private long _contentLength;

		// Token: 0x04000101 RID: 257
		private HttpListenerContext _context;

		// Token: 0x04000102 RID: 258
		private CookieCollection _cookies;

		// Token: 0x04000103 RID: 259
		private WebHeaderCollection _headers;

		// Token: 0x04000104 RID: 260
		private string _httpMethod;

		// Token: 0x04000105 RID: 261
		private Stream _inputStream;

		// Token: 0x04000106 RID: 262
		private Version _protocolVersion;

		// Token: 0x04000107 RID: 263
		private NameValueCollection _queryString;

		// Token: 0x04000108 RID: 264
		private string _rawUrl;

		// Token: 0x04000109 RID: 265
		private Guid _requestTraceIdentifier;

		// Token: 0x0400010A RID: 266
		private Uri _url;

		// Token: 0x0400010B RID: 267
		private Uri _urlReferrer;

		// Token: 0x0400010C RID: 268
		private bool _urlSet;

		// Token: 0x0400010D RID: 269
		private string _userHostName;

		// Token: 0x0400010E RID: 270
		private string[] _userLanguages;
	}
}
