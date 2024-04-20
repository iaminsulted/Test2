using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000492 RID: 1170
	internal class BamlPropertyWithConverterRecord : BamlPropertyRecord
	{
		// Token: 0x06003CB4 RID: 15540 RVA: 0x001FC087 File Offset: 0x001FB087
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this.ConverterTypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x001FC09C File Offset: 0x001FB09C
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			bamlBinaryWriter.Write(this.ConverterTypeId);
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x001FC0B1 File Offset: 0x001FB0B1
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlPropertyWithConverterRecord)record)._converterTypeId = this._converterTypeId;
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x001FC0CB File Offset: 0x001FB0CB
		// (set) Token: 0x06003CB8 RID: 15544 RVA: 0x001FC0D3 File Offset: 0x001FB0D3
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

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x001FC0DC File Offset: 0x001FB0DC
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyWithConverter;
			}
		}

		// Token: 0x04001EB1 RID: 7857
		private short _converterTypeId;
	}
}
