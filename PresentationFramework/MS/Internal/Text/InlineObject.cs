using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000320 RID: 800
	internal sealed class InlineObject : TextEmbeddedObject
	{
		// Token: 0x06001DAF RID: 7599 RVA: 0x0016DF89 File Offset: 0x0016CF89
		internal InlineObject(int dcp, int cch, UIElement element, TextRunProperties textProps, TextBlock host)
		{
			this._dcp = dcp;
			this._cch = cch;
			this._element = element;
			this._textProps = textProps;
			this._host = host;
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x0016DFB8 File Offset: 0x0016CFB8
		public override TextEmbeddedObjectMetrics Format(double remainingParagraphWidth)
		{
			Size size = this._host.MeasureChild(this);
			TextDpi.EnsureValidObjSize(ref size);
			double baseline = size.Height;
			double num = (double)this.Element.GetValue(TextBlock.BaselineOffsetProperty);
			if (!DoubleUtil.IsNaN(num))
			{
				baseline = num;
			}
			return new TextEmbeddedObjectMetrics(size.Width, size.Height, baseline);
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x0016E018 File Offset: 0x0016D018
		public override Rect ComputeBoundingBox(bool rightToLeft, bool sideways)
		{
			if (this._element.IsArrangeValid)
			{
				Size desiredSize = this._element.DesiredSize;
				double num = (!sideways) ? desiredSize.Height : desiredSize.Width;
				double num2 = (double)this.Element.GetValue(TextBlock.BaselineOffsetProperty);
				if (!sideways && !DoubleUtil.IsNaN(num2))
				{
					num = num2;
				}
				return new Rect(0.0, -num, sideways ? desiredSize.Height : desiredSize.Width, sideways ? desiredSize.Width : desiredSize.Height);
			}
			return Rect.Empty;
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public override void Draw(DrawingContext drawingContext, Point origin, bool rightToLeft, bool sideways)
		{
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x0011EF3D File Offset: 0x0011DF3D
		public override CharacterBufferReference CharacterBufferReference
		{
			get
			{
				return new CharacterBufferReference(string.Empty, 0);
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001DB4 RID: 7604 RVA: 0x0016E0B2 File Offset: 0x0016D0B2
		public override int Length
		{
			get
			{
				return this._cch;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001DB5 RID: 7605 RVA: 0x0016E0BA File Offset: 0x0016D0BA
		public override TextRunProperties Properties
		{
			get
			{
				return this._textProps;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x00105F35 File Offset: 0x00104F35
		public override LineBreakCondition BreakBefore
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x00105F35 File Offset: 0x00104F35
		public override LineBreakCondition BreakAfter
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001DB8 RID: 7608 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool HasFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001DB9 RID: 7609 RVA: 0x0016E0C2 File Offset: 0x0016D0C2
		internal int Dcp
		{
			get
			{
				return this._dcp;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001DBA RID: 7610 RVA: 0x0016E0CA File Offset: 0x0016D0CA
		internal UIElement Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x04000EBE RID: 3774
		private readonly int _dcp;

		// Token: 0x04000EBF RID: 3775
		private readonly int _cch;

		// Token: 0x04000EC0 RID: 3776
		private readonly UIElement _element;

		// Token: 0x04000EC1 RID: 3777
		private readonly TextRunProperties _textProps;

		// Token: 0x04000EC2 RID: 3778
		private readonly TextBlock _host;
	}
}
