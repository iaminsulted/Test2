using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace MS.Internal.AppModel
{
	// Token: 0x0200028E RID: 654
	internal static class NavigationHelper
	{
		// Token: 0x060018D1 RID: 6353 RVA: 0x001617A0 File Offset: 0x001607A0
		internal static Visual FindRootViewer(ContentControl navigator, string contentPresenterName)
		{
			object content = navigator.Content;
			if (content == null || content is Visual)
			{
				return content as Visual;
			}
			ContentPresenter contentPresenter = null;
			if (navigator.Template != null)
			{
				contentPresenter = (ContentPresenter)navigator.Template.FindName(contentPresenterName, navigator);
			}
			if (contentPresenter == null || contentPresenter.InternalVisualChildrenCount == 0)
			{
				return null;
			}
			return contentPresenter.InternalGetVisualChild(0);
		}
	}
}
