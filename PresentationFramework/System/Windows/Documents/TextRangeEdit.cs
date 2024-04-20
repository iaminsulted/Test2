using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006BA RID: 1722
	internal static class TextRangeEdit
	{
		// Token: 0x060058B3 RID: 22707 RVA: 0x00273B80 File Offset: 0x00272B80
		internal static TextElement InsertElementClone(TextPointer start, TextPointer end, TextElement element)
		{
			TextElement textElement = (TextElement)Activator.CreateInstance(element.GetType());
			textElement.TextContainer.SetValues(textElement.ContentStart, element.GetLocalValueEnumerator());
			textElement.Reposition(start, end);
			return textElement;
		}

		// Token: 0x060058B4 RID: 22708 RVA: 0x00273BBE File Offset: 0x00272BBE
		internal static TextPointer SplitFormattingElements(TextPointer splitPosition, bool keepEmptyFormatting)
		{
			return TextRangeEdit.SplitFormattingElements(splitPosition, keepEmptyFormatting, null);
		}

		// Token: 0x060058B5 RID: 22709 RVA: 0x00273BC8 File Offset: 0x00272BC8
		internal static TextPointer SplitFormattingElement(TextPointer splitPosition, bool keepEmptyFormatting)
		{
			Invariant.Assert(splitPosition.Parent != null && TextSchema.IsMergeableInline(splitPosition.Parent.GetType()));
			Inline inline = (Inline)splitPosition.Parent;
			if (splitPosition.IsFrozen)
			{
				splitPosition = new TextPointer(splitPosition);
			}
			if (!keepEmptyFormatting && splitPosition.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				splitPosition.MoveToPosition(inline.ElementStart);
			}
			else if (!keepEmptyFormatting && splitPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				splitPosition.MoveToPosition(inline.ElementEnd);
			}
			else
			{
				splitPosition = TextRangeEdit.SplitElement(splitPosition);
			}
			return splitPosition;
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x00273C50 File Offset: 0x00272C50
		private static bool InheritablePropertiesAreEqual(Inline firstInline, Inline secondInline)
		{
			Invariant.Assert(firstInline != null, "null check: firstInline");
			Invariant.Assert(secondInline != null, "null check: secondInline");
			foreach (DependencyProperty dependencyProperty in TextSchema.GetInheritableProperties(typeof(Inline)))
			{
				if (TextSchema.IsStructuralCharacterProperty(dependencyProperty))
				{
					if (firstInline.ReadLocalValue(dependencyProperty) != DependencyProperty.UnsetValue || secondInline.ReadLocalValue(dependencyProperty) != DependencyProperty.UnsetValue)
					{
						return false;
					}
				}
				else if (!TextSchema.ValuesAreEqual(firstInline.GetValue(dependencyProperty), secondInline.GetValue(dependencyProperty)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x00273CDC File Offset: 0x00272CDC
		private static bool CharacterPropertiesAreEqual(Inline firstElement, Inline secondElement)
		{
			Invariant.Assert(firstElement != null, "null check: firstElement");
			if (secondElement == null)
			{
				return false;
			}
			foreach (DependencyProperty dp in TextSchema.GetNoninheritableProperties(typeof(Span)))
			{
				if (!TextSchema.ValuesAreEqual(firstElement.GetValue(dp), secondElement.GetValue(dp)))
				{
					return false;
				}
			}
			return TextRangeEdit.InheritablePropertiesAreEqual(firstElement, secondElement);
		}

		// Token: 0x060058B8 RID: 22712 RVA: 0x00273D44 File Offset: 0x00272D44
		private static bool ExtractEmptyFormattingElements(TextPointer position)
		{
			bool result = false;
			Inline inline = position.Parent as Inline;
			if (inline != null && inline.IsEmpty)
			{
				while (inline != null && inline.IsEmpty)
				{
					if (TextSchema.IsFormattingType(inline.GetType()))
					{
						break;
					}
					inline.Reposition(null, null);
					result = true;
					inline = (position.Parent as Inline);
				}
				while (inline != null && inline.IsEmpty && (inline.GetType() == typeof(Run) || inline.GetType() == typeof(Span)))
				{
					if (TextRangeEdit.HasWriteableLocalPropertyValues(inline))
					{
						break;
					}
					inline.Reposition(null, null);
					result = true;
					inline = (position.Parent as Inline);
				}
				while (inline != null && inline.IsEmpty && ((inline.NextInline != null && TextSchema.IsFormattingType(inline.NextInline.GetType())) || (inline.PreviousInline != null && TextSchema.IsFormattingType(inline.PreviousInline.GetType()))))
				{
					inline.Reposition(null, null);
					result = true;
					inline = (position.Parent as Inline);
				}
			}
			return result;
		}

		// Token: 0x060058B9 RID: 22713 RVA: 0x00273E54 File Offset: 0x00272E54
		internal static void SetInlineProperty(TextPointer start, TextPointer end, DependencyProperty formattingProperty, object value, PropertyValueAction propertyValueAction)
		{
			if (start.CompareTo(end) >= 0 || (propertyValueAction == PropertyValueAction.SetValue && start.Parent is Run && start.Parent == end.Parent && TextSchema.ValuesAreEqual(start.Parent.GetValue(formattingProperty), value)))
			{
				return;
			}
			TextRangeEdit.RemoveUnnecessarySpans(start);
			TextRangeEdit.RemoveUnnecessarySpans(end);
			if (TextSchema.IsStructuralCharacterProperty(formattingProperty))
			{
				TextRangeEdit.SetStructuralInlineProperty(start, end, formattingProperty, value);
				return;
			}
			TextRangeEdit.SetNonStructuralInlineProperty(start, end, formattingProperty, value, propertyValueAction);
		}

		// Token: 0x060058BA RID: 22714 RVA: 0x00273EC8 File Offset: 0x00272EC8
		internal static bool MergeFormattingInlines(TextPointer position)
		{
			TextRangeEdit.RemoveUnnecessarySpans(position);
			TextRangeEdit.ExtractEmptyFormattingElements(position);
			while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				if (!TextSchema.IsMergeableInline(position.Parent.GetType()))
				{
					break;
				}
				position = ((Inline)position.Parent).ElementStart;
			}
			while (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd && TextSchema.IsMergeableInline(position.Parent.GetType()))
			{
				position = ((Inline)position.Parent).ElementEnd;
			}
			bool flag = false;
			Inline inline;
			Inline inline2;
			while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd && position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart && (inline = (position.GetAdjacentElement(LogicalDirection.Backward) as Inline)) != null && (inline2 = (position.GetAdjacentElement(LogicalDirection.Forward) as Inline)) != null)
			{
				if (TextSchema.IsFormattingType(inline.GetType()) && inline.TextRange.IsEmpty)
				{
					inline.RepositionWithContent(null);
					flag = true;
				}
				else if (TextSchema.IsFormattingType(inline2.GetType()) && inline2.TextRange.IsEmpty)
				{
					inline2.RepositionWithContent(null);
					flag = true;
				}
				else
				{
					if (!TextSchema.IsKnownType(inline.GetType()) || !TextSchema.IsKnownType(inline2.GetType()) || ((!(inline is Run) || !(inline2 is Run)) && (!(inline is Span) || !(inline2 is Span))) || !TextSchema.IsMergeableInline(inline.GetType()) || !TextSchema.IsMergeableInline(inline2.GetType()) || !TextRangeEdit.CharacterPropertiesAreEqual(inline, inline2))
					{
						break;
					}
					inline.Reposition(inline.ElementStart, inline2.ElementEnd);
					inline2.Reposition(null, null);
					flag = true;
				}
			}
			if (flag)
			{
				TextRangeEdit.RemoveUnnecessarySpans(position);
			}
			return flag;
		}

		// Token: 0x060058BB RID: 22715 RVA: 0x00274058 File Offset: 0x00273058
		private static void RemoveUnnecessarySpans(TextPointer position)
		{
			Inline inline = position.Parent as Inline;
			while (inline != null)
			{
				if (inline.Parent != null && TextSchema.IsMergeableInline(inline.Parent.GetType()) && TextSchema.IsKnownType(inline.Parent.GetType()) && inline.ElementStart.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && inline.ElementEnd.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
				{
					Span span = (Span)inline.Parent;
					if (span.Parent == null)
					{
						break;
					}
					foreach (DependencyProperty dp in TextSchema.GetInheritableProperties(typeof(Span)))
					{
						object value = inline.GetValue(dp);
						object value2 = span.GetValue(dp);
						if (TextSchema.ValuesAreEqual(value, value2))
						{
							object value3 = span.Parent.GetValue(dp);
							if (!TextSchema.ValuesAreEqual(value, value3))
							{
								inline.SetValue(dp, value2);
							}
						}
					}
					foreach (DependencyProperty dp2 in TextSchema.GetNoninheritableProperties(typeof(Span)))
					{
						bool flag2;
						bool flag = span.GetValueSource(dp2, null, out flag2) == BaseValueSourceInternal.Default && !flag2;
						if (inline.GetValueSource(dp2, null, out flag2) == BaseValueSourceInternal.Default && !flag2 && !flag)
						{
							inline.SetValue(dp2, span.GetValue(dp2));
						}
					}
					span.Reposition(null, null);
				}
				else
				{
					inline = (inline.Parent as Inline);
				}
			}
		}

		// Token: 0x060058BC RID: 22716 RVA: 0x002741DC File Offset: 0x002731DC
		internal static void CharacterResetFormatting(TextPointer start, TextPointer end)
		{
			if (start.CompareTo(end) < 0)
			{
				start = TextRangeEdit.SplitFormattingElements(start, false, true, null);
				end = TextRangeEdit.SplitFormattingElements(end, false, true, null);
				while (start.CompareTo(end) < 0)
				{
					if (start.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
					{
						TextElement textElement = (TextElement)start.Parent;
						if (!(textElement is Span) || textElement.ContentEnd.CompareTo(end) <= 0)
						{
							if (textElement is Span && TextSchema.IsKnownType(textElement.GetType()))
							{
								TextPointer elementStart = textElement.ElementStart;
								Span span = TextRangeEdit.TransferNonFormattingInlineProperties((Span)textElement);
								if (span != null)
								{
									span.Reposition(textElement.ElementStart, textElement.ElementEnd);
									elementStart = span.ElementStart;
								}
								textElement.Reposition(null, null);
								TextRangeEdit.MergeFormattingInlines(elementStart);
							}
							else if (textElement is Inline)
							{
								TextRangeEdit.ClearFormattingInlineProperties((Inline)textElement);
								TextRangeEdit.MergeFormattingInlines(textElement.ElementStart);
							}
						}
					}
					start = start.GetNextContextPosition(LogicalDirection.Forward);
				}
				TextRangeEdit.MergeFormattingInlines(end);
			}
		}

		// Token: 0x060058BD RID: 22717 RVA: 0x002742D4 File Offset: 0x002732D4
		private static void ClearFormattingInlineProperties(Inline inline)
		{
			LocalValueEnumerator localValueEnumerator = inline.GetLocalValueEnumerator();
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				DependencyProperty property = localValueEntry.Property;
				if (!property.ReadOnly && !TextSchema.IsNonFormattingCharacterProperty(property))
				{
					localValueEntry = localValueEnumerator.Current;
					inline.ClearValue(localValueEntry.Property);
				}
			}
		}

		// Token: 0x060058BE RID: 22718 RVA: 0x00274328 File Offset: 0x00273328
		private static Span TransferNonFormattingInlineProperties(Span source)
		{
			Span span = null;
			DependencyProperty[] nonFormattingCharacterProperties = TextSchema.GetNonFormattingCharacterProperties();
			for (int i = 0; i < nonFormattingCharacterProperties.Length; i++)
			{
				object value = source.GetValue(nonFormattingCharacterProperties[i]);
				object value2 = ((ITextPointer)source.ElementStart).GetValue(nonFormattingCharacterProperties[i]);
				if (!TextSchema.ValuesAreEqual(value, value2))
				{
					if (span == null)
					{
						span = new Span();
					}
					span.SetValue(nonFormattingCharacterProperties[i], value);
				}
			}
			return span;
		}

		// Token: 0x060058BF RID: 22719 RVA: 0x00274384 File Offset: 0x00273384
		internal static TextPointer SplitElement(TextPointer position)
		{
			TextElement textElement = (TextElement)position.Parent;
			if (position.IsFrozen)
			{
				position = new TextPointer(position);
			}
			if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				TextElement textElement2 = TextRangeEdit.InsertElementClone(textElement.ElementEnd, textElement.ElementEnd, textElement);
				position.MoveToPosition(textElement.ElementEnd);
			}
			else if (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				TextElement textElement2 = TextRangeEdit.InsertElementClone(textElement.ElementStart, textElement.ElementStart, textElement);
				position.MoveToPosition(textElement.ElementStart);
			}
			else
			{
				TextElement textElement2 = TextRangeEdit.InsertElementClone(position, textElement.ContentEnd, textElement);
				textElement.Reposition(textElement.ContentStart, textElement2.ElementStart);
				position.MoveToPosition(textElement.ElementEnd);
			}
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd, "position must be after ElementEnd");
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart, "position must be before ElementStart");
			return position;
		}

		// Token: 0x060058C0 RID: 22720 RVA: 0x00274458 File Offset: 0x00273458
		internal static TextPointer InsertParagraphBreak(TextPointer position, bool moveIntoSecondParagraph)
		{
			Invariant.Assert(position.TextContainer.Parent == null || TextSchema.IsValidChildOfContainer(position.TextContainer.Parent.GetType(), typeof(Paragraph)));
			bool flag = TextPointerBase.IsAtRowEnd(position) || TextPointerBase.IsBeforeFirstTable(position) || TextPointerBase.IsInBlockUIContainer(position);
			if (position.Paragraph == null)
			{
				position = TextRangeEditTables.EnsureInsertionPosition(position);
			}
			Inline nonMergeableInlineAncestor = position.GetNonMergeableInlineAncestor();
			if (nonMergeableInlineAncestor != null)
			{
				Invariant.Assert(TextPointerBase.IsPositionAtNonMergeableInlineBoundary(position), "Position must be at hyperlink boundary!");
				position = (position.IsAtNonMergeableInlineStart ? nonMergeableInlineAncestor.ElementStart : nonMergeableInlineAncestor.ElementEnd);
			}
			Paragraph paragraph = position.Paragraph;
			if (paragraph == null)
			{
				Invariant.Assert(position.TextContainer.Parent == null);
				paragraph = new Paragraph();
				paragraph.Reposition(position.DocumentStart, position.DocumentEnd);
			}
			if (flag)
			{
				return position;
			}
			TextPointer textPointer = position;
			textPointer = TextRangeEdit.SplitFormattingElements(textPointer, true);
			Invariant.Assert(textPointer.Parent == paragraph, "breakPosition must be in paragraph scope after splitting formatting elements");
			bool flag2 = TextPointerBase.GetImmediateListItem(paragraph.ContentStart) != null;
			textPointer = TextRangeEdit.SplitElement(textPointer);
			if (flag2)
			{
				Invariant.Assert(textPointer.Parent is ListItem, "breakPosition must be in ListItem scope");
				textPointer = TextRangeEdit.SplitElement(textPointer);
			}
			if (moveIntoSecondParagraph)
			{
				while (!(textPointer.Parent is Paragraph) && textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
				}
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
			}
			return textPointer;
		}

		// Token: 0x060058C1 RID: 22721 RVA: 0x002745B0 File Offset: 0x002735B0
		internal static TextPointer InsertLineBreak(TextPointer position)
		{
			if (!TextSchema.IsValidChild(position, typeof(LineBreak)))
			{
				position = TextRangeEditTables.EnsureInsertionPosition(position);
			}
			if (TextSchema.IsInTextContent(position))
			{
				position = TextRangeEdit.SplitElement(position);
			}
			Invariant.Assert(TextSchema.IsValidChild(position, typeof(LineBreak)), "position must be in valid scope now to insert a LineBreak element");
			LineBreak lineBreak = new LineBreak();
			position.InsertTextElement(lineBreak);
			return lineBreak.ElementEnd.GetInsertionPosition(LogicalDirection.Forward);
		}

		// Token: 0x060058C2 RID: 22722 RVA: 0x0027461A File Offset: 0x0027361A
		internal static void SetParagraphProperty(TextPointer start, TextPointer end, DependencyProperty property, object value)
		{
			TextRangeEdit.SetParagraphProperty(start, end, property, value, PropertyValueAction.SetValue);
		}

		// Token: 0x060058C3 RID: 22723 RVA: 0x00274628 File Offset: 0x00273628
		internal static void SetParagraphProperty(TextPointer start, TextPointer end, DependencyProperty property, object value, PropertyValueAction propertyValueAction)
		{
			Invariant.Assert(start != null, "null check: start");
			Invariant.Assert(end != null, "null check: end");
			Invariant.Assert(start.CompareTo(end) <= 0, "expecting: start <= end");
			Invariant.Assert(property != null, "null check: property");
			end = (TextPointer)TextRangeEdit.GetAdjustedRangeEnd(start, end);
			Block paragraphOrBlockUIContainer = start.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer != null)
			{
				start = paragraphOrBlockUIContainer.ContentStart;
			}
			if (property == Block.FlowDirectionProperty)
			{
				if (!TextRangeEditLists.SplitListsForFlowDirectionChange(start, end, value))
				{
					return;
				}
				ListItem listAncestor = start.GetListAncestor();
				if (listAncestor != null && listAncestor.List != null)
				{
					start = listAncestor.List.ElementStart;
				}
			}
			TextRangeEdit.SetParagraphPropertyWorker(start, end, property, value, propertyValueAction);
		}

		// Token: 0x060058C4 RID: 22724 RVA: 0x002746D4 File Offset: 0x002736D4
		private static void SetParagraphPropertyWorker(TextPointer start, TextPointer end, DependencyProperty property, object value, PropertyValueAction propertyValueAction)
		{
			for (Block nextBlock = TextRangeEdit.GetNextBlock(start, end); nextBlock != null; nextBlock = TextRangeEdit.GetNextBlock(start, end))
			{
				if (TextSchema.IsParagraphOrBlockUIContainer(nextBlock.GetType()))
				{
					TextRangeEdit.SetPropertyOnParagraphOrBlockUIContainer(start.TextContainer.Parent, nextBlock, property, value, propertyValueAction);
					start = nextBlock.ElementEnd.GetPositionAtOffset(0, LogicalDirection.Forward);
				}
				else if (nextBlock is List)
				{
					TextPointer nextContextPosition = nextBlock.ContentStart.GetPositionAtOffset(0, LogicalDirection.Forward).GetNextContextPosition(LogicalDirection.Forward);
					TextPointer contentEnd = nextBlock.ContentEnd;
					TextRangeEdit.SetParagraphPropertyWorker(nextContextPosition, contentEnd, property, value, propertyValueAction);
					if (property == Block.FlowDirectionProperty)
					{
						object value2 = nextBlock.GetValue(property);
						TextRangeEdit.SetPropertyValue(nextBlock, property, value2, value);
						if (!object.Equals(value2, value))
						{
							TextRangeEdit.SwapBlockLeftAndRightMargins(nextBlock);
						}
					}
					start = nextBlock.ElementEnd.GetPositionAtOffset(0, LogicalDirection.Forward);
				}
			}
		}

		// Token: 0x060058C5 RID: 22725 RVA: 0x00274794 File Offset: 0x00273794
		private static void SetPropertyOnParagraphOrBlockUIContainer(DependencyObject parent, Block block, DependencyProperty property, object value, PropertyValueAction propertyValueAction)
		{
			FlowDirection parentFlowDirection;
			if (parent != null)
			{
				parentFlowDirection = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
			}
			else
			{
				parentFlowDirection = (FlowDirection)FrameworkElement.FlowDirectionProperty.GetDefaultValue(typeof(FrameworkElement));
			}
			FlowDirection flowDirection = (FlowDirection)block.GetValue(Block.FlowDirectionProperty);
			object value2 = block.GetValue(property);
			object obj = value;
			TextRangeEdit.PreserveBlockContentStructuralProperty(block, property, value2, value);
			if (property.PropertyType == typeof(Thickness))
			{
				Invariant.Assert(value2 is Thickness, "Expecting the currentValue to be of Thinkness type");
				Invariant.Assert(obj is Thickness, "Expecting the newValue to be of Thinkness type");
				obj = TextRangeEdit.ComputeNewThicknessValue((Thickness)value2, (Thickness)obj, parentFlowDirection, flowDirection, propertyValueAction);
			}
			else if (property == Block.TextAlignmentProperty)
			{
				Invariant.Assert(value is TextAlignment, "Expecting TextAlignment as a value of a Paragraph.TextAlignmentProperty");
				obj = TextRangeEdit.ComputeNewTextAlignmentValue((TextAlignment)value, flowDirection);
				if (block is BlockUIContainer)
				{
					UIElement child = ((BlockUIContainer)block).Child;
					if (child != null)
					{
						HorizontalAlignment horizontalAlignmentFromTextAlignment = TextRangeEdit.GetHorizontalAlignmentFromTextAlignment((TextAlignment)obj);
						UIElementPropertyUndoUnit.Add(block.TextContainer, child, FrameworkElement.HorizontalAlignmentProperty, horizontalAlignmentFromTextAlignment);
						child.SetValue(FrameworkElement.HorizontalAlignmentProperty, horizontalAlignmentFromTextAlignment);
					}
				}
			}
			else if (value2 is double)
			{
				obj = TextRangeEdit.GetNewDoubleValue(property, (double)value2, (double)obj, propertyValueAction);
			}
			TextRangeEdit.SetPropertyValue(block, property, value2, obj);
			if (property == Block.FlowDirectionProperty && !object.Equals(value2, obj))
			{
				TextRangeEdit.SwapBlockLeftAndRightMargins(block);
			}
		}

		// Token: 0x060058C6 RID: 22726 RVA: 0x00274910 File Offset: 0x00273910
		private static void PreserveBlockContentStructuralProperty(Block block, DependencyProperty property, object currentValue, object newValue)
		{
			Paragraph paragraph = block as Paragraph;
			if (paragraph != null && TextSchema.IsStructuralCharacterProperty(property) && !TextSchema.ValuesAreEqual(currentValue, newValue))
			{
				Inline inline = paragraph.Inlines.FirstInline;
				Inline inline2 = paragraph.Inlines.LastInline;
				while (inline != null && inline == inline2 && inline is Span && !TextRangeEdit.HasLocalPropertyValue(inline, property))
				{
					inline = ((Span)inline).Inlines.FirstInline;
					inline2 = ((Span)inline2).Inlines.LastInline;
				}
				if (inline != inline2)
				{
					do
					{
						object value = inline.GetValue(property);
						inline2 = inline;
						Inline inline3;
						for (;;)
						{
							inline3 = (Inline)inline2.NextElement;
							if (inline3 == null || !TextSchema.ValuesAreEqual(inline3.GetValue(property), value))
							{
								break;
							}
							inline2 = inline3;
						}
						if (TextSchema.ValuesAreEqual(value, currentValue))
						{
							if (inline != inline2)
							{
								TextPointer frozenPointer = inline.ElementStart.GetFrozenPointer(LogicalDirection.Backward);
								TextPointer frozenPointer2 = inline2.ElementEnd.GetFrozenPointer(LogicalDirection.Forward);
								TextRangeEdit.SetStructuralInlineProperty(frozenPointer, frozenPointer2, property, currentValue);
								inline = (Inline)frozenPointer.GetAdjacentElement(LogicalDirection.Forward);
								inline2 = (Inline)frozenPointer2.GetAdjacentElement(LogicalDirection.Backward);
								if (inline != inline2)
								{
									Span span = inline.Parent as Span;
									if (span == null || span.Inlines.FirstInline != inline || span.Inlines.LastInline != inline2)
									{
										span = new Span(inline.ElementStart, inline2.ElementEnd);
									}
									span.SetValue(property, currentValue);
								}
							}
							if (inline == inline2)
							{
								TextRangeEdit.SetStructuralPropertyOnInline(inline, property, currentValue);
							}
						}
						inline = inline3;
					}
					while (inline != null);
					return;
				}
				TextRangeEdit.SetStructuralPropertyOnInline(inline, property, currentValue);
			}
		}

		// Token: 0x060058C7 RID: 22727 RVA: 0x00274A88 File Offset: 0x00273A88
		private static void SetStructuralPropertyOnInline(Inline inline, DependencyProperty property, object value)
		{
			if (inline is Run && !inline.IsEmpty && !TextRangeEdit.HasLocalPropertyValue(inline, property))
			{
				inline.SetValue(property, value);
			}
		}

		// Token: 0x060058C8 RID: 22728 RVA: 0x00274AAC File Offset: 0x00273AAC
		private static Block GetNextBlock(TextPointer pointer, TextPointer limit)
		{
			Block block = null;
			while (pointer != null && pointer.CompareTo(limit) <= 0)
			{
				if (pointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
				{
					block = (pointer.Parent as Block);
					if (block is Paragraph || block is BlockUIContainer || block is List)
					{
						break;
					}
				}
				if (TextPointerBase.IsAtPotentialParagraphPosition(pointer))
				{
					pointer = TextRangeEditTables.EnsureInsertionPosition(pointer);
					block = pointer.Paragraph;
					Invariant.Assert(block != null);
					break;
				}
				pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
			}
			return block;
		}

		// Token: 0x060058C9 RID: 22729 RVA: 0x00274B24 File Offset: 0x00273B24
		private static Thickness ComputeNewThicknessValue(Thickness currentThickness, Thickness newThickness, FlowDirection parentFlowDirection, FlowDirection flowDirection, PropertyValueAction propertyValueAction)
		{
			double top = (newThickness.Top < 0.0) ? currentThickness.Top : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Top, newThickness.Top, propertyValueAction);
			double bottom = (newThickness.Bottom < 0.0) ? currentThickness.Bottom : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Bottom, newThickness.Bottom, propertyValueAction);
			double left;
			double right;
			if (parentFlowDirection != flowDirection)
			{
				left = ((newThickness.Right < 0.0) ? currentThickness.Left : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Left, newThickness.Right, propertyValueAction));
				right = ((newThickness.Left < 0.0) ? currentThickness.Right : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Right, newThickness.Left, propertyValueAction));
			}
			else
			{
				left = ((newThickness.Left < 0.0) ? currentThickness.Left : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Left, newThickness.Left, propertyValueAction));
				right = ((newThickness.Right < 0.0) ? currentThickness.Right : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Right, newThickness.Right, propertyValueAction));
			}
			return new Thickness(left, top, right, bottom);
		}

		// Token: 0x060058CA RID: 22730 RVA: 0x00274C6C File Offset: 0x00273C6C
		private static TextAlignment ComputeNewTextAlignmentValue(TextAlignment textAlignment, FlowDirection flowDirection)
		{
			if (textAlignment == TextAlignment.Left)
			{
				textAlignment = ((flowDirection == FlowDirection.LeftToRight) ? TextAlignment.Left : TextAlignment.Right);
			}
			else if (textAlignment == TextAlignment.Right)
			{
				textAlignment = ((flowDirection == FlowDirection.LeftToRight) ? TextAlignment.Right : TextAlignment.Left);
			}
			return textAlignment;
		}

		// Token: 0x060058CB RID: 22731 RVA: 0x00274C8C File Offset: 0x00273C8C
		private static double GetNewDoubleValue(DependencyProperty property, double currentValue, double newValue, PropertyValueAction propertyValueAction)
		{
			double value = TextRangeEdit.NewValue(currentValue, newValue, propertyValueAction);
			return TextRangeEdit.DoublePropertyBounds.GetClosestValidValue(property, value);
		}

		// Token: 0x060058CC RID: 22732 RVA: 0x00274CAC File Offset: 0x00273CAC
		private static double NewValue(double currentValue, double newValue, PropertyValueAction propertyValueAction)
		{
			if (double.IsNaN(newValue))
			{
				return newValue;
			}
			if (double.IsNaN(currentValue))
			{
				currentValue = 0.0;
			}
			newValue = ((propertyValueAction == PropertyValueAction.IncreaseByAbsoluteValue) ? (currentValue + newValue) : ((propertyValueAction == PropertyValueAction.DecreaseByAbsoluteValue) ? (currentValue - newValue) : ((propertyValueAction == PropertyValueAction.IncreaseByPercentageValue) ? (currentValue * (1.0 + newValue / 100.0)) : ((propertyValueAction == PropertyValueAction.DecreaseByPercentageValue) ? (currentValue * (1.0 - newValue / 100.0)) : newValue))));
			return newValue;
		}

		// Token: 0x060058CD RID: 22733 RVA: 0x00274D28 File Offset: 0x00273D28
		internal static HorizontalAlignment GetHorizontalAlignmentFromTextAlignment(TextAlignment textAlignment)
		{
			HorizontalAlignment result;
			switch (textAlignment)
			{
			default:
				result = HorizontalAlignment.Left;
				break;
			case TextAlignment.Right:
				result = HorizontalAlignment.Right;
				break;
			case TextAlignment.Center:
				result = HorizontalAlignment.Center;
				break;
			case TextAlignment.Justify:
				result = HorizontalAlignment.Stretch;
				break;
			}
			return result;
		}

		// Token: 0x060058CE RID: 22734 RVA: 0x00274D5C File Offset: 0x00273D5C
		internal static TextAlignment GetTextAlignmentFromHorizontalAlignment(HorizontalAlignment horizontalAlignment)
		{
			switch (horizontalAlignment)
			{
			case HorizontalAlignment.Left:
				return TextAlignment.Left;
			case HorizontalAlignment.Center:
				return TextAlignment.Center;
			case HorizontalAlignment.Right:
				return TextAlignment.Right;
			}
			return TextAlignment.Justify;
		}

		// Token: 0x060058CF RID: 22735 RVA: 0x00274D90 File Offset: 0x00273D90
		private static void SetPropertyValue(TextElement element, DependencyProperty property, object currentValue, object newValue)
		{
			if (!TextSchema.ValuesAreEqual(newValue, currentValue))
			{
				element.ClearValue(property);
				if (!TextSchema.ValuesAreEqual(newValue, element.GetValue(property)))
				{
					element.SetValue(property, newValue);
				}
			}
		}

		// Token: 0x060058D0 RID: 22736 RVA: 0x00274DBC File Offset: 0x00273DBC
		private static void SwapBlockLeftAndRightMargins(Block block)
		{
			object value = block.GetValue(Block.MarginProperty);
			if (value is Thickness && !Paragraph.IsMarginAuto((Thickness)value))
			{
				object newValue = new Thickness(((Thickness)value).Right, ((Thickness)value).Top, ((Thickness)value).Left, ((Thickness)value).Bottom);
				TextRangeEdit.SetPropertyValue(block, Block.MarginProperty, value, newValue);
			}
		}

		// Token: 0x060058D1 RID: 22737 RVA: 0x00274E3A File Offset: 0x00273E3A
		internal static ITextPointer GetAdjustedRangeEnd(ITextPointer rangeStart, ITextPointer rangeEnd)
		{
			if (rangeStart.CompareTo(rangeEnd) < 0 && rangeEnd.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				rangeEnd = rangeEnd.GetNextInsertionPosition(LogicalDirection.Backward);
				if (rangeEnd == null)
				{
					rangeEnd = rangeStart;
				}
			}
			else if (TextPointerBase.IsAfterLastParagraph(rangeEnd))
			{
				rangeEnd = rangeEnd.GetInsertionPosition(LogicalDirection.Backward);
			}
			return rangeEnd;
		}

		// Token: 0x060058D2 RID: 22738 RVA: 0x00274E74 File Offset: 0x00273E74
		internal static void MergeFlowDirection(TextPointer position)
		{
			TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Backward);
			TextPointerContext pointerContext2 = position.GetPointerContext(LogicalDirection.Forward);
			if (pointerContext != TextPointerContext.ElementStart && pointerContext != TextPointerContext.ElementEnd && pointerContext2 != TextPointerContext.ElementStart && pointerContext2 != TextPointerContext.ElementEnd)
			{
				return;
			}
			while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				if (!TextSchema.IsMergeableInline(position.Parent.GetType()))
				{
					break;
				}
				position = ((Inline)position.Parent).ElementStart;
			}
			while (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd && TextSchema.IsMergeableInline(position.Parent.GetType()))
			{
				position = ((Inline)position.Parent).ElementEnd;
			}
			TextElement textElement = position.Parent as TextElement;
			if (!(textElement is Span) && !(textElement is Paragraph))
			{
				return;
			}
			TextPointer textPointer = position.CreatePointer();
			while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd && TextSchema.IsMergeableInline(textPointer.GetAdjacentElement(LogicalDirection.Backward).GetType()))
			{
				textPointer = ((Inline)textPointer.GetAdjacentElement(LogicalDirection.Backward)).ContentEnd;
			}
			Run run = textPointer.Parent as Run;
			TextPointer textPointer2 = position.CreatePointer();
			while (textPointer2.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart && TextSchema.IsMergeableInline(textPointer2.GetAdjacentElement(LogicalDirection.Forward).GetType()))
			{
				textPointer2 = ((Inline)textPointer2.GetAdjacentElement(LogicalDirection.Forward)).ContentStart;
			}
			Run run2 = textPointer2.Parent as Run;
			if (run == null || run.IsEmpty || run2 == null || run2.IsEmpty)
			{
				return;
			}
			FlowDirection flowDirection = (FlowDirection)textElement.GetValue(FrameworkElement.FlowDirectionProperty);
			FlowDirection flowDirection2 = (FlowDirection)run.GetValue(FrameworkElement.FlowDirectionProperty);
			FlowDirection flowDirection3 = (FlowDirection)run2.GetValue(FrameworkElement.FlowDirectionProperty);
			if (flowDirection2 == flowDirection3 && flowDirection2 != flowDirection)
			{
				TextElement scopingFlowDirectionInline = TextRangeEdit.GetScopingFlowDirectionInline(run);
				Inline scopingFlowDirectionInline2 = TextRangeEdit.GetScopingFlowDirectionInline(run2);
				TextRangeEdit.SetStructuralInlineProperty(scopingFlowDirectionInline.ElementStart, scopingFlowDirectionInline2.ElementEnd, FrameworkElement.FlowDirectionProperty, flowDirection2);
			}
		}

		// Token: 0x060058D3 RID: 22739 RVA: 0x00275039 File Offset: 0x00274039
		internal static bool CanApplyStructuralInlineProperty(TextPointer start, TextPointer end)
		{
			return TextRangeEdit.ValidateApplyStructuralInlineProperty(start, end, TextPointer.GetCommonAncestor(start, end), null);
		}

		// Token: 0x060058D4 RID: 22740 RVA: 0x0027504C File Offset: 0x0027404C
		internal static void IncrementParagraphLeadingMargin(TextRange range, double increment, PropertyValueAction propertyValueAction)
		{
			Invariant.Assert(increment >= 0.0);
			Invariant.Assert(propertyValueAction > PropertyValueAction.SetValue);
			if (increment == 0.0)
			{
				return;
			}
			Thickness thickness = new Thickness(increment, -1.0, -1.0, -1.0);
			TextRangeEdit.SetParagraphProperty(range.Start, range.End, Block.MarginProperty, thickness, propertyValueAction);
		}

		// Token: 0x060058D5 RID: 22741 RVA: 0x002750C3 File Offset: 0x002740C3
		internal static void DeleteInlineContent(ITextPointer start, ITextPointer end)
		{
			TextRangeEdit.DeleteParagraphContent(start, end);
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x002750CC File Offset: 0x002740CC
		internal static void DeleteParagraphContent(ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(start != null, "null check: start");
			Invariant.Assert(end != null, "null check: end");
			Invariant.Assert(start.CompareTo(end) <= 0, "expecting: start <= end");
			if (!(start is TextPointer))
			{
				start.DeleteContentToPosition(end);
				return;
			}
			TextPointer textPointer = (TextPointer)start;
			TextPointer textPointer2 = (TextPointer)end;
			TextRangeEdit.DeleteEquiScopedContent(textPointer, textPointer2);
			TextRangeEdit.DeleteEquiScopedContent(textPointer2, textPointer);
			if (textPointer.CompareTo(textPointer2) < 0)
			{
				if (TextPointerBase.IsAfterLastParagraph(textPointer2))
				{
					while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
					{
						if (textPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.ElementEnd)
						{
							break;
						}
						TextElement textElement = (TextElement)textPointer.Parent;
						if (!(textElement is Inline) && !TextSchema.AllowsParagraphMerging(textElement.GetType()))
						{
							break;
						}
						textElement.RepositionWithContent(null);
					}
				}
				else
				{
					Block block = textPointer.ParagraphOrBlockUIContainer;
					Block block2 = textPointer2.ParagraphOrBlockUIContainer;
					if (block == null && TextPointerBase.IsInEmptyListItem(textPointer))
					{
						textPointer = TextRangeEditTables.EnsureInsertionPosition(textPointer);
						block = textPointer.Paragraph;
						Invariant.Assert(block != null, "EnsureInsertionPosition must create a paragraph inside list item - 1");
					}
					if (block2 == null && TextPointerBase.IsInEmptyListItem(textPointer2))
					{
						textPointer2 = TextRangeEditTables.EnsureInsertionPosition(textPointer2);
						block2 = textPointer2.Paragraph;
						Invariant.Assert(block2 != null, "EnsureInsertionPosition must create a paragraph inside list item - 2");
					}
					if (block != null && block2 != null)
					{
						TextRangeEditLists.MergeParagraphs(block, block2);
					}
					else
					{
						TextRangeEdit.MergeEmptyParagraphsAndBlockUIContainers(textPointer, textPointer2);
					}
				}
			}
			TextRangeEdit.MergeFormattingInlines(textPointer);
			TextRangeEdit.MergeFormattingInlines(textPointer2);
			if (textPointer.Parent is BlockUIContainer && ((BlockUIContainer)textPointer.Parent).IsEmpty)
			{
				((BlockUIContainer)textPointer.Parent).Reposition(null, null);
				return;
			}
			if (textPointer.Parent is Hyperlink && ((Hyperlink)textPointer.Parent).IsEmpty)
			{
				((Hyperlink)textPointer.Parent).Reposition(null, null);
				TextRangeEdit.MergeFormattingInlines(textPointer);
			}
		}

		// Token: 0x060058D7 RID: 22743 RVA: 0x00275284 File Offset: 0x00274284
		private static void MergeEmptyParagraphsAndBlockUIContainers(TextPointer startPosition, TextPointer endPosition)
		{
			Block paragraphOrBlockUIContainer = startPosition.ParagraphOrBlockUIContainer;
			Block paragraphOrBlockUIContainer2 = endPosition.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer is BlockUIContainer)
			{
				if (paragraphOrBlockUIContainer.IsEmpty)
				{
					paragraphOrBlockUIContainer.Reposition(null, null);
					return;
				}
				if (paragraphOrBlockUIContainer2 is Paragraph && Paragraph.HasNoTextContent((Paragraph)paragraphOrBlockUIContainer2))
				{
					paragraphOrBlockUIContainer2.RepositionWithContent(null);
					return;
				}
			}
			if (paragraphOrBlockUIContainer2 is BlockUIContainer)
			{
				if (paragraphOrBlockUIContainer2.IsEmpty)
				{
					paragraphOrBlockUIContainer2.Reposition(null, null);
					return;
				}
				if (paragraphOrBlockUIContainer2 is Paragraph && Paragraph.HasNoTextContent((Paragraph)paragraphOrBlockUIContainer))
				{
					paragraphOrBlockUIContainer.RepositionWithContent(null);
					return;
				}
			}
		}

		// Token: 0x060058D8 RID: 22744 RVA: 0x0027530C File Offset: 0x0027430C
		private static void DeleteEquiScopedContent(TextPointer start, TextPointer end)
		{
			Invariant.Assert(start != null, "null check: start");
			Invariant.Assert(end != null, "null check: end");
			if (start.CompareTo(end) == 0)
			{
				return;
			}
			if (start.Parent == end.Parent)
			{
				TextRangeEdit.DeleteContentBetweenPositions(start, end);
				return;
			}
			LogicalDirection logicalDirection;
			LogicalDirection direction;
			TextPointerContext textPointerContext;
			TextPointerContext textPointerContext2;
			ElementEdge edge;
			ElementEdge edge2;
			if (start.CompareTo(end) < 0)
			{
				logicalDirection = LogicalDirection.Forward;
				direction = LogicalDirection.Backward;
				textPointerContext = TextPointerContext.ElementStart;
				textPointerContext2 = TextPointerContext.ElementEnd;
				edge = ElementEdge.BeforeStart;
				edge2 = ElementEdge.AfterEnd;
			}
			else
			{
				logicalDirection = LogicalDirection.Backward;
				direction = LogicalDirection.Forward;
				textPointerContext = TextPointerContext.ElementEnd;
				textPointerContext2 = TextPointerContext.ElementStart;
				edge = ElementEdge.AfterEnd;
				edge2 = ElementEdge.BeforeStart;
			}
			TextPointer textPointer = new TextPointer(start);
			TextPointer textPointer2 = new TextPointer(start);
			while (textPointer2.CompareTo(end) != 0)
			{
				Invariant.Assert((logicalDirection == LogicalDirection.Forward && textPointer2.CompareTo(end) < 0) || (logicalDirection == LogicalDirection.Backward && textPointer2.CompareTo(end) > 0), "Inappropriate position ordering");
				Invariant.Assert(textPointer.Parent == textPointer2.Parent, "inconsistent position Parents: previous and next");
				TextPointerContext pointerContext = textPointer2.GetPointerContext(logicalDirection);
				if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.EmbeddedElement)
				{
					textPointer2.MoveToNextContextPosition(logicalDirection);
					if ((logicalDirection == LogicalDirection.Forward && textPointer2.CompareTo(end) > 0) || (logicalDirection == LogicalDirection.Backward && textPointer2.CompareTo(end) < 0))
					{
						Invariant.Assert(textPointer2.Parent == end.Parent, "inconsistent poaition Parents: next and end");
						textPointer2.MoveToPosition(end);
						break;
					}
				}
				else if (pointerContext == textPointerContext)
				{
					textPointer2.MoveToNextContextPosition(logicalDirection);
					((ITextPointer)textPointer2).MoveToElementEdge(edge2);
					if ((logicalDirection == LogicalDirection.Forward && textPointer2.CompareTo(end) >= 0) || (logicalDirection == LogicalDirection.Backward && textPointer2.CompareTo(end) <= 0))
					{
						textPointer2.MoveToNextContextPosition(direction);
						((ITextPointer)textPointer2).MoveToElementEdge(edge);
						break;
					}
				}
				else
				{
					if (pointerContext != textPointerContext2)
					{
						Invariant.Assert(false, "Not expecting None context here");
						Invariant.Assert(pointerContext == TextPointerContext.None, "Unknown pointer context");
						break;
					}
					TextRangeEdit.DeleteContentBetweenPositions(textPointer, textPointer2);
					if (!TextRangeEdit.ExtractEmptyFormattingElements(textPointer))
					{
						Invariant.Assert(textPointer2.GetPointerContext(logicalDirection) == textPointerContext2, "Unexpected context of nextPosition");
						textPointer2.MoveToNextContextPosition(logicalDirection);
					}
					textPointer.MoveToPosition(textPointer2);
				}
			}
			Invariant.Assert(textPointer.Parent == textPointer2.Parent, "inconsistent Parents: previousPosition, nextPosition");
			TextRangeEdit.DeleteContentBetweenPositions(textPointer, textPointer2);
		}

		// Token: 0x060058D9 RID: 22745 RVA: 0x00275518 File Offset: 0x00274518
		private static bool DeleteContentBetweenPositions(TextPointer one, TextPointer two)
		{
			Invariant.Assert(one.Parent == two.Parent, "inconsistent Parents: one and two");
			if (one.CompareTo(two) < 0)
			{
				one.TextContainer.DeleteContentInternal(one, two);
			}
			else if (one.CompareTo(two) > 0)
			{
				two.TextContainer.DeleteContentInternal(two, one);
			}
			Invariant.Assert(one.CompareTo(two) == 0, "Positions one and two must be equal now");
			return false;
		}

		// Token: 0x060058DA RID: 22746 RVA: 0x00275582 File Offset: 0x00274582
		private static TextPointer SplitFormattingElements(TextPointer splitPosition, bool keepEmptyFormatting, TextElement limitingAncestor)
		{
			return TextRangeEdit.SplitFormattingElements(splitPosition, keepEmptyFormatting, false, limitingAncestor);
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x00275590 File Offset: 0x00274590
		private static TextPointer SplitFormattingElements(TextPointer splitPosition, bool keepEmptyFormatting, bool preserveStructuralFormatting, TextElement limitingAncestor)
		{
			if (preserveStructuralFormatting)
			{
				Run run = splitPosition.Parent as Run;
				if (run != null && run != limitingAncestor && ((run.Parent != null && TextRangeEdit.HasLocalInheritableStructuralPropertyValue(run)) || (run.Parent == null && TextRangeEdit.HasLocalStructuralPropertyValue(run))))
				{
					Span destination = new Span(run.ElementStart, run.ElementEnd);
					TextRangeEdit.TransferStructuralProperties(run, destination);
				}
			}
			while (splitPosition.Parent != null && TextSchema.IsMergeableInline(splitPosition.Parent.GetType()) && splitPosition.Parent != limitingAncestor && (!preserveStructuralFormatting || (((Inline)splitPosition.Parent).Parent != null && !TextRangeEdit.HasLocalInheritableStructuralPropertyValue((Inline)splitPosition.Parent)) || (((Inline)splitPosition.Parent).Parent == null && !TextRangeEdit.HasLocalStructuralPropertyValue((Inline)splitPosition.Parent))))
			{
				splitPosition = TextRangeEdit.SplitFormattingElement(splitPosition, keepEmptyFormatting);
			}
			return splitPosition;
		}

		// Token: 0x060058DC RID: 22748 RVA: 0x00275668 File Offset: 0x00274668
		private static void TransferStructuralProperties(Inline source, Inline destination)
		{
			bool flag = source.Parent == destination;
			for (int i = 0; i < TextSchema.StructuralCharacterProperties.Length; i++)
			{
				DependencyProperty dp = TextSchema.StructuralCharacterProperties[i];
				if ((flag && TextRangeEdit.HasLocalInheritableStructuralPropertyValue(source)) || (!flag && TextRangeEdit.HasLocalStructuralPropertyValue(source)))
				{
					object value = source.GetValue(dp);
					source.ClearValue(dp);
					destination.SetValue(dp, value);
				}
			}
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x002756C8 File Offset: 0x002746C8
		private static bool HasWriteableLocalPropertyValues(Inline inline)
		{
			LocalValueEnumerator localValueEnumerator = inline.GetLocalValueEnumerator();
			bool flag = false;
			while (!flag && localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				flag = !localValueEntry.Property.ReadOnly;
			}
			return flag;
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x00275708 File Offset: 0x00274708
		private static bool HasLocalInheritableStructuralPropertyValue(Inline inline)
		{
			int i;
			for (i = 0; i < TextSchema.StructuralCharacterProperties.Length; i++)
			{
				DependencyProperty dp = TextSchema.StructuralCharacterProperties[i];
				if (!TextSchema.ValuesAreEqual(inline.GetValue(dp), inline.Parent.GetValue(dp)))
				{
					break;
				}
			}
			return i < TextSchema.StructuralCharacterProperties.Length;
		}

		// Token: 0x060058DF RID: 22751 RVA: 0x00275754 File Offset: 0x00274754
		private static bool HasLocalStructuralPropertyValue(Inline inline)
		{
			int i;
			for (i = 0; i < TextSchema.StructuralCharacterProperties.Length; i++)
			{
				DependencyProperty property = TextSchema.StructuralCharacterProperties[i];
				if (TextRangeEdit.HasLocalPropertyValue(inline, property))
				{
					break;
				}
			}
			return i < TextSchema.StructuralCharacterProperties.Length;
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x00275790 File Offset: 0x00274790
		private static bool HasLocalPropertyValue(Inline inline, DependencyProperty property)
		{
			bool flag;
			BaseValueSourceInternal valueSource = inline.GetValueSource(property, null, out flag);
			return valueSource != BaseValueSourceInternal.Unknown && valueSource != BaseValueSourceInternal.Default && valueSource != BaseValueSourceInternal.Inherited;
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x002757B8 File Offset: 0x002747B8
		private static Inline GetScopingFlowDirectionInline(Run run)
		{
			FlowDirection flowDirection = run.FlowDirection;
			Inline inline = run;
			while ((FlowDirection)inline.Parent.GetValue(FrameworkElement.FlowDirectionProperty) == flowDirection)
			{
				inline = (Span)inline.Parent;
			}
			return inline;
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x002757F8 File Offset: 0x002747F8
		private static void SetNonStructuralInlineProperty(TextPointer start, TextPointer end, DependencyProperty formattingProperty, object value, PropertyValueAction propertyValueAction)
		{
			start = TextRangeEdit.SplitFormattingElements(start, false, true, null);
			end = TextRangeEdit.SplitFormattingElements(end, false, true, null);
			TextPointer textPointer;
			for (Run nextRun = TextRangeEdit.GetNextRun(start, end); nextRun != null; nextRun = TextRangeEdit.GetNextRun(textPointer, end))
			{
				object value2 = nextRun.GetValue(formattingProperty);
				object newValue = value;
				if (propertyValueAction != PropertyValueAction.SetValue)
				{
					Invariant.Assert(formattingProperty == TextElement.FontSizeProperty, "Only FontSize can be incremented/decremented among character properties");
					newValue = TextRangeEdit.GetNewFontSizeValue((double)value2, (double)value, propertyValueAction);
				}
				TextRangeEdit.SetPropertyValue(nextRun, formattingProperty, value2, newValue);
				textPointer = nextRun.ElementEnd.GetPositionAtOffset(0, LogicalDirection.Forward);
				if (TextPointerBase.IsAtPotentialRunPosition(nextRun))
				{
					textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
				}
				TextRangeEdit.MergeFormattingInlines(nextRun.ContentStart);
			}
			TextRangeEdit.MergeFormattingInlines(end);
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x002758A4 File Offset: 0x002748A4
		private static double GetNewFontSizeValue(double currentValue, double value, PropertyValueAction propertyValueAction)
		{
			double num = value;
			if (propertyValueAction == PropertyValueAction.IncreaseByAbsoluteValue)
			{
				num = currentValue + value;
			}
			else if (propertyValueAction == PropertyValueAction.DecreaseByAbsoluteValue)
			{
				num = currentValue - value;
			}
			if (num < 0.75)
			{
				num = 0.75;
			}
			else if (num > 1638.0)
			{
				num = 1638.0;
			}
			return num;
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x002758F4 File Offset: 0x002748F4
		private static void SetStructuralInlineProperty(TextPointer start, TextPointer end, DependencyProperty formattingProperty, object value)
		{
			DependencyObject commonAncestor = TextPointer.GetCommonAncestor(start, end);
			TextRangeEdit.ValidateApplyStructuralInlineProperty(start, end, commonAncestor, formattingProperty);
			if (commonAncestor is Run)
			{
				TextRangeEdit.ApplyStructuralInlinePropertyAcrossRun(start, end, (Run)commonAncestor, formattingProperty, value);
				return;
			}
			if ((commonAncestor is Inline && !(commonAncestor is AnchoredBlock)) || commonAncestor is Paragraph)
			{
				Invariant.Assert(!(commonAncestor is InlineUIContainer));
				TextRangeEdit.ApplyStructuralInlinePropertyAcrossInline(start, end, (TextElement)commonAncestor, formattingProperty, value);
				return;
			}
			TextRangeEdit.ApplyStructuralInlinePropertyAcrossParagraphs(start, end, formattingProperty, value);
		}

		// Token: 0x060058E5 RID: 22757 RVA: 0x00275970 File Offset: 0x00274970
		private static void FixupStructuralPropertyEnvironment(Inline inline, DependencyProperty property)
		{
			TextRangeEdit.ClearParentStructuralPropertyValue(inline, property);
			for (Inline inline2 = inline; inline2 != null; inline2 = (inline2.Parent as Span))
			{
				Inline inline3 = (Inline)inline2.PreviousElement;
				if (inline3 != null)
				{
					TextRangeEdit.FlattenStructuralProperties(inline3);
					break;
				}
			}
			for (Inline inline4 = inline; inline4 != null; inline4 = (inline4.Parent as Span))
			{
				Inline inline5 = (Inline)inline4.NextElement;
				if (inline5 != null)
				{
					TextRangeEdit.FlattenStructuralProperties(inline5);
					return;
				}
			}
		}

		// Token: 0x060058E6 RID: 22758 RVA: 0x002759D8 File Offset: 0x002749D8
		private static void FlattenStructuralProperties(Inline inline)
		{
			Span span = inline as Span;
			for (Span span2 = inline.Parent as Span; span2 != null; span2 = (span2.Parent as Span))
			{
				if (span2.Inlines.FirstInline != span2.Inlines.LastInline)
				{
					break;
				}
				span = span2;
			}
			while (span != null && span.Inlines.FirstInline == span.Inlines.LastInline)
			{
				Inline firstInline = span.Inlines.FirstInline;
				TextRangeEdit.TransferStructuralProperties(span, firstInline);
				if (TextSchema.IsMergeableInline(span.GetType()) && TextSchema.IsKnownType(span.GetType()) && !TextRangeEdit.HasWriteableLocalPropertyValues(span))
				{
					span.Reposition(null, null);
				}
				span = (firstInline as Span);
			}
		}

		// Token: 0x060058E7 RID: 22759 RVA: 0x00275A84 File Offset: 0x00274A84
		private static void ClearParentStructuralPropertyValue(Inline child, DependencyProperty property)
		{
			Span span = null;
			Span span2 = child.Parent as Span;
			while (span2 != null && TextSchema.IsMergeableInline(span2.GetType()))
			{
				if (TextRangeEdit.HasLocalPropertyValue(span2, property))
				{
					span = span2;
				}
				span2 = (span2.Parent as Span);
			}
			if (span != null)
			{
				TextElement limitingAncestor = (TextElement)span.Parent;
				TextRangeEdit.SplitFormattingElements(child.ElementStart, false, limitingAncestor);
				Span span3 = (Span)TextRangeEdit.SplitFormattingElements(child.ElementEnd, false, limitingAncestor).GetAdjacentElement(LogicalDirection.Backward);
				while (span3 != null && span3 != child)
				{
					span3.ClearValue(property);
					Span span4 = span3.Inlines.FirstInline as Span;
					if (!TextRangeEdit.HasWriteableLocalPropertyValues(span3))
					{
						span3.Reposition(null, null);
					}
					span3 = span4;
				}
			}
		}

		// Token: 0x060058E8 RID: 22760 RVA: 0x00275B30 File Offset: 0x00274B30
		private static Run GetNextRun(TextPointer pointer, TextPointer limit)
		{
			Run result = null;
			while (pointer != null && pointer.CompareTo(limit) < 0 && (pointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.ElementStart || (result = (pointer.GetAdjacentElement(LogicalDirection.Forward) as Run)) == null))
			{
				if (TextPointerBase.IsAtPotentialRunPosition(pointer))
				{
					pointer = TextRangeEditTables.EnsureInsertionPosition(pointer);
					Invariant.Assert(pointer.Parent is Run);
					result = (pointer.Parent as Run);
					break;
				}
				pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
			}
			return result;
		}

		// Token: 0x060058E9 RID: 22761 RVA: 0x00275BA4 File Offset: 0x00274BA4
		private static void ClearPropertyValueFromSpansAndRuns(TextPointer start, TextPointer end, DependencyProperty formattingProperty)
		{
			start = start.GetPositionAtOffset(0, LogicalDirection.Forward);
			start = start.GetNextContextPosition(LogicalDirection.Forward);
			while (start != null && start.CompareTo(end) < 0)
			{
				if (start.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && TextSchema.IsFormattingType(start.Parent.GetType()))
				{
					start.Parent.ClearValue(formattingProperty);
					TextRangeEdit.MergeFormattingInlines(start);
				}
				start = start.GetNextContextPosition(LogicalDirection.Forward);
			}
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x00275C0C File Offset: 0x00274C0C
		private static void ApplyStructuralInlinePropertyAcrossRun(TextPointer start, TextPointer end, Run run, DependencyProperty formattingProperty, object value)
		{
			if (start.CompareTo(end) == 0)
			{
				if (run.IsEmpty)
				{
					run.SetValue(formattingProperty, value);
				}
			}
			else
			{
				start = TextRangeEdit.SplitFormattingElements(start, false, run.Parent as TextElement);
				end = TextRangeEdit.SplitFormattingElements(end, false, run.Parent as TextElement);
				run = (Run)start.GetAdjacentElement(LogicalDirection.Forward);
				run.SetValue(formattingProperty, value);
			}
			TextRangeEdit.FixupStructuralPropertyEnvironment(run, formattingProperty);
		}

		// Token: 0x060058EB RID: 22763 RVA: 0x00275C7C File Offset: 0x00274C7C
		private static void ApplyStructuralInlinePropertyAcrossInline(TextPointer start, TextPointer end, TextElement commonAncestor, DependencyProperty formattingProperty, object value)
		{
			start = TextRangeEdit.SplitFormattingElements(start, false, commonAncestor);
			end = TextRangeEdit.SplitFormattingElements(end, false, commonAncestor);
			DependencyObject adjacentElement = start.GetAdjacentElement(LogicalDirection.Forward);
			DependencyObject adjacentElement2 = end.GetAdjacentElement(LogicalDirection.Backward);
			if (adjacentElement == adjacentElement2 && (adjacentElement is Run || adjacentElement is Span))
			{
				Inline inline = (Inline)start.GetAdjacentElement(LogicalDirection.Forward);
				inline.SetValue(formattingProperty, value);
				TextRangeEdit.FixupStructuralPropertyEnvironment(inline, formattingProperty);
				if (adjacentElement is Span)
				{
					TextRangeEdit.ClearPropertyValueFromSpansAndRuns(inline.ContentStart, inline.ContentEnd, formattingProperty);
					return;
				}
			}
			else
			{
				Span span;
				if (commonAncestor is Span && start.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && end.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd && start.GetAdjacentElement(LogicalDirection.Backward) == commonAncestor)
				{
					span = (Span)commonAncestor;
				}
				else
				{
					span = new Span();
					span.Reposition(start, end);
				}
				span.SetValue(formattingProperty, value);
				TextRangeEdit.FixupStructuralPropertyEnvironment(span, formattingProperty);
				TextRangeEdit.ClearPropertyValueFromSpansAndRuns(span.ContentStart, span.ContentEnd, formattingProperty);
			}
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x00275D58 File Offset: 0x00274D58
		private static void ApplyStructuralInlinePropertyAcrossParagraphs(TextPointer start, TextPointer end, DependencyProperty formattingProperty, object value)
		{
			Invariant.Assert(start.Paragraph != null);
			Invariant.Assert(start.Paragraph.ContentEnd.CompareTo(end) < 0);
			TextRangeEdit.SetStructuralInlineProperty(start, start.Paragraph.ContentEnd, formattingProperty, value);
			start = start.Paragraph.ElementEnd;
			if (end.Paragraph != null)
			{
				TextRangeEdit.SetStructuralInlineProperty(end.Paragraph.ContentStart, end, formattingProperty, value);
				end = end.Paragraph.ElementStart;
			}
			while (start != null && start.CompareTo(end) < 0)
			{
				if (start.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && start.Parent is Paragraph)
				{
					Paragraph paragraph = (Paragraph)start.Parent;
					TextRangeEdit.SetStructuralInlineProperty(paragraph.ContentStart, paragraph.ContentEnd, formattingProperty, value);
					start = paragraph.ElementEnd;
				}
				start = start.GetNextContextPosition(LogicalDirection.Forward);
			}
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x00275E2C File Offset: 0x00274E2C
		private static bool ValidateApplyStructuralInlineProperty(TextPointer start, TextPointer end, DependencyObject commonAncestor, DependencyProperty property)
		{
			if (!(commonAncestor is Inline))
			{
				return true;
			}
			Inline inline = null;
			Inline inline2;
			for (inline2 = (Inline)start.Parent; inline2 != commonAncestor; inline2 = (Inline)inline2.Parent)
			{
				if (!TextSchema.IsMergeableInline(inline2.GetType()))
				{
					inline = inline2;
					commonAncestor = inline2;
					break;
				}
			}
			for (inline2 = (Inline)end.Parent; inline2 != commonAncestor; inline2 = (Inline)inline2.Parent)
			{
				if (!TextSchema.IsMergeableInline(inline2.GetType()))
				{
					inline = inline2;
					break;
				}
			}
			if (property != null && inline2 != commonAncestor)
			{
				throw new InvalidOperationException(SR.Get("TextRangeEdit_InvalidStructuralPropertyApply", new object[]
				{
					property,
					inline
				}));
			}
			return inline2 == commonAncestor;
		}

		// Token: 0x02000B6F RID: 2927
		internal static class DoublePropertyBounds
		{
			// Token: 0x06008DF6 RID: 36342 RVA: 0x0033FEDC File Offset: 0x0033EEDC
			internal static double GetClosestValidValue(DependencyProperty property, double value)
			{
				return TextRangeEdit.DoublePropertyBounds.GetValueRange(property).GetClosestValue(value);
			}

			// Token: 0x06008DF7 RID: 36343 RVA: 0x0033FEF8 File Offset: 0x0033EEF8
			private static TextRangeEdit.DoublePropertyBounds.DoublePropertyRange GetValueRange(DependencyProperty property)
			{
				for (int i = 0; i < TextRangeEdit.DoublePropertyBounds._ranges.Length; i++)
				{
					if (property == TextRangeEdit.DoublePropertyBounds._ranges[i].Property)
					{
						return TextRangeEdit.DoublePropertyBounds._ranges[i];
					}
				}
				return TextRangeEdit.DoublePropertyBounds.DefaultRange;
			}

			// Token: 0x17001F03 RID: 7939
			// (get) Token: 0x06008DF8 RID: 36344 RVA: 0x0033FF3B File Offset: 0x0033EF3B
			private static TextRangeEdit.DoublePropertyBounds.DoublePropertyRange DefaultRange
			{
				get
				{
					return TextRangeEdit.DoublePropertyBounds._ranges[0];
				}
			}

			// Token: 0x040048DF RID: 18655
			private static readonly TextRangeEdit.DoublePropertyBounds.DoublePropertyRange[] _ranges = new TextRangeEdit.DoublePropertyBounds.DoublePropertyRange[]
			{
				new TextRangeEdit.DoublePropertyBounds.DoublePropertyRange(null, 0.0, double.MaxValue),
				new TextRangeEdit.DoublePropertyBounds.DoublePropertyRange(Paragraph.TextIndentProperty, (double)(-(double)Math.Min(1000000, 3500000)), (double)Math.Min(1000000, 3500000))
			};

			// Token: 0x02000C91 RID: 3217
			private struct DoublePropertyRange
			{
				// Token: 0x06009575 RID: 38261 RVA: 0x0034DFA9 File Offset: 0x0034CFA9
				internal DoublePropertyRange(DependencyProperty property, double lowerBound, double upperBound)
				{
					Invariant.Assert(lowerBound < upperBound);
					this._lowerBound = lowerBound;
					this._upperBound = upperBound;
					this._property = property;
				}

				// Token: 0x06009576 RID: 38262 RVA: 0x0034DFC9 File Offset: 0x0034CFC9
				internal double GetClosestValue(double value)
				{
					return Math.Min(Math.Max(this._lowerBound, value), this._upperBound);
				}

				// Token: 0x1700200C RID: 8204
				// (get) Token: 0x06009577 RID: 38263 RVA: 0x0034DFE2 File Offset: 0x0034CFE2
				internal DependencyProperty Property
				{
					get
					{
						return this._property;
					}
				}

				// Token: 0x04004FCD RID: 20429
				private DependencyProperty _property;

				// Token: 0x04004FCE RID: 20430
				private double _lowerBound;

				// Token: 0x04004FCF RID: 20431
				private double _upperBound;
			}
		}
	}
}
