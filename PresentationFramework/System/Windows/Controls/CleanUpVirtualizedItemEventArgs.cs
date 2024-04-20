using System;

namespace System.Windows.Controls
{
	// Token: 0x0200072E RID: 1838
	public class CleanUpVirtualizedItemEventArgs : RoutedEventArgs
	{
		// Token: 0x060060A4 RID: 24740 RVA: 0x0029A372 File Offset: 0x00299372
		public CleanUpVirtualizedItemEventArgs(object value, UIElement element) : base(VirtualizingStackPanel.CleanUpVirtualizedItemEvent)
		{
			this._value = value;
			this._element = element;
		}

		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x060060A5 RID: 24741 RVA: 0x0029A38D File Offset: 0x0029938D
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x060060A6 RID: 24742 RVA: 0x0029A395 File Offset: 0x00299395
		public UIElement UIElement
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x060060A7 RID: 24743 RVA: 0x0029A39D File Offset: 0x0029939D
		// (set) Token: 0x060060A8 RID: 24744 RVA: 0x0029A3A5 File Offset: 0x002993A5
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

		// Token: 0x0400323B RID: 12859
		private object _value;

		// Token: 0x0400323C RID: 12860
		private UIElement _element;

		// Token: 0x0400323D RID: 12861
		private bool _cancel;
	}
}
