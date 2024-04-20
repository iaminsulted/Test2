using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Xaml;

namespace System.Windows.Markup
{
	// Token: 0x020004C0 RID: 1216
	public sealed class DependencyPropertyConverter : TypeConverter
	{
		// Token: 0x06003E7C RID: 15996 RVA: 0x00200F1D File Offset: 0x001FFF1D
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(byte[]);
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return false;
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x00200F48 File Offset: 0x001FFF48
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			DependencyProperty dependencyProperty = DependencyPropertyConverter.ResolveProperty(context, null, source);
			if (dependencyProperty != null)
			{
				return dependencyProperty;
			}
			throw base.GetConvertFromException(source);
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x001F0D44 File Offset: 0x001EFD44
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00200F88 File Offset: 0x001FFF88
		internal static DependencyProperty ResolveProperty(IServiceProvider serviceProvider, string targetName, object source)
		{
			Type type = null;
			string text = null;
			DependencyProperty dependencyProperty = source as DependencyProperty;
			if (dependencyProperty != null)
			{
				return dependencyProperty;
			}
			byte[] array;
			if ((array = (source as byte[])) != null)
			{
				Baml2006SchemaContext baml2006SchemaContext = (serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider).SchemaContext as Baml2006SchemaContext;
				if (array.Length == 2)
				{
					short propertyId = (short)((int)array[0] | (int)array[1] << 8);
					return baml2006SchemaContext.GetDependencyProperty(propertyId);
				}
				using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(array)))
				{
					type = baml2006SchemaContext.GetXamlType(binaryReader.ReadInt16()).UnderlyingType;
					text = binaryReader.ReadString();
					goto IL_13E;
				}
			}
			string text2;
			if ((text2 = (source as string)) == null)
			{
				throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
				{
					"Property",
					typeof(DependencyProperty).FullName
				}));
			}
			text2 = text2.Trim();
			if (text2.Contains("."))
			{
				int num = text2.LastIndexOf('.');
				string qualifiedTypeName = text2.Substring(0, num);
				text = text2.Substring(num + 1);
				type = (serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver).Resolve(qualifiedTypeName);
			}
			else
			{
				int num2 = text2.LastIndexOf(':');
				text = text2.Substring(num2 + 1);
			}
			IL_13E:
			if (type == null && targetName != null)
			{
				IAmbientProvider ambientProvider = serviceProvider.GetService(typeof(IAmbientProvider)) as IAmbientProvider;
				type = DependencyPropertyConverter.GetTypeFromName((serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider).SchemaContext, ambientProvider, targetName);
			}
			if (type == null)
			{
				IXamlSchemaContextProvider xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
				if (xamlSchemaContextProvider == null)
				{
					throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						"Property",
						typeof(DependencyProperty).FullName
					}));
				}
				XamlSchemaContext schemaContext = xamlSchemaContextProvider.SchemaContext;
				XamlType xamlType = schemaContext.GetXamlType(typeof(Style));
				XamlType xamlType2 = schemaContext.GetXamlType(typeof(FrameworkTemplate));
				XamlType xamlType3 = schemaContext.GetXamlType(typeof(DataTemplate));
				XamlType xamlType4 = schemaContext.GetXamlType(typeof(ControlTemplate));
				List<XamlType> list = new List<XamlType>();
				list.Add(xamlType);
				list.Add(xamlType2);
				list.Add(xamlType3);
				list.Add(xamlType4);
				XamlMember member = xamlType.GetMember("TargetType");
				XamlMember member2 = xamlType2.GetMember("Template");
				XamlMember member3 = xamlType4.GetMember("TargetType");
				IAmbientProvider ambientProvider2 = serviceProvider.GetService(typeof(IAmbientProvider)) as IAmbientProvider;
				if (ambientProvider2 == null)
				{
					throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						"Property",
						typeof(DependencyProperty).FullName
					}));
				}
				AmbientPropertyValue firstAmbientValue = ambientProvider2.GetFirstAmbientValue(list, new XamlMember[]
				{
					member,
					member2,
					member3
				});
				if (firstAmbientValue != null)
				{
					if (firstAmbientValue.Value is Type)
					{
						type = (Type)firstAmbientValue.Value;
					}
					else
					{
						if (!(firstAmbientValue.Value is TemplateContent))
						{
							throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
							{
								"Property",
								typeof(DependencyProperty).FullName
							}));
						}
						type = (firstAmbientValue.Value as TemplateContent).OwnerTemplate.TargetTypeInternal;
					}
				}
			}
			if (type != null && text != null)
			{
				return DependencyProperty.FromName(text, type);
			}
			throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
			{
				"Property",
				typeof(DependencyProperty).FullName
			}));
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x0020134C File Offset: 0x0020034C
		private static Type GetTypeFromName(XamlSchemaContext schemaContext, IAmbientProvider ambientProvider, string target)
		{
			XamlType xamlType = schemaContext.GetXamlType(typeof(FrameworkTemplate));
			XamlMember member = xamlType.GetMember("Template");
			TemplateContent templateContent = ambientProvider.GetFirstAmbientValue(new XamlType[]
			{
				xamlType
			}, new XamlMember[]
			{
				member
			}).Value as TemplateContent;
			if (templateContent != null)
			{
				return templateContent.GetTypeForName(target).UnderlyingType;
			}
			return null;
		}
	}
}
