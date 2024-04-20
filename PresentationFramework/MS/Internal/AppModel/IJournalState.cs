using System;

namespace MS.Internal.AppModel
{
	// Token: 0x02000281 RID: 641
	internal interface IJournalState
	{
		// Token: 0x0600185D RID: 6237
		CustomJournalStateInternal GetJournalState(JournalReason journalReason);

		// Token: 0x0600185E RID: 6238
		void RestoreJournalState(CustomJournalStateInternal state);
	}
}
