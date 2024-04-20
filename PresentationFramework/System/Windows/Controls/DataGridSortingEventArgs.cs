using System;

namespace System.Windows.Controls
{
	// Token: 0x02000768 RID: 1896
	public class DataGridSortingEventArgs : DataGridColumnEventArgs
	{
		// Token: 0x060066FE RID: 26366 RVA: 0x002B04B6 File Offset: 0x002AF4B6
		public DataGridSortingEventArgs(DataGridColumn column) : base(column)
		{
		}

		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x060066FF RID: 26367 RVA: 0x002B3721 File Offset: 0x002B2721
		// (set) Token: 0x06006700 RID: 26368 RVA: 0x002B3729 File Offset: 0x002B2729
		public bool Handled
		{
			get
			{
				return this._handled;
			}
			set
			{
				this._handled = value;
			}
		}

		// Token: 0x04003422 RID: 13346
		private bool _handled;
	}
}
