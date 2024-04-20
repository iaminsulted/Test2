using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Serialization;
using MS.Internal;
using MS.Internal.IO.Packaging;
using MS.Internal.Utility;
using MS.Utility;

namespace System.Windows.Markup
{
	// Token: 0x0200047E RID: 1150
	internal class BamlRecordReader
	{
		// Token: 0x06003B42 RID: 15170 RVA: 0x001F5B44 File Offset: 0x001F4B44
		internal BamlRecordReader(Stream bamlStream, ParserContext parserContext) : this(bamlStream, parserContext, true)
		{
			this.XamlParseMode = XamlParseMode.Synchronous;
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x001F5B58 File Offset: 0x001F4B58
		internal BamlRecordReader(Stream bamlStream, ParserContext parserContext, object root)
		{
			this._contextStack = new ParserStack();
			this._parseMode = XamlParseMode.Synchronous;
			this._buildTopDown = true;
			base..ctor();
			this.ParserContext = parserContext;
			this._rootElement = root;
			this._bamlAsForest = (root != null);
			if (this._bamlAsForest)
			{
				this.ParserContext.RootElement = this._rootElement;
			}
			this._rootList = new ArrayList(1);
			this.BamlStream = bamlStream;
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x001F5BC8 File Offset: 0x001F4BC8
		internal BamlRecordReader(Stream bamlStream, ParserContext parserContext, bool loadMapper)
		{
			this._contextStack = new ParserStack();
			this._parseMode = XamlParseMode.Synchronous;
			this._buildTopDown = true;
			base..ctor();
			this.ParserContext = parserContext;
			this._rootList = new ArrayList(1);
			this.BamlStream = bamlStream;
			if (loadMapper)
			{
				this.ParserContext.XamlTypeMapper = this.XamlTypeMapper;
			}
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x001F5C22 File Offset: 0x001F4C22
		protected internal BamlRecordReader()
		{
			this._contextStack = new ParserStack();
			this._parseMode = XamlParseMode.Synchronous;
			this._buildTopDown = true;
			base..ctor();
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x001F5C43 File Offset: 0x001F4C43
		internal void Initialize()
		{
			this.MapTable.Initialize();
			this.XamlTypeMapper.Initialize();
			this.ParserContext.Initialize();
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06003B47 RID: 15175 RVA: 0x001F5C66 File Offset: 0x001F4C66
		// (set) Token: 0x06003B48 RID: 15176 RVA: 0x001F5C6E File Offset: 0x001F4C6E
		internal ArrayList RootList
		{
			get
			{
				return this._rootList;
			}
			set
			{
				this._rootList = value;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06003B49 RID: 15177 RVA: 0x001F5C77 File Offset: 0x001F4C77
		// (set) Token: 0x06003B4A RID: 15178 RVA: 0x001F5C7F File Offset: 0x001F4C7F
		internal bool BuildTopDown
		{
			get
			{
				return this._buildTopDown;
			}
			set
			{
				this._buildTopDown = value;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06003B4B RID: 15179 RVA: 0x001F5C88 File Offset: 0x001F4C88
		internal int BytesAvailible
		{
			get
			{
				Stream baseStream = this.BinaryReader.BaseStream;
				return (int)(baseStream.Length - baseStream.Position);
			}
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x001F5CB0 File Offset: 0x001F4CB0
		internal BamlRecord GetNextRecord()
		{
			BamlRecord bamlRecord = null;
			if (this.PreParsedRecordsStart == null)
			{
				Stream baseStream = this.BinaryReader.BaseStream;
				if (this.XamlReaderStream != null)
				{
					long position = baseStream.Position;
					long num = baseStream.Length - position;
					if (1L > num)
					{
						return null;
					}
					BamlRecordType recordType = (BamlRecordType)this.BinaryReader.ReadByte();
					num -= 1L;
					bamlRecord = this.ReadNextRecordWithDebugExtension(num, recordType);
					if (bamlRecord == null)
					{
						baseStream.Seek(position, SeekOrigin.Begin);
						return null;
					}
					this.XamlReaderStream.ReaderDoneWithFileUpToPosition(baseStream.Position - 1L);
				}
				else
				{
					bool flag = true;
					while (flag)
					{
						if (this.BinaryReader.BaseStream.Length > this.BinaryReader.BaseStream.Position)
						{
							BamlRecordType recordType2 = (BamlRecordType)this.BinaryReader.ReadByte();
							bamlRecord = this.ReadNextRecordWithDebugExtension(long.MaxValue, recordType2);
							flag = false;
						}
						else
						{
							flag = false;
						}
					}
				}
			}
			else if (this.PreParsedCurrentRecord != null)
			{
				bamlRecord = this.PreParsedCurrentRecord;
				this.PreParsedCurrentRecord = this.PreParsedCurrentRecord.Next;
				if (BamlRecordHelper.HasDebugExtensionRecord(this.ParserContext.IsDebugBamlStream, bamlRecord))
				{
					this.ProcessDebugBamlRecord(this.PreParsedCurrentRecord);
					this.PreParsedCurrentRecord = this.PreParsedCurrentRecord.Next;
				}
			}
			return bamlRecord;
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x001F5DE0 File Offset: 0x001F4DE0
		internal BamlRecord ReadNextRecordWithDebugExtension(long bytesAvailable, BamlRecordType recordType)
		{
			BamlRecord bamlRecord = this.BamlRecordManager.ReadNextRecord(this.BinaryReader, bytesAvailable, recordType);
			if (this.IsDebugBamlStream && BamlRecordHelper.DoesRecordTypeHaveDebugExtension(bamlRecord.RecordType))
			{
				BamlRecord next = this.ReadDebugExtensionRecord();
				bamlRecord.Next = next;
			}
			return bamlRecord;
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x001F5E28 File Offset: 0x001F4E28
		internal BamlRecord ReadDebugExtensionRecord()
		{
			Stream baseStream = this.BinaryReader.BaseStream;
			long num = baseStream.Length - baseStream.Position;
			if (num == 0L)
			{
				return null;
			}
			BamlRecordType recordType = (BamlRecordType)this.BinaryReader.ReadByte();
			if (BamlRecordHelper.IsDebugBamlRecordType(recordType))
			{
				BamlRecord bamlRecord = this.BamlRecordManager.ReadNextRecord(this.BinaryReader, num, recordType);
				this.ProcessDebugBamlRecord(bamlRecord);
				return bamlRecord;
			}
			baseStream.Seek(-1L, SeekOrigin.Current);
			return null;
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x001F5E90 File Offset: 0x001F4E90
		internal void ProcessDebugBamlRecord(BamlRecord bamlRecord)
		{
			if (bamlRecord.RecordType == BamlRecordType.LineNumberAndPosition)
			{
				BamlLineAndPositionRecord bamlLineAndPositionRecord = (BamlLineAndPositionRecord)bamlRecord;
				this.LineNumber = (int)bamlLineAndPositionRecord.LineNumber;
				this.LinePosition = (int)bamlLineAndPositionRecord.LinePosition;
				return;
			}
			BamlLinePositionRecord bamlLinePositionRecord = (BamlLinePositionRecord)bamlRecord;
			this.LinePosition = (int)bamlLinePositionRecord.LinePosition;
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x001F5EDC File Offset: 0x001F4EDC
		internal BamlRecordType GetNextRecordType()
		{
			BamlRecordType result;
			if (this.PreParsedRecordsStart == null)
			{
				result = (BamlRecordType)this.BinaryReader.PeekChar();
			}
			else
			{
				result = this.PreParsedCurrentRecord.RecordType;
			}
			return result;
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x001F5F0D File Offset: 0x001F4F0D
		internal void Close()
		{
			if (this.BamlStream != null)
			{
				this.BamlStream.Close();
			}
			this.EndOfDocument = true;
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x001F5F2C File Offset: 0x001F4F2C
		internal bool Read(bool singleRecord)
		{
			BamlRecord bamlRecord = null;
			bool flag = true;
			while (flag && (bamlRecord = this.GetNextRecord()) != null)
			{
				flag = this.ReadRecord(bamlRecord);
				if (singleRecord)
				{
					break;
				}
			}
			if (bamlRecord == null)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x001F5F5D File Offset: 0x001F4F5D
		internal bool Read()
		{
			return this.Read(false);
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x001F5F66 File Offset: 0x001F4F66
		internal bool Read(BamlRecord bamlRecord, int lineNumber, int linePosition)
		{
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
			return this.ReadRecord(bamlRecord);
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x001F5F7D File Offset: 0x001F4F7D
		internal void ReadVersionHeader()
		{
			new BamlVersionHeader().LoadVersion(this.BinaryReader);
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x001F5F90 File Offset: 0x001F4F90
		internal object ReadElement(long startPosition, XamlObjectIds contextXamlObjectIds, object dictionaryKey)
		{
			bool flag = true;
			this.BinaryReader.BaseStream.Position = startPosition;
			int num = 0;
			bool flag2 = false;
			this.PushContext(ReaderFlags.RealizeDeferContent, null, null, 0);
			this.CurrentContext.ElementNameOrPropertyName = contextXamlObjectIds.Name;
			this.CurrentContext.Uid = contextXamlObjectIds.Uid;
			this.CurrentContext.Key = dictionaryKey;
			BamlRecord nextRecord;
			while (flag && (nextRecord = this.GetNextRecord()) != null)
			{
				BamlElementStartRecord bamlElementStartRecord = nextRecord as BamlElementStartRecord;
				if (bamlElementStartRecord != null)
				{
					if (!this.MapTable.HasSerializerForTypeId(bamlElementStartRecord.TypeId))
					{
						num++;
					}
				}
				else if (nextRecord is BamlElementEndRecord)
				{
					num--;
				}
				flag = this.ReadRecord(nextRecord);
				if (!flag2)
				{
					this.CurrentContext.Key = dictionaryKey;
					flag2 = true;
				}
				if (num == 0)
				{
					break;
				}
			}
			object objectData = this.CurrentContext.ObjectData;
			this.CurrentContext.ObjectData = null;
			this.PopContext();
			this.MapTable.ClearConverterCache();
			return objectData;
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x001F6078 File Offset: 0x001F5078
		protected virtual void ReadConnectionId(BamlConnectionIdRecord bamlConnectionIdRecord)
		{
			if (this._componentConnector != null)
			{
				object currentObjectData = this.GetCurrentObjectData();
				this._componentConnector.Connect(bamlConnectionIdRecord.ConnectionId, currentObjectData);
			}
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x001F60A6 File Offset: 0x001F50A6
		private void ReadDocumentStartRecord(BamlDocumentStartRecord documentStartRecord)
		{
			this.IsDebugBamlStream = documentStartRecord.DebugBaml;
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x001F60B4 File Offset: 0x001F50B4
		private void ReadDocumentEndRecord()
		{
			this.SetPropertyValueToParent(false);
			this.ParserContext.RootElement = null;
			this.MapTable.ClearConverterCache();
			this.EndOfDocument = true;
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x001F60DC File Offset: 0x001F50DC
		internal virtual bool ReadRecord(BamlRecord bamlRecord)
		{
			bool result = true;
			try
			{
				switch (bamlRecord.RecordType)
				{
				case BamlRecordType.DocumentStart:
					this.ReadDocumentStartRecord((BamlDocumentStartRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.DocumentEnd:
					this.ReadDocumentEndRecord();
					result = false;
					goto IL_42E;
				case BamlRecordType.ElementStart:
				case BamlRecordType.StaticResourceStart:
					if (((BamlElementStartRecord)bamlRecord).IsInjected)
					{
						this.CurrentContext.SetFlag(ReaderFlags.InjectedElement);
						goto IL_42E;
					}
					this.ReadElementStartRecord((BamlElementStartRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.ElementEnd:
				case BamlRecordType.StaticResourceEnd:
					if (this.CurrentContext.CheckFlag(ReaderFlags.InjectedElement))
					{
						this.CurrentContext.ClearFlag(ReaderFlags.InjectedElement);
						goto IL_42E;
					}
					this.ReadElementEndRecord(false);
					goto IL_42E;
				case BamlRecordType.Property:
					this.ReadPropertyRecord((BamlPropertyRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyCustom:
					this.ReadPropertyCustomRecord((BamlPropertyCustomRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyComplexStart:
					this.ReadPropertyComplexStartRecord((BamlPropertyComplexStartRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyComplexEnd:
					this.ReadPropertyComplexEndRecord();
					goto IL_42E;
				case BamlRecordType.PropertyArrayStart:
					this.ReadPropertyArrayStartRecord((BamlPropertyArrayStartRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyArrayEnd:
					this.ReadPropertyArrayEndRecord();
					goto IL_42E;
				case BamlRecordType.PropertyIListStart:
					this.ReadPropertyIListStartRecord((BamlPropertyIListStartRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyIListEnd:
					this.ReadPropertyIListEndRecord();
					goto IL_42E;
				case BamlRecordType.PropertyIDictionaryStart:
					this.ReadPropertyIDictionaryStartRecord((BamlPropertyIDictionaryStartRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyIDictionaryEnd:
					this.ReadPropertyIDictionaryEndRecord();
					goto IL_42E;
				case BamlRecordType.LiteralContent:
					this.ReadLiteralContentRecord((BamlLiteralContentRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.Text:
				case BamlRecordType.TextWithConverter:
				case BamlRecordType.TextWithId:
					this.ReadTextRecord((BamlTextRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.RoutedEvent:
				{
					this.GetCurrentObjectData();
					BamlRoutedEventRecord bamlRoutedEventRecord = (BamlRoutedEventRecord)bamlRecord;
					this.ThrowException("ParserBamlEvent", bamlRoutedEventRecord.Value);
					goto IL_42E;
				}
				case BamlRecordType.XmlnsProperty:
					this.ReadXmlnsPropertyRecord((BamlXmlnsPropertyRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.DefAttribute:
					this.ReadDefAttributeRecord((BamlDefAttributeRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PIMapping:
				{
					BamlPIMappingRecord bamlPIMappingRecord = (BamlPIMappingRecord)bamlRecord;
					if (!this.XamlTypeMapper.PITable.Contains(bamlPIMappingRecord.XmlNamespace))
					{
						BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(bamlPIMappingRecord.AssemblyId);
						ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = new ClrNamespaceAssemblyPair(bamlPIMappingRecord.ClrNamespace, assemblyInfoFromId.AssemblyFullName);
						this.XamlTypeMapper.PITable.Add(bamlPIMappingRecord.XmlNamespace, clrNamespaceAssemblyPair);
						goto IL_42E;
					}
					goto IL_42E;
				}
				case BamlRecordType.AssemblyInfo:
					this.MapTable.LoadAssemblyInfoRecord((BamlAssemblyInfoRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.TypeInfo:
				case BamlRecordType.TypeSerializerInfo:
					this.MapTable.LoadTypeInfoRecord((BamlTypeInfoRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.AttributeInfo:
					this.MapTable.LoadAttributeInfoRecord((BamlAttributeInfoRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.StringInfo:
					this.MapTable.LoadStringInfoRecord((BamlStringInfoRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyStringReference:
					this.ReadPropertyStringRecord((BamlPropertyStringReferenceRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyTypeReference:
					this.ReadPropertyTypeRecord((BamlPropertyTypeReferenceRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyWithExtension:
					this.ReadPropertyWithExtensionRecord((BamlPropertyWithExtensionRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyWithConverter:
					this.ReadPropertyConverterRecord((BamlPropertyWithConverterRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.DeferableContentStart:
					this.ReadDeferableContentStart((BamlDeferableContentStartRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.DefAttributeKeyType:
					this.ReadDefAttributeKeyTypeRecord((BamlDefAttributeKeyTypeRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.KeyElementStart:
					this.ReadKeyElementStartRecord((BamlKeyElementStartRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.KeyElementEnd:
					this.ReadKeyElementEndRecord();
					goto IL_42E;
				case BamlRecordType.ConstructorParametersStart:
					this.ReadConstructorParametersStartRecord();
					goto IL_42E;
				case BamlRecordType.ConstructorParametersEnd:
					this.ReadConstructorParametersEndRecord();
					goto IL_42E;
				case BamlRecordType.ConstructorParameterType:
					this.ReadConstructorParameterTypeRecord((BamlConstructorParameterTypeRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.ConnectionId:
					this.ReadConnectionId((BamlConnectionIdRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.ContentProperty:
					this.ReadContentPropertyRecord((BamlContentPropertyRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.NamedElementStart:
					goto IL_42E;
				case BamlRecordType.StaticResourceId:
					this.ReadStaticResourceIdRecord((BamlStaticResourceIdRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PresentationOptionsAttribute:
					this.ReadPresentationOptionsAttributeRecord((BamlPresentationOptionsAttributeRecord)bamlRecord);
					goto IL_42E;
				case BamlRecordType.PropertyWithStaticResourceId:
					this.ReadPropertyWithStaticResourceIdRecord((BamlPropertyWithStaticResourceIdRecord)bamlRecord);
					goto IL_42E;
				}
				this.ThrowException("ParserUnknownBaml", ((int)bamlRecord.RecordType).ToString(CultureInfo.CurrentCulture));
				IL_42E:;
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				XamlParseException.ThrowException(this.ParserContext, this.LineNumber, this.LinePosition, string.Empty, ex);
			}
			return result;
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x001F656C File Offset: 0x001F556C
		protected virtual void ReadXmlnsPropertyRecord(BamlXmlnsPropertyRecord xmlnsRecord)
		{
			if (ReaderFlags.DependencyObject == this.CurrentContext.ContextType || ReaderFlags.ClrObject == this.CurrentContext.ContextType || ReaderFlags.PropertyComplexClr == this.CurrentContext.ContextType || ReaderFlags.PropertyComplexDP == this.CurrentContext.ContextType)
			{
				this.XmlnsDictionary[xmlnsRecord.Prefix] = xmlnsRecord.XmlNamespace;
				this.XamlTypeMapper.SetUriToAssemblyNameMapping(xmlnsRecord.XmlNamespace, xmlnsRecord.AssemblyIds);
				if (ReaderFlags.DependencyObject == this.CurrentContext.ContextType)
				{
					this.SetXmlnsOnCurrentObject(xmlnsRecord);
				}
			}
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x001F6608 File Offset: 0x001F5608
		private void GetElementAndFlags(BamlElementStartRecord bamlElementStartRecord, out object element, out ReaderFlags flags, out Type delayCreatedType, out short delayCreatedTypeId)
		{
			short typeId = bamlElementStartRecord.TypeId;
			Type typeFromId = this.MapTable.GetTypeFromId(typeId);
			element = null;
			delayCreatedType = null;
			delayCreatedTypeId = 0;
			flags = ReaderFlags.Unknown;
			if (null != typeFromId)
			{
				if (bamlElementStartRecord.CreateUsingTypeConverter || typeof(MarkupExtension).IsAssignableFrom(typeFromId))
				{
					delayCreatedType = typeFromId;
					delayCreatedTypeId = typeId;
				}
				else
				{
					element = this.CreateInstanceFromType(typeFromId, typeId, false);
					if (element == null)
					{
						this.ThrowException("ParserNoElementCreate2", typeFromId.FullName);
					}
				}
				flags = this.GetFlagsFromType(typeFromId);
			}
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x001F6690 File Offset: 0x001F5690
		protected ReaderFlags GetFlagsFromType(Type elementType)
		{
			ReaderFlags readerFlags = typeof(DependencyObject).IsAssignableFrom(elementType) ? ReaderFlags.DependencyObject : ReaderFlags.ClrObject;
			if (typeof(IDictionary).IsAssignableFrom(elementType))
			{
				readerFlags |= ReaderFlags.IDictionary;
			}
			else if (typeof(IList).IsAssignableFrom(elementType))
			{
				readerFlags |= ReaderFlags.IList;
			}
			else if (typeof(ArrayExtension).IsAssignableFrom(elementType))
			{
				readerFlags |= ReaderFlags.ArrayExt;
			}
			else if (BamlRecordManager.TreatAsIAddChild(elementType))
			{
				readerFlags |= ReaderFlags.IAddChild;
			}
			return readerFlags;
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x001F6718 File Offset: 0x001F5718
		internal static void CheckForTreeAdd(ref ReaderFlags flags, ReaderContextStackData context)
		{
			if (context == null || (context.ContextType != ReaderFlags.ConstructorParams && context.ContextType != ReaderFlags.RealizeDeferContent))
			{
				flags |= ReaderFlags.NeedToAddToTree;
			}
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x001F6740 File Offset: 0x001F5740
		internal void SetDependencyValue(DependencyObject dependencyObject, DependencyProperty dependencyProperty, object value)
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = (this.ParserContext != null && this.ParserContext.SkipJournaledProperties) ? (dependencyProperty.GetMetadata(dependencyObject.DependencyObjectType) as FrameworkPropertyMetadata) : null;
			if (frameworkPropertyMetadata == null || !frameworkPropertyMetadata.Journal || value is Expression)
			{
				this.SetDependencyValueCore(dependencyObject, dependencyProperty, value);
			}
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x001F6793 File Offset: 0x001F5793
		internal virtual void SetDependencyValueCore(DependencyObject dependencyObject, DependencyProperty dependencyProperty, object value)
		{
			dependencyObject.SetValue(dependencyProperty, value);
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x001F67A0 File Offset: 0x001F57A0
		internal object ProvideValueFromMarkupExtension(MarkupExtension markupExtension, object obj, object member)
		{
			object obj2 = null;
			ProvideValueServiceProvider provideValueProvider = this.ParserContext.ProvideValueProvider;
			provideValueProvider.SetData(obj, member);
			try
			{
				obj2 = markupExtension.ProvideValue(provideValueProvider);
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.TraceActivityItem(TraceMarkup.ProvideValue, new object[]
					{
						markupExtension,
						obj,
						member,
						obj2
					});
				}
			}
			finally
			{
				provideValueProvider.ClearData();
			}
			return obj2;
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x001F680C File Offset: 0x001F580C
		internal void BaseReadElementStartRecord(BamlElementStartRecord bamlElementRecord)
		{
			object obj = null;
			Type expectedType = null;
			short expectedTypeId = 0;
			ReaderFlags contextFlags = ReaderFlags.Unknown;
			ReaderContextStackData currentContext = this.CurrentContext;
			if (this._bamlAsForest && currentContext == null)
			{
				obj = this._rootElement;
				contextFlags = this.GetFlagsFromType(obj.GetType());
			}
			else
			{
				if (currentContext != null && (ReaderFlags.PropertyComplexClr == currentContext.ContextType || ReaderFlags.PropertyComplexDP == currentContext.ContextType) && null == currentContext.ExpectedType)
				{
					string propNameFrom = this.GetPropNameFrom(currentContext.ObjectData);
					this.ThrowException("ParserNoComplexMulti", propNameFrom);
				}
				if (this.ParentContext == null)
				{
					this.SetPropertyValueToParent(true);
				}
				this.GetElementAndFlags(bamlElementRecord, out obj, out contextFlags, out expectedType, out expectedTypeId);
			}
			Stream bamlStream = this.BamlStream;
			if (!this._bamlAsForest && currentContext == null && obj != null && bamlStream != null && !(bamlStream is ReaderStream) && this.StreamPosition == this.StreamLength)
			{
				this.ReadDocumentEndRecord();
				if (this.RootList.Count == 0)
				{
					this.RootList.Add(obj);
				}
				this.IsRootAlreadyLoaded = true;
				return;
			}
			if (obj != null)
			{
				string name = null;
				if (bamlElementRecord is BamlNamedElementStartRecord)
				{
					name = (bamlElementRecord as BamlNamedElementStartRecord).RuntimeName;
				}
				this.ElementInitialize(obj, name);
			}
			BamlRecordReader.CheckForTreeAdd(ref contextFlags, currentContext);
			this.PushContext(contextFlags, obj, expectedType, expectedTypeId, bamlElementRecord.CreateUsingTypeConverter);
			if (this.BuildTopDown && obj != null && (obj is UIElement || obj is ContentElement || obj is UIElement3D))
			{
				this.SetPropertyValueToParent(true);
				return;
			}
			if (this.CurrentContext.CheckFlag(ReaderFlags.IDictionary))
			{
				bool flag = false;
				if (this.CheckExplicitCollectionTag(ref flag))
				{
					this.CurrentContext.MarkAddedToTree();
					if (obj is ResourceDictionary)
					{
						this.SetCollectionPropertyValue(this.ParentContext);
					}
				}
			}
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x001F69B4 File Offset: 0x001F59B4
		protected virtual bool ReadElementStartRecord(BamlElementStartRecord bamlElementRecord)
		{
			if (this.MapTable.HasSerializerForTypeId(bamlElementRecord.TypeId))
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseRdrCrInstBegin);
				try
				{
					BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(bamlElementRecord.TypeId);
					XamlSerializer xamlSerializer = this.CreateSerializer((BamlTypeInfoWithSerializerRecord)typeInfoFromId);
					if (this.ParserContext.RootElement == null)
					{
						this.ParserContext.RootElement = this._rootElement;
					}
					if (this.ParserContext.StyleConnector == null)
					{
						this.ParserContext.StyleConnector = (this._rootElement as IStyleConnector);
					}
					xamlSerializer.ConvertBamlToObject(this, bamlElementRecord, this.ParserContext);
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseRdrCrInstEnd);
				}
				return true;
			}
			this.BaseReadElementStartRecord(bamlElementRecord);
			return false;
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x001F6A7C File Offset: 0x001F5A7C
		protected internal virtual void ReadElementEndRecord(bool fromNestedBamlRecordReader)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpectedEndEle");
			}
			object currentObjectData = this.GetCurrentObjectData();
			this.ElementEndInit(ref currentObjectData);
			this.SetPropertyValueToParent(false);
			bool contextFlags = this.CurrentContext.ContextFlags != ReaderFlags.Unknown;
			this.FreezeIfRequired(currentObjectData);
			this.PopContext();
			if (((contextFlags ? 1 : 0) & 2) == 0 && this.CurrentContext != null)
			{
				ReaderFlags contextType = this.CurrentContext.ContextType;
				if (contextType == ReaderFlags.RealizeDeferContent)
				{
					this.CurrentContext.ObjectData = currentObjectData;
					return;
				}
				if (contextType != ReaderFlags.ConstructorParams)
				{
					return;
				}
				this.SetConstructorParameter(currentObjectData);
			}
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x001F6B2C File Offset: 0x001F5B2C
		internal virtual void ReadKeyElementStartRecord(BamlKeyElementStartRecord bamlElementRecord)
		{
			Type typeFromId = this.MapTable.GetTypeFromId(bamlElementRecord.TypeId);
			ReaderFlags contextFlags = (typeFromId.IsAssignableFrom(typeof(DependencyObject)) ? ReaderFlags.DependencyObject : ReaderFlags.ClrObject) | ReaderFlags.NeedToAddToTree;
			this.PushContext(contextFlags, null, typeFromId, bamlElementRecord.TypeId);
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x001F6B7C File Offset: 0x001F5B7C
		internal virtual void ReadKeyElementEndRecord()
		{
			object key = this.ProvideValueFromMarkupExtension((MarkupExtension)this.GetCurrentObjectData(), this.ParentObjectData, null);
			this.SetKeyOnContext(key, "Key", this.ParentContext, this.GrandParentContext);
			this.PopContext();
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x001F6BC0 File Offset: 0x001F5BC0
		internal virtual void ReadConstructorParameterTypeRecord(BamlConstructorParameterTypeRecord constructorParameterType)
		{
			this.SetConstructorParameter(this.MapTable.GetTypeFromId(constructorParameterType.TypeId));
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x001F6BDC File Offset: 0x001F5BDC
		internal virtual void ReadContentPropertyRecord(BamlContentPropertyRecord bamlContentPropertyRecord)
		{
			object obj = null;
			short attributeId = bamlContentPropertyRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			if (currentObjectData != null)
			{
				short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(currentObjectData.GetType());
				if (knownTypeIdFromType < 0)
				{
					obj = KnownTypes.GetCollectionForCPA(currentObjectData, (KnownElements)(-(KnownElements)knownTypeIdFromType));
				}
			}
			if (obj == null)
			{
				WpfPropertyDefinition wpfPropertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
				if (wpfPropertyDefinition.DependencyProperty != null)
				{
					if (typeof(IList).IsAssignableFrom(wpfPropertyDefinition.PropertyType))
					{
						obj = (((DependencyObject)currentObjectData).GetValue(wpfPropertyDefinition.DependencyProperty) as IList);
					}
					else
					{
						obj = wpfPropertyDefinition.DependencyProperty;
					}
				}
				if (obj == null && wpfPropertyDefinition.PropertyInfo != null)
				{
					if (wpfPropertyDefinition.IsInternal)
					{
						obj = (XamlTypeMapper.GetInternalPropertyValue(this.ParserContext, this.ParserContext.RootElement, wpfPropertyDefinition.PropertyInfo, currentObjectData) as IList);
						if (obj == null)
						{
							bool allowProtected = this.ParserContext.RootElement is IComponentConnector && this.ParserContext.RootElement == currentObjectData;
							bool flag;
							if (!XamlTypeMapper.IsAllowedPropertySet(wpfPropertyDefinition.PropertyInfo, allowProtected, out flag))
							{
								this.ThrowException("ParserCantSetContentProperty", wpfPropertyDefinition.Name, wpfPropertyDefinition.PropertyInfo.ReflectedType.Name);
							}
						}
					}
					else
					{
						obj = (wpfPropertyDefinition.PropertyInfo.GetValue(currentObjectData, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, null, TypeConverterHelper.InvariantEnglishUS) as IList);
					}
					if (obj == null)
					{
						obj = wpfPropertyDefinition.PropertyInfo;
					}
				}
			}
			if (obj == null)
			{
				this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
			}
			this.CurrentContext.ContentProperty = obj;
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x001F6D60 File Offset: 0x001F5D60
		internal virtual void ReadConstructorParametersStartRecord()
		{
			this.PushContext(ReaderFlags.ConstructorParams, null, null, 0);
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x001F6D70 File Offset: 0x001F5D70
		internal virtual void ReadConstructorParametersEndRecord()
		{
			Type expectedType = this.ParentContext.ExpectedType;
			short num = -this.ParentContext.ExpectedTypeId;
			object obj = null;
			ArrayList arrayList = null;
			object obj2 = null;
			bool flag = false;
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.CreateMarkupExtension, expectedType);
			}
			int num2;
			if (this.CurrentContext.CheckFlag(ReaderFlags.SingletonConstructorParam))
			{
				obj = this.CurrentContext.ObjectData;
				num2 = 1;
				if (num <= 602)
				{
					if (num != 189)
					{
						if (num == 602)
						{
							obj2 = new StaticExtension((string)obj);
							flag = true;
						}
					}
					else
					{
						obj2 = new DynamicResourceExtension(obj);
						flag = true;
					}
				}
				else if (num != 603)
				{
					if (num != 634)
					{
						if (num == 691)
						{
							Type type = obj as Type;
							if (type != null)
							{
								obj2 = new TypeExtension(type);
							}
							else
							{
								obj2 = new TypeExtension((string)obj);
							}
							flag = true;
						}
					}
					else
					{
						DependencyProperty dependencyProperty = obj as DependencyProperty;
						if (dependencyProperty == null)
						{
							string text = obj as string;
							Type targetType = this.ParserContext.TargetType;
							dependencyProperty = XamlTypeMapper.ParsePropertyName(this.ParserContext, text.Trim(), ref targetType);
							if (dependencyProperty == null)
							{
								this.ThrowException("ParserNoDPOnOwner", text, targetType.FullName);
							}
						}
						obj2 = new TemplateBindingExtension(dependencyProperty);
						flag = true;
					}
				}
				else
				{
					obj2 = new StaticResourceExtension(obj);
					flag = true;
				}
			}
			else
			{
				arrayList = (ArrayList)this.CurrentContext.ObjectData;
				num2 = arrayList.Count;
			}
			if (!flag)
			{
				XamlTypeMapper.ConstructorData constructors = this.XamlTypeMapper.GetConstructors(expectedType);
				ConstructorInfo[] constructors2 = constructors.Constructors;
				for (int i = 0; i < constructors2.Length; i++)
				{
					ConstructorInfo constructorInfo = constructors2[i];
					ParameterInfo[] parameters = constructors.GetParameters(i);
					if (parameters.Length == num2)
					{
						object[] array = new object[parameters.Length];
						if (num2 == 1)
						{
							this.ProcessConstructorParameter(parameters[0], obj, ref array[0]);
							if (num == 516)
							{
								obj2 = new RelativeSource((RelativeSourceMode)array[0]);
								flag = true;
							}
						}
						else
						{
							for (int j = 0; j < parameters.Length; j++)
							{
								this.ProcessConstructorParameter(parameters[j], arrayList[j], ref array[j]);
							}
						}
						if (!flag)
						{
							try
							{
								obj2 = constructorInfo.Invoke(array);
								flag = true;
							}
							catch (Exception innerException)
							{
								if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
								{
									throw;
								}
								TargetInvocationException ex = innerException as TargetInvocationException;
								if (ex != null)
								{
									innerException = ex.InnerException;
								}
								this.ThrowExceptionWithLine(SR.Get("ParserFailedToCreateFromConstructor", new object[]
								{
									constructorInfo.DeclaringType.Name
								}), innerException);
							}
						}
					}
				}
			}
			if (flag)
			{
				this.ParentContext.ObjectData = obj2;
				this.ParentContext.ExpectedType = null;
				this.PopContext();
			}
			else
			{
				this.ThrowException("ParserBadConstructorParams", expectedType.Name, num2.ToString(CultureInfo.CurrentCulture));
			}
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.CreateMarkupExtension, expectedType, obj2);
			}
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x001F7088 File Offset: 0x001F6088
		private void ProcessConstructorParameter(ParameterInfo paramInfo, object param, ref object paramArrayItem)
		{
			MarkupExtension markupExtension = param as MarkupExtension;
			if (markupExtension != null)
			{
				param = this.ProvideValueFromMarkupExtension(markupExtension, null, null);
			}
			if (param != null && paramInfo.ParameterType != typeof(object) && paramInfo.ParameterType != param.GetType())
			{
				TypeConverter typeConverter = this.XamlTypeMapper.GetTypeConverter(paramInfo.ParameterType);
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.ProcessConstructorParameter, paramInfo.ParameterType, typeConverter.GetType(), param);
				}
				try
				{
					if (param is string)
					{
						param = typeConverter.ConvertFromString(this.TypeConvertContext, TypeConverterHelper.InvariantEnglishUS, param as string);
					}
					else if (!paramInfo.ParameterType.IsAssignableFrom(param.GetType()))
					{
						param = typeConverter.ConvertTo(this.TypeConvertContext, TypeConverterHelper.InvariantEnglishUS, param, paramInfo.ParameterType);
					}
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
					{
						throw;
					}
					this.ThrowExceptionWithLine(SR.Get("ParserCannotConvertString", new object[]
					{
						param.ToString(),
						paramInfo.ParameterType.FullName
					}), ex);
				}
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.ProcessConstructorParameter, paramInfo.ParameterType, typeConverter.GetType(), param);
				}
			}
			paramArrayItem = param;
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x001F71E4 File Offset: 0x001F61E4
		internal virtual void ReadDeferableContentStart(BamlDeferableContentStartRecord bamlRecord)
		{
			if (!(this.GetDictionaryFromContext(this.CurrentContext, true) is ResourceDictionary))
			{
				return;
			}
			Stream baseStream = this.BinaryReader.BaseStream;
			long position = baseStream.Position;
			if (baseStream.Length - position < (long)bamlRecord.ContentSize)
			{
				this.ThrowException("ParserDeferContentAsync");
			}
			ArrayList arrayList;
			List<object[]> list;
			this.BaseReadDeferableContentStart(bamlRecord, out arrayList, out list);
			long position2 = baseStream.Position;
			int num = (int)((long)bamlRecord.ContentSize - position2 + position);
			if (!this.ParserContext.OwnsBamlStream)
			{
				byte[] buffer = new byte[num];
				if (num > 0)
				{
					PackagingUtilities.ReliableRead(this.BinaryReader, buffer, 0, num);
				}
				throw new NotImplementedException();
			}
			throw new NotImplementedException();
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x001F7290 File Offset: 0x001F6290
		internal void BaseReadDeferableContentStart(BamlDeferableContentStartRecord bamlRecord, out ArrayList defKeyList, out List<object[]> staticResourceValuesList)
		{
			defKeyList = new ArrayList(Math.Max(5, bamlRecord.ContentSize / 400));
			staticResourceValuesList = new List<object[]>(defKeyList.Capacity);
			ArrayList arrayList = new ArrayList();
			BamlRecordType nextRecordType = this.GetNextRecordType();
			while (nextRecordType == BamlRecordType.DefAttributeKeyString || nextRecordType == BamlRecordType.DefAttributeKeyType || nextRecordType == BamlRecordType.KeyElementStart)
			{
				BamlRecord nextRecord = this.GetNextRecord();
				IBamlDictionaryKey bamlDictionaryKey = nextRecord as IBamlDictionaryKey;
				if (nextRecordType == BamlRecordType.KeyElementStart)
				{
					this.ReadKeyElementStartRecord((BamlKeyElementStartRecord)nextRecord);
					defKeyList.Add(nextRecord);
					bool flag = true;
					while (flag)
					{
						BamlRecord nextRecord2;
						if ((nextRecord2 = this.GetNextRecord()) == null)
						{
							break;
						}
						if (nextRecord2 is BamlKeyElementEndRecord)
						{
							object obj = this.GetCurrentObjectData();
							MarkupExtension markupExtension = obj as MarkupExtension;
							if (markupExtension != null)
							{
								obj = this.ProvideValueFromMarkupExtension(markupExtension, this.GetParentObjectData(), null);
							}
							bamlDictionaryKey.KeyObject = obj;
							this.PopContext();
							flag = false;
						}
						else
						{
							flag = this.ReadRecord(nextRecord2);
						}
					}
				}
				else
				{
					BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord = nextRecord as BamlDefAttributeKeyStringRecord;
					if (bamlDefAttributeKeyStringRecord != null)
					{
						bamlDefAttributeKeyStringRecord.Value = this.MapTable.GetStringFromStringId((int)bamlDefAttributeKeyStringRecord.ValueId);
						bamlDictionaryKey.KeyObject = this.XamlTypeMapper.GetDictionaryKey(bamlDefAttributeKeyStringRecord.Value, this.ParserContext);
						defKeyList.Add(bamlDefAttributeKeyStringRecord);
					}
					else
					{
						BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = nextRecord as BamlDefAttributeKeyTypeRecord;
						if (bamlDefAttributeKeyTypeRecord != null)
						{
							bamlDictionaryKey.KeyObject = this.MapTable.GetTypeFromId(bamlDefAttributeKeyTypeRecord.TypeId);
							defKeyList.Add(bamlDefAttributeKeyTypeRecord);
						}
						else
						{
							this.ThrowException("ParserUnexpInBAML", nextRecord.RecordType.ToString(CultureInfo.CurrentCulture));
						}
					}
				}
				nextRecordType = this.GetNextRecordType();
				if (!this.ParserContext.InDeferredSection)
				{
					while (nextRecordType == BamlRecordType.StaticResourceStart || nextRecordType == BamlRecordType.OptimizedStaticResource)
					{
						BamlRecord nextRecord3 = this.GetNextRecord();
						if (nextRecordType == BamlRecordType.StaticResourceStart)
						{
							BamlStaticResourceStartRecord bamlElementRecord = (BamlStaticResourceStartRecord)nextRecord3;
							this.BaseReadElementStartRecord(bamlElementRecord);
							bool flag2 = true;
							while (flag2)
							{
								BamlRecord nextRecord4;
								if ((nextRecord4 = this.GetNextRecord()) == null)
								{
									break;
								}
								if (nextRecord4.RecordType == BamlRecordType.StaticResourceEnd)
								{
									StaticResourceExtension value = (StaticResourceExtension)this.GetCurrentObjectData();
									arrayList.Add(value);
									this.PopContext();
									flag2 = false;
								}
								else
								{
									flag2 = this.ReadRecord(nextRecord4);
								}
							}
						}
						else
						{
							StaticResourceExtension value2 = (StaticResourceExtension)this.GetExtensionValue((IOptimizedMarkupExtension)nextRecord3, null);
							arrayList.Add(value2);
						}
						nextRecordType = this.GetNextRecordType();
					}
				}
				else
				{
					object[] array = this.ParserContext.StaticResourcesStack[this.ParserContext.StaticResourcesStack.Count - 1];
					while (nextRecordType == BamlRecordType.StaticResourceId)
					{
						BamlStaticResourceIdRecord bamlStaticResourceIdRecord = (BamlStaticResourceIdRecord)this.GetNextRecord();
						DeferredResourceReference deferredResourceReference = (DeferredResourceReference)array[(int)bamlStaticResourceIdRecord.StaticResourceId];
						arrayList.Add(new StaticResourceHolder(deferredResourceReference.Key, deferredResourceReference));
						nextRecordType = this.GetNextRecordType();
					}
				}
				staticResourceValuesList.Add(arrayList.ToArray());
				arrayList.Clear();
				nextRecordType = this.GetNextRecordType();
			}
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x001F7560 File Offset: 0x001F6560
		protected virtual void ReadStaticResourceIdRecord(BamlStaticResourceIdRecord bamlStaticResourceIdRecord)
		{
			object staticResourceFromId = this.GetStaticResourceFromId(bamlStaticResourceIdRecord.StaticResourceId);
			this.PushContext((ReaderFlags)8193, staticResourceFromId, null, 0);
			this.ReadElementEndRecord(true);
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x001F7590 File Offset: 0x001F6590
		protected virtual void ReadPropertyWithStaticResourceIdRecord(BamlPropertyWithStaticResourceIdRecord bamlPropertyWithStaticResourceIdRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			short attributeId = bamlPropertyWithStaticResourceIdRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			WpfPropertyDefinition propertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			object staticResourceFromId = this.GetStaticResourceFromId(bamlPropertyWithStaticResourceIdRecord.StaticResourceId);
			this.BaseReadOptimizedMarkupExtension(currentObjectData, attributeId, propertyDefinition, staticResourceFromId);
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x001F7610 File Offset: 0x001F6610
		internal StaticResourceHolder GetStaticResourceFromId(short staticResourceId)
		{
			DeferredResourceReference deferredResourceReference = (DeferredResourceReference)this.ParserContext.StaticResourcesStack[this.ParserContext.StaticResourcesStack.Count - 1][(int)staticResourceId];
			return new StaticResourceHolder(deferredResourceReference.Key, deferredResourceReference);
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x001F7654 File Offset: 0x001F6654
		internal virtual void ReadLiteralContentRecord(BamlLiteralContentRecord bamlLiteralContentRecord)
		{
			if (this.CurrentContext != null)
			{
				object obj = null;
				object obj2 = null;
				if (this.CurrentContext.ContentProperty != null)
				{
					obj = this.CurrentContext.ContentProperty;
					obj2 = this.CurrentContext.ObjectData;
				}
				else if (this.CurrentContext.ContextType == ReaderFlags.PropertyComplexClr || this.CurrentContext.ContextType == ReaderFlags.PropertyComplexDP)
				{
					obj = this.CurrentContext.ObjectData;
					obj2 = this.ParentContext.ObjectData;
				}
				IXmlSerializable xmlSerializable = null;
				PropertyInfo propertyInfo = obj as PropertyInfo;
				if (propertyInfo != null)
				{
					if (typeof(IXmlSerializable).IsAssignableFrom(propertyInfo.PropertyType))
					{
						xmlSerializable = (propertyInfo.GetValue(obj2, null) as IXmlSerializable);
					}
				}
				else
				{
					DependencyProperty dependencyProperty = obj as DependencyProperty;
					if (dependencyProperty != null && typeof(IXmlSerializable).IsAssignableFrom(dependencyProperty.PropertyType))
					{
						xmlSerializable = (((DependencyObject)obj2).GetValue(dependencyProperty) as IXmlSerializable);
					}
				}
				if (xmlSerializable != null)
				{
					FilteredXmlReader reader = new FilteredXmlReader(bamlLiteralContentRecord.Value, XmlNodeType.Element, this.ParserContext);
					xmlSerializable.ReadXml(reader);
					return;
				}
			}
			this.ThrowException("ParserUnexpInBAML", "BamlLiteralContent");
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x001F7778 File Offset: 0x001F6778
		protected virtual void ReadPropertyComplexStartRecord(BamlPropertyComplexStartRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.ClrObject != this.CurrentContext.ContextType && ReaderFlags.DependencyObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "PropertyComplexStart");
			}
			short attributeId = bamlPropertyRecord.AttributeId;
			WpfPropertyDefinition wpfPropertyDefinition = new WpfPropertyDefinition(this, attributeId, ReaderFlags.DependencyObject == this.CurrentContext.ContextType);
			if (wpfPropertyDefinition.DependencyProperty != null)
			{
				this.PushContext(ReaderFlags.PropertyComplexDP, wpfPropertyDefinition.AttributeInfo, wpfPropertyDefinition.PropertyType, 0);
			}
			else if (wpfPropertyDefinition.PropertyInfo != null)
			{
				this.PushContext(ReaderFlags.PropertyComplexClr, wpfPropertyDefinition.PropertyInfo, wpfPropertyDefinition.PropertyType, 0);
			}
			else if (wpfPropertyDefinition.AttachedPropertySetter != null)
			{
				this.PushContext(ReaderFlags.PropertyComplexClr, wpfPropertyDefinition.AttachedPropertySetter, wpfPropertyDefinition.PropertyType, 0);
			}
			else if (wpfPropertyDefinition.AttachedPropertyGetter != null)
			{
				this.PushContext(ReaderFlags.PropertyComplexClr, wpfPropertyDefinition.AttachedPropertyGetter, wpfPropertyDefinition.PropertyType, 0);
			}
			else
			{
				this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
			}
			this.CurrentContext.ElementNameOrPropertyName = wpfPropertyDefinition.Name;
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x001F78B0 File Offset: 0x001F68B0
		protected virtual void ReadPropertyComplexEndRecord()
		{
			this.PopContext();
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x001F78B8 File Offset: 0x001F68B8
		internal DependencyProperty GetCustomDependencyPropertyValue(BamlPropertyCustomRecord bamlPropertyRecord)
		{
			Type type = null;
			return this.GetCustomDependencyPropertyValue(bamlPropertyRecord, out type);
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x001F78D0 File Offset: 0x001F68D0
		internal DependencyProperty GetCustomDependencyPropertyValue(BamlPropertyCustomRecord bamlPropertyRecord, out Type declaringType)
		{
			declaringType = null;
			short serializerTypeId = bamlPropertyRecord.SerializerTypeId;
			DependencyProperty dependencyProperty;
			if (!bamlPropertyRecord.ValueObjectSet)
			{
				short memberId = this.BinaryReader.ReadInt16();
				string memberName = null;
				if (bamlPropertyRecord.IsValueTypeId)
				{
					memberName = this.BinaryReader.ReadString();
				}
				dependencyProperty = this.MapTable.GetDependencyPropertyValueFromId(memberId, memberName, out declaringType);
				if (dependencyProperty == null)
				{
					this.ThrowException("ParserCannotConvertPropertyValue", "Property", typeof(DependencyProperty).FullName);
				}
				bamlPropertyRecord.ValueObject = dependencyProperty;
				bamlPropertyRecord.ValueObjectSet = true;
			}
			else
			{
				dependencyProperty = (DependencyProperty)bamlPropertyRecord.ValueObject;
			}
			return dependencyProperty;
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x001F7964 File Offset: 0x001F6964
		internal object GetCustomValue(BamlPropertyCustomRecord bamlPropertyRecord, Type propertyType, string propertyName)
		{
			object result = null;
			if (!bamlPropertyRecord.ValueObjectSet)
			{
				Exception innerException = null;
				short serializerTypeId = bamlPropertyRecord.SerializerTypeId;
				try
				{
					if (serializerTypeId == 137)
					{
						result = this.GetCustomDependencyPropertyValue(bamlPropertyRecord);
					}
					else
					{
						result = bamlPropertyRecord.GetCustomValue(this.BinaryReader, propertyType, serializerTypeId, this);
					}
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
					{
						throw;
					}
					innerException = ex;
				}
				if (!bamlPropertyRecord.ValueObjectSet && !bamlPropertyRecord.IsRawEnumValueSet)
				{
					string message = SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						propertyName,
						propertyType.FullName
					});
					this.ThrowExceptionWithLine(message, innerException);
				}
			}
			else
			{
				result = bamlPropertyRecord.ValueObject;
			}
			return result;
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x001F7A14 File Offset: 0x001F6A14
		protected virtual void ReadPropertyCustomRecord(BamlPropertyCustomRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "PropertyCustom");
			}
			object obj = null;
			object currentObjectData = this.GetCurrentObjectData();
			short attributeId = bamlPropertyRecord.AttributeId;
			WpfPropertyDefinition wpfPropertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			if (!bamlPropertyRecord.ValueObjectSet)
			{
				try
				{
					obj = this.GetCustomValue(bamlPropertyRecord, wpfPropertyDefinition.PropertyType, wpfPropertyDefinition.Name);
					goto IL_D2;
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
					{
						throw;
					}
					string message = SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						wpfPropertyDefinition.Name,
						wpfPropertyDefinition.PropertyType.FullName
					});
					this.ThrowExceptionWithLine(message, ex);
					goto IL_D2;
				}
			}
			obj = bamlPropertyRecord.ValueObject;
			IL_D2:
			this.FreezeIfRequired(obj);
			if (wpfPropertyDefinition.DependencyProperty != null)
			{
				this.SetDependencyValue((DependencyObject)currentObjectData, wpfPropertyDefinition.DependencyProperty, obj);
				return;
			}
			if (wpfPropertyDefinition.PropertyInfo != null)
			{
				if (!wpfPropertyDefinition.IsInternal)
				{
					wpfPropertyDefinition.PropertyInfo.SetValue(currentObjectData, obj, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
					return;
				}
				if (!XamlTypeMapper.SetInternalPropertyValue(this.ParserContext, this.ParserContext.RootElement, wpfPropertyDefinition.PropertyInfo, currentObjectData, obj))
				{
					this.ThrowException("ParserCantSetAttribute", "property", wpfPropertyDefinition.Name, "set");
					return;
				}
			}
			else
			{
				if (wpfPropertyDefinition.AttachedPropertySetter != null)
				{
					wpfPropertyDefinition.AttachedPropertySetter.Invoke(null, new object[]
					{
						currentObjectData,
						obj
					});
					return;
				}
				this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
			}
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x001F7BD4 File Offset: 0x001F6BD4
		protected virtual void ReadPropertyRecord(BamlPropertyRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			this.ReadPropertyRecordBase(bamlPropertyRecord.Value, bamlPropertyRecord.AttributeId, 0);
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x001F7C30 File Offset: 0x001F6C30
		protected virtual void ReadPropertyConverterRecord(BamlPropertyWithConverterRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			this.ReadPropertyRecordBase(bamlPropertyRecord.Value, bamlPropertyRecord.AttributeId, bamlPropertyRecord.ConverterTypeId);
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x001F7C94 File Offset: 0x001F6C94
		protected virtual void ReadPropertyStringRecord(BamlPropertyStringReferenceRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			string propertyValueFromStringId = this.GetPropertyValueFromStringId(bamlPropertyRecord.StringId);
			this.ReadPropertyRecordBase(propertyValueFromStringId, bamlPropertyRecord.AttributeId, 0);
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x001F7CF8 File Offset: 0x001F6CF8
		private object GetInnerExtensionValue(IOptimizedMarkupExtension optimizedMarkupExtensionRecord)
		{
			short valueId = optimizedMarkupExtensionRecord.ValueId;
			object result;
			if (optimizedMarkupExtensionRecord.IsValueTypeExtension)
			{
				result = this.MapTable.GetTypeFromId(valueId);
			}
			else if (optimizedMarkupExtensionRecord.IsValueStaticExtension)
			{
				result = this.GetStaticExtensionValue(valueId);
			}
			else
			{
				result = this.MapTable.GetStringFromStringId((int)valueId);
			}
			return result;
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x001F7D48 File Offset: 0x001F6D48
		private object GetStaticExtensionValue(short memberId)
		{
			object result = null;
			if (memberId < 0)
			{
				short num = -memberId;
				bool flag;
				num = SystemResourceKey.GetSystemResourceKeyIdFromBamlId(num, out flag);
				if (flag)
				{
					result = SystemResourceKey.GetResourceKey(num);
				}
				else
				{
					result = SystemResourceKey.GetResource(num);
				}
			}
			else
			{
				BamlAttributeInfoRecord attributeInfoFromId = this.MapTable.GetAttributeInfoFromId(memberId);
				if (attributeInfoFromId != null)
				{
					result = new StaticExtension
					{
						MemberType = this.MapTable.GetTypeFromId(attributeInfoFromId.OwnerTypeId),
						Member = attributeInfoFromId.Name
					}.ProvideValue(null);
				}
			}
			return result;
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x001F7DC0 File Offset: 0x001F6DC0
		internal virtual object GetExtensionValue(IOptimizedMarkupExtension optimizedMarkupExtensionRecord, string propertyName)
		{
			object obj = null;
			short valueId = optimizedMarkupExtensionRecord.ValueId;
			short extensionTypeId = optimizedMarkupExtensionRecord.ExtensionTypeId;
			if (extensionTypeId != 189)
			{
				if (extensionTypeId != 602)
				{
					if (extensionTypeId == 603)
					{
						obj = new StaticResourceExtension(this.GetInnerExtensionValue(optimizedMarkupExtensionRecord));
					}
				}
				else
				{
					obj = this.GetStaticExtensionValue(valueId);
				}
			}
			else
			{
				obj = new DynamicResourceExtension(this.GetInnerExtensionValue(optimizedMarkupExtensionRecord));
			}
			if (obj == null)
			{
				string parameter = string.Empty;
				if (extensionTypeId != 189)
				{
					if (extensionTypeId != 602)
					{
						if (extensionTypeId == 603)
						{
							parameter = typeof(StaticResourceExtension).FullName;
						}
					}
					else
					{
						parameter = typeof(StaticExtension).FullName;
					}
				}
				else
				{
					parameter = typeof(DynamicResourceExtension).FullName;
				}
				this.ThrowException("ParserCannotConvertPropertyValue", propertyName, parameter);
			}
			return obj;
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x001F7E84 File Offset: 0x001F6E84
		protected virtual void ReadPropertyWithExtensionRecord(BamlPropertyWithExtensionRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			short attributeId = bamlPropertyRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			WpfPropertyDefinition propertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			object extensionValue = this.GetExtensionValue(bamlPropertyRecord, propertyDefinition.Name);
			this.BaseReadOptimizedMarkupExtension(currentObjectData, attributeId, propertyDefinition, extensionValue);
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x001F7F08 File Offset: 0x001F6F08
		private void BaseReadOptimizedMarkupExtension(object element, short attributeId, WpfPropertyDefinition propertyDefinition, object value)
		{
			try
			{
				MarkupExtension markupExtension = value as MarkupExtension;
				if (markupExtension != null)
				{
					value = this.ProvideValueFromMarkupExtension(markupExtension, element, propertyDefinition.DpOrPiOrMi);
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.TraceActivityItem(TraceMarkup.ProvideValue, new object[]
						{
							markupExtension,
							element,
							propertyDefinition.DpOrPiOrMi,
							value
						});
					}
				}
				if (!this.SetPropertyValue(element, propertyDefinition, value))
				{
					this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
				}
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				string message = SR.Get("ParserCannotConvertPropertyValue", new object[]
				{
					propertyDefinition.Name,
					propertyDefinition.PropertyType.FullName
				});
				this.ThrowExceptionWithLine(message, innerException);
			}
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x001F7FE8 File Offset: 0x001F6FE8
		private bool SetPropertyValue(object o, WpfPropertyDefinition propertyDefinition, object value)
		{
			bool result = true;
			if (propertyDefinition.DependencyProperty != null)
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, o, propertyDefinition.DependencyProperty.Name, value);
				}
				this.SetDependencyValue((DependencyObject)o, propertyDefinition.DependencyProperty, value);
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, o, propertyDefinition.DependencyProperty.Name, value);
				}
			}
			else if (propertyDefinition.PropertyInfo != null)
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, o, propertyDefinition.PropertyInfo.Name, value);
				}
				if (propertyDefinition.IsInternal)
				{
					if (!XamlTypeMapper.SetInternalPropertyValue(this.ParserContext, this.ParserContext.RootElement, propertyDefinition.PropertyInfo, o, value))
					{
						this.ThrowException("ParserCantSetAttribute", "property", propertyDefinition.Name, "set");
					}
				}
				else
				{
					propertyDefinition.PropertyInfo.SetValue(o, value, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
				}
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, o, propertyDefinition.PropertyInfo.Name, value);
				}
			}
			else if (propertyDefinition.AttachedPropertySetter != null)
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, o, propertyDefinition.AttachedPropertySetter.Name, value);
				}
				propertyDefinition.AttachedPropertySetter.Invoke(null, new object[]
				{
					o,
					value
				});
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, o, propertyDefinition.AttachedPropertySetter.Name, value);
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x001F819C File Offset: 0x001F719C
		protected virtual void ReadPropertyTypeRecord(BamlPropertyTypeReferenceRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			short attributeId = bamlPropertyRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			Type typeFromId = this.MapTable.GetTypeFromId(bamlPropertyRecord.TypeId);
			WpfPropertyDefinition propertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			try
			{
				if (!this.SetPropertyValue(currentObjectData, propertyDefinition, typeFromId))
				{
					this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
				}
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotSetValue", new object[]
				{
					currentObjectData.GetType().FullName,
					propertyDefinition.Name,
					typeFromId.Name
				}), innerException);
			}
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x001F82B0 File Offset: 0x001F72B0
		private void ReadPropertyRecordBase(string attribValue, short attributeId, short converterTypeId)
		{
			if (this.CurrentContext.CreateUsingTypeConverter)
			{
				this.ParserContext.XmlSpace = attribValue;
				return;
			}
			object currentObjectData = this.GetCurrentObjectData();
			WpfPropertyDefinition propertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			try
			{
				switch (propertyDefinition.AttributeUsage)
				{
				case BamlAttributeUsage.XmlLang:
					this.ParserContext.XmlLang = attribValue;
					break;
				case BamlAttributeUsage.XmlSpace:
					this.ParserContext.XmlSpace = attribValue;
					break;
				case BamlAttributeUsage.RuntimeName:
					this.DoRegisterName(attribValue, currentObjectData);
					break;
				}
				if (propertyDefinition.DependencyProperty != null)
				{
					object obj = this.ParseProperty((DependencyObject)currentObjectData, propertyDefinition.PropertyType, propertyDefinition.Name, propertyDefinition.DependencyProperty, attribValue, converterTypeId);
					if (obj != DependencyProperty.UnsetValue)
					{
						this.SetPropertyValue(currentObjectData, propertyDefinition, obj);
					}
				}
				else if (propertyDefinition.PropertyInfo != null)
				{
					object obj2 = this.ParseProperty(currentObjectData, propertyDefinition.PropertyType, propertyDefinition.Name, propertyDefinition.PropertyInfo, attribValue, converterTypeId);
					if (obj2 != DependencyProperty.UnsetValue)
					{
						this.SetPropertyValue(currentObjectData, propertyDefinition, obj2);
					}
				}
				else if (propertyDefinition.AttachedPropertySetter != null)
				{
					object obj3 = this.ParseProperty(currentObjectData, propertyDefinition.PropertyType, propertyDefinition.Name, propertyDefinition.AttachedPropertySetter, attribValue, converterTypeId);
					if (obj3 != DependencyProperty.UnsetValue)
					{
						this.SetPropertyValue(currentObjectData, propertyDefinition, obj3);
					}
				}
				else
				{
					bool flag = false;
					object obj4 = null;
					bool flag2 = false;
					if (this._componentConnector != null && this._rootElement != null)
					{
						obj4 = this.GetREOrEiFromAttributeId(attributeId, out flag2, out flag);
					}
					if (obj4 != null)
					{
						if (flag)
						{
							RoutedEvent routedEvent = obj4 as RoutedEvent;
							Delegate @delegate = XamlTypeMapper.CreateDelegate(this.ParserContext, routedEvent.HandlerType, this.ParserContext.RootElement, attribValue);
							if (@delegate == null)
							{
								this.ThrowException("ParserCantCreateDelegate", routedEvent.HandlerType.Name, attribValue);
							}
							UIElement uielement = currentObjectData as UIElement;
							if (uielement != null)
							{
								uielement.AddHandler(routedEvent, @delegate);
							}
							else
							{
								ContentElement contentElement = currentObjectData as ContentElement;
								if (contentElement != null)
								{
									contentElement.AddHandler(routedEvent, @delegate);
								}
								else
								{
									(currentObjectData as UIElement3D).AddHandler(routedEvent, @delegate);
								}
							}
						}
						else
						{
							EventInfo eventInfo = obj4 as EventInfo;
							Delegate @delegate = XamlTypeMapper.CreateDelegate(this.ParserContext, eventInfo.EventHandlerType, this.ParserContext.RootElement, attribValue);
							if (@delegate == null)
							{
								this.ThrowException("ParserCantCreateDelegate", eventInfo.EventHandlerType.Name, attribValue);
							}
							if (flag2)
							{
								if (!XamlTypeMapper.AddInternalEventHandler(this.ParserContext, this.ParserContext.RootElement, eventInfo, currentObjectData, @delegate))
								{
									this.ThrowException("ParserCantSetAttribute", "event", eventInfo.Name, "add");
								}
							}
							else
							{
								eventInfo.AddEventHandler(currentObjectData, @delegate);
							}
						}
					}
					else
					{
						this.ThrowException("ParserCantGetDPOrPi", propertyDefinition.Name);
					}
				}
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotSetValue", new object[]
				{
					currentObjectData.GetType().FullName,
					propertyDefinition.AttributeInfo.Name,
					attribValue
				}), innerException);
			}
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x001F85FC File Offset: 0x001F75FC
		private void DoRegisterName(string name, object element)
		{
			if (this.CurrentContext != null)
			{
				this.CurrentContext.ElementNameOrPropertyName = name;
			}
			if (this.ParserContext != null && this.ParserContext.NameScopeStack != null && this.ParserContext.NameScopeStack.Count != 0)
			{
				INameScope nameScope = this.ParserContext.NameScopeStack.Pop() as INameScope;
				if (NameScope.NameScopeFromObject(element) != null && this.ParserContext.NameScopeStack.Count != 0)
				{
					INameScope nameScope2 = this.ParserContext.NameScopeStack.Peek() as INameScope;
					if (nameScope2 != null)
					{
						nameScope2.RegisterName(name, element);
					}
				}
				else
				{
					nameScope.RegisterName(name, element);
				}
				this.ParserContext.NameScopeStack.Push(nameScope);
			}
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x001F86B4 File Offset: 0x001F76B4
		protected void ReadPropertyArrayStartRecord(BamlPropertyArrayStartRecord bamlPropertyArrayStartRecord)
		{
			short attributeId = bamlPropertyArrayStartRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			BamlCollectionHolder bamlCollectionHolder = new BamlCollectionHolder(this, currentObjectData, attributeId, false);
			if (!bamlCollectionHolder.PropertyType.IsArray)
			{
				this.ThrowException("ParserNoMatchingArray", this.GetPropertyNameFromAttributeId(attributeId));
			}
			this.PushContext((ReaderFlags)20488, bamlCollectionHolder, bamlCollectionHolder.PropertyType, 0);
			this.CurrentContext.ElementNameOrPropertyName = bamlCollectionHolder.AttributeName;
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x001F871C File Offset: 0x001F771C
		protected virtual void ReadPropertyArrayEndRecord()
		{
			BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)this.GetCurrentObjectData();
			if (bamlCollectionHolder.Collection == null)
			{
				this.InitPropertyCollection(bamlCollectionHolder, this.CurrentContext);
			}
			ArrayExtension arrayExt = bamlCollectionHolder.ArrayExt;
			bamlCollectionHolder.Collection = this.ProvideValueFromMarkupExtension(arrayExt, bamlCollectionHolder, null);
			bamlCollectionHolder.SetPropertyValue();
			this.PopContext();
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x001F876C File Offset: 0x001F776C
		protected virtual void ReadPropertyIListStartRecord(BamlPropertyIListStartRecord bamlPropertyIListStartRecord)
		{
			short attributeId = bamlPropertyIListStartRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			BamlCollectionHolder bamlCollectionHolder = new BamlCollectionHolder(this, currentObjectData, attributeId);
			Type type = bamlCollectionHolder.PropertyType;
			ReaderFlags readerFlags = ReaderFlags.Unknown;
			if (typeof(IList).IsAssignableFrom(type))
			{
				readerFlags = ReaderFlags.PropertyIList;
			}
			else if (BamlRecordManager.TreatAsIAddChild(type))
			{
				readerFlags = ReaderFlags.PropertyIAddChild;
				bamlCollectionHolder.Collection = bamlCollectionHolder.DefaultCollection;
				bamlCollectionHolder.ReadOnly = true;
			}
			else if (typeof(IEnumerable).IsAssignableFrom(type) && BamlRecordManager.AsIAddChild(this.GetCurrentObjectData()) != null)
			{
				readerFlags = ReaderFlags.PropertyIAddChild;
				bamlCollectionHolder.Collection = this.CurrentContext.ObjectData;
				bamlCollectionHolder.ReadOnly = true;
				type = this.CurrentContext.ObjectData.GetType();
			}
			else
			{
				this.ThrowException("ParserReadOnlyProp", bamlCollectionHolder.PropertyDefinition.Name);
			}
			this.PushContext(readerFlags | ReaderFlags.CollectionHolder, bamlCollectionHolder, type, 0);
			this.CurrentContext.ElementNameOrPropertyName = bamlCollectionHolder.AttributeName;
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x001F8865 File Offset: 0x001F7865
		protected virtual void ReadPropertyIListEndRecord()
		{
			this.SetCollectionPropertyValue(this.CurrentContext);
			this.PopContext();
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x001F887C File Offset: 0x001F787C
		protected virtual void ReadPropertyIDictionaryStartRecord(BamlPropertyIDictionaryStartRecord bamlPropertyIDictionaryStartRecord)
		{
			short attributeId = bamlPropertyIDictionaryStartRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			BamlCollectionHolder bamlCollectionHolder = new BamlCollectionHolder(this, currentObjectData, attributeId);
			this.PushContext((ReaderFlags)28680, bamlCollectionHolder, bamlCollectionHolder.PropertyType, 0);
			this.CurrentContext.ElementNameOrPropertyName = bamlCollectionHolder.AttributeName;
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x001F8865 File Offset: 0x001F7865
		protected virtual void ReadPropertyIDictionaryEndRecord()
		{
			this.SetCollectionPropertyValue(this.CurrentContext);
			this.PopContext();
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x001F88C4 File Offset: 0x001F78C4
		private void SetCollectionPropertyValue(ReaderContextStackData context)
		{
			BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)context.ObjectData;
			if (bamlCollectionHolder.Collection == null)
			{
				this.InitPropertyCollection(bamlCollectionHolder, context);
			}
			if (!bamlCollectionHolder.ReadOnly && bamlCollectionHolder.Collection != bamlCollectionHolder.DefaultCollection)
			{
				bamlCollectionHolder.SetPropertyValue();
			}
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x001F890C File Offset: 0x001F790C
		private void InitPropertyCollection(BamlCollectionHolder holder, ReaderContextStackData context)
		{
			if (context.ContextType == ReaderFlags.PropertyArray)
			{
				holder.Collection = new ArrayExtension
				{
					Type = context.ExpectedType.GetElementType()
				};
			}
			else if (holder.DefaultCollection != null)
			{
				holder.Collection = holder.DefaultCollection;
			}
			else
			{
				this.ThrowException("ParserNullPropertyCollection", holder.PropertyDefinition.Name);
			}
			context.ExpectedType = null;
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x001F897C File Offset: 0x001F797C
		private BamlCollectionHolder GetCollectionHolderFromContext(ReaderContextStackData context, bool toInsert)
		{
			BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)context.ObjectData;
			if (bamlCollectionHolder.Collection == null && toInsert)
			{
				this.InitPropertyCollection(bamlCollectionHolder, context);
			}
			if (toInsert && bamlCollectionHolder.IsClosed)
			{
				this.ThrowException("ParserPropertyCollectionClosed", bamlCollectionHolder.PropertyDefinition.Name);
			}
			return bamlCollectionHolder;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x001F89D0 File Offset: 0x001F79D0
		protected IDictionary GetDictionaryFromContext(ReaderContextStackData context, bool toInsert)
		{
			IDictionary result = null;
			if (context != null)
			{
				if (context.CheckFlag(ReaderFlags.IDictionary))
				{
					result = (IDictionary)this.GetObjectDataFromContext(context);
				}
				else if (context.ContextType == ReaderFlags.PropertyIDictionary)
				{
					result = this.GetCollectionHolderFromContext(context, toInsert).Dictionary;
				}
			}
			return result;
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x001F8A18 File Offset: 0x001F7A18
		private IList GetListFromContext(ReaderContextStackData context)
		{
			IList result = null;
			if (context != null)
			{
				if (context.CheckFlag(ReaderFlags.IList))
				{
					result = (IList)this.GetObjectDataFromContext(context);
				}
				else if (context.ContextType == ReaderFlags.PropertyIList)
				{
					result = this.GetCollectionHolderFromContext(context, true).List;
				}
			}
			return result;
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x001F8A60 File Offset: 0x001F7A60
		private IAddChild GetIAddChildFromContext(ReaderContextStackData context)
		{
			IAddChild result = null;
			if (context != null)
			{
				if (context.CheckFlag(ReaderFlags.IAddChild))
				{
					result = BamlRecordManager.AsIAddChild(context.ObjectData);
				}
				else if (context.ContextType == ReaderFlags.PropertyIAddChild)
				{
					result = BamlRecordManager.AsIAddChild(this.GetCollectionHolderFromContext(context, false).Collection);
				}
			}
			return result;
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x001F8AB0 File Offset: 0x001F7AB0
		private ArrayExtension GetArrayExtensionFromContext(ReaderContextStackData context)
		{
			ArrayExtension result = null;
			if (context != null)
			{
				result = (context.ObjectData as ArrayExtension);
				if (context.CheckFlag(ReaderFlags.ArrayExt))
				{
					result = (ArrayExtension)context.ObjectData;
				}
				else if (context.ContextType == ReaderFlags.PropertyArray)
				{
					result = this.GetCollectionHolderFromContext(context, true).ArrayExt;
				}
			}
			return result;
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x001F8B04 File Offset: 0x001F7B04
		protected virtual void ReadDefAttributeRecord(BamlDefAttributeRecord bamlDefAttributeRecord)
		{
			bamlDefAttributeRecord.Name = this.MapTable.GetStringFromStringId((int)bamlDefAttributeRecord.NameId);
			if (bamlDefAttributeRecord.Name == "Key")
			{
				object dictionaryKey = this.XamlTypeMapper.GetDictionaryKey(bamlDefAttributeRecord.Value, this.ParserContext);
				if (dictionaryKey == null)
				{
					this.ThrowException("ParserNoResource", bamlDefAttributeRecord.Value);
				}
				this.SetKeyOnContext(dictionaryKey, bamlDefAttributeRecord.Value, this.CurrentContext, this.ParentContext);
				return;
			}
			if (bamlDefAttributeRecord.Name == "Uid" || bamlDefAttributeRecord.NameId == BamlMapTable.UidStringId)
			{
				if (this.CurrentContext == null)
				{
					return;
				}
				this.CurrentContext.Uid = bamlDefAttributeRecord.Value;
				UIElement uielement = this.CurrentContext.ObjectData as UIElement;
				if (uielement != null)
				{
					this.SetDependencyValue(uielement, UIElement.UidProperty, bamlDefAttributeRecord.Value);
					return;
				}
			}
			else
			{
				if (bamlDefAttributeRecord.Name == "Shared")
				{
					this.ThrowException("ParserDefSharedOnlyInCompiled");
					return;
				}
				if (bamlDefAttributeRecord.Name == "Name")
				{
					object currentObjectData = this.GetCurrentObjectData();
					if (currentObjectData != null)
					{
						this.DoRegisterName(bamlDefAttributeRecord.Value, currentObjectData);
						return;
					}
				}
				else
				{
					this.ThrowException("ParserUnknownDefAttribute", bamlDefAttributeRecord.Name);
				}
			}
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x001F8C3C File Offset: 0x001F7C3C
		protected virtual void ReadDefAttributeKeyTypeRecord(BamlDefAttributeKeyTypeRecord bamlDefAttributeRecord)
		{
			Type typeFromId = this.MapTable.GetTypeFromId(bamlDefAttributeRecord.TypeId);
			if (typeFromId == null)
			{
				this.ThrowException("ParserNoResource", "Key");
			}
			this.SetKeyOnContext(typeFromId, "Key", this.CurrentContext, this.ParentContext);
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x001F8C8C File Offset: 0x001F7C8C
		private void SetKeyOnContext(object key, string attributeName, ReaderContextStackData context, ReaderContextStackData parentContext)
		{
			try
			{
				this.GetDictionaryFromContext(parentContext, true);
			}
			catch (XamlParseException innerException)
			{
				if (parentContext.CheckFlag(ReaderFlags.CollectionHolder))
				{
					BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)parentContext.ObjectData;
					object objectData = context.ObjectData;
					if (objectData != null && objectData == bamlCollectionHolder.Dictionary)
					{
						this.ThrowExceptionWithLine(SR.Get("ParserKeyOnExplicitDictionary", new object[]
						{
							attributeName,
							objectData.GetType().ToString(),
							bamlCollectionHolder.PropertyDefinition.Name
						}), innerException);
					}
				}
				this.ThrowExceptionWithLine(SR.Get("ParserNoMatchingIDictionary", new object[]
				{
					attributeName
				}), innerException);
			}
			context.Key = key;
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x001F8D44 File Offset: 0x001F7D44
		protected virtual void ReadTextRecord(BamlTextRecord bamlTextRecord)
		{
			BamlTextWithIdRecord bamlTextWithIdRecord = bamlTextRecord as BamlTextWithIdRecord;
			if (bamlTextWithIdRecord != null)
			{
				bamlTextWithIdRecord.Value = this.MapTable.GetStringFromStringId((int)bamlTextWithIdRecord.ValueId);
			}
			if (this.CurrentContext == null)
			{
				this._componentConnector = null;
				this._rootElement = null;
				this.RootList.Add(bamlTextRecord.Value);
				return;
			}
			short converterTypeId = 0;
			BamlTextWithConverterRecord bamlTextWithConverterRecord = bamlTextRecord as BamlTextWithConverterRecord;
			if (bamlTextWithConverterRecord != null)
			{
				converterTypeId = bamlTextWithConverterRecord.ConverterTypeId;
			}
			ReaderFlags contextType = this.CurrentContext.ContextType;
			if (contextType <= ReaderFlags.PropertyComplexClr)
			{
				if (contextType != ReaderFlags.DependencyObject && contextType != ReaderFlags.ClrObject)
				{
					if (contextType == ReaderFlags.PropertyComplexClr)
					{
						if (null == this.CurrentContext.ExpectedType)
						{
							this.ThrowException("ParserNoComplexMulti", this.GetPropNameFrom(this.CurrentContext.ObjectData));
						}
						object objectFromString = this.GetObjectFromString(this.CurrentContext.ExpectedType, bamlTextRecord.Value, converterTypeId);
						if (DependencyProperty.UnsetValue != objectFromString)
						{
							this.SetClrComplexProperty(objectFromString);
							return;
						}
						this.ThrowException("ParserCantCreateTextComplexProp", this.CurrentContext.ExpectedType.FullName, bamlTextRecord.Value);
						return;
					}
				}
				else if (this.CurrentContext.CreateUsingTypeConverter)
				{
					object objectFromString2 = this.GetObjectFromString(this.CurrentContext.ExpectedType, bamlTextRecord.Value, converterTypeId);
					if (DependencyProperty.UnsetValue != objectFromString2)
					{
						this.CurrentContext.ObjectData = objectFromString2;
						this.CurrentContext.ExpectedType = null;
						return;
					}
					this.ThrowException("ParserCannotConvertString", bamlTextRecord.Value, this.CurrentContext.ExpectedType.FullName);
					return;
				}
				else
				{
					object currentObjectData = this.GetCurrentObjectData();
					if (currentObjectData == null)
					{
						this.ThrowException("ParserCantCreateInstanceType", this.CurrentContext.ExpectedType.FullName);
					}
					IAddChild iaddChildFromContext = this.GetIAddChildFromContext(this.CurrentContext);
					if (iaddChildFromContext != null)
					{
						iaddChildFromContext.AddText(bamlTextRecord.Value);
						return;
					}
					if (this.CurrentContext.ContentProperty != null)
					{
						this.AddToContentProperty(currentObjectData, this.CurrentContext.ContentProperty, bamlTextRecord.Value);
						return;
					}
					this.ThrowException("ParserIAddChildText", currentObjectData.GetType().FullName, bamlTextRecord.Value);
					return;
				}
			}
			else if (contextType <= ReaderFlags.PropertyIList)
			{
				if (contextType != ReaderFlags.PropertyComplexDP)
				{
					if (contextType == ReaderFlags.PropertyIList)
					{
						BamlCollectionHolder collectionHolderFromContext = this.GetCollectionHolderFromContext(this.CurrentContext, true);
						if (collectionHolderFromContext.List == null)
						{
							this.ThrowException("ParserNoMatchingIList", "?");
						}
						collectionHolderFromContext.List.Add(bamlTextRecord.Value);
						return;
					}
				}
				else
				{
					if (null == this.CurrentContext.ExpectedType)
					{
						this.ThrowException("ParserNoComplexMulti", this.GetPropNameFrom(this.CurrentContext.ObjectData));
					}
					BamlAttributeInfoRecord bamlAttributeInfoRecord = this.CurrentContext.ObjectData as BamlAttributeInfoRecord;
					object obj = this.ParseProperty((DependencyObject)this.GetParentObjectData(), bamlAttributeInfoRecord.DP.PropertyType, bamlAttributeInfoRecord.DP.Name, bamlAttributeInfoRecord.DP, bamlTextRecord.Value, converterTypeId);
					if (DependencyProperty.UnsetValue != obj)
					{
						this.SetDependencyComplexProperty(obj);
						return;
					}
					this.ThrowException("ParserCantCreateTextComplexProp", bamlAttributeInfoRecord.OwnerType.FullName, bamlTextRecord.Value);
					return;
				}
			}
			else
			{
				if (contextType == ReaderFlags.PropertyIAddChild)
				{
					IAddChild addChild = BamlRecordManager.AsIAddChild(this.GetCollectionHolderFromContext(this.CurrentContext, true).Collection);
					if (addChild == null)
					{
						this.ThrowException("ParserNoMatchingIList", "?");
					}
					addChild.AddText(bamlTextRecord.Value);
					return;
				}
				if (contextType == ReaderFlags.ConstructorParams)
				{
					this.SetConstructorParameter(bamlTextRecord.Value);
					return;
				}
			}
			this.ThrowException("ParserUnexpInBAML", "Text");
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x001F90CC File Offset: 0x001F80CC
		protected virtual void ReadPresentationOptionsAttributeRecord(BamlPresentationOptionsAttributeRecord bamlPresentationOptionsAttributeRecord)
		{
			bamlPresentationOptionsAttributeRecord.Name = this.MapTable.GetStringFromStringId((int)bamlPresentationOptionsAttributeRecord.NameId);
			if (bamlPresentationOptionsAttributeRecord.Name == "Freeze")
			{
				bool freezeFreezables = bool.Parse(bamlPresentationOptionsAttributeRecord.Value);
				this._parserContext.FreezeFreezables = freezeFreezables;
				return;
			}
			this.ThrowException("ParserUnknownPresentationOptionsAttribute", bamlPresentationOptionsAttributeRecord.Name);
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x001F912C File Offset: 0x001F812C
		private void SetDependencyComplexProperty(object o)
		{
			object parentObjectData = this.GetParentObjectData();
			BamlAttributeInfoRecord attribInfo = (BamlAttributeInfoRecord)this.GetCurrentObjectData();
			this.SetDependencyComplexProperty(parentObjectData, attribInfo, o);
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x001F9158 File Offset: 0x001F8158
		private void SetDependencyComplexProperty(object currentTarget, BamlAttributeInfoRecord attribInfo, object o)
		{
			DependencyProperty dependencyProperty = (currentTarget is DependencyObject) ? attribInfo.DP : null;
			PropertyInfo propInfo = attribInfo.PropInfo;
			MethodInfo methodInfo = null;
			try
			{
				MarkupExtension markupExtension = o as MarkupExtension;
				if (markupExtension != null)
				{
					o = this.ProvideValueFromMarkupExtension(markupExtension, currentTarget, dependencyProperty);
				}
				Type propertyType = null;
				if (dependencyProperty != null)
				{
					propertyType = dependencyProperty.PropertyType;
				}
				else if (propInfo != null)
				{
					propertyType = propInfo.PropertyType;
				}
				else
				{
					if (attribInfo.AttachedPropertySetter == null)
					{
						this.XamlTypeMapper.UpdateAttachedPropertySetter(attribInfo);
					}
					methodInfo = attribInfo.AttachedPropertySetter;
					if (methodInfo != null)
					{
						propertyType = methodInfo.GetParameters()[1].ParameterType;
					}
				}
				o = this.OptionallyMakeNullable(propertyType, o, attribInfo.Name);
				if (dependencyProperty != null)
				{
					this.SetDependencyValue((DependencyObject)currentTarget, dependencyProperty, o);
				}
				else if (propInfo != null)
				{
					propInfo.SetValue(currentTarget, o, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
				}
				else if (methodInfo != null)
				{
					methodInfo.Invoke(null, new object[]
					{
						currentTarget,
						o
					});
				}
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotSetValue", new object[]
				{
					currentTarget.GetType().FullName,
					attribInfo.Name,
					o
				}), innerException);
			}
			this.CurrentContext.ExpectedType = null;
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x001F92D0 File Offset: 0x001F82D0
		internal static bool IsNullable(Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == BamlRecordReader.NullableType;
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x001F92EC File Offset: 0x001F82EC
		internal object OptionallyMakeNullable(Type propertyType, object o, string propName)
		{
			object result = o;
			if (!BamlRecordReader.TryOptionallyMakeNullable(propertyType, propName, ref result))
			{
				this.ThrowException("ParserBadNullableType", propName, propertyType.GetGenericArguments()[0].Name, o.GetType().FullName);
			}
			return result;
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x001F932B File Offset: 0x001F832B
		internal static bool TryOptionallyMakeNullable(Type propertyType, string propName, ref object o)
		{
			return o == null || !BamlRecordReader.IsNullable(propertyType) || o is Expression || o is MarkupExtension || !(propertyType.GetGenericArguments()[0] != o.GetType());
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x001F9364 File Offset: 0x001F8364
		internal virtual void SetClrComplexPropertyCore(object parentObject, object value, MemberInfo memberInfo)
		{
			MarkupExtension markupExtension = value as MarkupExtension;
			if (markupExtension != null)
			{
				value = this.ProvideValueFromMarkupExtension(markupExtension, parentObject, memberInfo);
			}
			if (memberInfo is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				value = this.OptionallyMakeNullable(propertyInfo.PropertyType, value, propertyInfo.Name);
				propertyInfo.SetValue(parentObject, value, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
				return;
			}
			MethodInfo methodInfo = (MethodInfo)memberInfo;
			value = this.OptionallyMakeNullable(methodInfo.GetParameters()[1].ParameterType, value, methodInfo.Name.Substring("Set".Length));
			methodInfo.Invoke(null, new object[]
			{
				parentObject,
				value
			});
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x001F9404 File Offset: 0x001F8404
		private void SetClrComplexProperty(object o)
		{
			MemberInfo memberInfo = (MemberInfo)this.GetCurrentObjectData();
			object parentObjectData = this.GetParentObjectData();
			this.SetClrComplexProperty(parentObjectData, memberInfo, o);
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x001F9430 File Offset: 0x001F8430
		private void SetClrComplexProperty(object parentObject, MemberInfo memberInfo, object o)
		{
			try
			{
				this.SetClrComplexPropertyCore(parentObject, o, memberInfo);
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotSetValue", new object[]
				{
					parentObject.GetType().FullName,
					memberInfo.Name,
					o
				}), innerException);
			}
			this.CurrentContext.ExpectedType = null;
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x001F94BC File Offset: 0x001F84BC
		private void SetConstructorParameter(object o)
		{
			MarkupExtension markupExtension = o as MarkupExtension;
			if (markupExtension != null)
			{
				o = this.ProvideValueFromMarkupExtension(markupExtension, null, null);
			}
			if (this.CurrentContext.ObjectData == null)
			{
				this.CurrentContext.ObjectData = o;
				this.CurrentContext.SetFlag(ReaderFlags.SingletonConstructorParam);
				return;
			}
			if (this.CurrentContext.CheckFlag(ReaderFlags.SingletonConstructorParam))
			{
				ArrayList arrayList = new ArrayList(2);
				arrayList.Add(this.CurrentContext.ObjectData);
				arrayList.Add(o);
				this.CurrentContext.ObjectData = arrayList;
				this.CurrentContext.ClearFlag(ReaderFlags.SingletonConstructorParam);
				return;
			}
			((ArrayList)this.CurrentContext.ObjectData).Add(o);
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x001F9570 File Offset: 0x001F8570
		protected void SetXmlnsOnCurrentObject(BamlXmlnsPropertyRecord xmlnsRecord)
		{
			DependencyObject dependencyObject = this.CurrentContext.ObjectData as DependencyObject;
			if (dependencyObject != null)
			{
				XmlnsDictionary xmlnsDictionary = XmlAttributeProperties.GetXmlnsDictionary(dependencyObject);
				if (xmlnsDictionary != null)
				{
					xmlnsDictionary.Unseal();
					xmlnsDictionary[xmlnsRecord.Prefix] = xmlnsRecord.XmlNamespace;
					xmlnsDictionary.Seal();
					return;
				}
				xmlnsDictionary = new XmlnsDictionary();
				xmlnsDictionary[xmlnsRecord.Prefix] = xmlnsRecord.XmlNamespace;
				xmlnsDictionary.Seal();
				XmlAttributeProperties.SetXmlnsDictionary(dependencyObject, xmlnsDictionary);
			}
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x001F95E0 File Offset: 0x001F85E0
		internal object ParseProperty(object element, Type propertyType, string propertyName, object dpOrPi, string attribValue, short converterTypeId)
		{
			object obj = null;
			try
			{
				obj = this.XamlTypeMapper.ParseProperty(element, propertyType, propertyName, dpOrPi, this.TypeConvertContext, this.ParserContext, attribValue, converterTypeId);
				this.FreezeIfRequired(obj);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				this.ThrowPropertyParseError(ex, propertyName, attribValue, element, propertyType);
			}
			if (DependencyProperty.UnsetValue == obj)
			{
				this.ThrowException("ParserNullReturned", propertyName, attribValue);
			}
			return obj;
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x001F9664 File Offset: 0x001F8664
		private void ThrowPropertyParseError(Exception e, string propertyName, string attribValue, object element, Type propertyType)
		{
			string message = string.Empty;
			if (this.FindResourceInParserStack(attribValue.Trim(), false, false) == DependencyProperty.UnsetValue)
			{
				if (propertyType == typeof(Type))
				{
					message = SR.Get("ParserErrorParsingAttribType", new object[]
					{
						propertyName,
						attribValue
					});
				}
				else
				{
					message = SR.Get("ParserErrorParsingAttrib", new object[]
					{
						propertyName,
						attribValue,
						propertyType.Name
					});
				}
			}
			else
			{
				message = SR.Get("ParserErrorParsingAttribType", new object[]
				{
					propertyName,
					attribValue
				});
			}
			this.ThrowExceptionWithLine(message, e);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x001F9700 File Offset: 0x001F8700
		private object GetObjectFromString(Type type, string s, short converterTypeId)
		{
			object unsetValue = DependencyProperty.UnsetValue;
			return this.ParserContext.XamlTypeMapper.ParseProperty(null, type, string.Empty, null, this.TypeConvertContext, this.ParserContext, s, converterTypeId);
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x001F973C File Offset: 0x001F873C
		private static object Lookup(IDictionary dictionary, object key, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary resourceDictionary;
			if (allowDeferredResourceReference && (resourceDictionary = (dictionary as ResourceDictionary)) != null)
			{
				bool flag;
				return resourceDictionary.FetchResource(key, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			if (!mustReturnDeferredResourceReference)
			{
				return dictionary[key];
			}
			return new DeferredResourceReferenceHolder(key, dictionary[key]);
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x001F977C File Offset: 0x001F877C
		internal object FindResourceInParserStack(object resourceNameObject, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			object obj = DependencyProperty.UnsetValue;
			ParserStack parserStack = this.ReaderContextStack;
			BamlRecordReader bamlRecordReader = this;
			while (parserStack != null)
			{
				for (int i = parserStack.Count - 1; i >= 0; i--)
				{
					ReaderContextStackData readerContextStackData = (ReaderContextStackData)parserStack[i];
					IDictionary dictionaryFromContext = this.GetDictionaryFromContext(readerContextStackData, false);
					if (dictionaryFromContext != null && dictionaryFromContext.Contains(resourceNameObject))
					{
						obj = BamlRecordReader.Lookup(dictionaryFromContext, resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					}
					else if (readerContextStackData.ContextType == ReaderFlags.DependencyObject)
					{
						FrameworkElement frameworkElement;
						FrameworkContentElement frameworkContentElement;
						Helper.DowncastToFEorFCE((DependencyObject)readerContextStackData.ObjectData, out frameworkElement, out frameworkContentElement, false);
						if (frameworkElement != null)
						{
							obj = frameworkElement.FindResourceOnSelf(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
						}
						else if (frameworkContentElement != null)
						{
							obj = frameworkContentElement.FindResourceOnSelf(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
						}
					}
					else if (readerContextStackData.CheckFlag(ReaderFlags.StyleObject))
					{
						obj = ((Style)readerContextStackData.ObjectData).FindResource(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					}
					else if (readerContextStackData.CheckFlag(ReaderFlags.FrameworkTemplateObject))
					{
						obj = ((FrameworkTemplate)readerContextStackData.ObjectData).FindResource(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					}
					if (obj != DependencyProperty.UnsetValue)
					{
						return obj;
					}
				}
				bool flag = false;
				while (bamlRecordReader._previousBamlRecordReader != null)
				{
					bamlRecordReader = bamlRecordReader._previousBamlRecordReader;
					if (bamlRecordReader.ReaderContextStack != parserStack)
					{
						parserStack = bamlRecordReader.ReaderContextStack;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					parserStack = null;
				}
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x001F98C4 File Offset: 0x001F88C4
		private object FindResourceInRootOrAppOrTheme(object resourceNameObject, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			object obj;
			if (!SystemResources.IsSystemResourcesParsing)
			{
				object obj2;
				obj = FrameworkElement.FindResourceFromAppOrSystem(resourceNameObject, out obj2, false, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			}
			else
			{
				obj = SystemResources.FindResourceInternal(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			}
			if (obj != null)
			{
				return obj;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x001F98FC File Offset: 0x001F88FC
		internal object FindResourceInParentChain(object resourceNameObject, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			object obj = this.FindResourceInParserStack(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			if (obj == DependencyProperty.UnsetValue)
			{
				obj = this.FindResourceInRootOrAppOrTheme(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			}
			if (obj == DependencyProperty.UnsetValue && mustReturnDeferredResourceReference)
			{
				obj = new DeferredResourceReferenceHolder(resourceNameObject, DependencyProperty.UnsetValue);
			}
			return obj;
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x001F9940 File Offset: 0x001F8940
		internal object LoadResource(string resourceNameString)
		{
			string keyString = resourceNameString.Substring(1, resourceNameString.Length - 2);
			object dictionaryKey = this.XamlTypeMapper.GetDictionaryKey(keyString, this.ParserContext);
			if (dictionaryKey == null)
			{
				this.ThrowException("ParserNoResource", resourceNameString);
			}
			object obj = this.FindResourceInParentChain(dictionaryKey, false, false);
			if (obj == DependencyProperty.UnsetValue)
			{
				this.ThrowException("ParserNoResource", "{" + dictionaryKey.ToString() + "}");
			}
			return obj;
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x001F99B0 File Offset: 0x001F89B0
		private object GetObjectDataFromContext(ReaderContextStackData context)
		{
			if (context.ObjectData == null && null != context.ExpectedType)
			{
				context.ObjectData = this.CreateInstanceFromType(context.ExpectedType, context.ExpectedTypeId, true);
				if (context.ObjectData == null)
				{
					this.ThrowException("ParserCantCreateInstanceType", context.ExpectedType.FullName);
				}
				context.ExpectedType = null;
				this.ElementInitialize(context.ObjectData, null);
			}
			return context.ObjectData;
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x001F9A25 File Offset: 0x001F8A25
		internal object GetCurrentObjectData()
		{
			return this.GetObjectDataFromContext(this.CurrentContext);
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x001F9A33 File Offset: 0x001F8A33
		protected object GetParentObjectData()
		{
			return this.GetObjectDataFromContext(this.ParentContext);
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x001F9A41 File Offset: 0x001F8A41
		internal void PushContext(ReaderFlags contextFlags, object contextData, Type expectedType, short expectedTypeId)
		{
			this.PushContext(contextFlags, contextData, expectedType, expectedTypeId, false);
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x001F9A50 File Offset: 0x001F8A50
		internal void PushContext(ReaderFlags contextFlags, object contextData, Type expectedType, short expectedTypeId, bool createUsingTypeConverter)
		{
			List<ReaderContextStackData> stackDataFactoryCache = BamlRecordReader._stackDataFactoryCache;
			ReaderContextStackData readerContextStackData;
			lock (stackDataFactoryCache)
			{
				if (BamlRecordReader._stackDataFactoryCache.Count == 0)
				{
					readerContextStackData = new ReaderContextStackData();
				}
				else
				{
					readerContextStackData = BamlRecordReader._stackDataFactoryCache[BamlRecordReader._stackDataFactoryCache.Count - 1];
					BamlRecordReader._stackDataFactoryCache.RemoveAt(BamlRecordReader._stackDataFactoryCache.Count - 1);
				}
			}
			readerContextStackData.ContextFlags = contextFlags;
			readerContextStackData.ObjectData = contextData;
			readerContextStackData.ExpectedType = expectedType;
			readerContextStackData.ExpectedTypeId = expectedTypeId;
			readerContextStackData.CreateUsingTypeConverter = createUsingTypeConverter;
			this.ReaderContextStack.Push(readerContextStackData);
			this.ParserContext.PushScope();
			INameScope nameScope = NameScope.NameScopeFromObject(contextData);
			if (nameScope != null)
			{
				this.ParserContext.NameScopeStack.Push(nameScope);
			}
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x001F9B24 File Offset: 0x001F8B24
		internal void PopContext()
		{
			ReaderContextStackData readerContextStackData = (ReaderContextStackData)this.ReaderContextStack.Pop();
			if (NameScope.NameScopeFromObject(readerContextStackData.ObjectData) != null)
			{
				this.ParserContext.NameScopeStack.Pop();
			}
			this.ParserContext.PopScope();
			readerContextStackData.ClearData();
			List<ReaderContextStackData> stackDataFactoryCache = BamlRecordReader._stackDataFactoryCache;
			lock (stackDataFactoryCache)
			{
				BamlRecordReader._stackDataFactoryCache.Add(readerContextStackData);
			}
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x001F9BA8 File Offset: 0x001F8BA8
		private Uri GetBaseUri()
		{
			Uri uri = this.ParserContext.BaseUri;
			if (uri == null)
			{
				uri = BindUriHelper.BaseUri;
			}
			else if (!uri.IsAbsoluteUri)
			{
				uri = new Uri(BindUriHelper.BaseUri, uri);
			}
			return uri;
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x001F9BE8 File Offset: 0x001F8BE8
		private bool ElementInitialize(object element, string name)
		{
			bool result = false;
			ISupportInitialize supportInitialize = element as ISupportInitialize;
			if (supportInitialize != null)
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.BeginInit, supportInitialize);
				}
				supportInitialize.BeginInit();
				result = true;
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.BeginInit, supportInitialize);
				}
			}
			if (name != null)
			{
				this.DoRegisterName(name, element);
			}
			IUriContext uriContext = element as IUriContext;
			if (uriContext != null)
			{
				uriContext.BaseUri = this.GetBaseUri();
			}
			else if (element is Application)
			{
				((Application)element).ApplicationMarkupBaseUri = this.GetBaseUri();
			}
			UIElement uielement = element as UIElement;
			if (uielement != null)
			{
				UIElement uielement2 = uielement;
				int persistId = this._persistId + 1;
				this._persistId = persistId;
				uielement2.SetPersistId(persistId);
			}
			if (this.CurrentContext == null)
			{
				IComponentConnector componentConnector = null;
				if (this._componentConnector == null)
				{
					componentConnector = (this._componentConnector = (element as IComponentConnector));
					if (this._componentConnector != null)
					{
						if (this.ParserContext.RootElement == null)
						{
							this.ParserContext.RootElement = element;
						}
						this._componentConnector.Connect(0, element);
					}
				}
				this._rootElement = element;
				DependencyObject dependencyObject = element as DependencyObject;
				if (!(element is INameScope) && this.ParserContext.NameScopeStack.Count == 0 && dependencyObject != null)
				{
					NameScope nameScope = null;
					if (componentConnector != null)
					{
						nameScope = (NameScope.GetNameScope(dependencyObject) as NameScope);
					}
					if (nameScope == null)
					{
						nameScope = new NameScope();
						NameScope.SetNameScope(dependencyObject, nameScope);
					}
				}
				if (dependencyObject != null)
				{
					Uri baseUri = this.GetBaseUri();
					this.SetDependencyValue(dependencyObject, BaseUriHelper.BaseUriProperty, baseUri);
				}
			}
			return result;
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x001F9D5C File Offset: 0x001F8D5C
		private void ElementEndInit(ref object element)
		{
			try
			{
				ISupportInitialize supportInitialize = element as ISupportInitialize;
				if (supportInitialize != null)
				{
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.EndInit, supportInitialize);
					}
					supportInitialize.EndInit();
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.EndInit, supportInitialize);
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				ReaderContextStackData parentContext = this.ParentContext;
				ReaderFlags readerFlags = (parentContext != null) ? parentContext.ContextType : ReaderFlags.Unknown;
				if (readerFlags == ReaderFlags.PropertyComplexClr || readerFlags == ReaderFlags.PropertyComplexDP || readerFlags == ReaderFlags.PropertyIList || readerFlags == ReaderFlags.PropertyIDictionary || readerFlags == ReaderFlags.PropertyArray || readerFlags == ReaderFlags.PropertyIAddChild)
				{
					IProvidePropertyFallback providePropertyFallback = this.GrandParentObjectData as IProvidePropertyFallback;
					if (providePropertyFallback != null)
					{
						string elementNameOrPropertyName = parentContext.ElementNameOrPropertyName;
						if (providePropertyFallback.CanProvidePropertyFallback(elementNameOrPropertyName))
						{
							element = providePropertyFallback.ProvidePropertyFallback(elementNameOrPropertyName, ex);
							this.CurrentContext.ObjectData = element;
							return;
						}
					}
				}
				this.ThrowExceptionWithLine(SR.Get("ParserFailedEndInit"), ex);
			}
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x001F9E68 File Offset: 0x001F8E68
		private void SetPropertyValueToParent(bool fromStartTag)
		{
			bool flag;
			this.SetPropertyValueToParent(fromStartTag, out flag);
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x001F9E80 File Offset: 0x001F8E80
		private void SetPropertyValueToParent(bool fromStartTag, out bool isMarkupExtension)
		{
			isMarkupExtension = false;
			object p = null;
			ReaderContextStackData currentContext = this.CurrentContext;
			ReaderContextStackData parentContext = this.ParentContext;
			if (currentContext == null || !currentContext.NeedToAddToTree || (ReaderFlags.DependencyObject != currentContext.ContextType && ReaderFlags.ClrObject != currentContext.ContextType))
			{
				return;
			}
			object obj = null;
			try
			{
				obj = this.GetCurrentObjectData();
				this.FreezeIfRequired(obj);
				if (parentContext == null)
				{
					if (this.RootList.Count == 0)
					{
						this.RootList.Add(obj);
					}
					currentContext.MarkAddedToTree();
				}
				else if (this.CheckExplicitCollectionTag(ref isMarkupExtension))
				{
					currentContext.MarkAddedToTree();
				}
				else
				{
					object parentObjectData = this.GetParentObjectData();
					IDictionary dictionaryFromContext = this.GetDictionaryFromContext(parentContext, true);
					if (dictionaryFromContext != null)
					{
						if (!fromStartTag)
						{
							obj = this.GetElementValue(obj, dictionaryFromContext, null, ref isMarkupExtension);
							if (currentContext.Key == null)
							{
								this.ThrowException("ParserNoDictionaryKey");
							}
							dictionaryFromContext.Add(currentContext.Key, obj);
							currentContext.MarkAddedToTree();
						}
					}
					else
					{
						IList listFromContext = this.GetListFromContext(parentContext);
						if (listFromContext != null)
						{
							obj = this.GetElementValue(obj, listFromContext, null, ref isMarkupExtension);
							listFromContext.Add(obj);
							currentContext.MarkAddedToTree();
						}
						else
						{
							ArrayExtension arrayExtensionFromContext = this.GetArrayExtensionFromContext(parentContext);
							if (arrayExtensionFromContext != null)
							{
								obj = this.GetElementValue(obj, arrayExtensionFromContext, null, ref isMarkupExtension);
								arrayExtensionFromContext.AddChild(obj);
								if (TraceMarkup.IsEnabled)
								{
									TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.AddValueToArray, p, parentContext.ElementNameOrPropertyName, obj);
								}
								currentContext.MarkAddedToTree();
							}
							else
							{
								IAddChild iaddChildFromContext = this.GetIAddChildFromContext(parentContext);
								if (iaddChildFromContext != null)
								{
									obj = this.GetElementValue(obj, iaddChildFromContext, null, ref isMarkupExtension);
									string text = obj as string;
									if (text != null)
									{
										iaddChildFromContext.AddText(text);
									}
									else
									{
										iaddChildFromContext.AddChild(obj);
									}
									if (TraceMarkup.IsEnabled)
									{
										TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.AddValueToAddChild, p, obj);
									}
									currentContext.MarkAddedToTree();
								}
								else
								{
									object contentProperty = parentContext.ContentProperty;
									if (contentProperty != null)
									{
										obj = this.GetElementValue(obj, parentContext.ObjectData, contentProperty, ref isMarkupExtension);
										this.AddToContentProperty(parentObjectData, contentProperty, obj);
										currentContext.MarkAddedToTree();
									}
									else if (parentContext.ContextType == ReaderFlags.PropertyComplexClr)
									{
										object objectDataFromContext = this.GetObjectDataFromContext(this.GrandParentContext);
										MemberInfo memberInfo = (MemberInfo)this.GetParentObjectData();
										this.SetClrComplexProperty(objectDataFromContext, memberInfo, obj);
										currentContext.MarkAddedToTree();
									}
									else if (parentContext.ContextType == ReaderFlags.PropertyComplexDP)
									{
										object objectDataFromContext2 = this.GetObjectDataFromContext(this.GrandParentContext);
										BamlAttributeInfoRecord attribInfo = (BamlAttributeInfoRecord)this.GetParentObjectData();
										this.SetDependencyComplexProperty(objectDataFromContext2, attribInfo, obj);
										currentContext.MarkAddedToTree();
									}
									else
									{
										Type parentType = this.GetParentType();
										string text2 = (parentType == null) ? string.Empty : parentType.FullName;
										if (obj == null)
										{
											this.ThrowException("ParserCannotAddAnyChildren", text2);
										}
										else
										{
											this.ThrowException("ParserCannotAddAnyChildren2", text2, obj.GetType().FullName);
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				Type parentType2 = this.GetParentType();
				string text3 = (parentType2 == null) ? string.Empty : parentType2.FullName;
				if (obj == null)
				{
					this.ThrowException("ParserCannotAddAnyChildren", text3);
				}
				else
				{
					this.ThrowException("ParserCannotAddAnyChildren2", text3, obj.GetType().FullName);
				}
			}
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x001FA1B4 File Offset: 0x001F91B4
		private Type GetParentType()
		{
			ReaderContextStackData parentContext = this.ParentContext;
			object obj = this.GetParentObjectData();
			if (parentContext.CheckFlag(ReaderFlags.CollectionHolder))
			{
				obj = ((BamlCollectionHolder)obj).Collection;
			}
			if (obj != null)
			{
				return obj.GetType();
			}
			if (parentContext.ExpectedType != null)
			{
				return parentContext.ExpectedType;
			}
			return null;
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x001FA204 File Offset: 0x001F9204
		private object GetElementValue(object element, object parent, object contentProperty, ref bool isMarkupExtension)
		{
			MarkupExtension markupExtension = element as MarkupExtension;
			if (markupExtension != null)
			{
				isMarkupExtension = true;
				element = this.ProvideValueFromMarkupExtension(markupExtension, parent, contentProperty);
				this.CurrentContext.ObjectData = element;
			}
			return element;
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x001FA238 File Offset: 0x001F9238
		private bool CheckExplicitCollectionTag(ref bool isMarkupExtension)
		{
			bool result = false;
			ReaderContextStackData parentContext = this.ParentContext;
			if (parentContext != null && parentContext.CheckFlag(ReaderFlags.CollectionHolder) && parentContext.ExpectedType != null)
			{
				BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)parentContext.ObjectData;
				if (!bamlCollectionHolder.IsClosed && !bamlCollectionHolder.ReadOnly)
				{
					ReaderContextStackData currentContext = this.CurrentContext;
					object obj = currentContext.ObjectData;
					Type c;
					if (currentContext.CheckFlag(ReaderFlags.ArrayExt))
					{
						c = ((ArrayExtension)obj).Type.MakeArrayType();
						isMarkupExtension = false;
					}
					else
					{
						obj = this.GetElementValue(obj, this.GrandParentObjectData, bamlCollectionHolder.PropertyDefinition.DependencyProperty, ref isMarkupExtension);
						c = ((obj == null) ? null : obj.GetType());
					}
					if (isMarkupExtension || parentContext.ExpectedType.IsAssignableFrom(c))
					{
						bamlCollectionHolder.Collection = obj;
						bamlCollectionHolder.IsClosed = true;
						parentContext.ExpectedType = null;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x001FA318 File Offset: 0x001F9318
		private void AddToContentProperty(object container, object contentProperty, object value)
		{
			IList list = contentProperty as IList;
			object p = null;
			try
			{
				if (list != null)
				{
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.AddValueToList, p, string.Empty, value);
					}
					list.Add(value);
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.AddValueToList, p, string.Empty, value);
					}
				}
				else
				{
					DependencyProperty dependencyProperty = contentProperty as DependencyProperty;
					if (dependencyProperty != null)
					{
						DependencyObject dependencyObject = container as DependencyObject;
						if (dependencyObject == null)
						{
							this.ThrowException("ParserParentDO", value.ToString());
						}
						if (TraceMarkup.IsEnabled)
						{
							TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, p, dependencyProperty.Name, value);
						}
						this.SetDependencyValue(dependencyObject, dependencyProperty, value);
						if (TraceMarkup.IsEnabled)
						{
							TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, p, dependencyProperty.Name, value);
						}
					}
					else
					{
						PropertyInfo propertyInfo = contentProperty as PropertyInfo;
						if (propertyInfo != null)
						{
							if (TraceMarkup.IsEnabled)
							{
								TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, p, propertyInfo.Name, value);
							}
							if (!XamlTypeMapper.SetInternalPropertyValue(this.ParserContext, this.ParserContext.RootElement, propertyInfo, container, value))
							{
								this.ThrowException("ParserCantSetContentProperty", propertyInfo.Name, propertyInfo.ReflectedType.Name);
							}
							if (TraceMarkup.IsEnabled)
							{
								TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, p, propertyInfo.Name, value);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotAddChild", new object[]
				{
					value.GetType().Name,
					container.GetType().Name
				}), ex);
			}
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetCPA, p, value);
			}
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x001FA4FC File Offset: 0x001F94FC
		internal string GetPropertyNameFromAttributeId(short id)
		{
			if (this.MapTable != null)
			{
				return this.MapTable.GetAttributeNameFromId(id);
			}
			return null;
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x001FA514 File Offset: 0x001F9514
		internal string GetPropertyValueFromStringId(short id)
		{
			string result = null;
			if (this.MapTable != null)
			{
				result = this.MapTable.GetStringFromStringId((int)id);
			}
			return result;
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x001FA53C File Offset: 0x001F953C
		private XamlSerializer CreateSerializer(BamlTypeInfoWithSerializerRecord typeWithSerializerInfo)
		{
			if (typeWithSerializerInfo.SerializerTypeId < 0)
			{
				return (XamlSerializer)this.MapTable.CreateKnownTypeFromId(typeWithSerializerInfo.SerializerTypeId);
			}
			if (typeWithSerializerInfo.SerializerType == null)
			{
				typeWithSerializerInfo.SerializerType = this.MapTable.GetTypeFromId(typeWithSerializerInfo.SerializerTypeId);
			}
			return (XamlSerializer)this.CreateInstanceFromType(typeWithSerializerInfo.SerializerType, typeWithSerializerInfo.SerializerTypeId, false);
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x001FA5A8 File Offset: 0x001F95A8
		internal object GetREOrEiFromAttributeId(short id, out bool isInternal, out bool isRE)
		{
			object obj = null;
			isRE = true;
			isInternal = false;
			BamlAttributeInfoRecord bamlAttributeInfoRecord = null;
			if (this.MapTable != null)
			{
				bamlAttributeInfoRecord = this.MapTable.GetAttributeInfoFromId(id);
				if (bamlAttributeInfoRecord != null)
				{
					obj = bamlAttributeInfoRecord.Event;
					if (obj == null)
					{
						obj = bamlAttributeInfoRecord.EventInfo;
						if (obj == null)
						{
							bamlAttributeInfoRecord.Event = this.MapTable.GetRoutedEvent(bamlAttributeInfoRecord);
							obj = bamlAttributeInfoRecord.Event;
							if (obj == null)
							{
								Type type = this.GetCurrentObjectData().GetType();
								if (ReflectionHelper.IsPublicType(type))
								{
									bamlAttributeInfoRecord.EventInfo = this.ParserContext.XamlTypeMapper.GetClrEventInfo(type, bamlAttributeInfoRecord.Name);
								}
								if (bamlAttributeInfoRecord.EventInfo == null)
								{
									bamlAttributeInfoRecord.EventInfo = type.GetEvent(bamlAttributeInfoRecord.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
									if (bamlAttributeInfoRecord.EventInfo != null)
									{
										bamlAttributeInfoRecord.IsInternal = true;
									}
								}
								obj = bamlAttributeInfoRecord.EventInfo;
								isRE = false;
							}
						}
						else
						{
							isRE = false;
						}
					}
				}
			}
			if (bamlAttributeInfoRecord != null)
			{
				isInternal = bamlAttributeInfoRecord.IsInternal;
			}
			return obj;
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x001FA698 File Offset: 0x001F9698
		private string GetPropNameFrom(object PiOrAttribInfo)
		{
			BamlAttributeInfoRecord bamlAttributeInfoRecord = PiOrAttribInfo as BamlAttributeInfoRecord;
			if (bamlAttributeInfoRecord != null)
			{
				return bamlAttributeInfoRecord.OwnerType.Name + "." + bamlAttributeInfoRecord.Name;
			}
			PropertyInfo propertyInfo = PiOrAttribInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.DeclaringType.Name + "." + propertyInfo.Name;
			}
			return string.Empty;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x001FA6FC File Offset: 0x001F96FC
		protected void ThrowException(string id)
		{
			this.ThrowExceptionWithLine(SR.Get(id), null);
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x001FA70B File Offset: 0x001F970B
		protected internal void ThrowException(string id, string parameter)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter
			}), null);
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x001FA724 File Offset: 0x001F9724
		protected void ThrowException(string id, string parameter1, string parameter2)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter1,
				parameter2
			}), null);
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x001FA741 File Offset: 0x001F9741
		protected void ThrowException(string id, string parameter1, string parameter2, string parameter3)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter1,
				parameter2,
				parameter3
			}), null);
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x001FA763 File Offset: 0x001F9763
		internal void ThrowExceptionWithLine(string message, Exception innerException)
		{
			XamlParseException.ThrowException(this.ParserContext, this.LineNumber, this.LinePosition, message, innerException);
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x001FA780 File Offset: 0x001F9780
		internal object CreateInstanceFromType(Type type, short typeId, bool throwOnFail)
		{
			bool flag = true;
			if (typeId >= 0)
			{
				BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(typeId);
				if (typeInfoFromId != null)
				{
					flag = !typeInfoFromId.IsInternalType;
				}
			}
			if (flag)
			{
				if (!ReflectionHelper.IsPublicType(type))
				{
					this.ThrowException("ParserNotMarkedPublic", type.Name);
				}
			}
			else if (!ReflectionHelper.IsInternalType(type))
			{
				this.ThrowException("ParserNotAllowedInternalType", type.Name);
			}
			object result;
			try
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseRdrCrInFTypBegin);
				object obj = null;
				try
				{
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.CreateObject, type);
					}
					if (type != typeof(string))
					{
						if (typeId < 0)
						{
							obj = this.MapTable.CreateKnownTypeFromId(typeId);
						}
						else if (flag)
						{
							obj = Activator.CreateInstance(type);
						}
						else
						{
							obj = XamlTypeMapper.CreateInternalInstance(this.ParserContext, type);
							if (obj == null && throwOnFail)
							{
								this.ThrowException("ParserNotAllowedInternalType", type.Name);
							}
						}
					}
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.CreateObject, type, obj);
					}
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseRdrCrInFTypEnd);
				}
				result = obj;
			}
			catch (MissingMethodException innerException)
			{
				if (throwOnFail)
				{
					if (this.ParentContext != null && this.ParentContext.ContextType == ReaderFlags.PropertyComplexDP)
					{
						BamlAttributeInfoRecord bamlAttributeInfoRecord = this.GetParentObjectData() as BamlAttributeInfoRecord;
						this.ThrowException("ParserNoDefaultPropConstructor", type.Name, bamlAttributeInfoRecord.DP.Name);
					}
					else
					{
						this.ThrowExceptionWithLine(SR.Get("ParserNoDefaultConstructor", new object[]
						{
							type.Name
						}), innerException);
					}
				}
				result = null;
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserErrorCreatingInstance", new object[]
				{
					type.Name,
					type.Assembly.FullName
				}), ex);
				result = null;
			}
			return result;
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x001FA978 File Offset: 0x001F9978
		internal void FreezeIfRequired(object element)
		{
			if (this._parserContext.FreezeFreezables)
			{
				Freezable freezable = element as Freezable;
				if (freezable != null)
				{
					freezable.Freeze();
				}
			}
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x001FA9A2 File Offset: 0x001F99A2
		internal void PreParsedBamlReset()
		{
			this.PreParsedCurrentRecord = this.PreParsedRecordsStart;
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x001FA9B0 File Offset: 0x001F99B0
		protected internal void SetPreviousBamlRecordReader(BamlRecordReader previousBamlRecordReader)
		{
			this._previousBamlRecordReader = previousBamlRecordReader;
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06003BC5 RID: 15301 RVA: 0x001FA9B9 File Offset: 0x001F99B9
		// (set) Token: 0x06003BC6 RID: 15302 RVA: 0x001FA9C1 File Offset: 0x001F99C1
		internal BamlRecord PreParsedRecordsStart
		{
			get
			{
				return this._preParsedBamlRecordsStart;
			}
			set
			{
				this._preParsedBamlRecordsStart = value;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x001FA9CA File Offset: 0x001F99CA
		// (set) Token: 0x06003BC8 RID: 15304 RVA: 0x001FA9D2 File Offset: 0x001F99D2
		internal BamlRecord PreParsedCurrentRecord
		{
			get
			{
				return this._preParsedIndexRecord;
			}
			set
			{
				this._preParsedIndexRecord = value;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06003BC9 RID: 15305 RVA: 0x001FA9DB File Offset: 0x001F99DB
		// (set) Token: 0x06003BCA RID: 15306 RVA: 0x001FA9E4 File Offset: 0x001F99E4
		internal Stream BamlStream
		{
			get
			{
				return this._bamlStream;
			}
			set
			{
				this._bamlStream = value;
				if (this._bamlStream is ReaderStream)
				{
					this._xamlReaderStream = (ReaderStream)this._bamlStream;
				}
				else
				{
					this._xamlReaderStream = null;
				}
				if (this.BamlStream != null)
				{
					this._binaryReader = new BamlBinaryReader(this.BamlStream, new UTF8Encoding());
				}
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06003BCB RID: 15307 RVA: 0x001FAA3D File Offset: 0x001F9A3D
		internal BamlBinaryReader BinaryReader
		{
			get
			{
				return this._binaryReader;
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06003BCC RID: 15308 RVA: 0x001FAA45 File Offset: 0x001F9A45
		internal XamlTypeMapper XamlTypeMapper
		{
			get
			{
				return this.ParserContext.XamlTypeMapper;
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06003BCD RID: 15309 RVA: 0x001FAA52 File Offset: 0x001F9A52
		// (set) Token: 0x06003BCE RID: 15310 RVA: 0x001FAA5A File Offset: 0x001F9A5A
		internal ParserContext ParserContext
		{
			get
			{
				return this._parserContext;
			}
			set
			{
				this._parserContext = value;
				this._typeConvertContext = null;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06003BCF RID: 15311 RVA: 0x001FAA6A File Offset: 0x001F9A6A
		internal TypeConvertContext TypeConvertContext
		{
			get
			{
				if (this._typeConvertContext == null)
				{
					this._typeConvertContext = new TypeConvertContext(this.ParserContext);
				}
				return this._typeConvertContext;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06003BD0 RID: 15312 RVA: 0x001FAA8B File Offset: 0x001F9A8B
		// (set) Token: 0x06003BD1 RID: 15313 RVA: 0x001FAA93 File Offset: 0x001F9A93
		internal XamlParseMode XamlParseMode
		{
			get
			{
				return this._parseMode;
			}
			set
			{
				this._parseMode = value;
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06003BD2 RID: 15314 RVA: 0x001FAA9C File Offset: 0x001F9A9C
		// (set) Token: 0x06003BD3 RID: 15315 RVA: 0x001FAAA4 File Offset: 0x001F9AA4
		internal int MaxAsyncRecords
		{
			get
			{
				return this._maxAsyncRecords;
			}
			set
			{
				this._maxAsyncRecords = value;
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06003BD4 RID: 15316 RVA: 0x001FAAAD File Offset: 0x001F9AAD
		internal BamlMapTable MapTable
		{
			get
			{
				return this.ParserContext.MapTable;
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06003BD5 RID: 15317 RVA: 0x001FAABA File Offset: 0x001F9ABA
		internal XmlnsDictionary XmlnsDictionary
		{
			get
			{
				return this.ParserContext.XmlnsDictionary;
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06003BD6 RID: 15318 RVA: 0x001FAAC7 File Offset: 0x001F9AC7
		internal ReaderContextStackData CurrentContext
		{
			get
			{
				return (ReaderContextStackData)this.ReaderContextStack.CurrentContext;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06003BD7 RID: 15319 RVA: 0x001FAAD9 File Offset: 0x001F9AD9
		internal ReaderContextStackData ParentContext
		{
			get
			{
				return (ReaderContextStackData)this.ReaderContextStack.ParentContext;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06003BD8 RID: 15320 RVA: 0x001FAAEC File Offset: 0x001F9AEC
		internal object ParentObjectData
		{
			get
			{
				ReaderContextStackData parentContext = this.ParentContext;
				if (parentContext != null)
				{
					return parentContext.ObjectData;
				}
				return null;
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06003BD9 RID: 15321 RVA: 0x001FAB0B File Offset: 0x001F9B0B
		internal ReaderContextStackData GrandParentContext
		{
			get
			{
				return (ReaderContextStackData)this.ReaderContextStack.GrandParentContext;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06003BDA RID: 15322 RVA: 0x001FAB20 File Offset: 0x001F9B20
		internal object GrandParentObjectData
		{
			get
			{
				ReaderContextStackData grandParentContext = this.GrandParentContext;
				if (grandParentContext != null)
				{
					return grandParentContext.ObjectData;
				}
				return null;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06003BDB RID: 15323 RVA: 0x001FAB3F File Offset: 0x001F9B3F
		internal ReaderContextStackData GreatGrandParentContext
		{
			get
			{
				return (ReaderContextStackData)this.ReaderContextStack.GreatGrandParentContext;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06003BDC RID: 15324 RVA: 0x001FAB51 File Offset: 0x001F9B51
		internal ParserStack ReaderContextStack
		{
			get
			{
				return this._contextStack;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06003BDD RID: 15325 RVA: 0x001FAB59 File Offset: 0x001F9B59
		internal BamlRecordManager BamlRecordManager
		{
			get
			{
				if (this._bamlRecordManager == null)
				{
					this._bamlRecordManager = new BamlRecordManager();
				}
				return this._bamlRecordManager;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06003BDE RID: 15326 RVA: 0x001FAB74 File Offset: 0x001F9B74
		// (set) Token: 0x06003BDF RID: 15327 RVA: 0x001FAB7C File Offset: 0x001F9B7C
		internal bool EndOfDocument
		{
			get
			{
				return this._endOfDocument;
			}
			set
			{
				this._endOfDocument = value;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x001FAB85 File Offset: 0x001F9B85
		// (set) Token: 0x06003BE1 RID: 15329 RVA: 0x001FAB8D File Offset: 0x001F9B8D
		internal object RootElement
		{
			get
			{
				return this._rootElement;
			}
			set
			{
				this._rootElement = value;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x001FAB96 File Offset: 0x001F9B96
		// (set) Token: 0x06003BE3 RID: 15331 RVA: 0x001FAB9E File Offset: 0x001F9B9E
		internal IComponentConnector ComponentConnector
		{
			get
			{
				return this._componentConnector;
			}
			set
			{
				this._componentConnector = value;
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06003BE4 RID: 15332 RVA: 0x001FABA7 File Offset: 0x001F9BA7
		private ReaderStream XamlReaderStream
		{
			get
			{
				return this._xamlReaderStream;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06003BE5 RID: 15333 RVA: 0x001FAB51 File Offset: 0x001F9B51
		// (set) Token: 0x06003BE6 RID: 15334 RVA: 0x001FABAF File Offset: 0x001F9BAF
		internal ParserStack ContextStack
		{
			get
			{
				return this._contextStack;
			}
			set
			{
				this._contextStack = value;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06003BE7 RID: 15335 RVA: 0x001FABB8 File Offset: 0x001F9BB8
		// (set) Token: 0x06003BE8 RID: 15336 RVA: 0x001FABC5 File Offset: 0x001F9BC5
		internal int LineNumber
		{
			get
			{
				return this.ParserContext.LineNumber;
			}
			set
			{
				this.ParserContext.LineNumber = value;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06003BE9 RID: 15337 RVA: 0x001FABD3 File Offset: 0x001F9BD3
		// (set) Token: 0x06003BEA RID: 15338 RVA: 0x001FABE0 File Offset: 0x001F9BE0
		internal int LinePosition
		{
			get
			{
				return this.ParserContext.LinePosition;
			}
			set
			{
				this.ParserContext.LinePosition = value;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06003BEB RID: 15339 RVA: 0x001FABEE File Offset: 0x001F9BEE
		// (set) Token: 0x06003BEC RID: 15340 RVA: 0x001FABFB File Offset: 0x001F9BFB
		internal bool IsDebugBamlStream
		{
			get
			{
				return this.ParserContext.IsDebugBamlStream;
			}
			set
			{
				this.ParserContext.IsDebugBamlStream = value;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06003BED RID: 15341 RVA: 0x001FAC09 File Offset: 0x001F9C09
		internal long StreamPosition
		{
			get
			{
				return this._bamlStream.Position;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06003BEE RID: 15342 RVA: 0x001FAC16 File Offset: 0x001F9C16
		private long StreamLength
		{
			get
			{
				return this._bamlStream.Length;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06003BEF RID: 15343 RVA: 0x001FAC23 File Offset: 0x001F9C23
		// (set) Token: 0x06003BF0 RID: 15344 RVA: 0x001FAC2B File Offset: 0x001F9C2B
		internal bool IsRootAlreadyLoaded
		{
			get
			{
				return this._isRootAlreadyLoaded;
			}
			set
			{
				this._isRootAlreadyLoaded = value;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x001FAC34 File Offset: 0x001F9C34
		internal BamlRecordReader PreviousBamlRecordReader
		{
			get
			{
				return this._previousBamlRecordReader;
			}
		}

		// Token: 0x04001E2F RID: 7727
		private static Type NullableType = typeof(Nullable<>);

		// Token: 0x04001E30 RID: 7728
		private IComponentConnector _componentConnector;

		// Token: 0x04001E31 RID: 7729
		private object _rootElement;

		// Token: 0x04001E32 RID: 7730
		private bool _bamlAsForest;

		// Token: 0x04001E33 RID: 7731
		private bool _isRootAlreadyLoaded;

		// Token: 0x04001E34 RID: 7732
		private ArrayList _rootList;

		// Token: 0x04001E35 RID: 7733
		private ParserContext _parserContext;

		// Token: 0x04001E36 RID: 7734
		private TypeConvertContext _typeConvertContext;

		// Token: 0x04001E37 RID: 7735
		private int _persistId;

		// Token: 0x04001E38 RID: 7736
		private ParserStack _contextStack;

		// Token: 0x04001E39 RID: 7737
		private XamlParseMode _parseMode;

		// Token: 0x04001E3A RID: 7738
		private int _maxAsyncRecords;

		// Token: 0x04001E3B RID: 7739
		private Stream _bamlStream;

		// Token: 0x04001E3C RID: 7740
		private ReaderStream _xamlReaderStream;

		// Token: 0x04001E3D RID: 7741
		private BamlBinaryReader _binaryReader;

		// Token: 0x04001E3E RID: 7742
		private BamlRecordManager _bamlRecordManager;

		// Token: 0x04001E3F RID: 7743
		private BamlRecord _preParsedBamlRecordsStart;

		// Token: 0x04001E40 RID: 7744
		private BamlRecord _preParsedIndexRecord;

		// Token: 0x04001E41 RID: 7745
		private bool _endOfDocument;

		// Token: 0x04001E42 RID: 7746
		private bool _buildTopDown;

		// Token: 0x04001E43 RID: 7747
		private BamlRecordReader _previousBamlRecordReader;

		// Token: 0x04001E44 RID: 7748
		private static List<ReaderContextStackData> _stackDataFactoryCache = new List<ReaderContextStackData>();
	}
}
