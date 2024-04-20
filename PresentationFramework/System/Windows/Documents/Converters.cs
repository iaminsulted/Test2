using System;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000666 RID: 1638
	internal static class Converters
	{
		// Token: 0x0600504A RID: 20554 RVA: 0x0024C81E File Offset: 0x0024B81E
		internal static double HalfPointToPositivePx(double halfPoint)
		{
			return Converters.TwipToPositivePx(halfPoint * 10.0);
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x0024C830 File Offset: 0x0024B830
		internal static double TwipToPx(double twip)
		{
			return twip / 1440.0 * 96.0;
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x0024C848 File Offset: 0x0024B848
		internal static double TwipToPositivePx(double twip)
		{
			double num = twip / 1440.0 * 96.0;
			if (num < 0.0)
			{
				num = 0.0;
			}
			return num;
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x0024C884 File Offset: 0x0024B884
		internal static double TwipToPositiveVisiblePx(double twip)
		{
			double num = twip / 1440.0 * 96.0;
			if (num < 0.0)
			{
				num = 0.0;
			}
			if (twip > 0.0 && num < 1.0)
			{
				num = 1.0;
			}
			return num;
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x0024C8E0 File Offset: 0x0024B8E0
		internal static string TwipToPxString(double twip)
		{
			return Converters.TwipToPx(twip).ToString("f2", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x0024C908 File Offset: 0x0024B908
		internal static string TwipToPositivePxString(double twip)
		{
			return Converters.TwipToPositivePx(twip).ToString("f2", CultureInfo.InvariantCulture);
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x0024C930 File Offset: 0x0024B930
		internal static string TwipToPositiveVisiblePxString(double twip)
		{
			return Converters.TwipToPositiveVisiblePx(twip).ToString("f2", CultureInfo.InvariantCulture);
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x0024C955 File Offset: 0x0024B955
		internal static double PxToPt(double px)
		{
			return px / 96.0 * 72.0;
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x0024C96C File Offset: 0x0024B96C
		internal static long PxToTwipRounded(double px)
		{
			double num = px / 96.0 * 1440.0;
			if (num < 0.0)
			{
				return (long)(num - 0.5);
			}
			return (long)(num + 0.5);
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x0024C9B4 File Offset: 0x0024B9B4
		internal static long PxToHalfPointRounded(double px)
		{
			double num = px / 96.0 * 1440.0 / 10.0;
			if (num < 0.0)
			{
				return (long)(num - 0.5);
			}
			return (long)(num + 0.5);
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x0024CA08 File Offset: 0x0024BA08
		internal static bool StringToDouble(string s, ref double d)
		{
			bool result = true;
			d = 0.0;
			try
			{
				d = Convert.ToDouble(s, CultureInfo.InvariantCulture);
			}
			catch (OverflowException)
			{
				result = false;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x0024CA58 File Offset: 0x0024BA58
		internal static bool StringToInt(string s, ref int i)
		{
			bool result = true;
			i = 0;
			try
			{
				i = Convert.ToInt32(s, CultureInfo.InvariantCulture);
			}
			catch (OverflowException)
			{
				result = false;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x0024CAA0 File Offset: 0x0024BAA0
		internal static string StringToXMLAttribute(string s)
		{
			if (s.IndexOf('"') == -1)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '"')
				{
					stringBuilder.Append("&quot;");
				}
				else
				{
					stringBuilder.Append(s[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x0024CB00 File Offset: 0x0024BB00
		internal static bool HexStringToInt(string s, ref int i)
		{
			bool result = true;
			i = 0;
			try
			{
				i = int.Parse(s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
			}
			catch (OverflowException)
			{
				result = false;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x0024CB50 File Offset: 0x0024BB50
		internal static string MarkerStyleToString(MarkerStyle ms)
		{
			switch (ms)
			{
			case MarkerStyle.MarkerArabic:
				return "Decimal";
			case MarkerStyle.MarkerUpperRoman:
				return "UpperRoman";
			case MarkerStyle.MarkerLowerRoman:
				return "LowerRoman";
			case MarkerStyle.MarkerUpperAlpha:
				return "UpperLatin";
			case MarkerStyle.MarkerLowerAlpha:
				return "LowerLatin";
			case MarkerStyle.MarkerOrdinal:
				return "Decimal";
			case MarkerStyle.MarkerCardinal:
				return "Decimal";
			default:
				if (ms == MarkerStyle.MarkerBullet)
				{
					return "Disc";
				}
				if (ms != MarkerStyle.MarkerHidden)
				{
					return "Decimal";
				}
				return "None";
			}
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x0024CBCC File Offset: 0x0024BBCC
		internal static string MarkerStyleToOldRTFString(MarkerStyle ms)
		{
			switch (ms)
			{
			case MarkerStyle.MarkerArabic:
				break;
			case MarkerStyle.MarkerUpperRoman:
				return "\\pnlvlbody\\pnucrm";
			case MarkerStyle.MarkerLowerRoman:
				return "\\pnlvlbody\\pnlcrm";
			case MarkerStyle.MarkerUpperAlpha:
				return "\\pnlvlbody\\pnucltr";
			case MarkerStyle.MarkerLowerAlpha:
				return "\\pnlvlbody\\pnlcltr";
			case MarkerStyle.MarkerOrdinal:
				return "\\pnlvlbody\\pnord";
			case MarkerStyle.MarkerCardinal:
				return "\\pnlvlbody\\pncard";
			default:
				if (ms == MarkerStyle.MarkerBullet)
				{
					return "\\pnlvlblt";
				}
				break;
			}
			return "\\pnlvlbody\\pndec";
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x0024CC30 File Offset: 0x0024BC30
		internal static bool ColorToUse(ConverterState converterState, long cb, long cf, long shade, ref Color c)
		{
			ColorTableEntry colorTableEntry = (cb >= 0L) ? converterState.ColorTable.EntryAt((int)cb) : null;
			ColorTableEntry colorTableEntry2 = (cf >= 0L) ? converterState.ColorTable.EntryAt((int)cf) : null;
			if (shade < 0L)
			{
				if (colorTableEntry == null)
				{
					return false;
				}
				c = colorTableEntry.Color;
				return true;
			}
			else
			{
				Color color = (colorTableEntry != null) ? colorTableEntry.Color : Color.FromArgb(byte.MaxValue, 0, 0, 0);
				Color color2 = (colorTableEntry2 != null) ? colorTableEntry2.Color : Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				if (colorTableEntry2 == null && colorTableEntry == null)
				{
					c = Color.FromArgb(byte.MaxValue, (byte)(255L - 255L * shade / 10000L), (byte)(255L - 255L * shade / 10000L), (byte)(255L - 255L * shade / 10000L));
					return true;
				}
				if (colorTableEntry == null)
				{
					c = Color.FromArgb(byte.MaxValue, (byte)((ulong)color2.R + (ulong)((long)(byte.MaxValue - color2.R) * (10000L - shade) / 10000L)), (byte)((ulong)color2.G + (ulong)((long)(byte.MaxValue - color2.G) * (10000L - shade) / 10000L)), (byte)((ulong)color2.B + (ulong)((long)(byte.MaxValue - color2.B) * (10000L - shade) / 10000L)));
					return true;
				}
				if (colorTableEntry2 == null)
				{
					c = Color.FromArgb(byte.MaxValue, (byte)((ulong)color.R - (ulong)color.R * (ulong)shade / 10000UL), (byte)((ulong)color.G - (ulong)color.G * (ulong)shade / 10000UL), (byte)((ulong)color.B - (ulong)color.B * (ulong)shade / 10000UL));
					return true;
				}
				c = Color.FromArgb(byte.MaxValue, (byte)((ulong)color.R * (ulong)(10000L - shade) / 10000UL + (ulong)color2.R * (ulong)shade / 10000UL), (byte)((ulong)color.G * (ulong)(10000L - shade) / 10000UL + (ulong)color2.G * (ulong)shade / 10000UL), (byte)((ulong)color.B * (ulong)(10000L - shade) / 10000UL + (ulong)color2.B * (ulong)shade / 10000UL));
				return true;
			}
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x0024CEAC File Offset: 0x0024BEAC
		internal static string AlignmentToString(HAlign a, DirState ds)
		{
			switch (a)
			{
			case HAlign.AlignLeft:
				if (ds == DirState.DirRTL)
				{
					return "Right";
				}
				return "Left";
			case HAlign.AlignRight:
				if (ds == DirState.DirRTL)
				{
					return "Left";
				}
				return "Right";
			case HAlign.AlignCenter:
				return "Center";
			case HAlign.AlignJustify:
				return "Justify";
			}
			return "";
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x0024CF08 File Offset: 0x0024BF08
		internal static string MarkerCountToString(MarkerStyle ms, long nCount)
		{
			StringBuilder sb = new StringBuilder();
			if (nCount < 0L)
			{
				nCount = 0L;
			}
			switch (ms)
			{
			case MarkerStyle.MarkerNone:
				break;
			case MarkerStyle.MarkerArabic:
			case MarkerStyle.MarkerOrdinal:
			case MarkerStyle.MarkerCardinal:
				return nCount.ToString(CultureInfo.InvariantCulture);
			case MarkerStyle.MarkerUpperRoman:
			case MarkerStyle.MarkerLowerRoman:
				return Converters.MarkerRomanCountToString(sb, ms, nCount);
			case MarkerStyle.MarkerUpperAlpha:
			case MarkerStyle.MarkerLowerAlpha:
				return Converters.MarkerAlphaCountToString(sb, ms, nCount);
			default:
				if (ms != MarkerStyle.MarkerBullet)
				{
					if (ms == MarkerStyle.MarkerHidden)
					{
						break;
					}
				}
				return "\\'B7";
			}
			return "";
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x0024CF88 File Offset: 0x0024BF88
		private static string MarkerRomanCountToString(StringBuilder sb, MarkerStyle ms, long nCount)
		{
			while (nCount >= 1000L)
			{
				sb.Append("M");
				nCount -= 1000L;
			}
			long num = nCount / 100L;
			long num2 = num;
			if (num2 <= 9L)
			{
				switch ((uint)num2)
				{
				case 1U:
					sb.Append("C");
					break;
				case 2U:
					sb.Append("CC");
					break;
				case 3U:
					sb.Append("CCC");
					break;
				case 4U:
					sb.Append("CD");
					break;
				case 5U:
					sb.Append("D");
					break;
				case 6U:
					sb.Append("DC");
					break;
				case 7U:
					sb.Append("DCC");
					break;
				case 8U:
					sb.Append("DCCC");
					break;
				case 9U:
					sb.Append("CM");
					break;
				}
			}
			nCount %= 100L;
			num = nCount / 10L;
			long num3 = num;
			if (num3 <= 9L)
			{
				switch ((uint)num3)
				{
				case 1U:
					sb.Append("X");
					break;
				case 2U:
					sb.Append("XX");
					break;
				case 3U:
					sb.Append("XXX");
					break;
				case 4U:
					sb.Append("XL");
					break;
				case 5U:
					sb.Append("L");
					break;
				case 6U:
					sb.Append("LX");
					break;
				case 7U:
					sb.Append("LXX");
					break;
				case 8U:
					sb.Append("LXXX");
					break;
				case 9U:
					sb.Append("XC");
					break;
				}
			}
			nCount %= 10L;
			long num4 = nCount;
			if (num4 <= 9L)
			{
				switch ((uint)num4)
				{
				case 1U:
					sb.Append("I");
					break;
				case 2U:
					sb.Append("II");
					break;
				case 3U:
					sb.Append("III");
					break;
				case 4U:
					sb.Append("IV");
					break;
				case 5U:
					sb.Append("V");
					break;
				case 6U:
					sb.Append("VI");
					break;
				case 7U:
					sb.Append("VII");
					break;
				case 8U:
					sb.Append("VIII");
					break;
				case 9U:
					sb.Append("IX");
					break;
				}
			}
			if (ms == MarkerStyle.MarkerUpperRoman)
			{
				return sb.ToString();
			}
			return sb.ToString().ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0024D218 File Offset: 0x0024C218
		private static string MarkerAlphaCountToString(StringBuilder sb, MarkerStyle ms, long nCount)
		{
			int num = 26;
			int num2 = 676;
			int num3 = 17576;
			int num4 = 456976;
			char[] array = new char[1];
			int num5 = 0;
			while (nCount > (long)(num4 + num3 + num2 + num))
			{
				num5++;
				nCount -= (long)num4;
			}
			if (num5 > 0)
			{
				if (num5 > 26)
				{
					num5 = 26;
				}
				array[0] = (char)(65 + (num5 - 1));
				sb.Append(array);
			}
			num5 = 0;
			while (nCount > (long)(num3 + num2 + num))
			{
				num5++;
				nCount -= (long)num3;
			}
			if (num5 > 0)
			{
				array[0] = (char)(65 + (num5 - 1));
				sb.Append(array);
			}
			num5 = 0;
			while (nCount > (long)(num2 + num))
			{
				num5++;
				nCount -= (long)num2;
			}
			if (num5 > 0)
			{
				array[0] = (char)(65 + (num5 - 1));
				sb.Append(array);
			}
			num5 = 0;
			while (nCount > (long)num)
			{
				num5++;
				nCount -= (long)num;
			}
			if (num5 > 0)
			{
				array[0] = (char)(65 + (num5 - 1));
				sb.Append(array);
			}
			array[0] = (char)(65L + (nCount - 1L));
			sb.Append(array);
			if (ms == MarkerStyle.MarkerUpperAlpha)
			{
				return sb.ToString();
			}
			return sb.ToString().ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x0024D34C File Offset: 0x0024C34C
		internal static void ByteToHex(byte byteData, out byte firstHexByte, out byte secondHexByte)
		{
			firstHexByte = (byte)(byteData >> 4 & 15);
			secondHexByte = (byteData & 15);
			if (firstHexByte >= 0 && firstHexByte <= 9)
			{
				firstHexByte += 48;
			}
			else if (firstHexByte >= 10 && firstHexByte <= 15)
			{
				firstHexByte += 87;
			}
			if (secondHexByte >= 0 && secondHexByte <= 9)
			{
				secondHexByte += 48;
				return;
			}
			if (secondHexByte >= 10 && secondHexByte <= 15)
			{
				secondHexByte += 87;
			}
		}
	}
}
