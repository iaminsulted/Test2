using System;

namespace System.Windows.Controls
{
	// Token: 0x020007AA RID: 1962
	public class ListViewItem : ListBoxItem
	{
		// Token: 0x06006ED8 RID: 28376 RVA: 0x002D3310 File Offset: 0x002D2310
		internal void SetDefaultStyleKey(object key)
		{
			base.DefaultStyleKey = key;
		}

		// Token: 0x06006ED9 RID: 28377 RVA: 0x002D3319 File Offset: 0x002D2319
		internal void ClearDefaultStyleKey()
		{
			base.ClearValue(FrameworkElement.DefaultStyleKeyProperty);
		}
	}
}
