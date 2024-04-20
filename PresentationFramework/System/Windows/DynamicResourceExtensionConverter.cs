using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows
{
	// Token: 0x0200035D RID: 861
	public class DynamicResourceExtensionConverter : TypeConverter
	{
		// Token: 0x0600207E RID: 8318 RVA: 0x001758C1 File Offset: 0x001748C1
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x001758E0 File Offset: 0x001748E0
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
			DynamicResourceExtension dynamicResourceExtension = value as DynamicResourceExtension;
			if (dynamicResourceExtension == null)
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"DynamicResourceExtension"
				}), "value");
			}
			return new InstanceDescriptor(typeof(DynamicResourceExtension).GetConstructor(new Type[]
			{
				typeof(object)
			}), new object[]
			{
				dynamicResourceExtension.ResourceKey
			});
		}
	}
}
