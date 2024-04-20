using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using WebSocketSharp.Net;

namespace WebSocketSharp
{
	// Token: 0x02000017 RID: 23
	internal class HttpResponse : HttpBase
	{
		// Token: 0x06000196 RID: 406 RVA: 0x0000B1C7 File Offset: 0x000093C7
		private HttpResponse(string code, string reason, Version version, NameValueCollection headers) : base(version, headers)
		{
			this._code = code;
			this._reason = reason;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000B1E2 File Offset: 0x000093E2
		internal HttpResponse(HttpStatusCode code) : this(code, code.GetDescription())
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000B1F4 File Offset: 0x000093F4
		internal HttpResponse(HttpStatusCode code, string reason)
		{
			int num = (int)code;
			this..ctor(num.ToString(), reason, HttpVersion.Version11, new NameValueCollection());
			base.Headers["Server"] = "websocket-sharp/1.0";
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000B234 File Offset: 0x00009434
		public CookieCollection Cookies
		{
			get
			{
				return base.Headers.GetCookies(true);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000B254 File Offset: 0x00009454
		public bool HasConnectionClose
		{
			get
			{
				StringComparison comparisonTypeForValue = StringComparison.OrdinalIgnoreCase;
				return base.Headers.Contains("Connection", "close", comparisonTypeForValue);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000B280 File Offset: 0x00009480
		public bool IsProxyAuthenticationRequired
		{
			get
			{
				return this._code == "407";
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000B2A4 File Offset: 0x000094A4
		public bool IsRedirect
		{
			get
			{
				return this._code == "301" || this._code == "302";
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000B2DC File Offset: 0x000094DC
		public bool IsUnauthorized
		{
			get
			{
				return this._code == "401";
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000B300 File Offset: 0x00009500
		public bool IsWebSocketResponse
		{
			get
			{
				return base.ProtocolVersion > HttpVersion.Version10 && this._code == "101" && base.Headers.Upgrades("websocket");
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000B34C File Offset: 0x0000954C
		public string Reason
		{
			get
			{
				return this._reason;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000B364 File Offset: 0x00009564
		public string StatusCode
		{
			get
			{
				return this._code;
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000B37C File Offset: 0x0000957C
		internal static HttpResponse CreateCloseResponse(HttpStatusCode code)
		{
			HttpResponse httpResponse = new HttpResponse(code);
			httpResponse.Headers["Connection"] = "close";
			return httpResponse;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000B3AC File Offset: 0x000095AC
		internal static HttpResponse CreateUnauthorizedResponse(string challenge)
		{
			HttpResponse httpResponse = new HttpResponse(HttpStatusCode.Unauthorized);
			httpResponse.Headers["WWW-Authenticate"] = challenge;
			return httpResponse;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000B3DC File Offset: 0x000095DC
		internal static HttpResponse CreateWebSocketResponse()
		{
			HttpResponse httpResponse = new HttpResponse(HttpStatusCode.SwitchingProtocols);
			NameValueCollection headers = httpResponse.Headers;
			headers["Upgrade"] = "websocket";
			headers["Connection"] = "Upgrade";
			return httpResponse;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000B420 File Offset: 0x00009620
		internal static HttpResponse Parse(string[] headerParts)
		{
			string[] array = headerParts[0].Split(new char[]
			{
				' '
			}, 3);
			bool flag = array.Length != 3;
			if (flag)
			{
				throw new ArgumentException("Invalid status line: " + headerParts[0]);
			}
			WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
			for (int i = 1; i < headerParts.Length; i++)
			{
				webHeaderCollection.InternalSet(headerParts[i], true);
			}
			return new HttpResponse(array[1], array[2], new Version(array[0].Substring(5)), webHeaderCollection);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000B4A8 File Offset: 0x000096A8
		internal static HttpResponse Read(Stream stream, int millisecondsTimeout)
		{
			return HttpBase.Read<HttpResponse>(stream, new Func<string[], HttpResponse>(HttpResponse.Parse), millisecondsTimeout);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000B4D0 File Offset: 0x000096D0
		public void SetCookies(CookieCollection cookies)
		{
			bool flag = cookies == null || cookies.Count == 0;
			if (!flag)
			{
				NameValueCollection headers = base.Headers;
				foreach (Cookie cookie in cookies.Sorted)
				{
					headers.Add("Set-Cookie", cookie.ToResponseString());
				}
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000B548 File Offset: 0x00009748
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("HTTP/{0} {1} {2}{3}", new object[]
			{
				base.ProtocolVersion,
				this._code,
				this._reason,
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

		// Token: 0x04000097 RID: 151
		private string _code;

		// Token: 0x04000098 RID: 152
		private string _reason;
	}
}
