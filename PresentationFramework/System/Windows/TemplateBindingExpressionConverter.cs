using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x020003C2 RID: 962
	public class TemplateBindingExpressionConverter : TypeConverter
	{
		// Token: 0x0600287F RID: 10367 RVA: 0x0019556F File Offset: 0x0019456F
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(MarkupExtension) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x00195590 File Offset: 0x00194590
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(destinationType == typeof(MarkupExtension)))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			TemplateBindingExpression templateBindingExpression = value as TemplateBindingExpression;
			if (templateBindingExpression == null)
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"TemplateBindingExpression"
				}));
			}
			return templateBindingExpression.TemplateBindingExtension;
		}
	}
}
