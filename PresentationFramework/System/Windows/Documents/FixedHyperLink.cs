using System;
using System.Windows.Navigation;

namespace System.Windows.Documents
{
	// Token: 0x020005FA RID: 1530
	internal static class FixedHyperLink
	{
		// Token: 0x06004A9B RID: 19099 RVA: 0x002342B0 File Offset: 0x002332B0
		public static void OnNavigationServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is FixedDocument)
			{
				NavigationService navigationService = (NavigationService)e.OldValue;
				NavigationService navigationService2 = (NavigationService)e.NewValue;
				if (navigationService != null)
				{
					navigationService.FragmentNavigation -= FixedHyperLink.FragmentHandler;
				}
				if (navigationService2 != null)
				{
					navigationService2.FragmentNavigation += FixedHyperLink.FragmentHandler;
				}
			}
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x0023430C File Offset: 0x0023330C
		internal static void FragmentHandler(object sender, FragmentNavigationEventArgs e)
		{
			NavigationService navigationService = sender as NavigationService;
			if (navigationService != null)
			{
				string fragment = e.Fragment;
				IFixedNavigate fixedNavigate = navigationService.Content as IFixedNavigate;
				if (fixedNavigate != null)
				{
					fixedNavigate.NavigateAsync(e.Fragment);
					e.Handled = true;
				}
			}
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x0023434C File Offset: 0x0023334C
		internal static void NavigateToElement(object ElementHost, string elementID)
		{
			FixedPage fixedPage = null;
			FrameworkElement frameworkElement = ((IFixedNavigate)ElementHost).FindElementByID(elementID, out fixedPage) as FrameworkElement;
			if (frameworkElement != null)
			{
				if (frameworkElement is FixedPage)
				{
					frameworkElement.BringIntoView();
					return;
				}
				frameworkElement.BringIntoView(frameworkElement.VisualContentBounds);
			}
		}
	}
}
