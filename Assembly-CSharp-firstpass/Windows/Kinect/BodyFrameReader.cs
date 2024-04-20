using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000052 RID: 82
	public sealed class BodyFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0001BDC5 File Offset: 0x00019FC5
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001BDCD File Offset: 0x00019FCD
		internal BodyFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrameReader.Windows_Kinect_BodyFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001BDE8 File Offset: 0x00019FE8
		~BodyFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06000402 RID: 1026
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000403 RID: 1027
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000404 RID: 1028 RVA: 0x0001BE18 File Offset: 0x0001A018
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrameReader>(this._pNative);
			if (disposing)
			{
				BodyFrameReader.Windows_Kinect_BodyFrameReader_Dispose(this._pNative);
			}
			BodyFrameReader.Windows_Kinect_BodyFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000405 RID: 1029
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameReader_get_BodyFrameSource(IntPtr pNative);

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0001BE70 File Offset: 0x0001A070
		public BodyFrameSource BodyFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameReader");
				}
				IntPtr intPtr = BodyFrameReader.Windows_Kinect_BodyFrameReader_get_BodyFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameSource>(intPtr, (IntPtr n) => new BodyFrameSource(n));
			}
		}

		// Token: 0x06000407 RID: 1031
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_BodyFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06000408 RID: 1032
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0001BEDF File Offset: 0x0001A0DF
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0001BF09 File Offset: 0x0001A109
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameReader");
				}
				return BodyFrameReader.Windows_Kinect_BodyFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameReader");
				}
				BodyFrameReader.Windows_Kinect_BodyFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001BF3C File Offset: 0x0001A13C
		[MonoPInvokeCallback(typeof(BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<BodyFrameArrivedEventArgs>> list = null;
			BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<BodyFrameArrivedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				BodyFrameReader objThis = NativeObjectCache.GetObject<BodyFrameReader>(pNative);
				BodyFrameArrivedEventArgs args = new BodyFrameArrivedEventArgs(result);
				using (List<EventHandler<BodyFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<BodyFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x0600040C RID: 1036
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_add_FrameArrived(IntPtr pNative, BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600040D RID: 1037 RVA: 0x0001C000 File Offset: 0x0001A200
		// (remove) Token: 0x0600040E RID: 1038 RVA: 0x0001C094 File Offset: 0x0001A294
		public event EventHandler<BodyFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<BodyFrameArrivedEventArgs>> list = BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<BodyFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate windows_Kinect_BodyFrameArrivedEventArgs_Delegate = new BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate(BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handler);
						BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_BodyFrameArrivedEventArgs_Delegate);
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_FrameArrived(this._pNative, windows_Kinect_BodyFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<BodyFrameArrivedEventArgs>> list = BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<BodyFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_FrameArrived(this._pNative, new BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate(BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handler), true);
						BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001C130 File Offset: 0x0001A330
		[MonoPInvokeCallback(typeof(BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				BodyFrameReader objThis = NativeObjectCache.GetObject<BodyFrameReader>(pNative);
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

		// Token: 0x06000410 RID: 1040
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_add_PropertyChanged(IntPtr pNative, BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000411 RID: 1041 RVA: 0x0001C1F4 File Offset: 0x0001A3F4
		// (remove) Token: 0x06000412 RID: 1042 RVA: 0x0001C288 File Offset: 0x0001A488
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_PropertyChanged(this._pNative, new BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000413 RID: 1043
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x06000414 RID: 1044 RVA: 0x0001C324 File Offset: 0x0001A524
		public BodyFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameReader");
			}
			IntPtr intPtr = BodyFrameReader.Windows_Kinect_BodyFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyFrame>(intPtr, (IntPtr n) => new BodyFrame(n));
		}

		// Token: 0x06000415 RID: 1045
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06000416 RID: 1046 RVA: 0x0001C393 File Offset: 0x0001A593
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001C3B8 File Offset: 0x0001A5B8
		private void __EventCleanup()
		{
			BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<BodyFrameArrivedEventArgs>> list = BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<BodyFrameArrivedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_FrameArrived(this._pNative, new BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate(BodyFrameReader.Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handler), true);
					}
					BodyFrameReader._Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						BodyFrameReader.Windows_Kinect_BodyFrameReader_add_PropertyChanged(this._pNative, new BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					BodyFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400022F RID: 559
		internal IntPtr _pNative;

		// Token: 0x04000230 RID: 560
		private static GCHandle _Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x04000231 RID: 561
		private static CollectionMap<IntPtr, List<EventHandler<BodyFrameArrivedEventArgs>>> Windows_Kinect_BodyFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<BodyFrameArrivedEventArgs>>>();

		// Token: 0x04000232 RID: 562
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04000233 RID: 563
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0200024F RID: 591
		// (Invoke) Token: 0x0600116A RID: 4458
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_BodyFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000250 RID: 592
		// (Invoke) Token: 0x0600116E RID: 4462
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
