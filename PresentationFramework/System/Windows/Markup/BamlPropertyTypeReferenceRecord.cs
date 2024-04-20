using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000491 RID: 1169
	internal class BamlPropertyTypeReferenceRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x06003CAB RID: 15531 RVA: 0x001FC024 File Offset: 0x001FB024
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.AttributeId = bamlBinaryReader.ReadInt16();
			this.TypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x001FC03E File Offset: 0x001FB03E
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.AttributeId);
			bamlBinaryWriter.Write(this.TypeId);
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x001FC058 File Offset: 0x001FB058
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlPropertyTypeReferenceRecord)record)._typeId = this._typeId;
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06003CAE RID: 15534 RVA: 0x001FC072 File Offset: 0x001FB072
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyTypeReference;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06003CAF RID: 15535 RVA: 0x001FC076 File Offset: 0x001FB076
		// (set) Token: 0x06003CB0 RID: 15536 RVA: 0x001FC07E File Offset: 0x001FB07E
		internal short TypeId
		{
			get
			{
				return this._typeId;
			}
			set
			{
				this._typeId = value;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06003CB1 RID: 15537 RVA: 0x001FC019 File Offset: 0x001FB019
		// (set) Token: 0x06003CB2 RID: 15538 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override int RecordSize
		{
			get
			{
				return 4;
			}
			set
			{
			}
		}

		// Token: 0x04001EB0 RID: 7856
		private short _typeId;
	}
}
