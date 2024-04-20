using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000496 RID: 1174
	internal class BamlPropertyCustomRecord : BamlVariableSizedRecord
	{
		// Token: 0x06003CE6 RID: 15590 RVA: 0x001FC760 File Offset: 0x001FB760
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			short num = bamlBinaryReader.ReadInt16();
			this.IsValueTypeId = ((num & BamlPropertyCustomRecord.TypeIdValueMask) == BamlPropertyCustomRecord.TypeIdValueMask);
			if (this.IsValueTypeId)
			{
				num &= ~BamlPropertyCustomRecord.TypeIdValueMask;
			}
			this.SerializerTypeId = num;
			this.ValueObjectSet = false;
			this.IsRawEnumValueSet = false;
			this._valueObject = null;
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x001FC7C4 File Offset: 0x001FB7C4
		internal object GetCustomValue(BinaryReader reader, Type propertyType, short serializerId, BamlRecordReader bamlRecordReader)
		{
			if (serializerId != 46)
			{
				if (serializerId != 195)
				{
					switch (serializerId)
					{
					case 744:
						this._valueObject = SolidColorBrush.DeserializeFrom(reader, bamlRecordReader.TypeConvertContext);
						goto IL_11D;
					case 745:
						this._valueObject = XamlInt32CollectionSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					case 746:
						this._valueObject = XamlPathDataSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					case 747:
						this._valueObject = XamlPoint3DCollectionSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					case 748:
						this._valueObject = XamlPointCollectionSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					case 752:
						this._valueObject = XamlVector3DCollectionSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					}
					return null;
				}
				uint num;
				if (this._valueObject == null)
				{
					num = reader.ReadUInt32();
				}
				else
				{
					num = (uint)this._valueObject;
				}
				if (propertyType.IsEnum)
				{
					this._valueObject = Enum.ToObject(propertyType, num);
					this.ValueObjectSet = true;
					this.IsRawEnumValueSet = false;
				}
				else
				{
					this._valueObject = num;
					this.ValueObjectSet = false;
					this.IsRawEnumValueSet = true;
				}
				return this._valueObject;
			}
			else
			{
				byte b = reader.ReadByte();
				this._valueObject = (b == 1);
			}
			IL_11D:
			this.ValueObjectSet = true;
			return this._valueObject;
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x001FC8FB File Offset: 0x001FB8FB
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyCustomRecord bamlPropertyCustomRecord = (BamlPropertyCustomRecord)record;
			bamlPropertyCustomRecord._valueObject = this._valueObject;
			bamlPropertyCustomRecord._attributeId = this._attributeId;
			bamlPropertyCustomRecord._serializerTypeId = this._serializerTypeId;
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06003CE9 RID: 15593 RVA: 0x001FC2AC File Offset: 0x001FB2AC
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyCustom;
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06003CEA RID: 15594 RVA: 0x001FC92D File Offset: 0x001FB92D
		// (set) Token: 0x06003CEB RID: 15595 RVA: 0x001FC935 File Offset: 0x001FB935
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

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06003CEC RID: 15596 RVA: 0x001FC93E File Offset: 0x001FB93E
		// (set) Token: 0x06003CED RID: 15597 RVA: 0x001FC946 File Offset: 0x001FB946
		internal short SerializerTypeId
		{
			get
			{
				return this._serializerTypeId;
			}
			set
			{
				this._serializerTypeId = value;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06003CEE RID: 15598 RVA: 0x001FC94F File Offset: 0x001FB94F
		// (set) Token: 0x06003CEF RID: 15599 RVA: 0x001FC957 File Offset: 0x001FB957
		internal object ValueObject
		{
			get
			{
				return this._valueObject;
			}
			set
			{
				this._valueObject = value;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06003CF0 RID: 15600 RVA: 0x001FC960 File Offset: 0x001FB960
		// (set) Token: 0x06003CF1 RID: 15601 RVA: 0x001FC978 File Offset: 0x001FB978
		internal bool ValueObjectSet
		{
			get
			{
				return this._flags[BamlPropertyCustomRecord._isValueSetSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyCustomRecord._isValueSetSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06003CF2 RID: 15602 RVA: 0x001FC991 File Offset: 0x001FB991
		// (set) Token: 0x06003CF3 RID: 15603 RVA: 0x001FC9A9 File Offset: 0x001FB9A9
		internal bool IsValueTypeId
		{
			get
			{
				return this._flags[BamlPropertyCustomRecord._isValueTypeIdSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyCustomRecord._isValueTypeIdSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06003CF4 RID: 15604 RVA: 0x001FC9C2 File Offset: 0x001FB9C2
		// (set) Token: 0x06003CF5 RID: 15605 RVA: 0x001FC9DA File Offset: 0x001FB9DA
		internal bool IsRawEnumValueSet
		{
			get
			{
				return this._flags[BamlPropertyCustomRecord._isRawEnumValueSetSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyCustomRecord._isRawEnumValueSetSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06003CF6 RID: 15606 RVA: 0x001FC9F3 File Offset: 0x001FB9F3
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlPropertyCustomRecord._isRawEnumValueSetSection;
			}
		}

		// Token: 0x04001EC1 RID: 7873
		private object _valueObject;

		// Token: 0x04001EC2 RID: 7874
		private static BitVector32.Section _isValueSetSection = BitVector32.CreateSection(1, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001EC3 RID: 7875
		private static BitVector32.Section _isValueTypeIdSection = BitVector32.CreateSection(1, BamlPropertyCustomRecord._isValueSetSection);

		// Token: 0x04001EC4 RID: 7876
		private static BitVector32.Section _isRawEnumValueSetSection = BitVector32.CreateSection(1, BamlPropertyCustomRecord._isValueTypeIdSection);

		// Token: 0x04001EC5 RID: 7877
		internal static readonly short TypeIdValueMask = 16384;

		// Token: 0x04001EC6 RID: 7878
		private short _attributeId;

		// Token: 0x04001EC7 RID: 7879
		private short _serializerTypeId;
	}
}
