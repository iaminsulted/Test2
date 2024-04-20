using System;

namespace System.Windows.Markup
{
	// Token: 0x020004A7 RID: 1191
	internal class BamlStaticResourceStartRecord : BamlElementStartRecord
	{
		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x001FCECA File Offset: 0x001FBECA
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StaticResourceStart;
			}
		}
	}
}
