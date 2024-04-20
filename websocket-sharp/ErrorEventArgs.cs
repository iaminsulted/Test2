using System;

namespace WebSocketSharp
{
	// Token: 0x02000006 RID: 6
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x06000082 RID: 130 RVA: 0x000046AA File Offset: 0x000028AA
		internal ErrorEventArgs(string message) : this(message, null)
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000046B6 File Offset: 0x000028B6
		internal ErrorEventArgs(string message, Exception exception)
		{
			this._message = message;
			this._exception = exception;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000046D0 File Offset: 0x000028D0
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000046E8 File Offset: 0x000028E8
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x0400000D RID: 13
		private Exception _exception;

		// Token: 0x0400000E RID: 14
		private string _message;
	}
}
