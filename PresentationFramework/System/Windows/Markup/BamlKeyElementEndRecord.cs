using System;

namespace System.Windows.Markup
{
	// Token: 0x020004B2 RID: 1202
	internal class BamlKeyElementEndRecord : BamlElementEndRecord
	{
		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06003D97 RID: 15767 RVA: 0x001FD37D File Offset: 0x001FC37D
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.KeyElementEnd;
			}
		}
	}
}
