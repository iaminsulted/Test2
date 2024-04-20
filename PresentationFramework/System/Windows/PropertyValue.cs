using System;

namespace System.Windows
{
	// Token: 0x020003A3 RID: 931
	internal struct PropertyValue
	{
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06002610 RID: 9744 RVA: 0x0018B08C File Offset: 0x0018A08C
		internal object Value
		{
			get
			{
				DeferredReference deferredReference = this.ValueInternal as DeferredReference;
				if (deferredReference != null)
				{
					this.ValueInternal = deferredReference.GetValue(BaseValueSourceInternal.Unknown);
				}
				return this.ValueInternal;
			}
		}

		// Token: 0x040011C2 RID: 4546
		internal PropertyValueType ValueType;

		// Token: 0x040011C3 RID: 4547
		internal TriggerCondition[] Conditions;

		// Token: 0x040011C4 RID: 4548
		internal string ChildName;

		// Token: 0x040011C5 RID: 4549
		internal DependencyProperty Property;

		// Token: 0x040011C6 RID: 4550
		internal object ValueInternal;
	}
}
