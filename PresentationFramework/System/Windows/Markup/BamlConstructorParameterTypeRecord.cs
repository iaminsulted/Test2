using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200049A RID: 1178
	internal class BamlConstructorParameterTypeRecord : BamlRecord
	{
		// Token: 0x06003CFF RID: 15615 RVA: 0x001FCA4A File Offset: 0x001FBA4A
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.TypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x001FCA58 File Offset: 0x001FBA58
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.TypeId);
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x001FCA66 File Offset: 0x001FBA66
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlConstructorParameterTypeRecord)record)._typeId = this._typeId;
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06003D02 RID: 15618 RVA: 0x001FCA80 File Offset: 0x001FBA80
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ConstructorParameterType;
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06003D03 RID: 15619 RVA: 0x001FCA84 File Offset: 0x001FBA84
		// (set) Token: 0x06003D04 RID: 15620 RVA: 0x001FCA8C File Offset: 0x001FBA8C
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

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06003D05 RID: 15621 RVA: 0x0010A7E1 File Offset: 0x001097E1
		// (set) Token: 0x06003D06 RID: 15622 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override int RecordSize
		{
			get
			{
				return 2;
			}
			set
			{
			}
		}

		// Token: 0x04001EC8 RID: 7880
		private short _typeId;
	}
}
