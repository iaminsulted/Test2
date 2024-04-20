using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000065 RID: 101
	public sealed class DepthFrameSource : INativeWrapper
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001FBDD File Offset: 0x0001DDDD
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001FBE5 File Offset: 0x0001DDE5
		internal DepthFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrameSource.Windows_Kinect_DepthFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001FC00 File Offset: 0x0001DE00
		~DepthFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06000529 RID: 1321
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600052A RID: 1322
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600052B RID: 1323 RVA: 0x0001FC30 File Offset: 0x0001DE30
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrameSource>(this._pNative);
			DepthFrameSource.Windows_Kinect_DepthFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600052C RID: 1324
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ushort Windows_Kinect_DepthFrameSource_get_DepthMaxReliableDistance(IntPtr pNative);

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0001FC6C File Offset: 0x0001DE6C
		public ushort DepthMaxReliableDistance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				return DepthFrameSource.Windows_Kinect_DepthFrameSource_get_DepthMaxReliableDistance(this._pNative);
			}
		}

		// Token: 0x0600052E RID: 1326
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ushort Windows_Kinect_DepthFrameSource_get_DepthMinReliableDistance(IntPtr pNative);

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001FC96 File Offset: 0x0001DE96
		public ushort DepthMinReliableDistance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				return DepthFrameSource.Windows_Kinect_DepthFrameSource_get_DepthMinReliableDistance(this._pNative);
			}
		}

		// Token: 0x06000530 RID: 1328
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0001FCC0 File Offset: 0x0001DEC0
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				IntPtr intPtr = DepthFrameSource.Windows_Kinect_DepthFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06000532 RID: 1330
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_DepthFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x0001FD2F File Offset: 0x0001DF2F
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				return DepthFrameSource.Windows_Kinect_DepthFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x06000534 RID: 1332
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0001FD5C File Offset: 0x0001DF5C
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameSource");
				}
				IntPtr intPtr = DepthFrameSource.Windows_Kinect_DepthFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001FDCC File Offset: 0x0001DFCC
		[MonoPInvokeCallback(typeof(DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				DepthFrameSource objThis = NativeObjectCache.GetObject<DepthFrameSource>(pNative);
				FrameCapturedEventArgs args = new FrameCapturedEventArgs(result);
				using (List<EventHandler<FrameCapturedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<FrameCapturedEventArgs> func = enumerator.Current;
						EventPump.Instance.Enqueue(delegate
						{
							try
							{
								func(objThis, args);
							}
							catch
							{
							}
						});
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(obj);
				}
			}
		}

		// Token: 0x06000537 RID: 1335
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameSource_add_FrameCaptured(IntPtr pNative, DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000538 RID: 1336 RVA: 0x0001FE90 File Offset: 0x0001E090
		// (remove) Token: 0x06000539 RID: 1337 RVA: 0x0001FF24 File Offset: 0x0001E124
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_FrameCaptured(this._pNative, new DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
						DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001FFC0 File Offset: 0x0001E1C0
		[MonoPInvokeCallback(typeof(DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				DepthFrameSource objThis = NativeObjectCache.GetObject<DepthFrameSource>(pNative);
				PropertyChangedEventArgs args = new PropertyChangedEventArgs(result);
				using (List<EventHandler<PropertyChangedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<PropertyChangedEventArgs> func = enumerator.Current;
						EventPump.Instance.Enqueue(delegate
						{
							try
							{
								func(objThis, args);
							}
							catch
							{
							}
						});
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(obj);
				}
			}
		}

		// Token: 0x0600053B RID: 1339
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameSource_add_PropertyChanged(IntPtr pNative, DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600053C RID: 1340 RVA: 0x00020084 File Offset: 0x0001E284
		// (remove) Token: 0x0600053D RID: 1341 RVA: 0x00020118 File Offset: 0x0001E318
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_PropertyChanged(this._pNative, new DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x0600053E RID: 1342
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x0600053F RID: 1343 RVA: 0x000201B4 File Offset: 0x0001E3B4
		public DepthFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrameSource");
			}
			IntPtr intPtr = DepthFrameSource.Windows_Kinect_DepthFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<DepthFrameReader>(intPtr, (IntPtr n) => new DepthFrameReader(n));
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00020224 File Offset: 0x0001E424
		private void __EventCleanup()
		{
			DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_FrameCaptured(this._pNative, new DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(DepthFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
					}
					DepthFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						DepthFrameSource.Windows_Kinect_DepthFrameSource_add_PropertyChanged(this._pNative, new DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					DepthFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04000267 RID: 615
		internal IntPtr _pNative;

		// Token: 0x04000268 RID: 616
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x04000269 RID: 617
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x0400026A RID: 618
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400026B RID: 619
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x02000287 RID: 647
		// (Invoke) Token: 0x06001200 RID: 4608
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000288 RID: 648
		// (Invoke) Token: 0x06001204 RID: 4612
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
