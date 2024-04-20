using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x0200073E RID: 1854
	public class DataGrid : MultiSelector
	{
		// Token: 0x0600624B RID: 25163 RVA: 0x0029F0A4 File Offset: 0x0029E0A4
		static DataGrid()
		{
			Type typeFromHandle = typeof(DataGrid);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(typeof(DataGrid)));
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(DataGridRowsPresenter));
			frameworkElementFactory.SetValue(FrameworkElement.NameProperty, "PART_RowsPresenter");
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new ItemsPanelTemplate(frameworkElementFactory)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(true, null, new CoerceValueCallback(DataGrid.OnCoerceIsVirtualizingProperty)));
			VirtualizingPanel.VirtualizationModeProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(VirtualizationMode.Recycling));
			ItemsControl.ItemContainerStyleProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGrid.OnCoerceItemContainerStyle)));
			ItemsControl.ItemContainerStyleSelectorProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGrid.OnCoerceItemContainerStyleSelector)));
			ItemsControl.ItemsSourceProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGrid.OnCoerceItemsSourceProperty)));
			ItemsControl.AlternationCountProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(0, null, new CoerceValueCallback(DataGrid.OnCoerceAlternationCount)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGrid.OnIsEnabledChanged)));
			UIElement.IsKeyboardFocusWithinPropertyKey.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGrid.OnIsKeyboardFocusWithinChanged)));
			Selector.IsSynchronizedWithCurrentItemProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGrid.OnCoerceIsSynchronizedWithCurrentItem)));
			Control.IsTabStopProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(false));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			CommandManager.RegisterClassInputBinding(typeFromHandle, new InputBinding(DataGrid.BeginEditCommand, new KeyGesture(Key.F2)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.BeginEditCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedBeginEdit), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteBeginEdit)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.CommitEditCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedCommitEdit), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteCommitEdit)));
			CommandManager.RegisterClassInputBinding(typeFromHandle, new InputBinding(DataGrid.CancelEditCommand, new KeyGesture(Key.Escape)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.CancelEditCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedCancelEdit), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteCancelEdit)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.SelectAllCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedSelectAll), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteSelectAll)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.DeleteCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedDelete), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteDelete)));
			CommandManager.RegisterClassCommandBinding(typeof(DataGrid), new CommandBinding(ApplicationCommands.Copy, new ExecutedRoutedEventHandler(DataGrid.OnExecutedCopy), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteCopy)));
			EventManager.RegisterClassHandler(typeof(DataGrid), UIElement.MouseUpEvent, new MouseButtonEventHandler(DataGrid.OnAnyMouseUpThunk), true);
			ControlsTraceLogger.AddControl(TelemetryControls.DataGrid);
		}

		// Token: 0x0600624C RID: 25164 RVA: 0x002A0010 File Offset: 0x0029F010
		public DataGrid()
		{
			this._columns = new DataGridColumnCollection(this);
			this._columns.CollectionChanged += this.OnColumnsChanged;
			this._rowValidationRules = new ObservableCollection<ValidationRule>();
			this._rowValidationRules.CollectionChanged += this.OnRowValidationRulesChanged;
			this._selectedCells = new SelectedCellsCollection(this);
			((INotifyCollectionChanged)base.Items).CollectionChanged += this.OnItemsCollectionChanged;
			((INotifyCollectionChanged)base.Items.SortDescriptions).CollectionChanged += this.OnItemsSortDescriptionsChanged;
			base.Items.GroupDescriptions.CollectionChanged += this.OnItemsGroupDescriptionsChanged;
			this.InternalColumns.InvalidateColumnWidthsComputation();
			this.CellsPanelHorizontalOffsetComputationPending = false;
		}

		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x0600624D RID: 25165 RVA: 0x002A00F9 File Offset: 0x0029F0F9
		public ObservableCollection<DataGridColumn> Columns
		{
			get
			{
				return this._columns;
			}
		}

		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x0600624E RID: 25166 RVA: 0x002A00F9 File Offset: 0x0029F0F9
		internal DataGridColumnCollection InternalColumns
		{
			get
			{
				return this._columns;
			}
		}

		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x0600624F RID: 25167 RVA: 0x002A0101 File Offset: 0x0029F101
		// (set) Token: 0x06006250 RID: 25168 RVA: 0x002A0113 File Offset: 0x0029F113
		public bool CanUserResizeColumns
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserResizeColumnsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserResizeColumnsProperty, value);
			}
		}

		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x06006251 RID: 25169 RVA: 0x002A0121 File Offset: 0x0029F121
		// (set) Token: 0x06006252 RID: 25170 RVA: 0x002A0133 File Offset: 0x0029F133
		public DataGridLength ColumnWidth
		{
			get
			{
				return (DataGridLength)base.GetValue(DataGrid.ColumnWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.ColumnWidthProperty, value);
			}
		}

		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x06006253 RID: 25171 RVA: 0x002A0146 File Offset: 0x0029F146
		// (set) Token: 0x06006254 RID: 25172 RVA: 0x002A0158 File Offset: 0x0029F158
		public double MinColumnWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.MinColumnWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.MinColumnWidthProperty, value);
			}
		}

		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06006255 RID: 25173 RVA: 0x002A016B File Offset: 0x0029F16B
		// (set) Token: 0x06006256 RID: 25174 RVA: 0x002A017D File Offset: 0x0029F17D
		public double MaxColumnWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.MaxColumnWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.MaxColumnWidthProperty, value);
			}
		}

		// Token: 0x06006257 RID: 25175 RVA: 0x002A0190 File Offset: 0x0029F190
		private static void OnColumnSizeConstraintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
		}

		// Token: 0x06006258 RID: 25176 RVA: 0x002A01A0 File Offset: 0x0029F1A0
		private static bool ValidateMinColumnWidth(object v)
		{
			double num = (double)v;
			return num >= 0.0 && !DoubleUtil.IsNaN(num) && !double.IsPositiveInfinity(num);
		}

		// Token: 0x06006259 RID: 25177 RVA: 0x002A01D4 File Offset: 0x0029F1D4
		private static bool ValidateMaxColumnWidth(object v)
		{
			double num = (double)v;
			return num >= 0.0 && !DoubleUtil.IsNaN(num);
		}

		// Token: 0x0600625A RID: 25178 RVA: 0x002A0200 File Offset: 0x0029F200
		private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.UpdateDataGridReference(e.NewItems, false);
				DataGrid.UpdateColumnSizeConstraints(e.NewItems);
				break;
			case NotifyCollectionChangedAction.Remove:
				this.UpdateDataGridReference(e.OldItems, true);
				break;
			case NotifyCollectionChangedAction.Replace:
				this.UpdateDataGridReference(e.OldItems, true);
				this.UpdateDataGridReference(e.NewItems, false);
				DataGrid.UpdateColumnSizeConstraints(e.NewItems);
				break;
			case NotifyCollectionChangedAction.Reset:
				this._selectedCells.Clear();
				break;
			}
			if (this.InternalColumns.DisplayIndexMapInitialized)
			{
				base.CoerceValue(DataGrid.FrozenColumnCountProperty);
			}
			bool flag = DataGrid.HasVisibleColumns(e.OldItems) | DataGrid.HasVisibleColumns(e.NewItems) | e.Action == NotifyCollectionChangedAction.Reset;
			if (flag)
			{
				this.InternalColumns.InvalidateColumnRealization(true);
			}
			this.UpdateColumnsOnRows(e);
			if (flag && e.Action != NotifyCollectionChangedAction.Move)
			{
				this.InternalColumns.InvalidateColumnWidthsComputation();
			}
		}

		// Token: 0x0600625B RID: 25179 RVA: 0x002A02F0 File Offset: 0x0029F2F0
		internal void UpdateDataGridReference(IList list, bool clear)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridColumn dataGridColumn = (DataGridColumn)list[i];
				if (clear)
				{
					if (dataGridColumn.DataGridOwner == this)
					{
						dataGridColumn.DataGridOwner = null;
					}
				}
				else
				{
					if (dataGridColumn.DataGridOwner != null && dataGridColumn.DataGridOwner != this)
					{
						dataGridColumn.DataGridOwner.Columns.Remove(dataGridColumn);
					}
					dataGridColumn.DataGridOwner = this;
				}
			}
		}

		// Token: 0x0600625C RID: 25180 RVA: 0x002A035C File Offset: 0x0029F35C
		private static void UpdateColumnSizeConstraints(IList list)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				((DataGridColumn)list[i]).SyncProperties();
			}
		}

		// Token: 0x0600625D RID: 25181 RVA: 0x002A0390 File Offset: 0x0029F390
		private static bool HasVisibleColumns(IList columns)
		{
			if (columns != null && columns.Count > 0)
			{
				using (IEnumerator enumerator = columns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((DataGridColumn)enumerator.Current).IsVisible)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600625E RID: 25182 RVA: 0x002A03F8 File Offset: 0x0029F3F8
		internal bool RetryBringColumnIntoView(bool retryRequested)
		{
			if (retryRequested)
			{
				int value = DataGrid.BringColumnIntoViewRetryCountField.GetValue(this);
				if (value < 4)
				{
					DataGrid.BringColumnIntoViewRetryCountField.SetValue(this, value + 1);
					return true;
				}
			}
			DataGrid.BringColumnIntoViewRetryCountField.ClearValue(this);
			return false;
		}

		// Token: 0x0600625F RID: 25183 RVA: 0x002A0434 File Offset: 0x0029F434
		public DataGridColumn ColumnFromDisplayIndex(int displayIndex)
		{
			if (displayIndex < 0 || displayIndex >= this.Columns.Count)
			{
				throw new ArgumentOutOfRangeException("displayIndex", displayIndex, SR.Get("DataGrid_DisplayIndexOutOfRange"));
			}
			return this.InternalColumns.ColumnFromDisplayIndex(displayIndex);
		}

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x06006260 RID: 25184 RVA: 0x002A0470 File Offset: 0x0029F470
		// (remove) Token: 0x06006261 RID: 25185 RVA: 0x002A04A8 File Offset: 0x0029F4A8
		public event EventHandler<DataGridColumnEventArgs> ColumnDisplayIndexChanged;

		// Token: 0x06006262 RID: 25186 RVA: 0x002A04DD File Offset: 0x0029F4DD
		protected internal virtual void OnColumnDisplayIndexChanged(DataGridColumnEventArgs e)
		{
			if (this.ColumnDisplayIndexChanged != null)
			{
				this.ColumnDisplayIndexChanged(this, e);
			}
		}

		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x06006263 RID: 25187 RVA: 0x002A04F4 File Offset: 0x0029F4F4
		internal List<int> DisplayIndexMap
		{
			get
			{
				return this.InternalColumns.DisplayIndexMap;
			}
		}

		// Token: 0x06006264 RID: 25188 RVA: 0x002A0501 File Offset: 0x0029F501
		internal void ValidateDisplayIndex(DataGridColumn column, int displayIndex)
		{
			this.InternalColumns.ValidateDisplayIndex(column, displayIndex);
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x002A0510 File Offset: 0x0029F510
		internal int ColumnIndexFromDisplayIndex(int displayIndex)
		{
			if (displayIndex >= 0 && displayIndex < this.DisplayIndexMap.Count)
			{
				return this.DisplayIndexMap[displayIndex];
			}
			return -1;
		}

		// Token: 0x06006266 RID: 25190 RVA: 0x002A0534 File Offset: 0x0029F534
		internal DataGridColumnHeader ColumnHeaderFromDisplayIndex(int displayIndex)
		{
			int num = this.ColumnIndexFromDisplayIndex(displayIndex);
			if (num != -1 && this.ColumnHeadersPresenter != null && this.ColumnHeadersPresenter.ItemContainerGenerator != null)
			{
				return (DataGridColumnHeader)this.ColumnHeadersPresenter.ItemContainerGenerator.ContainerFromIndex(num);
			}
			return null;
		}

		// Token: 0x06006267 RID: 25191 RVA: 0x002A057A File Offset: 0x0029F57A
		private static void OnNotifyCellsPresenterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter);
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x002A058A File Offset: 0x0029F58A
		private static void OnNotifyColumnAndCellPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.Columns);
		}

		// Token: 0x06006269 RID: 25193 RVA: 0x002A0190 File Offset: 0x0029F190
		private static void OnNotifyColumnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
		}

		// Token: 0x0600626A RID: 25194 RVA: 0x002A059A File Offset: 0x0029F59A
		private static void OnNotifyColumnAndColumnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns | DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x0600626B RID: 25195 RVA: 0x002A05AB File Offset: 0x0029F5AB
		private static void OnNotifyColumnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x0600626C RID: 25196 RVA: 0x002A05BC File Offset: 0x0029F5BC
		private static void OnNotifyHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders | DataGridNotificationTarget.RowHeaders);
		}

		// Token: 0x0600626D RID: 25197 RVA: 0x002A05D0 File Offset: 0x0029F5D0
		private static void OnNotifyDataGridAndRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.DataGrid | DataGridNotificationTarget.Rows);
		}

		// Token: 0x0600626E RID: 25198 RVA: 0x002A05E4 File Offset: 0x0029F5E4
		private static void OnNotifyGridLinePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				((DataGrid)d).OnItemTemplateChanged(null, null);
			}
		}

		// Token: 0x0600626F RID: 25199 RVA: 0x002A0603 File Offset: 0x0029F603
		private static void OnNotifyRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Rows);
		}

		// Token: 0x06006270 RID: 25200 RVA: 0x002A0617 File Offset: 0x0029F617
		private static void OnNotifyRowHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.RowHeaders);
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x002A062B File Offset: 0x0029F62B
		private static void OnNotifyRowAndRowHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
		}

		// Token: 0x06006272 RID: 25202 RVA: 0x002A063F File Offset: 0x0029F63F
		private static void OnNotifyRowAndDetailsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.DetailsPresenter | DataGridNotificationTarget.Rows);
		}

		// Token: 0x06006273 RID: 25203 RVA: 0x002A0653 File Offset: 0x0029F653
		private static void OnNotifyHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeadersPresenter);
		}

		// Token: 0x06006274 RID: 25204 RVA: 0x002A0664 File Offset: 0x0029F664
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			this.NotifyPropertyChanged(d, string.Empty, e, target);
		}

		// Token: 0x06006275 RID: 25205 RVA: 0x002A0674 File Offset: 0x0029F674
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyDataGrid(target))
			{
				if (e.Property == DataGrid.AlternatingRowBackgroundProperty)
				{
					base.CoerceValue(ItemsControl.AlternationCountProperty);
				}
				else if (e.Property == DataGridColumn.VisibilityProperty || e.Property == DataGridColumn.WidthProperty || e.Property == DataGridColumn.DisplayIndexProperty)
				{
					foreach (object obj in base.ItemContainerGenerator.RecyclableContainers)
					{
						DataGridRow dataGridRow = ((DependencyObject)obj) as DataGridRow;
						if (dataGridRow != null)
						{
							DataGridCellsPresenter cellsPresenter = dataGridRow.CellsPresenter;
							if (cellsPresenter != null)
							{
								cellsPresenter.InvalidateDataGridCellsPanelMeasureAndArrange();
							}
						}
					}
				}
			}
			if (DataGridHelper.ShouldNotifyRowSubtree(target))
			{
				for (ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
				{
					containerTracking.Container.NotifyPropertyChanged(d, propertyName, e, target);
				}
			}
			if (DataGridHelper.ShouldNotifyColumnCollection(target) || DataGridHelper.ShouldNotifyColumns(target))
			{
				this.InternalColumns.NotifyPropertyChanged(d, propertyName, e, target);
			}
			if ((DataGridHelper.ShouldNotifyColumnHeadersPresenter(target) || DataGridHelper.ShouldNotifyColumnHeaders(target)) && this.ColumnHeadersPresenter != null)
			{
				this.ColumnHeadersPresenter.NotifyPropertyChanged(d, propertyName, e, target);
			}
		}

		// Token: 0x06006276 RID: 25206 RVA: 0x002A07B4 File Offset: 0x0029F7B4
		internal void UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction action, int oldDisplayIndex, DataGridColumn oldColumn, int newDisplayIndex)
		{
			using (this.UpdateSelectedCells())
			{
				this._selectedCells.OnColumnsChanged(action, oldDisplayIndex, oldColumn, newDisplayIndex, base.SelectedItems);
			}
		}

		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x06006277 RID: 25207 RVA: 0x002A07FC File Offset: 0x0029F7FC
		// (set) Token: 0x06006278 RID: 25208 RVA: 0x002A0804 File Offset: 0x0029F804
		internal DataGridColumnHeadersPresenter ColumnHeadersPresenter
		{
			get
			{
				return this._columnHeadersPresenter;
			}
			set
			{
				this._columnHeadersPresenter = value;
			}
		}

		// Token: 0x06006279 RID: 25209 RVA: 0x002A080D File Offset: 0x0029F80D
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			this.ColumnHeadersPresenter = null;
		}

		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x0600627A RID: 25210 RVA: 0x002A081E File Offset: 0x0029F81E
		// (set) Token: 0x0600627B RID: 25211 RVA: 0x002A0830 File Offset: 0x0029F830
		public DataGridGridLinesVisibility GridLinesVisibility
		{
			get
			{
				return (DataGridGridLinesVisibility)base.GetValue(DataGrid.GridLinesVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGrid.GridLinesVisibilityProperty, value);
			}
		}

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x0600627C RID: 25212 RVA: 0x002A0843 File Offset: 0x0029F843
		// (set) Token: 0x0600627D RID: 25213 RVA: 0x002A0855 File Offset: 0x0029F855
		public Brush HorizontalGridLinesBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGrid.HorizontalGridLinesBrushProperty);
			}
			set
			{
				base.SetValue(DataGrid.HorizontalGridLinesBrushProperty, value);
			}
		}

		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x0600627E RID: 25214 RVA: 0x002A0863 File Offset: 0x0029F863
		// (set) Token: 0x0600627F RID: 25215 RVA: 0x002A0875 File Offset: 0x0029F875
		public Brush VerticalGridLinesBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGrid.VerticalGridLinesBrushProperty);
			}
			set
			{
				base.SetValue(DataGrid.VerticalGridLinesBrushProperty, value);
			}
		}

		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x06006280 RID: 25216 RVA: 0x002A0883 File Offset: 0x0029F883
		internal double HorizontalGridLineThickness
		{
			get
			{
				return 1.0;
			}
		}

		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x06006281 RID: 25217 RVA: 0x002A0883 File Offset: 0x0029F883
		internal double VerticalGridLineThickness
		{
			get
			{
				return 1.0;
			}
		}

		// Token: 0x06006282 RID: 25218 RVA: 0x002A088E File Offset: 0x0029F88E
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is DataGridRow;
		}

		// Token: 0x06006283 RID: 25219 RVA: 0x002A0899 File Offset: 0x0029F899
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new DataGridRow();
		}

		// Token: 0x06006284 RID: 25220 RVA: 0x002A08A0 File Offset: 0x0029F8A0
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			DataGridRow dataGridRow = (DataGridRow)element;
			if (dataGridRow.DataGridOwner != this)
			{
				dataGridRow.Tracker.StartTracking(ref this._rowTrackingRoot);
				if (item == CollectionView.NewItemPlaceholder || (this.IsAddingNewItem && item == this.EditableItems.CurrentAddItem))
				{
					dataGridRow.IsNewItem = true;
				}
				else
				{
					dataGridRow.ClearValue(DataGridRow.IsNewItemPropertyKey);
				}
				this.EnsureInternalScrollControls();
				this.EnqueueNewItemMarginComputation();
			}
			dataGridRow.PrepareRow(item, this);
			this.OnLoadingRow(new DataGridRowEventArgs(dataGridRow));
		}

		// Token: 0x06006285 RID: 25221 RVA: 0x002A0928 File Offset: 0x0029F928
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			DataGridRow dataGridRow = (DataGridRow)element;
			if (dataGridRow.DataGridOwner == this)
			{
				dataGridRow.Tracker.StopTracking(ref this._rowTrackingRoot);
				dataGridRow.ClearValue(DataGridRow.IsNewItemPropertyKey);
				this.EnqueueNewItemMarginComputation();
			}
			this.OnUnloadingRow(new DataGridRowEventArgs(dataGridRow));
			dataGridRow.ClearRow(this);
		}

		// Token: 0x06006286 RID: 25222 RVA: 0x002A0984 File Offset: 0x0029F984
		private void UpdateColumnsOnRows(NotifyCollectionChangedEventArgs e)
		{
			for (ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
			{
				containerTracking.Container.OnColumnsChanged(this._columns, e);
			}
		}

		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x06006287 RID: 25223 RVA: 0x002A09B6 File Offset: 0x0029F9B6
		// (set) Token: 0x06006288 RID: 25224 RVA: 0x002A09C8 File Offset: 0x0029F9C8
		public Style RowStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.RowStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowStyleProperty, value);
			}
		}

		// Token: 0x06006289 RID: 25225 RVA: 0x002A09D6 File Offset: 0x0029F9D6
		private static void OnRowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(ItemsControl.ItemContainerStyleProperty);
		}

		// Token: 0x0600628A RID: 25226 RVA: 0x002A09E3 File Offset: 0x0029F9E3
		private static object OnCoerceItemContainerStyle(DependencyObject d, object baseValue)
		{
			if (!DataGridHelper.IsDefaultValue(d, DataGrid.RowStyleProperty))
			{
				return d.GetValue(DataGrid.RowStyleProperty);
			}
			return baseValue;
		}

		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x0600628B RID: 25227 RVA: 0x002A09FF File Offset: 0x0029F9FF
		// (set) Token: 0x0600628C RID: 25228 RVA: 0x002A0A11 File Offset: 0x0029FA11
		public ControlTemplate RowValidationErrorTemplate
		{
			get
			{
				return (ControlTemplate)base.GetValue(DataGrid.RowValidationErrorTemplateProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowValidationErrorTemplateProperty, value);
			}
		}

		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x0600628D RID: 25229 RVA: 0x002A0A1F File Offset: 0x0029FA1F
		public ObservableCollection<ValidationRule> RowValidationRules
		{
			get
			{
				return this._rowValidationRules;
			}
		}

		// Token: 0x0600628E RID: 25230 RVA: 0x002A0A28 File Offset: 0x0029FA28
		private void OnRowValidationRulesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.EnsureItemBindingGroup();
			if (this._defaultBindingGroup != null)
			{
				if (base.ItemBindingGroup == this._defaultBindingGroup)
				{
					switch (e.Action)
					{
					case NotifyCollectionChangedAction.Add:
						using (IEnumerator enumerator = e.NewItems.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								ValidationRule item = (ValidationRule)obj;
								this._defaultBindingGroup.ValidationRules.Add(item);
							}
							return;
						}
						break;
					case NotifyCollectionChangedAction.Remove:
						break;
					case NotifyCollectionChangedAction.Replace:
						goto IL_D9;
					case NotifyCollectionChangedAction.Move:
						return;
					case NotifyCollectionChangedAction.Reset:
						goto IL_16A;
					default:
						return;
					}
					using (IEnumerator enumerator = e.OldItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							ValidationRule item2 = (ValidationRule)obj2;
							this._defaultBindingGroup.ValidationRules.Remove(item2);
						}
						return;
					}
					IL_D9:
					foreach (object obj3 in e.OldItems)
					{
						ValidationRule item3 = (ValidationRule)obj3;
						this._defaultBindingGroup.ValidationRules.Remove(item3);
					}
					using (IEnumerator enumerator = e.NewItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj4 = enumerator.Current;
							ValidationRule item4 = (ValidationRule)obj4;
							this._defaultBindingGroup.ValidationRules.Add(item4);
						}
						return;
					}
					IL_16A:
					this._defaultBindingGroup.ValidationRules.Clear();
					return;
				}
				this._defaultBindingGroup = null;
			}
		}

		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x0600628F RID: 25231 RVA: 0x002A0BEC File Offset: 0x0029FBEC
		// (set) Token: 0x06006290 RID: 25232 RVA: 0x002A0BFE File Offset: 0x0029FBFE
		public StyleSelector RowStyleSelector
		{
			get
			{
				return (StyleSelector)base.GetValue(DataGrid.RowStyleSelectorProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowStyleSelectorProperty, value);
			}
		}

		// Token: 0x06006291 RID: 25233 RVA: 0x002A0C0C File Offset: 0x0029FC0C
		private static void OnRowStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(ItemsControl.ItemContainerStyleSelectorProperty);
		}

		// Token: 0x06006292 RID: 25234 RVA: 0x002A0C19 File Offset: 0x0029FC19
		private static object OnCoerceItemContainerStyleSelector(DependencyObject d, object baseValue)
		{
			if (!DataGridHelper.IsDefaultValue(d, DataGrid.RowStyleSelectorProperty))
			{
				return d.GetValue(DataGrid.RowStyleSelectorProperty);
			}
			return baseValue;
		}

		// Token: 0x06006293 RID: 25235 RVA: 0x002A0C35 File Offset: 0x0029FC35
		private static object OnCoerceIsSynchronizedWithCurrentItem(DependencyObject d, object baseValue)
		{
			if (((DataGrid)d).SelectionUnit == DataGridSelectionUnit.Cell)
			{
				return false;
			}
			return baseValue;
		}

		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x06006294 RID: 25236 RVA: 0x002A0C4C File Offset: 0x0029FC4C
		// (set) Token: 0x06006295 RID: 25237 RVA: 0x002A0C5E File Offset: 0x0029FC5E
		public Brush RowBackground
		{
			get
			{
				return (Brush)base.GetValue(DataGrid.RowBackgroundProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowBackgroundProperty, value);
			}
		}

		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x06006296 RID: 25238 RVA: 0x002A0C6C File Offset: 0x0029FC6C
		// (set) Token: 0x06006297 RID: 25239 RVA: 0x002A0C7E File Offset: 0x0029FC7E
		public Brush AlternatingRowBackground
		{
			get
			{
				return (Brush)base.GetValue(DataGrid.AlternatingRowBackgroundProperty);
			}
			set
			{
				base.SetValue(DataGrid.AlternatingRowBackgroundProperty, value);
			}
		}

		// Token: 0x06006298 RID: 25240 RVA: 0x002A0C8C File Offset: 0x0029FC8C
		private static object OnCoerceAlternationCount(DependencyObject d, object baseValue)
		{
			if ((int)baseValue < 2 && ((DataGrid)d).AlternatingRowBackground != null)
			{
				return 2;
			}
			return baseValue;
		}

		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x06006299 RID: 25241 RVA: 0x002A0CAC File Offset: 0x0029FCAC
		// (set) Token: 0x0600629A RID: 25242 RVA: 0x002A0CBE File Offset: 0x0029FCBE
		public double RowHeight
		{
			get
			{
				return (double)base.GetValue(DataGrid.RowHeightProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeightProperty, value);
			}
		}

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x0600629B RID: 25243 RVA: 0x002A0CD1 File Offset: 0x0029FCD1
		// (set) Token: 0x0600629C RID: 25244 RVA: 0x002A0CE3 File Offset: 0x0029FCE3
		public double MinRowHeight
		{
			get
			{
				return (double)base.GetValue(DataGrid.MinRowHeightProperty);
			}
			set
			{
				base.SetValue(DataGrid.MinRowHeightProperty, value);
			}
		}

		// Token: 0x170016D4 RID: 5844
		// (get) Token: 0x0600629D RID: 25245 RVA: 0x002A0CF6 File Offset: 0x0029FCF6
		internal Visibility PlaceholderVisibility
		{
			get
			{
				return this._placeholderVisibility;
			}
		}

		// Token: 0x140000ED RID: 237
		// (add) Token: 0x0600629E RID: 25246 RVA: 0x002A0D00 File Offset: 0x0029FD00
		// (remove) Token: 0x0600629F RID: 25247 RVA: 0x002A0D38 File Offset: 0x0029FD38
		public event EventHandler<DataGridRowEventArgs> LoadingRow;

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x060062A0 RID: 25248 RVA: 0x002A0D70 File Offset: 0x0029FD70
		// (remove) Token: 0x060062A1 RID: 25249 RVA: 0x002A0DA8 File Offset: 0x0029FDA8
		public event EventHandler<DataGridRowEventArgs> UnloadingRow;

		// Token: 0x060062A2 RID: 25250 RVA: 0x002A0DE0 File Offset: 0x0029FDE0
		protected virtual void OnLoadingRow(DataGridRowEventArgs e)
		{
			if (this.LoadingRow != null)
			{
				this.LoadingRow(this, e);
			}
			DataGridRow row = e.Row;
			if (row.DetailsVisibility == Visibility.Visible && row.DetailsPresenter != null)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(new DispatcherOperationCallback(DataGrid.DelayedOnLoadingRowDetails), DispatcherPriority.Loaded, new object[]
				{
					row
				});
			}
		}

		// Token: 0x060062A3 RID: 25251 RVA: 0x002A0E3C File Offset: 0x0029FE3C
		internal static object DelayedOnLoadingRowDetails(object arg)
		{
			DataGridRow dataGridRow = (DataGridRow)arg;
			DataGrid dataGridOwner = dataGridRow.DataGridOwner;
			if (dataGridOwner != null)
			{
				dataGridOwner.OnLoadingRowDetailsWrapper(dataGridRow);
			}
			return null;
		}

		// Token: 0x060062A4 RID: 25252 RVA: 0x002A0E64 File Offset: 0x0029FE64
		protected virtual void OnUnloadingRow(DataGridRowEventArgs e)
		{
			if (this.UnloadingRow != null)
			{
				this.UnloadingRow(this, e);
			}
			DataGridRow row = e.Row;
			this.OnUnloadingRowDetailsWrapper(row);
		}

		// Token: 0x170016D5 RID: 5845
		// (get) Token: 0x060062A5 RID: 25253 RVA: 0x002A0E94 File Offset: 0x0029FE94
		// (set) Token: 0x060062A6 RID: 25254 RVA: 0x002A0EA6 File Offset: 0x0029FEA6
		public double RowHeaderWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.RowHeaderWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeaderWidthProperty, value);
			}
		}

		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x060062A7 RID: 25255 RVA: 0x002A0EB9 File Offset: 0x0029FEB9
		// (set) Token: 0x060062A8 RID: 25256 RVA: 0x002A0ECB File Offset: 0x0029FECB
		public double RowHeaderActualWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.RowHeaderActualWidthProperty);
			}
			internal set
			{
				base.SetValue(DataGrid.RowHeaderActualWidthPropertyKey, value);
			}
		}

		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x060062A9 RID: 25257 RVA: 0x002A0EDE File Offset: 0x0029FEDE
		// (set) Token: 0x060062AA RID: 25258 RVA: 0x002A0EF0 File Offset: 0x0029FEF0
		public double ColumnHeaderHeight
		{
			get
			{
				return (double)base.GetValue(DataGrid.ColumnHeaderHeightProperty);
			}
			set
			{
				base.SetValue(DataGrid.ColumnHeaderHeightProperty, value);
			}
		}

		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x060062AB RID: 25259 RVA: 0x002A0F03 File Offset: 0x0029FF03
		// (set) Token: 0x060062AC RID: 25260 RVA: 0x002A0F15 File Offset: 0x0029FF15
		public DataGridHeadersVisibility HeadersVisibility
		{
			get
			{
				return (DataGridHeadersVisibility)base.GetValue(DataGrid.HeadersVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGrid.HeadersVisibilityProperty, value);
			}
		}

		// Token: 0x060062AD RID: 25261 RVA: 0x002A0F28 File Offset: 0x0029FF28
		private static void OnNotifyRowHeaderWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			double num = (double)e.NewValue;
			if (!DoubleUtil.IsNaN(num))
			{
				dataGrid.RowHeaderActualWidth = num;
			}
			else
			{
				dataGrid.RowHeaderActualWidth = 0.0;
			}
			DataGrid.OnNotifyRowHeaderPropertyChanged(d, e);
		}

		// Token: 0x060062AE RID: 25262 RVA: 0x002A0F70 File Offset: 0x0029FF70
		private void ResetRowHeaderActualWidth()
		{
			if (DoubleUtil.IsNaN(this.RowHeaderWidth))
			{
				this.RowHeaderActualWidth = 0.0;
			}
		}

		// Token: 0x060062AF RID: 25263 RVA: 0x002A0F90 File Offset: 0x0029FF90
		public void SetDetailsVisibilityForItem(object item, Visibility detailsVisibility)
		{
			this._itemAttachedStorage.SetValue(item, DataGridRow.DetailsVisibilityProperty, detailsVisibility);
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(item);
			if (dataGridRow != null)
			{
				dataGridRow.DetailsVisibility = detailsVisibility;
			}
		}

		// Token: 0x060062B0 RID: 25264 RVA: 0x002A0FD0 File Offset: 0x0029FFD0
		public Visibility GetDetailsVisibilityForItem(object item)
		{
			object obj;
			if (this._itemAttachedStorage.TryGetValue(item, DataGridRow.DetailsVisibilityProperty, out obj))
			{
				return (Visibility)obj;
			}
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(item);
			if (dataGridRow != null)
			{
				return dataGridRow.DetailsVisibility;
			}
			DataGridRowDetailsVisibilityMode rowDetailsVisibilityMode = this.RowDetailsVisibilityMode;
			if (rowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.Visible)
			{
				return Visibility.Visible;
			}
			if (rowDetailsVisibilityMode != DataGridRowDetailsVisibilityMode.VisibleWhenSelected)
			{
				return Visibility.Collapsed;
			}
			if (!base.SelectedItems.Contains(item))
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		// Token: 0x060062B1 RID: 25265 RVA: 0x002A103C File Offset: 0x002A003C
		public void ClearDetailsVisibilityForItem(object item)
		{
			this._itemAttachedStorage.ClearValue(item, DataGridRow.DetailsVisibilityProperty);
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(item);
			if (dataGridRow != null)
			{
				dataGridRow.ClearValue(DataGridRow.DetailsVisibilityProperty);
			}
		}

		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x060062B2 RID: 25266 RVA: 0x002A107A File Offset: 0x002A007A
		internal DataGridItemAttachedStorage ItemAttachedStorage
		{
			get
			{
				return this._itemAttachedStorage;
			}
		}

		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x060062B3 RID: 25267 RVA: 0x002A1084 File Offset: 0x002A0084
		private bool ShouldSelectRowHeader
		{
			get
			{
				return this._selectionAnchor != null && base.SelectedItems.Contains(this._selectionAnchor.Value.Item) && this.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
			}
		}

		// Token: 0x170016DB RID: 5851
		// (get) Token: 0x060062B4 RID: 25268 RVA: 0x002A10D3 File Offset: 0x002A00D3
		// (set) Token: 0x060062B5 RID: 25269 RVA: 0x002A10E5 File Offset: 0x002A00E5
		public Style CellStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.CellStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.CellStyleProperty, value);
			}
		}

		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x060062B6 RID: 25270 RVA: 0x002A10F3 File Offset: 0x002A00F3
		// (set) Token: 0x060062B7 RID: 25271 RVA: 0x002A1105 File Offset: 0x002A0105
		public Style ColumnHeaderStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.ColumnHeaderStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.ColumnHeaderStyleProperty, value);
			}
		}

		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x060062B8 RID: 25272 RVA: 0x002A1113 File Offset: 0x002A0113
		// (set) Token: 0x060062B9 RID: 25273 RVA: 0x002A1125 File Offset: 0x002A0125
		public Style RowHeaderStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.RowHeaderStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeaderStyleProperty, value);
			}
		}

		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x060062BA RID: 25274 RVA: 0x002A1133 File Offset: 0x002A0133
		// (set) Token: 0x060062BB RID: 25275 RVA: 0x002A1145 File Offset: 0x002A0145
		public DataTemplate RowHeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGrid.RowHeaderTemplateProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeaderTemplateProperty, value);
			}
		}

		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x060062BC RID: 25276 RVA: 0x002A1153 File Offset: 0x002A0153
		// (set) Token: 0x060062BD RID: 25277 RVA: 0x002A1165 File Offset: 0x002A0165
		public DataTemplateSelector RowHeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGrid.RowHeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x060062BE RID: 25278 RVA: 0x002A1173 File Offset: 0x002A0173
		public static ComponentResourceKey FocusBorderBrushKey
		{
			get
			{
				return SystemResourceKey.DataGridFocusBorderBrushKey;
			}
		}

		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x060062BF RID: 25279 RVA: 0x002A117A File Offset: 0x002A017A
		public static IValueConverter HeadersVisibilityConverter
		{
			get
			{
				if (DataGrid._headersVisibilityConverter == null)
				{
					DataGrid._headersVisibilityConverter = new DataGridHeadersVisibilityToVisibilityConverter();
				}
				return DataGrid._headersVisibilityConverter;
			}
		}

		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x060062C0 RID: 25280 RVA: 0x002A1192 File Offset: 0x002A0192
		public static IValueConverter RowDetailsScrollingConverter
		{
			get
			{
				if (DataGrid._rowDetailsScrollingConverter == null)
				{
					DataGrid._rowDetailsScrollingConverter = new BooleanToSelectiveScrollingOrientationConverter();
				}
				return DataGrid._rowDetailsScrollingConverter;
			}
		}

		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x060062C1 RID: 25281 RVA: 0x002A11AA File Offset: 0x002A01AA
		// (set) Token: 0x060062C2 RID: 25282 RVA: 0x002A11BC File Offset: 0x002A01BC
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(DataGrid.HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGrid.HorizontalScrollBarVisibilityProperty, value);
			}
		}

		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x060062C3 RID: 25283 RVA: 0x002A11CF File Offset: 0x002A01CF
		// (set) Token: 0x060062C4 RID: 25284 RVA: 0x002A11E1 File Offset: 0x002A01E1
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(DataGrid.VerticalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGrid.VerticalScrollBarVisibilityProperty, value);
			}
		}

		// Token: 0x060062C5 RID: 25285 RVA: 0x002A11F4 File Offset: 0x002A01F4
		public void ScrollIntoView(object item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ScrollIntoView(base.NewItemInfo(item, null, -1));
		}

		// Token: 0x060062C6 RID: 25286 RVA: 0x002A1213 File Offset: 0x002A0213
		internal void ScrollIntoView(ItemsControl.ItemInfo info)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				base.OnBringItemIntoView(info);
				return;
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(base.OnBringItemIntoView), info);
		}

		// Token: 0x060062C7 RID: 25287 RVA: 0x002A1248 File Offset: 0x002A0248
		public void ScrollIntoView(object item, DataGridColumn column)
		{
			ItemsControl.ItemInfo info = (item == null) ? null : base.NewItemInfo(item, null, -1);
			this.ScrollIntoView(info, column);
		}

		// Token: 0x060062C8 RID: 25288 RVA: 0x002A1270 File Offset: 0x002A0270
		private void ScrollIntoView(ItemsControl.ItemInfo info, DataGridColumn column)
		{
			if (column == null)
			{
				this.ScrollIntoView(info);
				return;
			}
			if (!column.IsVisible)
			{
				return;
			}
			if (base.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.OnScrollIntoView), new object[]
				{
					info,
					column
				});
				return;
			}
			if (info == null)
			{
				this.ScrollColumnIntoView(column);
				return;
			}
			this.ScrollCellIntoView(info, column);
		}

		// Token: 0x060062C9 RID: 25289 RVA: 0x002A12E0 File Offset: 0x002A02E0
		private object OnScrollIntoView(object arg)
		{
			object[] array = arg as object[];
			if (array != null)
			{
				if (array[0] != null)
				{
					this.ScrollCellIntoView((ItemsControl.ItemInfo)array[0], (DataGridColumn)array[1]);
				}
				else
				{
					this.ScrollColumnIntoView((DataGridColumn)array[1]);
				}
			}
			else
			{
				base.OnBringItemIntoView((ItemsControl.ItemInfo)arg);
			}
			return null;
		}

		// Token: 0x060062CA RID: 25290 RVA: 0x002A1334 File Offset: 0x002A0334
		private void ScrollColumnIntoView(DataGridColumn column)
		{
			if (this._rowTrackingRoot != null)
			{
				DataGridRow container = this._rowTrackingRoot.Container;
				if (container != null)
				{
					int index = this._columns.IndexOf(column);
					container.ScrollCellIntoView(index);
				}
			}
		}

		// Token: 0x060062CB RID: 25291 RVA: 0x002A136C File Offset: 0x002A036C
		private void ScrollCellIntoView(ItemsControl.ItemInfo info, DataGridColumn column)
		{
			if (!column.IsVisible)
			{
				return;
			}
			DataGridRow dataGridRow = base.ContainerFromItemInfo(info) as DataGridRow;
			if (dataGridRow == null)
			{
				base.OnBringItemIntoView(info);
				base.UpdateLayout();
				dataGridRow = (base.ContainerFromItemInfo(info) as DataGridRow);
			}
			else
			{
				dataGridRow.BringIntoView();
				base.UpdateLayout();
			}
			if (dataGridRow != null)
			{
				int index = this._columns.IndexOf(column);
				dataGridRow.ScrollCellIntoView(index);
			}
		}

		// Token: 0x060062CC RID: 25292 RVA: 0x002A13D2 File Offset: 0x002A03D2
		protected override void OnIsMouseCapturedChanged(DependencyPropertyChangedEventArgs e)
		{
			if (!base.IsMouseCaptured)
			{
				this.StopAutoScroll();
			}
			base.OnIsMouseCapturedChanged(e);
		}

		// Token: 0x060062CD RID: 25293 RVA: 0x002A13EC File Offset: 0x002A03EC
		private void StartAutoScroll()
		{
			if (this._autoScrollTimer == null)
			{
				this._hasAutoScrolled = false;
				this._autoScrollTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
				this._autoScrollTimer.Interval = ItemsControl.AutoScrollTimeout;
				this._autoScrollTimer.Tick += this.OnAutoScrollTimeout;
				this._autoScrollTimer.Start();
			}
		}

		// Token: 0x060062CE RID: 25294 RVA: 0x002A1446 File Offset: 0x002A0446
		private void StopAutoScroll()
		{
			if (this._autoScrollTimer != null)
			{
				this._autoScrollTimer.Stop();
				this._autoScrollTimer = null;
				this._hasAutoScrolled = false;
			}
		}

		// Token: 0x060062CF RID: 25295 RVA: 0x002A1469 File Offset: 0x002A0469
		private void OnAutoScrollTimeout(object sender, EventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				this.DoAutoScroll();
				return;
			}
			this.StopAutoScroll();
		}

		// Token: 0x060062D0 RID: 25296 RVA: 0x002A1484 File Offset: 0x002A0484
		private new bool DoAutoScroll()
		{
			DataGrid.RelativeMousePositions relativeMousePosition = this.RelativeMousePosition;
			if (relativeMousePosition != DataGrid.RelativeMousePositions.Over)
			{
				DataGridCell dataGridCell = this.GetCellNearMouse();
				if (dataGridCell != null)
				{
					DataGridColumn dataGridColumn = dataGridCell.Column;
					ItemsControl.ItemInfo itemInfo = base.ItemInfoFromContainer(dataGridCell.RowOwner);
					if (DataGrid.IsMouseToLeft(relativeMousePosition))
					{
						int displayIndex = dataGridColumn.DisplayIndex;
						if (displayIndex > 0)
						{
							dataGridColumn = this.ColumnFromDisplayIndex(displayIndex - 1);
						}
					}
					else if (DataGrid.IsMouseToRight(relativeMousePosition))
					{
						int displayIndex2 = dataGridColumn.DisplayIndex;
						if (displayIndex2 < this._columns.Count - 1)
						{
							dataGridColumn = this.ColumnFromDisplayIndex(displayIndex2 + 1);
						}
					}
					if (DataGrid.IsMouseAbove(relativeMousePosition))
					{
						int index = itemInfo.Index;
						if (index > 0)
						{
							itemInfo = base.ItemInfoFromIndex(index - 1);
						}
					}
					else if (DataGrid.IsMouseBelow(relativeMousePosition))
					{
						int index2 = itemInfo.Index;
						if (index2 < base.Items.Count - 1)
						{
							itemInfo = base.ItemInfoFromIndex(index2 + 1);
						}
					}
					if (this._isRowDragging)
					{
						base.OnBringItemIntoView(itemInfo);
						DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromIndex(itemInfo.Index);
						if (dataGridRow != null)
						{
							this._hasAutoScrolled = true;
							this.HandleSelectionForRowHeaderAndDetailsInput(dataGridRow, false);
							this.SetCurrentItem(itemInfo.Item);
							return true;
						}
					}
					else
					{
						this.ScrollCellIntoView(itemInfo, dataGridColumn);
						dataGridCell = this.TryFindCell(itemInfo, dataGridColumn);
						if (dataGridCell != null)
						{
							this._hasAutoScrolled = true;
							this.HandleSelectionForCellInput(dataGridCell, false, true, true);
							dataGridCell.Focus();
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x060062D1 RID: 25297 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x060062D2 RID: 25298 RVA: 0x002A15D4 File Offset: 0x002A05D4
		// (set) Token: 0x060062D3 RID: 25299 RVA: 0x002A15DC File Offset: 0x002A05DC
		internal Panel InternalItemsHost
		{
			get
			{
				return this._internalItemsHost;
			}
			set
			{
				if (this._internalItemsHost != value)
				{
					this._internalItemsHost = value;
					if (this._internalItemsHost != null)
					{
						this.DetermineItemsHostStarBehavior();
						this.EnsureInternalScrollControls();
					}
				}
			}
		}

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x060062D4 RID: 25300 RVA: 0x002A1602 File Offset: 0x002A0602
		internal ScrollViewer InternalScrollHost
		{
			get
			{
				this.EnsureInternalScrollControls();
				return this._internalScrollHost;
			}
		}

		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x060062D5 RID: 25301 RVA: 0x002A1610 File Offset: 0x002A0610
		internal ScrollContentPresenter InternalScrollContentPresenter
		{
			get
			{
				this.EnsureInternalScrollControls();
				return this._internalScrollContentPresenter;
			}
		}

		// Token: 0x060062D6 RID: 25302 RVA: 0x002A1620 File Offset: 0x002A0620
		private void DetermineItemsHostStarBehavior()
		{
			VirtualizingStackPanel virtualizingStackPanel = this._internalItemsHost as VirtualizingStackPanel;
			if (virtualizingStackPanel != null)
			{
				virtualizingStackPanel.IgnoreMaxDesiredSize = this.InternalColumns.HasVisibleStarColumns;
			}
		}

		// Token: 0x060062D7 RID: 25303 RVA: 0x002A1650 File Offset: 0x002A0650
		private void EnsureInternalScrollControls()
		{
			if (this._internalScrollContentPresenter == null)
			{
				if (this._internalItemsHost != null)
				{
					this._internalScrollContentPresenter = DataGridHelper.FindVisualParent<ScrollContentPresenter>(this._internalItemsHost);
				}
				else if (this._rowTrackingRoot != null)
				{
					DataGridRow container = this._rowTrackingRoot.Container;
					this._internalScrollContentPresenter = DataGridHelper.FindVisualParent<ScrollContentPresenter>(container);
				}
				if (this._internalScrollContentPresenter != null)
				{
					this._internalScrollContentPresenter.SizeChanged += this.OnInternalScrollContentPresenterSizeChanged;
				}
			}
			if (this._internalScrollHost == null)
			{
				if (this._internalItemsHost != null)
				{
					this._internalScrollHost = DataGridHelper.FindVisualParent<ScrollViewer>(this._internalItemsHost);
				}
				else if (this._rowTrackingRoot != null)
				{
					DataGridRow container2 = this._rowTrackingRoot.Container;
					this._internalScrollHost = DataGridHelper.FindVisualParent<ScrollViewer>(container2);
				}
				if (this._internalScrollHost != null)
				{
					Binding binding = new Binding("ContentHorizontalOffset");
					binding.Source = this._internalScrollHost;
					base.SetBinding(DataGrid.HorizontalScrollOffsetProperty, binding);
				}
			}
		}

		// Token: 0x060062D8 RID: 25304 RVA: 0x002A172E File Offset: 0x002A072E
		private void CleanUpInternalScrollControls()
		{
			BindingOperations.ClearBinding(this, DataGrid.HorizontalScrollOffsetProperty);
			this._internalScrollHost = null;
			if (this._internalScrollContentPresenter != null)
			{
				this._internalScrollContentPresenter.SizeChanged -= this.OnInternalScrollContentPresenterSizeChanged;
				this._internalScrollContentPresenter = null;
			}
		}

		// Token: 0x060062D9 RID: 25305 RVA: 0x002A1768 File Offset: 0x002A0768
		private void OnInternalScrollContentPresenterSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this._internalScrollContentPresenter != null && !this._internalScrollContentPresenter.CanContentScroll)
			{
				this.OnViewportSizeChanged(e.PreviousSize, e.NewSize);
			}
		}

		// Token: 0x060062DA RID: 25306 RVA: 0x002A1794 File Offset: 0x002A0794
		internal void OnViewportSizeChanged(Size oldSize, Size newSize)
		{
			if (!this.InternalColumns.ColumnWidthsComputationPending && !DoubleUtil.AreClose(newSize.Width - oldSize.Width, 0.0))
			{
				this._finalViewportWidth = newSize.Width;
				if (!this._viewportWidthChangeNotificationPending)
				{
					this._originalViewportWidth = oldSize.Width;
					base.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.OnDelayedViewportWidthChanged), DispatcherPriority.Loaded, new object[]
					{
						this
					});
					this._viewportWidthChangeNotificationPending = true;
				}
			}
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x002A181C File Offset: 0x002A081C
		private object OnDelayedViewportWidthChanged(object args)
		{
			if (!this._viewportWidthChangeNotificationPending)
			{
				return null;
			}
			double num = this._finalViewportWidth - this._originalViewportWidth;
			if (!DoubleUtil.AreClose(num, 0.0))
			{
				this.NotifyPropertyChanged(this, "ViewportWidth", default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeadersPresenter);
				double num2 = this._finalViewportWidth;
				num2 -= this.CellsPanelHorizontalOffset;
				this.InternalColumns.RedistributeColumnWidthsOnAvailableSpaceChange(num, num2);
			}
			this._viewportWidthChangeNotificationPending = false;
			return null;
		}

		// Token: 0x060062DC RID: 25308 RVA: 0x002A188E File Offset: 0x002A088E
		internal void OnHasVisibleStarColumnsChanged()
		{
			this.DetermineItemsHostStarBehavior();
		}

		// Token: 0x170016E9 RID: 5865
		// (get) Token: 0x060062DD RID: 25309 RVA: 0x002A1896 File Offset: 0x002A0896
		internal double HorizontalScrollOffset
		{
			get
			{
				return (double)base.GetValue(DataGrid.HorizontalScrollOffsetProperty);
			}
		}

		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x060062DE RID: 25310 RVA: 0x002A18A8 File Offset: 0x002A08A8
		public static RoutedUICommand DeleteCommand
		{
			get
			{
				return ApplicationCommands.Delete;
			}
		}

		// Token: 0x060062DF RID: 25311 RVA: 0x002A18AF File Offset: 0x002A08AF
		private static void OnCanExecuteBeginEdit(object sender, CanExecuteRoutedEventArgs e)
		{
			((DataGrid)sender).OnCanExecuteBeginEdit(e);
		}

		// Token: 0x060062E0 RID: 25312 RVA: 0x002A18BD File Offset: 0x002A08BD
		private static void OnExecutedBeginEdit(object sender, ExecutedRoutedEventArgs e)
		{
			((DataGrid)sender).OnExecutedBeginEdit(e);
		}

		// Token: 0x060062E1 RID: 25313 RVA: 0x002A18CC File Offset: 0x002A08CC
		protected virtual void OnCanExecuteBeginEdit(CanExecuteRoutedEventArgs e)
		{
			bool flag = !this.IsReadOnly && this.CurrentCellContainer != null && !this.IsEditingCurrentCell && !this.IsCurrentCellReadOnly && !this.HasCellValidationError;
			if (flag && this.HasRowValidationError)
			{
				DataGridCell eventCellOrCurrentCell = this.GetEventCellOrCurrentCell(e);
				if (eventCellOrCurrentCell != null)
				{
					object rowDataItem = eventCellOrCurrentCell.RowDataItem;
					flag = this.IsAddingOrEditingRowItem(rowDataItem);
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				e.CanExecute = true;
				e.Handled = true;
				return;
			}
			e.ContinueRouting = true;
		}

		// Token: 0x060062E2 RID: 25314 RVA: 0x002A1948 File Offset: 0x002A0948
		protected virtual void OnExecutedBeginEdit(ExecutedRoutedEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null && !currentCellContainer.IsReadOnly && !currentCellContainer.IsEditing)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				List<int> list = null;
				int num = -1;
				object obj = null;
				bool flag4 = this.EditableItems.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning;
				if (this.IsNewItemPlaceholder(currentCellContainer.RowDataItem))
				{
					if (base.SelectedItems.Contains(CollectionView.NewItemPlaceholder))
					{
						this.UnselectItem(base.NewItemInfo(CollectionView.NewItemPlaceholder, null, -1));
						flag2 = true;
					}
					else
					{
						num = base.ItemContainerGenerator.IndexFromContainer(currentCellContainer.RowOwner);
						flag3 = (num >= 0 && this._selectedCells.Intersects(num, out list));
					}
					obj = this.AddNewItem();
					this.SetCurrentCellToNewItem(obj);
					currentCellContainer = this.CurrentCellContainer;
					if (this.CurrentCellContainer == null)
					{
						base.UpdateLayout();
						currentCellContainer = this.CurrentCellContainer;
						if (currentCellContainer != null && !currentCellContainer.IsKeyboardFocusWithin)
						{
							currentCellContainer.Focus();
						}
					}
					if (flag2)
					{
						this.SelectItem(base.NewItemInfo(obj, null, -1));
					}
					else if (flag3)
					{
						using (this.UpdateSelectedCells())
						{
							int num2 = num;
							if (flag4)
							{
								this._selectedCells.RemoveRegion(num, 0, 1, this.Columns.Count);
								num2++;
							}
							int i = 0;
							int count = list.Count;
							while (i < count)
							{
								this._selectedCells.AddRegion(num2, list[i], 1, list[i + 1]);
								i += 2;
							}
						}
					}
					flag = true;
				}
				RoutedEventArgs routedEventArgs = e.Parameter as RoutedEventArgs;
				DataGridBeginningEditEventArgs dataGridBeginningEditEventArgs = null;
				if (currentCellContainer != null)
				{
					dataGridBeginningEditEventArgs = new DataGridBeginningEditEventArgs(currentCellContainer.Column, currentCellContainer.RowOwner, routedEventArgs);
					this.OnBeginningEdit(dataGridBeginningEditEventArgs);
				}
				if (currentCellContainer == null || dataGridBeginningEditEventArgs.Cancel)
				{
					if (flag2)
					{
						this.UnselectItem(base.NewItemInfo(obj, null, -1));
					}
					else if (flag3 && flag4)
					{
						this._selectedCells.RemoveRegion(num + 1, 0, 1, this.Columns.Count);
					}
					if (flag)
					{
						this.CancelRowItem();
						this.UpdateNewItemPlaceholder(false);
						this.SetCurrentItemToPlaceholder();
					}
					if (flag2)
					{
						this.SelectItem(base.NewItemInfo(CollectionView.NewItemPlaceholder, null, -1));
					}
					else if (flag3)
					{
						int j = 0;
						int count2 = list.Count;
						while (j < count2)
						{
							this._selectedCells.AddRegion(num, list[j], 1, list[j + 1]);
							j += 2;
						}
					}
				}
				else
				{
					if (!flag && !this.IsEditingRowItem)
					{
						this.EditRowItem(currentCellContainer.RowDataItem);
						BindingGroup bindingGroup = currentCellContainer.RowOwner.BindingGroup;
						if (bindingGroup != null)
						{
							bindingGroup.BeginEdit();
						}
						this._editingRowInfo = base.ItemInfoFromContainer(currentCellContainer.RowOwner);
					}
					currentCellContainer.BeginEdit(routedEventArgs);
					currentCellContainer.RowOwner.IsEditing = true;
					this.EnsureCellAutomationValueHolder(currentCellContainer);
				}
			}
			CommandManager.InvalidateRequerySuggested();
			e.Handled = true;
		}

		// Token: 0x060062E3 RID: 25315 RVA: 0x002A1C34 File Offset: 0x002A0C34
		private static void OnCanExecuteCommitEdit(object sender, CanExecuteRoutedEventArgs e)
		{
			((DataGrid)sender).OnCanExecuteCommitEdit(e);
		}

		// Token: 0x060062E4 RID: 25316 RVA: 0x002A1C42 File Offset: 0x002A0C42
		private static void OnExecutedCommitEdit(object sender, ExecutedRoutedEventArgs e)
		{
			((DataGrid)sender).OnExecutedCommitEdit(e);
		}

		// Token: 0x060062E5 RID: 25317 RVA: 0x002A1C50 File Offset: 0x002A0C50
		private DataGridCell GetEventCellOrCurrentCell(RoutedEventArgs e)
		{
			UIElement uielement = e.OriginalSource as UIElement;
			if (uielement != this && uielement != null)
			{
				return DataGridHelper.FindVisualParent<DataGridCell>(uielement);
			}
			return this.CurrentCellContainer;
		}

		// Token: 0x060062E6 RID: 25318 RVA: 0x002A1C80 File Offset: 0x002A0C80
		private bool CanEndEdit(CanExecuteRoutedEventArgs e, bool commit)
		{
			DataGridCell eventCellOrCurrentCell = this.GetEventCellOrCurrentCell(e);
			if (eventCellOrCurrentCell == null)
			{
				return false;
			}
			DataGridEditingUnit editingUnit = this.GetEditingUnit(e.Parameter);
			IEditableCollectionView editableItems = this.EditableItems;
			object rowDataItem = eventCellOrCurrentCell.RowDataItem;
			return eventCellOrCurrentCell.IsEditing || (!this.HasCellValidationError && this.IsAddingOrEditingRowItem(editingUnit, rowDataItem));
		}

		// Token: 0x060062E7 RID: 25319 RVA: 0x002A1CD1 File Offset: 0x002A0CD1
		protected virtual void OnCanExecuteCommitEdit(CanExecuteRoutedEventArgs e)
		{
			if (this.CanEndEdit(e, true))
			{
				e.CanExecute = true;
				e.Handled = true;
				return;
			}
			e.ContinueRouting = true;
		}

		// Token: 0x060062E8 RID: 25320 RVA: 0x002A1CF4 File Offset: 0x002A0CF4
		protected virtual void OnExecutedCommitEdit(ExecutedRoutedEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			bool flag = true;
			if (currentCellContainer != null)
			{
				DataGridEditingUnit editingUnit = this.GetEditingUnit(e.Parameter);
				bool flag2 = false;
				if (currentCellContainer.IsEditing)
				{
					DataGridCellEditEndingEventArgs dataGridCellEditEndingEventArgs = new DataGridCellEditEndingEventArgs(currentCellContainer.Column, currentCellContainer.RowOwner, currentCellContainer.EditingElement, DataGridEditAction.Commit);
					this.OnCellEditEnding(dataGridCellEditEndingEventArgs);
					flag2 = dataGridCellEditEndingEventArgs.Cancel;
					if (!flag2)
					{
						flag = currentCellContainer.CommitEdit();
						this.HasCellValidationError = !flag;
						this.UpdateCellAutomationValueHolder(currentCellContainer);
					}
				}
				if (flag && !flag2 && this.IsAddingOrEditingRowItem(editingUnit, currentCellContainer.RowDataItem))
				{
					DataGridRowEditEndingEventArgs dataGridRowEditEndingEventArgs = new DataGridRowEditEndingEventArgs(currentCellContainer.RowOwner, DataGridEditAction.Commit);
					this.OnRowEditEnding(dataGridRowEditEndingEventArgs);
					if (!dataGridRowEditEndingEventArgs.Cancel)
					{
						BindingGroup bindingGroup = currentCellContainer.RowOwner.BindingGroup;
						if (bindingGroup != null)
						{
							base.Dispatcher.Invoke(new DispatcherOperationCallback(DataGrid.DoNothing), DispatcherPriority.DataBind, new object[]
							{
								bindingGroup
							});
							flag = bindingGroup.CommitEdit();
						}
						this.HasRowValidationError = !flag;
						if (flag)
						{
							this.CommitRowItem();
						}
					}
				}
				if (flag)
				{
					this.UpdateRowEditing(currentCellContainer);
					if (!currentCellContainer.RowOwner.IsEditing)
					{
						this.ReleaseCellAutomationValueHolders();
					}
				}
				CommandManager.InvalidateRequerySuggested();
			}
			e.Handled = true;
		}

		// Token: 0x060062E9 RID: 25321 RVA: 0x00109403 File Offset: 0x00108403
		private static object DoNothing(object arg)
		{
			return null;
		}

		// Token: 0x060062EA RID: 25322 RVA: 0x002A1E1C File Offset: 0x002A0E1C
		private DataGridEditingUnit GetEditingUnit(object parameter)
		{
			if (parameter != null && parameter is DataGridEditingUnit)
			{
				return (DataGridEditingUnit)parameter;
			}
			if (!this.IsEditingCurrentCell)
			{
				return DataGridEditingUnit.Row;
			}
			return DataGridEditingUnit.Cell;
		}

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x060062EB RID: 25323 RVA: 0x002A1E3C File Offset: 0x002A0E3C
		// (remove) Token: 0x060062EC RID: 25324 RVA: 0x002A1E74 File Offset: 0x002A0E74
		public event EventHandler<DataGridRowEditEndingEventArgs> RowEditEnding;

		// Token: 0x060062ED RID: 25325 RVA: 0x002A1EAC File Offset: 0x002A0EAC
		protected virtual void OnRowEditEnding(DataGridRowEditEndingEventArgs e)
		{
			if (this.RowEditEnding != null)
			{
				this.RowEditEnding(this, e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationRowInvokeEvents(e.Row);
				}
			}
		}

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x060062EE RID: 25326 RVA: 0x002A1EF4 File Offset: 0x002A0EF4
		// (remove) Token: 0x060062EF RID: 25327 RVA: 0x002A1F2C File Offset: 0x002A0F2C
		public event EventHandler<DataGridCellEditEndingEventArgs> CellEditEnding;

		// Token: 0x060062F0 RID: 25328 RVA: 0x002A1F64 File Offset: 0x002A0F64
		protected virtual void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
		{
			if (this.CellEditEnding != null)
			{
				this.CellEditEnding(this, e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationCellInvokeEvents(e.Column, e.Row);
				}
			}
		}

		// Token: 0x060062F1 RID: 25329 RVA: 0x002A1FAF File Offset: 0x002A0FAF
		private static void OnCanExecuteCancelEdit(object sender, CanExecuteRoutedEventArgs e)
		{
			((DataGrid)sender).OnCanExecuteCancelEdit(e);
		}

		// Token: 0x060062F2 RID: 25330 RVA: 0x002A1FBD File Offset: 0x002A0FBD
		private static void OnExecutedCancelEdit(object sender, ExecutedRoutedEventArgs e)
		{
			((DataGrid)sender).OnExecutedCancelEdit(e);
		}

		// Token: 0x060062F3 RID: 25331 RVA: 0x002A1FCB File Offset: 0x002A0FCB
		protected virtual void OnCanExecuteCancelEdit(CanExecuteRoutedEventArgs e)
		{
			if (this.CanEndEdit(e, false))
			{
				e.CanExecute = true;
				e.Handled = true;
				return;
			}
			e.ContinueRouting = true;
		}

		// Token: 0x060062F4 RID: 25332 RVA: 0x002A1FF0 File Offset: 0x002A0FF0
		protected virtual void OnExecutedCancelEdit(ExecutedRoutedEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null)
			{
				DataGridEditingUnit editingUnit = this.GetEditingUnit(e.Parameter);
				bool flag = false;
				if (currentCellContainer.IsEditing)
				{
					DataGridCellEditEndingEventArgs dataGridCellEditEndingEventArgs = new DataGridCellEditEndingEventArgs(currentCellContainer.Column, currentCellContainer.RowOwner, currentCellContainer.EditingElement, DataGridEditAction.Cancel);
					this.OnCellEditEnding(dataGridCellEditEndingEventArgs);
					flag = dataGridCellEditEndingEventArgs.Cancel;
					if (!flag)
					{
						currentCellContainer.CancelEdit();
						this.HasCellValidationError = false;
						this.UpdateCellAutomationValueHolder(currentCellContainer);
					}
				}
				if (!flag && this.IsAddingOrEditingRowItem(editingUnit, currentCellContainer.RowDataItem))
				{
					DataGridRowEditEndingEventArgs dataGridRowEditEndingEventArgs = new DataGridRowEditEndingEventArgs(currentCellContainer.RowOwner, DataGridEditAction.Cancel);
					this.OnRowEditEnding(dataGridRowEditEndingEventArgs);
					if (!dataGridRowEditEndingEventArgs.Cancel)
					{
						BindingGroup bindingGroup = currentCellContainer.RowOwner.BindingGroup;
						if (bindingGroup != null)
						{
							bindingGroup.CancelEdit();
						}
						this.CancelRowItem();
					}
				}
				this.UpdateRowEditing(currentCellContainer);
				if (!currentCellContainer.RowOwner.IsEditing)
				{
					this.HasRowValidationError = false;
					this.ReleaseCellAutomationValueHolders();
				}
				CommandManager.InvalidateRequerySuggested();
			}
			e.Handled = true;
		}

		// Token: 0x060062F5 RID: 25333 RVA: 0x002A20DE File Offset: 0x002A10DE
		private static void OnCanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			((DataGrid)sender).OnCanExecuteDelete(e);
		}

		// Token: 0x060062F6 RID: 25334 RVA: 0x002A20EC File Offset: 0x002A10EC
		private static void OnExecutedDelete(object sender, ExecutedRoutedEventArgs e)
		{
			((DataGrid)sender).OnExecutedDelete(e);
		}

		// Token: 0x060062F7 RID: 25335 RVA: 0x002A20FA File Offset: 0x002A10FA
		protected virtual void OnCanExecuteDelete(CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (this.CanUserDeleteRows && this.DataItemsSelected > 0 && (this._currentCellContainer == null || !this._currentCellContainer.IsEditing));
			e.Handled = true;
		}

		// Token: 0x060062F8 RID: 25336 RVA: 0x002A2138 File Offset: 0x002A1138
		protected virtual void OnExecutedDelete(ExecutedRoutedEventArgs e)
		{
			if (this.DataItemsSelected > 0)
			{
				bool flag = false;
				bool isEditingRowItem = this.IsEditingRowItem;
				if (isEditingRowItem || this.IsAddingNewItem)
				{
					if (this.CancelEdit(DataGridEditingUnit.Row) && isEditingRowItem)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					int count = base.SelectedItems.Count;
					int num = -1;
					ItemsControl.ItemInfo currentInfo = this.CurrentInfo;
					if (base.SelectedItems.Contains(currentInfo.Item))
					{
						num = currentInfo.Index;
						if (this._selectionAnchor != null)
						{
							int index = this._selectionAnchor.Value.ItemInfo.Index;
							if (index >= 0 && index < num)
							{
								num = index;
							}
						}
						num = Math.Min(base.Items.Count - count - 1, num);
					}
					ArrayList arrayList = new ArrayList(base.SelectedItems);
					using (this.UpdateSelectedCells())
					{
						bool isUpdatingSelectedItems = base.IsUpdatingSelectedItems;
						if (!isUpdatingSelectedItems)
						{
							base.BeginUpdateSelectedItems();
						}
						try
						{
							this._selectedCells.ClearFullRows(base.SelectedItems);
							base.SelectedItems.Clear();
						}
						finally
						{
							if (!isUpdatingSelectedItems)
							{
								base.EndUpdateSelectedItems();
							}
						}
					}
					for (int i = 0; i < count; i++)
					{
						object obj = arrayList[i];
						if (obj != CollectionView.NewItemPlaceholder)
						{
							this.EditableItems.Remove(obj);
						}
					}
					if (num >= 0)
					{
						object currentItem = base.Items[num];
						this.SetCurrentItem(currentItem);
						DataGridCell currentCellContainer = this.CurrentCellContainer;
						if (currentCellContainer != null)
						{
							this._selectionAnchor = null;
							this.HandleSelectionForCellInput(currentCellContainer, false, false, false);
						}
					}
				}
			}
			e.Handled = true;
		}

		// Token: 0x060062F9 RID: 25337 RVA: 0x002A22E8 File Offset: 0x002A12E8
		private void SetCurrentCellToNewItem(object newItem)
		{
			ItemsControl.ItemInfo itemInfo = null;
			NewItemPlaceholderPosition newItemPlaceholderPosition = this.EditableItems.NewItemPlaceholderPosition;
			if (newItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)
			{
				if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
				{
					int num = base.Items.Count - 2;
					if (num >= 0 && ItemsControl.EqualsEx(newItem, base.Items[num]))
					{
						itemInfo = base.ItemInfoFromIndex(num);
					}
				}
			}
			else
			{
				int num = 1;
				if (num < base.Items.Count && ItemsControl.EqualsEx(newItem, base.Items[num]))
				{
					itemInfo = base.ItemInfoFromIndex(num);
				}
			}
			if (itemInfo == null)
			{
				itemInfo = base.ItemInfoFromIndex(base.Items.IndexOf(newItem));
			}
			DataGridCellInfo dataGridCellInfo = this.CurrentCell;
			dataGridCellInfo = ((itemInfo != null) ? new DataGridCellInfo(itemInfo, dataGridCellInfo.Column, this) : DataGridCellInfo.CreatePossiblyPartialCellInfo(newItem, dataGridCellInfo.Column, this));
			base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, dataGridCellInfo);
		}

		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x060062FA RID: 25338 RVA: 0x002A23C3 File Offset: 0x002A13C3
		// (set) Token: 0x060062FB RID: 25339 RVA: 0x002A23D5 File Offset: 0x002A13D5
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(DataGrid.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(DataGrid.IsReadOnlyProperty, value);
			}
		}

		// Token: 0x060062FC RID: 25340 RVA: 0x002A23E3 File Offset: 0x002A13E3
		private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				((DataGrid)d).CancelAnyEdit();
			}
			CommandManager.InvalidateRequerySuggested();
			d.CoerceValue(DataGrid.CanUserAddRowsProperty);
			d.CoerceValue(DataGrid.CanUserDeleteRowsProperty);
			DataGrid.OnNotifyColumnAndCellPropertyChanged(d, e);
		}

		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x060062FD RID: 25341 RVA: 0x002A2420 File Offset: 0x002A1420
		// (set) Token: 0x060062FE RID: 25342 RVA: 0x002A242D File Offset: 0x002A142D
		public object CurrentItem
		{
			get
			{
				return base.GetValue(DataGrid.CurrentItemProperty);
			}
			set
			{
				base.SetValue(DataGrid.CurrentItemProperty, value);
			}
		}

		// Token: 0x060062FF RID: 25343 RVA: 0x002A243C File Offset: 0x002A143C
		private static void OnCurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridCellInfo currentCell = dataGrid.CurrentCell;
			object newValue = e.NewValue;
			if (currentCell.Item != newValue)
			{
				dataGrid.SetCurrentValueInternal(DataGrid.CurrentCellProperty, DataGridCellInfo.CreatePossiblyPartialCellInfo(newValue, currentCell.Column, dataGrid));
			}
			DataGrid.OnNotifyRowHeaderPropertyChanged(d, e);
		}

		// Token: 0x06006300 RID: 25344 RVA: 0x002A248E File Offset: 0x002A148E
		private void SetCurrentItem(object item)
		{
			if (item == DependencyProperty.UnsetValue)
			{
				item = null;
			}
			base.SetCurrentValueInternal(DataGrid.CurrentItemProperty, item);
		}

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x06006301 RID: 25345 RVA: 0x002A24A7 File Offset: 0x002A14A7
		// (set) Token: 0x06006302 RID: 25346 RVA: 0x002A24B9 File Offset: 0x002A14B9
		public DataGridColumn CurrentColumn
		{
			get
			{
				return (DataGridColumn)base.GetValue(DataGrid.CurrentColumnProperty);
			}
			set
			{
				base.SetValue(DataGrid.CurrentColumnProperty, value);
			}
		}

		// Token: 0x06006303 RID: 25347 RVA: 0x002A24C8 File Offset: 0x002A14C8
		private static void OnCurrentColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridCellInfo currentCell = dataGrid.CurrentCell;
			DataGridColumn dataGridColumn = (DataGridColumn)e.NewValue;
			if (currentCell.Column != dataGridColumn)
			{
				dataGrid.SetCurrentValueInternal(DataGrid.CurrentCellProperty, DataGridCellInfo.CreatePossiblyPartialCellInfo(currentCell.Item, dataGridColumn, dataGrid));
			}
		}

		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x06006304 RID: 25348 RVA: 0x002A2518 File Offset: 0x002A1518
		// (set) Token: 0x06006305 RID: 25349 RVA: 0x002A252A File Offset: 0x002A152A
		public DataGridCellInfo CurrentCell
		{
			get
			{
				return (DataGridCellInfo)base.GetValue(DataGrid.CurrentCellProperty);
			}
			set
			{
				base.SetValue(DataGrid.CurrentCellProperty, value);
			}
		}

		// Token: 0x06006306 RID: 25350 RVA: 0x002A2540 File Offset: 0x002A1540
		private static void OnCurrentCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridCellInfo dataGridCellInfo = (DataGridCellInfo)e.OldValue;
			DataGridCellInfo dataGridCellInfo2 = (DataGridCellInfo)e.NewValue;
			if (dataGrid.CurrentItem != dataGridCellInfo2.Item)
			{
				dataGrid.SetCurrentItem(dataGridCellInfo2.Item);
			}
			if (dataGrid.CurrentColumn != dataGridCellInfo2.Column)
			{
				dataGrid.SetCurrentValueInternal(DataGrid.CurrentColumnProperty, dataGridCellInfo2.Column);
			}
			if (dataGrid._currentCellContainer != null)
			{
				if ((dataGrid.IsAddingNewItem || dataGrid.IsEditingRowItem) && dataGridCellInfo.Item != dataGridCellInfo2.Item)
				{
					dataGrid.EndEdit(DataGrid.CommitEditCommand, dataGrid._currentCellContainer, DataGridEditingUnit.Row, true);
				}
				else if (dataGrid._currentCellContainer.IsEditing)
				{
					dataGrid.EndEdit(DataGrid.CommitEditCommand, dataGrid._currentCellContainer, DataGridEditingUnit.Cell, true);
				}
			}
			DataGridCell currentCellContainer = dataGrid._currentCellContainer;
			dataGrid._currentCellContainer = null;
			if (dataGridCellInfo2.IsValid && dataGrid.IsKeyboardFocusWithin)
			{
				DataGridCell dataGridCell = dataGrid._pendingCurrentCellContainer;
				if (dataGridCell == null)
				{
					dataGridCell = dataGrid.CurrentCellContainer;
					if (dataGridCell == null)
					{
						dataGrid.ScrollCellIntoView(dataGridCellInfo2.ItemInfo, dataGridCellInfo2.Column);
						dataGridCell = dataGrid.CurrentCellContainer;
					}
				}
				if (dataGridCell != null)
				{
					if (!dataGridCell.IsKeyboardFocusWithin)
					{
						dataGridCell.Focus();
					}
					if (currentCellContainer != dataGridCell)
					{
						if (currentCellContainer != null)
						{
							currentCellContainer.NotifyCurrentCellContainerChanged();
						}
						dataGridCell.NotifyCurrentCellContainerChanged();
					}
				}
				else if (currentCellContainer != null)
				{
					currentCellContainer.NotifyCurrentCellContainerChanged();
				}
			}
			dataGrid.OnCurrentCellChanged(EventArgs.Empty);
		}

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06006307 RID: 25351 RVA: 0x002A26A0 File Offset: 0x002A16A0
		// (remove) Token: 0x06006308 RID: 25352 RVA: 0x002A26D8 File Offset: 0x002A16D8
		public event EventHandler<EventArgs> CurrentCellChanged;

		// Token: 0x06006309 RID: 25353 RVA: 0x002A270D File Offset: 0x002A170D
		protected virtual void OnCurrentCellChanged(EventArgs e)
		{
			if (this.CurrentCellChanged != null)
			{
				this.CurrentCellChanged(this, e);
			}
		}

		// Token: 0x0600630A RID: 25354 RVA: 0x002A2724 File Offset: 0x002A1724
		private void UpdateCurrentCell(DataGridCell cell, bool isFocusWithinCell)
		{
			if (isFocusWithinCell)
			{
				this.CurrentCellContainer = cell;
				return;
			}
			if (!base.IsKeyboardFocusWithin)
			{
				this.CurrentCellContainer = null;
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x0600630B RID: 25355 RVA: 0x002A2740 File Offset: 0x002A1740
		// (set) Token: 0x0600630C RID: 25356 RVA: 0x002A2778 File Offset: 0x002A1778
		internal DataGridCell CurrentCellContainer
		{
			get
			{
				if (this._currentCellContainer == null)
				{
					DataGridCellInfo currentCell = this.CurrentCell;
					if (currentCell.IsValid)
					{
						this._currentCellContainer = this.TryFindCell(currentCell);
					}
				}
				return this._currentCellContainer;
			}
			set
			{
				if (this._currentCellContainer != value && (value == null || value != this._pendingCurrentCellContainer))
				{
					this._pendingCurrentCellContainer = value;
					if (value == null)
					{
						base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, DataGridCellInfo.Unset);
					}
					else
					{
						base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, new DataGridCellInfo(value));
					}
					this._pendingCurrentCellContainer = null;
					this._currentCellContainer = value;
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x0600630D RID: 25357 RVA: 0x002A27E4 File Offset: 0x002A17E4
		private bool IsEditingCurrentCell
		{
			get
			{
				DataGridCell currentCellContainer = this.CurrentCellContainer;
				return currentCellContainer != null && currentCellContainer.IsEditing;
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x0600630E RID: 25358 RVA: 0x002A2804 File Offset: 0x002A1804
		private bool IsCurrentCellReadOnly
		{
			get
			{
				DataGridCell currentCellContainer = this.CurrentCellContainer;
				return currentCellContainer != null && currentCellContainer.IsReadOnly;
			}
		}

		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x0600630F RID: 25359 RVA: 0x002A2824 File Offset: 0x002A1824
		internal ItemsControl.ItemInfo CurrentInfo
		{
			get
			{
				return base.LeaseItemInfo(this.CurrentCell.ItemInfo, false);
			}
		}

		// Token: 0x06006310 RID: 25360 RVA: 0x002A2848 File Offset: 0x002A1848
		internal bool IsCurrent(DataGridRow row, DataGridColumn column = null)
		{
			DataGridCellInfo dataGridCellInfo = this.CurrentCell;
			if (dataGridCellInfo.ItemInfo == null)
			{
				dataGridCellInfo = DataGridCellInfo.Unset;
			}
			DependencyObject container = dataGridCellInfo.ItemInfo.Container;
			int index = dataGridCellInfo.ItemInfo.Index;
			return (column == null || column == dataGridCellInfo.Column) && ((container != null && container == row) || (ItemsControl.EqualsEx(this.CurrentItem, row.Item) && (index < 0 || index == base.ItemContainerGenerator.IndexFromContainer(row))));
		}

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x06006311 RID: 25361 RVA: 0x002A28D0 File Offset: 0x002A18D0
		// (remove) Token: 0x06006312 RID: 25362 RVA: 0x002A2908 File Offset: 0x002A1908
		public event EventHandler<DataGridBeginningEditEventArgs> BeginningEdit;

		// Token: 0x06006313 RID: 25363 RVA: 0x002A2940 File Offset: 0x002A1940
		protected virtual void OnBeginningEdit(DataGridBeginningEditEventArgs e)
		{
			if (this.BeginningEdit != null)
			{
				this.BeginningEdit(this, e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationCellInvokeEvents(e.Column, e.Row);
				}
			}
		}

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x06006314 RID: 25364 RVA: 0x002A298C File Offset: 0x002A198C
		// (remove) Token: 0x06006315 RID: 25365 RVA: 0x002A29C4 File Offset: 0x002A19C4
		public event EventHandler<DataGridPreparingCellForEditEventArgs> PreparingCellForEdit;

		// Token: 0x06006316 RID: 25366 RVA: 0x002A29F9 File Offset: 0x002A19F9
		protected internal virtual void OnPreparingCellForEdit(DataGridPreparingCellForEditEventArgs e)
		{
			if (this.PreparingCellForEdit != null)
			{
				this.PreparingCellForEdit(this, e);
			}
		}

		// Token: 0x06006317 RID: 25367 RVA: 0x002A2A10 File Offset: 0x002A1A10
		public bool BeginEdit()
		{
			return this.BeginEdit(null);
		}

		// Token: 0x06006318 RID: 25368 RVA: 0x002A2A1C File Offset: 0x002A1A1C
		public bool BeginEdit(RoutedEventArgs editingEventArgs)
		{
			if (!this.IsReadOnly)
			{
				DataGridCell currentCellContainer = this.CurrentCellContainer;
				if (currentCellContainer != null)
				{
					if (!currentCellContainer.IsEditing && DataGrid.BeginEditCommand.CanExecute(editingEventArgs, currentCellContainer))
					{
						DataGrid.BeginEditCommand.Execute(editingEventArgs, currentCellContainer);
						currentCellContainer = this.CurrentCellContainer;
						if (currentCellContainer == null)
						{
							return false;
						}
					}
					return currentCellContainer.IsEditing;
				}
			}
			return false;
		}

		// Token: 0x06006319 RID: 25369 RVA: 0x002A2A71 File Offset: 0x002A1A71
		public bool CancelEdit()
		{
			if (this.IsEditingCurrentCell)
			{
				return this.CancelEdit(DataGridEditingUnit.Cell);
			}
			return (!this.IsEditingRowItem && !this.IsAddingNewItem) || this.CancelEdit(DataGridEditingUnit.Row);
		}

		// Token: 0x0600631A RID: 25370 RVA: 0x002A2A9C File Offset: 0x002A1A9C
		internal bool CancelEdit(DataGridCell cell)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			return currentCellContainer == null || currentCellContainer != cell || !currentCellContainer.IsEditing || this.CancelEdit(DataGridEditingUnit.Cell);
		}

		// Token: 0x0600631B RID: 25371 RVA: 0x002A2AC8 File Offset: 0x002A1AC8
		public bool CancelEdit(DataGridEditingUnit editingUnit)
		{
			return this.EndEdit(DataGrid.CancelEditCommand, this.CurrentCellContainer, editingUnit, true);
		}

		// Token: 0x0600631C RID: 25372 RVA: 0x002A2ADD File Offset: 0x002A1ADD
		private void CancelAnyEdit()
		{
			if (this.IsAddingNewItem || this.IsEditingRowItem)
			{
				this.CancelEdit(DataGridEditingUnit.Row);
				return;
			}
			if (this.IsEditingCurrentCell)
			{
				this.CancelEdit(DataGridEditingUnit.Cell);
			}
		}

		// Token: 0x0600631D RID: 25373 RVA: 0x002A2B08 File Offset: 0x002A1B08
		public bool CommitEdit()
		{
			if (this.IsEditingCurrentCell)
			{
				return this.CommitEdit(DataGridEditingUnit.Cell, true);
			}
			return (!this.IsEditingRowItem && !this.IsAddingNewItem) || this.CommitEdit(DataGridEditingUnit.Row, true);
		}

		// Token: 0x0600631E RID: 25374 RVA: 0x002A2B35 File Offset: 0x002A1B35
		public bool CommitEdit(DataGridEditingUnit editingUnit, bool exitEditingMode)
		{
			return this.EndEdit(DataGrid.CommitEditCommand, this.CurrentCellContainer, editingUnit, exitEditingMode);
		}

		// Token: 0x0600631F RID: 25375 RVA: 0x002A2B4A File Offset: 0x002A1B4A
		private bool CommitAnyEdit()
		{
			if (this.IsAddingNewItem || this.IsEditingRowItem)
			{
				return this.CommitEdit(DataGridEditingUnit.Row, true);
			}
			return !this.IsEditingCurrentCell || this.CommitEdit(DataGridEditingUnit.Cell, true);
		}

		// Token: 0x06006320 RID: 25376 RVA: 0x002A2B78 File Offset: 0x002A1B78
		private bool EndEdit(RoutedCommand command, DataGridCell cellContainer, DataGridEditingUnit editingUnit, bool exitEditMode)
		{
			bool flag = true;
			bool flag2 = true;
			if (cellContainer != null)
			{
				if (command.CanExecute(editingUnit, cellContainer))
				{
					command.Execute(editingUnit, cellContainer);
				}
				flag = !cellContainer.IsEditing;
				flag2 = (!this.IsEditingRowItem && !this.IsAddingNewItem);
			}
			if (!exitEditMode)
			{
				if (editingUnit != DataGridEditingUnit.Cell)
				{
					if (flag2)
					{
						object rowDataItem = cellContainer.RowDataItem;
						if (rowDataItem != null)
						{
							this.EditRowItem(rowDataItem);
							return this.IsEditingRowItem;
						}
					}
					return false;
				}
				if (cellContainer == null)
				{
					return false;
				}
				if (flag)
				{
					return this.BeginEdit(null);
				}
			}
			return flag && (editingUnit == DataGridEditingUnit.Cell || flag2);
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x06006321 RID: 25377 RVA: 0x002A2C06 File Offset: 0x002A1C06
		// (set) Token: 0x06006322 RID: 25378 RVA: 0x002A2C0E File Offset: 0x002A1C0E
		private bool HasCellValidationError
		{
			get
			{
				return this._hasCellValidationError;
			}
			set
			{
				if (this._hasCellValidationError != value)
				{
					this._hasCellValidationError = value;
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x06006323 RID: 25379 RVA: 0x002A2C25 File Offset: 0x002A1C25
		// (set) Token: 0x06006324 RID: 25380 RVA: 0x002A2C2D File Offset: 0x002A1C2D
		private bool HasRowValidationError
		{
			get
			{
				return this._hasRowValidationError;
			}
			set
			{
				if (this._hasRowValidationError != value)
				{
					this._hasRowValidationError = value;
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x06006325 RID: 25381 RVA: 0x002A2C44 File Offset: 0x002A1C44
		// (set) Token: 0x06006326 RID: 25382 RVA: 0x002A2C4C File Offset: 0x002A1C4C
		internal DataGridCell FocusedCell
		{
			get
			{
				return this._focusedCell;
			}
			set
			{
				if (this._focusedCell != value)
				{
					if (this._focusedCell != null)
					{
						this.UpdateCurrentCell(this._focusedCell, false);
					}
					this._focusedCell = value;
					if (this._focusedCell != null)
					{
						this.UpdateCurrentCell(this._focusedCell, true);
					}
				}
			}
		}

		// Token: 0x170016F6 RID: 5878
		// (get) Token: 0x06006327 RID: 25383 RVA: 0x002A2C88 File Offset: 0x002A1C88
		// (set) Token: 0x06006328 RID: 25384 RVA: 0x002A2C9A File Offset: 0x002A1C9A
		public bool CanUserAddRows
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserAddRowsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserAddRowsProperty, value);
			}
		}

		// Token: 0x06006329 RID: 25385 RVA: 0x002A2CA8 File Offset: 0x002A1CA8
		private static void OnCanUserAddRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).UpdateNewItemPlaceholder(false);
		}

		// Token: 0x0600632A RID: 25386 RVA: 0x002A2CB6 File Offset: 0x002A1CB6
		private static object OnCoerceCanUserAddRows(DependencyObject d, object baseValue)
		{
			return DataGrid.OnCoerceCanUserAddOrDeleteRows((DataGrid)d, (bool)baseValue, true);
		}

		// Token: 0x0600632B RID: 25387 RVA: 0x002A2CCF File Offset: 0x002A1CCF
		private static bool OnCoerceCanUserAddOrDeleteRows(DataGrid dataGrid, bool baseValue, bool canUserAddRowsProperty)
		{
			if (baseValue)
			{
				if (dataGrid.IsReadOnly || !dataGrid.IsEnabled)
				{
					return false;
				}
				if ((canUserAddRowsProperty && !dataGrid.EditableItems.CanAddNew) || (!canUserAddRowsProperty && !dataGrid.EditableItems.CanRemove))
				{
					return false;
				}
			}
			return baseValue;
		}

		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x0600632C RID: 25388 RVA: 0x002A2D09 File Offset: 0x002A1D09
		// (set) Token: 0x0600632D RID: 25389 RVA: 0x002A2D1B File Offset: 0x002A1D1B
		public bool CanUserDeleteRows
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserDeleteRowsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserDeleteRowsProperty, value);
			}
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x002A2D29 File Offset: 0x002A1D29
		private static void OnCanUserDeleteRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x0600632F RID: 25391 RVA: 0x002A2D30 File Offset: 0x002A1D30
		private static object OnCoerceCanUserDeleteRows(DependencyObject d, object baseValue)
		{
			return DataGrid.OnCoerceCanUserAddOrDeleteRows((DataGrid)d, (bool)baseValue, false);
		}

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x06006330 RID: 25392 RVA: 0x002A2D4C File Offset: 0x002A1D4C
		// (remove) Token: 0x06006331 RID: 25393 RVA: 0x002A2D84 File Offset: 0x002A1D84
		public event EventHandler<AddingNewItemEventArgs> AddingNewItem;

		// Token: 0x06006332 RID: 25394 RVA: 0x002A2DB9 File Offset: 0x002A1DB9
		protected virtual void OnAddingNewItem(AddingNewItemEventArgs e)
		{
			if (this.AddingNewItem != null)
			{
				this.AddingNewItem(this, e);
			}
		}

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x06006333 RID: 25395 RVA: 0x002A2DD0 File Offset: 0x002A1DD0
		// (remove) Token: 0x06006334 RID: 25396 RVA: 0x002A2E08 File Offset: 0x002A1E08
		public event InitializingNewItemEventHandler InitializingNewItem;

		// Token: 0x06006335 RID: 25397 RVA: 0x002A2E3D File Offset: 0x002A1E3D
		protected virtual void OnInitializingNewItem(InitializingNewItemEventArgs e)
		{
			if (this.InitializingNewItem != null)
			{
				this.InitializingNewItem(this, e);
			}
		}

		// Token: 0x06006336 RID: 25398 RVA: 0x002A2E54 File Offset: 0x002A1E54
		private object AddNewItem()
		{
			this.UpdateNewItemPlaceholder(true);
			object obj = null;
			IEditableCollectionViewAddNewItem items = base.Items;
			if (items.CanAddNewItem)
			{
				AddingNewItemEventArgs addingNewItemEventArgs = new AddingNewItemEventArgs();
				this.OnAddingNewItem(addingNewItemEventArgs);
				obj = addingNewItemEventArgs.NewItem;
			}
			obj = ((obj != null) ? items.AddNewItem(obj) : this.EditableItems.AddNew());
			if (obj != null)
			{
				this.OnInitializingNewItem(new InitializingNewItemEventArgs(obj));
			}
			CommandManager.InvalidateRequerySuggested();
			return obj;
		}

		// Token: 0x06006337 RID: 25399 RVA: 0x002A2EBA File Offset: 0x002A1EBA
		private void EditRowItem(object rowItem)
		{
			this.EditableItems.EditItem(rowItem);
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x06006338 RID: 25400 RVA: 0x002A2ECD File Offset: 0x002A1ECD
		private void CommitRowItem()
		{
			if (this.IsEditingRowItem)
			{
				this.EditableItems.CommitEdit();
				return;
			}
			this.EditableItems.CommitNew();
			this.UpdateNewItemPlaceholder(false);
		}

		// Token: 0x06006339 RID: 25401 RVA: 0x002A2EF8 File Offset: 0x002A1EF8
		private void CancelRowItem()
		{
			if (this.IsEditingRowItem)
			{
				if (this.EditableItems.CanCancelEdit)
				{
					this.EditableItems.CancelEdit();
					return;
				}
				this.EditableItems.CommitEdit();
				return;
			}
			else
			{
				object currentAddItem = this.EditableItems.CurrentAddItem;
				bool flag = currentAddItem == this.CurrentItem;
				bool flag2 = base.SelectedItems.Contains(currentAddItem);
				bool flag3 = false;
				List<int> list = null;
				int num = -1;
				if (flag2)
				{
					this.UnselectItem(base.NewItemInfo(currentAddItem, null, -1));
				}
				else
				{
					num = base.Items.IndexOf(currentAddItem);
					flag3 = (num >= 0 && this._selectedCells.Intersects(num, out list));
				}
				this.EditableItems.CancelNew();
				this.UpdateNewItemPlaceholder(false);
				if (flag)
				{
					this.SetCurrentItem(CollectionView.NewItemPlaceholder);
				}
				if (flag2)
				{
					this.SelectItem(base.NewItemInfo(CollectionView.NewItemPlaceholder, null, -1));
					return;
				}
				if (flag3)
				{
					using (this.UpdateSelectedCells())
					{
						int num2 = num;
						if (this.EditableItems.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
						{
							this._selectedCells.RemoveRegion(num, 0, 1, this.Columns.Count);
							num2--;
						}
						int i = 0;
						int count = list.Count;
						while (i < count)
						{
							this._selectedCells.AddRegion(num2, list[i], 1, list[i + 1]);
							i += 2;
						}
					}
				}
				return;
			}
		}

		// Token: 0x0600633A RID: 25402 RVA: 0x002A3064 File Offset: 0x002A2064
		private void UpdateRowEditing(DataGridCell cell)
		{
			object rowDataItem = cell.RowDataItem;
			if (!this.IsAddingOrEditingRowItem(rowDataItem))
			{
				cell.RowOwner.IsEditing = false;
				this._editingRowInfo = null;
			}
		}

		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x0600633B RID: 25403 RVA: 0x002A3094 File Offset: 0x002A2094
		private IEditableCollectionView EditableItems
		{
			get
			{
				return base.Items;
			}
		}

		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x0600633C RID: 25404 RVA: 0x002A309C File Offset: 0x002A209C
		private bool IsAddingNewItem
		{
			get
			{
				return this.EditableItems.IsAddingNew;
			}
		}

		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x0600633D RID: 25405 RVA: 0x002A30A9 File Offset: 0x002A20A9
		private bool IsEditingRowItem
		{
			get
			{
				return this.EditableItems.IsEditingItem;
			}
		}

		// Token: 0x0600633E RID: 25406 RVA: 0x002A30B6 File Offset: 0x002A20B6
		private bool IsAddingOrEditingRowItem(object item)
		{
			return this.IsEditingItem(item) || (this.IsAddingNewItem && this.EditableItems.CurrentAddItem == item);
		}

		// Token: 0x0600633F RID: 25407 RVA: 0x002A30DB File Offset: 0x002A20DB
		private bool IsAddingOrEditingRowItem(DataGridEditingUnit editingUnit, object item)
		{
			return editingUnit == DataGridEditingUnit.Row && this.IsAddingOrEditingRowItem(item);
		}

		// Token: 0x06006340 RID: 25408 RVA: 0x002A30EA File Offset: 0x002A20EA
		private bool IsEditingItem(object item)
		{
			return this.IsEditingRowItem && this.EditableItems.CurrentEditItem == item;
		}

		// Token: 0x06006341 RID: 25409 RVA: 0x002A3104 File Offset: 0x002A2104
		private void UpdateNewItemPlaceholder(bool isAddingNewItem)
		{
			IEditableCollectionView editableItems = this.EditableItems;
			bool flag = this.CanUserAddRows;
			if (DataGridHelper.IsDefaultValue(this, DataGrid.CanUserAddRowsProperty))
			{
				flag = DataGrid.OnCoerceCanUserAddOrDeleteRows(this, flag, true);
			}
			if (!isAddingNewItem)
			{
				if (flag)
				{
					if (editableItems.NewItemPlaceholderPosition == NewItemPlaceholderPosition.None)
					{
						editableItems.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtEnd;
					}
					this._placeholderVisibility = Visibility.Visible;
				}
				else
				{
					if (editableItems.NewItemPlaceholderPosition != NewItemPlaceholderPosition.None)
					{
						editableItems.NewItemPlaceholderPosition = NewItemPlaceholderPosition.None;
					}
					this._placeholderVisibility = Visibility.Collapsed;
				}
			}
			else
			{
				this._placeholderVisibility = Visibility.Collapsed;
			}
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(CollectionView.NewItemPlaceholder);
			if (dataGridRow != null)
			{
				dataGridRow.CoerceValue(UIElement.VisibilityProperty);
			}
		}

		// Token: 0x06006342 RID: 25410 RVA: 0x002A3198 File Offset: 0x002A2198
		private void SetCurrentItemToPlaceholder()
		{
			NewItemPlaceholderPosition newItemPlaceholderPosition = this.EditableItems.NewItemPlaceholderPosition;
			if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				int count = base.Items.Count;
				if (count > 0)
				{
					this.SetCurrentItem(base.Items[count - 1]);
					return;
				}
			}
			else if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning && base.Items.Count > 0)
			{
				this.SetCurrentItem(base.Items[0]);
			}
		}

		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x06006343 RID: 25411 RVA: 0x002A3200 File Offset: 0x002A2200
		private int DataItemsCount
		{
			get
			{
				int num = base.Items.Count;
				if (this.HasNewItemPlaceholder)
				{
					num--;
				}
				return num;
			}
		}

		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x06006344 RID: 25412 RVA: 0x002A3228 File Offset: 0x002A2228
		private int DataItemsSelected
		{
			get
			{
				int num = base.SelectedItems.Count;
				if (this.HasNewItemPlaceholder && base.SelectedItems.Contains(CollectionView.NewItemPlaceholder))
				{
					num--;
				}
				return num;
			}
		}

		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x06006345 RID: 25413 RVA: 0x002A3260 File Offset: 0x002A2260
		private bool HasNewItemPlaceholder
		{
			get
			{
				return this.EditableItems.NewItemPlaceholderPosition > NewItemPlaceholderPosition.None;
			}
		}

		// Token: 0x06006346 RID: 25414 RVA: 0x002A3270 File Offset: 0x002A2270
		private bool IsNewItemPlaceholder(object item)
		{
			return item == CollectionView.NewItemPlaceholder || item == DataGrid.NewItemPlaceholder;
		}

		// Token: 0x170016FE RID: 5886
		// (get) Token: 0x06006347 RID: 25415 RVA: 0x002A3284 File Offset: 0x002A2284
		// (set) Token: 0x06006348 RID: 25416 RVA: 0x002A3296 File Offset: 0x002A2296
		public DataGridRowDetailsVisibilityMode RowDetailsVisibilityMode
		{
			get
			{
				return (DataGridRowDetailsVisibilityMode)base.GetValue(DataGrid.RowDetailsVisibilityModeProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowDetailsVisibilityModeProperty, value);
			}
		}

		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x06006349 RID: 25417 RVA: 0x002A32A9 File Offset: 0x002A22A9
		// (set) Token: 0x0600634A RID: 25418 RVA: 0x002A32BB File Offset: 0x002A22BB
		public bool AreRowDetailsFrozen
		{
			get
			{
				return (bool)base.GetValue(DataGrid.AreRowDetailsFrozenProperty);
			}
			set
			{
				base.SetValue(DataGrid.AreRowDetailsFrozenProperty, value);
			}
		}

		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x0600634B RID: 25419 RVA: 0x002A32C9 File Offset: 0x002A22C9
		// (set) Token: 0x0600634C RID: 25420 RVA: 0x002A32DB File Offset: 0x002A22DB
		public DataTemplate RowDetailsTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGrid.RowDetailsTemplateProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowDetailsTemplateProperty, value);
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x0600634D RID: 25421 RVA: 0x002A32E9 File Offset: 0x002A22E9
		// (set) Token: 0x0600634E RID: 25422 RVA: 0x002A32FB File Offset: 0x002A22FB
		public DataTemplateSelector RowDetailsTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGrid.RowDetailsTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowDetailsTemplateSelectorProperty, value);
			}
		}

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x0600634F RID: 25423 RVA: 0x002A330C File Offset: 0x002A230C
		// (remove) Token: 0x06006350 RID: 25424 RVA: 0x002A3344 File Offset: 0x002A2344
		public event EventHandler<DataGridRowDetailsEventArgs> LoadingRowDetails;

		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x06006351 RID: 25425 RVA: 0x002A337C File Offset: 0x002A237C
		// (remove) Token: 0x06006352 RID: 25426 RVA: 0x002A33B4 File Offset: 0x002A23B4
		public event EventHandler<DataGridRowDetailsEventArgs> UnloadingRowDetails;

		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x06006353 RID: 25427 RVA: 0x002A33EC File Offset: 0x002A23EC
		// (remove) Token: 0x06006354 RID: 25428 RVA: 0x002A3424 File Offset: 0x002A2424
		public event EventHandler<DataGridRowDetailsEventArgs> RowDetailsVisibilityChanged;

		// Token: 0x06006355 RID: 25429 RVA: 0x002A345C File Offset: 0x002A245C
		internal void OnLoadingRowDetailsWrapper(DataGridRow row)
		{
			if (row != null && !row.DetailsLoaded && row.DetailsVisibility == Visibility.Visible && row.DetailsPresenter != null)
			{
				DataGridRowDetailsEventArgs e = new DataGridRowDetailsEventArgs(row, row.DetailsPresenter.DetailsElement);
				this.OnLoadingRowDetails(e);
				row.DetailsLoaded = true;
			}
		}

		// Token: 0x06006356 RID: 25430 RVA: 0x002A34A4 File Offset: 0x002A24A4
		internal void OnUnloadingRowDetailsWrapper(DataGridRow row)
		{
			if (row != null && row.DetailsLoaded && row.DetailsPresenter != null)
			{
				DataGridRowDetailsEventArgs e = new DataGridRowDetailsEventArgs(row, row.DetailsPresenter.DetailsElement);
				this.OnUnloadingRowDetails(e);
				row.DetailsLoaded = false;
			}
		}

		// Token: 0x06006357 RID: 25431 RVA: 0x002A34E4 File Offset: 0x002A24E4
		protected virtual void OnLoadingRowDetails(DataGridRowDetailsEventArgs e)
		{
			if (this.LoadingRowDetails != null)
			{
				this.LoadingRowDetails(this, e);
			}
		}

		// Token: 0x06006358 RID: 25432 RVA: 0x002A34FB File Offset: 0x002A24FB
		protected virtual void OnUnloadingRowDetails(DataGridRowDetailsEventArgs e)
		{
			if (this.UnloadingRowDetails != null)
			{
				this.UnloadingRowDetails(this, e);
			}
		}

		// Token: 0x06006359 RID: 25433 RVA: 0x002A3514 File Offset: 0x002A2514
		protected internal virtual void OnRowDetailsVisibilityChanged(DataGridRowDetailsEventArgs e)
		{
			if (this.RowDetailsVisibilityChanged != null)
			{
				this.RowDetailsVisibilityChanged(this, e);
			}
			DataGridRow row = e.Row;
			this.OnLoadingRowDetailsWrapper(row);
		}

		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x0600635A RID: 25434 RVA: 0x002A3544 File Offset: 0x002A2544
		// (set) Token: 0x0600635B RID: 25435 RVA: 0x002A3556 File Offset: 0x002A2556
		public bool CanUserResizeRows
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserResizeRowsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserResizeRowsProperty, value);
			}
		}

		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x0600635C RID: 25436 RVA: 0x002A3564 File Offset: 0x002A2564
		// (set) Token: 0x0600635D RID: 25437 RVA: 0x002A3576 File Offset: 0x002A2576
		public Thickness NewItemMargin
		{
			get
			{
				return (Thickness)base.GetValue(DataGrid.NewItemMarginProperty);
			}
			private set
			{
				base.SetValue(DataGrid.NewItemMarginPropertyKey, value);
			}
		}

		// Token: 0x0600635E RID: 25438 RVA: 0x002A3589 File Offset: 0x002A2589
		private void EnqueueNewItemMarginComputation()
		{
			if (!this._newItemMarginComputationPending)
			{
				this._newItemMarginComputationPending = true;
				base.Dispatcher.BeginInvoke(new Action(delegate()
				{
					double left = 0.0;
					if (base.IsGrouping && this.InternalScrollHost != null)
					{
						ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot;
						while (containerTracking != null)
						{
							DataGridRow container = containerTracking.Container;
							if (!container.IsNewItem)
							{
								GeneralTransform generalTransform = container.TransformToAncestor(this.InternalScrollHost);
								if (generalTransform != null)
								{
									left = generalTransform.Transform(default(Point)).X;
									break;
								}
								break;
							}
							else
							{
								containerTracking = containerTracking.Next;
							}
						}
					}
					this.NewItemMargin = new Thickness(left, 0.0, 0.0, 0.0);
					this._newItemMarginComputationPending = false;
				}), DispatcherPriority.Input, Array.Empty<object>());
			}
		}

		// Token: 0x0600635F RID: 25439 RVA: 0x002A35B8 File Offset: 0x002A25B8
		internal override void OnIsGroupingChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsGroupingChanged(e);
			this.EnqueueNewItemMarginComputation();
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x06006360 RID: 25440 RVA: 0x002A35C7 File Offset: 0x002A25C7
		internal SelectedItemCollection SelectedItemCollection
		{
			get
			{
				return (SelectedItemCollection)base.SelectedItems;
			}
		}

		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x06006361 RID: 25441 RVA: 0x002A35D4 File Offset: 0x002A25D4
		public IList<DataGridCellInfo> SelectedCells
		{
			get
			{
				return this._selectedCells;
			}
		}

		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x06006362 RID: 25442 RVA: 0x002A35D4 File Offset: 0x002A25D4
		internal SelectedCellsCollection SelectedCellsInternal
		{
			get
			{
				return this._selectedCells;
			}
		}

		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x06006363 RID: 25443 RVA: 0x002A35DC File Offset: 0x002A25DC
		// (remove) Token: 0x06006364 RID: 25444 RVA: 0x002A3614 File Offset: 0x002A2614
		public event SelectedCellsChangedEventHandler SelectedCellsChanged;

		// Token: 0x06006365 RID: 25445 RVA: 0x002A364C File Offset: 0x002A264C
		internal void OnSelectedCellsChanged(NotifyCollectionChangedAction action, VirtualizedCellInfoCollection oldItems, VirtualizedCellInfoCollection newItems)
		{
			DataGridSelectionMode selectionMode = this.SelectionMode;
			DataGridSelectionUnit selectionUnit = this.SelectionUnit;
			if (!this.IsUpdatingSelectedCells && selectionUnit == DataGridSelectionUnit.FullRow)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			if (oldItems != null)
			{
				if (this._pendingSelectedCells != null)
				{
					VirtualizedCellInfoCollection.Xor(this._pendingSelectedCells, oldItems);
				}
				if (this._pendingUnselectedCells == null)
				{
					this._pendingUnselectedCells = oldItems;
				}
				else
				{
					this._pendingUnselectedCells.Union(oldItems);
				}
			}
			if (newItems != null)
			{
				if (this._pendingUnselectedCells != null)
				{
					VirtualizedCellInfoCollection.Xor(this._pendingUnselectedCells, newItems);
				}
				if (this._pendingSelectedCells == null)
				{
					this._pendingSelectedCells = newItems;
				}
				else
				{
					this._pendingSelectedCells.Union(newItems);
				}
			}
			if (!this.IsUpdatingSelectedCells)
			{
				using (this.UpdateSelectedCells())
				{
					if (selectionMode == DataGridSelectionMode.Single && action == NotifyCollectionChangedAction.Add && this._selectedCells.Count > 1)
					{
						this._selectedCells.RemoveAllButOne(newItems[0]);
					}
					else if (action == NotifyCollectionChangedAction.Remove && oldItems != null && selectionUnit == DataGridSelectionUnit.CellOrRowHeader)
					{
						bool isUpdatingSelectedItems = base.IsUpdatingSelectedItems;
						if (!isUpdatingSelectedItems)
						{
							base.BeginUpdateSelectedItems();
						}
						try
						{
							object obj = null;
							foreach (DataGridCellInfo dataGridCellInfo in oldItems)
							{
								object item = dataGridCellInfo.Item;
								if (item != obj)
								{
									obj = item;
									if (base.SelectedItems.Contains(item))
									{
										base.SelectedItems.Remove(item);
									}
								}
							}
						}
						finally
						{
							if (!isUpdatingSelectedItems)
							{
								base.EndUpdateSelectedItems();
							}
						}
					}
				}
			}
		}

		// Token: 0x06006366 RID: 25446 RVA: 0x002A37E0 File Offset: 0x002A27E0
		private void NotifySelectedCellsChanged()
		{
			if ((this._pendingSelectedCells != null && this._pendingSelectedCells.Count > 0) || (this._pendingUnselectedCells != null && this._pendingUnselectedCells.Count > 0))
			{
				SelectedCellsChangedEventArgs e = new SelectedCellsChangedEventArgs(this, this._pendingSelectedCells, this._pendingUnselectedCells);
				int count = this._selectedCells.Count;
				int num = (this._pendingUnselectedCells != null) ? this._pendingUnselectedCells.Count : 0;
				int num2 = (this._pendingSelectedCells != null) ? this._pendingSelectedCells.Count : 0;
				bool flag = count - num2 + num != 0;
				this._pendingSelectedCells = null;
				this._pendingUnselectedCells = null;
				this.OnSelectedCellsChanged(e);
				if (!flag || count == 0)
				{
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		// Token: 0x06006367 RID: 25447 RVA: 0x002A388C File Offset: 0x002A288C
		protected virtual void OnSelectedCellsChanged(SelectedCellsChangedEventArgs e)
		{
			if (this.SelectedCellsChanged != null)
			{
				this.SelectedCellsChanged(this, e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationCellSelectedEvent(e);
				}
			}
		}

		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x06006368 RID: 25448 RVA: 0x002A38DC File Offset: 0x002A28DC
		public static RoutedUICommand SelectAllCommand
		{
			get
			{
				return ApplicationCommands.SelectAll;
			}
		}

		// Token: 0x06006369 RID: 25449 RVA: 0x002A38E4 File Offset: 0x002A28E4
		private static void OnCanExecuteSelectAll(object sender, CanExecuteRoutedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)sender;
			e.CanExecute = (dataGrid.SelectionMode == DataGridSelectionMode.Extended && dataGrid.IsEnabled);
			e.Handled = true;
		}

		// Token: 0x0600636A RID: 25450 RVA: 0x002A3918 File Offset: 0x002A2918
		private static void OnExecutedSelectAll(object sender, ExecutedRoutedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)sender;
			if (dataGrid.SelectionUnit == DataGridSelectionUnit.Cell)
			{
				dataGrid.SelectAllCells();
			}
			else
			{
				dataGrid.SelectAll();
			}
			e.Handled = true;
		}

		// Token: 0x0600636B RID: 25451 RVA: 0x002A394C File Offset: 0x002A294C
		internal override void SelectAllImpl()
		{
			int count = base.Items.Count;
			int count2 = this._columns.Count;
			if (count2 > 0 && count > 0)
			{
				using (this.UpdateSelectedCells())
				{
					this._selectedCells.AddRegion(0, 0, count, count2);
					base.SelectAllImpl();
				}
			}
		}

		// Token: 0x0600636C RID: 25452 RVA: 0x002A39B4 File Offset: 0x002A29B4
		internal void SelectOnlyThisCell(DataGridCellInfo currentCellInfo)
		{
			using (this.UpdateSelectedCells())
			{
				this._selectedCells.Clear();
				this._selectedCells.Add(currentCellInfo);
			}
		}

		// Token: 0x0600636D RID: 25453 RVA: 0x002A39FC File Offset: 0x002A29FC
		public void SelectAllCells()
		{
			if (this.SelectionUnit == DataGridSelectionUnit.FullRow)
			{
				base.SelectAll();
				return;
			}
			int count = base.Items.Count;
			int count2 = this._columns.Count;
			if (count > 0 && count2 > 0)
			{
				using (this.UpdateSelectedCells())
				{
					if (this._selectedCells.Count > 0)
					{
						this._selectedCells.Clear();
					}
					this._selectedCells.AddRegion(0, 0, count, count2);
				}
			}
		}

		// Token: 0x0600636E RID: 25454 RVA: 0x002A3A84 File Offset: 0x002A2A84
		public void UnselectAllCells()
		{
			using (this.UpdateSelectedCells())
			{
				this._selectedCells.Clear();
				if (this.SelectionUnit != DataGridSelectionUnit.Cell)
				{
					base.UnselectAll();
				}
			}
		}

		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x0600636F RID: 25455 RVA: 0x002A3AD0 File Offset: 0x002A2AD0
		// (set) Token: 0x06006370 RID: 25456 RVA: 0x002A3AE2 File Offset: 0x002A2AE2
		public DataGridSelectionMode SelectionMode
		{
			get
			{
				return (DataGridSelectionMode)base.GetValue(DataGrid.SelectionModeProperty);
			}
			set
			{
				base.SetValue(DataGrid.SelectionModeProperty, value);
			}
		}

		// Token: 0x06006371 RID: 25457 RVA: 0x002A3AF8 File Offset: 0x002A2AF8
		private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridSelectionMode dataGridSelectionMode = (DataGridSelectionMode)e.NewValue;
			bool flag = dataGridSelectionMode == DataGridSelectionMode.Single;
			DataGridSelectionUnit selectionUnit = dataGrid.SelectionUnit;
			if (flag && selectionUnit == DataGridSelectionUnit.Cell)
			{
				using (dataGrid.UpdateSelectedCells())
				{
					dataGrid._selectedCells.RemoveAllButOne();
				}
			}
			dataGrid.CanSelectMultipleItems = (dataGridSelectionMode > DataGridSelectionMode.Single);
			if (flag && selectionUnit == DataGridSelectionUnit.CellOrRowHeader)
			{
				if (dataGrid.SelectedItems.Count > 0)
				{
					using (dataGrid.UpdateSelectedCells())
					{
						dataGrid._selectedCells.RemoveAllButOneRow(dataGrid.InternalSelectedInfo.Index);
						return;
					}
				}
				using (dataGrid.UpdateSelectedCells())
				{
					dataGrid._selectedCells.RemoveAllButOne();
				}
			}
		}

		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x06006372 RID: 25458 RVA: 0x002A3BE0 File Offset: 0x002A2BE0
		// (set) Token: 0x06006373 RID: 25459 RVA: 0x002A3BF2 File Offset: 0x002A2BF2
		public DataGridSelectionUnit SelectionUnit
		{
			get
			{
				return (DataGridSelectionUnit)base.GetValue(DataGrid.SelectionUnitProperty);
			}
			set
			{
				base.SetValue(DataGrid.SelectionUnitProperty, value);
			}
		}

		// Token: 0x06006374 RID: 25460 RVA: 0x002A3C08 File Offset: 0x002A2C08
		private static void OnSelectionUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridSelectionUnit dataGridSelectionUnit = (DataGridSelectionUnit)e.OldValue;
			if (dataGridSelectionUnit != DataGridSelectionUnit.Cell)
			{
				dataGrid.UnselectAll();
			}
			if (dataGridSelectionUnit != DataGridSelectionUnit.FullRow)
			{
				using (dataGrid.UpdateSelectedCells())
				{
					dataGrid._selectedCells.Clear();
				}
			}
			dataGrid.CoerceValue(Selector.IsSynchronizedWithCurrentItemProperty);
		}

		// Token: 0x06006375 RID: 25461 RVA: 0x002A3C70 File Offset: 0x002A2C70
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			if (!this.IsUpdatingSelectedCells)
			{
				using (this.UpdateSelectedCells())
				{
					int count = e.RemovedInfos.Count;
					for (int i = 0; i < count; i++)
					{
						ItemsControl.ItemInfo rowInfo = e.RemovedInfos[i];
						this.UpdateSelectionOfCellsInRow(rowInfo, false);
					}
					count = e.AddedInfos.Count;
					for (int j = 0; j < count; j++)
					{
						ItemsControl.ItemInfo rowInfo2 = e.AddedInfos[j];
						this.UpdateSelectionOfCellsInRow(rowInfo2, true);
					}
				}
			}
			CommandManager.InvalidateRequerySuggested();
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationSelectionEvents(e);
				}
			}
			base.OnSelectionChanged(e);
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x002A3D48 File Offset: 0x002A2D48
		private void UpdateIsSelected()
		{
			this.UpdateIsSelected(this._pendingUnselectedCells, false);
			this.UpdateIsSelected(this._pendingSelectedCells, true);
		}

		// Token: 0x06006377 RID: 25463 RVA: 0x002A3D64 File Offset: 0x002A2D64
		private void UpdateIsSelected(VirtualizedCellInfoCollection cells, bool isSelected)
		{
			if (cells != null)
			{
				int count = cells.Count;
				if (count > 0)
				{
					bool flag = false;
					if (count > 750)
					{
						int num = 0;
						int count2 = this._columns.Count;
						for (ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
						{
							num += count2;
							if (num >= count)
							{
								break;
							}
						}
						flag = (count > num);
					}
					if (flag)
					{
						for (ContainerTracking<DataGridRow> containerTracking2 = this._rowTrackingRoot; containerTracking2 != null; containerTracking2 = containerTracking2.Next)
						{
							DataGridCellsPresenter cellsPresenter = containerTracking2.Container.CellsPresenter;
							if (cellsPresenter != null)
							{
								for (ContainerTracking<DataGridCell> containerTracking3 = cellsPresenter.CellTrackingRoot; containerTracking3 != null; containerTracking3 = containerTracking3.Next)
								{
									DataGridCell container = containerTracking3.Container;
									DataGridCellInfo cell = new DataGridCellInfo(container);
									if (cells.Contains(cell))
									{
										container.SyncIsSelected(isSelected);
									}
								}
							}
						}
						return;
					}
					foreach (DataGridCellInfo info in cells)
					{
						DataGridCell dataGridCell = this.TryFindCell(info);
						if (dataGridCell != null)
						{
							dataGridCell.SyncIsSelected(isSelected);
						}
					}
				}
			}
		}

		// Token: 0x06006378 RID: 25464 RVA: 0x002A3E7C File Offset: 0x002A2E7C
		private void UpdateSelectionOfCellsInRow(ItemsControl.ItemInfo rowInfo, bool isSelected)
		{
			int count = this._columns.Count;
			if (count > 0)
			{
				if (!isSelected && this._pendingInfos != null)
				{
					this._pendingInfos.Remove(rowInfo);
				}
				int index = rowInfo.Index;
				if (index >= 0)
				{
					if (isSelected)
					{
						this._selectedCells.AddRegion(index, 0, 1, count);
						return;
					}
					this._selectedCells.RemoveRegion(index, 0, 1, count);
					return;
				}
				else if (isSelected)
				{
					this.EnsurePendingInfos();
					this._pendingInfos.Add(rowInfo);
				}
			}
		}

		// Token: 0x06006379 RID: 25465 RVA: 0x002A3EF4 File Offset: 0x002A2EF4
		private void EnsurePendingInfos()
		{
			if (this._pendingInfos == null)
			{
				this._pendingInfos = new List<ItemsControl.ItemInfo>();
			}
		}

		// Token: 0x0600637A RID: 25466 RVA: 0x002A3F0C File Offset: 0x002A2F0C
		internal void CellIsSelectedChanged(DataGridCell cell, bool isSelected)
		{
			if (!this.IsUpdatingSelectedCells)
			{
				DataGridCellInfo cell2 = new DataGridCellInfo(cell);
				if (isSelected)
				{
					this._selectedCells.AddValidatedCell(cell2);
					return;
				}
				if (this._selectedCells.Contains(cell2))
				{
					this._selectedCells.Remove(cell2);
				}
			}
		}

		// Token: 0x0600637B RID: 25467 RVA: 0x002A3F54 File Offset: 0x002A2F54
		internal void HandleSelectionForCellInput(DataGridCell cell, bool startDragging, bool allowsExtendSelect, bool allowsMinimalSelect)
		{
			if (this.SelectionUnit == DataGridSelectionUnit.FullRow)
			{
				this.MakeFullRowSelection(base.ItemInfoFromContainer(cell.RowOwner), allowsExtendSelect, allowsMinimalSelect);
			}
			else
			{
				this.MakeCellSelection(new DataGridCellInfo(cell), allowsExtendSelect, allowsMinimalSelect);
			}
			if (startDragging)
			{
				this.BeginDragging();
			}
		}

		// Token: 0x0600637C RID: 25468 RVA: 0x002A3F90 File Offset: 0x002A2F90
		internal void HandleSelectionForRowHeaderAndDetailsInput(DataGridRow row, bool startDragging)
		{
			ItemsControl.ItemInfo itemInfo = base.ItemInfoFromContainer(row);
			if (!this._isDraggingSelection && this._columns.Count > 0)
			{
				if (!base.IsKeyboardFocusWithin)
				{
					base.Focus();
				}
				if (this.CurrentCell.ItemInfo != itemInfo)
				{
					base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, new DataGridCellInfo(itemInfo, this.ColumnFromDisplayIndex(0), this));
				}
				else if (this._currentCellContainer != null && this._currentCellContainer.IsEditing)
				{
					this.EndEdit(DataGrid.CommitEditCommand, this._currentCellContainer, DataGridEditingUnit.Cell, true);
				}
			}
			if (this.CanSelectRows)
			{
				this.MakeFullRowSelection(itemInfo, true, true);
				if (startDragging)
				{
					this.BeginRowDragging();
				}
			}
		}

		// Token: 0x0600637D RID: 25469 RVA: 0x002A4043 File Offset: 0x002A3043
		private void BeginRowDragging()
		{
			this.BeginDragging();
			this._isRowDragging = true;
		}

		// Token: 0x0600637E RID: 25470 RVA: 0x002A4052 File Offset: 0x002A3052
		private void BeginDragging()
		{
			if (Mouse.Capture(this, CaptureMode.SubTree))
			{
				this._isDraggingSelection = true;
				this._dragPoint = Mouse.GetPosition(this);
			}
		}

		// Token: 0x0600637F RID: 25471 RVA: 0x002A4070 File Offset: 0x002A3070
		private void EndDragging()
		{
			this.StopAutoScroll();
			if (Mouse.Captured == this)
			{
				base.ReleaseMouseCapture();
			}
			this._isDraggingSelection = false;
			this._isRowDragging = false;
		}

		// Token: 0x06006380 RID: 25472 RVA: 0x002A4094 File Offset: 0x002A3094
		private void MakeFullRowSelection(ItemsControl.ItemInfo info, bool allowsExtendSelect, bool allowsMinimalSelect)
		{
			bool flag = allowsExtendSelect && this.ShouldExtendSelection;
			bool flag2 = allowsMinimalSelect && DataGrid.ShouldMinimallyModifySelection;
			using (this.UpdateSelectedCells())
			{
				bool isUpdatingSelectedItems = base.IsUpdatingSelectedItems;
				if (!isUpdatingSelectedItems)
				{
					base.BeginUpdateSelectedItems();
				}
				try
				{
					if (flag)
					{
						if (this._columns.Count > 0)
						{
							int num = this._selectionAnchor.Value.ItemInfo.Index;
							int num2 = info.Index;
							if (num > num2)
							{
								int num3 = num;
								num = num2;
								num2 = num3;
							}
							if (num >= 0 && num2 >= 0)
							{
								int count = this._selectedItems.Count;
								if (!flag2)
								{
									bool flag3 = false;
									for (int i = 0; i < count; i++)
									{
										ItemsControl.ItemInfo itemInfo = this._selectedItems[i];
										int index = itemInfo.Index;
										if (index < num || num2 < index)
										{
											base.SelectionChange.Unselect(itemInfo);
											if (!flag3)
											{
												this._selectedCells.Clear();
												flag3 = true;
											}
										}
									}
								}
								else
								{
									int index2 = this.CurrentCell.ItemInfo.Index;
									int num4 = -1;
									int num5 = -1;
									if (index2 < num)
									{
										num4 = index2;
										num5 = num - 1;
									}
									else if (index2 > num2)
									{
										num4 = num2 + 1;
										num5 = index2;
									}
									if (num4 >= 0 && num5 >= 0)
									{
										for (int j = 0; j < count; j++)
										{
											ItemsControl.ItemInfo itemInfo2 = this._selectedItems[j];
											int index3 = itemInfo2.Index;
											if (num4 <= index3 && index3 <= num5)
											{
												base.SelectionChange.Unselect(itemInfo2);
											}
										}
										this._selectedCells.RemoveRegion(num4, 0, num5 - num4 + 1, this.Columns.Count);
									}
								}
								IEnumerator enumerator = ((IEnumerable)base.Items).GetEnumerator();
								int num6 = 0;
								while (num6 <= num2 && enumerator.MoveNext())
								{
									if (num6 >= num)
									{
										base.SelectionChange.Select(base.ItemInfoFromIndex(num6), true);
									}
									num6++;
								}
								IDisposable disposable2 = enumerator as IDisposable;
								if (disposable2 != null)
								{
									disposable2.Dispose();
								}
								this._selectedCells.AddRegion(num, 0, num2 - num + 1, this._columns.Count);
							}
						}
					}
					else
					{
						if (flag2 && this._selectedItems.Contains(info))
						{
							this.UnselectItem(info);
						}
						else
						{
							if (!flag2 || !base.CanSelectMultipleItems)
							{
								if (this._selectedCells.Count > 0)
								{
									this._selectedCells.Clear();
								}
								if (base.SelectedItems.Count > 0)
								{
									base.SelectedItems.Clear();
								}
							}
							if (this._editingRowInfo == info)
							{
								int count2 = this._columns.Count;
								if (count2 > 0)
								{
									this._selectedCells.AddRegion(this._editingRowInfo.Index, 0, 1, count2);
								}
								this.SelectItem(info, false);
							}
							else
							{
								this.SelectItem(info);
							}
						}
						this._selectionAnchor = new DataGridCellInfo?(new DataGridCellInfo(info.Clone(), this.ColumnFromDisplayIndex(0), this));
					}
				}
				finally
				{
					if (!isUpdatingSelectedItems)
					{
						base.EndUpdateSelectedItems();
					}
				}
			}
		}

		// Token: 0x06006381 RID: 25473 RVA: 0x002A43CC File Offset: 0x002A33CC
		private void MakeCellSelection(DataGridCellInfo cellInfo, bool allowsExtendSelect, bool allowsMinimalSelect)
		{
			bool flag = allowsExtendSelect && this.ShouldExtendSelection;
			bool flag2 = allowsMinimalSelect && DataGrid.ShouldMinimallyModifySelection;
			using (this.UpdateSelectedCells())
			{
				int displayIndex = cellInfo.Column.DisplayIndex;
				if (flag)
				{
					ItemCollection items = base.Items;
					int index = this._selectionAnchor.Value.ItemInfo.Index;
					int index2 = cellInfo.ItemInfo.Index;
					int displayIndex2 = this._selectionAnchor.Value.Column.DisplayIndex;
					int num = displayIndex;
					if (index >= 0 && index2 >= 0 && displayIndex2 >= 0 && num >= 0)
					{
						int num2 = Math.Abs(index2 - index) + 1;
						int num3 = Math.Abs(num - displayIndex2) + 1;
						if (!flag2)
						{
							if (base.SelectedItems.Count > 0)
							{
								base.UnselectAll();
							}
							this._selectedCells.Clear();
						}
						else
						{
							int index3 = this.CurrentCell.ItemInfo.Index;
							int displayIndex3 = this.CurrentCell.Column.DisplayIndex;
							int num4 = Math.Min(index, index3);
							int num5 = Math.Abs(index3 - index) + 1;
							int columnIndex = Math.Min(displayIndex2, displayIndex3);
							int num6 = Math.Abs(displayIndex3 - displayIndex2) + 1;
							this._selectedCells.RemoveRegion(num4, columnIndex, num5, num6);
							if (this.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader)
							{
								int num7 = num4;
								int num8 = num4 + num5 - 1;
								if (num6 <= num3)
								{
									if (num5 > num2)
									{
										int num9 = num5 - num2;
										num7 = ((num4 == index3) ? index3 : (index3 - num9 + 1));
										num8 = num7 + num9 - 1;
									}
									else
									{
										num8 = num7 - 1;
									}
								}
								for (int i = num7; i <= num8; i++)
								{
									object value = base.Items[i];
									if (base.SelectedItems.Contains(value))
									{
										base.SelectedItems.Remove(value);
									}
								}
							}
						}
						this._selectedCells.AddRegion(Math.Min(index, index2), Math.Min(displayIndex2, num), num2, num3);
					}
				}
				else
				{
					bool flag3 = this._selectedCells.Contains(cellInfo);
					bool flag4 = this._editingRowInfo != null && this._editingRowInfo.Index == cellInfo.ItemInfo.Index;
					if (!flag3 && flag4)
					{
						flag3 = this._selectedCells.Contains(this._editingRowInfo.Index, displayIndex);
					}
					if (flag2 && flag3)
					{
						if (flag4)
						{
							this._selectedCells.RemoveRegion(this._editingRowInfo.Index, displayIndex, 1, 1);
						}
						else
						{
							this._selectedCells.Remove(cellInfo);
						}
						if (this.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader && base.SelectedItems.Contains(cellInfo.Item))
						{
							base.SelectedItems.Remove(cellInfo.Item);
						}
					}
					else
					{
						if (!flag2 || !base.CanSelectMultipleItems)
						{
							if (base.SelectedItems.Count > 0)
							{
								base.UnselectAll();
							}
							this._selectedCells.Clear();
						}
						if (flag4)
						{
							this._selectedCells.AddRegion(this._editingRowInfo.Index, displayIndex, 1, 1);
						}
						else
						{
							this._selectedCells.AddValidatedCell(cellInfo);
						}
					}
					this._selectionAnchor = new DataGridCellInfo?(new DataGridCellInfo(cellInfo));
				}
			}
		}

		// Token: 0x06006382 RID: 25474 RVA: 0x002A4730 File Offset: 0x002A3730
		private void SelectItem(ItemsControl.ItemInfo info)
		{
			this.SelectItem(info, true);
		}

		// Token: 0x06006383 RID: 25475 RVA: 0x002A473C File Offset: 0x002A373C
		private void SelectItem(ItemsControl.ItemInfo info, bool selectCells)
		{
			if (selectCells)
			{
				using (this.UpdateSelectedCells())
				{
					int index = info.Index;
					int count = this._columns.Count;
					if (index >= 0 && count > 0)
					{
						this._selectedCells.AddRegion(index, 0, 1, count);
					}
				}
			}
			this.UpdateSelectedItems(info, true);
		}

		// Token: 0x06006384 RID: 25476 RVA: 0x002A47A4 File Offset: 0x002A37A4
		private void UnselectItem(ItemsControl.ItemInfo info)
		{
			using (this.UpdateSelectedCells())
			{
				int index = info.Index;
				int count = this._columns.Count;
				if (index >= 0 && count > 0)
				{
					this._selectedCells.RemoveRegion(index, 0, 1, count);
				}
			}
			this.UpdateSelectedItems(info, false);
		}

		// Token: 0x06006385 RID: 25477 RVA: 0x002A4808 File Offset: 0x002A3808
		private void UpdateSelectedItems(ItemsControl.ItemInfo info, bool add)
		{
			bool isUpdatingSelectedItems = base.IsUpdatingSelectedItems;
			if (!isUpdatingSelectedItems)
			{
				base.BeginUpdateSelectedItems();
			}
			try
			{
				if (add)
				{
					this.SelectedItemCollection.Add(info.Clone());
				}
				else
				{
					this.SelectedItemCollection.Remove(info);
				}
			}
			finally
			{
				if (!isUpdatingSelectedItems)
				{
					base.EndUpdateSelectedItems();
				}
			}
		}

		// Token: 0x06006386 RID: 25478 RVA: 0x002A4864 File Offset: 0x002A3864
		private IDisposable UpdateSelectedCells()
		{
			return new DataGrid.ChangingSelectedCellsHelper(this);
		}

		// Token: 0x06006387 RID: 25479 RVA: 0x002A486C File Offset: 0x002A386C
		private void BeginUpdateSelectedCells()
		{
			this._updatingSelectedCells = true;
		}

		// Token: 0x06006388 RID: 25480 RVA: 0x002A4875 File Offset: 0x002A3875
		private void EndUpdateSelectedCells()
		{
			this.UpdateIsSelected();
			this._updatingSelectedCells = false;
			this.NotifySelectedCellsChanged();
		}

		// Token: 0x1700170A RID: 5898
		// (get) Token: 0x06006389 RID: 25481 RVA: 0x002A488A File Offset: 0x002A388A
		private bool IsUpdatingSelectedCells
		{
			get
			{
				return this._updatingSelectedCells;
			}
		}

		// Token: 0x1700170B RID: 5899
		// (get) Token: 0x0600638A RID: 25482 RVA: 0x002A4892 File Offset: 0x002A3892
		private bool ShouldExtendSelection
		{
			get
			{
				return base.CanSelectMultipleItems && this._selectionAnchor != null && (this._isDraggingSelection || (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
			}
		}

		// Token: 0x1700170C RID: 5900
		// (get) Token: 0x0600638B RID: 25483 RVA: 0x002A48BF File Offset: 0x002A38BF
		private static bool ShouldMinimallyModifySelection
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			}
		}

		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x0600638C RID: 25484 RVA: 0x002A48CC File Offset: 0x002A38CC
		private bool CanSelectRows
		{
			get
			{
				DataGridSelectionUnit selectionUnit = this.SelectionUnit;
				return selectionUnit != DataGridSelectionUnit.Cell && selectionUnit - DataGridSelectionUnit.FullRow <= 1;
			}
		}

		// Token: 0x0600638D RID: 25485 RVA: 0x002A48F0 File Offset: 0x002A38F0
		private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this._currentCellContainer = null;
			List<Tuple<int, int>> ranges = null;
			using (this.UpdateSelectedCells())
			{
				if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					ranges = new List<Tuple<int, int>>();
					base.LocateSelectedItems(ranges, false);
				}
				this._selectedCells.OnItemsCollectionChanged(e, ranges);
			}
			if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
			{
				using (IEnumerator enumerator = e.OldItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object item = enumerator.Current;
						this._itemAttachedStorage.ClearItem(item);
					}
					return;
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				this._itemAttachedStorage.Clear();
			}
		}

		// Token: 0x0600638E RID: 25486 RVA: 0x002A49BC File Offset: 0x002A39BC
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(DataGrid.CanUserAddRowsProperty);
			d.CoerceValue(DataGrid.CanUserDeleteRowsProperty);
			CommandManager.InvalidateRequerySuggested();
			((DataGrid)d).UpdateVisualState();
		}

		// Token: 0x0600638F RID: 25487 RVA: 0x002A49E4 File Offset: 0x002A39E4
		private static void OnIsKeyboardFocusWithinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
		}

		// Token: 0x06006390 RID: 25488 RVA: 0x002A49F8 File Offset: 0x002A39F8
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (!e.Handled && !string.IsNullOrEmpty(e.Text) && base.IsTextSearchEnabled)
			{
				bool flag = e.OriginalSource == this;
				if (!flag)
				{
					ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(e.OriginalSource as DependencyObject);
					flag = (itemsControl == this);
					if (!flag)
					{
						DataGridCellsPresenter dataGridCellsPresenter = itemsControl as DataGridCellsPresenter;
						if (dataGridCellsPresenter != null)
						{
							flag = (dataGridCellsPresenter.DataGridOwner == this);
						}
					}
				}
				if (flag)
				{
					TextSearch textSearch = TextSearch.EnsureInstance(this);
					if (textSearch != null)
					{
						textSearch.DoSearch(e.Text);
						e.Handled = true;
					}
				}
			}
		}

		// Token: 0x06006391 RID: 25489 RVA: 0x002A4A84 File Offset: 0x002A3A84
		internal override bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			object item = info.Item;
			bool result = false;
			if (item != null)
			{
				DataGridColumn currentColumn = this.CurrentColumn;
				if (currentColumn == null)
				{
					this.SetCurrentItem(item);
				}
				else
				{
					DataGridCell dataGridCell = this.TryFindCell(info, currentColumn);
					if (dataGridCell != null)
					{
						dataGridCell.Focus();
						if (this.ShouldSelectRowHeader)
						{
							this.HandleSelectionForRowHeaderAndDetailsInput(dataGridCell.RowOwner, false);
						}
						else
						{
							this.HandleSelectionForCellInput(dataGridCell, false, false, false);
						}
					}
				}
			}
			if (itemNavigateArgs.DeviceUsed is KeyboardDevice)
			{
				KeyboardNavigation.ShowFocusVisual();
			}
			return result;
		}

		// Token: 0x06006392 RID: 25490 RVA: 0x002A4AF8 File Offset: 0x002A3AF8
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Key key = e.Key;
			if (key != Key.Tab)
			{
				if (key != Key.Return)
				{
					switch (key)
					{
					case Key.Prior:
					case Key.Next:
						this.OnPageUpOrDownKeyDown(e);
						break;
					case Key.End:
					case Key.Home:
						this.OnHomeOrEndKeyDown(e);
						break;
					case Key.Left:
					case Key.Up:
					case Key.Right:
					case Key.Down:
						this.OnArrowKeyDown(e);
						break;
					}
				}
				else
				{
					this.OnEnterKeyDown(e);
				}
			}
			else
			{
				this.OnTabKeyDown(e);
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x06006393 RID: 25491 RVA: 0x002A4B79 File Offset: 0x002A3B79
		private static FocusNavigationDirection KeyToTraversalDirection(Key key)
		{
			switch (key)
			{
			case Key.Left:
				return FocusNavigationDirection.Left;
			case Key.Up:
				return FocusNavigationDirection.Up;
			case Key.Right:
				return FocusNavigationDirection.Right;
			}
			return FocusNavigationDirection.Down;
		}

		// Token: 0x06006394 RID: 25492 RVA: 0x002A4BA0 File Offset: 0x002A3BA0
		private void OnArrowKeyDown(KeyEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null)
			{
				e.Handled = true;
				bool isEditing = currentCellContainer.IsEditing;
				KeyboardNavigation keyboardNavigation = KeyboardNavigation.Current;
				UIElement uielement = Keyboard.FocusedElement as UIElement;
				ContentElement contentElement = (uielement == null) ? (Keyboard.FocusedElement as ContentElement) : null;
				if (uielement != null || contentElement != null)
				{
					bool flag = e.OriginalSource == currentCellContainer;
					if (flag)
					{
						KeyboardNavigationMode directionalNavigation = KeyboardNavigation.GetDirectionalNavigation(this);
						if (directionalNavigation == KeyboardNavigationMode.Once)
						{
							DependencyObject dependencyObject = this.PredictFocus(DataGrid.KeyToTraversalDirection(e.Key));
							if (dependencyObject != null && !keyboardNavigation.IsAncestorOfEx(this, dependencyObject))
							{
								Keyboard.Focus(dependencyObject as IInputElement);
							}
							return;
						}
						int displayIndex = this.CurrentColumn.DisplayIndex;
						ItemsControl.ItemInfo currentInfo = this.CurrentInfo;
						int index = currentInfo.Index;
						int num = displayIndex;
						int num2 = index;
						bool flag2 = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
						if (!flag2 && (e.Key == Key.Up || e.Key == Key.Down))
						{
							bool flag3 = false;
							if (currentInfo.Item == CollectionView.NewItemPlaceholder)
							{
								flag3 = true;
							}
							else if (base.IsGrouping)
							{
								GroupItem groupItem = DataGridHelper.FindVisualParent<GroupItem>(currentCellContainer);
								if (groupItem != null)
								{
									CollectionViewGroupInternal collectionViewGroupInternal = base.ItemContainerGenerator.ItemFromContainer(groupItem) as CollectionViewGroupInternal;
									if (collectionViewGroupInternal != null && collectionViewGroupInternal.Items.Count > 0 && ((e.Key == Key.Up && ItemsControl.EqualsEx(collectionViewGroupInternal.Items[0], currentInfo.Item)) || (e.Key == Key.Down && ItemsControl.EqualsEx(collectionViewGroupInternal.Items[collectionViewGroupInternal.Items.Count - 1], currentInfo.Item))))
									{
										int num3 = collectionViewGroupInternal.LeafIndexFromItem(null, 0);
										if (e.Key == Key.Down)
										{
											num3 += collectionViewGroupInternal.ItemCount - 1;
										}
										if (index == num3)
										{
											flag3 = true;
										}
									}
								}
							}
							else if ((e.Key == Key.Up && index == 0) || (e.Key == Key.Down && index == base.Items.Count - 1))
							{
								flag3 = true;
							}
							if (flag3 && this.TryDefaultNavigation(e, currentInfo))
							{
								return;
							}
						}
						Key key = e.Key;
						if (base.FlowDirection == FlowDirection.RightToLeft)
						{
							if (key == Key.Left)
							{
								key = Key.Right;
							}
							else if (key == Key.Right)
							{
								key = Key.Left;
							}
						}
						switch (key)
						{
						case Key.Left:
							if (flag2)
							{
								num = this.InternalColumns.FirstVisibleDisplayIndex;
								goto IL_4AE;
							}
							num--;
							while (num >= 0 && !this.ColumnFromDisplayIndex(num).IsVisible)
							{
								num--;
							}
							if (num >= 0)
							{
								goto IL_4AE;
							}
							if (directionalNavigation == KeyboardNavigationMode.Cycle)
							{
								num = this.InternalColumns.LastVisibleDisplayIndex;
								goto IL_4AE;
							}
							if (directionalNavigation == KeyboardNavigationMode.Contained)
							{
								DependencyObject dependencyObject2 = keyboardNavigation.PredictFocusedElement(currentCellContainer, DataGrid.KeyToTraversalDirection(key), false, false);
								if (dependencyObject2 != null && keyboardNavigation.IsAncestorOfEx(this, dependencyObject2))
								{
									Keyboard.Focus(dependencyObject2 as IInputElement);
								}
								return;
							}
							this.MoveFocus(new TraversalRequest((e.Key == Key.Left) ? FocusNavigationDirection.Left : FocusNavigationDirection.Right));
							return;
						case Key.Up:
							if (flag2)
							{
								num2 = 0;
								goto IL_4AE;
							}
							num2--;
							if (num2 >= 0)
							{
								goto IL_4AE;
							}
							if (directionalNavigation == KeyboardNavigationMode.Cycle)
							{
								num2 = base.Items.Count - 1;
								goto IL_4AE;
							}
							if (directionalNavigation == KeyboardNavigationMode.Contained)
							{
								DependencyObject dependencyObject3 = keyboardNavigation.PredictFocusedElement(currentCellContainer, DataGrid.KeyToTraversalDirection(key), false, false);
								if (dependencyObject3 != null && keyboardNavigation.IsAncestorOfEx(this, dependencyObject3))
								{
									Keyboard.Focus(dependencyObject3 as IInputElement);
								}
								return;
							}
							this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
							return;
						case Key.Right:
						{
							if (flag2)
							{
								num = Math.Max(0, this.InternalColumns.LastVisibleDisplayIndex);
								goto IL_4AE;
							}
							num++;
							int count = this.Columns.Count;
							while (num < count && !this.ColumnFromDisplayIndex(num).IsVisible)
							{
								num++;
							}
							if (num < this.Columns.Count)
							{
								goto IL_4AE;
							}
							if (directionalNavigation == KeyboardNavigationMode.Cycle)
							{
								num = this.InternalColumns.FirstVisibleDisplayIndex;
								goto IL_4AE;
							}
							if (directionalNavigation == KeyboardNavigationMode.Contained)
							{
								DependencyObject dependencyObject4 = keyboardNavigation.PredictFocusedElement(currentCellContainer, DataGrid.KeyToTraversalDirection(key), false, false);
								if (dependencyObject4 != null && keyboardNavigation.IsAncestorOfEx(this, dependencyObject4))
								{
									Keyboard.Focus(dependencyObject4 as IInputElement);
								}
								return;
							}
							this.MoveFocus(new TraversalRequest((e.Key == Key.Left) ? FocusNavigationDirection.Left : FocusNavigationDirection.Right));
							return;
						}
						}
						if (flag2)
						{
							num2 = Math.Max(0, base.Items.Count - 1);
						}
						else
						{
							num2++;
							if (num2 >= base.Items.Count)
							{
								if (directionalNavigation == KeyboardNavigationMode.Cycle)
								{
									num2 = 0;
								}
								else
								{
									if (directionalNavigation == KeyboardNavigationMode.Contained)
									{
										DependencyObject dependencyObject5 = keyboardNavigation.PredictFocusedElement(currentCellContainer, DataGrid.KeyToTraversalDirection(key), false, false);
										if (dependencyObject5 != null && keyboardNavigation.IsAncestorOfEx(this, dependencyObject5))
										{
											Keyboard.Focus(dependencyObject5 as IInputElement);
										}
										return;
									}
									this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
									return;
								}
							}
						}
						IL_4AE:
						DataGridColumn column = this.ColumnFromDisplayIndex(num);
						ItemsControl.ItemInfo info = base.ItemInfoFromIndex(num2);
						this.ScrollCellIntoView(info, column);
						DataGridCell dataGridCell = this.TryFindCell(info, column);
						if (dataGridCell == null || dataGridCell == currentCellContainer || !dataGridCell.Focus())
						{
							return;
						}
					}
					else if (this.TryDefaultNavigation(e, null))
					{
						return;
					}
					TraversalRequest request = new TraversalRequest(DataGrid.KeyToTraversalDirection(e.Key));
					if (flag || (uielement != null && uielement.MoveFocus(request)) || (contentElement != null && contentElement.MoveFocus(request)))
					{
						this.SelectAndEditOnFocusMove(e, currentCellContainer, isEditing, true, true);
					}
				}
			}
		}

		// Token: 0x06006395 RID: 25493 RVA: 0x002A50E0 File Offset: 0x002A40E0
		private bool TryDefaultNavigation(KeyEventArgs e, ItemsControl.ItemInfo currentInfo)
		{
			FrameworkElement frameworkElement = Keyboard.FocusedElement as FrameworkElement;
			if (frameworkElement != null && base.ItemsHost.IsAncestorOf(frameworkElement))
			{
				FrameworkElement frameworkElement2;
				base.PrepareNavigateByLine(currentInfo, frameworkElement, (e.Key == Key.Up) ? FocusNavigationDirection.Up : FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.KeyboardDevice, Keyboard.Modifiers), out frameworkElement2);
				if (frameworkElement2 != null)
				{
					DataGridRow dataGridRow = DataGridHelper.FindVisualParent<DataGridRow>(frameworkElement2);
					if (dataGridRow == null || dataGridRow.DataGridOwner != this)
					{
						frameworkElement2.Focus();
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006396 RID: 25494 RVA: 0x002A5154 File Offset: 0x002A4154
		private void OnTabKeyDown(KeyEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null)
			{
				bool isEditing = currentCellContainer.IsEditing;
				bool flag = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
				UIElement uielement = Keyboard.FocusedElement as UIElement;
				ContentElement contentElement = (uielement == null) ? (Keyboard.FocusedElement as ContentElement) : null;
				if (uielement != null || contentElement != null)
				{
					e.Handled = true;
					TraversalRequest request = new TraversalRequest(flag ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next);
					if ((uielement != null && uielement.MoveFocus(request)) || (contentElement != null && contentElement.MoveFocus(request)))
					{
						if (isEditing && flag && Keyboard.FocusedElement == currentCellContainer)
						{
							currentCellContainer.MoveFocus(request);
						}
						if (base.IsGrouping && isEditing)
						{
							DataGridCell cellForSelectAndEditOnFocusMove = this.GetCellForSelectAndEditOnFocusMove();
							if (cellForSelectAndEditOnFocusMove != null && cellForSelectAndEditOnFocusMove.RowDataItem == currentCellContainer.RowDataItem)
							{
								DataGridCell dataGridCell = this.TryFindCell(cellForSelectAndEditOnFocusMove.RowDataItem, cellForSelectAndEditOnFocusMove.Column);
								if (dataGridCell == null)
								{
									base.UpdateLayout();
									dataGridCell = this.TryFindCell(cellForSelectAndEditOnFocusMove.RowDataItem, cellForSelectAndEditOnFocusMove.Column);
								}
								if (dataGridCell != null && dataGridCell != cellForSelectAndEditOnFocusMove)
								{
									dataGridCell.Focus();
								}
							}
						}
						this.SelectAndEditOnFocusMove(e, currentCellContainer, isEditing, false, true);
					}
				}
			}
		}

		// Token: 0x06006397 RID: 25495 RVA: 0x002A5274 File Offset: 0x002A4274
		private void OnEnterKeyDown(KeyEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null && this._columns.Count > 0)
			{
				e.Handled = true;
				DataGridColumn column = currentCellContainer.Column;
				if (this.CommitAnyEdit() && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
				{
					bool flag = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
					int count = base.Items.Count;
					int num = this.CurrentInfo.Index;
					if (num < 0)
					{
						num = base.Items.IndexOf(this.CurrentItem);
					}
					num = Math.Max(0, Math.Min(count - 1, num + (flag ? -1 : 1)));
					if (num < count)
					{
						ItemsControl.ItemInfo itemInfo = base.ItemInfoFromIndex(num);
						this.ScrollIntoView(itemInfo, column);
						if (!ItemsControl.EqualsEx(this.CurrentCell.Item, itemInfo.Item))
						{
							base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, new DataGridCellInfo(itemInfo, column, this));
							this.SelectAndEditOnFocusMove(e, currentCellContainer, false, false, true);
							return;
						}
						currentCellContainer = this.CurrentCellContainer;
						if (currentCellContainer != null)
						{
							currentCellContainer.Focus();
						}
					}
				}
			}
		}

		// Token: 0x06006398 RID: 25496 RVA: 0x002A5394 File Offset: 0x002A4394
		private DataGridCell GetCellForSelectAndEditOnFocusMove()
		{
			DataGridCell dataGridCell = Keyboard.FocusedElement as DataGridCell;
			if (dataGridCell == null && this.CurrentCellContainer != null && this.CurrentCellContainer.IsKeyboardFocusWithin)
			{
				dataGridCell = this.CurrentCellContainer;
			}
			return dataGridCell;
		}

		// Token: 0x06006399 RID: 25497 RVA: 0x002A53CC File Offset: 0x002A43CC
		private void SelectAndEditOnFocusMove(KeyEventArgs e, DataGridCell oldCell, bool wasEditing, bool allowsExtendSelect, bool ignoreControlKey)
		{
			DataGridCell cellForSelectAndEditOnFocusMove = this.GetCellForSelectAndEditOnFocusMove();
			if (cellForSelectAndEditOnFocusMove != null && cellForSelectAndEditOnFocusMove.DataGridOwner == this)
			{
				if (ignoreControlKey || (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
				{
					if (this.ShouldSelectRowHeader && allowsExtendSelect)
					{
						this.HandleSelectionForRowHeaderAndDetailsInput(cellForSelectAndEditOnFocusMove.RowOwner, false);
					}
					else
					{
						this.HandleSelectionForCellInput(cellForSelectAndEditOnFocusMove, false, allowsExtendSelect, false);
					}
				}
				if (wasEditing && !cellForSelectAndEditOnFocusMove.IsEditing && oldCell.RowDataItem == cellForSelectAndEditOnFocusMove.RowDataItem)
				{
					this.BeginEdit(e);
				}
			}
		}

		// Token: 0x0600639A RID: 25498 RVA: 0x002A5448 File Offset: 0x002A4448
		private void OnHomeOrEndKeyDown(KeyEventArgs e)
		{
			if (this._columns.Count > 0 && base.Items.Count > 0)
			{
				e.Handled = true;
				bool flag = e.Key == Key.Home;
				bool flag2 = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
				if (flag2)
				{
					ScrollViewer internalScrollHost = this.InternalScrollHost;
					if (internalScrollHost != null)
					{
						if (flag)
						{
							internalScrollHost.ScrollToHome();
						}
						else
						{
							internalScrollHost.ScrollToEnd();
						}
					}
				}
				ItemsControl.ItemInfo info = flag2 ? base.ItemInfoFromIndex(flag ? 0 : (base.Items.Count - 1)) : this.CurrentInfo;
				DataGridColumn column = this.ColumnFromDisplayIndex(flag ? this.InternalColumns.FirstVisibleDisplayIndex : this.InternalColumns.LastVisibleDisplayIndex);
				this.ScrollCellIntoView(info, column);
				DataGridCell dataGridCell = this.TryFindCell(info, column);
				if (dataGridCell != null)
				{
					dataGridCell.Focus();
					if (this.ShouldSelectRowHeader)
					{
						this.HandleSelectionForRowHeaderAndDetailsInput(dataGridCell.RowOwner, false);
						return;
					}
					this.HandleSelectionForCellInput(dataGridCell, false, true, false);
				}
			}
		}

		// Token: 0x0600639B RID: 25499 RVA: 0x002A553C File Offset: 0x002A453C
		private void OnPageUpOrDownKeyDown(KeyEventArgs e)
		{
			ScrollViewer internalScrollHost = this.InternalScrollHost;
			if (internalScrollHost != null)
			{
				e.Handled = true;
				ItemsControl.ItemInfo currentInfo = this.CurrentInfo;
				if (VirtualizingPanel.GetScrollUnit(this) == ScrollUnit.Item && !base.IsGrouping)
				{
					int index = currentInfo.Index;
					if (index >= 0)
					{
						int num = Math.Max(1, (int)internalScrollHost.ViewportHeight - 1);
						int num2 = (e.Key == Key.Prior) ? (index - num) : (index + num);
						num2 = Math.Max(0, Math.Min(num2, base.Items.Count - 1));
						ItemsControl.ItemInfo itemInfo = base.ItemInfoFromIndex(num2);
						DataGridColumn currentColumn = this.CurrentColumn;
						if (currentColumn == null)
						{
							base.OnBringItemIntoView(itemInfo);
							this.SetCurrentItem(itemInfo.Item);
							return;
						}
						this.ScrollCellIntoView(itemInfo, currentColumn);
						DataGridCell dataGridCell = this.TryFindCell(itemInfo, currentColumn);
						if (dataGridCell != null)
						{
							dataGridCell.Focus();
							if (this.ShouldSelectRowHeader)
							{
								this.HandleSelectionForRowHeaderAndDetailsInput(dataGridCell.RowOwner, false);
								return;
							}
							this.HandleSelectionForCellInput(dataGridCell, false, true, false);
							return;
						}
					}
				}
				else
				{
					FocusNavigationDirection direction = (e.Key == Key.Prior) ? FocusNavigationDirection.Up : FocusNavigationDirection.Down;
					ItemsControl.ItemInfo startingInfo = currentInfo;
					FrameworkElement frameworkElement = null;
					if (base.IsGrouping)
					{
						frameworkElement = (Keyboard.FocusedElement as FrameworkElement);
						if (frameworkElement != null)
						{
							startingInfo = null;
							DataGridRow dataGridRow = frameworkElement as DataGridRow;
							if (dataGridRow == null)
							{
								dataGridRow = DataGridHelper.FindVisualParent<DataGridRow>(frameworkElement);
							}
							if (dataGridRow != null && ItemsControl.ItemsControlFromItemContainer(dataGridRow) as DataGrid == this)
							{
								startingInfo = base.ItemInfoFromContainer(dataGridRow);
							}
						}
					}
					FrameworkElement frameworkElement2;
					base.PrepareToNavigateByPage(startingInfo, frameworkElement, direction, new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, Keyboard.Modifiers), out frameworkElement2);
					DataGridRow dataGridRow2 = frameworkElement2 as DataGridRow;
					if (dataGridRow2 == null)
					{
						dataGridRow2 = DataGridHelper.FindVisualParent<DataGridRow>(frameworkElement2);
					}
					if (dataGridRow2 != null)
					{
						ItemsControl.ItemInfo itemInfo2 = base.ItemInfoFromContainer(dataGridRow2);
						DataGridColumn currentColumn2 = this.CurrentColumn;
						if (currentColumn2 == null)
						{
							this.SetCurrentItem(itemInfo2.Item);
							return;
						}
						DataGridCell dataGridCell2 = this.TryFindCell(itemInfo2, currentColumn2);
						if (dataGridCell2 != null)
						{
							dataGridCell2.Focus();
							if (this.ShouldSelectRowHeader)
							{
								this.HandleSelectionForRowHeaderAndDetailsInput(dataGridCell2.RowOwner, false);
								return;
							}
							this.HandleSelectionForCellInput(dataGridCell2, false, true, false);
							return;
						}
					}
					else if (frameworkElement2 != null)
					{
						frameworkElement2.Focus();
					}
				}
			}
		}

		// Token: 0x0600639C RID: 25500 RVA: 0x002A5744 File Offset: 0x002A4744
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (this._isDraggingSelection)
			{
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					Point position = Mouse.GetPosition(this);
					if (!DoubleUtil.AreClose(position, this._dragPoint))
					{
						this._dragPoint = position;
						DataGrid.RelativeMousePositions relativeMousePosition = this.RelativeMousePosition;
						if (relativeMousePosition == DataGrid.RelativeMousePositions.Over)
						{
							if (this._isRowDragging)
							{
								DataGridRow mouseOverRow = this.MouseOverRow;
								if (mouseOverRow != null && mouseOverRow.Item != this.CurrentItem)
								{
									this.HandleSelectionForRowHeaderAndDetailsInput(mouseOverRow, false);
									this.SetCurrentItem(mouseOverRow.Item);
									e.Handled = true;
									return;
								}
							}
							else
							{
								DataGridCell dataGridCell = this.MouseOverCell;
								if (dataGridCell == null && this.MouseOverRow != null)
								{
									dataGridCell = this.GetCellNearMouse();
								}
								if (dataGridCell != null && dataGridCell != this.CurrentCellContainer)
								{
									this.HandleSelectionForCellInput(dataGridCell, false, true, true);
									dataGridCell.Focus();
									e.Handled = true;
									return;
								}
							}
						}
						else if (this._isRowDragging && DataGrid.IsMouseToLeftOrRightOnly(relativeMousePosition))
						{
							DataGridRow rowNearMouse = this.GetRowNearMouse();
							if (rowNearMouse != null && rowNearMouse.Item != this.CurrentItem)
							{
								this.HandleSelectionForRowHeaderAndDetailsInput(rowNearMouse, false);
								this.SetCurrentItem(rowNearMouse.Item);
								e.Handled = true;
								return;
							}
						}
						else
						{
							if (!this._hasAutoScrolled)
							{
								this.StartAutoScroll();
								return;
							}
							if (this.DoAutoScroll())
							{
								e.Handled = true;
								return;
							}
						}
					}
				}
				else
				{
					this.EndDragging();
				}
			}
		}

		// Token: 0x0600639D RID: 25501 RVA: 0x002A5888 File Offset: 0x002A4888
		private static void OnAnyMouseUpThunk(object sender, MouseButtonEventArgs e)
		{
			((DataGrid)sender).OnAnyMouseUp(e);
		}

		// Token: 0x0600639E RID: 25502 RVA: 0x002A5896 File Offset: 0x002A4896
		private void OnAnyMouseUp(MouseButtonEventArgs e)
		{
			this.EndDragging();
		}

		// Token: 0x0600639F RID: 25503 RVA: 0x002A58A0 File Offset: 0x002A48A0
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			if (!base.IsEnabled)
			{
				return;
			}
			DataGridCell dataGridCell = null;
			DataGridRowHeader dataGridRowHeader = null;
			for (UIElement uielement = e.OriginalSource as UIElement; uielement != null; uielement = (VisualTreeHelper.GetParent(uielement) as UIElement))
			{
				dataGridCell = (uielement as DataGridCell);
				if (dataGridCell != null)
				{
					break;
				}
				dataGridRowHeader = (uielement as DataGridRowHeader);
				if (dataGridRowHeader != null)
				{
					break;
				}
			}
			if (dataGridCell != null && !dataGridCell.IsSelected && !dataGridCell.IsKeyboardFocusWithin)
			{
				dataGridCell.Focus();
				this.HandleSelectionForCellInput(dataGridCell, false, true, true);
			}
			if (dataGridRowHeader != null)
			{
				DataGridRow parentRow = dataGridRowHeader.ParentRow;
				if (parentRow != null && !parentRow.IsSelected)
				{
					this.HandleSelectionForRowHeaderAndDetailsInput(parentRow, false);
				}
			}
		}

		// Token: 0x060063A0 RID: 25504 RVA: 0x002A592C File Offset: 0x002A492C
		private DataGridRow GetRowNearMouse()
		{
			Panel internalItemsHost = this.InternalItemsHost;
			if (internalItemsHost != null)
			{
				bool isGrouping = base.IsGrouping;
				for (int i = isGrouping ? (base.Items.Count - 1) : (internalItemsHost.Children.Count - 1); i >= 0; i--)
				{
					DataGridRow dataGridRow;
					if (isGrouping)
					{
						dataGridRow = (base.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow);
					}
					else
					{
						dataGridRow = (internalItemsHost.Children[i] as DataGridRow);
					}
					if (dataGridRow != null)
					{
						Point position = Mouse.GetPosition(dataGridRow);
						Rect rect = new Rect(default(Point), dataGridRow.RenderSize);
						if (position.Y >= rect.Top && position.Y <= rect.Bottom)
						{
							return dataGridRow;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060063A1 RID: 25505 RVA: 0x002A59E8 File Offset: 0x002A49E8
		private DataGridCell GetCellNearMouse()
		{
			Panel internalItemsHost = this.InternalItemsHost;
			if (internalItemsHost != null)
			{
				Rect itemsHostBounds = new Rect(default(Point), internalItemsHost.RenderSize);
				double num = double.PositiveInfinity;
				DataGridCell dataGridCell = null;
				bool isMouseInCorner = DataGrid.IsMouseInCorner(this.RelativeMousePosition);
				bool isGrouping = base.IsGrouping;
				for (int i = isGrouping ? (base.Items.Count - 1) : (internalItemsHost.Children.Count - 1); i >= 0; i--)
				{
					DataGridRow dataGridRow;
					if (isGrouping)
					{
						dataGridRow = (base.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow);
					}
					else
					{
						dataGridRow = (internalItemsHost.Children[i] as DataGridRow);
					}
					if (dataGridRow != null)
					{
						DataGridCellsPresenter cellsPresenter = dataGridRow.CellsPresenter;
						if (cellsPresenter != null)
						{
							for (ContainerTracking<DataGridCell> containerTracking = cellsPresenter.CellTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
							{
								DataGridCell container = containerTracking.Container;
								double num2;
								if (DataGrid.CalculateCellDistance(container, dataGridRow, internalItemsHost, itemsHostBounds, isMouseInCorner, out num2) && (dataGridCell == null || num2 < num))
								{
									num = num2;
									dataGridCell = container;
								}
							}
							DataGridRowHeader rowHeader = dataGridRow.RowHeader;
							double num3;
							if (rowHeader != null && DataGrid.CalculateCellDistance(rowHeader, dataGridRow, internalItemsHost, itemsHostBounds, isMouseInCorner, out num3) && (dataGridCell == null || num3 < num))
							{
								DataGridCell dataGridCell2 = dataGridRow.TryGetCell(this.DisplayIndexMap[0]);
								if (dataGridCell2 != null)
								{
									num = num3;
									dataGridCell = dataGridCell2;
								}
							}
						}
					}
				}
				return dataGridCell;
			}
			return null;
		}

		// Token: 0x060063A2 RID: 25506 RVA: 0x002A5B40 File Offset: 0x002A4B40
		private static bool CalculateCellDistance(FrameworkElement cell, DataGridRow rowOwner, Panel itemsHost, Rect itemsHostBounds, bool isMouseInCorner, out double distance)
		{
			GeneralTransform generalTransform = cell.TransformToAncestor(itemsHost);
			Rect rect = new Rect(default(Point), cell.RenderSize);
			if (itemsHostBounds.Contains(generalTransform.TransformBounds(rect)))
			{
				Point position = Mouse.GetPosition(cell);
				if (isMouseInCorner)
				{
					Vector vector = new Vector(position.X - rect.Width * 0.5, position.Y - rect.Height * 0.5);
					distance = vector.Length;
					return true;
				}
				Point position2 = Mouse.GetPosition(rowOwner);
				Rect rect2 = new Rect(default(Point), rowOwner.RenderSize);
				if (position.X >= rect.Left && position.X <= rect.Right)
				{
					if (position2.Y >= rect2.Top && position2.Y <= rect2.Bottom)
					{
						distance = 0.0;
					}
					else
					{
						distance = Math.Abs(position.Y - rect.Top);
					}
					return true;
				}
				if (position2.Y >= rect2.Top && position2.Y <= rect2.Bottom)
				{
					distance = Math.Abs(position.X - rect.Left);
					return true;
				}
			}
			distance = double.PositiveInfinity;
			return false;
		}

		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x060063A3 RID: 25507 RVA: 0x002A5C9C File Offset: 0x002A4C9C
		private DataGridRow MouseOverRow
		{
			get
			{
				UIElement element = Mouse.DirectlyOver as UIElement;
				DataGridRow dataGridRow = null;
				while (element != null)
				{
					dataGridRow = DataGridHelper.FindVisualParent<DataGridRow>(element);
					if (dataGridRow == null || dataGridRow.DataGridOwner == this)
					{
						break;
					}
					element = (VisualTreeHelper.GetParent(dataGridRow) as UIElement);
				}
				return dataGridRow;
			}
		}

		// Token: 0x1700170F RID: 5903
		// (get) Token: 0x060063A4 RID: 25508 RVA: 0x002A5CDC File Offset: 0x002A4CDC
		private DataGridCell MouseOverCell
		{
			get
			{
				UIElement element = Mouse.DirectlyOver as UIElement;
				DataGridCell dataGridCell = null;
				while (element != null)
				{
					dataGridCell = DataGridHelper.FindVisualParent<DataGridCell>(element);
					if (dataGridCell == null || dataGridCell.DataGridOwner == this)
					{
						break;
					}
					element = (VisualTreeHelper.GetParent(dataGridCell) as UIElement);
				}
				return dataGridCell;
			}
		}

		// Token: 0x17001710 RID: 5904
		// (get) Token: 0x060063A5 RID: 25509 RVA: 0x002A5D1C File Offset: 0x002A4D1C
		private DataGrid.RelativeMousePositions RelativeMousePosition
		{
			get
			{
				DataGrid.RelativeMousePositions relativeMousePositions = DataGrid.RelativeMousePositions.Over;
				Panel internalItemsHost = this.InternalItemsHost;
				if (internalItemsHost != null)
				{
					Point position = Mouse.GetPosition(internalItemsHost);
					Rect rect = new Rect(default(Point), internalItemsHost.RenderSize);
					if (position.X < rect.Left)
					{
						relativeMousePositions |= DataGrid.RelativeMousePositions.Left;
					}
					else if (position.X > rect.Right)
					{
						relativeMousePositions |= DataGrid.RelativeMousePositions.Right;
					}
					if (position.Y < rect.Top)
					{
						relativeMousePositions |= DataGrid.RelativeMousePositions.Above;
					}
					else if (position.Y > rect.Bottom)
					{
						relativeMousePositions |= DataGrid.RelativeMousePositions.Below;
					}
				}
				return relativeMousePositions;
			}
		}

		// Token: 0x060063A6 RID: 25510 RVA: 0x002A5DA8 File Offset: 0x002A4DA8
		private static bool IsMouseToLeft(DataGrid.RelativeMousePositions position)
		{
			return (position & DataGrid.RelativeMousePositions.Left) == DataGrid.RelativeMousePositions.Left;
		}

		// Token: 0x060063A7 RID: 25511 RVA: 0x002A5DB0 File Offset: 0x002A4DB0
		private static bool IsMouseToRight(DataGrid.RelativeMousePositions position)
		{
			return (position & DataGrid.RelativeMousePositions.Right) == DataGrid.RelativeMousePositions.Right;
		}

		// Token: 0x060063A8 RID: 25512 RVA: 0x002A5DB8 File Offset: 0x002A4DB8
		private static bool IsMouseAbove(DataGrid.RelativeMousePositions position)
		{
			return (position & DataGrid.RelativeMousePositions.Above) == DataGrid.RelativeMousePositions.Above;
		}

		// Token: 0x060063A9 RID: 25513 RVA: 0x002A5DC0 File Offset: 0x002A4DC0
		private static bool IsMouseBelow(DataGrid.RelativeMousePositions position)
		{
			return (position & DataGrid.RelativeMousePositions.Below) == DataGrid.RelativeMousePositions.Below;
		}

		// Token: 0x060063AA RID: 25514 RVA: 0x002A5DC8 File Offset: 0x002A4DC8
		private static bool IsMouseToLeftOrRightOnly(DataGrid.RelativeMousePositions position)
		{
			return position == DataGrid.RelativeMousePositions.Left || position == DataGrid.RelativeMousePositions.Right;
		}

		// Token: 0x060063AB RID: 25515 RVA: 0x002A5DD4 File Offset: 0x002A4DD4
		private static bool IsMouseInCorner(DataGrid.RelativeMousePositions position)
		{
			return position != DataGrid.RelativeMousePositions.Over && position != DataGrid.RelativeMousePositions.Above && position != DataGrid.RelativeMousePositions.Below && position != DataGrid.RelativeMousePositions.Left && position != DataGrid.RelativeMousePositions.Right;
		}

		// Token: 0x060063AC RID: 25516 RVA: 0x002A5DEE File Offset: 0x002A4DEE
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridAutomationPeer(this);
		}

		// Token: 0x060063AD RID: 25517 RVA: 0x002A5DF8 File Offset: 0x002A4DF8
		private DataGrid.CellAutomationValueHolder GetCellAutomationValueHolder(object item, DataGridColumn column)
		{
			DataGrid.CellAutomationValueHolder result;
			if (this._editingRowInfo == null || !ItemsControl.EqualsEx(item, this._editingRowInfo.Item) || !this._editingCellAutomationValueHolders.TryGetValue(column, out result))
			{
				DataGridCell dataGridCell = this.TryFindCell(item, column);
				result = ((dataGridCell != null) ? new DataGrid.CellAutomationValueHolder(dataGridCell) : new DataGrid.CellAutomationValueHolder(item, column));
			}
			return result;
		}

		// Token: 0x060063AE RID: 25518 RVA: 0x002A5E53 File Offset: 0x002A4E53
		internal string GetCellAutomationValue(object item, DataGridColumn column)
		{
			return this.GetCellAutomationValueHolder(item, column).Value;
		}

		// Token: 0x060063AF RID: 25519 RVA: 0x002A5E62 File Offset: 0x002A4E62
		internal object GetCellClipboardValue(object item, DataGridColumn column)
		{
			return this.GetCellAutomationValueHolder(item, column).GetClipboardValue();
		}

		// Token: 0x060063B0 RID: 25520 RVA: 0x002A5E71 File Offset: 0x002A4E71
		internal void SetCellAutomationValue(object item, DataGridColumn column, string value)
		{
			this.SetCellValue(item, column, value, false);
		}

		// Token: 0x060063B1 RID: 25521 RVA: 0x002A5E7D File Offset: 0x002A4E7D
		internal void SetCellClipboardValue(object item, DataGridColumn column, object value)
		{
			this.SetCellValue(item, column, value, true);
		}

		// Token: 0x060063B2 RID: 25522 RVA: 0x002A5E8C File Offset: 0x002A4E8C
		private void SetCellValue(object item, DataGridColumn column, object value, bool clipboard)
		{
			this.CurrentCellContainer = this.TryFindCell(item, column);
			if (this.CurrentCellContainer == null)
			{
				this.ScrollCellIntoView(base.NewItemInfo(item, null, -1), column);
				this.CurrentCellContainer = this.TryFindCell(item, column);
			}
			if (this.CurrentCellContainer == null)
			{
				return;
			}
			if (this.BeginEdit())
			{
				DataGrid.CellAutomationValueHolder cellAutomationValueHolder;
				if (this._editingCellAutomationValueHolders.TryGetValue(column, out cellAutomationValueHolder))
				{
					cellAutomationValueHolder.SetValue(this, value, clipboard);
					return;
				}
				this.CancelEdit();
			}
		}

		// Token: 0x060063B3 RID: 25523 RVA: 0x002A5F00 File Offset: 0x002A4F00
		private void EnsureCellAutomationValueHolder(DataGridCell cell)
		{
			if (!this._editingCellAutomationValueHolders.ContainsKey(cell.Column))
			{
				this._editingCellAutomationValueHolders.Add(cell.Column, new DataGrid.CellAutomationValueHolder(cell));
			}
		}

		// Token: 0x060063B4 RID: 25524 RVA: 0x002A5F2C File Offset: 0x002A4F2C
		private void UpdateCellAutomationValueHolder(DataGridCell cell)
		{
			DataGrid.CellAutomationValueHolder cellAutomationValueHolder;
			if (this._editingCellAutomationValueHolders.TryGetValue(cell.Column, out cellAutomationValueHolder))
			{
				cellAutomationValueHolder.TrackValue();
			}
		}

		// Token: 0x060063B5 RID: 25525 RVA: 0x002A5F54 File Offset: 0x002A4F54
		private void ReleaseCellAutomationValueHolders()
		{
			foreach (KeyValuePair<DataGridColumn, DataGrid.CellAutomationValueHolder> keyValuePair in this._editingCellAutomationValueHolders)
			{
				keyValuePair.Value.TrackValue();
			}
			this._editingCellAutomationValueHolders.Clear();
		}

		// Token: 0x060063B6 RID: 25526 RVA: 0x002A5FB8 File Offset: 0x002A4FB8
		internal DataGridCell TryFindCell(DataGridCellInfo info)
		{
			return this.TryFindCell(base.LeaseItemInfo(info.ItemInfo, false), info.Column);
		}

		// Token: 0x060063B7 RID: 25527 RVA: 0x002A5FD8 File Offset: 0x002A4FD8
		internal DataGridCell TryFindCell(ItemsControl.ItemInfo info, DataGridColumn column)
		{
			DataGridRow dataGridRow = (DataGridRow)info.Container;
			int num = this._columns.IndexOf(column);
			if (dataGridRow != null && num >= 0)
			{
				return dataGridRow.TryGetCell(num);
			}
			return null;
		}

		// Token: 0x060063B8 RID: 25528 RVA: 0x002A6010 File Offset: 0x002A5010
		internal DataGridCell TryFindCell(object item, DataGridColumn column)
		{
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(item);
			int num = this._columns.IndexOf(column);
			if (dataGridRow != null && num >= 0)
			{
				return dataGridRow.TryGetCell(num);
			}
			return null;
		}

		// Token: 0x17001711 RID: 5905
		// (get) Token: 0x060063B9 RID: 25529 RVA: 0x002A604C File Offset: 0x002A504C
		// (set) Token: 0x060063BA RID: 25530 RVA: 0x002A605E File Offset: 0x002A505E
		public bool CanUserSortColumns
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserSortColumnsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserSortColumnsProperty, value);
			}
		}

		// Token: 0x060063BB RID: 25531 RVA: 0x002A606C File Offset: 0x002A506C
		private static object OnCoerceCanUserSortColumns(DependencyObject d, object baseValue)
		{
			DataGrid dataGrid = (DataGrid)d;
			if (DataGridHelper.IsPropertyTransferEnabled(dataGrid, DataGrid.CanUserSortColumnsProperty) && DataGridHelper.IsDefaultValue(dataGrid, DataGrid.CanUserSortColumnsProperty) && !dataGrid.Items.CanSort)
			{
				return false;
			}
			return baseValue;
		}

		// Token: 0x060063BC RID: 25532 RVA: 0x002A60AF File Offset: 0x002A50AF
		private static void OnCanUserSortColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridHelper.TransferProperty((DataGrid)d, DataGrid.CanUserSortColumnsProperty);
			DataGrid.OnNotifyColumnPropertyChanged(d, e);
		}

		// Token: 0x140000FA RID: 250
		// (add) Token: 0x060063BD RID: 25533 RVA: 0x002A60C8 File Offset: 0x002A50C8
		// (remove) Token: 0x060063BE RID: 25534 RVA: 0x002A6100 File Offset: 0x002A5100
		public event DataGridSortingEventHandler Sorting;

		// Token: 0x060063BF RID: 25535 RVA: 0x002A6135 File Offset: 0x002A5135
		protected virtual void OnSorting(DataGridSortingEventArgs eventArgs)
		{
			eventArgs.Handled = false;
			if (this.Sorting != null)
			{
				this.Sorting(this, eventArgs);
			}
			if (!eventArgs.Handled)
			{
				this.DefaultSort(eventArgs.Column, (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift);
			}
		}

		// Token: 0x060063C0 RID: 25536 RVA: 0x002A6174 File Offset: 0x002A5174
		internal void PerformSort(DataGridColumn sortColumn)
		{
			if (!this.CanUserSortColumns || !sortColumn.CanUserSort)
			{
				return;
			}
			if (this.CommitAnyEdit())
			{
				this.PrepareForSort(sortColumn);
				DataGridSortingEventArgs eventArgs = new DataGridSortingEventArgs(sortColumn);
				this.OnSorting(eventArgs);
				if (base.Items.NeedsRefresh)
				{
					try
					{
						base.Items.Refresh();
					}
					catch (InvalidOperationException innerException)
					{
						base.Items.SortDescriptions.Clear();
						throw new InvalidOperationException(SR.Get("DataGrid_ProbableInvalidSortDescription"), innerException);
					}
				}
			}
		}

		// Token: 0x060063C1 RID: 25537 RVA: 0x002A61FC File Offset: 0x002A51FC
		private void PrepareForSort(DataGridColumn sortColumn)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
			{
				return;
			}
			if (this.Columns != null)
			{
				foreach (DataGridColumn dataGridColumn in this.Columns)
				{
					if (dataGridColumn != sortColumn)
					{
						dataGridColumn.SortDirection = null;
					}
				}
			}
		}

		// Token: 0x060063C2 RID: 25538 RVA: 0x002A6268 File Offset: 0x002A5268
		private void DefaultSort(DataGridColumn column, bool clearExistingSortDescriptions)
		{
			ListSortDirection listSortDirection = ListSortDirection.Ascending;
			ListSortDirection? sortDirection = column.SortDirection;
			if (sortDirection != null && sortDirection.Value == ListSortDirection.Ascending)
			{
				listSortDirection = ListSortDirection.Descending;
			}
			string sortMemberPath = column.SortMemberPath;
			if (!string.IsNullOrEmpty(sortMemberPath))
			{
				try
				{
					using (base.Items.DeferRefresh())
					{
						int num = -1;
						if (clearExistingSortDescriptions)
						{
							base.Items.SortDescriptions.Clear();
						}
						else
						{
							for (int i = 0; i < base.Items.SortDescriptions.Count; i++)
							{
								if (string.Compare(base.Items.SortDescriptions[i].PropertyName, sortMemberPath, StringComparison.Ordinal) == 0 && (this.GroupingSortDescriptionIndices == null || !this.GroupingSortDescriptionIndices.Contains(i)))
								{
									num = i;
									break;
								}
							}
						}
						SortDescription sortDescription = new SortDescription(sortMemberPath, listSortDirection);
						if (num >= 0)
						{
							base.Items.SortDescriptions[num] = sortDescription;
						}
						else
						{
							base.Items.SortDescriptions.Add(sortDescription);
						}
						if (clearExistingSortDescriptions || !this._sortingStarted)
						{
							this.RegenerateGroupingSortDescriptions();
							this._sortingStarted = true;
						}
					}
					column.SortDirection = new ListSortDirection?(listSortDirection);
				}
				catch (InvalidOperationException exception)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.CannotSort(new object[]
					{
						sortMemberPath
					}), exception);
					base.Items.SortDescriptions.Clear();
				}
			}
		}

		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x060063C3 RID: 25539 RVA: 0x002A63D8 File Offset: 0x002A53D8
		// (set) Token: 0x060063C4 RID: 25540 RVA: 0x002A63E0 File Offset: 0x002A53E0
		private List<int> GroupingSortDescriptionIndices
		{
			get
			{
				return this._groupingSortDescriptionIndices;
			}
			set
			{
				this._groupingSortDescriptionIndices = value;
			}
		}

		// Token: 0x060063C5 RID: 25541 RVA: 0x002A63EC File Offset: 0x002A53EC
		private void OnItemsSortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this._ignoreSortDescriptionsChange || this.GroupingSortDescriptionIndices == null)
			{
				return;
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				int i = 0;
				int count = this.GroupingSortDescriptionIndices.Count;
				while (i < count)
				{
					if (this.GroupingSortDescriptionIndices[i] >= e.NewStartingIndex)
					{
						List<int> groupingSortDescriptionIndices = this.GroupingSortDescriptionIndices;
						int num = i;
						int num2 = groupingSortDescriptionIndices[num];
						groupingSortDescriptionIndices[num] = num2 + 1;
					}
					i++;
				}
				return;
			}
			case NotifyCollectionChangedAction.Remove:
			{
				int j = 0;
				int num3 = this.GroupingSortDescriptionIndices.Count;
				while (j < num3)
				{
					if (this.GroupingSortDescriptionIndices[j] > e.OldStartingIndex)
					{
						List<int> groupingSortDescriptionIndices2 = this.GroupingSortDescriptionIndices;
						int num2 = j;
						int num = groupingSortDescriptionIndices2[num2];
						groupingSortDescriptionIndices2[num2] = num - 1;
					}
					else if (this.GroupingSortDescriptionIndices[j] == e.OldStartingIndex)
					{
						this.GroupingSortDescriptionIndices.RemoveAt(j);
						j--;
						num3--;
					}
					j++;
				}
				return;
			}
			case NotifyCollectionChangedAction.Replace:
				this.GroupingSortDescriptionIndices.Remove(e.OldStartingIndex);
				return;
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Reset:
				this.GroupingSortDescriptionIndices.Clear();
				break;
			default:
				return;
			}
		}

		// Token: 0x060063C6 RID: 25542 RVA: 0x002A6514 File Offset: 0x002A5514
		private void RemoveGroupingSortDescriptions()
		{
			if (this.GroupingSortDescriptionIndices == null)
			{
				return;
			}
			bool ignoreSortDescriptionsChange = this._ignoreSortDescriptionsChange;
			this._ignoreSortDescriptionsChange = true;
			try
			{
				int i = 0;
				int count = this.GroupingSortDescriptionIndices.Count;
				while (i < count)
				{
					base.Items.SortDescriptions.RemoveAt(this.GroupingSortDescriptionIndices[i] - i);
					i++;
				}
				this.GroupingSortDescriptionIndices.Clear();
			}
			finally
			{
				this._ignoreSortDescriptionsChange = ignoreSortDescriptionsChange;
			}
		}

		// Token: 0x060063C7 RID: 25543 RVA: 0x002A6594 File Offset: 0x002A5594
		private static bool CanConvertToSortDescription(PropertyGroupDescription propertyGroupDescription)
		{
			return propertyGroupDescription != null && propertyGroupDescription.Converter == null && propertyGroupDescription.StringComparison == StringComparison.Ordinal;
		}

		// Token: 0x060063C8 RID: 25544 RVA: 0x002A65B0 File Offset: 0x002A55B0
		private void AddGroupingSortDescriptions()
		{
			bool ignoreSortDescriptionsChange = this._ignoreSortDescriptionsChange;
			this._ignoreSortDescriptionsChange = true;
			try
			{
				int index = 0;
				foreach (GroupDescription groupDescription in base.Items.GroupDescriptions)
				{
					PropertyGroupDescription propertyGroupDescription = groupDescription as PropertyGroupDescription;
					if (DataGrid.CanConvertToSortDescription(propertyGroupDescription))
					{
						SortDescription item = new SortDescription(propertyGroupDescription.PropertyName, ListSortDirection.Ascending);
						base.Items.SortDescriptions.Insert(index, item);
						if (this.GroupingSortDescriptionIndices == null)
						{
							this.GroupingSortDescriptionIndices = new List<int>();
						}
						this.GroupingSortDescriptionIndices.Add(index++);
					}
				}
			}
			finally
			{
				this._ignoreSortDescriptionsChange = ignoreSortDescriptionsChange;
			}
		}

		// Token: 0x060063C9 RID: 25545 RVA: 0x002A6670 File Offset: 0x002A5670
		private void RegenerateGroupingSortDescriptions()
		{
			this.RemoveGroupingSortDescriptions();
			this.AddGroupingSortDescriptions();
		}

		// Token: 0x060063CA RID: 25546 RVA: 0x002A6680 File Offset: 0x002A5680
		private void OnItemsGroupDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.EnqueueNewItemMarginComputation();
			if (!this._sortingStarted)
			{
				return;
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (DataGrid.CanConvertToSortDescription(e.NewItems[0] as PropertyGroupDescription))
				{
					this.RegenerateGroupingSortDescriptions();
					return;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (DataGrid.CanConvertToSortDescription(e.OldItems[0] as PropertyGroupDescription))
				{
					this.RegenerateGroupingSortDescriptions();
					return;
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (DataGrid.CanConvertToSortDescription(e.OldItems[0] as PropertyGroupDescription) || DataGrid.CanConvertToSortDescription(e.NewItems[0] as PropertyGroupDescription))
				{
					this.RegenerateGroupingSortDescriptions();
					return;
				}
				break;
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Reset:
				this.RemoveGroupingSortDescriptions();
				break;
			default:
				return;
			}
		}

		// Token: 0x140000FB RID: 251
		// (add) Token: 0x060063CB RID: 25547 RVA: 0x002A673C File Offset: 0x002A573C
		// (remove) Token: 0x060063CC RID: 25548 RVA: 0x002A6774 File Offset: 0x002A5774
		public event EventHandler AutoGeneratedColumns;

		// Token: 0x140000FC RID: 252
		// (add) Token: 0x060063CD RID: 25549 RVA: 0x002A67AC File Offset: 0x002A57AC
		// (remove) Token: 0x060063CE RID: 25550 RVA: 0x002A67E4 File Offset: 0x002A57E4
		public event EventHandler<DataGridAutoGeneratingColumnEventArgs> AutoGeneratingColumn;

		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x060063CF RID: 25551 RVA: 0x002A6819 File Offset: 0x002A5819
		// (set) Token: 0x060063D0 RID: 25552 RVA: 0x002A682B File Offset: 0x002A582B
		public bool AutoGenerateColumns
		{
			get
			{
				return (bool)base.GetValue(DataGrid.AutoGenerateColumnsProperty);
			}
			set
			{
				base.SetValue(DataGrid.AutoGenerateColumnsProperty, value);
			}
		}

		// Token: 0x060063D1 RID: 25553 RVA: 0x002A6839 File Offset: 0x002A5839
		protected virtual void OnAutoGeneratedColumns(EventArgs e)
		{
			if (this.AutoGeneratedColumns != null)
			{
				this.AutoGeneratedColumns(this, e);
			}
		}

		// Token: 0x060063D2 RID: 25554 RVA: 0x002A6850 File Offset: 0x002A5850
		protected virtual void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e)
		{
			if (this.AutoGeneratingColumn != null)
			{
				this.AutoGeneratingColumn(this, e);
			}
		}

		// Token: 0x060063D3 RID: 25555 RVA: 0x002A6868 File Offset: 0x002A5868
		protected override Size MeasureOverride(Size availableSize)
		{
			if (this._measureNeverInvoked)
			{
				this._measureNeverInvoked = false;
				if (this.AutoGenerateColumns)
				{
					this.AddAutoColumns();
				}
				this.InternalColumns.InitializeDisplayIndexMap();
				base.CoerceValue(DataGrid.FrozenColumnCountProperty);
				base.CoerceValue(DataGrid.CanUserAddRowsProperty);
				base.CoerceValue(DataGrid.CanUserDeleteRowsProperty);
				this.UpdateNewItemPlaceholder(false);
				this.EnsureItemBindingGroup();
				base.ItemBindingGroup.SharesProposedValues = true;
			}
			else if (this.DeferAutoGeneration && this.AutoGenerateColumns)
			{
				this.AddAutoColumns();
			}
			return base.MeasureOverride(availableSize);
		}

		// Token: 0x060063D4 RID: 25556 RVA: 0x002A68F6 File Offset: 0x002A58F6
		private void EnsureItemBindingGroup()
		{
			if (base.ItemBindingGroup == null)
			{
				this._defaultBindingGroup = new BindingGroup();
				base.SetCurrentValue(ItemsControl.ItemBindingGroupProperty, this._defaultBindingGroup);
			}
		}

		// Token: 0x060063D5 RID: 25557 RVA: 0x002A691C File Offset: 0x002A591C
		private void ClearSortDescriptionsOnItemsSourceChange()
		{
			base.Items.SortDescriptions.Clear();
			this._sortingStarted = false;
			List<int> groupingSortDescriptionIndices = this.GroupingSortDescriptionIndices;
			if (groupingSortDescriptionIndices != null)
			{
				groupingSortDescriptionIndices.Clear();
			}
			foreach (DataGridColumn dataGridColumn in this.Columns)
			{
				dataGridColumn.SortDirection = null;
			}
		}

		// Token: 0x060063D6 RID: 25558 RVA: 0x002A6998 File Offset: 0x002A5998
		private static object OnCoerceItemsSourceProperty(DependencyObject d, object baseValue)
		{
			DataGrid dataGrid = (DataGrid)d;
			if (baseValue != dataGrid._cachedItemsSource && dataGrid._cachedItemsSource != null)
			{
				dataGrid.ClearSortDescriptionsOnItemsSourceChange();
			}
			return baseValue;
		}

		// Token: 0x060063D7 RID: 25559 RVA: 0x002A69C4 File Offset: 0x002A59C4
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);
			if (newValue == null)
			{
				this.ClearSortDescriptionsOnItemsSourceChange();
			}
			this._cachedItemsSource = newValue;
			using (this.UpdateSelectedCells())
			{
				List<Tuple<int, int>> ranges = new List<Tuple<int, int>>();
				base.LocateSelectedItems(ranges, false);
				this._selectedCells.RestoreOnlyFullRows(ranges);
			}
			if (this.AutoGenerateColumns)
			{
				this.RegenerateAutoColumns();
			}
			this.InternalColumns.RefreshAutoWidthColumns = true;
			this.InternalColumns.InvalidateColumnWidthsComputation();
			base.CoerceValue(DataGrid.CanUserAddRowsProperty);
			base.CoerceValue(DataGrid.CanUserDeleteRowsProperty);
			DataGridHelper.TransferProperty(this, DataGrid.CanUserSortColumnsProperty);
			this.ResetRowHeaderActualWidth();
			this.UpdateNewItemPlaceholder(false);
			this.HasCellValidationError = false;
			this.HasRowValidationError = false;
		}

		// Token: 0x17001714 RID: 5908
		// (get) Token: 0x060063D8 RID: 25560 RVA: 0x002A6A88 File Offset: 0x002A5A88
		// (set) Token: 0x060063D9 RID: 25561 RVA: 0x002A6A90 File Offset: 0x002A5A90
		private bool DeferAutoGeneration { get; set; }

		// Token: 0x060063DA RID: 25562 RVA: 0x002A6A9C File Offset: 0x002A5A9C
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				if (this.DeferAutoGeneration)
				{
					this.AddAutoColumns();
					return;
				}
			}
			else
			{
				if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
				{
					if (!this.HasRowValidationError && !this.HasCellValidationError)
					{
						return;
					}
					using (IEnumerator enumerator = e.OldItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object item = enumerator.Current;
							if (this.IsAddingOrEditingRowItem(item))
							{
								this.HasRowValidationError = false;
								this.HasCellValidationError = false;
								break;
							}
						}
						return;
					}
				}
				if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					this.ResetRowHeaderActualWidth();
					this.HasRowValidationError = false;
					this.HasCellValidationError = false;
				}
			}
		}

		// Token: 0x060063DB RID: 25563 RVA: 0x002A6B64 File Offset: 0x002A5B64
		internal override void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			List<ItemsControl.ItemInfo> list = new List<ItemsControl.ItemInfo>();
			if (this._selectionAnchor != null)
			{
				list.Add(this._selectionAnchor.Value.ItemInfo);
			}
			if (this._editingRowInfo != null)
			{
				list.Add(this._editingRowInfo);
			}
			if (DataGrid.CellInfoNeedsAdjusting(this.CurrentCell))
			{
				list.Add(this.CurrentCell.ItemInfo);
			}
			base.AdjustItemInfos(e, list);
			base.AdjustItemInfoOverride(e);
		}

		// Token: 0x060063DC RID: 25564 RVA: 0x002A6BE8 File Offset: 0x002A5BE8
		internal override void AdjustItemInfosAfterGeneratorChangeOverride()
		{
			List<ItemsControl.ItemInfo> list = new List<ItemsControl.ItemInfo>();
			if (this._selectionAnchor != null)
			{
				list.Add(this._selectionAnchor.Value.ItemInfo);
			}
			if (this._editingRowInfo != null)
			{
				list.Add(this._editingRowInfo);
			}
			if (DataGrid.CellInfoNeedsAdjusting(this.CurrentCell))
			{
				list.Add(this.CurrentCell.ItemInfo);
			}
			base.AdjustItemInfosAfterGeneratorChange(list, false);
			base.AdjustItemInfosAfterGeneratorChangeOverride();
			this.AdjustPendingInfos();
		}

		// Token: 0x060063DD RID: 25565 RVA: 0x002A6C70 File Offset: 0x002A5C70
		private static bool CellInfoNeedsAdjusting(DataGridCellInfo cellInfo)
		{
			ItemsControl.ItemInfo itemInfo = cellInfo.ItemInfo;
			return itemInfo != null && itemInfo.Index != -1;
		}

		// Token: 0x060063DE RID: 25566 RVA: 0x002A6C9C File Offset: 0x002A5C9C
		private void AdjustPendingInfos()
		{
			int count;
			if (this._pendingInfos != null && this._pendingInfos.Count > 0 && (count = this._columns.Count) > 0)
			{
				using (this.UpdateSelectedCells())
				{
					for (int i = this._pendingInfos.Count - 1; i >= 0; i--)
					{
						ItemsControl.ItemInfo itemInfo = this._pendingInfos[i];
						if (itemInfo.Index >= 0)
						{
							this._pendingInfos.RemoveAt(i);
							this._selectedCells.AddRegion(itemInfo.Index, 0, 1, count);
						}
					}
				}
			}
		}

		// Token: 0x060063DF RID: 25567 RVA: 0x002A6D40 File Offset: 0x002A5D40
		private void AddAutoColumns()
		{
			ReadOnlyCollection<ItemPropertyInfo> itemProperties = ((IItemProperties)base.Items).ItemProperties;
			if (itemProperties == null && this.DataItemsCount == 0)
			{
				this.DeferAutoGeneration = true;
				return;
			}
			if (!this._measureNeverInvoked)
			{
				DataGrid.GenerateColumns(itemProperties, this, null);
				this.DeferAutoGeneration = false;
				this.OnAutoGeneratedColumns(EventArgs.Empty);
			}
		}

		// Token: 0x060063E0 RID: 25568 RVA: 0x002A6D90 File Offset: 0x002A5D90
		private void DeleteAutoColumns()
		{
			if (!this.DeferAutoGeneration && !this._measureNeverInvoked)
			{
				for (int i = this.Columns.Count - 1; i >= 0; i--)
				{
					if (this.Columns[i].IsAutoGenerated)
					{
						this.Columns.RemoveAt(i);
					}
				}
				return;
			}
			this.DeferAutoGeneration = false;
		}

		// Token: 0x060063E1 RID: 25569 RVA: 0x002A6DEC File Offset: 0x002A5DEC
		private void RegenerateAutoColumns()
		{
			this.DeleteAutoColumns();
			this.AddAutoColumns();
		}

		// Token: 0x060063E2 RID: 25570 RVA: 0x002A6DFC File Offset: 0x002A5DFC
		public static Collection<DataGridColumn> GenerateColumns(IItemProperties itemProperties)
		{
			if (itemProperties == null)
			{
				throw new ArgumentNullException("itemProperties");
			}
			Collection<DataGridColumn> collection = new Collection<DataGridColumn>();
			DataGrid.GenerateColumns(itemProperties.ItemProperties, null, collection);
			return collection;
		}

		// Token: 0x060063E3 RID: 25571 RVA: 0x002A6E2C File Offset: 0x002A5E2C
		private static void GenerateColumns(ReadOnlyCollection<ItemPropertyInfo> itemProperties, DataGrid dataGrid, Collection<DataGridColumn> columnCollection)
		{
			if (itemProperties != null && itemProperties.Count > 0)
			{
				foreach (ItemPropertyInfo itemPropertyInfo in itemProperties)
				{
					DataGridColumn dataGridColumn = DataGridColumn.CreateDefaultColumn(itemPropertyInfo);
					if (dataGrid != null)
					{
						DataGridAutoGeneratingColumnEventArgs dataGridAutoGeneratingColumnEventArgs = new DataGridAutoGeneratingColumnEventArgs(dataGridColumn, itemPropertyInfo);
						dataGrid.OnAutoGeneratingColumn(dataGridAutoGeneratingColumnEventArgs);
						if (!dataGridAutoGeneratingColumnEventArgs.Cancel && dataGridAutoGeneratingColumnEventArgs.Column != null)
						{
							dataGridAutoGeneratingColumnEventArgs.Column.IsAutoGenerated = true;
							dataGrid.Columns.Add(dataGridAutoGeneratingColumnEventArgs.Column);
						}
					}
					else
					{
						columnCollection.Add(dataGridColumn);
					}
				}
			}
		}

		// Token: 0x060063E4 RID: 25572 RVA: 0x002A6EC8 File Offset: 0x002A5EC8
		private static void OnAutoGenerateColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			bool flag = (bool)e.NewValue;
			DataGrid dataGrid = (DataGrid)d;
			if (flag)
			{
				dataGrid.AddAutoColumns();
				return;
			}
			dataGrid.DeleteAutoColumns();
		}

		// Token: 0x17001715 RID: 5909
		// (get) Token: 0x060063E5 RID: 25573 RVA: 0x002A6EF7 File Offset: 0x002A5EF7
		// (set) Token: 0x060063E6 RID: 25574 RVA: 0x002A6F09 File Offset: 0x002A5F09
		public int FrozenColumnCount
		{
			get
			{
				return (int)base.GetValue(DataGrid.FrozenColumnCountProperty);
			}
			set
			{
				base.SetValue(DataGrid.FrozenColumnCountProperty, value);
			}
		}

		// Token: 0x060063E7 RID: 25575 RVA: 0x002A6F1C File Offset: 0x002A5F1C
		private static object OnCoerceFrozenColumnCount(DependencyObject d, object baseValue)
		{
			DataGrid dataGrid = (DataGrid)d;
			if ((int)baseValue > dataGrid.Columns.Count)
			{
				return dataGrid.Columns.Count;
			}
			return baseValue;
		}

		// Token: 0x060063E8 RID: 25576 RVA: 0x002A0653 File Offset: 0x0029F653
		private static void OnFrozenColumnCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeadersPresenter);
		}

		// Token: 0x060063E9 RID: 25577 RVA: 0x002A6F55 File Offset: 0x002A5F55
		private static bool ValidateFrozenColumnCount(object value)
		{
			return (int)value >= 0;
		}

		// Token: 0x17001716 RID: 5910
		// (get) Token: 0x060063EA RID: 25578 RVA: 0x002A6F63 File Offset: 0x002A5F63
		// (set) Token: 0x060063EB RID: 25579 RVA: 0x002A6F75 File Offset: 0x002A5F75
		public double NonFrozenColumnsViewportHorizontalOffset
		{
			get
			{
				return (double)base.GetValue(DataGrid.NonFrozenColumnsViewportHorizontalOffsetProperty);
			}
			internal set
			{
				base.SetValue(DataGrid.NonFrozenColumnsViewportHorizontalOffsetPropertyKey, value);
			}
		}

		// Token: 0x060063EC RID: 25580 RVA: 0x002A6F88 File Offset: 0x002A5F88
		public override void OnApplyTemplate()
		{
			if (this.InternalItemsHost != null && !base.IsAncestorOf(this.InternalItemsHost))
			{
				this.InternalItemsHost = null;
			}
			this.CleanUpInternalScrollControls();
			base.OnApplyTemplate();
		}

		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x060063ED RID: 25581 RVA: 0x002A6FB3 File Offset: 0x002A5FB3
		// (set) Token: 0x060063EE RID: 25582 RVA: 0x002A6FC5 File Offset: 0x002A5FC5
		public bool EnableRowVirtualization
		{
			get
			{
				return (bool)base.GetValue(DataGrid.EnableRowVirtualizationProperty);
			}
			set
			{
				base.SetValue(DataGrid.EnableRowVirtualizationProperty, value);
			}
		}

		// Token: 0x060063EF RID: 25583 RVA: 0x002A6FD4 File Offset: 0x002A5FD4
		private static void OnEnableRowVirtualizationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			dataGrid.CoerceValue(VirtualizingPanel.IsVirtualizingProperty);
			Panel internalItemsHost = dataGrid.InternalItemsHost;
			if (internalItemsHost != null)
			{
				internalItemsHost.InvalidateMeasure();
				internalItemsHost.InvalidateArrange();
			}
		}

		// Token: 0x060063F0 RID: 25584 RVA: 0x002A7007 File Offset: 0x002A6007
		private static object OnCoerceIsVirtualizingProperty(DependencyObject d, object baseValue)
		{
			if (!DataGridHelper.IsDefaultValue(d, DataGrid.EnableRowVirtualizationProperty))
			{
				return d.GetValue(DataGrid.EnableRowVirtualizationProperty);
			}
			return baseValue;
		}

		// Token: 0x17001718 RID: 5912
		// (get) Token: 0x060063F1 RID: 25585 RVA: 0x002A7023 File Offset: 0x002A6023
		// (set) Token: 0x060063F2 RID: 25586 RVA: 0x002A7035 File Offset: 0x002A6035
		public bool EnableColumnVirtualization
		{
			get
			{
				return (bool)base.GetValue(DataGrid.EnableColumnVirtualizationProperty);
			}
			set
			{
				base.SetValue(DataGrid.EnableColumnVirtualizationProperty, value);
			}
		}

		// Token: 0x060063F3 RID: 25587 RVA: 0x002A0653 File Offset: 0x0029F653
		private static void OnEnableColumnVirtualizationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeadersPresenter);
		}

		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x060063F4 RID: 25588 RVA: 0x002A7043 File Offset: 0x002A6043
		// (set) Token: 0x060063F5 RID: 25589 RVA: 0x002A7055 File Offset: 0x002A6055
		public bool CanUserReorderColumns
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserReorderColumnsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserReorderColumnsProperty, value);
			}
		}

		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x060063F6 RID: 25590 RVA: 0x002A7063 File Offset: 0x002A6063
		// (set) Token: 0x060063F7 RID: 25591 RVA: 0x002A7075 File Offset: 0x002A6075
		public Style DragIndicatorStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.DragIndicatorStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.DragIndicatorStyleProperty, value);
			}
		}

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x060063F8 RID: 25592 RVA: 0x002A7083 File Offset: 0x002A6083
		// (set) Token: 0x060063F9 RID: 25593 RVA: 0x002A7095 File Offset: 0x002A6095
		public Style DropLocationIndicatorStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.DropLocationIndicatorStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.DropLocationIndicatorStyleProperty, value);
			}
		}

		// Token: 0x140000FD RID: 253
		// (add) Token: 0x060063FA RID: 25594 RVA: 0x002A70A4 File Offset: 0x002A60A4
		// (remove) Token: 0x060063FB RID: 25595 RVA: 0x002A70DC File Offset: 0x002A60DC
		public event EventHandler<DataGridColumnReorderingEventArgs> ColumnReordering;

		// Token: 0x140000FE RID: 254
		// (add) Token: 0x060063FC RID: 25596 RVA: 0x002A7114 File Offset: 0x002A6114
		// (remove) Token: 0x060063FD RID: 25597 RVA: 0x002A714C File Offset: 0x002A614C
		public event EventHandler<DragStartedEventArgs> ColumnHeaderDragStarted;

		// Token: 0x140000FF RID: 255
		// (add) Token: 0x060063FE RID: 25598 RVA: 0x002A7184 File Offset: 0x002A6184
		// (remove) Token: 0x060063FF RID: 25599 RVA: 0x002A71BC File Offset: 0x002A61BC
		public event EventHandler<DragDeltaEventArgs> ColumnHeaderDragDelta;

		// Token: 0x14000100 RID: 256
		// (add) Token: 0x06006400 RID: 25600 RVA: 0x002A71F4 File Offset: 0x002A61F4
		// (remove) Token: 0x06006401 RID: 25601 RVA: 0x002A722C File Offset: 0x002A622C
		public event EventHandler<DragCompletedEventArgs> ColumnHeaderDragCompleted;

		// Token: 0x14000101 RID: 257
		// (add) Token: 0x06006402 RID: 25602 RVA: 0x002A7264 File Offset: 0x002A6264
		// (remove) Token: 0x06006403 RID: 25603 RVA: 0x002A729C File Offset: 0x002A629C
		public event EventHandler<DataGridColumnEventArgs> ColumnReordered;

		// Token: 0x06006404 RID: 25604 RVA: 0x002A72D1 File Offset: 0x002A62D1
		protected internal virtual void OnColumnHeaderDragStarted(DragStartedEventArgs e)
		{
			if (this.ColumnHeaderDragStarted != null)
			{
				this.ColumnHeaderDragStarted(this, e);
			}
		}

		// Token: 0x06006405 RID: 25605 RVA: 0x002A72E8 File Offset: 0x002A62E8
		protected internal virtual void OnColumnReordering(DataGridColumnReorderingEventArgs e)
		{
			if (this.ColumnReordering != null)
			{
				this.ColumnReordering(this, e);
			}
		}

		// Token: 0x06006406 RID: 25606 RVA: 0x002A72FF File Offset: 0x002A62FF
		protected internal virtual void OnColumnHeaderDragDelta(DragDeltaEventArgs e)
		{
			if (this.ColumnHeaderDragDelta != null)
			{
				this.ColumnHeaderDragDelta(this, e);
			}
		}

		// Token: 0x06006407 RID: 25607 RVA: 0x002A7316 File Offset: 0x002A6316
		protected internal virtual void OnColumnHeaderDragCompleted(DragCompletedEventArgs e)
		{
			if (this.ColumnHeaderDragCompleted != null)
			{
				this.ColumnHeaderDragCompleted(this, e);
			}
		}

		// Token: 0x06006408 RID: 25608 RVA: 0x002A732D File Offset: 0x002A632D
		protected internal virtual void OnColumnReordered(DataGridColumnEventArgs e)
		{
			if (this.ColumnReordered != null)
			{
				this.ColumnReordered(this, e);
			}
		}

		// Token: 0x06006409 RID: 25609 RVA: 0x002A2D29 File Offset: 0x002A1D29
		private static void OnClipboardCopyModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x0600640A RID: 25610 RVA: 0x002A7344 File Offset: 0x002A6344
		// (set) Token: 0x0600640B RID: 25611 RVA: 0x002A7356 File Offset: 0x002A6356
		public DataGridClipboardCopyMode ClipboardCopyMode
		{
			get
			{
				return (DataGridClipboardCopyMode)base.GetValue(DataGrid.ClipboardCopyModeProperty);
			}
			set
			{
				base.SetValue(DataGrid.ClipboardCopyModeProperty, value);
			}
		}

		// Token: 0x0600640C RID: 25612 RVA: 0x002A7369 File Offset: 0x002A6369
		private static void OnCanExecuteCopy(object target, CanExecuteRoutedEventArgs args)
		{
			((DataGrid)target).OnCanExecuteCopy(args);
		}

		// Token: 0x0600640D RID: 25613 RVA: 0x002A7377 File Offset: 0x002A6377
		protected virtual void OnCanExecuteCopy(CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = (this.ClipboardCopyMode != DataGridClipboardCopyMode.None && this._selectedCells.Count > 0);
			args.Handled = true;
		}

		// Token: 0x0600640E RID: 25614 RVA: 0x002A739F File Offset: 0x002A639F
		private static void OnExecutedCopy(object target, ExecutedRoutedEventArgs args)
		{
			((DataGrid)target).OnExecutedCopy(args);
		}

		// Token: 0x0600640F RID: 25615 RVA: 0x002A73B0 File Offset: 0x002A63B0
		protected virtual void OnExecutedCopy(ExecutedRoutedEventArgs args)
		{
			if (this.ClipboardCopyMode == DataGridClipboardCopyMode.None)
			{
				throw new NotSupportedException(SR.Get("ClipboardCopyMode_Disabled"));
			}
			args.Handled = true;
			Collection<string> collection = new Collection<string>(new string[]
			{
				DataFormats.Html,
				DataFormats.Text,
				DataFormats.UnicodeText,
				DataFormats.CommaSeparatedValue
			});
			Dictionary<string, StringBuilder> dictionary = new Dictionary<string, StringBuilder>(collection.Count);
			foreach (string key in collection)
			{
				dictionary[key] = new StringBuilder();
			}
			int startColumnDisplayIndex;
			int endColumnDisplayIndex;
			int num;
			int num2;
			if (this._selectedCells.GetSelectionRange(out startColumnDisplayIndex, out endColumnDisplayIndex, out num, out num2))
			{
				if (this.ClipboardCopyMode == DataGridClipboardCopyMode.IncludeHeader)
				{
					DataGridRowClipboardEventArgs dataGridRowClipboardEventArgs = new DataGridRowClipboardEventArgs(null, startColumnDisplayIndex, endColumnDisplayIndex, true);
					this.OnCopyingRowClipboardContent(dataGridRowClipboardEventArgs);
					foreach (string text in collection)
					{
						dictionary[text].Append(dataGridRowClipboardEventArgs.FormatClipboardCellValues(text));
					}
				}
				for (int i = num; i <= num2; i++)
				{
					object item = base.Items[i];
					if (this._selectedCells.Intersects(i))
					{
						DataGridRowClipboardEventArgs dataGridRowClipboardEventArgs2 = new DataGridRowClipboardEventArgs(item, startColumnDisplayIndex, endColumnDisplayIndex, false, i);
						this.OnCopyingRowClipboardContent(dataGridRowClipboardEventArgs2);
						foreach (string text2 in collection)
						{
							dictionary[text2].Append(dataGridRowClipboardEventArgs2.FormatClipboardCellValues(text2));
						}
					}
				}
			}
			DataGridClipboardHelper.GetClipboardContentForHtml(dictionary[DataFormats.Html]);
			DataObject dataObject = new DataObject();
			foreach (string text3 in collection)
			{
				dataObject.SetData(text3, dictionary[text3].ToString(), false);
			}
			try
			{
				Clipboard.CriticalSetDataObject(dataObject, true);
			}
			catch (ExternalException)
			{
			}
		}

		// Token: 0x06006410 RID: 25616 RVA: 0x002A75F4 File Offset: 0x002A65F4
		protected virtual void OnCopyingRowClipboardContent(DataGridRowClipboardEventArgs args)
		{
			if (args.IsColumnHeadersRow)
			{
				for (int i = args.StartColumnDisplayIndex; i <= args.EndColumnDisplayIndex; i++)
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
					if (dataGridColumn.IsVisible)
					{
						args.ClipboardRowContent.Add(new DataGridClipboardCellContent(args.Item, dataGridColumn, dataGridColumn.Header));
					}
				}
			}
			else
			{
				int num = args.RowIndexHint;
				if (num < 0)
				{
					num = base.Items.IndexOf(args.Item);
				}
				if (this._selectedCells.Intersects(num))
				{
					for (int j = args.StartColumnDisplayIndex; j <= args.EndColumnDisplayIndex; j++)
					{
						DataGridColumn dataGridColumn2 = this.ColumnFromDisplayIndex(j);
						if (dataGridColumn2.IsVisible)
						{
							object content = null;
							if (this._selectedCells.Contains(num, j))
							{
								content = dataGridColumn2.OnCopyingCellClipboardContent(args.Item);
							}
							args.ClipboardRowContent.Add(new DataGridClipboardCellContent(args.Item, dataGridColumn2, content));
						}
					}
				}
			}
			if (this.CopyingRowClipboardContent != null)
			{
				this.CopyingRowClipboardContent(this, args);
			}
		}

		// Token: 0x14000102 RID: 258
		// (add) Token: 0x06006411 RID: 25617 RVA: 0x002A76F4 File Offset: 0x002A66F4
		// (remove) Token: 0x06006412 RID: 25618 RVA: 0x002A772C File Offset: 0x002A672C
		public event EventHandler<DataGridRowClipboardEventArgs> CopyingRowClipboardContent;

		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x06006413 RID: 25619 RVA: 0x002A7761 File Offset: 0x002A6761
		// (set) Token: 0x06006414 RID: 25620 RVA: 0x002A7773 File Offset: 0x002A6773
		internal double CellsPanelActualWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.CellsPanelActualWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.CellsPanelActualWidthProperty, value);
			}
		}

		// Token: 0x06006415 RID: 25621 RVA: 0x002A7788 File Offset: 0x002A6788
		private static void CellsPanelActualWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			double value = (double)e.OldValue;
			double value2 = (double)e.NewValue;
			if (!DoubleUtil.AreClose(value, value2))
			{
				((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeadersPresenter);
			}
		}

		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x06006416 RID: 25622 RVA: 0x002A77C5 File Offset: 0x002A67C5
		// (set) Token: 0x06006417 RID: 25623 RVA: 0x002A77D7 File Offset: 0x002A67D7
		public double CellsPanelHorizontalOffset
		{
			get
			{
				return (double)base.GetValue(DataGrid.CellsPanelHorizontalOffsetProperty);
			}
			private set
			{
				base.SetValue(DataGrid.CellsPanelHorizontalOffsetPropertyKey, value);
			}
		}

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x06006418 RID: 25624 RVA: 0x002A77EA File Offset: 0x002A67EA
		// (set) Token: 0x06006419 RID: 25625 RVA: 0x002A77F2 File Offset: 0x002A67F2
		private bool CellsPanelHorizontalOffsetComputationPending { get; set; }

		// Token: 0x0600641A RID: 25626 RVA: 0x002A77FB File Offset: 0x002A67FB
		internal void QueueInvalidateCellsPanelHorizontalOffset()
		{
			if (!this.CellsPanelHorizontalOffsetComputationPending)
			{
				base.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.InvalidateCellsPanelHorizontalOffset), DispatcherPriority.Loaded, new object[]
				{
					this
				});
				this.CellsPanelHorizontalOffsetComputationPending = true;
			}
		}

		// Token: 0x0600641B RID: 25627 RVA: 0x002A7830 File Offset: 0x002A6830
		private object InvalidateCellsPanelHorizontalOffset(object args)
		{
			if (!this.CellsPanelHorizontalOffsetComputationPending)
			{
				return null;
			}
			IProvideDataGridColumn anyCellOrColumnHeader = this.GetAnyCellOrColumnHeader();
			if (anyCellOrColumnHeader != null)
			{
				this.CellsPanelHorizontalOffset = DataGridHelper.GetParentCellsPanelHorizontalOffset(anyCellOrColumnHeader);
			}
			else if (!double.IsNaN(this.RowHeaderWidth))
			{
				this.CellsPanelHorizontalOffset = this.RowHeaderWidth;
			}
			else
			{
				this.CellsPanelHorizontalOffset = 0.0;
			}
			this.CellsPanelHorizontalOffsetComputationPending = false;
			return null;
		}

		// Token: 0x0600641C RID: 25628 RVA: 0x002A7894 File Offset: 0x002A6894
		internal IProvideDataGridColumn GetAnyCellOrColumnHeader()
		{
			if (this._rowTrackingRoot != null)
			{
				for (ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
				{
					if (containerTracking.Container.IsVisible)
					{
						DataGridCellsPresenter cellsPresenter = containerTracking.Container.CellsPresenter;
						if (cellsPresenter != null)
						{
							for (ContainerTracking<DataGridCell> containerTracking2 = cellsPresenter.CellTrackingRoot; containerTracking2 != null; containerTracking2 = containerTracking2.Next)
							{
								if (containerTracking2.Container.IsVisible)
								{
									return containerTracking2.Container;
								}
							}
						}
					}
				}
			}
			if (this.ColumnHeadersPresenter != null)
			{
				for (ContainerTracking<DataGridColumnHeader> containerTracking3 = this.ColumnHeadersPresenter.HeaderTrackingRoot; containerTracking3 != null; containerTracking3 = containerTracking3.Next)
				{
					if (containerTracking3.Container.IsVisible)
					{
						return containerTracking3.Container;
					}
				}
			}
			return null;
		}

		// Token: 0x0600641D RID: 25629 RVA: 0x002A7934 File Offset: 0x002A6934
		internal double GetViewportWidthForColumns()
		{
			if (this.InternalScrollHost == null)
			{
				return 0.0;
			}
			return this.InternalScrollHost.ViewportWidth - this.CellsPanelHorizontalOffset;
		}

		// Token: 0x0600641E RID: 25630 RVA: 0x002A795C File Offset: 0x002A695C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Normal"
				});
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x0600641F RID: 25631 RVA: 0x002A79AC File Offset: 0x002A69AC
		internal static object NewItemPlaceholder
		{
			get
			{
				return DataGrid._newItemPlaceholder;
			}
		}

		// Token: 0x040032B3 RID: 12979
		public static readonly DependencyProperty CanUserResizeColumnsProperty = DependencyProperty.Register("CanUserResizeColumns", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnNotifyColumnAndColumnHeaderPropertyChanged)));

		// Token: 0x040032B4 RID: 12980
		public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(DataGridLength), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridLength.SizeToHeader));

		// Token: 0x040032B5 RID: 12981
		public static readonly DependencyProperty MinColumnWidthProperty = DependencyProperty.Register("MinColumnWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(20.0, new PropertyChangedCallback(DataGrid.OnColumnSizeConstraintChanged)), new ValidateValueCallback(DataGrid.ValidateMinColumnWidth));

		// Token: 0x040032B6 RID: 12982
		public static readonly DependencyProperty MaxColumnWidthProperty = DependencyProperty.Register("MaxColumnWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DataGrid.OnColumnSizeConstraintChanged)), new ValidateValueCallback(DataGrid.ValidateMaxColumnWidth));

		// Token: 0x040032B7 RID: 12983
		private static readonly UncommonField<int> BringColumnIntoViewRetryCountField = new UncommonField<int>(0);

		// Token: 0x040032B8 RID: 12984
		private const int MaxBringColumnIntoViewRetries = 4;

		// Token: 0x040032BA RID: 12986
		public static readonly DependencyProperty GridLinesVisibilityProperty = DependencyProperty.Register("GridLinesVisibility", typeof(DataGridGridLinesVisibility), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridGridLinesVisibility.All, new PropertyChangedCallback(DataGrid.OnNotifyGridLinePropertyChanged)));

		// Token: 0x040032BB RID: 12987
		public static readonly DependencyProperty HorizontalGridLinesBrushProperty = DependencyProperty.Register("HorizontalGridLinesBrush", typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata(Brushes.Black, new PropertyChangedCallback(DataGrid.OnNotifyGridLinePropertyChanged)));

		// Token: 0x040032BC RID: 12988
		public static readonly DependencyProperty VerticalGridLinesBrushProperty = DependencyProperty.Register("VerticalGridLinesBrush", typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata(Brushes.Black, new PropertyChangedCallback(DataGrid.OnNotifyGridLinePropertyChanged)));

		// Token: 0x040032BD RID: 12989
		public static readonly DependencyProperty RowStyleProperty = DependencyProperty.Register("RowStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnRowStyleChanged)));

		// Token: 0x040032BE RID: 12990
		public static readonly DependencyProperty RowValidationErrorTemplateProperty = DependencyProperty.Register("RowValidationErrorTemplate", typeof(ControlTemplate), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowPropertyChanged)));

		// Token: 0x040032BF RID: 12991
		public static readonly DependencyProperty RowStyleSelectorProperty = DependencyProperty.Register("RowStyleSelector", typeof(StyleSelector), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnRowStyleSelectorChanged)));

		// Token: 0x040032C0 RID: 12992
		public static readonly DependencyProperty RowBackgroundProperty = DependencyProperty.Register("RowBackground", typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowPropertyChanged)));

		// Token: 0x040032C1 RID: 12993
		public static readonly DependencyProperty AlternatingRowBackgroundProperty = DependencyProperty.Register("AlternatingRowBackground", typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyDataGridAndRowPropertyChanged)));

		// Token: 0x040032C2 RID: 12994
		public static readonly DependencyProperty RowHeightProperty = DependencyProperty.Register("RowHeight", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(DataGrid.OnNotifyCellsPresenterPropertyChanged)));

		// Token: 0x040032C3 RID: 12995
		public static readonly DependencyProperty MinRowHeightProperty = DependencyProperty.Register("MinRowHeight", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.OnNotifyCellsPresenterPropertyChanged)));

		// Token: 0x040032C6 RID: 12998
		public static readonly DependencyProperty RowHeaderWidthProperty = DependencyProperty.Register("RowHeaderWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(DataGrid.OnNotifyRowHeaderWidthPropertyChanged)));

		// Token: 0x040032C7 RID: 12999
		private static readonly DependencyPropertyKey RowHeaderActualWidthPropertyKey = DependencyProperty.RegisterReadOnly("RowHeaderActualWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.OnNotifyRowHeaderPropertyChanged)));

		// Token: 0x040032C8 RID: 13000
		public static readonly DependencyProperty RowHeaderActualWidthProperty = DataGrid.RowHeaderActualWidthPropertyKey.DependencyProperty;

		// Token: 0x040032C9 RID: 13001
		public static readonly DependencyProperty ColumnHeaderHeightProperty = DependencyProperty.Register("ColumnHeaderHeight", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(DataGrid.OnNotifyColumnHeaderPropertyChanged)));

		// Token: 0x040032CA RID: 13002
		public static readonly DependencyProperty HeadersVisibilityProperty = DependencyProperty.Register("HeadersVisibility", typeof(DataGridHeadersVisibility), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridHeadersVisibility.All));

		// Token: 0x040032CB RID: 13003
		public static readonly DependencyProperty CellStyleProperty = DependencyProperty.Register("CellStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyColumnAndCellPropertyChanged)));

		// Token: 0x040032CC RID: 13004
		public static readonly DependencyProperty ColumnHeaderStyleProperty = DependencyProperty.Register("ColumnHeaderStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyColumnAndColumnHeaderPropertyChanged)));

		// Token: 0x040032CD RID: 13005
		public static readonly DependencyProperty RowHeaderStyleProperty = DependencyProperty.Register("RowHeaderStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndRowHeaderPropertyChanged)));

		// Token: 0x040032CE RID: 13006
		public static readonly DependencyProperty RowHeaderTemplateProperty = DependencyProperty.Register("RowHeaderTemplate", typeof(DataTemplate), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndRowHeaderPropertyChanged)));

		// Token: 0x040032CF RID: 13007
		public static readonly DependencyProperty RowHeaderTemplateSelectorProperty = DependencyProperty.Register("RowHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndRowHeaderPropertyChanged)));

		// Token: 0x040032D0 RID: 13008
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(DataGrid), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));

		// Token: 0x040032D1 RID: 13009
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(DataGrid), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));

		// Token: 0x040032D2 RID: 13010
		internal static readonly DependencyProperty HorizontalScrollOffsetProperty = DependencyProperty.Register("HorizontalScrollOffset", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.OnNotifyHorizontalOffsetPropertyChanged)));

		// Token: 0x040032D3 RID: 13011
		public static readonly RoutedCommand BeginEditCommand = new RoutedCommand("BeginEdit", typeof(DataGrid));

		// Token: 0x040032D4 RID: 13012
		public static readonly RoutedCommand CommitEditCommand = new RoutedCommand("CommitEdit", typeof(DataGrid));

		// Token: 0x040032D5 RID: 13013
		public static readonly RoutedCommand CancelEditCommand = new RoutedCommand("CancelEdit", typeof(DataGrid));

		// Token: 0x040032D8 RID: 13016
		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGrid.OnIsReadOnlyChanged)));

		// Token: 0x040032D9 RID: 13017
		public static readonly DependencyProperty CurrentItemProperty = DependencyProperty.Register("CurrentItem", typeof(object), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnCurrentItemChanged)));

		// Token: 0x040032DA RID: 13018
		public static readonly DependencyProperty CurrentColumnProperty = DependencyProperty.Register("CurrentColumn", typeof(DataGridColumn), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnCurrentColumnChanged)));

		// Token: 0x040032DB RID: 13019
		public static readonly DependencyProperty CurrentCellProperty = DependencyProperty.Register("CurrentCell", typeof(DataGridCellInfo), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridCellInfo.Unset, new PropertyChangedCallback(DataGrid.OnCurrentCellChanged)));

		// Token: 0x040032DF RID: 13023
		public static readonly DependencyProperty CanUserAddRowsProperty = DependencyProperty.Register("CanUserAddRows", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnCanUserAddRowsChanged), new CoerceValueCallback(DataGrid.OnCoerceCanUserAddRows)));

		// Token: 0x040032E0 RID: 13024
		public static readonly DependencyProperty CanUserDeleteRowsProperty = DependencyProperty.Register("CanUserDeleteRows", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnCanUserDeleteRowsChanged), new CoerceValueCallback(DataGrid.OnCoerceCanUserDeleteRows)));

		// Token: 0x040032E3 RID: 13027
		public static readonly DependencyProperty RowDetailsVisibilityModeProperty = DependencyProperty.Register("RowDetailsVisibilityMode", typeof(DataGridRowDetailsVisibilityMode), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridRowDetailsVisibilityMode.VisibleWhenSelected, new PropertyChangedCallback(DataGrid.OnNotifyRowAndDetailsPropertyChanged)));

		// Token: 0x040032E4 RID: 13028
		public static readonly DependencyProperty AreRowDetailsFrozenProperty = DependencyProperty.Register("AreRowDetailsFrozen", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(false));

		// Token: 0x040032E5 RID: 13029
		public static readonly DependencyProperty RowDetailsTemplateProperty = DependencyProperty.Register("RowDetailsTemplate", typeof(DataTemplate), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndDetailsPropertyChanged)));

		// Token: 0x040032E6 RID: 13030
		public static readonly DependencyProperty RowDetailsTemplateSelectorProperty = DependencyProperty.Register("RowDetailsTemplateSelector", typeof(DataTemplateSelector), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndDetailsPropertyChanged)));

		// Token: 0x040032EA RID: 13034
		public static readonly DependencyProperty CanUserResizeRowsProperty = DependencyProperty.Register("CanUserResizeRows", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnNotifyRowHeaderPropertyChanged)));

		// Token: 0x040032EB RID: 13035
		private static readonly DependencyPropertyKey NewItemMarginPropertyKey = DependencyProperty.RegisterReadOnly("NewItemMargin", typeof(Thickness), typeof(DataGrid), new FrameworkPropertyMetadata(new Thickness(0.0)));

		// Token: 0x040032EC RID: 13036
		public static readonly DependencyProperty NewItemMarginProperty = DataGrid.NewItemMarginPropertyKey.DependencyProperty;

		// Token: 0x040032EE RID: 13038
		public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(DataGridSelectionMode), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridSelectionMode.Extended, new PropertyChangedCallback(DataGrid.OnSelectionModeChanged)));

		// Token: 0x040032EF RID: 13039
		public static readonly DependencyProperty SelectionUnitProperty = DependencyProperty.Register("SelectionUnit", typeof(DataGridSelectionUnit), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridSelectionUnit.FullRow, new PropertyChangedCallback(DataGrid.OnSelectionUnitChanged)));

		// Token: 0x040032F0 RID: 13040
		public static readonly DependencyProperty CanUserSortColumnsProperty = DependencyProperty.Register("CanUserSortColumns", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnCanUserSortColumnsPropertyChanged), new CoerceValueCallback(DataGrid.OnCoerceCanUserSortColumns)));

		// Token: 0x040032F4 RID: 13044
		public static readonly DependencyProperty AutoGenerateColumnsProperty = DependencyProperty.Register("AutoGenerateColumns", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnAutoGenerateColumnsPropertyChanged)));

		// Token: 0x040032F6 RID: 13046
		public static readonly DependencyProperty FrozenColumnCountProperty = DependencyProperty.Register("FrozenColumnCount", typeof(int), typeof(DataGrid), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(DataGrid.OnFrozenColumnCountPropertyChanged), new CoerceValueCallback(DataGrid.OnCoerceFrozenColumnCount)), new ValidateValueCallback(DataGrid.ValidateFrozenColumnCount));

		// Token: 0x040032F7 RID: 13047
		private static readonly DependencyPropertyKey NonFrozenColumnsViewportHorizontalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("NonFrozenColumnsViewportHorizontalOffset", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0));

		// Token: 0x040032F8 RID: 13048
		public static readonly DependencyProperty NonFrozenColumnsViewportHorizontalOffsetProperty = DataGrid.NonFrozenColumnsViewportHorizontalOffsetPropertyKey.DependencyProperty;

		// Token: 0x040032F9 RID: 13049
		public static readonly DependencyProperty EnableRowVirtualizationProperty = DependencyProperty.Register("EnableRowVirtualization", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnEnableRowVirtualizationChanged)));

		// Token: 0x040032FA RID: 13050
		public static readonly DependencyProperty EnableColumnVirtualizationProperty = DependencyProperty.Register("EnableColumnVirtualization", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGrid.OnEnableColumnVirtualizationChanged)));

		// Token: 0x040032FB RID: 13051
		public static readonly DependencyProperty CanUserReorderColumnsProperty = DependencyProperty.Register("CanUserReorderColumns", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnNotifyColumnPropertyChanged)));

		// Token: 0x040032FC RID: 13052
		public static readonly DependencyProperty DragIndicatorStyleProperty = DependencyProperty.Register("DragIndicatorStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyColumnPropertyChanged)));

		// Token: 0x040032FD RID: 13053
		public static readonly DependencyProperty DropLocationIndicatorStyleProperty = DependencyProperty.Register("DropLocationIndicatorStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null));

		// Token: 0x04003303 RID: 13059
		public static readonly DependencyProperty ClipboardCopyModeProperty = DependencyProperty.Register("ClipboardCopyMode", typeof(DataGridClipboardCopyMode), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridClipboardCopyMode.ExcludeHeader, new PropertyChangedCallback(DataGrid.OnClipboardCopyModeChanged)));

		// Token: 0x04003305 RID: 13061
		internal static readonly DependencyProperty CellsPanelActualWidthProperty = DependencyProperty.Register("CellsPanelActualWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.CellsPanelActualWidthChanged)));

		// Token: 0x04003306 RID: 13062
		private static readonly DependencyPropertyKey CellsPanelHorizontalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("CellsPanelHorizontalOffset", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.OnNotifyHorizontalOffsetPropertyChanged)));

		// Token: 0x04003307 RID: 13063
		public static readonly DependencyProperty CellsPanelHorizontalOffsetProperty = DataGrid.CellsPanelHorizontalOffsetPropertyKey.DependencyProperty;

		// Token: 0x04003309 RID: 13065
		private static IValueConverter _headersVisibilityConverter;

		// Token: 0x0400330A RID: 13066
		private static IValueConverter _rowDetailsScrollingConverter;

		// Token: 0x0400330B RID: 13067
		private static object _newItemPlaceholder = new NamedObject("DataGrid.NewItemPlaceholder");

		// Token: 0x0400330C RID: 13068
		private DataGridColumnCollection _columns;

		// Token: 0x0400330D RID: 13069
		private ContainerTracking<DataGridRow> _rowTrackingRoot;

		// Token: 0x0400330E RID: 13070
		private DataGridColumnHeadersPresenter _columnHeadersPresenter;

		// Token: 0x0400330F RID: 13071
		private DataGridCell _currentCellContainer;

		// Token: 0x04003310 RID: 13072
		private DataGridCell _pendingCurrentCellContainer;

		// Token: 0x04003311 RID: 13073
		private SelectedCellsCollection _selectedCells;

		// Token: 0x04003312 RID: 13074
		private List<ItemsControl.ItemInfo> _pendingInfos;

		// Token: 0x04003313 RID: 13075
		private DataGridCellInfo? _selectionAnchor;

		// Token: 0x04003314 RID: 13076
		private bool _isDraggingSelection;

		// Token: 0x04003315 RID: 13077
		private bool _isRowDragging;

		// Token: 0x04003316 RID: 13078
		private Panel _internalItemsHost;

		// Token: 0x04003317 RID: 13079
		private ScrollViewer _internalScrollHost;

		// Token: 0x04003318 RID: 13080
		private ScrollContentPresenter _internalScrollContentPresenter;

		// Token: 0x04003319 RID: 13081
		private DispatcherTimer _autoScrollTimer;

		// Token: 0x0400331A RID: 13082
		private bool _hasAutoScrolled;

		// Token: 0x0400331B RID: 13083
		private VirtualizedCellInfoCollection _pendingSelectedCells;

		// Token: 0x0400331C RID: 13084
		private VirtualizedCellInfoCollection _pendingUnselectedCells;

		// Token: 0x0400331D RID: 13085
		private bool _measureNeverInvoked = true;

		// Token: 0x0400331E RID: 13086
		private bool _updatingSelectedCells;

		// Token: 0x0400331F RID: 13087
		private Visibility _placeholderVisibility = Visibility.Collapsed;

		// Token: 0x04003320 RID: 13088
		private Point _dragPoint;

		// Token: 0x04003321 RID: 13089
		private List<int> _groupingSortDescriptionIndices;

		// Token: 0x04003322 RID: 13090
		private bool _ignoreSortDescriptionsChange;

		// Token: 0x04003323 RID: 13091
		private bool _sortingStarted;

		// Token: 0x04003324 RID: 13092
		private ObservableCollection<ValidationRule> _rowValidationRules;

		// Token: 0x04003325 RID: 13093
		private BindingGroup _defaultBindingGroup;

		// Token: 0x04003326 RID: 13094
		private ItemsControl.ItemInfo _editingRowInfo;

		// Token: 0x04003327 RID: 13095
		private bool _hasCellValidationError;

		// Token: 0x04003328 RID: 13096
		private bool _hasRowValidationError;

		// Token: 0x04003329 RID: 13097
		private IEnumerable _cachedItemsSource;

		// Token: 0x0400332A RID: 13098
		private DataGridItemAttachedStorage _itemAttachedStorage = new DataGridItemAttachedStorage();

		// Token: 0x0400332B RID: 13099
		private bool _viewportWidthChangeNotificationPending;

		// Token: 0x0400332C RID: 13100
		private double _originalViewportWidth;

		// Token: 0x0400332D RID: 13101
		private double _finalViewportWidth;

		// Token: 0x0400332E RID: 13102
		private Dictionary<DataGridColumn, DataGrid.CellAutomationValueHolder> _editingCellAutomationValueHolders = new Dictionary<DataGridColumn, DataGrid.CellAutomationValueHolder>();

		// Token: 0x0400332F RID: 13103
		private DataGridCell _focusedCell;

		// Token: 0x04003330 RID: 13104
		private bool _newItemMarginComputationPending;

		// Token: 0x04003331 RID: 13105
		private const string ItemsPanelPartName = "PART_RowsPresenter";

		// Token: 0x02000BC8 RID: 3016
		private class ChangingSelectedCellsHelper : IDisposable
		{
			// Token: 0x06008F57 RID: 36695 RVA: 0x00344045 File Offset: 0x00343045
			internal ChangingSelectedCellsHelper(DataGrid dataGrid)
			{
				this._dataGrid = dataGrid;
				this._wasUpdatingSelectedCells = this._dataGrid.IsUpdatingSelectedCells;
				if (!this._wasUpdatingSelectedCells)
				{
					this._dataGrid.BeginUpdateSelectedCells();
				}
			}

			// Token: 0x06008F58 RID: 36696 RVA: 0x00344078 File Offset: 0x00343078
			public void Dispose()
			{
				GC.SuppressFinalize(this);
				if (!this._wasUpdatingSelectedCells)
				{
					this._dataGrid.EndUpdateSelectedCells();
				}
			}

			// Token: 0x040049E4 RID: 18916
			private DataGrid _dataGrid;

			// Token: 0x040049E5 RID: 18917
			private bool _wasUpdatingSelectedCells;
		}

		// Token: 0x02000BC9 RID: 3017
		[Flags]
		private enum RelativeMousePositions
		{
			// Token: 0x040049E7 RID: 18919
			Over = 0,
			// Token: 0x040049E8 RID: 18920
			Above = 1,
			// Token: 0x040049E9 RID: 18921
			Below = 2,
			// Token: 0x040049EA RID: 18922
			Left = 4,
			// Token: 0x040049EB RID: 18923
			Right = 8
		}

		// Token: 0x02000BCA RID: 3018
		internal class CellAutomationValueHolder
		{
			// Token: 0x06008F59 RID: 36697 RVA: 0x00344093 File Offset: 0x00343093
			public CellAutomationValueHolder(DataGridCell cell)
			{
				this._cell = cell;
				this.Initialize(cell.RowDataItem, cell.Column);
			}

			// Token: 0x06008F5A RID: 36698 RVA: 0x003440B4 File Offset: 0x003430B4
			public CellAutomationValueHolder(object item, DataGridColumn column)
			{
				this.Initialize(item, column);
			}

			// Token: 0x06008F5B RID: 36699 RVA: 0x003440C4 File Offset: 0x003430C4
			private void Initialize(object item, DataGridColumn column)
			{
				this._item = item;
				this._column = column;
				this._value = this.GetValue();
			}

			// Token: 0x17001F57 RID: 8023
			// (get) Token: 0x06008F5C RID: 36700 RVA: 0x003440E0 File Offset: 0x003430E0
			public string Value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x06008F5D RID: 36701 RVA: 0x003440E8 File Offset: 0x003430E8
			public void TrackValue()
			{
				string value = this.GetValue();
				if (value != this._value)
				{
					if (AutomationPeer.ListenerExists(AutomationEvents.PropertyChanged))
					{
						DataGridColumn dataGridColumn = (this._cell != null) ? this._cell.Column : this._column;
						DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(dataGridColumn.DataGridOwner) as DataGridAutomationPeer;
						if (dataGridAutomationPeer != null)
						{
							object item = (this._cell != null) ? this._cell.DataContext : this._item;
							DataGridItemAutomationPeer dataGridItemAutomationPeer = dataGridAutomationPeer.FindOrCreateItemAutomationPeer(item) as DataGridItemAutomationPeer;
							if (dataGridItemAutomationPeer != null)
							{
								DataGridCellItemAutomationPeer orCreateCellItemPeer = dataGridItemAutomationPeer.GetOrCreateCellItemPeer(dataGridColumn);
								if (orCreateCellItemPeer != null)
								{
									orCreateCellItemPeer.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, this._value, value);
								}
							}
						}
					}
					this._value = value;
				}
			}

			// Token: 0x06008F5E RID: 36702 RVA: 0x0034419C File Offset: 0x0034319C
			private string GetValue()
			{
				string result;
				if (this._column.ClipboardContentBinding == null)
				{
					result = null;
				}
				else if (this._inSetValue)
				{
					result = (string)this._cell.GetValue(DataGrid.CellAutomationValueHolder.CellContentProperty);
				}
				else
				{
					FrameworkElement frameworkElement;
					if (this._cell != null)
					{
						frameworkElement = this._cell;
					}
					else
					{
						frameworkElement = new FrameworkElement();
						frameworkElement.DataContext = this._item;
					}
					BindingOperations.SetBinding(frameworkElement, DataGrid.CellAutomationValueHolder.CellContentProperty, this._column.ClipboardContentBinding);
					result = (string)frameworkElement.GetValue(DataGrid.CellAutomationValueHolder.CellContentProperty);
					BindingOperations.ClearBinding(frameworkElement, DataGrid.CellAutomationValueHolder.CellContentProperty);
				}
				return result;
			}

			// Token: 0x06008F5F RID: 36703 RVA: 0x00344234 File Offset: 0x00343234
			public object GetClipboardValue()
			{
				object result;
				if (this._column.ClipboardContentBinding == null)
				{
					result = null;
				}
				else
				{
					FrameworkElement frameworkElement;
					if (this._cell != null)
					{
						frameworkElement = this._cell;
					}
					else
					{
						frameworkElement = new FrameworkElement();
						frameworkElement.DataContext = this._item;
					}
					BindingOperations.SetBinding(frameworkElement, DataGrid.CellAutomationValueHolder.CellClipboardProperty, this._column.ClipboardContentBinding);
					result = frameworkElement.GetValue(DataGrid.CellAutomationValueHolder.CellClipboardProperty);
					BindingOperations.ClearBinding(frameworkElement, DataGrid.CellAutomationValueHolder.CellClipboardProperty);
				}
				return result;
			}

			// Token: 0x06008F60 RID: 36704 RVA: 0x003442A4 File Offset: 0x003432A4
			public void SetValue(DataGrid dataGrid, object value, bool clipboard)
			{
				if (this._column.ClipboardContentBinding == null)
				{
					return;
				}
				this._inSetValue = true;
				DependencyProperty dp = clipboard ? DataGrid.CellAutomationValueHolder.CellClipboardProperty : DataGrid.CellAutomationValueHolder.CellContentProperty;
				BindingBase binding = this._column.ClipboardContentBinding.Clone(BindingMode.TwoWay);
				BindingOperations.SetBinding(this._cell, dp, binding);
				this._cell.SetValue(dp, value);
				dataGrid.CommitEdit();
				BindingOperations.ClearBinding(this._cell, dp);
				this._inSetValue = false;
			}

			// Token: 0x040049EC RID: 18924
			private static DependencyProperty CellContentProperty = DependencyProperty.RegisterAttached("CellContent", typeof(string), typeof(DataGrid.CellAutomationValueHolder));

			// Token: 0x040049ED RID: 18925
			private static DependencyProperty CellClipboardProperty = DependencyProperty.RegisterAttached("CellClipboard", typeof(object), typeof(DataGrid.CellAutomationValueHolder));

			// Token: 0x040049EE RID: 18926
			private DataGridCell _cell;

			// Token: 0x040049EF RID: 18927
			private DataGridColumn _column;

			// Token: 0x040049F0 RID: 18928
			private object _item;

			// Token: 0x040049F1 RID: 18929
			private string _value;

			// Token: 0x040049F2 RID: 18930
			private bool _inSetValue;
		}
	}
}
