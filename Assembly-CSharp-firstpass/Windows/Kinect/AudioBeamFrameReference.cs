using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200004D RID: 77
	public sealed class AudioBeamFrameReference : INativeWrapper
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0001B296 File Offset: 0x00019496
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0001B29E File Offset: 0x0001949E
		internal AudioBeamFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001B2B8 File Offset: 0x000194B8
		~AudioBeamFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x060003C3 RID: 963
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060003C4 RID: 964
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x060003C5 RID: 965 RVA: 0x0001B2E8 File Offset: 0x000194E8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrameReference>(this._pNative);
			AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060003C6 RID: 966
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_AudioBeamFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0001B324 File Offset: 0x00019524
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x060003C8 RID: 968
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrameReference_AcquireBeamFrames_Length(IntPtr pNative);

		// Token: 0x060003C9 RID: 969
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_AudioBeamFrameReference_AcquireBeamFrames(IntPtr pNative, [Out] IntPtr[] outCollection, int outCollectionSize);

		// Token: 0x060003CA RID: 970 RVA: 0x0001B354 File Offset: 0x00019554
		public IList<AudioBeamFrame> AcquireBeamFrames()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("AudioBeamFrameReference");
			}
			int num = AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_AcquireBeamFrames_Length(this._pNative);
			IntPtr[] array = new IntPtr[num];
			AudioBeamFrame[] array2 = new AudioBeamFrame[num];
			num = AudioBeamFrameReference.Windows_Kinect_AudioBeamFrameReference_AcquireBeamFrames(this._pNative, array, num);
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

		// Token: 0x060003CB RID: 971 RVA: 0x0001B3F5 File Offset: 0x000195F5
		private void __EventCleanup()
		{
		}

		// Token: 0x04000224 RID: 548
		internal IntPtr _pNative;
	}
}
