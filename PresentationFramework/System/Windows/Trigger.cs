using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x020003D7 RID: 983
	[XamlSetTypeConverter("ReceiveTypeConverter")]
	[ContentProperty("Setters")]
	public class Trigger : TriggerBase, IAddChild, ISupportInitialize
	{
		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x0600292C RID: 10540 RVA: 0x001990DD File Offset: 0x001980DD
		// (set) Token: 0x0600292D RID: 10541 RVA: 0x001990EB File Offset: 0x001980EB
		[Ambient]
		[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)]
		public DependencyProperty Property
		{
			get
			{
				base.VerifyAccess();
				return this._property;
			}
			set
			{
				base.VerifyAccess();
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Trigger"
					}));
				}
				this._property = value;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x0600292E RID: 10542 RVA: 0x00199120 File Offset: 0x00198120
		// (set) Token: 0x0600292F RID: 10543 RVA: 0x00199130 File Offset: 0x00198130
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		[DependsOn("SourceName")]
		[DependsOn("Property")]
		[TypeConverter(typeof(SetterTriggerConditionValueConverter))]
		public object Value
		{
			get
			{
				base.VerifyAccess();
				return this._value;
			}
			set
			{
				base.VerifyAccess();
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Trigger"
					}));
				}
				if (value is NullExtension)
				{
					value = null;
				}
				if (value is MarkupExtension)
				{
					throw new ArgumentException(SR.Get("ConditionValueOfMarkupExtensionNotSupported", new object[]
					{
						value.GetType().Name
					}));
				}
				if (value is Expression)
				{
					throw new ArgumentException(SR.Get("ConditionValueOfExpressionNotSupported"));
				}
				this._value = value;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06002930 RID: 10544 RVA: 0x001991BF File Offset: 0x001981BF
		// (set) Token: 0x06002931 RID: 10545 RVA: 0x001991CD File Offset: 0x001981CD
		[Ambient]
		[DefaultValue(null)]
		public string SourceName
		{
			get
			{
				base.VerifyAccess();
				return this._sourceName;
			}
			set
			{
				base.VerifyAccess();
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Trigger"
					}));
				}
				this._sourceName = value;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x00199202 File Offset: 0x00198202
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SetterBaseCollection Setters
		{
			get
			{
				base.VerifyAccess();
				if (this._setters == null)
				{
					this._setters = new SetterBaseCollection();
				}
				return this._setters;
			}
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x00199223 File Offset: 0x00198223
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			this.Setters.Add(Trigger.CheckChildIsSetter(value));
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x00174CBA File Offset: 0x00173CBA
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x0019923C File Offset: 0x0019823C
		internal static Setter CheckChildIsSetter(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			Setter setter = o as Setter;
			if (setter == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(Setter)
				}), "o");
			}
			return setter;
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x00199294 File Offset: 0x00198294
		internal sealed override void Seal()
		{
			if (base.IsSealed)
			{
				return;
			}
			if (this._property != null && !this._property.IsValidValue(this._value))
			{
				throw new InvalidOperationException(SR.Get("InvalidPropertyValue", new object[]
				{
					this._value,
					this._property.Name
				}));
			}
			StyleHelper.SealIfSealable(this._value);
			base.ProcessSettersCollection(this._setters);
			base.TriggerConditions = new TriggerCondition[]
			{
				new TriggerCondition(this._property, LogicalOp.Equals, this._value, (this._sourceName != null) ? this._sourceName : "~Self")
			};
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				PropertyValue value = this.PropertyValues[i];
				value.Conditions = base.TriggerConditions;
				this.PropertyValues[i] = value;
			}
			base.Seal();
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x00199386 File Offset: 0x00198386
		internal override bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			return base.TriggerConditions[0].Match(container.GetValue(base.TriggerConditions[0].Property));
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ISupportInitialize.BeginInit()
		{
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x001993B0 File Offset: 0x001983B0
		void ISupportInitialize.EndInit()
		{
			if (this._unresolvedProperty != null)
			{
				try
				{
					this.Property = DependencyPropertyConverter.ResolveProperty(this._serviceProvider, this.SourceName, this._unresolvedProperty);
				}
				finally
				{
					this._unresolvedProperty = null;
				}
			}
			if (this._unresolvedValue != null)
			{
				try
				{
					this.Value = SetterTriggerConditionValueConverter.ResolveValue(this._serviceProvider, this.Property, this._cultureInfoForTypeConverter, this._unresolvedValue);
				}
				finally
				{
					this._unresolvedValue = null;
				}
			}
			this._serviceProvider = null;
			this._cultureInfoForTypeConverter = null;
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x0019944C File Offset: 0x0019844C
		public static void ReceiveTypeConverter(object targetObject, XamlSetTypeConverterEventArgs eventArgs)
		{
			Trigger trigger = targetObject as Trigger;
			if (trigger == null)
			{
				throw new ArgumentNullException("targetObject");
			}
			if (eventArgs == null)
			{
				throw new ArgumentNullException("eventArgs");
			}
			if (eventArgs.Member.Name == "Property")
			{
				trigger._unresolvedProperty = eventArgs.Value;
				trigger._serviceProvider = eventArgs.ServiceProvider;
				trigger._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
				return;
			}
			if (eventArgs.Member.Name == "Value")
			{
				trigger._unresolvedValue = eventArgs.Value;
				trigger._serviceProvider = eventArgs.ServiceProvider;
				trigger._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
			}
		}

		// Token: 0x040014DE RID: 5342
		private DependencyProperty _property;

		// Token: 0x040014DF RID: 5343
		private object _value = DependencyProperty.UnsetValue;

		// Token: 0x040014E0 RID: 5344
		private string _sourceName;

		// Token: 0x040014E1 RID: 5345
		private SetterBaseCollection _setters;

		// Token: 0x040014E2 RID: 5346
		private object _unresolvedProperty;

		// Token: 0x040014E3 RID: 5347
		private object _unresolvedValue;

		// Token: 0x040014E4 RID: 5348
		private ITypeDescriptorContext _serviceProvider;

		// Token: 0x040014E5 RID: 5349
		private CultureInfo _cultureInfoForTypeConverter;
	}
}
