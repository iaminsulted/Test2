using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x0200004C RID: 76
	public sealed class AudioBeamFrameReader : IDisposable, INativeWrapper
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0001AB35 File Offset: 0x00018D35
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001AB3D File Offset: 0x00018D3D
		internal AudioBeamFrameReader(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_AddRefObject(ref this._pNative);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001AB58 File Offset: 0x00018D58
		~AudioBeamFrameReader()
		{
			this.Dispose(false);
		}

		// Token: 0x060003A8 RID: 936
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060003A9 RID: 937
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_AddRefObject(ref IntPtr pNative);

		// Token: 0x060003AA RID: 938 RVA: 0x0001AB88 File Offset: 0x00018D88
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrameReader>(this._pNative);
			if (disposing)
			{
				AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_Dispose(this._pNative);
			}
			AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060003AB RID: 939
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamFrameReader_get_AudioSource(IntPtr pNative);

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0001ABE0 File Offset: 0x00018DE0
		public AudioSource AudioSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameReader");
				}
				IntPtr intPtr = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_get_AudioSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioSource>(intPtr, (IntPtr n) => new AudioSource(n));
			}
		}

		// Token: 0x060003AD RID: 941
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_AudioBeamFrameReader_get_IsPaused(IntPtr pNative);

		// Token: 0x060003AE RID: 942
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_put_IsPaused(IntPtr pNative, bool isPaused);

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0001AC4F File Offset: 0x00018E4F
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0001AC79 File Offset: 0x00018E79
		public bool IsPaused
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameReader");
				}
				return AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_get_IsPaused(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameReader");
				}
				AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_put_IsPaused(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001ACAC File Offset: 0x00018EAC
		[MonoPInvokeCallback(typeof(AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate))]
		private static void Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<AudioBeamFrameArrivedEventArgs>> list = null;
			AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<AudioBeamFrameArrivedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				AudioBeamFrameReader objThis = NativeObjectCache.GetObject<AudioBeamFrameReader>(pNative);
				AudioBeamFrameArrivedEventArgs args = new AudioBeamFrameArrivedEventArgs(result);
				using (List<EventHandler<AudioBeamFrameArrivedEventArgs>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventHandler<AudioBeamFrameArrivedEventArgs> func = enumerator.Current;
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

		// Token: 0x060003B2 RID: 946
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_add_FrameArrived(IntPtr pNative, AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060003B3 RID: 947 RVA: 0x0001AD70 File Offset: 0x00018F70
		// (remove) Token: 0x060003B4 RID: 948 RVA: 0x0001AE04 File Offset: 0x00019004
		public event EventHandler<AudioBeamFrameArrivedEventArgs> FrameArrived
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<AudioBeamFrameArrivedEventArgs>> list = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<AudioBeamFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate = new AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate(AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handler);
						AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate);
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_FrameArrived(this._pNative, windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<AudioBeamFrameArrivedEventArgs>> list = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<AudioBeamFrameArrivedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_FrameArrived(this._pNative, new AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate(AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handler), true);
						AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0001AEA0 File Offset: 0x000190A0
		[MonoPInvokeCallback(typeof(AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				AudioBeamFrameReader objThis = NativeObjectCache.GetObject<AudioBeamFrameReader>(pNative);
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

		// Token: 0x060003B6 RID: 950
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_add_PropertyChanged(IntPtr pNative, AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060003B7 RID: 951 RVA: 0x0001AF64 File Offset: 0x00019164
		// (remove) Token: 0x060003B8 RID: 952 RVA: 0x0001AFF8 File Offset: 0x000191F8
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_PropertyChanged(this._pNative, new AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x060003B9 RID: 953
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrameReader_AcquireLatestBeamFrames_Length(IntPtr pNative);

		// Token: 0x060003BA RID: 954
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrameReader_AcquireLatestBeamFrames(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x060003BB RID: 955 RVA: 0x0001B094 File Offset: 0x00019294
		public IList<AudioBeamFrame> AcquireLatestBeamFrames()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamFrameReader");
			}
			int num = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_AcquireLatestBeamFrames_Length(this._pNative);
			IntPtr[] array = new IntPtr[num];
			AudioBeamFrame[] array2 = new AudioBeamFrame[num];
			num = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_AcquireLatestBeamFrames(this._pNative, array, num);
			ExceptionHelper.CheckLastError();
			for (int i = 0; i < num; i++)
			{
				if (!(array[i] == IntPtr.Zero))
				{
					AudioBeamFrame audioBeamFrame = NativeObjectCache.CreateOrGetObject<AudioBeamFrame>(array[i], (IntPtr n) => new AudioBeamFrame(n));
					array2[i] = audioBeamFrame;
				}
			}
			return array2;
		}

		// Token: 0x060003BC RID: 956
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReader_Dispose(IntPtr pNative);

		// Token: 0x060003BD RID: 957 RVA: 0x0001B135 File Offset: 0x00019335
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001B158 File Offset: 0x00019358
		private void __EventCleanup()
		{
			AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<AudioBeamFrameArrivedEventArgs>> list = AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<AudioBeamFrameArrivedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_FrameArrived(this._pNative, new AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate(AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handler), true);
					}
					AudioBeamFrameReader._Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handle.Free();
				}
			}
			AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list2 = AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj2 = list2;
			lock (obj2)
			{
				if (list2.Count > 0)
				{
					list2.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						AudioBeamFrameReader.Windows_Kinect_AudioBeamFrameReader_add_PropertyChanged(this._pNative, new AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeamFrameReader.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					AudioBeamFrameReader._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400021F RID: 543
		internal IntPtr _pNative;

		// Token: 0x04000220 RID: 544
		private static GCHandle _Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_Handle;

		// Token: 0x04000221 RID: 545
		private static CollectionMap<IntPtr, List<EventHandler<AudioBeamFrameArrivedEventArgs>>> Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<AudioBeamFrameArrivedEventArgs>>>();

		// Token: 0x04000222 RID: 546
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x04000223 RID: 547
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0200023F RID: 575
		// (Invoke) Token: 0x0600113F RID: 4415
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Kinect_AudioBeamFrameArrivedEventArgs_Delegate(IntPtr args, IntPtr pNative);

		// Token: 0x02000240 RID: 576
		// (Invoke) Token: 0x06001143 RID: 4419
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
