using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200007E RID: 126
	public sealed class MultiSourceFrame : INativeWrapper
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00022AB6 File Offset: 0x00020CB6
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00022ABE File Offset: 0x00020CBE
		internal MultiSourceFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			MultiSourceFrame.Windows_Kinect_MultiSourceFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00022AD8 File Offset: 0x00020CD8
		~MultiSourceFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x0600061C RID: 1564
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600061D RID: 1565
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_MultiSourceFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600061E RID: 1566 RVA: 0x00022B08 File Offset: 0x00020D08
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<MultiSourceFrame>(this._pNative);
			MultiSourceFrame.Windows_Kinect_MultiSourceFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600061F RID: 1567
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_BodyFrameReference(IntPtr pNative);

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00022B44 File Offset: 0x00020D44
		public BodyFrameReference BodyFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_BodyFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyFrameReference>(intPtr, (IntPtr n) => new BodyFrameReference(n));
			}
		}

		// Token: 0x06000621 RID: 1569
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_BodyIndexFrameReference(IntPtr pNative);

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x00022BB4 File Offset: 0x00020DB4
		public BodyIndexFrameReference BodyIndexFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_BodyIndexFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameReference>(intPtr, (IntPtr n) => new BodyIndexFrameReference(n));
			}
		}

		// Token: 0x06000623 RID: 1571
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_ColorFrameReference(IntPtr pNative);

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x00022C24 File Offset: 0x00020E24
		public ColorFrameReference ColorFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_ColorFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<ColorFrameReference>(intPtr, (IntPtr n) => new ColorFrameReference(n));
			}
		}

		// Token: 0x06000625 RID: 1573
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_DepthFrameReference(IntPtr pNative);

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00022C94 File Offset: 0x00020E94
		public DepthFrameReference DepthFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_DepthFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameReference>(intPtr, (IntPtr n) => new DepthFrameReference(n));
			}
		}

		// Token: 0x06000627 RID: 1575
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_InfraredFrameReference(IntPtr pNative);

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00022D04 File Offset: 0x00020F04
		public InfraredFrameReference InfraredFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_InfraredFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameReference>(intPtr, (IntPtr n) => new InfraredFrameReference(n));
			}
		}

		// Token: 0x06000629 RID: 1577
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_MultiSourceFrame_get_LongExposureInfraredFrameReference(IntPtr pNative);

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00022D74 File Offset: 0x00020F74
		public LongExposureInfraredFrameReference LongExposureInfraredFrameReference
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("MultiSourceFrame");
				}
				IntPtr intPtr = MultiSourceFrame.Windows_Kinect_MultiSourceFrame_get_LongExposureInfraredFrameReference(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameReference>(intPtr, (IntPtr n) => new LongExposureInfraredFrameReference(n));
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00022DE3 File Offset: 0x00020FE3
		private void __EventCleanup()
		{
		}

		// Token: 0x040002D4 RID: 724
		internal IntPtr _pNative;
	}
}
