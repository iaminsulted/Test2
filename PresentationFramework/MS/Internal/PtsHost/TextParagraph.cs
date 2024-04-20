using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200014C RID: 332
	internal sealed class TextParagraph : BaseParagraph
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x00129D0D File Offset: 0x00128D0D
		internal TextParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00129D24 File Offset: 0x00128D24
		public override void Dispose()
		{
			if (this._attachedObjects != null)
			{
				foreach (AttachedObject attachedObject in this._attachedObjects)
				{
					attachedObject.Dispose();
				}
				this._attachedObjects = null;
			}
			if (this._inlineObjects != null)
			{
				foreach (InlineObject inlineObject in this._inlineObjects)
				{
					inlineObject.Dispose();
				}
				this._inlineObjects = null;
			}
			base.Dispose();
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00129DD8 File Offset: 0x00128DD8
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			fspap.fKeepWithNext = 0;
			fspap.fBreakPageBefore = 0;
			fspap.fBreakColumnBefore = 0;
			base.GetParaProperties(ref fspap, true);
			fspap.idobj = -1;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00129E00 File Offset: 0x00128E00
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			TextParaClient textParaClient = new TextParaClient(this);
			paraClientHandle = textParaClient.Handle;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00129E1C File Offset: 0x00128E1C
		internal void GetTextProperties(int iArea, ref PTS.FSTXTPROPS fstxtprops)
		{
			fstxtprops.fswdir = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			fstxtprops.dcpStartContent = 0;
			fstxtprops.fKeepTogether = PTS.FromBoolean(DynamicPropertyReader.GetKeepTogether(base.Element));
			fstxtprops.cMinLinesAfterBreak = DynamicPropertyReader.GetMinOrphanLines(base.Element);
			fstxtprops.cMinLinesBeforeBreak = DynamicPropertyReader.GetMinWidowLines(base.Element);
			fstxtprops.fDropCap = 0;
			fstxtprops.fVerticalGrid = 0;
			fstxtprops.fOptimizeParagraph = PTS.FromBoolean(this.IsOptimalParagraph);
			fstxtprops.fAvoidHyphenationAtTrackBottom = 0;
			fstxtprops.fAvoidHyphenationOnLastChainElement = 0;
			fstxtprops.cMaxConsecutiveHyphens = int.MaxValue;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00129EC0 File Offset: 0x00128EC0
		internal void CreateOptimalBreakSession(TextParaClient textParaClient, int dcpStart, int durTrack, LineBreakRecord lineBreakRecord, out OptimalBreakSession optimalBreakSession, out bool isParagraphJustified)
		{
			this._textRunCache = new TextRunCache();
			TextFormatter textFormatter = base.StructuralCache.TextFormatterHost.TextFormatter;
			TextLineBreak previousLineBreak = (lineBreakRecord != null) ? lineBreakRecord.TextLineBreak : null;
			OptimalTextSource optimalTextSource = new OptimalTextSource(base.StructuralCache.TextFormatterHost, base.ParagraphStartCharacterPosition, durTrack, textParaClient, this._textRunCache);
			base.StructuralCache.TextFormatterHost.Context = optimalTextSource;
			TextParagraphCache textParagraphCache = textFormatter.CreateParagraphCache(base.StructuralCache.TextFormatterHost, dcpStart, TextDpi.FromTextDpi(durTrack), this.GetLineProperties(true, dcpStart), previousLineBreak, this._textRunCache);
			base.StructuralCache.TextFormatterHost.Context = null;
			optimalBreakSession = new OptimalBreakSession(this, textParaClient, textParagraphCache, optimalTextSource);
			isParagraphJustified = ((TextAlignment)base.Element.GetValue(Block.TextAlignmentProperty) == TextAlignment.Justify);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00129F86 File Offset: 0x00128F86
		internal void GetNumberFootnotes(int fsdcpStart, int fsdcpLim, out int nFootnote)
		{
			nFootnote = 0;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00129F8B File Offset: 0x00128F8B
		internal void FormatBottomText(int iArea, uint fswdir, Line lastLine, int dvrLine, out IntPtr mcsClient)
		{
			Invariant.Assert(iArea == 0);
			mcsClient = IntPtr.Zero;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00129FA0 File Offset: 0x00128FA0
		internal bool InterruptFormatting(int dcpCur, int vrCur)
		{
			BackgroundFormatInfo backgroundFormatInfo = base.StructuralCache.BackgroundFormatInfo;
			if (!BackgroundFormatInfo.IsBackgroundFormatEnabled)
			{
				return false;
			}
			if (base.StructuralCache.CurrentFormatContext.FinitePage)
			{
				return false;
			}
			if (vrCur < TextDpi.ToTextDpi(double.IsPositiveInfinity(backgroundFormatInfo.ViewportHeight) ? 500.0 : backgroundFormatInfo.ViewportHeight))
			{
				return false;
			}
			if (backgroundFormatInfo.BackgroundFormatStopTime > DateTime.UtcNow)
			{
				return false;
			}
			if (!backgroundFormatInfo.DoesFinalDTRCoverRestOfText)
			{
				return false;
			}
			if (dcpCur + base.ParagraphStartCharacterPosition <= backgroundFormatInfo.LastCPUninterruptible)
			{
				return false;
			}
			base.StructuralCache.BackgroundFormatInfo.CPInterrupted = dcpCur + base.ParagraphStartCharacterPosition;
			return true;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0012A047 File Offset: 0x00129047
		internal IList<TextBreakpoint> FormatLineVariants(TextParaClient textParaClient, TextParagraphCache textParagraphCache, OptimalTextSource optimalTextSource, int dcp, TextLineBreak textLineBreak, uint fswdir, int urStartLine, int durLine, bool allowHyphenation, bool clearOnLeft, bool clearOnRight, bool treatAsFirstInPara, bool treatAsLastInPara, bool suppressTopSpace, IntPtr lineVariantRestriction, out int iLineBestVariant)
		{
			base.StructuralCache.TextFormatterHost.Context = optimalTextSource;
			IList<TextBreakpoint> result = textParagraphCache.FormatBreakpoints(dcp, textLineBreak, lineVariantRestriction, TextDpi.FromTextDpi(durLine), out iLineBestVariant);
			base.StructuralCache.TextFormatterHost.Context = null;
			return result;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0012A080 File Offset: 0x00129080
		internal void ReconstructLineVariant(TextParaClient paraClient, int iArea, int dcp, IntPtr pbrlineIn, int dcpLineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, bool fAllowHyphenation, bool fClearOnLeft, bool fClearOnRight, bool fTreatAsFirstInPara, bool fTreatAsLastInPara, bool fSuppressTopSpace, out IntPtr lineHandle, out int dcpLine, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine)
		{
			Invariant.Assert(iArea == 0);
			base.StructuralCache.CurrentFormatContext.OnFormatLine();
			Line line = new Line(base.StructuralCache.TextFormatterHost, paraClient, base.ParagraphStartCharacterPosition);
			this.FormatLineCore(line, pbrlineIn, new Line.FormattingContext(true, fClearOnLeft, fClearOnRight, this._textRunCache)
			{
				LineFormatLengthTarget = dcpLineIn
			}, dcp, durLine, durTrack, fTreatAsFirstInPara, dcp);
			lineHandle = line.Handle;
			dcpLine = line.SafeLength;
			TextLineBreak textLineBreak = line.GetTextLineBreak();
			if (textLineBreak != null)
			{
				LineBreakRecord lineBreakRecord = new LineBreakRecord(base.PtsContext, textLineBreak);
				ppbrlineOut = lineBreakRecord.Handle;
			}
			else
			{
				ppbrlineOut = IntPtr.Zero;
			}
			fForcedBroken = PTS.FromBoolean(line.IsTruncated);
			fsflres = line.FormattingResult;
			dvrAscent = line.Baseline;
			dvrDescent = line.Height - line.Baseline;
			urBBox = urStartLine + line.Start;
			durBBox = line.Width;
			dcpDepend = line.DependantLength;
			fReformatNeighborsAsLastLine = 0;
			this.CalcLineAscentDescent(dcp, ref dvrAscent, ref dvrDescent);
			int num = base.ParagraphStartCharacterPosition + dcp + line.ActualLength + dcpDepend;
			int symbolCount = base.StructuralCache.TextContainer.SymbolCount;
			if (num > symbolCount)
			{
				num = symbolCount;
			}
			base.StructuralCache.CurrentFormatContext.DependentMax = base.StructuralCache.TextContainer.CreatePointerAtOffset(num, LogicalDirection.Backward);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0012A1D4 File Offset: 0x001291D4
		internal void FormatLine(TextParaClient paraClient, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, bool fAllowHyphenation, bool fClearOnLeft, bool fClearOnRight, bool fTreatAsFirstInPara, bool fTreatAsLastInPara, bool fSuppressTopSpace, out IntPtr lineHandle, out int dcpLine, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine)
		{
			Invariant.Assert(iArea == 0);
			base.StructuralCache.CurrentFormatContext.OnFormatLine();
			Line line = new Line(base.StructuralCache.TextFormatterHost, paraClient, base.ParagraphStartCharacterPosition);
			Line.FormattingContext ctx = new Line.FormattingContext(true, fClearOnLeft, fClearOnRight, this._textRunCache);
			this.FormatLineCore(line, pbrlineIn, ctx, dcp, durLine, durTrack, fTreatAsFirstInPara, dcp);
			lineHandle = line.Handle;
			dcpLine = line.SafeLength;
			TextLineBreak textLineBreak = line.GetTextLineBreak();
			if (textLineBreak != null)
			{
				LineBreakRecord lineBreakRecord = new LineBreakRecord(base.PtsContext, textLineBreak);
				ppbrlineOut = lineBreakRecord.Handle;
			}
			else
			{
				ppbrlineOut = IntPtr.Zero;
			}
			fForcedBroken = PTS.FromBoolean(line.IsTruncated);
			fsflres = line.FormattingResult;
			dvrAscent = line.Baseline;
			dvrDescent = line.Height - line.Baseline;
			urBBox = urStartLine + line.Start;
			durBBox = line.Width;
			dcpDepend = line.DependantLength;
			fReformatNeighborsAsLastLine = 0;
			this.CalcLineAscentDescent(dcp, ref dvrAscent, ref dvrDescent);
			int num = base.ParagraphStartCharacterPosition + dcp + line.ActualLength + dcpDepend;
			int symbolCount = base.StructuralCache.TextContainer.SymbolCount;
			if (num > symbolCount)
			{
				num = symbolCount;
			}
			base.StructuralCache.CurrentFormatContext.DependentMax = base.StructuralCache.TextContainer.CreatePointerAtOffset(num, LogicalDirection.Backward);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0012A320 File Offset: 0x00129320
		internal void UpdGetChangeInText(out int dcpStart, out int ddcpOld, out int ddcpNew)
		{
			DtrList dtrList = base.StructuralCache.DtrsFromRange(base.ParagraphStartCharacterPosition, base.LastFormatCch);
			if (dtrList != null)
			{
				dcpStart = dtrList[0].StartIndex - base.ParagraphStartCharacterPosition;
				ddcpNew = dtrList[0].PositionsAdded;
				ddcpOld = dtrList[0].PositionsRemoved;
				if (dtrList.Length > 1)
				{
					for (int i = 1; i < dtrList.Length; i++)
					{
						int num = dtrList[i].StartIndex - dtrList[i - 1].StartIndex;
						ddcpNew += num + dtrList[i].PositionsAdded;
						ddcpOld += num + dtrList[i].PositionsRemoved;
					}
				}
				if (!base.StructuralCache.CurrentFormatContext.FinitePage)
				{
					this.UpdateEmbeddedObjectsCache<AttachedObject>(ref this._attachedObjects, dcpStart, ddcpOld, ddcpNew - ddcpOld);
					this.UpdateEmbeddedObjectsCache<InlineObject>(ref this._inlineObjects, dcpStart, ddcpOld, ddcpNew - ddcpOld);
				}
				Invariant.Assert(dcpStart >= 0 && base.Cch >= dcpStart && base.LastFormatCch >= dcpStart);
				ddcpOld = Math.Min(ddcpOld, base.LastFormatCch - dcpStart + 1);
				ddcpNew = Math.Min(ddcpNew, base.Cch - dcpStart + 1);
				return;
			}
			dcpStart = (ddcpOld = (ddcpNew = 0));
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0012A487 File Offset: 0x00129487
		internal void GetDvrAdvance(int dcp, uint fswdir, out int dvr)
		{
			this.EnsureLineProperties();
			dvr = TextDpi.ToTextDpi(this._lineProperties.CalcLineAdvanceForTextParagraph(this, dcp, this._lineProperties.DefaultTextRunProperties.FontRenderingEmSize));
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0012A4B4 File Offset: 0x001294B4
		internal int GetLastDcpAttachedObjectBeforeLine(int dcpFirst)
		{
			ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(base.StructuralCache.TextContainer, base.ParagraphStartCharacterPosition + dcpFirst, LogicalDirection.Forward);
			ITextPointer contentStart = TextContainerHelper.GetContentStart(base.StructuralCache.TextContainer, base.Element);
			while (textPointerFromCP.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
			{
				TextElement adjacentElementFromOuterPosition = ((TextPointer)textPointerFromCP).GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
				if (!(adjacentElementFromOuterPosition is Figure) && !(adjacentElementFromOuterPosition is Floater))
				{
					break;
				}
				textPointerFromCP.MoveByOffset(adjacentElementFromOuterPosition.SymbolCount);
			}
			return contentStart.GetOffsetToPosition(textPointerFromCP);
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0012A530 File Offset: 0x00129530
		private List<TextElement> GetAttachedObjectElements(int dcpFirst, int dcpLast)
		{
			List<TextElement> list = new List<TextElement>();
			ITextPointer contentStart = TextContainerHelper.GetContentStart(base.StructuralCache.TextContainer, base.Element);
			ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(base.StructuralCache.TextContainer, base.ParagraphStartCharacterPosition + dcpFirst, LogicalDirection.Forward);
			if (dcpLast > base.Cch)
			{
				dcpLast = base.Cch;
			}
			while (contentStart.GetOffsetToPosition(textPointerFromCP) < dcpLast)
			{
				if (textPointerFromCP.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					TextElement adjacentElementFromOuterPosition = ((TextPointer)textPointerFromCP).GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					if (adjacentElementFromOuterPosition is Figure || adjacentElementFromOuterPosition is Floater)
					{
						list.Add(adjacentElementFromOuterPosition);
						textPointerFromCP.MoveByOffset(adjacentElementFromOuterPosition.SymbolCount);
					}
					else
					{
						textPointerFromCP.MoveToNextContextPosition(LogicalDirection.Forward);
					}
				}
				else
				{
					textPointerFromCP.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
			return list;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0012A5E1 File Offset: 0x001295E1
		internal int GetAttachedObjectCount(int dcpFirst, int dcpLast)
		{
			List<TextElement> attachedObjectElements = this.GetAttachedObjectElements(dcpFirst, dcpLast);
			if (attachedObjectElements.Count == 0)
			{
				this.SubmitAttachedObjects(dcpFirst, dcpLast, null);
			}
			return attachedObjectElements.Count;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0012A604 File Offset: 0x00129604
		internal List<AttachedObject> GetAttachedObjects(int dcpFirst, int dcpLast)
		{
			ITextPointer contentStart = TextContainerHelper.GetContentStart(base.StructuralCache.TextContainer, base.Element);
			List<AttachedObject> list = new List<AttachedObject>();
			List<TextElement> attachedObjectElements = this.GetAttachedObjectElements(dcpFirst, dcpLast);
			for (int i = 0; i < attachedObjectElements.Count; i++)
			{
				TextElement textElement = attachedObjectElements[i];
				if (textElement is Figure && base.StructuralCache.CurrentFormatContext.FinitePage)
				{
					FigureParagraph figureParagraph = new FigureParagraph(textElement, base.StructuralCache);
					if (base.StructuralCache.CurrentFormatContext.IncrementalUpdate)
					{
						figureParagraph.SetUpdateInfo(PTS.FSKCHANGE.fskchNew, false);
					}
					FigureObject item = new FigureObject(contentStart.GetOffsetToPosition(textElement.ElementStart), figureParagraph);
					list.Add(item);
				}
				else
				{
					FloaterParagraph floaterParagraph = new FloaterParagraph(textElement, base.StructuralCache);
					if (base.StructuralCache.CurrentFormatContext.IncrementalUpdate)
					{
						floaterParagraph.SetUpdateInfo(PTS.FSKCHANGE.fskchNew, false);
					}
					FloaterObject item2 = new FloaterObject(contentStart.GetOffsetToPosition(textElement.ElementStart), floaterParagraph);
					list.Add(item2);
				}
			}
			if (list.Count != 0)
			{
				this.SubmitAttachedObjects(dcpFirst, dcpLast, list);
			}
			return list;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0012A716 File Offset: 0x00129716
		internal void SubmitInlineObjects(int dcpStart, int dcpLim, List<InlineObject> inlineObjects)
		{
			this.SubmitEmbeddedObjects<InlineObject>(ref this._inlineObjects, dcpStart, dcpLim, inlineObjects);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0012A727 File Offset: 0x00129727
		internal void SubmitAttachedObjects(int dcpStart, int dcpLim, List<AttachedObject> attachedObjects)
		{
			this.SubmitEmbeddedObjects<AttachedObject>(ref this._attachedObjects, dcpStart, dcpLim, attachedObjects);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0012A738 File Offset: 0x00129738
		internal List<InlineObject> InlineObjectsFromRange(int dcpStart, int dcpLast)
		{
			List<InlineObject> list = null;
			if (this._inlineObjects != null)
			{
				list = new List<InlineObject>(this._inlineObjects.Count);
				for (int i = 0; i < this._inlineObjects.Count; i++)
				{
					InlineObject inlineObject = this._inlineObjects[i];
					if (inlineObject.Dcp >= dcpStart && inlineObject.Dcp < dcpLast)
					{
						list.Add(inlineObject);
					}
					else if (inlineObject.Dcp >= dcpLast)
					{
						break;
					}
				}
			}
			if (list == null || list.Count == 0)
			{
				return null;
			}
			return list;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0012A7B8 File Offset: 0x001297B8
		internal void CalcLineAscentDescent(int dcp, ref int dvrAscent, ref int dvrDescent)
		{
			this.EnsureLineProperties();
			int num = dvrAscent + dvrDescent;
			int num2 = TextDpi.ToTextDpi(this._lineProperties.CalcLineAdvanceForTextParagraph(this, dcp, TextDpi.FromTextDpi(num)));
			if (num != num2)
			{
				double num3 = 1.0 * (double)num2 / (1.0 * (double)num);
				dvrAscent = (int)((double)dvrAscent * num3);
				dvrDescent = (int)((double)dvrDescent * num3);
			}
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0012A818 File Offset: 0x00129818
		internal override void SetUpdateInfo(PTS.FSKCHANGE fskch, bool stopAsking)
		{
			base.SetUpdateInfo(fskch, stopAsking);
			if (fskch == PTS.FSKCHANGE.fskchInside)
			{
				this._textRunCache = new TextRunCache();
				this._lineProperties = null;
			}
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0012A838 File Offset: 0x00129838
		internal override void ClearUpdateInfo()
		{
			base.ClearUpdateInfo();
			if (this._attachedObjects != null)
			{
				for (int i = 0; i < this._attachedObjects.Count; i++)
				{
					this._attachedObjects[i].Para.ClearUpdateInfo();
				}
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0012A880 File Offset: 0x00129880
		internal override bool InvalidateStructure(int startPosition)
		{
			Invariant.Assert(base.ParagraphEndCharacterPosition >= startPosition);
			bool result = false;
			if (base.ParagraphStartCharacterPosition == startPosition)
			{
				result = true;
				AnchoredBlock anchoredBlock = null;
				if (this._attachedObjects != null && this._attachedObjects.Count > 0)
				{
					anchoredBlock = (AnchoredBlock)this._attachedObjects[0].Element;
				}
				if (anchoredBlock != null && startPosition == anchoredBlock.ElementStartOffset)
				{
					StaticTextPointer staticTextPointerFromCP = TextContainerHelper.GetStaticTextPointerFromCP(base.StructuralCache.TextContainer, startPosition);
					if (staticTextPointerFromCP.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
					{
						result = (anchoredBlock != staticTextPointerFromCP.GetAdjacentElement(LogicalDirection.Forward));
					}
				}
			}
			this.InvalidateTextFormatCache();
			if (this._attachedObjects != null)
			{
				for (int i = 0; i < this._attachedObjects.Count; i++)
				{
					BaseParagraph para = this._attachedObjects[i].Para;
					if (para.ParagraphEndCharacterPosition >= startPosition)
					{
						para.InvalidateStructure(startPosition);
					}
				}
			}
			return result;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0012A960 File Offset: 0x00129960
		internal override void InvalidateFormatCache()
		{
			this.InvalidateTextFormatCache();
			if (this._attachedObjects != null)
			{
				for (int i = 0; i < this._attachedObjects.Count; i++)
				{
					this._attachedObjects[i].Para.InvalidateFormatCache();
				}
			}
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0012A9A7 File Offset: 0x001299A7
		internal void InvalidateTextFormatCache()
		{
			this._textRunCache = new TextRunCache();
			this._lineProperties = null;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0012A9BC File Offset: 0x001299BC
		internal void FormatLineCore(Line line, IntPtr pbrLineIn, Line.FormattingContext ctx, int dcp, int width, bool firstLine, int dcpLine)
		{
			this.FormatLineCore(line, pbrLineIn, ctx, dcp, width, -1, firstLine, dcpLine);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0012A9DC File Offset: 0x001299DC
		internal void FormatLineCore(Line line, IntPtr pbrLineIn, Line.FormattingContext ctx, int dcp, int width, int trackWidth, bool firstLine, int dcpLine)
		{
			TextDpi.EnsureValidLineWidth(ref width);
			this._currentLine = line;
			TextLineBreak textLineBreak = null;
			if (pbrLineIn != IntPtr.Zero)
			{
				LineBreakRecord lineBreakRecord = base.PtsContext.HandleToObject(pbrLineIn) as LineBreakRecord;
				PTS.ValidateHandle(lineBreakRecord);
				textLineBreak = lineBreakRecord.TextLineBreak;
			}
			try
			{
				line.Format(ctx, dcp, width, trackWidth, this.GetLineProperties(firstLine, dcpLine), textLineBreak);
			}
			finally
			{
				this._currentLine = null;
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0012AA58 File Offset: 0x00129A58
		internal Size MeasureChild(InlineObjectRun inlineObject)
		{
			if (this._currentLine == null)
			{
				return ((OptimalTextSource)base.StructuralCache.TextFormatterHost.Context).MeasureChild(inlineObject);
			}
			return this._currentLine.MeasureChild(inlineObject);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0012AA8A File Offset: 0x00129A8A
		internal bool HasFiguresFloatersOrInlineObjects()
		{
			return this.HasFiguresOrFloaters() || (this._inlineObjects != null && this._inlineObjects.Count > 0);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0012AAAD File Offset: 0x00129AAD
		internal bool HasFiguresOrFloaters()
		{
			return this._attachedObjects != null && this._attachedObjects.Count > 0;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0012AAC8 File Offset: 0x00129AC8
		internal void UpdateTextContentRangeFromAttachedObjects(TextContentRange textContentRange, int dcpFirst, int dcpLast, PTS.FSATTACHEDOBJECTDESCRIPTION[] arrayAttachedObjectDesc)
		{
			int num = dcpFirst;
			int num2 = 0;
			while (this._attachedObjects != null && num2 < this._attachedObjects.Count)
			{
				AttachedObject attachedObject = this._attachedObjects[num2];
				int paragraphStartCharacterPosition = attachedObject.Para.ParagraphStartCharacterPosition;
				int cch = attachedObject.Para.Cch;
				if (paragraphStartCharacterPosition >= num && paragraphStartCharacterPosition < dcpLast)
				{
					textContentRange.Merge(new TextContentRange(num, paragraphStartCharacterPosition, base.StructuralCache.TextContainer));
					num = paragraphStartCharacterPosition + cch;
				}
				if (dcpLast < num)
				{
					break;
				}
				num2++;
			}
			if (num < dcpLast)
			{
				textContentRange.Merge(new TextContentRange(num, dcpLast, base.StructuralCache.TextContainer));
			}
			int num3 = 0;
			while (arrayAttachedObjectDesc != null && num3 < arrayAttachedObjectDesc.Length)
			{
				BaseParaClient baseParaClient = base.PtsContext.HandleToObject(arrayAttachedObjectDesc[num3].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				textContentRange.Merge(baseParaClient.GetTextContentRange());
				num3++;
			}
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0012ABB1 File Offset: 0x00129BB1
		internal void OnUIElementDesiredSizeChanged(object sender, DesiredSizeChangedEventArgs e)
		{
			base.StructuralCache.FormattingOwner.OnChildDesiredSizeChanged(e.Child);
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0012ABC9 File Offset: 0x00129BC9
		internal TextRunCache TextRunCache
		{
			get
			{
				return this._textRunCache;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0012ABD1 File Offset: 0x00129BD1
		internal LineProperties Properties
		{
			get
			{
				this.EnsureLineProperties();
				return this._lineProperties;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0012ABDF File Offset: 0x00129BDF
		internal bool IsOptimalParagraph
		{
			get
			{
				return base.StructuralCache.IsOptimalParagraphEnabled && this.GetLineProperties(false, 0).TextWrapping != TextWrapping.NoWrap;
			}
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0012AC04 File Offset: 0x00129C04
		private void EnsureLineProperties()
		{
			if (this._lineProperties == null || (this._lineProperties != null && this._lineProperties.DefaultTextRunProperties.PixelsPerDip != base.StructuralCache.TextFormatterHost.PixelsPerDip))
			{
				TextProperties defaultTextProperties = new TextProperties(base.Element, StaticTextPointer.Null, false, false, base.StructuralCache.TextFormatterHost.PixelsPerDip);
				this._lineProperties = new LineProperties(base.Element, base.StructuralCache.FormattingOwner, defaultTextProperties, null);
				if ((bool)base.Element.GetValue(Block.IsHyphenationEnabledProperty))
				{
					this._lineProperties.Hyphenator = base.StructuralCache.Hyphenator;
				}
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0012ACB4 File Offset: 0x00129CB4
		private void SubmitEmbeddedObjects<T>(ref List<T> objectsCached, int dcpStart, int dcpLim, List<T> objectsNew) where T : EmbeddedObject
		{
			ErrorHandler.Assert(objectsNew == null || (objectsNew[0].Dcp >= dcpStart && objectsNew[objectsNew.Count - 1].Dcp <= dcpLim), ErrorHandler.SubmitInvalidList);
			if (objectsCached == null)
			{
				if (objectsNew == null)
				{
					return;
				}
				objectsCached = new List<T>(objectsNew.Count);
			}
			int num = objectsCached.Count;
			while (num > 0 && objectsCached[num - 1].Dcp >= dcpLim)
			{
				num--;
			}
			int i = num;
			while (i > 0 && objectsCached[i - 1].Dcp >= dcpStart)
			{
				i--;
			}
			if (objectsNew == null)
			{
				for (int j = i; j < num; j++)
				{
					objectsCached[j].Dispose();
				}
				objectsCached.RemoveRange(i, num - i);
				return;
			}
			if (num == i)
			{
				objectsCached.InsertRange(i, objectsNew);
				return;
			}
			int num2 = 0;
			while (i < num)
			{
				T t = objectsCached[i];
				int k;
				for (k = num2; k < objectsNew.Count; k++)
				{
					T t2 = objectsNew[k];
					if (t.Element == t2.Element)
					{
						if (k > num2)
						{
							objectsCached.InsertRange(i, objectsNew.GetRange(num2, k - num2));
							num += k - num2;
							i += k - num2;
						}
						t.Update(t2);
						objectsNew[k] = t;
						num2 = k + 1;
						i++;
						t2.Dispose();
						break;
					}
				}
				if (k >= objectsNew.Count)
				{
					objectsCached[i].Dispose();
					objectsCached.RemoveAt(i);
					num--;
				}
			}
			if (num2 < objectsNew.Count)
			{
				objectsCached.InsertRange(num, objectsNew.GetRange(num2, objectsNew.Count - num2));
			}
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0012AEAC File Offset: 0x00129EAC
		private void UpdateEmbeddedObjectsCache<T>(ref List<T> objectsCached, int dcpStart, int cchDeleted, int cchDiff) where T : EmbeddedObject
		{
			if (objectsCached != null)
			{
				int num = 0;
				while (num < objectsCached.Count && objectsCached[num].Dcp < dcpStart)
				{
					num++;
				}
				int i = num;
				while (i < objectsCached.Count && objectsCached[i].Dcp < dcpStart + cchDeleted)
				{
					i++;
				}
				if (num != i)
				{
					for (int j = num; j < i; j++)
					{
						objectsCached[j].Dispose();
					}
					objectsCached.RemoveRange(num, i - num);
				}
				while (i < objectsCached.Count)
				{
					objectsCached[i].Dcp += cchDiff;
					i++;
				}
				if (objectsCached.Count == 0)
				{
					objectsCached = null;
				}
			}
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0012AF74 File Offset: 0x00129F74
		private TextParagraphProperties GetLineProperties(bool firstLine, int dcpLine)
		{
			this.EnsureLineProperties();
			if (firstLine && this._lineProperties.HasFirstLineProperties)
			{
				if (dcpLine != 0)
				{
					firstLine = false;
				}
				else if (TextContainerHelper.GetCPFromElement(base.StructuralCache.TextContainer, base.Element, ElementEdge.AfterStart) < base.ParagraphStartCharacterPosition)
				{
					firstLine = false;
				}
				if (firstLine)
				{
					return this._lineProperties.FirstLineProps;
				}
			}
			return this._lineProperties;
		}

		// Token: 0x0400081A RID: 2074
		private List<AttachedObject> _attachedObjects;

		// Token: 0x0400081B RID: 2075
		private List<InlineObject> _inlineObjects;

		// Token: 0x0400081C RID: 2076
		private LineProperties _lineProperties;

		// Token: 0x0400081D RID: 2077
		private TextRunCache _textRunCache = new TextRunCache();

		// Token: 0x0400081E RID: 2078
		private Line _currentLine;
	}
}
