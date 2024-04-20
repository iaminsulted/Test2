using System;
using System.ComponentModel;

namespace MS.Internal.Data
{
	// Token: 0x02000243 RID: 579
	internal class ValueChangedEventArgs : EventArgs
	{
		// Token: 0x06001662 RID: 5730 RVA: 0x0015A6D4 File Offset: 0x001596D4
		internal ValueChangedEventArgs(PropertyDescriptor pd)
		{
			this._pd = pd;
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001663 RID: 5731 RVA: 0x0015A6E3 File Offset: 0x001596E3
		internal PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this._pd;
			}
		}

		// Token: 0x04000C5A RID: 3162
		private PropertyDescriptor _pd;
	}
}
