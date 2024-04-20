using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace System.Windows
{
	// Token: 0x020003B5 RID: 949
	public static class SystemFonts
	{
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060026BC RID: 9916 RVA: 0x0018C1C8 File Offset: 0x0018B1C8
		public static double IconFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.IconMetrics.lfFont.lfHeight);
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060026BD RID: 9917 RVA: 0x0018C1DE File Offset: 0x0018B1DE
		public static FontFamily IconFontFamily
		{
			get
			{
				if (SystemFonts._iconFontFamily == null)
				{
					SystemFonts._iconFontFamily = new FontFamily(SystemParameters.IconMetrics.lfFont.lfFaceName);
				}
				return SystemFonts._iconFontFamily;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x0018C205 File Offset: 0x0018B205
		public static FontStyle IconFontStyle
		{
			get
			{
				if (SystemParameters.IconMetrics.lfFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060026BF RID: 9919 RVA: 0x0018C223 File Offset: 0x0018B223
		public static FontWeight IconFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.IconMetrics.lfFont.lfWeight);
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x0018C23C File Offset: 0x0018B23C
		public static TextDecorationCollection IconFontTextDecorations
		{
			get
			{
				if (SystemFonts._iconFontTextDecorations == null)
				{
					SystemFonts._iconFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.IconMetrics.lfFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._iconFontTextDecorations);
					}
					if (SystemParameters.IconMetrics.lfFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._iconFontTextDecorations);
					}
					SystemFonts._iconFontTextDecorations.Freeze();
				}
				return SystemFonts._iconFontTextDecorations;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060026C1 RID: 9921 RVA: 0x0018C2A9 File Offset: 0x0018B2A9
		public static double CaptionFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfCaptionFont.lfHeight);
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x0018C2BF File Offset: 0x0018B2BF
		public static FontFamily CaptionFontFamily
		{
			get
			{
				if (SystemFonts._captionFontFamily == null)
				{
					SystemFonts._captionFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfCaptionFont.lfFaceName);
				}
				return SystemFonts._captionFontFamily;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060026C3 RID: 9923 RVA: 0x0018C2E6 File Offset: 0x0018B2E6
		public static FontStyle CaptionFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfCaptionFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060026C4 RID: 9924 RVA: 0x0018C304 File Offset: 0x0018B304
		public static FontWeight CaptionFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfCaptionFont.lfWeight);
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060026C5 RID: 9925 RVA: 0x0018C31C File Offset: 0x0018B31C
		public static TextDecorationCollection CaptionFontTextDecorations
		{
			get
			{
				if (SystemFonts._captionFontTextDecorations == null)
				{
					SystemFonts._captionFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfCaptionFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._captionFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfCaptionFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._captionFontTextDecorations);
					}
					SystemFonts._captionFontTextDecorations.Freeze();
				}
				return SystemFonts._captionFontTextDecorations;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060026C6 RID: 9926 RVA: 0x0018C389 File Offset: 0x0018B389
		public static double SmallCaptionFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfSmCaptionFont.lfHeight);
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060026C7 RID: 9927 RVA: 0x0018C39F File Offset: 0x0018B39F
		public static FontFamily SmallCaptionFontFamily
		{
			get
			{
				if (SystemFonts._smallCaptionFontFamily == null)
				{
					SystemFonts._smallCaptionFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfSmCaptionFont.lfFaceName);
				}
				return SystemFonts._smallCaptionFontFamily;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x0018C3C6 File Offset: 0x0018B3C6
		public static FontStyle SmallCaptionFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfSmCaptionFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060026C9 RID: 9929 RVA: 0x0018C3E4 File Offset: 0x0018B3E4
		public static FontWeight SmallCaptionFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfSmCaptionFont.lfWeight);
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060026CA RID: 9930 RVA: 0x0018C3FC File Offset: 0x0018B3FC
		public static TextDecorationCollection SmallCaptionFontTextDecorations
		{
			get
			{
				if (SystemFonts._smallCaptionFontTextDecorations == null)
				{
					SystemFonts._smallCaptionFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfSmCaptionFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._smallCaptionFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfSmCaptionFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._smallCaptionFontTextDecorations);
					}
					SystemFonts._smallCaptionFontTextDecorations.Freeze();
				}
				return SystemFonts._smallCaptionFontTextDecorations;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060026CB RID: 9931 RVA: 0x0018C469 File Offset: 0x0018B469
		public static double MenuFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfMenuFont.lfHeight);
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x0018C47F File Offset: 0x0018B47F
		public static FontFamily MenuFontFamily
		{
			get
			{
				if (SystemFonts._menuFontFamily == null)
				{
					SystemFonts._menuFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfMenuFont.lfFaceName);
				}
				return SystemFonts._menuFontFamily;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060026CD RID: 9933 RVA: 0x0018C4A6 File Offset: 0x0018B4A6
		public static FontStyle MenuFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfMenuFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x0018C4C4 File Offset: 0x0018B4C4
		public static FontWeight MenuFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfMenuFont.lfWeight);
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x060026CF RID: 9935 RVA: 0x0018C4DC File Offset: 0x0018B4DC
		public static TextDecorationCollection MenuFontTextDecorations
		{
			get
			{
				if (SystemFonts._menuFontTextDecorations == null)
				{
					SystemFonts._menuFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfMenuFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._menuFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfMenuFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._menuFontTextDecorations);
					}
					SystemFonts._menuFontTextDecorations.Freeze();
				}
				return SystemFonts._menuFontTextDecorations;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x0018C549 File Offset: 0x0018B549
		public static double StatusFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfStatusFont.lfHeight);
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x0018C55F File Offset: 0x0018B55F
		public static FontFamily StatusFontFamily
		{
			get
			{
				if (SystemFonts._statusFontFamily == null)
				{
					SystemFonts._statusFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfStatusFont.lfFaceName);
				}
				return SystemFonts._statusFontFamily;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x0018C586 File Offset: 0x0018B586
		public static FontStyle StatusFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfStatusFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x060026D3 RID: 9939 RVA: 0x0018C5A4 File Offset: 0x0018B5A4
		public static FontWeight StatusFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfStatusFont.lfWeight);
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x060026D4 RID: 9940 RVA: 0x0018C5BC File Offset: 0x0018B5BC
		public static TextDecorationCollection StatusFontTextDecorations
		{
			get
			{
				if (SystemFonts._statusFontTextDecorations == null)
				{
					SystemFonts._statusFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfStatusFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._statusFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfStatusFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._statusFontTextDecorations);
					}
					SystemFonts._statusFontTextDecorations.Freeze();
				}
				return SystemFonts._statusFontTextDecorations;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x0018C629 File Offset: 0x0018B629
		public static double MessageFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfMessageFont.lfHeight);
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x060026D6 RID: 9942 RVA: 0x0018C63F File Offset: 0x0018B63F
		public static FontFamily MessageFontFamily
		{
			get
			{
				if (SystemFonts._messageFontFamily == null)
				{
					SystemFonts._messageFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfMessageFont.lfFaceName);
				}
				return SystemFonts._messageFontFamily;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x060026D7 RID: 9943 RVA: 0x0018C666 File Offset: 0x0018B666
		public static FontStyle MessageFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfMessageFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x0018C684 File Offset: 0x0018B684
		public static FontWeight MessageFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfMessageFont.lfWeight);
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x060026D9 RID: 9945 RVA: 0x0018C69C File Offset: 0x0018B69C
		public static TextDecorationCollection MessageFontTextDecorations
		{
			get
			{
				if (SystemFonts._messageFontTextDecorations == null)
				{
					SystemFonts._messageFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfMessageFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._messageFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfMessageFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._messageFontTextDecorations);
					}
					SystemFonts._messageFontTextDecorations.Freeze();
				}
				return SystemFonts._messageFontTextDecorations;
			}
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x0018C70C File Offset: 0x0018B70C
		private static void CopyTextDecorationCollection(TextDecorationCollection from, TextDecorationCollection to)
		{
			int count = from.Count;
			for (int i = 0; i < count; i++)
			{
				to.Add(from[i]);
			}
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x0018B567 File Offset: 0x0018A567
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
		{
			return new SystemResourceKey(KeyId);
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x0018C739 File Offset: 0x0018B739
		public static ResourceKey IconFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheIconFontSize == null)
				{
					SystemFonts._cacheIconFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontSize);
				}
				return SystemFonts._cacheIconFontSize;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x060026DD RID: 9949 RVA: 0x0018C753 File Offset: 0x0018B753
		public static ResourceKey IconFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheIconFontFamily == null)
				{
					SystemFonts._cacheIconFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontFamily);
				}
				return SystemFonts._cacheIconFontFamily;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x060026DE RID: 9950 RVA: 0x0018C76D File Offset: 0x0018B76D
		public static ResourceKey IconFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheIconFontStyle == null)
				{
					SystemFonts._cacheIconFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontStyle);
				}
				return SystemFonts._cacheIconFontStyle;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x060026DF RID: 9951 RVA: 0x0018C787 File Offset: 0x0018B787
		public static ResourceKey IconFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheIconFontWeight == null)
				{
					SystemFonts._cacheIconFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontWeight);
				}
				return SystemFonts._cacheIconFontWeight;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x060026E0 RID: 9952 RVA: 0x0018C7A1 File Offset: 0x0018B7A1
		public static ResourceKey IconFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheIconFontTextDecorations == null)
				{
					SystemFonts._cacheIconFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontTextDecorations);
				}
				return SystemFonts._cacheIconFontTextDecorations;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x0018C7BB File Offset: 0x0018B7BB
		public static ResourceKey CaptionFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontSize == null)
				{
					SystemFonts._cacheCaptionFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontSize);
				}
				return SystemFonts._cacheCaptionFontSize;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x060026E2 RID: 9954 RVA: 0x0018C7D5 File Offset: 0x0018B7D5
		public static ResourceKey CaptionFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontFamily == null)
				{
					SystemFonts._cacheCaptionFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontFamily);
				}
				return SystemFonts._cacheCaptionFontFamily;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x060026E3 RID: 9955 RVA: 0x0018C7EF File Offset: 0x0018B7EF
		public static ResourceKey CaptionFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontStyle == null)
				{
					SystemFonts._cacheCaptionFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontStyle);
				}
				return SystemFonts._cacheCaptionFontStyle;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x060026E4 RID: 9956 RVA: 0x0018C809 File Offset: 0x0018B809
		public static ResourceKey CaptionFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontWeight == null)
				{
					SystemFonts._cacheCaptionFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontWeight);
				}
				return SystemFonts._cacheCaptionFontWeight;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x060026E5 RID: 9957 RVA: 0x0018C823 File Offset: 0x0018B823
		public static ResourceKey CaptionFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontTextDecorations == null)
				{
					SystemFonts._cacheCaptionFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontTextDecorations);
				}
				return SystemFonts._cacheCaptionFontTextDecorations;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x060026E6 RID: 9958 RVA: 0x0018C83D File Offset: 0x0018B83D
		public static ResourceKey SmallCaptionFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontSize == null)
				{
					SystemFonts._cacheSmallCaptionFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontSize);
				}
				return SystemFonts._cacheSmallCaptionFontSize;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x060026E7 RID: 9959 RVA: 0x0018C857 File Offset: 0x0018B857
		public static ResourceKey SmallCaptionFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontFamily == null)
				{
					SystemFonts._cacheSmallCaptionFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontFamily);
				}
				return SystemFonts._cacheSmallCaptionFontFamily;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x060026E8 RID: 9960 RVA: 0x0018C871 File Offset: 0x0018B871
		public static ResourceKey SmallCaptionFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontStyle == null)
				{
					SystemFonts._cacheSmallCaptionFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontStyle);
				}
				return SystemFonts._cacheSmallCaptionFontStyle;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x0018C88B File Offset: 0x0018B88B
		public static ResourceKey SmallCaptionFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontWeight == null)
				{
					SystemFonts._cacheSmallCaptionFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontWeight);
				}
				return SystemFonts._cacheSmallCaptionFontWeight;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x0018C8A5 File Offset: 0x0018B8A5
		public static ResourceKey SmallCaptionFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontTextDecorations == null)
				{
					SystemFonts._cacheSmallCaptionFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontTextDecorations);
				}
				return SystemFonts._cacheSmallCaptionFontTextDecorations;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x0018C8BF File Offset: 0x0018B8BF
		public static ResourceKey MenuFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontSize == null)
				{
					SystemFonts._cacheMenuFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontSize);
				}
				return SystemFonts._cacheMenuFontSize;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x060026EC RID: 9964 RVA: 0x0018C8D9 File Offset: 0x0018B8D9
		public static ResourceKey MenuFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontFamily == null)
				{
					SystemFonts._cacheMenuFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontFamily);
				}
				return SystemFonts._cacheMenuFontFamily;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x0018C8F3 File Offset: 0x0018B8F3
		public static ResourceKey MenuFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontStyle == null)
				{
					SystemFonts._cacheMenuFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontStyle);
				}
				return SystemFonts._cacheMenuFontStyle;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x0018C90D File Offset: 0x0018B90D
		public static ResourceKey MenuFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontWeight == null)
				{
					SystemFonts._cacheMenuFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontWeight);
				}
				return SystemFonts._cacheMenuFontWeight;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x0018C927 File Offset: 0x0018B927
		public static ResourceKey MenuFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontTextDecorations == null)
				{
					SystemFonts._cacheMenuFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontTextDecorations);
				}
				return SystemFonts._cacheMenuFontTextDecorations;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x0018C941 File Offset: 0x0018B941
		public static ResourceKey StatusFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontSize == null)
				{
					SystemFonts._cacheStatusFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontSize);
				}
				return SystemFonts._cacheStatusFontSize;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x0018C95B File Offset: 0x0018B95B
		public static ResourceKey StatusFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontFamily == null)
				{
					SystemFonts._cacheStatusFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontFamily);
				}
				return SystemFonts._cacheStatusFontFamily;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x060026F2 RID: 9970 RVA: 0x0018C975 File Offset: 0x0018B975
		public static ResourceKey StatusFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontStyle == null)
				{
					SystemFonts._cacheStatusFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontStyle);
				}
				return SystemFonts._cacheStatusFontStyle;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x0018C98F File Offset: 0x0018B98F
		public static ResourceKey StatusFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontWeight == null)
				{
					SystemFonts._cacheStatusFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontWeight);
				}
				return SystemFonts._cacheStatusFontWeight;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x0018C9A9 File Offset: 0x0018B9A9
		public static ResourceKey StatusFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontTextDecorations == null)
				{
					SystemFonts._cacheStatusFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontTextDecorations);
				}
				return SystemFonts._cacheStatusFontTextDecorations;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x0018C9C3 File Offset: 0x0018B9C3
		public static ResourceKey MessageFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontSize == null)
				{
					SystemFonts._cacheMessageFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontSize);
				}
				return SystemFonts._cacheMessageFontSize;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x0018C9DD File Offset: 0x0018B9DD
		public static ResourceKey MessageFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontFamily == null)
				{
					SystemFonts._cacheMessageFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontFamily);
				}
				return SystemFonts._cacheMessageFontFamily;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x060026F7 RID: 9975 RVA: 0x0018C9F7 File Offset: 0x0018B9F7
		public static ResourceKey MessageFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontStyle == null)
				{
					SystemFonts._cacheMessageFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontStyle);
				}
				return SystemFonts._cacheMessageFontStyle;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x0018CA11 File Offset: 0x0018BA11
		public static ResourceKey MessageFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontWeight == null)
				{
					SystemFonts._cacheMessageFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontWeight);
				}
				return SystemFonts._cacheMessageFontWeight;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x060026F9 RID: 9977 RVA: 0x0018CA2B File Offset: 0x0018BA2B
		public static ResourceKey MessageFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontTextDecorations == null)
				{
					SystemFonts._cacheMessageFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontTextDecorations);
				}
				return SystemFonts._cacheMessageFontTextDecorations;
			}
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x0018CA48 File Offset: 0x0018BA48
		private static double ConvertFontHeight(int height)
		{
			int dpi = SystemParameters.Dpi;
			if (dpi != 0)
			{
				return (double)(Math.Abs(height) * 96 / dpi);
			}
			return 11.0;
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x0018CA74 File Offset: 0x0018BA74
		internal static void InvalidateIconMetrics()
		{
			SystemFonts._iconFontTextDecorations = null;
			SystemFonts._iconFontFamily = null;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x0018CA82 File Offset: 0x0018BA82
		internal static void InvalidateNonClientMetrics()
		{
			SystemFonts._messageFontTextDecorations = null;
			SystemFonts._statusFontTextDecorations = null;
			SystemFonts._menuFontTextDecorations = null;
			SystemFonts._smallCaptionFontTextDecorations = null;
			SystemFonts._captionFontTextDecorations = null;
			SystemFonts._messageFontFamily = null;
			SystemFonts._statusFontFamily = null;
			SystemFonts._menuFontFamily = null;
			SystemFonts._smallCaptionFontFamily = null;
			SystemFonts._captionFontFamily = null;
		}

		// Token: 0x04001241 RID: 4673
		private const double FallbackFontSize = 11.0;

		// Token: 0x04001242 RID: 4674
		private static TextDecorationCollection _iconFontTextDecorations;

		// Token: 0x04001243 RID: 4675
		private static TextDecorationCollection _messageFontTextDecorations;

		// Token: 0x04001244 RID: 4676
		private static TextDecorationCollection _statusFontTextDecorations;

		// Token: 0x04001245 RID: 4677
		private static TextDecorationCollection _menuFontTextDecorations;

		// Token: 0x04001246 RID: 4678
		private static TextDecorationCollection _smallCaptionFontTextDecorations;

		// Token: 0x04001247 RID: 4679
		private static TextDecorationCollection _captionFontTextDecorations;

		// Token: 0x04001248 RID: 4680
		private static FontFamily _iconFontFamily;

		// Token: 0x04001249 RID: 4681
		private static FontFamily _messageFontFamily;

		// Token: 0x0400124A RID: 4682
		private static FontFamily _statusFontFamily;

		// Token: 0x0400124B RID: 4683
		private static FontFamily _menuFontFamily;

		// Token: 0x0400124C RID: 4684
		private static FontFamily _smallCaptionFontFamily;

		// Token: 0x0400124D RID: 4685
		private static FontFamily _captionFontFamily;

		// Token: 0x0400124E RID: 4686
		private static SystemResourceKey _cacheIconFontSize;

		// Token: 0x0400124F RID: 4687
		private static SystemResourceKey _cacheIconFontFamily;

		// Token: 0x04001250 RID: 4688
		private static SystemResourceKey _cacheIconFontStyle;

		// Token: 0x04001251 RID: 4689
		private static SystemResourceKey _cacheIconFontWeight;

		// Token: 0x04001252 RID: 4690
		private static SystemResourceKey _cacheIconFontTextDecorations;

		// Token: 0x04001253 RID: 4691
		private static SystemResourceKey _cacheCaptionFontSize;

		// Token: 0x04001254 RID: 4692
		private static SystemResourceKey _cacheCaptionFontFamily;

		// Token: 0x04001255 RID: 4693
		private static SystemResourceKey _cacheCaptionFontStyle;

		// Token: 0x04001256 RID: 4694
		private static SystemResourceKey _cacheCaptionFontWeight;

		// Token: 0x04001257 RID: 4695
		private static SystemResourceKey _cacheCaptionFontTextDecorations;

		// Token: 0x04001258 RID: 4696
		private static SystemResourceKey _cacheSmallCaptionFontSize;

		// Token: 0x04001259 RID: 4697
		private static SystemResourceKey _cacheSmallCaptionFontFamily;

		// Token: 0x0400125A RID: 4698
		private static SystemResourceKey _cacheSmallCaptionFontStyle;

		// Token: 0x0400125B RID: 4699
		private static SystemResourceKey _cacheSmallCaptionFontWeight;

		// Token: 0x0400125C RID: 4700
		private static SystemResourceKey _cacheSmallCaptionFontTextDecorations;

		// Token: 0x0400125D RID: 4701
		private static SystemResourceKey _cacheMenuFontSize;

		// Token: 0x0400125E RID: 4702
		private static SystemResourceKey _cacheMenuFontFamily;

		// Token: 0x0400125F RID: 4703
		private static SystemResourceKey _cacheMenuFontStyle;

		// Token: 0x04001260 RID: 4704
		private static SystemResourceKey _cacheMenuFontWeight;

		// Token: 0x04001261 RID: 4705
		private static SystemResourceKey _cacheMenuFontTextDecorations;

		// Token: 0x04001262 RID: 4706
		private static SystemResourceKey _cacheStatusFontSize;

		// Token: 0x04001263 RID: 4707
		private static SystemResourceKey _cacheStatusFontFamily;

		// Token: 0x04001264 RID: 4708
		private static SystemResourceKey _cacheStatusFontStyle;

		// Token: 0x04001265 RID: 4709
		private static SystemResourceKey _cacheStatusFontWeight;

		// Token: 0x04001266 RID: 4710
		private static SystemResourceKey _cacheStatusFontTextDecorations;

		// Token: 0x04001267 RID: 4711
		private static SystemResourceKey _cacheMessageFontSize;

		// Token: 0x04001268 RID: 4712
		private static SystemResourceKey _cacheMessageFontFamily;

		// Token: 0x04001269 RID: 4713
		private static SystemResourceKey _cacheMessageFontStyle;

		// Token: 0x0400126A RID: 4714
		private static SystemResourceKey _cacheMessageFontWeight;

		// Token: 0x0400126B RID: 4715
		private static SystemResourceKey _cacheMessageFontTextDecorations;
	}
}
