using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000041 RID: 65
	public sealed class DepthFrame : IDisposable, INativeWrapper
	{
		// Token: 0x060002C1 RID: 705
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Windows_Kinect_DepthFrame_CopyFrameDataToArray", SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_CopyFrameDataToIntPtr(IntPtr pNative, IntPtr frameData, uint frameDataSize);

		// Token: 0x060002C2 RID: 706 RVA: 0x000183F4 File Offset: 0x000165F4
		public void CopyFrameDataToIntPtr(IntPtr frameData, uint size)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrame");
			}
			DepthFrame.Windows_Kinect_DepthFrame_CopyFrameDataToIntPtr(this._pNative, frameData, size / 2U);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060002C3 RID: 707
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrame_LockImageBuffer(IntPtr pNative);

		// Token: 0x060002C4 RID: 708 RVA: 0x00018428 File Offset: 0x00016628
		public KinectBuffer LockImageBuffer()
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrame");
			}
			IntPtr intPtr = DepthFrame.Windows_Kinect_DepthFrame_LockImageBuffer(this._pNative);
			ExceptionHelper.CheckLastError();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return NativeObjectCache.CreateOrGetObject<KinectBuffer>(intPtr, (IntPtr n) => new KinectBuffer(n));
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x00018497 File Offset: 0x00016697
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0001849F File Offset: 0x0001669F
		internal DepthFrame(IntPtr pNative)
		{
			this._pNative = pNative;
			DepthFrame.Windows_Kinect_DepthFrame_AddRefObject(ref this._pNative);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000184BC File Offset: 0x000166BC
		~DepthFrame()
		{
			this.Dispose(false);
		}

		// Token: 0x060002C8 RID: 712
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060002C9 RID: 713
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_AddRefObject(ref IntPtr pNative);

		// Token: 0x060002CA RID: 714 RVA: 0x000184EC File Offset: 0x000166EC
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<DepthFrame>(this._pNative);
			if (disposing)
			{
				DepthFrame.Windows_Kinect_DepthFrame_Dispose(this._pNative);
			}
			DepthFrame.Windows_Kinect_DepthFrame_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060002CB RID: 715
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrame_get_DepthFrameSource(IntPtr pNative);

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00018544 File Offset: 0x00016744
		public DepthFrameSource DepthFrameSource
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				IntPtr intPtr = DepthFrame.Windows_Kinect_DepthFrame_get_DepthFrameSource(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<DepthFrameSource>(intPtr, (IntPtr n) => new DepthFrameSource(n));
			}
		}

		// Token: 0x060002CD RID: 717
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ushort Windows_Kinect_DepthFrame_get_DepthMaxReliableDistance(IntPtr pNative);

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002CE RID: 718 RVA: 0x000185B3 File Offset: 0x000167B3
		public ushort DepthMaxReliableDistance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				return DepthFrame.Windows_Kinect_DepthFrame_get_DepthMaxReliableDistance(this._pNative);
			}
		}

		// Token: 0x060002CF RID: 719
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ushort Windows_Kinect_DepthFrame_get_DepthMinReliableDistance(IntPtr pNative);

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x000185DD File Offset: 0x000167DD
		public ushort DepthMinReliableDistance
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				return DepthFrame.Windows_Kinect_DepthFrame_get_DepthMinReliableDistance(this._pNative);
			}
		}

		// Token: 0x060002D1 RID: 721
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Kinect_DepthFrame_get_FrameDescription(IntPtr pNative);

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00018608 File Offset: 0x00016808
		public FrameDescription FrameDescription
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				IntPtr intPtr = DepthFrame.Windows_Kinect_DepthFrame_get_FrameDescription(this._pNative);
				ExceptionHelper.CheckLastError();
				if (intPtr == IntPtr.Zero)
				{
					return null;
				}
				return NativeObjectCache.CreateOrGetObject<FrameDescription>(intPtr, (IntPtr n) => new FrameDescription(n));
			}
		}

		// Token: 0x060002D3 RID: 723
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_DepthFrame_get_RelativeTime(IntPtr pNative);

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00018677 File Offset: 0x00016877
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("DepthFrame");
				}
				return TimeSpan.FromMilliseconds((double)DepthFrame.Windows_Kinect_DepthFrame_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x060002D5 RID: 725
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_CopyFrameDataToArray(IntPtr pNative, IntPtr frameData, int frameDataSize);

		// Token: 0x060002D6 RID: 726 RVA: 0x000186A8 File Offset: 0x000168A8
		public void CopyFrameDataToArray(ushort[] frameData)
		{
			if (this._pNative == IntPtr.Zero)
			{
				throw new ObjectDisposedException("DepthFrame");
			}
			IntPtr frameData2 = new SmartGCHandle(GCHandle.Alloc(frameData, GCHandleType.Pinned)).AddrOfPinnedObject();
			DepthFrame.Windows_Kinect_DepthFrame_CopyFrameDataToArray(this._pNative, frameData2, frameData.Length);
			ExceptionHelper.CheckLastError();
		}

		// Token: 0x060002D7 RID: 727
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_DepthFrame_Dispose(IntPtr pNative);

		// Token: 0x060002D8 RID: 728 RVA: 0x000186F8 File Offset: 0x000168F8
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0001871A File Offset: 0x0001691A
		private void __EventCleanup()
		{
		}

		// Token: 0x04000205 RID: 517
		internal IntPtr _pNative;
	}
}
