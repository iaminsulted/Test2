using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x02000385 RID: 901
	[ContentProperty("Setters")]
	public sealed class MultiTrigger : TriggerBase, IAddChild
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06002450 RID: 9296 RVA: 0x00182204 File Offset: 0x00181204
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ConditionCollection Conditions
		{
			get
			{
				base.VerifyAccess();
				return this._conditions;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x00182212 File Offset: 0x00181212
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

		// Token: 0x06002452 RID: 9298 RVA: 0x00182233 File Offset: 0x00181233
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			this.Setters.Add(Trigger.CheckChildIsSetter(value));
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x00174CBA File Offset: 0x00173CBA
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0018224C File Offset: 0x0018124C
		internal override void Seal()
		{
			if (base.IsSealed)
			{
				return;
			}
			base.ProcessSettersCollection(this._setters);
			if (this._conditions.Count > 0)
			{
				this._conditions.Seal(ValueLookupType.Trigger);
			}
			base.TriggerConditions = new TriggerCondition[this._conditions.Count];
			for (int i = 0; i < base.TriggerConditions.Length; i++)
			{
				base.TriggerConditions[i] = new TriggerCondition(this._conditions[i].Property, LogicalOp.Equals, this._conditions[i].Value, (this._conditions[i].SourceName != null) ? this._conditions[i].SourceName : "~Self");
			}
			for (int j = 0; j < this.PropertyValues.Count; j++)
			{
				PropertyValue value = this.PropertyValues[j];
				value.Conditions = base.TriggerConditions;
				this.PropertyValues[j] = value;
			}
			base.Seal();
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x00182354 File Offset: 0x00181354
		internal override bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			bool flag = base.TriggerConditions.Length != 0;
			int num = 0;
			while (flag && num < base.TriggerConditions.Length)
			{
				flag = base.TriggerConditions[num].Match(container.GetValue(base.TriggerConditions[num].Property));
				num++;
			}
			return flag;
		}

		// Token: 0x0400113B RID: 4411
		private ConditionCollection _conditions = new ConditionCollection();

		// Token: 0x0400113C RID: 4412
		private SetterBaseCollection _setters;
	}
}
