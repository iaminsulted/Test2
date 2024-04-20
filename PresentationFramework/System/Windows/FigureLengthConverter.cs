using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x02000363 RID: 867
	public class FigureLengthConverter : TypeConverter
	{
		// Token: 0x060020B1 RID: 8369 RVA: 0x00174764 File Offset: 0x00173764
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x0017608B File Offset: 0x0017508B
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x001760B1 File Offset: 0x001750B1
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return FigureLengthConverter.FromString((string)source, cultureInfo);
			}
			return new FigureLength(Convert.ToDouble(source, cultureInfo));
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x001760EC File Offset: 0x001750EC
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is FigureLength)
			{
				FigureLength fl = (FigureLength)value;
				if (destinationType == typeof(string))
				{
					return FigureLengthConverter.ToString(fl, cultureInfo);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					return new InstanceDescriptor(typeof(FigureLength).GetConstructor(new Type[]
					{
						typeof(double),
						typeof(FigureUnitType)
					}), new object[]
					{
						fl.Value,
						fl.FigureUnitType
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x001761B8 File Offset: 0x001751B8
		internal static string ToString(FigureLength fl, CultureInfo cultureInfo)
		{
			FigureUnitType figureUnitType = fl.FigureUnitType;
			if (figureUnitType == FigureUnitType.Auto)
			{
				return "Auto";
			}
			if (figureUnitType != FigureUnitType.Pixel)
			{
				return Convert.ToString(fl.Value, cultureInfo) + " " + fl.FigureUnitType.ToString();
			}
			return Convert.ToString(fl.Value, cultureInfo);
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00176218 File Offset: 0x00175218
		internal static FigureLength FromString(string s, CultureInfo cultureInfo)
		{
			double value;
			FigureUnitType type;
			XamlFigureLengthSerializer.FromString(s, cultureInfo, out value, out type);
			return new FigureLength(value, type);
		}
	}
}
