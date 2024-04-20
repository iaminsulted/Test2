using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000485 RID: 1157
	internal abstract class BamlVariableSizedRecord : BamlRecord
	{
		// Token: 0x06003C1F RID: 15391 RVA: 0x001FB5FC File Offset: 0x001FA5FC
		internal override bool LoadRecordSize(BinaryReader bamlBinaryReader, long bytesAvailable)
		{
			int recordSize;
			bool flag = BamlVariableSizedRecord.LoadVariableRecordSize(bamlBinaryReader, bytesAvailable, out recordSize);
			if (flag)
			{
				this.RecordSize = recordSize;
			}
			return flag;
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x001FB61C File Offset: 0x001FA61C
		internal static bool LoadVariableRecordSize(BinaryReader bamlBinaryReader, long bytesAvailable, out int recordSize)
		{
			if (bytesAvailable >= 4L)
			{
				recordSize = ((BamlBinaryReader)bamlBinaryReader).Read7BitEncodedInt();
				return true;
			}
			recordSize = -1;
			return false;
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x001FB638 File Offset: 0x001FA638
		protected int ComputeSizeOfVariableLengthRecord(long start, long end)
		{
			int num = (int)(end - start);
			return BamlBinaryWriter.SizeOf7bitEncodedSize(BamlBinaryWriter.SizeOf7bitEncodedSize(num) + num) + num;
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x001FB65C File Offset: 0x001FA65C
		internal override void Write(BinaryWriter bamlBinaryWriter)
		{
			if (bamlBinaryWriter == null)
			{
				return;
			}
			bamlBinaryWriter.Write((byte)this.RecordType);
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			this.WriteRecordData(bamlBinaryWriter);
			long end = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			this.RecordSize = this.ComputeSizeOfVariableLengthRecord(num, end);
			bamlBinaryWriter.Seek((int)num, SeekOrigin.Begin);
			this.WriteRecordSize(bamlBinaryWriter);
			this.WriteRecordData(bamlBinaryWriter);
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x001FB6B8 File Offset: 0x001FA6B8
		internal void WriteRecordSize(BinaryWriter bamlBinaryWriter)
		{
			((BamlBinaryWriter)bamlBinaryWriter).Write7BitEncodedInt(this.RecordSize);
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x001FB6CB File Offset: 0x001FA6CB
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlVariableSizedRecord)record)._recordSize = this._recordSize;
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06003C25 RID: 15397 RVA: 0x001FB6E5 File Offset: 0x001FA6E5
		// (set) Token: 0x06003C26 RID: 15398 RVA: 0x001FB6ED File Offset: 0x001FA6ED
		internal override int RecordSize
		{
			get
			{
				return this._recordSize;
			}
			set
			{
				this._recordSize = value;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06003C27 RID: 15399 RVA: 0x001FB6F6 File Offset: 0x001FA6F6
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlRecord.LastFlagsSection;
			}
		}

		// Token: 0x04001E90 RID: 7824
		internal const int MaxRecordSizeFieldLength = 4;

		// Token: 0x04001E91 RID: 7825
		private int _recordSize = -1;
	}
}
