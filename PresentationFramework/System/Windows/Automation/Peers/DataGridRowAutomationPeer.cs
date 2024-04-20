using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000555 RID: 1365
	public sealed class DataGridRowAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004380 RID: 17280 RVA: 0x0021D244 File Offset: 0x0021C244
		public DataGridRowAutomationPeer(DataGridRow owner) : base(owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x001FD61F File Offset: 0x001FC61F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataItem;
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0021EA9C File Offset: 0x0021DA9C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			DataGridCellsPresenter cellsPresenter = this.OwningDataGridRow.CellsPresenter;
			if (cellsPresenter != null && cellsPresenter.ItemsHost != null)
			{
				List<AutomationPeer> list = new List<AutomationPeer>(3);
				AutomationPeer rowHeaderAutomationPeer = this.RowHeaderAutomationPeer;
				if (rowHeaderAutomationPeer != null)
				{
					list.Add(rowHeaderAutomationPeer);
				}
				DataGridItemAutomationPeer dataGridItemAutomationPeer = base.EventsSource as DataGridItemAutomationPeer;
				if (dataGridItemAutomationPeer != null)
				{
					list.AddRange(dataGridItemAutomationPeer.GetCellItemPeers());
				}
				AutomationPeer detailsPresenterAutomationPeer = this.DetailsPresenterAutomationPeer;
				if (detailsPresenterAutomationPeer != null)
				{
					list.Add(detailsPresenterAutomationPeer);
				}
				return list;
			}
			return base.GetChildrenCore();
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06004384 RID: 17284 RVA: 0x0021EB10 File Offset: 0x0021DB10
		internal AutomationPeer RowHeaderAutomationPeer
		{
			get
			{
				DataGridRowHeader rowHeader = this.OwningDataGridRow.RowHeader;
				if (rowHeader != null)
				{
					return UIElementAutomationPeer.CreatePeerForElement(rowHeader);
				}
				return null;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06004385 RID: 17285 RVA: 0x0021EB34 File Offset: 0x0021DB34
		private AutomationPeer DetailsPresenterAutomationPeer
		{
			get
			{
				DataGridDetailsPresenter detailsPresenter = this.OwningDataGridRow.DetailsPresenter;
				if (detailsPresenter != null)
				{
					return UIElementAutomationPeer.CreatePeerForElement(detailsPresenter);
				}
				return null;
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06004386 RID: 17286 RVA: 0x0021EB58 File Offset: 0x0021DB58
		private DataGridRow OwningDataGridRow
		{
			get
			{
				return (DataGridRow)base.Owner;
			}
		}
	}
}
