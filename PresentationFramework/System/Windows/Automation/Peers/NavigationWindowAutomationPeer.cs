using System;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000581 RID: 1409
	public class NavigationWindowAutomationPeer : WindowAutomationPeer
	{
		// Token: 0x06004511 RID: 17681 RVA: 0x00222F87 File Offset: 0x00221F87
		public NavigationWindowAutomationPeer(NavigationWindow owner) : base(owner)
		{
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x00222F90 File Offset: 0x00221F90
		protected override string GetClassNameCore()
		{
			return "NavigationWindow";
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x00222F98 File Offset: 0x00221F98
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void RaiseAsyncContentLoadedEvent(AutomationPeer peer, long bytesRead, long maxBytes)
		{
			double percentComplete = 0.0;
			AsyncContentLoadedState asyncContentState = AsyncContentLoadedState.Beginning;
			if (bytesRead > 0L)
			{
				if (bytesRead < maxBytes)
				{
					percentComplete = ((maxBytes > 0L) ? ((double)bytesRead * 100.0 / (double)maxBytes) : 0.0);
					asyncContentState = AsyncContentLoadedState.Progress;
				}
				else
				{
					percentComplete = 100.0;
					asyncContentState = AsyncContentLoadedState.Completed;
				}
			}
			peer.RaiseAsyncContentLoadedEvent(new AsyncContentLoadedEventArgs(asyncContentState, percentComplete));
		}
	}
}
