using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000493 RID: 1171
	internal class BamlPropertyRecord : BamlStringValueRecord
	{
		// Token: 0x06003CBB RID: 15547 RVA: 0x001FC0E8 File Offset: 0x001FB0E8
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			base.Value = bamlBinaryReader.ReadString();
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x001FC102 File Offset: 0x001FB102
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			bamlBinaryWriter.Write(base.Value);
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x001FC11C File Offset: 0x001FB11C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlPropertyRecord)record)._attributeId = this._attributeId;
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06003CBE RID: 15550 RVA: 0x001FC136 File Offset: 0x001FB136
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.Property;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06003CBF RID: 15551 RVA: 0x001FC139 File Offset: 0x001FB139
		// (set) Token: 0x06003CC0 RID: 15552 RVA: 0x001FC141 File Offset: 0x001FB141
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

		// Token: 0x06003CC1 RID: 15553 RVA: 0x001FC14A File Offset: 0x001FB14A
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} attr({1}) <== '{2}'", this.RecordType, this._attributeId, base.Value);
		}

		// Token: 0x04001EB2 RID: 7858
		private short _attributeId = -1;
	}
}
