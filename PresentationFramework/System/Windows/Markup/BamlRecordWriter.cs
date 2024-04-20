using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x020004BC RID: 1212
	internal class BamlRecordWriter
	{
		// Token: 0x06003E13 RID: 15891 RVA: 0x001FDDF0 File Offset: 0x001FCDF0
		public BamlRecordWriter(Stream stream, ParserContext parserContext, bool deferLoadingSupport)
		{
			this._bamlStream = stream;
			this._xamlTypeMapper = parserContext.XamlTypeMapper;
			this._deferLoadingSupport = deferLoadingSupport;
			this._bamlMapTable = parserContext.MapTable;
			this._parserContext = parserContext;
			this._debugBamlStream = false;
			this._lineNumber = -1;
			this._linePosition = -1;
			this._bamlBinaryWriter = new BamlBinaryWriter(stream, new UTF8Encoding());
			this._bamlRecordManager = new BamlRecordManager();
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x001FDE64 File Offset: 0x001FCE64
		internal virtual void WriteBamlRecord(BamlRecord bamlRecord, int lineNumber, int linePosition)
		{
			try
			{
				bamlRecord.Write(this.BinaryWriter);
				if (this.DebugBamlStream && BamlRecordHelper.DoesRecordTypeHaveDebugExtension(bamlRecord.RecordType))
				{
					this.WriteDebugExtensionRecord(lineNumber, linePosition);
				}
			}
			catch (XamlParseException ex)
			{
				this._xamlTypeMapper.ThrowExceptionWithLine(ex.Message, ex.InnerException);
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06003E15 RID: 15893 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal virtual bool UpdateParentNodes
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x001FDEC8 File Offset: 0x001FCEC8
		internal void SetParseMode(XamlParseMode xamlParseMode)
		{
			if (this.UpdateParentNodes && xamlParseMode == XamlParseMode.Asynchronous && this.DocumentStartRecord != null)
			{
				this.DocumentStartRecord.LoadAsync = true;
				this.DocumentStartRecord.UpdateWrite(this.BinaryWriter);
			}
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x001FDEFB File Offset: 0x001FCEFB
		internal virtual void SetMaxAsyncRecords(int maxAsyncRecords)
		{
			if (this.UpdateParentNodes && this.DocumentStartRecord != null)
			{
				this.DocumentStartRecord.MaxAsyncRecords = maxAsyncRecords;
				this.DocumentStartRecord.UpdateWrite(this.BinaryWriter);
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06003E18 RID: 15896 RVA: 0x001FDF2A File Offset: 0x001FCF2A
		public bool DebugBamlStream
		{
			get
			{
				return this._debugBamlStream;
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06003E19 RID: 15897 RVA: 0x001FDF32 File Offset: 0x001FCF32
		internal BamlLineAndPositionRecord LineAndPositionRecord
		{
			get
			{
				if (this._bamlLineAndPositionRecord == null)
				{
					this._bamlLineAndPositionRecord = new BamlLineAndPositionRecord();
				}
				return this._bamlLineAndPositionRecord;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06003E1A RID: 15898 RVA: 0x001FDF4D File Offset: 0x001FCF4D
		internal BamlLinePositionRecord LinePositionRecord
		{
			get
			{
				if (this._bamlLinePositionRecord == null)
				{
					this._bamlLinePositionRecord = new BamlLinePositionRecord();
				}
				return this._bamlLinePositionRecord;
			}
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x001FDF68 File Offset: 0x001FCF68
		internal void WriteDebugExtensionRecord(int lineNumber, int linePosition)
		{
			if (lineNumber != this._lineNumber)
			{
				BamlLineAndPositionRecord lineAndPositionRecord = this.LineAndPositionRecord;
				this._lineNumber = lineNumber;
				lineAndPositionRecord.LineNumber = (uint)lineNumber;
				this._linePosition = linePosition;
				lineAndPositionRecord.LinePosition = (uint)linePosition;
				lineAndPositionRecord.Write(this.BinaryWriter);
				return;
			}
			if (linePosition != this._linePosition)
			{
				this._linePosition = linePosition;
				BamlLinePositionRecord linePositionRecord = this.LinePositionRecord;
				linePositionRecord.LinePosition = (uint)linePosition;
				linePositionRecord.Write(this.BinaryWriter);
			}
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x001FDFD4 File Offset: 0x001FCFD4
		internal void WriteDocumentStart(XamlDocumentStartNode xamlDocumentNode)
		{
			new BamlVersionHeader().WriteVersion(this.BinaryWriter);
			this.DocumentStartRecord = (BamlDocumentStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DocumentStart);
			this.DocumentStartRecord.DebugBaml = this.DebugBamlStream;
			this.WriteBamlRecord(this.DocumentStartRecord, xamlDocumentNode.LineNumber, xamlDocumentNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(this.DocumentStartRecord);
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x001FE044 File Offset: 0x001FD044
		internal void WriteDocumentEnd(XamlDocumentEndNode xamlDocumentEndNode)
		{
			BamlDocumentEndRecord bamlDocumentEndRecord = (BamlDocumentEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DocumentEnd);
			this.WriteBamlRecord(bamlDocumentEndRecord, xamlDocumentEndNode.LineNumber, xamlDocumentEndNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(bamlDocumentEndRecord);
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x001FE084 File Offset: 0x001FD084
		internal void WriteConnectionId(int connectionId)
		{
			BamlConnectionIdRecord bamlConnectionIdRecord = (BamlConnectionIdRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ConnectionId);
			bamlConnectionIdRecord.ConnectionId = connectionId;
			this.WriteAndReleaseRecord(bamlConnectionIdRecord, null);
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x001FE0B4 File Offset: 0x001FD0B4
		internal void WriteElementStart(XamlElementStartNode xamlElementNode)
		{
			BamlElementStartRecord bamlElementStartRecord = (BamlElementStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ElementStart);
			short typeId;
			if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, xamlElementNode.AssemblyName, xamlElementNode.TypeFullName, out typeId))
			{
				string serializerAssemblyFullName = string.Empty;
				if (xamlElementNode.SerializerType != null)
				{
					serializerAssemblyFullName = xamlElementNode.SerializerType.Assembly.FullName;
				}
				typeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, xamlElementNode.AssemblyName, xamlElementNode.TypeFullName, xamlElementNode.ElementType, serializerAssemblyFullName, xamlElementNode.SerializerTypeFullName);
			}
			bamlElementStartRecord.TypeId = typeId;
			bamlElementStartRecord.CreateUsingTypeConverter = xamlElementNode.CreateUsingTypeConverter;
			bamlElementStartRecord.IsInjected = xamlElementNode.IsInjected;
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				this._deferElementDepth++;
				if (this.InStaticResourceSection)
				{
					this._staticResourceElementDepth += 1;
					this._staticResourceRecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition));
					return;
				}
				if (this.CollectingValues && KnownTypes.Types[603] == xamlElementNode.ElementType)
				{
					this._staticResourceElementDepth = 1;
					this._staticResourceRecordList = new List<BamlRecordWriter.ValueDeferRecord>(5);
					this._staticResourceRecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition));
					return;
				}
				if (this.InDynamicResourceSection)
				{
					this._dynamicResourceElementDepth += 1;
				}
				else if (this.CollectingValues && KnownTypes.Types[189] == xamlElementNode.ElementType)
				{
					this._dynamicResourceElementDepth = 1;
				}
				BamlRecordWriter.ValueDeferRecord valueDeferRecord = new BamlRecordWriter.ValueDeferRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition);
				if (this._deferComplexPropertyDepth > 0)
				{
					this._deferElement.Add(valueDeferRecord);
					return;
				}
				if (this._deferElementDepth == 2)
				{
					this._deferKeys.Add(new BamlRecordWriter.KeyDeferRecord(xamlElementNode.LineNumber, xamlElementNode.LinePosition));
					valueDeferRecord.UpdateOffset = true;
					this._deferValues.Add(valueDeferRecord);
					return;
				}
				if (!this._deferKeyCollecting)
				{
					this._deferValues.Add(valueDeferRecord);
					return;
				}
				if (typeof(string).IsAssignableFrom(xamlElementNode.ElementType) || KnownTypes.Types[602].IsAssignableFrom(xamlElementNode.ElementType) || KnownTypes.Types[691].IsAssignableFrom(xamlElementNode.ElementType))
				{
					((BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1]).RecordList.Add(valueDeferRecord);
					return;
				}
				XamlParser.ThrowException("ParserBadKey", xamlElementNode.TypeFullName, xamlElementNode.LineNumber, xamlElementNode.LinePosition);
				return;
			}
			else
			{
				if (this._deferLoadingSupport && KnownTypes.Types[524].IsAssignableFrom(xamlElementNode.ElementType))
				{
					this._deferElementDepth = 1;
					this._deferEndOfStartReached = false;
					this._deferElement = new ArrayList(2);
					this._deferKeys = new ArrayList(10);
					this._deferValues = new ArrayList(100);
					this._deferElement.Add(new BamlRecordWriter.ValueDeferRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition));
					return;
				}
				this.WriteBamlRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition);
				this.BamlRecordManager.ReleaseWriteRecord(bamlElementStartRecord);
				return;
			}
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x001FE400 File Offset: 0x001FD400
		internal void WriteElementEnd(XamlElementEndNode xamlElementEndNode)
		{
			BamlElementEndRecord bamlRecord = (BamlElementEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ElementEnd);
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				int deferElementDepth = this._deferElementDepth;
				this._deferElementDepth = deferElementDepth - 1;
				if (deferElementDepth == 1)
				{
					this.WriteDeferableContent(xamlElementEndNode);
					this._deferKeys = null;
					this._deferValues = null;
					this._deferElement = null;
					return;
				}
			}
			this.WriteAndReleaseRecord(bamlRecord, xamlElementEndNode);
			if (this._deferLoadingSupport && this._staticResourceElementDepth > 0)
			{
				short num = this._staticResourceElementDepth;
				this._staticResourceElementDepth = num - 1;
				if (num == 1)
				{
					this.WriteStaticResource();
					this._staticResourceRecordList = null;
					return;
				}
			}
			if (this._deferLoadingSupport && this._dynamicResourceElementDepth > 0)
			{
				short num = this._dynamicResourceElementDepth;
				this._dynamicResourceElementDepth = num - 1;
			}
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x001FE4C3 File Offset: 0x001FD4C3
		internal void WriteEndAttributes(XamlEndAttributesNode xamlEndAttributesNode)
		{
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				this._deferEndOfStartReached = true;
			}
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x001FE4E0 File Offset: 0x001FD4E0
		internal void WriteLiteralContent(XamlLiteralContentNode xamlLiteralContentNode)
		{
			BamlLiteralContentRecord bamlLiteralContentRecord = (BamlLiteralContentRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.LiteralContent);
			bamlLiteralContentRecord.Value = xamlLiteralContentNode.Content;
			this.WriteAndReleaseRecord(bamlLiteralContentRecord, xamlLiteralContentNode);
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x001FE514 File Offset: 0x001FD514
		internal void WriteDefAttributeKeyType(XamlDefAttributeKeyTypeNode xamlDefNode)
		{
			short typeId;
			if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, xamlDefNode.AssemblyName, xamlDefNode.Value, out typeId))
			{
				typeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, xamlDefNode.AssemblyName, xamlDefNode.Value, xamlDefNode.ValueType, string.Empty, string.Empty);
			}
			BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = this.BamlRecordManager.GetWriteRecord(BamlRecordType.DefAttributeKeyType) as BamlDefAttributeKeyTypeRecord;
			bamlDefAttributeKeyTypeRecord.TypeId = typeId;
			((IBamlDictionaryKey)bamlDefAttributeKeyTypeRecord).KeyObject = xamlDefNode.ValueType;
			if (this._deferLoadingSupport && this._deferElementDepth == 2)
			{
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				this.TransferOldSharedData(keyDeferRecord.Record as IBamlDictionaryKey, bamlDefAttributeKeyTypeRecord);
				keyDeferRecord.Record = bamlDefAttributeKeyTypeRecord;
				keyDeferRecord.LineNumber = xamlDefNode.LineNumber;
				keyDeferRecord.LinePosition = xamlDefNode.LinePosition;
				return;
			}
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				this._deferValues.Add(new BamlRecordWriter.ValueDeferRecord(bamlDefAttributeKeyTypeRecord, xamlDefNode.LineNumber, xamlDefNode.LinePosition));
				return;
			}
			this.WriteBamlRecord(bamlDefAttributeKeyTypeRecord, xamlDefNode.LineNumber, xamlDefNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(bamlDefAttributeKeyTypeRecord);
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x001FE646 File Offset: 0x001FD646
		private void TransferOldSharedData(IBamlDictionaryKey oldRecord, IBamlDictionaryKey newRecord)
		{
			if (oldRecord != null && newRecord != null)
			{
				newRecord.Shared = oldRecord.Shared;
				newRecord.SharedSet = oldRecord.SharedSet;
			}
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x001FE668 File Offset: 0x001FD668
		private IBamlDictionaryKey FindBamlDictionaryKey(BamlRecordWriter.KeyDeferRecord record)
		{
			if (record != null)
			{
				if (record.RecordList != null)
				{
					for (int i = 0; i < record.RecordList.Count; i++)
					{
						IBamlDictionaryKey bamlDictionaryKey = ((BamlRecordWriter.ValueDeferRecord)record.RecordList[i]).Record as IBamlDictionaryKey;
						if (bamlDictionaryKey != null)
						{
							return bamlDictionaryKey;
						}
					}
				}
				return record.Record as IBamlDictionaryKey;
			}
			return null;
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x001FE6C4 File Offset: 0x001FD6C4
		internal void WriteDefAttribute(XamlDefAttributeNode xamlDefNode)
		{
			if (this._deferLoadingSupport && this._deferElementDepth == 2 && xamlDefNode.Name == "Key")
			{
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord = keyDeferRecord.Record as BamlDefAttributeKeyStringRecord;
				if (bamlDefAttributeKeyStringRecord == null)
				{
					bamlDefAttributeKeyStringRecord = (BamlDefAttributeKeyStringRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DefAttributeKeyString);
					this.TransferOldSharedData(keyDeferRecord.Record as IBamlDictionaryKey, bamlDefAttributeKeyStringRecord);
					keyDeferRecord.Record = bamlDefAttributeKeyStringRecord;
				}
				short valueId;
				if (!this.MapTable.GetStringInfoId(xamlDefNode.Value, out valueId))
				{
					valueId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlDefNode.Value);
				}
				bamlDefAttributeKeyStringRecord.Value = xamlDefNode.Value;
				bamlDefAttributeKeyStringRecord.ValueId = valueId;
				keyDeferRecord.LineNumber = xamlDefNode.LineNumber;
				keyDeferRecord.LinePosition = xamlDefNode.LinePosition;
				return;
			}
			if (this._deferLoadingSupport && this._deferElementDepth == 2 && xamlDefNode.Name == "Shared")
			{
				BamlRecordWriter.KeyDeferRecord keyDeferRecord2 = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				IBamlDictionaryKey bamlDictionaryKey = this.FindBamlDictionaryKey(keyDeferRecord2);
				if (bamlDictionaryKey == null)
				{
					BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord2 = (BamlDefAttributeKeyStringRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DefAttributeKeyString);
					keyDeferRecord2.Record = bamlDefAttributeKeyStringRecord2;
					bamlDictionaryKey = bamlDefAttributeKeyStringRecord2;
				}
				bamlDictionaryKey.Shared = bool.Parse(xamlDefNode.Value);
				bamlDictionaryKey.SharedSet = true;
				keyDeferRecord2.LineNumber = xamlDefNode.LineNumber;
				keyDeferRecord2.LinePosition = xamlDefNode.LinePosition;
				return;
			}
			short nameId;
			if (!this.MapTable.GetStringInfoId(xamlDefNode.Name, out nameId))
			{
				nameId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlDefNode.Name);
			}
			BamlDefAttributeRecord bamlDefAttributeRecord = (BamlDefAttributeRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DefAttribute);
			bamlDefAttributeRecord.Value = xamlDefNode.Value;
			bamlDefAttributeRecord.Name = xamlDefNode.Name;
			bamlDefAttributeRecord.AttributeUsage = xamlDefNode.AttributeUsage;
			bamlDefAttributeRecord.NameId = nameId;
			this.WriteAndReleaseRecord(bamlDefAttributeRecord, xamlDefNode);
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x001FE8D4 File Offset: 0x001FD8D4
		internal void WritePresentationOptionsAttribute(XamlPresentationOptionsAttributeNode xamlPresentationOptionsNode)
		{
			short nameId;
			if (!this.MapTable.GetStringInfoId(xamlPresentationOptionsNode.Name, out nameId))
			{
				nameId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlPresentationOptionsNode.Name);
			}
			BamlPresentationOptionsAttributeRecord bamlPresentationOptionsAttributeRecord = (BamlPresentationOptionsAttributeRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PresentationOptionsAttribute);
			bamlPresentationOptionsAttributeRecord.Value = xamlPresentationOptionsNode.Value;
			bamlPresentationOptionsAttributeRecord.Name = xamlPresentationOptionsNode.Name;
			bamlPresentationOptionsAttributeRecord.NameId = nameId;
			this.WriteAndReleaseRecord(bamlPresentationOptionsAttributeRecord, xamlPresentationOptionsNode);
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x001FE948 File Offset: 0x001FD948
		internal void WriteNamespacePrefix(XamlXmlnsPropertyNode xamlXmlnsPropertyNode)
		{
			BamlXmlnsPropertyRecord bamlXmlnsPropertyRecord = (BamlXmlnsPropertyRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.XmlnsProperty);
			bamlXmlnsPropertyRecord.Prefix = xamlXmlnsPropertyNode.Prefix;
			bamlXmlnsPropertyRecord.XmlNamespace = xamlXmlnsPropertyNode.XmlNamespace;
			this.WriteAndReleaseRecord(bamlXmlnsPropertyRecord, xamlXmlnsPropertyNode);
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x001FE988 File Offset: 0x001FD988
		internal void WritePIMapping(XamlPIMappingNode xamlPIMappingNode)
		{
			BamlPIMappingRecord bamlPIMappingRecord = (BamlPIMappingRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PIMapping);
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = this.MapTable.AddAssemblyMap(this.BinaryWriter, xamlPIMappingNode.AssemblyName);
			bamlPIMappingRecord.XmlNamespace = xamlPIMappingNode.XmlNamespace;
			bamlPIMappingRecord.ClrNamespace = xamlPIMappingNode.ClrNamespace;
			bamlPIMappingRecord.AssemblyId = bamlAssemblyInfoRecord.AssemblyId;
			this.WriteBamlRecord(bamlPIMappingRecord, xamlPIMappingNode.LineNumber, xamlPIMappingNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(bamlPIMappingRecord);
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x001FEA04 File Offset: 0x001FDA04
		internal void WritePropertyComplexStart(XamlPropertyComplexStartNode xamlComplexPropertyNode)
		{
			BamlPropertyComplexStartRecord bamlPropertyComplexStartRecord = (BamlPropertyComplexStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyComplexStart);
			bamlPropertyComplexStartRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlComplexPropertyNode.AssemblyName, xamlComplexPropertyNode.TypeFullName, xamlComplexPropertyNode.PropDeclaringType, xamlComplexPropertyNode.PropName, xamlComplexPropertyNode.PropValidType, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlPropertyComplexStartRecord, xamlComplexPropertyNode);
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x001FEA64 File Offset: 0x001FDA64
		internal void WritePropertyComplexEnd(XamlPropertyComplexEndNode xamlPropertyComplexEnd)
		{
			BamlPropertyComplexEndRecord bamlRecord = (BamlPropertyComplexEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyComplexEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlPropertyComplexEnd);
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x001FEA8C File Offset: 0x001FDA8C
		public void WriteKeyElementStart(XamlElementStartNode xamlKeyElementNode)
		{
			if (!typeof(string).IsAssignableFrom(xamlKeyElementNode.ElementType) && !KnownTypes.Types[602].IsAssignableFrom(xamlKeyElementNode.ElementType) && !KnownTypes.Types[691].IsAssignableFrom(xamlKeyElementNode.ElementType) && !KnownTypes.Types[525].IsAssignableFrom(xamlKeyElementNode.ElementType))
			{
				XamlParser.ThrowException("ParserBadKey", xamlKeyElementNode.TypeFullName, xamlKeyElementNode.LineNumber, xamlKeyElementNode.LinePosition);
			}
			BamlKeyElementStartRecord bamlKeyElementStartRecord = (BamlKeyElementStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.KeyElementStart);
			short typeId;
			if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, xamlKeyElementNode.AssemblyName, xamlKeyElementNode.TypeFullName, out typeId))
			{
				string serializerAssemblyFullName = string.Empty;
				if (xamlKeyElementNode.SerializerType != null)
				{
					serializerAssemblyFullName = xamlKeyElementNode.SerializerType.Assembly.FullName;
				}
				typeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, xamlKeyElementNode.AssemblyName, xamlKeyElementNode.TypeFullName, xamlKeyElementNode.ElementType, serializerAssemblyFullName, xamlKeyElementNode.SerializerTypeFullName);
			}
			bamlKeyElementStartRecord.TypeId = typeId;
			if (this._deferLoadingSupport && this._deferElementDepth == 2)
			{
				this._deferElementDepth++;
				this._deferKeyCollecting = true;
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				keyDeferRecord.RecordList = new ArrayList(5);
				keyDeferRecord.RecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlKeyElementStartRecord, xamlKeyElementNode.LineNumber, xamlKeyElementNode.LinePosition));
				if (keyDeferRecord.Record != null)
				{
					this.TransferOldSharedData(keyDeferRecord.Record as IBamlDictionaryKey, bamlKeyElementStartRecord);
					keyDeferRecord.Record = null;
				}
				keyDeferRecord.LineNumber = xamlKeyElementNode.LineNumber;
				keyDeferRecord.LinePosition = xamlKeyElementNode.LinePosition;
				return;
			}
			this.WriteAndReleaseRecord(bamlKeyElementStartRecord, xamlKeyElementNode);
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x001FEC64 File Offset: 0x001FDC64
		internal void WriteKeyElementEnd(XamlElementEndNode xamlKeyElementEnd)
		{
			BamlKeyElementEndRecord bamlRecord = (BamlKeyElementEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.KeyElementEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlKeyElementEnd);
			if (this._deferLoadingSupport && this._deferKeyCollecting)
			{
				this._deferKeyCollecting = false;
				this._deferElementDepth--;
			}
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x001FECB4 File Offset: 0x001FDCB4
		internal void WriteConstructorParametersStart(XamlConstructorParametersStartNode xamlConstructorParametersStartNode)
		{
			BamlConstructorParametersStartRecord bamlRecord = (BamlConstructorParametersStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ConstructorParametersStart);
			this.WriteAndReleaseRecord(bamlRecord, xamlConstructorParametersStartNode);
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x001FECDC File Offset: 0x001FDCDC
		internal void WriteConstructorParametersEnd(XamlConstructorParametersEndNode xamlConstructorParametersEndNode)
		{
			BamlConstructorParametersEndRecord bamlRecord = (BamlConstructorParametersEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ConstructorParametersEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlConstructorParametersEndNode);
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x001FED04 File Offset: 0x001FDD04
		internal virtual void WriteContentProperty(XamlContentPropertyNode xamlContentPropertyNode)
		{
			BamlContentPropertyRecord bamlContentPropertyRecord = (BamlContentPropertyRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ContentProperty);
			bamlContentPropertyRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlContentPropertyNode.AssemblyName, xamlContentPropertyNode.TypeFullName, xamlContentPropertyNode.PropDeclaringType, xamlContentPropertyNode.PropName, xamlContentPropertyNode.PropValidType, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlContentPropertyRecord, xamlContentPropertyNode);
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x001FED64 File Offset: 0x001FDD64
		internal virtual void WriteProperty(XamlPropertyNode xamlProperty)
		{
			short attributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlProperty.AssemblyName, xamlProperty.TypeFullName, xamlProperty.PropDeclaringType, xamlProperty.PropName, xamlProperty.PropValidType, xamlProperty.AttributeUsage);
			if (xamlProperty.AssemblyName != string.Empty && xamlProperty.TypeFullName != string.Empty)
			{
				short num;
				Type type;
				bool customSerializerOrConverter = this.MapTable.GetCustomSerializerOrConverter(this.BinaryWriter, xamlProperty.ValueDeclaringType, xamlProperty.ValuePropertyType, xamlProperty.ValuePropertyMember, xamlProperty.ValuePropertyName, out num, out type);
				if (type != null)
				{
					if (customSerializerOrConverter)
					{
						BamlPropertyCustomWriteInfoRecord bamlPropertyCustomWriteInfoRecord = (BamlPropertyCustomWriteInfoRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyCustom);
						bamlPropertyCustomWriteInfoRecord.AttributeId = attributeId;
						bamlPropertyCustomWriteInfoRecord.Value = xamlProperty.Value;
						bamlPropertyCustomWriteInfoRecord.ValueType = xamlProperty.ValuePropertyType;
						bamlPropertyCustomWriteInfoRecord.SerializerTypeId = num;
						bamlPropertyCustomWriteInfoRecord.SerializerType = type;
						bamlPropertyCustomWriteInfoRecord.TypeContext = this.TypeConvertContext;
						if (num == 137)
						{
							if (xamlProperty.HasValueId)
							{
								bamlPropertyCustomWriteInfoRecord.ValueId = xamlProperty.ValueId;
								bamlPropertyCustomWriteInfoRecord.ValueMemberName = xamlProperty.MemberName;
							}
							else
							{
								string text;
								Type dependencyPropertyOwnerAndName = this._xamlTypeMapper.GetDependencyPropertyOwnerAndName(xamlProperty.Value, this.ParserContext, xamlProperty.DefaultTargetType, out text);
								short valueId;
								short attributeOrTypeId = this.MapTable.GetAttributeOrTypeId(this.BinaryWriter, dependencyPropertyOwnerAndName, text, out valueId);
								if (attributeOrTypeId < 0)
								{
									bamlPropertyCustomWriteInfoRecord.ValueId = attributeOrTypeId;
									bamlPropertyCustomWriteInfoRecord.ValueMemberName = null;
								}
								else
								{
									bamlPropertyCustomWriteInfoRecord.ValueId = valueId;
									bamlPropertyCustomWriteInfoRecord.ValueMemberName = text;
								}
							}
						}
						this.WriteAndReleaseRecord(bamlPropertyCustomWriteInfoRecord, xamlProperty);
						return;
					}
					BamlPropertyWithConverterRecord bamlPropertyWithConverterRecord = (BamlPropertyWithConverterRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyWithConverter);
					bamlPropertyWithConverterRecord.AttributeId = attributeId;
					bamlPropertyWithConverterRecord.Value = xamlProperty.Value;
					bamlPropertyWithConverterRecord.ConverterTypeId = num;
					this.WriteAndReleaseRecord(bamlPropertyWithConverterRecord, xamlProperty);
					return;
				}
			}
			this.BaseWriteProperty(xamlProperty);
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x001FEF44 File Offset: 0x001FDF44
		internal virtual void WritePropertyWithExtension(XamlPropertyWithExtensionNode xamlPropertyNode)
		{
			short valueId = 0;
			short extensionTypeId = xamlPropertyNode.ExtensionTypeId;
			bool isValueTypeExtension = false;
			bool isValueStaticExtension = false;
			if (extensionTypeId == 189 || extensionTypeId == 603)
			{
				if (xamlPropertyNode.IsValueNestedExtension)
				{
					if (xamlPropertyNode.IsValueTypeExtension)
					{
						Type typeFromBaseString = this._xamlTypeMapper.GetTypeFromBaseString(xamlPropertyNode.Value, this.ParserContext, true);
						if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, typeFromBaseString.Assembly.FullName, typeFromBaseString.FullName, out valueId))
						{
							valueId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, typeFromBaseString.Assembly.FullName, typeFromBaseString.FullName, typeFromBaseString, string.Empty, string.Empty);
						}
						isValueTypeExtension = true;
					}
					else
					{
						valueId = this.MapTable.GetStaticMemberId(this.BinaryWriter, this.ParserContext, 602, xamlPropertyNode.Value, xamlPropertyNode.DefaultTargetType);
						isValueStaticExtension = true;
					}
				}
				else if (!this.MapTable.GetStringInfoId(xamlPropertyNode.Value, out valueId))
				{
					valueId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlPropertyNode.Value);
				}
			}
			else
			{
				valueId = this.MapTable.GetStaticMemberId(this.BinaryWriter, this.ParserContext, extensionTypeId, xamlPropertyNode.Value, xamlPropertyNode.DefaultTargetType);
			}
			short attributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyNode.AssemblyName, xamlPropertyNode.TypeFullName, xamlPropertyNode.PropDeclaringType, xamlPropertyNode.PropName, xamlPropertyNode.PropValidType, BamlAttributeUsage.Default);
			if (this._deferLoadingSupport && this._deferElementDepth > 0 && this.CollectingValues && extensionTypeId == 603)
			{
				BamlOptimizedStaticResourceRecord bamlOptimizedStaticResourceRecord = (BamlOptimizedStaticResourceRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.OptimizedStaticResource);
				bamlOptimizedStaticResourceRecord.IsValueTypeExtension = isValueTypeExtension;
				bamlOptimizedStaticResourceRecord.IsValueStaticExtension = isValueStaticExtension;
				bamlOptimizedStaticResourceRecord.ValueId = valueId;
				this._staticResourceRecordList = new List<BamlRecordWriter.ValueDeferRecord>(1);
				this._staticResourceRecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlOptimizedStaticResourceRecord, xamlPropertyNode.LineNumber, xamlPropertyNode.LinePosition));
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				keyDeferRecord.StaticResourceRecordList.Add(this._staticResourceRecordList);
				BamlPropertyWithStaticResourceIdRecord bamlPropertyWithStaticResourceIdRecord = (BamlPropertyWithStaticResourceIdRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyWithStaticResourceId);
				bamlPropertyWithStaticResourceIdRecord.AttributeId = attributeId;
				bamlPropertyWithStaticResourceIdRecord.StaticResourceId = (short)(keyDeferRecord.StaticResourceRecordList.Count - 1);
				this._deferValues.Add(new BamlRecordWriter.ValueDeferRecord(bamlPropertyWithStaticResourceIdRecord, xamlPropertyNode.LineNumber, xamlPropertyNode.LinePosition));
				this._staticResourceRecordList = null;
				return;
			}
			BamlPropertyWithExtensionRecord bamlPropertyWithExtensionRecord = (BamlPropertyWithExtensionRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyWithExtension);
			bamlPropertyWithExtensionRecord.AttributeId = attributeId;
			bamlPropertyWithExtensionRecord.ExtensionTypeId = extensionTypeId;
			bamlPropertyWithExtensionRecord.IsValueTypeExtension = isValueTypeExtension;
			bamlPropertyWithExtensionRecord.IsValueStaticExtension = isValueStaticExtension;
			bamlPropertyWithExtensionRecord.ValueId = valueId;
			this.WriteAndReleaseRecord(bamlPropertyWithExtensionRecord, xamlPropertyNode);
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x001FF208 File Offset: 0x001FE208
		internal virtual void WritePropertyWithType(XamlPropertyWithTypeNode xamlPropertyWithType)
		{
			short attributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyWithType.AssemblyName, xamlPropertyWithType.TypeFullName, xamlPropertyWithType.PropDeclaringType, xamlPropertyWithType.PropName, xamlPropertyWithType.PropValidType, BamlAttributeUsage.Default);
			short typeId;
			if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, xamlPropertyWithType.ValueTypeAssemblyName, xamlPropertyWithType.ValueTypeFullName, out typeId))
			{
				typeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, xamlPropertyWithType.ValueTypeAssemblyName, xamlPropertyWithType.ValueTypeFullName, xamlPropertyWithType.ValueElementType, xamlPropertyWithType.ValueSerializerTypeAssemblyName, xamlPropertyWithType.ValueSerializerTypeFullName);
			}
			BamlPropertyTypeReferenceRecord bamlPropertyTypeReferenceRecord = this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyTypeReference) as BamlPropertyTypeReferenceRecord;
			bamlPropertyTypeReferenceRecord.AttributeId = attributeId;
			bamlPropertyTypeReferenceRecord.TypeId = typeId;
			this.WriteAndReleaseRecord(bamlPropertyTypeReferenceRecord, xamlPropertyWithType);
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x001FF2C0 File Offset: 0x001FE2C0
		internal void BaseWriteProperty(XamlPropertyNode xamlProperty)
		{
			short attributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlProperty.AssemblyName, xamlProperty.TypeFullName, xamlProperty.PropDeclaringType, xamlProperty.PropName, xamlProperty.PropValidType, xamlProperty.AttributeUsage);
			BamlPropertyRecord bamlPropertyRecord = (BamlPropertyRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.Property);
			bamlPropertyRecord.AttributeId = attributeId;
			bamlPropertyRecord.Value = xamlProperty.Value;
			this.WriteAndReleaseRecord(bamlPropertyRecord, xamlProperty);
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void WriteClrEvent(XamlClrEventNode xamlClrEventNode)
		{
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x001FF330 File Offset: 0x001FE330
		internal void WritePropertyArrayStart(XamlPropertyArrayStartNode xamlPropertyArrayStartNode)
		{
			BamlPropertyArrayStartRecord bamlPropertyArrayStartRecord = (BamlPropertyArrayStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyArrayStart);
			bamlPropertyArrayStartRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyArrayStartNode.AssemblyName, xamlPropertyArrayStartNode.TypeFullName, xamlPropertyArrayStartNode.PropDeclaringType, xamlPropertyArrayStartNode.PropName, null, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlPropertyArrayStartRecord, xamlPropertyArrayStartNode);
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x001FF38C File Offset: 0x001FE38C
		internal virtual void WritePropertyArrayEnd(XamlPropertyArrayEndNode xamlPropertyArrayEndNode)
		{
			BamlPropertyArrayEndRecord bamlRecord = (BamlPropertyArrayEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyArrayEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlPropertyArrayEndNode);
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x001FF3B4 File Offset: 0x001FE3B4
		internal void WritePropertyIListStart(XamlPropertyIListStartNode xamlPropertyIListStart)
		{
			BamlPropertyIListStartRecord bamlPropertyIListStartRecord = (BamlPropertyIListStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyIListStart);
			bamlPropertyIListStartRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyIListStart.AssemblyName, xamlPropertyIListStart.TypeFullName, xamlPropertyIListStart.PropDeclaringType, xamlPropertyIListStart.PropName, null, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlPropertyIListStartRecord, xamlPropertyIListStart);
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x001FF410 File Offset: 0x001FE410
		internal virtual void WritePropertyIListEnd(XamlPropertyIListEndNode xamlPropertyIListEndNode)
		{
			BamlPropertyIListEndRecord bamlRecord = (BamlPropertyIListEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyIListEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlPropertyIListEndNode);
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x001FF438 File Offset: 0x001FE438
		internal void WritePropertyIDictionaryStart(XamlPropertyIDictionaryStartNode xamlPropertyIDictionaryStartNode)
		{
			BamlPropertyIDictionaryStartRecord bamlPropertyIDictionaryStartRecord = (BamlPropertyIDictionaryStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyIDictionaryStart);
			bamlPropertyIDictionaryStartRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyIDictionaryStartNode.AssemblyName, xamlPropertyIDictionaryStartNode.TypeFullName, xamlPropertyIDictionaryStartNode.PropDeclaringType, xamlPropertyIDictionaryStartNode.PropName, null, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlPropertyIDictionaryStartRecord, xamlPropertyIDictionaryStartNode);
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x001FF494 File Offset: 0x001FE494
		internal virtual void WritePropertyIDictionaryEnd(XamlPropertyIDictionaryEndNode xamlPropertyIDictionaryEndNode)
		{
			BamlPropertyIDictionaryEndRecord bamlRecord = (BamlPropertyIDictionaryEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyIDictionaryEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlPropertyIDictionaryEndNode);
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x001FF4BC File Offset: 0x001FE4BC
		internal void WriteRoutedEvent(XamlRoutedEventNode xamlRoutedEventNode)
		{
			BamlRoutedEventRecord bamlRoutedEventRecord = (BamlRoutedEventRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.RoutedEvent);
			BamlAttributeInfoRecord bamlAttributeInfoRecord;
			this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlRoutedEventNode.AssemblyName, xamlRoutedEventNode.TypeFullName, null, xamlRoutedEventNode.EventName, null, BamlAttributeUsage.Default, out bamlAttributeInfoRecord);
			bamlAttributeInfoRecord.Event = xamlRoutedEventNode.Event;
			bamlRoutedEventRecord.AttributeId = bamlAttributeInfoRecord.AttributeId;
			bamlRoutedEventRecord.Value = xamlRoutedEventNode.Value;
			this.WriteAndReleaseRecord(bamlRoutedEventRecord, xamlRoutedEventNode);
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x001FF534 File Offset: 0x001FE534
		internal void WriteText(XamlTextNode xamlTextNode)
		{
			BamlTextRecord bamlTextRecord;
			if (xamlTextNode.ConverterType == null)
			{
				if (!this.InStaticResourceSection && !this.InDynamicResourceSection)
				{
					bamlTextRecord = (BamlTextRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.Text);
				}
				else
				{
					bamlTextRecord = (BamlTextWithIdRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.TextWithId);
					short valueId;
					if (!this.MapTable.GetStringInfoId(xamlTextNode.Text, out valueId))
					{
						valueId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlTextNode.Text);
					}
					((BamlTextWithIdRecord)bamlTextRecord).ValueId = valueId;
				}
			}
			else
			{
				bamlTextRecord = (BamlTextWithConverterRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.TextWithConverter);
				string fullName = xamlTextNode.ConverterType.Assembly.FullName;
				string fullName2 = xamlTextNode.ConverterType.FullName;
				short converterTypeId;
				if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, fullName, fullName2, out converterTypeId))
				{
					converterTypeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, fullName, fullName2, xamlTextNode.ConverterType, string.Empty, string.Empty);
				}
				((BamlTextWithConverterRecord)bamlTextRecord).ConverterTypeId = converterTypeId;
			}
			bamlTextRecord.Value = xamlTextNode.Text;
			this.WriteAndReleaseRecord(bamlTextRecord, xamlTextNode);
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x001FF654 File Offset: 0x001FE654
		private void WriteAndReleaseRecord(BamlRecord bamlRecord, XamlNode xamlNode)
		{
			int lineNumber = (xamlNode != null) ? xamlNode.LineNumber : 0;
			int linePosition = (xamlNode != null) ? xamlNode.LinePosition : 0;
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				if (this.InStaticResourceSection)
				{
					this._staticResourceRecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlRecord, lineNumber, linePosition));
					return;
				}
				BamlRecordWriter.ValueDeferRecord value = new BamlRecordWriter.ValueDeferRecord(bamlRecord, lineNumber, linePosition);
				if (!this._deferEndOfStartReached)
				{
					this._deferElement.Add(value);
					return;
				}
				if (this._deferElementDepth == 1 && xamlNode is XamlPropertyComplexStartNode)
				{
					this._deferComplexPropertyDepth++;
				}
				if (this._deferComplexPropertyDepth > 0)
				{
					this._deferElement.Add(value);
					if (this._deferElementDepth == 1 && xamlNode is XamlPropertyComplexEndNode)
					{
						this._deferComplexPropertyDepth--;
						return;
					}
				}
				else
				{
					if (this._deferKeyCollecting)
					{
						((BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1]).RecordList.Add(value);
						return;
					}
					this._deferValues.Add(value);
					return;
				}
			}
			else
			{
				this.WriteBamlRecord(bamlRecord, lineNumber, linePosition);
				this.BamlRecordManager.ReleaseWriteRecord(bamlRecord);
			}
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x001FF77C File Offset: 0x001FE77C
		private void WriteDeferableContent(XamlElementEndNode xamlNode)
		{
			for (int i = 0; i < this._deferElement.Count; i++)
			{
				BamlRecordWriter.ValueDeferRecord valueDeferRecord = (BamlRecordWriter.ValueDeferRecord)this._deferElement[i];
				this.WriteBamlRecord(valueDeferRecord.Record, valueDeferRecord.LineNumber, valueDeferRecord.LinePosition);
			}
			BamlDeferableContentStartRecord bamlDeferableContentStartRecord = (BamlDeferableContentStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DeferableContentStart);
			this.WriteBamlRecord(bamlDeferableContentStartRecord, xamlNode.LineNumber, xamlNode.LinePosition);
			long num = this.BinaryWriter.Seek(0, SeekOrigin.Current);
			for (int j = 0; j < this._deferKeys.Count; j++)
			{
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[j];
				if (keyDeferRecord.RecordList != null && keyDeferRecord.RecordList.Count > 0)
				{
					for (int k = 0; k < keyDeferRecord.RecordList.Count; k++)
					{
						BamlRecordWriter.ValueDeferRecord valueDeferRecord2 = (BamlRecordWriter.ValueDeferRecord)keyDeferRecord.RecordList[k];
						this.WriteBamlRecord(valueDeferRecord2.Record, valueDeferRecord2.LineNumber, valueDeferRecord2.LinePosition);
					}
				}
				else if (keyDeferRecord.Record == null)
				{
					XamlParser.ThrowException("ParserNoDictionaryKey", keyDeferRecord.LineNumber, keyDeferRecord.LinePosition);
				}
				else
				{
					this.WriteBamlRecord(keyDeferRecord.Record, keyDeferRecord.LineNumber, keyDeferRecord.LinePosition);
				}
				List<List<BamlRecordWriter.ValueDeferRecord>> staticResourceRecordList = keyDeferRecord.StaticResourceRecordList;
				if (staticResourceRecordList.Count > 0)
				{
					for (int l = 0; l < staticResourceRecordList.Count; l++)
					{
						List<BamlRecordWriter.ValueDeferRecord> list = staticResourceRecordList[l];
						for (int m = 0; m < list.Count; m++)
						{
							BamlRecordWriter.ValueDeferRecord valueDeferRecord3 = list[m];
							this.WriteBamlRecord(valueDeferRecord3.Record, valueDeferRecord3.LineNumber, valueDeferRecord3.LinePosition);
						}
					}
				}
			}
			long num2 = this.BinaryWriter.Seek(0, SeekOrigin.Current);
			int num3 = 0;
			for (int n = 0; n < this._deferValues.Count; n++)
			{
				BamlRecordWriter.ValueDeferRecord valueDeferRecord4 = (BamlRecordWriter.ValueDeferRecord)this._deferValues[n];
				if (valueDeferRecord4.UpdateOffset)
				{
					BamlRecordWriter.KeyDeferRecord keyDeferRecord2 = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[num3++];
					long num4 = this.BinaryWriter.Seek(0, SeekOrigin.Current);
					IBamlDictionaryKey bamlDictionaryKey;
					if (keyDeferRecord2.RecordList != null && keyDeferRecord2.RecordList.Count > 0)
					{
						bamlDictionaryKey = (IBamlDictionaryKey)((BamlRecordWriter.ValueDeferRecord)keyDeferRecord2.RecordList[0]).Record;
					}
					else
					{
						bamlDictionaryKey = (IBamlDictionaryKey)keyDeferRecord2.Record;
					}
					if (bamlDictionaryKey != null)
					{
						bamlDictionaryKey.UpdateValuePosition((int)(num4 - num2), this.BinaryWriter);
					}
				}
				this.WriteBamlRecord(valueDeferRecord4.Record, valueDeferRecord4.LineNumber, valueDeferRecord4.LinePosition);
			}
			long num5 = this.BinaryWriter.Seek(0, SeekOrigin.Current);
			bamlDeferableContentStartRecord.UpdateContentSize((int)(num5 - num), this.BinaryWriter);
			BamlElementEndRecord bamlElementEndRecord = (BamlElementEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ElementEnd);
			this.WriteBamlRecord(bamlElementEndRecord, xamlNode.LineNumber, xamlNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(bamlElementEndRecord);
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x001FFA90 File Offset: 0x001FEA90
		private void WriteStaticResource()
		{
			BamlRecordWriter.ValueDeferRecord valueDeferRecord = this._staticResourceRecordList[0];
			int lineNumber = valueDeferRecord.LineNumber;
			int linePosition = valueDeferRecord.LinePosition;
			BamlStaticResourceStartRecord bamlStaticResourceStartRecord = (BamlStaticResourceStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.StaticResourceStart);
			bamlStaticResourceStartRecord.TypeId = ((BamlElementStartRecord)valueDeferRecord.Record).TypeId;
			valueDeferRecord.Record = bamlStaticResourceStartRecord;
			valueDeferRecord = this._staticResourceRecordList[this._staticResourceRecordList.Count - 1];
			BamlStaticResourceEndRecord record = (BamlStaticResourceEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.StaticResourceEnd);
			valueDeferRecord.Record = record;
			BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
			keyDeferRecord.StaticResourceRecordList.Add(this._staticResourceRecordList);
			BamlStaticResourceIdRecord bamlStaticResourceIdRecord = (BamlStaticResourceIdRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.StaticResourceId);
			bamlStaticResourceIdRecord.StaticResourceId = (short)(keyDeferRecord.StaticResourceRecordList.Count - 1);
			this._deferValues.Add(new BamlRecordWriter.ValueDeferRecord(bamlStaticResourceIdRecord, lineNumber, linePosition));
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06003E41 RID: 15937 RVA: 0x001FFB8D File Offset: 0x001FEB8D
		public Stream BamlStream
		{
			get
			{
				return this._bamlStream;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06003E42 RID: 15938 RVA: 0x001FFB95 File Offset: 0x001FEB95
		internal BamlBinaryWriter BinaryWriter
		{
			get
			{
				return this._bamlBinaryWriter;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06003E43 RID: 15939 RVA: 0x001FFB9D File Offset: 0x001FEB9D
		internal BamlMapTable MapTable
		{
			get
			{
				return this._bamlMapTable;
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06003E44 RID: 15940 RVA: 0x001FFBA5 File Offset: 0x001FEBA5
		internal ParserContext ParserContext
		{
			get
			{
				return this._parserContext;
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06003E45 RID: 15941 RVA: 0x001FFBAD File Offset: 0x001FEBAD
		internal virtual BamlRecordManager BamlRecordManager
		{
			get
			{
				return this._bamlRecordManager;
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06003E46 RID: 15942 RVA: 0x001FFBB5 File Offset: 0x001FEBB5
		// (set) Token: 0x06003E47 RID: 15943 RVA: 0x001FFBBD File Offset: 0x001FEBBD
		private BamlDocumentStartRecord DocumentStartRecord
		{
			get
			{
				return this._startDocumentRecord;
			}
			set
			{
				this._startDocumentRecord = value;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06003E48 RID: 15944 RVA: 0x001FFBC6 File Offset: 0x001FEBC6
		private bool CollectingValues
		{
			get
			{
				return this._deferEndOfStartReached && !this._deferKeyCollecting && this._deferComplexPropertyDepth <= 0;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06003E49 RID: 15945 RVA: 0x001FFBE6 File Offset: 0x001FEBE6
		private ITypeDescriptorContext TypeConvertContext
		{
			get
			{
				if (this._typeConvertContext == null)
				{
					this._typeConvertContext = new TypeConvertContext(this._parserContext);
				}
				return this._typeConvertContext;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x001FFC07 File Offset: 0x001FEC07
		private bool InStaticResourceSection
		{
			get
			{
				return this._staticResourceElementDepth > 0;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06003E4B RID: 15947 RVA: 0x001FFC12 File Offset: 0x001FEC12
		private bool InDynamicResourceSection
		{
			get
			{
				return this._dynamicResourceElementDepth > 0;
			}
		}

		// Token: 0x04001F02 RID: 7938
		private XamlTypeMapper _xamlTypeMapper;

		// Token: 0x04001F03 RID: 7939
		private Stream _bamlStream;

		// Token: 0x04001F04 RID: 7940
		private BamlBinaryWriter _bamlBinaryWriter;

		// Token: 0x04001F05 RID: 7941
		private BamlDocumentStartRecord _startDocumentRecord;

		// Token: 0x04001F06 RID: 7942
		private ParserContext _parserContext;

		// Token: 0x04001F07 RID: 7943
		private BamlMapTable _bamlMapTable;

		// Token: 0x04001F08 RID: 7944
		private BamlRecordManager _bamlRecordManager;

		// Token: 0x04001F09 RID: 7945
		private ITypeDescriptorContext _typeConvertContext;

		// Token: 0x04001F0A RID: 7946
		private bool _deferLoadingSupport;

		// Token: 0x04001F0B RID: 7947
		private int _deferElementDepth;

		// Token: 0x04001F0C RID: 7948
		private bool _deferEndOfStartReached;

		// Token: 0x04001F0D RID: 7949
		private int _deferComplexPropertyDepth;

		// Token: 0x04001F0E RID: 7950
		private bool _deferKeyCollecting;

		// Token: 0x04001F0F RID: 7951
		private ArrayList _deferKeys;

		// Token: 0x04001F10 RID: 7952
		private ArrayList _deferValues;

		// Token: 0x04001F11 RID: 7953
		private ArrayList _deferElement;

		// Token: 0x04001F12 RID: 7954
		private short _staticResourceElementDepth;

		// Token: 0x04001F13 RID: 7955
		private short _dynamicResourceElementDepth;

		// Token: 0x04001F14 RID: 7956
		private List<BamlRecordWriter.ValueDeferRecord> _staticResourceRecordList;

		// Token: 0x04001F15 RID: 7957
		private bool _debugBamlStream;

		// Token: 0x04001F16 RID: 7958
		private int _lineNumber;

		// Token: 0x04001F17 RID: 7959
		private int _linePosition;

		// Token: 0x04001F18 RID: 7960
		private BamlLineAndPositionRecord _bamlLineAndPositionRecord;

		// Token: 0x04001F19 RID: 7961
		private BamlLinePositionRecord _bamlLinePositionRecord;

		// Token: 0x02000AF2 RID: 2802
		private class DeferRecord
		{
			// Token: 0x06008B77 RID: 35703 RVA: 0x00339E5E File Offset: 0x00338E5E
			internal DeferRecord(int lineNumber, int linePosition)
			{
				this._lineNumber = lineNumber;
				this._linePosition = linePosition;
			}

			// Token: 0x17001E8E RID: 7822
			// (get) Token: 0x06008B78 RID: 35704 RVA: 0x00339E74 File Offset: 0x00338E74
			// (set) Token: 0x06008B79 RID: 35705 RVA: 0x00339E7C File Offset: 0x00338E7C
			internal int LineNumber
			{
				get
				{
					return this._lineNumber;
				}
				set
				{
					this._lineNumber = value;
				}
			}

			// Token: 0x17001E8F RID: 7823
			// (get) Token: 0x06008B7A RID: 35706 RVA: 0x00339E85 File Offset: 0x00338E85
			// (set) Token: 0x06008B7B RID: 35707 RVA: 0x00339E8D File Offset: 0x00338E8D
			internal int LinePosition
			{
				get
				{
					return this._linePosition;
				}
				set
				{
					this._linePosition = value;
				}
			}

			// Token: 0x0400473D RID: 18237
			private int _lineNumber;

			// Token: 0x0400473E RID: 18238
			private int _linePosition;
		}

		// Token: 0x02000AF3 RID: 2803
		private class ValueDeferRecord : BamlRecordWriter.DeferRecord
		{
			// Token: 0x06008B7C RID: 35708 RVA: 0x00339E96 File Offset: 0x00338E96
			internal ValueDeferRecord(BamlRecord record, int lineNumber, int linePosition) : base(lineNumber, linePosition)
			{
				this._record = record;
				this._updateOffset = false;
			}

			// Token: 0x17001E90 RID: 7824
			// (get) Token: 0x06008B7D RID: 35709 RVA: 0x00339EAE File Offset: 0x00338EAE
			// (set) Token: 0x06008B7E RID: 35710 RVA: 0x00339EB6 File Offset: 0x00338EB6
			internal BamlRecord Record
			{
				get
				{
					return this._record;
				}
				set
				{
					this._record = value;
				}
			}

			// Token: 0x17001E91 RID: 7825
			// (get) Token: 0x06008B7F RID: 35711 RVA: 0x00339EBF File Offset: 0x00338EBF
			// (set) Token: 0x06008B80 RID: 35712 RVA: 0x00339EC7 File Offset: 0x00338EC7
			internal bool UpdateOffset
			{
				get
				{
					return this._updateOffset;
				}
				set
				{
					this._updateOffset = value;
				}
			}

			// Token: 0x0400473F RID: 18239
			private bool _updateOffset;

			// Token: 0x04004740 RID: 18240
			private BamlRecord _record;
		}

		// Token: 0x02000AF4 RID: 2804
		private class KeyDeferRecord : BamlRecordWriter.DeferRecord
		{
			// Token: 0x06008B81 RID: 35713 RVA: 0x00339ED0 File Offset: 0x00338ED0
			internal KeyDeferRecord(int lineNumber, int linePosition) : base(lineNumber, linePosition)
			{
			}

			// Token: 0x17001E92 RID: 7826
			// (get) Token: 0x06008B82 RID: 35714 RVA: 0x00339EDA File Offset: 0x00338EDA
			// (set) Token: 0x06008B83 RID: 35715 RVA: 0x00339EE2 File Offset: 0x00338EE2
			internal BamlRecord Record
			{
				get
				{
					return this._record;
				}
				set
				{
					this._record = value;
				}
			}

			// Token: 0x17001E93 RID: 7827
			// (get) Token: 0x06008B84 RID: 35716 RVA: 0x00339EEB File Offset: 0x00338EEB
			// (set) Token: 0x06008B85 RID: 35717 RVA: 0x00339EF3 File Offset: 0x00338EF3
			internal ArrayList RecordList
			{
				get
				{
					return this._recordList;
				}
				set
				{
					this._recordList = value;
				}
			}

			// Token: 0x17001E94 RID: 7828
			// (get) Token: 0x06008B86 RID: 35718 RVA: 0x00339EFC File Offset: 0x00338EFC
			internal List<List<BamlRecordWriter.ValueDeferRecord>> StaticResourceRecordList
			{
				get
				{
					if (this._staticResourceRecordList == null)
					{
						this._staticResourceRecordList = new List<List<BamlRecordWriter.ValueDeferRecord>>(1);
					}
					return this._staticResourceRecordList;
				}
			}

			// Token: 0x04004741 RID: 18241
			private BamlRecord _record;

			// Token: 0x04004742 RID: 18242
			private ArrayList _recordList;

			// Token: 0x04004743 RID: 18243
			private List<List<BamlRecordWriter.ValueDeferRecord>> _staticResourceRecordList;
		}
	}
}
