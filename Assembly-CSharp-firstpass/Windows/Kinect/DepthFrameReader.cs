using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000063 RID: 99
	public sealed class DepthFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001F37D File Offset: 0x0001D57D
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001F385 File Offset: 0x0001D585
		internal DepthFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrameReader.Windows_Kinect_DepthFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001F3A0 File Offset: 0x0001D5A0
		~DepthFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06000504 RID: 1284
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000505 RID: 1285
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000506 RID: 1286 RVA: 0x0001F3D0 File Offset: 0x0001D5D0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrameReader>(this._pNative);
			if (disposing)
			{
				DepthFrameReader.Windows_Kinect_DepthFrameReader_Dispose(this._pNative);
			}
			DepthFrameReader.Windows_Kinect_DepthFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000507 RID: 1287
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameReader_get_DepthFrameSource(IntPtr pNative);

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0001F428 File Offset: 0x0001D628
		public DepthFrameSource DepthFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameReader");
				}
				IntPtr intPtr = DepthFrameReader.Windows_Kinect_DepthFrameReader_get_DepthFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameSource>(intPtr, (IntPtr n) => new DepthFrameSource(n));
			}
		}

		// Token: 0x06000509 RID: 1289
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_DepthFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x0600050A RID: 1290
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001F497 File Offset: 0x0001D697
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0001F4C1 File Offset: 0x0001D6C1
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameReader");
				}
				return DepthFrameReader.Windows_Kinect_DepthFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameReader");
				}
				DepthFrameReader.Windows_Kinect_DepthFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001F4F4 File Offset: 0x0001D6F4
		[MonoPInvokeCallback(typeof(DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<DepthFrameArrivedEventArgs>> list = null;
			DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<DepthFrameArrivedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				DepthFrameReader objThis = NativeObjectCache.GetObject<DepthFrameReader>(pNative);
				DepthFrameArrivedEventArgs args = new DepthFrameArrivedEventArgs(result);
				using (List<EventHandler<DepthFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<DepthFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x0600050E RID: 1294
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_add_FrameArrived(IntPtr pNative, DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600050F RID: 1295 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
		// (remove) Token: 0x06000510 RID: 1296 RVA: 0x0001F64C File Offset: 0x0001D84C
		public event EventHandler<DepthFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<DepthFrameArrivedEventArgs>> list = DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<DepthFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate windows_Kinect_DepthFrameArrivedEventArgs_Delegate = new DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate(DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handler);
						DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_DepthFrameArrivedEventArgs_Delegate);
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_FrameArrived(this._pNative, windows_Kinect_DepthFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<DepthFrameArrivedEventArgs>> list = DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<DepthFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_FrameArrived(this._pNative, new DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate(DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handler), true);
						DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001F6E8 File Offset: 0x0001D8E8
		[MonoPInvokeCallback(typeof(DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				DepthFrameReader objThis = NativeObjectCache.GetObject<DepthFrameReader>(pNative);
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

		// Token: 0x06000512 RID: 1298
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_add_PropertyChanged(IntPtr pNative, DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000513 RID: 1299 RVA: 0x0001F7AC File Offset: 0x0001D9AC
		// (remove) Token: 0x06000514 RID: 1300 RVA: 0x0001F840 File Offset: 0x0001DA40
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_PropertyChanged(this._pNative, new DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000515 RID: 1301
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x06000516 RID: 1302 RVA: 0x0001F8DC File Offset: 0x0001DADC
		public DepthFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrameReader");
			}
			IntPtr intPtr = DepthFrameReader.Windows_Kinect_DepthFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<DepthFrame>(intPtr, (IntPtr n) => new DepthFrame(n));
		}

		// Token: 0x06000517 RID: 1303
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06000518 RID: 1304 RVA: 0x0001F94B File Offset: 0x0001DB4B
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001F970 File Offset: 0x0001DB70
		private void __EventCleanup()
		{
			DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<DepthFrameArrivedEventArgs>> list = DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<DepthFrameArrivedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_FrameArrived(this._pNative, new DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate(DepthFrameReader.Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handler), true);
					}
					DepthFrameReader._Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						DepthFrameReader.Windows_Kinect_DepthFrameReader_add_PropertyChanged(this._pNative, new DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(DepthFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					DepthFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04000261 RID: 609
		internal IntPtr _pNative;

		// Token: 0x04000262 RID: 610
		private static GCHandle _Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x04000263 RID: 611
		private static CollectionMap<IntPtr, List<EventHandler<DepthFrameArrivedEventArgs>>> Windows_Kinect_DepthFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<DepthFrameArrivedEventArgs>>>();

		// Token: 0x04000264 RID: 612
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04000265 RID: 613
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0200027F RID: 639
		// (Invoke) Token: 0x060011EB RID: 4587
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_DepthFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000280 RID: 640
		// (Invoke) Token: 0x060011EF RID: 4591
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
