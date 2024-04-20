using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000477 RID: 1143
	internal class BamlMapTable
	{
		// Token: 0x06003A9D RID: 15005 RVA: 0x001F1310 File Offset: 0x001F0310
		internal BamlMapTable(XamlTypeMapper xamlTypeMapper)
		{
			this._xamlTypeMapper = xamlTypeMapper;
			this._knownAssemblyInfoRecord = new BamlAssemblyInfoRecord();
			this._knownAssemblyInfoRecord.AssemblyId = -1;
			this._knownAssemblyInfoRecord.Assembly = ReflectionHelper.LoadAssembly("PresentationFramework", string.Empty);
			this._knownAssemblyInfoRecord.AssemblyFullName = this._knownAssemblyInfoRecord.Assembly.FullName;
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x001F13B2 File Offset: 0x001F03B2
		internal object CreateKnownTypeFromId(short id)
		{
			if (id < 0)
			{
				return KnownTypes.CreateKnownElement((KnownElements)(-(KnownElements)id));
			}
			return null;
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x001F13C2 File Offset: 0x001F03C2
		internal static Type GetKnownTypeFromId(short id)
		{
			if (id < 0)
			{
				return KnownTypes.Types[(int)(-(int)id)];
			}
			return null;
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x001F13D8 File Offset: 0x001F03D8
		internal static short GetKnownTypeIdFromName(string assemblyFullName, string clrNamespace, string typeShortName)
		{
			if (typeShortName == string.Empty)
			{
				return 0;
			}
			int num = 759;
			int i = 1;
			while (i <= num)
			{
				int num2 = (num + i) / 2;
				Type type = KnownTypes.Types[num2];
				int num3 = string.CompareOrdinal(typeShortName, type.Name);
				if (num3 == 0)
				{
					if (type.Namespace == clrNamespace && type.Assembly.FullName == assemblyFullName)
					{
						return (short)(-(short)num2);
					}
					return 0;
				}
				else if (num3 < 0)
				{
					num = num2 - 1;
				}
				else
				{
					i = num2 + 1;
				}
			}
			return 0;
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x001F145D File Offset: 0x001F045D
		internal static short GetKnownTypeIdFromType(Type type)
		{
			if (type == null)
			{
				return 0;
			}
			return BamlMapTable.GetKnownTypeIdFromName(type.Assembly.FullName, type.Namespace, type.Name);
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x001F1488 File Offset: 0x001F0488
		private static short GetKnownStringIdFromName(string stringValue)
		{
			int num = BamlMapTable._knownStrings.Length;
			for (int i = 1; i < num; i++)
			{
				if (BamlMapTable._knownStrings[i] == stringValue)
				{
					return (short)(-(short)i);
				}
			}
			return 0;
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x001F14C0 File Offset: 0x001F04C0
		internal static KnownElements GetKnownTypeConverterIdFromType(Type type)
		{
			KnownElements result;
			if (ReflectionHelper.IsNullableType(type))
			{
				result = KnownElements.NullableConverter;
			}
			else if (type == typeof(Type))
			{
				result = KnownElements.TypeTypeConverter;
			}
			else
			{
				short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(type);
				if (knownTypeIdFromType < 0)
				{
					result = KnownTypes.GetKnownTypeConverterId((KnownElements)(-(KnownElements)knownTypeIdFromType));
				}
				else
				{
					result = KnownElements.UnknownElement;
				}
			}
			return result;
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x001F1510 File Offset: 0x001F0510
		internal TypeConverter GetKnownConverterFromType(Type type)
		{
			KnownElements knownTypeConverterIdFromType = BamlMapTable.GetKnownTypeConverterIdFromType(type);
			TypeConverter result;
			if (knownTypeConverterIdFromType != KnownElements.UnknownElement)
			{
				result = this.GetConverterFromId((short)(-(short)knownTypeConverterIdFromType), type, null);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x001F1538 File Offset: 0x001F0538
		internal static TypeConverter GetKnownConverterFromType_NoCache(Type type)
		{
			KnownElements knownTypeConverterIdFromType = BamlMapTable.GetKnownTypeConverterIdFromType(type);
			TypeConverter result;
			if (knownTypeConverterIdFromType != KnownElements.UnknownElement)
			{
				if (knownTypeConverterIdFromType != KnownElements.EnumConverter)
				{
					if (knownTypeConverterIdFromType != KnownElements.NullableConverter)
					{
						result = (KnownTypes.CreateKnownElement(knownTypeConverterIdFromType) as TypeConverter);
					}
					else
					{
						result = new NullableConverter(type);
					}
				}
				else
				{
					result = new EnumConverter(type);
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x001F1584 File Offset: 0x001F0584
		internal Type GetKnownConverterTypeFromType(Type type)
		{
			if (type == typeof(Type))
			{
				return typeof(TypeTypeConverter);
			}
			short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(type);
			if (knownTypeIdFromType == 0)
			{
				return null;
			}
			KnownElements knownTypeConverterId = KnownTypes.GetKnownTypeConverterId((KnownElements)(-(KnownElements)knownTypeIdFromType));
			if (knownTypeConverterId == KnownElements.UnknownElement)
			{
				return null;
			}
			return KnownTypes.Types[(int)knownTypeConverterId];
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x001F15D4 File Offset: 0x001F05D4
		private static Type GetKnownConverterTypeFromPropName(Type propOwnerType, string propName)
		{
			short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(propOwnerType);
			if (knownTypeIdFromType == 0)
			{
				return null;
			}
			KnownElements knownTypeConverterIdForProperty = KnownTypes.GetKnownTypeConverterIdForProperty((KnownElements)(-(KnownElements)knownTypeIdFromType), propName);
			if (knownTypeConverterIdForProperty == KnownElements.UnknownElement)
			{
				return null;
			}
			return KnownTypes.Types[(int)knownTypeConverterIdForProperty];
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x001F1608 File Offset: 0x001F0608
		internal void Initialize()
		{
			if (this.AttributeIdMap.Count > 0 || this.TypeIdMap.Count > 0)
			{
				this._reusingMapTable = true;
				if (this.ObjectHashTable.Count == 0)
				{
					for (int i = 0; i < this.AttributeIdMap.Count; i++)
					{
						BamlAttributeInfoRecord bamlAttributeInfoRecord = this.AttributeIdMap[i] as BamlAttributeInfoRecord;
						if (bamlAttributeInfoRecord.PropInfo != null)
						{
							object attributeInfoKey = this.GetAttributeInfoKey(bamlAttributeInfoRecord.OwnerType.FullName, bamlAttributeInfoRecord.Name);
							this.ObjectHashTable.Add(attributeInfoKey, bamlAttributeInfoRecord);
						}
					}
					for (int j = 0; j < this.TypeIdMap.Count; j++)
					{
						BamlTypeInfoRecord bamlTypeInfoRecord = this.TypeIdMap[j] as BamlTypeInfoRecord;
						if (bamlTypeInfoRecord.Type != null)
						{
							BamlAssemblyInfoRecord assemblyInfoFromId = this.GetAssemblyInfoFromId(bamlTypeInfoRecord.AssemblyId);
							TypeInfoKey typeInfoKey = this.GetTypeInfoKey(assemblyInfoFromId.AssemblyFullName, bamlTypeInfoRecord.TypeFullName);
							this.ObjectHashTable.Add(typeInfoKey, bamlTypeInfoRecord);
						}
					}
				}
			}
			this.AssemblyIdMap.Clear();
			this.TypeIdMap.Clear();
			this.AttributeIdMap.Clear();
			this.StringIdMap.Clear();
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x001F1744 File Offset: 0x001F0744
		internal Type GetTypeFromId(short id)
		{
			Type type = null;
			if (id < 0)
			{
				return KnownTypes.Types[(int)(-(int)id)];
			}
			BamlTypeInfoRecord bamlTypeInfoRecord = (BamlTypeInfoRecord)this.TypeIdMap[(int)id];
			if (bamlTypeInfoRecord != null)
			{
				type = this.GetTypeFromTypeInfo(bamlTypeInfoRecord);
				if (null == type)
				{
					this.ThrowException("ParserFailFindType", bamlTypeInfoRecord.TypeFullName);
				}
			}
			return type;
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x001F179C File Offset: 0x001F079C
		internal bool HasSerializerForTypeId(short id)
		{
			return id < 0 && (-id == 620 || -id == 231 || -id == 108 || -id == 120 || -id == 271 || -id == 330);
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x001F17D8 File Offset: 0x001F07D8
		internal BamlTypeInfoRecord GetTypeInfoFromId(short id)
		{
			if (id < 0)
			{
				BamlTypeInfoRecord bamlTypeInfoRecord;
				if (-id == 620)
				{
					bamlTypeInfoRecord = new BamlTypeInfoWithSerializerRecord();
					((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerTypeId = -750;
					((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerType = KnownTypes.Types[750];
					bamlTypeInfoRecord.AssemblyId = -1;
				}
				else if (-id == 108 || -id == 120 || -id == 271 || -id == 330)
				{
					bamlTypeInfoRecord = new BamlTypeInfoWithSerializerRecord();
					((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerTypeId = -751;
					((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerType = KnownTypes.Types[751];
					bamlTypeInfoRecord.AssemblyId = -1;
				}
				else
				{
					bamlTypeInfoRecord = new BamlTypeInfoRecord();
					bamlTypeInfoRecord.AssemblyId = this.GetAssemblyIdForType(KnownTypes.Types[(int)(-(int)id)]);
				}
				bamlTypeInfoRecord.TypeId = id;
				bamlTypeInfoRecord.Type = KnownTypes.Types[(int)(-(int)id)];
				bamlTypeInfoRecord.TypeFullName = bamlTypeInfoRecord.Type.FullName;
				return bamlTypeInfoRecord;
			}
			return (BamlTypeInfoRecord)this.TypeIdMap[(int)id];
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x001F18E0 File Offset: 0x001F08E0
		private short GetAssemblyIdForType(Type t)
		{
			string fullName = t.Assembly.FullName;
			for (int i = 0; i < this.AssemblyIdMap.Count; i++)
			{
				if (((BamlAssemblyInfoRecord)this.AssemblyIdMap[i]).AssemblyFullName == fullName)
				{
					return (short)i;
				}
			}
			return -1;
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x001F1934 File Offset: 0x001F0934
		internal TypeConverter GetConverterFromId(short typeId, Type propType, ParserContext pc)
		{
			TypeConverter typeConverter;
			if (typeId < 0)
			{
				KnownElements knownElements = (KnownElements)(-(KnownElements)typeId);
				if (knownElements != KnownElements.EnumConverter)
				{
					if (knownElements != KnownElements.NullableConverter)
					{
						typeConverter = this.GetConverterFromCache(typeId);
						if (typeConverter == null)
						{
							typeConverter = (this.CreateKnownTypeFromId(typeId) as TypeConverter);
							this.ConverterCache.Add(typeId, typeConverter);
						}
					}
					else
					{
						typeConverter = this.GetConverterFromCache(propType);
						if (typeConverter == null)
						{
							typeConverter = new NullableConverter(propType);
							this.ConverterCache.Add(propType, typeConverter);
						}
					}
				}
				else
				{
					typeConverter = this.GetConverterFromCache(propType);
					if (typeConverter == null)
					{
						typeConverter = new EnumConverter(propType);
						this.ConverterCache.Add(propType, typeConverter);
					}
				}
			}
			else
			{
				Type typeFromId = this.GetTypeFromId(typeId);
				typeConverter = this.GetConverterFromCache(typeFromId);
				if (typeConverter == null)
				{
					if (ReflectionHelper.IsPublicType(typeFromId))
					{
						typeConverter = (Activator.CreateInstance(typeFromId) as TypeConverter);
					}
					else
					{
						typeConverter = (XamlTypeMapper.CreateInternalInstance(pc, typeFromId) as TypeConverter);
					}
					if (typeConverter == null)
					{
						this.ThrowException("ParserNoTypeConv", propType.Name);
					}
					else
					{
						this.ConverterCache.Add(typeFromId, typeConverter);
					}
				}
			}
			return typeConverter;
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x001F1A34 File Offset: 0x001F0A34
		internal string GetStringFromStringId(int id)
		{
			if (id < 0)
			{
				return BamlMapTable._knownStrings[-id];
			}
			return ((BamlStringInfoRecord)this.StringIdMap[id]).Value;
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x001F1A5C File Offset: 0x001F0A5C
		internal BamlAttributeInfoRecord GetAttributeInfoFromId(short id)
		{
			if (id < 0)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)id);
				BamlAttributeInfoRecord bamlAttributeInfoRecord = new BamlAttributeInfoRecord();
				bamlAttributeInfoRecord.AttributeId = id;
				bamlAttributeInfoRecord.OwnerTypeId = (short)(-(short)KnownTypes.GetKnownElementFromKnownCommonProperty(knownProperties));
				this.GetAttributeOwnerType(bamlAttributeInfoRecord);
				bamlAttributeInfoRecord.Name = this.GetAttributeNameFromKnownId(knownProperties);
				if (knownProperties < KnownProperties.MaxDependencyProperty)
				{
					DependencyProperty knownDependencyPropertyFromId = KnownTypes.GetKnownDependencyPropertyFromId(knownProperties);
					bamlAttributeInfoRecord.DP = knownDependencyPropertyFromId;
				}
				else
				{
					Type ownerType = bamlAttributeInfoRecord.OwnerType;
					bamlAttributeInfoRecord.PropInfo = ownerType.GetProperty(bamlAttributeInfoRecord.Name, BindingFlags.Instance | BindingFlags.Public);
				}
				return bamlAttributeInfoRecord;
			}
			return (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x001F1AE8 File Offset: 0x001F0AE8
		internal BamlAttributeInfoRecord GetAttributeInfoFromIdWithOwnerType(short attributeId)
		{
			BamlAttributeInfoRecord attributeInfoFromId = this.GetAttributeInfoFromId(attributeId);
			this.GetAttributeOwnerType(attributeInfoFromId);
			return attributeInfoFromId;
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x001F1B06 File Offset: 0x001F0B06
		private string GetAttributeNameFromKnownId(KnownProperties knownId)
		{
			if (knownId < KnownProperties.MaxDependencyProperty)
			{
				return KnownTypes.GetKnownDependencyPropertyFromId(knownId).Name;
			}
			return KnownTypes.GetKnownClrPropertyNameFromId(knownId);
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x001F1B24 File Offset: 0x001F0B24
		internal string GetAttributeNameFromId(short id)
		{
			if (id < 0)
			{
				return this.GetAttributeNameFromKnownId((KnownProperties)(-(KnownProperties)id));
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
			if (bamlAttributeInfoRecord != null)
			{
				return bamlAttributeInfoRecord.Name;
			}
			return null;
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x001F1B5C File Offset: 0x001F0B5C
		internal bool DoesAttributeMatch(short id, short ownerTypeId, string name)
		{
			if (id < 0)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)id);
				string attributeNameFromKnownId = this.GetAttributeNameFromKnownId(knownProperties);
				KnownElements knownElementFromKnownCommonProperty = KnownTypes.GetKnownElementFromKnownCommonProperty(knownProperties);
				return ownerTypeId == (short)(-(short)knownElementFromKnownCommonProperty) && string.CompareOrdinal(attributeNameFromKnownId, name) == 0;
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
			return bamlAttributeInfoRecord.OwnerTypeId == ownerTypeId && string.CompareOrdinal(bamlAttributeInfoRecord.Name, name) == 0;
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x001F1BC0 File Offset: 0x001F0BC0
		internal bool DoesAttributeMatch(short id, string name)
		{
			string attributeNameFromId = this.GetAttributeNameFromId(id);
			return attributeNameFromId != null && string.CompareOrdinal(attributeNameFromId, name) == 0;
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x001F1BE4 File Offset: 0x001F0BE4
		internal bool DoesAttributeMatch(short id, BamlAttributeUsage attributeUsage)
		{
			if (id < 0)
			{
				return attributeUsage == BamlMapTable.GetAttributeUsageFromKnownAttribute((KnownProperties)(-(KnownProperties)id));
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
			return attributeUsage == bamlAttributeInfoRecord.AttributeUsage;
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x001F1C1C File Offset: 0x001F0C1C
		internal void GetAttributeInfoFromId(short id, out short ownerTypeId, out string name, out BamlAttributeUsage attributeUsage)
		{
			if (id < 0)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)id);
				name = this.GetAttributeNameFromKnownId(knownProperties);
				ownerTypeId = (short)(-(short)KnownTypes.GetKnownElementFromKnownCommonProperty(knownProperties));
				attributeUsage = BamlMapTable.GetAttributeUsageFromKnownAttribute(knownProperties);
				return;
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[(int)id];
			name = bamlAttributeInfoRecord.Name;
			ownerTypeId = bamlAttributeInfoRecord.OwnerTypeId;
			attributeUsage = bamlAttributeInfoRecord.AttributeUsage;
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x001F1C79 File Offset: 0x001F0C79
		private static BamlAttributeUsage GetAttributeUsageFromKnownAttribute(KnownProperties knownId)
		{
			if (knownId == KnownProperties.FrameworkElement_Name)
			{
				return BamlAttributeUsage.RuntimeName;
			}
			return BamlAttributeUsage.Default;
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x001F1C84 File Offset: 0x001F0C84
		internal Type GetTypeFromTypeInfo(BamlTypeInfoRecord typeInfo)
		{
			if (null == typeInfo.Type)
			{
				BamlAssemblyInfoRecord assemblyInfoFromId = this.GetAssemblyInfoFromId(typeInfo.AssemblyId);
				if (assemblyInfoFromId != null)
				{
					TypeInfoKey typeInfoKey = this.GetTypeInfoKey(assemblyInfoFromId.AssemblyFullName, typeInfo.TypeFullName);
					BamlTypeInfoRecord bamlTypeInfoRecord = this.GetHashTableData(typeInfoKey) as BamlTypeInfoRecord;
					if (bamlTypeInfoRecord != null && bamlTypeInfoRecord.Type != null)
					{
						typeInfo.Type = bamlTypeInfoRecord.Type;
					}
					else
					{
						Assembly assemblyFromAssemblyInfo = this.GetAssemblyFromAssemblyInfo(assemblyInfoFromId);
						if (null != assemblyFromAssemblyInfo)
						{
							Type type = assemblyFromAssemblyInfo.GetType(typeInfo.TypeFullName);
							typeInfo.Type = type;
							this.AddHashTableData(typeInfoKey, typeInfo);
						}
					}
				}
			}
			return typeInfo.Type;
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x001F1D30 File Offset: 0x001F0D30
		private Type GetAttributeOwnerType(BamlAttributeInfoRecord bamlAttributeInfoRecord)
		{
			if (bamlAttributeInfoRecord.OwnerType == null)
			{
				if (bamlAttributeInfoRecord.OwnerTypeId < 0)
				{
					bamlAttributeInfoRecord.OwnerType = BamlMapTable.GetKnownTypeFromId(bamlAttributeInfoRecord.OwnerTypeId);
				}
				else
				{
					BamlTypeInfoRecord bamlTypeInfoRecord = (BamlTypeInfoRecord)this.TypeIdMap[(int)bamlAttributeInfoRecord.OwnerTypeId];
					if (bamlTypeInfoRecord != null)
					{
						bamlAttributeInfoRecord.OwnerType = this.GetTypeFromTypeInfo(bamlTypeInfoRecord);
					}
				}
			}
			return bamlAttributeInfoRecord.OwnerType;
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x001F1D94 File Offset: 0x001F0D94
		internal Type GetCLRPropertyTypeAndNameFromId(short attributeId, out string propName)
		{
			propName = null;
			Type type = null;
			BamlAttributeInfoRecord attributeInfoFromIdWithOwnerType = this.GetAttributeInfoFromIdWithOwnerType(attributeId);
			if (attributeInfoFromIdWithOwnerType != null && attributeInfoFromIdWithOwnerType.OwnerType != null)
			{
				this.XamlTypeMapper.UpdateClrPropertyInfo(attributeInfoFromIdWithOwnerType.OwnerType, attributeInfoFromIdWithOwnerType);
				type = attributeInfoFromIdWithOwnerType.GetPropertyType();
			}
			else
			{
				propName = string.Empty;
			}
			if (type == null)
			{
				if (propName == null)
				{
					propName = attributeInfoFromIdWithOwnerType.OwnerType.FullName + "." + attributeInfoFromIdWithOwnerType.Name;
				}
				this.ThrowException("ParserNoPropType", propName);
			}
			else
			{
				propName = attributeInfoFromIdWithOwnerType.Name;
			}
			return type;
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x001F1E24 File Offset: 0x001F0E24
		internal DependencyProperty GetDependencyPropertyValueFromId(short memberId, string memberName, out Type declaringType)
		{
			declaringType = null;
			DependencyProperty result = null;
			if (memberName == null)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)memberId);
				if (knownProperties < KnownProperties.MaxDependencyProperty || knownProperties == KnownProperties.Run_Text)
				{
					result = KnownTypes.GetKnownDependencyPropertyFromId(knownProperties);
				}
			}
			else
			{
				declaringType = this.GetTypeFromId(memberId);
				result = DependencyProperty.FromName(memberName, declaringType);
			}
			return result;
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x001F1E6C File Offset: 0x001F0E6C
		internal DependencyProperty GetDependencyPropertyValueFromId(short memberId)
		{
			DependencyProperty dependencyProperty = null;
			if (memberId < 0)
			{
				KnownProperties knownProperties = (KnownProperties)(-(KnownProperties)memberId);
				if (knownProperties < KnownProperties.MaxDependencyProperty)
				{
					dependencyProperty = KnownTypes.GetKnownDependencyPropertyFromId(knownProperties);
				}
			}
			if (dependencyProperty == null)
			{
				short id;
				string name;
				BamlAttributeUsage bamlAttributeUsage;
				this.GetAttributeInfoFromId(memberId, out id, out name, out bamlAttributeUsage);
				Type typeFromId = this.GetTypeFromId(id);
				dependencyProperty = DependencyProperty.FromName(name, typeFromId);
			}
			return dependencyProperty;
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x001F1EB8 File Offset: 0x001F0EB8
		internal DependencyProperty GetDependencyProperty(int id)
		{
			if (id < 0)
			{
				return KnownTypes.GetKnownDependencyPropertyFromId((KnownProperties)(-(KnownProperties)id));
			}
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.AttributeIdMap[id];
			return this.GetDependencyProperty(bamlAttributeInfoRecord);
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x001F1EEC File Offset: 0x001F0EEC
		internal DependencyProperty GetDependencyProperty(BamlAttributeInfoRecord bamlAttributeInfoRecord)
		{
			if (bamlAttributeInfoRecord.DP == null && null == bamlAttributeInfoRecord.PropInfo)
			{
				this.GetAttributeOwnerType(bamlAttributeInfoRecord);
				if (null != bamlAttributeInfoRecord.OwnerType)
				{
					bamlAttributeInfoRecord.DP = DependencyProperty.FromName(bamlAttributeInfoRecord.Name, bamlAttributeInfoRecord.OwnerType);
				}
			}
			return bamlAttributeInfoRecord.DP;
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x001F1F44 File Offset: 0x001F0F44
		internal RoutedEvent GetRoutedEvent(BamlAttributeInfoRecord bamlAttributeInfoRecord)
		{
			if (bamlAttributeInfoRecord.Event == null)
			{
				Type attributeOwnerType = this.GetAttributeOwnerType(bamlAttributeInfoRecord);
				if (null != attributeOwnerType)
				{
					bamlAttributeInfoRecord.Event = this.XamlTypeMapper.RoutedEventFromName(bamlAttributeInfoRecord.Name, attributeOwnerType);
				}
			}
			return bamlAttributeInfoRecord.Event;
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x001F1F88 File Offset: 0x001F0F88
		internal short GetAttributeOrTypeId(BinaryWriter binaryWriter, Type declaringType, string memberName, out short typeId)
		{
			short result = 0;
			if (!this.GetTypeInfoId(binaryWriter, declaringType.Assembly.FullName, declaringType.FullName, out typeId))
			{
				typeId = this.AddTypeInfoMap(binaryWriter, declaringType.Assembly.FullName, declaringType.FullName, declaringType, string.Empty, string.Empty);
			}
			else if (typeId < 0)
			{
				result = -KnownTypes.GetKnownPropertyAttributeId((KnownElements)(-(KnownElements)typeId), memberName);
			}
			return result;
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x001F1FF0 File Offset: 0x001F0FF0
		internal BamlAssemblyInfoRecord GetAssemblyInfoFromId(short id)
		{
			if (id == -1)
			{
				return this._knownAssemblyInfoRecord;
			}
			return (BamlAssemblyInfoRecord)this.AssemblyIdMap[(int)id];
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x001F2010 File Offset: 0x001F1010
		private Assembly GetAssemblyFromAssemblyInfo(BamlAssemblyInfoRecord assemblyInfoRecord)
		{
			if (null == assemblyInfoRecord.Assembly)
			{
				string assemblyPath = this.XamlTypeMapper.AssemblyPathFor(assemblyInfoRecord.AssemblyFullName);
				assemblyInfoRecord.Assembly = ReflectionHelper.LoadAssembly(assemblyInfoRecord.AssemblyFullName, assemblyPath);
			}
			return assemblyInfoRecord.Assembly;
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x001F2058 File Offset: 0x001F1058
		internal BamlAssemblyInfoRecord AddAssemblyMap(BinaryWriter binaryWriter, string assemblyFullName)
		{
			AssemblyInfoKey assemblyInfoKey = new AssemblyInfoKey
			{
				AssemblyFullName = assemblyFullName
			};
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = (BamlAssemblyInfoRecord)this.GetHashTableData(assemblyInfoKey);
			if (bamlAssemblyInfoRecord == null)
			{
				bamlAssemblyInfoRecord = new BamlAssemblyInfoRecord();
				bamlAssemblyInfoRecord.AssemblyFullName = assemblyFullName;
				bamlAssemblyInfoRecord.AssemblyId = (short)this.AssemblyIdMap.Add(bamlAssemblyInfoRecord);
				this.ObjectHashTable.Add(assemblyInfoKey, bamlAssemblyInfoRecord);
				bamlAssemblyInfoRecord.Write(binaryWriter);
			}
			else if (bamlAssemblyInfoRecord.AssemblyId == -1)
			{
				bamlAssemblyInfoRecord.AssemblyId = (short)this.AssemblyIdMap.Add(bamlAssemblyInfoRecord);
				bamlAssemblyInfoRecord.Write(binaryWriter);
			}
			return bamlAssemblyInfoRecord;
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x001F20E9 File Offset: 0x001F10E9
		internal void LoadAssemblyInfoRecord(BamlAssemblyInfoRecord record)
		{
			if (this.AssemblyIdMap.Count == (int)record.AssemblyId)
			{
				this.AssemblyIdMap.Add(record);
			}
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x001F210C File Offset: 0x001F110C
		internal void EnsureAssemblyRecord(Assembly asm)
		{
			string fullName = asm.FullName;
			if (!(this.ObjectHashTable[fullName] is BamlAssemblyInfoRecord))
			{
				BamlAssemblyInfoRecord bamlAssemblyInfoRecord = new BamlAssemblyInfoRecord();
				bamlAssemblyInfoRecord.AssemblyFullName = fullName;
				bamlAssemblyInfoRecord.Assembly = asm;
				this.ObjectHashTable[fullName] = bamlAssemblyInfoRecord;
			}
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x001F2158 File Offset: 0x001F1158
		private TypeInfoKey GetTypeInfoKey(string assemblyFullName, string typeFullName)
		{
			return new TypeInfoKey
			{
				DeclaringAssembly = assemblyFullName,
				TypeFullName = typeFullName
			};
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x001F2180 File Offset: 0x001F1180
		internal bool GetTypeInfoId(BinaryWriter binaryWriter, string assemblyFullName, string typeFullName, out short typeId)
		{
			int num = typeFullName.LastIndexOf(".", StringComparison.Ordinal);
			string typeShortName;
			string clrNamespace;
			if (num >= 0)
			{
				typeShortName = typeFullName.Substring(num + 1);
				clrNamespace = typeFullName.Substring(0, num);
			}
			else
			{
				typeShortName = typeFullName;
				clrNamespace = string.Empty;
			}
			typeId = BamlMapTable.GetKnownTypeIdFromName(assemblyFullName, clrNamespace, typeShortName);
			if (typeId < 0)
			{
				return true;
			}
			TypeInfoKey typeInfoKey = this.GetTypeInfoKey(assemblyFullName, typeFullName);
			BamlTypeInfoRecord bamlTypeInfoRecord = (BamlTypeInfoRecord)this.GetHashTableData(typeInfoKey);
			if (bamlTypeInfoRecord == null)
			{
				return false;
			}
			typeId = bamlTypeInfoRecord.TypeId;
			return true;
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x001F21FC File Offset: 0x001F11FC
		internal short AddTypeInfoMap(BinaryWriter binaryWriter, string assemblyFullName, string typeFullName, Type elementType, string serializerAssemblyFullName, string serializerTypeFullName)
		{
			TypeInfoKey typeInfoKey = this.GetTypeInfoKey(assemblyFullName, typeFullName);
			BamlTypeInfoRecord bamlTypeInfoRecord;
			if (serializerTypeFullName == string.Empty)
			{
				bamlTypeInfoRecord = new BamlTypeInfoRecord();
			}
			else
			{
				bamlTypeInfoRecord = new BamlTypeInfoWithSerializerRecord();
				short serializerTypeId;
				if (!this.GetTypeInfoId(binaryWriter, serializerAssemblyFullName, serializerTypeFullName, out serializerTypeId))
				{
					serializerTypeId = this.AddTypeInfoMap(binaryWriter, serializerAssemblyFullName, serializerTypeFullName, null, string.Empty, string.Empty);
				}
				((BamlTypeInfoWithSerializerRecord)bamlTypeInfoRecord).SerializerTypeId = serializerTypeId;
			}
			bamlTypeInfoRecord.TypeFullName = typeFullName;
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = this.AddAssemblyMap(binaryWriter, assemblyFullName);
			bamlTypeInfoRecord.AssemblyId = bamlAssemblyInfoRecord.AssemblyId;
			bamlTypeInfoRecord.IsInternalType = (elementType != null && ReflectionHelper.IsInternalType(elementType));
			bamlTypeInfoRecord.TypeId = (short)this.TypeIdMap.Add(bamlTypeInfoRecord);
			this.ObjectHashTable.Add(typeInfoKey, bamlTypeInfoRecord);
			bamlTypeInfoRecord.Write(binaryWriter);
			return bamlTypeInfoRecord.TypeId;
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x001F22C8 File Offset: 0x001F12C8
		internal void LoadTypeInfoRecord(BamlTypeInfoRecord record)
		{
			if (this.TypeIdMap.Count == (int)record.TypeId)
			{
				this.TypeIdMap.Add(record);
			}
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x001F22EA File Offset: 0x001F12EA
		internal object GetAttributeInfoKey(string ownerTypeName, string attributeName)
		{
			return ownerTypeName + "." + attributeName;
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x001F22F8 File Offset: 0x001F12F8
		internal short AddAttributeInfoMap(BinaryWriter binaryWriter, string assemblyFullName, string typeFullName, Type owningType, string fieldName, Type attributeType, BamlAttributeUsage attributeUsage)
		{
			BamlAttributeInfoRecord bamlAttributeInfoRecord;
			return this.AddAttributeInfoMap(binaryWriter, assemblyFullName, typeFullName, owningType, fieldName, attributeType, attributeUsage, out bamlAttributeInfoRecord);
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x001F2318 File Offset: 0x001F1318
		internal short AddAttributeInfoMap(BinaryWriter binaryWriter, string assemblyFullName, string typeFullName, Type owningType, string fieldName, Type attributeType, BamlAttributeUsage attributeUsage, out BamlAttributeInfoRecord bamlAttributeInfoRecord)
		{
			short num;
			if (!this.GetTypeInfoId(binaryWriter, assemblyFullName, typeFullName, out num))
			{
				Type xamlSerializerForType = this.XamlTypeMapper.GetXamlSerializerForType(owningType);
				string serializerAssemblyFullName = (xamlSerializerForType == null) ? string.Empty : xamlSerializerForType.Assembly.FullName;
				string serializerTypeFullName = (xamlSerializerForType == null) ? string.Empty : xamlSerializerForType.FullName;
				num = this.AddTypeInfoMap(binaryWriter, assemblyFullName, typeFullName, owningType, serializerAssemblyFullName, serializerTypeFullName);
			}
			else if (num < 0)
			{
				short num2 = -KnownTypes.GetKnownPropertyAttributeId((KnownElements)(-(KnownElements)num), fieldName);
				if (num2 < 0)
				{
					bamlAttributeInfoRecord = null;
					return num2;
				}
			}
			object attributeInfoKey = this.GetAttributeInfoKey(typeFullName, fieldName);
			bamlAttributeInfoRecord = (BamlAttributeInfoRecord)this.GetHashTableData(attributeInfoKey);
			if (bamlAttributeInfoRecord == null)
			{
				bamlAttributeInfoRecord = new BamlAttributeInfoRecord();
				bamlAttributeInfoRecord.Name = fieldName;
				bamlAttributeInfoRecord.OwnerTypeId = num;
				bamlAttributeInfoRecord.AttributeId = (short)this.AttributeIdMap.Add(bamlAttributeInfoRecord);
				bamlAttributeInfoRecord.AttributeUsage = attributeUsage;
				this.ObjectHashTable.Add(attributeInfoKey, bamlAttributeInfoRecord);
				bamlAttributeInfoRecord.Write(binaryWriter);
			}
			return bamlAttributeInfoRecord.AttributeId;
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x001F2420 File Offset: 0x001F1420
		internal bool GetCustomSerializerOrConverter(BinaryWriter binaryWriter, Type ownerType, Type attributeType, object piOrMi, string fieldName, out short converterOrSerializerTypeId, out Type converterOrSerializerType)
		{
			converterOrSerializerType = null;
			converterOrSerializerTypeId = 0;
			if (!this.ShouldBypassCustomCheck(ownerType, attributeType))
			{
				converterOrSerializerType = this.GetCustomSerializer(attributeType, out converterOrSerializerTypeId);
				if (converterOrSerializerType != null)
				{
					return true;
				}
				converterOrSerializerType = this.GetCustomConverter(piOrMi, ownerType, fieldName, attributeType);
				if (converterOrSerializerType == null && attributeType.IsEnum)
				{
					converterOrSerializerTypeId = 195;
					converterOrSerializerType = KnownTypes.Types[(int)converterOrSerializerTypeId];
					return true;
				}
				if (converterOrSerializerType != null)
				{
					string fullName = converterOrSerializerType.FullName;
					this.EnsureAssemblyRecord(converterOrSerializerType.Assembly);
					if (!this.GetTypeInfoId(binaryWriter, converterOrSerializerType.Assembly.FullName, fullName, out converterOrSerializerTypeId))
					{
						converterOrSerializerTypeId = this.AddTypeInfoMap(binaryWriter, converterOrSerializerType.Assembly.FullName, fullName, null, string.Empty, string.Empty);
					}
				}
			}
			return false;
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x001F24F8 File Offset: 0x001F14F8
		internal bool GetStringInfoId(string stringValue, out short stringId)
		{
			stringId = BamlMapTable.GetKnownStringIdFromName(stringValue);
			if (stringId < 0)
			{
				return true;
			}
			BamlStringInfoRecord bamlStringInfoRecord = (BamlStringInfoRecord)this.GetHashTableData(stringValue);
			if (bamlStringInfoRecord == null)
			{
				return false;
			}
			stringId = bamlStringInfoRecord.StringId;
			return true;
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x001F2530 File Offset: 0x001F1530
		internal short AddStringInfoMap(BinaryWriter binaryWriter, string stringValue)
		{
			BamlStringInfoRecord bamlStringInfoRecord = new BamlStringInfoRecord();
			bamlStringInfoRecord.StringId = (short)this.StringIdMap.Add(bamlStringInfoRecord);
			bamlStringInfoRecord.Value = stringValue;
			this.ObjectHashTable.Add(stringValue, bamlStringInfoRecord);
			bamlStringInfoRecord.Write(binaryWriter);
			return bamlStringInfoRecord.StringId;
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x001F2578 File Offset: 0x001F1578
		internal short GetStaticMemberId(BinaryWriter binaryWriter, ParserContext pc, short extensionTypeId, string memberValue, Type defaultTargetType)
		{
			short num = 0;
			Type type = null;
			string text = null;
			if (extensionTypeId != 602)
			{
				if (extensionTypeId == 634)
				{
					type = this.XamlTypeMapper.GetDependencyPropertyOwnerAndName(memberValue, pc, defaultTargetType, out text);
				}
			}
			else
			{
				type = this.XamlTypeMapper.GetTargetTypeAndMember(memberValue, pc, true, out text);
				if (this.XamlTypeMapper.GetStaticMemberInfo(type, text, false) is PropertyInfo)
				{
					num = SystemResourceKey.GetBamlIdBasedOnSystemResourceKeyId(type, text);
				}
			}
			if (num == 0)
			{
				num = this.AddAttributeInfoMap(binaryWriter, type.Assembly.FullName, type.FullName, type, text, null, BamlAttributeUsage.Default);
			}
			return num;
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x001F2602 File Offset: 0x001F1602
		private bool ShouldBypassCustomCheck(Type declaringType, Type attributeType)
		{
			return declaringType == null || attributeType == null;
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x001F261C File Offset: 0x001F161C
		private Type GetCustomConverter(object piOrMi, Type ownerType, string fieldName, Type attributeType)
		{
			Type type = BamlMapTable.GetKnownConverterTypeFromPropName(ownerType, fieldName);
			if (type != null)
			{
				return type;
			}
			Assembly assembly = ownerType.Assembly;
			if (!assembly.FullName.StartsWith("PresentationFramework", StringComparison.OrdinalIgnoreCase) && !assembly.FullName.StartsWith("PresentationCore", StringComparison.OrdinalIgnoreCase) && !assembly.FullName.StartsWith("WindowsBase", StringComparison.OrdinalIgnoreCase))
			{
				type = this.XamlTypeMapper.GetPropertyConverterType(attributeType, piOrMi);
				if (type != null)
				{
					return type;
				}
			}
			return this.XamlTypeMapper.GetTypeConverterType(attributeType);
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x001F26A8 File Offset: 0x001F16A8
		private Type GetCustomSerializer(Type type, out short converterOrSerializerTypeId)
		{
			int num;
			if (type == typeof(bool))
			{
				num = 46;
			}
			else if (type == KnownTypes.Types[136])
			{
				num = 137;
			}
			else
			{
				num = this.XamlTypeMapper.GetCustomBamlSerializerIdForType(type);
				if (num == 0)
				{
					converterOrSerializerTypeId = 0;
					return null;
				}
			}
			converterOrSerializerTypeId = (short)num;
			return KnownTypes.Types[num];
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x001F270F File Offset: 0x001F170F
		private void ThrowException(string id, string parameter)
		{
			throw new ApplicationException(SR.Get(id, new object[]
			{
				parameter
			}));
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x001F2726 File Offset: 0x001F1726
		internal void LoadAttributeInfoRecord(BamlAttributeInfoRecord record)
		{
			if (this.AttributeIdMap.Count == (int)record.AttributeId)
			{
				this.AttributeIdMap.Add(record);
			}
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x001F2748 File Offset: 0x001F1748
		internal void LoadStringInfoRecord(BamlStringInfoRecord record)
		{
			if (this.StringIdMap.Count == (int)record.StringId)
			{
				this.StringIdMap.Add(record);
			}
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x001F276A File Offset: 0x001F176A
		internal object GetHashTableData(object key)
		{
			return this.ObjectHashTable[key];
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x001F2778 File Offset: 0x001F1778
		internal void AddHashTableData(object key, object data)
		{
			if (this._reusingMapTable)
			{
				this.ObjectHashTable[key] = data;
			}
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x001F2790 File Offset: 0x001F1790
		internal BamlMapTable Clone()
		{
			return new BamlMapTable(this._xamlTypeMapper)
			{
				_objectHashTable = (Hashtable)this._objectHashTable.Clone(),
				_assemblyIdToInfo = (ArrayList)this._assemblyIdToInfo.Clone(),
				_typeIdToInfo = (ArrayList)this._typeIdToInfo.Clone(),
				_attributeIdToInfo = (ArrayList)this._attributeIdToInfo.Clone(),
				_stringIdToInfo = (ArrayList)this._stringIdToInfo.Clone()
			};
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x001F2818 File Offset: 0x001F1818
		private TypeConverter GetConverterFromCache(short typeId)
		{
			TypeConverter result = null;
			if (this._converterCache != null)
			{
				result = (this._converterCache[typeId] as TypeConverter);
			}
			return result;
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x001F2848 File Offset: 0x001F1848
		private TypeConverter GetConverterFromCache(Type type)
		{
			TypeConverter result = null;
			if (this._converterCache != null)
			{
				result = (this._converterCache[type] as TypeConverter);
			}
			return result;
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x001F2872 File Offset: 0x001F1872
		internal void ClearConverterCache()
		{
			if (this._converterCache != null)
			{
				this._converterCache.Clear();
				this._converterCache = null;
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x001F288E File Offset: 0x001F188E
		private Hashtable ObjectHashTable
		{
			get
			{
				return this._objectHashTable;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x001F2896 File Offset: 0x001F1896
		private ArrayList AssemblyIdMap
		{
			get
			{
				return this._assemblyIdToInfo;
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x001F289E File Offset: 0x001F189E
		private ArrayList TypeIdMap
		{
			get
			{
				return this._typeIdToInfo;
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x001F28A6 File Offset: 0x001F18A6
		private ArrayList AttributeIdMap
		{
			get
			{
				return this._attributeIdToInfo;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x001F28AE File Offset: 0x001F18AE
		private ArrayList StringIdMap
		{
			get
			{
				return this._stringIdToInfo;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x001F28B6 File Offset: 0x001F18B6
		// (set) Token: 0x06003AE3 RID: 15075 RVA: 0x001F28BE File Offset: 0x001F18BE
		internal XamlTypeMapper XamlTypeMapper
		{
			get
			{
				return this._xamlTypeMapper;
			}
			set
			{
				this._xamlTypeMapper = value;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06003AE4 RID: 15076 RVA: 0x001F28C7 File Offset: 0x001F18C7
		private Hashtable ConverterCache
		{
			get
			{
				if (this._converterCache == null)
				{
					this._converterCache = new Hashtable();
				}
				return this._converterCache;
			}
		}

		// Token: 0x04001DCD RID: 7629
		private const string _coreAssembly = "PresentationCore";

		// Token: 0x04001DCE RID: 7630
		private const string _frameworkAssembly = "PresentationFramework";

		// Token: 0x04001DCF RID: 7631
		private static string[] _knownStrings = new string[]
		{
			null,
			"Name",
			"Uid"
		};

		// Token: 0x04001DD0 RID: 7632
		internal static short NameStringId = -1;

		// Token: 0x04001DD1 RID: 7633
		internal static short UidStringId = -2;

		// Token: 0x04001DD2 RID: 7634
		internal static string NameString = "Name";

		// Token: 0x04001DD3 RID: 7635
		private Hashtable _objectHashTable = new Hashtable();

		// Token: 0x04001DD4 RID: 7636
		private ArrayList _assemblyIdToInfo = new ArrayList(1);

		// Token: 0x04001DD5 RID: 7637
		private ArrayList _typeIdToInfo = new ArrayList(0);

		// Token: 0x04001DD6 RID: 7638
		private ArrayList _attributeIdToInfo = new ArrayList(10);

		// Token: 0x04001DD7 RID: 7639
		private ArrayList _stringIdToInfo = new ArrayList(1);

		// Token: 0x04001DD8 RID: 7640
		private XamlTypeMapper _xamlTypeMapper;

		// Token: 0x04001DD9 RID: 7641
		private BamlAssemblyInfoRecord _knownAssemblyInfoRecord;

		// Token: 0x04001DDA RID: 7642
		private Hashtable _converterCache;

		// Token: 0x04001DDB RID: 7643
		private bool _reusingMapTable;
	}
}
