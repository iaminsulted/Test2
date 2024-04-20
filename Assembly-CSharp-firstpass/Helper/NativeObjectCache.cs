using System;
using System.Collections.Generic;
using System.Linq;

namespace Helper
{
	// Token: 0x02000034 RID: 52
	public static class NativeObjectCache
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x000166B8 File Offset: 0x000148B8
		public static void AddObject<T>(IntPtr nativePtr, T obj) where T : class
		{
			object @lock = NativeObjectCache._lock;
			lock (@lock)
			{
				Dictionary<IntPtr, WeakReference> dictionary = null;
				if (!NativeObjectCache._objectCache.TryGetValue(typeof(T), out dictionary) || dictionary == null)
				{
					dictionary = new Dictionary<IntPtr, WeakReference>();
					NativeObjectCache._objectCache[typeof(T)] = dictionary;
				}
				dictionary[nativePtr] = new WeakReference(obj);
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0001673C File Offset: 0x0001493C
		public static void Flush()
		{
			object @lock = NativeObjectCache._lock;
			lock (@lock)
			{
				foreach (KeyValuePair<Type, Dictionary<IntPtr, WeakReference>> keyValuePair in NativeObjectCache._objectCache.ToArray<KeyValuePair<Type, Dictionary<IntPtr, WeakReference>>>())
				{
					foreach (KeyValuePair<IntPtr, WeakReference> keyValuePair2 in keyValuePair.Value.ToArray<KeyValuePair<IntPtr, WeakReference>>())
					{
						IDisposable disposable = keyValuePair2.Value.Target as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000167E4 File Offset: 0x000149E4
		public static void RemoveObject<T>(IntPtr nativePtr)
		{
			object @lock = NativeObjectCache._lock;
			lock (@lock)
			{
				Dictionary<IntPtr, WeakReference> dictionary = null;
				if (!NativeObjectCache._objectCache.TryGetValue(typeof(T), out dictionary) || dictionary == null)
				{
					dictionary = new Dictionary<IntPtr, WeakReference>();
					NativeObjectCache._objectCache[typeof(T)] = dictionary;
				}
				if (dictionary.ContainsKey(nativePtr))
				{
					dictionary.Remove(nativePtr);
				}
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00016868 File Offset: 0x00014A68
		public static T GetObject<T>(IntPtr nativePtr) where T : class
		{
			object @lock = NativeObjectCache._lock;
			T result;
			lock (@lock)
			{
				Dictionary<IntPtr, WeakReference> dictionary = null;
				if (!NativeObjectCache._objectCache.TryGetValue(typeof(T), out dictionary) || dictionary == null)
				{
					dictionary = new Dictionary<IntPtr, WeakReference>();
					NativeObjectCache._objectCache[typeof(T)] = dictionary;
				}
				WeakReference weakReference = null;
				if (dictionary.TryGetValue(nativePtr, out weakReference) && weakReference != null)
				{
					T t = weakReference.Target as T;
					if (t != null)
					{
						return t;
					}
				}
				result = default(T);
			}
			return result;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00016918 File Offset: 0x00014B18
		public static T CreateOrGetObject<T>(IntPtr nativePtr, Func<IntPtr, T> create) where T : class
		{
			T t = default(T);
			object @lock = NativeObjectCache._lock;
			lock (@lock)
			{
				Dictionary<IntPtr, WeakReference> dictionary = null;
				if (!NativeObjectCache._objectCache.TryGetValue(typeof(T), out dictionary) || dictionary == null)
				{
					dictionary = new Dictionary<IntPtr, WeakReference>();
					NativeObjectCache._objectCache[typeof(T)] = dictionary;
				}
				WeakReference weakReference = null;
				if (dictionary.TryGetValue(nativePtr, out weakReference) && weakReference != null && weakReference.IsAlive)
				{
					t = (weakReference.Target as T);
				}
				if (t == null)
				{
					if (create != null)
					{
						t = create(nativePtr);
						dictionary[nativePtr] = new WeakReference(t);
					}
					else if (typeof(T) == typeof(object))
					{
						t = (T)((object)nativePtr);
					}
				}
			}
			return t;
		}

		// Token: 0x040001F0 RID: 496
		private static object _lock = new object();

		// Token: 0x040001F1 RID: 497
		private static Dictionary<Type, Dictionary<IntPtr, WeakReference>> _objectCache = new Dictionary<Type, Dictionary<IntPtr, WeakReference>>();
	}
}
