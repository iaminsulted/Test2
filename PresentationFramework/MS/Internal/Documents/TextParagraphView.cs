using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020001F4 RID: 500
	internal class TextParagraphView : TextViewBase
	{
		// Token: 0x06001219 RID: 4633 RVA: 0x00149983 File Offset: 0x00148983
		internal TextParagraphView(TextBlock owner, ITextContainer textContainer)
		{
			this._owner = owner;
			this._textContainer = textContainer;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0014999C File Offset: 0x0014899C
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ITextPointer textPositionFromPoint = TextParagraphView.GetTextPositionFromPoint(this.Lines, point, snapToText);
			Invariant.Assert(textPositionFromPoint == null || textPositionFromPoint.HasValidLayout);
			return textPositionFromPoint;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x001499E4 File Offset: 0x001489E4
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			transform = Transform.Identity;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			return this._owner.GetRectangleFromTextPosition(position);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00149A3C File Offset: 0x00148A3C
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (startPosition == null)
			{
				throw new ArgumentNullException("startPosition");
			}
			if (endPosition == null)
			{
				throw new ArgumentNullException("endPosition");
			}
			ValidationHelper.VerifyPosition(this._textContainer, startPosition, "startPosition");
			ValidationHelper.VerifyDirection(startPosition.LogicalDirection, "startPosition.LogicalDirection");
			ValidationHelper.VerifyPosition(this._textContainer, endPosition, "endPosition");
			return this._owner.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00149ABC File Offset: 0x00148ABC
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			newSuggestedX = suggestedX;
			linesMoved = 0;
			if (count == 0)
			{
				return position;
			}
			ReadOnlyCollection<LineResult> lines = this.Lines;
			int num = TextParagraphView.GetLineFromPosition(lines, position);
			if (num < 0 || num >= lines.Count)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			int num2 = num;
			num = Math.Max(0, num + count);
			num = Math.Min(lines.Count - 1, num);
			linesMoved = num - num2;
			ITextPointer textPointer;
			if (linesMoved == 0)
			{
				textPointer = position;
			}
			else if (!DoubleUtil.IsNaN(suggestedX))
			{
				textPointer = lines[num].GetTextPositionFromDistance(suggestedX);
			}
			else
			{
				textPointer = lines[num].StartPosition.CreatePointer(LogicalDirection.Forward);
			}
			Invariant.Assert(textPointer == null || textPointer.HasValidLayout);
			return textPointer;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00149B9C File Offset: 0x00148B9C
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			int lineFromPosition = TextParagraphView.GetLineFromPosition(this.Lines, position);
			int startPositionCP = this.Lines[lineFromPosition].StartPositionCP;
			return this._owner.IsAtCaretUnitBoundary(position, startPositionCP, lineFromPosition);
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00149C00 File Offset: 0x00148C00
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			int lineFromPosition = TextParagraphView.GetLineFromPosition(this.Lines, position);
			int startPositionCP = this.Lines[lineFromPosition].StartPositionCP;
			ITextPointer nextCaretUnitPosition = this._owner.GetNextCaretUnitPosition(position, direction, startPositionCP, lineFromPosition);
			Invariant.Assert(nextCaretUnitPosition == null || nextCaretUnitPosition.HasValidLayout);
			return nextCaretUnitPosition;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00149C78 File Offset: 0x00148C78
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			int lineFromPosition = TextParagraphView.GetLineFromPosition(this.Lines, position);
			int startPositionCP = this.Lines[lineFromPosition].StartPositionCP;
			ITextPointer backspaceCaretUnitPosition = this._owner.GetBackspaceCaretUnitPosition(position, startPositionCP, lineFromPosition);
			Invariant.Assert(backspaceCaretUnitPosition == null || backspaceCaretUnitPosition.HasValidLayout);
			return backspaceCaretUnitPosition;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00149CF0 File Offset: 0x00148CF0
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			ReadOnlyCollection<LineResult> lines = this.Lines;
			int lineFromPosition = TextParagraphView.GetLineFromPosition(lines, position);
			return new TextSegment(lines[lineFromPosition].StartPosition, lines[lineFromPosition].GetContentEndPosition(), true);
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00149D61 File Offset: 0x00148D61
		internal override bool Contains(ITextPointer position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return true;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00149D9B File Offset: 0x00148D9B
		internal override bool Validate()
		{
			this._owner.UpdateLayout();
			return this.IsValid;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00149DB0 File Offset: 0x00148DB0
		internal static ITextPointer GetTextPositionFromPoint(ReadOnlyCollection<LineResult> lines, Point point, bool snapToText)
		{
			int lineFromPoint = TextParagraphView.GetLineFromPoint(lines, point, snapToText);
			ITextPointer result;
			if (lineFromPoint < 0)
			{
				result = null;
			}
			else
			{
				result = lines[lineFromPoint].GetTextPositionFromDistance(point.X);
			}
			return result;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00149DE4 File Offset: 0x00148DE4
		internal static int GetLineFromPosition(ReadOnlyCollection<LineResult> lines, ITextPointer position)
		{
			int i = 0;
			int num = lines.Count - 1;
			int num2 = lines[0].StartPosition.GetOffsetToPosition(position) + lines[0].StartPositionCP;
			if (num2 >= lines[0].StartPositionCP && num2 <= lines[lines.Count - 1].EndPositionCP)
			{
				int num3 = 0;
				while (i < num)
				{
					if (num - i < 2)
					{
						num3 = ((num3 == i) ? num : i);
					}
					else
					{
						num3 = i + (num - i) / 2;
					}
					if (num2 < lines[num3].StartPositionCP)
					{
						num = num3;
					}
					else if (num2 > lines[num3].EndPositionCP)
					{
						i = num3;
					}
					else if (num2 == lines[num3].EndPositionCP)
					{
						if (position.LogicalDirection == LogicalDirection.Forward && num3 != lines.Count - 1)
						{
							num3++;
							break;
						}
						break;
					}
					else
					{
						if (num2 == lines[num3].StartPositionCP && position.LogicalDirection == LogicalDirection.Backward && num3 != 0)
						{
							num3--;
							break;
						}
						break;
					}
				}
				return num3;
			}
			if (num2 >= lines[0].StartPositionCP)
			{
				return lines.Count - 1;
			}
			return 0;
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0014045C File Offset: 0x0013F45C
		internal void OnUpdated()
		{
			this.OnUpdated(EventArgs.Empty);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00149EF4 File Offset: 0x00148EF4
		internal void Invalidate()
		{
			this._lines = null;
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x00149EFD File Offset: 0x00148EFD
		internal override UIElement RenderScope
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x00149F05 File Offset: 0x00148F05
		internal override ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x00149F0D File Offset: 0x00148F0D
		internal override bool IsValid
		{
			get
			{
				return this._owner.IsLayoutDataValid;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x00149F1A File Offset: 0x00148F1A
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				return new ReadOnlyCollection<TextSegment>(new List<TextSegment>(1)
				{
					new TextSegment(this._textContainer.Start, this._textContainer.End, true)
				});
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x00149F49 File Offset: 0x00148F49
		internal ReadOnlyCollection<LineResult> Lines
		{
			get
			{
				if (this._lines == null)
				{
					this._lines = this._owner.GetLineResults();
				}
				return this._lines;
			}
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00149F6C File Offset: 0x00148F6C
		internal static int GetLineFromPoint(ReadOnlyCollection<LineResult> lines, Point point, bool snapToText)
		{
			int result;
			bool flag = TextParagraphView.GetVerticalLineFromPoint(lines, point, snapToText, out result);
			if (flag)
			{
				flag = TextParagraphView.GetHorizontalLineFromPoint(lines, point, snapToText, ref result);
			}
			if (!flag)
			{
				return -1;
			}
			return result;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00149F98 File Offset: 0x00148F98
		private static bool GetVerticalLineFromPoint(ReadOnlyCollection<LineResult> lines, Point point, bool snapToText, out int lineIndex)
		{
			bool flag = false;
			double height = lines[0].LayoutBox.Height;
			lineIndex = Math.Max(Math.Min((int)(point.Y / height), lines.Count - 1), 0);
			while (!flag)
			{
				Rect layoutBox = lines[lineIndex].LayoutBox;
				if (point.Y < layoutBox.Y)
				{
					if (lineIndex <= 0)
					{
						flag = snapToText;
						break;
					}
					lineIndex--;
				}
				else
				{
					if (point.Y <= layoutBox.Y + layoutBox.Height)
					{
						double num = 0.0;
						if (lineIndex > 0)
						{
							Rect layoutBox2 = lines[lineIndex - 1].LayoutBox;
							num = layoutBox.Y - (layoutBox2.Y + layoutBox2.Height);
						}
						if (num < 0.0)
						{
							if (point.Y < layoutBox.Y - num / 2.0)
							{
								lineIndex--;
							}
						}
						else
						{
							num = 0.0;
							if (lineIndex < lines.Count - 1)
							{
								num = lines[lineIndex + 1].LayoutBox.Y - (layoutBox.Y + layoutBox.Height);
							}
							if (num < 0.0 && point.Y > layoutBox.Y + layoutBox.Height + num / 2.0)
							{
								lineIndex++;
							}
						}
						flag = true;
						break;
					}
					if (lineIndex >= lines.Count - 1)
					{
						flag = snapToText;
						break;
					}
					Rect layoutBox3 = lines[lineIndex + 1].LayoutBox;
					if (point.Y < layoutBox3.Y)
					{
						double num2 = layoutBox3.Y - (layoutBox.Y + layoutBox.Height);
						if (point.Y > layoutBox.Y + layoutBox.Height + num2 / 2.0)
						{
							lineIndex++;
						}
						flag = snapToText;
						break;
					}
					lineIndex++;
				}
			}
			return flag;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0014A1B0 File Offset: 0x001491B0
		private static bool GetHorizontalLineFromPoint(ReadOnlyCollection<LineResult> lines, Point point, bool snapToText, ref int lineIndex)
		{
			bool result = false;
			bool flag = true;
			while (flag)
			{
				Rect layoutBox = lines[lineIndex].LayoutBox;
				if (point.X < layoutBox.X && lineIndex > 0)
				{
					Rect layoutBox2 = lines[lineIndex - 1].LayoutBox;
					if (!DoubleUtil.AreClose(layoutBox2.Y, layoutBox.Y))
					{
						result = snapToText;
						break;
					}
					if (point.X > layoutBox2.X + layoutBox2.Width)
					{
						double num = Math.Max(layoutBox.X - (layoutBox2.X + layoutBox2.Width), 0.0);
						if (point.X < layoutBox.X - num / 2.0)
						{
							lineIndex--;
						}
						result = snapToText;
						break;
					}
					lineIndex--;
				}
				else
				{
					if (point.X <= layoutBox.X + layoutBox.Width || lineIndex >= lines.Count - 1)
					{
						result = (snapToText || (point.X >= layoutBox.X && point.X <= layoutBox.X + layoutBox.Width));
						break;
					}
					Rect layoutBox2 = lines[lineIndex + 1].LayoutBox;
					if (!DoubleUtil.AreClose(layoutBox2.Y, layoutBox.Y))
					{
						result = snapToText;
						break;
					}
					if (point.X < layoutBox2.X)
					{
						double num = Math.Max(layoutBox2.X - (layoutBox.X + layoutBox.Width), 0.0);
						if (point.X > layoutBox2.X - num / 2.0)
						{
							lineIndex++;
						}
						result = snapToText;
						break;
					}
					lineIndex++;
				}
			}
			return result;
		}

		// Token: 0x04000B25 RID: 2853
		private readonly TextBlock _owner;

		// Token: 0x04000B26 RID: 2854
		private readonly ITextContainer _textContainer;

		// Token: 0x04000B27 RID: 2855
		private ReadOnlyCollection<LineResult> _lines;
	}
}
