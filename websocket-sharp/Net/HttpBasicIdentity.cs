using System;
using System.Security.Principal;

namespace WebSocketSharp.Net
{
	// Token: 0x0200002F RID: 47
	public class HttpBasicIdentity : GenericIdentity
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x00016495 File Offset: 0x00014695
		internal HttpBasicIdentity(string username, string password) : base(username, "Basic")
		{
			this._password = password;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x000164AC File Offset: 0x000146AC
		public virtual string Password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x04000179 RID: 377
		private string _password;
	}
}
