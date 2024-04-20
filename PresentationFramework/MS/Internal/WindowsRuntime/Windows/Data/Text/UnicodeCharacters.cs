using System;
using MS.Internal.WindowsRuntime.ABI.Windows.Data.Text;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x02000317 RID: 791
	internal static class UnicodeCharacters
	{
		// Token: 0x06001D4F RID: 7503 RVA: 0x0016CCDB File Offset: 0x0016BCDB
		public static uint GetCodepointFromSurrogatePair(uint highSurrogate, uint lowSurrogate)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.GetCodepointFromSurrogatePair(highSurrogate, lowSurrogate);
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x0016CCE9 File Offset: 0x0016BCE9
		public static void GetSurrogatePairFromCodepoint(uint codepoint, out char highSurrogate, out char lowSurrogate)
		{
			UnicodeCharacters._IUnicodeCharactersStatics.Instance.GetSurrogatePairFromCodepoint(codepoint, out highSurrogate, out lowSurrogate);
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x0016CCF8 File Offset: 0x0016BCF8
		public static bool IsHighSurrogate(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsHighSurrogate(codepoint);
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0016CD05 File Offset: 0x0016BD05
		public static bool IsLowSurrogate(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsLowSurrogate(codepoint);
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x0016CD12 File Offset: 0x0016BD12
		public static bool IsSupplementary(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsSupplementary(codepoint);
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x0016CD1F File Offset: 0x0016BD1F
		public static bool IsNoncharacter(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsNoncharacter(codepoint);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x0016CD2C File Offset: 0x0016BD2C
		public static bool IsWhitespace(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsWhitespace(codepoint);
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x0016CD39 File Offset: 0x0016BD39
		public static bool IsAlphabetic(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsAlphabetic(codepoint);
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x0016CD46 File Offset: 0x0016BD46
		public static bool IsCased(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsCased(codepoint);
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x0016CD53 File Offset: 0x0016BD53
		public static bool IsUppercase(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsUppercase(codepoint);
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x0016CD60 File Offset: 0x0016BD60
		public static bool IsLowercase(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsLowercase(codepoint);
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x0016CD6D File Offset: 0x0016BD6D
		public static bool IsIdStart(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsIdStart(codepoint);
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x0016CD7A File Offset: 0x0016BD7A
		public static bool IsIdContinue(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsIdContinue(codepoint);
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x0016CD87 File Offset: 0x0016BD87
		public static bool IsGraphemeBase(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsGraphemeBase(codepoint);
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x0016CD94 File Offset: 0x0016BD94
		public static bool IsGraphemeExtend(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.IsGraphemeExtend(codepoint);
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x0016CDA1 File Offset: 0x0016BDA1
		public static UnicodeNumericType GetNumericType(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.GetNumericType(codepoint);
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x0016CDAE File Offset: 0x0016BDAE
		public static UnicodeGeneralCategory GetGeneralCategory(uint codepoint)
		{
			return UnicodeCharacters._IUnicodeCharactersStatics.Instance.GetGeneralCategory(codepoint);
		}

		// Token: 0x02000A65 RID: 2661
		internal class _IUnicodeCharactersStatics : IUnicodeCharactersStatics
		{
			// Token: 0x06008613 RID: 34323 RVA: 0x0032A0E4 File Offset: 0x003290E4
			public _IUnicodeCharactersStatics() : base(new BaseActivationFactory("Windows.Data.Text", "Windows.Data.Text.UnicodeCharacters")._As<IUnicodeCharactersStatics.Vftbl>())
			{
			}

			// Token: 0x17001DF6 RID: 7670
			// (get) Token: 0x06008614 RID: 34324 RVA: 0x0032A100 File Offset: 0x00329100
			public static IUnicodeCharactersStatics Instance
			{
				get
				{
					return UnicodeCharacters._IUnicodeCharactersStatics._instance.Value;
				}
			}

			// Token: 0x04004136 RID: 16694
			private static WeakLazy<UnicodeCharacters._IUnicodeCharactersStatics> _instance = new WeakLazy<UnicodeCharacters._IUnicodeCharactersStatics>();
		}
	}
}
