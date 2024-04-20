using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200007F RID: 127
	public sealed class MultiSourceFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00022DE5 File Offset: 0x00020FE5
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00022DED File Offset: 0x00020FED
		internal MultiSourceFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			MultiSourceFrameArrivedEventArgs.Windows_Kinect_MultiSourceFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00022E08 File Offset: 0x00021008
		~MultiSourceFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x0600062F RID: 1583
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000630 RID: 1584
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000631 RID: 1585 RVA: 0x00022E38 File Offset: 0x00021038
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<MultiSourceFrameArrivedEventArgs>(this._pNative);
			MultiSourceFrameArrivedEventArgs.Windows_Kinect_MultiSourceFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000632 RID: 1586
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00022E74 File Offset: 0x00021074
		public MultiSourceFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrameArrivedEventArgs");
				}
				IntPtr intPtr = MultiSourceFrameArrivedEventArgs.Windows_Kinect_MultiSourceFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<MultiSourceFrameReference>(intPtr, (IntPtr n) => new MultiSourceFrameReference(n));
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00022EE3 File Offset: 0x000210E3
		private void __EventCleanup()
		{
		}

		// Token: 0x040002D5 RID: 725
		internal IntPtr _pNative;
	}
}
