using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Data;
using System.Windows.Markup;

namespace MS.Internal.Data
{
	// Token: 0x02000217 RID: 535
	internal class DefaultValueConverter
	{
		// Token: 0x06001446 RID: 5190 RVA: 0x00151597 File Offset: 0x00150597
		protected DefaultValueConverter(TypeConverter typeConverter, Type sourceType, Type targetType, bool shouldConvertFrom, bool shouldConvertTo, DataBindEngine engine)
		{
			this._typeConverter = typeConverter;
			this._sourceType = sourceType;
			this._targetType = targetType;
			this._shouldConvertFrom = shouldConvertFrom;
			this._shouldConvertTo = shouldConvertTo;
			this._engine = engine;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x001515CC File Offset: 0x001505CC
		internal static IValueConverter Create(Type sourceType, Type targetType, bool targetToSource, DataBindEngine engine)
		{
			bool flag = false;
			bool flag2 = false;
			if (sourceType == targetType || (!targetToSource && targetType.IsAssignableFrom(sourceType)))
			{
				return DefaultValueConverter.ValueConverterNotNeeded;
			}
			if (targetType == typeof(object))
			{
				return new ObjectTargetConverter(sourceType, engine);
			}
			if (sourceType == typeof(object))
			{
				return new ObjectSourceConverter(targetType, engine);
			}
			if (SystemConvertConverter.CanConvert(sourceType, targetType))
			{
				return new SystemConvertConverter(sourceType, targetType);
			}
			Type underlyingType = Nullable.GetUnderlyingType(sourceType);
			if (underlyingType != null)
			{
				sourceType = underlyingType;
				flag = true;
			}
			underlyingType = Nullable.GetUnderlyingType(targetType);
			if (underlyingType != null)
			{
				targetType = underlyingType;
				flag2 = true;
			}
			if (flag || flag2)
			{
				return DefaultValueConverter.Create(sourceType, targetType, targetToSource, engine);
			}
			if (typeof(IListSource).IsAssignableFrom(sourceType) && targetType.IsAssignableFrom(typeof(IList)))
			{
				return new ListSourceConverter();
			}
			if (sourceType.IsInterface || targetType.IsInterface)
			{
				return new InterfaceConverter(sourceType, targetType);
			}
			TypeConverter converter = DefaultValueConverter.GetConverter(sourceType);
			bool flag3 = converter != null && converter.CanConvertTo(targetType);
			bool flag4 = converter != null && converter.CanConvertFrom(targetType);
			if ((flag3 || targetType.IsAssignableFrom(sourceType)) && (!targetToSource || flag4 || sourceType.IsAssignableFrom(targetType)))
			{
				return new SourceDefaultValueConverter(converter, sourceType, targetType, targetToSource && flag4, flag3, engine);
			}
			converter = DefaultValueConverter.GetConverter(targetType);
			flag3 = (converter != null && converter.CanConvertTo(sourceType));
			flag4 = (converter != null && converter.CanConvertFrom(sourceType));
			if ((flag4 || targetType.IsAssignableFrom(sourceType)) && (!targetToSource || flag3 || sourceType.IsAssignableFrom(targetType)))
			{
				return new TargetDefaultValueConverter(converter, sourceType, targetType, flag4, targetToSource && flag3, engine);
			}
			return null;
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0015175C File Offset: 0x0015075C
		internal static TypeConverter GetConverter(Type type)
		{
			TypeConverter typeConverter = null;
			WpfKnownType wpfKnownType = XamlReader.BamlSharedSchemaContext.GetKnownXamlType(type) as WpfKnownType;
			if (wpfKnownType != null && wpfKnownType.TypeConverter != null)
			{
				typeConverter = wpfKnownType.TypeConverter.ConverterInstance;
			}
			if (typeConverter == null)
			{
				typeConverter = TypeDescriptor.GetConverter(type);
			}
			return typeConverter;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x001517AC File Offset: 0x001507AC
		internal static object TryParse(object o, Type targetType, CultureInfo culture)
		{
			object result = DependencyProperty.UnsetValue;
			string text = o as string;
			if (text != null)
			{
				try
				{
					MethodInfo method;
					if (culture != null && (method = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						DefaultValueConverter.StringType,
						typeof(NumberStyles),
						typeof(IFormatProvider)
					}, null)) != null)
					{
						result = method.Invoke(null, new object[]
						{
							text,
							NumberStyles.Any,
							culture
						});
					}
					else if (culture != null && (method = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						DefaultValueConverter.StringType,
						typeof(IFormatProvider)
					}, null)) != null)
					{
						result = method.Invoke(null, new object[]
						{
							text,
							culture
						});
					}
					else if ((method = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						DefaultValueConverter.StringType
					}, null)) != null)
					{
						result = method.Invoke(null, new object[]
						{
							text
						});
					}
				}
				catch (TargetInvocationException)
				{
				}
			}
			return result;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x001518D8 File Offset: 0x001508D8
		protected object ConvertFrom(object o, Type destinationType, DependencyObject targetElement, CultureInfo culture)
		{
			return this.ConvertHelper(o, destinationType, targetElement, culture, false);
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x001518E6 File Offset: 0x001508E6
		protected object ConvertTo(object o, Type destinationType, DependencyObject targetElement, CultureInfo culture)
		{
			return this.ConvertHelper(o, destinationType, targetElement, culture, true);
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x001518F4 File Offset: 0x001508F4
		protected void EnsureConverter(Type type)
		{
			if (this._typeConverter == null)
			{
				this._typeConverter = DefaultValueConverter.GetConverter(type);
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0015190C File Offset: 0x0015090C
		private object ConvertHelper(object o, Type destinationType, DependencyObject targetElement, CultureInfo culture, bool isForward)
		{
			object obj = DependencyProperty.UnsetValue;
			bool flag = isForward ? (!this._shouldConvertTo) : (!this._shouldConvertFrom);
			NotSupportedException ex = null;
			if (!flag)
			{
				obj = DefaultValueConverter.TryParse(o, destinationType, culture);
				if (obj == DependencyProperty.UnsetValue)
				{
					ValueConverterContext valueConverterContext = this.Engine.ValueConverterContext;
					if (valueConverterContext.IsInUse)
					{
						valueConverterContext = new ValueConverterContext();
					}
					try
					{
						valueConverterContext.SetTargetElement(targetElement);
						if (isForward)
						{
							obj = this._typeConverter.ConvertTo(valueConverterContext, culture, o, destinationType);
						}
						else
						{
							obj = this._typeConverter.ConvertFrom(valueConverterContext, culture, o);
						}
					}
					catch (NotSupportedException ex2)
					{
						flag = true;
						ex = ex2;
					}
					finally
					{
						valueConverterContext.SetTargetElement(null);
					}
				}
			}
			if (flag && ((o != null && destinationType.IsAssignableFrom(o.GetType())) || (o == null && !destinationType.IsValueType)))
			{
				obj = o;
				flag = false;
			}
			if (TraceData.IsEnabled)
			{
				if (culture != null && ex != null)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.DefaultValueConverterFailedForCulture(new object[]
					{
						AvTrace.ToStringHelper(o),
						AvTrace.TypeName(o),
						destinationType.ToString(),
						culture
					}), ex);
				}
				else if (flag)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.DefaultValueConverterFailed(new object[]
					{
						AvTrace.ToStringHelper(o),
						AvTrace.TypeName(o),
						destinationType.ToString()
					}), ex);
				}
			}
			if (flag && ex != null)
			{
				throw ex;
			}
			return obj;
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x00151A64 File Offset: 0x00150A64
		protected DataBindEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x04000BBD RID: 3005
		internal static readonly IValueConverter ValueConverterNotNeeded = new ObjectTargetConverter(typeof(object), null);

		// Token: 0x04000BBE RID: 3006
		protected Type _sourceType;

		// Token: 0x04000BBF RID: 3007
		protected Type _targetType;

		// Token: 0x04000BC0 RID: 3008
		private TypeConverter _typeConverter;

		// Token: 0x04000BC1 RID: 3009
		private bool _shouldConvertFrom;

		// Token: 0x04000BC2 RID: 3010
		private bool _shouldConvertTo;

		// Token: 0x04000BC3 RID: 3011
		private DataBindEngine _engine;

		// Token: 0x04000BC4 RID: 3012
		private static Type StringType = typeof(string);
	}
}
