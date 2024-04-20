using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000415 RID: 1045
	internal class WpfKnownMember : WpfXamlMember
	{
		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06003206 RID: 12806 RVA: 0x001D0D33 File Offset: 0x001CFD33
		// (set) Token: 0x06003207 RID: 12807 RVA: 0x001D0D41 File Offset: 0x001CFD41
		private bool Frozen
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 1);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 1, value);
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003208 RID: 12808 RVA: 0x001D0D50 File Offset: 0x001CFD50
		// (set) Token: 0x06003209 RID: 12809 RVA: 0x001D0D5E File Offset: 0x001CFD5E
		private bool ReadOnly
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 4);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 4, value);
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x001D0D73 File Offset: 0x001CFD73
		// (set) Token: 0x0600320B RID: 12811 RVA: 0x001D0D81 File Offset: 0x001CFD81
		public bool HasSpecialTypeConverter
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 2);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 2, value);
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x001D0D96 File Offset: 0x001CFD96
		// (set) Token: 0x0600320D RID: 12813 RVA: 0x001D0DA4 File Offset: 0x001CFDA4
		public bool Ambient
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

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x001D0DB9 File Offset: 0x001CFDB9
		// (set) Token: 0x0600320F RID: 12815 RVA: 0x001D0DC8 File Offset: 0x001CFDC8
		public bool IsReadPrivate
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

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06003210 RID: 12816 RVA: 0x001D0DDE File Offset: 0x001CFDDE
		// (set) Token: 0x06003211 RID: 12817 RVA: 0x001D0DED File Offset: 0x001CFDED
		public bool IsWritePrivate
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

		// Token: 0x06003212 RID: 12818 RVA: 0x001D0E03 File Offset: 0x001CFE03
		public WpfKnownMember(XamlSchemaContext schema, XamlType declaringType, string name, DependencyProperty dProperty, bool isReadOnly, bool isAttachable) : base(dProperty, isAttachable)
		{
			base.DependencyProperty = dProperty;
			this.ReadOnly = isReadOnly;
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x001D0E1F File Offset: 0x001CFE1F
		public WpfKnownMember(XamlSchemaContext schema, XamlType declaringType, string name, Type type, bool isReadOnly, bool isAttachable) : base(name, declaringType, isAttachable)
		{
			this._type = type;
			this.ReadOnly = isReadOnly;
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool LookupIsUnknown()
		{
			return false;
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x001D0E3B File Offset: 0x001CFE3B
		public void Freeze()
		{
			this.Frozen = true;
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x001D0E44 File Offset: 0x001CFE44
		private void CheckFrozen()
		{
			if (this.Frozen)
			{
				throw new InvalidOperationException("Can't Assign to Known Member attributes");
			}
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x001D0E59 File Offset: 0x001CFE59
		protected override XamlMemberInvoker LookupInvoker()
		{
			return new WpfKnownMemberInvoker(this);
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x001D0E61 File Offset: 0x001CFE61
		// (set) Token: 0x06003219 RID: 12825 RVA: 0x001D0E69 File Offset: 0x001CFE69
		public Action<object, object> SetDelegate
		{
			get
			{
				return this._setDelegate;
			}
			set
			{
				this.CheckFrozen();
				this._setDelegate = value;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x0600321A RID: 12826 RVA: 0x001D0E78 File Offset: 0x001CFE78
		// (set) Token: 0x0600321B RID: 12827 RVA: 0x001D0E80 File Offset: 0x001CFE80
		public Func<object, object> GetDelegate
		{
			get
			{
				return this._getDelegate;
			}
			set
			{
				this.CheckFrozen();
				this._getDelegate = value;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x0600321C RID: 12828 RVA: 0x001D0E8F File Offset: 0x001CFE8F
		// (set) Token: 0x0600321D RID: 12829 RVA: 0x001D0E97 File Offset: 0x001CFE97
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

		// Token: 0x0600321E RID: 12830 RVA: 0x001D0EA8 File Offset: 0x001CFEA8
		protected override XamlValueConverter<TypeConverter> LookupTypeConverter()
		{
			WpfSharedBamlSchemaContext bamlSharedSchemaContext = System.Windows.Markup.XamlReader.BamlSharedSchemaContext;
			if (this.HasSpecialTypeConverter)
			{
				return bamlSharedSchemaContext.GetXamlType(this._typeConverterType).TypeConverter;
			}
			if (this._typeConverterType != null)
			{
				return bamlSharedSchemaContext.GetTypeConverter(this._typeConverterType);
			}
			return null;
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x0600321F RID: 12831 RVA: 0x001D0EF1 File Offset: 0x001CFEF1
		// (set) Token: 0x06003220 RID: 12832 RVA: 0x001D0EF9 File Offset: 0x001CFEF9
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

		// Token: 0x06003221 RID: 12833 RVA: 0x001D0F08 File Offset: 0x001CFF08
		protected override XamlValueConverter<XamlDeferringLoader> LookupDeferringLoader()
		{
			if (this._deferringLoader != null)
			{
				return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetDeferringLoader(this._deferringLoader);
			}
			return null;
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x001D0F2A File Offset: 0x001CFF2A
		protected override bool LookupIsReadOnly()
		{
			return this.ReadOnly;
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x001D0F32 File Offset: 0x001CFF32
		protected override XamlType LookupType()
		{
			if (base.DependencyProperty != null)
			{
				return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(base.DependencyProperty.PropertyType);
			}
			return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(this._type);
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x001D0F62 File Offset: 0x001CFF62
		protected override MemberInfo LookupUnderlyingMember()
		{
			return base.LookupUnderlyingMember();
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x001D0F6A File Offset: 0x001CFF6A
		protected override bool LookupIsAmbient()
		{
			return this.Ambient;
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x001D0F72 File Offset: 0x001CFF72
		protected override bool LookupIsWritePublic()
		{
			return !this.IsWritePrivate;
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x001D0F7D File Offset: 0x001CFF7D
		protected override bool LookupIsReadPublic()
		{
			return !this.IsReadPrivate;
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x000F93D3 File Offset: 0x000F83D3
		protected override WpfXamlMember GetAsContentProperty()
		{
			return this;
		}

		// Token: 0x04001BC9 RID: 7113
		private Action<object, object> _setDelegate;

		// Token: 0x04001BCA RID: 7114
		private Func<object, object> _getDelegate;

		// Token: 0x04001BCB RID: 7115
		private Type _deferringLoader;

		// Token: 0x04001BCC RID: 7116
		private Type _typeConverterType;

		// Token: 0x04001BCD RID: 7117
		private Type _type;

		// Token: 0x04001BCE RID: 7118
		private byte _bitField;

		// Token: 0x02000AB4 RID: 2740
		[Flags]
		private enum BoolMemberBits
		{
			// Token: 0x04004622 RID: 17954
			Frozen = 1,
			// Token: 0x04004623 RID: 17955
			HasSpecialTypeConverter = 2,
			// Token: 0x04004624 RID: 17956
			ReadOnly = 4,
			// Token: 0x04004625 RID: 17957
			Ambient = 8,
			// Token: 0x04004626 RID: 17958
			ReadPrivate = 16,
			// Token: 0x04004627 RID: 17959
			WritePrivate = 32
		}
	}
}
