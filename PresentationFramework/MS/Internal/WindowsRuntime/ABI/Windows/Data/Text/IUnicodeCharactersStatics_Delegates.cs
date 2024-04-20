using System;
using System.ComponentModel;
using MS.Internal.WindowsRuntime.Windows.Data.Text;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x020002FA RID: 762
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static class IUnicodeCharactersStatics_Delegates
	{
		// Token: 0x02000A3F RID: 2623
		// (Invoke) Token: 0x06008591 RID: 34193
		public delegate int GetCodepointFromSurrogatePair_0(IntPtr thisPtr, uint highSurrogate, uint lowSurrogate, out uint codepoint);

		// Token: 0x02000A40 RID: 2624
		// (Invoke) Token: 0x06008595 RID: 34197
		public delegate int GetSurrogatePairFromCodepoint_1(IntPtr thisPtr, uint codepoint, out ushort highSurrogate, out ushort lowSurrogate);

		// Token: 0x02000A41 RID: 2625
		// (Invoke) Token: 0x06008599 RID: 34201
		public delegate int IsHighSurrogate_2(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A42 RID: 2626
		// (Invoke) Token: 0x0600859D RID: 34205
		public delegate int IsLowSurrogate_3(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A43 RID: 2627
		// (Invoke) Token: 0x060085A1 RID: 34209
		public delegate int IsSupplementary_4(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A44 RID: 2628
		// (Invoke) Token: 0x060085A5 RID: 34213
		public delegate int IsNoncharacter_5(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A45 RID: 2629
		// (Invoke) Token: 0x060085A9 RID: 34217
		public delegate int IsWhitespace_6(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A46 RID: 2630
		// (Invoke) Token: 0x060085AD RID: 34221
		public delegate int IsAlphabetic_7(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A47 RID: 2631
		// (Invoke) Token: 0x060085B1 RID: 34225
		public delegate int IsCased_8(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A48 RID: 2632
		// (Invoke) Token: 0x060085B5 RID: 34229
		public delegate int IsUppercase_9(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A49 RID: 2633
		// (Invoke) Token: 0x060085B9 RID: 34233
		public delegate int IsLowercase_10(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A4A RID: 2634
		// (Invoke) Token: 0x060085BD RID: 34237
		public delegate int IsIdStart_11(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A4B RID: 2635
		// (Invoke) Token: 0x060085C1 RID: 34241
		public delegate int IsIdContinue_12(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A4C RID: 2636
		// (Invoke) Token: 0x060085C5 RID: 34245
		public delegate int IsGraphemeBase_13(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A4D RID: 2637
		// (Invoke) Token: 0x060085C9 RID: 34249
		public delegate int IsGraphemeExtend_14(IntPtr thisPtr, uint codepoint, out byte value);

		// Token: 0x02000A4E RID: 2638
		// (Invoke) Token: 0x060085CD RID: 34253
		public delegate int GetNumericType_15(IntPtr thisPtr, uint codepoint, out UnicodeNumericType value);

		// Token: 0x02000A4F RID: 2639
		// (Invoke) Token: 0x060085D1 RID: 34257
		public delegate int GetGeneralCategory_16(IntPtr thisPtr, uint codepoint, out UnicodeGeneralCategory value);
	}
}
