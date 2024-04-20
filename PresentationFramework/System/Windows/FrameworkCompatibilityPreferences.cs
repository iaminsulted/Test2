using System;
using System.Collections.Specialized;
using System.Configuration;

namespace System.Windows
{
	// Token: 0x02000365 RID: 869
	public static class FrameworkCompatibilityPreferences
	{
		// Token: 0x060020BE RID: 8382 RVA: 0x00176394 File Offset: 0x00175394
		static FrameworkCompatibilityPreferences()
		{
			FrameworkCompatibilityPreferences._targetsDesktop_V4_0 = false;
			NameValueCollection nameValueCollection = null;
			try
			{
				nameValueCollection = ConfigurationManager.AppSettings;
			}
			catch (ConfigurationErrorsException)
			{
			}
			if (nameValueCollection != null)
			{
				FrameworkCompatibilityPreferences.SetUseSetWindowPosForTopmostWindowsFromAppSettings(nameValueCollection);
				FrameworkCompatibilityPreferences.SetVSP45CompatFromAppSettings(nameValueCollection);
				FrameworkCompatibilityPreferences.SetScrollingTraceFromAppSettings(nameValueCollection);
				FrameworkCompatibilityPreferences.SetShouldThrowOnCopyOrCutFailuresFromAppSettings(nameValueCollection);
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060020BF RID: 8383 RVA: 0x00176408 File Offset: 0x00175408
		internal static bool TargetsDesktop_V4_0
		{
			get
			{
				return FrameworkCompatibilityPreferences._targetsDesktop_V4_0;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060020C0 RID: 8384 RVA: 0x0017640F File Offset: 0x0017540F
		// (set) Token: 0x060020C1 RID: 8385 RVA: 0x00176418 File Offset: 0x00175418
		public static bool AreInactiveSelectionHighlightBrushKeysSupported
		{
			get
			{
				return FrameworkCompatibilityPreferences._areInactiveSelectionHighlightBrushKeysSupported;
			}
			set
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					if (FrameworkCompatibilityPreferences._isSealed)
					{
						throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
						{
							"AreInactiveSelectionHighlightBrushKeysSupported",
							"FrameworkCompatibilityPreferences"
						}));
					}
					FrameworkCompatibilityPreferences._areInactiveSelectionHighlightBrushKeysSupported = value;
				}
			}
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x00176484 File Offset: 0x00175484
		internal static bool GetAreInactiveSelectionHighlightBrushKeysSupported()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.AreInactiveSelectionHighlightBrushKeysSupported;
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060020C3 RID: 8387 RVA: 0x00176490 File Offset: 0x00175490
		// (set) Token: 0x060020C4 RID: 8388 RVA: 0x00176498 File Offset: 0x00175498
		public static bool KeepTextBoxDisplaySynchronizedWithTextProperty
		{
			get
			{
				return FrameworkCompatibilityPreferences._keepTextBoxDisplaySynchronizedWithTextProperty;
			}
			set
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					if (FrameworkCompatibilityPreferences._isSealed)
					{
						throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
						{
							"AextBoxDisplaysText",
							"FrameworkCompatibilityPreferences"
						}));
					}
					FrameworkCompatibilityPreferences._keepTextBoxDisplaySynchronizedWithTextProperty = value;
				}
			}
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00176504 File Offset: 0x00175504
		internal static bool GetKeepTextBoxDisplaySynchronizedWithTextProperty()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty;
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060020C6 RID: 8390 RVA: 0x00176510 File Offset: 0x00175510
		// (set) Token: 0x060020C7 RID: 8391 RVA: 0x00176518 File Offset: 0x00175518
		internal static bool UseSetWindowPosForTopmostWindows
		{
			get
			{
				return FrameworkCompatibilityPreferences._useSetWindowPosForTopmostWindows;
			}
			set
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					if (FrameworkCompatibilityPreferences._isSealed)
					{
						throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
						{
							"UseSetWindowPosForTopmostWindows",
							"FrameworkCompatibilityPreferences"
						}));
					}
					FrameworkCompatibilityPreferences._useSetWindowPosForTopmostWindows = value;
				}
			}
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x00176584 File Offset: 0x00175584
		internal static bool GetUseSetWindowPosForTopmostWindows()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.UseSetWindowPosForTopmostWindows;
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x00176590 File Offset: 0x00175590
		private static void SetUseSetWindowPosForTopmostWindowsFromAppSettings(NameValueCollection appSettings)
		{
			bool useSetWindowPosForTopmostWindows;
			if (bool.TryParse(appSettings["UseSetWindowPosForTopmostWindows"], out useSetWindowPosForTopmostWindows))
			{
				FrameworkCompatibilityPreferences.UseSetWindowPosForTopmostWindows = useSetWindowPosForTopmostWindows;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x001765B7 File Offset: 0x001755B7
		// (set) Token: 0x060020CB RID: 8395 RVA: 0x001765C0 File Offset: 0x001755C0
		internal static bool VSP45Compat
		{
			get
			{
				return FrameworkCompatibilityPreferences._vsp45Compat;
			}
			set
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					if (FrameworkCompatibilityPreferences._isSealed)
					{
						throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
						{
							"IsVirtualizingStackPanel_45Compatible",
							"FrameworkCompatibilityPreferences"
						}));
					}
					FrameworkCompatibilityPreferences._vsp45Compat = value;
				}
			}
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x0017662C File Offset: 0x0017562C
		internal static bool GetVSP45Compat()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.VSP45Compat;
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x00176638 File Offset: 0x00175638
		private static void SetVSP45CompatFromAppSettings(NameValueCollection appSettings)
		{
			bool vsp45Compat;
			if (bool.TryParse(appSettings["IsVirtualizingStackPanel_45Compatible"], out vsp45Compat))
			{
				FrameworkCompatibilityPreferences.VSP45Compat = vsp45Compat;
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x0017665F File Offset: 0x0017565F
		internal static string GetScrollingTraceTarget()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences._scrollingTraceTarget;
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x0017666B File Offset: 0x0017566B
		internal static string GetScrollingTraceFile()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences._scrollingTraceFile;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00176677 File Offset: 0x00175677
		private static void SetScrollingTraceFromAppSettings(NameValueCollection appSettings)
		{
			FrameworkCompatibilityPreferences._scrollingTraceTarget = appSettings["ScrollingTraceTarget"];
			FrameworkCompatibilityPreferences._scrollingTraceFile = appSettings["ScrollingTraceFile"];
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060020D1 RID: 8401 RVA: 0x00176699 File Offset: 0x00175699
		// (set) Token: 0x060020D2 RID: 8402 RVA: 0x001766A0 File Offset: 0x001756A0
		public static bool ShouldThrowOnCopyOrCutFailure
		{
			get
			{
				return FrameworkCompatibilityPreferences._shouldThrowOnCopyOrCutFailure;
			}
			set
			{
				if (FrameworkCompatibilityPreferences._isSealed)
				{
					throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
					{
						"ShouldThrowOnCopyOrCutFailure",
						"FrameworkCompatibilityPreferences"
					}));
				}
				FrameworkCompatibilityPreferences._shouldThrowOnCopyOrCutFailure = value;
			}
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x001766D5 File Offset: 0x001756D5
		internal static bool GetShouldThrowOnCopyOrCutFailure()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.ShouldThrowOnCopyOrCutFailure;
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x001766E4 File Offset: 0x001756E4
		private static void SetShouldThrowOnCopyOrCutFailuresFromAppSettings(NameValueCollection appSettings)
		{
			bool shouldThrowOnCopyOrCutFailure;
			if (bool.TryParse(appSettings["ShouldThrowOnCopyOrCutFailure"], out shouldThrowOnCopyOrCutFailure))
			{
				FrameworkCompatibilityPreferences.ShouldThrowOnCopyOrCutFailure = shouldThrowOnCopyOrCutFailure;
			}
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x0017670C File Offset: 0x0017570C
		private static void Seal()
		{
			if (!FrameworkCompatibilityPreferences._isSealed)
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					FrameworkCompatibilityPreferences._isSealed = true;
				}
			}
		}

		// Token: 0x04000FF8 RID: 4088
		private static bool _targetsDesktop_V4_0;

		// Token: 0x04000FF9 RID: 4089
		private static bool _areInactiveSelectionHighlightBrushKeysSupported = true;

		// Token: 0x04000FFA RID: 4090
		private static bool _keepTextBoxDisplaySynchronizedWithTextProperty = true;

		// Token: 0x04000FFB RID: 4091
		private static bool _useSetWindowPosForTopmostWindows = false;

		// Token: 0x04000FFC RID: 4092
		private static bool _vsp45Compat = false;

		// Token: 0x04000FFD RID: 4093
		private static string _scrollingTraceTarget;

		// Token: 0x04000FFE RID: 4094
		private static string _scrollingTraceFile;

		// Token: 0x04000FFF RID: 4095
		private static bool _shouldThrowOnCopyOrCutFailure = false;

		// Token: 0x04001000 RID: 4096
		private static bool _isSealed;

		// Token: 0x04001001 RID: 4097
		private static object _lockObject = new object();
	}
}
