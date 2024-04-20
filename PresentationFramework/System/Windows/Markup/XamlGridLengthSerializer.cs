using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004DE RID: 1246
	internal class XamlGridLengthSerializer : XamlSerializer
	{
		// Token: 0x06003F7F RID: 16255 RVA: 0x002118F1 File Offset: 0x002108F1
		private XamlGridLengthSerializer()
		{
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x00211CD0 File Offset: 0x00210CD0
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			double num;
			GridUnitType gridUnitType;
			XamlGridLengthSerializer.FromString(stringValue, TypeConverterHelper.InvariantEnglishUS, out num, out gridUnitType);
			byte b = (byte)gridUnitType;
			int num2 = (int)num;
			if ((double)num2 == num)
			{
				if (num2 <= 127 && num2 >= 0 && gridUnitType == GridUnitType.Pixel)
				{
					writer.Write((byte)num2);
				}
				else if (num2 <= 255 && num2 >= 0)
				{
					writer.Write(128 | b);
					writer.Write((byte)num2);
				}
				else if (num2 <= 32767 && num2 >= -32768)
				{
					writer.Write(192 | b);
					writer.Write((short)num2);
				}
				else
				{
					writer.Write(160 | b);
					writer.Write(num2);
				}
			}
			else
			{
				writer.Write(224 | b);
				writer.Write(num);
			}
			return true;
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x00211D98 File Offset: 0x00210D98
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			byte b = reader.ReadByte();
			GridUnitType type;
			double value;
			if ((b & 128) == 0)
			{
				type = GridUnitType.Pixel;
				value = (double)b;
			}
			else
			{
				type = (GridUnitType)(b & 31);
				byte b2 = b & 224;
				if (b2 == 128)
				{
					value = (double)reader.ReadByte();
				}
				else if (b2 == 192)
				{
					value = (double)reader.ReadInt16();
				}
				else if (b2 == 160)
				{
					value = (double)reader.ReadInt32();
				}
				else
				{
					value = reader.ReadDouble();
				}
			}
			return new GridLength(value, type);
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x00211E24 File Offset: 0x00210E24
		internal static void FromString(string s, CultureInfo cultureInfo, out double value, out GridUnitType unit)
		{
			string text = s.Trim().ToLowerInvariant();
			value = 0.0;
			unit = GridUnitType.Pixel;
			int length = text.Length;
			int num = 0;
			double num2 = 1.0;
			int i = 0;
			if (text == XamlGridLengthSerializer.UnitStrings[i])
			{
				num = XamlGridLengthSerializer.UnitStrings[i].Length;
				unit = (GridUnitType)i;
			}
			else
			{
				for (i = 1; i < XamlGridLengthSerializer.UnitStrings.Length; i++)
				{
					if (text.EndsWith(XamlGridLengthSerializer.UnitStrings[i], StringComparison.Ordinal))
					{
						num = XamlGridLengthSerializer.UnitStrings[i].Length;
						unit = (GridUnitType)i;
						break;
					}
				}
			}
			if (i >= XamlGridLengthSerializer.UnitStrings.Length)
			{
				for (i = 0; i < XamlGridLengthSerializer.PixelUnitStrings.Length; i++)
				{
					if (text.EndsWith(XamlGridLengthSerializer.PixelUnitStrings[i], StringComparison.Ordinal))
					{
						num = XamlGridLengthSerializer.PixelUnitStrings[i].Length;
						num2 = XamlGridLengthSerializer.PixelUnitFactors[i];
						break;
					}
				}
			}
			if (length == num && (unit == GridUnitType.Auto || unit == GridUnitType.Star))
			{
				value = 1.0;
				return;
			}
			string value2 = text.Substring(0, length - num);
			value = Convert.ToDouble(value2, cultureInfo) * num2;
		}

		// Token: 0x0400237F RID: 9087
		private static string[] UnitStrings = new string[]
		{
			"auto",
			"px",
			"*"
		};

		// Token: 0x04002380 RID: 9088
		private static string[] PixelUnitStrings = new string[]
		{
			"in",
			"cm",
			"pt"
		};

		// Token: 0x04002381 RID: 9089
		private static double[] PixelUnitFactors = new double[]
		{
			96.0,
			37.79527559055118,
			1.3333333333333333
		};
	}
}
