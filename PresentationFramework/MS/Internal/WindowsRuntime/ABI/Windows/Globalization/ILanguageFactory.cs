using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Globalization;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Globalization
{
	// Token: 0x020002EF RID: 751
	[ObjectReferenceWrapper("_obj")]
	[Guid("9B0252AC-0C27-44F8-B792-9793FB66C63E")]
	internal class ILanguageFactory : ILanguageFactory
	{
		// Token: 0x06001C56 RID: 7254 RVA: 0x0016B794 File Offset: 0x0016A794
		public static ObjectReference<ILanguageFactory.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ILanguageFactory.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x0016B79C File Offset: 0x0016A79C
		public static implicit operator ILanguageFactory(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguageFactory(obj);
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x0016B7A9 File Offset: 0x0016A7A9
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001C59 RID: 7257 RVA: 0x0016B7B1 File Offset: 0x0016A7B1
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x0016B7BE File Offset: 0x0016A7BE
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x0016B7CB File Offset: 0x0016A7CB
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x0016B7D8 File Offset: 0x0016A7D8
		public ILanguageFactory(IObjectReference obj) : this(obj.As<ILanguageFactory.Vftbl>())
		{
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x0016B7E6 File Offset: 0x0016A7E6
		public ILanguageFactory(ObjectReference<ILanguageFactory.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x0016B7F8 File Offset: 0x0016A7F8
		public Language CreateLanguage(string languageTag)
		{
			MarshalString m = null;
			IntPtr intPtr = 0;
			Language result;
			try
			{
				m = MarshalString.CreateMarshaler(languageTag);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.CreateLanguage_0(this.ThisPtr, MarshalString.GetAbi(m), out intPtr));
				result = Language.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeMarshaler(m);
				Language.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x04000E77 RID: 3703
		protected readonly ObjectReference<ILanguageFactory.Vftbl> _obj;

		// Token: 0x02000A33 RID: 2611
		[Guid("9B0252AC-0C27-44F8-B792-9793FB66C63E")]
		internal struct Vftbl
		{
			// Token: 0x06008557 RID: 34135 RVA: 0x00328F34 File Offset: 0x00327F34
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ILanguageFactory.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<ILanguageFactory.Vftbl>(ILanguageFactory.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				ILanguageFactory.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x06008558 RID: 34136 RVA: 0x00328FAC File Offset: 0x00327FAC
			private static int Do_Abi_CreateLanguage_0(IntPtr thisPtr, IntPtr languageTag, out IntPtr result)
			{
				result = 0;
				try
				{
					Language obj = ComWrappersSupport.FindObject<ILanguageFactory>(thisPtr).CreateLanguage(MarshalString.FromAbi(languageTag));
					result = Language.FromManaged(obj);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040E9 RID: 16617
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040EA RID: 16618
			public ILanguageFactory_Delegates.CreateLanguage_0 CreateLanguage_0;

			// Token: 0x040040EB RID: 16619
			private static readonly ILanguageFactory.Vftbl AbiToProjectionVftable = new ILanguageFactory.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				CreateLanguage_0 = new ILanguageFactory_Delegates.CreateLanguage_0(ILanguageFactory.Vftbl.Do_Abi_CreateLanguage_0)
			};

			// Token: 0x040040EC RID: 16620
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
