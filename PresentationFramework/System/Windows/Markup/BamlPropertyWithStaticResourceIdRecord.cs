using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004AB RID: 1195
	internal class BamlPropertyWithStaticResourceIdRecord : BamlStaticResourceIdRecord
	{
		// Token: 0x06003D6A RID: 15722 RVA: 0x001FD0D7 File Offset: 0x001FC0D7
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			base.StaticResourceId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x001FD0F1 File Offset: 0x001FC0F1
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			bamlBinaryWriter.Write(base.StaticResourceId);
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x001FD10B File Offset: 0x001FC10B
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlPropertyWithStaticResourceIdRecord)record)._attributeId = this._attributeId;
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06003D6D RID: 15725 RVA: 0x001FD125 File Offset: 0x001FC125
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyWithStaticResourceId;
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06003D6E RID: 15726 RVA: 0x001FC019 File Offset: 0x001FB019
		// (set) Token: 0x06003D6F RID: 15727 RVA: 0x000F6B2C File Offset: 0x000F5B2C
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

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06003D70 RID: 15728 RVA: 0x001FD129 File Offset: 0x001FC129
		// (set) Token: 0x06003D71 RID: 15729 RVA: 0x001FD131 File Offset: 0x001FC131
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

		// Token: 0x06003D72 RID: 15730 RVA: 0x001FD13A File Offset: 0x001FC13A
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} attr({1}) staticResourceId({2})", this.RecordType, this.AttributeId, base.StaticResourceId);
		}

		// Token: 0x04001EDB RID: 7899
		private short _attributeId = -1;
	}
}
