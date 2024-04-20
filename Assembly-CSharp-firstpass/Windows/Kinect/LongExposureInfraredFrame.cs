using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000045 RID: 69
	public sealed class LongExposureInfraredFrame : IDisposable, INativeWrapper
	{
		// Token: 0x06000334 RID: 820
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x06000335 RID: 821 RVA: 0x000197F6 File Offset: 0x000179F6
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrame");
			}
			LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size / 2U);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000336 RID: 822
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrame_LockImageBuffer(IntPtr pNative);

		// Token: 0x06000337 RID: 823 RVA: 0x0001982C File Offset: 0x00017A2C
		public KinectBuffer LockImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrame");
			}
			IntPtr intPtr = LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_LockImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0001989B File Offset: 0x00017A9B
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000198A3 File Offset: 0x00017AA3
		internal LongExposureInfraredFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000198C0 File Offset: 0x00017AC0
		~LongExposureInfraredFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x0600033B RID: 827
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600033C RID: 828
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x0600033D RID: 829 RVA: 0x000198F0 File Offset: 0x00017AF0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<LongExposureInfraredFrame>(this._pNative);
			if (disposing)
			{
				LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_Dispose(this._pNative);
			}
			LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x0600033E RID: 830
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00019948 File Offset: 0x00017B48
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrame");
				}
				IntPtr intPtr = LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x06000340 RID: 832
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_LongExposureInfraredFrame_get_LongExposureInfraredFrameSource(IntPtr pNative);

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000341 RID: 833 RVA: 0x000199B8 File Offset: 0x00017BB8
		public LongExposureInfraredFrameSource LongExposureInfraredFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrame");
				}
				IntPtr intPtr = LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_get_LongExposureInfraredFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<LongExposureInfraredFrameSource>(intPtr, (IntPtr n) => new LongExposureInfraredFrameSource(n));
			}
		}

		// Token: 0x06000342 RID: 834
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_LongExposureInfraredFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00019A27 File Offset: 0x00017C27
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("LongExposureInfraredFrame");
				}
				return TimeSpan.FromMilliseconds((double)LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06000344 RID: 836
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x06000345 RID: 837 RVA: 0x00019A58 File Offset: 0x00017C58
		public void CopyFrameDataToArray(ushort[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("LongExposureInfraredFrame");
			}
			IntPtr frameData2 = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			LongExposureInfraredFrame.Windows_Kinect_LongExposureInfraredFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x06000346 RID: 838
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_LongExposureInfraredFrame_Dispose(IntPtr pNative);

		// Token: 0x06000347 RID: 839 RVA: 0x00019AA8 File Offset: 0x00017CA8
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00019ACA File Offset: 0x00017CCA
		private void __EventCleanup()
		{
		}

		// Token: 0x0400020D RID: 525
		internal IntPtr _pNative;
	}
}
