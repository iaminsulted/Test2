using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x020002F9 RID: 761
	[ObjectReferenceWrapper("_obj")]
	[Guid("97909E87-9291-4F91-B6C8-B6E359D7A7FB")]
	internal class IUnicodeCharactersStatics : IUnicodeCharactersStatics
	{
		// Token: 0x06001C95 RID: 7317 RVA: 0x0016BCD8 File Offset: 0x0016ACD8
		public static ObjectReference<IUnicodeCharactersStatics.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IUnicodeCharactersStatics.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x0016BCE0 File Offset: 0x0016ACE0
		public static implicit operator IUnicodeCharactersStatics(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IUnicodeCharactersStatics(obj);
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x0016BCED File Offset: 0x0016ACED
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001C98 RID: 7320 RVA: 0x0016BCF5 File Offset: 0x0016ACF5
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x0016BD02 File Offset: 0x0016AD02
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x0016BD0F File Offset: 0x0016AD0F
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x0016BD1C File Offset: 0x0016AD1C
		public IUnicodeCharactersStatics(IObjectReference obj) : this(obj.As<IUnicodeCharactersStatics.Vftbl>())
		{
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x0016BD2A File Offset: 0x0016AD2A
		public IUnicodeCharactersStatics(ObjectReference<IUnicodeCharactersStatics.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x0016BD3C File Offset: 0x0016AD3C
		public uint GetCodepointFromSurrogatePair(uint highSurrogate, uint lowSurrogate)
		{
			uint result = 0U;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetCodepointFromSurrogatePair_0(this.ThisPtr, highSurrogate, lowSurrogate, out result));
			return result;
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x0016BD70 File Offset: 0x0016AD70
		public void GetSurrogatePairFromCodepoint(uint codepoint, out char highSurrogate, out char lowSurrogate)
		{
			ushort num = 0;
			ushort num2 = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetSurrogatePairFromCodepoint_1(this.ThisPtr, codepoint, out num, out num2));
			highSurrogate = (char)num;
			lowSurrogate = (char)num2;
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x0016BDAC File Offset: 0x0016ADAC
		public bool IsHighSurrogate(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsHighSurrogate_2(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0016BDE4 File Offset: 0x0016ADE4
		public bool IsLowSurrogate(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsLowSurrogate_3(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0016BE1C File Offset: 0x0016AE1C
		public bool IsSupplementary(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsSupplementary_4(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x0016BE54 File Offset: 0x0016AE54
		public bool IsNoncharacter(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsNoncharacter_5(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0016BE8C File Offset: 0x0016AE8C
		public bool IsWhitespace(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsWhitespace_6(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0016BEC4 File Offset: 0x0016AEC4
		public bool IsAlphabetic(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsAlphabetic_7(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0016BEFC File Offset: 0x0016AEFC
		public bool IsCased(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsCased_8(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x0016BF34 File Offset: 0x0016AF34
		public bool IsUppercase(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsUppercase_9(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0016BF6C File Offset: 0x0016AF6C
		public bool IsLowercase(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsLowercase_10(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0016BFA4 File Offset: 0x0016AFA4
		public bool IsIdStart(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsIdStart_11(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x0016BFDC File Offset: 0x0016AFDC
		public bool IsIdContinue(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsIdContinue_12(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x0016C014 File Offset: 0x0016B014
		public bool IsGraphemeBase(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsGraphemeBase_13(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x0016C04C File Offset: 0x0016B04C
		public bool IsGraphemeExtend(uint codepoint)
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.IsGraphemeExtend_14(this.ThisPtr, codepoint, out b));
			return b > 0;
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x0016C084 File Offset: 0x0016B084
		public UnicodeNumericType GetNumericType(uint codepoint)
		{
			UnicodeNumericType result = UnicodeNumericType.None;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetNumericType_15(this.ThisPtr, codepoint, out result));
			return result;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x0016C0B8 File Offset: 0x0016B0B8
		public UnicodeGeneralCategory GetGeneralCategory(uint codepoint)
		{
			UnicodeGeneralCategory result = UnicodeGeneralCategory.UppercaseLetter;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetGeneralCategory_16(this.ThisPtr, codepoint, out result));
			return result;
		}

		// Token: 0x04000E7B RID: 3707
		protected readonly ObjectReference<IUnicodeCharactersStatics.Vftbl> _obj;

		// Token: 0x02000A3E RID: 2622
		[Guid("97909E87-9291-4F91-B6C8-B6E359D7A7FB")]
		internal struct Vftbl
		{
			// Token: 0x0600857E RID: 34174 RVA: 0x003293BC File Offset: 0x003283BC
			unsafe static Vftbl()
			{
				IntPtr* value = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IUnicodeCharactersStatics.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 17));
				Marshal.StructureToPtr<IUnicodeCharactersStatics.Vftbl>(IUnicodeCharactersStatics.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)value), false);
				IUnicodeCharactersStatics.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)value);
			}

			// Token: 0x0600857F RID: 34175 RVA: 0x00329568 File Offset: 0x00328568
			private static int Do_Abi_GetCodepointFromSurrogatePair_0(IntPtr thisPtr, uint highSurrogate, uint lowSurrogate, out uint codepoint)
			{
				codepoint = 0U;
				try
				{
					uint codepointFromSurrogatePair = ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).GetCodepointFromSurrogatePair(highSurrogate, lowSurrogate);
					codepoint = codepointFromSurrogatePair;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008580 RID: 34176 RVA: 0x003295B0 File Offset: 0x003285B0
			private static int Do_Abi_GetSurrogatePairFromCodepoint_1(IntPtr thisPtr, uint codepoint, out ushort highSurrogate, out ushort lowSurrogate)
			{
				highSurrogate = 0;
				lowSurrogate = 0;
				char c = '\0';
				char c2 = '\0';
				try
				{
					ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).GetSurrogatePairFromCodepoint(codepoint, out c, out c2);
					highSurrogate = (ushort)c;
					lowSurrogate = (ushort)c2;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008581 RID: 34177 RVA: 0x00329600 File Offset: 0x00328600
			private static int Do_Abi_IsHighSurrogate_2(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsHighSurrogate(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008582 RID: 34178 RVA: 0x0032964C File Offset: 0x0032864C
			private static int Do_Abi_IsLowSurrogate_3(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsLowSurrogate(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008583 RID: 34179 RVA: 0x00329698 File Offset: 0x00328698
			private static int Do_Abi_IsSupplementary_4(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsSupplementary(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008584 RID: 34180 RVA: 0x003296E4 File Offset: 0x003286E4
			private static int Do_Abi_IsNoncharacter_5(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsNoncharacter(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008585 RID: 34181 RVA: 0x00329730 File Offset: 0x00328730
			private static int Do_Abi_IsWhitespace_6(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsWhitespace(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008586 RID: 34182 RVA: 0x0032977C File Offset: 0x0032877C
			private static int Do_Abi_IsAlphabetic_7(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsAlphabetic(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008587 RID: 34183 RVA: 0x003297C8 File Offset: 0x003287C8
			private static int Do_Abi_IsCased_8(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsCased(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008588 RID: 34184 RVA: 0x00329814 File Offset: 0x00328814
			private static int Do_Abi_IsUppercase_9(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsUppercase(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008589 RID: 34185 RVA: 0x00329860 File Offset: 0x00328860
			private static int Do_Abi_IsLowercase_10(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsLowercase(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600858A RID: 34186 RVA: 0x003298AC File Offset: 0x003288AC
			private static int Do_Abi_IsIdStart_11(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsIdStart(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600858B RID: 34187 RVA: 0x003298F8 File Offset: 0x003288F8
			private static int Do_Abi_IsIdContinue_12(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsIdContinue(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600858C RID: 34188 RVA: 0x00329944 File Offset: 0x00328944
			private static int Do_Abi_IsGraphemeBase_13(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsGraphemeBase(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600858D RID: 34189 RVA: 0x00329990 File Offset: 0x00328990
			private static int Do_Abi_IsGraphemeExtend_14(IntPtr thisPtr, uint codepoint, out byte value)
			{
				value = 0;
				try
				{
					value = (ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).IsGraphemeExtend(codepoint) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600858E RID: 34190 RVA: 0x003299DC File Offset: 0x003289DC
			private static int Do_Abi_GetNumericType_15(IntPtr thisPtr, uint codepoint, out UnicodeNumericType value)
			{
				value = UnicodeNumericType.None;
				try
				{
					UnicodeNumericType numericType = ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).GetNumericType(codepoint);
					value = numericType;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600858F RID: 34191 RVA: 0x00329A24 File Offset: 0x00328A24
			private static int Do_Abi_GetGeneralCategory_16(IntPtr thisPtr, uint codepoint, out UnicodeGeneralCategory value)
			{
				value = UnicodeGeneralCategory.UppercaseLetter;
				try
				{
					UnicodeGeneralCategory generalCategory = ComWrappersSupport.FindObject<IUnicodeCharactersStatics>(thisPtr).GetGeneralCategory(codepoint);
					value = generalCategory;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04004102 RID: 16642
			public IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04004103 RID: 16643
			public IUnicodeCharactersStatics_Delegates.GetCodepointFromSurrogatePair_0 GetCodepointFromSurrogatePair_0;

			// Token: 0x04004104 RID: 16644
			public IUnicodeCharactersStatics_Delegates.GetSurrogatePairFromCodepoint_1 GetSurrogatePairFromCodepoint_1;

			// Token: 0x04004105 RID: 16645
			public IUnicodeCharactersStatics_Delegates.IsHighSurrogate_2 IsHighSurrogate_2;

			// Token: 0x04004106 RID: 16646
			public IUnicodeCharactersStatics_Delegates.IsLowSurrogate_3 IsLowSurrogate_3;

			// Token: 0x04004107 RID: 16647
			public IUnicodeCharactersStatics_Delegates.IsSupplementary_4 IsSupplementary_4;

			// Token: 0x04004108 RID: 16648
			public IUnicodeCharactersStatics_Delegates.IsNoncharacter_5 IsNoncharacter_5;

			// Token: 0x04004109 RID: 16649
			public IUnicodeCharactersStatics_Delegates.IsWhitespace_6 IsWhitespace_6;

			// Token: 0x0400410A RID: 16650
			public IUnicodeCharactersStatics_Delegates.IsAlphabetic_7 IsAlphabetic_7;

			// Token: 0x0400410B RID: 16651
			public IUnicodeCharactersStatics_Delegates.IsCased_8 IsCased_8;

			// Token: 0x0400410C RID: 16652
			public IUnicodeCharactersStatics_Delegates.IsUppercase_9 IsUppercase_9;

			// Token: 0x0400410D RID: 16653
			public IUnicodeCharactersStatics_Delegates.IsLowercase_10 IsLowercase_10;

			// Token: 0x0400410E RID: 16654
			public IUnicodeCharactersStatics_Delegates.IsIdStart_11 IsIdStart_11;

			// Token: 0x0400410F RID: 16655
			public IUnicodeCharactersStatics_Delegates.IsIdContinue_12 IsIdContinue_12;

			// Token: 0x04004110 RID: 16656
			public IUnicodeCharactersStatics_Delegates.IsGraphemeBase_13 IsGraphemeBase_13;

			// Token: 0x04004111 RID: 16657
			public IUnicodeCharactersStatics_Delegates.IsGraphemeExtend_14 IsGraphemeExtend_14;

			// Token: 0x04004112 RID: 16658
			public IUnicodeCharactersStatics_Delegates.GetNumericType_15 GetNumericType_15;

			// Token: 0x04004113 RID: 16659
			public IUnicodeCharactersStatics_Delegates.GetGeneralCategory_16 GetGeneralCategory_16;

			// Token: 0x04004114 RID: 16660
			private static readonly IUnicodeCharactersStatics.Vftbl AbiToProjectionVftable = new IUnicodeCharactersStatics.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				GetCodepointFromSurrogatePair_0 = new IUnicodeCharactersStatics_Delegates.GetCodepointFromSurrogatePair_0(IUnicodeCharactersStatics.Vftbl.Do_Abi_GetCodepointFromSurrogatePair_0),
				GetSurrogatePairFromCodepoint_1 = new IUnicodeCharactersStatics_Delegates.GetSurrogatePairFromCodepoint_1(IUnicodeCharactersStatics.Vftbl.Do_Abi_GetSurrogatePairFromCodepoint_1),
				IsHighSurrogate_2 = new IUnicodeCharactersStatics_Delegates.IsHighSurrogate_2(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsHighSurrogate_2),
				IsLowSurrogate_3 = new IUnicodeCharactersStatics_Delegates.IsLowSurrogate_3(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsLowSurrogate_3),
				IsSupplementary_4 = new IUnicodeCharactersStatics_Delegates.IsSupplementary_4(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsSupplementary_4),
				IsNoncharacter_5 = new IUnicodeCharactersStatics_Delegates.IsNoncharacter_5(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsNoncharacter_5),
				IsWhitespace_6 = new IUnicodeCharactersStatics_Delegates.IsWhitespace_6(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsWhitespace_6),
				IsAlphabetic_7 = new IUnicodeCharactersStatics_Delegates.IsAlphabetic_7(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsAlphabetic_7),
				IsCased_8 = new IUnicodeCharactersStatics_Delegates.IsCased_8(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsCased_8),
				IsUppercase_9 = new IUnicodeCharactersStatics_Delegates.IsUppercase_9(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsUppercase_9),
				IsLowercase_10 = new IUnicodeCharactersStatics_Delegates.IsLowercase_10(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsLowercase_10),
				IsIdStart_11 = new IUnicodeCharactersStatics_Delegates.IsIdStart_11(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsIdStart_11),
				IsIdContinue_12 = new IUnicodeCharactersStatics_Delegates.IsIdContinue_12(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsIdContinue_12),
				IsGraphemeBase_13 = new IUnicodeCharactersStatics_Delegates.IsGraphemeBase_13(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsGraphemeBase_13),
				IsGraphemeExtend_14 = new IUnicodeCharactersStatics_Delegates.IsGraphemeExtend_14(IUnicodeCharactersStatics.Vftbl.Do_Abi_IsGraphemeExtend_14),
				GetNumericType_15 = new IUnicodeCharactersStatics_Delegates.GetNumericType_15(IUnicodeCharactersStatics.Vftbl.Do_Abi_GetNumericType_15),
				GetGeneralCategory_16 = new IUnicodeCharactersStatics_Delegates.GetGeneralCategory_16(IUnicodeCharactersStatics.Vftbl.Do_Abi_GetGeneralCategory_16)
			};

			// Token: 0x04004115 RID: 16661
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
