using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000494 RID: 1172
	internal class BamlPropertyWithExtensionRecord : BamlRecord, IOptimizedMarkupExtension
	{
		// Token: 0x06003CC3 RID: 15555 RVA: 0x001FC188 File Offset: 0x001FB188
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			short num = bamlBinaryReader.ReadInt16();
			this.ValueId = bamlBinaryReader.ReadInt16();
			this._extensionTypeId = (num & BamlPropertyWithExtensionRecord.ExtensionIdMask);
			this.IsValueTypeExtension = ((num & BamlPropertyWithExtensionRecord.TypeExtensionValueMask) == BamlPropertyWithExtensionRecord.TypeExtensionValueMask);
			this.IsValueStaticExtension = ((num & BamlPropertyWithExtensionRecord.StaticExtensionValueMask) == BamlPropertyWithExtensionRecord.StaticExtensionValueMask);
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x001FC1EC File Offset: 0x001FB1EC
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			short num = this.ExtensionTypeId;
			if (this.IsValueTypeExtension)
			{
				num |= BamlPropertyWithExtensionRecord.TypeExtensionValueMask;
			}
			else if (this.IsValueStaticExtension)
			{
				num |= BamlPropertyWithExtensionRecord.StaticExtensionValueMask;
			}
			bamlBinaryWriter.Write(num);
			bamlBinaryWriter.Write(this.ValueId);
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x001FC243 File Offset: 0x001FB243
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyWithExtensionRecord bamlPropertyWithExtensionRecord = (BamlPropertyWithExtensionRecord)record;
			bamlPropertyWithExtensionRecord._attributeId = this._attributeId;
			bamlPropertyWithExtensionRecord._extensionTypeId = this._extensionTypeId;
			bamlPropertyWithExtensionRecord._valueId = this._valueId;
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x001FC275 File Offset: 0x001FB275
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyWithExtension;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x001FC279 File Offset: 0x001FB279
		// (set) Token: 0x06003CC8 RID: 15560 RVA: 0x001FC281 File Offset: 0x001FB281
		internal short AttributeId
		{
			get
			{
				return this._attributeId;
			}
			set
			{
				this._attributeId = value;
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06003CC9 RID: 15561 RVA: 0x001FC28A File Offset: 0x001FB28A
		// (set) Token: 0x06003CCA RID: 15562 RVA: 0x001FC292 File Offset: 0x001FB292
		public short ExtensionTypeId
		{
			get
			{
				return this._extensionTypeId;
			}
			set
			{
				this._extensionTypeId = value;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x001FC29B File Offset: 0x001FB29B
		// (set) Token: 0x06003CCC RID: 15564 RVA: 0x001FC2A3 File Offset: 0x001FB2A3
		public short ValueId
		{
			get
			{
				return this._valueId;
			}
			set
			{
				this._valueId = value;
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06003CCD RID: 15565 RVA: 0x001FC2AC File Offset: 0x001FB2AC
		// (set) Token: 0x06003CCE RID: 15566 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override int RecordSize
		{
			get
			{
				return 6;
			}
			set
			{
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06003CCF RID: 15567 RVA: 0x001FC2AF File Offset: 0x001FB2AF
		// (set) Token: 0x06003CD0 RID: 15568 RVA: 0x001FC2C7 File Offset: 0x001FB2C7
		public bool IsValueTypeExtension
		{
			get
			{
				return this._flags[BamlPropertyWithExtensionRecord._isValueTypeExtensionSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyWithExtensionRecord._isValueTypeExtensionSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x001FC2E0 File Offset: 0x001FB2E0
		// (set) Token: 0x06003CD2 RID: 15570 RVA: 0x001FC2F8 File Offset: 0x001FB2F8
		public bool IsValueStaticExtension
		{
			get
			{
				return this._flags[BamlPropertyWithExtensionRecord._isValueStaticExtensionSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyWithExtensionRecord._isValueStaticExtensionSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06003CD3 RID: 15571 RVA: 0x001FC311 File Offset: 0x001FB311
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlPropertyWithExtensionRecord._isValueStaticExtensionSection;
			}
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x001FC318 File Offset: 0x001FB318
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} attr({1}) extn({2}) valueId({3})", new object[]
			{
				this.RecordType,
				this._attributeId,
				this._extensionTypeId,
				this._valueId
			});
		}

		// Token: 0x04001EB3 RID: 7859
		private static BitVector32.Section _isValueTypeExtensionSection = BitVector32.CreateSection(1, BamlRecord.LastFlagsSection);

		// Token: 0x04001EB4 RID: 7860
		private static BitVector32.Section _isValueStaticExtensionSection = BitVector32.CreateSection(1, BamlPropertyWithExtensionRecord._isValueTypeExtensionSection);

		// Token: 0x04001EB5 RID: 7861
		private short _attributeId = -1;

		// Token: 0x04001EB6 RID: 7862
		private short _extensionTypeId;

		// Token: 0x04001EB7 RID: 7863
		private short _valueId;

		// Token: 0x04001EB8 RID: 7864
		private static readonly short ExtensionIdMask = 4095;

		// Token: 0x04001EB9 RID: 7865
		private static readonly short TypeExtensionValueMask = 16384;

		// Token: 0x04001EBA RID: 7866
		private static readonly short StaticExtensionValueMask = 8192;
	}
}
