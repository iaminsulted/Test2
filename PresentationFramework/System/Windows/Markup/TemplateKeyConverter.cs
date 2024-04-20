using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Markup
{
	// Token: 0x020004D8 RID: 1240
	public sealed class TemplateKeyConverter : TypeConverter
	{
		// Token: 0x06003F5A RID: 16218 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return false;
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return false;
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x0021101E File Offset: 0x0021001E
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
		{
			throw base.GetConvertFromException(source);
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x001F0D44 File Offset: 0x001EFD44
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			throw base.GetConvertToException(value, destinationType);
		}
	}
}
