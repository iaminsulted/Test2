using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Kinect
{
	// Token: 0x02000061 RID: 97
	public sealed class CoordinateMappingChangedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0001F1EE File Offset: 0x0001D3EE
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001F1F6 File Offset: 0x0001D3F6
		internal CoordinateMappingChangedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			CoordinateMappingChangedEventArgs.Windows_Kinect_CoordinateMappingChangedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001F210 File Offset: 0x0001D410
		~CoordinateMappingChangedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x060004F4 RID: 1268
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMappingChangedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060004F5 RID: 1269
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Kinect_CoordinateMappingChangedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001F240 File Offset: 0x0001D440
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<CoordinateMappingChangedEventArgs>(this._pNative);
			CoordinateMappingChangedEventArgs.Windows_Kinect_CoordinateMappingChangedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001F27C File Offset: 0x0001D47C
		private void __EventCleanup()
		{
		}

		// Token: 0x0400025F RID: 607
		internal IntPtr _pNative;
	}
}
