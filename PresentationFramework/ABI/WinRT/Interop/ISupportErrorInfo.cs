using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000091 RID: 145
	[Guid("DF0B3D60-548F-101B-8E65-08002B2BD119")]
	internal class ISupportErrorInfo : ISupportErrorInfo
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x000F9124 File Offset: 0x000F8124
		public static ObjectReference<ISupportErrorInfo.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ISupportErrorInfo.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000F912C File Offset: 0x000F812C
		public static implicit operator ISupportErrorInfo(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ISupportErrorInfo(obj);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000F9139 File Offset: 0x000F8139
		public static implicit operator ISupportErrorInfo(ObjectReference<ISupportErrorInfo.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ISupportErrorInfo(obj);
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000F9146 File Offset: 0x000F8146
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000F9153 File Offset: 0x000F8153
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000F9160 File Offset: 0x000F8160
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000F916D File Offset: 0x000F816D
		public ISupportErrorInfo(IObjectReference obj) : this(obj.As<ISupportErrorInfo.Vftbl>())
		{
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000F917B File Offset: 0x000F817B
		public ISupportErrorInfo(ObjectReference<ISupportErrorInfo.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000F918A File Offset: 0x000F818A
		public bool InterfaceSupportsErrorInfo(Guid riid)
		{
			return this._obj.Vftbl.InterfaceSupportsErrorInfo_0(this.ThisPtr, ref riid) == 0;
		}

		// Token: 0x04000579 RID: 1401
		protected readonly ObjectReference<ISupportErrorInfo.Vftbl> _obj;

		// Token: 0x0200087F RID: 2175
		[Guid("DF0B3D60-548F-101B-8E65-08002B2BD119")]
		internal struct Vftbl
		{
			// Token: 0x06008010 RID: 32784 RVA: 0x00321C48 File Offset: 0x00320C48
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<ISupportErrorInfo.Vftbl>()));
				Marshal.StructureToPtr<ISupportErrorInfo.Vftbl>(ISupportErrorInfo.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				ISupportErrorInfo.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x06008011 RID: 32785 RVA: 0x00321CB0 File Offset: 0x00320CB0
			private static int Do_Abi_InterfaceSupportsErrorInfo_0(IntPtr thisPtr, ref Guid guid)
			{
				int result;
				try
				{
					result = (ComWrappersSupport.FindObject<ISupportErrorInfo>(thisPtr).InterfaceSupportsErrorInfo(guid) ? 0 : 1);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					result = ExceptionHelpers.GetHRForException(ex);
				}
				return result;
			}

			// Token: 0x04003BC7 RID: 15303
			public IUnknownVftbl IUnknownVftbl;

			// Token: 0x04003BC8 RID: 15304
			public ISupportErrorInfo.Vftbl._InterfaceSupportsErrorInfo InterfaceSupportsErrorInfo_0;

			// Token: 0x04003BC9 RID: 15305
			private static readonly ISupportErrorInfo.Vftbl AbiToProjectionVftable = new ISupportErrorInfo.Vftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				InterfaceSupportsErrorInfo_0 = new ISupportErrorInfo.Vftbl._InterfaceSupportsErrorInfo(ISupportErrorInfo.Vftbl.Do_Abi_InterfaceSupportsErrorInfo_0)
			};

			// Token: 0x04003BCA RID: 15306
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x02000C6B RID: 3179
			// (Invoke) Token: 0x060091E9 RID: 37353
			internal delegate int _InterfaceSupportsErrorInfo(IntPtr thisPtr, ref Guid riid);
		}
	}
}
