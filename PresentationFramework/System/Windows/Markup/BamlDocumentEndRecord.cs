using System;

namespace System.Windows.Markup
{
	// Token: 0x020004B3 RID: 1203
	internal class BamlDocumentEndRecord : BamlRecord
	{
		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06003D99 RID: 15769 RVA: 0x0010A7E1 File Offset: 0x001097E1
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DocumentEnd;
			}
		}
	}
}
