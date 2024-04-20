using System;
using System.Windows.Controls;
using System.Windows.Media;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000692 RID: 1682
	public class TableColumn : FrameworkContentElement, IIndexedChild<Table>
	{
		// Token: 0x060053E1 RID: 21473 RVA: 0x0025CF7D File Offset: 0x0025BF7D
		public TableColumn()
		{
			this._parentIndex = -1;
		}

		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x060053E2 RID: 21474 RVA: 0x0025CF8C File Offset: 0x0025BF8C
		// (set) Token: 0x060053E3 RID: 21475 RVA: 0x0025CF9E File Offset: 0x0025BF9E
		public GridLength Width
		{
			get
			{
				return (GridLength)base.GetValue(TableColumn.WidthProperty);
			}
			set
			{
				base.SetValue(TableColumn.WidthProperty, value);
			}
		}

		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x060053E4 RID: 21476 RVA: 0x0025CFB1 File Offset: 0x0025BFB1
		// (set) Token: 0x060053E5 RID: 21477 RVA: 0x0025CFC3 File Offset: 0x0025BFC3
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(TableColumn.BackgroundProperty);
			}
			set
			{
				base.SetValue(TableColumn.BackgroundProperty, value);
			}
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x0025CFD1 File Offset: 0x0025BFD1
		void IIndexedChild<Table>.OnEnterParentTree()
		{
			this.OnEnterParentTree();
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x0025CFD9 File Offset: 0x0025BFD9
		void IIndexedChild<Table>.OnExitParentTree()
		{
			this.OnExitParentTree();
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IIndexedChild<Table>.OnAfterExitParentTree(Table parent)
		{
		}

		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x060053E9 RID: 21481 RVA: 0x0025CFE1 File Offset: 0x0025BFE1
		// (set) Token: 0x060053EA RID: 21482 RVA: 0x0025CFE9 File Offset: 0x0025BFE9
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

		// Token: 0x060053EB RID: 21483 RVA: 0x0025CFF2 File Offset: 0x0025BFF2
		internal void OnEnterParentTree()
		{
			this.Table.InvalidateColumns();
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x0025CFF2 File Offset: 0x0025BFF2
		internal void OnExitParentTree()
		{
			this.Table.InvalidateColumns();
		}

		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x060053ED RID: 21485 RVA: 0x0025CFFF File Offset: 0x0025BFFF
		internal Table Table
		{
			get
			{
				return base.Parent as Table;
			}
		}

		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x060053EE RID: 21486 RVA: 0x0025D00C File Offset: 0x0025C00C
		// (set) Token: 0x060053EF RID: 21487 RVA: 0x0025D014 File Offset: 0x0025C014
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

		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x060053F0 RID: 21488 RVA: 0x0025D01D File Offset: 0x0025C01D
		internal static GridLength DefaultWidth
		{
			get
			{
				return new GridLength(0.0, GridUnitType.Auto);
			}
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x0025D030 File Offset: 0x0025C030
		private static bool IsValidWidth(object value)
		{
			GridLength gridLength = (GridLength)value;
			if ((gridLength.GridUnitType == GridUnitType.Pixel || gridLength.GridUnitType == GridUnitType.Star) && gridLength.Value < 0.0)
			{
				return false;
			}
			double num = (double)Math.Min(1000000, 3500000);
			return gridLength.GridUnitType != GridUnitType.Pixel || gridLength.Value <= num;
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x0025D094 File Offset: 0x0025C094
		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Table table = ((TableColumn)d).Table;
			if (table != null)
			{
				table.InvalidateColumns();
			}
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x0025D094 File Offset: 0x0025C094
		private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Table table = ((TableColumn)d).Table;
			if (table != null)
			{
				table.InvalidateColumns();
			}
		}

		// Token: 0x04002EF2 RID: 12018
		private int _parentIndex;

		// Token: 0x04002EF3 RID: 12019
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(TableColumn), new FrameworkPropertyMetadata(new GridLength(0.0, GridUnitType.Auto), FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TableColumn.OnWidthChanged)), new ValidateValueCallback(TableColumn.IsValidWidth));

		// Token: 0x04002EF4 RID: 12020
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(TableColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(TableColumn.OnBackgroundChanged)));
	}
}
