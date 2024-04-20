using System;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020005D9 RID: 1497
	[ContentProperty("Blocks")]
	public abstract class AnchoredBlock : Inline
	{
		// Token: 0x0600483D RID: 18493 RVA: 0x0022BE20 File Offset: 0x0022AE20
		protected AnchoredBlock(Block block, TextPointer insertionPosition)
		{
			if (insertionPosition != null)
			{
				insertionPosition.TextContainer.BeginChange();
			}
			try
			{
				if (insertionPosition != null)
				{
					insertionPosition.InsertInline(this);
				}
				if (block != null)
				{
					this.Blocks.Add(block);
				}
			}
			finally
			{
				if (insertionPosition != null)
				{
					insertionPosition.TextContainer.EndChange();
				}
			}
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x0600483E RID: 18494 RVA: 0x0022BE7C File Offset: 0x0022AE7C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x0600483F RID: 18495 RVA: 0x0022BE85 File Offset: 0x0022AE85
		// (set) Token: 0x06004840 RID: 18496 RVA: 0x0022BE97 File Offset: 0x0022AE97
		public Thickness Margin
		{
			get
			{
				return (Thickness)base.GetValue(AnchoredBlock.MarginProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.MarginProperty, value);
			}
		}

		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x06004841 RID: 18497 RVA: 0x0022BEAA File Offset: 0x0022AEAA
		// (set) Token: 0x06004842 RID: 18498 RVA: 0x0022BEBC File Offset: 0x0022AEBC
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(AnchoredBlock.PaddingProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.PaddingProperty, value);
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x06004843 RID: 18499 RVA: 0x0022BECF File Offset: 0x0022AECF
		// (set) Token: 0x06004844 RID: 18500 RVA: 0x0022BEE1 File Offset: 0x0022AEE1
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(AnchoredBlock.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.BorderThicknessProperty, value);
			}
		}

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06004845 RID: 18501 RVA: 0x0022BEF4 File Offset: 0x0022AEF4
		// (set) Token: 0x06004846 RID: 18502 RVA: 0x0022BF06 File Offset: 0x0022AF06
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(AnchoredBlock.BorderBrushProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.BorderBrushProperty, value);
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06004847 RID: 18503 RVA: 0x0022BF14 File Offset: 0x0022AF14
		// (set) Token: 0x06004848 RID: 18504 RVA: 0x0022BF26 File Offset: 0x0022AF26
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(AnchoredBlock.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.TextAlignmentProperty, value);
			}
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06004849 RID: 18505 RVA: 0x0022BF39 File Offset: 0x0022AF39
		// (set) Token: 0x0600484A RID: 18506 RVA: 0x0022BF4B File Offset: 0x0022AF4B
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(AnchoredBlock.LineHeightProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.LineHeightProperty, value);
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x0600484B RID: 18507 RVA: 0x0022BF5E File Offset: 0x0022AF5E
		// (set) Token: 0x0600484C RID: 18508 RVA: 0x0022BF70 File Offset: 0x0022AF70
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(AnchoredBlock.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(AnchoredBlock.LineStackingStrategyProperty, value);
			}
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x0022BF83 File Offset: 0x0022AF83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBlocks(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x0600484E RID: 18510 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040025FC RID: 9724
		public static readonly DependencyProperty MarginProperty = Block.MarginProperty.AddOwner(typeof(AnchoredBlock), new FrameworkPropertyMetadata(new Thickness(double.NaN), FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x040025FD RID: 9725
		public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(AnchoredBlock), new FrameworkPropertyMetadata(new Thickness(double.NaN), FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x040025FE RID: 9726
		public static readonly DependencyProperty BorderThicknessProperty = Block.BorderThicknessProperty.AddOwner(typeof(AnchoredBlock), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x040025FF RID: 9727
		public static readonly DependencyProperty BorderBrushProperty = Block.BorderBrushProperty.AddOwner(typeof(AnchoredBlock), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002600 RID: 9728
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(AnchoredBlock));

		// Token: 0x04002601 RID: 9729
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(AnchoredBlock));

		// Token: 0x04002602 RID: 9730
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(AnchoredBlock));
	}
}
