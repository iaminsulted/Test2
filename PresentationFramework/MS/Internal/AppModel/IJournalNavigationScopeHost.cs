using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200028B RID: 651
	internal interface IJournalNavigationScopeHost : INavigatorBase
	{
		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060018C5 RID: 6341
		NavigationService NavigationService { get; }

		// Token: 0x060018C6 RID: 6342
		void VerifyContextAndObjectState();

		// Token: 0x060018C7 RID: 6343
		void OnJournalAvailable();

		// Token: 0x060018C8 RID: 6344
		bool GoBackOverride();

		// Token: 0x060018C9 RID: 6345
		bool GoForwardOverride();
	}
}
