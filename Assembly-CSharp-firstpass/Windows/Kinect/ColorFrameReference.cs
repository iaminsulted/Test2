using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200005D RID: 93
	public sealed class ColorFrameReference : INativeWrapper
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0001E866 File Offset: 0x0001CA66
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001E86E File Offset: 0x0001CA6E
		internal ColorFrameReference(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrameReference.Windows_Kinect_ColorFrameReference_AddRefObject(ref this._pNative);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001E888 File Offset: 0x0001CA88
		~ColorFrameReference()
		{
			this.Dispose(false);
		}

		// Token: 0x060004C6 RID: 1222
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReference_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060004C7 RID: 1223
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameReference_AddRefObject(ref IntPtr pNative);

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001E8B8 File Offset: 0x0001CAB8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrameReference>(this._pNative);
			ColorFrameReference.Windows_Kinect_ColorFrameReference_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060004C9 RID: 1225
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_ColorFrameReference_get_RelativeTime(IntPtr pNative);

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0001E8F4 File Offset: 0x0001CAF4
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameReference");
				}
				return TimeSpan.FromMilliseconds((double)ColorFrameReference.Windows_Kinect_ColorFrameReference_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x060004CB RID: 1227
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameReference_AcquireFrame(IntPtr pNative);

		// Token: 0x060004CC RID: 1228 RVA: 0x0001E924 File Offset: 0x0001CB24
		public ColorFrame AcquireFrame()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrameReference");
			}
			IntPtr intPtr = ColorFrameReference.Windows_Kinect_ColorFrameReference_AcquireFrame(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<ColorFrame>(intPtr, (IntPtr n) => new ColorFrame(n));
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001E993 File Offset: 0x0001CB93
		private void __EventCleanup()
		{
		}

		// Token: 0x04000250 RID: 592
		internal IntPtr _pNative;
	}
}
