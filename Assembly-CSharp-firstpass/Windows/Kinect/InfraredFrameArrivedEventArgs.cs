using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200006F RID: 111
	public sealed class InfraredFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x000206E4 File Offset: 0x0001E8E4
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000206EC File Offset: 0x0001E8EC
		internal InfraredFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrameArrivedEventArgs.Windows_Kinect_InfraredFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00020708 File Offset: 0x0001E908
		~InfraredFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06000570 RID: 1392
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000571 RID: 1393
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000572 RID: 1394 RVA: 0x00020738 File Offset: 0x0001E938
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrameArrivedEventArgs>(this._pNative);
			InfraredFrameArrivedEventArgs.Windows_Kinect_InfraredFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000573 RID: 1395
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00020774 File Offset: 0x0001E974
		public InfraredFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrameArrivedEventArgs");
				}
				IntPtr intPtr = InfraredFrameArrivedEventArgs.Windows_Kinect_InfraredFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameReference>(intPtr, (IntPtr n) => new InfraredFrameReference(n));
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000207E3 File Offset: 0x0001E9E3
		private void __EventCleanup()
		{
		}

		// Token: 0x04000291 RID: 657
		internal IntPtr _pNative;
	}
}
