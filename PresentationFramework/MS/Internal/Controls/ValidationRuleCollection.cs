using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000261 RID: 609
	internal class ValidationRuleCollection : Collection<ValidationRule>
	{
		// Token: 0x0600178D RID: 6029 RVA: 0x0015E98F File Offset: 0x0015D98F
		protected override void InsertItem(int index, ValidationRule item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0015E9A7 File Offset: 0x0015D9A7
		protected override void SetItem(int index, ValidationRule item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}
	}
}
