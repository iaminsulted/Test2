using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200005B RID: 91
	public sealed class ColorFrameArrivedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0001E036 File Offset: 0x0001C236
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001E03E File Offset: 0x0001C23E
		internal ColorFrameArrivedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrameArrivedEventArgs.Windows_Kinect_ColorFrameArrivedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001E058 File Offset: 0x0001C258
		~ColorFrameArrivedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x060004A3 RID: 1187
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameArrivedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060004A4 RID: 1188
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrameArrivedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001E088 File Offset: 0x0001C288
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrameArrivedEventArgs>(this._pNative);
			ColorFrameArrivedEventArgs.Windows_Kinect_ColorFrameArrivedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060004A6 RID: 1190
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrameArrivedEventArgs_get_FrameReference(IntPtr pNative);

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0001E0C4 File Offset: 0x0001C2C4
		public ColorFrameReference FrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrameArrivedEventArgs");
				}
				IntPtr intPtr = ColorFrameArrivedEventArgs.Windows_Kinect_ColorFrameArrivedEventArgs_get_FrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameReference>(intPtr, (IntPtr n) => new ColorFrameReference(n));
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001E133 File Offset: 0x0001C333
		private void __EventCleanup()
		{
		}

		// Token: 0x0400024A RID: 586
		internal IntPtr _pNative;
	}
}
