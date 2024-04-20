using System;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200014A RID: 330
	internal sealed class TextFormatterHost : TextSource
	{
		// Token: 0x06000A53 RID: 2643 RVA: 0x00124786 File Offset: 0x00123786
		internal TextFormatterHost(TextFormatter textFormatter, TextFormattingMode textFormattingMode, double pixelsPerDip)
		{
			if (textFormatter == null)
			{
				this.TextFormatter = TextFormatter.FromCurrentDispatcher(textFormattingMode);
			}
			else
			{
				this.TextFormatter = textFormatter;
			}
			base.PixelsPerDip = pixelsPerDip;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x001247B0 File Offset: 0x001237B0
		public override TextRun GetTextRun(int textSourceCharacterIndex)
		{
			TextRun textRun = this.Context.GetTextRun(textSourceCharacterIndex);
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x001247E4 File Offset: 0x001237E4
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit)
		{
			return this.Context.GetPrecedingText(textSourceCharacterIndexLimit);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x001247F2 File Offset: 0x001237F2
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return this.Context.GetTextEffectCharacterIndexFromTextSourceCharacterIndex(textSourceCharacterIndex);
		}

		// Token: 0x04000817 RID: 2071
		internal LineBase Context;

		// Token: 0x04000818 RID: 2072
		internal TextFormatter TextFormatter;
	}
}
