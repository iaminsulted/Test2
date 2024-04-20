using System;

namespace System.Windows.Controls
{
	// Token: 0x02000745 RID: 1861
	public struct DataGridCellInfo
	{
		// Token: 0x0600648B RID: 25739 RVA: 0x002A8D6E File Offset: 0x002A7D6E
		public DataGridCellInfo(object item, DataGridColumn column)
		{
			if (column == null)
			{
				throw new ArgumentNullException("column");
			}
			this._info = new ItemsControl.ItemInfo(item, null, -1);
			this._column = column;
			this._owner = null;
		}

		// Token: 0x0600648C RID: 25740 RVA: 0x002A8D9C File Offset: 0x002A7D9C
		public DataGridCellInfo(DataGridCell cell)
		{
			if (cell == null)
			{
				throw new ArgumentNullException("cell");
			}
			DataGrid dataGridOwner = cell.DataGridOwner;
			this._info = dataGridOwner.NewItemInfo(cell.RowDataItem, cell.RowOwner, -1);
			this._column = cell.Column;
			this._owner = new WeakReference(dataGridOwner);
		}

		// Token: 0x0600648D RID: 25741 RVA: 0x002A8DEF File Offset: 0x002A7DEF
		internal DataGridCellInfo(object item, DataGridColumn column, DataGrid owner)
		{
			this._info = owner.NewItemInfo(item, null, -1);
			this._column = column;
			this._owner = new WeakReference(owner);
		}

		// Token: 0x0600648E RID: 25742 RVA: 0x002A8E13 File Offset: 0x002A7E13
		internal DataGridCellInfo(ItemsControl.ItemInfo info, DataGridColumn column, DataGrid owner)
		{
			this._info = info;
			this._column = column;
			this._owner = new WeakReference(owner);
		}

		// Token: 0x0600648F RID: 25743 RVA: 0x002A8E2F File Offset: 0x002A7E2F
		internal DataGridCellInfo(object item)
		{
			this._info = new ItemsControl.ItemInfo(item, null, -1);
			this._column = null;
			this._owner = null;
		}

		// Token: 0x06006490 RID: 25744 RVA: 0x002A8E4D File Offset: 0x002A7E4D
		internal DataGridCellInfo(DataGridCellInfo info)
		{
			this._info = info._info.Clone();
			this._column = info._column;
			this._owner = info._owner;
		}

		// Token: 0x06006491 RID: 25745 RVA: 0x002A8E78 File Offset: 0x002A7E78
		private DataGridCellInfo(DataGrid owner, DataGridColumn column, object item)
		{
			this._info = owner.NewItemInfo(item, null, -1);
			this._column = column;
			this._owner = new WeakReference(owner);
		}

		// Token: 0x06006492 RID: 25746 RVA: 0x002A8E9C File Offset: 0x002A7E9C
		internal static DataGridCellInfo CreatePossiblyPartialCellInfo(object item, DataGridColumn column, DataGrid owner)
		{
			if (item == null && column == null)
			{
				return DataGridCellInfo.Unset;
			}
			return new DataGridCellInfo(owner, column, (item == null) ? DependencyProperty.UnsetValue : item);
		}

		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x06006493 RID: 25747 RVA: 0x002A8EBC File Offset: 0x002A7EBC
		public object Item
		{
			get
			{
				if (!(this._info != null))
				{
					return null;
				}
				return this._info.Item;
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x06006494 RID: 25748 RVA: 0x002A8ED9 File Offset: 0x002A7ED9
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x06006495 RID: 25749 RVA: 0x002A8EE1 File Offset: 0x002A7EE1
		public override bool Equals(object obj)
		{
			return obj is DataGridCellInfo && this.EqualsImpl((DataGridCellInfo)obj);
		}

		// Token: 0x06006496 RID: 25750 RVA: 0x002A8EF9 File Offset: 0x002A7EF9
		public static bool operator ==(DataGridCellInfo cell1, DataGridCellInfo cell2)
		{
			return cell1.EqualsImpl(cell2);
		}

		// Token: 0x06006497 RID: 25751 RVA: 0x002A8F03 File Offset: 0x002A7F03
		public static bool operator !=(DataGridCellInfo cell1, DataGridCellInfo cell2)
		{
			return !cell1.EqualsImpl(cell2);
		}

		// Token: 0x06006498 RID: 25752 RVA: 0x002A8F10 File Offset: 0x002A7F10
		internal bool EqualsImpl(DataGridCellInfo cell)
		{
			return cell._column == this._column && cell.Owner == this.Owner && cell._info == this._info;
		}

		// Token: 0x06006499 RID: 25753 RVA: 0x002A8F42 File Offset: 0x002A7F42
		public override int GetHashCode()
		{
			return ((this._info == null) ? 0 : this._info.GetHashCode()) ^ ((this._column == null) ? 0 : this._column.GetHashCode());
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x0600649A RID: 25754 RVA: 0x002A8F77 File Offset: 0x002A7F77
		public bool IsValid
		{
			get
			{
				return this.ArePropertyValuesValid;
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x0600649B RID: 25755 RVA: 0x002A8F7F File Offset: 0x002A7F7F
		internal bool IsSet
		{
			get
			{
				return this._column != null || this._info.Item != DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x0600649C RID: 25756 RVA: 0x002A8FA0 File Offset: 0x002A7FA0
		internal ItemsControl.ItemInfo ItemInfo
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x0600649D RID: 25757 RVA: 0x002A8FA8 File Offset: 0x002A7FA8
		internal bool IsValidForDataGrid(DataGrid dataGrid)
		{
			DataGrid owner = this.Owner;
			return (this.ArePropertyValuesValid && owner == dataGrid) || owner == null;
		}

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x0600649E RID: 25758 RVA: 0x002A8FCE File Offset: 0x002A7FCE
		private bool ArePropertyValuesValid
		{
			get
			{
				return this.Item != DependencyProperty.UnsetValue && this._column != null;
			}
		}

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x0600649F RID: 25759 RVA: 0x002A8FE8 File Offset: 0x002A7FE8
		internal static DataGridCellInfo Unset
		{
			get
			{
				return new DataGridCellInfo(DependencyProperty.UnsetValue);
			}
		}

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x060064A0 RID: 25760 RVA: 0x002A8FF4 File Offset: 0x002A7FF4
		private DataGrid Owner
		{
			get
			{
				if (this._owner != null)
				{
					return (DataGrid)this._owner.Target;
				}
				return null;
			}
		}

		// Token: 0x04003351 RID: 13137
		private ItemsControl.ItemInfo _info;

		// Token: 0x04003352 RID: 13138
		private DataGridColumn _column;

		// Token: 0x04003353 RID: 13139
		private WeakReference _owner;
	}
}
