using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x0200004F RID: 79
	public sealed class AudioBodyCorrelation : INativeWrapper
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0001B3F7 File Offset: 0x000195F7
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001B3FF File Offset: 0x000195FF
		internal AudioBodyCorrelation(IntPtr pNative)
		{
			this._pNative = pNative;
			AudioBodyCorrelation.Windows_Kinect_AudioBodyCorrelation_AddRefObject(ref this._pNative);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001B41C File Offset: 0x0001961C
		~AudioBodyCorrelation()
		{
			this.Dispose(false);
		}

		// Token: 0x060003CF RID: 975
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBodyCorrelation_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060003D0 RID: 976
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_AudioBodyCorrelation_AddRefObject(ref IntPtr pNative);

		// Token: 0x060003D1 RID: 977 RVA: 0x0001B44C File Offset: 0x0001964C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<AudioBodyCorrelation>(this._pNative);
			AudioBodyCorrelation.Windows_Kinect_AudioBodyCorrelation_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060003D2 RID: 978
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern ulong Windows_Kinect_AudioBodyCorrelation_get_BodyTrackingId(IntPtr pNative);

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0001B488 File Offset: 0x00019688
		public ulong BodyTrackingId
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("AudioBodyCorrelation");
				}
				return AudioBodyCorrelation.Windows_Kinect_AudioBodyCorrelation_get_BodyTrackingId(this._pNative);
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001B4B2 File Offset: 0x000196B2
		private void __EventCleanup()
		{
		}

		// Token: 0x04000228 RID: 552
		internal IntPtr _pNative;
	}
}
