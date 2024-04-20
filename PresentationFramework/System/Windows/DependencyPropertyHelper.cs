using System;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x02000356 RID: 854
	public static class DependencyPropertyHelper
	{
		// Token: 0x0600205E RID: 8286 RVA: 0x0017518C File Offset: 0x0017418C
		public static ValueSource GetValueSource(DependencyObject dependencyObject, DependencyProperty dependencyProperty)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			if (dependencyProperty == null)
			{
				throw new ArgumentNullException("dependencyProperty");
			}
			dependencyObject.VerifyAccess();
			bool flag;
			bool isExpression;
			bool isAnimated;
			bool isCoerced;
			bool isCurrent;
			return new ValueSource(dependencyObject.GetValueSource(dependencyProperty, null, out flag, out isExpression, out isAnimated, out isCoerced, out isCurrent), isExpression, isAnimated, isCoerced, isCurrent);
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x001751D8 File Offset: 0x001741D8
		public static bool IsTemplatedValueDynamic(DependencyObject elementInTemplate, DependencyProperty dependencyProperty)
		{
			if (elementInTemplate == null)
			{
				throw new ArgumentNullException("elementInTemplate");
			}
			if (dependencyProperty == null)
			{
				throw new ArgumentNullException("dependencyProperty");
			}
			FrameworkObject frameworkObject = new FrameworkObject(elementInTemplate);
			DependencyObject templatedParent = frameworkObject.TemplatedParent;
			if (templatedParent == null)
			{
				throw new ArgumentException(SR.Get("ElementMustBelongToTemplate"), "elementInTemplate");
			}
			int templateChildIndex = frameworkObject.TemplateChildIndex;
			return StyleHelper.IsValueDynamic(templatedParent, templateChildIndex, dependencyProperty);
		}
	}
}
