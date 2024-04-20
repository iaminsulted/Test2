using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200048F RID: 1167
	internal class BamlPropertyComplexStartRecord : BamlRecord
	{
		// Token: 0x06003C98 RID: 15512 RVA: 0x001FBF39 File Offset: 0x001FAF39
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x001FBF47 File Offset: 0x001FAF47
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x001FBF55 File Offset: 0x001FAF55
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlPropertyComplexStartRecord)record)._attributeId = this._attributeId;
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06003C9B RID: 15515 RVA: 0x0017DBED File Offset: 0x0017CBED
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyComplexStart;
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06003C9C RID: 15516 RVA: 0x001FBF6F File Offset: 0x001FAF6F
		// (set) Token: 0x06003C9D RID: 15517 RVA: 0x001FBF77 File Offset: 0x001FAF77
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

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06003C9E RID: 15518 RVA: 0x0010A7E1 File Offset: 0x001097E1
		// (set) Token: 0x06003C9F RID: 15519 RVA: 0x000F6B2C File Offset: 0x000F5B2C
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

		// Token: 0x06003CA0 RID: 15520 RVA: 0x001FBF80 File Offset: 0x001FAF80
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} attr({1})", this.RecordType, this._attributeId);
		}

		// Token: 0x04001EAE RID: 7854
		private short _attributeId = -1;
	}
}
