using System;

namespace System.Windows.Controls
{
	// Token: 0x02000762 RID: 1890
	public class DataGridRowDetailsEventArgs : EventArgs
	{
		// Token: 0x060066F1 RID: 26353 RVA: 0x002B3692 File Offset: 0x002B2692
		public DataGridRowDetailsEventArgs(DataGridRow row, FrameworkElement detailsElement)
		{
			this.Row = row;
			this.DetailsElement = detailsElement;
		}

		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x060066F2 RID: 26354 RVA: 0x002B36A8 File Offset: 0x002B26A8
		// (set) Token: 0x060066F3 RID: 26355 RVA: 0x002B36B0 File Offset: 0x002B26B0
		public FrameworkElement DetailsElement { get; private set; }

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x060066F4 RID: 26356 RVA: 0x002B36B9 File Offset: 0x002B26B9
		// (set) Token: 0x060066F5 RID: 26357 RVA: 0x002B36C1 File Offset: 0x002B26C1
		public DataGridRow Row { get; private set; }
	}
}
