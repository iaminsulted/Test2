using System;

namespace System.Windows.Controls
{
	// Token: 0x0200076E RID: 1902
	public class DatePickerDateValidationErrorEventArgs : EventArgs
	{
		// Token: 0x06006784 RID: 26500 RVA: 0x002B57E0 File Offset: 0x002B47E0
		public DatePickerDateValidationErrorEventArgs(Exception exception, string text)
		{
			this.Text = text;
			this.Exception = exception;
		}

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x06006785 RID: 26501 RVA: 0x002B57F6 File Offset: 0x002B47F6
		// (set) Token: 0x06006786 RID: 26502 RVA: 0x002B57FE File Offset: 0x002B47FE
		public Exception Exception { get; private set; }

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x06006787 RID: 26503 RVA: 0x002B5807 File Offset: 0x002B4807
		// (set) Token: 0x06006788 RID: 26504 RVA: 0x002B580F File Offset: 0x002B480F
		public string Text { get; private set; }

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x06006789 RID: 26505 RVA: 0x002B5818 File Offset: 0x002B4818
		// (set) Token: 0x0600678A RID: 26506 RVA: 0x002B5820 File Offset: 0x002B4820
		public bool ThrowException
		{
			get
			{
				return this._throwException;
			}
			set
			{
				this._throwException = value;
			}
		}

		// Token: 0x04003448 RID: 13384
		private bool _throwException;
	}
}
