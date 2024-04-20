using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000044 RID: 68
	public sealed class KinectSensor : INativeWrapper
	{
		// Token: 0x06000304 RID: 772 RVA: 0x00018CC4 File Offset: 0x00016EC4
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			if (this.IsOpen)
			{
				this.Close();
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<KinectSensor>(this._pNative);
			KinectSensor.Windows_Kinect_KinectSensor_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00018D19 File Offset: 0x00016F19
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00018D21 File Offset: 0x00016F21
		internal KinectSensor(IntPtr pNative)
		{
			this._pNative = pNative;
			KinectSensor.Windows_Kinect_KinectSensor_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00018D3C File Offset: 0x00016F3C
		~KinectSensor()
		{
			this.Dispose(false);
		}

		// Token: 0x06000308 RID: 776
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000309 RID: 777
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600030A RID: 778
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_AudioSource(IntPtr pNative);

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00018D6C File Offset: 0x00016F6C
		public AudioSource AudioSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_AudioSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioSource>(intPtr, (IntPtr n) => new AudioSource(n));
			}
		}

		// Token: 0x0600030C RID: 780
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_BodyFrameSource(IntPtr pNative);

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00018DDC File Offset: 0x00016FDC
		public BodyFrameSource BodyFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_BodyFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameSource>(intPtr, (IntPtr n) => new BodyFrameSource(n));
			}
		}

		// Token: 0x0600030E RID: 782
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_BodyIndexFrameSource(IntPtr pNative);

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00018E4C File Offset: 0x0001704C
		public BodyIndexFrameSource BodyIndexFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_BodyIndexFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameSource>(intPtr, (IntPtr n) => new BodyIndexFrameSource(n));
			}
		}

		// Token: 0x06000310 RID: 784
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_ColorFrameSource(IntPtr pNative);

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00018EBC File Offset: 0x000170BC
		public ColorFrameSource ColorFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_ColorFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameSource>(intPtr, (IntPtr n) => new ColorFrameSource(n));
			}
		}

		// Token: 0x06000312 RID: 786
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_CoordinateMapper(IntPtr pNative);

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00018F2C File Offset: 0x0001712C
		public CoordinateMapper CoordinateMapper
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_CoordinateMapper(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<CoordinateMapper>(intPtr, (IntPtr n) => new CoordinateMapper(n));
			}
		}

		// Token: 0x06000314 RID: 788
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_DepthFrameSource(IntPtr pNative);

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00018F9C File Offset: 0x0001719C
		public DepthFrameSource DepthFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_DepthFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameSource>(intPtr, (IntPtr n) => new DepthFrameSource(n));
			}
		}

		// Token: 0x06000316 RID: 790
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_InfraredFrameSource(IntPtr pNative);

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0001900C File Offset: 0x0001720C
		public InfraredFrameSource InfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_InfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameSource>(intPtr, (IntPtr n) => new InfraredFrameSource(n));
			}
		}

		// Token: 0x06000318 RID: 792
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_KinectSensor_get_IsAvailable(IntPtr pNative);

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0001907B File Offset: 0x0001727B
		public bool IsAvailable
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				return KinectSensor.Windows_Kinect_KinectSensor_get_IsAvailable(this._pNative);
			}
		}

		// Token: 0x0600031A RID: 794
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_KinectSensor_get_IsOpen(IntPtr pNative);

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600031B RID: 795 RVA: 0x000190A5 File Offset: 0x000172A5
		public bool IsOpen
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				return KinectSensor.Windows_Kinect_KinectSensor_get_IsOpen(this._pNative);
			}
		}

		// Token: 0x0600031C RID: 796
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern KinectCapabilities Windows_Kinect_KinectSensor_get_KinectCapabilities(IntPtr pNative);

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600031D RID: 797 RVA: 0x000190CF File Offset: 0x000172CF
		public KinectCapabilities KinectCapabilities
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				return KinectSensor.Windows_Kinect_KinectSensor_get_KinectCapabilities(this._pNative);
			}
		}

		// Token: 0x0600031E RID: 798
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_LongExposureInfraredFrameSource(IntPtr pNative);

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000190FC File Offset: 0x000172FC
		public LongExposureInfraredFrameSource LongExposureInfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_get_LongExposureInfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameSource>(intPtr, (IntPtr n) => new LongExposureInfraredFrameSource(n));
			}
		}

		// Token: 0x06000320 RID: 800
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_get_UniqueKinectId(IntPtr pNative);

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0001916C File Offset: 0x0001736C
		public string UniqueKinectId
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("KinectSensor");
				}
				IntPtr ptr = KinectSensor.Windows_Kinect_KinectSensor_get_UniqueKinectId(this._pNative);
				ExceptionHelper.CheckLastError();
				string result = Marshal.PtrToStringUni(ptr);
				Marshal.FreeCoTaskMem(ptr);
				return result;
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000191B4 File Offset: 0x000173B4
		[MonoPInvokeCallback(typeof(KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate))]
		private static void Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<IsAvailableChangedEventArgs>> list = null;
			KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<IsAvailableChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				KinectSensor objThis = NativeObjectCache.GetObject<KinectSensor>(pNative);
				IsAvailableChangedEventArgs args = new IsAvailableChangedEventArgs(result);
				using (List<EventHandler<IsAvailableChangedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<IsAvailableChangedEventArgs> func = enumerator.Current;
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

		// Token: 0x06000323 RID: 803
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_add_IsAvailableChanged(IntPtr pNative, KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000324 RID: 804 RVA: 0x00019278 File Offset: 0x00017478
		// (remove) Token: 0x06000325 RID: 805 RVA: 0x0001930C File Offset: 0x0001750C
		public event EventHandler<IsAvailableChangedEventArgs> IsAvailableChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<IsAvailableChangedEventArgs>> list = KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<IsAvailableChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate windows_Kinect_IsAvailableChangedEventArgs_Delegate = new KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate(KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handler);
						KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_IsAvailableChangedEventArgs_Delegate);
						KinectSensor.Windows_Kinect_KinectSensor_add_IsAvailableChanged(this._pNative, windows_Kinect_IsAvailableChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<IsAvailableChangedEventArgs>> list = KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<IsAvailableChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						KinectSensor.Windows_Kinect_KinectSensor_add_IsAvailableChanged(this._pNative, new KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate(KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handler), true);
						KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x000193A8 File Offset: 0x000175A8
		[MonoPInvokeCallback(typeof(KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				KinectSensor objThis = NativeObjectCache.GetObject<KinectSensor>(pNative);
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

		// Token: 0x06000327 RID: 807
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_add_PropertyChanged(IntPtr pNative, KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000328 RID: 808 RVA: 0x0001946C File Offset: 0x0001766C
		// (remove) Token: 0x06000329 RID: 809 RVA: 0x00019500 File Offset: 0x00017700
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate(KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						KinectSensor.Windows_Kinect_KinectSensor_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						KinectSensor.Windows_Kinect_KinectSensor_add_PropertyChanged(this._pNative, new KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate(KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x0600032A RID: 810
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_GetDefault();

		// Token: 0x0600032B RID: 811 RVA: 0x0001959C File Offset: 0x0001779C
		public static KinectSensor GetDefault()
		{
			IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_GetDefault();
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
		}

		// Token: 0x0600032C RID: 812
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_Open(IntPtr pNative);

		// Token: 0x0600032D RID: 813 RVA: 0x000195E8 File Offset: 0x000177E8
		public void Open()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("KinectSensor");
			}
			KinectSensor.Windows_Kinect_KinectSensor_Open(this._pNative);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600032E RID: 814
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_KinectSensor_Close(IntPtr pNative);

		// Token: 0x0600032F RID: 815 RVA: 0x00019617 File Offset: 0x00017817
		public void Close()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("KinectSensor");
			}
			KinectSensor.Windows_Kinect_KinectSensor_Close(this._pNative);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000330 RID: 816
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_KinectSensor_OpenMultiSourceFrameReader(IntPtr pNative, FrameSourceTypes enabledFrameSourceTypes);

		// Token: 0x06000331 RID: 817 RVA: 0x00019648 File Offset: 0x00017848
		public MultiSourceFrameReader OpenMultiSourceFrameReader(FrameSourceTypes enabledFrameSourceTypes)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("KinectSensor");
			}
			IntPtr intPtr = KinectSensor.Windows_Kinect_KinectSensor_OpenMultiSourceFrameReader(this._pNative, enabledFrameSourceTypes);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<MultiSourceFrameReader>(intPtr, (IntPtr n) => new MultiSourceFrameReader(n));
		}

		// Token: 0x06000332 RID: 818 RVA: 0x000196B8 File Offset: 0x000178B8
		private void __EventCleanup()
		{
			KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<IsAvailableChangedEventArgs>> list = KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<IsAvailableChangedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						KinectSensor.Windows_Kinect_KinectSensor_add_IsAvailableChanged(this._pNative, new KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate(KinectSensor.Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handler), true);
					}
					KinectSensor._Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handle.Free();
				}
			}
			KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						KinectSensor.Windows_Kinect_KinectSensor_add_PropertyChanged(this._pNative, new KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate(KinectSensor.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					KinectSensor._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04000208 RID: 520
		internal IntPtr _pNative;

		// Token: 0x04000209 RID: 521
		private static GCHandle _Windows_Kinect_IsAvailableChangedEventArgs_Delegate_Handle;

		// Token: 0x0400020A RID: 522
		private static CollectionMap<IntPtr, List<EventHandler<IsAvailableChangedEventArgs>>> Windows_Kinect_IsAvailableChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<IsAvailableChangedEventArgs>>>();

		// Token: 0x0400020B RID: 523
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400020C RID: 524
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0200022F RID: 559
		// (Invoke) Token: 0x0600110C RID: 4364
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_IsAvailableChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000230 RID: 560
		// (Invoke) Token: 0x06001110 RID: 4368
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
