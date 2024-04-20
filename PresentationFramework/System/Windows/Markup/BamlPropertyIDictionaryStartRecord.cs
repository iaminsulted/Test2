using System;

namespace System.Windows.Markup
{
	// Token: 0x020004A0 RID: 1184
	internal class BamlPropertyIDictionaryStartRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06003D12 RID: 15634 RVA: 0x001A519B File Offset: 0x001A419B
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyIDictionaryStart;
			}
		}
	}
}
