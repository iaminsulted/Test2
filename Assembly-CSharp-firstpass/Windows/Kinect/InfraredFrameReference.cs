using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000071 RID: 113
	public sealed class InfraredFrameReference : INativeWrapper
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00020F16 File Offset: 0x0001F116
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00020F1E File Offset: 0x0001F11E
		internal InfraredFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrameReference.Windows_Kinect_InfraredFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00020F38 File Offset: 0x0001F138
		~InfraredFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06000593 RID: 1427
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000594 RID: 1428
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000595 RID: 1429 RVA: 0x00020F68 File Offset: 0x0001F168
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrameReference>(this._pNative);
			InfraredFrameReference.Windows_Kinect_InfraredFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000596 RID: 1430
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_InfraredFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00020FA4 File Offset: 0x0001F1A4
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)InfraredFrameReference.Windows_Kinect_InfraredFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06000598 RID: 1432
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x06000599 RID: 1433 RVA: 0x00020FD4 File Offset: 0x0001F1D4
		public InfraredFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrameReference");
			}
			IntPtr intPtr = InfraredFrameReference.Windows_Kinect_InfraredFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<InfraredFrame>(intPtr, (IntPtr n) => new InfraredFrame(n));
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00021043 File Offset: 0x0001F243
		private void __EventCleanup()
		{
		}

		// Token: 0x04000297 RID: 663
		internal IntPtr _pNative;
	}
}
