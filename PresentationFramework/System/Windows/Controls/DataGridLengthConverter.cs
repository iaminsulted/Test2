using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200075C RID: 1884
	public class DataGridLengthConverter : TypeConverter
	{
		// Token: 0x06006682 RID: 26242 RVA: 0x002B1D48 File Offset: 0x002B0D48
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Byte <= 9 || typeCode == TypeCode.String;
		}

		// Token: 0x06006683 RID: 26243 RVA: 0x002B1D6B File Offset: 0x002B0D6B
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor);
		}

		// Token: 0x06006684 RID: 26244 RVA: 0x002B1D94 File Offset: 0x002B0D94
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value != null)
			{
				string text = value as string;
				if (text != null)
				{
					return DataGridLengthConverter.ConvertFromString(text, culture);
				}
				double num = Convert.ToDouble(value, culture);
				DataGridLengthUnitType type;
				if (DoubleUtil.IsNaN(num))
				{
					num = 1.0;
					type = DataGridLengthUnitType.Auto;
				}
				else
				{
					type = DataGridLengthUnitType.Pixel;
				}
				if (!double.IsInfinity(num))
				{
					return new DataGridLength(num, type);
				}
			}
			throw base.GetConvertFromException(value);
		}

		// Token: 0x06006685 RID: 26245 RVA: 0x002B1DF8 File Offset: 0x002B0DF8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is DataGridLength)
			{
				DataGridLength length = (DataGridLength)value;
				if (destinationType == typeof(string))
				{
					return DataGridLengthConverter.ConvertToString(length, culture);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					return new InstanceDescriptor(typeof(DataGridLength).GetConstructor(new Type[]
					{
						typeof(double),
						typeof(DataGridLengthUnitType)
					}), new object[]
					{
						length.Value,
						length.UnitType
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06006686 RID: 26246 RVA: 0x002B1EC4 File Offset: 0x002B0EC4
		internal static string ConvertToString(DataGridLength length, CultureInfo cultureInfo)
		{
			switch (length.UnitType)
			{
			case DataGridLengthUnitType.Auto:
			case DataGridLengthUnitType.SizeToCells:
			case DataGridLengthUnitType.SizeToHeader:
				return length.UnitType.ToString();
			case DataGridLengthUnitType.Star:
				if (!DoubleUtil.IsOne(length.Value))
				{
					return Convert.ToString(length.Value, cultureInfo) + "*";
				}
				return "*";
			}
			return Convert.ToString(length.Value, cultureInfo);
		}

		// Token: 0x06006687 RID: 26247 RVA: 0x002B1F44 File Offset: 0x002B0F44
		private static DataGridLength ConvertFromString(string s, CultureInfo cultureInfo)
		{
			string text = s.Trim().ToLowerInvariant();
			for (int i = 0; i < 3; i++)
			{
				string b = DataGridLengthConverter._unitStrings[i];
				if (text == b)
				{
					return new DataGridLength(1.0, (DataGridLengthUnitType)i);
				}
			}
			double value = 0.0;
			DataGridLengthUnitType dataGridLengthUnitType = DataGridLengthUnitType.Pixel;
			int length = text.Length;
			int num = 0;
			double num2 = 1.0;
			int num3 = DataGridLengthConverter._unitStrings.Length;
			for (int j = 3; j < num3; j++)
			{
				string text2 = DataGridLengthConverter._unitStrings[j];
				if (text.EndsWith(text2, StringComparison.Ordinal))
				{
					num = text2.Length;
					dataGridLengthUnitType = (DataGridLengthUnitType)j;
					break;
				}
			}
			if (num == 0)
			{
				num3 = DataGridLengthConverter._nonStandardUnitStrings.Length;
				for (int k = 0; k < num3; k++)
				{
					string text3 = DataGridLengthConverter._nonStandardUnitStrings[k];
					if (text.EndsWith(text3, StringComparison.Ordinal))
					{
						num = text3.Length;
						num2 = DataGridLengthConverter._pixelUnitFactors[k];
						break;
					}
				}
			}
			if (length == num)
			{
				if (dataGridLengthUnitType == DataGridLengthUnitType.Star)
				{
					value = 1.0;
				}
			}
			else
			{
				value = Convert.ToDouble(text.Substring(0, length - num), cultureInfo) * num2;
			}
			return new DataGridLength(value, dataGridLengthUnitType);
		}

		// Token: 0x040033C8 RID: 13256
		private static string[] _unitStrings = new string[]
		{
			"auto",
			"px",
			"sizetocells",
			"sizetoheader",
			"*"
		};

		// Token: 0x040033C9 RID: 13257
		private const int NumDescriptiveUnits = 3;

		// Token: 0x040033CA RID: 13258
		private static string[] _nonStandardUnitStrings = new string[]
		{
			"in",
			"cm",
			"pt"
		};

		// Token: 0x040033CB RID: 13259
		private static double[] _pixelUnitFactors = new double[]
		{
			96.0,
			37.79527559055118,
			1.3333333333333333
		};
	}
}
