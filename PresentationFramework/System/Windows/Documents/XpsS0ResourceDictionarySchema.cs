using System;
using System.IO;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000604 RID: 1540
	internal sealed class XpsS0ResourceDictionarySchema : XpsS0Schema
	{
		// Token: 0x06004B1E RID: 19230 RVA: 0x00235EE0 File Offset: 0x00234EE0
		public XpsS0ResourceDictionarySchema()
		{
			XpsSchema.RegisterSchema(this, new ContentType[]
			{
				XpsS0Schema._resourceDictionaryContentType
			});
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x00235EFC File Offset: 0x00234EFC
		public override string[] ExtractUriFromAttr(string attrName, string attrValue)
		{
			if (attrName.Equals("Source", StringComparison.Ordinal))
			{
				throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedMimeType"));
			}
			return base.ExtractUriFromAttr(attrName, attrValue);
		}
	}
}
