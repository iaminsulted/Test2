using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using MS.Internal.Xaml.Parser;

namespace System.Windows.Markup
{
	// Token: 0x020004CA RID: 1226
	internal class MarkupExtensionParser
	{
		// Token: 0x06003EAD RID: 16045 RVA: 0x0020DF96 File Offset: 0x0020CF96
		internal MarkupExtensionParser(IParserHelper parserHelper, ParserContext parserContext)
		{
			this._parserHelper = parserHelper;
			this._parserContext = parserContext;
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x0020DFAC File Offset: 0x0020CFAC
		internal AttributeData IsMarkupExtensionAttribute(Type declaringType, string propIdName, ref string attrValue, int lineNumber, int linePosition, int depth, object info)
		{
			string typename;
			string args;
			if (!MarkupExtensionParser.GetMarkupExtensionTypeAndArgs(ref attrValue, out typename, out args))
			{
				return null;
			}
			return this.FillAttributeData(declaringType, propIdName, typename, args, attrValue, lineNumber, linePosition, depth, info);
		}

		// Token: 0x06003EAF RID: 16047 RVA: 0x0020DFDC File Offset: 0x0020CFDC
		internal DefAttributeData IsMarkupExtensionDefAttribute(Type declaringType, ref string attrValue, int lineNumber, int linePosition, int depth)
		{
			string typename;
			string args;
			if (!MarkupExtensionParser.GetMarkupExtensionTypeAndArgs(ref attrValue, out typename, out args))
			{
				return null;
			}
			return this.FillDefAttributeData(declaringType, typename, args, attrValue, lineNumber, linePosition, depth);
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x0020E007 File Offset: 0x0020D007
		internal static bool LooksLikeAMarkupExtension(string attrValue)
		{
			return attrValue.Length >= 2 && attrValue[0] == '{' && attrValue[1] != '}';
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x0020E030 File Offset: 0x0020D030
		internal static string AddEscapeToLiteralString(string literalString)
		{
			string text = literalString;
			if (!string.IsNullOrEmpty(text) && text[0] == '{')
			{
				text = "{}" + text;
			}
			return text;
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x0020E060 File Offset: 0x0020D060
		private KnownElements GetKnownExtensionFromType(Type extensionType, out string propName)
		{
			if (KnownTypes.Types[691] == extensionType)
			{
				propName = "TypeName";
				return KnownElements.TypeExtension;
			}
			if (KnownTypes.Types[602] == extensionType)
			{
				propName = "Member";
				return KnownElements.StaticExtension;
			}
			if (KnownTypes.Types[634] == extensionType)
			{
				propName = "Property";
				return KnownElements.TemplateBindingExtension;
			}
			if (KnownTypes.Types[189] == extensionType)
			{
				propName = "ResourceKey";
				return KnownElements.DynamicResourceExtension;
			}
			if (KnownTypes.Types[603] == extensionType)
			{
				propName = "ResourceKey";
				return KnownElements.StaticResourceExtension;
			}
			propName = string.Empty;
			return KnownElements.UnknownElement;
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x0020E129 File Offset: 0x0020D129
		private bool IsSimpleTypeExtensionArgs(Type extensionType, int lineNumber, int linePosition, ref string args)
		{
			return KnownTypes.Types[691] == extensionType && this.IsSimpleExtensionArgs(lineNumber, linePosition, "TypeName", ref args, extensionType);
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x0020E154 File Offset: 0x0020D154
		private bool IsSimpleExtension(Type extensionType, int lineNumber, int linePosition, int depth, out short extensionTypeId, out bool isValueNestedExtension, out bool isValueTypeExtension, ref string args)
		{
			bool flag = false;
			extensionTypeId = 0;
			isValueNestedExtension = false;
			isValueTypeExtension = false;
			string propName;
			KnownElements knownExtensionFromType = this.GetKnownExtensionFromType(extensionType, out propName);
			if (knownExtensionFromType != KnownElements.UnknownElement)
			{
				flag = this.IsSimpleExtensionArgs(lineNumber, linePosition, propName, ref args, extensionType);
			}
			if (flag)
			{
				if ((knownExtensionFromType == KnownElements.DynamicResourceExtension || knownExtensionFromType == KnownElements.StaticResourceExtension) && MarkupExtensionParser.LooksLikeAMarkupExtension(args))
				{
					AttributeData attributeData = this.IsMarkupExtensionAttribute(extensionType, null, ref args, lineNumber, linePosition, depth, null);
					isValueTypeExtension = attributeData.IsTypeExtension;
					flag = (isValueTypeExtension || attributeData.IsStaticExtension);
					isValueNestedExtension = flag;
					if (flag)
					{
						args = attributeData.Args;
					}
					else
					{
						args += "}";
					}
				}
				if (flag)
				{
					extensionTypeId = (short)knownExtensionFromType;
				}
			}
			return flag;
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x0020E1F8 File Offset: 0x0020D1F8
		private bool IsSimpleExtensionArgs(int lineNumber, int linePosition, string propName, ref string args, Type targetType)
		{
			ArrayList arrayList = this.TokenizeAttributes(args, lineNumber, linePosition, targetType);
			if (arrayList == null)
			{
				return false;
			}
			if (arrayList.Count == 1)
			{
				args = (string)arrayList[0];
				return true;
			}
			if (arrayList.Count == 3 && (string)arrayList[0] == propName)
			{
				args = (string)arrayList[2];
				return true;
			}
			return false;
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0020E264 File Offset: 0x0020D264
		internal static bool GetMarkupExtensionTypeAndArgs(ref string attrValue, out string typeName, out string args)
		{
			int length = attrValue.Length;
			typeName = string.Empty;
			args = string.Empty;
			if (length < 1 || attrValue[0] != '{')
			{
				return false;
			}
			bool flag = false;
			StringBuilder stringBuilder = null;
			int i;
			for (i = 1; i < length; i++)
			{
				if (char.IsWhiteSpace(attrValue[i]))
				{
					if (stringBuilder != null)
					{
						break;
					}
				}
				else if (stringBuilder == null)
				{
					if (!flag && attrValue[i] == '\\')
					{
						flag = true;
					}
					else if (attrValue[i] == '}')
					{
						if (i == 1)
						{
							attrValue = attrValue.Substring(2);
							return false;
						}
					}
					else
					{
						stringBuilder = new StringBuilder(length - i);
						stringBuilder.Append(attrValue[i]);
						flag = false;
					}
				}
				else if (!flag && attrValue[i] == '\\')
				{
					flag = true;
				}
				else
				{
					if (attrValue[i] == '}')
					{
						break;
					}
					stringBuilder.Append(attrValue[i]);
					flag = false;
				}
			}
			if (stringBuilder != null)
			{
				typeName = stringBuilder.ToString();
			}
			if (i < length - 1)
			{
				args = attrValue.Substring(i, length - i);
			}
			else if (attrValue[length - 1] == '}')
			{
				args = "}";
			}
			return true;
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x0020E380 File Offset: 0x0020D380
		private DefAttributeData FillDefAttributeData(Type declaringType, string typename, string args, string attributeValue, int lineNumber, int linePosition, int depth)
		{
			bool isSimple = false;
			string targetNamespaceUri;
			string targetAssemblyName;
			string targetFullName;
			Type type;
			Type type2;
			if (this.GetExtensionType(typename, attributeValue, lineNumber, linePosition, out targetNamespaceUri, out targetAssemblyName, out targetFullName, out type, out type2))
			{
				isSimple = this.IsSimpleTypeExtensionArgs(type, lineNumber, linePosition, ref args);
			}
			return new DefAttributeData(targetAssemblyName, targetFullName, type, args, declaringType, targetNamespaceUri, lineNumber, linePosition, depth, isSimple);
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x0020E3CC File Offset: 0x0020D3CC
		private AttributeData FillAttributeData(Type declaringType, string propIdName, string typename, string args, string attributeValue, int lineNumber, int linePosition, int depth, object info)
		{
			bool flag = false;
			short extensionTypeId = 0;
			bool isValueNestedExtension = false;
			bool isValueTypeExtension = false;
			string targetNamespaceUri;
			string targetAssemblyName;
			string targetFullName;
			Type type;
			Type serializerType;
			if (this.GetExtensionType(typename, attributeValue, lineNumber, linePosition, out targetNamespaceUri, out targetAssemblyName, out targetFullName, out type, out serializerType) && propIdName != string.Empty)
			{
				if (propIdName == null)
				{
					if (KnownTypes.Types[691] == type)
					{
						flag = this.IsSimpleExtensionArgs(lineNumber, linePosition, "TypeName", ref args, type);
						isValueNestedExtension = flag;
						isValueTypeExtension = flag;
						extensionTypeId = 691;
					}
					else if (KnownTypes.Types[602] == type)
					{
						flag = this.IsSimpleExtensionArgs(lineNumber, linePosition, "Member", ref args, type);
						isValueNestedExtension = flag;
						extensionTypeId = 602;
					}
				}
				else
				{
					propIdName = propIdName.Trim();
					flag = this.IsSimpleExtension(type, lineNumber, linePosition, depth, out extensionTypeId, out isValueNestedExtension, out isValueTypeExtension, ref args);
				}
			}
			return new AttributeData(targetAssemblyName, targetFullName, type, args, declaringType, propIdName, info, serializerType, lineNumber, linePosition, depth, targetNamespaceUri, extensionTypeId, isValueNestedExtension, isValueTypeExtension, flag);
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x0020E4C8 File Offset: 0x0020D4C8
		private bool GetExtensionType(string typename, string attributeValue, int lineNumber, int linePosition, out string namespaceURI, out string targetAssemblyName, out string targetFullName, out Type targetType, out Type serializerType)
		{
			targetAssemblyName = null;
			targetFullName = null;
			targetType = null;
			serializerType = null;
			string text = typename;
			string prefix = string.Empty;
			int num = typename.IndexOf(':');
			if (num >= 0)
			{
				prefix = typename.Substring(0, num);
				typename = typename.Substring(num + 1);
			}
			namespaceURI = this._parserHelper.LookupNamespace(prefix);
			bool elementType = this._parserHelper.GetElementType(true, typename, namespaceURI, ref targetAssemblyName, ref targetFullName, ref targetType, ref serializerType);
			if (elementType)
			{
				if (!KnownTypes.Types[381].IsAssignableFrom(targetType))
				{
					this.ThrowException("ParserNotMarkupExtension", attributeValue, typename, namespaceURI, lineNumber, linePosition);
				}
				return elementType;
			}
			if (this._parserHelper.CanResolveLocalAssemblies())
			{
				this.ThrowException("ParserNotMarkupExtension", attributeValue, typename, namespaceURI, lineNumber, linePosition);
				return elementType;
			}
			targetFullName = text;
			targetType = typeof(MarkupExtensionParser.UnknownMarkupExtension);
			return elementType;
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x0020E598 File Offset: 0x0020D598
		internal ArrayList CompileAttributes(ArrayList markupExtensionList, int startingDepth)
		{
			ArrayList arrayList = new ArrayList(markupExtensionList.Count * 5);
			for (int i = 0; i < markupExtensionList.Count; i++)
			{
				AttributeData data = (AttributeData)markupExtensionList[i];
				this.CompileAttribute(arrayList, data);
			}
			return arrayList;
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x0020E5DC File Offset: 0x0020D5DC
		internal void CompileAttribute(ArrayList xamlNodes, AttributeData data)
		{
			string fullName = data.DeclaringType.Assembly.FullName;
			string fullName2 = data.DeclaringType.FullName;
			Type propertyType;
			bool propertyCanWrite;
			XamlTypeMapper.GetPropertyType(data.Info, out propertyType, out propertyCanWrite);
			XamlNode value;
			XamlNode value2;
			switch (BamlRecordManager.GetPropertyStartRecordType(propertyType, propertyCanWrite))
			{
			case BamlRecordType.PropertyArrayStart:
				value = new XamlPropertyArrayStartNode(data.LineNumber, data.LinePosition, data.Depth, data.Info, fullName, fullName2, data.PropertyName);
				value2 = new XamlPropertyArrayEndNode(data.LineNumber, data.LinePosition, data.Depth);
				goto IL_164;
			case BamlRecordType.PropertyIListStart:
				value = new XamlPropertyIListStartNode(data.LineNumber, data.LinePosition, data.Depth, data.Info, fullName, fullName2, data.PropertyName);
				value2 = new XamlPropertyIListEndNode(data.LineNumber, data.LinePosition, data.Depth);
				goto IL_164;
			case BamlRecordType.PropertyIDictionaryStart:
				value = new XamlPropertyIDictionaryStartNode(data.LineNumber, data.LinePosition, data.Depth, data.Info, fullName, fullName2, data.PropertyName);
				value2 = new XamlPropertyIDictionaryEndNode(data.LineNumber, data.LinePosition, data.Depth);
				goto IL_164;
			}
			value = new XamlPropertyComplexStartNode(data.LineNumber, data.LinePosition, data.Depth, data.Info, fullName, fullName2, data.PropertyName);
			value2 = new XamlPropertyComplexEndNode(data.LineNumber, data.LinePosition, data.Depth);
			IL_164:
			xamlNodes.Add(value);
			this.CompileAttributeCore(xamlNodes, data);
			xamlNodes.Add(value2);
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x0020E768 File Offset: 0x0020D768
		internal void CompileAttributeCore(ArrayList xamlNodes, AttributeData data)
		{
			string text = null;
			string xmlNamespace = null;
			ArrayList arrayList = this.TokenizeAttributes(data.Args, data.LineNumber, data.LinePosition, data.TargetType);
			int num2;
			if (data.TargetType == typeof(MarkupExtensionParser.UnknownMarkupExtension))
			{
				text = data.TargetFullName;
				string prefix = string.Empty;
				int num = text.IndexOf(':');
				if (num >= 0)
				{
					prefix = text.Substring(0, num);
					text = text.Substring(num + 1);
				}
				xmlNamespace = this._parserHelper.LookupNamespace(prefix);
				int lineNumber = data.LineNumber;
				int linePosition = data.LinePosition;
				num2 = data.Depth + 1;
				data.Depth = num2;
				xamlNodes.Add(new XamlUnknownTagStartNode(lineNumber, linePosition, num2, xmlNamespace, text));
			}
			else
			{
				int lineNumber2 = data.LineNumber;
				int linePosition2 = data.LinePosition;
				num2 = data.Depth + 1;
				data.Depth = num2;
				xamlNodes.Add(new XamlElementStartNode(lineNumber2, linePosition2, num2, data.TargetAssemblyName, data.TargetFullName, data.TargetType, data.SerializerType));
			}
			xamlNodes.Add(new XamlEndAttributesNode(data.LineNumber, data.LinePosition, data.Depth, true));
			int listIndex = 0;
			if (arrayList != null && (arrayList.Count == 1 || (arrayList.Count > 1 && !(arrayList[1] is string) && (char)arrayList[1] == ',')))
			{
				this.WriteConstructorParams(xamlNodes, arrayList, data, ref listIndex);
			}
			this.WriteProperties(xamlNodes, arrayList, listIndex, data);
			if (data.TargetType == typeof(MarkupExtensionParser.UnknownMarkupExtension))
			{
				int lineNumber3 = data.LineNumber;
				int linePosition3 = data.LinePosition;
				num2 = data.Depth;
				data.Depth = num2 - 1;
				xamlNodes.Add(new XamlUnknownTagEndNode(lineNumber3, linePosition3, num2, text, xmlNamespace));
				return;
			}
			int lineNumber4 = data.LineNumber;
			int linePosition4 = data.LinePosition;
			num2 = data.Depth;
			data.Depth = num2 - 1;
			xamlNodes.Add(new XamlElementEndNode(lineNumber4, linePosition4, num2));
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x0020E940 File Offset: 0x0020D940
		internal ArrayList CompileDictionaryKeys(ArrayList complexDefAttributesList, int startingDepth)
		{
			ArrayList arrayList = new ArrayList(complexDefAttributesList.Count * 5);
			for (int i = 0; i < complexDefAttributesList.Count; i++)
			{
				DefAttributeData data = (DefAttributeData)complexDefAttributesList[i];
				this.CompileDictionaryKey(arrayList, data);
			}
			return arrayList;
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x0020E984 File Offset: 0x0020D984
		internal void CompileDictionaryKey(ArrayList xamlNodes, DefAttributeData data)
		{
			ArrayList arrayList = this.TokenizeAttributes(data.Args, data.LineNumber, data.LinePosition, data.TargetType);
			int lineNumber = data.LineNumber;
			int linePosition = data.LinePosition;
			int num = data.Depth + 1;
			data.Depth = num;
			xamlNodes.Add(new XamlKeyElementStartNode(lineNumber, linePosition, num, data.TargetAssemblyName, data.TargetFullName, data.TargetType, null));
			xamlNodes.Add(new XamlEndAttributesNode(data.LineNumber, data.LinePosition, data.Depth, true));
			int listIndex = 0;
			if (arrayList != null && (arrayList.Count == 1 || (arrayList.Count > 1 && !(arrayList[1] is string) && (char)arrayList[1] == ',')))
			{
				this.WriteConstructorParams(xamlNodes, arrayList, data, ref listIndex);
			}
			this.WriteProperties(xamlNodes, arrayList, listIndex, data);
			int lineNumber2 = data.LineNumber;
			int linePosition2 = data.LinePosition;
			num = data.Depth;
			data.Depth = num - 1;
			xamlNodes.Add(new XamlKeyElementEndNode(lineNumber2, linePosition2, num));
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x0020EA80 File Offset: 0x0020DA80
		private ArrayList TokenizeAttributes(string args, int lineNumber, int linePosition, Type extensionType)
		{
			if (extensionType == typeof(MarkupExtensionParser.UnknownMarkupExtension))
			{
				return null;
			}
			int num = 0;
			ParameterInfo[] array = this.FindLongestConstructor(extensionType, out num);
			Dictionary<string, SpecialBracketCharacters> dictionary = this._parserContext.InitBracketCharacterCacheForType(extensionType);
			Stack<char> stack = new Stack<char>();
			int num2 = 0;
			bool flag = array != null && num > 0;
			bool flag2 = false;
			ArrayList arrayList = null;
			int length = args.Length;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			char c = '\'';
			int num3 = 0;
			StringBuilder stringBuilder = null;
			SpecialBracketCharacters specialBracketCharacters = null;
			if (flag && dictionary != null)
			{
				string text = (num > 0) ? array[num2].Name : null;
				if (!string.IsNullOrEmpty(text))
				{
					specialBracketCharacters = this.GetBracketCharacterForProperty(text, dictionary);
				}
			}
			int num4 = 0;
			while (num4 < length && !flag6)
			{
				if (!flag4 && args[num4] == '\\')
				{
					flag4 = true;
				}
				else
				{
					if (!flag5 && !char.IsWhiteSpace(args[num4]))
					{
						flag5 = true;
					}
					if (flag3 || num3 > 0 || flag5)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(length);
							arrayList = new ArrayList(1);
						}
						if (flag4)
						{
							stringBuilder.Append('\\');
							stringBuilder.Append(args[num4]);
							flag4 = false;
						}
						else if (flag3 || num3 > 0)
						{
							if (flag3 && args[num4] == c)
							{
								flag3 = false;
								flag5 = false;
								MarkupExtensionParser.AddToTokenList(arrayList, stringBuilder, false);
							}
							else
							{
								if (num3 > 0 && args[num4] == '}')
								{
									num3--;
								}
								else if (args[num4] == '{')
								{
									num3++;
								}
								stringBuilder.Append(args[num4]);
							}
						}
						else if (flag2)
						{
							stringBuilder.Append(args[num4]);
							if (specialBracketCharacters.StartsEscapeSequence(args[num4]))
							{
								stack.Push(args[num4]);
							}
							else if (specialBracketCharacters.EndsEscapeSequence(args[num4]))
							{
								if (specialBracketCharacters.Match(stack.Peek(), args[num4]))
								{
									stack.Pop();
								}
								else
								{
									this.ThrowException("ParserMarkupExtensionInvalidClosingBracketCharacers", args[num4].ToString(), lineNumber, linePosition);
								}
							}
							if (stack.Count == 0)
							{
								flag2 = false;
							}
						}
						else
						{
							char c2 = args[num4];
							if (c2 <= ',')
							{
								if (c2 == '"' || c2 == '\'')
								{
									if (stringBuilder.Length != 0)
									{
										this.ThrowException("ParserMarkupExtensionNoQuotesInName", args, lineNumber, linePosition);
									}
									flag3 = true;
									c = args[num4];
									goto IL_441;
								}
								if (c2 != ',')
								{
									goto IL_405;
								}
							}
							else if (c2 != '=')
							{
								if (c2 == '{')
								{
									num3++;
									stringBuilder.Append(args[num4]);
									goto IL_441;
								}
								if (c2 != '}')
								{
									goto IL_405;
								}
								flag6 = true;
								if (stringBuilder == null)
								{
									goto IL_441;
								}
								if (stringBuilder.Length > 0)
								{
									MarkupExtensionParser.AddToTokenList(arrayList, stringBuilder, true);
									goto IL_441;
								}
								if (arrayList.Count > 0 && arrayList[arrayList.Count - 1] is char)
								{
									this.ThrowException("ParserMarkupExtensionBadDelimiter", args, lineNumber, linePosition);
									goto IL_441;
								}
								goto IL_441;
							}
							if (flag && args[num4] == ',')
							{
								flag = (++num2 < num);
								if (flag)
								{
									string text = array[num2].Name;
									specialBracketCharacters = this.GetBracketCharacterForProperty(text, dictionary);
								}
							}
							if (stringBuilder != null && stringBuilder.Length > 0)
							{
								MarkupExtensionParser.AddToTokenList(arrayList, stringBuilder, true);
								if (stack.Count != 0)
								{
									this.ThrowException("ParserMarkupExtensionMalformedBracketCharacers", stack.Peek().ToString(), lineNumber, linePosition);
								}
							}
							else if (arrayList.Count == 0)
							{
								this.ThrowException("ParserMarkupExtensionDelimiterBeforeFirstAttribute", args, lineNumber, linePosition);
							}
							else if (arrayList[arrayList.Count - 1] is char)
							{
								this.ThrowException("ParserMarkupExtensionBadDelimiter", args, lineNumber, linePosition);
							}
							if (args[num4] == '=')
							{
								flag = false;
								string text = (string)arrayList[arrayList.Count - 1];
								specialBracketCharacters = this.GetBracketCharacterForProperty(text, dictionary);
							}
							arrayList.Add(args[num4]);
							flag5 = false;
							goto IL_441;
							IL_405:
							if (specialBracketCharacters != null && specialBracketCharacters.StartsEscapeSequence(args[num4]))
							{
								stack.Clear();
								stack.Push(args[num4]);
								flag2 = true;
							}
							stringBuilder.Append(args[num4]);
						}
					}
				}
				IL_441:
				num4++;
			}
			if (!flag6)
			{
				this.ThrowException("ParserMarkupExtensionNoEndCurlie", "}", lineNumber, linePosition);
			}
			else if (num4 < length)
			{
				this.ThrowException("ParserMarkupExtensionTrailingGarbage", "}", args.Substring(num4, length - num4), lineNumber, linePosition);
			}
			return arrayList;
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x0020EF20 File Offset: 0x0020DF20
		private static void AddToTokenList(ArrayList list, StringBuilder sb, bool trim)
		{
			if (trim)
			{
				int num = sb.Length - 1;
				while (char.IsWhiteSpace(sb[num]))
				{
					num--;
				}
				sb.Length = num + 1;
			}
			list.Add(sb.ToString());
			sb.Length = 0;
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x0020EF6C File Offset: 0x0020DF6C
		private ParameterInfo[] FindLongestConstructor(Type extensionType, out int maxConstructorArguments)
		{
			ParameterInfo[] result = null;
			ConstructorInfo[] constructors = extensionType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			maxConstructorArguments = 0;
			ConstructorInfo[] array = constructors;
			for (int i = 0; i < array.Length; i++)
			{
				ParameterInfo[] parameters = array[i].GetParameters();
				if (parameters.Length >= maxConstructorArguments)
				{
					maxConstructorArguments = parameters.Length;
					result = parameters;
				}
			}
			return result;
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0020EFB0 File Offset: 0x0020DFB0
		private void WriteConstructorParams(ArrayList xamlNodes, ArrayList list, DefAttributeData data, ref int listIndex)
		{
			if (list != null && listIndex < list.Count)
			{
				int lineNumber = data.LineNumber;
				int linePosition = data.LinePosition;
				int num = data.Depth + 1;
				data.Depth = num;
				xamlNodes.Add(new XamlConstructorParametersStartNode(lineNumber, linePosition, num));
				while (listIndex < list.Count)
				{
					if (!(list[listIndex] is string))
					{
						this.ThrowException("ParserMarkupExtensionBadConstructorParam", data.Args, data.LineNumber, data.LinePosition);
					}
					if (list.Count > listIndex + 1 && list[listIndex + 1] is char && (char)list[listIndex + 1] == '=')
					{
						break;
					}
					string textContent = (string)list[listIndex];
					AttributeData attributeData = this.IsMarkupExtensionAttribute(data.DeclaringType, string.Empty, ref textContent, data.LineNumber, data.LinePosition, data.Depth, null);
					if (attributeData == null)
					{
						MarkupExtensionParser.RemoveEscapes(ref textContent);
						xamlNodes.Add(new XamlTextNode(data.LineNumber, data.LinePosition, data.Depth, textContent, null));
					}
					else
					{
						this.CompileAttributeCore(xamlNodes, attributeData);
					}
					listIndex += 2;
				}
				int lineNumber2 = data.LineNumber;
				int linePosition2 = data.LinePosition;
				num = data.Depth;
				data.Depth = num - 1;
				xamlNodes.Add(new XamlConstructorParametersEndNode(lineNumber2, linePosition2, num));
			}
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0020F10C File Offset: 0x0020E10C
		private void WriteProperties(ArrayList xamlNodes, ArrayList list, int listIndex, DefAttributeData data)
		{
			if (list != null && listIndex < list.Count)
			{
				ArrayList arrayList = new ArrayList(list.Count / 4);
				for (int i = listIndex; i < list.Count; i += 4)
				{
					if (i > list.Count - 3 || list[i + 1] is string || (char)list[i + 1] != '=')
					{
						this.ThrowException("ParserMarkupExtensionNoNameValue", data.Args, data.LineNumber, data.LinePosition);
					}
					string text = list[i] as string;
					text = text.Trim();
					if (arrayList.Contains(text))
					{
						this.ThrowException("ParserDuplicateMarkupExtensionProperty", text, data.LineNumber, data.LinePosition);
					}
					arrayList.Add(text);
					int num = text.IndexOf(':');
					string text2 = (num < 0) ? text : text.Substring(num + 1);
					string prefix = (num < 0) ? string.Empty : text.Substring(0, num);
					string attributeNamespaceUri = this.ResolveAttributeNamespaceURI(prefix, text2, data.TargetNamespaceUri);
					object info;
					string text3;
					string text4;
					Type type;
					string text5;
					this.GetAttributeContext(data.TargetType, data.TargetNamespaceUri, attributeNamespaceUri, text2, out info, out text3, out text4, out type, out text5);
					string value = list[i + 2] as string;
					AttributeData attributeData = this.IsMarkupExtensionAttribute(data.TargetType, text, ref value, data.LineNumber, data.LinePosition, data.Depth, info);
					list[i + 2] = value;
					if (data.IsUnknownExtension)
					{
						return;
					}
					if (attributeData != null)
					{
						if (attributeData.IsSimple)
						{
							this.CompileProperty(xamlNodes, text, attributeData.Args, data.TargetType, data.TargetNamespaceUri, attributeData, attributeData.LineNumber, attributeData.LinePosition, attributeData.Depth);
						}
						else
						{
							this.CompileAttribute(xamlNodes, attributeData);
						}
					}
					else
					{
						this.CompileProperty(xamlNodes, text, (string)list[i + 2], data.TargetType, data.TargetNamespaceUri, null, data.LineNumber, data.LinePosition, data.Depth);
					}
				}
			}
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x0020F318 File Offset: 0x0020E318
		private string ResolveAttributeNamespaceURI(string prefix, string name, string parentURI)
		{
			string result;
			if (!string.IsNullOrEmpty(prefix))
			{
				result = this._parserHelper.LookupNamespace(prefix);
			}
			else
			{
				int num = name.IndexOf('.');
				if (-1 == num)
				{
					result = parentURI;
				}
				else
				{
					result = this._parserHelper.LookupNamespace("");
				}
			}
			return result;
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x0020F360 File Offset: 0x0020E360
		private SpecialBracketCharacters GetBracketCharacterForProperty(string propertyName, Dictionary<string, SpecialBracketCharacters> bracketCharacterCache)
		{
			SpecialBracketCharacters result = null;
			if (bracketCharacterCache != null && bracketCharacterCache.ContainsKey(propertyName))
			{
				result = bracketCharacterCache[propertyName];
			}
			return result;
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x0020F384 File Offset: 0x0020E384
		private void CompileProperty(ArrayList xamlNodes, string name, string value, Type parentType, string parentTypeNamespaceUri, AttributeData data, int lineNumber, int linePosition, int depth)
		{
			MarkupExtensionParser.RemoveEscapes(ref name);
			MarkupExtensionParser.RemoveEscapes(ref value);
			int num = name.IndexOf(':');
			string text = (num < 0) ? name : name.Substring(num + 1);
			string text2 = (num < 0) ? string.Empty : name.Substring(0, num);
			string text3 = this.ResolveAttributeNamespaceURI(text2, text, parentTypeNamespaceUri);
			if (string.IsNullOrEmpty(text3))
			{
				this.ThrowException("ParserPrefixNSProperty", text2, name, lineNumber, linePosition);
			}
			object propertyMember;
			string assemblyName;
			string typeFullName;
			Type type;
			string propertyName;
			if (this.GetAttributeContext(parentType, parentTypeNamespaceUri, text3, text, out propertyMember, out assemblyName, out typeFullName, out type, out propertyName) != AttributeContext.Property)
			{
				this.ThrowException("ParserMarkupExtensionUnknownAttr", text, parentType.FullName, lineNumber, linePosition);
			}
			if (data == null || !data.IsSimple)
			{
				XamlPropertyNode value2 = new XamlPropertyNode(lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName, value, BamlAttributeUsage.Default, true);
				xamlNodes.Add(value2);
				return;
			}
			if (data.IsTypeExtension)
			{
				string valueTypeFullName = value;
				string valueAssemblyName = null;
				Type typeFromBaseString = this._parserContext.XamlTypeMapper.GetTypeFromBaseString(value, this._parserContext, true);
				if (typeFromBaseString != null)
				{
					valueTypeFullName = typeFromBaseString.FullName;
					valueAssemblyName = typeFromBaseString.Assembly.FullName;
				}
				XamlPropertyWithTypeNode value3 = new XamlPropertyWithTypeNode(data.LineNumber, data.LinePosition, data.Depth, propertyMember, assemblyName, typeFullName, text, valueTypeFullName, valueAssemblyName, typeFromBaseString, string.Empty, string.Empty);
				xamlNodes.Add(value3);
				return;
			}
			XamlPropertyWithExtensionNode value4 = new XamlPropertyWithExtensionNode(data.LineNumber, data.LinePosition, data.Depth, propertyMember, assemblyName, typeFullName, text, value, data.ExtensionTypeId, data.IsValueNestedExtension, data.IsValueTypeExtension);
			xamlNodes.Add(value4);
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x0020F520 File Offset: 0x0020E520
		internal static void RemoveEscapes(ref string value)
		{
			StringBuilder stringBuilder = null;
			bool flag = true;
			for (int i = 0; i < value.Length; i++)
			{
				if (flag && value[i] == '\\')
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(value.Length);
						stringBuilder.Append(value.Substring(0, i));
					}
					flag = false;
				}
				else if (stringBuilder != null)
				{
					stringBuilder.Append(value[i]);
					flag = true;
				}
			}
			if (stringBuilder != null)
			{
				value = stringBuilder.ToString();
			}
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x0020F598 File Offset: 0x0020E598
		private AttributeContext GetAttributeContext(Type elementBaseType, string elementBaseTypeNamespaceUri, string attributeNamespaceUri, string attributeLocalName, out object dynamicObject, out string assemblyName, out string typeFullName, out Type declaringType, out string dynamicObjectName)
		{
			AttributeContext result = AttributeContext.Unknown;
			dynamicObject = null;
			assemblyName = null;
			typeFullName = null;
			declaringType = null;
			dynamicObjectName = null;
			MemberInfo clrInfo = this._parserContext.XamlTypeMapper.GetClrInfo(false, elementBaseType, attributeNamespaceUri, attributeLocalName, ref dynamicObjectName);
			if (null != clrInfo)
			{
				result = AttributeContext.Property;
				dynamicObject = clrInfo;
				declaringType = clrInfo.DeclaringType;
				typeFullName = declaringType.FullName;
				assemblyName = declaringType.Assembly.FullName;
			}
			return result;
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x0020F608 File Offset: 0x0020E608
		private void ThrowException(string id, string parameter1, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				parameter1
			});
			this.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x0020F630 File Offset: 0x0020E630
		private void ThrowException(string id, string parameter1, string parameter2, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				parameter1,
				parameter2
			});
			this.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x0020F660 File Offset: 0x0020E660
		private void ThrowException(string id, string parameter1, string parameter2, string parameter3, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				parameter1,
				parameter2,
				parameter3
			});
			this.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x0020F694 File Offset: 0x0020E694
		private void ThrowExceptionWithLine(string message, int lineNumber, int linePosition)
		{
			message += " ";
			message += SR.Get("ParserLineAndOffset", new object[]
			{
				lineNumber.ToString(CultureInfo.CurrentCulture),
				linePosition.ToString(CultureInfo.CurrentCulture)
			});
			throw new XamlParseException(message, lineNumber, linePosition);
		}

		// Token: 0x04002338 RID: 9016
		private IParserHelper _parserHelper;

		// Token: 0x04002339 RID: 9017
		private ParserContext _parserContext;

		// Token: 0x02000AF6 RID: 2806
		internal class UnknownMarkupExtension
		{
		}
	}
}
