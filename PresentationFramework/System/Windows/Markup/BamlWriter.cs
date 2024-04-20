using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x020004BE RID: 1214
	internal class BamlWriter : IParserHelper
	{
		// Token: 0x06003E53 RID: 15955 RVA: 0x001FFD40 File Offset: 0x001FED40
		public BamlWriter(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException(SR.Get("BamlWriterBadStream"));
			}
			this._parserContext = new ParserContext();
			if (this._parserContext.XamlTypeMapper == null)
			{
				this._parserContext.XamlTypeMapper = new BamlWriterXamlTypeMapper(XmlParserDefaults.GetDefaultAssemblyNames(), XmlParserDefaults.GetDefaultNamespaceMaps());
			}
			this._xamlTypeMapper = this._parserContext.XamlTypeMapper;
			this._bamlRecordWriter = new BamlRecordWriter(stream, this._parserContext, true);
			this._startDocumentWritten = false;
			this._depth = 0;
			this._closed = false;
			this._nodeTypeStack = new ParserStack();
			this._assemblies = new Hashtable(7);
			this._extensionParser = new MarkupExtensionParser(this, this._parserContext);
			this._markupExtensionNodes = new ArrayList();
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x001FFE18 File Offset: 0x001FEE18
		public void Close()
		{
			this._bamlRecordWriter.BamlStream.Close();
			this._closed = true;
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x001FFE31 File Offset: 0x001FEE31
		string IParserHelper.LookupNamespace(string prefix)
		{
			return this._parserContext.XmlnsDictionary[prefix];
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x001FFE44 File Offset: 0x001FEE44
		bool IParserHelper.GetElementType(bool extensionFirst, string localName, string namespaceURI, ref string assemblyName, ref string typeFullName, ref Type baseType, ref Type serializerType)
		{
			bool result = false;
			assemblyName = string.Empty;
			typeFullName = string.Empty;
			serializerType = null;
			baseType = null;
			if (namespaceURI == null || localName == null)
			{
				return false;
			}
			TypeAndSerializer typeAndSerializer = this._xamlTypeMapper.GetTypeAndSerializer(namespaceURI, localName, null);
			if (typeAndSerializer == null)
			{
				typeAndSerializer = this._xamlTypeMapper.GetTypeAndSerializer(namespaceURI, localName + "Extension", null);
			}
			if (typeAndSerializer != null && typeAndSerializer.ObjectType != null)
			{
				serializerType = typeAndSerializer.SerializerType;
				baseType = typeAndSerializer.ObjectType;
				typeFullName = baseType.FullName;
				assemblyName = baseType.Assembly.FullName;
				result = true;
			}
			return result;
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IParserHelper.CanResolveLocalAssemblies()
		{
			return false;
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x001FFEE0 File Offset: 0x001FEEE0
		public void WriteStartDocument()
		{
			if (this._closed)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterClosed"));
			}
			if (this._startDocumentWritten)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterStartDoc"));
			}
			XamlDocumentStartNode xamlDocumentNode = new XamlDocumentStartNode(0, 0, this._depth);
			this._bamlRecordWriter.WriteDocumentStart(xamlDocumentNode);
			this._startDocumentWritten = true;
			this.Push(BamlRecordType.DocumentStart);
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x001FFF48 File Offset: 0x001FEF48
		public void WriteEndDocument()
		{
			this.VerifyEndTagState(BamlRecordType.DocumentStart, BamlRecordType.DocumentEnd);
			XamlDocumentEndNode xamlDocumentEndNode = new XamlDocumentEndNode(0, 0, this._depth);
			this._bamlRecordWriter.WriteDocumentEnd(xamlDocumentEndNode);
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x001FFF77 File Offset: 0x001FEF77
		public void WriteConnectionId(int connectionId)
		{
			this.VerifyWriteState();
			this._bamlRecordWriter.WriteConnectionId(connectionId);
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x001FFF8C File Offset: 0x001FEF8C
		public void WriteStartElement(string assemblyName, string typeFullName, bool isInjected, bool useTypeConverter)
		{
			this.VerifyWriteState();
			this._dpProperty = null;
			this._parserContext.PushScope();
			this.ProcessMarkupExtensionNodes();
			Type type = this.GetType(assemblyName, typeFullName);
			Type xamlSerializerForType = this._xamlTypeMapper.GetXamlSerializerForType(type);
			this.Push(BamlRecordType.ElementStart, type);
			int lineNumber = 0;
			int linePosition = 0;
			int depth = this._depth;
			this._depth = depth + 1;
			XamlElementStartNode xamlElementStartNode = new XamlElementStartNode(lineNumber, linePosition, depth, assemblyName, typeFullName, type, xamlSerializerForType);
			xamlElementStartNode.IsInjected = isInjected;
			xamlElementStartNode.CreateUsingTypeConverter = useTypeConverter;
			this._bamlRecordWriter.WriteElementStart(xamlElementStartNode);
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x00200010 File Offset: 0x001FF010
		public void WriteEndElement()
		{
			this.VerifyEndTagState(BamlRecordType.ElementStart, BamlRecordType.ElementEnd);
			this.ProcessMarkupExtensionNodes();
			int lineNumber = 0;
			int linePosition = 0;
			int depth = this._depth - 1;
			this._depth = depth;
			XamlElementEndNode xamlElementEndNode = new XamlElementEndNode(lineNumber, linePosition, depth);
			this._bamlRecordWriter.WriteElementEnd(xamlElementEndNode);
			this._parserContext.PopScope();
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x0020005C File Offset: 0x001FF05C
		public void WriteStartConstructor()
		{
			this.VerifyWriteState();
			this.Push(BamlRecordType.ConstructorParametersStart);
			int lineNumber = 0;
			int linePosition = 0;
			int depth = this._depth - 1;
			this._depth = depth;
			XamlConstructorParametersStartNode xamlConstructorParametersStartNode = new XamlConstructorParametersStartNode(lineNumber, linePosition, depth);
			this._bamlRecordWriter.WriteConstructorParametersStart(xamlConstructorParametersStartNode);
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x0020009C File Offset: 0x001FF09C
		public void WriteEndConstructor()
		{
			this.VerifyEndTagState(BamlRecordType.ConstructorParametersStart, BamlRecordType.ConstructorParametersEnd);
			int lineNumber = 0;
			int linePosition = 0;
			int depth = this._depth - 1;
			this._depth = depth;
			XamlConstructorParametersEndNode xamlConstructorParametersEndNode = new XamlConstructorParametersEndNode(lineNumber, linePosition, depth);
			this._bamlRecordWriter.WriteConstructorParametersEnd(xamlConstructorParametersEndNode);
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x002000D8 File Offset: 0x001FF0D8
		public void WriteProperty(string assemblyName, string ownerTypeFullName, string propName, string value, BamlAttributeUsage propUsage)
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.PeekRecordType();
			if (bamlRecordType != BamlRecordType.ElementStart)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterNoInElement", new object[]
				{
					"WriteProperty",
					bamlRecordType.ToString()
				}));
			}
			object obj;
			Type declaringType;
			this.GetDpOrPi(assemblyName, ownerTypeFullName, propName, out obj, out declaringType);
			AttributeData attributeData = this._extensionParser.IsMarkupExtensionAttribute(declaringType, propName, ref value, 0, 0, 0, obj);
			if (attributeData == null)
			{
				XamlPropertyNode xamlPropertyNode = new XamlPropertyNode(0, 0, this._depth, obj, assemblyName, ownerTypeFullName, propName, value, propUsage, false);
				if (XamlTypeMapper.GetPropertyType(obj) == typeof(DependencyProperty))
				{
					Type declaringType2 = null;
					this._dpProperty = XamlTypeMapper.ParsePropertyName(this._parserContext, value, ref declaringType2);
					if (this._bamlRecordWriter != null && this._dpProperty != null)
					{
						short valueId;
						short attributeOrTypeId = this._parserContext.MapTable.GetAttributeOrTypeId(this._bamlRecordWriter.BinaryWriter, declaringType2, this._dpProperty.Name, out valueId);
						if (attributeOrTypeId < 0)
						{
							xamlPropertyNode.ValueId = attributeOrTypeId;
							xamlPropertyNode.MemberName = null;
						}
						else
						{
							xamlPropertyNode.ValueId = valueId;
							xamlPropertyNode.MemberName = this._dpProperty.Name;
						}
					}
				}
				else if (this._dpProperty != null)
				{
					xamlPropertyNode.ValuePropertyType = this._dpProperty.PropertyType;
					xamlPropertyNode.ValuePropertyMember = this._dpProperty;
					xamlPropertyNode.ValuePropertyName = this._dpProperty.Name;
					xamlPropertyNode.ValueDeclaringType = this._dpProperty.OwnerType;
					string fullName = this._dpProperty.OwnerType.Assembly.FullName;
					this._dpProperty = null;
				}
				this._bamlRecordWriter.WriteProperty(xamlPropertyNode);
				return;
			}
			if (!attributeData.IsSimple)
			{
				this._extensionParser.CompileAttribute(this._markupExtensionNodes, attributeData);
				return;
			}
			if (attributeData.IsTypeExtension)
			{
				Type typeFromBaseString = this._xamlTypeMapper.GetTypeFromBaseString(attributeData.Args, this._parserContext, true);
				XamlPropertyWithTypeNode xamlPropertyWithType = new XamlPropertyWithTypeNode(0, 0, this._depth, obj, assemblyName, ownerTypeFullName, propName, typeFromBaseString.FullName, typeFromBaseString.Assembly.FullName, typeFromBaseString, string.Empty, string.Empty);
				this._bamlRecordWriter.WritePropertyWithType(xamlPropertyWithType);
				return;
			}
			XamlPropertyWithExtensionNode xamlPropertyNode2 = new XamlPropertyWithExtensionNode(0, 0, this._depth, obj, assemblyName, ownerTypeFullName, propName, attributeData.Args, attributeData.ExtensionTypeId, attributeData.IsValueNestedExtension, attributeData.IsValueTypeExtension);
			this._bamlRecordWriter.WritePropertyWithExtension(xamlPropertyNode2);
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x00200340 File Offset: 0x001FF340
		public void WriteContentProperty(string assemblyName, string ownerTypeFullName, string propName)
		{
			object propertyMember;
			Type type;
			this.GetDpOrPi(assemblyName, ownerTypeFullName, propName, out propertyMember, out type);
			XamlContentPropertyNode xamlContentPropertyNode = new XamlContentPropertyNode(0, 0, this._depth, propertyMember, assemblyName, ownerTypeFullName, propName);
			this._bamlRecordWriter.WriteContentProperty(xamlContentPropertyNode);
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x00200378 File Offset: 0x001FF378
		public void WriteXmlnsProperty(string localName, string xmlNamespace)
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.PeekRecordType();
			if (bamlRecordType != BamlRecordType.ElementStart && bamlRecordType != BamlRecordType.PropertyComplexStart && bamlRecordType != BamlRecordType.PropertyArrayStart && bamlRecordType != BamlRecordType.PropertyIListStart && bamlRecordType != BamlRecordType.PropertyIDictionaryStart)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterBadXmlns", new object[]
				{
					"WriteXmlnsProperty",
					bamlRecordType.ToString()
				}));
			}
			XamlXmlnsPropertyNode xamlXmlnsPropertyNode = new XamlXmlnsPropertyNode(0, 0, this._depth, localName, xmlNamespace);
			this._bamlRecordWriter.WriteNamespacePrefix(xamlXmlnsPropertyNode);
			this._parserContext.XmlnsDictionary[localName] = xmlNamespace;
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x00200408 File Offset: 0x001FF408
		public void WriteDefAttribute(string name, string value)
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.PeekRecordType();
			if (bamlRecordType != BamlRecordType.ElementStart && name != "Uid")
			{
				throw new InvalidOperationException(SR.Get("BamlWriterNoInElement", new object[]
				{
					"WriteDefAttribute",
					bamlRecordType.ToString()
				}));
			}
			if (name == "Key")
			{
				DefAttributeData defAttributeData = this._extensionParser.IsMarkupExtensionDefAttribute(this.PeekElementType(), ref value, 0, 0, 0);
				if (defAttributeData != null)
				{
					if (name != "Key")
					{
						defAttributeData.IsSimple = false;
					}
					if (defAttributeData.IsSimple)
					{
						int num = defAttributeData.Args.IndexOf(':');
						string prefix = string.Empty;
						string localName = defAttributeData.Args;
						if (num > 0)
						{
							prefix = defAttributeData.Args.Substring(0, num);
							localName = defAttributeData.Args.Substring(num + 1);
						}
						string namespaceURI = this._parserContext.XmlnsDictionary[prefix];
						string empty = string.Empty;
						string empty2 = string.Empty;
						Type type = null;
						Type type2 = null;
						if (((IParserHelper)this).GetElementType(false, localName, namespaceURI, ref empty, ref empty2, ref type, ref type2))
						{
							XamlDefAttributeKeyTypeNode xamlDefNode = new XamlDefAttributeKeyTypeNode(0, 0, this._depth, empty2, type.Assembly.FullName, type);
							this._bamlRecordWriter.WriteDefAttributeKeyType(xamlDefNode);
						}
						else
						{
							defAttributeData.IsSimple = false;
							DefAttributeData defAttributeData2 = defAttributeData;
							defAttributeData2.Args += "}";
						}
					}
					if (!defAttributeData.IsSimple)
					{
						this._extensionParser.CompileDictionaryKey(this._markupExtensionNodes, defAttributeData);
					}
					return;
				}
			}
			XamlDefAttributeNode xamlDefNode2 = new XamlDefAttributeNode(0, 0, this._depth, name, value);
			this._bamlRecordWriter.WriteDefAttribute(xamlDefNode2);
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x002005AC File Offset: 0x001FF5AC
		public void WritePresentationOptionsAttribute(string name, string value)
		{
			this.VerifyWriteState();
			XamlPresentationOptionsAttributeNode xamlPresentationOptionsNode = new XamlPresentationOptionsAttributeNode(0, 0, this._depth, name, value);
			this._bamlRecordWriter.WritePresentationOptionsAttribute(xamlPresentationOptionsNode);
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x002005DC File Offset: 0x001FF5DC
		public void WriteStartComplexProperty(string assemblyName, string ownerTypeFullName, string propName)
		{
			this.VerifyWriteState();
			this._parserContext.PushScope();
			this.ProcessMarkupExtensionNodes();
			Type type = null;
			bool propertyCanWrite = true;
			object obj;
			Type type2;
			this.GetDpOrPi(assemblyName, ownerTypeFullName, propName, out obj, out type2);
			if (obj == null)
			{
				MethodInfo mi = this.GetMi(assemblyName, ownerTypeFullName, propName, out type2);
				if (mi != null)
				{
					XamlTypeMapper.GetPropertyType(mi, out type, out propertyCanWrite);
				}
			}
			else
			{
				type = XamlTypeMapper.GetPropertyType(obj);
				PropertyInfo propertyInfo = obj as PropertyInfo;
				if (propertyInfo != null)
				{
					propertyCanWrite = propertyInfo.CanWrite;
				}
				else
				{
					DependencyProperty dependencyProperty = obj as DependencyProperty;
					if (dependencyProperty != null)
					{
						propertyCanWrite = !dependencyProperty.ReadOnly;
					}
				}
			}
			int depth;
			if (type == null)
			{
				this.Push(BamlRecordType.PropertyComplexStart);
				int lineNumber = 0;
				int linePosition = 0;
				depth = this._depth;
				this._depth = depth + 1;
				XamlPropertyComplexStartNode xamlComplexPropertyNode = new XamlPropertyComplexStartNode(lineNumber, linePosition, depth, null, assemblyName, ownerTypeFullName, propName);
				this._bamlRecordWriter.WritePropertyComplexStart(xamlComplexPropertyNode);
				return;
			}
			BamlRecordType propertyStartRecordType = BamlRecordManager.GetPropertyStartRecordType(type, propertyCanWrite);
			this.Push(propertyStartRecordType);
			switch (propertyStartRecordType)
			{
			case BamlRecordType.PropertyArrayStart:
			{
				int lineNumber2 = 0;
				int linePosition2 = 0;
				depth = this._depth;
				this._depth = depth + 1;
				XamlPropertyArrayStartNode xamlPropertyArrayStartNode = new XamlPropertyArrayStartNode(lineNumber2, linePosition2, depth, obj, assemblyName, ownerTypeFullName, propName);
				this._bamlRecordWriter.WritePropertyArrayStart(xamlPropertyArrayStartNode);
				return;
			}
			case BamlRecordType.PropertyIListStart:
			{
				int lineNumber3 = 0;
				int linePosition3 = 0;
				depth = this._depth;
				this._depth = depth + 1;
				XamlPropertyIListStartNode xamlPropertyIListStart = new XamlPropertyIListStartNode(lineNumber3, linePosition3, depth, obj, assemblyName, ownerTypeFullName, propName);
				this._bamlRecordWriter.WritePropertyIListStart(xamlPropertyIListStart);
				return;
			}
			case BamlRecordType.PropertyIDictionaryStart:
			{
				int lineNumber4 = 0;
				int linePosition4 = 0;
				depth = this._depth;
				this._depth = depth + 1;
				XamlPropertyIDictionaryStartNode xamlPropertyIDictionaryStartNode = new XamlPropertyIDictionaryStartNode(lineNumber4, linePosition4, depth, obj, assemblyName, ownerTypeFullName, propName);
				this._bamlRecordWriter.WritePropertyIDictionaryStart(xamlPropertyIDictionaryStartNode);
				return;
			}
			}
			int lineNumber5 = 0;
			int linePosition5 = 0;
			depth = this._depth;
			this._depth = depth + 1;
			XamlPropertyComplexStartNode xamlComplexPropertyNode2 = new XamlPropertyComplexStartNode(lineNumber5, linePosition5, depth, obj, assemblyName, ownerTypeFullName, propName);
			this._bamlRecordWriter.WritePropertyComplexStart(xamlComplexPropertyNode2);
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x002007A0 File Offset: 0x001FF7A0
		public void WriteEndComplexProperty()
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.Pop();
			switch (bamlRecordType)
			{
			case BamlRecordType.PropertyComplexStart:
			{
				int lineNumber = 0;
				int linePosition = 0;
				int depth = this._depth - 1;
				this._depth = depth;
				XamlPropertyComplexEndNode xamlPropertyComplexEnd = new XamlPropertyComplexEndNode(lineNumber, linePosition, depth);
				this._bamlRecordWriter.WritePropertyComplexEnd(xamlPropertyComplexEnd);
				goto IL_11F;
			}
			case BamlRecordType.PropertyArrayStart:
			{
				int lineNumber2 = 0;
				int linePosition2 = 0;
				int depth = this._depth - 1;
				this._depth = depth;
				XamlPropertyArrayEndNode xamlPropertyArrayEndNode = new XamlPropertyArrayEndNode(lineNumber2, linePosition2, depth);
				this._bamlRecordWriter.WritePropertyArrayEnd(xamlPropertyArrayEndNode);
				goto IL_11F;
			}
			case BamlRecordType.PropertyIListStart:
			{
				int lineNumber3 = 0;
				int linePosition3 = 0;
				int depth = this._depth - 1;
				this._depth = depth;
				XamlPropertyIListEndNode xamlPropertyIListEndNode = new XamlPropertyIListEndNode(lineNumber3, linePosition3, depth);
				this._bamlRecordWriter.WritePropertyIListEnd(xamlPropertyIListEndNode);
				goto IL_11F;
			}
			case BamlRecordType.PropertyIDictionaryStart:
			{
				int lineNumber4 = 0;
				int linePosition4 = 0;
				int depth = this._depth - 1;
				this._depth = depth;
				XamlPropertyIDictionaryEndNode xamlPropertyIDictionaryEndNode = new XamlPropertyIDictionaryEndNode(lineNumber4, linePosition4, depth);
				this._bamlRecordWriter.WritePropertyIDictionaryEnd(xamlPropertyIDictionaryEndNode);
				goto IL_11F;
			}
			}
			throw new InvalidOperationException(SR.Get("BamlWriterBadScope", new object[]
			{
				bamlRecordType.ToString(),
				BamlRecordType.PropertyComplexEnd.ToString()
			}));
			IL_11F:
			this._parserContext.PopScope();
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x002008D8 File Offset: 0x001FF8D8
		public void WriteLiteralContent(string contents)
		{
			this.VerifyWriteState();
			this.ProcessMarkupExtensionNodes();
			XamlLiteralContentNode xamlLiteralContentNode = new XamlLiteralContentNode(0, 0, this._depth, contents);
			this._bamlRecordWriter.WriteLiteralContent(xamlLiteralContentNode);
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x0020090C File Offset: 0x001FF90C
		public void WritePIMapping(string xmlNamespace, string clrNamespace, string assemblyName)
		{
			this.VerifyWriteState();
			this.ProcessMarkupExtensionNodes();
			XamlPIMappingNode xamlPIMappingNode = new XamlPIMappingNode(0, 0, this._depth, xmlNamespace, clrNamespace, assemblyName);
			if (!this._xamlTypeMapper.PITable.Contains(xmlNamespace))
			{
				ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = new ClrNamespaceAssemblyPair(clrNamespace, assemblyName);
				this._xamlTypeMapper.PITable.Add(xmlNamespace, clrNamespaceAssemblyPair);
			}
			this._bamlRecordWriter.WritePIMapping(xamlPIMappingNode);
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00200978 File Offset: 0x001FF978
		public void WriteText(string textContent, string typeConverterAssemblyName, string typeConverterName)
		{
			this.VerifyWriteState();
			this.ProcessMarkupExtensionNodes();
			Type converterType = null;
			if (!string.IsNullOrEmpty(typeConverterName))
			{
				converterType = this.GetType(typeConverterAssemblyName, typeConverterName);
			}
			XamlTextNode xamlTextNode = new XamlTextNode(0, 0, this._depth, textContent, converterType);
			this._bamlRecordWriter.WriteText(xamlTextNode);
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x002009C0 File Offset: 0x001FF9C0
		public void WriteRoutedEvent(string assemblyName, string ownerTypeFullName, string eventIdName, string handlerName)
		{
			throw new NotSupportedException(SR.Get("ParserBamlEvent", new object[]
			{
				eventIdName
			}));
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x002009DB File Offset: 0x001FF9DB
		public void WriteEvent(string eventName, string handlerName)
		{
			throw new NotSupportedException(SR.Get("ParserBamlEvent", new object[]
			{
				eventName
			}));
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x002009F8 File Offset: 0x001FF9F8
		private void ProcessMarkupExtensionNodes()
		{
			int i = 0;
			while (i < this._markupExtensionNodes.Count)
			{
				XamlNode xamlNode = this._markupExtensionNodes[i] as XamlNode;
				XamlNodeType tokenType = xamlNode.TokenType;
				switch (tokenType)
				{
				case XamlNodeType.ElementStart:
					this._bamlRecordWriter.WriteElementStart((XamlElementStartNode)xamlNode);
					break;
				case XamlNodeType.ElementEnd:
					this._bamlRecordWriter.WriteElementEnd((XamlElementEndNode)xamlNode);
					break;
				case XamlNodeType.Property:
					this._bamlRecordWriter.WriteProperty((XamlPropertyNode)xamlNode);
					break;
				case XamlNodeType.PropertyComplexStart:
					this._bamlRecordWriter.WritePropertyComplexStart((XamlPropertyComplexStartNode)xamlNode);
					break;
				case XamlNodeType.PropertyComplexEnd:
					this._bamlRecordWriter.WritePropertyComplexEnd((XamlPropertyComplexEndNode)xamlNode);
					break;
				case XamlNodeType.PropertyArrayStart:
				case XamlNodeType.PropertyArrayEnd:
				case XamlNodeType.PropertyIListStart:
				case XamlNodeType.PropertyIListEnd:
				case XamlNodeType.PropertyIDictionaryStart:
				case XamlNodeType.PropertyIDictionaryEnd:
				case XamlNodeType.LiteralContent:
					goto IL_1A2;
				case XamlNodeType.PropertyWithExtension:
					this._bamlRecordWriter.WritePropertyWithExtension((XamlPropertyWithExtensionNode)xamlNode);
					break;
				case XamlNodeType.PropertyWithType:
					this._bamlRecordWriter.WritePropertyWithType((XamlPropertyWithTypeNode)xamlNode);
					break;
				case XamlNodeType.Text:
					this._bamlRecordWriter.WriteText((XamlTextNode)xamlNode);
					break;
				default:
					switch (tokenType)
					{
					case XamlNodeType.EndAttributes:
						this._bamlRecordWriter.WriteEndAttributes((XamlEndAttributesNode)xamlNode);
						break;
					case XamlNodeType.PIMapping:
					case XamlNodeType.UnknownTagStart:
					case XamlNodeType.UnknownTagEnd:
					case XamlNodeType.UnknownAttribute:
						goto IL_1A2;
					case XamlNodeType.KeyElementStart:
						this._bamlRecordWriter.WriteKeyElementStart((XamlKeyElementStartNode)xamlNode);
						break;
					case XamlNodeType.KeyElementEnd:
						this._bamlRecordWriter.WriteKeyElementEnd((XamlKeyElementEndNode)xamlNode);
						break;
					case XamlNodeType.ConstructorParametersStart:
						this._bamlRecordWriter.WriteConstructorParametersStart((XamlConstructorParametersStartNode)xamlNode);
						break;
					case XamlNodeType.ConstructorParametersEnd:
						this._bamlRecordWriter.WriteConstructorParametersEnd((XamlConstructorParametersEndNode)xamlNode);
						break;
					default:
						goto IL_1A2;
					}
					break;
				}
				i++;
				continue;
				IL_1A2:
				throw new InvalidOperationException(SR.Get("BamlWriterUnknownMarkupExtension"));
			}
			this._markupExtensionNodes.Clear();
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x00200BD7 File Offset: 0x001FFBD7
		private void VerifyWriteState()
		{
			if (this._closed)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterClosed"));
			}
			if (!this._startDocumentWritten)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterStartDoc"));
			}
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00200C0C File Offset: 0x001FFC0C
		private void VerifyEndTagState(BamlRecordType expectedStartTag, BamlRecordType endTagBeingWritten)
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.Pop();
			if (bamlRecordType != expectedStartTag)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterBadScope", new object[]
				{
					bamlRecordType.ToString(),
					endTagBeingWritten.ToString()
				}));
			}
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x00200C60 File Offset: 0x001FFC60
		private Assembly GetAssembly(string assemblyName)
		{
			Assembly assembly = this._assemblies[assemblyName] as Assembly;
			if (assembly == null)
			{
				assembly = ReflectionHelper.LoadAssembly(assemblyName, null);
				if (assembly == null)
				{
					throw new ArgumentException(SR.Get("BamlWriterBadAssembly", new object[]
					{
						assemblyName
					}));
				}
				this._assemblies[assemblyName] = assembly;
			}
			return assembly;
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x00200CC1 File Offset: 0x001FFCC1
		private Type GetType(string assemblyName, string typeFullName)
		{
			return this.GetAssembly(assemblyName).GetType(typeFullName);
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x00200CD0 File Offset: 0x001FFCD0
		private object GetDpOrPi(Type ownerType, string propName)
		{
			object obj = null;
			if (ownerType != null)
			{
				obj = DependencyProperty.FromName(propName, ownerType);
				if (obj == null)
				{
					PropertyInfo propertyInfo = null;
					foreach (PropertyInfo propertyInfo2 in ownerType.GetMember(propName, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public))
					{
						if (propertyInfo2.GetIndexParameters().Length == 0 && (propertyInfo == null || propertyInfo.DeclaringType.IsAssignableFrom(propertyInfo2.DeclaringType)))
						{
							propertyInfo = propertyInfo2;
						}
					}
					obj = propertyInfo;
				}
			}
			return obj;
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x00200D46 File Offset: 0x001FFD46
		private void GetDpOrPi(string assemblyName, string ownerTypeFullName, string propName, out object dpOrPi, out Type ownerType)
		{
			if (assemblyName == string.Empty || ownerTypeFullName == string.Empty)
			{
				dpOrPi = null;
				ownerType = null;
				return;
			}
			ownerType = this.GetType(assemblyName, ownerTypeFullName);
			dpOrPi = this.GetDpOrPi(ownerType, propName);
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x00200D84 File Offset: 0x001FFD84
		private MethodInfo GetMi(Type ownerType, string propName)
		{
			MethodInfo methodInfo = ownerType.GetMethod("Set" + propName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (methodInfo != null && methodInfo.GetParameters().Length != 2)
			{
				methodInfo = null;
			}
			if (methodInfo == null)
			{
				methodInfo = ownerType.GetMethod("Get" + propName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				if (methodInfo != null && methodInfo.GetParameters().Length != 1)
				{
					methodInfo = null;
				}
			}
			return methodInfo;
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x00200DF4 File Offset: 0x001FFDF4
		private MethodInfo GetMi(string assemblyName, string ownerTypeFullName, string propName, out Type ownerType)
		{
			MethodInfo result;
			if (assemblyName == string.Empty || ownerTypeFullName == string.Empty)
			{
				result = null;
				ownerType = null;
			}
			else
			{
				ownerType = this.GetType(assemblyName, ownerTypeFullName);
				result = this.GetMi(ownerType, propName);
			}
			return result;
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x00200E3C File Offset: 0x001FFE3C
		private void Push(BamlRecordType recordType)
		{
			this.CheckEndAttributes();
			this._nodeTypeStack.Push(new BamlWriter.WriteStackNode(recordType));
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x00200E55 File Offset: 0x001FFE55
		private void Push(BamlRecordType recordType, Type elementType)
		{
			this.CheckEndAttributes();
			this._nodeTypeStack.Push(new BamlWriter.WriteStackNode(recordType, elementType));
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x00200E6F File Offset: 0x001FFE6F
		private BamlRecordType Pop()
		{
			return (this._nodeTypeStack.Pop() as BamlWriter.WriteStackNode).RecordType;
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x00200E86 File Offset: 0x001FFE86
		private BamlRecordType PeekRecordType()
		{
			return (this._nodeTypeStack.Peek() as BamlWriter.WriteStackNode).RecordType;
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00200E9D File Offset: 0x001FFE9D
		private Type PeekElementType()
		{
			return (this._nodeTypeStack.Peek() as BamlWriter.WriteStackNode).ElementType;
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x00200EB4 File Offset: 0x001FFEB4
		private void CheckEndAttributes()
		{
			if (this._nodeTypeStack.Count > 0)
			{
				BamlWriter.WriteStackNode writeStackNode = this._nodeTypeStack.Peek() as BamlWriter.WriteStackNode;
				if (!writeStackNode.EndAttributesReached && writeStackNode.RecordType == BamlRecordType.ElementStart)
				{
					XamlEndAttributesNode xamlEndAttributesNode = new XamlEndAttributesNode(0, 0, this._depth, false);
					this._bamlRecordWriter.WriteEndAttributes(xamlEndAttributesNode);
				}
				writeStackNode.EndAttributesReached = true;
			}
		}

		// Token: 0x04001F1C RID: 7964
		private BamlRecordWriter _bamlRecordWriter;

		// Token: 0x04001F1D RID: 7965
		private bool _startDocumentWritten;

		// Token: 0x04001F1E RID: 7966
		private int _depth;

		// Token: 0x04001F1F RID: 7967
		private bool _closed;

		// Token: 0x04001F20 RID: 7968
		private DependencyProperty _dpProperty;

		// Token: 0x04001F21 RID: 7969
		private ParserStack _nodeTypeStack;

		// Token: 0x04001F22 RID: 7970
		private Hashtable _assemblies;

		// Token: 0x04001F23 RID: 7971
		private XamlTypeMapper _xamlTypeMapper;

		// Token: 0x04001F24 RID: 7972
		private ParserContext _parserContext;

		// Token: 0x04001F25 RID: 7973
		private MarkupExtensionParser _extensionParser;

		// Token: 0x04001F26 RID: 7974
		private ArrayList _markupExtensionNodes;

		// Token: 0x02000AF5 RID: 2805
		private class WriteStackNode
		{
			// Token: 0x06008B87 RID: 35719 RVA: 0x00339F18 File Offset: 0x00338F18
			public WriteStackNode(BamlRecordType recordType)
			{
				this._recordType = recordType;
				this._endAttributesReached = false;
			}

			// Token: 0x06008B88 RID: 35720 RVA: 0x00339F2E File Offset: 0x00338F2E
			public WriteStackNode(BamlRecordType recordType, Type elementType) : this(recordType)
			{
				this._elementType = elementType;
			}

			// Token: 0x17001E95 RID: 7829
			// (get) Token: 0x06008B89 RID: 35721 RVA: 0x00339F3E File Offset: 0x00338F3E
			public BamlRecordType RecordType
			{
				get
				{
					return this._recordType;
				}
			}

			// Token: 0x17001E96 RID: 7830
			// (get) Token: 0x06008B8A RID: 35722 RVA: 0x00339F46 File Offset: 0x00338F46
			// (set) Token: 0x06008B8B RID: 35723 RVA: 0x00339F4E File Offset: 0x00338F4E
			public bool EndAttributesReached
			{
				get
				{
					return this._endAttributesReached;
				}
				set
				{
					this._endAttributesReached = value;
				}
			}

			// Token: 0x17001E97 RID: 7831
			// (get) Token: 0x06008B8C RID: 35724 RVA: 0x00339F57 File Offset: 0x00338F57
			public Type ElementType
			{
				get
				{
					return this._elementType;
				}
			}

			// Token: 0x04004744 RID: 18244
			private bool _endAttributesReached;

			// Token: 0x04004745 RID: 18245
			private BamlRecordType _recordType;

			// Token: 0x04004746 RID: 18246
			private Type _elementType;
		}
	}
}
