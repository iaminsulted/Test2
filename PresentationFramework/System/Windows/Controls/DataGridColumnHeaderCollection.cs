using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x02000750 RID: 1872
	internal class DataGridColumnHeaderCollection : IEnumerable, INotifyCollectionChanged, IDisposable
	{
		// Token: 0x060065E8 RID: 26088 RVA: 0x002B025C File Offset: 0x002AF25C
		public DataGridColumnHeaderCollection(ObservableCollection<DataGridColumn> columns)
		{
			this._columns = columns;
			if (this._columns != null)
			{
				this._columns.CollectionChanged += this.OnColumnsChanged;
			}
		}

		// Token: 0x060065E9 RID: 26089 RVA: 0x002B028A File Offset: 0x002AF28A
		public DataGridColumn ColumnFromIndex(int index)
		{
			if (index >= 0 && index < this._columns.Count)
			{
				return this._columns[index];
			}
			return null;
		}

		// Token: 0x060065EA RID: 26090 RVA: 0x002B02AC File Offset: 0x002AF2AC
		internal void NotifyHeaderPropertyChanged(DataGridColumn column, DependencyPropertyChangedEventArgs e)
		{
			NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewValue, e.OldValue, this._columns.IndexOf(column));
			this.FireCollectionChanged(args);
		}

		// Token: 0x060065EB RID: 26091 RVA: 0x002B02E1 File Offset: 0x002AF2E1
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (this._columns != null)
			{
				this._columns.CollectionChanged -= this.OnColumnsChanged;
			}
		}

		// Token: 0x060065EC RID: 26092 RVA: 0x002B0308 File Offset: 0x002AF308
		public IEnumerator GetEnumerator()
		{
			return new DataGridColumnHeaderCollection.ColumnHeaderCollectionEnumerator(this._columns);
		}

		// Token: 0x14000107 RID: 263
		// (add) Token: 0x060065ED RID: 26093 RVA: 0x002B0318 File Offset: 0x002AF318
		// (remove) Token: 0x060065EE RID: 26094 RVA: 0x002B0350 File Offset: 0x002AF350
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x060065EF RID: 26095 RVA: 0x002B0388 File Offset: 0x002AF388
		private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedEventArgs args;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.NewItems), e.NewStartingIndex);
				break;
			case NotifyCollectionChangedAction.Remove:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.OldItems), e.OldStartingIndex);
				break;
			case NotifyCollectionChangedAction.Replace:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.NewItems), DataGridColumnHeaderCollection.HeadersFromColumns(e.OldItems), e.OldStartingIndex);
				break;
			case NotifyCollectionChangedAction.Move:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.OldItems), e.NewStartingIndex, e.OldStartingIndex);
				break;
			default:
				args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				break;
			}
			this.FireCollectionChanged(args);
		}

		// Token: 0x060065F0 RID: 26096 RVA: 0x002B0452 File Offset: 0x002AF452
		private void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		// Token: 0x060065F1 RID: 26097 RVA: 0x002B046C File Offset: 0x002AF46C
		private static object[] HeadersFromColumns(IList columns)
		{
			object[] array = new object[columns.Count];
			for (int i = 0; i < columns.Count; i++)
			{
				DataGridColumn dataGridColumn = columns[i] as DataGridColumn;
				if (dataGridColumn != null)
				{
					array[i] = dataGridColumn.Header;
				}
				else
				{
					array[i] = null;
				}
			}
			return array;
		}

		// Token: 0x0400339D RID: 13213
		private ObservableCollection<DataGridColumn> _columns;

		// Token: 0x02000BCC RID: 3020
		private class ColumnHeaderCollectionEnumerator : IEnumerator, IDisposable
		{
			// Token: 0x06008F73 RID: 36723 RVA: 0x0034446F File Offset: 0x0034346F
			public ColumnHeaderCollectionEnumerator(ObservableCollection<DataGridColumn> columns)
			{
				if (columns != null)
				{
					this._columns = columns;
					this._columns.CollectionChanged += this.OnColumnsChanged;
				}
				this._current = -1;
			}

			// Token: 0x17001F60 RID: 8032
			// (get) Token: 0x06008F74 RID: 36724 RVA: 0x003444A0 File Offset: 0x003434A0
			public object Current
			{
				get
				{
					if (!this.IsValid)
					{
						throw new InvalidOperationException();
					}
					DataGridColumn dataGridColumn = this._columns[this._current];
					if (dataGridColumn != null)
					{
						return dataGridColumn.Header;
					}
					return null;
				}
			}

			// Token: 0x06008F75 RID: 36725 RVA: 0x003444D8 File Offset: 0x003434D8
			public bool MoveNext()
			{
				if (this.HasChanged)
				{
					throw new InvalidOperationException();
				}
				if (this._columns != null && this._current < this._columns.Count - 1)
				{
					this._current++;
					return true;
				}
				return false;
			}

			// Token: 0x06008F76 RID: 36726 RVA: 0x00344516 File Offset: 0x00343516
			public void Reset()
			{
				if (this.HasChanged)
				{
					throw new InvalidOperationException();
				}
				this._current = -1;
			}

			// Token: 0x06008F77 RID: 36727 RVA: 0x0034452D File Offset: 0x0034352D
			public void Dispose()
			{
				GC.SuppressFinalize(this);
				if (this._columns != null)
				{
					this._columns.CollectionChanged -= this.OnColumnsChanged;
				}
			}

			// Token: 0x17001F61 RID: 8033
			// (get) Token: 0x06008F78 RID: 36728 RVA: 0x00344554 File Offset: 0x00343554
			private bool HasChanged
			{
				get
				{
					return this._columnsChanged;
				}
			}

			// Token: 0x17001F62 RID: 8034
			// (get) Token: 0x06008F79 RID: 36729 RVA: 0x0034455C File Offset: 0x0034355C
			private bool IsValid
			{
				get
				{
					return this._columns != null && this._current >= 0 && this._current < this._columns.Count && !this.HasChanged;
				}
			}

			// Token: 0x06008F7A RID: 36730 RVA: 0x0034458D File Offset: 0x0034358D
			private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				this._columnsChanged = true;
			}

			// Token: 0x040049FB RID: 18939
			private int _current;

			// Token: 0x040049FC RID: 18940
			private bool _columnsChanged;

			// Token: 0x040049FD RID: 18941
			private ObservableCollection<DataGridColumn> _columns;
		}
	}
}
