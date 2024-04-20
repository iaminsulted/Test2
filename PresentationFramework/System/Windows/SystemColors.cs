using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using MS.Win32;

namespace System.Windows
{
	// Token: 0x020003B3 RID: 947
	public static class SystemColors
	{
		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06002626 RID: 9766 RVA: 0x0018B462 File Offset: 0x0018A462
		public static Color ActiveBorderColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ActiveBorder);
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002627 RID: 9767 RVA: 0x0018B46A File Offset: 0x0018A46A
		public static Color ActiveCaptionColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ActiveCaption);
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06002628 RID: 9768 RVA: 0x0018B472 File Offset: 0x0018A472
		public static Color ActiveCaptionTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ActiveCaptionText);
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06002629 RID: 9769 RVA: 0x0018B47A File Offset: 0x0018A47A
		public static Color AppWorkspaceColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.AppWorkspace);
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600262A RID: 9770 RVA: 0x0018B482 File Offset: 0x0018A482
		public static Color ControlColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Control);
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600262B RID: 9771 RVA: 0x0018B48A File Offset: 0x0018A48A
		public static Color ControlDarkColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlDark);
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600262C RID: 9772 RVA: 0x0018B492 File Offset: 0x0018A492
		public static Color ControlDarkDarkColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlDarkDark);
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600262D RID: 9773 RVA: 0x0018B49A File Offset: 0x0018A49A
		public static Color ControlLightColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlLight);
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600262E RID: 9774 RVA: 0x0018B4A2 File Offset: 0x0018A4A2
		public static Color ControlLightLightColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlLightLight);
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x0600262F RID: 9775 RVA: 0x0018B4AA File Offset: 0x0018A4AA
		public static Color ControlTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlText);
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x0018B4B3 File Offset: 0x0018A4B3
		public static Color DesktopColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Desktop);
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002631 RID: 9777 RVA: 0x0018B4BC File Offset: 0x0018A4BC
		public static Color GradientActiveCaptionColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.GradientActiveCaption);
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x0018B4C5 File Offset: 0x0018A4C5
		public static Color GradientInactiveCaptionColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.GradientInactiveCaption);
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002633 RID: 9779 RVA: 0x0018B4CE File Offset: 0x0018A4CE
		public static Color GrayTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.GrayText);
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x0018B4D7 File Offset: 0x0018A4D7
		public static Color HighlightColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Highlight);
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002635 RID: 9781 RVA: 0x0018B4E0 File Offset: 0x0018A4E0
		public static Color HighlightTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.HighlightText);
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x0018B4E9 File Offset: 0x0018A4E9
		public static Color HotTrackColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.HotTrack);
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002637 RID: 9783 RVA: 0x0018B4F2 File Offset: 0x0018A4F2
		public static Color InactiveBorderColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.InactiveBorder);
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x0018B4FB File Offset: 0x0018A4FB
		public static Color InactiveCaptionColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.InactiveCaption);
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002639 RID: 9785 RVA: 0x0018B504 File Offset: 0x0018A504
		public static Color InactiveCaptionTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.InactiveCaptionText);
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x0018B50D File Offset: 0x0018A50D
		public static Color InfoColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Info);
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600263B RID: 9787 RVA: 0x0018B516 File Offset: 0x0018A516
		public static Color InfoTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.InfoText);
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x0600263C RID: 9788 RVA: 0x0018B51F File Offset: 0x0018A51F
		public static Color MenuColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Menu);
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x0600263D RID: 9789 RVA: 0x0018B528 File Offset: 0x0018A528
		public static Color MenuBarColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.MenuBar);
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x0600263E RID: 9790 RVA: 0x0018B531 File Offset: 0x0018A531
		public static Color MenuHighlightColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.MenuHighlight);
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600263F RID: 9791 RVA: 0x0018B53A File Offset: 0x0018A53A
		public static Color MenuTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.MenuText);
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002640 RID: 9792 RVA: 0x0018B543 File Offset: 0x0018A543
		public static Color ScrollBarColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ScrollBar);
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002641 RID: 9793 RVA: 0x0018B54C File Offset: 0x0018A54C
		public static Color WindowColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Window);
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002642 RID: 9794 RVA: 0x0018B555 File Offset: 0x0018A555
		public static Color WindowFrameColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.WindowFrame);
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002643 RID: 9795 RVA: 0x0018B55E File Offset: 0x0018A55E
		public static Color WindowTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.WindowText);
			}
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x0018B567 File Offset: 0x0018A567
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
		{
			return new SystemResourceKey(KeyId);
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002645 RID: 9797 RVA: 0x0018B56F File Offset: 0x0018A56F
		public static ResourceKey ActiveBorderColorKey
		{
			get
			{
				if (SystemColors._cacheActiveBorderColor == null)
				{
					SystemColors._cacheActiveBorderColor = SystemColors.CreateInstance(SystemResourceKeyID.ActiveBorderColor);
				}
				return SystemColors._cacheActiveBorderColor;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002646 RID: 9798 RVA: 0x0018B589 File Offset: 0x0018A589
		public static ResourceKey ActiveCaptionColorKey
		{
			get
			{
				if (SystemColors._cacheActiveCaptionColor == null)
				{
					SystemColors._cacheActiveCaptionColor = SystemColors.CreateInstance(SystemResourceKeyID.ActiveCaptionColor);
				}
				return SystemColors._cacheActiveCaptionColor;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x0018B5A3 File Offset: 0x0018A5A3
		public static ResourceKey ActiveCaptionTextColorKey
		{
			get
			{
				if (SystemColors._cacheActiveCaptionTextColor == null)
				{
					SystemColors._cacheActiveCaptionTextColor = SystemColors.CreateInstance(SystemResourceKeyID.ActiveCaptionTextColor);
				}
				return SystemColors._cacheActiveCaptionTextColor;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06002648 RID: 9800 RVA: 0x0018B5BD File Offset: 0x0018A5BD
		public static ResourceKey AppWorkspaceColorKey
		{
			get
			{
				if (SystemColors._cacheAppWorkspaceColor == null)
				{
					SystemColors._cacheAppWorkspaceColor = SystemColors.CreateInstance(SystemResourceKeyID.AppWorkspaceColor);
				}
				return SystemColors._cacheAppWorkspaceColor;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002649 RID: 9801 RVA: 0x0018B5D7 File Offset: 0x0018A5D7
		public static ResourceKey ControlColorKey
		{
			get
			{
				if (SystemColors._cacheControlColor == null)
				{
					SystemColors._cacheControlColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlColor);
				}
				return SystemColors._cacheControlColor;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x0600264A RID: 9802 RVA: 0x0018B5F1 File Offset: 0x0018A5F1
		public static ResourceKey ControlDarkColorKey
		{
			get
			{
				if (SystemColors._cacheControlDarkColor == null)
				{
					SystemColors._cacheControlDarkColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlDarkColor);
				}
				return SystemColors._cacheControlDarkColor;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600264B RID: 9803 RVA: 0x0018B60B File Offset: 0x0018A60B
		public static ResourceKey ControlDarkDarkColorKey
		{
			get
			{
				if (SystemColors._cacheControlDarkDarkColor == null)
				{
					SystemColors._cacheControlDarkDarkColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlDarkDarkColor);
				}
				return SystemColors._cacheControlDarkDarkColor;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x0600264C RID: 9804 RVA: 0x0018B625 File Offset: 0x0018A625
		public static ResourceKey ControlLightColorKey
		{
			get
			{
				if (SystemColors._cacheControlLightColor == null)
				{
					SystemColors._cacheControlLightColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlLightColor);
				}
				return SystemColors._cacheControlLightColor;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x0600264D RID: 9805 RVA: 0x0018B63F File Offset: 0x0018A63F
		public static ResourceKey ControlLightLightColorKey
		{
			get
			{
				if (SystemColors._cacheControlLightLightColor == null)
				{
					SystemColors._cacheControlLightLightColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlLightLightColor);
				}
				return SystemColors._cacheControlLightLightColor;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600264E RID: 9806 RVA: 0x0018B659 File Offset: 0x0018A659
		public static ResourceKey ControlTextColorKey
		{
			get
			{
				if (SystemColors._cacheControlTextColor == null)
				{
					SystemColors._cacheControlTextColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlTextColor);
				}
				return SystemColors._cacheControlTextColor;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600264F RID: 9807 RVA: 0x0018B673 File Offset: 0x0018A673
		public static ResourceKey DesktopColorKey
		{
			get
			{
				if (SystemColors._cacheDesktopColor == null)
				{
					SystemColors._cacheDesktopColor = SystemColors.CreateInstance(SystemResourceKeyID.DesktopColor);
				}
				return SystemColors._cacheDesktopColor;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x0018B68D File Offset: 0x0018A68D
		public static ResourceKey GradientActiveCaptionColorKey
		{
			get
			{
				if (SystemColors._cacheGradientActiveCaptionColor == null)
				{
					SystemColors._cacheGradientActiveCaptionColor = SystemColors.CreateInstance(SystemResourceKeyID.GradientActiveCaptionColor);
				}
				return SystemColors._cacheGradientActiveCaptionColor;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x0018B6A7 File Offset: 0x0018A6A7
		public static ResourceKey GradientInactiveCaptionColorKey
		{
			get
			{
				if (SystemColors._cacheGradientInactiveCaptionColor == null)
				{
					SystemColors._cacheGradientInactiveCaptionColor = SystemColors.CreateInstance(SystemResourceKeyID.GradientInactiveCaptionColor);
				}
				return SystemColors._cacheGradientInactiveCaptionColor;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x0018B6C1 File Offset: 0x0018A6C1
		public static ResourceKey GrayTextColorKey
		{
			get
			{
				if (SystemColors._cacheGrayTextColor == null)
				{
					SystemColors._cacheGrayTextColor = SystemColors.CreateInstance(SystemResourceKeyID.GrayTextColor);
				}
				return SystemColors._cacheGrayTextColor;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x0018B6DB File Offset: 0x0018A6DB
		public static ResourceKey HighlightColorKey
		{
			get
			{
				if (SystemColors._cacheHighlightColor == null)
				{
					SystemColors._cacheHighlightColor = SystemColors.CreateInstance(SystemResourceKeyID.HighlightColor);
				}
				return SystemColors._cacheHighlightColor;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002654 RID: 9812 RVA: 0x0018B6F5 File Offset: 0x0018A6F5
		public static ResourceKey HighlightTextColorKey
		{
			get
			{
				if (SystemColors._cacheHighlightTextColor == null)
				{
					SystemColors._cacheHighlightTextColor = SystemColors.CreateInstance(SystemResourceKeyID.HighlightTextColor);
				}
				return SystemColors._cacheHighlightTextColor;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002655 RID: 9813 RVA: 0x0018B70F File Offset: 0x0018A70F
		public static ResourceKey HotTrackColorKey
		{
			get
			{
				if (SystemColors._cacheHotTrackColor == null)
				{
					SystemColors._cacheHotTrackColor = SystemColors.CreateInstance(SystemResourceKeyID.HotTrackColor);
				}
				return SystemColors._cacheHotTrackColor;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06002656 RID: 9814 RVA: 0x0018B729 File Offset: 0x0018A729
		public static ResourceKey InactiveBorderColorKey
		{
			get
			{
				if (SystemColors._cacheInactiveBorderColor == null)
				{
					SystemColors._cacheInactiveBorderColor = SystemColors.CreateInstance(SystemResourceKeyID.InactiveBorderColor);
				}
				return SystemColors._cacheInactiveBorderColor;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x0018B743 File Offset: 0x0018A743
		public static ResourceKey InactiveCaptionColorKey
		{
			get
			{
				if (SystemColors._cacheInactiveCaptionColor == null)
				{
					SystemColors._cacheInactiveCaptionColor = SystemColors.CreateInstance(SystemResourceKeyID.InactiveCaptionColor);
				}
				return SystemColors._cacheInactiveCaptionColor;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06002658 RID: 9816 RVA: 0x0018B75D File Offset: 0x0018A75D
		public static ResourceKey InactiveCaptionTextColorKey
		{
			get
			{
				if (SystemColors._cacheInactiveCaptionTextColor == null)
				{
					SystemColors._cacheInactiveCaptionTextColor = SystemColors.CreateInstance(SystemResourceKeyID.InactiveCaptionTextColor);
				}
				return SystemColors._cacheInactiveCaptionTextColor;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x0018B777 File Offset: 0x0018A777
		public static ResourceKey InfoColorKey
		{
			get
			{
				if (SystemColors._cacheInfoColor == null)
				{
					SystemColors._cacheInfoColor = SystemColors.CreateInstance(SystemResourceKeyID.InfoColor);
				}
				return SystemColors._cacheInfoColor;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x0600265A RID: 9818 RVA: 0x0018B791 File Offset: 0x0018A791
		public static ResourceKey InfoTextColorKey
		{
			get
			{
				if (SystemColors._cacheInfoTextColor == null)
				{
					SystemColors._cacheInfoTextColor = SystemColors.CreateInstance(SystemResourceKeyID.InfoTextColor);
				}
				return SystemColors._cacheInfoTextColor;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600265B RID: 9819 RVA: 0x0018B7AB File Offset: 0x0018A7AB
		public static ResourceKey MenuColorKey
		{
			get
			{
				if (SystemColors._cacheMenuColor == null)
				{
					SystemColors._cacheMenuColor = SystemColors.CreateInstance(SystemResourceKeyID.MenuColor);
				}
				return SystemColors._cacheMenuColor;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600265C RID: 9820 RVA: 0x0018B7C5 File Offset: 0x0018A7C5
		public static ResourceKey MenuBarColorKey
		{
			get
			{
				if (SystemColors._cacheMenuBarColor == null)
				{
					SystemColors._cacheMenuBarColor = SystemColors.CreateInstance(SystemResourceKeyID.MenuBarColor);
				}
				return SystemColors._cacheMenuBarColor;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600265D RID: 9821 RVA: 0x0018B7DF File Offset: 0x0018A7DF
		public static ResourceKey MenuHighlightColorKey
		{
			get
			{
				if (SystemColors._cacheMenuHighlightColor == null)
				{
					SystemColors._cacheMenuHighlightColor = SystemColors.CreateInstance(SystemResourceKeyID.MenuHighlightColor);
				}
				return SystemColors._cacheMenuHighlightColor;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x0600265E RID: 9822 RVA: 0x0018B7F9 File Offset: 0x0018A7F9
		public static ResourceKey MenuTextColorKey
		{
			get
			{
				if (SystemColors._cacheMenuTextColor == null)
				{
					SystemColors._cacheMenuTextColor = SystemColors.CreateInstance(SystemResourceKeyID.MenuTextColor);
				}
				return SystemColors._cacheMenuTextColor;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x0600265F RID: 9823 RVA: 0x0018B813 File Offset: 0x0018A813
		public static ResourceKey ScrollBarColorKey
		{
			get
			{
				if (SystemColors._cacheScrollBarColor == null)
				{
					SystemColors._cacheScrollBarColor = SystemColors.CreateInstance(SystemResourceKeyID.ScrollBarColor);
				}
				return SystemColors._cacheScrollBarColor;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002660 RID: 9824 RVA: 0x0018B82D File Offset: 0x0018A82D
		public static ResourceKey WindowColorKey
		{
			get
			{
				if (SystemColors._cacheWindowColor == null)
				{
					SystemColors._cacheWindowColor = SystemColors.CreateInstance(SystemResourceKeyID.WindowColor);
				}
				return SystemColors._cacheWindowColor;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x0018B847 File Offset: 0x0018A847
		public static ResourceKey WindowFrameColorKey
		{
			get
			{
				if (SystemColors._cacheWindowFrameColor == null)
				{
					SystemColors._cacheWindowFrameColor = SystemColors.CreateInstance(SystemResourceKeyID.WindowFrameColor);
				}
				return SystemColors._cacheWindowFrameColor;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06002662 RID: 9826 RVA: 0x0018B861 File Offset: 0x0018A861
		public static ResourceKey WindowTextColorKey
		{
			get
			{
				if (SystemColors._cacheWindowTextColor == null)
				{
					SystemColors._cacheWindowTextColor = SystemColors.CreateInstance(SystemResourceKeyID.WindowTextColor);
				}
				return SystemColors._cacheWindowTextColor;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x0018B87B File Offset: 0x0018A87B
		public static SolidColorBrush ActiveBorderBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ActiveBorder);
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002664 RID: 9828 RVA: 0x0018B883 File Offset: 0x0018A883
		public static SolidColorBrush ActiveCaptionBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ActiveCaption);
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002665 RID: 9829 RVA: 0x0018B88B File Offset: 0x0018A88B
		public static SolidColorBrush ActiveCaptionTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ActiveCaptionText);
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002666 RID: 9830 RVA: 0x0018B893 File Offset: 0x0018A893
		public static SolidColorBrush AppWorkspaceBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.AppWorkspace);
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002667 RID: 9831 RVA: 0x0018B89B File Offset: 0x0018A89B
		public static SolidColorBrush ControlBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Control);
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002668 RID: 9832 RVA: 0x0018B8A3 File Offset: 0x0018A8A3
		public static SolidColorBrush ControlDarkBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlDark);
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002669 RID: 9833 RVA: 0x0018B8AB File Offset: 0x0018A8AB
		public static SolidColorBrush ControlDarkDarkBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlDarkDark);
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x0600266A RID: 9834 RVA: 0x0018B8B3 File Offset: 0x0018A8B3
		public static SolidColorBrush ControlLightBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlLight);
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x0600266B RID: 9835 RVA: 0x0018B8BB File Offset: 0x0018A8BB
		public static SolidColorBrush ControlLightLightBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlLightLight);
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x0600266C RID: 9836 RVA: 0x0018B8C3 File Offset: 0x0018A8C3
		public static SolidColorBrush ControlTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlText);
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x0600266D RID: 9837 RVA: 0x0018B8CC File Offset: 0x0018A8CC
		public static SolidColorBrush DesktopBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Desktop);
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x0600266E RID: 9838 RVA: 0x0018B8D5 File Offset: 0x0018A8D5
		public static SolidColorBrush GradientActiveCaptionBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.GradientActiveCaption);
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x0600266F RID: 9839 RVA: 0x0018B8DE File Offset: 0x0018A8DE
		public static SolidColorBrush GradientInactiveCaptionBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.GradientInactiveCaption);
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002670 RID: 9840 RVA: 0x0018B8E7 File Offset: 0x0018A8E7
		public static SolidColorBrush GrayTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.GrayText);
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002671 RID: 9841 RVA: 0x0018B8F0 File Offset: 0x0018A8F0
		public static SolidColorBrush HighlightBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Highlight);
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06002672 RID: 9842 RVA: 0x0018B8F9 File Offset: 0x0018A8F9
		public static SolidColorBrush HighlightTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.HighlightText);
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002673 RID: 9843 RVA: 0x0018B902 File Offset: 0x0018A902
		public static SolidColorBrush HotTrackBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.HotTrack);
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002674 RID: 9844 RVA: 0x0018B90B File Offset: 0x0018A90B
		public static SolidColorBrush InactiveBorderBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.InactiveBorder);
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002675 RID: 9845 RVA: 0x0018B914 File Offset: 0x0018A914
		public static SolidColorBrush InactiveCaptionBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.InactiveCaption);
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002676 RID: 9846 RVA: 0x0018B91D File Offset: 0x0018A91D
		public static SolidColorBrush InactiveCaptionTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.InactiveCaptionText);
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002677 RID: 9847 RVA: 0x0018B926 File Offset: 0x0018A926
		public static SolidColorBrush InfoBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Info);
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x0018B92F File Offset: 0x0018A92F
		public static SolidColorBrush InfoTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.InfoText);
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002679 RID: 9849 RVA: 0x0018B938 File Offset: 0x0018A938
		public static SolidColorBrush MenuBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Menu);
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x0600267A RID: 9850 RVA: 0x0018B941 File Offset: 0x0018A941
		public static SolidColorBrush MenuBarBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.MenuBar);
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600267B RID: 9851 RVA: 0x0018B94A File Offset: 0x0018A94A
		public static SolidColorBrush MenuHighlightBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.MenuHighlight);
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x0600267C RID: 9852 RVA: 0x0018B953 File Offset: 0x0018A953
		public static SolidColorBrush MenuTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.MenuText);
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x0600267D RID: 9853 RVA: 0x0018B95C File Offset: 0x0018A95C
		public static SolidColorBrush ScrollBarBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ScrollBar);
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x0018B965 File Offset: 0x0018A965
		public static SolidColorBrush WindowBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Window);
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x0600267F RID: 9855 RVA: 0x0018B96E File Offset: 0x0018A96E
		public static SolidColorBrush WindowFrameBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.WindowFrame);
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06002680 RID: 9856 RVA: 0x0018B977 File Offset: 0x0018A977
		public static SolidColorBrush WindowTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.WindowText);
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002681 RID: 9857 RVA: 0x0018B980 File Offset: 0x0018A980
		public static SolidColorBrush InactiveSelectionHighlightBrush
		{
			get
			{
				if (SystemParameters.HighContrast)
				{
					return SystemColors.HighlightBrush;
				}
				return SystemColors.ControlBrush;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002682 RID: 9858 RVA: 0x0018B994 File Offset: 0x0018A994
		public static SolidColorBrush InactiveSelectionHighlightTextBrush
		{
			get
			{
				if (SystemParameters.HighContrast)
				{
					return SystemColors.HighlightTextBrush;
				}
				return SystemColors.ControlTextBrush;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002683 RID: 9859 RVA: 0x0018B9A8 File Offset: 0x0018A9A8
		public static ResourceKey ActiveBorderBrushKey
		{
			get
			{
				if (SystemColors._cacheActiveBorderBrush == null)
				{
					SystemColors._cacheActiveBorderBrush = SystemColors.CreateInstance(SystemResourceKeyID.ActiveBorderBrush);
				}
				return SystemColors._cacheActiveBorderBrush;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x0018B9C1 File Offset: 0x0018A9C1
		public static ResourceKey ActiveCaptionBrushKey
		{
			get
			{
				if (SystemColors._cacheActiveCaptionBrush == null)
				{
					SystemColors._cacheActiveCaptionBrush = SystemColors.CreateInstance(SystemResourceKeyID.ActiveCaptionBrush);
				}
				return SystemColors._cacheActiveCaptionBrush;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002685 RID: 9861 RVA: 0x0018B9DA File Offset: 0x0018A9DA
		public static ResourceKey ActiveCaptionTextBrushKey
		{
			get
			{
				if (SystemColors._cacheActiveCaptionTextBrush == null)
				{
					SystemColors._cacheActiveCaptionTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.ActiveCaptionTextBrush);
				}
				return SystemColors._cacheActiveCaptionTextBrush;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002686 RID: 9862 RVA: 0x0018B9F3 File Offset: 0x0018A9F3
		public static ResourceKey AppWorkspaceBrushKey
		{
			get
			{
				if (SystemColors._cacheAppWorkspaceBrush == null)
				{
					SystemColors._cacheAppWorkspaceBrush = SystemColors.CreateInstance(SystemResourceKeyID.AppWorkspaceBrush);
				}
				return SystemColors._cacheAppWorkspaceBrush;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002687 RID: 9863 RVA: 0x0018BA0C File Offset: 0x0018AA0C
		public static ResourceKey ControlBrushKey
		{
			get
			{
				if (SystemColors._cacheControlBrush == null)
				{
					SystemColors._cacheControlBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlBrush);
				}
				return SystemColors._cacheControlBrush;
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002688 RID: 9864 RVA: 0x0018BA25 File Offset: 0x0018AA25
		public static ResourceKey ControlDarkBrushKey
		{
			get
			{
				if (SystemColors._cacheControlDarkBrush == null)
				{
					SystemColors._cacheControlDarkBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlDarkBrush);
				}
				return SystemColors._cacheControlDarkBrush;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002689 RID: 9865 RVA: 0x0018BA3E File Offset: 0x0018AA3E
		public static ResourceKey ControlDarkDarkBrushKey
		{
			get
			{
				if (SystemColors._cacheControlDarkDarkBrush == null)
				{
					SystemColors._cacheControlDarkDarkBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlDarkDarkBrush);
				}
				return SystemColors._cacheControlDarkDarkBrush;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x0600268A RID: 9866 RVA: 0x0018BA57 File Offset: 0x0018AA57
		public static ResourceKey ControlLightBrushKey
		{
			get
			{
				if (SystemColors._cacheControlLightBrush == null)
				{
					SystemColors._cacheControlLightBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlLightBrush);
				}
				return SystemColors._cacheControlLightBrush;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600268B RID: 9867 RVA: 0x0018BA70 File Offset: 0x0018AA70
		public static ResourceKey ControlLightLightBrushKey
		{
			get
			{
				if (SystemColors._cacheControlLightLightBrush == null)
				{
					SystemColors._cacheControlLightLightBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlLightLightBrush);
				}
				return SystemColors._cacheControlLightLightBrush;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x0600268C RID: 9868 RVA: 0x0018BA8A File Offset: 0x0018AA8A
		public static ResourceKey ControlTextBrushKey
		{
			get
			{
				if (SystemColors._cacheControlTextBrush == null)
				{
					SystemColors._cacheControlTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlTextBrush);
				}
				return SystemColors._cacheControlTextBrush;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x0600268D RID: 9869 RVA: 0x0018BAA4 File Offset: 0x0018AAA4
		public static ResourceKey DesktopBrushKey
		{
			get
			{
				if (SystemColors._cacheDesktopBrush == null)
				{
					SystemColors._cacheDesktopBrush = SystemColors.CreateInstance(SystemResourceKeyID.DesktopBrush);
				}
				return SystemColors._cacheDesktopBrush;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x0600268E RID: 9870 RVA: 0x0018BABE File Offset: 0x0018AABE
		public static ResourceKey GradientActiveCaptionBrushKey
		{
			get
			{
				if (SystemColors._cacheGradientActiveCaptionBrush == null)
				{
					SystemColors._cacheGradientActiveCaptionBrush = SystemColors.CreateInstance(SystemResourceKeyID.GradientActiveCaptionBrush);
				}
				return SystemColors._cacheGradientActiveCaptionBrush;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x0600268F RID: 9871 RVA: 0x0018BAD8 File Offset: 0x0018AAD8
		public static ResourceKey GradientInactiveCaptionBrushKey
		{
			get
			{
				if (SystemColors._cacheGradientInactiveCaptionBrush == null)
				{
					SystemColors._cacheGradientInactiveCaptionBrush = SystemColors.CreateInstance(SystemResourceKeyID.GradientInactiveCaptionBrush);
				}
				return SystemColors._cacheGradientInactiveCaptionBrush;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002690 RID: 9872 RVA: 0x0018BAF2 File Offset: 0x0018AAF2
		public static ResourceKey GrayTextBrushKey
		{
			get
			{
				if (SystemColors._cacheGrayTextBrush == null)
				{
					SystemColors._cacheGrayTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.GrayTextBrush);
				}
				return SystemColors._cacheGrayTextBrush;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002691 RID: 9873 RVA: 0x0018BB0C File Offset: 0x0018AB0C
		public static ResourceKey HighlightBrushKey
		{
			get
			{
				if (SystemColors._cacheHighlightBrush == null)
				{
					SystemColors._cacheHighlightBrush = SystemColors.CreateInstance(SystemResourceKeyID.HighlightBrush);
				}
				return SystemColors._cacheHighlightBrush;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002692 RID: 9874 RVA: 0x0018BB26 File Offset: 0x0018AB26
		public static ResourceKey HighlightTextBrushKey
		{
			get
			{
				if (SystemColors._cacheHighlightTextBrush == null)
				{
					SystemColors._cacheHighlightTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.HighlightTextBrush);
				}
				return SystemColors._cacheHighlightTextBrush;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002693 RID: 9875 RVA: 0x0018BB40 File Offset: 0x0018AB40
		public static ResourceKey HotTrackBrushKey
		{
			get
			{
				if (SystemColors._cacheHotTrackBrush == null)
				{
					SystemColors._cacheHotTrackBrush = SystemColors.CreateInstance(SystemResourceKeyID.HotTrackBrush);
				}
				return SystemColors._cacheHotTrackBrush;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002694 RID: 9876 RVA: 0x0018BB5A File Offset: 0x0018AB5A
		public static ResourceKey InactiveBorderBrushKey
		{
			get
			{
				if (SystemColors._cacheInactiveBorderBrush == null)
				{
					SystemColors._cacheInactiveBorderBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveBorderBrush);
				}
				return SystemColors._cacheInactiveBorderBrush;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002695 RID: 9877 RVA: 0x0018BB74 File Offset: 0x0018AB74
		public static ResourceKey InactiveCaptionBrushKey
		{
			get
			{
				if (SystemColors._cacheInactiveCaptionBrush == null)
				{
					SystemColors._cacheInactiveCaptionBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveCaptionBrush);
				}
				return SystemColors._cacheInactiveCaptionBrush;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002696 RID: 9878 RVA: 0x0018BB8E File Offset: 0x0018AB8E
		public static ResourceKey InactiveCaptionTextBrushKey
		{
			get
			{
				if (SystemColors._cacheInactiveCaptionTextBrush == null)
				{
					SystemColors._cacheInactiveCaptionTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveCaptionTextBrush);
				}
				return SystemColors._cacheInactiveCaptionTextBrush;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002697 RID: 9879 RVA: 0x0018BBA8 File Offset: 0x0018ABA8
		public static ResourceKey InfoBrushKey
		{
			get
			{
				if (SystemColors._cacheInfoBrush == null)
				{
					SystemColors._cacheInfoBrush = SystemColors.CreateInstance(SystemResourceKeyID.InfoBrush);
				}
				return SystemColors._cacheInfoBrush;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002698 RID: 9880 RVA: 0x0018BBC2 File Offset: 0x0018ABC2
		public static ResourceKey InfoTextBrushKey
		{
			get
			{
				if (SystemColors._cacheInfoTextBrush == null)
				{
					SystemColors._cacheInfoTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.InfoTextBrush);
				}
				return SystemColors._cacheInfoTextBrush;
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002699 RID: 9881 RVA: 0x0018BBDC File Offset: 0x0018ABDC
		public static ResourceKey MenuBrushKey
		{
			get
			{
				if (SystemColors._cacheMenuBrush == null)
				{
					SystemColors._cacheMenuBrush = SystemColors.CreateInstance(SystemResourceKeyID.MenuBrush);
				}
				return SystemColors._cacheMenuBrush;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x0600269A RID: 9882 RVA: 0x0018BBF6 File Offset: 0x0018ABF6
		public static ResourceKey MenuBarBrushKey
		{
			get
			{
				if (SystemColors._cacheMenuBarBrush == null)
				{
					SystemColors._cacheMenuBarBrush = SystemColors.CreateInstance(SystemResourceKeyID.MenuBarBrush);
				}
				return SystemColors._cacheMenuBarBrush;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x0600269B RID: 9883 RVA: 0x0018BC10 File Offset: 0x0018AC10
		public static ResourceKey MenuHighlightBrushKey
		{
			get
			{
				if (SystemColors._cacheMenuHighlightBrush == null)
				{
					SystemColors._cacheMenuHighlightBrush = SystemColors.CreateInstance(SystemResourceKeyID.MenuHighlightBrush);
				}
				return SystemColors._cacheMenuHighlightBrush;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x0600269C RID: 9884 RVA: 0x0018BC2A File Offset: 0x0018AC2A
		public static ResourceKey MenuTextBrushKey
		{
			get
			{
				if (SystemColors._cacheMenuTextBrush == null)
				{
					SystemColors._cacheMenuTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.MenuTextBrush);
				}
				return SystemColors._cacheMenuTextBrush;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x0600269D RID: 9885 RVA: 0x0018BC44 File Offset: 0x0018AC44
		public static ResourceKey ScrollBarBrushKey
		{
			get
			{
				if (SystemColors._cacheScrollBarBrush == null)
				{
					SystemColors._cacheScrollBarBrush = SystemColors.CreateInstance(SystemResourceKeyID.ScrollBarBrush);
				}
				return SystemColors._cacheScrollBarBrush;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x0600269E RID: 9886 RVA: 0x0018BC5E File Offset: 0x0018AC5E
		public static ResourceKey WindowBrushKey
		{
			get
			{
				if (SystemColors._cacheWindowBrush == null)
				{
					SystemColors._cacheWindowBrush = SystemColors.CreateInstance(SystemResourceKeyID.WindowBrush);
				}
				return SystemColors._cacheWindowBrush;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x0600269F RID: 9887 RVA: 0x0018BC78 File Offset: 0x0018AC78
		public static ResourceKey WindowFrameBrushKey
		{
			get
			{
				if (SystemColors._cacheWindowFrameBrush == null)
				{
					SystemColors._cacheWindowFrameBrush = SystemColors.CreateInstance(SystemResourceKeyID.WindowFrameBrush);
				}
				return SystemColors._cacheWindowFrameBrush;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060026A0 RID: 9888 RVA: 0x0018BC92 File Offset: 0x0018AC92
		public static ResourceKey WindowTextBrushKey
		{
			get
			{
				if (SystemColors._cacheWindowTextBrush == null)
				{
					SystemColors._cacheWindowTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.WindowTextBrush);
				}
				return SystemColors._cacheWindowTextBrush;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060026A1 RID: 9889 RVA: 0x0018BCAC File Offset: 0x0018ACAC
		public static ResourceKey InactiveSelectionHighlightBrushKey
		{
			get
			{
				if (FrameworkCompatibilityPreferences.GetAreInactiveSelectionHighlightBrushKeysSupported())
				{
					if (SystemColors._cacheInactiveSelectionHighlightBrush == null)
					{
						SystemColors._cacheInactiveSelectionHighlightBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveSelectionHighlightBrush);
					}
					return SystemColors._cacheInactiveSelectionHighlightBrush;
				}
				return SystemColors.ControlBrushKey;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060026A2 RID: 9890 RVA: 0x0018BCD6 File Offset: 0x0018ACD6
		public static ResourceKey InactiveSelectionHighlightTextBrushKey
		{
			get
			{
				if (FrameworkCompatibilityPreferences.GetAreInactiveSelectionHighlightBrushKeysSupported())
				{
					if (SystemColors._cacheInactiveSelectionHighlightTextBrush == null)
					{
						SystemColors._cacheInactiveSelectionHighlightTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveSelectionHighlightTextBrush);
					}
					return SystemColors._cacheInactiveSelectionHighlightTextBrush;
				}
				return SystemColors.ControlTextBrushKey;
			}
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0018BD00 File Offset: 0x0018AD00
		internal static bool InvalidateCache()
		{
			bool flag = SystemResources.ClearBitArray(SystemColors._colorCacheValid);
			bool flag2 = SystemResources.ClearBitArray(SystemColors._brushCacheValid);
			return flag || flag2;
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0018BD24 File Offset: 0x0018AD24
		private static int Encode(int alpha, int red, int green, int blue)
		{
			return red << 16 | green << 8 | blue | alpha << 24;
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0018BD35 File Offset: 0x0018AD35
		private static int FromWin32Value(int value)
		{
			return SystemColors.Encode(255, value & 255, value >> 8 & 255, value >> 16 & 255);
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x0018BD5C File Offset: 0x0018AD5C
		private static Color GetSystemColor(SystemColors.CacheSlot slot)
		{
			BitArray colorCacheValid = SystemColors._colorCacheValid;
			Color color;
			lock (colorCacheValid)
			{
				while (!SystemColors._colorCacheValid[(int)slot])
				{
					SystemColors._colorCacheValid[(int)slot] = true;
					uint num = (uint)SystemColors.FromWin32Value(SafeNativeMethods.GetSysColor(SystemColors.SlotToFlag(slot)));
					color = Color.FromArgb((byte)((num & 4278190080U) >> 24), (byte)((num & 16711680U) >> 16), (byte)((num & 65280U) >> 8), (byte)(num & 255U));
					SystemColors._colorCache[(int)slot] = color;
				}
				color = SystemColors._colorCache[(int)slot];
			}
			return color;
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x0018BE08 File Offset: 0x0018AE08
		private static SolidColorBrush MakeBrush(SystemColors.CacheSlot slot)
		{
			BitArray brushCacheValid = SystemColors._brushCacheValid;
			SolidColorBrush solidColorBrush;
			lock (brushCacheValid)
			{
				while (!SystemColors._brushCacheValid[(int)slot])
				{
					SystemColors._brushCacheValid[(int)slot] = true;
					solidColorBrush = new SolidColorBrush(SystemColors.GetSystemColor(slot));
					solidColorBrush.Freeze();
					SystemColors._brushCache[(int)slot] = solidColorBrush;
				}
				solidColorBrush = SystemColors._brushCache[(int)slot];
			}
			return solidColorBrush;
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x0018BE80 File Offset: 0x0018AE80
		private static int SlotToFlag(SystemColors.CacheSlot slot)
		{
			switch (slot)
			{
			case SystemColors.CacheSlot.ActiveBorder:
				return 10;
			case SystemColors.CacheSlot.ActiveCaption:
				return 2;
			case SystemColors.CacheSlot.ActiveCaptionText:
				return 9;
			case SystemColors.CacheSlot.AppWorkspace:
				return 12;
			case SystemColors.CacheSlot.Control:
				return 15;
			case SystemColors.CacheSlot.ControlDark:
				return 16;
			case SystemColors.CacheSlot.ControlDarkDark:
				return 21;
			case SystemColors.CacheSlot.ControlLight:
				return 22;
			case SystemColors.CacheSlot.ControlLightLight:
				return 20;
			case SystemColors.CacheSlot.ControlText:
				return 18;
			case SystemColors.CacheSlot.Desktop:
				return 1;
			case SystemColors.CacheSlot.GradientActiveCaption:
				return 27;
			case SystemColors.CacheSlot.GradientInactiveCaption:
				return 28;
			case SystemColors.CacheSlot.GrayText:
				return 17;
			case SystemColors.CacheSlot.Highlight:
				return 13;
			case SystemColors.CacheSlot.HighlightText:
				return 14;
			case SystemColors.CacheSlot.HotTrack:
				return 26;
			case SystemColors.CacheSlot.InactiveBorder:
				return 11;
			case SystemColors.CacheSlot.InactiveCaption:
				return 3;
			case SystemColors.CacheSlot.InactiveCaptionText:
				return 19;
			case SystemColors.CacheSlot.Info:
				return 24;
			case SystemColors.CacheSlot.InfoText:
				return 23;
			case SystemColors.CacheSlot.Menu:
				return 4;
			case SystemColors.CacheSlot.MenuBar:
				return 30;
			case SystemColors.CacheSlot.MenuHighlight:
				return 29;
			case SystemColors.CacheSlot.MenuText:
				return 7;
			case SystemColors.CacheSlot.ScrollBar:
				return 0;
			case SystemColors.CacheSlot.Window:
				return 5;
			case SystemColors.CacheSlot.WindowFrame:
				return 6;
			case SystemColors.CacheSlot.WindowText:
				return 8;
			default:
				return 0;
			}
		}

		// Token: 0x040011F3 RID: 4595
		private const int AlphaShift = 24;

		// Token: 0x040011F4 RID: 4596
		private const int RedShift = 16;

		// Token: 0x040011F5 RID: 4597
		private const int GreenShift = 8;

		// Token: 0x040011F6 RID: 4598
		private const int BlueShift = 0;

		// Token: 0x040011F7 RID: 4599
		private const int Win32RedShift = 0;

		// Token: 0x040011F8 RID: 4600
		private const int Win32GreenShift = 8;

		// Token: 0x040011F9 RID: 4601
		private const int Win32BlueShift = 16;

		// Token: 0x040011FA RID: 4602
		private static BitArray _colorCacheValid = new BitArray(30);

		// Token: 0x040011FB RID: 4603
		private static Color[] _colorCache = new Color[30];

		// Token: 0x040011FC RID: 4604
		private static BitArray _brushCacheValid = new BitArray(30);

		// Token: 0x040011FD RID: 4605
		private static SolidColorBrush[] _brushCache = new SolidColorBrush[30];

		// Token: 0x040011FE RID: 4606
		private static SystemResourceKey _cacheActiveBorderBrush;

		// Token: 0x040011FF RID: 4607
		private static SystemResourceKey _cacheActiveCaptionBrush;

		// Token: 0x04001200 RID: 4608
		private static SystemResourceKey _cacheActiveCaptionTextBrush;

		// Token: 0x04001201 RID: 4609
		private static SystemResourceKey _cacheAppWorkspaceBrush;

		// Token: 0x04001202 RID: 4610
		private static SystemResourceKey _cacheControlBrush;

		// Token: 0x04001203 RID: 4611
		private static SystemResourceKey _cacheControlDarkBrush;

		// Token: 0x04001204 RID: 4612
		private static SystemResourceKey _cacheControlDarkDarkBrush;

		// Token: 0x04001205 RID: 4613
		private static SystemResourceKey _cacheControlLightBrush;

		// Token: 0x04001206 RID: 4614
		private static SystemResourceKey _cacheControlLightLightBrush;

		// Token: 0x04001207 RID: 4615
		private static SystemResourceKey _cacheControlTextBrush;

		// Token: 0x04001208 RID: 4616
		private static SystemResourceKey _cacheDesktopBrush;

		// Token: 0x04001209 RID: 4617
		private static SystemResourceKey _cacheGradientActiveCaptionBrush;

		// Token: 0x0400120A RID: 4618
		private static SystemResourceKey _cacheGradientInactiveCaptionBrush;

		// Token: 0x0400120B RID: 4619
		private static SystemResourceKey _cacheGrayTextBrush;

		// Token: 0x0400120C RID: 4620
		private static SystemResourceKey _cacheHighlightBrush;

		// Token: 0x0400120D RID: 4621
		private static SystemResourceKey _cacheHighlightTextBrush;

		// Token: 0x0400120E RID: 4622
		private static SystemResourceKey _cacheHotTrackBrush;

		// Token: 0x0400120F RID: 4623
		private static SystemResourceKey _cacheInactiveBorderBrush;

		// Token: 0x04001210 RID: 4624
		private static SystemResourceKey _cacheInactiveCaptionBrush;

		// Token: 0x04001211 RID: 4625
		private static SystemResourceKey _cacheInactiveCaptionTextBrush;

		// Token: 0x04001212 RID: 4626
		private static SystemResourceKey _cacheInfoBrush;

		// Token: 0x04001213 RID: 4627
		private static SystemResourceKey _cacheInfoTextBrush;

		// Token: 0x04001214 RID: 4628
		private static SystemResourceKey _cacheMenuBrush;

		// Token: 0x04001215 RID: 4629
		private static SystemResourceKey _cacheMenuBarBrush;

		// Token: 0x04001216 RID: 4630
		private static SystemResourceKey _cacheMenuHighlightBrush;

		// Token: 0x04001217 RID: 4631
		private static SystemResourceKey _cacheMenuTextBrush;

		// Token: 0x04001218 RID: 4632
		private static SystemResourceKey _cacheScrollBarBrush;

		// Token: 0x04001219 RID: 4633
		private static SystemResourceKey _cacheWindowBrush;

		// Token: 0x0400121A RID: 4634
		private static SystemResourceKey _cacheWindowFrameBrush;

		// Token: 0x0400121B RID: 4635
		private static SystemResourceKey _cacheWindowTextBrush;

		// Token: 0x0400121C RID: 4636
		private static SystemResourceKey _cacheInactiveSelectionHighlightBrush;

		// Token: 0x0400121D RID: 4637
		private static SystemResourceKey _cacheInactiveSelectionHighlightTextBrush;

		// Token: 0x0400121E RID: 4638
		private static SystemResourceKey _cacheActiveBorderColor;

		// Token: 0x0400121F RID: 4639
		private static SystemResourceKey _cacheActiveCaptionColor;

		// Token: 0x04001220 RID: 4640
		private static SystemResourceKey _cacheActiveCaptionTextColor;

		// Token: 0x04001221 RID: 4641
		private static SystemResourceKey _cacheAppWorkspaceColor;

		// Token: 0x04001222 RID: 4642
		private static SystemResourceKey _cacheControlColor;

		// Token: 0x04001223 RID: 4643
		private static SystemResourceKey _cacheControlDarkColor;

		// Token: 0x04001224 RID: 4644
		private static SystemResourceKey _cacheControlDarkDarkColor;

		// Token: 0x04001225 RID: 4645
		private static SystemResourceKey _cacheControlLightColor;

		// Token: 0x04001226 RID: 4646
		private static SystemResourceKey _cacheControlLightLightColor;

		// Token: 0x04001227 RID: 4647
		private static SystemResourceKey _cacheControlTextColor;

		// Token: 0x04001228 RID: 4648
		private static SystemResourceKey _cacheDesktopColor;

		// Token: 0x04001229 RID: 4649
		private static SystemResourceKey _cacheGradientActiveCaptionColor;

		// Token: 0x0400122A RID: 4650
		private static SystemResourceKey _cacheGradientInactiveCaptionColor;

		// Token: 0x0400122B RID: 4651
		private static SystemResourceKey _cacheGrayTextColor;

		// Token: 0x0400122C RID: 4652
		private static SystemResourceKey _cacheHighlightColor;

		// Token: 0x0400122D RID: 4653
		private static SystemResourceKey _cacheHighlightTextColor;

		// Token: 0x0400122E RID: 4654
		private static SystemResourceKey _cacheHotTrackColor;

		// Token: 0x0400122F RID: 4655
		private static SystemResourceKey _cacheInactiveBorderColor;

		// Token: 0x04001230 RID: 4656
		private static SystemResourceKey _cacheInactiveCaptionColor;

		// Token: 0x04001231 RID: 4657
		private static SystemResourceKey _cacheInactiveCaptionTextColor;

		// Token: 0x04001232 RID: 4658
		private static SystemResourceKey _cacheInfoColor;

		// Token: 0x04001233 RID: 4659
		private static SystemResourceKey _cacheInfoTextColor;

		// Token: 0x04001234 RID: 4660
		private static SystemResourceKey _cacheMenuColor;

		// Token: 0x04001235 RID: 4661
		private static SystemResourceKey _cacheMenuBarColor;

		// Token: 0x04001236 RID: 4662
		private static SystemResourceKey _cacheMenuHighlightColor;

		// Token: 0x04001237 RID: 4663
		private static SystemResourceKey _cacheMenuTextColor;

		// Token: 0x04001238 RID: 4664
		private static SystemResourceKey _cacheScrollBarColor;

		// Token: 0x04001239 RID: 4665
		private static SystemResourceKey _cacheWindowColor;

		// Token: 0x0400123A RID: 4666
		private static SystemResourceKey _cacheWindowFrameColor;

		// Token: 0x0400123B RID: 4667
		private static SystemResourceKey _cacheWindowTextColor;

		// Token: 0x02000A8E RID: 2702
		private enum CacheSlot
		{
			// Token: 0x040041C0 RID: 16832
			ActiveBorder,
			// Token: 0x040041C1 RID: 16833
			ActiveCaption,
			// Token: 0x040041C2 RID: 16834
			ActiveCaptionText,
			// Token: 0x040041C3 RID: 16835
			AppWorkspace,
			// Token: 0x040041C4 RID: 16836
			Control,
			// Token: 0x040041C5 RID: 16837
			ControlDark,
			// Token: 0x040041C6 RID: 16838
			ControlDarkDark,
			// Token: 0x040041C7 RID: 16839
			ControlLight,
			// Token: 0x040041C8 RID: 16840
			ControlLightLight,
			// Token: 0x040041C9 RID: 16841
			ControlText,
			// Token: 0x040041CA RID: 16842
			Desktop,
			// Token: 0x040041CB RID: 16843
			GradientActiveCaption,
			// Token: 0x040041CC RID: 16844
			GradientInactiveCaption,
			// Token: 0x040041CD RID: 16845
			GrayText,
			// Token: 0x040041CE RID: 16846
			Highlight,
			// Token: 0x040041CF RID: 16847
			HighlightText,
			// Token: 0x040041D0 RID: 16848
			HotTrack,
			// Token: 0x040041D1 RID: 16849
			InactiveBorder,
			// Token: 0x040041D2 RID: 16850
			InactiveCaption,
			// Token: 0x040041D3 RID: 16851
			InactiveCaptionText,
			// Token: 0x040041D4 RID: 16852
			Info,
			// Token: 0x040041D5 RID: 16853
			InfoText,
			// Token: 0x040041D6 RID: 16854
			Menu,
			// Token: 0x040041D7 RID: 16855
			MenuBar,
			// Token: 0x040041D8 RID: 16856
			MenuHighlight,
			// Token: 0x040041D9 RID: 16857
			MenuText,
			// Token: 0x040041DA RID: 16858
			ScrollBar,
			// Token: 0x040041DB RID: 16859
			Window,
			// Token: 0x040041DC RID: 16860
			WindowFrame,
			// Token: 0x040041DD RID: 16861
			WindowText,
			// Token: 0x040041DE RID: 16862
			NumSlots
		}
	}
}
