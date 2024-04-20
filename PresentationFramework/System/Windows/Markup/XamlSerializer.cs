using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000510 RID: 1296
	internal class XamlSerializer
	{
		// Token: 0x06004074 RID: 16500 RVA: 0x002140D7 File Offset: 0x002130D7
		internal virtual void ConvertXamlToBaml(XamlReaderHelper tokenReader, ParserContext context, XamlNode xamlNode, BamlRecordWriter bamlWriter)
		{
			throw new InvalidOperationException(SR.Get("InvalidDeSerialize"));
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x002140D7 File Offset: 0x002130D7
		internal virtual void ConvertXamlToObject(XamlReaderHelper tokenReader, ReadWriteStreamManager streamManager, ParserContext context, XamlNode xamlNode, BamlRecordReader reader)
		{
			throw new InvalidOperationException(SR.Get("InvalidDeSerialize"));
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x002140D7 File Offset: 0x002130D7
		internal virtual void ConvertBamlToObject(BamlRecordReader reader, BamlRecord bamlRecord, ParserContext context)
		{
			throw new InvalidOperationException(SR.Get("InvalidDeSerialize"));
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x002140E8 File Offset: 0x002130E8
		public virtual bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			throw new InvalidOperationException(SR.Get("InvalidCustomSerialize"));
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x002140E8 File Offset: 0x002130E8
		public virtual object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			throw new InvalidOperationException(SR.Get("InvalidCustomSerialize"));
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual object GetDictionaryKey(BamlRecord bamlRecord, ParserContext parserContext)
		{
			return null;
		}

		// Token: 0x0400242D RID: 9261
		internal const string DefNamespacePrefix = "x";

		// Token: 0x0400242E RID: 9262
		internal const string DefNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";

		// Token: 0x0400242F RID: 9263
		internal const string ArrayTag = "Array";

		// Token: 0x04002430 RID: 9264
		internal const string ArrayTagTypeAttribute = "Type";
	}
}
