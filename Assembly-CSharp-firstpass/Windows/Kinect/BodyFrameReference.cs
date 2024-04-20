using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000053 RID: 83
	public sealed class BodyFrameReference : INativeWrapper
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0001C4F6 File Offset: 0x0001A6F6
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001C4FE File Offset: 0x0001A6FE
		internal BodyFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyFrameReference.Windows_Kinect_BodyFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001C518 File Offset: 0x0001A718
		~BodyFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x0600041C RID: 1052
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600041D RID: 1053
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600041E RID: 1054 RVA: 0x0001C548 File Offset: 0x0001A748
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyFrameReference>(this._pNative);
			BodyFrameReference.Windows_Kinect_BodyFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600041F RID: 1055
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_BodyFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0001C584 File Offset: 0x0001A784
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)BodyFrameReference.Windows_Kinect_BodyFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06000421 RID: 1057
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06000422 RID: 1058 RVA: 0x0001C5B4 File Offset: 0x0001A7B4
		public BodyFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyFrameReference");
			}
			IntPtr intPtr = BodyFrameReference.Windows_Kinect_BodyFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyFrame>(intPtr, (IntPtr n) => new BodyFrame(n));
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001C623 File Offset: 0x0001A823
		private void __EventCleanup()
		{
		}

		// Token: 0x04000234 RID: 564
		internal IntPtr _pNative;
	}
}
