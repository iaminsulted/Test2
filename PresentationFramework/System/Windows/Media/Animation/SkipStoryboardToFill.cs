using System;

namespace System.Windows.Media.Animation
{
	// Token: 0x0200043C RID: 1084
	public sealed class SkipStoryboardToFill : ControllableStoryboardAction
	{
		// Token: 0x06003498 RID: 13464 RVA: 0x001DBA0A File Offset: 0x001DAA0A
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.SkipToFill(containingFE);
				return;
			}
			storyboard.SkipToFill(containingFCE);
		}
	}
}
