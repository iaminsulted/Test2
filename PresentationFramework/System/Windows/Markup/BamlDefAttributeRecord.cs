using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200048D RID: 1165
	internal class BamlDefAttributeRecord : BamlStringValueRecord
	{
		// Token: 0x06003C82 RID: 15490 RVA: 0x001FBD81 File Offset: 0x001FAD81
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.Value = bamlBinaryReader.ReadString();
			this.NameId = bamlBinaryReader.ReadInt16();
			this.Name = null;
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x001FBDA2 File Offset: 0x001FADA2
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.Value);
			bamlBinaryWriter.Write(this.NameId);
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x001FBDBC File Offset: 0x001FADBC
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDefAttributeRecord bamlDefAttributeRecord = (BamlDefAttributeRecord)record;
			bamlDefAttributeRecord._name = this._name;
			bamlDefAttributeRecord._nameId = this._nameId;
			bamlDefAttributeRecord._attributeUsage = this._attributeUsage;
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06003C85 RID: 15493 RVA: 0x001FBDEE File Offset: 0x001FADEE
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DefAttribute;
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06003C86 RID: 15494 RVA: 0x001FBDF2 File Offset: 0x001FADF2
		// (set) Token: 0x06003C87 RID: 15495 RVA: 0x001FBDFA File Offset: 0x001FADFA
		internal short NameId
		{
			get
			{
				return this._nameId;
			}
			set
			{
				this._nameId = value;
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06003C88 RID: 15496 RVA: 0x001FBE03 File Offset: 0x001FAE03
		// (set) Token: 0x06003C89 RID: 15497 RVA: 0x001FBE0B File Offset: 0x001FAE0B
		internal string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06003C8A RID: 15498 RVA: 0x001FBE14 File Offset: 0x001FAE14
		// (set) Token: 0x06003C8B RID: 15499 RVA: 0x001FBE1C File Offset: 0x001FAE1C
		internal BamlAttributeUsage AttributeUsage
		{
			get
			{
				return this._attributeUsage;
			}
			set
			{
				this._attributeUsage = value;
			}
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x001FBE28 File Offset: 0x001FAE28
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} nameId({1}) is '{2}' usage={3}", new object[]
			{
				this.RecordType,
				this.NameId,
				this.Name,
				this.AttributeUsage
			});
		}

		// Token: 0x04001EA9 RID: 7849
		private string _name;

		// Token: 0x04001EAA RID: 7850
		private short _nameId;

		// Token: 0x04001EAB RID: 7851
		private BamlAttributeUsage _attributeUsage;
	}
}
