using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.ABI.System.Collections.Generic;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x020002FB RID: 763
	[Guid("D2D4BA6D-987C-4CC0-B6BD-D49A11B38F9A")]
	[ObjectReferenceWrapper("_obj")]
	internal class IWordSegment : IWordSegment
	{
		// Token: 0x06001CAE RID: 7342 RVA: 0x0016C0EB File Offset: 0x0016B0EB
		public static ObjectReference<IWordSegment.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IWordSegment.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x0016C0F3 File Offset: 0x0016B0F3
		public static implicit operator IWordSegment(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IWordSegment(obj);
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x0016C100 File Offset: 0x0016B100
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x0016C108 File Offset: 0x0016B108
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x0016C115 File Offset: 0x0016B115
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x0016C122 File Offset: 0x0016B122
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x0016C12F File Offset: 0x0016B12F
		public IWordSegment(IObjectReference obj) : this(obj.As<IWordSegment.Vftbl>())
		{
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0016C13D File Offset: 0x0016B13D
		public IWordSegment(ObjectReference<IWordSegment.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001CB6 RID: 7350 RVA: 0x0016C14C File Offset: 0x0016B14C
		public IReadOnlyList<AlternateWordForm> AlternateForms
		{
			get
			{
				IntPtr intPtr = 0;
				IReadOnlyList<AlternateWordForm> result;
				try
				{
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_AlternateForms_2(this.ThisPtr, out intPtr));
					result = IReadOnlyList<AlternateWordForm>.FromAbi(intPtr);
				}
				finally
				{
					IReadOnlyList<AlternateWordForm>.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x0016C1A4 File Offset: 0x0016B1A4
		public TextSegment SourceTextSegment
		{
			get
			{
				TextSegment result = default(TextSegment);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_SourceTextSegment_1(this.ThisPtr, out result));
				return result;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x0016C1DC File Offset: 0x0016B1DC
		public string Text
		{
			get
			{
				IntPtr intPtr = 0;
				string result;
				try
				{
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_Text_0(this.ThisPtr, out intPtr));
					result = MarshalString.FromAbi(intPtr);
				}
				finally
				{
					MarshalString.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x04000E7C RID: 3708
		protected readonly ObjectReference<IWordSegment.Vftbl> _obj;

		// Token: 0x02000A50 RID: 2640
		[Guid("D2D4BA6D-987C-4CC0-B6BD-D49A11B38F9A")]
		internal struct Vftbl
		{
			// Token: 0x060085D4 RID: 34260 RVA: 0x00329A6C File Offset: 0x00328A6C
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IWordSegment.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 3));
				Marshal.StructureToPtr<IWordSegment.Vftbl>(IWordSegment.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				IWordSegment.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x060085D5 RID: 34261 RVA: 0x00329B0C File Offset: 0x00328B0C
			private static int Do_Abi_get_AlternateForms_2(IntPtr thisPtr, out IntPtr value)
			{
				value = 0;
				try
				{
					IReadOnlyList<AlternateWordForm> alternateForms = ComWrappersSupport.FindObject<IWordSegment>(thisPtr).AlternateForms;
					value = IReadOnlyList<AlternateWordForm>.FromManaged(alternateForms);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060085D6 RID: 34262 RVA: 0x00329B5C File Offset: 0x00328B5C
			private static int Do_Abi_get_SourceTextSegment_1(IntPtr thisPtr, out TextSegment value)
			{
				TextSegment textSegment = default(TextSegment);
				value = default(TextSegment);
				try
				{
					textSegment = ComWrappersSupport.FindObject<IWordSegment>(thisPtr).SourceTextSegment;
					value = textSegment;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060085D7 RID: 34263 RVA: 0x00329BB0 File Offset: 0x00328BB0
			private static int Do_Abi_get_Text_0(IntPtr thisPtr, out IntPtr value)
			{
				value = 0;
				try
				{
					string text = ComWrappersSupport.FindObject<IWordSegment>(thisPtr).Text;
					value = MarshalString.FromManaged(text);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04004116 RID: 16662
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04004117 RID: 16663
			public _get_PropertyAsString get_Text_0;

			// Token: 0x04004118 RID: 16664
			public IWordSegment_Delegates.get_SourceTextSegment_1 get_SourceTextSegment_1;

			// Token: 0x04004119 RID: 16665
			public _get_PropertyAsObject get_AlternateForms_2;

			// Token: 0x0400411A RID: 16666
			private static readonly IWordSegment.Vftbl AbiToProjectionVftable = new IWordSegment.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				get_Text_0 = new _get_PropertyAsString(IWordSegment.Vftbl.Do_Abi_get_Text_0),
				get_SourceTextSegment_1 = new IWordSegment_Delegates.get_SourceTextSegment_1(IWordSegment.Vftbl.Do_Abi_get_SourceTextSegment_1),
				get_AlternateForms_2 = new _get_PropertyAsObject(IWordSegment.Vftbl.Do_Abi_get_AlternateForms_2)
			};

			// Token: 0x0400411B RID: 16667
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
