using System;

namespace System.Windows.Markup
{
	// Token: 0x020004AC RID: 1196
	internal class BamlTextRecord : BamlStringValueRecord
	{
		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06003D74 RID: 15732 RVA: 0x001FD17B File Offset: 0x001FC17B
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.Text;
			}
		}
	}
}
