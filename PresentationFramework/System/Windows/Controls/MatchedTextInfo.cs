using System;

namespace System.Windows.Controls
{
	// Token: 0x02000715 RID: 1813
	internal class MatchedTextInfo
	{
		// Token: 0x06005F40 RID: 24384 RVA: 0x002948BC File Offset: 0x002938BC
		internal MatchedTextInfo(int matchedItemIndex, string matchedText, int matchedPrefixLength, int textExcludingPrefixLength)
		{
			this._matchedItemIndex = matchedItemIndex;
			this._matchedText = matchedText;
			this._matchedPrefixLength = matchedPrefixLength;
			this._textExcludingPrefixLength = textExcludingPrefixLength;
		}

		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x06005F41 RID: 24385 RVA: 0x002948E1 File Offset: 0x002938E1
		internal static MatchedTextInfo NoMatch
		{
			get
			{
				return MatchedTextInfo.s_NoMatch;
			}
		}

		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x06005F42 RID: 24386 RVA: 0x002948E8 File Offset: 0x002938E8
		internal string MatchedText
		{
			get
			{
				return this._matchedText;
			}
		}

		// Token: 0x170015FB RID: 5627
		// (get) Token: 0x06005F43 RID: 24387 RVA: 0x002948F0 File Offset: 0x002938F0
		internal int MatchedItemIndex
		{
			get
			{
				return this._matchedItemIndex;
			}
		}

		// Token: 0x170015FC RID: 5628
		// (get) Token: 0x06005F44 RID: 24388 RVA: 0x002948F8 File Offset: 0x002938F8
		internal int MatchedPrefixLength
		{
			get
			{
				return this._matchedPrefixLength;
			}
		}

		// Token: 0x170015FD RID: 5629
		// (get) Token: 0x06005F45 RID: 24389 RVA: 0x00294900 File Offset: 0x00293900
		internal int TextExcludingPrefixLength
		{
			get
			{
				return this._textExcludingPrefixLength;
			}
		}

		// Token: 0x040031B6 RID: 12726
		private readonly string _matchedText;

		// Token: 0x040031B7 RID: 12727
		private readonly int _matchedItemIndex;

		// Token: 0x040031B8 RID: 12728
		private readonly int _matchedPrefixLength;

		// Token: 0x040031B9 RID: 12729
		private readonly int _textExcludingPrefixLength;

		// Token: 0x040031BA RID: 12730
		private static MatchedTextInfo s_NoMatch = new MatchedTextInfo(-1, null, 0, 0);
	}
}
