using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000051 RID: 81
	public sealed class BodyFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0001BCC6 File Offset: 0x00019EC6
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001BCCE File Offset: 0x00019ECE
		internal BodyFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrameArrivedEventArgs.Windows_Kinect_BodyFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001BCE8 File Offset: 0x00019EE8
		~BodyFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x060003F9 RID: 1017
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060003FA RID: 1018
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x060003FB RID: 1019 RVA: 0x0001BD18 File Offset: 0x00019F18
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrameArrivedEventArgs>(this._pNative);
			BodyFrameArrivedEventArgs.Windows_Kinect_BodyFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060003FC RID: 1020
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0001BD54 File Offset: 0x00019F54
		public BodyFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameArrivedEventArgs");
				}
				IntPtr intPtr = BodyFrameArrivedEventArgs.Windows_Kinect_BodyFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameReference>(intPtr, (IntPtr n) => new BodyFrameReference(n));
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001BDC3 File Offset: 0x00019FC3
		private void __EventCleanup()
		{
		}

		// Token: 0x0400022E RID: 558
		internal IntPtr _pNative;
	}
}
