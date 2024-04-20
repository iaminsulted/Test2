using System;

namespace MS.Internal.Data
{
	// Token: 0x02000203 RID: 515
	internal class AsyncSetValueRequest : AsyncDataRequest
	{
		// Token: 0x060012D2 RID: 4818 RVA: 0x0014BCD1 File Offset: 0x0014ACD1
		internal AsyncSetValueRequest(object item, string propertyName, object value, object bindingState, AsyncRequestCallback workCallback, AsyncRequestCallback completedCallback, params object[] args) : base(bindingState, workCallback, completedCallback, args)
		{
			this._item = item;
			this._propertyName = propertyName;
			this._value = value;
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x0014BCF6 File Offset: 0x0014ACF6
		public object TargetItem
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x0014BCFE File Offset: 0x0014ACFE
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x04000B64 RID: 2916
		private object _item;

		// Token: 0x04000B65 RID: 2917
		private string _propertyName;

		// Token: 0x04000B66 RID: 2918
		private object _value;
	}
}
