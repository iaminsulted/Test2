using System;
using System.Xml;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000160 RID: 352
	internal class FixedPageContentExtractor
	{
		// Token: 0x06000BB0 RID: 2992 RVA: 0x0012D28F File Offset: 0x0012C28F
		internal FixedPageContentExtractor(XmlNode fixedPage)
		{
			this._fixedPageInfo = new XmlFixedPageInfo(fixedPage);
			this._nextGlyphRun = 0;
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0012D2AC File Offset: 0x0012C2AC
		internal string NextGlyphContent(out bool inline, out uint lcid)
		{
			inline = false;
			lcid = 0U;
			if (this._nextGlyphRun >= this._fixedPageInfo.GlyphRunCount)
			{
				return null;
			}
			GlyphRunInfo glyphRunInfo = this._fixedPageInfo.GlyphRunAtPosition(this._nextGlyphRun);
			lcid = glyphRunInfo.LanguageID;
			this._nextGlyphRun++;
			return glyphRunInfo.UnicodeString;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x0012D302 File Offset: 0x0012C302
		internal bool AtEndOfPage
		{
			get
			{
				return this._nextGlyphRun >= this._fixedPageInfo.GlyphRunCount;
			}
		}

		// Token: 0x040008E0 RID: 2272
		private XmlFixedPageInfo _fixedPageInfo;

		// Token: 0x040008E1 RID: 2273
		private int _nextGlyphRun;
	}
}
