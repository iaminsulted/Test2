using System;

namespace System.Windows.Markup
{
	// Token: 0x020004B1 RID: 1201
	internal class BamlKeyElementStartRecord : BamlDefAttributeKeyTypeRecord, IBamlDictionaryKey
	{
		// Token: 0x06003D95 RID: 15765 RVA: 0x001FD36B File Offset: 0x001FC36B
		internal BamlKeyElementStartRecord()
		{
			base.Pin();
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06003D96 RID: 15766 RVA: 0x001FD379 File Offset: 0x001FC379
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.KeyElementStart;
			}
		}
	}
}
