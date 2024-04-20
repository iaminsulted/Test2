using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x0200007D RID: 125
	public sealed class LongExposureInfraredFrameSource : INativeWrapper
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00022385 File Offset: 0x00020585
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0002238D File Offset: 0x0002058D
		internal LongExposureInfraredFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000223A8 File Offset: 0x000205A8
		~LongExposureInfraredFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06000604 RID: 1540
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000605 RID: 1541
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000606 RID: 1542 RVA: 0x000223D8 File Offset: 0x000205D8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrameSource>(this._pNative);
			LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000607 RID: 1543
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00022414 File Offset: 0x00020614
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameSource");
				}
				IntPtr intPtr = LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06000609 RID: 1545
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_LongExposureInfraredFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x00022483 File Offset: 0x00020683
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameSource");
				}
				return LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x0600060B RID: 1547
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x000224B0 File Offset: 0x000206B0
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameSource");
				}
				IntPtr intPtr = LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00022520 File Offset: 0x00020720
		[MonoPInvokeCallback(typeof(LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				LongExposureInfraredFrameSource objThis = NativeObjectCache.GetObject<LongExposureInfraredFrameSource>(pNative);
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

		// Token: 0x0600060E RID: 1550
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameSource_add_FrameCaptured(IntPtr pNative, LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600060F RID: 1551 RVA: 0x000225E4 File Offset: 0x000207E4
		// (remove) Token: 0x06000610 RID: 1552 RVA: 0x00022678 File Offset: 0x00020878
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_FrameCaptured(this._pNative, new LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
						LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00022714 File Offset: 0x00020914
		[MonoPInvokeCallback(typeof(LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				LongExposureInfraredFrameSource objThis = NativeObjectCache.GetObject<LongExposureInfraredFrameSource>(pNative);
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

		// Token: 0x06000612 RID: 1554
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameSource_add_PropertyChanged(IntPtr pNative, LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000613 RID: 1555 RVA: 0x000227D8 File Offset: 0x000209D8
		// (remove) Token: 0x06000614 RID: 1556 RVA: 0x0002286C File Offset: 0x00020A6C
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_PropertyChanged(this._pNative, new LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000615 RID: 1557
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x06000616 RID: 1558 RVA: 0x00022908 File Offset: 0x00020B08
		public LongExposureInfraredFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrameSource");
			}
			IntPtr intPtr = LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameReader>(intPtr, (IntPtr n) => new LongExposureInfraredFrameReader(n));
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00022978 File Offset: 0x00020B78
		private void __EventCleanup()
		{
			LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_FrameCaptured(this._pNative, new LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
					}
					LongExposureInfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						LongExposureInfraredFrameSource.Windows_Kinect_LongExposureInfraredFrameSource_add_PropertyChanged(this._pNative, new LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					LongExposureInfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x040002CF RID: 719
		internal IntPtr _pNative;

		// Token: 0x040002D0 RID: 720
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x040002D1 RID: 721
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x040002D2 RID: 722
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x040002D3 RID: 723
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x020002A7 RID: 679
		// (Invoke) Token: 0x06001256 RID: 4694
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020002A8 RID: 680
		// (Invoke) Token: 0x0600125A RID: 4698
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
