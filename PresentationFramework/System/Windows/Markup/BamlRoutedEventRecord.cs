using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004A1 RID: 1185
	internal class BamlRoutedEventRecord : BamlStringValueRecord
	{
		// Token: 0x06003D14 RID: 15636 RVA: 0x001FCAA5 File Offset: 0x001FBAA5
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			base.Value = bamlBinaryReader.ReadString();
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x001FCABF File Offset: 0x001FBABF
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			bamlBinaryWriter.Write(base.Value);
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x001FCAD9 File Offset: 0x001FBAD9
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlRoutedEventRecord)record)._attributeId = this._attributeId;
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06003D17 RID: 15639 RVA: 0x001FCAF3 File Offset: 0x001FBAF3
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.RoutedEvent;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x001FCAF7 File Offset: 0x001FBAF7
		// (set) Token: 0x06003D19 RID: 15641 RVA: 0x001FCAFF File Offset: 0x001FBAFF
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

		// Token: 0x04001EC9 RID: 7881
		private short _attributeId = -1;
	}
}
