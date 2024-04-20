using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000628 RID: 1576
	[TextElementEditingBehavior(IsMergeable = true, IsTypographicOnly = true)]
	public abstract class Inline : TextElement
	{
		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x06004DFA RID: 19962 RVA: 0x00242C7C File Offset: 0x00241C7C
		public InlineCollection SiblingInlines
		{
			get
			{
				if (base.Parent == null)
				{
					return null;
				}
				return new InlineCollection(this, false);
			}
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06004DFB RID: 19963 RVA: 0x00242C8F File Offset: 0x00241C8F
		public Inline NextInline
		{
			get
			{
				return base.NextElement as Inline;
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06004DFC RID: 19964 RVA: 0x00242C9C File Offset: 0x00241C9C
		public Inline PreviousInline
		{
			get
			{
				return base.PreviousElement as Inline;
			}
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06004DFD RID: 19965 RVA: 0x00242CA9 File Offset: 0x00241CA9
		// (set) Token: 0x06004DFE RID: 19966 RVA: 0x00242CBB File Offset: 0x00241CBB
		public BaselineAlignment BaselineAlignment
		{
			get
			{
				return (BaselineAlignment)base.GetValue(Inline.BaselineAlignmentProperty);
			}
			set
			{
				base.SetValue(Inline.BaselineAlignmentProperty, value);
			}
		}

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06004DFF RID: 19967 RVA: 0x00242CCE File Offset: 0x00241CCE
		// (set) Token: 0x06004E00 RID: 19968 RVA: 0x00242CE0 File Offset: 0x00241CE0
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(Inline.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(Inline.TextDecorationsProperty, value);
			}
		}

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06004E01 RID: 19969 RVA: 0x00242CEE File Offset: 0x00241CEE
		// (set) Token: 0x06004E02 RID: 19970 RVA: 0x00242D00 File Offset: 0x00241D00
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(Inline.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(Inline.FlowDirectionProperty, value);
			}
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x00242D13 File Offset: 0x00241D13
		internal static Run CreateImplicitRun(DependencyObject parent)
		{
			return new Run();
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x00242D1A File Offset: 0x00241D1A
		internal static InlineUIContainer CreateImplicitInlineUIContainer(DependencyObject parent)
		{
			return new InlineUIContainer();
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x00242D24 File Offset: 0x00241D24
		private static bool IsValidBaselineAlignment(object o)
		{
			BaselineAlignment baselineAlignment = (BaselineAlignment)o;
			return baselineAlignment == BaselineAlignment.Baseline || baselineAlignment == BaselineAlignment.Bottom || baselineAlignment == BaselineAlignment.Center || baselineAlignment == BaselineAlignment.Subscript || baselineAlignment == BaselineAlignment.Superscript || baselineAlignment == BaselineAlignment.TextBottom || baselineAlignment == BaselineAlignment.TextTop || baselineAlignment == BaselineAlignment.Top;
		}

		// Token: 0x04002832 RID: 10290
		public static readonly DependencyProperty BaselineAlignmentProperty = DependencyProperty.Register("BaselineAlignment", typeof(BaselineAlignment), typeof(Inline), new FrameworkPropertyMetadata(BaselineAlignment.Baseline, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Inline.IsValidBaselineAlignment));

		// Token: 0x04002833 RID: 10291
		public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(Inline), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002834 RID: 10292
		public static readonly DependencyProperty FlowDirectionProperty = FrameworkElement.FlowDirectionProperty.AddOwner(typeof(Inline));
	}
}
