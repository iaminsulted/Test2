using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.FontCache;
using MS.Internal.Interop;
using MS.Utility;
using MS.Win32;
using Standard;

namespace System.Windows
{
	// Token: 0x020003B7 RID: 951
	public static class SystemParameters
	{
		// Token: 0x14000064 RID: 100
		// (add) Token: 0x060026FD RID: 9981 RVA: 0x0018CAC0 File Offset: 0x0018BAC0
		// (remove) Token: 0x060026FE RID: 9982 RVA: 0x0018CAF4 File Offset: 0x0018BAF4
		public static event PropertyChangedEventHandler StaticPropertyChanged;

		// Token: 0x060026FF RID: 9983 RVA: 0x0018CB28 File Offset: 0x0018BB28
		private static void OnPropertiesChanged(params string[] propertyNames)
		{
			PropertyChangedEventHandler staticPropertyChanged = SystemParameters.StaticPropertyChanged;
			if (staticPropertyChanged != null)
			{
				for (int i = 0; i < propertyNames.Length; i++)
				{
					staticPropertyChanged(null, new PropertyChangedEventArgs(propertyNames[i]));
				}
			}
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x0018CB5B File Offset: 0x0018BB5B
		private static bool InvalidateProperty(int slot, string name)
		{
			if (!SystemResources.ClearSlot(SystemParameters._cacheValid, slot))
			{
				return false;
			}
			SystemParameters.OnPropertiesChanged(new string[]
			{
				name
			});
			return true;
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002701 RID: 9985 RVA: 0x0018CB7C File Offset: 0x0018BB7C
		public static double FocusBorderWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[1])
					{
						SystemParameters._cacheValid[1] = true;
						int pixel = 0;
						if (!UnsafeNativeMethods.SystemParametersInfo(8206, 0, ref pixel, 0))
						{
							SystemParameters._cacheValid[1] = false;
							throw new Win32Exception();
						}
						SystemParameters._focusBorderWidth = SystemParameters.ConvertPixel(pixel);
					}
				}
				return SystemParameters._focusBorderWidth;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002702 RID: 9986 RVA: 0x0018CC08 File Offset: 0x0018BC08
		public static double FocusBorderHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[2])
					{
						SystemParameters._cacheValid[2] = true;
						int pixel = 0;
						if (!UnsafeNativeMethods.SystemParametersInfo(8208, 0, ref pixel, 0))
						{
							SystemParameters._cacheValid[2] = false;
							throw new Win32Exception();
						}
						SystemParameters._focusBorderHeight = SystemParameters.ConvertPixel(pixel);
					}
				}
				return SystemParameters._focusBorderHeight;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06002703 RID: 9987 RVA: 0x0018CC94 File Offset: 0x0018BC94
		public static bool HighContrast
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[3])
					{
						SystemParameters._cacheValid[3] = true;
						NativeMethods.HIGHCONTRAST_I highcontrast_I = default(NativeMethods.HIGHCONTRAST_I);
						highcontrast_I.cbSize = Marshal.SizeOf(typeof(NativeMethods.HIGHCONTRAST_I));
						if (!UnsafeNativeMethods.SystemParametersInfo(66, highcontrast_I.cbSize, ref highcontrast_I, 0))
						{
							SystemParameters._cacheValid[3] = false;
							throw new Win32Exception();
						}
						SystemParameters._highContrast = ((highcontrast_I.dwFlags & 1) == 1);
					}
				}
				return SystemParameters._highContrast;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002704 RID: 9988 RVA: 0x0018CD44 File Offset: 0x0018BD44
		internal static bool MouseVanish
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[4])
					{
						SystemParameters._cacheValid[4] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4128, 0, ref SystemParameters._mouseVanish, 0))
						{
							SystemParameters._cacheValid[4] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._mouseVanish;
			}
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x0018B567 File Offset: 0x0018A567
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
		{
			return new SystemResourceKey(KeyId);
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06002706 RID: 9990 RVA: 0x0018CDC4 File Offset: 0x0018BDC4
		public static ResourceKey FocusBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheFocusBorderWidth == null)
				{
					SystemParameters._cacheFocusBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.FocusBorderWidth);
				}
				return SystemParameters._cacheFocusBorderWidth;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06002707 RID: 9991 RVA: 0x0018CDE1 File Offset: 0x0018BDE1
		public static ResourceKey FocusBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheFocusBorderHeight == null)
				{
					SystemParameters._cacheFocusBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.FocusBorderHeight);
				}
				return SystemParameters._cacheFocusBorderHeight;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002708 RID: 9992 RVA: 0x0018CDFE File Offset: 0x0018BDFE
		public static ResourceKey HighContrastKey
		{
			get
			{
				if (SystemParameters._cacheHighContrast == null)
				{
					SystemParameters._cacheHighContrast = SystemParameters.CreateInstance(SystemResourceKeyID.HighContrast);
				}
				return SystemParameters._cacheHighContrast;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06002709 RID: 9993 RVA: 0x0018CE1C File Offset: 0x0018BE1C
		public static bool DropShadow
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[5])
					{
						SystemParameters._cacheValid[5] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4132, 0, ref SystemParameters._dropShadow, 0))
						{
							SystemParameters._cacheValid[5] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._dropShadow;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x0600270A RID: 9994 RVA: 0x0018CE9C File Offset: 0x0018BE9C
		public static bool FlatMenu
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[6])
					{
						SystemParameters._cacheValid[6] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4130, 0, ref SystemParameters._flatMenu, 0))
						{
							SystemParameters._cacheValid[6] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._flatMenu;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x0018CF1C File Offset: 0x0018BF1C
		internal static NativeMethods.RECT WorkAreaInternal
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[7])
					{
						SystemParameters._cacheValid[7] = true;
						SystemParameters._workAreaInternal = default(NativeMethods.RECT);
						if (!UnsafeNativeMethods.SystemParametersInfo(48, 0, ref SystemParameters._workAreaInternal, 0))
						{
							SystemParameters._cacheValid[7] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._workAreaInternal;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x0600270C RID: 9996 RVA: 0x0018CFA4 File Offset: 0x0018BFA4
		public static Rect WorkArea
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[8])
					{
						SystemParameters._cacheValid[8] = true;
						NativeMethods.RECT workAreaInternal = SystemParameters.WorkAreaInternal;
						SystemParameters._workArea = new Rect(SystemParameters.ConvertPixel(workAreaInternal.left), SystemParameters.ConvertPixel(workAreaInternal.top), SystemParameters.ConvertPixel(workAreaInternal.Width), SystemParameters.ConvertPixel(workAreaInternal.Height));
					}
				}
				return SystemParameters._workArea;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x0600270D RID: 9997 RVA: 0x0018D03C File Offset: 0x0018C03C
		public static ResourceKey DropShadowKey
		{
			get
			{
				if (SystemParameters._cacheDropShadow == null)
				{
					SystemParameters._cacheDropShadow = SystemParameters.CreateInstance(SystemResourceKeyID.DropShadow);
				}
				return SystemParameters._cacheDropShadow;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x0600270E RID: 9998 RVA: 0x0018D059 File Offset: 0x0018C059
		public static ResourceKey FlatMenuKey
		{
			get
			{
				if (SystemParameters._cacheFlatMenu == null)
				{
					SystemParameters._cacheFlatMenu = SystemParameters.CreateInstance(SystemResourceKeyID.FlatMenu);
				}
				return SystemParameters._cacheFlatMenu;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x0600270F RID: 9999 RVA: 0x0018D076 File Offset: 0x0018C076
		public static ResourceKey WorkAreaKey
		{
			get
			{
				if (SystemParameters._cacheWorkArea == null)
				{
					SystemParameters._cacheWorkArea = SystemParameters.CreateInstance(SystemResourceKeyID.WorkArea);
				}
				return SystemParameters._cacheWorkArea;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002710 RID: 10000 RVA: 0x0018D094 File Offset: 0x0018C094
		internal static NativeMethods.ICONMETRICS IconMetrics
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[9])
					{
						SystemParameters._cacheValid[9] = true;
						SystemParameters._iconMetrics = new NativeMethods.ICONMETRICS();
						if (!UnsafeNativeMethods.SystemParametersInfo(45, SystemParameters._iconMetrics.cbSize, SystemParameters._iconMetrics, 0))
						{
							SystemParameters._cacheValid[9] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._iconMetrics;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002711 RID: 10001 RVA: 0x0018D128 File Offset: 0x0018C128
		public static double IconHorizontalSpacing
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.IconMetrics.iHorzSpacing);
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002712 RID: 10002 RVA: 0x0018D139 File Offset: 0x0018C139
		public static double IconVerticalSpacing
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.IconMetrics.iVertSpacing);
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002713 RID: 10003 RVA: 0x0018D14A File Offset: 0x0018C14A
		public static bool IconTitleWrap
		{
			get
			{
				return SystemParameters.IconMetrics.iTitleWrap != 0;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06002714 RID: 10004 RVA: 0x0018D159 File Offset: 0x0018C159
		public static ResourceKey IconHorizontalSpacingKey
		{
			get
			{
				if (SystemParameters._cacheIconHorizontalSpacing == null)
				{
					SystemParameters._cacheIconHorizontalSpacing = SystemParameters.CreateInstance(SystemResourceKeyID.IconHorizontalSpacing);
				}
				return SystemParameters._cacheIconHorizontalSpacing;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002715 RID: 10005 RVA: 0x0018D176 File Offset: 0x0018C176
		public static ResourceKey IconVerticalSpacingKey
		{
			get
			{
				if (SystemParameters._cacheIconVerticalSpacing == null)
				{
					SystemParameters._cacheIconVerticalSpacing = SystemParameters.CreateInstance(SystemResourceKeyID.IconVerticalSpacing);
				}
				return SystemParameters._cacheIconVerticalSpacing;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002716 RID: 10006 RVA: 0x0018D193 File Offset: 0x0018C193
		public static ResourceKey IconTitleWrapKey
		{
			get
			{
				if (SystemParameters._cacheIconTitleWrap == null)
				{
					SystemParameters._cacheIconTitleWrap = SystemParameters.CreateInstance(SystemResourceKeyID.IconTitleWrap);
				}
				return SystemParameters._cacheIconTitleWrap;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002717 RID: 10007 RVA: 0x0018D1B0 File Offset: 0x0018C1B0
		public static bool KeyboardCues
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[10])
					{
						SystemParameters._cacheValid[10] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4106, 0, ref SystemParameters._keyboardCues, 0))
						{
							SystemParameters._cacheValid[10] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._keyboardCues;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002718 RID: 10008 RVA: 0x0018D234 File Offset: 0x0018C234
		public static int KeyboardDelay
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[11])
					{
						SystemParameters._cacheValid[11] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(22, 0, ref SystemParameters._keyboardDelay, 0))
						{
							SystemParameters._cacheValid[11] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._keyboardDelay;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002719 RID: 10009 RVA: 0x0018D2B4 File Offset: 0x0018C2B4
		public static bool KeyboardPreference
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[12])
					{
						SystemParameters._cacheValid[12] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(68, 0, ref SystemParameters._keyboardPref, 0))
						{
							SystemParameters._cacheValid[12] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._keyboardPref;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600271A RID: 10010 RVA: 0x0018D334 File Offset: 0x0018C334
		public static int KeyboardSpeed
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[13])
					{
						SystemParameters._cacheValid[13] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(10, 0, ref SystemParameters._keyboardSpeed, 0))
						{
							SystemParameters._cacheValid[13] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._keyboardSpeed;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600271B RID: 10011 RVA: 0x0018D3B4 File Offset: 0x0018C3B4
		public static bool SnapToDefaultButton
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[14])
					{
						SystemParameters._cacheValid[14] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(95, 0, ref SystemParameters._snapToDefButton, 0))
						{
							SystemParameters._cacheValid[14] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._snapToDefButton;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x0018D434 File Offset: 0x0018C434
		public static int WheelScrollLines
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[15])
					{
						SystemParameters._cacheValid[15] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(104, 0, ref SystemParameters._wheelScrollLines, 0))
						{
							SystemParameters._cacheValid[15] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._wheelScrollLines;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x0600271D RID: 10013 RVA: 0x0018D4B4 File Offset: 0x0018C4B4
		public static TimeSpan MouseHoverTime
		{
			get
			{
				return TimeSpan.FromMilliseconds((double)SystemParameters.MouseHoverTimeMilliseconds);
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x0600271E RID: 10014 RVA: 0x0018D4C4 File Offset: 0x0018C4C4
		internal static int MouseHoverTimeMilliseconds
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[16])
					{
						SystemParameters._cacheValid[16] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(102, 0, ref SystemParameters._mouseHoverTime, 0))
						{
							SystemParameters._cacheValid[16] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._mouseHoverTime;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x0600271F RID: 10015 RVA: 0x0018D544 File Offset: 0x0018C544
		public static double MouseHoverHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[17])
					{
						SystemParameters._cacheValid[17] = true;
						int pixel = 0;
						if (!UnsafeNativeMethods.SystemParametersInfo(100, 0, ref pixel, 0))
						{
							SystemParameters._cacheValid[17] = false;
							throw new Win32Exception();
						}
						SystemParameters._mouseHoverHeight = SystemParameters.ConvertPixel(pixel);
					}
				}
				return SystemParameters._mouseHoverHeight;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002720 RID: 10016 RVA: 0x0018D5D0 File Offset: 0x0018C5D0
		public static double MouseHoverWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[18])
					{
						SystemParameters._cacheValid[18] = true;
						int pixel = 0;
						if (!UnsafeNativeMethods.SystemParametersInfo(98, 0, ref pixel, 0))
						{
							SystemParameters._cacheValid[18] = false;
							throw new Win32Exception();
						}
						SystemParameters._mouseHoverWidth = SystemParameters.ConvertPixel(pixel);
					}
				}
				return SystemParameters._mouseHoverWidth;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002721 RID: 10017 RVA: 0x0018D65C File Offset: 0x0018C65C
		public static ResourceKey KeyboardCuesKey
		{
			get
			{
				if (SystemParameters._cacheKeyboardCues == null)
				{
					SystemParameters._cacheKeyboardCues = SystemParameters.CreateInstance(SystemResourceKeyID.KeyboardCues);
				}
				return SystemParameters._cacheKeyboardCues;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x0018D679 File Offset: 0x0018C679
		public static ResourceKey KeyboardDelayKey
		{
			get
			{
				if (SystemParameters._cacheKeyboardDelay == null)
				{
					SystemParameters._cacheKeyboardDelay = SystemParameters.CreateInstance(SystemResourceKeyID.KeyboardDelay);
				}
				return SystemParameters._cacheKeyboardDelay;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002723 RID: 10019 RVA: 0x0018D696 File Offset: 0x0018C696
		public static ResourceKey KeyboardPreferenceKey
		{
			get
			{
				if (SystemParameters._cacheKeyboardPreference == null)
				{
					SystemParameters._cacheKeyboardPreference = SystemParameters.CreateInstance(SystemResourceKeyID.KeyboardPreference);
				}
				return SystemParameters._cacheKeyboardPreference;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002724 RID: 10020 RVA: 0x0018D6B3 File Offset: 0x0018C6B3
		public static ResourceKey KeyboardSpeedKey
		{
			get
			{
				if (SystemParameters._cacheKeyboardSpeed == null)
				{
					SystemParameters._cacheKeyboardSpeed = SystemParameters.CreateInstance(SystemResourceKeyID.KeyboardSpeed);
				}
				return SystemParameters._cacheKeyboardSpeed;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002725 RID: 10021 RVA: 0x0018D6D0 File Offset: 0x0018C6D0
		public static ResourceKey SnapToDefaultButtonKey
		{
			get
			{
				if (SystemParameters._cacheSnapToDefaultButton == null)
				{
					SystemParameters._cacheSnapToDefaultButton = SystemParameters.CreateInstance(SystemResourceKeyID.SnapToDefaultButton);
				}
				return SystemParameters._cacheSnapToDefaultButton;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x0018D6ED File Offset: 0x0018C6ED
		public static ResourceKey WheelScrollLinesKey
		{
			get
			{
				if (SystemParameters._cacheWheelScrollLines == null)
				{
					SystemParameters._cacheWheelScrollLines = SystemParameters.CreateInstance(SystemResourceKeyID.WheelScrollLines);
				}
				return SystemParameters._cacheWheelScrollLines;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002727 RID: 10023 RVA: 0x0018D70A File Offset: 0x0018C70A
		public static ResourceKey MouseHoverTimeKey
		{
			get
			{
				if (SystemParameters._cacheMouseHoverTime == null)
				{
					SystemParameters._cacheMouseHoverTime = SystemParameters.CreateInstance(SystemResourceKeyID.MouseHoverTime);
				}
				return SystemParameters._cacheMouseHoverTime;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002728 RID: 10024 RVA: 0x0018D727 File Offset: 0x0018C727
		public static ResourceKey MouseHoverHeightKey
		{
			get
			{
				if (SystemParameters._cacheMouseHoverHeight == null)
				{
					SystemParameters._cacheMouseHoverHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MouseHoverHeight);
				}
				return SystemParameters._cacheMouseHoverHeight;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002729 RID: 10025 RVA: 0x0018D744 File Offset: 0x0018C744
		public static ResourceKey MouseHoverWidthKey
		{
			get
			{
				if (SystemParameters._cacheMouseHoverWidth == null)
				{
					SystemParameters._cacheMouseHoverWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MouseHoverWidth);
				}
				return SystemParameters._cacheMouseHoverWidth;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x0018D764 File Offset: 0x0018C764
		public static bool MenuDropAlignment
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[19])
					{
						SystemParameters._cacheValid[19] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(27, 0, ref SystemParameters._menuDropAlignment, 0))
						{
							SystemParameters._cacheValid[19] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._menuDropAlignment;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x0600272B RID: 10027 RVA: 0x0018D7E4 File Offset: 0x0018C7E4
		public static bool MenuFade
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[20])
					{
						SystemParameters._cacheValid[20] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4114, 0, ref SystemParameters._menuFade, 0))
						{
							SystemParameters._cacheValid[20] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._menuFade;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x0018D868 File Offset: 0x0018C868
		public static int MenuShowDelay
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[21])
					{
						SystemParameters._cacheValid[21] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(106, 0, ref SystemParameters._menuShowDelay, 0))
						{
							SystemParameters._cacheValid[21] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._menuShowDelay;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x0600272D RID: 10029 RVA: 0x0018D8E8 File Offset: 0x0018C8E8
		public static ResourceKey MenuDropAlignmentKey
		{
			get
			{
				if (SystemParameters._cacheMenuDropAlignment == null)
				{
					SystemParameters._cacheMenuDropAlignment = SystemParameters.CreateInstance(SystemResourceKeyID.MenuDropAlignment);
				}
				return SystemParameters._cacheMenuDropAlignment;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x0018D905 File Offset: 0x0018C905
		public static ResourceKey MenuFadeKey
		{
			get
			{
				if (SystemParameters._cacheMenuFade == null)
				{
					SystemParameters._cacheMenuFade = SystemParameters.CreateInstance(SystemResourceKeyID.MenuFade);
				}
				return SystemParameters._cacheMenuFade;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x0600272F RID: 10031 RVA: 0x0018D922 File Offset: 0x0018C922
		public static ResourceKey MenuShowDelayKey
		{
			get
			{
				if (SystemParameters._cacheMenuShowDelay == null)
				{
					SystemParameters._cacheMenuShowDelay = SystemParameters.CreateInstance(SystemResourceKeyID.MenuShowDelay);
				}
				return SystemParameters._cacheMenuShowDelay;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x0018D93F File Offset: 0x0018C93F
		public static PopupAnimation ComboBoxPopupAnimation
		{
			get
			{
				if (SystemParameters.ComboBoxAnimation)
				{
					return PopupAnimation.Slide;
				}
				return PopupAnimation.None;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002731 RID: 10033 RVA: 0x0018D94C File Offset: 0x0018C94C
		public static bool ComboBoxAnimation
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[22])
					{
						SystemParameters._cacheValid[22] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4100, 0, ref SystemParameters._comboBoxAnimation, 0))
						{
							SystemParameters._cacheValid[22] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._comboBoxAnimation;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x0018D9D0 File Offset: 0x0018C9D0
		public static bool ClientAreaAnimation
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[23])
					{
						SystemParameters._cacheValid[23] = true;
						if (Environment.OSVersion.Version.Major >= 6)
						{
							if (!UnsafeNativeMethods.SystemParametersInfo(4162, 0, ref SystemParameters._clientAreaAnimation, 0))
							{
								SystemParameters._cacheValid[23] = false;
								throw new Win32Exception();
							}
						}
						else
						{
							SystemParameters._clientAreaAnimation = true;
						}
					}
				}
				return SystemParameters._clientAreaAnimation;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002733 RID: 10035 RVA: 0x0018DA6C File Offset: 0x0018CA6C
		public static bool CursorShadow
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[24])
					{
						SystemParameters._cacheValid[24] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4122, 0, ref SystemParameters._cursorShadow, 0))
						{
							SystemParameters._cacheValid[24] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._cursorShadow;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x0018DAF0 File Offset: 0x0018CAF0
		public static bool GradientCaptions
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[25])
					{
						SystemParameters._cacheValid[25] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4104, 0, ref SystemParameters._gradientCaptions, 0))
						{
							SystemParameters._cacheValid[25] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._gradientCaptions;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x0018DB74 File Offset: 0x0018CB74
		public static bool HotTracking
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[26])
					{
						SystemParameters._cacheValid[26] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4110, 0, ref SystemParameters._hotTracking, 0))
						{
							SystemParameters._cacheValid[26] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._hotTracking;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x0018DBF8 File Offset: 0x0018CBF8
		public static bool ListBoxSmoothScrolling
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[27])
					{
						SystemParameters._cacheValid[27] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4102, 0, ref SystemParameters._listBoxSmoothScrolling, 0))
						{
							SystemParameters._cacheValid[27] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._listBoxSmoothScrolling;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x0018DC7C File Offset: 0x0018CC7C
		public static PopupAnimation MenuPopupAnimation
		{
			get
			{
				if (!SystemParameters.MenuAnimation)
				{
					return PopupAnimation.None;
				}
				if (SystemParameters.MenuFade)
				{
					return PopupAnimation.Fade;
				}
				return PopupAnimation.Scroll;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x0018DC94 File Offset: 0x0018CC94
		public static bool MenuAnimation
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[28])
					{
						SystemParameters._cacheValid[28] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4098, 0, ref SystemParameters._menuAnimation, 0))
						{
							SystemParameters._cacheValid[28] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._menuAnimation;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002739 RID: 10041 RVA: 0x0018DD18 File Offset: 0x0018CD18
		public static bool SelectionFade
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[29])
					{
						SystemParameters._cacheValid[29] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4116, 0, ref SystemParameters._selectionFade, 0))
						{
							SystemParameters._cacheValid[29] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._selectionFade;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x0018DD9C File Offset: 0x0018CD9C
		public static bool StylusHotTracking
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[30])
					{
						SystemParameters._cacheValid[30] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4112, 0, ref SystemParameters._stylusHotTracking, 0))
						{
							SystemParameters._cacheValid[30] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._stylusHotTracking;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x0600273B RID: 10043 RVA: 0x0018DE20 File Offset: 0x0018CE20
		public static PopupAnimation ToolTipPopupAnimation
		{
			get
			{
				if (SystemParameters.ToolTipAnimation && SystemParameters.ToolTipFade)
				{
					return PopupAnimation.Fade;
				}
				return PopupAnimation.None;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x0018DE34 File Offset: 0x0018CE34
		public static bool ToolTipAnimation
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[31])
					{
						SystemParameters._cacheValid[31] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4118, 0, ref SystemParameters._toolTipAnimation, 0))
						{
							SystemParameters._cacheValid[31] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._toolTipAnimation;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x0600273D RID: 10045 RVA: 0x0018DEB8 File Offset: 0x0018CEB8
		public static bool ToolTipFade
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[32])
					{
						SystemParameters._cacheValid[32] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4120, 0, ref SystemParameters._tooltipFade, 0))
						{
							SystemParameters._cacheValid[32] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._tooltipFade;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x0600273E RID: 10046 RVA: 0x0018DF3C File Offset: 0x0018CF3C
		public static bool UIEffects
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[33])
					{
						SystemParameters._cacheValid[33] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4158, 0, ref SystemParameters._uiEffects, 0))
						{
							SystemParameters._cacheValid[33] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._uiEffects;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x0600273F RID: 10047 RVA: 0x0018DFC0 File Offset: 0x0018CFC0
		public static ResourceKey ComboBoxAnimationKey
		{
			get
			{
				if (SystemParameters._cacheComboBoxAnimation == null)
				{
					SystemParameters._cacheComboBoxAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ComboBoxAnimation);
				}
				return SystemParameters._cacheComboBoxAnimation;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002740 RID: 10048 RVA: 0x0018DFDD File Offset: 0x0018CFDD
		public static ResourceKey ClientAreaAnimationKey
		{
			get
			{
				if (SystemParameters._cacheClientAreaAnimation == null)
				{
					SystemParameters._cacheClientAreaAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ClientAreaAnimation);
				}
				return SystemParameters._cacheClientAreaAnimation;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002741 RID: 10049 RVA: 0x0018DFFA File Offset: 0x0018CFFA
		public static ResourceKey CursorShadowKey
		{
			get
			{
				if (SystemParameters._cacheCursorShadow == null)
				{
					SystemParameters._cacheCursorShadow = SystemParameters.CreateInstance(SystemResourceKeyID.CursorShadow);
				}
				return SystemParameters._cacheCursorShadow;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002742 RID: 10050 RVA: 0x0018E017 File Offset: 0x0018D017
		public static ResourceKey GradientCaptionsKey
		{
			get
			{
				if (SystemParameters._cacheGradientCaptions == null)
				{
					SystemParameters._cacheGradientCaptions = SystemParameters.CreateInstance(SystemResourceKeyID.GradientCaptions);
				}
				return SystemParameters._cacheGradientCaptions;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002743 RID: 10051 RVA: 0x0018E034 File Offset: 0x0018D034
		public static ResourceKey HotTrackingKey
		{
			get
			{
				if (SystemParameters._cacheHotTracking == null)
				{
					SystemParameters._cacheHotTracking = SystemParameters.CreateInstance(SystemResourceKeyID.HotTracking);
				}
				return SystemParameters._cacheHotTracking;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002744 RID: 10052 RVA: 0x0018E051 File Offset: 0x0018D051
		public static ResourceKey ListBoxSmoothScrollingKey
		{
			get
			{
				if (SystemParameters._cacheListBoxSmoothScrolling == null)
				{
					SystemParameters._cacheListBoxSmoothScrolling = SystemParameters.CreateInstance(SystemResourceKeyID.ListBoxSmoothScrolling);
				}
				return SystemParameters._cacheListBoxSmoothScrolling;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002745 RID: 10053 RVA: 0x0018E06E File Offset: 0x0018D06E
		public static ResourceKey MenuAnimationKey
		{
			get
			{
				if (SystemParameters._cacheMenuAnimation == null)
				{
					SystemParameters._cacheMenuAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.MenuAnimation);
				}
				return SystemParameters._cacheMenuAnimation;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002746 RID: 10054 RVA: 0x0018E08B File Offset: 0x0018D08B
		public static ResourceKey SelectionFadeKey
		{
			get
			{
				if (SystemParameters._cacheSelectionFade == null)
				{
					SystemParameters._cacheSelectionFade = SystemParameters.CreateInstance(SystemResourceKeyID.SelectionFade);
				}
				return SystemParameters._cacheSelectionFade;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002747 RID: 10055 RVA: 0x0018E0A8 File Offset: 0x0018D0A8
		public static ResourceKey StylusHotTrackingKey
		{
			get
			{
				if (SystemParameters._cacheStylusHotTracking == null)
				{
					SystemParameters._cacheStylusHotTracking = SystemParameters.CreateInstance(SystemResourceKeyID.StylusHotTracking);
				}
				return SystemParameters._cacheStylusHotTracking;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x0018E0C5 File Offset: 0x0018D0C5
		public static ResourceKey ToolTipAnimationKey
		{
			get
			{
				if (SystemParameters._cacheToolTipAnimation == null)
				{
					SystemParameters._cacheToolTipAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ToolTipAnimation);
				}
				return SystemParameters._cacheToolTipAnimation;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x0018E0E2 File Offset: 0x0018D0E2
		public static ResourceKey ToolTipFadeKey
		{
			get
			{
				if (SystemParameters._cacheToolTipFade == null)
				{
					SystemParameters._cacheToolTipFade = SystemParameters.CreateInstance(SystemResourceKeyID.ToolTipFade);
				}
				return SystemParameters._cacheToolTipFade;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x0600274A RID: 10058 RVA: 0x0018E0FF File Offset: 0x0018D0FF
		public static ResourceKey UIEffectsKey
		{
			get
			{
				if (SystemParameters._cacheUIEffects == null)
				{
					SystemParameters._cacheUIEffects = SystemParameters.CreateInstance(SystemResourceKeyID.UIEffects);
				}
				return SystemParameters._cacheUIEffects;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x0018E11C File Offset: 0x0018D11C
		public static ResourceKey ComboBoxPopupAnimationKey
		{
			get
			{
				if (SystemParameters._cacheComboBoxPopupAnimation == null)
				{
					SystemParameters._cacheComboBoxPopupAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ComboBoxPopupAnimation);
				}
				return SystemParameters._cacheComboBoxPopupAnimation;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x0600274C RID: 10060 RVA: 0x0018E139 File Offset: 0x0018D139
		public static ResourceKey MenuPopupAnimationKey
		{
			get
			{
				if (SystemParameters._cacheMenuPopupAnimation == null)
				{
					SystemParameters._cacheMenuPopupAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.MenuPopupAnimation);
				}
				return SystemParameters._cacheMenuPopupAnimation;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x0600274D RID: 10061 RVA: 0x0018E156 File Offset: 0x0018D156
		public static ResourceKey ToolTipPopupAnimationKey
		{
			get
			{
				if (SystemParameters._cacheToolTipPopupAnimation == null)
				{
					SystemParameters._cacheToolTipPopupAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ToolTipPopupAnimation);
				}
				return SystemParameters._cacheToolTipPopupAnimation;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x0600274E RID: 10062 RVA: 0x0018E174 File Offset: 0x0018D174
		public static bool MinimizeAnimation
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[34])
					{
						SystemParameters._cacheValid[34] = true;
						NativeMethods.ANIMATIONINFO animationinfo = new NativeMethods.ANIMATIONINFO();
						if (!UnsafeNativeMethods.SystemParametersInfo(72, animationinfo.cbSize, animationinfo, 0))
						{
							SystemParameters._cacheValid[34] = false;
							throw new Win32Exception();
						}
						SystemParameters._minAnimation = (animationinfo.iMinAnimate != 0);
					}
				}
				return SystemParameters._minAnimation;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x0600274F RID: 10063 RVA: 0x0018E20C File Offset: 0x0018D20C
		public static int Border
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[35])
					{
						SystemParameters._cacheValid[35] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(5, 0, ref SystemParameters._border, 0))
						{
							SystemParameters._cacheValid[35] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._border;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002750 RID: 10064 RVA: 0x0018E28C File Offset: 0x0018D28C
		public static double CaretWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[36])
					{
						SystemParameters._cacheValid[36] = true;
						int num = 0;
						using (DpiUtil.WithDpiAwarenessContext(DpiAwarenessContextValue.Unaware))
						{
							if (!UnsafeNativeMethods.SystemParametersInfo(8198, 0, ref num, 0))
							{
								SystemParameters._cacheValid[36] = false;
								throw new Win32Exception();
							}
							SystemParameters._caretWidth = (double)num;
						}
					}
				}
				return SystemParameters._caretWidth;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x0018E334 File Offset: 0x0018D334
		public static bool DragFullWindows
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[38])
					{
						SystemParameters._cacheValid[38] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(38, 0, ref SystemParameters._dragFullWindows, 0))
						{
							SystemParameters._cacheValid[38] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._dragFullWindows;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002752 RID: 10066 RVA: 0x0018E3B4 File Offset: 0x0018D3B4
		public static int ForegroundFlashCount
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[37])
					{
						SystemParameters._cacheValid[37] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(8196, 0, ref SystemParameters._foregroundFlashCount, 0))
						{
							SystemParameters._cacheValid[37] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._foregroundFlashCount;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002753 RID: 10067 RVA: 0x0018E438 File Offset: 0x0018D438
		internal static NativeMethods.NONCLIENTMETRICS NonClientMetrics
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[39])
					{
						SystemParameters._cacheValid[39] = true;
						SystemParameters._ncm = new NativeMethods.NONCLIENTMETRICS();
						if (!UnsafeNativeMethods.SystemParametersInfo(41, SystemParameters._ncm.cbSize, SystemParameters._ncm, 0))
						{
							SystemParameters._cacheValid[39] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._ncm;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002754 RID: 10068 RVA: 0x0018E4CC File Offset: 0x0018D4CC
		public static double BorderWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iBorderWidth);
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x0018E4DD File Offset: 0x0018D4DD
		public static double ScrollWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iScrollWidth);
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002756 RID: 10070 RVA: 0x0018E4EE File Offset: 0x0018D4EE
		public static double ScrollHeight
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iScrollHeight);
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002757 RID: 10071 RVA: 0x0018E4FF File Offset: 0x0018D4FF
		public static double CaptionWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iCaptionWidth);
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002758 RID: 10072 RVA: 0x0018E510 File Offset: 0x0018D510
		public static double CaptionHeight
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iCaptionHeight);
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002759 RID: 10073 RVA: 0x0018E521 File Offset: 0x0018D521
		public static double SmallCaptionWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iSmCaptionWidth);
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x0600275A RID: 10074 RVA: 0x0018E532 File Offset: 0x0018D532
		public static double SmallCaptionHeight
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iSmCaptionHeight);
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x0600275B RID: 10075 RVA: 0x0018E543 File Offset: 0x0018D543
		public static double MenuWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iMenuWidth);
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x0018E554 File Offset: 0x0018D554
		public static double MenuHeight
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iMenuHeight);
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x0600275D RID: 10077 RVA: 0x0018E565 File Offset: 0x0018D565
		public static ResourceKey MinimizeAnimationKey
		{
			get
			{
				if (SystemParameters._cacheMinimizeAnimation == null)
				{
					SystemParameters._cacheMinimizeAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizeAnimation);
				}
				return SystemParameters._cacheMinimizeAnimation;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x0018E582 File Offset: 0x0018D582
		public static ResourceKey BorderKey
		{
			get
			{
				if (SystemParameters._cacheBorder == null)
				{
					SystemParameters._cacheBorder = SystemParameters.CreateInstance(SystemResourceKeyID.Border);
				}
				return SystemParameters._cacheBorder;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x0018E59F File Offset: 0x0018D59F
		public static ResourceKey CaretWidthKey
		{
			get
			{
				if (SystemParameters._cacheCaretWidth == null)
				{
					SystemParameters._cacheCaretWidth = SystemParameters.CreateInstance(SystemResourceKeyID.CaretWidth);
				}
				return SystemParameters._cacheCaretWidth;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002760 RID: 10080 RVA: 0x0018E5BC File Offset: 0x0018D5BC
		public static ResourceKey ForegroundFlashCountKey
		{
			get
			{
				if (SystemParameters._cacheForegroundFlashCount == null)
				{
					SystemParameters._cacheForegroundFlashCount = SystemParameters.CreateInstance(SystemResourceKeyID.ForegroundFlashCount);
				}
				return SystemParameters._cacheForegroundFlashCount;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x0018E5D9 File Offset: 0x0018D5D9
		public static ResourceKey DragFullWindowsKey
		{
			get
			{
				if (SystemParameters._cacheDragFullWindows == null)
				{
					SystemParameters._cacheDragFullWindows = SystemParameters.CreateInstance(SystemResourceKeyID.DragFullWindows);
				}
				return SystemParameters._cacheDragFullWindows;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002762 RID: 10082 RVA: 0x0018E5F6 File Offset: 0x0018D5F6
		public static ResourceKey BorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheBorderWidth == null)
				{
					SystemParameters._cacheBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.BorderWidth);
				}
				return SystemParameters._cacheBorderWidth;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x0018E613 File Offset: 0x0018D613
		public static ResourceKey ScrollWidthKey
		{
			get
			{
				if (SystemParameters._cacheScrollWidth == null)
				{
					SystemParameters._cacheScrollWidth = SystemParameters.CreateInstance(SystemResourceKeyID.ScrollWidth);
				}
				return SystemParameters._cacheScrollWidth;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002764 RID: 10084 RVA: 0x0018E630 File Offset: 0x0018D630
		public static ResourceKey ScrollHeightKey
		{
			get
			{
				if (SystemParameters._cacheScrollHeight == null)
				{
					SystemParameters._cacheScrollHeight = SystemParameters.CreateInstance(SystemResourceKeyID.ScrollHeight);
				}
				return SystemParameters._cacheScrollHeight;
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x0018E64D File Offset: 0x0018D64D
		public static ResourceKey CaptionWidthKey
		{
			get
			{
				if (SystemParameters._cacheCaptionWidth == null)
				{
					SystemParameters._cacheCaptionWidth = SystemParameters.CreateInstance(SystemResourceKeyID.CaptionWidth);
				}
				return SystemParameters._cacheCaptionWidth;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002766 RID: 10086 RVA: 0x0018E66A File Offset: 0x0018D66A
		public static ResourceKey CaptionHeightKey
		{
			get
			{
				if (SystemParameters._cacheCaptionHeight == null)
				{
					SystemParameters._cacheCaptionHeight = SystemParameters.CreateInstance(SystemResourceKeyID.CaptionHeight);
				}
				return SystemParameters._cacheCaptionHeight;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x0018E687 File Offset: 0x0018D687
		public static ResourceKey SmallCaptionWidthKey
		{
			get
			{
				if (SystemParameters._cacheSmallCaptionWidth == null)
				{
					SystemParameters._cacheSmallCaptionWidth = SystemParameters.CreateInstance(SystemResourceKeyID.SmallCaptionWidth);
				}
				return SystemParameters._cacheSmallCaptionWidth;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002768 RID: 10088 RVA: 0x0018E6A4 File Offset: 0x0018D6A4
		public static ResourceKey MenuWidthKey
		{
			get
			{
				if (SystemParameters._cacheMenuWidth == null)
				{
					SystemParameters._cacheMenuWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MenuWidth);
				}
				return SystemParameters._cacheMenuWidth;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002769 RID: 10089 RVA: 0x0018E6C1 File Offset: 0x0018D6C1
		public static ResourceKey MenuHeightKey
		{
			get
			{
				if (SystemParameters._cacheMenuHeight == null)
				{
					SystemParameters._cacheMenuHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MenuHeight);
				}
				return SystemParameters._cacheMenuHeight;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x0600276A RID: 10090 RVA: 0x0018E6E0 File Offset: 0x0018D6E0
		public static double ThinHorizontalBorderHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[40])
					{
						SystemParameters._cacheValid[40] = true;
						SystemParameters._thinHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXBORDER));
					}
				}
				return SystemParameters._thinHorizontalBorderHeight;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x0600276B RID: 10091 RVA: 0x0018E74C File Offset: 0x0018D74C
		public static double ThinVerticalBorderWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[41])
					{
						SystemParameters._cacheValid[41] = true;
						SystemParameters._thinVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYBORDER));
					}
				}
				return SystemParameters._thinVerticalBorderWidth;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x0600276C RID: 10092 RVA: 0x0018E7B8 File Offset: 0x0018D7B8
		public static double CursorWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[42])
					{
						SystemParameters._cacheValid[42] = true;
						SystemParameters._cursorWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXCURSOR));
					}
				}
				return SystemParameters._cursorWidth;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x0018E824 File Offset: 0x0018D824
		public static double CursorHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[43])
					{
						SystemParameters._cacheValid[43] = true;
						SystemParameters._cursorHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYCURSOR));
					}
				}
				return SystemParameters._cursorHeight;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x0600276E RID: 10094 RVA: 0x0018E890 File Offset: 0x0018D890
		public static double ThickHorizontalBorderHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[44])
					{
						SystemParameters._cacheValid[44] = true;
						SystemParameters._thickHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXEDGE));
					}
				}
				return SystemParameters._thickHorizontalBorderHeight;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x0018E8FC File Offset: 0x0018D8FC
		public static double ThickVerticalBorderWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[45])
					{
						SystemParameters._cacheValid[45] = true;
						SystemParameters._thickVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYEDGE));
					}
				}
				return SystemParameters._thickVerticalBorderWidth;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002770 RID: 10096 RVA: 0x0018E968 File Offset: 0x0018D968
		public static double MinimumHorizontalDragDistance
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[46])
					{
						SystemParameters._cacheValid[46] = true;
						SystemParameters._minimumHorizontalDragDistance = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXDRAG));
					}
				}
				return SystemParameters._minimumHorizontalDragDistance;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x0018E9D4 File Offset: 0x0018D9D4
		public static double MinimumVerticalDragDistance
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[47])
					{
						SystemParameters._cacheValid[47] = true;
						SystemParameters._minimumVerticalDragDistance = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYDRAG));
					}
				}
				return SystemParameters._minimumVerticalDragDistance;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002772 RID: 10098 RVA: 0x0018EA40 File Offset: 0x0018DA40
		public static double FixedFrameHorizontalBorderHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[48])
					{
						SystemParameters._cacheValid[48] = true;
						SystemParameters._fixedFrameHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXFIXEDFRAME));
					}
				}
				return SystemParameters._fixedFrameHorizontalBorderHeight;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002773 RID: 10099 RVA: 0x0018EAAC File Offset: 0x0018DAAC
		public static double FixedFrameVerticalBorderWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[49])
					{
						SystemParameters._cacheValid[49] = true;
						SystemParameters._fixedFrameVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYFIXEDFRAME));
					}
				}
				return SystemParameters._fixedFrameVerticalBorderWidth;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002774 RID: 10100 RVA: 0x0018EB18 File Offset: 0x0018DB18
		public static double FocusHorizontalBorderHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[50])
					{
						SystemParameters._cacheValid[50] = true;
						SystemParameters._focusHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXFOCUSBORDER));
					}
				}
				return SystemParameters._focusHorizontalBorderHeight;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002775 RID: 10101 RVA: 0x0018EB84 File Offset: 0x0018DB84
		public static double FocusVerticalBorderWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[51])
					{
						SystemParameters._cacheValid[51] = true;
						SystemParameters._focusVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYFOCUSBORDER));
					}
				}
				return SystemParameters._focusVerticalBorderWidth;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002776 RID: 10102 RVA: 0x0018EBF0 File Offset: 0x0018DBF0
		public static double FullPrimaryScreenWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[52])
					{
						SystemParameters._cacheValid[52] = true;
						SystemParameters._fullPrimaryScreenWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXFULLSCREEN));
					}
				}
				return SystemParameters._fullPrimaryScreenWidth;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002777 RID: 10103 RVA: 0x0018EC5C File Offset: 0x0018DC5C
		public static double FullPrimaryScreenHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[53])
					{
						SystemParameters._cacheValid[53] = true;
						SystemParameters._fullPrimaryScreenHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYFULLSCREEN));
					}
				}
				return SystemParameters._fullPrimaryScreenHeight;
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x0018ECC8 File Offset: 0x0018DCC8
		public static double HorizontalScrollBarButtonWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[54])
					{
						SystemParameters._cacheValid[54] = true;
						SystemParameters._horizontalScrollBarButtonWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXHSCROLL));
					}
				}
				return SystemParameters._horizontalScrollBarButtonWidth;
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002779 RID: 10105 RVA: 0x0018ED34 File Offset: 0x0018DD34
		public static double HorizontalScrollBarHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[55])
					{
						SystemParameters._cacheValid[55] = true;
						SystemParameters._horizontalScrollBarHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYHSCROLL));
					}
				}
				return SystemParameters._horizontalScrollBarHeight;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x0600277A RID: 10106 RVA: 0x0018EDA0 File Offset: 0x0018DDA0
		public static double HorizontalScrollBarThumbWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[56])
					{
						SystemParameters._cacheValid[56] = true;
						SystemParameters._horizontalScrollBarThumbWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXHTHUMB));
					}
				}
				return SystemParameters._horizontalScrollBarThumbWidth;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x0600277B RID: 10107 RVA: 0x0018EE0C File Offset: 0x0018DE0C
		public static double IconWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[57])
					{
						SystemParameters._cacheValid[57] = true;
						SystemParameters._iconWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXICON));
					}
				}
				return SystemParameters._iconWidth;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x0600277C RID: 10108 RVA: 0x0018EE78 File Offset: 0x0018DE78
		public static double IconHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[58])
					{
						SystemParameters._cacheValid[58] = true;
						SystemParameters._iconHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYICON));
					}
				}
				return SystemParameters._iconHeight;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x0018EEE4 File Offset: 0x0018DEE4
		public static double IconGridWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[59])
					{
						SystemParameters._cacheValid[59] = true;
						SystemParameters._iconGridWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXICONSPACING));
					}
				}
				return SystemParameters._iconGridWidth;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x0600277E RID: 10110 RVA: 0x0018EF50 File Offset: 0x0018DF50
		public static double IconGridHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[60])
					{
						SystemParameters._cacheValid[60] = true;
						SystemParameters._iconGridHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYICONSPACING));
					}
				}
				return SystemParameters._iconGridHeight;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x0600277F RID: 10111 RVA: 0x0018EFBC File Offset: 0x0018DFBC
		public static double MaximizedPrimaryScreenWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[61])
					{
						SystemParameters._cacheValid[61] = true;
						SystemParameters._maximizedPrimaryScreenWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMAXIMIZED));
					}
				}
				return SystemParameters._maximizedPrimaryScreenWidth;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06002780 RID: 10112 RVA: 0x0018F028 File Offset: 0x0018E028
		public static double MaximizedPrimaryScreenHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[62])
					{
						SystemParameters._cacheValid[62] = true;
						SystemParameters._maximizedPrimaryScreenHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMAXIMIZED));
					}
				}
				return SystemParameters._maximizedPrimaryScreenHeight;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06002781 RID: 10113 RVA: 0x0018F094 File Offset: 0x0018E094
		public static double MaximumWindowTrackWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[63])
					{
						SystemParameters._cacheValid[63] = true;
						SystemParameters._maximumWindowTrackWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMAXTRACK));
					}
				}
				return SystemParameters._maximumWindowTrackWidth;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x0018F100 File Offset: 0x0018E100
		public static double MaximumWindowTrackHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[64])
					{
						SystemParameters._cacheValid[64] = true;
						SystemParameters._maximumWindowTrackHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMAXTRACK));
					}
				}
				return SystemParameters._maximumWindowTrackHeight;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06002783 RID: 10115 RVA: 0x0018F16C File Offset: 0x0018E16C
		public static double MenuCheckmarkWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[65])
					{
						SystemParameters._cacheValid[65] = true;
						SystemParameters._menuCheckmarkWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMENUCHECK));
					}
				}
				return SystemParameters._menuCheckmarkWidth;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002784 RID: 10116 RVA: 0x0018F1D8 File Offset: 0x0018E1D8
		public static double MenuCheckmarkHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[66])
					{
						SystemParameters._cacheValid[66] = true;
						SystemParameters._menuCheckmarkHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMENUCHECK));
					}
				}
				return SystemParameters._menuCheckmarkHeight;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x0018F244 File Offset: 0x0018E244
		public static double MenuButtonWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[67])
					{
						SystemParameters._cacheValid[67] = true;
						SystemParameters._menuButtonWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMENUSIZE));
					}
				}
				return SystemParameters._menuButtonWidth;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002786 RID: 10118 RVA: 0x0018F2B0 File Offset: 0x0018E2B0
		public static double MenuButtonHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[68])
					{
						SystemParameters._cacheValid[68] = true;
						SystemParameters._menuButtonHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMENUSIZE));
					}
				}
				return SystemParameters._menuButtonHeight;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x0018F31C File Offset: 0x0018E31C
		public static double MinimumWindowWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[69])
					{
						SystemParameters._cacheValid[69] = true;
						SystemParameters._minimumWindowWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMIN));
					}
				}
				return SystemParameters._minimumWindowWidth;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x0018F388 File Offset: 0x0018E388
		public static double MinimumWindowHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[70])
					{
						SystemParameters._cacheValid[70] = true;
						SystemParameters._minimumWindowHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMIN));
					}
				}
				return SystemParameters._minimumWindowHeight;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002789 RID: 10121 RVA: 0x0018F3F4 File Offset: 0x0018E3F4
		public static double MinimizedWindowWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[71])
					{
						SystemParameters._cacheValid[71] = true;
						SystemParameters._minimizedWindowWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMINIMIZED));
					}
				}
				return SystemParameters._minimizedWindowWidth;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x0018F460 File Offset: 0x0018E460
		public static double MinimizedWindowHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[72])
					{
						SystemParameters._cacheValid[72] = true;
						SystemParameters._minimizedWindowHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMINIMIZED));
					}
				}
				return SystemParameters._minimizedWindowHeight;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x0018F4CC File Offset: 0x0018E4CC
		public static double MinimizedGridWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[73])
					{
						SystemParameters._cacheValid[73] = true;
						SystemParameters._minimizedGridWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMINSPACING));
					}
				}
				return SystemParameters._minimizedGridWidth;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x0018F538 File Offset: 0x0018E538
		public static double MinimizedGridHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[74])
					{
						SystemParameters._cacheValid[74] = true;
						SystemParameters._minimizedGridHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMINSPACING));
					}
				}
				return SystemParameters._minimizedGridHeight;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x0018F5A4 File Offset: 0x0018E5A4
		public static double MinimumWindowTrackWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[75])
					{
						SystemParameters._cacheValid[75] = true;
						SystemParameters._minimumWindowTrackWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMINTRACK));
					}
				}
				return SystemParameters._minimumWindowTrackWidth;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x0018F610 File Offset: 0x0018E610
		public static double MinimumWindowTrackHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[76])
					{
						SystemParameters._cacheValid[76] = true;
						SystemParameters._minimumWindowTrackHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMINTRACK));
					}
				}
				return SystemParameters._minimumWindowTrackHeight;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x0018F67C File Offset: 0x0018E67C
		public static double PrimaryScreenWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[77])
					{
						SystemParameters._cacheValid[77] = true;
						SystemParameters._primaryScreenWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXSCREEN));
					}
				}
				return SystemParameters._primaryScreenWidth;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002790 RID: 10128 RVA: 0x0018F6E8 File Offset: 0x0018E6E8
		public static double PrimaryScreenHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[78])
					{
						SystemParameters._cacheValid[78] = true;
						SystemParameters._primaryScreenHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYSCREEN));
					}
				}
				return SystemParameters._primaryScreenHeight;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x0018F754 File Offset: 0x0018E754
		public static double WindowCaptionButtonWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[79])
					{
						SystemParameters._cacheValid[79] = true;
						SystemParameters._windowCaptionButtonWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXSIZE));
					}
				}
				return SystemParameters._windowCaptionButtonWidth;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002792 RID: 10130 RVA: 0x0018F7C0 File Offset: 0x0018E7C0
		public static double WindowCaptionButtonHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[80])
					{
						SystemParameters._cacheValid[80] = true;
						SystemParameters._windowCaptionButtonHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYSIZE));
					}
				}
				return SystemParameters._windowCaptionButtonHeight;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x0018F82C File Offset: 0x0018E82C
		public static double ResizeFrameHorizontalBorderHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[81])
					{
						SystemParameters._cacheValid[81] = true;
						SystemParameters._resizeFrameHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXFRAME));
					}
				}
				return SystemParameters._resizeFrameHorizontalBorderHeight;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002794 RID: 10132 RVA: 0x0018F898 File Offset: 0x0018E898
		public static double ResizeFrameVerticalBorderWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[82])
					{
						SystemParameters._cacheValid[82] = true;
						SystemParameters._resizeFrameVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYFRAME));
					}
				}
				return SystemParameters._resizeFrameVerticalBorderWidth;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002795 RID: 10133 RVA: 0x0018F904 File Offset: 0x0018E904
		public static double SmallIconWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[83])
					{
						SystemParameters._cacheValid[83] = true;
						SystemParameters._smallIconWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXSMICON));
					}
				}
				return SystemParameters._smallIconWidth;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002796 RID: 10134 RVA: 0x0018F970 File Offset: 0x0018E970
		public static double SmallIconHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[84])
					{
						SystemParameters._cacheValid[84] = true;
						SystemParameters._smallIconHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYSMICON));
					}
				}
				return SystemParameters._smallIconHeight;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002797 RID: 10135 RVA: 0x0018F9DC File Offset: 0x0018E9DC
		public static double SmallWindowCaptionButtonWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[85])
					{
						SystemParameters._cacheValid[85] = true;
						SystemParameters._smallWindowCaptionButtonWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXSMSIZE));
					}
				}
				return SystemParameters._smallWindowCaptionButtonWidth;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002798 RID: 10136 RVA: 0x0018FA48 File Offset: 0x0018EA48
		public static double SmallWindowCaptionButtonHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[86])
					{
						SystemParameters._cacheValid[86] = true;
						SystemParameters._smallWindowCaptionButtonHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYSMSIZE));
					}
				}
				return SystemParameters._smallWindowCaptionButtonHeight;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002799 RID: 10137 RVA: 0x0018FAB4 File Offset: 0x0018EAB4
		public static double VirtualScreenWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[87])
					{
						SystemParameters._cacheValid[87] = true;
						SystemParameters._virtualScreenWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXVIRTUALSCREEN));
					}
				}
				return SystemParameters._virtualScreenWidth;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x0018FB20 File Offset: 0x0018EB20
		public static double VirtualScreenHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[88])
					{
						SystemParameters._cacheValid[88] = true;
						SystemParameters._virtualScreenHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYVIRTUALSCREEN));
					}
				}
				return SystemParameters._virtualScreenHeight;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x0600279B RID: 10139 RVA: 0x0018FB8C File Offset: 0x0018EB8C
		public static double VerticalScrollBarWidth
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[89])
					{
						SystemParameters._cacheValid[89] = true;
						SystemParameters._verticalScrollBarWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXVSCROLL));
					}
				}
				return SystemParameters._verticalScrollBarWidth;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x0018FBF8 File Offset: 0x0018EBF8
		public static double VerticalScrollBarButtonHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[90])
					{
						SystemParameters._cacheValid[90] = true;
						SystemParameters._verticalScrollBarButtonHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYVSCROLL));
					}
				}
				return SystemParameters._verticalScrollBarButtonHeight;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x0600279D RID: 10141 RVA: 0x0018FC64 File Offset: 0x0018EC64
		public static double WindowCaptionHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[91])
					{
						SystemParameters._cacheValid[91] = true;
						SystemParameters._windowCaptionHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYCAPTION));
					}
				}
				return SystemParameters._windowCaptionHeight;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x0600279E RID: 10142 RVA: 0x0018FCD0 File Offset: 0x0018ECD0
		public static double KanjiWindowHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[92])
					{
						SystemParameters._cacheValid[92] = true;
						SystemParameters._kanjiWindowHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYKANJIWINDOW));
					}
				}
				return SystemParameters._kanjiWindowHeight;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x0600279F RID: 10143 RVA: 0x0018FD3C File Offset: 0x0018ED3C
		public static double MenuBarHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[93])
					{
						SystemParameters._cacheValid[93] = true;
						SystemParameters._menuBarHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMENU));
					}
				}
				return SystemParameters._menuBarHeight;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x0018FDA8 File Offset: 0x0018EDA8
		public static double VerticalScrollBarThumbHeight
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[94])
					{
						SystemParameters._cacheValid[94] = true;
						SystemParameters._verticalScrollBarThumbHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYVTHUMB));
					}
				}
				return SystemParameters._verticalScrollBarThumbHeight;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x060027A1 RID: 10145 RVA: 0x0018FE14 File Offset: 0x0018EE14
		public static bool IsImmEnabled
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[95])
					{
						SystemParameters._cacheValid[95] = true;
						SystemParameters._isImmEnabled = (UnsafeNativeMethods.GetSystemMetrics(SM.IMMENABLED) != 0);
					}
				}
				return SystemParameters._isImmEnabled;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x060027A2 RID: 10146 RVA: 0x0018FE80 File Offset: 0x0018EE80
		public static bool IsMediaCenter
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[96])
					{
						SystemParameters._cacheValid[96] = true;
						SystemParameters._isMediaCenter = (UnsafeNativeMethods.GetSystemMetrics(SM.MEDIACENTER) != 0);
					}
				}
				return SystemParameters._isMediaCenter;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x060027A3 RID: 10147 RVA: 0x0018FEEC File Offset: 0x0018EEEC
		public static bool IsMenuDropRightAligned
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[97])
					{
						SystemParameters._cacheValid[97] = true;
						SystemParameters._isMenuDropRightAligned = (UnsafeNativeMethods.GetSystemMetrics(SM.MENUDROPALIGNMENT) != 0);
					}
				}
				return SystemParameters._isMenuDropRightAligned;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x060027A4 RID: 10148 RVA: 0x0018FF58 File Offset: 0x0018EF58
		public static bool IsMiddleEastEnabled
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[98])
					{
						SystemParameters._cacheValid[98] = true;
						SystemParameters._isMiddleEastEnabled = (UnsafeNativeMethods.GetSystemMetrics(SM.MIDEASTENABLED) != 0);
					}
				}
				return SystemParameters._isMiddleEastEnabled;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x060027A5 RID: 10149 RVA: 0x0018FFC4 File Offset: 0x0018EFC4
		public static bool IsMousePresent
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[99])
					{
						SystemParameters._cacheValid[99] = true;
						SystemParameters._isMousePresent = (UnsafeNativeMethods.GetSystemMetrics(SM.MOUSEPRESENT) != 0);
					}
				}
				return SystemParameters._isMousePresent;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x060027A6 RID: 10150 RVA: 0x00190030 File Offset: 0x0018F030
		public static bool IsMouseWheelPresent
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[100])
					{
						SystemParameters._cacheValid[100] = true;
						SystemParameters._isMouseWheelPresent = (UnsafeNativeMethods.GetSystemMetrics(SM.MOUSEWHEELPRESENT) != 0);
					}
				}
				return SystemParameters._isMouseWheelPresent;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x060027A7 RID: 10151 RVA: 0x0019009C File Offset: 0x0018F09C
		public static bool IsPenWindows
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[101])
					{
						SystemParameters._cacheValid[101] = true;
						SystemParameters._isPenWindows = (UnsafeNativeMethods.GetSystemMetrics(SM.PENWINDOWS) != 0);
					}
				}
				return SystemParameters._isPenWindows;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x00190108 File Offset: 0x0018F108
		public static bool IsRemotelyControlled
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[102])
					{
						SystemParameters._cacheValid[102] = true;
						SystemParameters._isRemotelyControlled = (UnsafeNativeMethods.GetSystemMetrics(SM.REMOTECONTROL) != 0);
					}
				}
				return SystemParameters._isRemotelyControlled;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x060027A9 RID: 10153 RVA: 0x00190178 File Offset: 0x0018F178
		public static bool IsRemoteSession
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[103])
					{
						SystemParameters._cacheValid[103] = true;
						SystemParameters._isRemoteSession = (UnsafeNativeMethods.GetSystemMetrics(SM.REMOTESESSION) != 0);
					}
				}
				return SystemParameters._isRemoteSession;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x060027AA RID: 10154 RVA: 0x001901E8 File Offset: 0x0018F1E8
		public static bool ShowSounds
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[104])
					{
						SystemParameters._cacheValid[104] = true;
						SystemParameters._showSounds = (UnsafeNativeMethods.GetSystemMetrics(SM.SHOWSOUNDS) != 0);
					}
				}
				return SystemParameters._showSounds;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x00190254 File Offset: 0x0018F254
		public static bool IsSlowMachine
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[105])
					{
						SystemParameters._cacheValid[105] = true;
						SystemParameters._isSlowMachine = (UnsafeNativeMethods.GetSystemMetrics(SM.SLOWMACHINE) != 0);
					}
				}
				return SystemParameters._isSlowMachine;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x060027AC RID: 10156 RVA: 0x001902C0 File Offset: 0x0018F2C0
		public static bool SwapButtons
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[106])
					{
						SystemParameters._cacheValid[106] = true;
						SystemParameters._swapButtons = (UnsafeNativeMethods.GetSystemMetrics(SM.SWAPBUTTON) != 0);
					}
				}
				return SystemParameters._swapButtons;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x060027AD RID: 10157 RVA: 0x0019032C File Offset: 0x0018F32C
		public static bool IsTabletPC
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[107])
					{
						SystemParameters._cacheValid[107] = true;
						SystemParameters._isTabletPC = (UnsafeNativeMethods.GetSystemMetrics(SM.TABLETPC) != 0);
					}
				}
				return SystemParameters._isTabletPC;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x00190398 File Offset: 0x0018F398
		public static double VirtualScreenLeft
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[108])
					{
						SystemParameters._cacheValid[108] = true;
						SystemParameters._virtualScreenLeft = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.XVIRTUALSCREEN));
					}
				}
				return SystemParameters._virtualScreenLeft;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x060027AF RID: 10159 RVA: 0x00190404 File Offset: 0x0018F404
		public static double VirtualScreenTop
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[109])
					{
						SystemParameters._cacheValid[109] = true;
						SystemParameters._virtualScreenTop = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.YVIRTUALSCREEN));
					}
				}
				return SystemParameters._virtualScreenTop;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x060027B0 RID: 10160 RVA: 0x00190470 File Offset: 0x0018F470
		public static ResourceKey ThinHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheThinHorizontalBorderHeight == null)
				{
					SystemParameters._cacheThinHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.ThinHorizontalBorderHeight);
				}
				return SystemParameters._cacheThinHorizontalBorderHeight;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x060027B1 RID: 10161 RVA: 0x0019048A File Offset: 0x0018F48A
		public static ResourceKey ThinVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheThinVerticalBorderWidth == null)
				{
					SystemParameters._cacheThinVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.ThinVerticalBorderWidth);
				}
				return SystemParameters._cacheThinVerticalBorderWidth;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x060027B2 RID: 10162 RVA: 0x001904A4 File Offset: 0x0018F4A4
		public static ResourceKey CursorWidthKey
		{
			get
			{
				if (SystemParameters._cacheCursorWidth == null)
				{
					SystemParameters._cacheCursorWidth = SystemParameters.CreateInstance(SystemResourceKeyID.CursorWidth);
				}
				return SystemParameters._cacheCursorWidth;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x060027B3 RID: 10163 RVA: 0x001904BE File Offset: 0x0018F4BE
		public static ResourceKey CursorHeightKey
		{
			get
			{
				if (SystemParameters._cacheCursorHeight == null)
				{
					SystemParameters._cacheCursorHeight = SystemParameters.CreateInstance(SystemResourceKeyID.CursorHeight);
				}
				return SystemParameters._cacheCursorHeight;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x060027B4 RID: 10164 RVA: 0x001904D8 File Offset: 0x0018F4D8
		public static ResourceKey ThickHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheThickHorizontalBorderHeight == null)
				{
					SystemParameters._cacheThickHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.ThickHorizontalBorderHeight);
				}
				return SystemParameters._cacheThickHorizontalBorderHeight;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x060027B5 RID: 10165 RVA: 0x001904F2 File Offset: 0x0018F4F2
		public static ResourceKey ThickVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheThickVerticalBorderWidth == null)
				{
					SystemParameters._cacheThickVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.ThickVerticalBorderWidth);
				}
				return SystemParameters._cacheThickVerticalBorderWidth;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x060027B6 RID: 10166 RVA: 0x0019050C File Offset: 0x0018F50C
		public static ResourceKey FixedFrameHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheFixedFrameHorizontalBorderHeight == null)
				{
					SystemParameters._cacheFixedFrameHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.FixedFrameHorizontalBorderHeight);
				}
				return SystemParameters._cacheFixedFrameHorizontalBorderHeight;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x00190526 File Offset: 0x0018F526
		public static ResourceKey FixedFrameVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheFixedFrameVerticalBorderWidth == null)
				{
					SystemParameters._cacheFixedFrameVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.FixedFrameVerticalBorderWidth);
				}
				return SystemParameters._cacheFixedFrameVerticalBorderWidth;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x060027B8 RID: 10168 RVA: 0x00190540 File Offset: 0x0018F540
		public static ResourceKey FocusHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheFocusHorizontalBorderHeight == null)
				{
					SystemParameters._cacheFocusHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.FocusHorizontalBorderHeight);
				}
				return SystemParameters._cacheFocusHorizontalBorderHeight;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x060027B9 RID: 10169 RVA: 0x0019055A File Offset: 0x0018F55A
		public static ResourceKey FocusVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheFocusVerticalBorderWidth == null)
				{
					SystemParameters._cacheFocusVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.FocusVerticalBorderWidth);
				}
				return SystemParameters._cacheFocusVerticalBorderWidth;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x060027BA RID: 10170 RVA: 0x00190574 File Offset: 0x0018F574
		public static ResourceKey FullPrimaryScreenWidthKey
		{
			get
			{
				if (SystemParameters._cacheFullPrimaryScreenWidth == null)
				{
					SystemParameters._cacheFullPrimaryScreenWidth = SystemParameters.CreateInstance(SystemResourceKeyID.FullPrimaryScreenWidth);
				}
				return SystemParameters._cacheFullPrimaryScreenWidth;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x060027BB RID: 10171 RVA: 0x0019058E File Offset: 0x0018F58E
		public static ResourceKey FullPrimaryScreenHeightKey
		{
			get
			{
				if (SystemParameters._cacheFullPrimaryScreenHeight == null)
				{
					SystemParameters._cacheFullPrimaryScreenHeight = SystemParameters.CreateInstance(SystemResourceKeyID.FullPrimaryScreenHeight);
				}
				return SystemParameters._cacheFullPrimaryScreenHeight;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x060027BC RID: 10172 RVA: 0x001905A8 File Offset: 0x0018F5A8
		public static ResourceKey HorizontalScrollBarButtonWidthKey
		{
			get
			{
				if (SystemParameters._cacheHorizontalScrollBarButtonWidth == null)
				{
					SystemParameters._cacheHorizontalScrollBarButtonWidth = SystemParameters.CreateInstance(SystemResourceKeyID.HorizontalScrollBarButtonWidth);
				}
				return SystemParameters._cacheHorizontalScrollBarButtonWidth;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x060027BD RID: 10173 RVA: 0x001905C2 File Offset: 0x0018F5C2
		public static ResourceKey HorizontalScrollBarHeightKey
		{
			get
			{
				if (SystemParameters._cacheHorizontalScrollBarHeight == null)
				{
					SystemParameters._cacheHorizontalScrollBarHeight = SystemParameters.CreateInstance(SystemResourceKeyID.HorizontalScrollBarHeight);
				}
				return SystemParameters._cacheHorizontalScrollBarHeight;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x060027BE RID: 10174 RVA: 0x001905DC File Offset: 0x0018F5DC
		public static ResourceKey HorizontalScrollBarThumbWidthKey
		{
			get
			{
				if (SystemParameters._cacheHorizontalScrollBarThumbWidth == null)
				{
					SystemParameters._cacheHorizontalScrollBarThumbWidth = SystemParameters.CreateInstance(SystemResourceKeyID.HorizontalScrollBarThumbWidth);
				}
				return SystemParameters._cacheHorizontalScrollBarThumbWidth;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x001905F6 File Offset: 0x0018F5F6
		public static ResourceKey IconWidthKey
		{
			get
			{
				if (SystemParameters._cacheIconWidth == null)
				{
					SystemParameters._cacheIconWidth = SystemParameters.CreateInstance(SystemResourceKeyID.IconWidth);
				}
				return SystemParameters._cacheIconWidth;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x00190610 File Offset: 0x0018F610
		public static ResourceKey IconHeightKey
		{
			get
			{
				if (SystemParameters._cacheIconHeight == null)
				{
					SystemParameters._cacheIconHeight = SystemParameters.CreateInstance(SystemResourceKeyID.IconHeight);
				}
				return SystemParameters._cacheIconHeight;
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x0019062A File Offset: 0x0018F62A
		public static ResourceKey IconGridWidthKey
		{
			get
			{
				if (SystemParameters._cacheIconGridWidth == null)
				{
					SystemParameters._cacheIconGridWidth = SystemParameters.CreateInstance(SystemResourceKeyID.IconGridWidth);
				}
				return SystemParameters._cacheIconGridWidth;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x060027C2 RID: 10178 RVA: 0x00190644 File Offset: 0x0018F644
		public static ResourceKey IconGridHeightKey
		{
			get
			{
				if (SystemParameters._cacheIconGridHeight == null)
				{
					SystemParameters._cacheIconGridHeight = SystemParameters.CreateInstance(SystemResourceKeyID.IconGridHeight);
				}
				return SystemParameters._cacheIconGridHeight;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x0019065E File Offset: 0x0018F65E
		public static ResourceKey MaximizedPrimaryScreenWidthKey
		{
			get
			{
				if (SystemParameters._cacheMaximizedPrimaryScreenWidth == null)
				{
					SystemParameters._cacheMaximizedPrimaryScreenWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MaximizedPrimaryScreenWidth);
				}
				return SystemParameters._cacheMaximizedPrimaryScreenWidth;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x060027C4 RID: 10180 RVA: 0x00190678 File Offset: 0x0018F678
		public static ResourceKey MaximizedPrimaryScreenHeightKey
		{
			get
			{
				if (SystemParameters._cacheMaximizedPrimaryScreenHeight == null)
				{
					SystemParameters._cacheMaximizedPrimaryScreenHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MaximizedPrimaryScreenHeight);
				}
				return SystemParameters._cacheMaximizedPrimaryScreenHeight;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x060027C5 RID: 10181 RVA: 0x00190692 File Offset: 0x0018F692
		public static ResourceKey MaximumWindowTrackWidthKey
		{
			get
			{
				if (SystemParameters._cacheMaximumWindowTrackWidth == null)
				{
					SystemParameters._cacheMaximumWindowTrackWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MaximumWindowTrackWidth);
				}
				return SystemParameters._cacheMaximumWindowTrackWidth;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x060027C6 RID: 10182 RVA: 0x001906AC File Offset: 0x0018F6AC
		public static ResourceKey MaximumWindowTrackHeightKey
		{
			get
			{
				if (SystemParameters._cacheMaximumWindowTrackHeight == null)
				{
					SystemParameters._cacheMaximumWindowTrackHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MaximumWindowTrackHeight);
				}
				return SystemParameters._cacheMaximumWindowTrackHeight;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x060027C7 RID: 10183 RVA: 0x001906C6 File Offset: 0x0018F6C6
		public static ResourceKey MenuCheckmarkWidthKey
		{
			get
			{
				if (SystemParameters._cacheMenuCheckmarkWidth == null)
				{
					SystemParameters._cacheMenuCheckmarkWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MenuCheckmarkWidth);
				}
				return SystemParameters._cacheMenuCheckmarkWidth;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x060027C8 RID: 10184 RVA: 0x001906E0 File Offset: 0x0018F6E0
		public static ResourceKey MenuCheckmarkHeightKey
		{
			get
			{
				if (SystemParameters._cacheMenuCheckmarkHeight == null)
				{
					SystemParameters._cacheMenuCheckmarkHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MenuCheckmarkHeight);
				}
				return SystemParameters._cacheMenuCheckmarkHeight;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x060027C9 RID: 10185 RVA: 0x001906FA File Offset: 0x0018F6FA
		public static ResourceKey MenuButtonWidthKey
		{
			get
			{
				if (SystemParameters._cacheMenuButtonWidth == null)
				{
					SystemParameters._cacheMenuButtonWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MenuButtonWidth);
				}
				return SystemParameters._cacheMenuButtonWidth;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x060027CA RID: 10186 RVA: 0x00190714 File Offset: 0x0018F714
		public static ResourceKey MenuButtonHeightKey
		{
			get
			{
				if (SystemParameters._cacheMenuButtonHeight == null)
				{
					SystemParameters._cacheMenuButtonHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MenuButtonHeight);
				}
				return SystemParameters._cacheMenuButtonHeight;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x060027CB RID: 10187 RVA: 0x0019072E File Offset: 0x0018F72E
		public static ResourceKey MinimumWindowWidthKey
		{
			get
			{
				if (SystemParameters._cacheMinimumWindowWidth == null)
				{
					SystemParameters._cacheMinimumWindowWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MinimumWindowWidth);
				}
				return SystemParameters._cacheMinimumWindowWidth;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x060027CC RID: 10188 RVA: 0x00190748 File Offset: 0x0018F748
		public static ResourceKey MinimumWindowHeightKey
		{
			get
			{
				if (SystemParameters._cacheMinimumWindowHeight == null)
				{
					SystemParameters._cacheMinimumWindowHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MinimumWindowHeight);
				}
				return SystemParameters._cacheMinimumWindowHeight;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x060027CD RID: 10189 RVA: 0x00190762 File Offset: 0x0018F762
		public static ResourceKey MinimizedWindowWidthKey
		{
			get
			{
				if (SystemParameters._cacheMinimizedWindowWidth == null)
				{
					SystemParameters._cacheMinimizedWindowWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizedWindowWidth);
				}
				return SystemParameters._cacheMinimizedWindowWidth;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060027CE RID: 10190 RVA: 0x0019077C File Offset: 0x0018F77C
		public static ResourceKey MinimizedWindowHeightKey
		{
			get
			{
				if (SystemParameters._cacheMinimizedWindowHeight == null)
				{
					SystemParameters._cacheMinimizedWindowHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizedWindowHeight);
				}
				return SystemParameters._cacheMinimizedWindowHeight;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060027CF RID: 10191 RVA: 0x00190796 File Offset: 0x0018F796
		public static ResourceKey MinimizedGridWidthKey
		{
			get
			{
				if (SystemParameters._cacheMinimizedGridWidth == null)
				{
					SystemParameters._cacheMinimizedGridWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizedGridWidth);
				}
				return SystemParameters._cacheMinimizedGridWidth;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060027D0 RID: 10192 RVA: 0x001907B0 File Offset: 0x0018F7B0
		public static ResourceKey MinimizedGridHeightKey
		{
			get
			{
				if (SystemParameters._cacheMinimizedGridHeight == null)
				{
					SystemParameters._cacheMinimizedGridHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizedGridHeight);
				}
				return SystemParameters._cacheMinimizedGridHeight;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060027D1 RID: 10193 RVA: 0x001907CA File Offset: 0x0018F7CA
		public static ResourceKey MinimumWindowTrackWidthKey
		{
			get
			{
				if (SystemParameters._cacheMinimumWindowTrackWidth == null)
				{
					SystemParameters._cacheMinimumWindowTrackWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MinimumWindowTrackWidth);
				}
				return SystemParameters._cacheMinimumWindowTrackWidth;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060027D2 RID: 10194 RVA: 0x001907E7 File Offset: 0x0018F7E7
		public static ResourceKey MinimumWindowTrackHeightKey
		{
			get
			{
				if (SystemParameters._cacheMinimumWindowTrackHeight == null)
				{
					SystemParameters._cacheMinimumWindowTrackHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MinimumWindowTrackHeight);
				}
				return SystemParameters._cacheMinimumWindowTrackHeight;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060027D3 RID: 10195 RVA: 0x00190804 File Offset: 0x0018F804
		public static ResourceKey PrimaryScreenWidthKey
		{
			get
			{
				if (SystemParameters._cachePrimaryScreenWidth == null)
				{
					SystemParameters._cachePrimaryScreenWidth = SystemParameters.CreateInstance(SystemResourceKeyID.PrimaryScreenWidth);
				}
				return SystemParameters._cachePrimaryScreenWidth;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060027D4 RID: 10196 RVA: 0x00190821 File Offset: 0x0018F821
		public static ResourceKey PrimaryScreenHeightKey
		{
			get
			{
				if (SystemParameters._cachePrimaryScreenHeight == null)
				{
					SystemParameters._cachePrimaryScreenHeight = SystemParameters.CreateInstance(SystemResourceKeyID.PrimaryScreenHeight);
				}
				return SystemParameters._cachePrimaryScreenHeight;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060027D5 RID: 10197 RVA: 0x0019083E File Offset: 0x0018F83E
		public static ResourceKey WindowCaptionButtonWidthKey
		{
			get
			{
				if (SystemParameters._cacheWindowCaptionButtonWidth == null)
				{
					SystemParameters._cacheWindowCaptionButtonWidth = SystemParameters.CreateInstance(SystemResourceKeyID.WindowCaptionButtonWidth);
				}
				return SystemParameters._cacheWindowCaptionButtonWidth;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060027D6 RID: 10198 RVA: 0x0019085B File Offset: 0x0018F85B
		public static ResourceKey WindowCaptionButtonHeightKey
		{
			get
			{
				if (SystemParameters._cacheWindowCaptionButtonHeight == null)
				{
					SystemParameters._cacheWindowCaptionButtonHeight = SystemParameters.CreateInstance(SystemResourceKeyID.WindowCaptionButtonHeight);
				}
				return SystemParameters._cacheWindowCaptionButtonHeight;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060027D7 RID: 10199 RVA: 0x00190878 File Offset: 0x0018F878
		public static ResourceKey ResizeFrameHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheResizeFrameHorizontalBorderHeight == null)
				{
					SystemParameters._cacheResizeFrameHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.ResizeFrameHorizontalBorderHeight);
				}
				return SystemParameters._cacheResizeFrameHorizontalBorderHeight;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060027D8 RID: 10200 RVA: 0x00190895 File Offset: 0x0018F895
		public static ResourceKey ResizeFrameVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheResizeFrameVerticalBorderWidth == null)
				{
					SystemParameters._cacheResizeFrameVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.ResizeFrameVerticalBorderWidth);
				}
				return SystemParameters._cacheResizeFrameVerticalBorderWidth;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x001908B2 File Offset: 0x0018F8B2
		public static ResourceKey SmallIconWidthKey
		{
			get
			{
				if (SystemParameters._cacheSmallIconWidth == null)
				{
					SystemParameters._cacheSmallIconWidth = SystemParameters.CreateInstance(SystemResourceKeyID.SmallIconWidth);
				}
				return SystemParameters._cacheSmallIconWidth;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060027DA RID: 10202 RVA: 0x001908CF File Offset: 0x0018F8CF
		public static ResourceKey SmallIconHeightKey
		{
			get
			{
				if (SystemParameters._cacheSmallIconHeight == null)
				{
					SystemParameters._cacheSmallIconHeight = SystemParameters.CreateInstance(SystemResourceKeyID.SmallIconHeight);
				}
				return SystemParameters._cacheSmallIconHeight;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060027DB RID: 10203 RVA: 0x001908EC File Offset: 0x0018F8EC
		public static ResourceKey SmallWindowCaptionButtonWidthKey
		{
			get
			{
				if (SystemParameters._cacheSmallWindowCaptionButtonWidth == null)
				{
					SystemParameters._cacheSmallWindowCaptionButtonWidth = SystemParameters.CreateInstance(SystemResourceKeyID.SmallWindowCaptionButtonWidth);
				}
				return SystemParameters._cacheSmallWindowCaptionButtonWidth;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x060027DC RID: 10204 RVA: 0x00190909 File Offset: 0x0018F909
		public static ResourceKey SmallWindowCaptionButtonHeightKey
		{
			get
			{
				if (SystemParameters._cacheSmallWindowCaptionButtonHeight == null)
				{
					SystemParameters._cacheSmallWindowCaptionButtonHeight = SystemParameters.CreateInstance(SystemResourceKeyID.SmallWindowCaptionButtonHeight);
				}
				return SystemParameters._cacheSmallWindowCaptionButtonHeight;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x060027DD RID: 10205 RVA: 0x00190926 File Offset: 0x0018F926
		public static ResourceKey VirtualScreenWidthKey
		{
			get
			{
				if (SystemParameters._cacheVirtualScreenWidth == null)
				{
					SystemParameters._cacheVirtualScreenWidth = SystemParameters.CreateInstance(SystemResourceKeyID.VirtualScreenWidth);
				}
				return SystemParameters._cacheVirtualScreenWidth;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x060027DE RID: 10206 RVA: 0x00190943 File Offset: 0x0018F943
		public static ResourceKey VirtualScreenHeightKey
		{
			get
			{
				if (SystemParameters._cacheVirtualScreenHeight == null)
				{
					SystemParameters._cacheVirtualScreenHeight = SystemParameters.CreateInstance(SystemResourceKeyID.VirtualScreenHeight);
				}
				return SystemParameters._cacheVirtualScreenHeight;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x00190960 File Offset: 0x0018F960
		public static ResourceKey VerticalScrollBarWidthKey
		{
			get
			{
				if (SystemParameters._cacheVerticalScrollBarWidth == null)
				{
					SystemParameters._cacheVerticalScrollBarWidth = SystemParameters.CreateInstance(SystemResourceKeyID.VerticalScrollBarWidth);
				}
				return SystemParameters._cacheVerticalScrollBarWidth;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x060027E0 RID: 10208 RVA: 0x0019097D File Offset: 0x0018F97D
		public static ResourceKey VerticalScrollBarButtonHeightKey
		{
			get
			{
				if (SystemParameters._cacheVerticalScrollBarButtonHeight == null)
				{
					SystemParameters._cacheVerticalScrollBarButtonHeight = SystemParameters.CreateInstance(SystemResourceKeyID.VerticalScrollBarButtonHeight);
				}
				return SystemParameters._cacheVerticalScrollBarButtonHeight;
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060027E1 RID: 10209 RVA: 0x0019099A File Offset: 0x0018F99A
		public static ResourceKey WindowCaptionHeightKey
		{
			get
			{
				if (SystemParameters._cacheWindowCaptionHeight == null)
				{
					SystemParameters._cacheWindowCaptionHeight = SystemParameters.CreateInstance(SystemResourceKeyID.WindowCaptionHeight);
				}
				return SystemParameters._cacheWindowCaptionHeight;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060027E2 RID: 10210 RVA: 0x001909B7 File Offset: 0x0018F9B7
		public static ResourceKey KanjiWindowHeightKey
		{
			get
			{
				if (SystemParameters._cacheKanjiWindowHeight == null)
				{
					SystemParameters._cacheKanjiWindowHeight = SystemParameters.CreateInstance(SystemResourceKeyID.KanjiWindowHeight);
				}
				return SystemParameters._cacheKanjiWindowHeight;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060027E3 RID: 10211 RVA: 0x001909D4 File Offset: 0x0018F9D4
		public static ResourceKey MenuBarHeightKey
		{
			get
			{
				if (SystemParameters._cacheMenuBarHeight == null)
				{
					SystemParameters._cacheMenuBarHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MenuBarHeight);
				}
				return SystemParameters._cacheMenuBarHeight;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060027E4 RID: 10212 RVA: 0x001909F1 File Offset: 0x0018F9F1
		public static ResourceKey SmallCaptionHeightKey
		{
			get
			{
				if (SystemParameters._cacheSmallCaptionHeight == null)
				{
					SystemParameters._cacheSmallCaptionHeight = SystemParameters.CreateInstance(SystemResourceKeyID.SmallCaptionHeight);
				}
				return SystemParameters._cacheSmallCaptionHeight;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x060027E5 RID: 10213 RVA: 0x00190A0E File Offset: 0x0018FA0E
		public static ResourceKey VerticalScrollBarThumbHeightKey
		{
			get
			{
				if (SystemParameters._cacheVerticalScrollBarThumbHeight == null)
				{
					SystemParameters._cacheVerticalScrollBarThumbHeight = SystemParameters.CreateInstance(SystemResourceKeyID.VerticalScrollBarThumbHeight);
				}
				return SystemParameters._cacheVerticalScrollBarThumbHeight;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x00190A2B File Offset: 0x0018FA2B
		public static ResourceKey IsImmEnabledKey
		{
			get
			{
				if (SystemParameters._cacheIsImmEnabled == null)
				{
					SystemParameters._cacheIsImmEnabled = SystemParameters.CreateInstance(SystemResourceKeyID.IsImmEnabled);
				}
				return SystemParameters._cacheIsImmEnabled;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060027E7 RID: 10215 RVA: 0x00190A48 File Offset: 0x0018FA48
		public static ResourceKey IsMediaCenterKey
		{
			get
			{
				if (SystemParameters._cacheIsMediaCenter == null)
				{
					SystemParameters._cacheIsMediaCenter = SystemParameters.CreateInstance(SystemResourceKeyID.IsMediaCenter);
				}
				return SystemParameters._cacheIsMediaCenter;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x060027E8 RID: 10216 RVA: 0x00190A65 File Offset: 0x0018FA65
		public static ResourceKey IsMenuDropRightAlignedKey
		{
			get
			{
				if (SystemParameters._cacheIsMenuDropRightAligned == null)
				{
					SystemParameters._cacheIsMenuDropRightAligned = SystemParameters.CreateInstance(SystemResourceKeyID.IsMenuDropRightAligned);
				}
				return SystemParameters._cacheIsMenuDropRightAligned;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x060027E9 RID: 10217 RVA: 0x00190A82 File Offset: 0x0018FA82
		public static ResourceKey IsMiddleEastEnabledKey
		{
			get
			{
				if (SystemParameters._cacheIsMiddleEastEnabled == null)
				{
					SystemParameters._cacheIsMiddleEastEnabled = SystemParameters.CreateInstance(SystemResourceKeyID.IsMiddleEastEnabled);
				}
				return SystemParameters._cacheIsMiddleEastEnabled;
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x00190A9F File Offset: 0x0018FA9F
		public static ResourceKey IsMousePresentKey
		{
			get
			{
				if (SystemParameters._cacheIsMousePresent == null)
				{
					SystemParameters._cacheIsMousePresent = SystemParameters.CreateInstance(SystemResourceKeyID.IsMousePresent);
				}
				return SystemParameters._cacheIsMousePresent;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x060027EB RID: 10219 RVA: 0x00190ABC File Offset: 0x0018FABC
		public static ResourceKey IsMouseWheelPresentKey
		{
			get
			{
				if (SystemParameters._cacheIsMouseWheelPresent == null)
				{
					SystemParameters._cacheIsMouseWheelPresent = SystemParameters.CreateInstance(SystemResourceKeyID.IsMouseWheelPresent);
				}
				return SystemParameters._cacheIsMouseWheelPresent;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x060027EC RID: 10220 RVA: 0x00190AD9 File Offset: 0x0018FAD9
		public static ResourceKey IsPenWindowsKey
		{
			get
			{
				if (SystemParameters._cacheIsPenWindows == null)
				{
					SystemParameters._cacheIsPenWindows = SystemParameters.CreateInstance(SystemResourceKeyID.IsPenWindows);
				}
				return SystemParameters._cacheIsPenWindows;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060027ED RID: 10221 RVA: 0x00190AF6 File Offset: 0x0018FAF6
		public static ResourceKey IsRemotelyControlledKey
		{
			get
			{
				if (SystemParameters._cacheIsRemotelyControlled == null)
				{
					SystemParameters._cacheIsRemotelyControlled = SystemParameters.CreateInstance(SystemResourceKeyID.IsRemotelyControlled);
				}
				return SystemParameters._cacheIsRemotelyControlled;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060027EE RID: 10222 RVA: 0x00190B13 File Offset: 0x0018FB13
		public static ResourceKey IsRemoteSessionKey
		{
			get
			{
				if (SystemParameters._cacheIsRemoteSession == null)
				{
					SystemParameters._cacheIsRemoteSession = SystemParameters.CreateInstance(SystemResourceKeyID.IsRemoteSession);
				}
				return SystemParameters._cacheIsRemoteSession;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060027EF RID: 10223 RVA: 0x00190B30 File Offset: 0x0018FB30
		public static ResourceKey ShowSoundsKey
		{
			get
			{
				if (SystemParameters._cacheShowSounds == null)
				{
					SystemParameters._cacheShowSounds = SystemParameters.CreateInstance(SystemResourceKeyID.ShowSounds);
				}
				return SystemParameters._cacheShowSounds;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060027F0 RID: 10224 RVA: 0x00190B4D File Offset: 0x0018FB4D
		public static ResourceKey IsSlowMachineKey
		{
			get
			{
				if (SystemParameters._cacheIsSlowMachine == null)
				{
					SystemParameters._cacheIsSlowMachine = SystemParameters.CreateInstance(SystemResourceKeyID.IsSlowMachine);
				}
				return SystemParameters._cacheIsSlowMachine;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060027F1 RID: 10225 RVA: 0x00190B6A File Offset: 0x0018FB6A
		public static ResourceKey SwapButtonsKey
		{
			get
			{
				if (SystemParameters._cacheSwapButtons == null)
				{
					SystemParameters._cacheSwapButtons = SystemParameters.CreateInstance(SystemResourceKeyID.SwapButtons);
				}
				return SystemParameters._cacheSwapButtons;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060027F2 RID: 10226 RVA: 0x00190B87 File Offset: 0x0018FB87
		public static ResourceKey IsTabletPCKey
		{
			get
			{
				if (SystemParameters._cacheIsTabletPC == null)
				{
					SystemParameters._cacheIsTabletPC = SystemParameters.CreateInstance(SystemResourceKeyID.IsTabletPC);
				}
				return SystemParameters._cacheIsTabletPC;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060027F3 RID: 10227 RVA: 0x00190BA4 File Offset: 0x0018FBA4
		public static ResourceKey VirtualScreenLeftKey
		{
			get
			{
				if (SystemParameters._cacheVirtualScreenLeft == null)
				{
					SystemParameters._cacheVirtualScreenLeft = SystemParameters.CreateInstance(SystemResourceKeyID.VirtualScreenLeft);
				}
				return SystemParameters._cacheVirtualScreenLeft;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x00190BC1 File Offset: 0x0018FBC1
		public static ResourceKey VirtualScreenTopKey
		{
			get
			{
				if (SystemParameters._cacheVirtualScreenTop == null)
				{
					SystemParameters._cacheVirtualScreenTop = SystemParameters.CreateInstance(SystemResourceKeyID.VirtualScreenTop);
				}
				return SystemParameters._cacheVirtualScreenTop;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060027F5 RID: 10229 RVA: 0x00190BDE File Offset: 0x0018FBDE
		public static ResourceKey FocusVisualStyleKey
		{
			get
			{
				if (SystemParameters._cacheFocusVisualStyle == null)
				{
					SystemParameters._cacheFocusVisualStyle = new SystemThemeKey(SystemResourceKeyID.FocusVisualStyle);
				}
				return SystemParameters._cacheFocusVisualStyle;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x00190BFB File Offset: 0x0018FBFB
		public static ResourceKey NavigationChromeStyleKey
		{
			get
			{
				if (SystemParameters._cacheNavigationChromeStyle == null)
				{
					SystemParameters._cacheNavigationChromeStyle = new SystemThemeKey(SystemResourceKeyID.NavigationChromeStyle);
				}
				return SystemParameters._cacheNavigationChromeStyle;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060027F7 RID: 10231 RVA: 0x00190C18 File Offset: 0x0018FC18
		public static ResourceKey NavigationChromeDownLevelStyleKey
		{
			get
			{
				if (SystemParameters._cacheNavigationChromeDownLevelStyle == null)
				{
					SystemParameters._cacheNavigationChromeDownLevelStyle = new SystemThemeKey(SystemResourceKeyID.NavigationChromeDownLevelStyle);
				}
				return SystemParameters._cacheNavigationChromeDownLevelStyle;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x00190C38 File Offset: 0x0018FC38
		public static PowerLineStatus PowerLineStatus
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[110])
					{
						SystemParameters._cacheValid[110] = true;
						NativeMethods.SYSTEM_POWER_STATUS system_POWER_STATUS = default(NativeMethods.SYSTEM_POWER_STATUS);
						if (!UnsafeNativeMethods.GetSystemPowerStatus(ref system_POWER_STATUS))
						{
							SystemParameters._cacheValid[110] = false;
							throw new Win32Exception();
						}
						SystemParameters._powerLineStatus = (PowerLineStatus)system_POWER_STATUS.ACLineStatus;
					}
				}
				return SystemParameters._powerLineStatus;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060027F9 RID: 10233 RVA: 0x00190CC4 File Offset: 0x0018FCC4
		public static ResourceKey PowerLineStatusKey
		{
			get
			{
				if (SystemParameters._cachePowerLineStatus == null)
				{
					SystemParameters._cachePowerLineStatus = SystemParameters.CreateInstance(SystemResourceKeyID.PowerLineStatus);
				}
				return SystemParameters._cachePowerLineStatus;
			}
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x00190CE4 File Offset: 0x0018FCE4
		internal static void InvalidateCache()
		{
			int[] array = new int[]
			{
				8207,
				8209,
				67,
				4129,
				4133,
				4131,
				47,
				46,
				4107,
				23,
				69,
				11,
				96,
				105,
				103,
				101,
				99,
				28,
				4115,
				107,
				4101,
				4163,
				4123,
				4105,
				4111,
				4103,
				4099,
				4117,
				4113,
				4119,
				4121,
				4159,
				73,
				8199,
				8197,
				37,
				6,
				42,
				76,
				77,
				49,
				57,
				33
			};
			for (int i = 0; i < array.Length; i++)
			{
				SystemParameters.InvalidateCache(array[i]);
			}
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x00190D1C File Offset: 0x0018FD1C
		internal static bool InvalidateDeviceDependentCache()
		{
			bool flag = false;
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 99))
			{
				flag |= (SystemParameters._isMousePresent != SystemParameters.IsMousePresent);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 100))
			{
				flag |= (SystemParameters._isMouseWheelPresent != SystemParameters.IsMouseWheelPresent);
			}
			if (flag)
			{
				SystemParameters.OnPropertiesChanged(new string[]
				{
					"IsMousePresent",
					"IsMouseWheelPresent"
				});
			}
			return flag;
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x00190D8C File Offset: 0x0018FD8C
		internal static bool InvalidateDisplayDependentCache()
		{
			bool flag = false;
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 7))
			{
				NativeMethods.RECT workAreaInternal = SystemParameters._workAreaInternal;
				NativeMethods.RECT workAreaInternal2 = SystemParameters.WorkAreaInternal;
				flag |= (workAreaInternal.left != workAreaInternal2.left);
				flag |= (workAreaInternal.top != workAreaInternal2.top);
				flag |= (workAreaInternal.right != workAreaInternal2.right);
				flag |= (workAreaInternal.bottom != workAreaInternal2.bottom);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 8))
			{
				flag |= (SystemParameters._workArea != SystemParameters.WorkArea);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 52))
			{
				flag |= (SystemParameters._fullPrimaryScreenWidth != SystemParameters.FullPrimaryScreenWidth);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 53))
			{
				flag |= (SystemParameters._fullPrimaryScreenHeight != SystemParameters.FullPrimaryScreenHeight);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 61))
			{
				flag |= (SystemParameters._maximizedPrimaryScreenWidth != SystemParameters.MaximizedPrimaryScreenWidth);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 62))
			{
				flag |= (SystemParameters._maximizedPrimaryScreenHeight != SystemParameters.MaximizedPrimaryScreenHeight);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 77))
			{
				flag |= (SystemParameters._primaryScreenWidth != SystemParameters.PrimaryScreenWidth);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 78))
			{
				flag |= (SystemParameters._primaryScreenHeight != SystemParameters.PrimaryScreenHeight);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 87))
			{
				flag |= (SystemParameters._virtualScreenWidth != SystemParameters.VirtualScreenWidth);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 88))
			{
				flag |= (SystemParameters._virtualScreenHeight != SystemParameters.VirtualScreenHeight);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 108))
			{
				flag |= (SystemParameters._virtualScreenLeft != SystemParameters.VirtualScreenLeft);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 109))
			{
				flag |= (SystemParameters._virtualScreenTop != SystemParameters.VirtualScreenTop);
			}
			if (flag)
			{
				SystemParameters.OnPropertiesChanged(new string[]
				{
					"WorkArea",
					"FullPrimaryScreenWidth",
					"FullPrimaryScreenHeight",
					"MaximizedPrimaryScreenWidth",
					"MaximizedPrimaryScreenHeight",
					"PrimaryScreenWidth",
					"PrimaryScreenHeight",
					"VirtualScreenWidth",
					"VirtualScreenHeight",
					"VirtualScreenLeft",
					"VirtualScreenTop"
				});
			}
			return flag;
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x00190FD0 File Offset: 0x0018FFD0
		internal static bool InvalidatePowerDependentCache()
		{
			bool flag = false;
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 110))
			{
				flag |= (SystemParameters._powerLineStatus != SystemParameters.PowerLineStatus);
			}
			if (flag)
			{
				SystemParameters.OnPropertiesChanged(new string[]
				{
					"PowerLineStatus"
				});
			}
			return flag;
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x00191018 File Offset: 0x00190018
		internal static bool InvalidateCache(int param)
		{
			if (param <= 69)
			{
				if (param <= 33)
				{
					if (param <= 11)
					{
						if (param != 6)
						{
							if (param != 11)
							{
								return false;
							}
							return SystemParameters.InvalidateProperty(13, "KeyboardSpeed");
						}
					}
					else
					{
						if (param == 23)
						{
							return SystemParameters.InvalidateProperty(11, "KeyboardDelay");
						}
						if (param == 28)
						{
							bool flag = SystemResources.ClearSlot(SystemParameters._cacheValid, 19);
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 97))
							{
								flag |= (SystemParameters._isMenuDropRightAligned != SystemParameters.IsMenuDropRightAligned);
							}
							if (flag)
							{
								SystemParameters.OnPropertiesChanged(new string[]
								{
									"MenuDropAlignment",
									"IsMenuDropRightAligned"
								});
							}
							return flag;
						}
						if (param != 33)
						{
							return false;
						}
						return SystemParameters.InvalidateProperty(106, "SwapButtons");
					}
				}
				else if (param <= 49)
				{
					if (param == 37)
					{
						return SystemParameters.InvalidateProperty(38, "DragFullWindows");
					}
					if (param != 42)
					{
						switch (param)
						{
						case 46:
						{
							bool flag2 = SystemResources.ClearSlot(SystemParameters._cacheValid, 9);
							if (flag2)
							{
								SystemFonts.InvalidateIconMetrics();
							}
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 57))
							{
								flag2 |= (SystemParameters._iconWidth != SystemParameters.IconWidth);
							}
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 58))
							{
								flag2 |= (SystemParameters._iconHeight != SystemParameters.IconHeight);
							}
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 59))
							{
								flag2 |= (SystemParameters._iconGridWidth != SystemParameters.IconGridWidth);
							}
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 60))
							{
								flag2 |= (SystemParameters._iconGridHeight != SystemParameters.IconGridHeight);
							}
							if (flag2)
							{
								SystemParameters.OnPropertiesChanged(new string[]
								{
									"IconMetrics",
									"IconWidth",
									"IconHeight",
									"IconGridWidth",
									"IconGridHeight"
								});
							}
							return flag2;
						}
						case 47:
							return SystemParameters.InvalidateDisplayDependentCache();
						case 48:
							return false;
						case 49:
							return SystemParameters.InvalidateProperty(101, "IsPenWindows");
						default:
							return false;
						}
					}
				}
				else
				{
					if (param == 57)
					{
						return SystemParameters.InvalidateProperty(104, "ShowSounds");
					}
					if (param == 67)
					{
						return SystemParameters.InvalidateProperty(3, "HighContrast");
					}
					if (param != 69)
					{
						return false;
					}
					return SystemParameters.InvalidateProperty(12, "KeyboardPreference");
				}
				bool flag3 = SystemResources.ClearSlot(SystemParameters._cacheValid, 39);
				if (flag3)
				{
					SystemFonts.InvalidateNonClientMetrics();
				}
				flag3 |= SystemResources.ClearSlot(SystemParameters._cacheValid, 35);
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 40))
				{
					flag3 |= (SystemParameters._thinHorizontalBorderHeight != SystemParameters.ThinHorizontalBorderHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 41))
				{
					flag3 |= (SystemParameters._thinVerticalBorderWidth != SystemParameters.ThinVerticalBorderWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 42))
				{
					flag3 |= (SystemParameters._cursorWidth != SystemParameters.CursorWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 43))
				{
					flag3 |= (SystemParameters._cursorHeight != SystemParameters.CursorHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 44))
				{
					flag3 |= (SystemParameters._thickHorizontalBorderHeight != SystemParameters.ThickHorizontalBorderHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 45))
				{
					flag3 |= (SystemParameters._thickVerticalBorderWidth != SystemParameters.ThickVerticalBorderWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 48))
				{
					flag3 |= (SystemParameters._fixedFrameHorizontalBorderHeight != SystemParameters.FixedFrameHorizontalBorderHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 49))
				{
					flag3 |= (SystemParameters._fixedFrameVerticalBorderWidth != SystemParameters.FixedFrameVerticalBorderWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 54))
				{
					flag3 |= (SystemParameters._horizontalScrollBarButtonWidth != SystemParameters.HorizontalScrollBarButtonWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 55))
				{
					flag3 |= (SystemParameters._horizontalScrollBarHeight != SystemParameters.HorizontalScrollBarHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 56))
				{
					flag3 |= (SystemParameters._horizontalScrollBarThumbWidth != SystemParameters.HorizontalScrollBarThumbWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 57))
				{
					flag3 |= (SystemParameters._iconWidth != SystemParameters.IconWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 58))
				{
					flag3 |= (SystemParameters._iconHeight != SystemParameters.IconHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 59))
				{
					flag3 |= (SystemParameters._iconGridWidth != SystemParameters.IconGridWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 60))
				{
					flag3 |= (SystemParameters._iconGridHeight != SystemParameters.IconGridHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 63))
				{
					flag3 |= (SystemParameters._maximumWindowTrackWidth != SystemParameters.MaximumWindowTrackWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 64))
				{
					flag3 |= (SystemParameters._maximumWindowTrackHeight != SystemParameters.MaximumWindowTrackHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 65))
				{
					flag3 |= (SystemParameters._menuCheckmarkWidth != SystemParameters.MenuCheckmarkWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 66))
				{
					flag3 |= (SystemParameters._menuCheckmarkHeight != SystemParameters.MenuCheckmarkHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 67))
				{
					flag3 |= (SystemParameters._menuButtonWidth != SystemParameters.MenuButtonWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 68))
				{
					flag3 |= (SystemParameters._menuButtonHeight != SystemParameters.MenuButtonHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 69))
				{
					flag3 |= (SystemParameters._minimumWindowWidth != SystemParameters.MinimumWindowWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 70))
				{
					flag3 |= (SystemParameters._minimumWindowHeight != SystemParameters.MinimumWindowHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 71))
				{
					flag3 |= (SystemParameters._minimizedWindowWidth != SystemParameters.MinimizedWindowWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 72))
				{
					flag3 |= (SystemParameters._minimizedWindowHeight != SystemParameters.MinimizedWindowHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 73))
				{
					flag3 |= (SystemParameters._minimizedGridWidth != SystemParameters.MinimizedGridWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 74))
				{
					flag3 |= (SystemParameters._minimizedGridHeight != SystemParameters.MinimizedGridHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 75))
				{
					flag3 |= (SystemParameters._minimumWindowTrackWidth != SystemParameters.MinimumWindowTrackWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 76))
				{
					flag3 |= (SystemParameters._minimumWindowTrackHeight != SystemParameters.MinimumWindowTrackHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 79))
				{
					flag3 |= (SystemParameters._windowCaptionButtonWidth != SystemParameters.WindowCaptionButtonWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 80))
				{
					flag3 |= (SystemParameters._windowCaptionButtonHeight != SystemParameters.WindowCaptionButtonHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 81))
				{
					flag3 |= (SystemParameters._resizeFrameHorizontalBorderHeight != SystemParameters.ResizeFrameHorizontalBorderHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 82))
				{
					flag3 |= (SystemParameters._resizeFrameVerticalBorderWidth != SystemParameters.ResizeFrameVerticalBorderWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 83))
				{
					flag3 |= (SystemParameters._smallIconWidth != SystemParameters.SmallIconWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 84))
				{
					flag3 |= (SystemParameters._smallIconHeight != SystemParameters.SmallIconHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 85))
				{
					flag3 |= (SystemParameters._smallWindowCaptionButtonWidth != SystemParameters.SmallWindowCaptionButtonWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 86))
				{
					flag3 |= (SystemParameters._smallWindowCaptionButtonHeight != SystemParameters.SmallWindowCaptionButtonHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 89))
				{
					flag3 |= (SystemParameters._verticalScrollBarWidth != SystemParameters.VerticalScrollBarWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 90))
				{
					flag3 |= (SystemParameters._verticalScrollBarButtonHeight != SystemParameters.VerticalScrollBarButtonHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 91))
				{
					flag3 |= (SystemParameters._windowCaptionHeight != SystemParameters.WindowCaptionHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 93))
				{
					flag3 |= (SystemParameters._menuBarHeight != SystemParameters.MenuBarHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 94))
				{
					flag3 |= (SystemParameters._verticalScrollBarThumbHeight != SystemParameters.VerticalScrollBarThumbHeight);
				}
				if (flag3)
				{
					SystemParameters.OnPropertiesChanged(new string[]
					{
						"NonClientMetrics",
						"Border",
						"ThinHorizontalBorderHeight",
						"ThinVerticalBorderWidth",
						"CursorWidth",
						"CursorHeight",
						"ThickHorizontalBorderHeight",
						"ThickVerticalBorderWidth",
						"FixedFrameHorizontalBorderHeight",
						"FixedFrameVerticalBorderWidth",
						"HorizontalScrollBarButtonWidth",
						"HorizontalScrollBarHeight",
						"HorizontalScrollBarThumbWidth",
						"IconWidth",
						"IconHeight",
						"IconGridWidth",
						"IconGridHeight",
						"MaximumWindowTrackWidth",
						"MaximumWindowTrackHeight",
						"MenuCheckmarkWidth",
						"MenuCheckmarkHeight",
						"MenuButtonWidth",
						"MenuButtonHeight",
						"MinimumWindowWidth",
						"MinimumWindowHeight",
						"MinimizedWindowWidth",
						"MinimizedWindowHeight",
						"MinimizedGridWidth",
						"MinimizedGridHeight",
						"MinimumWindowTrackWidth",
						"MinimumWindowTrackHeight",
						"WindowCaptionButtonWidth",
						"WindowCaptionButtonHeight",
						"ResizeFrameHorizontalBorderHeight",
						"ResizeFrameVerticalBorderWidth",
						"SmallIconWidth",
						"SmallIconHeight",
						"SmallWindowCaptionButtonWidth",
						"SmallWindowCaptionButtonHeight",
						"VerticalScrollBarWidth",
						"VerticalScrollBarButtonHeight",
						"MenuBarHeight",
						"VerticalScrollBarThumbHeight"
					});
				}
				return flag3 | SystemParameters.InvalidateDisplayDependentCache();
			}
			if (param <= 4133)
			{
				if (param <= 107)
				{
					switch (param)
					{
					case 73:
						return SystemParameters.InvalidateProperty(34, "MinimizeAnimation");
					case 74:
					case 75:
						break;
					case 76:
						return SystemParameters.InvalidateProperty(46, "MinimumHorizontalDragDistance");
					case 77:
						return SystemParameters.InvalidateProperty(47, "MinimumVerticalDragDistance");
					default:
						if (param == 96)
						{
							return SystemParameters.InvalidateProperty(14, "SnapToDefaultButton");
						}
						switch (param)
						{
						case 99:
							return SystemParameters.InvalidateProperty(18, "MouseHoverWidth");
						case 101:
							return SystemParameters.InvalidateProperty(17, "MouseHoverHeight");
						case 103:
							return SystemParameters.InvalidateProperty(16, "MouseHoverTime");
						case 105:
							return SystemParameters.InvalidateProperty(15, "WheelScrollLines");
						case 107:
							return SystemParameters.InvalidateProperty(21, "MenuShowDelay");
						}
						break;
					}
				}
				else
				{
					switch (param)
					{
					case 4099:
						return SystemParameters.InvalidateProperty(28, "MenuAnimation");
					case 4100:
					case 4102:
					case 4104:
					case 4106:
						break;
					case 4101:
						return SystemParameters.InvalidateProperty(22, "ComboBoxAnimation");
					case 4103:
						return SystemParameters.InvalidateProperty(27, "ListBoxSmoothScrolling");
					case 4105:
						return SystemParameters.InvalidateProperty(25, "GradientCaptions");
					case 4107:
						return SystemParameters.InvalidateProperty(10, "KeyboardCues");
					default:
						switch (param)
						{
						case 4111:
							return SystemParameters.InvalidateProperty(26, "HotTracking");
						case 4112:
						case 4114:
						case 4116:
						case 4118:
						case 4120:
						case 4122:
							break;
						case 4113:
							return SystemParameters.InvalidateProperty(30, "StylusHotTracking");
						case 4115:
							return SystemParameters.InvalidateProperty(20, "MenuFade");
						case 4117:
							return SystemParameters.InvalidateProperty(29, "SelectionFade");
						case 4119:
							return SystemParameters.InvalidateProperty(31, "ToolTipAnimation");
						case 4121:
							return SystemParameters.InvalidateProperty(32, "ToolTipFade");
						case 4123:
							return SystemParameters.InvalidateProperty(24, "CursorShadow");
						default:
							switch (param)
							{
							case 4129:
								return SystemParameters.InvalidateProperty(4, "MouseVanish");
							case 4131:
								return SystemParameters.InvalidateProperty(6, "FlatMenu");
							case 4133:
								return SystemParameters.InvalidateProperty(5, "DropShadow");
							}
							break;
						}
						break;
					}
				}
			}
			else if (param <= 8197)
			{
				if (param == 4159)
				{
					return SystemParameters.InvalidateProperty(33, "UIEffects");
				}
				if (param == 4163)
				{
					return SystemParameters.InvalidateProperty(23, "ClientAreaAnimation");
				}
				if (param == 8197)
				{
					return SystemParameters.InvalidateProperty(37, "ForegroundFlashCount");
				}
			}
			else
			{
				if (param == 8199)
				{
					return SystemParameters.InvalidateProperty(36, "CaretWidth");
				}
				if (param == 8207)
				{
					bool flag4 = SystemResources.ClearSlot(SystemParameters._cacheValid, 1);
					if (SystemResources.ClearSlot(SystemParameters._cacheValid, 50))
					{
						flag4 |= (SystemParameters._focusHorizontalBorderHeight != SystemParameters.FocusHorizontalBorderHeight);
					}
					if (SystemResources.ClearSlot(SystemParameters._cacheValid, 51))
					{
						flag4 |= (SystemParameters._focusVerticalBorderWidth != SystemParameters.FocusVerticalBorderWidth);
					}
					if (flag4)
					{
						SystemParameters.OnPropertiesChanged(new string[]
						{
							"FocusBorderWidth",
							"FocusHorizontalBorderHeight",
							"FocusVerticalBorderWidth"
						});
					}
					return flag4;
				}
				if (param == 8209)
				{
					return SystemParameters.InvalidateProperty(2, "FocusBorderHeight");
				}
			}
			return false;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x00191C6D File Offset: 0x00190C6D
		internal static bool InvalidateIsGlassEnabled()
		{
			return SystemParameters.InvalidateProperty(111, "IsGlassEnabled");
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x00191C7B File Offset: 0x00190C7B
		internal static void InvalidateDerivedThemeRelatedProperties()
		{
			SystemParameters.InvalidateProperty(112, "UxThemeName");
			SystemParameters.InvalidateProperty(113, "UxThemeColor");
			SystemParameters.InvalidateProperty(114, "WindowCornerRadius");
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x00191CA4 File Offset: 0x00190CA4
		internal static void InvalidateWindowGlassColorizationProperties()
		{
			SystemParameters.InvalidateProperty(115, "WindowGlassColor");
			SystemParameters.InvalidateProperty(116, "WindowGlassBrush");
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x00191CC0 File Offset: 0x00190CC0
		internal static void InvalidateWindowFrameThicknessProperties()
		{
			SystemParameters.InvalidateProperty(117, "WindowNonClientFrameThickness");
			SystemParameters.InvalidateProperty(118, "WindowResizeBorderThickness");
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06002803 RID: 10243 RVA: 0x00191CDC File Offset: 0x00190CDC
		public static bool IsGlassEnabled
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[111])
					{
						SystemParameters._cacheValid[111] = true;
						SystemParameters._isGlassEnabled = NativeMethods.DwmIsCompositionEnabled();
					}
				}
				return SystemParameters._isGlassEnabled;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06002804 RID: 10244 RVA: 0x00191D44 File Offset: 0x00190D44
		public static string UxThemeName
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[112])
					{
						SystemParameters._cacheValid[112] = true;
						if (!NativeMethods.IsThemeActive())
						{
							SystemParameters._uxThemeName = "Classic";
						}
						else
						{
							string path;
							string text;
							string text2;
							NativeMethods.GetCurrentThemeName(out path, out text, out text2);
							SystemParameters._uxThemeName = Path.GetFileNameWithoutExtension(path);
						}
					}
				}
				return SystemParameters._uxThemeName;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06002805 RID: 10245 RVA: 0x00191DC8 File Offset: 0x00190DC8
		public static string UxThemeColor
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[113])
					{
						SystemParameters._cacheValid[113] = true;
						if (!NativeMethods.IsThemeActive())
						{
							SystemParameters._uxThemeColor = "";
						}
						else
						{
							string text;
							string uxThemeColor;
							string text2;
							NativeMethods.GetCurrentThemeName(out text, out uxThemeColor, out text2);
							SystemParameters._uxThemeColor = uxThemeColor;
						}
					}
				}
				return SystemParameters._uxThemeColor;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06002806 RID: 10246 RVA: 0x00191E48 File Offset: 0x00190E48
		public static CornerRadius WindowCornerRadius
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[114])
					{
						SystemParameters._cacheValid[114] = true;
						CornerRadius windowCornerRadius = default(CornerRadius);
						string a = SystemParameters.UxThemeName.ToUpperInvariant();
						if (!(a == "LUNA"))
						{
							if (!(a == "AERO"))
							{
								if (!(a == "CLASSIC") && !(a == "ZUNE") && !(a == "ROYALE"))
								{
								}
								windowCornerRadius = new CornerRadius(0.0);
							}
							else if (NativeMethods.DwmIsCompositionEnabled())
							{
								windowCornerRadius = new CornerRadius(8.0);
							}
							else
							{
								windowCornerRadius = new CornerRadius(6.0, 6.0, 0.0, 0.0);
							}
						}
						else
						{
							windowCornerRadius = new CornerRadius(6.0, 6.0, 0.0, 0.0);
						}
						SystemParameters._windowCornerRadius = windowCornerRadius;
					}
				}
				return SystemParameters._windowCornerRadius;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06002807 RID: 10247 RVA: 0x00191F9C File Offset: 0x00190F9C
		public static Color WindowGlassColor
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[115])
					{
						SystemParameters._cacheValid[115] = true;
						uint num;
						bool flag2;
						NativeMethods.DwmGetColorizationColor(out num, out flag2);
						num |= (flag2 ? 4278190080U : 0U);
						SystemParameters._windowGlassColor = Utility.ColorFromArgbDword(num);
					}
				}
				return SystemParameters._windowGlassColor;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x0019201C File Offset: 0x0019101C
		public static Brush WindowGlassBrush
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[116])
					{
						SystemParameters._cacheValid[116] = true;
						SolidColorBrush solidColorBrush = new SolidColorBrush(SystemParameters.WindowGlassColor);
						solidColorBrush.Freeze();
						SystemParameters._windowGlassBrush = solidColorBrush;
					}
				}
				return SystemParameters._windowGlassBrush;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06002809 RID: 10249 RVA: 0x0019208C File Offset: 0x0019108C
		public static Thickness WindowResizeBorderThickness
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[118])
					{
						SystemParameters._cacheValid[118] = true;
						Size size = DpiHelper.DeviceSizeToLogical(new Size((double)NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)NativeMethods.GetSystemMetrics(SM.CYFRAME)), (double)SystemParameters.DpiX / 96.0, (double)SystemParameters.Dpi / 96.0);
						SystemParameters._windowResizeBorderThickness = new Thickness(size.Width, size.Height, size.Width, size.Height);
					}
				}
				return SystemParameters._windowResizeBorderThickness;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x00192148 File Offset: 0x00191148
		public static Thickness WindowNonClientFrameThickness
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[117])
					{
						SystemParameters._cacheValid[117] = true;
						Size size = DpiHelper.DeviceSizeToLogical(new Size((double)NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)NativeMethods.GetSystemMetrics(SM.CYFRAME)), (double)SystemParameters.DpiX / 96.0, (double)SystemParameters.Dpi / 96.0);
						int systemMetrics = NativeMethods.GetSystemMetrics(SM.CYCAPTION);
						double y = DpiHelper.DevicePixelsToLogical(new Point(0.0, (double)systemMetrics), (double)SystemParameters.DpiX / 96.0, (double)SystemParameters.Dpi / 96.0).Y;
						SystemParameters._windowNonClientFrameThickness = new Thickness(size.Width, size.Height + y, size.Width, size.Height);
					}
				}
				return SystemParameters._windowNonClientFrameThickness;
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x0600280B RID: 10251 RVA: 0x00192254 File Offset: 0x00191254
		internal static int Dpi
		{
			get
			{
				return Util.Dpi;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x0019225C File Offset: 0x0019125C
		internal static int DpiX
		{
			get
			{
				if (SystemParameters._setDpiX)
				{
					BitArray cacheValid = SystemParameters._cacheValid;
					lock (cacheValid)
					{
						if (SystemParameters._setDpiX)
						{
							SystemParameters._setDpiX = false;
							HandleRef hWnd = new HandleRef(null, IntPtr.Zero);
							IntPtr dc = UnsafeNativeMethods.GetDC(hWnd);
							if (dc == IntPtr.Zero)
							{
								throw new Win32Exception();
							}
							try
							{
								SystemParameters._dpiX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
								SystemParameters._cacheValid[0] = true;
							}
							finally
							{
								UnsafeNativeMethods.ReleaseDC(hWnd, new HandleRef(null, dc));
							}
						}
					}
				}
				return SystemParameters._dpiX;
			}
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x00192318 File Offset: 0x00191318
		internal static double ConvertPixel(int pixel)
		{
			int dpi = SystemParameters.Dpi;
			if (dpi != 0)
			{
				return (double)pixel * 96.0 / (double)dpi;
			}
			return (double)pixel;
		}

		// Token: 0x04001271 RID: 4721
		private static BitArray _cacheValid = new BitArray(119);

		// Token: 0x04001272 RID: 4722
		private static bool _isGlassEnabled;

		// Token: 0x04001273 RID: 4723
		private static string _uxThemeName;

		// Token: 0x04001274 RID: 4724
		private static string _uxThemeColor;

		// Token: 0x04001275 RID: 4725
		private static CornerRadius _windowCornerRadius;

		// Token: 0x04001276 RID: 4726
		private static Color _windowGlassColor;

		// Token: 0x04001277 RID: 4727
		private static Brush _windowGlassBrush;

		// Token: 0x04001278 RID: 4728
		private static Thickness _windowNonClientFrameThickness;

		// Token: 0x04001279 RID: 4729
		private static Thickness _windowResizeBorderThickness;

		// Token: 0x0400127A RID: 4730
		private static int _dpiX;

		// Token: 0x0400127B RID: 4731
		private static bool _setDpiX = true;

		// Token: 0x0400127C RID: 4732
		private static double _focusBorderWidth;

		// Token: 0x0400127D RID: 4733
		private static double _focusBorderHeight;

		// Token: 0x0400127E RID: 4734
		private static bool _highContrast;

		// Token: 0x0400127F RID: 4735
		private static bool _mouseVanish;

		// Token: 0x04001280 RID: 4736
		private static bool _dropShadow;

		// Token: 0x04001281 RID: 4737
		private static bool _flatMenu;

		// Token: 0x04001282 RID: 4738
		private static NativeMethods.RECT _workAreaInternal;

		// Token: 0x04001283 RID: 4739
		private static Rect _workArea;

		// Token: 0x04001284 RID: 4740
		private static NativeMethods.ICONMETRICS _iconMetrics;

		// Token: 0x04001285 RID: 4741
		private static bool _keyboardCues;

		// Token: 0x04001286 RID: 4742
		private static int _keyboardDelay;

		// Token: 0x04001287 RID: 4743
		private static bool _keyboardPref;

		// Token: 0x04001288 RID: 4744
		private static int _keyboardSpeed;

		// Token: 0x04001289 RID: 4745
		private static bool _snapToDefButton;

		// Token: 0x0400128A RID: 4746
		private static int _wheelScrollLines;

		// Token: 0x0400128B RID: 4747
		private static int _mouseHoverTime;

		// Token: 0x0400128C RID: 4748
		private static double _mouseHoverHeight;

		// Token: 0x0400128D RID: 4749
		private static double _mouseHoverWidth;

		// Token: 0x0400128E RID: 4750
		private static bool _menuDropAlignment;

		// Token: 0x0400128F RID: 4751
		private static bool _menuFade;

		// Token: 0x04001290 RID: 4752
		private static int _menuShowDelay;

		// Token: 0x04001291 RID: 4753
		private static bool _comboBoxAnimation;

		// Token: 0x04001292 RID: 4754
		private static bool _clientAreaAnimation;

		// Token: 0x04001293 RID: 4755
		private static bool _cursorShadow;

		// Token: 0x04001294 RID: 4756
		private static bool _gradientCaptions;

		// Token: 0x04001295 RID: 4757
		private static bool _hotTracking;

		// Token: 0x04001296 RID: 4758
		private static bool _listBoxSmoothScrolling;

		// Token: 0x04001297 RID: 4759
		private static bool _menuAnimation;

		// Token: 0x04001298 RID: 4760
		private static bool _selectionFade;

		// Token: 0x04001299 RID: 4761
		private static bool _stylusHotTracking;

		// Token: 0x0400129A RID: 4762
		private static bool _toolTipAnimation;

		// Token: 0x0400129B RID: 4763
		private static bool _tooltipFade;

		// Token: 0x0400129C RID: 4764
		private static bool _uiEffects;

		// Token: 0x0400129D RID: 4765
		private static bool _minAnimation;

		// Token: 0x0400129E RID: 4766
		private static int _border;

		// Token: 0x0400129F RID: 4767
		private static double _caretWidth;

		// Token: 0x040012A0 RID: 4768
		private static bool _dragFullWindows;

		// Token: 0x040012A1 RID: 4769
		private static int _foregroundFlashCount;

		// Token: 0x040012A2 RID: 4770
		private static NativeMethods.NONCLIENTMETRICS _ncm;

		// Token: 0x040012A3 RID: 4771
		private static double _thinHorizontalBorderHeight;

		// Token: 0x040012A4 RID: 4772
		private static double _thinVerticalBorderWidth;

		// Token: 0x040012A5 RID: 4773
		private static double _cursorWidth;

		// Token: 0x040012A6 RID: 4774
		private static double _cursorHeight;

		// Token: 0x040012A7 RID: 4775
		private static double _thickHorizontalBorderHeight;

		// Token: 0x040012A8 RID: 4776
		private static double _thickVerticalBorderWidth;

		// Token: 0x040012A9 RID: 4777
		private static double _minimumHorizontalDragDistance;

		// Token: 0x040012AA RID: 4778
		private static double _minimumVerticalDragDistance;

		// Token: 0x040012AB RID: 4779
		private static double _fixedFrameHorizontalBorderHeight;

		// Token: 0x040012AC RID: 4780
		private static double _fixedFrameVerticalBorderWidth;

		// Token: 0x040012AD RID: 4781
		private static double _focusHorizontalBorderHeight;

		// Token: 0x040012AE RID: 4782
		private static double _focusVerticalBorderWidth;

		// Token: 0x040012AF RID: 4783
		private static double _fullPrimaryScreenHeight;

		// Token: 0x040012B0 RID: 4784
		private static double _fullPrimaryScreenWidth;

		// Token: 0x040012B1 RID: 4785
		private static double _horizontalScrollBarHeight;

		// Token: 0x040012B2 RID: 4786
		private static double _horizontalScrollBarButtonWidth;

		// Token: 0x040012B3 RID: 4787
		private static double _horizontalScrollBarThumbWidth;

		// Token: 0x040012B4 RID: 4788
		private static double _iconWidth;

		// Token: 0x040012B5 RID: 4789
		private static double _iconHeight;

		// Token: 0x040012B6 RID: 4790
		private static double _iconGridWidth;

		// Token: 0x040012B7 RID: 4791
		private static double _iconGridHeight;

		// Token: 0x040012B8 RID: 4792
		private static double _maximizedPrimaryScreenWidth;

		// Token: 0x040012B9 RID: 4793
		private static double _maximizedPrimaryScreenHeight;

		// Token: 0x040012BA RID: 4794
		private static double _maximumWindowTrackWidth;

		// Token: 0x040012BB RID: 4795
		private static double _maximumWindowTrackHeight;

		// Token: 0x040012BC RID: 4796
		private static double _menuCheckmarkWidth;

		// Token: 0x040012BD RID: 4797
		private static double _menuCheckmarkHeight;

		// Token: 0x040012BE RID: 4798
		private static double _menuButtonWidth;

		// Token: 0x040012BF RID: 4799
		private static double _menuButtonHeight;

		// Token: 0x040012C0 RID: 4800
		private static double _minimumWindowWidth;

		// Token: 0x040012C1 RID: 4801
		private static double _minimumWindowHeight;

		// Token: 0x040012C2 RID: 4802
		private static double _minimizedWindowWidth;

		// Token: 0x040012C3 RID: 4803
		private static double _minimizedWindowHeight;

		// Token: 0x040012C4 RID: 4804
		private static double _minimizedGridWidth;

		// Token: 0x040012C5 RID: 4805
		private static double _minimizedGridHeight;

		// Token: 0x040012C6 RID: 4806
		private static double _minimumWindowTrackWidth;

		// Token: 0x040012C7 RID: 4807
		private static double _minimumWindowTrackHeight;

		// Token: 0x040012C8 RID: 4808
		private static double _primaryScreenWidth;

		// Token: 0x040012C9 RID: 4809
		private static double _primaryScreenHeight;

		// Token: 0x040012CA RID: 4810
		private static double _windowCaptionButtonWidth;

		// Token: 0x040012CB RID: 4811
		private static double _windowCaptionButtonHeight;

		// Token: 0x040012CC RID: 4812
		private static double _resizeFrameHorizontalBorderHeight;

		// Token: 0x040012CD RID: 4813
		private static double _resizeFrameVerticalBorderWidth;

		// Token: 0x040012CE RID: 4814
		private static double _smallIconWidth;

		// Token: 0x040012CF RID: 4815
		private static double _smallIconHeight;

		// Token: 0x040012D0 RID: 4816
		private static double _smallWindowCaptionButtonWidth;

		// Token: 0x040012D1 RID: 4817
		private static double _smallWindowCaptionButtonHeight;

		// Token: 0x040012D2 RID: 4818
		private static double _virtualScreenWidth;

		// Token: 0x040012D3 RID: 4819
		private static double _virtualScreenHeight;

		// Token: 0x040012D4 RID: 4820
		private static double _verticalScrollBarWidth;

		// Token: 0x040012D5 RID: 4821
		private static double _verticalScrollBarButtonHeight;

		// Token: 0x040012D6 RID: 4822
		private static double _windowCaptionHeight;

		// Token: 0x040012D7 RID: 4823
		private static double _kanjiWindowHeight;

		// Token: 0x040012D8 RID: 4824
		private static double _menuBarHeight;

		// Token: 0x040012D9 RID: 4825
		private static double _verticalScrollBarThumbHeight;

		// Token: 0x040012DA RID: 4826
		private static bool _isImmEnabled;

		// Token: 0x040012DB RID: 4827
		private static bool _isMediaCenter;

		// Token: 0x040012DC RID: 4828
		private static bool _isMenuDropRightAligned;

		// Token: 0x040012DD RID: 4829
		private static bool _isMiddleEastEnabled;

		// Token: 0x040012DE RID: 4830
		private static bool _isMousePresent;

		// Token: 0x040012DF RID: 4831
		private static bool _isMouseWheelPresent;

		// Token: 0x040012E0 RID: 4832
		private static bool _isPenWindows;

		// Token: 0x040012E1 RID: 4833
		private static bool _isRemotelyControlled;

		// Token: 0x040012E2 RID: 4834
		private static bool _isRemoteSession;

		// Token: 0x040012E3 RID: 4835
		private static bool _showSounds;

		// Token: 0x040012E4 RID: 4836
		private static bool _isSlowMachine;

		// Token: 0x040012E5 RID: 4837
		private static bool _swapButtons;

		// Token: 0x040012E6 RID: 4838
		private static bool _isTabletPC;

		// Token: 0x040012E7 RID: 4839
		private static double _virtualScreenLeft;

		// Token: 0x040012E8 RID: 4840
		private static double _virtualScreenTop;

		// Token: 0x040012E9 RID: 4841
		private static PowerLineStatus _powerLineStatus;

		// Token: 0x040012EA RID: 4842
		private static SystemResourceKey _cacheThinHorizontalBorderHeight;

		// Token: 0x040012EB RID: 4843
		private static SystemResourceKey _cacheThinVerticalBorderWidth;

		// Token: 0x040012EC RID: 4844
		private static SystemResourceKey _cacheCursorWidth;

		// Token: 0x040012ED RID: 4845
		private static SystemResourceKey _cacheCursorHeight;

		// Token: 0x040012EE RID: 4846
		private static SystemResourceKey _cacheThickHorizontalBorderHeight;

		// Token: 0x040012EF RID: 4847
		private static SystemResourceKey _cacheThickVerticalBorderWidth;

		// Token: 0x040012F0 RID: 4848
		private static SystemResourceKey _cacheFixedFrameHorizontalBorderHeight;

		// Token: 0x040012F1 RID: 4849
		private static SystemResourceKey _cacheFixedFrameVerticalBorderWidth;

		// Token: 0x040012F2 RID: 4850
		private static SystemResourceKey _cacheFocusHorizontalBorderHeight;

		// Token: 0x040012F3 RID: 4851
		private static SystemResourceKey _cacheFocusVerticalBorderWidth;

		// Token: 0x040012F4 RID: 4852
		private static SystemResourceKey _cacheFullPrimaryScreenWidth;

		// Token: 0x040012F5 RID: 4853
		private static SystemResourceKey _cacheFullPrimaryScreenHeight;

		// Token: 0x040012F6 RID: 4854
		private static SystemResourceKey _cacheHorizontalScrollBarButtonWidth;

		// Token: 0x040012F7 RID: 4855
		private static SystemResourceKey _cacheHorizontalScrollBarHeight;

		// Token: 0x040012F8 RID: 4856
		private static SystemResourceKey _cacheHorizontalScrollBarThumbWidth;

		// Token: 0x040012F9 RID: 4857
		private static SystemResourceKey _cacheIconWidth;

		// Token: 0x040012FA RID: 4858
		private static SystemResourceKey _cacheIconHeight;

		// Token: 0x040012FB RID: 4859
		private static SystemResourceKey _cacheIconGridWidth;

		// Token: 0x040012FC RID: 4860
		private static SystemResourceKey _cacheIconGridHeight;

		// Token: 0x040012FD RID: 4861
		private static SystemResourceKey _cacheMaximizedPrimaryScreenWidth;

		// Token: 0x040012FE RID: 4862
		private static SystemResourceKey _cacheMaximizedPrimaryScreenHeight;

		// Token: 0x040012FF RID: 4863
		private static SystemResourceKey _cacheMaximumWindowTrackWidth;

		// Token: 0x04001300 RID: 4864
		private static SystemResourceKey _cacheMaximumWindowTrackHeight;

		// Token: 0x04001301 RID: 4865
		private static SystemResourceKey _cacheMenuCheckmarkWidth;

		// Token: 0x04001302 RID: 4866
		private static SystemResourceKey _cacheMenuCheckmarkHeight;

		// Token: 0x04001303 RID: 4867
		private static SystemResourceKey _cacheMenuButtonWidth;

		// Token: 0x04001304 RID: 4868
		private static SystemResourceKey _cacheMenuButtonHeight;

		// Token: 0x04001305 RID: 4869
		private static SystemResourceKey _cacheMinimumWindowWidth;

		// Token: 0x04001306 RID: 4870
		private static SystemResourceKey _cacheMinimumWindowHeight;

		// Token: 0x04001307 RID: 4871
		private static SystemResourceKey _cacheMinimizedWindowWidth;

		// Token: 0x04001308 RID: 4872
		private static SystemResourceKey _cacheMinimizedWindowHeight;

		// Token: 0x04001309 RID: 4873
		private static SystemResourceKey _cacheMinimizedGridWidth;

		// Token: 0x0400130A RID: 4874
		private static SystemResourceKey _cacheMinimizedGridHeight;

		// Token: 0x0400130B RID: 4875
		private static SystemResourceKey _cacheMinimumWindowTrackWidth;

		// Token: 0x0400130C RID: 4876
		private static SystemResourceKey _cacheMinimumWindowTrackHeight;

		// Token: 0x0400130D RID: 4877
		private static SystemResourceKey _cachePrimaryScreenWidth;

		// Token: 0x0400130E RID: 4878
		private static SystemResourceKey _cachePrimaryScreenHeight;

		// Token: 0x0400130F RID: 4879
		private static SystemResourceKey _cacheWindowCaptionButtonWidth;

		// Token: 0x04001310 RID: 4880
		private static SystemResourceKey _cacheWindowCaptionButtonHeight;

		// Token: 0x04001311 RID: 4881
		private static SystemResourceKey _cacheResizeFrameHorizontalBorderHeight;

		// Token: 0x04001312 RID: 4882
		private static SystemResourceKey _cacheResizeFrameVerticalBorderWidth;

		// Token: 0x04001313 RID: 4883
		private static SystemResourceKey _cacheSmallIconWidth;

		// Token: 0x04001314 RID: 4884
		private static SystemResourceKey _cacheSmallIconHeight;

		// Token: 0x04001315 RID: 4885
		private static SystemResourceKey _cacheSmallWindowCaptionButtonWidth;

		// Token: 0x04001316 RID: 4886
		private static SystemResourceKey _cacheSmallWindowCaptionButtonHeight;

		// Token: 0x04001317 RID: 4887
		private static SystemResourceKey _cacheVirtualScreenWidth;

		// Token: 0x04001318 RID: 4888
		private static SystemResourceKey _cacheVirtualScreenHeight;

		// Token: 0x04001319 RID: 4889
		private static SystemResourceKey _cacheVerticalScrollBarWidth;

		// Token: 0x0400131A RID: 4890
		private static SystemResourceKey _cacheVerticalScrollBarButtonHeight;

		// Token: 0x0400131B RID: 4891
		private static SystemResourceKey _cacheWindowCaptionHeight;

		// Token: 0x0400131C RID: 4892
		private static SystemResourceKey _cacheKanjiWindowHeight;

		// Token: 0x0400131D RID: 4893
		private static SystemResourceKey _cacheMenuBarHeight;

		// Token: 0x0400131E RID: 4894
		private static SystemResourceKey _cacheSmallCaptionHeight;

		// Token: 0x0400131F RID: 4895
		private static SystemResourceKey _cacheVerticalScrollBarThumbHeight;

		// Token: 0x04001320 RID: 4896
		private static SystemResourceKey _cacheIsImmEnabled;

		// Token: 0x04001321 RID: 4897
		private static SystemResourceKey _cacheIsMediaCenter;

		// Token: 0x04001322 RID: 4898
		private static SystemResourceKey _cacheIsMenuDropRightAligned;

		// Token: 0x04001323 RID: 4899
		private static SystemResourceKey _cacheIsMiddleEastEnabled;

		// Token: 0x04001324 RID: 4900
		private static SystemResourceKey _cacheIsMousePresent;

		// Token: 0x04001325 RID: 4901
		private static SystemResourceKey _cacheIsMouseWheelPresent;

		// Token: 0x04001326 RID: 4902
		private static SystemResourceKey _cacheIsPenWindows;

		// Token: 0x04001327 RID: 4903
		private static SystemResourceKey _cacheIsRemotelyControlled;

		// Token: 0x04001328 RID: 4904
		private static SystemResourceKey _cacheIsRemoteSession;

		// Token: 0x04001329 RID: 4905
		private static SystemResourceKey _cacheShowSounds;

		// Token: 0x0400132A RID: 4906
		private static SystemResourceKey _cacheIsSlowMachine;

		// Token: 0x0400132B RID: 4907
		private static SystemResourceKey _cacheSwapButtons;

		// Token: 0x0400132C RID: 4908
		private static SystemResourceKey _cacheIsTabletPC;

		// Token: 0x0400132D RID: 4909
		private static SystemResourceKey _cacheVirtualScreenLeft;

		// Token: 0x0400132E RID: 4910
		private static SystemResourceKey _cacheVirtualScreenTop;

		// Token: 0x0400132F RID: 4911
		private static SystemResourceKey _cacheFocusBorderWidth;

		// Token: 0x04001330 RID: 4912
		private static SystemResourceKey _cacheFocusBorderHeight;

		// Token: 0x04001331 RID: 4913
		private static SystemResourceKey _cacheHighContrast;

		// Token: 0x04001332 RID: 4914
		private static SystemResourceKey _cacheDropShadow;

		// Token: 0x04001333 RID: 4915
		private static SystemResourceKey _cacheFlatMenu;

		// Token: 0x04001334 RID: 4916
		private static SystemResourceKey _cacheWorkArea;

		// Token: 0x04001335 RID: 4917
		private static SystemResourceKey _cacheIconHorizontalSpacing;

		// Token: 0x04001336 RID: 4918
		private static SystemResourceKey _cacheIconVerticalSpacing;

		// Token: 0x04001337 RID: 4919
		private static SystemResourceKey _cacheIconTitleWrap;

		// Token: 0x04001338 RID: 4920
		private static SystemResourceKey _cacheKeyboardCues;

		// Token: 0x04001339 RID: 4921
		private static SystemResourceKey _cacheKeyboardDelay;

		// Token: 0x0400133A RID: 4922
		private static SystemResourceKey _cacheKeyboardPreference;

		// Token: 0x0400133B RID: 4923
		private static SystemResourceKey _cacheKeyboardSpeed;

		// Token: 0x0400133C RID: 4924
		private static SystemResourceKey _cacheSnapToDefaultButton;

		// Token: 0x0400133D RID: 4925
		private static SystemResourceKey _cacheWheelScrollLines;

		// Token: 0x0400133E RID: 4926
		private static SystemResourceKey _cacheMouseHoverTime;

		// Token: 0x0400133F RID: 4927
		private static SystemResourceKey _cacheMouseHoverHeight;

		// Token: 0x04001340 RID: 4928
		private static SystemResourceKey _cacheMouseHoverWidth;

		// Token: 0x04001341 RID: 4929
		private static SystemResourceKey _cacheMenuDropAlignment;

		// Token: 0x04001342 RID: 4930
		private static SystemResourceKey _cacheMenuFade;

		// Token: 0x04001343 RID: 4931
		private static SystemResourceKey _cacheMenuShowDelay;

		// Token: 0x04001344 RID: 4932
		private static SystemResourceKey _cacheComboBoxAnimation;

		// Token: 0x04001345 RID: 4933
		private static SystemResourceKey _cacheClientAreaAnimation;

		// Token: 0x04001346 RID: 4934
		private static SystemResourceKey _cacheCursorShadow;

		// Token: 0x04001347 RID: 4935
		private static SystemResourceKey _cacheGradientCaptions;

		// Token: 0x04001348 RID: 4936
		private static SystemResourceKey _cacheHotTracking;

		// Token: 0x04001349 RID: 4937
		private static SystemResourceKey _cacheListBoxSmoothScrolling;

		// Token: 0x0400134A RID: 4938
		private static SystemResourceKey _cacheMenuAnimation;

		// Token: 0x0400134B RID: 4939
		private static SystemResourceKey _cacheSelectionFade;

		// Token: 0x0400134C RID: 4940
		private static SystemResourceKey _cacheStylusHotTracking;

		// Token: 0x0400134D RID: 4941
		private static SystemResourceKey _cacheToolTipAnimation;

		// Token: 0x0400134E RID: 4942
		private static SystemResourceKey _cacheToolTipFade;

		// Token: 0x0400134F RID: 4943
		private static SystemResourceKey _cacheUIEffects;

		// Token: 0x04001350 RID: 4944
		private static SystemResourceKey _cacheMinimizeAnimation;

		// Token: 0x04001351 RID: 4945
		private static SystemResourceKey _cacheBorder;

		// Token: 0x04001352 RID: 4946
		private static SystemResourceKey _cacheCaretWidth;

		// Token: 0x04001353 RID: 4947
		private static SystemResourceKey _cacheForegroundFlashCount;

		// Token: 0x04001354 RID: 4948
		private static SystemResourceKey _cacheDragFullWindows;

		// Token: 0x04001355 RID: 4949
		private static SystemResourceKey _cacheBorderWidth;

		// Token: 0x04001356 RID: 4950
		private static SystemResourceKey _cacheScrollWidth;

		// Token: 0x04001357 RID: 4951
		private static SystemResourceKey _cacheScrollHeight;

		// Token: 0x04001358 RID: 4952
		private static SystemResourceKey _cacheCaptionWidth;

		// Token: 0x04001359 RID: 4953
		private static SystemResourceKey _cacheCaptionHeight;

		// Token: 0x0400135A RID: 4954
		private static SystemResourceKey _cacheSmallCaptionWidth;

		// Token: 0x0400135B RID: 4955
		private static SystemResourceKey _cacheMenuWidth;

		// Token: 0x0400135C RID: 4956
		private static SystemResourceKey _cacheMenuHeight;

		// Token: 0x0400135D RID: 4957
		private static SystemResourceKey _cacheComboBoxPopupAnimation;

		// Token: 0x0400135E RID: 4958
		private static SystemResourceKey _cacheMenuPopupAnimation;

		// Token: 0x0400135F RID: 4959
		private static SystemResourceKey _cacheToolTipPopupAnimation;

		// Token: 0x04001360 RID: 4960
		private static SystemResourceKey _cachePowerLineStatus;

		// Token: 0x04001361 RID: 4961
		private static SystemThemeKey _cacheFocusVisualStyle;

		// Token: 0x04001362 RID: 4962
		private static SystemThemeKey _cacheNavigationChromeStyle;

		// Token: 0x04001363 RID: 4963
		private static SystemThemeKey _cacheNavigationChromeDownLevelStyle;

		// Token: 0x02000A8F RID: 2703
		private enum CacheSlot
		{
			// Token: 0x040041E0 RID: 16864
			DpiX,
			// Token: 0x040041E1 RID: 16865
			FocusBorderWidth,
			// Token: 0x040041E2 RID: 16866
			FocusBorderHeight,
			// Token: 0x040041E3 RID: 16867
			HighContrast,
			// Token: 0x040041E4 RID: 16868
			MouseVanish,
			// Token: 0x040041E5 RID: 16869
			DropShadow,
			// Token: 0x040041E6 RID: 16870
			FlatMenu,
			// Token: 0x040041E7 RID: 16871
			WorkAreaInternal,
			// Token: 0x040041E8 RID: 16872
			WorkArea,
			// Token: 0x040041E9 RID: 16873
			IconMetrics,
			// Token: 0x040041EA RID: 16874
			KeyboardCues,
			// Token: 0x040041EB RID: 16875
			KeyboardDelay,
			// Token: 0x040041EC RID: 16876
			KeyboardPreference,
			// Token: 0x040041ED RID: 16877
			KeyboardSpeed,
			// Token: 0x040041EE RID: 16878
			SnapToDefaultButton,
			// Token: 0x040041EF RID: 16879
			WheelScrollLines,
			// Token: 0x040041F0 RID: 16880
			MouseHoverTime,
			// Token: 0x040041F1 RID: 16881
			MouseHoverHeight,
			// Token: 0x040041F2 RID: 16882
			MouseHoverWidth,
			// Token: 0x040041F3 RID: 16883
			MenuDropAlignment,
			// Token: 0x040041F4 RID: 16884
			MenuFade,
			// Token: 0x040041F5 RID: 16885
			MenuShowDelay,
			// Token: 0x040041F6 RID: 16886
			ComboBoxAnimation,
			// Token: 0x040041F7 RID: 16887
			ClientAreaAnimation,
			// Token: 0x040041F8 RID: 16888
			CursorShadow,
			// Token: 0x040041F9 RID: 16889
			GradientCaptions,
			// Token: 0x040041FA RID: 16890
			HotTracking,
			// Token: 0x040041FB RID: 16891
			ListBoxSmoothScrolling,
			// Token: 0x040041FC RID: 16892
			MenuAnimation,
			// Token: 0x040041FD RID: 16893
			SelectionFade,
			// Token: 0x040041FE RID: 16894
			StylusHotTracking,
			// Token: 0x040041FF RID: 16895
			ToolTipAnimation,
			// Token: 0x04004200 RID: 16896
			ToolTipFade,
			// Token: 0x04004201 RID: 16897
			UIEffects,
			// Token: 0x04004202 RID: 16898
			MinimizeAnimation,
			// Token: 0x04004203 RID: 16899
			Border,
			// Token: 0x04004204 RID: 16900
			CaretWidth,
			// Token: 0x04004205 RID: 16901
			ForegroundFlashCount,
			// Token: 0x04004206 RID: 16902
			DragFullWindows,
			// Token: 0x04004207 RID: 16903
			NonClientMetrics,
			// Token: 0x04004208 RID: 16904
			ThinHorizontalBorderHeight,
			// Token: 0x04004209 RID: 16905
			ThinVerticalBorderWidth,
			// Token: 0x0400420A RID: 16906
			CursorWidth,
			// Token: 0x0400420B RID: 16907
			CursorHeight,
			// Token: 0x0400420C RID: 16908
			ThickHorizontalBorderHeight,
			// Token: 0x0400420D RID: 16909
			ThickVerticalBorderWidth,
			// Token: 0x0400420E RID: 16910
			MinimumHorizontalDragDistance,
			// Token: 0x0400420F RID: 16911
			MinimumVerticalDragDistance,
			// Token: 0x04004210 RID: 16912
			FixedFrameHorizontalBorderHeight,
			// Token: 0x04004211 RID: 16913
			FixedFrameVerticalBorderWidth,
			// Token: 0x04004212 RID: 16914
			FocusHorizontalBorderHeight,
			// Token: 0x04004213 RID: 16915
			FocusVerticalBorderWidth,
			// Token: 0x04004214 RID: 16916
			FullPrimaryScreenWidth,
			// Token: 0x04004215 RID: 16917
			FullPrimaryScreenHeight,
			// Token: 0x04004216 RID: 16918
			HorizontalScrollBarButtonWidth,
			// Token: 0x04004217 RID: 16919
			HorizontalScrollBarHeight,
			// Token: 0x04004218 RID: 16920
			HorizontalScrollBarThumbWidth,
			// Token: 0x04004219 RID: 16921
			IconWidth,
			// Token: 0x0400421A RID: 16922
			IconHeight,
			// Token: 0x0400421B RID: 16923
			IconGridWidth,
			// Token: 0x0400421C RID: 16924
			IconGridHeight,
			// Token: 0x0400421D RID: 16925
			MaximizedPrimaryScreenWidth,
			// Token: 0x0400421E RID: 16926
			MaximizedPrimaryScreenHeight,
			// Token: 0x0400421F RID: 16927
			MaximumWindowTrackWidth,
			// Token: 0x04004220 RID: 16928
			MaximumWindowTrackHeight,
			// Token: 0x04004221 RID: 16929
			MenuCheckmarkWidth,
			// Token: 0x04004222 RID: 16930
			MenuCheckmarkHeight,
			// Token: 0x04004223 RID: 16931
			MenuButtonWidth,
			// Token: 0x04004224 RID: 16932
			MenuButtonHeight,
			// Token: 0x04004225 RID: 16933
			MinimumWindowWidth,
			// Token: 0x04004226 RID: 16934
			MinimumWindowHeight,
			// Token: 0x04004227 RID: 16935
			MinimizedWindowWidth,
			// Token: 0x04004228 RID: 16936
			MinimizedWindowHeight,
			// Token: 0x04004229 RID: 16937
			MinimizedGridWidth,
			// Token: 0x0400422A RID: 16938
			MinimizedGridHeight,
			// Token: 0x0400422B RID: 16939
			MinimumWindowTrackWidth,
			// Token: 0x0400422C RID: 16940
			MinimumWindowTrackHeight,
			// Token: 0x0400422D RID: 16941
			PrimaryScreenWidth,
			// Token: 0x0400422E RID: 16942
			PrimaryScreenHeight,
			// Token: 0x0400422F RID: 16943
			WindowCaptionButtonWidth,
			// Token: 0x04004230 RID: 16944
			WindowCaptionButtonHeight,
			// Token: 0x04004231 RID: 16945
			ResizeFrameHorizontalBorderHeight,
			// Token: 0x04004232 RID: 16946
			ResizeFrameVerticalBorderWidth,
			// Token: 0x04004233 RID: 16947
			SmallIconWidth,
			// Token: 0x04004234 RID: 16948
			SmallIconHeight,
			// Token: 0x04004235 RID: 16949
			SmallWindowCaptionButtonWidth,
			// Token: 0x04004236 RID: 16950
			SmallWindowCaptionButtonHeight,
			// Token: 0x04004237 RID: 16951
			VirtualScreenWidth,
			// Token: 0x04004238 RID: 16952
			VirtualScreenHeight,
			// Token: 0x04004239 RID: 16953
			VerticalScrollBarWidth,
			// Token: 0x0400423A RID: 16954
			VerticalScrollBarButtonHeight,
			// Token: 0x0400423B RID: 16955
			WindowCaptionHeight,
			// Token: 0x0400423C RID: 16956
			KanjiWindowHeight,
			// Token: 0x0400423D RID: 16957
			MenuBarHeight,
			// Token: 0x0400423E RID: 16958
			VerticalScrollBarThumbHeight,
			// Token: 0x0400423F RID: 16959
			IsImmEnabled,
			// Token: 0x04004240 RID: 16960
			IsMediaCenter,
			// Token: 0x04004241 RID: 16961
			IsMenuDropRightAligned,
			// Token: 0x04004242 RID: 16962
			IsMiddleEastEnabled,
			// Token: 0x04004243 RID: 16963
			IsMousePresent,
			// Token: 0x04004244 RID: 16964
			IsMouseWheelPresent,
			// Token: 0x04004245 RID: 16965
			IsPenWindows,
			// Token: 0x04004246 RID: 16966
			IsRemotelyControlled,
			// Token: 0x04004247 RID: 16967
			IsRemoteSession,
			// Token: 0x04004248 RID: 16968
			ShowSounds,
			// Token: 0x04004249 RID: 16969
			IsSlowMachine,
			// Token: 0x0400424A RID: 16970
			SwapButtons,
			// Token: 0x0400424B RID: 16971
			IsTabletPC,
			// Token: 0x0400424C RID: 16972
			VirtualScreenLeft,
			// Token: 0x0400424D RID: 16973
			VirtualScreenTop,
			// Token: 0x0400424E RID: 16974
			PowerLineStatus,
			// Token: 0x0400424F RID: 16975
			IsGlassEnabled,
			// Token: 0x04004250 RID: 16976
			UxThemeName,
			// Token: 0x04004251 RID: 16977
			UxThemeColor,
			// Token: 0x04004252 RID: 16978
			WindowCornerRadius,
			// Token: 0x04004253 RID: 16979
			WindowGlassColor,
			// Token: 0x04004254 RID: 16980
			WindowGlassBrush,
			// Token: 0x04004255 RID: 16981
			WindowNonClientFrameThickness,
			// Token: 0x04004256 RID: 16982
			WindowResizeBorderThickness,
			// Token: 0x04004257 RID: 16983
			NumSlots
		}
	}
}
