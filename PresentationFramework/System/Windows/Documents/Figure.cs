using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x020005EF RID: 1519
	public class Figure : AnchoredBlock
	{
		// Token: 0x06004A0E RID: 18958 RVA: 0x002317A0 File Offset: 0x002307A0
		public Figure() : this(null)
		{
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x002317A9 File Offset: 0x002307A9
		public Figure(Block childBlock) : this(childBlock, null)
		{
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x002317B3 File Offset: 0x002307B3
		public Figure(Block childBlock, TextPointer insertionPosition) : base(childBlock, insertionPosition)
		{
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x06004A11 RID: 18961 RVA: 0x002317BD File Offset: 0x002307BD
		// (set) Token: 0x06004A12 RID: 18962 RVA: 0x002317CF File Offset: 0x002307CF
		public FigureHorizontalAnchor HorizontalAnchor
		{
			get
			{
				return (FigureHorizontalAnchor)base.GetValue(Figure.HorizontalAnchorProperty);
			}
			set
			{
				base.SetValue(Figure.HorizontalAnchorProperty, value);
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x06004A13 RID: 18963 RVA: 0x002317E2 File Offset: 0x002307E2
		// (set) Token: 0x06004A14 RID: 18964 RVA: 0x002317F4 File Offset: 0x002307F4
		public FigureVerticalAnchor VerticalAnchor
		{
			get
			{
				return (FigureVerticalAnchor)base.GetValue(Figure.VerticalAnchorProperty);
			}
			set
			{
				base.SetValue(Figure.VerticalAnchorProperty, value);
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06004A15 RID: 18965 RVA: 0x00231807 File Offset: 0x00230807
		// (set) Token: 0x06004A16 RID: 18966 RVA: 0x00231819 File Offset: 0x00230819
		[TypeConverter(typeof(LengthConverter))]
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(Figure.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(Figure.HorizontalOffsetProperty, value);
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06004A17 RID: 18967 RVA: 0x0023182C File Offset: 0x0023082C
		// (set) Token: 0x06004A18 RID: 18968 RVA: 0x0023183E File Offset: 0x0023083E
		[TypeConverter(typeof(LengthConverter))]
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(Figure.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(Figure.VerticalOffsetProperty, value);
			}
		}

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06004A19 RID: 18969 RVA: 0x00231851 File Offset: 0x00230851
		// (set) Token: 0x06004A1A RID: 18970 RVA: 0x00231863 File Offset: 0x00230863
		public bool CanDelayPlacement
		{
			get
			{
				return (bool)base.GetValue(Figure.CanDelayPlacementProperty);
			}
			set
			{
				base.SetValue(Figure.CanDelayPlacementProperty, value);
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x06004A1B RID: 18971 RVA: 0x00231871 File Offset: 0x00230871
		// (set) Token: 0x06004A1C RID: 18972 RVA: 0x00231883 File Offset: 0x00230883
		public WrapDirection WrapDirection
		{
			get
			{
				return (WrapDirection)base.GetValue(Figure.WrapDirectionProperty);
			}
			set
			{
				base.SetValue(Figure.WrapDirectionProperty, value);
			}
		}

		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06004A1D RID: 18973 RVA: 0x00231896 File Offset: 0x00230896
		// (set) Token: 0x06004A1E RID: 18974 RVA: 0x002318A8 File Offset: 0x002308A8
		public FigureLength Width
		{
			get
			{
				return (FigureLength)base.GetValue(Figure.WidthProperty);
			}
			set
			{
				base.SetValue(Figure.WidthProperty, value);
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06004A1F RID: 18975 RVA: 0x002318BB File Offset: 0x002308BB
		// (set) Token: 0x06004A20 RID: 18976 RVA: 0x002318CD File Offset: 0x002308CD
		public FigureLength Height
		{
			get
			{
				return (FigureLength)base.GetValue(Figure.HeightProperty);
			}
			set
			{
				base.SetValue(Figure.HeightProperty, value);
			}
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x002318E0 File Offset: 0x002308E0
		private static bool IsValidHorizontalAnchor(object o)
		{
			FigureHorizontalAnchor figureHorizontalAnchor = (FigureHorizontalAnchor)o;
			return figureHorizontalAnchor == FigureHorizontalAnchor.ContentCenter || figureHorizontalAnchor == FigureHorizontalAnchor.ContentLeft || figureHorizontalAnchor == FigureHorizontalAnchor.ContentRight || figureHorizontalAnchor == FigureHorizontalAnchor.PageCenter || figureHorizontalAnchor == FigureHorizontalAnchor.PageLeft || figureHorizontalAnchor == FigureHorizontalAnchor.PageRight || figureHorizontalAnchor == FigureHorizontalAnchor.ColumnCenter || figureHorizontalAnchor == FigureHorizontalAnchor.ColumnLeft || figureHorizontalAnchor == FigureHorizontalAnchor.ColumnRight;
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x0023191C File Offset: 0x0023091C
		private static bool IsValidVerticalAnchor(object o)
		{
			FigureVerticalAnchor figureVerticalAnchor = (FigureVerticalAnchor)o;
			return figureVerticalAnchor == FigureVerticalAnchor.ContentBottom || figureVerticalAnchor == FigureVerticalAnchor.ContentCenter || figureVerticalAnchor == FigureVerticalAnchor.ContentTop || figureVerticalAnchor == FigureVerticalAnchor.PageBottom || figureVerticalAnchor == FigureVerticalAnchor.PageCenter || figureVerticalAnchor == FigureVerticalAnchor.PageTop || figureVerticalAnchor == FigureVerticalAnchor.ParagraphTop;
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x00231950 File Offset: 0x00230950
		private static bool IsValidWrapDirection(object o)
		{
			WrapDirection wrapDirection = (WrapDirection)o;
			return wrapDirection == WrapDirection.Both || wrapDirection == WrapDirection.None || wrapDirection == WrapDirection.Left || wrapDirection == WrapDirection.Right;
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x00231978 File Offset: 0x00230978
		private static bool IsValidOffset(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			double num3 = -num2;
			return !double.IsNaN(num) && num >= num3 && num <= num2;
		}

		// Token: 0x040026D7 RID: 9943
		public static readonly DependencyProperty HorizontalAnchorProperty = DependencyProperty.Register("HorizontalAnchor", typeof(FigureHorizontalAnchor), typeof(Figure), new FrameworkPropertyMetadata(FigureHorizontalAnchor.ColumnRight, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidHorizontalAnchor));

		// Token: 0x040026D8 RID: 9944
		public static readonly DependencyProperty VerticalAnchorProperty = DependencyProperty.Register("VerticalAnchor", typeof(FigureVerticalAnchor), typeof(Figure), new FrameworkPropertyMetadata(FigureVerticalAnchor.ParagraphTop, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidVerticalAnchor));

		// Token: 0x040026D9 RID: 9945
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(Figure), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidOffset));

		// Token: 0x040026DA RID: 9946
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(Figure), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidOffset));

		// Token: 0x040026DB RID: 9947
		public static readonly DependencyProperty CanDelayPlacementProperty = DependencyProperty.Register("CanDelayPlacement", typeof(bool), typeof(Figure), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		// Token: 0x040026DC RID: 9948
		public static readonly DependencyProperty WrapDirectionProperty = DependencyProperty.Register("WrapDirection", typeof(WrapDirection), typeof(Figure), new FrameworkPropertyMetadata(WrapDirection.Both, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Figure.IsValidWrapDirection));

		// Token: 0x040026DD RID: 9949
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(FigureLength), typeof(Figure), new FrameworkPropertyMetadata(new FigureLength(1.0, FigureUnitType.Auto), FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x040026DE RID: 9950
		public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(FigureLength), typeof(Figure), new FrameworkPropertyMetadata(new FigureLength(1.0, FigureUnitType.Auto), FrameworkPropertyMetadataOptions.AffectsMeasure));
	}
}
