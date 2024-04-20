using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x02000375 RID: 885
	public class GridLengthConverter : TypeConverter
	{
		// Token: 0x060023E3 RID: 9187 RVA: 0x00174764 File Offset: 0x00173764
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x0017608B File Offset: 0x0017508B
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x00181254 File Offset: 0x00180254
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return GridLengthConverter.FromString((string)source, cultureInfo);
			}
			double value = Convert.ToDouble(source, cultureInfo);
			GridUnitType type;
			if (DoubleUtil.IsNaN(value))
			{
				value = 1.0;
				type = GridUnitType.Auto;
			}
			else
			{
				type = GridUnitType.Pixel;
			}
			return new GridLength(value, type);
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x001812B4 File Offset: 0x001802B4
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is GridLength)
			{
				GridLength gl = (GridLength)value;
				if (destinationType == typeof(string))
				{
					return GridLengthConverter.ToString(gl, cultureInfo);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					return new InstanceDescriptor(typeof(GridLength).GetConstructor(new Type[]
					{
						typeof(double),
						typeof(GridUnitType)
					}), new object[]
					{
						gl.Value,
						gl.GridUnitType
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x00181380 File Offset: 0x00180380
		internal static string ToString(GridLength gl, CultureInfo cultureInfo)
		{
			GridUnitType gridUnitType = gl.GridUnitType;
			if (gridUnitType == GridUnitType.Auto)
			{
				return "Auto";
			}
			if (gridUnitType != GridUnitType.Star)
			{
				return Convert.ToString(gl.Value, cultureInfo);
			}
			if (!DoubleUtil.IsOne(gl.Value))
			{
				return Convert.ToString(gl.Value, cultureInfo) + "*";
			}
			return "*";
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x001813E0 File Offset: 0x001803E0
		internal static GridLength FromString(string s, CultureInfo cultureInfo)
		{
			double value;
			GridUnitType type;
			XamlGridLengthSerializer.FromString(s, cultureInfo, out value, out type);
			return new GridLength(value, type);
		}
	}
}
