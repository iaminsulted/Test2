using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004BB RID: 1211
	internal class BamlLinePositionRecord : BamlRecord
	{
		// Token: 0x06003E0A RID: 15882 RVA: 0x001FDD7D File Offset: 0x001FCD7D
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.LinePosition = (uint)bamlBinaryReader.ReadInt32();
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x001FDD8B File Offset: 0x001FCD8B
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.LinePosition);
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x001FDD99 File Offset: 0x001FCD99
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlLinePositionRecord)record)._linePosition = this._linePosition;
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06003E0D RID: 15885 RVA: 0x001FDDB3 File Offset: 0x001FCDB3
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.LinePosition;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06003E0E RID: 15886 RVA: 0x001FDDB7 File Offset: 0x001FCDB7
		// (set) Token: 0x06003E0F RID: 15887 RVA: 0x001FDDBF File Offset: 0x001FCDBF
		internal uint LinePosition
		{
			get
			{
				return this._linePosition;
			}
			set
			{
				this._linePosition = value;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06003E10 RID: 15888 RVA: 0x001FC019 File Offset: 0x001FB019
		internal override int RecordSize
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x001FDDC8 File Offset: 0x001FCDC8
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} LinePos={1}", this.RecordType, this.LinePosition);
		}

		// Token: 0x04001F01 RID: 7937
		private uint _linePosition;
	}
}
