using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200004B RID: 75
	public sealed class AudioBeamFrameList : IDisposable, INativeWrapper
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0001AA69 File Offset: 0x00018C69
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001AA71 File Offset: 0x00018C71
		internal AudioBeamFrameList(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBeamFrameList.Windows_Kinect_AudioBeamFrameList_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001AA8C File Offset: 0x00018C8C
		~AudioBeamFrameList()
		{
			this.Dispose(false);
		}

		// Token: 0x0600039F RID: 927
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameList_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060003A0 RID: 928
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameList_AddRefObject(ref IntPtr pNative);

		// Token: 0x060003A1 RID: 929 RVA: 0x0001AABC File Offset: 0x00018CBC
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBeamFrameList>(this._pNative);
			if (disposing)
			{
				AudioBeamFrameList.Windows_Kinect_AudioBeamFrameList_Dispose(this._pNative);
			}
			AudioBeamFrameList.Windows_Kinect_AudioBeamFrameList_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060003A2 RID: 930
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBeamFrameList_Dispose(IntPtr pNative);

		// Token: 0x060003A3 RID: 931 RVA: 0x0001AB11 File Offset: 0x00018D11
		public void Dispose()
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001AB33 File Offset: 0x00018D33
		private void __EventCleanup()
		{
		}

		// Token: 0x0400021E RID: 542
		internal IntPtr _pNative;
	}
}
