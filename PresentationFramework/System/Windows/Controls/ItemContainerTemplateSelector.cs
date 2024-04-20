using System;

namespace System.Windows.Controls
{
	// Token: 0x0200079F RID: 1951
	public abstract class ItemContainerTemplateSelector
	{
		// Token: 0x06006D9D RID: 28061 RVA: 0x00109403 File Offset: 0x00108403
		public virtual DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
		{
			return null;
		}
	}
}
