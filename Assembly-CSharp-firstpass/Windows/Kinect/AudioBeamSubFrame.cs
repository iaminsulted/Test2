using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200003C RID: 60
	public sealed class AudioBeamSubFrame : IDisposable, INativeWrapper
	{
		// Token: 0x0600022C RID: 556
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x0600022D RID: 557 RVA: 0x00017175 File Offset: 0x00015375
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamSubFrame");
			}
			AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x0600022E RID: 558
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamSubFrame_LockAudioBuffer(IntPtr pNative);

		// Token: 0x0600022F RID: 559 RVA: 0x000171A8 File Offset: 0x000153A8
		public KinectBuffer LockAudioBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamSubFrame");
			}
			IntPtr intPtr = AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_LockAudioBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00017217 File Offset: 0x00015417
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0001721F File Offset: 0x0001541F
		internal AudioBeamSubFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0001723C File Offset: 0x0001543C
		~AudioBeamSubFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x06000233 RID: 563
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000234 RID: 564
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000235 RID: 565 RVA: 0x0001726C File Offset: 0x0001546C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamSubFrame>(this._pNative);
			if (disposing)
			{
				AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_Dispose(this._pNative);
			}
			AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000236 RID: 566
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern AudioBeamMode Windows_Kinect_AudioBeamSubFrame_get_AudioBeamMode(IntPtr pNative);

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000237 RID: 567 RVA: 0x000172C1 File Offset: 0x000154C1
		public AudioBeamMode AudioBeamMode
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_AudioBeamMode(this._pNative);
			}
		}

		// Token: 0x06000238 RID: 568
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamSubFrame_get_AudioBodyCorrelations(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x06000239 RID: 569
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamSubFrame_get_AudioBodyCorrelations_Length(IntPtr pNative);

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600023A RID: 570 RVA: 0x000172EC File Offset: 0x000154EC
		public IList<AudioBodyCorrelation> AudioBodyCorrelations
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				int num = AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_AudioBodyCorrelations_Length(this._pNative);
				IntPtr[] array = new IntPtr[num];
				AudioBodyCorrelation[] array2 = new AudioBodyCorrelation[num];
				num = AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_AudioBodyCorrelations(this._pNative, array, num);
				ExceptionHelper.CheckLastError();
				for (int i = 0; i < num; i++)
				{
					if (!(array[i] == IntPtr.Zero))
					{
						AudioBodyCorrelation audioBodyCorrelation = NativeObjectCache.CreateOrGetObject<AudioBodyCorrelation>(array[i], (IntPtr n) => new AudioBodyCorrelation(n));
						array2[i] = audioBodyCorrelation;
					}
				}
				return array2;
			}
		}

		// Token: 0x0600023B RID: 571
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_AudioBeamSubFrame_get_BeamAngle(IntPtr pNative);

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0001738D File Offset: 0x0001558D
		public float BeamAngle
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_BeamAngle(this._pNative);
			}
		}

		// Token: 0x0600023D RID: 573
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_AudioBeamSubFrame_get_BeamAngleConfidence(IntPtr pNative);

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600023E RID: 574 RVA: 0x000173B7 File Offset: 0x000155B7
		public float BeamAngleConfidence
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_BeamAngleConfidence(this._pNative);
			}
		}

		// Token: 0x0600023F RID: 575
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamSubFrame_get_Duration(IntPtr pNative);

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000240 RID: 576 RVA: 0x000173E1 File Offset: 0x000155E1
		public TimeSpan Duration
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_Duration(this._pNative));
			}
		}

		// Token: 0x06000241 RID: 577
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_AudioBeamSubFrame_get_FrameLengthInBytes(IntPtr pNative);

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00017411 File Offset: 0x00015611
		public uint FrameLengthInBytes
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_FrameLengthInBytes(this._pNative);
			}
		}

		// Token: 0x06000243 RID: 579
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamSubFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0001743B File Offset: 0x0001563B
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamSubFrame");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06000245 RID: 581
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x06000246 RID: 582 RVA: 0x0001746C File Offset: 0x0001566C
		public void CopyFrameDataToArray(byte[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamSubFrame");
			}
			IntPtr frameData2 = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			AudioBeamSubFrame.Windows_Kinect_AudioBeamSubFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000247 RID: 583
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamSubFrame_Dispose(IntPtr pNative);

		// Token: 0x06000248 RID: 584 RVA: 0x000174BC File Offset: 0x000156BC
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000174DE File Offset: 0x000156DE
		private void __EventCleanup()
		{
		}

		// Token: 0x040001FF RID: 511
		internal IntPtr _pNative;
	}
}
