using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000321 RID: 801
	internal abstract class Line : TextSource, IDisposable
	{
		// Token: 0x06001DBB RID: 7611 RVA: 0x0016E0D2 File Offset: 0x0016D0D2
		public void Dispose()
		{
			if (this._line != null)
			{
				this._line.Dispose();
				this._line = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0016E0F4 File Offset: 0x0016D0F4
		internal Line(TextBlock owner)
		{
			this._owner = owner;
			this._textAlignment = owner.TextAlignment;
			this._showParagraphEllipsis = false;
			this._wrappingWidth = this._owner.RenderSize.Width;
			base.PixelsPerDip = this._owner.GetDpi().PixelsPerDip;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0016E154 File Offset: 0x0016D154
		internal void Format(int dcp, double width, TextParagraphProperties lineProperties, TextLineBreak textLineBreak, TextRunCache textRunCache, bool showParagraphEllipsis)
		{
			this._mirror = (lineProperties.FlowDirection == FlowDirection.RightToLeft);
			this._dcp = dcp;
			this._showParagraphEllipsis = showParagraphEllipsis;
			this._wrappingWidth = width;
			this._line = this._owner.TextFormatter.FormatLine(this, dcp, width, lineProperties, textLineBreak, textRunCache);
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void Arrange(VisualCollection vc, Vector lineOffset)
		{
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0016E1A4 File Offset: 0x0016D1A4
		internal void Render(DrawingContext ctx, Point lineOffset)
		{
			TextLine textLine = this._line;
			if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
			{
				textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
				});
			}
			double num = this.CalculateXOffsetShift();
			textLine.Draw(ctx, new Point(lineOffset.X + num, lineOffset.Y), this._mirror ? InvertAxes.Horizontal : InvertAxes.None);
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x0016E232 File Offset: 0x0016D232
		internal Rect GetBoundsFromTextPosition(int characterIndex, out FlowDirection flowDirection)
		{
			return this.GetBoundsFromPosition(characterIndex, 1, out flowDirection);
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0016E240 File Offset: 0x0016D240
		internal List<Rect> GetRangeBounds(int cp, int cch, double xOffset, double yOffset)
		{
			List<Rect> list = new List<Rect>();
			double num = this.CalculateXOffsetShift();
			double num2 = xOffset + num;
			IList<TextBounds> textBounds;
			if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
				});
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				textBounds = textLine.GetTextBounds(cp, cch);
			}
			else
			{
				textBounds = this._line.GetTextBounds(cp, cch);
			}
			Invariant.Assert(textBounds.Count > 0);
			for (int i = 0; i < textBounds.Count; i++)
			{
				Rect rectangle = textBounds[i].Rectangle;
				rectangle.X += num2;
				rectangle.Y += yOffset;
				list.Add(rectangle);
			}
			return list;
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0016E340 File Offset: 0x0016D340
		internal CharacterHit GetTextPositionFromDistance(double distance)
		{
			double num = this.CalculateXOffsetShift();
			if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
			{
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
				});
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				return textLine.GetCharacterHitFromDistance(distance);
			}
			return this._line.GetCharacterHitFromDistance(distance - num);
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x0016E3D7 File Offset: 0x0016D3D7
		internal CharacterHit GetNextCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetNextCaretCharacterHit(index);
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0016E3E5 File Offset: 0x0016D3E5
		internal CharacterHit GetPreviousCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetPreviousCaretCharacterHit(index);
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0016E3F3 File Offset: 0x0016D3F3
		internal CharacterHit GetBackspaceCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetBackspaceCaretCharacterHit(index);
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0016E401 File Offset: 0x0016D401
		internal bool IsAtCaretCharacterHit(CharacterHit charHit)
		{
			return this._line.IsAtCaretCharacterHit(charHit, this._dcp);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool HasInlineObjects()
		{
			return false;
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual IInputElement InputHitTest(double offset)
		{
			return null;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x0016E415 File Offset: 0x0016D415
		internal TextLineBreak GetTextLineBreak()
		{
			if (this._line == null)
			{
				return null;
			}
			return this._line.GetTextLineBreak();
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x0016E42C File Offset: 0x0016D42C
		internal int GetEllipsesLength()
		{
			if (!this._line.HasOverflowed)
			{
				return 0;
			}
			if (this._owner.ParagraphProperties.TextTrimming == TextTrimming.None)
			{
				return 0;
			}
			IList<TextCollapsedRange> textCollapsedRanges = this._line.Collapse(new TextCollapsingProperties[]
			{
				this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
			}).GetTextCollapsedRanges();
			if (textCollapsedRanges != null)
			{
				return textCollapsedRanges[0].Length;
			}
			return 0;
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0016E4A0 File Offset: 0x0016D4A0
		internal double GetCollapsedWidth()
		{
			if (!this._line.HasOverflowed)
			{
				return this.Width;
			}
			if (this._owner.ParagraphProperties.TextTrimming == TextTrimming.None)
			{
				return this.Width;
			}
			return this._line.Collapse(new TextCollapsingProperties[]
			{
				this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
			}).Width;
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001DCC RID: 7628 RVA: 0x0016E50A File Offset: 0x0016D50A
		internal double Width
		{
			get
			{
				if (this.IsWidthAdjusted)
				{
					return this._line.WidthIncludingTrailingWhitespace;
				}
				return this._line.Width;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001DCD RID: 7629 RVA: 0x0016E52B File Offset: 0x0016D52B
		internal double Start
		{
			get
			{
				if (this.IsXOffsetAdjusted)
				{
					return this._line.Start + this.CalculateXOffsetShift();
				}
				return this._line.Start;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001DCE RID: 7630 RVA: 0x0016E553 File Offset: 0x0016D553
		internal double Height
		{
			get
			{
				return this._line.Height;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001DCF RID: 7631 RVA: 0x0016E560 File Offset: 0x0016D560
		internal double BaselineOffset
		{
			get
			{
				return this._line.Baseline;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001DD0 RID: 7632 RVA: 0x0016E56D File Offset: 0x0016D56D
		internal bool EndOfParagraph
		{
			get
			{
				if (this._line.NewlineLength == 0)
				{
					return false;
				}
				IList<TextSpan<TextRun>> textRunSpans = this._line.GetTextRunSpans();
				return textRunSpans[textRunSpans.Count - 1].Value is TextEndOfParagraph;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x0016E5A3 File Offset: 0x0016D5A3
		internal int Length
		{
			get
			{
				return this._line.Length - (this.EndOfParagraph ? Line._syntheticCharacterLength : 0);
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x0016E5C1 File Offset: 0x0016D5C1
		internal int ContentLength
		{
			get
			{
				return this._line.Length - this._line.NewlineLength;
			}
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x0016E5DC File Offset: 0x0016D5DC
		protected Rect GetBoundsFromPosition(int cp, int cch, out FlowDirection flowDirection)
		{
			double num = this.CalculateXOffsetShift();
			IList<TextBounds> textBounds;
			if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
				});
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				textBounds = textLine.GetTextBounds(cp, cch);
			}
			else
			{
				textBounds = this._line.GetTextBounds(cp, cch);
			}
			Invariant.Assert(textBounds != null && textBounds.Count == 1, "Expecting exactly one TextBounds for a single text position.");
			IList<TextRunBounds> textRunBounds = textBounds[0].TextRunBounds;
			Rect rectangle;
			if (textRunBounds != null)
			{
				rectangle = textRunBounds[0].Rectangle;
			}
			else
			{
				rectangle = textBounds[0].Rectangle;
			}
			rectangle.X += num;
			flowDirection = textBounds[0].FlowDirection;
			return rectangle;
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x0016E6DC File Offset: 0x0016D6DC
		protected TextCollapsingProperties GetCollapsingProps(double wrappingWidth, LineProperties paraProperties)
		{
			TextCollapsingProperties result;
			if (paraProperties.TextTrimming == TextTrimming.CharacterEllipsis)
			{
				result = new TextTrailingCharacterEllipsis(wrappingWidth, paraProperties.DefaultTextRunProperties);
			}
			else
			{
				result = new TextTrailingWordEllipsis(wrappingWidth, paraProperties.DefaultTextRunProperties);
			}
			return result;
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x0016E710 File Offset: 0x0016D710
		protected double CalculateXOffsetShift()
		{
			if (!this.IsXOffsetAdjusted)
			{
				return 0.0;
			}
			if (this._textAlignment == TextAlignment.Center)
			{
				return (this._line.Width - this._line.WidthIncludingTrailingWhitespace) / 2.0;
			}
			return this._line.Width - this._line.WidthIncludingTrailingWhitespace;
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x0016E771 File Offset: 0x0016D771
		protected bool ShowEllipsis
		{
			get
			{
				return this._owner.ParagraphProperties.TextTrimming != TextTrimming.None && (this._line.HasOverflowed || this._showParagraphEllipsis);
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x0016E79F File Offset: 0x0016D79F
		protected bool HasLineBreak
		{
			get
			{
				return this._line.NewlineLength > 0;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x0016E7AF File Offset: 0x0016D7AF
		protected bool IsXOffsetAdjusted
		{
			get
			{
				return (this._textAlignment == TextAlignment.Right || this._textAlignment == TextAlignment.Center) && this.IsWidthAdjusted;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x0016E7CC File Offset: 0x0016D7CC
		protected bool IsWidthAdjusted
		{
			get
			{
				bool result = false;
				if ((this.HasLineBreak || this.EndOfParagraph) && !this.ShowEllipsis)
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x04000EC3 RID: 3779
		protected TextBlock _owner;

		// Token: 0x04000EC4 RID: 3780
		protected TextLine _line;

		// Token: 0x04000EC5 RID: 3781
		protected int _dcp;

		// Token: 0x04000EC6 RID: 3782
		protected static int _syntheticCharacterLength = 1;

		// Token: 0x04000EC7 RID: 3783
		protected bool _mirror;

		// Token: 0x04000EC8 RID: 3784
		protected TextAlignment _textAlignment;

		// Token: 0x04000EC9 RID: 3785
		protected bool _showParagraphEllipsis;

		// Token: 0x04000ECA RID: 3786
		protected double _wrappingWidth;
	}
}
