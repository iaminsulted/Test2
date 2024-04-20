using System;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000439 RID: 1081
	public sealed class ResumeStoryboard : ControllableStoryboardAction
	{
		// Token: 0x0600348B RID: 13451 RVA: 0x001DB8A2 File Offset: 0x001DA8A2
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Resume(containingFE);
				return;
			}
			storyboard.Resume(containingFCE);
		}
	}
}
