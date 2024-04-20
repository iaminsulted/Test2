using System;
using System.Collections.Specialized;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x02000039 RID: 57
	internal class AuthenticationChallenge : AuthenticationBase
	{
		// Token: 0x060003D5 RID: 981 RVA: 0x00016FD8 File Offset: 0x000151D8
		private AuthenticationChallenge(AuthenticationSchemes scheme, NameValueCollection parameters) : base(scheme, parameters)
		{
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00016FE4 File Offset: 0x000151E4
		internal AuthenticationChallenge(AuthenticationSchemes scheme, string realm) : base(scheme, new NameValueCollection())
		{
			this.Parameters["realm"] = realm;
			bool flag = scheme == AuthenticationSchemes.Digest;
			if (flag)
			{
				this.Parameters["nonce"] = AuthenticationBase.CreateNonceValue();
				this.Parameters["algorithm"] = "MD5";
				this.Parameters["qop"] = "auth";
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00017060 File Offset: 0x00015260
		public string Domain
		{
			get
			{
				return this.Parameters["domain"];
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00017084 File Offset: 0x00015284
		public string Stale
		{
			get
			{
				return this.Parameters["stale"];
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000170A8 File Offset: 0x000152A8
		internal static AuthenticationChallenge CreateBasicChallenge(string realm)
		{
			return new AuthenticationChallenge(AuthenticationSchemes.Basic, realm);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000170C4 File Offset: 0x000152C4
		internal static AuthenticationChallenge CreateDigestChallenge(string realm)
		{
			return new AuthenticationChallenge(AuthenticationSchemes.Digest, realm);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000170E0 File Offset: 0x000152E0
		internal static AuthenticationChallenge Parse(string value)
		{
			string[] array = value.Split(new char[]
			{
				' '
			}, 2);
			bool flag = array.Length != 2;
			AuthenticationChallenge result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string a = array[0].ToLower();
				result = ((a == "basic") ? new AuthenticationChallenge(AuthenticationSchemes.Basic, AuthenticationBase.ParseParameters(array[1])) : ((a == "digest") ? new AuthenticationChallenge(AuthenticationSchemes.Digest, AuthenticationBase.ParseParameters(array[1])) : null));
			}
			return result;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001715C File Offset: 0x0001535C
		internal override string ToBasicString()
		{
			return string.Format("Basic realm=\"{0}\"", this.Parameters["realm"]);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00017188 File Offset: 0x00015388
		internal override string ToDigestString()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			string text = this.Parameters["domain"];
			bool flag = text != null;
			if (flag)
			{
				stringBuilder.AppendFormat("Digest realm=\"{0}\", domain=\"{1}\", nonce=\"{2}\"", this.Parameters["realm"], text, this.Parameters["nonce"]);
			}
			else
			{
				stringBuilder.AppendFormat("Digest realm=\"{0}\", nonce=\"{1}\"", this.Parameters["realm"], this.Parameters["nonce"]);
			}
			string text2 = this.Parameters["opaque"];
			bool flag2 = text2 != null;
			if (flag2)
			{
				stringBuilder.AppendFormat(", opaque=\"{0}\"", text2);
			}
			string text3 = this.Parameters["stale"];
			bool flag3 = text3 != null;
			if (flag3)
			{
				stringBuilder.AppendFormat(", stale={0}", text3);
			}
			string text4 = this.Parameters["algorithm"];
			bool flag4 = text4 != null;
			if (flag4)
			{
				stringBuilder.AppendFormat(", algorithm={0}", text4);
			}
			string text5 = this.Parameters["qop"];
			bool flag5 = text5 != null;
			if (flag5)
			{
				stringBuilder.AppendFormat(", qop=\"{0}\"", text5);
			}
			return stringBuilder.ToString();
		}
	}
}
