using System;
using System.ComponentModel;
using System.Windows.Media;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	// Token: 0x020005DA RID: 1498
	public abstract class Block : TextElement
	{
		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06004850 RID: 18512 RVA: 0x0022C09E File Offset: 0x0022B09E
		public BlockCollection SiblingBlocks
		{
			get
			{
				if (base.Parent == null)
				{
					return null;
				}
				return new BlockCollection(this, false);
			}
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06004851 RID: 18513 RVA: 0x0022C0B1 File Offset: 0x0022B0B1
		public Block NextBlock
		{
			get
			{
				return base.NextElement as Block;
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06004852 RID: 18514 RVA: 0x0022C0BE File Offset: 0x0022B0BE
		public Block PreviousBlock
		{
			get
			{
				return base.PreviousElement as Block;
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06004853 RID: 18515 RVA: 0x0022C0CB File Offset: 0x0022B0CB
		// (set) Token: 0x06004854 RID: 18516 RVA: 0x0022C0DD File Offset: 0x0022B0DD
		public bool IsHyphenationEnabled
		{
			get
			{
				return (bool)base.GetValue(Block.IsHyphenationEnabledProperty);
			}
			set
			{
				base.SetValue(Block.IsHyphenationEnabledProperty, value);
			}
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x0022C0EB File Offset: 0x0022B0EB
		public static void SetIsHyphenationEnabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Block.IsHyphenationEnabledProperty, value);
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x0022C107 File Offset: 0x0022B107
		public static bool GetIsHyphenationEnabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Block.IsHyphenationEnabledProperty);
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06004857 RID: 18519 RVA: 0x0022C127 File Offset: 0x0022B127
		// (set) Token: 0x06004858 RID: 18520 RVA: 0x0022C139 File Offset: 0x0022B139
		public Thickness Margin
		{
			get
			{
				return (Thickness)base.GetValue(Block.MarginProperty);
			}
			set
			{
				base.SetValue(Block.MarginProperty, value);
			}
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06004859 RID: 18521 RVA: 0x0022C14C File Offset: 0x0022B14C
		// (set) Token: 0x0600485A RID: 18522 RVA: 0x0022C15E File Offset: 0x0022B15E
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(Block.PaddingProperty);
			}
			set
			{
				base.SetValue(Block.PaddingProperty, value);
			}
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x0600485B RID: 18523 RVA: 0x0022C171 File Offset: 0x0022B171
		// (set) Token: 0x0600485C RID: 18524 RVA: 0x0022C183 File Offset: 0x0022B183
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(Block.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(Block.BorderThicknessProperty, value);
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x0600485D RID: 18525 RVA: 0x0022C196 File Offset: 0x0022B196
		// (set) Token: 0x0600485E RID: 18526 RVA: 0x0022C1A8 File Offset: 0x0022B1A8
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(Block.BorderBrushProperty);
			}
			set
			{
				base.SetValue(Block.BorderBrushProperty, value);
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x0600485F RID: 18527 RVA: 0x0022C1B6 File Offset: 0x0022B1B6
		// (set) Token: 0x06004860 RID: 18528 RVA: 0x0022C1C8 File Offset: 0x0022B1C8
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(Block.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(Block.TextAlignmentProperty, value);
			}
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x0022C1DB File Offset: 0x0022B1DB
		public static void SetTextAlignment(DependencyObject element, TextAlignment value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Block.TextAlignmentProperty, value);
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x0022C1FC File Offset: 0x0022B1FC
		public static TextAlignment GetTextAlignment(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextAlignment)element.GetValue(Block.TextAlignmentProperty);
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06004863 RID: 18531 RVA: 0x0022C21C File Offset: 0x0022B21C
		// (set) Token: 0x06004864 RID: 18532 RVA: 0x0022C22E File Offset: 0x0022B22E
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(Block.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(Block.FlowDirectionProperty, value);
			}
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06004865 RID: 18533 RVA: 0x0022C241 File Offset: 0x0022B241
		// (set) Token: 0x06004866 RID: 18534 RVA: 0x0022C253 File Offset: 0x0022B253
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(Block.LineHeightProperty);
			}
			set
			{
				base.SetValue(Block.LineHeightProperty, value);
			}
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x0022C266 File Offset: 0x0022B266
		public static void SetLineHeight(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Block.LineHeightProperty, value);
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x0022C287 File Offset: 0x0022B287
		[TypeConverter(typeof(LengthConverter))]
		public static double GetLineHeight(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(Block.LineHeightProperty);
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06004869 RID: 18537 RVA: 0x0022C2A7 File Offset: 0x0022B2A7
		// (set) Token: 0x0600486A RID: 18538 RVA: 0x0022C2B9 File Offset: 0x0022B2B9
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(Block.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(Block.LineStackingStrategyProperty, value);
			}
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x0022C2CC File Offset: 0x0022B2CC
		public static void SetLineStackingStrategy(DependencyObject element, LineStackingStrategy value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Block.LineStackingStrategyProperty, value);
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x0022C2ED File Offset: 0x0022B2ED
		public static LineStackingStrategy GetLineStackingStrategy(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (LineStackingStrategy)element.GetValue(Block.LineStackingStrategyProperty);
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x0600486D RID: 18541 RVA: 0x0022C30D File Offset: 0x0022B30D
		// (set) Token: 0x0600486E RID: 18542 RVA: 0x0022C31F File Offset: 0x0022B31F
		public bool BreakPageBefore
		{
			get
			{
				return (bool)base.GetValue(Block.BreakPageBeforeProperty);
			}
			set
			{
				base.SetValue(Block.BreakPageBeforeProperty, value);
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x0600486F RID: 18543 RVA: 0x0022C32D File Offset: 0x0022B32D
		// (set) Token: 0x06004870 RID: 18544 RVA: 0x0022C33F File Offset: 0x0022B33F
		public bool BreakColumnBefore
		{
			get
			{
				return (bool)base.GetValue(Block.BreakColumnBeforeProperty);
			}
			set
			{
				base.SetValue(Block.BreakColumnBeforeProperty, value);
			}
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06004871 RID: 18545 RVA: 0x0022C34D File Offset: 0x0022B34D
		// (set) Token: 0x06004872 RID: 18546 RVA: 0x0022C35F File Offset: 0x0022B35F
		public WrapDirection ClearFloaters
		{
			get
			{
				return (WrapDirection)base.GetValue(Block.ClearFloatersProperty);
			}
			set
			{
				base.SetValue(Block.ClearFloatersProperty, value);
			}
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x0022C372 File Offset: 0x0022B372
		internal static bool IsValidMargin(object o)
		{
			return Block.IsValidThickness((Thickness)o, true);
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x0022C372 File Offset: 0x0022B372
		internal static bool IsValidPadding(object o)
		{
			return Block.IsValidThickness((Thickness)o, true);
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x0022C380 File Offset: 0x0022B380
		internal static bool IsValidBorderThickness(object o)
		{
			return Block.IsValidThickness((Thickness)o, false);
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06004876 RID: 18550 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x0022C390 File Offset: 0x0022B390
		private static bool IsValidLineHeight(object o)
		{
			double num = (double)o;
			double minWidth = TextDpi.MinWidth;
			double num2 = (double)Math.Min(1000000, 160000);
			return double.IsNaN(num) || (num >= minWidth && num <= num2);
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x0022C3D4 File Offset: 0x0022B3D4
		private static bool IsValidLineStackingStrategy(object o)
		{
			LineStackingStrategy lineStackingStrategy = (LineStackingStrategy)o;
			return lineStackingStrategy == LineStackingStrategy.MaxHeight || lineStackingStrategy == LineStackingStrategy.BlockLineHeight;
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x0022C3F4 File Offset: 0x0022B3F4
		private static bool IsValidTextAlignment(object o)
		{
			TextAlignment textAlignment = (TextAlignment)o;
			return textAlignment == TextAlignment.Center || textAlignment == TextAlignment.Justify || textAlignment == TextAlignment.Left || textAlignment == TextAlignment.Right;
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x0022C41C File Offset: 0x0022B41C
		private static bool IsValidWrapDirection(object o)
		{
			WrapDirection wrapDirection = (WrapDirection)o;
			return wrapDirection == WrapDirection.None || wrapDirection == WrapDirection.Left || wrapDirection == WrapDirection.Right || wrapDirection == WrapDirection.Both;
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x0022C444 File Offset: 0x0022B444
		internal static bool IsValidThickness(Thickness t, bool allowNaN)
		{
			double num = (double)Math.Min(1000000, 3500000);
			return (allowNaN || (!double.IsNaN(t.Left) && !double.IsNaN(t.Right) && !double.IsNaN(t.Top) && !double.IsNaN(t.Bottom))) && (double.IsNaN(t.Left) || (t.Left >= 0.0 && t.Left <= num)) && (double.IsNaN(t.Right) || (t.Right >= 0.0 && t.Right <= num)) && (double.IsNaN(t.Top) || (t.Top >= 0.0 && t.Top <= num)) && (double.IsNaN(t.Bottom) || (t.Bottom >= 0.0 && t.Bottom <= num));
		}

		// Token: 0x04002603 RID: 9731
		public static readonly DependencyProperty IsHyphenationEnabledProperty = DependencyProperty.RegisterAttached("IsHyphenationEnabled", typeof(bool), typeof(Block), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04002604 RID: 9732
		public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(Block), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Block.IsValidMargin));

		// Token: 0x04002605 RID: 9733
		public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Block), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Block.IsValidPadding));

		// Token: 0x04002606 RID: 9734
		public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Block), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Block.IsValidBorderThickness));

		// Token: 0x04002607 RID: 9735
		public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Block), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002608 RID: 9736
		public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.RegisterAttached("TextAlignment", typeof(TextAlignment), typeof(Block), new FrameworkPropertyMetadata(TextAlignment.Left, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(Block.IsValidTextAlignment));

		// Token: 0x04002609 RID: 9737
		public static readonly DependencyProperty FlowDirectionProperty = FrameworkElement.FlowDirectionProperty.AddOwner(typeof(Block));

		// Token: 0x0400260A RID: 9738
		public static readonly DependencyProperty LineHeightProperty = DependencyProperty.RegisterAttached("LineHeight", typeof(double), typeof(Block), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(Block.IsValidLineHeight));

		// Token: 0x0400260B RID: 9739
		public static readonly DependencyProperty LineStackingStrategyProperty = DependencyProperty.RegisterAttached("LineStackingStrategy", typeof(LineStackingStrategy), typeof(Block), new FrameworkPropertyMetadata(LineStackingStrategy.MaxHeight, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(Block.IsValidLineStackingStrategy));

		// Token: 0x0400260C RID: 9740
		public static readonly DependencyProperty BreakPageBeforeProperty = DependencyProperty.Register("BreakPageBefore", typeof(bool), typeof(Block), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		// Token: 0x0400260D RID: 9741
		public static readonly DependencyProperty BreakColumnBeforeProperty = DependencyProperty.Register("BreakColumnBefore", typeof(bool), typeof(Block), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		// Token: 0x0400260E RID: 9742
		public static readonly DependencyProperty ClearFloatersProperty = DependencyProperty.Register("ClearFloaters", typeof(WrapDirection), typeof(Block), new FrameworkPropertyMetadata(WrapDirection.None, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Block.IsValidWrapDirection));
	}
}
