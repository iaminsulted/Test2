using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000054 RID: 84
	public sealed class BodyFrameSource : INativeWrapper
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0001C625 File Offset: 0x0001A825
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001C62D File Offset: 0x0001A82D
		internal BodyFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrameSource.Windows_Kinect_BodyFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001C648 File Offset: 0x0001A848
		~BodyFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06000427 RID: 1063
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000428 RID: 1064
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000429 RID: 1065 RVA: 0x0001C678 File Offset: 0x0001A878
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrameSource>(this._pNative);
			BodyFrameSource.Windows_Kinect_BodyFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600042A RID: 1066
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_BodyFrameSource_get_BodyCount(IntPtr pNative);

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0001C6B4 File Offset: 0x0001A8B4
		public int BodyCount
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameSource");
				}
				return BodyFrameSource.Windows_Kinect_BodyFrameSource_get_BodyCount(this._pNative);
			}
		}

		// Token: 0x0600042C RID: 1068
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_BodyFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0001C6DE File Offset: 0x0001A8DE
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameSource");
				}
				return BodyFrameSource.Windows_Kinect_BodyFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x0600042E RID: 1070
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0001C708 File Offset: 0x0001A908
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameSource");
				}
				IntPtr intPtr = BodyFrameSource.Windows_Kinect_BodyFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001C778 File Offset: 0x0001A978
		[MonoPInvokeCallback(typeof(BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				BodyFrameSource objThis = NativeObjectCache.GetObject<BodyFrameSource>(pNative);
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

		// Token: 0x06000431 RID: 1073
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_add_FrameCaptured(IntPtr pNative, BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000432 RID: 1074 RVA: 0x0001C83C File Offset: 0x0001AA3C
		// (remove) Token: 0x06000433 RID: 1075 RVA: 0x0001C8D0 File Offset: 0x0001AAD0
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_FrameCaptured(this._pNative, new BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
						BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001C96C File Offset: 0x0001AB6C
		[MonoPInvokeCallback(typeof(BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				BodyFrameSource objThis = NativeObjectCache.GetObject<BodyFrameSource>(pNative);
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

		// Token: 0x06000435 RID: 1077
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_add_PropertyChanged(IntPtr pNative, BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000436 RID: 1078 RVA: 0x0001CA30 File Offset: 0x0001AC30
		// (remove) Token: 0x06000437 RID: 1079 RVA: 0x0001CAC4 File Offset: 0x0001ACC4
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_PropertyChanged(this._pNative, new BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000438 RID: 1080
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x06000439 RID: 1081 RVA: 0x0001CB60 File Offset: 0x0001AD60
		public BodyFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameSource");
			}
			IntPtr intPtr = BodyFrameSource.Windows_Kinect_BodyFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyFrameReader>(intPtr, (IntPtr n) => new BodyFrameReader(n));
		}

		// Token: 0x0600043A RID: 1082
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_OverrideHandTracking(IntPtr pNative, ulong trackingId);

		// Token: 0x0600043B RID: 1083 RVA: 0x0001CBCF File Offset: 0x0001ADCF
		public void OverrideHandTracking(ulong trackingId)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameSource");
			}
			BodyFrameSource.Windows_Kinect_BodyFrameSource_OverrideHandTracking(this._pNative, trackingId);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600043C RID: 1084
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameSource_OverrideHandTracking_1(IntPtr pNative, ulong oldTrackingId, ulong newTrackingId);

		// Token: 0x0600043D RID: 1085 RVA: 0x0001CBFF File Offset: 0x0001ADFF
		public void OverrideHandTracking(ulong oldTrackingId, ulong newTrackingId)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameSource");
			}
			BodyFrameSource.Windows_Kinect_BodyFrameSource_OverrideHandTracking_1(this._pNative, oldTrackingId, newTrackingId);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001CC30 File Offset: 0x0001AE30
		private void __EventCleanup()
		{
			BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_FrameCaptured(this._pNative, new BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
					}
					BodyFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						BodyFrameSource.Windows_Kinect_BodyFrameSource_add_PropertyChanged(this._pNative, new BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					BodyFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04000235 RID: 565
		internal IntPtr _pNative;

		// Token: 0x04000236 RID: 566
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x04000237 RID: 567
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x04000238 RID: 568
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04000239 RID: 569
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x02000257 RID: 599
		// (Invoke) Token: 0x0600117F RID: 4479
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000258 RID: 600
		// (Invoke) Token: 0x06001183 RID: 4483
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
