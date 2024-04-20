using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200005A RID: 90
	public sealed class ColorCameraSettings : INativeWrapper
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0001DEF1 File Offset: 0x0001C0F1
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001DEF9 File Offset: 0x0001C0F9
		internal ColorCameraSettings(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorCameraSettings.Windows_Kinect_ColorCameraSettings_AddRefObject(ref this._pNative);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001DF14 File Offset: 0x0001C114
		~ColorCameraSettings()
		{
			this.Dispose(false);
		}

		// Token: 0x06000494 RID: 1172
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorCameraSettings_ReleaseObject(ref IntPtr pNative);

		// Token: 0x06000495 RID: 1173
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorCameraSettings_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000496 RID: 1174 RVA: 0x0001DF44 File Offset: 0x0001C144
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorCameraSettings>(this._pNative);
			ColorCameraSettings.Windows_Kinect_ColorCameraSettings_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000497 RID: 1175
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_ColorCameraSettings_get_ExposureTime(IntPtr pNative);

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001DF80 File Offset: 0x0001C180
		public TimeSpan ExposureTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorCameraSettings");
				}
				return TimeSpan.FromMilliseconds((double)ColorCameraSettings.Windows_Kinect_ColorCameraSettings_get_ExposureTime(this._pNative));
			}
		}

		// Token: 0x06000499 RID: 1177
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_ColorCameraSettings_get_FrameInterval(IntPtr pNative);

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001DFB0 File Offset: 0x0001C1B0
		public TimeSpan FrameInterval
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorCameraSettings");
				}
				return TimeSpan.FromMilliseconds((double)ColorCameraSettings.Windows_Kinect_ColorCameraSettings_get_FrameInterval(this._pNative));
			}
		}

		// Token: 0x0600049B RID: 1179
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_ColorCameraSettings_get_Gain(IntPtr pNative);

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x0001DFE0 File Offset: 0x0001C1E0
		public float Gain
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorCameraSettings");
				}
				return ColorCameraSettings.Windows_Kinect_ColorCameraSettings_get_Gain(this._pNative);
			}
		}

		// Token: 0x0600049D RID: 1181
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_ColorCameraSettings_get_Gamma(IntPtr pNative);

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x0001E00A File Offset: 0x0001C20A
		public float Gamma
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorCameraSettings");
				}
				return ColorCameraSettings.Windows_Kinect_ColorCameraSettings_get_Gamma(this._pNative);
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001E034 File Offset: 0x0001C234
		private void __EventCleanup()
		{
		}

		// Token: 0x04000249 RID: 585
		internal IntPtr _pNative;
	}
}
