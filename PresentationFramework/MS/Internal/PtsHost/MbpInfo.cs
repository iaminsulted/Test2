using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200012E RID: 302
	internal sealed class MbpInfo
	{
		// Token: 0x0600082F RID: 2095 RVA: 0x001142F8 File Offset: 0x001132F8
		internal static MbpInfo FromElement(DependencyObject o, double pixelsPerDip)
		{
			if (o is Block || o is AnchoredBlock || o is TableCell || o is ListItem)
			{
				MbpInfo mbpInfo = new MbpInfo((TextElement)o);
				double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(o);
				if (mbpInfo.IsMarginAuto)
				{
					MbpInfo.ResolveAutoMargin(mbpInfo, o, lineHeightValue);
				}
				if (mbpInfo.IsPaddingAuto)
				{
					MbpInfo.ResolveAutoPadding(mbpInfo, o, lineHeightValue, pixelsPerDip);
				}
				return mbpInfo;
			}
			return MbpInfo._empty;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00114360 File Offset: 0x00113360
		internal void MirrorMargin()
		{
			MbpInfo.ReverseFlowDirection(ref this._margin);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0011436D File Offset: 0x0011336D
		internal void MirrorBP()
		{
			MbpInfo.ReverseFlowDirection(ref this._border);
			MbpInfo.ReverseFlowDirection(ref this._padding);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00114388 File Offset: 0x00113388
		private static void ReverseFlowDirection(ref Thickness thickness)
		{
			double left = thickness.Left;
			thickness.Left = thickness.Right;
			thickness.Right = left;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x001143BB File Offset: 0x001133BB
		private MbpInfo()
		{
			this._margin = default(Thickness);
			this._border = default(Thickness);
			this._padding = default(Thickness);
			this._borderBrush = new SolidColorBrush();
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x001143F4 File Offset: 0x001133F4
		private MbpInfo(TextElement block)
		{
			this._margin = (Thickness)block.GetValue(Block.MarginProperty);
			this._border = (Thickness)block.GetValue(Block.BorderThicknessProperty);
			this._padding = (Thickness)block.GetValue(Block.PaddingProperty);
			this._borderBrush = (Brush)block.GetValue(Block.BorderBrushProperty);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00114460 File Offset: 0x00113460
		private static void ResolveAutoMargin(MbpInfo mbp, DependencyObject o, double lineHeight)
		{
			Thickness thickness;
			if (o is Paragraph)
			{
				DependencyObject parent = ((Paragraph)o).Parent;
				if (parent is ListItem || parent is TableCell || parent is AnchoredBlock)
				{
					thickness = new Thickness(0.0);
				}
				else
				{
					thickness = new Thickness(0.0, lineHeight, 0.0, lineHeight);
				}
			}
			else if (o is Table || o is List)
			{
				thickness = new Thickness(0.0, lineHeight, 0.0, lineHeight);
			}
			else if (o is Figure || o is Floater)
			{
				thickness = new Thickness(0.5 * lineHeight);
			}
			else
			{
				thickness = new Thickness(0.0);
			}
			mbp.Margin = new Thickness(double.IsNaN(mbp.Margin.Left) ? thickness.Left : mbp.Margin.Left, double.IsNaN(mbp.Margin.Top) ? thickness.Top : mbp.Margin.Top, double.IsNaN(mbp.Margin.Right) ? thickness.Right : mbp.Margin.Right, double.IsNaN(mbp.Margin.Bottom) ? thickness.Bottom : mbp.Margin.Bottom);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x001145E4 File Offset: 0x001135E4
		private static void ResolveAutoPadding(MbpInfo mbp, DependencyObject o, double lineHeight, double pixelsPerDip)
		{
			Thickness thickness;
			if (o is Figure || o is Floater)
			{
				thickness = new Thickness(0.5 * lineHeight);
			}
			else if (o is List)
			{
				thickness = ListMarkerSourceInfo.CalculatePadding((List)o, lineHeight, pixelsPerDip);
			}
			else
			{
				thickness = new Thickness(0.0);
			}
			mbp.Padding = new Thickness(double.IsNaN(mbp.Padding.Left) ? thickness.Left : mbp.Padding.Left, double.IsNaN(mbp.Padding.Top) ? thickness.Top : mbp.Padding.Top, double.IsNaN(mbp.Padding.Right) ? thickness.Right : mbp.Padding.Right, double.IsNaN(mbp.Padding.Bottom) ? thickness.Bottom : mbp.Padding.Bottom);
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x001146F8 File Offset: 0x001136F8
		internal int MBPLeft
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Left) + TextDpi.ToTextDpi(this._border.Left) + TextDpi.ToTextDpi(this._padding.Left);
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x0011472C File Offset: 0x0011372C
		internal int MBPRight
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Right) + TextDpi.ToTextDpi(this._border.Right) + TextDpi.ToTextDpi(this._padding.Right);
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x00114760 File Offset: 0x00113760
		internal int MBPTop
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Top) + TextDpi.ToTextDpi(this._border.Top) + TextDpi.ToTextDpi(this._padding.Top);
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x00114794 File Offset: 0x00113794
		internal int MBPBottom
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Bottom) + TextDpi.ToTextDpi(this._border.Bottom) + TextDpi.ToTextDpi(this._padding.Bottom);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x001147C8 File Offset: 0x001137C8
		internal int BPLeft
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Left) + TextDpi.ToTextDpi(this._padding.Left);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x001147EB File Offset: 0x001137EB
		internal int BPRight
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Right) + TextDpi.ToTextDpi(this._padding.Right);
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0011480E File Offset: 0x0011380E
		internal int BPTop
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Top) + TextDpi.ToTextDpi(this._padding.Top);
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x00114831 File Offset: 0x00113831
		internal int BPBottom
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Bottom) + TextDpi.ToTextDpi(this._padding.Bottom);
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x00114854 File Offset: 0x00113854
		internal int BorderLeft
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Left);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x00114866 File Offset: 0x00113866
		internal int BorderRight
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Right);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x00114878 File Offset: 0x00113878
		internal int BorderTop
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Top);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0011488A File Offset: 0x0011388A
		internal int BorderBottom
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Bottom);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x0011489C File Offset: 0x0011389C
		internal int MarginLeft
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Left);
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x001148AE File Offset: 0x001138AE
		internal int MarginRight
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Right);
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x001148C0 File Offset: 0x001138C0
		internal int MarginTop
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Top);
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x001148D2 File Offset: 0x001138D2
		internal int MarginBottom
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Bottom);
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x001148E4 File Offset: 0x001138E4
		// (set) Token: 0x06000849 RID: 2121 RVA: 0x001148EC File Offset: 0x001138EC
		internal Thickness Margin
		{
			get
			{
				return this._margin;
			}
			set
			{
				this._margin = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x001148F5 File Offset: 0x001138F5
		// (set) Token: 0x0600084B RID: 2123 RVA: 0x001148FD File Offset: 0x001138FD
		internal Thickness Border
		{
			get
			{
				return this._border;
			}
			set
			{
				this._border = value;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x00114906 File Offset: 0x00113906
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x0011490E File Offset: 0x0011390E
		internal Thickness Padding
		{
			get
			{
				return this._padding;
			}
			set
			{
				this._padding = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x00114917 File Offset: 0x00113917
		internal Brush BorderBrush
		{
			get
			{
				return this._borderBrush;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x00114920 File Offset: 0x00113920
		private bool IsPaddingAuto
		{
			get
			{
				return double.IsNaN(this._padding.Left) || double.IsNaN(this._padding.Right) || double.IsNaN(this._padding.Top) || double.IsNaN(this._padding.Bottom);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x00114978 File Offset: 0x00113978
		private bool IsMarginAuto
		{
			get
			{
				return double.IsNaN(this._margin.Left) || double.IsNaN(this._margin.Right) || double.IsNaN(this._margin.Top) || double.IsNaN(this._margin.Bottom);
			}
		}

		// Token: 0x040007AE RID: 1966
		private Thickness _margin;

		// Token: 0x040007AF RID: 1967
		private Thickness _border;

		// Token: 0x040007B0 RID: 1968
		private Thickness _padding;

		// Token: 0x040007B1 RID: 1969
		private Brush _borderBrush;

		// Token: 0x040007B2 RID: 1970
		private static MbpInfo _empty = new MbpInfo();
	}
}
