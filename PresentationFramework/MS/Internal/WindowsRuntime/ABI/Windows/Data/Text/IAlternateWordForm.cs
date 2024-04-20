using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x020002F7 RID: 759
	[ObjectReferenceWrapper("_obj")]
	[Guid("47396C1E-51B9-4207-9146-248E636A1D1D")]
	internal class IAlternateWordForm : IAlternateWordForm
	{
		// Token: 0x06001C8A RID: 7306 RVA: 0x0016BBB0 File Offset: 0x0016ABB0
		public static ObjectReference<IAlternateWordForm.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IAlternateWordForm.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x0016BBB8 File Offset: 0x0016ABB8
		public static implicit operator IAlternateWordForm(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IAlternateWordForm(obj);
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001C8C RID: 7308 RVA: 0x0016BBC5 File Offset: 0x0016ABC5
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001C8D RID: 7309 RVA: 0x0016BBCD File Offset: 0x0016ABCD
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x0016BBDA File Offset: 0x0016ABDA
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x0016BBE7 File Offset: 0x0016ABE7
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x0016BBF4 File Offset: 0x0016ABF4
		public IAlternateWordForm(IObjectReference obj) : this(obj.As<IAlternateWordForm.Vftbl>())
		{
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x0016BC02 File Offset: 0x0016AC02
		public IAlternateWordForm(ObjectReference<IAlternateWordForm.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001C92 RID: 7314 RVA: 0x0016BC14 File Offset: 0x0016AC14
		public string AlternateText
		{
			get
			{
				IntPtr intPtr = 0;
				string result;
				try
				{
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_AlternateText_1(this.ThisPtr, out intPtr));
					result = MarshalString.FromAbi(intPtr);
				}
				finally
				{
					MarshalString.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x0016BC6C File Offset: 0x0016AC6C
		public AlternateNormalizationFormat NormalizationFormat
		{
			get
			{
				AlternateNormalizationFormat result = AlternateNormalizationFormat.NotNormalized;
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_NormalizationFormat_2(this.ThisPtr, out result));
				return result;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001C94 RID: 7316 RVA: 0x0016BCA0 File Offset: 0x0016ACA0
		public TextSegment SourceTextSegment
		{
			get
			{
				TextSegment result = default(TextSegment);
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_SourceTextSegment_0(this.ThisPtr, out result));
				return result;
			}
		}

		// Token: 0x04000E7A RID: 3706
		protected readonly ObjectReference<IAlternateWordForm.Vftbl> _obj;

		// Token: 0x02000A3B RID: 2619
		[Guid("47396C1E-51B9-4207-9146-248E636A1D1D")]
		internal struct Vftbl
		{
			// Token: 0x06008572 RID: 34162 RVA: 0x00329234 File Offset: 0x00328234
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IAlternateWordForm.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 3));
				Marshal.StructureToPtr<IAlternateWordForm.Vftbl>(IAlternateWordForm.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				IAlternateWordForm.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x06008573 RID: 34163 RVA: 0x003292D4 File Offset: 0x003282D4
			private static int Do_Abi_get_AlternateText_1(IntPtr thisPtr, out IntPtr value)
			{
				value = 0;
				try
				{
					string alternateText = ComWrappersSupport.FindObject<IAlternateWordForm>(thisPtr).AlternateText;
					value = MarshalString.FromManaged(alternateText);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008574 RID: 34164 RVA: 0x00329324 File Offset: 0x00328324
			private static int Do_Abi_get_NormalizationFormat_2(IntPtr thisPtr, out AlternateNormalizationFormat value)
			{
				value = AlternateNormalizationFormat.NotNormalized;
				try
				{
					AlternateNormalizationFormat normalizationFormat = ComWrappersSupport.FindObject<IAlternateWordForm>(thisPtr).NormalizationFormat;
					value = normalizationFormat;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008575 RID: 34165 RVA: 0x00329368 File Offset: 0x00328368
			private static int Do_Abi_get_SourceTextSegment_0(IntPtr thisPtr, out TextSegment value)
			{
				TextSegment textSegment = default(TextSegment);
				value = default(TextSegment);
				try
				{
					textSegment = ComWrappersSupport.FindObject<IAlternateWordForm>(thisPtr).SourceTextSegment;
					value = textSegment;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040FC RID: 16636
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040FD RID: 16637
			public IAlternateWordForm_Delegates.get_SourceTextSegment_0 get_SourceTextSegment_0;

			// Token: 0x040040FE RID: 16638
			public _get_PropertyAsString get_AlternateText_1;

			// Token: 0x040040FF RID: 16639
			public IAlternateWordForm_Delegates.get_NormalizationFormat_2 get_NormalizationFormat_2;

			// Token: 0x04004100 RID: 16640
			private static readonly IAlternateWordForm.Vftbl AbiToProjectionVftable = new IAlternateWordForm.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				get_SourceTextSegment_0 = new IAlternateWordForm_Delegates.get_SourceTextSegment_0(IAlternateWordForm.Vftbl.Do_Abi_get_SourceTextSegment_0),
				get_AlternateText_1 = new _get_PropertyAsString(IAlternateWordForm.Vftbl.Do_Abi_get_AlternateText_1),
				get_NormalizationFormat_2 = new IAlternateWordForm_Delegates.get_NormalizationFormat_2(IAlternateWordForm.Vftbl.Do_Abi_get_NormalizationFormat_2)
			};

			// Token: 0x04004101 RID: 16641
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
