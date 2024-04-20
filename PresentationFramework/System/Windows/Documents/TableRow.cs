using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal.Documents;
using MS.Internal.PtsTable;

namespace System.Windows.Documents
{
	// Token: 0x02000694 RID: 1684
	[ContentProperty("Cells")]
	public class TableRow : TextElement, IAddChild, IIndexedChild<TableRowGroup>, IAcceptInsertion
	{
		// Token: 0x06005416 RID: 21526 RVA: 0x0025D354 File Offset: 0x0025C354
		public TableRow()
		{
			this.PrivateInitialize();
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x0025D364 File Offset: 0x0025C364
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TableCell tableCell = value as TableCell;
			if (tableCell != null)
			{
				this.Cells.Add(tableCell);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableCell)
			}), "value");
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x0025D3C8 File Offset: 0x0025C3C8
		internal override void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = base.Parent;
			if (newParent != null && !(newParent is TableRowGroup))
			{
				throw new InvalidOperationException(SR.Get("TableInvalidParentNodeType", new object[]
				{
					newParent.GetType().ToString()
				}));
			}
			if (parent != null)
			{
				((TableRowGroup)parent).Rows.InternalRemove(this);
			}
			base.OnNewParent(newParent);
			if (newParent != null)
			{
				((TableRowGroup)newParent).Rows.InternalAdd(this);
			}
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x0025D43A File Offset: 0x0025C43A
		void IIndexedChild<TableRowGroup>.OnEnterParentTree()
		{
			this.OnEnterParentTree();
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x0025D442 File Offset: 0x0025C442
		void IIndexedChild<TableRowGroup>.OnExitParentTree()
		{
			this.OnExitParentTree();
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x0025D44A File Offset: 0x0025C44A
		void IIndexedChild<TableRowGroup>.OnAfterExitParentTree(TableRowGroup parent)
		{
			this.OnAfterExitParentTree(parent);
		}

		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x0600541D RID: 21533 RVA: 0x0025D453 File Offset: 0x0025C453
		// (set) Token: 0x0600541E RID: 21534 RVA: 0x0025D45B File Offset: 0x0025C45B
		int IIndexedChild<TableRowGroup>.Index
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

		// Token: 0x0600541F RID: 21535 RVA: 0x0025D464 File Offset: 0x0025C464
		internal void OnEnterParentTree()
		{
			if (this.Table != null)
			{
				this.Table.OnStructureChanged();
			}
		}

		// Token: 0x06005420 RID: 21536 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void OnExitParentTree()
		{
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x0025D479 File Offset: 0x0025C479
		internal void OnAfterExitParentTree(TableRowGroup rowGroup)
		{
			if (rowGroup.Table != null)
			{
				this.Table.OnStructureChanged();
			}
		}

		// Token: 0x06005422 RID: 21538 RVA: 0x0025D490 File Offset: 0x0025C490
		internal void ValidateStructure(RowSpanVector rowSpanVector)
		{
			this.SetFlags(!rowSpanVector.Empty(), TableRow.Flags.HasForeignCells);
			this.SetFlags(false, TableRow.Flags.HasRealCells);
			this._formatCellCount = 0;
			this._columnCount = 0;
			int num;
			int num2;
			rowSpanVector.GetFirstAvailableRange(out num, out num2);
			for (int i = 0; i < this._cells.Count; i++)
			{
				TableCell tableCell = this._cells[i];
				int columnSpan = tableCell.ColumnSpan;
				int rowSpan = tableCell.RowSpan;
				while (num + columnSpan > num2)
				{
					rowSpanVector.GetNextAvailableRange(out num, out num2);
				}
				tableCell.ValidateStructure(num);
				if (rowSpan > 1)
				{
					rowSpanVector.Register(tableCell);
				}
				else
				{
					this._formatCellCount++;
				}
				num += columnSpan;
			}
			this._columnCount = num;
			bool flag = false;
			rowSpanVector.GetSpanCells(out this._spannedCells, out flag);
			if (this._formatCellCount > 0 || flag)
			{
				this.SetFlags(true, TableRow.Flags.HasRealCells);
			}
			this._formatCellCount += this._spannedCells.Length;
		}

		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x06005423 RID: 21539 RVA: 0x0025D583 File Offset: 0x0025C583
		internal TableRowGroup RowGroup
		{
			get
			{
				return base.Parent as TableRowGroup;
			}
		}

		// Token: 0x170013D4 RID: 5076
		// (get) Token: 0x06005424 RID: 21540 RVA: 0x0025D590 File Offset: 0x0025C590
		internal Table Table
		{
			get
			{
				if (this.RowGroup == null)
				{
					return null;
				}
				return this.RowGroup.Table;
			}
		}

		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x06005425 RID: 21541 RVA: 0x0025D5A7 File Offset: 0x0025C5A7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableCellCollection Cells
		{
			get
			{
				return this._cells;
			}
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x0025D5AF File Offset: 0x0025C5AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCells()
		{
			return this.Cells.Count > 0;
		}

		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x06005427 RID: 21543 RVA: 0x0025D5BF File Offset: 0x0025C5BF
		// (set) Token: 0x06005428 RID: 21544 RVA: 0x0025D5C7 File Offset: 0x0025C5C7
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

		// Token: 0x170013D7 RID: 5079
		// (get) Token: 0x06005429 RID: 21545 RVA: 0x0025D5D0 File Offset: 0x0025C5D0
		// (set) Token: 0x0600542A RID: 21546 RVA: 0x0025D5D8 File Offset: 0x0025C5D8
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

		// Token: 0x170013D8 RID: 5080
		// (get) Token: 0x0600542B RID: 21547 RVA: 0x0025D5E1 File Offset: 0x0025C5E1
		// (set) Token: 0x0600542C RID: 21548 RVA: 0x0025D5E9 File Offset: 0x0025C5E9
		internal int InsertionIndex
		{
			get
			{
				return this._cellInsertionIndex;
			}
			set
			{
				this._cellInsertionIndex = value;
			}
		}

		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x0600542D RID: 21549 RVA: 0x0025D5F2 File Offset: 0x0025C5F2
		internal TableCell[] SpannedCells
		{
			get
			{
				return this._spannedCells;
			}
		}

		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x0600542E RID: 21550 RVA: 0x0025D5FA File Offset: 0x0025C5FA
		internal int ColumnCount
		{
			get
			{
				return this._columnCount;
			}
		}

		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x0600542F RID: 21551 RVA: 0x0025D602 File Offset: 0x0025C602
		internal bool HasForeignCells
		{
			get
			{
				return this.CheckFlags(TableRow.Flags.HasForeignCells);
			}
		}

		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x06005430 RID: 21552 RVA: 0x0025D60C File Offset: 0x0025C60C
		internal bool HasRealCells
		{
			get
			{
				return this.CheckFlags(TableRow.Flags.HasRealCells);
			}
		}

		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x06005431 RID: 21553 RVA: 0x0025D616 File Offset: 0x0025C616
		internal int FormatCellCount
		{
			get
			{
				return this._formatCellCount;
			}
		}

		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x06005432 RID: 21554 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005433 RID: 21555 RVA: 0x0025D61E File Offset: 0x0025C61E
		private void PrivateInitialize()
		{
			this._cells = new TableCellCollection(this);
			this._parentIndex = -1;
			this._cellInsertionIndex = -1;
		}

		// Token: 0x06005434 RID: 21556 RVA: 0x0025D63A File Offset: 0x0025C63A
		private void SetFlags(bool value, TableRow.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x0025D658 File Offset: 0x0025C658
		private bool CheckFlags(TableRow.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x04002EF6 RID: 12022
		private TableCellCollection _cells;

		// Token: 0x04002EF7 RID: 12023
		private TableCell[] _spannedCells;

		// Token: 0x04002EF8 RID: 12024
		private int _parentIndex;

		// Token: 0x04002EF9 RID: 12025
		private int _cellInsertionIndex;

		// Token: 0x04002EFA RID: 12026
		private int _columnCount;

		// Token: 0x04002EFB RID: 12027
		private TableRow.Flags _flags;

		// Token: 0x04002EFC RID: 12028
		private int _formatCellCount;

		// Token: 0x02000B5D RID: 2909
		[Flags]
		private enum Flags
		{
			// Token: 0x040048AF RID: 18607
			HasForeignCells = 16,
			// Token: 0x040048B0 RID: 18608
			HasRealCells = 32
		}
	}
}
