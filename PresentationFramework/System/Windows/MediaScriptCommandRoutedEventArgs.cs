using System;

namespace System.Windows
{
	// Token: 0x0200034C RID: 844
	public sealed class MediaScriptCommandRoutedEventArgs : RoutedEventArgs
	{
		// Token: 0x0600200E RID: 8206 RVA: 0x00174411 File Offset: 0x00173411
		internal MediaScriptCommandRoutedEventArgs(RoutedEvent routedEvent, object sender, string parameterType, string parameterValue) : base(routedEvent, sender)
		{
			if (parameterType == null)
			{
				throw new ArgumentNullException("parameterType");
			}
			if (parameterValue == null)
			{
				throw new ArgumentNullException("parameterValue");
			}
			this._parameterType = parameterType;
			this._parameterValue = parameterValue;
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600200F RID: 8207 RVA: 0x00174447 File Offset: 0x00173447
		public string ParameterType
		{
			get
			{
				return this._parameterType;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06002010 RID: 8208 RVA: 0x0017444F File Offset: 0x0017344F
		public string ParameterValue
		{
			get
			{
				return this._parameterValue;
			}
		}

		// Token: 0x04000FB8 RID: 4024
		private string _parameterType;

		// Token: 0x04000FB9 RID: 4025
		private string _parameterValue;
	}
}
