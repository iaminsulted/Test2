using System;

namespace System.Windows.Controls
{
	// Token: 0x02000794 RID: 1940
	public class InitializingNewItemEventArgs : EventArgs
	{
		// Token: 0x06006BE9 RID: 27625 RVA: 0x002C734B File Offset: 0x002C634B
		public InitializingNewItemEventArgs(object newItem)
		{
			this._newItem = newItem;
		}

		// Token: 0x170018F3 RID: 6387
		// (get) Token: 0x06006BEA RID: 27626 RVA: 0x002C735A File Offset: 0x002C635A
		public object NewItem
		{
			get
			{
				return this._newItem;
			}
		}

		// Token: 0x040035CC RID: 13772
		private object _newItem;
	}
}
