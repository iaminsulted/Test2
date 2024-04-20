using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xaml;
using MS.Internal.WindowsBase;

namespace System.Windows.Markup
{
	// Token: 0x020004D2 RID: 1234
	public sealed class RoutedEventConverter : TypeConverter
	{
		// Token: 0x06003F3E RID: 16190 RVA: 0x001832F5 File Offset: 0x001822F5
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return false;
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x00210A28 File Offset: 0x0020FA28
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			string text = source as string;
			RoutedEvent routedEvent = null;
			if (text != null)
			{
				text = text.Trim();
				if (typeDescriptorContext != null)
				{
					IXamlTypeResolver xamlTypeResolver = typeDescriptorContext.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
					Type type = null;
					if (xamlTypeResolver != null)
					{
						int num = text.IndexOf('.');
						if (num != -1)
						{
							string qualifiedTypeName = text.Substring(0, num);
							text = text.Substring(num + 1);
							type = xamlTypeResolver.Resolve(qualifiedTypeName);
						}
					}
					if (type == null)
					{
						IXamlSchemaContextProvider xamlSchemaContextProvider = typeDescriptorContext.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
						IAmbientProvider ambientProvider = typeDescriptorContext.GetService(typeof(IAmbientProvider)) as IAmbientProvider;
						if (xamlSchemaContextProvider != null && ambientProvider != null)
						{
							XamlType xamlType = xamlSchemaContextProvider.SchemaContext.GetXamlType(typeof(Style));
							List<XamlType> list = new List<XamlType>();
							list.Add(xamlType);
							XamlMember member = xamlType.GetMember("TargetType");
							AmbientPropertyValue firstAmbientValue = ambientProvider.GetFirstAmbientValue(list, new XamlMember[]
							{
								member
							});
							if (firstAmbientValue != null)
							{
								type = (firstAmbientValue.Value as Type);
							}
							if (type == null)
							{
								type = typeof(FrameworkElement);
							}
						}
					}
					if (type != null)
					{
						Type type2 = type;
						while (null != type2)
						{
							SecurityHelper.RunClassConstructor(type2);
							type2 = type2.BaseType;
						}
						routedEvent = EventManager.GetRoutedEventFromName(text, type);
					}
				}
			}
			if (routedEvent == null)
			{
				throw base.GetConvertFromException(source);
			}
			return routedEvent;
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x00210B94 File Offset: 0x0020FB94
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x00210BC4 File Offset: 0x0020FBC4
		private string ExtractNamespaceString(ref string nameString, ParserContext parserContext)
		{
			int num = nameString.IndexOf(':');
			string text = string.Empty;
			if (num != -1)
			{
				text = nameString.Substring(0, num);
				nameString = nameString.Substring(num + 1);
			}
			string text2 = parserContext.XmlnsDictionary[text];
			if (text2 == null)
			{
				throw new ArgumentException(SR.Get("ParserPrefixNSProperty", new object[]
				{
					text,
					nameString
				}));
			}
			return text2;
		}
	}
}
