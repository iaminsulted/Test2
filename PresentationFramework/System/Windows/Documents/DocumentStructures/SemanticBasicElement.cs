using System;
using System.Collections.Generic;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x02000703 RID: 1795
	public class SemanticBasicElement : BlockElement
	{
		// Token: 0x06005DD2 RID: 24018 RVA: 0x0028DEC9 File Offset: 0x0028CEC9
		internal SemanticBasicElement()
		{
			this._elementList = new List<BlockElement>();
		}

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x06005DD3 RID: 24019 RVA: 0x0028DEDC File Offset: 0x0028CEDC
		internal List<BlockElement> BlockElementList
		{
			get
			{
				return this._elementList;
			}
		}

		// Token: 0x0400316B RID: 12651
		internal List<BlockElement> _elementList;
	}
}
