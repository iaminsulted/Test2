using System;

namespace WebSocketSharp.Net
{
	// Token: 0x0200002E RID: 46
	internal class HttpHeaderInfo
	{
		// Token: 0x06000398 RID: 920 RVA: 0x00016355 File Offset: 0x00014555
		internal HttpHeaderInfo(string name, HttpHeaderType type)
		{
			this._name = name;
			this._type = type;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00016370 File Offset: 0x00014570
		internal bool IsMultiValueInRequest
		{
			get
			{
				return (this._type & HttpHeaderType.MultiValueInRequest) == HttpHeaderType.MultiValueInRequest;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00016390 File Offset: 0x00014590
		internal bool IsMultiValueInResponse
		{
			get
			{
				return (this._type & HttpHeaderType.MultiValueInResponse) == HttpHeaderType.MultiValueInResponse;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600039B RID: 923 RVA: 0x000163B0 File Offset: 0x000145B0
		public bool IsRequest
		{
			get
			{
				return (this._type & HttpHeaderType.Request) == HttpHeaderType.Request;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600039C RID: 924 RVA: 0x000163D0 File Offset: 0x000145D0
		public bool IsResponse
		{
			get
			{
				return (this._type & HttpHeaderType.Response) == HttpHeaderType.Response;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600039D RID: 925 RVA: 0x000163F0 File Offset: 0x000145F0
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00016408 File Offset: 0x00014608
		public HttpHeaderType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00016420 File Offset: 0x00014620
		public bool IsMultiValue(bool response)
		{
			return ((this._type & HttpHeaderType.MultiValue) == HttpHeaderType.MultiValue) ? (response ? this.IsResponse : this.IsRequest) : (response ? this.IsMultiValueInResponse : this.IsMultiValueInRequest);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00016464 File Offset: 0x00014664
		public bool IsRestricted(bool response)
		{
			return (this._type & HttpHeaderType.Restricted) == HttpHeaderType.Restricted && (response ? this.IsResponse : this.IsRequest);
		}

		// Token: 0x04000177 RID: 375
		private string _name;

		// Token: 0x04000178 RID: 376
		private HttpHeaderType _type;
	}
}
