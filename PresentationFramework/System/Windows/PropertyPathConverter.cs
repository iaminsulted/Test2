using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using MS.Internal.Data;

namespace System.Windows
{
	// Token: 0x0200038A RID: 906
	public sealed class PropertyPathConverter : TypeConverter
	{
		// Token: 0x06002484 RID: 9348 RVA: 0x001832F5 File Offset: 0x001822F5
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x001832F5 File Offset: 0x001822F5
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x0018330C File Offset: 0x0018230C
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (source is string)
			{
				return new PropertyPath((string)source, typeDescriptorContext);
			}
			throw new ArgumentException(SR.Get("CannotConvertType", new object[]
			{
				source.GetType().FullName,
				typeof(PropertyPath)
			}));
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x0018336C File Offset: 0x0018236C
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (null == destinationType)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType != typeof(string))
			{
				throw new ArgumentException(SR.Get("CannotConvertType", new object[]
				{
					typeof(PropertyPath),
					destinationType.FullName
				}));
			}
			PropertyPath propertyPath = value as PropertyPath;
			if (propertyPath == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(PropertyPath)
				}), "value");
			}
			if (propertyPath.PathParameters.Count == 0)
			{
				return propertyPath.Path;
			}
			string path = propertyPath.Path;
			Collection<object> pathParameters = propertyPath.PathParameters;
			bool flag = ((typeDescriptorContext == null) ? null : (typeDescriptorContext.GetService(typeof(XamlDesignerSerializationManager)) as XamlDesignerSerializationManager)) != null;
			ValueSerializer valueSerializer = null;
			IValueSerializerContext valueSerializerContext = null;
			if (!flag)
			{
				valueSerializerContext = (typeDescriptorContext as IValueSerializerContext);
				if (valueSerializerContext != null)
				{
					valueSerializer = ValueSerializer.GetSerializerFor(typeof(Type), valueSerializerContext);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (int i = 0; i < path.Length; i++)
			{
				if (path[i] == '(')
				{
					int num2 = i + 1;
					while (num2 < path.Length && path[num2] != ')')
					{
						num2++;
					}
					int index;
					if (int.TryParse(path.Substring(i + 1, num2 - i - 1), NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out index))
					{
						stringBuilder.Append(path.Substring(num, i - num + 1));
						object obj = pathParameters[index];
						DependencyProperty dependencyProperty;
						PropertyInfo propertyInfo;
						PropertyDescriptor propertyDescriptor;
						DynamicObjectAccessor dynamicObjectAccessor;
						PropertyPath.DowncastAccessor(obj, out dependencyProperty, out propertyInfo, out propertyDescriptor, out dynamicObjectAccessor);
						Type type;
						string text;
						if (dependencyProperty != null)
						{
							type = dependencyProperty.OwnerType;
							text = dependencyProperty.Name;
						}
						else if (propertyInfo != null)
						{
							type = propertyInfo.DeclaringType;
							text = propertyInfo.Name;
						}
						else if (propertyDescriptor != null)
						{
							type = propertyDescriptor.ComponentType;
							text = propertyDescriptor.Name;
						}
						else if (dynamicObjectAccessor != null)
						{
							type = dynamicObjectAccessor.OwnerType;
							text = dynamicObjectAccessor.PropertyName;
						}
						else
						{
							type = obj.GetType();
							text = null;
						}
						if (valueSerializer != null)
						{
							stringBuilder.Append(valueSerializer.ConvertToString(type, valueSerializerContext));
						}
						else
						{
							string text2 = null;
							if (text2 != null && text2 != string.Empty)
							{
								stringBuilder.Append(text2);
								stringBuilder.Append(':');
							}
							stringBuilder.Append(type.Name);
						}
						if (text != null)
						{
							stringBuilder.Append('.');
							stringBuilder.Append(text);
							stringBuilder.Append(')');
						}
						else
						{
							stringBuilder.Append(')');
							text = (obj as string);
							if (text == null)
							{
								TypeConverter converter = TypeDescriptor.GetConverter(type);
								if (converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
								{
									try
									{
										text = converter.ConvertToString(obj);
									}
									catch (NotSupportedException)
									{
									}
								}
							}
							stringBuilder.Append(text);
						}
						i = num2;
						num = num2 + 1;
					}
				}
			}
			if (num < path.Length)
			{
				stringBuilder.Append(path.Substring(num));
			}
			return stringBuilder.ToString();
		}
	}
}
