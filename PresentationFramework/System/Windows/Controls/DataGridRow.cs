using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000760 RID: 1888
	public class DataGridRow : Control
	{
		// Token: 0x0600668F RID: 26255 RVA: 0x002B212C File Offset: 0x002B112C
		static DataGridRow()
		{
			DataGridRow.SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(DataGridRow));
			DataGridRow.UnselectedEvent = Selector.UnselectedEvent.AddOwner(typeof(DataGridRow));
			DataGridRow.IsEditingPropertyKey = DependencyProperty.RegisterReadOnly("IsEditing", typeof(bool), typeof(DataGridRow), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged)));
			DataGridRow.IsEditingProperty = DataGridRow.IsEditingPropertyKey.DependencyProperty;
			DataGridRow.IsNewItemPropertyKey = DependencyProperty.RegisterReadOnly("IsNewItem", typeof(bool), typeof(DataGridRow), new FrameworkPropertyMetadata(false));
			DataGridRow.IsNewItemProperty = DataGridRow.IsNewItemPropertyKey.DependencyProperty;
			UIElement.VisibilityProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridRow.OnCoerceVisibility)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(typeof(DataGridRow)));
			DataGridRow.ItemsPanelProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(new ItemsPanelTemplate(new FrameworkElementFactory(typeof(DataGridCellsPanel)))));
			UIElement.FocusableProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(false));
			Control.BackgroundProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceBackground)));
			FrameworkElement.BindingGroupProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRow.OnNotifyRowPropertyChanged)));
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(DataGridRow), new UIPropertyMetadata(new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged)));
			VirtualizingPanel.ShouldCacheContainerSizeProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridRow.OnCoerceShouldCacheContainerSize)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x002B268F File Offset: 0x002B168F
		public DataGridRow()
		{
			this._tracker = new ContainerTracking<DataGridRow>(this);
		}

		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x06006691 RID: 26257 RVA: 0x002B26A3 File Offset: 0x002B16A3
		// (set) Token: 0x06006692 RID: 26258 RVA: 0x002B26B0 File Offset: 0x002B16B0
		public object Item
		{
			get
			{
				return base.GetValue(DataGridRow.ItemProperty);
			}
			set
			{
				base.SetValue(DataGridRow.ItemProperty, value);
			}
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x002B26C0 File Offset: 0x002B16C0
		protected virtual void OnItemChanged(object oldItem, object newItem)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.Item = newItem;
			}
		}

		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x06006694 RID: 26260 RVA: 0x002B26DE File Offset: 0x002B16DE
		// (set) Token: 0x06006695 RID: 26261 RVA: 0x002B26F0 File Offset: 0x002B16F0
		public ItemsPanelTemplate ItemsPanel
		{
			get
			{
				return (ItemsPanelTemplate)base.GetValue(DataGridRow.ItemsPanelProperty);
			}
			set
			{
				base.SetValue(DataGridRow.ItemsPanelProperty, value);
			}
		}

		// Token: 0x06006696 RID: 26262 RVA: 0x002B26FE File Offset: 0x002B16FE
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			this.CellsPresenter = null;
			this.DetailsPresenter = null;
		}

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x06006697 RID: 26263 RVA: 0x002B2718 File Offset: 0x002B1718
		private bool IsDataGridKeyboardFocusWithin
		{
			get
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				return dataGridOwner != null && dataGridOwner.IsKeyboardFocusWithin;
			}
		}

		// Token: 0x06006698 RID: 26264 RVA: 0x002B2738 File Offset: 0x002B1738
		internal override void ChangeVisualState(bool useTransitions)
		{
			byte b = 0;
			if (this.IsSelected || this.IsEditing)
			{
				b += 8;
			}
			if (this.IsEditing)
			{
				b += 4;
			}
			if (base.IsMouseOver)
			{
				b += 2;
			}
			if (this.IsDataGridKeyboardFocusWithin)
			{
				b += 1;
			}
			for (byte b2 = DataGridRow._idealStateMapping[(int)b]; b2 != 255; b2 = DataGridRow._fallbackStateMapping[(int)b2])
			{
				string stateName;
				if (b2 == 5)
				{
					if (this.AlternationIndex % 2 == 1)
					{
						stateName = "Normal_AlternatingRow";
					}
					else
					{
						stateName = "Normal";
					}
				}
				else
				{
					stateName = DataGridRow._stateNames[(int)b2];
				}
				if (VisualStateManager.GoToState(this, stateName, useTransitions))
				{
					break;
				}
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x06006699 RID: 26265 RVA: 0x002B27D5 File Offset: 0x002B17D5
		// (set) Token: 0x0600669A RID: 26266 RVA: 0x002B27E2 File Offset: 0x002B17E2
		public object Header
		{
			get
			{
				return base.GetValue(DataGridRow.HeaderProperty);
			}
			set
			{
				base.SetValue(DataGridRow.HeaderProperty, value);
			}
		}

		// Token: 0x0600669B RID: 26267 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
		{
		}

		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x0600669C RID: 26268 RVA: 0x002B27F0 File Offset: 0x002B17F0
		// (set) Token: 0x0600669D RID: 26269 RVA: 0x002B2802 File Offset: 0x002B1802
		public Style HeaderStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridRow.HeaderStyleProperty);
			}
			set
			{
				base.SetValue(DataGridRow.HeaderStyleProperty, value);
			}
		}

		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x0600669E RID: 26270 RVA: 0x002B2810 File Offset: 0x002B1810
		// (set) Token: 0x0600669F RID: 26271 RVA: 0x002B2822 File Offset: 0x002B1822
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridRow.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridRow.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x060066A0 RID: 26272 RVA: 0x002B2830 File Offset: 0x002B1830
		// (set) Token: 0x060066A1 RID: 26273 RVA: 0x002B2842 File Offset: 0x002B1842
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridRow.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridRow.HeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x060066A2 RID: 26274 RVA: 0x002B2850 File Offset: 0x002B1850
		// (set) Token: 0x060066A3 RID: 26275 RVA: 0x002B2862 File Offset: 0x002B1862
		public ControlTemplate ValidationErrorTemplate
		{
			get
			{
				return (ControlTemplate)base.GetValue(DataGridRow.ValidationErrorTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridRow.ValidationErrorTemplateProperty, value);
			}
		}

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x060066A4 RID: 26276 RVA: 0x002B2870 File Offset: 0x002B1870
		// (set) Token: 0x060066A5 RID: 26277 RVA: 0x002B2882 File Offset: 0x002B1882
		public DataTemplate DetailsTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridRow.DetailsTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridRow.DetailsTemplateProperty, value);
			}
		}

		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x060066A6 RID: 26278 RVA: 0x002B2890 File Offset: 0x002B1890
		// (set) Token: 0x060066A7 RID: 26279 RVA: 0x002B28A2 File Offset: 0x002B18A2
		public DataTemplateSelector DetailsTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridRow.DetailsTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridRow.DetailsTemplateSelectorProperty, value);
			}
		}

		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x060066A8 RID: 26280 RVA: 0x002B28B0 File Offset: 0x002B18B0
		// (set) Token: 0x060066A9 RID: 26281 RVA: 0x002B28C2 File Offset: 0x002B18C2
		public Visibility DetailsVisibility
		{
			get
			{
				return (Visibility)base.GetValue(DataGridRow.DetailsVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGridRow.DetailsVisibilityProperty, value);
			}
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x060066AA RID: 26282 RVA: 0x002B28D5 File Offset: 0x002B18D5
		// (set) Token: 0x060066AB RID: 26283 RVA: 0x002B28DD File Offset: 0x002B18DD
		internal bool DetailsLoaded
		{
			get
			{
				return this._detailsLoaded;
			}
			set
			{
				this._detailsLoaded = value;
			}
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x002B28E6 File Offset: 0x002B18E6
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DataGridRow.AlternationIndexProperty)
			{
				this.NotifyPropertyChanged(this, e, DataGridNotificationTarget.Rows);
			}
		}

		// Token: 0x060066AD RID: 26285 RVA: 0x002B290C File Offset: 0x002B190C
		internal void PrepareRow(object item, DataGrid owningDataGrid)
		{
			bool flag = this._owner != owningDataGrid;
			bool forcePrepareCells = false;
			this._owner = owningDataGrid;
			if (this != item)
			{
				if (this.Item != item)
				{
					this.Item = item;
				}
				else
				{
					forcePrepareCells = true;
				}
			}
			if (this.IsEditing)
			{
				this.IsEditing = false;
			}
			if (flag)
			{
				this.SyncProperties(forcePrepareCells);
			}
			base.CoerceValue(VirtualizingPanel.ShouldCacheContainerSizeProperty);
			base.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.DelayedValidateWithoutUpdate), DispatcherPriority.DataBind, new object[]
			{
				base.BindingGroup
			});
		}

		// Token: 0x060066AE RID: 26286 RVA: 0x002B2994 File Offset: 0x002B1994
		internal void ClearRow(DataGrid owningDataGrid)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				this.PersistAttachedItemValue(cellsPresenter, FrameworkElement.HeightProperty);
			}
			this.PersistAttachedItemValue(this, DataGridRow.DetailsVisibilityProperty);
			this.Item = BindingExpressionBase.DisconnectedItem;
			DataGridDetailsPresenter detailsPresenter = this.DetailsPresenter;
			if (detailsPresenter != null)
			{
				detailsPresenter.Content = BindingExpressionBase.DisconnectedItem;
			}
			this._owner = null;
		}

		// Token: 0x060066AF RID: 26287 RVA: 0x002B29EC File Offset: 0x002B19EC
		private void PersistAttachedItemValue(DependencyObject objectWithProperty, DependencyProperty property)
		{
			if (DependencyPropertyHelper.GetValueSource(objectWithProperty, property).BaseValueSource == BaseValueSource.Local)
			{
				this._owner.ItemAttachedStorage.SetValue(this.Item, property, objectWithProperty.GetValue(property));
				objectWithProperty.ClearValue(property);
			}
		}

		// Token: 0x060066B0 RID: 26288 RVA: 0x002B2A34 File Offset: 0x002B1A34
		private void RestoreAttachedItemValue(DependencyObject objectWithProperty, DependencyProperty property)
		{
			object value;
			if (this._owner.ItemAttachedStorage.TryGetValue(this.Item, property, out value))
			{
				objectWithProperty.SetValue(property, value);
			}
		}

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x060066B1 RID: 26289 RVA: 0x002B2A64 File Offset: 0x002B1A64
		internal ContainerTracking<DataGridRow> Tracker
		{
			get
			{
				return this._tracker;
			}
		}

		// Token: 0x060066B2 RID: 26290 RVA: 0x002B2A6C File Offset: 0x002B1A6C
		internal void OnRowResizeStarted()
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				this._cellsPresenterResizeHeight = cellsPresenter.Height;
			}
		}

		// Token: 0x060066B3 RID: 26291 RVA: 0x002B2A90 File Offset: 0x002B1A90
		internal void OnRowResize(double changeAmount)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				double num = cellsPresenter.ActualHeight + changeAmount;
				double num2 = Math.Max(this.RowHeader.DesiredSize.Height, base.MinHeight);
				if (DoubleUtil.LessThan(num, num2))
				{
					num = num2;
				}
				double maxHeight = base.MaxHeight;
				if (DoubleUtil.GreaterThan(num, maxHeight))
				{
					num = maxHeight;
				}
				cellsPresenter.Height = num;
			}
		}

		// Token: 0x060066B4 RID: 26292 RVA: 0x002B2AF4 File Offset: 0x002B1AF4
		internal void OnRowResizeCompleted(bool canceled)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null && canceled)
			{
				cellsPresenter.Height = this._cellsPresenterResizeHeight;
			}
		}

		// Token: 0x060066B5 RID: 26293 RVA: 0x002B2B1C File Offset: 0x002B1B1C
		internal void OnRowResizeReset()
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.ClearValue(FrameworkElement.HeightProperty);
				if (this._owner != null)
				{
					this._owner.ItemAttachedStorage.ClearValue(this.Item, FrameworkElement.HeightProperty);
				}
			}
		}

		// Token: 0x060066B6 RID: 26294 RVA: 0x002B2B64 File Offset: 0x002B1B64
		protected internal virtual void OnColumnsChanged(ObservableCollection<DataGridColumn> columns, NotifyCollectionChangedEventArgs e)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.OnColumnsChanged(columns, e);
			}
		}

		// Token: 0x060066B7 RID: 26295 RVA: 0x002B2B84 File Offset: 0x002B1B84
		private static object OnCoerceHeaderStyle(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.HeaderStyleProperty, dataGridRow.DataGridOwner, DataGrid.RowHeaderStyleProperty);
		}

		// Token: 0x060066B8 RID: 26296 RVA: 0x002B2BB0 File Offset: 0x002B1BB0
		private static object OnCoerceHeaderTemplate(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.HeaderTemplateProperty, dataGridRow.DataGridOwner, DataGrid.RowHeaderTemplateProperty);
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x002B2BDC File Offset: 0x002B1BDC
		private static object OnCoerceHeaderTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.HeaderTemplateSelectorProperty, dataGridRow.DataGridOwner, DataGrid.RowHeaderTemplateSelectorProperty);
		}

		// Token: 0x060066BA RID: 26298 RVA: 0x002B2C08 File Offset: 0x002B1C08
		private static object OnCoerceBackground(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			object result = baseValue;
			int alternationIndex = dataGridRow.AlternationIndex;
			if (alternationIndex != 0)
			{
				if (alternationIndex == 1)
				{
					result = DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, Control.BackgroundProperty, dataGridRow.DataGridOwner, DataGrid.AlternatingRowBackgroundProperty);
				}
			}
			else
			{
				result = DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, Control.BackgroundProperty, dataGridRow.DataGridOwner, DataGrid.RowBackgroundProperty);
			}
			return result;
		}

		// Token: 0x060066BB RID: 26299 RVA: 0x002B2C64 File Offset: 0x002B1C64
		private static object OnCoerceValidationErrorTemplate(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.ValidationErrorTemplateProperty, dataGridRow.DataGridOwner, DataGrid.RowValidationErrorTemplateProperty);
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x002B2C90 File Offset: 0x002B1C90
		private static object OnCoerceDetailsTemplate(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.DetailsTemplateProperty, dataGridRow.DataGridOwner, DataGrid.RowDetailsTemplateProperty);
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x002B2CBC File Offset: 0x002B1CBC
		private static object OnCoerceDetailsTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.DetailsTemplateSelectorProperty, dataGridRow.DataGridOwner, DataGrid.RowDetailsTemplateSelectorProperty);
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x002B2CE8 File Offset: 0x002B1CE8
		private static object OnCoerceDetailsVisibility(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			object obj = DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.DetailsVisibilityProperty, dataGridRow.DataGridOwner, DataGrid.RowDetailsVisibilityModeProperty);
			if (obj is DataGridRowDetailsVisibilityMode)
			{
				DataGridRowDetailsVisibilityMode dataGridRowDetailsVisibilityMode = (DataGridRowDetailsVisibilityMode)obj;
				bool flag = dataGridRow.DetailsTemplate != null || dataGridRow.DetailsTemplateSelector != null;
				bool flag2 = dataGridRow.Item != CollectionView.NewItemPlaceholder;
				switch (dataGridRowDetailsVisibilityMode)
				{
				case DataGridRowDetailsVisibilityMode.Collapsed:
					obj = Visibility.Collapsed;
					break;
				case DataGridRowDetailsVisibilityMode.Visible:
					obj = ((flag && flag2) ? Visibility.Visible : Visibility.Collapsed);
					break;
				case DataGridRowDetailsVisibilityMode.VisibleWhenSelected:
					obj = ((dataGridRow.IsSelected && flag && flag2) ? Visibility.Visible : Visibility.Collapsed);
					break;
				default:
					obj = Visibility.Collapsed;
					break;
				}
			}
			return obj;
		}

		// Token: 0x060066BF RID: 26303 RVA: 0x002B2D9C File Offset: 0x002B1D9C
		private static object OnCoerceVisibility(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			DataGrid dataGridOwner = dataGridRow.DataGridOwner;
			if (dataGridRow.Item == CollectionView.NewItemPlaceholder && dataGridOwner != null)
			{
				return dataGridOwner.PlaceholderVisibility;
			}
			return baseValue;
		}

		// Token: 0x060066C0 RID: 26304 RVA: 0x002B2DD2 File Offset: 0x002B1DD2
		private static object OnCoerceShouldCacheContainerSize(DependencyObject d, object baseValue)
		{
			if (((DataGridRow)d).Item == CollectionView.NewItemPlaceholder)
			{
				return false;
			}
			return baseValue;
		}

		// Token: 0x060066C1 RID: 26305 RVA: 0x002B2DEE File Offset: 0x002B1DEE
		private static void OnNotifyRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DataGridRow).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Rows);
		}

		// Token: 0x060066C2 RID: 26306 RVA: 0x002B2E02 File Offset: 0x002B1E02
		private static void OnNotifyRowAndRowHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DataGridRow).NotifyPropertyChanged(d, e, DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
		}

		// Token: 0x060066C3 RID: 26307 RVA: 0x002B2E18 File Offset: 0x002B1E18
		private static void OnNotifyDetailsTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			dataGridRow.NotifyPropertyChanged(dataGridRow, e, DataGridNotificationTarget.DetailsPresenter | DataGridNotificationTarget.Rows);
			if (dataGridRow.DetailsLoaded && d.GetValue(e.Property) == e.NewValue)
			{
				if (dataGridRow.DataGridOwner != null)
				{
					dataGridRow.DataGridOwner.OnUnloadingRowDetailsWrapper(dataGridRow);
				}
				if (e.NewValue != null)
				{
					Dispatcher.CurrentDispatcher.BeginInvoke(new DispatcherOperationCallback(DataGrid.DelayedOnLoadingRowDetails), DispatcherPriority.Loaded, new object[]
					{
						dataGridRow
					});
				}
			}
		}

		// Token: 0x060066C4 RID: 26308 RVA: 0x002B2E98 File Offset: 0x002B1E98
		private static void OnNotifyDetailsVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			Dispatcher.CurrentDispatcher.BeginInvoke(new DispatcherOperationCallback(DataGridRow.DelayedRowDetailsVisibilityChanged), DispatcherPriority.Loaded, new object[]
			{
				dataGridRow
			});
			dataGridRow.NotifyPropertyChanged(d, e, DataGridNotificationTarget.DetailsPresenter | DataGridNotificationTarget.Rows);
		}

		// Token: 0x060066C5 RID: 26309 RVA: 0x002B2EDC File Offset: 0x002B1EDC
		private static object DelayedRowDetailsVisibilityChanged(object arg)
		{
			DataGridRow dataGridRow = (DataGridRow)arg;
			DataGrid dataGridOwner = dataGridRow.DataGridOwner;
			FrameworkElement detailsElement = (dataGridRow.DetailsPresenter != null) ? dataGridRow.DetailsPresenter.DetailsElement : null;
			if (dataGridOwner != null)
			{
				DataGridRowDetailsEventArgs e = new DataGridRowDetailsEventArgs(dataGridRow, detailsElement);
				dataGridOwner.OnRowDetailsVisibilityChanged(e);
			}
			return null;
		}

		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x060066C6 RID: 26310 RVA: 0x002B2F21 File Offset: 0x002B1F21
		// (set) Token: 0x060066C7 RID: 26311 RVA: 0x002B2F29 File Offset: 0x002B1F29
		internal DataGridCellsPresenter CellsPresenter
		{
			get
			{
				return this._cellsPresenter;
			}
			set
			{
				this._cellsPresenter = value;
			}
		}

		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x060066C8 RID: 26312 RVA: 0x002B2F32 File Offset: 0x002B1F32
		// (set) Token: 0x060066C9 RID: 26313 RVA: 0x002B2F3A File Offset: 0x002B1F3A
		internal DataGridDetailsPresenter DetailsPresenter
		{
			get
			{
				return this._detailsPresenter;
			}
			set
			{
				this._detailsPresenter = value;
			}
		}

		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x060066CA RID: 26314 RVA: 0x002B2F43 File Offset: 0x002B1F43
		// (set) Token: 0x060066CB RID: 26315 RVA: 0x002B2F4B File Offset: 0x002B1F4B
		internal DataGridRowHeader RowHeader
		{
			get
			{
				return this._rowHeader;
			}
			set
			{
				this._rowHeader = value;
			}
		}

		// Token: 0x060066CC RID: 26316 RVA: 0x002B2F54 File Offset: 0x002B1F54
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			this.NotifyPropertyChanged(d, string.Empty, e, target);
		}

		// Token: 0x060066CD RID: 26317 RVA: 0x002B2F64 File Offset: 0x002B1F64
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyRows(target))
			{
				if (e.Property == DataGrid.RowBackgroundProperty || e.Property == DataGrid.AlternatingRowBackgroundProperty || e.Property == Control.BackgroundProperty || e.Property == DataGridRow.AlternationIndexProperty)
				{
					DataGridHelper.TransferProperty(this, Control.BackgroundProperty);
				}
				else if (e.Property == DataGrid.RowHeaderStyleProperty || e.Property == DataGridRow.HeaderStyleProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.HeaderStyleProperty);
				}
				else if (e.Property == DataGrid.RowHeaderTemplateProperty || e.Property == DataGridRow.HeaderTemplateProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.HeaderTemplateProperty);
				}
				else if (e.Property == DataGrid.RowHeaderTemplateSelectorProperty || e.Property == DataGridRow.HeaderTemplateSelectorProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.HeaderTemplateSelectorProperty);
				}
				else if (e.Property == DataGrid.RowValidationErrorTemplateProperty || e.Property == DataGridRow.ValidationErrorTemplateProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.ValidationErrorTemplateProperty);
				}
				else if (e.Property == DataGrid.RowDetailsTemplateProperty || e.Property == DataGridRow.DetailsTemplateProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsTemplateProperty);
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsVisibilityProperty);
				}
				else if (e.Property == DataGrid.RowDetailsTemplateSelectorProperty || e.Property == DataGridRow.DetailsTemplateSelectorProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsTemplateSelectorProperty);
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsVisibilityProperty);
				}
				else if (e.Property == DataGrid.RowDetailsVisibilityModeProperty || e.Property == DataGridRow.DetailsVisibilityProperty || e.Property == DataGridRow.IsSelectedProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsVisibilityProperty);
				}
				else if (e.Property == DataGridRow.ItemProperty)
				{
					this.OnItemChanged(e.OldValue, e.NewValue);
				}
				else if (e.Property == DataGridRow.HeaderProperty)
				{
					this.OnHeaderChanged(e.OldValue, e.NewValue);
				}
				else if (e.Property == FrameworkElement.BindingGroupProperty)
				{
					base.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.DelayedValidateWithoutUpdate), DispatcherPriority.DataBind, new object[]
					{
						e.NewValue
					});
				}
				else if (e.Property == DataGridRow.IsEditingProperty || e.Property == UIElement.IsMouseOverProperty || e.Property == UIElement.IsKeyboardFocusWithinProperty)
				{
					base.UpdateVisualState();
				}
			}
			if (DataGridHelper.ShouldNotifyDetailsPresenter(target) && this.DetailsPresenter != null)
			{
				this.DetailsPresenter.NotifyPropertyChanged(d, e);
			}
			if (DataGridHelper.ShouldNotifyCellsPresenter(target) || DataGridHelper.ShouldNotifyCells(target) || DataGridHelper.ShouldRefreshCellContent(target))
			{
				DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
				if (cellsPresenter != null)
				{
					cellsPresenter.NotifyPropertyChanged(d, propertyName, e, target);
				}
			}
			if (DataGridHelper.ShouldNotifyRowHeaders(target) && this.RowHeader != null)
			{
				this.RowHeader.NotifyPropertyChanged(d, e);
			}
		}

		// Token: 0x060066CE RID: 26318 RVA: 0x002B3240 File Offset: 0x002B2240
		private object DelayedValidateWithoutUpdate(object arg)
		{
			BindingGroup bindingGroup = (BindingGroup)arg;
			if (bindingGroup != null && bindingGroup.Items.Count > 0)
			{
				bindingGroup.ValidateWithoutUpdate();
			}
			return null;
		}

		// Token: 0x060066CF RID: 26319 RVA: 0x002B3270 File Offset: 0x002B2270
		private void SyncProperties(bool forcePrepareCells)
		{
			DataGridHelper.TransferProperty(this, Control.BackgroundProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.HeaderStyleProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.HeaderTemplateProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.HeaderTemplateSelectorProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.ValidationErrorTemplateProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.DetailsTemplateProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.DetailsTemplateSelectorProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.DetailsVisibilityProperty);
			base.CoerceValue(UIElement.VisibilityProperty);
			this.RestoreAttachedItemValue(this, DataGridRow.DetailsVisibilityProperty);
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.SyncProperties(forcePrepareCells);
				this.RestoreAttachedItemValue(cellsPresenter, FrameworkElement.HeightProperty);
			}
			if (this.DetailsPresenter != null)
			{
				this.DetailsPresenter.SyncProperties();
			}
			if (this.RowHeader != null)
			{
				this.RowHeader.SyncProperties();
			}
		}

		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x060066D0 RID: 26320 RVA: 0x002B332F File Offset: 0x002B232F
		public int AlternationIndex
		{
			get
			{
				return (int)base.GetValue(DataGridRow.AlternationIndexProperty);
			}
		}

		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x060066D1 RID: 26321 RVA: 0x002B3341 File Offset: 0x002B2341
		// (set) Token: 0x060066D2 RID: 26322 RVA: 0x002B3353 File Offset: 0x002B2353
		[Category("Appearance")]
		[Bindable(true)]
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(DataGridRow.IsSelectedProperty);
			}
			set
			{
				base.SetValue(DataGridRow.IsSelectedProperty, value);
			}
		}

		// Token: 0x060066D3 RID: 26323 RVA: 0x002B3364 File Offset: 0x002B2364
		private static void OnIsSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			DataGridRow dataGridRow = (DataGridRow)sender;
			bool flag = (bool)e.NewValue;
			if (flag && !dataGridRow.IsSelectable)
			{
				throw new InvalidOperationException(SR.Get("DataGridRow_CannotSelectRowWhenCells"));
			}
			DataGrid dataGridOwner = dataGridRow.DataGridOwner;
			if (dataGridOwner != null && dataGridRow.DataContext != null)
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(dataGridOwner) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					DataGridItemAutomationPeer dataGridItemAutomationPeer = dataGridAutomationPeer.FindOrCreateItemAutomationPeer(dataGridRow.DataContext) as DataGridItemAutomationPeer;
					if (dataGridItemAutomationPeer != null)
					{
						dataGridItemAutomationPeer.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, (bool)e.OldValue, flag);
					}
				}
			}
			dataGridRow.NotifyPropertyChanged(dataGridRow, e, DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
			dataGridRow.RaiseSelectionChangedEvent(flag);
			dataGridRow.UpdateVisualState();
			dataGridRow.NotifyPropertyChanged(dataGridRow, e, DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
		}

		// Token: 0x060066D4 RID: 26324 RVA: 0x002B3422 File Offset: 0x002B2422
		private void RaiseSelectionChangedEvent(bool isSelected)
		{
			if (isSelected)
			{
				this.OnSelected(new RoutedEventArgs(DataGridRow.SelectedEvent, this));
				return;
			}
			this.OnUnselected(new RoutedEventArgs(DataGridRow.UnselectedEvent, this));
		}

		// Token: 0x14000108 RID: 264
		// (add) Token: 0x060066D5 RID: 26325 RVA: 0x002B344A File Offset: 0x002B244A
		// (remove) Token: 0x060066D6 RID: 26326 RVA: 0x002B3458 File Offset: 0x002B2458
		public event RoutedEventHandler Selected
		{
			add
			{
				base.AddHandler(DataGridRow.SelectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DataGridRow.SelectedEvent, value);
			}
		}

		// Token: 0x060066D7 RID: 26327 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x14000109 RID: 265
		// (add) Token: 0x060066D8 RID: 26328 RVA: 0x002B3466 File Offset: 0x002B2466
		// (remove) Token: 0x060066D9 RID: 26329 RVA: 0x002B3474 File Offset: 0x002B2474
		public event RoutedEventHandler Unselected
		{
			add
			{
				base.AddHandler(DataGridRow.UnselectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DataGridRow.UnselectedEvent, value);
			}
		}

		// Token: 0x060066DA RID: 26330 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x060066DB RID: 26331 RVA: 0x002B3484 File Offset: 0x002B2484
		private bool IsSelectable
		{
			get
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				if (dataGridOwner != null)
				{
					DataGridSelectionUnit selectionUnit = dataGridOwner.SelectionUnit;
					return selectionUnit == DataGridSelectionUnit.FullRow || selectionUnit == DataGridSelectionUnit.CellOrRowHeader;
				}
				return true;
			}
		}

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x060066DC RID: 26332 RVA: 0x002B34AE File Offset: 0x002B24AE
		// (set) Token: 0x060066DD RID: 26333 RVA: 0x002B34C0 File Offset: 0x002B24C0
		public bool IsEditing
		{
			get
			{
				return (bool)base.GetValue(DataGridRow.IsEditingProperty);
			}
			internal set
			{
				base.SetValue(DataGridRow.IsEditingPropertyKey, value);
			}
		}

		// Token: 0x060066DE RID: 26334 RVA: 0x002B34CE File Offset: 0x002B24CE
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridRowAutomationPeer(this);
		}

		// Token: 0x060066DF RID: 26335 RVA: 0x002B34D8 File Offset: 0x002B24D8
		internal void ScrollCellIntoView(int index)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.ScrollCellIntoView(index);
			}
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x002B34F8 File Offset: 0x002B24F8
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner != null)
			{
				dataGridOwner.QueueInvalidateCellsPanelHorizontalOffset();
			}
			return base.ArrangeOverride(arrangeBounds);
		}

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x060066E1 RID: 26337 RVA: 0x002B351C File Offset: 0x002B251C
		// (set) Token: 0x060066E2 RID: 26338 RVA: 0x002B352E File Offset: 0x002B252E
		public bool IsNewItem
		{
			get
			{
				return (bool)base.GetValue(DataGridRow.IsNewItemProperty);
			}
			internal set
			{
				base.SetValue(DataGridRow.IsNewItemPropertyKey, value);
			}
		}

		// Token: 0x060066E3 RID: 26339 RVA: 0x002B353C File Offset: 0x002B253C
		public int GetIndex()
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner != null)
			{
				return dataGridOwner.ItemContainerGenerator.IndexFromContainer(this);
			}
			return -1;
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x002B3561 File Offset: 0x002B2561
		public static DataGridRow GetRowContainingElement(FrameworkElement element)
		{
			return DataGridHelper.FindVisualParent<DataGridRow>(element);
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x060066E5 RID: 26341 RVA: 0x002B3569 File Offset: 0x002B2569
		internal DataGrid DataGridOwner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x060066E6 RID: 26342 RVA: 0x002B3571 File Offset: 0x002B2571
		internal bool DetailsPresenterDrawsGridLines
		{
			get
			{
				return this._detailsPresenter != null && this._detailsPresenter.Visibility == Visibility.Visible;
			}
		}

		// Token: 0x060066E7 RID: 26343 RVA: 0x002B358C File Offset: 0x002B258C
		internal DataGridCell TryGetCell(int index)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				return cellsPresenter.ItemContainerGenerator.ContainerFromIndex(index) as DataGridCell;
			}
			return null;
		}

		// Token: 0x040033E4 RID: 13284
		private const byte DATAGRIDROW_stateMouseOverCode = 0;

		// Token: 0x040033E5 RID: 13285
		private const byte DATAGRIDROW_stateMouseOverEditingCode = 1;

		// Token: 0x040033E6 RID: 13286
		private const byte DATAGRIDROW_stateMouseOverEditingFocusedCode = 2;

		// Token: 0x040033E7 RID: 13287
		private const byte DATAGRIDROW_stateMouseOverSelectedCode = 3;

		// Token: 0x040033E8 RID: 13288
		private const byte DATAGRIDROW_stateMouseOverSelectedFocusedCode = 4;

		// Token: 0x040033E9 RID: 13289
		private const byte DATAGRIDROW_stateNormalCode = 5;

		// Token: 0x040033EA RID: 13290
		private const byte DATAGRIDROW_stateNormalEditingCode = 6;

		// Token: 0x040033EB RID: 13291
		private const byte DATAGRIDROW_stateNormalEditingFocusedCode = 7;

		// Token: 0x040033EC RID: 13292
		private const byte DATAGRIDROW_stateSelectedCode = 8;

		// Token: 0x040033ED RID: 13293
		private const byte DATAGRIDROW_stateSelectedFocusedCode = 9;

		// Token: 0x040033EE RID: 13294
		private const byte DATAGRIDROW_stateNullCode = 255;

		// Token: 0x040033EF RID: 13295
		private static byte[] _idealStateMapping = new byte[]
		{
			5,
			5,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			8,
			9,
			3,
			4,
			6,
			7,
			1,
			2
		};

		// Token: 0x040033F0 RID: 13296
		private static byte[] _fallbackStateMapping = new byte[]
		{
			5,
			2,
			7,
			4,
			9,
			byte.MaxValue,
			7,
			9,
			9,
			5
		};

		// Token: 0x040033F1 RID: 13297
		private static string[] _stateNames = new string[]
		{
			"MouseOver",
			"MouseOver_Unfocused_Editing",
			"MouseOver_Editing",
			"MouseOver_Unfocused_Selected",
			"MouseOver_Selected",
			"Normal",
			"Unfocused_Editing",
			"Normal_Editing",
			"Unfocused_Selected",
			"Normal_Selected"
		};

		// Token: 0x040033F2 RID: 13298
		public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(object), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowPropertyChanged)));

		// Token: 0x040033F3 RID: 13299
		public static readonly DependencyProperty ItemsPanelProperty = ItemsControl.ItemsPanelProperty.AddOwner(typeof(DataGridRow));

		// Token: 0x040033F4 RID: 13300
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged)));

		// Token: 0x040033F5 RID: 13301
		public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle", typeof(Style), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceHeaderStyle)));

		// Token: 0x040033F6 RID: 13302
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceHeaderTemplate)));

		// Token: 0x040033F7 RID: 13303
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register("HeaderTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceHeaderTemplateSelector)));

		// Token: 0x040033F8 RID: 13304
		public static readonly DependencyProperty ValidationErrorTemplateProperty = DependencyProperty.Register("ValidationErrorTemplate", typeof(ControlTemplate), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceValidationErrorTemplate)));

		// Token: 0x040033F9 RID: 13305
		public static readonly DependencyProperty DetailsTemplateProperty = DependencyProperty.Register("DetailsTemplate", typeof(DataTemplate), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyDetailsTemplatePropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceDetailsTemplate)));

		// Token: 0x040033FA RID: 13306
		public static readonly DependencyProperty DetailsTemplateSelectorProperty = DependencyProperty.Register("DetailsTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyDetailsTemplatePropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceDetailsTemplateSelector)));

		// Token: 0x040033FB RID: 13307
		public static readonly DependencyProperty DetailsVisibilityProperty = DependencyProperty.Register("DetailsVisibility", typeof(Visibility), typeof(DataGridRow), new FrameworkPropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(DataGridRow.OnNotifyDetailsVisibilityChanged), new CoerceValueCallback(DataGridRow.OnCoerceDetailsVisibility)));

		// Token: 0x040033FC RID: 13308
		public static readonly DependencyProperty AlternationIndexProperty = ItemsControl.AlternationIndexProperty.AddOwner(typeof(DataGridRow));

		// Token: 0x040033FD RID: 13309
		public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(DataGridRow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(DataGridRow.OnIsSelectedChanged)));

		// Token: 0x04003400 RID: 13312
		private static readonly DependencyPropertyKey IsEditingPropertyKey;

		// Token: 0x04003401 RID: 13313
		public static readonly DependencyProperty IsEditingProperty;

		// Token: 0x04003402 RID: 13314
		internal static readonly DependencyPropertyKey IsNewItemPropertyKey;

		// Token: 0x04003403 RID: 13315
		public static readonly DependencyProperty IsNewItemProperty;

		// Token: 0x04003404 RID: 13316
		internal bool _detailsLoaded;

		// Token: 0x04003405 RID: 13317
		private DataGrid _owner;

		// Token: 0x04003406 RID: 13318
		private DataGridCellsPresenter _cellsPresenter;

		// Token: 0x04003407 RID: 13319
		private DataGridDetailsPresenter _detailsPresenter;

		// Token: 0x04003408 RID: 13320
		private DataGridRowHeader _rowHeader;

		// Token: 0x04003409 RID: 13321
		private ContainerTracking<DataGridRow> _tracker;

		// Token: 0x0400340A RID: 13322
		private double _cellsPresenterResizeHeight;
	}
}
