using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200074C RID: 1868
	internal class DataGridColumnCollection : ObservableCollection<DataGridColumn>
	{
		// Token: 0x0600656C RID: 25964 RVA: 0x002AD330 File Offset: 0x002AC330
		internal DataGridColumnCollection(DataGrid dataGridOwner)
		{
			this.DisplayIndexMap = new List<int>(5);
			this._dataGridOwner = dataGridOwner;
			this.RealizedColumnsBlockListForNonVirtualizedRows = null;
			this.RealizedColumnsDisplayIndexBlockListForNonVirtualizedRows = null;
			this.RebuildRealizedColumnsBlockListForNonVirtualizedRows = true;
			this.RealizedColumnsBlockListForVirtualizedRows = null;
			this.RealizedColumnsDisplayIndexBlockListForVirtualizedRows = null;
			this.RebuildRealizedColumnsBlockListForVirtualizedRows = true;
		}

		// Token: 0x0600656D RID: 25965 RVA: 0x002AD380 File Offset: 0x002AC380
		protected override void InsertItem(int index, DataGridColumn item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item", SR.Get("DataGrid_NullColumn"));
			}
			if (item.DataGridOwner != null)
			{
				throw new ArgumentException(SR.Get("DataGrid_InvalidColumnReuse", new object[]
				{
					item.Header
				}), "item");
			}
			if (this.DisplayIndexMapInitialized)
			{
				this.ValidateDisplayIndex(item, item.DisplayIndex, true);
			}
			base.InsertItem(index, item);
			item.CoerceValue(DataGridColumn.IsFrozenProperty);
		}

		// Token: 0x0600656E RID: 25966 RVA: 0x002AD3FC File Offset: 0x002AC3FC
		protected override void SetItem(int index, DataGridColumn item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item", SR.Get("DataGrid_NullColumn"));
			}
			if (index >= base.Count || index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.Get("DataGrid_ColumnIndexOutOfRange", new object[]
				{
					item.Header
				}));
			}
			if (item.DataGridOwner != null && base[index] != item)
			{
				throw new ArgumentException(SR.Get("DataGrid_InvalidColumnReuse", new object[]
				{
					item.Header
				}), "item");
			}
			if (this.DisplayIndexMapInitialized)
			{
				this.ValidateDisplayIndex(item, item.DisplayIndex);
			}
			base.SetItem(index, item);
			item.CoerceValue(DataGridColumn.IsFrozenProperty);
		}

		// Token: 0x0600656F RID: 25967 RVA: 0x002AD4B0 File Offset: 0x002AC4B0
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (this.DisplayIndexMapInitialized)
				{
					this.UpdateDisplayIndexForNewColumns(e.NewItems, e.NewStartingIndex);
				}
				this.InvalidateHasVisibleStarColumns();
				break;
			case NotifyCollectionChangedAction.Remove:
				if (this.DisplayIndexMapInitialized)
				{
					this.UpdateDisplayIndexForRemovedColumns(e.OldItems, e.OldStartingIndex);
				}
				this.ClearDisplayIndex(e.OldItems, e.NewItems);
				this.InvalidateHasVisibleStarColumns();
				break;
			case NotifyCollectionChangedAction.Replace:
				if (this.DisplayIndexMapInitialized)
				{
					this.UpdateDisplayIndexForReplacedColumn(e.OldItems, e.NewItems);
				}
				this.ClearDisplayIndex(e.OldItems, e.NewItems);
				this.InvalidateHasVisibleStarColumns();
				break;
			case NotifyCollectionChangedAction.Move:
				if (this.DisplayIndexMapInitialized)
				{
					this.UpdateDisplayIndexForMovedColumn(e.OldStartingIndex, e.NewStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				if (this.DisplayIndexMapInitialized)
				{
					this.DisplayIndexMap.Clear();
					this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Reset, -1, null, -1);
				}
				this.HasVisibleStarColumns = false;
				break;
			}
			this.InvalidateAverageColumnWidth();
			base.OnCollectionChanged(e);
		}

		// Token: 0x06006570 RID: 25968 RVA: 0x002AD5C8 File Offset: 0x002AC5C8
		protected override void ClearItems()
		{
			this.ClearDisplayIndex(this, null);
			this.DataGridOwner.UpdateDataGridReference(this, true);
			base.ClearItems();
		}

		// Token: 0x06006571 RID: 25969 RVA: 0x002AD5E8 File Offset: 0x002AC5E8
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyColumnCollection(target))
			{
				if (e.Property == DataGridColumn.DisplayIndexProperty)
				{
					this.OnColumnDisplayIndexChanged((DataGridColumn)d, (int)e.OldValue, (int)e.NewValue);
					if (((DataGridColumn)d).IsVisible)
					{
						this.InvalidateColumnRealization(true);
					}
				}
				else if (e.Property == DataGridColumn.WidthProperty)
				{
					if (((DataGridColumn)d).IsVisible)
					{
						this.InvalidateColumnRealization(false);
					}
				}
				else if (e.Property == DataGrid.FrozenColumnCountProperty)
				{
					this.InvalidateColumnRealization(false);
					this.OnDataGridFrozenColumnCountChanged((int)e.OldValue, (int)e.NewValue);
				}
				else if (e.Property == DataGridColumn.VisibilityProperty)
				{
					this.InvalidateAverageColumnWidth();
					this.InvalidateHasVisibleStarColumns();
					this.InvalidateColumnWidthsComputation();
					this.InvalidateColumnRealization(true);
				}
				else if (e.Property == DataGrid.EnableColumnVirtualizationProperty)
				{
					this.InvalidateColumnRealization(true);
				}
				else if (e.Property == DataGrid.CellsPanelHorizontalOffsetProperty)
				{
					this.OnCellsPanelHorizontalOffsetChanged(e);
				}
				else if (e.Property == DataGrid.HorizontalScrollOffsetProperty || string.Compare(propertyName, "ViewportWidth", StringComparison.Ordinal) == 0)
				{
					this.InvalidateColumnRealization(false);
				}
			}
			if (DataGridHelper.ShouldNotifyColumns(target))
			{
				int count = base.Count;
				for (int i = 0; i < count; i++)
				{
					base[i].NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
				}
			}
		}

		// Token: 0x06006572 RID: 25970 RVA: 0x002AD753 File Offset: 0x002AC753
		internal DataGridColumn ColumnFromDisplayIndex(int displayIndex)
		{
			return base[this.DisplayIndexMap[displayIndex]];
		}

		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x06006573 RID: 25971 RVA: 0x002AD767 File Offset: 0x002AC767
		// (set) Token: 0x06006574 RID: 25972 RVA: 0x002AD77D File Offset: 0x002AC77D
		internal List<int> DisplayIndexMap
		{
			get
			{
				if (!this.DisplayIndexMapInitialized)
				{
					this.InitializeDisplayIndexMap();
				}
				return this._displayIndexMap;
			}
			private set
			{
				this._displayIndexMap = value;
			}
		}

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x06006575 RID: 25973 RVA: 0x002AD786 File Offset: 0x002AC786
		// (set) Token: 0x06006576 RID: 25974 RVA: 0x002AD78E File Offset: 0x002AC78E
		private bool IsUpdatingDisplayIndex
		{
			get
			{
				return this._isUpdatingDisplayIndex;
			}
			set
			{
				this._isUpdatingDisplayIndex = value;
			}
		}

		// Token: 0x06006577 RID: 25975 RVA: 0x002AD797 File Offset: 0x002AC797
		private int CoerceDefaultDisplayIndex(DataGridColumn column)
		{
			return this.CoerceDefaultDisplayIndex(column, base.IndexOf(column));
		}

		// Token: 0x06006578 RID: 25976 RVA: 0x002AD7A8 File Offset: 0x002AC7A8
		private int CoerceDefaultDisplayIndex(DataGridColumn column, int newDisplayIndex)
		{
			if (DataGridHelper.IsDefaultValue(column, DataGridColumn.DisplayIndexProperty))
			{
				bool isUpdatingDisplayIndex = this.IsUpdatingDisplayIndex;
				try
				{
					this.IsUpdatingDisplayIndex = true;
					column.DisplayIndex = newDisplayIndex;
				}
				finally
				{
					this.IsUpdatingDisplayIndex = isUpdatingDisplayIndex;
				}
				return newDisplayIndex;
			}
			return column.DisplayIndex;
		}

		// Token: 0x06006579 RID: 25977 RVA: 0x002AD7FC File Offset: 0x002AC7FC
		private void OnColumnDisplayIndexChanged(DataGridColumn column, int oldDisplayIndex, int newDisplayIndex)
		{
			int num = oldDisplayIndex;
			if (!this._displayIndexMapInitialized)
			{
				this.InitializeDisplayIndexMap(column, oldDisplayIndex, out oldDisplayIndex);
			}
			if (this._isClearingDisplayIndex)
			{
				return;
			}
			newDisplayIndex = this.CoerceDefaultDisplayIndex(column);
			if (newDisplayIndex == oldDisplayIndex)
			{
				return;
			}
			if (num != -1)
			{
				this.DataGridOwner.OnColumnDisplayIndexChanged(new DataGridColumnEventArgs(column));
			}
			this.UpdateDisplayIndexForChangedColumn(oldDisplayIndex, newDisplayIndex);
		}

		// Token: 0x0600657A RID: 25978 RVA: 0x002AD854 File Offset: 0x002AC854
		private void UpdateDisplayIndexForChangedColumn(int oldDisplayIndex, int newDisplayIndex)
		{
			if (this.IsUpdatingDisplayIndex)
			{
				return;
			}
			try
			{
				this.IsUpdatingDisplayIndex = true;
				int item = this.DisplayIndexMap[oldDisplayIndex];
				this.DisplayIndexMap.RemoveAt(oldDisplayIndex);
				this.DisplayIndexMap.Insert(newDisplayIndex, item);
				if (newDisplayIndex < oldDisplayIndex)
				{
					for (int i = newDisplayIndex + 1; i <= oldDisplayIndex; i++)
					{
						DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
						int displayIndex = dataGridColumn.DisplayIndex;
						dataGridColumn.DisplayIndex = displayIndex + 1;
					}
				}
				else
				{
					for (int j = oldDisplayIndex; j < newDisplayIndex; j++)
					{
						DataGridColumn dataGridColumn2 = this.ColumnFromDisplayIndex(j);
						int displayIndex = dataGridColumn2.DisplayIndex;
						dataGridColumn2.DisplayIndex = displayIndex - 1;
					}
				}
				this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Move, oldDisplayIndex, null, newDisplayIndex);
			}
			finally
			{
				this.IsUpdatingDisplayIndex = false;
			}
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x002AD910 File Offset: 0x002AC910
		private void UpdateDisplayIndexForMovedColumn(int oldColumnIndex, int newColumnIndex)
		{
			int newDisplayIndex = this.RemoveFromDisplayIndexMap(oldColumnIndex);
			this.InsertInDisplayIndexMap(newDisplayIndex, newColumnIndex);
			this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Move, oldColumnIndex, null, newColumnIndex);
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x002AD93C File Offset: 0x002AC93C
		private void UpdateDisplayIndexForNewColumns(IList newColumns, int startingIndex)
		{
			try
			{
				this.IsUpdatingDisplayIndex = true;
				DataGridColumn dataGridColumn = (DataGridColumn)newColumns[0];
				int num = this.CoerceDefaultDisplayIndex(dataGridColumn, startingIndex);
				this.InsertInDisplayIndexMap(num, startingIndex);
				for (int i = 0; i < this.DisplayIndexMap.Count; i++)
				{
					if (i > num)
					{
						dataGridColumn = this.ColumnFromDisplayIndex(i);
						DataGridColumn dataGridColumn2 = dataGridColumn;
						int displayIndex = dataGridColumn2.DisplayIndex;
						dataGridColumn2.DisplayIndex = displayIndex + 1;
					}
				}
				this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Add, -1, null, num);
			}
			finally
			{
				this.IsUpdatingDisplayIndex = false;
			}
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x002AD9D0 File Offset: 0x002AC9D0
		internal void InitializeDisplayIndexMap()
		{
			int num = -1;
			this.InitializeDisplayIndexMap(null, -1, out num);
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x002AD9EC File Offset: 0x002AC9EC
		private void InitializeDisplayIndexMap(DataGridColumn changingColumn, int oldDisplayIndex, out int resultDisplayIndex)
		{
			resultDisplayIndex = oldDisplayIndex;
			if (this._displayIndexMapInitialized)
			{
				return;
			}
			this._displayIndexMapInitialized = true;
			int count = base.Count;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			if (changingColumn != null && oldDisplayIndex >= count)
			{
				throw new ArgumentOutOfRangeException("displayIndex", oldDisplayIndex, SR.Get("DataGrid_ColumnDisplayIndexOutOfRange", new object[]
				{
					changingColumn.Header
				}));
			}
			for (int i = 0; i < count; i++)
			{
				DataGridColumn dataGridColumn = base[i];
				int num = dataGridColumn.DisplayIndex;
				this.ValidateDisplayIndex(dataGridColumn, num);
				if (dataGridColumn == changingColumn)
				{
					num = oldDisplayIndex;
				}
				if (num >= 0)
				{
					if (dictionary.ContainsKey(num))
					{
						throw new ArgumentException(SR.Get("DataGrid_DuplicateDisplayIndex"));
					}
					dictionary.Add(num, i);
				}
			}
			int num2 = 0;
			for (int j = 0; j < count; j++)
			{
				DataGridColumn dataGridColumn2 = base[j];
				int displayIndex = dataGridColumn2.DisplayIndex;
				bool flag = DataGridHelper.IsDefaultValue(dataGridColumn2, DataGridColumn.DisplayIndexProperty);
				if (dataGridColumn2 == changingColumn && oldDisplayIndex == -1)
				{
					flag = true;
				}
				if (flag)
				{
					while (dictionary.ContainsKey(num2))
					{
						num2++;
					}
					this.CoerceDefaultDisplayIndex(dataGridColumn2, num2);
					dictionary.Add(num2, j);
					if (dataGridColumn2 == changingColumn)
					{
						resultDisplayIndex = num2;
					}
					num2++;
				}
			}
			for (int k = 0; k < count; k++)
			{
				this.DisplayIndexMap.Add(dictionary[k]);
			}
		}

		// Token: 0x0600657F RID: 25983 RVA: 0x002ADB38 File Offset: 0x002ACB38
		private void UpdateDisplayIndexForRemovedColumns(IList oldColumns, int startingIndex)
		{
			try
			{
				this.IsUpdatingDisplayIndex = true;
				int num = this.RemoveFromDisplayIndexMap(startingIndex);
				for (int i = 0; i < this.DisplayIndexMap.Count; i++)
				{
					if (i >= num)
					{
						DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
						int displayIndex = dataGridColumn.DisplayIndex;
						dataGridColumn.DisplayIndex = displayIndex - 1;
					}
				}
				this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Remove, num, (DataGridColumn)oldColumns[0], -1);
			}
			finally
			{
				this.IsUpdatingDisplayIndex = false;
			}
		}

		// Token: 0x06006580 RID: 25984 RVA: 0x002ADBB8 File Offset: 0x002ACBB8
		private void UpdateDisplayIndexForReplacedColumn(IList oldColumns, IList newColumns)
		{
			if (oldColumns != null && oldColumns.Count > 0 && newColumns != null && newColumns.Count > 0)
			{
				DataGridColumn dataGridColumn = (DataGridColumn)oldColumns[0];
				DataGridColumn dataGridColumn2 = (DataGridColumn)newColumns[0];
				if (dataGridColumn != null && dataGridColumn2 != null)
				{
					int num = this.CoerceDefaultDisplayIndex(dataGridColumn2);
					if (dataGridColumn.DisplayIndex != num)
					{
						this.UpdateDisplayIndexForChangedColumn(dataGridColumn.DisplayIndex, num);
					}
					this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Replace, num, dataGridColumn, num);
				}
			}
		}

		// Token: 0x06006581 RID: 25985 RVA: 0x002ADC2C File Offset: 0x002ACC2C
		private void ClearDisplayIndex(IList oldColumns, IList newColumns)
		{
			if (oldColumns != null)
			{
				try
				{
					this._isClearingDisplayIndex = true;
					int count = oldColumns.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridColumn dataGridColumn = (DataGridColumn)oldColumns[i];
						if (newColumns == null || !newColumns.Contains(dataGridColumn))
						{
							dataGridColumn.ClearValue(DataGridColumn.DisplayIndexProperty);
						}
					}
				}
				finally
				{
					this._isClearingDisplayIndex = false;
				}
			}
		}

		// Token: 0x06006582 RID: 25986 RVA: 0x002ADC94 File Offset: 0x002ACC94
		private bool IsDisplayIndexValid(DataGridColumn column, int displayIndex, bool isAdding)
		{
			if (displayIndex == -1 && DataGridHelper.IsDefaultValue(column, DataGridColumn.DisplayIndexProperty))
			{
				return true;
			}
			if (displayIndex < 0)
			{
				return false;
			}
			if (!isAdding)
			{
				return displayIndex < base.Count;
			}
			return displayIndex <= base.Count;
		}

		// Token: 0x06006583 RID: 25987 RVA: 0x002ADCC8 File Offset: 0x002ACCC8
		private void InsertInDisplayIndexMap(int newDisplayIndex, int columnIndex)
		{
			this.DisplayIndexMap.Insert(newDisplayIndex, columnIndex);
			for (int i = 0; i < this.DisplayIndexMap.Count; i++)
			{
				if (this.DisplayIndexMap[i] >= columnIndex && i != newDisplayIndex)
				{
					List<int> displayIndexMap = this.DisplayIndexMap;
					int index = i;
					int num = displayIndexMap[index];
					displayIndexMap[index] = num + 1;
				}
			}
		}

		// Token: 0x06006584 RID: 25988 RVA: 0x002ADD24 File Offset: 0x002ACD24
		private int RemoveFromDisplayIndexMap(int columnIndex)
		{
			int num = this.DisplayIndexMap.IndexOf(columnIndex);
			this.DisplayIndexMap.RemoveAt(num);
			for (int i = 0; i < this.DisplayIndexMap.Count; i++)
			{
				if (this.DisplayIndexMap[i] >= columnIndex)
				{
					List<int> displayIndexMap = this.DisplayIndexMap;
					int index = i;
					int num2 = displayIndexMap[index];
					displayIndexMap[index] = num2 - 1;
				}
			}
			return num;
		}

		// Token: 0x06006585 RID: 25989 RVA: 0x002ADD89 File Offset: 0x002ACD89
		internal void ValidateDisplayIndex(DataGridColumn column, int displayIndex)
		{
			this.ValidateDisplayIndex(column, displayIndex, false);
		}

		// Token: 0x06006586 RID: 25990 RVA: 0x002ADD94 File Offset: 0x002ACD94
		internal void ValidateDisplayIndex(DataGridColumn column, int displayIndex, bool isAdding)
		{
			if (!this.IsDisplayIndexValid(column, displayIndex, isAdding))
			{
				throw new ArgumentOutOfRangeException("displayIndex", displayIndex, SR.Get("DataGrid_ColumnDisplayIndexOutOfRange", new object[]
				{
					column.Header
				}));
			}
		}

		// Token: 0x06006587 RID: 25991 RVA: 0x002ADDCC File Offset: 0x002ACDCC
		[Conditional("DEBUG")]
		private void Debug_VerifyDisplayIndexMap()
		{
			for (int i = 0; i < this.DisplayIndexMap.Count; i++)
			{
			}
		}

		// Token: 0x06006588 RID: 25992 RVA: 0x002ADDF0 File Offset: 0x002ACDF0
		private void OnDataGridFrozenColumnCountChanged(int oldFrozenCount, int newFrozenCount)
		{
			if (newFrozenCount > oldFrozenCount)
			{
				int num = Math.Min(newFrozenCount, base.Count);
				for (int i = oldFrozenCount; i < num; i++)
				{
					this.ColumnFromDisplayIndex(i).IsFrozen = true;
				}
				return;
			}
			int num2 = Math.Min(oldFrozenCount, base.Count);
			for (int j = newFrozenCount; j < num2; j++)
			{
				this.ColumnFromDisplayIndex(j).IsFrozen = false;
			}
		}

		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x06006589 RID: 25993 RVA: 0x002ADE4E File Offset: 0x002ACE4E
		private DataGrid DataGridOwner
		{
			get
			{
				return this._dataGridOwner;
			}
		}

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x0600658A RID: 25994 RVA: 0x002ADE56 File Offset: 0x002ACE56
		internal bool DisplayIndexMapInitialized
		{
			get
			{
				return this._displayIndexMapInitialized;
			}
		}

		// Token: 0x0600658B RID: 25995 RVA: 0x002ADE60 File Offset: 0x002ACE60
		private bool HasVisibleStarColumnsInternal(DataGridColumn ignoredColumn, out double perStarWidth)
		{
			bool result = false;
			perStarWidth = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn != ignoredColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar)
					{
						result = true;
						if (!DoubleUtil.AreClose(width.Value, 0.0) && !DoubleUtil.AreClose(width.DesiredValue, 0.0))
						{
							perStarWidth = width.DesiredValue / width.Value;
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600658C RID: 25996 RVA: 0x002ADF10 File Offset: 0x002ACF10
		private bool HasVisibleStarColumnsInternal(out double perStarWidth)
		{
			return this.HasVisibleStarColumnsInternal(null, out perStarWidth);
		}

		// Token: 0x0600658D RID: 25997 RVA: 0x002ADF1C File Offset: 0x002ACF1C
		private bool HasVisibleStarColumnsInternal(DataGridColumn ignoredColumn)
		{
			double num;
			return this.HasVisibleStarColumnsInternal(ignoredColumn, out num);
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x0600658E RID: 25998 RVA: 0x002ADF32 File Offset: 0x002ACF32
		// (set) Token: 0x0600658F RID: 25999 RVA: 0x002ADF3A File Offset: 0x002ACF3A
		internal bool HasVisibleStarColumns
		{
			get
			{
				return this._hasVisibleStarColumns;
			}
			private set
			{
				if (this._hasVisibleStarColumns != value)
				{
					this._hasVisibleStarColumns = value;
					this.DataGridOwner.OnHasVisibleStarColumnsChanged();
				}
			}
		}

		// Token: 0x06006590 RID: 26000 RVA: 0x002ADF57 File Offset: 0x002ACF57
		internal void InvalidateHasVisibleStarColumns()
		{
			this.HasVisibleStarColumns = this.HasVisibleStarColumnsInternal(null);
		}

		// Token: 0x06006591 RID: 26001 RVA: 0x002ADF68 File Offset: 0x002ACF68
		private void RecomputeStarColumnWidths()
		{
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			double num = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				DataGridLength width = dataGridColumn.Width;
				if (dataGridColumn.IsVisible && !width.IsStar)
				{
					num += width.DisplayValue;
				}
			}
			if (DoubleUtil.IsNaN(num))
			{
				return;
			}
			this.ComputeStarColumnWidths(viewportWidthForColumns - num);
		}

		// Token: 0x06006592 RID: 26002 RVA: 0x002ADFF4 File Offset: 0x002ACFF4
		private double ComputeStarColumnWidths(double availableStarSpace)
		{
			List<DataGridColumn> list = new List<DataGridColumn>();
			List<DataGridColumn> list2 = new List<DataGridColumn>();
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				DataGridLength width = dataGridColumn.Width;
				if (dataGridColumn.IsVisible && width.IsStar)
				{
					list.Add(dataGridColumn);
					num += width.Value;
					num2 += dataGridColumn.MinWidth;
					num3 += dataGridColumn.MaxWidth;
				}
			}
			if (DoubleUtil.LessThan(availableStarSpace, num2))
			{
				availableStarSpace = num2;
			}
			if (DoubleUtil.GreaterThan(availableStarSpace, num3))
			{
				availableStarSpace = num3;
			}
			while (list.Count > 0)
			{
				double num5 = availableStarSpace / num;
				int i = 0;
				int num6 = list.Count;
				while (i < num6)
				{
					DataGridColumn dataGridColumn2 = list[i];
					DataGridLength width2 = dataGridColumn2.Width;
					double minWidth = dataGridColumn2.MinWidth;
					double value = availableStarSpace * width2.Value / num;
					if (DoubleUtil.GreaterThan(minWidth, value))
					{
						availableStarSpace = Math.Max(0.0, availableStarSpace - minWidth);
						num -= width2.Value;
						list.RemoveAt(i);
						i--;
						num6--;
						list2.Add(dataGridColumn2);
					}
					i++;
				}
				bool flag = false;
				int j = 0;
				int count = list.Count;
				while (j < count)
				{
					DataGridColumn dataGridColumn3 = list[j];
					DataGridLength width3 = dataGridColumn3.Width;
					double maxWidth = dataGridColumn3.MaxWidth;
					double value2 = availableStarSpace * width3.Value / num;
					if (DoubleUtil.LessThan(maxWidth, value2))
					{
						flag = true;
						list.RemoveAt(j);
						availableStarSpace -= maxWidth;
						num4 += maxWidth;
						num -= width3.Value;
						dataGridColumn3.UpdateWidthForStarColumn(maxWidth, num5 * width3.Value, width3.Value);
						break;
					}
					j++;
				}
				if (flag)
				{
					int k = 0;
					int count2 = list2.Count;
					while (k < count2)
					{
						DataGridColumn dataGridColumn4 = list2[k];
						list.Add(dataGridColumn4);
						availableStarSpace += dataGridColumn4.MinWidth;
						num += dataGridColumn4.Width.Value;
						k++;
					}
					list2.Clear();
				}
				else
				{
					int l = 0;
					int count3 = list2.Count;
					while (l < count3)
					{
						DataGridColumn dataGridColumn5 = list2[l];
						DataGridLength width4 = dataGridColumn5.Width;
						double minWidth2 = dataGridColumn5.MinWidth;
						dataGridColumn5.UpdateWidthForStarColumn(minWidth2, width4.Value * num5, width4.Value);
						num4 += minWidth2;
						l++;
					}
					list2.Clear();
					int m = 0;
					int count4 = list.Count;
					while (m < count4)
					{
						DataGridColumn dataGridColumn6 = list[m];
						DataGridLength width5 = dataGridColumn6.Width;
						double num7 = availableStarSpace * width5.Value / num;
						dataGridColumn6.UpdateWidthForStarColumn(num7, width5.Value * num5, width5.Value);
						num4 += num7;
						m++;
					}
					list.Clear();
				}
			}
			return num4;
		}

		// Token: 0x06006593 RID: 26003 RVA: 0x002AE308 File Offset: 0x002AD308
		private void OnCellsPanelHorizontalOffsetChanged(DependencyPropertyChangedEventArgs e)
		{
			this.InvalidateColumnRealization(false);
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			this.RedistributeColumnWidthsOnAvailableSpaceChange((double)e.OldValue - (double)e.NewValue, viewportWidthForColumns);
		}

		// Token: 0x06006594 RID: 26004 RVA: 0x002AE348 File Offset: 0x002AD348
		internal void InvalidateAverageColumnWidth()
		{
			this._averageColumnWidth = null;
			VirtualizingStackPanel virtualizingStackPanel = (this.DataGridOwner == null) ? null : (this.DataGridOwner.InternalItemsHost as VirtualizingStackPanel);
			if (virtualizingStackPanel != null)
			{
				virtualizingStackPanel.ResetMaximumDesiredSize();
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x06006595 RID: 26005 RVA: 0x002AE386 File Offset: 0x002AD386
		internal double AverageColumnWidth
		{
			get
			{
				if (this._averageColumnWidth == null)
				{
					this._averageColumnWidth = new double?(this.ComputeAverageColumnWidth());
				}
				return this._averageColumnWidth.Value;
			}
		}

		// Token: 0x06006596 RID: 26006 RVA: 0x002AE3B4 File Offset: 0x002AD3B4
		private double ComputeAverageColumnWidth()
		{
			double num = 0.0;
			int num2 = 0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				DataGridLength width = dataGridColumn.Width;
				if (dataGridColumn.IsVisible && !DoubleUtil.IsNaN(width.DisplayValue))
				{
					num += width.DisplayValue;
					num2++;
				}
			}
			if (num2 != 0)
			{
				return num / (double)num2;
			}
			return 0.0;
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x06006597 RID: 26007 RVA: 0x002AE43C File Offset: 0x002AD43C
		internal bool ColumnWidthsComputationPending
		{
			get
			{
				return this._columnWidthsComputationPending;
			}
		}

		// Token: 0x06006598 RID: 26008 RVA: 0x002AE444 File Offset: 0x002AD444
		internal void InvalidateColumnWidthsComputation()
		{
			if (this._columnWidthsComputationPending)
			{
				return;
			}
			this.DataGridOwner.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.ComputeColumnWidths), DispatcherPriority.Render, new object[]
			{
				this
			});
			this._columnWidthsComputationPending = true;
		}

		// Token: 0x06006599 RID: 26009 RVA: 0x002AE480 File Offset: 0x002AD480
		private object ComputeColumnWidths(object arg)
		{
			this.ComputeColumnWidths();
			this.DataGridOwner.NotifyPropertyChanged(this.DataGridOwner, "DelayedColumnWidthComputation", default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnHeadersPresenter);
			return null;
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x002AE4B8 File Offset: 0x002AD4B8
		private void ComputeColumnWidths()
		{
			if (this.HasVisibleStarColumns)
			{
				this.InitializeColumnDisplayValues();
				this.DistributeSpaceAmongColumns(this.DataGridOwner.GetViewportWidthForColumns());
			}
			else
			{
				this.ExpandAllColumnWidthsToDesiredValue();
			}
			if (this.RefreshAutoWidthColumns)
			{
				foreach (DataGridColumn dataGridColumn in this)
				{
					if (dataGridColumn.Width.IsAuto)
					{
						dataGridColumn.Width = DataGridLength.Auto;
					}
				}
				this.RefreshAutoWidthColumns = false;
			}
			this._columnWidthsComputationPending = false;
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x002AE554 File Offset: 0x002AD554
		private void InitializeColumnDisplayValues()
		{
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar)
					{
						double minWidth = dataGridColumn.MinWidth;
						double num = DataGridHelper.CoerceToMinMax(DoubleUtil.IsNaN(width.DesiredValue) ? minWidth : width.DesiredValue, minWidth, dataGridColumn.MaxWidth);
						if (!DoubleUtil.AreClose(width.DisplayValue, num))
						{
							dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, num));
						}
					}
				}
			}
		}

		// Token: 0x0600659C RID: 26012 RVA: 0x002AE610 File Offset: 0x002AD610
		internal void RedistributeColumnWidthsOnMinWidthChangeOfColumn(DataGridColumn changedColumn, double oldMinWidth)
		{
			if (this.ColumnWidthsComputationPending)
			{
				return;
			}
			DataGridLength width = changedColumn.Width;
			double minWidth = changedColumn.MinWidth;
			if (DoubleUtil.GreaterThan(minWidth, width.DisplayValue))
			{
				if (this.HasVisibleStarColumns)
				{
					this.TakeAwayWidthFromColumns(changedColumn, minWidth - width.DisplayValue, false);
				}
				changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, minWidth));
				return;
			}
			if (DoubleUtil.LessThan(minWidth, oldMinWidth))
			{
				if (width.IsStar)
				{
					if (DoubleUtil.AreClose(width.DisplayValue, oldMinWidth))
					{
						this.GiveAwayWidthToColumns(changedColumn, oldMinWidth - minWidth, true);
						return;
					}
				}
				else if (DoubleUtil.GreaterThan(oldMinWidth, width.DesiredValue))
				{
					double num = Math.Max(width.DesiredValue, minWidth);
					if (this.HasVisibleStarColumns)
					{
						this.GiveAwayWidthToColumns(changedColumn, oldMinWidth - num);
					}
					changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, num));
				}
			}
		}

		// Token: 0x0600659D RID: 26013 RVA: 0x002AE700 File Offset: 0x002AD700
		internal void RedistributeColumnWidthsOnMaxWidthChangeOfColumn(DataGridColumn changedColumn, double oldMaxWidth)
		{
			if (this.ColumnWidthsComputationPending)
			{
				return;
			}
			DataGridLength width = changedColumn.Width;
			double maxWidth = changedColumn.MaxWidth;
			if (DoubleUtil.LessThan(maxWidth, width.DisplayValue))
			{
				if (this.HasVisibleStarColumns)
				{
					this.GiveAwayWidthToColumns(changedColumn, width.DisplayValue - maxWidth);
				}
				changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, maxWidth));
				return;
			}
			if (DoubleUtil.GreaterThan(maxWidth, oldMaxWidth))
			{
				if (width.IsStar)
				{
					this.RecomputeStarColumnWidths();
					return;
				}
				if (DoubleUtil.LessThan(oldMaxWidth, width.DesiredValue))
				{
					double num = Math.Min(width.DesiredValue, maxWidth);
					if (this.HasVisibleStarColumns)
					{
						double num2 = this.TakeAwayWidthFromUnusedSpace(false, num - oldMaxWidth);
						num2 = this.TakeAwayWidthFromStarColumns(changedColumn, num2);
						num -= num2;
					}
					changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, num));
				}
			}
		}

		// Token: 0x0600659E RID: 26014 RVA: 0x002AE7E8 File Offset: 0x002AD7E8
		internal void RedistributeColumnWidthsOnWidthChangeOfColumn(DataGridColumn changedColumn, DataGridLength oldWidth)
		{
			if (this.ColumnWidthsComputationPending)
			{
				return;
			}
			DataGridLength width = changedColumn.Width;
			bool hasVisibleStarColumns = this.HasVisibleStarColumns;
			if (oldWidth.IsStar && !width.IsStar && !hasVisibleStarColumns)
			{
				this.ExpandAllColumnWidthsToDesiredValue();
				return;
			}
			if (width.IsStar && !oldWidth.IsStar)
			{
				if (!this.HasVisibleStarColumnsInternal(changedColumn))
				{
					this.ComputeColumnWidths();
					return;
				}
				double minWidth = changedColumn.MinWidth;
				double num = this.GiveAwayWidthToNonStarColumns(null, oldWidth.DisplayValue - minWidth);
				changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, minWidth + num));
				this.RecomputeStarColumnWidths();
				return;
			}
			else
			{
				if (width.IsStar && oldWidth.IsStar)
				{
					this.RecomputeStarColumnWidths();
					return;
				}
				if (hasVisibleStarColumns)
				{
					this.RedistributeColumnWidthsOnNonStarWidthChange(changedColumn, oldWidth);
				}
				return;
			}
		}

		// Token: 0x0600659F RID: 26015 RVA: 0x002AE8B0 File Offset: 0x002AD8B0
		internal void RedistributeColumnWidthsOnAvailableSpaceChange(double availableSpaceChange, double newTotalAvailableSpace)
		{
			if (!this.ColumnWidthsComputationPending && this.HasVisibleStarColumns)
			{
				if (DoubleUtil.GreaterThan(availableSpaceChange, 0.0))
				{
					this.GiveAwayWidthToColumns(null, availableSpaceChange);
					return;
				}
				if (DoubleUtil.LessThan(availableSpaceChange, 0.0))
				{
					this.TakeAwayWidthFromColumns(null, Math.Abs(availableSpaceChange), false, newTotalAvailableSpace);
				}
			}
		}

		// Token: 0x060065A0 RID: 26016 RVA: 0x002AE90C File Offset: 0x002AD90C
		private void ExpandAllColumnWidthsToDesiredValue()
		{
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					double maxWidth = dataGridColumn.MaxWidth;
					if (DoubleUtil.GreaterThan(width.DesiredValue, width.DisplayValue) && !DoubleUtil.AreClose(width.DisplayValue, maxWidth))
					{
						dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, Math.Min(width.DesiredValue, maxWidth)));
					}
				}
			}
		}

		// Token: 0x060065A1 RID: 26017 RVA: 0x002AE9B4 File Offset: 0x002AD9B4
		private void RedistributeColumnWidthsOnNonStarWidthChange(DataGridColumn changedColumn, DataGridLength oldWidth)
		{
			DataGridLength width = changedColumn.Width;
			if (DoubleUtil.GreaterThan(width.DesiredValue, oldWidth.DisplayValue))
			{
				double num = this.TakeAwayWidthFromColumns(changedColumn, width.DesiredValue - oldWidth.DisplayValue, changedColumn != null);
				if (DoubleUtil.GreaterThan(num, 0.0))
				{
					changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, Math.Max(width.DisplayValue - num, changedColumn.MinWidth)));
					return;
				}
			}
			else if (DoubleUtil.LessThan(width.DesiredValue, oldWidth.DisplayValue))
			{
				double num2 = DataGridHelper.CoerceToMinMax(width.DesiredValue, changedColumn.MinWidth, changedColumn.MaxWidth);
				this.GiveAwayWidthToColumns(changedColumn, oldWidth.DisplayValue - num2);
			}
		}

		// Token: 0x060065A2 RID: 26018 RVA: 0x002AEA80 File Offset: 0x002ADA80
		private void DistributeSpaceAmongColumns(double availableSpace)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					num += dataGridColumn.MinWidth;
					num2 += dataGridColumn.MaxWidth;
					if (dataGridColumn.Width.IsStar)
					{
						num3 += dataGridColumn.MinWidth;
					}
				}
			}
			if (DoubleUtil.LessThan(availableSpace, num))
			{
				availableSpace = num;
			}
			if (DoubleUtil.GreaterThan(availableSpace, num2))
			{
				availableSpace = num2;
			}
			double num4 = this.DistributeSpaceAmongNonStarColumns(availableSpace - num3);
			this.ComputeStarColumnWidths(num3 + num4);
		}

		// Token: 0x060065A3 RID: 26019 RVA: 0x002AEB4C File Offset: 0x002ADB4C
		private double DistributeSpaceAmongNonStarColumns(double availableSpace)
		{
			double num = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				DataGridLength width = dataGridColumn.Width;
				if (dataGridColumn.IsVisible && !width.IsStar)
				{
					num += width.DisplayValue;
				}
			}
			if (DoubleUtil.LessThan(availableSpace, num))
			{
				double takeAwayWidth = num - availableSpace;
				this.TakeAwayWidthFromNonStarColumns(null, takeAwayWidth);
			}
			return Math.Max(availableSpace - num, 0.0);
		}

		// Token: 0x060065A4 RID: 26020 RVA: 0x002AEBE0 File Offset: 0x002ADBE0
		internal void OnColumnResizeStarted()
		{
			this._originalWidthsForResize = new Dictionary<DataGridColumn, DataGridLength>();
			foreach (DataGridColumn dataGridColumn in this)
			{
				this._originalWidthsForResize[dataGridColumn] = dataGridColumn.Width;
			}
		}

		// Token: 0x060065A5 RID: 26021 RVA: 0x002AEC40 File Offset: 0x002ADC40
		internal void OnColumnResizeCompleted(bool cancel)
		{
			if (cancel && this._originalWidthsForResize != null)
			{
				foreach (DataGridColumn dataGridColumn in this)
				{
					if (this._originalWidthsForResize.ContainsKey(dataGridColumn))
					{
						dataGridColumn.Width = this._originalWidthsForResize[dataGridColumn];
					}
				}
			}
			this._originalWidthsForResize = null;
		}

		// Token: 0x060065A6 RID: 26022 RVA: 0x002AECB4 File Offset: 0x002ADCB4
		internal void RecomputeColumnWidthsOnColumnResize(DataGridColumn resizingColumn, double horizontalChange, bool retainAuto)
		{
			DataGridLength width = resizingColumn.Width;
			double value = width.DisplayValue + horizontalChange;
			if (DoubleUtil.LessThan(value, resizingColumn.MinWidth))
			{
				horizontalChange = resizingColumn.MinWidth - width.DisplayValue;
			}
			else if (DoubleUtil.GreaterThan(value, resizingColumn.MaxWidth))
			{
				horizontalChange = resizingColumn.MaxWidth - width.DisplayValue;
			}
			int displayIndex = resizingColumn.DisplayIndex;
			if (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				this.RecomputeColumnWidthsOnColumnPositiveResize(horizontalChange, displayIndex, retainAuto);
				return;
			}
			if (DoubleUtil.LessThan(horizontalChange, 0.0))
			{
				this.RecomputeColumnWidthsOnColumnNegativeResize(-horizontalChange, displayIndex, retainAuto);
			}
		}

		// Token: 0x060065A7 RID: 26023 RVA: 0x002AED50 File Offset: 0x002ADD50
		private void RecomputeColumnWidthsOnColumnPositiveResize(double horizontalChange, int resizingColumnIndex, bool retainAuto)
		{
			double perStarWidth = 0.0;
			if (this.HasVisibleStarColumnsInternal(out perStarWidth))
			{
				horizontalChange = this.TakeAwayUnusedSpaceOnColumnPositiveResize(horizontalChange, resizingColumnIndex, retainAuto);
				horizontalChange = this.RecomputeNonStarColumnWidthsOnColumnPositiveResize(horizontalChange, resizingColumnIndex, retainAuto, true);
				horizontalChange = this.RecomputeStarColumnWidthsOnColumnPositiveResize(horizontalChange, resizingColumnIndex, perStarWidth, retainAuto);
				horizontalChange = this.RecomputeNonStarColumnWidthsOnColumnPositiveResize(horizontalChange, resizingColumnIndex, retainAuto, false);
				return;
			}
			DataGridColumnCollection.SetResizedColumnWidth(this.ColumnFromDisplayIndex(resizingColumnIndex), horizontalChange, retainAuto);
		}

		// Token: 0x060065A8 RID: 26024 RVA: 0x002AEDB0 File Offset: 0x002ADDB0
		private double RecomputeStarColumnWidthsOnColumnPositiveResize(double horizontalChange, int resizingColumnIndex, double perStarWidth, bool retainAuto)
		{
			while (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				double positiveInfinity = double.PositiveInfinity;
				double starFactorsForPositiveResize = this.GetStarFactorsForPositiveResize(resizingColumnIndex + 1, out positiveInfinity);
				if (!DoubleUtil.GreaterThan(starFactorsForPositiveResize, 0.0))
				{
					break;
				}
				horizontalChange = this.ReallocateStarValuesForPositiveResize(resizingColumnIndex, horizontalChange, positiveInfinity, starFactorsForPositiveResize, perStarWidth, retainAuto);
				if (DoubleUtil.AreClose(horizontalChange, 0.0))
				{
					break;
				}
			}
			return horizontalChange;
		}

		// Token: 0x060065A9 RID: 26025 RVA: 0x002AEE18 File Offset: 0x002ADE18
		private static bool CanColumnParticipateInResize(DataGridColumn column)
		{
			return column.IsVisible && column.CanUserResize;
		}

		// Token: 0x060065AA RID: 26026 RVA: 0x002AEE2C File Offset: 0x002ADE2C
		private double GetStarFactorsForPositiveResize(int startIndex, out double minPerStarExcessRatio)
		{
			minPerStarExcessRatio = double.PositiveInfinity;
			double num = 0.0;
			int i = startIndex;
			int count = base.Count;
			while (i < count)
			{
				DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
				if (DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn))
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar && !DoubleUtil.AreClose(width.Value, 0.0) && DoubleUtil.GreaterThan(width.DisplayValue, dataGridColumn.MinWidth))
					{
						num += width.Value;
						double num2 = (width.DisplayValue - dataGridColumn.MinWidth) / width.Value;
						if (DoubleUtil.LessThan(num2, minPerStarExcessRatio))
						{
							minPerStarExcessRatio = num2;
						}
					}
				}
				i++;
			}
			return num;
		}

		// Token: 0x060065AB RID: 26027 RVA: 0x002AEEE8 File Offset: 0x002ADEE8
		private double ReallocateStarValuesForPositiveResize(int startIndex, double horizontalChange, double perStarExcessRatio, double totalStarFactors, double perStarWidth, bool retainAuto)
		{
			double num;
			double num2;
			if (DoubleUtil.LessThan(horizontalChange, perStarExcessRatio * totalStarFactors))
			{
				num = horizontalChange / totalStarFactors;
				num2 = horizontalChange;
				horizontalChange = 0.0;
			}
			else
			{
				num = perStarExcessRatio;
				num2 = num * totalStarFactors;
				horizontalChange -= num2;
			}
			int i = startIndex;
			int count = base.Count;
			while (i < count)
			{
				DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
				DataGridLength width = dataGridColumn.Width;
				if (i == startIndex)
				{
					DataGridColumnCollection.SetResizedColumnWidth(dataGridColumn, num2, retainAuto);
				}
				else if (dataGridColumn.Width.IsStar && DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn) && DoubleUtil.GreaterThan(width.DisplayValue, dataGridColumn.MinWidth))
				{
					double num3 = width.DisplayValue - width.Value * num;
					dataGridColumn.UpdateWidthForStarColumn(Math.Max(num3, dataGridColumn.MinWidth), num3, num3 / perStarWidth);
				}
				i++;
			}
			return horizontalChange;
		}

		// Token: 0x060065AC RID: 26028 RVA: 0x002AEFD0 File Offset: 0x002ADFD0
		private double RecomputeNonStarColumnWidthsOnColumnPositiveResize(double horizontalChange, int resizingColumnIndex, bool retainAuto, bool onlyShrinkToDesiredWidth)
		{
			if (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				double num = 0.0;
				bool flag = true;
				int num2 = base.Count - 1;
				while (flag && num2 > resizingColumnIndex)
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(num2);
					if (DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn))
					{
						DataGridLength width = dataGridColumn.Width;
						double num3 = onlyShrinkToDesiredWidth ? (width.DisplayValue - Math.Max(width.DesiredValue, dataGridColumn.MinWidth)) : (width.DisplayValue - dataGridColumn.MinWidth);
						if (!width.IsStar && DoubleUtil.GreaterThan(num3, 0.0))
						{
							if (DoubleUtil.GreaterThanOrClose(num + num3, horizontalChange))
							{
								num3 = horizontalChange - num;
								flag = false;
							}
							dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue - num3));
							num += num3;
						}
					}
					num2--;
				}
				if (DoubleUtil.GreaterThan(num, 0.0))
				{
					DataGridColumnCollection.SetResizedColumnWidth(this.ColumnFromDisplayIndex(resizingColumnIndex), num, retainAuto);
					horizontalChange -= num;
				}
			}
			return horizontalChange;
		}

		// Token: 0x060065AD RID: 26029 RVA: 0x002AF0E8 File Offset: 0x002AE0E8
		private void RecomputeColumnWidthsOnColumnNegativeResize(double horizontalChange, int resizingColumnIndex, bool retainAuto)
		{
			double perStarWidth = 0.0;
			if (this.HasVisibleStarColumnsInternal(out perStarWidth))
			{
				horizontalChange = this.RecomputeNonStarColumnWidthsOnColumnNegativeResize(horizontalChange, resizingColumnIndex, retainAuto, false);
				horizontalChange = this.RecomputeStarColumnWidthsOnColumnNegativeResize(horizontalChange, resizingColumnIndex, perStarWidth, retainAuto);
				horizontalChange = this.RecomputeNonStarColumnWidthsOnColumnNegativeResize(horizontalChange, resizingColumnIndex, retainAuto, true);
				if (DoubleUtil.GreaterThan(horizontalChange, 0.0))
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(resizingColumnIndex);
					if (!dataGridColumn.Width.IsStar)
					{
						DataGridColumnCollection.SetResizedColumnWidth(dataGridColumn, -horizontalChange, retainAuto);
						return;
					}
				}
			}
			else
			{
				DataGridColumnCollection.SetResizedColumnWidth(this.ColumnFromDisplayIndex(resizingColumnIndex), -horizontalChange, retainAuto);
			}
		}

		// Token: 0x060065AE RID: 26030 RVA: 0x002AF170 File Offset: 0x002AE170
		private double RecomputeNonStarColumnWidthsOnColumnNegativeResize(double horizontalChange, int resizingColumnIndex, bool retainAuto, bool expandBeyondDesiredWidth)
		{
			if (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				double num = 0.0;
				bool flag = true;
				int num2 = resizingColumnIndex + 1;
				int count = base.Count;
				while (flag && num2 < count)
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(num2);
					if (DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn))
					{
						DataGridLength width = dataGridColumn.Width;
						double num3 = expandBeyondDesiredWidth ? dataGridColumn.MaxWidth : Math.Min(width.DesiredValue, dataGridColumn.MaxWidth);
						if (!width.IsStar && DoubleUtil.LessThan(width.DisplayValue, num3))
						{
							double num4 = num3 - width.DisplayValue;
							if (DoubleUtil.GreaterThanOrClose(num + num4, horizontalChange))
							{
								num4 = horizontalChange - num;
								flag = false;
							}
							dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue + num4));
							num += num4;
						}
					}
					num2++;
				}
				if (DoubleUtil.GreaterThan(num, 0.0))
				{
					DataGridColumnCollection.SetResizedColumnWidth(this.ColumnFromDisplayIndex(resizingColumnIndex), -num, retainAuto);
					horizontalChange -= num;
				}
			}
			return horizontalChange;
		}

		// Token: 0x060065AF RID: 26031 RVA: 0x002AF28C File Offset: 0x002AE28C
		private double RecomputeStarColumnWidthsOnColumnNegativeResize(double horizontalChange, int resizingColumnIndex, double perStarWidth, bool retainAuto)
		{
			while (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				double positiveInfinity = double.PositiveInfinity;
				double starFactorsForNegativeResize = this.GetStarFactorsForNegativeResize(resizingColumnIndex + 1, out positiveInfinity);
				if (!DoubleUtil.GreaterThan(starFactorsForNegativeResize, 0.0))
				{
					break;
				}
				horizontalChange = this.ReallocateStarValuesForNegativeResize(resizingColumnIndex, horizontalChange, positiveInfinity, starFactorsForNegativeResize, perStarWidth, retainAuto);
				if (DoubleUtil.AreClose(horizontalChange, 0.0))
				{
					break;
				}
			}
			return horizontalChange;
		}

		// Token: 0x060065B0 RID: 26032 RVA: 0x002AF2F4 File Offset: 0x002AE2F4
		private double GetStarFactorsForNegativeResize(int startIndex, out double minPerStarLagRatio)
		{
			minPerStarLagRatio = double.PositiveInfinity;
			double num = 0.0;
			int i = startIndex;
			int count = base.Count;
			while (i < count)
			{
				DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
				if (DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn))
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar && !DoubleUtil.AreClose(width.Value, 0.0) && DoubleUtil.LessThan(width.DisplayValue, dataGridColumn.MaxWidth))
					{
						num += width.Value;
						double num2 = (dataGridColumn.MaxWidth - width.DisplayValue) / width.Value;
						if (DoubleUtil.LessThan(num2, minPerStarLagRatio))
						{
							minPerStarLagRatio = num2;
						}
					}
				}
				i++;
			}
			return num;
		}

		// Token: 0x060065B1 RID: 26033 RVA: 0x002AF3B0 File Offset: 0x002AE3B0
		private double ReallocateStarValuesForNegativeResize(int startIndex, double horizontalChange, double perStarLagRatio, double totalStarFactors, double perStarWidth, bool retainAuto)
		{
			double num;
			double num2;
			if (DoubleUtil.LessThan(horizontalChange, perStarLagRatio * totalStarFactors))
			{
				num = horizontalChange / totalStarFactors;
				num2 = horizontalChange;
				horizontalChange = 0.0;
			}
			else
			{
				num = perStarLagRatio;
				num2 = num * totalStarFactors;
				horizontalChange -= num2;
			}
			int i = startIndex;
			int count = base.Count;
			while (i < count)
			{
				DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
				DataGridLength width = dataGridColumn.Width;
				if (i == startIndex)
				{
					DataGridColumnCollection.SetResizedColumnWidth(dataGridColumn, -num2, retainAuto);
				}
				else if (dataGridColumn.Width.IsStar && DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn) && DoubleUtil.LessThan(width.DisplayValue, dataGridColumn.MaxWidth))
				{
					double num3 = width.DisplayValue + width.Value * num;
					dataGridColumn.UpdateWidthForStarColumn(Math.Min(num3, dataGridColumn.MaxWidth), num3, num3 / perStarWidth);
				}
				i++;
			}
			return horizontalChange;
		}

		// Token: 0x060065B2 RID: 26034 RVA: 0x002AF49C File Offset: 0x002AE49C
		private static void SetResizedColumnWidth(DataGridColumn column, double widthDelta, bool retainAuto)
		{
			DataGridLength width = column.Width;
			double num = DataGridHelper.CoerceToMinMax(width.DisplayValue + widthDelta, column.MinWidth, column.MaxWidth);
			if (width.IsStar)
			{
				double num2 = width.DesiredValue / width.Value;
				column.UpdateWidthForStarColumn(num, num, num / num2);
				return;
			}
			if (!width.IsAbsolute && retainAuto)
			{
				column.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, num));
				return;
			}
			column.SetWidthInternal(new DataGridLength(num, DataGridLengthUnitType.Pixel, num, num));
		}

		// Token: 0x060065B3 RID: 26035 RVA: 0x002AF52F File Offset: 0x002AE52F
		private double GiveAwayWidthToColumns(DataGridColumn ignoredColumn, double giveAwayWidth)
		{
			return this.GiveAwayWidthToColumns(ignoredColumn, giveAwayWidth, false);
		}

		// Token: 0x060065B4 RID: 26036 RVA: 0x002AF53C File Offset: 0x002AE53C
		private double GiveAwayWidthToColumns(DataGridColumn ignoredColumn, double giveAwayWidth, bool recomputeStars)
		{
			double num = giveAwayWidth;
			giveAwayWidth = this.GiveAwayWidthToScrollViewerExcess(giveAwayWidth, ignoredColumn != null);
			giveAwayWidth = this.GiveAwayWidthToNonStarColumns(ignoredColumn, giveAwayWidth);
			if (DoubleUtil.GreaterThan(giveAwayWidth, 0.0) || recomputeStars)
			{
				double num2 = 0.0;
				double num3 = 0.0;
				bool flag = false;
				foreach (DataGridColumn dataGridColumn in this)
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar && dataGridColumn.IsVisible)
					{
						if (dataGridColumn == ignoredColumn)
						{
							flag = true;
						}
						num2 += width.DisplayValue;
						num3 += dataGridColumn.MaxWidth;
					}
				}
				double num4 = num2;
				if (!flag)
				{
					num4 += giveAwayWidth;
				}
				else if (!DoubleUtil.AreClose(num, giveAwayWidth))
				{
					num4 -= num - giveAwayWidth;
				}
				giveAwayWidth = Math.Max(this.ComputeStarColumnWidths(Math.Min(num4, num3)) - num4, 0.0);
			}
			return giveAwayWidth;
		}

		// Token: 0x060065B5 RID: 26037 RVA: 0x002AF640 File Offset: 0x002AE640
		private double GiveAwayWidthToNonStarColumns(DataGridColumn ignoredColumn, double giveAwayWidth)
		{
			while (DoubleUtil.GreaterThan(giveAwayWidth, 0.0))
			{
				int num = 0;
				double num2 = this.FindMinimumLaggingWidthOfNonStarColumns(ignoredColumn, out num);
				if (num == 0)
				{
					break;
				}
				double num3 = num2 * (double)num;
				if (DoubleUtil.GreaterThanOrClose(num3, giveAwayWidth))
				{
					num2 = giveAwayWidth / (double)num;
					giveAwayWidth = 0.0;
				}
				else
				{
					giveAwayWidth -= num3;
				}
				this.GiveAwayWidthToEveryNonStarColumn(ignoredColumn, num2);
			}
			return giveAwayWidth;
		}

		// Token: 0x060065B6 RID: 26038 RVA: 0x002AF6A0 File Offset: 0x002AE6A0
		private double FindMinimumLaggingWidthOfNonStarColumns(DataGridColumn ignoredColumn, out int countOfParticipatingColumns)
		{
			double num = double.PositiveInfinity;
			countOfParticipatingColumns = 0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (ignoredColumn != dataGridColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar)
					{
						double maxWidth = dataGridColumn.MaxWidth;
						if (DoubleUtil.LessThan(width.DisplayValue, width.DesiredValue) && !DoubleUtil.AreClose(width.DisplayValue, maxWidth))
						{
							countOfParticipatingColumns++;
							double num2 = Math.Min(width.DesiredValue, maxWidth) - width.DisplayValue;
							if (DoubleUtil.LessThan(num2, num))
							{
								num = num2;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060065B7 RID: 26039 RVA: 0x002AF768 File Offset: 0x002AE768
		private void GiveAwayWidthToEveryNonStarColumn(DataGridColumn ignoredColumn, double perColumnGiveAwayWidth)
		{
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (ignoredColumn != dataGridColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar && DoubleUtil.LessThan(width.DisplayValue, Math.Min(width.DesiredValue, dataGridColumn.MaxWidth)))
					{
						dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue + perColumnGiveAwayWidth));
					}
				}
			}
		}

		// Token: 0x060065B8 RID: 26040 RVA: 0x002AF810 File Offset: 0x002AE810
		private double GiveAwayWidthToScrollViewerExcess(double giveAwayWidth, bool includedInColumnsWidth)
		{
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			double num = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					num += dataGridColumn.Width.DisplayValue;
				}
			}
			if (includedInColumnsWidth)
			{
				if (DoubleUtil.GreaterThan(num, viewportWidthForColumns))
				{
					double val = num - viewportWidthForColumns;
					giveAwayWidth -= Math.Min(val, giveAwayWidth);
				}
			}
			else
			{
				giveAwayWidth = Math.Min(giveAwayWidth, Math.Max(0.0, viewportWidthForColumns - num));
			}
			return giveAwayWidth;
		}

		// Token: 0x060065B9 RID: 26041 RVA: 0x002AF8BC File Offset: 0x002AE8BC
		private double TakeAwayUnusedSpaceOnColumnPositiveResize(double horizontalChange, int resizingColumnIndex, bool retainAuto)
		{
			double num = this.TakeAwayWidthFromUnusedSpace(false, horizontalChange);
			if (DoubleUtil.LessThan(num, horizontalChange))
			{
				DataGridColumnCollection.SetResizedColumnWidth(this.ColumnFromDisplayIndex(resizingColumnIndex), horizontalChange - num, retainAuto);
			}
			return num;
		}

		// Token: 0x060065BA RID: 26042 RVA: 0x002AF8EC File Offset: 0x002AE8EC
		private double TakeAwayWidthFromUnusedSpace(bool spaceAlreadyUtilized, double takeAwayWidth, double totalAvailableWidth)
		{
			double num = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					num += dataGridColumn.Width.DisplayValue;
				}
			}
			if (!spaceAlreadyUtilized)
			{
				double num2 = totalAvailableWidth - num;
				if (DoubleUtil.GreaterThan(num2, 0.0))
				{
					takeAwayWidth = Math.Max(0.0, takeAwayWidth - num2);
				}
				return takeAwayWidth;
			}
			if (DoubleUtil.GreaterThanOrClose(totalAvailableWidth, num))
			{
				return 0.0;
			}
			return Math.Min(num - totalAvailableWidth, takeAwayWidth);
		}

		// Token: 0x060065BB RID: 26043 RVA: 0x002AF99C File Offset: 0x002AE99C
		private double TakeAwayWidthFromUnusedSpace(bool spaceAlreadyUtilized, double takeAwayWidth)
		{
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			if (DoubleUtil.GreaterThan(viewportWidthForColumns, 0.0))
			{
				return this.TakeAwayWidthFromUnusedSpace(spaceAlreadyUtilized, takeAwayWidth, viewportWidthForColumns);
			}
			return takeAwayWidth;
		}

		// Token: 0x060065BC RID: 26044 RVA: 0x002AF9D4 File Offset: 0x002AE9D4
		private double TakeAwayWidthFromColumns(DataGridColumn ignoredColumn, double takeAwayWidth, bool widthAlreadyUtilized)
		{
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			return this.TakeAwayWidthFromColumns(ignoredColumn, takeAwayWidth, widthAlreadyUtilized, viewportWidthForColumns);
		}

		// Token: 0x060065BD RID: 26045 RVA: 0x002AF9F7 File Offset: 0x002AE9F7
		private double TakeAwayWidthFromColumns(DataGridColumn ignoredColumn, double takeAwayWidth, bool widthAlreadyUtilized, double totalAvailableWidth)
		{
			takeAwayWidth = this.TakeAwayWidthFromUnusedSpace(widthAlreadyUtilized, takeAwayWidth, totalAvailableWidth);
			takeAwayWidth = this.TakeAwayWidthFromStarColumns(ignoredColumn, takeAwayWidth);
			takeAwayWidth = this.TakeAwayWidthFromNonStarColumns(ignoredColumn, takeAwayWidth);
			return takeAwayWidth;
		}

		// Token: 0x060065BE RID: 26046 RVA: 0x002AFA1C File Offset: 0x002AEA1C
		private double TakeAwayWidthFromStarColumns(DataGridColumn ignoredColumn, double takeAwayWidth)
		{
			if (DoubleUtil.GreaterThan(takeAwayWidth, 0.0))
			{
				double num = 0.0;
				double num2 = 0.0;
				foreach (DataGridColumn dataGridColumn in this)
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar && dataGridColumn.IsVisible)
					{
						if (dataGridColumn == ignoredColumn)
						{
							num += takeAwayWidth;
						}
						num += width.DisplayValue;
						num2 += dataGridColumn.MinWidth;
					}
				}
				double num3 = num - takeAwayWidth;
				takeAwayWidth = Math.Max(this.ComputeStarColumnWidths(Math.Max(num3, num2)) - num3, 0.0);
			}
			return takeAwayWidth;
		}

		// Token: 0x060065BF RID: 26047 RVA: 0x002AFAE4 File Offset: 0x002AEAE4
		private double TakeAwayWidthFromNonStarColumns(DataGridColumn ignoredColumn, double takeAwayWidth)
		{
			while (DoubleUtil.GreaterThan(takeAwayWidth, 0.0))
			{
				int num = 0;
				double num2 = this.FindMinimumExcessWidthOfNonStarColumns(ignoredColumn, out num);
				if (num == 0)
				{
					break;
				}
				double num3 = num2 * (double)num;
				if (DoubleUtil.GreaterThanOrClose(num3, takeAwayWidth))
				{
					num2 = takeAwayWidth / (double)num;
					takeAwayWidth = 0.0;
				}
				else
				{
					takeAwayWidth -= num3;
				}
				this.TakeAwayWidthFromEveryNonStarColumn(ignoredColumn, num2);
			}
			return takeAwayWidth;
		}

		// Token: 0x060065C0 RID: 26048 RVA: 0x002AFB44 File Offset: 0x002AEB44
		private double FindMinimumExcessWidthOfNonStarColumns(DataGridColumn ignoredColumn, out int countOfParticipatingColumns)
		{
			double num = double.PositiveInfinity;
			countOfParticipatingColumns = 0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (ignoredColumn != dataGridColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar)
					{
						double minWidth = dataGridColumn.MinWidth;
						if (DoubleUtil.GreaterThan(width.DisplayValue, minWidth))
						{
							countOfParticipatingColumns++;
							double num2 = width.DisplayValue - minWidth;
							if (DoubleUtil.LessThan(num2, num))
							{
								num = num2;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060065C1 RID: 26049 RVA: 0x002AFBE8 File Offset: 0x002AEBE8
		private void TakeAwayWidthFromEveryNonStarColumn(DataGridColumn ignoredColumn, double perColumnTakeAwayWidth)
		{
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (ignoredColumn != dataGridColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar && DoubleUtil.GreaterThan(width.DisplayValue, dataGridColumn.MinWidth))
					{
						dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue - perColumnTakeAwayWidth));
					}
				}
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x060065C2 RID: 26050 RVA: 0x002AFC84 File Offset: 0x002AEC84
		// (set) Token: 0x060065C3 RID: 26051 RVA: 0x002AFC8C File Offset: 0x002AEC8C
		internal bool RebuildRealizedColumnsBlockListForNonVirtualizedRows { get; set; }

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x060065C4 RID: 26052 RVA: 0x002AFC95 File Offset: 0x002AEC95
		// (set) Token: 0x060065C5 RID: 26053 RVA: 0x002AFCA0 File Offset: 0x002AECA0
		internal List<RealizedColumnsBlock> RealizedColumnsBlockListForNonVirtualizedRows
		{
			get
			{
				return this._realizedColumnsBlockListForNonVirtualizedRows;
			}
			set
			{
				this._realizedColumnsBlockListForNonVirtualizedRows = value;
				DataGrid dataGridOwner = this.DataGridOwner;
				dataGridOwner.NotifyPropertyChanged(dataGridOwner, "RealizedColumnsBlockListForNonVirtualizedRows", default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnHeadersPresenter);
			}
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x060065C6 RID: 26054 RVA: 0x002AFCD0 File Offset: 0x002AECD0
		// (set) Token: 0x060065C7 RID: 26055 RVA: 0x002AFCD8 File Offset: 0x002AECD8
		internal List<RealizedColumnsBlock> RealizedColumnsDisplayIndexBlockListForNonVirtualizedRows { get; set; }

		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x060065C8 RID: 26056 RVA: 0x002AFCE1 File Offset: 0x002AECE1
		// (set) Token: 0x060065C9 RID: 26057 RVA: 0x002AFCE9 File Offset: 0x002AECE9
		internal bool RebuildRealizedColumnsBlockListForVirtualizedRows { get; set; }

		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x060065CA RID: 26058 RVA: 0x002AFCF2 File Offset: 0x002AECF2
		// (set) Token: 0x060065CB RID: 26059 RVA: 0x002AFCFC File Offset: 0x002AECFC
		internal List<RealizedColumnsBlock> RealizedColumnsBlockListForVirtualizedRows
		{
			get
			{
				return this._realizedColumnsBlockListForVirtualizedRows;
			}
			set
			{
				this._realizedColumnsBlockListForVirtualizedRows = value;
				DataGrid dataGridOwner = this.DataGridOwner;
				dataGridOwner.NotifyPropertyChanged(dataGridOwner, "RealizedColumnsBlockListForVirtualizedRows", default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnHeadersPresenter);
			}
		}

		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x060065CC RID: 26060 RVA: 0x002AFD2C File Offset: 0x002AED2C
		// (set) Token: 0x060065CD RID: 26061 RVA: 0x002AFD34 File Offset: 0x002AED34
		internal List<RealizedColumnsBlock> RealizedColumnsDisplayIndexBlockListForVirtualizedRows { get; set; }

		// Token: 0x060065CE RID: 26062 RVA: 0x002AFD3D File Offset: 0x002AED3D
		internal void InvalidateColumnRealization(bool invalidateForNonVirtualizedRows)
		{
			this.RebuildRealizedColumnsBlockListForVirtualizedRows = true;
			if (invalidateForNonVirtualizedRows)
			{
				this.RebuildRealizedColumnsBlockListForNonVirtualizedRows = true;
			}
		}

		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x060065CF RID: 26063 RVA: 0x002AFD50 File Offset: 0x002AED50
		internal int FirstVisibleDisplayIndex
		{
			get
			{
				int i = 0;
				int count = base.Count;
				while (i < count)
				{
					if (this.ColumnFromDisplayIndex(i).IsVisible)
					{
						return i;
					}
					i++;
				}
				return -1;
			}
		}

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x060065D0 RID: 26064 RVA: 0x002AFD84 File Offset: 0x002AED84
		internal int LastVisibleDisplayIndex
		{
			get
			{
				for (int i = base.Count - 1; i >= 0; i--)
				{
					if (this.ColumnFromDisplayIndex(i).IsVisible)
					{
						return i;
					}
				}
				return -1;
			}
		}

		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x060065D1 RID: 26065 RVA: 0x002AFDB5 File Offset: 0x002AEDB5
		// (set) Token: 0x060065D2 RID: 26066 RVA: 0x002AFDBD File Offset: 0x002AEDBD
		internal bool RefreshAutoWidthColumns { get; set; }

		// Token: 0x0400338C RID: 13196
		private DataGrid _dataGridOwner;

		// Token: 0x0400338D RID: 13197
		private bool _isUpdatingDisplayIndex;

		// Token: 0x0400338E RID: 13198
		private List<int> _displayIndexMap;

		// Token: 0x0400338F RID: 13199
		private bool _displayIndexMapInitialized;

		// Token: 0x04003390 RID: 13200
		private bool _isClearingDisplayIndex;

		// Token: 0x04003391 RID: 13201
		private bool _columnWidthsComputationPending;

		// Token: 0x04003392 RID: 13202
		private Dictionary<DataGridColumn, DataGridLength> _originalWidthsForResize;

		// Token: 0x04003393 RID: 13203
		private double? _averageColumnWidth;

		// Token: 0x04003394 RID: 13204
		private List<RealizedColumnsBlock> _realizedColumnsBlockListForNonVirtualizedRows;

		// Token: 0x04003395 RID: 13205
		private List<RealizedColumnsBlock> _realizedColumnsBlockListForVirtualizedRows;

		// Token: 0x04003396 RID: 13206
		private bool _hasVisibleStarColumns;
	}
}
