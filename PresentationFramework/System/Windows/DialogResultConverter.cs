using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows
{
	// Token: 0x0200035B RID: 859
	public class DialogResultConverter : TypeConverter
	{
		// Token: 0x06002074 RID: 8308 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			return false;
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return false;
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x00175835 File Offset: 0x00174835
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			throw new InvalidOperationException(SR.Get("CantSetInMarkup"));
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x00175835 File Offset: 0x00174835
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			throw new InvalidOperationException(SR.Get("CantSetInMarkup"));
		}
	}
}
