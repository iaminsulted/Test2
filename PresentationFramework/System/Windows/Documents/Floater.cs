using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x0200061A RID: 1562
	public class Floater : AnchoredBlock
	{
		// Token: 0x06004C77 RID: 19575 RVA: 0x0023C7D0 File Offset: 0x0023B7D0
		static Floater()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Floater), new FrameworkPropertyMetadata(typeof(Floater)));
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x0023C868 File Offset: 0x0023B868
		public Floater() : this(null, null)
		{
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x0023C872 File Offset: 0x0023B872
		public Floater(Block childBlock) : this(childBlock, null)
		{
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x002317B3 File Offset: 0x002307B3
		public Floater(Block childBlock, TextPointer insertionPosition) : base(childBlock, insertionPosition)
		{
		}

		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x06004C7B RID: 19579 RVA: 0x0023C87C File Offset: 0x0023B87C
		// (set) Token: 0x06004C7C RID: 19580 RVA: 0x0023C88E File Offset: 0x0023B88E
		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return (HorizontalAlignment)base.GetValue(Floater.HorizontalAlignmentProperty);
			}
			set
			{
				base.SetValue(Floater.HorizontalAlignmentProperty, value);
			}
		}

		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x06004C7D RID: 19581 RVA: 0x0023C8A1 File Offset: 0x0023B8A1
		// (set) Token: 0x06004C7E RID: 19582 RVA: 0x0023C8B3 File Offset: 0x0023B8B3
		[TypeConverter(typeof(LengthConverter))]
		public double Width
		{
			get
			{
				return (double)base.GetValue(Floater.WidthProperty);
			}
			set
			{
				base.SetValue(Floater.WidthProperty, value);
			}
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x0023C8C8 File Offset: 0x0023B8C8
		private static bool IsValidWidth(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return double.IsNaN(num) || (num >= 0.0 && num <= num2);
		}

		// Token: 0x040027C9 RID: 10185
		public static readonly DependencyProperty HorizontalAlignmentProperty = FrameworkElement.HorizontalAlignmentProperty.AddOwner(typeof(Floater), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x040027CA RID: 10186
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(Floater), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Floater.IsValidWidth));
	}
}
