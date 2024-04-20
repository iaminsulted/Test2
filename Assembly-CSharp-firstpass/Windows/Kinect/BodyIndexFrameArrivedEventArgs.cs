using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000055 RID: 85
	public sealed class BodyIndexFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0001CD6E File Offset: 0x0001AF6E
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001CD76 File Offset: 0x0001AF76
		internal BodyIndexFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrameArrivedEventArgs.Windows_Kinect_BodyIndexFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0001CD90 File Offset: 0x0001AF90
		~BodyIndexFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06000443 RID: 1091
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000444 RID: 1092
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000445 RID: 1093 RVA: 0x0001CDC0 File Offset: 0x0001AFC0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrameArrivedEventArgs>(this._pNative);
			BodyIndexFrameArrivedEventArgs.Windows_Kinect_BodyIndexFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000446 RID: 1094
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x0001CDFC File Offset: 0x0001AFFC
		public BodyIndexFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameArrivedEventArgs");
				}
				IntPtr intPtr = BodyIndexFrameArrivedEventArgs.Windows_Kinect_BodyIndexFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameReference>(intPtr, (IntPtr n) => new BodyIndexFrameReference(n));
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001CE6B File Offset: 0x0001B06B
		private void __EventCleanup()
		{
		}

		// Token: 0x0400023A RID: 570
		internal IntPtr _pNative;
	}
}
