using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000057 RID: 87
	public sealed class BodyIndexFrameReference : INativeWrapper
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0001D59E File Offset: 0x0001B79E
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001D5A6 File Offset: 0x0001B7A6
		internal BodyIndexFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrameReference.Windows_Kinect_BodyIndexFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001D5C0 File Offset: 0x0001B7C0
		~BodyIndexFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06000466 RID: 1126
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000467 RID: 1127
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000468 RID: 1128 RVA: 0x0001D5F0 File Offset: 0x0001B7F0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrameReference>(this._pNative);
			BodyIndexFrameReference.Windows_Kinect_BodyIndexFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000469 RID: 1129
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_BodyIndexFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0001D62C File Offset: 0x0001B82C
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)BodyIndexFrameReference.Windows_Kinect_BodyIndexFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x0600046B RID: 1131
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x0600046C RID: 1132 RVA: 0x0001D65C File Offset: 0x0001B85C
		public BodyIndexFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrameReference");
			}
			IntPtr intPtr = BodyIndexFrameReference.Windows_Kinect_BodyIndexFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<BodyIndexFrame>(intPtr, (IntPtr n) => new BodyIndexFrame(n));
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001D6CB File Offset: 0x0001B8CB
		private void __EventCleanup()
		{
		}

		// Token: 0x04000240 RID: 576
		internal IntPtr _pNative;
	}
}
