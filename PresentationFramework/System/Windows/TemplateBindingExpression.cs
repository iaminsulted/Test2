using System;
using System.ComponentModel;

namespace System.Windows
{
	// Token: 0x020003C1 RID: 961
	[TypeConverter(typeof(TemplateBindingExpressionConverter))]
	public class TemplateBindingExpression : Expression
	{
		// Token: 0x0600287C RID: 10364 RVA: 0x0019554A File Offset: 0x0019454A
		internal TemplateBindingExpression(TemplateBindingExtension templateBindingExtension)
		{
			this._templateBindingExtension = templateBindingExtension;
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x00195559 File Offset: 0x00194559
		public TemplateBindingExtension TemplateBindingExtension
		{
			get
			{
				return this._templateBindingExtension;
			}
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x00195561 File Offset: 0x00194561
		internal override object GetValue(DependencyObject d, DependencyProperty dp)
		{
			return dp.GetDefaultValue(d.DependencyObjectType);
		}

		// Token: 0x0400148C RID: 5260
		private TemplateBindingExtension _templateBindingExtension;
	}
}
