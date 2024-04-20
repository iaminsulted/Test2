using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x0200047B RID: 1147
	internal class BamlReader
	{
		// Token: 0x06003AF0 RID: 15088 RVA: 0x001F2ADC File Offset: 0x001F1ADC
		public BamlReader(Stream bamlStream)
		{
			this._parserContext = new ParserContext();
			this._parserContext.XamlTypeMapper = XmlParserDefaults.DefaultMapper;
			this._bamlRecordReader = new BamlRecordReader(bamlStream, this._parserContext, false);
			this._readState = ReadState.Initial;
			this._bamlNodeType = BamlNodeType.None;
			this._prefixDictionary = new XmlnsDictionary();
			this._value = string.Empty;
			this._assemblyName = string.Empty;
			this._prefix = string.Empty;
			this._xmlNamespace = string.Empty;
			this._clrNamespace = string.Empty;
			this._name = string.Empty;
			this._localName = string.Empty;
			this._ownerTypeName = string.Empty;
			this._properties = new ArrayList();
			this._haveUnprocessedRecord = false;
			this._deferableContentBlockDepth = -1;
			this._nodeStack = new Stack();
			this._reverseXmlnsTable = new Dictionary<string, List<string>>();
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x001F2BBD File Offset: 0x001F1BBD
		public int PropertyCount
		{
			get
			{
				return this._properties.Count;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06003AF2 RID: 15090 RVA: 0x001F2BCA File Offset: 0x001F1BCA
		public bool HasProperties
		{
			get
			{
				return this.PropertyCount > 0;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x001F2BD5 File Offset: 0x001F1BD5
		public int ConnectionId
		{
			get
			{
				return this._connectionId;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x001F2BDD File Offset: 0x001F1BDD
		public BamlAttributeUsage AttributeUsage
		{
			get
			{
				return this._attributeUsage;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06003AF5 RID: 15093 RVA: 0x001F2BE5 File Offset: 0x001F1BE5
		public BamlNodeType NodeType
		{
			get
			{
				return this._bamlNodeType;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06003AF6 RID: 15094 RVA: 0x001F2BED File Offset: 0x001F1BED
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06003AF7 RID: 15095 RVA: 0x001F2BF5 File Offset: 0x001F1BF5
		public string LocalName
		{
			get
			{
				return this._localName;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06003AF8 RID: 15096 RVA: 0x001F2BFD File Offset: 0x001F1BFD
		public string Prefix
		{
			get
			{
				return this._prefix;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x001F2C05 File Offset: 0x001F1C05
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06003AFA RID: 15098 RVA: 0x001F2C0D File Offset: 0x001F1C0D
		public string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06003AFB RID: 15099 RVA: 0x001F2C15 File Offset: 0x001F1C15
		public string ClrNamespace
		{
			get
			{
				return this._clrNamespace;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06003AFC RID: 15100 RVA: 0x001F2C1D File Offset: 0x001F1C1D
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06003AFD RID: 15101 RVA: 0x001F2C25 File Offset: 0x001F1C25
		public bool IsInjected
		{
			get
			{
				return this._isInjected;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06003AFE RID: 15102 RVA: 0x001F2C2D File Offset: 0x001F1C2D
		public bool CreateUsingTypeConverter
		{
			get
			{
				return this._useTypeConverter;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06003AFF RID: 15103 RVA: 0x001F2C35 File Offset: 0x001F1C35
		public string TypeConverterName
		{
			get
			{
				return this._typeConverterName;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06003B00 RID: 15104 RVA: 0x001F2C3D File Offset: 0x001F1C3D
		public string TypeConverterAssemblyName
		{
			get
			{
				return this._typeConverterAssemblyName;
			}
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x001F2C45 File Offset: 0x001F1C45
		public bool Read()
		{
			if (this._readState == ReadState.EndOfFile || this._readState == ReadState.Closed)
			{
				throw new InvalidOperationException(SR.Get("BamlReaderClosed"));
			}
			this.ReadNextRecord();
			return this._readState != ReadState.EndOfFile;
		}

		// Token: 0x17000CD0 RID: 3280
		// (set) Token: 0x06003B02 RID: 15106 RVA: 0x001F2C7B File Offset: 0x001F1C7B
		private BamlNodeType NodeTypeInternal
		{
			set
			{
				this._bamlNodeType = value;
			}
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x001F2C84 File Offset: 0x001F1C84
		private void AddToPropertyInfoCollection(object info)
		{
			this._properties.Add(info);
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x001F2C93 File Offset: 0x001F1C93
		public void Close()
		{
			if (this._readState != ReadState.Closed)
			{
				this._bamlRecordReader.Close();
				this._currentBamlRecord = null;
				this._bamlRecordReader = null;
				this._readState = ReadState.Closed;
			}
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x001F2CBE File Offset: 0x001F1CBE
		public bool MoveToFirstProperty()
		{
			if (this.HasProperties)
			{
				this._propertiesIndex = -1;
				return this.MoveToNextProperty();
			}
			return false;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x001F2CD8 File Offset: 0x001F1CD8
		public bool MoveToNextProperty()
		{
			if (this._propertiesIndex >= this._properties.Count - 1)
			{
				return false;
			}
			this._propertiesIndex++;
			object obj = this._properties[this._propertiesIndex];
			BamlReader.BamlPropertyInfo bamlPropertyInfo = obj as BamlReader.BamlPropertyInfo;
			if (bamlPropertyInfo != null)
			{
				this._name = bamlPropertyInfo.Name;
				this._localName = bamlPropertyInfo.LocalName;
				int num = bamlPropertyInfo.Name.LastIndexOf(".", StringComparison.Ordinal);
				if (num > 0)
				{
					this._ownerTypeName = bamlPropertyInfo.Name.Substring(0, num);
				}
				else
				{
					this._ownerTypeName = string.Empty;
				}
				this._value = bamlPropertyInfo.Value;
				this._assemblyName = bamlPropertyInfo.AssemblyName;
				this._prefix = bamlPropertyInfo.Prefix;
				this._xmlNamespace = bamlPropertyInfo.XmlNamespace;
				this._clrNamespace = bamlPropertyInfo.ClrNamespace;
				this._connectionId = 0;
				this._contentPropertyName = string.Empty;
				this._attributeUsage = bamlPropertyInfo.AttributeUsage;
				if (bamlPropertyInfo.RecordType == BamlRecordType.XmlnsProperty)
				{
					this.NodeTypeInternal = BamlNodeType.XmlnsProperty;
				}
				else if (bamlPropertyInfo.RecordType == BamlRecordType.DefAttribute)
				{
					this.NodeTypeInternal = BamlNodeType.DefAttribute;
				}
				else if (bamlPropertyInfo.RecordType == BamlRecordType.PresentationOptionsAttribute)
				{
					this.NodeTypeInternal = BamlNodeType.PresentationOptionsAttribute;
				}
				else
				{
					this.NodeTypeInternal = BamlNodeType.Property;
				}
				return true;
			}
			BamlReader.BamlContentPropertyInfo bamlContentPropertyInfo = obj as BamlReader.BamlContentPropertyInfo;
			if (bamlContentPropertyInfo != null)
			{
				this._contentPropertyName = bamlContentPropertyInfo.LocalName;
				this._connectionId = 0;
				this._prefix = string.Empty;
				this._name = bamlContentPropertyInfo.Name;
				int num2 = bamlContentPropertyInfo.Name.LastIndexOf(".", StringComparison.Ordinal);
				if (num2 > 0)
				{
					this._ownerTypeName = bamlContentPropertyInfo.Name.Substring(0, num2);
				}
				this._localName = bamlContentPropertyInfo.LocalName;
				this._ownerTypeName = string.Empty;
				this._assemblyName = bamlContentPropertyInfo.AssemblyName;
				this._xmlNamespace = string.Empty;
				this._clrNamespace = string.Empty;
				this._attributeUsage = BamlAttributeUsage.Default;
				this._value = bamlContentPropertyInfo.LocalName;
				this.NodeTypeInternal = BamlNodeType.ContentProperty;
				return true;
			}
			this._connectionId = (int)obj;
			this._contentPropertyName = string.Empty;
			this._prefix = string.Empty;
			this._name = string.Empty;
			this._localName = string.Empty;
			this._ownerTypeName = string.Empty;
			this._assemblyName = string.Empty;
			this._xmlNamespace = string.Empty;
			this._clrNamespace = string.Empty;
			this._attributeUsage = BamlAttributeUsage.Default;
			this._value = this._connectionId.ToString(CultureInfo.CurrentCulture);
			this.NodeTypeInternal = BamlNodeType.ConnectionId;
			return true;
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x001F2F5C File Offset: 0x001F1F5C
		private void GetNextRecord()
		{
			if (this._currentStaticResourceRecords != null)
			{
				List<BamlRecord> currentStaticResourceRecords = this._currentStaticResourceRecords;
				int currentStaticResourceRecordIndex = this._currentStaticResourceRecordIndex;
				this._currentStaticResourceRecordIndex = currentStaticResourceRecordIndex + 1;
				this._currentBamlRecord = currentStaticResourceRecords[currentStaticResourceRecordIndex];
				if (this._currentStaticResourceRecordIndex == this._currentStaticResourceRecords.Count)
				{
					this._currentStaticResourceRecords = null;
					this._currentStaticResourceRecordIndex = -1;
					return;
				}
			}
			else
			{
				this._currentBamlRecord = this._bamlRecordReader.GetNextRecord();
			}
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x001F2FC8 File Offset: 0x001F1FC8
		private void ReadNextRecord()
		{
			if (this._readState == ReadState.Initial)
			{
				this._bamlRecordReader.ReadVersionHeader();
			}
			bool flag = true;
			while (flag)
			{
				if (this._haveUnprocessedRecord)
				{
					this._haveUnprocessedRecord = false;
				}
				else
				{
					this.GetNextRecord();
				}
				if (this._currentBamlRecord == null)
				{
					this.NodeTypeInternal = BamlNodeType.None;
					this._readState = ReadState.EndOfFile;
					this.ClearProperties();
					return;
				}
				this._readState = ReadState.Interactive;
				flag = false;
				switch (this._currentBamlRecord.RecordType)
				{
				case BamlRecordType.DocumentStart:
					this.ReadDocumentStartRecord();
					continue;
				case BamlRecordType.DocumentEnd:
					this.ReadDocumentEndRecord();
					continue;
				case BamlRecordType.ElementStart:
				case BamlRecordType.StaticResourceStart:
					this.ReadElementStartRecord();
					continue;
				case BamlRecordType.ElementEnd:
				case BamlRecordType.StaticResourceEnd:
					this.ReadElementEndRecord();
					continue;
				case BamlRecordType.PropertyComplexStart:
				case BamlRecordType.PropertyArrayStart:
				case BamlRecordType.PropertyIListStart:
				case BamlRecordType.PropertyIDictionaryStart:
					this.ReadPropertyComplexStartRecord();
					continue;
				case BamlRecordType.PropertyComplexEnd:
				case BamlRecordType.PropertyArrayEnd:
				case BamlRecordType.PropertyIListEnd:
				case BamlRecordType.PropertyIDictionaryEnd:
					this.ReadPropertyComplexEndRecord();
					continue;
				case BamlRecordType.LiteralContent:
					this.ReadLiteralContentRecord();
					continue;
				case BamlRecordType.Text:
				case BamlRecordType.TextWithConverter:
				case BamlRecordType.TextWithId:
					this.ReadTextRecord();
					continue;
				case BamlRecordType.PIMapping:
					this.ReadPIMappingRecord();
					continue;
				case BamlRecordType.AssemblyInfo:
					this.ReadAssemblyInfoRecord();
					flag = true;
					continue;
				case BamlRecordType.TypeInfo:
				case BamlRecordType.TypeSerializerInfo:
					this.MapTable.LoadTypeInfoRecord((BamlTypeInfoRecord)this._currentBamlRecord);
					flag = true;
					continue;
				case BamlRecordType.AttributeInfo:
					this.MapTable.LoadAttributeInfoRecord((BamlAttributeInfoRecord)this._currentBamlRecord);
					flag = true;
					continue;
				case BamlRecordType.StringInfo:
					this.MapTable.LoadStringInfoRecord((BamlStringInfoRecord)this._currentBamlRecord);
					flag = true;
					continue;
				case BamlRecordType.DeferableContentStart:
					this.ReadDeferableContentRecord();
					flag = true;
					continue;
				case BamlRecordType.ConstructorParametersStart:
					this.ReadConstructorStart();
					continue;
				case BamlRecordType.ConstructorParametersEnd:
					this.ReadConstructorEnd();
					continue;
				case BamlRecordType.ConnectionId:
					this.ReadConnectionIdRecord();
					continue;
				case BamlRecordType.ContentProperty:
					this.ReadContentPropertyRecord();
					flag = true;
					continue;
				case BamlRecordType.StaticResourceId:
					this.ReadStaticResourceId();
					flag = true;
					continue;
				}
				throw new InvalidOperationException(SR.Get("ParserUnknownBaml", new object[]
				{
					((int)this._currentBamlRecord.RecordType).ToString(CultureInfo.CurrentCulture)
				}));
			}
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x001F3239 File Offset: 0x001F2239
		private void ReadProperties()
		{
			while (!this._haveUnprocessedRecord)
			{
				this.GetNextRecord();
				this.ProcessPropertyRecord();
			}
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x001F3254 File Offset: 0x001F2254
		private void ProcessPropertyRecord()
		{
			BamlRecordType recordType = this._currentBamlRecord.RecordType;
			if (recordType <= BamlRecordType.PropertyCustom)
			{
				if (recordType != BamlRecordType.Property)
				{
					if (recordType != BamlRecordType.PropertyCustom)
					{
						goto IL_182;
					}
					this.ReadPropertyCustomRecord();
					return;
				}
			}
			else
			{
				switch (recordType)
				{
				case BamlRecordType.RoutedEvent:
					this.ReadRoutedEventRecord();
					return;
				case BamlRecordType.ClrEvent:
					this.ReadClrEventRecord();
					return;
				case BamlRecordType.XmlnsProperty:
					this.ReadXmlnsPropertyRecord();
					return;
				case BamlRecordType.XmlAttribute:
				case BamlRecordType.ProcessingInstruction:
				case BamlRecordType.Comment:
				case BamlRecordType.DefTag:
				case BamlRecordType.EndAttributes:
				case BamlRecordType.PIMapping:
				case BamlRecordType.DeferableContentStart:
				case BamlRecordType.DefAttributeKeyString:
				case BamlRecordType.KeyElementEnd:
				case BamlRecordType.ConstructorParametersStart:
				case BamlRecordType.ConstructorParametersEnd:
				case BamlRecordType.ConstructorParameterType:
				case BamlRecordType.NamedElementStart:
				case BamlRecordType.StaticResourceStart:
				case BamlRecordType.StaticResourceEnd:
				case BamlRecordType.StaticResourceId:
				case BamlRecordType.TextWithId:
					goto IL_182;
				case BamlRecordType.DefAttribute:
					this.ReadDefAttributeRecord();
					return;
				case BamlRecordType.AssemblyInfo:
					this.ReadAssemblyInfoRecord();
					return;
				case BamlRecordType.TypeInfo:
				case BamlRecordType.TypeSerializerInfo:
					this.MapTable.LoadTypeInfoRecord((BamlTypeInfoRecord)this._currentBamlRecord);
					return;
				case BamlRecordType.AttributeInfo:
					this.MapTable.LoadAttributeInfoRecord((BamlAttributeInfoRecord)this._currentBamlRecord);
					return;
				case BamlRecordType.StringInfo:
					this.MapTable.LoadStringInfoRecord((BamlStringInfoRecord)this._currentBamlRecord);
					return;
				case BamlRecordType.PropertyStringReference:
					this.ReadPropertyStringRecord();
					return;
				case BamlRecordType.PropertyTypeReference:
					this.ReadPropertyTypeRecord();
					return;
				case BamlRecordType.PropertyWithExtension:
					this.ReadPropertyWithExtensionRecord();
					return;
				case BamlRecordType.PropertyWithConverter:
					break;
				case BamlRecordType.DefAttributeKeyType:
					this.ReadDefAttributeKeyTypeRecord();
					return;
				case BamlRecordType.KeyElementStart:
				{
					BamlReader.BamlKeyInfo info = this.ProcessKeyTree();
					this.AddToPropertyInfoCollection(info);
					return;
				}
				case BamlRecordType.ConnectionId:
					this.ReadConnectionIdRecord();
					return;
				case BamlRecordType.ContentProperty:
					this.ReadContentPropertyRecord();
					return;
				case BamlRecordType.PresentationOptionsAttribute:
					this.ReadPresentationOptionsAttributeRecord();
					return;
				default:
					if (recordType != BamlRecordType.PropertyWithStaticResourceId)
					{
						goto IL_182;
					}
					this.ReadPropertyWithStaticResourceIdRecord();
					return;
				}
			}
			this.ReadPropertyRecord();
			return;
			IL_182:
			this._haveUnprocessedRecord = true;
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x001F33EC File Offset: 0x001F23EC
		private void ReadXmlnsPropertyRecord()
		{
			BamlXmlnsPropertyRecord bamlXmlnsPropertyRecord = (BamlXmlnsPropertyRecord)this._currentBamlRecord;
			this._parserContext.XmlnsDictionary[bamlXmlnsPropertyRecord.Prefix] = bamlXmlnsPropertyRecord.XmlNamespace;
			this._prefixDictionary[bamlXmlnsPropertyRecord.XmlNamespace] = bamlXmlnsPropertyRecord.Prefix;
			this.AddToPropertyInfoCollection(new BamlReader.BamlPropertyInfo
			{
				Value = bamlXmlnsPropertyRecord.XmlNamespace,
				XmlNamespace = string.Empty,
				ClrNamespace = string.Empty,
				AssemblyName = string.Empty,
				Prefix = "xmlns",
				LocalName = ((bamlXmlnsPropertyRecord.Prefix == null) ? string.Empty : bamlXmlnsPropertyRecord.Prefix),
				Name = ((bamlXmlnsPropertyRecord.Prefix == null || bamlXmlnsPropertyRecord.Prefix == string.Empty) ? "xmlns" : ("xmlns:" + bamlXmlnsPropertyRecord.Prefix)),
				RecordType = BamlRecordType.XmlnsProperty
			});
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x001F34D8 File Offset: 0x001F24D8
		private void ReadPropertyRecord()
		{
			string text = ((BamlPropertyRecord)this._currentBamlRecord).Value;
			text = MarkupExtensionParser.AddEscapeToLiteralString(text);
			this.AddToPropertyInfoCollection(this.ReadPropertyRecordCore(text));
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x001F350C File Offset: 0x001F250C
		private void ReadContentPropertyRecord()
		{
			BamlReader.BamlContentPropertyInfo bamlContentPropertyInfo = new BamlReader.BamlContentPropertyInfo();
			BamlContentPropertyRecord bamlContentPropertyRecord = (BamlContentPropertyRecord)this._currentBamlRecord;
			this.SetCommonPropertyInfo(bamlContentPropertyInfo, bamlContentPropertyRecord.AttributeId);
			bamlContentPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			this.AddToPropertyInfoCollection(bamlContentPropertyInfo);
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x001F3554 File Offset: 0x001F2554
		private void ReadPropertyStringRecord()
		{
			string stringFromStringId = this.MapTable.GetStringFromStringId((int)((BamlPropertyStringReferenceRecord)this._currentBamlRecord).StringId);
			this.AddToPropertyInfoCollection(this.ReadPropertyRecordCore(stringFromStringId));
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x001F358C File Offset: 0x001F258C
		private void ReadPropertyTypeRecord()
		{
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			this.SetCommonPropertyInfo(bamlPropertyInfo, ((BamlPropertyTypeReferenceRecord)this._currentBamlRecord).AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			bamlPropertyInfo.Value = this.GetTypeValueString(((BamlPropertyTypeReferenceRecord)this._currentBamlRecord).TypeId);
			bamlPropertyInfo.AttributeUsage = BamlAttributeUsage.Default;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x001F35F4 File Offset: 0x001F25F4
		private void ReadPropertyWithExtensionRecord()
		{
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			this.SetCommonPropertyInfo(bamlPropertyInfo, ((BamlPropertyWithExtensionRecord)this._currentBamlRecord).AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			bamlPropertyInfo.Value = this.GetExtensionValueString((IOptimizedMarkupExtension)this._currentBamlRecord);
			bamlPropertyInfo.AttributeUsage = BamlAttributeUsage.Default;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x001F3658 File Offset: 0x001F2658
		private void ReadPropertyWithStaticResourceIdRecord()
		{
			BamlPropertyWithStaticResourceIdRecord bamlPropertyWithStaticResourceIdRecord = (BamlPropertyWithStaticResourceIdRecord)this._currentBamlRecord;
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			this.SetCommonPropertyInfo(bamlPropertyInfo, bamlPropertyWithStaticResourceIdRecord.AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			BamlOptimizedStaticResourceRecord optimizedMarkupExtensionRecord = (BamlOptimizedStaticResourceRecord)this._currentKeyInfo.StaticResources[(int)bamlPropertyWithStaticResourceIdRecord.StaticResourceId][0];
			bamlPropertyInfo.Value = this.GetExtensionValueString(optimizedMarkupExtensionRecord);
			bamlPropertyInfo.AttributeUsage = BamlAttributeUsage.Default;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x001F36D4 File Offset: 0x001F26D4
		private BamlReader.BamlPropertyInfo ReadPropertyRecordCore(string value)
		{
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			this.SetCommonPropertyInfo(bamlPropertyInfo, ((BamlPropertyRecord)this._currentBamlRecord).AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			bamlPropertyInfo.Value = value;
			return bamlPropertyInfo;
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x001F3718 File Offset: 0x001F2718
		private void ReadPropertyCustomRecord()
		{
			BamlReader.BamlPropertyInfo propertyCustomRecordInfo = this.GetPropertyCustomRecordInfo();
			this.AddToPropertyInfoCollection(propertyCustomRecordInfo);
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x001F3734 File Offset: 0x001F2734
		private BamlReader.BamlPropertyInfo GetPropertyCustomRecordInfo()
		{
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			BamlAttributeInfoRecord bamlAttributeInfoRecord = this.SetCommonPropertyInfo(bamlPropertyInfo, ((BamlPropertyCustomRecord)this._currentBamlRecord).AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			bamlPropertyInfo.AttributeUsage = BamlAttributeUsage.Default;
			BamlPropertyCustomRecord bamlPropertyCustomRecord = (BamlPropertyCustomRecord)this._currentBamlRecord;
			if (bamlAttributeInfoRecord.DP == null && bamlAttributeInfoRecord.PropInfo == null)
			{
				bamlAttributeInfoRecord.DP = this.MapTable.GetDependencyProperty(bamlAttributeInfoRecord);
				if (bamlAttributeInfoRecord.OwnerType == null)
				{
					throw new InvalidOperationException(SR.Get("BamlReaderNoOwnerType", new object[]
					{
						bamlAttributeInfoRecord.Name,
						this.AssemblyName
					}));
				}
				if (bamlAttributeInfoRecord.DP == null)
				{
					try
					{
						bamlAttributeInfoRecord.PropInfo = bamlAttributeInfoRecord.OwnerType.GetProperty(bamlAttributeInfoRecord.Name, BindingFlags.Instance | BindingFlags.Public);
					}
					catch (AmbiguousMatchException)
					{
						PropertyInfo[] properties = bamlAttributeInfoRecord.OwnerType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
						for (int i = 0; i < properties.Length; i++)
						{
							if (properties[i].Name == bamlAttributeInfoRecord.Name)
							{
								bamlAttributeInfoRecord.PropInfo = properties[i];
								break;
							}
						}
					}
					if (bamlAttributeInfoRecord.PropInfo == null)
					{
						throw new InvalidOperationException(SR.Get("ParserCantGetDPOrPi", new object[]
						{
							bamlPropertyInfo.Name
						}));
					}
				}
			}
			Type propertyType = bamlAttributeInfoRecord.GetPropertyType();
			string name = bamlAttributeInfoRecord.Name;
			if (bamlPropertyCustomRecord.SerializerTypeId == 137)
			{
				Type type = null;
				this._propertyDP = this._bamlRecordReader.GetCustomDependencyPropertyValue(bamlPropertyCustomRecord, out type);
				type = ((type == null) ? this._propertyDP.OwnerType : type);
				bamlPropertyInfo.Value = type.Name + "." + this._propertyDP.Name;
				string xmlNamespace = this._parserContext.XamlTypeMapper.GetXmlNamespace(type.Namespace, type.Assembly.FullName);
				string xmlnsPrefix = this.GetXmlnsPrefix(xmlNamespace);
				if (xmlnsPrefix != string.Empty)
				{
					bamlPropertyInfo.Value = xmlnsPrefix + ":" + bamlPropertyInfo.Value;
				}
				if (!this._propertyDP.PropertyType.IsEnum)
				{
					this._propertyDP = null;
				}
			}
			else
			{
				if (this._propertyDP != null)
				{
					propertyType = this._propertyDP.PropertyType;
					name = this._propertyDP.Name;
					this._propertyDP = null;
				}
				object customValue = this._bamlRecordReader.GetCustomValue(bamlPropertyCustomRecord, propertyType, name);
				TypeConverter converter = TypeDescriptor.GetConverter(customValue.GetType());
				bamlPropertyInfo.Value = converter.ConvertToString(null, TypeConverterHelper.InvariantEnglishUS, customValue);
			}
			return bamlPropertyInfo;
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x001F39D4 File Offset: 0x001F29D4
		private void ReadDefAttributeRecord()
		{
			BamlDefAttributeRecord bamlDefAttributeRecord = (BamlDefAttributeRecord)this._currentBamlRecord;
			bamlDefAttributeRecord.Name = this.MapTable.GetStringFromStringId((int)bamlDefAttributeRecord.NameId);
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			bamlPropertyInfo.Value = bamlDefAttributeRecord.Value;
			bamlPropertyInfo.AssemblyName = string.Empty;
			bamlPropertyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			bamlPropertyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
			bamlPropertyInfo.ClrNamespace = string.Empty;
			bamlPropertyInfo.Name = bamlDefAttributeRecord.Name;
			bamlPropertyInfo.LocalName = bamlPropertyInfo.Name;
			bamlPropertyInfo.RecordType = BamlRecordType.DefAttribute;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x001F3A74 File Offset: 0x001F2A74
		private void ReadPresentationOptionsAttributeRecord()
		{
			BamlPresentationOptionsAttributeRecord bamlPresentationOptionsAttributeRecord = (BamlPresentationOptionsAttributeRecord)this._currentBamlRecord;
			bamlPresentationOptionsAttributeRecord.Name = this.MapTable.GetStringFromStringId((int)bamlPresentationOptionsAttributeRecord.NameId);
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			bamlPropertyInfo.Value = bamlPresentationOptionsAttributeRecord.Value;
			bamlPropertyInfo.AssemblyName = string.Empty;
			bamlPropertyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml/presentation/options"];
			bamlPropertyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation/options";
			bamlPropertyInfo.ClrNamespace = string.Empty;
			bamlPropertyInfo.Name = bamlPresentationOptionsAttributeRecord.Name;
			bamlPropertyInfo.LocalName = bamlPropertyInfo.Name;
			bamlPropertyInfo.RecordType = BamlRecordType.PresentationOptionsAttribute;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x001F3B14 File Offset: 0x001F2B14
		private void ReadDefAttributeKeyTypeRecord()
		{
			BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = (BamlDefAttributeKeyTypeRecord)this._currentBamlRecord;
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			bamlPropertyInfo.Value = this.GetTypeValueString(bamlDefAttributeKeyTypeRecord.TypeId);
			bamlPropertyInfo.AssemblyName = string.Empty;
			bamlPropertyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			bamlPropertyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
			bamlPropertyInfo.ClrNamespace = string.Empty;
			bamlPropertyInfo.Name = "Key";
			bamlPropertyInfo.LocalName = bamlPropertyInfo.Name;
			bamlPropertyInfo.RecordType = BamlRecordType.DefAttribute;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x001F3BA2 File Offset: 0x001F2BA2
		private void ReadDeferableContentRecord()
		{
			this._deferableContentBlockDepth = this._nodeStack.Count;
			this._deferableContentPosition = this.ReadDeferKeys();
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x001F3BC4 File Offset: 0x001F2BC4
		private long ReadDeferKeys()
		{
			long result = -1L;
			this._deferKeys = new List<BamlReader.BamlKeyInfo>();
			while (!this._haveUnprocessedRecord)
			{
				this.GetNextRecord();
				this.ProcessDeferKey();
				if (!this._haveUnprocessedRecord)
				{
					result = this._bamlRecordReader.StreamPosition;
				}
			}
			return result;
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x001F3C0C File Offset: 0x001F2C0C
		private void ProcessDeferKey()
		{
			BamlRecordType recordType = this._currentBamlRecord.RecordType;
			switch (recordType)
			{
			case BamlRecordType.DefAttributeKeyString:
			{
				BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord = this._currentBamlRecord as BamlDefAttributeKeyStringRecord;
				if (bamlDefAttributeKeyStringRecord != null)
				{
					BamlReader.BamlKeyInfo bamlKeyInfo = this.CheckForSharedness();
					if (bamlKeyInfo != null)
					{
						this._deferKeys.Add(bamlKeyInfo);
					}
					bamlDefAttributeKeyStringRecord.Value = this.MapTable.GetStringFromStringId((int)bamlDefAttributeKeyStringRecord.ValueId);
					bamlKeyInfo = new BamlReader.BamlKeyInfo();
					bamlKeyInfo.Value = bamlDefAttributeKeyStringRecord.Value;
					bamlKeyInfo.AssemblyName = string.Empty;
					bamlKeyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
					bamlKeyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
					bamlKeyInfo.ClrNamespace = string.Empty;
					bamlKeyInfo.Name = "Key";
					bamlKeyInfo.LocalName = bamlKeyInfo.Name;
					bamlKeyInfo.RecordType = BamlRecordType.DefAttribute;
					bamlKeyInfo.Offset = ((IBamlDictionaryKey)bamlDefAttributeKeyStringRecord).ValuePosition;
					this._deferKeys.Add(bamlKeyInfo);
					return;
				}
				break;
			}
			case BamlRecordType.DefAttributeKeyType:
			{
				BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = this._currentBamlRecord as BamlDefAttributeKeyTypeRecord;
				if (bamlDefAttributeKeyTypeRecord != null)
				{
					string text = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
					string text2;
					if (text != string.Empty)
					{
						text2 = "{" + text + ":Type ";
					}
					else
					{
						text2 = "{Type ";
					}
					BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(bamlDefAttributeKeyTypeRecord.TypeId);
					string text3 = typeInfoFromId.TypeFullName;
					text3 = text3.Substring(text3.LastIndexOf(".", StringComparison.Ordinal) + 1);
					string text4;
					string text5;
					string text6;
					this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text4, out text5, out text6);
					if (text5 != string.Empty)
					{
						text3 = string.Concat(new string[]
						{
							text2,
							text5,
							":",
							text3,
							"}"
						});
					}
					else
					{
						text3 = text2 + text3 + "}";
					}
					BamlReader.BamlKeyInfo bamlKeyInfo2 = new BamlReader.BamlKeyInfo();
					bamlKeyInfo2.Value = text3;
					bamlKeyInfo2.AssemblyName = string.Empty;
					bamlKeyInfo2.Prefix = text;
					bamlKeyInfo2.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
					bamlKeyInfo2.ClrNamespace = string.Empty;
					bamlKeyInfo2.Name = "Key";
					bamlKeyInfo2.LocalName = bamlKeyInfo2.Name;
					bamlKeyInfo2.RecordType = BamlRecordType.DefAttribute;
					bamlKeyInfo2.Offset = ((IBamlDictionaryKey)bamlDefAttributeKeyTypeRecord).ValuePosition;
					this._deferKeys.Add(bamlKeyInfo2);
					return;
				}
				break;
			}
			case BamlRecordType.KeyElementStart:
			{
				BamlReader.BamlKeyInfo bamlKeyInfo3 = this.CheckForSharedness();
				if (bamlKeyInfo3 != null)
				{
					this._deferKeys.Add(bamlKeyInfo3);
				}
				bamlKeyInfo3 = this.ProcessKeyTree();
				this._deferKeys.Add(bamlKeyInfo3);
				return;
			}
			default:
				if (recordType == BamlRecordType.StaticResourceStart || recordType == BamlRecordType.OptimizedStaticResource)
				{
					List<BamlRecord> list = new List<BamlRecord>();
					this._currentBamlRecord.Pin();
					list.Add(this._currentBamlRecord);
					if (this._currentBamlRecord.RecordType == BamlRecordType.StaticResourceStart)
					{
						this.ProcessStaticResourceTree(list);
					}
					this._deferKeys[this._deferKeys.Count - 1].StaticResources.Add(list);
					return;
				}
				this._haveUnprocessedRecord = true;
				break;
			}
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x001F3EF8 File Offset: 0x001F2EF8
		private BamlReader.BamlKeyInfo CheckForSharedness()
		{
			IBamlDictionaryKey bamlDictionaryKey = (IBamlDictionaryKey)this._currentBamlRecord;
			if (!bamlDictionaryKey.SharedSet)
			{
				return null;
			}
			BamlReader.BamlKeyInfo bamlKeyInfo = new BamlReader.BamlKeyInfo();
			bamlKeyInfo.Value = bamlDictionaryKey.Shared.ToString();
			bamlKeyInfo.AssemblyName = string.Empty;
			bamlKeyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			bamlKeyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
			bamlKeyInfo.ClrNamespace = string.Empty;
			bamlKeyInfo.Name = "Shared";
			bamlKeyInfo.LocalName = bamlKeyInfo.Name;
			bamlKeyInfo.RecordType = BamlRecordType.DefAttribute;
			bamlKeyInfo.Offset = bamlDictionaryKey.ValuePosition;
			return bamlKeyInfo;
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x001F3F98 File Offset: 0x001F2F98
		private BamlReader.BamlKeyInfo ProcessKeyTree()
		{
			BamlKeyElementStartRecord bamlKeyElementStartRecord = this._currentBamlRecord as BamlKeyElementStartRecord;
			BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(bamlKeyElementStartRecord.TypeId);
			string text = typeInfoFromId.TypeFullName;
			text = text.Substring(text.LastIndexOf(".", StringComparison.Ordinal) + 1);
			string text2;
			string text3;
			string text4;
			this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text2, out text3, out text4);
			if (text3 != string.Empty)
			{
				text = string.Concat(new string[]
				{
					"{",
					text3,
					":",
					text,
					" "
				});
			}
			else
			{
				text = "{" + text + " ";
			}
			bool flag = true;
			Stack stack = new Stack();
			Stack stack2 = new Stack();
			Stack stack3 = new Stack();
			stack.Push(false);
			stack2.Push(false);
			stack3.Push(false);
			while (flag)
			{
				if (!this._haveUnprocessedRecord)
				{
					this.GetNextRecord();
				}
				else
				{
					this._haveUnprocessedRecord = false;
				}
				BamlRecordType recordType = this._currentBamlRecord.RecordType;
				switch (recordType)
				{
				case BamlRecordType.ElementStart:
				{
					if ((bool)stack3.Peek())
					{
						text += ", ";
					}
					if ((bool)stack2.Peek())
					{
						stack3.Pop();
						stack3.Push(true);
					}
					stack.Push(false);
					stack2.Push(false);
					stack3.Push(false);
					BamlElementStartRecord bamlElementStartRecord = this._currentBamlRecord as BamlElementStartRecord;
					BamlTypeInfoRecord typeInfoFromId2 = this.MapTable.GetTypeInfoFromId(bamlElementStartRecord.TypeId);
					string text5 = typeInfoFromId2.TypeFullName;
					text5 = text5.Substring(text5.LastIndexOf(".", StringComparison.Ordinal) + 1);
					this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId2, out text2, out text3, out text4);
					if (text3 != string.Empty)
					{
						text = string.Concat(new string[]
						{
							text,
							"{",
							text3,
							":",
							text5,
							" "
						});
						continue;
					}
					text = "{" + text5 + " ";
					continue;
				}
				case BamlRecordType.ElementEnd:
					stack.Pop();
					stack2.Pop();
					stack3.Pop();
					text += "}";
					continue;
				case BamlRecordType.Property:
					break;
				case BamlRecordType.PropertyCustom:
				{
					BamlReader.BamlPropertyInfo propertyCustomRecordInfo = this.GetPropertyCustomRecordInfo();
					if ((bool)stack.Pop())
					{
						text += ", ";
					}
					text = text + propertyCustomRecordInfo.LocalName + "=" + propertyCustomRecordInfo.Value;
					stack.Push(true);
					continue;
				}
				case BamlRecordType.PropertyComplexStart:
				{
					this.ReadPropertyComplexStartRecord();
					BamlReader.BamlNodeInfo bamlNodeInfo = (BamlReader.BamlNodeInfo)this._nodeStack.Pop();
					if ((bool)stack.Pop())
					{
						text += ", ";
					}
					text = text + bamlNodeInfo.LocalName + "=";
					stack.Push(true);
					continue;
				}
				case BamlRecordType.PropertyComplexEnd:
					continue;
				default:
				{
					if (recordType != BamlRecordType.Text)
					{
						switch (recordType)
						{
						case BamlRecordType.AssemblyInfo:
							this.ReadAssemblyInfoRecord();
							continue;
						case BamlRecordType.TypeInfo:
						case BamlRecordType.TypeSerializerInfo:
							this.MapTable.LoadTypeInfoRecord((BamlTypeInfoRecord)this._currentBamlRecord);
							continue;
						case BamlRecordType.AttributeInfo:
							this.MapTable.LoadAttributeInfoRecord((BamlAttributeInfoRecord)this._currentBamlRecord);
							continue;
						case BamlRecordType.StringInfo:
							this.MapTable.LoadStringInfoRecord((BamlStringInfoRecord)this._currentBamlRecord);
							continue;
						case BamlRecordType.PropertyStringReference:
						{
							string stringFromStringId = this.MapTable.GetStringFromStringId((int)((BamlPropertyStringReferenceRecord)this._currentBamlRecord).StringId);
							BamlReader.BamlPropertyInfo bamlPropertyInfo = this.ReadPropertyRecordCore(stringFromStringId);
							if ((bool)stack.Pop())
							{
								text += ", ";
							}
							text = text + bamlPropertyInfo.LocalName + "=" + bamlPropertyInfo.Value;
							stack.Push(true);
							continue;
						}
						case BamlRecordType.PropertyTypeReference:
						{
							string typeValueString = this.GetTypeValueString(((BamlPropertyTypeReferenceRecord)this._currentBamlRecord).TypeId);
							string attributeNameFromId = this.MapTable.GetAttributeNameFromId(((BamlPropertyTypeReferenceRecord)this._currentBamlRecord).AttributeId);
							if ((bool)stack.Pop())
							{
								text += ", ";
							}
							text = text + attributeNameFromId + "=" + typeValueString;
							stack.Push(true);
							continue;
						}
						case BamlRecordType.PropertyWithExtension:
						{
							string extensionValueString = this.GetExtensionValueString((BamlPropertyWithExtensionRecord)this._currentBamlRecord);
							string attributeNameFromId2 = this.MapTable.GetAttributeNameFromId(((BamlPropertyWithExtensionRecord)this._currentBamlRecord).AttributeId);
							if ((bool)stack.Pop())
							{
								text += ", ";
							}
							text = text + attributeNameFromId2 + "=" + extensionValueString;
							stack.Push(true);
							continue;
						}
						case BamlRecordType.PropertyWithConverter:
							goto IL_4BE;
						case BamlRecordType.KeyElementEnd:
							text += "}";
							flag = false;
							this._haveUnprocessedRecord = false;
							continue;
						case BamlRecordType.ConstructorParametersStart:
							stack2.Pop();
							stack2.Push(true);
							continue;
						case BamlRecordType.ConstructorParametersEnd:
							stack2.Pop();
							stack2.Push(false);
							stack3.Pop();
							stack3.Push(false);
							continue;
						case BamlRecordType.ConstructorParameterType:
						{
							if ((bool)stack3.Peek())
							{
								text += ", ";
							}
							if ((bool)stack2.Peek())
							{
								stack3.Pop();
								stack3.Push(true);
							}
							BamlConstructorParameterTypeRecord bamlConstructorParameterTypeRecord = this._currentBamlRecord as BamlConstructorParameterTypeRecord;
							text += this.GetTypeValueString(bamlConstructorParameterTypeRecord.TypeId);
							continue;
						}
						case BamlRecordType.TextWithId:
							goto IL_249;
						}
						throw new InvalidOperationException(SR.Get("ParserUnknownBaml", new object[]
						{
							((int)this._currentBamlRecord.RecordType).ToString(CultureInfo.CurrentCulture)
						}));
					}
					IL_249:
					BamlTextWithIdRecord bamlTextWithIdRecord = this._currentBamlRecord as BamlTextWithIdRecord;
					if (bamlTextWithIdRecord != null)
					{
						bamlTextWithIdRecord.Value = this.MapTable.GetStringFromStringId((int)bamlTextWithIdRecord.ValueId);
					}
					string str = this.EscapeString(((BamlTextRecord)this._currentBamlRecord).Value);
					if ((bool)stack3.Peek())
					{
						text += ", ";
					}
					text += str;
					if ((bool)stack2.Peek())
					{
						stack3.Pop();
						stack3.Push(true);
						continue;
					}
					continue;
				}
				}
				IL_4BE:
				string value = ((BamlPropertyRecord)this._currentBamlRecord).Value;
				BamlReader.BamlPropertyInfo bamlPropertyInfo2 = this.ReadPropertyRecordCore(value);
				if ((bool)stack.Pop())
				{
					text += ", ";
				}
				text = text + bamlPropertyInfo2.LocalName + "=" + bamlPropertyInfo2.Value;
				stack.Push(true);
			}
			BamlReader.BamlKeyInfo bamlKeyInfo = new BamlReader.BamlKeyInfo();
			bamlKeyInfo.Value = text;
			bamlKeyInfo.AssemblyName = string.Empty;
			bamlKeyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			bamlKeyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
			bamlKeyInfo.ClrNamespace = string.Empty;
			bamlKeyInfo.Name = "Key";
			bamlKeyInfo.LocalName = bamlKeyInfo.Name;
			bamlKeyInfo.RecordType = BamlRecordType.DefAttribute;
			bamlKeyInfo.Offset = ((IBamlDictionaryKey)bamlKeyElementStartRecord).ValuePosition;
			return bamlKeyInfo;
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x001F471C File Offset: 0x001F371C
		private void ProcessStaticResourceTree(List<BamlRecord> srRecords)
		{
			bool flag = true;
			while (flag)
			{
				if (this._haveUnprocessedRecord)
				{
					this._haveUnprocessedRecord = false;
				}
				else
				{
					this.GetNextRecord();
				}
				this._currentBamlRecord.Pin();
				srRecords.Add(this._currentBamlRecord);
				if (this._currentBamlRecord.RecordType == BamlRecordType.StaticResourceEnd)
				{
					flag = false;
				}
			}
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x001F4770 File Offset: 0x001F3770
		private void ReadStaticResourceId()
		{
			BamlStaticResourceIdRecord bamlStaticResourceIdRecord = (BamlStaticResourceIdRecord)this._currentBamlRecord;
			this._currentStaticResourceRecords = this._currentKeyInfo.StaticResources[(int)bamlStaticResourceIdRecord.StaticResourceId];
			this._currentStaticResourceRecordIndex = 0;
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x001F47AC File Offset: 0x001F37AC
		private string EscapeString(string value)
		{
			StringBuilder stringBuilder = null;
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == '{' || value[i] == '}')
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(value.Length + 2);
						stringBuilder.Append(value, 0, i);
					}
					stringBuilder.Append('\\');
				}
				if (stringBuilder != null)
				{
					stringBuilder.Append(value[i]);
				}
			}
			if (stringBuilder == null)
			{
				return value;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x001F4822 File Offset: 0x001F3822
		private void ReadRoutedEventRecord()
		{
			throw new InvalidOperationException(SR.Get("ParserBamlEvent", new object[]
			{
				string.Empty
			}));
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x001F4822 File Offset: 0x001F3822
		private void ReadClrEventRecord()
		{
			throw new InvalidOperationException(SR.Get("ParserBamlEvent", new object[]
			{
				string.Empty
			}));
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x001F4844 File Offset: 0x001F3844
		private void ReadDocumentStartRecord()
		{
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.StartDocument;
			BamlDocumentStartRecord bamlDocumentStartRecord = (BamlDocumentStartRecord)this._currentBamlRecord;
			this._parserContext.IsDebugBamlStream = bamlDocumentStartRecord.DebugBaml;
			BamlReader.BamlNodeInfo bamlNodeInfo = new BamlReader.BamlNodeInfo();
			bamlNodeInfo.RecordType = BamlRecordType.DocumentStart;
			this._nodeStack.Push(bamlNodeInfo);
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x001F4894 File Offset: 0x001F3894
		private void ReadDocumentEndRecord()
		{
			BamlReader.BamlNodeInfo bamlNodeInfo = (BamlReader.BamlNodeInfo)this._nodeStack.Pop();
			if (bamlNodeInfo.RecordType != BamlRecordType.DocumentStart)
			{
				throw new InvalidOperationException(SR.Get("BamlScopeError", new object[]
				{
					bamlNodeInfo.RecordType.ToString(),
					"DocumentEnd"
				}));
			}
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.EndDocument;
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x001F4900 File Offset: 0x001F3900
		private void ReadAssemblyInfoRecord()
		{
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = (BamlAssemblyInfoRecord)this._currentBamlRecord;
			this.MapTable.LoadAssemblyInfoRecord(bamlAssemblyInfoRecord);
			Assembly assembly = Assembly.Load(bamlAssemblyInfoRecord.AssemblyFullName);
			foreach (XmlnsDefinitionAttribute xmlnsDefinitionAttribute in assembly.GetCustomAttributes(typeof(XmlnsDefinitionAttribute), true))
			{
				this.SetXmlNamespace(xmlnsDefinitionAttribute.ClrNamespace, assembly.FullName, xmlnsDefinitionAttribute.XmlNamespace);
			}
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x001F4978 File Offset: 0x001F3978
		private void ReadPIMappingRecord()
		{
			BamlPIMappingRecord bamlPIMappingRecord = (BamlPIMappingRecord)this._currentBamlRecord;
			BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(bamlPIMappingRecord.AssemblyId);
			if (assemblyInfoFromId == null)
			{
				throw new InvalidOperationException(SR.Get("ParserMapPIMissingAssembly"));
			}
			if (!this._parserContext.XamlTypeMapper.PITable.Contains(bamlPIMappingRecord.XmlNamespace))
			{
				this._parserContext.XamlTypeMapper.AddMappingProcessingInstruction(bamlPIMappingRecord.XmlNamespace, bamlPIMappingRecord.ClrNamespace, assemblyInfoFromId.AssemblyFullName);
			}
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.PIMapping;
			this._name = "Mapping";
			this._localName = this._name;
			this._ownerTypeName = string.Empty;
			this._xmlNamespace = bamlPIMappingRecord.XmlNamespace;
			this._clrNamespace = bamlPIMappingRecord.ClrNamespace;
			this._assemblyName = assemblyInfoFromId.AssemblyFullName;
			StringBuilder stringBuilder = new StringBuilder(100);
			stringBuilder.Append("XmlNamespace=\"");
			stringBuilder.Append(this._xmlNamespace);
			stringBuilder.Append("\" ClrNamespace=\"");
			stringBuilder.Append(this._clrNamespace);
			stringBuilder.Append("\" Assembly=\"");
			stringBuilder.Append(this._assemblyName);
			stringBuilder.Append("\"");
			this._value = stringBuilder.ToString();
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x001F4AB4 File Offset: 0x001F3AB4
		private void ReadLiteralContentRecord()
		{
			this.ClearProperties();
			BamlLiteralContentRecord bamlLiteralContentRecord = (BamlLiteralContentRecord)this._currentBamlRecord;
			this.NodeTypeInternal = BamlNodeType.LiteralContent;
			this._value = bamlLiteralContentRecord.Value;
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x001F4AE8 File Offset: 0x001F3AE8
		private void ReadConnectionIdRecord()
		{
			BamlConnectionIdRecord bamlConnectionIdRecord = (BamlConnectionIdRecord)this._currentBamlRecord;
			this.AddToPropertyInfoCollection(bamlConnectionIdRecord.ConnectionId);
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x001F4B14 File Offset: 0x001F3B14
		private void ReadElementStartRecord()
		{
			this.ClearProperties();
			this._propertyDP = null;
			this._parserContext.PushScope();
			this._prefixDictionary.PushScope();
			BamlElementStartRecord bamlElementStartRecord = (BamlElementStartRecord)this._currentBamlRecord;
			BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(bamlElementStartRecord.TypeId);
			this.NodeTypeInternal = BamlNodeType.StartElement;
			this._name = typeInfoFromId.TypeFullName;
			this._localName = this._name.Substring(this._name.LastIndexOf(".", StringComparison.Ordinal) + 1);
			this._ownerTypeName = string.Empty;
			this._clrNamespace = typeInfoFromId.ClrNamespace;
			this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out this._assemblyName, out this._prefix, out this._xmlNamespace);
			BamlReader.BamlNodeInfo bamlNodeInfo = new BamlReader.BamlNodeInfo();
			bamlNodeInfo.Name = this._name;
			bamlNodeInfo.LocalName = this._localName;
			bamlNodeInfo.AssemblyName = this._assemblyName;
			bamlNodeInfo.Prefix = this._prefix;
			bamlNodeInfo.ClrNamespace = this._clrNamespace;
			bamlNodeInfo.XmlNamespace = this._xmlNamespace;
			bamlNodeInfo.RecordType = BamlRecordType.ElementStart;
			this._useTypeConverter = bamlElementStartRecord.CreateUsingTypeConverter;
			this._isInjected = bamlElementStartRecord.IsInjected;
			if (this._deferableContentBlockDepth == this._nodeStack.Count)
			{
				int num = (int)(this._bamlRecordReader.StreamPosition - this._deferableContentPosition);
				num -= bamlElementStartRecord.RecordSize + 1;
				if (BamlRecordHelper.HasDebugExtensionRecord(this._parserContext.IsDebugBamlStream, bamlElementStartRecord))
				{
					BamlRecord next = bamlElementStartRecord.Next;
					num -= next.RecordSize + 1;
				}
				this.InsertDeferedKey(num);
			}
			this._nodeStack.Push(bamlNodeInfo);
			this.ReadProperties();
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x001F4CA8 File Offset: 0x001F3CA8
		private void ReadElementEndRecord()
		{
			if (this._deferableContentBlockDepth == this._nodeStack.Count)
			{
				this._deferableContentBlockDepth = -1;
				this._deferableContentPosition = -1L;
			}
			BamlReader.BamlNodeInfo bamlNodeInfo = (BamlReader.BamlNodeInfo)this._nodeStack.Pop();
			if (bamlNodeInfo.RecordType != BamlRecordType.ElementStart)
			{
				throw new InvalidOperationException(SR.Get("BamlScopeError", new object[]
				{
					this._currentBamlRecord.RecordType.ToString(),
					BamlRecordType.ElementEnd.ToString()
				}));
			}
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.EndElement;
			this._name = bamlNodeInfo.Name;
			this._localName = bamlNodeInfo.LocalName;
			this._ownerTypeName = string.Empty;
			this._assemblyName = bamlNodeInfo.AssemblyName;
			this._prefix = bamlNodeInfo.Prefix;
			this._xmlNamespace = bamlNodeInfo.XmlNamespace;
			this._clrNamespace = bamlNodeInfo.ClrNamespace;
			this._parserContext.PopScope();
			this._prefixDictionary.PopScope();
			this.ReadProperties();
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x001F4DB4 File Offset: 0x001F3DB4
		private void ReadPropertyComplexStartRecord()
		{
			this.ClearProperties();
			this._parserContext.PushScope();
			this._prefixDictionary.PushScope();
			BamlReader.BamlNodeInfo bamlNodeInfo = new BamlReader.BamlNodeInfo();
			this.SetCommonPropertyInfo(bamlNodeInfo, ((BamlPropertyComplexStartRecord)this._currentBamlRecord).AttributeId);
			this.NodeTypeInternal = BamlNodeType.StartComplexProperty;
			this._localName = bamlNodeInfo.LocalName;
			int num = bamlNodeInfo.Name.LastIndexOf(".", StringComparison.Ordinal);
			if (num > 0)
			{
				this._ownerTypeName = bamlNodeInfo.Name.Substring(0, num);
			}
			else
			{
				this._ownerTypeName = string.Empty;
			}
			this._name = bamlNodeInfo.Name;
			this._clrNamespace = bamlNodeInfo.ClrNamespace;
			this._assemblyName = bamlNodeInfo.AssemblyName;
			this._prefix = bamlNodeInfo.Prefix;
			this._xmlNamespace = bamlNodeInfo.XmlNamespace;
			bamlNodeInfo.RecordType = this._currentBamlRecord.RecordType;
			this._nodeStack.Push(bamlNodeInfo);
			this.ReadProperties();
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x001F4EA4 File Offset: 0x001F3EA4
		private void ReadPropertyComplexEndRecord()
		{
			BamlReader.BamlNodeInfo bamlNodeInfo = (BamlReader.BamlNodeInfo)this._nodeStack.Pop();
			BamlRecordType bamlRecordType;
			switch (bamlNodeInfo.RecordType)
			{
			case BamlRecordType.PropertyComplexStart:
				bamlRecordType = BamlRecordType.PropertyComplexEnd;
				goto IL_53;
			case BamlRecordType.PropertyArrayStart:
				bamlRecordType = BamlRecordType.PropertyArrayEnd;
				goto IL_53;
			case BamlRecordType.PropertyIListStart:
				bamlRecordType = BamlRecordType.PropertyIListEnd;
				goto IL_53;
			case BamlRecordType.PropertyIDictionaryStart:
				bamlRecordType = BamlRecordType.PropertyIDictionaryEnd;
				goto IL_53;
			}
			bamlRecordType = BamlRecordType.Unknown;
			IL_53:
			if (this._currentBamlRecord.RecordType != bamlRecordType)
			{
				throw new InvalidOperationException(SR.Get("BamlScopeError", new object[]
				{
					this._currentBamlRecord.RecordType.ToString(),
					bamlRecordType.ToString()
				}));
			}
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.EndComplexProperty;
			this._name = bamlNodeInfo.Name;
			this._localName = bamlNodeInfo.LocalName;
			int num = bamlNodeInfo.Name.LastIndexOf(".", StringComparison.Ordinal);
			if (num > 0)
			{
				this._ownerTypeName = bamlNodeInfo.Name.Substring(0, num);
			}
			else
			{
				this._ownerTypeName = string.Empty;
			}
			this._assemblyName = bamlNodeInfo.AssemblyName;
			this._prefix = bamlNodeInfo.Prefix;
			this._xmlNamespace = bamlNodeInfo.XmlNamespace;
			this._clrNamespace = bamlNodeInfo.ClrNamespace;
			this._parserContext.PopScope();
			this._prefixDictionary.PopScope();
			this.ReadProperties();
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x001F4FFC File Offset: 0x001F3FFC
		private void ReadTextRecord()
		{
			this.ClearProperties();
			BamlTextWithIdRecord bamlTextWithIdRecord = this._currentBamlRecord as BamlTextWithIdRecord;
			if (bamlTextWithIdRecord != null)
			{
				bamlTextWithIdRecord.Value = this.MapTable.GetStringFromStringId((int)bamlTextWithIdRecord.ValueId);
			}
			BamlTextWithConverterRecord bamlTextWithConverterRecord = this._currentBamlRecord as BamlTextWithConverterRecord;
			if (bamlTextWithConverterRecord != null)
			{
				short converterTypeId = bamlTextWithConverterRecord.ConverterTypeId;
				Type typeFromId = this.MapTable.GetTypeFromId(converterTypeId);
				this._typeConverterAssemblyName = typeFromId.Assembly.FullName;
				this._typeConverterName = typeFromId.FullName;
			}
			this.NodeTypeInternal = BamlNodeType.Text;
			this._prefix = string.Empty;
			this._value = ((BamlTextRecord)this._currentBamlRecord).Value;
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x001F50A0 File Offset: 0x001F40A0
		private void ReadConstructorStart()
		{
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.StartConstructor;
			BamlReader.BamlNodeInfo bamlNodeInfo = new BamlReader.BamlNodeInfo();
			bamlNodeInfo.RecordType = BamlRecordType.ConstructorParametersStart;
			this._nodeStack.Push(bamlNodeInfo);
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x001F50D8 File Offset: 0x001F40D8
		private void ReadConstructorEnd()
		{
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.EndConstructor;
			if (((BamlReader.BamlNodeInfo)this._nodeStack.Pop()).RecordType != BamlRecordType.ConstructorParametersStart)
			{
				throw new InvalidOperationException(SR.Get("BamlScopeError", new object[]
				{
					this._currentBamlRecord.RecordType.ToString(),
					BamlRecordType.ConstructorParametersEnd.ToString()
				}));
			}
			this.ReadProperties();
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x001F5158 File Offset: 0x001F4158
		private void InsertDeferedKey(int valueOffset)
		{
			if (this._deferKeys == null)
			{
				return;
			}
			BamlReader.BamlKeyInfo bamlKeyInfo = this._deferKeys[0];
			while (bamlKeyInfo.Offset == valueOffset)
			{
				this._currentKeyInfo = bamlKeyInfo;
				BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
				bamlPropertyInfo.Value = bamlKeyInfo.Value;
				bamlPropertyInfo.AssemblyName = string.Empty;
				bamlPropertyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
				bamlPropertyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
				bamlPropertyInfo.ClrNamespace = string.Empty;
				bamlPropertyInfo.Name = bamlKeyInfo.Name;
				bamlPropertyInfo.LocalName = bamlPropertyInfo.Name;
				bamlPropertyInfo.RecordType = BamlRecordType.DefAttribute;
				this.AddToPropertyInfoCollection(bamlPropertyInfo);
				this._deferKeys.RemoveAt(0);
				if (this._deferKeys.Count <= 0)
				{
					return;
				}
				bamlKeyInfo = this._deferKeys[0];
			}
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x001F5230 File Offset: 0x001F4230
		private void ClearProperties()
		{
			this._value = string.Empty;
			this._prefix = string.Empty;
			this._name = string.Empty;
			this._localName = string.Empty;
			this._ownerTypeName = string.Empty;
			this._assemblyName = string.Empty;
			this._xmlNamespace = string.Empty;
			this._clrNamespace = string.Empty;
			this._connectionId = 0;
			this._contentPropertyName = string.Empty;
			this._attributeUsage = BamlAttributeUsage.Default;
			this._typeConverterAssemblyName = string.Empty;
			this._typeConverterName = string.Empty;
			this._properties.Clear();
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x001F52D0 File Offset: 0x001F42D0
		private BamlAttributeInfoRecord SetCommonPropertyInfo(BamlReader.BamlNodeInfo nodeInfo, short attrId)
		{
			BamlAttributeInfoRecord attributeInfoFromId = this.MapTable.GetAttributeInfoFromId(attrId);
			BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(attributeInfoFromId.OwnerTypeId);
			nodeInfo.LocalName = attributeInfoFromId.Name;
			nodeInfo.Name = typeInfoFromId.TypeFullName + "." + nodeInfo.LocalName;
			string assemblyName;
			string prefix;
			string xmlNamespace;
			this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out assemblyName, out prefix, out xmlNamespace);
			nodeInfo.AssemblyName = assemblyName;
			nodeInfo.Prefix = prefix;
			nodeInfo.XmlNamespace = xmlNamespace;
			nodeInfo.ClrNamespace = typeInfoFromId.ClrNamespace;
			nodeInfo.AttributeUsage = attributeInfoFromId.AttributeUsage;
			return attributeInfoFromId;
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x001F5360 File Offset: 0x001F4360
		private string GetTemplateBindingExtensionValueString(short memberId)
		{
			string str = string.Empty;
			string text = null;
			string text2;
			string name;
			if (memberId < 0)
			{
				memberId = -memberId;
				DependencyProperty dependencyProperty = null;
				if (memberId < 137)
				{
					dependencyProperty = KnownTypes.GetKnownDependencyPropertyFromId((KnownProperties)memberId);
				}
				if (dependencyProperty == null)
				{
					throw new InvalidOperationException(SR.Get("BamlBadExtensionValue"));
				}
				text2 = dependencyProperty.OwnerType.Name;
				name = dependencyProperty.Name;
				object obj = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml/presentation"];
				text = ((obj == null) ? string.Empty : ((string)obj));
			}
			else
			{
				BamlAttributeInfoRecord attributeInfoFromId = this.MapTable.GetAttributeInfoFromId(memberId);
				BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(attributeInfoFromId.OwnerTypeId);
				string text3;
				string text4;
				this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text3, out text, out text4);
				text2 = typeInfoFromId.TypeFullName;
				text2 = text2.Substring(text2.LastIndexOf(".", StringComparison.Ordinal) + 1);
				name = attributeInfoFromId.Name;
			}
			if (text == string.Empty)
			{
				str += text2;
			}
			else
			{
				str = str + text + ":" + text2;
			}
			return str + "." + name + "}";
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x001F5474 File Offset: 0x001F4474
		private string GetStaticExtensionValueString(short memberId)
		{
			string str = string.Empty;
			string text = null;
			string text2 = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			if (text2 != string.Empty)
			{
				str = "{" + text2 + ":Static ";
			}
			else
			{
				str = "{Static ";
			}
			string text3;
			string str2;
			if (memberId < 0)
			{
				memberId = -memberId;
				bool flag = true;
				memberId = SystemResourceKey.GetSystemResourceKeyIdFromBamlId(memberId, out flag);
				if (!Enum.IsDefined(typeof(SystemResourceKeyID), (int)memberId))
				{
					throw new InvalidOperationException(SR.Get("BamlBadExtensionValue"));
				}
				SystemResourceKeyID id = (SystemResourceKeyID)memberId;
				text3 = SystemKeyConverter.GetSystemClassName(id);
				if (flag)
				{
					str2 = SystemKeyConverter.GetSystemKeyName(id);
				}
				else
				{
					str2 = SystemKeyConverter.GetSystemPropertyName(id);
				}
				object obj = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml/presentation"];
				text = ((obj == null) ? string.Empty : ((string)obj));
			}
			else
			{
				BamlAttributeInfoRecord attributeInfoFromId = this.MapTable.GetAttributeInfoFromId(memberId);
				BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(attributeInfoFromId.OwnerTypeId);
				string text4;
				string text5;
				this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text4, out text, out text5);
				text3 = typeInfoFromId.TypeFullName;
				text3 = text3.Substring(text3.LastIndexOf(".", StringComparison.Ordinal) + 1);
				str2 = attributeInfoFromId.Name;
			}
			if (text == string.Empty)
			{
				str += text3;
			}
			else
			{
				str = str + text + ":" + text3;
			}
			return str + "." + str2 + "}";
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x001F55E0 File Offset: 0x001F45E0
		private string GetExtensionPrefixString(string extensionName)
		{
			string result = string.Empty;
			string text = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml/presentation"];
			if (!string.IsNullOrEmpty(text))
			{
				result = string.Concat(new string[]
				{
					"{",
					text,
					":",
					extensionName,
					" "
				});
			}
			else
			{
				result = "{" + extensionName + " ";
			}
			return result;
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x001F564C File Offset: 0x001F464C
		private string GetInnerExtensionValueString(IOptimizedMarkupExtension optimizedMarkupExtensionRecord)
		{
			string str = string.Empty;
			short valueId = optimizedMarkupExtensionRecord.ValueId;
			if (optimizedMarkupExtensionRecord.IsValueTypeExtension)
			{
				str = this.GetTypeValueString(valueId);
			}
			else if (optimizedMarkupExtensionRecord.IsValueStaticExtension)
			{
				str = this.GetStaticExtensionValueString(valueId);
			}
			else
			{
				str = this.MapTable.GetStringFromStringId((int)valueId);
			}
			return str + "}";
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x001F56A4 File Offset: 0x001F46A4
		private string GetExtensionValueString(IOptimizedMarkupExtension optimizedMarkupExtensionRecord)
		{
			string text = string.Empty;
			short valueId = optimizedMarkupExtensionRecord.ValueId;
			short extensionTypeId = optimizedMarkupExtensionRecord.ExtensionTypeId;
			if (extensionTypeId <= 602)
			{
				if (extensionTypeId != 189)
				{
					if (extensionTypeId == 602)
					{
						text = this.GetStaticExtensionValueString(valueId);
					}
				}
				else
				{
					text = this.GetExtensionPrefixString("DynamicResource");
					text += this.GetInnerExtensionValueString(optimizedMarkupExtensionRecord);
				}
			}
			else if (extensionTypeId != 603)
			{
				if (extensionTypeId == 634)
				{
					text = this.GetExtensionPrefixString("TemplateBinding");
					text += this.GetTemplateBindingExtensionValueString(valueId);
				}
			}
			else
			{
				text = this.GetExtensionPrefixString("StaticResource");
				text += this.GetInnerExtensionValueString(optimizedMarkupExtensionRecord);
			}
			return text;
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x001F5750 File Offset: 0x001F4750
		private string GetTypeValueString(short typeId)
		{
			string text = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			string str;
			if (text != string.Empty)
			{
				str = "{" + text + ":Type ";
			}
			else
			{
				str = "{Type ";
			}
			BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(typeId);
			string text2;
			string text3;
			string text4;
			this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text2, out text3, out text4);
			string text5 = typeInfoFromId.TypeFullName;
			text5 = text5.Substring(text5.LastIndexOf(".", StringComparison.Ordinal) + 1);
			if (text3 == string.Empty)
			{
				str += text5;
			}
			else
			{
				str = str + text3 + ":" + text5;
			}
			return str + "}";
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x001F5804 File Offset: 0x001F4804
		private void GetAssemblyAndPrefixAndXmlns(BamlTypeInfoRecord typeInfo, out string assemblyFullName, out string prefix, out string xmlns)
		{
			if (typeInfo.AssemblyId >= 0 || typeInfo.Type == null)
			{
				BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(typeInfo.AssemblyId);
				assemblyFullName = assemblyInfoFromId.AssemblyFullName;
			}
			else
			{
				Assembly assembly = typeInfo.Type.Assembly;
				assemblyFullName = assembly.FullName;
			}
			if (typeInfo.ClrNamespace == "System.Windows.Markup" && (assemblyFullName.StartsWith("PresentationFramework", StringComparison.Ordinal) || assemblyFullName.StartsWith("System.Xaml", StringComparison.Ordinal)))
			{
				xmlns = "http://schemas.microsoft.com/winfx/2006/xaml";
			}
			else
			{
				xmlns = this._parserContext.XamlTypeMapper.GetXmlNamespace(typeInfo.ClrNamespace, assemblyFullName);
				if (string.IsNullOrEmpty(xmlns))
				{
					List<string> xmlNamespaceList = this.GetXmlNamespaceList(typeInfo.ClrNamespace, assemblyFullName);
					prefix = this.GetXmlnsPrefix(xmlNamespaceList);
					return;
				}
			}
			prefix = this.GetXmlnsPrefix(xmlns);
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x001F58DC File Offset: 0x001F48DC
		private void SetXmlNamespace(string clrNamespace, string assemblyFullName, string xmlNs)
		{
			string key = clrNamespace + "#" + assemblyFullName;
			List<string> list;
			if (this._reverseXmlnsTable.ContainsKey(key))
			{
				list = this._reverseXmlnsTable[key];
			}
			else
			{
				list = new List<string>();
				this._reverseXmlnsTable[key] = list;
			}
			list.Add(xmlNs);
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x001F5930 File Offset: 0x001F4930
		private List<string> GetXmlNamespaceList(string clrNamespace, string assemblyFullName)
		{
			string key = clrNamespace + "#" + assemblyFullName;
			List<string> result = null;
			if (this._reverseXmlnsTable.ContainsKey(key))
			{
				result = this._reverseXmlnsTable[key];
			}
			return result;
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x001F5968 File Offset: 0x001F4968
		internal string GetXmlnsPrefix(string xmlns)
		{
			string result = string.Empty;
			if (xmlns == string.Empty)
			{
				xmlns = this._parserContext.XmlnsDictionary[string.Empty];
			}
			else
			{
				object obj = this._prefixDictionary[xmlns];
				if (obj != null)
				{
					result = (string)obj;
				}
			}
			return result;
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x001F59BC File Offset: 0x001F49BC
		private string GetXmlnsPrefix(List<string> xmlnsList)
		{
			if (xmlnsList != null)
			{
				for (int i = 0; i < xmlnsList.Count; i++)
				{
					string prefix = xmlnsList[i];
					string text = this._prefixDictionary[prefix];
					if (text != null)
					{
						return text;
					}
				}
			}
			return string.Empty;
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06003B3D RID: 15165 RVA: 0x001F59FC File Offset: 0x001F49FC
		private BamlMapTable MapTable
		{
			get
			{
				return this._parserContext.MapTable;
			}
		}

		// Token: 0x04001DF5 RID: 7669
		private BamlRecordReader _bamlRecordReader;

		// Token: 0x04001DF6 RID: 7670
		private XmlnsDictionary _prefixDictionary;

		// Token: 0x04001DF7 RID: 7671
		private BamlRecord _currentBamlRecord;

		// Token: 0x04001DF8 RID: 7672
		private bool _haveUnprocessedRecord;

		// Token: 0x04001DF9 RID: 7673
		private int _deferableContentBlockDepth;

		// Token: 0x04001DFA RID: 7674
		private long _deferableContentPosition;

		// Token: 0x04001DFB RID: 7675
		private List<BamlReader.BamlKeyInfo> _deferKeys;

		// Token: 0x04001DFC RID: 7676
		private BamlReader.BamlKeyInfo _currentKeyInfo;

		// Token: 0x04001DFD RID: 7677
		private List<BamlRecord> _currentStaticResourceRecords;

		// Token: 0x04001DFE RID: 7678
		private int _currentStaticResourceRecordIndex;

		// Token: 0x04001DFF RID: 7679
		private BamlNodeType _bamlNodeType;

		// Token: 0x04001E00 RID: 7680
		private ReadState _readState;

		// Token: 0x04001E01 RID: 7681
		private string _assemblyName;

		// Token: 0x04001E02 RID: 7682
		private string _prefix;

		// Token: 0x04001E03 RID: 7683
		private string _xmlNamespace;

		// Token: 0x04001E04 RID: 7684
		private string _clrNamespace;

		// Token: 0x04001E05 RID: 7685
		private string _value;

		// Token: 0x04001E06 RID: 7686
		private string _name;

		// Token: 0x04001E07 RID: 7687
		private string _localName;

		// Token: 0x04001E08 RID: 7688
		private string _ownerTypeName;

		// Token: 0x04001E09 RID: 7689
		private ArrayList _properties;

		// Token: 0x04001E0A RID: 7690
		private DependencyProperty _propertyDP;

		// Token: 0x04001E0B RID: 7691
		private int _propertiesIndex;

		// Token: 0x04001E0C RID: 7692
		private int _connectionId;

		// Token: 0x04001E0D RID: 7693
		private string _contentPropertyName;

		// Token: 0x04001E0E RID: 7694
		private BamlAttributeUsage _attributeUsage;

		// Token: 0x04001E0F RID: 7695
		private Stack _nodeStack;

		// Token: 0x04001E10 RID: 7696
		private ParserContext _parserContext;

		// Token: 0x04001E11 RID: 7697
		private bool _isInjected;

		// Token: 0x04001E12 RID: 7698
		private bool _useTypeConverter;

		// Token: 0x04001E13 RID: 7699
		private string _typeConverterAssemblyName;

		// Token: 0x04001E14 RID: 7700
		private string _typeConverterName;

		// Token: 0x04001E15 RID: 7701
		private Dictionary<string, List<string>> _reverseXmlnsTable;

		// Token: 0x02000AED RID: 2797
		internal class BamlNodeInfo
		{
			// Token: 0x06008B5E RID: 35678 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
			internal BamlNodeInfo()
			{
			}

			// Token: 0x17001E83 RID: 7811
			// (get) Token: 0x06008B5F RID: 35679 RVA: 0x00339D89 File Offset: 0x00338D89
			// (set) Token: 0x06008B60 RID: 35680 RVA: 0x00339D91 File Offset: 0x00338D91
			internal BamlRecordType RecordType
			{
				get
				{
					return this._recordType;
				}
				set
				{
					this._recordType = value;
				}
			}

			// Token: 0x17001E84 RID: 7812
			// (get) Token: 0x06008B61 RID: 35681 RVA: 0x00339D9A File Offset: 0x00338D9A
			// (set) Token: 0x06008B62 RID: 35682 RVA: 0x00339DA2 File Offset: 0x00338DA2
			internal string AssemblyName
			{
				get
				{
					return this._assemblyName;
				}
				set
				{
					this._assemblyName = value;
				}
			}

			// Token: 0x17001E85 RID: 7813
			// (get) Token: 0x06008B63 RID: 35683 RVA: 0x00339DAB File Offset: 0x00338DAB
			// (set) Token: 0x06008B64 RID: 35684 RVA: 0x00339DB3 File Offset: 0x00338DB3
			internal string Prefix
			{
				get
				{
					return this._prefix;
				}
				set
				{
					this._prefix = value;
				}
			}

			// Token: 0x17001E86 RID: 7814
			// (get) Token: 0x06008B65 RID: 35685 RVA: 0x00339DBC File Offset: 0x00338DBC
			// (set) Token: 0x06008B66 RID: 35686 RVA: 0x00339DC4 File Offset: 0x00338DC4
			internal string XmlNamespace
			{
				get
				{
					return this._xmlNamespace;
				}
				set
				{
					this._xmlNamespace = value;
				}
			}

			// Token: 0x17001E87 RID: 7815
			// (get) Token: 0x06008B67 RID: 35687 RVA: 0x00339DCD File Offset: 0x00338DCD
			// (set) Token: 0x06008B68 RID: 35688 RVA: 0x00339DD5 File Offset: 0x00338DD5
			internal string ClrNamespace
			{
				get
				{
					return this._clrNamespace;
				}
				set
				{
					this._clrNamespace = value;
				}
			}

			// Token: 0x17001E88 RID: 7816
			// (get) Token: 0x06008B69 RID: 35689 RVA: 0x00339DDE File Offset: 0x00338DDE
			// (set) Token: 0x06008B6A RID: 35690 RVA: 0x00339DE6 File Offset: 0x00338DE6
			internal string Name
			{
				get
				{
					return this._name;
				}
				set
				{
					this._name = value;
				}
			}

			// Token: 0x17001E89 RID: 7817
			// (get) Token: 0x06008B6B RID: 35691 RVA: 0x00339DEF File Offset: 0x00338DEF
			// (set) Token: 0x06008B6C RID: 35692 RVA: 0x00339DF7 File Offset: 0x00338DF7
			internal string LocalName
			{
				get
				{
					return this._localName;
				}
				set
				{
					this._localName = value;
				}
			}

			// Token: 0x17001E8A RID: 7818
			// (get) Token: 0x06008B6D RID: 35693 RVA: 0x00339E00 File Offset: 0x00338E00
			// (set) Token: 0x06008B6E RID: 35694 RVA: 0x00339E08 File Offset: 0x00338E08
			internal BamlAttributeUsage AttributeUsage
			{
				get
				{
					return this._attributeUsage;
				}
				set
				{
					this._attributeUsage = value;
				}
			}

			// Token: 0x0400472E RID: 18222
			private BamlRecordType _recordType;

			// Token: 0x0400472F RID: 18223
			private string _assemblyName;

			// Token: 0x04004730 RID: 18224
			private string _prefix;

			// Token: 0x04004731 RID: 18225
			private string _xmlNamespace;

			// Token: 0x04004732 RID: 18226
			private string _clrNamespace;

			// Token: 0x04004733 RID: 18227
			private string _name;

			// Token: 0x04004734 RID: 18228
			private string _localName;

			// Token: 0x04004735 RID: 18229
			private BamlAttributeUsage _attributeUsage;
		}

		// Token: 0x02000AEE RID: 2798
		internal class BamlPropertyInfo : BamlReader.BamlNodeInfo
		{
			// Token: 0x06008B6F RID: 35695 RVA: 0x00339E11 File Offset: 0x00338E11
			internal BamlPropertyInfo()
			{
			}

			// Token: 0x17001E8B RID: 7819
			// (get) Token: 0x06008B70 RID: 35696 RVA: 0x00339E19 File Offset: 0x00338E19
			// (set) Token: 0x06008B71 RID: 35697 RVA: 0x00339E21 File Offset: 0x00338E21
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

			// Token: 0x04004736 RID: 18230
			private string _value;
		}

		// Token: 0x02000AEF RID: 2799
		internal class BamlContentPropertyInfo : BamlReader.BamlNodeInfo
		{
		}

		// Token: 0x02000AF0 RID: 2800
		[DebuggerDisplay("{_offset}")]
		internal class BamlKeyInfo : BamlReader.BamlPropertyInfo
		{
			// Token: 0x06008B73 RID: 35699 RVA: 0x00339E2A File Offset: 0x00338E2A
			internal BamlKeyInfo()
			{
			}

			// Token: 0x17001E8C RID: 7820
			// (get) Token: 0x06008B74 RID: 35700 RVA: 0x00339E32 File Offset: 0x00338E32
			// (set) Token: 0x06008B75 RID: 35701 RVA: 0x00339E3A File Offset: 0x00338E3A
			internal int Offset
			{
				get
				{
					return this._offset;
				}
				set
				{
					this._offset = value;
				}
			}

			// Token: 0x17001E8D RID: 7821
			// (get) Token: 0x06008B76 RID: 35702 RVA: 0x00339E43 File Offset: 0x00338E43
			internal List<List<BamlRecord>> StaticResources
			{
				get
				{
					if (this._staticResources == null)
					{
						this._staticResources = new List<List<BamlRecord>>();
					}
					return this._staticResources;
				}
			}

			// Token: 0x04004737 RID: 18231
			private int _offset;

			// Token: 0x04004738 RID: 18232
			private List<List<BamlRecord>> _staticResources;
		}
	}
}
