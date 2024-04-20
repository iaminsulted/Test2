using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000069 RID: 105
	public sealed class FrameCapturedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0002041A File Offset: 0x0001E61A
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00020422 File Offset: 0x0001E622
		internal FrameCapturedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0002043C File Offset: 0x0001E63C
		~FrameCapturedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x0600054E RID: 1358
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_FrameCapturedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x0600054F RID: 1359
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_FrameCapturedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x06000550 RID: 1360 RVA: 0x0002046C File Offset: 0x0001E66C
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<FrameCapturedEventArgs>(this._pNative);
			FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x06000551 RID: 1361
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern FrameCapturedStatus Windows_Kinect_FrameCapturedEventArgs_get_FrameStatus(IntPtr pNative);

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x000204A8 File Offset: 0x0001E6A8
		public FrameCapturedStatus FrameStatus
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameCapturedEventArgs");
				}
				return FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_get_FrameStatus(this._pNative);
			}
		}

		// Token: 0x06000553 RID: 1363
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern FrameSourceTypes Windows_Kinect_FrameCapturedEventArgs_get_FrameType(IntPtr pNative);

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x000204D2 File Offset: 0x0001E6D2
		public FrameSourceTypes FrameType
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameCapturedEventArgs");
				}
				return FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_get_FrameType(this._pNative);
			}
		}

		// Token: 0x06000555 RID: 1365
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern long Windows_Kinect_FrameCapturedEventArgs_get_RelativeTime(IntPtr pNative);

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x000204FC File Offset: 0x0001E6FC
		public TimeSpan RelativeTime
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("FrameCapturedEventArgs");
				}
				return TimeSpan.FromMilliseconds((double)FrameCapturedEventArgs.Windows_Kinect_FrameCapturedEventArgs_get_RelativeTime(this._pNative));
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0002052C File Offset: 0x0001E72C
		private void __EventCleanup()
		{
		}

		// Token: 0x04000276 RID: 630
		internal IntPtr _pNative;
	}
}
