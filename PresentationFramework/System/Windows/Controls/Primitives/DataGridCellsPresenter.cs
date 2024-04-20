using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000829 RID: 2089
	public class DataGridCellsPresenter : ItemsControl
	{
		// Token: 0x060079FD RID: 31229 RVA: 0x00305E70 File Offset: 0x00304E70
		static DataGridCellsPresenter()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(typeof(DataGridCellsPresenter)));
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(new ItemsPanelTemplate(new FrameworkElementFactory(typeof(DataGridCellsPanel)))));
			UIElement.FocusableProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(false));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridCellsPresenter.OnNotifyHeightPropertyChanged), new CoerceValueCallback(DataGridCellsPresenter.OnCoerceHeight)));
			FrameworkElement.MinHeightProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridCellsPresenter.OnNotifyHeightPropertyChanged), new CoerceValueCallback(DataGridCellsPresenter.OnCoerceMinHeight)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridCellsPresenter.OnIsVirtualizingPropertyChanged), new CoerceValueCallback(DataGridCellsPresenter.OnCoerceIsVirtualizingProperty)));
			VirtualizingPanel.VirtualizationModeProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(VirtualizationMode.Recycling));
		}

		// Token: 0x060079FF RID: 31231 RVA: 0x00305FA4 File Offset: 0x00304FA4
		public override void OnApplyTemplate()
		{
			if (this.InternalItemsHost != null && !base.IsAncestorOf(this.InternalItemsHost))
			{
				this.InternalItemsHost = null;
			}
			base.OnApplyTemplate();
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner != null)
			{
				dataGridRowOwner.CellsPresenter = this;
				this.Item = dataGridRowOwner.Item;
			}
			this.SyncProperties(false);
		}

		// Token: 0x06007A00 RID: 31232 RVA: 0x00305FF8 File Offset: 0x00304FF8
		internal void SyncProperties(bool forcePrepareCells)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner == null)
			{
				return;
			}
			DataGridHelper.TransferProperty(this, FrameworkElement.HeightProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.MinHeightProperty);
			DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);
			this.NotifyPropertyChanged(this, new DependencyPropertyChangedEventArgs(DataGrid.CellStyleProperty, null, null), DataGridNotificationTarget.Cells);
			MultipleCopiesCollection multipleCopiesCollection = base.ItemsSource as MultipleCopiesCollection;
			if (multipleCopiesCollection != null)
			{
				ObservableCollection<DataGridColumn> columns = dataGridOwner.Columns;
				int count = columns.Count;
				int count2 = multipleCopiesCollection.Count;
				int num = 0;
				bool flag = false;
				if (count != count2)
				{
					multipleCopiesCollection.SyncToCount(count);
					num = Math.Min(count, count2);
				}
				else if (forcePrepareCells)
				{
					num = count;
				}
				DataGridCellsPanel dataGridCellsPanel = this.InternalItemsHost as DataGridCellsPanel;
				if (dataGridCellsPanel != null)
				{
					if (dataGridCellsPanel.HasCorrectRealizedColumns)
					{
						dataGridCellsPanel.InvalidateArrange();
					}
					else
					{
						this.InvalidateDataGridCellsPanelMeasureAndArrange();
						flag = true;
					}
				}
				DataGridRow dataGridRowOwner = this.DataGridRowOwner;
				for (int i = 0; i < num; i++)
				{
					DataGridCell dataGridCell = (DataGridCell)base.ItemContainerGenerator.ContainerFromIndex(i);
					if (dataGridCell != null)
					{
						dataGridCell.PrepareCell(dataGridRowOwner.Item, this, dataGridRowOwner);
						if (!flag && !DoubleUtil.AreClose(dataGridCell.ActualWidth, columns[i].Width.DisplayValue))
						{
							this.InvalidateDataGridCellsPanelMeasureAndArrange();
							flag = true;
						}
					}
				}
				if (!flag)
				{
					for (int j = num; j < count; j++)
					{
						DataGridCell dataGridCell = (DataGridCell)base.ItemContainerGenerator.ContainerFromIndex(j);
						if (dataGridCell != null && !DoubleUtil.AreClose(dataGridCell.ActualWidth, columns[j].Width.DisplayValue))
						{
							this.InvalidateDataGridCellsPanelMeasureAndArrange();
							return;
						}
					}
				}
			}
		}

		// Token: 0x06007A01 RID: 31233 RVA: 0x00306188 File Offset: 0x00305188
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			DataGridCellsPresenter dataGridCellsPresenter = d as DataGridCellsPresenter;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridCellsPresenter, baseValue, FrameworkElement.HeightProperty, dataGridCellsPresenter.DataGridOwner, DataGrid.RowHeightProperty);
		}

		// Token: 0x06007A02 RID: 31234 RVA: 0x003061B4 File Offset: 0x003051B4
		private static object OnCoerceMinHeight(DependencyObject d, object baseValue)
		{
			DataGridCellsPresenter dataGridCellsPresenter = d as DataGridCellsPresenter;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridCellsPresenter, baseValue, FrameworkElement.MinHeightProperty, dataGridCellsPresenter.DataGridOwner, DataGrid.MinRowHeightProperty);
		}

		// Token: 0x17001C3F RID: 7231
		// (get) Token: 0x06007A03 RID: 31235 RVA: 0x003061DF File Offset: 0x003051DF
		// (set) Token: 0x06007A04 RID: 31236 RVA: 0x003061E8 File Offset: 0x003051E8
		public object Item
		{
			get
			{
				return this._item;
			}
			internal set
			{
				if (this._item != value)
				{
					object item = this._item;
					this._item = value;
					this.OnItemChanged(item, this._item);
				}
			}
		}

		// Token: 0x06007A05 RID: 31237 RVA: 0x0030621C File Offset: 0x0030521C
		protected virtual void OnItemChanged(object oldItem, object newItem)
		{
			ObservableCollection<DataGridColumn> columns = this.Columns;
			if (columns != null)
			{
				MultipleCopiesCollection multipleCopiesCollection = base.ItemsSource as MultipleCopiesCollection;
				if (multipleCopiesCollection == null)
				{
					multipleCopiesCollection = new MultipleCopiesCollection(newItem, columns.Count);
					base.ItemsSource = multipleCopiesCollection;
					return;
				}
				multipleCopiesCollection.CopiedItem = newItem;
			}
		}

		// Token: 0x06007A06 RID: 31238 RVA: 0x0030625E File Offset: 0x0030525E
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is DataGridCell;
		}

		// Token: 0x06007A07 RID: 31239 RVA: 0x002CF5E9 File Offset: 0x002CE5E9
		internal bool IsItemItsOwnContainerInternal(object item)
		{
			return this.IsItemItsOwnContainerOverride(item);
		}

		// Token: 0x06007A08 RID: 31240 RVA: 0x00306269 File Offset: 0x00305269
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new DataGridCell();
		}

		// Token: 0x06007A09 RID: 31241 RVA: 0x00306270 File Offset: 0x00305270
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			DataGridCell dataGridCell = (DataGridCell)element;
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridCell.RowOwner != dataGridRowOwner)
			{
				dataGridCell.Tracker.StartTracking(ref this._cellTrackingRoot);
			}
			dataGridCell.PrepareCell(item, this, dataGridRowOwner);
		}

		// Token: 0x06007A0A RID: 31242 RVA: 0x003062B0 File Offset: 0x003052B0
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			DataGridCell dataGridCell = (DataGridCell)element;
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridCell.RowOwner == dataGridRowOwner)
			{
				dataGridCell.Tracker.StopTracking(ref this._cellTrackingRoot);
			}
			dataGridCell.ClearCell(dataGridRowOwner);
		}

		// Token: 0x06007A0B RID: 31243 RVA: 0x003062EC File Offset: 0x003052EC
		protected internal virtual void OnColumnsChanged(ObservableCollection<DataGridColumn> columns, NotifyCollectionChangedEventArgs e)
		{
			MultipleCopiesCollection multipleCopiesCollection = base.ItemsSource as MultipleCopiesCollection;
			if (multipleCopiesCollection != null)
			{
				multipleCopiesCollection.MirrorCollectionChange(e);
			}
		}

		// Token: 0x06007A0C RID: 31244 RVA: 0x0030630F File Offset: 0x0030530F
		private static void OnNotifyHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridCellsPresenter)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter);
		}

		// Token: 0x06007A0D RID: 31245 RVA: 0x0030631F File Offset: 0x0030531F
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			this.NotifyPropertyChanged(d, string.Empty, e, target);
		}

		// Token: 0x06007A0E RID: 31246 RVA: 0x00306330 File Offset: 0x00305330
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyCellsPresenter(target))
			{
				if (e.Property == DataGridColumn.WidthProperty || e.Property == DataGridColumn.DisplayIndexProperty)
				{
					if (((DataGridColumn)d).IsVisible)
					{
						this.InvalidateDataGridCellsPanelMeasureAndArrangeImpl(e.Property == DataGridColumn.WidthProperty);
					}
				}
				else if (e.Property == DataGrid.FrozenColumnCountProperty || e.Property == DataGridColumn.VisibilityProperty || e.Property == DataGrid.CellsPanelHorizontalOffsetProperty || e.Property == DataGrid.HorizontalScrollOffsetProperty || string.Compare(propertyName, "ViewportWidth", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "DelayedColumnWidthComputation", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange();
				}
				else if (string.Compare(propertyName, "RealizedColumnsBlockListForNonVirtualizedRows", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange(false);
				}
				else if (string.Compare(propertyName, "RealizedColumnsBlockListForVirtualizedRows", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange(true);
				}
				else if (e.Property == DataGrid.RowHeightProperty || e.Property == FrameworkElement.HeightProperty)
				{
					DataGridHelper.TransferProperty(this, FrameworkElement.HeightProperty);
				}
				else if (e.Property == DataGrid.MinRowHeightProperty || e.Property == FrameworkElement.MinHeightProperty)
				{
					DataGridHelper.TransferProperty(this, FrameworkElement.MinHeightProperty);
				}
				else if (e.Property == DataGrid.EnableColumnVirtualizationProperty)
				{
					DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);
				}
			}
			if (DataGridHelper.ShouldNotifyCells(target) || DataGridHelper.ShouldRefreshCellContent(target))
			{
				for (ContainerTracking<DataGridCell> containerTracking = this._cellTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
				{
					containerTracking.Container.NotifyPropertyChanged(d, propertyName, e, target);
				}
			}
		}

		// Token: 0x06007A0F RID: 31247 RVA: 0x003064BE File Offset: 0x003054BE
		protected override Size MeasureOverride(Size availableSize)
		{
			return base.MeasureOverride(availableSize);
		}

		// Token: 0x06007A10 RID: 31248 RVA: 0x003064C7 File Offset: 0x003054C7
		protected override Size ArrangeOverride(Size finalSize)
		{
			return base.ArrangeOverride(finalSize);
		}

		// Token: 0x06007A11 RID: 31249 RVA: 0x003064D0 File Offset: 0x003054D0
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner == null)
			{
				return;
			}
			DataGrid dataGridOwner = dataGridRowOwner.DataGridOwner;
			if (dataGridOwner == null)
			{
				return;
			}
			if (DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Rect rectangle = new Rect(new Size(base.RenderSize.Width, horizontalGridLineThickness));
				rectangle.Y = base.RenderSize.Height - horizontalGridLineThickness;
				drawingContext.DrawRectangle(dataGridOwner.HorizontalGridLinesBrush, null, rectangle);
			}
		}

		// Token: 0x06007A12 RID: 31250 RVA: 0x0030654C File Offset: 0x0030554C
		private static void OnIsVirtualizingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridCellsPresenter dataGridCellsPresenter = (DataGridCellsPresenter)d;
			DataGridHelper.TransferProperty(dataGridCellsPresenter, VirtualizingPanel.IsVirtualizingProperty);
			if (e.OldValue != dataGridCellsPresenter.GetValue(VirtualizingPanel.IsVirtualizingProperty))
			{
				dataGridCellsPresenter.InvalidateDataGridCellsPanelMeasureAndArrange();
			}
		}

		// Token: 0x06007A13 RID: 31251 RVA: 0x00306588 File Offset: 0x00305588
		private static object OnCoerceIsVirtualizingProperty(DependencyObject d, object baseValue)
		{
			DataGridCellsPresenter dataGridCellsPresenter = d as DataGridCellsPresenter;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridCellsPresenter, baseValue, VirtualizingPanel.IsVirtualizingProperty, dataGridCellsPresenter.DataGridOwner, DataGrid.EnableColumnVirtualizationProperty);
		}

		// Token: 0x06007A14 RID: 31252 RVA: 0x003065B3 File Offset: 0x003055B3
		internal void InvalidateDataGridCellsPanelMeasureAndArrange()
		{
			this.InvalidateDataGridCellsPanelMeasureAndArrangeImpl(false);
		}

		// Token: 0x06007A15 RID: 31253 RVA: 0x003065BC File Offset: 0x003055BC
		private void InvalidateDataGridCellsPanelMeasureAndArrangeImpl(bool invalidateMeasureUptoRowsPresenter)
		{
			if (this._internalItemsHost != null)
			{
				this._internalItemsHost.InvalidateMeasure();
				this._internalItemsHost.InvalidateArrange();
				if (invalidateMeasureUptoRowsPresenter)
				{
					DataGrid dataGridOwner = this.DataGridOwner;
					if (dataGridOwner != null && dataGridOwner.InternalItemsHost != null)
					{
						Helper.InvalidateMeasureOnPath(this._internalItemsHost, dataGridOwner.InternalItemsHost, false, true);
					}
				}
			}
		}

		// Token: 0x06007A16 RID: 31254 RVA: 0x0030660F File Offset: 0x0030560F
		private void InvalidateDataGridCellsPanelMeasureAndArrange(bool withColumnVirtualization)
		{
			if (withColumnVirtualization == VirtualizingPanel.GetIsVirtualizing(this))
			{
				this.InvalidateDataGridCellsPanelMeasureAndArrange();
			}
		}

		// Token: 0x17001C40 RID: 7232
		// (get) Token: 0x06007A17 RID: 31255 RVA: 0x00306620 File Offset: 0x00305620
		// (set) Token: 0x06007A18 RID: 31256 RVA: 0x00306628 File Offset: 0x00305628
		internal Panel InternalItemsHost
		{
			get
			{
				return this._internalItemsHost;
			}
			set
			{
				this._internalItemsHost = value;
			}
		}

		// Token: 0x06007A19 RID: 31257 RVA: 0x00306634 File Offset: 0x00305634
		internal void ScrollCellIntoView(int index)
		{
			DataGridCellsPanel dataGridCellsPanel = this.InternalItemsHost as DataGridCellsPanel;
			if (dataGridCellsPanel != null)
			{
				dataGridCellsPanel.InternalBringIndexIntoView(index);
				return;
			}
		}

		// Token: 0x17001C41 RID: 7233
		// (get) Token: 0x06007A1A RID: 31258 RVA: 0x00306658 File Offset: 0x00305658
		internal DataGrid DataGridOwner
		{
			get
			{
				DataGridRow dataGridRowOwner = this.DataGridRowOwner;
				if (dataGridRowOwner != null)
				{
					return dataGridRowOwner.DataGridOwner;
				}
				return null;
			}
		}

		// Token: 0x17001C42 RID: 7234
		// (get) Token: 0x06007A1B RID: 31259 RVA: 0x00306677 File Offset: 0x00305677
		internal DataGridRow DataGridRowOwner
		{
			get
			{
				return DataGridHelper.FindParent<DataGridRow>(this);
			}
		}

		// Token: 0x17001C43 RID: 7235
		// (get) Token: 0x06007A1C RID: 31260 RVA: 0x00306680 File Offset: 0x00305680
		private ObservableCollection<DataGridColumn> Columns
		{
			get
			{
				DataGridRow dataGridRowOwner = this.DataGridRowOwner;
				DataGrid dataGrid = (dataGridRowOwner != null) ? dataGridRowOwner.DataGridOwner : null;
				if (dataGrid == null)
				{
					return null;
				}
				return dataGrid.Columns;
			}
		}

		// Token: 0x17001C44 RID: 7236
		// (get) Token: 0x06007A1D RID: 31261 RVA: 0x003066AC File Offset: 0x003056AC
		internal ContainerTracking<DataGridCell> CellTrackingRoot
		{
			get
			{
				return this._cellTrackingRoot;
			}
		}

		// Token: 0x040039DB RID: 14811
		private object _item;

		// Token: 0x040039DC RID: 14812
		private ContainerTracking<DataGridCell> _cellTrackingRoot;

		// Token: 0x040039DD RID: 14813
		private Panel _internalItemsHost;
	}
}
