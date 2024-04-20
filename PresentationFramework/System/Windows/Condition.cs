using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x02000349 RID: 841
	[XamlSetMarkupExtension("ReceiveMarkupExtension")]
	[XamlSetTypeConverter("ReceiveTypeConverter")]
	public sealed class Condition : ISupportInitialize
	{
		// Token: 0x06001FF2 RID: 8178 RVA: 0x00173E08 File Offset: 0x00172E08
		public Condition()
		{
			this._property = null;
			this._binding = null;
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x00173E29 File Offset: 0x00172E29
		public Condition(DependencyProperty conditionProperty, object conditionValue) : this(conditionProperty, conditionValue, null)
		{
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x00173E34 File Offset: 0x00172E34
		public Condition(DependencyProperty conditionProperty, object conditionValue, string sourceName)
		{
			if (conditionProperty == null)
			{
				throw new ArgumentNullException("conditionProperty");
			}
			if (!conditionProperty.IsValidValue(conditionValue))
			{
				throw new ArgumentException(SR.Get("InvalidPropertyValue", new object[]
				{
					conditionValue,
					conditionProperty.Name
				}));
			}
			this._property = conditionProperty;
			this.Value = conditionValue;
			this._sourceName = sourceName;
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x00173EA1 File Offset: 0x00172EA1
		public Condition(BindingBase binding, object conditionValue)
		{
			if (binding == null)
			{
				throw new ArgumentNullException("binding");
			}
			this.Binding = binding;
			this.Value = conditionValue;
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x00173ED0 File Offset: 0x00172ED0
		// (set) Token: 0x06001FF7 RID: 8183 RVA: 0x00173ED8 File Offset: 0x00172ED8
		[Ambient]
		[DefaultValue(null)]
		public DependencyProperty Property
		{
			get
			{
				return this._property;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Condition"
					}));
				}
				if (this._binding != null)
				{
					throw new InvalidOperationException(SR.Get("ConditionCannotUseBothPropertyAndBinding"));
				}
				this._property = value;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x00173F2A File Offset: 0x00172F2A
		// (set) Token: 0x06001FF9 RID: 8185 RVA: 0x00173F34 File Offset: 0x00172F34
		[DefaultValue(null)]
		public BindingBase Binding
		{
			get
			{
				return this._binding;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Condition"
					}));
				}
				if (this._property != null)
				{
					throw new InvalidOperationException(SR.Get("ConditionCannotUseBothPropertyAndBinding"));
				}
				this._binding = value;
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001FFA RID: 8186 RVA: 0x00173F86 File Offset: 0x00172F86
		// (set) Token: 0x06001FFB RID: 8187 RVA: 0x00173F90 File Offset: 0x00172F90
		[TypeConverter(typeof(SetterTriggerConditionValueConverter))]
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Condition"
					}));
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

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001FFC RID: 8188 RVA: 0x0017400E File Offset: 0x0017300E
		// (set) Token: 0x06001FFD RID: 8189 RVA: 0x00174016 File Offset: 0x00173016
		[DefaultValue(null)]
		public string SourceName
		{
			get
			{
				return this._sourceName;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Condition"
					}));
				}
				this._sourceName = value;
			}
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x00174048 File Offset: 0x00173048
		internal void Seal(ValueLookupType type)
		{
			if (this._sealed)
			{
				return;
			}
			this._sealed = true;
			if (this._property != null && this._binding != null)
			{
				throw new InvalidOperationException(SR.Get("ConditionCannotUseBothPropertyAndBinding"));
			}
			if (type - ValueLookupType.Trigger > 1)
			{
				if (type - ValueLookupType.DataTrigger > 1)
				{
					throw new InvalidOperationException(SR.Get("UnexpectedValueTypeForCondition", new object[]
					{
						type
					}));
				}
				if (this._binding == null)
				{
					throw new InvalidOperationException(SR.Get("NullPropertyIllegal", new object[]
					{
						"Binding"
					}));
				}
			}
			else
			{
				if (this._property == null)
				{
					throw new InvalidOperationException(SR.Get("NullPropertyIllegal", new object[]
					{
						"Property"
					}));
				}
				if (!this._property.IsValidValue(this._value))
				{
					throw new InvalidOperationException(SR.Get("InvalidPropertyValue", new object[]
					{
						this._value,
						this._property.Name
					}));
				}
			}
			StyleHelper.SealIfSealable(this._value);
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ISupportInitialize.BeginInit()
		{
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x0017414C File Offset: 0x0017314C
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

		// Token: 0x06002001 RID: 8193 RVA: 0x001741E8 File Offset: 0x001731E8
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
			Condition condition = targetObject as Condition;
			if (condition != null && eventArgs.Member.Name == "Binding" && eventArgs.MarkupExtension is BindingBase)
			{
				condition.Binding = (eventArgs.MarkupExtension as BindingBase);
				eventArgs.Handled = true;
			}
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x00174258 File Offset: 0x00173258
		public static void ReceiveTypeConverter(object targetObject, XamlSetTypeConverterEventArgs eventArgs)
		{
			Condition condition = targetObject as Condition;
			if (condition == null)
			{
				throw new ArgumentNullException("targetObject");
			}
			if (eventArgs == null)
			{
				throw new ArgumentNullException("eventArgs");
			}
			if (eventArgs.Member.Name == "Property")
			{
				condition._unresolvedProperty = eventArgs.Value;
				condition._serviceProvider = eventArgs.ServiceProvider;
				condition._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
				return;
			}
			if (eventArgs.Member.Name == "Value")
			{
				condition._unresolvedValue = eventArgs.Value;
				condition._serviceProvider = eventArgs.ServiceProvider;
				condition._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
			}
		}

		// Token: 0x04000FAD RID: 4013
		private bool _sealed;

		// Token: 0x04000FAE RID: 4014
		private DependencyProperty _property;

		// Token: 0x04000FAF RID: 4015
		private BindingBase _binding;

		// Token: 0x04000FB0 RID: 4016
		private object _value = DependencyProperty.UnsetValue;

		// Token: 0x04000FB1 RID: 4017
		private string _sourceName;

		// Token: 0x04000FB2 RID: 4018
		private object _unresolvedProperty;

		// Token: 0x04000FB3 RID: 4019
		private object _unresolvedValue;

		// Token: 0x04000FB4 RID: 4020
		private ITypeDescriptorContext _serviceProvider;

		// Token: 0x04000FB5 RID: 4021
		private CultureInfo _cultureInfoForTypeConverter;
	}
}
