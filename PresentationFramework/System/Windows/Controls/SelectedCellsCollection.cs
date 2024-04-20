using System;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x020007D0 RID: 2000
	internal sealed class SelectedCellsCollection : VirtualizedCellInfoCollection
	{
		// Token: 0x060072C4 RID: 29380 RVA: 0x002E0399 File Offset: 0x002DF399
		internal SelectedCellsCollection(DataGrid owner) : base(owner)
		{
		}

		// Token: 0x060072C5 RID: 29381 RVA: 0x002E03A2 File Offset: 0x002DF3A2
		internal bool GetSelectionRange(out int minColumnDisplayIndex, out int maxColumnDisplayIndex, out int minRowIndex, out int maxRowIndex)
		{
			if (base.IsEmpty)
			{
				minColumnDisplayIndex = -1;
				maxColumnDisplayIndex = -1;
				minRowIndex = -1;
				maxRowIndex = -1;
				return false;
			}
			base.GetBoundingRegion(out minColumnDisplayIndex, out minRowIndex, out maxColumnDisplayIndex, out maxRowIndex);
			return true;
		}

		// Token: 0x060072C6 RID: 29382 RVA: 0x002E03C7 File Offset: 0x002DF3C7
		protected override void OnCollectionChanged(NotifyCollectionChangedAction action, VirtualizedCellInfoCollection oldItems, VirtualizedCellInfoCollection newItems)
		{
			base.Owner.OnSelectedCellsChanged(action, oldItems, newItems);
		}
	}
}
