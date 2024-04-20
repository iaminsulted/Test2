using System;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020001E5 RID: 485
	internal sealed class UIElementParagraphResult : FloaterBaseParagraphResult
	{
		// Token: 0x06001115 RID: 4373 RVA: 0x00142659 File Offset: 0x00141659
		internal UIElementParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool HasTextContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0014274D File Offset: 0x0014174D
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			return ((UIElementParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
		}
	}
}
