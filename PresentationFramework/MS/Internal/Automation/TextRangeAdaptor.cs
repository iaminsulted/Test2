using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Automation.Text;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.Documents;

namespace MS.Internal.Automation
{
	// Token: 0x02000107 RID: 263
	internal class TextRangeAdaptor : ITextRangeProvider
	{
		// Token: 0x0600065A RID: 1626 RVA: 0x00106DC8 File Offset: 0x00105DC8
		static TextRangeAdaptor()
		{
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.AnimationStyleAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				TextEffectCollection textEffectCollection = tp.GetValue(TextElement.TextEffectsProperty) as TextEffectCollection;
				return (textEffectCollection != null && textEffectCollection.Count > 0) ? AnimationStyle.Other : AnimationStyle.None;
			}, (object val1, object val2) => (AnimationStyle)val1 == (AnimationStyle)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.BackgroundColorAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.ColorFromBrush(tp.GetValue(TextElement.BackgroundProperty)), (object val1, object val2) => (int)val1 == (int)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.BulletStyleAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				object obj = tp.GetValue(List.MarkerStyleProperty);
				if (obj is TextMarkerStyle)
				{
					switch ((TextMarkerStyle)obj)
					{
					case TextMarkerStyle.None:
						obj = BulletStyle.None;
						break;
					case TextMarkerStyle.Disc:
						obj = BulletStyle.FilledRoundBullet;
						break;
					case TextMarkerStyle.Circle:
						obj = BulletStyle.HollowRoundBullet;
						break;
					case TextMarkerStyle.Square:
						obj = BulletStyle.HollowSquareBullet;
						break;
					case TextMarkerStyle.Box:
						obj = BulletStyle.FilledSquareBullet;
						break;
					default:
						obj = BulletStyle.Other;
						break;
					}
				}
				else
				{
					obj = BulletStyle.None;
				}
				return obj;
			}, (object val1, object val2) => (BulletStyle)val1 == (BulletStyle)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.CapStyleAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				CapStyle capStyle;
				switch ((FontCapitals)tp.GetValue(Typography.CapitalsProperty))
				{
				case FontCapitals.Normal:
					capStyle = CapStyle.None;
					break;
				case FontCapitals.AllSmallCaps:
					capStyle = CapStyle.AllCap;
					break;
				case FontCapitals.SmallCaps:
					capStyle = CapStyle.SmallCap;
					break;
				case FontCapitals.AllPetiteCaps:
					capStyle = CapStyle.AllPetiteCaps;
					break;
				case FontCapitals.PetiteCaps:
					capStyle = CapStyle.PetiteCaps;
					break;
				case FontCapitals.Unicase:
					capStyle = CapStyle.Unicase;
					break;
				case FontCapitals.Titling:
					capStyle = CapStyle.Titling;
					break;
				default:
					capStyle = CapStyle.Other;
					break;
				}
				return capStyle;
			}, (object val1, object val2) => (CapStyle)val1 == (CapStyle)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.CultureAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				object value = tp.GetValue(FrameworkElement.LanguageProperty);
				return (value is XmlLanguage) ? ((XmlLanguage)value).GetEquivalentCulture().LCID : CultureInfo.InvariantCulture.LCID;
			}, (object val1, object val2) => (int)val1 == (int)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.FontNameAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.GetFontFamilyName((FontFamily)tp.GetValue(TextElement.FontFamilyProperty), tp), (object val1, object val2) => val1 as string == val2 as string));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.FontSizeAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.NativeObjectLengthToPoints((double)tp.GetValue(TextElement.FontSizeProperty)), (object val1, object val2) => (double)val1 == (double)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.FontWeightAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => ((FontWeight)tp.GetValue(TextElement.FontWeightProperty)).ToOpenTypeWeight(), (object val1, object val2) => (int)val1 == (int)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.ForegroundColorAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.ColorFromBrush(tp.GetValue(TextElement.ForegroundProperty)), (object val1, object val2) => (int)val1 == (int)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.HorizontalTextAlignmentAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				HorizontalTextAlignment horizontalTextAlignment;
				switch ((TextAlignment)tp.GetValue(Block.TextAlignmentProperty))
				{
				default:
					horizontalTextAlignment = HorizontalTextAlignment.Left;
					break;
				case TextAlignment.Right:
					horizontalTextAlignment = HorizontalTextAlignment.Right;
					break;
				case TextAlignment.Center:
					horizontalTextAlignment = HorizontalTextAlignment.Centered;
					break;
				case TextAlignment.Justify:
					horizontalTextAlignment = HorizontalTextAlignment.Justified;
					break;
				}
				return horizontalTextAlignment;
			}, (object val1, object val2) => (HorizontalTextAlignment)val1 == (HorizontalTextAlignment)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.IndentationFirstLineAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.NativeObjectLengthToPoints((double)tp.GetValue(Paragraph.TextIndentProperty)), (object val1, object val2) => (double)val1 == (double)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.IndentationLeadingAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				Thickness thickness = (Thickness)tp.GetValue(Block.PaddingProperty);
				return thickness.IsValid(true, false, false, false) ? TextRangeAdaptor.NativeObjectLengthToPoints(thickness.Left) : 0.0;
			}, (object val1, object val2) => (double)val1 == (double)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.IndentationTrailingAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				Thickness thickness = (Thickness)tp.GetValue(Block.PaddingProperty);
				return thickness.IsValid(true, false, false, false) ? TextRangeAdaptor.NativeObjectLengthToPoints(thickness.Right) : 0.0;
			}, (object val1, object val2) => (double)val1 == (double)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.IsHiddenAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => false, (object val1, object val2) => (bool)val1 == (bool)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.IsItalicAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				FontStyle left = (FontStyle)tp.GetValue(TextElement.FontStyleProperty);
				return left == FontStyles.Italic || left == FontStyles.Oblique;
			}, (object val1, object val2) => (bool)val1 == (bool)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.IsReadOnlyAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				bool flag = false;
				if (tp.TextContainer.TextSelection != null)
				{
					flag = tp.TextContainer.TextSelection.TextEditor.IsReadOnly;
				}
				return flag;
			}, (object val1, object val2) => (bool)val1 == (bool)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.IsSubscriptAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => (FontVariants)tp.GetValue(Typography.VariantsProperty) == FontVariants.Subscript, (object val1, object val2) => (bool)val1 == (bool)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.IsSuperscriptAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => (FontVariants)tp.GetValue(Typography.VariantsProperty) == FontVariants.Superscript, (object val1, object val2) => (bool)val1 == (bool)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.MarginBottomAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				Thickness thickness = (Thickness)tp.GetValue(FrameworkElement.MarginProperty);
				return thickness.IsValid(true, false, false, false) ? TextRangeAdaptor.NativeObjectLengthToPoints(thickness.Bottom) : 0.0;
			}, (object val1, object val2) => (double)val1 == (double)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.MarginLeadingAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				Thickness thickness = (Thickness)tp.GetValue(FrameworkElement.MarginProperty);
				return thickness.IsValid(true, false, false, false) ? TextRangeAdaptor.NativeObjectLengthToPoints(thickness.Left) : 0.0;
			}, (object val1, object val2) => (double)val1 == (double)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.MarginTopAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				Thickness thickness = (Thickness)tp.GetValue(FrameworkElement.MarginProperty);
				return thickness.IsValid(true, false, false, false) ? TextRangeAdaptor.NativeObjectLengthToPoints(thickness.Top) : 0.0;
			}, (object val1, object val2) => (double)val1 == (double)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.MarginTrailingAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				Thickness thickness = (Thickness)tp.GetValue(FrameworkElement.MarginProperty);
				return thickness.IsValid(true, false, false, false) ? TextRangeAdaptor.NativeObjectLengthToPoints(thickness.Right) : 0.0;
			}, (object val1, object val2) => (double)val1 == (double)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.OutlineStylesAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => OutlineStyles.None, (object val1, object val2) => (OutlineStyles)val1 == (OutlineStyles)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.OverlineColorAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.GetTextDecorationColor(tp.GetValue(Inline.TextDecorationsProperty) as TextDecorationCollection, TextDecorationLocation.OverLine), (object val1, object val2) => (int)val1 == (int)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.OverlineStyleAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.GetTextDecorationLineStyle(tp.GetValue(Inline.TextDecorationsProperty) as TextDecorationCollection, TextDecorationLocation.OverLine), (object val1, object val2) => (TextDecorationLineStyle)val1 == (TextDecorationLineStyle)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.StrikethroughColorAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.GetTextDecorationColor(tp.GetValue(Inline.TextDecorationsProperty) as TextDecorationCollection, TextDecorationLocation.Strikethrough), (object val1, object val2) => (int)val1 == (int)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.StrikethroughStyleAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.GetTextDecorationLineStyle(tp.GetValue(Inline.TextDecorationsProperty) as TextDecorationCollection, TextDecorationLocation.Strikethrough), (object val1, object val2) => (TextDecorationLineStyle)val1 == (TextDecorationLineStyle)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.TextFlowDirectionsAttribute, new TextRangeAdaptor.TextAttributeHelper(delegate(ITextPointer tp)
			{
				FlowDirection flowDirection = (FlowDirection)tp.GetValue(FrameworkElement.FlowDirectionProperty);
				FlowDirections flowDirections;
				if (flowDirection == FlowDirection.LeftToRight || flowDirection != FlowDirection.RightToLeft)
				{
					flowDirections = FlowDirections.Default;
				}
				else
				{
					flowDirections = FlowDirections.RightToLeft;
				}
				return flowDirections;
			}, (object val1, object val2) => (FlowDirections)val1 == (FlowDirections)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.UnderlineColorAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.GetTextDecorationColor(tp.GetValue(Inline.TextDecorationsProperty) as TextDecorationCollection, TextDecorationLocation.Underline), (object val1, object val2) => (int)val1 == (int)val2));
			TextRangeAdaptor._textPatternAttributes.Add(TextPatternIdentifiers.UnderlineStyleAttribute, new TextRangeAdaptor.TextAttributeHelper((ITextPointer tp) => TextRangeAdaptor.GetTextDecorationLineStyle(tp.GetValue(Inline.TextDecorationsProperty) as TextDecorationCollection, TextDecorationLocation.Underline), (object val1, object val2) => (TextDecorationLineStyle)val1 == (TextDecorationLineStyle)val2));
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x001073F8 File Offset: 0x001063F8
		internal TextRangeAdaptor(TextAdaptor textAdaptor, ITextPointer start, ITextPointer end, AutomationPeer textPeer)
		{
			Invariant.Assert(textAdaptor != null, "Invalid textAdaptor.");
			Invariant.Assert(textPeer != null, "Invalid textPeer.");
			Invariant.Assert(start != null && end != null, "Invalid range.");
			Invariant.Assert(start.CompareTo(end) <= 0, "Invalid range, end < start.");
			this._textAdaptor = textAdaptor;
			this._start = start.CreatePointer();
			this._end = end.CreatePointer();
			this._textPeer = textPeer;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0010747A File Offset: 0x0010647A
		internal static bool MoveToInsertionPosition(ITextPointer position, LogicalDirection direction)
		{
			return (!position.TextContainer.IsReadOnly || (!TextPointerBase.IsAtNonMergeableInlineStart(position) && !TextPointerBase.IsAtNonMergeableInlineEnd(position))) && position.MoveToInsertionPosition(direction);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x001074A4 File Offset: 0x001064A4
		private TextRangeAdaptor ValidateAndThrow(ITextRangeProvider range)
		{
			TextRangeAdaptor textRangeAdaptor = range as TextRangeAdaptor;
			if (textRangeAdaptor == null || textRangeAdaptor._start.TextContainer != this._start.TextContainer)
			{
				throw new ArgumentException(SR.Get("TextRangeProvider_WrongTextRange"));
			}
			return textRangeAdaptor;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x001074E4 File Offset: 0x001064E4
		private void ExpandToEnclosingUnit(TextUnit unit, bool expandStart, bool expandEnd)
		{
			switch (unit)
			{
			case TextUnit.Character:
				if (expandStart && !TextPointerBase.IsAtInsertionPosition(this._start))
				{
					TextPointerBase.MoveToNextInsertionPosition(this._start, LogicalDirection.Backward);
				}
				if (expandEnd && !TextPointerBase.IsAtInsertionPosition(this._end))
				{
					TextPointerBase.MoveToNextInsertionPosition(this._end, LogicalDirection.Forward);
					return;
				}
				break;
			case TextUnit.Format:
				if (expandStart)
				{
					TextPointerContext textPointerContext = this._start.GetPointerContext(LogicalDirection.Forward);
					for (;;)
					{
						TextPointerContext pointerContext = this._start.GetPointerContext(LogicalDirection.Backward);
						if (pointerContext == TextPointerContext.None || (textPointerContext == TextPointerContext.Text && pointerContext != TextPointerContext.Text))
						{
							break;
						}
						textPointerContext = pointerContext;
						this._start.MoveToNextContextPosition(LogicalDirection.Backward);
					}
				}
				if (expandEnd)
				{
					TextPointerContext textPointerContext2 = this._end.GetPointerContext(LogicalDirection.Backward);
					for (;;)
					{
						TextPointerContext pointerContext2 = this._end.GetPointerContext(LogicalDirection.Forward);
						if (pointerContext2 == TextPointerContext.None || (pointerContext2 == TextPointerContext.Text && textPointerContext2 != TextPointerContext.Text))
						{
							break;
						}
						textPointerContext2 = pointerContext2;
						this._end.MoveToNextContextPosition(LogicalDirection.Forward);
					}
				}
				this._start.SetLogicalDirection(LogicalDirection.Forward);
				this._end.SetLogicalDirection(LogicalDirection.Forward);
				return;
			case TextUnit.Word:
				if (expandStart && !TextRangeAdaptor.IsAtWordBoundary(this._start))
				{
					TextRangeAdaptor.MoveToNextWordBoundary(this._start, LogicalDirection.Backward);
				}
				if (expandEnd && !TextRangeAdaptor.IsAtWordBoundary(this._end))
				{
					TextRangeAdaptor.MoveToNextWordBoundary(this._end, LogicalDirection.Forward);
					return;
				}
				break;
			case TextUnit.Line:
			{
				ITextView updatedTextView = this._textAdaptor.GetUpdatedTextView();
				if (updatedTextView != null && updatedTextView.IsValid)
				{
					bool flag = true;
					if (expandStart && updatedTextView.Contains(this._start))
					{
						TextSegment lineRange = updatedTextView.GetLineRange(this._start);
						if (!lineRange.IsNull)
						{
							if (this._start.CompareTo(lineRange.Start) != 0)
							{
								this._start = lineRange.Start.CreatePointer();
							}
							if (lineRange.Contains(this._end))
							{
								flag = false;
								if (this._end.CompareTo(lineRange.End) != 0)
								{
									this._end = lineRange.End.CreatePointer();
								}
							}
						}
					}
					if (expandEnd && flag && updatedTextView.Contains(this._end))
					{
						TextSegment lineRange2 = updatedTextView.GetLineRange(this._end);
						if (!lineRange2.IsNull && this._end.CompareTo(lineRange2.End) != 0)
						{
							this._end = lineRange2.End.CreatePointer();
							return;
						}
					}
				}
				break;
			}
			case TextUnit.Paragraph:
			{
				ITextRange textRange = new TextRange(this._start, this._end);
				TextRangeBase.SelectParagraph(textRange, this._start);
				if (expandStart && this._start.CompareTo(textRange.Start) != 0)
				{
					this._start = textRange.Start.CreatePointer();
				}
				if (expandEnd)
				{
					if (!textRange.Contains(this._end))
					{
						TextRangeBase.SelectParagraph(textRange, this._end);
					}
					if (this._end.CompareTo(textRange.End) != 0)
					{
						this._end = textRange.End.CreatePointer();
						return;
					}
				}
				break;
			}
			case TextUnit.Page:
			{
				ITextView updatedTextView = this._textAdaptor.GetUpdatedTextView();
				if (updatedTextView != null && updatedTextView.IsValid)
				{
					if (expandStart && updatedTextView.Contains(this._start))
					{
						ITextView textView = updatedTextView;
						if (updatedTextView is MultiPageTextView)
						{
							textView = ((MultiPageTextView)updatedTextView).GetPageTextViewFromPosition(this._start);
						}
						ReadOnlyCollection<TextSegment> textSegments = textView.TextSegments;
						if (textSegments != null && textSegments.Count > 0 && this._start.CompareTo(textSegments[0].Start) != 0)
						{
							this._start = textSegments[0].Start.CreatePointer();
						}
					}
					if (expandEnd && updatedTextView.Contains(this._end))
					{
						ITextView textView2 = updatedTextView;
						if (updatedTextView is MultiPageTextView)
						{
							textView2 = ((MultiPageTextView)updatedTextView).GetPageTextViewFromPosition(this._end);
						}
						ReadOnlyCollection<TextSegment> textSegments2 = textView2.TextSegments;
						if (textSegments2 != null && textSegments2.Count > 0 && this._end.CompareTo(textSegments2[textSegments2.Count - 1].End) != 0)
						{
							this._end = textSegments2[textSegments2.Count - 1].End.CreatePointer();
							return;
						}
					}
				}
				break;
			}
			case TextUnit.Document:
				if (expandStart && this._start.CompareTo(this._start.TextContainer.Start) != 0)
				{
					this._start = this._start.TextContainer.Start.CreatePointer();
				}
				if (expandEnd && this._end.CompareTo(this._start.TextContainer.End) != 0)
				{
					this._end = this._start.TextContainer.End.CreatePointer();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00107970 File Offset: 0x00106970
		private bool MoveToUnitBoundary(ITextPointer position, bool isStart, LogicalDirection direction, TextUnit unit)
		{
			bool flag = false;
			switch (unit)
			{
			case TextUnit.Character:
				if (!TextPointerBase.IsAtInsertionPosition(position) && TextPointerBase.MoveToNextInsertionPosition(position, direction))
				{
					flag = true;
				}
				break;
			case TextUnit.Format:
				while (position.GetPointerContext(direction) == TextPointerContext.Text)
				{
					if (position.MoveToNextContextPosition(direction))
					{
						flag = true;
					}
				}
				if (flag && direction == LogicalDirection.Forward)
				{
					for (;;)
					{
						TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Forward);
						if (pointerContext != TextPointerContext.ElementStart && pointerContext != TextPointerContext.ElementEnd)
						{
							break;
						}
						position.MoveToNextContextPosition(LogicalDirection.Forward);
					}
				}
				break;
			case TextUnit.Word:
				if (!TextRangeAdaptor.IsAtWordBoundary(position) && TextRangeAdaptor.MoveToNextWordBoundary(position, direction))
				{
					flag = true;
				}
				break;
			case TextUnit.Line:
			{
				ITextView updatedTextView = this._textAdaptor.GetUpdatedTextView();
				if (updatedTextView != null && updatedTextView.IsValid && updatedTextView.Contains(position))
				{
					TextSegment lineRange = updatedTextView.GetLineRange(position);
					if (!lineRange.IsNull)
					{
						int num = 0;
						if (direction == LogicalDirection.Forward)
						{
							ITextPointer position2 = null;
							if (isStart)
							{
								double num2;
								position2 = updatedTextView.GetPositionAtNextLine(lineRange.End, double.NaN, 1, out num2, out num);
							}
							if (num != 0)
							{
								position2 = updatedTextView.GetLineRange(position2).Start;
							}
							else
							{
								position2 = lineRange.End;
							}
							position2 = this.GetInsertionPosition(position2, LogicalDirection.Forward);
							if (position.CompareTo(position2) != 0)
							{
								position.MoveToPosition(position2);
								position.SetLogicalDirection(isStart ? LogicalDirection.Forward : LogicalDirection.Backward);
								flag = true;
							}
						}
						else
						{
							ITextPointer position3 = null;
							if (!isStart)
							{
								double num2;
								position3 = updatedTextView.GetPositionAtNextLine(lineRange.Start, double.NaN, -1, out num2, out num);
							}
							if (num != 0)
							{
								position3 = updatedTextView.GetLineRange(position3).End;
							}
							else
							{
								position3 = lineRange.Start;
							}
							position3 = this.GetInsertionPosition(position3, LogicalDirection.Backward);
							if (position.CompareTo(position3) != 0)
							{
								position.MoveToPosition(position3);
								position.SetLogicalDirection(isStart ? LogicalDirection.Forward : LogicalDirection.Backward);
								flag = true;
							}
						}
					}
				}
				break;
			}
			case TextUnit.Paragraph:
			{
				ITextRange textRange = new TextRange(position, position);
				TextRangeBase.SelectParagraph(textRange, position);
				if (direction == LogicalDirection.Forward)
				{
					ITextPointer textPointer = textRange.End;
					if (isStart)
					{
						textPointer = textPointer.CreatePointer();
						if (textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward))
						{
							TextRangeBase.SelectParagraph(textRange, textPointer);
							textPointer = textRange.Start;
						}
					}
					if (position.CompareTo(textPointer) != 0)
					{
						position.MoveToPosition(textPointer);
						position.SetLogicalDirection(isStart ? LogicalDirection.Forward : LogicalDirection.Backward);
						flag = true;
					}
				}
				else
				{
					ITextPointer textPointer2 = textRange.Start;
					if (!isStart)
					{
						textPointer2 = textPointer2.CreatePointer();
						if (textPointer2.MoveToNextInsertionPosition(LogicalDirection.Backward))
						{
							TextRangeBase.SelectParagraph(textRange, textPointer2);
							textPointer2 = textRange.End;
						}
					}
					if (position.CompareTo(textPointer2) != 0)
					{
						position.MoveToPosition(textPointer2);
						position.SetLogicalDirection(isStart ? LogicalDirection.Forward : LogicalDirection.Backward);
						flag = true;
					}
				}
				break;
			}
			case TextUnit.Page:
			{
				ITextView updatedTextView = this._textAdaptor.GetUpdatedTextView();
				if (updatedTextView != null && updatedTextView.IsValid && updatedTextView.Contains(position))
				{
					ITextView textView = updatedTextView;
					if (updatedTextView is MultiPageTextView)
					{
						textView = ((MultiPageTextView)updatedTextView).GetPageTextViewFromPosition(position);
					}
					ReadOnlyCollection<TextSegment> textSegments = textView.TextSegments;
					if (textSegments != null && textSegments.Count > 0)
					{
						if (direction == LogicalDirection.Forward)
						{
							while (position.CompareTo(textSegments[textSegments.Count - 1].End) != 0)
							{
								if (position.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.ElementEnd)
								{
									position.MoveToPosition(textSegments[textSegments.Count - 1].End);
									flag = true;
									break;
								}
								Invariant.Assert(position.MoveToNextContextPosition(LogicalDirection.Forward));
							}
							TextRangeAdaptor.MoveToInsertionPosition(position, LogicalDirection.Forward);
						}
						else
						{
							while (position.CompareTo(textSegments[0].Start) != 0)
							{
								if (position.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.ElementStart)
								{
									position.MoveToPosition(textSegments[0].Start);
									flag = true;
									break;
								}
								Invariant.Assert(position.MoveToNextContextPosition(LogicalDirection.Backward));
							}
							TextRangeAdaptor.MoveToInsertionPosition(position, LogicalDirection.Backward);
						}
					}
				}
				break;
			}
			case TextUnit.Document:
				if (direction == LogicalDirection.Forward)
				{
					if (position.CompareTo(this.GetInsertionPosition(position.TextContainer.End, LogicalDirection.Backward)) != 0)
					{
						position.MoveToPosition(position.TextContainer.End);
						flag = true;
					}
				}
				else if (position.CompareTo(this.GetInsertionPosition(position.TextContainer.Start, LogicalDirection.Forward)) != 0)
				{
					position.MoveToPosition(position.TextContainer.Start);
					flag = true;
				}
				break;
			}
			return flag;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00107DA0 File Offset: 0x00106DA0
		private int MovePositionByUnits(ITextPointer position, TextUnit unit, int count)
		{
			int i = 0;
			int num = (count == int.MinValue) ? int.MaxValue : Math.Abs(count);
			LogicalDirection logicalDirection = (count > 0) ? LogicalDirection.Forward : LogicalDirection.Backward;
			switch (unit)
			{
			case TextUnit.Character:
				while (i < num)
				{
					if (!TextPointerBase.MoveToNextInsertionPosition(position, logicalDirection))
					{
						break;
					}
					i++;
				}
				break;
			case TextUnit.Format:
				while (i < num)
				{
					ITextPointer position2 = position.CreatePointer();
					while (position.GetPointerContext(logicalDirection) == TextPointerContext.Text && position.MoveToNextContextPosition(logicalDirection))
					{
					}
					if (!position.MoveToNextContextPosition(logicalDirection))
					{
						break;
					}
					while (position.GetPointerContext(logicalDirection) != TextPointerContext.Text && position.MoveToNextContextPosition(logicalDirection))
					{
					}
					if (logicalDirection == LogicalDirection.Backward)
					{
						while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text && position.MoveToNextContextPosition(LogicalDirection.Backward))
						{
						}
					}
					if (position.GetPointerContext(logicalDirection) == TextPointerContext.None)
					{
						position.MoveToPosition(position2);
						break;
					}
					i++;
				}
				position.SetLogicalDirection(LogicalDirection.Forward);
				break;
			case TextUnit.Word:
				while (i < num)
				{
					if (!TextRangeAdaptor.MoveToNextWordBoundary(position, logicalDirection))
					{
						break;
					}
					i++;
				}
				break;
			case TextUnit.Line:
			{
				ITextView updatedTextView = this._textAdaptor.GetUpdatedTextView();
				if (updatedTextView != null && updatedTextView.IsValid && updatedTextView.Contains(position))
				{
					if (TextPointerBase.IsAtRowEnd(position))
					{
						position.MoveToNextInsertionPosition(LogicalDirection.Backward);
					}
					i = position.MoveToLineBoundary(count);
					TextRangeAdaptor.MoveToInsertionPosition(position, LogicalDirection.Forward);
					if (i < 0)
					{
						i = -i;
					}
				}
				break;
			}
			case TextUnit.Paragraph:
			{
				ITextRange textRange = new TextRange(position, position);
				textRange.SelectParagraph(position);
				while (i < num)
				{
					position.MoveToPosition((logicalDirection == LogicalDirection.Forward) ? textRange.End : textRange.Start);
					if (!position.MoveToNextInsertionPosition(logicalDirection))
					{
						break;
					}
					i++;
					textRange.SelectParagraph(position);
					position.MoveToPosition(textRange.Start);
				}
				break;
			}
			case TextUnit.Page:
			{
				ITextView updatedTextView = this._textAdaptor.GetUpdatedTextView();
				if (updatedTextView != null && updatedTextView.IsValid && updatedTextView.Contains(position) && updatedTextView is MultiPageTextView)
				{
					ReadOnlyCollection<TextSegment> textSegments = ((MultiPageTextView)updatedTextView).GetPageTextViewFromPosition(position).TextSegments;
					while (i < num && textSegments != null && textSegments.Count != 0)
					{
						if (logicalDirection == LogicalDirection.Backward)
						{
							position.MoveToPosition(textSegments[0].Start);
							TextRangeAdaptor.MoveToInsertionPosition(position, LogicalDirection.Backward);
						}
						else
						{
							position.MoveToPosition(textSegments[textSegments.Count - 1].End);
							TextRangeAdaptor.MoveToInsertionPosition(position, LogicalDirection.Forward);
						}
						ITextPointer textPointer = position.CreatePointer();
						if (!textPointer.MoveToNextInsertionPosition(logicalDirection))
						{
							break;
						}
						if (logicalDirection == LogicalDirection.Forward)
						{
							if (textPointer.CompareTo(position) <= 0)
							{
								break;
							}
						}
						else if (textPointer.CompareTo(position) >= 0)
						{
							break;
						}
						if (!updatedTextView.Contains(textPointer))
						{
							break;
						}
						textSegments = ((MultiPageTextView)updatedTextView).GetPageTextViewFromPosition(textPointer).TextSegments;
						i++;
					}
				}
				break;
			}
			}
			if (logicalDirection != LogicalDirection.Forward)
			{
				return -i;
			}
			return i;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00108060 File Offset: 0x00107060
		private object GetAttributeValue(TextRangeAdaptor.TextAttributeHelper attr)
		{
			ITextPointer textPointer = this._start.CreatePointer();
			ITextPointer textPointer2 = this._end.CreatePointer();
			if (textPointer.CompareTo(textPointer2) < 0)
			{
				while (this.IsElementBoundary(textPointer.GetPointerContext(LogicalDirection.Forward)))
				{
					if (!textPointer.MoveToNextContextPosition(LogicalDirection.Forward) || textPointer.CompareTo(textPointer2) >= 0)
					{
						IL_5B:
						while (this.IsElementBoundary(textPointer2.GetPointerContext(LogicalDirection.Backward)) && textPointer2.MoveToNextContextPosition(LogicalDirection.Backward) && textPointer.CompareTo(textPointer2) < 0)
						{
						}
						if (textPointer.CompareTo(textPointer2) > 0)
						{
							return AutomationElementIdentifiers.NotSupported;
						}
						goto IL_7A;
					}
				}
				goto IL_5B;
			}
			IL_7A:
			object obj = attr.GetValueAt(textPointer2);
			while (textPointer.CompareTo(textPointer2) < 0 && attr.AreEqual(obj, attr.GetValueAt(textPointer)) && textPointer.MoveToNextContextPosition(LogicalDirection.Forward) && textPointer.CompareTo(textPointer2) <= 0)
			{
			}
			if (textPointer.CompareTo(textPointer2) < 0)
			{
				return TextPatternIdentifiers.MixedAttributeValue;
			}
			return obj;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0010813E File Offset: 0x0010713E
		private bool IsElementBoundary(TextPointerContext symbolType)
		{
			return symbolType == TextPointerContext.ElementStart || symbolType == TextPointerContext.ElementEnd;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0010814C File Offset: 0x0010714C
		private static int ColorFromBrush(object brush)
		{
			SolidColorBrush solidColorBrush = brush as SolidColorBrush;
			Color color = (solidColorBrush != null) ? solidColorBrush.Color : Colors.Black;
			return ((int)color.R << 16) + ((int)color.G << 8) + (int)color.B;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00108190 File Offset: 0x00107190
		private static string GetFontFamilyName(FontFamily fontFamily, ITextPointer context)
		{
			if (fontFamily != null)
			{
				if (fontFamily.Source != null)
				{
					return fontFamily.Source;
				}
				if (fontFamily.FamilyMaps != null)
				{
					XmlLanguage xmlLanguage = (context != null) ? ((XmlLanguage)context.GetValue(FrameworkElement.LanguageProperty)) : null;
					foreach (FontFamilyMap fontFamilyMap in fontFamily.FamilyMaps)
					{
						if (fontFamilyMap.Language == null)
						{
							return fontFamilyMap.Target;
						}
						if (xmlLanguage != null && fontFamilyMap.Language.RangeIncludes(xmlLanguage))
						{
							return fontFamilyMap.Target;
						}
					}
				}
			}
			return "Global User Interface";
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0010823C File Offset: 0x0010723C
		private static int GetTextDecorationColor(TextDecorationCollection decorations, TextDecorationLocation location)
		{
			if (decorations == null)
			{
				return 0;
			}
			int result = 0;
			foreach (TextDecoration textDecoration in decorations)
			{
				if (textDecoration.Location == location && textDecoration.Pen != null)
				{
					result = TextRangeAdaptor.ColorFromBrush(textDecoration.Pen.Brush);
					break;
				}
			}
			return result;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x001082B0 File Offset: 0x001072B0
		private static TextDecorationLineStyle GetTextDecorationLineStyle(TextDecorationCollection decorations, TextDecorationLocation location)
		{
			if (decorations == null)
			{
				return TextDecorationLineStyle.None;
			}
			TextDecorationLineStyle textDecorationLineStyle = TextDecorationLineStyle.None;
			foreach (TextDecoration textDecoration in decorations)
			{
				if (textDecoration.Location == location)
				{
					if (textDecorationLineStyle != TextDecorationLineStyle.None)
					{
						textDecorationLineStyle = TextDecorationLineStyle.Other;
						break;
					}
					if (textDecoration.Pen != null)
					{
						textDecorationLineStyle = ((textDecoration.Pen.DashStyle.Dashes.Count > 1) ? TextDecorationLineStyle.Dash : TextDecorationLineStyle.Single);
					}
					else
					{
						textDecorationLineStyle = TextDecorationLineStyle.Single;
					}
				}
			}
			return textDecorationLineStyle;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0010833C File Offset: 0x0010733C
		private static double NativeObjectLengthToPoints(double length)
		{
			if (!DoubleUtil.IsNaN(length))
			{
				return length * 72.0 / 96.0;
			}
			return 0.0;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00108368 File Offset: 0x00107368
		private AutomationPeer GetEnclosingAutomationPeer(ITextPointer start, ITextPointer end)
		{
			ITextPointer textPointer;
			ITextPointer textPointer2;
			AutomationPeer automationPeer = TextContainerHelper.GetEnclosingAutomationPeer(start, end, out textPointer, out textPointer2);
			if (automationPeer == null)
			{
				automationPeer = this._textPeer;
			}
			else
			{
				Invariant.Assert(textPointer != null && textPointer2 != null);
				AutomationPeer enclosingAutomationPeer = this.GetEnclosingAutomationPeer(textPointer, textPointer2);
				this.GetAutomationPeersFromRange(enclosingAutomationPeer, textPointer, textPointer2);
			}
			return automationPeer;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x001083B0 File Offset: 0x001073B0
		private IRawElementProviderSimple ProviderFromPeer(AutomationPeer peer)
		{
			IRawElementProviderSimple result;
			if (this._textPeer is TextAutomationPeer)
			{
				result = ((TextAutomationPeer)this._textPeer).ProviderFromPeer(peer);
			}
			else
			{
				result = ((ContentTextAutomationPeer)this._textPeer).ProviderFromPeer(peer);
			}
			return result;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x001083F4 File Offset: 0x001073F4
		private List<AutomationPeer> GetAutomationPeersFromRange(AutomationPeer peer, ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(peer is TextAutomationPeer || peer is ContentTextAutomationPeer);
			List<AutomationPeer> automationPeersFromRange;
			if (peer is TextAutomationPeer)
			{
				automationPeersFromRange = ((TextAutomationPeer)peer).GetAutomationPeersFromRange(start, end);
			}
			else
			{
				automationPeersFromRange = ((ContentTextAutomationPeer)peer).GetAutomationPeersFromRange(start, end);
			}
			return automationPeersFromRange;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00108441 File Offset: 0x00107441
		private static bool IsAtWordBoundary(ITextPointer position)
		{
			return TextPointerBase.IsAtInsertionPosition(position) && TextPointerBase.IsAtWordBoundary(position, LogicalDirection.Forward);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00108454 File Offset: 0x00107454
		private static bool MoveToNextWordBoundary(ITextPointer position, LogicalDirection direction)
		{
			int num = 0;
			ITextPointer position2 = position.CreatePointer();
			while (position.MoveToNextInsertionPosition(direction))
			{
				num++;
				if (TextRangeAdaptor.IsAtWordBoundary(position))
				{
					break;
				}
				if (num > 128)
				{
					position.MoveToPosition(position2);
					position.MoveToNextContextPosition(direction);
					break;
				}
			}
			if (num > 0)
			{
				position.SetLogicalDirection(LogicalDirection.Forward);
			}
			return num > 0;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x001084AC File Offset: 0x001074AC
		private void Normalize()
		{
			TextRangeAdaptor.MoveToInsertionPosition(this._start, this._start.LogicalDirection);
			TextRangeAdaptor.MoveToInsertionPosition(this._end, this._end.LogicalDirection);
			if (this._start.CompareTo(this._end) > 0)
			{
				this._end.MoveToPosition(this._start);
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0010850C File Offset: 0x0010750C
		private ITextPointer GetInsertionPosition(ITextPointer position, LogicalDirection direction)
		{
			position = position.CreatePointer();
			TextRangeAdaptor.MoveToInsertionPosition(position, direction);
			return position;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0010851F File Offset: 0x0010751F
		ITextRangeProvider ITextRangeProvider.Clone()
		{
			return new TextRangeAdaptor(this._textAdaptor, this._start, this._end, this._textPeer);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00108540 File Offset: 0x00107540
		bool ITextRangeProvider.Compare(ITextRangeProvider range)
		{
			if (range == null)
			{
				throw new ArgumentNullException("range");
			}
			TextRangeAdaptor textRangeAdaptor = this.ValidateAndThrow(range);
			this.Normalize();
			textRangeAdaptor.Normalize();
			return textRangeAdaptor._start.CompareTo(this._start) == 0 && textRangeAdaptor._end.CompareTo(this._end) == 0;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00108598 File Offset: 0x00107598
		int ITextRangeProvider.CompareEndpoints(TextPatternRangeEndpoint endpoint, ITextRangeProvider targetRange, TextPatternRangeEndpoint targetEndpoint)
		{
			if (targetRange == null)
			{
				throw new ArgumentNullException("targetRange");
			}
			TextRangeAdaptor textRangeAdaptor = this.ValidateAndThrow(targetRange);
			this.Normalize();
			textRangeAdaptor.Normalize();
			ITextPointer textPointer = (endpoint == TextPatternRangeEndpoint.Start) ? this._start : this._end;
			ITextPointer position = (targetEndpoint == TextPatternRangeEndpoint.Start) ? textRangeAdaptor._start : textRangeAdaptor._end;
			return textPointer.CompareTo(position);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x001085F0 File Offset: 0x001075F0
		void ITextRangeProvider.ExpandToEnclosingUnit(TextUnit unit)
		{
			this.Normalize();
			this._end.MoveToPosition(this._start);
			this._end.MoveToNextInsertionPosition(LogicalDirection.Forward);
			this.ExpandToEnclosingUnit(unit, true, true);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00108620 File Offset: 0x00107620
		ITextRangeProvider ITextRangeProvider.FindAttribute(int attributeId, object value, bool backward)
		{
			AutomationTextAttribute automationTextAttribute = AutomationTextAttribute.LookupById(attributeId);
			if (automationTextAttribute == null)
			{
				throw new ArgumentNullException("attributeId");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!TextRangeAdaptor._textPatternAttributes.ContainsKey(automationTextAttribute))
			{
				return null;
			}
			this.Normalize();
			ITextRangeProvider result = null;
			ITextPointer textPointer = null;
			ITextPointer textPointer2 = null;
			TextRangeAdaptor.TextAttributeHelper textAttributeHelper = (TextRangeAdaptor.TextAttributeHelper)TextRangeAdaptor._textPatternAttributes[automationTextAttribute];
			if (backward)
			{
				ITextPointer start = this._start;
				ITextPointer textPointer3 = this._end.CreatePointer(LogicalDirection.Backward);
				textPointer = start;
				while (textPointer3.CompareTo(start) > 0)
				{
					if (textAttributeHelper.AreEqual(value, textAttributeHelper.GetValueAt(textPointer3)))
					{
						if (textPointer2 == null)
						{
							textPointer2 = textPointer3.CreatePointer(LogicalDirection.Backward);
						}
					}
					else if (textPointer2 != null)
					{
						textPointer = textPointer3.CreatePointer(LogicalDirection.Forward);
						break;
					}
					if (!textPointer3.MoveToNextContextPosition(LogicalDirection.Backward))
					{
						break;
					}
				}
			}
			else
			{
				ITextPointer end = this._end;
				ITextPointer textPointer4 = this._start.CreatePointer(LogicalDirection.Forward);
				textPointer2 = end;
				while (textPointer4.CompareTo(end) < 0)
				{
					if (textAttributeHelper.AreEqual(value, textAttributeHelper.GetValueAt(textPointer4)))
					{
						if (textPointer == null)
						{
							textPointer = textPointer4.CreatePointer(LogicalDirection.Forward);
						}
					}
					else if (textPointer != null)
					{
						textPointer2 = textPointer4.CreatePointer(LogicalDirection.Backward);
						break;
					}
					if (!textPointer4.MoveToNextContextPosition(LogicalDirection.Forward))
					{
						break;
					}
				}
			}
			if (textPointer != null && textPointer2 != null)
			{
				result = new TextRangeAdaptor(this._textAdaptor, textPointer, textPointer2, this._textPeer);
			}
			return result;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00108774 File Offset: 0x00107774
		ITextRangeProvider ITextRangeProvider.FindText(string text, bool backward, bool ignoreCase)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (text.Length == 0)
			{
				throw new ArgumentException(SR.Get("TextRangeProvider_EmptyStringParameter", new object[]
				{
					"text"
				}));
			}
			this.Normalize();
			if (this._start.CompareTo(this._end) == 0)
			{
				return null;
			}
			TextRangeAdaptor result = null;
			FindFlags findFlags = FindFlags.None;
			if (!ignoreCase)
			{
				findFlags |= FindFlags.MatchCase;
			}
			if (backward)
			{
				findFlags |= FindFlags.FindInReverse;
			}
			ITextRange textRange = TextFindEngine.Find(this._start, this._end, text, findFlags, CultureInfo.CurrentCulture);
			if (textRange != null && !textRange.IsEmpty)
			{
				result = new TextRangeAdaptor(this._textAdaptor, textRange.Start, textRange.End, this._textPeer);
			}
			return result;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00108828 File Offset: 0x00107828
		object ITextRangeProvider.GetAttributeValue(int attributeId)
		{
			AutomationTextAttribute automationTextAttribute = AutomationTextAttribute.LookupById(attributeId);
			if (automationTextAttribute == null || !TextRangeAdaptor._textPatternAttributes.ContainsKey(automationTextAttribute))
			{
				return AutomationElementIdentifiers.NotSupported;
			}
			this.Normalize();
			return this.GetAttributeValue((TextRangeAdaptor.TextAttributeHelper)TextRangeAdaptor._textPatternAttributes[automationTextAttribute]);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00108870 File Offset: 0x00107870
		double[] ITextRangeProvider.GetBoundingRectangles()
		{
			this.Normalize();
			Rect[] boundingRectangles = this._textAdaptor.GetBoundingRectangles(this._start, this._end, true, true);
			double[] array = new double[boundingRectangles.Length * 4];
			for (int i = 0; i < boundingRectangles.Length; i++)
			{
				array[4 * i] = boundingRectangles[i].X;
				array[4 * i + 1] = boundingRectangles[i].Y;
				array[4 * i + 2] = boundingRectangles[i].Width;
				array[4 * i + 3] = boundingRectangles[i].Height;
			}
			return array;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00108904 File Offset: 0x00107904
		IRawElementProviderSimple ITextRangeProvider.GetEnclosingElement()
		{
			this.Normalize();
			AutomationPeer enclosingAutomationPeer = this.GetEnclosingAutomationPeer(this._start, this._end);
			Invariant.Assert(enclosingAutomationPeer != null);
			IRawElementProviderSimple rawElementProviderSimple = this.ProviderFromPeer(enclosingAutomationPeer);
			Invariant.Assert(rawElementProviderSimple != null);
			return rawElementProviderSimple;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00108944 File Offset: 0x00107944
		string ITextRangeProvider.GetText(int maxLength)
		{
			if (maxLength < 0 && maxLength != -1)
			{
				throw new ArgumentException(SR.Get("TextRangeProvider_InvalidParameterValue", new object[]
				{
					maxLength,
					"maxLength"
				}));
			}
			this.Normalize();
			string textInternal = TextRangeBase.GetTextInternal(this._start, this._end);
			if (textInternal.Length > maxLength && maxLength != -1)
			{
				return textInternal.Substring(0, maxLength);
			}
			return textInternal;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x001089B0 File Offset: 0x001079B0
		int ITextRangeProvider.Move(TextUnit unit, int count)
		{
			this.Normalize();
			int num = 0;
			if (unit != TextUnit.Paragraph)
			{
				this.ExpandToEnclosingUnit(unit, true, true);
			}
			if (count != 0)
			{
				ITextPointer textPointer = this._start.CreatePointer();
				num = this.MovePositionByUnits(textPointer, unit, count);
				if ((textPointer.CompareTo(this._start) == 0 && textPointer.LogicalDirection != this._start.LogicalDirection) || (count > 0 && textPointer.CompareTo(this._start) > 0) || (count < 0 && textPointer.CompareTo(this._start) < 0))
				{
					this._start = textPointer;
					this._end = textPointer.CreatePointer();
					if (unit != TextUnit.Page)
					{
						this._end.MoveToNextInsertionPosition(LogicalDirection.Forward);
					}
					this.ExpandToEnclosingUnit(unit, true, true);
					if (num == 0)
					{
						num = ((count > 0) ? 1 : -1);
					}
				}
			}
			return num;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00108A70 File Offset: 0x00107A70
		int ITextRangeProvider.MoveEndpointByUnit(TextPatternRangeEndpoint endpoint, TextUnit unit, int count)
		{
			this.Normalize();
			int num = 0;
			if (count != 0)
			{
				bool flag = endpoint == TextPatternRangeEndpoint.Start;
				ITextPointer textPointer = flag ? this._start : this._end;
				ITextPointer textPointer2 = textPointer.CreatePointer();
				if (this.MoveToUnitBoundary(textPointer2, flag, (count < 0) ? LogicalDirection.Backward : LogicalDirection.Forward, unit))
				{
					num = ((count > 0) ? 1 : -1);
				}
				if (count != num)
				{
					num += this.MovePositionByUnits(textPointer2, unit, count - num);
				}
				if ((count > 0 && textPointer2.CompareTo(textPointer) > 0) || (count < 0 && textPointer2.CompareTo(textPointer) < 0) || (textPointer2.CompareTo(textPointer) == 0 && textPointer2.LogicalDirection != textPointer.LogicalDirection))
				{
					if (flag)
					{
						this._start = textPointer2;
					}
					else
					{
						this._end = textPointer2;
					}
					if (unit != TextUnit.Page)
					{
						this.ExpandToEnclosingUnit(unit, flag, !flag);
					}
					if (num == 0)
					{
						num = ((count > 0) ? 1 : -1);
					}
				}
				if (this._start.CompareTo(this._end) > 0)
				{
					if (flag)
					{
						this._end = this._start.CreatePointer();
					}
					else
					{
						this._start = this._end.CreatePointer();
					}
				}
			}
			return num;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00108B78 File Offset: 0x00107B78
		void ITextRangeProvider.MoveEndpointByRange(TextPatternRangeEndpoint endpoint, ITextRangeProvider targetRange, TextPatternRangeEndpoint targetEndpoint)
		{
			if (targetRange == null)
			{
				throw new ArgumentNullException("targetRange");
			}
			TextRangeAdaptor textRangeAdaptor = this.ValidateAndThrow(targetRange);
			ITextPointer textPointer = (targetEndpoint == TextPatternRangeEndpoint.Start) ? textRangeAdaptor._start : textRangeAdaptor._end;
			if (endpoint == TextPatternRangeEndpoint.Start)
			{
				this._start = textPointer.CreatePointer();
				if (this._start.CompareTo(this._end) > 0)
				{
					this._end = this._start.CreatePointer();
					return;
				}
			}
			else
			{
				this._end = textPointer.CreatePointer();
				if (this._start.CompareTo(this._end) > 0)
				{
					this._start = this._end.CreatePointer();
				}
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00108C13 File Offset: 0x00107C13
		void ITextRangeProvider.Select()
		{
			if (((ITextProvider)this._textAdaptor).SupportedTextSelection == SupportedTextSelection.None)
			{
				throw new InvalidOperationException(SR.Get("TextProvider_TextSelectionNotSupported"));
			}
			this.Normalize();
			this._textAdaptor.Select(this._start, this._end);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00108C4F File Offset: 0x00107C4F
		void ITextRangeProvider.AddToSelection()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00108C4F File Offset: 0x00107C4F
		void ITextRangeProvider.RemoveFromSelection()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00108C56 File Offset: 0x00107C56
		void ITextRangeProvider.ScrollIntoView(bool alignToTop)
		{
			this.Normalize();
			this._textAdaptor.ScrollIntoView(this._start, this._end, alignToTop);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00108C78 File Offset: 0x00107C78
		IRawElementProviderSimple[] ITextRangeProvider.GetChildren()
		{
			this.Normalize();
			IRawElementProviderSimple[] array = null;
			AutomationPeer enclosingAutomationPeer = this.GetEnclosingAutomationPeer(this._start, this._end);
			Invariant.Assert(enclosingAutomationPeer != null);
			List<AutomationPeer> automationPeersFromRange = this.GetAutomationPeersFromRange(enclosingAutomationPeer, this._start, this._end);
			if (automationPeersFromRange.Count > 0)
			{
				array = new IRawElementProviderSimple[automationPeersFromRange.Count];
				for (int i = 0; i < automationPeersFromRange.Count; i++)
				{
					array[i] = this.ProviderFromPeer(automationPeersFromRange[i]);
				}
			}
			return array;
		}

		// Token: 0x040006FE RID: 1790
		private ITextPointer _start;

		// Token: 0x040006FF RID: 1791
		private ITextPointer _end;

		// Token: 0x04000700 RID: 1792
		private TextAdaptor _textAdaptor;

		// Token: 0x04000701 RID: 1793
		private AutomationPeer _textPeer;

		// Token: 0x04000702 RID: 1794
		private static Hashtable _textPatternAttributes = new Hashtable();

		// Token: 0x04000703 RID: 1795
		private const string _defaultFamilyName = "Global User Interface";

		// Token: 0x020008BC RID: 2236
		private class TextAttributeHelper
		{
			// Token: 0x06008115 RID: 33045 RVA: 0x00322A98 File Offset: 0x00321A98
			internal TextAttributeHelper(TextRangeAdaptor.TextAttributeHelper.GetValueAtDelegate getValueDelegate, TextRangeAdaptor.TextAttributeHelper.AreEqualDelegate areEqualDelegate)
			{
				this._getValueDelegate = getValueDelegate;
				this._areEqualDelegate = areEqualDelegate;
			}

			// Token: 0x17001D90 RID: 7568
			// (get) Token: 0x06008116 RID: 33046 RVA: 0x00322AAE File Offset: 0x00321AAE
			internal TextRangeAdaptor.TextAttributeHelper.GetValueAtDelegate GetValueAt
			{
				get
				{
					return this._getValueDelegate;
				}
			}

			// Token: 0x17001D91 RID: 7569
			// (get) Token: 0x06008117 RID: 33047 RVA: 0x00322AB6 File Offset: 0x00321AB6
			internal TextRangeAdaptor.TextAttributeHelper.AreEqualDelegate AreEqual
			{
				get
				{
					return this._areEqualDelegate;
				}
			}

			// Token: 0x04003C24 RID: 15396
			private TextRangeAdaptor.TextAttributeHelper.GetValueAtDelegate _getValueDelegate;

			// Token: 0x04003C25 RID: 15397
			private TextRangeAdaptor.TextAttributeHelper.AreEqualDelegate _areEqualDelegate;

			// Token: 0x02000C76 RID: 3190
			// (Invoke) Token: 0x06009217 RID: 37399
			internal delegate object GetValueAtDelegate(ITextPointer textPointer);

			// Token: 0x02000C77 RID: 3191
			// (Invoke) Token: 0x0600921B RID: 37403
			internal delegate bool AreEqualDelegate(object val1, object val2);
		}
	}
}
