using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200012F RID: 303
	internal sealed class OptimalBreakSession : UnmanagedHandle
	{
		// Token: 0x06000851 RID: 2129 RVA: 0x001149CD File Offset: 0x001139CD
		internal OptimalBreakSession(TextParagraph textParagraph, TextParaClient textParaClient, TextParagraphCache TextParagraphCache, OptimalTextSource optimalTextSource) : base(textParagraph.PtsContext)
		{
			this._textParagraph = textParagraph;
			this._textParaClient = textParaClient;
			this._textParagraphCache = TextParagraphCache;
			this._optimalTextSource = optimalTextSource;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x001149F8 File Offset: 0x001139F8
		public override void Dispose()
		{
			try
			{
				if (this._textParagraphCache != null)
				{
					this._textParagraphCache.Dispose();
				}
				if (this._optimalTextSource != null)
				{
					this._optimalTextSource.Dispose();
				}
			}
			finally
			{
				this._textParagraphCache = null;
				this._optimalTextSource = null;
			}
			base.Dispose();
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00114A54 File Offset: 0x00113A54
		internal TextParagraphCache TextParagraphCache
		{
			get
			{
				return this._textParagraphCache;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x00114A5C File Offset: 0x00113A5C
		internal TextParagraph TextParagraph
		{
			get
			{
				return this._textParagraph;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x00114A64 File Offset: 0x00113A64
		internal TextParaClient TextParaClient
		{
			get
			{
				return this._textParaClient;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x00114A6C File Offset: 0x00113A6C
		internal OptimalTextSource OptimalTextSource
		{
			get
			{
				return this._optimalTextSource;
			}
		}

		// Token: 0x040007B3 RID: 1971
		private TextParagraphCache _textParagraphCache;

		// Token: 0x040007B4 RID: 1972
		private TextParagraph _textParagraph;

		// Token: 0x040007B5 RID: 1973
		private TextParaClient _textParaClient;

		// Token: 0x040007B6 RID: 1974
		private OptimalTextSource _optimalTextSource;
	}
}
