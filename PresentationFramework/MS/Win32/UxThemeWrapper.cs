using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using MS.Internal;

namespace MS.Win32
{
	// Token: 0x020000E7 RID: 231
	internal static class UxThemeWrapper
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000FF00C File Offset: 0x000FE00C
		internal static bool IsActive
		{
			get
			{
				return UxThemeWrapper.IsActiveCompatWrapper;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x000FF013 File Offset: 0x000FE013
		internal static string ThemeName
		{
			get
			{
				return UxThemeWrapper.ThemeNameCompatWrapper;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x000FF01A File Offset: 0x000FE01A
		internal static string ThemeColor
		{
			get
			{
				return UxThemeWrapper.ThemeColorCompatWrapper;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x000FF021 File Offset: 0x000FE021
		internal static string ThemedResourceName
		{
			get
			{
				return UxThemeWrapper.ThemedResourceNameCompatWrapper;
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000FF028 File Offset: 0x000FE028
		private static UxThemeWrapper.ThemeState EnsureThemeState(bool themeChanged)
		{
			UxThemeWrapper.ThemeState themeState = UxThemeWrapper._themeState;
			bool flag = !themeChanged;
			bool flag2 = true;
			while (flag2)
			{
				UxThemeWrapper.ThemeState themeState2;
				if (themeChanged)
				{
					bool flag3 = !SystemParameters.HighContrast && SafeNativeMethods.IsUxThemeActive();
					string name;
					string color;
					if (flag3 && (flag || themeState.ThemeName != null))
					{
						UxThemeWrapper.GetThemeNameAndColor(out name, out color);
					}
					else
					{
						color = (name = null);
					}
					themeState2 = new UxThemeWrapper.ThemeState(flag3, name, color);
				}
				else if (themeState.IsActive && themeState.ThemeName == null)
				{
					string name;
					string color;
					UxThemeWrapper.GetThemeNameAndColor(out name, out color);
					themeState2 = new UxThemeWrapper.ThemeState(themeState.IsActive, name, color);
				}
				else
				{
					themeState2 = themeState;
					flag2 = false;
				}
				if (flag2)
				{
					UxThemeWrapper.ThemeState themeState3 = Interlocked.CompareExchange<UxThemeWrapper.ThemeState>(ref UxThemeWrapper._themeState, themeState2, themeState);
					if (themeState3 == themeState)
					{
						themeState = themeState2;
						flag2 = false;
					}
					else if (themeState3.IsActive == themeState2.IsActive && (!themeState2.IsActive || themeState2.ThemeName == null || themeState3.ThemeName != null))
					{
						themeState = themeState3;
						flag2 = false;
					}
					else
					{
						themeChanged = true;
						themeState = themeState3;
					}
				}
			}
			return themeState;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000FF114 File Offset: 0x000FE114
		private static void GetThemeNameAndColor(out string themeName, out string themeColor)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			StringBuilder stringBuilder2 = new StringBuilder(260);
			if (UnsafeNativeMethods.GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, stringBuilder2, stringBuilder2.Capacity, null, 0) == 0)
			{
				themeName = stringBuilder.ToString();
				themeName = Path.GetFileNameWithoutExtension(themeName);
				if (string.Compare(themeName, "aero", StringComparison.OrdinalIgnoreCase) == 0 && Utilities.IsOSWindows8OrNewer)
				{
					themeName = "Aero2";
				}
				themeColor = stringBuilder2.ToString();
				return;
			}
			string empty;
			themeColor = (empty = string.Empty);
			themeName = empty;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000FF191 File Offset: 0x000FE191
		internal static void OnThemeChanged()
		{
			UxThemeWrapper.RestoreSupportedState();
			UxThemeWrapper.EnsureThemeState(true);
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x000FF19F File Offset: 0x000FE19F
		private static bool IsAppSupported
		{
			get
			{
				return UxThemeWrapper._themeName == null;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x000FF1A9 File Offset: 0x000FE1A9
		private static bool IsActiveCompatWrapper
		{
			get
			{
				if (!UxThemeWrapper.IsAppSupported)
				{
					return UxThemeWrapper._isActive;
				}
				return UxThemeWrapper._themeState.IsActive;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x000FF1C4 File Offset: 0x000FE1C4
		private static string ThemeNameCompatWrapper
		{
			get
			{
				if (!UxThemeWrapper.IsAppSupported)
				{
					return UxThemeWrapper._themeName;
				}
				UxThemeWrapper.ThemeState themeState = UxThemeWrapper.EnsureThemeState(false);
				if (themeState.IsActive)
				{
					return themeState.ThemeName;
				}
				return "classic";
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x000FF1F9 File Offset: 0x000FE1F9
		private static string ThemeColorCompatWrapper
		{
			get
			{
				if (UxThemeWrapper.IsAppSupported)
				{
					return UxThemeWrapper.EnsureThemeState(false).ThemeColor;
				}
				return UxThemeWrapper._themeColor;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x000FF214 File Offset: 0x000FE214
		private static string ThemedResourceNameCompatWrapper
		{
			get
			{
				if (UxThemeWrapper.IsAppSupported)
				{
					UxThemeWrapper.ThemeState themeState = UxThemeWrapper.EnsureThemeState(false);
					if (themeState.IsActive)
					{
						return "themes/" + themeState.ThemeName.ToLowerInvariant() + "." + themeState.ThemeColor.ToLowerInvariant();
					}
					return "themes/classic";
				}
				else
				{
					if (UxThemeWrapper._isActive)
					{
						return "themes/" + UxThemeWrapper._themeName.ToLowerInvariant() + "." + UxThemeWrapper._themeColor.ToLowerInvariant();
					}
					return "themes/classic";
				}
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000FF293 File Offset: 0x000FE293
		private static void RestoreSupportedState()
		{
			UxThemeWrapper._isActive = false;
			UxThemeWrapper._themeName = null;
			UxThemeWrapper._themeColor = null;
		}

		// Token: 0x0400060C RID: 1548
		private static UxThemeWrapper.ThemeState _themeState = new UxThemeWrapper.ThemeState(!SystemParameters.HighContrast && SafeNativeMethods.IsUxThemeActive(), null, null);

		// Token: 0x0400060D RID: 1549
		private static bool _isActive;

		// Token: 0x0400060E RID: 1550
		private static string _themeName;

		// Token: 0x0400060F RID: 1551
		private static string _themeColor;

		// Token: 0x020008A6 RID: 2214
		private class ThemeState
		{
			// Token: 0x060080AC RID: 32940 RVA: 0x003223C7 File Offset: 0x003213C7
			public ThemeState(bool isActive, string name, string color)
			{
				this._isActive = isActive;
				this._themeName = name;
				this._themeColor = color;
			}

			// Token: 0x17001D7B RID: 7547
			// (get) Token: 0x060080AD RID: 32941 RVA: 0x003223E4 File Offset: 0x003213E4
			public bool IsActive
			{
				get
				{
					return this._isActive;
				}
			}

			// Token: 0x17001D7C RID: 7548
			// (get) Token: 0x060080AE RID: 32942 RVA: 0x003223EC File Offset: 0x003213EC
			public string ThemeName
			{
				get
				{
					return this._themeName;
				}
			}

			// Token: 0x17001D7D RID: 7549
			// (get) Token: 0x060080AF RID: 32943 RVA: 0x003223F4 File Offset: 0x003213F4
			public string ThemeColor
			{
				get
				{
					return this._themeColor;
				}
			}

			// Token: 0x04003C05 RID: 15365
			private bool _isActive;

			// Token: 0x04003C06 RID: 15366
			private string _themeName;

			// Token: 0x04003C07 RID: 15367
			private string _themeColor;
		}
	}
}
