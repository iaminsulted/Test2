using System;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000206 RID: 518
	internal class BindingExpressionUncommonField : UncommonField<BindingExpression>
	{
		// Token: 0x060012E1 RID: 4833 RVA: 0x0014BE09 File Offset: 0x0014AE09
		internal new void SetValue(DependencyObject instance, BindingExpression bindingExpr)
		{
			base.SetValue(instance, bindingExpr);
			bindingExpr.Attach(instance);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0014BE1C File Offset: 0x0014AE1C
		internal new void ClearValue(DependencyObject instance)
		{
			BindingExpression value = base.GetValue(instance);
			if (value != null)
			{
				value.Detach();
			}
			base.ClearValue(instance);
		}
	}
}
