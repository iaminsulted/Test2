using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004B8 RID: 1208
	internal class BamlStringInfoRecord : BamlVariableSizedRecord
	{
		// Token: 0x06003DEA RID: 15850 RVA: 0x001FDB43 File Offset: 0x001FCB43
		internal BamlStringInfoRecord()
		{
			base.Pin();
			this.StringId = -1;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x001FDB58 File Offset: 0x001FCB58
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.StringId = bamlBinaryReader.ReadInt16();
			this.Value = bamlBinaryReader.ReadString();
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x001FDB72 File Offset: 0x001FCB72
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.StringId);
			bamlBinaryWriter.Write(this.Value);
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x001FDB8C File Offset: 0x001FCB8C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlStringInfoRecord)record)._value = this._value;
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06003DEE RID: 15854 RVA: 0x001FDBA6 File Offset: 0x001FCBA6
		// (set) Token: 0x06003DEF RID: 15855 RVA: 0x001FDBCE File Offset: 0x001FCBCE
		internal short StringId
		{
			get
			{
				return (short)this._flags[BamlStringInfoRecord._stringIdLowSection] | (short)(this._flags[BamlStringInfoRecord._stringIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlStringInfoRecord._stringIdLowSection] = (int)(value & 255);
				this._flags[BamlStringInfoRecord._stringIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06003DF0 RID: 15856 RVA: 0x001FDC02 File Offset: 0x001FCC02
		// (set) Token: 0x06003DF1 RID: 15857 RVA: 0x001FDC0A File Offset: 0x001FCC0A
		internal string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x0013CE2F File Offset: 0x0013BE2F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StringInfo;
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06003DF3 RID: 15859 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool HasSerializer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x001FDC13 File Offset: 0x001FCC13
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} stringId({1}='{2}'", this.RecordType, this.StringId, this._value);
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x001FDC40 File Offset: 0x001FCC40
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlStringInfoRecord._stringIdHighSection;
			}
		}

		// Token: 0x04001EFB RID: 7931
		private static BitVector32.Section _stringIdLowSection = BitVector32.CreateSection(255, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001EFC RID: 7932
		private static BitVector32.Section _stringIdHighSection = BitVector32.CreateSection(255, BamlStringInfoRecord._stringIdLowSection);

		// Token: 0x04001EFD RID: 7933
		private string _value;
	}
}
