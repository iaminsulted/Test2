using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows
{
	// Token: 0x020003C4 RID: 964
	public class TemplateBindingExtensionConverter : TypeConverter
	{
		// Token: 0x0600288B RID: 10379 RVA: 0x001758C1 File Offset: 0x001748C1
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x00195680 File Offset: 0x00194680
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(destinationType == typeof(InstanceDescriptor)))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TemplateBindingExtension templateBindingExtension = value as TemplateBindingExtension;
			if (templateBindingExtension == null)
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"TemplateBindingExtension"
				}), "value");
			}
			return new InstanceDescriptor(typeof(TemplateBindingExtension).GetConstructor(new Type[]
			{
				typeof(DependencyProperty)
			}), new object[]
			{
				templateBindingExtension.Property
			});
		}
	}
}
