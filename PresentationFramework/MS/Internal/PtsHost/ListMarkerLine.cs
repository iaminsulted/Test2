using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000129 RID: 297
	internal class ListMarkerLine : LineBase
	{
		// Token: 0x06000812 RID: 2066 RVA: 0x00113698 File Offset: 0x00112698
		internal ListMarkerLine(TextFormatterHost host, ListParaClient paraClient) : base(paraClient)
		{
			this._host = host;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x001136A8 File Offset: 0x001126A8
		internal override TextRun GetTextRun(int dcp)
		{
			return new ParagraphBreakRun(1, PTS.FSFLRES.fsflrEndOfParagraph);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x001136B1 File Offset: 0x001126B1
		internal override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			return new TextSpan<CultureSpecificCharacterBufferRange>(0, new CultureSpecificCharacterBufferRange(null, CharacterBufferRange.Empty));
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x001136C4 File Offset: 0x001126C4
		internal override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int dcp)
		{
			return dcp;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x001136C8 File Offset: 0x001126C8
		internal void FormatAndDrawVisual(DrawingContext ctx, LineProperties lineProps, int ur, int vrBaseline)
		{
			bool flag = lineProps.FlowDirection == FlowDirection.RightToLeft;
			this._host.Context = this;
			try
			{
				TextLine textLine = this._host.TextFormatter.FormatLine(this._host, 0, 0.0, lineProps.FirstLineProps, null, new TextRunCache());
				Point origin = new Point(TextDpi.FromTextDpi(ur), TextDpi.FromTextDpi(vrBaseline) - textLine.Baseline);
				textLine.Draw(ctx, origin, flag ? InvertAxes.Horizontal : InvertAxes.None);
				textLine.Dispose();
			}
			finally
			{
				this._host.Context = null;
			}
		}

		// Token: 0x040007A3 RID: 1955
		private readonly TextFormatterHost _host;
	}
}
