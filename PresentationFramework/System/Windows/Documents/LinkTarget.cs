using System;

namespace System.Windows.Documents
{
	// Token: 0x0200063E RID: 1598
	public sealed class LinkTarget
	{
		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06004F12 RID: 20242 RVA: 0x0024350C File Offset: 0x0024250C
		// (set) Token: 0x06004F13 RID: 20243 RVA: 0x00243514 File Offset: 0x00242514
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x04002841 RID: 10305
		private string _name;
	}
}
