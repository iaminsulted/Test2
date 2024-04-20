using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200007A RID: 122
	public sealed class LongExposureInfraredFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00021A29 File Offset: 0x0001FC29
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00021A31 File Offset: 0x0001FC31
		internal LongExposureInfraredFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrameArrivedEventArgs.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00021A4C File Offset: 0x0001FC4C
		~LongExposureInfraredFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x060005D6 RID: 1494
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060005D7 RID: 1495
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x060005D8 RID: 1496 RVA: 0x00021A7C File Offset: 0x0001FC7C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrameArrivedEventArgs>(this._pNative);
			LongExposureInfraredFrameArrivedEventArgs.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060005D9 RID: 1497
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x00021AB8 File Offset: 0x0001FCB8
		public LongExposureInfraredFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameArrivedEventArgs");
				}
				IntPtr intPtr = LongExposureInfraredFrameArrivedEventArgs.Windows_Kinect_LongExposureInfraredFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameReference>(intPtr, (IntPtr n) => new LongExposureInfraredFrameReference(n));
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00021B27 File Offset: 0x0001FD27
		private void __EventCleanup()
		{
		}

		// Token: 0x040002C8 RID: 712
		internal IntPtr _pNative;
	}
}
