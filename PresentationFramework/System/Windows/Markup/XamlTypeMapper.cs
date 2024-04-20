using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Baml2006;
using System.Xaml;
using MS.Internal;
using MS.Internal.WindowsBase;
using MS.Utility;

namespace System.Windows.Markup
{
	// Token: 0x02000517 RID: 1303
	public class XamlTypeMapper
	{
		// Token: 0x060040BF RID: 16575 RVA: 0x00214A04 File Offset: 0x00213A04
		public XamlTypeMapper(string[] assemblyNames)
		{
			if (assemblyNames == null)
			{
				throw new ArgumentNullException("assemblyNames");
			}
			this._assemblyNames = assemblyNames;
			this._namespaceMaps = null;
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x00214A78 File Offset: 0x00213A78
		public XamlTypeMapper(string[] assemblyNames, NamespaceMapEntry[] namespaceMaps)
		{
			if (assemblyNames == null)
			{
				throw new ArgumentNullException("assemblyNames");
			}
			this._assemblyNames = assemblyNames;
			this._namespaceMaps = namespaceMaps;
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x00214AEC File Offset: 0x00213AEC
		public Type GetType(string xmlNamespace, string localName)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (localName == null)
			{
				throw new ArgumentNullException("localName");
			}
			TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, localName);
			if (typeOnly == null)
			{
				return null;
			}
			return typeOnly.ObjectType;
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x00214B2C File Offset: 0x00213B2C
		public void AddMappingProcessingInstruction(string xmlNamespace, string clrNamespace, string assemblyName)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (clrNamespace == null)
			{
				throw new ArgumentNullException("clrNamespace");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = new ClrNamespaceAssemblyPair(clrNamespace, assemblyName);
			this.PITable[xmlNamespace] = clrNamespaceAssemblyPair;
			string str = assemblyName.ToUpper(TypeConverterHelper.InvariantEnglishUS);
			string key = clrNamespace + "#" + str;
			this._piReverseTable[key] = xmlNamespace;
			if (this._schemaContext != null)
			{
				this._schemaContext.SetMappingProcessingInstruction(xmlNamespace, clrNamespaceAssemblyPair);
			}
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x00214BBC File Offset: 0x00213BBC
		public void SetAssemblyPath(string assemblyName, string assemblyPath)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (assemblyPath == null)
			{
				throw new ArgumentNullException("assemblyPath");
			}
			if (assemblyPath == string.Empty)
			{
				this._lineNumber = 0;
				this.ThrowException("ParserBadAssemblyPath");
			}
			if (assemblyName == string.Empty)
			{
				this._lineNumber = 0;
				this.ThrowException("ParserBadAssemblyName");
			}
			string text = assemblyName.ToUpper(CultureInfo.InvariantCulture);
			HybridDictionary assemblyPathTable = this._assemblyPathTable;
			lock (assemblyPathTable)
			{
				this._assemblyPathTable[text] = assemblyPath;
			}
			Assembly alreadyLoadedAssembly = ReflectionHelper.GetAlreadyLoadedAssembly(text);
			if (alreadyLoadedAssembly != null && !alreadyLoadedAssembly.GlobalAssemblyCache)
			{
				ReflectionHelper.ResetCacheForAssembly(text);
				if (this._schemaContext != null)
				{
					this._schemaContext = null;
				}
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x00214C98 File Offset: 0x00213C98
		public static XamlTypeMapper DefaultMapper
		{
			get
			{
				return XmlParserDefaults.DefaultMapper;
			}
		}

		// Token: 0x060040C5 RID: 16581 RVA: 0x00214CA0 File Offset: 0x00213CA0
		internal void Initialize()
		{
			this._typeLookupFromXmlHashtable.Clear();
			this._namespaceMapHashList.Clear();
			this._piTable.Clear();
			this._piReverseTable.Clear();
			HybridDictionary assemblyPathTable = this._assemblyPathTable;
			lock (assemblyPathTable)
			{
				this._assemblyPathTable.Clear();
			}
			this._referenceAssembliesLoaded = false;
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x00214D18 File Offset: 0x00213D18
		internal XamlTypeMapper Clone()
		{
			return new XamlTypeMapper(this._assemblyNames.Clone() as string[])
			{
				_mapTable = this._mapTable,
				_referenceAssembliesLoaded = this._referenceAssembliesLoaded,
				_lineNumber = this._lineNumber,
				_linePosition = this._linePosition,
				_namespaceMaps = (this._namespaceMaps.Clone() as NamespaceMapEntry[]),
				_typeLookupFromXmlHashtable = (this._typeLookupFromXmlHashtable.Clone() as Hashtable),
				_namespaceMapHashList = (this._namespaceMapHashList.Clone() as Hashtable),
				_typeInformationCache = this.CloneHybridDictionary(this._typeInformationCache),
				_piTable = this.CloneHybridDictionary(this._piTable),
				_piReverseTable = this.CloneStringDictionary(this._piReverseTable),
				_assemblyPathTable = this.CloneHybridDictionary(this._assemblyPathTable)
			};
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x00214DF4 File Offset: 0x00213DF4
		private HybridDictionary CloneHybridDictionary(HybridDictionary dict)
		{
			HybridDictionary hybridDictionary = new HybridDictionary(dict.Count);
			foreach (object obj in dict)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				hybridDictionary.Add(dictionaryEntry.Key, dictionaryEntry.Value);
			}
			return hybridDictionary;
		}

		// Token: 0x060040C8 RID: 16584 RVA: 0x00214E64 File Offset: 0x00213E64
		private Dictionary<string, string> CloneStringDictionary(Dictionary<string, string> dict)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> keyValuePair in dict)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return dictionary;
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x00214EC8 File Offset: 0x00213EC8
		internal string AssemblyPathFor(string assemblyName)
		{
			string result = null;
			if (assemblyName != null)
			{
				HybridDictionary assemblyPathTable = this._assemblyPathTable;
				lock (assemblyPathTable)
				{
					result = (this._assemblyPathTable[assemblyName.ToUpper(CultureInfo.InvariantCulture)] as string);
				}
			}
			return result;
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x00214F24 File Offset: 0x00213F24
		private bool LoadReferenceAssemblies()
		{
			if (!this._referenceAssembliesLoaded)
			{
				this._referenceAssembliesLoaded = true;
				foreach (object obj in this._assemblyPathTable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					ReflectionHelper.LoadAssembly(dictionaryEntry.Key as string, dictionaryEntry.Value as string);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x00214FA8 File Offset: 0x00213FA8
		internal RoutedEvent GetRoutedEvent(Type owner, string xmlNamespace, string localName)
		{
			Type type = null;
			string text = null;
			if (localName == null)
			{
				throw new ArgumentNullException("localName");
			}
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (owner != null && !ReflectionHelper.IsPublicType(owner))
			{
				this._lineNumber = 0;
				this.ThrowException("ParserOwnerEventMustBePublic", owner.FullName);
			}
			return this.GetDependencyObject(true, owner, xmlNamespace, localName, ref type, ref text) as RoutedEvent;
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00215014 File Offset: 0x00214014
		internal object ParseProperty(object targetObject, Type propType, string propName, object dpOrPiOrFi, ITypeDescriptorContext typeContext, ParserContext parserContext, string value, short converterTypeId)
		{
			this._lineNumber = ((parserContext != null) ? parserContext.LineNumber : 0);
			this._linePosition = ((parserContext != null) ? parserContext.LinePosition : 0);
			if (converterTypeId < 0 && -converterTypeId == 615)
			{
				if (propType == typeof(object) || propType == typeof(string))
				{
					return value;
				}
				string message = SR.Get("ParserCannotConvertPropertyValueString", new object[]
				{
					value,
					propName,
					propType.FullName
				});
				XamlParseException.ThrowException(parserContext, this._lineNumber, this._linePosition, message, null);
			}
			object obj = null;
			TypeConverter typeConverter;
			if (converterTypeId != 0)
			{
				typeConverter = parserContext.MapTable.GetConverterFromId(converterTypeId, propType, parserContext);
			}
			else
			{
				typeConverter = this.GetPropertyConverter(propType, dpOrPiOrFi);
			}
			try
			{
				obj = typeConverter.ConvertFromString(typeContext, TypeConverterHelper.InvariantEnglishUS, value);
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.TraceActivityItem(TraceMarkup.TypeConvert, typeConverter, value, obj);
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				IProvidePropertyFallback providePropertyFallback = targetObject as IProvidePropertyFallback;
				if (providePropertyFallback != null && providePropertyFallback.CanProvidePropertyFallback(propName))
				{
					obj = providePropertyFallback.ProvidePropertyFallback(propName, ex);
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.TraceActivityItem(TraceMarkup.TypeConvertFallback, typeConverter, value, obj);
					}
				}
				else if (typeConverter.GetType() == typeof(TypeConverter))
				{
					string message2;
					if (propName != string.Empty)
					{
						message2 = SR.Get("ParserDefaultConverterProperty", new object[]
						{
							propType.FullName,
							propName,
							value
						});
					}
					else
					{
						message2 = SR.Get("ParserDefaultConverterElement", new object[]
						{
							propType.FullName,
							value
						});
					}
					XamlParseException.ThrowException(parserContext, this._lineNumber, this._linePosition, message2, null);
				}
				else
				{
					string message3 = this.TypeConverterFailure(value, propName, propType.FullName);
					XamlParseException.ThrowException(parserContext, this._lineNumber, this._linePosition, message3, ex);
				}
			}
			if (obj != null && !propType.IsAssignableFrom(obj.GetType()))
			{
				string message4 = this.TypeConverterFailure(value, propName, propType.FullName);
				XamlParseException.ThrowException(parserContext, this._lineNumber, this._linePosition, message4, null);
			}
			return obj;
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x0021524C File Offset: 0x0021424C
		private string TypeConverterFailure(string value, string propName, string propType)
		{
			string result;
			if (propName != string.Empty)
			{
				result = SR.Get("ParserCannotConvertPropertyValueString", new object[]
				{
					value,
					propName,
					propType
				});
			}
			else
			{
				result = SR.Get("ParserCannotConvertInitializationText", new object[]
				{
					value,
					propType
				});
			}
			return result;
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x002152A0 File Offset: 0x002142A0
		internal void ValidateNames(string value, int lineNumber, int linePosition)
		{
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
			if (value == string.Empty)
			{
				this.ThrowException("ParserBadName", value);
			}
			if (MarkupExtensionParser.LooksLikeAMarkupExtension(value))
			{
				throw new XamlParseException(SR.Get("ParserBadUidOrNameME", new object[]
				{
					value
				}) + " " + SR.Get("ParserLineAndOffset", new object[]
				{
					lineNumber.ToString(CultureInfo.CurrentCulture),
					linePosition.ToString(CultureInfo.CurrentCulture)
				}), lineNumber, linePosition);
			}
			if (!NameValidationHelper.IsValidIdentifierName(value))
			{
				this.ThrowException("ParserBadName", value);
			}
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x0021534C File Offset: 0x0021434C
		internal void ValidateEnums(string propName, Type propType, string attribValue)
		{
			if (propType.IsEnum && attribValue != string.Empty)
			{
				bool flag = false;
				for (int i = 0; i < attribValue.Length; i++)
				{
					if (!char.IsWhiteSpace(attribValue[i]))
					{
						if (flag)
						{
							if (attribValue[i] == ',')
							{
								flag = false;
							}
						}
						else if (char.IsDigit(attribValue[i]))
						{
							this.ThrowException("ParserNoDigitEnums", propName, attribValue);
						}
						else
						{
							flag = true;
						}
					}
				}
			}
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x002153C0 File Offset: 0x002143C0
		private MemberInfo GetCachedMemberInfo(Type owner, string propName, bool onlyPropInfo, out BamlAttributeInfoRecord infoRecord)
		{
			infoRecord = null;
			if (this.MapTable != null)
			{
				string ownerTypeName = owner.IsGenericType ? (owner.Namespace + "." + owner.Name) : owner.FullName;
				object attributeInfoKey = this.MapTable.GetAttributeInfoKey(ownerTypeName, propName);
				infoRecord = (this.MapTable.GetHashTableData(attributeInfoKey) as BamlAttributeInfoRecord);
				if (infoRecord != null)
				{
					return infoRecord.GetPropertyMember(onlyPropInfo) as MemberInfo;
				}
			}
			return null;
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x00215438 File Offset: 0x00214438
		private void AddCachedAttributeInfo(Type ownerType, BamlAttributeInfoRecord infoRecord)
		{
			if (this.MapTable != null)
			{
				object attributeInfoKey = this.MapTable.GetAttributeInfoKey(ownerType.FullName, infoRecord.Name);
				this.MapTable.AddHashTableData(attributeInfoKey, infoRecord);
			}
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x00215474 File Offset: 0x00214474
		internal void UpdateClrPropertyInfo(Type currentParentType, BamlAttributeInfoRecord attribInfo)
		{
			bool isInternal = false;
			string name = attribInfo.Name;
			BamlAttributeInfoRecord bamlAttributeInfoRecord;
			attribInfo.PropInfo = (this.GetCachedMemberInfo(currentParentType, name, true, out bamlAttributeInfoRecord) as PropertyInfo);
			if (attribInfo.PropInfo == null)
			{
				attribInfo.PropInfo = this.PropertyInfoFromName(name, currentParentType, !ReflectionHelper.IsPublicType(currentParentType), false, out isInternal);
				attribInfo.IsInternal = isInternal;
				if (attribInfo.PropInfo != null)
				{
					if (bamlAttributeInfoRecord != null)
					{
						bamlAttributeInfoRecord.SetPropertyMember(attribInfo.PropInfo);
						bamlAttributeInfoRecord.IsInternal = attribInfo.IsInternal;
						return;
					}
					this.AddCachedAttributeInfo(currentParentType, attribInfo);
					return;
				}
			}
			else
			{
				attribInfo.IsInternal = bamlAttributeInfoRecord.IsInternal;
			}
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x00215510 File Offset: 0x00214510
		private void UpdateAttachedPropertyMethdodInfo(BamlAttributeInfoRecord attributeInfo, bool isSetter)
		{
			MethodInfo methodInfo = null;
			Type ownerType = attributeInfo.OwnerType;
			bool flag = !ReflectionHelper.IsPublicType(ownerType);
			string name = (isSetter ? "Set" : "Get") + attributeInfo.Name;
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
			try
			{
				if (!flag)
				{
					methodInfo = ownerType.GetMethod(name, bindingFlags);
				}
				if (methodInfo == null)
				{
					methodInfo = ownerType.GetMethod(name, bindingFlags | BindingFlags.NonPublic);
				}
			}
			catch (AmbiguousMatchException)
			{
			}
			int num = isSetter ? 2 : 1;
			if (methodInfo != null && methodInfo.GetParameters().Length == num)
			{
				if (isSetter)
				{
					attributeInfo.AttachedPropertySetter = methodInfo;
					return;
				}
				attributeInfo.AttachedPropertyGetter = methodInfo;
			}
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x002155B8 File Offset: 0x002145B8
		internal void UpdateAttachedPropertySetter(BamlAttributeInfoRecord attributeInfo)
		{
			if (attributeInfo.AttachedPropertySetter == null)
			{
				this.UpdateAttachedPropertyMethdodInfo(attributeInfo, true);
			}
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x002155D0 File Offset: 0x002145D0
		internal void UpdateAttachedPropertyGetter(BamlAttributeInfoRecord attributeInfo)
		{
			if (attributeInfo.AttachedPropertyGetter == null)
			{
				this.UpdateAttachedPropertyMethdodInfo(attributeInfo, false);
			}
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x002155E8 File Offset: 0x002145E8
		internal MemberInfo GetClrInfo(bool isEvent, Type owner, string xmlNamespace, string localName, ref string propName)
		{
			string globalClassName = null;
			int num = localName.LastIndexOf('.');
			if (-1 != num)
			{
				globalClassName = localName.Substring(0, num);
				localName = localName.Substring(num + 1);
			}
			return this.GetClrInfoForClass(isEvent, owner, xmlNamespace, localName, globalClassName, ref propName);
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x0021562C File Offset: 0x0021462C
		internal bool IsAllowedPropertySet(PropertyInfo pi)
		{
			MethodInfo setMethod = pi.GetSetMethod(true);
			return setMethod != null && setMethod.IsPublic;
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x00215654 File Offset: 0x00214654
		internal bool IsAllowedPropertyGet(PropertyInfo pi)
		{
			MethodInfo getMethod = pi.GetGetMethod(true);
			return getMethod != null && getMethod.IsPublic;
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x0021567C File Offset: 0x0021467C
		internal static bool IsAllowedPropertySet(PropertyInfo pi, bool allowProtected, out bool isPublic)
		{
			MethodInfo setMethod = pi.GetSetMethod(true);
			bool flag = allowProtected && setMethod != null && setMethod.IsFamily;
			isPublic = (setMethod != null && setMethod.IsPublic && ReflectionHelper.IsPublicType(setMethod.DeclaringType));
			return setMethod != null && (setMethod.IsPublic || setMethod.IsAssembly || setMethod.IsFamilyOrAssembly || flag);
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x002156F0 File Offset: 0x002146F0
		private static bool IsAllowedPropertyGet(PropertyInfo pi, bool allowProtected, out bool isPublic)
		{
			MethodInfo getMethod = pi.GetGetMethod(true);
			bool flag = allowProtected && getMethod != null && getMethod.IsFamily;
			isPublic = (getMethod != null && getMethod.IsPublic && ReflectionHelper.IsPublicType(getMethod.DeclaringType));
			return getMethod != null && (getMethod.IsPublic || getMethod.IsAssembly || getMethod.IsFamilyOrAssembly || flag);
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x00215764 File Offset: 0x00214764
		private static bool IsAllowedEvent(EventInfo ei, bool allowProtected, out bool isPublic)
		{
			MethodInfo addMethod = ei.GetAddMethod(true);
			bool flag = allowProtected && addMethod != null && addMethod.IsFamily;
			isPublic = (addMethod != null && addMethod.IsPublic && ReflectionHelper.IsPublicType(addMethod.DeclaringType));
			return addMethod != null && (addMethod.IsPublic || addMethod.IsAssembly || addMethod.IsFamilyOrAssembly || flag);
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x002157D8 File Offset: 0x002147D8
		private static bool IsPublicEvent(EventInfo ei)
		{
			MethodInfo addMethod = ei.GetAddMethod(true);
			return addMethod != null && addMethod.IsPublic;
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool AllowInternalType(Type type)
		{
			return false;
		}

		// Token: 0x060040DE RID: 16606 RVA: 0x00215800 File Offset: 0x00214800
		private bool IsInternalTypeAllowedInFullTrust(Type type)
		{
			bool result = false;
			if (ReflectionHelper.IsInternalType(type))
			{
				result = this.AllowInternalType(type);
			}
			return result;
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x00215820 File Offset: 0x00214820
		internal MemberInfo GetClrInfoForClass(bool isEvent, Type owner, string xmlNamespace, string localName, string globalClassName, ref string propName)
		{
			return this.GetClrInfoForClass(isEvent, owner, xmlNamespace, localName, globalClassName, false, ref propName);
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x00215834 File Offset: 0x00214834
		private MemberInfo GetClrInfoForClass(bool isEvent, Type owner, string xmlNamespace, string localName, string globalClassName, bool tryInternal, ref string propName)
		{
			bool flag = false;
			MemberInfo memberInfo = null;
			BindingFlags bindingFlags = BindingFlags.Public;
			propName = null;
			if (globalClassName != null)
			{
				TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, globalClassName);
				if (typeOnly != null && typeOnly.ObjectType != null)
				{
					Type objectType = typeOnly.ObjectType;
					BamlAttributeInfoRecord bamlAttributeInfoRecord;
					memberInfo = this.GetCachedMemberInfo(objectType, localName, false, out bamlAttributeInfoRecord);
					if (memberInfo == null)
					{
						if (isEvent)
						{
							memberInfo = objectType.GetMethod("Add" + localName + "Handler", bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
							if (memberInfo != null)
							{
								MethodInfo methodInfo = memberInfo as MethodInfo;
								if (methodInfo != null)
								{
									ParameterInfo[] parameters = methodInfo.GetParameters();
									Type type = KnownTypes.Types[135];
									if (parameters == null || parameters.Length != 2 || !type.IsAssignableFrom(parameters[0].ParameterType))
									{
										memberInfo = null;
									}
								}
							}
							if (memberInfo == null)
							{
								memberInfo = objectType.GetEvent(localName, bindingFlags | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
								if (memberInfo != null)
								{
									EventInfo eventInfo = memberInfo as EventInfo;
									if (!ReflectionHelper.IsPublicType(eventInfo.EventHandlerType))
									{
										this.ThrowException("ParserEventDelegateTypeNotAccessible", eventInfo.EventHandlerType.FullName, objectType.Name + "." + localName);
									}
									if (!XamlTypeMapper.IsPublicEvent(eventInfo))
									{
										this.ThrowException("ParserCantSetAttribute", "event", objectType.Name + "." + localName, "add");
									}
								}
							}
						}
						else
						{
							memberInfo = objectType.GetMethod("Set" + localName, bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
							if (memberInfo != null && ((MethodInfo)memberInfo).GetParameters().Length != 2)
							{
								memberInfo = null;
							}
							if (memberInfo == null)
							{
								memberInfo = objectType.GetMethod("Get" + localName, bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
								if (memberInfo != null && ((MethodInfo)memberInfo).GetParameters().Length != 1)
								{
									memberInfo = null;
								}
							}
							if (memberInfo == null)
							{
								memberInfo = this.PropertyInfoFromName(localName, objectType, tryInternal, true, out flag);
								if (memberInfo != null && owner != null && !objectType.IsAssignableFrom(owner))
								{
									this.ThrowException("ParserAttachedPropInheritError", string.Format(CultureInfo.CurrentCulture, "{0}.{1}", objectType.Name, localName), owner.Name);
								}
							}
							if (null != memberInfo && bamlAttributeInfoRecord != null)
							{
								if (bamlAttributeInfoRecord.DP == null)
								{
									bamlAttributeInfoRecord.DP = this.MapTable.GetDependencyProperty(bamlAttributeInfoRecord);
								}
								bamlAttributeInfoRecord.SetPropertyMember(memberInfo);
							}
						}
					}
				}
			}
			else if (null != owner && null != owner)
			{
				BamlAttributeInfoRecord bamlAttributeInfoRecord2;
				memberInfo = this.GetCachedMemberInfo(owner, localName, false, out bamlAttributeInfoRecord2);
				if (memberInfo == null)
				{
					if (isEvent)
					{
						memberInfo = owner.GetMethod("Add" + localName + "Handler", bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
						if (memberInfo != null)
						{
							MethodInfo methodInfo2 = memberInfo as MethodInfo;
							if (methodInfo2 != null)
							{
								ParameterInfo[] parameters = methodInfo2.GetParameters();
								Type type2 = KnownTypes.Types[135];
								if (parameters == null || parameters.Length != 2 || !type2.IsAssignableFrom(parameters[0].ParameterType))
								{
									memberInfo = null;
								}
							}
						}
						if (memberInfo == null)
						{
							memberInfo = owner.GetEvent(localName, BindingFlags.Instance | BindingFlags.FlattenHierarchy | bindingFlags);
							if (memberInfo != null)
							{
								EventInfo eventInfo2 = memberInfo as EventInfo;
								if (!ReflectionHelper.IsPublicType(eventInfo2.EventHandlerType))
								{
									this.ThrowException("ParserEventDelegateTypeNotAccessible", eventInfo2.EventHandlerType.FullName, owner.Name + "." + localName);
								}
								if (!XamlTypeMapper.IsPublicEvent(eventInfo2))
								{
									this.ThrowException("ParserCantSetAttribute", "event", owner.Name + "." + localName, "add");
								}
							}
						}
					}
					else
					{
						memberInfo = owner.GetMethod("Set" + localName, bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
						if (memberInfo != null && ((MethodInfo)memberInfo).GetParameters().Length != 2)
						{
							memberInfo = null;
						}
						if (memberInfo == null)
						{
							memberInfo = owner.GetMethod("Get" + localName, bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
							if (memberInfo != null && ((MethodInfo)memberInfo).GetParameters().Length != 1)
							{
								memberInfo = null;
							}
						}
						if (memberInfo == null)
						{
							memberInfo = this.PropertyInfoFromName(localName, owner, tryInternal, true, out flag);
						}
						if (null != memberInfo && bamlAttributeInfoRecord2 != null)
						{
							if (bamlAttributeInfoRecord2.DP == null)
							{
								bamlAttributeInfoRecord2.DP = this.MapTable.GetDependencyProperty(bamlAttributeInfoRecord2);
							}
							bamlAttributeInfoRecord2.SetPropertyMember(memberInfo);
						}
					}
				}
			}
			if (null != memberInfo)
			{
				propName = localName;
			}
			return memberInfo;
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x00215CD8 File Offset: 0x00214CD8
		internal EventInfo GetClrEventInfo(Type owner, string eventName)
		{
			EventInfo eventInfo = null;
			while (owner != null)
			{
				eventInfo = owner.GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public);
				if (eventInfo != null)
				{
					break;
				}
				owner = this.GetCachedBaseType(owner);
			}
			return eventInfo;
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x00215D10 File Offset: 0x00214D10
		internal object GetDependencyObject(bool isEvent, Type owner, string xmlNamespace, string localName, ref Type baseType, ref string dynamicObjectName)
		{
			object obj = null;
			string text = null;
			dynamicObjectName = null;
			int num = localName.LastIndexOf('.');
			if (-1 != num)
			{
				text = localName.Substring(0, num);
				localName = localName.Substring(num + 1);
			}
			if (text != null)
			{
				TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, text);
				if (typeOnly != null && typeOnly.ObjectType != null)
				{
					baseType = typeOnly.ObjectType;
					if (isEvent)
					{
						obj = this.RoutedEventFromName(localName, baseType);
					}
					else
					{
						obj = DependencyProperty.FromName(localName, baseType);
					}
					if (obj != null)
					{
						dynamicObjectName = localName;
					}
				}
			}
			else
			{
				NamespaceMapEntry[] namespaceMapEntries = this.GetNamespaceMapEntries(xmlNamespace);
				if (namespaceMapEntries == null)
				{
					return null;
				}
				baseType = owner;
				while (null != baseType)
				{
					bool flag = false;
					int num2 = 0;
					while (num2 < namespaceMapEntries.Length && !flag)
					{
						if (namespaceMapEntries[num2].ClrNamespace == this.GetCachedNamespace(baseType))
						{
							flag = true;
						}
						num2++;
					}
					if (flag)
					{
						if (isEvent)
						{
							obj = this.RoutedEventFromName(localName, baseType);
						}
						else
						{
							obj = DependencyProperty.FromName(localName, baseType);
						}
					}
					if (obj != null || isEvent)
					{
						dynamicObjectName = localName;
						break;
					}
					baseType = this.GetCachedBaseType(baseType);
				}
			}
			return obj;
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x00215E34 File Offset: 0x00214E34
		internal DependencyProperty DependencyPropertyFromName(string localName, string xmlNamespace, ref Type ownerType)
		{
			int num = localName.LastIndexOf('.');
			if (-1 != num)
			{
				string text = localName.Substring(0, num);
				localName = localName.Substring(num + 1);
				TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, text);
				if (typeOnly == null || typeOnly.ObjectType == null)
				{
					this.ThrowException("ParserNoType", text);
				}
				ownerType = typeOnly.ObjectType;
			}
			if (null == ownerType)
			{
				throw new ArgumentNullException("ownerType");
			}
			return DependencyProperty.FromName(localName, ownerType);
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x00215EB0 File Offset: 0x00214EB0
		internal PropertyInfo GetXmlLangProperty(string xmlNamespace, string localName)
		{
			TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, localName);
			if (typeOnly == null || typeOnly.ObjectType == null)
			{
				return null;
			}
			if (typeOnly.XmlLangProperty == null)
			{
				BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(-1);
				if (typeOnly.ObjectType.Assembly == assemblyInfoFromId.Assembly)
				{
					if (KnownTypes.Types[226].IsAssignableFrom(typeOnly.ObjectType) || KnownTypes.Types[225].IsAssignableFrom(typeOnly.ObjectType))
					{
						typeOnly.XmlLangProperty = KnownTypes.Types[226].GetProperty("Language", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					}
				}
				else
				{
					string text = null;
					bool flag = false;
					AttributeCollection attributes = TypeDescriptor.GetAttributes(typeOnly.ObjectType);
					if (attributes != null)
					{
						XmlLangPropertyAttribute xmlLangPropertyAttribute = attributes[typeof(XmlLangPropertyAttribute)] as XmlLangPropertyAttribute;
						if (xmlLangPropertyAttribute != null)
						{
							flag = true;
							text = xmlLangPropertyAttribute.Name;
						}
					}
					if (flag)
					{
						if (text != null && text.Length > 0)
						{
							typeOnly.XmlLangProperty = typeOnly.ObjectType.GetProperty(text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						}
						if (typeOnly.XmlLangProperty == null)
						{
							this.ThrowException("ParserXmlLangPropertyValueInvalid");
						}
					}
				}
			}
			return typeOnly.XmlLangProperty;
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x00215FEC File Offset: 0x00214FEC
		private PropertyInfo PropertyInfoFromName(string localName, Type ownerType, bool tryInternal, bool tryPublicOnly, out bool isInternal)
		{
			PropertyInfo propertyInfo = null;
			isInternal = false;
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(ownerType);
			XamlTypeMapper.PropertyAndType propertyAndType = cachedInformationForType.GetPropertyAndType(localName);
			if (propertyAndType == null || !propertyAndType.PropInfoSet)
			{
				try
				{
					BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
					if (!tryInternal)
					{
						propertyInfo = ownerType.GetProperty(localName, bindingFlags);
					}
					if (propertyInfo == null && !tryPublicOnly)
					{
						propertyInfo = ownerType.GetProperty(localName, bindingFlags | BindingFlags.NonPublic);
						if (propertyInfo != null)
						{
							isInternal = true;
						}
					}
				}
				catch (AmbiguousMatchException)
				{
					PropertyInfo[] properties = ownerType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
					for (int i = 0; i < properties.Length; i++)
					{
						if (properties[i].Name == localName)
						{
							propertyInfo = properties[i];
							break;
						}
					}
				}
				cachedInformationForType.SetPropertyAndType(localName, propertyInfo, ownerType, isInternal);
			}
			else
			{
				propertyInfo = propertyAndType.PropInfo;
				isInternal = propertyAndType.IsInternal;
			}
			return propertyInfo;
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x002160C0 File Offset: 0x002150C0
		internal RoutedEvent RoutedEventFromName(string localName, Type ownerType)
		{
			Type type = ownerType;
			while (null != type)
			{
				SecurityHelper.RunClassConstructor(type);
				type = this.GetCachedBaseType(type);
			}
			return EventManager.GetRoutedEventFromName(localName, ownerType);
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x002160F0 File Offset: 0x002150F0
		internal static Type GetPropertyType(object propertyMember)
		{
			Type result;
			bool flag;
			XamlTypeMapper.GetPropertyType(propertyMember, out result, out flag);
			return result;
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x00216108 File Offset: 0x00215108
		internal static void GetPropertyType(object propertyMember, out Type propertyType, out bool propertyCanWrite)
		{
			DependencyProperty dependencyProperty = propertyMember as DependencyProperty;
			if (dependencyProperty != null)
			{
				propertyType = dependencyProperty.PropertyType;
				propertyCanWrite = !dependencyProperty.ReadOnly;
				return;
			}
			PropertyInfo propertyInfo = propertyMember as PropertyInfo;
			if (propertyInfo != null)
			{
				propertyType = propertyInfo.PropertyType;
				propertyCanWrite = propertyInfo.CanWrite;
				return;
			}
			MethodInfo methodInfo = propertyMember as MethodInfo;
			if (methodInfo != null)
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				propertyType = ((parameters.Length == 1) ? methodInfo.ReturnType : parameters[1].ParameterType);
				propertyCanWrite = (parameters.Length != 1);
				return;
			}
			propertyType = typeof(object);
			propertyCanWrite = false;
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x002161A0 File Offset: 0x002151A0
		internal static string GetPropertyName(object propertyMember)
		{
			DependencyProperty dependencyProperty = propertyMember as DependencyProperty;
			if (dependencyProperty != null)
			{
				return dependencyProperty.Name;
			}
			PropertyInfo propertyInfo = propertyMember as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.Name;
			}
			MethodInfo methodInfo = propertyMember as MethodInfo;
			if (methodInfo != null)
			{
				return methodInfo.Name.Substring("Get".Length);
			}
			return null;
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x002161FC File Offset: 0x002151FC
		internal static Type GetDeclaringType(object propertyMember)
		{
			MemberInfo memberInfo = propertyMember as MemberInfo;
			Type result;
			if (memberInfo != null)
			{
				result = memberInfo.DeclaringType;
			}
			else
			{
				result = ((DependencyProperty)propertyMember).OwnerType;
			}
			return result;
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x00216234 File Offset: 0x00215234
		internal static Type GetTypeFromName(string typeName, DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			int num = typeName.IndexOf(':');
			string text = string.Empty;
			if (num > 0)
			{
				text = typeName.Substring(0, num);
				typeName = typeName.Substring(num + 1, typeName.Length - num - 1);
			}
			XmlnsDictionary xmlnsDictionary = element.GetValue(XmlAttributeProperties.XmlnsDictionaryProperty) as XmlnsDictionary;
			object obj = (xmlnsDictionary != null) ? xmlnsDictionary[text] : null;
			Hashtable hashtable = element.GetValue(XmlAttributeProperties.XmlNamespaceMapsProperty) as Hashtable;
			NamespaceMapEntry[] array = (hashtable != null && obj != null) ? (hashtable[obj] as NamespaceMapEntry[]) : null;
			if (array == null)
			{
				if (text == string.Empty)
				{
					foreach (ClrNamespaceAssemblyPair clrNamespaceAssemblyPair in XamlTypeMapper.GetClrNamespacePairFromCache("http://schemas.microsoft.com/winfx/2006/xaml/presentation"))
					{
						if (clrNamespaceAssemblyPair.AssemblyName != null)
						{
							Assembly assembly = ReflectionHelper.LoadAssembly(clrNamespaceAssemblyPair.AssemblyName, null);
							if (assembly != null)
							{
								string name = string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}.{1}", clrNamespaceAssemblyPair.ClrNamespace, typeName);
								Type type = assembly.GetType(name);
								if (type != null)
								{
									return type;
								}
							}
						}
					}
				}
				return null;
			}
			for (int i = 0; i < array.Length; i++)
			{
				Assembly assembly2 = array[i].Assembly;
				if (assembly2 != null)
				{
					string name2 = string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}.{1}", array[i].ClrNamespace, typeName);
					Type type2 = assembly2.GetType(name2);
					if (type2 != null)
					{
						return type2;
					}
				}
			}
			return null;
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x002163F0 File Offset: 0x002153F0
		internal Type GetTargetTypeAndMember(string valueParam, ParserContext context, bool isTypeExpected, out string memberName)
		{
			string text = valueParam;
			string text2 = string.Empty;
			int num = text.IndexOf(':');
			if (num >= 0)
			{
				text2 = text.Substring(0, num);
				text = text.Substring(num + 1);
			}
			memberName = null;
			Type type = null;
			num = text.LastIndexOf('.');
			if (num >= 0)
			{
				memberName = text.Substring(num + 1);
				text = text.Substring(0, num);
				string xmlNamespace = context.XmlnsDictionary[text2];
				TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, text);
				if (typeOnly != null)
				{
					type = typeOnly.ObjectType;
				}
				if (type == null)
				{
					this.ThrowException("ParserNoType", text);
				}
			}
			else if (!isTypeExpected && text2.Length == 0)
			{
				memberName = text;
			}
			else
			{
				this.ThrowException("ParserBadMemberReference", valueParam);
			}
			return type;
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x002164A8 File Offset: 0x002154A8
		internal Type GetDependencyPropertyOwnerAndName(string memberValue, ParserContext context, Type defaultTargetType, out string memberName)
		{
			Type type = this.GetTargetTypeAndMember(memberValue, context, false, out memberName);
			if (type == null)
			{
				type = defaultTargetType;
				if (type == null)
				{
					this.ThrowException("ParserBadMemberReference", memberValue);
				}
			}
			string memberName2 = memberName + "Property";
			MemberInfo staticMemberInfo = this.GetStaticMemberInfo(type, memberName2, true);
			if (staticMemberInfo.DeclaringType != type)
			{
				type = staticMemberInfo.DeclaringType;
			}
			return type;
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x0021650F File Offset: 0x0021550F
		internal MemberInfo GetStaticMemberInfo(Type targetType, string memberName, bool fieldInfoOnly)
		{
			MemberInfo staticMemberInfo = this.GetStaticMemberInfo(targetType, memberName, fieldInfoOnly, false);
			if (staticMemberInfo == null)
			{
				this.ThrowException("ParserInvalidStaticMember", memberName, targetType.Name);
			}
			return staticMemberInfo;
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x00216538 File Offset: 0x00215538
		private MemberInfo GetStaticMemberInfo(Type targetType, string memberName, bool fieldInfoOnly, bool tryInternal)
		{
			MemberInfo memberInfo = null;
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
			if (tryInternal)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			if (!fieldInfoOnly)
			{
				memberInfo = targetType.GetProperty(memberName, bindingFlags);
			}
			if (memberInfo == null)
			{
				memberInfo = targetType.GetField(memberName, bindingFlags);
			}
			return memberInfo;
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x00216574 File Offset: 0x00215574
		internal TypeAndSerializer GetTypeOnly(string xmlNamespace, string localName)
		{
			string key = xmlNamespace + ":" + localName;
			TypeAndSerializer typeAndSerializer = this._typeLookupFromXmlHashtable[key] as TypeAndSerializer;
			if (typeAndSerializer == null && !this._typeLookupFromXmlHashtable.Contains(key))
			{
				typeAndSerializer = this.CreateTypeAndSerializer(xmlNamespace, localName);
				this._typeLookupFromXmlHashtable[key] = typeAndSerializer;
			}
			return typeAndSerializer;
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x002165C8 File Offset: 0x002155C8
		internal TypeAndSerializer GetTypeAndSerializer(string xmlNamespace, string localName, object dpOrPiorMi)
		{
			string key = xmlNamespace + ":" + localName;
			TypeAndSerializer typeAndSerializer = this._typeLookupFromXmlHashtable[key] as TypeAndSerializer;
			if (typeAndSerializer == null && !this._typeLookupFromXmlHashtable.Contains(key))
			{
				typeAndSerializer = this.CreateTypeAndSerializer(xmlNamespace, localName);
				this._typeLookupFromXmlHashtable[key] = typeAndSerializer;
			}
			if (typeAndSerializer != null && !typeAndSerializer.IsSerializerTypeSet)
			{
				typeAndSerializer.SerializerType = this.GetXamlSerializerForType(typeAndSerializer.ObjectType);
				typeAndSerializer.IsSerializerTypeSet = true;
			}
			return typeAndSerializer;
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x00216640 File Offset: 0x00215640
		private TypeAndSerializer CreateTypeAndSerializer(string xmlNamespace, string localName)
		{
			TypeAndSerializer typeAndSerializer = null;
			NamespaceMapEntry[] namespaceMapEntries = this.GetNamespaceMapEntries(xmlNamespace);
			if (namespaceMapEntries != null)
			{
				bool flag = true;
				int i = 0;
				while (i < namespaceMapEntries.Length)
				{
					NamespaceMapEntry namespaceMapEntry = namespaceMapEntries[i];
					if (namespaceMapEntry != null)
					{
						Type objectType = this.GetObjectType(namespaceMapEntry, localName, flag);
						if (null != objectType)
						{
							if (!ReflectionHelper.IsPublicType(objectType) && !this.IsInternalTypeAllowedInFullTrust(objectType))
							{
								this.ThrowException("ParserPublicType", objectType.Name);
							}
							typeAndSerializer = new TypeAndSerializer();
							typeAndSerializer.ObjectType = objectType;
							break;
						}
					}
					i++;
					if (flag && i == namespaceMapEntries.Length)
					{
						flag = false;
						i = 0;
					}
				}
			}
			return typeAndSerializer;
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x002166CC File Offset: 0x002156CC
		private Type GetObjectType(NamespaceMapEntry namespaceMap, string localName, bool knownTypesOnly)
		{
			Type result = null;
			if (knownTypesOnly)
			{
				short knownTypeIdFromName = BamlMapTable.GetKnownTypeIdFromName(namespaceMap.AssemblyName, namespaceMap.ClrNamespace, localName);
				if (knownTypeIdFromName != 0)
				{
					result = BamlMapTable.GetKnownTypeFromId(knownTypeIdFromName);
				}
			}
			else
			{
				Assembly assembly = namespaceMap.Assembly;
				if (null != assembly)
				{
					string name = namespaceMap.ClrNamespace + "." + localName;
					try
					{
						result = assembly.GetType(name);
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalException(ex))
						{
							throw;
						}
						if (this.LoadReferenceAssemblies())
						{
							try
							{
								result = assembly.GetType(name);
							}
							catch (ArgumentException)
							{
								result = null;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x00216768 File Offset: 0x00215768
		internal int GetCustomBamlSerializerIdForType(Type objectType)
		{
			if (objectType == KnownTypes.Types[52])
			{
				return 744;
			}
			if (objectType == KnownTypes.Types[237] || objectType == KnownTypes.Types[609])
			{
				return 746;
			}
			if (objectType == KnownTypes.Types[461])
			{
				return 747;
			}
			if (objectType == KnownTypes.Types[713])
			{
				return 752;
			}
			if (objectType == KnownTypes.Types[472])
			{
				return 748;
			}
			if (objectType == KnownTypes.Types[313])
			{
				return 745;
			}
			return 0;
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x00216838 File Offset: 0x00215838
		internal Type GetXamlSerializerForType(Type objectType)
		{
			if (objectType == KnownTypes.Types[620])
			{
				return typeof(XamlStyleSerializer);
			}
			if (KnownTypes.Types[231].IsAssignableFrom(objectType))
			{
				return typeof(XamlTemplateSerializer);
			}
			return null;
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x0021688C File Offset: 0x0021588C
		internal static Type GetInternalTypeHelperTypeFromAssembly(ParserContext pc)
		{
			Assembly streamCreatedAssembly = pc.StreamCreatedAssembly;
			if (streamCreatedAssembly == null)
			{
				return null;
			}
			Type type = streamCreatedAssembly.GetType("XamlGeneratedNamespace.GeneratedInternalTypeHelper");
			if (type == null)
			{
				RootNamespaceAttribute rootNamespaceAttribute = (RootNamespaceAttribute)Attribute.GetCustomAttribute(streamCreatedAssembly, typeof(RootNamespaceAttribute));
				if (rootNamespaceAttribute != null)
				{
					string @namespace = rootNamespaceAttribute.Namespace;
					type = streamCreatedAssembly.GetType(@namespace + ".XamlGeneratedNamespace.GeneratedInternalTypeHelper");
				}
			}
			return type;
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x002168F4 File Offset: 0x002158F4
		private static InternalTypeHelper GetInternalTypeHelperFromAssembly(ParserContext pc)
		{
			InternalTypeHelper result = null;
			Type internalTypeHelperTypeFromAssembly = XamlTypeMapper.GetInternalTypeHelperTypeFromAssembly(pc);
			if (internalTypeHelperTypeFromAssembly != null)
			{
				result = (InternalTypeHelper)Activator.CreateInstance(internalTypeHelperTypeFromAssembly);
			}
			return result;
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00216920 File Offset: 0x00215920
		internal static object CreateInternalInstance(ParserContext pc, Type type)
		{
			return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, TypeConverterHelper.InvariantEnglishUS);
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x00216934 File Offset: 0x00215934
		internal static object GetInternalPropertyValue(ParserContext pc, object rootElement, PropertyInfo pi, object target)
		{
			object result = null;
			bool flag = false;
			bool allowProtected = rootElement is IComponentConnector && rootElement == target;
			if (XamlTypeMapper.IsAllowedPropertyGet(pi, allowProtected, out flag))
			{
				result = pi.GetValue(target, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
			}
			return result;
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x00216974 File Offset: 0x00215974
		internal static bool SetInternalPropertyValue(ParserContext pc, object rootElement, PropertyInfo pi, object target, object value)
		{
			bool flag = false;
			bool allowProtected = rootElement is IComponentConnector && rootElement == target;
			if (XamlTypeMapper.IsAllowedPropertySet(pi, allowProtected, out flag))
			{
				pi.SetValue(target, value, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
				return true;
			}
			return false;
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x002169B4 File Offset: 0x002159B4
		internal static Delegate CreateDelegate(ParserContext pc, Type delegateType, object target, string handler)
		{
			Delegate result = null;
			if (ReflectionHelper.IsPublicType(delegateType) || ReflectionHelper.IsInternalType(delegateType))
			{
				result = Delegate.CreateDelegate(delegateType, target, handler);
			}
			return result;
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x002169E0 File Offset: 0x002159E0
		internal static bool AddInternalEventHandler(ParserContext pc, object rootElement, EventInfo eventInfo, object target, Delegate handler)
		{
			bool flag = false;
			bool allowProtected = rootElement == target;
			if (XamlTypeMapper.IsAllowedEvent(eventInfo, allowProtected, out flag))
			{
				eventInfo.AddEventHandler(target, handler);
				return true;
			}
			return false;
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x00105F35 File Offset: 0x00104F35
		internal bool IsLocalAssembly(string namespaceUri)
		{
			return false;
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x00216A0C File Offset: 0x00215A0C
		internal Type GetTypeFromBaseString(string typeString, ParserContext context, bool throwOnError)
		{
			string text = string.Empty;
			Type type = null;
			int num = typeString.IndexOf(':');
			if (num == -1)
			{
				text = context.XmlnsDictionary[string.Empty];
				if (text == null)
				{
					this.ThrowException("ParserUndeclaredNS", string.Empty);
				}
			}
			else
			{
				string text2 = typeString.Substring(0, num);
				text = context.XmlnsDictionary[text2];
				if (text == null)
				{
					this.ThrowException("ParserUndeclaredNS", text2);
				}
				else
				{
					typeString = typeString.Substring(num + 1);
				}
			}
			if (string.CompareOrdinal(text, "http://schemas.microsoft.com/winfx/2006/xaml/presentation") == 0)
			{
				if (!(typeString == "SystemParameters"))
				{
					if (!(typeString == "SystemColors"))
					{
						if (typeString == "SystemFonts")
						{
							type = typeof(SystemFonts);
						}
					}
					else
					{
						type = typeof(SystemColors);
					}
				}
				else
				{
					type = typeof(SystemParameters);
				}
			}
			if (type == null)
			{
				type = this.GetType(text, typeString);
			}
			if (type == null && throwOnError && !this.IsLocalAssembly(text))
			{
				this._lineNumber = ((context != null) ? context.LineNumber : 0);
				this._linePosition = ((context != null) ? context.LinePosition : 0);
				this.ThrowException("ParserResourceKeyType", typeString);
			}
			return type;
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x00216B38 File Offset: 0x00215B38
		private XamlTypeMapper.TypeInformationCacheData GetCachedInformationForType(Type type)
		{
			XamlTypeMapper.TypeInformationCacheData typeInformationCacheData = this._typeInformationCache[type] as XamlTypeMapper.TypeInformationCacheData;
			if (typeInformationCacheData == null)
			{
				typeInformationCacheData = new XamlTypeMapper.TypeInformationCacheData(type.BaseType);
				typeInformationCacheData.ClrNamespace = type.Namespace;
				this._typeInformationCache[type] = typeInformationCacheData;
			}
			return typeInformationCacheData;
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x00216B80 File Offset: 0x00215B80
		private Type GetCachedBaseType(Type t)
		{
			return this.GetCachedInformationForType(t).BaseType;
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x00216B90 File Offset: 0x00215B90
		internal static string ProcessNameString(ParserContext parserContext, ref string nameString)
		{
			int num = nameString.IndexOf(':');
			string text = string.Empty;
			if (num != -1)
			{
				text = nameString.Substring(0, num);
				nameString = nameString.Substring(num + 1);
			}
			string text2 = parserContext.XmlnsDictionary[text];
			if (text2 == null)
			{
				parserContext.XamlTypeMapper.ThrowException("ParserPrefixNSProperty", text, nameString);
			}
			return text2;
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x00216BEC File Offset: 0x00215BEC
		internal static DependencyProperty ParsePropertyName(ParserContext parserContext, string propertyName, ref Type ownerType)
		{
			string xmlNamespace = XamlTypeMapper.ProcessNameString(parserContext, ref propertyName);
			return parserContext.XamlTypeMapper.DependencyPropertyFromName(propertyName, xmlNamespace, ref ownerType);
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x00216C10 File Offset: 0x00215C10
		internal static RoutedEvent ParseEventName(ParserContext parserContext, string eventName, Type ownerType)
		{
			string xmlNamespace = XamlTypeMapper.ProcessNameString(parserContext, ref eventName);
			return parserContext.XamlTypeMapper.GetRoutedEvent(ownerType, xmlNamespace, eventName);
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x00216C34 File Offset: 0x00215C34
		internal object CreateInstance(Type t)
		{
			short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(t);
			object result;
			if (knownTypeIdFromType < 0)
			{
				result = this.MapTable.CreateKnownTypeFromId(knownTypeIdFromType);
			}
			else
			{
				result = Activator.CreateInstance(t, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, TypeConverterHelper.InvariantEnglishUS);
			}
			return result;
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x00216C74 File Offset: 0x00215C74
		internal bool IsXmlNamespaceKnown(string xmlNamespace, out string newXmlNamespace)
		{
			bool result;
			if (string.IsNullOrEmpty(xmlNamespace))
			{
				result = false;
				newXmlNamespace = null;
			}
			else
			{
				NamespaceMapEntry[] namespaceMapEntries = this.GetNamespaceMapEntries(xmlNamespace);
				if (XamlTypeMapper._xmlnsCache == null)
				{
					XamlTypeMapper._xmlnsCache = new XmlnsCache();
				}
				newXmlNamespace = XamlTypeMapper._xmlnsCache.GetNewXmlnamespace(xmlNamespace);
				result = ((namespaceMapEntries != null && namespaceMapEntries.Length != 0) || !string.IsNullOrEmpty(newXmlNamespace));
			}
			return result;
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x00216CCC File Offset: 0x00215CCC
		internal void SetUriToAssemblyNameMapping(string xmlNamespace, short[] assemblyIds)
		{
			if (xmlNamespace.StartsWith("clr-namespace:", StringComparison.Ordinal))
			{
				return;
			}
			if (XamlTypeMapper._xmlnsCache == null)
			{
				XamlTypeMapper._xmlnsCache = new XmlnsCache();
			}
			string[] array = null;
			if (assemblyIds != null && assemblyIds.Length != 0)
			{
				array = new string[assemblyIds.Length];
				for (int i = 0; i < assemblyIds.Length; i++)
				{
					BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(assemblyIds[i]);
					array[i] = assemblyInfoFromId.AssemblyFullName;
				}
			}
			XamlTypeMapper._xmlnsCache.SetUriToAssemblyNameMapping(xmlNamespace, array);
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x00216D40 File Offset: 0x00215D40
		internal NamespaceMapEntry[] GetNamespaceMapEntries(string xmlNamespace)
		{
			NamespaceMapEntry[] array = this._namespaceMapHashList[xmlNamespace] as NamespaceMapEntry[];
			if (array == null)
			{
				ArrayList arrayList = new ArrayList(6);
				if (this._namespaceMaps != null)
				{
					for (int i = 0; i < this._namespaceMaps.Length; i++)
					{
						NamespaceMapEntry namespaceMapEntry = this._namespaceMaps[i];
						if (namespaceMapEntry.XmlNamespace == xmlNamespace)
						{
							arrayList.Add(namespaceMapEntry);
						}
					}
				}
				List<ClrNamespaceAssemblyPair> list;
				if (this.PITable.Contains(xmlNamespace))
				{
					list = new List<ClrNamespaceAssemblyPair>(1);
					list.Add((ClrNamespaceAssemblyPair)this.PITable[xmlNamespace]);
				}
				else
				{
					list = XamlTypeMapper.GetClrNamespacePairFromCache(xmlNamespace);
				}
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = list[j];
						string text = null;
						string assemblyPath = this.AssemblyPathFor(clrNamespaceAssemblyPair.AssemblyName);
						if (!string.IsNullOrEmpty(clrNamespaceAssemblyPair.AssemblyName) && !string.IsNullOrEmpty(clrNamespaceAssemblyPair.ClrNamespace))
						{
							text = clrNamespaceAssemblyPair.AssemblyName;
							NamespaceMapEntry value = new NamespaceMapEntry(xmlNamespace, text, clrNamespaceAssemblyPair.ClrNamespace, assemblyPath);
							arrayList.Add(value);
						}
						if (!string.IsNullOrEmpty(clrNamespaceAssemblyPair.ClrNamespace))
						{
							for (int k = 0; k < this._assemblyNames.Length; k++)
							{
								if (text == null)
								{
									arrayList.Add(new NamespaceMapEntry(xmlNamespace, this._assemblyNames[k], clrNamespaceAssemblyPair.ClrNamespace, assemblyPath));
								}
								else
								{
									int num = this._assemblyNames[k].LastIndexOf('\\');
									if (num > 0 && this._assemblyNames[k].Substring(num + 1) == text)
									{
										arrayList.Add(new NamespaceMapEntry(xmlNamespace, this._assemblyNames[k], clrNamespaceAssemblyPair.ClrNamespace, assemblyPath));
									}
								}
							}
						}
					}
				}
				array = (NamespaceMapEntry[])arrayList.ToArray(typeof(NamespaceMapEntry));
				if (array != null)
				{
					this._namespaceMapHashList.Add(xmlNamespace, array);
				}
			}
			return array;
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x00216F28 File Offset: 0x00215F28
		internal string GetXmlNamespace(string clrNamespaceFullName, string assemblyFullName)
		{
			string str = assemblyFullName.ToUpper(TypeConverterHelper.InvariantEnglishUS);
			string key = clrNamespaceFullName + "#" + str;
			string text;
			if (this._piReverseTable.TryGetValue(key, out text) && text != null)
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x00216F68 File Offset: 0x00215F68
		private string GetCachedNamespace(Type t)
		{
			return this.GetCachedInformationForType(t).ClrNamespace;
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x00216F76 File Offset: 0x00215F76
		internal static List<ClrNamespaceAssemblyPair> GetClrNamespacePairFromCache(string namespaceUri)
		{
			if (XamlTypeMapper._xmlnsCache == null)
			{
				XamlTypeMapper._xmlnsCache = new XmlnsCache();
			}
			return XamlTypeMapper._xmlnsCache.GetMappingArray(namespaceUri);
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x00216F94 File Offset: 0x00215F94
		internal Type GetTypeConverterType(Type type)
		{
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(type);
			if (null != cachedInformationForType.TypeConverterType)
			{
				return cachedInformationForType.TypeConverterType;
			}
			Type type2 = this.MapTable.GetKnownConverterTypeFromType(type);
			if (type2 == null)
			{
				type2 = TypeConverterHelper.GetConverterType(type);
				if (type2 == null)
				{
					type2 = TypeConverterHelper.GetCoreConverterTypeFromCustomType(type);
				}
			}
			cachedInformationForType.TypeConverterType = type2;
			return type2;
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x00216FF8 File Offset: 0x00215FF8
		internal TypeConverter GetTypeConverter(Type type)
		{
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(type);
			if (cachedInformationForType.Converter != null)
			{
				return cachedInformationForType.Converter;
			}
			TypeConverter typeConverter = this.MapTable.GetKnownConverterFromType(type);
			if (typeConverter == null)
			{
				Type converterType = TypeConverterHelper.GetConverterType(type);
				if (converterType == null)
				{
					typeConverter = TypeConverterHelper.GetCoreConverterFromCustomType(type);
				}
				else
				{
					typeConverter = (this.CreateInstance(converterType) as TypeConverter);
				}
			}
			cachedInformationForType.Converter = typeConverter;
			if (typeConverter == null)
			{
				this.ThrowException("ParserNoTypeConv", type.Name);
			}
			return typeConverter;
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x00217074 File Offset: 0x00216074
		internal Type GetPropertyConverterType(Type propType, object dpOrPiOrMi)
		{
			Type result = null;
			if (dpOrPiOrMi != null)
			{
				MemberInfo memberInfoForPropertyConverter = TypeConverterHelper.GetMemberInfoForPropertyConverter(dpOrPiOrMi);
				if (memberInfoForPropertyConverter != null)
				{
					result = TypeConverterHelper.GetConverterType(memberInfoForPropertyConverter);
				}
			}
			return result;
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x002170A0 File Offset: 0x002160A0
		internal TypeConverter GetPropertyConverter(Type propType, object dpOrPiOrMi)
		{
			TypeConverter typeConverter = null;
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(propType);
			if (dpOrPiOrMi != null)
			{
				object obj = cachedInformationForType.PropertyConverters[dpOrPiOrMi];
				if (obj != null)
				{
					return (TypeConverter)obj;
				}
				MemberInfo memberInfoForPropertyConverter = TypeConverterHelper.GetMemberInfoForPropertyConverter(dpOrPiOrMi);
				if (memberInfoForPropertyConverter != null)
				{
					Type converterType = TypeConverterHelper.GetConverterType(memberInfoForPropertyConverter);
					if (converterType != null)
					{
						typeConverter = (this.CreateInstance(converterType) as TypeConverter);
					}
				}
			}
			if (typeConverter == null)
			{
				typeConverter = this.GetTypeConverter(propType);
			}
			if (dpOrPiOrMi != null)
			{
				cachedInformationForType.SetPropertyConverter(dpOrPiOrMi, typeConverter);
			}
			return typeConverter;
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x0021711A File Offset: 0x0021611A
		internal object GetDictionaryKey(string keyString, ParserContext context)
		{
			if (keyString.Length > 0 && (char.IsWhiteSpace(keyString[0]) || char.IsWhiteSpace(keyString[keyString.Length - 1])))
			{
				keyString = keyString.Trim();
			}
			return keyString;
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x00217154 File Offset: 0x00216154
		internal XamlTypeMapper.ConstructorData GetConstructors(Type type)
		{
			if (this._constructorInformationCache == null)
			{
				this._constructorInformationCache = new HybridDictionary(3);
			}
			if (!this._constructorInformationCache.Contains(type))
			{
				this._constructorInformationCache[type] = new XamlTypeMapper.ConstructorData(type.GetConstructors(BindingFlags.Instance | BindingFlags.Public));
			}
			return (XamlTypeMapper.ConstructorData)this._constructorInformationCache[type];
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x002171B0 File Offset: 0x002161B0
		internal bool GetCachedTrimSurroundingWhitespace(Type t)
		{
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(t);
			if (!cachedInformationForType.TrimSurroundingWhitespaceSet)
			{
				cachedInformationForType.TrimSurroundingWhitespace = this.GetTrimSurroundingWhitespace(t);
				cachedInformationForType.TrimSurroundingWhitespaceSet = true;
			}
			return cachedInformationForType.TrimSurroundingWhitespace;
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x002171E7 File Offset: 0x002161E7
		private bool GetTrimSurroundingWhitespace(Type type)
		{
			return null != type && (type.GetCustomAttributes(typeof(TrimSurroundingWhitespaceAttribute), true) as TrimSurroundingWhitespaceAttribute[]).Length != 0;
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0021720E File Offset: 0x0021620E
		private void ThrowException(string id)
		{
			this.ThrowExceptionWithLine(SR.Get(id), null);
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0021721D File Offset: 0x0021621D
		internal void ThrowException(string id, string parameter)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter
			}), null);
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x00217236 File Offset: 0x00216236
		private void ThrowException(string id, string parameter1, string parameter2)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter1,
				parameter2
			}), null);
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x00217253 File Offset: 0x00216253
		private void ThrowException(string id, string parameter1, string parameter2, string parameter3)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter1,
				parameter2,
				parameter3
			}), null);
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x00217275 File Offset: 0x00216275
		internal void ThrowExceptionWithLine(string message, Exception innerException)
		{
			XamlParseException.ThrowException(message, innerException, this._lineNumber, this._linePosition);
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06004118 RID: 16664 RVA: 0x0021728A File Offset: 0x0021628A
		internal HybridDictionary PITable
		{
			get
			{
				return this._piTable;
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06004119 RID: 16665 RVA: 0x00217292 File Offset: 0x00216292
		// (set) Token: 0x0600411A RID: 16666 RVA: 0x0021729A File Offset: 0x0021629A
		internal BamlMapTable MapTable
		{
			get
			{
				return this._mapTable;
			}
			set
			{
				this._mapTable = value;
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (set) Token: 0x0600411B RID: 16667 RVA: 0x002172A3 File Offset: 0x002162A3
		internal int LineNumber
		{
			set
			{
				this._lineNumber = value;
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (set) Token: 0x0600411C RID: 16668 RVA: 0x002172AC File Offset: 0x002162AC
		internal int LinePosition
		{
			set
			{
				this._linePosition = value;
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x0600411D RID: 16669 RVA: 0x002172B5 File Offset: 0x002162B5
		internal Hashtable NamespaceMapHashList
		{
			get
			{
				return this._namespaceMapHashList;
			}
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x0600411E RID: 16670 RVA: 0x002172BD File Offset: 0x002162BD
		internal XamlSchemaContext SchemaContext
		{
			get
			{
				if (this._schemaContext == null)
				{
					this._schemaContext = new XamlTypeMapper.XamlTypeMapperSchemaContext(this);
				}
				return this._schemaContext;
			}
		}

		// Token: 0x04002486 RID: 9350
		internal const string MarkupExtensionTypeString = "Type ";

		// Token: 0x04002487 RID: 9351
		internal const string MarkupExtensionStaticString = "Static ";

		// Token: 0x04002488 RID: 9352
		internal const string MarkupExtensionDynamicResourceString = "DynamicResource ";

		// Token: 0x04002489 RID: 9353
		internal const string PresentationFrameworkDllName = "PresentationFramework";

		// Token: 0x0400248A RID: 9354
		internal const string GeneratedNamespace = "XamlGeneratedNamespace";

		// Token: 0x0400248B RID: 9355
		internal const string GeneratedInternalTypeHelperClassName = "GeneratedInternalTypeHelper";

		// Token: 0x0400248C RID: 9356
		internal const string MarkupExtensionTemplateBindingString = "TemplateBinding ";

		// Token: 0x0400248D RID: 9357
		private BamlMapTable _mapTable;

		// Token: 0x0400248E RID: 9358
		private string[] _assemblyNames;

		// Token: 0x0400248F RID: 9359
		private NamespaceMapEntry[] _namespaceMaps;

		// Token: 0x04002490 RID: 9360
		private Hashtable _typeLookupFromXmlHashtable = new Hashtable();

		// Token: 0x04002491 RID: 9361
		private Hashtable _namespaceMapHashList = new Hashtable();

		// Token: 0x04002492 RID: 9362
		private HybridDictionary _typeInformationCache = new HybridDictionary();

		// Token: 0x04002493 RID: 9363
		private HybridDictionary _constructorInformationCache;

		// Token: 0x04002494 RID: 9364
		private XamlTypeMapper.XamlTypeMapperSchemaContext _schemaContext;

		// Token: 0x04002495 RID: 9365
		private HybridDictionary _piTable = new HybridDictionary();

		// Token: 0x04002496 RID: 9366
		private Dictionary<string, string> _piReverseTable = new Dictionary<string, string>();

		// Token: 0x04002497 RID: 9367
		private HybridDictionary _assemblyPathTable = new HybridDictionary();

		// Token: 0x04002498 RID: 9368
		private bool _referenceAssembliesLoaded;

		// Token: 0x04002499 RID: 9369
		private int _lineNumber;

		// Token: 0x0400249A RID: 9370
		private int _linePosition;

		// Token: 0x0400249B RID: 9371
		private static XmlnsCache _xmlnsCache;

		// Token: 0x02000B00 RID: 2816
		internal class ConstructorData
		{
			// Token: 0x06008BA8 RID: 35752 RVA: 0x0033A20F File Offset: 0x0033920F
			internal ConstructorData(ConstructorInfo[] constructors)
			{
				this._constructors = constructors;
			}

			// Token: 0x06008BA9 RID: 35753 RVA: 0x0033A220 File Offset: 0x00339220
			internal ParameterInfo[] GetParameters(int constructorIndex)
			{
				if (this._parameters == null)
				{
					this._parameters = new ParameterInfo[this._constructors.Length][];
				}
				if (this._parameters[constructorIndex] == null)
				{
					this._parameters[constructorIndex] = this._constructors[constructorIndex].GetParameters();
				}
				return this._parameters[constructorIndex];
			}

			// Token: 0x17001E9C RID: 7836
			// (get) Token: 0x06008BAA RID: 35754 RVA: 0x0033A26F File Offset: 0x0033926F
			internal ConstructorInfo[] Constructors
			{
				get
				{
					return this._constructors;
				}
			}

			// Token: 0x04004762 RID: 18274
			private ConstructorInfo[] _constructors;

			// Token: 0x04004763 RID: 18275
			private ParameterInfo[][] _parameters;
		}

		// Token: 0x02000B01 RID: 2817
		internal class TypeInformationCacheData
		{
			// Token: 0x06008BAB RID: 35755 RVA: 0x0033A277 File Offset: 0x00339277
			internal TypeInformationCacheData(Type baseType)
			{
				this._baseType = baseType;
			}

			// Token: 0x17001E9D RID: 7837
			// (get) Token: 0x06008BAC RID: 35756 RVA: 0x0033A291 File Offset: 0x00339291
			// (set) Token: 0x06008BAD RID: 35757 RVA: 0x0033A299 File Offset: 0x00339299
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

			// Token: 0x17001E9E RID: 7838
			// (get) Token: 0x06008BAE RID: 35758 RVA: 0x0033A2A2 File Offset: 0x003392A2
			internal Type BaseType
			{
				get
				{
					return this._baseType;
				}
			}

			// Token: 0x17001E9F RID: 7839
			// (get) Token: 0x06008BAF RID: 35759 RVA: 0x0033A2AA File Offset: 0x003392AA
			// (set) Token: 0x06008BB0 RID: 35760 RVA: 0x0033A2B2 File Offset: 0x003392B2
			internal TypeConverter Converter
			{
				get
				{
					return this._typeConverter;
				}
				set
				{
					this._typeConverter = value;
				}
			}

			// Token: 0x17001EA0 RID: 7840
			// (get) Token: 0x06008BB1 RID: 35761 RVA: 0x0033A2BB File Offset: 0x003392BB
			// (set) Token: 0x06008BB2 RID: 35762 RVA: 0x0033A2C3 File Offset: 0x003392C3
			internal Type TypeConverterType
			{
				get
				{
					return this._typeConverterType;
				}
				set
				{
					this._typeConverterType = value;
				}
			}

			// Token: 0x17001EA1 RID: 7841
			// (get) Token: 0x06008BB3 RID: 35763 RVA: 0x0033A2CC File Offset: 0x003392CC
			// (set) Token: 0x06008BB4 RID: 35764 RVA: 0x0033A2D4 File Offset: 0x003392D4
			internal bool TrimSurroundingWhitespace
			{
				get
				{
					return this._trimSurroundingWhitespace;
				}
				set
				{
					this._trimSurroundingWhitespace = value;
				}
			}

			// Token: 0x17001EA2 RID: 7842
			// (get) Token: 0x06008BB5 RID: 35765 RVA: 0x0033A2DD File Offset: 0x003392DD
			// (set) Token: 0x06008BB6 RID: 35766 RVA: 0x0033A2E5 File Offset: 0x003392E5
			internal bool TrimSurroundingWhitespaceSet
			{
				get
				{
					return this._trimSurroundingWhitespaceSet;
				}
				set
				{
					this._trimSurroundingWhitespaceSet = value;
				}
			}

			// Token: 0x06008BB7 RID: 35767 RVA: 0x0033A2EE File Offset: 0x003392EE
			internal XamlTypeMapper.PropertyAndType GetPropertyAndType(string dpName)
			{
				if (this._dpLookupHashtable == null)
				{
					this._dpLookupHashtable = new Hashtable();
					return null;
				}
				return this._dpLookupHashtable[dpName] as XamlTypeMapper.PropertyAndType;
			}

			// Token: 0x06008BB8 RID: 35768 RVA: 0x0033A318 File Offset: 0x00339318
			internal void SetPropertyAndType(string dpName, PropertyInfo dpInfo, Type ownerType, bool isInternal)
			{
				XamlTypeMapper.PropertyAndType propertyAndType = this._dpLookupHashtable[dpName] as XamlTypeMapper.PropertyAndType;
				if (propertyAndType == null)
				{
					this._dpLookupHashtable[dpName] = new XamlTypeMapper.PropertyAndType(null, dpInfo, false, true, ownerType, isInternal);
					return;
				}
				propertyAndType.PropInfo = dpInfo;
				propertyAndType.PropInfoSet = true;
				propertyAndType.IsInternal = isInternal;
			}

			// Token: 0x17001EA3 RID: 7843
			// (get) Token: 0x06008BB9 RID: 35769 RVA: 0x0033A369 File Offset: 0x00339369
			internal HybridDictionary PropertyConverters
			{
				get
				{
					if (this._propertyConverters == null)
					{
						this._propertyConverters = new HybridDictionary();
					}
					return this._propertyConverters;
				}
			}

			// Token: 0x06008BBA RID: 35770 RVA: 0x0033A384 File Offset: 0x00339384
			internal void SetPropertyConverter(object dpOrPi, TypeConverter converter)
			{
				this._propertyConverters[dpOrPi] = converter;
			}

			// Token: 0x04004764 RID: 18276
			private string _clrNamespace;

			// Token: 0x04004765 RID: 18277
			private Type _baseType;

			// Token: 0x04004766 RID: 18278
			private bool _trimSurroundingWhitespace;

			// Token: 0x04004767 RID: 18279
			private Hashtable _dpLookupHashtable;

			// Token: 0x04004768 RID: 18280
			private HybridDictionary _propertyConverters = new HybridDictionary();

			// Token: 0x04004769 RID: 18281
			private bool _trimSurroundingWhitespaceSet;

			// Token: 0x0400476A RID: 18282
			private TypeConverter _typeConverter;

			// Token: 0x0400476B RID: 18283
			private Type _typeConverterType;
		}

		// Token: 0x02000B02 RID: 2818
		internal class PropertyAndType
		{
			// Token: 0x06008BBB RID: 35771 RVA: 0x0033A393 File Offset: 0x00339393
			public PropertyAndType(MethodInfo dpSetter, PropertyInfo dpInfo, bool setterSet, bool propInfoSet, Type ot, bool isInternal)
			{
				this.Setter = dpSetter;
				this.PropInfo = dpInfo;
				this.OwnerType = ot;
				this.SetterSet = setterSet;
				this.PropInfoSet = propInfoSet;
				this.IsInternal = isInternal;
			}

			// Token: 0x0400476C RID: 18284
			public PropertyInfo PropInfo;

			// Token: 0x0400476D RID: 18285
			public MethodInfo Setter;

			// Token: 0x0400476E RID: 18286
			public Type OwnerType;

			// Token: 0x0400476F RID: 18287
			public bool PropInfoSet;

			// Token: 0x04004770 RID: 18288
			public bool SetterSet;

			// Token: 0x04004771 RID: 18289
			public bool IsInternal;
		}

		// Token: 0x02000B03 RID: 2819
		internal class XamlTypeMapperSchemaContext : XamlSchemaContext
		{
			// Token: 0x06008BBC RID: 35772 RVA: 0x0033A3C8 File Offset: 0x003393C8
			internal XamlTypeMapperSchemaContext(XamlTypeMapper typeMapper)
			{
				this._typeMapper = typeMapper;
				this._sharedSchemaContext = (WpfSharedXamlSchemaContext)XamlReader.GetWpfSchemaContext();
				if (typeMapper._namespaceMaps != null)
				{
					this._nsDefinitions = new Dictionary<string, FrugalObjectList<string>>();
					foreach (NamespaceMapEntry namespaceMapEntry in this._typeMapper._namespaceMaps)
					{
						FrugalObjectList<string> frugalObjectList;
						if (!this._nsDefinitions.TryGetValue(namespaceMapEntry.XmlNamespace, out frugalObjectList))
						{
							frugalObjectList = new FrugalObjectList<string>(1);
							this._nsDefinitions.Add(namespaceMapEntry.XmlNamespace, frugalObjectList);
						}
						string clrNsUri = XamlTypeMapper.XamlTypeMapperSchemaContext.GetClrNsUri(namespaceMapEntry.ClrNamespace, namespaceMapEntry.AssemblyName);
						frugalObjectList.Add(clrNsUri);
					}
				}
				if (typeMapper.PITable.Count > 0)
				{
					this._piNamespaces = new Dictionary<string, string>(typeMapper.PITable.Count);
					foreach (object obj in typeMapper.PITable)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = (ClrNamespaceAssemblyPair)dictionaryEntry.Value;
						string clrNsUri2 = XamlTypeMapper.XamlTypeMapperSchemaContext.GetClrNsUri(clrNamespaceAssemblyPair.ClrNamespace, clrNamespaceAssemblyPair.AssemblyName);
						this._piNamespaces.Add((string)dictionaryEntry.Key, clrNsUri2);
					}
				}
				this._clrNamespaces = new HashSet<string>();
			}

			// Token: 0x06008BBD RID: 35773 RVA: 0x0033A534 File Offset: 0x00339534
			public override IEnumerable<string> GetAllXamlNamespaces()
			{
				IEnumerable<string> enumerable = this._allXamlNamespaces;
				if (enumerable == null)
				{
					object obj = this.syncObject;
					lock (obj)
					{
						if (this._nsDefinitions != null || this._piNamespaces != null)
						{
							List<string> list = new List<string>(this._sharedSchemaContext.GetAllXamlNamespaces());
							this.AddKnownNamespaces(list);
							enumerable = list.AsReadOnly();
						}
						else
						{
							enumerable = this._sharedSchemaContext.GetAllXamlNamespaces();
						}
						this._allXamlNamespaces = enumerable;
					}
				}
				return enumerable;
			}

			// Token: 0x06008BBE RID: 35774 RVA: 0x0033A5C0 File Offset: 0x003395C0
			public override XamlType GetXamlType(Type type)
			{
				if (ReflectionHelper.IsPublicType(type))
				{
					return this._sharedSchemaContext.GetXamlType(type);
				}
				return this.GetInternalType(type, null);
			}

			// Token: 0x06008BBF RID: 35775 RVA: 0x0033A5E0 File Offset: 0x003395E0
			public override bool TryGetCompatibleXamlNamespace(string xamlNamespace, out string compatibleNamespace)
			{
				if (this._sharedSchemaContext.TryGetCompatibleXamlNamespace(xamlNamespace, out compatibleNamespace))
				{
					return true;
				}
				if ((this._nsDefinitions != null && this._nsDefinitions.ContainsKey(xamlNamespace)) || (this._piNamespaces != null && this.SyncContainsKey<string, string>(this._piNamespaces, xamlNamespace)))
				{
					compatibleNamespace = xamlNamespace;
					return true;
				}
				return false;
			}

			// Token: 0x06008BC0 RID: 35776 RVA: 0x0033A634 File Offset: 0x00339634
			internal Hashtable GetNamespaceMapHashList()
			{
				Hashtable hashtable = new Hashtable();
				if (this._typeMapper._namespaceMaps != null)
				{
					foreach (NamespaceMapEntry namespaceMapEntry in this._typeMapper._namespaceMaps)
					{
						NamespaceMapEntry value = new NamespaceMapEntry
						{
							XmlNamespace = namespaceMapEntry.XmlNamespace,
							ClrNamespace = namespaceMapEntry.ClrNamespace,
							AssemblyName = namespaceMapEntry.AssemblyName,
							AssemblyPath = namespaceMapEntry.AssemblyPath
						};
						XamlTypeMapper.XamlTypeMapperSchemaContext.AddToMultiHashtable<string, NamespaceMapEntry>(hashtable, namespaceMapEntry.XmlNamespace, value);
					}
				}
				foreach (object obj in this._typeMapper.PITable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = (ClrNamespaceAssemblyPair)dictionaryEntry.Value;
					NamespaceMapEntry namespaceMapEntry2 = new NamespaceMapEntry
					{
						XmlNamespace = (string)dictionaryEntry.Key,
						ClrNamespace = clrNamespaceAssemblyPair.ClrNamespace,
						AssemblyName = clrNamespaceAssemblyPair.AssemblyName,
						AssemblyPath = this._typeMapper.AssemblyPathFor(clrNamespaceAssemblyPair.AssemblyName)
					};
					XamlTypeMapper.XamlTypeMapperSchemaContext.AddToMultiHashtable<string, NamespaceMapEntry>(hashtable, namespaceMapEntry2.XmlNamespace, namespaceMapEntry2);
				}
				object obj2 = this.syncObject;
				lock (obj2)
				{
					foreach (string xmlNamespace in this._clrNamespaces)
					{
						string clrNamespace;
						string text;
						XamlTypeMapper.XamlTypeMapperSchemaContext.SplitClrNsUri(xmlNamespace, out clrNamespace, out text);
						if (!string.IsNullOrEmpty(text))
						{
							string text2 = this._typeMapper.AssemblyPathFor(text);
							if (!string.IsNullOrEmpty(text2))
							{
								NamespaceMapEntry namespaceMapEntry3 = new NamespaceMapEntry
								{
									XmlNamespace = xmlNamespace,
									ClrNamespace = clrNamespace,
									AssemblyName = text,
									AssemblyPath = text2
								};
								XamlTypeMapper.XamlTypeMapperSchemaContext.AddToMultiHashtable<string, NamespaceMapEntry>(hashtable, namespaceMapEntry3.XmlNamespace, namespaceMapEntry3);
							}
						}
					}
				}
				object[] array = new object[hashtable.Count];
				hashtable.Keys.CopyTo(array, 0);
				foreach (object key in array)
				{
					List<NamespaceMapEntry> list = (List<NamespaceMapEntry>)hashtable[key];
					hashtable[key] = list.ToArray();
				}
				return hashtable;
			}

			// Token: 0x06008BC1 RID: 35777 RVA: 0x0033A894 File Offset: 0x00339894
			internal void SetMappingProcessingInstruction(string xamlNamespace, ClrNamespaceAssemblyPair pair)
			{
				string clrNsUri = XamlTypeMapper.XamlTypeMapperSchemaContext.GetClrNsUri(pair.ClrNamespace, pair.AssemblyName);
				object obj = this.syncObject;
				lock (obj)
				{
					if (this._piNamespaces == null)
					{
						this._piNamespaces = new Dictionary<string, string>();
					}
					this._piNamespaces[xamlNamespace] = clrNsUri;
					this._allXamlNamespaces = null;
				}
			}

			// Token: 0x06008BC2 RID: 35778 RVA: 0x0033A90C File Offset: 0x0033990C
			protected override XamlType GetXamlType(string xamlNamespace, string name, params XamlType[] typeArguments)
			{
				XamlType result;
				try
				{
					result = this.LookupXamlType(xamlNamespace, name, typeArguments);
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex))
					{
						throw;
					}
					if (!this._typeMapper.LoadReferenceAssemblies())
					{
						throw;
					}
					result = this.LookupXamlType(xamlNamespace, name, typeArguments);
				}
				return result;
			}

			// Token: 0x06008BC3 RID: 35779 RVA: 0x0033A95C File Offset: 0x0033995C
			protected override Assembly OnAssemblyResolve(string assemblyName)
			{
				string text = this._typeMapper.AssemblyPathFor(assemblyName);
				if (!string.IsNullOrEmpty(text))
				{
					return ReflectionHelper.LoadAssembly(assemblyName, text);
				}
				return base.OnAssemblyResolve(assemblyName);
			}

			// Token: 0x06008BC4 RID: 35780 RVA: 0x0033A98D File Offset: 0x0033998D
			private static string GetClrNsUri(string clrNamespace, string assembly)
			{
				return "clr-namespace:" + clrNamespace + ";assembly=" + assembly;
			}

			// Token: 0x06008BC5 RID: 35781 RVA: 0x0033A9A0 File Offset: 0x003399A0
			private static void SplitClrNsUri(string xmlNamespace, out string clrNamespace, out string assembly)
			{
				clrNamespace = null;
				assembly = null;
				int num = xmlNamespace.IndexOf("clr-namespace:", StringComparison.Ordinal);
				if (num < 0)
				{
					return;
				}
				num += "clr-namespace:".Length;
				if (num <= xmlNamespace.Length)
				{
					return;
				}
				int num2 = xmlNamespace.IndexOf(";assembly=", StringComparison.Ordinal);
				if (num2 < num)
				{
					clrNamespace = xmlNamespace.Substring(num);
					return;
				}
				clrNamespace = xmlNamespace.Substring(num, num2 - num);
				num2 += ";assembly=".Length;
				if (num2 <= xmlNamespace.Length)
				{
					return;
				}
				assembly = xmlNamespace.Substring(num2);
			}

			// Token: 0x06008BC6 RID: 35782 RVA: 0x0033AA24 File Offset: 0x00339A24
			private void AddKnownNamespaces(List<string> nsList)
			{
				if (this._nsDefinitions != null)
				{
					foreach (string item in this._nsDefinitions.Keys)
					{
						if (!nsList.Contains(item))
						{
							nsList.Add(item);
						}
					}
				}
				if (this._piNamespaces != null)
				{
					foreach (string item2 in this._piNamespaces.Keys)
					{
						if (!nsList.Contains(item2))
						{
							nsList.Add(item2);
						}
					}
				}
			}

			// Token: 0x06008BC7 RID: 35783 RVA: 0x0033AAE8 File Offset: 0x00339AE8
			private XamlType GetInternalType(Type type, XamlType sharedSchemaXamlType)
			{
				object obj = this.syncObject;
				XamlType result;
				lock (obj)
				{
					if (this._allowedInternalTypes == null)
					{
						this._allowedInternalTypes = new Dictionary<Type, XamlType>();
					}
					XamlType xamlType;
					if (!this._allowedInternalTypes.TryGetValue(type, out xamlType))
					{
						WpfSharedXamlSchemaContext.RequireRuntimeType(type);
						if (this._typeMapper.IsInternalTypeAllowedInFullTrust(type))
						{
							xamlType = new XamlTypeMapper.VisibilityMaskingXamlType(type, this._sharedSchemaContext);
						}
						else
						{
							xamlType = (sharedSchemaXamlType ?? this._sharedSchemaContext.GetXamlType(type));
						}
						this._allowedInternalTypes.Add(type, xamlType);
					}
					result = xamlType;
				}
				return result;
			}

			// Token: 0x06008BC8 RID: 35784 RVA: 0x0033AB8C File Offset: 0x00339B8C
			private XamlType LookupXamlType(string xamlNamespace, string name, XamlType[] typeArguments)
			{
				FrugalObjectList<string> frugalObjectList;
				XamlType xamlType;
				if (this._nsDefinitions != null && this._nsDefinitions.TryGetValue(xamlNamespace, out frugalObjectList))
				{
					for (int i = 0; i < frugalObjectList.Count; i++)
					{
						xamlType = base.GetXamlType(frugalObjectList[i], name, typeArguments);
						if (xamlType != null)
						{
							return xamlType;
						}
					}
				}
				string xamlNamespace2;
				if (this._piNamespaces != null && this.SyncTryGetValue(this._piNamespaces, xamlNamespace, out xamlNamespace2))
				{
					return base.GetXamlType(xamlNamespace2, name, typeArguments);
				}
				if (xamlNamespace.StartsWith("clr-namespace:", StringComparison.Ordinal))
				{
					object obj = this.syncObject;
					lock (obj)
					{
						if (!this._clrNamespaces.Contains(xamlNamespace))
						{
							this._clrNamespaces.Add(xamlNamespace);
						}
					}
					return base.GetXamlType(xamlNamespace, name, typeArguments);
				}
				xamlType = this._sharedSchemaContext.GetXamlTypeInternal(xamlNamespace, name, typeArguments);
				if (!(xamlType == null) && !xamlType.IsPublic)
				{
					return this.GetInternalType(xamlType.UnderlyingType, xamlType);
				}
				return xamlType;
			}

			// Token: 0x06008BC9 RID: 35785 RVA: 0x0033AC94 File Offset: 0x00339C94
			private bool SyncContainsKey<K, V>(IDictionary<K, V> dict, K key)
			{
				object obj = this.syncObject;
				bool result;
				lock (obj)
				{
					result = dict.ContainsKey(key);
				}
				return result;
			}

			// Token: 0x06008BCA RID: 35786 RVA: 0x0033ACD8 File Offset: 0x00339CD8
			private bool SyncTryGetValue(IDictionary<string, string> dict, string key, out string value)
			{
				object obj = this.syncObject;
				bool result;
				lock (obj)
				{
					result = dict.TryGetValue(key, out value);
				}
				return result;
			}

			// Token: 0x06008BCB RID: 35787 RVA: 0x0033AD1C File Offset: 0x00339D1C
			private static void AddToMultiHashtable<K, V>(Hashtable hashtable, K key, V value)
			{
				List<V> list = (List<V>)hashtable[key];
				if (list == null)
				{
					list = new List<V>();
					hashtable.Add(key, list);
				}
				list.Add(value);
			}

			// Token: 0x04004772 RID: 18290
			private Dictionary<string, FrugalObjectList<string>> _nsDefinitions;

			// Token: 0x04004773 RID: 18291
			private XamlTypeMapper _typeMapper;

			// Token: 0x04004774 RID: 18292
			private WpfSharedXamlSchemaContext _sharedSchemaContext;

			// Token: 0x04004775 RID: 18293
			private object syncObject = new object();

			// Token: 0x04004776 RID: 18294
			private Dictionary<string, string> _piNamespaces;

			// Token: 0x04004777 RID: 18295
			private IEnumerable<string> _allXamlNamespaces;

			// Token: 0x04004778 RID: 18296
			private Dictionary<Type, XamlType> _allowedInternalTypes;

			// Token: 0x04004779 RID: 18297
			private HashSet<string> _clrNamespaces;
		}

		// Token: 0x02000B04 RID: 2820
		private class VisibilityMaskingXamlType : XamlType
		{
			// Token: 0x06008BCC RID: 35788 RVA: 0x0033AD58 File Offset: 0x00339D58
			public VisibilityMaskingXamlType(Type underlyingType, XamlSchemaContext schemaContext) : base(underlyingType, schemaContext)
			{
			}

			// Token: 0x06008BCD RID: 35789 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			protected override bool LookupIsPublic()
			{
				return true;
			}
		}
	}
}
