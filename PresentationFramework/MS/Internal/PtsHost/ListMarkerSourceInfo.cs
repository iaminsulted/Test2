using System;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200012A RID: 298
	internal sealed class ListMarkerSourceInfo
	{
		// Token: 0x06000817 RID: 2071 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private ListMarkerSourceInfo()
		{
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00113768 File Offset: 0x00112768
		internal static Thickness CalculatePadding(List list, double lineHeight, double pixelsPerDip)
		{
			return new Thickness((double)((int)((ListMarkerSourceInfo.GetFormattedMarker(list, pixelsPerDip).Width + 1.5 * lineHeight) / lineHeight) + 1) * lineHeight, 0.0, 0.0, 0.0);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x001137B8 File Offset: 0x001127B8
		private static FormattedText GetFormattedMarker(List list, double pixelsPerDip)
		{
			string textToFormat = "";
			FormattedText result;
			if (ListMarkerSourceInfo.IsKnownSymbolMarkerStyle(list.MarkerStyle))
			{
				switch (list.MarkerStyle)
				{
				case TextMarkerStyle.Disc:
					textToFormat = "\u009f";
					break;
				case TextMarkerStyle.Circle:
					textToFormat = "¡";
					break;
				case TextMarkerStyle.Square:
					textToFormat = "q";
					break;
				case TextMarkerStyle.Box:
					textToFormat = "§";
					break;
				}
				Typeface modifiedTypeface = DynamicPropertyReader.GetModifiedTypeface(list, new FontFamily("Wingdings"));
				result = new FormattedText(textToFormat, DynamicPropertyReader.GetCultureInfo(list), list.FlowDirection, modifiedTypeface, list.FontSize, list.Foreground, pixelsPerDip);
			}
			else if (ListMarkerSourceInfo.IsKnownIndexMarkerStyle(list.MarkerStyle))
			{
				int startIndex = list.StartIndex;
				Invariant.Assert(startIndex > 0);
				int count = list.ListItems.Count;
				int num;
				if (2147483647 - count < startIndex)
				{
					num = int.MaxValue;
				}
				else
				{
					num = ((count == 0) ? startIndex : (startIndex + count - 1));
				}
				switch (list.MarkerStyle)
				{
				case TextMarkerStyle.LowerRoman:
					textToFormat = ListMarkerSourceInfo.GetStringForLargestRomanMarker(startIndex, num, false);
					break;
				case TextMarkerStyle.UpperRoman:
					textToFormat = ListMarkerSourceInfo.GetStringForLargestRomanMarker(startIndex, num, true);
					break;
				case TextMarkerStyle.LowerLatin:
					textToFormat = ListMarkerSourceInfo.ConvertNumberToString(num, true, ListMarkerSourceInfo.LowerLatinNumerics);
					break;
				case TextMarkerStyle.UpperLatin:
					textToFormat = ListMarkerSourceInfo.ConvertNumberToString(num, true, ListMarkerSourceInfo.UpperLatinNumerics);
					break;
				case TextMarkerStyle.Decimal:
					textToFormat = ListMarkerSourceInfo.ConvertNumberToString(num, false, ListMarkerSourceInfo.DecimalNumerics);
					break;
				}
				result = new FormattedText(textToFormat, DynamicPropertyReader.GetCultureInfo(list), list.FlowDirection, DynamicPropertyReader.GetTypeface(list), list.FontSize, list.Foreground, pixelsPerDip);
			}
			else
			{
				textToFormat = "\u009f";
				Typeface modifiedTypeface2 = DynamicPropertyReader.GetModifiedTypeface(list, new FontFamily("Wingdings"));
				result = new FormattedText(textToFormat, DynamicPropertyReader.GetCultureInfo(list), list.FlowDirection, modifiedTypeface2, list.FontSize, list.Foreground, pixelsPerDip);
			}
			return result;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00113974 File Offset: 0x00112974
		private static string ConvertNumberToString(int number, bool oneBased, string numericSymbols)
		{
			if (oneBased)
			{
				number--;
			}
			Invariant.Assert(number >= 0);
			int length = numericSymbols.Length;
			char[] array;
			if (number < length)
			{
				array = new char[]
				{
					numericSymbols[number],
					ListMarkerSourceInfo.NumberSuffix
				};
			}
			else
			{
				int num = oneBased ? 1 : 0;
				int num2 = 1;
				long num3 = (long)length;
				long num4 = (long)length;
				while ((long)number >= num3)
				{
					num4 *= (long)length;
					num3 = num4 + num3 * (long)num;
					num2++;
				}
				array = new char[num2 + 1];
				array[num2] = ListMarkerSourceInfo.NumberSuffix;
				for (int i = num2 - 1; i >= 0; i--)
				{
					array[i] = numericSymbols[number % length];
					number = number / length - num;
				}
			}
			return new string(array);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00113A28 File Offset: 0x00112A28
		private static string GetStringForLargestRomanMarker(int startIndex, int highestIndex, bool uppercase)
		{
			if (highestIndex <= 3999)
			{
				int indexForLargestRomanMarker = ListMarkerSourceInfo.GetIndexForLargestRomanMarker(startIndex, highestIndex);
				return ListMarkerSourceInfo.ConvertNumberToRomanString(indexForLargestRomanMarker, uppercase);
			}
			if (!uppercase)
			{
				return ListMarkerSourceInfo.LargestRomanMarkerLower;
			}
			return ListMarkerSourceInfo.LargestRomanMarkerUpper;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00113A60 File Offset: 0x00112A60
		private static int GetIndexForLargestRomanMarker(int startIndex, int highestIndex)
		{
			int num = 0;
			if (startIndex == 1)
			{
				int num2 = highestIndex / 1000;
				highestIndex %= 1000;
				for (int i = 0; i < ListMarkerSourceInfo.RomanNumericSizeIncrements.Length; i++)
				{
					Invariant.Assert(highestIndex >= ListMarkerSourceInfo.RomanNumericSizeIncrements[i]);
					if (highestIndex == ListMarkerSourceInfo.RomanNumericSizeIncrements[i])
					{
						num = highestIndex;
						break;
					}
					Invariant.Assert(highestIndex > ListMarkerSourceInfo.RomanNumericSizeIncrements[i]);
					if (i >= ListMarkerSourceInfo.RomanNumericSizeIncrements.Length - 1 || highestIndex < ListMarkerSourceInfo.RomanNumericSizeIncrements[i + 1])
					{
						num = ListMarkerSourceInfo.RomanNumericSizeIncrements[i];
						break;
					}
				}
				if (num2 > 0)
				{
					num = num2 * 1000 + num;
				}
			}
			else
			{
				int num3 = 0;
				for (int j = startIndex; j <= highestIndex; j++)
				{
					string text = ListMarkerSourceInfo.ConvertNumberToRomanString(j, true);
					if (text.Length > num3)
					{
						num = j;
						num3 = text.Length;
					}
				}
			}
			Invariant.Assert(num > 0);
			return num;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00113B38 File Offset: 0x00112B38
		private static string ConvertNumberToRomanString(int number, bool uppercase)
		{
			Invariant.Assert(number <= 3999);
			StringBuilder stringBuilder = new StringBuilder();
			ListMarkerSourceInfo.AddRomanNumeric(stringBuilder, number / 1000, ListMarkerSourceInfo.RomanNumerics[uppercase ? 1 : 0][0]);
			number %= 1000;
			ListMarkerSourceInfo.AddRomanNumeric(stringBuilder, number / 100, ListMarkerSourceInfo.RomanNumerics[uppercase ? 1 : 0][1]);
			number %= 100;
			ListMarkerSourceInfo.AddRomanNumeric(stringBuilder, number / 10, ListMarkerSourceInfo.RomanNumerics[uppercase ? 1 : 0][2]);
			number %= 10;
			ListMarkerSourceInfo.AddRomanNumeric(stringBuilder, number, ListMarkerSourceInfo.RomanNumerics[uppercase ? 1 : 0][3]);
			stringBuilder.Append(ListMarkerSourceInfo.NumberSuffix);
			return stringBuilder.ToString();
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00113BE4 File Offset: 0x00112BE4
		private static void AddRomanNumeric(StringBuilder builder, int number, string oneFiveTen)
		{
			if (number >= 1 && number <= 9)
			{
				if (number == 4 || number == 9)
				{
					builder.Append(oneFiveTen[0]);
				}
				if (number == 9)
				{
					builder.Append(oneFiveTen[2]);
					return;
				}
				if (number >= 4)
				{
					builder.Append(oneFiveTen[1]);
				}
				int num = number % 5;
				while (num > 0 && num < 4)
				{
					builder.Append(oneFiveTen[0]);
					num--;
				}
			}
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00113C57 File Offset: 0x00112C57
		private static bool IsKnownSymbolMarkerStyle(TextMarkerStyle markerStyle)
		{
			return markerStyle == TextMarkerStyle.Disc || markerStyle == TextMarkerStyle.Circle || markerStyle == TextMarkerStyle.Square || markerStyle == TextMarkerStyle.Box;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00113C6B File Offset: 0x00112C6B
		private static bool IsKnownIndexMarkerStyle(TextMarkerStyle markerStyle)
		{
			return markerStyle == TextMarkerStyle.Decimal || markerStyle == TextMarkerStyle.LowerLatin || markerStyle == TextMarkerStyle.UpperLatin || markerStyle == TextMarkerStyle.LowerRoman || markerStyle == TextMarkerStyle.UpperRoman;
		}

		// Token: 0x040007A4 RID: 1956
		private static char NumberSuffix = '.';

		// Token: 0x040007A5 RID: 1957
		private static string DecimalNumerics = "0123456789";

		// Token: 0x040007A6 RID: 1958
		private static string LowerLatinNumerics = "abcdefghijklmnopqrstuvwxyz";

		// Token: 0x040007A7 RID: 1959
		private static string UpperLatinNumerics = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		// Token: 0x040007A8 RID: 1960
		private static string LargestRomanMarkerUpper = "MMMDCCCLXXXVIII";

		// Token: 0x040007A9 RID: 1961
		private static string LargestRomanMarkerLower = "mmmdccclxxxviii";

		// Token: 0x040007AA RID: 1962
		private static string[][] RomanNumerics = new string[][]
		{
			new string[]
			{
				"m??",
				"cdm",
				"xlc",
				"ivx"
			},
			new string[]
			{
				"M??",
				"CDM",
				"XLC",
				"IVX"
			}
		};

		// Token: 0x040007AB RID: 1963
		private static int[] RomanNumericSizeIncrements = new int[]
		{
			1,
			2,
			3,
			8,
			18,
			28,
			38,
			88,
			188,
			288,
			388,
			888
		};
	}
}
