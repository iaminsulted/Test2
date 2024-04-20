using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Text;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x020003D4 RID: 980
	public class ThicknessConverter : TypeConverter
	{
		// Token: 0x06002906 RID: 10502 RVA: 0x00174764 File Offset: 0x00173764
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x00174786 File Offset: 0x00173786
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x00197F04 File Offset: 0x00196F04
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return ThicknessConverter.FromString((string)source, cultureInfo);
			}
			if (source is double)
			{
				return new Thickness((double)source);
			}
			return new Thickness(Convert.ToDouble(source, cultureInfo));
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x00197F60 File Offset: 0x00196F60
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
			if (!(value is Thickness))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(Thickness)
				}), "value");
			}
			Thickness th = (Thickness)value;
			if (destinationType == typeof(string))
			{
				return ThicknessConverter.ToString(th, cultureInfo);
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				return new InstanceDescriptor(typeof(Thickness).GetConstructor(new Type[]
				{
					typeof(double),
					typeof(double),
					typeof(double),
					typeof(double)
				}), new object[]
				{
					th.Left,
					th.Top,
					th.Right,
					th.Bottom
				});
			}
			throw new ArgumentException(SR.Get("CannotConvertType", new object[]
			{
				typeof(Thickness),
				destinationType.FullName
			}));
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x001980C0 File Offset: 0x001970C0
		internal static string ToString(Thickness th, CultureInfo cultureInfo)
		{
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append(LengthConverter.ToString(th.Left, cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(LengthConverter.ToString(th.Top, cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(LengthConverter.ToString(th.Right, cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(LengthConverter.ToString(th.Bottom, cultureInfo));
			return stringBuilder.ToString();
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x00198148 File Offset: 0x00197148
		internal static Thickness FromString(string s, CultureInfo cultureInfo)
		{
			TokenizerHelper tokenizerHelper = new TokenizerHelper(s, cultureInfo);
			double[] array = new double[4];
			int num = 0;
			while (tokenizerHelper.NextToken())
			{
				if (num >= 4)
				{
					num = 5;
					break;
				}
				array[num] = LengthConverter.FromString(tokenizerHelper.GetCurrentToken(), cultureInfo);
				num++;
			}
			switch (num)
			{
			case 1:
				return new Thickness(array[0]);
			case 2:
				return new Thickness(array[0], array[1], array[0], array[1]);
			case 4:
				return new Thickness(array[0], array[1], array[2], array[3]);
			}
			throw new FormatException(SR.Get("InvalidStringThickness", new object[]
			{
				s
			}));
		}
	}
}
