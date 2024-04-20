using System;
using System.Runtime.InteropServices;
using Helper;

namespace Windows.Data
{
	// Token: 0x02000038 RID: 56
	public sealed class PropertyChangedEventArgs : EventArgs, INativeWrapper
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00016C8B File Offset: 0x00014E8B
		IntPtr INativeWrapper.nativePtr
		{
			get
			{
				return this._pNative;
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00016C93 File Offset: 0x00014E93
		internal PropertyChangedEventArgs(IntPtr pNative)
		{
			this._pNative = pNative;
			PropertyChangedEventArgs.Windows_Data_PropertyChangedEventArgs_AddRefObject(ref this._pNative);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00016CB0 File Offset: 0x00014EB0
		~PropertyChangedEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x060001FA RID: 506
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Data_PropertyChangedEventArgs_ReleaseObject(ref IntPtr pNative);

		// Token: 0x060001FB RID: 507
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern void Windows_Data_PropertyChangedEventArgs_AddRefObject(ref IntPtr pNative);

		// Token: 0x060001FC RID: 508 RVA: 0x00016CE0 File Offset: 0x00014EE0
		private void Dispose(bool disposing)
		{
			if (this._pNative == IntPtr.Zero)
			{
				return;
			}
			this.__EventCleanup();
			NativeObjectCache.RemoveObject<PropertyChangedEventArgs>(this._pNative);
			PropertyChangedEventArgs.Windows_Data_PropertyChangedEventArgs_ReleaseObject(ref this._pNative);
			this._pNative = IntPtr.Zero;
		}

		// Token: 0x060001FD RID: 509
		[DllImport("KinectUnityAddin", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Windows_Data_PropertyChangedEventArgs_get_PropertyName(IntPtr pNative);

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00016D1C File Offset: 0x00014F1C
		public string PropertyName
		{
			get
			{
				if (this._pNative == IntPtr.Zero)
				{
					throw new ObjectDisposedException("PropertyChangedEventArgs");
				}
				IntPtr ptr = PropertyChangedEventArgs.Windows_Data_PropertyChangedEventArgs_get_PropertyName(this._pNative);
				ExceptionHelper.CheckLastError();
				string result = Marshal.PtrToStringUni(ptr);
				Marshal.FreeCoTaskMem(ptr);
				return result;
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00016D63 File Offset: 0x00014F63
		private void __EventCleanup()
		{
		}

		// Token: 0x040001F4 RID: 500
		internal IntPtr _pNative;
	}
}
