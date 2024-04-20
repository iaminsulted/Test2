using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200064E RID: 1614
	[ContentProperty("Inlines")]
	public class Paragraph : Block
	{
		// Token: 0x06004FF9 RID: 20473 RVA: 0x0024502C File Offset: 0x0024402C
		static Paragraph()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Paragraph), new FrameworkPropertyMetadata(typeof(Paragraph)));
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x0022C811 File Offset: 0x0022B811
		public Paragraph()
		{
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x0024519E File Offset: 0x0024419E
		public Paragraph(Inline inline)
		{
			if (inline == null)
			{
				throw new ArgumentNullException("inline");
			}
			this.Inlines.Add(inline);
		}

		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06004FFC RID: 20476 RVA: 0x002451C0 File Offset: 0x002441C0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public InlineCollection Inlines
		{
			get
			{
				return new InlineCollection(this, true);
			}
		}

		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06004FFD RID: 20477 RVA: 0x002451C9 File Offset: 0x002441C9
		// (set) Token: 0x06004FFE RID: 20478 RVA: 0x002451DB File Offset: 0x002441DB
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(Paragraph.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(Paragraph.TextDecorationsProperty, value);
			}
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06004FFF RID: 20479 RVA: 0x002451E9 File Offset: 0x002441E9
		// (set) Token: 0x06005000 RID: 20480 RVA: 0x002451FB File Offset: 0x002441FB
		[TypeConverter(typeof(LengthConverter))]
		public double TextIndent
		{
			get
			{
				return (double)base.GetValue(Paragraph.TextIndentProperty);
			}
			set
			{
				base.SetValue(Paragraph.TextIndentProperty, value);
			}
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06005001 RID: 20481 RVA: 0x0024520E File Offset: 0x0024420E
		// (set) Token: 0x06005002 RID: 20482 RVA: 0x00245220 File Offset: 0x00244220
		public int MinOrphanLines
		{
			get
			{
				return (int)base.GetValue(Paragraph.MinOrphanLinesProperty);
			}
			set
			{
				base.SetValue(Paragraph.MinOrphanLinesProperty, value);
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06005003 RID: 20483 RVA: 0x00245233 File Offset: 0x00244233
		// (set) Token: 0x06005004 RID: 20484 RVA: 0x00245245 File Offset: 0x00244245
		public int MinWidowLines
		{
			get
			{
				return (int)base.GetValue(Paragraph.MinWidowLinesProperty);
			}
			set
			{
				base.SetValue(Paragraph.MinWidowLinesProperty, value);
			}
		}

		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06005005 RID: 20485 RVA: 0x00245258 File Offset: 0x00244258
		// (set) Token: 0x06005006 RID: 20486 RVA: 0x0024526A File Offset: 0x0024426A
		public bool KeepWithNext
		{
			get
			{
				return (bool)base.GetValue(Paragraph.KeepWithNextProperty);
			}
			set
			{
				base.SetValue(Paragraph.KeepWithNextProperty, value);
			}
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06005007 RID: 20487 RVA: 0x00245278 File Offset: 0x00244278
		// (set) Token: 0x06005008 RID: 20488 RVA: 0x0024528A File Offset: 0x0024428A
		public bool KeepTogether
		{
			get
			{
				return (bool)base.GetValue(Paragraph.KeepTogetherProperty);
			}
			set
			{
				base.SetValue(Paragraph.KeepTogetherProperty, value);
			}
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x00245298 File Offset: 0x00244298
		internal void GetDefaultMarginValue(ref Thickness margin)
		{
			double num = base.LineHeight;
			if (Paragraph.IsLineHeightAuto(num))
			{
				num = base.FontFamily.LineSpacing * base.FontSize;
			}
			margin = new Thickness(0.0, num, 0.0, num);
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x002452E6 File Offset: 0x002442E6
		internal static bool IsMarginAuto(Thickness margin)
		{
			return double.IsNaN(margin.Left) && double.IsNaN(margin.Right) && double.IsNaN(margin.Top) && double.IsNaN(margin.Bottom);
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x00245320 File Offset: 0x00244320
		internal static bool IsLineHeightAuto(double lineHeight)
		{
			return double.IsNaN(lineHeight);
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x00245328 File Offset: 0x00244328
		internal static bool HasNoTextContent(Paragraph paragraph)
		{
			ITextPointer textPointer = paragraph.ContentStart.CreatePointer();
			ITextPointer contentEnd = paragraph.ContentEnd;
			while (textPointer.CompareTo(contentEnd) < 0)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.EmbeddedElement || typeof(LineBreak).IsAssignableFrom(textPointer.ParentType) || typeof(AnchoredBlock).IsAssignableFrom(textPointer.ParentType))
				{
					return false;
				}
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			return true;
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x0022BF83 File Offset: 0x0022AF83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeInlines(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x002453A0 File Offset: 0x002443A0
		private static bool IsValidMinOrphanLines(object o)
		{
			int num = (int)o;
			return num >= 0 && num <= 1000000;
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x002453A0 File Offset: 0x002443A0
		private static bool IsValidMinWidowLines(object o)
		{
			int num = (int)o;
			return num >= 0 && num <= 1000000;
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x00231978 File Offset: 0x00230978
		private static bool IsValidTextIndent(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			double num3 = -num2;
			return !double.IsNaN(num) && num >= num3 && num <= num2;
		}

		// Token: 0x04002879 RID: 10361
		public static readonly DependencyProperty TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(Paragraph), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x0400287A RID: 10362
		public static readonly DependencyProperty TextIndentProperty = DependencyProperty.Register("TextIndent", typeof(double), typeof(Paragraph), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Paragraph.IsValidTextIndent));

		// Token: 0x0400287B RID: 10363
		public static readonly DependencyProperty MinOrphanLinesProperty = DependencyProperty.Register("MinOrphanLines", typeof(int), typeof(Paragraph), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Paragraph.IsValidMinOrphanLines));

		// Token: 0x0400287C RID: 10364
		public static readonly DependencyProperty MinWidowLinesProperty = DependencyProperty.Register("MinWidowLines", typeof(int), typeof(Paragraph), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Paragraph.IsValidMinWidowLines));

		// Token: 0x0400287D RID: 10365
		public static readonly DependencyProperty KeepWithNextProperty = DependencyProperty.Register("KeepWithNext", typeof(bool), typeof(Paragraph), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		// Token: 0x0400287E RID: 10366
		public static readonly DependencyProperty KeepTogetherProperty = DependencyProperty.Register("KeepTogether", typeof(bool), typeof(Paragraph), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
	}
}
