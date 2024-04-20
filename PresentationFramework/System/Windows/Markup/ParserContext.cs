using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Xml;
using MS.Internal;
using MS.Internal.Xaml.Parser;

namespace System.Windows.Markup
{
	// Token: 0x020004CB RID: 1227
	public class ParserContext : IUriContext
	{
		// Token: 0x06003ECD RID: 16077 RVA: 0x0020F6EC File Offset: 0x0020E6EC
		public ParserContext()
		{
			this.Initialize();
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x0020F710 File Offset: 0x0020E710
		internal void Initialize()
		{
			this._xmlnsDictionary = null;
			this._nameScopeStack = null;
			this._xmlLang = string.Empty;
			this._xmlSpace = string.Empty;
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0020F738 File Offset: 0x0020E738
		public ParserContext(XmlParserContext xmlParserContext)
		{
			if (xmlParserContext == null)
			{
				throw new ArgumentNullException("xmlParserContext");
			}
			this._xmlLang = xmlParserContext.XmlLang;
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(XmlSpace));
			if (converter != null)
			{
				this._xmlSpace = converter.ConvertToString(null, TypeConverterHelper.InvariantEnglishUS, xmlParserContext.XmlSpace);
			}
			else
			{
				this._xmlSpace = string.Empty;
			}
			this._xmlnsDictionary = new XmlnsDictionary();
			if (xmlParserContext.BaseURI != null && xmlParserContext.BaseURI.Length > 0)
			{
				this._baseUri = new Uri(xmlParserContext.BaseURI, UriKind.RelativeOrAbsolute);
			}
			XmlNamespaceManager namespaceManager = xmlParserContext.NamespaceManager;
			if (namespaceManager != null)
			{
				foreach (object obj in namespaceManager)
				{
					string prefix = (string)obj;
					this._xmlnsDictionary.Add(prefix, namespaceManager.LookupNamespace(prefix));
				}
			}
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x0020F84C File Offset: 0x0020E84C
		internal ParserContext(XmlReader xmlReader)
		{
			if (xmlReader.BaseURI != null && xmlReader.BaseURI.Length != 0)
			{
				this.BaseUri = new Uri(xmlReader.BaseURI);
			}
			this.XmlLang = xmlReader.XmlLang;
			if (xmlReader.XmlSpace != System.Xml.XmlSpace.None)
			{
				this.XmlSpace = xmlReader.XmlSpace.ToString();
			}
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0020F8CC File Offset: 0x0020E8CC
		internal ParserContext(ParserContext parserContext)
		{
			this._xmlLang = parserContext.XmlLang;
			this._xmlSpace = parserContext.XmlSpace;
			this._xamlTypeMapper = parserContext.XamlTypeMapper;
			this._mapTable = parserContext.MapTable;
			this._baseUri = parserContext.BaseUri;
			this._masterBracketCharacterCache = parserContext.MasterBracketCharacterCache;
			this._rootElement = parserContext._rootElement;
			if (parserContext._nameScopeStack != null)
			{
				this._nameScopeStack = (Stack)parserContext._nameScopeStack.Clone();
			}
			else
			{
				this._nameScopeStack = null;
			}
			this._skipJournaledProperties = parserContext._skipJournaledProperties;
			this._xmlnsDictionary = null;
			if (parserContext._xmlnsDictionary != null && parserContext._xmlnsDictionary.Count > 0)
			{
				this._xmlnsDictionary = new XmlnsDictionary();
				XmlnsDictionary xmlnsDictionary = parserContext.XmlnsDictionary;
				if (xmlnsDictionary != null)
				{
					foreach (object obj in xmlnsDictionary.Keys)
					{
						string prefix = (string)obj;
						this._xmlnsDictionary[prefix] = xmlnsDictionary[prefix];
					}
				}
			}
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0020FA08 File Offset: 0x0020EA08
		internal Dictionary<string, SpecialBracketCharacters> InitBracketCharacterCacheForType(Type type)
		{
			if (!this.MasterBracketCharacterCache.ContainsKey(type))
			{
				Dictionary<string, SpecialBracketCharacters> value = this.BuildBracketCharacterCacheForType(type);
				this.MasterBracketCharacterCache.Add(type, value);
			}
			return this.MasterBracketCharacterCache[type];
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0020FA44 File Offset: 0x0020EA44
		internal void PushScope()
		{
			this._repeat++;
			this._currentFreezeStackFrame.IncrementRepeatCount();
			if (this._xmlnsDictionary != null)
			{
				this._xmlnsDictionary.PushScope();
			}
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x0020FA74 File Offset: 0x0020EA74
		internal void PopScope()
		{
			if (this._repeat > 0)
			{
				this._repeat--;
			}
			else if (this._langSpaceStack != null && this._langSpaceStack.Count > 0)
			{
				this._repeat = (int)this._langSpaceStack.Pop();
				this._targetType = (Type)this._langSpaceStack.Pop();
				this._xmlSpace = (string)this._langSpaceStack.Pop();
				this._xmlLang = (string)this._langSpaceStack.Pop();
			}
			if (!this._currentFreezeStackFrame.DecrementRepeatCount())
			{
				this._currentFreezeStackFrame = (ParserContext.FreezeStackFrame)this._freezeStack.Pop();
			}
			if (this._xmlnsDictionary != null)
			{
				this._xmlnsDictionary.PopScope();
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06003ED5 RID: 16085 RVA: 0x0020FB3E File Offset: 0x0020EB3E
		public XmlnsDictionary XmlnsDictionary
		{
			get
			{
				if (this._xmlnsDictionary == null)
				{
					this._xmlnsDictionary = new XmlnsDictionary();
				}
				return this._xmlnsDictionary;
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06003ED6 RID: 16086 RVA: 0x0020FB59 File Offset: 0x0020EB59
		// (set) Token: 0x06003ED7 RID: 16087 RVA: 0x0020FB61 File Offset: 0x0020EB61
		public string XmlLang
		{
			get
			{
				return this._xmlLang;
			}
			set
			{
				this.EndRepeat();
				this._xmlLang = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06003ED8 RID: 16088 RVA: 0x0020FB7A File Offset: 0x0020EB7A
		// (set) Token: 0x06003ED9 RID: 16089 RVA: 0x0020FB82 File Offset: 0x0020EB82
		public string XmlSpace
		{
			get
			{
				return this._xmlSpace;
			}
			set
			{
				this.EndRepeat();
				this._xmlSpace = value;
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06003EDA RID: 16090 RVA: 0x0020FB91 File Offset: 0x0020EB91
		// (set) Token: 0x06003EDB RID: 16091 RVA: 0x0020FB99 File Offset: 0x0020EB99
		internal Type TargetType
		{
			get
			{
				return this._targetType;
			}
			set
			{
				this.EndRepeat();
				this._targetType = value;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06003EDC RID: 16092 RVA: 0x0020FBA8 File Offset: 0x0020EBA8
		// (set) Token: 0x06003EDD RID: 16093 RVA: 0x0020FBB0 File Offset: 0x0020EBB0
		public XamlTypeMapper XamlTypeMapper
		{
			get
			{
				return this._xamlTypeMapper;
			}
			set
			{
				if (this._xamlTypeMapper != value)
				{
					this._xamlTypeMapper = value;
					this._mapTable = new BamlMapTable(value);
					this._xamlTypeMapper.MapTable = this._mapTable;
				}
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06003EDE RID: 16094 RVA: 0x0020FBDF File Offset: 0x0020EBDF
		internal Stack NameScopeStack
		{
			get
			{
				if (this._nameScopeStack == null)
				{
					this._nameScopeStack = new Stack(2);
				}
				return this._nameScopeStack;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06003EDF RID: 16095 RVA: 0x0020FBFB File Offset: 0x0020EBFB
		// (set) Token: 0x06003EE0 RID: 16096 RVA: 0x0020FC03 File Offset: 0x0020EC03
		public Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
			set
			{
				this._baseUri = value;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06003EE1 RID: 16097 RVA: 0x0020FC0C File Offset: 0x0020EC0C
		// (set) Token: 0x06003EE2 RID: 16098 RVA: 0x0020FC14 File Offset: 0x0020EC14
		internal bool SkipJournaledProperties
		{
			get
			{
				return this._skipJournaledProperties;
			}
			set
			{
				this._skipJournaledProperties = value;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06003EE3 RID: 16099 RVA: 0x0020FC1D File Offset: 0x0020EC1D
		// (set) Token: 0x06003EE4 RID: 16100 RVA: 0x0020FC2A File Offset: 0x0020EC2A
		internal Assembly StreamCreatedAssembly
		{
			get
			{
				return this._streamCreatedAssembly.Value;
			}
			set
			{
				this._streamCreatedAssembly.Value = value;
			}
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0020FC38 File Offset: 0x0020EC38
		public static implicit operator XmlParserContext(ParserContext parserContext)
		{
			return ParserContext.ToXmlParserContext(parserContext);
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0020FC40 File Offset: 0x0020EC40
		public static XmlParserContext ToXmlParserContext(ParserContext parserContext)
		{
			if (parserContext == null)
			{
				throw new ArgumentNullException("parserContext");
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
			XmlSpace xmlSpace = System.Xml.XmlSpace.None;
			if (parserContext.XmlSpace != null && parserContext.XmlSpace.Length != 0)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(XmlSpace));
				if (converter != null)
				{
					try
					{
						xmlSpace = (XmlSpace)converter.ConvertFromString(null, TypeConverterHelper.InvariantEnglishUS, parserContext.XmlSpace);
					}
					catch (FormatException)
					{
						xmlSpace = System.Xml.XmlSpace.None;
					}
				}
			}
			if (parserContext._xmlnsDictionary != null)
			{
				foreach (object obj in parserContext._xmlnsDictionary.Keys)
				{
					string prefix = (string)obj;
					xmlNamespaceManager.AddNamespace(prefix, parserContext._xmlnsDictionary[prefix]);
				}
			}
			XmlParserContext xmlParserContext = new XmlParserContext(null, xmlNamespaceManager, parserContext.XmlLang, xmlSpace);
			if (parserContext.BaseUri == null)
			{
				xmlParserContext.BaseURI = null;
			}
			else
			{
				string components = new Uri(parserContext.BaseUri.GetComponents(UriComponents.SerializationInfoString, UriFormat.SafeUnescaped)).GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped);
				xmlParserContext.BaseURI = components;
			}
			return xmlParserContext;
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0020FD7C File Offset: 0x0020ED7C
		private void EndRepeat()
		{
			if (this._repeat > 0)
			{
				if (this._langSpaceStack == null)
				{
					this._langSpaceStack = new Stack(1);
				}
				this._langSpaceStack.Push(this.XmlLang);
				this._langSpaceStack.Push(this.XmlSpace);
				this._langSpaceStack.Push(this.TargetType);
				this._langSpaceStack.Push(this._repeat);
				this._repeat = 0;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06003EE8 RID: 16104 RVA: 0x0020FDF6 File Offset: 0x0020EDF6
		// (set) Token: 0x06003EE9 RID: 16105 RVA: 0x0020FDFE File Offset: 0x0020EDFE
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

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06003EEA RID: 16106 RVA: 0x0020FE07 File Offset: 0x0020EE07
		// (set) Token: 0x06003EEB RID: 16107 RVA: 0x0020FE0F File Offset: 0x0020EE0F
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

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06003EEC RID: 16108 RVA: 0x0020FE18 File Offset: 0x0020EE18
		// (set) Token: 0x06003EED RID: 16109 RVA: 0x0020FE20 File Offset: 0x0020EE20
		internal bool IsDebugBamlStream
		{
			get
			{
				return this._isDebugBamlStream;
			}
			set
			{
				this._isDebugBamlStream = value;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06003EEE RID: 16110 RVA: 0x0020FE29 File Offset: 0x0020EE29
		// (set) Token: 0x06003EEF RID: 16111 RVA: 0x0020FE31 File Offset: 0x0020EE31
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

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06003EF0 RID: 16112 RVA: 0x0020FE3A File Offset: 0x0020EE3A
		// (set) Token: 0x06003EF1 RID: 16113 RVA: 0x0020FE42 File Offset: 0x0020EE42
		internal bool OwnsBamlStream
		{
			get
			{
				return this._ownsBamlStream;
			}
			set
			{
				this._ownsBamlStream = value;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06003EF2 RID: 16114 RVA: 0x0020FE4B File Offset: 0x0020EE4B
		// (set) Token: 0x06003EF3 RID: 16115 RVA: 0x0020FE53 File Offset: 0x0020EE53
		internal BamlMapTable MapTable
		{
			get
			{
				return this._mapTable;
			}
			set
			{
				if (this._mapTable != value)
				{
					this._mapTable = value;
					this._xamlTypeMapper = this._mapTable.XamlTypeMapper;
					this._xamlTypeMapper.MapTable = this._mapTable;
				}
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06003EF4 RID: 16116 RVA: 0x0020FE87 File Offset: 0x0020EE87
		// (set) Token: 0x06003EF5 RID: 16117 RVA: 0x0020FE8F File Offset: 0x0020EE8F
		internal IStyleConnector StyleConnector
		{
			get
			{
				return this._styleConnector;
			}
			set
			{
				this._styleConnector = value;
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06003EF6 RID: 16118 RVA: 0x0020FE98 File Offset: 0x0020EE98
		internal ProvideValueServiceProvider ProvideValueProvider
		{
			get
			{
				if (this._provideValueServiceProvider == null)
				{
					this._provideValueServiceProvider = new ProvideValueServiceProvider(this);
				}
				return this._provideValueServiceProvider;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06003EF7 RID: 16119 RVA: 0x0020FEB4 File Offset: 0x0020EEB4
		internal List<object[]> StaticResourcesStack
		{
			get
			{
				if (this._staticResourcesStack == null)
				{
					this._staticResourcesStack = new List<object[]>();
				}
				return this._staticResourcesStack;
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06003EF8 RID: 16120 RVA: 0x0020FECF File Offset: 0x0020EECF
		internal bool InDeferredSection
		{
			get
			{
				return this._staticResourcesStack != null && this._staticResourcesStack.Count > 0;
			}
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x0020FEE9 File Offset: 0x0020EEE9
		internal ParserContext ScopedCopy()
		{
			return this.ScopedCopy(true);
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x0020FEF4 File Offset: 0x0020EEF4
		internal ParserContext ScopedCopy(bool copyNameScopeStack)
		{
			ParserContext parserContext = new ParserContext();
			parserContext._baseUri = this._baseUri;
			parserContext._skipJournaledProperties = this._skipJournaledProperties;
			parserContext._xmlLang = this._xmlLang;
			parserContext._xmlSpace = this._xmlSpace;
			parserContext._repeat = this._repeat;
			parserContext._lineNumber = this._lineNumber;
			parserContext._linePosition = this._linePosition;
			parserContext._isDebugBamlStream = this._isDebugBamlStream;
			parserContext._mapTable = this._mapTable;
			parserContext._xamlTypeMapper = this._xamlTypeMapper;
			parserContext._targetType = this._targetType;
			parserContext._streamCreatedAssembly.Value = this._streamCreatedAssembly.Value;
			parserContext._rootElement = this._rootElement;
			parserContext._styleConnector = this._styleConnector;
			if (this._nameScopeStack != null && copyNameScopeStack)
			{
				parserContext._nameScopeStack = ((this._nameScopeStack != null) ? ((Stack)this._nameScopeStack.Clone()) : null);
			}
			else
			{
				parserContext._nameScopeStack = null;
			}
			parserContext._langSpaceStack = ((this._langSpaceStack != null) ? ((Stack)this._langSpaceStack.Clone()) : null);
			if (this._xmlnsDictionary != null)
			{
				parserContext._xmlnsDictionary = new XmlnsDictionary(this._xmlnsDictionary);
			}
			else
			{
				parserContext._xmlnsDictionary = null;
			}
			parserContext._currentFreezeStackFrame = this._currentFreezeStackFrame;
			parserContext._freezeStack = ((this._freezeStack != null) ? ((Stack)this._freezeStack.Clone()) : null);
			return parserContext;
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x00210061 File Offset: 0x0020F061
		internal void TrimState()
		{
			if (this._nameScopeStack != null && this._nameScopeStack.Count == 0)
			{
				this._nameScopeStack = null;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06003EFC RID: 16124 RVA: 0x0021007F File Offset: 0x0020F07F
		internal Dictionary<Type, Dictionary<string, SpecialBracketCharacters>> MasterBracketCharacterCache
		{
			get
			{
				if (this._masterBracketCharacterCache == null)
				{
					this._masterBracketCharacterCache = new Dictionary<Type, Dictionary<string, SpecialBracketCharacters>>();
				}
				return this._masterBracketCharacterCache;
			}
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x0021009C File Offset: 0x0020F09C
		internal ParserContext Clone()
		{
			ParserContext parserContext = this.ScopedCopy();
			parserContext._mapTable = ((this._mapTable != null) ? this._mapTable.Clone() : null);
			parserContext._xamlTypeMapper = ((this._xamlTypeMapper != null) ? this._xamlTypeMapper.Clone() : null);
			parserContext._xamlTypeMapper.MapTable = parserContext._mapTable;
			parserContext._mapTable.XamlTypeMapper = parserContext._xamlTypeMapper;
			return parserContext;
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06003EFE RID: 16126 RVA: 0x0021010B File Offset: 0x0020F10B
		// (set) Token: 0x06003EFF RID: 16127 RVA: 0x00210118 File Offset: 0x0020F118
		internal bool FreezeFreezables
		{
			get
			{
				return this._currentFreezeStackFrame.FreezeFreezables;
			}
			set
			{
				if (value != this._currentFreezeStackFrame.FreezeFreezables)
				{
					this._currentFreezeStackFrame.DecrementRepeatCount();
					if (this._freezeStack == null)
					{
						this._freezeStack = new Stack();
					}
					this._freezeStack.Push(this._currentFreezeStackFrame);
					this._currentFreezeStackFrame.Reset(value);
				}
			}
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x00210174 File Offset: 0x0020F174
		internal bool TryCacheFreezable(string value, Freezable freezable)
		{
			if (this.FreezeFreezables && freezable.CanFreeze)
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

		// Token: 0x06003F01 RID: 16129 RVA: 0x002101C4 File Offset: 0x0020F1C4
		internal Freezable TryGetFreezable(string value)
		{
			Freezable result = null;
			if (this._freezeCache != null)
			{
				this._freezeCache.TryGetValue(value, out result);
			}
			return result;
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x002101EC File Offset: 0x0020F1EC
		private Dictionary<string, SpecialBracketCharacters> BuildBracketCharacterCacheForType(Type extensionType)
		{
			Dictionary<string, SpecialBracketCharacters> dictionary = new Dictionary<string, SpecialBracketCharacters>(StringComparer.OrdinalIgnoreCase);
			PropertyInfo[] properties = extensionType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			Type type = null;
			Type type2 = null;
			foreach (PropertyInfo propertyInfo in properties)
			{
				string name = propertyInfo.Name;
				string text = null;
				IEnumerable<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(propertyInfo);
				SpecialBracketCharacters specialBracketCharacters = null;
				foreach (CustomAttributeData customAttributeData in customAttributes)
				{
					Type attributeType = customAttributeData.AttributeType;
					Assembly assembly = attributeType.Assembly;
					if (type == null || type2 == null)
					{
						type = assembly.GetType("System.Windows.Markup.ConstructorArgumentAttribute");
						type2 = assembly.GetType("System.Windows.Markup.MarkupExtensionBracketCharactersAttribute");
					}
					if (attributeType.IsAssignableFrom(type))
					{
						text = (customAttributeData.ConstructorArguments[0].Value as string);
					}
					else if (attributeType.IsAssignableFrom(type2))
					{
						if (specialBracketCharacters == null)
						{
							specialBracketCharacters = new SpecialBracketCharacters();
						}
						specialBracketCharacters.AddBracketCharacters((char)customAttributeData.ConstructorArguments[0].Value, (char)customAttributeData.ConstructorArguments[1].Value);
					}
				}
				if (specialBracketCharacters != null)
				{
					specialBracketCharacters.EndInit();
					dictionary.Add(name, specialBracketCharacters);
					if (text != null)
					{
						dictionary.Add(text, specialBracketCharacters);
					}
				}
			}
			if (dictionary.Count != 0)
			{
				return dictionary;
			}
			return null;
		}

		// Token: 0x0400233A RID: 9018
		private XamlTypeMapper _xamlTypeMapper;

		// Token: 0x0400233B RID: 9019
		private Uri _baseUri;

		// Token: 0x0400233C RID: 9020
		private XmlnsDictionary _xmlnsDictionary;

		// Token: 0x0400233D RID: 9021
		private string _xmlLang = string.Empty;

		// Token: 0x0400233E RID: 9022
		private string _xmlSpace = string.Empty;

		// Token: 0x0400233F RID: 9023
		private Stack _langSpaceStack;

		// Token: 0x04002340 RID: 9024
		private int _repeat;

		// Token: 0x04002341 RID: 9025
		private Type _targetType;

		// Token: 0x04002342 RID: 9026
		private Dictionary<Type, Dictionary<string, SpecialBracketCharacters>> _masterBracketCharacterCache;

		// Token: 0x04002343 RID: 9027
		private bool _skipJournaledProperties;

		// Token: 0x04002344 RID: 9028
		private SecurityCriticalDataForSet<Assembly> _streamCreatedAssembly;

		// Token: 0x04002345 RID: 9029
		private bool _ownsBamlStream;

		// Token: 0x04002346 RID: 9030
		private ProvideValueServiceProvider _provideValueServiceProvider;

		// Token: 0x04002347 RID: 9031
		private IStyleConnector _styleConnector;

		// Token: 0x04002348 RID: 9032
		private Stack _nameScopeStack;

		// Token: 0x04002349 RID: 9033
		private List<object[]> _staticResourcesStack;

		// Token: 0x0400234A RID: 9034
		private object _rootElement;

		// Token: 0x0400234B RID: 9035
		private ParserContext.FreezeStackFrame _currentFreezeStackFrame;

		// Token: 0x0400234C RID: 9036
		private Dictionary<string, Freezable> _freezeCache;

		// Token: 0x0400234D RID: 9037
		private Stack _freezeStack;

		// Token: 0x0400234E RID: 9038
		private int _lineNumber;

		// Token: 0x0400234F RID: 9039
		private int _linePosition;

		// Token: 0x04002350 RID: 9040
		private BamlMapTable _mapTable;

		// Token: 0x04002351 RID: 9041
		private bool _isDebugBamlStream;

		// Token: 0x02000AF7 RID: 2807
		private struct FreezeStackFrame
		{
			// Token: 0x06008B8E RID: 35726 RVA: 0x00339F5F File Offset: 0x00338F5F
			internal void IncrementRepeatCount()
			{
				this._repeatCount++;
			}

			// Token: 0x06008B8F RID: 35727 RVA: 0x00339F6F File Offset: 0x00338F6F
			internal bool DecrementRepeatCount()
			{
				if (this._repeatCount > 0)
				{
					this._repeatCount--;
					return true;
				}
				return false;
			}

			// Token: 0x17001E98 RID: 7832
			// (get) Token: 0x06008B90 RID: 35728 RVA: 0x00339F8B File Offset: 0x00338F8B
			internal bool FreezeFreezables
			{
				get
				{
					return this._freezeFreezables;
				}
			}

			// Token: 0x06008B91 RID: 35729 RVA: 0x00339F93 File Offset: 0x00338F93
			internal void Reset(bool freezeFreezables)
			{
				this._freezeFreezables = freezeFreezables;
				this._repeatCount = 0;
			}

			// Token: 0x04004747 RID: 18247
			private bool _freezeFreezables;

			// Token: 0x04004748 RID: 18248
			private int _repeatCount;
		}
	}
}
