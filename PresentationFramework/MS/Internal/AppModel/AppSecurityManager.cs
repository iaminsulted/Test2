using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using MS.Internal.Documents.Application;
using MS.Internal.Utility;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x0200026D RID: 621
	internal static class AppSecurityManager
	{
		// Token: 0x060017FC RID: 6140 RVA: 0x0015FE40 File Offset: 0x0015EE40
		internal static void SafeLaunchBrowserDemandWhenUnsafe(Uri originatingUri, Uri destinationUri, bool fIsTopLevel)
		{
			if (AppSecurityManager.SafeLaunchBrowserOnlyIfPossible(originatingUri, destinationUri, fIsTopLevel) == LaunchResult.NotLaunched)
			{
				AppSecurityManager.UnsafeLaunchBrowser(destinationUri, null);
			}
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0015FE54 File Offset: 0x0015EE54
		internal static LaunchResult SafeLaunchBrowserOnlyIfPossible(Uri originatingUri, Uri destinationUri, bool fIsTopLevel)
		{
			return AppSecurityManager.SafeLaunchBrowserOnlyIfPossible(originatingUri, destinationUri, null, fIsTopLevel);
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x0015FE60 File Offset: 0x0015EE60
		internal static LaunchResult SafeLaunchBrowserOnlyIfPossible(Uri originatingUri, Uri destinationUri, string targetName, bool fIsTopLevel)
		{
			LaunchResult result = LaunchResult.NotLaunched;
			bool flag = destinationUri.Scheme == Uri.UriSchemeHttp || destinationUri.Scheme == Uri.UriSchemeHttps || destinationUri.IsFile;
			bool flag2 = string.Compare(destinationUri.Scheme, Uri.UriSchemeMailto, StringComparison.OrdinalIgnoreCase) == 0;
			if (!BrowserInteropHelper.IsInitialViewerNavigation && ((fIsTopLevel && flag) || flag2) && (!flag && flag2))
			{
				UnsafeNativeMethods.ShellExecute(new HandleRef(null, IntPtr.Zero), null, BindUriHelper.UriToString(destinationUri), null, null, 0);
				result = LaunchResult.Launched;
			}
			return result;
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x0015FEDC File Offset: 0x0015EEDC
		internal static void UnsafeLaunchBrowser(Uri uri, string targetFrame = null)
		{
			AppSecurityManager.ShellExecuteDefaultBrowser(uri);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x0015FEE4 File Offset: 0x0015EEE4
		internal static void ShellExecuteDefaultBrowser(Uri uri)
		{
			UnsafeNativeMethods.ShellExecuteInfo shellExecuteInfo = new UnsafeNativeMethods.ShellExecuteInfo();
			shellExecuteInfo.cbSize = Marshal.SizeOf<UnsafeNativeMethods.ShellExecuteInfo>(shellExecuteInfo);
			shellExecuteInfo.fMask = UnsafeNativeMethods.ShellExecuteFlags.SEE_MASK_FLAG_DDEWAIT;
			if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
			{
				shellExecuteInfo.fMask |= UnsafeNativeMethods.ShellExecuteFlags.SEE_MASK_CLASSNAME;
				shellExecuteInfo.lpClass = ".htm";
			}
			shellExecuteInfo.lpFile = uri.ToString();
			if (!UnsafeNativeMethods.ShellExecuteEx(shellExecuteInfo))
			{
				throw new InvalidOperationException(SR.Get("FailToLaunchDefaultBrowser"), new Win32Exception());
			}
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0015FF74 File Offset: 0x0015EF74
		private static string GetHeaders(Uri destinationUri)
		{
			string text = BindUriHelper.GetReferer(destinationUri);
			if (!string.IsNullOrEmpty(text))
			{
				text = "Referer: " + text + "\r\n";
			}
			return text;
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x0015FFA4 File Offset: 0x0015EFA4
		private static LaunchResult CanNavigateToUrlWithZoneCheck(Uri originatingUri, Uri destinationUri)
		{
			AppSecurityManager.EnsureSecurityManager();
			bool flag = UnsafeNativeMethods.CoInternetIsFeatureEnabled(1, 2) != 1;
			int num = AppSecurityManager.MapUrlToZone(destinationUri);
			Uri uri = null;
			if (Application.Current.MimeType != MimeType.Document)
			{
				uri = BrowserInteropHelper.Source;
			}
			else if (destinationUri.IsFile && Path.GetExtension(destinationUri.LocalPath).Equals(DocumentStream.XpsFileExtension, StringComparison.OrdinalIgnoreCase))
			{
				num = 3;
			}
			if (!(uri != null))
			{
				return LaunchResult.Launched;
			}
			int num2 = AppSecurityManager.MapUrlToZone(uri);
			if ((!flag && ((num2 != 3 && num2 != 4) || num != 0)) || (flag && (num2 == num || (num2 <= 4 && num <= 4 && (num2 < num || ((num2 == 2 || num2 == 1) && (num == 2 || num == 1)))))))
			{
				return LaunchResult.Launched;
			}
			return AppSecurityManager.CheckBlockNavigation(uri, destinationUri, flag);
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x00160059 File Offset: 0x0015F059
		private static LaunchResult CheckBlockNavigation(Uri originatingUri, Uri destinationUri, bool fEnabled)
		{
			if (!fEnabled)
			{
				return LaunchResult.Launched;
			}
			if (UnsafeNativeMethods.CoInternetIsFeatureZoneElevationEnabled(BindUriHelper.UriToString(originatingUri), BindUriHelper.UriToString(destinationUri), AppSecurityManager._secMgr, 2) == 1)
			{
				return LaunchResult.Launched;
			}
			if (AppSecurityManager.IsZoneElevationSettingPrompt(destinationUri))
			{
				return LaunchResult.NotLaunchedDueToPrompt;
			}
			return LaunchResult.NotLaunched;
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00160088 File Offset: 0x0015F088
		private unsafe static bool IsZoneElevationSettingPrompt(Uri target)
		{
			Invariant.Assert(AppSecurityManager._secMgr != null);
			int num = 3;
			string pwszUrl = BindUriHelper.UriToString(target);
			AppSecurityManager._secMgr.ProcessUrlAction(pwszUrl, 8449, (byte*)(&num), Marshal.SizeOf(typeof(int)), null, 0, 1, 0);
			return num == 1;
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x001600D8 File Offset: 0x0015F0D8
		private static void EnsureSecurityManager()
		{
			if (AppSecurityManager._secMgr == null)
			{
				object lockObj = AppSecurityManager._lockObj;
				lock (lockObj)
				{
					if (AppSecurityManager._secMgr == null)
					{
						AppSecurityManager._secMgr = (UnsafeNativeMethods.IInternetSecurityManager)new AppSecurityManager.InternetSecurityManager();
						AppSecurityManager._secMgrSite = new SecurityMgrSite();
						AppSecurityManager._secMgr.SetSecuritySite(AppSecurityManager._secMgrSite);
					}
				}
			}
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00160148 File Offset: 0x0015F148
		internal static void ClearSecurityManager()
		{
			if (AppSecurityManager._secMgr != null)
			{
				object lockObj = AppSecurityManager._lockObj;
				lock (lockObj)
				{
					if (AppSecurityManager._secMgr != null)
					{
						AppSecurityManager._secMgr.SetSecuritySite(null);
						AppSecurityManager._secMgrSite = null;
						AppSecurityManager._secMgr = null;
					}
				}
			}
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x001601A8 File Offset: 0x0015F1A8
		internal static int MapUrlToZone(Uri url)
		{
			AppSecurityManager.EnsureSecurityManager();
			int result;
			AppSecurityManager._secMgr.MapUrlToZone(BindUriHelper.UriToString(url), out result, 0);
			return result;
		}

		// Token: 0x04000CDE RID: 3294
		private const string RefererHeader = "Referer: ";

		// Token: 0x04000CDF RID: 3295
		private const string BrowserOpenCommandLookupKey = "htmlfile\\shell\\open\\command";

		// Token: 0x04000CE0 RID: 3296
		private static object _lockObj = new object();

		// Token: 0x04000CE1 RID: 3297
		private static UnsafeNativeMethods.IInternetSecurityManager _secMgr;

		// Token: 0x04000CE2 RID: 3298
		private static SecurityMgrSite _secMgrSite;

		// Token: 0x02000A0F RID: 2575
		[ComVisible(false)]
		[Guid("7b8a2d94-0ac9-11d1-896c-00c04Fb6bfc4")]
		[ComImport]
		internal class InternetSecurityManager
		{
			// Token: 0x060084C4 RID: 33988
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern InternetSecurityManager();
		}
	}
}
