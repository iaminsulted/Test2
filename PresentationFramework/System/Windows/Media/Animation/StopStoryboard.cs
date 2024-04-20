using System;

namespace System.Windows.Media.Animation
{
	// Token: 0x0200043D RID: 1085
	public sealed class StopStoryboard : ControllableStoryboardAction
	{
		// Token: 0x0600349A RID: 13466 RVA: 0x001DBA1E File Offset: 0x001DAA1E
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Stop(containingFE);
				return;
			}
			storyboard.Stop(containingFCE);
		}
	}
}
