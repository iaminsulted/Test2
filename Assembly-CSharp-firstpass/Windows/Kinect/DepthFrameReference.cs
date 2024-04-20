using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000064 RID: 100
	public sealed class DepthFrameReference : INativeWrapper
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001FAAE File Offset: 0x0001DCAE
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001FAB6 File Offset: 0x0001DCB6
		internal DepthFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrameReference.Windows_Kinect_DepthFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001FAD0 File Offset: 0x0001DCD0
		~DepthFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x0600051E RID: 1310
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600051F RID: 1311
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000520 RID: 1312 RVA: 0x0001FB00 File Offset: 0x0001DD00
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrameReference>(this._pNative);
			DepthFrameReference.Windows_Kinect_DepthFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000521 RID: 1313
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_DepthFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x0001FB3C File Offset: 0x0001DD3C
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)DepthFrameReference.Windows_Kinect_DepthFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06000523 RID: 1315
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06000524 RID: 1316 RVA: 0x0001FB6C File Offset: 0x0001DD6C
		public DepthFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrameReference");
			}
			IntPtr intPtr = DepthFrameReference.Windows_Kinect_DepthFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<DepthFrame>(intPtr, (IntPtr n) => new DepthFrame(n));
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001FBDB File Offset: 0x0001DDDB
		private void __EventCleanup()
		{
		}

		// Token: 0x04000266 RID: 614
		internal IntPtr _pNative;
	}
}
