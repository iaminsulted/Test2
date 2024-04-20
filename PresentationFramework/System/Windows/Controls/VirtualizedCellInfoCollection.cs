using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x02000807 RID: 2055
	internal class VirtualizedCellInfoCollection : IList<DataGridCellInfo>, ICollection<DataGridCellInfo>, IEnumerable<DataGridCellInfo>, IEnumerable
	{
		// Token: 0x06007738 RID: 30520 RVA: 0x002F18E7 File Offset: 0x002F08E7
		internal VirtualizedCellInfoCollection(DataGrid owner)
		{
			this._owner = owner;
			this._regions = new List<VirtualizedCellInfoCollection.CellRegion>();
		}

		// Token: 0x06007739 RID: 30521 RVA: 0x002F1901 File Offset: 0x002F0901
		private VirtualizedCellInfoCollection(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions)
		{
			this._owner = owner;
			this._regions = ((regions != null) ? new List<VirtualizedCellInfoCollection.CellRegion>(regions) : new List<VirtualizedCellInfoCollection.CellRegion>());
			this.IsReadOnly = true;
		}

		// Token: 0x0600773A RID: 30522 RVA: 0x002F192D File Offset: 0x002F092D
		internal static VirtualizedCellInfoCollection MakeEmptyCollection(DataGrid owner)
		{
			return new VirtualizedCellInfoCollection(owner, null);
		}

		// Token: 0x0600773B RID: 30523 RVA: 0x002F1938 File Offset: 0x002F0938
		public void Add(DataGridCellInfo cell)
		{
			this._owner.Dispatcher.VerifyAccess();
			this.ValidateIsReadOnly();
			if (!this.IsValidPublicCell(cell))
			{
				throw new ArgumentException(SR.Get("SelectedCellsCollection_InvalidItem"), "cell");
			}
			if (this.Contains(cell))
			{
				throw new ArgumentException(SR.Get("SelectedCellsCollection_DuplicateItem"), "cell");
			}
			this.AddValidatedCell(cell);
		}

		// Token: 0x0600773C RID: 30524 RVA: 0x002F19A0 File Offset: 0x002F09A0
		internal void AddValidatedCell(DataGridCellInfo cell)
		{
			int rowIndex;
			int columnIndex;
			this.ConvertCellInfoToIndexes(cell, out rowIndex, out columnIndex);
			this.AddRegion(rowIndex, columnIndex, 1, 1);
		}

		// Token: 0x0600773D RID: 30525 RVA: 0x002F19C4 File Offset: 0x002F09C4
		public void Clear()
		{
			this._owner.Dispatcher.VerifyAccess();
			this.ValidateIsReadOnly();
			if (!this.IsEmpty)
			{
				VirtualizedCellInfoCollection oldItems = new VirtualizedCellInfoCollection(this._owner, this._regions);
				this._regions.Clear();
				this.OnRemove(oldItems);
			}
		}

		// Token: 0x0600773E RID: 30526 RVA: 0x002F1A14 File Offset: 0x002F0A14
		public bool Contains(DataGridCellInfo cell)
		{
			if (!this.IsValidPublicCell(cell))
			{
				throw new ArgumentException(SR.Get("SelectedCellsCollection_InvalidItem"), "cell");
			}
			if (this.IsEmpty)
			{
				return false;
			}
			int rowIndex;
			int columnIndex;
			this.ConvertCellInfoToIndexes(cell, out rowIndex, out columnIndex);
			return this.Contains(rowIndex, columnIndex);
		}

		// Token: 0x0600773F RID: 30527 RVA: 0x002F1A5C File Offset: 0x002F0A5C
		internal bool Contains(DataGridCell cell)
		{
			if (!this.IsEmpty)
			{
				object rowDataItem = cell.RowDataItem;
				int displayIndex = cell.Column.DisplayIndex;
				ItemCollection items = this._owner.Items;
				int count = items.Count;
				int count2 = this._regions.Count;
				for (int i = 0; i < count2; i++)
				{
					VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
					if (cellRegion.Left <= displayIndex && displayIndex <= cellRegion.Right)
					{
						int bottom = cellRegion.Bottom;
						for (int j = cellRegion.Top; j <= bottom; j++)
						{
							if (j < count && items[j] == rowDataItem)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007740 RID: 30528 RVA: 0x002F1B10 File Offset: 0x002F0B10
		internal bool Contains(int rowIndex, int columnIndex)
		{
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this._regions[i].Contains(columnIndex, rowIndex))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007741 RID: 30529 RVA: 0x002F1B50 File Offset: 0x002F0B50
		public void CopyTo(DataGridCellInfo[] array, int arrayIndex)
		{
			List<DataGridCellInfo> list = new List<DataGridCellInfo>();
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				this.AddRegionToList(this._regions[i], list);
			}
			list.CopyTo(array, arrayIndex);
		}

		// Token: 0x06007742 RID: 30530 RVA: 0x002F1B96 File Offset: 0x002F0B96
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new VirtualizedCellInfoCollection.VirtualizedCellInfoCollectionEnumerator(this._owner, this._regions, this);
		}

		// Token: 0x06007743 RID: 30531 RVA: 0x002F1B96 File Offset: 0x002F0B96
		public IEnumerator<DataGridCellInfo> GetEnumerator()
		{
			return new VirtualizedCellInfoCollection.VirtualizedCellInfoCollectionEnumerator(this._owner, this._regions, this);
		}

		// Token: 0x06007744 RID: 30532 RVA: 0x002F1BAC File Offset: 0x002F0BAC
		public int IndexOf(DataGridCellInfo cell)
		{
			int num;
			int num2;
			this.ConvertCellInfoToIndexes(cell, out num, out num2);
			int count = this._regions.Count;
			int num3 = 0;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
				if (cellRegion.Contains(num2, num))
				{
					return num3 + ((num - cellRegion.Top) * cellRegion.Width + num2 - cellRegion.Left);
				}
				num3 += cellRegion.Size;
			}
			return -1;
		}

		// Token: 0x06007745 RID: 30533 RVA: 0x002F1C25 File Offset: 0x002F0C25
		public void Insert(int index, DataGridCellInfo cell)
		{
			throw new NotSupportedException(SR.Get("VirtualizedCellInfoCollection_DoesNotSupportIndexChanges"));
		}

		// Token: 0x06007746 RID: 30534 RVA: 0x002F1C38 File Offset: 0x002F0C38
		public bool Remove(DataGridCellInfo cell)
		{
			this._owner.Dispatcher.VerifyAccess();
			this.ValidateIsReadOnly();
			if (!this.IsEmpty)
			{
				int rowIndex;
				int columnIndex;
				this.ConvertCellInfoToIndexes(cell, out rowIndex, out columnIndex);
				if (this.Contains(rowIndex, columnIndex))
				{
					this.RemoveRegion(rowIndex, columnIndex, 1, 1);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007747 RID: 30535 RVA: 0x002F1C25 File Offset: 0x002F0C25
		public void RemoveAt(int index)
		{
			throw new NotSupportedException(SR.Get("VirtualizedCellInfoCollection_DoesNotSupportIndexChanges"));
		}

		// Token: 0x17001BB0 RID: 7088
		public DataGridCellInfo this[int index]
		{
			get
			{
				if (index >= 0 && index < this.Count)
				{
					return this.GetCellInfoFromIndex(this._owner, this._regions, index);
				}
				throw new ArgumentOutOfRangeException("index");
			}
			set
			{
				throw new NotSupportedException(SR.Get("VirtualizedCellInfoCollection_DoesNotSupportIndexChanges"));
			}
		}

		// Token: 0x17001BB1 RID: 7089
		// (get) Token: 0x0600774A RID: 30538 RVA: 0x002F1CB4 File Offset: 0x002F0CB4
		public int Count
		{
			get
			{
				int num = 0;
				int count = this._regions.Count;
				for (int i = 0; i < count; i++)
				{
					num += this._regions[i].Size;
				}
				return num;
			}
		}

		// Token: 0x17001BB2 RID: 7090
		// (get) Token: 0x0600774B RID: 30539 RVA: 0x002F1CF3 File Offset: 0x002F0CF3
		// (set) Token: 0x0600774C RID: 30540 RVA: 0x002F1CFB File Offset: 0x002F0CFB
		public bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
			private set
			{
				this._isReadOnly = value;
			}
		}

		// Token: 0x0600774D RID: 30541 RVA: 0x002F1D04 File Offset: 0x002F0D04
		private void OnAdd(VirtualizedCellInfoCollection newItems)
		{
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, null, newItems);
		}

		// Token: 0x0600774E RID: 30542 RVA: 0x002F1D0F File Offset: 0x002F0D0F
		private void OnRemove(VirtualizedCellInfoCollection oldItems)
		{
			this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, oldItems, null);
		}

		// Token: 0x0600774F RID: 30543 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, VirtualizedCellInfoCollection oldItems, VirtualizedCellInfoCollection newItems)
		{
		}

		// Token: 0x06007750 RID: 30544 RVA: 0x002F1D1A File Offset: 0x002F0D1A
		private bool IsValidCell(DataGridCellInfo cell)
		{
			return cell.IsValidForDataGrid(this._owner);
		}

		// Token: 0x06007751 RID: 30545 RVA: 0x002F1D29 File Offset: 0x002F0D29
		private bool IsValidPublicCell(DataGridCellInfo cell)
		{
			return cell.IsValidForDataGrid(this._owner) && cell.IsValid;
		}

		// Token: 0x17001BB3 RID: 7091
		// (get) Token: 0x06007752 RID: 30546 RVA: 0x002F1D44 File Offset: 0x002F0D44
		protected bool IsEmpty
		{
			get
			{
				int count = this._regions.Count;
				for (int i = 0; i < count; i++)
				{
					if (!this._regions[i].IsEmpty)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06007753 RID: 30547 RVA: 0x002F1D84 File Offset: 0x002F0D84
		protected void GetBoundingRegion(out int left, out int top, out int right, out int bottom)
		{
			left = int.MaxValue;
			top = int.MaxValue;
			right = 0;
			bottom = 0;
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
				if (cellRegion.Left < left)
				{
					left = cellRegion.Left;
				}
				if (cellRegion.Top < top)
				{
					top = cellRegion.Top;
				}
				if (cellRegion.Right > right)
				{
					right = cellRegion.Right;
				}
				if (cellRegion.Bottom > bottom)
				{
					bottom = cellRegion.Bottom;
				}
			}
		}

		// Token: 0x06007754 RID: 30548 RVA: 0x002F1E1D File Offset: 0x002F0E1D
		internal void AddRegion(int rowIndex, int columnIndex, int rowCount, int columnCount)
		{
			this.AddRegion(rowIndex, columnIndex, rowCount, columnCount, true);
		}

		// Token: 0x06007755 RID: 30549 RVA: 0x002F1E2C File Offset: 0x002F0E2C
		private void AddRegion(int rowIndex, int columnIndex, int rowCount, int columnCount, bool notify)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = new List<VirtualizedCellInfoCollection.CellRegion>();
			list.Add(new VirtualizedCellInfoCollection.CellRegion(columnIndex, rowIndex, columnCount, rowCount));
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion region = this._regions[i];
				for (int j = 0; j < list.Count; j++)
				{
					List<VirtualizedCellInfoCollection.CellRegion> list2;
					if (list[j].Remainder(region, out list2))
					{
						list.RemoveAt(j);
						j--;
						if (list2 != null)
						{
							list.AddRange(list2);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				VirtualizedCellInfoCollection newItems = new VirtualizedCellInfoCollection(this._owner, list);
				for (int k = 0; k < count; k++)
				{
					for (int l = 0; l < list.Count; l++)
					{
						VirtualizedCellInfoCollection.CellRegion value = this._regions[k];
						if (value.Union(list[l]))
						{
							this._regions[k] = value;
							list.RemoveAt(l);
							l--;
						}
					}
				}
				int count2 = list.Count;
				for (int m = 0; m < count2; m++)
				{
					this._regions.Add(list[m]);
				}
				if (notify)
				{
					this.OnAdd(newItems);
				}
			}
		}

		// Token: 0x06007756 RID: 30550 RVA: 0x002F1F70 File Offset: 0x002F0F70
		internal void RemoveRegion(int rowIndex, int columnIndex, int rowCount, int columnCount)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			this.RemoveRegion(rowIndex, columnIndex, rowCount, columnCount, ref list);
			if (list != null && list.Count > 0)
			{
				this.OnRemove(new VirtualizedCellInfoCollection(this._owner, list));
			}
		}

		// Token: 0x06007757 RID: 30551 RVA: 0x002F1FAC File Offset: 0x002F0FAC
		private void RemoveRegion(int rowIndex, int columnIndex, int rowCount, int columnCount, ref List<VirtualizedCellInfoCollection.CellRegion> removeList)
		{
			if (!this.IsEmpty)
			{
				VirtualizedCellInfoCollection.CellRegion region = new VirtualizedCellInfoCollection.CellRegion(columnIndex, rowIndex, columnCount, rowCount);
				for (int i = 0; i < this._regions.Count; i++)
				{
					VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
					VirtualizedCellInfoCollection.CellRegion cellRegion2 = cellRegion.Intersection(region);
					if (!cellRegion2.IsEmpty)
					{
						if (removeList == null)
						{
							removeList = new List<VirtualizedCellInfoCollection.CellRegion>();
						}
						removeList.Add(cellRegion2);
						this._regions.RemoveAt(i);
						List<VirtualizedCellInfoCollection.CellRegion> list;
						cellRegion.Remainder(cellRegion2, out list);
						if (list != null)
						{
							this._regions.InsertRange(i, list);
							i += list.Count;
						}
						i--;
					}
				}
			}
		}

		// Token: 0x06007758 RID: 30552 RVA: 0x002F2054 File Offset: 0x002F1054
		internal void OnItemsCollectionChanged(NotifyCollectionChangedEventArgs e, List<Tuple<int, int>> ranges)
		{
			if (!this.IsEmpty)
			{
				switch (e.Action)
				{
				case NotifyCollectionChangedAction.Add:
					this.OnAddRow(e.NewStartingIndex);
					return;
				case NotifyCollectionChangedAction.Remove:
					this.OnRemoveRow(e.OldStartingIndex, e.OldItems[0]);
					return;
				case NotifyCollectionChangedAction.Replace:
					this.OnReplaceRow(e.OldStartingIndex, e.OldItems[0]);
					return;
				case NotifyCollectionChangedAction.Move:
					this.OnMoveRow(e.OldStartingIndex, e.NewStartingIndex);
					return;
				case NotifyCollectionChangedAction.Reset:
					this.RestoreOnlyFullRows(ranges);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06007759 RID: 30553 RVA: 0x002F20E4 File Offset: 0x002F10E4
		private void OnAddRow(int rowIndex)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count2 > 0)
			{
				this.RemoveRegion(rowIndex, 0, count - 1 - rowIndex, count2, ref list);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top + 1, cellRegion.Left, cellRegion.Height, cellRegion.Width, false);
					}
				}
			}
		}

		// Token: 0x0600775A RID: 30554 RVA: 0x002F2174 File Offset: 0x002F1174
		private void OnRemoveRow(int rowIndex, object item)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			List<VirtualizedCellInfoCollection.CellRegion> list2 = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count2 > 0)
			{
				this.RemoveRegion(rowIndex + 1, 0, count - rowIndex, count2, ref list);
				this.RemoveRegion(rowIndex, 0, 1, count2, ref list2);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top - 1, cellRegion.Left, cellRegion.Height, cellRegion.Width, false);
					}
				}
				if (list2 != null)
				{
					VirtualizedCellInfoCollection.RemovedCellInfoCollection oldItems = new VirtualizedCellInfoCollection.RemovedCellInfoCollection(this._owner, list2, item);
					this.OnRemove(oldItems);
				}
			}
		}

		// Token: 0x0600775B RID: 30555 RVA: 0x002F2230 File Offset: 0x002F1230
		private void OnReplaceRow(int rowIndex, object item)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			this.RemoveRegion(rowIndex, 0, 1, this._owner.Columns.Count, ref list);
			if (list != null)
			{
				VirtualizedCellInfoCollection.RemovedCellInfoCollection oldItems = new VirtualizedCellInfoCollection.RemovedCellInfoCollection(this._owner, list, item);
				this.OnRemove(oldItems);
			}
		}

		// Token: 0x0600775C RID: 30556 RVA: 0x002F2274 File Offset: 0x002F1274
		private void OnMoveRow(int oldIndex, int newIndex)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			List<VirtualizedCellInfoCollection.CellRegion> list2 = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count2 > 0)
			{
				this.RemoveRegion(oldIndex + 1, 0, count - oldIndex - 1, count2, ref list);
				this.RemoveRegion(oldIndex, 0, 1, count2, ref list2);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top - 1, cellRegion.Left, cellRegion.Height, cellRegion.Width, false);
					}
				}
				list = null;
				this.RemoveRegion(newIndex, 0, count - newIndex, count2, ref list);
				if (list2 != null)
				{
					int count4 = list2.Count;
					for (int j = 0; j < count4; j++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion2 = list2[j];
						this.AddRegion(newIndex, cellRegion2.Left, cellRegion2.Height, cellRegion2.Width, false);
					}
				}
				if (list != null)
				{
					int count5 = list.Count;
					for (int k = 0; k < count5; k++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion3 = list[k];
						this.AddRegion(cellRegion3.Top + 1, cellRegion3.Left, cellRegion3.Height, cellRegion3.Width, false);
					}
				}
			}
		}

		// Token: 0x0600775D RID: 30557 RVA: 0x002F23B8 File Offset: 0x002F13B8
		internal void OnColumnsChanged(NotifyCollectionChangedAction action, int oldDisplayIndex, DataGridColumn oldColumn, int newDisplayIndex, IList selectedRows)
		{
			if (!this.IsEmpty)
			{
				switch (action)
				{
				case NotifyCollectionChangedAction.Add:
					this.OnAddColumn(newDisplayIndex, selectedRows);
					return;
				case NotifyCollectionChangedAction.Remove:
					this.OnRemoveColumn(oldDisplayIndex, oldColumn);
					return;
				case NotifyCollectionChangedAction.Replace:
					this.OnReplaceColumn(oldDisplayIndex, oldColumn, selectedRows);
					return;
				case NotifyCollectionChangedAction.Move:
					this.OnMoveColumn(oldDisplayIndex, newDisplayIndex);
					return;
				case NotifyCollectionChangedAction.Reset:
					this._regions.Clear();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600775E RID: 30558 RVA: 0x002F241C File Offset: 0x002F141C
		private void OnAddColumn(int columnIndex, IList selectedRows)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count > 0)
			{
				this.RemoveRegion(0, columnIndex, count, count2 - 1 - columnIndex, ref list);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top, cellRegion.Left + 1, cellRegion.Height, cellRegion.Width, false);
					}
				}
				this.FillInFullRowRegions(selectedRows, columnIndex, true);
			}
		}

		// Token: 0x0600775F RID: 30559 RVA: 0x002F24B4 File Offset: 0x002F14B4
		private void FillInFullRowRegions(IList rows, int columnIndex, bool notify)
		{
			int count = rows.Count;
			for (int i = 0; i < count; i++)
			{
				int num = this._owner.Items.IndexOf(rows[i]);
				if (num >= 0)
				{
					this.AddRegion(num, columnIndex, 1, 1, notify);
				}
			}
		}

		// Token: 0x06007760 RID: 30560 RVA: 0x002F24FC File Offset: 0x002F14FC
		private void OnRemoveColumn(int columnIndex, DataGridColumn oldColumn)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			List<VirtualizedCellInfoCollection.CellRegion> list2 = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count > 0)
			{
				this.RemoveRegion(0, columnIndex + 1, count, count2 - columnIndex, ref list);
				this.RemoveRegion(0, columnIndex, count, 1, ref list2);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top, cellRegion.Left - 1, cellRegion.Height, cellRegion.Width, false);
					}
				}
				if (list2 != null)
				{
					VirtualizedCellInfoCollection.RemovedCellInfoCollection oldItems = new VirtualizedCellInfoCollection.RemovedCellInfoCollection(this._owner, list2, oldColumn);
					this.OnRemove(oldItems);
				}
			}
		}

		// Token: 0x06007761 RID: 30561 RVA: 0x002F25B8 File Offset: 0x002F15B8
		private void OnReplaceColumn(int columnIndex, DataGridColumn oldColumn, IList selectedRows)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			this.RemoveRegion(0, columnIndex, this._owner.Items.Count, 1, ref list);
			if (list != null)
			{
				VirtualizedCellInfoCollection.RemovedCellInfoCollection oldItems = new VirtualizedCellInfoCollection.RemovedCellInfoCollection(this._owner, list, oldColumn);
				this.OnRemove(oldItems);
			}
			this.FillInFullRowRegions(selectedRows, columnIndex, true);
		}

		// Token: 0x06007762 RID: 30562 RVA: 0x002F2604 File Offset: 0x002F1604
		private void OnMoveColumn(int oldIndex, int newIndex)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			List<VirtualizedCellInfoCollection.CellRegion> list2 = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count > 0)
			{
				this.RemoveRegion(0, oldIndex + 1, count, count2 - oldIndex - 1, ref list);
				this.RemoveRegion(0, oldIndex, count, 1, ref list2);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top, cellRegion.Left - 1, cellRegion.Height, cellRegion.Width, false);
					}
				}
				list = null;
				this.RemoveRegion(0, newIndex, count, count2 - newIndex, ref list);
				if (list2 != null)
				{
					int count4 = list2.Count;
					for (int j = 0; j < count4; j++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion2 = list2[j];
						this.AddRegion(cellRegion2.Top, newIndex, cellRegion2.Height, cellRegion2.Width, false);
					}
				}
				if (list != null)
				{
					int count5 = list.Count;
					for (int k = 0; k < count5; k++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion3 = list[k];
						this.AddRegion(cellRegion3.Top, cellRegion3.Left + 1, cellRegion3.Height, cellRegion3.Width, false);
					}
				}
			}
		}

		// Token: 0x06007763 RID: 30563 RVA: 0x002F2748 File Offset: 0x002F1748
		internal void Union(VirtualizedCellInfoCollection collection)
		{
			int count = collection._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = collection._regions[i];
				this.AddRegion(cellRegion.Top, cellRegion.Left, cellRegion.Height, cellRegion.Width);
			}
		}

		// Token: 0x06007764 RID: 30564 RVA: 0x002F279C File Offset: 0x002F179C
		internal static void Xor(VirtualizedCellInfoCollection c1, VirtualizedCellInfoCollection c2)
		{
			VirtualizedCellInfoCollection virtualizedCellInfoCollection = new VirtualizedCellInfoCollection(c2._owner, c2._regions);
			int count = c1._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = c1._regions[i];
				c2.RemoveRegion(cellRegion.Top, cellRegion.Left, cellRegion.Height, cellRegion.Width);
			}
			count = virtualizedCellInfoCollection._regions.Count;
			for (int j = 0; j < count; j++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion2 = virtualizedCellInfoCollection._regions[j];
				c1.RemoveRegion(cellRegion2.Top, cellRegion2.Left, cellRegion2.Height, cellRegion2.Width);
			}
		}

		// Token: 0x06007765 RID: 30565 RVA: 0x002F2850 File Offset: 0x002F1850
		internal void ClearFullRows(IList rows)
		{
			if (!this.IsEmpty)
			{
				int count = this._owner.Columns.Count;
				if (this._regions.Count == 1)
				{
					VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[0];
					if (cellRegion.Width == count && cellRegion.Height == rows.Count)
					{
						this.Clear();
						return;
					}
				}
				List<VirtualizedCellInfoCollection.CellRegion> list = new List<VirtualizedCellInfoCollection.CellRegion>();
				int count2 = rows.Count;
				for (int i = 0; i < count2; i++)
				{
					int num = this._owner.Items.IndexOf(rows[i]);
					if (num >= 0)
					{
						this.RemoveRegion(num, 0, 1, count, ref list);
					}
				}
				if (list.Count > 0)
				{
					this.OnRemove(new VirtualizedCellInfoCollection(this._owner, list));
				}
			}
		}

		// Token: 0x06007766 RID: 30566 RVA: 0x002F2918 File Offset: 0x002F1918
		internal void RestoreOnlyFullRows(List<Tuple<int, int>> ranges)
		{
			this.Clear();
			int count = this._owner.Columns.Count;
			if (count > 0)
			{
				foreach (Tuple<int, int> tuple in ranges)
				{
					this.AddRegion(tuple.Item1, 0, tuple.Item2, count);
				}
			}
		}

		// Token: 0x06007767 RID: 30567 RVA: 0x002F2990 File Offset: 0x002F1990
		internal void RemoveAllButOne(DataGridCellInfo cellInfo)
		{
			if (!this.IsEmpty)
			{
				int rowIndex;
				int columnIndex;
				this.ConvertCellInfoToIndexes(cellInfo, out rowIndex, out columnIndex);
				this.RemoveAllButRegion(rowIndex, columnIndex, 1, 1);
			}
		}

		// Token: 0x06007768 RID: 30568 RVA: 0x002F29BC File Offset: 0x002F19BC
		internal void RemoveAllButOne()
		{
			if (!this.IsEmpty)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[0];
				this.RemoveAllButRegion(cellRegion.Top, cellRegion.Left, 1, 1);
			}
		}

		// Token: 0x06007769 RID: 30569 RVA: 0x002F29F4 File Offset: 0x002F19F4
		internal void RemoveAllButOneRow(int rowIndex)
		{
			if (!this.IsEmpty && rowIndex >= 0)
			{
				this.RemoveAllButRegion(rowIndex, 0, 1, this._owner.Columns.Count);
			}
		}

		// Token: 0x0600776A RID: 30570 RVA: 0x002F2A1C File Offset: 0x002F1A1C
		private void RemoveAllButRegion(int rowIndex, int columnIndex, int rowCount, int columnCount)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			this.RemoveRegion(rowIndex, columnIndex, rowCount, columnCount, ref list);
			VirtualizedCellInfoCollection oldItems = new VirtualizedCellInfoCollection(this._owner, this._regions);
			this._regions.Clear();
			this._regions.Add(new VirtualizedCellInfoCollection.CellRegion(columnIndex, rowIndex, columnCount, rowCount));
			this.OnRemove(oldItems);
		}

		// Token: 0x0600776B RID: 30571 RVA: 0x002F2A74 File Offset: 0x002F1A74
		internal bool Intersects(int rowIndex)
		{
			VirtualizedCellInfoCollection.CellRegion region = new VirtualizedCellInfoCollection.CellRegion(0, rowIndex, this._owner.Columns.Count, 1);
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this._regions[i].Intersects(region))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600776C RID: 30572 RVA: 0x002F2AD0 File Offset: 0x002F1AD0
		internal bool Intersects(int rowIndex, out List<int> columnIndexRanges)
		{
			VirtualizedCellInfoCollection.CellRegion region = new VirtualizedCellInfoCollection.CellRegion(0, rowIndex, this._owner.Columns.Count, 1);
			columnIndexRanges = null;
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
				if (cellRegion.Intersects(region))
				{
					if (columnIndexRanges == null)
					{
						columnIndexRanges = new List<int>();
					}
					columnIndexRanges.Add(cellRegion.Left);
					columnIndexRanges.Add(cellRegion.Width);
				}
			}
			return columnIndexRanges != null;
		}

		// Token: 0x17001BB4 RID: 7092
		// (get) Token: 0x0600776D RID: 30573 RVA: 0x002F2B55 File Offset: 0x002F1B55
		protected DataGrid Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x0600776E RID: 30574 RVA: 0x002F2B5D File Offset: 0x002F1B5D
		private void ConvertCellInfoToIndexes(DataGridCellInfo cell, out int rowIndex, out int columnIndex)
		{
			columnIndex = cell.Column.DisplayIndex;
			rowIndex = cell.ItemInfo.Index;
			if (rowIndex < 0)
			{
				rowIndex = this._owner.Items.IndexOf(cell.Item);
			}
		}

		// Token: 0x0600776F RID: 30575 RVA: 0x002F2B9C File Offset: 0x002F1B9C
		private static void ConvertIndexToIndexes(List<VirtualizedCellInfoCollection.CellRegion> regions, int index, out int rowIndex, out int columnIndex)
		{
			columnIndex = -1;
			rowIndex = -1;
			int count = regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = regions[i];
				int size = cellRegion.Size;
				if (index < size)
				{
					columnIndex = cellRegion.Left + index % cellRegion.Width;
					rowIndex = cellRegion.Top + index / cellRegion.Width;
					return;
				}
				index -= size;
			}
		}

		// Token: 0x06007770 RID: 30576 RVA: 0x002F2C04 File Offset: 0x002F1C04
		private DataGridCellInfo GetCellInfoFromIndex(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions, int index)
		{
			int num;
			int num2;
			VirtualizedCellInfoCollection.ConvertIndexToIndexes(regions, index, out num, out num2);
			if (num >= 0 && num2 >= 0 && num < owner.Items.Count && num2 < owner.Columns.Count)
			{
				DataGridColumn column = owner.ColumnFromDisplayIndex(num2);
				ItemsControl.ItemInfo rowInfo = owner.ItemInfoFromIndex(num);
				return this.CreateCellInfo(rowInfo, column, owner);
			}
			return DataGridCellInfo.Unset;
		}

		// Token: 0x06007771 RID: 30577 RVA: 0x002F2C5F File Offset: 0x002F1C5F
		private void ValidateIsReadOnly()
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException(SR.Get("VirtualizedCellInfoCollection_IsReadOnly"));
			}
		}

		// Token: 0x06007772 RID: 30578 RVA: 0x002F2C7C File Offset: 0x002F1C7C
		private void AddRegionToList(VirtualizedCellInfoCollection.CellRegion region, List<DataGridCellInfo> list)
		{
			for (int i = region.Top; i <= region.Bottom; i++)
			{
				ItemsControl.ItemInfo rowInfo = this._owner.ItemInfoFromIndex(i);
				for (int j = region.Left; j <= region.Right; j++)
				{
					DataGridColumn column = this._owner.ColumnFromDisplayIndex(j);
					DataGridCellInfo item = this.CreateCellInfo(rowInfo, column, this._owner);
					list.Add(item);
				}
			}
		}

		// Token: 0x06007773 RID: 30579 RVA: 0x002F2CEB File Offset: 0x002F1CEB
		protected virtual DataGridCellInfo CreateCellInfo(ItemsControl.ItemInfo rowInfo, DataGridColumn column, DataGrid owner)
		{
			return new DataGridCellInfo(rowInfo, column, owner);
		}

		// Token: 0x040038D4 RID: 14548
		private bool _isReadOnly;

		// Token: 0x040038D5 RID: 14549
		private DataGrid _owner;

		// Token: 0x040038D6 RID: 14550
		private List<VirtualizedCellInfoCollection.CellRegion> _regions;

		// Token: 0x02000C2B RID: 3115
		private class VirtualizedCellInfoCollectionEnumerator : IEnumerator<DataGridCellInfo>, IEnumerator, IDisposable
		{
			// Token: 0x060090B1 RID: 37041 RVA: 0x00347058 File Offset: 0x00346058
			public VirtualizedCellInfoCollectionEnumerator(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions, VirtualizedCellInfoCollection collection)
			{
				this._owner = owner;
				this._regions = new List<VirtualizedCellInfoCollection.CellRegion>(regions);
				this._current = -1;
				this._collection = collection;
				if (this._regions != null)
				{
					int count = this._regions.Count;
					for (int i = 0; i < count; i++)
					{
						this._count += this._regions[i].Size;
					}
				}
			}

			// Token: 0x060090B2 RID: 37042 RVA: 0x00219F89 File Offset: 0x00218F89
			public void Dispose()
			{
				GC.SuppressFinalize(this);
			}

			// Token: 0x060090B3 RID: 37043 RVA: 0x003470CD File Offset: 0x003460CD
			public bool MoveNext()
			{
				if (this._current < this._count)
				{
					this._current++;
				}
				return this.CurrentWithinBounds;
			}

			// Token: 0x060090B4 RID: 37044 RVA: 0x003470F1 File Offset: 0x003460F1
			public void Reset()
			{
				this._current = -1;
			}

			// Token: 0x17001FA0 RID: 8096
			// (get) Token: 0x060090B5 RID: 37045 RVA: 0x003470FA File Offset: 0x003460FA
			public DataGridCellInfo Current
			{
				get
				{
					if (this.CurrentWithinBounds)
					{
						return this._collection.GetCellInfoFromIndex(this._owner, this._regions, this._current);
					}
					return DataGridCellInfo.Unset;
				}
			}

			// Token: 0x17001FA1 RID: 8097
			// (get) Token: 0x060090B6 RID: 37046 RVA: 0x00347127 File Offset: 0x00346127
			private bool CurrentWithinBounds
			{
				get
				{
					return this._current >= 0 && this._current < this._count;
				}
			}

			// Token: 0x17001FA2 RID: 8098
			// (get) Token: 0x060090B7 RID: 37047 RVA: 0x00347142 File Offset: 0x00346142
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04004B57 RID: 19287
			private DataGrid _owner;

			// Token: 0x04004B58 RID: 19288
			private List<VirtualizedCellInfoCollection.CellRegion> _regions;

			// Token: 0x04004B59 RID: 19289
			private int _current;

			// Token: 0x04004B5A RID: 19290
			private int _count;

			// Token: 0x04004B5B RID: 19291
			private VirtualizedCellInfoCollection _collection;
		}

		// Token: 0x02000C2C RID: 3116
		private struct CellRegion
		{
			// Token: 0x060090B8 RID: 37048 RVA: 0x0034714F File Offset: 0x0034614F
			public CellRegion(int left, int top, int width, int height)
			{
				this._left = left;
				this._top = top;
				this._width = width;
				this._height = height;
			}

			// Token: 0x17001FA3 RID: 8099
			// (get) Token: 0x060090B9 RID: 37049 RVA: 0x0034716E File Offset: 0x0034616E
			// (set) Token: 0x060090BA RID: 37050 RVA: 0x00347176 File Offset: 0x00346176
			public int Left
			{
				get
				{
					return this._left;
				}
				set
				{
					this._left = value;
				}
			}

			// Token: 0x17001FA4 RID: 8100
			// (get) Token: 0x060090BB RID: 37051 RVA: 0x0034717F File Offset: 0x0034617F
			// (set) Token: 0x060090BC RID: 37052 RVA: 0x00347187 File Offset: 0x00346187
			public int Top
			{
				get
				{
					return this._top;
				}
				set
				{
					this._top = value;
				}
			}

			// Token: 0x17001FA5 RID: 8101
			// (get) Token: 0x060090BD RID: 37053 RVA: 0x00347190 File Offset: 0x00346190
			// (set) Token: 0x060090BE RID: 37054 RVA: 0x003471A1 File Offset: 0x003461A1
			public int Right
			{
				get
				{
					return this._left + this._width - 1;
				}
				set
				{
					this._width = value - this._left + 1;
				}
			}

			// Token: 0x17001FA6 RID: 8102
			// (get) Token: 0x060090BF RID: 37055 RVA: 0x003471B3 File Offset: 0x003461B3
			// (set) Token: 0x060090C0 RID: 37056 RVA: 0x003471C4 File Offset: 0x003461C4
			public int Bottom
			{
				get
				{
					return this._top + this._height - 1;
				}
				set
				{
					this._height = value - this._top + 1;
				}
			}

			// Token: 0x17001FA7 RID: 8103
			// (get) Token: 0x060090C1 RID: 37057 RVA: 0x003471D6 File Offset: 0x003461D6
			// (set) Token: 0x060090C2 RID: 37058 RVA: 0x003471DE File Offset: 0x003461DE
			public int Width
			{
				get
				{
					return this._width;
				}
				set
				{
					this._width = value;
				}
			}

			// Token: 0x17001FA8 RID: 8104
			// (get) Token: 0x060090C3 RID: 37059 RVA: 0x003471E7 File Offset: 0x003461E7
			// (set) Token: 0x060090C4 RID: 37060 RVA: 0x003471EF File Offset: 0x003461EF
			public int Height
			{
				get
				{
					return this._height;
				}
				set
				{
					this._height = value;
				}
			}

			// Token: 0x17001FA9 RID: 8105
			// (get) Token: 0x060090C5 RID: 37061 RVA: 0x003471F8 File Offset: 0x003461F8
			public bool IsEmpty
			{
				get
				{
					return this._width == 0 || this._height == 0;
				}
			}

			// Token: 0x17001FAA RID: 8106
			// (get) Token: 0x060090C6 RID: 37062 RVA: 0x0034720D File Offset: 0x0034620D
			public int Size
			{
				get
				{
					return this._width * this._height;
				}
			}

			// Token: 0x060090C7 RID: 37063 RVA: 0x0034721C File Offset: 0x0034621C
			public bool Contains(int x, int y)
			{
				return !this.IsEmpty && (x >= this.Left && y >= this.Top && x <= this.Right) && y <= this.Bottom;
			}

			// Token: 0x060090C8 RID: 37064 RVA: 0x00347254 File Offset: 0x00346254
			public bool Contains(VirtualizedCellInfoCollection.CellRegion region)
			{
				return this.Left <= region.Left && this.Top <= region.Top && this.Right >= region.Right && this.Bottom >= region.Bottom;
			}

			// Token: 0x060090C9 RID: 37065 RVA: 0x003472A4 File Offset: 0x003462A4
			public bool Intersects(VirtualizedCellInfoCollection.CellRegion region)
			{
				return VirtualizedCellInfoCollection.CellRegion.Intersects(this.Left, this.Right, region.Left, region.Right) && VirtualizedCellInfoCollection.CellRegion.Intersects(this.Top, this.Bottom, region.Top, region.Bottom);
			}

			// Token: 0x060090CA RID: 37066 RVA: 0x003472F3 File Offset: 0x003462F3
			private static bool Intersects(int start1, int end1, int start2, int end2)
			{
				return start1 <= end2 && end1 >= start2;
			}

			// Token: 0x060090CB RID: 37067 RVA: 0x00347304 File Offset: 0x00346304
			public VirtualizedCellInfoCollection.CellRegion Intersection(VirtualizedCellInfoCollection.CellRegion region)
			{
				if (this.Intersects(region))
				{
					int num = Math.Max(this.Left, region.Left);
					int num2 = Math.Max(this.Top, region.Top);
					int num3 = Math.Min(this.Right, region.Right);
					int num4 = Math.Min(this.Bottom, region.Bottom);
					return new VirtualizedCellInfoCollection.CellRegion(num, num2, num3 - num + 1, num4 - num2 + 1);
				}
				return VirtualizedCellInfoCollection.CellRegion.Empty;
			}

			// Token: 0x060090CC RID: 37068 RVA: 0x00347380 File Offset: 0x00346380
			public bool Union(VirtualizedCellInfoCollection.CellRegion region)
			{
				if (this.Contains(region))
				{
					return true;
				}
				if (region.Contains(this))
				{
					this.Left = region.Left;
					this.Top = region.Top;
					this.Width = region.Width;
					this.Height = region.Height;
					return true;
				}
				bool flag = region.Left == this.Left && region.Width == this.Width;
				bool flag2 = region.Top == this.Top && region.Height == this.Height;
				if (flag || flag2)
				{
					int num = flag ? this.Top : this.Left;
					int num2 = flag ? this.Bottom : this.Right;
					int num3 = flag ? region.Top : region.Left;
					int num4 = flag ? region.Bottom : region.Right;
					bool flag3 = false;
					if (num4 <= num2)
					{
						flag3 = (num - num4 <= 1);
					}
					else if (num <= num3)
					{
						flag3 = (num3 - num2 <= 1);
					}
					if (flag3)
					{
						int right = this.Right;
						int bottom = this.Bottom;
						this.Left = Math.Min(this.Left, region.Left);
						this.Top = Math.Min(this.Top, region.Top);
						this.Right = Math.Max(right, region.Right);
						this.Bottom = Math.Max(bottom, region.Bottom);
						return true;
					}
				}
				return false;
			}

			// Token: 0x060090CD RID: 37069 RVA: 0x00347510 File Offset: 0x00346510
			public bool Remainder(VirtualizedCellInfoCollection.CellRegion region, out List<VirtualizedCellInfoCollection.CellRegion> remainder)
			{
				if (this.Intersects(region))
				{
					if (region.Contains(this))
					{
						remainder = null;
					}
					else
					{
						remainder = new List<VirtualizedCellInfoCollection.CellRegion>();
						if (this.Top < region.Top)
						{
							remainder.Add(new VirtualizedCellInfoCollection.CellRegion(this.Left, this.Top, this.Width, region.Top - this.Top));
						}
						if (this.Left < region.Left)
						{
							int num = Math.Max(this.Top, region.Top);
							int num2 = Math.Min(this.Bottom, region.Bottom);
							remainder.Add(new VirtualizedCellInfoCollection.CellRegion(this.Left, num, region.Left - this.Left, num2 - num + 1));
						}
						if (this.Right > region.Right)
						{
							int num3 = Math.Max(this.Top, region.Top);
							int num4 = Math.Min(this.Bottom, region.Bottom);
							remainder.Add(new VirtualizedCellInfoCollection.CellRegion(region.Right + 1, num3, this.Right - region.Right, num4 - num3 + 1));
						}
						if (this.Bottom > region.Bottom)
						{
							remainder.Add(new VirtualizedCellInfoCollection.CellRegion(this.Left, region.Bottom + 1, this.Width, this.Bottom - region.Bottom));
						}
					}
					return true;
				}
				remainder = null;
				return false;
			}

			// Token: 0x17001FAB RID: 8107
			// (get) Token: 0x060090CE RID: 37070 RVA: 0x0034767F File Offset: 0x0034667F
			public static VirtualizedCellInfoCollection.CellRegion Empty
			{
				get
				{
					return new VirtualizedCellInfoCollection.CellRegion(0, 0, 0, 0);
				}
			}

			// Token: 0x04004B5C RID: 19292
			private int _left;

			// Token: 0x04004B5D RID: 19293
			private int _top;

			// Token: 0x04004B5E RID: 19294
			private int _width;

			// Token: 0x04004B5F RID: 19295
			private int _height;
		}

		// Token: 0x02000C2D RID: 3117
		private class RemovedCellInfoCollection : VirtualizedCellInfoCollection
		{
			// Token: 0x060090CF RID: 37071 RVA: 0x0034768A File Offset: 0x0034668A
			internal RemovedCellInfoCollection(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions, DataGridColumn column) : base(owner, regions)
			{
				this._removedColumn = column;
			}

			// Token: 0x060090D0 RID: 37072 RVA: 0x0034769B File Offset: 0x0034669B
			internal RemovedCellInfoCollection(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions, object item) : base(owner, regions)
			{
				this._removedItem = item;
			}

			// Token: 0x060090D1 RID: 37073 RVA: 0x003476AC File Offset: 0x003466AC
			protected override DataGridCellInfo CreateCellInfo(ItemsControl.ItemInfo rowInfo, DataGridColumn column, DataGrid owner)
			{
				if (this._removedColumn != null)
				{
					return new DataGridCellInfo(rowInfo, this._removedColumn, owner);
				}
				return new DataGridCellInfo(this._removedItem, column, owner);
			}

			// Token: 0x04004B60 RID: 19296
			private DataGridColumn _removedColumn;

			// Token: 0x04004B61 RID: 19297
			private object _removedItem;
		}
	}
}
