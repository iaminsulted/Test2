using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Utility;

namespace System.Windows.Diagnostics
{
	// Token: 0x02000441 RID: 1089
	public static class ResourceDictionaryDiagnostics
	{
		// Token: 0x06003511 RID: 13585 RVA: 0x001DCF7C File Offset: 0x001DBF7C
		static ResourceDictionaryDiagnostics()
		{
			ResourceDictionaryDiagnostics.IgnorableProperties.Add(typeof(ResourceDictionary).GetProperty("DeferrableContent"));
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06003512 RID: 13586 RVA: 0x001DD020 File Offset: 0x001DC020
		public static IEnumerable<ResourceDictionaryInfo> ThemedResourceDictionaries
		{
			get
			{
				if (!ResourceDictionaryDiagnostics.IsEnabled)
				{
					return ResourceDictionaryDiagnostics.EmptyResourceDictionaryInfos;
				}
				return SystemResources.ThemedResourceDictionaries;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06003513 RID: 13587 RVA: 0x001DD034 File Offset: 0x001DC034
		public static IEnumerable<ResourceDictionaryInfo> GenericResourceDictionaries
		{
			get
			{
				if (!ResourceDictionaryDiagnostics.IsEnabled)
				{
					return ResourceDictionaryDiagnostics.EmptyResourceDictionaryInfos;
				}
				return SystemResources.GenericResourceDictionaries;
			}
		}

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06003514 RID: 13588 RVA: 0x001DD048 File Offset: 0x001DC048
		// (remove) Token: 0x06003515 RID: 13589 RVA: 0x001DD057 File Offset: 0x001DC057
		public static event EventHandler<ResourceDictionaryLoadedEventArgs> ThemedResourceDictionaryLoaded
		{
			add
			{
				if (ResourceDictionaryDiagnostics.IsEnabled)
				{
					SystemResources.ThemedDictionaryLoaded += value;
				}
			}
			remove
			{
				SystemResources.ThemedDictionaryLoaded -= value;
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06003516 RID: 13590 RVA: 0x001DD05F File Offset: 0x001DC05F
		// (remove) Token: 0x06003517 RID: 13591 RVA: 0x001DD06E File Offset: 0x001DC06E
		public static event EventHandler<ResourceDictionaryUnloadedEventArgs> ThemedResourceDictionaryUnloaded
		{
			add
			{
				if (ResourceDictionaryDiagnostics.IsEnabled)
				{
					SystemResources.ThemedDictionaryUnloaded += value;
				}
			}
			remove
			{
				SystemResources.ThemedDictionaryUnloaded -= value;
			}
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06003518 RID: 13592 RVA: 0x001DD076 File Offset: 0x001DC076
		// (remove) Token: 0x06003519 RID: 13593 RVA: 0x001DD085 File Offset: 0x001DC085
		public static event EventHandler<ResourceDictionaryLoadedEventArgs> GenericResourceDictionaryLoaded
		{
			add
			{
				if (ResourceDictionaryDiagnostics.IsEnabled)
				{
					SystemResources.GenericDictionaryLoaded += value;
				}
			}
			remove
			{
				SystemResources.GenericDictionaryLoaded -= value;
			}
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x001DD090 File Offset: 0x001DC090
		public static IEnumerable<ResourceDictionary> GetResourceDictionariesForSource(Uri uri)
		{
			if (!ResourceDictionaryDiagnostics.IsEnabled || ResourceDictionaryDiagnostics._dictionariesFromUri == null)
			{
				return ResourceDictionaryDiagnostics.EmptyResourceDictionaries;
			}
			object dictionariesFromUriLock = ResourceDictionaryDiagnostics._dictionariesFromUriLock;
			IEnumerable<ResourceDictionary> result;
			lock (dictionariesFromUriLock)
			{
				List<WeakReference<ResourceDictionary>> list;
				if (!ResourceDictionaryDiagnostics._dictionariesFromUri.TryGetValue(uri, out list) || list.Count == 0)
				{
					result = ResourceDictionaryDiagnostics.EmptyResourceDictionaries;
				}
				else
				{
					List<ResourceDictionary> list2 = new List<ResourceDictionary>(list.Count);
					List<WeakReference<ResourceDictionary>> list3 = null;
					foreach (WeakReference<ResourceDictionary> weakReference in list)
					{
						ResourceDictionary item;
						if (weakReference.TryGetTarget(out item))
						{
							list2.Add(item);
						}
						else
						{
							if (list3 == null)
							{
								list3 = new List<WeakReference<ResourceDictionary>>();
							}
							list3.Add(weakReference);
						}
					}
					if (list3 != null)
					{
						ResourceDictionaryDiagnostics.RemoveEntries(uri, list, list3);
					}
					result = list2.AsReadOnly();
				}
			}
			return result;
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x001DD188 File Offset: 0x001DC188
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void AddResourceDictionaryForUri(Uri uri, ResourceDictionary rd)
		{
			if (uri != null && ResourceDictionaryDiagnostics.IsEnabled)
			{
				ResourceDictionaryDiagnostics.AddResourceDictionaryForUriImpl(uri, rd);
			}
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x001DD1A4 File Offset: 0x001DC1A4
		private static void AddResourceDictionaryForUriImpl(Uri uri, ResourceDictionary rd)
		{
			object dictionariesFromUriLock = ResourceDictionaryDiagnostics._dictionariesFromUriLock;
			lock (dictionariesFromUriLock)
			{
				if (ResourceDictionaryDiagnostics._dictionariesFromUri == null)
				{
					ResourceDictionaryDiagnostics._dictionariesFromUri = new Dictionary<Uri, List<WeakReference<ResourceDictionary>>>();
				}
				List<WeakReference<ResourceDictionary>> list;
				if (!ResourceDictionaryDiagnostics._dictionariesFromUri.TryGetValue(uri, out list))
				{
					list = new List<WeakReference<ResourceDictionary>>(1);
					ResourceDictionaryDiagnostics._dictionariesFromUri.Add(uri, list);
				}
				list.Add(new WeakReference<ResourceDictionary>(rd));
			}
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x001DD21C File Offset: 0x001DC21C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void RemoveResourceDictionaryForUri(Uri uri, ResourceDictionary rd)
		{
			if (uri != null && ResourceDictionaryDiagnostics.IsEnabled)
			{
				ResourceDictionaryDiagnostics.RemoveResourceDictionaryForUriImpl(uri, rd);
			}
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x001DD238 File Offset: 0x001DC238
		private static void RemoveResourceDictionaryForUriImpl(Uri uri, ResourceDictionary rd)
		{
			object dictionariesFromUriLock = ResourceDictionaryDiagnostics._dictionariesFromUriLock;
			lock (dictionariesFromUriLock)
			{
				List<WeakReference<ResourceDictionary>> list;
				if (ResourceDictionaryDiagnostics._dictionariesFromUri != null && ResourceDictionaryDiagnostics._dictionariesFromUri.TryGetValue(uri, out list))
				{
					List<WeakReference<ResourceDictionary>> list2 = new List<WeakReference<ResourceDictionary>>();
					foreach (WeakReference<ResourceDictionary> weakReference in list)
					{
						ResourceDictionary resourceDictionary;
						if (!weakReference.TryGetTarget(out resourceDictionary) || resourceDictionary == rd)
						{
							list2.Add(weakReference);
						}
					}
					ResourceDictionaryDiagnostics.RemoveEntries(uri, list, list2);
				}
			}
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x001DD2E8 File Offset: 0x001DC2E8
		private static void RemoveEntries(Uri uri, List<WeakReference<ResourceDictionary>> list, List<WeakReference<ResourceDictionary>> toRemove)
		{
			foreach (WeakReference<ResourceDictionary> item in toRemove)
			{
				list.Remove(item);
			}
			if (list.Count == 0)
			{
				ResourceDictionaryDiagnostics._dictionariesFromUri.Remove(uri);
			}
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x001DD34C File Offset: 0x001DC34C
		public static IEnumerable<FrameworkElement> GetFrameworkElementOwners(ResourceDictionary dictionary)
		{
			return ResourceDictionaryDiagnostics.GetOwners<FrameworkElement>(dictionary.FrameworkElementOwners, ResourceDictionaryDiagnostics.EmptyFrameworkElementList);
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x001DD35E File Offset: 0x001DC35E
		public static IEnumerable<FrameworkContentElement> GetFrameworkContentElementOwners(ResourceDictionary dictionary)
		{
			return ResourceDictionaryDiagnostics.GetOwners<FrameworkContentElement>(dictionary.FrameworkContentElementOwners, ResourceDictionaryDiagnostics.EmptyFrameworkContentElementList);
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x001DD370 File Offset: 0x001DC370
		public static IEnumerable<Application> GetApplicationOwners(ResourceDictionary dictionary)
		{
			return ResourceDictionaryDiagnostics.GetOwners<Application>(dictionary.ApplicationOwners, ResourceDictionaryDiagnostics.EmptyApplicationList);
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x001DD384 File Offset: 0x001DC384
		private static IEnumerable<T> GetOwners<T>(WeakReferenceList list, IEnumerable<T> emptyList) where T : DispatcherObject
		{
			if (!ResourceDictionaryDiagnostics.IsEnabled || list == null || list.Count == 0)
			{
				return emptyList;
			}
			List<T> list2 = new List<T>(list.Count);
			foreach (object obj in list)
			{
				T t = obj as T;
				if (t != null)
				{
					list2.Add(t);
				}
			}
			return list2.AsReadOnly();
		}

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06003524 RID: 13604 RVA: 0x001DD3EC File Offset: 0x001DC3EC
		// (remove) Token: 0x06003525 RID: 13605 RVA: 0x001DD420 File Offset: 0x001DC420
		public static event EventHandler<StaticResourceResolvedEventArgs> StaticResourceResolved;

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06003526 RID: 13606 RVA: 0x001DD453 File Offset: 0x001DC453
		internal static bool HasStaticResourceResolvedListeners
		{
			get
			{
				return ResourceDictionaryDiagnostics.IsEnabled && ResourceDictionaryDiagnostics.StaticResourceResolved != null;
			}
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x001DD466 File Offset: 0x001DC466
		internal static bool ShouldIgnoreProperty(object targetProperty)
		{
			return ResourceDictionaryDiagnostics.IgnorableProperties.Contains(targetProperty);
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x001DD474 File Offset: 0x001DC474
		internal static ResourceDictionaryDiagnostics.LookupResult RequestLookupResult(StaticResourceExtension requester)
		{
			if (ResourceDictionaryDiagnostics._lookupResultStack == null)
			{
				ResourceDictionaryDiagnostics._lookupResultStack = new Stack<ResourceDictionaryDiagnostics.LookupResult>();
			}
			ResourceDictionaryDiagnostics.LookupResult lookupResult = new ResourceDictionaryDiagnostics.LookupResult(requester);
			ResourceDictionaryDiagnostics._lookupResultStack.Push(lookupResult);
			return lookupResult;
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x001DD4A5 File Offset: 0x001DC4A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void RecordLookupResult(object key, ResourceDictionary rd)
		{
			if (ResourceDictionaryDiagnostics.IsEnabled && ResourceDictionaryDiagnostics._lookupResultStack != null)
			{
				ResourceDictionaryDiagnostics.RecordLookupResultImpl(key, rd);
			}
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x001DD4BC File Offset: 0x001DC4BC
		private static void RecordLookupResultImpl(object key, ResourceDictionary rd)
		{
			if (ResourceDictionaryDiagnostics._lookupResultStack.Count > 0)
			{
				ResourceDictionaryDiagnostics.LookupResult lookupResult = ResourceDictionaryDiagnostics._lookupResultStack.Peek();
				if (!object.Equals(lookupResult.Requester.ResourceKey, key))
				{
					return;
				}
				if (lookupResult.Requester.GetType() == typeof(StaticResourceExtension))
				{
					lookupResult.Key = key;
					lookupResult.Dictionary = rd;
					return;
				}
				lookupResult.Key = key;
				lookupResult.Dictionary = rd;
			}
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x001DD530 File Offset: 0x001DC530
		internal static void RevertRequest(StaticResourceExtension requester, bool success)
		{
			ResourceDictionaryDiagnostics.LookupResult lookupResult = ResourceDictionaryDiagnostics._lookupResultStack.Pop();
			if (!success)
			{
				return;
			}
			if (lookupResult.Requester.GetType() == typeof(StaticResourceExtension))
			{
				return;
			}
			if (ResourceDictionaryDiagnostics._resultCache == null)
			{
				ResourceDictionaryDiagnostics._resultCache = new Dictionary<WeakReferenceKey<StaticResourceExtension>, WeakReference<ResourceDictionary>>();
			}
			WeakReferenceKey<StaticResourceExtension> key = new WeakReferenceKey<StaticResourceExtension>(requester);
			ResourceDictionary dictionary = null;
			WeakReference<ResourceDictionary> weakReference;
			if (ResourceDictionaryDiagnostics._resultCache.TryGetValue(key, out weakReference))
			{
				weakReference.TryGetTarget(out dictionary);
			}
			if (lookupResult.Dictionary != null)
			{
				ResourceDictionaryDiagnostics._resultCache[key] = new WeakReference<ResourceDictionary>(lookupResult.Dictionary);
				return;
			}
			lookupResult.Key = requester.ResourceKey;
			lookupResult.Dictionary = dictionary;
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x001DD5D0 File Offset: 0x001DC5D0
		internal static void OnStaticResourceResolved(object targetObject, object targetProperty, ResourceDictionaryDiagnostics.LookupResult result)
		{
			EventHandler<StaticResourceResolvedEventArgs> staticResourceResolved = ResourceDictionaryDiagnostics.StaticResourceResolved;
			if (staticResourceResolved != null && result.Dictionary != null)
			{
				staticResourceResolved(null, new StaticResourceResolvedEventArgs(targetObject, targetProperty, result.Dictionary, result.Key));
			}
			ResourceDictionaryDiagnostics.RequestCacheCleanup(targetObject);
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x001DD610 File Offset: 0x001DC610
		private static void RequestCacheCleanup(object targetObject)
		{
			DispatcherObject dispatcherObject;
			Dispatcher dispatcher;
			if (ResourceDictionaryDiagnostics._resultCache == null || ResourceDictionaryDiagnostics._cleanupOperation != null || (dispatcherObject = (targetObject as DispatcherObject)) == null || (dispatcher = dispatcherObject.Dispatcher) == null)
			{
				return;
			}
			ResourceDictionaryDiagnostics._cleanupOperation = dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ResourceDictionaryDiagnostics.DoCleanup));
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x001DD658 File Offset: 0x001DC658
		private static void DoCleanup()
		{
			ResourceDictionaryDiagnostics._cleanupOperation = null;
			List<WeakReferenceKey<StaticResourceExtension>> list = null;
			foreach (KeyValuePair<WeakReferenceKey<StaticResourceExtension>, WeakReference<ResourceDictionary>> keyValuePair in ResourceDictionaryDiagnostics._resultCache)
			{
				ResourceDictionary resourceDictionary;
				if (keyValuePair.Key.Item == null || !keyValuePair.Value.TryGetTarget(out resourceDictionary))
				{
					if (list == null)
					{
						list = new List<WeakReferenceKey<StaticResourceExtension>>();
					}
					list.Add(keyValuePair.Key);
				}
			}
			if (list != null)
			{
				foreach (WeakReferenceKey<StaticResourceExtension> key in list)
				{
					ResourceDictionaryDiagnostics._resultCache.Remove(key);
				}
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x0600352F RID: 13615 RVA: 0x001DD728 File Offset: 0x001DC728
		// (set) Token: 0x06003530 RID: 13616 RVA: 0x001DD72F File Offset: 0x001DC72F
		internal static bool IsEnabled { get; private set; } = VisualDiagnostics.IsEnabled && VisualDiagnostics.IsEnvironmentVariableSet(null, "ENABLE_XAML_DIAGNOSTICS_SOURCE_INFO");

		// Token: 0x04001C6C RID: 7276
		private static Dictionary<Uri, List<WeakReference<ResourceDictionary>>> _dictionariesFromUri;

		// Token: 0x04001C6D RID: 7277
		private static object _dictionariesFromUriLock = new object();

		// Token: 0x04001C6E RID: 7278
		private static readonly ReadOnlyCollection<ResourceDictionary> EmptyResourceDictionaries = new List<ResourceDictionary>().AsReadOnly();

		// Token: 0x04001C6F RID: 7279
		private static readonly ReadOnlyCollection<FrameworkElement> EmptyFrameworkElementList = new List<FrameworkElement>().AsReadOnly();

		// Token: 0x04001C70 RID: 7280
		private static readonly ReadOnlyCollection<FrameworkContentElement> EmptyFrameworkContentElementList = new List<FrameworkContentElement>().AsReadOnly();

		// Token: 0x04001C71 RID: 7281
		private static readonly ReadOnlyCollection<Application> EmptyApplicationList = new List<Application>().AsReadOnly();

		// Token: 0x04001C73 RID: 7283
		private static List<object> IgnorableProperties = new List<object>();

		// Token: 0x04001C74 RID: 7284
		[ThreadStatic]
		private static Stack<ResourceDictionaryDiagnostics.LookupResult> _lookupResultStack;

		// Token: 0x04001C75 RID: 7285
		[ThreadStatic]
		private static Dictionary<WeakReferenceKey<StaticResourceExtension>, WeakReference<ResourceDictionary>> _resultCache;

		// Token: 0x04001C76 RID: 7286
		[ThreadStatic]
		private static DispatcherOperation _cleanupOperation;

		// Token: 0x04001C78 RID: 7288
		private static readonly ReadOnlyCollection<ResourceDictionaryInfo> EmptyResourceDictionaryInfos = new List<ResourceDictionaryInfo>().AsReadOnly();

		// Token: 0x02000AC6 RID: 2758
		internal class LookupResult
		{
			// Token: 0x17001E64 RID: 7780
			// (get) Token: 0x06008AEB RID: 35563 RVA: 0x00338C75 File Offset: 0x00337C75
			// (set) Token: 0x06008AEC RID: 35564 RVA: 0x00338C7D File Offset: 0x00337C7D
			public StaticResourceExtension Requester { get; set; }

			// Token: 0x17001E65 RID: 7781
			// (get) Token: 0x06008AED RID: 35565 RVA: 0x00338C86 File Offset: 0x00337C86
			// (set) Token: 0x06008AEE RID: 35566 RVA: 0x00338C8E File Offset: 0x00337C8E
			public object Key { get; set; }

			// Token: 0x17001E66 RID: 7782
			// (get) Token: 0x06008AEF RID: 35567 RVA: 0x00338C97 File Offset: 0x00337C97
			// (set) Token: 0x06008AF0 RID: 35568 RVA: 0x00338C9F File Offset: 0x00337C9F
			public ResourceDictionary Dictionary { get; set; }

			// Token: 0x06008AF1 RID: 35569 RVA: 0x00338CA8 File Offset: 0x00337CA8
			public LookupResult(StaticResourceExtension requester)
			{
				this.Requester = requester;
			}
		}
	}
}
