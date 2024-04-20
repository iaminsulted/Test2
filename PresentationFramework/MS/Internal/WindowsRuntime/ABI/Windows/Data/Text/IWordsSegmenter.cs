using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.ABI.System.Collections.Generic;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x020002FD RID: 765
	[ObjectReferenceWrapper("_obj")]
	[Guid("86B4D4D1-B2FE-4E34-A81D-66640300454F")]
	internal class IWordsSegmenter : IWordsSegmenter
	{
		// Token: 0x06001CB9 RID: 7353 RVA: 0x0016C234 File Offset: 0x0016B234
		public static ObjectReference<IWordsSegmenter.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IWordsSegmenter.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x0016C23C File Offset: 0x0016B23C
		public static implicit operator IWordsSegmenter(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IWordsSegmenter(obj);
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001CBB RID: 7355 RVA: 0x0016C249 File Offset: 0x0016B249
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001CBC RID: 7356 RVA: 0x0016C251 File Offset: 0x0016B251
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x0016C25E File Offset: 0x0016B25E
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x0016C26B File Offset: 0x0016B26B
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x0016C278 File Offset: 0x0016B278
		public IWordsSegmenter(IObjectReference obj) : this(obj.As<IWordsSegmenter.Vftbl>())
		{
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x0016C286 File Offset: 0x0016B286
		public IWordsSegmenter(ObjectReference<IWordsSegmenter.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x0016C298 File Offset: 0x0016B298
		public WordSegment GetTokenAt(string text, uint startIndex)
		{
			MarshalString m = null;
			IntPtr intPtr = 0;
			WordSegment result;
			try
			{
				m = MarshalString.CreateMarshaler(text);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetTokenAt_1(this.ThisPtr, MarshalString.GetAbi(m), startIndex, out intPtr));
				result = WordSegment.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeMarshaler(m);
				WordSegment.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x0016C308 File Offset: 0x0016B308
		public IReadOnlyList<WordSegment> GetTokens(string text)
		{
			MarshalString m = null;
			IntPtr intPtr = 0;
			IReadOnlyList<WordSegment> result;
			try
			{
				m = MarshalString.CreateMarshaler(text);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetTokens_2(this.ThisPtr, MarshalString.GetAbi(m), out intPtr));
				result = IReadOnlyList<WordSegment>.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeMarshaler(m);
				IReadOnlyList<WordSegment>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x0016C378 File Offset: 0x0016B378
		public void Tokenize(string text, uint startIndex, WordSegmentsTokenizingHandler handler)
		{
			MarshalString m = null;
			IObjectReference value = null;
			try
			{
				m = MarshalString.CreateMarshaler(text);
				value = WordSegmentsTokenizingHandler.CreateMarshaler(handler);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.Tokenize_3(this.ThisPtr, MarshalString.GetAbi(m), startIndex, WordSegmentsTokenizingHandler.GetAbi(value)));
			}
			finally
			{
				MarshalString.DisposeMarshaler(m);
				WordSegmentsTokenizingHandler.DisposeMarshaler(value);
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x0016C3E4 File Offset: 0x0016B3E4
		public string ResolvedLanguage
		{
			get
			{
				IntPtr intPtr = 0;
				string result;
				try
				{
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_ResolvedLanguage_0(this.ThisPtr, out intPtr));
					result = MarshalString.FromAbi(intPtr);
				}
				finally
				{
					MarshalString.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x04000E7D RID: 3709
		protected readonly ObjectReference<IWordsSegmenter.Vftbl> _obj;

		// Token: 0x02000A52 RID: 2642
		[Guid("86B4D4D1-B2FE-4E34-A81D-66640300454F")]
		internal struct Vftbl
		{
			// Token: 0x060085DC RID: 34268 RVA: 0x00329C00 File Offset: 0x00328C00
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IWordsSegmenter.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
				Marshal.StructureToPtr<IWordsSegmenter.Vftbl>(IWordsSegmenter.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				IWordsSegmenter.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x060085DD RID: 34269 RVA: 0x00329CB4 File Offset: 0x00328CB4
			private static int Do_Abi_GetTokenAt_1(IntPtr thisPtr, IntPtr text, uint startIndex, out IntPtr result)
			{
				result = 0;
				try
				{
					WordSegment tokenAt = ComWrappersSupport.FindObject<IWordsSegmenter>(thisPtr).GetTokenAt(MarshalString.FromAbi(text), startIndex);
					result = WordSegment.FromManaged(tokenAt);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060085DE RID: 34270 RVA: 0x00329D08 File Offset: 0x00328D08
			private static int Do_Abi_GetTokens_2(IntPtr thisPtr, IntPtr text, out IntPtr result)
			{
				result = 0;
				try
				{
					IReadOnlyList<WordSegment> tokens = ComWrappersSupport.FindObject<IWordsSegmenter>(thisPtr).GetTokens(MarshalString.FromAbi(text));
					result = IReadOnlyList<WordSegment>.FromManaged(tokens);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060085DF RID: 34271 RVA: 0x00329D5C File Offset: 0x00328D5C
			private static int Do_Abi_Tokenize_3(IntPtr thisPtr, IntPtr text, uint startIndex, IntPtr handler)
			{
				try
				{
					ComWrappersSupport.FindObject<IWordsSegmenter>(thisPtr).Tokenize(MarshalString.FromAbi(text), startIndex, WordSegmentsTokenizingHandler.FromAbi(handler));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060085E0 RID: 34272 RVA: 0x00329DA4 File Offset: 0x00328DA4
			private static int Do_Abi_get_ResolvedLanguage_0(IntPtr thisPtr, out IntPtr value)
			{
				value = 0;
				try
				{
					string resolvedLanguage = ComWrappersSupport.FindObject<IWordsSegmenter>(thisPtr).ResolvedLanguage;
					value = MarshalString.FromManaged(resolvedLanguage);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400411C RID: 16668
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400411D RID: 16669
			public _get_PropertyAsString get_ResolvedLanguage_0;

			// Token: 0x0400411E RID: 16670
			public IWordsSegmenter_Delegates.GetTokenAt_1 GetTokenAt_1;

			// Token: 0x0400411F RID: 16671
			public IWordsSegmenter_Delegates.GetTokens_2 GetTokens_2;

			// Token: 0x04004120 RID: 16672
			public IWordsSegmenter_Delegates.Tokenize_3 Tokenize_3;

			// Token: 0x04004121 RID: 16673
			private static readonly IWordsSegmenter.Vftbl AbiToProjectionVftable = new IWordsSegmenter.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				get_ResolvedLanguage_0 = new _get_PropertyAsString(IWordsSegmenter.Vftbl.Do_Abi_get_ResolvedLanguage_0),
				GetTokenAt_1 = new IWordsSegmenter_Delegates.GetTokenAt_1(IWordsSegmenter.Vftbl.Do_Abi_GetTokenAt_1),
				GetTokens_2 = new IWordsSegmenter_Delegates.GetTokens_2(IWordsSegmenter.Vftbl.Do_Abi_GetTokens_2),
				Tokenize_3 = new IWordsSegmenter_Delegates.Tokenize_3(IWordsSegmenter.Vftbl.Do_Abi_Tokenize_3)
			};

			// Token: 0x04004122 RID: 16674
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
