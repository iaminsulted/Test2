using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000050 RID: 80
	public sealed class AudioSource : INativeWrapper
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0001B4B4 File Offset: 0x000196B4
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001B4BC File Offset: 0x000196BC
		internal AudioSource(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioSource.Windows_Kinect_AudioSource_AddRefObject(ref this._pNative);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001B4D8 File Offset: 0x000196D8
		~AudioSource()
		{
			this.Dispose(false);
		}

		// Token: 0x060003D8 RID: 984
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioSource_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060003D9 RID: 985
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioSource_AddRefObject(ref IntPtr pNative);

		// Token: 0x060003DA RID: 986 RVA: 0x0001B508 File Offset: 0x00019708
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioSource>(this._pNative);
			AudioSource.Windows_Kinect_AudioSource_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060003DB RID: 987
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioSource_get_AudioBeams(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x060003DC RID: 988
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioSource_get_AudioBeams_Length(IntPtr pNative);

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0001B544 File Offset: 0x00019744
		public IList<AudioBeam> AudioBeams
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				int num = AudioSource.Windows_Kinect_AudioSource_get_AudioBeams_Length(this._pNative);
				IntPtr[] array = new IntPtr[num];
				AudioBeam[] array2 = new AudioBeam[num];
				num = AudioSource.Windows_Kinect_AudioSource_get_AudioBeams(this._pNative, array, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					if (!(array[i] == IntPtr.Zero))
					{
						AudioBeam audioBeam = NativeObjectCache.CreateOrGetObject<AudioBeam>(array[i], (IntPtr n) => new AudioBeam(n));
						array2[i] = audioBeam;
					}
				}
				return array2;
			}
		}

		// Token: 0x060003DE RID: 990
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_AudioSource_get_IsActive(IntPtr pNative);

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0001B5E5 File Offset: 0x000197E5
		public bool IsActive
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return AudioSource.Windows_Kinect_AudioSource_get_IsActive(this._pNative);
			}
		}

		// Token: 0x060003E0 RID: 992
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioSource_get_KinectSensor(IntPtr pNative);

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001B610 File Offset: 0x00019810
		public KinectSensor KinectSensor
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				IntPtr intPtr = AudioSource.Windows_Kinect_AudioSource_get_KinectSensor(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<KinectSensor>(intPtr, (IntPtr n) => new KinectSensor(n));
			}
		}

		// Token: 0x060003E2 RID: 994
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_AudioSource_get_MaxSubFrameCount(IntPtr pNative);

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0001B67F File Offset: 0x0001987F
		public uint MaxSubFrameCount
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return AudioSource.Windows_Kinect_AudioSource_get_MaxSubFrameCount(this._pNative);
			}
		}

		// Token: 0x060003E4 RID: 996
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioSource_get_SubFrameDuration(IntPtr pNative);

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0001B6A9 File Offset: 0x000198A9
		public TimeSpan SubFrameDuration
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return TimeSpan.FromMilliseconds((double)AudioSource.Windows_Kinect_AudioSource_get_SubFrameDuration(this._pNative));
			}
		}

		// Token: 0x060003E6 RID: 998
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_AudioSource_get_SubFrameLengthInBytes(IntPtr pNative);

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0001B6D9 File Offset: 0x000198D9
		public uint SubFrameLengthInBytes
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return AudioSource.Windows_Kinect_AudioSource_get_SubFrameLengthInBytes(this._pNative);
			}
		}

		// Token: 0x060003E8 RID: 1000
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern KinectAudioCalibrationState Windows_Kinect_AudioSource_get_AudioCalibrationState(IntPtr pNative);

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0001B703 File Offset: 0x00019903
		public KinectAudioCalibrationState AudioCalibrationState
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioSource");
				}
				return AudioSource.Windows_Kinect_AudioSource_get_AudioCalibrationState(this._pNative);
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001B730 File Offset: 0x00019930
		[MonoPInvokeCallback(typeof(AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate))]
		private static void Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<FrameCapturedEventArgs>> list = null;
			AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				AudioSource objThis = NativeObjectCache.GetObject<AudioSource>(pNative);
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

		// Token: 0x060003EB RID: 1003
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioSource_add_FrameCaptured(IntPtr pNative, AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060003EC RID: 1004 RVA: 0x0001B7F4 File Offset: 0x000199F4
		// (remove) Token: 0x060003ED RID: 1005 RVA: 0x0001B888 File Offset: 0x00019A88
		public event EventHandler<FrameCapturedEventArgs> FrameCaptured
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate windows_Kinect_FrameCapturedEventArgs_Delegate = new AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler);
						AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_FrameCapturedEventArgs_Delegate);
						AudioSource.Windows_Kinect_AudioSource_add_FrameCaptured(this._pNative, windows_Kinect_FrameCapturedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<FrameCapturedEventArgs>> list = AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<FrameCapturedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						AudioSource.Windows_Kinect_AudioSource_add_FrameCaptured(this._pNative, new AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
						AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001B924 File Offset: 0x00019B24
		[MonoPInvokeCallback(typeof(AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				AudioSource objThis = NativeObjectCache.GetObject<AudioSource>(pNative);
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

		// Token: 0x060003EF RID: 1007
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioSource_add_PropertyChanged(IntPtr pNative, AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060003F0 RID: 1008 RVA: 0x0001B9E8 File Offset: 0x00019BE8
		// (remove) Token: 0x060003F1 RID: 1009 RVA: 0x0001BA7C File Offset: 0x00019C7C
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate(AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						AudioSource.Windows_Kinect_AudioSource_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						AudioSource.Windows_Kinect_AudioSource_add_PropertyChanged(this._pNative, new AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate(AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060003F2 RID: 1010
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioSource_OpenReader(IntPtr pNative);

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001BB18 File Offset: 0x00019D18
		public AudioBeamFrameReader OpenReader()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioSource");
			}
			IntPtr intPtr = AudioSource.Windows_Kinect_AudioSource_OpenReader(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<AudioBeamFrameReader>(intPtr, (IntPtr n) => new AudioBeamFrameReader(n));
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001BB88 File Offset: 0x00019D88
		private void __EventCleanup()
		{
			AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<FrameCapturedEventArgs>> list = AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<FrameCapturedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						AudioSource.Windows_Kinect_AudioSource_add_FrameCaptured(this._pNative, new AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate(AudioSource.Windows_Kinect_FrameCapturedEventArgs_Delegate_Handler), true);
					}
					AudioSource._Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle.Free();
				}
			}
			AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						AudioSource.Windows_Kinect_AudioSource_add_PropertyChanged(this._pNative, new AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate(AudioSource.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					AudioSource._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x04000229 RID: 553
		internal IntPtr _pNative;

		// Token: 0x0400022A RID: 554
		private static GCHandle _Windows_Kinect_FrameCapturedEventArgs_Delegate_Handle;

		// Token: 0x0400022B RID: 555
		private static CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>> Windows_Kinect_FrameCapturedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<FrameCapturedEventArgs>>>();

		// Token: 0x0400022C RID: 556
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400022D RID: 557
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x02000247 RID: 583
		// (Invoke) Token: 0x06001154 RID: 4436
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_FrameCapturedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000248 RID: 584
		// (Invoke) Token: 0x06001158 RID: 4440
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
