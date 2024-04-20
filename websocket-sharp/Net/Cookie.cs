using System;
using System.Globalization;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public sealed class Cookie
	{
		// Token: 0x060001BA RID: 442 RVA: 0x0000BD90 File Offset: 0x00009F90
		public Cookie()
		{
			this._comment = string.Empty;
			this._domain = string.Empty;
			this._expires = DateTime.MinValue;
			this._name = string.Empty;
			this._path = string.Empty;
			this._port = string.Empty;
			this._ports = new int[0];
			this._timestamp = DateTime.Now;
			this._value = string.Empty;
			this._version = 0;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000BE10 File Offset: 0x0000A010
		public Cookie(string name, string value) : this()
		{
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000BE2A File Offset: 0x0000A02A
		public Cookie(string name, string value, string path) : this(name, value)
		{
			this.Path = path;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000BE3E File Offset: 0x0000A03E
		public Cookie(string name, string value, string path, string domain) : this(name, value, path)
		{
			this.Domain = domain;
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000BE54 File Offset: 0x0000A054
		// (set) Token: 0x060001BF RID: 447 RVA: 0x0000BE5C File Offset: 0x0000A05C
		internal bool ExactDomain { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000BE68 File Offset: 0x0000A068
		internal int MaxAge
		{
			get
			{
				bool flag = this._expires == DateTime.MinValue;
				int result;
				if (flag)
				{
					result = 0;
				}
				else
				{
					DateTime d = (this._expires.Kind != DateTimeKind.Local) ? this._expires.ToLocalTime() : this._expires;
					TimeSpan t = d - DateTime.Now;
					result = ((t > TimeSpan.Zero) ? ((int)t.TotalSeconds) : 0);
				}
				return result;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000BED8 File Offset: 0x0000A0D8
		internal int[] Ports
		{
			get
			{
				return this._ports;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000BEF0 File Offset: 0x0000A0F0
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x0000BF08 File Offset: 0x0000A108
		public string Comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				this._comment = (value ?? string.Empty);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000BF1C File Offset: 0x0000A11C
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x0000BF34 File Offset: 0x0000A134
		public Uri CommentUri
		{
			get
			{
				return this._commentUri;
			}
			set
			{
				this._commentUri = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000BF40 File Offset: 0x0000A140
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x0000BF58 File Offset: 0x0000A158
		public bool Discard
		{
			get
			{
				return this._discard;
			}
			set
			{
				this._discard = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000BF64 File Offset: 0x0000A164
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000BF7C File Offset: 0x0000A17C
		public string Domain
		{
			get
			{
				return this._domain;
			}
			set
			{
				bool flag = value.IsNullOrEmpty();
				if (flag)
				{
					this._domain = string.Empty;
					this.ExactDomain = true;
				}
				else
				{
					this._domain = value;
					this.ExactDomain = (value[0] != '.');
				}
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000C003 File Offset: 0x0000A203
		public bool Expired
		{
			get
			{
				return this._expires != DateTime.MinValue && this._expires <= DateTime.Now;
			}
			set
			{
				this._expires = (value ? DateTime.Now : DateTime.MinValue);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000C01C File Offset: 0x0000A21C
		// (set) Token: 0x060001CD RID: 461 RVA: 0x0000C034 File Offset: 0x0000A234
		public DateTime Expires
		{
			get
			{
				return this._expires;
			}
			set
			{
				this._expires = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000C040 File Offset: 0x0000A240
		// (set) Token: 0x060001CF RID: 463 RVA: 0x0000C058 File Offset: 0x0000A258
		public bool HttpOnly
		{
			get
			{
				return this._httpOnly;
			}
			set
			{
				this._httpOnly = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000C064 File Offset: 0x0000A264
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x0000C07C File Offset: 0x0000A27C
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				string message;
				bool flag = !Cookie.canSetName(value, out message);
				if (flag)
				{
					throw new CookieException(message);
				}
				this._name = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000C0A8 File Offset: 0x0000A2A8
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x0000C0C0 File Offset: 0x0000A2C0
		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = (value ?? string.Empty);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x0000C0EC File Offset: 0x0000A2EC
		public string Port
		{
			get
			{
				return this._port;
			}
			set
			{
				bool flag = value.IsNullOrEmpty();
				if (flag)
				{
					this._port = string.Empty;
					this._ports = new int[0];
				}
				else
				{
					bool flag2 = !value.IsEnclosedIn('"');
					if (flag2)
					{
						throw new CookieException("The value specified for the Port attribute isn't enclosed in double quotes.");
					}
					string arg;
					bool flag3 = !Cookie.tryCreatePorts(value, out this._ports, out arg);
					if (flag3)
					{
						throw new CookieException(string.Format("The value specified for the Port attribute contains an invalid value: {0}", arg));
					}
					this._port = value;
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000C168 File Offset: 0x0000A368
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x0000C180 File Offset: 0x0000A380
		public bool Secure
		{
			get
			{
				return this._secure;
			}
			set
			{
				this._secure = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000C18C File Offset: 0x0000A38C
		public DateTime TimeStamp
		{
			get
			{
				return this._timestamp;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000C1A4 File Offset: 0x0000A3A4
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000C1BC File Offset: 0x0000A3BC
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				string message;
				bool flag = !Cookie.canSetValue(value, out message);
				if (flag)
				{
					throw new CookieException(message);
				}
				this._value = ((value.Length > 0) ? value : "\"\"");
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
		// (set) Token: 0x060001DC RID: 476 RVA: 0x0000C210 File Offset: 0x0000A410
		public int Version
		{
			get
			{
				return this._version;
			}
			set
			{
				bool flag = value < 0 || value > 1;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("value", "Not 0 or 1.");
				}
				this._version = value;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000C244 File Offset: 0x0000A444
		private static bool canSetName(string name, out string message)
		{
			bool flag = name.IsNullOrEmpty();
			bool result;
			if (flag)
			{
				message = "The value specified for the Name is null or empty.";
				result = false;
			}
			else
			{
				bool flag2 = name[0] == '$' || name.Contains(Cookie._reservedCharsForName);
				if (flag2)
				{
					message = "The value specified for the Name contains an invalid character.";
					result = false;
				}
				else
				{
					message = string.Empty;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000C2A0 File Offset: 0x0000A4A0
		private static bool canSetValue(string value, out string message)
		{
			bool flag = value == null;
			bool result;
			if (flag)
			{
				message = "The value specified for the Value is null.";
				result = false;
			}
			else
			{
				bool flag2 = value.Contains(Cookie._reservedCharsForValue) && !value.IsEnclosedIn('"');
				if (flag2)
				{
					message = "The value specified for the Value contains an invalid character.";
					result = false;
				}
				else
				{
					message = string.Empty;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000C2FC File Offset: 0x0000A4FC
		private static int hash(int i, int j, int k, int l, int m)
		{
			return i ^ (j << 13 | j >> 19) ^ (k << 26 | k >> 6) ^ (l << 7 | l >> 25) ^ (m << 20 | m >> 12);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000C338 File Offset: 0x0000A538
		private string toResponseStringVersion0()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("{0}={1}", this._name, this._value);
			bool flag = this._expires != DateTime.MinValue;
			if (flag)
			{
				stringBuilder.AppendFormat("; Expires={0}", this._expires.ToUniversalTime().ToString("ddd, dd'-'MMM'-'yyyy HH':'mm':'ss 'GMT'", CultureInfo.CreateSpecificCulture("en-US")));
			}
			bool flag2 = !this._path.IsNullOrEmpty();
			if (flag2)
			{
				stringBuilder.AppendFormat("; Path={0}", this._path);
			}
			bool flag3 = !this._domain.IsNullOrEmpty();
			if (flag3)
			{
				stringBuilder.AppendFormat("; Domain={0}", this._domain);
			}
			bool secure = this._secure;
			if (secure)
			{
				stringBuilder.Append("; Secure");
			}
			bool httpOnly = this._httpOnly;
			if (httpOnly)
			{
				stringBuilder.Append("; HttpOnly");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000C430 File Offset: 0x0000A630
		private string toResponseStringVersion1()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("{0}={1}; Version={2}", this._name, this._value, this._version);
			bool flag = this._expires != DateTime.MinValue;
			if (flag)
			{
				stringBuilder.AppendFormat("; Max-Age={0}", this.MaxAge);
			}
			bool flag2 = !this._path.IsNullOrEmpty();
			if (flag2)
			{
				stringBuilder.AppendFormat("; Path={0}", this._path);
			}
			bool flag3 = !this._domain.IsNullOrEmpty();
			if (flag3)
			{
				stringBuilder.AppendFormat("; Domain={0}", this._domain);
			}
			bool flag4 = !this._port.IsNullOrEmpty();
			if (flag4)
			{
				stringBuilder.Append((this._port != "\"\"") ? string.Format("; Port={0}", this._port) : "; Port");
			}
			bool flag5 = !this._comment.IsNullOrEmpty();
			if (flag5)
			{
				stringBuilder.AppendFormat("; Comment={0}", HttpUtility.UrlEncode(this._comment));
			}
			bool flag6 = this._commentUri != null;
			if (flag6)
			{
				string originalString = this._commentUri.OriginalString;
				stringBuilder.AppendFormat("; CommentURL={0}", (!originalString.IsToken()) ? originalString.Quote() : originalString);
			}
			bool discard = this._discard;
			if (discard)
			{
				stringBuilder.Append("; Discard");
			}
			bool secure = this._secure;
			if (secure)
			{
				stringBuilder.Append("; Secure");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000C5CC File Offset: 0x0000A7CC
		private static bool tryCreatePorts(string value, out int[] result, out string parseError)
		{
			string[] array = value.Trim(new char[]
			{
				'"'
			}).Split(new char[]
			{
				','
			});
			int num = array.Length;
			int[] array2 = new int[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = int.MinValue;
				string text = array[i].Trim();
				bool flag = text.Length == 0;
				if (!flag)
				{
					bool flag2 = !int.TryParse(text, out array2[i]);
					if (flag2)
					{
						result = new int[0];
						parseError = text;
						return false;
					}
				}
			}
			result = array2;
			parseError = string.Empty;
			return true;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000C67C File Offset: 0x0000A87C
		internal string ToRequestString(Uri uri)
		{
			bool flag = this._name.Length == 0;
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				bool flag2 = this._version == 0;
				if (flag2)
				{
					result = string.Format("{0}={1}", this._name, this._value);
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder(64);
					stringBuilder.AppendFormat("$Version={0}; {1}={2}", this._version, this._name, this._value);
					bool flag3 = !this._path.IsNullOrEmpty();
					if (flag3)
					{
						stringBuilder.AppendFormat("; $Path={0}", this._path);
					}
					else
					{
						bool flag4 = uri != null;
						if (flag4)
						{
							stringBuilder.AppendFormat("; $Path={0}", uri.GetAbsolutePath());
						}
						else
						{
							stringBuilder.Append("; $Path=/");
						}
					}
					bool flag5 = uri == null || uri.Host != this._domain;
					bool flag6 = flag5 && !this._domain.IsNullOrEmpty();
					if (flag6)
					{
						stringBuilder.AppendFormat("; $Domain={0}", this._domain);
					}
					bool flag7 = !this._port.IsNullOrEmpty();
					if (flag7)
					{
						bool flag8 = this._port == "\"\"";
						if (flag8)
						{
							stringBuilder.Append("; $Port");
						}
						else
						{
							stringBuilder.AppendFormat("; $Port={0}", this._port);
						}
					}
					result = stringBuilder.ToString();
				}
			}
			return result;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
		internal string ToResponseString()
		{
			return (this._name.Length > 0) ? ((this._version == 0) ? this.toResponseStringVersion0() : this.toResponseStringVersion1()) : string.Empty;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000C834 File Offset: 0x0000AA34
		public override bool Equals(object comparand)
		{
			Cookie cookie = comparand as Cookie;
			return cookie != null && this._name.Equals(cookie.Name, StringComparison.InvariantCultureIgnoreCase) && this._value.Equals(cookie.Value, StringComparison.InvariantCulture) && this._path.Equals(cookie.Path, StringComparison.InvariantCulture) && this._domain.Equals(cookie.Domain, StringComparison.InvariantCultureIgnoreCase) && this._version == cookie.Version;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000C8B4 File Offset: 0x0000AAB4
		public override int GetHashCode()
		{
			return Cookie.hash(StringComparer.InvariantCultureIgnoreCase.GetHashCode(this._name), this._value.GetHashCode(), this._path.GetHashCode(), StringComparer.InvariantCultureIgnoreCase.GetHashCode(this._domain), this._version);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000C908 File Offset: 0x0000AB08
		public override string ToString()
		{
			return this.ToRequestString(null);
		}

		// Token: 0x040000A7 RID: 167
		private string _comment;

		// Token: 0x040000A8 RID: 168
		private Uri _commentUri;

		// Token: 0x040000A9 RID: 169
		private bool _discard;

		// Token: 0x040000AA RID: 170
		private string _domain;

		// Token: 0x040000AB RID: 171
		private DateTime _expires;

		// Token: 0x040000AC RID: 172
		private bool _httpOnly;

		// Token: 0x040000AD RID: 173
		private string _name;

		// Token: 0x040000AE RID: 174
		private string _path;

		// Token: 0x040000AF RID: 175
		private string _port;

		// Token: 0x040000B0 RID: 176
		private int[] _ports;

		// Token: 0x040000B1 RID: 177
		private static readonly char[] _reservedCharsForName = new char[]
		{
			' ',
			'=',
			';',
			',',
			'\n',
			'\r',
			'\t'
		};

		// Token: 0x040000B2 RID: 178
		private static readonly char[] _reservedCharsForValue = new char[]
		{
			';',
			','
		};

		// Token: 0x040000B3 RID: 179
		private bool _secure;

		// Token: 0x040000B4 RID: 180
		private DateTime _timestamp;

		// Token: 0x040000B5 RID: 181
		private string _value;

		// Token: 0x040000B6 RID: 182
		private int _version;
	}
}
