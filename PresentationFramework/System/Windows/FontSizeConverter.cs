using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows
{
	// Token: 0x02000364 RID: 868
	public class FontSizeConverter : TypeConverter
	{
		// Token: 0x060020B8 RID: 8376 RVA: 0x00176238 File Offset: 0x00175238
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(int) || sourceType == typeof(float) || sourceType == typeof(double);
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x001758C1 File Offset: 0x001748C1
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00176290 File Offset: 0x00175290
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				throw base.GetConvertFromException(value);
			}
			string text = value as string;
			if (text != null)
			{
				double num;
				FontSizeConverter.FromString(text, culture, out num);
				return num;
			}
			if (value is int || value is float || value is double)
			{
				return (double)value;
			}
			return null;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x001762E8 File Offset: 0x001752E8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			double num = (double)value;
			if (destinationType == typeof(string))
			{
				return num.ToString(culture);
			}
			if (destinationType == typeof(int))
			{
				return (int)num;
			}
			if (destinationType == typeof(float))
			{
				return (float)num;
			}
			if (destinationType == typeof(double))
			{
				return num;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00176388 File Offset: 0x00175388
		internal static void FromString(string text, CultureInfo culture, out double amount)
		{
			amount = LengthConverter.FromString(text, culture);
		}
	}
}
