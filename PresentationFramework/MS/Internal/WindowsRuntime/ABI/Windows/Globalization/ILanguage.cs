using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Globalization;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Globalization
{
	// Token: 0x020002EA RID: 746
	[ObjectReferenceWrapper("_obj")]
	[Guid("EA79A752-F7C2-4265-B1BD-C4DEC4E4F080")]
	internal class ILanguage : ILanguage
	{
		// Token: 0x06001C38 RID: 7224 RVA: 0x0016B46A File Offset: 0x0016A46A
		public static ObjectReference<ILanguage.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ILanguage.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x0016B472 File Offset: 0x0016A472
		public static implicit operator ILanguage(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguage(obj);
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x0016B47F File Offset: 0x0016A47F
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001C3B RID: 7227 RVA: 0x0016B487 File Offset: 0x0016A487
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x0016B494 File Offset: 0x0016A494
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0016B4A1 File Offset: 0x0016A4A1
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x0016B4AE File Offset: 0x0016A4AE
		public ILanguage(IObjectReference obj) : this(obj.As<ILanguage.Vftbl>())
		{
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x0016B4BC File Offset: 0x0016A4BC
		public ILanguage(ObjectReference<ILanguage.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x0016B4CC File Offset: 0x0016A4CC
		public string DisplayName
		{
			get
			{
				IntPtr intPtr = 0;
				string result;
				try
				{
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_DisplayName_1(this.ThisPtr, out intPtr));
					result = MarshalString.FromAbi(intPtr);
				}
				finally
				{
					MarshalString.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001C41 RID: 7233 RVA: 0x0016B524 File Offset: 0x0016A524
		public string LanguageTag
		{
			get
			{
				IntPtr intPtr = 0;
				string result;
				try
				{
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_LanguageTag_0(this.ThisPtr, out intPtr));
					result = MarshalString.FromAbi(intPtr);
				}
				finally
				{
					MarshalString.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x0016B57C File Offset: 0x0016A57C
		public string NativeName
		{
			get
			{
				IntPtr intPtr = 0;
				string result;
				try
				{
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_NativeName_2(this.ThisPtr, out intPtr));
					result = MarshalString.FromAbi(intPtr);
				}
				finally
				{
					MarshalString.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x0016B5D4 File Offset: 0x0016A5D4
		public string Script
		{
			get
			{
				IntPtr intPtr = 0;
				string result;
				try
				{
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_Script_3(this.ThisPtr, out intPtr));
					result = MarshalString.FromAbi(intPtr);
				}
				finally
				{
					MarshalString.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x04000E74 RID: 3700
		protected readonly ObjectReference<ILanguage.Vftbl> _obj;

		// Token: 0x02000A2E RID: 2606
		[Guid("EA79A752-F7C2-4265-B1BD-C4DEC4E4F080")]
		internal struct Vftbl
		{
			// Token: 0x06008546 RID: 34118 RVA: 0x00328BB8 File Offset: 0x00327BB8
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ILanguage.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
				Marshal.StructureToPtr<ILanguage.Vftbl>(ILanguage.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				ILanguage.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x06008547 RID: 34119 RVA: 0x00328C6C File Offset: 0x00327C6C
			private static int Do_Abi_get_DisplayName_1(IntPtr thisPtr, out IntPtr value)
			{
				value = 0;
				try
				{
					string displayName = ComWrappersSupport.FindObject<ILanguage>(thisPtr).DisplayName;
					value = MarshalString.FromManaged(displayName);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008548 RID: 34120 RVA: 0x00328CBC File Offset: 0x00327CBC
			private static int Do_Abi_get_LanguageTag_0(IntPtr thisPtr, out IntPtr value)
			{
				value = 0;
				try
				{
					string languageTag = ComWrappersSupport.FindObject<ILanguage>(thisPtr).LanguageTag;
					value = MarshalString.FromManaged(languageTag);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008549 RID: 34121 RVA: 0x00328D0C File Offset: 0x00327D0C
			private static int Do_Abi_get_NativeName_2(IntPtr thisPtr, out IntPtr value)
			{
				value = 0;
				try
				{
					string nativeName = ComWrappersSupport.FindObject<ILanguage>(thisPtr).NativeName;
					value = MarshalString.FromManaged(nativeName);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600854A RID: 34122 RVA: 0x00328D5C File Offset: 0x00327D5C
			private static int Do_Abi_get_Script_3(IntPtr thisPtr, out IntPtr value)
			{
				value = 0;
				try
				{
					string script = ComWrappersSupport.FindObject<ILanguage>(thisPtr).Script;
					value = MarshalString.FromManaged(script);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040DA RID: 16602
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040DB RID: 16603
			public _get_PropertyAsString get_LanguageTag_0;

			// Token: 0x040040DC RID: 16604
			public _get_PropertyAsString get_DisplayName_1;

			// Token: 0x040040DD RID: 16605
			public _get_PropertyAsString get_NativeName_2;

			// Token: 0x040040DE RID: 16606
			public _get_PropertyAsString get_Script_3;

			// Token: 0x040040DF RID: 16607
			private static readonly ILanguage.Vftbl AbiToProjectionVftable = new ILanguage.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				get_LanguageTag_0 = new _get_PropertyAsString(ILanguage.Vftbl.Do_Abi_get_LanguageTag_0),
				get_DisplayName_1 = new _get_PropertyAsString(ILanguage.Vftbl.Do_Abi_get_DisplayName_1),
				get_NativeName_2 = new _get_PropertyAsString(ILanguage.Vftbl.Do_Abi_get_NativeName_2),
				get_Script_3 = new _get_PropertyAsString(ILanguage.Vftbl.Do_Abi_get_Script_3)
			};

			// Token: 0x040040E0 RID: 16608
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
