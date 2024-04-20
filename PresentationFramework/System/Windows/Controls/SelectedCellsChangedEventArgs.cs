using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Windows.Controls
{
	// Token: 0x020007CE RID: 1998
	public class SelectedCellsChangedEventArgs : EventArgs
	{
		// Token: 0x060072BB RID: 29371 RVA: 0x002E02EF File Offset: 0x002DF2EF
		public SelectedCellsChangedEventArgs(List<DataGridCellInfo> addedCells, List<DataGridCellInfo> removedCells)
		{
			if (addedCells == null)
			{
				throw new ArgumentNullException("addedCells");
			}
			if (removedCells == null)
			{
				throw new ArgumentNullException("removedCells");
			}
			this._addedCells = addedCells.AsReadOnly();
			this._removedCells = removedCells.AsReadOnly();
		}

		// Token: 0x060072BC RID: 29372 RVA: 0x002E032B File Offset: 0x002DF32B
		public SelectedCellsChangedEventArgs(ReadOnlyCollection<DataGridCellInfo> addedCells, ReadOnlyCollection<DataGridCellInfo> removedCells)
		{
			if (addedCells == null)
			{
				throw new ArgumentNullException("addedCells");
			}
			if (removedCells == null)
			{
				throw new ArgumentNullException("removedCells");
			}
			this._addedCells = addedCells;
			this._removedCells = removedCells;
		}

		// Token: 0x060072BD RID: 29373 RVA: 0x002E035D File Offset: 0x002DF35D
		internal SelectedCellsChangedEventArgs(DataGrid owner, VirtualizedCellInfoCollection addedCells, VirtualizedCellInfoCollection removedCells)
		{
			this._addedCells = ((addedCells != null) ? addedCells : VirtualizedCellInfoCollection.MakeEmptyCollection(owner));
			this._removedCells = ((removedCells != null) ? removedCells : VirtualizedCellInfoCollection.MakeEmptyCollection(owner));
		}

		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x060072BE RID: 29374 RVA: 0x002E0389 File Offset: 0x002DF389
		public IList<DataGridCellInfo> AddedCells
		{
			get
			{
				return this._addedCells;
			}
		}

		// Token: 0x17001A9C RID: 6812
		// (get) Token: 0x060072BF RID: 29375 RVA: 0x002E0391 File Offset: 0x002DF391
		public IList<DataGridCellInfo> RemovedCells
		{
			get
			{
				return this._removedCells;
			}
		}

		// Token: 0x04003797 RID: 14231
		private IList<DataGridCellInfo> _addedCells;

		// Token: 0x04003798 RID: 14232
		private IList<DataGridCellInfo> _removedCells;
	}
}
