using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Globalization;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Globalization
{
	// Token: 0x020002F1 RID: 753
	[ObjectReferenceWrapper("_obj")]
	[Guid("B23CD557-0865-46D4-89B8-D59BE8990F0D")]
	internal class ILanguageStatics : ILanguageStatics
	{
		// Token: 0x06001C5F RID: 7263 RVA: 0x0016B868 File Offset: 0x0016A868
		public static ObjectReference<ILanguageStatics.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ILanguageStatics.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x0016B870 File Offset: 0x0016A870
		public static implicit operator ILanguageStatics(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguageStatics(obj);
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001C61 RID: 7265 RVA: 0x0016B87D File Offset: 0x0016A87D
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x0016B885 File Offset: 0x0016A885
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x0016B892 File Offset: 0x0016A892
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x0016B89F File Offset: 0x0016A89F
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x0016B8AC File Offset: 0x0016A8AC
		public ILanguageStatics(IObjectReference obj) : this(obj.As<ILanguageStatics.Vftbl>())
		{
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x0016B8BA File Offset: 0x0016A8BA
		public ILanguageStatics(ObjectReference<ILanguageStatics.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x0016B8CC File Offset: 0x0016A8CC
		public bool IsWellFormed(string languageTag)
		{
			MarshalString m = null;
			byte b = 0;
			bool result;
			try
			{
				m = MarshalString.CreateMarshaler(languageTag);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsWellFormed_0(this.ThisPtr, MarshalString.GetAbi(m), out b));
				result = (b > 0);
			}
			finally
			{
				MarshalString.DisposeMarshaler(m);
			}
			return result;
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001C68 RID: 7272 RVA: 0x0016B92C File Offset: 0x0016A92C
		public string CurrentInputMethodLanguageTag
		{
			get
			{
				IntPtr intPtr = 0;
				string result;
				try
				{
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_CurrentInputMethodLanguageTag_1(this.ThisPtr, out intPtr));
					result = MarshalString.FromAbi(intPtr);
				}
				finally
				{
					MarshalString.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x04000E78 RID: 3704
		protected readonly ObjectReference<ILanguageStatics.Vftbl> _obj;

		// Token: 0x02000A35 RID: 2613
		[Guid("B23CD557-0865-46D4-89B8-D59BE8990F0D")]
		internal struct Vftbl
		{
			// Token: 0x0600855D RID: 34141 RVA: 0x00329000 File Offset: 0x00328000
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ILanguageStatics.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 2));
				Marshal.StructureToPtr<ILanguageStatics.Vftbl>(ILanguageStatics.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				ILanguageStatics.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x0600855E RID: 34142 RVA: 0x0032908C File Offset: 0x0032808C
			private static int Do_Abi_IsWellFormed_0(IntPtr thisPtr, IntPtr languageTag, out byte result)
			{
				result = 0;
				try
				{
					result = (ComWrappersSupport.FindObject<ILanguageStatics>(thisPtr).IsWellFormed(MarshalString.FromAbi(languageTag)) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600855F RID: 34143 RVA: 0x003290E0 File Offset: 0x003280E0
			private static int Do_Abi_get_CurrentInputMethodLanguageTag_1(IntPtr thisPtr, out IntPtr value)
			{
				value = 0;
				try
				{
					string currentInputMethodLanguageTag = ComWrappersSupport.FindObject<ILanguageStatics>(thisPtr).CurrentInputMethodLanguageTag;
					value = MarshalString.FromManaged(currentInputMethodLanguageTag);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040ED RID: 16621
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040EE RID: 16622
			public ILanguageStatics_Delegates.IsWellFormed_0 IsWellFormed_0;

			// Token: 0x040040EF RID: 16623
			public _get_PropertyAsString get_CurrentInputMethodLanguageTag_1;

			// Token: 0x040040F0 RID: 16624
			private static readonly ILanguageStatics.Vftbl AbiToProjectionVftable = new ILanguageStatics.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				IsWellFormed_0 = new ILanguageStatics_Delegates.IsWellFormed_0(ILanguageStatics.Vftbl.Do_Abi_IsWellFormed_0),
				get_CurrentInputMethodLanguageTag_1 = new _get_PropertyAsString(ILanguageStatics.Vftbl.Do_Abi_get_CurrentInputMethodLanguageTag_1)
			};

			// Token: 0x040040F1 RID: 16625
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
