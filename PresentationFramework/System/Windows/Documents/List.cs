using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000640 RID: 1600
	[ContentProperty("ListItems")]
	public class List : Block
	{
		// Token: 0x06004F1E RID: 20254 RVA: 0x00243578 File Offset: 0x00242578
		static List()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(List), new FrameworkPropertyMetadata(typeof(List)));
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x0022C811 File Offset: 0x0022B811
		public List()
		{
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x00243664 File Offset: 0x00242664
		public List(ListItem listItem)
		{
			if (listItem == null)
			{
				throw new ArgumentNullException("listItem");
			}
			this.ListItems.Add(listItem);
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06004F21 RID: 20257 RVA: 0x00243686 File Offset: 0x00242686
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ListItemCollection ListItems
		{
			get
			{
				return new ListItemCollection(this, true);
			}
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06004F22 RID: 20258 RVA: 0x0024368F File Offset: 0x0024268F
		// (set) Token: 0x06004F23 RID: 20259 RVA: 0x002436A1 File Offset: 0x002426A1
		public TextMarkerStyle MarkerStyle
		{
			get
			{
				return (TextMarkerStyle)base.GetValue(List.MarkerStyleProperty);
			}
			set
			{
				base.SetValue(List.MarkerStyleProperty, value);
			}
		}

		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06004F24 RID: 20260 RVA: 0x002436B4 File Offset: 0x002426B4
		// (set) Token: 0x06004F25 RID: 20261 RVA: 0x002436C6 File Offset: 0x002426C6
		[TypeConverter(typeof(LengthConverter))]
		public double MarkerOffset
		{
			get
			{
				return (double)base.GetValue(List.MarkerOffsetProperty);
			}
			set
			{
				base.SetValue(List.MarkerOffsetProperty, value);
			}
		}

		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06004F26 RID: 20262 RVA: 0x002436D9 File Offset: 0x002426D9
		// (set) Token: 0x06004F27 RID: 20263 RVA: 0x002436EB File Offset: 0x002426EB
		public int StartIndex
		{
			get
			{
				return (int)base.GetValue(List.StartIndexProperty);
			}
			set
			{
				base.SetValue(List.StartIndexProperty, value);
			}
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x00243700 File Offset: 0x00242700
		internal int GetListItemIndex(ListItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.Parent != this)
			{
				throw new InvalidOperationException(SR.Get("ListElementItemNotAChildOfList"));
			}
			int num = this.StartIndex;
			TextPointer textPointer = new TextPointer(base.ContentStart);
			while (textPointer.CompareTo(base.ContentEnd) != 0)
			{
				if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					DependencyObject adjacentElementFromOuterPosition = textPointer.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					if (adjacentElementFromOuterPosition is ListItem)
					{
						if (adjacentElementFromOuterPosition == item)
						{
							break;
						}
						if (num < 2147483647)
						{
							num++;
						}
					}
					textPointer.MoveToPosition(((TextElement)adjacentElementFromOuterPosition).ElementEnd);
				}
				else
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
			return num;
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x002437A0 File Offset: 0x002427A0
		internal void Apply(Block firstBlock, Block lastBlock)
		{
			Invariant.Assert(base.Parent == null, "Cannot Apply List Because It Is Inserted In The Tree Already.");
			Invariant.Assert(base.IsEmpty, "Cannot Apply List Because It Is Not Empty.");
			Invariant.Assert(firstBlock.Parent == lastBlock.Parent, "Cannot Apply List Because Block Are Not Siblings.");
			TextContainer textContainer = base.TextContainer;
			textContainer.BeginChange();
			try
			{
				base.Reposition(firstBlock.ElementStart, lastBlock.ElementEnd);
				ListItem listItem;
				for (Block block = firstBlock; block != null; block = ((block == lastBlock) ? null : ((Block)listItem.ElementEnd.GetAdjacentElement(LogicalDirection.Forward))))
				{
					if (block is List)
					{
						listItem = (block.ElementStart.GetAdjacentElement(LogicalDirection.Backward) as ListItem);
						if (listItem != null)
						{
							listItem.Reposition(listItem.ContentStart, block.ElementEnd);
						}
						else
						{
							listItem = new ListItem();
							listItem.Reposition(block.ElementStart, block.ElementEnd);
						}
					}
					else
					{
						listItem = new ListItem();
						listItem.Reposition(block.ElementStart, block.ElementEnd);
						block.ClearValue(Block.MarginProperty);
						block.ClearValue(Block.PaddingProperty);
						block.ClearValue(Paragraph.TextIndentProperty);
					}
				}
				TextRangeEdit.SetParagraphProperty(base.ElementStart, base.ElementEnd, Block.FlowDirectionProperty, firstBlock.GetValue(Block.FlowDirectionProperty));
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x002438EC File Offset: 0x002428EC
		private static bool IsValidMarkerStyle(object o)
		{
			TextMarkerStyle textMarkerStyle = (TextMarkerStyle)o;
			return textMarkerStyle == TextMarkerStyle.None || textMarkerStyle == TextMarkerStyle.Disc || textMarkerStyle == TextMarkerStyle.Circle || textMarkerStyle == TextMarkerStyle.Square || textMarkerStyle == TextMarkerStyle.Box || textMarkerStyle == TextMarkerStyle.LowerRoman || textMarkerStyle == TextMarkerStyle.UpperRoman || textMarkerStyle == TextMarkerStyle.LowerLatin || textMarkerStyle == TextMarkerStyle.UpperLatin || textMarkerStyle == TextMarkerStyle.Decimal;
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x0024392A File Offset: 0x0024292A
		private static bool IsValidStartIndex(object o)
		{
			return (int)o > 0;
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x00243938 File Offset: 0x00242938
		private static bool IsValidMarkerOffset(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			double num3 = -num2;
			return double.IsNaN(num) || (num >= num3 && num <= num2);
		}

		// Token: 0x04002842 RID: 10306
		public static readonly DependencyProperty MarkerStyleProperty = DependencyProperty.Register("MarkerStyle", typeof(TextMarkerStyle), typeof(List), new FrameworkPropertyMetadata(TextMarkerStyle.Disc, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(List.IsValidMarkerStyle));

		// Token: 0x04002843 RID: 10307
		public static readonly DependencyProperty MarkerOffsetProperty = DependencyProperty.Register("MarkerOffset", typeof(double), typeof(List), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(List.IsValidMarkerOffset));

		// Token: 0x04002844 RID: 10308
		public static readonly DependencyProperty StartIndexProperty = DependencyProperty.Register("StartIndex", typeof(int), typeof(List), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(List.IsValidStartIndex));
	}
}
