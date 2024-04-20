using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000787 RID: 1927
	[Serializable]
	public class GridViewColumnCollection : ObservableCollection<GridViewColumn>
	{
		// Token: 0x06006A93 RID: 27283 RVA: 0x002C2332 File Offset: 0x002C1332
		protected override void ClearItems()
		{
			this.VerifyAccess();
			this._internalEventArg = this.ClearPreprocess();
			base.ClearItems();
		}

		// Token: 0x06006A94 RID: 27284 RVA: 0x002C234C File Offset: 0x002C134C
		protected override void RemoveItem(int index)
		{
			this.VerifyAccess();
			this._internalEventArg = this.RemoveAtPreprocess(index);
			base.RemoveItem(index);
		}

		// Token: 0x06006A95 RID: 27285 RVA: 0x002C2368 File Offset: 0x002C1368
		protected override void InsertItem(int index, GridViewColumn column)
		{
			this.VerifyAccess();
			this._internalEventArg = this.InsertPreprocess(index, column);
			base.InsertItem(index, column);
		}

		// Token: 0x06006A96 RID: 27286 RVA: 0x002C2386 File Offset: 0x002C1386
		protected override void SetItem(int index, GridViewColumn column)
		{
			this.VerifyAccess();
			this._internalEventArg = this.SetPreprocess(index, column);
			if (this._internalEventArg != null)
			{
				base.SetItem(index, column);
			}
		}

		// Token: 0x06006A97 RID: 27287 RVA: 0x002C23AC File Offset: 0x002C13AC
		protected override void MoveItem(int oldIndex, int newIndex)
		{
			if (oldIndex != newIndex)
			{
				this.VerifyAccess();
				this._internalEventArg = this.MovePreprocess(oldIndex, newIndex);
				base.MoveItem(oldIndex, newIndex);
			}
		}

		// Token: 0x06006A98 RID: 27288 RVA: 0x002C23CE File Offset: 0x002C13CE
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			this.OnInternalCollectionChanged();
			base.OnCollectionChanged(e);
		}

		// Token: 0x1400011A RID: 282
		// (add) Token: 0x06006A99 RID: 27289 RVA: 0x002C23DD File Offset: 0x002C13DD
		// (remove) Token: 0x06006A9A RID: 27290 RVA: 0x002C23E6 File Offset: 0x002C13E6
		internal event NotifyCollectionChangedEventHandler InternalCollectionChanged
		{
			add
			{
				this._internalCollectionChanged += value;
			}
			remove
			{
				this._internalCollectionChanged -= value;
			}
		}

		// Token: 0x06006A9B RID: 27291 RVA: 0x002C23EF File Offset: 0x002C13EF
		internal void BlockWrite()
		{
			this.IsImmutable = true;
		}

		// Token: 0x06006A9C RID: 27292 RVA: 0x002C23F8 File Offset: 0x002C13F8
		internal void UnblockWrite()
		{
			this.IsImmutable = false;
		}

		// Token: 0x170018A4 RID: 6308
		// (get) Token: 0x06006A9D RID: 27293 RVA: 0x002C2401 File Offset: 0x002C1401
		internal List<GridViewColumn> ColumnCollection
		{
			get
			{
				return this._columns;
			}
		}

		// Token: 0x170018A5 RID: 6309
		// (get) Token: 0x06006A9E RID: 27294 RVA: 0x002C2409 File Offset: 0x002C1409
		internal List<int> IndexList
		{
			get
			{
				return this._actualIndices;
			}
		}

		// Token: 0x170018A6 RID: 6310
		// (get) Token: 0x06006A9F RID: 27295 RVA: 0x002C2411 File Offset: 0x002C1411
		// (set) Token: 0x06006AA0 RID: 27296 RVA: 0x002C241C File Offset: 0x002C141C
		internal DependencyObject Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (value != this._owner)
				{
					if (value == null)
					{
						using (List<GridViewColumn>.Enumerator enumerator = this._columns.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								GridViewColumn oldValue = enumerator.Current;
								InheritanceContextHelper.RemoveContextFromObject(this._owner, oldValue);
							}
							goto IL_7D;
						}
					}
					foreach (GridViewColumn newValue in this._columns)
					{
						InheritanceContextHelper.ProvideContextForObject(value, newValue);
					}
					IL_7D:
					this._owner = value;
				}
			}
		}

		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x06006AA1 RID: 27297 RVA: 0x002C24CC File Offset: 0x002C14CC
		// (set) Token: 0x06006AA2 RID: 27298 RVA: 0x002C24D4 File Offset: 0x002C14D4
		internal bool InViewMode
		{
			get
			{
				return this._inViewMode;
			}
			set
			{
				this._inViewMode = value;
			}
		}

		// Token: 0x06006AA3 RID: 27299 RVA: 0x002C24DD File Offset: 0x002C14DD
		private void OnInternalCollectionChanged()
		{
			if (this._internalCollectionChanged != null && this._internalEventArg != null)
			{
				this._internalCollectionChanged(this, this._internalEventArg);
				this._internalEventArg = null;
			}
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x002C2508 File Offset: 0x002C1508
		private void ColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = sender as GridViewColumn;
			if (this._internalCollectionChanged != null && gridViewColumn != null)
			{
				this._internalCollectionChanged(this, new GridViewColumnCollectionChangedEventArgs(gridViewColumn, e.PropertyName));
			}
		}

		// Token: 0x06006AA5 RID: 27301 RVA: 0x002C2540 File Offset: 0x002C1540
		private GridViewColumnCollectionChangedEventArgs MovePreprocess(int oldIndex, int newIndex)
		{
			this.VerifyIndexInRange(oldIndex, "oldIndex");
			this.VerifyIndexInRange(newIndex, "newIndex");
			int num = this._actualIndices[oldIndex];
			if (oldIndex < newIndex)
			{
				for (int i = oldIndex; i < newIndex; i++)
				{
					this._actualIndices[i] = this._actualIndices[i + 1];
				}
			}
			else
			{
				for (int j = oldIndex; j > newIndex; j--)
				{
					this._actualIndices[j] = this._actualIndices[j - 1];
				}
			}
			this._actualIndices[newIndex] = num;
			return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, this._columns[num], newIndex, oldIndex, num);
		}

		// Token: 0x06006AA6 RID: 27302 RVA: 0x002C25E8 File Offset: 0x002C15E8
		private GridViewColumnCollectionChangedEventArgs ClearPreprocess()
		{
			GridViewColumn[] array = new GridViewColumn[base.Count];
			if (base.Count > 0)
			{
				base.CopyTo(array, 0);
			}
			foreach (GridViewColumn gridViewColumn in this._columns)
			{
				gridViewColumn.ResetPrivateData();
				((INotifyPropertyChanged)gridViewColumn).PropertyChanged -= this.ColumnPropertyChanged;
				InheritanceContextHelper.RemoveContextFromObject(this._owner, gridViewColumn);
			}
			this._columns.Clear();
			this._actualIndices.Clear();
			return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, array);
		}

		// Token: 0x06006AA7 RID: 27303 RVA: 0x002C2694 File Offset: 0x002C1694
		private GridViewColumnCollectionChangedEventArgs RemoveAtPreprocess(int index)
		{
			this.VerifyIndexInRange(index, "index");
			int num = this._actualIndices[index];
			GridViewColumn gridViewColumn = this._columns[num];
			gridViewColumn.ResetPrivateData();
			((INotifyPropertyChanged)gridViewColumn).PropertyChanged -= this.ColumnPropertyChanged;
			this._columns.RemoveAt(num);
			this.UpdateIndexList(num, index);
			this.UpdateActualIndexInColumn(num);
			InheritanceContextHelper.RemoveContextFromObject(this._owner, gridViewColumn);
			return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, gridViewColumn, index, num);
		}

		// Token: 0x06006AA8 RID: 27304 RVA: 0x002C2710 File Offset: 0x002C1710
		private void UpdateIndexList(int actualIndex, int index)
		{
			for (int i = 0; i < index; i++)
			{
				int num = this._actualIndices[i];
				if (num > actualIndex)
				{
					this._actualIndices[i] = num - 1;
				}
			}
			for (int j = index + 1; j < this._actualIndices.Count; j++)
			{
				int num2 = this._actualIndices[j];
				if (num2 < actualIndex)
				{
					this._actualIndices[j - 1] = num2;
				}
				else if (num2 > actualIndex)
				{
					this._actualIndices[j - 1] = num2 - 1;
				}
			}
			this._actualIndices.RemoveAt(this._actualIndices.Count - 1);
		}

		// Token: 0x06006AA9 RID: 27305 RVA: 0x002C27B0 File Offset: 0x002C17B0
		private void UpdateActualIndexInColumn(int iStart)
		{
			for (int i = iStart; i < this._columns.Count; i++)
			{
				this._columns[i].ActualIndex = i;
			}
		}

		// Token: 0x06006AAA RID: 27306 RVA: 0x002C27E8 File Offset: 0x002C17E8
		private GridViewColumnCollectionChangedEventArgs InsertPreprocess(int index, GridViewColumn column)
		{
			int count = this._columns.Count;
			if (index < 0 || index > count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.ValidateColumnForInsert(column);
			this._columns.Add(column);
			column.ActualIndex = count;
			this._actualIndices.Insert(index, count);
			InheritanceContextHelper.ProvideContextForObject(this._owner, column);
			((INotifyPropertyChanged)column).PropertyChanged += this.ColumnPropertyChanged;
			return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, column, index, count);
		}

		// Token: 0x06006AAB RID: 27307 RVA: 0x002C2864 File Offset: 0x002C1864
		private GridViewColumnCollectionChangedEventArgs SetPreprocess(int index, GridViewColumn newColumn)
		{
			this.VerifyIndexInRange(index, "index");
			GridViewColumn gridViewColumn = base[index];
			if (gridViewColumn != newColumn)
			{
				int actualIndex = this._actualIndices[index];
				this.RemoveAtPreprocess(index);
				this.InsertPreprocess(index, newColumn);
				return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newColumn, gridViewColumn, index, actualIndex);
			}
			return null;
		}

		// Token: 0x06006AAC RID: 27308 RVA: 0x002C28B3 File Offset: 0x002C18B3
		private void VerifyIndexInRange(int index, string indexName)
		{
			if (index < 0 || index >= this._actualIndices.Count)
			{
				throw new ArgumentOutOfRangeException(indexName);
			}
		}

		// Token: 0x06006AAD RID: 27309 RVA: 0x002C28CE File Offset: 0x002C18CE
		private void ValidateColumnForInsert(GridViewColumn column)
		{
			if (column == null)
			{
				throw new ArgumentNullException("column");
			}
			if (column.ActualIndex >= 0)
			{
				throw new InvalidOperationException(SR.Get("ListView_NotAllowShareColumnToTwoColumnCollection"));
			}
		}

		// Token: 0x06006AAE RID: 27310 RVA: 0x002C28F7 File Offset: 0x002C18F7
		private void VerifyAccess()
		{
			if (this.IsImmutable)
			{
				throw new InvalidOperationException(SR.Get("ListView_GridViewColumnCollectionIsReadOnly"));
			}
			base.CheckReentrancy();
		}

		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x06006AAF RID: 27311 RVA: 0x002C2917 File Offset: 0x002C1917
		// (set) Token: 0x06006AB0 RID: 27312 RVA: 0x002C291F File Offset: 0x002C191F
		private bool IsImmutable
		{
			get
			{
				return this._isImmutable;
			}
			set
			{
				this._isImmutable = value;
			}
		}

		// Token: 0x1400011B RID: 283
		// (add) Token: 0x06006AB1 RID: 27313 RVA: 0x002C2928 File Offset: 0x002C1928
		// (remove) Token: 0x06006AB2 RID: 27314 RVA: 0x002C2960 File Offset: 0x002C1960
		private event NotifyCollectionChangedEventHandler _internalCollectionChanged;

		// Token: 0x0400355B RID: 13659
		[NonSerialized]
		private DependencyObject _owner;

		// Token: 0x0400355C RID: 13660
		private bool _inViewMode;

		// Token: 0x0400355D RID: 13661
		private List<GridViewColumn> _columns = new List<GridViewColumn>();

		// Token: 0x0400355E RID: 13662
		private List<int> _actualIndices = new List<int>();

		// Token: 0x0400355F RID: 13663
		private bool _isImmutable;

		// Token: 0x04003561 RID: 13665
		[NonSerialized]
		private GridViewColumnCollectionChangedEventArgs _internalEventArg;
	}
}
