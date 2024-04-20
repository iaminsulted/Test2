using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Markup
{
	// Token: 0x0200046F RID: 1135
	public class ComponentResourceKeyConverter : ExpressionConverter
	{
		// Token: 0x06003A75 RID: 14965 RVA: 0x001F0B9F File Offset: 0x001EFB9F
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			return base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x001F0BBD File Offset: 0x001EFBBD
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			return base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x001F0BDB File Offset: 0x001EFBDB
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x001F0BE8 File Offset: 0x001EFBE8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(value is ComponentResourceKey))
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"ComponentResourceKey"
				}));
			}
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			return base.CanConvertTo(context, destinationType);
		}
	}
}
