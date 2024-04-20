using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000594 RID: 1428
	public class TableAutomationPeer : TextElementAutomationPeer, IGridProvider
	{
		// Token: 0x06004598 RID: 17816 RVA: 0x002242A7 File Offset: 0x002232A7
		public TableAutomationPeer(Table owner) : base(owner)
		{
			this._rowCount = this.GetRowCount();
			this._columnCount = this.GetColumnCount();
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x002242C8 File Offset: 0x002232C8
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Grid)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x001FC0DC File Offset: 0x001FB0DC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Table;
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x002242D7 File Offset: 0x002232D7
		protected override string GetClassNameCore()
		{
			return "Table";
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x00221440 File Offset: 0x00220440
		protected override bool IsControlElementCore()
		{
			if (!base.IncludeInvisibleElementsInControlView)
			{
				bool? isTextViewVisible = base.IsTextViewVisible;
				bool flag = true;
				return isTextViewVisible.GetValueOrDefault() == flag & isTextViewVisible != null;
			}
			return true;
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x002242E0 File Offset: 0x002232E0
		internal void OnStructureInvalidated()
		{
			int rowCount = this.GetRowCount();
			if (rowCount != this._rowCount)
			{
				base.RaisePropertyChangedEvent(GridPatternIdentifiers.RowCountProperty, this._rowCount, rowCount);
				this._rowCount = rowCount;
			}
			int columnCount = this.GetColumnCount();
			if (columnCount != this._columnCount)
			{
				base.RaisePropertyChangedEvent(GridPatternIdentifiers.ColumnCountProperty, this._columnCount, columnCount);
				this._columnCount = columnCount;
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x00224354 File Offset: 0x00223354
		private int GetRowCount()
		{
			int num = 0;
			foreach (TableRowGroup tableRowGroup in ((IEnumerable<TableRowGroup>)((Table)base.Owner).RowGroups))
			{
				num += tableRowGroup.Rows.Count;
			}
			return num;
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x002243B8 File Offset: 0x002233B8
		private int GetColumnCount()
		{
			return ((Table)base.Owner).ColumnCount;
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x002243CC File Offset: 0x002233CC
		IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
		{
			if (row < 0 || row >= ((IGridProvider)this).RowCount)
			{
				throw new ArgumentOutOfRangeException("row");
			}
			if (column < 0 || column >= ((IGridProvider)this).ColumnCount)
			{
				throw new ArgumentOutOfRangeException("column");
			}
			int num = 0;
			foreach (TableRowGroup tableRowGroup in ((IEnumerable<TableRowGroup>)((Table)base.Owner).RowGroups))
			{
				if (num + tableRowGroup.Rows.Count < row)
				{
					num += tableRowGroup.Rows.Count;
				}
				else
				{
					foreach (TableRow tableRow in ((IEnumerable<TableRow>)tableRowGroup.Rows))
					{
						if (num == row)
						{
							foreach (TableCell tableCell in ((IEnumerable<TableCell>)tableRow.Cells))
							{
								if (tableCell.ColumnIndex <= column && tableCell.ColumnIndex + tableCell.ColumnSpan > column)
								{
									return base.ProviderFromPeer(ContentElementAutomationPeer.CreatePeerForElement(tableCell));
								}
							}
							foreach (TableCell tableCell2 in tableRow.SpannedCells)
							{
								if (tableCell2.ColumnIndex <= column && tableCell2.ColumnIndex + tableCell2.ColumnSpan > column)
								{
									return base.ProviderFromPeer(ContentElementAutomationPeer.CreatePeerForElement(tableCell2));
								}
							}
						}
						else
						{
							num++;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060045A1 RID: 17825 RVA: 0x002245A4 File Offset: 0x002235A4
		int IGridProvider.RowCount
		{
			get
			{
				return this._rowCount;
			}
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x060045A2 RID: 17826 RVA: 0x002245AC File Offset: 0x002235AC
		int IGridProvider.ColumnCount
		{
			get
			{
				return this._columnCount;
			}
		}

		// Token: 0x04002542 RID: 9538
		private int _rowCount;

		// Token: 0x04002543 RID: 9539
		private int _columnCount;
	}
}
