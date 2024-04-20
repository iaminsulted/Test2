using System;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200082B RID: 2091
	[TemplatePart(Name = "PART_FillerColumnHeader", Type = typeof(DataGridColumnHeader))]
	public class DataGridColumnHeadersPresenter : ItemsControl
	{
		// Token: 0x06007A5C RID: 31324 RVA: 0x003078A0 File Offset: 0x003068A0
		static DataGridColumnHeadersPresenter()
		{
			Type typeFromHandle = typeof(DataGridColumnHeadersPresenter);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(typeFromHandle));
			UIElement.FocusableProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(false));
			FrameworkElementFactory root = new FrameworkElementFactory(typeof(DataGridCellsPanel));
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new ItemsPanelTemplate(root)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridColumnHeadersPresenter.OnIsVirtualizingPropertyChanged), new CoerceValueCallback(DataGridColumnHeadersPresenter.OnCoerceIsVirtualizingProperty)));
			VirtualizingPanel.VirtualizationModeProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(VirtualizationMode.Recycling));
		}

		// Token: 0x06007A5D RID: 31325 RVA: 0x0030794C File Offset: 0x0030694C
		public override void OnApplyTemplate()
		{
			if (this.InternalItemsHost != null && !base.IsAncestorOf(this.InternalItemsHost))
			{
				this.InternalItemsHost = null;
			}
			base.OnApplyTemplate();
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null)
			{
				base.ItemsSource = new DataGridColumnHeaderCollection(parentDataGrid.Columns);
				parentDataGrid.ColumnHeadersPresenter = this;
				DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);
				DataGridColumnHeader dataGridColumnHeader = base.GetTemplateChild("PART_FillerColumnHeader") as DataGridColumnHeader;
				if (dataGridColumnHeader != null)
				{
					DataGridHelper.TransferProperty(dataGridColumnHeader, FrameworkElement.StyleProperty);
					DataGridHelper.TransferProperty(dataGridColumnHeader, FrameworkElement.HeightProperty);
					return;
				}
			}
			else
			{
				base.ItemsSource = null;
			}
		}

		// Token: 0x06007A5E RID: 31326 RVA: 0x003079DB File Offset: 0x003069DB
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridColumnHeadersPresenterAutomationPeer(this);
		}

		// Token: 0x06007A5F RID: 31327 RVA: 0x003079E4 File Offset: 0x003069E4
		protected override Size MeasureOverride(Size availableSize)
		{
			Size size = availableSize;
			size.Width = double.PositiveInfinity;
			Size result = base.MeasureOverride(size);
			if (this._columnHeaderDragIndicator != null && this._isColumnHeaderDragging)
			{
				this._columnHeaderDragIndicator.Measure(size);
				Size desiredSize = this._columnHeaderDragIndicator.DesiredSize;
				result.Width = Math.Max(result.Width, desiredSize.Width);
				result.Height = Math.Max(result.Height, desiredSize.Height);
			}
			if (this._columnHeaderDropLocationIndicator != null && this._isColumnHeaderDragging)
			{
				this._columnHeaderDropLocationIndicator.Measure(availableSize);
				Size desiredSize = this._columnHeaderDropLocationIndicator.DesiredSize;
				result.Width = Math.Max(result.Width, desiredSize.Width);
				result.Height = Math.Max(result.Height, desiredSize.Height);
			}
			result.Width = Math.Min(availableSize.Width, result.Width);
			return result;
		}

		// Token: 0x06007A60 RID: 31328 RVA: 0x00307AE0 File Offset: 0x00306AE0
		protected override Size ArrangeOverride(Size finalSize)
		{
			UIElement uielement = (VisualTreeHelper.GetChildrenCount(this) > 0) ? (VisualTreeHelper.GetChild(this, 0) as UIElement) : null;
			if (uielement != null)
			{
				Rect finalRect = new Rect(finalSize);
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					finalRect.X = -parentDataGrid.HorizontalScrollOffset;
					finalRect.Width = Math.Max(finalSize.Width, parentDataGrid.CellsPanelActualWidth);
				}
				uielement.Arrange(finalRect);
			}
			if (this._columnHeaderDragIndicator != null && this._isColumnHeaderDragging)
			{
				this._columnHeaderDragIndicator.Arrange(new Rect(new Point(this._columnHeaderDragCurrentPosition.X - this._columnHeaderDragStartRelativePosition.X, 0.0), new Size(this._columnHeaderDragIndicator.Width, this._columnHeaderDragIndicator.Height)));
			}
			if (this._columnHeaderDropLocationIndicator != null && this._isColumnHeaderDragging)
			{
				Point location = this.FindColumnHeaderPositionByCurrentPosition(this._columnHeaderDragCurrentPosition, true);
				double width = this._columnHeaderDropLocationIndicator.Width;
				location.X -= width * 0.5;
				this._columnHeaderDropLocationIndicator.Arrange(new Rect(location, new Size(width, this._columnHeaderDropLocationIndicator.Height)));
			}
			return finalSize;
		}

		// Token: 0x06007A61 RID: 31329 RVA: 0x00307C12 File Offset: 0x00306C12
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(base.RenderSize));
			rectangleGeometry.Freeze();
			return rectangleGeometry;
		}

		// Token: 0x06007A62 RID: 31330 RVA: 0x00307C2A File Offset: 0x00306C2A
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new DataGridColumnHeader();
		}

		// Token: 0x06007A63 RID: 31331 RVA: 0x00307C31 File Offset: 0x00306C31
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is DataGridColumnHeader;
		}

		// Token: 0x06007A64 RID: 31332 RVA: 0x002CF5E9 File Offset: 0x002CE5E9
		internal bool IsItemItsOwnContainerInternal(object item)
		{
			return this.IsItemItsOwnContainerOverride(item);
		}

		// Token: 0x06007A65 RID: 31333 RVA: 0x00307C3C File Offset: 0x00306C3C
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			DataGridColumnHeader dataGridColumnHeader = element as DataGridColumnHeader;
			if (dataGridColumnHeader != null)
			{
				DataGridColumn column = this.ColumnFromContainer(dataGridColumnHeader);
				if (dataGridColumnHeader.Column == null)
				{
					dataGridColumnHeader.Tracker.StartTracking(ref this._headerTrackingRoot);
				}
				dataGridColumnHeader.PrepareColumnHeader(item, column);
			}
		}

		// Token: 0x06007A66 RID: 31334 RVA: 0x00307C7C File Offset: 0x00306C7C
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			DataGridColumnHeader dataGridColumnHeader = element as DataGridColumnHeader;
			base.ClearContainerForItemOverride(element, item);
			if (dataGridColumnHeader != null)
			{
				dataGridColumnHeader.Tracker.StopTracking(ref this._headerTrackingRoot);
				dataGridColumnHeader.ClearHeader();
			}
		}

		// Token: 0x06007A67 RID: 31335 RVA: 0x00307CB4 File Offset: 0x00306CB4
		private DataGridColumn ColumnFromContainer(DataGridColumnHeader container)
		{
			int index = base.ItemContainerGenerator.IndexFromContainer(container);
			return this.HeaderCollection.ColumnFromIndex(index);
		}

		// Token: 0x06007A68 RID: 31336 RVA: 0x00307CDA File Offset: 0x00306CDA
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			this.NotifyPropertyChanged(d, string.Empty, e, target);
		}

		// Token: 0x06007A69 RID: 31337 RVA: 0x00307CEC File Offset: 0x00306CEC
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			if (DataGridHelper.ShouldNotifyColumnHeadersPresenter(target))
			{
				if (e.Property == DataGridColumn.WidthProperty || e.Property == DataGridColumn.DisplayIndexProperty)
				{
					if (dataGridColumn.IsVisible)
					{
						this.InvalidateDataGridCellsPanelMeasureAndArrange();
					}
				}
				else if (e.Property == DataGrid.FrozenColumnCountProperty || e.Property == DataGridColumn.VisibilityProperty || e.Property == DataGrid.CellsPanelHorizontalOffsetProperty || string.Compare(propertyName, "ViewportWidth", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "DelayedColumnWidthComputation", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange();
				}
				else if (e.Property == DataGrid.HorizontalScrollOffsetProperty)
				{
					base.InvalidateArrange();
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
				else if (e.Property == DataGrid.CellsPanelActualWidthProperty)
				{
					base.InvalidateArrange();
				}
				else if (e.Property == DataGrid.EnableColumnVirtualizationProperty)
				{
					DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);
				}
			}
			if (DataGridHelper.ShouldNotifyColumnHeaders(target))
			{
				if (e.Property == DataGridColumn.HeaderProperty)
				{
					if (this.HeaderCollection != null)
					{
						this.HeaderCollection.NotifyHeaderPropertyChanged(dataGridColumn, e);
						return;
					}
				}
				else
				{
					for (ContainerTracking<DataGridColumnHeader> containerTracking = this._headerTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
					{
						containerTracking.Container.NotifyPropertyChanged(d, e);
					}
					if (d is DataGrid && (e.Property == DataGrid.ColumnHeaderStyleProperty || e.Property == DataGrid.ColumnHeaderHeightProperty))
					{
						DataGridColumnHeader dataGridColumnHeader = base.GetTemplateChild("PART_FillerColumnHeader") as DataGridColumnHeader;
						if (dataGridColumnHeader != null)
						{
							dataGridColumnHeader.NotifyPropertyChanged(d, e);
						}
					}
				}
			}
		}

		// Token: 0x06007A6A RID: 31338 RVA: 0x00307E98 File Offset: 0x00306E98
		private static void OnIsVirtualizingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = (DataGridColumnHeadersPresenter)d;
			DataGridHelper.TransferProperty(dataGridColumnHeadersPresenter, VirtualizingPanel.IsVirtualizingProperty);
			if (e.OldValue != dataGridColumnHeadersPresenter.GetValue(VirtualizingPanel.IsVirtualizingProperty))
			{
				dataGridColumnHeadersPresenter.InvalidateDataGridCellsPanelMeasureAndArrange();
			}
		}

		// Token: 0x06007A6B RID: 31339 RVA: 0x00307ED4 File Offset: 0x00306ED4
		private static object OnCoerceIsVirtualizingProperty(DependencyObject d, object baseValue)
		{
			DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = d as DataGridColumnHeadersPresenter;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeadersPresenter, baseValue, VirtualizingPanel.IsVirtualizingProperty, dataGridColumnHeadersPresenter.ParentDataGrid, DataGrid.EnableColumnVirtualizationProperty);
		}

		// Token: 0x06007A6C RID: 31340 RVA: 0x00307EFF File Offset: 0x00306EFF
		private void InvalidateDataGridCellsPanelMeasureAndArrange()
		{
			if (this._internalItemsHost != null)
			{
				this._internalItemsHost.InvalidateMeasure();
				this._internalItemsHost.InvalidateArrange();
			}
		}

		// Token: 0x06007A6D RID: 31341 RVA: 0x00307F1F File Offset: 0x00306F1F
		private void InvalidateDataGridCellsPanelMeasureAndArrange(bool withColumnVirtualization)
		{
			if (withColumnVirtualization == VirtualizingPanel.GetIsVirtualizing(this))
			{
				this.InvalidateDataGridCellsPanelMeasureAndArrange();
			}
		}

		// Token: 0x17001C56 RID: 7254
		// (get) Token: 0x06007A6E RID: 31342 RVA: 0x00307F30 File Offset: 0x00306F30
		// (set) Token: 0x06007A6F RID: 31343 RVA: 0x00307F38 File Offset: 0x00306F38
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

		// Token: 0x17001C57 RID: 7255
		// (get) Token: 0x06007A70 RID: 31344 RVA: 0x00307F44 File Offset: 0x00306F44
		protected override int VisualChildrenCount
		{
			get
			{
				int num = base.VisualChildrenCount;
				if (this._columnHeaderDragIndicator != null)
				{
					num++;
				}
				if (this._columnHeaderDropLocationIndicator != null)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x06007A71 RID: 31345 RVA: 0x00307F74 File Offset: 0x00306F74
		protected override Visual GetVisualChild(int index)
		{
			int visualChildrenCount = base.VisualChildrenCount;
			if (index == visualChildrenCount)
			{
				if (this._columnHeaderDragIndicator != null)
				{
					return this._columnHeaderDragIndicator;
				}
				if (this._columnHeaderDropLocationIndicator != null)
				{
					return this._columnHeaderDropLocationIndicator;
				}
			}
			if (index == visualChildrenCount + 1 && this._columnHeaderDragIndicator != null && this._columnHeaderDropLocationIndicator != null)
			{
				return this._columnHeaderDropLocationIndicator;
			}
			return base.GetVisualChild(index);
		}

		// Token: 0x06007A72 RID: 31346 RVA: 0x00307FD0 File Offset: 0x00306FD0
		internal void OnHeaderMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (this.ParentDataGrid == null)
			{
				return;
			}
			if (this._columnHeaderDragIndicator != null)
			{
				base.RemoveVisualChild(this._columnHeaderDragIndicator);
				this._columnHeaderDragIndicator = null;
			}
			if (this._columnHeaderDropLocationIndicator != null)
			{
				base.RemoveVisualChild(this._columnHeaderDropLocationIndicator);
				this._columnHeaderDropLocationIndicator = null;
			}
			Point position = e.GetPosition(this);
			DataGridColumnHeader dataGridColumnHeader = this.FindColumnHeaderByPosition(position);
			if (dataGridColumnHeader != null)
			{
				DataGridColumn column = dataGridColumnHeader.Column;
				if (this.ParentDataGrid.CanUserReorderColumns && column.CanUserReorder)
				{
					this.PrepareColumnHeaderDrag(dataGridColumnHeader, e.GetPosition(this), e.GetPosition(dataGridColumnHeader));
					return;
				}
			}
			else
			{
				this._isColumnHeaderDragging = false;
				this._prepareColumnHeaderDragging = false;
				this._draggingSrcColumnHeader = null;
				base.InvalidateArrange();
			}
		}

		// Token: 0x06007A73 RID: 31347 RVA: 0x0030807C File Offset: 0x0030707C
		internal void OnHeaderMouseMove(MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && this._prepareColumnHeaderDragging)
			{
				this._columnHeaderDragCurrentPosition = e.GetPosition(this);
				if (!this._isColumnHeaderDragging)
				{
					if (DataGridColumnHeadersPresenter.CheckStartColumnHeaderDrag(this._columnHeaderDragCurrentPosition, this._columnHeaderDragStartPosition))
					{
						this.StartColumnHeaderDrag();
						return;
					}
				}
				else
				{
					Visibility visibility = this.IsMousePositionValidForColumnDrag(2.0) ? Visibility.Visible : Visibility.Collapsed;
					if (this._columnHeaderDragIndicator != null)
					{
						this._columnHeaderDragIndicator.Visibility = visibility;
					}
					if (this._columnHeaderDropLocationIndicator != null)
					{
						this._columnHeaderDropLocationIndicator.Visibility = visibility;
					}
					base.InvalidateArrange();
					DragDeltaEventArgs e2 = new DragDeltaEventArgs(this._columnHeaderDragCurrentPosition.X - this._columnHeaderDragStartPosition.X, this._columnHeaderDragCurrentPosition.Y - this._columnHeaderDragStartPosition.Y);
					this._columnHeaderDragStartPosition = this._columnHeaderDragCurrentPosition;
					this.ParentDataGrid.OnColumnHeaderDragDelta(e2);
				}
			}
		}

		// Token: 0x06007A74 RID: 31348 RVA: 0x00308162 File Offset: 0x00307162
		internal void OnHeaderMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (this._isColumnHeaderDragging)
			{
				this._columnHeaderDragCurrentPosition = e.GetPosition(this);
				this.FinishColumnHeaderDrag(false);
				return;
			}
			this.ClearColumnHeaderDragInfo();
		}

		// Token: 0x06007A75 RID: 31349 RVA: 0x00308187 File Offset: 0x00307187
		internal void OnHeaderLostMouseCapture(MouseEventArgs e)
		{
			if (this._isColumnHeaderDragging && Mouse.LeftButton == MouseButtonState.Pressed)
			{
				this.FinishColumnHeaderDrag(true);
			}
		}

		// Token: 0x06007A76 RID: 31350 RVA: 0x003081A0 File Offset: 0x003071A0
		private void ClearColumnHeaderDragInfo()
		{
			this._isColumnHeaderDragging = false;
			this._prepareColumnHeaderDragging = false;
			this._draggingSrcColumnHeader = null;
			if (this._columnHeaderDragIndicator != null)
			{
				base.RemoveVisualChild(this._columnHeaderDragIndicator);
				this._columnHeaderDragIndicator = null;
			}
			if (this._columnHeaderDropLocationIndicator != null)
			{
				base.RemoveVisualChild(this._columnHeaderDropLocationIndicator);
				this._columnHeaderDropLocationIndicator = null;
			}
		}

		// Token: 0x06007A77 RID: 31351 RVA: 0x003081F8 File Offset: 0x003071F8
		private void PrepareColumnHeaderDrag(DataGridColumnHeader header, Point pos, Point relativePos)
		{
			this._prepareColumnHeaderDragging = true;
			this._isColumnHeaderDragging = false;
			this._draggingSrcColumnHeader = header;
			this._columnHeaderDragStartPosition = pos;
			this._columnHeaderDragStartRelativePosition = relativePos;
		}

		// Token: 0x06007A78 RID: 31352 RVA: 0x0030821D File Offset: 0x0030721D
		private static bool CheckStartColumnHeaderDrag(Point currentPos, Point originalPos)
		{
			return DoubleUtil.GreaterThan(Math.Abs(currentPos.X - originalPos.X), SystemParameters.MinimumHorizontalDragDistance);
		}

		// Token: 0x06007A79 RID: 31353 RVA: 0x00308240 File Offset: 0x00307240
		private bool IsMousePositionValidForColumnDrag(double dragFactor)
		{
			int num = -1;
			return this.IsMousePositionValidForColumnDrag(dragFactor, out num);
		}

		// Token: 0x06007A7A RID: 31354 RVA: 0x00308258 File Offset: 0x00307258
		private bool IsMousePositionValidForColumnDrag(double dragFactor, out int nearestDisplayIndex)
		{
			nearestDisplayIndex = -1;
			bool flag = false;
			if (this._draggingSrcColumnHeader.Column != null)
			{
				flag = this._draggingSrcColumnHeader.Column.IsFrozen;
			}
			int num = 0;
			if (this.ParentDataGrid != null)
			{
				num = this.ParentDataGrid.FrozenColumnCount;
			}
			nearestDisplayIndex = this.FindDisplayIndexByPosition(this._columnHeaderDragCurrentPosition, true);
			if (flag && nearestDisplayIndex >= num)
			{
				return false;
			}
			if (!flag && nearestDisplayIndex < num)
			{
				return false;
			}
			double num2;
			if (this._columnHeaderDragIndicator == null)
			{
				num2 = this._draggingSrcColumnHeader.RenderSize.Height;
			}
			else
			{
				num2 = Math.Max(this._draggingSrcColumnHeader.RenderSize.Height, this._columnHeaderDragIndicator.Height);
			}
			return DoubleUtil.LessThanOrClose(-num2 * dragFactor, this._columnHeaderDragCurrentPosition.Y) && DoubleUtil.LessThanOrClose(this._columnHeaderDragCurrentPosition.Y, num2 * (dragFactor + 1.0));
		}

		// Token: 0x06007A7B RID: 31355 RVA: 0x00308344 File Offset: 0x00307344
		private void StartColumnHeaderDrag()
		{
			this._columnHeaderDragStartPosition = this._columnHeaderDragCurrentPosition;
			DragStartedEventArgs e = new DragStartedEventArgs(this._columnHeaderDragStartPosition.X, this._columnHeaderDragStartPosition.Y);
			this.ParentDataGrid.OnColumnHeaderDragStarted(e);
			DataGridColumnReorderingEventArgs dataGridColumnReorderingEventArgs = new DataGridColumnReorderingEventArgs(this._draggingSrcColumnHeader.Column);
			this._columnHeaderDragIndicator = this.CreateColumnHeaderDragIndicator();
			this._columnHeaderDropLocationIndicator = this.CreateColumnHeaderDropIndicator();
			dataGridColumnReorderingEventArgs.DragIndicator = this._columnHeaderDragIndicator;
			dataGridColumnReorderingEventArgs.DropLocationIndicator = this._columnHeaderDropLocationIndicator;
			this.ParentDataGrid.OnColumnReordering(dataGridColumnReorderingEventArgs);
			if (!dataGridColumnReorderingEventArgs.Cancel)
			{
				this._isColumnHeaderDragging = true;
				this._columnHeaderDragIndicator = dataGridColumnReorderingEventArgs.DragIndicator;
				this._columnHeaderDropLocationIndicator = dataGridColumnReorderingEventArgs.DropLocationIndicator;
				if (this._columnHeaderDragIndicator != null)
				{
					this.SetDefaultsOnDragIndicator();
					base.AddVisualChild(this._columnHeaderDragIndicator);
				}
				if (this._columnHeaderDropLocationIndicator != null)
				{
					this.SetDefaultsOnDropIndicator();
					base.AddVisualChild(this._columnHeaderDropLocationIndicator);
				}
				this._draggingSrcColumnHeader.SuppressClickEvent = true;
				base.InvalidateMeasure();
				return;
			}
			this.FinishColumnHeaderDrag(true);
		}

		// Token: 0x06007A7C RID: 31356 RVA: 0x00308447 File Offset: 0x00307447
		private Control CreateColumnHeaderDragIndicator()
		{
			return new DataGridColumnFloatingHeader
			{
				ReferenceHeader = this._draggingSrcColumnHeader
			};
		}

		// Token: 0x06007A7D RID: 31357 RVA: 0x0030845C File Offset: 0x0030745C
		private void SetDefaultsOnDragIndicator()
		{
			DataGridColumn column = this._draggingSrcColumnHeader.Column;
			Style style = null;
			if (column != null)
			{
				style = column.DragIndicatorStyle;
			}
			this._columnHeaderDragIndicator.Style = style;
			this._columnHeaderDragIndicator.CoerceValue(FrameworkElement.WidthProperty);
			this._columnHeaderDragIndicator.CoerceValue(FrameworkElement.HeightProperty);
		}

		// Token: 0x06007A7E RID: 31358 RVA: 0x003084AD File Offset: 0x003074AD
		private Control CreateColumnHeaderDropIndicator()
		{
			return new DataGridColumnDropSeparator
			{
				ReferenceHeader = this._draggingSrcColumnHeader
			};
		}

		// Token: 0x06007A7F RID: 31359 RVA: 0x003084C0 File Offset: 0x003074C0
		private void SetDefaultsOnDropIndicator()
		{
			Style style = null;
			if (this.ParentDataGrid != null)
			{
				style = this.ParentDataGrid.DropLocationIndicatorStyle;
			}
			this._columnHeaderDropLocationIndicator.Style = style;
			this._columnHeaderDropLocationIndicator.CoerceValue(FrameworkElement.WidthProperty);
			this._columnHeaderDropLocationIndicator.CoerceValue(FrameworkElement.HeightProperty);
		}

		// Token: 0x06007A80 RID: 31360 RVA: 0x00308510 File Offset: 0x00307510
		private void FinishColumnHeaderDrag(bool isCancel)
		{
			this._prepareColumnHeaderDragging = false;
			this._isColumnHeaderDragging = false;
			this._draggingSrcColumnHeader.SuppressClickEvent = false;
			if (this._columnHeaderDragIndicator != null)
			{
				this._columnHeaderDragIndicator.Visibility = Visibility.Collapsed;
				DataGridColumnFloatingHeader dataGridColumnFloatingHeader = this._columnHeaderDragIndicator as DataGridColumnFloatingHeader;
				if (dataGridColumnFloatingHeader != null)
				{
					dataGridColumnFloatingHeader.ClearHeader();
				}
				base.RemoveVisualChild(this._columnHeaderDragIndicator);
			}
			if (this._columnHeaderDropLocationIndicator != null)
			{
				this._columnHeaderDropLocationIndicator.Visibility = Visibility.Collapsed;
				DataGridColumnDropSeparator dataGridColumnDropSeparator = this._columnHeaderDropLocationIndicator as DataGridColumnDropSeparator;
				if (dataGridColumnDropSeparator != null)
				{
					dataGridColumnDropSeparator.ReferenceHeader = null;
				}
				base.RemoveVisualChild(this._columnHeaderDropLocationIndicator);
			}
			DragCompletedEventArgs e = new DragCompletedEventArgs(this._columnHeaderDragCurrentPosition.X - this._columnHeaderDragStartPosition.X, this._columnHeaderDragCurrentPosition.Y - this._columnHeaderDragStartPosition.Y, isCancel);
			this.ParentDataGrid.OnColumnHeaderDragCompleted(e);
			this._draggingSrcColumnHeader.InvalidateArrange();
			if (!isCancel)
			{
				int num = -1;
				bool flag = this.IsMousePositionValidForColumnDrag(2.0, out num);
				DataGridColumn column = this._draggingSrcColumnHeader.Column;
				if (column != null && flag && num != column.DisplayIndex)
				{
					column.DisplayIndex = num;
					DataGridColumnEventArgs e2 = new DataGridColumnEventArgs(this._draggingSrcColumnHeader.Column);
					this.ParentDataGrid.OnColumnReordered(e2);
				}
			}
			this._draggingSrcColumnHeader = null;
			this._columnHeaderDragIndicator = null;
			this._columnHeaderDropLocationIndicator = null;
		}

		// Token: 0x06007A81 RID: 31361 RVA: 0x00308664 File Offset: 0x00307664
		private int FindDisplayIndexByPosition(Point startPos, bool findNearestColumn)
		{
			int result;
			Point point;
			DataGridColumnHeader dataGridColumnHeader;
			this.FindDisplayIndexAndHeaderPosition(startPos, findNearestColumn, out result, out point, out dataGridColumnHeader);
			return result;
		}

		// Token: 0x06007A82 RID: 31362 RVA: 0x00308680 File Offset: 0x00307680
		private DataGridColumnHeader FindColumnHeaderByPosition(Point startPos)
		{
			int num;
			Point point;
			DataGridColumnHeader result;
			this.FindDisplayIndexAndHeaderPosition(startPos, false, out num, out point, out result);
			return result;
		}

		// Token: 0x06007A83 RID: 31363 RVA: 0x0030869C File Offset: 0x0030769C
		private Point FindColumnHeaderPositionByCurrentPosition(Point startPos, bool findNearestColumn)
		{
			int num;
			Point result;
			DataGridColumnHeader dataGridColumnHeader;
			this.FindDisplayIndexAndHeaderPosition(startPos, findNearestColumn, out num, out result, out dataGridColumnHeader);
			return result;
		}

		// Token: 0x06007A84 RID: 31364 RVA: 0x003086B8 File Offset: 0x003076B8
		private static double GetColumnEstimatedWidth(DataGridColumn column, double averageColumnWidth)
		{
			double num = column.Width.DisplayValue;
			if (DoubleUtil.IsNaN(num))
			{
				num = Math.Max(averageColumnWidth, column.MinWidth);
				num = Math.Min(num, column.MaxWidth);
			}
			return num;
		}

		// Token: 0x06007A85 RID: 31365 RVA: 0x003086F8 File Offset: 0x003076F8
		private void FindDisplayIndexAndHeaderPosition(Point startPos, bool findNearestColumn, out int displayIndex, out Point headerPos, out DataGridColumnHeader header)
		{
			Point point = new Point(0.0, 0.0);
			headerPos = point;
			displayIndex = -1;
			header = null;
			if (startPos.X < 0.0)
			{
				if (findNearestColumn)
				{
					displayIndex = 0;
				}
				return;
			}
			double num = 0.0;
			double num2 = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
			bool flag = false;
			int i = 0;
			while (i < parentDataGrid.Columns.Count)
			{
				displayIndex++;
				DataGridColumnHeader dataGridColumnHeader = parentDataGrid.ColumnHeaderFromDisplayIndex(i);
				if (dataGridColumnHeader != null)
				{
					num = dataGridColumnHeader.TransformToAncestor(this).Transform(point).X;
					num2 = num + dataGridColumnHeader.RenderSize.Width;
					goto IL_F7;
				}
				DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(i);
				if (dataGridColumn.IsVisible)
				{
					num = num2;
					if (i >= parentDataGrid.FrozenColumnCount && !flag)
					{
						num -= parentDataGrid.HorizontalScrollOffset;
						flag = true;
					}
					num2 = num + DataGridColumnHeadersPresenter.GetColumnEstimatedWidth(dataGridColumn, averageColumnWidth);
					goto IL_F7;
				}
				IL_189:
				i++;
				continue;
				IL_F7:
				if (DoubleUtil.LessThanOrClose(startPos.X, num))
				{
					break;
				}
				if (!DoubleUtil.GreaterThanOrClose(startPos.X, num) || !DoubleUtil.LessThanOrClose(startPos.X, num2))
				{
					goto IL_189;
				}
				if (!findNearestColumn)
				{
					header = dataGridColumnHeader;
					break;
				}
				double value = (num + num2) * 0.5;
				if (DoubleUtil.GreaterThanOrClose(startPos.X, value))
				{
					num = num2;
					displayIndex++;
				}
				if (this._draggingSrcColumnHeader != null && this._draggingSrcColumnHeader.Column != null && this._draggingSrcColumnHeader.Column.DisplayIndex < displayIndex)
				{
					displayIndex--;
					break;
				}
				break;
			}
			if (i == parentDataGrid.Columns.Count)
			{
				displayIndex = parentDataGrid.Columns.Count - 1;
				num = num2;
			}
			headerPos.X = num;
		}

		// Token: 0x17001C58 RID: 7256
		// (get) Token: 0x06007A86 RID: 31366 RVA: 0x003088CD File Offset: 0x003078CD
		private DataGridColumnHeaderCollection HeaderCollection
		{
			get
			{
				return base.ItemsSource as DataGridColumnHeaderCollection;
			}
		}

		// Token: 0x17001C59 RID: 7257
		// (get) Token: 0x06007A87 RID: 31367 RVA: 0x003088DA File Offset: 0x003078DA
		internal DataGrid ParentDataGrid
		{
			get
			{
				if (this._parentDataGrid == null)
				{
					this._parentDataGrid = DataGridHelper.FindParent<DataGrid>(this);
				}
				return this._parentDataGrid;
			}
		}

		// Token: 0x17001C5A RID: 7258
		// (get) Token: 0x06007A88 RID: 31368 RVA: 0x003088F6 File Offset: 0x003078F6
		internal ContainerTracking<DataGridColumnHeader> HeaderTrackingRoot
		{
			get
			{
				return this._headerTrackingRoot;
			}
		}

		// Token: 0x040039F0 RID: 14832
		private const string ElementFillerColumnHeader = "PART_FillerColumnHeader";

		// Token: 0x040039F1 RID: 14833
		private ContainerTracking<DataGridColumnHeader> _headerTrackingRoot;

		// Token: 0x040039F2 RID: 14834
		private DataGrid _parentDataGrid;

		// Token: 0x040039F3 RID: 14835
		private bool _prepareColumnHeaderDragging;

		// Token: 0x040039F4 RID: 14836
		private bool _isColumnHeaderDragging;

		// Token: 0x040039F5 RID: 14837
		private DataGridColumnHeader _draggingSrcColumnHeader;

		// Token: 0x040039F6 RID: 14838
		private Point _columnHeaderDragStartPosition;

		// Token: 0x040039F7 RID: 14839
		private Point _columnHeaderDragStartRelativePosition;

		// Token: 0x040039F8 RID: 14840
		private Point _columnHeaderDragCurrentPosition;

		// Token: 0x040039F9 RID: 14841
		private Control _columnHeaderDropLocationIndicator;

		// Token: 0x040039FA RID: 14842
		private Control _columnHeaderDragIndicator;

		// Token: 0x040039FB RID: 14843
		private Panel _internalItemsHost;
	}
}
