using System;
using System.Collections.Specialized;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x0200003B RID: 59
	internal abstract class AuthenticationBase
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x00017ADE File Offset: 0x00015CDE
		protected AuthenticationBase(AuthenticationSchemes scheme, NameValueCollection parameters)
		{
			this._scheme = scheme;
			this.Parameters = parameters;
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00017AF8 File Offset: 0x00015CF8
		public string Algorithm
		{
			get
			{
				return this.Parameters["algorithm"];
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00017B1C File Offset: 0x00015D1C
		public string Nonce
		{
			get
			{
				return this.Parameters["nonce"];
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00017B40 File Offset: 0x00015D40
		public string Opaque
		{
			get
			{
				return this.Parameters["opaque"];
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00017B64 File Offset: 0x00015D64
		public string Qop
		{
			get
			{
				return this.Parameters["qop"];
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00017B88 File Offset: 0x00015D88
		public string Realm
		{
			get
			{
				return this.Parameters["realm"];
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00017BAC File Offset: 0x00015DAC
		public AuthenticationSchemes Scheme
		{
			get
			{
				return this._scheme;
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00017BC4 File Offset: 0x00015DC4
		internal static string CreateNonceValue()
		{
			byte[] array = new byte[16];
			Random random = new Random();
			random.NextBytes(array);
			StringBuilder stringBuilder = new StringBuilder(32);
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00017C2C File Offset: 0x00015E2C
		internal static NameValueCollection ParseParameters(string value)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			foreach (string text in value.SplitHeaderValue(new char[]
			{
				','
			}))
			{
				int num = text.IndexOf('=');
				string name = (num > 0) ? text.Substring(0, num).Trim() : null;
				string value2 = (num < 0) ? text.Trim().Trim(new char[]
				{
					'"'
				}) : ((num < text.Length - 1) ? text.Substring(num + 1).Trim().Trim(new char[]
				{
					'"'
				}) : string.Empty);
				nameValueCollection.Add(name, value2);
			}
			return nameValueCollection;
		}

		// Token: 0x060003FE RID: 1022
		internal abstract string ToBasicString();

		// Token: 0x060003FF RID: 1023
		internal abstract string ToDigestString();

		// Token: 0x06000400 RID: 1024 RVA: 0x00017D10 File Offset: 0x00015F10
		public override string ToString()
		{
			return (this._scheme == AuthenticationSchemes.Basic) ? this.ToBasicString() : ((this._scheme == AuthenticationSchemes.Digest) ? this.ToDigestString() : string.Empty);
		}

		// Token: 0x0400019A RID: 410
		private AuthenticationSchemes _scheme;

		// Token: 0x0400019B RID: 411
		internal NameValueCollection Parameters;
	}
}
