using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Globalization;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Globalization
{
	// Token: 0x020002F3 RID: 755
	[Guid("30199F6E-914B-4B2A-9D6E-E3B0E27DBE4F")]
	[ObjectReferenceWrapper("_obj")]
	internal class ILanguageStatics2 : ILanguageStatics2
	{
		// Token: 0x06001C69 RID: 7273 RVA: 0x0016B984 File Offset: 0x0016A984
		public static ObjectReference<ILanguageStatics2.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ILanguageStatics2.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x0016B98C File Offset: 0x0016A98C
		public static implicit operator ILanguageStatics2(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguageStatics2(obj);
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x0016B999 File Offset: 0x0016A999
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001C6C RID: 7276 RVA: 0x0016B9A1 File Offset: 0x0016A9A1
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x0016B9AE File Offset: 0x0016A9AE
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x0016B9BB File Offset: 0x0016A9BB
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x0016B9C8 File Offset: 0x0016A9C8
		public ILanguageStatics2(IObjectReference obj) : this(obj.As<ILanguageStatics2.Vftbl>())
		{
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x0016B9D6 File Offset: 0x0016A9D6
		public ILanguageStatics2(ObjectReference<ILanguageStatics2.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x0016B9E8 File Offset: 0x0016A9E8
		public bool TrySetInputMethodLanguageTag(string languageTag)
		{
			MarshalString m = null;
			byte b = 0;
			bool result;
			try
			{
				m = MarshalString.CreateMarshaler(languageTag);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.TrySetInputMethodLanguageTag_0(this.ThisPtr, MarshalString.GetAbi(m), out b));
				result = (b > 0);
			}
			finally
			{
				MarshalString.DisposeMarshaler(m);
			}
			return result;
		}

		// Token: 0x04000E79 RID: 3705
		protected readonly ObjectReference<ILanguageStatics2.Vftbl> _obj;

		// Token: 0x02000A37 RID: 2615
		[Guid("30199F6E-914B-4B2A-9D6E-E3B0E27DBE4F")]
		internal struct Vftbl
		{
			// Token: 0x06008564 RID: 34148 RVA: 0x00329130 File Offset: 0x00328130
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ILanguageStatics2.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<ILanguageStatics2.Vftbl>(ILanguageStatics2.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				ILanguageStatics2.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x06008565 RID: 34149 RVA: 0x003291A8 File Offset: 0x003281A8
			private static int Do_Abi_TrySetInputMethodLanguageTag_0(IntPtr thisPtr, IntPtr languageTag, out byte result)
			{
				result = 0;
				try
				{
					result = (ComWrappersSupport.FindObject<ILanguageStatics2>(thisPtr).TrySetInputMethodLanguageTag(MarshalString.FromAbi(languageTag)) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040F2 RID: 16626
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040F3 RID: 16627
			public ILanguageStatics2_Delegates.TrySetInputMethodLanguageTag_0 TrySetInputMethodLanguageTag_0;

			// Token: 0x040040F4 RID: 16628
			private static readonly ILanguageStatics2.Vftbl AbiToProjectionVftable = new ILanguageStatics2.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				TrySetInputMethodLanguageTag_0 = new ILanguageStatics2_Delegates.TrySetInputMethodLanguageTag_0(ILanguageStatics2.Vftbl.Do_Abi_TrySetInputMethodLanguageTag_0)
			};

			// Token: 0x040040F5 RID: 16629
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
