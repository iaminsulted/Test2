using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Globalization;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Globalization
{
	// Token: 0x020002EB RID: 747
	[ObjectReferenceWrapper("_obj")]
	[Guid("6A47E5B5-D94D-4886-A404-A5A5B9D5B494")]
	internal class ILanguage2 : ILanguage2
	{
		// Token: 0x06001C44 RID: 7236 RVA: 0x0016B62C File Offset: 0x0016A62C
		public static ObjectReference<ILanguage2.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ILanguage2.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x0016B634 File Offset: 0x0016A634
		public static implicit operator ILanguage2(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguage2(obj);
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x0016B641 File Offset: 0x0016A641
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001C47 RID: 7239 RVA: 0x0016B649 File Offset: 0x0016A649
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x0016B656 File Offset: 0x0016A656
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x0016B663 File Offset: 0x0016A663
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x0016B670 File Offset: 0x0016A670
		public ILanguage2(IObjectReference obj) : this(obj.As<ILanguage2.Vftbl>())
		{
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x0016B67E File Offset: 0x0016A67E
		public ILanguage2(ObjectReference<ILanguage2.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001C4C RID: 7244 RVA: 0x0016B690 File Offset: 0x0016A690
		public LanguageLayoutDirection LayoutDirection
		{
			get
			{
				LanguageLayoutDirection result = LanguageLayoutDirection.Ltr;
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_LayoutDirection_0(this.ThisPtr, out result));
				return result;
			}
		}

		// Token: 0x04000E75 RID: 3701
		protected readonly ObjectReference<ILanguage2.Vftbl> _obj;

		// Token: 0x02000A2F RID: 2607
		[Guid("6A47E5B5-D94D-4886-A404-A5A5B9D5B494")]
		internal struct Vftbl
		{
			// Token: 0x0600854B RID: 34123 RVA: 0x00328DAC File Offset: 0x00327DAC
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ILanguage2.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<ILanguage2.Vftbl>(ILanguage2.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				ILanguage2.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x0600854C RID: 34124 RVA: 0x00328E24 File Offset: 0x00327E24
			private static int Do_Abi_get_LayoutDirection_0(IntPtr thisPtr, out LanguageLayoutDirection value)
			{
				value = LanguageLayoutDirection.Ltr;
				try
				{
					LanguageLayoutDirection layoutDirection = ComWrappersSupport.FindObject<ILanguage2>(thisPtr).LayoutDirection;
					value = layoutDirection;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040E1 RID: 16609
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040E2 RID: 16610
			public ILanguage2_Delegates.get_LayoutDirection_0 get_LayoutDirection_0;

			// Token: 0x040040E3 RID: 16611
			private static readonly ILanguage2.Vftbl AbiToProjectionVftable = new ILanguage2.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				get_LayoutDirection_0 = new ILanguage2_Delegates.get_LayoutDirection_0(ILanguage2.Vftbl.Do_Abi_get_LayoutDirection_0)
			};

			// Token: 0x040040E4 RID: 16612
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
