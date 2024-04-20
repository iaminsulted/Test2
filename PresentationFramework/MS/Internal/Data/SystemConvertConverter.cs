using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200021A RID: 538
	internal class SystemConvertConverter : IValueConverter
	{
		// Token: 0x06001456 RID: 5206 RVA: 0x00151AFF File Offset: 0x00150AFF
		public SystemConvertConverter(Type sourceType, Type targetType)
		{
			this._sourceType = sourceType;
			this._targetType = targetType;
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x00151B15 File Offset: 0x00150B15
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return System.Convert.ChangeType(o, this._targetType, culture);
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x00151B28 File Offset: 0x00150B28
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			object obj = DefaultValueConverter.TryParse(o, this._sourceType, culture);
			if (obj == DependencyProperty.UnsetValue)
			{
				return System.Convert.ChangeType(o, this._sourceType, culture);
			}
			return obj;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x00151B5C File Offset: 0x00150B5C
		public static bool CanConvert(Type sourceType, Type targetType)
		{
			if (sourceType == typeof(DateTime))
			{
				return targetType == typeof(string);
			}
			if (targetType == typeof(DateTime))
			{
				return sourceType == typeof(string);
			}
			if (sourceType == typeof(char))
			{
				return SystemConvertConverter.CanConvertChar(targetType);
			}
			if (targetType == typeof(char))
			{
				return SystemConvertConverter.CanConvertChar(sourceType);
			}
			for (int i = 0; i < SystemConvertConverter.SupportedTypes.Length; i++)
			{
				if (sourceType == SystemConvertConverter.SupportedTypes[i])
				{
					for (i++; i < SystemConvertConverter.SupportedTypes.Length; i++)
					{
						if (targetType == SystemConvertConverter.SupportedTypes[i])
						{
							return true;
						}
					}
				}
				else if (targetType == SystemConvertConverter.SupportedTypes[i])
				{
					for (i++; i < SystemConvertConverter.SupportedTypes.Length; i++)
					{
						if (sourceType == SystemConvertConverter.SupportedTypes[i])
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00151C60 File Offset: 0x00150C60
		private static bool CanConvertChar(Type type)
		{
			for (int i = 0; i < SystemConvertConverter.CharSupportedTypes.Length; i++)
			{
				if (type == SystemConvertConverter.CharSupportedTypes[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000BC5 RID: 3013
		private Type _sourceType;

		// Token: 0x04000BC6 RID: 3014
		private Type _targetType;

		// Token: 0x04000BC7 RID: 3015
		private static readonly Type[] SupportedTypes = new Type[]
		{
			typeof(string),
			typeof(int),
			typeof(long),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(bool),
			typeof(byte),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte)
		};

		// Token: 0x04000BC8 RID: 3016
		private static readonly Type[] CharSupportedTypes = new Type[]
		{
			typeof(string),
			typeof(int),
			typeof(long),
			typeof(byte),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte)
		};
	}
}
