using System;

namespace System.Windows.Data
{
	// Token: 0x0200045C RID: 1116
	public class FilterEventArgs : EventArgs
	{
		// Token: 0x060038D5 RID: 14549 RVA: 0x001EA032 File Offset: 0x001E9032
		internal FilterEventArgs(object item)
		{
			this._item = item;
			this._accepted = true;
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x060038D6 RID: 14550 RVA: 0x001EA048 File Offset: 0x001E9048
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x060038D7 RID: 14551 RVA: 0x001EA050 File Offset: 0x001E9050
		// (set) Token: 0x060038D8 RID: 14552 RVA: 0x001EA058 File Offset: 0x001E9058
		public bool Accepted
		{
			get
			{
				return this._accepted;
			}
			set
			{
				this._accepted = value;
			}
		}

		// Token: 0x04001D50 RID: 7504
		private object _item;

		// Token: 0x04001D51 RID: 7505
		private bool _accepted;
	}
}
