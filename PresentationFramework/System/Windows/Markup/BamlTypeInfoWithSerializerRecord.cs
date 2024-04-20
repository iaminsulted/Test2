using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004B6 RID: 1206
	internal class BamlTypeInfoWithSerializerRecord : BamlTypeInfoRecord
	{
		// Token: 0x06003DBB RID: 15803 RVA: 0x001FD6B7 File Offset: 0x001FC6B7
		internal BamlTypeInfoWithSerializerRecord()
		{
			base.Pin();
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x001FD6C5 File Offset: 0x001FC6C5
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this.SerializerTypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x001FD6DA File Offset: 0x001FC6DA
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			bamlBinaryWriter.Write(this.SerializerTypeId);
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x001FD6EF File Offset: 0x001FC6EF
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlTypeInfoWithSerializerRecord bamlTypeInfoWithSerializerRecord = (BamlTypeInfoWithSerializerRecord)record;
			bamlTypeInfoWithSerializerRecord._serializerTypeId = this._serializerTypeId;
			bamlTypeInfoWithSerializerRecord._serializerType = this._serializerType;
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06003DBF RID: 15807 RVA: 0x001FD715 File Offset: 0x001FC715
		// (set) Token: 0x06003DC0 RID: 15808 RVA: 0x001FD71D File Offset: 0x001FC71D
		internal short SerializerTypeId
		{
			get
			{
				return this._serializerTypeId;
			}
			set
			{
				this._serializerTypeId = value;
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06003DC1 RID: 15809 RVA: 0x001FD726 File Offset: 0x001FC726
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TypeSerializerInfo;
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x001FD72A File Offset: 0x001FC72A
		// (set) Token: 0x06003DC3 RID: 15811 RVA: 0x001FD732 File Offset: 0x001FC732
		internal Type SerializerType
		{
			get
			{
				return this._serializerType;
			}
			set
			{
				this._serializerType = value;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool HasSerializer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001EEC RID: 7916
		private short _serializerTypeId;

		// Token: 0x04001EED RID: 7917
		private Type _serializerType;
	}
}
