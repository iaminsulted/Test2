using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000144 RID: 324
	internal interface ISegment
	{
		// Token: 0x060009C8 RID: 2504
		void GetFirstPara(out int successful, out IntPtr firstParaName);

		// Token: 0x060009C9 RID: 2505
		void GetNextPara(BaseParagraph currentParagraph, out int found, out IntPtr nextParaName);

		// Token: 0x060009CA RID: 2506
		void UpdGetFirstChangeInSegment(out int fFound, out int fChangeFirst, out IntPtr nmpBeforeChange);
	}
}
