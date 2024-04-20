using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x02000711 RID: 1809
	internal interface ITextBoxViewHost
	{
		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x06005E9E RID: 24222
		ITextContainer TextContainer { get; }

		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x06005E9F RID: 24223
		bool IsTypographyDefaultValue { get; }
	}
}
