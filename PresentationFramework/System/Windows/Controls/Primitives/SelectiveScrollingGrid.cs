using System;
using System.Windows.Data;
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000854 RID: 2132
	public class SelectiveScrollingGrid : Grid
	{
		// Token: 0x06007D24 RID: 32036 RVA: 0x00312311 File Offset: 0x00311311
		public static SelectiveScrollingOrientation GetSelectiveScrollingOrientation(DependencyObject obj)
		{
			return (SelectiveScrollingOrientation)obj.GetValue(SelectiveScrollingGrid.SelectiveScrollingOrientationProperty);
		}

		// Token: 0x06007D25 RID: 32037 RVA: 0x00312323 File Offset: 0x00311323
		public static void SetSelectiveScrollingOrientation(DependencyObject obj, SelectiveScrollingOrientation value)
		{
			obj.SetValue(SelectiveScrollingGrid.SelectiveScrollingOrientationProperty, value);
		}

		// Token: 0x06007D26 RID: 32038 RVA: 0x00312338 File Offset: 0x00311338
		private static void OnSelectiveScrollingOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = d as UIElement;
			SelectiveScrollingOrientation selectiveScrollingOrientation = (SelectiveScrollingOrientation)e.NewValue;
			ScrollViewer scrollViewer = DataGridHelper.FindVisualParent<ScrollViewer>(uielement);
			if (scrollViewer != null && uielement != null)
			{
				Transform renderTransform = uielement.RenderTransform;
				if (renderTransform != null)
				{
					BindingOperations.ClearBinding(renderTransform, TranslateTransform.XProperty);
					BindingOperations.ClearBinding(renderTransform, TranslateTransform.YProperty);
				}
				if (selectiveScrollingOrientation == SelectiveScrollingOrientation.Both)
				{
					uielement.RenderTransform = null;
					return;
				}
				TranslateTransform translateTransform = new TranslateTransform();
				if (selectiveScrollingOrientation != SelectiveScrollingOrientation.Horizontal)
				{
					Binding binding = new Binding("ContentHorizontalOffset");
					binding.Source = scrollViewer;
					BindingOperations.SetBinding(translateTransform, TranslateTransform.XProperty, binding);
				}
				if (selectiveScrollingOrientation != SelectiveScrollingOrientation.Vertical)
				{
					Binding binding2 = new Binding("ContentVerticalOffset");
					binding2.Source = scrollViewer;
					BindingOperations.SetBinding(translateTransform, TranslateTransform.YProperty, binding2);
				}
				uielement.RenderTransform = translateTransform;
			}
		}

		// Token: 0x04003AD8 RID: 15064
		public static readonly DependencyProperty SelectiveScrollingOrientationProperty = DependencyProperty.RegisterAttached("SelectiveScrollingOrientation", typeof(SelectiveScrollingOrientation), typeof(SelectiveScrollingGrid), new FrameworkPropertyMetadata(SelectiveScrollingOrientation.Both, new PropertyChangedCallback(SelectiveScrollingGrid.OnSelectiveScrollingOrientationChanged)));
	}
}
