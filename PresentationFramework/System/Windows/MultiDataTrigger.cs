using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x02000384 RID: 900
	[ContentProperty("Setters")]
	public sealed class MultiDataTrigger : TriggerBase, IAddChild
	{
		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002449 RID: 9289 RVA: 0x00181FEB File Offset: 0x00180FEB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ConditionCollection Conditions
		{
			get
			{
				base.VerifyAccess();
				return this._conditions;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x00181FF9 File Offset: 0x00180FF9
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

		// Token: 0x0600244B RID: 9291 RVA: 0x0018201A File Offset: 0x0018101A
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			this.Setters.Add(Trigger.CheckChildIsSetter(value));
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x00174CBA File Offset: 0x00173CBA
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x00182034 File Offset: 0x00181034
		internal override void Seal()
		{
			if (base.IsSealed)
			{
				return;
			}
			base.ProcessSettersCollection(this._setters);
			if (this._conditions.Count > 0)
			{
				this._conditions.Seal(ValueLookupType.DataTrigger);
			}
			base.TriggerConditions = new TriggerCondition[this._conditions.Count];
			for (int i = 0; i < base.TriggerConditions.Length; i++)
			{
				if (this._conditions[i].SourceName != null && this._conditions[i].SourceName.Length > 0)
				{
					throw new InvalidOperationException(SR.Get("SourceNameNotSupportedForDataTriggers"));
				}
				base.TriggerConditions[i] = new TriggerCondition(this._conditions[i].Binding, LogicalOp.Equals, this._conditions[i].Value);
			}
			for (int j = 0; j < this.PropertyValues.Count; j++)
			{
				PropertyValue propertyValue = this.PropertyValues[j];
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
				this.PropertyValues[j] = propertyValue;
			}
			base.Seal();
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x00182198 File Offset: 0x00181198
		internal override bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			bool flag = base.TriggerConditions.Length != 0;
			int num = 0;
			while (flag && num < base.TriggerConditions.Length)
			{
				flag = base.TriggerConditions[num].ConvertAndMatch(StyleHelper.GetDataTriggerValue(dataField, container, base.TriggerConditions[num].Binding));
				num++;
			}
			return flag;
		}

		// Token: 0x04001139 RID: 4409
		private ConditionCollection _conditions = new ConditionCollection();

		// Token: 0x0400113A RID: 4410
		private SetterBaseCollection _setters;
	}
}
