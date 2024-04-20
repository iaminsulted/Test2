using System;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200011F RID: 287
	internal abstract class FloaterBaseParaClient : BaseParaClient
	{
		// Token: 0x0600076D RID: 1901 RVA: 0x0010A829 File Offset: 0x00109829
		protected FloaterBaseParaClient(FloaterBaseParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void ArrangeFloater(PTS.FSRECT rcFloater, PTS.FSRECT rcHostPara, uint fswdirParent, PageContext pageContext)
		{
		}

		// Token: 0x0600076F RID: 1903
		internal abstract override TextContentRange GetTextContentRange();
	}
}
