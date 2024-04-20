using System;

namespace System.Windows.Controls
{
	// Token: 0x02000717 RID: 1815
	public class AddingNewItemEventArgs : EventArgs
	{
		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x06005F82 RID: 24450 RVA: 0x002953A6 File Offset: 0x002943A6
		// (set) Token: 0x06005F83 RID: 24451 RVA: 0x002953AE File Offset: 0x002943AE
		public object NewItem
		{
			get
			{
				return this._newItem;
			}
			set
			{
				this._newItem = value;
			}
		}

		// Token: 0x040031D3 RID: 12755
		private object _newItem;
	}
}
