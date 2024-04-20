using System;
using System.Windows.Markup;

namespace System.Windows.Controls
{
	// Token: 0x0200079D RID: 1949
	[DictionaryKeyProperty("ItemContainerTemplateKey")]
	public class ItemContainerTemplate : DataTemplate
	{
		// Token: 0x1700195E RID: 6494
		// (get) Token: 0x06006D99 RID: 28057 RVA: 0x002CE3B8 File Offset: 0x002CD3B8
		public object ItemContainerTemplateKey
		{
			get
			{
				if (base.DataType == null)
				{
					return null;
				}
				return new ItemContainerTemplateKey(base.DataType);
			}
		}
	}
}
