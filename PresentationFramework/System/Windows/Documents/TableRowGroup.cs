using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal.Documents;
using MS.Internal.PtsTable;

namespace System.Windows.Documents
{
	// Token: 0x02000696 RID: 1686
	[ContentProperty("Rows")]
	public class TableRowGroup : TextElement, IAddChild, IIndexedChild<Table>, IAcceptInsertion
	{
		// Token: 0x06005460 RID: 21600 RVA: 0x0025D8A0 File Offset: 0x0025C8A0
		public TableRowGroup()
		{
			this.Initialize();
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x0025D8AE File Offset: 0x0025C8AE
		private void Initialize()
		{
			this._rows = new TableRowCollection(this);
			this._rowInsertionIndex = -1;
			this._parentIndex = -1;
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x0025D8CC File Offset: 0x0025C8CC
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TableRow tableRow = value as TableRow;
			if (tableRow != null)
			{
				this.Rows.Add(tableRow);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableRow)
			}), "value");
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x06005464 RID: 21604 RVA: 0x0025D92E File Offset: 0x0025C92E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableRowCollection Rows
		{
			get
			{
				return this._rows;
			}
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x0025D936 File Offset: 0x0025C936
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeRows()
		{
			return this.Rows.Count > 0;
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x0025D946 File Offset: 0x0025C946
		void IIndexedChild<Table>.OnEnterParentTree()
		{
			this.OnEnterParentTree();
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x0025D94E File Offset: 0x0025C94E
		void IIndexedChild<Table>.OnExitParentTree()
		{
			this.OnExitParentTree();
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x0025D956 File Offset: 0x0025C956
		void IIndexedChild<Table>.OnAfterExitParentTree(Table parent)
		{
			this.OnAfterExitParentTree(parent);
		}

		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x06005469 RID: 21609 RVA: 0x0025D95F File Offset: 0x0025C95F
		// (set) Token: 0x0600546A RID: 21610 RVA: 0x0025D967 File Offset: 0x0025C967
		int IIndexedChild<Table>.Index
		{
			get
			{
				return this.Index;
			}
			set
			{
				this.Index = value;
			}
		}

		// Token: 0x0600546B RID: 21611 RVA: 0x0025D970 File Offset: 0x0025C970
		internal void OnEnterParentTree()
		{
			if (this.Table != null)
			{
				this.Table.OnStructureChanged();
			}
		}

		// Token: 0x0600546C RID: 21612 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void OnExitParentTree()
		{
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x0025D985 File Offset: 0x0025C985
		internal void OnAfterExitParentTree(Table table)
		{
			table.OnStructureChanged();
		}

		// Token: 0x0600546E RID: 21614 RVA: 0x0025D990 File Offset: 0x0025C990
		internal void ValidateStructure()
		{
			RowSpanVector rowSpanVector = new RowSpanVector();
			this._columnCount = 0;
			for (int i = 0; i < this.Rows.Count; i++)
			{
				this.Rows[i].ValidateStructure(rowSpanVector);
				this._columnCount = Math.Max(this._columnCount, this.Rows[i].ColumnCount);
			}
			this.Table.EnsureColumnCount(this._columnCount);
		}

		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x0600546F RID: 21615 RVA: 0x0025CFFF File Offset: 0x0025BFFF
		internal Table Table
		{
			get
			{
				return base.Parent as Table;
			}
		}

		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x06005470 RID: 21616 RVA: 0x0025DA05 File Offset: 0x0025CA05
		// (set) Token: 0x06005471 RID: 21617 RVA: 0x0025DA0D File Offset: 0x0025CA0D
		internal int Index
		{
			get
			{
				return this._parentIndex;
			}
			set
			{
				this._parentIndex = value;
			}
		}

		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x06005472 RID: 21618 RVA: 0x0025DA16 File Offset: 0x0025CA16
		// (set) Token: 0x06005473 RID: 21619 RVA: 0x0025DA1E File Offset: 0x0025CA1E
		int IAcceptInsertion.InsertionIndex
		{
			get
			{
				return this.InsertionIndex;
			}
			set
			{
				this.InsertionIndex = value;
			}
		}

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06005474 RID: 21620 RVA: 0x0025DA27 File Offset: 0x0025CA27
		// (set) Token: 0x06005475 RID: 21621 RVA: 0x0025DA2F File Offset: 0x0025CA2F
		internal int InsertionIndex
		{
			get
			{
				return this._rowInsertionIndex;
			}
			set
			{
				this._rowInsertionIndex = value;
			}
		}

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x06005476 RID: 21622 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x0025DA38 File Offset: 0x0025CA38
		internal override void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = base.Parent;
			if (newParent != null && !(newParent is Table))
			{
				throw new InvalidOperationException(SR.Get("TableInvalidParentNodeType", new object[]
				{
					newParent.GetType().ToString()
				}));
			}
			if (parent != null)
			{
				this.OnExitParentTree();
				((Table)parent).RowGroups.InternalRemove(this);
				this.OnAfterExitParentTree(parent as Table);
			}
			base.OnNewParent(newParent);
			if (newParent != null)
			{
				((Table)newParent).RowGroups.InternalAdd(this);
				this.OnEnterParentTree();
			}
		}

		// Token: 0x04002EFE RID: 12030
		private TableRowCollection _rows;

		// Token: 0x04002EFF RID: 12031
		private int _parentIndex;

		// Token: 0x04002F00 RID: 12032
		private int _rowInsertionIndex;

		// Token: 0x04002F01 RID: 12033
		private int _columnCount;
	}
}
