using System;

namespace MS.Internal.Documents
{
	// Token: 0x020001D2 RID: 466
	internal interface IUndoUnit
	{
		// Token: 0x0600106A RID: 4202
		void Do();

		// Token: 0x0600106B RID: 4203
		bool Merge(IUndoUnit unit);
	}
}
