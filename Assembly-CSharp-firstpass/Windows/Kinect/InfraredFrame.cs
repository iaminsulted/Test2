using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000043 RID: 67
	public sealed class InfraredFrame : IDisposable, INativeWrapper
	{
		// Token: 0x060002EF RID: 751
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_InfraredFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x060002F0 RID: 752 RVA: 0x000189F0 File Offset: 0x00016BF0
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrame");
			}
			InfraredFrame.Windows_Kinect_InfraredFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size / 2U);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060002F1 RID: 753
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrame_LockImageBuffer(IntPtr pNative);

		// Token: 0x060002F2 RID: 754 RVA: 0x00018A24 File Offset: 0x00016C24
		public KinectBuffer LockImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrame");
			}
			IntPtr intPtr = InfraredFrame.Windows_Kinect_InfraredFrame_LockImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00018A93 File Offset: 0x00016C93
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00018A9B File Offset: 0x00016C9B
		internal InfraredFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			InfraredFrame.Windows_Kinect_InfraredFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00018AB8 File Offset: 0x00016CB8
		~InfraredFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x060002F6 RID: 758
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060002F7 RID: 759
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x060002F8 RID: 760 RVA: 0x00018AE8 File Offset: 0x00016CE8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<InfraredFrame>(this._pNative);
			if (disposing)
			{
				InfraredFrame.Windows_Kinect_InfraredFrame_Dispose(this._pNative);
			}
			InfraredFrame.Windows_Kinect_InfraredFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060002F9 RID: 761
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00018B40 File Offset: 0x00016D40
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrame");
				}
				IntPtr intPtr = InfraredFrame.Windows_Kinect_InfraredFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x060002FB RID: 763
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_InfraredFrame_get_InfraredFrameSource(IntPtr pNative);

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00018BB0 File Offset: 0x00016DB0
		public InfraredFrameSource InfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrame");
				}
				IntPtr intPtr = InfraredFrame.Windows_Kinect_InfraredFrame_get_InfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<InfraredFrameSource>(intPtr, (IntPtr n) => new InfraredFrameSource(n));
			}
		}

		// Token: 0x060002FD RID: 765
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_InfraredFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00018C1F File Offset: 0x00016E1F
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("InfraredFrame");
				}
				return TimeSpan.FromMilliseconds((double)InfraredFrame.Windows_Kinect_InfraredFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x060002FF RID: 767
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x06000300 RID: 768 RVA: 0x00018C50 File Offset: 0x00016E50
		public void CopyFrameDataToArray(ushort[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("InfraredFrame");
			}
			IntPtr frameData2 = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			InfraredFrame.Windows_Kinect_InfraredFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000301 RID: 769
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_InfraredFrame_Dispose(IntPtr pNative);

		// Token: 0x06000302 RID: 770 RVA: 0x00018CA0 File Offset: 0x00016EA0
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00018CC2 File Offset: 0x00016EC2
		private void __EventCleanup()
		{
		}

		// Token: 0x04000207 RID: 519
		internal IntPtr _pNative;
	}
}
