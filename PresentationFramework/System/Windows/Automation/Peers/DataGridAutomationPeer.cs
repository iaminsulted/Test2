using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200054D RID: 1357
	public sealed class DataGridAutomationPeer : ItemsControlAutomationPeer, IGridProvider, ISelectionProvider, ITableProvider
	{
		// Token: 0x060042E4 RID: 17124 RVA: 0x0021CB95 File Offset: 0x0021BB95
		public DataGridAutomationPeer(DataGrid owner) : base(owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x001FD464 File Offset: 0x001FC464
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataGrid;
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x0021CBAC File Offset: 0x0021BBAC
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			DataGridColumnHeadersPresenter columnHeadersPresenter = this.OwningDataGrid.ColumnHeadersPresenter;
			if (columnHeadersPresenter != null && columnHeadersPresenter.IsVisible)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(columnHeadersPresenter);
				if (automationPeer != null)
				{
					if (list == null)
					{
						list = new List<AutomationPeer>(1);
					}
					list.Insert(0, automationPeer);
				}
			}
			return list;
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x0021CBF4 File Offset: 0x0021BBF4
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.Scroll)
			{
				if (patternInterface != PatternInterface.Selection)
				{
					if (patternInterface != PatternInterface.Scroll)
					{
						goto IL_45;
					}
					ScrollViewer internalScrollHost = this.OwningDataGrid.InternalScrollHost;
					if (internalScrollHost == null)
					{
						goto IL_45;
					}
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(internalScrollHost);
					IScrollProvider scrollProvider = automationPeer as IScrollProvider;
					if (automationPeer != null && scrollProvider != null)
					{
						automationPeer.EventsSource = this;
						return scrollProvider;
					}
					goto IL_45;
				}
			}
			else if (patternInterface != PatternInterface.Grid && patternInterface != PatternInterface.Table)
			{
				goto IL_45;
			}
			return this;
			IL_45:
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x0021CC4D File Offset: 0x0021BC4D
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new DataGridItemAutomationPeer(item, this);
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x0021CC56 File Offset: 0x0021BC56
		internal override bool IsPropertySupportedByControlForFindItem(int id)
		{
			return SelectorAutomationPeer.IsPropertySupportedByControlForFindItemInternal(id);
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x0021CC5E File Offset: 0x0021BC5E
		internal override object GetSupportedPropertyValue(ItemAutomationPeer itemPeer, int propertyId)
		{
			return SelectorAutomationPeer.GetSupportedPropertyValueInternal(itemPeer, propertyId);
		}

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x060042EC RID: 17132 RVA: 0x0021CC67 File Offset: 0x0021BC67
		int IGridProvider.ColumnCount
		{
			get
			{
				return this.OwningDataGrid.Columns.Count;
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x060042ED RID: 17133 RVA: 0x0021CC79 File Offset: 0x0021BC79
		int IGridProvider.RowCount
		{
			get
			{
				return this.OwningDataGrid.Items.Count;
			}
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x0021CC8C File Offset: 0x0021BC8C
		IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
		{
			if (row >= 0 && row < this.OwningDataGrid.Items.Count && column >= 0 && column < this.OwningDataGrid.Columns.Count)
			{
				object item = this.OwningDataGrid.Items[row];
				DataGridColumn column2 = this.OwningDataGrid.Columns[column];
				this.OwningDataGrid.ScrollIntoView(item, column2);
				this.OwningDataGrid.UpdateLayout();
				DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(item) as DataGridItemAutomationPeer;
				if (dataGridItemAutomationPeer != null)
				{
					DataGridCellItemAutomationPeer orCreateCellItemPeer = dataGridItemAutomationPeer.GetOrCreateCellItemPeer(column2);
					if (orCreateCellItemPeer != null)
					{
						return base.ProviderFromPeer(orCreateCellItemPeer);
					}
				}
			}
			return null;
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x0021CD2C File Offset: 0x0021BD2C
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
			switch (this.OwningDataGrid.SelectionUnit)
			{
			case DataGridSelectionUnit.Cell:
				this.AddSelectedCells(list);
				break;
			case DataGridSelectionUnit.FullRow:
				this.AddSelectedRows(list);
				break;
			case DataGridSelectionUnit.CellOrRowHeader:
				this.AddSelectedRows(list);
				this.AddSelectedCells(list);
				break;
			}
			return list.ToArray();
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x060042F0 RID: 17136 RVA: 0x0021CD85 File Offset: 0x0021BD85
		bool ISelectionProvider.CanSelectMultiple
		{
			get
			{
				return this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Extended;
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x060042F1 RID: 17137 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x060042F2 RID: 17138 RVA: 0x00105F35 File Offset: 0x00104F35
		RowOrColumnMajor ITableProvider.RowOrColumnMajor
		{
			get
			{
				return RowOrColumnMajor.RowMajor;
			}
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x0021CD98 File Offset: 0x0021BD98
		IRawElementProviderSimple[] ITableProvider.GetColumnHeaders()
		{
			if ((this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Column) == DataGridHeadersVisibility.Column)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
				DataGridColumnHeadersPresenter columnHeadersPresenter = this.OwningDataGrid.ColumnHeadersPresenter;
				if (columnHeadersPresenter != null)
				{
					DataGridColumnHeadersPresenterAutomationPeer dataGridColumnHeadersPresenterAutomationPeer = columnHeadersPresenter.GetAutomationPeer() as DataGridColumnHeadersPresenterAutomationPeer;
					if (dataGridColumnHeadersPresenterAutomationPeer != null)
					{
						for (int i = 0; i < this.OwningDataGrid.Columns.Count; i++)
						{
							AutomationPeer automationPeer = dataGridColumnHeadersPresenterAutomationPeer.FindOrCreateItemAutomationPeer(this.OwningDataGrid.Columns[i]);
							if (automationPeer != null)
							{
								list.Add(base.ProviderFromPeer(automationPeer));
							}
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x0021CE30 File Offset: 0x0021BE30
		IRawElementProviderSimple[] ITableProvider.GetRowHeaders()
		{
			if ((this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Row) == DataGridHeadersVisibility.Row)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
				foreach (object item in ((IEnumerable)this.OwningDataGrid.Items))
				{
					AutomationPeer rowHeaderAutomationPeer = (this.FindOrCreateItemAutomationPeer(item) as DataGridItemAutomationPeer).RowHeaderAutomationPeer;
					if (rowHeaderAutomationPeer != null)
					{
						list.Add(base.ProviderFromPeer(rowHeaderAutomationPeer));
					}
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x060042F5 RID: 17141 RVA: 0x0021CED0 File Offset: 0x0021BED0
		private DataGrid OwningDataGrid
		{
			get
			{
				return (DataGrid)base.Owner;
			}
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x0021CEE0 File Offset: 0x0021BEE0
		private DataGridCellItemAutomationPeer GetCellItemPeer(DataGridCellInfo cellInfo)
		{
			if (cellInfo.IsValid)
			{
				DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(cellInfo.Item) as DataGridItemAutomationPeer;
				if (dataGridItemAutomationPeer != null)
				{
					return dataGridItemAutomationPeer.GetOrCreateCellItemPeer(cellInfo.Column);
				}
			}
			return null;
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x0021CF1C File Offset: 0x0021BF1C
		internal void RaiseAutomationCellSelectedEvent(SelectedCellsChangedEventArgs e)
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) && this.OwningDataGrid.SelectedCells.Count == 1 && e.AddedCells.Count == 1)
			{
				DataGridCellItemAutomationPeer cellItemPeer = this.GetCellItemPeer(e.AddedCells[0]);
				if (cellItemPeer != null)
				{
					cellItemPeer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
					return;
				}
			}
			else
			{
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection))
				{
					for (int i = 0; i < e.AddedCells.Count; i++)
					{
						DataGridCellItemAutomationPeer cellItemPeer2 = this.GetCellItemPeer(e.AddedCells[i]);
						if (cellItemPeer2 != null)
						{
							cellItemPeer2.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
						}
					}
				}
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
				{
					for (int i = 0; i < e.RemovedCells.Count; i++)
					{
						DataGridCellItemAutomationPeer cellItemPeer3 = this.GetCellItemPeer(e.RemovedCells[i]);
						if (cellItemPeer3 != null)
						{
							cellItemPeer3.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
						}
					}
				}
			}
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x0021CFE8 File Offset: 0x0021BFE8
		internal void RaiseAutomationRowInvokeEvents(DataGridRow row)
		{
			DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(row.Item) as DataGridItemAutomationPeer;
			if (dataGridItemAutomationPeer != null)
			{
				dataGridItemAutomationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
			}
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x0021D014 File Offset: 0x0021C014
		internal void RaiseAutomationCellInvokeEvents(DataGridColumn column, DataGridRow row)
		{
			DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(row.Item) as DataGridItemAutomationPeer;
			if (dataGridItemAutomationPeer != null)
			{
				DataGridCellItemAutomationPeer orCreateCellItemPeer = dataGridItemAutomationPeer.GetOrCreateCellItemPeer(column);
				if (orCreateCellItemPeer != null)
				{
					orCreateCellItemPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x0021D048 File Offset: 0x0021C048
		internal void RaiseAutomationSelectionEvents(SelectionChangedEventArgs e)
		{
			int count = this.OwningDataGrid.SelectedItems.Count;
			int count2 = e.AddedItems.Count;
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) && count == 1 && count2 == 1)
			{
				ItemAutomationPeer itemAutomationPeer = this.FindOrCreateItemAutomationPeer(this.OwningDataGrid.SelectedItem);
				if (itemAutomationPeer != null)
				{
					itemAutomationPeer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
					return;
				}
			}
			else
			{
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection))
				{
					for (int i = 0; i < e.AddedItems.Count; i++)
					{
						ItemAutomationPeer itemAutomationPeer2 = this.FindOrCreateItemAutomationPeer(e.AddedItems[i]);
						if (itemAutomationPeer2 != null)
						{
							itemAutomationPeer2.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
						}
					}
				}
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
				{
					for (int i = 0; i < e.RemovedItems.Count; i++)
					{
						ItemAutomationPeer itemAutomationPeer3 = this.FindOrCreateItemAutomationPeer(e.RemovedItems[i]);
						if (itemAutomationPeer3 != null)
						{
							itemAutomationPeer3.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
						}
					}
				}
			}
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x0021D120 File Offset: 0x0021C120
		private void AddSelectedCells(List<IRawElementProviderSimple> cellProviders)
		{
			if (cellProviders == null)
			{
				throw new ArgumentNullException("cellProviders");
			}
			if (this.OwningDataGrid.SelectedCells != null)
			{
				foreach (DataGridCellInfo dataGridCellInfo in this.OwningDataGrid.SelectedCells)
				{
					DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(dataGridCellInfo.Item) as DataGridItemAutomationPeer;
					if (dataGridItemAutomationPeer != null)
					{
						IRawElementProviderSimple rawElementProviderSimple = base.ProviderFromPeer(dataGridItemAutomationPeer.GetOrCreateCellItemPeer(dataGridCellInfo.Column));
						if (rawElementProviderSimple != null)
						{
							cellProviders.Add(rawElementProviderSimple);
						}
					}
				}
			}
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x0021D1BC File Offset: 0x0021C1BC
		private void AddSelectedRows(List<IRawElementProviderSimple> itemProviders)
		{
			if (itemProviders == null)
			{
				throw new ArgumentNullException("itemProviders");
			}
			if (this.OwningDataGrid.SelectedItems != null)
			{
				foreach (object item in this.OwningDataGrid.SelectedItems)
				{
					IRawElementProviderSimple rawElementProviderSimple = base.ProviderFromPeer(this.FindOrCreateItemAutomationPeer(item));
					if (rawElementProviderSimple != null)
					{
						itemProviders.Add(rawElementProviderSimple);
					}
				}
			}
		}
	}
}
