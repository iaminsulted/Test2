using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004B9 RID: 1209
	internal class BamlContentPropertyRecord : BamlRecord
	{
		// Token: 0x06003DF7 RID: 15863 RVA: 0x001FDC71 File Offset: 0x001FCC71
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x001FDC7F File Offset: 0x001FCC7F
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x001FDC8D File Offset: 0x001FCC8D
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlContentPropertyRecord)record)._attributeId = this._attributeId;
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06003DFA RID: 15866 RVA: 0x001FDCA7 File Offset: 0x001FCCA7
		// (set) Token: 0x06003DFB RID: 15867 RVA: 0x001FDCAF File Offset: 0x001FCCAF
		internal short AttributeId
		{
			get
			{
				return this._attributeId;
			}
			set
			{
				this._attributeId = value;
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x001FDCB8 File Offset: 0x001FCCB8
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ContentProperty;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06003DFD RID: 15869 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool HasSerializer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001EFE RID: 7934
		private short _attributeId = -1;
	}
}
