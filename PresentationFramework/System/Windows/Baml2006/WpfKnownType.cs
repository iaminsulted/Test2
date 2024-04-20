using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000417 RID: 1047
	internal class WpfKnownType : WpfXamlType, ICustomAttributeProvider
	{
		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x001D112D File Offset: 0x001D012D
		// (set) Token: 0x0600322E RID: 12846 RVA: 0x001D113B File Offset: 0x001D013B
		private bool Frozen
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 4);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 4, value);
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x001D114A File Offset: 0x001D014A
		// (set) Token: 0x06003230 RID: 12848 RVA: 0x001D1158 File Offset: 0x001D0158
		public bool WhitespaceSignificantCollection
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 8);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 8, value);
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06003231 RID: 12849 RVA: 0x001D116D File Offset: 0x001D016D
		// (set) Token: 0x06003232 RID: 12850 RVA: 0x001D117C File Offset: 0x001D017C
		public bool IsUsableDuringInit
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 16);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 16, value);
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x001D1192 File Offset: 0x001D0192
		// (set) Token: 0x06003234 RID: 12852 RVA: 0x001D11A1 File Offset: 0x001D01A1
		public bool HasSpecialValueConverter
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 32);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 32, value);
			}
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x001D11B7 File Offset: 0x001D01B7
		public WpfKnownType(XamlSchemaContext schema, int bamlNumber, string name, Type underlyingType) : this(schema, bamlNumber, name, underlyingType, true, true)
		{
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x001D11C6 File Offset: 0x001D01C6
		public WpfKnownType(XamlSchemaContext schema, int bamlNumber, string name, Type underlyingType, bool isBamlType, bool useV3Rules) : base(underlyingType, schema, isBamlType, useV3Rules)
		{
			this._bamlNumber = (short)bamlNumber;
			this._name = name;
			this._underlyingType = underlyingType;
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x001D11EC File Offset: 0x001D01EC
		public void Freeze()
		{
			this.Frozen = true;
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x001D11F5 File Offset: 0x001D01F5
		private void CheckFrozen()
		{
			if (this.Frozen)
			{
				throw new InvalidOperationException("Can't Assign to Known Type attributes");
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06003239 RID: 12857 RVA: 0x001D120A File Offset: 0x001D020A
		public short BamlNumber
		{
			get
			{
				return this._bamlNumber;
			}
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x001D1212 File Offset: 0x001D0212
		protected override XamlMember LookupContentProperty()
		{
			return this.CallGetMember(this._contentPropertyName);
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x001D1220 File Offset: 0x001D0220
		// (set) Token: 0x0600323C RID: 12860 RVA: 0x001D1228 File Offset: 0x001D0228
		public string ContentPropertyName
		{
			get
			{
				return this._contentPropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._contentPropertyName = value;
			}
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x001D1238 File Offset: 0x001D0238
		protected override XamlMember LookupAliasedProperty(XamlDirective directive)
		{
			if (directive == XamlLanguage.Name)
			{
				return this.CallGetMember(this._runtimeNamePropertyName);
			}
			if (directive == XamlLanguage.Key && this._dictionaryKeyPropertyName != null)
			{
				return this.LookupMember(this._dictionaryKeyPropertyName, true);
			}
			if (directive == XamlLanguage.Lang)
			{
				return this.CallGetMember(this._xmlLangPropertyName);
			}
			if (directive == XamlLanguage.Uid)
			{
				return this.CallGetMember(this._uidPropertyName);
			}
			return null;
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x0600323E RID: 12862 RVA: 0x001D12B7 File Offset: 0x001D02B7
		// (set) Token: 0x0600323F RID: 12863 RVA: 0x001D12BF File Offset: 0x001D02BF
		public string RuntimeNamePropertyName
		{
			get
			{
				return this._runtimeNamePropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._runtimeNamePropertyName = value;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x001D12CE File Offset: 0x001D02CE
		// (set) Token: 0x06003241 RID: 12865 RVA: 0x001D12D6 File Offset: 0x001D02D6
		public string XmlLangPropertyName
		{
			get
			{
				return this._xmlLangPropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._xmlLangPropertyName = value;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06003242 RID: 12866 RVA: 0x001D12E5 File Offset: 0x001D02E5
		// (set) Token: 0x06003243 RID: 12867 RVA: 0x001D12ED File Offset: 0x001D02ED
		public string UidPropertyName
		{
			get
			{
				return this._uidPropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._uidPropertyName = value;
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003244 RID: 12868 RVA: 0x001D12FC File Offset: 0x001D02FC
		// (set) Token: 0x06003245 RID: 12869 RVA: 0x001D1304 File Offset: 0x001D0304
		public string DictionaryKeyPropertyName
		{
			get
			{
				return this._dictionaryKeyPropertyName;
			}
			set
			{
				this.CheckFrozen();
				this._dictionaryKeyPropertyName = value;
			}
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x001D1313 File Offset: 0x001D0313
		protected override XamlCollectionKind LookupCollectionKind()
		{
			return this._collectionKind;
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003247 RID: 12871 RVA: 0x001D1313 File Offset: 0x001D0313
		// (set) Token: 0x06003248 RID: 12872 RVA: 0x001D131B File Offset: 0x001D031B
		public XamlCollectionKind CollectionKind
		{
			get
			{
				return this._collectionKind;
			}
			set
			{
				this.CheckFrozen();
				this._collectionKind = value;
			}
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x001D132A File Offset: 0x001D032A
		protected override bool LookupIsWhitespaceSignificantCollection()
		{
			return this.WhitespaceSignificantCollection;
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x0600324A RID: 12874 RVA: 0x001D1332 File Offset: 0x001D0332
		// (set) Token: 0x0600324B RID: 12875 RVA: 0x001D133A File Offset: 0x001D033A
		public Func<object> DefaultConstructor
		{
			get
			{
				return this._defaultConstructor;
			}
			set
			{
				this.CheckFrozen();
				this._defaultConstructor = value;
			}
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x001D134C File Offset: 0x001D034C
		protected override XamlValueConverter<TypeConverter> LookupTypeConverter()
		{
			WpfSharedBamlSchemaContext bamlSharedSchemaContext = System.Windows.Markup.XamlReader.BamlSharedSchemaContext;
			if (this._typeConverterType != null)
			{
				return bamlSharedSchemaContext.GetTypeConverter(this._typeConverterType);
			}
			if (this.HasSpecialValueConverter)
			{
				return base.LookupTypeConverter();
			}
			return null;
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x0600324D RID: 12877 RVA: 0x001D138A File Offset: 0x001D038A
		// (set) Token: 0x0600324E RID: 12878 RVA: 0x001D1392 File Offset: 0x001D0392
		public Type TypeConverterType
		{
			get
			{
				return this._typeConverterType;
			}
			set
			{
				this.CheckFrozen();
				this._typeConverterType = value;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x0600324F RID: 12879 RVA: 0x001D13A1 File Offset: 0x001D03A1
		// (set) Token: 0x06003250 RID: 12880 RVA: 0x001D13A9 File Offset: 0x001D03A9
		public Type DeferringLoaderType
		{
			get
			{
				return this._deferringLoader;
			}
			set
			{
				this.CheckFrozen();
				this._deferringLoader = value;
			}
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x001D13B8 File Offset: 0x001D03B8
		protected override XamlValueConverter<XamlDeferringLoader> LookupDeferringLoader()
		{
			if (this._deferringLoader != null)
			{
				return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetDeferringLoader(this._deferringLoader);
			}
			return null;
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x001D13DC File Offset: 0x001D03DC
		protected override EventHandler<XamlSetMarkupExtensionEventArgs> LookupSetMarkupExtensionHandler()
		{
			if (typeof(Setter).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(Setter.ReceiveMarkupExtension);
			}
			if (typeof(DataTrigger).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(DataTrigger.ReceiveMarkupExtension);
			}
			if (typeof(Condition).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(Condition.ReceiveMarkupExtension);
			}
			return null;
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x001D1458 File Offset: 0x001D0458
		protected override EventHandler<XamlSetTypeConverterEventArgs> LookupSetTypeConverterHandler()
		{
			if (typeof(Setter).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetTypeConverterEventArgs>(Setter.ReceiveTypeConverter);
			}
			if (typeof(Trigger).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetTypeConverterEventArgs>(Trigger.ReceiveTypeConverter);
			}
			if (typeof(Condition).IsAssignableFrom(this._underlyingType))
			{
				return new EventHandler<XamlSetTypeConverterEventArgs>(Condition.ReceiveTypeConverter);
			}
			return null;
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x001D14D2 File Offset: 0x001D04D2
		protected override bool LookupUsableDuringInitialization()
		{
			return this.IsUsableDuringInit;
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x001D14DA File Offset: 0x001D04DA
		protected override XamlTypeInvoker LookupInvoker()
		{
			return new WpfKnownTypeInvoker(this);
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x001D14E2 File Offset: 0x001D04E2
		private XamlMember CallGetMember(string name)
		{
			if (name != null)
			{
				return base.GetMember(name);
			}
			return null;
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x001D14F0 File Offset: 0x001D04F0
		public Dictionary<int, Baml6ConstructorInfo> Constructors
		{
			get
			{
				if (this._constructors == null)
				{
					this._constructors = new Dictionary<int, Baml6ConstructorInfo>();
				}
				return this._constructors;
			}
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x001D150C File Offset: 0x001D050C
		protected override IList<XamlType> LookupPositionalParameters(int paramCount)
		{
			if (base.IsMarkupExtension)
			{
				List<XamlType> list = null;
				Baml6ConstructorInfo baml6ConstructorInfo = this.Constructors[paramCount];
				if (this.Constructors.TryGetValue(paramCount, out baml6ConstructorInfo))
				{
					list = new List<XamlType>();
					foreach (Type type in baml6ConstructorInfo.Types)
					{
						list.Add(base.SchemaContext.GetXamlType(type));
					}
				}
				return list;
			}
			return base.LookupPositionalParameters(paramCount);
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x000F93D3 File Offset: 0x000F83D3
		protected override ICustomAttributeProvider LookupCustomAttributeProvider()
		{
			return this;
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x001D15A4 File Offset: 0x001D05A4
		object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit)
		{
			return base.UnderlyingType.GetCustomAttributes(inherit);
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x001D15B4 File Offset: 0x001D05B4
		object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit)
		{
			Attribute attribute;
			if (!this.TryGetCustomAttribute(attributeType, out attribute))
			{
				return base.UnderlyingType.GetCustomAttributes(attributeType, inherit);
			}
			if (attribute != null)
			{
				return new Attribute[]
				{
					attribute
				};
			}
			if (WpfKnownType.s_EmptyAttributes == null)
			{
				WpfKnownType.s_EmptyAttributes = Array.Empty<Attribute>();
			}
			return WpfKnownType.s_EmptyAttributes;
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x001D1604 File Offset: 0x001D0604
		private bool TryGetCustomAttribute(Type attributeType, out Attribute result)
		{
			bool result2 = true;
			if (attributeType == typeof(ContentPropertyAttribute))
			{
				result = ((this._contentPropertyName == null) ? null : new ContentPropertyAttribute(this._contentPropertyName));
			}
			else if (attributeType == typeof(RuntimeNamePropertyAttribute))
			{
				result = ((this._runtimeNamePropertyName == null) ? null : new RuntimeNamePropertyAttribute(this._runtimeNamePropertyName));
			}
			else if (attributeType == typeof(DictionaryKeyPropertyAttribute))
			{
				result = ((this._dictionaryKeyPropertyName == null) ? null : new DictionaryKeyPropertyAttribute(this._dictionaryKeyPropertyName));
			}
			else if (attributeType == typeof(XmlLangPropertyAttribute))
			{
				result = ((this._xmlLangPropertyName == null) ? null : new XmlLangPropertyAttribute(this._xmlLangPropertyName));
			}
			else if (attributeType == typeof(UidPropertyAttribute))
			{
				result = ((this._uidPropertyName == null) ? null : new UidPropertyAttribute(this._uidPropertyName));
			}
			else
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x001D16FB File Offset: 0x001D06FB
		bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit)
		{
			return base.UnderlyingType.IsDefined(attributeType, inherit);
		}

		// Token: 0x04001BD2 RID: 7122
		private static Attribute[] s_EmptyAttributes;

		// Token: 0x04001BD3 RID: 7123
		private short _bamlNumber;

		// Token: 0x04001BD4 RID: 7124
		private string _name;

		// Token: 0x04001BD5 RID: 7125
		private Type _underlyingType;

		// Token: 0x04001BD6 RID: 7126
		private string _contentPropertyName;

		// Token: 0x04001BD7 RID: 7127
		private string _runtimeNamePropertyName;

		// Token: 0x04001BD8 RID: 7128
		private string _dictionaryKeyPropertyName;

		// Token: 0x04001BD9 RID: 7129
		private string _xmlLangPropertyName;

		// Token: 0x04001BDA RID: 7130
		private string _uidPropertyName;

		// Token: 0x04001BDB RID: 7131
		private Func<object> _defaultConstructor;

		// Token: 0x04001BDC RID: 7132
		private Type _deferringLoader;

		// Token: 0x04001BDD RID: 7133
		private Type _typeConverterType;

		// Token: 0x04001BDE RID: 7134
		private XamlCollectionKind _collectionKind;

		// Token: 0x04001BDF RID: 7135
		private Dictionary<int, Baml6ConstructorInfo> _constructors;

		// Token: 0x02000AB5 RID: 2741
		[Flags]
		private enum BoolTypeBits
		{
			// Token: 0x04004629 RID: 17961
			Frozen = 4,
			// Token: 0x0400462A RID: 17962
			WhitespaceSignificantCollection = 8,
			// Token: 0x0400462B RID: 17963
			UsableDurintInit = 16,
			// Token: 0x0400462C RID: 17964
			HasSpecialValueConverter = 32
		}
	}
}
