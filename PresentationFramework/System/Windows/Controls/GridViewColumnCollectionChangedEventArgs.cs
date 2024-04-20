using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x02000788 RID: 1928
	internal class GridViewColumnCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		// Token: 0x06006AB4 RID: 27316 RVA: 0x002C29B3 File Offset: 0x002C19B3
		internal GridViewColumnCollectionChangedEventArgs(GridViewColumn column, string propertyName) : base(NotifyCollectionChangedAction.Reset)
		{
			this._column = column;
			this._propertyName = propertyName;
		}

		// Token: 0x06006AB5 RID: 27317 RVA: 0x002C29D1 File Offset: 0x002C19D1
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn[] clearedColumns) : base(action)
		{
			this._clearedColumns = Array.AsReadOnly<GridViewColumn>(clearedColumns);
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x002C29ED File Offset: 0x002C19ED
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn changedItem, int index, int actualIndex) : base(action, changedItem, index)
		{
			this._actualIndex = actualIndex;
		}

		// Token: 0x06006AB7 RID: 27319 RVA: 0x002C2A07 File Offset: 0x002C1A07
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn newItem, GridViewColumn oldItem, int index, int actualIndex) : base(action, newItem, oldItem, index)
		{
			this._actualIndex = actualIndex;
		}

		// Token: 0x06006AB8 RID: 27320 RVA: 0x002C2A23 File Offset: 0x002C1A23
		internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn changedItem, int index, int oldIndex, int actualIndex) : base(action, changedItem, index, oldIndex)
		{
			this._actualIndex = actualIndex;
		}

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x06006AB9 RID: 27321 RVA: 0x002C2A3F File Offset: 0x002C1A3F
		internal int ActualIndex
		{
			get
			{
				return this._actualIndex;
			}
		}

		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x06006ABA RID: 27322 RVA: 0x002C2A47 File Offset: 0x002C1A47
		internal ReadOnlyCollection<GridViewColumn> ClearedColumns
		{
			get
			{
				return this._clearedColumns;
			}
		}

		// Token: 0x170018AB RID: 6315
		// (get) Token: 0x06006ABB RID: 27323 RVA: 0x002C2A4F File Offset: 0x002C1A4F
		internal GridViewColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x06006ABC RID: 27324 RVA: 0x002C2A57 File Offset: 0x002C1A57
		internal string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x04003562 RID: 13666
		private int _actualIndex = -1;

		// Token: 0x04003563 RID: 13667
		private ReadOnlyCollection<GridViewColumn> _clearedColumns;

		// Token: 0x04003564 RID: 13668
		private GridViewColumn _column;

		// Token: 0x04003565 RID: 13669
		private string _propertyName;
	}
}
