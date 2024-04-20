using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200006B RID: 107
	public sealed class FrameDescription : INativeWrapper
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0002052E File Offset: 0x0001E72E
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00020536 File Offset: 0x0001E736
		internal FrameDescription(IntPtr pNative)
		{
			this._pNative = pNative;
			FrameDescription.Windows_Kinect_FrameDescription_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00020550 File Offset: 0x0001E750
		~FrameDescription()
		{
			this.Dispose(false);
		}

		// Token: 0x0600055B RID: 1371
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_FrameDescription_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600055C RID: 1372
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_FrameDescription_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600055D RID: 1373 RVA: 0x00020580 File Offset: 0x0001E780
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<FrameDescription>(this._pNative);
			FrameDescription.Windows_Kinect_FrameDescription_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600055E RID: 1374
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_FrameDescription_get_BytesPerPixel(IntPtr pNative);

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x000205BC File Offset: 0x0001E7BC
		public uint BytesPerPixel
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_BytesPerPixel(this._pNative);
			}
		}

		// Token: 0x06000560 RID: 1376
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_FrameDescription_get_DiagonalFieldOfView(IntPtr pNative);

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x000205E6 File Offset: 0x0001E7E6
		public float DiagonalFieldOfView
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_DiagonalFieldOfView(this._pNative);
			}
		}

		// Token: 0x06000562 RID: 1378
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_FrameDescription_get_Height(IntPtr pNative);

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00020610 File Offset: 0x0001E810
		public int Height
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_Height(this._pNative);
			}
		}

		// Token: 0x06000564 RID: 1380
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_FrameDescription_get_HorizontalFieldOfView(IntPtr pNative);

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0002063A File Offset: 0x0001E83A
		public float HorizontalFieldOfView
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_HorizontalFieldOfView(this._pNative);
			}
		}

		// Token: 0x06000566 RID: 1382
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern uint Windows_Kinect_FrameDescription_get_LengthInPixels(IntPtr pNative);

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x00020664 File Offset: 0x0001E864
		public uint LengthInPixels
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_LengthInPixels(this._pNative);
			}
		}

		// Token: 0x06000568 RID: 1384
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern float Windows_Kinect_FrameDescription_get_VerticalFieldOfView(IntPtr pNative);

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0002068E File Offset: 0x0001E88E
		public float VerticalFieldOfView
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_VerticalFieldOfView(this._pNative);
			}
		}

		// Token: 0x0600056A RID: 1386
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int Windows_Kinect_FrameDescription_get_Width(IntPtr pNative);

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x000206B8 File Offset: 0x0001E8B8
		public int Width
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameDescription");
				}
				return FrameDescription.Windows_Kinect_FrameDescription_get_Width(this._pNative);
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000206E2 File Offset: 0x0001E8E2
		private void __EventCleanup()
		{
		}

		// Token: 0x0400027B RID: 635
		internal IntPtr _pNative;
	}
}
