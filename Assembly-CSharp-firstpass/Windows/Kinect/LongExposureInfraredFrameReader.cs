using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x0200007B RID: 123
	public sealed class LongExposureInfraredFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x00021B29 File Offset: 0x0001FD29
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00021B31 File Offset: 0x0001FD31
		internal LongExposureInfraredFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00021B4C File Offset: 0x0001FD4C
		~LongExposureInfraredFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x060005DF RID: 1503
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060005E0 RID: 1504
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x060005E1 RID: 1505 RVA: 0x00021B7C File Offset: 0x0001FD7C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrameReader>(this._pNative);
			if (disposing)
			{
				LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_Dispose(this._pNative);
			}
			LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060005E2 RID: 1506
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_LongExposureInfraredFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x060005E3 RID: 1507
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00021BD1 File Offset: 0x0001FDD1
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x00021BFB File Offset: 0x0001FDFB
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameReader");
				}
				return LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameReader");
				}
				LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x060005E6 RID: 1510
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameReader_get_LongExposureInfraredFrameSource(IntPtr pNative);

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00021C2C File Offset: 0x0001FE2C
		public LongExposureInfraredFrameSource LongExposureInfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameReader");
				}
				IntPtr intPtr = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_get_LongExposureInfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameSource>(intPtr, (IntPtr n) => new LongExposureInfraredFrameSource(n));
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00021C9C File Offset: 0x0001FE9C
		[MonoPInvokeCallback(typeof(LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> list = null;
			LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				LongExposureInfraredFrameReader objThis = NativeObjectCache.GetObject<LongExposureInfraredFrameReader>(pNative);
				LongExposureInfraredFrameArrivedEventArgs args = new LongExposureInfraredFrameArrivedEventArgs(result);
				using (List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<LongExposureInfraredFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x060005E9 RID: 1513
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_add_FrameArrived(IntPtr pNative, LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060005EA RID: 1514 RVA: 0x00021D60 File Offset: 0x0001FF60
		// (remove) Token: 0x060005EB RID: 1515 RVA: 0x00021DF4 File Offset: 0x0001FFF4
		public event EventHandler<LongExposureInfraredFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate = new LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handler);
						LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate);
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_FrameArrived(this._pNative, windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_FrameArrived(this._pNative, new LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handler), true);
						LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00021E90 File Offset: 0x00020090
		[MonoPInvokeCallback(typeof(LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				LongExposureInfraredFrameReader objThis = NativeObjectCache.GetObject<LongExposureInfraredFrameReader>(pNative);
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

		// Token: 0x060005ED RID: 1517
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_add_PropertyChanged(IntPtr pNative, LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060005EE RID: 1518 RVA: 0x00021F54 File Offset: 0x00020154
		// (remove) Token: 0x060005EF RID: 1519 RVA: 0x00021FE8 File Offset: 0x000201E8
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_PropertyChanged(this._pNative, new LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060005F0 RID: 1520
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x060005F1 RID: 1521 RVA: 0x00022084 File Offset: 0x00020284
		public LongExposureInfraredFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrameReader");
			}
			IntPtr intPtr = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrame>(intPtr, (IntPtr n) => new LongExposureInfraredFrame(n));
		}

		// Token: 0x060005F2 RID: 1522
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReader_Dispose(IntPtr pNative);

		// Token: 0x060005F3 RID: 1523 RVA: 0x000220F3 File Offset: 0x000202F3
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00022118 File Offset: 0x00020318
		private void __EventCleanup()
		{
			LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> list = LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_FrameArrived(this._pNative, new LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handler), true);
					}
					LongExposureInfraredFrameReader._Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						LongExposureInfraredFrameReader.Windows_Kinect_LongExposureInfraredFrameReader_add_PropertyChanged(this._pNative, new LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(LongExposureInfraredFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					LongExposureInfraredFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x040002C9 RID: 713
		internal IntPtr _pNative;

		// Token: 0x040002CA RID: 714
		private static GCHandle _Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x040002CB RID: 715
		private static CollectionMap<IntPtr, List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>>> Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<LongExposureInfraredFrameArrivedEventArgs>>>();

		// Token: 0x040002CC RID: 716
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x040002CD RID: 717
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0200029F RID: 671
		// (Invoke) Token: 0x06001241 RID: 4673
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020002A0 RID: 672
		// (Invoke) Token: 0x06001245 RID: 4677
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
