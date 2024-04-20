using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Diagnostics;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xaml;
using MS.Internal.WindowsBase;

namespace System.Windows.Baml2006
{
	// Token: 0x02000406 RID: 1030
	public class Baml2006Reader : System.Xaml.XamlReader, IXamlLineInfo, IFreezeFreezables
	{
		// Token: 0x06002C8B RID: 11403 RVA: 0x001A7578 File Offset: 0x001A6578
		public Baml2006Reader(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			Baml2006SchemaContext schemaContext = new Baml2006SchemaContext(null);
			Baml2006ReaderSettings baml2006ReaderSettings = System.Windows.Markup.XamlReader.CreateBamlReaderSettings();
			baml2006ReaderSettings.OwnsStream = true;
			this.Initialize(stream, schemaContext, baml2006ReaderSettings);
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x001A75E4 File Offset: 0x001A65E4
		public Baml2006Reader(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			Baml2006SchemaContext schemaContext = new Baml2006SchemaContext(null);
			Baml2006ReaderSettings settings = new Baml2006ReaderSettings();
			this.Initialize(stream, schemaContext, settings);
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x001A763C File Offset: 0x001A663C
		public Baml2006Reader(Stream stream, XamlReaderSettings xamlReaderSettings)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (xamlReaderSettings == null)
			{
				throw new ArgumentNullException("xamlReaderSettings");
			}
			Baml2006SchemaContext schemaContext;
			if (xamlReaderSettings.ValuesMustBeString)
			{
				schemaContext = new Baml2006SchemaContext(xamlReaderSettings.LocalAssembly, System.Windows.Markup.XamlReader.XamlV3SharedSchemaContext);
			}
			else
			{
				schemaContext = new Baml2006SchemaContext(xamlReaderSettings.LocalAssembly);
			}
			Baml2006ReaderSettings settings = new Baml2006ReaderSettings(xamlReaderSettings);
			this.Initialize(stream, schemaContext, settings);
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x001A76C4 File Offset: 0x001A66C4
		internal Baml2006Reader(Stream stream, Baml2006SchemaContext schemaContext, Baml2006ReaderSettings settings)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (schemaContext == null)
			{
				throw new ArgumentNullException("schemaContext");
			}
			this.Initialize(stream, schemaContext, settings ?? new Baml2006ReaderSettings());
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x001A7726 File Offset: 0x001A6726
		internal Baml2006Reader(Stream stream, Baml2006SchemaContext baml2006SchemaContext, Baml2006ReaderSettings baml2006ReaderSettings, object root) : this(stream, baml2006SchemaContext, baml2006ReaderSettings)
		{
			this._root = root;
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x001A773C File Offset: 0x001A673C
		private void Initialize(Stream stream, Baml2006SchemaContext schemaContext, Baml2006ReaderSettings settings)
		{
			schemaContext.Settings = settings;
			this._settings = settings;
			this._context = new Baml2006ReaderContext(schemaContext);
			this._xamlMainNodeQueue = new XamlNodeQueue(schemaContext);
			this._xamlNodesReader = this._xamlMainNodeQueue.Reader;
			this._xamlNodesWriter = this._xamlMainNodeQueue.Writer;
			this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth = -1;
			this._isBinaryProvider = !settings.ValuesMustBeString;
			if (this._settings.OwnsStream)
			{
				stream = new SharedStream(stream);
			}
			this._binaryReader = new BamlBinaryReader(stream);
			this._context.TemplateStartDepth = -1;
			if (!this._settings.IsBamlFragment)
			{
				this.Process_Header();
			}
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x001A77E8 File Offset: 0x001A67E8
		public override bool Read()
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException("Baml2006Reader");
			}
			if (this.IsEof)
			{
				return false;
			}
			while (!this._xamlNodesReader.Read())
			{
				if (!this.Process_BamlRecords())
				{
					this._isEof = true;
					return false;
				}
			}
			if (this._binaryReader.BaseStream.Length == this._binaryReader.BaseStream.Position && this._xamlNodesReader.NodeType != System.Xaml.XamlNodeType.EndObject)
			{
				this._isEof = true;
				return false;
			}
			return true;
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x001A786A File Offset: 0x001A686A
		public override System.Xaml.XamlNodeType NodeType
		{
			get
			{
				return this._xamlNodesReader.NodeType;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06002C93 RID: 11411 RVA: 0x001A7877 File Offset: 0x001A6877
		public override bool IsEof
		{
			get
			{
				return this._isEof;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x001A787F File Offset: 0x001A687F
		public override NamespaceDeclaration Namespace
		{
			get
			{
				return this._xamlNodesReader.Namespace;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06002C95 RID: 11413 RVA: 0x001A788C File Offset: 0x001A688C
		public override XamlSchemaContext SchemaContext
		{
			get
			{
				return this._xamlNodesReader.SchemaContext;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x001A7899 File Offset: 0x001A6899
		public override XamlType Type
		{
			get
			{
				return this._xamlNodesReader.Type;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06002C97 RID: 11415 RVA: 0x001A78A6 File Offset: 0x001A68A6
		public override object Value
		{
			get
			{
				return this._xamlNodesReader.Value;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06002C98 RID: 11416 RVA: 0x001A78B3 File Offset: 0x001A68B3
		public override XamlMember Member
		{
			get
			{
				return this._xamlNodesReader.Member;
			}
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x001A78C0 File Offset: 0x001A68C0
		protected override void Dispose(bool disposing)
		{
			if (this._binaryReader != null)
			{
				if (this._settings.OwnsStream)
				{
					SharedStream sharedStream = this._binaryReader.BaseStream as SharedStream;
					if (sharedStream != null && sharedStream.SharedCount < 1)
					{
						this._binaryReader.Close();
					}
				}
				this._binaryReader = null;
				this._context = null;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06002C9A RID: 11418 RVA: 0x001A7918 File Offset: 0x001A6918
		bool IXamlLineInfo.HasLineInfo
		{
			get
			{
				return this._context.CurrentFrame != null;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06002C9B RID: 11419 RVA: 0x001A7928 File Offset: 0x001A6928
		int IXamlLineInfo.LineNumber
		{
			get
			{
				return this._context.LineNumber;
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06002C9C RID: 11420 RVA: 0x001A7935 File Offset: 0x001A6935
		int IXamlLineInfo.LinePosition
		{
			get
			{
				return this._context.LineOffset;
			}
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x001A7944 File Offset: 0x001A6944
		internal List<KeyRecord> ReadKeys()
		{
			this._context.KeyList = new List<KeyRecord>();
			this._context.CurrentFrame.IsDeferredContent = true;
			bool flag = true;
			while (flag)
			{
				Baml2006RecordType baml2006RecordType = this.Read_RecordType();
				switch (baml2006RecordType)
				{
				case Baml2006RecordType.DefAttributeKeyString:
					this.Process_DefAttributeKeyString();
					break;
				case Baml2006RecordType.DefAttributeKeyType:
					this.Process_DefAttributeKeyType();
					break;
				case Baml2006RecordType.KeyElementStart:
					this.Process_KeyElementStart();
					for (;;)
					{
						baml2006RecordType = this.Read_RecordType();
						if (baml2006RecordType == Baml2006RecordType.KeyElementEnd)
						{
							break;
						}
						this._binaryReader.BaseStream.Seek(-1L, SeekOrigin.Current);
						this.Process_OneBamlRecord();
					}
					this.Process_KeyElementEnd();
					break;
				default:
					if (baml2006RecordType != Baml2006RecordType.StaticResourceStart)
					{
						if (baml2006RecordType != Baml2006RecordType.OptimizedStaticResource)
						{
							flag = false;
							this._binaryReader.BaseStream.Seek(-1L, SeekOrigin.Current);
						}
						else
						{
							this.Process_OptimizedStaticResource();
						}
					}
					else
					{
						this.Process_StaticResourceStart();
						for (;;)
						{
							baml2006RecordType = this.Read_RecordType();
							if (baml2006RecordType == Baml2006RecordType.StaticResourceEnd)
							{
								break;
							}
							this._binaryReader.BaseStream.Seek(-1L, SeekOrigin.Current);
							this.Process_OneBamlRecord();
						}
						this.Process_StaticResourceEnd();
					}
					break;
				}
				if (this._binaryReader.BaseStream.Length == this._binaryReader.BaseStream.Position)
				{
					break;
				}
			}
			KeyRecord keyRecord = null;
			long position = this._binaryReader.BaseStream.Position;
			foreach (KeyRecord keyRecord2 in this._context.KeyList)
			{
				keyRecord2.ValuePosition += position;
				if (keyRecord != null)
				{
					keyRecord.ValueSize = (int)(keyRecord2.ValuePosition - keyRecord.ValuePosition);
				}
				keyRecord = keyRecord2;
			}
			keyRecord.ValueSize = (int)(this._binaryReader.BaseStream.Length - keyRecord.ValuePosition);
			return this._context.KeyList;
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x001A7B20 File Offset: 0x001A6B20
		internal System.Xaml.XamlReader ReadObject(KeyRecord record)
		{
			if (record.ValuePosition == this._binaryReader.BaseStream.Length)
			{
				return null;
			}
			this._binaryReader.BaseStream.Seek(record.ValuePosition, SeekOrigin.Begin);
			this._context.CurrentKey = this._context.KeyList.IndexOf(record);
			if (this._xamlMainNodeQueue.Count > 0)
			{
				throw new System.Xaml.XamlParseException();
			}
			if (this.Read_RecordType() != Baml2006RecordType.ElementStart)
			{
				throw new System.Xaml.XamlParseException();
			}
			System.Xaml.XamlWriter xamlNodesWriter = this._xamlNodesWriter;
			int num = (record.ValueSize < 800) ? ((int)((double)record.ValueSize / 2.2)) : ((int)((double)record.ValueSize / 4.25));
			num = ((num < 8) ? 8 : num);
			XamlNodeList xamlNodeList = new XamlNodeList(this._xamlNodesReader.SchemaContext, num);
			this._xamlNodesWriter = xamlNodeList.Writer;
			Baml2006ReaderFrame currentFrame = this._context.CurrentFrame;
			this.Process_ElementStart();
			while (currentFrame != this._context.CurrentFrame)
			{
				this.Process_OneBamlRecord();
			}
			this._xamlNodesWriter.Close();
			this._xamlNodesWriter = xamlNodesWriter;
			return xamlNodeList.GetReader();
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x001A7C44 File Offset: 0x001A6C44
		internal Type GetTypeOfFirstStartObject(KeyRecord record)
		{
			this._context.CurrentKey = this._context.KeyList.IndexOf(record);
			if (record.ValuePosition == this._binaryReader.BaseStream.Length)
			{
				return null;
			}
			this._binaryReader.BaseStream.Seek(record.ValuePosition, SeekOrigin.Begin);
			if (this.Read_RecordType() != Baml2006RecordType.ElementStart)
			{
				throw new System.Xaml.XamlParseException();
			}
			return this.BamlSchemaContext.GetClrType(this._binaryReader.ReadInt16());
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x001A7CC4 File Offset: 0x001A6CC4
		private bool Process_BamlRecords()
		{
			int count = this._xamlMainNodeQueue.Count;
			while (this.Process_OneBamlRecord())
			{
				if (this._xamlMainNodeQueue.Count > count)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x001A7CF8 File Offset: 0x001A6CF8
		private bool Process_OneBamlRecord()
		{
			if (this._binaryReader.BaseStream.Position == this._binaryReader.BaseStream.Length)
			{
				this._isEof = true;
				return false;
			}
			Baml2006RecordType baml2006RecordType = this.Read_RecordType();
			switch (baml2006RecordType)
			{
			case Baml2006RecordType.DocumentStart:
				this.SkipBytes(6L);
				return true;
			case Baml2006RecordType.DocumentEnd:
				return false;
			case Baml2006RecordType.ElementStart:
				this.Process_ElementStart();
				return true;
			case Baml2006RecordType.ElementEnd:
				this.Process_ElementEnd();
				return true;
			case Baml2006RecordType.Property:
				this.Process_Property();
				return true;
			case Baml2006RecordType.PropertyCustom:
				this.Process_PropertyCustom();
				return true;
			case Baml2006RecordType.PropertyComplexStart:
				this.Process_PropertyComplexStart();
				return true;
			case Baml2006RecordType.PropertyComplexEnd:
			case Baml2006RecordType.PropertyArrayEnd:
			case Baml2006RecordType.PropertyIListEnd:
				this.Process_PropertyEnd();
				return true;
			case Baml2006RecordType.PropertyArrayStart:
			case Baml2006RecordType.PropertyIListStart:
				this.Process_PropertyArrayStart();
				return true;
			case Baml2006RecordType.PropertyIDictionaryStart:
				this.Process_PropertyIDictionaryStart();
				return true;
			case Baml2006RecordType.PropertyIDictionaryEnd:
				this.Process_PropertyIDictionaryEnd();
				return true;
			case Baml2006RecordType.LiteralContent:
				this.Process_LiteralContent();
				return true;
			case Baml2006RecordType.Text:
				this.Process_Text();
				return true;
			case Baml2006RecordType.TextWithConverter:
				this.Process_TextWithConverter();
				return true;
			case Baml2006RecordType.RoutedEvent:
				this.Process_RoutedEvent();
				return true;
			case Baml2006RecordType.ClrEvent:
				this.Process_ClrEvent();
				return true;
			case Baml2006RecordType.XmlnsProperty:
				throw new System.Xaml.XamlParseException("Found unexpected Xmlns BAML record");
			case Baml2006RecordType.XmlAttribute:
				this.Process_XmlAttribute();
				return true;
			case Baml2006RecordType.ProcessingInstruction:
				this.Process_ProcessingInstruction();
				return true;
			case Baml2006RecordType.Comment:
				this.Process_Comment();
				return true;
			case Baml2006RecordType.DefTag:
				this.Process_DefTag();
				return true;
			case Baml2006RecordType.DefAttribute:
				this.Process_DefAttribute();
				return true;
			case Baml2006RecordType.EndAttributes:
				this.Process_EndAttributes();
				return true;
			case Baml2006RecordType.PIMapping:
				this.Process_PIMapping();
				return true;
			case Baml2006RecordType.AssemblyInfo:
				this.Process_AssemblyInfo();
				return true;
			case Baml2006RecordType.TypeInfo:
				this.Process_TypeInfo();
				return true;
			case Baml2006RecordType.TypeSerializerInfo:
				this.Process_TypeSerializerInfo();
				return true;
			case Baml2006RecordType.AttributeInfo:
				this.Process_AttributeInfo();
				return true;
			case Baml2006RecordType.StringInfo:
				this.Process_StringInfo();
				return true;
			case Baml2006RecordType.PropertyStringReference:
				this.Process_PropertyStringReference();
				return true;
			case Baml2006RecordType.PropertyTypeReference:
				this.Process_PropertyTypeReference();
				return true;
			case Baml2006RecordType.PropertyWithExtension:
				this.Process_PropertyWithExtension();
				return true;
			case Baml2006RecordType.PropertyWithConverter:
				this.Process_PropertyWithConverter();
				return true;
			case Baml2006RecordType.DeferableContentStart:
				this.Process_DeferableContentStart();
				return true;
			case Baml2006RecordType.DefAttributeKeyString:
				this.Process_DefAttributeKeyString();
				return true;
			case Baml2006RecordType.DefAttributeKeyType:
				this.Process_DefAttributeKeyType();
				return true;
			case Baml2006RecordType.KeyElementStart:
				this.Process_KeyElementStart();
				return true;
			case Baml2006RecordType.KeyElementEnd:
				this.Process_KeyElementEnd();
				return true;
			case Baml2006RecordType.ConstructorParametersStart:
				this.Process_ConstructorParametersStart();
				return true;
			case Baml2006RecordType.ConstructorParametersEnd:
				this.Process_ConstructorParametersEnd();
				return true;
			case Baml2006RecordType.ConstructorParameterType:
				this.Process_ConstructorParameterType();
				return true;
			case Baml2006RecordType.ConnectionId:
				this.Process_ConnectionId();
				return true;
			case Baml2006RecordType.ContentProperty:
				this.Process_ContentProperty();
				return true;
			case Baml2006RecordType.NamedElementStart:
				throw new System.Xaml.XamlParseException();
			case Baml2006RecordType.StaticResourceStart:
				this.Process_StaticResourceStart();
				return true;
			case Baml2006RecordType.StaticResourceEnd:
				this.Process_StaticResourceEnd();
				return true;
			case Baml2006RecordType.StaticResourceId:
				this.Process_StaticResourceId();
				return true;
			case Baml2006RecordType.TextWithId:
				this.Process_TextWithId();
				return true;
			case Baml2006RecordType.PresentationOptionsAttribute:
				this.Process_PresentationOptionsAttribute();
				return true;
			case Baml2006RecordType.LineNumberAndPosition:
				this.Process_LineNumberAndPosition();
				return true;
			case Baml2006RecordType.LinePosition:
				this.Process_LinePosition();
				return true;
			case Baml2006RecordType.OptimizedStaticResource:
				this.Process_OptimizedStaticResource();
				return true;
			case Baml2006RecordType.PropertyWithStaticResourceId:
				this.Process_PropertyWithStaticResourceId();
				return true;
			}
			throw new System.Xaml.XamlParseException(string.Format(CultureInfo.CurrentCulture, SR.Get("UnknownBamlRecord", new object[]
			{
				baml2006RecordType
			}), Array.Empty<object>()));
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x001056E1 File Offset: 0x001046E1
		private void Process_ProcessingInstruction()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x001056E1 File Offset: 0x001046E1
		private void Process_DefTag()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x001056E1 File Offset: 0x001046E1
		private void Process_EndAttributes()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x001056E1 File Offset: 0x001046E1
		private void Process_XmlAttribute()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x001A8070 File Offset: 0x001A7070
		private void Process_PresentationOptionsAttribute()
		{
			this.Common_Process_Property();
			this.Read_RecordSize();
			string value = this._binaryReader.ReadString();
			this._context.SchemaContext.GetString(this._binaryReader.ReadInt16());
			if (this._context.TemplateStartDepth < 0)
			{
				this._xamlNodesWriter.WriteStartMember(XamlReaderHelper.Freeze);
				this._xamlNodesWriter.WriteValue(value);
				this._xamlNodesWriter.WriteEndMember();
			}
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x001056E1 File Offset: 0x001046E1
		private void Process_Comment()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x001A80E8 File Offset: 0x001A70E8
		private void Process_LiteralContent()
		{
			this.Read_RecordSize();
			string text = this._binaryReader.ReadString();
			this._binaryReader.ReadInt32();
			this._binaryReader.ReadInt32();
			bool flag = this._context.CurrentFrame.Member == null;
			if (flag)
			{
				if (!(this._context.CurrentFrame.XamlType.ContentProperty != null))
				{
					throw new NotImplementedException();
				}
				this.Common_Process_Property();
				this._xamlNodesWriter.WriteStartMember(this._context.CurrentFrame.XamlType.ContentProperty);
			}
			if (!this._isBinaryProvider)
			{
				this._xamlNodesWriter.WriteStartObject(XamlLanguage.XData);
				XamlMember member = XamlLanguage.XData.GetMember("Text");
				this._xamlNodesWriter.WriteStartMember(member);
				this._xamlNodesWriter.WriteValue(text);
				this._xamlNodesWriter.WriteEndMember();
				this._xamlNodesWriter.WriteEndObject();
			}
			else
			{
				XData xdata = new XData();
				xdata.Text = text;
				this._xamlNodesWriter.WriteValue(xdata);
			}
			if (flag)
			{
				this._xamlNodesWriter.WriteEndMember();
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x001A8204 File Offset: 0x001A7204
		private void Process_TextWithConverter()
		{
			this.Read_RecordSize();
			string value = this._binaryReader.ReadString();
			this._binaryReader.ReadInt16();
			bool flag = this._context.CurrentFrame.Member == null;
			if (flag)
			{
				this.Common_Process_Property();
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.Initialization);
			}
			this._xamlNodesWriter.WriteValue(value);
			if (flag)
			{
				this._xamlNodesWriter.WriteEndMember();
			}
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x001A8278 File Offset: 0x001A7278
		private void Process_StaticResourceEnd()
		{
			System.Xaml.XamlWriter writer = this.GetLastStaticResource().ResourceNodeList.Writer;
			writer.WriteEndObject();
			writer.Close();
			this._context.InsideStaticResource = false;
			this._xamlNodesWriter = this._xamlWriterStack.Pop();
			this._context.PopScope();
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x001A82C8 File Offset: 0x001A72C8
		private void Process_StaticResourceStart()
		{
			XamlType xamlType = this.BamlSchemaContext.GetXamlType(this._binaryReader.ReadInt16());
			this._binaryReader.ReadByte();
			StaticResource staticResource = new StaticResource(xamlType, this.BamlSchemaContext);
			this._context.LastKey.StaticResources.Add(staticResource);
			this._context.InsideStaticResource = true;
			this._xamlWriterStack.Push(this._xamlNodesWriter);
			this._xamlNodesWriter = staticResource.ResourceNodeList.Writer;
			this._context.PushScope();
			this._context.CurrentFrame.XamlType = xamlType;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x001A8368 File Offset: 0x001A7368
		private void Process_StaticResourceId()
		{
			this.InjectPropertyAndFrameIfNeeded(this._context.SchemaContext.GetXamlType(typeof(StaticResourceExtension)), 0);
			short index = this._binaryReader.ReadInt16();
			object obj = this._context.KeyList[this._context.CurrentKey - 1].StaticResources[(int)index];
			StaticResource staticResource = obj as StaticResource;
			if (staticResource != null)
			{
				XamlServices.Transform(staticResource.ResourceNodeList.GetReader(), this._xamlNodesWriter, false);
				return;
			}
			this._xamlNodesWriter.WriteValue(obj);
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x001056E1 File Offset: 0x001046E1
		private void Process_ClrEvent()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x001056E1 File Offset: 0x001046E1
		private void Process_RoutedEvent()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x001056E1 File Offset: 0x001046E1
		private void Process_PropertyStringReference()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x001A83FC File Offset: 0x001A73FC
		private void Process_OptimizedStaticResource()
		{
			byte flags = this._binaryReader.ReadByte();
			short num = this._binaryReader.ReadInt16();
			OptimizedStaticResource optimizedStaticResource = new OptimizedStaticResource(flags, num);
			if (this._isBinaryProvider)
			{
				if (optimizedStaticResource.IsKeyTypeExtension)
				{
					XamlType xamlType = this.BamlSchemaContext.GetXamlType(num);
					optimizedStaticResource.KeyValue = xamlType.UnderlyingType;
				}
				else if (optimizedStaticResource.IsKeyStaticExtension)
				{
					Type memberType;
					object obj;
					string staticExtensionValue = this.GetStaticExtensionValue(num, out memberType, out obj);
					if (obj == null)
					{
						obj = new StaticExtension(staticExtensionValue)
						{
							MemberType = memberType
						}.ProvideValue(null);
					}
					optimizedStaticResource.KeyValue = obj;
				}
				else
				{
					optimizedStaticResource.KeyValue = this._context.SchemaContext.GetString(num);
				}
			}
			this._context.LastKey.StaticResources.Add(optimizedStaticResource);
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x001A84B8 File Offset: 0x001A74B8
		private void Process_DeferableContentStart()
		{
			int num = this._binaryReader.ReadInt32();
			if (this._isBinaryProvider && num > 0)
			{
				object value;
				if (this._settings.OwnsStream)
				{
					long position = this._binaryReader.BaseStream.Position;
					value = new SharedStream(this._binaryReader.BaseStream, position, (long)num);
					this._binaryReader.BaseStream.Seek(position + (long)num, SeekOrigin.Begin);
				}
				else
				{
					value = new MemoryStream(this._binaryReader.ReadBytes(num));
				}
				this.Common_Process_Property();
				this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.ResourceDictionaryDeferredContentProperty);
				this._xamlNodesWriter.WriteValue(value);
				this._xamlNodesWriter.WriteEndMember();
				return;
			}
			this._context.KeyList = new List<KeyRecord>();
			this._context.CurrentKey = 0;
			this._context.CurrentFrame.IsDeferredContent = true;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x001A85A4 File Offset: 0x001A75A4
		private void Process_DefAttribute()
		{
			this.Read_RecordSize();
			string text = this._binaryReader.ReadString();
			short stringId = this._binaryReader.ReadInt16();
			XamlMember xamlDirective = this.BamlSchemaContext.GetXamlDirective("http://schemas.microsoft.com/winfx/2006/xaml", this.BamlSchemaContext.GetString(stringId));
			if (xamlDirective == XamlLanguage.Key)
			{
				this._context.CurrentFrame.Key = new KeyRecord(false, false, 0, text);
				return;
			}
			this.Common_Process_Property();
			this._xamlNodesWriter.WriteStartMember(xamlDirective);
			this._xamlNodesWriter.WriteValue(text);
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x001A8640 File Offset: 0x001A7640
		private void Process_DefAttributeKeyString()
		{
			this.Read_RecordSize();
			short stringId = this._binaryReader.ReadInt16();
			int valuePosition = this._binaryReader.ReadInt32();
			bool shared = this._binaryReader.ReadBoolean();
			bool sharedSet = this._binaryReader.ReadBoolean();
			string @string = this._context.SchemaContext.GetString(stringId);
			KeyRecord keyRecord = new KeyRecord(shared, sharedSet, valuePosition, @string);
			if (this._context.CurrentFrame.IsDeferredContent)
			{
				this._context.KeyList.Add(keyRecord);
				return;
			}
			this._context.CurrentFrame.Key = keyRecord;
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x001A86D8 File Offset: 0x001A76D8
		private void Process_DefAttributeKeyType()
		{
			short typeId = this._binaryReader.ReadInt16();
			this._binaryReader.ReadByte();
			int valuePosition = this._binaryReader.ReadInt32();
			bool shared = this._binaryReader.ReadBoolean();
			bool sharedSet = this._binaryReader.ReadBoolean();
			Type type = Baml2006SchemaContext.KnownTypes.GetKnownType(typeId);
			if (type == null)
			{
				type = this.BamlSchemaContext.GetClrType(typeId);
			}
			KeyRecord keyRecord = new KeyRecord(shared, sharedSet, valuePosition, type);
			if (this._context.CurrentFrame.IsDeferredContent)
			{
				this._context.KeyList.Add(keyRecord);
				return;
			}
			this._context.CurrentFrame.Key = keyRecord;
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x001A8780 File Offset: 0x001A7780
		private bool IsStringOnlyWhiteSpace(string value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x001A87B0 File Offset: 0x001A77B0
		private void Process_Text()
		{
			this.Read_RecordSize();
			string stringValue = this._binaryReader.ReadString();
			this.Process_Text_Helper(stringValue);
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x001A87D8 File Offset: 0x001A77D8
		private void Process_TextWithId()
		{
			this.Read_RecordSize();
			short stringId = this._binaryReader.ReadInt16();
			string @string = this.BamlSchemaContext.GetString(stringId);
			this.Process_Text_Helper(@string);
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x001A880C File Offset: 0x001A780C
		private void Process_Text_Helper(string stringValue)
		{
			if (!this._context.InsideKeyRecord && !this._context.InsideStaticResource)
			{
				this.InjectPropertyAndFrameIfNeeded(this._context.SchemaContext.GetXamlType(typeof(string)), 0);
			}
			if (this.IsStringOnlyWhiteSpace(stringValue) && this._context.CurrentFrame.Member != XamlLanguage.PositionalParameters)
			{
				if (this._context.CurrentFrame.XamlType != null && this._context.CurrentFrame.XamlType.IsCollection)
				{
					if (!this._context.CurrentFrame.XamlType.IsWhitespaceSignificantCollection)
					{
						return;
					}
				}
				else if (this._context.CurrentFrame.Member.Type != null && !this._context.CurrentFrame.Member.Type.UnderlyingType.IsAssignableFrom(typeof(string)))
				{
					return;
				}
			}
			this._xamlNodesWriter.WriteValue(stringValue);
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x001A891B File Offset: 0x001A791B
		private void Process_ConstructorParametersEnd()
		{
			this._xamlNodesWriter.WriteEndMember();
			this._context.CurrentFrame.Member = null;
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x001A8939 File Offset: 0x001A7939
		private void Process_ConstructorParametersStart()
		{
			this.Common_Process_Property();
			this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
			this._context.CurrentFrame.Member = XamlLanguage.PositionalParameters;
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x001A8968 File Offset: 0x001A7968
		private void Process_ConstructorParameterType()
		{
			short typeId = this._binaryReader.ReadInt16();
			if (this._isBinaryProvider)
			{
				this._xamlNodesWriter.WriteValue(this.BamlSchemaContext.GetXamlType(typeId).UnderlyingType);
				return;
			}
			this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
			this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
			this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.BamlSchemaContext.GetXamlType(typeId)));
			this._xamlNodesWriter.WriteEndMember();
			this._xamlNodesWriter.WriteEndObject();
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x001A89FC File Offset: 0x001A79FC
		private void Process_Header()
		{
			int count = this._binaryReader.ReadInt32();
			this._binaryReader.ReadBytes(count);
			this._binaryReader.ReadInt32();
			this._binaryReader.ReadInt32();
			this._binaryReader.ReadInt32();
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x001A8A48 File Offset: 0x001A7A48
		private void Process_ElementStart()
		{
			short typeId = this._binaryReader.ReadInt16();
			XamlType xamlType;
			if (this._root != null && this._context.CurrentFrame.Depth == 0)
			{
				Type type = this._root.GetType();
				xamlType = this.BamlSchemaContext.GetXamlType(type);
			}
			else
			{
				xamlType = this.BamlSchemaContext.GetXamlType(typeId);
			}
			sbyte b = this._binaryReader.ReadSByte();
			if (b < 0 || b > 3)
			{
				throw new System.Xaml.XamlParseException();
			}
			this.InjectPropertyAndFrameIfNeeded(xamlType, b);
			this._context.PushScope();
			this._context.CurrentFrame.XamlType = xamlType;
			bool flag = true;
			for (;;)
			{
				Baml2006RecordType baml2006RecordType = this.Read_RecordType();
				if (baml2006RecordType <= Baml2006RecordType.AssemblyInfo)
				{
					if (baml2006RecordType != Baml2006RecordType.XmlnsProperty)
					{
						if (baml2006RecordType != Baml2006RecordType.AssemblyInfo)
						{
							goto IL_DA;
						}
						this.Process_AssemblyInfo();
					}
					else
					{
						this.Process_XmlnsProperty();
					}
				}
				else if (baml2006RecordType != Baml2006RecordType.LineNumberAndPosition)
				{
					if (baml2006RecordType != Baml2006RecordType.LinePosition)
					{
						goto IL_DA;
					}
					this.Process_LinePosition();
				}
				else
				{
					this.Process_LineNumberAndPosition();
				}
				IL_E4:
				if (!flag)
				{
					break;
				}
				continue;
				IL_DA:
				this.SkipBytes(-1L);
				flag = false;
				goto IL_E4;
			}
			if ((b & 2) > 0)
			{
				this._xamlNodesWriter.WriteGetObject();
			}
			else
			{
				this._xamlNodesWriter.WriteStartObject(this._context.CurrentFrame.XamlType);
			}
			if (this._context.CurrentFrame.Depth == 1 && this._settings.BaseUri != null && !string.IsNullOrEmpty(this._settings.BaseUri.ToString()))
			{
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.Base);
				this._xamlNodesWriter.WriteValue(this._settings.BaseUri.ToString());
				this._xamlNodesWriter.WriteEndMember();
			}
			if (this._context.PreviousFrame.IsDeferredContent && !this._context.InsideStaticResource)
			{
				if (!this._isBinaryProvider)
				{
					this._xamlNodesWriter.WriteStartMember(XamlLanguage.Key);
					KeyRecord keyRecord = this._context.KeyList[this._context.CurrentKey];
					if (!string.IsNullOrEmpty(keyRecord.KeyString))
					{
						this._xamlNodesWriter.WriteValue(keyRecord.KeyString);
					}
					else if (keyRecord.KeyType != null)
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.SchemaContext.GetXamlType(keyRecord.KeyType)));
						this._xamlNodesWriter.WriteEndMember();
						this._xamlNodesWriter.WriteEndObject();
					}
					else
					{
						XamlServices.Transform(keyRecord.KeyNodeList.GetReader(), this._xamlNodesWriter, false);
					}
					this._xamlNodesWriter.WriteEndMember();
				}
				Baml2006ReaderContext context = this._context;
				int currentKey = context.CurrentKey;
				context.CurrentKey = currentKey + 1;
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x001A8D08 File Offset: 0x001A7D08
		private void Process_ElementEnd()
		{
			this.RemoveImplicitFrame();
			if (this._context.CurrentFrame.Key != null)
			{
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.Key);
				KeyRecord key = this._context.CurrentFrame.Key;
				if (key.KeyType != null)
				{
					if (this._isBinaryProvider)
					{
						this._xamlNodesWriter.WriteValue(key.KeyType);
					}
					else
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.SchemaContext.GetXamlType(key.KeyType)));
						this._xamlNodesWriter.WriteEndMember();
						this._xamlNodesWriter.WriteEndObject();
					}
				}
				else if (key.KeyNodeList != null)
				{
					XamlServices.Transform(key.KeyNodeList.GetReader(), this._xamlNodesWriter, false);
				}
				else
				{
					this._xamlNodesWriter.WriteValue(key.KeyString);
				}
				this._xamlNodesWriter.WriteEndMember();
				this._context.CurrentFrame.Key = null;
			}
			if (this._context.CurrentFrame.DelayedConnectionId != -1)
			{
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.ConnectionId);
				if (this._isBinaryProvider)
				{
					this._xamlNodesWriter.WriteValue(this._context.CurrentFrame.DelayedConnectionId);
				}
				else
				{
					this._xamlNodesWriter.WriteValue(this._context.CurrentFrame.DelayedConnectionId.ToString(TypeConverterHelper.InvariantEnglishUS));
				}
				this._xamlNodesWriter.WriteEndMember();
			}
			this._xamlNodesWriter.WriteEndObject();
			if (this._context.CurrentFrame.IsDeferredContent)
			{
				this._context.KeyList = null;
			}
			this._context.PopScope();
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x001A8EDC File Offset: 0x001A7EDC
		private void Process_KeyElementStart()
		{
			short typeId = this._binaryReader.ReadInt16();
			byte flags = this._binaryReader.ReadByte();
			int valuePosition = this._binaryReader.ReadInt32();
			bool shared = this._binaryReader.ReadBoolean();
			bool sharedSet = this._binaryReader.ReadBoolean();
			XamlType xamlType = this._context.SchemaContext.GetXamlType(typeId);
			this._context.PushScope();
			this._context.CurrentFrame.XamlType = xamlType;
			KeyRecord keyRecord = new KeyRecord(shared, sharedSet, valuePosition, this._context.SchemaContext);
			keyRecord.Flags = flags;
			keyRecord.KeyNodeList.Writer.WriteStartObject(xamlType);
			this._context.InsideKeyRecord = true;
			this._xamlWriterStack.Push(this._xamlNodesWriter);
			this._xamlNodesWriter = keyRecord.KeyNodeList.Writer;
			if (this._context.PreviousFrame.IsDeferredContent)
			{
				this._context.KeyList.Add(keyRecord);
				return;
			}
			this._context.PreviousFrame.Key = keyRecord;
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x001A8FEC File Offset: 0x001A7FEC
		private void Process_KeyElementEnd()
		{
			KeyRecord keyRecord;
			if (this._context.PreviousFrame.IsDeferredContent)
			{
				keyRecord = this._context.LastKey;
			}
			else
			{
				keyRecord = this._context.PreviousFrame.Key;
			}
			keyRecord.KeyNodeList.Writer.WriteEndObject();
			keyRecord.KeyNodeList.Writer.Close();
			this._xamlNodesWriter = this._xamlWriterStack.Pop();
			this._context.InsideKeyRecord = false;
			this._context.PopScope();
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x001A9074 File Offset: 0x001A8074
		private void Process_Property()
		{
			this.Common_Process_Property();
			this.Read_RecordSize();
			if (this._context.CurrentFrame.XamlType.UnderlyingType == typeof(EventSetter))
			{
				this._xamlNodesWriter.WriteStartMember(this._context.SchemaContext.EventSetterEventProperty);
				XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), false);
				Type type = property.DeclaringType.UnderlyingType;
				while (null != type)
				{
					SecurityHelper.RunClassConstructor(type);
					type = type.BaseType;
				}
				RoutedEvent routedEventFromName = EventManager.GetRoutedEventFromName(property.Name, property.DeclaringType.UnderlyingType);
				this._xamlNodesWriter.WriteValue(routedEventFromName);
				this._xamlNodesWriter.WriteEndMember();
				this._xamlNodesWriter.WriteStartMember(this._context.SchemaContext.EventSetterHandlerProperty);
				this._xamlNodesWriter.WriteValue(this._binaryReader.ReadString());
				this._xamlNodesWriter.WriteEndMember();
				return;
			}
			XamlMember property2 = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._xamlNodesWriter.WriteStartMember(property2);
			this._xamlNodesWriter.WriteValue(this._binaryReader.ReadString());
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x001A91C4 File Offset: 0x001A81C4
		private void Common_Process_Property()
		{
			if (this._context.InsideKeyRecord || this._context.InsideStaticResource)
			{
				return;
			}
			this.RemoveImplicitFrame();
			if (this._context.CurrentFrame.XamlType == null)
			{
				throw new System.Xaml.XamlParseException(SR.Get("PropertyFoundOutsideStartElement"));
			}
			if (this._context.CurrentFrame.Member != null)
			{
				throw new System.Xaml.XamlParseException(SR.Get("PropertyOutOfOrder", new object[]
				{
					this._context.CurrentFrame.Member
				}));
			}
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x001A925C File Offset: 0x001A825C
		private Int32Collection GetInt32Collection()
		{
			BinaryReader binaryReader = new BinaryReader(this._binaryReader.BaseStream);
			XamlInt32CollectionSerializer.IntegerCollectionType integerCollectionType = (XamlInt32CollectionSerializer.IntegerCollectionType)binaryReader.ReadByte();
			int num = binaryReader.ReadInt32();
			if (num < 0)
			{
				throw new ArgumentException(SR.Get("IntegerCollectionLengthLessThanZero", new object[0]));
			}
			Int32Collection int32Collection = new Int32Collection(num);
			switch (integerCollectionType)
			{
			case XamlInt32CollectionSerializer.IntegerCollectionType.Consecutive:
			{
				int num2 = binaryReader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					int32Collection.Add(num2 + i);
				}
				return int32Collection;
			}
			case XamlInt32CollectionSerializer.IntegerCollectionType.Byte:
				for (int j = 0; j < num; j++)
				{
					int32Collection.Add((int)binaryReader.ReadByte());
				}
				return int32Collection;
			case XamlInt32CollectionSerializer.IntegerCollectionType.UShort:
				for (int k = 0; k < num; k++)
				{
					int32Collection.Add((int)binaryReader.ReadUInt16());
				}
				return int32Collection;
			case XamlInt32CollectionSerializer.IntegerCollectionType.Integer:
				for (int l = 0; l < num; l++)
				{
					int value = binaryReader.ReadInt32();
					int32Collection.Add(value);
				}
				return int32Collection;
			default:
				throw new InvalidOperationException(SR.Get("UnableToConvertInt32"));
			}
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x001A9358 File Offset: 0x001A8358
		private XamlMember GetProperty(short propertyId, XamlType parentType)
		{
			return this.BamlSchemaContext.GetProperty(propertyId, parentType);
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x001A9367 File Offset: 0x001A8367
		private XamlMember GetProperty(short propertyId, bool isAttached)
		{
			return this.BamlSchemaContext.GetProperty(propertyId, isAttached);
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x001A9378 File Offset: 0x001A8378
		private void Process_PropertyCustom()
		{
			this.Common_Process_Property();
			int num = this.Read_RecordSize();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._xamlNodesWriter.WriteStartMember(property);
			short num2 = this._binaryReader.ReadInt16();
			if ((num2 & 16384) == 16384)
			{
				num2 &= -16385;
			}
			if (this._isBinaryProvider)
			{
				this.WriteTypeConvertedInstance(num2, num - 5);
			}
			else
			{
				this._xamlNodesWriter.WriteValue(this.GetTextFromBinary(this._binaryReader.ReadBytes(num - 5), num2, property, this._context.CurrentFrame.XamlType));
			}
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x001A9434 File Offset: 0x001A8434
		private bool WriteTypeConvertedInstance(short converterId, int dataByteSize)
		{
			if (converterId <= 137)
			{
				if (converterId == 46)
				{
					this._xamlNodesWriter.WriteValue(this._binaryReader.ReadBytes(1)[0] != 0);
					return true;
				}
				if (converterId == 137)
				{
					DependencyProperty value;
					if (dataByteSize == 2)
					{
						value = this.BamlSchemaContext.GetDependencyProperty(this._binaryReader.ReadInt16());
					}
					else
					{
						Type underlyingType = this.BamlSchemaContext.GetXamlType(this._binaryReader.ReadInt16()).UnderlyingType;
						value = DependencyProperty.FromName(this._binaryReader.ReadString(), underlyingType);
					}
					this._xamlNodesWriter.WriteValue(value);
					return true;
				}
			}
			else
			{
				if (converterId == 195)
				{
					TypeConverter typeConverter = new EnumConverter(this._context.CurrentFrame.XamlType.UnderlyingType);
					this._xamlNodesWriter.WriteValue(typeConverter.ConvertFrom(this._binaryReader.ReadBytes(dataByteSize)));
					return true;
				}
				if (converterId == 615)
				{
					this._xamlNodesWriter.WriteValue(this._binaryReader.ReadString());
					return true;
				}
				switch (converterId)
				{
				case 744:
				case 746:
				case 747:
				case 748:
				case 752:
				{
					DeferredBinaryDeserializerExtension value2 = new DeferredBinaryDeserializerExtension(this, this._binaryReader, (int)converterId, dataByteSize);
					this._xamlNodesWriter.WriteValue(value2);
					return true;
				}
				case 745:
					this._xamlNodesWriter.WriteValue(this.GetInt32Collection());
					return true;
				}
			}
			throw new NotImplementedException();
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x001A95BC File Offset: 0x001A85BC
		private void Process_PropertyWithConverter()
		{
			this.Common_Process_Property();
			this.Read_RecordSize();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._xamlNodesWriter.WriteStartMember(property);
			object obj = this._binaryReader.ReadString();
			short num = this._binaryReader.ReadInt16();
			if (this._isBinaryProvider && num < 0 && -num != 615)
			{
				TypeConverter typeConverter = null;
				if (-num == 195)
				{
					Type underlyingType = property.Type.UnderlyingType;
					if (underlyingType.IsEnum && !this._enumTypeConverterMap.TryGetValue(underlyingType, out typeConverter))
					{
						typeConverter = new EnumConverter(underlyingType);
						this._enumTypeConverterMap[underlyingType] = typeConverter;
					}
				}
				else if (!this._typeConverterMap.TryGetValue((int)num, out typeConverter))
				{
					typeConverter = Baml2006SchemaContext.KnownTypes.CreateKnownTypeConverter(num);
					this._typeConverterMap[(int)num] = typeConverter;
				}
				if (typeConverter != null)
				{
					obj = this.CreateTypeConverterMarkupExtension(property, typeConverter, obj, this._settings);
				}
			}
			this._xamlNodesWriter.WriteValue(obj);
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x001A96D2 File Offset: 0x001A86D2
		internal virtual object CreateTypeConverterMarkupExtension(XamlMember property, TypeConverter converter, object propertyValue, Baml2006ReaderSettings settings)
		{
			return new TypeConverterMarkupExtension(converter, propertyValue);
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x001A96DC File Offset: 0x001A86DC
		private void Process_PropertyWithExtension()
		{
			this.Common_Process_Property();
			short propertyId = this._binaryReader.ReadInt16();
			short num = this._binaryReader.ReadInt16();
			short num2 = this._binaryReader.ReadInt16();
			XamlMember property = this.GetProperty(propertyId, this._context.CurrentFrame.XamlType);
			short num3 = num & 4095;
			XamlType xamlType = this.BamlSchemaContext.GetXamlType(-num3);
			bool flag = (num & 16384) == 16384;
			bool flag2 = (num & 8192) == 8192;
			Type type = null;
			object value = null;
			this._xamlNodesWriter.WriteStartMember(property);
			bool flag3 = false;
			if (this._isBinaryProvider)
			{
				object obj = null;
				object obj2;
				if (flag2)
				{
					Type memberType = null;
					string staticExtensionValue = this.GetStaticExtensionValue(num2, out memberType, out obj);
					if (obj != null)
					{
						obj2 = obj;
					}
					else
					{
						obj2 = new StaticExtension(staticExtensionValue)
						{
							MemberType = memberType
						}.ProvideValue(null);
					}
				}
				else if (flag)
				{
					obj2 = this.BamlSchemaContext.GetXamlType(num2).UnderlyingType;
				}
				else if (num3 == 634)
				{
					obj2 = this._context.SchemaContext.GetDependencyProperty(num2);
				}
				else if (num3 == 602)
				{
					obj2 = this.GetStaticExtensionValue(num2, out type, out obj);
				}
				else if (num3 == 691)
				{
					obj2 = this.BamlSchemaContext.GetXamlType(num2).UnderlyingType;
				}
				else
				{
					obj2 = this.BamlSchemaContext.GetString(num2);
				}
				if (num3 == 189)
				{
					value = new DynamicResourceExtension(obj2);
					flag3 = true;
				}
				else if (num3 == 603)
				{
					value = new StaticResourceExtension(obj2);
					flag3 = true;
				}
				else if (num3 == 634)
				{
					value = new TemplateBindingExtension((DependencyProperty)obj2);
					flag3 = true;
				}
				else if (num3 == 691)
				{
					value = obj2;
					flag3 = true;
				}
				else if (num3 == 602)
				{
					if (obj != null)
					{
						value = obj;
					}
					else
					{
						value = new StaticExtension((string)obj2)
						{
							MemberType = type
						};
					}
					flag3 = true;
				}
				if (flag3)
				{
					this._xamlNodesWriter.WriteValue(value);
					this._xamlNodesWriter.WriteEndMember();
					return;
				}
			}
			if (!flag3)
			{
				this._xamlNodesWriter.WriteStartObject(xamlType);
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
				if (flag2)
				{
					Type type2 = null;
					object obj3;
					value = this.GetStaticExtensionValue(num2, out type2, out obj3);
					if (obj3 != null)
					{
						this._xamlNodesWriter.WriteValue(obj3);
					}
					else
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Static);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(value);
						this._xamlNodesWriter.WriteEndMember();
						if (type2 != null)
						{
							this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.StaticExtensionMemberTypeProperty);
							this._xamlNodesWriter.WriteValue(type2);
							this._xamlNodesWriter.WriteEndMember();
						}
						this._xamlNodesWriter.WriteEndObject();
					}
				}
				else if (flag)
				{
					this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
					this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.TypeExtensionTypeProperty);
					Type underlyingType = this.BamlSchemaContext.GetXamlType(num2).UnderlyingType;
					if (this._isBinaryProvider)
					{
						this._xamlNodesWriter.WriteValue(underlyingType);
					}
					else
					{
						this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.BamlSchemaContext.GetXamlType(num2)));
					}
					this._xamlNodesWriter.WriteEndMember();
					this._xamlNodesWriter.WriteEndObject();
				}
				else
				{
					if (num3 == 634)
					{
						if (this._isBinaryProvider)
						{
							value = BitConverter.GetBytes(num2);
						}
						else
						{
							value = this.Logic_GetFullyQualifiedNameForMember(num2);
						}
					}
					else if (num3 == 602)
					{
						object obj4;
						value = this.GetStaticExtensionValue(num2, out type, out obj4);
					}
					else if (num3 == 691)
					{
						value = this.BamlSchemaContext.GetXamlType(num2).UnderlyingType;
					}
					else
					{
						value = this.BamlSchemaContext.GetString(num2);
					}
					this._xamlNodesWriter.WriteValue(value);
				}
				this._xamlNodesWriter.WriteEndMember();
				if (type != null)
				{
					this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.StaticExtensionMemberTypeProperty);
					this._xamlNodesWriter.WriteValue(type);
					this._xamlNodesWriter.WriteEndMember();
				}
			}
			this._xamlNodesWriter.WriteEndObject();
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x001A9B08 File Offset: 0x001A8B08
		private void Process_PropertyTypeReference()
		{
			this.Common_Process_Property();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			XamlType xamlType = this.BamlSchemaContext.GetXamlType(this._binaryReader.ReadInt16());
			this._xamlNodesWriter.WriteStartMember(property);
			if (this._isBinaryProvider)
			{
				this._xamlNodesWriter.WriteValue(xamlType.UnderlyingType);
			}
			else
			{
				this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
				this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(xamlType));
				this._xamlNodesWriter.WriteEndMember();
				this._xamlNodesWriter.WriteEndObject();
			}
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x001A9BD0 File Offset: 0x001A8BD0
		private void Process_PropertyWithStaticResourceId()
		{
			this.Common_Process_Property();
			short propertyId = this._binaryReader.ReadInt16();
			short index = this._binaryReader.ReadInt16();
			XamlMember property = this._context.SchemaContext.GetProperty(propertyId, this._context.CurrentFrame.XamlType);
			object obj = this._context.KeyList[this._context.CurrentKey - 1].StaticResources[(int)index];
			if (obj is StaticResourceHolder)
			{
				this._xamlNodesWriter.WriteStartMember(property);
				this._xamlNodesWriter.WriteValue(obj);
				this._xamlNodesWriter.WriteEndMember();
				return;
			}
			this._xamlNodesWriter.WriteStartMember(property);
			this._xamlNodesWriter.WriteStartObject(this.BamlSchemaContext.StaticResourceExtensionType);
			this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
			OptimizedStaticResource optimizedStaticResource = obj as OptimizedStaticResource;
			if (optimizedStaticResource != null)
			{
				if (optimizedStaticResource.IsKeyStaticExtension)
				{
					Type type = null;
					object obj2;
					string staticExtensionValue = this.GetStaticExtensionValue(optimizedStaticResource.KeyId, out type, out obj2);
					if (obj2 != null)
					{
						this._xamlNodesWriter.WriteValue(obj2);
					}
					else
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Static);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(staticExtensionValue);
						this._xamlNodesWriter.WriteEndMember();
						if (type != null)
						{
							this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.StaticExtensionMemberTypeProperty);
							this._xamlNodesWriter.WriteValue(type);
							this._xamlNodesWriter.WriteEndMember();
						}
						this._xamlNodesWriter.WriteEndObject();
					}
				}
				else if (optimizedStaticResource.IsKeyTypeExtension)
				{
					if (this._isBinaryProvider)
					{
						XamlType xamlType = this.BamlSchemaContext.GetXamlType(optimizedStaticResource.KeyId);
						this._xamlNodesWriter.WriteValue(xamlType.UnderlyingType);
					}
					else
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.BamlSchemaContext.GetXamlType(optimizedStaticResource.KeyId)));
						this._xamlNodesWriter.WriteEndMember();
						this._xamlNodesWriter.WriteEndObject();
					}
				}
				else
				{
					string @string = this._context.SchemaContext.GetString(optimizedStaticResource.KeyId);
					this._xamlNodesWriter.WriteValue(@string);
				}
			}
			else
			{
				XamlServices.Transform((obj as StaticResource).ResourceNodeList.GetReader(), this._xamlNodesWriter, false);
			}
			this._xamlNodesWriter.WriteEndMember();
			this._xamlNodesWriter.WriteEndObject();
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x001A9E6C File Offset: 0x001A8E6C
		private void Process_PropertyComplexStart()
		{
			this.Common_Process_Property();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._context.CurrentFrame.Member = property;
			this._xamlNodesWriter.WriteStartMember(property);
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x001A9E6C File Offset: 0x001A8E6C
		private void Process_PropertyArrayStart()
		{
			this.Common_Process_Property();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._context.CurrentFrame.Member = property;
			this._xamlNodesWriter.WriteStartMember(property);
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x001A9E6C File Offset: 0x001A8E6C
		private void Process_PropertyIDictionaryStart()
		{
			this.Common_Process_Property();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._context.CurrentFrame.Member = property;
			this._xamlNodesWriter.WriteStartMember(property);
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x001A9EBE File Offset: 0x001A8EBE
		private void Process_PropertyEnd()
		{
			this.RemoveImplicitFrame();
			this._context.CurrentFrame.Member = null;
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x001A9EE4 File Offset: 0x001A8EE4
		private void Process_PropertyIDictionaryEnd()
		{
			if (this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth == this._context.CurrentFrame.Depth)
			{
				this.RestoreSavedFirstItemInDictionary();
			}
			this.RemoveImplicitFrame();
			this._context.CurrentFrame.Member = null;
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x001A9F31 File Offset: 0x001A8F31
		private string Logic_GetFullyQualifiedNameForMember(short propertyId)
		{
			return this.Logic_GetFullyQualifiedNameForType(this.BamlSchemaContext.GetPropertyDeclaringType(propertyId)) + "." + this.BamlSchemaContext.GetPropertyName(propertyId, false);
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x001A9F5C File Offset: 0x001A8F5C
		private string Logic_GetFullyQualifiedNameForType(XamlType type)
		{
			Baml2006ReaderFrame baml2006ReaderFrame = this._context.CurrentFrame;
			IList<string> xamlNamespaces = type.GetXamlNamespaces();
			while (baml2006ReaderFrame != null)
			{
				foreach (string xamlNs in xamlNamespaces)
				{
					string text = null;
					if (baml2006ReaderFrame.TryGetPrefixByNamespace(xamlNs, out text))
					{
						if (string.IsNullOrEmpty(text))
						{
							return type.Name;
						}
						return text + ":" + type.Name;
					}
				}
				baml2006ReaderFrame = (Baml2006ReaderFrame)baml2006ReaderFrame.Previous;
			}
			throw new InvalidOperationException("Could not find prefix for type: " + type.Name);
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x001AA010 File Offset: 0x001A9010
		private string Logic_GetFullXmlns(string uriInput)
		{
			int num = uriInput.IndexOf(':');
			if (num != -1 && string.Equals(uriInput.Substring(0, num), "clr-namespace"))
			{
				int num2 = uriInput.IndexOf(';');
				if (-1 == num2)
				{
					return uriInput + ((this._settings.LocalAssembly != null) ? (";assembly=" + this.GetAssemblyNameForNamespace(this._settings.LocalAssembly)) : string.Empty);
				}
				int num3 = num2 + 1;
				int num4 = uriInput.IndexOf('=');
				if (-1 == num4)
				{
					throw new ArgumentException(SR.Get("MissingTagInNamespace", new object[]
					{
						"=",
						uriInput
					}));
				}
				if (!string.Equals(uriInput.Substring(num3, num4 - num3), "assembly"))
				{
					throw new ArgumentException(SR.Get("AssemblyTagMissing", new object[]
					{
						"assembly",
						uriInput
					}));
				}
				if (string.IsNullOrEmpty(uriInput.Substring(num4 + 1)))
				{
					return uriInput + this.GetAssemblyNameForNamespace(this._settings.LocalAssembly);
				}
			}
			return uriInput;
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x001AA124 File Offset: 0x001A9124
		internal virtual string GetAssemblyNameForNamespace(Assembly assembly)
		{
			string fullName = assembly.FullName;
			return fullName.Substring(0, fullName.IndexOf(','));
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x001AA148 File Offset: 0x001A9148
		private void Process_XmlnsProperty()
		{
			this.Read_RecordSize();
			string prefix = this._binaryReader.ReadString();
			string text = this._binaryReader.ReadString();
			text = this.Logic_GetFullXmlns(text);
			this._context.CurrentFrame.AddNamespace(prefix, text);
			NamespaceDeclaration namespaceDeclaration = new NamespaceDeclaration(text, prefix);
			this._xamlNodesWriter.WriteNamespace(namespaceDeclaration);
			short num = this._binaryReader.ReadInt16();
			if (text.StartsWith("clr-namespace:", StringComparison.Ordinal))
			{
				this.SkipBytes((long)(num * 2));
				return;
			}
			if (num > 0)
			{
				short[] array = new short[(int)num];
				for (int i = 0; i < (int)num; i++)
				{
					array[i] = this._binaryReader.ReadInt16();
				}
				this.BamlSchemaContext.AddXmlnsMapping(text, array);
			}
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x001AA204 File Offset: 0x001A9204
		private void Process_LinePosition()
		{
			this._context.LineOffset = this._binaryReader.ReadInt32();
			IXamlLineInfoConsumer xamlLineInfoConsumer = this._xamlNodesWriter as IXamlLineInfoConsumer;
			if (xamlLineInfoConsumer != null)
			{
				xamlLineInfoConsumer.SetLineInfo(this._context.LineNumber, this._context.LineOffset);
			}
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x001AA254 File Offset: 0x001A9254
		private void Process_LineNumberAndPosition()
		{
			this._context.LineNumber = this._binaryReader.ReadInt32();
			this._context.LineOffset = this._binaryReader.ReadInt32();
			IXamlLineInfoConsumer xamlLineInfoConsumer = this._xamlNodesWriter as IXamlLineInfoConsumer;
			if (xamlLineInfoConsumer != null)
			{
				xamlLineInfoConsumer.SetLineInfo(this._context.LineNumber, this._context.LineOffset);
			}
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x001AA2B8 File Offset: 0x001A92B8
		private void Process_PIMapping()
		{
			this.Read_RecordSize();
			this._binaryReader.ReadString();
			this._binaryReader.ReadString();
			this._binaryReader.ReadInt16();
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x001AA2E8 File Offset: 0x001A92E8
		private void Process_AssemblyInfo()
		{
			this.Read_RecordSize();
			short assemblyId = this._binaryReader.ReadInt16();
			string assemblyName = this._binaryReader.ReadString();
			this.BamlSchemaContext.AddAssembly(assemblyId, assemblyName);
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x001AA324 File Offset: 0x001A9324
		private void Process_TypeInfo()
		{
			this.Read_RecordSize();
			short typeId = this._binaryReader.ReadInt16();
			short num = this._binaryReader.ReadInt16();
			string typeName = this._binaryReader.ReadString();
			Baml2006SchemaContext.TypeInfoFlags flags = (Baml2006SchemaContext.TypeInfoFlags)(num >> 12);
			num &= 4095;
			this.BamlSchemaContext.AddXamlType(typeId, num, typeName, flags);
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x001AA37C File Offset: 0x001A937C
		private void Process_TypeSerializerInfo()
		{
			this.Read_RecordSize();
			short typeId = this._binaryReader.ReadInt16();
			short num = this._binaryReader.ReadInt16();
			string typeName = this._binaryReader.ReadString();
			this._binaryReader.ReadInt16();
			Baml2006SchemaContext.TypeInfoFlags flags = (Baml2006SchemaContext.TypeInfoFlags)(num >> 12);
			num &= 4095;
			this.BamlSchemaContext.AddXamlType(typeId, num, typeName, flags);
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x001AA3E0 File Offset: 0x001A93E0
		private void Process_AttributeInfo()
		{
			this.Read_RecordSize();
			short propertyId = this._binaryReader.ReadInt16();
			short declaringTypeId = this._binaryReader.ReadInt16();
			this._binaryReader.ReadByte();
			string propertyName = this._binaryReader.ReadString();
			this.BamlSchemaContext.AddProperty(propertyId, declaringTypeId, propertyName);
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x001AA434 File Offset: 0x001A9434
		private void Process_StringInfo()
		{
			this.Read_RecordSize();
			short stringId = this._binaryReader.ReadInt16();
			string value = this._binaryReader.ReadString();
			this.BamlSchemaContext.AddString(stringId, value);
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x001AA470 File Offset: 0x001A9470
		private void Process_ContentProperty()
		{
			short num = this._binaryReader.ReadInt16();
			if (num != -174)
			{
				XamlMember xamlMember = this.GetProperty(num, false);
				WpfXamlMember wpfXamlMember = xamlMember as WpfXamlMember;
				if (wpfXamlMember != null)
				{
					xamlMember = wpfXamlMember.AsContentProperty;
				}
				this._context.CurrentFrame.ContentProperty = xamlMember;
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x001AA4C4 File Offset: 0x001A94C4
		private void Process_ConnectionId()
		{
			int num = this._binaryReader.ReadInt32();
			if (this._context.CurrentFrame.Member != null)
			{
				Baml2006ReaderFrame baml2006ReaderFrame = this._context.CurrentFrame;
				if (baml2006ReaderFrame.Flags == Baml2006ReaderFrameFlags.IsImplict)
				{
					baml2006ReaderFrame = this._context.PreviousFrame;
				}
				baml2006ReaderFrame.DelayedConnectionId = num;
				return;
			}
			this.Common_Process_Property();
			this._xamlNodesWriter.WriteStartMember(XamlLanguage.ConnectionId);
			if (this._isBinaryProvider)
			{
				this._xamlNodesWriter.WriteValue(num);
			}
			else
			{
				this._xamlNodesWriter.WriteValue(num.ToString(TypeConverterHelper.InvariantEnglishUS));
			}
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x001AA574 File Offset: 0x001A9574
		private Baml2006RecordType Read_RecordType()
		{
			byte b = this._binaryReader.ReadByte();
			if (b < 0)
			{
				return Baml2006RecordType.DocumentEnd;
			}
			return (Baml2006RecordType)b;
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x001AA594 File Offset: 0x001A9594
		private int Read_RecordSize()
		{
			long position = this._binaryReader.BaseStream.Position;
			int num = this._binaryReader.Read7BitEncodedInt();
			int num2 = (int)(this._binaryReader.BaseStream.Position - position);
			if (num2 == 1)
			{
				return num;
			}
			return num - num2 + 1;
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x001AA5DD File Offset: 0x001A95DD
		private void SkipBytes(long offset)
		{
			this._binaryReader.BaseStream.Seek(offset, SeekOrigin.Current);
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x001AA5F4 File Offset: 0x001A95F4
		private void RemoveImplicitFrame()
		{
			if (this._context.CurrentFrame.Flags == Baml2006ReaderFrameFlags.IsImplict)
			{
				this._xamlNodesWriter.WriteEndMember();
				this._xamlNodesWriter.WriteEndObject();
				this._context.PopScope();
			}
			if (this._context.CurrentFrame.Flags == Baml2006ReaderFrameFlags.HasImplicitProperty)
			{
				if (this._context.CurrentFrame.Depth == this._context.TemplateStartDepth)
				{
					this._xamlNodesWriter.Close();
					this._xamlNodesWriter = this._xamlWriterStack.Pop();
					this._xamlNodesWriter.WriteValue(this._xamlTemplateNodeList);
					this._xamlTemplateNodeList = null;
					this._context.TemplateStartDepth = -1;
				}
				this._xamlNodesWriter.WriteEndMember();
				this._context.CurrentFrame.Member = null;
				this._context.CurrentFrame.Flags = Baml2006ReaderFrameFlags.None;
			}
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x001AA6D8 File Offset: 0x001A96D8
		private void InjectPropertyAndFrameIfNeeded(XamlType elementType, sbyte flags)
		{
			if (this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth == this._context.CurrentFrame.Depth)
			{
				this.RestoreSavedFirstItemInDictionary();
			}
			XamlType xamlType = this._context.CurrentFrame.XamlType;
			XamlMember xamlMember = this._context.CurrentFrame.Member;
			if (xamlType != null)
			{
				if (xamlMember == null)
				{
					if (this._context.CurrentFrame.ContentProperty != null)
					{
						xamlMember = (this._context.CurrentFrame.Member = this._context.CurrentFrame.ContentProperty);
					}
					else if (xamlType.ContentProperty != null)
					{
						xamlMember = (this._context.CurrentFrame.Member = xamlType.ContentProperty);
					}
					else if (xamlType.IsCollection || xamlType.IsDictionary)
					{
						xamlMember = (this._context.CurrentFrame.Member = XamlLanguage.Items);
					}
					else
					{
						if (!(xamlType.TypeConverter != null))
						{
							throw new System.Xaml.XamlParseException(SR.Get("RecordOutOfOrder", new object[]
							{
								xamlType.Name
							}));
						}
						xamlMember = (this._context.CurrentFrame.Member = XamlLanguage.Initialization);
					}
					this._context.CurrentFrame.Flags = Baml2006ReaderFrameFlags.HasImplicitProperty;
					this._xamlNodesWriter.WriteStartMember(xamlMember);
					if (this._context.TemplateStartDepth < 0 && this._isBinaryProvider && xamlMember == this.BamlSchemaContext.FrameworkTemplateTemplateProperty)
					{
						this._context.TemplateStartDepth = this._context.CurrentFrame.Depth;
						this._xamlTemplateNodeList = new XamlNodeList(this._xamlNodesWriter.SchemaContext);
						this._xamlWriterStack.Push(this._xamlNodesWriter);
						this._xamlNodesWriter = this._xamlTemplateNodeList.Writer;
						if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
						{
							IXamlLineInfoConsumer xamlLineInfoConsumer = this._xamlNodesWriter as IXamlLineInfoConsumer;
							if (xamlLineInfoConsumer != null)
							{
								xamlLineInfoConsumer.SetLineInfo(this._context.LineNumber, this._context.LineOffset);
							}
						}
					}
				}
				XamlType type = xamlMember.Type;
				if (type != null && (type.IsCollection || type.IsDictionary) && !xamlMember.IsDirective && (flags & 2) == 0)
				{
					bool flag = false;
					if (xamlMember.IsReadOnly)
					{
						flag = true;
					}
					else if (!elementType.CanAssignTo(type))
					{
						if (!elementType.IsMarkupExtension)
						{
							flag = true;
						}
						else if (this._context.CurrentFrame.Flags == Baml2006ReaderFrameFlags.HasImplicitProperty)
						{
							flag = true;
						}
						else if (elementType == XamlLanguage.Array)
						{
							flag = true;
						}
					}
					if (flag)
					{
						this.EmitGoItemsPreamble(type);
					}
					if (!flag && type.IsDictionary && elementType.IsMarkupExtension)
					{
						this.StartSavingFirstItemInDictionary();
					}
				}
			}
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x001AA990 File Offset: 0x001A9990
		private void StartSavingFirstItemInDictionary()
		{
			this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth = this._context.CurrentFrame.Depth;
			this._lookingForAKeyOnAMarkupExtensionInADictionaryNodeList = new XamlNodeList(this._xamlNodesWriter.SchemaContext);
			this._xamlWriterStack.Push(this._xamlNodesWriter);
			this._xamlNodesWriter = this._lookingForAKeyOnAMarkupExtensionInADictionaryNodeList.Writer;
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x001AA9EC File Offset: 0x001A99EC
		private void RestoreSavedFirstItemInDictionary()
		{
			this._xamlNodesWriter.Close();
			this._xamlNodesWriter = this._xamlWriterStack.Pop();
			if (this.NodeListHasAKeySetOnTheRoot(this._lookingForAKeyOnAMarkupExtensionInADictionaryNodeList.GetReader()))
			{
				this.EmitGoItemsPreamble(this._context.CurrentFrame.Member.Type);
			}
			XamlServices.Transform(this._lookingForAKeyOnAMarkupExtensionInADictionaryNodeList.GetReader(), this._xamlNodesWriter, false);
			this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth = -1;
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x001AAA64 File Offset: 0x001A9A64
		private void EmitGoItemsPreamble(XamlType parentPropertyType)
		{
			this._context.PushScope();
			this._context.CurrentFrame.XamlType = parentPropertyType;
			this._xamlNodesWriter.WriteGetObject();
			this._context.CurrentFrame.Flags = Baml2006ReaderFrameFlags.IsImplict;
			this._context.CurrentFrame.Member = XamlLanguage.Items;
			this._xamlNodesWriter.WriteStartMember(this._context.CurrentFrame.Member);
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x001AAAD9 File Offset: 0x001A9AD9
		private StaticResource GetLastStaticResource()
		{
			return this._context.LastKey.LastStaticResource;
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x001AAAEC File Offset: 0x001A9AEC
		private string GetTextFromBinary(byte[] bytes, short serializerId, XamlMember property, XamlType type)
		{
			if (serializerId <= 46)
			{
				if (serializerId != 0)
				{
					if (serializerId != 46)
					{
						goto IL_269;
					}
					if (bytes[0] != 0)
					{
						return true.ToString();
					}
					return false.ToString();
				}
			}
			else if (serializerId != 137)
			{
				if (serializerId == 195)
				{
					return Enum.ToObject(type.UnderlyingType, bytes).ToString();
				}
				switch (serializerId)
				{
				case 744:
					using (MemoryStream memoryStream = new MemoryStream(bytes))
					{
						using (BinaryReader binaryReader = new BinaryReader(memoryStream))
						{
							return (SolidColorBrush.DeserializeFrom(binaryReader) as SolidColorBrush).ToString();
						}
					}
					break;
				case 745:
					goto IL_1C1;
				case 746:
					break;
				case 747:
					goto IL_101;
				case 748:
					goto IL_181;
				case 749:
				case 750:
				case 751:
					goto IL_269;
				case 752:
					goto IL_141;
				default:
					goto IL_269;
				}
				using (MemoryStream memoryStream2 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader2 = new BinaryReader(memoryStream2))
					{
						return new XamlPathDataSerializer().ConvertCustomBinaryToObject(binaryReader2).ToString();
					}
				}
				IL_101:
				using (MemoryStream memoryStream3 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader3 = new BinaryReader(memoryStream3))
					{
						return new XamlPoint3DCollectionSerializer().ConvertCustomBinaryToObject(binaryReader3).ToString();
					}
				}
				IL_141:
				using (MemoryStream memoryStream4 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader4 = new BinaryReader(memoryStream4))
					{
						return new XamlVector3DCollectionSerializer().ConvertCustomBinaryToObject(binaryReader4).ToString();
					}
				}
				IL_181:
				using (MemoryStream memoryStream5 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader5 = new BinaryReader(memoryStream5))
					{
						return new XamlPointCollectionSerializer().ConvertCustomBinaryToObject(binaryReader5).ToString();
					}
				}
				IL_1C1:
				using (MemoryStream memoryStream6 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader6 = new BinaryReader(memoryStream6))
					{
						return new XamlInt32CollectionSerializer().ConvertCustomBinaryToObject(binaryReader6).ToString();
					}
				}
			}
			if (bytes.Length == 2)
			{
				short propertyId = (short)((int)bytes[0] | (int)bytes[1] << 8);
				return this.Logic_GetFullyQualifiedNameForMember(propertyId);
			}
			using (BinaryReader binaryReader7 = new BinaryReader(new MemoryStream(bytes)))
			{
				XamlType xamlType = this.BamlSchemaContext.GetXamlType(binaryReader7.ReadInt16());
				string str = binaryReader7.ReadString();
				return this.Logic_GetFullyQualifiedNameForType(xamlType) + "." + str;
			}
			IL_269:
			throw new NotImplementedException();
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x001AAE0C File Offset: 0x001A9E0C
		private string GetStaticExtensionValue(short valueId, out Type memberType, out object providedValue)
		{
			string text = "";
			memberType = null;
			providedValue = null;
			if (valueId < 0)
			{
				valueId = -valueId;
				bool flag = true;
				valueId = SystemResourceKey.GetSystemResourceKeyIdFromBamlId(valueId, out flag);
				if (valueId <= 0 || valueId >= 236)
				{
					throw new InvalidOperationException(SR.Get("BamlBadExtensionValue"));
				}
				if (this._isBinaryProvider)
				{
					if (flag)
					{
						providedValue = SystemResourceKey.GetResourceKey(valueId);
					}
					else
					{
						providedValue = SystemResourceKey.GetResource(valueId);
					}
				}
				else
				{
					SystemResourceKeyID id = (SystemResourceKeyID)valueId;
					XamlType xamlType = this._context.SchemaContext.GetXamlType(SystemKeyConverter.GetSystemClassType(id));
					text = this.Logic_GetFullyQualifiedNameForType(xamlType) + ".";
					if (flag)
					{
						text += SystemKeyConverter.GetSystemKeyName(id);
					}
					else
					{
						text += SystemKeyConverter.GetSystemPropertyName(id);
					}
				}
			}
			else if (this._isBinaryProvider)
			{
				memberType = this.BamlSchemaContext.GetPropertyDeclaringType(valueId).UnderlyingType;
				text = this.BamlSchemaContext.GetPropertyName(valueId, false);
				providedValue = CommandConverter.GetKnownControlCommand(memberType, text);
			}
			else
			{
				text = this.Logic_GetFullyQualifiedNameForMember(valueId);
			}
			return text;
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x001AAF08 File Offset: 0x001A9F08
		private bool NodeListHasAKeySetOnTheRoot(System.Xaml.XamlReader reader)
		{
			int num = 0;
			while (reader.Read())
			{
				switch (reader.NodeType)
				{
				case System.Xaml.XamlNodeType.StartObject:
					num++;
					break;
				case System.Xaml.XamlNodeType.EndObject:
					num--;
					break;
				case System.Xaml.XamlNodeType.StartMember:
					if (reader.Member == XamlLanguage.Key && num == 1)
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06002CED RID: 11501 RVA: 0x001AAF67 File Offset: 0x001A9F67
		// (set) Token: 0x06002CEE RID: 11502 RVA: 0x001AAF79 File Offset: 0x001A9F79
		internal bool FreezeFreezables
		{
			get
			{
				return this._context.CurrentFrame.FreezeFreezables;
			}
			set
			{
				this._context.CurrentFrame.FreezeFreezables = value;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06002CEF RID: 11503 RVA: 0x001AAF67 File Offset: 0x001A9F67
		bool IFreezeFreezables.FreezeFreezables
		{
			get
			{
				return this._context.CurrentFrame.FreezeFreezables;
			}
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x001AAF8C File Offset: 0x001A9F8C
		bool IFreezeFreezables.TryFreeze(string value, Freezable freezable)
		{
			if (freezable.CanFreeze)
			{
				if (!freezable.IsFrozen)
				{
					freezable.Freeze();
				}
				if (this._freezeCache == null)
				{
					this._freezeCache = new Dictionary<string, Freezable>();
				}
				this._freezeCache.Add(value, freezable);
				return true;
			}
			return false;
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x001AAFC8 File Offset: 0x001A9FC8
		Freezable IFreezeFreezables.TryGetFreezable(string value)
		{
			Freezable result = null;
			if (this._freezeCache != null)
			{
				this._freezeCache.TryGetValue(value, out result);
			}
			return result;
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x001AAFEF File Offset: 0x001A9FEF
		private Baml2006SchemaContext BamlSchemaContext
		{
			get
			{
				return (Baml2006SchemaContext)this.SchemaContext;
			}
		}

		// Token: 0x04001B3F RID: 6975
		private Baml2006ReaderSettings _settings;

		// Token: 0x04001B40 RID: 6976
		private bool _isBinaryProvider;

		// Token: 0x04001B41 RID: 6977
		private bool _isEof;

		// Token: 0x04001B42 RID: 6978
		private int _lookingForAKeyOnAMarkupExtensionInADictionaryDepth;

		// Token: 0x04001B43 RID: 6979
		private XamlNodeList _lookingForAKeyOnAMarkupExtensionInADictionaryNodeList;

		// Token: 0x04001B44 RID: 6980
		private BamlBinaryReader _binaryReader;

		// Token: 0x04001B45 RID: 6981
		private Baml2006ReaderContext _context;

		// Token: 0x04001B46 RID: 6982
		private XamlNodeQueue _xamlMainNodeQueue;

		// Token: 0x04001B47 RID: 6983
		private XamlNodeList _xamlTemplateNodeList;

		// Token: 0x04001B48 RID: 6984
		private System.Xaml.XamlReader _xamlNodesReader;

		// Token: 0x04001B49 RID: 6985
		private System.Xaml.XamlWriter _xamlNodesWriter;

		// Token: 0x04001B4A RID: 6986
		private Stack<System.Xaml.XamlWriter> _xamlWriterStack = new Stack<System.Xaml.XamlWriter>();

		// Token: 0x04001B4B RID: 6987
		private Dictionary<int, TypeConverter> _typeConverterMap = new Dictionary<int, TypeConverter>();

		// Token: 0x04001B4C RID: 6988
		private Dictionary<Type, TypeConverter> _enumTypeConverterMap = new Dictionary<Type, TypeConverter>();

		// Token: 0x04001B4D RID: 6989
		private Dictionary<string, Freezable> _freezeCache;

		// Token: 0x04001B4E RID: 6990
		private const short ExtensionIdMask = 4095;

		// Token: 0x04001B4F RID: 6991
		private const short TypeExtensionValueMask = 16384;

		// Token: 0x04001B50 RID: 6992
		private const short StaticExtensionValueMask = 8192;

		// Token: 0x04001B51 RID: 6993
		private const sbyte ReaderFlags_AddedToTree = 2;

		// Token: 0x04001B52 RID: 6994
		private object _root;
	}
}
