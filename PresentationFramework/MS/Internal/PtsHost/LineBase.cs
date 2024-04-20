using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000125 RID: 293
	internal abstract class LineBase : UnmanagedHandle
	{
		// Token: 0x060007FF RID: 2047 RVA: 0x001131FA File Offset: 0x001121FA
		internal LineBase(BaseParaClient paraClient) : base(paraClient.PtsContext)
		{
			this._paraClient = paraClient;
		}

		// Token: 0x06000800 RID: 2048
		internal abstract TextRun GetTextRun(int dcp);

		// Token: 0x06000801 RID: 2049
		internal abstract TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp);

		// Token: 0x06000802 RID: 2050
		internal abstract int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int dcp);

		// Token: 0x06000803 RID: 2051 RVA: 0x00113210 File Offset: 0x00112210
		protected TextRun HandleText(StaticTextPointer position)
		{
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text, "TextPointer does not point to characters.");
			DependencyObject target;
			if (position.Parent != null)
			{
				target = position.Parent;
			}
			else
			{
				target = this._paraClient.Paragraph.Element;
			}
			TextProperties textRunProperties = new TextProperties(target, position, false, true, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			StaticTextPointer position2 = position.TextContainer.Highlights.GetNextPropertyChangePosition(position, LogicalDirection.Forward);
			if (position.GetOffsetToPosition(position2) > 4096)
			{
				position2 = position.CreatePointer(4096);
			}
			char[] array = new char[position.GetOffsetToPosition(position2)];
			int textInRun = position.GetTextInRun(LogicalDirection.Forward, array, 0, array.Length);
			return new TextCharacters(array, 0, textInRun, textRunProperties);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x001132D4 File Offset: 0x001122D4
		protected TextRun HandleElementStartEdge(StaticTextPointer position)
		{
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart, "TextPointer does not point to element start edge.");
			TextElement textElement = (TextElement)position.GetAdjacentElement(LogicalDirection.Forward);
			Invariant.Assert(!(textElement is Block), "We do not expect any Blocks inside Paragraphs");
			TextRun result;
			if (textElement is Figure || textElement is Floater)
			{
				result = new FloatingRun(TextContainerHelper.GetElementLength(this._paraClient.Paragraph.StructuralCache.TextContainer, textElement), textElement is Figure);
				if (textElement is Figure)
				{
					this._hasFigures = true;
				}
				else
				{
					this._hasFloaters = true;
				}
			}
			else if (textElement is LineBreak)
			{
				result = new LineBreakRun(TextContainerHelper.GetElementLength(this._paraClient.Paragraph.StructuralCache.TextContainer, textElement), PTS.FSFLRES.fsflrSoftBreak);
			}
			else if (textElement.IsEmpty)
			{
				TextProperties textRunProperties = new TextProperties(textElement, position, false, true, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				char[] array = new char[LineBase._elementEdgeCharacterLength * 2];
				Invariant.Assert(LineBase._elementEdgeCharacterLength == 1, "Expected value of _elementEdgeCharacterLength is 1");
				array[0] = '​';
				array[1] = '​';
				result = new TextCharacters(array, 0, array.Length, textRunProperties);
			}
			else
			{
				Inline inline = (Inline)textElement;
				DependencyObject parent = inline.Parent;
				FlowDirection flowDirection = inline.FlowDirection;
				FlowDirection flowDirection2 = flowDirection;
				TextDecorationCollection textDecorations = DynamicPropertyReader.GetTextDecorations(inline);
				if (parent != null)
				{
					flowDirection2 = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
				}
				if (flowDirection != flowDirection2)
				{
					if (textDecorations == null || textDecorations.Count == 0)
					{
						result = new TextSpanModifier(LineBase._elementEdgeCharacterLength, null, null, flowDirection);
					}
					else
					{
						result = new TextSpanModifier(LineBase._elementEdgeCharacterLength, textDecorations, inline.Foreground, flowDirection);
					}
				}
				else if (textDecorations == null || textDecorations.Count == 0)
				{
					result = new TextHidden(LineBase._elementEdgeCharacterLength);
				}
				else
				{
					result = new TextSpanModifier(LineBase._elementEdgeCharacterLength, textDecorations, inline.Foreground);
				}
			}
			return result;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x001134BC File Offset: 0x001124BC
		protected TextRun HandleElementEndEdge(StaticTextPointer position)
		{
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd, "TextPointer does not point to element end edge.");
			TextRun result;
			if (position.Parent == this._paraClient.Paragraph.Element)
			{
				result = new ParagraphBreakRun(LineBase._syntheticCharacterLength, PTS.FSFLRES.fsflrEndOfParagraph);
			}
			else
			{
				Inline inline = (Inline)((TextElement)position.GetAdjacentElement(LogicalDirection.Forward));
				DependencyObject parent = inline.Parent;
				FlowDirection flowDirection = inline.FlowDirection;
				if (parent != null)
				{
					flowDirection = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
				}
				if (inline.FlowDirection != flowDirection)
				{
					result = new TextEndOfSegment(LineBase._elementEdgeCharacterLength);
				}
				else
				{
					TextDecorationCollection textDecorations = DynamicPropertyReader.GetTextDecorations(inline);
					if (textDecorations == null || textDecorations.Count == 0)
					{
						result = new TextHidden(LineBase._elementEdgeCharacterLength);
					}
					else
					{
						result = new TextEndOfSegment(LineBase._elementEdgeCharacterLength);
					}
				}
			}
			return result;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00113580 File Offset: 0x00112580
		protected TextRun HandleEmbeddedObject(int dcp, StaticTextPointer position)
		{
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement, "TextPointer does not point to embedded object.");
			DependencyObject dependencyObject = position.GetAdjacentElement(LogicalDirection.Forward) as DependencyObject;
			TextRun result;
			if (dependencyObject is UIElement)
			{
				TextRunProperties textProps = new TextProperties(dependencyObject, position, true, true, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				result = new InlineObjectRun(TextContainerHelper.EmbeddedObjectLength, (UIElement)dependencyObject, textProps, this._paraClient.Paragraph as TextParagraph);
			}
			else
			{
				result = new TextHidden(TextContainerHelper.EmbeddedObjectLength);
			}
			return result;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x0011360E File Offset: 0x0011260E
		internal static int SyntheticCharacterLength
		{
			get
			{
				return LineBase._syntheticCharacterLength;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00113615 File Offset: 0x00112615
		internal bool HasFigures
		{
			get
			{
				return this._hasFigures;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x0011361D File Offset: 0x0011261D
		internal bool HasFloaters
		{
			get
			{
				return this._hasFloaters;
			}
		}

		// Token: 0x0400079C RID: 1948
		protected readonly BaseParaClient _paraClient;

		// Token: 0x0400079D RID: 1949
		protected bool _hasFigures;

		// Token: 0x0400079E RID: 1950
		protected bool _hasFloaters;

		// Token: 0x0400079F RID: 1951
		protected static int _syntheticCharacterLength = 1;

		// Token: 0x040007A0 RID: 1952
		protected static int _elementEdgeCharacterLength = 1;
	}
}
