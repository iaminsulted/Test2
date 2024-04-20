using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200013E RID: 318
	internal sealed class InlineObjectRun : TextEmbeddedObject
	{
		// Token: 0x060009A2 RID: 2466 RVA: 0x0011EE17 File Offset: 0x0011DE17
		internal InlineObjectRun(int cch, UIElement element, TextRunProperties textProps, TextParagraph host)
		{
			this._cch = cch;
			this._textProps = textProps;
			this._host = host;
			this._inlineUIContainer = (InlineUIContainer)LogicalTreeHelper.GetParent(element);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0011EE48 File Offset: 0x0011DE48
		public override TextEmbeddedObjectMetrics Format(double remainingParagraphWidth)
		{
			Size size = this._host.MeasureChild(this);
			TextDpi.EnsureValidObjSize(ref size);
			double baseline = size.Height;
			double num = (double)this.UIElementIsland.Root.GetValue(TextBlock.BaselineOffsetProperty);
			if (!DoubleUtil.IsNaN(num))
			{
				baseline = num;
			}
			return new TextEmbeddedObjectMetrics(size.Width, size.Height, baseline);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0011EEAC File Offset: 0x0011DEAC
		public override Rect ComputeBoundingBox(bool rightToLeft, bool sideways)
		{
			Size desiredSize = this.UIElementIsland.Root.DesiredSize;
			double num = (!sideways) ? desiredSize.Height : desiredSize.Width;
			double num2 = (double)this.UIElementIsland.Root.GetValue(TextBlock.BaselineOffsetProperty);
			if (!sideways && !DoubleUtil.IsNaN(num2))
			{
				num = num2;
			}
			return new Rect(0.0, -num, sideways ? desiredSize.Height : desiredSize.Width, sideways ? desiredSize.Width : desiredSize.Height);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public override void Draw(DrawingContext drawingContext, Point origin, bool rightToLeft, bool sideways)
		{
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0011EF3D File Offset: 0x0011DF3D
		public override CharacterBufferReference CharacterBufferReference
		{
			get
			{
				return new CharacterBufferReference(string.Empty, 0);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0011EF4A File Offset: 0x0011DF4A
		public override int Length
		{
			get
			{
				return this._cch;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0011EF52 File Offset: 0x0011DF52
		public override TextRunProperties Properties
		{
			get
			{
				return this._textProps;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x00105F35 File Offset: 0x00104F35
		public override LineBreakCondition BreakBefore
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x00105F35 File Offset: 0x00104F35
		public override LineBreakCondition BreakAfter
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool HasFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0011EF5A File Offset: 0x0011DF5A
		internal UIElementIsland UIElementIsland
		{
			get
			{
				return this._inlineUIContainer.UIElementIsland;
			}
		}

		// Token: 0x040007EC RID: 2028
		private readonly int _cch;

		// Token: 0x040007ED RID: 2029
		private readonly TextRunProperties _textProps;

		// Token: 0x040007EE RID: 2030
		private readonly TextParagraph _host;

		// Token: 0x040007EF RID: 2031
		private InlineUIContainer _inlineUIContainer;
	}
}
