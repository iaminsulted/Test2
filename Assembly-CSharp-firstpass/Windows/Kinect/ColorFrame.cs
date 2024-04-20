using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000040 RID: 64
	public sealed class ColorFrame : IDisposable, INativeWrapper
	{
		// Token: 0x060002A2 RID: 674
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_ColorFrame_CopyRawFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_CopyRawFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x060002A3 RID: 675 RVA: 0x00017F8E File Offset: 0x0001618E
		public void CopyRawFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			ColorFrame.Windows_Kinect_ColorFrame_CopyRawFrameDataToIntPtr(this._pNative, frameData, size);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060002A4 RID: 676
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_ColorFrame_CopyConvertedFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_CopyConvertedFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize, ColorImageFormat colorFormat);

		// Token: 0x060002A5 RID: 677 RVA: 0x00017FBF File Offset: 0x000161BF
		public void CopyConvertedFrameDataToIntPtr(IntPtr frameData, uint size, ColorImageFormat colorFormat)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			ColorFrame.Windows_Kinect_ColorFrame_CopyConvertedFrameDataToIntPtr(this._pNative, frameData, size, colorFormat);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060002A6 RID: 678
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_LockRawImageBuffer(IntPtr pNative);

		// Token: 0x060002A7 RID: 679 RVA: 0x00017FF4 File Offset: 0x000161F4
		public KinectBuffer LockRawImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_LockRawImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x00018063 File Offset: 0x00016263
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0001806B File Offset: 0x0001626B
		internal ColorFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			ColorFrame.Windows_Kinect_ColorFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00018088 File Offset: 0x00016288
		~ColorFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x060002AB RID: 683
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060002AC RID: 684
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x060002AD RID: 685 RVA: 0x000180B8 File Offset: 0x000162B8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<ColorFrame>(this._pNative);
			if (disposing)
			{
				ColorFrame.Windows_Kinect_ColorFrame_Dispose(this._pNative);
			}
			ColorFrame.Windows_Kinect_ColorFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060002AE RID: 686
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_get_ColorCameraSettings(IntPtr pNative);

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00018110 File Offset: 0x00016310
		public ColorCameraSettings ColorCameraSettings
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_get_ColorCameraSettings(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorCameraSettings>(intPtr, (IntPtr n) => new ColorCameraSettings(n));
			}
		}

		// Token: 0x060002B0 RID: 688
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_get_ColorFrameSource(IntPtr pNative);

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00018180 File Offset: 0x00016380
		public ColorFrameSource ColorFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_get_ColorFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameSource>(intPtr, (IntPtr n) => new ColorFrameSource(n));
			}
		}

		// Token: 0x060002B2 RID: 690
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x000181F0 File Offset: 0x000163F0
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x060002B4 RID: 692
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ColorImageFormat Windows_Kinect_ColorFrame_get_RawColorImageFormat(IntPtr pNative);

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0001825F File Offset: 0x0001645F
		public ColorImageFormat RawColorImageFormat
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				return ColorFrame.Windows_Kinect_ColorFrame_get_RawColorImageFormat(this._pNative);
			}
		}

		// Token: 0x060002B6 RID: 694
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_ColorFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00018289 File Offset: 0x00016489
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("ColorFrame");
				}
				return TimeSpan.FromMilliseconds((double)ColorFrame.Windows_Kinect_ColorFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x060002B8 RID: 696
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_CopyRawFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x060002B9 RID: 697 RVA: 0x000182BC File Offset: 0x000164BC
		public void CopyRawFrameDataToArray(byte[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			IntPtr frameData2 = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			ColorFrame.Windows_Kinect_ColorFrame_CopyRawFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060002BA RID: 698
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_CopyConvertedFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize, ColorImageFormat colorFormat);

		// Token: 0x060002BB RID: 699 RVA: 0x0001830C File Offset: 0x0001650C
		public void CopyConvertedFrameDataToArray(byte[] frameData, ColorImageFormat colorFormat)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			IntPtr frameData2 = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			ColorFrame.Windows_Kinect_ColorFrame_CopyConvertedFrameDataToArray(this._pNative, frameData2, frameData.Length, colorFormat);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060002BC RID: 700
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_ColorFrame_CreateFrameDescription(IntPtr pNative, ColorImageFormat format);

		// Token: 0x060002BD RID: 701 RVA: 0x00018360 File Offset: 0x00016560
		public FrameDescription CreateFrameDescription(ColorImageFormat format)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ColorFrame");
			}
			IntPtr intPtr = ColorFrame.Windows_Kinect_ColorFrame_CreateFrameDescription(this._pNative, format);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
		}

		// Token: 0x060002BE RID: 702
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_ColorFrame_Dispose(IntPtr pNative);

		// Token: 0x060002BF RID: 703 RVA: 0x000183D0 File Offset: 0x000165D0
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000183F2 File Offset: 0x000165F2
		private void __EventCleanup()
		{
		}

		// Token: 0x04000204 RID: 516
		internal IntPtr _pNative;
	}
}
