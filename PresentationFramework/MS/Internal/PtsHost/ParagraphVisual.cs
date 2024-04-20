using System;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000135 RID: 309
	internal class ParagraphVisual : DrawingVisual
	{
		// Token: 0x06000871 RID: 2161 RVA: 0x001150D8 File Offset: 0x001140D8
		internal ParagraphVisual()
		{
			this._renderBounds = Rect.Empty;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x001150EC File Offset: 0x001140EC
		internal void DrawBackgroundAndBorder(Brush backgroundBrush, Brush borderBrush, Thickness borderThickness, Rect renderBounds, bool isFirstChunk, bool isLastChunk)
		{
			if (this._backgroundBrush != backgroundBrush || this._renderBounds != renderBounds || this._borderBrush != borderBrush || !Thickness.AreClose(this._borderThickness, borderThickness))
			{
				using (DrawingContext drawingContext = base.RenderOpen())
				{
					this.DrawBackgroundAndBorderIntoContext(drawingContext, backgroundBrush, borderBrush, borderThickness, renderBounds, isFirstChunk, isLastChunk);
				}
			}
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0011515C File Offset: 0x0011415C
		internal void DrawBackgroundAndBorderIntoContext(DrawingContext dc, Brush backgroundBrush, Brush borderBrush, Thickness borderThickness, Rect renderBounds, bool isFirstChunk, bool isLastChunk)
		{
			this._backgroundBrush = (Brush)FreezableOperations.GetAsFrozenIfPossible(backgroundBrush);
			this._renderBounds = renderBounds;
			this._borderBrush = (Brush)FreezableOperations.GetAsFrozenIfPossible(borderBrush);
			this._borderThickness = borderThickness;
			if (!isFirstChunk)
			{
				this._borderThickness.Top = 0.0;
			}
			if (!isLastChunk)
			{
				this._borderThickness.Bottom = 0.0;
			}
			if (this._borderBrush != null)
			{
				Pen pen = new Pen();
				pen.Brush = this._borderBrush;
				pen.Thickness = this._borderThickness.Left;
				if (pen.CanFreeze)
				{
					pen.Freeze();
				}
				if (this._borderThickness.IsUniform)
				{
					dc.DrawRectangle(null, pen, new Rect(new Point(this._renderBounds.Left + pen.Thickness * 0.5, this._renderBounds.Bottom - pen.Thickness * 0.5), new Point(this._renderBounds.Right - pen.Thickness * 0.5, this._renderBounds.Top + pen.Thickness * 0.5)));
				}
				else
				{
					if (DoubleUtil.GreaterThan(this._borderThickness.Left, 0.0))
					{
						dc.DrawLine(pen, new Point(this._renderBounds.Left + pen.Thickness / 2.0, this._renderBounds.Top), new Point(this._renderBounds.Left + pen.Thickness / 2.0, this._renderBounds.Bottom));
					}
					if (DoubleUtil.GreaterThan(this._borderThickness.Right, 0.0))
					{
						pen = new Pen();
						pen.Brush = this._borderBrush;
						pen.Thickness = this._borderThickness.Right;
						if (pen.CanFreeze)
						{
							pen.Freeze();
						}
						dc.DrawLine(pen, new Point(this._renderBounds.Right - pen.Thickness / 2.0, this._renderBounds.Top), new Point(this._renderBounds.Right - pen.Thickness / 2.0, this._renderBounds.Bottom));
					}
					if (DoubleUtil.GreaterThan(this._borderThickness.Top, 0.0))
					{
						pen = new Pen();
						pen.Brush = this._borderBrush;
						pen.Thickness = this._borderThickness.Top;
						if (pen.CanFreeze)
						{
							pen.Freeze();
						}
						dc.DrawLine(pen, new Point(this._renderBounds.Left, this._renderBounds.Top + pen.Thickness / 2.0), new Point(this._renderBounds.Right, this._renderBounds.Top + pen.Thickness / 2.0));
					}
					if (DoubleUtil.GreaterThan(this._borderThickness.Bottom, 0.0))
					{
						pen = new Pen();
						pen.Brush = this._borderBrush;
						pen.Thickness = this._borderThickness.Bottom;
						if (pen.CanFreeze)
						{
							pen.Freeze();
						}
						dc.DrawLine(pen, new Point(this._renderBounds.Left, this._renderBounds.Bottom - pen.Thickness / 2.0), new Point(this._renderBounds.Right, this._renderBounds.Bottom - pen.Thickness / 2.0));
					}
				}
			}
			if (this._backgroundBrush != null)
			{
				dc.DrawRectangle(this._backgroundBrush, null, new Rect(new Point(this._renderBounds.Left + this._borderThickness.Left, this._renderBounds.Top + this._borderThickness.Top), new Point(this._renderBounds.Right - this._borderThickness.Right, this._renderBounds.Bottom - this._borderThickness.Bottom)));
			}
		}

		// Token: 0x040007C4 RID: 1988
		private Brush _backgroundBrush;

		// Token: 0x040007C5 RID: 1989
		private Brush _borderBrush;

		// Token: 0x040007C6 RID: 1990
		private Thickness _borderThickness;

		// Token: 0x040007C7 RID: 1991
		private Rect _renderBounds;
	}
}
