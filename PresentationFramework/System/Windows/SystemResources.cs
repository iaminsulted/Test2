using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Baml2006;
using System.Windows.Controls.Primitives;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xaml;
using System.Xaml.Permissions;
using MS.Internal;
using MS.Internal.Ink;
using MS.Internal.Interop;
using MS.Internal.PresentationFramework;
using MS.Internal.WindowsBase;
using MS.Utility;
using MS.Win32;

namespace System.Windows
{
	// Token: 0x020003BB RID: 955
	internal static class SystemResources
	{
		// Token: 0x0600282E RID: 10286 RVA: 0x0019401C File Offset: 0x0019301C
		internal static object FindThemeStyle(DependencyObjectType key)
		{
			object obj = SystemResources._themeStyleCache[key];
			if (obj == null)
			{
				obj = SystemResources.FindResourceInternal(key.SystemType);
				object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
				lock (themeDictionaryLock)
				{
					if (obj != null)
					{
						SystemResources._themeStyleCache[key] = obj;
					}
					else
					{
						SystemResources._themeStyleCache[key] = SystemResources._specialNull;
					}
				}
				return obj;
			}
			if (obj == SystemResources._specialNull)
			{
				return null;
			}
			return obj;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x001940A0 File Offset: 0x001930A0
		internal static object FindResourceInternal(object key)
		{
			return SystemResources.FindResourceInternal(key, false, false);
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x001940AC File Offset: 0x001930AC
		internal static object FindResourceInternal(object key, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			SystemResources.EnsureResourceChangeListener();
			object obj = null;
			bool flag = EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
			Type type = key as Type;
			ResourceKey resourceKey = (type == null) ? (key as ResourceKey) : null;
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceFindBegin, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, (key == null) ? "null" : key.ToString());
			}
			if (type == null && resourceKey == null)
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceFindEnd, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
				}
				return null;
			}
			if (!SystemResources.FindCachedResource(key, ref obj, mustReturnDeferredResourceReference))
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceCacheMiss, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
				}
				object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
				lock (themeDictionaryLock)
				{
					bool flag3 = true;
					SystemResourceKey systemResourceKey = (resourceKey != null) ? (resourceKey as SystemResourceKey) : null;
					if (systemResourceKey != null)
					{
						if (!mustReturnDeferredResourceReference)
						{
							obj = systemResourceKey.Resource;
						}
						else
						{
							obj = new DeferredResourceReferenceHolder(systemResourceKey, systemResourceKey.Resource);
						}
						if (flag)
						{
							EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceStock, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, systemResourceKey.ToString());
						}
					}
					else
					{
						obj = SystemResources.FindDictionaryResource(key, type, resourceKey, flag, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag3);
					}
					if ((flag3 && !allowDeferredResourceReference) || obj == null)
					{
						SystemResources.CacheResource(key, obj, flag);
					}
				}
			}
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceFindEnd, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
			}
			return obj;
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x00194210 File Offset: 0x00193210
		internal static ReadOnlyCollection<ResourceDictionaryInfo> ThemedResourceDictionaries
		{
			get
			{
				List<ResourceDictionaryInfo> list = new List<ResourceDictionaryInfo>();
				if (SystemResources._dictionaries != null)
				{
					object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
					lock (themeDictionaryLock)
					{
						if (SystemResources._dictionaries != null)
						{
							foreach (KeyValuePair<Assembly, SystemResources.ResourceDictionaries> keyValuePair in SystemResources._dictionaries)
							{
								ResourceDictionaryInfo themedDictionaryInfo = keyValuePair.Value.ThemedDictionaryInfo;
								if (themedDictionaryInfo.ResourceDictionary != null)
								{
									list.Add(themedDictionaryInfo);
								}
							}
						}
					}
				}
				return list.AsReadOnly();
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x001942C0 File Offset: 0x001932C0
		internal static ReadOnlyCollection<ResourceDictionaryInfo> GenericResourceDictionaries
		{
			get
			{
				List<ResourceDictionaryInfo> list = new List<ResourceDictionaryInfo>();
				if (SystemResources._dictionaries != null)
				{
					object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
					lock (themeDictionaryLock)
					{
						if (SystemResources._dictionaries != null)
						{
							foreach (KeyValuePair<Assembly, SystemResources.ResourceDictionaries> keyValuePair in SystemResources._dictionaries)
							{
								ResourceDictionaryInfo genericDictionaryInfo = keyValuePair.Value.GenericDictionaryInfo;
								if (genericDictionaryInfo.ResourceDictionary != null)
								{
									list.Add(genericDictionaryInfo);
								}
							}
						}
					}
				}
				return list.AsReadOnly();
			}
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x00194370 File Offset: 0x00193370
		internal static void CacheResource(object key, object resource, bool isTraceEnabled)
		{
			if (resource != null)
			{
				SystemResources._resourceCache[key] = resource;
				if (isTraceEnabled)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceCacheValue, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
					return;
				}
			}
			else
			{
				SystemResources._resourceCache[key] = SystemResources._specialNull;
				if (isTraceEnabled)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceCacheNull, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
				}
			}
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x001943CC File Offset: 0x001933CC
		private static bool FindCachedResource(object key, ref object resource, bool mustReturnDeferredResourceReference)
		{
			resource = SystemResources._resourceCache[key];
			bool flag = resource != null;
			if (resource == SystemResources._specialNull)
			{
				resource = null;
			}
			else
			{
				DispatcherObject dispatcherObject = resource as DispatcherObject;
				if (dispatcherObject != null)
				{
					dispatcherObject.VerifyAccess();
				}
			}
			if (flag && mustReturnDeferredResourceReference)
			{
				resource = new DeferredResourceReferenceHolder(key, resource);
			}
			return flag;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x0019441C File Offset: 0x0019341C
		private static object FindDictionaryResource(object key, Type typeKey, ResourceKey resourceKey, bool isTraceEnabled, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, out bool canCache)
		{
			canCache = true;
			object obj = null;
			Assembly assembly = (typeKey != null) ? typeKey.Assembly : resourceKey.Assembly;
			if (assembly == null || SystemResources.IgnoreAssembly(assembly))
			{
				return null;
			}
			SystemResources.ResourceDictionaries resourceDictionaries = SystemResources.EnsureDictionarySlot(assembly);
			ResourceDictionary resourceDictionary = resourceDictionaries.LoadThemedDictionary(isTraceEnabled);
			if (resourceDictionary != null)
			{
				obj = SystemResources.LookupResourceInDictionary(resourceDictionary, key, allowDeferredResourceReference, mustReturnDeferredResourceReference, out canCache);
			}
			if (obj == null)
			{
				resourceDictionary = resourceDictionaries.LoadGenericDictionary(isTraceEnabled);
				if (resourceDictionary != null)
				{
					obj = SystemResources.LookupResourceInDictionary(resourceDictionary, key, allowDeferredResourceReference, mustReturnDeferredResourceReference, out canCache);
				}
			}
			if (obj != null)
			{
				SystemResources.Freeze(obj);
			}
			return obj;
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x001944A0 File Offset: 0x001934A0
		private static object LookupResourceInDictionary(ResourceDictionary dictionary, object key, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, out bool canCache)
		{
			object result = null;
			SystemResources.IsSystemResourcesParsing = true;
			try
			{
				result = dictionary.FetchResource(key, allowDeferredResourceReference, mustReturnDeferredResourceReference, out canCache);
			}
			finally
			{
				SystemResources.IsSystemResourcesParsing = false;
			}
			return result;
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x001944DC File Offset: 0x001934DC
		private static void Freeze(object resource)
		{
			Freezable freezable = resource as Freezable;
			if (freezable != null && !freezable.IsFrozen)
			{
				freezable.Freeze();
			}
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x00194504 File Offset: 0x00193504
		private static SystemResources.ResourceDictionaries EnsureDictionarySlot(Assembly assembly)
		{
			SystemResources.ResourceDictionaries resourceDictionaries = null;
			if (SystemResources._dictionaries != null)
			{
				SystemResources._dictionaries.TryGetValue(assembly, out resourceDictionaries);
			}
			else
			{
				SystemResources._dictionaries = new Dictionary<Assembly, SystemResources.ResourceDictionaries>(1);
			}
			if (resourceDictionaries == null)
			{
				resourceDictionaries = new SystemResources.ResourceDictionaries(assembly);
				SystemResources._dictionaries.Add(assembly, resourceDictionaries);
			}
			return resourceDictionaries;
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x0019454C File Offset: 0x0019354C
		private static bool IgnoreAssembly(Assembly assembly)
		{
			return assembly == SystemResources.MsCorLib || assembly == SystemResources.PresentationCore || assembly == SystemResources.WindowsBase;
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x0600283A RID: 10298 RVA: 0x00194575 File Offset: 0x00193575
		private static Assembly MsCorLib
		{
			get
			{
				if (SystemResources._mscorlib == null)
				{
					SystemResources._mscorlib = typeof(string).Assembly;
				}
				return SystemResources._mscorlib;
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x0600283B RID: 10299 RVA: 0x0019459D File Offset: 0x0019359D
		private static Assembly PresentationFramework
		{
			get
			{
				if (SystemResources._presentationFramework == null)
				{
					SystemResources._presentationFramework = typeof(FrameworkElement).Assembly;
				}
				return SystemResources._presentationFramework;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x001945C5 File Offset: 0x001935C5
		private static Assembly PresentationCore
		{
			get
			{
				if (SystemResources._presentationCore == null)
				{
					SystemResources._presentationCore = typeof(UIElement).Assembly;
				}
				return SystemResources._presentationCore;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x001945ED File Offset: 0x001935ED
		private static Assembly WindowsBase
		{
			get
			{
				if (SystemResources._windowsBase == null)
				{
					SystemResources._windowsBase = typeof(DependencyObject).Assembly;
				}
				return SystemResources._windowsBase;
			}
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x00194618 File Offset: 0x00193618
		private static void EnsureResourceChangeListener()
		{
			if (SystemResources._hwndNotify != null && SystemResources._hwndNotifyHook != null && SystemResources._hwndNotify.Count != 0)
			{
				if (SystemResources._hwndNotify.Keys.FirstOrDefault((DpiUtil.HwndDpiInfo hwndDpiContext) => hwndDpiContext.DpiAwarenessContextValue == SystemResources.ProcessDpiAwarenessContextValue) != null)
				{
					return;
				}
			}
			SystemResources._hwndNotify = new Dictionary<DpiUtil.HwndDpiInfo, SecurityCriticalDataClass<HwndWrapper>>();
			SystemResources._hwndNotifyHook = new Dictionary<DpiUtil.HwndDpiInfo, HwndWrapperHook>();
			SystemResources._dpiAwarenessContextAndDpis = new List<DpiUtil.HwndDpiInfo>();
			DpiUtil.HwndDpiInfo item = SystemResources.CreateResourceChangeListenerWindow(SystemResources.ProcessDpiAwarenessContextValue, 0, 0, "EnsureResourceChangeListener");
			SystemResources._dpiAwarenessContextAndDpis.Add(item);
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x001946AC File Offset: 0x001936AC
		private static bool EnsureResourceChangeListener(DpiUtil.HwndDpiInfo hwndDpiInfo)
		{
			SystemResources.EnsureResourceChangeListener();
			if (hwndDpiInfo.DpiAwarenessContextValue == DpiAwarenessContextValue.Invalid)
			{
				return false;
			}
			if (!SystemResources._hwndNotify.ContainsKey(hwndDpiInfo) && SystemResources.CreateResourceChangeListenerWindow(hwndDpiInfo.DpiAwarenessContextValue, hwndDpiInfo.ContainingMonitorScreenRect.left, hwndDpiInfo.ContainingMonitorScreenRect.top, "EnsureResourceChangeListener") == hwndDpiInfo && !SystemResources._dpiAwarenessContextAndDpis.Contains(hwndDpiInfo))
			{
				SystemResources._dpiAwarenessContextAndDpis.Add(hwndDpiInfo);
			}
			return SystemResources._hwndNotify.ContainsKey(hwndDpiInfo);
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x00194724 File Offset: 0x00193724
		private static DpiUtil.HwndDpiInfo CreateResourceChangeListenerWindow(DpiAwarenessContextValue dpiContextValue, int x = 0, int y = 0, [CallerMemberName] string callerName = "")
		{
			DpiUtil.HwndDpiInfo result;
			using (DpiUtil.WithDpiAwarenessContext(dpiContextValue))
			{
				HwndWrapper hwndWrapper = new HwndWrapper(0, -2013265920, 0, x, y, 0, 0, "SystemResourceNotifyWindow", IntPtr.Zero, null);
				DpiUtil.HwndDpiInfo hwndDpiInfo = SystemResources.IsPerMonitorDpiScalingActive ? DpiUtil.GetExtendedDpiInfoForWindow(hwndWrapper.Handle) : new DpiUtil.HwndDpiInfo(dpiContextValue, SystemResources.GetDpiScaleForUnawareOrSystemAwareContext(dpiContextValue));
				SystemResources._hwndNotify[hwndDpiInfo] = new SecurityCriticalDataClass<HwndWrapper>(hwndWrapper);
				SystemResources._hwndNotify[hwndDpiInfo].Value.Dispatcher.ShutdownFinished += SystemResources.OnShutdownFinished;
				SystemResources._hwndNotifyHook[hwndDpiInfo] = new HwndWrapperHook(SystemResources.SystemThemeFilterMessage);
				SystemResources._hwndNotify[hwndDpiInfo].Value.AddHook(SystemResources._hwndNotifyHook[hwndDpiInfo]);
				result = hwndDpiInfo;
			}
			return result;
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x00194804 File Offset: 0x00193804
		private static void OnShutdownFinished(object sender, EventArgs args)
		{
			if (SystemResources._hwndNotify != null && SystemResources._hwndNotify.Count != 0)
			{
				foreach (DpiUtil.HwndDpiInfo key in SystemResources._dpiAwarenessContextAndDpis)
				{
					SystemResources._hwndNotify[key].Value.Dispose();
					SystemResources._hwndNotifyHook[key] = null;
				}
			}
			Dictionary<DpiUtil.HwndDpiInfo, SecurityCriticalDataClass<HwndWrapper>> hwndNotify = SystemResources._hwndNotify;
			if (hwndNotify != null)
			{
				hwndNotify.Clear();
			}
			SystemResources._hwndNotify = null;
			Dictionary<DpiUtil.HwndDpiInfo, HwndWrapperHook> hwndNotifyHook = SystemResources._hwndNotifyHook;
			if (hwndNotifyHook != null)
			{
				hwndNotifyHook.Clear();
			}
			SystemResources._hwndNotifyHook = null;
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x001948B0 File Offset: 0x001938B0
		private static DpiScale2 GetDpiScaleForUnawareOrSystemAwareContext(DpiAwarenessContextValue dpiContextValue)
		{
			DpiScale2 result;
			if (dpiContextValue != DpiAwarenessContextValue.SystemAware && dpiContextValue == DpiAwarenessContextValue.Unaware)
			{
				result = DpiScale2.FromPixelsPerInch(96.0, 96.0);
			}
			else
			{
				result = DpiUtil.GetSystemDpi();
			}
			return result;
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x001948EC File Offset: 0x001938EC
		private static void OnThemeChanged()
		{
			SystemResources.ResourceDictionaries.OnThemeChanged();
			UxThemeWrapper.OnThemeChanged();
			ThemeDictionaryExtension.OnThemeChanged();
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			lock (themeDictionaryLock)
			{
				SystemResources._resourceCache.Clear();
				SystemResources._themeStyleCache.Clear();
				if (SystemResources._dictionaries != null)
				{
					foreach (SystemResources.ResourceDictionaries resourceDictionaries in SystemResources._dictionaries.Values)
					{
						resourceDictionaries.ClearThemedDictionary();
					}
				}
			}
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x00194994 File Offset: 0x00193994
		private static void OnSystemValueChanged()
		{
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			lock (themeDictionaryLock)
			{
				List<SystemResourceKey> list = new List<SystemResourceKey>();
				foreach (object obj in SystemResources._resourceCache.Keys)
				{
					SystemResourceKey systemResourceKey = obj as SystemResourceKey;
					if (systemResourceKey != null)
					{
						list.Add(systemResourceKey);
					}
				}
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					SystemResources._resourceCache.Remove(list[i]);
				}
			}
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x00194A58 File Offset: 0x00193A58
		private static object InvalidateTreeResources(object args)
		{
			object[] array = (object[])args;
			PresentationSource presentationSource = (PresentationSource)array[0];
			if (!presentationSource.IsDisposed)
			{
				FrameworkElement frameworkElement = presentationSource.RootVisual as FrameworkElement;
				if (frameworkElement != null)
				{
					if ((bool)array[1])
					{
						TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, null, ResourcesChangeInfo.SysColorsOrSettingsChangeInfo);
					}
					else
					{
						TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, null, ResourcesChangeInfo.ThemeChangeInfo);
					}
					KeyboardNavigation.AlwaysShowFocusVisual = SystemParameters.KeyboardCues;
					frameworkElement.CoerceValue(KeyboardNavigation.ShowKeyboardCuesProperty);
					SystemResources.SystemResourcesAreChanging = true;
					frameworkElement.CoerceValue(TextElement.FontFamilyProperty);
					frameworkElement.CoerceValue(TextElement.FontSizeProperty);
					frameworkElement.CoerceValue(TextElement.FontStyleProperty);
					frameworkElement.CoerceValue(TextElement.FontWeightProperty);
					SystemResources.SystemResourcesAreChanging = false;
					PopupRoot popupRoot = frameworkElement as PopupRoot;
					if (popupRoot != null && popupRoot.Parent != null)
					{
						popupRoot.Parent.CoerceValue(Popup.HasDropShadowProperty);
					}
				}
			}
			return null;
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x00194B28 File Offset: 0x00193B28
		private static void InvalidateTabletDevices(WindowMessage msg, IntPtr wParam, IntPtr lParam)
		{
			if (StylusLogic.IsStylusAndTouchSupportEnabled && StylusLogic.IsInstantiated && SystemResources._hwndNotify != null && SystemResources._hwndNotify.Count != 0)
			{
				Dispatcher dispatcher = SystemResources.Hwnd.Dispatcher;
				if (((dispatcher != null) ? dispatcher.InputManager : null) != null)
				{
					StylusLogic.CurrentStylusLogic.HandleMessage(msg, wParam, lParam);
				}
			}
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x00194B7C File Offset: 0x00193B7C
		private static void InvalidateResources(bool isSysColorsOrSettingsChange)
		{
			SystemResources.SystemResourcesHaveChanged = true;
			Dispatcher dispatcher = isSysColorsOrSettingsChange ? null : Dispatcher.FromThread(Thread.CurrentThread);
			if (dispatcher != null || isSysColorsOrSettingsChange)
			{
				foreach (object obj in PresentationSource.CriticalCurrentSources)
				{
					PresentationSource presentationSource = (PresentationSource)obj;
					if (!presentationSource.IsDisposed && (isSysColorsOrSettingsChange || presentationSource.Dispatcher == dispatcher))
					{
						presentationSource.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(SystemResources.InvalidateTreeResources), new object[]
						{
							presentationSource,
							isSysColorsOrSettingsChange
						});
					}
				}
			}
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x00194C30 File Offset: 0x00193C30
		private static IntPtr SystemThemeFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg <= 536)
			{
				if (msg <= 26)
				{
					if (msg != 21)
					{
						if (msg == 26)
						{
							SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
							if (SystemParameters.InvalidateCache((int)wParam))
							{
								SystemResources.OnSystemValueChanged();
								SystemResources.InvalidateResources(true);
								HighContrastHelper.OnSettingChanged();
							}
							SystemParameters.InvalidateWindowFrameThicknessProperties();
						}
					}
					else if (SystemColors.InvalidateCache())
					{
						SystemResources.OnSystemValueChanged();
						SystemResources.InvalidateResources(true);
					}
				}
				else if (msg != 126)
				{
					if (msg == 536)
					{
						if (NativeMethods.IntPtrToInt32(wParam) == 10 && SystemParameters.InvalidatePowerDependentCache())
						{
							SystemResources.OnSystemValueChanged();
							SystemResources.InvalidateResources(true);
						}
					}
				}
				else
				{
					SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
					if (SystemParameters.InvalidateDisplayDependentCache())
					{
						SystemResources.OnSystemValueChanged();
						SystemResources.InvalidateResources(true);
					}
				}
			}
			else if (msg <= 712)
			{
				if (msg != 537)
				{
					if (msg == 712)
					{
						SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
					}
				}
				else
				{
					SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
					if (SystemParameters.InvalidateDeviceDependentCache())
					{
						SystemResources.OnSystemValueChanged();
						SystemResources.InvalidateResources(true);
					}
				}
			}
			else if (msg != 713)
			{
				switch (msg)
				{
				case 794:
					SystemColors.InvalidateCache();
					SystemParameters.InvalidateCache();
					SystemParameters.InvalidateDerivedThemeRelatedProperties();
					SystemResources.OnThemeChanged();
					SystemResources.InvalidateResources(false);
					break;
				case 798:
				case 799:
					SystemParameters.InvalidateIsGlassEnabled();
					break;
				case 800:
					SystemParameters.InvalidateWindowGlassColorizationProperties();
					break;
				}
			}
			else
			{
				SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x00194DBC File Offset: 0x00193DBC
		internal static bool ClearBitArray(BitArray cacheValid)
		{
			bool result = false;
			for (int i = 0; i < cacheValid.Count; i++)
			{
				if (SystemResources.ClearSlot(cacheValid, i))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x00194DE8 File Offset: 0x00193DE8
		internal static bool ClearSlot(BitArray cacheValid, int slot)
		{
			if (cacheValid[slot])
			{
				cacheValid[slot] = false;
				return true;
			}
			return false;
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x0600284B RID: 10315 RVA: 0x00194DFE File Offset: 0x00193DFE
		// (set) Token: 0x0600284C RID: 10316 RVA: 0x00194E08 File Offset: 0x00193E08
		internal static bool IsSystemResourcesParsing
		{
			get
			{
				return SystemResources._parsing > 0;
			}
			set
			{
				if (value)
				{
					SystemResources._parsing++;
					return;
				}
				SystemResources._parsing--;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600284D RID: 10317 RVA: 0x00194E26 File Offset: 0x00193E26
		internal static object ThemeDictionaryLock
		{
			get
			{
				return SystemResources._resourceCache.SyncRoot;
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x00194E34 File Offset: 0x00193E34
		private static DpiAwarenessContextValue ProcessDpiAwarenessContextValue
		{
			get
			{
				bool? flag = HwndTarget.IsProcessUnaware;
				bool flag2 = true;
				if (flag.GetValueOrDefault() == flag2 & flag != null)
				{
					return DpiAwarenessContextValue.Unaware;
				}
				flag = HwndTarget.IsProcessSystemAware;
				flag2 = true;
				if (flag.GetValueOrDefault() == flag2 & flag != null)
				{
					return DpiAwarenessContextValue.SystemAware;
				}
				flag = HwndTarget.IsProcessPerMonitorDpiAware;
				flag2 = true;
				if (flag.GetValueOrDefault() == flag2 & flag != null)
				{
					return DpiAwarenessContextValue.PerMonitorAware;
				}
				return DpiUtil.GetProcessDpiAwarenessContextValue(IntPtr.Zero);
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x0600284F RID: 10319 RVA: 0x00194EA7 File Offset: 0x00193EA7
		private static bool IsPerMonitorDpiScalingActive
		{
			get
			{
				return HwndTarget.IsPerMonitorDpiScalingEnabled && (SystemResources.ProcessDpiAwarenessContextValue == DpiAwarenessContextValue.PerMonitorAware || SystemResources.ProcessDpiAwarenessContextValue == DpiAwarenessContextValue.PerMonitorAwareVersion2);
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06002850 RID: 10320 RVA: 0x00194EC8 File Offset: 0x00193EC8
		private static HwndWrapper Hwnd
		{
			get
			{
				SystemResources.EnsureResourceChangeListener();
				DpiUtil.HwndDpiInfo key = SystemResources._hwndNotify.Keys.FirstOrDefault((DpiUtil.HwndDpiInfo hwndDpiContext) => hwndDpiContext.DpiAwarenessContextValue == SystemResources.ProcessDpiAwarenessContextValue);
				return SystemResources._hwndNotify[key].Value;
			}
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x00194F1C File Offset: 0x00193F1C
		internal static HwndWrapper GetDpiAwarenessCompatibleNotificationWindow(HandleRef hwnd)
		{
			DpiAwarenessContextValue processDpiAwarenessContextValue = SystemResources.ProcessDpiAwarenessContextValue;
			DpiUtil.HwndDpiInfo hwndDpiInfo = SystemResources.IsPerMonitorDpiScalingActive ? DpiUtil.GetExtendedDpiInfoForWindow(hwnd.Handle, true) : new DpiUtil.HwndDpiInfo(processDpiAwarenessContextValue, SystemResources.GetDpiScaleForUnawareOrSystemAwareContext(processDpiAwarenessContextValue));
			if (SystemResources.EnsureResourceChangeListener(hwndDpiInfo))
			{
				return SystemResources._hwndNotify[hwndDpiInfo].Value;
			}
			return null;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x00194F6C File Offset: 0x00193F6C
		internal static void DelayHwndShutdown()
		{
			if (SystemResources._hwndNotify != null && SystemResources._hwndNotify.Count != 0)
			{
				Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;
				currentDispatcher.ShutdownFinished -= SystemResources.OnShutdownFinished;
				currentDispatcher.ShutdownFinished += SystemResources.OnShutdownFinished;
			}
		}

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06002853 RID: 10323 RVA: 0x00194FAC File Offset: 0x00193FAC
		// (remove) Token: 0x06002854 RID: 10324 RVA: 0x00194FE0 File Offset: 0x00193FE0
		internal static event EventHandler<ResourceDictionaryLoadedEventArgs> ThemedDictionaryLoaded;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06002855 RID: 10325 RVA: 0x00195014 File Offset: 0x00194014
		// (remove) Token: 0x06002856 RID: 10326 RVA: 0x00195048 File Offset: 0x00194048
		internal static event EventHandler<ResourceDictionaryUnloadedEventArgs> ThemedDictionaryUnloaded;

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06002857 RID: 10327 RVA: 0x0019507C File Offset: 0x0019407C
		// (remove) Token: 0x06002858 RID: 10328 RVA: 0x001950B0 File Offset: 0x001940B0
		internal static event EventHandler<ResourceDictionaryLoadedEventArgs> GenericDictionaryLoaded;

		// Token: 0x04001472 RID: 5234
		[ThreadStatic]
		private static int _parsing;

		// Token: 0x04001473 RID: 5235
		[ThreadStatic]
		private static List<DpiUtil.HwndDpiInfo> _dpiAwarenessContextAndDpis;

		// Token: 0x04001474 RID: 5236
		[ThreadStatic]
		private static Dictionary<DpiUtil.HwndDpiInfo, SecurityCriticalDataClass<HwndWrapper>> _hwndNotify;

		// Token: 0x04001475 RID: 5237
		[ThreadStatic]
		private static Dictionary<DpiUtil.HwndDpiInfo, HwndWrapperHook> _hwndNotifyHook;

		// Token: 0x04001476 RID: 5238
		private static Hashtable _resourceCache = new Hashtable();

		// Token: 0x04001477 RID: 5239
		private static DTypeMap _themeStyleCache = new DTypeMap(100);

		// Token: 0x04001478 RID: 5240
		private static Dictionary<Assembly, SystemResources.ResourceDictionaries> _dictionaries;

		// Token: 0x04001479 RID: 5241
		private static object _specialNull = new object();

		// Token: 0x0400147A RID: 5242
		internal const string GenericResourceName = "themes/generic";

		// Token: 0x0400147B RID: 5243
		internal const string ClassicResourceName = "themes/classic";

		// Token: 0x0400147C RID: 5244
		private static Assembly _mscorlib;

		// Token: 0x0400147D RID: 5245
		private static Assembly _presentationFramework;

		// Token: 0x0400147E RID: 5246
		private static Assembly _presentationCore;

		// Token: 0x0400147F RID: 5247
		private static Assembly _windowsBase;

		// Token: 0x04001480 RID: 5248
		internal const string PresentationFrameworkName = "PresentationFramework";

		// Token: 0x04001481 RID: 5249
		internal static bool SystemResourcesHaveChanged;

		// Token: 0x04001482 RID: 5250
		[ThreadStatic]
		internal static bool SystemResourcesAreChanging;

		// Token: 0x02000A90 RID: 2704
		internal class ResourceDictionaries
		{
			// Token: 0x0600868A RID: 34442 RVA: 0x0032AA48 File Offset: 0x00329A48
			internal ResourceDictionaries(Assembly assembly)
			{
				this._assembly = assembly;
				this._themedDictionaryAssembly = null;
				this._themedDictionarySourceUri = null;
				this._genericDictionaryAssembly = null;
				this._genericDictionarySourceUri = null;
				if (assembly == SystemResources.PresentationFramework)
				{
					this._assemblyName = "PresentationFramework";
					this._genericDictionary = null;
					this._genericLoaded = true;
					this._genericLocation = ResourceDictionaryLocation.None;
					this._themedLocation = ResourceDictionaryLocation.ExternalAssembly;
					this._locationsLoaded = true;
					return;
				}
				this._assemblyName = SafeSecurityHelper.GetAssemblyPartialName(assembly);
			}

			// Token: 0x0600868B RID: 34443 RVA: 0x0032AAC8 File Offset: 0x00329AC8
			internal void ClearThemedDictionary()
			{
				ResourceDictionaryInfo themedDictionaryInfo = this.ThemedDictionaryInfo;
				this._themedLoaded = false;
				this._themedDictionary = null;
				this._themedDictionaryAssembly = null;
				this._themedDictionarySourceUri = null;
				if (themedDictionaryInfo.ResourceDictionary != null)
				{
					EventHandler<ResourceDictionaryUnloadedEventArgs> themedDictionaryUnloaded = SystemResources.ThemedDictionaryUnloaded;
					if (themedDictionaryUnloaded == null)
					{
						return;
					}
					themedDictionaryUnloaded(null, new ResourceDictionaryUnloadedEventArgs(themedDictionaryInfo));
				}
			}

			// Token: 0x0600868C RID: 34444 RVA: 0x0032AB18 File Offset: 0x00329B18
			internal ResourceDictionary LoadThemedDictionary(bool isTraceEnabled)
			{
				if (!this._themedLoaded)
				{
					this.LoadDictionaryLocations();
					if (this._preventReEnter || this._themedLocation == ResourceDictionaryLocation.None)
					{
						return null;
					}
					SystemResources.IsSystemResourcesParsing = true;
					this._preventReEnter = true;
					try
					{
						ResourceDictionary resourceDictionary = null;
						bool flag = this._themedLocation == ResourceDictionaryLocation.ExternalAssembly;
						string assemblyName;
						if (flag)
						{
							this.LoadExternalAssembly(false, false, out this._themedDictionaryAssembly, out assemblyName);
						}
						else
						{
							this._themedDictionaryAssembly = this._assembly;
							assemblyName = this._assemblyName;
						}
						if (this._themedDictionaryAssembly != null)
						{
							resourceDictionary = this.LoadDictionary(this._themedDictionaryAssembly, assemblyName, SystemResources.ResourceDictionaries.ThemedResourceName, isTraceEnabled, out this._themedDictionarySourceUri);
							if (resourceDictionary == null && !flag)
							{
								this.LoadExternalAssembly(false, false, out this._themedDictionaryAssembly, out assemblyName);
								if (this._themedDictionaryAssembly != null)
								{
									resourceDictionary = this.LoadDictionary(this._themedDictionaryAssembly, assemblyName, SystemResources.ResourceDictionaries.ThemedResourceName, isTraceEnabled, out this._themedDictionarySourceUri);
								}
							}
						}
						if (resourceDictionary == null && UxThemeWrapper.IsActive)
						{
							if (flag)
							{
								this.LoadExternalAssembly(true, false, out this._themedDictionaryAssembly, out assemblyName);
							}
							else
							{
								this._themedDictionaryAssembly = this._assembly;
								assemblyName = this._assemblyName;
							}
							if (this._themedDictionaryAssembly != null)
							{
								resourceDictionary = this.LoadDictionary(this._themedDictionaryAssembly, assemblyName, "themes/classic", isTraceEnabled, out this._themedDictionarySourceUri);
							}
						}
						this._themedDictionary = resourceDictionary;
						this._themedLoaded = true;
						if (this._themedDictionary != null)
						{
							EventHandler<ResourceDictionaryLoadedEventArgs> themedDictionaryLoaded = SystemResources.ThemedDictionaryLoaded;
							if (themedDictionaryLoaded != null)
							{
								themedDictionaryLoaded(null, new ResourceDictionaryLoadedEventArgs(this.ThemedDictionaryInfo));
							}
						}
					}
					finally
					{
						this._preventReEnter = false;
						SystemResources.IsSystemResourcesParsing = false;
					}
				}
				return this._themedDictionary;
			}

			// Token: 0x0600868D RID: 34445 RVA: 0x0032ACB0 File Offset: 0x00329CB0
			internal ResourceDictionary LoadGenericDictionary(bool isTraceEnabled)
			{
				if (!this._genericLoaded)
				{
					this.LoadDictionaryLocations();
					if (this._preventReEnter || this._genericLocation == ResourceDictionaryLocation.None)
					{
						return null;
					}
					SystemResources.IsSystemResourcesParsing = true;
					this._preventReEnter = true;
					try
					{
						ResourceDictionary genericDictionary = null;
						string assemblyName;
						if (this._genericLocation == ResourceDictionaryLocation.ExternalAssembly)
						{
							this.LoadExternalAssembly(false, true, out this._genericDictionaryAssembly, out assemblyName);
						}
						else
						{
							this._genericDictionaryAssembly = this._assembly;
							assemblyName = this._assemblyName;
						}
						if (this._genericDictionaryAssembly != null)
						{
							genericDictionary = this.LoadDictionary(this._genericDictionaryAssembly, assemblyName, "themes/generic", isTraceEnabled, out this._genericDictionarySourceUri);
						}
						this._genericDictionary = genericDictionary;
						this._genericLoaded = true;
						if (this._genericDictionary != null)
						{
							EventHandler<ResourceDictionaryLoadedEventArgs> genericDictionaryLoaded = SystemResources.GenericDictionaryLoaded;
							if (genericDictionaryLoaded != null)
							{
								genericDictionaryLoaded(null, new ResourceDictionaryLoadedEventArgs(this.GenericDictionaryInfo));
							}
						}
					}
					finally
					{
						this._preventReEnter = false;
						SystemResources.IsSystemResourcesParsing = false;
					}
				}
				return this._genericDictionary;
			}

			// Token: 0x0600868E RID: 34446 RVA: 0x0032ADA0 File Offset: 0x00329DA0
			private void LoadDictionaryLocations()
			{
				if (!this._locationsLoaded)
				{
					ThemeInfoAttribute themeInfoAttribute = ThemeInfoAttribute.FromAssembly(this._assembly);
					if (themeInfoAttribute != null)
					{
						this._themedLocation = themeInfoAttribute.ThemeDictionaryLocation;
						this._genericLocation = themeInfoAttribute.GenericDictionaryLocation;
					}
					else
					{
						this._themedLocation = ResourceDictionaryLocation.None;
						this._genericLocation = ResourceDictionaryLocation.None;
					}
					this._locationsLoaded = true;
				}
			}

			// Token: 0x0600868F RID: 34447 RVA: 0x0032ADF4 File Offset: 0x00329DF4
			private void LoadExternalAssembly(bool classic, bool generic, out Assembly assembly, out string assemblyName)
			{
				StringBuilder stringBuilder = new StringBuilder(this._assemblyName.Length + 10);
				stringBuilder.Append(this._assemblyName);
				stringBuilder.Append(".");
				if (generic)
				{
					stringBuilder.Append("generic");
				}
				else if (classic)
				{
					stringBuilder.Append("classic");
				}
				else
				{
					stringBuilder.Append(UxThemeWrapper.ThemeName);
				}
				assemblyName = stringBuilder.ToString();
				string fullAssemblyNameFromPartialName = SafeSecurityHelper.GetFullAssemblyNameFromPartialName(this._assembly, assemblyName);
				assembly = null;
				try
				{
					assembly = Assembly.Load(fullAssemblyNameFromPartialName);
				}
				catch (FileNotFoundException)
				{
				}
				catch (BadImageFormatException)
				{
				}
				if (this._assemblyName == "PresentationFramework" && assembly != null)
				{
					Type type = assembly.GetType("Microsoft.Windows.Themes.KnownTypeHelper");
					if (type != null)
					{
						SecurityHelper.RunClassConstructor(type);
					}
				}
			}

			// Token: 0x17001E26 RID: 7718
			// (get) Token: 0x06008690 RID: 34448 RVA: 0x0032AEDC File Offset: 0x00329EDC
			internal static string ThemedResourceName
			{
				get
				{
					string text = SystemResources.ResourceDictionaries._themedResourceName;
					while (text == null)
					{
						text = UxThemeWrapper.ThemedResourceName;
						string text2 = Interlocked.CompareExchange<string>(ref SystemResources.ResourceDictionaries._themedResourceName, text, null);
						if (text2 != null && text2 != text)
						{
							SystemResources.ResourceDictionaries._themedResourceName = null;
							text = null;
						}
					}
					return text;
				}
			}

			// Token: 0x17001E27 RID: 7719
			// (get) Token: 0x06008691 RID: 34449 RVA: 0x0032AF1C File Offset: 0x00329F1C
			internal ResourceDictionaryInfo GenericDictionaryInfo
			{
				get
				{
					return new ResourceDictionaryInfo(this._assembly, this._genericDictionaryAssembly, this._genericDictionary, this._genericDictionarySourceUri);
				}
			}

			// Token: 0x17001E28 RID: 7720
			// (get) Token: 0x06008692 RID: 34450 RVA: 0x0032AF3B File Offset: 0x00329F3B
			internal ResourceDictionaryInfo ThemedDictionaryInfo
			{
				get
				{
					return new ResourceDictionaryInfo(this._assembly, this._themedDictionaryAssembly, this._themedDictionary, this._themedDictionarySourceUri);
				}
			}

			// Token: 0x06008693 RID: 34451 RVA: 0x0032AF5C File Offset: 0x00329F5C
			private ResourceDictionary LoadDictionary(Assembly assembly, string assemblyName, string resourceName, bool isTraceEnabled, out Uri dictionarySourceUri)
			{
				ResourceDictionary resourceDictionary = null;
				dictionarySourceUri = null;
				ResourceManager resourceManager = new ResourceManager(assemblyName + ".g", assembly);
				resourceName += ".baml";
				Stream stream = null;
				try
				{
					stream = resourceManager.GetStream(resourceName, CultureInfo.CurrentUICulture);
				}
				catch (MissingManifestResourceException)
				{
				}
				catch (MissingSatelliteAssemblyException)
				{
				}
				catch (InvalidOperationException)
				{
				}
				if (stream != null)
				{
					Baml2006ReaderSettings baml2006ReaderSettings = new Baml2006ReaderSettings();
					baml2006ReaderSettings.OwnsStream = true;
					baml2006ReaderSettings.LocalAssembly = assembly;
					Baml2006ReaderInternal baml2006ReaderInternal = new Baml2006ReaderInternal(stream, new Baml2006SchemaContext(baml2006ReaderSettings.LocalAssembly), baml2006ReaderSettings);
					XamlObjectWriterSettings xamlObjectWriterSettings = System.Windows.Markup.XamlReader.CreateObjectWriterSettingsForBaml();
					if (assembly != null)
					{
						xamlObjectWriterSettings.AccessLevel = XamlAccessLevel.AssemblyAccessTo(assembly);
						AssemblyName assemblyName2 = new AssemblyName(assembly.FullName);
						Uri uri = null;
						if (Uri.TryCreate(string.Format("pack://application:,,,/{0};v{1};component/{2}", assemblyName2.Name, assemblyName2.Version.ToString(), resourceName), UriKind.Absolute, out uri))
						{
							if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
							{
								xamlObjectWriterSettings.SourceBamlUri = uri;
							}
							dictionarySourceUri = uri;
						}
					}
					XamlObjectWriter xamlObjectWriter = new XamlObjectWriter(baml2006ReaderInternal.SchemaContext, xamlObjectWriterSettings);
					XamlServices.Transform(baml2006ReaderInternal, xamlObjectWriter);
					resourceDictionary = (ResourceDictionary)xamlObjectWriter.Result;
					if (isTraceEnabled && resourceDictionary != null)
					{
						EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceBamlAssembly, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, assemblyName);
					}
				}
				return resourceDictionary;
			}

			// Token: 0x06008694 RID: 34452 RVA: 0x0032B0A8 File Offset: 0x0032A0A8
			internal static void OnThemeChanged()
			{
				SystemResources.ResourceDictionaries._themedResourceName = null;
			}

			// Token: 0x04004258 RID: 16984
			private ResourceDictionary _genericDictionary;

			// Token: 0x04004259 RID: 16985
			private ResourceDictionary _themedDictionary;

			// Token: 0x0400425A RID: 16986
			private bool _genericLoaded;

			// Token: 0x0400425B RID: 16987
			private bool _themedLoaded;

			// Token: 0x0400425C RID: 16988
			private bool _preventReEnter;

			// Token: 0x0400425D RID: 16989
			private bool _locationsLoaded;

			// Token: 0x0400425E RID: 16990
			private string _assemblyName;

			// Token: 0x0400425F RID: 16991
			private Assembly _assembly;

			// Token: 0x04004260 RID: 16992
			private ResourceDictionaryLocation _genericLocation;

			// Token: 0x04004261 RID: 16993
			private ResourceDictionaryLocation _themedLocation;

			// Token: 0x04004262 RID: 16994
			private static string _themedResourceName;

			// Token: 0x04004263 RID: 16995
			private Assembly _themedDictionaryAssembly;

			// Token: 0x04004264 RID: 16996
			private Assembly _genericDictionaryAssembly;

			// Token: 0x04004265 RID: 16997
			private Uri _themedDictionarySourceUri;

			// Token: 0x04004266 RID: 16998
			private Uri _genericDictionarySourceUri;
		}
	}
}
