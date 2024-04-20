using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004A4 RID: 1188
	internal class BamlElementStartRecord : BamlRecord
	{
		// Token: 0x06003D28 RID: 15656 RVA: 0x001FCBB0 File Offset: 0x001FBBB0
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.TypeId = bamlBinaryReader.ReadInt16();
			byte b = bamlBinaryReader.ReadByte();
			this.CreateUsingTypeConverter = ((b & 1) > 0);
			this.IsInjected = ((b & 2) > 0);
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x001FCBE8 File Offset: 0x001FBBE8
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.TypeId);
			byte value = (byte)((this.CreateUsingTypeConverter ? 1 : 0) | (this.IsInjected ? 2 : 0));
			bamlBinaryWriter.Write(value);
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06003D2A RID: 15658 RVA: 0x001E977A File Offset: 0x001E877A
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ElementStart;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06003D2B RID: 15659 RVA: 0x001FCC23 File Offset: 0x001FBC23
		// (set) Token: 0x06003D2C RID: 15660 RVA: 0x001FCC4B File Offset: 0x001FBC4B
		internal short TypeId
		{
			get
			{
				return (short)this._flags[BamlElementStartRecord._typeIdLowSection] | (short)(this._flags[BamlElementStartRecord._typeIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlElementStartRecord._typeIdLowSection] = (int)(value & 255);
				this._flags[BamlElementStartRecord._typeIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06003D2D RID: 15661 RVA: 0x001FCC7F File Offset: 0x001FBC7F
		// (set) Token: 0x06003D2E RID: 15662 RVA: 0x001FCC97 File Offset: 0x001FBC97
		internal bool CreateUsingTypeConverter
		{
			get
			{
				return this._flags[BamlElementStartRecord._useTypeConverter] == 1;
			}
			set
			{
				this._flags[BamlElementStartRecord._useTypeConverter] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06003D2F RID: 15663 RVA: 0x001FCCB0 File Offset: 0x001FBCB0
		// (set) Token: 0x06003D30 RID: 15664 RVA: 0x001FCCC8 File Offset: 0x001FBCC8
		internal bool IsInjected
		{
			get
			{
				return this._flags[BamlElementStartRecord._isInjected] == 1;
			}
			set
			{
				this._flags[BamlElementStartRecord._isInjected] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06003D31 RID: 15665 RVA: 0x001E977A File Offset: 0x001E877A
		// (set) Token: 0x06003D32 RID: 15666 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override int RecordSize
		{
			get
			{
				return 3;
			}
			set
			{
			}
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x001FCCE1 File Offset: 0x001FBCE1
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} typeId={1}", this.RecordType, BamlRecord.GetTypeName((int)this.TypeId));
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06003D34 RID: 15668 RVA: 0x001FCD08 File Offset: 0x001FBD08
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlElementStartRecord._isInjected;
			}
		}

		// Token: 0x04001ECB RID: 7883
		private static BitVector32.Section _typeIdLowSection = BitVector32.CreateSection(255, BamlRecord.LastFlagsSection);

		// Token: 0x04001ECC RID: 7884
		private static BitVector32.Section _typeIdHighSection = BitVector32.CreateSection(255, BamlElementStartRecord._typeIdLowSection);

		// Token: 0x04001ECD RID: 7885
		private static BitVector32.Section _useTypeConverter = BitVector32.CreateSection(1, BamlElementStartRecord._typeIdHighSection);

		// Token: 0x04001ECE RID: 7886
		private static BitVector32.Section _isInjected = BitVector32.CreateSection(1, BamlElementStartRecord._useTypeConverter);
	}
}
