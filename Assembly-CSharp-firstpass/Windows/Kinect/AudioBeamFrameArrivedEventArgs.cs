using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200004A RID: 74
	public sealed class AudioBeamFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0001A968 File Offset: 0x00018B68
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001A970 File Offset: 0x00018B70
		internal AudioBeamFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrameArrivedEventArgs.Windows_Kinect_AudioBeamFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001A98C File Offset: 0x00018B8C
		~AudioBeamFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06000396 RID: 918
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000397 RID: 919
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000398 RID: 920 RVA: 0x0001A9BC File Offset: 0x00018BBC
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrameArrivedEventArgs>(this._pNative);
			AudioBeamFrameArrivedEventArgs.Windows_Kinect_AudioBeamFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000399 RID: 921
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_AudioBeamFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0001A9F8 File Offset: 0x00018BF8
		public AudioBeamFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBeamFrameArrivedEventArgs");
				}
				IntPtr intPtr = AudioBeamFrameArrivedEventArgs.Windows_Kinect_AudioBeamFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<AudioBeamFrameReference>(intPtr, (IntPtr n) => new AudioBeamFrameReference(n));
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001AA67 File Offset: 0x00018C67
		private void __EventCleanup()
		{
		}

		// Token: 0x0400021D RID: 541
		internal IntPtr _pNative;
	}
}
