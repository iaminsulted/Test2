using System;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x02000700 RID: 1792
	public class BlockElement
	{
		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x06005DCC RID: 24012 RVA: 0x0028DEA8 File Offset: 0x0028CEA8
		internal FixedElement.ElementType ElementType
		{
			get
			{
				return this._elementType;
			}
		}

		// Token: 0x04003169 RID: 12649
		internal FixedElement.ElementType _elementType;
	}
}
