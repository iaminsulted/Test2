using System;
using System.Globalization;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020006B6 RID: 1718
	internal static class TextPointerBase
	{
		// Token: 0x060057D5 RID: 22485 RVA: 0x0026FF86 File Offset: 0x0026EF86
		internal static ITextPointer Min(ITextPointer position1, ITextPointer position2)
		{
			if (position1.CompareTo(position2) > 0)
			{
				return position2;
			}
			return position1;
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x0026FF95 File Offset: 0x0026EF95
		internal static ITextPointer Max(ITextPointer position1, ITextPointer position2)
		{
			if (position1.CompareTo(position2) < 0)
			{
				return position2;
			}
			return position1;
		}

		// Token: 0x060057D7 RID: 22487 RVA: 0x0026FFA4 File Offset: 0x0026EFA4
		internal static string GetTextInRun(ITextPointer position, LogicalDirection direction)
		{
			int textRunLength = position.GetTextRunLength(direction);
			char[] array = new char[textRunLength];
			Invariant.Assert(position.GetTextInRun(direction, array, 0, textRunLength) == textRunLength, "textLengths returned from GetTextRunLength and GetTextInRun are innconsistent");
			return new string(array);
		}

		// Token: 0x060057D8 RID: 22488 RVA: 0x0026FFE0 File Offset: 0x0026EFE0
		internal static int GetTextWithLimit(ITextPointer thisPointer, LogicalDirection direction, char[] textBuffer, int startIndex, int count, ITextPointer limit)
		{
			int result;
			if (limit == null)
			{
				result = thisPointer.GetTextInRun(direction, textBuffer, startIndex, count);
			}
			else if (direction == LogicalDirection.Forward && limit.CompareTo(thisPointer) <= 0)
			{
				result = 0;
			}
			else if (direction == LogicalDirection.Backward && limit.CompareTo(thisPointer) >= 0)
			{
				result = 0;
			}
			else
			{
				int num;
				if (direction == LogicalDirection.Forward)
				{
					num = Math.Min(count, thisPointer.GetOffsetToPosition(limit));
				}
				else
				{
					num = Math.Min(count, limit.GetOffsetToPosition(thisPointer));
				}
				num = Math.Min(count, num);
				result = thisPointer.GetTextInRun(direction, textBuffer, startIndex, num);
			}
			return result;
		}

		// Token: 0x060057D9 RID: 22489 RVA: 0x0027005F File Offset: 0x0026F05F
		internal static bool IsAtInsertionPosition(ITextPointer position)
		{
			return TextPointerBase.IsAtNormalizedPosition(position, true);
		}

		// Token: 0x060057DA RID: 22490 RVA: 0x00270068 File Offset: 0x0026F068
		internal static bool IsAtPotentialRunPosition(ITextPointer position)
		{
			bool flag = TextPointerBase.IsAtPotentialRunPosition(position, position);
			if (!flag)
			{
				flag = TextPointerBase.IsAtPotentialParagraphPosition(position);
			}
			return flag;
		}

		// Token: 0x060057DB RID: 22491 RVA: 0x00270088 File Offset: 0x0026F088
		internal static bool IsAtPotentialRunPosition(TextElement run)
		{
			return run is Run && run.IsEmpty && TextPointerBase.IsAtPotentialRunPosition(run.ElementStart, run.ElementEnd);
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x002700B0 File Offset: 0x0026F0B0
		private static bool IsAtPotentialRunPosition(ITextPointer backwardPosition, ITextPointer forwardPosition)
		{
			Invariant.Assert(backwardPosition.HasEqualScope(forwardPosition));
			if (TextSchema.IsValidChild(backwardPosition, typeof(Run)))
			{
				Type elementType = forwardPosition.GetElementType(LogicalDirection.Forward);
				Type elementType2 = backwardPosition.GetElementType(LogicalDirection.Backward);
				if (elementType != null && elementType2 != null)
				{
					TextPointerContext pointerContext = forwardPosition.GetPointerContext(LogicalDirection.Forward);
					TextPointerContext pointerContext2 = backwardPosition.GetPointerContext(LogicalDirection.Backward);
					if ((pointerContext2 == TextPointerContext.ElementStart && pointerContext == TextPointerContext.ElementEnd) || (pointerContext2 == TextPointerContext.ElementStart && TextSchema.IsNonFormattingInline(elementType) && !TextPointerBase.IsAtNonMergeableInlineStart(backwardPosition)) || (pointerContext == TextPointerContext.ElementEnd && TextSchema.IsNonFormattingInline(elementType2) && !TextPointerBase.IsAtNonMergeableInlineEnd(forwardPosition)) || (pointerContext2 == TextPointerContext.ElementEnd && pointerContext == TextPointerContext.ElementStart && TextSchema.IsNonFormattingInline(elementType2) && TextSchema.IsNonFormattingInline(elementType)) || (pointerContext2 == TextPointerContext.ElementEnd && typeof(Inline).IsAssignableFrom(elementType2) && !TextSchema.IsMergeableInline(elementType2) && !typeof(Run).IsAssignableFrom(elementType) && (pointerContext != TextPointerContext.ElementEnd || !TextPointerBase.IsAtNonMergeableInlineEnd(forwardPosition))) || (pointerContext == TextPointerContext.ElementStart && typeof(Inline).IsAssignableFrom(elementType) && !TextSchema.IsMergeableInline(elementType) && !typeof(Run).IsAssignableFrom(elementType2) && (pointerContext2 != TextPointerContext.ElementStart || !TextPointerBase.IsAtNonMergeableInlineStart(backwardPosition))))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x002701E4 File Offset: 0x0026F1E4
		internal static bool IsAtPotentialParagraphPosition(ITextPointer position)
		{
			Type parentType = position.ParentType;
			TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Backward);
			TextPointerContext pointerContext2 = position.GetPointerContext(LogicalDirection.Forward);
			if (pointerContext == TextPointerContext.ElementStart && pointerContext2 == TextPointerContext.ElementEnd)
			{
				return typeof(ListItem).IsAssignableFrom(parentType) || typeof(TableCell).IsAssignableFrom(parentType);
			}
			return pointerContext == TextPointerContext.None && pointerContext2 == TextPointerContext.None && (typeof(FlowDocumentView).IsAssignableFrom(parentType) || typeof(FlowDocument).IsAssignableFrom(parentType));
		}

		// Token: 0x060057DE RID: 22494 RVA: 0x00270264 File Offset: 0x0026F264
		internal static bool IsBeforeFirstTable(ITextPointer position)
		{
			int pointerContext = (int)position.GetPointerContext(LogicalDirection.Forward);
			TextPointerContext pointerContext2 = position.GetPointerContext(LogicalDirection.Backward);
			return pointerContext == 3 && (pointerContext2 == TextPointerContext.ElementStart || pointerContext2 == TextPointerContext.None) && typeof(Table).IsAssignableFrom(position.GetElementType(LogicalDirection.Forward));
		}

		// Token: 0x060057DF RID: 22495 RVA: 0x002702A2 File Offset: 0x0026F2A2
		internal static bool IsInBlockUIContainer(ITextPointer position)
		{
			return typeof(BlockUIContainer).IsAssignableFrom(position.ParentType);
		}

		// Token: 0x060057E0 RID: 22496 RVA: 0x002702B9 File Offset: 0x0026F2B9
		internal static bool IsAtBlockUIContainerStart(ITextPointer position)
		{
			return TextPointerBase.IsInBlockUIContainer(position) && position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart;
		}

		// Token: 0x060057E1 RID: 22497 RVA: 0x002702CF File Offset: 0x0026F2CF
		internal static bool IsAtBlockUIContainerEnd(ITextPointer position)
		{
			return TextPointerBase.IsInBlockUIContainer(position) && position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd;
		}

		// Token: 0x060057E2 RID: 22498 RVA: 0x002702E8 File Offset: 0x0026F2E8
		private static bool IsInAncestorScope(ITextPointer position, Type allowedParentType, Type limitingType)
		{
			ITextPointer textPointer = position.CreatePointer();
			Type parentType = textPointer.ParentType;
			while (parentType != null && allowedParentType.IsAssignableFrom(parentType))
			{
				if (limitingType.IsAssignableFrom(parentType))
				{
					return true;
				}
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
				parentType = textPointer.ParentType;
			}
			return false;
		}

		// Token: 0x060057E3 RID: 22499 RVA: 0x00270331 File Offset: 0x0026F331
		internal static bool IsInAnchoredBlock(ITextPointer position)
		{
			return TextPointerBase.IsInAncestorScope(position, typeof(TextElement), typeof(AnchoredBlock));
		}

		// Token: 0x060057E4 RID: 22500 RVA: 0x0027034D File Offset: 0x0026F34D
		internal static bool IsInHyperlinkScope(ITextPointer position)
		{
			return TextPointerBase.IsInAncestorScope(position, typeof(Inline), typeof(Hyperlink));
		}

		// Token: 0x060057E5 RID: 22501 RVA: 0x0027036C File Offset: 0x0026F36C
		internal static ITextPointer GetFollowingNonMergeableInlineContentStart(ITextPointer position)
		{
			ITextPointer textPointer = position.CreatePointer();
			bool flag = false;
			Type elementType;
			for (;;)
			{
				if (TextPointerBase.GetBorderingElementCategory(textPointer, LogicalDirection.Forward) == TextPointerBase.BorderingElementCategory.MergeableScopingInline)
				{
					do
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					while (TextPointerBase.GetBorderingElementCategory(textPointer, LogicalDirection.Forward) == TextPointerBase.BorderingElementCategory.MergeableScopingInline);
					flag = true;
				}
				elementType = textPointer.GetElementType(LogicalDirection.Forward);
				if (elementType == typeof(InlineUIContainer) || elementType == typeof(BlockUIContainer))
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
				}
				else
				{
					if (!(textPointer.ParentType == typeof(InlineUIContainer)) && !(textPointer.ParentType == typeof(BlockUIContainer)))
					{
						break;
					}
					textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
				}
				elementType = textPointer.GetElementType(LogicalDirection.Forward);
				if (!(elementType == typeof(InlineUIContainer)) && !(elementType == typeof(BlockUIContainer)))
				{
					textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
				}
				flag = true;
			}
			if (typeof(Inline).IsAssignableFrom(elementType) && !TextSchema.IsMergeableInline(elementType))
			{
				do
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
				while (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart);
				flag = true;
			}
			if (!flag)
			{
				return null;
			}
			return textPointer;
		}

		// Token: 0x060057E6 RID: 22502 RVA: 0x0027047F File Offset: 0x0026F47F
		internal static bool IsAtNonMergeableInlineStart(ITextPointer position)
		{
			return TextPointerBase.IsAtNonMergeableInlineEdge(position, LogicalDirection.Backward);
		}

		// Token: 0x060057E7 RID: 22503 RVA: 0x00270488 File Offset: 0x0026F488
		internal static bool IsAtNonMergeableInlineEnd(ITextPointer position)
		{
			return TextPointerBase.IsAtNonMergeableInlineEdge(position, LogicalDirection.Forward);
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x00270491 File Offset: 0x0026F491
		internal static bool IsPositionAtNonMergeableInlineBoundary(ITextPointer position)
		{
			return TextPointerBase.IsAtNonMergeableInlineStart(position) || TextPointerBase.IsAtNonMergeableInlineEnd(position);
		}

		// Token: 0x060057E9 RID: 22505 RVA: 0x002704A3 File Offset: 0x0026F4A3
		internal static bool IsAtFormatNormalizedPosition(ITextPointer position, LogicalDirection direction)
		{
			return TextPointerBase.IsAtNormalizedPosition(position, direction, false);
		}

		// Token: 0x060057EA RID: 22506 RVA: 0x002704AD File Offset: 0x0026F4AD
		internal static bool IsAtInsertionPosition(ITextPointer position, LogicalDirection direction)
		{
			return TextPointerBase.IsAtNormalizedPosition(position, direction, true);
		}

		// Token: 0x060057EB RID: 22507 RVA: 0x002704B8 File Offset: 0x0026F4B8
		internal static bool IsAtNormalizedPosition(ITextPointer position, LogicalDirection direction, bool respectCaretUnitBoundaries)
		{
			if (!TextPointerBase.IsAtNormalizedPosition(position, respectCaretUnitBoundaries))
			{
				return false;
			}
			if (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				return true;
			}
			if (TextSchema.IsFormattingType(position.GetElementType(direction)))
			{
				position = position.CreatePointer();
				while (TextSchema.IsFormattingType(position.GetElementType(direction)))
				{
					position.MoveToNextContextPosition(direction);
				}
				if (TextPointerBase.IsAtNormalizedPosition(position, respectCaretUnitBoundaries))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060057EC RID: 22508 RVA: 0x00270520 File Offset: 0x0026F520
		internal static int GetOffset(ITextPointer thisPosition)
		{
			return thisPosition.TextContainer.Start.GetOffsetToPosition(thisPosition);
		}

		// Token: 0x060057ED RID: 22509 RVA: 0x00270534 File Offset: 0x0026F534
		internal static bool IsAtWordBoundary(ITextPointer thisPosition, LogicalDirection insideWordDirection)
		{
			ITextPointer textPointer = thisPosition.CreatePointer();
			if (textPointer.GetPointerContext(insideWordDirection) != TextPointerContext.Text)
			{
				textPointer.MoveToInsertionPosition(insideWordDirection);
			}
			bool result;
			if (textPointer.GetPointerContext(insideWordDirection) == TextPointerContext.Text)
			{
				char[] text;
				int position;
				TextPointerBase.GetWordBreakerText(thisPosition, out text, out position);
				result = SelectionWordBreaker.IsAtWordBoundary(text, position, insideWordDirection);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x0027057C File Offset: 0x0026F57C
		internal static TextSegment GetWordRange(ITextPointer thisPosition)
		{
			return TextPointerBase.GetWordRange(thisPosition, LogicalDirection.Forward);
		}

		// Token: 0x060057EF RID: 22511 RVA: 0x00270588 File Offset: 0x0026F588
		internal static TextSegment GetWordRange(ITextPointer thisPosition, LogicalDirection direction)
		{
			if (!thisPosition.IsAtInsertionPosition)
			{
				thisPosition = thisPosition.GetInsertionPosition(direction);
			}
			if (!thisPosition.IsAtInsertionPosition)
			{
				return new TextSegment(thisPosition, thisPosition);
			}
			ITextPointer textPointer = thisPosition.CreatePointer();
			bool flag = TextPointerBase.MoveToNextWordBoundary(textPointer, direction);
			ITextPointer textPointer2 = textPointer;
			ITextPointer textPointer3;
			if (flag && TextPointerBase.IsAtWordBoundary(thisPosition, LogicalDirection.Forward))
			{
				textPointer3 = thisPosition;
			}
			else
			{
				ITextPointer textPointer4 = thisPosition.CreatePointer();
				TextPointerBase.MoveToNextWordBoundary(textPointer4, (direction == LogicalDirection.Backward) ? LogicalDirection.Forward : LogicalDirection.Backward);
				textPointer3 = textPointer4;
			}
			if (direction == LogicalDirection.Backward)
			{
				ITextPointer textPointer5 = textPointer3;
				textPointer3 = textPointer2;
				textPointer2 = textPointer5;
			}
			textPointer3 = TextPointerBase.RestrictWithinBlock(thisPosition, textPointer3, LogicalDirection.Backward);
			textPointer2 = TextPointerBase.RestrictWithinBlock(thisPosition, textPointer2, LogicalDirection.Forward);
			if (textPointer3.CompareTo(textPointer2) < 0)
			{
				textPointer3 = textPointer3.GetFrozenPointer(LogicalDirection.Backward);
				textPointer2 = textPointer2.GetFrozenPointer(LogicalDirection.Forward);
			}
			else
			{
				textPointer3 = textPointer2.GetFrozenPointer(LogicalDirection.Backward);
				textPointer2 = textPointer3;
			}
			Invariant.Assert(textPointer3.CompareTo(textPointer2) <= 0, "expecting wordStart <= wordEnd");
			return new TextSegment(textPointer3, textPointer2);
		}

		// Token: 0x060057F0 RID: 22512 RVA: 0x00270648 File Offset: 0x0026F648
		private static ITextPointer RestrictWithinBlock(ITextPointer position, ITextPointer limit, LogicalDirection direction)
		{
			Invariant.Assert(direction != LogicalDirection.Backward || position.CompareTo(limit) >= 0, "for backward direction position must be >= than limit");
			Invariant.Assert(direction != LogicalDirection.Forward || position.CompareTo(limit) <= 0, "for forward direcion position must be <= than linit");
			while ((direction == LogicalDirection.Backward) ? (position.CompareTo(limit) > 0) : (position.CompareTo(limit) < 0))
			{
				TextPointerContext pointerContext = position.GetPointerContext(direction);
				if (pointerContext == TextPointerContext.ElementStart || pointerContext == TextPointerContext.ElementEnd)
				{
					Type elementType = position.GetElementType(direction);
					if (!typeof(Inline).IsAssignableFrom(elementType))
					{
						limit = position;
						break;
					}
				}
				else if (pointerContext == TextPointerContext.EmbeddedElement)
				{
					limit = position;
					break;
				}
				position = position.GetNextContextPosition(direction);
			}
			return limit.GetInsertionPosition((direction == LogicalDirection.Backward) ? LogicalDirection.Forward : LogicalDirection.Backward);
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x002706FC File Offset: 0x0026F6FC
		internal static bool IsNextToPlainLineBreak(ITextPointer thisPosition, LogicalDirection direction)
		{
			char[] array = new char[2];
			int textInRun = thisPosition.GetTextInRun(direction, array, 0, 2);
			return (textInRun == 1 && TextPointerBase.IsCharUnicodeNewLine(array[0])) || (textInRun == 2 && ((direction == LogicalDirection.Backward && TextPointerBase.IsCharUnicodeNewLine(array[1])) || (direction == LogicalDirection.Forward && TextPointerBase.IsCharUnicodeNewLine(array[0]))));
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x0027074E File Offset: 0x0026F74E
		internal static bool IsCharUnicodeNewLine(char ch)
		{
			return Array.IndexOf<char>(TextPointerBase.NextLineCharacters, ch) > -1;
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x0027075E File Offset: 0x0026F75E
		internal static bool IsNextToRichLineBreak(ITextPointer thisPosition, LogicalDirection direction)
		{
			return TextPointerBase.IsNextToRichBreak(thisPosition, direction, typeof(LineBreak));
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x00270771 File Offset: 0x0026F771
		internal static bool IsNextToParagraphBreak(ITextPointer thisPosition, LogicalDirection direction)
		{
			return TextPointerBase.IsNextToRichBreak(thisPosition, direction, typeof(Paragraph));
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x00270784 File Offset: 0x0026F784
		internal static bool IsNextToAnyBreak(ITextPointer thisPosition, LogicalDirection direction)
		{
			if (!thisPosition.IsAtInsertionPosition)
			{
				thisPosition = thisPosition.GetInsertionPosition(direction);
			}
			return TextPointerBase.IsNextToPlainLineBreak(thisPosition, direction) || TextPointerBase.IsNextToRichBreak(thisPosition, direction, null);
		}

		// Token: 0x060057F6 RID: 22518 RVA: 0x002707AC File Offset: 0x0026F7AC
		internal static bool IsAtLineWrappingPosition(ITextPointer position, ITextView textView)
		{
			Invariant.Assert(position != null, "null check: position");
			if (!position.HasValidLayout)
			{
				return false;
			}
			Invariant.Assert(textView != null, "textView cannot be null because the position has valid layout");
			TextSegment lineRange = textView.GetLineRange(position);
			if (lineRange.IsNull)
			{
				return false;
			}
			if (position.LogicalDirection != LogicalDirection.Forward)
			{
				return position.CompareTo(lineRange.End) == 0;
			}
			return position.CompareTo(lineRange.Start) == 0;
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x0027081C File Offset: 0x0026F81C
		internal static bool IsAtRowEnd(ITextPointer thisPosition)
		{
			return typeof(TableRow).IsAssignableFrom(thisPosition.ParentType) && thisPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd && thisPosition.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.ElementStart;
		}

		// Token: 0x060057F8 RID: 22520 RVA: 0x0027084E File Offset: 0x0026F84E
		internal static bool IsAfterLastParagraph(ITextPointer thisPosition)
		{
			return thisPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.None && thisPosition.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd && !typeof(Inline).IsAssignableFrom(thisPosition.GetElementType(LogicalDirection.Backward));
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x0027087E File Offset: 0x0026F87E
		internal static bool IsAtParagraphOrBlockUIContainerStart(ITextPointer pointer)
		{
			if (TextPointerBase.IsAtPotentialParagraphPosition(pointer))
			{
				return true;
			}
			while (pointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				if (TextSchema.IsParagraphOrBlockUIContainer(pointer.ParentType))
				{
					return true;
				}
				pointer = pointer.GetNextContextPosition(LogicalDirection.Backward);
			}
			return false;
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x002708B0 File Offset: 0x0026F8B0
		internal static ListItem GetListItem(TextPointer pointer)
		{
			if (pointer.Parent is ListItem)
			{
				return (ListItem)pointer.Parent;
			}
			Block paragraphOrBlockUIContainer = pointer.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer != null)
			{
				return paragraphOrBlockUIContainer.Parent as ListItem;
			}
			return null;
		}

		// Token: 0x060057FB RID: 22523 RVA: 0x002708F0 File Offset: 0x0026F8F0
		internal static ListItem GetImmediateListItem(TextPointer position)
		{
			if (position.Parent is ListItem)
			{
				return (ListItem)position.Parent;
			}
			Block paragraphOrBlockUIContainer = position.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer != null && paragraphOrBlockUIContainer.Parent is ListItem && paragraphOrBlockUIContainer.ElementStart.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				return (ListItem)paragraphOrBlockUIContainer.Parent;
			}
			return null;
		}

		// Token: 0x060057FC RID: 22524 RVA: 0x0027094C File Offset: 0x0026F94C
		internal static bool IsInEmptyListItem(TextPointer position)
		{
			ListItem listItem = position.Parent as ListItem;
			return listItem != null && listItem.IsEmpty;
		}

		// Token: 0x060057FD RID: 22525 RVA: 0x00270970 File Offset: 0x0026F970
		internal static int MoveToLineBoundary(ITextPointer thisPointer, ITextView textView, int count)
		{
			return TextPointerBase.MoveToLineBoundary(thisPointer, textView, count, false);
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x0027097C File Offset: 0x0026F97C
		internal static int MoveToLineBoundary(ITextPointer thisPointer, ITextView textView, int count, bool respectNonMeargeableInlineStart)
		{
			Invariant.Assert(!thisPointer.IsFrozen, "Can't reposition a frozen pointer!");
			Invariant.Assert(textView != null, "Null TextView!");
			double num;
			ITextPointer positionAtNextLine = textView.GetPositionAtNextLine(thisPointer, double.NaN, count, out num, out count);
			if (!positionAtNextLine.IsAtInsertionPosition && (!respectNonMeargeableInlineStart || (!TextPointerBase.IsAtNonMergeableInlineStart(positionAtNextLine) && !TextPointerBase.IsAtNonMergeableInlineEnd(positionAtNextLine))))
			{
				positionAtNextLine.MoveToInsertionPosition(positionAtNextLine.LogicalDirection);
			}
			if (TextPointerBase.IsAtRowEnd(positionAtNextLine))
			{
				thisPointer.MoveToPosition(positionAtNextLine);
				thisPointer.SetLogicalDirection(positionAtNextLine.LogicalDirection);
			}
			else
			{
				TextSegment lineRange = textView.GetLineRange(positionAtNextLine);
				if (!lineRange.IsNull)
				{
					thisPointer.MoveToPosition(lineRange.Start);
					thisPointer.SetLogicalDirection(lineRange.Start.LogicalDirection);
				}
				else if (count > 0)
				{
					thisPointer.MoveToPosition(positionAtNextLine);
					thisPointer.SetLogicalDirection(positionAtNextLine.LogicalDirection);
				}
			}
			return count;
		}

		// Token: 0x060057FF RID: 22527 RVA: 0x00270A4F File Offset: 0x0026FA4F
		internal static Rect GetCharacterRect(ITextPointer thisPointer, LogicalDirection direction)
		{
			return TextPointerBase.GetCharacterRect(thisPointer, direction, true);
		}

		// Token: 0x06005800 RID: 22528 RVA: 0x00270A5C File Offset: 0x0026FA5C
		internal static Rect GetCharacterRect(ITextPointer thisPointer, LogicalDirection direction, bool transformToUiScope)
		{
			ITextView textView = thisPointer.TextContainer.TextView;
			Invariant.Assert(textView != null, "Null TextView!");
			Invariant.Assert(textView.RenderScope != null, "Null RenderScope");
			Invariant.Assert(thisPointer.TextContainer != null, "Null TextContainer");
			Invariant.Assert(thisPointer.TextContainer.Parent != null, "Null parent of TextContainer");
			if (!thisPointer.IsAtInsertionPosition)
			{
				ITextPointer insertionPosition = thisPointer.GetInsertionPosition(direction);
				if (insertionPosition != null)
				{
					thisPointer = insertionPosition;
				}
			}
			Rect rect = textView.GetRectangleFromTextPosition(thisPointer.CreatePointer(direction));
			if (transformToUiScope)
			{
				Visual visual;
				if (thisPointer.TextContainer.Parent is FlowDocument && textView.RenderScope is FlowDocumentView)
				{
					visual = (((FlowDocumentView)textView.RenderScope).TemplatedParent as Visual);
					if (visual == null && ((FlowDocumentView)textView.RenderScope).Parent is FrameworkElement)
					{
						visual = (((FrameworkElement)((FlowDocumentView)textView.RenderScope).Parent).TemplatedParent as Visual);
					}
				}
				else if (thisPointer.TextContainer.Parent is Visual)
				{
					Invariant.Assert(textView.RenderScope == thisPointer.TextContainer.Parent || ((Visual)thisPointer.TextContainer.Parent).IsAncestorOf(textView.RenderScope), "Unexpected location of RenderScope within visual tree");
					visual = (Visual)thisPointer.TextContainer.Parent;
				}
				else
				{
					visual = null;
				}
				if (visual != null && visual.IsAncestorOf(textView.RenderScope))
				{
					rect = textView.RenderScope.TransformToAncestor(visual).TransformBounds(rect);
				}
			}
			return rect;
		}

		// Token: 0x06005801 RID: 22529 RVA: 0x00270BE9 File Offset: 0x0026FBE9
		internal static bool MoveToFormatNormalizedPosition(ITextPointer thisNavigator, LogicalDirection direction)
		{
			return TextPointerBase.NormalizePosition(thisNavigator, direction, false);
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x00270BF3 File Offset: 0x0026FBF3
		internal static bool MoveToInsertionPosition(ITextPointer thisNavigator, LogicalDirection direction)
		{
			return TextPointerBase.NormalizePosition(thisNavigator, direction, true);
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x00270C00 File Offset: 0x0026FC00
		internal static bool MoveToNextInsertionPosition(ITextPointer thisNavigator, LogicalDirection direction)
		{
			Invariant.Assert(!thisNavigator.IsFrozen, "Can't reposition a frozen pointer!");
			bool flag = true;
			int num = (direction == LogicalDirection.Forward) ? 1 : -1;
			ITextPointer textPointer = thisNavigator.CreatePointer();
			if (!TextPointerBase.IsAtInsertionPosition(thisNavigator))
			{
				if (!TextPointerBase.MoveToInsertionPosition(thisNavigator, direction))
				{
					flag = false;
					goto IL_E9;
				}
				if (direction == LogicalDirection.Forward && textPointer.CompareTo(thisNavigator) < 0)
				{
					goto IL_E9;
				}
				if (direction == LogicalDirection.Backward && thisNavigator.CompareTo(textPointer) < 0)
				{
					goto IL_E9;
				}
			}
			while (TextSchema.IsFormattingType(thisNavigator.GetElementType(direction)))
			{
				thisNavigator.MoveByOffset(num);
			}
			while (thisNavigator.GetPointerContext(direction) != TextPointerContext.None)
			{
				thisNavigator.MoveByOffset(num);
				if (TextPointerBase.IsAtInsertionPosition(thisNavigator))
				{
					if (direction != LogicalDirection.Backward)
					{
						goto IL_E9;
					}
					while (TextSchema.IsFormattingType(thisNavigator.GetElementType(direction)))
					{
						thisNavigator.MoveByOffset(num);
					}
					TextPointerContext pointerContext = thisNavigator.GetPointerContext(direction);
					if (pointerContext == TextPointerContext.ElementStart || pointerContext == TextPointerContext.None)
					{
						num = -num;
						while (TextSchema.IsFormattingType(thisNavigator.GetElementType(LogicalDirection.Forward)) && !TextPointerBase.IsAtInsertionPosition(thisNavigator))
						{
							thisNavigator.MoveByOffset(num);
						}
						goto IL_E9;
					}
					goto IL_E9;
				}
			}
			thisNavigator.MoveToPosition(textPointer);
			flag = false;
			IL_E9:
			if (flag)
			{
				if (direction == LogicalDirection.Forward)
				{
					Invariant.Assert(thisNavigator.CompareTo(textPointer) > 0, "thisNavigator is expected to be moved from initialPosition - 1");
				}
				else
				{
					Invariant.Assert(thisNavigator.CompareTo(textPointer) < 0, "thisNavigator is expected to be moved from initialPosition - 2");
				}
			}
			else
			{
				Invariant.Assert(thisNavigator.CompareTo(textPointer) == 0, "thisNavigator must stay at initial position");
			}
			return flag;
		}

		// Token: 0x06005804 RID: 22532 RVA: 0x00270D40 File Offset: 0x0026FD40
		internal static bool MoveToNextWordBoundary(ITextPointer thisNavigator, LogicalDirection movingDirection)
		{
			int num = 0;
			Invariant.Assert(!thisNavigator.IsFrozen, "Can't reposition a frozen pointer!");
			ITextPointer position = thisNavigator.CreatePointer();
			while (thisNavigator.MoveToNextInsertionPosition(movingDirection))
			{
				num++;
				if (num > 64)
				{
					thisNavigator.MoveToPosition(position);
					thisNavigator.MoveToNextContextPosition(movingDirection);
					break;
				}
				if (TextPointerBase.IsAtWordBoundary(thisNavigator, LogicalDirection.Forward))
				{
					break;
				}
			}
			return num > 0;
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x00270D9C File Offset: 0x0026FD9C
		internal static ITextPointer GetFrozenPointer(ITextPointer thisPointer, LogicalDirection logicalDirection)
		{
			ITextPointer textPointer;
			if (thisPointer.IsFrozen && thisPointer.LogicalDirection == logicalDirection)
			{
				textPointer = thisPointer;
			}
			else
			{
				textPointer = thisPointer.CreatePointer(logicalDirection);
				textPointer.Freeze();
			}
			return textPointer;
		}

		// Token: 0x06005806 RID: 22534 RVA: 0x00270DCD File Offset: 0x0026FDCD
		internal static bool ValidateLayout(ITextPointer thisPointer, ITextView textView)
		{
			return textView != null && textView.Validate(thisPointer);
		}

		// Token: 0x06005807 RID: 22535 RVA: 0x00270DDC File Offset: 0x0026FDDC
		private static bool NormalizePosition(ITextPointer thisNavigator, LogicalDirection direction, bool respectCaretUnitBoundaries)
		{
			Invariant.Assert(!thisNavigator.IsFrozen, "Can't reposition a frozen pointer!");
			int num = 0;
			int num2;
			LogicalDirection direction2;
			TextPointerContext textPointerContext;
			TextPointerContext textPointerContext2;
			if (direction == LogicalDirection.Forward)
			{
				num2 = 1;
				direction2 = LogicalDirection.Backward;
				textPointerContext = TextPointerContext.ElementStart;
				textPointerContext2 = TextPointerContext.ElementEnd;
			}
			else
			{
				num2 = -1;
				direction2 = LogicalDirection.Forward;
				textPointerContext = TextPointerContext.ElementEnd;
				textPointerContext2 = TextPointerContext.ElementStart;
			}
			if (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
			{
				while (thisNavigator.GetPointerContext(direction) == textPointerContext && !typeof(Inline).IsAssignableFrom(thisNavigator.GetElementType(direction)))
				{
					if (TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
					{
						break;
					}
					thisNavigator.MoveToNextContextPosition(direction);
					num += num2;
				}
				while (thisNavigator.GetPointerContext(direction2) == textPointerContext2 && !typeof(Inline).IsAssignableFrom(thisNavigator.GetElementType(direction2)) && !TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
				{
					thisNavigator.MoveToNextContextPosition(direction2);
					num -= num2;
				}
			}
			num = TextPointerBase.LeaveNonMergeableInlineBoundary(thisNavigator, direction, num);
			if (respectCaretUnitBoundaries)
			{
				while (!TextPointerBase.IsAtCaretUnitBoundary(thisNavigator))
				{
					num += num2;
					thisNavigator.MoveByOffset(num2);
				}
			}
			while (TextSchema.IsMergeableInline(thisNavigator.GetElementType(direction)))
			{
				thisNavigator.MoveToNextContextPosition(direction);
				num += num2;
			}
			if (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
			{
				while (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
				{
					if (!TextSchema.IsMergeableInline(thisNavigator.GetElementType(direction2)))
					{
						break;
					}
					thisNavigator.MoveToNextContextPosition(direction2);
					num -= num2;
				}
				while (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
				{
					if (!thisNavigator.MoveToNextContextPosition(direction))
					{
						break;
					}
					num += num2;
				}
				while (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries) && thisNavigator.MoveToNextContextPosition(direction2))
				{
					num -= num2;
				}
				if (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
				{
					thisNavigator.MoveByOffset(-num);
				}
			}
			return num != 0;
		}

		// Token: 0x06005808 RID: 22536 RVA: 0x00270F44 File Offset: 0x0026FF44
		private static int LeaveNonMergeableInlineBoundary(ITextPointer thisNavigator, LogicalDirection direction, int symbolCount)
		{
			if (TextPointerBase.IsAtNonMergeableInlineStart(thisNavigator))
			{
				if (direction == LogicalDirection.Forward && TextPointerBase.IsAtNonMergeableInlineEnd(thisNavigator))
				{
					symbolCount += TextPointerBase.LeaveNonMergeableAncestor(thisNavigator, LogicalDirection.Forward);
				}
				else
				{
					symbolCount += TextPointerBase.LeaveNonMergeableAncestor(thisNavigator, LogicalDirection.Backward);
				}
			}
			else if (TextPointerBase.IsAtNonMergeableInlineEnd(thisNavigator))
			{
				if (direction == LogicalDirection.Backward && TextPointerBase.IsAtNonMergeableInlineStart(thisNavigator))
				{
					symbolCount += TextPointerBase.LeaveNonMergeableAncestor(thisNavigator, LogicalDirection.Backward);
				}
				else
				{
					symbolCount += TextPointerBase.LeaveNonMergeableAncestor(thisNavigator, LogicalDirection.Forward);
				}
			}
			return symbolCount;
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x00270FAC File Offset: 0x0026FFAC
		private static int LeaveNonMergeableAncestor(ITextPointer thisNavigator, LogicalDirection direction)
		{
			int num = 0;
			int num2 = (direction == LogicalDirection.Forward) ? 1 : -1;
			while (TextSchema.IsMergeableInline(thisNavigator.ParentType))
			{
				thisNavigator.MoveToNextContextPosition(direction);
				num += num2;
			}
			thisNavigator.MoveToNextContextPosition(direction);
			return num + num2;
		}

		// Token: 0x0600580A RID: 22538 RVA: 0x00270FEC File Offset: 0x0026FFEC
		private static bool IsAtNormalizedPosition(ITextPointer position, bool respectCaretUnitBoundaries)
		{
			if (TextPointerBase.IsPositionAtNonMergeableInlineBoundary(position))
			{
				return false;
			}
			if (TextSchema.IsValidChild(position, typeof(string)))
			{
				return !respectCaretUnitBoundaries || TextPointerBase.IsAtCaretUnitBoundary(position);
			}
			return TextPointerBase.IsAtRowEnd(position) || TextPointerBase.IsAtPotentialRunPosition(position) || TextPointerBase.IsBeforeFirstTable(position) || TextPointerBase.IsInBlockUIContainer(position);
		}

		// Token: 0x0600580B RID: 22539 RVA: 0x00271044 File Offset: 0x00270044
		private static bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Forward);
			bool result;
			if (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text && pointerContext == TextPointerContext.Text)
			{
				if (position.HasValidLayout)
				{
					result = position.IsAtCaretUnitBoundary;
				}
				else
				{
					result = !TextPointerBase.IsInsideCompoundSequence(position);
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x00271088 File Offset: 0x00270088
		private static bool IsInsideCompoundSequence(ITextPointer position)
		{
			char[] array = new char[2];
			if (position.GetTextInRun(LogicalDirection.Backward, array, 0, 1) == 1 && position.GetTextInRun(LogicalDirection.Forward, array, 1, 1) == 1)
			{
				if (char.IsSurrogatePair(array[0], array[1]) || (array[0] == '\r' && array[1] == '\n'))
				{
					return true;
				}
				UnicodeCategory unicodeCategory = char.GetUnicodeCategory(array[1]);
				if (unicodeCategory == UnicodeCategory.SpacingCombiningMark || unicodeCategory == UnicodeCategory.NonSpacingMark || unicodeCategory == UnicodeCategory.EnclosingMark)
				{
					UnicodeCategory unicodeCategory2 = char.GetUnicodeCategory(array[0]);
					if (unicodeCategory2 != UnicodeCategory.Control && unicodeCategory2 != UnicodeCategory.Format && unicodeCategory2 != UnicodeCategory.OtherNotAssigned)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x00271104 File Offset: 0x00270104
		private static void GetWordBreakerText(ITextPointer pointer, out char[] text, out int position)
		{
			char[] array = new char[SelectionWordBreaker.MinContextLength];
			char[] array2 = new char[SelectionWordBreaker.MinContextLength];
			int num = 0;
			int num2 = 0;
			ITextPointer textPointer = pointer.CreatePointer();
			do
			{
				int num3 = Math.Min(textPointer.GetTextRunLength(LogicalDirection.Backward), SelectionWordBreaker.MinContextLength - num);
				num += num3;
				textPointer.MoveByOffset(-num3);
				textPointer.GetTextInRun(LogicalDirection.Forward, array, SelectionWordBreaker.MinContextLength - num, num3);
				if (num == SelectionWordBreaker.MinContextLength)
				{
					break;
				}
				textPointer.MoveToInsertionPosition(LogicalDirection.Backward);
			}
			while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text);
			textPointer.MoveToPosition(pointer);
			do
			{
				int num3 = Math.Min(textPointer.GetTextRunLength(LogicalDirection.Forward), SelectionWordBreaker.MinContextLength - num2);
				textPointer.GetTextInRun(LogicalDirection.Forward, array2, num2, num3);
				num2 += num3;
				if (num2 == SelectionWordBreaker.MinContextLength)
				{
					break;
				}
				textPointer.MoveByOffset(num3);
				textPointer.MoveToInsertionPosition(LogicalDirection.Forward);
			}
			while (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text);
			text = new char[num + num2];
			Array.Copy(array, SelectionWordBreaker.MinContextLength - num, text, 0, num);
			Array.Copy(array2, 0, text, num, num2);
			position = num;
		}

		// Token: 0x0600580E RID: 22542 RVA: 0x00271208 File Offset: 0x00270208
		private static bool IsAtNonMergeableInlineEdge(ITextPointer position, LogicalDirection direction)
		{
			TextPointerBase.BorderingElementCategory borderingElementCategory = TextPointerBase.GetBorderingElementCategory(position, direction);
			if (borderingElementCategory == TextPointerBase.BorderingElementCategory.MergeableScopingInline)
			{
				ITextPointer textPointer = position.CreatePointer();
				do
				{
					textPointer.MoveToNextContextPosition(direction);
				}
				while ((borderingElementCategory = TextPointerBase.GetBorderingElementCategory(textPointer, direction)) == TextPointerBase.BorderingElementCategory.MergeableScopingInline);
			}
			return borderingElementCategory == TextPointerBase.BorderingElementCategory.NonMergeableScopingInline;
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x00271240 File Offset: 0x00270240
		private static TextPointerBase.BorderingElementCategory GetBorderingElementCategory(ITextPointer position, LogicalDirection direction)
		{
			TextPointerContext textPointerContext = (direction == LogicalDirection.Forward) ? TextPointerContext.ElementEnd : TextPointerContext.ElementStart;
			TextPointerBase.BorderingElementCategory result;
			if (position.GetPointerContext(direction) != textPointerContext || !typeof(Inline).IsAssignableFrom(position.ParentType))
			{
				result = TextPointerBase.BorderingElementCategory.NotScopingInline;
			}
			else if (TextSchema.IsMergeableInline(position.ParentType))
			{
				result = TextPointerBase.BorderingElementCategory.MergeableScopingInline;
			}
			else
			{
				result = TextPointerBase.BorderingElementCategory.NonMergeableScopingInline;
			}
			return result;
		}

		// Token: 0x06005810 RID: 22544 RVA: 0x00271290 File Offset: 0x00270290
		private static bool IsNextToRichBreak(ITextPointer thisPosition, LogicalDirection direction, Type lineBreakType)
		{
			Invariant.Assert(lineBreakType == null || lineBreakType == typeof(LineBreak) || lineBreakType == typeof(Paragraph));
			bool result = false;
			for (;;)
			{
				Type elementType = thisPosition.GetElementType(direction);
				if (lineBreakType == null)
				{
					if (typeof(LineBreak).IsAssignableFrom(elementType) || typeof(Paragraph).IsAssignableFrom(elementType))
					{
						break;
					}
				}
				else if (lineBreakType.IsAssignableFrom(elementType))
				{
					goto Block_5;
				}
				if (!TextSchema.IsFormattingType(elementType))
				{
					return result;
				}
				thisPosition = thisPosition.GetNextContextPosition(direction);
			}
			return true;
			Block_5:
			result = true;
			return result;
		}

		// Token: 0x04002FD8 RID: 12248
		internal static char[] NextLineCharacters = new char[]
		{
			'\n',
			'\r',
			'\v',
			'\f',
			'\u0085',
			'\u2028',
			'\u2029'
		};

		// Token: 0x02000B6C RID: 2924
		private enum BorderingElementCategory
		{
			// Token: 0x040048D6 RID: 18646
			MergeableScopingInline,
			// Token: 0x040048D7 RID: 18647
			NonMergeableScopingInline,
			// Token: 0x040048D8 RID: 18648
			NotScopingInline
		}
	}
}
