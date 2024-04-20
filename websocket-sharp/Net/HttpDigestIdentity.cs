using System;
using System.Collections.Specialized;
using System.Security.Principal;

namespace WebSocketSharp.Net
{
	// Token: 0x02000030 RID: 48
	public class HttpDigestIdentity : GenericIdentity
	{
		// Token: 0x060003A3 RID: 931 RVA: 0x000164C4 File Offset: 0x000146C4
		internal HttpDigestIdentity(NameValueCollection parameters) : base(parameters["username"], "Digest")
		{
			this._parameters = parameters;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x000164E8 File Offset: 0x000146E8
		public string Algorithm
		{
			get
			{
				return this._parameters["algorithm"];
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0001650C File Offset: 0x0001470C
		public string Cnonce
		{
			get
			{
				return this._parameters["cnonce"];
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00016530 File Offset: 0x00014730
		public string Nc
		{
			get
			{
				return this._parameters["nc"];
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00016554 File Offset: 0x00014754
		public string Nonce
		{
			get
			{
				return this._parameters["nonce"];
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00016578 File Offset: 0x00014778
		public string Opaque
		{
			get
			{
				return this._parameters["opaque"];
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0001659C File Offset: 0x0001479C
		public string Qop
		{
			get
			{
				return this._parameters["qop"];
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003AA RID: 938 RVA: 0x000165C0 File Offset: 0x000147C0
		public string Realm
		{
			get
			{
				return this._parameters["realm"];
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003AB RID: 939 RVA: 0x000165E4 File Offset: 0x000147E4
		public string Response
		{
			get
			{
				return this._parameters["response"];
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00016608 File Offset: 0x00014808
		public string Uri
		{
			get
			{
				return this._parameters["uri"];
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001662C File Offset: 0x0001482C
		internal bool IsValid(string password, string realm, string method, string entity)
		{
			NameValueCollection nameValueCollection = new NameValueCollection(this._parameters);
			nameValueCollection["password"] = password;
			nameValueCollection["realm"] = realm;
			nameValueCollection["method"] = method;
			nameValueCollection["entity"] = entity;
			string b = AuthenticationResponse.CreateRequestDigest(nameValueCollection);
			return this._parameters["response"] == b;
		}

		// Token: 0x0400017A RID: 378
		private NameValueCollection _parameters;
	}
}
