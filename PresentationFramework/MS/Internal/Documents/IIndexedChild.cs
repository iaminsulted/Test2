using System;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001D0 RID: 464
	internal interface IIndexedChild<TParent> where TParent : TextElement
	{
		// Token: 0x06001057 RID: 4183
		void OnEnterParentTree();

		// Token: 0x06001058 RID: 4184
		void OnExitParentTree();

		// Token: 0x06001059 RID: 4185
		void OnAfterExitParentTree(TParent parent);

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x0600105A RID: 4186
		// (set) Token: 0x0600105B RID: 4187
		int Index { get; set; }
	}
}
