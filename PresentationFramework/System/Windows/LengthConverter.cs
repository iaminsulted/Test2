using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x0200037B RID: 891
	public class LengthConverter : TypeConverter
	{
		// Token: 0x06002411 RID: 9233 RVA: 0x00174764 File Offset: 0x00173764
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x00174786 File Offset: 0x00173786
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x00181570 File Offset: 0x00180570
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return LengthConverter.FromString((string)source, cultureInfo);
			}
			return Convert.ToDouble(source, cultureInfo);
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x001815A4 File Offset: 0x001805A4
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is double)
			{
				double num = (double)value;
				if (destinationType == typeof(string))
				{
					if (DoubleUtil.IsNaN(num))
					{
						return "Auto";
					}
					return Convert.ToString(num, cultureInfo);
				}
				else if (destinationType == typeof(InstanceDescriptor))
				{
					return new InstanceDescriptor(typeof(double).GetConstructor(new Type[]
					{
						typeof(double)
					}), new object[]
					{
						num
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x00181658 File Offset: 0x00180658
		internal static double FromString(string s, CultureInfo cultureInfo)
		{
			string text = s.Trim();
			string text2 = text.ToLowerInvariant();
			int length = text2.Length;
			int num = 0;
			double num2 = 1.0;
			if (text2 == "auto")
			{
				return double.NaN;
			}
			for (int i = 0; i < LengthConverter.PixelUnitStrings.Length; i++)
			{
				if (text2.EndsWith(LengthConverter.PixelUnitStrings[i], StringComparison.Ordinal))
				{
					num = LengthConverter.PixelUnitStrings[i].Length;
					num2 = LengthConverter.PixelUnitFactors[i];
					break;
				}
			}
			text = text.Substring(0, length - num);
			double result;
			try
			{
				result = Convert.ToDouble(text, cultureInfo) * num2;
			}
			catch (FormatException)
			{
				throw new FormatException(SR.Get("LengthFormatError", new object[]
				{
					text
				}));
			}
			return result;
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x00181728 File Offset: 0x00180728
		internal static string ToString(double l, CultureInfo cultureInfo)
		{
			if (DoubleUtil.IsNaN(l))
			{
				return "Auto";
			}
			return Convert.ToString(l, cultureInfo);
		}

		// Token: 0x0400110E RID: 4366
		private static string[] PixelUnitStrings = new string[]
		{
			"px",
			"in",
			"cm",
			"pt"
		};

		// Token: 0x0400110F RID: 4367
		private static double[] PixelUnitFactors = new double[]
		{
			1.0,
			96.0,
			37.79527559055118,
			1.3333333333333333
		};
	}
}
