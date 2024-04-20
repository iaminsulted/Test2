using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost;

namespace MS.Internal.Text
{
	// Token: 0x0200031E RID: 798
	internal sealed class ComplexLine : Line
	{
		// Token: 0x06001D8F RID: 7567 RVA: 0x0016D3A0 File Offset: 0x0016C3A0
		public override TextRun GetTextRun(int dcp)
		{
			TextRun textRun = null;
			StaticTextPointer position = this._owner.TextContainer.CreateStaticPointerAtOffset(dcp);
			switch (position.GetPointerContext(LogicalDirection.Forward))
			{
			case TextPointerContext.None:
				textRun = new TextEndOfParagraph(Line._syntheticCharacterLength);
				break;
			case TextPointerContext.Text:
				textRun = this.HandleText(position);
				break;
			case TextPointerContext.EmbeddedElement:
				textRun = this.HandleInlineObject(position, dcp);
				break;
			case TextPointerContext.ElementStart:
				textRun = this.HandleElementStartEdge(position);
				break;
			case TextPointerContext.ElementEnd:
				textRun = this.HandleElementEndEdge(position);
				break;
			}
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x0016D434 File Offset: 0x0016C434
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			int num = 0;
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				ITextPointer textPointer = this._owner.TextContainer.CreatePointerAtOffset(dcp, LogicalDirection.Backward);
				while (textPointer.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text && textPointer.CompareTo(this._owner.TextContainer.Start) != 0)
				{
					textPointer.MoveByOffset(-1);
					num++;
				}
				string textInRun = textPointer.GetTextInRun(LogicalDirection.Backward);
				empty = new CharacterBufferRange(textInRun, 0, textInRun.Length);
				StaticTextPointer staticTextPointer = textPointer.CreateStaticPointer();
				culture = DynamicPropertyReader.GetCultureInfo((staticTextPointer.Parent != null) ? staticTextPointer.Parent : this._owner);
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(num + empty.Length, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x001136C4 File Offset: 0x001126C4
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return textSourceCharacterIndex;
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0016D4EA File Offset: 0x0016C4EA
		internal ComplexLine(TextBlock owner) : base(owner)
		{
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x0016D4F4 File Offset: 0x0016C4F4
		internal override void Arrange(VisualCollection vc, Vector lineOffset)
		{
			int num = this._dcp;
			IEnumerable<TextSpan<TextRun>> textRunSpans = this._line.GetTextRunSpans();
			double x = lineOffset.X;
			base.CalculateXOffsetShift();
			foreach (TextSpan<TextRun> textSpan in textRunSpans)
			{
				TextRun value = textSpan.Value;
				if (value is InlineObject)
				{
					InlineObject inlineObject = value as InlineObject;
					Visual visual = VisualTreeHelper.GetParent(inlineObject.Element) as Visual;
					if (visual != null)
					{
						ContainerVisual containerVisual = visual as ContainerVisual;
						Invariant.Assert(containerVisual != null, "parent should always derives from ContainerVisual");
						containerVisual.Children.Remove(inlineObject.Element);
					}
					FlowDirection flowDirection;
					Rect boundsFromPosition = base.GetBoundsFromPosition(num, inlineObject.Length, out flowDirection);
					ContainerVisual containerVisual2 = new ContainerVisual();
					if (inlineObject.Element is FrameworkElement)
					{
						FlowDirection childFD = this._owner.FlowDirection;
						DependencyObject parent = ((FrameworkElement)inlineObject.Element).Parent;
						if (parent != null)
						{
							childFD = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
						}
						PtsHelper.UpdateMirroringTransform(this._owner.FlowDirection, childFD, containerVisual2, boundsFromPosition.Width);
					}
					vc.Add(containerVisual2);
					if (this._owner.UseLayoutRounding)
					{
						DpiScale dpi = this._owner.GetDpi();
						containerVisual2.Offset = new Vector(UIElement.RoundLayoutValue(lineOffset.X + boundsFromPosition.Left, dpi.DpiScaleX), UIElement.RoundLayoutValue(lineOffset.Y + boundsFromPosition.Top, dpi.DpiScaleY));
					}
					else
					{
						containerVisual2.Offset = new Vector(lineOffset.X + boundsFromPosition.Left, lineOffset.Y + boundsFromPosition.Top);
					}
					containerVisual2.Children.Add(inlineObject.Element);
					inlineObject.Element.Arrange(new Rect(inlineObject.Element.DesiredSize));
				}
				num += textSpan.Length;
			}
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x0016D704 File Offset: 0x0016C704
		internal override bool HasInlineObjects()
		{
			bool result = false;
			using (IEnumerator<TextSpan<TextRun>> enumerator = this._line.GetTextRunSpans().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Value is InlineObject)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x0016D764 File Offset: 0x0016C764
		internal override IInputElement InputHitTest(double offset)
		{
			DependencyObject dependencyObject = null;
			bool flag = this._owner.TextContainer is TextContainer;
			double num = base.CalculateXOffsetShift();
			if (flag)
			{
				CharacterHit characterHitFromDistance;
				if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
				{
					Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
					TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
					{
						base.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
					});
					Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
					characterHitFromDistance = textLine.GetCharacterHitFromDistance(offset);
				}
				else
				{
					characterHitFromDistance = this._line.GetCharacterHitFromDistance(offset - num);
				}
				TextPointer textPointer = new TextPointer(this._owner.ContentStart, this.CalcPositionOffset(characterHitFromDistance), LogicalDirection.Forward);
				if (textPointer != null)
				{
					TextPointerContext pointerContext;
					if (characterHitFromDistance.TrailingLength == 0)
					{
						pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
					}
					else
					{
						pointerContext = textPointer.GetPointerContext(LogicalDirection.Backward);
					}
					if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.ElementEnd)
					{
						dependencyObject = (textPointer.Parent as TextElement);
					}
					else if (pointerContext == TextPointerContext.ElementStart)
					{
						dependencyObject = textPointer.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					}
				}
			}
			return dependencyObject as IInputElement;
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x0016D87C File Offset: 0x0016C87C
		private TextRun HandleText(StaticTextPointer position)
		{
			DependencyObject target;
			if (position.Parent != null)
			{
				target = position.Parent;
			}
			else
			{
				target = this._owner;
			}
			TextRunProperties textRunProperties = new TextProperties(target, position, false, true, base.PixelsPerDip);
			StaticTextPointer position2 = this._owner.Highlights.GetNextPropertyChangePosition(position, LogicalDirection.Forward);
			if (position.GetOffsetToPosition(position2) > 4096)
			{
				position2 = position.CreatePointer(4096);
			}
			char[] array = new char[position.GetOffsetToPosition(position2)];
			int textInRun = position.GetTextInRun(LogicalDirection.Forward, array, 0, array.Length);
			return new TextCharacters(array, 0, textInRun, textRunProperties);
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x0016D90C File Offset: 0x0016C90C
		private TextRun HandleElementStartEdge(StaticTextPointer position)
		{
			TextElement textElement = (TextElement)position.GetAdjacentElement(LogicalDirection.Forward);
			TextRun result;
			if (textElement is LineBreak)
			{
				result = new TextEndOfLine(ComplexLine._elementEdgeCharacterLength * 2);
			}
			else if (textElement.IsEmpty)
			{
				TextRunProperties textRunProperties = new TextProperties(textElement, position, false, true, base.PixelsPerDip);
				char[] array = new char[ComplexLine._elementEdgeCharacterLength * 2];
				array[0] = '​';
				array[1] = '​';
				result = new TextCharacters(array, 0, array.Length, textRunProperties);
			}
			else
			{
				Inline inline = textElement as Inline;
				if (inline == null)
				{
					result = new TextHidden(ComplexLine._elementEdgeCharacterLength);
				}
				else
				{
					DependencyObject parent = inline.Parent;
					FlowDirection flowDirection = inline.FlowDirection;
					FlowDirection flowDirection2 = flowDirection;
					if (parent != null)
					{
						flowDirection2 = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
					}
					TextDecorationCollection textDecorations = DynamicPropertyReader.GetTextDecorations(inline);
					if (flowDirection != flowDirection2)
					{
						if (textDecorations == null || textDecorations.Count == 0)
						{
							result = new TextSpanModifier(ComplexLine._elementEdgeCharacterLength, null, null, flowDirection);
						}
						else
						{
							result = new TextSpanModifier(ComplexLine._elementEdgeCharacterLength, textDecorations, inline.Foreground, flowDirection);
						}
					}
					else if (textDecorations == null || textDecorations.Count == 0)
					{
						result = new TextHidden(ComplexLine._elementEdgeCharacterLength);
					}
					else
					{
						result = new TextSpanModifier(ComplexLine._elementEdgeCharacterLength, textDecorations, inline.Foreground);
					}
				}
			}
			return result;
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x0016DA48 File Offset: 0x0016CA48
		private TextRun HandleElementEndEdge(StaticTextPointer position)
		{
			Inline inline = ((TextElement)position.GetAdjacentElement(LogicalDirection.Forward)) as Inline;
			TextRun result;
			if (inline == null)
			{
				result = new TextHidden(ComplexLine._elementEdgeCharacterLength);
			}
			else
			{
				DependencyObject parent = inline.Parent;
				FlowDirection flowDirection = inline.FlowDirection;
				if (parent != null)
				{
					flowDirection = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
				}
				if (inline.FlowDirection != flowDirection)
				{
					result = new TextEndOfSegment(ComplexLine._elementEdgeCharacterLength);
				}
				else
				{
					TextDecorationCollection textDecorations = DynamicPropertyReader.GetTextDecorations(inline);
					if (textDecorations == null || textDecorations.Count == 0)
					{
						result = new TextHidden(ComplexLine._elementEdgeCharacterLength);
					}
					else
					{
						result = new TextEndOfSegment(ComplexLine._elementEdgeCharacterLength);
					}
				}
			}
			return result;
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x0016DAE0 File Offset: 0x0016CAE0
		private TextRun HandleInlineObject(StaticTextPointer position, int dcp)
		{
			DependencyObject dependencyObject = position.GetAdjacentElement(LogicalDirection.Forward) as DependencyObject;
			TextRun result;
			if (dependencyObject is UIElement)
			{
				TextRunProperties textProps = new TextProperties(dependencyObject, position, true, true, base.PixelsPerDip);
				result = new InlineObject(dcp, TextContainerHelper.EmbeddedObjectLength, (UIElement)dependencyObject, textProps, this._owner);
			}
			else
			{
				result = this.HandleElementEndEdge(position);
			}
			return result;
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x0016DB3C File Offset: 0x0016CB3C
		private int CalcPositionOffset(CharacterHit charHit)
		{
			int num = charHit.FirstCharacterIndex + charHit.TrailingLength;
			if (base.EndOfParagraph)
			{
				num = Math.Min(this._dcp + base.Length, num);
			}
			return num;
		}

		// Token: 0x04000EBD RID: 3773
		private static int _elementEdgeCharacterLength = 1;
	}
}
