using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x02000398 RID: 920
	[XamlSetMarkupExtension("ReceiveMarkupExtension")]
	[XamlSetTypeConverter("ReceiveTypeConverter")]
	public class Setter : SetterBase, ISupportInitialize
	{
		// Token: 0x0600253C RID: 9532 RVA: 0x00185D96 File Offset: 0x00184D96
		public Setter()
		{
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x00185DA9 File Offset: 0x00184DA9
		public Setter(DependencyProperty property, object value)
		{
			this.Initialize(property, value, null);
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x00185DC5 File Offset: 0x00184DC5
		public Setter(DependencyProperty property, object value, string targetName)
		{
			this.Initialize(property, value, targetName);
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x00185DE1 File Offset: 0x00184DE1
		private void Initialize(DependencyProperty property, object value, string target)
		{
			if (value == DependencyProperty.UnsetValue)
			{
				throw new ArgumentException(SR.Get("SetterValueCannotBeUnset"));
			}
			this.CheckValidProperty(property);
			this._property = property;
			this._value = value;
			this._target = target;
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x00185E18 File Offset: 0x00184E18
		private void CheckValidProperty(DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			if (property.ReadOnly)
			{
				throw new ArgumentException(SR.Get("ReadOnlyPropertyNotAllowed", new object[]
				{
					property.Name,
					base.GetType().Name
				}));
			}
			if (property == FrameworkElement.NameProperty)
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInStyle", new object[]
				{
					FrameworkElement.NameProperty.Name
				}));
			}
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x00185E94 File Offset: 0x00184E94
		internal override void Seal()
		{
			DependencyProperty property = this.Property;
			object valueInternal = this.ValueInternal;
			if (property == null)
			{
				throw new ArgumentException(SR.Get("NullPropertyIllegal", new object[]
				{
					"Setter.Property"
				}));
			}
			if (string.IsNullOrEmpty(this.TargetName) && property == FrameworkElement.StyleProperty)
			{
				throw new ArgumentException(SR.Get("StylePropertyInStyleNotAllowed"));
			}
			if (!property.IsValidValue(valueInternal))
			{
				if (valueInternal is MarkupExtension)
				{
					if (!(valueInternal is DynamicResourceExtension) && !(valueInternal is BindingBase))
					{
						throw new ArgumentException(SR.Get("SetterValueOfMarkupExtensionNotSupported", new object[]
						{
							valueInternal.GetType().Name
						}));
					}
				}
				else if (!(valueInternal is DeferredReference))
				{
					throw new ArgumentException(SR.Get("InvalidSetterValue", new object[]
					{
						valueInternal,
						property.OwnerType,
						property.Name
					}));
				}
			}
			StyleHelper.SealIfSealable(this._value);
			base.Seal();
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002542 RID: 9538 RVA: 0x00185F7F File Offset: 0x00184F7F
		// (set) Token: 0x06002543 RID: 9539 RVA: 0x00185F87 File Offset: 0x00184F87
		[Ambient]
		[DefaultValue(null)]
		[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)]
		public DependencyProperty Property
		{
			get
			{
				return this._property;
			}
			set
			{
				this.CheckValidProperty(value);
				base.CheckSealed();
				this._property = value;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x00185FA0 File Offset: 0x00184FA0
		// (set) Token: 0x06002545 RID: 9541 RVA: 0x00185FCF File Offset: 0x00184FCF
		[DependsOn("Property")]
		[DependsOn("TargetName")]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		[TypeConverter(typeof(SetterTriggerConditionValueConverter))]
		public object Value
		{
			get
			{
				DeferredReference deferredReference = this._value as DeferredReference;
				if (deferredReference != null)
				{
					this._value = deferredReference.GetValue(BaseValueSourceInternal.Unknown);
				}
				return this._value;
			}
			set
			{
				if (value == DependencyProperty.UnsetValue)
				{
					throw new ArgumentException(SR.Get("SetterValueCannotBeUnset"));
				}
				base.CheckSealed();
				if (value is Expression)
				{
					throw new ArgumentException(SR.Get("StyleValueOfExpressionNotSupported"));
				}
				this._value = value;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x0018600E File Offset: 0x0018500E
		internal object ValueInternal
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002547 RID: 9543 RVA: 0x00186016 File Offset: 0x00185016
		// (set) Token: 0x06002548 RID: 9544 RVA: 0x0018601E File Offset: 0x0018501E
		[DefaultValue(null)]
		[Ambient]
		public string TargetName
		{
			get
			{
				return this._target;
			}
			set
			{
				base.CheckSealed();
				this._target = value;
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x00186030 File Offset: 0x00185030
		public static void ReceiveMarkupExtension(object targetObject, XamlSetMarkupExtensionEventArgs eventArgs)
		{
			if (targetObject == null)
			{
				throw new ArgumentNullException("targetObject");
			}
			if (eventArgs == null)
			{
				throw new ArgumentNullException("eventArgs");
			}
			Setter setter = targetObject as Setter;
			if (setter == null || eventArgs.Member.Name != "Value")
			{
				return;
			}
			MarkupExtension markupExtension = eventArgs.MarkupExtension;
			if (markupExtension is StaticResourceExtension)
			{
				StaticResourceExtension staticResourceExtension = markupExtension as StaticResourceExtension;
				setter.Value = staticResourceExtension.ProvideValueInternal(eventArgs.ServiceProvider, true);
				eventArgs.Handled = true;
				return;
			}
			if (markupExtension is DynamicResourceExtension || markupExtension is BindingBase)
			{
				setter.Value = markupExtension;
				eventArgs.Handled = true;
			}
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x001860CC File Offset: 0x001850CC
		public static void ReceiveTypeConverter(object targetObject, XamlSetTypeConverterEventArgs eventArgs)
		{
			Setter setter = targetObject as Setter;
			if (setter == null)
			{
				throw new ArgumentNullException("targetObject");
			}
			if (eventArgs == null)
			{
				throw new ArgumentNullException("eventArgs");
			}
			if (eventArgs.Member.Name == "Property")
			{
				setter._unresolvedProperty = eventArgs.Value;
				setter._serviceProvider = eventArgs.ServiceProvider;
				setter._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
				return;
			}
			if (eventArgs.Member.Name == "Value")
			{
				setter._unresolvedValue = eventArgs.Value;
				setter._serviceProvider = eventArgs.ServiceProvider;
				setter._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
			}
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ISupportInitialize.BeginInit()
		{
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x00186184 File Offset: 0x00185184
		void ISupportInitialize.EndInit()
		{
			if (this._unresolvedProperty != null)
			{
				try
				{
					this.Property = DependencyPropertyConverter.ResolveProperty(this._serviceProvider, this.TargetName, this._unresolvedProperty);
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

		// Token: 0x0400117D RID: 4477
		private DependencyProperty _property;

		// Token: 0x0400117E RID: 4478
		private object _value = DependencyProperty.UnsetValue;

		// Token: 0x0400117F RID: 4479
		private string _target;

		// Token: 0x04001180 RID: 4480
		private object _unresolvedProperty;

		// Token: 0x04001181 RID: 4481
		private object _unresolvedValue;

		// Token: 0x04001182 RID: 4482
		private ITypeDescriptorContext _serviceProvider;

		// Token: 0x04001183 RID: 4483
		private CultureInfo _cultureInfoForTypeConverter;
	}
}
