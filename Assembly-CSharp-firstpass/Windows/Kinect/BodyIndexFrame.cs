using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000042 RID: 66
	public sealed class BodyIndexFrame : IDisposable, INativeWrapper
	{
		// Token: 0x060002DA RID: 730
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_BodyIndexFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x060002DB RID: 731 RVA: 0x0001871C File Offset: 0x0001691C
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrame");
			}
			BodyIndexFrame.Windows_Kinect_BodyIndexFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060002DC RID: 732
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrame_LockImageBuffer(IntPtr pNative);

		// Token: 0x060002DD RID: 733 RVA: 0x00018750 File Offset: 0x00016950
		public KinectBuffer LockImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrame");
			}
			IntPtr intPtr = BodyIndexFrame.Windows_Kinect_BodyIndexFrame_LockImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060002DE RID: 734 RVA: 0x000187BF File Offset: 0x000169BF
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000187C7 File Offset: 0x000169C7
		internal BodyIndexFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			BodyIndexFrame.Windows_Kinect_BodyIndexFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000187E4 File Offset: 0x000169E4
		~BodyIndexFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x060002E1 RID: 737
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060002E2 RID: 738
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x060002E3 RID: 739 RVA: 0x00018814 File Offset: 0x00016A14
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<BodyIndexFrame>(this._pNative);
			if (disposing)
			{
				BodyIndexFrame.Windows_Kinect_BodyIndexFrame_Dispose(this._pNative);
			}
			BodyIndexFrame.Windows_Kinect_BodyIndexFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060002E4 RID: 740
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrame_get_BodyIndexFrameSource(IntPtr pNative);

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0001886C File Offset: 0x00016A6C
		public BodyIndexFrameSource BodyIndexFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrame");
				}
				IntPtr intPtr = BodyIndexFrame.Windows_Kinect_BodyIndexFrame_get_BodyIndexFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<BodyIndexFrameSource>(intPtr, (IntPtr n) => new BodyIndexFrameSource(n));
			}
		}

		// Token: 0x060002E6 RID: 742
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_BodyIndexFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x000188DC File Offset: 0x00016ADC
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrame");
				}
				IntPtr intPtr = BodyIndexFrame.Windows_Kinect_BodyIndexFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x060002E8 RID: 744
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_BodyIndexFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0001894B File Offset: 0x00016B4B
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("BodyIndexFrame");
				}
				return TimeSpan.FromMilliseconds((double)BodyIndexFrame.Windows_Kinect_BodyIndexFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x060002EA RID: 746
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x060002EB RID: 747 RVA: 0x0001897C File Offset: 0x00016B7C
		public void CopyFrameDataToArray(byte[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("BodyIndexFrame");
			}
			IntPtr frameData2 = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			BodyIndexFrame.Windows_Kinect_BodyIndexFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060002EC RID: 748
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_BodyIndexFrame_Dispose(IntPtr pNative);

		// Token: 0x060002ED RID: 749 RVA: 0x000189CC File Offset: 0x00016BCC
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000189EE File Offset: 0x00016BEE
		private void __EventCleanup()
		{
		}

		// Token: 0x04000206 RID: 518
		internal IntPtr _pNative;
	}
}
