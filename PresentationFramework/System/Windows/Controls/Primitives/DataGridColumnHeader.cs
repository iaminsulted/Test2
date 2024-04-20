using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200082A RID: 2090
	[TemplatePart(Name = "PART_RightHeaderGripper", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_LeftHeaderGripper", Type = typeof(Thumb))]
	public class DataGridColumnHeader : ButtonBase, IProvideDataGridColumn
	{
		// Token: 0x06007A1E RID: 31262 RVA: 0x003066B4 File Offset: 0x003056B4
		static DataGridColumnHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(typeof(DataGridColumnHeader)));
			ContentControl.ContentProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceContent)));
			ContentControl.ContentTemplateProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceContentTemplate)));
			ContentControl.ContentTemplateSelectorProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceContentTemplateSelector)));
			ContentControl.ContentStringFormatProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceStringFormat)));
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceStyle)));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceHeight)));
			UIElement.FocusableProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(false));
			UIElement.ClipProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridColumnHeader.OnCoerceClip)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		// Token: 0x06007A1F RID: 31263 RVA: 0x00306A02 File Offset: 0x00305A02
		public DataGridColumnHeader()
		{
			this._tracker = new ContainerTracking<DataGridColumnHeader>(this);
		}

		// Token: 0x17001C45 RID: 7237
		// (get) Token: 0x06007A20 RID: 31264 RVA: 0x00306A16 File Offset: 0x00305A16
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x17001C46 RID: 7238
		// (get) Token: 0x06007A21 RID: 31265 RVA: 0x00306A1E File Offset: 0x00305A1E
		// (set) Token: 0x06007A22 RID: 31266 RVA: 0x00306A30 File Offset: 0x00305A30
		public Brush SeparatorBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGridColumnHeader.SeparatorBrushProperty);
			}
			set
			{
				base.SetValue(DataGridColumnHeader.SeparatorBrushProperty, value);
			}
		}

		// Token: 0x17001C47 RID: 7239
		// (get) Token: 0x06007A23 RID: 31267 RVA: 0x00306A3E File Offset: 0x00305A3E
		// (set) Token: 0x06007A24 RID: 31268 RVA: 0x00306A50 File Offset: 0x00305A50
		public Visibility SeparatorVisibility
		{
			get
			{
				return (Visibility)base.GetValue(DataGridColumnHeader.SeparatorVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGridColumnHeader.SeparatorVisibilityProperty, value);
			}
		}

		// Token: 0x06007A25 RID: 31269 RVA: 0x00306A64 File Offset: 0x00305A64
		internal void PrepareColumnHeader(object item, DataGridColumn column)
		{
			this._column = column;
			base.TabIndex = column.DisplayIndex;
			DataGridHelper.TransferProperty(this, ContentControl.ContentProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateSelectorProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentStringFormatProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.HeightProperty);
			base.CoerceValue(DataGridColumnHeader.CanUserSortProperty);
			base.CoerceValue(DataGridColumnHeader.SortDirectionProperty);
			base.CoerceValue(DataGridColumnHeader.IsFrozenProperty);
			base.CoerceValue(UIElement.ClipProperty);
			base.CoerceValue(DataGridColumnHeader.DisplayIndexProperty);
		}

		// Token: 0x06007A26 RID: 31270 RVA: 0x00306AFD File Offset: 0x00305AFD
		internal void ClearHeader()
		{
			this._column = null;
		}

		// Token: 0x17001C48 RID: 7240
		// (get) Token: 0x06007A27 RID: 31271 RVA: 0x00306B06 File Offset: 0x00305B06
		internal ContainerTracking<DataGridColumnHeader> Tracker
		{
			get
			{
				return this._tracker;
			}
		}

		// Token: 0x17001C49 RID: 7241
		// (get) Token: 0x06007A28 RID: 31272 RVA: 0x00306B0E File Offset: 0x00305B0E
		public int DisplayIndex
		{
			get
			{
				return (int)base.GetValue(DataGridColumnHeader.DisplayIndexProperty);
			}
		}

		// Token: 0x06007A29 RID: 31273 RVA: 0x00306B20 File Offset: 0x00305B20
		private static object OnCoerceDisplayIndex(DependencyObject d, object baseValue)
		{
			DataGridColumn column = ((DataGridColumnHeader)d).Column;
			if (column != null)
			{
				return column.DisplayIndex;
			}
			return -1;
		}

		// Token: 0x06007A2A RID: 31274 RVA: 0x00306B50 File Offset: 0x00305B50
		private static void OnDisplayIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			if (column != null)
			{
				DataGrid dataGridOwner = column.DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridColumnHeader.SetLeftGripperVisibility();
					DataGridColumnHeader dataGridColumnHeader2 = dataGridOwner.ColumnHeaderFromDisplayIndex(dataGridColumnHeader.DisplayIndex + 1);
					if (dataGridColumnHeader2 != null)
					{
						dataGridColumnHeader2.SetLeftGripperVisibility(column.CanUserResize);
					}
				}
			}
		}

		// Token: 0x06007A2B RID: 31275 RVA: 0x00306B9C File Offset: 0x00305B9C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.HookupGripperEvents();
		}

		// Token: 0x06007A2C RID: 31276 RVA: 0x00306BAC File Offset: 0x00305BAC
		private void HookupGripperEvents()
		{
			this.UnhookGripperEvents();
			this._leftGripper = (base.GetTemplateChild("PART_LeftHeaderGripper") as Thumb);
			this._rightGripper = (base.GetTemplateChild("PART_RightHeaderGripper") as Thumb);
			if (this._leftGripper != null)
			{
				this._leftGripper.DragStarted += this.OnColumnHeaderGripperDragStarted;
				this._leftGripper.DragDelta += this.OnColumnHeaderResize;
				this._leftGripper.DragCompleted += this.OnColumnHeaderGripperDragCompleted;
				this._leftGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this.SetLeftGripperVisibility();
			}
			if (this._rightGripper != null)
			{
				this._rightGripper.DragStarted += this.OnColumnHeaderGripperDragStarted;
				this._rightGripper.DragDelta += this.OnColumnHeaderResize;
				this._rightGripper.DragCompleted += this.OnColumnHeaderGripperDragCompleted;
				this._rightGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this.SetRightGripperVisibility();
			}
		}

		// Token: 0x06007A2D RID: 31277 RVA: 0x00306CC0 File Offset: 0x00305CC0
		private void UnhookGripperEvents()
		{
			if (this._leftGripper != null)
			{
				this._leftGripper.DragStarted -= this.OnColumnHeaderGripperDragStarted;
				this._leftGripper.DragDelta -= this.OnColumnHeaderResize;
				this._leftGripper.DragCompleted -= this.OnColumnHeaderGripperDragCompleted;
				this._leftGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._leftGripper = null;
			}
			if (this._rightGripper != null)
			{
				this._rightGripper.DragStarted -= this.OnColumnHeaderGripperDragStarted;
				this._rightGripper.DragDelta -= this.OnColumnHeaderResize;
				this._rightGripper.DragCompleted -= this.OnColumnHeaderGripperDragCompleted;
				this._rightGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._rightGripper = null;
			}
		}

		// Token: 0x06007A2E RID: 31278 RVA: 0x00306DA3 File Offset: 0x00305DA3
		private DataGridColumnHeader HeaderToResize(object gripper)
		{
			if (gripper != this._rightGripper)
			{
				return this.PreviousVisibleHeader;
			}
			return this;
		}

		// Token: 0x06007A2F RID: 31279 RVA: 0x00306DB8 File Offset: 0x00305DB8
		private void OnColumnHeaderGripperDragStarted(object sender, DragStartedEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = this.HeaderToResize(sender);
			if (dataGridColumnHeader != null)
			{
				if (dataGridColumnHeader.Column != null)
				{
					DataGrid dataGridOwner = dataGridColumnHeader.Column.DataGridOwner;
					if (dataGridOwner != null)
					{
						dataGridOwner.InternalColumns.OnColumnResizeStarted();
					}
				}
				e.Handled = true;
			}
		}

		// Token: 0x06007A30 RID: 31280 RVA: 0x00306DFC File Offset: 0x00305DFC
		private void OnColumnHeaderResize(object sender, DragDeltaEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = this.HeaderToResize(sender);
			if (dataGridColumnHeader != null)
			{
				DataGridColumnHeader.RecomputeColumnWidthsOnColumnResize(dataGridColumnHeader, e.HorizontalChange);
				e.Handled = true;
			}
		}

		// Token: 0x06007A31 RID: 31281 RVA: 0x00306E28 File Offset: 0x00305E28
		private static void RecomputeColumnWidthsOnColumnResize(DataGridColumnHeader header, double horizontalChange)
		{
			DataGridColumn column = header.Column;
			if (column == null)
			{
				return;
			}
			DataGrid dataGridOwner = column.DataGridOwner;
			if (dataGridOwner == null)
			{
				return;
			}
			dataGridOwner.InternalColumns.RecomputeColumnWidthsOnColumnResize(column, horizontalChange, false);
		}

		// Token: 0x06007A32 RID: 31282 RVA: 0x00306E5C File Offset: 0x00305E5C
		private void OnColumnHeaderGripperDragCompleted(object sender, DragCompletedEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = this.HeaderToResize(sender);
			if (dataGridColumnHeader != null)
			{
				if (dataGridColumnHeader.Column != null)
				{
					DataGrid dataGridOwner = dataGridColumnHeader.Column.DataGridOwner;
					if (dataGridOwner != null)
					{
						dataGridOwner.InternalColumns.OnColumnResizeCompleted(e.Canceled);
					}
				}
				e.Handled = true;
			}
		}

		// Token: 0x06007A33 RID: 31283 RVA: 0x00306EA4 File Offset: 0x00305EA4
		private void OnGripperDoubleClicked(object sender, MouseButtonEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = this.HeaderToResize(sender);
			if (dataGridColumnHeader != null && dataGridColumnHeader.Column != null)
			{
				dataGridColumnHeader.Column.Width = DataGridLength.Auto;
				e.Handled = true;
			}
		}

		// Token: 0x17001C4A RID: 7242
		// (get) Token: 0x06007A34 RID: 31284 RVA: 0x00306EDB File Offset: 0x00305EDB
		private DataGridLength ColumnWidth
		{
			get
			{
				if (this.Column == null)
				{
					return DataGridLength.Auto;
				}
				return this.Column.Width;
			}
		}

		// Token: 0x17001C4B RID: 7243
		// (get) Token: 0x06007A35 RID: 31285 RVA: 0x00306EF6 File Offset: 0x00305EF6
		private double ColumnActualWidth
		{
			get
			{
				if (this.Column == null)
				{
					return base.ActualWidth;
				}
				return this.Column.ActualWidth;
			}
		}

		// Token: 0x06007A36 RID: 31286 RVA: 0x00306F12 File Offset: 0x00305F12
		private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumnHeader)d).NotifyPropertyChanged(d, e);
		}

		// Token: 0x06007A37 RID: 31287 RVA: 0x00306F24 File Offset: 0x00305F24
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			if (dataGridColumn != null && dataGridColumn != this.Column)
			{
				return;
			}
			if (e.Property == DataGridColumn.WidthProperty)
			{
				DataGridHelper.OnColumnWidthChanged(this, e);
				return;
			}
			if (e.Property == DataGridColumn.HeaderProperty || e.Property == ContentControl.ContentProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentProperty);
				return;
			}
			if (e.Property == DataGridColumn.HeaderTemplateProperty || e.Property == ContentControl.ContentTemplateProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateProperty);
				return;
			}
			if (e.Property == DataGridColumn.HeaderTemplateSelectorProperty || e.Property == ContentControl.ContentTemplateSelectorProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateSelectorProperty);
				return;
			}
			if (e.Property == DataGridColumn.HeaderStringFormatProperty || e.Property == ContentControl.ContentStringFormatProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentStringFormatProperty);
				return;
			}
			if (e.Property == DataGrid.ColumnHeaderStyleProperty || e.Property == DataGridColumn.HeaderStyleProperty || e.Property == FrameworkElement.StyleProperty)
			{
				DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
				return;
			}
			if (e.Property == DataGrid.ColumnHeaderHeightProperty || e.Property == FrameworkElement.HeightProperty)
			{
				DataGridHelper.TransferProperty(this, FrameworkElement.HeightProperty);
				return;
			}
			if (e.Property == DataGridColumn.DisplayIndexProperty)
			{
				base.CoerceValue(DataGridColumnHeader.DisplayIndexProperty);
				base.TabIndex = dataGridColumn.DisplayIndex;
				return;
			}
			if (e.Property == DataGrid.CanUserResizeColumnsProperty)
			{
				this.OnCanUserResizeColumnsChanged();
				return;
			}
			if (e.Property == DataGridColumn.CanUserSortProperty)
			{
				base.CoerceValue(DataGridColumnHeader.CanUserSortProperty);
				return;
			}
			if (e.Property == DataGridColumn.SortDirectionProperty)
			{
				base.CoerceValue(DataGridColumnHeader.SortDirectionProperty);
				return;
			}
			if (e.Property == DataGridColumn.IsFrozenProperty)
			{
				base.CoerceValue(DataGridColumnHeader.IsFrozenProperty);
				return;
			}
			if (e.Property == DataGridColumn.CanUserResizeProperty)
			{
				this.OnCanUserResizeChanged();
				return;
			}
			if (e.Property == DataGridColumn.VisibilityProperty)
			{
				this.OnColumnVisibilityChanged(e);
			}
		}

		// Token: 0x06007A38 RID: 31288 RVA: 0x0030710C File Offset: 0x0030610C
		private void OnCanUserResizeColumnsChanged()
		{
			if (this.Column.DataGridOwner != null)
			{
				this.SetLeftGripperVisibility();
				this.SetRightGripperVisibility();
			}
		}

		// Token: 0x06007A39 RID: 31289 RVA: 0x00307127 File Offset: 0x00306127
		private void OnCanUserResizeChanged()
		{
			if (this.Column.DataGridOwner != null)
			{
				this.SetNextHeaderLeftGripperVisibility(this.Column.CanUserResize);
				this.SetRightGripperVisibility();
			}
		}

		// Token: 0x06007A3A RID: 31290 RVA: 0x00307150 File Offset: 0x00306150
		private void SetLeftGripperVisibility()
		{
			if (this._leftGripper == null || this.Column == null)
			{
				return;
			}
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			bool leftGripperVisibility = false;
			for (int i = this.DisplayIndex - 1; i >= 0; i--)
			{
				DataGridColumn dataGridColumn = dataGridOwner.ColumnFromDisplayIndex(i);
				if (dataGridColumn.IsVisible)
				{
					leftGripperVisibility = dataGridColumn.CanUserResize;
					break;
				}
			}
			this.SetLeftGripperVisibility(leftGripperVisibility);
		}

		// Token: 0x06007A3B RID: 31291 RVA: 0x003071B0 File Offset: 0x003061B0
		private void SetLeftGripperVisibility(bool canPreviousColumnResize)
		{
			if (this._leftGripper == null || this.Column == null)
			{
				return;
			}
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			if (dataGridOwner != null && dataGridOwner.CanUserResizeColumns && canPreviousColumnResize)
			{
				this._leftGripper.Visibility = Visibility.Visible;
				return;
			}
			this._leftGripper.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06007A3C RID: 31292 RVA: 0x00307204 File Offset: 0x00306204
		private void SetRightGripperVisibility()
		{
			if (this._rightGripper == null || this.Column == null)
			{
				return;
			}
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			if (dataGridOwner != null && dataGridOwner.CanUserResizeColumns && this.Column.CanUserResize)
			{
				this._rightGripper.Visibility = Visibility.Visible;
				return;
			}
			this._rightGripper.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06007A3D RID: 31293 RVA: 0x00307260 File Offset: 0x00306260
		private void SetNextHeaderLeftGripperVisibility(bool canUserResize)
		{
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			int count = dataGridOwner.Columns.Count;
			int i = this.DisplayIndex + 1;
			while (i < count)
			{
				if (dataGridOwner.ColumnFromDisplayIndex(i).IsVisible)
				{
					DataGridColumnHeader dataGridColumnHeader = dataGridOwner.ColumnHeaderFromDisplayIndex(i);
					if (dataGridColumnHeader != null)
					{
						dataGridColumnHeader.SetLeftGripperVisibility(canUserResize);
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06007A3E RID: 31294 RVA: 0x003072BC File Offset: 0x003062BC
		private void OnColumnVisibilityChanged(DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			if (dataGridOwner != null)
			{
				bool flag = (Visibility)e.OldValue == Visibility.Visible;
				bool flag2 = (Visibility)e.NewValue == Visibility.Visible;
				if (flag != flag2)
				{
					if (flag2)
					{
						this.SetLeftGripperVisibility();
						this.SetRightGripperVisibility();
						this.SetNextHeaderLeftGripperVisibility(this.Column.CanUserResize);
						return;
					}
					bool nextHeaderLeftGripperVisibility = false;
					for (int i = this.DisplayIndex - 1; i >= 0; i--)
					{
						DataGridColumn dataGridColumn = dataGridOwner.ColumnFromDisplayIndex(i);
						if (dataGridColumn.IsVisible)
						{
							nextHeaderLeftGripperVisibility = dataGridColumn.CanUserResize;
							break;
						}
					}
					this.SetNextHeaderLeftGripperVisibility(nextHeaderLeftGripperVisibility);
				}
			}
		}

		// Token: 0x06007A3F RID: 31295 RVA: 0x00307354 File Offset: 0x00306354
		private static object OnCoerceContent(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = d as DataGridColumnHeader;
			object coercedTransferPropertyValue = DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, ContentControl.ContentProperty, dataGridColumnHeader.Column, DataGridColumn.HeaderProperty);
			FrameworkObject frameworkObject = new FrameworkObject(coercedTransferPropertyValue as DependencyObject);
			if (frameworkObject.Parent != null && frameworkObject.Parent != dataGridColumnHeader)
			{
				frameworkObject.ChangeLogicalParent(null);
			}
			return coercedTransferPropertyValue;
		}

		// Token: 0x06007A40 RID: 31296 RVA: 0x003073AC File Offset: 0x003063AC
		private static object OnCoerceContentTemplate(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = d as DataGridColumnHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, ContentControl.ContentTemplateProperty, dataGridColumnHeader.Column, DataGridColumn.HeaderTemplateProperty);
		}

		// Token: 0x06007A41 RID: 31297 RVA: 0x003073D8 File Offset: 0x003063D8
		private static object OnCoerceContentTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = d as DataGridColumnHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, ContentControl.ContentTemplateSelectorProperty, dataGridColumnHeader.Column, DataGridColumn.HeaderTemplateSelectorProperty);
		}

		// Token: 0x06007A42 RID: 31298 RVA: 0x00307404 File Offset: 0x00306404
		private static object OnCoerceStringFormat(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = d as DataGridColumnHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, ContentControl.ContentStringFormatProperty, dataGridColumnHeader.Column, DataGridColumn.HeaderStringFormatProperty);
		}

		// Token: 0x06007A43 RID: 31299 RVA: 0x00307430 File Offset: 0x00306430
		private static object OnCoerceStyle(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			DataGrid grandParentObject = null;
			if (column == null)
			{
				DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = dataGridColumnHeader.TemplatedParent as DataGridColumnHeadersPresenter;
				if (dataGridColumnHeadersPresenter != null)
				{
					grandParentObject = dataGridColumnHeadersPresenter.ParentDataGrid;
				}
			}
			else
			{
				grandParentObject = column.DataGridOwner;
			}
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, FrameworkElement.StyleProperty, column, DataGridColumn.HeaderStyleProperty, grandParentObject, DataGrid.ColumnHeaderStyleProperty);
		}

		// Token: 0x17001C4C RID: 7244
		// (get) Token: 0x06007A44 RID: 31300 RVA: 0x00307487 File Offset: 0x00306487
		public bool CanUserSort
		{
			get
			{
				return (bool)base.GetValue(DataGridColumnHeader.CanUserSortProperty);
			}
		}

		// Token: 0x17001C4D RID: 7245
		// (get) Token: 0x06007A45 RID: 31301 RVA: 0x00307499 File Offset: 0x00306499
		public ListSortDirection? SortDirection
		{
			get
			{
				return (ListSortDirection?)base.GetValue(DataGridColumnHeader.SortDirectionProperty);
			}
		}

		// Token: 0x06007A46 RID: 31302 RVA: 0x003074AC File Offset: 0x003064AC
		protected override void OnClick()
		{
			if (!this.SuppressClickEvent)
			{
				if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
					if (automationPeer != null)
					{
						automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
					}
				}
				base.OnClick();
				if (this.Column != null && this.Column.DataGridOwner != null)
				{
					this.Column.DataGridOwner.PerformSort(this.Column);
				}
			}
		}

		// Token: 0x06007A47 RID: 31303 RVA: 0x0030750C File Offset: 0x0030650C
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			DataGrid parentObject = null;
			if (column == null)
			{
				DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = dataGridColumnHeader.TemplatedParent as DataGridColumnHeadersPresenter;
				if (dataGridColumnHeadersPresenter != null)
				{
					parentObject = dataGridColumnHeadersPresenter.ParentDataGrid;
				}
			}
			else
			{
				parentObject = column.DataGridOwner;
			}
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, FrameworkElement.HeightProperty, parentObject, DataGrid.ColumnHeaderHeightProperty);
		}

		// Token: 0x06007A48 RID: 31304 RVA: 0x00307560 File Offset: 0x00306560
		private static object OnCoerceCanUserSort(DependencyObject d, object baseValue)
		{
			DataGridColumn column = ((DataGridColumnHeader)d).Column;
			if (column != null)
			{
				return column.CanUserSort;
			}
			return baseValue;
		}

		// Token: 0x06007A49 RID: 31305 RVA: 0x0030758C File Offset: 0x0030658C
		private static object OnCoerceSortDirection(DependencyObject d, object baseValue)
		{
			DataGridColumn column = ((DataGridColumnHeader)d).Column;
			if (column != null)
			{
				return column.SortDirection;
			}
			return baseValue;
		}

		// Token: 0x06007A4A RID: 31306 RVA: 0x003075B5 File Offset: 0x003065B5
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridColumnHeaderAutomationPeer(this);
		}

		// Token: 0x06007A4B RID: 31307 RVA: 0x0030352B File Offset: 0x0030252B
		internal void Invoke()
		{
			this.OnClick();
		}

		// Token: 0x17001C4E RID: 7246
		// (get) Token: 0x06007A4C RID: 31308 RVA: 0x003075BD File Offset: 0x003065BD
		public bool IsFrozen
		{
			get
			{
				return (bool)base.GetValue(DataGridColumnHeader.IsFrozenProperty);
			}
		}

		// Token: 0x06007A4D RID: 31309 RVA: 0x003075D0 File Offset: 0x003065D0
		private static object OnCoerceIsFrozen(DependencyObject d, object baseValue)
		{
			DataGridColumn column = ((DataGridColumnHeader)d).Column;
			if (column != null)
			{
				return column.IsFrozen;
			}
			return baseValue;
		}

		// Token: 0x06007A4E RID: 31310 RVA: 0x003075FC File Offset: 0x003065FC
		private static object OnCoerceClip(DependencyObject d, object baseValue)
		{
			IProvideDataGridColumn cell = (DataGridColumnHeader)d;
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

		// Token: 0x17001C4F RID: 7247
		// (get) Token: 0x06007A4F RID: 31311 RVA: 0x0030762E File Offset: 0x0030662E
		internal DataGridColumnHeadersPresenter ParentPresenter
		{
			get
			{
				if (this._parentPresenter == null)
				{
					this._parentPresenter = (ItemsControl.ItemsControlFromItemContainer(this) as DataGridColumnHeadersPresenter);
				}
				return this._parentPresenter;
			}
		}

		// Token: 0x17001C50 RID: 7248
		// (get) Token: 0x06007A50 RID: 31312 RVA: 0x0030764F File Offset: 0x0030664F
		// (set) Token: 0x06007A51 RID: 31313 RVA: 0x00307657 File Offset: 0x00306657
		internal bool SuppressClickEvent
		{
			get
			{
				return this._suppressClickEvent;
			}
			set
			{
				this._suppressClickEvent = value;
			}
		}

		// Token: 0x06007A52 RID: 31314 RVA: 0x00307660 File Offset: 0x00306660
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			DataGridColumnHeadersPresenter parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				if (base.ClickMode == ClickMode.Hover && e.ButtonState == MouseButtonState.Pressed)
				{
					base.CaptureMouse();
				}
				parentPresenter.OnHeaderMouseLeftButtonDown(e);
				e.Handled = true;
			}
		}

		// Token: 0x06007A53 RID: 31315 RVA: 0x003076A8 File Offset: 0x003066A8
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			DataGridColumnHeadersPresenter parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				parentPresenter.OnHeaderMouseMove(e);
				e.Handled = true;
			}
		}

		// Token: 0x06007A54 RID: 31316 RVA: 0x003076D4 File Offset: 0x003066D4
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			DataGridColumnHeadersPresenter parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				if (base.ClickMode == ClickMode.Hover && base.IsMouseCaptured)
				{
					base.ReleaseMouseCapture();
				}
				parentPresenter.OnHeaderMouseLeftButtonUp(e);
				e.Handled = true;
			}
		}

		// Token: 0x06007A55 RID: 31317 RVA: 0x00307718 File Offset: 0x00306718
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			DataGridColumnHeadersPresenter parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				parentPresenter.OnHeaderLostMouseCapture(e);
				e.Handled = true;
			}
		}

		// Token: 0x17001C51 RID: 7249
		// (get) Token: 0x06007A56 RID: 31318 RVA: 0x00307744 File Offset: 0x00306744
		public static ComponentResourceKey ColumnHeaderDropSeparatorStyleKey
		{
			get
			{
				return SystemResourceKey.DataGridColumnHeaderColumnHeaderDropSeparatorStyleKey;
			}
		}

		// Token: 0x17001C52 RID: 7250
		// (get) Token: 0x06007A57 RID: 31319 RVA: 0x0030774B File Offset: 0x0030674B
		public static ComponentResourceKey ColumnFloatingHeaderStyleKey
		{
			get
			{
				return SystemResourceKey.DataGridColumnHeaderColumnFloatingHeaderStyleKey;
			}
		}

		// Token: 0x06007A58 RID: 31320 RVA: 0x00307754 File Offset: 0x00306754
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (base.IsPressed)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Pressed",
					"MouseOver",
					"Normal"
				});
			}
			else if (base.IsMouseOver)
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
			ListSortDirection? sortDirection = this.SortDirection;
			if (sortDirection != null)
			{
				ListSortDirection? listSortDirection = sortDirection;
				ListSortDirection listSortDirection2 = ListSortDirection.Ascending;
				if (listSortDirection.GetValueOrDefault() == listSortDirection2 & listSortDirection != null)
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"SortAscending",
						"Unsorted"
					});
				}
				listSortDirection = sortDirection;
				listSortDirection2 = ListSortDirection.Descending;
				if (listSortDirection.GetValueOrDefault() == listSortDirection2 & listSortDirection != null)
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"SortDescending",
						"Unsorted"
					});
				}
			}
			else
			{
				VisualStateManager.GoToState(this, "Unsorted", useTransitions);
			}
			base.ChangeValidationVisualState(useTransitions);
		}

		// Token: 0x17001C53 RID: 7251
		// (get) Token: 0x06007A59 RID: 31321 RVA: 0x00306A16 File Offset: 0x00305A16
		DataGridColumn IProvideDataGridColumn.Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x17001C54 RID: 7252
		// (get) Token: 0x06007A5A RID: 31322 RVA: 0x002A8C7C File Offset: 0x002A7C7C
		private Panel ParentPanel
		{
			get
			{
				return base.VisualParent as Panel;
			}
		}

		// Token: 0x17001C55 RID: 7253
		// (get) Token: 0x06007A5B RID: 31323 RVA: 0x00307854 File Offset: 0x00306854
		private DataGridColumnHeader PreviousVisibleHeader
		{
			get
			{
				DataGridColumn column = this.Column;
				if (column != null)
				{
					DataGrid dataGridOwner = column.DataGridOwner;
					if (dataGridOwner != null)
					{
						for (int i = this.DisplayIndex - 1; i >= 0; i--)
						{
							if (dataGridOwner.ColumnFromDisplayIndex(i).IsVisible)
							{
								return dataGridOwner.ColumnHeaderFromDisplayIndex(i);
							}
						}
					}
				}
				return null;
			}
		}

		// Token: 0x040039DE RID: 14814
		public static readonly DependencyProperty SeparatorBrushProperty = DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null));

		// Token: 0x040039DF RID: 14815
		public static readonly DependencyProperty SeparatorVisibilityProperty = DependencyProperty.Register("SeparatorVisibility", typeof(Visibility), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(Visibility.Visible));

		// Token: 0x040039E0 RID: 14816
		private static readonly DependencyPropertyKey DisplayIndexPropertyKey = DependencyProperty.RegisterReadOnly("DisplayIndex", typeof(int), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(DataGridColumnHeader.OnDisplayIndexChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceDisplayIndex)));

		// Token: 0x040039E1 RID: 14817
		public static readonly DependencyProperty DisplayIndexProperty = DataGridColumnHeader.DisplayIndexPropertyKey.DependencyProperty;

		// Token: 0x040039E2 RID: 14818
		private static readonly DependencyPropertyKey CanUserSortPropertyKey = DependencyProperty.RegisterReadOnly("CanUserSort", typeof(bool), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(true, null, new CoerceValueCallback(DataGridColumnHeader.OnCoerceCanUserSort)));

		// Token: 0x040039E3 RID: 14819
		public static readonly DependencyProperty CanUserSortProperty = DataGridColumnHeader.CanUserSortPropertyKey.DependencyProperty;

		// Token: 0x040039E4 RID: 14820
		private static readonly DependencyPropertyKey SortDirectionPropertyKey = DependencyProperty.RegisterReadOnly("SortDirection", typeof(ListSortDirection?), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceSortDirection)));

		// Token: 0x040039E5 RID: 14821
		public static readonly DependencyProperty SortDirectionProperty = DataGridColumnHeader.SortDirectionPropertyKey.DependencyProperty;

		// Token: 0x040039E6 RID: 14822
		private static readonly DependencyPropertyKey IsFrozenPropertyKey = DependencyProperty.RegisterReadOnly("IsFrozen", typeof(bool), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(false, null, new CoerceValueCallback(DataGridColumnHeader.OnCoerceIsFrozen)));

		// Token: 0x040039E7 RID: 14823
		public static readonly DependencyProperty IsFrozenProperty = DataGridColumnHeader.IsFrozenPropertyKey.DependencyProperty;

		// Token: 0x040039E8 RID: 14824
		private DataGridColumn _column;

		// Token: 0x040039E9 RID: 14825
		private ContainerTracking<DataGridColumnHeader> _tracker;

		// Token: 0x040039EA RID: 14826
		private DataGridColumnHeadersPresenter _parentPresenter;

		// Token: 0x040039EB RID: 14827
		private Thumb _leftGripper;

		// Token: 0x040039EC RID: 14828
		private Thumb _rightGripper;

		// Token: 0x040039ED RID: 14829
		private bool _suppressClickEvent;

		// Token: 0x040039EE RID: 14830
		private const string LeftHeaderGripperTemplateName = "PART_LeftHeaderGripper";

		// Token: 0x040039EF RID: 14831
		private const string RightHeaderGripperTemplateName = "PART_RightHeaderGripper";
	}
}
