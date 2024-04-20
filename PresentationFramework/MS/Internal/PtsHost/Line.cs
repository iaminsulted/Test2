using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000124 RID: 292
	internal sealed class Line : LineBase
	{
		// Token: 0x060007D5 RID: 2005 RVA: 0x00111EF8 File Offset: 0x00110EF8
		internal Line(TextFormatterHost host, TextParaClient paraClient, int cpPara) : base(paraClient)
		{
			this._host = host;
			this._cpPara = cpPara;
			this._textAlignment = (TextAlignment)this.TextParagraph.Element.GetValue(Block.TextAlignmentProperty);
			this._indent = 0.0;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00111F58 File Offset: 0x00110F58
		public override void Dispose()
		{
			try
			{
				if (this._line != null)
				{
					this._line.Dispose();
				}
			}
			finally
			{
				this._line = null;
				this._runs = null;
				this._hasFigures = false;
				this._hasFloaters = false;
				base.Dispose();
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00111FB0 File Offset: 0x00110FB0
		internal void GetDvrSuppressibleBottomSpace(out int dvrSuppressible)
		{
			dvrSuppressible = Math.Max(0, TextDpi.ToTextDpi(this._line.OverhangAfter));
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00111FCC File Offset: 0x00110FCC
		internal void GetDurFigureAnchor(FigureParagraph paraFigure, uint fswdir, out int dur)
		{
			int firstCharacterIndex = TextContainerHelper.GetCPFromElement(this._paraClient.Paragraph.StructuralCache.TextContainer, paraFigure.Element, ElementEdge.BeforeStart) - this._cpPara;
			double distanceFromCharacterHit = this._line.GetDistanceFromCharacterHit(new CharacterHit(firstCharacterIndex, 0));
			dur = TextDpi.ToTextDpi(distanceFromCharacterHit);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00112020 File Offset: 0x00111020
		internal override TextRun GetTextRun(int dcp)
		{
			TextRun textRun = null;
			StaticTextPointer position = ((ITextContainer)this._paraClient.Paragraph.StructuralCache.TextContainer).CreateStaticPointerAtOffset(this._cpPara + dcp);
			switch (position.GetPointerContext(LogicalDirection.Forward))
			{
			case TextPointerContext.None:
				textRun = new ParagraphBreakRun(LineBase._syntheticCharacterLength, PTS.FSFLRES.fsflrEndOfParagraph);
				break;
			case TextPointerContext.Text:
				textRun = base.HandleText(position);
				break;
			case TextPointerContext.EmbeddedElement:
				textRun = base.HandleEmbeddedObject(dcp, position);
				break;
			case TextPointerContext.ElementStart:
				textRun = base.HandleElementStartEdge(position);
				break;
			case TextPointerContext.ElementEnd:
				textRun = base.HandleElementEndEdge(position);
				break;
			}
			Invariant.Assert(textRun != null, "TextRun has not been created.");
			Invariant.Assert(textRun.Length > 0, "TextRun has to have positive length.");
			return textRun;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x001120D0 File Offset: 0x001110D0
		internal override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			Invariant.Assert(dcp >= 0);
			int num = 0;
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, this._cpPara, LogicalDirection.Forward);
				ITextPointer textPointerFromCP2 = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, this._cpPara + dcp, LogicalDirection.Forward);
				while (textPointerFromCP2.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text && textPointerFromCP2.CompareTo(textPointerFromCP) != 0)
				{
					textPointerFromCP2.MoveByOffset(-1);
					num++;
				}
				string textInRun = textPointerFromCP2.GetTextInRun(LogicalDirection.Backward);
				empty = new CharacterBufferRange(textInRun, 0, textInRun.Length);
				StaticTextPointer staticTextPointer = textPointerFromCP2.CreateStaticPointer();
				culture = DynamicPropertyReader.GetCultureInfo((staticTextPointer.Parent != null) ? staticTextPointer.Parent : this._paraClient.Paragraph.Element);
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(num + empty.Length, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x001121C6 File Offset: 0x001111C6
		internal override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int dcp)
		{
			return this._cpPara + dcp;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x001121D0 File Offset: 0x001111D0
		internal void Format(Line.FormattingContext ctx, int dcp, int width, int trackWidth, TextParagraphProperties lineProps, TextLineBreak textLineBreak)
		{
			this._formattingContext = ctx;
			this._dcp = dcp;
			this._host.Context = this;
			this._wrappingWidth = TextDpi.FromTextDpi(width);
			this._trackWidth = TextDpi.FromTextDpi(trackWidth);
			this._mirror = (lineProps.FlowDirection == FlowDirection.RightToLeft);
			this._indent = lineProps.Indent;
			try
			{
				if (ctx.LineFormatLengthTarget == -1)
				{
					this._line = this._host.TextFormatter.FormatLine(this._host, dcp, this._wrappingWidth, lineProps, textLineBreak, ctx.TextRunCache);
				}
				else
				{
					this._line = this._host.TextFormatter.RecreateLine(this._host, dcp, ctx.LineFormatLengthTarget, this._wrappingWidth, lineProps, textLineBreak, ctx.TextRunCache);
				}
				this._runs = this._line.GetTextRunSpans();
				Invariant.Assert(this._runs != null, "Cannot retrieve runs collection.");
				if (this._formattingContext.MeasureMode)
				{
					List<InlineObject> list = new List<InlineObject>(1);
					int num = this._dcp;
					foreach (TextSpan<TextRun> textSpan in this._runs)
					{
						TextRun value = textSpan.Value;
						if (value is InlineObjectRun)
						{
							list.Add(new InlineObject(num, ((InlineObjectRun)value).UIElementIsland, (TextParagraph)this._paraClient.Paragraph));
						}
						else if (value is FloatingRun)
						{
							if (((FloatingRun)value).Figure)
							{
								this._hasFigures = true;
							}
							else
							{
								this._hasFloaters = true;
							}
						}
						num += textSpan.Length;
					}
					if (list.Count == 0)
					{
						list = null;
					}
					this.TextParagraph.SubmitInlineObjects(dcp, dcp + this.ActualLength, list);
				}
			}
			finally
			{
				this._host.Context = null;
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x001123D0 File Offset: 0x001113D0
		internal Size MeasureChild(InlineObjectRun inlineObject)
		{
			Size result;
			if (this._formattingContext.MeasureMode)
			{
				double height = this._paraClient.Paragraph.StructuralCache.CurrentFormatContext.DocumentPageSize.Height;
				if (!this._paraClient.Paragraph.StructuralCache.CurrentFormatContext.FinitePage)
				{
					height = double.PositiveInfinity;
				}
				result = inlineObject.UIElementIsland.DoLayout(new Size(this._trackWidth, height), true, true);
			}
			else
			{
				result = inlineObject.UIElementIsland.Root.DesiredSize;
			}
			return result;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00112464 File Offset: 0x00111464
		internal ContainerVisual CreateVisual()
		{
			LineVisual lineVisual = new LineVisual();
			this._host.Context = this;
			try
			{
				IList<TextSpan<TextRun>> list = this._runs;
				TextLine textLine = this._line;
				if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
				{
					textLine = this._line.Collapse(new TextCollapsingProperties[]
					{
						this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
					});
					Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
					list = textLine.GetTextRunSpans();
				}
				if (this.HasInlineObjects())
				{
					VisualCollection children = lineVisual.Children;
					FlowDirection parentFD = (FlowDirection)this._paraClient.Paragraph.Element.GetValue(FrameworkElement.FlowDirectionProperty);
					int num = this._dcp;
					foreach (TextSpan<TextRun> textSpan in list)
					{
						TextRun value = textSpan.Value;
						if (value is InlineObjectRun)
						{
							InlineObjectRun inlineObjectRun = (InlineObjectRun)value;
							FlowDirection flowDirection;
							Rect boundsFromPosition = this.GetBoundsFromPosition(num, value.Length, out flowDirection);
							Visual visual = VisualTreeHelper.GetParent(inlineObjectRun.UIElementIsland) as Visual;
							if (visual != null)
							{
								ContainerVisual containerVisual = visual as ContainerVisual;
								Invariant.Assert(containerVisual != null, "Parent should always derives from ContainerVisual.");
								containerVisual.Children.Remove(inlineObjectRun.UIElementIsland);
							}
							if (!textLine.HasCollapsed || boundsFromPosition.Left + inlineObjectRun.UIElementIsland.Root.DesiredSize.Width < textLine.Width)
							{
								if (inlineObjectRun.UIElementIsland.Root is FrameworkElement)
								{
									FlowDirection childFD = (FlowDirection)((FrameworkElement)inlineObjectRun.UIElementIsland.Root).Parent.GetValue(FrameworkElement.FlowDirectionProperty);
									PtsHelper.UpdateMirroringTransform(parentFD, childFD, inlineObjectRun.UIElementIsland, boundsFromPosition.Width);
								}
								children.Add(inlineObjectRun.UIElementIsland);
								inlineObjectRun.UIElementIsland.Offset = new Vector(boundsFromPosition.Left, boundsFromPosition.Top);
							}
						}
						num += textSpan.Length;
					}
				}
				double x = TextDpi.FromTextDpi(this.CalculateUOffsetShift());
				DrawingContext drawingContext = lineVisual.Open();
				textLine.Draw(drawingContext, new Point(x, 0.0), this._mirror ? InvertAxes.Horizontal : InvertAxes.None);
				drawingContext.Close();
				lineVisual.WidthIncludingTrailingWhitespace = textLine.WidthIncludingTrailingWhitespace - this._indent;
			}
			finally
			{
				this._host.Context = null;
			}
			return lineVisual;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00112720 File Offset: 0x00111720
		internal Rect GetBoundsFromTextPosition(int textPosition, out FlowDirection flowDirection)
		{
			return this.GetBoundsFromPosition(textPosition, 1, out flowDirection);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0011272C File Offset: 0x0011172C
		internal List<Rect> GetRangeBounds(int cp, int cch, double xOffset, double yOffset)
		{
			List<Rect> list = new List<Rect>();
			double num = TextDpi.FromTextDpi(this.CalculateUOffsetShift());
			double num2 = xOffset + num;
			IList<TextBounds> textBounds;
			if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
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

		// Token: 0x060007E1 RID: 2017 RVA: 0x0011282F File Offset: 0x0011182F
		internal TextLineBreak GetTextLineBreak()
		{
			if (this._line == null)
			{
				return null;
			}
			return this._line.GetTextLineBreak();
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00112848 File Offset: 0x00111848
		internal CharacterHit GetTextPositionFromDistance(int urDistance)
		{
			int num = this.CalculateUOffsetShift();
			if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
			{
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
				});
				Invariant.Assert(num == 0);
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				return textLine.GetCharacterHitFromDistance(TextDpi.FromTextDpi(urDistance));
			}
			return this._line.GetCharacterHitFromDistance(TextDpi.FromTextDpi(urDistance - num));
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x001128E0 File Offset: 0x001118E0
		internal IInputElement InputHitTest(int urOffset)
		{
			DependencyObject dependencyObject = null;
			int num = this.CalculateUOffsetShift();
			CharacterHit characterHitFromDistance;
			if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(num == 0);
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
				});
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				characterHitFromDistance = textLine.GetCharacterHitFromDistance(TextDpi.FromTextDpi(urOffset));
			}
			else
			{
				characterHitFromDistance = this._line.GetCharacterHitFromDistance(TextDpi.FromTextDpi(urOffset - num));
			}
			int cp = this._paraClient.Paragraph.ParagraphStartCharacterPosition + characterHitFromDistance.FirstCharacterIndex + characterHitFromDistance.TrailingLength;
			TextPointer textPointer = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, cp, LogicalDirection.Forward) as TextPointer;
			if (textPointer != null)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext((characterHitFromDistance.TrailingLength == 0) ? LogicalDirection.Forward : LogicalDirection.Backward);
				if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.ElementEnd)
				{
					dependencyObject = textPointer.Parent;
				}
				else if (pointerContext == TextPointerContext.ElementStart)
				{
					dependencyObject = textPointer.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
				}
			}
			return dependencyObject as IInputElement;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00112A00 File Offset: 0x00111A00
		internal int GetEllipsesLength()
		{
			if (!this._line.HasOverflowed)
			{
				return 0;
			}
			if (this.TextParagraph.Properties.TextTrimming == TextTrimming.None)
			{
				return 0;
			}
			TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
			{
				this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
			});
			Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
			IList<TextCollapsedRange> textCollapsedRanges = textLine.GetTextCollapsedRanges();
			if (textCollapsedRanges != null)
			{
				Invariant.Assert(textCollapsedRanges.Count == 1, "Multiple collapsed ranges are not supported.");
				return textCollapsedRanges[0].Length;
			}
			return 0;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00112A98 File Offset: 0x00111A98
		internal void GetGlyphRuns(List<GlyphRun> glyphRuns, int dcpStart, int dcpEnd)
		{
			int num = dcpStart - this._dcp;
			int num2 = dcpEnd - dcpStart;
			IList<TextSpan<TextRun>> textRunSpans = this._line.GetTextRunSpans();
			DrawingGroup drawingGroup = new DrawingGroup();
			DrawingContext drawingContext = drawingGroup.Open();
			double x = TextDpi.FromTextDpi(this.CalculateUOffsetShift());
			this._line.Draw(drawingContext, new Point(x, 0.0), InvertAxes.None);
			drawingContext.Close();
			int num3 = 0;
			ArrayList arrayList = new ArrayList(4);
			this.AddGlyphRunRecursive(drawingGroup, arrayList, ref num3);
			int num4 = 0;
			using (IEnumerator<TextSpan<TextRun>> enumerator = textRunSpans.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TextSpan<TextRun> textSpan = enumerator.Current;
					if (textSpan.Value is TextCharacters)
					{
						num4 += textSpan.Length;
					}
				}
				goto IL_EA;
			}
			IL_B6:
			GlyphRun glyphRun = (GlyphRun)arrayList[0];
			num3 -= ((glyphRun.Characters == null) ? 0 : glyphRun.Characters.Count);
			arrayList.RemoveAt(0);
			IL_EA:
			if (num3 <= num4)
			{
				int num5 = 0;
				int num6 = 0;
				foreach (TextSpan<TextRun> textSpan2 in textRunSpans)
				{
					if (textSpan2.Value is TextCharacters)
					{
						int i = 0;
						while (i < textSpan2.Length)
						{
							Invariant.Assert(num6 < arrayList.Count);
							GlyphRun glyphRun2 = (GlyphRun)arrayList[num6];
							int num7 = (glyphRun2.Characters == null) ? 0 : glyphRun2.Characters.Count;
							if (num < num5 + num7 && num + num2 > num5)
							{
								glyphRuns.Add(glyphRun2);
							}
							i += num7;
							num6++;
						}
						Invariant.Assert(i == textSpan2.Length);
						if (num + num2 <= num5 + textSpan2.Length)
						{
							break;
						}
					}
					num5 += textSpan2.Length;
				}
				return;
			}
			goto IL_B6;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00112C94 File Offset: 0x00111C94
		internal CharacterHit GetNextCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetNextCaretCharacterHit(index);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00112CA2 File Offset: 0x00111CA2
		internal CharacterHit GetPreviousCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetPreviousCaretCharacterHit(index);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00112CB0 File Offset: 0x00111CB0
		internal CharacterHit GetBackspaceCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetBackspaceCaretCharacterHit(index);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00112CBE File Offset: 0x00111CBE
		internal bool IsAtCaretCharacterHit(CharacterHit charHit)
		{
			return this._line.IsAtCaretCharacterHit(charHit, this._dcp);
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x00112CD2 File Offset: 0x00111CD2
		internal int Start
		{
			get
			{
				return TextDpi.ToTextDpi(this._line.Start) + TextDpi.ToTextDpi(this._indent) + this.CalculateUOffsetShift();
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x00112CF8 File Offset: 0x00111CF8
		internal int Width
		{
			get
			{
				int num;
				if (this.IsWidthAdjusted)
				{
					num = TextDpi.ToTextDpi(this._line.WidthIncludingTrailingWhitespace) - TextDpi.ToTextDpi(this._indent);
				}
				else
				{
					num = TextDpi.ToTextDpi(this._line.Width) - TextDpi.ToTextDpi(this._indent);
				}
				Invariant.Assert(num >= 0, "Line width cannot be negative");
				return num;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x00112D5B File Offset: 0x00111D5B
		internal int Height
		{
			get
			{
				return TextDpi.ToTextDpi(this._line.Height);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00112D6D File Offset: 0x00111D6D
		internal int Baseline
		{
			get
			{
				return TextDpi.ToTextDpi(this._line.Baseline);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x00112D7F File Offset: 0x00111D7F
		internal bool EndOfParagraph
		{
			get
			{
				return this._line.NewlineLength != 0 && this._runs[this._runs.Count - 1].Value is ParagraphBreakRun;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x00112DB5 File Offset: 0x00111DB5
		internal int SafeLength
		{
			get
			{
				return this._line.Length;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x00112DC2 File Offset: 0x00111DC2
		internal int ActualLength
		{
			get
			{
				return this._line.Length - (this.EndOfParagraph ? LineBase._syntheticCharacterLength : 0);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00112DE0 File Offset: 0x00111DE0
		internal int ContentLength
		{
			get
			{
				return this._line.Length - this._line.NewlineLength;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00112DF9 File Offset: 0x00111DF9
		internal int DependantLength
		{
			get
			{
				return this._line.DependentLength;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00112E06 File Offset: 0x00111E06
		internal bool IsTruncated
		{
			get
			{
				return this._line.IsTruncated;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x00112E14 File Offset: 0x00111E14
		internal PTS.FSFLRES FormattingResult
		{
			get
			{
				PTS.FSFLRES result = PTS.FSFLRES.fsflrOutOfSpace;
				if (this._line.NewlineLength == 0)
				{
					return result;
				}
				TextRun value = this._runs[this._runs.Count - 1].Value;
				if (value is ParagraphBreakRun)
				{
					result = ((ParagraphBreakRun)value).BreakReason;
				}
				else if (value is LineBreakRun)
				{
					result = ((LineBreakRun)value).BreakReason;
				}
				return result;
			}
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00112E7C File Offset: 0x00111E7C
		private bool HasInlineObjects()
		{
			bool result = false;
			using (IEnumerator<TextSpan<TextRun>> enumerator = this._runs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Value is InlineObjectRun)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00112ED4 File Offset: 0x00111ED4
		private Rect GetBoundsFromPosition(int cp, int cch, out FlowDirection flowDirection)
		{
			double num = TextDpi.FromTextDpi(this.CalculateUOffsetShift());
			IList<TextBounds> textBounds;
			if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
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
			flowDirection = textBounds[0].FlowDirection;
			rectangle.X += num;
			return rectangle;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00112FD8 File Offset: 0x00111FD8
		private TextCollapsingProperties GetCollapsingProps(double wrappingWidth, LineProperties paraProperties)
		{
			Invariant.Assert(paraProperties.TextTrimming > TextTrimming.None, "Text trimming must be enabled.");
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

		// Token: 0x060007F8 RID: 2040 RVA: 0x00113020 File Offset: 0x00112020
		private void AddGlyphRunRecursive(Drawing drawing, IList glyphRunsCollection, ref int cchGlyphRuns)
		{
			DrawingGroup drawingGroup = drawing as DrawingGroup;
			if (drawingGroup != null)
			{
				using (DrawingCollection.Enumerator enumerator = drawingGroup.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Drawing drawing2 = enumerator.Current;
						this.AddGlyphRunRecursive(drawing2, glyphRunsCollection, ref cchGlyphRuns);
					}
					return;
				}
			}
			GlyphRunDrawing glyphRunDrawing = drawing as GlyphRunDrawing;
			if (glyphRunDrawing != null)
			{
				GlyphRun glyphRun = glyphRunDrawing.GlyphRun;
				if (glyphRun != null)
				{
					cchGlyphRuns += ((glyphRun.Characters == null) ? 0 : glyphRun.Characters.Count);
					glyphRunsCollection.Add(glyphRun);
				}
			}
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x001130BC File Offset: 0x001120BC
		internal int CalculateUOffsetShift()
		{
			int num;
			int num2;
			if (this.IsUOffsetAdjusted)
			{
				num = TextDpi.ToTextDpi(this._line.WidthIncludingTrailingWhitespace);
				num2 = TextDpi.ToTextDpi(this._line.Width) - num;
				Invariant.Assert(num2 <= 0);
			}
			else
			{
				num = TextDpi.ToTextDpi(this._line.Width);
				num2 = 0;
			}
			int num3 = 0;
			if ((this._textAlignment == TextAlignment.Center || this._textAlignment == TextAlignment.Right) && !this.ShowEllipses)
			{
				if (num > TextDpi.ToTextDpi(this._wrappingWidth))
				{
					num3 = num - TextDpi.ToTextDpi(this._wrappingWidth);
				}
				else
				{
					num3 = 0;
				}
			}
			int result;
			if (this._textAlignment == TextAlignment.Center)
			{
				result = (num3 + num2) / 2;
			}
			else
			{
				result = num3 + num2;
			}
			return result;
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0011316A File Offset: 0x0011216A
		private bool HasLineBreak
		{
			get
			{
				return this._line.NewlineLength > 0;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0011317A File Offset: 0x0011217A
		private bool IsUOffsetAdjusted
		{
			get
			{
				return (this._textAlignment == TextAlignment.Right || this._textAlignment == TextAlignment.Center) && this.IsWidthAdjusted;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00113198 File Offset: 0x00112198
		private bool IsWidthAdjusted
		{
			get
			{
				bool result = false;
				if ((this.HasLineBreak || this.EndOfParagraph) && !this.ShowEllipses)
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x001131C2 File Offset: 0x001121C2
		private bool ShowEllipses
		{
			get
			{
				return this.TextParagraph.Properties.TextTrimming != TextTrimming.None && this._line.HasOverflowed;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x001131E8 File Offset: 0x001121E8
		private TextParagraph TextParagraph
		{
			get
			{
				return this._paraClient.Paragraph as TextParagraph;
			}
		}

		// Token: 0x04000791 RID: 1937
		private readonly TextFormatterHost _host;

		// Token: 0x04000792 RID: 1938
		private readonly int _cpPara;

		// Token: 0x04000793 RID: 1939
		private Line.FormattingContext _formattingContext;

		// Token: 0x04000794 RID: 1940
		private TextLine _line;

		// Token: 0x04000795 RID: 1941
		private IList<TextSpan<TextRun>> _runs;

		// Token: 0x04000796 RID: 1942
		private int _dcp;

		// Token: 0x04000797 RID: 1943
		private double _wrappingWidth;

		// Token: 0x04000798 RID: 1944
		private double _trackWidth = double.NaN;

		// Token: 0x04000799 RID: 1945
		private bool _mirror;

		// Token: 0x0400079A RID: 1946
		private double _indent;

		// Token: 0x0400079B RID: 1947
		private TextAlignment _textAlignment;

		// Token: 0x020008C1 RID: 2241
		internal class FormattingContext
		{
			// Token: 0x06008157 RID: 33111 RVA: 0x00323123 File Offset: 0x00322123
			internal FormattingContext(bool measureMode, bool clearOnLeft, bool clearOnRight, TextRunCache textRunCache)
			{
				this.MeasureMode = measureMode;
				this.ClearOnLeft = clearOnLeft;
				this.ClearOnRight = clearOnRight;
				this.TextRunCache = textRunCache;
				this.LineFormatLengthTarget = -1;
			}

			// Token: 0x04003C32 RID: 15410
			internal TextRunCache TextRunCache;

			// Token: 0x04003C33 RID: 15411
			internal bool MeasureMode;

			// Token: 0x04003C34 RID: 15412
			internal bool ClearOnLeft;

			// Token: 0x04003C35 RID: 15413
			internal bool ClearOnRight;

			// Token: 0x04003C36 RID: 15414
			internal int LineFormatLengthTarget;
		}
	}
}
