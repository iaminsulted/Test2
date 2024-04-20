using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000062 RID: 98
	public sealed class DepthFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0001F27E File Offset: 0x0001D47E
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001F286 File Offset: 0x0001D486
		internal DepthFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrameArrivedEventArgs.Windows_Kinect_DepthFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001F2A0 File Offset: 0x0001D4A0
		~DepthFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x060004FB RID: 1275
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060004FC RID: 1276
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x060004FD RID: 1277 RVA: 0x0001F2D0 File Offset: 0x0001D4D0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrameArrivedEventArgs>(this._pNative);
			DepthFrameArrivedEventArgs.Windows_Kinect_DepthFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060004FE RID: 1278
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001F30C File Offset: 0x0001D50C
		public DepthFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrameArrivedEventArgs");
				}
				IntPtr intPtr = DepthFrameArrivedEventArgs.Windows_Kinect_DepthFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameReference>(intPtr, (IntPtr n) => new DepthFrameReference(n));
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001F37B File Offset: 0x0001D57B
		private void __EventCleanup()
		{
		}

		// Token: 0x04000260 RID: 608
		internal IntPtr _pNative;
	}
}
