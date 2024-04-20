using System;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000437 RID: 1079
	public sealed class PauseStoryboard : ControllableStoryboardAction
	{
		// Token: 0x06003487 RID: 13447 RVA: 0x001DB872 File Offset: 0x001DA872
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Pause(containingFE);
				return;
			}
			storyboard.Pause(containingFCE);
		}
	}
}
