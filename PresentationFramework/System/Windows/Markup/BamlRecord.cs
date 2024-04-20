using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000484 RID: 1156
	internal abstract class BamlRecord
	{
		// Token: 0x06003C0A RID: 15370 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal virtual bool LoadRecordSize(BinaryReader bamlBinaryReader, long bytesAvailable)
		{
			return true;
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void LoadRecordData(BinaryReader bamlBinaryReader)
		{
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x001FB4B9 File Offset: 0x001FA4B9
		internal virtual void Write(BinaryWriter bamlBinaryWriter)
		{
			if (bamlBinaryWriter == null)
			{
				return;
			}
			bamlBinaryWriter.Write((byte)this.RecordType);
			this.WriteRecordData(bamlBinaryWriter);
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06003C0E RID: 15374 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06003C0F RID: 15375 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual int RecordSize
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06003C10 RID: 15376 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.Unknown;
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06003C11 RID: 15377 RVA: 0x001FB4D2 File Offset: 0x001FA4D2
		// (set) Token: 0x06003C12 RID: 15378 RVA: 0x001FB4DA File Offset: 0x001FA4DA
		internal BamlRecord Next
		{
			get
			{
				return this._nextRecord;
			}
			set
			{
				this._nextRecord = value;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003C13 RID: 15379 RVA: 0x001FB4E3 File Offset: 0x001FA4E3
		internal bool IsPinned
		{
			get
			{
				return this.PinnedCount > 0;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06003C14 RID: 15380 RVA: 0x001FB4EE File Offset: 0x001FA4EE
		// (set) Token: 0x06003C15 RID: 15381 RVA: 0x001FB500 File Offset: 0x001FA500
		internal int PinnedCount
		{
			get
			{
				return this._flags[BamlRecord._pinnedFlagSection];
			}
			set
			{
				this._flags[BamlRecord._pinnedFlagSection] = value;
			}
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x001FB514 File Offset: 0x001FA514
		internal void Pin()
		{
			if (this.PinnedCount < 3)
			{
				int pinnedCount = this.PinnedCount + 1;
				this.PinnedCount = pinnedCount;
			}
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x001FB53C File Offset: 0x001FA53C
		internal void Unpin()
		{
			if (this.PinnedCount < 3)
			{
				int pinnedCount = this.PinnedCount - 1;
				this.PinnedCount = pinnedCount;
			}
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x001FB562 File Offset: 0x001FA562
		internal virtual void Copy(BamlRecord record)
		{
			record._flags = this._flags;
			record._nextRecord = this._nextRecord;
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06003C19 RID: 15385 RVA: 0x001FB57C File Offset: 0x001FA57C
		internal static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlRecord._pinnedFlagSection;
			}
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x001FB583 File Offset: 0x001FA583
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", this.RecordType);
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x001FB5A0 File Offset: 0x001FA5A0
		protected static string GetTypeName(int typeId)
		{
			string result = typeId.ToString(CultureInfo.InvariantCulture);
			if (typeId < 0)
			{
				result = ((KnownElements)(-(KnownElements)typeId)).ToString();
			}
			return result;
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x001FB5D1 File Offset: 0x001FA5D1
		internal static bool IsContentRecord(BamlRecordType bamlRecordType)
		{
			return bamlRecordType == BamlRecordType.PropertyComplexStart || bamlRecordType == BamlRecordType.PropertyArrayStart || bamlRecordType == BamlRecordType.PropertyIListStart || bamlRecordType == BamlRecordType.PropertyIDictionaryStart || bamlRecordType == BamlRecordType.Text;
		}

		// Token: 0x04001E8C RID: 7820
		internal BitVector32 _flags;

		// Token: 0x04001E8D RID: 7821
		private static BitVector32.Section _pinnedFlagSection = BitVector32.CreateSection(3);

		// Token: 0x04001E8E RID: 7822
		private BamlRecord _nextRecord;

		// Token: 0x04001E8F RID: 7823
		internal const int RecordTypeFieldLength = 1;
	}
}
