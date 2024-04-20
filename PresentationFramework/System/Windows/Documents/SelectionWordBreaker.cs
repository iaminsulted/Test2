using System;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000686 RID: 1670
	internal static class SelectionWordBreaker
	{
		// Token: 0x060052B9 RID: 21177 RVA: 0x00258EF4 File Offset: 0x00257EF4
		internal static bool IsAtWordBoundary(char[] text, int position, LogicalDirection insideWordDirection)
		{
			SelectionWordBreaker.CharClass[] classes = SelectionWordBreaker.GetClasses(text);
			if (insideWordDirection == LogicalDirection.Backward)
			{
				if (position == text.Length)
				{
					return true;
				}
				if (position == 0 || SelectionWordBreaker.IsWhiteSpace(text[position - 1], classes[position - 1]))
				{
					return false;
				}
			}
			else
			{
				if (position == 0)
				{
					return true;
				}
				if (position == text.Length || SelectionWordBreaker.IsWhiteSpace(text[position], classes[position]))
				{
					return false;
				}
			}
			ushort[] array = new ushort[2];
			SafeNativeMethods.GetStringTypeEx(0U, 4U, new char[]
			{
				text[position - 1],
				text[position]
			}, 2, array);
			return SelectionWordBreaker.IsWordBoundary(text[position - 1], text[position]) || (!SelectionWordBreaker.IsSameClass(array[0], classes[position - 1], array[1], classes[position]) && !SelectionWordBreaker.IsMidLetter(text, position - 1, classes) && !SelectionWordBreaker.IsMidLetter(text, position, classes));
		}

		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x060052BA RID: 21178 RVA: 0x0010A7E1 File Offset: 0x001097E1
		internal static int MinContextLength
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x00258FA8 File Offset: 0x00257FA8
		private static bool IsWordBoundary(char previousChar, char followingChar)
		{
			bool result = false;
			if (followingChar == '\r')
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x00258FC0 File Offset: 0x00257FC0
		private static bool IsMidLetter(char[] text, int index, SelectionWordBreaker.CharClass[] classes)
		{
			Invariant.Assert(text.Length == classes.Length);
			return (text[index] == '\'' || text[index] == '’' || text[index] == '­') && index > 0 && index + 1 < classes.Length && ((classes[index - 1] == SelectionWordBreaker.CharClass.Alphanumeric && classes[index + 1] == SelectionWordBreaker.CharClass.Alphanumeric) || (text[index] == '"' && SelectionWordBreaker.IsHebrew(text[index - 1]) && SelectionWordBreaker.IsHebrew(text[index + 1])));
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x00259032 File Offset: 0x00258032
		private static bool IsIdeographicCharType(ushort charType3)
		{
			return (charType3 & 304) > 0;
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x00259040 File Offset: 0x00258040
		private static bool IsSameClass(ushort preceedingType3, SelectionWordBreaker.CharClass preceedingClass, ushort followingType3, SelectionWordBreaker.CharClass followingClass)
		{
			bool result = false;
			if (SelectionWordBreaker.IsIdeographicCharType(preceedingType3) && SelectionWordBreaker.IsIdeographicCharType(followingType3))
			{
				ushort num = (preceedingType3 & 496) ^ (followingType3 & 496);
				result = ((preceedingType3 & 240) != 0 && (num == 0 || num == 128 || num == 32 || num == 160));
			}
			else if (!SelectionWordBreaker.IsIdeographicCharType(preceedingType3) && !SelectionWordBreaker.IsIdeographicCharType(followingType3))
			{
				result = ((preceedingClass & SelectionWordBreaker.CharClass.WBF_CLASS) == (followingClass & SelectionWordBreaker.CharClass.WBF_CLASS));
			}
			return result;
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x002590B6 File Offset: 0x002580B6
		private static bool IsWhiteSpace(char ch, SelectionWordBreaker.CharClass charClass)
		{
			return (charClass & SelectionWordBreaker.CharClass.WBF_CLASS) == SelectionWordBreaker.CharClass.Blank && ch != '￼';
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x002590CC File Offset: 0x002580CC
		private static SelectionWordBreaker.CharClass[] GetClasses(char[] text)
		{
			SelectionWordBreaker.CharClass[] array = new SelectionWordBreaker.CharClass[text.Length];
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				SelectionWordBreaker.CharClass charClass;
				if (c < 'Ā')
				{
					charClass = (SelectionWordBreaker.CharClass)SelectionWordBreaker._latinClasses[(int)c];
				}
				else if (SelectionWordBreaker.IsKorean(c))
				{
					charClass = SelectionWordBreaker.CharClass.Alphanumeric;
				}
				else if (SelectionWordBreaker.IsThai(c))
				{
					charClass = SelectionWordBreaker.CharClass.Alphanumeric;
				}
				else if (c == '￼')
				{
					charClass = (SelectionWordBreaker.CharClass.Blank | SelectionWordBreaker.CharClass.WBF_BREAKAFTER);
				}
				else
				{
					ushort[] array2 = new ushort[1];
					SafeNativeMethods.GetStringTypeEx(0U, 1U, new char[]
					{
						c
					}, 1, array2);
					if ((array2[0] & 8) != 0)
					{
						if ((array2[0] & 64) != 0)
						{
							charClass = (SelectionWordBreaker.CharClass.Blank | SelectionWordBreaker.CharClass.WBF_ISWHITE);
						}
						else
						{
							charClass = (SelectionWordBreaker.CharClass.WhiteSpace | SelectionWordBreaker.CharClass.WBF_ISWHITE);
						}
					}
					else if ((array2[0] & 16) != 0 && !SelectionWordBreaker.IsDiacriticOrKashida(c))
					{
						charClass = SelectionWordBreaker.CharClass.Punctuation;
					}
					else
					{
						charClass = SelectionWordBreaker.CharClass.Alphanumeric;
					}
				}
				array[i] = charClass;
			}
			return array;
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x00259188 File Offset: 0x00258188
		private static bool IsDiacriticOrKashida(char ch)
		{
			ushort[] array = new ushort[1];
			SafeNativeMethods.GetStringTypeEx(0U, 4U, new char[]
			{
				ch
			}, 1, array);
			return (array[0] & 519) > 0;
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x002591BC File Offset: 0x002581BC
		private static bool IsInRange(uint lower, char ch, uint upper)
		{
			return lower <= (uint)ch && (uint)ch <= upper;
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x002591CB File Offset: 0x002581CB
		private static bool IsKorean(char ch)
		{
			return SelectionWordBreaker.IsInRange(44032U, ch, 55295U);
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x002591DD File Offset: 0x002581DD
		private static bool IsThai(char ch)
		{
			return SelectionWordBreaker.IsInRange(3584U, ch, 3711U);
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x002591EF File Offset: 0x002581EF
		private static bool IsHebrew(char ch)
		{
			return SelectionWordBreaker.IsInRange(1488U, ch, 1522U);
		}

		// Token: 0x04002EB7 RID: 11959
		private const char LineFeedChar = '\n';

		// Token: 0x04002EB8 RID: 11960
		private const char CarriageReturnChar = '\r';

		// Token: 0x04002EB9 RID: 11961
		private const char QuotationMarkChar = '"';

		// Token: 0x04002EBA RID: 11962
		private const char ApostropheChar = '\'';

		// Token: 0x04002EBB RID: 11963
		private const char SoftHyphenChar = '­';

		// Token: 0x04002EBC RID: 11964
		private const char RightSingleQuotationChar = '’';

		// Token: 0x04002EBD RID: 11965
		private const char ObjectReplacementChar = '￼';

		// Token: 0x04002EBE RID: 11966
		private static readonly byte[] _latinClasses = new byte[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			20,
			0,
			19,
			20,
			20,
			20,
			20,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			50,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			65,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			18,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

		// Token: 0x02000B4D RID: 2893
		[Flags]
		private enum CharClass : byte
		{
			// Token: 0x04004883 RID: 18563
			Alphanumeric = 0,
			// Token: 0x04004884 RID: 18564
			Punctuation = 1,
			// Token: 0x04004885 RID: 18565
			Blank = 2,
			// Token: 0x04004886 RID: 18566
			WhiteSpace = 4,
			// Token: 0x04004887 RID: 18567
			WBF_CLASS = 15,
			// Token: 0x04004888 RID: 18568
			WBF_ISWHITE = 16,
			// Token: 0x04004889 RID: 18569
			WBF_BREAKAFTER = 64
		}
	}
}
