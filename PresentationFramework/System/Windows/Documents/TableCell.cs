using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000690 RID: 1680
	[ContentProperty("Blocks")]
	public class TableCell : TextElement, IIndexedChild<TableRow>
	{
		// Token: 0x06005388 RID: 21384 RVA: 0x0025C7F2 File Offset: 0x0025B7F2
		public TableCell()
		{
			this.PrivateInitialize();
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x0025C800 File Offset: 0x0025B800
		public TableCell(Block blockItem)
		{
			this.PrivateInitialize();
			if (blockItem == null)
			{
				throw new ArgumentNullException("blockItem");
			}
			this.Blocks.Add(blockItem);
		}

		// Token: 0x0600538A RID: 21386 RVA: 0x0025C828 File Offset: 0x0025B828
		internal override void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = base.Parent;
			TableRow tableRow = newParent as TableRow;
			if (newParent != null && tableRow == null)
			{
				throw new InvalidOperationException(SR.Get("TableInvalidParentNodeType", new object[]
				{
					newParent.GetType().ToString()
				}));
			}
			if (parent != null)
			{
				((TableRow)parent).Cells.InternalRemove(this);
			}
			base.OnNewParent(newParent);
			if (tableRow != null && tableRow.Cells != null)
			{
				tableRow.Cells.InternalAdd(this);
			}
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x0600538B RID: 21387 RVA: 0x0022BE7C File Offset: 0x0022AE7C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x0600538C RID: 21388 RVA: 0x0025C89F File Offset: 0x0025B89F
		// (set) Token: 0x0600538D RID: 21389 RVA: 0x0025C8B1 File Offset: 0x0025B8B1
		public int ColumnSpan
		{
			get
			{
				return (int)base.GetValue(TableCell.ColumnSpanProperty);
			}
			set
			{
				base.SetValue(TableCell.ColumnSpanProperty, value);
			}
		}

		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x0600538E RID: 21390 RVA: 0x0025C8C4 File Offset: 0x0025B8C4
		// (set) Token: 0x0600538F RID: 21391 RVA: 0x0025C8D6 File Offset: 0x0025B8D6
		public int RowSpan
		{
			get
			{
				return (int)base.GetValue(TableCell.RowSpanProperty);
			}
			set
			{
				base.SetValue(TableCell.RowSpanProperty, value);
			}
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x0025C8E9 File Offset: 0x0025B8E9
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TableCellAutomationPeer(this);
		}

		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x06005391 RID: 21393 RVA: 0x0025C8F1 File Offset: 0x0025B8F1
		// (set) Token: 0x06005392 RID: 21394 RVA: 0x0025C903 File Offset: 0x0025B903
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(TableCell.PaddingProperty);
			}
			set
			{
				base.SetValue(TableCell.PaddingProperty, value);
			}
		}

		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x06005393 RID: 21395 RVA: 0x0025C916 File Offset: 0x0025B916
		// (set) Token: 0x06005394 RID: 21396 RVA: 0x0025C928 File Offset: 0x0025B928
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(TableCell.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(TableCell.BorderThicknessProperty, value);
			}
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x06005395 RID: 21397 RVA: 0x0025C93B File Offset: 0x0025B93B
		// (set) Token: 0x06005396 RID: 21398 RVA: 0x0025C94D File Offset: 0x0025B94D
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(TableCell.BorderBrushProperty);
			}
			set
			{
				base.SetValue(TableCell.BorderBrushProperty, value);
			}
		}

		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x06005397 RID: 21399 RVA: 0x0025C95B File Offset: 0x0025B95B
		// (set) Token: 0x06005398 RID: 21400 RVA: 0x0025C96D File Offset: 0x0025B96D
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(TableCell.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(TableCell.TextAlignmentProperty, value);
			}
		}

		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x06005399 RID: 21401 RVA: 0x0025C980 File Offset: 0x0025B980
		// (set) Token: 0x0600539A RID: 21402 RVA: 0x0025C992 File Offset: 0x0025B992
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(TableCell.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(TableCell.FlowDirectionProperty, value);
			}
		}

		// Token: 0x170013AF RID: 5039
		// (get) Token: 0x0600539B RID: 21403 RVA: 0x0025C9A5 File Offset: 0x0025B9A5
		// (set) Token: 0x0600539C RID: 21404 RVA: 0x0025C9B7 File Offset: 0x0025B9B7
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(TableCell.LineHeightProperty);
			}
			set
			{
				base.SetValue(TableCell.LineHeightProperty, value);
			}
		}

		// Token: 0x170013B0 RID: 5040
		// (get) Token: 0x0600539D RID: 21405 RVA: 0x0025C9CA File Offset: 0x0025B9CA
		// (set) Token: 0x0600539E RID: 21406 RVA: 0x0025C9DC File Offset: 0x0025B9DC
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(TableCell.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(TableCell.LineStackingStrategyProperty, value);
			}
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x0025C9EF File Offset: 0x0025B9EF
		void IIndexedChild<TableRow>.OnEnterParentTree()
		{
			this.OnEnterParentTree();
		}

		// Token: 0x060053A0 RID: 21408 RVA: 0x0025C9F7 File Offset: 0x0025B9F7
		void IIndexedChild<TableRow>.OnExitParentTree()
		{
			this.OnExitParentTree();
		}

		// Token: 0x060053A1 RID: 21409 RVA: 0x0025C9FF File Offset: 0x0025B9FF
		void IIndexedChild<TableRow>.OnAfterExitParentTree(TableRow parent)
		{
			this.OnAfterExitParentTree(parent);
		}

		// Token: 0x170013B1 RID: 5041
		// (get) Token: 0x060053A2 RID: 21410 RVA: 0x0025CA08 File Offset: 0x0025BA08
		// (set) Token: 0x060053A3 RID: 21411 RVA: 0x0025CA10 File Offset: 0x0025BA10
		int IIndexedChild<TableRow>.Index
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

		// Token: 0x060053A4 RID: 21412 RVA: 0x0025CA19 File Offset: 0x0025BA19
		internal void OnEnterParentTree()
		{
			if (this.Table != null)
			{
				this.Table.OnStructureChanged();
			}
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void OnExitParentTree()
		{
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x0025CA2E File Offset: 0x0025BA2E
		internal void OnAfterExitParentTree(TableRow row)
		{
			if (row.Table != null)
			{
				row.Table.OnStructureChanged();
			}
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x0025CA43 File Offset: 0x0025BA43
		internal void ValidateStructure(int columnIndex)
		{
			Invariant.Assert(columnIndex >= 0);
			this._columnIndex = columnIndex;
		}

		// Token: 0x170013B2 RID: 5042
		// (get) Token: 0x060053A8 RID: 21416 RVA: 0x0025CA58 File Offset: 0x0025BA58
		internal TableRow Row
		{
			get
			{
				return base.Parent as TableRow;
			}
		}

		// Token: 0x170013B3 RID: 5043
		// (get) Token: 0x060053A9 RID: 21417 RVA: 0x0025CA65 File Offset: 0x0025BA65
		internal Table Table
		{
			get
			{
				if (this.Row == null)
				{
					return null;
				}
				return this.Row.Table;
			}
		}

		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x060053AA RID: 21418 RVA: 0x0025CA7C File Offset: 0x0025BA7C
		// (set) Token: 0x060053AB RID: 21419 RVA: 0x0025CA84 File Offset: 0x0025BA84
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

		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x060053AC RID: 21420 RVA: 0x0025CA8D File Offset: 0x0025BA8D
		internal int RowIndex
		{
			get
			{
				return this.Row.Index;
			}
		}

		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x060053AD RID: 21421 RVA: 0x0025CA9A File Offset: 0x0025BA9A
		internal int RowGroupIndex
		{
			get
			{
				return this.Row.RowGroup.Index;
			}
		}

		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x060053AE RID: 21422 RVA: 0x0025CAAC File Offset: 0x0025BAAC
		// (set) Token: 0x060053AF RID: 21423 RVA: 0x0025CAB4 File Offset: 0x0025BAB4
		internal int ColumnIndex
		{
			get
			{
				return this._columnIndex;
			}
			set
			{
				this._columnIndex = value;
			}
		}

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x060053B0 RID: 21424 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x0025CABD File Offset: 0x0025BABD
		private void PrivateInitialize()
		{
			this._parentIndex = -1;
			this._columnIndex = -1;
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x0025CAD0 File Offset: 0x0025BAD0
		private static bool IsValidRowSpan(object value)
		{
			int num = (int)value;
			return num >= 1 && num <= 1000000;
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x0025CAF8 File Offset: 0x0025BAF8
		private static bool IsValidColumnSpan(object value)
		{
			int num = (int)value;
			return num >= 1 && num <= 1000;
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x0025CB20 File Offset: 0x0025BB20
		private static void OnColumnSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TableCell tableCell = (TableCell)d;
			if (tableCell.Table != null)
			{
				tableCell.Table.OnStructureChanged();
			}
			TableCellAutomationPeer tableCellAutomationPeer = ContentElementAutomationPeer.FromElement(tableCell) as TableCellAutomationPeer;
			if (tableCellAutomationPeer != null)
			{
				tableCellAutomationPeer.OnColumnSpanChanged((int)e.OldValue, (int)e.NewValue);
			}
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x0025CB74 File Offset: 0x0025BB74
		private static void OnRowSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TableCell tableCell = (TableCell)d;
			if (tableCell.Table != null)
			{
				tableCell.Table.OnStructureChanged();
			}
			TableCellAutomationPeer tableCellAutomationPeer = ContentElementAutomationPeer.FromElement(tableCell) as TableCellAutomationPeer;
			if (tableCellAutomationPeer != null)
			{
				tableCellAutomationPeer.OnRowSpanChanged((int)e.OldValue, (int)e.NewValue);
			}
		}

		// Token: 0x04002EE6 RID: 12006
		public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(TableCell), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x04002EE7 RID: 12007
		public static readonly DependencyProperty BorderThicknessProperty = Block.BorderThicknessProperty.AddOwner(typeof(TableCell), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x04002EE8 RID: 12008
		public static readonly DependencyProperty BorderBrushProperty = Block.BorderBrushProperty.AddOwner(typeof(TableCell), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002EE9 RID: 12009
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TableCell));

		// Token: 0x04002EEA RID: 12010
		public static readonly DependencyProperty FlowDirectionProperty = Block.FlowDirectionProperty.AddOwner(typeof(TableCell));

		// Token: 0x04002EEB RID: 12011
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(TableCell));

		// Token: 0x04002EEC RID: 12012
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(TableCell));

		// Token: 0x04002EED RID: 12013
		private int _parentIndex;

		// Token: 0x04002EEE RID: 12014
		private int _columnIndex;

		// Token: 0x04002EEF RID: 12015
		public static readonly DependencyProperty ColumnSpanProperty = DependencyProperty.Register("ColumnSpan", typeof(int), typeof(TableCell), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TableCell.OnColumnSpanChanged)), new ValidateValueCallback(TableCell.IsValidColumnSpan));

		// Token: 0x04002EF0 RID: 12016
		public static readonly DependencyProperty RowSpanProperty = DependencyProperty.Register("RowSpan", typeof(int), typeof(TableCell), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TableCell.OnRowSpanChanged)), new ValidateValueCallback(TableCell.IsValidRowSpan));
	}
}
