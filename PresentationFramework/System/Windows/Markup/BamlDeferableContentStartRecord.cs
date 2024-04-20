using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004A6 RID: 1190
	internal class BamlDeferableContentStartRecord : BamlRecord
	{
		// Token: 0x06003D3F RID: 15679 RVA: 0x001FCDF1 File Offset: 0x001FBDF1
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ContentSize = bamlBinaryReader.ReadInt32();
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x001FCDFF File Offset: 0x001FBDFF
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			this._contentSizePosition = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			bamlBinaryWriter.Write(this.ContentSize);
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x001FCE1C File Offset: 0x001FBE1C
		internal void UpdateContentSize(int contentSize, BinaryWriter bamlBinaryWriter)
		{
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			int num2 = (int)(this._contentSizePosition - num);
			bamlBinaryWriter.Seek(num2, SeekOrigin.Current);
			bamlBinaryWriter.Write(contentSize);
			bamlBinaryWriter.Seek((int)(-4L - (long)num2), SeekOrigin.Current);
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x001FCE5B File Offset: 0x001FBE5B
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDeferableContentStartRecord bamlDeferableContentStartRecord = (BamlDeferableContentStartRecord)record;
			bamlDeferableContentStartRecord._contentSize = this._contentSize;
			bamlDeferableContentStartRecord._contentSizePosition = this._contentSizePosition;
			bamlDeferableContentStartRecord._valuesBuffer = this._valuesBuffer;
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06003D43 RID: 15683 RVA: 0x001FCE8D File Offset: 0x001FBE8D
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DeferableContentStart;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06003D44 RID: 15684 RVA: 0x001FCE91 File Offset: 0x001FBE91
		// (set) Token: 0x06003D45 RID: 15685 RVA: 0x001FCE99 File Offset: 0x001FBE99
		internal int ContentSize
		{
			get
			{
				return this._contentSize;
			}
			set
			{
				this._contentSize = value;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06003D46 RID: 15686 RVA: 0x001FC019 File Offset: 0x001FB019
		// (set) Token: 0x06003D47 RID: 15687 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override int RecordSize
		{
			get
			{
				return 4;
			}
			set
			{
			}
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06003D48 RID: 15688 RVA: 0x001FCEA2 File Offset: 0x001FBEA2
		// (set) Token: 0x06003D49 RID: 15689 RVA: 0x001FCEAA File Offset: 0x001FBEAA
		internal byte[] ValuesBuffer
		{
			get
			{
				return this._valuesBuffer;
			}
			set
			{
				this._valuesBuffer = value;
			}
		}

		// Token: 0x04001ED1 RID: 7889
		private const long ContentSizeSize = 4L;

		// Token: 0x04001ED2 RID: 7890
		private int _contentSize = -1;

		// Token: 0x04001ED3 RID: 7891
		private long _contentSizePosition = -1L;

		// Token: 0x04001ED4 RID: 7892
		private byte[] _valuesBuffer;
	}
}
