using System;

namespace MS.Internal.AppModel
{
	// Token: 0x020002B5 RID: 693
	[Serializable]
	internal struct SubStream
	{
		// Token: 0x060019FF RID: 6655 RVA: 0x001629EA File Offset: 0x001619EA
		internal SubStream(string propertyName, byte[] dataBytes)
		{
			this._propertyName = propertyName;
			this._data = dataBytes;
		}

		// Token: 0x04000D93 RID: 3475
		internal string _propertyName;

		// Token: 0x04000D94 RID: 3476
		internal byte[] _data;
	}
}
