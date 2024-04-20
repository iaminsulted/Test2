using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000111 RID: 273
	internal sealed class ColumnPropertiesGroup
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x0010A750 File Offset: 0x00109750
		internal ColumnPropertiesGroup(DependencyObject o)
		{
			this._columnWidth = (double)o.GetValue(FlowDocument.ColumnWidthProperty);
			this._columnGap = (double)o.GetValue(FlowDocument.ColumnGapProperty);
			this._columnRuleWidth = (double)o.GetValue(FlowDocument.ColumnRuleWidthProperty);
			this._columnRuleBrush = (Brush)o.GetValue(FlowDocument.ColumnRuleBrushProperty);
			this._isColumnWidthFlexible = (bool)o.GetValue(FlowDocument.IsColumnWidthFlexibleProperty);
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0010A7D1 File Offset: 0x001097D1
		internal double ColumnWidth
		{
			get
			{
				return this._columnWidth;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0010A7D9 File Offset: 0x001097D9
		internal bool IsColumnWidthFlexible
		{
			get
			{
				return this._isColumnWidthFlexible;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0010A7E1 File Offset: 0x001097E1
		internal ColumnSpaceDistribution ColumnSpaceDistribution
		{
			get
			{
				return ColumnSpaceDistribution.Between;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0010A7E4 File Offset: 0x001097E4
		internal double ColumnGap
		{
			get
			{
				Invariant.Assert(!double.IsNaN(this._columnGap));
				return this._columnGap;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0010A7FF File Offset: 0x001097FF
		internal Brush ColumnRuleBrush
		{
			get
			{
				return this._columnRuleBrush;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0010A807 File Offset: 0x00109807
		internal double ColumnRuleWidth
		{
			get
			{
				return this._columnRuleWidth;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0010A80F File Offset: 0x0010980F
		internal bool ColumnWidthAuto
		{
			get
			{
				return DoubleUtil.IsNaN(this._columnWidth);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0010A81C File Offset: 0x0010981C
		internal bool ColumnGapAuto
		{
			get
			{
				return DoubleUtil.IsNaN(this._columnGap);
			}
		}

		// Token: 0x04000734 RID: 1844
		private double _columnWidth;

		// Token: 0x04000735 RID: 1845
		private bool _isColumnWidthFlexible;

		// Token: 0x04000736 RID: 1846
		private double _columnGap;

		// Token: 0x04000737 RID: 1847
		private Brush _columnRuleBrush;

		// Token: 0x04000738 RID: 1848
		private double _columnRuleWidth;
	}
}
