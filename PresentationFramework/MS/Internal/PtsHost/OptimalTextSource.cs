using System;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000131 RID: 305
	internal sealed class OptimalTextSource : LineBase
	{
		// Token: 0x0600085A RID: 2138 RVA: 0x00114AB3 File Offset: 0x00113AB3
		internal OptimalTextSource(TextFormatterHost host, int cpPara, int durTrack, TextParaClient paraClient, TextRunCache runCache) : base(paraClient)
		{
			this._host = host;
			this._durTrack = durTrack;
			this._runCache = runCache;
			this._cpPara = cpPara;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00114ADA File Offset: 0x00113ADA
		public override void Dispose()
		{
			base.Dispose();
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00114AE4 File Offset: 0x00113AE4
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

		// Token: 0x0600085D RID: 2141 RVA: 0x00114B94 File Offset: 0x00113B94
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

		// Token: 0x0600085E RID: 2142 RVA: 0x00114C8C File Offset: 0x00113C8C
		internal override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int dcp)
		{
			ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, this._cpPara + dcp, LogicalDirection.Forward);
			return textPointerFromCP.TextContainer.Start.GetOffsetToPosition(textPointerFromCP);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00114CD0 File Offset: 0x00113CD0
		internal PTS.FSFLRES GetFormatResultForBreakpoint(int dcp, TextBreakpoint textBreakpoint)
		{
			int num = 0;
			PTS.FSFLRES result = PTS.FSFLRES.fsflrOutOfSpace;
			foreach (TextSpan<TextRun> textSpan in this._runCache.GetTextRunSpans())
			{
				TextRun value = textSpan.Value;
				if (value != null && num + value.Length >= dcp + textBreakpoint.Length)
				{
					if (value is ParagraphBreakRun)
					{
						result = ((ParagraphBreakRun)value).BreakReason;
						break;
					}
					if (value is LineBreakRun)
					{
						result = ((LineBreakRun)value).BreakReason;
						break;
					}
					break;
				}
				else
				{
					num += textSpan.Length;
				}
			}
			return result;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00114D78 File Offset: 0x00113D78
		internal Size MeasureChild(InlineObjectRun inlineObject)
		{
			double height = this._paraClient.Paragraph.StructuralCache.CurrentFormatContext.DocumentPageSize.Height;
			if (!this._paraClient.Paragraph.StructuralCache.CurrentFormatContext.FinitePage)
			{
				height = double.PositiveInfinity;
			}
			return inlineObject.UIElementIsland.DoLayout(new Size(TextDpi.FromTextDpi(this._durTrack), height), true, true);
		}

		// Token: 0x040007B9 RID: 1977
		private readonly TextFormatterHost _host;

		// Token: 0x040007BA RID: 1978
		private TextRunCache _runCache;

		// Token: 0x040007BB RID: 1979
		private int _durTrack;

		// Token: 0x040007BC RID: 1980
		private int _cpPara;
	}
}
