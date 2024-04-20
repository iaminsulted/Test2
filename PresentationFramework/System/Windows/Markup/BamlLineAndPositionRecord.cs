using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004BA RID: 1210
	internal class BamlLineAndPositionRecord : BamlRecord
	{
		// Token: 0x06003DFF RID: 15871 RVA: 0x001FDCCB File Offset: 0x001FCCCB
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.LineNumber = (uint)bamlBinaryReader.ReadInt32();
			this.LinePosition = (uint)bamlBinaryReader.ReadInt32();
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x001FDCE5 File Offset: 0x001FCCE5
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.LineNumber);
			bamlBinaryWriter.Write(this.LinePosition);
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x001FDCFF File Offset: 0x001FCCFF
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlLineAndPositionRecord bamlLineAndPositionRecord = (BamlLineAndPositionRecord)record;
			bamlLineAndPositionRecord._lineNumber = this._lineNumber;
			bamlLineAndPositionRecord._linePosition = this._linePosition;
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06003E02 RID: 15874 RVA: 0x001FDD25 File Offset: 0x001FCD25
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.LineNumberAndPosition;
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06003E03 RID: 15875 RVA: 0x001FDD29 File Offset: 0x001FCD29
		// (set) Token: 0x06003E04 RID: 15876 RVA: 0x001FDD31 File Offset: 0x001FCD31
		internal uint LineNumber
		{
			get
			{
				return this._lineNumber;
			}
			set
			{
				this._lineNumber = value;
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06003E05 RID: 15877 RVA: 0x001FDD3A File Offset: 0x001FCD3A
		// (set) Token: 0x06003E06 RID: 15878 RVA: 0x001FDD42 File Offset: 0x001FCD42
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

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06003E07 RID: 15879 RVA: 0x001390BC File Offset: 0x001380BC
		internal override int RecordSize
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x001FDD4B File Offset: 0x001FCD4B
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} LineNum={1} Pos={2}", this.RecordType, this.LineNumber, this.LinePosition);
		}

		// Token: 0x04001EFF RID: 7935
		private uint _lineNumber;

		// Token: 0x04001F00 RID: 7936
		private uint _linePosition;
	}
}
