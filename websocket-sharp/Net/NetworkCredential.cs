using System;

namespace WebSocketSharp.Net
{
	// Token: 0x02000031 RID: 49
	public class NetworkCredential
	{
		// Token: 0x060003AF RID: 943 RVA: 0x000166AA File Offset: 0x000148AA
		public NetworkCredential(string username, string password) : this(username, password, null, null)
		{
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x000166B8 File Offset: 0x000148B8
		public NetworkCredential(string username, string password, string domain, params string[] roles)
		{
			bool flag = username == null;
			if (flag)
			{
				throw new ArgumentNullException("username");
			}
			bool flag2 = username.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("An empty string.", "username");
			}
			this._username = username;
			this._password = password;
			this._domain = domain;
			this._roles = roles;
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0001671C File Offset: 0x0001491C
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0001673D File Offset: 0x0001493D
		public string Domain
		{
			get
			{
				return this._domain ?? string.Empty;
			}
			internal set
			{
				this._domain = value;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00016748 File Offset: 0x00014948
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x00016769 File Offset: 0x00014969
		public string Password
		{
			get
			{
				return this._password ?? string.Empty;
			}
			internal set
			{
				this._password = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00016774 File Offset: 0x00014974
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00016795 File Offset: 0x00014995
		public string[] Roles
		{
			get
			{
				return this._roles ?? NetworkCredential._noRoles;
			}
			internal set
			{
				this._roles = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x000167A0 File Offset: 0x000149A0
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x000167B8 File Offset: 0x000149B8
		public string Username
		{
			get
			{
				return this._username;
			}
			internal set
			{
				this._username = value;
			}
		}

		// Token: 0x0400017B RID: 379
		private string _domain;

		// Token: 0x0400017C RID: 380
		private static readonly string[] _noRoles = new string[0];

		// Token: 0x0400017D RID: 381
		private string _password;

		// Token: 0x0400017E RID: 382
		private string[] _roles;

		// Token: 0x0400017F RID: 383
		private string _username;
	}
}
