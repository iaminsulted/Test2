using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Markup
{
	// Token: 0x02000524 RID: 1316
	public class ResourceReferenceExpressionConverter : ExpressionConverter
	{
		// Token: 0x0600417A RID: 16762 RVA: 0x00218484 File Offset: 0x00217484
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x0021848E File Offset: 0x0021748E
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			return destinationType == typeof(MarkupExtension) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x001F0BDB File Offset: 0x001EFBDB
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x002184C0 File Offset: 0x002174C0
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			ResourceReferenceExpression resourceReferenceExpression = value as ResourceReferenceExpression;
			if (resourceReferenceExpression == null)
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"ResourceReferenceExpression"
				}));
			}
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(MarkupExtension))
			{
				return new DynamicResourceExtension(resourceReferenceExpression.ResourceKey);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
