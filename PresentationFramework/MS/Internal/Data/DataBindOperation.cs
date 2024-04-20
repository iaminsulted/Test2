using System;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000214 RID: 532
	internal class DataBindOperation
	{
		// Token: 0x0600143A RID: 5178 RVA: 0x001512F3 File Offset: 0x001502F3
		public DataBindOperation(DispatcherOperationCallback method, object arg, int cost = 1)
		{
			this._method = method;
			this._arg = arg;
			this._cost = cost;
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x00151310 File Offset: 0x00150310
		// (set) Token: 0x0600143C RID: 5180 RVA: 0x00151318 File Offset: 0x00150318
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				this._cost = value;
			}
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x00151321 File Offset: 0x00150321
		public void Invoke()
		{
			this._method(this._arg);
		}

		// Token: 0x04000BB9 RID: 3001
		private DispatcherOperationCallback _method;

		// Token: 0x04000BBA RID: 3002
		private object _arg;

		// Token: 0x04000BBB RID: 3003
		private int _cost;
	}
}
