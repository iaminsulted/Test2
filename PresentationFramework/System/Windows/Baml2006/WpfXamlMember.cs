using System;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x0200041C RID: 1052
	internal class WpfXamlMember : XamlMember, IProvideValueTarget
	{
		// Token: 0x06003268 RID: 12904 RVA: 0x001D1AB8 File Offset: 0x001D0AB8
		public WpfXamlMember(DependencyProperty dp, bool isAttachable) : base(dp.Name, System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(dp.OwnerType), isAttachable)
		{
			this.DependencyProperty = dp;
			this._useV3Rules = true;
			this._isBamlMember = true;
			this._underlyingMemberIsKnown = false;
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x001D1AF3 File Offset: 0x001D0AF3
		public WpfXamlMember(RoutedEvent re, bool isAttachable) : base(re.Name, System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(re.OwnerType), isAttachable)
		{
			this.RoutedEvent = re;
			this._useV3Rules = true;
			this._isBamlMember = true;
			this._underlyingMemberIsKnown = false;
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x001D1B2E File Offset: 0x001D0B2E
		public WpfXamlMember(DependencyProperty dp, MethodInfo getter, MethodInfo setter, XamlSchemaContext schemaContext, bool useV3Rules) : base(dp.Name, getter, setter, schemaContext)
		{
			this.DependencyProperty = dp;
			this._useV3Rules = useV3Rules;
			this._underlyingMemberIsKnown = true;
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x001D1B56 File Offset: 0x001D0B56
		public WpfXamlMember(DependencyProperty dp, PropertyInfo property, XamlSchemaContext schemaContext, bool useV3Rules) : base(property, schemaContext)
		{
			this.DependencyProperty = dp;
			this._useV3Rules = useV3Rules;
			this._underlyingMemberIsKnown = true;
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x001D1B76 File Offset: 0x001D0B76
		public WpfXamlMember(RoutedEvent re, MethodInfo setter, XamlSchemaContext schemaContext, bool useV3Rules) : base(re.Name, setter, schemaContext)
		{
			this.RoutedEvent = re;
			this._useV3Rules = useV3Rules;
			this._underlyingMemberIsKnown = true;
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x001D1B9C File Offset: 0x001D0B9C
		public WpfXamlMember(RoutedEvent re, EventInfo eventInfo, XamlSchemaContext schemaContext, bool useV3Rules) : base(eventInfo, schemaContext)
		{
			this.RoutedEvent = re;
			this._useV3Rules = useV3Rules;
			this._underlyingMemberIsKnown = true;
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x001D1BBC File Offset: 0x001D0BBC
		protected WpfXamlMember(string name, XamlType declaringType, bool isAttachable) : base(name, declaringType, isAttachable)
		{
			this._useV3Rules = true;
			this._isBamlMember = true;
			this._underlyingMemberIsKnown = false;
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x0600326F RID: 12911 RVA: 0x001D1BDC File Offset: 0x001D0BDC
		// (set) Token: 0x06003270 RID: 12912 RVA: 0x001D1BE4 File Offset: 0x001D0BE4
		public DependencyProperty DependencyProperty { get; set; }

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003271 RID: 12913 RVA: 0x001D1BED File Offset: 0x001D0BED
		// (set) Token: 0x06003272 RID: 12914 RVA: 0x001D1BF5 File Offset: 0x001D0BF5
		public RoutedEvent RoutedEvent { get; set; }

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x001D1BFE File Offset: 0x001D0BFE
		// (set) Token: 0x06003274 RID: 12916 RVA: 0x001D1C0C File Offset: 0x001D0C0C
		internal bool ApplyGetterFallback
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 8);
			}
			private set
			{
				WpfXamlType.SetFlag(ref this._bitField, 8, value);
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003275 RID: 12917 RVA: 0x001D1C1B File Offset: 0x001D0C1B
		internal WpfXamlMember AsContentProperty
		{
			get
			{
				if (this._asContentProperty == null)
				{
					this._asContentProperty = this.GetAsContentProperty();
				}
				return this._asContentProperty;
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x001D1C40 File Offset: 0x001D0C40
		protected virtual WpfXamlMember GetAsContentProperty()
		{
			if (this.DependencyProperty == null)
			{
				return this;
			}
			WpfXamlMember wpfXamlMember;
			if (this._underlyingMemberIsKnown)
			{
				PropertyInfo propertyInfo = base.UnderlyingMember as PropertyInfo;
				if (propertyInfo == null)
				{
					return this;
				}
				wpfXamlMember = new WpfXamlMember(this.DependencyProperty, propertyInfo, base.DeclaringType.SchemaContext, this._useV3Rules);
			}
			else
			{
				wpfXamlMember = new WpfXamlMember(this.DependencyProperty, false);
			}
			wpfXamlMember.ApplyGetterFallback = true;
			return wpfXamlMember;
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x001D1CB0 File Offset: 0x001D0CB0
		protected override XamlType LookupType()
		{
			if (this.DependencyProperty != null)
			{
				if (this._isBamlMember)
				{
					return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(this.DependencyProperty.PropertyType);
				}
				return System.Windows.Markup.XamlReader.GetWpfSchemaContext().GetXamlType(this.DependencyProperty.PropertyType);
			}
			else
			{
				if (this.RoutedEvent == null)
				{
					return base.LookupType();
				}
				if (this._isBamlMember)
				{
					return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(this.RoutedEvent.HandlerType);
				}
				return System.Windows.Markup.XamlReader.GetWpfSchemaContext().GetXamlType(this.RoutedEvent.HandlerType);
			}
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x001D1D3C File Offset: 0x001D0D3C
		protected override MemberInfo LookupUnderlyingMember()
		{
			MemberInfo memberInfo = base.LookupUnderlyingMember();
			if (memberInfo == null && this.BaseUnderlyingMember != null)
			{
				memberInfo = this.BaseUnderlyingMember.UnderlyingMember;
			}
			this._underlyingMemberIsKnown = true;
			return memberInfo;
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x001D1D7C File Offset: 0x001D0D7C
		protected override MethodInfo LookupUnderlyingSetter()
		{
			MethodInfo methodInfo = base.LookupUnderlyingSetter();
			if (methodInfo == null && this.BaseUnderlyingMember != null)
			{
				methodInfo = this.BaseUnderlyingMember.Invoker.UnderlyingSetter;
			}
			this._underlyingMemberIsKnown = true;
			return methodInfo;
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x001D1DC0 File Offset: 0x001D0DC0
		protected override MethodInfo LookupUnderlyingGetter()
		{
			MethodInfo methodInfo = base.LookupUnderlyingGetter();
			if (methodInfo == null && this.BaseUnderlyingMember != null)
			{
				methodInfo = this.BaseUnderlyingMember.Invoker.UnderlyingGetter;
			}
			this._underlyingMemberIsKnown = true;
			return methodInfo;
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x001D1E04 File Offset: 0x001D0E04
		protected override bool LookupIsReadOnly()
		{
			if (this.DependencyProperty != null)
			{
				return this.DependencyProperty.ReadOnly;
			}
			return base.LookupIsReadOnly();
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x001D1E20 File Offset: 0x001D0E20
		protected override bool LookupIsEvent()
		{
			return this.RoutedEvent != null;
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x001D1E2D File Offset: 0x001D0E2D
		protected override XamlMemberInvoker LookupInvoker()
		{
			return new WpfMemberInvoker(this);
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool LookupIsUnknown()
		{
			return false;
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x001D1E35 File Offset: 0x001D0E35
		protected override XamlValueConverter<XamlDeferringLoader> LookupDeferringLoader()
		{
			if (this._useV3Rules)
			{
				return null;
			}
			return base.LookupDeferringLoader();
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003280 RID: 12928 RVA: 0x001D1E47 File Offset: 0x001D0E47
		// (set) Token: 0x06003281 RID: 12929 RVA: 0x001D1E55 File Offset: 0x001D0E55
		private bool _useV3Rules
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

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06003282 RID: 12930 RVA: 0x001D1E64 File Offset: 0x001D0E64
		// (set) Token: 0x06003283 RID: 12931 RVA: 0x001D1E72 File Offset: 0x001D0E72
		private bool _isBamlMember
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 2);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 2, value);
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06003284 RID: 12932 RVA: 0x001D1E81 File Offset: 0x001D0E81
		// (set) Token: 0x06003285 RID: 12933 RVA: 0x001D1E8F File Offset: 0x001D0E8F
		private bool _underlyingMemberIsKnown
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

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06003286 RID: 12934 RVA: 0x0012F160 File Offset: 0x0012E160
		object IProvideValueTarget.TargetObject
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06003287 RID: 12935 RVA: 0x001D1E9E File Offset: 0x001D0E9E
		object IProvideValueTarget.TargetProperty
		{
			get
			{
				if (this.DependencyProperty != null)
				{
					return this.DependencyProperty;
				}
				return base.UnderlyingMember;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06003288 RID: 12936 RVA: 0x001D1EB8 File Offset: 0x001D0EB8
		private XamlMember BaseUnderlyingMember
		{
			get
			{
				if (this._baseUnderlyingMember == null)
				{
					WpfXamlType wpfXamlType = base.DeclaringType as WpfXamlType;
					this._baseUnderlyingMember = wpfXamlType.FindBaseXamlMember(base.Name, base.IsAttachable);
					if (this._baseUnderlyingMember == null)
					{
						this._baseUnderlyingMember = wpfXamlType.FindBaseXamlMember(base.Name, !base.IsAttachable);
					}
				}
				return this._baseUnderlyingMember;
			}
		}

		// Token: 0x04001BF2 RID: 7154
		private byte _bitField;

		// Token: 0x04001BF3 RID: 7155
		private XamlMember _baseUnderlyingMember;

		// Token: 0x04001BF4 RID: 7156
		private WpfXamlMember _asContentProperty;

		// Token: 0x02000AB6 RID: 2742
		[Flags]
		private enum BoolMemberBits
		{
			// Token: 0x0400462E RID: 17966
			UseV3Rules = 1,
			// Token: 0x0400462F RID: 17967
			BamlMember = 2,
			// Token: 0x04004630 RID: 17968
			UnderlyingMemberIsKnown = 4,
			// Token: 0x04004631 RID: 17969
			ApplyGetterFallback = 8
		}
	}
}
