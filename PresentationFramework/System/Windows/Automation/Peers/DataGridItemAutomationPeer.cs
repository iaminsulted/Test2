using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000554 RID: 1364
	public sealed class DataGridItemAutomationPeer : ItemAutomationPeer, IInvokeProvider, IScrollItemProvider, ISelectionItemProvider, ISelectionProvider, IItemContainerProvider
	{
		// Token: 0x06004362 RID: 17250 RVA: 0x0021E2C8 File Offset: 0x0021D2C8
		public DataGridItemAutomationPeer(object item, DataGridAutomationPeer dataGridPeer) : base(item, dataGridPeer)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (dataGridPeer == null)
			{
				throw new ArgumentNullException("dataGridPeer");
			}
			this._dataGridAutomationPeer = dataGridPeer;
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x001FD61F File Offset: 0x001FC61F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataItem;
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x0021E318 File Offset: 0x0021D318
		protected override List<AutomationPeer> GetChildrenCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				wrapperPeer.ForceEnsureChildren();
				return wrapperPeer.GetChildren();
			}
			return this.GetCellItemPeers();
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x0021DD94 File Offset: 0x0021CD94
		protected override string GetClassNameCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClassName();
			}
			base.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x0021E344 File Offset: 0x0021D344
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.Selection)
			{
				if (patternInterface != PatternInterface.Invoke)
				{
					if (patternInterface != PatternInterface.Selection)
					{
						goto IL_38;
					}
				}
				else
				{
					if (!this.OwningDataGrid.IsReadOnly)
					{
						return this;
					}
					goto IL_38;
				}
			}
			else if (patternInterface != PatternInterface.ScrollItem)
			{
				if (patternInterface != PatternInterface.SelectionItem)
				{
					if (patternInterface != PatternInterface.ItemContainer)
					{
						goto IL_38;
					}
				}
				else
				{
					if (this.IsRowSelectionUnit)
					{
						return this;
					}
					goto IL_38;
				}
			}
			return this;
			IL_38:
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0021E390 File Offset: 0x0021D390
		protected override AutomationPeer GetPeerFromPointCore(Point point)
		{
			if (!base.IsOffscreen())
			{
				AutomationPeer rowHeaderAutomationPeer = this.RowHeaderAutomationPeer;
				if (rowHeaderAutomationPeer != null)
				{
					AutomationPeer peerFromPoint = rowHeaderAutomationPeer.GetPeerFromPoint(point);
					if (peerFromPoint != null)
					{
						return peerFromPoint;
					}
				}
			}
			return base.GetPeerFromPointCore(point);
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x0021E3C4 File Offset: 0x0021D3C4
		IRawElementProviderSimple IItemContainerProvider.FindItemByProperty(IRawElementProviderSimple startAfter, int propertyId, object value)
		{
			base.ResetChildrenCache();
			if (propertyId != 0 && !SelectorAutomationPeer.IsPropertySupportedByControlForFindItemInternal(propertyId))
			{
				throw new ArgumentException(SR.Get("PropertyNotSupported"));
			}
			IList<DataGridColumn> columns = this.OwningDataGrid.Columns;
			if (columns != null && columns.Count > 0)
			{
				DataGridCellItemAutomationPeer dataGridCellItemAutomationPeer = null;
				if (startAfter != null)
				{
					dataGridCellItemAutomationPeer = (base.PeerFromProvider(startAfter) as DataGridCellItemAutomationPeer);
				}
				int num = 0;
				if (dataGridCellItemAutomationPeer != null)
				{
					if (dataGridCellItemAutomationPeer.Column == null)
					{
						throw new InvalidOperationException(SR.Get("InavalidStartItem"));
					}
					num = columns.IndexOf(dataGridCellItemAutomationPeer.Column) + 1;
					if (num == 0 || num == columns.Count)
					{
						return null;
					}
				}
				if (propertyId == 0 && num < columns.Count)
				{
					return base.ProviderFromPeer(this.GetOrCreateCellItemPeer(columns[num]));
				}
				object obj = null;
				for (int i = num; i < columns.Count; i++)
				{
					DataGridCellItemAutomationPeer orCreateCellItemPeer = this.GetOrCreateCellItemPeer(columns[i]);
					if (orCreateCellItemPeer != null)
					{
						try
						{
							obj = SelectorAutomationPeer.GetSupportedPropertyValueInternal(orCreateCellItemPeer, propertyId);
						}
						catch (Exception ex)
						{
							if (ex is ElementNotAvailableException)
							{
								goto IL_104;
							}
						}
						if (value == null || obj == null)
						{
							if (obj == null && value == null)
							{
								return base.ProviderFromPeer(orCreateCellItemPeer);
							}
						}
						else if (value.Equals(obj))
						{
							return base.ProviderFromPeer(orCreateCellItemPeer);
						}
					}
					IL_104:;
				}
			}
			return null;
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x0021E4F8 File Offset: 0x0021D4F8
		void IInvokeProvider.Invoke()
		{
			this.EnsureEnabled();
			object item = base.Item;
			if (this.GetWrapperPeer() == null)
			{
				this.OwningDataGrid.ScrollIntoView(item);
			}
			bool flag = false;
			if (base.GetWrapper() != null)
			{
				if (((IEditableCollectionView)this.OwningDataGrid.Items).CurrentEditItem == item)
				{
					flag = this.OwningDataGrid.CommitEdit();
				}
				else if (this.OwningDataGrid.Columns.Count > 0)
				{
					DataGridCell dataGridCell = this.OwningDataGrid.TryFindCell(item, this.OwningDataGrid.Columns[0]);
					if (dataGridCell != null)
					{
						this.OwningDataGrid.UnselectAll();
						dataGridCell.Focus();
						flag = this.OwningDataGrid.BeginEdit();
					}
				}
			}
			if (!flag && !this.IsNewItemPlaceholder)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_AutomationInvokeFailed"));
			}
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0021E5BE File Offset: 0x0021D5BE
		void IScrollItemProvider.ScrollIntoView()
		{
			this.OwningDataGrid.ScrollIntoView(base.Item);
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x0600436B RID: 17259 RVA: 0x0021E5D1 File Offset: 0x0021D5D1
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return this.OwningDataGrid.SelectedItems.Contains(base.Item);
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x0600436C RID: 17260 RVA: 0x0021E5E9 File Offset: 0x0021D5E9
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return base.ProviderFromPeer(this._dataGridAutomationPeer);
			}
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x0021E5F8 File Offset: 0x0021D5F8
		void ISelectionItemProvider.AddToSelection()
		{
			if (!this.IsRowSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGridRow_CannotSelectRowWhenCells"));
			}
			object item = base.Item;
			if (this.OwningDataGrid.SelectedItems.Contains(item))
			{
				return;
			}
			this.EnsureEnabled();
			if (this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Single && this.OwningDataGrid.SelectedItems.Count > 0)
			{
				throw new InvalidOperationException();
			}
			if (this.OwningDataGrid.Items.Contains(item))
			{
				this.OwningDataGrid.SelectedItems.Add(item);
			}
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x0021E68C File Offset: 0x0021D68C
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!this.IsRowSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGridRow_CannotSelectRowWhenCells"));
			}
			this.EnsureEnabled();
			object item = base.Item;
			if (this.OwningDataGrid.SelectedItems.Contains(item))
			{
				this.OwningDataGrid.SelectedItems.Remove(item);
			}
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0021E6E2 File Offset: 0x0021D6E2
		void ISelectionItemProvider.Select()
		{
			if (!this.IsRowSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGridRow_CannotSelectRowWhenCells"));
			}
			this.EnsureEnabled();
			this.OwningDataGrid.SelectedItem = base.Item;
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06004370 RID: 17264 RVA: 0x0021E713 File Offset: 0x0021D713
		bool ISelectionProvider.CanSelectMultiple
		{
			get
			{
				return this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Extended;
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06004371 RID: 17265 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x0021E724 File Offset: 0x0021D724
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			DataGrid owningDataGrid = this.OwningDataGrid;
			if (owningDataGrid == null)
			{
				return null;
			}
			int num = owningDataGrid.Items.IndexOf(base.Item);
			if (num > -1 && owningDataGrid.SelectedCellsInternal.Intersects(num))
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
				for (int i = 0; i < this.OwningDataGrid.Columns.Count; i++)
				{
					if (owningDataGrid.SelectedCellsInternal.Contains(num, i))
					{
						DataGridColumn column = owningDataGrid.ColumnFromDisplayIndex(i);
						DataGridCellItemAutomationPeer orCreateCellItemPeer = this.GetOrCreateCellItemPeer(column);
						if (orCreateCellItemPeer != null)
						{
							list.Add(base.ProviderFromPeer(orCreateCellItemPeer));
						}
					}
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x0021E7C8 File Offset: 0x0021D7C8
		internal List<AutomationPeer> GetCellItemPeers()
		{
			List<AutomationPeer> list = null;
			ItemPeersStorage<DataGridCellItemAutomationPeer> itemPeersStorage = new ItemPeersStorage<DataGridCellItemAutomationPeer>();
			IList list2 = null;
			bool flag = false;
			DataGridRow dataGridRow = base.GetWrapper() as DataGridRow;
			if (dataGridRow != null && dataGridRow.CellsPresenter != null)
			{
				Panel itemsHost = dataGridRow.CellsPresenter.ItemsHost;
				if (itemsHost != null)
				{
					list2 = itemsHost.Children;
					flag = true;
				}
			}
			if (!flag)
			{
				list2 = this.OwningDataGrid.Columns;
			}
			if (list2 != null)
			{
				list = new List<AutomationPeer>(list2.Count);
				foreach (object obj in list2)
				{
					DataGridColumn dataGridColumn;
					if (flag)
					{
						dataGridColumn = (obj as DataGridCell).Column;
					}
					else
					{
						dataGridColumn = (obj as DataGridColumn);
					}
					if (dataGridColumn != null)
					{
						DataGridCellItemAutomationPeer orCreateCellItemPeer = this.GetOrCreateCellItemPeer(dataGridColumn, false);
						list.Add(orCreateCellItemPeer);
						itemPeersStorage[dataGridColumn] = orCreateCellItemPeer;
					}
				}
			}
			this.CellItemPeers = itemPeersStorage;
			return list;
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x0021E8C0 File Offset: 0x0021D8C0
		internal DataGridCellItemAutomationPeer GetOrCreateCellItemPeer(DataGridColumn column)
		{
			return this.GetOrCreateCellItemPeer(column, true);
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x0021E8CC File Offset: 0x0021D8CC
		private DataGridCellItemAutomationPeer GetOrCreateCellItemPeer(DataGridColumn column, bool addParentInfo)
		{
			DataGridCellItemAutomationPeer dataGridCellItemAutomationPeer = this.CellItemPeers[column];
			if (dataGridCellItemAutomationPeer == null)
			{
				dataGridCellItemAutomationPeer = this.GetPeerFromWeakRefStorage(column);
				if (dataGridCellItemAutomationPeer != null && !addParentInfo)
				{
					dataGridCellItemAutomationPeer.AncestorsInvalid = false;
					dataGridCellItemAutomationPeer.ChildrenValid = false;
				}
			}
			if (dataGridCellItemAutomationPeer == null)
			{
				dataGridCellItemAutomationPeer = new DataGridCellItemAutomationPeer(base.Item, column);
				if (addParentInfo && dataGridCellItemAutomationPeer != null)
				{
					dataGridCellItemAutomationPeer.TrySetParentInfo(this);
				}
			}
			AutomationPeer owningCellPeer = dataGridCellItemAutomationPeer.OwningCellPeer;
			if (owningCellPeer != null)
			{
				owningCellPeer.EventsSource = dataGridCellItemAutomationPeer;
			}
			return dataGridCellItemAutomationPeer;
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x0021E938 File Offset: 0x0021D938
		private DataGridCellItemAutomationPeer GetPeerFromWeakRefStorage(object column)
		{
			DataGridCellItemAutomationPeer dataGridCellItemAutomationPeer = null;
			WeakReference weakReference = this.WeakRefElementProxyStorage[column];
			if (weakReference != null)
			{
				ElementProxy elementProxy = weakReference.Target as ElementProxy;
				if (elementProxy != null)
				{
					dataGridCellItemAutomationPeer = (base.PeerFromProvider(elementProxy) as DataGridCellItemAutomationPeer);
					if (dataGridCellItemAutomationPeer == null)
					{
						this.WeakRefElementProxyStorage.Remove(column);
					}
				}
				else
				{
					this.WeakRefElementProxyStorage.Remove(column);
				}
			}
			return dataGridCellItemAutomationPeer;
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x0021E994 File Offset: 0x0021D994
		internal void AddProxyToWeakRefStorage(WeakReference wr, DataGridCellItemAutomationPeer cellItemPeer)
		{
			IList<DataGridColumn> columns = this.OwningDataGrid.Columns;
			if (columns != null && columns.Contains(cellItemPeer.Column) && this.GetPeerFromWeakRefStorage(cellItemPeer.Column) == null)
			{
				this.WeakRefElementProxyStorage[cellItemPeer.Column] = wr;
			}
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x0021E9DE File Offset: 0x0021D9DE
		private void EnsureEnabled()
		{
			if (!this._dataGridAutomationPeer.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06004379 RID: 17273 RVA: 0x0021E9F3 File Offset: 0x0021D9F3
		private bool IsRowSelectionUnit
		{
			get
			{
				return this.OwningDataGrid != null && (this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.FullRow || this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader);
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x0600437A RID: 17274 RVA: 0x0021EA20 File Offset: 0x0021DA20
		private bool IsNewItemPlaceholder
		{
			get
			{
				object item = base.Item;
				return item == CollectionView.NewItemPlaceholder || item == DataGrid.NewItemPlaceholder;
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x0600437B RID: 17275 RVA: 0x0021EA48 File Offset: 0x0021DA48
		internal AutomationPeer RowHeaderAutomationPeer
		{
			get
			{
				DataGridRowAutomationPeer dataGridRowAutomationPeer = this.GetWrapperPeer() as DataGridRowAutomationPeer;
				if (dataGridRowAutomationPeer == null)
				{
					return null;
				}
				return dataGridRowAutomationPeer.RowHeaderAutomationPeer;
			}
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x0600437C RID: 17276 RVA: 0x0021EA6C File Offset: 0x0021DA6C
		private DataGrid OwningDataGrid
		{
			get
			{
				return (DataGrid)(this._dataGridAutomationPeer as DataGridAutomationPeer).Owner;
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x0600437D RID: 17277 RVA: 0x0021EA83 File Offset: 0x0021DA83
		// (set) Token: 0x0600437E RID: 17278 RVA: 0x0021EA8B File Offset: 0x0021DA8B
		private ItemPeersStorage<DataGridCellItemAutomationPeer> CellItemPeers
		{
			get
			{
				return this._dataChildren;
			}
			set
			{
				this._dataChildren = value;
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x0600437F RID: 17279 RVA: 0x0021EA94 File Offset: 0x0021DA94
		private ItemPeersStorage<WeakReference> WeakRefElementProxyStorage
		{
			get
			{
				return this._weakRefElementProxyStorage;
			}
		}

		// Token: 0x04002518 RID: 9496
		private AutomationPeer _dataGridAutomationPeer;

		// Token: 0x04002519 RID: 9497
		private ItemPeersStorage<DataGridCellItemAutomationPeer> _dataChildren = new ItemPeersStorage<DataGridCellItemAutomationPeer>();

		// Token: 0x0400251A RID: 9498
		private ItemPeersStorage<WeakReference> _weakRefElementProxyStorage = new ItemPeersStorage<WeakReference>();
	}
}
