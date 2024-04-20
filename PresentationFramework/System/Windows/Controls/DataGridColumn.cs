using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200074B RID: 1867
	public abstract class DataGridColumn : DependencyObject
	{
		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x060064FF RID: 25855 RVA: 0x002ABAF2 File Offset: 0x002AAAF2
		// (set) Token: 0x06006500 RID: 25856 RVA: 0x002ABAFF File Offset: 0x002AAAFF
		public object Header
		{
			get
			{
				return base.GetValue(DataGridColumn.HeaderProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderProperty, value);
			}
		}

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x06006501 RID: 25857 RVA: 0x002ABB0D File Offset: 0x002AAB0D
		// (set) Token: 0x06006502 RID: 25858 RVA: 0x002ABB1F File Offset: 0x002AAB1F
		public Style HeaderStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridColumn.HeaderStyleProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderStyleProperty, value);
			}
		}

		// Token: 0x06006503 RID: 25859 RVA: 0x002ABB30 File Offset: 0x002AAB30
		private static object OnCoerceHeaderStyle(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.HeaderStyleProperty, dataGridColumn.DataGridOwner, DataGrid.ColumnHeaderStyleProperty);
		}

		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x06006504 RID: 25860 RVA: 0x002ABB5B File Offset: 0x002AAB5B
		// (set) Token: 0x06006505 RID: 25861 RVA: 0x002ABB6D File Offset: 0x002AAB6D
		public string HeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(DataGridColumn.HeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderStringFormatProperty, value);
			}
		}

		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x06006506 RID: 25862 RVA: 0x002ABB7B File Offset: 0x002AAB7B
		// (set) Token: 0x06006507 RID: 25863 RVA: 0x002ABB8D File Offset: 0x002AAB8D
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridColumn.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x06006508 RID: 25864 RVA: 0x002ABB9B File Offset: 0x002AAB9B
		// (set) Token: 0x06006509 RID: 25865 RVA: 0x002ABBAD File Offset: 0x002AABAD
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridColumn.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x0600650A RID: 25866 RVA: 0x002ABBBB File Offset: 0x002AABBB
		// (set) Token: 0x0600650B RID: 25867 RVA: 0x002ABBCD File Offset: 0x002AABCD
		public Style CellStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridColumn.CellStyleProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.CellStyleProperty, value);
			}
		}

		// Token: 0x0600650C RID: 25868 RVA: 0x002ABBDC File Offset: 0x002AABDC
		private static object OnCoerceCellStyle(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.CellStyleProperty, dataGridColumn.DataGridOwner, DataGrid.CellStyleProperty);
		}

		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x0600650D RID: 25869 RVA: 0x002ABC07 File Offset: 0x002AAC07
		// (set) Token: 0x0600650E RID: 25870 RVA: 0x002ABC19 File Offset: 0x002AAC19
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.IsReadOnlyProperty, value);
			}
		}

		// Token: 0x0600650F RID: 25871 RVA: 0x002ABC27 File Offset: 0x002AAC27
		private static object OnCoerceIsReadOnly(DependencyObject d, object baseValue)
		{
			return (d as DataGridColumn).OnCoerceIsReadOnly((bool)baseValue);
		}

		// Token: 0x06006510 RID: 25872 RVA: 0x002ABC3F File Offset: 0x002AAC3F
		protected virtual bool OnCoerceIsReadOnly(bool baseValue)
		{
			return (bool)DataGridHelper.GetCoercedTransferPropertyValue(this, baseValue, DataGridColumn.IsReadOnlyProperty, this.DataGridOwner, DataGrid.IsReadOnlyProperty);
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x06006511 RID: 25873 RVA: 0x002ABC62 File Offset: 0x002AAC62
		// (set) Token: 0x06006512 RID: 25874 RVA: 0x002ABC74 File Offset: 0x002AAC74
		public DataGridLength Width
		{
			get
			{
				return (DataGridLength)base.GetValue(DataGridColumn.WidthProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.WidthProperty, value);
			}
		}

		// Token: 0x06006513 RID: 25875 RVA: 0x002ABC88 File Offset: 0x002AAC88
		internal void SetWidthInternal(DataGridLength width)
		{
			bool ignoreRedistributionOnWidthChange = this._ignoreRedistributionOnWidthChange;
			this._ignoreRedistributionOnWidthChange = true;
			try
			{
				this.Width = width;
			}
			finally
			{
				this._ignoreRedistributionOnWidthChange = ignoreRedistributionOnWidthChange;
			}
		}

		// Token: 0x06006514 RID: 25876 RVA: 0x002ABCC4 File Offset: 0x002AACC4
		private static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			DataGridLength dataGridLength = (DataGridLength)e.OldValue;
			DataGridLength dataGridLength2 = (DataGridLength)e.NewValue;
			DataGrid dataGridOwner = dataGridColumn.DataGridOwner;
			if (dataGridOwner != null && !DoubleUtil.AreClose(dataGridLength.DisplayValue, dataGridLength2.DisplayValue))
			{
				dataGridOwner.InternalColumns.InvalidateAverageColumnWidth();
			}
			if (dataGridColumn._processingWidthChange)
			{
				dataGridColumn.CoerceValue(DataGridColumn.ActualWidthProperty);
				return;
			}
			dataGridColumn._processingWidthChange = true;
			if (dataGridLength.IsStar != dataGridLength2.IsStar)
			{
				dataGridColumn.CoerceValue(DataGridColumn.MaxWidthProperty);
			}
			try
			{
				if (dataGridOwner != null && (dataGridLength2.IsStar ^ dataGridLength.IsStar))
				{
					dataGridOwner.InternalColumns.InvalidateHasVisibleStarColumns();
				}
				dataGridColumn.NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.Columns | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeaders | DataGridNotificationTarget.ColumnHeadersPresenter | DataGridNotificationTarget.DataGrid);
				if (dataGridOwner != null && !dataGridColumn._ignoreRedistributionOnWidthChange && dataGridColumn.IsVisible)
				{
					if (!dataGridLength2.IsStar && !dataGridLength2.IsAbsolute)
					{
						DataGridLength width = dataGridColumn.Width;
						double displayValue = DataGridHelper.CoerceToMinMax(width.DesiredValue, dataGridColumn.MinWidth, dataGridColumn.MaxWidth);
						dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, displayValue));
					}
					dataGridOwner.InternalColumns.RedistributeColumnWidthsOnWidthChangeOfColumn(dataGridColumn, (DataGridLength)e.OldValue);
				}
			}
			finally
			{
				dataGridColumn._processingWidthChange = false;
			}
		}

		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x06006515 RID: 25877 RVA: 0x002ABE18 File Offset: 0x002AAE18
		// (set) Token: 0x06006516 RID: 25878 RVA: 0x002ABE2A File Offset: 0x002AAE2A
		public double MinWidth
		{
			get
			{
				return (double)base.GetValue(DataGridColumn.MinWidthProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.MinWidthProperty, value);
			}
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x002ABE40 File Offset: 0x002AAE40
		private static void OnMinWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			DataGrid dataGridOwner = dataGridColumn.DataGridOwner;
			dataGridColumn.NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
			if (dataGridOwner != null && dataGridColumn.IsVisible)
			{
				dataGridOwner.InternalColumns.RedistributeColumnWidthsOnMinWidthChangeOfColumn(dataGridColumn, (double)e.OldValue);
			}
		}

		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x06006518 RID: 25880 RVA: 0x002ABE87 File Offset: 0x002AAE87
		// (set) Token: 0x06006519 RID: 25881 RVA: 0x002ABE99 File Offset: 0x002AAE99
		public double MaxWidth
		{
			get
			{
				return (double)base.GetValue(DataGridColumn.MaxWidthProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.MaxWidthProperty, value);
			}
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x002ABEAC File Offset: 0x002AAEAC
		private static void OnMaxWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			DataGrid dataGridOwner = dataGridColumn.DataGridOwner;
			dataGridColumn.NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
			if (dataGridOwner != null && dataGridColumn.IsVisible)
			{
				dataGridOwner.InternalColumns.RedistributeColumnWidthsOnMaxWidthChangeOfColumn(dataGridColumn, (double)e.OldValue);
			}
		}

		// Token: 0x0600651B RID: 25883 RVA: 0x002ABEF3 File Offset: 0x002AAEF3
		private static double CoerceDesiredOrDisplayWidthValue(double widthValue, double memberValue, DataGridLengthUnitType type)
		{
			if (DoubleUtil.IsNaN(memberValue))
			{
				if (type == DataGridLengthUnitType.Pixel)
				{
					memberValue = widthValue;
				}
				else if (type == DataGridLengthUnitType.Auto || type == DataGridLengthUnitType.SizeToCells || type == DataGridLengthUnitType.SizeToHeader)
				{
					memberValue = 0.0;
				}
			}
			return memberValue;
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x002ABF20 File Offset: 0x002AAF20
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			DataGridLength dataGridLength = (DataGridLength)DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.WidthProperty, dataGridColumn.DataGridOwner, DataGrid.ColumnWidthProperty);
			double desiredValue = DataGridColumn.CoerceDesiredOrDisplayWidthValue(dataGridLength.Value, dataGridLength.DesiredValue, dataGridLength.UnitType);
			double num = DataGridColumn.CoerceDesiredOrDisplayWidthValue(dataGridLength.Value, dataGridLength.DisplayValue, dataGridLength.UnitType);
			num = (DoubleUtil.IsNaN(num) ? num : DataGridHelper.CoerceToMinMax(num, dataGridColumn.MinWidth, dataGridColumn.MaxWidth));
			if (DoubleUtil.IsNaN(num) || DoubleUtil.AreClose(num, dataGridLength.DisplayValue))
			{
				return dataGridLength;
			}
			return new DataGridLength(dataGridLength.Value, dataGridLength.UnitType, desiredValue, num);
		}

		// Token: 0x0600651D RID: 25885 RVA: 0x002ABFE0 File Offset: 0x002AAFE0
		private static object OnCoerceMinWidth(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.MinWidthProperty, dataGridColumn.DataGridOwner, DataGrid.MinColumnWidthProperty);
		}

		// Token: 0x0600651E RID: 25886 RVA: 0x002AC00C File Offset: 0x002AB00C
		private static object OnCoerceMaxWidth(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			double num = (double)DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.MaxWidthProperty, dataGridColumn.DataGridOwner, DataGrid.MaxColumnWidthProperty);
			if (double.IsPositiveInfinity(num) && dataGridColumn.Width.IsStar)
			{
				return 10000.0;
			}
			return num;
		}

		// Token: 0x0600651F RID: 25887 RVA: 0x002A01A0 File Offset: 0x0029F1A0
		private static bool ValidateMinWidth(object v)
		{
			double num = (double)v;
			return num >= 0.0 && !DoubleUtil.IsNaN(num) && !double.IsPositiveInfinity(num);
		}

		// Token: 0x06006520 RID: 25888 RVA: 0x002A01D4 File Offset: 0x0029F1D4
		private static bool ValidateMaxWidth(object v)
		{
			double num = (double)v;
			return num >= 0.0 && !DoubleUtil.IsNaN(num);
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x06006521 RID: 25889 RVA: 0x002AC06A File Offset: 0x002AB06A
		// (set) Token: 0x06006522 RID: 25890 RVA: 0x002AC07C File Offset: 0x002AB07C
		public double ActualWidth
		{
			get
			{
				return (double)base.GetValue(DataGridColumn.ActualWidthProperty);
			}
			private set
			{
				base.SetValue(DataGridColumn.ActualWidthPropertyKey, value);
			}
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x002AC090 File Offset: 0x002AB090
		private static object OnCoerceActualWidth(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			double num = (double)baseValue;
			double minWidth = dataGridColumn.MinWidth;
			double maxWidth = dataGridColumn.MaxWidth;
			DataGridLength width = dataGridColumn.Width;
			if (width.IsAbsolute)
			{
				num = width.DisplayValue;
			}
			if (num < minWidth)
			{
				num = minWidth;
			}
			else if (num > maxWidth)
			{
				num = maxWidth;
			}
			return num;
		}

		// Token: 0x06006524 RID: 25892 RVA: 0x002AC0E4 File Offset: 0x002AB0E4
		internal double GetConstraintWidth(bool isHeader)
		{
			DataGridLength width = this.Width;
			if (!DoubleUtil.IsNaN(width.DisplayValue))
			{
				return width.DisplayValue;
			}
			if (width.IsAbsolute || width.IsStar || (width.IsSizeToCells && isHeader) || (width.IsSizeToHeader && !isHeader))
			{
				return this.ActualWidth;
			}
			return double.PositiveInfinity;
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x002AC148 File Offset: 0x002AB148
		internal void UpdateDesiredWidthForAutoColumn(bool isHeader, double pixelWidth)
		{
			DataGridLength width = this.Width;
			double minWidth = this.MinWidth;
			double maxWidth = this.MaxWidth;
			double num = DataGridHelper.CoerceToMinMax(pixelWidth, minWidth, maxWidth);
			if (width.IsAuto || (width.IsSizeToCells && !isHeader) || (width.IsSizeToHeader && isHeader))
			{
				if (DoubleUtil.IsNaN(width.DesiredValue) || DoubleUtil.LessThan(width.DesiredValue, pixelWidth))
				{
					if (DoubleUtil.IsNaN(width.DisplayValue))
					{
						this.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, pixelWidth, num));
					}
					else
					{
						double value = DataGridHelper.CoerceToMinMax(width.DesiredValue, minWidth, maxWidth);
						this.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, pixelWidth, width.DisplayValue));
						if (DoubleUtil.AreClose(value, width.DisplayValue))
						{
							this.DataGridOwner.InternalColumns.RecomputeColumnWidthsOnColumnResize(this, pixelWidth - width.DisplayValue, true);
						}
					}
					width = this.Width;
				}
				if (DoubleUtil.IsNaN(width.DisplayValue))
				{
					if (this.ActualWidth < num)
					{
						this.ActualWidth = num;
						return;
					}
				}
				else if (!DoubleUtil.AreClose(this.ActualWidth, width.DisplayValue))
				{
					this.ActualWidth = width.DisplayValue;
				}
			}
		}

		// Token: 0x06006526 RID: 25894 RVA: 0x002AC280 File Offset: 0x002AB280
		internal void UpdateWidthForStarColumn(double displayWidth, double desiredWidth, double starValue)
		{
			DataGridLength width = this.Width;
			if (!DoubleUtil.AreClose(displayWidth, width.DisplayValue) || !DoubleUtil.AreClose(desiredWidth, width.DesiredValue) || !DoubleUtil.AreClose(width.Value, starValue))
			{
				this.SetWidthInternal(new DataGridLength(starValue, width.UnitType, desiredWidth, displayWidth));
				this.ActualWidth = displayWidth;
			}
		}

		// Token: 0x06006527 RID: 25895 RVA: 0x002AC2E0 File Offset: 0x002AB2E0
		public FrameworkElement GetCellContent(object dataItem)
		{
			if (dataItem == null)
			{
				throw new ArgumentNullException("dataItem");
			}
			if (this._dataGridOwner != null)
			{
				DataGridRow dataGridRow = this._dataGridOwner.ItemContainerGenerator.ContainerFromItem(dataItem) as DataGridRow;
				if (dataGridRow != null)
				{
					return this.GetCellContent(dataGridRow);
				}
			}
			return null;
		}

		// Token: 0x06006528 RID: 25896 RVA: 0x002AC328 File Offset: 0x002AB328
		public FrameworkElement GetCellContent(DataGridRow dataGridRow)
		{
			if (dataGridRow == null)
			{
				throw new ArgumentNullException("dataGridRow");
			}
			if (this._dataGridOwner != null)
			{
				int num = this._dataGridOwner.Columns.IndexOf(this);
				if (num >= 0)
				{
					DataGridCell dataGridCell = dataGridRow.TryGetCell(num);
					if (dataGridCell != null)
					{
						return dataGridCell.Content as FrameworkElement;
					}
				}
			}
			return null;
		}

		// Token: 0x06006529 RID: 25897 RVA: 0x002AC379 File Offset: 0x002AB379
		internal FrameworkElement BuildVisualTree(bool isEditing, object dataItem, DataGridCell cell)
		{
			if (isEditing)
			{
				return this.GenerateEditingElement(cell, dataItem);
			}
			return this.GenerateElement(cell, dataItem);
		}

		// Token: 0x0600652A RID: 25898
		protected abstract FrameworkElement GenerateElement(DataGridCell cell, object dataItem);

		// Token: 0x0600652B RID: 25899
		protected abstract FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem);

		// Token: 0x0600652C RID: 25900 RVA: 0x00109403 File Offset: 0x00108403
		protected virtual object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			return null;
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x002AC38F File Offset: 0x002AB38F
		protected virtual void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
		{
			DataGridHelper.UpdateTarget(editingElement);
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x002AC397 File Offset: 0x002AB397
		protected virtual bool CommitCellEdit(FrameworkElement editingElement)
		{
			return DataGridHelper.ValidateWithoutUpdate(editingElement);
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x002AC3A0 File Offset: 0x002AB3A0
		internal void BeginEdit(FrameworkElement editingElement, RoutedEventArgs e)
		{
			if (editingElement != null)
			{
				editingElement.UpdateLayout();
				object value = this.PrepareCellForEdit(editingElement, e);
				DataGridColumn.SetOriginalValue(editingElement, value);
			}
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x002AC3C6 File Offset: 0x002AB3C6
		internal void CancelEdit(FrameworkElement editingElement)
		{
			if (editingElement != null)
			{
				this.CancelCellEdit(editingElement, DataGridColumn.GetOriginalValue(editingElement));
				DataGridColumn.ClearOriginalValue(editingElement);
			}
		}

		// Token: 0x06006531 RID: 25905 RVA: 0x002AC3DE File Offset: 0x002AB3DE
		internal bool CommitEdit(FrameworkElement editingElement)
		{
			if (editingElement == null)
			{
				return true;
			}
			if (this.CommitCellEdit(editingElement))
			{
				DataGridColumn.ClearOriginalValue(editingElement);
				return true;
			}
			return false;
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x002AC3F7 File Offset: 0x002AB3F7
		private static object GetOriginalValue(DependencyObject obj)
		{
			return obj.GetValue(DataGridColumn.OriginalValueProperty);
		}

		// Token: 0x06006533 RID: 25907 RVA: 0x002AC404 File Offset: 0x002AB404
		private static void SetOriginalValue(DependencyObject obj, object value)
		{
			obj.SetValue(DataGridColumn.OriginalValueProperty, value);
		}

		// Token: 0x06006534 RID: 25908 RVA: 0x002AC412 File Offset: 0x002AB412
		private static void ClearOriginalValue(DependencyObject obj)
		{
			obj.ClearValue(DataGridColumn.OriginalValueProperty);
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x002AC41F File Offset: 0x002AB41F
		internal static void OnNotifyCellPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.Columns);
		}

		// Token: 0x06006536 RID: 25910 RVA: 0x002AC42F File Offset: 0x002AB42F
		private static void OnNotifyColumnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns | DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x06006537 RID: 25911 RVA: 0x002AC440 File Offset: 0x002AB440
		private static void OnNotifyColumnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x002AC450 File Offset: 0x002AB450
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyColumns(target))
			{
				target &= ~DataGridNotificationTarget.Columns;
				if (e.Property == DataGrid.MaxColumnWidthProperty || e.Property == DataGridColumn.MaxWidthProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.MaxWidthProperty);
				}
				else if (e.Property == DataGrid.MinColumnWidthProperty || e.Property == DataGridColumn.MinWidthProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.MinWidthProperty);
				}
				else if (e.Property == DataGrid.ColumnWidthProperty || e.Property == DataGridColumn.WidthProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.WidthProperty);
				}
				else if (e.Property == DataGrid.ColumnHeaderStyleProperty || e.Property == DataGridColumn.HeaderStyleProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.HeaderStyleProperty);
				}
				else if (e.Property == DataGrid.CellStyleProperty || e.Property == DataGridColumn.CellStyleProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.CellStyleProperty);
				}
				else if (e.Property == DataGrid.IsReadOnlyProperty || e.Property == DataGridColumn.IsReadOnlyProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.IsReadOnlyProperty);
				}
				else if (e.Property == DataGrid.DragIndicatorStyleProperty || e.Property == DataGridColumn.DragIndicatorStyleProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.DragIndicatorStyleProperty);
				}
				else if (e.Property == DataGridColumn.DisplayIndexProperty)
				{
					base.CoerceValue(DataGridColumn.IsFrozenProperty);
				}
				else if (e.Property == DataGrid.CanUserSortColumnsProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.CanUserSortProperty);
				}
				else if (e.Property == DataGrid.CanUserResizeColumnsProperty || e.Property == DataGridColumn.CanUserResizeProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.CanUserResizeProperty);
				}
				else if (e.Property == DataGrid.CanUserReorderColumnsProperty || e.Property == DataGridColumn.CanUserReorderProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.CanUserReorderProperty);
				}
				if (e.Property == DataGridColumn.WidthProperty || e.Property == DataGridColumn.MinWidthProperty || e.Property == DataGridColumn.MaxWidthProperty)
				{
					base.CoerceValue(DataGridColumn.ActualWidthProperty);
				}
			}
			if (target != DataGridNotificationTarget.None)
			{
				DataGrid dataGridOwner = ((DataGridColumn)d).DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridOwner.NotifyPropertyChanged(d, e, target);
				}
			}
		}

		// Token: 0x06006539 RID: 25913 RVA: 0x002AC678 File Offset: 0x002AB678
		protected void NotifyPropertyChanged(string propertyName)
		{
			if (this.DataGridOwner != null)
			{
				this.DataGridOwner.NotifyPropertyChanged(this, propertyName, default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.RefreshCellContent);
			}
		}

		// Token: 0x0600653A RID: 25914 RVA: 0x002AC6A8 File Offset: 0x002AB6A8
		internal static void NotifyPropertyChangeForRefreshContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(e.Property.Name);
		}

		// Token: 0x0600653B RID: 25915 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected internal virtual void RefreshCellContent(FrameworkElement element, string propertyName)
		{
		}

		// Token: 0x0600653C RID: 25916 RVA: 0x002AC6C4 File Offset: 0x002AB6C4
		internal void SyncProperties()
		{
			DataGridHelper.TransferProperty(this, DataGridColumn.MinWidthProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.MaxWidthProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.WidthProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.HeaderStyleProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.CellStyleProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.IsReadOnlyProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.DragIndicatorStyleProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.CanUserSortProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.CanUserReorderProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.CanUserResizeProperty);
		}

		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x0600653D RID: 25917 RVA: 0x002AC73F File Offset: 0x002AB73F
		// (set) Token: 0x0600653E RID: 25918 RVA: 0x002AC747 File Offset: 0x002AB747
		protected internal DataGrid DataGridOwner
		{
			get
			{
				return this._dataGridOwner;
			}
			internal set
			{
				this._dataGridOwner = value;
			}
		}

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x0600653F RID: 25919 RVA: 0x002AC750 File Offset: 0x002AB750
		// (set) Token: 0x06006540 RID: 25920 RVA: 0x002AC762 File Offset: 0x002AB762
		public int DisplayIndex
		{
			get
			{
				return (int)base.GetValue(DataGridColumn.DisplayIndexProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.DisplayIndexProperty, value);
			}
		}

		// Token: 0x06006541 RID: 25921 RVA: 0x002AC778 File Offset: 0x002AB778
		private static object OnCoerceDisplayIndex(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			if (dataGridColumn.DataGridOwner != null)
			{
				dataGridColumn.DataGridOwner.ValidateDisplayIndex(dataGridColumn, (int)baseValue);
			}
			return baseValue;
		}

		// Token: 0x06006542 RID: 25922 RVA: 0x002AC7A7 File Offset: 0x002AB7A7
		private static void DisplayIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.Columns | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeaders | DataGridNotificationTarget.ColumnHeadersPresenter | DataGridNotificationTarget.DataGrid);
		}

		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x06006543 RID: 25923 RVA: 0x002AC7B8 File Offset: 0x002AB7B8
		// (set) Token: 0x06006544 RID: 25924 RVA: 0x002AC7CA File Offset: 0x002AB7CA
		public string SortMemberPath
		{
			get
			{
				return (string)base.GetValue(DataGridColumn.SortMemberPathProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.SortMemberPathProperty, value);
			}
		}

		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x06006545 RID: 25925 RVA: 0x002AC7D8 File Offset: 0x002AB7D8
		// (set) Token: 0x06006546 RID: 25926 RVA: 0x002AC7EA File Offset: 0x002AB7EA
		public bool CanUserSort
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.CanUserSortProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.CanUserSortProperty, value);
			}
		}

		// Token: 0x06006547 RID: 25927 RVA: 0x002AC7F8 File Offset: 0x002AB7F8
		internal static object OnCoerceCanUserSort(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			bool flag;
			BaseValueSourceInternal valueSource = dataGridColumn.GetValueSource(DataGridColumn.CanUserSortProperty, null, out flag);
			bool flag2;
			if (dataGridColumn.DataGridOwner != null && (dataGridColumn.DataGridOwner.GetValueSource(DataGrid.CanUserSortColumnsProperty, null, out flag2) == valueSource && !flag && flag2))
			{
				return dataGridColumn.DataGridOwner.GetValue(DataGrid.CanUserSortColumnsProperty);
			}
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.CanUserSortProperty, dataGridColumn.DataGridOwner, DataGrid.CanUserSortColumnsProperty);
		}

		// Token: 0x06006548 RID: 25928 RVA: 0x002AC86C File Offset: 0x002AB86C
		private static void OnCanUserSortPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!DataGridHelper.IsPropertyTransferEnabled(d, DataGridColumn.CanUserSortProperty))
			{
				DataGridHelper.TransferProperty(d, DataGridColumn.CanUserSortProperty);
			}
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x06006549 RID: 25929 RVA: 0x002AC895 File Offset: 0x002AB895
		// (set) Token: 0x0600654A RID: 25930 RVA: 0x002AC8A7 File Offset: 0x002AB8A7
		public ListSortDirection? SortDirection
		{
			get
			{
				return (ListSortDirection?)base.GetValue(DataGridColumn.SortDirectionProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.SortDirectionProperty, value);
			}
		}

		// Token: 0x0600654B RID: 25931 RVA: 0x002AC8BA File Offset: 0x002AB8BA
		private static void OnNotifySortPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x0600654C RID: 25932 RVA: 0x002AC8CB File Offset: 0x002AB8CB
		// (set) Token: 0x0600654D RID: 25933 RVA: 0x002AC8DD File Offset: 0x002AB8DD
		public bool IsAutoGenerated
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.IsAutoGeneratedProperty);
			}
			internal set
			{
				base.SetValue(DataGridColumn.IsAutoGeneratedPropertyKey, value);
			}
		}

		// Token: 0x0600654E RID: 25934 RVA: 0x002AC8EC File Offset: 0x002AB8EC
		internal static DataGridColumn CreateDefaultColumn(ItemPropertyInfo itemProperty)
		{
			DataGridComboBoxColumn dataGridComboBoxColumn = null;
			Type propertyType = itemProperty.PropertyType;
			DataGridColumn dataGridColumn;
			if (propertyType.IsEnum)
			{
				dataGridComboBoxColumn = new DataGridComboBoxColumn();
				dataGridComboBoxColumn.ItemsSource = Enum.GetValues(propertyType);
				dataGridColumn = dataGridComboBoxColumn;
			}
			else if (typeof(string).IsAssignableFrom(propertyType))
			{
				dataGridColumn = new DataGridTextColumn();
			}
			else if (typeof(bool).IsAssignableFrom(propertyType))
			{
				dataGridColumn = new DataGridCheckBoxColumn();
			}
			else if (typeof(Uri).IsAssignableFrom(propertyType))
			{
				dataGridColumn = new DataGridHyperlinkColumn();
			}
			else
			{
				dataGridColumn = new DataGridTextColumn();
			}
			if (!typeof(IComparable).IsAssignableFrom(propertyType))
			{
				dataGridColumn.CanUserSort = false;
			}
			dataGridColumn.Header = itemProperty.Name;
			DataGridBoundColumn dataGridBoundColumn = dataGridColumn as DataGridBoundColumn;
			if (dataGridBoundColumn != null || dataGridComboBoxColumn != null)
			{
				Binding binding = new Binding(itemProperty.Name);
				if (dataGridComboBoxColumn != null)
				{
					dataGridComboBoxColumn.SelectedItemBinding = binding;
				}
				else
				{
					dataGridBoundColumn.Binding = binding;
				}
				PropertyDescriptor propertyDescriptor = itemProperty.Descriptor as PropertyDescriptor;
				if (propertyDescriptor != null)
				{
					if (propertyDescriptor.IsReadOnly)
					{
						binding.Mode = BindingMode.OneWay;
						dataGridColumn.IsReadOnly = true;
					}
				}
				else
				{
					PropertyInfo propertyInfo = itemProperty.Descriptor as PropertyInfo;
					if (propertyInfo != null && !propertyInfo.CanWrite)
					{
						binding.Mode = BindingMode.OneWay;
						dataGridColumn.IsReadOnly = true;
					}
				}
			}
			return dataGridColumn;
		}

		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x0600654F RID: 25935 RVA: 0x002ACA25 File Offset: 0x002ABA25
		// (set) Token: 0x06006550 RID: 25936 RVA: 0x002ACA37 File Offset: 0x002ABA37
		public bool IsFrozen
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.IsFrozenProperty);
			}
			internal set
			{
				base.SetValue(DataGridColumn.IsFrozenPropertyKey, value);
			}
		}

		// Token: 0x06006551 RID: 25937 RVA: 0x002AC8BA File Offset: 0x002AB8BA
		private static void OnNotifyFrozenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x06006552 RID: 25938 RVA: 0x002ACA48 File Offset: 0x002ABA48
		private static object OnCoerceIsFrozen(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			DataGrid dataGridOwner = dataGridColumn.DataGridOwner;
			if (dataGridOwner == null)
			{
				return baseValue;
			}
			if (dataGridColumn.DisplayIndex < dataGridOwner.FrozenColumnCount)
			{
				return true;
			}
			return false;
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x06006553 RID: 25939 RVA: 0x002ACA83 File Offset: 0x002ABA83
		// (set) Token: 0x06006554 RID: 25940 RVA: 0x002ACA95 File Offset: 0x002ABA95
		public bool CanUserReorder
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.CanUserReorderProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.CanUserReorderProperty, value);
			}
		}

		// Token: 0x06006555 RID: 25941 RVA: 0x002ACAA4 File Offset: 0x002ABAA4
		private static object OnCoerceCanUserReorder(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.CanUserReorderProperty, dataGridColumn.DataGridOwner, DataGrid.CanUserReorderColumnsProperty);
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x06006556 RID: 25942 RVA: 0x002ACACF File Offset: 0x002ABACF
		// (set) Token: 0x06006557 RID: 25943 RVA: 0x002ACAE1 File Offset: 0x002ABAE1
		public Style DragIndicatorStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridColumn.DragIndicatorStyleProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.DragIndicatorStyleProperty, value);
			}
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x002ACAF0 File Offset: 0x002ABAF0
		private static object OnCoerceDragIndicatorStyle(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.DragIndicatorStyleProperty, dataGridColumn.DataGridOwner, DataGrid.DragIndicatorStyleProperty);
		}

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x06006559 RID: 25945 RVA: 0x002ACB1B File Offset: 0x002ABB1B
		// (set) Token: 0x0600655A RID: 25946 RVA: 0x002ACB23 File Offset: 0x002ABB23
		public virtual BindingBase ClipboardContentBinding
		{
			get
			{
				return this._clipboardContentBinding;
			}
			set
			{
				this._clipboardContentBinding = value;
			}
		}

		// Token: 0x0600655B RID: 25947 RVA: 0x002ACB2C File Offset: 0x002ABB2C
		public virtual object OnCopyingCellClipboardContent(object item)
		{
			object obj = this.DataGridOwner.GetCellClipboardValue(item, this);
			if (this.CopyingCellClipboardContent != null)
			{
				DataGridCellClipboardEventArgs dataGridCellClipboardEventArgs = new DataGridCellClipboardEventArgs(item, this, obj);
				this.CopyingCellClipboardContent(this, dataGridCellClipboardEventArgs);
				obj = dataGridCellClipboardEventArgs.Content;
			}
			return obj;
		}

		// Token: 0x0600655C RID: 25948 RVA: 0x002ACB70 File Offset: 0x002ABB70
		public virtual void OnPastingCellClipboardContent(object item, object cellContent)
		{
			if (this.ClipboardContentBinding != null)
			{
				if (this.PastingCellClipboardContent != null)
				{
					DataGridCellClipboardEventArgs dataGridCellClipboardEventArgs = new DataGridCellClipboardEventArgs(item, this, cellContent);
					this.PastingCellClipboardContent(this, dataGridCellClipboardEventArgs);
					cellContent = dataGridCellClipboardEventArgs.Content;
				}
				if (cellContent != null)
				{
					this.DataGridOwner.SetCellClipboardValue(item, this, cellContent);
				}
			}
		}

		// Token: 0x14000105 RID: 261
		// (add) Token: 0x0600655D RID: 25949 RVA: 0x002ACBBC File Offset: 0x002ABBBC
		// (remove) Token: 0x0600655E RID: 25950 RVA: 0x002ACBF4 File Offset: 0x002ABBF4
		public event EventHandler<DataGridCellClipboardEventArgs> CopyingCellClipboardContent;

		// Token: 0x14000106 RID: 262
		// (add) Token: 0x0600655F RID: 25951 RVA: 0x002ACC2C File Offset: 0x002ABC2C
		// (remove) Token: 0x06006560 RID: 25952 RVA: 0x002ACC64 File Offset: 0x002ABC64
		public event EventHandler<DataGridCellClipboardEventArgs> PastingCellClipboardContent;

		// Token: 0x06006561 RID: 25953 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnInput(InputEventArgs e)
		{
		}

		// Token: 0x06006562 RID: 25954 RVA: 0x002ACC9C File Offset: 0x002ABC9C
		internal void BeginEdit(InputEventArgs e, bool handled)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner != null && dataGridOwner.BeginEdit(e))
			{
				e.Handled = (e.Handled || handled);
			}
		}

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x06006563 RID: 25955 RVA: 0x002ACCCA File Offset: 0x002ABCCA
		// (set) Token: 0x06006564 RID: 25956 RVA: 0x002ACCDC File Offset: 0x002ABCDC
		public bool CanUserResize
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.CanUserResizeProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.CanUserResizeProperty, value);
			}
		}

		// Token: 0x06006565 RID: 25957 RVA: 0x002ACCEC File Offset: 0x002ABCEC
		private static object OnCoerceCanUserResize(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.CanUserResizeProperty, dataGridColumn.DataGridOwner, DataGrid.CanUserResizeColumnsProperty);
		}

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x06006566 RID: 25958 RVA: 0x002ACD17 File Offset: 0x002ABD17
		// (set) Token: 0x06006567 RID: 25959 RVA: 0x002ACD29 File Offset: 0x002ABD29
		public Visibility Visibility
		{
			get
			{
				return (Visibility)base.GetValue(DataGridColumn.VisibilityProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.VisibilityProperty, value);
			}
		}

		// Token: 0x06006568 RID: 25960 RVA: 0x002ACD3C File Offset: 0x002ABD3C
		private static void OnVisibilityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
		{
			bool flag = (Visibility)eventArgs.OldValue != Visibility.Visible;
			Visibility visibility = (Visibility)eventArgs.NewValue;
			if (flag && visibility != Visibility.Visible)
			{
				return;
			}
			((DataGridColumn)d).NotifyPropertyChanged(d, eventArgs, DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeaders | DataGridNotificationTarget.ColumnHeadersPresenter | DataGridNotificationTarget.DataGrid);
		}

		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x06006569 RID: 25961 RVA: 0x002ACD77 File Offset: 0x002ABD77
		internal bool IsVisible
		{
			get
			{
				return this.Visibility == Visibility.Visible;
			}
		}

		// Token: 0x04003367 RID: 13159
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged)));

		// Token: 0x04003368 RID: 13160
		public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle", typeof(Style), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceHeaderStyle)));

		// Token: 0x04003369 RID: 13161
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register("HeaderStringFormat", typeof(string), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged)));

		// Token: 0x0400336A RID: 13162
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged)));

		// Token: 0x0400336B RID: 13163
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register("HeaderTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged)));

		// Token: 0x0400336C RID: 13164
		public static readonly DependencyProperty CellStyleProperty = DependencyProperty.Register("CellStyle", typeof(Style), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyCellPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceCellStyle)));

		// Token: 0x0400336D RID: 13165
		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridColumn.OnNotifyCellPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceIsReadOnly)));

		// Token: 0x0400336E RID: 13166
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(DataGridLength), typeof(DataGridColumn), new FrameworkPropertyMetadata(DataGridLength.Auto, new PropertyChangedCallback(DataGridColumn.OnWidthPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceWidth)));

		// Token: 0x0400336F RID: 13167
		public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), typeof(DataGridColumn), new FrameworkPropertyMetadata(20.0, new PropertyChangedCallback(DataGridColumn.OnMinWidthPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceMinWidth)), new ValidateValueCallback(DataGridColumn.ValidateMinWidth));

		// Token: 0x04003370 RID: 13168
		public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), typeof(DataGridColumn), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DataGridColumn.OnMaxWidthPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceMaxWidth)), new ValidateValueCallback(DataGridColumn.ValidateMaxWidth));

		// Token: 0x04003371 RID: 13169
		private static readonly DependencyPropertyKey ActualWidthPropertyKey = DependencyProperty.RegisterReadOnly("ActualWidth", typeof(double), typeof(DataGridColumn), new FrameworkPropertyMetadata(0.0, null, new CoerceValueCallback(DataGridColumn.OnCoerceActualWidth)));

		// Token: 0x04003372 RID: 13170
		public static readonly DependencyProperty ActualWidthProperty = DataGridColumn.ActualWidthPropertyKey.DependencyProperty;

		// Token: 0x04003373 RID: 13171
		private static readonly DependencyProperty OriginalValueProperty = DependencyProperty.RegisterAttached("OriginalValue", typeof(object), typeof(DataGridColumn), new FrameworkPropertyMetadata(null));

		// Token: 0x04003374 RID: 13172
		public static readonly DependencyProperty DisplayIndexProperty = DependencyProperty.Register("DisplayIndex", typeof(int), typeof(DataGridColumn), new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(DataGridColumn.DisplayIndexChanged), new CoerceValueCallback(DataGridColumn.OnCoerceDisplayIndex)));

		// Token: 0x04003375 RID: 13173
		public static readonly DependencyProperty SortMemberPathProperty = DependencyProperty.Register("SortMemberPath", typeof(string), typeof(DataGridColumn), new FrameworkPropertyMetadata(string.Empty));

		// Token: 0x04003376 RID: 13174
		public static readonly DependencyProperty CanUserSortProperty = DependencyProperty.Register("CanUserSort", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGridColumn.OnCanUserSortPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceCanUserSort)));

		// Token: 0x04003377 RID: 13175
		public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register("SortDirection", typeof(ListSortDirection?), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifySortPropertyChanged)));

		// Token: 0x04003378 RID: 13176
		private static readonly DependencyPropertyKey IsAutoGeneratedPropertyKey = DependencyProperty.RegisterReadOnly("IsAutoGenerated", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(false));

		// Token: 0x04003379 RID: 13177
		public static readonly DependencyProperty IsAutoGeneratedProperty = DataGridColumn.IsAutoGeneratedPropertyKey.DependencyProperty;

		// Token: 0x0400337A RID: 13178
		private static readonly DependencyPropertyKey IsFrozenPropertyKey = DependencyProperty.RegisterReadOnly("IsFrozen", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridColumn.OnNotifyFrozenPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceIsFrozen)));

		// Token: 0x0400337B RID: 13179
		public static readonly DependencyProperty IsFrozenProperty = DataGridColumn.IsFrozenPropertyKey.DependencyProperty;

		// Token: 0x0400337C RID: 13180
		public static readonly DependencyProperty CanUserReorderProperty = DependencyProperty.Register("CanUserReorder", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceCanUserReorder)));

		// Token: 0x0400337D RID: 13181
		public static readonly DependencyProperty DragIndicatorStyleProperty = DependencyProperty.Register("DragIndicatorStyle", typeof(Style), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceDragIndicatorStyle)));

		// Token: 0x04003380 RID: 13184
		public static readonly DependencyProperty CanUserResizeProperty = DependencyProperty.Register("CanUserResize", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceCanUserResize)));

		// Token: 0x04003381 RID: 13185
		public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(DataGridColumn), new FrameworkPropertyMetadata(Visibility.Visible, new PropertyChangedCallback(DataGridColumn.OnVisibilityPropertyChanged)));

		// Token: 0x04003382 RID: 13186
		private DataGrid _dataGridOwner;

		// Token: 0x04003383 RID: 13187
		private BindingBase _clipboardContentBinding;

		// Token: 0x04003384 RID: 13188
		private bool _ignoreRedistributionOnWidthChange;

		// Token: 0x04003385 RID: 13189
		private bool _processingWidthChange;

		// Token: 0x04003386 RID: 13190
		private const double _starMaxWidth = 10000.0;
	}
}
