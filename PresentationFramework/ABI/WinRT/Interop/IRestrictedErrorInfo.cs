using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000092 RID: 146
	[Guid("82BA7092-4C88-427D-A7BC-16DD93FEB67E")]
	internal class IRestrictedErrorInfo : IRestrictedErrorInfo
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x000F91AC File Offset: 0x000F81AC
		public static ObjectReference<IRestrictedErrorInfo.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IRestrictedErrorInfo.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000F91B4 File Offset: 0x000F81B4
		public static implicit operator IRestrictedErrorInfo(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IRestrictedErrorInfo(obj);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000F91C1 File Offset: 0x000F81C1
		public static implicit operator IRestrictedErrorInfo(ObjectReference<IRestrictedErrorInfo.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IRestrictedErrorInfo(obj);
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001FC RID: 508 RVA: 0x000F91CE File Offset: 0x000F81CE
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000F91DB File Offset: 0x000F81DB
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000F91E8 File Offset: 0x000F81E8
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000F91F5 File Offset: 0x000F81F5
		public IRestrictedErrorInfo(IObjectReference obj) : this(obj.As<IRestrictedErrorInfo.Vftbl>())
		{
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000F9203 File Offset: 0x000F8203
		public IRestrictedErrorInfo(ObjectReference<IRestrictedErrorInfo.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000F9214 File Offset: 0x000F8214
		public void GetErrorDetails(out string description, out int error, out string restrictedDescription, out string capabilitySid)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			IntPtr zero3 = IntPtr.Zero;
			try
			{
				Marshal.ThrowExceptionForHR(this._obj.Vftbl.GetErrorDetails_0(this.ThisPtr, out zero, out error, out zero2, out zero3));
				description = ((zero != IntPtr.Zero) ? Marshal.PtrToStringBSTR(zero) : string.Empty);
				restrictedDescription = ((zero2 != IntPtr.Zero) ? Marshal.PtrToStringBSTR(zero2) : string.Empty);
				capabilitySid = ((zero3 != IntPtr.Zero) ? Marshal.PtrToStringBSTR(zero3) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(zero);
				Marshal.FreeBSTR(zero2);
				Marshal.FreeBSTR(zero3);
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000F92D4 File Offset: 0x000F82D4
		public string GetReference()
		{
			IntPtr intPtr = 0;
			string result;
			try
			{
				Marshal.ThrowExceptionForHR(this._obj.Vftbl.GetReference_1(this.ThisPtr, out intPtr));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x0400057A RID: 1402
		protected readonly ObjectReference<IRestrictedErrorInfo.Vftbl> _obj;

		// Token: 0x02000880 RID: 2176
		[Guid("82BA7092-4C88-427D-A7BC-16DD93FEB67E")]
		internal struct Vftbl
		{
			// Token: 0x04003BCB RID: 15307
			public IUnknownVftbl unknownVftbl;

			// Token: 0x04003BCC RID: 15308
			public IRestrictedErrorInfo.Vftbl._GetErrorDetails GetErrorDetails_0;

			// Token: 0x04003BCD RID: 15309
			public IRestrictedErrorInfo.Vftbl._GetReference GetReference_1;

			// Token: 0x02000C6C RID: 3180
			// (Invoke) Token: 0x060091ED RID: 37357
			internal delegate int _GetErrorDetails(IntPtr thisPtr, out IntPtr description, out int error, out IntPtr restrictedDescription, out IntPtr capabilitySid);

			// Token: 0x02000C6D RID: 3181
			// (Invoke) Token: 0x060091F1 RID: 37361
			internal delegate int _GetReference(IntPtr thisPtr, out IntPtr reference);
		}
	}
}
