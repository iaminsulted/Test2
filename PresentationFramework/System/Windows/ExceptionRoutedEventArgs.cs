using System;

namespace System.Windows
{
	// Token: 0x0200034B RID: 843
	public sealed class ExceptionRoutedEventArgs : RoutedEventArgs
	{
		// Token: 0x0600200C RID: 8204 RVA: 0x001743EA File Offset: 0x001733EA
		internal ExceptionRoutedEventArgs(RoutedEvent routedEvent, object sender, Exception errorException) : base(routedEvent, sender)
		{
			if (errorException == null)
			{
				throw new ArgumentNullException("errorException");
			}
			this._errorException = errorException;
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600200D RID: 8205 RVA: 0x00174409 File Offset: 0x00173409
		public Exception ErrorException
		{
			get
			{
				return this._errorException;
			}
		}

		// Token: 0x04000FB7 RID: 4023
		private Exception _errorException;
	}
}
