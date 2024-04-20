using System;

namespace System.Windows.Documents
{
	// Token: 0x0200063B RID: 1595
	internal interface IXamlContentHandler
	{
		// Token: 0x06004F04 RID: 20228
		XamlToRtfError StartDocument();

		// Token: 0x06004F05 RID: 20229
		XamlToRtfError EndDocument();

		// Token: 0x06004F06 RID: 20230
		XamlToRtfError StartPrefixMapping(string prefix, string uri);

		// Token: 0x06004F07 RID: 20231
		XamlToRtfError StartElement(string nameSpaceUri, string localName, string qName, IXamlAttributes attributes);

		// Token: 0x06004F08 RID: 20232
		XamlToRtfError EndElement(string nameSpaceUri, string localName, string qName);

		// Token: 0x06004F09 RID: 20233
		XamlToRtfError Characters(string characters);

		// Token: 0x06004F0A RID: 20234
		XamlToRtfError IgnorableWhitespace(string characters);

		// Token: 0x06004F0B RID: 20235
		XamlToRtfError ProcessingInstruction(string target, string data);

		// Token: 0x06004F0C RID: 20236
		XamlToRtfError SkippedEntity(string name);
	}
}
