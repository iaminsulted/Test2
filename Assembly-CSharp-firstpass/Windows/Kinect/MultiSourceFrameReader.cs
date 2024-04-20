using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000080 RID: 128
	public sealed class MultiSourceFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00022EE5 File Offset: 0x000210E5
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00022EED File Offset: 0x000210ED
		internal MultiSourceFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00022F08 File Offset: 0x00021108
		~MultiSourceFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x06000638 RID: 1592
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000639 RID: 1593
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600063A RID: 1594 RVA: 0x00022F38 File Offset: 0x00021138
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<MultiSourceFrameReader>(this._pNative);
			if (disposing)
			{
				MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_Dispose(this._pNative);
			}
			MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600063B RID: 1595
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern FrameSourceTypes Windows_Kinect_MultiSourceFrameReader_get_FrameSourceTypes(IntPtr pNative);

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x00022F8D File Offset: 0x0002118D
		public FrameSourceTypes FrameSourceTypes
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameReader");
				}
				return MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_get_FrameSourceTypes(this._pNative);
			}
		}

		// Token: 0x0600063D RID: 1597
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_MultiSourceFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x0600063E RID: 1598
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00022FB7 File Offset: 0x000211B7
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x00022FE1 File Offset: 0x000211E1
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameReader");
				}
				return MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameReader");
				}
				MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06000641 RID: 1601
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrameReader_get_KinectSensor(IntPtr pNative);

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x00023014 File Offset: 0x00021214
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameReader");
				}
				IntPtr intPtr = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00023084 File Offset: 0x00021284
		[MonoPInvokeCallback(typeof(MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<MultiSourceFrameArrivedEventArgs>> list = null;
			MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<MultiSourceFrameArrivedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				MultiSourceFrameReader objThis = NativeObjectCache.GetObject<MultiSourceFrameReader>(pNative);
				MultiSourceFrameArrivedEventArgs args = new MultiSourceFrameArrivedEventArgs(result);
				using (List<EventHandler<MultiSourceFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<MultiSourceFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06000644 RID: 1604
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_add_MultiSourceFrameArrived(IntPtr pNative, MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000645 RID: 1605 RVA: 0x00023148 File Offset: 0x00021348
		// (remove) Token: 0x06000646 RID: 1606 RVA: 0x000231DC File Offset: 0x000213DC
		public event EventHandler<MultiSourceFrameArrivedEventArgs> MultiSourceFrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<MultiSourceFrameArrivedEventArgs>> list = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<MultiSourceFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate = new MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate(MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handler);
						MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate);
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_MultiSourceFrameArrived(this._pNative, windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<MultiSourceFrameArrivedEventArgs>> list = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<MultiSourceFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_MultiSourceFrameArrived(this._pNative, new MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate(MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handler), true);
						MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00023278 File Offset: 0x00021478
		[MonoPInvokeCallback(typeof(MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				MultiSourceFrameReader objThis = NativeObjectCache.GetObject<MultiSourceFrameReader>(pNative);
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

		// Token: 0x06000648 RID: 1608
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_add_PropertyChanged(IntPtr pNative, MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000649 RID: 1609 RVA: 0x0002333C File Offset: 0x0002153C
		// (remove) Token: 0x0600064A RID: 1610 RVA: 0x000233D0 File Offset: 0x000215D0
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_PropertyChanged(this._pNative, new MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x0600064B RID: 1611
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x0600064C RID: 1612 RVA: 0x0002346C File Offset: 0x0002166C
		public MultiSourceFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("MultiSourceFrameReader");
			}
			IntPtr intPtr = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<MultiSourceFrame>(intPtr, (IntPtr n) => new MultiSourceFrame(n));
		}

		// Token: 0x0600064D RID: 1613
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReader_Dispose(IntPtr pNative);

		// Token: 0x0600064E RID: 1614 RVA: 0x000234DB File Offset: 0x000216DB
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00023500 File Offset: 0x00021700
		private void __EventCleanup()
		{
			MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<MultiSourceFrameArrivedEventArgs>> list = MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<MultiSourceFrameArrivedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_MultiSourceFrameArrived(this._pNative, new MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate(MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handler), true);
					}
					MultiSourceFrameReader._Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						MultiSourceFrameReader.Windows_Kinect_MultiSourceFrameReader_add_PropertyChanged(this._pNative, new MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(MultiSourceFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					MultiSourceFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x040002D6 RID: 726
		internal IntPtr _pNative;

		// Token: 0x040002D7 RID: 727
		private static GCHandle _Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x040002D8 RID: 728
		private static CollectionMap<IntPtr, List<EventHandler<MultiSourceFrameArrivedEventArgs>>> Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<MultiSourceFrameArrivedEventArgs>>>();

		// Token: 0x040002D9 RID: 729
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x040002DA RID: 730
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x020002B0 RID: 688
		// (Invoke) Token: 0x06001274 RID: 4724
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_MultiSourceFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x020002B1 RID: 689
		// (Invoke) Token: 0x06001278 RID: 4728
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
