using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000495 RID: 1173
	internal class BamlPropertyCustomWriteInfoRecord : BamlPropertyCustomRecord
	{
		// Token: 0x06003CD7 RID: 15575 RVA: 0x001FC3C4 File Offset: 0x001FB3C4
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			short serializerTypeId = base.SerializerTypeId;
			bamlBinaryWriter.Write(base.AttributeId);
			if (serializerTypeId == 137)
			{
				if (this.ValueMemberName != null)
				{
					bamlBinaryWriter.Write(serializerTypeId | BamlPropertyCustomRecord.TypeIdValueMask);
				}
				else
				{
					bamlBinaryWriter.Write(serializerTypeId);
				}
				bamlBinaryWriter.Write(this.ValueId);
				if (this.ValueMemberName != null)
				{
					bamlBinaryWriter.Write(this.ValueMemberName);
				}
				return;
			}
			bamlBinaryWriter.Write(serializerTypeId);
			bool flag = false;
			if (this.ValueType != null && this.ValueType.IsEnum)
			{
				uint num = 0U;
				foreach (string text in this.Value.Split(new char[]
				{
					','
				}))
				{
					FieldInfo field = this.ValueType.GetField(text.Trim(), BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
					if (!(field != null))
					{
						flag = false;
						break;
					}
					object rawConstantValue = field.GetRawConstantValue();
					num += (uint)Convert.ChangeType(rawConstantValue, typeof(uint), TypeConverterHelper.InvariantEnglishUS);
					flag = true;
				}
				if (flag)
				{
					bamlBinaryWriter.Write(num);
				}
			}
			else if (this.ValueType == typeof(bool))
			{
				object value = TypeDescriptor.GetConverter(typeof(bool)).ConvertFromString(this.TypeContext, TypeConverterHelper.InvariantEnglishUS, this.Value);
				bamlBinaryWriter.Write((byte)Convert.ChangeType(value, typeof(byte), TypeConverterHelper.InvariantEnglishUS));
				flag = true;
			}
			else if (this.SerializerType == typeof(XamlBrushSerializer))
			{
				flag = new XamlBrushSerializer().ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlPoint3DCollectionSerializer))
			{
				flag = new XamlPoint3DCollectionSerializer().ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlVector3DCollectionSerializer))
			{
				flag = new XamlVector3DCollectionSerializer().ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlPointCollectionSerializer))
			{
				flag = new XamlPointCollectionSerializer().ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlInt32CollectionSerializer))
			{
				flag = new XamlInt32CollectionSerializer().ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlPathDataSerializer))
			{
				flag = new XamlPathDataSerializer().ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			if (!flag)
			{
				throw new XamlParseException(SR.Get("ParserBadString", new object[]
				{
					this.Value,
					this.ValueType.Name
				}));
			}
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x001FC690 File Offset: 0x001FB690
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyCustomWriteInfoRecord bamlPropertyCustomWriteInfoRecord = (BamlPropertyCustomWriteInfoRecord)record;
			bamlPropertyCustomWriteInfoRecord._valueId = this._valueId;
			bamlPropertyCustomWriteInfoRecord._valueType = this._valueType;
			bamlPropertyCustomWriteInfoRecord._value = this._value;
			bamlPropertyCustomWriteInfoRecord._valueMemberName = this._valueMemberName;
			bamlPropertyCustomWriteInfoRecord._serializerType = this._serializerType;
			bamlPropertyCustomWriteInfoRecord._typeContext = this._typeContext;
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06003CD9 RID: 15577 RVA: 0x001FC6F1 File Offset: 0x001FB6F1
		// (set) Token: 0x06003CDA RID: 15578 RVA: 0x001FC6F9 File Offset: 0x001FB6F9
		internal short ValueId
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

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06003CDB RID: 15579 RVA: 0x001FC702 File Offset: 0x001FB702
		// (set) Token: 0x06003CDC RID: 15580 RVA: 0x001FC70A File Offset: 0x001FB70A
		internal string ValueMemberName
		{
			get
			{
				return this._valueMemberName;
			}
			set
			{
				this._valueMemberName = value;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06003CDD RID: 15581 RVA: 0x001FC713 File Offset: 0x001FB713
		// (set) Token: 0x06003CDE RID: 15582 RVA: 0x001FC71B File Offset: 0x001FB71B
		internal Type ValueType
		{
			get
			{
				return this._valueType;
			}
			set
			{
				this._valueType = value;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06003CDF RID: 15583 RVA: 0x001FC724 File Offset: 0x001FB724
		// (set) Token: 0x06003CE0 RID: 15584 RVA: 0x001FC72C File Offset: 0x001FB72C
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

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06003CE1 RID: 15585 RVA: 0x001FC735 File Offset: 0x001FB735
		// (set) Token: 0x06003CE2 RID: 15586 RVA: 0x001FC73D File Offset: 0x001FB73D
		internal Type SerializerType
		{
			get
			{
				return this._serializerType;
			}
			set
			{
				this._serializerType = value;
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x001FC746 File Offset: 0x001FB746
		// (set) Token: 0x06003CE4 RID: 15588 RVA: 0x001FC74E File Offset: 0x001FB74E
		internal ITypeDescriptorContext TypeContext
		{
			get
			{
				return this._typeContext;
			}
			set
			{
				this._typeContext = value;
			}
		}

		// Token: 0x04001EBB RID: 7867
		private short _valueId;

		// Token: 0x04001EBC RID: 7868
		private Type _valueType;

		// Token: 0x04001EBD RID: 7869
		private string _value;

		// Token: 0x04001EBE RID: 7870
		private string _valueMemberName;

		// Token: 0x04001EBF RID: 7871
		private Type _serializerType;

		// Token: 0x04001EC0 RID: 7872
		private ITypeDescriptorContext _typeContext;
	}
}
