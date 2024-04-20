using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200048E RID: 1166
	internal class BamlPresentationOptionsAttributeRecord : BamlStringValueRecord
	{
		// Token: 0x06003C8E RID: 15502 RVA: 0x001FBE85 File Offset: 0x001FAE85
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.Value = bamlBinaryReader.ReadString();
			this.NameId = bamlBinaryReader.ReadInt16();
			this.Name = null;
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x001FBEA6 File Offset: 0x001FAEA6
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.Value);
			bamlBinaryWriter.Write(this.NameId);
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x001FBEC0 File Offset: 0x001FAEC0
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPresentationOptionsAttributeRecord bamlPresentationOptionsAttributeRecord = (BamlPresentationOptionsAttributeRecord)record;
			bamlPresentationOptionsAttributeRecord._name = this._name;
			bamlPresentationOptionsAttributeRecord._nameId = this._nameId;
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06003C91 RID: 15505 RVA: 0x001FBEE6 File Offset: 0x001FAEE6
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PresentationOptionsAttribute;
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06003C92 RID: 15506 RVA: 0x001FBEEA File Offset: 0x001FAEEA
		// (set) Token: 0x06003C93 RID: 15507 RVA: 0x001FBEF2 File Offset: 0x001FAEF2
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

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06003C94 RID: 15508 RVA: 0x001FBEFB File Offset: 0x001FAEFB
		// (set) Token: 0x06003C95 RID: 15509 RVA: 0x001FBF03 File Offset: 0x001FAF03
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

		// Token: 0x06003C96 RID: 15510 RVA: 0x001FBF0C File Offset: 0x001FAF0C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} nameId({1}) is '{2}' ", this.RecordType, this.NameId, this.Name);
		}

		// Token: 0x04001EAC RID: 7852
		private string _name;

		// Token: 0x04001EAD RID: 7853
		private short _nameId;
	}
}
