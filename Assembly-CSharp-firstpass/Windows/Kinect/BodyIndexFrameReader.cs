using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000056 RID: 86
	public sealed class BodyIndexFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0001CE6D File Offset: 0x0001B06D
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001CE75 File Offset: 0x0001B075
		internal BodyIndexFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001CE90 File Offset: 0x0001B090
		~BodyIndexFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x0600044C RID: 1100
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600044D RID: 1101
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600044E RID: 1102 RVA: 0x0001CEC0 File Offset: 0x0001B0C0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrameReader>(this._pNative);
			if (disposing)
			{
				BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_Dispose(this._pNative);
			}
			BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600044F RID: 1103
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameReader_get_BodyIndexFrameSource(IntPtr pNative);

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0001CF18 File Offset: 0x0001B118
		public BodyIndexFrameSource BodyIndexFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameReader");
				}
				IntPtr intPtr = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_get_BodyIndexFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameSource>(intPtr, (IntPtr n) => new BodyIndexFrameSource(n));
			}
		}

		// Token: 0x06000451 RID: 1105
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_BodyIndexFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x06000452 RID: 1106
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x0001CF87 File Offset: 0x0001B187
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x0001CFB1 File Offset: 0x0001B1B1
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameReader");
				}
				return BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameReader");
				}
				BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		[MonoPInvokeCallback(typeof(BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<BodyIndexFrameArrivedEventArgs>> list = null;
			BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<BodyIndexFrameArrivedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				BodyIndexFrameReader objThis = NativeObjectCache.GetObject<BodyIndexFrameReader>(pNative);
				BodyIndexFrameArrivedEventArgs args = new BodyIndexFrameArrivedEventArgs(result);
				using (List<EventHandler<BodyIndexFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<BodyIndexFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x06000456 RID: 1110
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_add_FrameArrived(IntPtr pNative, BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000457 RID: 1111 RVA: 0x0001D0A8 File Offset: 0x0001B2A8
		// (remove) Token: 0x06000458 RID: 1112 RVA: 0x0001D13C File Offset: 0x0001B33C
		public event EventHandler<BodyIndexFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<BodyIndexFrameArrivedEventArgs>> list = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<BodyIndexFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate = new BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate(BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handler);
						BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate);
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_FrameArrived(this._pNative, windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<BodyIndexFrameArrivedEventArgs>> list = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<BodyIndexFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_FrameArrived(this._pNative, new BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate(BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handler), true);
						BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0001D1D8 File Offset: 0x0001B3D8
		[MonoPInvokeCallback(typeof(BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				BodyIndexFrameReader objThis = NativeObjectCache.GetObject<BodyIndexFrameReader>(pNative);
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

		// Token: 0x0600045A RID: 1114
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_add_PropertyChanged(IntPtr pNative, BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600045B RID: 1115 RVA: 0x0001D29C File Offset: 0x0001B49C
		// (remove) Token: 0x0600045C RID: 1116 RVA: 0x0001D330 File Offset: 0x0001B530
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_PropertyChanged(this._pNative, new BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x0600045D RID: 1117
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameReader_AcquireLatestFrame(IntPtr pNative);

		// Token: 0x0600045E RID: 1118 RVA: 0x0001D3CC File Offset: 0x0001B5CC
		public BodyIndexFrame AcquireLatestFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrameReader");
			}
			IntPtr intPtr = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_AcquireLatestFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyIndexFrame>(intPtr, (IntPtr n) => new BodyIndexFrame(n));
		}

		// Token: 0x0600045F RID: 1119
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReader_Dispose(IntPtr pNative);

		// Token: 0x06000460 RID: 1120 RVA: 0x0001D43B File Offset: 0x0001B63B
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001D460 File Offset: 0x0001B660
		private void __EventCleanup()
		{
			BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<BodyIndexFrameArrivedEventArgs>> list = BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<BodyIndexFrameArrivedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_FrameArrived(this._pNative, new BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate(BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handler), true);
					}
					BodyIndexFrameReader._Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						BodyIndexFrameReader.Windows_Kinect_BodyIndexFrameReader_add_PropertyChanged(this._pNative, new BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					BodyIndexFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400023B RID: 571
		internal IntPtr _pNative;

		// Token: 0x0400023C RID: 572
		private static GCHandle _Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x0400023D RID: 573
		private static CollectionMap<IntPtr, List<EventHandler<BodyIndexFrameArrivedEventArgs>>> Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<BodyIndexFrameArrivedEventArgs>>>();

		// Token: 0x0400023E RID: 574
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400023F RID: 575
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0200025F RID: 607
		// (Invoke) Token: 0x06001194 RID: 4500
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_BodyIndexFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000260 RID: 608
		// (Invoke) Token: 0x06001198 RID: 4504
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
