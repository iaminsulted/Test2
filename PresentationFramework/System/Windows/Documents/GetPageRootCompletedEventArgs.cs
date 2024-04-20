using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x0200064A RID: 1610
	public sealed class GetPageRootCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06004FDE RID: 20446 RVA: 0x00244D9F File Offset: 0x00243D9F
		internal GetPageRootCompletedEventArgs(FixedPage page, Exception error, bool cancelled, object userToken) : base(error, cancelled, userToken)
		{
			this._page = page;
		}

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06004FDF RID: 20447 RVA: 0x00244DB2 File Offset: 0x00243DB2
		public FixedPage Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._page;
			}
		}

		// Token: 0x04002869 RID: 10345
		private FixedPage _page;
	}
}
