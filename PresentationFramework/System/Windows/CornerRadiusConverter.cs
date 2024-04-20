using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Text;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x0200034E RID: 846
	public class CornerRadiusConverter : TypeConverter
	{
		// Token: 0x06002023 RID: 8227 RVA: 0x00174764 File Offset: 0x00173764
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x00174786 File Offset: 0x00173786
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x001747AF File Offset: 0x001737AF
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return CornerRadiusConverter.FromString((string)source, cultureInfo);
			}
			return new CornerRadius(Convert.ToDouble(source, cultureInfo));
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x001747E8 File Offset: 0x001737E8
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
			if (!(value is CornerRadius))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(CornerRadius)
				}), "value");
			}
			CornerRadius cr = (CornerRadius)value;
			if (destinationType == typeof(string))
			{
				return CornerRadiusConverter.ToString(cr, cultureInfo);
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				return new InstanceDescriptor(typeof(CornerRadius).GetConstructor(new Type[]
				{
					typeof(double),
					typeof(double),
					typeof(double),
					typeof(double)
				}), new object[]
				{
					cr.TopLeft,
					cr.TopRight,
					cr.BottomRight,
					cr.BottomLeft
				});
			}
			throw new ArgumentException(SR.Get("CannotConvertType", new object[]
			{
				typeof(CornerRadius),
				destinationType.FullName
			}));
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x00174948 File Offset: 0x00173948
		internal static string ToString(CornerRadius cr, CultureInfo cultureInfo)
		{
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append(cr.TopLeft.ToString(cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(cr.TopRight.ToString(cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(cr.BottomRight.ToString(cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(cr.BottomLeft.ToString(cultureInfo));
			return stringBuilder.ToString();
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x001749DC File Offset: 0x001739DC
		internal static CornerRadius FromString(string s, CultureInfo cultureInfo)
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
				array[num] = double.Parse(tokenizerHelper.GetCurrentToken(), cultureInfo);
				num++;
			}
			if (num == 1)
			{
				return new CornerRadius(array[0]);
			}
			if (num != 4)
			{
				throw new FormatException(SR.Get("InvalidStringCornerRadius", new object[]
				{
					s
				}));
			}
			return new CornerRadius(array[0], array[1], array[2], array[3]);
		}
	}
}
