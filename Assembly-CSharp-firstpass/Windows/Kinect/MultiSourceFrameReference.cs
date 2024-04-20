using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000081 RID: 129
	public sealed class MultiSourceFrameReference : INativeWrapper
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x0002363E File Offset: 0x0002183E
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00023646 File Offset: 0x00021846
		internal MultiSourceFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			MultiSourceFrameReference.Windows_Kinect_MultiSourceFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00023660 File Offset: 0x00021860
		~MultiSourceFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06000654 RID: 1620
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000655 RID: 1621
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000656 RID: 1622 RVA: 0x00023690 File Offset: 0x00021890
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<MultiSourceFrameReference>(this._pNative);
			MultiSourceFrameReference.Windows_Kinect_MultiSourceFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000657 RID: 1623
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06000658 RID: 1624 RVA: 0x000236CC File Offset: 0x000218CC
		public MultiSourceFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("MultiSourceFrameReference");
			}
			IntPtr intPtr = MultiSourceFrameReference.Windows_Kinect_MultiSourceFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<MultiSourceFrame>(intPtr, (IntPtr n) => new MultiSourceFrame(n));
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0002373B File Offset: 0x0002193B
		private void __EventCleanup()
		{
		}

		// Token: 0x040002DB RID: 731
		internal IntPtr _pNative;
	}
}
