using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200027B RID: 635
	internal interface INavigatorBase
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001837 RID: 6199
		// (set) Token: 0x06001838 RID: 6200
		Uri Source { get; set; }

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001839 RID: 6201
		Uri CurrentSource { get; }

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600183A RID: 6202
		// (set) Token: 0x0600183B RID: 6203
		object Content { get; set; }

		// Token: 0x0600183C RID: 6204
		bool Navigate(Uri source);

		// Token: 0x0600183D RID: 6205
		bool Navigate(Uri source, object extraData);

		// Token: 0x0600183E RID: 6206
		bool Navigate(object content);

		// Token: 0x0600183F RID: 6207
		bool Navigate(object content, object extraData);

		// Token: 0x06001840 RID: 6208
		void StopLoading();

		// Token: 0x06001841 RID: 6209
		void Refresh();

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06001842 RID: 6210
		// (remove) Token: 0x06001843 RID: 6211
		event NavigatingCancelEventHandler Navigating;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06001844 RID: 6212
		// (remove) Token: 0x06001845 RID: 6213
		event NavigationProgressEventHandler NavigationProgress;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06001846 RID: 6214
		// (remove) Token: 0x06001847 RID: 6215
		event NavigationFailedEventHandler NavigationFailed;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06001848 RID: 6216
		// (remove) Token: 0x06001849 RID: 6217
		event NavigatedEventHandler Navigated;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x0600184A RID: 6218
		// (remove) Token: 0x0600184B RID: 6219
		event LoadCompletedEventHandler LoadCompleted;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x0600184C RID: 6220
		// (remove) Token: 0x0600184D RID: 6221
		event NavigationStoppedEventHandler NavigationStopped;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x0600184E RID: 6222
		// (remove) Token: 0x0600184F RID: 6223
		event FragmentNavigationEventHandler FragmentNavigation;
	}
}
