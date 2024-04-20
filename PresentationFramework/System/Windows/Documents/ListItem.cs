using System;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000641 RID: 1601
	[ContentProperty("Blocks")]
	public class ListItem : TextElement
	{
		// Token: 0x06004F2D RID: 20269 RVA: 0x0022C550 File Offset: 0x0022B550
		public ListItem()
		{
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x00243975 File Offset: 0x00242975
		public ListItem(Paragraph paragraph)
		{
			if (paragraph == null)
			{
				throw new ArgumentNullException("paragraph");
			}
			this.Blocks.Add(paragraph);
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06004F2F RID: 20271 RVA: 0x00243997 File Offset: 0x00242997
		public List List
		{
			get
			{
				return base.Parent as List;
			}
		}

		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06004F30 RID: 20272 RVA: 0x0022BE7C File Offset: 0x0022AE7C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06004F31 RID: 20273 RVA: 0x002439A4 File Offset: 0x002429A4
		public ListItemCollection SiblingListItems
		{
			get
			{
				if (base.Parent == null)
				{
					return null;
				}
				return new ListItemCollection(this, false);
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06004F32 RID: 20274 RVA: 0x002439B7 File Offset: 0x002429B7
		public ListItem NextListItem
		{
			get
			{
				return base.NextElement as ListItem;
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06004F33 RID: 20275 RVA: 0x002439C4 File Offset: 0x002429C4
		public ListItem PreviousListItem
		{
			get
			{
				return base.PreviousElement as ListItem;
			}
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06004F34 RID: 20276 RVA: 0x002439D1 File Offset: 0x002429D1
		// (set) Token: 0x06004F35 RID: 20277 RVA: 0x002439E3 File Offset: 0x002429E3
		public Thickness Margin
		{
			get
			{
				return (Thickness)base.GetValue(ListItem.MarginProperty);
			}
			set
			{
				base.SetValue(ListItem.MarginProperty, value);
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06004F36 RID: 20278 RVA: 0x002439F6 File Offset: 0x002429F6
		// (set) Token: 0x06004F37 RID: 20279 RVA: 0x00243A08 File Offset: 0x00242A08
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(ListItem.PaddingProperty);
			}
			set
			{
				base.SetValue(ListItem.PaddingProperty, value);
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06004F38 RID: 20280 RVA: 0x00243A1B File Offset: 0x00242A1B
		// (set) Token: 0x06004F39 RID: 20281 RVA: 0x00243A2D File Offset: 0x00242A2D
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(ListItem.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(ListItem.BorderThicknessProperty, value);
			}
		}

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06004F3A RID: 20282 RVA: 0x00243A40 File Offset: 0x00242A40
		// (set) Token: 0x06004F3B RID: 20283 RVA: 0x00243A52 File Offset: 0x00242A52
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(ListItem.BorderBrushProperty);
			}
			set
			{
				base.SetValue(ListItem.BorderBrushProperty, value);
			}
		}

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06004F3C RID: 20284 RVA: 0x00243A60 File Offset: 0x00242A60
		// (set) Token: 0x06004F3D RID: 20285 RVA: 0x00243A72 File Offset: 0x00242A72
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(ListItem.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(ListItem.TextAlignmentProperty, value);
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06004F3E RID: 20286 RVA: 0x00243A85 File Offset: 0x00242A85
		// (set) Token: 0x06004F3F RID: 20287 RVA: 0x00243A97 File Offset: 0x00242A97
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(ListItem.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(ListItem.FlowDirectionProperty, value);
			}
		}

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06004F40 RID: 20288 RVA: 0x00243AAA File Offset: 0x00242AAA
		// (set) Token: 0x06004F41 RID: 20289 RVA: 0x00243ABC File Offset: 0x00242ABC
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(ListItem.LineHeightProperty);
			}
			set
			{
				base.SetValue(ListItem.LineHeightProperty, value);
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06004F42 RID: 20290 RVA: 0x00243ACF File Offset: 0x00242ACF
		// (set) Token: 0x06004F43 RID: 20291 RVA: 0x00243AE1 File Offset: 0x00242AE1
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(ListItem.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(ListItem.LineStackingStrategyProperty, value);
			}
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x0022BF83 File Offset: 0x0022AF83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBlocks(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06004F45 RID: 20293 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04002845 RID: 10309
		public static readonly DependencyProperty MarginProperty = Block.MarginProperty.AddOwner(typeof(ListItem), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x04002846 RID: 10310
		public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(ListItem), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x04002847 RID: 10311
		public static readonly DependencyProperty BorderThicknessProperty = Block.BorderThicknessProperty.AddOwner(typeof(ListItem), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002848 RID: 10312
		public static readonly DependencyProperty BorderBrushProperty = Block.BorderBrushProperty.AddOwner(typeof(ListItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002849 RID: 10313
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(ListItem));

		// Token: 0x0400284A RID: 10314
		public static readonly DependencyProperty FlowDirectionProperty = Block.FlowDirectionProperty.AddOwner(typeof(ListItem));

		// Token: 0x0400284B RID: 10315
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(ListItem));

		// Token: 0x0400284C RID: 10316
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(ListItem));
	}
}
