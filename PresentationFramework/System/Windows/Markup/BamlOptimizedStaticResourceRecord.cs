using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004A9 RID: 1193
	internal class BamlOptimizedStaticResourceRecord : BamlRecord, IOptimizedMarkupExtension
	{
		// Token: 0x06003D4F RID: 15695 RVA: 0x001FCEDC File Offset: 0x001FBEDC
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			byte b = bamlBinaryReader.ReadByte();
			this.ValueId = bamlBinaryReader.ReadInt16();
			this.IsValueTypeExtension = ((b & BamlOptimizedStaticResourceRecord.TypeExtensionValueMask) > 0);
			this.IsValueStaticExtension = ((b & BamlOptimizedStaticResourceRecord.StaticExtensionValueMask) > 0);
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x001FCF1C File Offset: 0x001FBF1C
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			byte b = 0;
			if (this.IsValueTypeExtension)
			{
				b |= BamlOptimizedStaticResourceRecord.TypeExtensionValueMask;
			}
			else if (this.IsValueStaticExtension)
			{
				b |= BamlOptimizedStaticResourceRecord.StaticExtensionValueMask;
			}
			bamlBinaryWriter.Write(b);
			bamlBinaryWriter.Write(this.ValueId);
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x001FCF62 File Offset: 0x001FBF62
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlOptimizedStaticResourceRecord)record)._valueId = this._valueId;
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06003D52 RID: 15698 RVA: 0x001FCF7C File Offset: 0x001FBF7C
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.OptimizedStaticResource;
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06003D53 RID: 15699 RVA: 0x001FCF80 File Offset: 0x001FBF80
		public short ExtensionTypeId
		{
			get
			{
				return 603;
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06003D54 RID: 15700 RVA: 0x001FCF87 File Offset: 0x001FBF87
		// (set) Token: 0x06003D55 RID: 15701 RVA: 0x001FCF8F File Offset: 0x001FBF8F
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

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06003D56 RID: 15702 RVA: 0x001E977A File Offset: 0x001E877A
		// (set) Token: 0x06003D57 RID: 15703 RVA: 0x000F6B2C File Offset: 0x000F5B2C
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

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06003D58 RID: 15704 RVA: 0x001FCF98 File Offset: 0x001FBF98
		// (set) Token: 0x06003D59 RID: 15705 RVA: 0x001FCFB0 File Offset: 0x001FBFB0
		public bool IsValueTypeExtension
		{
			get
			{
				return this._flags[BamlOptimizedStaticResourceRecord._isValueTypeExtensionSection] == 1;
			}
			set
			{
				this._flags[BamlOptimizedStaticResourceRecord._isValueTypeExtensionSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06003D5A RID: 15706 RVA: 0x001FCFC9 File Offset: 0x001FBFC9
		// (set) Token: 0x06003D5B RID: 15707 RVA: 0x001FCFE1 File Offset: 0x001FBFE1
		public bool IsValueStaticExtension
		{
			get
			{
				return this._flags[BamlOptimizedStaticResourceRecord._isValueStaticExtensionSection] == 1;
			}
			set
			{
				this._flags[BamlOptimizedStaticResourceRecord._isValueStaticExtensionSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06003D5C RID: 15708 RVA: 0x001FCFFA File Offset: 0x001FBFFA
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlOptimizedStaticResourceRecord._isValueStaticExtensionSection;
			}
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x001FD001 File Offset: 0x001FC001
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} extn(StaticResourceExtension) valueId({1})", this.RecordType, this._valueId);
		}

		// Token: 0x04001ED5 RID: 7893
		private short _valueId;

		// Token: 0x04001ED6 RID: 7894
		private static readonly byte TypeExtensionValueMask = 1;

		// Token: 0x04001ED7 RID: 7895
		private static readonly byte StaticExtensionValueMask = 2;

		// Token: 0x04001ED8 RID: 7896
		private static BitVector32.Section _isValueTypeExtensionSection = BitVector32.CreateSection(1, BamlRecord.LastFlagsSection);

		// Token: 0x04001ED9 RID: 7897
		private static BitVector32.Section _isValueStaticExtensionSection = BitVector32.CreateSection(1, BamlOptimizedStaticResourceRecord._isValueTypeExtensionSection);
	}
}
