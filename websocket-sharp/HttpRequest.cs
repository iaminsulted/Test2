using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using WebSocketSharp.Net;

namespace WebSocketSharp
{
	// Token: 0x02000016 RID: 22
	internal class HttpRequest : HttpBase
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000AD26 File Offset: 0x00008F26
		private HttpRequest(string method, string uri, Version version, NameValueCollection headers) : base(version, headers)
		{
			this._method = method;
			this._uri = uri;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000AD41 File Offset: 0x00008F41
		internal HttpRequest(string method, string uri) : this(method, uri, HttpVersion.Version11, new NameValueCollection())
		{
			base.Headers["User-Agent"] = "websocket-sharp/1.0";
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000AD70 File Offset: 0x00008F70
		public AuthenticationResponse AuthenticationResponse
		{
			get
			{
				string text = base.Headers["Authorization"];
				return (text != null && text.Length > 0) ? AuthenticationResponse.Parse(text) : null;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000ADA8 File Offset: 0x00008FA8
		public CookieCollection Cookies
		{
			get
			{
				bool flag = this._cookies == null;
				if (flag)
				{
					this._cookies = base.Headers.GetCookies(false);
				}
				return this._cookies;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		public string HttpMethod
		{
			get
			{
				return this._method;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000ADF8 File Offset: 0x00008FF8
		public bool IsWebSocketRequest
		{
			get
			{
				return this._method == "GET" && base.ProtocolVersion > HttpVersion.Version10 && base.Headers.Upgrades("websocket");
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0000AE44 File Offset: 0x00009044
		public string RequestUri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000AE5C File Offset: 0x0000905C
		internal static HttpRequest CreateConnectRequest(Uri uri)
		{
			string dnsSafeHost = uri.DnsSafeHost;
			int port = uri.Port;
			string text = string.Format("{0}:{1}", dnsSafeHost, port);
			HttpRequest httpRequest = new HttpRequest("CONNECT", text);
			httpRequest.Headers["Host"] = ((port == 80) ? dnsSafeHost : text);
			return httpRequest;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000AEB8 File Offset: 0x000090B8
		internal static HttpRequest CreateWebSocketRequest(Uri uri)
		{
			HttpRequest httpRequest = new HttpRequest("GET", uri.PathAndQuery);
			NameValueCollection headers = httpRequest.Headers;
			int port = uri.Port;
			string scheme = uri.Scheme;
			headers["Host"] = (((port == 80 && scheme == "ws") || (port == 443 && scheme == "wss")) ? uri.DnsSafeHost : uri.Authority);
			headers["Upgrade"] = "websocket";
			headers["Connection"] = "Upgrade";
			return httpRequest;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000AF58 File Offset: 0x00009158
		internal HttpResponse GetResponse(Stream stream, int millisecondsTimeout)
		{
			byte[] array = base.ToByteArray();
			stream.Write(array, 0, array.Length);
			return HttpBase.Read<HttpResponse>(stream, new Func<string[], HttpResponse>(HttpResponse.Parse), millisecondsTimeout);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000AF90 File Offset: 0x00009190
		internal static HttpRequest Parse(string[] headerParts)
		{
			string[] array = headerParts[0].Split(new char[]
			{
				' '
			}, 3);
			bool flag = array.Length != 3;
			if (flag)
			{
				throw new ArgumentException("Invalid request line: " + headerParts[0]);
			}
			WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
			for (int i = 1; i < headerParts.Length; i++)
			{
				webHeaderCollection.InternalSet(headerParts[i], false);
			}
			return new HttpRequest(array[0], array[1], new Version(array[2].Substring(5)), webHeaderCollection);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000B018 File Offset: 0x00009218
		internal static HttpRequest Read(Stream stream, int millisecondsTimeout)
		{
			return HttpBase.Read<HttpRequest>(stream, new Func<string[], HttpRequest>(HttpRequest.Parse), millisecondsTimeout);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000B040 File Offset: 0x00009240
		public void SetCookies(CookieCollection cookies)
		{
			bool flag = cookies == null || cookies.Count == 0;
			if (!flag)
			{
				StringBuilder stringBuilder = new StringBuilder(64);
				foreach (Cookie cookie in cookies.Sorted)
				{
					bool flag2 = !cookie.Expired;
					if (flag2)
					{
						stringBuilder.AppendFormat("{0}; ", cookie.ToString());
					}
				}
				int length = stringBuilder.Length;
				bool flag3 = length > 2;
				if (flag3)
				{
					stringBuilder.Length = length - 2;
					base.Headers["Cookie"] = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000B104 File Offset: 0x00009304
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("{0} {1} HTTP/{2}{3}", new object[]
			{
				this._method,
				this._uri,
				base.ProtocolVersion,
				"\r\n"
			});
			NameValueCollection headers = base.Headers;
			foreach (string text in headers.AllKeys)
			{
				stringBuilder.AppendFormat("{0}: {1}{2}", text, headers[text], "\r\n");
			}
			stringBuilder.Append("\r\n");
			string entityBody = base.EntityBody;
			bool flag = entityBody.Length > 0;
			if (flag)
			{
				stringBuilder.Append(entityBody);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000094 RID: 148
		private CookieCollection _cookies;

		// Token: 0x04000095 RID: 149
		private string _method;

		// Token: 0x04000096 RID: 150
		private string _uri;
	}
}
