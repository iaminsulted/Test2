using System;

namespace System.Windows.Markup
{
	// Token: 0x020004A8 RID: 1192
	internal class BamlStaticResourceEndRecord : BamlElementEndRecord
	{
		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06003D4D RID: 15693 RVA: 0x001FCECE File Offset: 0x001FBECE
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StaticResourceEnd;
			}
		}
	}
}
