using System;

namespace MS.Internal.Data
{
	// Token: 0x02000207 RID: 519
	internal class BindingValueChangedEventArgs : EventArgs
	{
		// Token: 0x060012E4 RID: 4836 RVA: 0x0014BE49 File Offset: 0x0014AE49
		internal BindingValueChangedEventArgs(object oldValue, object newValue)
		{
			this._oldValue = oldValue;
			this._newValue = newValue;
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x0014BE5F File Offset: 0x0014AE5F
		public object OldValue
		{
			get
			{
				return this._oldValue;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x0014BE67 File Offset: 0x0014AE67
		public object NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		// Token: 0x04000B69 RID: 2921
		private object _oldValue;

		// Token: 0x04000B6A RID: 2922
		private object _newValue;
	}
}
