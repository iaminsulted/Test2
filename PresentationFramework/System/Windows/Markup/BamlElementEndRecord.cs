using System;

namespace System.Windows.Markup
{
	// Token: 0x020004B0 RID: 1200
	internal class BamlElementEndRecord : BamlRecord
	{
		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06003D93 RID: 15763 RVA: 0x001FC019 File Offset: 0x001FB019
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ElementEnd;
			}
		}
	}
}
