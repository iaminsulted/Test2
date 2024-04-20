using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200056A RID: 1386
	public class GridViewItemAutomationPeer : ListBoxItemAutomationPeer
	{
		// Token: 0x0600445A RID: 17498 RVA: 0x00220CCC File Offset: 0x0021FCCC
		public GridViewItemAutomationPeer(object owner, ListViewAutomationPeer listviewAP) : base(owner, listviewAP)
		{
			Invariant.Assert(listviewAP != null);
			this._listviewAP = listviewAP;
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x00220CE6 File Offset: 0x0021FCE6
		protected override string GetClassNameCore()
		{
			return "ListViewItem";
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x001FD61F File Offset: 0x001FC61F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataItem;
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x00220CF0 File Offset: 0x0021FCF0
		protected override List<AutomationPeer> GetChildrenCore()
		{
			ListView listView = this._listviewAP.Owner as ListView;
			Invariant.Assert(listView != null);
			object item = base.Item;
			ListViewItem listViewItem = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
			if (listViewItem != null)
			{
				GridViewRowPresenter gridViewRowPresenter = GridViewAutomationPeer.FindVisualByType(listViewItem, typeof(GridViewRowPresenter)) as GridViewRowPresenter;
				if (gridViewRowPresenter != null)
				{
					Hashtable dataChildren = this._dataChildren;
					this._dataChildren = new Hashtable(gridViewRowPresenter.ActualCells.Count);
					List<AutomationPeer> list = new List<AutomationPeer>();
					int row = listView.Items.IndexOf(item);
					int num = 0;
					foreach (UIElement uielement in gridViewRowPresenter.ActualCells)
					{
						GridViewCellAutomationPeer gridViewCellAutomationPeer = (dataChildren == null) ? null : ((GridViewCellAutomationPeer)dataChildren[uielement]);
						if (gridViewCellAutomationPeer == null)
						{
							if (uielement is ContentPresenter)
							{
								gridViewCellAutomationPeer = new GridViewCellAutomationPeer((ContentPresenter)uielement, this._listviewAP);
							}
							else if (uielement is TextBlock)
							{
								gridViewCellAutomationPeer = new GridViewCellAutomationPeer((TextBlock)uielement, this._listviewAP);
							}
							else
							{
								Invariant.Assert(false, "Children of GridViewRowPresenter should be ContentPresenter or TextBlock");
							}
						}
						if (this._dataChildren[uielement] == null)
						{
							gridViewCellAutomationPeer.Column = num;
							gridViewCellAutomationPeer.Row = row;
							list.Add(gridViewCellAutomationPeer);
							this._dataChildren.Add(uielement, gridViewCellAutomationPeer);
							num++;
						}
					}
					return list;
				}
			}
			return null;
		}

		// Token: 0x0400252C RID: 9516
		private ListViewAutomationPeer _listviewAP;

		// Token: 0x0400252D RID: 9517
		private Hashtable _dataChildren;
	}
}
