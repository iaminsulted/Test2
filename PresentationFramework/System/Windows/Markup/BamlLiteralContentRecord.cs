using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004A2 RID: 1186
	internal class BamlLiteralContentRecord : BamlStringValueRecord
	{
		// Token: 0x06003D1B RID: 15643 RVA: 0x001FCB17 File Offset: 0x001FBB17
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.Value = bamlBinaryReader.ReadString();
			bamlBinaryReader.ReadInt32();
			bamlBinaryReader.ReadInt32();
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x001FCB33 File Offset: 0x001FBB33
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.Value);
			bamlBinaryWriter.Write(0);
			bamlBinaryWriter.Write(0);
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06003D1D RID: 15645 RVA: 0x001FCB4F File Offset: 0x001FBB4F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.LiteralContent;
			}
		}
	}
}
