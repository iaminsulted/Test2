using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000058 RID: 88
	public sealed class BodyIndexFrameSource : INativeWrapper
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001D6CD File Offset: 0x0001B8CD
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001D6D5 File Offset: 0x0001B8D5
		internal BodyIndexFrameSource(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001D6F0 File Offset: 0x0001B8F0
		~BodyIndexFrameSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06000471 RID: 1137
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000472 RID: 1138
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000473 RID: 1139 RVA: 0x0001D720 File Offset: 0x0001B920
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrameSource>(this._pNative);
			BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000474 RID: 1140
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameSource_get_FrameDescription(IntPtr pNative);

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x0001D75C File Offset: 0x0001B95C
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameSource");
				}
				IntPtr intPtr = BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06000476 RID: 1142
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_BodyIndexFrameSource_get_IsActive(IntPtr pNative);

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0001D7CB File Offset: 0x0001B9CB
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameSource");
				}
				return BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x06000478 RID: 1144
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x0001D7F8 File Offset: 0x0001B9F8
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameSource");
				}
				IntPtr intPtr = BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001D868 File Offset: 0x0001BA68
		[MonoPInvokeCallback(typeof(BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				BodyIndexFrameSource objThis = NativeObjectCache.GetObject<BodyIndexFrameSource>(pNative);
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

		// Token: 0x0600047B RID: 1147
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameSource_add_FrameCaptured(IntPtr pNative, BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600047C RID: 1148 RVA: 0x0001D92C File Offset: 0x0001BB2C
		// (remove) Token: 0x0600047D RID: 1149 RVA: 0x0001D9C0 File Offset: 0x0001BBC0
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_FrameCaptured(this._pNative, new BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
						BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001DA5C File Offset: 0x0001BC5C
		[MonoPInvokeCallback(typeof(BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				BodyIndexFrameSource objThis = NativeObjectCache.GetObject<BodyIndexFrameSource>(pNative);
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

		// Token: 0x0600047F RID: 1151
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameSource_add_PropertyChanged(IntPtr pNative, BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000480 RID: 1152 RVA: 0x0001DB20 File Offset: 0x0001BD20
		// (remove) Token: 0x06000481 RID: 1153 RVA: 0x0001DBB4 File Offset: 0x0001BDB4
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_PropertyChanged(this._pNative, new BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000482 RID: 1154
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameSource_OpenReader(IntPtr pNative);

		// Token: 0x06000483 RID: 1155 RVA: 0x0001DC50 File Offset: 0x0001BE50
		public BodyIndexFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrameSource");
			}
			IntPtr intPtr = BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyIndexFrameReader>(intPtr, (IntPtr n) => new BodyIndexFrameReader(n));
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001DCC0 File Offset: 0x0001BEC0
		private void __EventCleanup()
		{
			BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_FrameCaptured(this._pNative, new BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(BodyIndexFrameSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
					}
					BodyIndexFrameSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						BodyIndexFrameSource.Windows_Kinect_BodyIndexFrameSource_add_PropertyChanged(this._pNative, new BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate(BodyIndexFrameSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					BodyIndexFrameSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04000241 RID: 577
		internal IntPtr _pNative;

		// Token: 0x04000242 RID: 578
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x04000243 RID: 579
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x04000244 RID: 580
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04000245 RID: 581
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x02000267 RID: 615
		// (Invoke) Token: 0x060011A9 RID: 4521
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000268 RID: 616
		// (Invoke) Token: 0x060011AD RID: 4525
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
