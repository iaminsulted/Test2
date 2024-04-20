using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading;
using WebSocketSharp.Net;

namespace WebSocketSharp
{
	// Token: 0x02000015 RID: 21
	internal abstract class HttpBase
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0000A9C4 File Offset: 0x00008BC4
		protected HttpBase(Version version, NameValueCollection headers)
		{
			this._version = version;
			this._headers = headers;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000A9DC File Offset: 0x00008BDC
		public string EntityBody
		{
			get
			{
				bool flag = this.EntityBodyData == null || (long)this.EntityBodyData.Length == 0L;
				string result;
				if (flag)
				{
					result = string.Empty;
				}
				else
				{
					Encoding encoding = null;
					string text = this._headers["Content-Type"];
					bool flag2 = text != null && text.Length > 0;
					if (flag2)
					{
						encoding = HttpUtility.GetEncoding(text);
					}
					result = (encoding ?? Encoding.UTF8).GetString(this.EntityBodyData);
				}
				return result;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000AA58 File Offset: 0x00008C58
		public NameValueCollection Headers
		{
			get
			{
				return this._headers;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000AA70 File Offset: 0x00008C70
		public Version ProtocolVersion
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000AA88 File Offset: 0x00008C88
		private static byte[] readEntityBody(Stream stream, string length)
		{
			long num;
			bool flag = !long.TryParse(length, out num);
			if (flag)
			{
				throw new ArgumentException("Cannot be parsed.", "length");
			}
			bool flag2 = num < 0L;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("length", "Less than zero.");
			}
			return (num > 1024L) ? stream.ReadBytes(num, 1024) : ((num > 0L) ? stream.ReadBytes((int)num) : null);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000AAFC File Offset: 0x00008CFC
		private static string[] readHeaders(Stream stream, int maxLength)
		{
			List<byte> buff = new List<byte>();
			int cnt = 0;
			Action<int> action = delegate(int i)
			{
				bool flag4 = i == -1;
				if (flag4)
				{
					throw new EndOfStreamException("The header cannot be read from the data source.");
				}
				buff.Add((byte)i);
				int cnt = cnt;
				cnt++;
			};
			bool flag = false;
			while (cnt < maxLength)
			{
				bool flag2 = stream.ReadByte().EqualsWith('\r', action) && stream.ReadByte().EqualsWith('\n', action) && stream.ReadByte().EqualsWith('\r', action) && stream.ReadByte().EqualsWith('\n', action);
				if (flag2)
				{
					flag = true;
					break;
				}
			}
			bool flag3 = !flag;
			if (flag3)
			{
				throw new WebSocketException("The length of header part is greater than the max length.");
			}
			return Encoding.UTF8.GetString(buff.ToArray()).Replace("\r\n ", " ").Replace("\r\n\t", " ").Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000ABF0 File Offset: 0x00008DF0
		protected static T Read<T>(Stream stream, Func<string[], T> parser, int millisecondsTimeout) where T : HttpBase
		{
			bool timeout = false;
			Timer timer = new Timer(delegate(object state)
			{
				timeout = true;
				stream.Close();
			}, null, millisecondsTimeout, -1);
			T t = default(T);
			Exception ex = null;
			try
			{
				t = parser(HttpBase.readHeaders(stream, 8192));
				string text = t.Headers["Content-Length"];
				bool flag = text != null && text.Length > 0;
				if (flag)
				{
					t.EntityBodyData = HttpBase.readEntityBody(stream, text);
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			finally
			{
				timer.Change(-1, -1);
				timer.Dispose();
			}
			string text2 = timeout ? "A timeout has occurred while reading an HTTP request/response." : ((ex != null) ? "An exception has occurred while reading an HTTP request/response." : null);
			bool flag2 = text2 != null;
			if (flag2)
			{
				throw new WebSocketException(text2, ex);
			}
			return t;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000AD04 File Offset: 0x00008F04
		public byte[] ToByteArray()
		{
			return Encoding.UTF8.GetBytes(this.ToString());
		}

		// Token: 0x0400008F RID: 143
		private NameValueCollection _headers;

		// Token: 0x04000090 RID: 144
		private const int _headersMaxLength = 8192;

		// Token: 0x04000091 RID: 145
		private Version _version;

		// Token: 0x04000092 RID: 146
		internal byte[] EntityBodyData;

		// Token: 0x04000093 RID: 147
		protected const string CrLf = "\r\n";
	}
}
