using System;

namespace System.Windows.Markup
{
	// Token: 0x0200049E RID: 1182
	internal class BamlPropertyArrayStartRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06003D0E RID: 15630 RVA: 0x001FCA9D File Offset: 0x001FBA9D
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyArrayStart;
			}
		}
	}
}
