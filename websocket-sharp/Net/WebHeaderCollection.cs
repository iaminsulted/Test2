using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x0200002A RID: 42
	[ComVisible(true)]
	[Serializable]
	public class WebHeaderCollection : NameValueCollection, ISerializable
	{
		// Token: 0x06000358 RID: 856 RVA: 0x0001589A File Offset: 0x00013A9A
		internal WebHeaderCollection(HttpHeaderType state, bool internallyUsed)
		{
			this._state = state;
			this._internallyUsed = internallyUsed;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000158B4 File Offset: 0x00013AB4
		protected WebHeaderCollection(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			bool flag = serializationInfo == null;
			if (flag)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			try
			{
				this._internallyUsed = serializationInfo.GetBoolean("InternallyUsed");
				this._state = (HttpHeaderType)serializationInfo.GetInt32("State");
				int @int = serializationInfo.GetInt32("Count");
				for (int i = 0; i < @int; i++)
				{
					base.Add(serializationInfo.GetString(i.ToString()), serializationInfo.GetString((@int + i).ToString()));
				}
			}
			catch (SerializationException ex)
			{
				throw new ArgumentException(ex.Message, "serializationInfo", ex);
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00015970 File Offset: 0x00013B70
		public WebHeaderCollection()
		{
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0001597C File Offset: 0x00013B7C
		internal HttpHeaderType State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00015994 File Offset: 0x00013B94
		public override string[] AllKeys
		{
			get
			{
				return base.AllKeys;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600035D RID: 861 RVA: 0x000159AC File Offset: 0x00013BAC
		public override int Count
		{
			get
			{
				return base.Count;
			}
		}

		// Token: 0x170000DA RID: 218
		public string this[HttpRequestHeader header]
		{
			get
			{
				return this.Get(WebHeaderCollection.Convert(header));
			}
			set
			{
				this.Add(header, value);
			}
		}

		// Token: 0x170000DB RID: 219
		public string this[HttpResponseHeader header]
		{
			get
			{
				return this.Get(WebHeaderCollection.Convert(header));
			}
			set
			{
				this.Add(header, value);
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00015A1C File Offset: 0x00013C1C
		public override NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				return base.Keys;
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00015A34 File Offset: 0x00013C34
		private void add(string name, string value, bool ignoreRestricted)
		{
			Action<string, string> action = ignoreRestricted ? new Action<string, string>(this.addWithoutCheckingNameAndRestricted) : new Action<string, string>(this.addWithoutCheckingName);
			this.doWithCheckingState(action, WebHeaderCollection.checkName(name), value, true);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00015A70 File Offset: 0x00013C70
		private void addWithoutCheckingName(string name, string value)
		{
			this.doWithoutCheckingName(new Action<string, string>(base.Add), name, value);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00015A88 File Offset: 0x00013C88
		private void addWithoutCheckingNameAndRestricted(string name, string value)
		{
			base.Add(name, WebHeaderCollection.checkValue(value));
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00015A9C File Offset: 0x00013C9C
		private static int checkColonSeparated(string header)
		{
			int num = header.IndexOf(':');
			bool flag = num == -1;
			if (flag)
			{
				throw new ArgumentException("No colon could be found.", "header");
			}
			return num;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00015AD0 File Offset: 0x00013CD0
		private static HttpHeaderType checkHeaderType(string name)
		{
			HttpHeaderInfo headerInfo = WebHeaderCollection.getHeaderInfo(name);
			return (headerInfo == null) ? HttpHeaderType.Unspecified : ((headerInfo.IsRequest && !headerInfo.IsResponse) ? HttpHeaderType.Request : ((!headerInfo.IsRequest && headerInfo.IsResponse) ? HttpHeaderType.Response : HttpHeaderType.Unspecified));
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00015B18 File Offset: 0x00013D18
		private static string checkName(string name)
		{
			bool flag = name == null || name.Length == 0;
			if (flag)
			{
				throw new ArgumentNullException("name");
			}
			name = name.Trim();
			bool flag2 = !WebHeaderCollection.IsHeaderName(name);
			if (flag2)
			{
				throw new ArgumentException("Contains invalid characters.", "name");
			}
			return name;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00015B70 File Offset: 0x00013D70
		private void checkRestricted(string name)
		{
			bool flag = !this._internallyUsed && WebHeaderCollection.isRestricted(name, true);
			if (flag)
			{
				throw new ArgumentException("This header must be modified with the appropiate property.");
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00015BA0 File Offset: 0x00013DA0
		private void checkState(bool response)
		{
			bool flag = this._state == HttpHeaderType.Unspecified;
			if (!flag)
			{
				bool flag2 = response && this._state == HttpHeaderType.Request;
				if (flag2)
				{
					throw new InvalidOperationException("This collection has already been used to store the request headers.");
				}
				bool flag3 = !response && this._state == HttpHeaderType.Response;
				if (flag3)
				{
					throw new InvalidOperationException("This collection has already been used to store the response headers.");
				}
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00015BFC File Offset: 0x00013DFC
		private static string checkValue(string value)
		{
			bool flag = value == null || value.Length == 0;
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				value = value.Trim();
				bool flag2 = value.Length > 65535;
				if (flag2)
				{
					throw new ArgumentOutOfRangeException("value", "Greater than 65,535 characters.");
				}
				bool flag3 = !WebHeaderCollection.IsHeaderValue(value);
				if (flag3)
				{
					throw new ArgumentException("Contains invalid characters.", "value");
				}
				result = value;
			}
			return result;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00015C70 File Offset: 0x00013E70
		private static string convert(string key)
		{
			HttpHeaderInfo httpHeaderInfo;
			return WebHeaderCollection._headers.TryGetValue(key, out httpHeaderInfo) ? httpHeaderInfo.Name : string.Empty;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00015CA0 File Offset: 0x00013EA0
		private void doWithCheckingState(Action<string, string> action, string name, string value, bool setState)
		{
			HttpHeaderType httpHeaderType = WebHeaderCollection.checkHeaderType(name);
			bool flag = httpHeaderType == HttpHeaderType.Request;
			if (flag)
			{
				this.doWithCheckingState(action, name, value, false, setState);
			}
			else
			{
				bool flag2 = httpHeaderType == HttpHeaderType.Response;
				if (flag2)
				{
					this.doWithCheckingState(action, name, value, true, setState);
				}
				else
				{
					action(name, value);
				}
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00015CEC File Offset: 0x00013EEC
		private void doWithCheckingState(Action<string, string> action, string name, string value, bool response, bool setState)
		{
			this.checkState(response);
			action(name, value);
			bool flag = setState && this._state == HttpHeaderType.Unspecified;
			if (flag)
			{
				this._state = (response ? HttpHeaderType.Response : HttpHeaderType.Request);
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00015D2E File Offset: 0x00013F2E
		private void doWithoutCheckingName(Action<string, string> action, string name, string value)
		{
			this.checkRestricted(name);
			action(name, WebHeaderCollection.checkValue(value));
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00015D48 File Offset: 0x00013F48
		private static HttpHeaderInfo getHeaderInfo(string name)
		{
			foreach (HttpHeaderInfo httpHeaderInfo in WebHeaderCollection._headers.Values)
			{
				bool flag = httpHeaderInfo.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase);
				if (flag)
				{
					return httpHeaderInfo;
				}
			}
			return null;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00015DB8 File Offset: 0x00013FB8
		private static bool isRestricted(string name, bool response)
		{
			HttpHeaderInfo headerInfo = WebHeaderCollection.getHeaderInfo(name);
			return headerInfo != null && headerInfo.IsRestricted(response);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00015DDE File Offset: 0x00013FDE
		private void removeWithoutCheckingName(string name, string unuse)
		{
			this.checkRestricted(name);
			base.Remove(name);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00015DF1 File Offset: 0x00013FF1
		private void setWithoutCheckingName(string name, string value)
		{
			this.doWithoutCheckingName(new Action<string, string>(base.Set), name, value);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00015E0C File Offset: 0x0001400C
		internal static string Convert(HttpRequestHeader header)
		{
			return WebHeaderCollection.convert(header.ToString());
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00015E30 File Offset: 0x00014030
		internal static string Convert(HttpResponseHeader header)
		{
			return WebHeaderCollection.convert(header.ToString());
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00015E54 File Offset: 0x00014054
		internal void InternalRemove(string name)
		{
			base.Remove(name);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00015E60 File Offset: 0x00014060
		internal void InternalSet(string header, bool response)
		{
			int num = WebHeaderCollection.checkColonSeparated(header);
			this.InternalSet(header.Substring(0, num), header.Substring(num + 1), response);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00015E90 File Offset: 0x00014090
		internal void InternalSet(string name, string value, bool response)
		{
			value = WebHeaderCollection.checkValue(value);
			bool flag = WebHeaderCollection.IsMultiValue(name, response);
			if (flag)
			{
				base.Add(name, value);
			}
			else
			{
				base.Set(name, value);
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00015EC8 File Offset: 0x000140C8
		internal static bool IsHeaderName(string name)
		{
			return name != null && name.Length > 0 && name.IsToken();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00015EF0 File Offset: 0x000140F0
		internal static bool IsHeaderValue(string value)
		{
			return value.IsText();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00015F08 File Offset: 0x00014108
		internal static bool IsMultiValue(string headerName, bool response)
		{
			bool flag = headerName == null || headerName.Length == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				HttpHeaderInfo headerInfo = WebHeaderCollection.getHeaderInfo(headerName);
				result = (headerInfo != null && headerInfo.IsMultiValue(response));
			}
			return result;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00015F48 File Offset: 0x00014148
		internal string ToStringMultiValue(bool response)
		{
			StringBuilder buff = new StringBuilder();
			this.Count.Times(delegate(int i)
			{
				string key = this.GetKey(i);
				bool flag = WebHeaderCollection.IsMultiValue(key, response);
				if (flag)
				{
					foreach (string arg in this.GetValues(i))
					{
						buff.AppendFormat("{0}: {1}\r\n", key, arg);
					}
				}
				else
				{
					buff.AppendFormat("{0}: {1}\r\n", key, this.Get(i));
				}
			});
			return buff.Append("\r\n").ToString();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00015FA6 File Offset: 0x000141A6
		protected void AddWithoutValidate(string headerName, string headerValue)
		{
			this.add(headerName, headerValue, true);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00015FB4 File Offset: 0x000141B4
		public void Add(string header)
		{
			bool flag = header == null || header.Length == 0;
			if (flag)
			{
				throw new ArgumentNullException("header");
			}
			int num = WebHeaderCollection.checkColonSeparated(header);
			this.add(header.Substring(0, num), header.Substring(num + 1), false);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00016000 File Offset: 0x00014200
		public void Add(HttpRequestHeader header, string value)
		{
			this.doWithCheckingState(new Action<string, string>(this.addWithoutCheckingName), WebHeaderCollection.Convert(header), value, false, true);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0001601F File Offset: 0x0001421F
		public void Add(HttpResponseHeader header, string value)
		{
			this.doWithCheckingState(new Action<string, string>(this.addWithoutCheckingName), WebHeaderCollection.Convert(header), value, true, true);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001603E File Offset: 0x0001423E
		public override void Add(string name, string value)
		{
			this.add(name, value, false);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0001604B File Offset: 0x0001424B
		public override void Clear()
		{
			base.Clear();
			this._state = HttpHeaderType.Unspecified;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001605C File Offset: 0x0001425C
		public override string Get(int index)
		{
			return base.Get(index);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00016078 File Offset: 0x00014278
		public override string Get(string name)
		{
			return base.Get(name);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00016094 File Offset: 0x00014294
		public override IEnumerator GetEnumerator()
		{
			return base.GetEnumerator();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000160AC File Offset: 0x000142AC
		public override string GetKey(int index)
		{
			return base.GetKey(index);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000160C8 File Offset: 0x000142C8
		public override string[] GetValues(int index)
		{
			string[] values = base.GetValues(index);
			return (values != null && values.Length != 0) ? values : null;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000160F0 File Offset: 0x000142F0
		public override string[] GetValues(string header)
		{
			string[] values = base.GetValues(header);
			return (values != null && values.Length != 0) ? values : null;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00016118 File Offset: 0x00014318
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			bool flag = serializationInfo == null;
			if (flag)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			serializationInfo.AddValue("InternallyUsed", this._internallyUsed);
			serializationInfo.AddValue("State", (int)this._state);
			int cnt = this.Count;
			serializationInfo.AddValue("Count", cnt);
			cnt.Times(delegate(int i)
			{
				serializationInfo.AddValue(i.ToString(), this.GetKey(i));
				serializationInfo.AddValue((cnt + i).ToString(), this.Get(i));
			});
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000161BC File Offset: 0x000143BC
		public static bool IsRestricted(string headerName)
		{
			return WebHeaderCollection.isRestricted(WebHeaderCollection.checkName(headerName), false);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000161DC File Offset: 0x000143DC
		public static bool IsRestricted(string headerName, bool response)
		{
			return WebHeaderCollection.isRestricted(WebHeaderCollection.checkName(headerName), response);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00014C7C File Offset: 0x00012E7C
		public override void OnDeserialization(object sender)
		{
		}

		// Token: 0x0600038D RID: 909 RVA: 0x000161FA File Offset: 0x000143FA
		public void Remove(HttpRequestHeader header)
		{
			this.doWithCheckingState(new Action<string, string>(this.removeWithoutCheckingName), WebHeaderCollection.Convert(header), null, false, false);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00016219 File Offset: 0x00014419
		public void Remove(HttpResponseHeader header)
		{
			this.doWithCheckingState(new Action<string, string>(this.removeWithoutCheckingName), WebHeaderCollection.Convert(header), null, true, false);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00016238 File Offset: 0x00014438
		public override void Remove(string name)
		{
			this.doWithCheckingState(new Action<string, string>(this.removeWithoutCheckingName), WebHeaderCollection.checkName(name), null, false);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00016256 File Offset: 0x00014456
		public void Set(HttpRequestHeader header, string value)
		{
			this.doWithCheckingState(new Action<string, string>(this.setWithoutCheckingName), WebHeaderCollection.Convert(header), value, false, true);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00016275 File Offset: 0x00014475
		public void Set(HttpResponseHeader header, string value)
		{
			this.doWithCheckingState(new Action<string, string>(this.setWithoutCheckingName), WebHeaderCollection.Convert(header), value, true, true);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00016294 File Offset: 0x00014494
		public override void Set(string name, string value)
		{
			this.doWithCheckingState(new Action<string, string>(this.setWithoutCheckingName), WebHeaderCollection.checkName(name), value, true);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000162B4 File Offset: 0x000144B4
		public byte[] ToByteArray()
		{
			return Encoding.UTF8.GetBytes(this.ToString());
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000162D8 File Offset: 0x000144D8
		public override string ToString()
		{
			StringBuilder buff = new StringBuilder();
			this.Count.Times(delegate(int i)
			{
				buff.AppendFormat("{0}: {1}\r\n", this.GetKey(i), this.Get(i));
			});
			return buff.Append("\r\n").ToString();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001632F File Offset: 0x0001452F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0400013B RID: 315
		private static readonly Dictionary<string, HttpHeaderInfo> _headers = new Dictionary<string, HttpHeaderInfo>(StringComparer.InvariantCultureIgnoreCase)
		{
			{
				"Accept",
				new HttpHeaderInfo("Accept", HttpHeaderType.Request | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
			},
			{
				"AcceptCharset",
				new HttpHeaderInfo("Accept-Charset", HttpHeaderType.Request | HttpHeaderType.MultiValue)
			},
			{
				"AcceptEncoding",
				new HttpHeaderInfo("Accept-Encoding", HttpHeaderType.Request | HttpHeaderType.MultiValue)
			},
			{
				"AcceptLanguage",
				new HttpHeaderInfo("Accept-Language", HttpHeaderType.Request | HttpHeaderType.MultiValue)
			},
			{
				"AcceptRanges",
				new HttpHeaderInfo("Accept-Ranges", HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"Age",
				new HttpHeaderInfo("Age", HttpHeaderType.Response)
			},
			{
				"Allow",
				new HttpHeaderInfo("Allow", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"Authorization",
				new HttpHeaderInfo("Authorization", HttpHeaderType.Request | HttpHeaderType.MultiValue)
			},
			{
				"CacheControl",
				new HttpHeaderInfo("Cache-Control", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"Connection",
				new HttpHeaderInfo("Connection", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
			},
			{
				"ContentEncoding",
				new HttpHeaderInfo("Content-Encoding", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"ContentLanguage",
				new HttpHeaderInfo("Content-Language", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"ContentLength",
				new HttpHeaderInfo("Content-Length", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted)
			},
			{
				"ContentLocation",
				new HttpHeaderInfo("Content-Location", HttpHeaderType.Request | HttpHeaderType.Response)
			},
			{
				"ContentMd5",
				new HttpHeaderInfo("Content-MD5", HttpHeaderType.Request | HttpHeaderType.Response)
			},
			{
				"ContentRange",
				new HttpHeaderInfo("Content-Range", HttpHeaderType.Request | HttpHeaderType.Response)
			},
			{
				"ContentType",
				new HttpHeaderInfo("Content-Type", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted)
			},
			{
				"Cookie",
				new HttpHeaderInfo("Cookie", HttpHeaderType.Request)
			},
			{
				"Cookie2",
				new HttpHeaderInfo("Cookie2", HttpHeaderType.Request)
			},
			{
				"Date",
				new HttpHeaderInfo("Date", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted)
			},
			{
				"Expect",
				new HttpHeaderInfo("Expect", HttpHeaderType.Request | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
			},
			{
				"Expires",
				new HttpHeaderInfo("Expires", HttpHeaderType.Request | HttpHeaderType.Response)
			},
			{
				"ETag",
				new HttpHeaderInfo("ETag", HttpHeaderType.Response)
			},
			{
				"From",
				new HttpHeaderInfo("From", HttpHeaderType.Request)
			},
			{
				"Host",
				new HttpHeaderInfo("Host", HttpHeaderType.Request | HttpHeaderType.Restricted)
			},
			{
				"IfMatch",
				new HttpHeaderInfo("If-Match", HttpHeaderType.Request | HttpHeaderType.MultiValue)
			},
			{
				"IfModifiedSince",
				new HttpHeaderInfo("If-Modified-Since", HttpHeaderType.Request | HttpHeaderType.Restricted)
			},
			{
				"IfNoneMatch",
				new HttpHeaderInfo("If-None-Match", HttpHeaderType.Request | HttpHeaderType.MultiValue)
			},
			{
				"IfRange",
				new HttpHeaderInfo("If-Range", HttpHeaderType.Request)
			},
			{
				"IfUnmodifiedSince",
				new HttpHeaderInfo("If-Unmodified-Since", HttpHeaderType.Request)
			},
			{
				"KeepAlive",
				new HttpHeaderInfo("Keep-Alive", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"LastModified",
				new HttpHeaderInfo("Last-Modified", HttpHeaderType.Request | HttpHeaderType.Response)
			},
			{
				"Location",
				new HttpHeaderInfo("Location", HttpHeaderType.Response)
			},
			{
				"MaxForwards",
				new HttpHeaderInfo("Max-Forwards", HttpHeaderType.Request)
			},
			{
				"Pragma",
				new HttpHeaderInfo("Pragma", HttpHeaderType.Request | HttpHeaderType.Response)
			},
			{
				"ProxyAuthenticate",
				new HttpHeaderInfo("Proxy-Authenticate", HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"ProxyAuthorization",
				new HttpHeaderInfo("Proxy-Authorization", HttpHeaderType.Request)
			},
			{
				"ProxyConnection",
				new HttpHeaderInfo("Proxy-Connection", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted)
			},
			{
				"Public",
				new HttpHeaderInfo("Public", HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"Range",
				new HttpHeaderInfo("Range", HttpHeaderType.Request | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
			},
			{
				"Referer",
				new HttpHeaderInfo("Referer", HttpHeaderType.Request | HttpHeaderType.Restricted)
			},
			{
				"RetryAfter",
				new HttpHeaderInfo("Retry-After", HttpHeaderType.Response)
			},
			{
				"SecWebSocketAccept",
				new HttpHeaderInfo("Sec-WebSocket-Accept", HttpHeaderType.Response | HttpHeaderType.Restricted)
			},
			{
				"SecWebSocketExtensions",
				new HttpHeaderInfo("Sec-WebSocket-Extensions", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValueInRequest)
			},
			{
				"SecWebSocketKey",
				new HttpHeaderInfo("Sec-WebSocket-Key", HttpHeaderType.Request | HttpHeaderType.Restricted)
			},
			{
				"SecWebSocketProtocol",
				new HttpHeaderInfo("Sec-WebSocket-Protocol", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValueInRequest)
			},
			{
				"SecWebSocketVersion",
				new HttpHeaderInfo("Sec-WebSocket-Version", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValueInResponse)
			},
			{
				"Server",
				new HttpHeaderInfo("Server", HttpHeaderType.Response)
			},
			{
				"SetCookie",
				new HttpHeaderInfo("Set-Cookie", HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"SetCookie2",
				new HttpHeaderInfo("Set-Cookie2", HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"Te",
				new HttpHeaderInfo("TE", HttpHeaderType.Request)
			},
			{
				"Trailer",
				new HttpHeaderInfo("Trailer", HttpHeaderType.Request | HttpHeaderType.Response)
			},
			{
				"TransferEncoding",
				new HttpHeaderInfo("Transfer-Encoding", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
			},
			{
				"Translate",
				new HttpHeaderInfo("Translate", HttpHeaderType.Request)
			},
			{
				"Upgrade",
				new HttpHeaderInfo("Upgrade", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"UserAgent",
				new HttpHeaderInfo("User-Agent", HttpHeaderType.Request | HttpHeaderType.Restricted)
			},
			{
				"Vary",
				new HttpHeaderInfo("Vary", HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"Via",
				new HttpHeaderInfo("Via", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"Warning",
				new HttpHeaderInfo("Warning", HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
			},
			{
				"WwwAuthenticate",
				new HttpHeaderInfo("WWW-Authenticate", HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
			}
		};

		// Token: 0x0400013C RID: 316
		private bool _internallyUsed;

		// Token: 0x0400013D RID: 317
		private HttpHeaderType _state;
	}
}
