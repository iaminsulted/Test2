using System;

namespace System.Windows.Controls
{
	// Token: 0x02000751 RID: 1873
	public class DataGridColumnReorderingEventArgs : DataGridColumnEventArgs
	{
		// Token: 0x060065F2 RID: 26098 RVA: 0x002B04B6 File Offset: 0x002AF4B6
		public DataGridColumnReorderingEventArgs(DataGridColumn dataGridColumn) : base(dataGridColumn)
		{
		}

		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x060065F3 RID: 26099 RVA: 0x002B04BF File Offset: 0x002AF4BF
		// (set) Token: 0x060065F4 RID: 26100 RVA: 0x002B04C7 File Offset: 0x002AF4C7
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x060065F5 RID: 26101 RVA: 0x002B04D0 File Offset: 0x002AF4D0
		// (set) Token: 0x060065F6 RID: 26102 RVA: 0x002B04D8 File Offset: 0x002AF4D8
		public Control DropLocationIndicator
		{
			get
			{
				return this._dropLocationIndicator;
			}
			set
			{
				this._dropLocationIndicator = value;
			}
		}

		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x060065F7 RID: 26103 RVA: 0x002B04E1 File Offset: 0x002AF4E1
		// (set) Token: 0x060065F8 RID: 26104 RVA: 0x002B04E9 File Offset: 0x002AF4E9
		public Control DragIndicator
		{
			get
			{
				return this._dragIndicator;
			}
			set
			{
				this._dragIndicator = value;
			}
		}

		// Token: 0x0400339E RID: 13214
		private bool _cancel;

		// Token: 0x0400339F RID: 13215
		private Control _dropLocationIndicator;

		// Token: 0x040033A0 RID: 13216
		private Control _dragIndicator;
	}
}
