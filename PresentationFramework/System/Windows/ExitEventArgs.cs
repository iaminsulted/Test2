using System;

namespace System.Windows
{
	// Token: 0x02000360 RID: 864
	public class ExitEventArgs : EventArgs
	{
		// Token: 0x0600209F RID: 8351 RVA: 0x00175E32 File Offset: 0x00174E32
		internal ExitEventArgs(int exitCode)
		{
			this._exitCode = exitCode;
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x00175E41 File Offset: 0x00174E41
		// (set) Token: 0x060020A1 RID: 8353 RVA: 0x00175E49 File Offset: 0x00174E49
		public int ApplicationExitCode
		{
			get
			{
				return this._exitCode;
			}
			set
			{
				this._exitCode = value;
			}
		}

		// Token: 0x04000FEF RID: 4079
		internal int _exitCode;
	}
}
