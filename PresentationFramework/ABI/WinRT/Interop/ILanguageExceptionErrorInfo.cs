using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000090 RID: 144
	[Guid("04a2dbf3-df83-116c-0946-0812abf6e07d")]
	internal class ILanguageExceptionErrorInfo : ILanguageExceptionErrorInfo
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x000F9048 File Offset: 0x000F8048
		public static ObjectReference<ILanguageExceptionErrorInfo.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ILanguageExceptionErrorInfo.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000F9050 File Offset: 0x000F8050
		public static implicit operator ILanguageExceptionErrorInfo(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguageExceptionErrorInfo(obj);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000F905D File Offset: 0x000F805D
		public static implicit operator ILanguageExceptionErrorInfo(ObjectReference<ILanguageExceptionErrorInfo.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguageExceptionErrorInfo(obj);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000F906A File Offset: 0x000F806A
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000F9077 File Offset: 0x000F8077
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000F9084 File Offset: 0x000F8084
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000F9091 File Offset: 0x000F8091
		public ILanguageExceptionErrorInfo(IObjectReference obj) : this(obj.As<ILanguageExceptionErrorInfo.Vftbl>())
		{
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000F909F File Offset: 0x000F809F
		public ILanguageExceptionErrorInfo(ObjectReference<ILanguageExceptionErrorInfo.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000F90B0 File Offset: 0x000F80B0
		public IObjectReference GetLanguageException()
		{
			IntPtr zero = IntPtr.Zero;
			IObjectReference result;
			try
			{
				Marshal.ThrowExceptionForHR(this._obj.Vftbl.GetLanguageException_0(this.ThisPtr, out zero));
				result = ObjectReference<IUnknownVftbl>.Attach(ref zero);
			}
			finally
			{
				using (ObjectReference<IUnknownVftbl>.Attach(ref zero))
				{
				}
			}
			return result;
		}

		// Token: 0x04000578 RID: 1400
		protected readonly ObjectReference<ILanguageExceptionErrorInfo.Vftbl> _obj;

		// Token: 0x0200087E RID: 2174
		[Guid("04a2dbf3-df83-116c-0946-0812abf6e07d")]
		internal struct Vftbl
		{
			// Token: 0x04003BC5 RID: 15301
			public IUnknownVftbl IUnknownVftbl;

			// Token: 0x04003BC6 RID: 15302
			public ILanguageExceptionErrorInfo.Vftbl._GetLanguageException GetLanguageException_0;

			// Token: 0x02000C6A RID: 3178
			// (Invoke) Token: 0x060091E5 RID: 37349
			internal delegate int _GetLanguageException(IntPtr thisPtr, out IntPtr languageException);
		}
	}
}
