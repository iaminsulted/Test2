using System;

namespace System.Windows.Documents
{
	// Token: 0x0200063A RID: 1594
	internal interface IXamlAttributes
	{
		// Token: 0x06004EF7 RID: 20215
		XamlToRtfError GetLength(ref int length);

		// Token: 0x06004EF8 RID: 20216
		XamlToRtfError GetUri(int index, ref string uri);

		// Token: 0x06004EF9 RID: 20217
		XamlToRtfError GetLocalName(int index, ref string localName);

		// Token: 0x06004EFA RID: 20218
		XamlToRtfError GetQName(int index, ref string qName);

		// Token: 0x06004EFB RID: 20219
		XamlToRtfError GetName(int index, ref string uri, ref string localName, ref string qName);

		// Token: 0x06004EFC RID: 20220
		XamlToRtfError GetIndexFromName(string uri, string localName, ref int index);

		// Token: 0x06004EFD RID: 20221
		XamlToRtfError GetIndexFromQName(string qName, ref int index);

		// Token: 0x06004EFE RID: 20222
		XamlToRtfError GetType(int index, ref string type);

		// Token: 0x06004EFF RID: 20223
		XamlToRtfError GetTypeFromName(string uri, string localName, ref string type);

		// Token: 0x06004F00 RID: 20224
		XamlToRtfError GetTypeFromQName(string qName, ref string type);

		// Token: 0x06004F01 RID: 20225
		XamlToRtfError GetValue(int index, ref string value);

		// Token: 0x06004F02 RID: 20226
		XamlToRtfError GetValueFromName(string uri, string localName, ref string value);

		// Token: 0x06004F03 RID: 20227
		XamlToRtfError GetValueFromQName(string qName, ref string value);
	}
}
