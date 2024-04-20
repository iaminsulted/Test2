using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000072 RID: 114
	public sealed class InfraredFrameSource : INativeWrapper
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00021045 File Offset: 0x0001F245
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0002104D File Offset: 0x0001F24D
		internal InfraredFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrameSource.Windows_Kinect_InfraredFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00021068 File Offset: 0x0001F268
		~InfraredFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x0600059E RID: 1438
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600059F RID: 1439
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x060005A0 RID: 1440 RVA: 0x00021098 File Offset: 0x0001F298
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrameSource>(this._pNative);
			InfraredFrameSource.Windows_Kinect_InfraredFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060005A1 RID: 1441
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x000210D4 File Offset: 0x0001F2D4
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameSource");
				}
				IntPtr intPtr = InfraredFrameSource.Windows_Kinect_InfraredFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x060005A3 RID: 1443
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_InfraredFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00021143 File Offset: 0x0001F343
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameSource");
				}
				return InfraredFrameSource.Windows_Kinect_InfraredFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x060005A5 RID: 1445
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00021170 File Offset: 0x0001F370
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameSource");
				}
				IntPtr intPtr = InfraredFrameSource.Windows_Kinect_InfraredFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000211E0 File Offset: 0x0001F3E0
		[MonoPInvokeCallback(typeof(InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				InfraredFrameSource objThis = NativeObjectCache.GetObject<InfraredFrameSource>(pNative);
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

		// Token: 0x060005A8 RID: 1448
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameSource_add_FrameCaptured(IntPtr pNative, InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060005A9 RID: 1449 RVA: 0x000212A4 File Offset: 0x0001F4A4
		// (remove) Token: 0x060005AA RID: 1450 RVA: 0x00021338 File Offset: 0x0001F538
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_FrameCaptured(this._pNative, new InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
						InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x000213D4 File Offset: 0x0001F5D4
		[MonoPInvokeCallback(typeof(InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				InfraredFrameSource objThis = NativeObjectCache.GetObject<InfraredFrameSource>(pNative);
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

		// Token: 0x060005AC RID: 1452
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameSource_add_PropertyChanged(IntPtr pNative, InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060005AD RID: 1453 RVA: 0x00021498 File Offset: 0x0001F698
		// (remove) Token: 0x060005AE RID: 1454 RVA: 0x0002152C File Offset: 0x0001F72C
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_PropertyChanged(this._pNative, new InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060005AF RID: 1455
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x060005B0 RID: 1456 RVA: 0x000215C8 File Offset: 0x0001F7C8
		public InfraredFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrameSource");
			}
			IntPtr intPtr = InfraredFrameSource.Windows_Kinect_InfraredFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<InfraredFrameReader>(intPtr, (IntPtr n) => new InfraredFrameReader(n));
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00021638 File Offset: 0x0001F838
		private void __EventCleanup()
		{
			InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_FrameCaptured(this._pNative, new InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(InfraredFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
					}
					InfraredFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						InfraredFrameSource.Windows_Kinect_InfraredFrameSource_add_PropertyChanged(this._pNative, new InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(InfraredFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					InfraredFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04000298 RID: 664
		internal IntPtr _pNative;

		// Token: 0x04000299 RID: 665
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x0400029A RID: 666
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x0400029B RID: 667
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400029C RID: 668
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x02000297 RID: 663
		// (Invoke) Token: 0x0600122B RID: 4651
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000298 RID: 664
		// (Invoke) Token: 0x0600122F RID: 4655
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
