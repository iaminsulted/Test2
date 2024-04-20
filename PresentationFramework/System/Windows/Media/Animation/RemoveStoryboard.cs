using System;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000438 RID: 1080
	public sealed class RemoveStoryboard : ControllableStoryboardAction
	{
		// Token: 0x06003489 RID: 13449 RVA: 0x001DB88E File Offset: 0x001DA88E
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Remove(containingFE);
				return;
			}
			storyboard.Remove(containingFCE);
		}
	}
}
