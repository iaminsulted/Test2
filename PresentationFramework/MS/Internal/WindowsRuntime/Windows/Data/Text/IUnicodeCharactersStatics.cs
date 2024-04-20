using System;
using System.Runtime.InteropServices;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x02000312 RID: 786
	[WindowsRuntimeType]
	[Guid("97909E87-9291-4F91-B6C8-B6E359D7A7FB")]
	internal interface IUnicodeCharactersStatics
	{
		// Token: 0x06001D30 RID: 7472
		uint GetCodepointFromSurrogatePair(uint highSurrogate, uint lowSurrogate);

		// Token: 0x06001D31 RID: 7473
		void GetSurrogatePairFromCodepoint(uint codepoint, out char highSurrogate, out char lowSurrogate);

		// Token: 0x06001D32 RID: 7474
		bool IsHighSurrogate(uint codepoint);

		// Token: 0x06001D33 RID: 7475
		bool IsLowSurrogate(uint codepoint);

		// Token: 0x06001D34 RID: 7476
		bool IsSupplementary(uint codepoint);

		// Token: 0x06001D35 RID: 7477
		bool IsNoncharacter(uint codepoint);

		// Token: 0x06001D36 RID: 7478
		bool IsWhitespace(uint codepoint);

		// Token: 0x06001D37 RID: 7479
		bool IsAlphabetic(uint codepoint);

		// Token: 0x06001D38 RID: 7480
		bool IsCased(uint codepoint);

		// Token: 0x06001D39 RID: 7481
		bool IsUppercase(uint codepoint);

		// Token: 0x06001D3A RID: 7482
		bool IsLowercase(uint codepoint);

		// Token: 0x06001D3B RID: 7483
		bool IsIdStart(uint codepoint);

		// Token: 0x06001D3C RID: 7484
		bool IsIdContinue(uint codepoint);

		// Token: 0x06001D3D RID: 7485
		bool IsGraphemeBase(uint codepoint);

		// Token: 0x06001D3E RID: 7486
		bool IsGraphemeExtend(uint codepoint);

		// Token: 0x06001D3F RID: 7487
		UnicodeNumericType GetNumericType(uint codepoint);

		// Token: 0x06001D40 RID: 7488
		UnicodeGeneralCategory GetGeneralCategory(uint codepoint);
	}
}
