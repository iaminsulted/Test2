using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004AA RID: 1194
	internal class BamlStaticResourceIdRecord : BamlRecord
	{
		// Token: 0x06003D60 RID: 15712 RVA: 0x001FD056 File Offset: 0x001FC056
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.StaticResourceId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x001FD064 File Offset: 0x001FC064
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.StaticResourceId);
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x001FD072 File Offset: 0x001FC072
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlStaticResourceIdRecord)record)._staticResourceId = this._staticResourceId;
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06003D63 RID: 15715 RVA: 0x001FD08C File Offset: 0x001FC08C
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StaticResourceId;
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06003D64 RID: 15716 RVA: 0x0010A7E1 File Offset: 0x001097E1
		// (set) Token: 0x06003D65 RID: 15717 RVA: 0x000F6B2C File Offset: 0x000F5B2C
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

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06003D66 RID: 15718 RVA: 0x001FD090 File Offset: 0x001FC090
		// (set) Token: 0x06003D67 RID: 15719 RVA: 0x001FD098 File Offset: 0x001FC098
		internal short StaticResourceId
		{
			get
			{
				return this._staticResourceId;
			}
			set
			{
				this._staticResourceId = value;
			}
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x001FD0A1 File Offset: 0x001FC0A1
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} staticResourceId({1})", this.RecordType, this.StaticResourceId);
		}

		// Token: 0x04001EDA RID: 7898
		private short _staticResourceId = -1;
	}
}
