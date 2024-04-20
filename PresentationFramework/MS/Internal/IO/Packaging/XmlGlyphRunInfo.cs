using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200016C RID: 364
	internal class XmlGlyphRunInfo : GlyphRunInfo
	{
		// Token: 0x06000C2F RID: 3119 RVA: 0x0012FAF3 File Offset: 0x0012EAF3
		internal XmlGlyphRunInfo(XmlNode glyphsNode)
		{
			this._glyphsNode = (glyphsNode as XmlElement);
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0012FB07 File Offset: 0x0012EB07
		internal override Point StartPosition
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0012FB07 File Offset: 0x0012EB07
		internal override Point EndPosition
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x0012FB07 File Offset: 0x0012EB07
		internal override double WidthEmFontSize
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0012FB07 File Offset: 0x0012EB07
		internal override double HeightEmFontSize
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x0012FB07 File Offset: 0x0012EB07
		internal override bool GlyphsHaveSidewaysOrientation
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0012FB07 File Offset: 0x0012EB07
		internal override int BidiLevel
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x0012FB18 File Offset: 0x0012EB18
		internal override uint LanguageID
		{
			get
			{
				checked
				{
					if (this._languageID == null)
					{
						XmlElement xmlElement = this._glyphsNode;
						while (xmlElement != null && this._languageID == null)
						{
							string attribute = xmlElement.GetAttribute("xml:lang");
							if (attribute != null && attribute.Length > 0)
							{
								if (string.CompareOrdinal(attribute.ToUpperInvariant(), "UND") == 0)
								{
									this._languageID = new uint?(0U);
								}
								else
								{
									CultureInfo compatibleCulture = XmlLanguage.GetLanguage(attribute).GetCompatibleCulture();
									this._languageID = new uint?((uint)compatibleCulture.LCID);
								}
							}
							xmlElement = (xmlElement.ParentNode as XmlElement);
						}
						if (this._languageID == null)
						{
							this._languageID = new uint?((uint)CultureInfo.InvariantCulture.LCID);
						}
					}
					return this._languageID.Value;
				}
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0012FBDE File Offset: 0x0012EBDE
		internal override string UnicodeString
		{
			get
			{
				if (this._unicodeString == null)
				{
					this._unicodeString = this._glyphsNode.GetAttribute("UnicodeString");
				}
				return this._unicodeString;
			}
		}

		// Token: 0x04000927 RID: 2343
		private const string _glyphRunName = "Glyphs";

		// Token: 0x04000928 RID: 2344
		private const string _xmlLangAttribute = "xml:lang";

		// Token: 0x04000929 RID: 2345
		private const string _unicodeStringAttribute = "UnicodeString";

		// Token: 0x0400092A RID: 2346
		private const string _undeterminedLanguageStringUpper = "UND";

		// Token: 0x0400092B RID: 2347
		private XmlElement _glyphsNode;

		// Token: 0x0400092C RID: 2348
		private string _unicodeString;

		// Token: 0x0400092D RID: 2349
		private uint? _languageID;
	}
}
