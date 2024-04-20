using System;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001CB RID: 459
	internal interface IFlowDocumentFormatter
	{
		// Token: 0x06000FF1 RID: 4081
		void OnContentInvalidated(bool affectsLayout);

		// Token: 0x06000FF2 RID: 4082
		void OnContentInvalidated(bool affectsLayout, ITextPointer start, ITextPointer end);

		// Token: 0x06000FF3 RID: 4083
		void Suspend();

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000FF4 RID: 4084
		bool IsLayoutDataValid { get; }
	}
}
