using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x020002FF RID: 767
	[ObjectReferenceWrapper("_obj")]
	[Guid("E6977274-FC35-455C-8BFB-6D7F4653CA97")]
	internal class IWordsSegmenterFactory : IWordsSegmenterFactory
	{
		// Token: 0x06001CC5 RID: 7365 RVA: 0x0016C43C File Offset: 0x0016B43C
		public static ObjectReference<IWordsSegmenterFactory.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IWordsSegmenterFactory.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x0016C444 File Offset: 0x0016B444
		public static implicit operator IWordsSegmenterFactory(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IWordsSegmenterFactory(obj);
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x0016C451 File Offset: 0x0016B451
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x0016C459 File Offset: 0x0016B459
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x0016C466 File Offset: 0x0016B466
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0016C473 File Offset: 0x0016B473
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x0016C480 File Offset: 0x0016B480
		public IWordsSegmenterFactory(IObjectReference obj) : this(obj.As<IWordsSegmenterFactory.Vftbl>())
		{
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x0016C48E File Offset: 0x0016B48E
		public IWordsSegmenterFactory(ObjectReference<IWordsSegmenterFactory.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0016C4A0 File Offset: 0x0016B4A0
		public WordsSegmenter CreateWithLanguage(string language)
		{
			MarshalString m = null;
			IntPtr intPtr = 0;
			WordsSegmenter result;
			try
			{
				m = MarshalString.CreateMarshaler(language);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.CreateWithLanguage_0(this.ThisPtr, MarshalString.GetAbi(m), out intPtr));
				result = WordsSegmenter.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeMarshaler(m);
				WordsSegmenter.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x04000E7E RID: 3710
		protected readonly ObjectReference<IWordsSegmenterFactory.Vftbl> _obj;

		// Token: 0x02000A56 RID: 2646
		[Guid("E6977274-FC35-455C-8BFB-6D7F4653CA97")]
		internal struct Vftbl
		{
			// Token: 0x060085ED RID: 34285 RVA: 0x00329DF4 File Offset: 0x00328DF4
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IWordsSegmenterFactory.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<IWordsSegmenterFactory.Vftbl>(IWordsSegmenterFactory.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				IWordsSegmenterFactory.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x060085EE RID: 34286 RVA: 0x00329E6C File Offset: 0x00328E6C
			private static int Do_Abi_CreateWithLanguage_0(IntPtr thisPtr, IntPtr language, out IntPtr result)
			{
				result = 0;
				try
				{
					WordsSegmenter obj = ComWrappersSupport.FindObject<IWordsSegmenterFactory>(thisPtr).CreateWithLanguage(MarshalString.FromAbi(language));
					result = WordsSegmenter.FromManaged(obj);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04004123 RID: 16675
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04004124 RID: 16676
			public IWordsSegmenterFactory_Delegates.CreateWithLanguage_0 CreateWithLanguage_0;

			// Token: 0x04004125 RID: 16677
			private static readonly IWordsSegmenterFactory.Vftbl AbiToProjectionVftable = new IWordsSegmenterFactory.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				CreateWithLanguage_0 = new IWordsSegmenterFactory_Delegates.CreateWithLanguage_0(IWordsSegmenterFactory.Vftbl.Do_Abi_CreateWithLanguage_0)
			};

			// Token: 0x04004126 RID: 16678
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
