using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200054F RID: 1359
	public sealed class DataGridCellItemAutomationPeer : AutomationPeer, IGridItemProvider, ITableItemProvider, IInvokeProvider, IScrollItemProvider, ISelectionItemProvider, IValueProvider, IVirtualizedItemProvider
	{
		// Token: 0x06004300 RID: 17152 RVA: 0x0021D25B File Offset: 0x0021C25B
		public DataGridCellItemAutomationPeer(object item, DataGridColumn dataGridColumn)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (dataGridColumn == null)
			{
				throw new ArgumentNullException("dataGridColumn");
			}
			this._item = new WeakReference(item);
			this._column = dataGridColumn;
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x0021D294 File Offset: 0x0021C294
		protected override string GetAcceleratorKeyCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetAcceleratorKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x0021D2C0 File Offset: 0x0021C2C0
		protected override string GetAccessKeyCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetAccessKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x001FBDEE File Offset: 0x001FADEE
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x0021D2EC File Offset: 0x0021C2EC
		protected override string GetAutomationIdCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetAutomationId();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x0021D318 File Offset: 0x0021C318
		protected override Rect GetBoundingRectangleCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetBoundingRectangle();
			}
			this.ThrowElementNotAvailableException();
			return default(Rect);
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x0021D348 File Offset: 0x0021C348
		protected override List<AutomationPeer> GetChildrenCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				owningCellPeer.ForceEnsureChildren();
				return owningCellPeer.GetChildren();
			}
			return null;
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x0021D370 File Offset: 0x0021C370
		protected override string GetClassNameCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetClassName();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x0021D39C File Offset: 0x0021C39C
		protected override Point GetClickablePointCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetClickablePoint();
			}
			this.ThrowElementNotAvailableException();
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x0021D3D8 File Offset: 0x0021C3D8
		protected override string GetHelpTextCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetHelpText();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x0021D404 File Offset: 0x0021C404
		protected override string GetItemStatusCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetItemStatus();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x0021D430 File Offset: 0x0021C430
		protected override string GetItemTypeCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetItemType();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x0021D45C File Offset: 0x0021C45C
		protected override AutomationPeer GetLabeledByCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetLabeledBy();
			}
			this.ThrowElementNotAvailableException();
			return null;
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x0021D481 File Offset: 0x0021C481
		protected override string GetLocalizedControlTypeCore()
		{
			if (!AccessibilitySwitches.UseNetFx47CompatibleAccessibilityFeatures)
			{
				return SR.Get("DataGridCellItemAutomationPeer_LocalizedControlType");
			}
			return base.GetLocalizedControlTypeCore();
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x0021D49C File Offset: 0x0021C49C
		protected override AutomationLiveSetting GetLiveSettingCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			AutomationLiveSetting result = AutomationLiveSetting.Off;
			if (owningCellPeer != null)
			{
				result = owningCellPeer.GetLiveSetting();
			}
			else
			{
				this.ThrowElementNotAvailableException();
			}
			return result;
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x0021D4C8 File Offset: 0x0021C4C8
		protected override string GetNameCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			string text = null;
			if (owningCellPeer != null)
			{
				text = owningCellPeer.GetName();
			}
			if (string.IsNullOrEmpty(text))
			{
				text = SR.Get("DataGridCellItemAutomationPeer_NameCoreFormat", new object[]
				{
					this.Item,
					this._column.DisplayIndex
				});
			}
			return text;
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x0021D520 File Offset: 0x0021C520
		protected override AutomationOrientation GetOrientationCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetOrientation();
			}
			this.ThrowElementNotAvailableException();
			return AutomationOrientation.None;
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x0021D548 File Offset: 0x0021C548
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.ScrollItem)
			{
				if (patternInterface != PatternInterface.Invoke)
				{
					if (patternInterface != PatternInterface.Value)
					{
						if (patternInterface != PatternInterface.ScrollItem)
						{
							goto IL_8C;
						}
					}
					else
					{
						if (!this.IsNewItemPlaceholder)
						{
							return this;
						}
						goto IL_8C;
					}
				}
				else
				{
					if (!this.OwningDataGrid.IsReadOnly && !this._column.IsReadOnly)
					{
						return this;
					}
					goto IL_8C;
				}
			}
			else if (patternInterface <= PatternInterface.SelectionItem)
			{
				if (patternInterface != PatternInterface.GridItem)
				{
					if (patternInterface != PatternInterface.SelectionItem)
					{
						goto IL_8C;
					}
					if (this.IsCellSelectionUnit)
					{
						return this;
					}
					goto IL_8C;
				}
			}
			else if (patternInterface != PatternInterface.TableItem)
			{
				if (patternInterface != PatternInterface.VirtualizedItem)
				{
					goto IL_8C;
				}
				if (VirtualizedItemPatternIdentifiers.Pattern == null)
				{
					goto IL_8C;
				}
				if (this.OwningCellPeer == null)
				{
					return this;
				}
				if (this.OwningItemPeer != null && !this.IsItemInAutomationTree())
				{
					return this;
				}
				if (this.OwningItemPeer == null)
				{
					return this;
				}
				goto IL_8C;
			}
			return this;
			IL_8C:
			return null;
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x0021D5E4 File Offset: 0x0021C5E4
		protected override int GetPositionInSetCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			int result = -1;
			if (owningCellPeer != null)
			{
				result = owningCellPeer.GetPositionInSet();
			}
			else
			{
				this.ThrowElementNotAvailableException();
			}
			return result;
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x0021D610 File Offset: 0x0021C610
		protected override int GetSizeOfSetCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			int result = -1;
			if (owningCellPeer != null)
			{
				result = owningCellPeer.GetSizeOfSet();
			}
			else
			{
				this.ThrowElementNotAvailableException();
			}
			return result;
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x0021D63C File Offset: 0x0021C63C
		internal override Rect GetVisibleBoundingRectCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetVisibleBoundingRectCore();
			}
			return base.GetBoundingRectangle();
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x0021D660 File Offset: 0x0021C660
		protected override bool HasKeyboardFocusCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.HasKeyboardFocus();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x0021D688 File Offset: 0x0021C688
		protected override bool IsContentElementCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			return owningCellPeer == null || owningCellPeer.IsContentElement();
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x0021D6A8 File Offset: 0x0021C6A8
		protected override bool IsControlElementCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			return owningCellPeer == null || owningCellPeer.IsControlElement();
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x0021D6C8 File Offset: 0x0021C6C8
		protected override bool IsEnabledCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsEnabled();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x0021D6F0 File Offset: 0x0021C6F0
		protected override bool IsKeyboardFocusableCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsKeyboardFocusable();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x0021D718 File Offset: 0x0021C718
		protected override bool IsOffscreenCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsOffscreen();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x0021D740 File Offset: 0x0021C740
		protected override bool IsPasswordCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsPassword();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x0021D768 File Offset: 0x0021C768
		protected override bool IsRequiredForFormCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsRequiredForForm();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x0021D790 File Offset: 0x0021C790
		protected override void SetFocusCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				owningCellPeer.SetFocus();
				return;
			}
			this.ThrowElementNotAvailableException();
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsDataItemAutomationPeer()
		{
			return true;
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x0021D7B4 File Offset: 0x0021C7B4
		internal override void AddToParentProxyWeakRefCache()
		{
			DataGridItemAutomationPeer owningItemPeer = this.OwningItemPeer;
			if (owningItemPeer != null)
			{
				owningItemPeer.AddProxyToWeakRefStorage(base.ElementProxyWeakReference, this);
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06004320 RID: 17184 RVA: 0x0021D7D8 File Offset: 0x0021C7D8
		int IGridItemProvider.Column
		{
			get
			{
				return this.OwningDataGrid.Columns.IndexOf(this._column);
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06004321 RID: 17185 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		int IGridItemProvider.ColumnSpan
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06004322 RID: 17186 RVA: 0x0021D7F0 File Offset: 0x0021C7F0
		IRawElementProviderSimple IGridItemProvider.ContainingGrid
		{
			get
			{
				return this.ContainingGrid;
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06004323 RID: 17187 RVA: 0x0021D7F8 File Offset: 0x0021C7F8
		int IGridItemProvider.Row
		{
			get
			{
				return this.OwningDataGrid.Items.IndexOf(this.Item);
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06004324 RID: 17188 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		int IGridItemProvider.RowSpan
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x0021D810 File Offset: 0x0021C810
		IRawElementProviderSimple[] ITableItemProvider.GetColumnHeaderItems()
		{
			if (this.OwningDataGrid != null && (this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Column) == DataGridHeadersVisibility.Column && this.OwningDataGrid.ColumnHeadersPresenter != null)
			{
				DataGridColumnHeadersPresenterAutomationPeer dataGridColumnHeadersPresenterAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid.ColumnHeadersPresenter) as DataGridColumnHeadersPresenterAutomationPeer;
				if (dataGridColumnHeadersPresenterAutomationPeer != null)
				{
					AutomationPeer automationPeer = dataGridColumnHeadersPresenterAutomationPeer.FindOrCreateItemAutomationPeer(this._column);
					if (automationPeer != null)
					{
						return new List<IRawElementProviderSimple>(1)
						{
							base.ProviderFromPeer(automationPeer)
						}.ToArray();
					}
				}
			}
			return null;
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x0021D888 File Offset: 0x0021C888
		IRawElementProviderSimple[] ITableItemProvider.GetRowHeaderItems()
		{
			if (this.OwningDataGrid != null && (this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Row) == DataGridHeadersVisibility.Row)
			{
				DataGridItemAutomationPeer dataGridItemAutomationPeer = (UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid) as DataGridAutomationPeer).FindOrCreateItemAutomationPeer(this.Item) as DataGridItemAutomationPeer;
				if (dataGridItemAutomationPeer != null)
				{
					AutomationPeer rowHeaderAutomationPeer = dataGridItemAutomationPeer.RowHeaderAutomationPeer;
					if (rowHeaderAutomationPeer != null)
					{
						return new List<IRawElementProviderSimple>(1)
						{
							base.ProviderFromPeer(rowHeaderAutomationPeer)
						}.ToArray();
					}
				}
			}
			return null;
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x0021D8F8 File Offset: 0x0021C8F8
		void IInvokeProvider.Invoke()
		{
			if (this.OwningDataGrid.IsReadOnly || this._column.IsReadOnly)
			{
				return;
			}
			this.EnsureEnabled();
			bool flag = false;
			if (this.OwningCell == null)
			{
				this.OwningDataGrid.ScrollIntoView(this.Item, this._column);
			}
			DataGridCell owningCell = this.OwningCell;
			if (owningCell != null)
			{
				if (!owningCell.IsEditing)
				{
					if (!owningCell.IsKeyboardFocusWithin)
					{
						owningCell.Focus();
					}
					this.OwningDataGrid.HandleSelectionForCellInput(owningCell, false, false, false);
					flag = this.OwningDataGrid.BeginEdit();
				}
				else
				{
					flag = true;
				}
			}
			if (!flag && !this.IsNewItemPlaceholder)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_AutomationInvokeFailed"));
			}
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x0021D9A2 File Offset: 0x0021C9A2
		void IScrollItemProvider.ScrollIntoView()
		{
			this.OwningDataGrid.ScrollIntoView(this.Item, this._column);
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06004329 RID: 17193 RVA: 0x0021D9BB File Offset: 0x0021C9BB
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return this.OwningDataGrid.SelectedCellsInternal.Contains(new DataGridCellInfo(this.Item, this._column));
			}
		}

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x0600432A RID: 17194 RVA: 0x0021D7F0 File Offset: 0x0021C7F0
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return this.ContainingGrid;
			}
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x0021D9E0 File Offset: 0x0021C9E0
		void ISelectionItemProvider.AddToSelection()
		{
			if (!this.IsCellSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			DataGridCellInfo cell = new DataGridCellInfo(this.Item, this._column);
			if (this.OwningDataGrid.SelectedCellsInternal.Contains(cell))
			{
				return;
			}
			this.EnsureEnabled();
			if (this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Single && this.OwningDataGrid.SelectedCells.Count > 0)
			{
				throw new InvalidOperationException();
			}
			this.OwningDataGrid.SelectedCellsInternal.Add(cell);
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x0021DA6C File Offset: 0x0021CA6C
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!this.IsCellSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			this.EnsureEnabled();
			DataGridCellInfo cell = new DataGridCellInfo(this.Item, this._column);
			if (this.OwningDataGrid.SelectedCellsInternal.Contains(cell))
			{
				this.OwningDataGrid.SelectedCellsInternal.Remove(cell);
			}
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x0021DAD0 File Offset: 0x0021CAD0
		void ISelectionItemProvider.Select()
		{
			if (!this.IsCellSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			this.EnsureEnabled();
			DataGridCellInfo currentCellInfo = new DataGridCellInfo(this.Item, this._column);
			this.OwningDataGrid.SelectOnlyThisCell(currentCellInfo);
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x0600432E RID: 17198 RVA: 0x0021DB1A File Offset: 0x0021CB1A
		bool IValueProvider.IsReadOnly
		{
			get
			{
				return this._column.IsReadOnly;
			}
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x0021DB27 File Offset: 0x0021CB27
		void IValueProvider.SetValue(string value)
		{
			if (this._column.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_ColumnIsReadOnly"));
			}
			if (this.OwningDataGrid != null)
			{
				this.OwningDataGrid.SetCellAutomationValue(this.Item, this._column, value);
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06004330 RID: 17200 RVA: 0x0021DB66 File Offset: 0x0021CB66
		string IValueProvider.Value
		{
			get
			{
				if (this.OwningDataGrid != null)
				{
					return this.OwningDataGrid.GetCellAutomationValue(this.Item, this._column);
				}
				return null;
			}
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x0021D9A2 File Offset: 0x0021C9A2
		void IVirtualizedItemProvider.Realize()
		{
			this.OwningDataGrid.ScrollIntoView(this.Item, this._column);
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x0021DB89 File Offset: 0x0021CB89
		private void EnsureEnabled()
		{
			if (!this.OwningDataGrid.IsEnabled)
			{
				throw new ElementNotEnabledException();
			}
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x0021DB9E File Offset: 0x0021CB9E
		private void ThrowElementNotAvailableException()
		{
			if (VirtualizedItemPatternIdentifiers.Pattern != null && !this.IsItemInAutomationTree())
			{
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x0021DBC0 File Offset: 0x0021CBC0
		private bool IsItemInAutomationTree()
		{
			AutomationPeer parent = base.GetParent();
			return base.Index != -1 && parent != null && parent.Children != null && base.Index < parent.Children.Count && parent.Children[base.Index] == this;
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06004335 RID: 17205 RVA: 0x0021DC12 File Offset: 0x0021CC12
		private bool IsCellSelectionUnit
		{
			get
			{
				return this.OwningDataGrid != null && (this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.Cell || this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader);
			}
		}

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06004336 RID: 17206 RVA: 0x0021DC3C File Offset: 0x0021CC3C
		private bool IsNewItemPlaceholder
		{
			get
			{
				object item = this.Item;
				return item == CollectionView.NewItemPlaceholder || item == DataGrid.NewItemPlaceholder;
			}
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06004337 RID: 17207 RVA: 0x0021DC62 File Offset: 0x0021CC62
		private DataGrid OwningDataGrid
		{
			get
			{
				return this._column.DataGridOwner;
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06004338 RID: 17208 RVA: 0x0021DC70 File Offset: 0x0021CC70
		private DataGridCell OwningCell
		{
			get
			{
				DataGrid owningDataGrid = this.OwningDataGrid;
				if (owningDataGrid == null)
				{
					return null;
				}
				return owningDataGrid.TryFindCell(this.Item, this._column);
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06004339 RID: 17209 RVA: 0x0021DC9C File Offset: 0x0021CC9C
		internal DataGridCellAutomationPeer OwningCellPeer
		{
			get
			{
				DataGridCellAutomationPeer dataGridCellAutomationPeer = null;
				DataGridCell owningCell = this.OwningCell;
				if (owningCell != null)
				{
					dataGridCellAutomationPeer = (UIElementAutomationPeer.CreatePeerForElement(owningCell) as DataGridCellAutomationPeer);
					dataGridCellAutomationPeer.EventsSource = this;
				}
				return dataGridCellAutomationPeer;
			}
		}

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x0600433A RID: 17210 RVA: 0x0021DCCC File Offset: 0x0021CCCC
		private IRawElementProviderSimple ContainingGrid
		{
			get
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid);
				if (automationPeer != null)
				{
					return base.ProviderFromPeer(automationPeer);
				}
				return null;
			}
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x0600433B RID: 17211 RVA: 0x0021DCF1 File Offset: 0x0021CCF1
		internal DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x0600433C RID: 17212 RVA: 0x0021DCF9 File Offset: 0x0021CCF9
		internal object Item
		{
			get
			{
				if (this._item != null)
				{
					return this._item.Target;
				}
				return null;
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x0600433D RID: 17213 RVA: 0x0021DD10 File Offset: 0x0021CD10
		private DataGridItemAutomationPeer OwningItemPeer
		{
			get
			{
				if (this.OwningDataGrid != null)
				{
					DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid) as DataGridAutomationPeer;
					if (dataGridAutomationPeer != null)
					{
						return dataGridAutomationPeer.GetExistingPeerByItem(this.Item, true) as DataGridItemAutomationPeer;
					}
				}
				return null;
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x0600433E RID: 17214 RVA: 0x0021DD4D File Offset: 0x0021CD4D
		// (set) Token: 0x0600433F RID: 17215 RVA: 0x0021DD58 File Offset: 0x0021CD58
		internal override bool AncestorsInvalid
		{
			get
			{
				return base.AncestorsInvalid;
			}
			set
			{
				base.AncestorsInvalid = value;
				if (value)
				{
					return;
				}
				AutomationPeer owningCellPeer = this.OwningCellPeer;
				if (owningCellPeer != null)
				{
					owningCellPeer.AncestorsInvalid = false;
				}
			}
		}

		// Token: 0x04002515 RID: 9493
		private WeakReference _item;

		// Token: 0x04002516 RID: 9494
		private DataGridColumn _column;
	}
}
