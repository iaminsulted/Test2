using System;

namespace System.Windows.Controls
{
	// Token: 0x020007A0 RID: 1952
	internal class DefaultItemContainerTemplateSelector : ItemContainerTemplateSelector
	{
		// Token: 0x06006D9F RID: 28063 RVA: 0x002CE3E2 File Offset: 0x002CD3E2
		public override DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
		{
			return FrameworkElement.FindTemplateResourceInternal(parentItemsControl, item, typeof(ItemContainerTemplate)) as DataTemplate;
		}
	}
}
