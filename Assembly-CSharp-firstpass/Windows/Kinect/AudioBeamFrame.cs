using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200003D RID: 61
	public sealed class AudioBeamFrame : IDisposable, INativeWrapper
	{
		// Token: 0x0600024A RID: 586 RVA: 0x000174E0 File Offset: 0x000156E0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			if (this._subFrames != null)
			{
				AudioBeamSubFrame[] subFrames = this._subFrames;
				for (int i = 0; i < subFrames.Length; i++)
				{
					subFrames[i].Dispose();
				}
				this._subFrames = null;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrame>(this._pNative);
			AudioBeamFrame.Windows_Kinect_AudioBeamFrame_ReleaseObject(ref this._pNative);
			if (disposing)
			{
				AudioBeamFrame.Windows_Kinect_AudioBeamFrame_Dispose(this._pNative);
			}
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600024B RID: 587
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Windows_Kinect_AudioBeamFrame_Dispose(IntPtr pNative);

		// Token: 0x0600024C RID: 588 RVA: 0x00017561 File Offset: 0x00015761
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600024D RID: 589
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrame_get_SubFrames(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x0600024E RID: 590
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl)]
		private static extern int Windows_Kinect_AudioBeamFrame_get_SubFrames_Length(IntPtr pNative);

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00017584 File Offset: 0x00015784
		public IList<AudioBeamSubFrame> SubFrames
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				if (this._subFrames == null)
				{
					int num = AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_SubFrames_Length(this._pNative);
					IntPtr[] array = new IntPtr[num];
					this._subFrames = new AudioBeamSubFrame[num];
					num = AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_SubFrames(this._pNative, array, num);
					ExceptionHelper.CheckLastError();
					for (int i = 0; i < num; i++)
					{
						if (!(array[i] == IntPtr.Zero))
						{
							AudioBeamSubFrame audioBeamSubFrame = NativeObjectCache.GetObject<AudioBeamSubFrame>(array[i]);
							if (audioBeamSubFrame == null)
							{
								audioBeamSubFrame = new AudioBeamSubFrame(array[i]);
								NativeObjectCache.AddObject<AudioBeamSubFrame>(array[i], audioBeamSubFrame);
							}
							this._subFrames[i] = audioBeamSubFrame;
						}
					}
				}
				return this._subFrames;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00017630 File Offset: 0x00015830
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00017638 File Offset: 0x00015838
		internal AudioBeamFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrame.Windows_Kinect_AudioBeamFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00017654 File Offset: 0x00015854
		~AudioBeamFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x06000253 RID: 595
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000254 RID: 596
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000255 RID: 597
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamFrame_get_AudioBeam(IntPtr pNative);

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00017684 File Offset: 0x00015884
		public AudioBeam AudioBeam
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				IntPtr intPtr = AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_AudioBeam(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioBeam>(intPtr, (IntPtr n) => new AudioBeam(n));
			}
		}

		// Token: 0x06000257 RID: 599
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamFrame_get_AudioSource(IntPtr pNative);

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000258 RID: 600 RVA: 0x000176F4 File Offset: 0x000158F4
		public AudioSource AudioSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				IntPtr intPtr = AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_AudioSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioSource>(intPtr, (IntPtr n) => new AudioSource(n));
			}
		}

		// Token: 0x06000259 RID: 601
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamFrame_get_Duration(IntPtr pNative);

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00017763 File Offset: 0x00015963
		public TimeSpan Duration
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_Duration(this._pNative));
			}
		}

		// Token: 0x0600025B RID: 603
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamFrame_get_RelativeTimeStart(IntPtr pNative);

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00017793 File Offset: 0x00015993
		public TimeSpan RelativeTimeStart
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrame");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamFrame.Windows_Kinect_AudioBeamFrame_get_RelativeTimeStart(this._pNative));
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000177C3 File Offset: 0x000159C3
		private void __EventCleanup()
		{
		}

		// Token: 0x04000200 RID: 512
		private AudioBeamSubFrame[] _subFrames;

		// Token: 0x04000201 RID: 513
		internal IntPtr _pNative;
	}
}
