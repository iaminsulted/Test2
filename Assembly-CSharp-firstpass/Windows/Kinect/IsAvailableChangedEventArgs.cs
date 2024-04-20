using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000073 RID: 115
	public sealed class IsAvailableChangedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00021776 File Offset: 0x0001F976
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0002177E File Offset: 0x0001F97E
		internal IsAvailableChangedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			IsAvailableChangedEventArgs.Windows_Kinect_IsAvailableChangedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00021798 File Offset: 0x0001F998
		~IsAvailableChangedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x060005B6 RID: 1462
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_IsAvailableChangedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060005B7 RID: 1463
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_IsAvailableChangedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x060005B8 RID: 1464 RVA: 0x000217C8 File Offset: 0x0001F9C8
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<IsAvailableChangedEventArgs>(this._pNative);
			IsAvailableChangedEventArgs.Windows_Kinect_IsAvailableChangedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060005B9 RID: 1465
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern bool Windows_Kinect_IsAvailableChangedEventArgs_get_IsAvailable(IntPtr pNative);

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x00021804 File Offset: 0x0001FA04
		public bool IsAvailable
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("IsAvailableChangedEventArgs");
				}
				return IsAvailableChangedEventArgs.Windows_Kinect_IsAvailableChangedEventArgs_get_IsAvailable(this._pNative);
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0002182E File Offset: 0x0001FA2E
		private void __EventCleanup()
		{
		}

		// Token: 0x0400029D RID: 669
		internal IntPtr _pNative;
	}
}
