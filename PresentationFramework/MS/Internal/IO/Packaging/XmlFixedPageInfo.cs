using System;
using System.Globalization;
using System.Windows;
using System.Xml;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200016B RID: 363
	internal class XmlFixedPageInfo : FixedPageInfo
	{
		// Token: 0x06000C2A RID: 3114 RVA: 0x0012F9B4 File Offset: 0x0012E9B4
		internal XmlFixedPageInfo(XmlNode fixedPageNode)
		{
			this._pageNode = fixedPageNode;
			if (this._pageNode.LocalName != "FixedPage" || this._pageNode.NamespaceURI != ElementTableKey.FixedMarkupNamespace)
			{
				throw new ArgumentException(SR.Get("UnexpectedXmlNodeInXmlFixedPageInfoConstructor", new object[]
				{
					this._pageNode.NamespaceURI,
					this._pageNode.LocalName,
					ElementTableKey.FixedMarkupNamespace,
					"FixedPage"
				}));
			}
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0012FA3E File Offset: 0x0012EA3E
		internal override GlyphRunInfo GlyphRunAtPosition(int position)
		{
			if (position < 0 || position >= this.GlyphRunList.Length)
			{
				return null;
			}
			if (this.GlyphRunList[position] == null)
			{
				this.GlyphRunList[position] = new XmlGlyphRunInfo(this.NodeList[position]);
			}
			return this.GlyphRunList[position];
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0012FA7C File Offset: 0x0012EA7C
		internal override int GlyphRunCount
		{
			get
			{
				return this.GlyphRunList.Length;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x0012FA86 File Offset: 0x0012EA86
		private XmlGlyphRunInfo[] GlyphRunList
		{
			get
			{
				if (this._glyphRunList == null)
				{
					this._glyphRunList = new XmlGlyphRunInfo[this.NodeList.Count];
				}
				return this._glyphRunList;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0012FAAC File Offset: 0x0012EAAC
		private XmlNodeList NodeList
		{
			get
			{
				if (this._nodeList == null)
				{
					string xpath = string.Format(CultureInfo.InvariantCulture, ".//*[namespace-uri()='{0}' and local-name()='{1}']", ElementTableKey.FixedMarkupNamespace, "Glyphs");
					this._nodeList = this._pageNode.SelectNodes(xpath);
				}
				return this._nodeList;
			}
		}

		// Token: 0x04000922 RID: 2338
		private const string _fixedPageName = "FixedPage";

		// Token: 0x04000923 RID: 2339
		private const string _glyphRunName = "Glyphs";

		// Token: 0x04000924 RID: 2340
		private XmlNode _pageNode;

		// Token: 0x04000925 RID: 2341
		private XmlNodeList _nodeList;

		// Token: 0x04000926 RID: 2342
		private XmlGlyphRunInfo[] _glyphRunList;
	}
}
