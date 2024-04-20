using System;

namespace MS.Internal.Data
{
	// Token: 0x02000227 RID: 551
	internal interface IAsyncDataDispatcher
	{
		// Token: 0x060014B8 RID: 5304
		void AddRequest(AsyncDataRequest request);

		// Token: 0x060014B9 RID: 5305
		void CancelAllRequests();
	}
}
