using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x02000351 RID: 849
	[ContentProperty("Setters")]
	[XamlSetMarkupExtension("ReceiveMarkupExtension")]
	public class DataTrigger : TriggerBase, IAddChild
	{
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06002038 RID: 8248 RVA: 0x00174BAA File Offset: 0x00173BAA
		// (set) Token: 0x06002039 RID: 8249 RVA: 0x00174BB8 File Offset: 0x00173BB8
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public BindingBase Binding
		{
			get
			{
				base.VerifyAccess();
				return this._binding;
			}
			set
			{
				base.VerifyAccess();
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"DataTrigger"
					}));
				}
				this._binding = value;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600203A RID: 8250 RVA: 0x00174BED File Offset: 0x00173BED
		// (set) Token: 0x0600203B RID: 8251 RVA: 0x00174BFC File Offset: 0x00173BFC
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		[DependsOn("Binding")]
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
						"DataTrigger"
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

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600203C RID: 8252 RVA: 0x00174C80 File Offset: 0x00173C80
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

		// Token: 0x0600203D RID: 8253 RVA: 0x00174CA1 File Offset: 0x00173CA1
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			this.Setters.Add(Trigger.CheckChildIsSetter(value));
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x00174CBA File Offset: 0x00173CBA
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x00174CCC File Offset: 0x00173CCC
		internal sealed override void Seal()
		{
			if (base.IsSealed)
			{
				return;
			}
			base.ProcessSettersCollection(this._setters);
			StyleHelper.SealIfSealable(this._value);
			base.TriggerConditions = new TriggerCondition[]
			{
				new TriggerCondition(this._binding, LogicalOp.Equals, this._value)
			};
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				PropertyValue propertyValue = this.PropertyValues[i];
				propertyValue.Conditions = base.TriggerConditions;
				PropertyValueType valueType = propertyValue.ValueType;
				if (valueType != PropertyValueType.Trigger)
				{
					if (valueType != PropertyValueType.PropertyTriggerResource)
					{
						throw new InvalidOperationException(SR.Get("UnexpectedValueTypeForDataTrigger", new object[]
						{
							propertyValue.ValueType
						}));
					}
					propertyValue.ValueType = PropertyValueType.DataTriggerResource;
				}
				else
				{
					propertyValue.ValueType = PropertyValueType.DataTrigger;
				}
				this.PropertyValues[i] = propertyValue;
			}
			base.Seal();
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x00174DAD File Offset: 0x00173DAD
		internal override bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			return base.TriggerConditions[0].ConvertAndMatch(StyleHelper.GetDataTriggerValue(dataField, container, base.TriggerConditions[0].Binding));
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x00174DD8 File Offset: 0x00173DD8
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
			DataTrigger dataTrigger = targetObject as DataTrigger;
			if (dataTrigger != null && eventArgs.Member.Name == "Binding" && eventArgs.MarkupExtension is BindingBase)
			{
				dataTrigger.Binding = (eventArgs.MarkupExtension as BindingBase);
				eventArgs.Handled = true;
				return;
			}
			eventArgs.CallBase();
		}

		// Token: 0x04000FC0 RID: 4032
		private BindingBase _binding;

		// Token: 0x04000FC1 RID: 4033
		private object _value = DependencyProperty.UnsetValue;

		// Token: 0x04000FC2 RID: 4034
		private SetterBaseCollection _setters;
	}
}
