using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000487 RID: 1159
	internal class BamlPIMappingRecord : BamlVariableSizedRecord
	{
		// Token: 0x06003C34 RID: 15412 RVA: 0x001FB845 File Offset: 0x001FA845
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.XmlNamespace = bamlBinaryReader.ReadString();
			this.ClrNamespace = bamlBinaryReader.ReadString();
			this.AssemblyId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x001FB86B File Offset: 0x001FA86B
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.XmlNamespace);
			bamlBinaryWriter.Write(this.ClrNamespace);
			bamlBinaryWriter.Write(this.AssemblyId);
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x001FB891 File Offset: 0x001FA891
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPIMappingRecord bamlPIMappingRecord = (BamlPIMappingRecord)record;
			bamlPIMappingRecord._xmlns = this._xmlns;
			bamlPIMappingRecord._clrns = this._clrns;
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06003C37 RID: 15415 RVA: 0x001FB8B7 File Offset: 0x001FA8B7
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PIMapping;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x001FB8BB File Offset: 0x001FA8BB
		// (set) Token: 0x06003C39 RID: 15417 RVA: 0x001FB8C3 File Offset: 0x001FA8C3
		internal string XmlNamespace
		{
			get
			{
				return this._xmlns;
			}
			set
			{
				this._xmlns = value;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06003C3A RID: 15418 RVA: 0x001FB8CC File Offset: 0x001FA8CC
		// (set) Token: 0x06003C3B RID: 15419 RVA: 0x001FB8D4 File Offset: 0x001FA8D4
		internal string ClrNamespace
		{
			get
			{
				return this._clrns;
			}
			set
			{
				this._clrns = value;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06003C3C RID: 15420 RVA: 0x001FB8DD File Offset: 0x001FA8DD
		// (set) Token: 0x06003C3D RID: 15421 RVA: 0x001FB905 File Offset: 0x001FA905
		internal short AssemblyId
		{
			get
			{
				return (short)this._flags[BamlPIMappingRecord._assemblyIdLowSection] | (short)(this._flags[BamlPIMappingRecord._assemblyIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlPIMappingRecord._assemblyIdLowSection] = (int)(value & 255);
				this._flags[BamlPIMappingRecord._assemblyIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06003C3E RID: 15422 RVA: 0x001FB939 File Offset: 0x001FA939
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlPIMappingRecord._assemblyIdHighSection;
			}
		}

		// Token: 0x04001E95 RID: 7829
		private static BitVector32.Section _assemblyIdLowSection = BitVector32.CreateSection(255, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001E96 RID: 7830
		private static BitVector32.Section _assemblyIdHighSection = BitVector32.CreateSection(255, BamlPIMappingRecord._assemblyIdLowSection);

		// Token: 0x04001E97 RID: 7831
		private string _xmlns;

		// Token: 0x04001E98 RID: 7832
		private string _clrns;
	}
}
