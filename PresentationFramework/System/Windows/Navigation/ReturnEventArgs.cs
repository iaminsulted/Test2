using System;

namespace System.Windows.Navigation
{
	// Token: 0x020005D3 RID: 1491
	public class ReturnEventArgs<T> : EventArgs
	{
		// Token: 0x060047FC RID: 18428 RVA: 0x0022AC23 File Offset: 0x00229C23
		public ReturnEventArgs()
		{
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0022AC2B File Offset: 0x00229C2B
		public ReturnEventArgs(T result)
		{
			this._result = result;
		}

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x060047FE RID: 18430 RVA: 0x0022AC3A File Offset: 0x00229C3A
		// (set) Token: 0x060047FF RID: 18431 RVA: 0x0022AC42 File Offset: 0x00229C42
		public T Result
		{
			get
			{
				return this._result;
			}
			set
			{
				this._result = value;
			}
		}

		// Token: 0x040025F1 RID: 9713
		private T _result;
	}
}
