using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004AE RID: 1198
	internal class BamlTextWithConverterRecord : BamlTextRecord
	{
		// Token: 0x06003D7D RID: 15741 RVA: 0x001FD1D2 File Offset: 0x001FC1D2
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this.ConverterTypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x001FD1E7 File Offset: 0x001FC1E7
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			bamlBinaryWriter.Write(this.ConverterTypeId);
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x001FD1FC File Offset: 0x001FC1FC
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlTextWithConverterRecord)record)._converterTypeId = this._converterTypeId;
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06003D80 RID: 15744 RVA: 0x001FD216 File Offset: 0x001FC216
		// (set) Token: 0x06003D81 RID: 15745 RVA: 0x001FD21E File Offset: 0x001FC21E
		internal short ConverterTypeId
		{
			get
			{
				return this._converterTypeId;
			}
			set
			{
				this._converterTypeId = value;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06003D82 RID: 15746 RVA: 0x001FD227 File Offset: 0x001FC227
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TextWithConverter;
			}
		}

		// Token: 0x04001EDD RID: 7901
		private short _converterTypeId;
	}
}
