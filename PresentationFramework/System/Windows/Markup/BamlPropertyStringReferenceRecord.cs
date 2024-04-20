using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000490 RID: 1168
	internal class BamlPropertyStringReferenceRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x06003CA2 RID: 15522 RVA: 0x001FBFB6 File Offset: 0x001FAFB6
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.AttributeId = bamlBinaryReader.ReadInt16();
			this.StringId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x001FBFD0 File Offset: 0x001FAFD0
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.AttributeId);
			bamlBinaryWriter.Write(this.StringId);
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x001FBFEA File Offset: 0x001FAFEA
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlPropertyStringReferenceRecord)record)._stringId = this._stringId;
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06003CA5 RID: 15525 RVA: 0x001FC004 File Offset: 0x001FB004
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyStringReference;
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x001FC008 File Offset: 0x001FB008
		// (set) Token: 0x06003CA7 RID: 15527 RVA: 0x001FC010 File Offset: 0x001FB010
		internal short StringId
		{
			get
			{
				return this._stringId;
			}
			set
			{
				this._stringId = value;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x001FC019 File Offset: 0x001FB019
		// (set) Token: 0x06003CA9 RID: 15529 RVA: 0x000F6B2C File Offset: 0x000F5B2C
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

		// Token: 0x04001EAF RID: 7855
		private short _stringId;
	}
}
