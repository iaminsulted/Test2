using System;

namespace System.Windows.Documents.Serialization
{
	// Token: 0x020006F2 RID: 1778
	public class WritingCancelledEventArgs : EventArgs
	{
		// Token: 0x06005D51 RID: 23889 RVA: 0x0028C8A3 File Offset: 0x0028B8A3
		public WritingCancelledEventArgs(Exception exception)
		{
			this._exception = exception;
		}

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x06005D52 RID: 23890 RVA: 0x0028C8B2 File Offset: 0x0028B8B2
		public Exception Error
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x0400315A RID: 12634
		private Exception _exception;
	}
}
