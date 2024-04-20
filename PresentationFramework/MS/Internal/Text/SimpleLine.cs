using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000325 RID: 805
	internal sealed class SimpleLine : Line
	{
		// Token: 0x06001DFD RID: 7677 RVA: 0x0016EC88 File Offset: 0x0016DC88
		public override TextRun GetTextRun(int dcp)
		{
			TextRun textRun;
			if (dcp < this._content.Length)
			{
				textRun = new TextCharacters(this._content, dcp, this._content.Length - dcp, this._textProps);
			}
			else
			{
				textRun = new TextEndOfParagraph(Line._syntheticCharacterLength);
			}
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x0016ECEC File Offset: 0x0016DCEC
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				empty = new CharacterBufferRange(this._content, 0, Math.Min(dcp, this._content.Length));
				culture = this._textProps.CultureInfo;
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(dcp, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x001136C4 File Offset: 0x001126C4
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return textSourceCharacterIndex;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0016ED3D File Offset: 0x0016DD3D
		internal SimpleLine(TextBlock owner, string content, TextRunProperties textProps) : base(owner)
		{
			this._content = content;
			this._textProps = textProps;
		}

		// Token: 0x04000EE2 RID: 3810
		private readonly string _content;

		// Token: 0x04000EE3 RID: 3811
		private readonly TextRunProperties _textProps;
	}
}
