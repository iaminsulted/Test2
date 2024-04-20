using System;

namespace MS.Internal.Data
{
	// Token: 0x02000202 RID: 514
	internal class AsyncGetValueRequest : AsyncDataRequest
	{
		// Token: 0x060012D0 RID: 4816 RVA: 0x0014BCAC File Offset: 0x0014ACAC
		internal AsyncGetValueRequest(object item, string propertyName, object bindingState, AsyncRequestCallback workCallback, AsyncRequestCallback completedCallback, params object[] args) : base(bindingState, workCallback, completedCallback, args)
		{
			this._item = item;
			this._propertyName = propertyName;
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0014BCC9 File Offset: 0x0014ACC9
		public object SourceItem
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x04000B62 RID: 2914
		private object _item;

		// Token: 0x04000B63 RID: 2915
		private string _propertyName;
	}
}
