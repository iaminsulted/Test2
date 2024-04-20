using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200007C RID: 124
	public sealed class LongExposureInfraredFrameReference : INativeWrapper
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x00022256 File Offset: 0x00020456
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0002225E File Offset: 0x0002045E
		internal LongExposureInfraredFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrameReference.Windows_Kinect_LongExposureInfraredFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00022278 File Offset: 0x00020478
		~LongExposureInfraredFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x060005F9 RID: 1529
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060005FA RID: 1530
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x060005FB RID: 1531 RVA: 0x000222A8 File Offset: 0x000204A8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrameReference>(this._pNative);
			LongExposureInfraredFrameReference.Windows_Kinect_LongExposureInfraredFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060005FC RID: 1532
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_LongExposureInfraredFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x000222E4 File Offset: 0x000204E4
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)LongExposureInfraredFrameReference.Windows_Kinect_LongExposureInfraredFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x060005FE RID: 1534
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x060005FF RID: 1535 RVA: 0x00022314 File Offset: 0x00020514
		public LongExposureInfraredFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrameReference");
			}
			IntPtr intPtr = LongExposureInfraredFrameReference.Windows_Kinect_LongExposureInfraredFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrame>(intPtr, (IntPtr n) => new LongExposureInfraredFrame(n));
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00022383 File Offset: 0x00020583
		private void __EventCleanup()
		{
		}

		// Token: 0x040002CE RID: 718
		internal IntPtr _pNative;
	}
}
