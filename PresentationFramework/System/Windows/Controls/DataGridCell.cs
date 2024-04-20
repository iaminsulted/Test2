using System;
using System.Collections.ObjectModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls
{
	// Token: 0x02000742 RID: 1858
	public class DataGridCell : ContentControl, IProvideDataGridColumn
	{
		// Token: 0x06006443 RID: 25667 RVA: 0x002A7DC8 File Offset: 0x002A6DC8
		static DataGridCell()
		{
			DataGridCell.SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataGridCell));
			DataGridCell.UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataGridCell));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(typeof(DataGridCell)));
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridCell.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridCell.OnCoerceStyle)));
			UIElement.ClipProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridCell.OnCoerceClip)));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));
			EventManager.RegisterClassHandler(typeof(DataGridCell), UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(DataGridCell.OnAnyMouseLeftButtonDownThunk), true);
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(DataGridCell), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			EventManager.RegisterClassHandler(typeof(DataGridCell), UIElement.LostFocusEvent, new RoutedEventHandler(DataGridCell.OnAnyLostFocus), true);
			EventManager.RegisterClassHandler(typeof(DataGridCell), UIElement.GotFocusEvent, new RoutedEventHandler(DataGridCell.OnAnyGotFocus), true);
		}

		// Token: 0x06006444 RID: 25668 RVA: 0x002A808B File Offset: 0x002A708B
		public DataGridCell()
		{
			this._tracker = new ContainerTracking<DataGridCell>(this);
		}

		// Token: 0x06006445 RID: 25669 RVA: 0x002A809F File Offset: 0x002A709F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridCellAutomationPeer(this);
		}

		// Token: 0x06006446 RID: 25670 RVA: 0x002A80A7 File Offset: 0x002A70A7
		internal void PrepareCell(object item, ItemsControl cellsPresenter, DataGridRow ownerRow)
		{
			this.PrepareCell(item, ownerRow, cellsPresenter.ItemContainerGenerator.IndexFromContainer(this));
		}

		// Token: 0x06006447 RID: 25671 RVA: 0x002A80C0 File Offset: 0x002A70C0
		internal void PrepareCell(object item, DataGridRow ownerRow, int index)
		{
			this._owner = ownerRow;
			DataGrid dataGridOwner = this._owner.DataGridOwner;
			if (dataGridOwner != null)
			{
				if (index >= 0 && index < dataGridOwner.Columns.Count)
				{
					DataGridColumn dataGridColumn = dataGridOwner.Columns[index];
					this.Column = dataGridColumn;
					base.TabIndex = dataGridColumn.DisplayIndex;
				}
				if (this.IsEditing)
				{
					this.IsEditing = false;
				}
				else if (!(base.Content is FrameworkElement))
				{
					this.BuildVisualTree();
					if (!this.NeedsVisualTree)
					{
						base.Content = item;
					}
				}
				bool isSelected = dataGridOwner.SelectedCellsInternal.Contains(this);
				this.SyncIsSelected(isSelected);
			}
			DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
			DataGridHelper.TransferProperty(this, DataGridCell.IsReadOnlyProperty);
			base.CoerceValue(UIElement.ClipProperty);
		}

		// Token: 0x06006448 RID: 25672 RVA: 0x002A817D File Offset: 0x002A717D
		internal void ClearCell(DataGridRow ownerRow)
		{
			this._owner = null;
		}

		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x06006449 RID: 25673 RVA: 0x002A8186 File Offset: 0x002A7186
		internal ContainerTracking<DataGridCell> Tracker
		{
			get
			{
				return this._tracker;
			}
		}

		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x0600644A RID: 25674 RVA: 0x002A818E File Offset: 0x002A718E
		// (set) Token: 0x0600644B RID: 25675 RVA: 0x002A81A0 File Offset: 0x002A71A0
		public DataGridColumn Column
		{
			get
			{
				return (DataGridColumn)base.GetValue(DataGridCell.ColumnProperty);
			}
			internal set
			{
				base.SetValue(DataGridCell.ColumnPropertyKey, value);
			}
		}

		// Token: 0x0600644C RID: 25676 RVA: 0x002A81B0 File Offset: 0x002A71B0
		private static void OnColumnChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			DataGridCell dataGridCell = sender as DataGridCell;
			if (dataGridCell != null)
			{
				dataGridCell.OnColumnChanged((DataGridColumn)e.OldValue, (DataGridColumn)e.NewValue);
			}
		}

		// Token: 0x0600644D RID: 25677 RVA: 0x002A81E5 File Offset: 0x002A71E5
		protected virtual void OnColumnChanged(DataGridColumn oldColumn, DataGridColumn newColumn)
		{
			base.Content = null;
			DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
			DataGridHelper.TransferProperty(this, DataGridCell.IsReadOnlyProperty);
		}

		// Token: 0x0600644E RID: 25678 RVA: 0x002A8204 File Offset: 0x002A7204
		private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridCell)d).NotifyPropertyChanged(d, string.Empty, e, DataGridNotificationTarget.Cells);
		}

		// Token: 0x0600644F RID: 25679 RVA: 0x002A821C File Offset: 0x002A721C
		private static void OnNotifyIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridCell dataGridCell = (DataGridCell)d;
			DataGrid dataGridOwner = dataGridCell.DataGridOwner;
			if ((bool)e.NewValue && dataGridOwner != null)
			{
				dataGridOwner.CancelEdit(dataGridCell);
			}
			CommandManager.InvalidateRequerySuggested();
			dataGridCell.NotifyPropertyChanged(d, string.Empty, e, DataGridNotificationTarget.Cells);
		}

		// Token: 0x06006450 RID: 25680 RVA: 0x002A8264 File Offset: 0x002A7264
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			if (dataGridColumn != null && dataGridColumn != this.Column)
			{
				return;
			}
			if (DataGridHelper.ShouldNotifyCells(target))
			{
				if (e.Property == DataGridColumn.WidthProperty)
				{
					DataGridHelper.OnColumnWidthChanged(this, e);
				}
				else if (e.Property == DataGrid.CellStyleProperty || e.Property == DataGridColumn.CellStyleProperty || e.Property == FrameworkElement.StyleProperty)
				{
					DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
				}
				else if (e.Property == DataGrid.IsReadOnlyProperty || e.Property == DataGridColumn.IsReadOnlyProperty || e.Property == DataGridCell.IsReadOnlyProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridCell.IsReadOnlyProperty);
				}
				else if (e.Property == DataGridColumn.DisplayIndexProperty)
				{
					base.TabIndex = dataGridColumn.DisplayIndex;
				}
				else if (e.Property == UIElement.IsKeyboardFocusWithinProperty)
				{
					base.UpdateVisualState();
				}
			}
			if (DataGridHelper.ShouldRefreshCellContent(target) && dataGridColumn != null && this.NeedsVisualTree)
			{
				if (!string.IsNullOrEmpty(propertyName))
				{
					dataGridColumn.RefreshCellContent(this, propertyName);
					return;
				}
				if (e.Property != null)
				{
					dataGridColumn.RefreshCellContent(this, e.Property.Name);
				}
			}
		}

		// Token: 0x06006451 RID: 25681 RVA: 0x002A838C File Offset: 0x002A738C
		private static object OnCoerceStyle(DependencyObject d, object baseValue)
		{
			DataGridCell dataGridCell = d as DataGridCell;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridCell, baseValue, FrameworkElement.StyleProperty, dataGridCell.Column, DataGridColumn.CellStyleProperty, dataGridCell.DataGridOwner, DataGrid.CellStyleProperty);
		}

		// Token: 0x06006452 RID: 25682 RVA: 0x002A83C4 File Offset: 0x002A73C4
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (this.DataGridOwner == null)
			{
				return;
			}
			if (base.IsMouseOver)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (this.IsSelected)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Selected",
					"Unselected"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Unselected"
				});
			}
			if (this.DataGridOwner.IsKeyboardFocusWithin)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			if (this.IsCurrent)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Current",
					"Regular"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Regular", useTransitions);
			}
			if (this.IsEditing)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Editing",
					"Display"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Display", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x06006453 RID: 25683 RVA: 0x002A84F4 File Offset: 0x002A74F4
		internal void BuildVisualTree()
		{
			if (this.NeedsVisualTree)
			{
				DataGridColumn column = this.Column;
				if (column != null)
				{
					DataGridRow rowOwner = this.RowOwner;
					if (rowOwner != null)
					{
						BindingGroup bindingGroup = rowOwner.BindingGroup;
						if (bindingGroup != null)
						{
							this.RemoveBindingExpressions(bindingGroup, base.Content as DependencyObject);
						}
					}
					FrameworkElement frameworkElement = column.BuildVisualTree(this.IsEditing, this.RowDataItem, this);
					FrameworkElement frameworkElement2 = base.Content as FrameworkElement;
					if (frameworkElement2 != null && frameworkElement2 != frameworkElement)
					{
						ContentPresenter contentPresenter = frameworkElement2 as ContentPresenter;
						if (contentPresenter == null)
						{
							frameworkElement2.SetValue(FrameworkElement.DataContextProperty, BindingExpressionBase.DisconnectedItem);
						}
						else
						{
							contentPresenter.Content = BindingExpressionBase.DisconnectedItem;
						}
					}
					base.Content = frameworkElement;
				}
			}
		}

		// Token: 0x06006454 RID: 25684 RVA: 0x002A859C File Offset: 0x002A759C
		private void RemoveBindingExpressions(BindingGroup bindingGroup, DependencyObject element)
		{
			if (element == null)
			{
				return;
			}
			Collection<BindingExpressionBase> bindingExpressions = bindingGroup.BindingExpressions;
			BindingExpressionBase[] array = new BindingExpressionBase[bindingExpressions.Count];
			bindingExpressions.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (DataGridHelper.BindingExpressionBelongsToElement<DataGridCell>(array[i], this))
				{
					bindingExpressions.Remove(array[i]);
				}
			}
		}

		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x06006455 RID: 25685 RVA: 0x002A85EB File Offset: 0x002A75EB
		// (set) Token: 0x06006456 RID: 25686 RVA: 0x002A85FD File Offset: 0x002A75FD
		public bool IsEditing
		{
			get
			{
				return (bool)base.GetValue(DataGridCell.IsEditingProperty);
			}
			set
			{
				base.SetValue(DataGridCell.IsEditingProperty, value);
			}
		}

		// Token: 0x06006457 RID: 25687 RVA: 0x002A860B File Offset: 0x002A760B
		private static void OnIsEditingChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			((DataGridCell)sender).OnIsEditingChanged((bool)e.NewValue);
		}

		// Token: 0x06006458 RID: 25688 RVA: 0x002A8624 File Offset: 0x002A7624
		protected virtual void OnIsEditingChanged(bool isEditing)
		{
			if (base.IsKeyboardFocusWithin && !base.IsKeyboardFocused)
			{
				base.Focus();
			}
			this.BuildVisualTree();
			base.UpdateVisualState();
		}

		// Token: 0x06006459 RID: 25689 RVA: 0x002A8649 File Offset: 0x002A7649
		internal void NotifyCurrentCellContainerChanged()
		{
			base.UpdateVisualState();
		}

		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x0600645A RID: 25690 RVA: 0x002A8654 File Offset: 0x002A7654
		private bool IsCurrent
		{
			get
			{
				DataGridRow rowOwner = this.RowOwner;
				DataGridColumn column = this.Column;
				if (rowOwner != null && column != null)
				{
					DataGrid dataGridOwner = rowOwner.DataGridOwner;
					if (dataGridOwner != null)
					{
						return dataGridOwner.IsCurrent(rowOwner, column);
					}
				}
				return false;
			}
		}

		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x0600645B RID: 25691 RVA: 0x002A8689 File Offset: 0x002A7689
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(DataGridCell.IsReadOnlyProperty);
			}
		}

		// Token: 0x0600645C RID: 25692 RVA: 0x002A869C File Offset: 0x002A769C
		private static object OnCoerceIsReadOnly(DependencyObject d, object baseValue)
		{
			DataGridCell dataGridCell = d as DataGridCell;
			DataGridColumn column = dataGridCell.Column;
			DataGrid dataGridOwner = dataGridCell.DataGridOwner;
			return DataGridHelper.GetCoercedTransferPropertyValue(column, column.IsReadOnly, DataGridColumn.IsReadOnlyProperty, dataGridOwner, DataGrid.IsReadOnlyProperty);
		}

		// Token: 0x0600645D RID: 25693 RVA: 0x002A86D8 File Offset: 0x002A76D8
		private static void OnAnyLostFocus(object sender, RoutedEventArgs e)
		{
			DataGridCell dataGridCell = DataGridHelper.FindVisualParent<DataGridCell>(e.OriginalSource as UIElement);
			if (dataGridCell != null && dataGridCell == sender)
			{
				DataGrid dataGridOwner = dataGridCell.DataGridOwner;
				if (dataGridOwner != null && !dataGridCell.IsKeyboardFocusWithin && dataGridOwner.FocusedCell == dataGridCell)
				{
					dataGridOwner.FocusedCell = null;
				}
			}
		}

		// Token: 0x0600645E RID: 25694 RVA: 0x002A8720 File Offset: 0x002A7720
		private static void OnAnyGotFocus(object sender, RoutedEventArgs e)
		{
			DataGridCell dataGridCell = DataGridHelper.FindVisualParent<DataGridCell>(e.OriginalSource as UIElement);
			if (dataGridCell != null && dataGridCell == sender)
			{
				DataGrid dataGridOwner = dataGridCell.DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridOwner.FocusedCell = dataGridCell;
				}
			}
		}

		// Token: 0x0600645F RID: 25695 RVA: 0x002A8758 File Offset: 0x002A7758
		internal void BeginEdit(RoutedEventArgs e)
		{
			this.IsEditing = true;
			DataGridColumn column = this.Column;
			if (column != null)
			{
				column.BeginEdit(base.Content as FrameworkElement, e);
			}
			this.RaisePreparingCellForEdit(e);
		}

		// Token: 0x06006460 RID: 25696 RVA: 0x002A8790 File Offset: 0x002A7790
		internal void CancelEdit()
		{
			DataGridColumn column = this.Column;
			if (column != null)
			{
				column.CancelEdit(base.Content as FrameworkElement);
			}
			this.IsEditing = false;
		}

		// Token: 0x06006461 RID: 25697 RVA: 0x002A87C0 File Offset: 0x002A77C0
		internal bool CommitEdit()
		{
			bool flag = true;
			DataGridColumn column = this.Column;
			if (column != null)
			{
				flag = column.CommitEdit(base.Content as FrameworkElement);
			}
			if (flag)
			{
				this.IsEditing = false;
			}
			return flag;
		}

		// Token: 0x06006462 RID: 25698 RVA: 0x002A87F8 File Offset: 0x002A77F8
		private void RaisePreparingCellForEdit(RoutedEventArgs editingEventArgs)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner != null)
			{
				FrameworkElement editingElement = this.EditingElement;
				DataGridPreparingCellForEditEventArgs e = new DataGridPreparingCellForEditEventArgs(this.Column, this.RowOwner, editingEventArgs, editingElement);
				dataGridOwner.OnPreparingCellForEdit(e);
			}
		}

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x06006463 RID: 25699 RVA: 0x002A8831 File Offset: 0x002A7831
		internal FrameworkElement EditingElement
		{
			get
			{
				return base.Content as FrameworkElement;
			}
		}

		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x06006464 RID: 25700 RVA: 0x002A883E File Offset: 0x002A783E
		// (set) Token: 0x06006465 RID: 25701 RVA: 0x002A8850 File Offset: 0x002A7850
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(DataGridCell.IsSelectedProperty);
			}
			set
			{
				base.SetValue(DataGridCell.IsSelectedProperty, value);
			}
		}

		// Token: 0x06006466 RID: 25702 RVA: 0x002A8860 File Offset: 0x002A7860
		private static void OnIsSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			DataGridCell dataGridCell = (DataGridCell)sender;
			bool isSelected = (bool)e.NewValue;
			if (!dataGridCell._syncingIsSelected)
			{
				DataGrid dataGridOwner = dataGridCell.DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridOwner.CellIsSelectedChanged(dataGridCell, isSelected);
				}
			}
			dataGridCell.RaiseSelectionChangedEvent(isSelected);
			dataGridCell.UpdateVisualState();
		}

		// Token: 0x06006467 RID: 25703 RVA: 0x002A88A8 File Offset: 0x002A78A8
		internal void SyncIsSelected(bool isSelected)
		{
			bool syncingIsSelected = this._syncingIsSelected;
			this._syncingIsSelected = true;
			try
			{
				this.IsSelected = isSelected;
			}
			finally
			{
				this._syncingIsSelected = syncingIsSelected;
			}
		}

		// Token: 0x06006468 RID: 25704 RVA: 0x002A88E4 File Offset: 0x002A78E4
		private void RaiseSelectionChangedEvent(bool isSelected)
		{
			if (isSelected)
			{
				this.OnSelected(new RoutedEventArgs(DataGridCell.SelectedEvent, this));
				return;
			}
			this.OnUnselected(new RoutedEventArgs(DataGridCell.UnselectedEvent, this));
		}

		// Token: 0x14000103 RID: 259
		// (add) Token: 0x06006469 RID: 25705 RVA: 0x002A890C File Offset: 0x002A790C
		// (remove) Token: 0x0600646A RID: 25706 RVA: 0x002A891A File Offset: 0x002A791A
		public event RoutedEventHandler Selected
		{
			add
			{
				base.AddHandler(DataGridCell.SelectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DataGridCell.SelectedEvent, value);
			}
		}

		// Token: 0x0600646B RID: 25707 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x14000104 RID: 260
		// (add) Token: 0x0600646C RID: 25708 RVA: 0x002A8928 File Offset: 0x002A7928
		// (remove) Token: 0x0600646D RID: 25709 RVA: 0x002A8936 File Offset: 0x002A7936
		public event RoutedEventHandler Unselected
		{
			add
			{
				base.AddHandler(DataGridCell.UnselectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DataGridCell.UnselectedEvent, value);
			}
		}

		// Token: 0x0600646E RID: 25710 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x0600646F RID: 25711 RVA: 0x002A8944 File Offset: 0x002A7944
		protected override Size MeasureOverride(Size constraint)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			bool flag = DataGridHelper.IsGridLineVisible(dataGridOwner, true);
			bool flag2 = DataGridHelper.IsGridLineVisible(dataGridOwner, false);
			double num = 0.0;
			double num2 = 0.0;
			if (flag)
			{
				num = dataGridOwner.HorizontalGridLineThickness;
				constraint = DataGridHelper.SubtractFromSize(constraint, num, true);
			}
			if (flag2)
			{
				num2 = dataGridOwner.VerticalGridLineThickness;
				constraint = DataGridHelper.SubtractFromSize(constraint, num2, false);
			}
			Size result = base.MeasureOverride(constraint);
			if (flag)
			{
				result.Height += num;
			}
			if (flag2)
			{
				result.Width += num2;
			}
			return result;
		}

		// Token: 0x06006470 RID: 25712 RVA: 0x002A89D4 File Offset: 0x002A79D4
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			bool flag = DataGridHelper.IsGridLineVisible(dataGridOwner, true);
			bool flag2 = DataGridHelper.IsGridLineVisible(dataGridOwner, false);
			double num = 0.0;
			double num2 = 0.0;
			if (flag)
			{
				num = dataGridOwner.HorizontalGridLineThickness;
				arrangeSize = DataGridHelper.SubtractFromSize(arrangeSize, num, true);
			}
			if (flag2)
			{
				num2 = dataGridOwner.VerticalGridLineThickness;
				arrangeSize = DataGridHelper.SubtractFromSize(arrangeSize, num2, false);
			}
			Size result = base.ArrangeOverride(arrangeSize);
			if (flag)
			{
				result.Height += num;
			}
			if (flag2)
			{
				result.Width += num2;
			}
			return result;
		}

		// Token: 0x06006471 RID: 25713 RVA: 0x002A8A64 File Offset: 0x002A7A64
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			DataGrid dataGridOwner = this.DataGridOwner;
			if (DataGridHelper.IsGridLineVisible(dataGridOwner, false))
			{
				double verticalGridLineThickness = this.DataGridOwner.VerticalGridLineThickness;
				Rect rectangle = new Rect(new Size(verticalGridLineThickness, base.RenderSize.Height));
				rectangle.X = base.RenderSize.Width - verticalGridLineThickness;
				drawingContext.DrawRectangle(this.DataGridOwner.VerticalGridLinesBrush, null, rectangle);
			}
			if (DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Rect rectangle2 = new Rect(new Size(base.RenderSize.Width, horizontalGridLineThickness));
				rectangle2.Y = base.RenderSize.Height - horizontalGridLineThickness;
				drawingContext.DrawRectangle(dataGridOwner.HorizontalGridLinesBrush, null, rectangle2);
			}
		}

		// Token: 0x06006472 RID: 25714 RVA: 0x002A8B2D File Offset: 0x002A7B2D
		private static void OnAnyMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs e)
		{
			((DataGridCell)sender).OnAnyMouseLeftButtonDown(e);
		}

		// Token: 0x06006473 RID: 25715 RVA: 0x002A8B3C File Offset: 0x002A7B3C
		private void OnAnyMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			bool isKeyboardFocusWithin = base.IsKeyboardFocusWithin;
			bool flag = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			if (isKeyboardFocusWithin && !flag && !e.Handled && this.IsSelected)
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridOwner.HandleSelectionForCellInput(this, false, true, false);
					if (!this.IsEditing && !this.IsReadOnly)
					{
						dataGridOwner.BeginEdit(e);
						e.Handled = true;
						return;
					}
				}
			}
			else if (!isKeyboardFocusWithin || !this.IsSelected || flag)
			{
				if (!isKeyboardFocusWithin)
				{
					base.Focus();
				}
				DataGrid dataGridOwner2 = this.DataGridOwner;
				if (dataGridOwner2 != null)
				{
					dataGridOwner2.HandleSelectionForCellInput(this, Mouse.Captured == null, true, true);
				}
				e.Handled = true;
			}
		}

		// Token: 0x06006474 RID: 25716 RVA: 0x002A8BE4 File Offset: 0x002A7BE4
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			this.SendInputToColumn(e);
		}

		// Token: 0x06006475 RID: 25717 RVA: 0x002A8BE4 File Offset: 0x002A7BE4
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			this.SendInputToColumn(e);
		}

		// Token: 0x06006476 RID: 25718 RVA: 0x002A8BE4 File Offset: 0x002A7BE4
		protected override void OnKeyDown(KeyEventArgs e)
		{
			this.SendInputToColumn(e);
		}

		// Token: 0x06006477 RID: 25719 RVA: 0x002A8BF0 File Offset: 0x002A7BF0
		private void SendInputToColumn(InputEventArgs e)
		{
			DataGridColumn column = this.Column;
			if (column != null)
			{
				column.OnInput(e);
			}
		}

		// Token: 0x06006478 RID: 25720 RVA: 0x002A8C10 File Offset: 0x002A7C10
		private static object OnCoerceClip(DependencyObject d, object baseValue)
		{
			IProvideDataGridColumn cell = (DataGridCell)d;
			Geometry geometry = baseValue as Geometry;
			Geometry frozenClipForCell = DataGridHelper.GetFrozenClipForCell(cell);
			if (frozenClipForCell != null)
			{
				if (geometry == null)
				{
					return frozenClipForCell;
				}
				geometry = new CombinedGeometry(GeometryCombineMode.Intersect, geometry, frozenClipForCell);
			}
			return geometry;
		}

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x06006479 RID: 25721 RVA: 0x002A8C44 File Offset: 0x002A7C44
		internal DataGrid DataGridOwner
		{
			get
			{
				if (this._owner != null)
				{
					DataGrid dataGrid = this._owner.DataGridOwner;
					if (dataGrid == null)
					{
						dataGrid = (ItemsControl.ItemsControlFromItemContainer(this._owner) as DataGrid);
					}
					return dataGrid;
				}
				return null;
			}
		}

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x0600647A RID: 25722 RVA: 0x002A8C7C File Offset: 0x002A7C7C
		private Panel ParentPanel
		{
			get
			{
				return base.VisualParent as Panel;
			}
		}

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x0600647B RID: 25723 RVA: 0x002A8C89 File Offset: 0x002A7C89
		internal DataGridRow RowOwner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x0600647C RID: 25724 RVA: 0x002A8C94 File Offset: 0x002A7C94
		internal object RowDataItem
		{
			get
			{
				DataGridRow rowOwner = this.RowOwner;
				if (rowOwner != null)
				{
					return rowOwner.Item;
				}
				return base.DataContext;
			}
		}

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x0600647D RID: 25725 RVA: 0x002A8CB8 File Offset: 0x002A7CB8
		private DataGridCellsPresenter CellsPresenter
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as DataGridCellsPresenter;
			}
		}

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x0600647E RID: 25726 RVA: 0x002A8CC5 File Offset: 0x002A7CC5
		private bool NeedsVisualTree
		{
			get
			{
				return base.ContentTemplate == null && base.ContentTemplateSelector == null;
			}
		}

		// Token: 0x0400333E RID: 13118
		private static readonly DependencyPropertyKey ColumnPropertyKey = DependencyProperty.RegisterReadOnly("Column", typeof(DataGridColumn), typeof(DataGridCell), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridCell.OnColumnChanged)));

		// Token: 0x0400333F RID: 13119
		public static readonly DependencyProperty ColumnProperty = DataGridCell.ColumnPropertyKey.DependencyProperty;

		// Token: 0x04003340 RID: 13120
		public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(DataGridCell), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridCell.OnIsEditingChanged)));

		// Token: 0x04003341 RID: 13121
		private static readonly DependencyPropertyKey IsReadOnlyPropertyKey = DependencyProperty.RegisterReadOnly("IsReadOnly", typeof(bool), typeof(DataGridCell), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridCell.OnNotifyIsReadOnlyChanged), new CoerceValueCallback(DataGridCell.OnCoerceIsReadOnly)));

		// Token: 0x04003342 RID: 13122
		public static readonly DependencyProperty IsReadOnlyProperty = DataGridCell.IsReadOnlyPropertyKey.DependencyProperty;

		// Token: 0x04003343 RID: 13123
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(DataGridCell), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridCell.OnIsSelectedChanged)));

		// Token: 0x04003346 RID: 13126
		private DataGridRow _owner;

		// Token: 0x04003347 RID: 13127
		private ContainerTracking<DataGridCell> _tracker;

		// Token: 0x04003348 RID: 13128
		private bool _syncingIsSelected;
	}
}
