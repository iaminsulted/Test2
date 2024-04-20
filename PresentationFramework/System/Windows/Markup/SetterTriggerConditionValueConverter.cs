using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Baml2006;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Markup
{
	// Token: 0x020004D4 RID: 1236
	public sealed class SetterTriggerConditionValueConverter : TypeConverter
	{
		// Token: 0x06003F4B RID: 16203 RVA: 0x00200F1D File Offset: 0x001FFF1D
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(byte[]);
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return false;
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x00210D3A File Offset: 0x0020FD3A
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
		{
			return SetterTriggerConditionValueConverter.ResolveValue(context, null, culture, source);
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x001F0D44 File Offset: 0x001EFD44
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x00210D48 File Offset: 0x0020FD48
		internal static object ResolveValue(ITypeDescriptorContext serviceProvider, DependencyProperty property, CultureInfo culture, object source)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (!(source is byte[]) && !(source is string) && !(source is Stream))
			{
				return source;
			}
			IXamlSchemaContextProvider xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
			if (xamlSchemaContextProvider == null)
			{
				throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
				{
					"Value",
					typeof(object).FullName
				}));
			}
			XamlSchemaContext schemaContext = xamlSchemaContextProvider.SchemaContext;
			if (property == null)
			{
				return source;
			}
			XamlMember xamlMember = schemaContext.GetXamlType(property.OwnerType).GetMember(property.Name);
			if (xamlMember == null)
			{
				xamlMember = schemaContext.GetXamlType(property.OwnerType).GetAttachableMember(property.Name);
			}
			XamlValueConverter<TypeConverter> typeConverter;
			if (xamlMember != null)
			{
				if (xamlMember.Type.UnderlyingType.IsEnum && schemaContext is Baml2006SchemaContext)
				{
					typeConverter = XamlReader.BamlSharedSchemaContext.GetTypeConverter(xamlMember.Type.UnderlyingType);
				}
				else
				{
					typeConverter = xamlMember.TypeConverter;
					if (typeConverter == null)
					{
						typeConverter = xamlMember.Type.TypeConverter;
					}
				}
			}
			else
			{
				typeConverter = schemaContext.GetXamlType(property.PropertyType).TypeConverter;
			}
			if (typeConverter.ConverterType == null)
			{
				return source;
			}
			TypeConverter typeConverter2;
			if (xamlMember != null && xamlMember.Type.UnderlyingType == typeof(bool))
			{
				if (source is string)
				{
					typeConverter2 = new BooleanConverter();
				}
				else
				{
					if (!(source is byte[]))
					{
						throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
						{
							"Value",
							typeof(object).FullName
						}));
					}
					byte[] array = source as byte[];
					if (array != null && array.Length == 1)
					{
						return array[0] > 0;
					}
					throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						"Value",
						typeof(object).FullName
					}));
				}
			}
			else
			{
				typeConverter2 = typeConverter.ConverterInstance;
			}
			return typeConverter2.ConvertFrom(serviceProvider, culture, source);
		}
	}
}
