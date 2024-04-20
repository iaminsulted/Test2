using System;
using System.Collections;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200027C RID: 636
	internal interface INavigator : INavigatorBase
	{
		// Token: 0x06001850 RID: 6224
		JournalNavigationScope GetJournal(bool create);

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001851 RID: 6225
		bool CanGoForward { get; }

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001852 RID: 6226
		bool CanGoBack { get; }

		// Token: 0x06001853 RID: 6227
		void GoForward();

		// Token: 0x06001854 RID: 6228
		void GoBack();

		// Token: 0x06001855 RID: 6229
		void AddBackEntry(CustomContentState state);

		// Token: 0x06001856 RID: 6230
		JournalEntry RemoveBackEntry();

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001857 RID: 6231
		IEnumerable BackStack { get; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001858 RID: 6232
		IEnumerable ForwardStack { get; }
	}
}
