using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using Helper;
using Windows.Data;

namespace Windows.Kinect
{
	// Token: 0x02000049 RID: 73
	public sealed class AudioBeam : INativeWrapper
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0001A4BC File Offset: 0x000186BC
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001A4C4 File Offset: 0x000186C4
		internal AudioBeam(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeam.Windows_Kinect_AudioBeam_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001A4E0 File Offset: 0x000186E0
		~AudioBeam()
		{
			this.Dispose(false);
		}

		// Token: 0x0600037C RID: 892
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600037D RID: 893
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600037E RID: 894 RVA: 0x0001A510 File Offset: 0x00018710
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeam>(this._pNative);
			AudioBeam.Windows_Kinect_AudioBeam_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600037F RID: 895
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern AudioBeamMode Windows_Kinect_AudioBeam_get_AudioBeamMode(IntPtr pNative);

		// Token: 0x06000380 RID: 896
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_put_AudioBeamMode(IntPtr pNative, AudioBeamMode audioBeamMode);

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0001A54C File Offset: 0x0001874C
		// (set) Token: 0x06000382 RID: 898 RVA: 0x0001A576 File Offset: 0x00018776
		public AudioBeamMode AudioBeamMode
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				return AudioBeam.Windows_Kinect_AudioBeam_get_AudioBeamMode(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				AudioBeam.Windows_Kinect_AudioBeam_put_AudioBeamMode(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06000383 RID: 899
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeam_get_AudioSource(IntPtr pNative);

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0001A5A8 File Offset: 0x000187A8
		public AudioSource AudioSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				IntPtr intPtr = AudioBeam.Windows_Kinect_AudioBeam_get_AudioSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioSource>(intPtr, (IntPtr n) => new AudioSource(n));
			}
		}

		// Token: 0x06000385 RID: 901
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_AudioBeam_get_BeamAngle(IntPtr pNative);

		// Token: 0x06000386 RID: 902
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_put_BeamAngle(IntPtr pNative, float beamAngle);

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0001A617 File Offset: 0x00018817
		// (set) Token: 0x06000388 RID: 904 RVA: 0x0001A641 File Offset: 0x00018841
		public float BeamAngle
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				return AudioBeam.Windows_Kinect_AudioBeam_get_BeamAngle(this._pNative);
			}
			set
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				AudioBeam.Windows_Kinect_AudioBeam_put_BeamAngle(this._pNative, value);
				ExceptionHelper.CheckLastError();
			}
		}

		// Token: 0x06000389 RID: 905
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_AudioBeam_get_BeamAngleConfidence(IntPtr pNative);

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0001A671 File Offset: 0x00018871
		public float BeamAngleConfidence
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				return AudioBeam.Windows_Kinect_AudioBeam_get_BeamAngleConfidence(this._pNative);
			}
		}

		// Token: 0x0600038B RID: 907
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeam_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0001A69B File Offset: 0x0001889B
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeam");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeam.Windows_Kinect_AudioBeam_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001A6CC File Offset: 0x000188CC
		[MonoPInvokeCallback(typeof(AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate))]
		private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(IntPtr result, IntPtr pNative)
		{
			List<EventHandler<PropertyChangedEventArgs>> list = null;
			AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out list);
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				AudioBeam objThis = NativeObjectCache.GetObject<AudioBeam>(pNative);
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

		// Token: 0x0600038E RID: 910
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeam_add_PropertyChanged(IntPtr pNative, AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600038F RID: 911 RVA: 0x0001A790 File Offset: 0x00018990
		// (remove) Token: 0x06000390 RID: 912 RVA: 0x0001A824 File Offset: 0x00018A24
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged
		{
			add
			{
				EventPump.EnsureInitialized();
				AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Add(value);
					if (list.Count == 1)
					{
						AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate windows_Data_PropertyChangedEventArgs_Delegate = new AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
						AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate_Handle = GCHandle.Alloc(windows_Data_PropertyChangedEventArgs_Delegate);
						AudioBeam.Windows_Kinect_AudioBeam_add_PropertyChanged(this._pNative, windows_Data_PropertyChangedEventArgs_Delegate, false);
					}
				}
			}
			remove
			{
				if (this._pNative == IntPtr.Zero)
				{
					return;
				}
				AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
				List<EventHandler<PropertyChangedEventArgs>> list = AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
				List<EventHandler<PropertyChangedEventArgs>> obj = list;
				lock (obj)
				{
					list.Remove(value);
					if (list.Count == 0)
					{
						AudioBeam.Windows_Kinect_AudioBeam_add_PropertyChanged(this._pNative, new AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
						AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
					}
				}
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001A8C0 File Offset: 0x00018AC0
		private void __EventCleanup()
		{
			AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(this._pNative);
			List<EventHandler<PropertyChangedEventArgs>> list = AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[this._pNative];
			List<EventHandler<PropertyChangedEventArgs>> obj = list;
			lock (obj)
			{
				if (list.Count > 0)
				{
					list.Clear();
					if (this._pNative != IntPtr.Zero)
					{
						AudioBeam.Windows_Kinect_AudioBeam_add_PropertyChanged(this._pNative, new AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate(AudioBeam.Windows_Data_PropertyChangedEventArgs_Delegate_Handler), true);
					}
					AudioBeam._Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
				}
			}
		}

		// Token: 0x0400021A RID: 538
		internal IntPtr _pNative;

		// Token: 0x0400021B RID: 539
		private static GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;

		// Token: 0x0400021C RID: 540
		private static CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new CollectionMap<IntPtr, List<EventHandler<PropertyChangedEventArgs>>>();

		// Token: 0x0200023A RID: 570
		// (Invoke) Token: 0x06001132 RID: 4402
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(IntPtr args, IntPtr pNative);
	}
}
